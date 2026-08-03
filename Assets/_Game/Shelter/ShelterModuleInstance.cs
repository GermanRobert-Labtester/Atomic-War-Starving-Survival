using System;
using UnityEngine;
using AtomicWar._Game.Shelter.Modules;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Runtime state of an installed shelter module. Save/load safe.
    /// Supports add/remove at runtime without null refs.
    /// </summary>
    [Serializable]
    public class ShelterModuleInstance
    {
        public string ModuleId;
        public int Level = 1;
        public bool IsEnabled = true;
        public float FilterHealth = 100f;
        public float Fuel = 0f;
        public float WaterConversionProgress = 0f;

        /// <summary>Shelter room this module is installed in (e.g. "quarters", "plant").</summary>
        public string RoomId;

        /// <summary>Bed modules: current sleepers this evaluation wave (Prompt #32).</summary>
        public int Occupancy;

        /// <summary>Bed modules without a bound SO: comfort 0..1 fallback.</summary>
        public float ComfortLevel;

        /// <summary>Bed modules without a bound SO: capacity fallback (0 = use default 1).</summary>
        public int Capacity;

        /// <summary>Hatch defense modules without a bound SO: security points per level.</summary>
        public float SecurityContribution;

        [NonSerialized]
        private ShelterModule _definition;

        public ShelterModule Definition
        {
            get => _definition;
            set => _definition = value;
        }

        public ShelterModuleInstance() { }

        public ShelterModuleInstance(ShelterModule definition, int level = 1)
        {
            _definition = definition;
            ModuleId = definition != null ? definition.ModuleId : string.Empty;
            Level = level;
            FilterHealth = 100f;
            Fuel = 0f;
            IsEnabled = true;
        }

        public ShelterModuleInstance(string moduleId, int level = 1)
        {
            ModuleId = moduleId;
            Level = level;
            FilterHealth = 100f;
            Fuel = 0f;
            IsEnabled = true;
        }

        public bool IsOperational => IsEnabled && Level > 0;

        public void Tick(float gameHours, Shelter shelter)
        {
            if (!IsOperational || gameHours <= 0f) return;

            if (_definition is AirFiltrationModuleSO airSO)
            {
                FilterHealth = Mathf.Max(0f, FilterHealth - airSO.DegradationRatePerHour * gameHours);
            }
            else if (_definition is HeaterModuleSO heaterSO)
            {
                if (Fuel > 0f)
                {
                    Fuel = Mathf.Max(0f, Fuel - heaterSO.FuelConsumptionRatePerHour * gameHours);
                }
            }
            else if (_definition is GrowLightModuleSO growSO)
            {
                if (Fuel > 0f)
                {
                    Fuel = Mathf.Max(0f, Fuel - growSO.FuelConsumptionRatePerHour * gameHours);
                }
            }
            else if (_definition is RadioModuleSO radioSO)
            {
                if (Fuel > 0f)
                {
                    Fuel = Mathf.Max(0f, Fuel - radioSO.PowerConsumptionPerHour * gameHours);
                }
            }
            else
            {
                // Generic degradation fallback if ModuleId matches air_filtration without explicit SO instance
                if (ModuleId == "air_filtration")
                {
                    FilterHealth = Mathf.Max(0f, FilterHealth - 2f * gameHours);
                }
                else if (ModuleId == "heater" && Fuel > 0f)
                {
                    Fuel = Mathf.Max(0f, Fuel - 1f * gameHours);
                }
                else if (ModuleId == "grow_light" && Fuel > 0f)
                {
                    Fuel = Mathf.Max(0f, Fuel - 1.5f * gameHours);
                }
                else if (ModuleId == "radio" && Fuel > 0f)
                {
                    Fuel = Mathf.Max(0f, Fuel - 0.5f * gameHours);
                }
            }

        }

        public void ReplaceFilter()
        {
            FilterHealth = 100f;
        }

        public void AddFuel(float amount)
        {
            Fuel = Mathf.Max(0f, Fuel + amount);
        }
    }
}
