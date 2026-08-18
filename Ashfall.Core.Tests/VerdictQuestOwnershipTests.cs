using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins Verdict quest ownership: VerdictSave (v3+) is the single persisted
    /// owner of quest_verdict_* progress; the Year of Ash envelope is no longer a
    /// second owner, and pre-v3 quest progress carried by an older Year of Ash
    /// save is adopted into the Verdict envelope without loss.
    /// </summary>
    public class VerdictQuestOwnershipTests
    {
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        private static QuestlineSystem BuildQuestsWithProgress(out string questlineId)
        {
            questlineId = "quest_verdict_selftest";
            var system = new QuestlineSystem();
            system.RegisterQuestline(new QuestlineDefinition
            {
                questlineId = questlineId,
                title = "Selftest Quest",
                firstStageId = "stage_selftest_first",
                minDay = 160,
                maxDay = 360,
                stages = new List<QuestStage>
                {
                    new QuestStage
                    {
                        stageId = "stage_selftest_first",
                        title = "First",
                        choices = new List<QuestChoice>
                        {
                            new QuestChoice
                            {
                                choiceId = "choice_selftest_advance",
                                text = "Advance",
                                nextStageId = "stage_selftest_terminal",
                                moraleDelta = 1
                            }
                        }
                    },
                    new QuestStage
                    {
                        stageId = "stage_selftest_terminal",
                        title = "Terminal",
                        isTerminal = true,
                        terminalOutcome = QuestlineStatus.Completed,
                        choices = new List<QuestChoice>()
                    }
                }
            });
            Assert.True(system.StartQuestline(questlineId, 170), "questline starts");
            var result = system.TakeChoice(questlineId, "choice_selftest_advance", 175);
            Assert.NotNull(result);
            return system;
        }

        [Fact]
        public void VerdictSaveV3_RoundTripsQuestProgress()
        {
            var quests = BuildQuestsWithProgress(out string qid);
            var save = VerdictSaveCodec.Capture(
                241, new MachineLogSystem(), new ReckoningSystem(), new EvidenceLedger(), -1,
                npcs: new VerdictNpcSystem(), quests: quests);
            Assert.Equal(3, save.saveVersion);
            Assert.True(save.quests.active.Exists(a => a.questlineId == qid),
                "quest progress captured in the Verdict envelope");
            Assert.Contains(qid, save.quests.completedQuestlineIds);

            string encoded = VerdictSaveCodec.Encode(save, s_json);
            Assert.True(VerdictSaveCodec.TryDecode(encoded, s_json, out var loaded));
            var restored = new QuestlineSystem();
            VerdictSaveCodec.Restore(loaded, new MachineLogSystem(), new ReckoningSystem(),
                new EvidenceLedger(), npcs: new VerdictNpcSystem(), quests: restored);
            Assert.True(restored.State.active.Exists(a => a.questlineId == qid));
            Assert.Contains(qid, restored.State.completedQuestlineIds);
            Assert.Equal(1, restored.State.totalMoraleDeltaFromQuests);
        }

        [Fact]
        public void AdoptFromYearOfAsh_CopiesOnlyVerdictQuestRecords()
        {
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_verdict_warm_range", currentStageId = "stage_warm_path", status = QuestlineStatus.Active, dayStarted = 180 },
                    new ActiveQuestlineRecord { questlineId = "quest_garrison_blood_debt", currentStageId = "stage_blood_debt_demand", status = QuestlineStatus.Active, dayStarted = 185 }
                },
                completedQuestlineIds = new List<string> { "quest_verdict_count_held", "quest_seed_vault" },
                failedQuestlineIds = new List<string> { "quest_verdict_broken_relay" }
            };
            var verdict = new QuestlineSystemState();

            int adopted = VerdictQuestMigration.AdoptFromYearOfAsh(verdict, yearOfAsh);
            Assert.Equal(3, adopted); // warm_range active + count_held completed + broken_relay failed
            Assert.Single(verdict.active);
            Assert.Equal("quest_verdict_warm_range", verdict.active[0].questlineId);
            Assert.Contains("quest_verdict_count_held", verdict.completedQuestlineIds);
            Assert.Contains("quest_verdict_broken_relay", verdict.failedQuestlineIds);
            // Non-Verdict records are never adopted.
            Assert.DoesNotContain("quest_garrison_blood_debt", verdict.active.ConvertAll(a => a.questlineId));
            Assert.DoesNotContain("quest_seed_vault", verdict.completedQuestlineIds);
        }

        [Fact]
        public void AdoptFromYearOfAsh_VerdictWinsOnConflict()
        {
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_verdict_warm_range", currentStageId = "stage_old", status = QuestlineStatus.Active, dayStarted = 100 }
                }
            };
            var verdict = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_verdict_warm_range", currentStageId = "stage_new", status = QuestlineStatus.Active, dayStarted = 170 }
                }
            };

            int adopted = VerdictQuestMigration.AdoptFromYearOfAsh(verdict, yearOfAsh);
            Assert.Equal(0, adopted);
            Assert.Single(verdict.active);
            Assert.Equal("stage_new", verdict.active[0].currentStageId); // Verdict state wins
        }

        [Fact]
        public void StripFromYearOfAsh_RemovesOnlyVerdictQuestRecords()
        {
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_verdict_warm_range", currentStageId = "stage_warm_path", status = QuestlineStatus.Active, dayStarted = 180 },
                    new ActiveQuestlineRecord { questlineId = "quest_garrison_blood_debt", currentStageId = "stage_blood_debt_demand", status = QuestlineStatus.Active, dayStarted = 185 }
                },
                completedQuestlineIds = new List<string> { "quest_verdict_count_held", "quest_seed_vault" },
                failedQuestlineIds = new List<string> { "quest_verdict_broken_relay" }
            };

            int removed = VerdictQuestMigration.StripFromYearOfAsh(yearOfAsh);
            Assert.Equal(3, removed);
            Assert.Single(yearOfAsh.active);
            Assert.Equal("quest_garrison_blood_debt", yearOfAsh.active[0].questlineId);
            Assert.Single(yearOfAsh.completedQuestlineIds);
            Assert.Contains("quest_seed_vault", yearOfAsh.completedQuestlineIds);
            Assert.Empty(yearOfAsh.failedQuestlineIds);
        }

        [Fact]
        public void AdoptThenStrip_LeavesOneOwnerOnly()
        {
            // Simulates the upgrade path: legacy YearOfAsh save carries Verdict
            // quest progress; after adoption + strip, only the Verdict envelope
            // holds it and the YearOfAsh envelope is clean.
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_verdict_warm_range", currentStageId = "stage_warm_path", status = QuestlineStatus.Active, dayStarted = 180 },
                    new ActiveQuestlineRecord { questlineId = "quest_garrison_blood_debt", currentStageId = "stage_blood_debt_demand", status = QuestlineStatus.Active, dayStarted = 185 }
                }
            };
            var verdict = new QuestlineSystemState();

            int adopted = VerdictQuestMigration.AdoptFromYearOfAsh(verdict, yearOfAsh);
            Assert.Equal(1, adopted);
            Assert.Single(verdict.active);
            Assert.Equal("quest_verdict_warm_range", verdict.active[0].questlineId);

            int stripped = VerdictQuestMigration.StripFromYearOfAsh(yearOfAsh);
            Assert.Equal(1, stripped);
            Assert.Single(yearOfAsh.active);
            Assert.Equal("quest_garrison_blood_debt", yearOfAsh.active[0].questlineId);
        }

        [Fact]
        public void VerdictQuestCatalogs_RemainReachable()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var system = new QuestlineSystem();
            int registered = VerdictQuestCatalogLoader.LoadAndRegister(system, dataDir, io, s_json);
            Assert.True(registered > 0, "verdict_questlines.json must register questlines");
            Assert.NotNull(system.FindDefinition("quest_verdict_the_warm_range"));
        }
    }
}
