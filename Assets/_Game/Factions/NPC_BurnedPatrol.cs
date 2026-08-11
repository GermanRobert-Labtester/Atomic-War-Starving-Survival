using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_BurnedPatrolState
    {
        public string id = "npc_burnedpatrol";
        public string displayName = "BurnedPatrol";
        public bool isActive = false;
        public bool isHostile = false;
        public float lastSeenDay = -1f;
        public List<string> lootTags = new List<string>();
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — NPC archetype
    /// `NPC_BurnedPatrol`. Description from the design brief; behaviour is host-injected
    /// through the existing EventRunner + faction-trust pipeline.
    /// </summary>
    public class NPC_BurnedPatrol
    {
        private NPC_BurnedPatrolState _state = new NPC_BurnedPatrolState();

        public event Action<NPC_BurnedPatrolState> OnEncounterStarted;
        public event Action<NPC_BurnedPatrolState> OnEncounterResolved;

        public NPC_BurnedPatrolState State => _state;

        public void SetActive(bool active) { _state.isActive = active; }
        public void SetHostile(bool hostile) { _state.isHostile = hostile; }

        public NPC_BurnedPatrolState CaptureState() => _state;
        public void RestoreState(NPC_BurnedPatrolState saved) { _state = saved ?? new NPC_BurnedPatrolState(); }
    }
}
