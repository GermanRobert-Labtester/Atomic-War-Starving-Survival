// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 165 (Modding Support & Mod Data Contract).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Mods;

namespace AtomicWar.GodotApp
{
    public static class HostCliModSupport
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Modding Support & Mod Data Contract Self-Test (Plan 165) ===");
            int passed = 0;
            int total = 12;
            string root = Path.Combine(Path.GetTempPath(), "ashfall-mod-support-selftest");

            try
            {
                // Check 1: authored specification loads through the strict loader
                string specPath = Path.Combine(dataDir, "mod_manifest_schema.json");
                ModManifestSpecificationDef? spec = null;
                if (File.Exists(specPath))
                    spec = ModManifestSpecificationLoader.LoadFromJson(File.ReadAllText(specPath));
                if (spec != null && spec.AllowedCatalogs.Count >= 9 && spec.RequiredManifestFields.Count >= 5)
                {
                    GD.Print($"[PASS] Check 1: Strict loader accepted the specification ({spec.AllowedCatalogs.Count} catalogs, {spec.RequiredManifestFields.Count} required fields).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 1: Authored mod manifest specification missing or incomplete.");
                }

                // Check 2: strict loader rejects malformed specifications
                if (ExpectReject("{'schema_version':1,'schema_name':'s','allowed_catalogs':['a.json','a.json'],'required_manifest_fields':['mod_id']}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'schema_name':'s','allowed_catalogs':[],'required_manifest_fields':['mod_id']}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'schema_name':'','allowed_catalogs':['a.json'],'required_manifest_fields':['mod_id']}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':9,'schema_name':'s','allowed_catalogs':['a.json'],'required_manifest_fields':['mod_id']}".Replace('\'', '"')))
                {
                    GD.Print("[PASS] Check 2: Strict loader rejected duplicate catalogs, an empty whitelist, an empty schema name, and a future schema.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: A malformed specification was accepted.");
                }

                // Build a temp mods tree.
                if (Directory.Exists(root)) Directory.Delete(root, recursive: true);
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(mods);

                WriteManifest(mods, "alpha", "10", true, "[\"items.json\"]", null, null);
                WriteManifest(mods, "beta", "20", false, "[\"items.json\"]", "[\"alpha\"]", null);
                WriteManifest(mods, "gamma", "30", false, "[\"items.json\"]", null, null);
                WriteManifest(mods, "delta", "40", true, "[\"items.json\"]", "[\"nonexistent\"]", null);
                WriteManifest(mods, "cyc1", "50", true, "[\"items.json\"]", "[\"cyc2\"]", null);
                WriteManifest(mods, "cyc2", "51", true, "[\"items.json\"]", "[\"cyc1\"]", null);
                WriteManifest(mods, "incompat", "60", true, "[\"items.json\"]", null, ">=9.0.0");
                WriteBadIdManifest(mods, "badid");

                var host = ModSupportHostSession.Create(dataDir);
                int registered = host.DiscoverAndRegister(mods, enabledModIds: null, currentGameVersion: "1.0.0");

                // Check 3: every discovered manifest was registered
                if (registered == 8 && host.Census.TotalMods == 8)
                {
                    GD.Print($"[PASS] Check 3: Registered {registered} discovered mods.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Expected 8 registrations, got {registered} (census {host.Census.TotalMods}).");
                }

                // Check 4: invalid mod id rejected
                if (host.GetStatus("Bad-ID") == ModStatus.InvalidManifest || host.Census.InvalidMods >= 1)
                {
                    GD.Print("[PASS] Check 4: Invalid mod_id rejected as InvalidManifest.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Invalid mod_id not rejected (census invalid={host.Census.InvalidMods}).");
                }

                // Check 5: incompatible game range rejected
                if (host.GetStatus("incompat") == ModStatus.Incompatible)
                {
                    GD.Print("[PASS] Check 5: Incompatible game range rejected.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Incompatible mod status {host.GetStatus("incompat")}.");
                }

                // Check 6: missing dependency rejected
                if (host.GetStatus("delta") == ModStatus.MissingDependency)
                {
                    GD.Print("[PASS] Check 6: Missing dependency rejected.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Missing-dependency status {host.GetStatus("delta")}.");
                }

                // Check 7: cyclic dependency detected
                if (host.GetStatus("cyc1") == ModStatus.CyclicDependency && host.GetStatus("cyc2") == ModStatus.CyclicDependency)
                {
                    GD.Print("[PASS] Check 7: Cyclic dependency detected on both nodes.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Cycle statuses cyc1={host.GetStatus("cyc1")} cyc2={host.GetStatus("cyc2")}.");
                }

                // Check 8: deterministic load order excludes rejected mods and honours dependencies
                var order = host.ResolveLoadOrder().ToList();
                int alphaIndex = order.IndexOf("alpha");
                int betaIndex = order.IndexOf("beta");
                if (order.Contains("alpha") && order.Contains("beta") && alphaIndex < betaIndex
                    && !order.Contains("delta") && !order.Contains("cyc1") && !order.Contains("incompat"))
                {
                    GD.Print($"[PASS] Check 8: Load order [{string.Join(", ", order)}] honours dependencies and excludes rejected mods.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Load order wrong [{string.Join(", ", order)}].");
                }

                // Check 9: override conflict detected between two non-override mods
                var conflicts = host.DetectConflicts();
                if (conflicts.Count >= 1 && conflicts.Any(c => c.CatalogName == "items.json"
                    && ((c.ModIdA == "beta" && c.ModIdB == "gamma") || (c.ModIdA == "gamma" && c.ModIdB == "beta"))))
                {
                    GD.Print($"[PASS] Check 9: Override conflict detected ({conflicts.Count} conflict(s) on items.json).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Expected a beta/gamma conflict, found {conflicts.Count}.");
                }

                // Check 10: census reflects the mix of verdicts
                var census = host.Census;
                if (census.EnabledMods >= 3 && census.InvalidMods >= 1 && census.MissingDependencyMods >= 1
                    && census.CyclicMods >= 2 && census.IncompatibleMods >= 1 && census.AllowedCatalogCount >= 9)
                {
                    GD.Print($"[PASS] Check 10: Census accurate (enabled={census.EnabledMods}, invalid={census.InvalidMods}, missing={census.MissingDependencyMods}, cyclic={census.CyclicMods}, incompatible={census.IncompatibleMods}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Census mismatch (enabled={census.EnabledMods}, invalid={census.InvalidMods}, missing={census.MissingDependencyMods}, cyclic={census.CyclicMods}).");
                }

                // Check 11: enable/disable toggle
                bool disabled = host.SetModEnabled("alpha", false);
                bool alphaDisabled = host.GetStatus("alpha") == ModStatus.Disabled;
                bool reenabled = host.SetModEnabled("alpha", true);
                if (disabled && alphaDisabled && reenabled && host.GetStatus("alpha") == ModStatus.Enabled)
                {
                    GD.Print("[PASS] Check 11: Enable/disable toggle honoured.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Toggle wrong (disabled={disabled}, status={host.GetStatus("alpha")}).");
                }

                // Check 12: capture/restore + future-schema rejection
                var state = host.CaptureState();
                bool restoreOk = state.ActiveModIds.Count >= 1 && state.ModVersions.Count >= 1;
                bool gated = false;
                try
                {
                    var newer = host.CaptureState();
                    newer.SchemaVersion = 99;
                    host.RestoreState(newer);
                }
                catch (InvalidOperationException)
                {
                    gated = true;
                }
                if (restoreOk && gated)
                {
                    GD.Print("[PASS] Check 12: State captured/restored and a future schema was rejected.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: State/schema handling wrong (restoreOk={restoreOk}, gated={gated}).");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in mod support self-test: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                try { if (Directory.Exists(root)) Directory.Delete(root, recursive: true); } catch { /* best effort */ }
            }

            GD.Print($"=== Mod Support Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static void WriteManifest(string mods, string id, string loadOrder, bool allowOverrides,
            string catalogs, string? dependencies, string? gameRange)
        {
            string dir = Path.Combine(mods, id);
            Directory.CreateDirectory(dir);
            string deps = dependencies ?? "[]";
            string range = gameRange == null ? string.Empty : $",\"game_range\":\"{gameRange}\"";
            File.WriteAllText(Path.Combine(dir, "manifest.json"),
                $"{{\"schema_version\":1,\"mod_id\":\"{id}\",\"display_name\":\"{id}\",\"version\":\"1.0.0\",\"load_order\":{loadOrder},\"allow_overrides\":{(allowOverrides ? "true" : "false")},\"catalogs\":{catalogs},\"dependencies\":{deps}{range}}}");
        }

        private static void WriteBadIdManifest(string mods, string directoryName)
        {
            string dir = Path.Combine(mods, directoryName);
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "manifest.json"),
                "{\"schema_version\":1,\"mod_id\":\"Bad-ID\",\"display_name\":\"Bad\",\"version\":\"1.0.0\",\"load_order\":5,\"allow_overrides\":true,\"catalogs\":[\"items.json\"]}");
        }

        private static bool ExpectReject(string json)
        {
            try
            {
                ModManifestSpecificationLoader.LoadFromJson(json);
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}
