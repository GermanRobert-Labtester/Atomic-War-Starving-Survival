using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class EpiPenState
    {
        public string itemId = "item_epipen";
        public string displayName = "Adrenaline EpiPen";
        public float hoursUntilCrash = 4.0f;
        public float healthCrashMultiplier = 0.50f; // Halves health
        public float fatigueCrashValue = 100f;       // Locks fatigue at 100%
    }

    /// <summary>
    /// Prompt #430: Item: Adrenaline EpiPen.
    /// Instantly revives a survivor from Coma or Death's Door (#205).
    /// 4 hours after use, the survivor suffers a chemical crash (Fatigue locked at 100%, Health halved).
    /// </summary>
    public class Item_EpiPen
    {
        private EpiPenState _state = new EpiPenState();

        public event Action<EpiPenState, string> OnSurvivorRevivedFromDeathDoor;
        public event Action<EpiPenState, string> OnAdrenalineCrashTriggered;

        public EpiPenState State => _state;

        public bool ReviveSurvivor(string survivorId, ref bool isComaOrDeathsDoor)
        {
            if (isComaOrDeathsDoor)
            {
                isComaOrDeathsDoor = false;
                OnSurvivorRevivedFromDeathDoor?.Invoke(_state, survivorId);
                return true;
            }
            return false;
        }

        public void TriggerChemicalCrash(string survivorId, ref float health, ref float fatigue)
        {
            health *= _state.healthCrashMultiplier;
            fatigue = _state.fatigueCrashValue;

            OnAdrenalineCrashTriggered?.Invoke(_state, survivorId);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public EpiPenState CaptureState() => _state;

        public void RestoreState(EpiPenState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
