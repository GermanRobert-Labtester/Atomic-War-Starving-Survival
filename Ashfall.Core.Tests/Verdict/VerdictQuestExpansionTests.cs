// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Verdict
{
    public class VerdictQuestExpansionTests : CatalogTestBase
    {
        private static readonly string[] s_sevenNewQuestlineIds = new[]
        {
            "quest_verdict_the_dead_frequency",
            "quest_verdict_the_missing_reel",
            "quest_verdict_the_cold_reading",
            "quest_verdict_the_unsigned_tally",
            "quest_verdict_the_interference_pattern",
            "quest_verdict_the_last_entry",
            "quest_verdict_the_open_count"
        };

        private static readonly string[] s_eightBaselineNarrativeIds = new[]
        {
            "quest_verdict_the_warm_range",
            "quest_verdict_the_reckoning_call",
            "quest_verdict_the_hold",
            "quest_verdict_eden_grabs",
            "quest_verdict_the_tape_silo",
            "quest_verdict_the_mortars_timetable",
            "quest_verdict_the_shift_charter",
            "quest_verdict_the_summons"
        };

        private static readonly string[] s_eightCourtProceduralIds = new[]
        {
            "quest_verdict_alibi_verification",
            "quest_verdict_witness_subpoena",
            "quest_verdict_charter_authentication",
            "quest_verdict_prior_verdict_appeal",
            "quest_verdict_chain_of_custody",
            "quest_verdict_machine_interpretation_contest",
            "quest_verdict_forged_evidence_inquest",
            "quest_verdict_reconciled_testimony"
        };

        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            return string.Empty;
        }

        private static (QuestlineSystem system, List<QuestlineDefinition> catalog) LoadSystemAndCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Data directory not found.");
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new QuestlineSystem();
            int count = VerdictQuestCatalogLoader.LoadAndRegister(system, dataDir, files, json);
            Assert.True(count > 0, "Expected registered questlines from verdict_questlines.json");
            return (system, system.Catalog.ToList());
        }

        [Fact]
        public void LoadAndRegister_LoadsAllQuestlines_CountAtLeast23()
        {
            var (_, catalog) = LoadSystemAndCatalog();
            Assert.True(catalog.Count >= 23, $"Expected at least 23 questlines, got {catalog.Count}");
        }

        [Fact]
        public void PreservesAllEightBaselineNarrativeQuestlines()
        {
            var (system, _) = LoadSystemAndCatalog();
            foreach (string id in s_eightBaselineNarrativeIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);
                Assert.Equal(id, def.questlineId);
                Assert.NotEmpty(def.title);
                Assert.NotEmpty(def.synopsis);
                Assert.NotEmpty(def.stages);
            }
        }

        [Fact]
        public void PreservesAllEightCourtProceduralQuestlines()
        {
            var (system, _) = LoadSystemAndCatalog();
            foreach (string id in s_eightCourtProceduralIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);
                Assert.Equal(id, def.questlineId);
                Assert.NotEmpty(def.title);
                Assert.NotEmpty(def.synopsis);
                Assert.NotEmpty(def.stages);
            }
        }

        [Fact]
        public void ContainsAllSevenNewInvestigationCases()
        {
            var (system, _) = LoadSystemAndCatalog();
            foreach (string id in s_sevenNewQuestlineIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);
                Assert.Equal(id, def.questlineId);
                Assert.NotEmpty(def.title);
                Assert.NotEmpty(def.synopsis);
            }
        }

        [Fact]
        public void NewQuestlines_HaveFourToSevenStages_AndTwoToFourChoicesPerStage()
        {
            var (system, _) = LoadSystemAndCatalog();
            foreach (string id in s_sevenNewQuestlineIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);
                Assert.InRange(def.stages.Count, 4, 7);

                foreach (var stage in def.stages)
                {
                    Assert.NotEmpty(stage.stageId);
                    Assert.NotEmpty(stage.title);
                    Assert.NotEmpty(stage.narrativePrompt);
                    Assert.InRange(stage.choices.Count, 2, 4);

                    foreach (var choice in stage.choices)
                    {
                        Assert.NotEmpty(choice.choiceId);
                        Assert.NotEmpty(choice.text);
                        Assert.NotEmpty(choice.outcomeNarrative);
                    }
                }
            }
        }

        [Fact]
        public void NewQuestlines_StageGraphsAreAcyclicDirectedGraphs_WithValidFirstStageAndTerminals()
        {
            var (system, _) = LoadSystemAndCatalog();
            foreach (string id in s_sevenNewQuestlineIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);

                var stageIds = new HashSet<string>(def.stages.Select(s => s.stageId), StringComparer.Ordinal);
                Assert.Equal(def.stages.Count, stageIds.Count); // all unique within questline

                Assert.True(stageIds.Contains(def.firstStageId),
                    $"Questline '{id}' firstStageId '{def.firstStageId}' must exist in stages.");

                var terminalStages = def.stages.Where(s => s.isTerminal).ToList();
                Assert.NotEmpty(terminalStages);

                // Check all non-empty nextStageIds resolve
                foreach (var stage in def.stages)
                {
                    foreach (var choice in stage.choices)
                    {
                        if (!string.IsNullOrEmpty(choice.nextStageId))
                        {
                            Assert.True(stageIds.Contains(choice.nextStageId),
                                $"Questline '{id}' choice '{choice.choiceId}' points to non-existent stage '{choice.nextStageId}'");
                        }
                        else if (stage.isTerminal)
                        {
                            // Terminal choices should have empty nextStageId
                            Assert.Equal(string.Empty, choice.nextStageId);
                        }
                    }
                }

                // Check DAG: BFS/DFS to ensure no cycles and every stage is reachable
                var reachable = new HashSet<string>(StringComparer.Ordinal);
                var queue = new Queue<string>();
                queue.Enqueue(def.firstStageId);
                reachable.Add(def.firstStageId);

                while (queue.Count > 0)
                {
                    string current = queue.Dequeue();
                    var s = def.FindStage(current);
                    Assert.NotNull(s);
                    foreach (var c in s.choices)
                    {
                        if (!string.IsNullOrEmpty(c.nextStageId) && reachable.Add(c.nextStageId))
                        {
                            queue.Enqueue(c.nextStageId);
                        }
                    }
                }

                // Verify every stage is reachable from firstStageId
                foreach (var stage in def.stages)
                {
                    Assert.True(reachable.Contains(stage.stageId),
                        $"Stage '{stage.stageId}' in questline '{id}' must be reachable from firstStageId '{def.firstStageId}'.");
                }
            }
        }

        [Fact]
        public void NewQuestlines_HaveItemGrants_AndFactionStandingShifts()
        {
            var (system, _) = LoadSystemAndCatalog();
            var validFactions = new HashSet<string>(StringComparer.Ordinal)
            {
                "faction_the_tempest",
                "faction_archivists",
                "faction_ash_militia",
                "faction_scavengers",
                "faction_central_garrison",
                "faction_ash_sign"
            };

            foreach (string id in s_sevenNewQuestlineIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);

                bool hasItemGrant = false;
                bool hasFactionDelta = false;

                foreach (var stage in def.stages)
                {
                    foreach (var choice in stage.choices)
                    {
                        if (!string.IsNullOrEmpty(choice.grantItemId))
                        {
                            hasItemGrant = true;
                            Assert.True(choice.grantItemQuantity > 0,
                                $"Choice '{choice.choiceId}' in questline '{id}' has grantItemId '{choice.grantItemId}' but quantity is 0.");
                        }

                        if (choice.factionStandingDelta != 0)
                        {
                            hasFactionDelta = true;
                            Assert.True(validFactions.Contains(choice.targetFactionId),
                                $"Choice '{choice.choiceId}' targetFactionId '{choice.targetFactionId}' must be a recognized faction.");
                        }
                    }
                }

                Assert.True(hasItemGrant, $"Questline '{id}' must have at least one choice granting an item.");
                Assert.True(hasFactionDelta, $"Questline '{id}' must have at least one choice altering faction standing.");
            }
        }

        [Fact]
        public void NewQuestlines_DayWindowsAreOrdered_AndWithinRange()
        {
            var (system, _) = LoadSystemAndCatalog();
            foreach (string id in s_sevenNewQuestlineIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);

                Assert.InRange(def.minDay, 160, 360);
                Assert.InRange(def.maxDay, 160, 360);
                Assert.True(def.minDay <= def.maxDay,
                    $"Questline '{id}' minDay {def.minDay} must be <= maxDay {def.maxDay}.");

                foreach (var stage in def.stages)
                {
                    Assert.InRange(stage.unlockOnDay, def.minDay, def.maxDay);
                }
            }
        }

        [Fact]
        public void NewQuestlines_SimulatedResolution_ExecutesToCompletion()
        {
            var (system, _) = LoadSystemAndCatalog();

            foreach (string id in s_sevenNewQuestlineIds)
            {
                var def = system.FindDefinition(id);
                Assert.NotNull(def);

                // Start the questline
                Assert.True(system.StartQuestline(id, def.minDay));
                var rec = system.GetActiveRecord(id);
                Assert.NotNull(rec);
                Assert.Equal(def.firstStageId, rec.currentStageId);

                // Step through the first choice of each stage until terminal
                int stepLimit = 20;
                int steps = 0;
                while (rec.status == QuestlineStatus.Active && steps++ < stepLimit)
                {
                    var currentStage = def.FindStage(rec.currentStageId);
                    Assert.NotNull(currentStage);
                    var choice = currentStage.choices[0];

                    var result = system.TakeChoice(id, choice.choiceId, def.minDay + steps);
                    Assert.NotNull(result);
                    Assert.Equal(choice.choiceId, result.choiceId);
                    Assert.Equal(choice.nextStageId, result.nextStageId);
                }

                Assert.Equal(QuestlineStatus.Completed, rec.status);
                Assert.Contains(id, system.CaptureState().completedQuestlineIds);
            }
        }
    }
}
