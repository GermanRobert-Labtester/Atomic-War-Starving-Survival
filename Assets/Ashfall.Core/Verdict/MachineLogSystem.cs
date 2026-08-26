using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — machine log state. One record per
    /// facility reading. `read` is the load-bearing field: an entry is only
    /// "true" once a human has read it, and only read entries enroll evidence.
    /// Engine-agnostic, deterministic, save/load safe.
    /// </summary>
    [Serializable]
    public sealed class MachineLogEntry
    {
        public string facilityId = string.Empty;
        public int day = 0;
        public string kind = "operating";   // operating | maintenance | anomaly | count
        public string bodyShort = string.Empty;
        public string evidenceTag = string.Empty;
        public bool read;
    }

    [Serializable]
    public sealed class MachineLogSystemState
    {
        public List<MachineLogEntry> entries = new List<MachineLogEntry>();
        public int lastTapeSpinDay = -1;
        public int logIndex;
        public bool countdownActive;
        public int countdownDaysLeft;
    }

    /// <summary>
    /// The player-facing machine presence. Facilities post readings; the log
    /// presents them; a human choosing to read one enrolls its evidence.
    /// Ordering is by (day, sequence); duplicates suppressed by facility+day+kind.
    /// </summary>
    public sealed class MachineLogSystem
    {
        private readonly MachineLogSystemState _state;

        public MachineLogSystemState State => _state;
        public IReadOnlyList<MachineLogEntry> Entries => _state.entries;

        public event Action<MachineLogEntry> OnLogPosted;
        public event Action<MachineLogEntry> OnEntryRead;
        public event Action OnTapeSpin;

        public MachineLogSystem(MachineLogSystemState? state = null)
        {
            _state = state ?? new MachineLogSystemState();
        }

        /// <summary>Post a facility reading. Returns false if already logged (idempotent).</summary>
        public bool Post(string facilityId, int day, string kind, string bodyShort, string evidenceTag)
        {
            if (string.IsNullOrEmpty(facilityId)) return false;
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e.facilityId == facilityId && e.day == day && e.kind == kind)
                    return false; // duplicate suppression
            }

            var entry = new MachineLogEntry
            {
                facilityId = facilityId,
                day = day,
                kind = kind,
                bodyShort = bodyShort ?? string.Empty,
                evidenceTag = evidenceTag ?? string.Empty,
                read = false
            };
            _state.entries.Add(entry);
            _state.logIndex++;
            OnLogPosted?.Invoke(entry);
            return true;
        }

        /// <summary>Insert a deterministic, seed-dependent garbling marker (corruption).
        /// Corpus is data-driven (verdict_data.json). Falls back to built-ins if none supplied.</summary>
        public bool InsertCorruptionMarker(int day, ISeededRng rng, IReadOnlyList<string>? corpus = null)
        {
            if (rng == null) return false;

            string[] builtIn =
            {
                "[00:03:07] — signal lost mid-verbose.",
                "[unreadable] sector halts. Sector halts.",
                "11111111 — no hand. No hand on the valve.",
                "[tone] [tone] [tone] — the count repeats itself.",
                "the meter read. The meter read. The meter read."
            };

            IReadOnlyList<string> pool = corpus != null && corpus.Count > 0 ? corpus : builtIn;
            int idx = rng.Next(0, pool.Count);
            return Post("corruption", day, "anomaly", pool[idx], string.Empty);
        }

        /// <summary>Mark an entry read; returns its evidence tag, or empty if not found/already read.
        /// Read is one-way by design — the record does not forget.</summary>
        public string ReadEntry(int index)
        {
            if (index < 0 || index >= _state.entries.Count) return string.Empty;
            var e = _state.entries[index];
            if (e.read) return string.Empty;
            e.read = true;
            OnEntryRead?.Invoke(e);
            return e.evidenceTag;
        }

        /// <summary>Rotate the log's presentation (the tape-silo key).</summary>
        public void SpinTape(int day)
        {
            if (_state.lastTapeSpinDay == day) return; // one spin per day
            _state.lastTapeSpinDay = day;
            OnTapeSpin?.Invoke();
        }

        public int UnreadCount()
        {
            int n = 0;
            for (int i = 0; i < _state.entries.Count; i++)
                if (!_state.entries[i].read) n++;
            return n;
        }

        public int ReadCount()
        {
            int n = 0;
            for (int i = 0; i < _state.entries.Count; i++)
                if (_state.entries[i].read) n++;
            return n;
        }

        public MachineLogSystemState CaptureState()
        {
            var copy = new MachineLogSystemState
            {
                lastTapeSpinDay = _state.lastTapeSpinDay,
                logIndex = _state.logIndex,
                countdownActive = _state.countdownActive,
                countdownDaysLeft = _state.countdownDaysLeft
            };
            copy.entries.AddRange(_state.entries);
            return copy;
        }

        public void RestoreState(MachineLogSystemState state)
        {
            if (state == null) return;
            _state.entries.Clear();
            _state.entries.AddRange(state.entries);
            _state.lastTapeSpinDay = state.lastTapeSpinDay;
            _state.logIndex = state.logIndex;
            _state.countdownActive = state.countdownActive;
            _state.countdownDaysLeft = state.countdownDaysLeft;
        }
    }
}
