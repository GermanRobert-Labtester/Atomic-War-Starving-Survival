using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BunkerBlueprintCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void BunkerBlueprints_LoadsAll24CanonicalRoomSchematics()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_blueprints_codex.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerBlueprintCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(24, catalog.AllBlueprints.Count);

            // Test first blueprint (Airlock)
            var airlock = catalog.GetById("room_bp_01_surface_airlock_vestibule");
            Assert.NotNull(airlock);
            Assert.Equal("Security & Perimeter", airlock.category);
            Assert.Equal(0.0f, airlock.optimal_depth_meters);
            Assert.Equal(6, airlock.max_dweller_capacity);
            Assert.Contains("lead-bismuth sandwich doors", airlock.structural_header_spec);
            Assert.Contains("weighs three tons", airlock.chief_engineer_note);

            // Test generator producer
            var gen = catalog.GetById("room_bp_02_diesel_generator_vault");
            Assert.NotNull(gen);
            Assert.True(gen.base_power_draw_kw < 0f);

            var producers = catalog.GetPowerProducers();
            Assert.NotEmpty(producers);

            // Test final blueprint (Sonya's Tree Atrium)
            var finale = catalog.GetById("room_bp_24_the_century_seed_sunken_atrium");
            Assert.NotNull(finale);
            Assert.Equal(50, finale.max_dweller_capacity);
            Assert.Contains("Sonya's apple tree", finale.chief_engineer_note);

            // Test category search
            var medical = catalog.GetByCategory("Medic");
            Assert.True(medical.Count >= 2);

            var water = catalog.GetByCategory("Water");
            Assert.True(water.Count >= 2);
        }

        [Fact]
        public void BunkerBlueprints_AllEntriesHaveValidStructuralAndFailureSpecs()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_blueprints_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerBlueprintCatalog();
            catalog.Load(json, serializer);

            foreach (var bp in catalog.AllBlueprints)
            {
                Assert.False(string.IsNullOrWhiteSpace(bp.room_id), "Missing room_id");
                Assert.False(string.IsNullOrWhiteSpace(bp.room_name), $"Missing room_name on {bp.room_id}");
                Assert.False(string.IsNullOrWhiteSpace(bp.category), $"Missing category on {bp.room_id}");
                Assert.True(bp.max_dweller_capacity > 0, $"Invalid capacity on {bp.room_id}");
                Assert.True(bp.thermal_r_value > 0f, $"Invalid R-value on {bp.room_id}");
                Assert.True(bp.acoustic_noise_db > 0f, $"Invalid dB on {bp.room_id}");
                Assert.True(bp.maintenance_cycle_days > 0, $"Invalid maintenance cycle on {bp.room_id}");
                Assert.False(string.IsNullOrWhiteSpace(bp.structural_header_spec), $"Missing structural spec on {bp.room_id}");
                Assert.False(string.IsNullOrWhiteSpace(bp.catastrophic_failure_mode), $"Missing failure mode on {bp.room_id}");
                Assert.False(string.IsNullOrWhiteSpace(bp.chief_engineer_note), $"Missing engineer note on {bp.room_id}");
                Assert.True(bp.chief_engineer_note.Length > 30, $"Engineer note too brief on {bp.room_id}");
                Assert.NotNull(bp.tags);
                Assert.True(bp.tags.Length > 0, $"Tags empty on {bp.room_id}");
            }
        }
    }
}
