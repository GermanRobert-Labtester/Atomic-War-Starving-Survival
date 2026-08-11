using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_FeralChildrenState
    {
        public string id = "npc_feralchildren";
        public string displayName = "FeralChildren";
        public bool isActive = false;
        public bool isHostile = false;
        public float lastSeenDay = -1f;
        public List<string> lootTags = new List<string>();
    }

    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — NPC archetype
    /// `NPC_FeralChildren`. Description from the design brief; behaviour is host-injected
    /// through the existing EventRunner + faction-trust pipeline.
    /// </summary>
    public class NPC_FeralChildren
    {
        private NPC_FeralChildrenState _state = new NPC_FeralChildrenState();

        public event Action<NPC_FeralChildrenState> OnEncounterStarted;
        public event Action<NPC_FeralChildrenState> OnEncounterResolved;

        public NPC_FeralChildrenState State => _state;

        public void SetActive(bool active) { _state.isActive = active; }
        public void SetHostile(bool hostile) { _state.isHostile = hostile; }

        public NPC_FeralChildrenState CaptureState() => _state;
        public void RestoreState(NPC_FeralChildrenState saved) { _state = saved ?? new NPC_FeralChildrenState(); }
    }
}
