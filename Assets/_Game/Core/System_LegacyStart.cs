// System_LegacyStart.cs — Legacy Save System (Prompt #859)
// New game can choose "Legacy Save." Start in exact bunker from last death.
// It's ruined, flooded, filled with corpses/ghosts of previous crew.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Legacy Start system (Prompt #859).
    /// Tracks previous save data so a new playthrough begins in the ruins
    /// of the player's own failure.
    /// </summary>
    [Serializable]
    public class LegacyStartState
    {
        public string system_id = "system_legacy_start";
        public string previous_save_id = string.Empty;
        public bool bunker_ruined;
        public List<string> flooded_rooms = new List<string>();
        public List<CorpseLocation> corpse_locations = new List<CorpseLocation>();
        public float excavation_progress;
    }

    /// <summary>
    /// A corpse placed where a previous survivor died.
    /// </summary>
    [Serializable]
    public class CorpseLocation
    {
        public string survivor_id;
        public string room_id;

        public CorpseLocation() { }

        public CorpseLocation(string survivorId, string roomId)
        {
            survivor_id = survivorId;
            room_id = roomId;
        }
    }

    /// <summary>
    /// Legacy Start system (Prompt #859).
    /// Reads previous save data, marks all rooms as ruined, places corpses
    /// where they died, and requires the player to excavate room by room.
    /// Finding corpses triggers grief events.
    /// </summary>
    public class System_LegacyStart
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<string> OnLegacyLoaded;
        public event Action<string> OnRuinedRoomDiscovered;
        public event Action<string, string> OnCorpseFound;
        public event Action<string> OnRoomExcavated;
        public event Action<string> OnGhostEncountered;

        // ── State ──────────────────────────────────────────────────────
        private LegacyStartState _state = new LegacyStartState();
        private readonly HashSet<string> _excavatedRooms = new HashSet<string>();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Load legacy data from a previous save. Marks bunker as ruined,
        /// populates flooded rooms and corpse locations.
        /// </summary>
        public void LoadLegacy(string previousSaveId)
        {
            _state.previous_save_id = previousSaveId;
            _state.bunker_ruined = true;
            _state.excavation_progress = 0f;
            _excavatedRooms.Clear();

            OnLegacyLoaded?.Invoke(previousSaveId);

            // Fire discovery events for every ruined room
            for (int i = 0; i < _state.flooded_rooms.Count; i++)
            {
                OnRuinedRoomDiscovered?.Invoke(_state.flooded_rooms[i]);
            }

            // Fire corpse-found events
            for (int i = 0; i < _state.corpse_locations.Count; i++)
            {
                var c = _state.corpse_locations[i];
                OnCorpseFound?.Invoke(c.survivor_id, c.room_id);
            }
        }

        /// <summary>
        /// Returns true when a valid previous save exists and can be loaded.
        /// </summary>
        public bool CheckAvailability()
        {
            return !string.IsNullOrEmpty(_state.previous_save_id);
        }

        /// <summary>
        /// Returns the list of rooms that are ruined / flooded.
        /// </summary>
        public IReadOnlyList<string> GetRuinedRooms()
        {
            return _state.flooded_rooms.AsReadOnly();
        }

        /// <summary>
        /// Returns all corpse locations from the previous playthrough.
        /// </summary>
        public IReadOnlyList<CorpseLocation> GetCorpseLocations()
        {
            return _state.corpse_locations.AsReadOnly();
        }

        /// <summary>
        /// Excavate a single room. Triggers ghost encounter if a corpse is present.
        /// Player must clear rooms one by one.
        /// </summary>
        public void ExcavateRoom(string roomId)
        {
            if (_excavatedRooms.Contains(roomId))
                return;

            _excavatedRooms.Add(roomId);

            // Check for corpses in this room — triggers grief events
            for (int i = 0; i < _state.corpse_locations.Count; i++)
            {
                if (_state.corpse_locations[i].room_id == roomId)
                {
                    OnGhostEncountered?.Invoke(_state.corpse_locations[i].survivor_id);
                }
            }

            // Update excavation progress
            int totalRooms = _state.flooded_rooms.Count;
            if (totalRooms > 0)
            {
                _state.excavation_progress = (float)_excavatedRooms.Count / totalRooms;
            }

            OnRoomExcavated?.Invoke(roomId);
        }

        /// <summary>
        /// Returns overall excavation progress as a 0–1 ratio.
        /// </summary>
        public float GetExcavationProgress()
        {
            return _state.excavation_progress;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public LegacyStartState CaptureState()
        {
            return _state;
        }

        public void RestoreState(LegacyStartState state)
        {
            _state = state ?? new LegacyStartState();
        }
    }
}
