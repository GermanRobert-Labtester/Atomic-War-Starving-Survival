using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CourierDispatchCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void CourierDispatches_LoadsAll30CanonicalLettersAndParleys()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "courier_dispatches_master.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new CourierDispatchCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(30, catalog.AllDispatches.Count);

            // Test first dispatch (Salt Runner Day 18)
            var first = catalog.GetById("dispatch_01_the_salt_runner_letter");
            Assert.NotNull(first);
            Assert.Equal("Yakov the Salt Runner", first.sender);
            Assert.Equal(18, first.recorded_day);
            Assert.Contains("salted mutton lard", first.transcript);

            // Test final dispatch (Anton to Sonya Day 3650)
            var final = catalog.GetById("dispatch_30_the_final_courier_delivery");
            Assert.NotNull(final);
            Assert.Contains("Anton", final.sender);
            Assert.Equal(3650, final.recorded_day);
            Assert.Contains("Sonya's Tree", final.goods_manifest);
            Assert.Contains("first red apple", final.transcript);

            // Test participant search
            var harlanDispatches = catalog.GetByParticipant("Harlan");
            Assert.True(harlanDispatches.Count >= 3);

            // Test tag search
            var tradeDispatches = catalog.GetByTag("trade");
            Assert.True(tradeDispatches.Count >= 2);

            var courierDispatches = catalog.GetByTag("courier");
            Assert.True(courierDispatches.Count >= 2);
        }

        [Fact]
        public void CourierDispatches_AllEntriesHaveValidTranscriptsAndGoods()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "courier_dispatches_master.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new CourierDispatchCatalog();
            catalog.Load(json, serializer);

            foreach (var d in catalog.AllDispatches)
            {
                Assert.False(string.IsNullOrWhiteSpace(d.dispatch_id), "Missing dispatch_id");
                Assert.True(d.recorded_day > 0, $"Invalid recorded_day on {d.dispatch_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.sender), $"Missing sender on {d.dispatch_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.recipient), $"Missing recipient on {d.dispatch_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.route), $"Missing route on {d.dispatch_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.delivery_status), $"Missing status on {d.dispatch_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.goods_manifest), $"Missing goods on {d.dispatch_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.transcript), $"Missing transcript on {d.dispatch_id}");
                Assert.True(d.transcript.Length > 80, $"Transcript too brief on {d.dispatch_id}");
                Assert.NotNull(d.tags);
                Assert.True(d.tags.Length > 0, $"Tags empty on {d.dispatch_id}");
            }
        }
    }
}
