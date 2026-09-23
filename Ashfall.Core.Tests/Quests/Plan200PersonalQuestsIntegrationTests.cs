// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests.Quests
{
    public sealed class Plan200PersonalQuestsIntegrationTests
    {
        private const string SampleCatalog = @"{
            ""schema_version"": 1,
            ""quests"": [
                {
                    ""id"": ""quest_arc_engineer"",
                    ""title"": ""The Dynamo Secret"",
                    ""category"": ""personal"",
                    ""required_trait"": ""engineer"",
                    ""summary"": ""Fix the backup turbines."",
                    ""stages"": [
                        {
                            ""stage_index"": 0,
                            ""title"": ""Scavenge Spare Parts"",
                            ""description"": ""Find copper wiring."",
                            ""requirement_kind"": ""scavenge"",
                            ""target_id"": ""wire_copper"",
                            ""target_count"": 2,
                            ""choices"": [
                                {
                                    ""choice_id"": ""choice_overclock"",
                                    ""label"": ""Overclock Turbine"",
                                    ""morale_delta"": 4.0,
                                    ""next_stage"": 1
                                }
                            ]
                        },
                        {
                            ""stage_index"": 1,
                            ""title"": ""Run Test"",
                            ""description"": ""Let turbine run for 3 days."",
                            ""requirement_kind"": ""days_elapsed"",
                            ""target_count"": 3,
                            ""choices"": [
                                {
                                    ""choice_id"": ""choice_stabilize"",
                                    ""label"": ""Stabilize Frequency"",
                                    ""morale_delta"": 8.0,
                                    ""next_stage"": -1
                                }
                            ]
                        }
                    ]
                }
            ]
        }";

        [Fact]
        public void PersonalQuest_Lifecycle_TriggersProgressesAndCompletes()
        {
            var system = new PersonalQuestSystem();
            system.LoadCatalog(SampleCatalog, new SystemTextJsonSerializer());

            bool triggered = system.TryTriggerQuest("surv_tech", "engineer", 1);
            Assert.True(triggered);

            var q = system.GetActiveQuest("surv_tech");
            Assert.NotNull(q);
            Assert.Equal("quest_arc_engineer", q.questId);
            Assert.Equal(0, q.currentStage);

            // Progress stage 0
            system.ProgressRequirement("surv_tech", "scavenge", 2, "wire_copper");
            Assert.Equal(2, q.progressCount);

            bool choiceMade = system.ChooseOption("surv_tech", "choice_overclock", 1, out var choiceDef);
            Assert.True(choiceMade);
            Assert.NotNull(choiceDef);
            Assert.Equal(1, q.currentStage);

            // Advance stage 1 days_elapsed
            system.TickDay(2);
            system.TickDay(3);
            system.TickDay(4);
            Assert.Equal(3, q.progressCount);

            bool resolved = system.ChooseOption("surv_tech", "choice_stabilize", 4, out _);
            Assert.True(resolved);
            Assert.Null(system.GetActiveQuest("surv_tech"));
            Assert.Single(system.CompletedQuests);
            Assert.Equal(PersonalQuestStatus.Completed, system.CompletedQuests[0].status);
        }

        [Fact]
        public void PersonalQuest_StateRoundtrip_PreservesActiveAndCompleted()
        {
            var system = new PersonalQuestSystem();
            system.LoadCatalog(SampleCatalog, new SystemTextJsonSerializer());
            system.TryTriggerQuest("surv_tech", "engineer", 1);

            var state = system.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var deserialized = JsonSerializer.Deserialize<PersonalQuestSaveState>(json);
            Assert.NotNull(deserialized);

            var restoredSystem = new PersonalQuestSystem();
            restoredSystem.LoadCatalog(SampleCatalog, new SystemTextJsonSerializer());
            restoredSystem.RestoreState(deserialized!);

            var restoredQ = restoredSystem.GetActiveQuest("surv_tech");
            Assert.NotNull(restoredQ);
            Assert.Equal("quest_arc_engineer", restoredQ.questId);
        }

        [Fact]
        public void AuthoredData_PersonalQuestsJson_LoadsSuccessfully()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "personal_quests.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "personal_quests.json");
            Assert.True(System.IO.File.Exists(filePath), $"File not found: {filePath}");

            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<PersonalQuestCatalogData>(json);
            Assert.NotNull(catalog);
            Assert.True(catalog!.quests.Count >= 10);

            foreach (var q in catalog.quests)
            {
                Assert.False(string.IsNullOrWhiteSpace(q.id));
                Assert.False(string.IsNullOrWhiteSpace(q.title));
                Assert.False(string.IsNullOrWhiteSpace(q.required_trait));
                Assert.NotEmpty(q.stages);
                foreach (var stage in q.stages)
                {
                    Assert.False(string.IsNullOrWhiteSpace(stage.title));
                    Assert.NotEmpty(stage.choices);
                }
            }

            var system = new PersonalQuestSystem();
            system.LoadCatalog(catalog);
            Assert.True(system.Catalog.Count >= 10);

            var scoutQuests = system.GetQuestsForTrait("scout");
            Assert.NotEmpty(scoutQuests);
            Assert.Contains(scoutQuests, sq => sq.id == "pq_buried_cache");
        }

        [Fact]
        public void RewardBridges_TriggerMoraleAndItemDelegates_OnChoice()
        {
            var system = new PersonalQuestSystem();
            system.LoadCatalog(SampleCatalog, new SystemTextJsonSerializer());

            string? rewardedSurvivor = null;
            float appliedMorale = 0f;
            string? rewardedItem = null;
            int rewardedAmount = 0;

            system.MoraleDeltaApplier = (survId, delta) =>
            {
                rewardedSurvivor = survId;
                appliedMorale = delta;
            };

            system.ItemRewardApplier = (itemId, amount) =>
            {
                rewardedItem = itemId;
                rewardedAmount = amount;
            };

            system.TryTriggerQuest("surv_tech", "engineer", 1);
            system.ProgressRequirement("surv_tech", "scavenge", 2, "wire_copper");

            // Make choice with morale delta 4.0
            bool chosen = system.ChooseOption("surv_tech", "choice_overclock", 1, out _);
            Assert.True(chosen);

            Assert.Equal("surv_tech", rewardedSurvivor);
            Assert.Equal(4.0f, appliedMorale);
        }
    }
}
