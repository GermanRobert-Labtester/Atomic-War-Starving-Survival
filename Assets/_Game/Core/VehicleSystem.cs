using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class VehicleComponent
    {
        public string componentName; // Engine, Tires, Chassis, Battery
        public float maxDurability = 100f;
        public float currentDurability = 100f;
    }

    [Serializable]
    public class VehicleData
    {
        public string vehicleId;
        public string displayName;
        public float speedMultiplier = 2.0f;
        public float encumbranceCapacity = 200f;
        public float fuelConsumptionPerKm = 0.5f;
        public List<VehicleComponent> components = new List<VehicleComponent>();
    }

    /// <summary>
    /// Prompt #379: System: Vehicle Chassis & Modularity.
    /// Vehicles consist of Engine, Tires, Chassis, and Battery. Repaired individually.
    /// If Tire Durability drops to 0, travel speed drops to walking pace.
    /// </summary>
    
    [Serializable]
    public class VehicleSystemSave
    {
        public string systemId = "vehicle_system";

        public List<VehicleData> vehicles = new List<VehicleData>();
    }
public class VehicleSystem
    {
        private readonly Dictionary<string, VehicleData> _vehicles = new Dictionary<string, VehicleData>();

        public event Action<string, string, float> OnComponentDurabilityChanged;
        public event Action<string> OnTiresBlownWalkingPace;

        public IReadOnlyDictionary<string, VehicleData> Vehicles => _vehicles;

        public VehicleData CreateVehicle(string vehicleId, string displayName, float speedMult, float capacity)
        {
            var vehicle = new VehicleData
            {
                vehicleId = vehicleId,
                displayName = displayName,
                speedMultiplier = speedMult,
                encumbranceCapacity = capacity
            };
            vehicle.components.Add(new VehicleComponent { componentName = "Engine" });
            vehicle.components.Add(new VehicleComponent { componentName = "Tires" });
            vehicle.components.Add(new VehicleComponent { componentName = "Chassis" });
            vehicle.components.Add(new VehicleComponent { componentName = "Battery" });

            _vehicles[vehicleId] = vehicle;
            return vehicle;
        }

        public float GetEffectiveSpeedMultiplier(string vehicleId)
        {
            if (!_vehicles.TryGetValue(vehicleId, out var vehicle)) return 1.0f;

            var tires = vehicle.components.Find(c => c.componentName == "Tires");
            if (tires != null && tires.currentDurability <= 0f)
            {
                OnTiresBlownWalkingPace?.Invoke(vehicleId);
                return 1.0f; // Walking pace
            }
            return vehicle.speedMultiplier;
        }

        public void DamageComponent(string vehicleId, string componentName, float damage)
        {
            if (_vehicles.TryGetValue(vehicleId, out var vehicle))
            {
                var comp = vehicle.components.Find(c => c.componentName == componentName);
                if (comp != null)
                {
                    comp.currentDurability = Mathf.Max(0f, comp.currentDurability - damage);
                    OnComponentDurabilityChanged?.Invoke(vehicleId, componentName, comp.currentDurability);
                }
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public VehicleSystemSave CaptureState() => new VehicleSystemSave
        {
            vehicles = SaveMap.Capture(_vehicles),
        };

        public void RestoreState(VehicleSystemSave saved) =>
            SaveMap.Restore(_vehicles, saved?.vehicles, e => e.vehicleId);

}
}
