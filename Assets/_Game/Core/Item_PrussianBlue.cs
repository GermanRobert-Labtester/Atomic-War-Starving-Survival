using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PrussianBlueState
    {
        public string itemId = "item_prussian_blue";
        public string displayName = "Prussian Blue Pills";
        public bool isCraftable = false;
        public string curedAfflictionId = "heavy_metal_poisoning";
    }

    /// <summary>
    /// Prompt #429: Item: Prussian Blue Pills.
    /// Rare pre-war medication. The ONLY item in the game that can safely purge HeavyMetalPoisoning
    /// from drinking tainted stream water. Cannot be crafted.
    /// </summary>
    public class Item_PrussianBlue
    {
        private PrussianBlueState _state = new PrussianBlueState();

        public event Action<PrussianBlueState, string> OnHeavyMetalPoisoningCured;

        public PrussianBlueState State => _state;

        public bool ConsumePill(string survivorId, ref bool hasHeavyMetalPoisoning)
        {
            if (hasHeavyMetalPoisoning)
            {
                hasHeavyMetalPoisoning = false;
                OnHeavyMetalPoisoningCured?.Invoke(_state, survivorId);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PrussianBlueState CaptureState() => _state;

        public void RestoreState(PrussianBlueState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
