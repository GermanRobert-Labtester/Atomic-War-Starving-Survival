using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class GeologicalStrataCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void GeologicalStrata_LoadsAll24CanonicalStrata()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "geological_strata_logs.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new GeologicalStrataCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(24, catalog.AllStrata.Count);

            // Test surface loess
            var s1 = catalog.GetById("strata_01_surface_weathered_loess_topsoil");
            Assert.NotNull(s1);
            Assert.Equal(0.0f, s1.depth_start_meters);
            Assert.Equal(3.5f, s1.depth_end_meters);
            Assert.Equal(18000.0f, s1.radon_emission_bq_m3);
            Assert.Contains("positive-pressure air hoods", s1.geologist_field_notes);

            // Test depth-based lookup (e.g. -50m depth should hit limestone/karst)
            var stratum50 = catalog.GetByDepth(-50.0f);
            Assert.NotNull(stratum50);
            Assert.Equal("strata_07_vuggy_karst_cavern_horizon", stratum50.strata_id);

            // Test radon hot zones
            var hotZones = catalog.GetHighRadonHazards(5000.0f);
            Assert.True(hotZones.Count >= 2); // Loess, Fault Gouge, Uraniferous Black Slate

            // Test deepest stratum (-350m Granulite)
            var deep = catalog.GetById("strata_24_hypersthene_granulite_core_roots");
            Assert.NotNull(deep);
            Assert.Equal(350.0f, deep.depth_end_meters);
            Assert.Equal(310.0f, deep.compressive_strength_mpa);
            Assert.Contains("two billion years of unbroken stone", deep.geologist_field_notes);
        }

        [Fact]
        public void GeologicalStrata_AllEntriesHaveContiguousDepthAndValidEngineeringData()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "geological_strata_logs.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new GeologicalStrataCatalog();
            catalog.Load(json, serializer);

            float lastEnd = 0.0f;
            foreach (var s in catalog.AllStrata)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.strata_id), "Missing strata_id");
                Assert.False(string.IsNullOrWhiteSpace(s.strata_name), $"Missing strata_name on {s.strata_id}");
                Assert.Equal(lastEnd, s.depth_start_meters);
                Assert.True(s.depth_end_meters > s.depth_start_meters, $"End depth <= start depth on {s.strata_id}");
                lastEnd = s.depth_end_meters;

                Assert.False(string.IsNullOrWhiteSpace(s.rock_type), $"Missing rock_type on {s.strata_id}");
                Assert.True(s.compressive_strength_mpa > 0f, $"Invalid compressive MPa on {s.strata_id}");
                Assert.True(s.radon_emission_bq_m3 >= 0f, $"Invalid radon Bq on {s.strata_id}");
                Assert.True(s.fault_shear_stress_kpa > 0f, $"Invalid shear kPa on {s.strata_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.mining_hazard_type), $"Missing mining hazard on {s.strata_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.geologist_field_notes), $"Missing field notes on {s.strata_id}");
                Assert.True(s.geologist_field_notes.Length > 30, $"Field notes too brief on {s.strata_id}");
                Assert.NotNull(s.tags);
                Assert.True(s.tags.Length > 0, $"Tags empty on {s.strata_id}");
            }
        }
    }
}
