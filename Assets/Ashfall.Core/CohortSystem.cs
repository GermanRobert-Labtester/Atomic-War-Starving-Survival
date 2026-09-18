// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

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
        // Plan 12A — maturation flag. Plan 12A is the plan that first
        // promotes a child into a working survivor; the flag is part of
        // the per-child DTO so save/load round-trips it cleanly without
        // a parallel registry. Default false; acquired only via
        // TryMaturation (one-way, idempotent within the cohort).
        public bool isMatured;
        public int maturationDay;
        // Plan 19B — child mortality and loss tracking. Round-trips cleanly
        // through DoseLedgerSave without a parallel registry.
        public bool isDeceased;
        public int deathDay = -1;
        public string deathCause = string.Empty;
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
        public event Action<string, int> OnMaturation;           // childId, day  -- Plan 12A
        public event Action<string, int, string>? OnChildLost;   // childId, day, cause -- Plan 19B
        public event Action<string, int>? OnChildAged;           // childId, ageDays -- Plan 19B
        public event Action<CohortSystemState> OnStateChanged;

        public CohortTuning Tuning { get; set; } = CohortTuning.Default;

        public CohortSystemState State => _state;
        public IReadOnlyList<CohortChild> Children => _state.children;

        public int SurvivingChildrenCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _state.children.Count; i++)
                {
                    if (!_state.children[i].isDeceased) count++;
                }
                return count;
            }
        }

        public bool AnyChildrenSurvived => SurvivingChildrenCount > 0;

        /// <summary>Book a child with a guess band ("low"/"medium"/"high"). Never rewrite.</summary>
        public bool BookChild(string childId, IReadOnlyList<string> parentIds, string guessBand, int birthDay, string? moralityMemory = null)
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

        public CohortChild? GetChild(string childId) =>
            _children.TryGetValue(childId, out var c) ? c : null;

        /// <summary>
        /// Plan 12A: mark a CohortChild as matured at <paramref name="day"/>
        /// (one-way). The child is not removed from the cohort roster —
        /// the registry keeps a permanent record — but the boolean makes
        /// the maturation event queryable for save/load and for downstream
        /// quest/event gating (e.g. "first solo surface trip"). Returns
        /// false when the id is unknown, the child has already matured,
        /// or the day is invalid. Pure event publisher; no roster mutation.
        /// </summary>
        public bool TryMaturation(string childId, int day)
        {
            if (string.IsNullOrEmpty(childId) || day <= 0) return false;
            if (!_children.TryGetValue(childId, out var child)) return false;
            if (child.isMatured || child.isDeceased) return false;

            child.isMatured = true;
            child.maturationDay = day;
            OnMaturation?.Invoke(childId, day);
            RaiseChanged();
            return true;
        }

        public const int DefaultMaturationAgeDays = 365;

        /// <summary>
        /// Plan 41 / C1[12]: Calendar-driven maturation check across living cohort children.
        /// Evaluates all living, unmatured children and transitions those whose age (currentDay - birthDay)
        /// is >= maturationAgeDays (default 365 days) into mature status via TryMaturation.
        /// Returns the count of children matured on this check.
        /// </summary>
        public int CheckCohortMaturation(int currentDay, int maturationAgeDays = DefaultMaturationAgeDays)
        {
            if (currentDay <= 0 || maturationAgeDays <= 0) return 0;
            int count = 0;
            for (int i = 0; i < _state.children.Count; i++)
            {
                var child = _state.children[i];
                if (child == null || child.isMatured || child.isDeceased) continue;
                int age = currentDay - child.birthDay;
                if (age >= maturationAgeDays)
                {
                    if (TryMaturation(child.survivorId, currentDay))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Plan 19B.7 / 19B.10: Record a cohort child death or loss.
        /// </summary>
        public bool MarkChildLost(string childId, int day, string? cause = null)
        {
            if (string.IsNullOrEmpty(childId) || day <= 0) return false;
            if (!_children.TryGetValue(childId, out var child)) return false;
            if (child.isDeceased) return false;

            child.isDeceased = true;
            child.deathDay = day;
            child.deathCause = string.IsNullOrEmpty(cause) ? "lost" : cause;
            OnChildLost?.Invoke(childId, day, child.deathCause);
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Plan 19B.5: Cohort maturation determines when a child becomes work/duty eligible.
        /// Unmatured living children cannot be assigned to duty roster.
        /// Returns true for survivors not tracked in the cohort (standard adults).
        /// </summary>
        public bool IsWorkEligible(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_children.TryGetValue(survivorId, out var child))
                return true; // Not a cohort child; standard adult dweller
            return !child.isDeceased && child.isMatured;
        }

        /// <summary>
        /// Plan 19B.4: Determines if a child has reached schooling/apprenticeship age.
        /// Under-age (e.g. infants) are not eligible; matured dwellers graduate.
        /// </summary>
        public bool IsSchoolEligible(string childId, int currentDay)
        {
            if (string.IsNullOrEmpty(childId) || currentDay <= 0) return false;
            if (!_children.TryGetValue(childId, out var child)) return false;
            if (child.isDeceased || child.isMatured) return false;
            int age = Math.Max(0, currentDay - child.birthDay);
            int minAge = Tuning?.schooling_age_days ?? 30;
            return age >= minAge;
        }

        /// <summary>
        /// Plan 19B.2 / 19B.3: Calculates total daily child food units required deterministically
        /// based on surviving cohort population and the child ration fraction in data.
        /// </summary>
        public int CalculateChildFoodUnits(StartingLevel.RationPolicy policy = StartingLevel.RationPolicy.Standard)
        {
            int living = SurvivingChildrenCount;
            if (living <= 0) return 0;
            float basePerAdult = policy == StartingLevel.RationPolicy.Half ? 2f : 3f;
            float fraction = Tuning?.child_ration_fraction ?? 0.5f;
            float total = living * basePerAdult * fraction;
            return (int)Math.Ceiling(total);
        }

        public CohortSystemState CaptureState()
        {
            // Fresh copy, ordinal-ordered (aliasing + cross-host determinism).
            var copy = new CohortSystemState { systemId = _state.systemId };
            var keys = new List<string>(_children.Count);
            foreach (var kv in _children) keys.Add(kv.Key);
            keys.Sort(string.CompareOrdinal);
            for (int i = 0; i < keys.Count; i++)
            {
                var c = _children[keys[i]];
                copy.children.Add(new CohortChild
                {
                    survivorId = c.survivorId,
                    guessBand = c.guessBand,
                    trueBand = c.trueBand,
                    birthDay = c.birthDay,
                    baselineCorrected = c.baselineCorrected,
                    moralityMemory = c.moralityMemory,
                    parentIds = new List<string>(c.parentIds),
                    isMatured = c.isMatured,
                    maturationDay = c.maturationDay,
                    isDeceased = c.isDeceased,
                    deathDay = c.deathDay,
                    deathCause = c.deathCause ?? string.Empty
                });
            }
            return copy;
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
                        parentIds = c.parentIds != null ? new List<string>(c.parentIds) : new List<string>(),
                        isMatured = c.isMatured,
                        maturationDay = c.maturationDay,
                        isDeceased = c.isDeceased,
                        deathDay = c.deathDay,
                        deathCause = c.deathCause ?? string.Empty
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
