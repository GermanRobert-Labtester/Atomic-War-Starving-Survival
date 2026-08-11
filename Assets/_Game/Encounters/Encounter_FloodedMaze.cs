using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class FloodedMazeState
    {
        public string encounter_id = "encounter_flooded_maze";
        public int total_rooms = 5;
        public int breath_turns = 4;
        public int current_room = -1;
        public int breath_remaining = 0;
        public bool is_drowning = false;
        public bool is_active = false;
        public bool has_surfaced = false;
        public string active_survivor_id;
        public List<string> collected_loot_ids = new List<string>();
        public List<bool> room_explored = new List<bool>();
    }

    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public sealed class Encounter_FloodedMaze
    {
        private FloodedMazeState _state;

        public event Action<string, int> OnRoomEntered;         // (survivor_id, room_index)
        public event Action<string, string> OnLootFound;        // (survivor_id, loot_id)
        public event Action<string> OnDrowningStarted;          // (survivor_id)
        public event Action<string> OnSurvivorDrowned;          // (survivor_id)

        public string EncounterId => _state.encounter_id;
        public bool IsActive => _state.is_active;
        public int CurrentRoom => _state.current_room;
        public int BreathRemaining => _state.breath_remaining;

        public Encounter_FloodedMaze()
        {
            _state = new FloodedMazeState();
        }

        /// <summary>
        /// Starts the flooded maze for a survivor with the given breath capacity.
        /// </summary>
        public void EnterMaze(string survivor_id, int breath_capacity)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Encounter_FloodedMaze] survivor_id is null or empty.");
                return;
            }

            _state.active_survivor_id = survivor_id;
            _state.current_room = -1;
            _state.breath_remaining = breath_capacity > 0 ? breath_capacity : _state.breath_turns;
            _state.is_drowning = false;
            _state.is_active = true;
            _state.has_surfaced = false;
            _state.collected_loot_ids.Clear();
            _state.room_explored.Clear();

            for (int i = 0; i < _state.total_rooms; i++)
            {
                _state.room_explored.Add(false);
            }

            GameLog.Log($"[Encounter_FloodedMaze] Survivor '{survivor_id}' entered the flooded maze. " +
                      $"Breath capacity: {_state.breath_remaining} turns.");
        }

        /// <summary>
        /// Explores the next room. Returns result type and whether drowning has started.
        /// Debris costs 1 extra turn. When breath runs out, drowning begins.
        /// </summary>
        public (string result, bool drowning) ExploreRoom(string survivor_id)
        {
            if (!_state.is_active || _state.has_surfaced)
            {
                Debug.LogWarning("[Encounter_FloodedMaze] Maze is not active.");
                return ("empty", false);
            }

            if (string.IsNullOrEmpty(survivor_id) || survivor_id != _state.active_survivor_id)
            {
                Debug.LogError("[Encounter_FloodedMaze] Invalid survivor_id.");
                return ("empty", false);
            }

            // Advance to next room
            _state.current_room++;

            if (_state.current_room >= _state.total_rooms)
            {
                GameLog.Log("[Encounter_FloodedMaze] No more rooms to explore.");
                return ("empty", _state.is_drowning);
            }

            // Consume 1 breath for entering the room
            _state.breath_remaining--;
            _state.room_explored[_state.current_room] = true;

            OnRoomEntered?.Invoke(survivor_id, _state.current_room);

            // Determine room content using deterministic seed per room
            var rng = new System.Random(
                (_state.encounter_id + survivor_id + _state.current_room).GetHashCode());
            int roll = rng.Next(3); // 0=loot, 1=debris, 2=empty

            string result;
            if (roll == 0)
            {
                // Loot room — free to grab
                string loot_id = $"loot_flooded_room_{_state.current_room}";
                _state.collected_loot_ids.Add(loot_id);
                OnLootFound?.Invoke(survivor_id, loot_id);
                result = "loot";
            }
            else if (roll == 1)
            {
                // Debris — costs 1 extra turn to clear
                _state.breath_remaining--;
                result = "debris";
            }
            else
            {
                result = "empty";
            }

            // Check drowning
            if (_state.breath_remaining <= 0 && !_state.is_drowning)
            {
                _state.is_drowning = true;
                _state.breath_remaining = 0;
                OnDrowningStarted?.Invoke(survivor_id);
                GameLog.Log($"[Encounter_FloodedMaze] Survivor '{survivor_id}' is drowning!");
            }

            return (result, _state.is_drowning);
        }

        /// <summary>
        /// Surface from the maze. If drowning, the survivor drowns.
        /// Otherwise, escape with collected loot.
        /// </summary>
        public void Surface(string survivor_id)
        {
            if (!_state.is_active)
            {
                Debug.LogWarning("[Encounter_FloodedMaze] Maze is not active.");
                return;
            }

            if (string.IsNullOrEmpty(survivor_id) || survivor_id != _state.active_survivor_id)
            {
                Debug.LogError("[Encounter_FloodedMaze] Invalid survivor_id.");
                return;
            }

            _state.has_surfaced = true;
            _state.is_active = false;

            if (_state.is_drowning)
            {
                OnSurvivorDrowned?.Invoke(survivor_id);
                GameLog.Log($"[Encounter_FloodedMaze] Survivor '{survivor_id}' drowned.");
            }
            else
            {
                GameLog.Log($"[Encounter_FloodedMaze] Survivor '{survivor_id}' surfaced with " +
                          $"{_state.collected_loot_ids.Count} loot item(s).");
            }
        }

        public FloodedMazeState CaptureState()
        {
            return new FloodedMazeState
            {
                encounter_id = _state.encounter_id,
                total_rooms = _state.total_rooms,
                breath_turns = _state.breath_turns,
                current_room = _state.current_room,
                breath_remaining = _state.breath_remaining,
                is_drowning = _state.is_drowning,
                is_active = _state.is_active,
                has_surfaced = _state.has_surfaced,
                active_survivor_id = _state.active_survivor_id,
                collected_loot_ids = new List<string>(_state.collected_loot_ids),
                room_explored = new List<bool>(_state.room_explored)
            };
        }

        public void RestoreState(FloodedMazeState saved)
        {
            _state = saved ?? new FloodedMazeState();
        }
    }
}
