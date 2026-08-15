using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class VenusTrapState
    {
        public string hazard_id = "map_hazard_venus_trap";
        public string node_id = "";
        public bool is_disguised = true;
        public int traps_active = 0;
        public int amputations = 0;
        public List<string> triggered_survivors = new List<string>();
    }

    /// <summary>
    /// Prompt #852: Carnivorous Plants — Massive mutated flora in Swamp nodes.
    /// Look like normal Berry bushes. Harvest = plant snaps shut. Strength check
    /// or lose arm (instant amputation). Perception >= 0.7 spots the disguise.
    /// REPROMOTE-MapHazard-001 — Boot/Save live; ExpeditionSystem calls EnterNode /
    /// AttemptHarvest when the target map node is tagged <c>swamp</c>.
    /// </summary>
    public sealed class MapHazard_VenusTrap
    {
        private VenusTrapState _state;

        private const float PerceptionThreshold = 0.7f;
        private const float StrengthEscapeThreshold = 0.6f;

        public event Action<string> OnNodeEntered;                       // node_id
        public event Action<string> OnTrapTriggered;                     // survivor_id
        public event Action<string> OnStrengthCheckPassed;               // survivor_id
        public event Action<string, string> OnArmLost;                   // survivor_id, arm_id
        public event Action<string> OnDisguiseSpotted;                   // survivor_id

        public string HazardId => _state.hazard_id;
        public string NodeId => _state.node_id;

        public MapHazard_VenusTrap()
        {
            _state = new VenusTrapState();
        }

        /// <summary>
        /// Initialises the node with 2–4 active traps disguised as berry bushes.
        /// </summary>
        public void EnterNode()
        {
            EnterNode(_state.node_id);
        }

        /// <summary>Bind the swamp node and arm disguised traps (seeded trap count).</summary>
        /// <remarks>
        /// Idempotent: arms only on the FIRST bind for a given node_id. If the
        /// hazard is already armed for this node (traps_active > 0), EnterNode
        /// is a no-op so repeated visit dispatches from the expedition host do
        /// not re-arm cleared traps. If a different node_id arrives, the state
        /// resets cleanly (no carryover traps between distinct swamp nodes).
        /// </remarks>
        public void EnterNode(string nodeId)
        {
            bool isNewNode = !string.Equals(_state.node_id, nodeId, StringComparison.Ordinal)
                              || string.IsNullOrEmpty(nodeId);
            if (!string.IsNullOrEmpty(nodeId) && !isNewNode && _state.traps_active > 0)
            {
                // Already armed for this node. Skip re-arm; the host will tick
                // attempts against the live state.
                return;
            }

            _state.node_id = nodeId ?? string.Empty;
            var rng = SeededRandom.Create(SeededRandom.WorldSeed, "venus_trap:" + (_state.node_id ?? "node"));
            _state.traps_active = 2 + rng.Next(3); // 2..4 inclusive
            _state.is_disguised = true;
            _state.triggered_survivors.Clear();

            OnNodeEntered?.Invoke(_state.node_id);
            GameLog.Log($"[MapHazard_VenusTrap] Entered node '{_state.node_id}' — " +
                      $"{_state.traps_active} traps active, disguised as berries.");
        }

        /// <summary>
        /// Attempts to harvest what appears to be berries. If the trap is
        /// disguised, the plant snaps shut. Strength >= 0.6 = escape.
        /// Strength &lt; 0.6 = instant arm amputation.
        /// </summary>
        /// <remarks>
        /// One trap is consumed per call (decremented on both escape and
        /// amputation outcomes). When the last trap fires, is_disguised is
        /// cleared so subsequent harvesters see the hazard as exposed. The
        /// Expedition host dispatches this per-looting-arrival; the per-call
        /// decrement is what stops the trap from re-arming on every visit.
        /// </remarks>
        public bool AttemptHarvest(string survivor_id, float strength)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapHazard_VenusTrap] survivor_id is null or empty.");
                return false;
            }

            if (_state.traps_active <= 0)
            {
                GameLog.Log($"[MapHazard_VenusTrap] No active traps in node '{_state.node_id}'.");
                return true;
            }

            OnTrapTriggered?.Invoke(survivor_id);

            bool survived;
            if (strength >= StrengthEscapeThreshold)
            {
                OnStrengthCheckPassed?.Invoke(survivor_id);
                GameLog.Log($"[MapHazard_VenusTrap] Survivor '{survivor_id}' escaped " +
                          $"(strength {strength:F2} >= {StrengthEscapeThreshold}).");
                survived = true;
            }
            else
            {
                // Arm amputation
                _state.amputations++;
                string arm_id = "arm_left"; // default; external systems may override
                if (!_state.triggered_survivors.Contains(survivor_id))
                {
                    _state.triggered_survivors.Add(survivor_id);
                }

                OnArmLost?.Invoke(survivor_id, arm_id);
                GameLog.Log($"[MapHazard_VenusTrap] Survivor '{survivor_id}' lost {arm_id}! " +
                          $"(strength {strength:F2} < {StrengthEscapeThreshold}).");
                survived = false;
            }

            // Consume one trap regardless of outcome. A surviving harvester still
            // pulled a trap and saw through the disguise on that one bush; the
            // remaining traps keep snapping at subsequent harvesters.
            _state.traps_active = Mathf.Max(0, _state.traps_active - 1);
            if (_state.traps_active == 0)
            {
                _state.is_disguised = false;
            }
            return survived;
        }

        /// <summary>
        /// Perception check to spot the disguise. Returns true if the
        /// survivor notices the traps (perception >= 0.7).
        /// </summary>
        public bool CheckDisguise(string survivor_id, float perception_skill)
        {
            if (perception_skill >= PerceptionThreshold && _state.is_disguised)
            {
                OnDisguiseSpotted?.Invoke(survivor_id);
                GameLog.Log($"[MapHazard_VenusTrap] Survivor '{survivor_id}' spotted the disguise " +
                          $"(perception {perception_skill:F2}).");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns whether the carnivorous plants are still disguised as berries.
        /// </summary>
        public bool IsDisguisedAsBerry() => _state.is_disguised;

        /// <summary>
        /// Returns the total number of amputations caused by this hazard.
        /// </summary>
        public int GetArmLossResult() => _state.amputations;

        public VenusTrapState CaptureState()
        {
            return new VenusTrapState
            {
                hazard_id = _state.hazard_id,
                node_id = _state.node_id,
                is_disguised = _state.is_disguised,
                traps_active = _state.traps_active,
                amputations = _state.amputations,
                triggered_survivors = new List<string>(_state.triggered_survivors)
            };
        }

        public void RestoreState(VenusTrapState saved)
        {
            _state = saved ?? new VenusTrapState();
        }
    }
}
