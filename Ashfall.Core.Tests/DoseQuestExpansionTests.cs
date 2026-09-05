// SPDX-License-Identifier: MIT
// Comprehensive verification suite for Plan 101 — Dose Quests Expansion (4 -> 12 questlines).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DoseQuestExpansionTests
    {
        private static string FindDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(candidate)) return candidate;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            return string.Empty;
        }

        private static DoseContentCatalog LoadDoseCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "StreamingAssets/Data directory missing");
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return DoseContentCatalogLoader.Load(dataDir, io, json);
        }

        private static HashSet<string> LoadValidItemIds()
        {
            var catalog = LoadDoseCatalog();
            var validIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var it in catalog.items)
            {
                if (!string.IsNullOrEmpty(it.id)) validIds.Add(it.id);
            }

            // Also load base items.json
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string itemPath = io.Combine(dataDir, "items.json");
            if (io.FileExists(itemPath))
            {
                try
                {
                    var itemsRoot = json.Deserialize<ItemsEnvelopeProbe>(io.ReadAllText(itemPath));
                    if (itemsRoot?.items != null)
                    {
                        foreach (var it in itemsRoot.items)
                        {
                            if (!string.IsNullOrEmpty(it.id)) validIds.Add(it.id);
                        }
                    }
                }
                catch { /* best effort */ }
            }
            return validIds;
        }

        private class ItemsEnvelopeProbe
        {
            public List<ItemProbe>? items { get; set; }
        }

        private class ItemProbe
        {
            public string id { get; set; } = string.Empty;
        }

        [Fact]
        public void DoseCatalog_LoadsExactlyTwelveQuestlines()
        {
            var catalog = LoadDoseCatalog();
            Assert.NotNull(catalog.quests);
            Assert.Equal(12, catalog.quests.Count);
        }

        [Fact]
        public void CanonicalQuestlines_AllTwelveMatchMigrationAllowlist()
        {
            var catalog = LoadDoseCatalog();
            var loadedIds = catalog.quests.Select(q => q.questlineId).ToHashSet(StringComparer.Ordinal);

            Assert.Equal(12, DoseQuestMigration.CanonicalQuestlineIds.Length);
            foreach (var canonicalId in DoseQuestMigration.CanonicalQuestlineIds)
            {
                Assert.Contains(canonicalId, loadedIds);
                Assert.True(DoseQuestMigration.IsDoseQuestline(canonicalId));
            }
        }

        [Fact]
        public void BaselineFourQuests_ArePreservedWithOriginalContent()
        {
            var catalog = LoadDoseCatalog();
            var quests = catalog.quests.ToDictionary(q => q.questlineId);

            Assert.True(quests.ContainsKey("quest_the_dose_the_first_reading"));
            Assert.Equal("The First Reading", quests["quest_the_dose_the_first_reading"].title);

            Assert.True(quests.ContainsKey("quest_the_sick_of_room_seven"));
            Assert.Equal("The Sick of Room Seven", quests["quest_the_sick_of_room_seven"].title);

            Assert.True(quests.ContainsKey("quest_the_childs_number"));
            Assert.Equal("The Child's Number", quests["quest_the_childs_number"].title);

            Assert.True(quests.ContainsKey("quest_the_signed_hour"));
            Assert.Equal("The Signed Hour", quests["quest_the_signed_hour"].title);
        }

        [Fact]
        public void NewEightQuests_AreAuthoredAndDistinct()
        {
            var catalog = LoadDoseCatalog();
            var quests = catalog.quests.ToDictionary(q => q.questlineId);

            string[] newIds =
            {
                "quest_the_falsified_reading",
                "quest_the_stolen_dosimeter",
                "quest_child_over_the_limit",
                "quest_the_register_audit",
                "quest_black_market_clean_bill",
                "quest_the_broken_calibration_chain",
                "quest_exposure_for_the_essential_worker",
                "quest_the_missing_page"
            };

            foreach (var id in newIds)
            {
                Assert.True(quests.ContainsKey(id), $"Missing expected new quest: {id}");
                var q = quests[id];
                Assert.False(string.IsNullOrWhiteSpace(q.title));
                Assert.False(string.IsNullOrWhiteSpace(q.synopsis));
                Assert.True(q.stages.Count >= 2, $"Quest {id} has fewer than 2 stages");
            }
        }

        [Fact]
        public void GraphIntegrity_AllStagesAndChoicesFormValidDAGsWithoutCycles()
        {
            var catalog = LoadDoseCatalog();

            foreach (var q in catalog.quests)
            {
                var stageMap = q.stages.ToDictionary(s => s.stageId);
                var visited = new HashSet<string>();
                var inStack = new HashSet<string>();

                // First stage must exist
                Assert.False(string.IsNullOrEmpty(q.firstStageId), $"Quest {q.questlineId} has empty firstStageId");
                Assert.True(stageMap.ContainsKey(q.firstStageId), $"Quest {q.questlineId} firstStageId {q.firstStageId} not in stages");

                // Check DFS for reachability and cycles
                bool HasCycle(string stageId)
                {
                    visited.Add(stageId);
                    inStack.Add(stageId);

                    if (stageMap.TryGetValue(stageId, out var stage))
                    {
                        if (stage.isTerminal)
                        {
                            Assert.Empty(stage.choices);
                        }
                        else
                        {
                            Assert.InRange(stage.choices.Count, 2, 3);
                            foreach (var choice in stage.choices)
                            {
                                Assert.False(string.IsNullOrEmpty(choice.nextStageId),
                                    $"Choice {choice.choiceId} in {stageId} has empty nextStageId");
                                Assert.True(stageMap.ContainsKey(choice.nextStageId),
                                    $"Choice {choice.choiceId} in {stageId} references missing nextStageId {choice.nextStageId}");

                                if (!visited.Contains(choice.nextStageId))
                                {
                                    if (HasCycle(choice.nextStageId)) return true;
                                }
                                else if (inStack.Contains(choice.nextStageId))
                                {
                                    return true; // cycle detected
                                }
                            }
                        }
                    }

                    inStack.Remove(stageId);
                    return false;
                }

                Assert.False(HasCycle(q.firstStageId), $"Quest {q.questlineId} contains a cycle");

                // At least one terminal stage must be reachable
                bool hasTerminal = q.stages.Any(s => s.isTerminal);
                Assert.True(hasTerminal, $"Quest {q.questlineId} has no terminal stages");
            }
        }

        [Fact]
        public void ContentQuality_ChoicesHaveValidTextAndNarratives()
        {
            var catalog = LoadDoseCatalog();

            foreach (var q in catalog.quests)
            {
                foreach (var stage in q.stages)
                {
                    Assert.False(string.IsNullOrWhiteSpace(stage.title), $"Stage {stage.stageId} has empty title");
                    Assert.False(string.IsNullOrWhiteSpace(stage.narrativePrompt), $"Stage {stage.stageId} has empty prompt");

                    foreach (var c in stage.choices)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(c.text), $"Choice {c.choiceId} has empty text");
                        Assert.False(string.IsNullOrWhiteSpace(c.outcomeNarrative), $"Choice {c.choiceId} has empty outcome narrative");
                        Assert.InRange(c.moraleDelta, -5, 5);
                        Assert.InRange(c.guiltDelta, -5, 5);
                    }
                }
            }
        }

        [Fact]
        public void ItemGrants_AllReferencedItemsExistInItemCatalog()
        {
            var catalog = LoadDoseCatalog();
            var validItems = LoadValidItemIds();

            foreach (var q in catalog.quests)
            {
                foreach (var s in q.stages)
                {
                    foreach (var c in s.choices)
                    {
                        if (!string.IsNullOrEmpty(c.grantItemId))
                        {
                            Assert.Contains(c.grantItemId, validItems);
                            Assert.True(c.grantItemQuantity > 0, $"Choice {c.choiceId} grants item {c.grantItemId} with non-positive quantity");
                        }
                    }
                }
            }
        }

        [Fact]
        public void Pacing_AllTwelveQuestsAreWithinReachableCampaignWindows()
        {
            var catalog = LoadDoseCatalog();

            foreach (var q in catalog.quests)
            {
                Assert.True(q.minDay >= 1, $"Quest {q.questlineId} minDay < 1");
                Assert.True(q.minDay <= 240, $"Quest {q.questlineId} minDay {q.minDay} exceeds reachable day 240 window");
                Assert.True(q.maxDay >= q.minDay, $"Quest {q.questlineId} inverted day window: {q.minDay} > {q.maxDay}");
                Assert.True(q.maxDay <= 360, $"Quest {q.questlineId} maxDay {q.maxDay} exceeds 360-day horizon");
            }
        }

        [Fact]
        public void Migration_AdoptAndStripRoundTripsCleanly()
        {
            var doseState = new QuestlineSystemState();
            var yoaState = new QuestlineSystemState();

            // Simulate legacy Year of Ash progress with dose quests
            yoaState.active.Add(new ActiveQuestlineRecord
            {
                questlineId = "quest_the_falsified_reading",
                currentStageId = "stage_falsified_audit",
                status = QuestlineStatus.Active,
                dayStarted = 65,
                choiceHistory = new List<string>()
            });
            yoaState.completedQuestlineIds.Add("quest_the_dose_the_first_reading");

            int adopted = DoseQuestMigration.AdoptFromYearOfAsh(doseState, yoaState);
            Assert.Equal(2, adopted);
            Assert.Single(doseState.active);
            Assert.Equal("quest_the_falsified_reading", doseState.active[0].questlineId);
            Assert.Contains("quest_the_dose_the_first_reading", doseState.completedQuestlineIds);

            int stripped = DoseQuestMigration.StripFromYearOfAsh(yoaState);
            Assert.Equal(2, stripped);
            Assert.Empty(yoaState.active);
            Assert.Empty(yoaState.completedQuestlineIds);
        }
    }
}
