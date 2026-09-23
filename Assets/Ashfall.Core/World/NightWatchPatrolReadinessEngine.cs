// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 36 — The Watch
// Subsystem    : Night Watch Patrol Route, Perimeter Detection & Readiness Engine
// Authority    : docs/expansions/wave5/expansion_36_the_watch_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Patrol route coverage quality for a given sector.
    /// </summary>
    public enum PatrolCoverageGrade
    {
        Blind       = 0,  // no patrol coverage; breach undetected
        Sparse      = 1,  // irregular patrols; 10–30% detection
        Partial     = 2,  // overlapping gaps; 31–60% detection
        Adequate    = 3,  // most angles covered; 61–85% detection
        Full        = 4   // continuous coverage; >85% detection
    }

    /// <summary>
    /// Watch-post fatigue tier affecting observation reliability.
    /// </summary>
    public enum WatchFatigueTier
    {
        Alert     = 0,  // <20% fatigue; full vigilance
        Tired     = 1,  // 20–50% fatigue; minor detection penalty
        Exhausted = 2,  // 51–80% fatigue; significant gaps
        Impaired  = 3   // >80% fatigue; severe detection failure risk
    }

    /// <summary>
    /// Immutable result of a patrol route coverage evaluation.
    /// </summary>
    public readonly struct PatrolCoverageResult
    {
        public PatrolCoverageGrade CoverageGrade     { get; }
        /// <summary>Detection probability permille for a breach event (0..1000).</summary>
        public int DetectionProbabilityPermille       { get; }
        /// <summary>Number of unpatrolled gap-hours in the 24h cycle.</summary>
        public int UncoveredGapHours                 { get; }
        /// <summary>True if the coverage meets the readiness threshold for the sector type.</summary>
        public bool MeetsReadinessThreshold          { get; }

        public PatrolCoverageResult(
            PatrolCoverageGrade coverageGrade,
            int detectionProbabilityPermille,
            int uncoveredGapHours,
            bool meetsReadinessThreshold)
        {
            CoverageGrade                = coverageGrade;
            DetectionProbabilityPermille = Math.Clamp(detectionProbabilityPermille, 0, 1000);
            UncoveredGapHours            = Math.Clamp(uncoveredGapHours, 0, 24);
            MeetsReadinessThreshold      = meetsReadinessThreshold;
        }
    }

    /// <summary>
    /// Immutable result of a gate-protocol readiness evaluation.
    /// </summary>
    public readonly struct GateProtocolReadinessResult
    {
        /// <summary>Composite readiness score (0..1000 permille).</summary>
        public int ReadinessPermille                 { get; }
        /// <summary>True if the gate is ready to operate under a threat scenario.</summary>
        public bool IsGateReady                      { get; }
        /// <summary>Time-to-seal estimate in minutes at current staffing.</summary>
        public int EstimatedSealTimeMinutes          { get; }
        /// <summary>Primary bottleneck description (empty when IsGateReady).</summary>
        public string PrimaryBottleneck              { get; }

        public GateProtocolReadinessResult(
            int readinessPermille,
            bool isGateReady,
            int estimatedSealTimeMinutes,
            string primaryBottleneck)
        {
            ReadinessPermille        = Math.Clamp(readinessPermille, 0, 1000);
            IsGateReady              = isGateReady;
            EstimatedSealTimeMinutes = Math.Max(0, estimatedSealTimeMinutes);
            PrimaryBottleneck        = primaryBottleneck ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine governing patrol route coverage gaps, watch-post fatigue decay,
    /// perimeter alarm trigger thresholds, gate-protocol readiness, and drill-recency scoring.
    /// Extends PatrolTerritoryAuthority (TerritoryNodeRecord, DynamicTerritoryState) and
    /// SoundRangingThreatEngine (AcousticSensorNode, calibration drift, AcousticThreatEstimate)
    /// without duplicating territory or acoustic detection authority.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class NightWatchPatrolReadinessEngine
    {
        /// <summary>Gate readiness permille threshold for operational certification.</summary>
        public const int GateReadinessThreshold = 700;

        /// <summary>Watch-post fatigue permille at which impairment begins seriously degrading detection.</summary>
        public const int ImpairedFatigueThreshold = 800;

        /// <summary>Patrol hours per 24h cycle required for Adequate coverage.</summary>
        public const int AdequateCoverageHoursRequired = 18;

        /// <summary>
        /// Evaluates patrol route coverage for a sector over a 24-hour cycle.
        /// </summary>
        /// <param name="assignedPatrollerCount">Number of patrollers assigned to the sector.</param>
        /// <param name="sectorPerimeterMetres">Perimeter length of the sector in metres.</param>
        /// <param name="averageFatiguePermille">Average fatigue permille of assigned watch staff (0..1000).</param>
        /// <param name="nightVisionEquipmentPermille">Night-vision equipment quality (0..1000).</param>
        public static PatrolCoverageResult EvaluatePatrolCoverage(
            int assignedPatrollerCount,
            int sectorPerimeterMetres,
            int averageFatiguePermille,
            int nightVisionEquipmentPermille)
        {
            assignedPatrollerCount       = Math.Max(0, assignedPatrollerCount);
            sectorPerimeterMetres        = Math.Max(1, sectorPerimeterMetres);
            averageFatiguePermille       = Math.Clamp(averageFatiguePermille, 0, 1000);
            nightVisionEquipmentPermille = Math.Clamp(nightVisionEquipmentPermille, 0, 1000);

            if (assignedPatrollerCount == 0)
                return new PatrolCoverageResult(PatrolCoverageGrade.Blind, 0, 24, false);

            // Each patroller can cover ~500m of perimeter effectively per night
            int effectiveCoverageMetres = assignedPatrollerCount * 500;

            // Fatigue reduces effective coverage
            int fatiguePenalty = (averageFatiguePermille * effectiveCoverageMetres) / 2000;
            effectiveCoverageMetres = Math.Max(0, effectiveCoverageMetres - fatiguePenalty);

            // Night-vision equipment extends coverage
            int nvBonus = (nightVisionEquipmentPermille * effectiveCoverageMetres) / 2000;
            effectiveCoverageMetres += nvBonus;

            // Coverage ratio against perimeter
            int coverageRatio = Math.Min(1000,
                (effectiveCoverageMetres * 1000) / sectorPerimeterMetres);

            PatrolCoverageGrade grade = coverageRatio switch
            {
                0        => PatrolCoverageGrade.Blind,
                <= 300   => PatrolCoverageGrade.Sparse,
                <= 600   => PatrolCoverageGrade.Partial,
                <= 850   => PatrolCoverageGrade.Adequate,
                _        => PatrolCoverageGrade.Full
            };

            // Uncovered gap hours proportional to uncovered perimeter fraction
            int uncoveredGapHours = Math.Max(0,
                24 - (24 * coverageRatio) / 1000);

            // Detection probability incorporates coverage ratio and night vision
            int detectionBase   = coverageRatio;
            int nvDetectionBonus = (nightVisionEquipmentPermille * 100) / 1000;
            int detection        = Math.Clamp(detectionBase + nvDetectionBonus, 0, 1000);

            bool meetsThreshold = uncoveredGapHours <= (24 - AdequateCoverageHoursRequired);

            return new PatrolCoverageResult(grade, detection, uncoveredGapHours, meetsThreshold);
        }

        /// <summary>
        /// Computes watch-post fatigue after a given shift duration.
        /// </summary>
        /// <param name="currentFatiguePermille">Current fatigue permille (0..1000).</param>
        /// <param name="shiftHours">Duration of the current shift in hours (1..16).</param>
        /// <param name="restQualityPermille">Quality of prior rest (0..1000; reduces fatigue accumulation).</param>
        /// <returns>New fatigue permille after the shift.</returns>
        public static int AdvanceWatchFatigue(
            int currentFatiguePermille,
            int shiftHours,
            int restQualityPermille)
        {
            currentFatiguePermille = Math.Clamp(currentFatiguePermille, 0, 1000);
            shiftHours             = Math.Clamp(shiftHours, 1, 16);
            restQualityPermille    = Math.Clamp(restQualityPermille, 0, 1000);

            // Fatigue accumulates at ~60 permille per hour; good rest slows it
            int restReduction  = (restQualityPermille * 30) / 1000;
            int fatiguePerHour = Math.Max(10, 60 - restReduction);
            int newFatigue     = Math.Min(1000, currentFatiguePermille + shiftHours * fatiguePerHour);

            return newFatigue;
        }

        /// <summary>
        /// Classifies fatigue permille into a watch fatigue tier.
        /// </summary>
        public static WatchFatigueTier ClassifyFatigue(int fatiguePermille) =>
            fatiguePermille switch
            {
                <= 200 => WatchFatigueTier.Alert,
                <= 500 => WatchFatigueTier.Tired,
                <= 800 => WatchFatigueTier.Exhausted,
                _      => WatchFatigueTier.Impaired
            };

        /// <summary>
        /// Evaluates gate-protocol readiness for a shelter gate under a threat scenario.
        /// </summary>
        /// <param name="gateStaffPermille">Gate crew complement as permille of full staff (0..1000).</param>
        /// <param name="drillRecencyPermille">
        ///     Recency of last gate-seal drill (0..1000; decays daily; 1000=today).
        /// </param>
        /// <param name="mechanicalConditionPermille">Gate mechanism condition (0..1000).</param>
        /// <param name="alarmSystemOnline">True if the electronic alarm relay is operational.</param>
        public static GateProtocolReadinessResult EvaluateGateProtocol(
            int gateStaffPermille,
            int drillRecencyPermille,
            int mechanicalConditionPermille,
            bool alarmSystemOnline)
        {
            gateStaffPermille          = Math.Clamp(gateStaffPermille, 0, 1000);
            drillRecencyPermille       = Math.Clamp(drillRecencyPermille, 0, 1000);
            mechanicalConditionPermille = Math.Clamp(mechanicalConditionPermille, 0, 1000);

            // Weighted composite
            int composite = (gateStaffPermille         * 35 +
                             drillRecencyPermille       * 30 +
                             mechanicalConditionPermille * 25) / 90;

            if (alarmSystemOnline)
                composite = Math.Min(1000, composite + 100);

            bool isReady = composite >= GateReadinessThreshold;

            // Seal time: fully staffed with fresh drill seals in 2 min; degraded up to 15 min
            int deficiency     = Math.Max(0, 1000 - composite);
            int sealTimeMinutes = 2 + (deficiency * 13) / 1000;

            // Primary bottleneck
            string bottleneck = string.Empty;
            if (!isReady)
            {
                if (gateStaffPermille < 400)
                    bottleneck = "Gate crew under-staffed";
                else if (drillRecencyPermille < 300)
                    bottleneck = "Gate-seal drill overdue";
                else if (mechanicalConditionPermille < 400)
                    bottleneck = "Gate mechanism degraded";
                else if (!alarmSystemOnline)
                    bottleneck = "Alarm relay offline";
                else
                    bottleneck = "Composite readiness below threshold";
            }

            return new GateProtocolReadinessResult(composite, isReady, sealTimeMinutes, bottleneck);
        }

        /// <summary>
        /// Scores the overall watch readiness of a sector from patrol, fatigue, and drill inputs.
        /// </summary>
        /// <param name="patrolCoveragePermille">Patrol coverage ratio (0..1000).</param>
        /// <param name="averageFatiguePermille">Average watch-staff fatigue (0..1000; lower is better).</param>
        /// <param name="drillRecencyPermille">Drill recency (0..1000).</param>
        /// <returns>Composite watch readiness score (0..1000).</returns>
        public static int ComputeWatchReadinessScore(
            int patrolCoveragePermille,
            int averageFatiguePermille,
            int drillRecencyPermille)
        {
            patrolCoveragePermille = Math.Clamp(patrolCoveragePermille, 0, 1000);
            averageFatiguePermille = Math.Clamp(averageFatiguePermille, 0, 1000);
            drillRecencyPermille   = Math.Clamp(drillRecencyPermille, 0, 1000);

            int alertnessBonus = Math.Max(0, 1000 - averageFatiguePermille); // invert fatigue
            int score = (patrolCoveragePermille * 40 +
                         alertnessBonus          * 35 +
                         drillRecencyPermille    * 25) / 100;

            return Math.Clamp(score, 0, 1000);
        }
    }
}
