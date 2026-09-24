// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 36 — The Watch
// Pure live-readiness projection. It consumes facts supplied by canonical
// owners and delegates all arithmetic to NightWatchPatrolReadinessEngine.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.World
{
    /// <summary>Facts about one authored watch sector at the moment of evaluation.</summary>
    public sealed class NightWatchSectorFacts
    {
        public string SectorId { get; set; } = string.Empty;
        public int PerimeterMetres { get; set; } = 1000;
        public int AssignedPatrollers { get; set; }
        public int AverageFatiguePermille { get; set; }
        public int NightVisionPermille { get; set; }
        public int ActivePostCount { get; set; }
        public int PostConditionPermille { get; set; } = 1000;
        public bool AlarmOnline { get; set; }
        public int GateStaffPermille { get; set; }
        public int GateMechanismPermille { get; set; } = 1000;
        public int GateDrillRecencyPermille { get; set; }
        public bool TerritoryContested { get; set; }
        public int AcousticConfidencePermille { get; set; }
        public int SoundSensorCount { get; set; }
    }

    public enum NightWatchReadinessBand
    {
        Blind = 0,
        Fragile = 1,
        Watched = 2,
        Reliable = 3,
        Fortified = 4
    }

    /// <summary>Immutable presentation-safe summary of live watch readiness.</summary>
    public readonly struct NightWatchReadinessSnapshot
    {
        public string SectorId { get; }
        public PatrolCoverageResult PatrolCoverage { get; }
        public GateProtocolReadinessResult GateReadiness { get; }
        public int OverallReadinessPermille { get; }
        public NightWatchReadinessBand Band { get; }
        public bool TerritoryContested { get; }
        public bool AcousticContactActive { get; }
        public int AcousticConfidencePermille { get; }
        public int ActivePostCount { get; }
        public int SoundSensorCount { get; }

        public NightWatchReadinessSnapshot(
            string sectorId,
            PatrolCoverageResult patrolCoverage,
            GateProtocolReadinessResult gateReadiness,
            int overallReadinessPermille,
            NightWatchReadinessBand band,
            bool territoryContested,
            bool acousticContactActive,
            int acousticConfidencePermille,
            int activePostCount,
            int soundSensorCount)
        {
            SectorId = sectorId ?? string.Empty;
            PatrolCoverage = patrolCoverage;
            GateReadiness = gateReadiness;
            OverallReadinessPermille = Math.Clamp(overallReadinessPermille, 0, 1000);
            Band = band;
            TerritoryContested = territoryContested;
            AcousticContactActive = acousticContactActive;
            AcousticConfidencePermille = Math.Clamp(acousticConfidencePermille, 0, 1000);
            ActivePostCount = Math.Max(0, activePostCount);
            SoundSensorCount = Math.Max(0, soundSensorCount);
        }
    }

    /// <summary>
    /// Pure adapter around the signed readiness engine. It contains no
    /// persistence, engine references, or mutable state; the host supplies
    /// facts from TerritoryControlSystem, PerimeterDefenseSystem,
    /// SoundRangingThreatEngine, DutyRosterSystem, and ShelterSecuritySystem.
    /// </summary>
    public static class NightWatchReadinessProjection
    {
        public static NightWatchReadinessSnapshot Evaluate(NightWatchSectorFacts facts)
        {
            facts ??= new NightWatchSectorFacts();
            string sector = string.IsNullOrWhiteSpace(facts.SectorId) ? "gate" : facts.SectorId.Trim();

            int staff = Math.Max(0, facts.AssignedPatrollers);
            int perimeter = Math.Max(1, facts.PerimeterMetres);
            int fatigue = Math.Clamp(facts.AverageFatiguePermille, 0, 1000);
            int nightVision = Math.Clamp(facts.NightVisionPermille, 0, 1000);

            // A degraded post cannot provide the full authored night-vision
            // bonus. This keeps the projection honest when a post is disabled.
            int activePostFactor = facts.ActivePostCount <= 0
                ? 0
                : Math.Clamp(facts.PostConditionPermille, 0, 1000);
            nightVision = (nightVision * activePostFactor) / 1000;

            var coverage = NightWatchPatrolReadinessEngine.EvaluatePatrolCoverage(
                staff, perimeter, fatigue, nightVision);

            var gate = NightWatchPatrolReadinessEngine.EvaluateGateProtocol(
                Math.Clamp(facts.GateStaffPermille, 0, 1000),
                Math.Clamp(facts.GateDrillRecencyPermille, 0, 1000),
                Math.Clamp(facts.GateMechanismPermille, 0, 1000),
                facts.AlarmOnline);

            int drill = Math.Clamp(facts.GateDrillRecencyPermille, 0, 1000);
            int overall = NightWatchPatrolReadinessEngine.ComputeWatchReadinessScore(
                coverage.DetectionProbabilityPermille,
                fatigue,
                drill);

            // Acoustic confidence is evidence, not a hidden combat score. It
            // raises readiness only as a bounded confirmation/readiness hint.
            if (facts.AcousticConfidencePermille > 0 && facts.SoundSensorCount > 0)
            {
                int acousticBonus = Math.Min(50, facts.AcousticConfidencePermille / 20);
                overall = Math.Clamp(overall + acousticBonus, 0, 1000);
            }

            if (facts.TerritoryContested)
                overall = Math.Clamp(overall - 50, 0, 1000);

            NightWatchReadinessBand band = overall switch
            {
                >= 850 => NightWatchReadinessBand.Fortified,
                >= 700 => NightWatchReadinessBand.Reliable,
                >= 450 => NightWatchReadinessBand.Watched,
                >= 200 => NightWatchReadinessBand.Fragile,
                _ => NightWatchReadinessBand.Blind
            };

            return new NightWatchReadinessSnapshot(
                sector,
                coverage,
                gate,
                overall,
                band,
                facts.TerritoryContested,
                facts.AcousticConfidencePermille > 0,
                facts.AcousticConfidencePermille,
                facts.ActivePostCount,
                facts.SoundSensorCount);
        }

        public static NightWatchReadinessSnapshot Evaluate(
            IEnumerable<NightWatchSectorFacts> sectors,
            string fallbackSector = "gate")
        {
            if (sectors == null) return Evaluate(new NightWatchSectorFacts { SectorId = fallbackSector });
            var rows = sectors.Where(x => x != null).ToList();
            if (rows.Count == 0) return Evaluate(new NightWatchSectorFacts { SectorId = fallbackSector });
            return Evaluate(rows.OrderBy(x => x.SectorId, StringComparer.Ordinal).First());
        }
    }
}
