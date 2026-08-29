using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public class VolunteerEntry
    {
        public string survivorId;
        public string task;
        public int acceptedDay;
        public int completedDay = -1;
        public float doseIncurred;
        public string reasonText;
        public bool completed;
    }

    [Serializable]
    public class VoluntaryRegisterSystemState
    {
        public string systemId = VoluntaryRegisterSystem.SystemId;
        public List<VolunteerEntry> entries = new List<VolunteerEntry>();
    }

    /// <summary>
    /// ASHFALL: THE DOSE — survivors who sign away the front of the days for
    /// high-dose surface work. Not a penalty; a signature. On completion the
    /// dose is banked (host composes this with DoseLedgerSystem).
    /// </summary>
    public class VoluntaryRegisterSystem
    {
        public const string SystemId = "voluntary_register_system";

        private readonly VoluntaryRegisterSystemState _state = new VoluntaryRegisterSystemState();
        private readonly Dictionary<string, VolunteerEntry> _entries = new Dictionary<string, VolunteerEntry>();

        public event Action<string, string> OnVolunteered;        // survivorId, task
        public event Action<string, float> OnVolunteerCompleted;  // survivorId, dose
        public event Action<VoluntaryRegisterSystemState> OnStateChanged;

        public VoluntaryRegisterSystemState State => _state;
        public IReadOnlyList<VolunteerEntry> Entries => _state.entries;

        /// <summary>Sign a survivor up for a task. A new signature for a new task.</summary>
        public bool Volunteer(string survivorId, string task, int day, string? reasonText = null)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(task)) return false;
            // A new signature is for a new task: double-signing the same task
            // would diverge the live list from the saved list (silent data loss).
            string key = survivorId + "|" + task;
            if (_entries.ContainsKey(key)) return false;
            var entry = new VolunteerEntry
            {
                survivorId = survivorId,
                task = task,
                acceptedDay = day,
                reasonText = reasonText ?? string.Empty
            };
            _entries[key] = entry;
            _state.entries.Add(entry);
            OnVolunteered?.Invoke(survivorId, task);
            RaiseChanged();
            return true;
        }

        /// <summary>Mark a volunteer task done and bank the incurred dose.</summary>
        public bool CompleteVolunteer(string survivorId, string task, float doseIncurred, int day)
        {
            string key = survivorId + "|" + task;
            if (!_entries.TryGetValue(key, out var entry)) return false;
            if (entry.completed) return false;
            entry.completed = true;
            entry.completedDay = day;
            entry.doseIncurred = doseIncurred;
            OnVolunteerCompleted?.Invoke(survivorId, doseIncurred);
            RaiseChanged();
            return true;
        }

        public VolunteerEntry? GetEntry(string survivorId, string? task = null)
        {
            if (string.IsNullOrEmpty(task))
            {
                foreach (var kv in _entries)
                    if (kv.Value.survivorId == survivorId && !kv.Value.completed) return kv.Value;
                return null;
            }
            return _entries.TryGetValue(survivorId + "|" + task, out var e) ? e : null;
        }

        public VoluntaryRegisterSystemState CaptureState()
        {
            // Fresh copy with deep-copied entries, ordinal-ordered by
            // survivor+task (aliasing + cross-host determinism).
            var copy = new VoluntaryRegisterSystemState { systemId = _state.systemId };
            var keys = new List<string>(_entries.Count);
            foreach (var kv in _entries) keys.Add(kv.Key);
            keys.Sort(string.CompareOrdinal);
            for (int i = 0; i < keys.Count; i++)
            {
                var e = _entries[keys[i]];
                copy.entries.Add(new VolunteerEntry
                {
                    survivorId = e.survivorId,
                    task = e.task,
                    acceptedDay = e.acceptedDay,
                    completedDay = e.completedDay,
                    doseIncurred = e.doseIncurred,
                    reasonText = e.reasonText,
                    completed = e.completed
                });
            }
            return copy;
        }

        public void RestoreState(VoluntaryRegisterSystemState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _entries.Clear();
            _state.entries.Clear();
            if (saved.entries != null)
            {
                foreach (var e in saved.entries)
                {
                    if (e == null || string.IsNullOrEmpty(e.survivorId)) continue;
                    var copy = new VolunteerEntry
                    {
                        survivorId = e.survivorId,
                        task = e.task,
                        acceptedDay = e.acceptedDay,
                        completedDay = e.completedDay,
                        doseIncurred = e.doseIncurred,
                        reasonText = e.reasonText,
                        completed = e.completed
                    };
                    _entries[copy.survivorId + "|" + copy.task] = copy;
                    _state.entries.Add(copy);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
