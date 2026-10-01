// SPDX-License-Identifier: MIT
// Task 2 — a failed rescue sortie must not leave its mission dangling at
// Dispatched. The transition is terminal, exactly once, and grants no salvage
// (the expedition never arrived).
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public sealed class ExpeditionRescueFailureBridgeTests
    {
        private readonly DistressRescueMissionManager _manager = new DistressRescueMissionManager();

        private void DispatchTrader()
        {
            _manager.RecordSignalHeard("freq_distress_156_8", 1);
            _manager.RecordExpeditionDispatched("quest_distress_injured_trader", "exp_fail");
        }

        [Fact]
        public void FailedRescue_TransitionsToTerminalFailed_ExactlyOnce()
        {
            DispatchTrader();
            Assert.True(_manager.RecordExpeditionFailed(
                "quest_distress_injured_trader", "Collapsed from exhaustion.", 3));

            var m = _manager.GetMissionByQuest("quest_distress_injured_trader")!;
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, m.Stage);
            Assert.True(m.IsTerminal);
            Assert.False(m.ArrivalResolved); // never arrived
            Assert.Contains("failed", m.OutcomeSummary);

            // Re-entry is refused — the mission is terminal.
            Assert.False(_manager.RecordExpeditionFailed(
                "quest_distress_injured_trader", "again", 4));
        }

        [Fact]
        public void FailedRescue_GrantsNoSalvage()
        {
            DispatchTrader();
            _manager.RecordExpeditionFailed("quest_distress_injured_trader", "Collapsed.", 3);

            var (items, rep) = _manager.ClaimIdempotentRewards("quest_distress_injured_trader");
            Assert.Empty(items);
            Assert.Equal(0, rep);
        }

        [Fact]
        public void FailedRescue_RefusedBeforeDispatch()
        {
            _manager.RecordSignalHeard("freq_distress_445_2", 1); // Heard only
            Assert.False(_manager.RecordExpeditionFailed("quest_distress_family_shelter", "x", 2));
            Assert.False(_manager.RecordExpeditionFailed("quest_does_not_exist", "x", 2));
        }

        [Fact]
        public void FailedRescue_RefusedOnceRescued()
        {
            DispatchTrader();
            // Arrive in time → TerminalRescued with ArrivalResolved=true.
            var stage = _manager.RecordDestinationReached("quest_distress_injured_trader", 2);
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, stage);

            // A later failure report must not rewrite a completed rescue.
            Assert.False(_manager.RecordExpeditionFailed("quest_distress_injured_trader", "late", 3));
            var m = _manager.GetMissionByQuest("quest_distress_injured_trader")!;
            Assert.Equal(DistressRescueMissionStage.TerminalRescued, m.Stage);
        }

        [Fact]
        public void FailedRescue_SurvivesSaveLoad()
        {
            DispatchTrader();
            _manager.RecordExpeditionFailed("quest_distress_injured_trader", "Lost to the wastes.", 3);

            var fresh = new DistressRescueMissionManager();
            fresh.RestoreState(_manager.CaptureState());
            var m = fresh.GetMissionByQuest("quest_distress_injured_trader")!;
            Assert.Equal(DistressRescueMissionStage.TerminalFailed, m.Stage);
            Assert.Contains("Lost to the wastes.", m.OutcomeSummary);
        }
    }
}
