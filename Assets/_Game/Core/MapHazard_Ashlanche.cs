using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AshlancheState
    {
        public string hazard_id = "map_hazard_ashlanche";
        public string node_id = "";
        public float noise_level = 0f;
        public float threshold = 0.7f;
        public bool avalanche_triggered = false;
        public List<string> buried_survivors = new List<string>();
        public float suffocation_timer_minutes = 10f;
        public float rad_exposure_mSv = 200f;
    }

    /// <summary>
    /// Prompt #855: Ash-lanches — Loud noises (gunfire, explosions) in
    /// mountain/skyscraper biomes trigger radioactive ash avalanches.
    /// Buried survivors must dig out (massive fatigue) or suffocate.
    /// Radioactive ash adds +200 mSv exposure.
    /// </summary>
    public sealed class MapHazard_Ashlanche
    {
        private AshlancheState _state;

        private const float DigFatigueCost = 60f;
        private const float SuffocationLimitMinutes = 10f;

        public event Action<float> OnNoiseDetected;                     // level
        public event Action<string> OnAvalancheTriggered;               // node_id
        public event Action<string[]> OnSurvivorsBuried;               // survivor_ids
        public event Action<string> OnDiggingStarted;                  // survivor_id
        public event Action<string> OnSuffocation;                     // survivor_id
        public event Action<string, float> OnDigComplete;              // survivor_id, fatigue_cost

        public string HazardId => _state.hazard_id;

        public MapHazard_Ashlanche()
        {
            _state = new AshlancheState();
        }

        /// <summary>
        /// Registers the hazard at a given node when the survivor enters.
        /// </summary>
        public void EnterNode(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapHazard_Ashlanche] node_id is null or empty.");
                return;
            }

            _state.node_id = node_id;
            GameLog.Log($"[MapHazard_Ashlanche] Entered node '{node_id}'. Noise threshold: {_state.threshold:F1}.");
        }

        /// <summary>
        /// Registers a noise event. If the noise level exceeds the threshold,
        /// an avalanche is triggered.
        /// </summary>
        public void MakeNoise(float noise_level)
        {
            _state.noise_level = noise_level;
            OnNoiseDetected?.Invoke(noise_level);

            if (noise_level >= _state.threshold)
            {
                CheckAvalanche();
            }
        }

        /// <summary>
        /// Triggers the avalanche if not already active.
        /// </summary>
        public void CheckAvalanche()
        {
            if (_state.avalanche_triggered)
                return;

            _state.avalanche_triggered = true;
            _state.suffocation_timer_minutes = SuffocationLimitMinutes;

            OnAvalancheTriggered?.Invoke(_state.node_id);
            GameLog.Log($"[MapHazard_Ashlanche] Avalanche triggered at node '{_state.node_id}'!");
        }

        /// <summary>
        /// Buries all survivors currently in the node when the avalanche hits.
        /// </summary>
        public void BurySurvivors(string[] survivor_ids)
        {
            if (survivor_ids == null || survivor_ids.Length == 0)
                return;

            _state.buried_survivors.Clear();
            _state.buried_survivors.AddRange(survivor_ids);

            OnSurvivorsBuried?.Invoke(survivor_ids);
            GameLog.Log($"[MapHazard_Ashlanche] {survivor_ids.Length} survivor(s) buried under radioactive ash " +
                      $"(+{_state.rad_exposure_mSv:F0} mSv).");
        }

        /// <summary>
        /// Attempts to dig out a buried survivor. Costs 60 Fatigue.
        /// Returns true if the survivor was freed.
        /// </summary>
        public bool DigOut(string survivor_id, float strength)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapHazard_Ashlanche] survivor_id is null or empty.");
                return false;
            }

            if (!_state.buried_survivors.Contains(survivor_id))
            {
                Debug.LogWarning($"[MapHazard_Ashlanche] Survivor '{survivor_id}' is not buried.");
                return false;
            }

            OnDiggingStarted?.Invoke(survivor_id);

            _state.buried_survivors.Remove(survivor_id);
            OnDigComplete?.Invoke(survivor_id, DigFatigueCost);
            GameLog.Log($"[MapHazard_Ashlanche] Survivor '{survivor_id}' dug out. Fatigue cost: {DigFatigueCost:F0}.");
            return true;
        }

        /// <summary>
        /// Ticks one in-game minute. Reduces the suffocation timer. If it
        /// expires, all remaining buried survivors suffocate.
        /// </summary>
        public void TickMinute()
        {
            if (!_state.avalanche_triggered || _state.buried_survivors.Count == 0)
                return;

            _state.suffocation_timer_minutes -= 1f;

            if (_state.suffocation_timer_minutes <= 0f)
            {
                // All remaining buried survivors suffocate
                for (int i = _state.buried_survivors.Count - 1; i >= 0; i--)
                {
                    string survivor_id = _state.buried_survivors[i];
                    OnSuffocation?.Invoke(survivor_id);
                    GameLog.Log($"[MapHazard_Ashlanche] Survivor '{survivor_id}' suffocated.");
                }

                _state.buried_survivors.Clear();
                _state.avalanche_triggered = false;
            }
        }

        public AshlancheState CaptureState()
        {
            return new AshlancheState
            {
                hazard_id = _state.hazard_id,
                node_id = _state.node_id,
                noise_level = _state.noise_level,
                threshold = _state.threshold,
                avalanche_triggered = _state.avalanche_triggered,
                buried_survivors = new List<string>(_state.buried_survivors),
                suffocation_timer_minutes = _state.suffocation_timer_minutes,
                rad_exposure_mSv = _state.rad_exposure_mSv
            };
        }

        public void RestoreState(AshlancheState saved)
        {
            _state = saved ?? new AshlancheState();
        }
    }
}
