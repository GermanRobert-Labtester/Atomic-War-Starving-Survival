using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Criticality priority tiers for shelter electrical loads.
    /// </summary>
    public enum LoadPriorityTier
    {
        Tier4_Comfort = 0,      // Non-essential lighting, leisure, heat luxury (shed first)
        Tier3_Industrial = 1,   // Workshop, foundry, material processing
        Tier2_Agricultural = 2, // Greenhouse lamps, hydroponic pumps
        Tier1_Clinical = 3,     // Medical ward, trauma bay, vaccine refrigeration
        Tier0_LifeSupport = 4   // Main air scrubbers, sump pumps, oxygen generation (shed last)
    }

    /// <summary>
    /// Represents a consumer load request on the microgrid.
    /// </summary>
    public readonly struct SubgridLoadDemand
    {
        public string ConsumerId { get; }
        public LoadPriorityTier Priority { get; }
        public int DemandKw { get; }

        public SubgridLoadDemand(string consumerId, LoadPriorityTier priority, int demandKw)
        {
            ConsumerId = consumerId ?? string.Empty;
            Priority = priority;
            DemandKw = Math.Max(0, demandKw);
        }
    }

    /// <summary>
    /// Immutable load allocation and shedding result produced by <see cref="PowerLoadSheddingEngine"/>.
    /// </summary>
    public readonly struct LoadSheddingEvaluationResult
    {
        public int AvailableGenerationKw { get; }
        public int TotalDemandKw { get; }
        public int ServedLoadKw { get; }
        public int ShedLoadKw { get; }
        public int BrownoutRiskPermille { get; }
        public int CascadingTripRiskPermille { get; }
        public int EnergyPovertyMoralePenaltyPermille { get; }
        public bool IsBlackout { get; }
        public IReadOnlyList<string> ShedConsumers { get; }

        public LoadSheddingEvaluationResult(
            int availableGenerationKw,
            int totalDemandKw,
            int servedLoadKw,
            int shedLoadKw,
            int brownoutRiskPermille,
            int cascadingTripRiskPermille,
            int energyPovertyMoralePenaltyPermille,
            bool isBlackout,
            IReadOnlyList<string> shedConsumers)
        {
            AvailableGenerationKw = availableGenerationKw;
            TotalDemandKw = totalDemandKw;
            ServedLoadKw = servedLoadKw;
            ShedLoadKw = shedLoadKw;
            BrownoutRiskPermille = brownoutRiskPermille;
            CascadingTripRiskPermille = cascadingTripRiskPermille;
            EnergyPovertyMoralePenaltyPermille = energyPovertyMoralePenaltyPermille;
            IsBlackout = isBlackout;
            ShedConsumers = shedConsumers ?? Array.Empty<string>();
        }
    }

    /// <summary>
    /// Pure domain engine for microgrid load shedding, brownout risk calculation,
    /// and cascading trip evaluation (Expansion 21: The Grid).
    /// Operates without engine dependencies or duplicate power grid state.
    /// </summary>
    public static class PowerLoadSheddingEngine
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Evaluates load shedding order, brownout vulnerability, and cascading breaker trips.
        /// </summary>
        /// <param name="availableGenerationKw">Available generated and stored power in kW.</param>
        /// <param name="demands">Collection of active subgrid demands.</param>
        /// <param name="gridWearPermille">Substation and transformer wear (0..1000).</param>
        /// <returns>Immutable <see cref="LoadSheddingEvaluationResult"/>.</returns>
        public static LoadSheddingEvaluationResult Evaluate(
            int availableGenerationKw,
            IEnumerable<SubgridLoadDemand> demands,
            int gridWearPermille = 0)
        {
            if (availableGenerationKw < 0) availableGenerationKw = 0;
            gridWearPermille = Math.Max(0, Math.Min(PermilleScale, gridWearPermille));

            var sortedDemands = new List<SubgridLoadDemand>();
            int totalDemand = 0;
            if (demands != null)
            {
                foreach (var d in demands)
                {
                    sortedDemands.Add(d);
                    totalDemand += d.DemandKw;
                }
            }

            // Sort: highest priority (Tier0_LifeSupport = 4) served first, lowest priority (Tier4_Comfort = 0) shed first
            sortedDemands.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            int remainingPower = availableGenerationKw;
            int servedPower = 0;
            int shedPower = 0;
            var shedList = new List<string>();

            int shedComfortCount = 0;
            int shedIndustrialCount = 0;
            int shedAgriculturalCount = 0;
            int shedClinicalCount = 0;
            int shedLifeSupportCount = 0;

            foreach (var load in sortedDemands)
            {
                if (remainingPower >= load.DemandKw)
                {
                    remainingPower -= load.DemandKw;
                    servedPower += load.DemandKw;
                }
                else
                {
                    // Insufficient power; shed this consumer
                    shedPower += load.DemandKw;
                    shedList.Add(load.ConsumerId);

                    switch (load.Priority)
                    {
                        case LoadPriorityTier.Tier4_Comfort:
                            shedComfortCount++;
                            break;
                        case LoadPriorityTier.Tier3_Industrial:
                            shedIndustrialCount++;
                            break;
                        case LoadPriorityTier.Tier2_Agricultural:
                            shedAgriculturalCount++;
                            break;
                        case LoadPriorityTier.Tier1_Clinical:
                            shedClinicalCount++;
                            break;
                        case LoadPriorityTier.Tier0_LifeSupport:
                            shedLifeSupportCount++;
                            break;
                    }
                }
            }

            // Brownout risk calculation:
            // When total demand is close to or exceeds available generation (>85% load factor)
            int brownoutRisk = 0;
            if (availableGenerationKw > 0)
            {
                int loadFactorPermille = Math.Min(PermilleScale, (servedPower * PermilleScale) / availableGenerationKw);
                if (loadFactorPermille > 850)
                {
                    brownoutRisk = ((loadFactorPermille - 850) * PermilleScale) / 150; // up to 1000
                }
            }
            else if (totalDemand > 0)
            {
                brownoutRisk = PermilleScale;
            }

            // Grid wear compounds brownout risk
            brownoutRisk = Math.Min(PermilleScale, brownoutRisk + (gridWearPermille / 4));

            // Cascading trip risk:
            // High brownout risk combined with severe grid wear and unserved heavy loads
            int cascadeRisk = 0;
            if (brownoutRisk > 400 && shedPower > 0)
            {
                cascadeRisk = ((brownoutRisk - 400) * (PermilleScale + gridWearPermille)) / (PermilleScale * 2);
            }
            cascadeRisk = Math.Max(0, Math.Min(PermilleScale, cascadeRisk));

            // Energy poverty morale penalty:
            // Shedding comfort: -30 permille per load
            // Shedding agricultural: -60 permille (crop loss worry)
            // Shedding clinical: -150 permille (fear of death/decay)
            // Shedding life support: -400 permille (panic)
            int moralePenalty = (shedComfortCount * 30) +
                                (shedIndustrialCount * 20) +
                                (shedAgriculturalCount * 60) +
                                (shedClinicalCount * 150) +
                                (shedLifeSupportCount * 400);
            moralePenalty = Math.Min(PermilleScale, moralePenalty);

            bool isBlackout = (availableGenerationKw == 0 && totalDemand > 0) || (servedPower == 0 && totalDemand > 0);

            return new LoadSheddingEvaluationResult(
                availableGenerationKw: availableGenerationKw,
                totalDemandKw: totalDemand,
                servedLoadKw: servedPower,
                shedLoadKw: shedPower,
                brownoutRiskPermille: brownoutRisk,
                cascadingTripRiskPermille: cascadeRisk,
                energyPovertyMoralePenaltyPermille: moralePenalty,
                isBlackout: isBlackout,
                shedConsumers: shedList
            );
        }
    }
}
