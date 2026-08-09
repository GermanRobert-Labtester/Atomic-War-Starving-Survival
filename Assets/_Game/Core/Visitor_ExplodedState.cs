using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ExplodedStateEffect
    {
        public string cardId = "visitor_exploded_state";
        public string displayName = "The Exploded State (Crater)";
        public int npcCount = 0;
        public float radiationMillisieverts = 5000f; // Extreme 5000mSv
        public List<string> highTierLoot = new List<string> { "military_black_box", "fissile_nuclear_material" };
        public bool requiresEndgameHazmatSuit = true;
    }

    /// <summary>
    /// Prompt #367: Location Visitor: The Exploded State.
    /// Massive crater. Zero NPCs, extreme 5000mSv radiation. Only loot is military BlackBoxes or NuclearMaterial.
    /// Requires endgame Hazmat gear to click on/interact with the node.
    /// </summary>
    
    [Serializable]
    public class Visitor_ExplodedStateSave
    {
        public string systemId = "visitor_exploded_state";
    }
/// <summary>DEMOTE-Visitor-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Visitor_ExplodedState
    {
        private ExplodedStateEffect _effect = new ExplodedStateEffect();

        public event Action<ExplodedStateEffect, bool> OnNodeInteractionAttempted;

        public ExplodedStateEffect Effect => _effect;

        public List<string> TryInteractAndLoot(bool hasEndgameHazmatSuit)
        {
            OnNodeInteractionAttempted?.Invoke(_effect, hasEndgameHazmatSuit);
            if (hasEndgameHazmatSuit)
            {
                return new List<string>(_effect.highTierLoot);
            }
            return new List<string>();
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Visitor_ExplodedStateSave CaptureState() => new Visitor_ExplodedStateSave();

        public void RestoreState(Visitor_ExplodedStateSave saved) { _ = saved; }

}
}
