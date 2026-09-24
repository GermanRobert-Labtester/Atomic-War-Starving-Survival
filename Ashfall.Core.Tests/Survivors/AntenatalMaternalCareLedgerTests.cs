// SPDX-License-Identifier: MIT
// Expansion 37 — The Quickening : AntenatalMaternalCareLedger tests
using System;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class AntenatalMaternalCareLedgerTests
    {
        [Fact]
        public void RegisterPregnancy_InitializesState_AndReflectsInCensus()
        {
            var ledger = new AntenatalMaternalCareLedger();
            ledger.SetClinicQuality(750);
            ledger.SetSanitationQuality(800);

            var preg = ledger.RegisterPregnancy("survivor_alice", gestationDays: 15, nutritionReservePermille: 850);

            Assert.NotNull(preg);
            Assert.Equal("survivor_alice", preg.MotherSurvivorId);
            Assert.Equal(15, preg.GestationDays);
            Assert.Equal(850, preg.MaternalNutritionReservePermille);
            Assert.Equal(750, preg.MedicalSupervisionQualityPermille);
            Assert.Equal(800, preg.ShelterSanitationQualityPermille);

            var census = ledger.GetCensus();
            Assert.Equal(1, census.ActivePregnanciesCount);
            Assert.Equal(1, census.StableCount);
            Assert.Equal(0, census.LaborReadyCount);
            Assert.Equal(0, census.TotalDeliveriesCount);
            Assert.Equal(750, census.ShelterClinicQualityPermille);
        }

        [Fact]
        public void IsPregnant_ReflectsRegistration_AndPostpartumTransition()
        {
            var ledger = new AntenatalMaternalCareLedger();

            Assert.False(ledger.IsPregnant("survivor_unknown"));
            Assert.False(ledger.IsPregnant(string.Empty));

            ledger.RegisterPregnancy("survivor_jane", gestationDays: 100);
            Assert.True(ledger.IsPregnant("survivor_jane"));

            ledger.ResolveDelivery("survivor_jane", birthSeed: 7, childId: "child_test");
            Assert.False(ledger.IsPregnant("survivor_jane"));
        }

        [Fact]
        public void AdvanceDay_ProgressesGestation_AcrossTrimesterBoundaries()
        {
            var ledger = new AntenatalMaternalCareLedger();
            ledger.RegisterPregnancy("survivor_beth", gestationDays: 90);

            var results = ledger.AdvanceDay(10, _ => 1200, _ => 8);

            Assert.True(results.ContainsKey("survivor_beth"));
            Assert.Equal(GestationTrimester.SecondTrimester, results["survivor_beth"].Trimester);

            var preg = ledger.GetPregnancy("survivor_beth");
            Assert.NotNull(preg);
            Assert.Equal(91, preg.GestationDays);
        }

        [Fact]
        public void AdvanceDay_UnderCaloricDeficit_ReducesReserves()
        {
            var ledger = new AntenatalMaternalCareLedger();
            ledger.RegisterPregnancy("survivor_carol", gestationDays: 120, nutritionReservePermille: 800);

            // Severe underfeeding: only 400 permille intake provided
            ledger.AdvanceDay(11, _ => 400, _ => 6);

            var preg = ledger.GetPregnancy("survivor_carol");
            Assert.NotNull(preg);
            Assert.True(preg.MaternalNutritionReservePermille < 800,
                $"Nutrition reserve should drop below 800 under deficit, was {preg.MaternalNutritionReservePermille}");
        }

        [Fact]
        public void ResolveDelivery_CreatesRecord_AndTransitionsToPostpartum()
        {
            var ledger = new AntenatalMaternalCareLedger();
            ledger.SetClinicQuality(900);
            ledger.SetSanitationQuality(900);
            ledger.RegisterPregnancy("survivor_diana", gestationDays: 275, nutritionReservePermille: 950, fatiguePermille: 100);

            Assert.True(ledger.IsLaborReady("survivor_diana"));

            var resolution = ledger.ResolveDelivery("survivor_diana", birthSeed: 42, childId: "child_hope");

            Assert.Equal(BirthOutcomeClassification.Healthy, resolution.Outcome);
            Assert.True(resolution.NeonatalVigorPermille >= 700);

            var preg = ledger.GetPregnancy("survivor_diana");
            Assert.NotNull(preg);
            Assert.True(preg.IsPostpartum);

            Assert.Single(ledger.DeliveryHistory);
            var history = ledger.DeliveryHistory[0];
            Assert.Equal("survivor_diana", history.MotherSurvivorId);
            Assert.Equal("child_hope", history.ChildId);
            Assert.Equal(BirthOutcomeClassification.Healthy, history.Outcome);

            var census = ledger.GetCensus();
            Assert.Equal(0, census.ActivePregnanciesCount); // Postpartum not active pregnancy
            Assert.Equal(1, census.TotalDeliveriesCount);
        }

        [Fact]
        public void SaveAndRestore_PreservesAllActivePregnanciesAndHistory()
        {
            var ledger = new AntenatalMaternalCareLedger();
            ledger.SetClinicQuality(820);
            ledger.SetSanitationQuality(780);
            ledger.RegisterPregnancy("survivor_elena", gestationDays: 200, nutritionReservePermille: 700);

            var captured = ledger.CaptureState();
            Assert.NotNull(captured);

            var freshLedger = new AntenatalMaternalCareLedger();
            freshLedger.RestoreState(captured);

            Assert.Equal(820, freshLedger.ShelterClinicQualityPermille);
            Assert.Equal(780, freshLedger.ShelterSanitationQualityPermille);

            var restoredPreg = freshLedger.GetPregnancy("survivor_elena");
            Assert.NotNull(restoredPreg);
            Assert.Equal(200, restoredPreg.GestationDays);
            Assert.Equal(700, restoredPreg.MaternalNutritionReservePermille);
            Assert.Equal(820, restoredPreg.MedicalSupervisionQualityPermille);
        }

        [Fact]
        public void Census_AccuratelyReflectsActiveRiskCategories()
        {
            var ledger = new AntenatalMaternalCareLedger();
            ledger.SetClinicQuality(100); // poor clinic
            ledger.SetSanitationQuality(100); // poor sanitation

            // Stable pregnancy
            ledger.RegisterPregnancy("surv_stable", gestationDays: 50, nutritionReservePermille: 950, fatiguePermille: 50);

            // Critical pregnancy
            ledger.RegisterPregnancy("surv_crit", gestationDays: 265, nutritionReservePermille: 150, fatiguePermille: 900);

            var census = ledger.GetCensus();
            Assert.Equal(2, census.ActivePregnanciesCount);
            Assert.True(census.CriticalCount >= 1, "Critical count should be at least 1");
            Assert.Equal(1, census.LaborReadyCount); // surv_crit is >= 260 days
        }
    }
}
