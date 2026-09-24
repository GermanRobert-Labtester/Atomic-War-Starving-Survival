// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class HealthHistorySaveStore
    {
        public const string SectionName = "health_history";
        public const string FileName = "health_history_save.json";

        private static readonly SaveStore<HealthHistoryState> s_store =
            SaveStoreHub.Checksummed<HealthHistoryState>(FileName, nameof(HealthHistorySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string? TryCapturePersisted(HealthHistoryState state) => s_store.CaptureBare(state);
        public static HealthHistoryState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(HealthHistoryState state) => s_store.TrySave(state);
        public static HealthHistoryState? TryLoad() => s_store.TryLoad();
    }

    public sealed class HealthHistoryHostSession : HostSessionBase
    {
        public HealthHistorySystem System { get; }

        public HealthHistoryHostSession(HealthHistorySystem? system = null)
        {
            System = system ?? new HealthHistorySystem();
            System.OnRecordAdded += _ => RaiseStateChanged();
            System.OnRecordResolved += _ => RaiseStateChanged();
            System.OnHealthEventLogged += _ => RaiseStateChanged();
            System.OnVaccinationAdministered += _ => RaiseStateChanged();
            System.OnBoosterDueAlert += _ => RaiseStateChanged();
            System.OnTrendRecorded += _ => RaiseStateChanged();
        }

        public static HealthHistoryHostSession Create(string dataDir, HealthHistoryState? restoredState = null)
        {
            var system = new HealthHistorySystem();
            if (restoredState != null)
            {
                system.RestoreState(restoredState);
            }

            string catalogPath = Path.Combine(dataDir, "medical_record_templates.json");
            if (File.Exists(catalogPath))
            {
                system.LoadCatalog(File.ReadAllText(catalogPath));
            }

            return new HealthHistoryHostSession(system);
        }

        public MedicalRecord LogRecord(
            string survivorId,
            string recordType,
            string description,
            string severity,
            int day,
            string medicId = "",
            string outcome = "ongoing",
            int durationDays = 0,
            List<string>? treatments = null,
            string notes = "")
        {
            var rec = System.LogRecord(survivorId, recordType, description, severity, day, medicId, outcome, durationDays, treatments, notes);
            RaiseStateChanged();
            return rec;
        }

        public bool ResolveRecord(string recordId, string outcome, int day, string notes = "")
        {
            bool ok = System.ResolveRecord(recordId, outcome, day, notes);
            if (ok) RaiseStateChanged();
            return ok;
        }

        public VaccinationRecord AdministerVaccine(
            string survivorId,
            string vaccineType,
            int day,
            string medicId = "",
            float initialImmunity = 100.0f,
            int durationDays = 60)
        {
            var vac = System.AdministerVaccine(survivorId, vaccineType, day, medicId, initialImmunity, durationDays);
            RaiseStateChanged();
            return vac;
        }

        public List<HealthTrend> RecordDailyHealthTrend(
            string survivorId,
            int day,
            float overallHealth,
            float radiationDose,
            float immuneStrength,
            int chronicCount)
        {
            var trends = System.RecordDailyHealthTrend(survivorId, day, overallHealth, radiationDose, immuneStrength, chronicCount);
            RaiseStateChanged();
            return trends;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public HealthHistoryCensus GetCensus() => System.GetCensus();

        public override void Save()
        {
            if (!IsDirty) return;
            HealthHistorySaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
