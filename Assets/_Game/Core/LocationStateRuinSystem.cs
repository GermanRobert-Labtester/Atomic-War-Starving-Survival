using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RuinEffects
    {
        public LocationStateModifier modifier;
        public float ashCharcoalLootRatio; // 0.5 for HalfBurned
        public float ambientHeatDelta;      // High heat for HalfBurned
        public bool hasMassiveDebris;       // Exploded
        public bool layoutFragmented;       // Exploded
        public float radiationMultiplier;  // Exploded
        public bool forceZeroNpcs;          // Abandoned
        public float verminAndMoldLevel;    // 1.0 for Abandoned
    }

    /// <summary>
    /// Prompt #320: System: The State of Ruin (Environmental RNG).
    /// Applies environmental modifiers (Pristine, Looted, HalfBurned, Exploded, Abandoned)
    /// to procedural node layouts before NPC visitor assignment.
    /// </summary>
    
    [Serializable]
    public class LocationStateRuinSystemSave
    {
        public string systemId = "location_state_ruin_system";
    }
public class LocationStateRuinSystem
    {
        public event Action<string, LocationStateModifier, RuinEffects> OnRuinModifierApplied;

        public RuinEffects ApplyRuinModifier(FixedNodeState node, LocationStateModifier modifier)
        {
            if (node == null) return null;
            node.ruinModifier = modifier.ToString();

            RuinEffects effects = ComputeEffects(modifier);

            if (effects.forceZeroNpcs)
            {
                node.assignedVisitorCardId = "visitor_abandoned";
                node.assignedVisitorTitle = "Abandoned Ruins";
                node.primaryFactionId = "none";
                node.isSkirmishActive = false;
            }

            OnRuinModifierApplied?.Invoke(node.locationId, modifier, effects);
            return effects;
        }

        public RuinEffects ComputeEffects(LocationStateModifier modifier)
        {
            var effects = new RuinEffects
            {
                modifier = modifier,
                ashCharcoalLootRatio = 0f,
                ambientHeatDelta = 0f,
                hasMassiveDebris = false,
                layoutFragmented = false,
                radiationMultiplier = 1f,
                forceZeroNpcs = false,
                verminAndMoldLevel = 0f
            };

            switch (modifier)
            {
                case LocationStateModifier.HalfBurned:
                    effects.ashCharcoalLootRatio = 0.5f; // 50% loot replaced with Ash/Charcoal
                    effects.ambientHeatDelta = 20f;      // Ambient heat is high
                    break;
                case LocationStateModifier.Exploded:
                    effects.layoutFragmented = true;
                    effects.hasMassiveDebris = true;      // Massive debris blocks paths
                    effects.radiationMultiplier = 4.0f;  // Extreme radiation
                    break;
                case LocationStateModifier.Abandoned:
                    effects.forceZeroNpcs = true;         // Zero NPCs
                    effects.verminAndMoldLevel = 1.0f;   // Max vermin and mold
                    break;
                case LocationStateModifier.Looted:
                    effects.ashCharcoalLootRatio = 0.2f;
                    effects.verminAndMoldLevel = 0.3f;
                    break;
                case LocationStateModifier.Pristine:
                default:
                    break;
            }

            return effects;
        }

        public LocationStateModifier DrawRandomModifier(System.Random rng)
        {
            var values = (LocationStateModifier[])Enum.GetValues(typeof(LocationStateModifier));
            return values[rng.Next(0, values.Length)];
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public LocationStateRuinSystemSave CaptureState() => new LocationStateRuinSystemSave();

        public void RestoreState(LocationStateRuinSystemSave saved) { _ = saved; }

}
}
