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
