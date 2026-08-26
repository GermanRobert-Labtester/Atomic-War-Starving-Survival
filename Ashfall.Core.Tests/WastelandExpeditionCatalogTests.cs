using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class WastelandExpeditionCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void WastelandExpeditions_LoadsAll30LandmarkScriptbooks()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_expeditions_master.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new WastelandExpeditionCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(30, catalog.AllExpeditions.Count);

            // Test first expedition (Drowned Locomotive)
            var first = catalog.GetById("exp_site_01_the_drowned_locomotive");
            Assert.NotNull(first);
            Assert.Equal("The Sunken Coal Train of Siding 9", first.title);
            Assert.Contains("Canal Marshes", first.zone);
            Assert.Equal(2, first.choices.Count);
            Assert.Contains("+800 Fuel", first.choices[0].outcome_success);

            // Test final expedition (Summit Beacon Day 3650)
            var final = catalog.GetById("exp_site_30_the_valley_summit_beacon");
            Assert.NotNull(final);
            Assert.Equal("The Tessarat Valley High Sentry Mast", final.title);
            Assert.Contains("The New Dawn", final.choices[0].outcome_success);

            // Test zone filtering
            var canalMarshes = catalog.GetByZone("Canal Marshes");
            Assert.True(canalMarshes.Count >= 4);

            // Test tag search
            var medicalSites = catalog.GetByTag("medical");
            Assert.True(medicalSites.Count >= 3);
        }

        [Fact]
        public void WastelandExpeditions_AllEntriesHaveValidChoicesAndProse()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_expeditions_master.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new WastelandExpeditionCatalog();
            catalog.Load(json, serializer);

            foreach (var e in catalog.AllExpeditions)
            {
                Assert.False(string.IsNullOrWhiteSpace(e.expedition_id), "Missing expedition_id");
                Assert.False(string.IsNullOrWhiteSpace(e.title), $"Missing title on {e.expedition_id}");
                Assert.False(string.IsNullOrWhiteSpace(e.zone), $"Missing zone on {e.expedition_id}");
                Assert.False(string.IsNullOrWhiteSpace(e.ambient_prose), $"Missing prose on {e.expedition_id}");
                Assert.True(e.ambient_prose.Length > 80, $"Prose too brief on {e.expedition_id}");
                Assert.False(string.IsNullOrWhiteSpace(e.threat_description), $"Missing threat on {e.expedition_id}");
                Assert.NotNull(e.choices);
                Assert.True(e.choices.Count >= 2, $"Expected at least 2 choices on {e.expedition_id}");
                foreach (var c in e.choices)
                {
                    Assert.False(string.IsNullOrWhiteSpace(c.choice_id), $"Missing choice_id on {e.expedition_id}");
                    Assert.False(string.IsNullOrWhiteSpace(c.label), $"Missing label on {c.choice_id}");
                    Assert.False(string.IsNullOrWhiteSpace(c.skill_required), $"Missing skill on {c.choice_id}");
                    Assert.False(string.IsNullOrWhiteSpace(c.outcome_success), $"Missing success outcome on {c.choice_id}");
                    Assert.False(string.IsNullOrWhiteSpace(c.outcome_risk), $"Missing risk outcome on {c.choice_id}");
                }
                Assert.NotNull(e.tags);
                Assert.True(e.tags.Length > 0, $"Tags empty on {e.expedition_id}");
            }
        }
    }
}
