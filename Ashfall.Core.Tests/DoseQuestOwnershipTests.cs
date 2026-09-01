using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins Dose quest ownership: DoseLedgerSave (v2+) is the single persisted
    /// owner of the four canonical Dose quest lines; the Year of Ash envelope is
    /// no longer a second owner, and pre-v2 quest progress carried by an older
    /// Year of Ash save is adopted into the Dose envelope without loss.
    /// </summary>
    public class DoseQuestOwnershipTests
    {
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        private static QuestlineSystem BuildQuestsWithProgress(out string questlineId)
        {
            questlineId = "quest_the_dose_the_first_reading";
            var system = new QuestlineSystem();
            system.RegisterQuestline(new QuestlineDefinition
            {
                questlineId = questlineId,
                title = "Selftest Dose Quest",
                firstStageId = "stage_dose_selftest_first",
                minDay = 180,
                maxDay = 360,
                stages = new List<QuestStage>
                {
                    new QuestStage
                    {
                        stageId = "stage_dose_selftest_first",
                        title = "First",
                        choices = new List<QuestChoice>
                        {
                            new QuestChoice
                            {
                                choiceId = "choice_dose_selftest_advance",
                                text = "Advance",
                                nextStageId = "stage_dose_selftest_terminal",
                                moraleDelta = 1
                            }
                        }
                    },
                    new QuestStage
                    {
                        stageId = "stage_dose_selftest_terminal",
                        title = "Terminal",
                        isTerminal = true,
                        terminalOutcome = QuestlineStatus.Completed,
                        choices = new List<QuestChoice>()
                    }
                }
            });
            Assert.True(system.StartQuestline(questlineId, 185), "questline starts");
            var result = system.TakeChoice(questlineId, "choice_dose_selftest_advance", 190);
            Assert.NotNull(result);
            return system;
        }

        [Fact]
        public void DoseSaveV2_RoundTripsQuestProgress()
        {
            var quests = BuildQuestsWithProgress(out string qid);
            var save = DoseLedgerSaveCodec.Capture(
                40, new DoseLedgerSystem(), new SickListSystem(), new CohortSystem(),
                new VoluntaryRegisterSystem(), quests);
            Assert.Equal(2, save.saveVersion);
            Assert.True(save.quests.active.Exists(a => a.questlineId == qid),
                "quest progress captured in the Dose envelope");
            Assert.Contains(qid, save.quests.completedQuestlineIds);

            string encoded = DoseLedgerSaveCodec.Encode(save, s_json);
            var loaded = DoseLedgerSaveCodec.Decode(encoded, s_json);
            var restored = new QuestlineSystem();
            DoseLedgerSaveCodec.Restore(loaded, new DoseLedgerSystem(), new SickListSystem(),
                new CohortSystem(), new VoluntaryRegisterSystem(), restored);
            Assert.True(restored.State.active.Exists(a => a.questlineId == qid));
            Assert.Contains(qid, restored.State.completedQuestlineIds);
            Assert.Equal(1, restored.State.totalMoraleDeltaFromQuests);
        }

        [Fact]
        public void GenuineV1Save_MigratesToV2_WithEmptyQuestSection()
        {
            // A genuine v1 save: hashed over the v1 field set (no quests key).
            var v1 = new DoseLedgerSaveV1
            {
                saveVersion = 1,
                simDay = 40,
                doseLedger = new DoseLedgerSystemState(),
                sickList = new SickListSystemState(),
                cohort = new CohortSystemState(),
                voluntaryRegister = new VoluntaryRegisterSystemState()
            };
            v1.Checksum = SaveChecksum.Compute(v1);
            string json = s_json.Serialize(v1);
            Assert.DoesNotContain("\"quests\"", json, System.StringComparison.Ordinal);

            var migrated = DoseLedgerSaveCodec.Decode(json, s_json);
            Assert.Equal(2, migrated.saveVersion);
            Assert.NotNull(migrated.quests);
            Assert.Empty(migrated.quests.active);
            Assert.Equal(40, migrated.simDay);
        }

        [Fact]
        public void AdoptFromYearOfAsh_CopiesOnlyDoseQuestRecords()
        {
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_the_sick_of_room_seven", currentStageId = "stage_sick_first", status = QuestlineStatus.Active, dayStarted = 200 },
                    new ActiveQuestlineRecord { questlineId = "quest_garrison_blood_debt", currentStageId = "stage_blood_debt_demand", status = QuestlineStatus.Active, dayStarted = 185 }
                },
                completedQuestlineIds = new List<string> { "quest_the_signed_hour", "quest_seed_vault" },
                failedQuestlineIds = new List<string> { "quest_the_childs_number" }
            };
            var dose = new QuestlineSystemState();

            int adopted = DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh);
            Assert.Equal(3, adopted); // sick active + signed completed + child failed
            Assert.Single(dose.active);
            Assert.Equal("quest_the_sick_of_room_seven", dose.active[0].questlineId);
            Assert.Contains("quest_the_signed_hour", dose.completedQuestlineIds);
            Assert.Contains("quest_the_childs_number", dose.failedQuestlineIds);
            Assert.DoesNotContain("quest_garrison_blood_debt", dose.active.ConvertAll(a => a.questlineId));
            Assert.DoesNotContain("quest_seed_vault", dose.completedQuestlineIds);
        }

        [Fact]
        public void AdoptFromYearOfAsh_DoseWinsOnConflict()
        {
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_the_dose_the_first_reading", currentStageId = "stage_old", status = QuestlineStatus.Active, dayStarted = 100 }
                }
            };
            var dose = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_the_dose_the_first_reading", currentStageId = "stage_new", status = QuestlineStatus.Active, dayStarted = 170 }
                }
            };

            int adopted = DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh);
            Assert.Equal(0, adopted);
            Assert.Single(dose.active);
            Assert.Equal("stage_new", dose.active[0].currentStageId); // Dose state wins
        }

        [Fact]
        public void StripFromYearOfAsh_RemovesOnlyDoseQuestRecords()
        {
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_the_sick_of_room_seven", currentStageId = "stage_sick_first", status = QuestlineStatus.Active, dayStarted = 200 },
                    new ActiveQuestlineRecord { questlineId = "quest_garrison_blood_debt", currentStageId = "stage_blood_debt_demand", status = QuestlineStatus.Active, dayStarted = 185 }
                },
                completedQuestlineIds = new List<string> { "quest_the_signed_hour", "quest_seed_vault" },
                failedQuestlineIds = new List<string> { "quest_the_childs_number" }
            };

            int removed = DoseQuestMigration.StripFromYearOfAsh(yearOfAsh);
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
            var yearOfAsh = new QuestlineSystemState
            {
                active = new List<ActiveQuestlineRecord>
                {
                    new ActiveQuestlineRecord { questlineId = "quest_the_sick_of_room_seven", currentStageId = "stage_sick_first", status = QuestlineStatus.Active, dayStarted = 200 },
                    new ActiveQuestlineRecord { questlineId = "quest_garrison_blood_debt", currentStageId = "stage_blood_debt_demand", status = QuestlineStatus.Active, dayStarted = 185 }
                }
            };
            var dose = new QuestlineSystemState();

            Assert.Equal(1, DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh));
            Assert.Single(dose.active);
            Assert.Equal("quest_the_sick_of_room_seven", dose.active[0].questlineId);
            Assert.Equal(1, DoseQuestMigration.StripFromYearOfAsh(yearOfAsh));
            Assert.Single(yearOfAsh.active);
            Assert.Equal("quest_garrison_blood_debt", yearOfAsh.active[0].questlineId);
        }

        [Fact]
        public void DoseQuestCatalogs_RemainReachable()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var content = DoseContentCatalogLoader.Load(dataDir, io, s_json);
            Assert.NotNull(content);
            int registered = 0;
            var system = new QuestlineSystem();
            foreach (var q in content.quests)
            {
                if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
                Assert.True(DoseQuestMigration.IsDoseQuestline(q.questlineId),
                    "authored dose questline must be canonical: " + q.questlineId);
                system.RegisterQuestline(q);
                registered++;
            }
            Assert.Equal(12, registered);
            Assert.NotNull(system.FindDefinition("quest_the_dose_the_first_reading"));
        }
    }
}
