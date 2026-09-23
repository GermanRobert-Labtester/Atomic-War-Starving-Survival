// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Mods;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunModSelfTest()
        {
            string root = Path.Combine(Path.GetTempPath(), "ashfall-mod-selftest");
            try
            {
                if (Directory.Exists(root))
                    Directory.Delete(root, recursive: true);
                string data = Path.Combine(root, "Data");
                string mods = Path.Combine(root, "Mods");
                Directory.CreateDirectory(data);
                Directory.CreateDirectory(mods);
                File.WriteAllText(
                    Path.Combine(data, "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"item_base\",\"displayName\":\"Base\"}]}");

                WriteMod(mods, "sample_supply", 0, false, "items.json", "item_sample", "Sample");
                WriteMod(mods, "unsafe_path", 1, false, "../items.json", "item_unsafe", "Ignored");
                File.WriteAllText(
                    Path.Combine(mods, "unsafe_path", "items.json"),
                    "{\"schema_version\":1,\"items\":[{\"id\":\"item_unsafe\"}]}");

                var result = new JsonModLayering().Build(
                    data,
                    mods,
                    new FileSystemIO(),
                    new SystemTextJsonSerializer());
                foreach (ModDiagnostic diagnostic in result.Diagnostics)
                    Godot.GD.Print($"[ModSelfTest] diagnostic — {diagnostic}");
                var checks = new List<string>
                {
                    result.AcceptedModIds.Count == 1 ? "valid manifest accepted" : "valid manifest rejected",
                    result.RejectedModIds.Contains("unsafe_path") ? "unsafe catalog isolated" : "unsafe catalog not isolated",
                    result.CatalogOverlays.TryGetValue("items.json", out string? merged)
                        && merged.Contains("\"id\":\"item_sample\"", StringComparison.Ordinal)
                        ? "layered item present"
                        : "layered item missing"
                };

                string staged = Path.Combine(root, "Staged");
                Directory.CreateDirectory(staged);
                File.WriteAllText(Path.Combine(staged, "items.json"), merged ?? string.Empty);

                // Scoped overlay verification — this probe owns MOD LAYERING, not
                // whole-data-authority integrity. Running the full
                // CatalogIntegrityValidator over a single-catalog temp directory
                // could never pass: the validator cross-references every catalog
                // in the authority, so a staged dir containing only items.json
                // reports missing-relationship errors by construction. Full
                // authority integrity is --data-integrity-selftest's job, and it
                // runs over the real shipped data authority.
                //
                // What IS in scope here: the merged document is well-formed,
                // declares its schema version, and carries BOTH the base item and
                // the layered item with the overlay applied.
                checks.Add(VerifyStagedOverlay(staged));
                bool pass = checks.TrueForAll(value => value.EndsWith("accepted", StringComparison.Ordinal)
                    || value.EndsWith("isolated", StringComparison.Ordinal)
                    || value.EndsWith("present", StringComparison.Ordinal)
                    || value.EndsWith("verified", StringComparison.Ordinal));

                foreach (string check in checks)
                    Godot.GD.Print($"[ModSelfTest] {(check.EndsWith("fail", StringComparison.Ordinal) ? "FAIL" : "PASS")} — {check}");
                Godot.GD.Print($"[ModSelfTest] {(pass ? "PASS" : "FAIL")} — {checks.Count} checks");
                return pass ? 0 : 1;
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr("[ModSelfTest] FAIL — " + ex.Message);
                return 1;
            }
            finally
            {
                TryDeleteTempDirectory(root);
            }
        }

        /// <summary>
        /// Scoped verification of one staged overlay document: well-formed JSON,
        /// an explicit schema version, the base item, and the layered item.
        /// Deliberately NOT a whole-authority integrity pass (see the caller).
        /// </summary>
        private static string VerifyStagedOverlay(string stagedDirectory)
        {
            string path = Path.Combine(stagedDirectory, "items.json");
            string json = File.ReadAllText(path);

            bool wellFormed;
            string failure;
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                bool hasSchema = doc.RootElement.TryGetProperty("schema_version", out var schema)
                    && schema.ValueKind == System.Text.Json.JsonValueKind.Number;
                bool hasBase = json.Contains("\"id\":\"item_base\"", StringComparison.Ordinal);
                bool hasLayered = json.Contains("\"id\":\"item_sample\"", StringComparison.Ordinal);

                if (!hasSchema) { wellFormed = false; failure = "merged document declares no schema_version"; }
                else if (!hasBase) { wellFormed = false; failure = "merged document lost the base item"; }
                else if (!hasLayered) { wellFormed = false; failure = "merged document lost the layered item"; }
                else { wellFormed = true; failure = string.Empty; }
            }
            catch (Exception ex)
            {
                wellFormed = false;
                failure = "merged document is not well-formed: " + ex.Message;
            }

            return wellFormed
                ? "base + layered item present and schema_version declared — staged overlay verified"
                : failure + " — staged overlay fail";
        }

        private static void WriteMod(
            string mods,
            string id,
            int loadOrder,
            bool allowOverrides,
            string catalog,
            string definitionId,
            string? displayName = null)
        {
            string directory = Path.Combine(mods, id);
            Directory.CreateDirectory(directory);
            File.WriteAllText(
                Path.Combine(directory, "manifest.json"),
                $"{{\"schema_version\":1,\"mod_id\":\"{id}\",\"load_order\":{loadOrder},\"allow_overrides\":{(allowOverrides ? "true" : "false")},\"catalogs\":[\"{catalog}\"]}}");
            if (!catalog.Contains("..", StringComparison.Ordinal))
            {
                File.WriteAllText(
                    Path.Combine(directory, catalog),
                    $"{{\"schema_version\":1,\"items\":[{{\"id\":\"{definitionId}\",\"displayName\":\"{displayName ?? definitionId}\"}}]}}");
            }
        }
    }
}
