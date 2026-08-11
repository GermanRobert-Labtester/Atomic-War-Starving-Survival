using System;
using System.Collections.Generic;
using UnityEngine;

using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CognitiveDecayState
    {
        public string survivorId;
        public bool isCognitiveDecayActive = false;
        public bool isUtilityAIFailing = false;
        public float errantActionChance = 0.40f; // 40% chance of random erratic action
    }

    /// <summary>
    /// Prompt #391: System: ARS Stage 4 (Cognitive Decay).
    /// Extreme unchelated radiation sickness causes brain degradation.
    /// Overrides UtilityAI control, causing survivors to randomly drop items, forget cooking, or wander into walls.
    /// </summary>
    
    [Serializable]
    public class CognitiveDecaySystemSave
    {
        public string systemId = "cognitive_decay_system";

        public List<CognitiveDecayState> decayMap = new List<CognitiveDecayState>();
    }
public class CognitiveDecaySystem
    {
        private readonly Dictionary<string, CognitiveDecayState> _decayMap = new Dictionary<string, CognitiveDecayState>();

        public event Action<string> OnCognitiveDecayTriggered;
        public event Action<string, string> OnErraticBehaviorExecuted;

        public IReadOnlyDictionary<string, CognitiveDecayState> DecayMap => _decayMap;

        public void InflictCognitiveDecay(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var state = new CognitiveDecayState { survivorId = survivorId, isCognitiveDecayActive = true, isUtilityAIFailing = true };
            _decayMap[survivorId] = state;

            OnCognitiveDecayTriggered?.Invoke(survivorId);
        }

        public string TickUtilityAIErratica(string survivorId, System.Random rng)
        {
            if (_decayMap.TryGetValue(survivorId, out var state) && state.isCognitiveDecayActive)
            {
                if (rng.NextDouble() < state.errantActionChance)
                {
                    string[] behaviors = { "DropInventoryOnFloor", "ForgetRecipeAndWasteFuel", "WalkIntoWall" };
                    string chosen = behaviors[rng.Next(behaviors.Length)];
                    OnErraticBehaviorExecuted?.Invoke(survivorId, chosen);
                    return chosen;
                }
            }
            return null;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CognitiveDecaySystemSave CaptureState() => new CognitiveDecaySystemSave
        {
            decayMap = SaveMap.Capture(_decayMap),
        };

        public void RestoreState(CognitiveDecaySystemSave saved) =>
            SaveMap.Restore(_decayMap, saved?.decayMap, e => e.survivorId);

}
}
