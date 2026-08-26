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
    }
}
