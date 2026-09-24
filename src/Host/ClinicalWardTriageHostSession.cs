// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ClinicalWardTriageSaveStore
// Core State : Ashfall.Core.Medical.ClinicalWardTriageState
// Host Caller: Main.ClinicalWardTriage
// Purpose    : Expansion 38 — Clinical Ward Triage Priority, Surgical Roster & Sterile Supply
//              host session and persistence.
// ============================================================================

using System;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ClinicalWardTriageSaveStore
    {
        public const string FileName = "clinical_ward_triage_save.json";
        public const string SectionName = "clinical_ward_triage";

        private static readonly SaveStore<ClinicalWardTriageState> s_store =
            SaveStoreHub.Checksummed<ClinicalWardTriageState>(FileName, nameof(ClinicalWardTriageSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ClinicalWardTriageState state) => s_store.CaptureBare(state);
        public static ClinicalWardTriageState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ClinicalWardTriageState state) => s_store.TrySave(state);
        public static ClinicalWardTriageState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 38 host session. Wraps the stateful
    /// <see cref="ClinicalWardLedger"/> over the signed pure
    /// <see cref="ClinicalWardTriageEngine"/>.
    /// MedicalWardSystem and AdvancedSurgicalWardSystem remain the underlying admission and surgical authorities;
    /// this host session manages clinical triage priority, surgical suite readiness, and sterile supply inventory.
    /// </summary>
    public sealed class ClinicalWardTriageHostSession : HostSessionBase
    {
        private readonly ClinicalWardLedger _ledger;

        public ClinicalWardLedger Ledger => _ledger;
        public ClinicalWardCensus Census => _ledger.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public ClinicalWardTriageHostSession(ClinicalWardTriageState? state = null)
        {
            _ledger = new ClinicalWardLedger(state);
        }

        public static ClinicalWardTriageHostSession Create(ClinicalWardTriageState? state = null) =>
            new ClinicalWardTriageHostSession(state);

        public TriageEvaluationResult TriageAndAdmitPatient(
            string patientId,
            int traumaSeverityPermille,
            int vitalStabilityPermille,
            bool isContagious,
            int currentDay)
        {
            var result = _ledger.TriageAndAdmitPatient(patientId, traumaSeverityPermille, vitalStabilityPermille, isContagious, currentDay);
            LastEvent = $"Triaged patient '{patientId}': Priority {result.AssignedPriority}, Bed Available: {result.BedAvailable}, Isolation: {result.RequiresIsolation}.";
            RaiseStateChanged();
            return result;
        }

        public SurgicalReadinessResult PreflightAndExecuteSurgery(
            string procedureId,
            string patientId,
            int procedureComplexityPermille,
            int patientConditionPermille,
            int currentDay,
            int surgerySeed)
        {
            var result = _ledger.PreflightAndExecuteSurgery(
                procedureId, patientId, procedureComplexityPermille, patientConditionPermille, currentDay, surgerySeed);
            LastEvent = result.IsApprovedForSurgery
                ? $"Surgery '{procedureId}' executed for '{patientId}' (Shock: {result.ShockRiskPermille}\u2030, Infection: {result.InfectionRiskPermille}\u2030, Supplies Consumed: {result.SterileSuppliesConsumedPermille}\u2030)."
                : $"Surgery '{procedureId}' denied for '{patientId}': {result.BottleneckReason}";
            RaiseStateChanged();
            return result;
        }

        public bool DischargePatient(string patientId)
        {
            bool success = _ledger.DischargePatient(patientId);
            if (success)
            {
                LastEvent = $"Discharged patient '{patientId}' from clinical ward.";
                RaiseStateChanged();
            }
            return success;
        }

        public void RestockSterileSupplies(int amountPermille)
        {
            _ledger.RestockSterileSupplies(amountPermille);
            LastEvent = $"Restocked sterile supplies by {amountPermille}\u2030.";
            RaiseStateChanged();
        }

        public void RunAutoclaveCycle()
        {
            _ledger.RunAutoclaveCycle();
            LastEvent = "Ran autoclave sterilization cycle; surgical theater restored to SterileField.";
            RaiseStateChanged();
        }

        public void SetWardCleanliness(WardCleanlinessGrade grade)
        {
            _ledger.SetWardCleanliness(grade);
            LastEvent = $"Ward cleanliness standard updated to {grade}.";
            RaiseStateChanged();
        }

        public void SetStaffingReadiness(int staffingPermille)
        {
            _ledger.SetStaffingReadiness(staffingPermille);
            LastEvent = $"Clinical staffing readiness updated to {staffingPermille}\u2030.";
            RaiseStateChanged();
        }

        public void AdvanceDay(int currentDay, int seed)
        {
            _ledger.AdvanceDay(currentDay, seed);
            LastEvent = $"Advanced clinical ward operations for day {currentDay}.";
            RaiseStateChanged();
        }

        public ClinicalWardTriageState CaptureState() => _ledger.CaptureState();

        public void RestoreState(ClinicalWardTriageState? state)
        {
            _ledger.RestoreState(state);
            LastEvent = "Restored clinical ward triage state from snapshot.";
            RaiseStateChanged();
        }
    }
}
