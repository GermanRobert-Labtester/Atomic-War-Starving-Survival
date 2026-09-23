// SPDX-License-Identifier: MIT
// Expansion 36 — The Watch : NightWatchPatrolReadinessEngine focused tests
using Xunit;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests.World
{
    public sealed class NightWatchPatrolReadinessEngineTests
    {
        // ── 1. Zero patrollers yields Blind coverage ──
        [Fact]
        public void EvaluatePatrolCoverage_Blind_WhenNoPatrollersAssigned()
        {
            var result = NightWatchPatrolReadinessEngine.EvaluatePatrolCoverage(
                assignedPatrollerCount: 0,
                sectorPerimeterMetres: 1000,
                averageFatiguePermille: 0,
                nightVisionEquipmentPermille: 1000);

            Assert.Equal(PatrolCoverageGrade.Blind, result.CoverageGrade);
            Assert.Equal(0, result.DetectionProbabilityPermille);
            Assert.False(result.MeetsReadinessThreshold);
        }

        // ── 2. Adequate patrollers cover a manageable perimeter ──
        [Fact]
        public void EvaluatePatrolCoverage_Adequate_WhenSufficientPatrollers()
        {
            // 6 patrollers × 500m = 3000m effective; sector = 2000m → >100% coverage
            var result = NightWatchPatrolReadinessEngine.EvaluatePatrolCoverage(
                assignedPatrollerCount: 6,
                sectorPerimeterMetres: 2000,
                averageFatiguePermille: 100,
                nightVisionEquipmentPermille: 500);

            Assert.True(result.CoverageGrade >= PatrolCoverageGrade.Adequate,
                $"6 patrollers on 2km perimeter should be Adequate+, got {result.CoverageGrade}");
            Assert.True(result.MeetsReadinessThreshold);
        }

        // ── 3. Fatigue accumulates correctly over a long shift ──
        [Fact]
        public void AdvanceWatchFatigue_Accumulates_OverLongShift()
        {
            int startFatigue = 100;
            int after8h = NightWatchPatrolReadinessEngine.AdvanceWatchFatigue(
                startFatigue, shiftHours: 8, restQualityPermille: 500);
            int after12h = NightWatchPatrolReadinessEngine.AdvanceWatchFatigue(
                startFatigue, shiftHours: 12, restQualityPermille: 500);

            Assert.True(after12h > after8h,
                "12-hour shift should produce more fatigue than 8-hour shift");
            Assert.True(after8h > startFatigue,
                "Fatigue should increase from baseline after any shift");
        }

        // ── 4. Fully staffed, drilled, maintained gate with alarm is gate-ready ──
        [Fact]
        public void EvaluateGateProtocol_Ready_WhenFullyStaffedAndDrilled()
        {
            var result = NightWatchPatrolReadinessEngine.EvaluateGateProtocol(
                gateStaffPermille: 1000,
                drillRecencyPermille: 900,
                mechanicalConditionPermille: 950,
                alarmSystemOnline: true);

            Assert.True(result.IsGateReady,
                "Fully staffed, drilled, maintained gate should be ready");
            Assert.True(result.EstimatedSealTimeMinutes <= 5,
                "Well-prepared gate should seal quickly");
        }

        // ── 5. High fatigue + sparse patrol yields low overall watch readiness ──
        [Fact]
        public void ComputeWatchReadinessScore_Low_WhenHighFatigueAndSparseCoverage()
        {
            int lowReadiness = NightWatchPatrolReadinessEngine.ComputeWatchReadinessScore(
                patrolCoveragePermille: 150,   // sparse
                averageFatiguePermille: 850,   // exhausted/impaired
                drillRecencyPermille: 100);    // stale drill

            int highReadiness = NightWatchPatrolReadinessEngine.ComputeWatchReadinessScore(
                patrolCoveragePermille: 900,
                averageFatiguePermille: 100,
                drillRecencyPermille: 850);

            Assert.True(lowReadiness < 300,
                $"High fatigue + sparse patrol should yield low readiness, got {lowReadiness}");
            Assert.True(highReadiness > lowReadiness,
                "Well-rested, full-coverage, drilled watch should score significantly higher");
        }
    }
}
