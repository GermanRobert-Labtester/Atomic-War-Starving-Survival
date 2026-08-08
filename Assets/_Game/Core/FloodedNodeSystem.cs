using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Flooded Ruins & Hypothermia (Prompt #69). Map nodes can have a Flooded
    /// modifier. Looting flooded subway stations requires Pumps or wading in.
    /// Wading drops Warmth to 0 instantly and destroys HazmatSuit durability.
    /// Massive Hypothermia risk for high-tier loot. Save/load safe. Plain C#.
    /// </summary>
    public class FloodedNodeSystem
    {
        public const string PumpItemId = "pump";
        public const string HypothermiaAfflictionId = "hypothermia";

        /// <summary>Warmth set to 0 when wading without a pump.</summary>
        public const float WadingWarmthSet = 0f;

        /// <summary>Hazmat suit durability destroyed per hour of wading.</summary>
        public const float WadingSuitDegradePerHour = 30f;

        /// <summary>Health crash per hour of severe hypothermia.</summary>
        public const float HypothermiaHealthDrainPerHour = 8f;

        /// <summary>Fatigue drain per hour of wading (cold water exhaustion).</summary>
        public const float WadingFatigueDrainPerHour = 10f;

        /// <summary>Loot quality multiplier for flooded nodes (high risk, high reward).</summary>
        public const float FloodedLootMultiplier = 2f;

        /// <summary>Node ids that are flooded.</summary>
        private readonly HashSet<string> _floodedNodeIds = new HashSet<string>();

        // -- Events --
        public event Action<Core.ExpeditionState> OnWadingStarted;
        public event Action<Core.ExpeditionState> OnHypothermiaTriggered;

        public IReadOnlyCollection<string> FloodedNodeIds => _floodedNodeIds;

        public FloodedNodeSystem() { }

        /// <summary>Mark a node as flooded (from proc-gen or save).</summary>
        public void SetFlooded(string nodeId, bool flooded)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            if (flooded) _floodedNodeIds.Add(nodeId);
            else _floodedNodeIds.Remove(nodeId);
        }

        public bool IsFlooded(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId) && _floodedNodeIds.Contains(nodeId);
        }

        /// <summary>
        /// Process a flooded node arrival. If the expedition has a Pump, they
        /// drain the area safely. Otherwise they wade — massive risk.
        /// Returns true if wading (danger path).
        /// </summary>
        public bool ProcessFloodedArrival(Core.ExpeditionState exp, Func<string, bool> hasItem)
        {
            if (exp == null) return false;
            if (!IsFlooded(exp.TargetLocationId)) return false;

            // Has a pump? Safe passage, no wading needed.
            if (hasItem != null && hasItem(PumpItemId))
            {
                exp.IsWading = false;
                return false;
            }

            // Wading in — catastrophic.
            exp.IsWading = true;
            if (exp.Survivor != null)
            {
                exp.Survivor.Needs.Warmth = WadingWarmthSet;
                exp.Survivor.Needs.Fatigue = Mathf.Clamp(
                    exp.Survivor.Needs.Fatigue + WadingFatigueDrainPerHour, 0f, 100f);
            }
            exp.SuitDegradation += WadingSuitDegradePerHour;
            OnWadingStarted?.Invoke(exp);
            return true;
        }

        /// <summary>
        /// Tick wading effects during an expedition. Health crash from hypothermia.
        /// </summary>
        public void TickWading(Core.ExpeditionState exp, float tickHours)
        {
            if (exp == null || !exp.IsWading || exp.Survivor == null) return;
            if (tickHours <= 0f) return;

            // Hypothermia health crash.
            SurvivorNeedWrite.AdjustHealth(exp.Survivor, -HypothermiaHealthDrainPerHour * tickHours);

            // Warmth stays at 0 while wading.
            exp.Survivor.Needs.Warmth = Mathf.Clamp(
                exp.Survivor.Needs.Warmth - WadingWarmthSet * 0.1f * tickHours, 0f, 100f);

            if (exp.Survivor.Needs.Warmth <= 0f && exp.Survivor.Needs.Health < 30f)
            {
                OnHypothermiaTriggered?.Invoke(exp);
            }
        }

        /// <summary>
        /// Get loot quality multiplier for this node (2x if flooded).
        /// </summary>
        public float GetLootMultiplier(string nodeId)
        {
            return IsFlooded(nodeId) ? FloodedLootMultiplier : 1f;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public FloodedNodeSave CaptureState()
        {
            var ids = new string[_floodedNodeIds.Count];
            _floodedNodeIds.CopyTo(ids);
            return new FloodedNodeSave { FloodedNodeIds = ids };
        }

        public void RestoreState(FloodedNodeSave save)
        {
            _floodedNodeIds.Clear();
            if (save?.FloodedNodeIds == null) return;
            for (int i = 0; i < save.FloodedNodeIds.Length; i++)
                if (!string.IsNullOrEmpty(save.FloodedNodeIds[i]))
                    _floodedNodeIds.Add(save.FloodedNodeIds[i]);
        }
    }

    [Serializable]
    public class FloodedNodeSave
    {
        public string[] FloodedNodeIds;
    }
}
