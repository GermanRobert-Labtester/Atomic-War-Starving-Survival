using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class GarageModuleState
    {
        public string moduleId = "shelter_module_garage";
        public string displayName = "Underground Garage";
        public bool isBuilt = false;
        public int vehicleStorageCapacity = 2;
        public List<string> storedVehicleIds = new List<string>();
        public bool protectsFromAcidSnowRust = true;
        public bool protectsFromRaiderTheft = true;
    }

    /// <summary>
    /// Prompt #386: Module: The Underground Garage.
    /// Connects a ramp to the surface, allowing vehicles to be parked safely inside the shelter.
    /// Protects stored vehicles from rusting in acid snow and theft by Raiders.
    /// </summary>
    public class ShelterModule_Garage
    {
        private GarageModuleState _state = new GarageModuleState();

        public event Action<GarageModuleState, string> OnVehicleParkedInGarage;
        public event Action<GarageModuleState, string> OnVehicleRetrievedFromGarage;

        public GarageModuleState State => _state;

        public bool ParkVehicle(string vehicleId)
        {
            if (!_state.isBuilt || _state.storedVehicleIds.Count >= _state.vehicleStorageCapacity)
                return false;

            if (!_state.storedVehicleIds.Contains(vehicleId))
            {
                _state.storedVehicleIds.Add(vehicleId);
                OnVehicleParkedInGarage?.Invoke(_state, vehicleId);
                return true;
            }
            return false;
        }

        public bool RetrieveVehicle(string vehicleId)
        {
            if (_state.storedVehicleIds.Remove(vehicleId))
            {
                OnVehicleRetrievedFromGarage?.Invoke(_state, vehicleId);
                return true;
            }
            return false;
        }
    
        public GarageModuleState CaptureState()
        {
            return _state;
        }

        public void RestoreState(GarageModuleState saved)
        {
            _state = saved ?? new GarageModuleState();
            if (_state.storedVehicleIds == null)
                _state.storedVehicleIds = new System.Collections.Generic.List<string>();
        }
    }
}

