using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_LenQuillState
    {
        public string id = "npc_len_quill";
        public string displayName = "Len Quill";
        public bool isActive;
        public bool hasTag;
        public string tagSentence;
        public bool invitedToAirlock;
        public float trust;
    }

    /// <summary>
    /// Quiet House runner. Not a medic. Two knocks. Name, and one true thing.
    /// Will not enter the Stack uninvited. Will not adjudicate the back room.
    /// </summary>
    public class NPC_LenQuill
    {
        private NPC_LenQuillState _state = new NPC_LenQuillState();

        public event Action<NPC_LenQuillState> OnStateChanged;

        public NPC_LenQuillState State => _state;

        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        public void WriteTag(string sentence)
        {
            _state.hasTag = true;
            _state.tagSentence = sentence;
            OnStateChanged?.Invoke(_state);
        }

        public void InviteToAirlock(bool invited)
        {
            _state.invitedToAirlock = invited;
            OnStateChanged?.Invoke(_state);
        }

        public NPC_LenQuillState CaptureState() => _state;
        public void RestoreState(NPC_LenQuillState saved) { _state = saved ?? new NPC_LenQuillState(); }
    }
}