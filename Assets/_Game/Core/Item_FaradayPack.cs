using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FaradayPackState
    {
        public string itemId = "item_faraday_pack";
        public string displayName = "Faraday Backpack";
        public bool isEMPShielded = true;
        public float maxCarryCapacityKg = 30f; // Reduced compared to standard 50kg
    }

    /// <summary>
    /// Prompt #426: Gear: Faraday Backpack.
    /// Copper mesh lined heavy backpack. Electronic items inside are 100% immune to EMPAftershocks on expeditions.
    /// Provides reduced max carry weight compared to a standard pack.
    /// </summary>
    public class Item_FaradayPack
    {
        private FaradayPackState _state = new FaradayPackState();

        public event Action<FaradayPackState> OnEMPSheldedProtectionTriggered;

        public FaradayPackState State => _state;

        public bool ProtectElectronicsFromEMP(bool isEMPStormActive)
        {
            if (isEMPStormActive)
            {
                OnEMPSheldedProtectionTriggered?.Invoke(_state);
                return true;
            }
            return false;
        }
    }
}
