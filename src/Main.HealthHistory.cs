// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private HealthHistoryHostSession? _healthHistory;
        private bool _healthHistoryDirty;

        public HealthHistoryHostSession EnsureHealthHistory()
        {
            if (_healthHistory != null) return _healthHistory;
            SetupHealthHistory();
            return _healthHistory!;
        }

        private void SetupHealthHistory()
        {
            if (_healthHistory != null) return;

            string dataDir = CatalogPath.ResolveDataDir();
            var saved = HealthHistorySaveStore.TryLoad();
            _healthHistory = HealthHistoryHostSession.Create(dataDir, saved);

            _healthHistory.StateChanged += () =>
            {
                _healthHistoryDirty = true;
            };
        }

        private void SaveHealthHistory()
        {
            if (_healthHistory == null) return;

            var state = _healthHistory.System.CaptureState();
            HealthHistorySaveStore.TrySave(state);
            string? payload = HealthHistorySaveStore.TryCapturePersisted(state);
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(HealthHistorySaveStore.SectionName, payload);
            }
            _healthHistoryDirty = false;
        }

        public void FlushHealthHistorySave()
        {
            if (_healthHistoryDirty)
            {
                SaveHealthHistory();
            }
        }

        public void ResetHealthHistory()
        {
            _healthHistory = null;
            _healthHistoryDirty = false;
        }

        public MedicalRecord LogMedicalRecord(
            string survivorId,
            string recordType,
            string description,
            string severity,
            string medicId = "",
            string outcome = "ongoing",
            int durationDays = 0,
            List<string>? treatments = null,
            string notes = "")
        {
            var session = EnsureHealthHistory();
            var rec = session.LogRecord(survivorId, recordType, description, severity, _simDay, medicId, outcome, durationDays, treatments, notes);

            _journal?.TryAddRawEntry(
                "medical_record_logged",
                $"Medical record filed for {survivorId}: {description} (Severity: {severity}, Type: {recordType}).",
                null!,
                _simDay);

            return rec;
        }

        public bool ResolveMedicalRecord(string recordId, string outcome, string notes = "")
        {
            var session = EnsureHealthHistory();
            bool ok = session.ResolveRecord(recordId, outcome, _simDay, notes);
            if (ok)
            {
                _journal?.TryAddRawEntry(
                    "medical_record_resolved",
                    $"Medical record {recordId} outcome marked: {outcome}.",
                    null!,
                    _simDay);
            }
            return ok;
        }

        public VaccinationRecord AdministerVaccine(string survivorId, string vaccineType, string medicId = "")
        {
            var session = EnsureHealthHistory();
            var vac = session.AdministerVaccine(survivorId, vaccineType, _simDay, medicId);

            _journal?.TryAddRawEntry(
                "vaccination_administered",
                $"Administered {vaccineType} inoculation to {survivorId} (Booster due day {vac.BoosterDueDay}).",
                null!,
                _simDay);

            return vac;
        }

        public List<HealthTrend> RecordDailyHealthTrend(
            string survivorId,
            float overallHealth,
            float radiationDose,
            float immuneStrength,
            int chronicCount)
        {
            var session = EnsureHealthHistory();
            return session.RecordDailyHealthTrend(survivorId, _simDay, overallHealth, radiationDose, immuneStrength, chronicCount);
        }

        public HealthHistoryCensus GetHealthHistoryCensus()
        {
            return EnsureHealthHistory().GetCensus();
        }
    }
}
