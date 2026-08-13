#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_SurgeonsCaravanState
    {
        public string id = "npc_the_surgeon_caravan";
        public string displayName = "SurgeonsCaravan";
        public bool isActive = false;
        public bool isHostile = false;
        public float lastSeenDay = -1f;
        public List<string> lootTags = new List<string>();
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — NPC archetype
    /// `NPC_SurgeonsCaravan`. Description from the design brief; behaviour is host-injected
    /// through the existing EventRunner + faction-trust pipeline.
    /// </summary>
    public class NPC_SurgeonsCaravan
    {
        private NPC_SurgeonsCaravanState _state = new NPC_SurgeonsCaravanState();

        public event Action<NPC_SurgeonsCaravanState> OnEncounterStarted;
        public event Action<NPC_SurgeonsCaravanState> OnEncounterResolved;

        public NPC_SurgeonsCaravanState State => _state;

        public void SetActive(bool active) { _state.isActive = active; }
        public void SetHostile(bool hostile) { _state.isHostile = hostile; }

        public NPC_SurgeonsCaravanState CaptureState() => _state;
        public void RestoreState(NPC_SurgeonsCaravanState saved) { _state = saved ?? new NPC_SurgeonsCaravanState(); }
    }
}
