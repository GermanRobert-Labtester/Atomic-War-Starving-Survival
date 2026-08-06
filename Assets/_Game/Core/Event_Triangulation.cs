using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TriangulationState
    {
        public string eventId = "event_triangulation";
        public string displayName = "Radio Triangulation — Supply Drop";
        public bool isActive = false;
        public float signalDistanceKm = 0f;
        public bool triangulationComplete = false;
        public float supplyLootValue = 500f;
        public bool factionRaceActive = false;
        public string claimedByNodeId = string.Empty;
    }

    /// <summary>
    /// Prompt #634: Event: Triangulation.
    /// A military supply drop is announced via radio distance ping. The player
    /// must triangulate the exact node via intersecting movements before rival
    /// Factions arrive and claim it.
    /// </summary>
    public class Event_Triangulation
    {
        private TriangulationState _state = new TriangulationState();

        public event Action<TriangulationState, float> OnSignalReceived;
        public event Action<TriangulationState, bool, string> OnTriangulationAttempt;
        public event Action<TriangulationState> OnSupplyClaimed;

        public TriangulationState State => _state;

        public void ReceiveSignal(float distance)
        {
            _state.isActive = true;
            _state.signalDistanceKm = distance;
            _state.factionRaceActive = true;
            _state.triangulationComplete = false;
            _state.claimedByNodeId = string.Empty;
            OnSignalReceived?.Invoke(_state, distance);
        }

        public bool TryTriangulate(string nodeId1, string nodeId2, System.Random rng)
        {
            if (!_state.isActive || _state.triangulationComplete)
                return false;

            // Triangulation succeeds if two distinct nodes are provided and RNG favors the player
            if (string.IsNullOrEmpty(nodeId1) || string.IsNullOrEmpty(nodeId2) || nodeId1 == nodeId2)
            {
                OnTriangulationAttempt?.Invoke(_state, false, string.Empty);
                return false;
            }

            bool success = (float)rng.NextDouble() < 0.65f;
            if (success)
            {
                _state.triangulationComplete = true;
                _state.claimedByNodeId = nodeId1;
            }

            OnTriangulationAttempt?.Invoke(_state, success, success ? nodeId1 : string.Empty);
            return success;
        }

        public bool ClaimSupply()
        {
            if (!_state.triangulationComplete) return false;

            _state.isActive = false;
            _state.factionRaceActive = false;
            OnSupplyClaimed?.Invoke(_state);
            return true;
        }
    }
}
