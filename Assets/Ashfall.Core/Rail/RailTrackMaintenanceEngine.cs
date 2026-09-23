// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 25 — The Iron Road
// Subsystem    : Rail Route Interlock & Track Maintenance Engine
// Authority    : docs/expansions/wave3/expansion_25_the_iron_road_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Rail
{
    /// <summary>
    /// Rail gauge physical alignment and distortion state.
    /// </summary>
    public enum TrackGaugeStability
    {
        PristineStandard = 0,
        MinorSpread = 1,
        SevereDistortion = 2,
        BrokenGaugeRefusal = 3
    }

    /// <summary>
    /// Classification of rolling stock traversing the wasteland rail corridor.
    /// </summary>
    public enum LocomotiveClass
    {
        ManualHandcar = 0,      // 2 metric tons
        LightSteamShunter = 1,  // 30 metric tons
        DieselFreightRig = 2,   // 120 metric tons
        ArmoredBattleTrain = 3  // 250 metric tons
    }

    /// <summary>
    /// Route feasibility outcome for train dispatch.
    /// </summary>
    public enum RailFeasibilityOutcome
    {
        Passable = 0,
        SpeedRestricted = 1,
        BridgeLoadRefusal = 2,
        GaugeSpreadRefusal = 3
    }

    /// <summary>
    /// State record of a track segment in the rail corridor.
    /// </summary>
    public sealed class TrackSegmentState
    {
        public string SegmentId { get; set; } = string.Empty;
        public TrackGaugeStability GaugeStability { get; set; } = TrackGaugeStability.PristineStandard;
        public int TrackWearPermille { get; set; } = 100; // 0..1000
        public int BridgeIntegrityPermille { get; set; } = 1000; // 0..1000
        public int MaxBridgeLoadTons { get; set; } = 150;
        public bool IsBlockedByDebris { get; set; }

        public TrackSegmentState Clone() => new TrackSegmentState
        {
            SegmentId = SegmentId,
            GaugeStability = GaugeStability,
            TrackWearPermille = TrackWearPermille,
            BridgeIntegrityPermille = BridgeIntegrityPermille,
            MaxBridgeLoadTons = MaxBridgeLoadTons,
            IsBlockedByDebris = IsBlockedByDebris
        };
    }

    /// <summary>
    /// Feasibility evaluation result for rail transit.
    /// </summary>
    public readonly struct RailTransitEvaluationResult
    {
        public RailFeasibilityOutcome Outcome { get; }
        public int DerailmentRiskPermille { get; }
        public int SpeedMultiplierPermille { get; }
        public string RefusalReason { get; }

        public RailTransitEvaluationResult(
            RailFeasibilityOutcome outcome,
            int derailmentRiskPermille,
            int speedMultiplierPermille,
            string refusalReason)
        {
            Outcome = outcome;
            DerailmentRiskPermille = Math.Clamp(derailmentRiskPermille, 0, 1000);
            SpeedMultiplierPermille = Math.Clamp(speedMultiplierPermille, 0, 1000);
            RefusalReason = refusalReason ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine governing rail track wear, gauge integrity, bridge load limits,
    /// and track gang maintenance scheduling.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class RailTrackMaintenanceEngine
    {
        public const int DerailmentThresholdPermille = 400;

        /// <summary>
        /// Returns the estimated tare weight in tons for a locomotive class.
        /// </summary>
        public static int GetLocomotiveWeightTons(LocomotiveClass loco)
        {
            return loco switch
            {
                LocomotiveClass.ManualHandcar => 2,
                LocomotiveClass.LightSteamShunter => 30,
                LocomotiveClass.DieselFreightRig => 120,
                LocomotiveClass.ArmoredBattleTrain => 250,
                _ => 50
            };
        }

        /// <summary>
        /// Evaluates route feasibility and derailment risk for a train on a track segment.
        /// </summary>
        public static RailTransitEvaluationResult EvaluateTrackFeasibility(
            TrackSegmentState track,
            LocomotiveClass loco,
            int cargoTonnage)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));

            if (track.IsBlockedByDebris)
            {
                return new RailTransitEvaluationResult(
                    RailFeasibilityOutcome.GaugeSpreadRefusal,
                    1000,
                    0,
                    "Track is blocked by debris or collapsed structure");
            }

            if (track.GaugeStability == TrackGaugeStability.BrokenGaugeRefusal)
            {
                return new RailTransitEvaluationResult(
                    RailFeasibilityOutcome.GaugeSpreadRefusal,
                    1000,
                    0,
                    "Track gauge is broken or spread beyond operational limits");
            }

            int totalTons = GetLocomotiveWeightTons(loco) + Math.Max(0, cargoTonnage);
            int effectiveBridgeCapacity = (track.MaxBridgeLoadTons * track.BridgeIntegrityPermille) / 1000;

            if (totalTons > effectiveBridgeCapacity)
            {
                return new RailTransitEvaluationResult(
                    RailFeasibilityOutcome.BridgeLoadRefusal,
                    850,
                    0,
                    $"Total train weight ({totalTons}t) exceeds bridge capacity ({effectiveBridgeCapacity}t)");
            }

            // Derailment risk calculation based on gauge stability and track wear
            int baseGaugeRisk = track.GaugeStability switch
            {
                TrackGaugeStability.PristineStandard => 10,
                TrackGaugeStability.MinorSpread => 80,
                TrackGaugeStability.SevereDistortion => 300,
                _ => 1000
            };

            int wearRisk = (track.TrackWearPermille * 200) / 1000;
            int totalRisk = Math.Min(1000, baseGaugeRisk + wearRisk);

            int speedMult = track.GaugeStability switch
            {
                TrackGaugeStability.PristineStandard => 1000,
                TrackGaugeStability.MinorSpread => 750,
                TrackGaugeStability.SevereDistortion => 400,
                _ => 0
            };

            RailFeasibilityOutcome outcome = totalRisk >= DerailmentThresholdPermille || speedMult < 1000
                ? RailFeasibilityOutcome.SpeedRestricted
                : RailFeasibilityOutcome.Passable;

            return new RailTransitEvaluationResult(outcome, totalRisk, speedMult, string.Empty);
        }

        /// <summary>
        /// Applies track wear and bridge fatigue from a completed train run.
        /// </summary>
        public static void ApplyTrainWear(TrackSegmentState track, LocomotiveClass loco, int cargoTonnage)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));

            int totalTons = GetLocomotiveWeightTons(loco) + Math.Max(0, cargoTonnage);

            // Heavy trains cause higher track wear
            int wearDelta = Math.Max(1, (totalTons * 15) / 100);
            track.TrackWearPermille = Math.Min(1000, track.TrackWearPermille + wearDelta);

            // Bridge integrity fatigue
            if (totalTons > 50)
            {
                int bridgeFatigue = Math.Max(1, (totalTons * 5) / 100);
                track.BridgeIntegrityPermille = Math.Max(0, track.BridgeIntegrityPermille - bridgeFatigue);
            }

            // High wear can induce gauge spread
            if (track.TrackWearPermille >= 800 && track.GaugeStability == TrackGaugeStability.PristineStandard)
            {
                track.GaugeStability = TrackGaugeStability.MinorSpread;
            }
            else if (track.TrackWearPermille >= 950 && track.GaugeStability == TrackGaugeStability.MinorSpread)
            {
                track.GaugeStability = TrackGaugeStability.SevereDistortion;
            }
        }

        /// <summary>
        /// Performs maintenance workgang repairs on a track segment.
        /// </summary>
        public static void PerformMaintenance(
            TrackSegmentState track,
            int repairMaterialPermille,
            int workgangLaborHours)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));

            int mat = Math.Clamp(repairMaterialPermille, 0, 1000);
            int labor = Math.Max(0, workgangLaborHours);

            int repairPower = (mat * 500 + Math.Min(labor, 100) * 500) / 1000;

            track.TrackWearPermille = Math.Max(0, track.TrackWearPermille - repairPower);
            track.BridgeIntegrityPermille = Math.Min(1000, track.BridgeIntegrityPermille + repairPower / 2);
            track.IsBlockedByDebris = false;

            if (track.TrackWearPermille < 400 && track.GaugeStability == TrackGaugeStability.SevereDistortion)
            {
                track.GaugeStability = TrackGaugeStability.MinorSpread;
            }
            if (track.TrackWearPermille < 150 && track.GaugeStability == TrackGaugeStability.MinorSpread)
            {
                track.GaugeStability = TrackGaugeStability.PristineStandard;
            }
        }
    }
}
