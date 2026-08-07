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
        // Parallel arrays — JsonUtility-safe (Dictionary is not).
        public string[] survivorIdsInside = Array.Empty<string>();
        public float[] survivorHoursInside = Array.Empty<float>();
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
        private Dictionary<string, float> _hoursInside = new Dictionary<string, float>();

        public event Action<ServerFarmState, string> OnServerFarmEntered;
        public event Action<ServerFarmState, string> OnHeatstrokeContracted;
        public event Action<ServerFarmState, int> OnGoldHarvested;
        public event Action<ServerFarmState, string> OnBlizzardSheltered;

        public ServerFarmState State => _state;

        public void EnterShelter(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
                return;

            if (!_hoursInside.ContainsKey(survivorId))
                _hoursInside[survivorId] = 0f;

            OnServerFarmEntered?.Invoke(_state, survivorId);

            if (_state.blizzardProtection)
                OnBlizzardSheltered?.Invoke(_state, survivorId);
        }

        public bool TickHour(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(survivorId))
                return false;

            if (!_hoursInside.ContainsKey(survivorId))
                return false;

            _hoursInside[survivorId] += hours;

            if (_hoursInside[survivorId] >= _state.heatstrokeThresholdHours)
            {
                OnHeatstrokeContracted?.Invoke(_state, survivorId);
                return true;
            }

            return false;
        }

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

        public bool IsBlizzardSafe()
        {
            return _state.blizzardProtection;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public ServerFarmState CaptureState()
        {
            var ids = new string[_hoursInside.Count];
            var hours = new float[_hoursInside.Count];
            int i = 0;
            foreach (var kvp in _hoursInside)
            {
                ids[i] = kvp.Key;
                hours[i] = kvp.Value;
                i++;
            }

            return new ServerFarmState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                ambientTemperature = _state.ambientTemperature,
                blizzardProtection = _state.blizzardProtection,
                heatstrokeThresholdHours = _state.heatstrokeThresholdHours,
                goldPerMotherboard = _state.goldPerMotherboard,
                motherboardsAvailable = _state.motherboardsAvailable,
                survivorIdsInside = ids,
                survivorHoursInside = hours
            };
        }

        public void RestoreState(ServerFarmState saved)
        {
            if (saved == null)
            {
                _state = new ServerFarmState();
                _hoursInside = new Dictionary<string, float>();
                return;
            }

            _state = new ServerFarmState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                ambientTemperature = saved.ambientTemperature,
                blizzardProtection = saved.blizzardProtection,
                heatstrokeThresholdHours = saved.heatstrokeThresholdHours,
                goldPerMotherboard = saved.goldPerMotherboard,
                motherboardsAvailable = saved.motherboardsAvailable,
                survivorIdsInside = saved.survivorIdsInside ?? Array.Empty<string>(),
                survivorHoursInside = saved.survivorHoursInside ?? Array.Empty<float>()
            };

            _hoursInside = new Dictionary<string, float>();
            if (_state.survivorIdsInside != null && _state.survivorHoursInside != null)
            {
                int n = Math.Min(_state.survivorIdsInside.Length, _state.survivorHoursInside.Length);
                for (int i = 0; i < n; i++)
                {
                    if (!string.IsNullOrEmpty(_state.survivorIdsInside[i]))
                        _hoursInside[_state.survivorIdsInside[i]] = _state.survivorHoursInside[i];
                }
            }
        }
    }
}
