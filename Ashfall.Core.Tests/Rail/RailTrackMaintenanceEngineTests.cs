// SPDX-License-Identifier: MIT
// Expansion 25 — The Iron Road : RailTrackMaintenanceEngine focused tests
using Xunit;
using Ashfall.Core.Rail;

namespace Ashfall.Core.Tests.Rail
{
    public sealed class RailTrackMaintenanceEngineTests
    {
        // ── 1. Bridge load refusal when total tonnage exceeds effective capacity ──
        [Fact]
        public void EvaluateTrackFeasibility_BridgeLoadRefusal_WhenOverweight()
        {
            var track = new TrackSegmentState
            {
                SegmentId      = "seg-bridge-01",
                MaxBridgeLoadTons = 100,
                BridgeIntegrityPermille = 1000, // full capacity = 100t
                GaugeStability = TrackGaugeStability.PristineStandard
            };

            // ArmoredBattleTrain = 250 t — far over 100 t capacity
            var result = RailTrackMaintenanceEngine.EvaluateTrackFeasibility(
                track, LocomotiveClass.ArmoredBattleTrain, 0);

            Assert.Equal(RailFeasibilityOutcome.BridgeLoadRefusal, result.Outcome);
            Assert.True(result.DerailmentRiskPermille >= 800);
            Assert.Equal(0, result.SpeedMultiplierPermille);
        }

        // ── 2. Speed-restricted when gauge is SevereDistortion ──
        [Fact]
        public void EvaluateTrackFeasibility_SpeedRestricted_WhenGaugeSevere()
        {
            var track = new TrackSegmentState
            {
                SegmentId      = "seg-bad-gauge",
                MaxBridgeLoadTons = 500,
                BridgeIntegrityPermille = 1000,
                GaugeStability = TrackGaugeStability.SevereDistortion,
                TrackWearPermille = 200
            };

            var result = RailTrackMaintenanceEngine.EvaluateTrackFeasibility(
                track, LocomotiveClass.ManualHandcar, 0);

            Assert.Equal(RailFeasibilityOutcome.SpeedRestricted, result.Outcome);
            Assert.True(result.SpeedMultiplierPermille < 1000);
        }

        // ── 3. Passable on pristine track with light handcar ──
        [Fact]
        public void EvaluateTrackFeasibility_Passable_WhenPristine()
        {
            var track = new TrackSegmentState
            {
                SegmentId      = "seg-pristine",
                MaxBridgeLoadTons = 500,
                BridgeIntegrityPermille = 1000,
                GaugeStability = TrackGaugeStability.PristineStandard,
                TrackWearPermille = 50
            };

            var result = RailTrackMaintenanceEngine.EvaluateTrackFeasibility(
                track, LocomotiveClass.ManualHandcar, 0);

            Assert.Equal(RailFeasibilityOutcome.Passable, result.Outcome);
            Assert.Equal(1000, result.SpeedMultiplierPermille);
        }

        // ── 4. Multiple heavy runs push wear high enough to induce gauge spread ──
        [Fact]
        public void ApplyTrainWear_InducesGaugeSpread_AfterRepeatedHeavyRuns()
        {
            var track = new TrackSegmentState
            {
                SegmentId      = "seg-heavy",
                GaugeStability = TrackGaugeStability.PristineStandard,
                TrackWearPermille = 0,
                BridgeIntegrityPermille = 1000,
                MaxBridgeLoadTons = 500
            };

            // Each ArmoredBattleTrain run adds (250 * 15)/100 = 37 wear; need 800 → ~22 runs
            for (int i = 0; i < 25; i++)
                RailTrackMaintenanceEngine.ApplyTrainWear(track, LocomotiveClass.ArmoredBattleTrain, 0);

            Assert.True(track.TrackWearPermille >= 800,
                $"Expected wear ≥ 800, got {track.TrackWearPermille}");
            Assert.True(track.GaugeStability >= TrackGaugeStability.MinorSpread,
                $"Expected MinorSpread or worse, got {track.GaugeStability}");
        }

        // ── 5. Maintenance restores track wear and clears debris ──
        [Fact]
        public void PerformMaintenance_RestoresTrackAndClearsDebris()
        {
            var track = new TrackSegmentState
            {
                SegmentId      = "seg-damaged",
                TrackWearPermille = 900,
                BridgeIntegrityPermille = 500,
                GaugeStability = TrackGaugeStability.SevereDistortion,
                IsBlockedByDebris = true
            };

            // Full materials + 80 labor hours
            RailTrackMaintenanceEngine.PerformMaintenance(track, 1000, 80);

            Assert.False(track.IsBlockedByDebris);
            Assert.True(track.TrackWearPermille < 900, "Wear should have decreased");
            Assert.True(track.BridgeIntegrityPermille > 500, "Bridge integrity should have improved");
        }
    }
}
