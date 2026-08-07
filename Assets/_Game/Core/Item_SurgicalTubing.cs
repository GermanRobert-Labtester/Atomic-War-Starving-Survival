using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SurgicalTubingState
    {
        public string itemId = "item_surgical_tubing";
        public string displayName = "Surgical Tubing & Valves";
        public bool isHighTierCraftingComponent = true;
    }

    /// <summary>
    /// Prompt #465: Artifact: Surgical Tubing & Valves.
    /// Essential high-tier crafting ingredient required for constructing Advanced Hydroponics (#325),
    /// Composting Bio-Latrines (#445), and performing Medical Transfusions (#55).
    /// </summary>
    public class Item_SurgicalTubing
    {
        private SurgicalTubingState _state = new SurgicalTubingState();

        public event Action<SurgicalTubingState, string> OnTubingConsumedInCrafting;

        public SurgicalTubingState State => _state;

        public bool ConsumeForAdvancedRecipe(string recipeId)
        {
            OnTubingConsumedInCrafting?.Invoke(_state, recipeId);
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SurgicalTubingState CaptureState() => _state;

        public void RestoreState(SurgicalTubingState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
