using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SealRoomState
    {
        public string actionId = "action_seal_room";
        public int cementCost = 50;
    }

    /// <summary>
    /// Prompt #582: Action: Seal Room.
    /// Permanently entombs an irradiated room. Costs massive Cement.
    /// Room removed from grid forever, stops rad leaks.
    /// </summary>
    public class Action_SealRoom
    {
        private SealRoomState _state = new SealRoomState();

        public event Action<SealRoomState, string> OnRoomSealed;
        public event Action<SealRoomState, string, string> OnSealAttemptFailed;

        public SealRoomState State => _state;

        public bool CanSealRoom(string roomId, int cementAvailable, float roomRadiationLevel)
        {
            if (cementAvailable < _state.cementCost) return false;
            if (roomRadiationLevel <= 0f) return false;
            if (string.IsNullOrEmpty(roomId)) return false;
            return true;
        }

        public void SealRoom(string roomId, Action<string> removeRoomFromGrid, Action stopRadLeak,
            int cementAvailable, float roomRadiationLevel)
        {
            if (!CanSealRoom(roomId, cementAvailable, roomRadiationLevel))
            {
                string reason = cementAvailable < _state.cementCost
                    ? "insufficient_cement"
                    : "room_not_irradiated";
                OnSealAttemptFailed?.Invoke(_state, roomId, reason);
                return;
            }

            removeRoomFromGrid?.Invoke(roomId);
            stopRadLeak?.Invoke();
            OnRoomSealed?.Invoke(_state, roomId);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SealRoomState CaptureState() => _state;

        public void RestoreState(SealRoomState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
