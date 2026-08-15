using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class RegionalTreatyCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void RegionalTreaties_LoadsAll16CanonicalTreaties()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "regional_treaty_protocols.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RegionalTreatyCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(16, catalog.AllTreaties.Count);

            // Test first treaty (Lock 4)
            var t1 = catalog.GetById("treaty_01_lock_4_sluice_and_brine_concession");
            Assert.NotNull(t1);
            Assert.Equal(45, t1.ratified_day);
            Assert.Contains("Canal Salvage Syndicate", t1.treaty_articles);
            Assert.Equal(120.0f, t1.water_allocation_lpm);

            // Test day-based ratification query
            var day100Treaties = catalog.GetRatifiedByDay(100);
            Assert.Equal(2, day100Treaties.Count);

            var day3650Treaties = catalog.GetRatifiedByDay(3650);
            Assert.Equal(16, day3650Treaties.Count);

            // Test final grand treaty (Constitution of the Century Seed)
            var finale = catalog.GetById("treaty_16_the_constitution_of_the_valley_of_tessarat");
            Assert.NotNull(finale);
            Assert.Equal(16, finale.signatory_factions.Length);
            Assert.Contains("Sonya's Tree in the Sunken Atrium", finale.treaty_articles);

            // Test faction search
            var harlanTreaties = catalog.GetBySignatoryFaction("weigher");
            Assert.True(harlanTreaties.Count >= 4);

            var garrisonTreaties = catalog.GetBySignatoryFaction("garrison");
            Assert.True(garrisonTreaties.Count >= 4);
        }

        [Fact]
        public void RegionalTreaties_AllEntriesHaveValidArticlesAndPenalties()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "regional_treaty_protocols.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RegionalTreatyCatalog();
            catalog.Load(json, serializer);

            foreach (var t in catalog.AllTreaties)
            {
                Assert.False(string.IsNullOrWhiteSpace(t.treaty_id), "Missing treaty_id");
                Assert.False(string.IsNullOrWhiteSpace(t.treaty_title), $"Missing treaty_title on {t.treaty_id}");
                Assert.True(t.ratified_day > 0, $"Invalid ratified_day on {t.treaty_id}");
                Assert.NotNull(t.signatory_factions);
                Assert.True(t.signatory_factions.Length >= 2, $"Less than 2 signatories on {t.treaty_id}");
                Assert.False(string.IsNullOrWhiteSpace(t.demarcated_territory), $"Missing territory on {t.treaty_id}");
                Assert.False(string.IsNullOrWhiteSpace(t.tariff_schedule), $"Missing tariff on {t.treaty_id}");
                Assert.False(string.IsNullOrWhiteSpace(t.treaty_articles), $"Missing articles on {t.treaty_id}");
                Assert.True(t.treaty_articles.Length > 50, $"Articles too short on {t.treaty_id}");
                Assert.False(string.IsNullOrWhiteSpace(t.penalties), $"Missing penalties on {t.treaty_id}");
                Assert.NotNull(t.tags);
                Assert.True(t.tags.Length > 0, $"Tags empty on {t.treaty_id}");
            }
        }
    }
}
