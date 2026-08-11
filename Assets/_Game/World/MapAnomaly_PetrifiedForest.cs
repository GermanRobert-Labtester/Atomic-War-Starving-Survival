using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class PetrifiedForestState
    {
        public string anomaly_id = "map_anomaly_petrified_forest";
        public string node_id = "";
        public bool is_discovered = false;
        public float carbon_scrap_per_tree = 3f;
        public float wood_yield = 0f;
        public bool audio_muted = false;
    }

    /// <summary>
    /// Prompt #849: Petrified Forest — Trees flash-fried by thermal radiation
    /// into stone. Zero wood; mining trees yields CarbonScrap. Eerie silence.
    /// </summary>
    public sealed class MapAnomaly_PetrifiedForest
    {
        private PetrifiedForestState _state;

        public event Action<string> OnDiscovered;                              // node_id
        public event Action<string, int, float> OnTreeHarvested;               // node_id, count, carbon_scrap
        public event Action OnWoodZero;
        public event Action OnAudioSilenced;

        public string AnomalyId => _state.anomaly_id;

        public MapAnomaly_PetrifiedForest()
        {
            _state = new PetrifiedForestState();
        }

        /// <summary>
        /// Marks the petrified forest as discovered at the given node.
        /// Drops audio to near-silence for the eerie atmosphere.
        /// </summary>
        public void Discover(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapAnomaly_PetrifiedForest] node_id is null or empty.");
                return;
            }

            _state.node_id = node_id;
            _state.is_discovered = true;
            _state.audio_muted = true;

            OnDiscovered?.Invoke(node_id);
            OnAudioSilenced?.Invoke();
            GameLog.Log($"[MapAnomaly_PetrifiedForest] Discovered at node '{node_id}'. Audio silenced.");
        }

        /// <summary>
        /// Harvests petrified trees. Each tree yields CarbonScrap (default 3).
        /// Wood yield is always 0 — the trees are stone.
        /// </summary>
        public void HarvestTree(int count)
        {
            if (count <= 0)
            {
                Debug.LogWarning("[MapAnomaly_PetrifiedForest] Harvest count must be positive.");
                return;
            }

            float carbon_scrap = count * _state.carbon_scrap_per_tree;

            OnTreeHarvested?.Invoke(_state.node_id, count, carbon_scrap);
            OnWoodZero?.Invoke();
            GameLog.Log($"[MapAnomaly_PetrifiedForest] Harvested {count} petrified trees — " +
                      $"{carbon_scrap:F1} CarbonScrap, 0 wood.");
        }

        /// <summary>
        /// Returns the carbon scrap yield per tree.
        /// </summary>
        public float GetCarbonYield() => _state.carbon_scrap_per_tree;

        /// <summary>
        /// Returns the wood yield — always 0 for petrified trees.
        /// </summary>
        public float GetWoodYield() => _state.wood_yield;

        /// <summary>
        /// Returns whether the audio mixer has been dropped to near-silence.
        /// </summary>
        public bool IsAudioMuted() => _state.audio_muted;

        public PetrifiedForestState CaptureState()
        {
            return new PetrifiedForestState
            {
                anomaly_id = _state.anomaly_id,
                node_id = _state.node_id,
                is_discovered = _state.is_discovered,
                carbon_scrap_per_tree = _state.carbon_scrap_per_tree,
                wood_yield = _state.wood_yield,
                audio_muted = _state.audio_muted
            };
        }

        public void RestoreState(PetrifiedForestState saved)
        {
            _state = saved ?? new PetrifiedForestState();
        }
    }
}
