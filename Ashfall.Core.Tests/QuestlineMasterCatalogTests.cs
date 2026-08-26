using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class QuestlineMasterCatalogTests
    : CatalogTestBase{
        private static string FindDataDir() => DataDirectory;

        private static QuestlineMasterCatalog LoadReal()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new QuestlineMasterCatalogLoader(files, json);
            return loader.Load(FindDataDir());
        }

        [Fact]
        public void Registry_Loads_And_Contains_Known_Quests()
        {
            var catalog = LoadReal();
            Assert.True(catalog.Count >= 200, $"Expected >= 200 quest IDs, got {catalog.Count}");

            // Spot-check IDs from different expansion catalogs
            Assert.True(catalog.IsRegistered("quest_roster_caretaker"));   // duty_roster
            Assert.True(catalog.IsRegistered("quest_crossing_first_weigh")); // crossing
            Assert.True(catalog.IsRegistered("quest_holdfast_the_clerk"));   // holdfast
            Assert.True(catalog.IsRegistered("quest_the_childs_number"));    // dose
        }

        [Fact]
        public void Registry_Contains_All_YearOfAsh_Quest_IDs()
        {
            var catalog = LoadReal();

            // Load year_of_ash quest IDs from the actual catalog
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = files.ReadAllText(files.Combine(dataDir, "year_of_ash_quests.json"));
            var root = json.Deserialize<YearOfAshQuestRootProbe>(raw);
            Assert.NotNull(root);
            Assert.NotNull(root!.quests);

            var unregistered = new List<string>();
            foreach (var e in root.quests)
            {
                if (e == null || string.IsNullOrEmpty(e.id)) continue;
                if (!catalog.IsRegistered(e.id))
                    unregistered.Add(e.id);
            }

            Assert.True(unregistered.Count == 0,
                $"year_of_ash quest IDs not in master registry: {string.Join(", ", unregistered)}");
        }

        [Fact]
        public void Registry_Contains_All_Dose_Quest_IDs()
        {
            var catalog = LoadReal();

            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = files.ReadAllText(files.Combine(dataDir, "dose_quests.json"));
            var entries = CatalogLocator.LoadWrappedList<DoseQuestProbe>(raw, SystemTextJsonSerializer.Options);
            Assert.NotNull(entries);

            var unregistered = new List<string>();
            foreach (var e in entries!)
            {
                if (e == null || string.IsNullOrEmpty(e.questlineId)) continue;
                if (!catalog.IsRegistered(e.questlineId))
                    unregistered.Add(e.questlineId);
            }

            Assert.True(unregistered.Count == 0,
                $"dose quest IDs not in master registry: {string.Join(", ", unregistered)}");
        }

        [Fact]
        public void Registry_Rejects_Unknown_IDs()
        {
            var catalog = LoadReal();
            Assert.False(catalog.IsRegistered("quest_nonexistent_fake"));
            Assert.False(catalog.IsRegistered(""));
            Assert.False(catalog.IsRegistered(null!));
        }

        [Fact]
        public void Registry_FindUnregistered_Returns_Missing()
        {
            var catalog = LoadReal();
            var testIds = new[] { "quest_roster_caretaker", "quest_fake_not_in_registry", "quest_also_fake" };
            var missing = catalog.FindUnregistered(testIds);
            Assert.Equal(2, missing.Count);
            Assert.Contains("quest_fake_not_in_registry", missing);
            Assert.Contains("quest_also_fake", missing);
        }

        [Fact]
        public void Registry_Has_No_Duplicates()
        {
            var catalog = LoadReal();
            var all = catalog.All;
            var distinct = all.Distinct().ToList();
            Assert.Equal(distinct.Count, all.Count);
        }

        [Fact]
        public void Registry_Loads_From_Missing_Directory_Without_Crash()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new QuestlineMasterCatalogLoader(files, json);
            var catalog = loader.Load("nonexistent/path");
            Assert.Equal(0, catalog.Count);
        }

        [Fact]
        public void Registry_Loads_From_Missing_File_Without_Crash()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new QuestlineMasterCatalogLoader(files, json);
            // Point at a subdirectory that doesn't contain questline_master.json
            var catalog = loader.Load(Path.Combine(FindDataDir(), "narrative"));
            // narrative/ doesn't contain questline_master.json
            Assert.Equal(0, catalog.Count);
        }

        // ── Probe DTOs (test-local, matching the JSON shape) ──────────

        private sealed class YearOfAshQuestRootProbe
        {
            public int schema_version { get; set; }
            public List<YearOfAshQuestProbe> quests { get; set; } = new List<YearOfAshQuestProbe>();
        }

        private sealed class YearOfAshQuestProbe
        {
            public string id { get; set; } = string.Empty;
        }

        private sealed class DoseQuestProbe
        {
            public string questlineId { get; set; } = string.Empty;
        }
    }
}
