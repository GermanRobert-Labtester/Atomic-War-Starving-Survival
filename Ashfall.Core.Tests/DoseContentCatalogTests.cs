using System.IO;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>Expansion 07 content bundle — the four quest lines, three
    /// standing locations and five story/tool items that give the four dose
    /// registers a playable surface (plan §IV/VI/VII).</summary>
    public class DoseContentCatalogTests
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
        public void Load_FindsThreeLocationsFiveItemsFourQuests()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(3, catalog.locations.Count);
            Assert.Equal(5, catalog.items.Count);
            Assert.Equal(4, catalog.quests.Count);
        }

        [Fact]
        public void Locations_AreTheThreeStandingRooms()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var ids = new HashSet<string>();
            foreach (var l in catalog.locations) ids.Add(l.id);
            Assert.Contains("loc_the_dose_room", ids);
            Assert.Contains("loc_the_calibration_bench", ids);
            Assert.Contains("loc_the_childrens_baseline_board", ids);
        }

        [Fact]
        public void Items_AreBooksToolsAndMedicine()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var ids = new HashSet<string>();
            foreach (var i in catalog.items) ids.Add(i.id);
            Assert.Contains("item_dose_ledger", ids);
            Assert.Contains("item_calibration_key", ids);
            Assert.Contains("item_dosimeter_tag", ids);
            Assert.Contains("item_palliative_morphine", ids);
            Assert.Contains("item_cohort_first_board", ids);
        }

        [Fact]
        public void Quests_ExposeStagesAndChoices_NoDeadEnds()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(4, catalog.quests.Count);

            foreach (var q in catalog.quests)
            {
                Assert.False(string.IsNullOrEmpty(q.questlineId));
                Assert.False(string.IsNullOrEmpty(q.firstStageId));
                Assert.True(q.stages.Count >= 2, $"{q.questlineId} should have >= 2 stages");
                foreach (var s in q.stages)
                {
                    Assert.False(string.IsNullOrEmpty(s.stageId));
                    Assert.False(string.IsNullOrEmpty(s.narrativePrompt));
                    // a non-terminal stage must lead somewhere (a choice with a next stage)
                    if (!s.isTerminal)
                    {
                        Assert.NotNull(s.choices);
                        Assert.NotEmpty(s.choices);
                        bool hasNext = false;
                        foreach (var c in s.choices)
                            if (!string.IsNullOrEmpty(c.nextStageId)) { hasNext = true; break; }
                        Assert.True(hasNext, $"{q.questlineId}/{s.stageId} has no onward choice");
                    }
                    // every nextStageId must resolve within the questline
                    if (s.choices != null)
                    {
                        foreach (var c in s.choices)
                        {
                            if (string.IsNullOrEmpty(c.nextStageId)) continue;
                            Assert.True(q.FindStage(c.nextStageId) != null,
                                $"{q.questlineId}: {s.stageId} → dangling nextStageId '{c.nextStageId}'");
                        }
                    }
                }
            }
        }

        [Fact]
        public void Quests_HaveTheFourExpectedIds()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var ids = new HashSet<string>();
            foreach (var q in catalog.quests) ids.Add(q.questlineId);
            Assert.Contains("quest_the_dose_the_first_reading", ids);
            Assert.Contains("quest_the_sick_of_room_seven", ids);
            Assert.Contains("quest_the_childs_number", ids);
            Assert.Contains("quest_the_signed_hour", ids);
        }

        [Fact]
        public void QuestGateDays_MatchThePlan()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var byId = new Dictionary<string, QuestlineDefinition>();
            foreach (var q in catalog.quests) byId[q.questlineId] = q;

            // §IV: Day 40+ / 90+ / 150+ / 200+
            Assert.Equal(40, byId["quest_the_dose_the_first_reading"].minDay);
            Assert.Equal(90, byId["quest_the_sick_of_room_seven"].minDay);
            Assert.Equal(150, byId["quest_the_childs_number"].minDay);
            Assert.Equal(200, byId["quest_the_signed_hour"].minDay);
        }

        [Fact]
        public void GrantItems_ResolveAgainstItemCatalog()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var itemIds = new HashSet<string>();
            foreach (var i in catalog.items) if (!string.IsNullOrEmpty(i.id)) itemIds.Add(i.id);

            foreach (var q in catalog.quests)
            {
                foreach (var s in q.stages)
                {
                    foreach (var c in s.choices)
                    {
                        if (string.IsNullOrEmpty(c.grantItemId)) continue;
                        Assert.True(itemIds.Contains(c.grantItemId),
                            $"{q.questlineId} grants {c.grantItemId} but it is absent from dose_items.json");
                    }
                }
            }
        }

        [Fact]
        public void Host_RegistersContentQuestsIntoQuestlineSystem()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var session = DoseContentHostHarness.Create(dataDir);
            var questSystem = new QuestlineSystem();
            int registered = session.RegisterContentQuests(questSystem);
            Assert.Equal(4, registered);
            foreach (var id in new[] { "quest_the_dose_the_first_reading", "quest_the_sick_of_room_seven",
                "quest_the_childs_number", "quest_the_signed_hour" })
                Assert.NotNull(questSystem.FindDefinition(id));
        }

        /// <summary>Minimal engine-agnostic stand-in for the host wiring (the real
        /// host is in src/ and cannot be referenced by the core test project).</summary>
        private sealed class DoseContentHostHarness
        {
            public DoseContentCatalog Catalog;
            public static DoseContentHostHarness Create(string dataDir)
            {
                return new DoseContentHostHarness
                {
                    Catalog = DoseContentCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                };
            }
            public int RegisterContentQuests(QuestlineSystem questSystem)
            {
                if (questSystem == null || Catalog == null) return 0;
                int n = 0;
                foreach (var q in Catalog.quests)
                {
                    if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
                    questSystem.RegisterQuestline(q);
                    n++;
                }
                return n;
            }
        }
    }
}
