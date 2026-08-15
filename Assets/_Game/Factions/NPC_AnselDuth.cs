using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_AnselDuthState
    {
        public string id = "npc_ansel_duth";
        public string displayName = "Ansel Duth";
        public bool isActive;
        public bool childPresent;
        public string childId;
        public bool childTruthTold;
        public bool ladleDefaultSaw;
        public float trust;
    }

    public class NPC_AnselDuth
    {
        private NPC_AnselDuthState _state = new NPC_AnselDuthState();

        public event Action<NPC_AnselDuthState> OnStateChanged;

        public NPC_AnselDuthState State => _state;

        public void Initialise(string displayName, string childId = null)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
            _state.childId = childId;
            _state.childPresent = !string.IsNullOrEmpty(childId);
        }

        public void SetChildPresent(bool present, string childId = null)
        {
            _state.childPresent = present;
            if (childId != null) _state.childId = childId;
            else if (!present) _state.childId = null;
            OnStateChanged?.Invoke(_state);
        }

        public void NotifyChildTruth(bool truthTold)
        {
            _state.childTruthTold = truthTold;
            OnStateChanged?.Invoke(_state);
        }

        public void NotifyLadleDefault()
        {
            _state.ladleDefaultSaw = true;
            OnStateChanged?.Invoke(_state);
        }

        public NPC_AnselDuthState CaptureState() => _state;
        public void RestoreState(NPC_AnselDuthState saved) { _state = saved ?? new NPC_AnselDuthState(); }
    }
}