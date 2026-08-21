using System;
using System.Collections.Generic;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — the Evidence Ledger.
    /// A dedicated domain ledger (NOT items, NOT loose flags): evidence is the
    /// authoritative state that drives the three Verdict endings. One record
    /// per evidence id; enrollment is one-way and idempotent.
    /// </summary>
    [Serializable]
    public sealed class EvidenceLedgerState
    {
        public List<string> enrolled = new List<string>();
        public string lastEnrolled = string.Empty;
        public int enrollmentDay = -1;
    }

    /// <summary>Evidence definition (verdict_data.json 'evidence' section).</summary>
    public class EvidenceDefinition
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string category = "story_item";
        public string tier = "Old-World";
        public string flavor = string.Empty;
        public string questTrigger = string.Empty;
        public string factionAffinity = string.Empty;
        public string rarity = "Rare";
    }

    public sealed class EvidenceLedger
    {
        private readonly EvidenceLedgerState _state;
        private readonly Dictionary<string, EvidenceDefinition> _catalog =
            new Dictionary<string, EvidenceDefinition>(StringComparer.Ordinal);

        public EvidenceLedgerState State => _state;
        public IReadOnlyList<string> Enrolled => _state.enrolled;

        public event Action<string> OnEnrolled;

        public EvidenceLedger(EvidenceLedgerState state = null)
        {
            _state = state ?? new EvidenceLedgerState();
        }

        public void Register(EvidenceDefinition def)
        {
            if (def != null && !string.IsNullOrEmpty(def.id) && !_catalog.ContainsKey(def.id))
                _catalog[def.id] = def;
        }

        public EvidenceDefinition? Get(string id)
            => string.IsNullOrEmpty(id) ? null : (_catalog.TryGetValue(id, out var d) ? d : null);

        public bool IsEnrolled(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < _state.enrolled.Count; i++)
                if (_state.enrolled[i] == id) return true;
            return false;
        }

        /// <summary>Enroll an evidence fragment. Idempotent; fires OnEnrolled once.</summary>
        public bool Enroll(string id, int day)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (_catalog.Count > 0 && !_catalog.ContainsKey(id)) return false;
            if (IsEnrolled(id)) return false; // idempotent — a record cannot be read twice
            _state.enrolled.Add(id);
            _state.lastEnrolled = id;
            _state.enrollmentDay = day;
            OnEnrolled?.Invoke(id);
            return true;
        }

        public int Count => _state.enrolled.Count;

        public EvidenceLedgerState CaptureState()
        {
            var copy = new EvidenceLedgerState
            {
                lastEnrolled = _state.lastEnrolled,
                enrollmentDay = _state.enrollmentDay
            };
            copy.enrolled.AddRange(_state.enrolled);
            return copy;
        }

        public void RestoreState(EvidenceLedgerState state)
        {
            if (state == null) return;
            _state.enrolled.Clear();
            _state.enrolled.AddRange(state.enrolled);
            _state.lastEnrolled = state.lastEnrolled;
            _state.enrollmentDay = state.enrollmentDay;
        }
    }
}
