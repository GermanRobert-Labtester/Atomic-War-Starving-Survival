// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 27 — The Thread
// Subsystem    : Garment Layering & Thermal Insulation Engine
// Authority    : docs/expansions/wave4/expansion_27_the_thread_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Textiles
{
    /// <summary>
    /// Garment layer position in the clothing system.
    /// Layering order matters: Base → Mid → Outer → WeatherShell.
    /// </summary>
    public enum GarmentLayer
    {
        Base         = 0,  // skin-contact undergarments
        Mid          = 1,  // insulating fleece, wool
        Outer        = 2,  // woven coat, leather jacket
        WeatherShell = 3   // wind/rain-proof shell
    }

    /// <summary>
    /// Activity level affecting garment wear rate.
    /// </summary>
    public enum ActivityLevel
    {
        Rest    = 0,
        Light   = 1,
        Moderate = 2,
        Heavy   = 3
    }

    /// <summary>
    /// Mutable state record for a single worn garment.
    /// </summary>
    public sealed class WornGarmentState
    {
        public string GarmentId       { get; set; } = string.Empty;
        public GarmentLayer Layer     { get; set; } = GarmentLayer.Base;
        /// <summary>Thermal insulation value 0..1000 permille (1000 = arctic-grade).</summary>
        public int InsulationPermille { get; set; } = 500;
        /// <summary>Durability remaining 0..1000 permille.</summary>
        public int DurabilityPermille { get; set; } = 1000;
        /// <summary>Dirt/contamination accumulation 0..1000 permille (0 = clean).</summary>
        public int DirtPermille       { get; set; } = 0;
        /// <summary>True if garment is waterproof (WeatherShell layer).</summary>
        public bool IsWaterproof      { get; set; } = false;

        public WornGarmentState Clone() => new WornGarmentState
        {
            GarmentId         = GarmentId,
            Layer             = Layer,
            InsulationPermille = InsulationPermille,
            DurabilityPermille = DurabilityPermille,
            DirtPermille       = DirtPermille,
            IsWaterproof       = IsWaterproof
        };
    }

    /// <summary>
    /// Immutable result of a thermal insulation evaluation pass.
    /// </summary>
    public readonly struct ThermalLayeringResult
    {
        /// <summary>Effective warmth provided to the survivor (0..1000 permille).</summary>
        public int EffectiveWarmthPermille { get; }
        /// <summary>True if a waterproof shell is present and active.</summary>
        public bool HasWaterproofShell     { get; }
        /// <summary>True if layering is optimal (all four layer slots filled).</summary>
        public bool IsFullyLayered         { get; }
        /// <summary>Hygiene penalty from dirt accumulation (-500..0 permille on comfort).</summary>
        public int HygienePenaltyPermille  { get; }

        public ThermalLayeringResult(
            int effectiveWarmthPermille,
            bool hasWaterproofShell,
            bool isFullyLayered,
            int hygienePenaltyPermille)
        {
            EffectiveWarmthPermille = Math.Clamp(effectiveWarmthPermille, 0, 1000);
            HasWaterproofShell      = hasWaterproofShell;
            IsFullyLayered          = isFullyLayered;
            HygienePenaltyPermille  = Math.Clamp(hygienePenaltyPermille, -500, 0);
        }
    }

    /// <summary>
    /// Pure domain engine governing garment layering, thermal insulation,
    /// laundry hygiene restoration, and daily garment wear accumulation.
    /// Extends the NeedsSystem.Warmth seam and WornGear.EffectiveProtection()
    /// without duplicating equipment slot authority.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class GarmentLayeringThermalEngine
    {
        /// <summary>
        /// Dirt permille above this threshold causes a hygiene penalty.
        /// </summary>
        public const int DirtHygienePenaltyThreshold = 300;

        /// <summary>
        /// Layering bonus applied when all four layer slots are filled (+15% warmth).
        /// </summary>
        public const int FullLayeringBonusPermille = 150;

        /// <summary>
        /// Evaluates thermal insulation from a set of worn garments under shelter conditions.
        /// </summary>
        /// <param name="garments">Currently worn garments (any order; duplicates by layer averaged).</param>
        /// <param name="shelterThermalComfortPermille">
        ///     Ambient warmth provided by shelter (0..1000 permille).
        ///     Stacks additively with garment insulation up to the 1000 cap.
        /// </param>
        public static ThermalLayeringResult EvaluateThermalInsulation(
            IReadOnlyList<WornGarmentState> garments,
            int shelterThermalComfortPermille)
        {
            if (garments == null) throw new ArgumentNullException(nameof(garments));

            shelterThermalComfortPermille = Math.Clamp(shelterThermalComfortPermille, 0, 1000);

            // Aggregate insulation by layer; track occupied layers
            int totalInsulation = 0;
            bool hasWaterproofShell = false;
            bool[] layerOccupied = new bool[4];
            int totalDirt = 0;

            foreach (var g in garments)
            {
                // Effective insulation degrades with durability and dirt
                int effectiveDurability = Math.Max(0, g.DurabilityPermille);
                int dirtPenalty = (g.DirtPermille * 200) / 1000; // heavy dirt hurts up to 200 permille
                int effectiveInsulation = ((g.InsulationPermille * effectiveDurability) / 1000)
                                          - dirtPenalty;
                effectiveInsulation = Math.Max(0, effectiveInsulation);

                totalInsulation += effectiveInsulation;
                totalDirt       += g.DirtPermille;

                if (g.IsWaterproof && g.Layer == GarmentLayer.WeatherShell)
                    hasWaterproofShell = true;

                int layerIdx = (int)g.Layer;
                if (layerIdx >= 0 && layerIdx < 4)
                    layerOccupied[layerIdx] = true;
            }

            bool isFullyLayered = layerOccupied[0] && layerOccupied[1] &&
                                   layerOccupied[2] && layerOccupied[3];

            if (isFullyLayered)
                totalInsulation += FullLayeringBonusPermille;

            // Add shelter ambient warmth
            int combinedWarmth = Math.Min(1000, totalInsulation + shelterThermalComfortPermille);

            // Average dirt → hygiene penalty
            int avgDirt = garments.Count > 0 ? totalDirt / garments.Count : 0;
            int hygienePenalty = 0;
            if (avgDirt > DirtHygienePenaltyThreshold)
            {
                int excessDirt = avgDirt - DirtHygienePenaltyThreshold;
                hygienePenalty = -Math.Min(500, (excessDirt * 500) / (1000 - DirtHygienePenaltyThreshold));
            }

            return new ThermalLayeringResult(combinedWarmth, hasWaterproofShell, isFullyLayered, hygienePenalty);
        }

        /// <summary>
        /// Calculates hygiene and dirt restoration after a laundry wash session.
        /// </summary>
        /// <param name="garmentDirtPermille">Current dirt level of the garment (0..1000).</param>
        /// <param name="washQualityPermille">
        ///     Quality of wash resources (0 = hand-rinse cold water, 1000 = soap + hot water).
        /// </param>
        /// <returns>Dirt reduction delta (positive = less dirt; apply as: DirtPermille -= delta).</returns>
        public static int CalculateLaundryHygieneRestoration(int garmentDirtPermille, int washQualityPermille)
        {
            garmentDirtPermille  = Math.Clamp(garmentDirtPermille, 0, 1000);
            washQualityPermille  = Math.Clamp(washQualityPermille, 0, 1000);

            // Base restoration proportional to wash quality; harder to remove stubborn dirt
            int baseRestoration  = (washQualityPermille * 600) / 1000;

            // Diminishing returns for lightly soiled garments
            int cappedByDirt     = Math.Min(baseRestoration, garmentDirtPermille);
            return cappedByDirt;
        }

        /// <summary>
        /// Advances daily garment wear from one day's use at the given activity level.
        /// Modifies DurabilityPermille and DirtPermille in-place.
        /// </summary>
        /// <param name="garment">Garment state to mutate.</param>
        /// <param name="activity">Survivor activity level for the day.</param>
        public static void AdvanceDailyGarmentWear(WornGarmentState garment, ActivityLevel activity)
        {
            if (garment == null) throw new ArgumentNullException(nameof(garment));

            int durabilityLoss = activity switch
            {
                ActivityLevel.Rest     => 3,
                ActivityLevel.Light    => 8,
                ActivityLevel.Moderate => 18,
                ActivityLevel.Heavy    => 35,
                _                      => 10
            };

            int dirtAccumulation = activity switch
            {
                ActivityLevel.Rest     => 5,
                ActivityLevel.Light    => 20,
                ActivityLevel.Moderate => 50,
                ActivityLevel.Heavy    => 90,
                _                      => 30
            };

            // WeatherShell garments take more durability abuse outdoors
            if (garment.Layer == GarmentLayer.WeatherShell)
                durabilityLoss = (durabilityLoss * 150) / 100;

            garment.DurabilityPermille = Math.Max(0, garment.DurabilityPermille - durabilityLoss);
            garment.DirtPermille       = Math.Min(1000, garment.DirtPermille + dirtAccumulation);
        }
    }
}
