using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class IsotopeTraceState
    {
        public string action_id = "action_isotope_trace";
        public bool requires_pristine_geiger = true;
        public bool reveals_safe_paths = true;
    }

    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public sealed class Action_IsotopeTrace
    {
        private IsotopeTraceState _state;

        public event Action<string, string> OnSafePathRevealed;
        public event Action<string> OnTracingFailed;

        public string ActionId => _state.action_id;

        public Action_IsotopeTrace()
        {
            _state = new IsotopeTraceState();
        }

        public List<string> TraceBiome(string survivor_id, string biome_id, bool has_pristine_geiger, int rad_level)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Action_IsotopeTrace] survivor_id is null or empty.");
                OnTracingFailed?.Invoke(survivor_id ?? "");
                return new List<string>();
            }

            if (string.IsNullOrEmpty(biome_id))
            {
                Debug.LogError("[Action_IsotopeTrace] biome_id is null or empty.");
                OnTracingFailed?.Invoke(survivor_id);
                return new List<string>();
            }

            if (!has_pristine_geiger)
            {
                OnTracingFailed?.Invoke(survivor_id);
                GameLog.Log($"[Action_IsotopeTrace] Failed for '{survivor_id}' — no pristine geiger counter.");
                return new List<string>();
            }

            if (!_state.reveals_safe_paths)
            {
                OnTracingFailed?.Invoke(survivor_id);
                GameLog.Log($"[Action_IsotopeTrace] Safe-path revelation is disabled.");
                return new List<string>();
            }

            List<string> safe_path_node_ids = GenerateSafePaths(biome_id, rad_level);

            if (safe_path_node_ids.Count > 0)
            {
                OnSafePathRevealed?.Invoke(survivor_id, biome_id);
                GameLog.Log($"[Action_IsotopeTrace] Revealed {safe_path_node_ids.Count} safe path nodes in biome '{biome_id}' for '{survivor_id}'.");
            }
            else
            {
                GameLog.Log($"[Action_IsotopeTrace] No safe paths found in biome '{biome_id}' at rad level {rad_level}.");
            }

            return safe_path_node_ids;
        }

        private static List<string> GenerateSafePaths(string biome_id, int rad_level)
        {
            var safe_nodes = new List<string>();

            if (rad_level < 5)
            {
                safe_nodes.Add($"{biome_id}_node_a");
                safe_nodes.Add($"{biome_id}_node_b");
            }
            else
            {
                safe_nodes.Add($"{biome_id}_node_safe_1");
            }

            return safe_nodes;
        }

        public IsotopeTraceState CaptureState()
        {
            return new IsotopeTraceState
            {
                action_id = _state.action_id,
                requires_pristine_geiger = _state.requires_pristine_geiger,
                reveals_safe_paths = _state.reveals_safe_paths
            };
        }

        public void RestoreState(IsotopeTraceState saved)
        {
            _state = saved ?? new IsotopeTraceState();
        }
    }
}
