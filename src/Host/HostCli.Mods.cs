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
                var report = CatalogIntegrityValidator.Validate(staged, new FileSystemIO());
                checks.Add(report.Errors.Count == 0 ? "integrity pass" : "integrity fail");
                bool pass = checks.TrueForAll(value => value.EndsWith("accepted", StringComparison.Ordinal)
                    || value.EndsWith("isolated", StringComparison.Ordinal)
                    || value.EndsWith("present", StringComparison.Ordinal)
                    || value.EndsWith("pass", StringComparison.Ordinal));

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
