using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Phase 6.D Chain 1 (Census / Human Cost) and Chain 3 (Survival
    /// Reckoning) tests for the new ReckoningSystem recorders.
    /// </summary>
    public class VerdictChainTests
    {
        // ── Chain 1 ────────────────────────────────────────────────────────

        [Fact]
        public void Chain1_RecordDrift_AccumulatesAcrossDays()
        {
            var r = new ReckoningSystem();
            r.RecordDrift(day: 161, count: 1);
            r.RecordDrift(day: 162, count: 2);
            r.RecordDrift(day: 162, count: 0); // ignored
            r.RecordDrift(day: -1, count: 0); // ignored
            Assert.Equal(3, r.DwellingDriftTotal);
            Assert.Equal(2, r.LastDriftDeltaToday);
            Assert.Equal(162, r.LastDriftDay);
        }

        [Fact]
        public void Chain1_RecordDrift_NeverReducesTotal()
        {
            var r = new ReckoningSystem();
            r.RecordDrift(161, 5);
            r.RecordDrift(162, 2);
            Assert.Equal(7, r.DwellingDriftTotal);
        }

        [Fact]
        public void Chain1_DriftIsResilientAcrossSaveAndRestore()
        {
            var r = new ReckoningSystem();
            r.RecordDrift(160, 3);
            r.RecordDrift(161, 2);
            var saved = r.CaptureState();
            var fresh = new ReckoningSystem();
            fresh.RestoreState(saved);
            Assert.Equal(5, fresh.DwellingDriftTotal);
            Assert.Equal(2, fresh.LastDriftDeltaToday);
            Assert.Equal(161, fresh.LastDriftDay);
        }

        // ── Chain 3 ────────────────────────────────────────────────────────

        [Fact]
        public void Chain3_CumulativeDoseBelowThreshold_NoPromotion()
        {
            var r = new ReckoningSystem();
            r.RecordCumulativeDose(150, 3.5f);
            Assert.False(r.HighDosePromoted);
            Assert.Equal(ReckoningPhase.Dormant, r.Phase);
        }

        [Fact]
        public void Chain3_CumulativeDoseAboveThreshold_PromotesDormantToKnowing()
        {
            var r = new ReckoningSystem();
            int fired = 0;
            r.OnPhaseChanged += _ => fired++;
            r.RecordCumulativeDose(day: 70, sieverts: 4.5f);
            Assert.True(r.HighDosePromoted);
            Assert.Equal(ReckoningPhase.Knowing, r.Phase);
            Assert.Equal(1, fired);
        }

        [Fact]
        public void Chain3_PromotionIsOneShot()
        {
            var r = new ReckoningSystem();
            r.RecordCumulativeDose(70, 4.5f);
            r.RecordCumulativeDose(75, 9.0f);
            Assert.True(r.HighDosePromoted);
            Assert.Equal(ReckoningPhase.Knowing, r.Phase);
        }

        [Fact]
        public void Chain3_HighDoseDoesNotDowngradePhase()
        {
            var r = new ReckoningSystem();
            var poll = r.Poll(day: 240, livingCount: 4, logReadCount: 1, evidenceCount: 1);
            Assert.Equal(ReckoningPhase.Counted, r.Phase);
            r.RecordCumulativeDose(241, 7f);
            Assert.Equal(ReckoningPhase.Counted, r.Phase);
        }

        [Fact]
        public void Chain3_OpenEndedRecurrencePreservesState()
        {
            var r = new ReckoningSystem();
            r.RecordCumulativeDose(160, 4.0f);
            var snap = r.CaptureState();
            Assert.True(snap.highDosePromoted);
            Assert.Equal(4.0f, snap.cumulativeDoseSieverts);
            var fresh = new ReckoningSystem();
            fresh.RestoreState(snap);
            Assert.True(fresh.HighDosePromoted);
            Assert.Equal(4.0f, fresh.CumulativeDoseSieverts);
            Assert.Equal(ReckoningPhase.Knowing, fresh.Phase);
        }

        [Fact]
        public void Gateway_RecordCumulativeDoseClampsNegativeInput()
        {
            var r = new ReckoningSystem();
            r.RecordCumulativeDose(160, -1.0f);
            Assert.Equal(0f, r.CumulativeDoseSieverts);
        }
    }
}
