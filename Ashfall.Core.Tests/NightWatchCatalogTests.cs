using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class NightWatchCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void NightWatch_LoadsAll15CanonicalIncidentReports()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "night_watch_logbook.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new NightWatchCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(15, catalog.AllIncidents.Count);

            // Test first report (Standing Man at Post 4)
            var first = catalog.GetById("watch_01_the_standing_man_in_whiteout");
            Assert.NotNull(first);
            Assert.Equal("Sgt. Ilya Taras", first.sentry_name);
            Assert.Equal(22, first.recorded_day);
            Assert.Contains("Perimeter Post 4", first.log_entry);

            // Test final report (Sonya Day 3650)
            var final = catalog.GetById("watch_15_the_final_watch_entry");
            Assert.NotNull(final);
            Assert.Equal("Councilwoman Sonya", final.sentry_name);
            Assert.Equal(3650, final.recorded_day);
            Assert.Contains("First Morning Without a Mask", final.title);

            // Test sentry queries
            var polinaReports = catalog.GetBySentry("dwr_polina");
            Assert.Equal(4, polinaReports.Count);

            // Test threat level search
            var highThreats = catalog.GetByThreatLevel("High");
            Assert.True(highThreats.Count >= 2);
        }

        [Fact]
        public void NightWatch_AllEntriesHaveValidNarrativeIntegrity()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "night_watch_logbook.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new NightWatchCatalog();
            catalog.Load(json, serializer);

            foreach (var inc in catalog.AllIncidents)
            {
                Assert.False(string.IsNullOrWhiteSpace(inc.log_id), "Missing log_id");
                Assert.False(string.IsNullOrWhiteSpace(inc.sentry_name), $"Missing sentry_name on {inc.log_id}");
                Assert.False(string.IsNullOrWhiteSpace(inc.post_location), $"Missing location on {inc.log_id}");
                Assert.False(string.IsNullOrWhiteSpace(inc.weather_conditions), $"Missing weather on {inc.log_id}");
                Assert.False(string.IsNullOrWhiteSpace(inc.log_entry), $"Missing log_entry on {inc.log_id}");
                Assert.True(inc.log_entry.Length > 100, $"Log entry too brief on {inc.log_id}");
                Assert.NotNull(inc.tags);
                Assert.True(inc.tags.Length > 0, $"Tags empty on {inc.log_id}");
            }
        }
    }
}
