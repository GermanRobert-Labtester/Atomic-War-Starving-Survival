using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class SlaveMarketState
    {
        public string locationId = "location_slave_market";
        public string displayName = "The Slave Market";
        public int medicineCostPerSurvivor = 5;
        public List<string> permanentTraitsAdded = new List<string> { "Trauma", "Distrust" };
    }

    /// <summary>
    /// Prompt #414: Event: The Slave Market.
    /// Neutral trading node allowing players to purchase "Indentured Survivors" using Medicine.
    /// Purchased survivors join the bunker but carry permanent Trauma and Distrust traits.
    /// </summary>
    /// <summary>DEMOTE-Location-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Location_SlaveMarket
    {
        private SlaveMarketState _state = new SlaveMarketState();

        public event Action<SlaveMarketState, string> OnIndenturedSurvivorPurchased;

        public SlaveMarketState State => _state;

        public bool TryPurchaseSurvivor(ref int medicineStorage, string newSurvivorId)
        {
            if (medicineStorage >= _state.medicineCostPerSurvivor)
            {
                medicineStorage -= _state.medicineCostPerSurvivor;
                OnIndenturedSurvivorPurchased?.Invoke(_state, newSurvivorId);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SlaveMarketState CaptureState() => _state;

        public void RestoreState(SlaveMarketState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
