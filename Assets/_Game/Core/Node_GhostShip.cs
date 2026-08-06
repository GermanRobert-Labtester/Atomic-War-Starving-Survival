using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GhostShipState
    {
        public string node_id = "node_ghost_ship";
        public bool is_discovered = false;
        public float fuel_remaining = float.MaxValue; // effectively infinite
        public float tetanus_chance = 0.15f;
        public List<string> rooms_explored = new List<string>();
        public int maze_depth = 0;
        public List<string> trapped_survivors = new List<string>();
        public float fuel_harvest_rate_per_hour = 10f;
    }

    /// <summary>
    /// Prompt #857: Ghost Ship — Oil tanker dropped 50 miles inland by tsunami.
    /// Infinite fuel, but hull is a labyrinth of rusted metal and Tetanus hazards.
    /// Each room explored has 15% Tetanus chance. Fuel is slow to harvest
    /// (10/hour). Maze layout can trap low-agility survivors.
    /// </summary>
    public sealed class Node_GhostShip
    {
        private GhostShipState _state;
        private readonly System.Random _rng;

        public event Action<string> OnDiscovered;                       // node_id
        public event Action<string, bool> OnRoomExplored;               // room_id, tetanus_triggered
        public event Action<float> OnFuelHarvested;                     // amount
        public event Action<string> OnTetanusApplied;                   // survivor_id
        public event Action<string, string> OnSurvivorTrapped;          // survivor_id, room_id

        public string NodeId => _state.node_id;

        public Node_GhostShip() : this(new System.Random()) { }

        public Node_GhostShip(System.Random rng)
        {
            _state = new GhostShipState();
            _rng = rng ?? new System.Random();
        }

        /// <summary>
        /// Marks the ghost ship as discovered at the given node.
        /// </summary>
        public void Discover(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[Node_GhostShip] node_id is null or empty.");
                return;
            }

            _state.node_id = node_id;
            _state.is_discovered = true;

            OnDiscovered?.Invoke(node_id);
            Debug.Log($"[Node_GhostShip] Discovered at node '{node_id}'. Infinite fuel available.");
        }

        /// <summary>
        /// Explores a room within the ghost ship hull. 15% chance of Tetanus.
        /// Low agility (below 0.4) may trap the survivor in the maze.
        /// Returns true if the room was explored safely.
        /// </summary>
        public bool ExploreRoom(string room_id, float agility_skill)
        {
            if (string.IsNullOrEmpty(room_id))
            {
                Debug.LogError("[Node_GhostShip] room_id is null or empty.");
                return false;
            }

            if (_state.rooms_explored.Contains(room_id))
            {
                Debug.LogWarning($"[Node_GhostShip] Room '{room_id}' already explored.");
                return true;
            }

            _state.rooms_explored.Add(room_id);
            _state.maze_depth++;

            // Tetanus check — 15% per room
            bool tetanus = (float)_rng.NextDouble() < _state.tetanus_chance;
            OnRoomExplored?.Invoke(room_id, tetanus);

            if (tetanus)
            {
                Debug.Log($"[Node_GhostShip] Tetanus triggered in room '{room_id}'.");
            }

            // Maze trap check — low agility survivors may get trapped
            if (agility_skill < 0.4f)
            {
                OnSurvivorTrapped?.Invoke("unknown", room_id);
                Debug.Log($"[Node_GhostShip] Survivor trapped in room '{room_id}' " +
                          $"(agility {agility_skill:F2} < 0.4).");
                return false;
            }

            Debug.Log($"[Node_GhostShip] Room '{room_id}' explored. Depth: {_state.maze_depth}.");
            return true;
        }

        /// <summary>
        /// Harvests fuel from the tanker. Rate: 10 fuel per in-game hour.
        /// Fuel is effectively infinite (float.MaxValue).
        /// </summary>
        public float HarvestFuel(float hours)
        {
            if (hours <= 0f)
                return 0f;

            float amount = _state.fuel_harvest_rate_per_hour * hours;
            // Fuel is effectively infinite — no depletion
            OnFuelHarvested?.Invoke(amount);
            Debug.Log($"[Node_GhostShip] Harvested {amount:F1} fuel over {hours:F1} hours.");
            return amount;
        }

        /// <summary>
        /// Checks whether a survivor contracts Tetanus from a room hazard.
        /// Fires the event if positive. Returns true if Tetanus applied.
        /// </summary>
        public bool CheckTetanus(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Node_GhostShip] survivor_id is null or empty.");
                return false;
            }

            bool contracted = (float)_rng.NextDouble() < _state.tetanus_chance;
            if (contracted)
            {
                OnTetanusApplied?.Invoke(survivor_id);
                Debug.Log($"[Node_GhostShip] Survivor '{survivor_id}' contracted Tetanus.");
            }

            return contracted;
        }

        /// <summary>
        /// Returns the number of rooms explored so far.
        /// </summary>
        public int GetRoomCount() => _state.rooms_explored.Count;

        public GhostShipState CaptureState()
        {
            return new GhostShipState
            {
                node_id = _state.node_id,
                is_discovered = _state.is_discovered,
                fuel_remaining = _state.fuel_remaining,
                tetanus_chance = _state.tetanus_chance,
                rooms_explored = new List<string>(_state.rooms_explored),
                maze_depth = _state.maze_depth,
                trapped_survivors = new List<string>(_state.trapped_survivors),
                fuel_harvest_rate_per_hour = _state.fuel_harvest_rate_per_hour
            };
        }

        public void RestoreState(GhostShipState saved)
        {
            _state = saved ?? new GhostShipState();
        }
    }
}
