using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FlammableGasState
    {
        public string hazard_id = "map_hazard_flammable_gas";
        public bool is_present = true;
        public List<string> survivors_passed = new List<string>();
        public List<string> survivors_ignited = new List<string>();
    }

    /// <summary>DEMOTE-MapHazard-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public sealed class MapHazard_FlammableGas
    {
        private FlammableGasState _state;

        public event Action<string> OnSparkIgnited;     // survivor_id — instant death
        public event Action<string> OnSafePassage;       // survivor_id

        public string HazardId => _state.hazard_id;
        public bool IsPresent => _state.is_present;

        // Equipment types that produce sparks (unsafe)
        private static readonly HashSet<string> UnsafeEquipment = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "firearm",
            "flashlight"
        };

        // Equipment types that are safe to use
        private static readonly HashSet<string> SafeEquipment = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "melee",
            "glowstick"
        };

        public MapHazard_FlammableGas()
        {
            _state = new FlammableGasState();
        }

        /// <summary>
        /// Checks whether a single equipment type is safe in a gas-filled area.
        /// Returns true if safe, false if it would cause ignition.
        /// </summary>
        public bool CheckEquipment(string survivor_id, string equipment_type)
        {
            if (string.IsNullOrEmpty(equipment_type))
            {
                Debug.LogWarning("[MapHazard_FlammableGas] equipment_type is null or empty.");
                return true; // nothing equipped = safe
            }

            if (UnsafeEquipment.Contains(equipment_type))
            {
                Debug.Log($"[MapHazard_FlammableGas] Equipment '{equipment_type}' is UNSAFE in gas zone.");
                return false;
            }

            if (SafeEquipment.Contains(equipment_type))
            {
                return true;
            }

            // Unknown equipment type — treat as safe but log a warning
            Debug.LogWarning($"[MapHazard_FlammableGas] Unknown equipment type '{equipment_type}', assuming safe.");
            return true;
        }

        /// <summary>
        /// Navigate through a gas-filled node. If any equipped item produces
        /// a spark, the survivor ignites the gas (instant death).
        /// Returns true if the survivor passed safely, false if ignited.
        /// </summary>
        public bool NavigateNode(string survivor_id, List<string> equipped_items)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapHazard_FlammableGas] survivor_id is null or empty.");
                return false;
            }

            if (!_state.is_present)
            {
                // No gas hazard — safe by default
                OnSafePassage?.Invoke(survivor_id);
                return true;
            }

            // Check each equipped item for spark risk
            if (equipped_items != null)
            {
                for (int i = 0; i < equipped_items.Count; i++)
                {
                    if (!CheckEquipment(survivor_id, equipped_items[i]))
                    {
                        // Spark — ignition
                        if (!_state.survivors_ignited.Contains(survivor_id))
                        {
                            _state.survivors_ignited.Add(survivor_id);
                        }

                        OnSparkIgnited?.Invoke(survivor_id);
                        Debug.Log($"[MapHazard_FlammableGas] Survivor '{survivor_id}' ignited gas " +
                                  $"with '{equipped_items[i]}'. Fatal.");
                        return false;
                    }
                }
            }

            // All clear — safe passage
            if (!_state.survivors_passed.Contains(survivor_id))
            {
                _state.survivors_passed.Add(survivor_id);
            }

            OnSafePassage?.Invoke(survivor_id);
            Debug.Log($"[MapHazard_FlammableGas] Survivor '{survivor_id}' passed safely.");
            return true;
        }

        public FlammableGasState CaptureState()
        {
            return new FlammableGasState
            {
                hazard_id = _state.hazard_id,
                is_present = _state.is_present,
                survivors_passed = new List<string>(_state.survivors_passed),
                survivors_ignited = new List<string>(_state.survivors_ignited)
            };
        }

        public void RestoreState(FlammableGasState saved)
        {
            _state = saved ?? new FlammableGasState();
        }
    }
}
