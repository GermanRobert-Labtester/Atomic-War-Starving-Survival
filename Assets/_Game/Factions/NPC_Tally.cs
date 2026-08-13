using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_TallyState
    {
        public string id = "faction_the_tally";
        public string displayName = "The Tally";
        public bool isActive;
        public List<TallyContract> contracts = new List<TallyContract>();
        public string badgeAssetId = "faction_leader_2";
    }

    [Serializable]
    public class TallyContract
    {
        public string debtorId;
        public string debt;
        public string term;
        public string rate;
        public string forfeit;
        public int dueDay;
        public bool enforced;
    }

    /// <summary>
    /// Lore bible 05_FACTIONS §7 — The Tally (dangerous, lawful Current).
    /// Debt enforcement by written contract. They do not raid, do not extort,
    /// and have never once been known to break an agreement — including
    /// agreements that end with someone's death. The contract is read back
    /// before signing, and read again, in full, before enforcement.
    /// </summary>
    public class NPC_Tally
    {
        private NPC_TallyState _state = new NPC_TallyState();

        public event Action<NPC_TallyState, TallyContract> OnContractEnforced;

        public NPC_TallyState State => _state;

        public void Initialise(string displayName, string badgeAssetId)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            if (!string.IsNullOrEmpty(badgeAssetId)) _state.badgeAssetId = badgeAssetId;
            _state.isActive = true;
        }

        /// <summary>Write a contract: the debt, the term, the rate, and the forfeit, in plain language.</summary>
        public TallyContract WriteContract(string debtorId, string debt, string term, string rate, string forfeit, int dueDay)
        {
            var c = new TallyContract
            {
                debtorId = debtorId ?? "",
                debt = debt ?? "",
                term = term ?? "",
                rate = rate ?? "",
                forfeit = forfeit ?? "",
                dueDay = dueDay
            };
            _state.contracts.Add(c);
            return c;
        }

        /// <summary>
        /// Daily tick: contracts due today are enforced, as written. The Tally
        /// has never once been known to break an agreement.
        /// </summary>
        public List<TallyContract> EnforceDueContracts(int currentDay)
        {
            var due = new List<TallyContract>();
            for (int i = 0; i < _state.contracts.Count; i++)
            {
                var c = _state.contracts[i];
                if (c.enforced || c.dueDay > currentDay) continue;
                c.enforced = true;
                due.Add(c);
                OnContractEnforced?.Invoke(_state, c);
            }
            return due;
        }

        public NPC_TallyState CaptureState() => _state;
        public void RestoreState(NPC_TallyState saved) { _state = saved ?? new NPC_TallyState(); }
    }
}
