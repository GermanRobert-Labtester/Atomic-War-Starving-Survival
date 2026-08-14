using System;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_WynSablerState
    {
        public string id = "npc_wyn_sabler";
        public string displayName = "Wyn Sabler";
        public bool isActive;
        /// <summary>Times Wyn has recited her own terms back, unprompted.</summary>
        public int termsRecited;
        /// <summary>Grain still hers in the pledged granary — countable, real.</summary>
        public float grainPledged = 60f;
        /// <summary>True once she fled with the pledged grain (theft of collateral).</summary>
        public bool fledWithGrain;
        /// <summary>True once her pledge was paid in full on the original terms.</summary>
        public bool debtHonoured;
    }

    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Wyn Sabler, farmer. Second bad season.
    /// Her granary pledge is due and she can count exactly what is owed
    /// without looking at the paper. She will not ask anyone to break the
    /// terms for her — she read them twice, same as everyone. She will
    /// accept it if the player breaks them anyway.
    /// Stationary at the Annex; her pledge sits at the Pledged Granary.
    /// </summary>
    public class NPC_WynSabler
    {
        private NPC_WynSablerState _state = new NPC_WynSablerState();

        public event Action<NPC_WynSablerState, int> OnTermsRecited;
        public event Action<NPC_WynSablerState> OnFledWithGrain;
        public event Action<NPC_WynSablerState> OnDebtHonoured;

        public NPC_WynSablerState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>Recite her own terms, unprompted. Returns the running count.</summary>
        public int ReciteTerms()
        {
            _state.termsRecited++;
            OnTermsRecited?.Invoke(_state, _state.termsRecited);
            return _state.termsRecited;
        }

        /// <summary>
        /// Flee with the pledged grain before collection. She never asked;
        /// she accepts it anyway. One-time — the theft of collateral.
        /// </summary>
        public bool FleeWithGrain()
        {
            if (_state.fledWithGrain) return false;
            _state.fledWithGrain = true;
            OnFledWithGrain?.Invoke(_state);
            return true;
        }

        /// <summary>Paid in full, honestly, on the original terms. One-time.</summary>
        public bool HonourDebt()
        {
            if (_state.debtHonoured) return false;
            _state.debtHonoured = true;
            OnDebtHonoured?.Invoke(_state);
            return true;
        }

        public NPC_WynSablerState CaptureState() => _state;
        public void RestoreState(NPC_WynSablerState saved) { _state = saved ?? new NPC_WynSablerState(); }
    }
}
