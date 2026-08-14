using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_TamsinRookState
    {
        public string id = "npc_tamsin_rook";
        public string displayName = "Tamsin Rook";
        public bool isActive;
        public bool watchShort;
        public bool sentToWaystation;
        public bool intercomTruthKept;
        public float trust;
    }

    /// <summary>
    /// Harbour night-clerk, unlisted. The hatch intercom. Will not lie about
    /// who is outside. Will not sleep the same bunk two nights if the watch is short.
    /// </summary>
    public class NPC_TamsinRook
    {
        private NPC_TamsinRookState _state = new NPC_TamsinRookState();

        public event Action<NPC_TamsinRookState> OnStateChanged;

        public NPC_TamsinRookState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        public void NotifyWatchShort()
        {
            _state.watchShort = true;
            OnStateChanged?.Invoke(_state);
        }

        public void SendToWaystation(bool sent)
        {
            _state.sentToWaystation = sent;
            OnStateChanged?.Invoke(_state);
        }

        public void NotifyIntercomTruth(bool kept)
        {
            _state.intercomTruthKept = kept;
            OnStateChanged?.Invoke(_state);
        }

        public NPC_TamsinRookState CaptureState() => _state;
        public void RestoreState(NPC_TamsinRookState saved) { _state = saved ?? new NPC_TamsinRookState(); }
    }
}