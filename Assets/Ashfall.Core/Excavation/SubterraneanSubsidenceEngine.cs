// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 18 — The Underneath
// Subsystem    : Subterranean Subsidence & Excavation Integrity Engine
// Authority    : docs/expansions/wave2/expansion_18_the_underneath_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Excavation
{
    /// <summary>
    /// Depth tiers corresponding to subterranean zones beneath the shelter foundation.
    /// </summary>
    public enum DepthTier
    {
        Tier1_ShallowBasement = 1,
        Tier2_ServiceTunnels = 2,
        Tier3_DeepBedrock = 3,
        Tier4_SaltAquifer = 4,
        Tier5_AbyssalCrystalline = 5
    }

    /// <summary>
    /// Geological strata types with distinct load-bearing capacities and water dissolution risks.
    /// </summary>
    public enum StrataType
    {
        SandstoneUnconsolidated = 0,
        LimestoneKarst = 1,
        GraniteSolid = 2,
        SaltSeam = 3,
        BasaltShield = 4
    }

    /// <summary>
    /// Severity classification for surface subsidence and ground destabilization.
    /// </summary>
    public enum SubsidenceCategory
    {
        Negligible = 0,
        Low = 1,
        Moderate = 2,
        Severe = 3,
        CatastrophicCollapse = 4
    }

    /// <summary>
    /// Snapshot of a subterranean excavation node for subsidence evaluation.
    /// </summary>
    public sealed class ExcavationNodeProfile
    {
        public string NodeId { get; set; } = string.Empty;
        public DepthTier Tier { get; set; } = DepthTier.Tier1_ShallowBasement;
        public StrataType Strata { get; set; } = StrataType.GraniteSolid;
        public int VoidVolumeCubicMeters { get; set; } = 100;
        public int ShoringLevel { get; set; } = 0; // 0..3
        public int StructuralIntegrityPermille { get; set; } = 1000; // 0..1000
    }

    /// <summary>
    /// Result of evaluating structural stability and surface subsidence risk.
    /// </summary>
    public readonly struct SubsidenceEvaluationResult
    {
        public string NodeId { get; }
        public int SubsidenceRiskPermille { get; }
        public SubsidenceCategory Category { get; }
        public int SurfaceDistortionMm { get; }
        public int InducedSeismicRiskPermille { get; }
        public bool RequiresImmediateEvacuation { get; }

        public SubsidenceEvaluationResult(
            string nodeId,
            int subsidenceRiskPermille,
            SubsidenceCategory category,
            int surfaceDistortionMm,
            int inducedSeismicRiskPermille,
            bool requiresImmediateEvacuation)
        {
            NodeId = nodeId ?? string.Empty;
            SubsidenceRiskPermille = Math.Max(0, Math.Min(1000, subsidenceRiskPermille));
            Category = category;
            SurfaceDistortionMm = Math.Max(0, surfaceDistortionMm);
            InducedSeismicRiskPermille = Math.Max(0, Math.Min(1000, inducedSeismicRiskPermille));
            RequiresImmediateEvacuation = requiresImmediateEvacuation;
        }
    }

    /// <summary>
    /// Result of an induced seismic tremor check.
    /// </summary>
    public readonly struct SeismicTriggerOutcome
    {
        public bool TremorTriggered { get; }
        public int TremorMagnitude { get; } // 1..6
        public int IntegrityDamagePermille { get; }
        public int NewIntegrityPermille { get; }
        public bool CaveInOccurred { get; }

        public SeismicTriggerOutcome(
            bool tremorTriggered,
            int tremorMagnitude,
            int integrityDamagePermille,
            int newIntegrityPermille,
            bool caveInOccurred)
        {
            TremorTriggered = tremorTriggered;
            TremorMagnitude = tremorMagnitude;
            IntegrityDamagePermille = integrityDamagePermille;
            NewIntegrityPermille = Math.Max(0, Math.Min(1000, newIntegrityPermille));
            CaveInOccurred = caveInOccurred;
        }
    }

    /// <summary>
    /// Pure domain engine evaluating subterranean subsidence, structural integrity decay,
    /// and induced seismic events across excavation depth tiers.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class SubterraneanSubsidenceEngine
    {
        public const int MaxShoringLevel = 3;
        public const int EvacuationThresholdPermille = 750;
        public const int CaveInIntegrityThresholdPermille = 200;

        /// <summary>
        /// Returns the base bearing resilience factor (permille) for a strata type.
        /// </summary>
        public static int GetStrataResiliencePermille(StrataType strata)
        {
            return strata switch
            {
                StrataType.SandstoneUnconsolidated => 350,
                StrataType.LimestoneKarst => 550,
                StrataType.GraniteSolid => 900,
                StrataType.SaltSeam => 300,
                StrataType.BasaltShield => 950,
                _ => 500
            };
        }

        /// <summary>
        /// Returns the shoring mitigation factor (permille discount against subsidence).
        /// </summary>
        public static int GetShoringMitigationPermille(int shoringLevel)
        {
            return Math.Clamp(shoringLevel, 0, MaxShoringLevel) switch
            {
                0 => 0,
                1 => 300,  // Timber props: 30% risk reduction
                2 => 600,  // Steel T-beams: 60% risk reduction
                3 => 850,  // Reinforced concrete arch: 85% risk reduction
                _ => 0
            };
        }

        /// <summary>
        /// Evaluates the surface subsidence risk and induced seismic risk for an excavation profile.
        /// </summary>
        public static SubsidenceEvaluationResult EvaluateSubsidence(ExcavationNodeProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));

            int strataResilience = GetStrataResiliencePermille(profile.Strata);
            int shoringMitigation = GetShoringMitigationPermille(profile.ShoringLevel);

            // Shallow excavations transfer more void pressure directly to the surface.
            // Tier 1 transfers 100%, Tier 2 transfers 70%, Tier 3 transfers 40%, Tier 4 transfers 25%, Tier 5 transfers 10%.
            int depthTransferPermille = profile.Tier switch
            {
                DepthTier.Tier1_ShallowBasement => 1000,
                DepthTier.Tier2_ServiceTunnels => 700,
                DepthTier.Tier3_DeepBedrock => 400,
                DepthTier.Tier4_SaltAquifer => 250,
                DepthTier.Tier5_AbyssalCrystalline => 100,
                _ => 500
            };

            // Void load component: scaled by volume (e.g. 500 m³ = full baseline load 500 permille)
            int voidLoadPermille = Math.Clamp(profile.VoidVolumeCubicMeters, 0, 1000);

            // Structural weakness: inverse of integrity
            int weaknessPermille = 1000 - Math.Clamp(profile.StructuralIntegrityPermille, 0, 1000);

            // Strata vulnerability: 1000 - resilience
            int strataVulnerability = 1000 - strataResilience;

            // Combined raw subsidence load
            long combinedRaw = ((long)voidLoadPermille * depthTransferPermille) / 1000;
            combinedRaw += ((long)weaknessPermille * strataVulnerability) / 1000;

            // Apply shoring reduction
            long mitigated = combinedRaw * (1000 - shoringMitigation) / 1000;
            int subsidenceRisk = Math.Clamp((int)mitigated, 0, 1000);

            // Estimated surface distortion in millimeters: up to 500mm at 1000 permille
            int distortionMm = (subsidenceRisk * 500) / 1000;

            // Induced seismic risk: deep high-stress voids trigger seismic shifts rather than surface drop
            int seismicDepthFactor = profile.Tier switch
            {
                DepthTier.Tier1_ShallowBasement => 150,
                DepthTier.Tier2_ServiceTunnels => 300,
                DepthTier.Tier3_DeepBedrock => 700,
                DepthTier.Tier4_SaltAquifer => 850,
                DepthTier.Tier5_AbyssalCrystalline => 1000,
                _ => 500
            };

            long rawSeismic = ((long)voidLoadPermille * weaknessPermille) / 1000;
            rawSeismic = (rawSeismic * seismicDepthFactor) / 1000;
            long mitigatedSeismic = rawSeismic * (1000 - shoringMitigation) / 1000;
            int inducedSeismicRisk = Math.Clamp((int)mitigatedSeismic, 0, 1000);

            // Categorization
            SubsidenceCategory category = subsidenceRisk switch
            {
                < 100 => SubsidenceCategory.Negligible,
                < 300 => SubsidenceCategory.Low,
                < 600 => SubsidenceCategory.Moderate,
                < 850 => SubsidenceCategory.Severe,
                _ => SubsidenceCategory.CatastrophicCollapse
            };

            bool evac = subsidenceRisk >= EvacuationThresholdPermille ||
                        profile.StructuralIntegrityPermille <= CaveInIntegrityThresholdPermille;

            return new SubsidenceEvaluationResult(
                profile.NodeId,
                subsidenceRisk,
                category,
                distortionMm,
                inducedSeismicRisk,
                evac);
        }

        /// <summary>
        /// Evaluates whether an induced seismic event or cave-in triggers on a given simulation tick.
        /// </summary>
        public static SeismicTriggerOutcome TryTriggerInducedSeismicEvent(
            ExcavationNodeProfile profile,
            long simTick,
            int worldSeed)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));

            var eval = EvaluateSubsidence(profile);
            if (eval.InducedSeismicRiskPermille < 50)
            {
                return new SeismicTriggerOutcome(false, 0, 0, profile.StructuralIntegrityPermille, false);
            }

            // Deterministic hash roll. HashCode.Combine is process-randomized
            // in .NET and cannot participate in a replayable simulation.
            int hash = StableHash.Combine(worldSeed, profile.NodeId);
            hash = StableHash.Combine(hash, simTick);
            hash = StableHash.Combine(hash, profile.StructuralIntegrityPermille);
            int rollPermille = StableHash.NonNegativeRemainder(hash, 1000);

            if (rollPermille < eval.InducedSeismicRiskPermille)
            {
                // Tremor triggers. Magnitude scales with depth tier and void volume.
                int mag = 1 + (eval.InducedSeismicRiskPermille * 5) / 1000;
                mag = Math.Clamp(mag, 1, 6);

                // Integrity damage inflicted by the tremor: 50 to 300 permille
                int damage = 50 + (mag * 40);
                int newIntegrity = Math.Max(0, profile.StructuralIntegrityPermille - damage);
                bool caveIn = newIntegrity <= CaveInIntegrityThresholdPermille;

                return new SeismicTriggerOutcome(true, mag, damage, newIntegrity, caveIn);
            }

            return new SeismicTriggerOutcome(false, 0, 0, profile.StructuralIntegrityPermille, false);
        }

        /// <summary>
        /// Calculates daily natural integrity decay caused by water saturation and strata weakness.
        /// </summary>
        public static int CalculateDailyIntegrityDecayPermille(
            ExcavationNodeProfile profile,
            int waterLevelPermille)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));

            int water = Math.Clamp(waterLevelPermille, 0, 1000);
            int strataVulnerability = 1000 - GetStrataResiliencePermille(profile.Strata);
            int shoringReduction = GetShoringMitigationPermille(profile.ShoringLevel);

            // Salt seams dissolve rapidly under water; granite resists
            int waterSensitivity = profile.Strata == StrataType.SaltSeam ? 2000 : 1000;

            long rawDecay = ((long)strataVulnerability * 15) / 1000; // Base 0..15 permille/day
            long waterDecay = ((long)water * waterSensitivity * 25) / 1_000_000; // Up to 25-50 permille/day from flood

            long totalDecay = (rawDecay + waterDecay) * (1000 - shoringReduction) / 1000;
            return Math.Clamp((int)totalDecay, 0, 200);
        }
    }
}
