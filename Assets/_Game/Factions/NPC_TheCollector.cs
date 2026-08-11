using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_TheCollectorState
    {
        public string id = "npc_the_collector";
        public string displayName = "TheCollector";
        public bool isActive = false;
        public bool isHostile = false;
        public float lastSeenDay = -1f;
        public List<string> lootTags = new List<string>();
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — NPC archetype
    /// `NPC_TheCollector`. Description from the design brief; behaviour is host-injected
    /// through the existing EventRunner + faction-trust pipeline.
    /// </summary>
    public class NPC_TheCollector
    {
        private NPC_TheCollectorState _state = new NPC_TheCollectorState();

        public event Action<NPC_TheCollectorState> OnEncounterStarted;
        public event Action<NPC_TheCollectorState> OnEncounterResolved;

        public NPC_TheCollectorState State => _state;

        public void SetActive(bool active) { _state.isActive = active; }
        public void SetHostile(bool hostile) { _state.isHostile = hostile; }

        public NPC_TheCollectorState CaptureState() => _state;
        public void RestoreState(NPC_TheCollectorState saved) { _state = saved ?? new NPC_TheCollectorState(); }
    }
}
