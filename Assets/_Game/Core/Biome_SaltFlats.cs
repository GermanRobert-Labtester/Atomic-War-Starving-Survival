using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SaltFlatsState
    {
        public string biome_id = "biome_salt_flats";
        public float temperature = 50f;
        public float thirst_drain_multiplier = 5f;
        public bool cover_available = false;
        public bool vehicle_required = true;
        public float salt_harvest_per_hour = 2f;
    }

    /// <summary>
    /// Prompt #853: Salt Flats — Oceans boiled away. Pure white salt for miles.
    /// Zero cover. 50°C. Thirst drains 5x speed. Only crossable with vehicles.
    /// Without a vehicle = instant death (too far to walk).
    /// </summary>
    public sealed class Biome_SaltFlats
    {
        private SaltFlatsState _state;

        public event Action<bool> OnBiomeEntered;                        // with_vehicle
        public event Action<float, float> OnThirstDrained;               // amount, multiplier
        public event Action<float> OnHeatExposure;                       // temperature
        public event Action OnVehicleBroken;
        public event Action<float> OnSaltHarvested;                      // amount

        public string BiomeId => _state.biome_id;

        public Biome_SaltFlats()
        {
            _state = new SaltFlatsState();
        }

        /// <summary>
        /// Enters the salt flats biome. Without a vehicle the survivor dies
        /// instantly — the flats are too vast to cross on foot.
        /// Returns true if the survivor entered safely (has vehicle).
        /// </summary>
        public bool EnterBiome(bool has_vehicle)
        {
            OnBiomeEntered?.Invoke(has_vehicle);
            OnHeatExposure?.Invoke(_state.temperature);

            if (!has_vehicle && _state.vehicle_required)
            {
                GameLog.Log("[Biome_SaltFlats] Survivor entered without vehicle — instant death.");
                return false;
            }

            GameLog.Log("[Biome_SaltFlats] Survivor entered with vehicle. Thirst drain 5x.");
            return true;
        }

        /// <summary>
        /// Ticks one in-game hour. Drains thirst at the multiplied rate.
        /// Returns the new thirst value after drain.
        /// </summary>
        public float TickHour(float current_thirst)
        {
            float drain = GetThirstDrain();
            float new_thirst = Mathf.Max(0f, current_thirst - drain);

            OnThirstDrained?.Invoke(drain, _state.thirst_drain_multiplier);
            OnHeatExposure?.Invoke(_state.temperature);

            GameLog.Log($"[Biome_SaltFlats] Thirst drained {drain:F1} (x{_state.thirst_drain_multiplier:F0}). " +
                      $"Thirst: {current_thirst:F1} → {new_thirst:F1}.");
            return new_thirst;
        }

        /// <summary>
        /// Returns the thirst drain per hour (base 1 * multiplier).
        /// </summary>
        public float GetThirstDrain()
        {
            return 1f * _state.thirst_drain_multiplier;
        }

        /// <summary>
        /// Returns the ambient temperature in Celsius.
        /// </summary>
        public float GetTemperature() => _state.temperature;

        /// <summary>
        /// Returns whether the biome can be crossed. Requires a vehicle.
        /// </summary>
        public bool CanCross(bool has_vehicle) => has_vehicle;

        /// <summary>
        /// Harvests salt over the given number of hours. Requires a vehicle
        /// to be present (enforced externally).
        /// </summary>
        public float HarvestSalt(float hours)
        {
            if (hours <= 0f)
                return 0f;

            float amount = _state.salt_harvest_per_hour * hours;
            OnSaltHarvested?.Invoke(amount);
            GameLog.Log($"[Biome_SaltFlats] Harvested {amount:F1} salt over {hours:F1} hours.");
            return amount;
        }

        public SaltFlatsState CaptureState()
        {
            return new SaltFlatsState
            {
                biome_id = _state.biome_id,
                temperature = _state.temperature,
                thirst_drain_multiplier = _state.thirst_drain_multiplier,
                cover_available = _state.cover_available,
                vehicle_required = _state.vehicle_required,
                salt_harvest_per_hour = _state.salt_harvest_per_hour
            };
        }

        public void RestoreState(SaltFlatsState saved)
        {
            _state = saved ?? new SaltFlatsState();
        }
    }
}
