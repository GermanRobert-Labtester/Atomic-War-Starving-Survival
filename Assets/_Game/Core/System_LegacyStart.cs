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
    /// Tracks previous wipe data so a new playthrough can begin in the ruins
    /// of the player's own failure.
    /// </summary>
    [Serializable]
    public class LegacyStartState
    {
        public string system_id = "system_legacy_start";
        public string previous_save_id = string.Empty;
        public bool bunker_ruined;
        /// <summary>True after <see cref="System_LegacyStart.BeginLegacyRun"/> has been applied this run.</summary>
        public bool legacy_run_active;
        public List<string> flooded_rooms = new List<string>();
        public List<CorpseLocation> corpse_locations = new List<CorpseLocation>();
        public List<string> remaining_loot_ids = new List<string>();
        public List<string> excavated_rooms = new List<string>();
        public float excavation_progress;
        public int prior_day_of_death;
        public string cause_of_death = string.Empty;
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
    /// Seeds from Last Will / prior wipe data, marks rooms as ruined, places
    /// corpses, and requires room-by-room excavation. Finding corpses triggers
    /// grief / ghost events. Remaining loot is a small starting bonus.
    /// </summary>
    public class System_LegacyStart
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<string> OnLegacyLoaded;                 // previousSaveId
        public event Action<string> OnRuinedRoomDiscovered;         // roomId
        public event Action<string, string> OnCorpseFound;          // survivorId, roomId
        public event Action<string> OnRoomExcavated;                // roomId
        public event Action<string> OnGhostEncountered;             // survivorId
        public event Action<IReadOnlyList<string>> OnLegacyLootGranted; // loot item ids

        // ── State ──────────────────────────────────────────────────────
        private LegacyStartState _state = new LegacyStartState();
        private readonly HashSet<string> _excavatedRooms = new HashSet<string>(StringComparer.Ordinal);

        // ── Public API ─────────────────────────────────────────────────

        public bool IsPrepared =>
            !string.IsNullOrEmpty(_state.previous_save_id)
            || (_state.corpse_locations != null && _state.corpse_locations.Count > 0)
            || (_state.flooded_rooms != null && _state.flooded_rooms.Count > 0);

        public bool IsLegacyRunActive => _state.legacy_run_active;
        public bool IsBunkerRuined => _state.bunker_ruined;
        public string PreviousSaveId => _state.previous_save_id ?? string.Empty;
        public int PriorDayOfDeath => _state.prior_day_of_death;
        public string CauseOfDeath => _state.cause_of_death ?? string.Empty;

        /// <summary>
        /// Seed legacy data from a Last Will grave without activating the run.
        /// Safe to call after a wipe or when a prior grave is available at new-game.
        /// </summary>
        public void PrepareFromGrave(
            GraveSiteData grave,
            IList<string> roomIds,
            string previousSaveId = null)
        {
            if (grave == null) return;

            _state.previous_save_id = !string.IsNullOrEmpty(previousSaveId)
                ? previousSaveId
                : (grave.locationId ?? "legacy_prior");
            _state.bunker_ruined = false;
            _state.legacy_run_active = false;
            _state.excavation_progress = 0f;
            _state.prior_day_of_death = grave.dayOfDeath;
            _state.cause_of_death = grave.causeOfDeath ?? string.Empty;
            _excavatedRooms.Clear();
            _state.excavated_rooms = new List<string>();

            // Flooded / ruined rooms from current bunker layout (or defaults).
            _state.flooded_rooms = new List<string>();
            if (roomIds != null)
            {
                for (int i = 0; i < roomIds.Count; i++)
                {
                    if (string.IsNullOrEmpty(roomIds[i])) continue;
                    if (!_state.flooded_rooms.Contains(roomIds[i]))
                        _state.flooded_rooms.Add(roomIds[i]);
                }
            }
            if (_state.flooded_rooms.Count == 0)
            {
                // Fallback layout ids used by bootstrap default bunker.
                _state.flooded_rooms.Add("quarters");
                _state.flooded_rooms.Add("stores");
                _state.flooded_rooms.Add("entry");
                _state.flooded_rooms.Add("plant");
            }

            // Place corpses: cycle names through rooms.
            _state.corpse_locations = new List<CorpseLocation>();
            var names = grave.deadSurvivorNames;
            if (names != null)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    if (string.IsNullOrEmpty(names[i])) continue;
                    string room = _state.flooded_rooms[i % _state.flooded_rooms.Count];
                    _state.corpse_locations.Add(new CorpseLocation(names[i], room));
                }
            }

            _state.remaining_loot_ids = new List<string>();
            if (grave.remainingLootIds != null)
            {
                for (int i = 0; i < grave.remainingLootIds.Count; i++)
                {
                    if (string.IsNullOrEmpty(grave.remainingLootIds[i])) continue;
                    if (!_state.remaining_loot_ids.Contains(grave.remainingLootIds[i]))
                        _state.remaining_loot_ids.Add(grave.remainingLootIds[i]);
                }
            }
        }

        /// <summary>
        /// Convenience: prepare from a LastWillSystem grave when present.
        /// Returns true if a grave was available and state was seeded.
        /// </summary>
        public bool PrepareFromLastWill(LastWillSystem lastWill, IList<string> roomIds, string previousSaveId = null)
        {
            if (lastWill == null || !lastWill.HasGraveSite || lastWill.CurrentGraveSite == null)
                return false;
            PrepareFromGrave(lastWill.CurrentGraveSite, roomIds, previousSaveId);
            return IsPrepared;
        }

        /// <summary>
        /// Activate the legacy run: bunker is ruined, discovery events fire.
        /// Returns loot item ids granted as a starting bonus (may be empty).
        /// No-op (returns empty) if already active or nothing prepared.
        /// </summary>
        public IReadOnlyList<string> BeginLegacyRun(string previousSaveId = null)
        {
            if (_state.legacy_run_active)
                return _state.remaining_loot_ids ?? (IReadOnlyList<string>)Array.Empty<string>();

            if (!IsPrepared && string.IsNullOrEmpty(previousSaveId))
                return Array.Empty<string>();

            if (!string.IsNullOrEmpty(previousSaveId))
                _state.previous_save_id = previousSaveId;
            if (string.IsNullOrEmpty(_state.previous_save_id))
                _state.previous_save_id = "legacy_prior";

            _state.bunker_ruined = true;
            _state.legacy_run_active = true;
            _state.excavation_progress = 0f;
            _excavatedRooms.Clear();
            _state.excavated_rooms = new List<string>();

            OnLegacyLoaded?.Invoke(_state.previous_save_id);

            if (_state.flooded_rooms != null)
            {
                for (int i = 0; i < _state.flooded_rooms.Count; i++)
                    OnRuinedRoomDiscovered?.Invoke(_state.flooded_rooms[i]);
            }

            if (_state.corpse_locations != null)
            {
                for (int i = 0; i < _state.corpse_locations.Count; i++)
                {
                    var c = _state.corpse_locations[i];
                    if (c == null) continue;
                    OnCorpseFound?.Invoke(c.survivor_id, c.room_id);
                }
            }

            var loot = _state.remaining_loot_ids != null
                ? (IReadOnlyList<string>)_state.remaining_loot_ids
                : Array.Empty<string>();
            if (loot.Count > 0)
                OnLegacyLootGranted?.Invoke(loot);
            return loot;
        }

        /// <summary>
        /// Load legacy data by id (Prompt #859). Prefer <see cref="BeginLegacyRun"/> after
        /// <see cref="PrepareFromGrave"/> when corpse/flood data is available.
        /// </summary>
        public void LoadLegacy(string previousSaveId)
        {
            if (string.IsNullOrEmpty(previousSaveId) && !IsPrepared) return;
            BeginLegacyRun(previousSaveId);
        }

        /// <summary>True when a prior wipe was prepared or a legacy run is mid-progress.</summary>
        public bool CheckAvailability()
        {
            return IsPrepared || _state.legacy_run_active;
        }

        public IReadOnlyList<string> GetRuinedRooms()
        {
            return _state.flooded_rooms != null
                ? _state.flooded_rooms.AsReadOnly()
                : (IReadOnlyList<string>)Array.Empty<string>();
        }

        public IReadOnlyList<CorpseLocation> GetCorpseLocations()
        {
            return _state.corpse_locations != null
                ? _state.corpse_locations.AsReadOnly()
                : (IReadOnlyList<CorpseLocation>)Array.Empty<CorpseLocation>();
        }

        public IReadOnlyList<string> GetRemainingLootIds()
        {
            return _state.remaining_loot_ids != null
                ? _state.remaining_loot_ids.AsReadOnly()
                : (IReadOnlyList<string>)Array.Empty<string>();
        }

        public bool IsRoomExcavated(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            return _excavatedRooms.Contains(roomId);
        }

        /// <summary>
        /// Excavate a single room. Triggers ghost encounter if a corpse is present.
        /// </summary>
        public void ExcavateRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            if (_excavatedRooms.Contains(roomId)) return;

            _excavatedRooms.Add(roomId);
            if (_state.excavated_rooms == null)
                _state.excavated_rooms = new List<string>();
            if (!_state.excavated_rooms.Contains(roomId))
                _state.excavated_rooms.Add(roomId);

            if (_state.corpse_locations != null)
            {
                for (int i = 0; i < _state.corpse_locations.Count; i++)
                {
                    var c = _state.corpse_locations[i];
                    if (c != null && string.Equals(c.room_id, roomId, StringComparison.Ordinal))
                        OnGhostEncountered?.Invoke(c.survivor_id);
                }
            }

            int totalRooms = _state.flooded_rooms != null ? _state.flooded_rooms.Count : 0;
            if (totalRooms > 0)
                _state.excavation_progress = (float)_excavatedRooms.Count / totalRooms;
            else
                _state.excavation_progress = 1f;

            OnRoomExcavated?.Invoke(roomId);
        }

        public float GetExcavationProgress() => _state.excavation_progress;

        // ── Save / Load ────────────────────────────────────────────────

        public LegacyStartState CaptureState()
        {
            var copy = new LegacyStartState
            {
                system_id = "system_legacy_start",
                previous_save_id = _state.previous_save_id ?? string.Empty,
                bunker_ruined = _state.bunker_ruined,
                legacy_run_active = _state.legacy_run_active,
                excavation_progress = _state.excavation_progress,
                prior_day_of_death = _state.prior_day_of_death,
                cause_of_death = _state.cause_of_death ?? string.Empty,
                flooded_rooms = new List<string>(),
                corpse_locations = new List<CorpseLocation>(),
                remaining_loot_ids = new List<string>(),
                excavated_rooms = new List<string>()
            };

            if (_state.flooded_rooms != null)
            {
                for (int i = 0; i < _state.flooded_rooms.Count; i++)
                    copy.flooded_rooms.Add(_state.flooded_rooms[i]);
            }
            if (_state.corpse_locations != null)
            {
                for (int i = 0; i < _state.corpse_locations.Count; i++)
                {
                    var c = _state.corpse_locations[i];
                    if (c == null) continue;
                    copy.corpse_locations.Add(new CorpseLocation(c.survivor_id, c.room_id));
                }
            }
            if (_state.remaining_loot_ids != null)
            {
                for (int i = 0; i < _state.remaining_loot_ids.Count; i++)
                    copy.remaining_loot_ids.Add(_state.remaining_loot_ids[i]);
            }
            foreach (var room in _excavatedRooms)
                copy.excavated_rooms.Add(room);

            return copy;
        }

        public void RestoreState(LegacyStartState saved)
        {
            _excavatedRooms.Clear();
            if (saved == null)
            {
                _state = new LegacyStartState();
                return;
            }

            _state = new LegacyStartState
            {
                system_id = "system_legacy_start",
                previous_save_id = saved.previous_save_id ?? string.Empty,
                bunker_ruined = saved.bunker_ruined,
                legacy_run_active = saved.legacy_run_active,
                excavation_progress = saved.excavation_progress,
                prior_day_of_death = saved.prior_day_of_death,
                cause_of_death = saved.cause_of_death ?? string.Empty,
                flooded_rooms = new List<string>(),
                corpse_locations = new List<CorpseLocation>(),
                remaining_loot_ids = new List<string>(),
                excavated_rooms = new List<string>()
            };

            if (saved.flooded_rooms != null)
            {
                for (int i = 0; i < saved.flooded_rooms.Count; i++)
                {
                    if (!string.IsNullOrEmpty(saved.flooded_rooms[i]))
                        _state.flooded_rooms.Add(saved.flooded_rooms[i]);
                }
            }
            if (saved.corpse_locations != null)
            {
                for (int i = 0; i < saved.corpse_locations.Count; i++)
                {
                    var c = saved.corpse_locations[i];
                    if (c == null || string.IsNullOrEmpty(c.survivor_id)) continue;
                    _state.corpse_locations.Add(new CorpseLocation(c.survivor_id, c.room_id));
                }
            }
            if (saved.remaining_loot_ids != null)
            {
                for (int i = 0; i < saved.remaining_loot_ids.Count; i++)
                {
                    if (!string.IsNullOrEmpty(saved.remaining_loot_ids[i]))
                        _state.remaining_loot_ids.Add(saved.remaining_loot_ids[i]);
                }
            }
            if (saved.excavated_rooms != null)
            {
                for (int i = 0; i < saved.excavated_rooms.Count; i++)
                {
                    string room = saved.excavated_rooms[i];
                    if (string.IsNullOrEmpty(room)) continue;
                    _excavatedRooms.Add(room);
                    _state.excavated_rooms.Add(room);
                }
            }
        }
    }
}
