// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 39 — The Reagent
// Subsystem    : Chemical Synthesis Safety, Catalyst Purity & Reagent Grade Engine
// Authority    : docs/expansions/wave6/expansion_39_the_reagent_plan.md
//                WAVE6_INDEX.md
// ============================================================================
using System;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Quality grade of synthesized chemical reagents.
    /// </summary>
    public enum ReagentPurityGrade
    {
        Crude            = 0, // Unrefined reaction mass; high impurities (masonry/cleaning only)
        Technical        = 1, // 80–90% purity; industrial/metallurgical grade
        ReagentGrade     = 2, // 95–98% purity; suitable for battery chemistry, fine synthesis
        AnalyticalPharma = 3  // >99% purity; medical and injectable synthesis quality
    }

    /// <summary>
    /// Thermal and pressure operating hazard state of a synthesis reactor vessel.
    /// </summary>
    public enum SynthesisReactorHazardState
    {
        Stable                  = 0, // Operating within normal thermal and pressure envelopes
        ElevatedPressure        = 1, // Pressure accumulation; vent venting or feed reduction needed
        ThermalStress           = 2, // Exothermic heat output outstripping cooling jacket capacity
        ThermalExcursionRunaway = 3  // Auto-catalytic thermal runaway; immediate shutdown or breach
    }

    /// <summary>
    /// Mutable state record of a chemical synthesis reactor vessel.
    /// Extends ChlorAlkaliSynthesisEngine, FischerTropschSynthesisEngine, and BioFermentationEngine seams.
    /// </summary>
    public sealed class SynthesisReactorState
    {
        public string ReactorId                   { get; set; } = string.Empty;
        public int OperatingTemperaturePermille   { get; set; } = 400;
        public int OperatingPressurePermille      { get; set; } = 350;
        public int CatalystActivityPermille       { get; set; } = 900;
        public int FeedstockPurityPermille        { get; set; } = 850;
        public int CoolingCapacityPermille        { get; set; } = 800;
        public SynthesisReactorHazardState HazardState { get; set; } = SynthesisReactorHazardState.Stable;

        public SynthesisReactorState Clone() => new SynthesisReactorState
        {
            ReactorId                 = ReactorId,
            OperatingTemperaturePermille = OperatingTemperaturePermille,
            OperatingPressurePermille  = OperatingPressurePermille,
            CatalystActivityPermille   = CatalystActivityPermille,
            FeedstockPurityPermille    = FeedstockPurityPermille,
            CoolingCapacityPermille    = CoolingCapacityPermille,
            HazardState               = HazardState
        };
    }

    /// <summary>
    /// Immutable result of a chemical synthesis execution cycle.
    /// </summary>
    public readonly struct SynthesisReactionStepResult
    {
        public int OutputBatchKg                       { get; }
        public ReagentPurityGrade PurityGrade          { get; }
        public SynthesisReactorHazardState HazardState { get; }
        public int CatalystDegradationPermille         { get; }
        public int NeutralizingWasteVolumeLitres       { get; }
        public bool IsReactionAborted                  { get; }

        public SynthesisReactionStepResult(
            int outputBatchKg,
            ReagentPurityGrade purityGrade,
            SynthesisReactorHazardState hazardState,
            int catalystDegradationPermille,
            int neutralizingWasteVolumeLitres,
            bool isReactionAborted)
        {
            OutputBatchKg                 = Math.Max(0, outputBatchKg);
            PurityGrade                   = purityGrade;
            HazardState                   = hazardState;
            CatalystDegradationPermille   = Math.Clamp(catalystDegradationPermille, 0, 1000);
            NeutralizingWasteVolumeLitres = Math.Max(0, neutralizingWasteVolumeLitres);
            IsReactionAborted             = isReactionAborted;
        }
    }

    /// <summary>
    /// Immutable mass balance evaluation for precursor inputs and stoichiometric yields.
    /// </summary>
    public readonly struct MassBalanceResult
    {
        public int TheoreticalYieldKg     { get; }
        public int ActualYieldKg          { get; }
        public int ConversionRatePermille { get; }
        public int UnreactedPrecursorKg   { get; }

        public MassBalanceResult(
            int theoreticalYieldKg,
            int actualYieldKg,
            int conversionRatePermille,
            int unreactedPrecursorKg)
        {
            TheoreticalYieldKg     = Math.Max(0, theoreticalYieldKg);
            ActualYieldKg          = Math.Max(0, actualYieldKg);
            ConversionRatePermille = Math.Clamp(conversionRatePermille, 0, 1000);
            UnreactedPrecursorKg   = Math.Max(0, unreactedPrecursorKg);
        }
    }

    /// <summary>
    /// Pure domain engine governing chemical reaction execution, stoichiometric mass balance,
    /// catalyst degradation, reagent purity determination, and hazardous waste neutralization.
    /// Extends ChlorAlkaliSynthesisEngine and PharmaLabSystem seams.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class ChemicalReagentSynthesisEngine
    {
        public const int RunawayTemperatureThresholdPermille = 850;
        public const int RupturePressureThresholdPermille = 900;

        /// <summary>
        /// Executes one synthesis reaction step, evaluating thermal equilibrium, product yield,
        /// catalyst wear, and waste generation.
        /// </summary>
        public static SynthesisReactionStepResult EvaluateReactionStep(
            SynthesisReactorState state,
            int targetBatchKg,
            int reactantRatioPermille,
            int operatorSkillPermille)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            targetBatchKg         = Math.Max(1, targetBatchKg);
            reactantRatioPermille = Math.Clamp(reactantRatioPermille, 0, 1500);
            operatorSkillPermille = Math.Clamp(operatorSkillPermille, 0, 1000);

            // Exothermic heat generation scales with batch size and reactant ratio
            int heatGenerated = (targetBatchKg * 10) + (reactantRatioPermille / 3);
            int coolingMitigation = (state.CoolingCapacityPermille * 8) / 10;
            int netThermalShift = (heatGenerated - coolingMitigation) / 10;

            state.OperatingTemperaturePermille = Math.Clamp(
                state.OperatingTemperaturePermille + netThermalShift, 0, 1000);

            // Pressure scales with temperature
            int pressureShift = (netThermalShift * 6) / 10;
            state.OperatingPressurePermille = Math.Clamp(
                state.OperatingPressurePermille + pressureShift, 0, 1000);

            // Evaluate hazard state
            if (state.OperatingTemperaturePermille >= RunawayTemperatureThresholdPermille &&
                state.OperatingPressurePermille >= RupturePressureThresholdPermille)
            {
                state.HazardState = SynthesisReactorHazardState.ThermalExcursionRunaway;
                return new SynthesisReactionStepResult(
                    0, ReagentPurityGrade.Crude, state.HazardState, 500, targetBatchKg * 3, true);
            }

            if (state.OperatingTemperaturePermille >= 750)
            {
                state.HazardState = SynthesisReactorHazardState.ThermalStress;
            }
            else if (state.OperatingPressurePermille >= 700)
            {
                state.HazardState = SynthesisReactorHazardState.ElevatedPressure;
            }
            else
            {
                state.HazardState = SynthesisReactorHazardState.Stable;
            }

            // Conversion efficiency
            int catalystFactor = state.CatalystActivityPermille;
            int skillFactor    = operatorSkillPermille;
            int conversionRate = (catalystFactor * 60 + skillFactor * 40) / 100;
            if (state.HazardState != SynthesisReactorHazardState.Stable)
            {
                conversionRate = (conversionRate * 80) / 100;
            }

            int outputKg = (targetBatchKg * conversionRate) / 1000;

            // Catalyst activity wear
            int degradation = Math.Max(5, (targetBatchKg * 20) / 100);
            if (state.OperatingTemperaturePermille > 700)
            {
                degradation += 25; // Thermal sintering accelerates poisoning
            }
            state.CatalystActivityPermille = Math.Max(0, state.CatalystActivityPermille - degradation);

            // Purity determination
            int processStability = 1000 - (state.OperatingTemperaturePermille / 2);
            ReagentPurityGrade grade = DeterminePurityGrade(
                state.FeedstockPurityPermille,
                state.CatalystActivityPermille,
                processStability);

            // Neutralizing waste effluent (acids/bases produced as by-product)
            int wasteLitres = Math.Max(5, (targetBatchKg * 12) / 10);

            return new SynthesisReactionStepResult(
                outputKg,
                grade,
                state.HazardState,
                degradation,
                wasteLitres,
                false);
        }

        /// <summary>
        /// Calculates mass balance yield and unconverted precursor mass.
        /// </summary>
        public static MassBalanceResult CalculateMassBalance(
            int precursorInputKg,
            int stoichiometricRatioPermille,
            int conversionRatePermille)
        {
            precursorInputKg            = Math.Max(0, precursorInputKg);
            stoichiometricRatioPermille = Math.Clamp(stoichiometricRatioPermille, 100, 2000);
            conversionRatePermille      = Math.Clamp(conversionRatePermille, 0, 1000);

            int theoreticalYieldKg = (precursorInputKg * stoichiometricRatioPermille) / 1000;
            int actualYieldKg      = (theoreticalYieldKg * conversionRatePermille) / 1000;
            int unreactedPrecursor = precursorInputKg - (precursorInputKg * conversionRatePermille) / 1000;

            return new MassBalanceResult(
                theoreticalYieldKg,
                actualYieldKg,
                conversionRatePermille,
                Math.Max(0, unreactedPrecursor));
        }

        /// <summary>
        /// Maps chemical variables to output reagent purity tiers.
        /// </summary>
        public static ReagentPurityGrade DeterminePurityGrade(
            int feedstockPurityPermille,
            int catalystActivityPermille,
            int processStabilityPermille)
        {
            feedstockPurityPermille  = Math.Clamp(feedstockPurityPermille, 0, 1000);
            catalystActivityPermille = Math.Clamp(catalystActivityPermille, 0, 1000);
            processStabilityPermille = Math.Clamp(processStabilityPermille, 0, 1000);

            int composite = (feedstockPurityPermille * 40 +
                             catalystActivityPermille * 35 +
                             processStabilityPermille * 25) / 100;

            return composite switch
            {
                >= 900 => ReagentPurityGrade.AnalyticalPharma,
                >= 750 => ReagentPurityGrade.ReagentGrade,
                >= 500 => ReagentPurityGrade.Technical,
                _      => ReagentPurityGrade.Crude
            };
        }

        /// <summary>
        /// Computes kilograms of slaked lime or soda ash required to neutralize acidic effluent waste.
        /// </summary>
        public static int CalculateWasteNeutralizationDemand(
            int acidicWasteLitres,
            int acidConcentrationPermille)
        {
            acidicWasteLitres         = Math.Max(0, acidicWasteLitres);
            acidConcentrationPermille = Math.Clamp(acidConcentrationPermille, 0, 1000);

            // Neutralization factor ~0.15 kg alkali per litre at 1000 permille concentration
            int effectiveAcidity = (acidicWasteLitres * acidConcentrationPermille) / 1000;
            return Math.Max(1, (effectiveAcidity * 150) / 1000);
        }
    }
}
