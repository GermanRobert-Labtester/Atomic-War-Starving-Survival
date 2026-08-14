using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_KessAdlerState
    {
        public string id = "npc_kess_adler";
        public string displayName = "Kess Adler";
        public bool isActive;
        public bool pencilAllowed;
        public bool waitInk;
        public bool chartErasedOnce;
        public bool dobErrorLeft;
        public float trust;
    }

    public class NPC_KessAdler
    {
        private NPC_KessAdlerState _state = new NPC_KessAdlerState();

        public event Action<NPC_KessAdlerState> OnStateChanged;

        public NPC_KessAdlerState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        public void AllowPencil(bool allow)
        {
            _state.pencilAllowed = allow;
            _state.waitInk = false;
            OnStateChanged?.Invoke(_state);
        }

        public void SetWaitInk(bool wait)
        {
            _state.waitInk = wait;
            _state.pencilAllowed = false;
            OnStateChanged?.Invoke(_state);
        }

        public void NotifyErased()
        {
            _state.chartErasedOnce = true;
            OnStateChanged?.Invoke(_state);
        }

        public void LeaveDobError(bool leave)
        {
            _state.dobErrorLeft = leave;
            OnStateChanged?.Invoke(_state);
        }

        public NPC_KessAdlerState CaptureState() => _state;
        public void RestoreState(NPC_KessAdlerState saved) { _state = saved ?? new NPC_KessAdlerState(); }
    }
}