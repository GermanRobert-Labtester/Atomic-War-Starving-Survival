using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CannibalsState
    {
        public string id = "npc_cannibals";
        public string displayName = "Armed Cannibals";
        public float hp = 140f;
        public float moveSpeedMultiplier = 1.6f;
        public List<string> weaponsEquipped = new List<string> { "cleaver", "shotgun" };
        public float horrorMoraleDebuff = 25f;
    }

    /// <summary>
    /// Prompt #336: NPC Encounter: Armed Cannibals.
    /// High speed, high HP combatants with cleavers and shotguns.
    /// Triggers an immediate Horror Morale debuff for the scavenger. Node loot consists of HumanMeat and Bones.
    /// </summary>
    public class NPC_Cannibals
    {
        private CannibalsState _state = new CannibalsState();

        public event Action<CannibalsState, float> OnHorrorDebuffApplied;

        public CannibalsState State => _state;

        public float TriggerEncounterHorror()
        {
            OnHorrorDebuffApplied?.Invoke(_state, _state.horrorMoraleDebuff);
            return _state.horrorMoraleDebuff;
        }

        public List<string> GenerateCannibalLoot()
        {
            return new List<string>
            {
                "human_meat_raw",
                "human_meat_raw",
                "human_bones_pile"
            };
        }
    }
}
