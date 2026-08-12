using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Vehicle Maintenance System — repair, breakdown hazards, field
    /// modifications, and cargo management for Expansion 4 (#101-110).
    ///
    /// Extends existing VehicleSystem. Plain C#, save-safe.
    /// </summary>
    [Serializable]
    public class VehicleModification
    {
        public string ModId;
        public string DisplayName;
        public bool IsInstalled;
        public float ConditionPct = 1f;
    }

    [Serializable]
    public class VehicleMaintenanceState
    {
        public string VehicleId;
        public float ConditionPct = 1f;
        public float FuelLitres;
        public float MaxFuelCapacity = 60f;
        public float CargoCapacityKg = 200f;
        public float CurrentCargoKg;
        public float TravelSpeedMultiplier = 1f;
        public bool HasWinch;
        public bool HasArmoredRam;
        public bool HasSolarArray;
        public bool HasMedicalBay;
        public bool IsMobileCommandPost;
        public List<VehicleModification> Modifications = new List<VehicleModification>();
        public float BreakdownRisk;
        public int DaysSinceLastMaintenance;
    }

    public class VehicleMaintenanceSystem
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, float> OnVehicleConditionChanged;
        // vehicleId, newCondition
        public event Action<string> OnVehicleBreakdown;
        public event Action<string, string> OnModificationInstalled;
        // vehicleId, modId
        public event Action<string, float> OnFuelChanged;
        public event Action<string, float, float> OnCargoChanged;
        // vehicleId, currentKg, maxKg

        private readonly Dictionary<string, VehicleMaintenanceState> _vehicles =
            new Dictionary<string, VehicleMaintenanceState>();

        private System.Random _rng;

        public void SetRng(System.Random rng) => _rng = rng;

        public void RegisterVehicle(string vehicleId, string displayName,
            float maxFuel = 60f, float cargoCapacity = 200f,
            float speedMult = 1f)
        {
            if (_vehicles.ContainsKey(vehicleId)) return;
            _vehicles[vehicleId] = new VehicleMaintenanceState
            {
                VehicleId = vehicleId,
                MaxFuelCapacity = maxFuel,
                CargoCapacityKg = cargoCapacity,
                TravelSpeedMultiplier = speedMult
            };
        }

        public VehicleMaintenanceState GetVehicle(string vehicleId)
        {
            return _vehicles.TryGetValue(vehicleId, out var v) ? v : null;
        }

        /// <summary>Repair vehicle condition.</summary>
        public void RepairVehicle(string vehicleId, float repairAmount)
        {
            if (!_vehicles.TryGetValue(vehicleId, out var v)) return;
            v.ConditionPct = Mathf.Clamp01(v.ConditionPct + repairAmount);
            v.DaysSinceLastMaintenance = 0;
            OnVehicleConditionChanged?.Invoke(vehicleId, v.ConditionPct);
        }

        /// <summary>Consume fuel for travel.</summary>
        public float ConsumeFuel(string vehicleId, float litres)
        {
            if (!_vehicles.TryGetValue(vehicleId, out var v)) return 0f;
            float consumed = Mathf.Min(v.FuelLitres, litres);
            v.FuelLitres -= consumed;
            if (v.HasSolarArray)
                consumed *= 0.5f; // solar reduces consumption
            OnFuelChanged?.Invoke(vehicleId, v.FuelLitres);
            return consumed;
        }

        /// <summary>Refuel vehicle.</summary>
        public void Refuel(string vehicleId, float litres)
        {
            if (!_vehicles.TryGetValue(vehicleId, out var v)) return;
            v.FuelLitres = Mathf.Min(v.MaxFuelCapacity, v.FuelLitres + litres);
            OnFuelChanged?.Invoke(vehicleId, v.FuelLitres);
        }

        /// <summary>Load/unload cargo.</summary>
        public void AdjustCargo(string vehicleId, float kgDelta)
        {
            if (!_vehicles.TryGetValue(vehicleId, out var v)) return;
            v.CurrentCargoKg = Mathf.Clamp(v.CurrentCargoKg + kgDelta,
                0f, v.CargoCapacityKg);
            OnCargoChanged?.Invoke(vehicleId, v.CurrentCargoKg,
                v.CargoCapacityKg);
        }

        /// <summary>Install a modification.</summary>
        public bool InstallModification(string vehicleId, string modId)
        {
            if (!_vehicles.TryGetValue(vehicleId, out var v)) return false;

            var mod = new VehicleModification
            {
                ModId = modId,
                IsInstalled = true
            };

            switch (modId)
            {
                case "winch":
                    v.HasWinch = true;
                    mod.DisplayName = "Heavy Winch";
                    break;
                case "armored_ram":
                    v.HasArmoredRam = true;
                    mod.DisplayName = "Spiked Bumper";
                    break;
                case "solar_array":
                    v.HasSolarArray = true;
                    mod.DisplayName = "Solar-Electric Array";
                    break;
                case "medical_bay":
                    v.HasMedicalBay = true;
                    mod.DisplayName = "Mobile Field Clinic";
                    break;
                case "command_post":
                    v.IsMobileCommandPost = true;
                    mod.DisplayName = "Mobile Command Post";
                    break;
                case "cargo_trailer":
                    v.CargoCapacityKg += 500f;
                    mod.DisplayName = "Heavy Cargo Trailer";
                    break;
                default: return false;
            }

            v.Modifications.Add(mod);
            OnModificationInstalled?.Invoke(vehicleId, modId);
            return true;
        }

        /// <summary>Check for vehicle breakdown in dangerous zones.</summary>
        public bool CheckBreakdown(string vehicleId, float dangerLevel)
        {
            if (!_vehicles.TryGetValue(vehicleId, out var v)) return false;

            v.BreakdownRisk = (1f - v.ConditionPct) * dangerLevel *
                (1f + v.DaysSinceLastMaintenance * 0.1f);

            if ((_rng?.NextDouble() ?? 0.5) < v.BreakdownRisk)
            {
                v.ConditionPct = Mathf.Max(0.1f, v.ConditionPct - 0.3f);
                OnVehicleBreakdown?.Invoke(vehicleId);
                OnVehicleConditionChanged?.Invoke(vehicleId, v.ConditionPct);
                return true;
            }
            return false;
        }

        /// <summary>Tick — decay condition without maintenance.</summary>
        public void TickDaily(int currentDay)
        {
            foreach (var kv in _vehicles)
            {
                kv.Value.DaysSinceLastMaintenance++;
                if (kv.Value.DaysSinceLastMaintenance > 10)
                {
                    kv.Value.ConditionPct = Mathf.Max(0.1f,
                        kv.Value.ConditionPct - 0.01f);
                }
            }
        }
    }
}
