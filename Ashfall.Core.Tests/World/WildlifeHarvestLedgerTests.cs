// SPDX-License-Identifier: MIT
// Expansion 32 — wildlife harvest ledger (stateful owner over DEC-86).

using System;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class WildlifeHarvestLedgerTests
    {
        [Fact]
        public void EvaluateQuota_ProtectsLowPopulationsAndNeverMutates()
        {
            var ledger = new WildlifeHarvestLedger();
            var extinct = ledger.EvaluateQuota("s1", 0, 100, 3);
            Assert.Equal(0, extinct.MaxSafeHarvestUnits);
            Assert.False(extinct.IsWithinQuota);

            // A read-only evaluation records the species record but no harvest.
            Assert.Equal(1, ledger.TrackedSpecies);
            Assert.Equal(0, ledger.GetCensus().TotalHarvestTaken);
        }

        [Fact]
        public void ApplyHarvest_RecordsUnitsAndFlagsOverQuota()
        {
            var ledger = new WildlifeHarvestLedger();
            ledger.ApplyHarvest("s1", 650, 200, 1);
            ledger.ApplyHarvest("s1", 650, 200, 500);

            var census = ledger.GetCensus();
            Assert.Equal(1, census.TrackedSpecies);
            Assert.Equal(501, census.TotalHarvestTaken);
            Assert.True(census.SpeciesOverQuota >= 1);
        }

        [Fact]
        public void BeginSeason_ResetsCountersButKeepsSpecies()
        {
            var ledger = new WildlifeHarvestLedger();
            ledger.ApplyHarvest("s1", 650, 200, 3);
            ledger.BeginSeason(4);

            Assert.Equal(0, ledger.GetCensus().TotalHarvestTaken);
            Assert.Equal(1, ledger.TrackedSpecies);
            Assert.Equal(4, ledger.Season);
        }

        [Fact]
        public void CaptureRestore_RoundTripsAndSchemaGates()
        {
            var ledger = new WildlifeHarvestLedger();
            ledger.ApplyHarvest("s1", 950, 250, 5);
            var state = ledger.CaptureState();

            var restored = new WildlifeHarvestLedger();
            restored.RestoreState(state);
            Assert.Equal(ledger.GetCensus().TotalHarvestTaken, restored.GetCensus().TotalHarvestTaken);

            var newer = ledger.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));

            var legacy = ledger.CaptureState();
            legacy.SchemaVersion = 0;
            restored.RestoreState(legacy);
            Assert.Equal(1, restored.CaptureState().SchemaVersion);
        }

        [Fact]
        public void PredatorAndTamingEvaluations_AreReadOnlyAndDeterministic()
        {
            var ledger = new WildlifeHarvestLedger();
            var conflict = ledger.EvaluatePredatorConflict(900, 100, 900);
            var tameA = ledger.EvaluateTamingReadiness(800, 1000, 900, 7);
            var tameB = ledger.EvaluateTamingReadiness(800, 1000, 900, 7);

            Assert.True(conflict >= PredatorConflictPosture.Aggressive);
            Assert.Equal(tameA.ReadinessPermille, tameB.ReadinessPermille);
            Assert.Equal(0, ledger.TrackedSpecies);
        }
    }
}
