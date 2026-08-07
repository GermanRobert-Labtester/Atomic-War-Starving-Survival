using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MutantGlandState
    {
        public string itemId = "item_mutant_gland";
        public string displayName = "Mutant Gland";
        public string sourceCreature = "";
        public float weight = 0.5f;
        public bool isConsumable = true;
        public int stackMax = 3;
    }

    /// <summary>
    /// Prompt #573 companion: Item: Mutant Gland (for pheromone masking system).
    /// Harvested from Amalgamations or Bears. Crushed and spread on HazmatSuit.
    /// Grants 24h of animal-friendly, human-hostile pheromone masking.
    /// </summary>
    public class Item_MutantGland
    {
        private MutantGlandState _state = new MutantGlandState();

        public event Action<MutantGlandState, string> OnGlandHarvested;
        public event Action<MutantGlandState> OnGlandApplied;

        public MutantGlandState State => _state;

        public MutantGlandState Harvest(string sourceCreature)
        {
            _state.sourceCreature = sourceCreature;
            OnGlandHarvested?.Invoke(_state, sourceCreature);
            return _state;
        }

        public bool CanApply(bool hasHazmatSuit)
        {
            return hasHazmatSuit;
        }

        public MutantGlandState ApplyToSuit()
        {
            OnGlandApplied?.Invoke(_state);
            return _state; // Caller uses this to activate PheromoneMaskingSystem
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MutantGlandState CaptureState() => _state;

        public void RestoreState(MutantGlandState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
