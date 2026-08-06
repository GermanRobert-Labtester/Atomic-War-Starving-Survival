using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class MimicCrateState
    {
        public string hazard_id = "hazard_mimic_crate";
        public float perception_threshold = 0.7f;
        public float explosion_damage = 80f;
        public List<string> detected_crate_ids = new List<string>();
        public List<string> exploded_survivor_ids = new List<string>();
        public List<string> destroyed_loot_crate_ids = new List<string>();
    }

    public sealed class Hazard_MimicCrate
    {
        private MimicCrateState _state;

        public event Action<string> OnWireDetected;       // survivor_id
        public event Action<string> OnCrateExploded;      // survivor_id
        public event Action<string> OnLootDestroyed;      // crate_id

        public string HazardId => _state.hazard_id;
        public float PerceptionThreshold => _state.perception_threshold;
        public float ExplosionDamage => _state.explosion_damage;

        public Hazard_MimicCrate()
        {
            _state = new MimicCrateState();
        }

        /// <summary>
        /// Inspect a crate to notice the tripwire. Returns true if the
        /// survivor's perception is high enough to spot the wire.
        /// </summary>
        public bool InspectCrate(string survivor_id, float perception)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Hazard_MimicCrate] survivor_id is null or empty.");
                return false;
            }

            bool detected = perception >= _state.perception_threshold;

            if (detected)
            {
                string crate_key = $"{survivor_id}_inspected";
                if (!_state.detected_crate_ids.Contains(crate_key))
                {
                    _state.detected_crate_ids.Add(crate_key);
                }

                OnWireDetected?.Invoke(survivor_id);
                Debug.Log($"[Hazard_MimicCrate] Survivor '{survivor_id}' detected the tripwire " +
                          $"(perception={perception:F2} >= {_state.perception_threshold:F2}).");
            }
            else
            {
                Debug.Log($"[Hazard_MimicCrate] Survivor '{survivor_id}' did NOT detect the tripwire " +
                          $"(perception={perception:F2} < {_state.perception_threshold:F2}).");
            }

            return detected;
        }

        /// <summary>
        /// Opens a mimic crate. If the survivor's perception is below threshold,
        /// the IED explodes — destroying loot and crippling the survivor's arms.
        /// Returns (survived, loot_intact).
        /// </summary>
        public (bool survived, bool loot_intact) OpenCrate(
            string survivor_id,
            float perception,
            System.Random rng)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Hazard_MimicCrate] survivor_id is null or empty.");
                return (false, false);
            }

            if (rng == null)
            {
                Debug.LogError("[Hazard_MimicCrate] rng is null.");
                return (false, false);
            }

            if (perception >= _state.perception_threshold)
            {
                // Survivor notices the wire, can disarm — safe open
                string crate_key = $"{survivor_id}_safe_open";
                if (!_state.detected_crate_ids.Contains(crate_key))
                {
                    _state.detected_crate_ids.Add(crate_key);
                }

                OnWireDetected?.Invoke(survivor_id);
                Debug.Log($"[Hazard_MimicCrate] Survivor '{survivor_id}' safely disarmed the mimic crate.");
                return (true, true);
            }

            // Below threshold — explosion
            string crate_id = $"crate_{survivor_id}_{rng.Next(10000)}";

            if (!_state.exploded_survivor_ids.Contains(survivor_id))
            {
                _state.exploded_survivor_ids.Add(survivor_id);
            }

            if (!_state.destroyed_loot_crate_ids.Contains(crate_id))
            {
                _state.destroyed_loot_crate_ids.Add(crate_id);
            }

            OnCrateExploded?.Invoke(survivor_id);
            OnLootDestroyed?.Invoke(crate_id);
            Debug.Log($"[Hazard_MimicCrate] Survivor '{survivor_id}' opened a mimic crate — " +
                      $"EXPLOSION ({_state.explosion_damage} damage). Loot destroyed, arms crippled.");

            // Survivor survives the blast but is badly hurt; loot is gone
            return (true, false);
        }

        public MimicCrateState CaptureState()
        {
            return new MimicCrateState
            {
                hazard_id = _state.hazard_id,
                perception_threshold = _state.perception_threshold,
                explosion_damage = _state.explosion_damage,
                detected_crate_ids = new List<string>(_state.detected_crate_ids),
                exploded_survivor_ids = new List<string>(_state.exploded_survivor_ids),
                destroyed_loot_crate_ids = new List<string>(_state.destroyed_loot_crate_ids)
            };
        }

        public void RestoreState(MimicCrateState saved)
        {
            _state = saved ?? new MimicCrateState();
        }
    }
}
