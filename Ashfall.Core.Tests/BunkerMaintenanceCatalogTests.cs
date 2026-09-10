using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BunkerMaintenanceCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void BunkerMaintenance_LoadsAll20CanonicalGlitches()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_maintenance_glitches.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerMaintenanceCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(20, catalog.AllGlitches.Count);

            // Test first glitch (Steam Fracture)
            var g1 = catalog.GetById("glitch_01_radiator_header_steam_fracture");
            Assert.NotNull(g1);
            Assert.Equal("ENG-FL-088-STEAM", g1.log_code);
            Assert.Equal(3, g1.severity_tier);
            Assert.Contains("superheated steam venting at 140°C", g1.anomaly_description);
            Assert.Contains("Valery burned his knuckles", g1.dmitri_shift_note);

            // Test critical emergencies (severity >= 4)
            var criticals = catalog.GetCriticalEmergencies(4);
            Assert.True(criticals.Count >= 7); // Hammer, Ground, Filter, Diesel, Door, CO2, Glycol, Ozone, Runaway, Mercury

            // Test subsystem query
            var boilerGlitches = catalog.GetBySubsystem("Boiler");
            Assert.True(boilerGlitches.Count >= 1);

            // Test finale log (The Century Seed Opening Lube)
            var g20 = catalog.GetById("glitch_20_the_century_seed_pneumatic_gate_final_lube");
            Assert.NotNull(g20);
            Assert.Equal("ENG-FL-3650-TRIUMPH", g20.log_code);
            Assert.Equal(1, g20.severity_tier);
            Assert.Contains("All maintenance tickets permanently closed", g20.dmitri_shift_note);

            // Test tag search
            var dmitri = catalog.GetByTag("dmitri");
            Assert.True(dmitri.Count >= 6);
        }

        [Fact]
        public void BunkerMaintenance_AllEntriesHaveValidFieldsAndUniqueLogCodes()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_maintenance_glitches.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerMaintenanceCatalog();
            catalog.Load(json, serializer);

            var seenCodes = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var g in catalog.AllGlitches)
            {
                Assert.False(string.IsNullOrWhiteSpace(g.glitch_id), "Missing glitch_id");
                Assert.False(string.IsNullOrWhiteSpace(g.log_code), $"Missing log_code on {g.glitch_id}");
                Assert.True(seenCodes.Add(g.log_code), $"Duplicate log code: {g.log_code}");

                Assert.False(string.IsNullOrWhiteSpace(g.affected_subsystem), $"Missing subsystem on {g.glitch_id}");
                Assert.InRange(g.severity_tier, 1, 5);
                Assert.False(string.IsNullOrWhiteSpace(g.anomaly_description), $"Missing anomaly on {g.glitch_id}");
                Assert.False(string.IsNullOrWhiteSpace(g.diagnostic_telemetry), $"Missing telemetry on {g.glitch_id}");
                Assert.NotNull(g.required_repair_kit);
                Assert.True(g.required_repair_kit.Length >= 3, $"Repair kit too small on {g.glitch_id}");
                Assert.False(string.IsNullOrWhiteSpace(g.emergency_protocol), $"Missing protocol on {g.glitch_id}");
                Assert.True(g.emergency_protocol.Length > 30, $"Protocol too brief on {g.glitch_id}");
                Assert.False(string.IsNullOrWhiteSpace(g.dmitri_shift_note), $"Missing shift note on {g.glitch_id}");
                Assert.True(g.dmitri_shift_note.Length > 25, $"Shift note too brief on {g.glitch_id}");
                Assert.NotNull(g.tags);
                Assert.True(g.tags.Length > 0, $"Tags empty on {g.glitch_id}");
            }
        }

        [Fact]
        public void BunkerMaintenance_Load_IsIdempotent()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_maintenance_glitches.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerMaintenanceCatalog();

            catalog.Load(json, serializer);
            Assert.Equal(20, catalog.AllGlitches.Count);

            // Re-loading same payload must not duplicate entries
            catalog.Load(json, serializer);
            Assert.Equal(20, catalog.AllGlitches.Count);
        }

        [Fact]
        public void BunkerMaintenance_LoadFromDirectory_ResolvesCanonicalFile()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(20, catalog.AllGlitches.Count);
            Assert.NotNull(catalog.GetById("glitch_01_radiator_header_steam_fracture"));
        }

        [Fact]
        public void BunkerMaintenance_GetByLogCode_ReturnsExactMatch()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var steam = catalog.GetByLogCode("ENG-FL-088-STEAM");
            Assert.NotNull(steam);
            Assert.Equal("glitch_01_radiator_header_steam_fracture", steam.glitch_id);

            var triumph = catalog.GetByLogCode("eng-fl-3650-triumph"); // case-insensitive
            Assert.NotNull(triumph);
            Assert.Equal("glitch_20_the_century_seed_pneumatic_gate_final_lube", triumph.glitch_id);

            Assert.Null(catalog.GetByLogCode("NON-EXISTENT-CODE"));
        }

        [Fact]
        public void BunkerMaintenance_GetBySeverity_ReturnsExactTiers()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var tier5 = catalog.GetBySeverity(5);
            Assert.Equal(3, tier5.Count); // Mold (04), CO2 (11), Runaway (18)
            foreach (var g in tier5)
            {
                Assert.Equal(5, g.severity_tier);
            }

            var tier1 = catalog.GetBySeverity(1);
            Assert.Single(tier1);
            Assert.Equal("glitch_20_the_century_seed_pneumatic_gate_final_lube", tier1[0].glitch_id);
        }

        [Fact]
        public void BunkerMaintenance_GetBySearch_FindsRelevantRecords()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var dmitriNotes = catalog.GetBySearch("Dmitri");
            Assert.NotEmpty(dmitriNotes);

            var mercuryMatches = catalog.GetBySearch("mercury");
            Assert.Contains(mercuryMatches, g => g.log_code == "ENG-FL-1030-MERCURY");

            var weldingMatches = catalog.GetBySearch("welding_rig");
            Assert.Contains(weldingMatches, g => g.glitch_id == "glitch_01_radiator_header_steam_fracture");

            Assert.Empty(catalog.GetBySearch("gibberish_term_xyz_12345"));
        }

        [Fact]
        public void BunkerMaintenance_Projection_MapsCanonicalRooms()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var validRooms = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "room_bunker_corridor", "room_water_pump", "room_workshop",
                "room_filtration", "room_greenhouse", "room_airlock",
                "room_storage_bay", "room_clinic", "room_kitchen",
                "room_radio_tuner", "room_main"
            };

            foreach (var g in catalog.AllGlitches)
            {
                string roomId = BunkerMaintenanceProjection.ResolveRoomForGlitch(g.glitch_id);
                Assert.True(validRooms.Contains(roomId), $"Glitch {g.glitch_id} mapped to invalid room {roomId}");
            }

            var filtrationGlitches = BunkerMaintenanceProjection.GetGlitchesForRoom(catalog, "room_filtration");
            Assert.True(filtrationGlitches.Count >= 4); // filter, boiler slag, diesel genset, co2 scrubber
        }

        [Fact]
        public void BunkerMaintenance_Projection_MapsSubsystemCategories()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            foreach (var g in catalog.AllGlitches)
            {
                var cat = BunkerMaintenanceProjection.ResolveCategory(g.glitch_id);
                Assert.NotEqual(SubsystemCategory.Other, cat);
            }

            var powerGlitches = BunkerMaintenanceProjection.GetGlitchesForCategory(catalog, SubsystemCategory.PowerAndElectrical);
            Assert.Equal(3, powerGlitches.Count); // ground loop (03), diesel knock (07), battery runaway (18)

            var powerMatches = BunkerMaintenanceProjection.MatchGlitchesForConditionKey(catalog, "power.battery_reserve");
            Assert.Equal(3, powerMatches.Count);
        }

        [Fact]
        public void BunkerMaintenance_Projection_ZeroMutationContract()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            int countBefore = catalog.AllGlitches.Count;
            var roomList = BunkerMaintenanceProjection.GetGlitchesForRoom(catalog, "room_filtration");
            var catList = BunkerMaintenanceProjection.GetGlitchesForCategory(catalog, SubsystemCategory.HeatingAndSteam);

            Assert.Equal(countBefore, catalog.AllGlitches.Count);
            Assert.NotEmpty(roomList);
            Assert.NotEmpty(catList);
        }

        [Fact]
        public void BunkerMaintenance_DeterministicOrdering()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var sortedDesc = catalog.GetSortedBySeverity(descending: true);
            Assert.Equal(20, sortedDesc.Count);
            for (int i = 0; i < sortedDesc.Count - 1; i++)
            {
                Assert.True(sortedDesc[i].severity_tier >= sortedDesc[i + 1].severity_tier);
                if (sortedDesc[i].severity_tier == sortedDesc[i + 1].severity_tier)
                {
                    Assert.True(string.Compare(sortedDesc[i].glitch_id, sortedDesc[i + 1].glitch_id, StringComparison.Ordinal) <= 0);
                }
            }
        }

        [Fact]
        public void BunkerMaintenance_Clear_ResetsCatalog()
        {
            string dataDir = FindDataDir();
            var catalog = BunkerMaintenanceCatalog.LoadFromDirectory(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(20, catalog.AllGlitches.Count);
            catalog.Clear();
            Assert.Empty(catalog.AllGlitches);
            Assert.Null(catalog.GetById("glitch_01_radiator_header_steam_fracture"));
        }
    }
}
