// System_CultLeash.cs — Cult of the Glow's "leash" mechanic (Expansion II:
// The Weight of Factions). Each shelter has a visit counter. After three
// visits a shelter can be "blessed" (cult protection enabled). Missing the
// weekly communion by 1 week = warned, 2+ = forbidden to leave with
// consequences. The cult does not call itself a cult; the leash is a pact.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Cult Leash system.
    /// One CultLeashEntry per shelter that has interacted with the cult.
    /// </summary>
    [Serializable]
    public class CultLeashState
    {
        public string system_id = "system_cult_leash";
        public List<CultLeashEntry> entries = new List<CultLeashEntry>();
    }

    /// <summary>
    /// Per-shelter cult relationship. children_at_communion stores the
    /// surviving child count at the last weekly communion (used to
    /// compute the answer to the deacon's question "who came?").
    /// </summary>
    [Serializable]
    public class CultLeashEntry
    {
        public string shelter_id;
        public int visit_count;
        public bool blessed;
        public int consecutive_communion_weeks_missed;
        public bool under_protection;
        public string[] children_at_communion;

        public CultLeashEntry() { }

        public CultLeashEntry(string id)
        {
            shelter_id = id;
            visit_count = 0;
            blessed = false;
            consecutive_communion_weeks_missed = 0;
            under_protection = false;
            children_at_communion = new string[0];
        }

        public CultLeashEntry Clone()
        {
            return new CultLeashEntry
            {
                shelter_id = shelter_id,
                visit_count = visit_count,
                blessed = blessed,
                consecutive_communion_weeks_missed = consecutive_communion_weeks_missed,
                under_protection = under_protection,
                children_at_communion = children_at_communion == null
                    ? new string[0]
                    : (string[])children_at_communion.Clone()
            };
        }
    }

    /// <summary>
    /// Outcome enum for AttemptLeave. Permitted = cult has no claim;
    /// Warned = cult formally objects and demands a future communion;
    /// ForbiddenWithConsequence = leaving now triggers the cult's
    /// "consequence" (a narrative / mechanical fallout).
    /// </summary>
    public enum CultLeaveOutcome
    {
        Permitted = 0,
        Warned = 1,
        ForbiddenWithConsequence = 2
    }

    /// <summary>
    /// Cult of the Glow "leash" — a soft-binding that converts to a
    /// hard-binding after the third visit. No hard references to other
    /// systems; use the public events.
    /// </summary>
    public class System_CultLeash
    {
        // Ids (snake_case)
        public const string CultLeashSystemId = "system_cult_leash";
        public const string SlotId = "cult_leash";

        // Lore rules
        public const int VisitsForBlessing = 3;
        public const int CommunionGraceWeeks = 1;

        // Events
        public event Action<string, int> OnVisitRecorded;
        public event Action<string> OnBlessed;
        public event Action<string, int> OnCommunionMissed;
        public event Action<string> OnLeaveAttempted;

        private CultLeashState _state = new CultLeashState();

        public IReadOnlyList<CultLeashEntry> Entries => _state.entries;

        public int EntryCount => _state.entries.Count;

        public CultLeashEntry GetEntry(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId)) return null;
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e != null && e.shelter_id == shelterId) return e;
            }
            return null;
        }

        // Record a single cult emissary visit. Increments visit count
        // and raises OnVisitRecorded (carrying the new total).
        // day is a logging context only (not used by this method).
        public void RecordVisit(string shelterId, int day = 0)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            entry.visit_count += 1;
            OnVisitRecorded?.Invoke(shelterId, entry.visit_count);
        }

        // Attempt to "bless" a shelter. Requires visit_count >= 3.
        // Returns true on success; flips blessed + under_protection.
        public bool AttemptBlessing(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId)) return false;
            var entry = GetOrCreate(shelterId);
            if (entry.visit_count < VisitsForBlessing) return false;
            if (entry.blessed) return false;

            entry.blessed = true;
            entry.under_protection = true;
            entry.consecutive_communion_weeks_missed = 0;
            OnBlessed?.Invoke(shelterId);
            return true;
        }

        // Record a communion attendance. The child count is the headcount
        // at the ritual. Resets the miss streak.
        public void RecordCommunionAttendance(string shelterId, int weekIndex, int childCount)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            entry.consecutive_communion_weeks_missed = 0;
            entry.children_at_communion = new string[childCount];
            for (int i = 0; i < childCount; i++)
            {
                entry.children_at_communion[i] = "child_" + i;
            }
        }

        // Record a missed communion (the shelter failed to show this week).
        // 1 miss = warned, 2+ = forbidden to leave.
        // weekIndex is a logging/tracking context only (not used by this method).
        public void RecordMissedCommunion(string shelterId, int weekIndex = 0)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            entry.consecutive_communion_weeks_missed += 1;
            OnCommunionMissed?.Invoke(shelterId, entry.consecutive_communion_weeks_missed);
        }

        // Attempt to leave the cult. Returns the outcome enum.
        // - Permitted: never blessed, OR blessed with no missed weeks yet.
        // - Warned:    blessed and missed exactly the grace week (1).
        // - ForbiddenWithConsequence: blessed and missed 2+ weeks.
        public CultLeaveOutcome AttemptLeave(string shelterId)
        {
            CultLeaveOutcome outcome;
            if (string.IsNullOrEmpty(shelterId))
            {
                outcome = CultLeaveOutcome.Permitted;
                OnLeaveAttempted?.Invoke(string.Empty);
                return outcome;
            }
            var entry = GetOrCreate(shelterId);
            if (!entry.blessed || entry.consecutive_communion_weeks_missed <= 0)
            {
                outcome = CultLeaveOutcome.Permitted;
            }
            else if (entry.consecutive_communion_weeks_missed <= CommunionGraceWeeks)
            {
                outcome = CultLeaveOutcome.Warned;
            }
            else
            {
                outcome = CultLeaveOutcome.ForbiddenWithConsequence;
            }
            OnLeaveAttempted?.Invoke(shelterId);
            return outcome;
        }

        public bool IsBlessed(string shelterId)
        {
            var e = GetEntry(shelterId);
            return e != null && e.blessed;
        }

        public bool IsUnderProtection(string shelterId)
        {
            var e = GetEntry(shelterId);
            return e != null && e.under_protection;
        }

        public int GetVisitCount(string shelterId)
        {
            var e = GetEntry(shelterId);
            return e == null ? 0 : e.visit_count;
        }

        public int GetConsecutiveMissedWeeks(string shelterId)
        {
            var e = GetEntry(shelterId);
            return e == null ? 0 : e.consecutive_communion_weeks_missed;
        }

        // Save/Load (deep copy)
        public CultLeashState CaptureState()
        {
            var copy = new CultLeashState
            {
                system_id = "system_cult_leash",
                entries = new List<CultLeashEntry>()
            };
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e == null || string.IsNullOrEmpty(e.shelter_id)) continue;
                copy.entries.Add(e.Clone());
            }
            return copy;
        }

        public void RestoreState(CultLeashState saved)
        {
            if (saved == null)
            {
                _state = new CultLeashState();
                return;
            }
            _state = new CultLeashState
            {
                system_id = "system_cult_leash",
                entries = new List<CultLeashEntry>()
            };
            for (int i = 0; i < saved.entries.Count; i++)
            {
                var e = saved.entries[i];
                if (e == null || string.IsNullOrEmpty(e.shelter_id)) continue;
                _state.entries.Add(e.Clone());
            }
        }

        private CultLeashEntry GetOrCreate(string shelterId)
        {
            var existing = GetEntry(shelterId);
            if (existing != null) return existing;
            var fresh = new CultLeashEntry(shelterId);
            _state.entries.Add(fresh);
            return fresh;
        }
    }
}
