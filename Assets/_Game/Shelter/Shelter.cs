using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Shelter.Modules;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// The bunker aggregate: manages installed upgradeable modules, exposes aggregate
    /// stats (IndoorRadLevel, IndoorTempBonus, AirQuality), and advances module logic.
    /// Save/load safe and null-reference safe on add/remove at runtime.
    /// </summary>
    [Serializable]
    public class Shelter
    {
        [SerializeField]
        private List<ShelterModuleInstance> _modules = new List<ShelterModuleInstance>();

        public IReadOnlyList<ShelterModuleInstance> Modules => _modules;

        public event Action<ShelterModuleInstance> OnModuleAdded;
        public event Action<string> OnModuleRemoved;
        public event Action<ShelterModuleInstance, int> OnModuleUpgraded;

        // Legacy compatibility properties
        public Shielding Shielding { get; private set; }
        public AirFiltration AirFiltration { get; private set; }

        public Shelter()
        {
            Shielding = new Shielding();
            AirFiltration = new AirFiltration();
            Shielding.BindShelter(this);
            AirFiltration.BindShelter(this);
        }

        public ShelterModuleInstance GetModule(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId) || _modules == null) return null;
            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i] != null && _modules[i].ModuleId == moduleId)
                {
                    return _modules[i];
                }
            }
            return null;
        }

        public ShelterModuleInstance GetModule<T>() where T : ShelterModule
        {
            if (_modules == null) return null;
            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i] != null && _modules[i].Definition is T)
                {
                    return _modules[i];
                }
            }
            return null;
        }

        public void AddModule(ShelterModuleInstance module)
        {
            if (module == null || string.IsNullOrEmpty(module.ModuleId)) return;
            var existing = GetModule(module.ModuleId);
            if (existing != null)
            {
                _modules.Remove(existing);
            }
            _modules.Add(module);
            OnModuleAdded?.Invoke(module);
        }

        public bool RemoveModule(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId) || _modules == null) return false;
            var existing = GetModule(moduleId);
            if (existing != null)
            {
                bool removed = _modules.Remove(existing);
                if (removed)
                {
                    OnModuleRemoved?.Invoke(moduleId);
                }
                return removed;
            }
            return false;
        }

        /// <summary>Indoor air quality index (0..100).</summary>
        public float AirQuality
        {
            get
            {
                var airModule = GetModule("air_filtration");
                if (airModule == null || !airModule.IsOperational)
                {
                    return 0f;
                }
                return Mathf.Clamp(airModule.FilterHealth, 0f, 100f);
            }
        }

        /// <summary>Interior warmth bonus (°C) output by heater modules.</summary>
        public float IndoorTempBonus
        {
            get
            {
                var heaterModule = GetModule("heater");
                if (heaterModule == null || !heaterModule.IsOperational)
                {
                    return 0f;
                }

                if (heaterModule.Definition is HeaterModuleSO heaterSO)
                {
                    if (heaterModule.Fuel <= 0f && heaterSO.FuelConsumptionRatePerHour > 0f)
                    {
                        return 0f;
                    }
                    return heaterModule.Level * heaterSO.HeatOutputPerLevel;
                }

                return heaterModule.Fuel > 0f ? heaterModule.Level * 5f : 0f;
            }
        }

        /// <summary>Indoor radiation level for a given exterior radiation dose rate.</summary>
        public float GetInteriorRadsPerHour(float exteriorRads)
        {
            float rads = exteriorRads;

            var shieldingModule = GetModule("radiation_shielding");
            if (shieldingModule != null && shieldingModule.IsOperational)
            {
                float attenuation = 0f;
                if (shieldingModule.Definition is RadiationShieldingModuleSO shieldSO)
                {
                    attenuation = shieldSO.GetAttenuationFraction(shieldingModule.Level);
                }
                else
                {
                    attenuation = Mathf.Clamp01(shieldingModule.Level * 0.15f);
                }
                rads = exteriorRads * (1f - attenuation);
            }

            var airModule = GetModule("air_filtration");
            if (airModule != null && airModule.IsOperational)
            {
                float lowThreshold = 25f;
                float leakRate = 5f;
                if (airModule.Definition is AirFiltrationModuleSO airSO)
                {
                    lowThreshold = airSO.LowHealthThreshold;
                    leakRate = airSO.RadLeakPerTickWhenDepleted;
                }

                if (airModule.FilterHealth <= lowThreshold)
                {
                    float depletionFactor = lowThreshold > 0f ? (lowThreshold - airModule.FilterHealth) / lowThreshold : 1f;
                    rads += leakRate * Mathf.Clamp01(depletionFactor);
                }
            }
            else
            {
                // Unfiltered air leak
                rads += 5f;
            }

            return Mathf.Max(0f, rads);
        }

        public void Tick(float gameHours)
        {
            if (gameHours <= 0f || _modules == null) return;

            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i] != null)
                {
                    _modules[i].Tick(gameHours, this);
                }
            }
        }

        public void NotifyModuleUpgraded(ShelterModuleInstance module, int newLevel)
        {
            OnModuleUpgraded?.Invoke(module, newLevel);
        }
    }
}
