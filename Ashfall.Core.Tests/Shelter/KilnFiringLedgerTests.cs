// SPDX-License-Identifier: MIT
// Expansion 31 — kiln firing ledger (stateful owner over the signed pure engine).

using System;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class KilnFiringLedgerTests
    {
        [Fact]
        public void AddBatch_IsIdGuardedAndClampsQuality()
        {
            var ledger = new KilnFiringLedger();

            Assert.True(ledger.AddBatch("k1", KilnLoadKind.FiredBrick, 5000));
            Assert.False(ledger.AddBatch("k1", KilnLoadKind.FiredBrick, 700));
            Assert.False(ledger.AddBatch("  ", KilnLoadKind.FiredBrick, 700));

            var batch = ledger.FindBatch("k1");
            Assert.NotNull(batch);
            Assert.Equal(1000, batch!.RawMaterialQualityPermille);
            Assert.Equal(0, batch.FiringStage);
        }

        [Fact]
        public void AdvanceFiring_ChargesFuelAndTalliesDraw()
        {
            var ledger = new KilnFiringLedger();
            ledger.AddBatch("k1", KilnLoadKind.RefractoryTile, 900);

            for (int i = 0; i < KilnFiringEngine.MaxFiringStages; i++)
                ledger.AdvanceFiring("k1", KilnFiringLedger.OptimalFiringTemperaturePermille);

            var batch = ledger.FindBatch("k1");
            Assert.Equal(KilnFiringEngine.MaxFiringStages, batch!.FiringStage);
            Assert.False(batch.IsWaster);

            // Fuel is charged per stage, and the draw accrued lining wear.
            Assert.Equal(1000 - (KilnFiringLedger.FuelCostPerStagePermille * KilnFiringEngine.MaxFiringStages),
                ledger.FuelReservePermille);
            Assert.True(ledger.LiningWearPermille > 0);

            var census = ledger.GetCensus();
            Assert.Equal(1, census.DrawnRefractoryTile);
            Assert.Equal(1, census.DrawReadyBatches);
        }

        [Fact]
        public void AdvanceFiring_StarvedKilnHoldsBatchAndFuel()
        {
            var ledger = new KilnFiringLedger(new KilnFiringState { FuelReservePermille = 40 });
            ledger.AddBatch("k1", KilnLoadKind.ClayPottery, 700);

            var batch = ledger.AdvanceFiring("k1");

            Assert.NotNull(batch);
            Assert.Equal(0, batch!.FiringStage);
            Assert.Equal(40, ledger.FuelReservePermille);
        }

        [Fact]
        public void CalcinateLimestone_TalliesQuicklimeAndConsumesFuel()
        {
            var ledger = new KilnFiringLedger();
            int fuelBefore = ledger.FuelReservePermille;

            // 1000‰ is the engine's peak band: tempEfficiency 300 over a full 24h soak
            // drives the calcination rate to 1000‰, i.e. fully calcined.
            var result = ledger.CalcinateLimestone(1000, 1000, 24);

            Assert.True(result.IsFullyCalcined);
            Assert.Equal(0, result.ResidualCarbonatePermille);
            Assert.True(result.QuicklimeYieldKg > 400);
            Assert.Equal(result.QuicklimeYieldKg, ledger.GetCensus().CumulativeQuicklimeKg);
            Assert.True(ledger.FuelReservePermille < fuelBefore);
            // Calcination is the hardest load on the lining.
            Assert.True(ledger.LiningWearPermille > 0);
        }

        [Fact]
        public void CaptureRestore_RoundTripsBatchesFuelAndWear_AndSchemaGates()
        {
            var ledger = new KilnFiringLedger();
            ledger.AddBatch("k1", KilnLoadKind.FiredBrick, 800);
            ledger.AdvanceFiring("k1");
            ledger.CalcinateLimestone(500, 900, 12);
            var state = ledger.CaptureState();
            var before = ledger.GetCensus();

            var restored = new KilnFiringLedger();
            restored.RestoreState(state);
            var after = restored.GetCensus();

            Assert.Equal(before.BatchCount, after.BatchCount);
            Assert.Equal(before.FuelReservePermille, after.FuelReservePermille);
            Assert.Equal(before.LiningWearPermille, after.LiningWearPermille);
            Assert.Equal(before.CumulativeQuicklimeKg, after.CumulativeQuicklimeKg);
            Assert.Equal(ledger.FindBatch("k1")!.FiringStage, restored.FindBatch("k1")!.FiringStage);

            var newer = ledger.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));

            var legacy = ledger.CaptureState();
            legacy.SchemaVersion = 0;
            restored.RestoreState(legacy);
        }

        // kilnworks replay coverage: paired runs must remain identical.
        [Fact]
        public void KilnworksDeterminismReplay_IsStableAcrossIdenticalRuns()
        {
            static string Run()
            {
                var ledger = new KilnFiringLedger();
                ledger.AddBatch("kilnworks_batch", KilnLoadKind.RefractoryTile, 900);
                ledger.AdvanceFiring("kilnworks_batch");
                ledger.CalcinateLimestone(500, 900, 12);
                var census = ledger.GetCensus();
                return $"{census.BatchCount}:{census.FuelReservePermille}:{census.LiningWearPermille}:{census.CumulativeQuicklimeKg}";
            }

            Assert.Equal(Run(), Run());
        }

        [Fact]
        public void RefuelAndReline_ClampAndClearWear()
        {
            var ledger = new KilnFiringLedger(new KilnFiringState
            {
                FuelReservePermille = 950,
                LiningWearPermille = 850
            });

            ledger.Refuel(500);
            Assert.Equal(1000, ledger.FuelReservePermille);
            Assert.True(ledger.GetCensus().IsLiningReplacementDue);

            ledger.Reline(1000);
            Assert.Equal(0, ledger.LiningWearPermille);
            Assert.False(ledger.GetCensus().IsLiningReplacementDue);

            ledger.Reline(-100);
            Assert.Equal(0, ledger.LiningWearPermille);
        }
    }
}
