using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Endgame
{
    [Serializable]
    public class MinecartState
    {
        public string projectId = "project_minecart";
        public bool isBuilt = false;
        public int constructionDays = 12;
        public int daysSpent = 0;
        public float movementSpeedMultiplier = 2.0f;
        public float ramDamage = 80f;
    }

    /// <summary>
    /// Prompt #586: Project: Minecart.
    /// For lateral bases (Sewer/Subway). Connects rooms horizontally.
    /// Speeds movement. Can ram intruders during breach.
    /// </summary>
    public class Project_Minecart
    {
        private MinecartState _state = new MinecartState();

        public event Action<MinecartState> OnMinecartBuilt;
        public event Action<MinecartState, string, string> OnRoomConnected;
        public event Action<MinecartState, string, bool> OnIntruderRammed;

        public MinecartState State => _state;

        public void StartConstruction()
        {
            if (_state.isBuilt) return;
            _state.daysSpent = 0;
        }

        public void TickDay()
        {
            if (_state.isBuilt) return;

            _state.daysSpent++;
            if (_state.daysSpent >= _state.constructionDays)
            {
                _state.isBuilt = true;
                OnMinecartBuilt?.Invoke(_state);
            }
        }

        public float GetMovementSpeed(string baseType)
        {
            if (!_state.isBuilt) return 1f;

            // Minecart only works in lateral bases
            if (baseType == "sewer" || baseType == "subway")
                return _state.movementSpeedMultiplier;

            return 1f;
        }

        public bool TryRamIntruder(string intruderId, System.Random rng)
        {
            if (!_state.isBuilt) return false;

            // 70% hit chance
            bool hit = rng.NextDouble() < 0.70;
            OnIntruderRammed?.Invoke(_state, intruderId, hit);
            return hit;
        }

        public void ConnectRooms(string roomA, string roomB)
        {
            if (!_state.isBuilt) return;
            OnRoomConnected?.Invoke(_state, roomA, roomB);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MinecartState CaptureState() => _state;

        public void RestoreState(MinecartState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
