using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PotassiumIodideState
    {
        public string itemId = "item_potassium_iodide";
        public string displayName = "Potassium Iodide (KI)";
        public float preventiveBlockRatio = 0.90f; // 90% block if taken before
        public float reactiveBlockRatio = 0.10f;    // 10% block if taken after
    }

    /// <summary>
    /// Prompt #436: Item: Potassium Iodide (KI).
    /// Gold standard for radiation protection. Taking before entering a radiation zone blocks 90%
    /// of internal rad accumulation; taking after blocks only 10%. Pure timing mechanic.
    /// </summary>
    public class Item_PotassiumIodide
    {
        private PotassiumIodideState _state = new PotassiumIodideState();

        public event Action<PotassiumIodideState, bool, float> OnKIPillConsumed;

        public PotassiumIodideState State => _state;

        public float CalculateRadProtection(bool isTakenBeforeZoneEntry)
        {
            float blockRatio = isTakenBeforeZoneEntry ? _state.preventiveBlockRatio : _state.reactiveBlockRatio;
            OnKIPillConsumed?.Invoke(_state, isTakenBeforeZoneEntry, blockRatio);
            return blockRatio;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PotassiumIodideState CaptureState() => _state;

        public void RestoreState(PotassiumIodideState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
