using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AmmoniaState
    {
        public string itemId = "item_ammonia";
        public string displayName = "Ammonia / Bleach";
        public bool isStoredInBox = true;
        public string toxicGasCloudId = "toxic_gas_combustion_cloud";
    }

    /// <summary>
    /// Prompt #434: Item: Ammonia / Bleach.
    /// Required for high-tier Hygiene cleaning and Fertilizer crafting.
    /// If a FireEntity impacts the storage box containing Ammonia, it causes toxic combustion (spawns a lethal ToxicGas cloud).
    /// </summary>
    public class Item_Ammonia
    {
        private AmmoniaState _state = new AmmoniaState();

        public event Action<AmmoniaState, string> OnToxicGasCloudSpawnedFromFire;

        public AmmoniaState State => _state;

        public bool TriggerFireImpactOnStorage()
        {
            if (_state.isStoredInBox)
            {
                OnToxicGasCloudSpawnedFromFire?.Invoke(_state, _state.toxicGasCloudId);
                return true;
            }
            return false;
        }
    }
}
