using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class DustDevilState
    {
        public string weather_id = "weather_dust_devil";
        public bool is_roaming = true;
        public bool damages_air_filter = true;
        public bool clogs_wind_turbine = true;
        public string current_node_id = "";
    }

    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public sealed class Weather_DustDevil
    {
        private DustDevilState _state;
        private readonly List<string> _adjacent_node_pool;

        public event Action<string> OnAirFilterRuined;
        public event Action<string> OnWindTurbineClogged;
        public event Action<string, string> OnDustDevilSpotted;

        public string WeatherId => _state.weather_id;
        public string CurrentNodeId => _state.current_node_id;

        public Weather_DustDevil(List<string> adjacent_node_pool = null)
        {
            _state = new DustDevilState();
            _adjacent_node_pool = adjacent_node_pool ?? new List<string>();
        }

        public void Tick(string current_node_id, string bunker_node_id, System.Random rng)
        {
            if (rng == null)
            {
                Debug.LogError("[Weather_DustDevil] rng is null.");
                return;
            }

            if (!_state.is_roaming)
                return;

            string next_node = MoveDevil(_state.current_node_id, rng);
            _state.current_node_id = next_node;

            OnDustDevilSpotted?.Invoke(next_node, GetRandomDirection(rng));

            if (!string.IsNullOrEmpty(bunker_node_id) &&
                string.Equals(next_node, bunker_node_id, StringComparison.Ordinal))
            {
                if (_state.damages_air_filter)
                {
                    OnAirFilterRuined?.Invoke(bunker_node_id);
                    GameLog.Log($"[Weather_DustDevil] Air filter ruined at bunker '{bunker_node_id}'.");
                }

                if (_state.clogs_wind_turbine)
                {
                    OnWindTurbineClogged?.Invoke(bunker_node_id);
                    GameLog.Log($"[Weather_DustDevil] Wind turbine clogged at bunker '{bunker_node_id}'.");
                }
            }
        }

        public string MoveDevil(string current_devil_node, System.Random rng)
        {
            if (rng == null)
            {
                Debug.LogError("[Weather_DustDevil] rng is null in MoveDevil.");
                return current_devil_node;
            }

            if (_adjacent_node_pool.Count == 0)
            {
                Debug.LogWarning("[Weather_DustDevil] No adjacent nodes in pool; devil stays in place.");
                return current_devil_node;
            }

            int idx = rng.Next(0, _adjacent_node_pool.Count);
            return _adjacent_node_pool[idx];
        }

        private static string GetRandomDirection(System.Random rng)
        {
            string[] directions = { "north", "south", "east", "west", "northeast", "northwest", "southeast", "southwest" };
            if (rng == null)
                return "north";
            return directions[rng.Next(0, directions.Length)];
        }

        public DustDevilState CaptureState()
        {
            return new DustDevilState
            {
                weather_id = _state.weather_id,
                is_roaming = _state.is_roaming,
                damages_air_filter = _state.damages_air_filter,
                clogs_wind_turbine = _state.clogs_wind_turbine,
                current_node_id = _state.current_node_id
            };
        }

        public void RestoreState(DustDevilState saved)
        {
            _state = saved ?? new DustDevilState();
        }
    }
}
