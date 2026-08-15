using System.IO;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CrossingEndingsWorldHistoryTests
    {
        private static string FindDataDir()
        {
            string dataDir = string.Empty;
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) { dataDir = candidate; break; }
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return dataDir;
        }

        [Fact]
        public void WorldHistory_EveryEndingHasASecondParagraph()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = fileIO.ReadAllText(fileIO.Combine(dataDir, "world_history.json"));
            var entries = json.Deserialize<WorldHistoryEntry[]>(raw);

            string[] expected =
            {
                "ending_crossing_scale", "ending_crossing_underwrite",
                "ending_crossing_compact", "ending_crossing_none", "ending_crossing_walked"
            };
            var found = new System.Collections.Generic.List<string>();
            foreach (var e in entries)
                if (e != null && e.knowledge_key != null && e.knowledge_key.StartsWith("ending_crossing_"))
                    found.Add(e.knowledge_key);
            Assert.Equal(expected.Length, found.Count);
            foreach (var k in expected)
                Assert.Contains(k, found);
        }

        [Fact]
        public void WorldHistory_EndingParagraphsDiscoverableAtRecordsRoomOrWeighbridge()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = fileIO.ReadAllText(fileIO.Combine(dataDir, "world_history.json"));
            var entries = json.Deserialize<WorldHistoryEntry[]>(raw);

            foreach (var e in entries)
            {
                if (e == null || e.knowledge_key == null || !e.knowledge_key.StartsWith("ending_crossing_")) continue;
                Assert.True(
                    e.discovery_location_id == "loc_crossing_records_room" ||
                    e.discovery_location_id == "loc_crossing_weighbridge",
                    e.knowledge_key + " must be discoverable at records_room or weighbridge");
                Assert.Equal("ending_reached", e.discovery_trigger);
                Assert.False(string.IsNullOrEmpty(e.body));
                Assert.False(string.IsNullOrEmpty(e.title));
            }
        }

        [Fact]
        public void WorldHistory_EndingProseMatchesTheBibleHouseVoice()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = fileIO.ReadAllText(fileIO.Combine(dataDir, "world_history.json"));
            var entries = json.Deserialize<WorldHistoryEntry[]>(raw);

            var byKey = new System.Collections.Generic.Dictionary<string, WorldHistoryEntry>();
            foreach (var e in entries)
                if (e != null && e.knowledge_key != null) byKey[e.knowledge_key] = e;

            Assert.Contains("Osran still says he doesn't run the place", byKey["ending_crossing_scale"].body);
            Assert.Contains("Everyone eats. Everyone owes.", byKey["ending_crossing_underwrite"].body);
            Assert.Contains("There is a document now", byKey["ending_crossing_compact"].body);
            Assert.Contains("now has no scale either", byKey["ending_crossing_none"].body);
            Assert.Contains("Nobody there will remember the player's name", byKey["ending_crossing_walked"].body);
        }

        private class WorldHistoryEntry
        {
            public string era = string.Empty;
            public string year_month = string.Empty;
            public string title = string.Empty;
            public string body = string.Empty;
            public string discovery_location_id = string.Empty;
            public string discovery_trigger = string.Empty;
            public string knowledge_key = string.Empty;
        }
    }
}
