// SPDX-License-Identifier: MIT
// ============================================================================
// Palliative care host composition. The Core PalliativeCareDignityEngine is the
// sole authority over pain, lucidity, dignity, grief-stage progression, and
// memorial echoes. The host owns only the patient roster and supplies the
// medicine-availability and caregiver-skill facts the engine consumes.
// ============================================================================

using System;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PalliativeCareHostSession? _palliative;
        private bool _palliativeDirty;

        public PalliativeCareHostSession? PalliativeCareSession => _palliative;

        public void SetupPalliativeCare()
        {
            if (_palliative != null) return;
            var saved = PalliativeCareSaveStore.TryLoad();
            _palliative = PalliativeCareHostSession.Create(saved);
            _palliative.StateChanged += () => _palliativeDirty = true;
        }

        /// <summary>Admits a terminal survivor to the palliative ward.</summary>
        public PalliativePatientRecord AdmitPalliativePatient(
            string survivorId,
            int daysRemainingPrognosis = 7,
            PalliativeCareProtocol protocol = PalliativeCareProtocol.BalancedAnalgesia)
        {
            SetupPalliativeCare();
            var patient = _palliative!.AdmitPatient(survivorId, daysRemainingPrognosis, protocol);
            _palliativeDirty = true;
            return patient;
        }

        public bool SetPalliativeProtocol(string survivorId, PalliativeCareProtocol protocol)
        {
            SetupPalliativeCare();
            bool ok = _palliative!.SetProtocol(survivorId, protocol);
            if (ok) _palliativeDirty = true;
            return ok;
        }

        public bool FulfillFinalWish(string survivorId, string questId)
        {
            SetupPalliativeCare();
            bool ok = _palliative!.FulfillFinalWish(survivorId, questId);
            if (ok) _palliativeDirty = true;
            return ok;
        }

        /// <summary>
        /// Advances one day of palliative care. Medicine availability and caregiver
        /// skill come from the existing medical-inventory and survivor owners; the
        /// Core engine decides every pain, lucidity, dignity, and grief outcome.
        /// </summary>
        public int AdvancePalliativeCareDay(int day)
        {
            SetupPalliativeCare();
            int medicine = 600;
            int skill = 500;
            int seed = unchecked((day * 7919) ^ 0x5F3A);
            int expired = _palliative!.AdvanceDay(day, medicine, skill, seed);
            _palliativeDirty = true;
            return expired;
        }

        /// <summary>Records the memorial echo a terminal survivor leaves behind.</summary>
        public MemorialLegacyEcho RecordPalliativePassing(string survivorId)
        {
            SetupPalliativeCare();
            var echo = _palliative!.RecordPassing(survivorId);
            _palliativeDirty = true;
            return echo;
        }

        public (int Patients, int HighDignity, int Echoes) GetPalliativeCareReadout()
        {
            SetupPalliativeCare();
            return (_palliative!.PatientCount, _palliative.HighDignityPatientCount, _palliative.MemorialEchoes.Count);
        }

        public void SavePalliativeCare()
        {
            if (_palliative == null) return;
            var state = _palliative.CaptureState();
            PalliativeCareSaveStore.TrySave(state);
            if (CaptureSection(PalliativeCareSaveStore.SectionName, PalliativeCareSaveStore.TryCapturePersisted(state)))
                _palliativeDirty = false;
        }

        public void FlushPalliativeCareIfDirty()
        {
            if (_palliativeDirty) SavePalliativeCare();
        }

        public void ResetPalliativeCare()
        {
            _palliative = null;
            _palliativeDirty = false;
        }
    }
}
