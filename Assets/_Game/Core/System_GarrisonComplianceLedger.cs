// System_GarrisonComplianceLedger.cs — Central Garrison's compliance ledger for
// requisition settlements (Expansion II: The Weight of Factions). Each shelter
// has a slot in the ledger. Three non-payments flips the slot to "non_compliant";
// patrol route weight drops to 0 for that shelter. The shelter can only return
// to compliant standing by completing ReinstatedWeeks consecutive compliant
// weeks. Lore-true phrasing: "the absence of protection is the bullet".
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Garrison Compliance Ledger system.
    /// One LedgerEntry per shelter currently on the books.
    /// </summary>
    [Serializable]
    public class GarrisonComplianceLedgerState
    {
        public string system_id = "system_garrison_compliance_ledger";
        public List<LedgerEntry> entries = new List<LedgerEntry>();
    }

    /// <summary>
    /// Per-shelter compliance record. compliance_strikes counts unresolved
    /// non-payments; at StrikeThreshold the slot flips to non_compliant_flag.
    /// The ledger also tracks the latest requisition id and the most recent
    /// patrol-route membership (read by the world simulator's route picker).
    /// </summary>
    [Serializable]
    public class LedgerEntry
    {
        public string shelter_id;
        public int ledger_position;
        public int compliance_strikes;
        public int consecutive_compliant_weeks;
        public string last_requisition_id;
        public bool is_on_patrol_route;
        public bool non_compliant_flag;

        public LedgerEntry() { }

        public LedgerEntry(string id, int position)
        {
            shelter_id = id;
            ledger_position = position;
            compliance_strikes = 0;
            consecutive_compliant_weeks = 0;
            last_requisition_id = string.Empty;
            is_on_patrol_route = true;
            non_compliant_flag = false;
        }

        public LedgerEntry Clone()
        {
            return new LedgerEntry
            {
                shelter_id = shelter_id,
                ledger_position = ledger_position,
                compliance_strikes = compliance_strikes,
                consecutive_compliant_weeks = consecutive_compliant_weeks,
                last_requisition_id = last_requisition_id,
                is_on_patrol_route = is_on_patrol_route,
                non_compliant_flag = non_compliant_flag
            };
        }
    }

    /// <summary>
    /// Central Garrison compliance ledger. Tracks which shelters have paid
    /// their weekly requisition, who is on the patrol route, and who has
    /// slipped below the line into "non-compliant" status. No hard references
    /// to other systems — wire callbacks via the public events.
    /// </summary>
    public class System_GarrisonComplianceLedger
    {
        // Ids (snake_case)
        public const string LedgerSystemId = "system_garrison_compliance_ledger";
        public const string LedgerSlotId = "garrison_compliance_ledger";

        // Lore rules (Expansion II)
        public const int StrikeThreshold = 3;
        public const int ReinstatedWeeks = 4;
        public const float CompliantPatrolWeight = 1.0f;
        public const float NonCompliantPatrolWeight = 0.0f;

        // Events
        public event Action<string, int> OnStrikeRecorded;
        public event Action<string> OnNonCompliant;
        public event Action<string> OnReinstated;

        private GarrisonComplianceLedgerState _state = new GarrisonComplianceLedgerState();

        public IReadOnlyList<LedgerEntry> Entries => _state.entries;

        private int _nextPosition = 1;

        // Record a compliant visit. Four-in-a-row reinstates a non-compliant shelter.
        public void RecordCompliantVisit(string shelterId, int weekIndex)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = EnsureEntry(shelterId);

            entry.compliance_strikes = 0;
            entry.consecutive_compliant_weeks += 1;
            entry.is_on_patrol_route = true;

            if (entry.non_compliant_flag && entry.consecutive_compliant_weeks >= ReinstatedWeeks)
            {
                entry.non_compliant_flag = false;
                entry.consecutive_compliant_weeks = 0;
                OnReinstated?.Invoke(shelterId);
            }
        }

        // Record a requisition id. Does not change strike count.
        public void RecordRequisition(string shelterId, string requisitionId)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = EnsureEntry(shelterId);
            entry.last_requisition_id = requisitionId ?? string.Empty;
        }

        // File a non-compliance strike. Triggers OnStrikeRecorded; at threshold triggers OnNonCompliant.
        public void FileNonCompliance(string shelterId, string reason)
        {
            if (string.IsNullOrEmpty(shelterId)) return;
            var entry = EnsureEntry(shelterId);

            entry.compliance_strikes += 1;
            OnStrikeRecorded?.Invoke(shelterId, entry.compliance_strikes);

            if (!entry.non_compliant_flag && entry.compliance_strikes >= StrikeThreshold)
            {
                entry.non_compliant_flag = true;
                entry.consecutive_compliant_weeks = 0;
                entry.is_on_patrol_route = false;
                OnNonCompliant?.Invoke(shelterId);
            }
        }

        // Force-reinstate after weeks of good behavior. Returns true on transition.
        public bool ReinstateFromNonCompliance(string shelterId, int weeksOfGoodBehavior)
        {
            if (string.IsNullOrEmpty(shelterId)) return false;
            var entry = FindEntry(shelterId);
            if (entry == null || !entry.non_compliant_flag) return false;
            if (weeksOfGoodBehavior < ReinstatedWeeks) return false;

            entry.non_compliant_flag = false;
            entry.compliance_strikes = 0;
            entry.consecutive_compliant_weeks = 0;
            entry.is_on_patrol_route = true;
            OnReinstated?.Invoke(shelterId);
            return true;
        }

        public LedgerEntry GetShelterStatus(string shelterId) => FindEntry(shelterId);

        // Patrol route = compliant shelters.
        public List<string> GetPatrolRoute()
        {
            var result = new List<string>();
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e != null && e.is_on_patrol_route && !e.non_compliant_flag
                    && !string.IsNullOrEmpty(e.shelter_id))
                {
                    result.Add(e.shelter_id);
                }
            }
            return result;
        }

        public float GetPatrolRouteWeight(string shelterId)
        {
            var e = FindEntry(shelterId);
            if (e == null) return CompliantPatrolWeight;
            return e.non_compliant_flag ? NonCompliantPatrolWeight : CompliantPatrolWeight;
        }

        public int EntryCount => _state.entries.Count;

        public GarrisonComplianceLedgerState CaptureState()
        {
            var copy = new GarrisonComplianceLedgerState
            {
                system_id = "system_garrison_compliance_ledger",
                entries = new List<LedgerEntry>()
            };
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e == null || string.IsNullOrEmpty(e.shelter_id)) continue;
                copy.entries.Add(e.Clone());
            }
            return copy;
        }

        public void RestoreState(GarrisonComplianceLedgerState saved)
        {
            if (saved == null)
            {
                _state = new GarrisonComplianceLedgerState();
                _nextPosition = 1;
                return;
            }
            _state = new GarrisonComplianceLedgerState
            {
                system_id = "system_garrison_compliance_ledger",
                entries = new List<LedgerEntry>()
            };
            int maxPos = 0;
            for (int i = 0; i < saved.entries.Count; i++)
            {
                var e = saved.entries[i];
                if (e == null || string.IsNullOrEmpty(e.shelter_id)) continue;
                var clone = e.Clone();
                _state.entries.Add(clone);
                if (clone.ledger_position > maxPos) maxPos = clone.ledger_position;
            }
            _nextPosition = maxPos + 1;
        }

        private LedgerEntry EnsureEntry(string shelterId)
        {
            var existing = FindEntry(shelterId);
            if (existing != null) return existing;
            var fresh = new LedgerEntry(shelterId, _nextPosition);
            _nextPosition += 1;
            _state.entries.Add(fresh);
            return fresh;
        }

        private LedgerEntry FindEntry(string shelterId)
        {
            if (string.IsNullOrEmpty(shelterId)) return null;
            for (int i = 0; i < _state.entries.Count; i++)
            {
                var e = _state.entries[i];
                if (e != null && e.shelter_id == shelterId) return e;
            }
            return null;
        }
    }
}
