using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TheOldState
    {
        public string id = "npc_the_old";
        public string displayName = "The Old";
        public bool isDefenseless = true;
        public float teaMoraleRestore = 15f;
        public float storySkillXpGain = 100f;
        public bool isProtectedFromRaid = false;
    }

    /// <summary>
    /// Prompt #352: NPC Encounter: The Old.
    /// Defenseless elders who refused to leave home. Offer tea (+15 Morale) and tell stories (+100 Skill XP).
    /// If a Faction Raid hits this node, the player must defend them.
    /// </summary>
    public class NPC_TheOld
    {
        private TheOldState _state = new TheOldState();

        public event Action<TheOldState, float, float> OnTeaAndStoriesShared;
        public event Action<TheOldState> OnDefendedFromRaid;

        public TheOldState State => _state;

        public (float morale, float xp) DrinkTeaAndListenToStories()
        {
            OnTeaAndStoriesShared?.Invoke(_state, _state.teaMoraleRestore, _state.storySkillXpGain);
            return (_state.teaMoraleRestore, _state.storySkillXpGain);
        }

        public void DefendFromFactionRaid()
        {
            _state.isProtectedFromRaid = true;
            OnDefendedFromRaid?.Invoke(_state);
        }
    }
}
