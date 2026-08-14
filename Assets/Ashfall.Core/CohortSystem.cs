using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public class CohortChild
    {
        public string survivorId;
        public List<string> parentIds = new List<string>();
        public string guessBand;       // "low" | "medium" | "high"
        public string trueBand;        // corrected later, empty until known
        public int birthDay;
        public bool baselineCorrected;
        public string moralityMemory;  // the story told, not the dose
    }

    [Serializable]
    public class CohortSystemState
    {
        public string systemId = CohortSystem.SystemId;
        public List<CohortChild> children = new List<CohortChild>();
    }

    /// <summary>
    /// ASHFALL: THE DOSE — the second generation's baseline as a contested,
    /// intractable number. The baseline is inherited as a guess the player
    /// chooses; a later dosimeter read corrects it silently.
    /// </summary>
    public class CohortSystem
    {
        public const string SystemId = "cohort_system";

        private readonly CohortSystemState _state = new CohortSystemState();
        private readonly Dictionary<string, CohortChild> _children = new Dictionary<string, CohortChild>();

        public event Action<string, string> OnChildBooked;       // childId, guessBand
        public event Action<string, string> OnBaselineCorrected; // childId, trueBand
        public event Action<CohortSystemState> OnStateChanged;

        public CohortSystemState State => _state;
        public IReadOnlyList<CohortChild> Children => _state.children;

        /// <summary>Book a child with a guess band ("low"/"medium"/"high"). Never rewrite.</summary>
        public bool BookChild(string childId, IReadOnlyList<string> parentIds, string guessBand, int birthDay, string moralityMemory = null)
        {
            if (string.IsNullOrEmpty(childId) || string.IsNullOrEmpty(guessBand)) return false;
            if (_children.ContainsKey(childId)) return false; // booked twice is refused
            if (guessBand != "low" && guessBand != "medium" && guessBand != "high") return false;

            var child = new CohortChild
            {
                survivorId = childId,
                guessBand = guessBand,
                birthDay = birthDay,
                moralityMemory = moralityMemory ?? string.Empty,
                parentIds = new List<string>()
            };
            if (parentIds != null)
                foreach (var p in parentIds)
                    if (!string.IsNullOrEmpty(p)) child.parentIds.Add(p);

            _children[childId] = child;
            _state.children.Add(child);
            OnChildBooked?.Invoke(childId, guessBand);
            RaiseChanged();
            return true;
        }

        /// <summary>Correct the baseline with a true band. Does not auto-post to the ledger.</summary>
        public bool CorrectBaseline(string childId, string trueBand)
        {
            if (!_children.TryGetValue(childId, out var child)) return false;
            if (string.IsNullOrEmpty(trueBand)) return false;
            child.trueBand = trueBand;
            child.baselineCorrected = true;
            OnBaselineCorrected?.Invoke(childId, trueBand);
            RaiseChanged();
            return true;
        }

        public CohortChild GetChild(string childId) =>
            _children.TryGetValue(childId, out var c) ? c : null;

        public CohortSystemState CaptureState()
        {
            _state.children.Clear();
            foreach (var kv in _children)
            {
                var c = kv.Value;
                _state.children.Add(new CohortChild
                {
                    survivorId = c.survivorId,
                    guessBand = c.guessBand,
                    trueBand = c.trueBand,
                    birthDay = c.birthDay,
                    baselineCorrected = c.baselineCorrected,
                    moralityMemory = c.moralityMemory,
                    parentIds = new List<string>(c.parentIds)
                });
            }
            return _state;
        }

        public void RestoreState(CohortSystemState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _children.Clear();
            _state.children.Clear();
            if (saved.children != null)
            {
                foreach (var c in saved.children)
                {
                    if (c == null || string.IsNullOrEmpty(c.survivorId)) continue;
                    var copy = new CohortChild
                    {
                        survivorId = c.survivorId,
                        guessBand = c.guessBand,
                        trueBand = c.trueBand,
                        birthDay = c.birthDay,
                        baselineCorrected = c.baselineCorrected,
                        moralityMemory = c.moralityMemory,
                        parentIds = c.parentIds != null ? new List<string>(c.parentIds) : new List<string>()
                    };
                    _children[c.survivorId] = copy;
                    _state.children.Add(copy);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}