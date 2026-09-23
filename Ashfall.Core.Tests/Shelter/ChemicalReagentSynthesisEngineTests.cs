// SPDX-License-Identifier: MIT
// Expansion 39 — The Reagent : ChemicalReagentSynthesisEngine focused tests
using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ChemicalReagentSynthesisEngineTests
    {
        // ── 1. Excessive heat and pressure trigger ThermalExcursionRunaway and abort ──
        [Fact]
        public void EvaluateReactionStep_AbortsReaction_OnRunawayConditions()
        {
            var reactor = new SynthesisReactorState
            {
                ReactorId = "reactor-01",
                OperatingTemperaturePermille = 850,
                OperatingPressurePermille = 920,
                CoolingCapacityPermille = 100, // Insufficient cooling
                CatalystActivityPermille = 800
            };

            var result = ChemicalReagentSynthesisEngine.EvaluateReactionStep(
                state: reactor,
                targetBatchKg: 50,
                reactantRatioPermille: 1200,
                operatorSkillPermille: 300);

            Assert.True(result.IsReactionAborted);
            Assert.Equal(SynthesisReactorHazardState.ThermalExcursionRunaway, result.HazardState);
            Assert.Equal(0, result.OutputBatchKg);
        }

        // ── 2. Controlled reaction yields output and degrades catalyst ──
        [Fact]
        public void EvaluateReactionStep_ProducesYield_AndWearsCatalyst_UnderStableConditions()
        {
            var reactor = new SynthesisReactorState
            {
                ReactorId = "reactor-02",
                OperatingTemperaturePermille = 300,
                OperatingPressurePermille = 250,
                CoolingCapacityPermille = 900,
                CatalystActivityPermille = 950,
                FeedstockPurityPermille = 900
            };

            int startCatalyst = reactor.CatalystActivityPermille;

            var result = ChemicalReagentSynthesisEngine.EvaluateReactionStep(
                state: reactor,
                targetBatchKg: 20,
                reactantRatioPermille: 1000,
                operatorSkillPermille: 850);

            Assert.False(result.IsReactionAborted);
            Assert.True(result.OutputBatchKg > 0);
            Assert.True(reactor.CatalystActivityPermille < startCatalyst,
                "Catalyst should degrade after reaction cycle");
            Assert.True(result.NeutralizingWasteVolumeLitres > 0);
        }

        // ── 3. High feedstock and catalyst purity produce AnalyticalPharma grade ──
        [Fact]
        public void DeterminePurityGrade_YieldsAnalyticalPharma_UnderHighPurityAndStability()
        {
            var grade = ChemicalReagentSynthesisEngine.DeterminePurityGrade(
                feedstockPurityPermille: 950,
                catalystActivityPermille: 920,
                processStabilityPermille: 900);

            Assert.Equal(ReagentPurityGrade.AnalyticalPharma, grade);
        }

        // ── 4. Mass balance correctly computes actual yield and unconverted precursor ──
        [Fact]
        public void CalculateMassBalance_AccuratelyConservesMass()
        {
            // 100 kg precursor with 1000 permille stoichiometric ratio (1:1) at 80% (800 permille) conversion
            var balance = ChemicalReagentSynthesisEngine.CalculateMassBalance(
                precursorInputKg: 100,
                stoichiometricRatioPermille: 1000,
                conversionRatePermille: 800);

            Assert.Equal(100, balance.TheoreticalYieldKg);
            Assert.Equal(80, balance.ActualYieldKg);
            Assert.Equal(20, balance.UnreactedPrecursorKg);
            Assert.Equal(800, balance.ConversionRatePermille);
        }

        // ── 5. Waste neutralization demand scales with volume and concentration ──
        [Fact]
        public void CalculateWasteNeutralizationDemand_ScalesWithVolumeAndConcentration()
        {
            int lowDemand = ChemicalReagentSynthesisEngine.CalculateWasteNeutralizationDemand(
                acidicWasteLitres: 100,
                acidConcentrationPermille: 200);

            int highDemand = ChemicalReagentSynthesisEngine.CalculateWasteNeutralizationDemand(
                acidicWasteLitres: 500,
                acidConcentrationPermille: 800);

            Assert.True(highDemand > lowDemand * 5,
                $"High concentration and volume ({highDemand}) should require substantially more neutralizing agent than low ({lowDemand})");
        }
    }
}
