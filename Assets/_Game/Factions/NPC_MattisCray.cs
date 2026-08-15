using System;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_MattisCrayState
    {
        public string id = "npc_mattis_cray";
        public string displayName = "Mattis Cray";
        public bool isActive;
        /// <summary>Times Mattis has staked his name on a vouchee.</summary>
        public int vouchesGiven;
        /// <summary>He will not vouch a second time for someone who burned him once.</summary>
        public bool hasBeenBurned;
    }

    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Mattis Cray, the best kind of gate attend.
    /// Will vouch, will run messages, will not pick a bloc. The pack's explicit
    /// "last resort" vouch: always available at real narrative cost.
    /// </summary>
    public class NPC_MattisCray
    {
        private NPC_MattisCrayState _state = new NPC_MattisCrayState();

        public event Action<NPC_MattisCrayState, int> OnVouchGiven;
        public event Action<NPC_MattisCrayState> OnBurned;

        public NPC_MattisCrayState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>True while Mattis will still stake his name (he has not been burned).</summary>
        public bool WillVouch => !_state.hasBeenBurned;

        /// <summary>Mattis vouches. Idempotent while still trusted.</summary>
        public bool GiveVouch()
        {
            if (_state.hasBeenBurned) return false;
            _state.vouchesGiven++;
            OnVouchGiven?.Invoke(_state, _state.vouchesGiven);
            return true;
        }

        /// <summary>The vouchee burned his name. He will not vouch again.</summary>
        public void BurnMattis()
        {
            if (_state.hasBeenBurned) return;
            _state.hasBeenBurned = true;
            OnBurned?.Invoke(_state);
        }

        public NPC_MattisCrayState CaptureState() => _state;
        public void RestoreState(NPC_MattisCrayState saved) { _state = saved ?? new NPC_MattisCrayState(); }
    }
}