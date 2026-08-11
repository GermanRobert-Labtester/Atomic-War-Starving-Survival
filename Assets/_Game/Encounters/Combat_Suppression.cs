using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class SuppressionState
    {
        public string mechanicId = "combat_suppression";
        public bool accuracyDropToZero = true;
        public int durationTurns = 1;
        public List<string> pinnedEnemyIds = new List<string>();
    }

    public class Combat_Suppression
    {
        // Events
        public event Action<string, string> OnSuppressionFired;   // survivorId, weaponId
        public event Action<string> OnEnemyPinned;                // enemyId

        // Internal state
        private readonly SuppressionState _state;
        private readonly HashSet<string> _pinnedEnemies = new HashSet<string>();
        private static readonly HashSet<string> SuppressionWeapons = new HashSet<string>
        {
            "weapon_assault_rifle",
            "weapon_lmg"
        };

        public Combat_Suppression()
        {
            _state = new SuppressionState();
        }

        /// <summary>
        /// Attempt suppression fire. Requires AssaultRifle or LMG.
        /// Deals 0 damage. Pins all enemies: accuracy drops to 0% for 1 turn.
        /// </summary>
        public bool TrySuppress(string survivorId, string weaponId, List<string> enemyIds)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(weaponId))
            {
                Debug.LogWarning("[Combat_Suppression] Invalid survivor or weapon id.");
                return false;
            }

            if (!SuppressionWeapons.Contains(weaponId))
            {
                Debug.LogWarning($"[Combat_Suppression] Weapon '{weaponId}' cannot suppress. Requires AssaultRifle or LMG.");
                return false;
            }

            if (enemyIds == null || enemyIds.Count == 0)
            {
                Debug.LogWarning("[Combat_Suppression] No enemies to suppress.");
                return false;
            }

            OnSuppressionFired?.Invoke(survivorId, weaponId);

            for (int i = 0; i < enemyIds.Count; i++)
            {
                string enemyId = enemyIds[i];
                if (!string.IsNullOrEmpty(enemyId))
                {
                    _pinnedEnemies.Add(enemyId);
                    OnEnemyPinned?.Invoke(enemyId);
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if the enemy is currently pinned (accuracy = 0%).
        /// </summary>
        public bool IsPinned(string enemyId)
        {
            return !string.IsNullOrEmpty(enemyId) && _pinnedEnemies.Contains(enemyId);
        }

        /// <summary>
        /// Call at end of turn to clear all pins.
        /// </summary>
        public void ClearPin(string enemyId)
        {
            if (!string.IsNullOrEmpty(enemyId))
            {
                _pinnedEnemies.Remove(enemyId);
            }
        }

        /// <summary>
        /// Clear all pins at once (convenience for turn-end).
        /// </summary>
        public void ClearAllPins()
        {
            _pinnedEnemies.Clear();
        }

        public SuppressionState CaptureState()
        {
            return new SuppressionState
            {
                mechanicId = _state.mechanicId,
                accuracyDropToZero = _state.accuracyDropToZero,
                durationTurns = _state.durationTurns,
                pinnedEnemyIds = new List<string>(_pinnedEnemies)
            };
        }

        public void RestoreState(SuppressionState saved)
        {
            if (saved == null) return;
            _state.mechanicId = saved.mechanicId;
            _state.accuracyDropToZero = saved.accuracyDropToZero;
            _state.durationTurns = saved.durationTurns;

            _pinnedEnemies.Clear();
            if (saved.pinnedEnemyIds != null)
            {
                for (int i = 0; i < saved.pinnedEnemyIds.Count; i++)
                {
                    _pinnedEnemies.Add(saved.pinnedEnemyIds[i]);
                }
            }
        }
    }
}
