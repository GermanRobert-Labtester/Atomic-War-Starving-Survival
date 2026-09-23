// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 31 — The Kiln
// Subsystem    : Ceramics Firing, Lime Calcination & Masonry Materials Engine
// Authority    : docs/expansions/wave4/expansion_31_the_kiln_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Classification of materials that can be batch-fired in the kiln.
    /// </summary>
    public enum KilnLoadKind
    {
        ClayPottery    = 0,  // cups, vessels, sanitary ware
        FiredBrick     = 1,  // structural masonry
        RefractoryTile = 2,  // furnace linings for CupolaFoundryEngine
        LimestoneCalc  = 3,  // limestone → quicklime calcination
        CeramicTile    = 4   // floor/wall tiles, drainage ware
    }

    /// <summary>
    /// Draw-grade quality of a completed kiln firing.
    /// </summary>
    public enum DrawGrade
    {
        Waster       = 0,  // cracked / unusable; kiln failure
        SubStandard  = 1,  // weak or porous; structural use only
        Standard     = 2,  // fit for purpose
        Superior     = 3,  // dense, well-fired; premium trade good
        ArchivalGrade = 4  // perfect draw; refractory or precision use
    }

    /// <summary>
    /// Mutable state of a kiln batch load queued for firing.
    /// </summary>
    public sealed class KilnBatchState
    {
        public string BatchId              { get; set; } = string.Empty;
        public KilnLoadKind LoadKind       { get; set; } = KilnLoadKind.ClayPottery;
        /// <summary>Raw material quality entering the kiln (0..1000 permille).</summary>
        public int RawMaterialQualityPermille { get; set; } = 700;
        /// <summary>Current firing stage (0 = loaded, 3 = draw-ready).</summary>
        public int FiringStage             { get; set; } = 0;
        /// <summary>Accumulated heat work (0..1000 permille of target soaking heat).</summary>
        public int HeatWorkPermille        { get; set; } = 0;
        /// <summary>Whether the batch suffered thermal shock and cracked.</summary>
        public bool IsWaster               { get; set; } = false;
        /// <summary>Draw grade once firing is complete.</summary>
        public DrawGrade ResultingGrade    { get; set; } = DrawGrade.SubStandard;

        public KilnBatchState Clone() => new KilnBatchState
        {
            BatchId               = BatchId,
            LoadKind              = LoadKind,
            RawMaterialQualityPermille = RawMaterialQualityPermille,
            FiringStage           = FiringStage,
            HeatWorkPermille      = HeatWorkPermille,
            IsWaster              = IsWaster,
            ResultingGrade        = ResultingGrade
        };
    }

    /// <summary>
    /// Immutable result of a lime calcination pass.
    /// </summary>
    public readonly struct LimeCalcinationResult
    {
        /// <summary>Quicklime yield in kilograms from the limestone input.</summary>
        public int QuicklimeYieldKg        { get; }
        /// <summary>Residual calcium carbonate fraction (0..1000 permille; 0 = fully calcined).</summary>
        public int ResidualCarbonatePermille { get; }
        /// <summary>True if the calcination was complete (≤50 permille residual).</summary>
        public bool IsFullyCalcined        { get; }
        /// <summary>Fuel consumed permille of kiln fuel reserve.</summary>
        public int FuelConsumedPermille    { get; }

        public LimeCalcinationResult(
            int quicklimeYieldKg,
            int residualCarbonatePermille,
            bool isFullyCalcined,
            int fuelConsumedPermille)
        {
            QuicklimeYieldKg          = Math.Max(0, quicklimeYieldKg);
            ResidualCarbonatePermille = Math.Clamp(residualCarbonatePermille, 0, 1000);
            IsFullyCalcined           = isFullyCalcined;
            FuelConsumedPermille      = Math.Clamp(fuelConsumedPermille, 0, 1000);
        }
    }

    /// <summary>
    /// Pure domain engine governing kiln batch firing, draw-grade quality,
    /// lime calcination, and refractory lining wear computation.
    /// Extends CeramicsKilnCatalog and MasonryBrickworksCatalog narrative seams;
    /// feeds CupolaFoundryEngine with refractory grades.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class KilnFiringEngine
    {
        /// <summary>Firing stages required to reach draw-ready state.</summary>
        public const int MaxFiringStages = 3;

        /// <summary>Heat work permille at which a batch may thermally shock if ramped too fast.</summary>
        public const int ThermalShockRampThreshold = 500;

        /// <summary>Minimum fuel permille required for any firing operation.</summary>
        public const int MinimumFuelPermille = 80;

        /// <summary>
        /// Advances one firing stage of a kiln batch.
        /// Mutates FiringStage, HeatWorkPermille, IsWaster, and ResultingGrade.
        /// </summary>
        /// <param name="batch">Batch state to mutate.</param>
        /// <param name="kilnTemperaturePermille">
        ///     Kiln temperature precision (0..1000; 1000 = optimal peak fire for the load kind).
        ///     Too fast a ramp (high temp on stage 0) risks thermal shock.
        /// </param>
        /// <param name="fuelAvailablePermille">Available fuel stock (0..1000). Shortfall reduces heat work.</param>
        public static void AdvanceFiringStage(
            KilnBatchState batch,
            int kilnTemperaturePermille,
            int fuelAvailablePermille)
        {
            if (batch == null) throw new ArgumentNullException(nameof(batch));
            if (batch.IsWaster) return;
            if (batch.FiringStage >= MaxFiringStages) return;

            kilnTemperaturePermille = Math.Clamp(kilnTemperaturePermille, 0, 1000);
            fuelAvailablePermille   = Math.Clamp(fuelAvailablePermille, 0, 1000);

            // Thermal shock: fast ramp on cold load (stage 0 at high temperature)
            if (batch.FiringStage == 0 && kilnTemperaturePermille > 800 &&
                batch.HeatWorkPermille < ThermalShockRampThreshold)
            {
                batch.IsWaster = true;
                batch.ResultingGrade = DrawGrade.Waster;
                return;
            }

            // Heat work contribution from this stage (fuel-limited)
            int heatContrib = (kilnTemperaturePermille * Math.Min(fuelAvailablePermille, 1000)) / 1000;
            batch.HeatWorkPermille = Math.Min(1000, batch.HeatWorkPermille + heatContrib / 3);
            batch.FiringStage++;

            // Determine draw grade when fully fired
            if (batch.FiringStage >= MaxFiringStages)
            {
                int effectiveScore = (batch.HeatWorkPermille + batch.RawMaterialQualityPermille) / 2;
                batch.ResultingGrade = effectiveScore switch
                {
                    >= 920 => DrawGrade.ArchivalGrade,
                    >= 800 => DrawGrade.Superior,
                    >= 600 => DrawGrade.Standard,
                    >= 350 => DrawGrade.SubStandard,
                    _      => DrawGrade.Waster
                };
                if (batch.ResultingGrade == DrawGrade.Waster)
                    batch.IsWaster = true;
            }
        }

        /// <summary>
        /// Computes lime calcination yield from a limestone batch at given kiln temperature and soak time.
        /// </summary>
        /// <param name="limestoneKg">Input limestone mass in kilograms.</param>
        /// <param name="kilnTemperaturePermille">
        ///     Operating temperature (0..1000; calcination requires ≥700 permille ≈ 900°C).
        /// </param>
        /// <param name="soakHours">Hours held at peak temperature (1..72).</param>
        /// <param name="fuelAvailablePermille">Available fuel stock permille (0..1000).</param>
        public static LimeCalcinationResult CalcinateLimestone(
            int limestoneKg,
            int kilnTemperaturePermille,
            int soakHours,
            int fuelAvailablePermille)
        {
            limestoneKg             = Math.Max(0, limestoneKg);
            kilnTemperaturePermille = Math.Clamp(kilnTemperaturePermille, 0, 1000);
            soakHours               = Math.Clamp(soakHours, 1, 72);
            fuelAvailablePermille   = Math.Clamp(fuelAvailablePermille, 0, 1000);

            if (kilnTemperaturePermille < 700 || fuelAvailablePermille < MinimumFuelPermille)
            {
                // Insufficient temperature: limestone barely cracks
                int residual = 900 + ((700 - kilnTemperaturePermille) * 100) / 700;
                return new LimeCalcinationResult(0, Math.Min(1000, residual), false,
                    (limestoneKg * 15) / 100);
            }

            // Calcination efficiency: temperature × soak time × fuel
            int tempEfficiency = kilnTemperaturePermille - 700; // 0..300 above threshold
            int soakFactor     = Math.Min(1000, (soakHours * 1000) / 24); // full soak = 24h
            int fuelFactor     = fuelAvailablePermille;
            int calcinationRate = (tempEfficiency * soakFactor / 300 + fuelFactor) / 2;
            calcinationRate     = Math.Clamp(calcinationRate, 0, 1000);

            // CaCO3 → CaO + CO2: ~56% mass yield of quicklime from limestone at full calcination
            int yieldFraction  = calcinationRate;
            int quicklimeYield = (limestoneKg * 56 * yieldFraction) / (100 * 1000);
            int residualCarbonate = Math.Max(0, 1000 - calcinationRate);
            bool fullyCalcined    = residualCarbonate <= 50;

            int fuelConsumed = Math.Min(fuelAvailablePermille,
                (limestoneKg * 20 * soakHours) / (100 * 24));

            return new LimeCalcinationResult(quicklimeYield, residualCarbonate, fullyCalcined, fuelConsumed);
        }

        /// <summary>
        /// Calculates refractory lining wear incurred by one kiln firing session.
        /// Used by CupolaFoundryEngine to track when lining replacement is due.
        /// </summary>
        /// <param name="loadKind">What was fired (refractory tiles wear the lining least; brick most).</param>
        /// <param name="kilnTemperaturePermille">Peak temperature reached (higher = more lining wear).</param>
        /// <returns>Lining wear delta permille (add to accumulated lining wear).</returns>
        public static int CalculateRefractoryLiningWear(KilnLoadKind loadKind, int kilnTemperaturePermille)
        {
            kilnTemperaturePermille = Math.Clamp(kilnTemperaturePermille, 0, 1000);

            int baseWear = loadKind switch
            {
                KilnLoadKind.LimestoneCalc  => 30,  // highest thermal stress
                KilnLoadKind.FiredBrick     => 25,
                KilnLoadKind.CeramicTile    => 18,
                KilnLoadKind.ClayPottery    => 12,
                KilnLoadKind.RefractoryTile => 8,   // self-protecting load
                _                           => 15
            };

            // Temperature accelerates lining degradation above 800 permille
            int tempBonus = kilnTemperaturePermille > 800
                ? ((kilnTemperaturePermille - 800) * 20) / 200
                : 0;

            return Math.Min(100, baseWear + tempBonus);
        }
    }
}
