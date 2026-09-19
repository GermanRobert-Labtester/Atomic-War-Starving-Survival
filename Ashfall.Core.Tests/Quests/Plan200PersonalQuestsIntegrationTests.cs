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
    }
}
