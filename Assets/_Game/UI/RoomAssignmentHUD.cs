using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// HUD widget that shows every survivor in the shelter alongside their
    /// assigned room, and lets the player reassign them via a dropdown or
    /// drag-and-drop flow. The widget reads <see cref="Survivor.CurrentRoomId"/>
    /// to populate the current room and writes it when the player picks a
    /// different room. The <see cref="MentalBreakSystem"/> picks up the
    /// change on the next tick, scoping its passive morale drain to the
    /// room the broken survivor shares.
    ///
    /// The actual visual integration (UGUI Buttons, Dropdowns, etc.) lives
    /// in a Unity-scene MonoBehaviour that inherits from this class or
    /// reads its public properties. The core logic runs in EditMode
    /// without a scene loaded so the room-to-survivor mapping is testable
    /// in pure C#.
    /// </summary>
    public class RoomAssignmentHUD : MonoBehaviour
    {
        [Tooltip("Refresh-rate of the room-list cache in seconds. Leave at 0 to refresh every frame.")]
        public float RoomListRefreshInterval = 5f;

        [Tooltip("Player-facing label for the 'unassigned' pseudo-room displayed in the UI.")]
        public string UnassignedRoomLabel = "Common Area";

        /// <summary>The survivors whose rooms this widget manages. Injected
        /// by GameBootstrap.RoomAssignmentHUD.Bind.</summary>
        public IReadOnlyList<Survivor> Survivors { get; private set; }

        /// <summary>The shelter whose rooms serve as the valid room-id
        /// set for the dropdown. Injected by GameBootstrap on Bind.</summary>
        public Shelter.Shelter Shelter { get; private set; }

        // -----------------------------------------------------------------
        // Public read API (used by the visual MonoBehaviour)
        // -----------------------------------------------------------------

        /// <summary>All unique room ids currently present in the shelter
        /// (from ShelterModuleInstance.RoomId across all installed modules).
        /// Cached for <see cref="RoomListRefreshInterval"/> seconds.</summary>
        public List<string> RoomIds
        {
            get
            {
                bool cacheExpired = Time.time - _lastRoomCacheTime >= RoomListRefreshInterval;
                if (_cachedRoomIds != null && !cacheExpired) return _cachedRoomIds;
                _cachedRoomIds = Shelter != null ? Shelter.GetRoomIds() : new List<string>();
                _lastRoomCacheTime = Time.time;
                return _cachedRoomIds;
            }
        }

        /// <summary>
        /// Returns a list of (Survivor, current-room-label) pairs for
        /// the widget to render. Every living survivor gets a row; dead
        /// survivors are excluded. Survivors with an empty/null
        /// CurrentRoomId are shown with <see cref="UnassignedRoomLabel"/>.
        /// </summary>
        public List<SurvivorRoomPair> GetAssignmentRows()
        {
            var rows = new List<SurvivorRoomPair>();
            if (Survivors == null) return rows;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                string label = string.IsNullOrEmpty(sv.CurrentRoomId)
                    ? UnassignedRoomLabel
                    : sv.CurrentRoomId;
                rows.Add(new SurvivorRoomPair { Survivor = sv, CurrentRoomLabel = label });
            }
            return rows;
        }

        // -----------------------------------------------------------------
        // Public write API (called by UI events / drag-drop handlers)
        // -----------------------------------------------------------------

        /// <summary>
        /// Move a survivor to a different room. Writes
        /// <c>sv.CurrentRoomId</c> so the <see cref="MentalBreakSystem"/>
        /// picks it up on the next tick. Empty roomId = unassign (common area).
        /// Returns true if the assignment changed; false if the survivor
        /// was already in that room or if the survivor id wasn't found.
        /// </summary>
        public bool AssignSurvivorToRoom(string survivorId, string roomId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (Survivors == null) return false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (sv.Id != survivorId) continue;
                if (sv.CurrentRoomId == roomId) return false;
                sv.CurrentRoomId = roomId;
                OnRoomAssignmentChanged?.Invoke(sv, roomId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Fire-once convenience: assign every unassigned survivor to a
        /// default room so the MentalBreakSystem immediately has room
        /// boundaries and no one stays in the catch-all "common area."
        /// </summary>
        public void AssignAllUnassigned(string defaultRoomId)
        {
            if (Survivors == null || string.IsNullOrEmpty(defaultRoomId)) return;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var sv = Survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (string.IsNullOrEmpty(sv.CurrentRoomId))
                {
                    sv.CurrentRoomId = defaultRoomId;
                    OnRoomAssignmentChanged?.Invoke(sv, defaultRoomId);
                }
            }
        }

        /// <summary>Fired whenever a survivor's room assignment changes.
        /// Args: (survivor, newRoomId). Used by the visual HUD to refresh
        /// the row and by the GameBootstrap for a one-line debug log.</summary>
        public event Action<Survivor, string> OnRoomAssignmentChanged;

        // -----------------------------------------------------------------
        // Binding
        // -----------------------------------------------------------------

        /// <summary>
        /// Wire the survivor list and the shelter. Called by
        /// <see cref="HUD.BindRoomAssignment"/> once per scene load.
        /// Invalidates the room-id cache.
        /// </summary>
        public void Bind(IReadOnlyList<Survivor> survivors, Shelter.Shelter shelter)
        {
            Survivors = survivors;
            Shelter = shelter;
            _cachedRoomIds = null;
            _lastRoomCacheTime = float.NegativeInfinity;
        }

        // -----------------------------------------------------------------
        // Internal
        // -----------------------------------------------------------------

        private List<string> _cachedRoomIds;
        private float _lastRoomCacheTime = float.NegativeInfinity;
    }

    /// <summary>
    /// One row in the room-assignment widget: a survivor and the label
    /// the UI should show for their current room. The visual MonoBehaviour
    /// can bind the survivor's id to the AssignSurvivorToRoom call.
    /// </summary>
    [Serializable]
    public struct SurvivorRoomPair
    {
        public Survivor Survivor;
        public string CurrentRoomLabel;
    }
}
