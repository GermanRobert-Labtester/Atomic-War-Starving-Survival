// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 38 — The Ward: Clinical Triage Priority, Surgical Roster
// & Sterile Supply host wiring.
// The signed pure ClinicalWardTriageEngine is the calculation authority.
// MedicalWardSystem and AdvancedSurgicalWardSystem remain the admission and surgical authorities;
// this host owns the active clinical triage priority, surgical suite readiness, and sterile supply inventory.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ClinicalWardTriageHostSession? _clinicalWardTriage;
        private bool _clinicalWardTriageDirty;

        public ClinicalWardTriageHostSession? ClinicalWardTriage => _clinicalWardTriage;

        public void SetupClinicalWardTriage()
        {
            if (_clinicalWardTriage != null) return;

            var saved = ClinicalWardTriageSaveStore.TryLoad();
            _clinicalWardTriage = ClinicalWardTriageHostSession.Create(saved);
            _clinicalWardTriage.StateChanged += () => _clinicalWardTriageDirty = true;
        }

        public TriageEvaluationResult TriageClinicalPatient(
            string patientId,
            int traumaSeverityPermille,
            int vitalStabilityPermille,
            bool isContagious,
            int currentDay)
        {
            SetupClinicalWardTriage();
            return _clinicalWardTriage!.TriageAndAdmitPatient(
                patientId, traumaSeverityPermille, vitalStabilityPermille, isContagious, currentDay);
        }

        public SurgicalReadinessResult PreflightAndExecuteSurgery(
            string procedureId,
            string patientId,
            int procedureComplexityPermille,
            int patientConditionPermille,
            int currentDay,
            int surgerySeed)
        {
            SetupClinicalWardTriage();
            return _clinicalWardTriage!.PreflightAndExecuteSurgery(
                procedureId, patientId, procedureComplexityPermille, patientConditionPermille, currentDay, surgerySeed);
        }

        public bool DischargeClinicalPatient(string patientId)
        {
            SetupClinicalWardTriage();
            return _clinicalWardTriage!.DischargePatient(patientId);
        }

        public void RestockWardSterileSupplies(int amountPermille)
        {
            SetupClinicalWardTriage();
            _clinicalWardTriage!.RestockSterileSupplies(amountPermille);
        }

        public void RunWardAutoclaveCycle()
        {
            SetupClinicalWardTriage();
            _clinicalWardTriage!.RunAutoclaveCycle();
        }

        public void SetWardCleanlinessGrade(WardCleanlinessGrade grade)
        {
            SetupClinicalWardTriage();
            _clinicalWardTriage!.SetWardCleanliness(grade);
        }

        public void SetWardStaffingReadiness(int staffingPermille)
        {
            SetupClinicalWardTriage();
            _clinicalWardTriage!.SetStaffingReadiness(staffingPermille);
        }

        public void AdvanceClinicalWardDay(int currentDay, int seed)
        {
            SetupClinicalWardTriage();
            _clinicalWardTriage!.AdvanceDay(currentDay, seed);
        }

        public ClinicalWardCensus GetClinicalWardCensus() =>
            _clinicalWardTriage?.Census ?? default;

        public void SaveClinicalWardTriage()
        {
            if (_clinicalWardTriage == null) return;
            var state = _clinicalWardTriage.Ledger.CaptureState();
            ClinicalWardTriageSaveStore.TrySave(state);
            if (CaptureSection(ClinicalWardTriageSaveStore.SectionName, ClinicalWardTriageSaveStore.TryCapturePersisted(state)))
                _clinicalWardTriageDirty = false;
        }

        public void FlushClinicalWardTriageIfDirty()
        {
            if (_clinicalWardTriageDirty)
                SaveClinicalWardTriage();
        }

        public void ResetClinicalWardTriage()
        {
            _clinicalWardTriage = null;
            _clinicalWardTriageDirty = false;
        }
    }
}
