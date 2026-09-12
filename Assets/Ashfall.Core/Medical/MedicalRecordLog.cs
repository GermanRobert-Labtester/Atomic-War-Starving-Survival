// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// One recorded medical-pipeline fact. Identifiers and a day only — never
    /// free-text notes, diagnosis prose, or player-visible commentary.
    /// </summary>
    [Serializable]
    public sealed class MedicalRecordEntry
    {
        public int day;
        /// <summary>Stable event kind: diagnosis_confirmed | treatment_completed | patient_recovered | ...</summary>
        public string kind = string.Empty;
        public string survivorId = string.Empty;
        /// <summary>Affliction/treatment/protocol id. Never prose.</summary>
        public string detail = string.Empty;
    }

    /// <summary>
    /// Plan 193/198 — the single medical-record owner: a bounded, append-only log
    /// of pipeline events that were already emitted. It replaces the absent
    /// `HealthHistorySystem`/`ChronicConditionSystem`; chronic presentation stays
    /// derived from existing domain flags.
    ///
    /// <para><b>Retention &amp; privacy rule (signed with this package):</b></para>
    /// <list type="bullet">
    /// <item>Only clinical facts are recorded: day, event kind, survivor id, and
    /// the affliction/treatment/protocol id. <b>No free-text notes</b> are ever
    /// stored, so there is no narrative to leak or stigmatize.</item>
    /// <item>The log is bounded to <see cref="MaxEntries"/>; the oldest entry is
    /// evicted first, so save size cannot grow without limit over a campaign.</item>
    /// <item>Retention is campaign-scoped: the log persists with the pipeline
    /// section and is cleared only when a new campaign starts.</item>
    /// <item>The dose ledger stays radiation-only and is never copied here.</item>
    /// </list>
    /// </summary>
    public sealed class MedicalRecordLog
    {
        /// <summary>Maximum retained entries. Oldest evicted beyond this.</summary>
        public const int MaxEntries = 64;

        private readonly List<MedicalRecordEntry> _entries = new List<MedicalRecordEntry>();

        /// <summary>Oldest-first view of the retained record.</summary>
        public IReadOnlyList<MedicalRecordEntry> Entries => _entries;

        public int Count => _entries.Count;

        /// <summary>
        /// Appends one fact. Unknown/blank kinds are ignored so the log cannot
        /// become a scratch buffer. Null or empty detail is allowed (some events
        /// carry only a kind).
        /// </summary>
        public void Append(int day, string kind, string survivorId, string detail = "")
        {
            if (string.IsNullOrEmpty(kind)) return;

            _entries.Add(new MedicalRecordEntry
            {
                day = day,
                kind = kind,
                survivorId = survivorId ?? string.Empty,
                detail = detail ?? string.Empty
            });

            if (_entries.Count > MaxEntries)
                _entries.RemoveAt(0);
        }

        /// <summary>Most recent entries for one survivor, newest first (bounded).</summary>
        public List<MedicalRecordEntry> ForSurvivor(string survivorId, int max = 8)
        {
            var result = new List<MedicalRecordEntry>();
            if (string.IsNullOrEmpty(survivorId) || max <= 0) return result;

            for (int i = _entries.Count - 1; i >= 0 && result.Count < max; i--)
            {
                if (string.Equals(_entries[i].survivorId, survivorId, StringComparison.Ordinal))
                    result.Add(_entries[i]);
            }
            return result;
        }

        /// <summary>Copy of the retained entries for persistence.</summary>
        public List<MedicalRecordEntry> CaptureState()
        {
            var copy = new List<MedicalRecordEntry>(_entries.Count);
            for (int i = 0; i < _entries.Count; i++)
            {
                var e = _entries[i];
                copy.Add(new MedicalRecordEntry
                {
                    day = e.day,
                    kind = e.kind,
                    survivorId = e.survivorId,
                    detail = e.detail
                });
            }
            return copy;
        }

        /// <summary>
        /// Restores from a save. A save without the record field (older builds)
        /// yields an empty log — never a failure.
        /// </summary>
        public void RestoreState(List<MedicalRecordEntry>? saved)
        {
            _entries.Clear();
            if (saved == null) return;

            for (int i = 0; i < saved.Count; i++)
            {
                var e = saved[i];
                if (e == null || string.IsNullOrEmpty(e.kind)) continue;
                _entries.Add(e);
            }

            // Defend the bound on load in case an edited save exceeds it.
            while (_entries.Count > MaxEntries)
                _entries.RemoveAt(0);
        }

        /// <summary>Clears the log (new campaign).</summary>
        public void Clear() => _entries.Clear();
    }

    /// <summary>Stable event kinds for <see cref="MedicalRecordLog"/>.</summary>
    public static class MedicalRecordKinds
    {
        public const string DiagnosisSuspected = "diagnosis_suspected";
        public const string DiagnosisConfirmed = "diagnosis_confirmed";
        public const string PatientStabilized = "patient_stabilized";
        public const string PatientRecovered = "patient_recovered";
        public const string TreatmentScheduled = "treatment_scheduled";
        public const string TreatmentCompleted = "treatment_completed";
        public const string TreatmentRefused = "treatment_refused";
        public const string ProtocolExecuted = "protocol_executed";
    }
}
