// SPDX-License-Identifier: MIT
// Task #133 — Diagnosis knowledge: what the shelter knows, not what is true.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>Serialized knowledge record for one affliction episode.</summary>
    [Serializable]
    public sealed class DiagnosisRecord
    {
        public string episodeId = string.Empty;
        /// <summary>"unknown" | "suspected" | "confirmed" | "ruled_out" — snake_case on the wire.</summary>
        public string status = "unknown";
        public int statusDay;
        public int confirmedDay = -1;
        public string detail = string.Empty;

        public DiagnosisRecord Clone() => new DiagnosisRecord
        {
            episodeId = episodeId,
            status = status,
            statusDay = statusDay,
            confirmedDay = confirmedDay,
            detail = detail
        };
    }

    /// <summary>Versioned save snapshot of all diagnosis knowledge.</summary>
    [Serializable]
    public sealed class DiagnosisKnowledgeSaveState
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public List<DiagnosisRecord> records = new List<DiagnosisRecord>();
    }

    /// <summary>
    /// The shelter's clinical knowledge ledger: per (survivor, affliction episode)
    /// what is Unknown / Suspected / Confirmed / RuledOut. This is deliberately
    /// NOT the condition itself — a survivor can be dying of respiratory
    /// degeneration the shelter has not yet confirmed.
    ///
    /// <para>Ordering is ordinal by episode id; iteration order is deterministic.
    /// Values are validated at the boundary; unknown status strings are rejected
    /// on restore rather than silently coerced.</para>
    /// </summary>
    public sealed class DiagnosisKnowledgeStore
    {
        public const string StatusUnknown = "unknown";
        public const string StatusSuspected = "suspected";
        public const string StatusConfirmed = "confirmed";
        public const string StatusRuledOut = "ruled_out";

        private readonly Dictionary<string, DiagnosisRecord> _records =
            new Dictionary<string, DiagnosisRecord>(StringComparer.Ordinal);

        public event Action? OnStateChanged;

        public static string StatusToString(DiagnosisStatus status) => status switch
        {
            DiagnosisStatus.Suspected => StatusSuspected,
            DiagnosisStatus.Confirmed => StatusConfirmed,
            DiagnosisStatus.RuledOut => StatusRuledOut,
            _ => StatusUnknown
        };

        public static bool TryParseStatus(string? value, out DiagnosisStatus status)
        {
            switch (value)
            {
                case StatusUnknown: status = DiagnosisStatus.Unknown; return true;
                case StatusSuspected: status = DiagnosisStatus.Suspected; return true;
                case StatusConfirmed: status = DiagnosisStatus.Confirmed; return true;
                case StatusRuledOut: status = DiagnosisStatus.RuledOut; return true;
                default: status = DiagnosisStatus.Unknown; return false;
            }
        }

        public DiagnosisStatus GetStatus(AfflictionEpisodeId episode)
        {
            return _records.TryGetValue(episode.Value, out var r)
                && TryParseStatus(r.status, out var s) ? s : DiagnosisStatus.Unknown;
        }

        public DiagnosisRecord? GetRecord(AfflictionEpisodeId episode)
        {
            return _records.TryGetValue(episode.Value, out var r) ? r.Clone() : null;
        }

        /// <summary>Every recorded episode, in deterministic ordinal order.</summary>
        public IReadOnlyList<DiagnosisRecord> Records
        {
            get
            {
                var keys = new List<string>(_records.Keys);
                keys.Sort(string.CompareOrdinal);
                var list = new List<DiagnosisRecord>(keys.Count);
                foreach (var k in keys) list.Add(_records[k].Clone());
                return list;
            }
        }

        /// <summary>
        /// Raise the status one rung (Unknown→Suspected→Confirmed). Never lowers;
        /// lowering goes through <see cref="RuleOut"/> or an explicit set. Returns
        /// the (possibly unchanged) resulting status.
        /// </summary>
        public DiagnosisStatus Promote(AfflictionEpisodeId episode, int day, string? detail = null)
        {
            var current = GetStatus(episode);
            DiagnosisStatus target;
            if (current == DiagnosisStatus.Confirmed || current == DiagnosisStatus.RuledOut)
                return current; // terminal knowledge states
            target = current == DiagnosisStatus.Unknown ? DiagnosisStatus.Suspected : DiagnosisStatus.Confirmed;
            SetStatus(episode, target, day, detail);
            return target;
        }

        /// <summary>
        /// Confirm directly (explicit diagnose operation or legacy migration).
        /// Returns true when the state changed.
        /// </summary>
        public bool Confirm(AfflictionEpisodeId episode, int day, string? detail = null)
        {
            if (GetStatus(episode) == DiagnosisStatus.Confirmed) return false;
            SetStatus(episode, DiagnosisStatus.Confirmed, day, detail);
            var r = _records[episode.Value];
            r.confirmedDay = day;
            return true;
        }

        public bool RuleOut(AfflictionEpisodeId episode, int day, string? detail = null)
        {
            if (GetStatus(episode) == DiagnosisStatus.RuledOut) return false;
            SetStatus(episode, DiagnosisStatus.RuledOut, day, detail);
            return true;
        }

        public void SetStatus(AfflictionEpisodeId episode, DiagnosisStatus status, int day, string? detail = null)
        {
            if (!_records.TryGetValue(episode.Value, out var r))
            {
                r = new DiagnosisRecord { episodeId = episode.Value };
                _records[episode.Value] = r;
            }
            r.status = StatusToString(status);
            r.statusDay = day;
            if (!string.IsNullOrEmpty(detail)) r.detail = detail!;
            RaiseChanged();
        }

        /// <summary>Remove records for an episode (e.g. integrity repair of unknown references).</summary>
        public bool Remove(AfflictionEpisodeId episode)
        {
            bool removed = _records.Remove(episode.Value);
            if (removed) RaiseChanged();
            return removed;
        }

        public DiagnosisKnowledgeSaveState CaptureState()
        {
            var copy = new DiagnosisKnowledgeSaveState();
            var keys = new List<string>(_records.Keys);
            keys.Sort(string.CompareOrdinal);
            foreach (var k in keys) copy.records.Add(_records[k].Clone());
            return copy;
        }

        public void RestoreState(DiagnosisKnowledgeSaveState? saved)
        {
            _records.Clear();
            if (saved == null) { RaiseChanged(); return; }
            for (int i = 0; i < saved.records.Count; i++)
            {
                var r = saved.records[i];
                if (r == null || string.IsNullOrEmpty(r.episodeId)) continue;
                if (!AfflictionEpisodeId.IsValid(r.episodeId)) continue; // invalid refs are dropped, not invented
                if (!TryParseStatus(r.status, out var status)) continue;
                var clone = r.Clone();
                clone.status = StatusToString(status);
                _records[clone.episodeId] = clone;
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
