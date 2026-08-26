using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class GhostTransmissionCatalogTests
    : CatalogTestBase{
        private readonly string _catalogPath;

        public GhostTransmissionCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _catalogPath = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative", "ghost_transmissions.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative", "ghost_transmissions.json");
            }
        }

        [Fact]
        public void GhostTransmissionCatalog_LoadsAll12Entries()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file not found at: {_catalogPath}");

            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new GhostTransmissionCatalog();
            catalog.Load(json, serializer);

            Assert.NotNull(catalog);
            Assert.Equal(12, catalog.AllTransmissions.Count);
        }

        [Fact]
        public void GhostTransmissionCatalog_AllEntriesHaveValidFields()
        {
            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new GhostTransmissionCatalog();
            catalog.Load(json, serializer);

            var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in catalog.AllTransmissions)
            {
                Assert.False(string.IsNullOrWhiteSpace(entry.transmission_id), "Missing transmission_id");
                Assert.StartsWith("ghost_", entry.transmission_id);
                Assert.True(seenIds.Add(entry.transmission_id), $"Duplicate transmission ID: {entry.transmission_id}");

                Assert.True(entry.frequency_khz > 0, $"Invalid frequency on {entry.transmission_id}");
                Assert.False(string.IsNullOrWhiteSpace(entry.broadcast_format), $"Missing broadcast_format on {entry.transmission_id}");
                Assert.False(string.IsNullOrWhiteSpace(entry.estimated_origin), $"Missing estimated_origin on {entry.transmission_id}");
                Assert.False(string.IsNullOrWhiteSpace(entry.signal_anomaly), $"Missing signal_anomaly on {entry.transmission_id}");
                Assert.False(string.IsNullOrWhiteSpace(entry.transcript), $"Missing transcript on {entry.transmission_id}");
                Assert.False(string.IsNullOrWhiteSpace(entry.mystery_thread), $"Missing mystery_thread on {entry.transmission_id}");
                Assert.NotNull(entry.tags);
                Assert.NotEmpty(entry.tags);
            }
        }

        [Fact]
        public void GhostTransmissionCatalog_QueriesFunctionCorrectly()
        {
            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new GhostTransmissionCatalog();
            catalog.Load(json, serializer);

            // By ID
            var ticking = catalog.GetById("ghost_01_the_ticking_chime");
            Assert.NotNull(ticking);
            Assert.Equal(4625.0f, ticking.frequency_khz);
            Assert.Equal("Signal strength increases during severe geomagnetic storms and drops during clear daylight", ticking.signal_anomaly);
            Assert.Equal("q_mystery_salt_silo", ticking.mystery_thread);

            // By mystery thread
            var deadStations = catalog.GetByMysteryThread("q_mystery_salt_silo");
            Assert.NotEmpty(deadStations);

            // Frequency nearest match
            var nearest = catalog.FindNearestFrequency(4625.0f, 1.0f);
            Assert.NotNull(nearest);
            Assert.Equal("ghost_01_the_ticking_chime", nearest.transmission_id);
        }
    }
}
