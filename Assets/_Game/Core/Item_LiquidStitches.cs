using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LiquidStitchesState
    {
        public string itemId = "item_liquid_stitches";
        public string displayName = "Liquid Stitches (Superglue)";
        public float infectionChance = 0.35f;
        public string minorInfectionAffliction = "minor_skin_infection";
    }

    /// <summary>
    /// Prompt #431: Item: Liquid Stitches (Superglue).
    /// Applied mid-expedition to instantly cure Lacerations or Bleeding.
    /// Carries a 35% chance to cause a minor Infection, but prevents immediate bleed-out on the road.
    /// </summary>
    public class Item_LiquidStitches
    {
        private LiquidStitchesState _state = new LiquidStitchesState();

        public event Action<LiquidStitchesState, string> OnBleedingSealedFieldTriage;
        public event Action<LiquidStitchesState, string, string> OnInfectionContracted;

        public LiquidStitchesState State => _state;

        public bool ApplyFieldTriage(string survivorId, ref bool isBleedingOrLacerated, System.Random rng, out string contractedInfection)
        {
            contractedInfection = null;
            if (isBleedingOrLacerated)
            {
                isBleedingOrLacerated = false;
                OnBleedingSealedFieldTriage?.Invoke(_state, survivorId);

                if (rng.NextDouble() < _state.infectionChance)
                {
                    contractedInfection = _state.minorInfectionAffliction;
                    OnInfectionContracted?.Invoke(_state, survivorId, contractedInfection);
                }
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public LiquidStitchesState CaptureState() => _state;

        public void RestoreState(LiquidStitchesState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
