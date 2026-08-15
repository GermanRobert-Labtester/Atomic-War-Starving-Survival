using System;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_OsranKellState
    {
        public string id = "npc_osran_kell";
        public string displayName = "Osran Kell";
        public bool isActive;
        /// <summary>Honest weighs performed on the depot scale.</summary>
        public int weighsPerformed;
        /// <summary>True once Osran has refused a bribe on the record.</summary>
        public bool refusedBribe;
        /// <summary>Escalated by a bribe attempt: trade access stays, but permanently colder.</summary>
        public bool bribeAttempted;
    }

    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Osran Kell, the Scalehouse's keeper.
    /// Weighs, verifies, refuses to arbitrate. "The number is real; what
    /// people infer from it is not my problem." Companion (labour + expedition
    /// company), not a party member.
    /// </summary>
    public class NPC_OsranKell
    {
        private NPC_OsranKellState _state = new NPC_OsranKellState();

        public event Action<NPC_OsranKellState, int> OnWeighPerformed;
        public event Action<NPC_OsranKellState> OnRefusedBribe;

        public NPC_OsranKellState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>An honest weigh. Returns the confirmed weight-reading count.</summary>
        public int PerformWeigh()
        {
            _state.weighsPerformed++;
            OnWeighPerformed?.Invoke(_state, _state.weighsPerformed);
            return _state.weighsPerformed;
        }

        /// <summary>Osran declines an attempted calibration-bribe, in front of witnesses.</summary>
        public bool AttemptBribe()
        {
            if (_state.refusedBribe) return false;
            _state.refusedBribe = true;
            _state.bribeAttempted = true;
            OnRefusedBribe?.Invoke(_state);
            return true;
        }

        public NPC_OsranKellState CaptureState() => _state;
        public void RestoreState(NPC_OsranKellState saved) { _state = saved ?? new NPC_OsranKellState(); }
    }
}