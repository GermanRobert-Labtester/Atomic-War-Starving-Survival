using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_EdorValeState
    {
        public string id = "npc_edor_vale";
        public string displayName = "Edor Vale";
        public bool isActive;
        public bool waitingAtHatch;
        public bool interviewDone;
        public bool dobErrorLeft;
        public float trust;
    }

    /// <summary>
    /// Census Clerk Grade III. Will not enter the bunker uninvited. Will not falsify a DOB.
    /// </summary>
    public class NPC_EdorVale
    {
        private NPC_EdorValeState _state = new NPC_EdorValeState();

        public event Action<NPC_EdorValeState> OnWaitingChanged;
        public event Action<NPC_EdorValeState> OnStateChanged;

        public NPC_EdorValeState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        public void SetWaitingAtHatch(bool waiting)
        {
            _state.waitingAtHatch = waiting;
            _state.interviewDone = true;
            OnWaitingChanged?.Invoke(_state);
            OnStateChanged?.Invoke(_state);
        }

        public void LeaveDobError(bool leave)
        {
            _state.dobErrorLeft = leave;
            OnStateChanged?.Invoke(_state);
        }

        public NPC_EdorValeState CaptureState() => _state;
        public void RestoreState(NPC_EdorValeState saved) { _state = saved ?? new NPC_EdorValeState(); }
    }
}
