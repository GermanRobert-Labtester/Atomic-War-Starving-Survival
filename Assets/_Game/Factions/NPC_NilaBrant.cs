using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_NilaBrantState
    {
        public string id = "npc_nila_brant";
        public string displayName = "Nila Brant";
        public bool isActive;
        public bool accessGranted;
        public bool accessWithdrawn;
        public bool filterTraded;
        public int hiddenCount;
        public float trust;
    }

    /// <summary>
    /// Lamp-oil clerk, unlisted. Occupies Allocation 11 with three others.
    /// Keeps their chart blank. Will not hide a person already on Ormund's return.
    /// Will not open 11 after you ink her living.
    /// </summary>
    public class NPC_NilaBrant
    {
        private NPC_NilaBrantState _state = new NPC_NilaBrantState();

        public event Action<NPC_NilaBrantState> OnStateChanged;

        public NPC_NilaBrantState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        public void GrantAccess(bool granted)
        {
            _state.accessGranted = granted;
            if (granted) _state.accessWithdrawn = false;
            OnStateChanged?.Invoke(_state);
        }

        public void WithdrawAccess()
        {
            _state.accessGranted = false;
            _state.accessWithdrawn = true;
            OnStateChanged?.Invoke(_state);
        }

        public void TradeFilter(bool traded)
        {
            _state.filterTraded = traded;
            OnStateChanged?.Invoke(_state);
        }

        public void HideOne()
        {
            _state.hiddenCount++;
            OnStateChanged?.Invoke(_state);
        }

        public NPC_NilaBrantState CaptureState() => _state;
        public void RestoreState(NPC_NilaBrantState saved) { _state = saved ?? new NPC_NilaBrantState(); }
    }
}