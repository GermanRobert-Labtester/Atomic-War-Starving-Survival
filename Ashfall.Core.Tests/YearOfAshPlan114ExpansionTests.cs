using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 114 catalog gates. These tests validate the data-authority expansion
    /// without creating a second quest runtime: the live loader and QuestlineSystem
    /// remain the only execution path.
    /// </summary>
    public sealed class YearOfAshPlan114ExpansionTests
    {
        private static readonly string[] ExistingQuestlineIds =
        {
            "quest_garrison_blood_debt",
            "quest_ash_sign_revelation",
            "quest_rebuilder_seed_vault",
            "quest_hydro_baron_aqueduct",
            "quest_black_ops_null_order",
            "quest_survivor_mutiny",
            "quest_the_last_broadcast",
            "quest_winter_harvest"
        };

        private static readonly string[] NewQuestlineIds =
        {
            "quest_garrison_amnesty_offer",
            "quest_ash_sign_pilgrimage",
            "quest_rebuilder_irrigation",
            "quest_hydro_baron_water_tax",
            "quest_black_ops_blackmail",
            "quest_garrison_mutiny",
            "quest_rebuilder_seed_failure"
        };

        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 8; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string? parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }

            return Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
        }

        private static List<QuestlineDefinition> LoadCatalog()
        {
            return YearOfAshCatalogLoader.LoadQuestlines(
                FindDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        [Fact]
        public void Catalog_HasExactlyFifteenQuestlines_AndPreservesTheExistingEight()
        {
            var catalog = LoadCatalog();
            Assert.Equal(15, catalog.Count);

            var ids = catalog.Select(q => q.questlineId).ToList();
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
            foreach (string id in ExistingQuestlineIds)
                Assert.Contains(id, ids);
            foreach (string id in NewQuestlineIds)
                Assert.Contains(id, ids);
        }

        [Fact]
        public void NewQuestlines_HaveTheRequestedFactionCoverageAndDepth()
        {
            var catalog = LoadCatalog();
            var expected = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["quest_garrison_amnesty_offer"] = "faction_central_garrison",
                ["quest_ash_sign_pilgrimage"] = "faction_ash_sign",
                ["quest_rebuilder_irrigation"] = "faction_rebuilders",
                ["quest_hydro_baron_water_tax"] = "faction_hydro_barons",
                ["quest_black_ops_blackmail"] = "faction_black_ops",
                ["quest_garrison_mutiny"] = "faction_central_garrison",
                ["quest_rebuilder_seed_failure"] = "faction_rebuilders"
            };

            foreach (var pair in expected)
            {
                var quest = catalog.Single(q => q.questlineId == pair.Key);
                Assert.Equal(pair.Value, quest.factionTag);
                Assert.InRange(quest.minDay, 180, 360);
                Assert.True(quest.maxDay > quest.minDay);
                Assert.InRange(quest.stages.Count, 4, 7);
                Assert.All(quest.stages, stage => Assert.InRange(stage.choices.Count, 0, 4));
                Assert.Contains(quest.stages, stage => stage.isTerminal);
            }
        }

        [Fact]
        public void Catalog_HasGloballyUniqueStageAndChoiceIds_AndResolvedEdges()
        {
            var catalog = LoadCatalog();
            var stageIds = new HashSet<string>(StringComparer.Ordinal);
            var choiceIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var quest in catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(quest.firstStageId), quest.questlineId);
                Assert.NotNull(quest.FindStage(quest.firstStageId));

                var stages = quest.stages.ToDictionary(stage => stage.stageId, StringComparer.Ordinal);
                foreach (var stage in quest.stages)
                {
                    Assert.False(string.IsNullOrWhiteSpace(stage.stageId));
                    Assert.True(stageIds.Add(stage.stageId), $"Duplicate stage ID: {stage.stageId}");
                    Assert.False(string.IsNullOrWhiteSpace(stage.title));
                    Assert.False(string.IsNullOrWhiteSpace(stage.narrativePrompt));
                    Assert.InRange(stage.unlockOnDay, quest.minDay, quest.maxDay);

                    if (stage.isTerminal)
                    {
                        Assert.Empty(stage.choices);
                        Assert.True(stage.terminalOutcome == QuestlineStatus.Completed
                            || stage.terminalOutcome == QuestlineStatus.Failed);
                    }
                    else
                    {
                        Assert.NotEmpty(stage.choices);
                    }

                    foreach (var choice in stage.choices)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(choice.choiceId));
                        Assert.True(choiceIds.Add(choice.choiceId), $"Duplicate choice ID: {choice.choiceId}");
                        Assert.False(string.IsNullOrWhiteSpace(choice.text));
                        Assert.False(string.IsNullOrWhiteSpace(choice.outcomeNarrative));
                        Assert.True(choice.grantItemQuantity >= 0);

                        if (!string.IsNullOrEmpty(choice.nextStageId))
                            Assert.True(stages.ContainsKey(choice.nextStageId),
                                $"{quest.questlineId}/{stage.stageId}/{choice.choiceId} points to missing {choice.nextStageId}");
                    }
                }

                AssertReachableWithoutCycles(quest, stages);
            }
        }

        [Fact]
        public void NewQuestlines_ReferencesResolveToCanonicalFactionsItemsAndDoorEncounters()
        {
            var catalog = LoadCatalog();
            var canonicalFactions = new HashSet<string>(StringComparer.Ordinal)
            {
                "faction_central_garrison",
                "faction_ash_sign",
                "faction_rebuilders",
                "faction_hydro_barons",
                "faction_black_ops"
            };

            var dataDir = FindDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var itemIds = new HashSet<string>(
                YearOfAshCatalogLoader.LoadItems(dataDir, io, json).Select(item => item.id),
                StringComparer.Ordinal);
            var encounterIds = new HashSet<string>(
                DoorEncounterCatalogLoader.Load(dataDir, io, json).Select(entry => entry.encounterId),
                StringComparer.Ordinal);

            foreach (var quest in catalog.Where(q => NewQuestlineIds.Contains(q.questlineId)))
            {
                Assert.Contains(quest.factionTag, canonicalFactions);
                foreach (var choice in quest.stages.SelectMany(stage => stage.choices))
                {
                    if (!string.IsNullOrEmpty(choice.targetFactionId))
                        Assert.Contains(choice.targetFactionId, canonicalFactions);
                    if (!string.IsNullOrEmpty(choice.grantItemId))
                        Assert.Contains(choice.grantItemId, itemIds);
                    if (!string.IsNullOrEmpty(choice.unlockEncounterId))
                        Assert.Contains(choice.unlockEncounterId, encounterIds);
                }
            }
        }

        [Fact]
        public void NewQuestlines_AreAvailableOnlyInsideTheirAbsoluteStartWindows()
        {
            var catalog = LoadCatalog();
            var system = new QuestlineSystem();
            foreach (var quest in catalog.Where(q => NewQuestlineIds.Contains(q.questlineId)))
                system.RegisterQuestline(quest);

            foreach (var quest in catalog.Where(q => NewQuestlineIds.Contains(q.questlineId)))
            {
                Assert.Contains(system.GetAvailableQuestlines(quest.minDay), q => q.questlineId == quest.questlineId);
                Assert.Contains(system.GetAvailableQuestlines(quest.maxDay), q => q.questlineId == quest.questlineId);
                Assert.DoesNotContain(system.GetAvailableQuestlines(quest.minDay - 1), q => q.questlineId == quest.questlineId);
                Assert.DoesNotContain(system.GetAvailableQuestlines(quest.maxDay + 1), q => q.questlineId == quest.questlineId);
            }
        }

        [Fact]
        public void NewQuestlines_ChoiceEffectsRemainOneShotThroughTheLiveRuntime()
        {
            var quest = LoadCatalog().Single(q => q.questlineId == "quest_hydro_baron_water_tax");
            var system = new QuestlineSystem();
            system.RegisterQuestline(quest);
            Assert.True(system.StartQuestline(quest.questlineId, quest.minDay));

            var first = system.TakeChoice(quest.questlineId, "choice_water_tax_pay_levy", quest.minDay);
            Assert.NotNull(first);
            Assert.Equal(20, first!.factionDelta);

            var record = system.GetActiveRecord(quest.questlineId);
            Assert.NotNull(record);
            Assert.Null(system.TakeChoice(quest.questlineId, "choice_water_tax_pay_levy", quest.minDay));
            Assert.Single(record!.choiceHistory);
        }

        private static void AssertReachableWithoutCycles(
            QuestlineDefinition quest,
            Dictionary<string, QuestStage> stages)
        {
            var reachable = new HashSet<string>(StringComparer.Ordinal);
            var visiting = new HashSet<string>(StringComparer.Ordinal);

            void Visit(string stageId)
            {
                Assert.True(stages.ContainsKey(stageId), $"Missing stage {stageId}");
                Assert.True(reachable.Add(stageId) || !visiting.Contains(stageId),
                    $"Cycle detected in {quest.questlineId} at {stageId}");
                if (!visiting.Add(stageId)) return;

                foreach (var choice in stages[stageId].choices)
                {
                    if (!string.IsNullOrEmpty(choice.nextStageId))
                        Visit(choice.nextStageId);
                }

                visiting.Remove(stageId);
            }

            Visit(quest.firstStageId);
            Assert.Equal(stages.Count, reachable.Count);
        }
    }
}
