using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BarricadeState
    {
        public string actionId = "action_barricade_door";
        public bool requiresCrowbarToBreak = true;
        // roomId → survivorId that barricaded it
        public List<string> barricadedRoomIds = new List<string>();
        public List<string> barricaderIds = new List<string>();
    }

    /// <summary>
    /// A paranoid survivor can barricade a room's door from the inside,
    /// dragging furniture against it. Breaking through requires a crowbar;
    /// without one the door module is destroyed in the attempt.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_BarricadeDoor
    {
        // ── Events ──────────────────────────────────────────────────────
        public event Action<string, string> OnDoorBarricaded;    // survivorId, roomId
        public event Action<string, string> OnDoorBreached;      // breacherId, roomId
        public event Action<string> OnDoorModuleDestroyed;       // roomId

        // ── State ───────────────────────────────────────────────────────
        // roomId → survivorId who barricaded it
        private Dictionary<string, string> _barricadedRooms = new Dictionary<string, string>();

        private bool _requiresCrowbarToBreak = true;

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>
        /// A survivor barricades a room from the inside, dragging furniture
        /// against the door. Only one survivor can barricade a given room.
        /// </summary>
        public void Barricade(string survivorId, string roomId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(roomId)) return;
            if (_barricadedRooms.ContainsKey(roomId))
            {
                Debug.LogWarning($"[Action_BarricadeDoor] Room '{roomId}' is already barricaded.");
                return;
            }

            _barricadedRooms[roomId] = survivorId;
            OnDoorBarricaded?.Invoke(survivorId, roomId);
        }

        /// <summary>
        /// Attempts to breach a barricaded room. If the breacher has a
        /// crowbar the barricade is broken cleanly; otherwise the door
        /// module is destroyed in the process.
        /// </summary>
        public void BreachRoom(string breacherId, string roomId, bool hasCrowbar)
        {
            if (string.IsNullOrEmpty(breacherId) || string.IsNullOrEmpty(roomId)) return;

            if (!_barricadedRooms.ContainsKey(roomId))
            {
                Debug.LogWarning($"[Action_BarricadeDoor] Room '{roomId}' is not barricaded.");
                return;
            }

            _barricadedRooms.Remove(roomId);
            OnDoorBreached?.Invoke(breacherId, roomId);

            if (!hasCrowbar && _requiresCrowbarToBreak)
            {
                OnDoorModuleDestroyed?.Invoke(roomId);
            }
        }

        /// <summary>
        /// Returns true if the room is currently barricaded.
        /// </summary>
        public bool IsBarricaded(string roomId)
        {
            return !string.IsNullOrEmpty(roomId) && _barricadedRooms.ContainsKey(roomId);
        }

        /// <summary>
        /// Returns the survivorId of whoever barricaded the room, or null.
        /// </summary>
        public string GetBarricader(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return null;
            return _barricadedRooms.TryGetValue(roomId, out var survivorId) ? survivorId : null;
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public BarricadeState CaptureState()
        {
            var state = new BarricadeState
            {
                actionId = "action_barricade_door",
                requiresCrowbarToBreak = _requiresCrowbarToBreak,
                barricadedRoomIds = new List<string>(),
                barricaderIds = new List<string>()
            };

            foreach (var kvp in _barricadedRooms)
            {
                state.barricadedRoomIds.Add(kvp.Key);
                state.barricaderIds.Add(kvp.Value);
            }

            return state;
        }

        public void RestoreState(BarricadeState saved)
        {
            _barricadedRooms.Clear();
            if (saved == null) return;

            _requiresCrowbarToBreak = saved.requiresCrowbarToBreak;

            // Either list is null when the save omitted it explicitly; guard before Count.
            if (saved.barricadedRoomIds == null || saved.barricaderIds == null) return;
            int count = Mathf.Min(saved.barricadedRoomIds.Count, saved.barricaderIds.Count);
            for (int i = 0; i < count; i++)
            {
                string roomId = saved.barricadedRoomIds[i];
                if (string.IsNullOrEmpty(roomId)) continue;
                _barricadedRooms[roomId] = saved.barricaderIds[i];
            }
        }
    }
}
