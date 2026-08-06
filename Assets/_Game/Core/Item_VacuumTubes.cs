using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class VacuumTubesState
    {
        public string itemId = "item_vacuum_tubes";
        public string displayName = "Vacuum Tubes";
        public bool isEMPProof = true;
        public bool isFragile = true;
        public bool isIntact = true;
        public bool requiredForHamRadio = true;
    }

    /// <summary>
    /// Prompt #615: Item: Vacuum Tubes.
    /// EMP-proof electronics required to repair HamRadio.
    /// Highly fragile — shatters from FallDamage or ExplosiveTrauma.
    /// </summary>
    public class Item_VacuumTubes
    {
        private VacuumTubesState _state = new VacuumTubesState();

        public event Action<VacuumTubesState, string> OnTubesShattered;
        public event Action<VacuumTubesState> OnHamRadioRepaired;

        public VacuumTubesState State => _state;

        public bool CheckBreakage(string damageType)
        {
            if (!_state.isIntact)
                return false;

            if (damageType == "fall_damage" || damageType == "explosive_trauma")
            {
                _state.isIntact = false;
                OnTubesShattered?.Invoke(_state, damageType);
                return true;
            }

            return false;
        }

        public bool IsUsable()
        {
            return _state.isIntact;
        }

        public bool CanRepairHamRadio()
        {
            return _state.isIntact && _state.requiredForHamRadio;
        }
    }
}
