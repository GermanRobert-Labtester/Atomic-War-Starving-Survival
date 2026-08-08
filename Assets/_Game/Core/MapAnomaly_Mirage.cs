using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MirageState
    {
        public string anomaly_id = "map_anomaly_mirage";
        public bool is_active = false;
        public bool heatwave_required = true;
        public List<string> fake_nodes_spawned = new List<string>();
        public int players_deceived = 0;
        public float fuel_wasted = 0f;
        public float water_wasted = 0f;
        public float time_wasted_hours = 0f;
    }

    /// <summary>
    /// Prompt #858: Mirage Nodes — During heatwaves, the map UI spawns fake
    /// "Pristine Military Cache" nodes. Travelling there = node vanishes,
    /// wasting time (6h), fuel (10), and water (5). Only active during heatwaves.
    /// Spawns 1–3 fake nodes that look identical to real loot nodes.
    /// </summary>
    public sealed class MapAnomaly_Mirage
    {
        private MirageState _state;
        private readonly System.Random _rng;

        private const float FuelCostPerDeception = 10f;
        private const float WaterCostPerDeception = 5f;
        private const float TimeCostHoursPerDeception = 6f;

        public event Action OnHeatwaveStarted;
        public event Action<string, string> OnFakeNodeSpawned;           // node_id, display_name
        public event Action<string> OnPlayerDeceived;                   // node_id
        public event Action<float, float, float> OnResourcesWasted;     // fuel, water, time_hours
        public event Action<string> OnMirageDissolved;                  // node_id

        public string AnomalyId => _state.anomaly_id;

        public MapAnomaly_Mirage() : this(AtomicWar._Game.Utilities.SeededRandom.CreateFixed("mapanomaly_mirage")) { }

        public MapAnomaly_Mirage(System.Random rng)
        {
            _state = new MirageState();
            _rng = rng ?? AtomicWar._Game.Utilities.SeededRandom.CreateFixed("mapanomaly_mirage");
        }

        /// <summary>
        /// Activates the mirage anomaly during a heatwave. Spawns 1–3 fake
        /// loot nodes on the map UI. Does nothing if not a heatwave.
        /// </summary>
        public void SpawnDuringHeatwave(bool is_heatwave)
        {
            if (!is_heatwave)
            {
                Debug.Log("[MapAnomaly_Mirage] Not a heatwave — mirage not activated.");
                return;
            }

            _state.is_active = true;
            OnHeatwaveStarted?.Invoke();

            int count = _rng.Next(1, 4); // 1-3 fake nodes
            for (int i = 0; i < count; i++)
            {
                string fake_id = $"mirage_fake_node_{i + 1}";
                string display_name = "Pristine Military Cache";
                CreateFakeNode(fake_id, display_name);
            }

            Debug.Log($"[MapAnomaly_Mirage] Heatwave active — {count} fake node(s) spawned on map.");
        }

        /// <summary>
        /// Creates a single fake node on the map UI that looks like a real
        /// loot node.
        /// </summary>
        public void CreateFakeNode(string node_id, string display_name)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapAnomaly_Mirage] node_id is null or empty.");
                return;
            }

            if (!_state.fake_nodes_spawned.Contains(node_id))
            {
                _state.fake_nodes_spawned.Add(node_id);
            }

            OnFakeNodeSpawned?.Invoke(node_id, display_name);
            Debug.Log($"[MapAnomaly_Mirage] Fake node '{node_id}' spawned as '{display_name}'.");
        }

        /// <summary>
        /// Called when the player arrives at a fake node. The mirage dissolves,
        /// wasting fuel, water, and time. The node vanishes from the map.
        /// </summary>
        public void OnPlayerArrives(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapAnomaly_Mirage] node_id is null or empty.");
                return;
            }

            if (!_state.fake_nodes_spawned.Contains(node_id))
            {
                Debug.LogWarning($"[MapAnomaly_Mirage] Node '{node_id}' is not a fake mirage node.");
                return;
            }

            _state.players_deceived++;
            _state.fuel_wasted += FuelCostPerDeception;
            _state.water_wasted += WaterCostPerDeception;
            _state.time_wasted_hours += TimeCostHoursPerDeception;

            OnPlayerDeceived?.Invoke(node_id);
            OnResourcesWasted?.Invoke(FuelCostPerDeception, WaterCostPerDeception, TimeCostHoursPerDeception);
            OnMirageDissolved?.Invoke(node_id);

            _state.fake_nodes_spawned.Remove(node_id);
            Debug.Log($"[MapAnomaly_Mirage] Mirage at '{node_id}' dissolved. " +
                      $"Wasted: {FuelCostPerDeception} fuel, {WaterCostPerDeception} water, " +
                      $"{TimeCostHoursPerDeception}h time.");
        }

        /// <summary>
        /// Returns the total resources wasted on mirage deceptions.
        /// </summary>
        public (float fuel, float water, float time_hours) GetWastedResources()
        {
            return (_state.fuel_wasted, _state.water_wasted, _state.time_wasted_hours);
        }

        public MirageState CaptureState()
        {
            return new MirageState
            {
                anomaly_id = _state.anomaly_id,
                is_active = _state.is_active,
                heatwave_required = _state.heatwave_required,
                fake_nodes_spawned = new List<string>(_state.fake_nodes_spawned),
                players_deceived = _state.players_deceived,
                fuel_wasted = _state.fuel_wasted,
                water_wasted = _state.water_wasted,
                time_wasted_hours = _state.time_wasted_hours
            };
        }

        public void RestoreState(MirageState saved)
        {
            _state = saved ?? new MirageState();
        }
    }
}
