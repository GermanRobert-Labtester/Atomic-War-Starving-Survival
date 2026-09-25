// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W7 — Quest reopen grammar (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W7-QUEST-REOPEN-GRAMMAR (cases AV.7).
//
// A campaign loses threads: quests fail, expire, or are abandoned. Nothing ever
// brought them back, even when a later discovery satisfied the same conditions.
// W7 gives the quest owner an exactly-once `Reopen` transition and an
// `Abandon` transition (the state existed with no way in), and keeps the
// eligibility decision in the host where authored prerequisites and the flag
// ledger actually live.
// ============================================================================

using System;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class QuestReopenGrammarTests
    {
        private static QuestRuntimeCoordinator Fresh()
            => new QuestRuntimeCoordinator();

        private static QuestInstanceState Register(QuestRuntimeCoordinator q, string id, string title = "Thread")
        {
            q.Register(new QuestInstanceState
            {
                instanceId = id,
                definitionId = "def_" + id,
                titleKey = title,
                createdDay = 1,
                status = QuestLifecycleState.Active
            });
            return q.Find(id)!;
        }

        // ── The grammar itself (AV.7 cases 1–3) ─────────────────────────────

        [Fact]
        public void FailedQuest_Reopens_WhenEligible()
        {
            var q = Fresh();
            var quest = Register(q, "q_a");
            Assert.True(q.Fail("q_a"));
            Assert.Equal(QuestLifecycleState.Failed, quest.status);

            Assert.True(q.Reopen("q_a", day: 5, eligibility: _ => true));
            Assert.Equal(QuestLifecycleState.Active, quest.status);
            Assert.True(quest.reopenedOnce);
            Assert.Equal(5, quest.reopenedDay);
        }

        [Fact]
        public void AbandonedQuest_Reopens()
        {
            var q = Fresh();
            var quest = Register(q, "q_b");
            Assert.True(q.Abandon("q_b"));
            Assert.Equal(QuestLifecycleState.Abandoned, quest.status);
            Assert.True(q.Reopen("q_b", day: 9, eligibility: _ => true));
            Assert.Equal(QuestLifecycleState.Active, quest.status);
        }

        [Fact]
        public void Reopen_HappensAtMostOnce_Ever()
        {
            var q = Fresh();
            Register(q, "q_c");
            q.Fail("q_c");
            Assert.True(q.Reopen("q_c", 5, _ => true));
            // Fail it again: the one-shot guard still refuses.
            q.Fail("q_c");
            Assert.False(q.Reopen("q_c", 6, _ => true));
            Assert.Equal(1, q.Find("q_c")!.reopenedDay == 5 ? 1 : 0);
        }

        [Fact]
        public void Reopen_RespectsEligibility()
        {
            var q = Fresh();
            Register(q, "q_d");
            q.Fail("q_d");
            Assert.False(q.Reopen("q_d", 5, eligibility: _ => false));
            Assert.Equal(QuestLifecycleState.Failed, q.Find("q_d")!.status);
        }

        [Fact]
        public void NoEligibilityPredicate_StillReopensFailedThread()
        {
            // With no authored prerequisite, losing a thread should not be final.
            var q = Fresh();
            Register(q, "q_e");
            q.Fail("q_e");
            Assert.True(q.Reopen("q_e", 3));
        }

        [Fact]
        public void ActiveCompletedAndExpiredQuests_NeverReopen()
        {
            var q = Fresh();
            Register(q, "q_active");
            Assert.False(q.Reopen("q_active", 5, _ => true));

            Register(q, "q_done");
            q.Complete("q_done");   // completes because there are no objectives
            Assert.False(q.Reopen("q_done", 5, _ => true));

            var expiring = Register(q, "q_exp");
            expiring.expiryDay = 2;
            q.Tick(3);
            Assert.Equal(QuestLifecycleState.Expired, expiring.status);
            Assert.False(q.Reopen("q_exp", 5, _ => true));
        }

        [Fact]
        public void UnknownQuest_FailsClosed()
        {
            var q = Fresh();
            Assert.False(q.Reopen("nope", 5, _ => true));
        }

        // ── Candidate listing (the host's day-open evaluation) ───────────────

        [Fact]
        public void Candidates_ListsOnlyEligibleFailedOrAbandonedThreads()
        {
            var q = Fresh();
            Register(q, "q_failed"); q.Fail("q_failed");
            Register(q, "q_abandoned"); q.Abandon("q_abandoned");
            Register(q, "q_active");
            Register(q, "q_done"); q.Complete("q_done");
            q.Reopen("q_failed", 4, _ => true); // no longer a candidate

            var all = q.GetReopenCandidates();
            var ids = new System.Collections.Generic.List<string>();
            foreach (var x in all) ids.Add(x.instanceId);

            Assert.DoesNotContain("q_active", ids);
            Assert.DoesNotContain("q_done", ids);
            Assert.DoesNotContain("q_failed", ids);
            Assert.Contains("q_abandoned", ids);
        }

        [Fact]
        public void Candidates_RespectsThePredicate()
        {
            var q = Fresh();
            Register(q, "q_yes"); q.Fail("q_yes");
            Register(q, "q_no"); q.Fail("q_no");

            var filtered = q.GetReopenCandidates(x => x.instanceId == "q_yes");
            Assert.Single(filtered);
            Assert.Equal("q_yes", filtered[0].instanceId);
        }

        // ── Events, persistence, and lifecycle hygiene ──────────────────────

        [Fact]
        public void Events_RaiseOnceForAbandonAndReopen()
        {
            var q = Fresh();
            Register(q, "q_ev");
            int abandoned = 0, reopened = 0;
            q.OnQuestAbandoned += _ => abandoned++;
            q.OnQuestReopened += _ => reopened++;

            q.Abandon("q_ev");
            q.Reopen("q_ev", 7, _ => true);
            q.Reopen("q_ev", 8, _ => true); // refused — no second event

            Assert.Equal(1, abandoned);
            Assert.Equal(1, reopened);
        }

        [Fact]
        public void ReopenFlag_SurvivesSaveAndLoad()
        {
            var q = Fresh();
            Register(q, "q_save");
            q.Fail("q_save");
            q.Reopen("q_save", 6, _ => true);

            var restored = Fresh();
            restored.RestoreState(q.CaptureState());

            var quest = restored.Find("q_save")!;
            Assert.True(quest.reopenedOnce);
            Assert.Equal(6, quest.reopenedDay);
            // A reloaded save must not reopen the same thread again.
            restored.Fail("q_save");
            Assert.False(restored.Reopen("q_save", 9, _ => true));
        }

        [Fact]
        public void OldSavesWithoutTheField_DefaultToNeverReopened()
        {
            // Schema tolerance: an instance authored before W7 has reopenedOnce=false
            // and reopenedDay=0, which must read as "not yet reopened", not "used".
            var restored = Fresh();
            restored.RestoreState(new QuestRuntimeState { systemId = "quest_runtime" });
            var quest = Register(restored, "q_old");
            Assert.False(quest.reopenedOnce);
            restored.Fail("q_old");
            Assert.True(restored.Reopen("q_old", 4, _ => true));
        }

        [Fact]
        public void Abandon_RequiresAnActiveQuest()
        {
            var q = Fresh();
            Register(q, "q_done");
            q.Complete("q_done");
            Assert.False(q.Abandon("q_done"));
            Assert.False(q.Abandon("missing"));
        }
    }
}
