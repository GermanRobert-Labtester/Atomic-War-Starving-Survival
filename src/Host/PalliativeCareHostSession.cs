// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PalliativeCareSaveStore
// Core Engine: Ashfall.Core.Medical.PalliativeCareDignityEngine
// Host Caller: Main.PalliativeCare
// Purpose    : Expansion 24 The Long Goodbye — terminal palliative care.
//              The Core engine is the sole authority over pain, lucidity,
//              dignity, grief-stage progression, and memorial echoes; the host
//              owns only the patient roster (records) and its persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persisted palliative ward state: the patient roster plus the day ledger.
    /// </summary>
    [Serializable]
    public sealed class PalliativeCareSaveState
    {
        public int schema_version = 1;
        public int currentDay;
        public List<PalliativePatientRecord> patients = new List<PalliativePatientRecord>();
        public List<string> memorialEchoes = new List<string>();
    }

    public static class PalliativeCareSaveStore
    {
        public const string FileName = "palliative_care_save.json";
        public const string SectionName = "palliative_care";

        private static readonly SaveStore<PalliativeCareSaveState> s_store =
            SaveStoreHub.Checksummed<PalliativeCareSaveState>(FileName, nameof(PalliativeCareSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(PalliativeCareSaveState state) => s_store.CaptureBare(state);
        public static PalliativeCareSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(PalliativeCareSaveState state) => s_store.TrySave(state);
        public static PalliativeCareSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core palliative-care engine.</summary>
    public sealed class PalliativeCareHostSession : HostSessionBase
    {
        private readonly PalliativeCareSaveState _state;

        public PalliativeCareHostSession(PalliativeCareSaveState? state = null)
        {
            _state = state ?? new PalliativeCareSaveState();
            _state.patients ??= new List<PalliativePatientRecord>();
            _state.memorialEchoes ??= new List<string>();
        }

        public static PalliativeCareHostSession Create(PalliativeCareSaveState? state = null) =>
            new PalliativeCareHostSession(state);

        public string LastEvent { get; private set; } = string.Empty;
        public int CurrentDay => _state.currentDay;
        public IReadOnlyList<PalliativePatientRecord> Patients => _state.patients;
        public IReadOnlyList<string> MemorialEchoes => _state.memorialEchoes;
        public int PatientCount => _state.patients.Count;

        public PalliativePatientRecord AdmitPatient(
            string survivorId,
            int daysRemainingPrognosis = 7,
            PalliativeCareProtocol protocol = PalliativeCareProtocol.BalancedAnalgesia)
        {
            var existing = _state.patients.FirstOrDefault(p =>
                string.Equals(p.SurvivorId, survivorId, StringComparison.Ordinal));
            if (existing != null) return existing;

            var patient = new PalliativePatientRecord
            {
                SurvivorId = survivorId ?? string.Empty,
                DaysRemainingPrognosis = Math.Max(0, daysRemainingPrognosis),
                ActiveProtocol = protocol
            };
            _state.patients.Add(patient);
            LastEvent = $"Admitted {survivorId} to palliative care.";
            RaiseStateChanged();
            return patient;
        }

        public bool RemovePatient(string survivorId)
        {
            int removed = _state.patients.RemoveAll(p =>
                string.Equals(p.SurvivorId, survivorId, StringComparison.Ordinal));
            if (removed == 0) return false;
            LastEvent = $"Discharged {survivorId} from palliative care.";
            RaiseStateChanged();
            return true;
        }

        public PalliativePatientRecord? FindPatient(string survivorId) => _state.patients.FirstOrDefault(p =>
            string.Equals(p.SurvivorId, survivorId, StringComparison.Ordinal));

        public bool SetProtocol(string survivorId, PalliativeCareProtocol protocol)
        {
            var patient = FindPatient(survivorId);
            if (patient == null) return false;
            patient.ActiveProtocol = protocol;
            LastEvent = $"Set {protocol} protocol for {survivorId}.";
            RaiseStateChanged();
            return true;
        }

        public bool FulfillFinalWish(string survivorId, string questId)
        {
            var patient = FindPatient(survivorId);
            if (patient == null) return false;
            patient.FinalWishQuestId = questId ?? string.Empty;
            patient.FinalWishFulfilled = !string.IsNullOrEmpty(questId);
            LastEvent = $"Final wish for {survivorId} {(patient.FinalWishFulfilled ? "fulfilled" : "cleared")}.";
            RaiseStateChanged();
            return true;
        }

        /// <summary>
        /// Advances one day of care for every patient. All decisions are made by the
        /// Core engine from integer permille facts; this method only supplies the
        /// host-owned supply and skill facts and records the outcome.
        /// </summary>
        public int AdvanceDay(
            int day,
            int medicineAvailabilityPermille,
            int caregiverSkillPermille,
            int worldSeed)
        {
            _state.currentDay = day;
            int expired = 0;
            foreach (var patient in _state.patients)
            {
                var outcome = PalliativeCareDignityEngine.AdvanceDailyCare(
                    patient, medicineAvailabilityPermille, caregiverSkillPermille);
                PalliativeCareDignityEngine.EvaluateGriefStageProgression(patient, day, worldSeed);
                if (outcome.PrognosisExpired) expired++;
            }

            // Patients remain in the ward until <see cref="RecordPassing"/> archives their echo.
            if (expired > 0) LastEvent = $"Palliative day {day} advanced; {expired} prognosis(s) expired.";
            else LastEvent = $"Palliative day {day} advanced for {_state.patients.Count} patient(s).";
            RaiseStateChanged();
            return expired;
        }

        /// <summary>Records the memorial echo left by a terminal survivor.</summary>
        public MemorialLegacyEcho RecordPassing(string survivorId)
        {
            var patient = FindPatient(survivorId) ?? new PalliativePatientRecord { SurvivorId = survivorId ?? string.Empty };
            var echo = PalliativeCareDignityEngine.CalculateMemorialEcho(patient);
            _state.memorialEchoes.Add($"{survivorId}:{echo.MemorialJournalKey}:{(echo.DiedInDignity ? "dignified" : "undignified")}");
            _state.patients.RemoveAll(p => string.Equals(p.SurvivorId, survivorId, StringComparison.Ordinal));
            LastEvent = $"Recorded memorial echo for {survivorId}.";
            RaiseStateChanged();
            return echo;
        }

        public int HighDignityPatientCount => _state.patients.Count(p =>
            p.DignityIndexPermille >= PalliativeCareDignityEngine.HighDignityThresholdPermille);

        public PalliativeCareSaveState CaptureState()
        {
            var copy = new PalliativeCareSaveState
            {
                schema_version = _state.schema_version,
                currentDay = _state.currentDay
            };
            foreach (var p in _state.patients) copy.patients.Add(p.Clone());
            copy.memorialEchoes.AddRange(_state.memorialEchoes);
            return copy;
        }

        public void RestoreState(PalliativeCareSaveState state)
        {
            _state.currentDay = state?.currentDay ?? 0;
            _state.patients.Clear();
            _state.memorialEchoes.Clear();
            if (state == null) return;
            _state.schema_version = state.schema_version;
            foreach (var p in state.patients ?? new List<PalliativePatientRecord>())
                if (p != null) _state.patients.Add(p.Clone());
            if (state.memorialEchoes != null) _state.memorialEchoes.AddRange(state.memorialEchoes);
            LastEvent = "Restored palliative care state.";
            RaiseStateChanged();
        }

        public bool TrySave() => PalliativeCareSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = PalliativeCareSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
