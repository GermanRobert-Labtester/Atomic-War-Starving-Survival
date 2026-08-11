using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_TheTollmanState
    {
        public string id = "npc_thetollman";
        public string displayName = "TheTollman";
        public bool isActive = false;
        public bool isHostile = false;
        public float lastSeenDay = -1f;
        public List<string> lootTags = new List<string>();
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — NPC archetype
    /// `NPC_TheTollman`. Description from the design brief; behaviour is host-injected
    /// through the existing EventRunner + faction-trust pipeline.
    /// </summary>
    public class NPC_TheTollman
    {
        private NPC_TheTollmanState _state = new NPC_TheTollmanState();

        public event Action<NPC_TheTollmanState> OnEncounterStarted;
        public event Action<NPC_TheTollmanState> OnEncounterResolved;

        public NPC_TheTollmanState State => _state;

        public void SetActive(bool active) { _state.isActive = active; }
        public void SetHostile(bool hostile) { _state.isHostile = hostile; }

        public NPC_TheTollmanState CaptureState() => _state;
        public void RestoreState(NPC_TheTollmanState saved) { _state = saved ?? new NPC_TheTollmanState(); }
    }
}
