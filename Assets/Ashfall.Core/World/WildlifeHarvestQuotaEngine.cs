// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 32 — The Wild
// Subsystem    : Wildlife Harvest Quota, Predator Conflict & Ecology Balance Engine
// Authority    : docs/expansions/wave5/expansion_32_the_wild_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Population pressure band for a fauna species in a sector.
    /// </summary>
    public enum SpeciesPopulationBand
    {
        Extinct        = 0,  // locally gone; no hunting possible
        CriticallyLow  = 1,  // <10% viable population; harvest banned
        Depleted       = 2,  // 10–30% viable; limited quota only
        Recovering     = 3,  // 30–60%; sustainable low harvest
        Stable         = 4,  // 60–90%; full sustainable harvest
        Abundant       = 5   // >90%; can absorb elevated harvest
    }

    /// <summary>
    /// Predator aggression posture when conflict proximity is reached.
    /// </summary>
    public enum PredatorConflictPosture
    {
        Passive     = 0,  // stays away; no interaction
        Territorial = 1,  // warns; retreat resolves
        Aggressive  = 2,  // charges; shelter alert required
        Rampage     = 3   // apex predator breach event
    }

    /// <summary>
    /// Immutable result of a harvest quota evaluation.
    /// </summary>
    public readonly struct HarvestQuotaResult
    {
        /// <summary>Maximum safe harvest units this season (0 = banned).</summary>
        public int MaxSafeHarvestUnits     { get; }
        /// <summary>Population pressure band after this harvest would be applied.</summary>
        public SpeciesPopulationBand PostHarvestBand { get; }
        /// <summary>True if the requested harvest is within the safe quota.</summary>
        public bool IsWithinQuota          { get; }
        /// <summary>Overhunt risk permille if max quota exceeded by 50% (0..1000).</summary>
        public int OverhuntCollapseRiskPermille { get; }

        public HarvestQuotaResult(
            int maxSafeHarvestUnits,
            SpeciesPopulationBand postHarvestBand,
            bool isWithinQuota,
            int overhuntCollapseRiskPermille)
        {
            MaxSafeHarvestUnits          = Math.Max(0, maxSafeHarvestUnits);
            PostHarvestBand              = postHarvestBand;
            IsWithinQuota                = isWithinQuota;
            OverhuntCollapseRiskPermille = Math.Clamp(overhuntCollapseRiskPermille, 0, 1000);
        }
    }

    /// <summary>
    /// Immutable result of a taming readiness evaluation.
    /// </summary>
    public readonly struct TamingReadinessResult
    {
        /// <summary>Taming readiness score (0..1000 permille; ≥700 = tameable).</summary>
        public int ReadinessPermille       { get; }
        /// <summary>True if the animal is ready for a taming attempt.</summary>
        public bool IsTameable             { get; }
        /// <summary>Estimated sessions required to complete taming (1..20).</summary>
        public int EstimatedSessionsNeeded { get; }

        public TamingReadinessResult(int readinessPermille, bool isTameable, int estimatedSessionsNeeded)
        {
            ReadinessPermille       = Math.Clamp(readinessPermille, 0, 1000);
            IsTameable              = isTameable;
            EstimatedSessionsNeeded = Math.Clamp(estimatedSessionsNeeded, 1, 20);
        }
    }

    /// <summary>
    /// Pure domain engine governing sustainable wildlife harvest quotas,
    /// overhunt population collapse curves, predator conflict thresholds,
    /// and taming readiness scoring.
    /// Extends WildlifeEcosystemSystem and WildlifeTrappingSystem seams
    /// without duplicating ecosystem state or trap-site ownership.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class WildlifeHarvestQuotaEngine
    {
        /// <summary>Population permille below which a species is considered CriticallyLow.</summary>
        public const int CriticalPopulationThreshold = 100;  // 10%

        /// <summary>Predator conflict distance threshold below which aggression is evaluated (metres).</summary>
        public const int ConflictProximityMetres = 300;

        /// <summary>Taming readiness permille required for a taming attempt.</summary>
        public const int TamingReadinessThreshold = 700;

        /// <summary>
        /// Evaluates the maximum safe harvest quota for a species in a sector this season.
        /// </summary>
        /// <param name="currentPopulationPermille">
        ///     Current estimated population as permille of viable maximum (0..1000).
        /// </param>
        /// <param name="seasonalReproductionPermille">
        ///     Seasonal reproduction rate permille of population (0..300 typical).
        /// </param>
        /// <param name="requestedHarvestUnits">
        ///     Harvest units the shelter intends to take.
        /// </param>
        public static HarvestQuotaResult EvaluateHarvestQuota(
            int currentPopulationPermille,
            int seasonalReproductionPermille,
            int requestedHarvestUnits)
        {
            currentPopulationPermille    = Math.Clamp(currentPopulationPermille, 0, 1000);
            seasonalReproductionPermille = Math.Clamp(seasonalReproductionPermille, 0, 300);
            requestedHarvestUnits        = Math.Max(0, requestedHarvestUnits);

            // Species band
            SpeciesPopulationBand band = currentPopulationPermille switch
            {
                0        => SpeciesPopulationBand.Extinct,
                <= 100   => SpeciesPopulationBand.CriticallyLow,
                <= 300   => SpeciesPopulationBand.Depleted,
                <= 600   => SpeciesPopulationBand.Recovering,
                <= 900   => SpeciesPopulationBand.Stable,
                _        => SpeciesPopulationBand.Abundant
            };

            // No harvest allowed on extinct or critically low
            if (band <= SpeciesPopulationBand.CriticallyLow)
            {
                return new HarvestQuotaResult(0, band, false, 950);
            }

            // Safe quota = reproduction surplus (only harvest what reproduced, plus small stable draw)
            int reproductionSurplus = (currentPopulationPermille * seasonalReproductionPermille) / 1000;

            int stabilityBonus = band switch
            {
                SpeciesPopulationBand.Depleted   => 0,
                SpeciesPopulationBand.Recovering => reproductionSurplus / 3,
                SpeciesPopulationBand.Stable     => reproductionSurplus * 2 / 3,
                SpeciesPopulationBand.Abundant   => reproductionSurplus,
                _                                => 0
            };

            int maxQuota = (reproductionSurplus + stabilityBonus) / 10; // convert permille-units to harvest units
            maxQuota     = Math.Max(0, maxQuota);

            // Post-harvest band
            int postPopulation = currentPopulationPermille - (requestedHarvestUnits * 5);
            postPopulation     = Math.Clamp(postPopulation, 0, 1000);
            SpeciesPopulationBand postBand = postPopulation switch
            {
                0      => SpeciesPopulationBand.Extinct,
                <= 100 => SpeciesPopulationBand.CriticallyLow,
                <= 300 => SpeciesPopulationBand.Depleted,
                <= 600 => SpeciesPopulationBand.Recovering,
                <= 900 => SpeciesPopulationBand.Stable,
                _      => SpeciesPopulationBand.Abundant
            };

            bool withinQuota = requestedHarvestUnits <= maxQuota;

            // Overhunt collapse risk if quota exceeded by 50%
            int overhuntRisk = 0;
            if (!withinQuota && maxQuota > 0)
            {
                int excess   = requestedHarvestUnits - maxQuota;
                overhuntRisk = Math.Min(1000, (excess * 1000) / (maxQuota + 1));
            }
            else if (band <= SpeciesPopulationBand.Depleted && requestedHarvestUnits > 0)
            {
                overhuntRisk = 400; // always risky to harvest a depleted species
            }

            return new HarvestQuotaResult(maxQuota, postBand, withinQuota, overhuntRisk);
        }

        /// <summary>
        /// Evaluates predator conflict posture based on proximity and population pressure.
        /// </summary>
        /// <param name="predatorPopulationPermille">Predator population pressure (0..1000).</param>
        /// <param name="proximityMetres">Distance from shelter perimeter to predator activity (metres).</param>
        /// <param name="shelterNoisePermille">
        ///     Shelter activity noise level (0..1000; high noise attracts bold predators).
        /// </param>
        public static PredatorConflictPosture EvaluatePredatorConflict(
            int predatorPopulationPermille,
            int proximityMetres,
            int shelterNoisePermille)
        {
            predatorPopulationPermille = Math.Clamp(predatorPopulationPermille, 0, 1000);
            proximityMetres            = Math.Max(0, proximityMetres);
            shelterNoisePermille       = Math.Clamp(shelterNoisePermille, 0, 1000);

            if (proximityMetres > ConflictProximityMetres * 3)
                return PredatorConflictPosture.Passive;

            int aggressionScore = 0;
            if (proximityMetres <= ConflictProximityMetres)
                aggressionScore += 400;
            else if (proximityMetres <= ConflictProximityMetres * 2)
                aggressionScore += 200;

            aggressionScore += (predatorPopulationPermille * 200) / 1000;
            aggressionScore += (shelterNoisePermille * 100) / 1000;

            return aggressionScore switch
            {
                >= 750 => PredatorConflictPosture.Rampage,
                >= 500 => PredatorConflictPosture.Aggressive,
                >= 250 => PredatorConflictPosture.Territorial,
                _      => PredatorConflictPosture.Passive
            };
        }

        /// <summary>
        /// Scores an animal's readiness to begin taming based on hunger, trust exposure,
        /// and species tamability.
        /// </summary>
        /// <param name="animalHungerPermille">
        ///     How hungry the animal is (0..1000; hunger increases receptivity to food-based taming).
        /// </param>
        /// <param name="trustExposurePermille">
        ///     Accumulated passive trust from proximity sessions (0..1000).
        /// </param>
        /// <param name="speciesTamabilityPermille">
        ///     Inherent tamability of the species (0..1000; dogs=900, wolves=400, apex=0).
        /// </param>
        /// <param name="tamingSeed">Deterministic seed for readiness variance.</param>
        public static TamingReadinessResult EvaluateTamingReadiness(
            int animalHungerPermille,
            int trustExposurePermille,
            int speciesTamabilityPermille,
            int tamingSeed)
        {
            animalHungerPermille      = Math.Clamp(animalHungerPermille, 0, 1000);
            trustExposurePermille     = Math.Clamp(trustExposurePermille, 0, 1000);
            speciesTamabilityPermille = Math.Clamp(speciesTamabilityPermille, 0, 1000);

            if (speciesTamabilityPermille == 0)
                return new TamingReadinessResult(0, false, 20);

            int baseReadiness = (animalHungerPermille * 200 +
                                 trustExposurePermille * 500 +
                                 speciesTamabilityPermille * 300) / 1000;

            // Deterministic variance ±8%
            int hash = StableHash.Combine(tamingSeed, animalHungerPermille);
            hash = StableHash.Combine(hash, trustExposurePermille);
            int variance = ((hash & 0x7FFFFFFF) % 17) - 8; // -8..+8
            baseReadiness = Math.Clamp(baseReadiness + (baseReadiness * variance) / 100, 0, 1000);

            bool isTameable = baseReadiness >= TamingReadinessThreshold;
            int sessionsNeeded = isTameable
                ? Math.Max(1, (1000 - baseReadiness) / 80)
                : Math.Min(20, (TamingReadinessThreshold - baseReadiness) / 40 + 5);

            return new TamingReadinessResult(baseReadiness, isTameable, sessionsNeeded);
        }
    }
}
