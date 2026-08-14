using System;
using System.Linq;
using Xunit;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// QA reviewer tests for QuestlineSystem.
    /// Written independently from the implementation — only the spec and the diff.
    /// Covers: registration, lifecycle, branching, terminal outcomes, state capture.
    /// </summary>
    public class QuestlineSystemTests
    {
        // ─── Helpers ───────────────────────────────────────────────────────────────

        private static QuestlineDefinition MakeSimpleQuest(
            string id = "quest_simple",
            int minDay = 100,
            int maxDay = 200)
        {
            var def = new QuestlineDefinition
            {
                questlineId  = id,
                title        = "Simple Quest",
                synopsis     = "A two-stage test questline.",
                firstStageId = "stage_a",
                minDay       = minDay,
                maxDay       = maxDay
            };

            def.stages.Add(new QuestStage
            {
                stageId        = "stage_a",
                title          = "Stage A",
                narrativePrompt = "You face a choice.",
                unlockOnDay    = 100,
                choices        = new System.Collections.Generic.List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId         = "choice_a_good",
                        text             = "The compassionate path.",
                        nextStageId      = "stage_b",
                        moraleDelta      = 15,
                        guiltDelta       = 0,
                        targetFactionId  = "faction_rebuilders",
                        factionStandingDelta = 10,
                        outcomeNarrative = "You choose well."
                    },
                    new QuestChoice
                    {
                        choiceId         = "choice_a_ruthless",
                        text             = "The ruthless path.",
                        nextStageId      = "",   // terminal immediately
                        moraleDelta      = -20,
                        guiltDelta       = 25,
                        outcomeNarrative = "The shelter endures. Others do not."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_b",
                title           = "Stage B — Resolution",
                narrativePrompt = "The consequence arrives.",
                unlockOnDay     = 110,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new System.Collections.Generic.List<QuestChoice>()
            });

            return def;
        }

        private static QuestlineDefinition MakeFailQuest(string id = "quest_fail")
        {
            var def = new QuestlineDefinition
            {
                questlineId  = id,
                title        = "Fail Quest",
                firstStageId = "stage_only",
                minDay       = 100,
                maxDay       = 300
            };
            def.stages.Add(new QuestStage
            {
                stageId         = "stage_only",
                narrativePrompt = "Last chance.",
                isTerminal      = false,
                choices         = new System.Collections.Generic.List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId         = "choice_fail",
                        text             = "Fail.",
                        nextStageId      = "stage_fail_terminal",
                        moraleDelta      = -5,
                        guiltDelta       = 0,
                        outcomeNarrative = "It fails."
                    }
                }
            });
            def.stages.Add(new QuestStage
            {
                stageId         = "stage_fail_terminal",
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Failed,
                choices         = new System.Collections.Generic.List<QuestChoice>()
            });
            return def;
        }

        // ─── Registration ──────────────────────────────────────────────────────────

        [Fact]
        public void RegisterQuestline_AddsToInternalCatalog()
        {
            var sys = new QuestlineSystem();
            var extra = MakeSimpleQuest("quest_extra");
            sys.RegisterQuestline(extra);

            var found = sys.FindDefinition("quest_extra");
            Assert.NotNull(found);
            Assert.Equal("quest_extra", found.questlineId);
        }

        [Fact]
        public void RegisterQuestline_NoDuplicates()
        {
            var sys = new QuestlineSystem();
            var q = MakeSimpleQuest("quest_dup");
            sys.RegisterQuestline(q);
            sys.RegisterQuestline(q); // second add should be ignored

            int count = sys.Catalog.Count(c => c.questlineId == "quest_dup");
            Assert.Equal(1, count);
        }

        [Fact]
        public void RegisterQuestline_NullIsSafe()
        {
            var sys = new QuestlineSystem();
            // Must not throw
            sys.RegisterQuestline(null);
        }

        // ─── Built-in catalog ──────────────────────────────────────────────────────

        [Fact]
        public void BuiltInCatalog_HasEightQuestlines()
        {
            var sys = new QuestlineSystem();
            Assert.Equal(8, sys.Catalog.Count);
        }

        [Fact]
        public void BuiltInCatalog_AllQuestlinesHaveFirstStage()
        {
            var sys = new QuestlineSystem();
            foreach (var def in sys.Catalog)
            {
                Assert.False(string.IsNullOrEmpty(def.firstStageId),
                    $"{def.questlineId} has no firstStageId");

                var firstStage = def.FindStage(def.firstStageId);
                Assert.NotNull(firstStage);
            }
        }

        [Fact]
        public void BuiltInCatalog_AllQuestlinesHaveValidDayRanges()
        {
            var sys = new QuestlineSystem();
            foreach (var def in sys.Catalog)
            {
                Assert.True(def.minDay >= 180 && def.minDay <= 360,
                    $"{def.questlineId} minDay={def.minDay} is outside 180-360");
                Assert.True(def.maxDay > def.minDay,
                    $"{def.questlineId} maxDay must be > minDay");
            }
        }

        [Fact]
        public void BuiltInCatalog_GarrisonBloodDebt_Exists()
        {
            var sys = new QuestlineSystem();
            Assert.NotNull(sys.FindDefinition("quest_garrison_blood_debt"));
        }

        [Fact]
        public void BuiltInCatalog_TheLastBroadcast_Exists()
        {
            var sys = new QuestlineSystem();
            Assert.NotNull(sys.FindDefinition("quest_the_last_broadcast"));
        }

        // ─── Availability ──────────────────────────────────────────────────────────

        [Fact]
        public void GetAvailableQuestlines_ReturnsQuestlinesInDayWindow()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest("quest_simple", minDay: 150, maxDay: 200));

            var available = sys.GetAvailableQuestlines(175);
            Assert.Contains(available, q => q.questlineId == "quest_simple");
        }

        [Fact]
        public void GetAvailableQuestlines_ExcludesOutOfWindowQuests()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest("quest_future", minDay: 300, maxDay: 360));

            var available = sys.GetAvailableQuestlines(100);
            Assert.DoesNotContain(available, q => q.questlineId == "quest_future");
        }

        [Fact]
        public void GetAvailableQuestlines_ExcludesAlreadyActive()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest("quest_simple", minDay: 100, maxDay: 200));
            sys.StartQuestline("quest_simple", 120);

            var available = sys.GetAvailableQuestlines(120);
            Assert.DoesNotContain(available, q => q.questlineId == "quest_simple");
        }

        [Fact]
        public void GetAvailableQuestlines_ExcludesCompleted()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest("quest_simple", minDay: 100, maxDay: 200));
            sys.StartQuestline("quest_simple", 100);
            // Take ruthless path → ends immediately (no nextStageId)
            sys.TakeChoice("quest_simple", "choice_a_ruthless", 100);

            var available = sys.GetAvailableQuestlines(150);
            Assert.DoesNotContain(available, q => q.questlineId == "quest_simple");
        }

        // ─── Start ────────────────────────────────────────────────────────────────

        [Fact]
        public void StartQuestline_ReturnsTrue_AndCreatesActiveRecord()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());

            bool started = sys.StartQuestline("quest_simple", 100);

            Assert.True(started);
            var record = sys.GetActiveRecord("quest_simple");
            Assert.NotNull(record);
            Assert.Equal(QuestlineStatus.Active, record.status);
            Assert.Equal("stage_a", record.currentStageId);
            Assert.Equal(100, record.dayStarted);
        }

        [Fact]
        public void StartQuestline_ReturnsFalse_IfAlreadyActive()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);
            bool second = sys.StartQuestline("quest_simple", 101);

            Assert.False(second);
        }

        [Fact]
        public void StartQuestline_ReturnsFalse_ForUnknownId()
        {
            var sys = new QuestlineSystem();
            bool result = sys.StartQuestline("quest_nonexistent", 100);
            Assert.False(result);
        }

        [Fact]
        public void StartQuestline_FiresOnQuestlineStartedEvent()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());

            QuestlineDefinition? eventDef = null;
            sys.OnQuestlineStarted += def => eventDef = def;

            sys.StartQuestline("quest_simple", 100);
            Assert.NotNull(eventDef);
            Assert.Equal("quest_simple", eventDef.questlineId);
        }

        // ─── TakeChoice ───────────────────────────────────────────────────────────

        [Fact]
        public void TakeChoice_ReturnsNull_IfQuestNotActive()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());

            var result = sys.TakeChoice("quest_simple", "choice_a_good", 100);
            Assert.Null(result);
        }

        [Fact]
        public void TakeChoice_ReturnsNull_ForInvalidChoiceId()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);

            var result = sys.TakeChoice("quest_simple", "choice_bogus", 100);
            Assert.Null(result);
        }

        [Fact]
        public void TakeChoice_GoodPath_AdvancesToNextStage()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);

            var result = sys.TakeChoice("quest_simple", "choice_a_good", 105);

            Assert.NotNull(result);
            Assert.Equal("stage_a", result.stageId);
            Assert.Equal("stage_b", result.nextStageId);
            Assert.Equal(QuestlineStatus.Completed, result.newQuestStatus);

            // Record should be resolved
            var record = sys.GetActiveRecord("quest_simple");
            Assert.Equal(QuestlineStatus.Completed, record.status);
            Assert.Equal(105, record.dayResolved);
        }

        [Fact]
        public void TakeChoice_RuthlessPath_ResolvesImmediately()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);

            var result = sys.TakeChoice("quest_simple", "choice_a_ruthless", 100);

            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.nextStageId);
            Assert.Equal(-20, result.moraleDelta);
            Assert.Equal(25, result.guiltDelta);
        }

        [Fact]
        public void TakeChoice_RecordsChoiceInHistory()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);
            sys.TakeChoice("quest_simple", "choice_a_good", 100);

            var record = sys.GetActiveRecord("quest_simple");
            Assert.Single(record.choiceHistory);
            Assert.Equal("choice_a_good", record.choiceHistory[0]);
        }

        [Fact]
        public void TakeChoice_AccumulatesMoraleAndGuilt()
        {
            var sys = new QuestlineSystem();
            var q = MakeSimpleQuest("quest_m");
            sys.RegisterQuestline(q);
            sys.StartQuestline("quest_m", 100);
            sys.TakeChoice("quest_m", "choice_a_good", 100);

            Assert.Equal(15, sys.State.totalMoraleDeltaFromQuests);
            Assert.Equal(0, sys.State.totalGuiltDeltaFromQuests);
        }

        [Fact]
        public void TakeChoice_FiresOnQuestChoiceTakenEvent()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);

            QuestChoiceResult? eventResult = null;
            sys.OnQuestChoiceTaken += r => eventResult = r;

            sys.TakeChoice("quest_simple", "choice_a_good", 100);
            Assert.NotNull(eventResult);
            Assert.Equal("choice_a_good", eventResult.choiceId);
        }

        [Fact]
        public void TakeChoice_FiresOnQuestlineResolved_WhenTerminal()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);

            string? resolvedId = null;
            QuestlineStatus resolvedStatus = QuestlineStatus.NotStarted;
            sys.OnQuestlineResolved += (id, status) => { resolvedId = id; resolvedStatus = status; };

            sys.TakeChoice("quest_simple", "choice_a_good", 100);

            Assert.Equal("quest_simple", resolvedId);
            Assert.Equal(QuestlineStatus.Completed, resolvedStatus);
        }

        [Fact]
        public void TakeChoice_CannotActOnResolvedQuestline()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);
            sys.TakeChoice("quest_simple", "choice_a_ruthless", 100);

            // Quest is now resolved — second call should return null
            var result = sys.TakeChoice("quest_simple", "choice_a_good", 101);
            Assert.Null(result);
        }

        // ─── Failed terminal ───────────────────────────────────────────────────────

        [Fact]
        public void TakeChoice_FailedTerminal_RecordsAsFailure()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeFailQuest());
            sys.StartQuestline("quest_fail", 100);

            var result = sys.TakeChoice("quest_fail", "choice_fail", 100);

            Assert.NotNull(result);
            Assert.Equal(QuestlineStatus.Failed, result.newQuestStatus);
            Assert.Contains("quest_fail", sys.State.failedQuestlineIds);
            Assert.DoesNotContain("quest_fail", sys.State.completedQuestlineIds);
        }

        // ─── State capture / restore ───────────────────────────────────────────────

        [Fact]
        public void CaptureState_IsDeepCopy()
        {
            var sys = new QuestlineSystem();
            sys.RegisterQuestline(MakeSimpleQuest());
            sys.StartQuestline("quest_simple", 100);
            sys.TakeChoice("quest_simple", "choice_a_good", 100);

            var snap1 = sys.CaptureState();

            // Mutate original
            sys.RegisterQuestline(MakeFailQuest());
            sys.StartQuestline("quest_fail", 101);
            sys.TakeChoice("quest_fail", "choice_fail", 101);

            var snap2 = sys.CaptureState();

            // snap1 must not reflect changes made after capture
            Assert.Equal(1, snap1.completedQuestlineIds.Count);
            Assert.Equal(0, snap1.failedQuestlineIds.Count);

            Assert.Equal(1, snap2.completedQuestlineIds.Count);
            Assert.Equal(1, snap2.failedQuestlineIds.Count);
        }

        [Fact]
        public void CaptureState_CanRestoreSystemFromState()
        {
            var sys1 = new QuestlineSystem();
            sys1.RegisterQuestline(MakeSimpleQuest());
            sys1.StartQuestline("quest_simple", 100);
            sys1.TakeChoice("quest_simple", "choice_a_ruthless", 100);

            var savedState = sys1.CaptureState();

            // Reconstruct from saved state
            var sys2 = new QuestlineSystem(savedState);

            Assert.Contains("quest_simple", sys2.State.completedQuestlineIds);
            Assert.Equal(-20, sys2.State.totalMoraleDeltaFromQuests);
            Assert.Equal(25, sys2.State.totalGuiltDeltaFromQuests);
        }

        // ─── Garrison Blood Debt integration smoke test ────────────────────────────

        [Fact]
        public void GarrisonBloodDebt_CompassionatePath_ReachesProtectedResolution()
        {
            var sys = new QuestlineSystem();
            // Built-in catalog already has it
            sys.StartQuestline("quest_garrison_blood_debt", 186);

            var r1 = sys.TakeChoice("quest_garrison_blood_debt", "choice_confront_ola", 186);
            Assert.NotNull(r1);
            Assert.Equal("stage_blood_debt_ola_testimony", r1.nextStageId);
            Assert.Equal(QuestlineStatus.Active, r1.newQuestStatus);

            var r2 = sys.TakeChoice("quest_garrison_blood_debt", "choice_send_ola_underground", 190);
            Assert.NotNull(r2);
            Assert.Equal("stage_blood_debt_garrison_search", r2.nextStageId);
            Assert.Equal(QuestlineStatus.Active, r2.newQuestStatus);

            var r3 = sys.TakeChoice("quest_garrison_blood_debt", "choice_scatter_rad_bait", 206);
            Assert.NotNull(r3);
            Assert.Equal("stage_blood_debt_resolution_protected", r3.nextStageId);
            // Terminal stage → should resolve
            Assert.Equal(QuestlineStatus.Completed, r3.newQuestStatus);
        }

        [Fact]
        public void GarrisonBloodDebt_ComplyCausesHighGuiltDelta()
        {
            var sys = new QuestlineSystem();
            sys.StartQuestline("quest_garrison_blood_debt", 186);

            var r = sys.TakeChoice("quest_garrison_blood_debt", "choice_comply_immediately", 186);
            Assert.NotNull(r);
            Assert.Equal(40, r.guiltDelta);
            Assert.Equal(-30, r.moraleDelta);
        }

        // ─── The Last Broadcast smoke test ─────────────────────────────────────────

        [Fact]
        public void TheLastBroadcast_DenyingAntennaEndsQuestWithHighGuilt()
        {
            var sys = new QuestlineSystem();
            sys.StartQuestline("quest_the_last_broadcast", 320);

            var r = sys.TakeChoice("quest_the_last_broadcast", "choice_deny_antenna_access", 320);
            Assert.NotNull(r);
            Assert.Equal(40, r.guiltDelta);
            Assert.Equal(-30, r.moraleDelta);
            Assert.Equal(QuestlineStatus.Completed, r.newQuestStatus); // no next stage → completed by default
        }

        [Fact]
        public void TheLastBroadcast_RequestArchive_GrantsItem()
        {
            var sys = new QuestlineSystem();
            sys.StartQuestline("quest_the_last_broadcast", 320);

            var r = sys.TakeChoice("quest_the_last_broadcast", "choice_request_copy_of_archive", 320);
            Assert.NotNull(r);
            Assert.Equal("item_meridian_archive_copy", r.grantItemId);
            Assert.Equal(1, r.grantItemQty);
        }

        // ─── Ash Sign Revelation smoke test ───────────────────────────────────────

        [Fact]
        public void AshSignRevelation_BuryingEvidenceLeadsToFailure()
        {
            var sys = new QuestlineSystem();
            sys.StartQuestline("quest_ash_sign_revelation", 220);

            var r1 = sys.TakeChoice("quest_ash_sign_revelation", "choice_verify_documents", 220);
            Assert.NotNull(r1);

            var r2 = sys.TakeChoice("quest_ash_sign_revelation", "choice_bury_evidence", 226);
            Assert.NotNull(r2);
            Assert.Equal("stage_revelation_buried", r2.nextStageId);
            Assert.Equal(QuestlineStatus.Failed, r2.newQuestStatus);
        }
    }
}
