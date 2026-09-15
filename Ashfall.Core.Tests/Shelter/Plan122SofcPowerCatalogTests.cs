// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan122Sofc
{
    /// <summary>
    /// Plan 122 Phase 2 — sofc_power_catalog.json characterization: schema,
    /// duplicate-ID rejection, numeric ranges, ordered thermal bands, closed
    /// class vocabularies, internal foreign keys, items.json foreign keys,
    /// and deterministic load behavior. Engine behavior lands in Phase 3.
    /// </summary>
    public sealed class Plan122SofcPowerCatalogTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static SofcPowerCatalog LoadReal()
        {
            var catalog = SofcPowerCatalogLoader.Load(GetDataDir(), new RealFileIo());
            Assert.True(catalog.Stacks.Count > 0, "sofc_power_catalog.json must load with stack profiles");
            return catalog;
        }

        private sealed class RealFileIo : IFileIO
        {
            public bool DirectoryExists(string path) => Directory.Exists(path);
            public bool FileExists(string path) => File.Exists(path);
            public string ReadAllText(string path) => File.ReadAllText(path);
            public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
            public string Combine(params string[] parts) => Path.Combine(parts);
        }

        // TEST-AGGREGATION: source_rows=4 catalogs x 5 rule families; each
        // family reports all failing rows, not just the first.
        [Fact]
        public void Real_catalog_passes_structural_validation()
        {
            var catalog = LoadReal();
            Assert.True(catalog.ValidateCatalog(out string error), $"catalog invalid: {error}");
        }

        [Fact]
        public void Every_stack_row_has_valid_ranges_and_band()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var stack in catalog.stack_profiles)
            {
                if (stack.rated_power_kw <= 0) failures.Add($"{stack.id}: rated_power_kw <= 0");
                if (stack.fuel_efficiency is <= 0f or >= 1f) failures.Add($"{stack.id}: fuel_efficiency not in (0,1)");
                if (stack.startup_ticks <= 0) failures.Add($"{stack.id}: startup_ticks <= 0 (no instant start)");
                if (stack.degradation_per_operating_tick_bp < 0) failures.Add($"{stack.id}: negative degradation");
                string bandError = string.Empty;
                if (stack.thermal_band == null || !stack.thermal_band.Validate(out bandError))
                    failures.Add($"{stack.id}: thermal_band invalid ({bandError})");
                if (stack.acoustic_signature_class != SofcPowerCatalog.SignatureVeryLow
                    && stack.acoustic_signature_class != SofcPowerCatalog.SignatureLow)
                    failures.Add($"{stack.id}: SOFC must be low/very_low signature, got '{stack.acoustic_signature_class}'");
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Every_stack_foreign_key_resolves()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var stack in catalog.stack_profiles)
            {
                if (catalog.GetFuel(stack.fuel_profile_id) == null) failures.Add($"{stack.id}: fuel_profile_id '{stack.fuel_profile_id}' unresolved");
                if (catalog.GetGrade(stack.grade_profile_id) == null) failures.Add($"{stack.id}: grade_profile_id '{stack.grade_profile_id}' unresolved");
                if (catalog.GetWasteHeat(stack.waste_heat_profile_id) == null) failures.Add($"{stack.id}: waste_heat_profile_id unresolved");
                if (catalog.GetMaintenance(stack.maintenance_profile_id) == null) failures.Add($"{stack.id}: maintenance_profile_id unresolved");
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Every_item_reference_exists_in_items_json()
        {
            var catalog = LoadReal();
            var dataDir = GetDataDir();
            var itemIds = new HashSet<string>(
                System.Text.RegularExpressions.Regex.Matches(
                        File.ReadAllText(Path.Combine(dataDir, "items.json")),
                        "\"id\"\\s*:\\s*\"(item_[a-z0-9_]+)\"")
                    .Select(m => m.Groups[1].Value),
                StringComparer.Ordinal);
            Assert.True(itemIds.Count > 0, "items.json produced no ids");

            var failures = new List<string>();
            foreach (var stack in catalog.stack_profiles)
            {
                foreach (var itemId in stack.install_item_ids.Concat(stack.repair_item_ids))
                    if (!itemIds.Contains(itemId)) failures.Add($"stack {stack.id}: item '{itemId}' not in items.json");
            }
            foreach (var fuel in catalog.fuel_profiles)
                foreach (var itemId in fuel.feedstock_item_ids)
                    if (!itemIds.Contains(itemId)) failures.Add($"fuel {fuel.id}: item '{itemId}' not in items.json");
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Duplicate_ids_are_rejected()
        {
            var catalog = new SofcPowerCatalog
            {
                stack_profiles =
                {
                    new SofcStackProfile { id = "dup_stack", fuel_profile_id = "f", rated_power_kw = 10, fuel_efficiency = 0.5f, startup_ticks = 1, cooldown_ticks = 1, thermal_band = new SofcThermalBand { minimum = 0.1f, optimal_min = 0.2f, optimal_max = 0.3f, maximum = 0.4f }, acoustic_signature_class = SofcPowerCatalog.SignatureLow },
                    new SofcStackProfile { id = "dup_stack", fuel_profile_id = "f", rated_power_kw = 10, fuel_efficiency = 0.5f, startup_ticks = 1, cooldown_ticks = 1, thermal_band = new SofcThermalBand { minimum = 0.1f, optimal_min = 0.2f, optimal_max = 0.3f, maximum = 0.4f }, acoustic_signature_class = SofcPowerCatalog.SignatureLow }
                },
                fuel_profiles = { new SofcFuelProfile { id = "f", quality_class = SofcPowerCatalog.QualityClean, output_modifier_bp = 10000, degradation_multiplier_bp = 100 } }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("duplicate stack id 'dup_stack'", error);
        }

        [Fact]
        public void Unresolved_internal_fk_is_rejected()
        {
            var catalog = new SofcPowerCatalog
            {
                stack_profiles =
                {
                    new SofcStackProfile { id = "s1", fuel_profile_id = "missing_fuel", rated_power_kw = 10, fuel_efficiency = 0.5f, startup_ticks = 1, cooldown_ticks = 1, thermal_band = new SofcThermalBand { minimum = 0.1f, optimal_min = 0.2f, optimal_max = 0.3f, maximum = 0.4f }, acoustic_signature_class = SofcPowerCatalog.SignatureLow }
                }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("missing_fuel", error);
        }

        [Fact]
        public void Load_is_deterministic()
        {
            var dataDir = GetDataDir();
            var a = SofcPowerCatalogLoader.Load(dataDir, new RealFileIo());
            var b = SofcPowerCatalogLoader.Load(dataDir, new RealFileIo());
            Assert.Equal(a.stack_profiles.Count, b.stack_profiles.Count);
            Assert.Equal(a.fuel_profiles.Count, b.fuel_profiles.Count);
            foreach (var (x, y) in a.stack_profiles.Zip(b.stack_profiles, (x, y) => (x, y)))
            {
                Assert.Equal(x.id, y.id);
                Assert.Equal(x.rated_power_kw, y.rated_power_kw);
                Assert.Equal(x.fuel_efficiency, y.fuel_efficiency);
                Assert.Equal(x.startup_ticks, y.startup_ticks);
            }
        }

        [Fact]
        public void Missing_file_loads_empty_without_throwing()
        {
            var catalog = SofcPowerCatalogLoader.Load(Path.Combine(Path.GetTempPath(), "no_such_dir_122"), new RealFileIo());
            Assert.Empty(catalog.Stacks);
        }
    }
}
