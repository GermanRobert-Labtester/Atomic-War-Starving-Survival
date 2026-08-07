using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TurretModuleState
    {
        public string moduleId = "shelter_module_turret";
        public string displayName = "Automated Scrap Turret";
        public bool isBuilt = false;
        public bool hasPower = true;
        public int ammoLoaded = 0;
        public float ammoConsumptionMultiplier = 3.0f; // Burns ammo 3x faster
        public float raidReductionValue = 50f;
    }

    /// <summary>
    /// Prompt #399: Module: Automated Scrap Turret.
    /// Placed in Airlock. Requires Power and Ammunition.
    /// Fires automatically during Raids, drastically reducing Raid strength while burning ammo 3x faster than a human.
    /// </summary>
    public class ShelterModule_Turret
    {
        private TurretModuleState _state = new TurretModuleState();

        public event Action<TurretModuleState, int, float> OnTurretFiredInRaid;

        public TurretModuleState State => _state;

        public float TriggerRaidDefense(ref int totalAmmunition, float baseRaidStrength)
        {
            if (!_state.isBuilt || !_state.hasPower || totalAmmunition <= 0)
                return baseRaidStrength;

            int ammoUsed = Mathf.Min(totalAmmunition, 30);
            totalAmmunition -= ammoUsed;
            _state.ammoLoaded = totalAmmunition;

            float reducedStrength = Mathf.Max(0f, baseRaidStrength - _state.raidReductionValue);
            OnTurretFiredInRaid?.Invoke(_state, ammoUsed, reducedStrength);

            return reducedStrength;
        }
    
        public TurretModuleState CaptureState()
        {
            return _state;
        }

        public void RestoreState(TurretModuleState saved)
        {
            _state = saved ?? new TurretModuleState();
        }
    }
}

