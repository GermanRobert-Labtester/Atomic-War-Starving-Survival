using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_AshWidowsState
    {
        public string id = "npc_ash_widows";
        public string displayName = "AshWidows";
        public bool isActive = false;
        public bool isHostile = false;
        public float lastSeenDay = -1f;
        public List<string> lootTags = new List<string>();
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — NPC archetype
    /// `NPC_AshWidows`. Description from the design brief; behaviour is host-injected
    /// through the existing EventRunner + faction-trust pipeline.
    /// </summary>
    public class NPC_AshWidows
    {
        private NPC_AshWidowsState _state = new NPC_AshWidowsState();

        public event Action<NPC_AshWidowsState> OnEncounterStarted;
        public event Action<NPC_AshWidowsState> OnEncounterResolved;

        public NPC_AshWidowsState State => _state;

        public void SetActive(bool active) { _state.isActive = active; }
        public void SetHostile(bool hostile) { _state.isHostile = hostile; }

        public NPC_AshWidowsState CaptureState() => _state;
        public void RestoreState(NPC_AshWidowsState saved) { _state = saved ?? new NPC_AshWidowsState(); }
    }
}
