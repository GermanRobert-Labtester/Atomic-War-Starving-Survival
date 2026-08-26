using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class RadioScriptbookCatalogTests
    : CatalogTestBase{
        private readonly string _catalogPath;

        public RadioScriptbookCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _catalogPath = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative", "radio_scriptbook.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative", "radio_scriptbook.json");
            }
        }

        [Fact]
        public void RadioScriptbookCatalog_LoadsAll15Entries()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file not found at: {_catalogPath}");

            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RadioScriptbookCatalog();
            catalog.Load(json, serializer);

            Assert.NotNull(catalog);
            Assert.Equal(15, catalog.AllBroadcasts.Count);
        }

        [Fact]
        public void RadioScriptbookCatalog_AllEntriesHaveValidFields()
        {
            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RadioScriptbookCatalog();
            catalog.Load(json, serializer);

            var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var broadcast in catalog.AllBroadcasts)
            {
                Assert.False(string.IsNullOrWhiteSpace(broadcast.broadcast_id), "Missing broadcast_id");
                Assert.StartsWith("rad_", broadcast.broadcast_id);
                Assert.True(seenIds.Add(broadcast.broadcast_id), $"Duplicate broadcast ID: {broadcast.broadcast_id}");

                Assert.True(broadcast.frequency_mhz > 0, $"Invalid frequency_mhz on {broadcast.broadcast_id}");
                Assert.False(string.IsNullOrWhiteSpace(broadcast.station_name), $"Missing station_name on {broadcast.broadcast_id}");
                Assert.False(string.IsNullOrWhiteSpace(broadcast.voice_profile), $"Missing voice_profile on {broadcast.broadcast_id}");
                Assert.True(broadcast.day_trigger >= 0, $"Invalid day_trigger on {broadcast.broadcast_id}");
                Assert.False(string.IsNullOrWhiteSpace(broadcast.title), $"Missing title on {broadcast.broadcast_id}");
                Assert.False(string.IsNullOrWhiteSpace(broadcast.transcript), $"Missing transcript on {broadcast.broadcast_id}");
                Assert.NotNull(broadcast.tags);
                Assert.NotEmpty(broadcast.tags);
            }
        }

        [Fact]
        public void RadioScriptbookCatalog_QueriesFunctionCorrectly()
        {
            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RadioScriptbookCatalog();
            catalog.Load(json, serializer);

            // By ID
            var keeper = catalog.GetById("rad_keeper_01_first_contact");
            Assert.NotNull(keeper);
            Assert.Equal(104.7f, keeper.frequency_mhz);
            Assert.Equal("Station 0 (The Deep Vault)", keeper.station_name);
            Assert.Equal(3, keeper.day_trigger);
            Assert.Equal("To Whoever is Holding the Tuner", keeper.title);

            // By frequency
            var freq104 = catalog.GetByFrequency(104.7f, 0.01f);
            Assert.NotEmpty(freq104);
            Assert.Contains(freq104, b => b.broadcast_id == "rad_keeper_01_first_contact");

            // Active broadcast on day 3
            var active = catalog.GetActiveBroadcast(104.7f, 3, 0.01f);
            Assert.NotNull(active);
            Assert.Equal("rad_keeper_01_first_contact", active.broadcast_id);
        }
    }
}
