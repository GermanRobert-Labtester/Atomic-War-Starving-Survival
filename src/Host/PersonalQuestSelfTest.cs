// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Quests;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 200 (Survivor Personal Quests & Character Arcs).
    /// </summary>
    internal static class PersonalQuestSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print("[PASS] " + message);
                }
                else
                {
                    GD.PrintErr("[FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                GD.Print("[PersonalQuestSelfTest] Starting Plan 200 verification...");

                // 1. HostSession & Core Initialization
                var session = PersonalQuestHostSession.Create(dataDirectory, new SeededRng(100), new GodotLog());
                Check(session != null, "HostSession: Created successfully");
                if (session == null)
                {
                    GD.PrintErr("[FAIL] HostSession is null");
                    return 1;
                }
                Check(session.System != null, "HostSession: System is initialized");

                // 2. Load catalog manually if needed for test
                const string testCatalogJson = @"{
                    ""schema_version"": 1,
                    ""quests"": [
                        {
                            ""id"": ""quest_medic_redemption"",
                            ""title"": ""A Doctor's Oath"",
                            ""required_class"": ""medic"",
                            ""min_survivor_level"": 1,
                            ""stages"": [
                                {
                                    ""stage_index"": 0,
                                    ""description"": ""Gather medical supplies from the wasteland."",
                                    ""requirement_kind"": ""gather_herbs"",
                                    ""target_count"": 3,
                                    ""target_entity_id"": ""herb_root"",
                                    ""choices"": [
                                        {
                                            ""choice_id"": ""choice_help_all"",
                                            ""label"": ""Help Everyone"",
                                            ""morale_delta"": 5.0,
                                            ""next_stage"": 1
                                        }
                                    ]
                                },
                                {
                                    ""stage_index"": 1,
                                    ""description"": ""Spend time reflecting on the journey."",
                                    ""requirement_kind"": ""days_elapsed"",
                                    ""target_count"": 2,
                                    ""choices"": [
                                        {
                                            ""choice_id"": ""choice_conclude"",
                                            ""label"": ""Move Forward"",
                                            ""morale_delta"": 10.0,
                                            ""next_stage"": -1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }";

                session.System!.LoadCatalog(testCatalogJson, new SystemTextJsonSerializer());
                Check(session.System!.Catalog.ContainsKey("quest_medic_redemption"), "Core: Catalog loaded quest_medic_redemption");

                // 3. Trigger Quest
                bool triggered = session.TryTriggerQuest("surv_doctor", "medic", 1);
                Check(triggered, "HostSession: TryTriggerQuest succeeded for surv_doctor");

                var activeQuest = session.GetActiveQuest("surv_doctor");
                Check(activeQuest != null, "HostSession: Active quest found for surv_doctor");
                if (activeQuest == null)
                {
                    GD.PrintErr("[FAIL] activeQuest is null");
                    return 1;
                }
                Check(activeQuest.questId == "quest_medic_redemption", "Core: Quest ID matches");
                Check(activeQuest.currentStage == 0, "Core: Initial stage is 0");

                // 4. Progress Requirement & Choices
                bool progressed = session.ProgressRequirement("surv_doctor", "gather_herbs", 3, "herb_root");
                Check(progressed, "HostSession: ProgressRequirement succeeded");

                bool choiceMade = session.ChooseOption("surv_doctor", "choice_help_all", 1, out var chosenDef);
                Check(choiceMade && chosenDef != null, "HostSession: ChooseOption advanced stage");
                Check(activeQuest.currentStage == 1, "Core: Advanced to stage 1");

                // 5. Daily Tick & Final Stage Choice
                session.TickDay(2);
                Check(activeQuest.progressCount == 1, "Core: TickDay incremented days_elapsed progress count");

                bool finished = session.ChooseOption("surv_doctor", "choice_conclude", 2, out _);
                Check(finished, "HostSession: Final stage choice concluded quest");
                Check(session.GetActiveQuest("surv_doctor") == null, "HostSession: Quest moved from active");
                Check(session.System.CompletedQuests.Count == 1, "Core: CompletedQuests count is 1");

                // 6. Persistence Roundtrip
                var captured = session.CaptureState();
                Check(captured != null && captured.completedQuests.Count == 1, "SaveStore: State captured successfully");

                var newSession = PersonalQuestHostSession.Create(dataDirectory, new SeededRng(200), new GodotLog());
                newSession.RestoreState(captured!);
                Check(newSession.System.CompletedQuests.Count == 1, "SaveStore: State restored successfully");
                Check(newSession.System.CompletedQuests[0].questId == "quest_medic_redemption", "SaveStore: Restored quest ID matches");

                // 7. UI Construction
                var panel = new UI.PersonalQuestPanel();
                panel.Bind(session);
                panel.RefreshView();
                Check(panel != null, "UI: PersonalQuestPanel instantiated and bound cleanly");

                GD.Print($"[PersonalQuestSelfTest] Complete with {failures} failure(s).");
                return failures == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[PersonalQuestSelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
