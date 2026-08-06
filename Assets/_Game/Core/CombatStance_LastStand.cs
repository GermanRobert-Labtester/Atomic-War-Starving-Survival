using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LastStandState
    {
        public string stanceId = "combat_stance_last_stand";
        public float accuracyMultiplier = 2.0f;
        public float damageMultiplier = 2.0f;
        public bool canFlee = false;
        public bool deathIsInstant = true;
    }

    public class CombatStance_LastStand
    {
        public event Action<string> OnLastStandActivated;
        public event Action<string, string> OnMutualKill;

        private LastStandState _state;
        private HashSet<string> _activeSurvivors = new HashSet<string>();

        public CombatStance_LastStand()
        {
            _state = new LastStandState();
        }

        public CombatStance_LastStand(LastStandState state)
        {
            _state = state ?? new LastStandState();
        }

        public LastStandState CaptureState() => _state;

        public void RestoreState(LastStandState state)
        {
            _state = state ?? new LastStandState();
        }

        public void Activate(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _activeSurvivors.Add(survivorId);
            OnLastStandActivated?.Invoke(survivorId);
        }

        /// <summary>
        /// When a survivor in Last Stand reaches 0 HP, they die instantly
        /// and take the specified target with them.
        /// </summary>
        public void OnReachZeroHP(string survivorId, string targetId)
        {
            if (!IsActive(survivorId)) return;
            _activeSurvivors.Remove(survivorId);
            OnMutualKill?.Invoke(survivorId, targetId);
        }

        public bool IsActive(string survivorId)
        {
            return _activeSurvivors.Contains(survivorId);
        }

        public float GetAccuracyMultiplier() => _state.accuracyMultiplier;

        public float GetDamageMultiplier() => _state.damageMultiplier;

        public bool CanFlee() => _state.canFlee;
    }
}
