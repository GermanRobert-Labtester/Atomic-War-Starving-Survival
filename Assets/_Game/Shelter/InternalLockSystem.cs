using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    public class InternalLockSystem
    {
        private readonly Dictionary<string, bool> _roomLocks = new Dictionary<string, bool>();
        private readonly Dictionary<string, string> _roomGuards = new Dictionary<string, string>();

        public event Action<string, bool> OnDoorLockChanged;
        public event Action<string, string> OnGuardAssigned;

        public void SetDoorLock(string roomId, bool isLocked)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            _roomLocks[roomId] = isLocked;
            OnDoorLockChanged?.Invoke(roomId, isLocked);
        }

        public bool IsDoorLocked(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            return _roomLocks.TryGetValue(roomId, out var locked) && locked;
        }

        public void AssignGuard(string roomId, string guardSurvivorId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            _roomGuards[roomId] = guardSurvivorId;
            OnGuardAssigned?.Invoke(roomId, guardSurvivorId);
        }

        public bool IsGuarded(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return false;
            return _roomGuards.TryGetValue(roomId, out var guardId) && !string.IsNullOrEmpty(guardId);
        }

        public string GetGuard(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return null;
            return _roomGuards.TryGetValue(roomId, out var guardId) ? guardId : null;
        }

        public bool CanSleepwalkerEscape(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return true;
            if (IsDoorLocked(roomId)) return false;
            if (IsGuarded(roomId)) return false;
            return true;
        }
    }
}
