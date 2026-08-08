using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AbandonedStateEffect
    {
        public string cardId = "visitor_abandoned";
        public string displayName = "Abandoned State";
        public int npcCount = 0; // ZERO NPCs
        public float structuralHazardMultiplier = 3.0f; // 3x normal hazards
        public List<string> activeHazards = new List<string> { "cave_in", "exposed_wires", "flooded_basement" };
    }

    /// <summary>
    /// Prompt #357: Visitor Event: The Abandoned State.
    /// RNG visitor card spawning zero NPCs. Generates 3x normal structural hazards
    /// (Cave-ins, Exposed wires, Flooded basements). Testing gear, not guns.
    /// </summary>
    
    [Serializable]
    public class Visitor_AbandonedStateSave
    {
        public string systemId = "visitor_abandoned_state";
    }
/// <summary>DEMOTE-Visitor-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Visitor_AbandonedState
    {
        private AbandonedStateEffect _effect = new AbandonedStateEffect();

        public event Action<AbandonedStateEffect, List<string>> OnMaxHazardsGenerated;

        public AbandonedStateEffect Effect => _effect;

        public List<string> GenerateStructuralHazards()
        {
            OnMaxHazardsGenerated?.Invoke(_effect, _effect.activeHazards);
            return new List<string>(_effect.activeHazards);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Visitor_AbandonedStateSave CaptureState() => new Visitor_AbandonedStateSave();

        public void RestoreState(Visitor_AbandonedStateSave saved) { _ = saved; }

}
}
