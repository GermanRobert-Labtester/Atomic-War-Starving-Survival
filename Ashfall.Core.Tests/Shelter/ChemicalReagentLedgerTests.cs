// SPDX-License-Identifier: MIT
using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ChemicalReagentLedgerTests
    {
        [Fact]
        public void DefaultInitialization_CreatesPrimaryVesselAndCensus()
        {
            var ledger = new ChemicalReagentLedger();
            var census = ledger.GetCensus();

            Assert.Equal(1, census.ActiveReactorsCount);
            Assert.Equal(0, census.HazardousReactorsCount);
            Assert.Equal(0, census.AccumulatedAcidicWasteLitres);
            Assert.Equal(250, census.NeutralizingAlkaliStockKg);
            Assert.Equal(0, census.TotalReagentStockKg);
        }

        [Fact]
        public void ExecuteSynthesisCycle_ProducesReagentsAndDegradesCatalyst()
        {
            var ledger = new ChemicalReagentLedger();
            var reactor = ledger.GetReactor("primary_synthesis_vessel");
            Assert.NotNull(reactor);

            int startCatalyst = reactor.CatalystActivityPermille;

            var result = ledger.ExecuteSynthesisCycle(
                reactorId: "primary_synthesis_vessel",
                targetBatchKg: 30,
                reactantRatioPermille: 950,
                operatorSkillPermille: 800,
                timestampTicks: 1001);

            Assert.False(result.IsReactionAborted);
            Assert.True(result.OutputBatchKg > 0);
            Assert.True(reactor.CatalystActivityPermille < startCatalyst);

            var census = ledger.GetCensus();
            Assert.True(census.TotalReagentStockKg > 0);
            Assert.True(census.AccumulatedAcidicWasteLitres > 0);
        }

        [Fact]
        public void ExecuteSynthesisCycle_HandlesThermalRunaway_AndIncrementsAlerts()
        {
            var ledger = new ChemicalReagentLedger();
            ledger.RegisterOrUpdateReactor(new SynthesisReactorState
            {
                ReactorId = "hot_reactor",
                OperatingTemperaturePermille = 860,
                OperatingPressurePermille = 920,
                CoolingCapacityPermille = 50,
                CatalystActivityPermille = 800
            });

            var result = ledger.ExecuteSynthesisCycle(
                reactorId: "hot_reactor",
                targetBatchKg: 40,
                reactantRatioPermille: 1100,
                operatorSkillPermille: 200,
                timestampTicks: 1002);

            Assert.True(result.IsReactionAborted);
            Assert.Equal(SynthesisReactorHazardState.ThermalExcursionRunaway, result.HazardState);

            var census = ledger.GetCensus();
            Assert.True(census.ThermalRunawayCount > 0);
            Assert.True(census.SafetyAlertCount > 0);
        }

        [Fact]
        public void NeutralizeAcidicWaste_DeductsAlkaliStock_AndClearsWaste()
        {
            var ledger = new ChemicalReagentLedger();

            // Run a reaction to generate waste
            ledger.ExecuteSynthesisCycle(
                reactorId: "primary_synthesis_vessel",
                targetBatchKg: 50,
                reactantRatioPermille: 1000,
                operatorSkillPermille: 750,
                timestampTicks: 1003);

            var censusBefore = ledger.GetCensus();
            int wasteBefore = censusBefore.AccumulatedAcidicWasteLitres;
            Assert.True(wasteBefore > 0);

            bool neutralized = ledger.NeutralizeAcidicWaste(
                litresToNeutralize: wasteBefore,
                acidConcentrationPermille: 300);

            Assert.True(neutralized);

            var censusAfter = ledger.GetCensus();
            Assert.Equal(0, censusAfter.AccumulatedAcidicWasteLitres);
            Assert.True(censusAfter.NeutralizingAlkaliStockKg < censusBefore.NeutralizingAlkaliStockKg);
        }

        [Fact]
        public void NeutralizeAcidicWaste_RejectsWhenAlkaliStockInsufficient()
        {
            var ledger = new ChemicalReagentLedger();
            var state = ledger.CaptureState();
            state.NeutralizingAlkaliStockKg = 1; // Insufficient for high-concentration waste
            state.AccumulatedAcidicWasteLitres = 500;
            ledger.RestoreState(state);

            bool neutralized = ledger.NeutralizeAcidicWaste(
                litresToNeutralize: 500,
                acidConcentrationPermille: 900);

            Assert.False(neutralized);
            Assert.Equal(500, ledger.GetCensus().AccumulatedAcidicWasteLitres);
        }

        [Fact]
        public void AdvanceDay_CoolsReactors_AndAlertsOnExcessiveWaste()
        {
            var ledger = new ChemicalReagentLedger();
            ledger.RegisterOrUpdateReactor(new SynthesisReactorState
            {
                ReactorId = "cooling_reactor",
                OperatingTemperaturePermille = 600,
                OperatingPressurePermille = 500,
                HazardState = SynthesisReactorHazardState.ElevatedPressure
            });

            var state = ledger.CaptureState();
            state.AccumulatedAcidicWasteLitres = 600; // Above 500L alert threshold
            ledger.RestoreState(state);

            ledger.AdvanceDay(10);

            var reactor = ledger.GetReactor("cooling_reactor");
            Assert.NotNull(reactor);
            Assert.True(reactor.OperatingTemperaturePermille < 600);
            Assert.True(reactor.OperatingPressurePermille < 500);

            var census = ledger.GetCensus();
            Assert.True(census.SafetyAlertCount > 0);
        }

        [Fact]
        public void CaptureAndRestoreState_PreservesStateFidelity()
        {
            var ledger1 = new ChemicalReagentLedger();
            ledger1.RestockNeutralizingAlkali(300);
            ledger1.ExecuteSynthesisCycle(
                reactorId: "primary_synthesis_vessel",
                targetBatchKg: 25,
                reactantRatioPermille: 900,
                operatorSkillPermille: 950,
                timestampTicks: 2001);

            var captured = ledger1.CaptureState();

            var ledger2 = new ChemicalReagentLedger();
            ledger2.RestoreState(captured);

            var c1 = ledger1.GetCensus();
            var c2 = ledger2.GetCensus();

            Assert.Equal(c1.ActiveReactorsCount, c2.ActiveReactorsCount);
            Assert.Equal(c1.AccumulatedAcidicWasteLitres, c2.AccumulatedAcidicWasteLitres);
            Assert.Equal(c1.NeutralizingAlkaliStockKg, c2.NeutralizingAlkaliStockKg);
            Assert.Equal(c1.TotalReagentStockKg, c2.TotalReagentStockKg);
            Assert.Equal(c1.SafetyAlertCount, c2.SafetyAlertCount);
        }
    }
}
