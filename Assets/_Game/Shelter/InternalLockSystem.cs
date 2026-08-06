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

        // -----------------------------------------------------------------
        // Save / Load (audit wiring fix)
        // -----------------------------------------------------------------
        public InternalLockSave CaptureState()
        {
            var sav = new InternalLockSave();
            sav.LockKeys = new string[_roomLocks.Count];
            sav.LockValues = new bool[_roomLocks.Count];
            int i = 0;
            foreach (var kv in _roomLocks) { sav.LockKeys[i] = kv.Key; sav.LockValues[i] = kv.Value; i++; }
            sav.GuardKeys = new string[_roomGuards.Count];
            sav.GuardValues = new string[_roomGuards.Count];
            i = 0;
            foreach (var kv in _roomGuards) { sav.GuardKeys[i] = kv.Key; sav.GuardValues[i] = kv.Value ?? ""; i++; }
            return sav;
        }

        public void RestoreState(InternalLockSave save)
        {
            _roomLocks.Clear();
            _roomGuards.Clear();
            if (save == null) return;
            if (save.LockKeys != null)
                for (int i = 0; i < save.LockKeys.Length; i++)
                    if (!string.IsNullOrEmpty(save.LockKeys[i]))
                        _roomLocks[save.LockKeys[i]] = save.LockValues != null && i < save.LockValues.Length && save.LockValues[i];
            if (save.GuardKeys != null)
                for (int i = 0; i < save.GuardKeys.Length; i++)
                    if (!string.IsNullOrEmpty(save.GuardKeys[i]))
                        _roomGuards[save.GuardKeys[i]] = save.GuardValues != null && i < save.GuardValues.Length ? save.GuardValues[i] : null;
        }
    }

    [Serializable]
    public class InternalLockSave
    {
        public string[] LockKeys;
        public bool[] LockValues;
        public string[] GuardKeys;
        public string[] GuardValues;
    }
}
