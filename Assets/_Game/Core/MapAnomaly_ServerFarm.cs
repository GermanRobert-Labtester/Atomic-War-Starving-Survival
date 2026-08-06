using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ServerFarmState
    {
        public string anomalyId = "map_anomaly_server_farm";
        public string displayName = "The Server Farm";
        public float ambientTemperature = 60f;
        public bool blizzardProtection = true;
        public float heatstrokeThresholdHours = 6f;
        public int goldPerMotherboard = 3;
        public int motherboardsAvailable = 10;
        public Dictionary<string, float> survivorHoursInside = new Dictionary<string, float>();
    }

    /// <summary>
    /// Prompt #616: Map Anomaly: The Server Farm.
    /// A subterranean data center where servers generate 60°C heat. Provides haven
    /// from Blizzards but staying too long causes Heatstroke. Gold can be harvested
    /// from motherboards.
    /// </summary>
    public class MapAnomaly_ServerFarm
    {
        private ServerFarmState _state = new ServerFarmState();

        public event Action<ServerFarmState, string> OnServerFarmEntered;
        public event Action<ServerFarmState, string> OnHeatstrokeContracted;
        public event Action<ServerFarmState, int> OnGoldHarvested;
        public event Action<ServerFarmState, string> OnBlizzardSheltered;

        public ServerFarmState State => _state;

        /// <summary>
        /// A survivor enters the server farm shelter.
        /// </summary>
        /// <param name="survivorId">The survivor's unique identifier.</param>
        public void EnterShelter(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return;

            if (!_state.survivorHoursInside.ContainsKey(survivorId))
            {
                _state.survivorHoursInside[survivorId] = 0f;
            }

            OnServerFarmEntered?.Invoke(_state, survivorId);

            if (_state.blizzardProtection)
            {
                OnBlizzardSheltered?.Invoke(_state, survivorId);
            }
        }

        /// <summary>
        /// Advances the heatstroke timer for a survivor inside the server farm.
        /// If cumulative hours exceed the threshold, heatstroke is contracted.
        /// </summary>
        /// <param name="survivorId">The survivor's unique identifier.</param>
        /// <param name="hours">Number of hours to advance.</param>
        /// <returns>True if heatstroke was contracted.</returns>
        public bool TickHour(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(survivorId))
                return false;

            if (!_state.survivorHoursInside.ContainsKey(survivorId))
                return false;

            _state.survivorHoursInside[survivorId] += hours;

            if (_state.survivorHoursInside[survivorId] >= _state.heatstrokeThresholdHours)
            {
                OnHeatstrokeContracted?.Invoke(_state, survivorId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to harvest gold from motherboards.
        /// </summary>
        /// <param name="motherboards">Number of motherboards to harvest.</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>Total gold harvested.</returns>
        public int TryHarvestGold(int motherboards, System.Random rng)
        {
            if (motherboards <= 0 || rng == null)
                return 0;

            int boardsToHarvest = Math.Min(motherboards, _state.motherboardsAvailable);
            _state.motherboardsAvailable -= boardsToHarvest;

            int totalGold = boardsToHarvest * _state.goldPerMotherboard;

            OnGoldHarvested?.Invoke(_state, totalGold);
            return totalGold;
        }

        /// <summary>
        /// Returns whether the server farm provides blizzard protection.
        /// </summary>
        public bool IsBlizzardSafe()
        {
            return _state.blizzardProtection;
        }
    }
}
