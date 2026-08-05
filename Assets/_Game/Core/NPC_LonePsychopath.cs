using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LonePsychopathState
    {
        public string id = "npc_lone_psychopath";
        public string displayName = "The Lone Psychopath";
        public bool canFlee = false;
        public int bearTrapCount = 4;
        public int tripwireCount = 3;
        public int huntProgress = 0;
        public int requiredHuntProgress = 4;
        public bool isHuntedDown = false;
    }

    /// <summary>
    /// Prompt #337: NPC Encounter: The Lone Psychopath.
    /// Single enemy in a node rigged with BearTraps and Tripwires. Stalks player through UI.
    /// Player cannot flee and must hunt them down in a cat-and-mouse minigame.
    /// </summary>
    public class NPC_LonePsychopath
    {
        private LonePsychopathState _state = new LonePsychopathState();

        public event Action<LonePsychopathState, int> OnHuntProgressed;
        public event Action<LonePsychopathState> OnPsychopathHuntedDown;
        public event Action<LonePsychopathState, float> OnTrapTriggered;

        public LonePsychopathState State => _state;

        public bool AdvanceHunt(bool passedPerceptionCheck, out float trapDamage)
        {
            trapDamage = 0f;
            if (!passedPerceptionCheck)
            {
                trapDamage = 20f;
                OnTrapTriggered?.Invoke(_state, trapDamage);
            }

            _state.huntProgress++;
            OnHuntProgressed?.Invoke(_state, _state.huntProgress);

            if (_state.huntProgress >= _state.requiredHuntProgress)
            {
                _state.isHuntedDown = true;
                OnPsychopathHuntedDown?.Invoke(_state);
                return true;
            }
            return false;
        }
    }
}
