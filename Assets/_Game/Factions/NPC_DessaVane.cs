using System;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_DessaVaneState
    {
        public string id = "npc_dessa_vane";
        public string displayName = "Dessa Vane";
        public bool isActive;
        /// <summary>Contracts drawn and read twice before signing.</summary>
        public int contractsDrawn;
        /// <summary>Forfeits collected at the hall (never at the hatch).</summary>
        public int forfeitsCollected;
        /// <summary>True once Dessa has forgiven a debt publicly (she won't).</summary>
        public bool forgaveDebt;
    }

    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Dessa Vane, keeper of the Underwrite Hall.
    /// Draws contracts, collects forfeits at the hall, never leaves to collect.
    /// "Read it twice. I'll say it twice. After the second time there is only the ink."
    /// </summary>
    public class NPC_DessaVane
    {
        private NPC_DessaVaneState _state = new NPC_DessaVaneState();

        public event Action<NPC_DessaVaneState, int> OnContractDrawn;
        public event Action<NPC_DessaVaneState, int> OnForfeitCollected;

        public NPC_DessaVaneState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>Draw a contract. Read twice. Returns the running count.</summary>
        public int DrawContract()
        {
            _state.contractsDrawn++;
            OnContractDrawn?.Invoke(_state, _state.contractsDrawn);
            return _state.contractsDrawn;
        }

        /// <summary>Collect a forfeit at the hall. Never at the hatch.</summary>
        public int CollectForfeit()
        {
            _state.forfeitsCollected++;
            OnForfeitCollected?.Invoke(_state, _state.forfeitsCollected);
            return _state.forfeitsCollected;
        }

        public NPC_DessaVaneState CaptureState() => _state;
        public void RestoreState(NPC_DessaVaneState saved) { _state = saved ?? new NPC_DessaVaneState(); }
    }
}
