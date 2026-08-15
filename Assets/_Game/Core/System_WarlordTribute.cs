// System_WarlordTribute.cs — Scavenger Warlord tribute ledger (Expansion II:
// The Weight of Factions). Each shelter has a tribute amount. Short payments
// (< 90% of required) escalate the required amount by 1.5x, capped at 8x the
// initial. The Warlord Code is a public string-constant set so the world
// simulator and radio library can quote the same rules.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Warlord Tribute system.
    /// One WarlordTributeEntry per shelter on the warlord's books.
    /// </summary>
    [Serializable]
    public class WarlordTributeState
    {
        public string system_id = "system_warlord_tribute";
        public List<WarlordTributeEntry> entries = new List<WarlordTributeEntry>();
    }

    /// <summary>
    /// Per-shelter tribute record. current_tribute_amount is the weekly ask
    /// (the figure the collector will quote). base_amount is the un-escalated
    /// floor used to compute the MaxTributeMultiplier cap. is_burned is
    /// the terminal state once tribute stops entirely.
    /// </summary>
    [Serializable]
    public class WarlordTributeEntry
    {
        public string shelter_id;
        public float current_tribute_amount;
        public int consecutive_short_weeks;
        public int total_weeks_paid;
        public string last_collector_leader;
        public bool leave_one_thing_fulfilled;
        public float base_amount;
        public bool is_burned;

        public WarlordTributeEntry() { }

        public WarlordTributeEntry(string id, float baseAmount)
        {
            shelter_id = id;
            base_amount = baseAmount;
            current_tribute_amount = baseAmount;
            consecutive_short_weeks = 0;
            total_weeks_paid = 0;
            last_collector_leader = string.Empty;
            leave_one_thing_fulfilled = false;
            is_burned = false;
        }

        public WarlordTributeEntry Clone()
        {
            return new WarlordTributeEntry
            {
                shelter_id = shelter_id,
                current_tribute_amount = current_tribute_amount,
                consecutive_short_weeks = consecutive_short_weeks,
                total_weeks_paid = total_weeks_paid,
                last_collector_leader = last_collector_leader,
                leave_one_thing_fulfilled = leave_one_thing_fulfilled,
                base_amount = base_amount,
                is_burned = is_burned
            };
        }
    }

    /// <summary>
    /// Scavenger Warlord tribute book. Tracks what each shelter owes,
    /// whether the short-payment streak is escalating the demand, and
    /// whether the "leave one thing" ritual has been fulfilled this cycle.
    /// No hard references to other systems; use the public events.
    /// </summary>
    public class System_WarlordTribute
    {
        // Ids (snake_case)
        public const string WarlordTributeSystemId = "system_warlord_tribute";
        public const string SlotId = "warlord_tribute";

        // Lore rules
        public const float TributeEscalationFactor = 1.5f;
        public const float ShortThreshold = 0.9f;
        public const float MaxTributeMultiplier = 8f;

        // Warlord Code (canonical, public — quoted in radio intercepts)
        public const string CodeNoKillIfPaying = "code_no_kill_if_paying";
        public const string CodeNoBurnShelters = "code_no_burn_shelters";
        public const string CodeNoTakeChildren = "code_no_take_children";
        public const string CodeKillQuicklyIfResisted = "code_kill_quickly_if_resisted";
        public const string CodeAlwaysLeaveOneThing = "code_always_leave_one_thing";

        // Events
        public event Action<string, float> OnTributeSet;
        public event Action<string, int> OnShortPaymentEscalated;
        public event Action<string> OnLeaveOneThingGiven;
        public event Action<string> OnShelterBurned;

        private WarlordTributeState _state = new WarlordTributeState();

        public IReadOnlyList<WarlordTributeEntry> Entries => _state.entries;

        public int EntryCount => _state.entries.Count;

        public WarlordTributeEntry GetEntry(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId)) return null;
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e != null && e.shelter_id == shelterId) return e;
            }
            return null;
        }

        // Set the initial tribute for a shelter; if the entry exists,
        // resets the required amount to the new base.
        public void SetInitialTribute(string shelterId, float baseAmount)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            if (baseAmount < 0f) baseAmount = 0f;

            var entry = GetEntry(shelterId);
            if (entry == null)
            {
                entry = new WarlordTributeEntry(shelterId, baseAmount);
                _state.entries.Add(entry);
            }
            else
            {
                entry.base_amount = baseAmount;
                entry.current_tribute_amount = baseAmount;
                entry.consecutive_short_weeks = 0;
            }
            OnTributeSet?.Invoke(shelterId, entry.current_tribute_amount);
        }

        // Record a full payment. Resets the short-payment streak and
        // resets the required amount to the base.
        public void PayFull(string shelterId, int weekIndex)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            entry.consecutive_short_weeks = 0;
            entry.total_weeks_paid += 1;
            if (entry.current_tribute_amount != entry.base_amount)
            {
                entry.current_tribute_amount = entry.base_amount;
                OnTributeSet?.Invoke(shelterId, entry.current_tribute_amount);
            }
        }

        // Record a short payment. If actualPaid < 90% of required, multiply
        // the required by 1.5 (cap 8x base). Fires OnShortPaymentEscalated
        // when the cap moves.
        public void PayShort(string shelterId, float actualPaid, int weekIndex)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            float required = entry.current_tribute_amount;
            if (required <= 0f) return;

            float ratio = actualPaid / required;
            entry.total_weeks_paid += 1;

            if (ratio < ShortThreshold)
            {
                entry.consecutive_short_weeks += 1;
                float escalated = entry.current_tribute_amount * TributeEscalationFactor;
                float cap = entry.base_amount * MaxTributeMultiplier;
                if (escalated > cap) escalated = cap;
                if (escalated != entry.current_tribute_amount)
                {
                    entry.current_tribute_amount = escalated;
                    OnTributeSet?.Invoke(shelterId, entry.current_tribute_amount);
                }
                OnShortPaymentEscalated?.Invoke(shelterId, entry.consecutive_short_weeks);
            }
            else
            {
                // Borderline but not "short" — counts as paid, resets streak.
                entry.consecutive_short_weeks = 0;
                if (entry.current_tribute_amount != entry.base_amount)
                {
                    entry.current_tribute_amount = entry.base_amount;
                    OnTributeSet?.Invoke(shelterId, entry.current_tribute_amount);
                }
            }
        }

        // Mark the Warlord Code "leave one thing" ritual as fulfilled
        // for this shelter (fires OnLeaveOneThingGiven).
        public void FulfillLeaveOneThing(string shelterId, string itemId)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            entry.leave_one_thing_fulfilled = true;
            OnLeaveOneThingGiven?.Invoke(shelterId);
        }

        // Reset the leave-one-thing flag (next cycle starts).
        public void ClearLeaveOneThing(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            entry.leave_one_thing_fulfilled = false;
        }

        // Mark the shelter as burned (tribute stopped entirely).
        // Fires OnShelterBurned. The entry stays in the ledger as
        // historical record.
        public void MarkShelterBurned(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            if (entry.is_burned) return;
            entry.is_burned = true;
            OnShelterBurned?.Invoke(shelterId);
        }

        // Required tribute this week (clamped at base * MaxTributeMultiplier).
        public float GetRequiredTribute(string shelterId)
        {
            var e = GetEntry(shelterId);
            if (e == null) return 0f;
            if (e.is_burned) return 0f;
            float cap = e.base_amount * MaxTributeMultiplier;
            if (e.current_tribute_amount > cap) return cap;
            return e.current_tribute_amount;
        }

        public bool IsShelterBurned(string shelterId)
        {
            var e = GetEntry(shelterId);
            return e != null && e.is_burned;
        }

        public void SetLastCollectorLeader(string shelterId, string leaderId)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = GetOrCreate(shelterId);
            entry.last_collector_leader = leaderId ?? string.Empty;
        }

        // Save/Load (deep copy)
        public WarlordTributeState CaptureState()
        {
            var copy = new WarlordTributeState
            {
                system_id = "system_warlord_tribute",
                entries = new List<WarlordTributeEntry>()
            };
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e == null || string.IsNullOrEmpty(e.shelter_id)) continue;
                copy.entries.Add(e.Clone());
            }
            return copy;
        }

        public void RestoreState(WarlordTributeState saved)
        {
            if (saved == null)
            {
                _state = new WarlordTributeState();
                return;
            }
            _state = new WarlordTributeState
            {
                system_id = "system_warlord_tribute",
                entries = new List<WarlordTributeEntry>()
            };
            for (int i = 0; i < saved.entries.Count; i++)
            {
                var e = saved.entries[i];
                if (e == null || string.IsNullOrEmpty(e.shelter_id)) continue;
                _state.entries.Add(e.Clone());
            }
        }

        private WarlordTributeEntry GetOrCreate(string shelterId)
        {
            var existing = GetEntry(shelterId);
            if (existing != null) return existing;
            var fresh = new WarlordTributeEntry(shelterId, 0f);
            _state.entries.Add(fresh);
            return fresh;
        }
    }
}
