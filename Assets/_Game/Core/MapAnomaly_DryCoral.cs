using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DryCoralState
    {
        public string anomaly_id = "map_anomaly_dry_coral";
        public string node_id = "";
        public float radiation_level = 10000f;
        public bool is_discovered = false;
        public int crystals_harvested = 0;
        public float rad_exposure_accumulated = 0f;
    }

    /// <summary>
    /// Prompt #854: Irradiated Coral — In Salt Flats. Glowing neon coral.
    /// Emits 10,000 mSv/hour. Harvesting yields RadiotrophicCrystals
    /// (endgame power source). HazmatSuit required; degrades fast.
    /// Unprotected = lethal dose in minutes.
    /// </summary>
    public sealed class MapAnomaly_DryCoral
    {
        private DryCoralState _state;

        private const float HazmatDegradationPerHarvest = 25f;
        private const float LethalDoseSv = 6f; // 6000 mSv ≈ LD100

        public event Action<string> OnDiscovered;                        // node_id
        public event Action<string> OnHarvestStarted;                    // survivor_id
        public event Action<string, float> OnRadExposure;                // survivor_id, mSv
        public event Action<string, int> OnCrystalHarvested;             // node_id, count
        public event Action<float> OnHazmatDegraded;                     // amount

        public string AnomalyId => _state.anomaly_id;

        public MapAnomaly_DryCoral()
        {
            _state = new DryCoralState();
        }

        /// <summary>
        /// Marks the irradiated coral anomaly as discovered at the given node.
        /// </summary>
        public void Discover(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapAnomaly_DryCoral] node_id is null or empty.");
                return;
            }

            _state.node_id = node_id;
            _state.is_discovered = true;

            OnDiscovered?.Invoke(node_id);
            GameLog.Log($"[MapAnomaly_DryCoral] Discovered at node '{node_id}'. " +
                      $"Radiation level: {_state.radiation_level:F0} mSv/hr.");
        }

        /// <summary>
        /// Harvests a RadiotrophicCrystal from the coral. Requires a hazmat
        /// suit, which degrades by 25% per harvest. Unprotected harvesting
        /// applies the full radiation dose.
        /// Returns true if the crystal was harvested successfully.
        /// </summary>
        public bool HarvestCrystal(string survivor_id, bool has_hazmat, float hazmat_level)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapAnomaly_DryCoral] survivor_id is null or empty.");
                return false;
            }

            OnHarvestStarted?.Invoke(survivor_id);

            if (has_hazmat)
            {
                // Hazmat degrades with each harvest
                OnHazmatDegraded?.Invoke(HazmatDegradationPerHarvest);
                GameLog.Log($"[MapAnomaly_DryCoral] Hazmat degraded by {HazmatDegradationPerHarvest:F0}%.");

                // Partial radiation leaks through degraded suit
                float leak_fraction = 1f - (hazmat_level / 100f);
                float exposure = _state.radiation_level * leak_fraction;
                _state.rad_exposure_accumulated += exposure;
                OnRadExposure?.Invoke(survivor_id, exposure);
            }
            else
            {
                // Full lethal dose
                float exposure = _state.radiation_level;
                _state.rad_exposure_accumulated += exposure;
                OnRadExposure?.Invoke(survivor_id, exposure);
                GameLog.Log($"[MapAnomaly_DryCoral] Survivor '{survivor_id}' received " +
                          $"{exposure:F0} mSv — lethal dose.");
            }

            _state.crystals_harvested++;
            OnCrystalHarvested?.Invoke(_state.node_id, _state.crystals_harvested);
            GameLog.Log($"[MapAnomaly_DryCoral] Crystal harvested (total: {_state.crystals_harvested}).");
            return true;
        }

        /// <summary>
        /// Returns the radiation exposure in mSv for the given hours spent
        /// near the coral.
        /// </summary>
        public float GetRadExposure(float hours_near)
        {
            return _state.radiation_level * Mathf.Max(0f, hours_near);
        }

        /// <summary>
        /// Returns the total number of RadiotrophicCrystals harvested.
        /// </summary>
        public int GetCrystalYield() => _state.crystals_harvested;

        public DryCoralState CaptureState()
        {
            return new DryCoralState
            {
                anomaly_id = _state.anomaly_id,
                node_id = _state.node_id,
                radiation_level = _state.radiation_level,
                is_discovered = _state.is_discovered,
                crystals_harvested = _state.crystals_harvested,
                rad_exposure_accumulated = _state.rad_exposure_accumulated
            };
        }

        public void RestoreState(DryCoralState saved)
        {
            _state = saved ?? new DryCoralState();
        }
    }
}
