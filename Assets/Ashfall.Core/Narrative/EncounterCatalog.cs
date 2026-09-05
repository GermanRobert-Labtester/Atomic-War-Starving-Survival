using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public enum EncounterCategory
    {
        Hazard,
        Discovery,
        Social,
        Trade
    }

    /// <summary>One selectable choice on an encounter (data-driven).</summary>
    [System.Serializable]
    public class EncounterChoiceDefinition
    {
        public string choiceId = string.Empty;
        public string text = string.Empty;
        public int moraleDelta = 0;
        public int guiltDelta = 0;

        // Plan 49 — Micro-location extensions (backward-compatible defaults)
        /// <summary>Item ID to grant on choice resolution. Empty = no grant.</summary>
        public string grantItemId = string.Empty;
        /// <summary>Quantity of granted item. 0 = no grant.</summary>
        public int grantItemQuantity = 0;
        /// <summary>World flag to set on choice resolution. Empty = no flag.</summary>
        public string setWorldFlag = string.Empty;
        /// <summary>Journal/codex knowledge key to unlock. Empty = no unlock.</summary>
        public string journalUnlockId = string.Empty;
        /// <summary>Location ID to discover via radio triangulation. Empty = no discovery.</summary>
        public string discoverLocationId = string.Empty;
        /// <summary>Whether this choice depletes the micro-location (one-time loot).</summary>
        public bool depletesOnResolve = false;

        // Plan 52 — Recurring NPC arc extensions (backward-compatible defaults)
        /// <summary>Expansion quest this choice completes on resolution. The
        /// quest progress (including the recorded choice id) is the persisted
        /// arc-memory authority. Empty = no arc decision.</summary>
        public string completesQuestId = string.Empty;
        /// <summary>Choice id recorded into the expansion-quest progress when
        /// this encounter choice resolves. Empty = complete without a choice.</summary>
        public string completesQuestChoiceId = string.Empty;

        // Plan 45 / Patrol Encounter extensions (backward-compatible defaults)
        /// <summary>Required item ID to enable this choice.</summary>
        public string requiredItemId = string.Empty;
        /// <summary>Required quantity of the gating item.</summary>
        public int requiredItemQuantity = 0;
        /// <summary>Faction associated with this choice.</summary>
        public string factionId = string.Empty;
        /// <summary>Delta applied to faction standing on resolution.</summary>
        public int factionStandingDelta = 0;
        /// <summary>Items consumed by selecting this choice.</summary>
        public List<string> costItems = new List<string>();
    }

    /// <summary>
    /// One narrative encounter definition (the JSON is the authority; this
    /// mirrors the Unity EncounterSO fields the expedition system consumed).
    /// </summary>
    [System.Serializable]
    public class EncounterDefinition
    {
        public string id = string.Empty;
        public string title = string.Empty;
        public string description = string.Empty;
        public string category = "Discovery";
        public float baseWeight = 1f;
        public float stealthWeightMultiplier = 0.5f;
        public float speedWeightMultiplier = 1.5f;
        public float minDangerLevel = 0f;
        public string requiredLocationId = string.Empty;
        public bool forceOnArrival = false;

        // Plan 52 — Recurring NPC arc extension (backward-compatible default)
        /// <summary>npc_* id of the recurring character featured in this
        /// encounter. Empty = anonymous encounter. Data-linkage only; the arc
        /// resolver never reads it.</summary>
        public string npcId = string.Empty;
        public bool isMicroLocation = false;
        public string sourceFile = string.Empty;

        public List<EncounterChoiceDefinition> choices = new List<EncounterChoiceDefinition>();

        /// <summary>Unity parity: effective selection weight for a stance + danger + location.</summary>
        public float GetEffectiveWeight(string stance, float dangerLevel, string locationId)
        {
            if (dangerLevel < minDangerLevel) return 0f;
            if (!string.IsNullOrEmpty(requiredLocationId))
            {
                if (string.IsNullOrEmpty(locationId)
                    || !string.Equals(requiredLocationId, locationId, System.StringComparison.Ordinal))
                    return 0f;
            }
            float weight = baseWeight;
            if (stance == "Stealth") weight *= stealthWeightMultiplier;
            else if (stance == "Speed") weight *= speedWeightMultiplier;
            return System.Math.Max(0f, weight);
        }
    }

    /// <summary>Serialized resolution history (save/load safe).</summary>
    [System.Serializable]
    public class EncounterResolutionRecord
    {
        public string encounterId = string.Empty;
        public string choiceId = string.Empty;
        public string locationId = string.Empty;
        public int day = 0;
        public int moraleDelta = 0;
        public int guiltDelta = 0;
    }

    [System.Serializable]
    public class NarrativeEncounterState
    {
        public string systemId = NarrativeEncounterSystem.SystemId;
        public int totalResolved = 0;
        public int cumulativeMorale = 0;
        public int cumulativeGuilt = 0;
        public List<EncounterResolutionRecord> history = new List<EncounterResolutionRecord>();
        public List<PendingSurfacedEncounter> pending = new List<PendingSurfacedEncounter>();

        /// <summary>F1 — encounter IDs whose depleting choice has been resolved.
        /// Null on legacy saves that predate depletion (restore then reconstructs
        /// the set from history); a present list (even empty) means "known".</summary>
        public List<string>? depletedEncounterIds = new List<string>();
    }

    [System.Serializable]
    public class PendingSurfacedEncounter
    {
        public string encounterId = string.Empty;
        public string locationId = string.Empty;
        public int legIndex = 0;
        public int day = 0;
    }
}
