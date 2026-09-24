// SPDX-License-Identifier: MIT
// Expansion 38 — The Ward : ClinicalWardLedger unit tests
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class ClinicalWardLedgerTests
    {
        [Fact]
        public void TriageAndAdmitPatient_AllocatesBedsAndTracksIsolation()
        {
            var ledger = new ClinicalWardLedger();
            ledger.State.Ward.TotalBeds = 5;
            ledger.State.Ward.OccupiedBeds = 0;
            ledger.State.Ward.IsolationBedsTotal = 2;
            ledger.State.Ward.IsolationBedsOccupied = 0;

            // Non-contagious triage
            var result1 = ledger.TriageAndAdmitPatient("patient_1", 400, 800, false, 10);
            Assert.True(result1.BedAvailable);
            Assert.Equal(1, ledger.State.Ward.OccupiedBeds);
            Assert.Equal(0, ledger.State.Ward.IsolationBedsOccupied);
            Assert.Single(ledger.State.ActiveAdmissions);

            // Contagious triage
            var result2 = ledger.TriageAndAdmitPatient("patient_2", 700, 300, true, 10);
            Assert.True(result2.BedAvailable);
            Assert.True(result2.RequiresIsolation);
            Assert.Equal(TriagePriorityTier.Immediate, result2.AssignedPriority);
            Assert.Equal(1, ledger.State.Ward.OccupiedBeds);
            Assert.Equal(1, ledger.State.Ward.IsolationBedsOccupied);
            Assert.Equal(2, ledger.State.ActiveAdmissions.Count);
        }

        [Fact]
        public void TriageAndAdmitPatient_HandlesBedSaturation()
        {
            var ledger = new ClinicalWardLedger();
            ledger.State.Ward.TotalBeds = 1;
            ledger.State.Ward.OccupiedBeds = 1;

            var result = ledger.TriageAndAdmitPatient("patient_overflow", 200, 900, false, 5);
            Assert.False(result.BedAvailable);
            Assert.Empty(ledger.State.ActiveAdmissions);
        }

        [Fact]
        public void PreflightAndExecuteSurgery_RecordsOutcomeAndConsumesSterileSupplies()
        {
            var ledger = new ClinicalWardLedger();
            ledger.State.Ward.Cleanliness = WardCleanlinessGrade.SterileField;
            ledger.State.Ward.SterileSupplyStockPermille = 800;
            ledger.State.Ward.StaffingReadinessPermille = 800;

            var result = ledger.PreflightAndExecuteSurgery(
                procedureId: "proc_laparotomy",
                patientId: "patient_trauma",
                procedureComplexityPermille: 500,
                patientConditionPermille: 700,
                currentDay: 12,
                surgerySeed: 42);

            Assert.True(result.IsApprovedForSurgery);
            Assert.True(result.SterileSuppliesConsumedPermille > 0);
            Assert.Equal(800 - result.SterileSuppliesConsumedPermille, ledger.State.Ward.SterileSupplyStockPermille);
            Assert.Single(ledger.State.SurgicalHistory);
            Assert.Equal(1, ledger.State.TotalSurgeriesPerformed);
        }

        [Fact]
        public void DischargePatient_FreesBedsAndUpdatesCensus()
        {
            var ledger = new ClinicalWardLedger();
            ledger.State.Ward.TotalBeds = 5;
            ledger.State.Ward.OccupiedBeds = 0;

            ledger.TriageAndAdmitPatient("patient_x", 350, 600, false, 1);
            Assert.Equal(1, ledger.State.Ward.OccupiedBeds);

            bool discharged = ledger.DischargePatient("patient_x");
            Assert.True(discharged);
            Assert.Equal(0, ledger.State.Ward.OccupiedBeds);
            Assert.Empty(ledger.State.ActiveAdmissions);

            var census = ledger.GetCensus();
            Assert.Equal(5, census.AvailableBeds);
            Assert.Equal(0, census.OccupiedBeds);
        }

        [Fact]
        public void AdvanceDay_TracksTenureAndDrainsSupplies()
        {
            var ledger = new ClinicalWardLedger();
            ledger.State.Ward.TotalBeds = 10;
            ledger.State.Ward.SterileSupplyStockPermille = 500;

            ledger.TriageAndAdmitPatient("patient_stay", 200, 900, false, 1);
            Assert.Equal(0, ledger.State.ActiveAdmissions["patient_stay"].DaysInWard);

            ledger.AdvanceDay(currentDay: 2, seed: 12345);
            Assert.Equal(1, ledger.State.ActiveAdmissions["patient_stay"].DaysInWard);
            Assert.Equal(495, ledger.State.Ward.SterileSupplyStockPermille); // 5 permille consumed
        }

        [Fact]
        public void CaptureAndRestore_PreservesAllStateAndHistory()
        {
            var ledger1 = new ClinicalWardLedger();
            ledger1.State.Ward.Cleanliness = WardCleanlinessGrade.SterileField;
            ledger1.State.Ward.SterileSupplyStockPermille = 900;
            ledger1.TriageAndAdmitPatient("patient_alpha", 500, 500, true, 3);
            ledger1.PreflightAndExecuteSurgery("proc_amputation", "patient_alpha", 600, 500, 3, 99);
            ledger1.RunAutoclaveCycle();

            var snapshot = ledger1.CaptureState();

            var ledger2 = new ClinicalWardLedger();
            ledger2.RestoreState(snapshot);

            Assert.Equal(WardCleanlinessGrade.SterileField, ledger2.State.Ward.Cleanliness);
            Assert.Equal(1, ledger2.State.AutoclaveCycleCount);
            Assert.Equal(1, ledger2.State.TotalSurgeriesPerformed);
            Assert.Single(ledger2.State.ActiveAdmissions);
            Assert.Single(ledger2.State.SurgicalHistory);

            var census = ledger2.GetCensus();
            Assert.Equal(1, census.ActiveAdmissionsCount);
            Assert.Equal(1, census.SurgeriesPerformedCount);
        }
    }
}
