// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Xunit;

namespace Ashfall.Core.Tests.Plan123SoundRanging
{
    /// <summary>
    /// Plan 123 Phase 2 — sound_ranging_catalog.json characterization:
    /// schema, duplicate-ID rejection, ranges, internal foreign keys, and a
    /// hard schema invariant: the catalog carries NO weapon-targeting output
    /// fields. Engine lands in Phase 5.
    /// </summary>
    public sealed class Plan123SoundRangingCatalogTests
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

        private sealed class RealFileIo : IFileIO
        {
            public bool DirectoryExists(string path) => Directory.Exists(path);
            public bool FileExists(string path) => File.Exists(path);
            public string ReadAllText(string path) => File.ReadAllText(path);
            public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
            public string Combine(params string[] parts) => Path.Combine(parts);
        }

        private static SoundRangingCatalog LoadReal()
        {
            var catalog = SoundRangingCatalogLoader.Load(GetDataDir(), new RealFileIo());
            Assert.True(catalog._arrays_nonempty_for_test(), "sound_ranging_catalog.json must load with array profiles");
            return catalog;
        }

        // TEST-AGGREGATION: source_rows=6 rule families; failures reported per row.
        [Fact]
        public void Real_catalog_passes_structural_validation()
        {
            var catalog = LoadReal();
            Assert.True(catalog.ValidateCatalog(out string error), $"catalog invalid: {error}");
        }

        [Fact]
        public void Array_rows_have_coarse_resolution_and_bounded_confidence()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var array in catalog.array_profiles)
            {
                // Defensive design gate: bearing error must stay coarse (>= 2 deg)
                // and region radius >= 1 cell — never weapon-grade precision.
                if (array.base_bearing_error_deg < 2f) failures.Add($"{array.id}: bearing error {array.base_bearing_error_deg} too precise for defensive output");
                if (array.base_region_radius_cells < 1) failures.Add($"{array.id}: region radius < 1 cell");
                if (array.confidence_cap_bp > 10000) failures.Add($"{array.id}: confidence cap exceeds bp scale");
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Array_foreign_keys_resolve()
        {
            var catalog = LoadReal();
            var failures = new List<string>();
            foreach (var array in catalog.array_profiles)
            {
                if (catalog.GetSensor(array.sensor_profile_id) == null) failures.Add($"{array.id}: sensor_profile_id unresolved");
                if (catalog.GetMaintenance(array.maintenance_profile_id) == null) failures.Add($"{array.id}: maintenance_profile_id unresolved");
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Catalog_schema_carries_no_targeting_fields()
        {
            // Hard invariant (plan §5.15 case 18): the catalog JSON must not
            // contain weapon-targeting schema. If someone adds a targeting
            // field later, this test fails and the design review re-opens.
            var text = File.ReadAllText(Path.Combine(GetDataDir(), SoundRangingCatalogLoader.DefaultFileName));
            foreach (var forbidden in new[] { "target_cell", "target_coordinate", "firing_solution",
                     "counter_battery", "strike_solution", "target_x", "target_y", "aim_point" })
            {
                Assert.False(text.Contains(forbidden, StringComparison.Ordinal),
                    $"sound_ranging_catalog.json must not carry '{forbidden}' (defensive-only schema)");
            }
        }

        [Fact]
        public void Duplicate_ids_are_rejected()
        {
            var catalog = new SoundRangingCatalog
            {
                array_profiles =
                {
                    new SoundRangingArrayProfile { id = "dup_array", sensor_profile_id = "s", sensor_count = 4, base_bearing_error_deg = 8, base_region_radius_cells = 3, timing_quality = 0.75f, weather_sensitivity = 0.45f, confidence_cap_bp = 9000 },
                    new SoundRangingArrayProfile { id = "dup_array", sensor_profile_id = "s", sensor_count = 4, base_bearing_error_deg = 8, base_region_radius_cells = 3, timing_quality = 0.75f, weather_sensitivity = 0.45f, confidence_cap_bp = 9000 }
                },
                sensor_profiles = { new SoundRangingSensorProfile { id = "s", reliability_bp = 8000 } }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("duplicate array id 'dup_array'", error);
        }

        [Fact]
        public void Unresolved_sensor_fk_is_rejected()
        {
            var catalog = new SoundRangingCatalog
            {
                array_profiles =
                {
                    new SoundRangingArrayProfile { id = "a1", sensor_profile_id = "missing", sensor_count = 4, base_bearing_error_deg = 8, base_region_radius_cells = 3, timing_quality = 0.75f, weather_sensitivity = 0.45f, confidence_cap_bp = 9000 }
                }
            };
            catalog.Index();
            Assert.False(catalog.ValidateCatalog(out string error));
            Assert.Contains("missing", error);
        }

        [Fact]
        public void Load_is_deterministic()
        {
            var dataDir = GetDataDir();
            var a = SoundRangingCatalogLoader.Load(dataDir, new RealFileIo());
            var b = SoundRangingCatalogLoader.Load(dataDir, new RealFileIo());
            Assert.Equal(a.array_profiles.Count, b.array_profiles.Count);
            foreach (var (x, y) in a.array_profiles.Zip(b.array_profiles, (x, y) => (x, y)))
                Assert.Equal((x.id, x.base_bearing_error_deg, x.sensor_count),
                             (y.id, y.base_bearing_error_deg, y.sensor_count));
        }

        [Fact]
        public void Missing_file_loads_empty_without_throwing()
        {
            var catalog = SoundRangingCatalogLoader.Load(Path.Combine(Path.GetTempPath(), "no_such_dir_123"), new RealFileIo());
            Assert.Empty(catalog._arrays_for_test());
        }
    }

    internal static class SoundRangingCatalogTestAccess
    {
        public static bool _arrays_nonempty_for_test(this SoundRangingCatalog c) => c.array_profiles.Count > 0;
        public static System.Collections.Generic.IReadOnlyCollection<SoundRangingArrayProfile> _arrays_for_test(this SoundRangingCatalog c)
            => c.array_profiles;
    }
}
