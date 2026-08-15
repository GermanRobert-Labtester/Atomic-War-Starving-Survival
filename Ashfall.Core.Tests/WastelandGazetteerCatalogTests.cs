using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class WastelandGazetteerCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void WastelandGazetteer_LoadsAll20CanonicalSettlements()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_settlement_gazetteer.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new WastelandGazetteerCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(20, catalog.AllSettlements.Count);

            // Test first settlement (Fort Karkov)
            var s1 = catalog.GetById("settlement_01_fort_karkov_rail_garrison");
            Assert.NotNull(s1);
            Assert.Equal("The Iron Gantry", s1.colloquial_name);
            Assert.Equal(140, s1.estimated_population);
            Assert.Equal("faction_the_garrison", s1.controlling_faction);
            Assert.Contains("Major Volkov", s1.harlan_scout_survey);

            // Test major hubs (population >= 100)
            var majorHubs = catalog.GetMajorHubs(100);
            Assert.True(majorHubs.Count >= 5); // Garrison, Commune, Slag City, Crossroads, Metro, Republic

            // Test final settlement (The Free Republic of Tessarat)
            var s20 = catalog.GetById("settlement_20_the_valley_sunken_atrium_republic");
            Assert.NotNull(s20);
            Assert.Equal(350, s20.estimated_population);
            Assert.Equal(0.00f, s20.water_source_radiation_mrh);
            Assert.Contains("Sonya's apple tree is in full bloom", s20.harlan_scout_survey);

            // Test tag search
            var trade = catalog.GetByTag("trade");
            Assert.True(trade.Count >= 3);
        }

        [Fact]
        public void WastelandGazetteer_AllEntriesHaveValidFieldsAndCoordinates()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_settlement_gazetteer.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new WastelandGazetteerCatalog();
            catalog.Load(json, serializer);

            foreach (var s in catalog.AllSettlements)
            {
                Assert.False(string.IsNullOrWhiteSpace(s.settlement_id), "Missing settlement_id");
                Assert.False(string.IsNullOrWhiteSpace(s.settlement_name), $"Missing settlement_name on {s.settlement_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.colloquial_name), $"Missing colloquial_name on {s.settlement_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.geographic_coordinates), $"Missing coordinates on {s.settlement_id}");
                Assert.True(s.estimated_population >= 0, $"Invalid population on {s.settlement_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.controlling_faction), $"Missing faction on {s.settlement_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.primary_export), $"Missing export on {s.settlement_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.defense_fortifications), $"Missing fortifications on {s.settlement_id}");
                Assert.True(s.water_source_radiation_mrh >= 0f, $"Invalid water radiation on {s.settlement_id}");
                Assert.False(string.IsNullOrWhiteSpace(s.harlan_scout_survey), $"Missing scout survey on {s.settlement_id}");
                Assert.True(s.harlan_scout_survey.Length > 25, $"Scout survey too brief on {s.settlement_id}");
                Assert.NotNull(s.tags);
                Assert.True(s.tags.Length > 0, $"Tags empty on {s.settlement_id}");
            }
        }
    }
}
