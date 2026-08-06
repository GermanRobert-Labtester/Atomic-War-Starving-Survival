using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TearGasState
    {
        public string itemId = "item_tear_gas";
        public int stunDurationTurns = 2;
        public bool uselessVsGasMask = true;
        public bool uselessVsMutants = true;
    }

    public class Item_TearGas
    {
        // Events
        public event Action<string, CombatLane> OnLaneEvacuated;   // throwerId, lane
        public event Action<string, CombatLane> OnLaneStunned;     // throwerId, lane

        // Internal state
        private readonly TearGasState _state;
        private readonly Dictionary<string, int> _stunnedEnemies = new Dictionary<string, int>(); // enemyId → turns remaining

        public Item_TearGas()
        {
            _state = new TearGasState();
        }

        /// <summary>
        /// Throw tear gas into a combat lane.
        /// Forces human enemies without gas masks to evacuate lane or suffer stun.
        /// Useless against enemies with GasMasks or Mutants.
        /// Returns list of affected enemy IDs.
        /// </summary>
        public List<string> Throw(
            string throwerId,
            CombatLane targetLane,
            List<(string enemyId, bool hasGasMask, bool isMutant)> enemies)
        {
            var affected = new List<string>();

            if (string.IsNullOrEmpty(throwerId))
            {
                Debug.LogWarning("[Item_TearGas] Invalid thrower id.");
                return affected;
            }

            if (enemies == null || enemies.Count == 0)
            {
                return affected;
            }

            bool anyEvacuated = false;

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];

                // Useless vs gas masks
                if (_state.uselessVsGasMask && enemy.hasGasMask)
                {
                    continue;
                }

                // Useless vs mutants
                if (_state.uselessVsMutants && enemy.isMutant)
                {
                    continue;
                }

                // Human without gas mask: affected
                affected.Add(enemy.enemyId);
                _stunnedEnemies[enemy.enemyId] = _state.stunDurationTurns;
            }

            if (affected.Count > 0)
            {
                // Fire evacuation event for the lane
                OnLaneEvacuated?.Invoke(throwerId, targetLane);
                anyEvacuated = true;

                // Fire stun event for enemies that didn't evacuate
                OnLaneStunned?.Invoke(throwerId, targetLane);
            }

            return affected;
        }

        /// <summary>
        /// Returns true if the enemy is currently stunned by tear gas.
        /// </summary>
        public bool IsStunned(string enemyId)
        {
            return !string.IsNullOrEmpty(enemyId)
                && _stunnedEnemies.TryGetValue(enemyId, out int turns)
                && turns > 0;
        }

        /// <summary>
        /// Tick stun durations down by 1 turn. Call at end of each turn.
        /// </summary>
        public void TickTurnEnd()
        {
            var toRemove = new List<string>();
            foreach (var kvp in _stunnedEnemies)
            {
                _stunnedEnemies[kvp.Key] = kvp.Value - 1;
                if (_stunnedEnemies[kvp.Key] <= 0)
                {
                    toRemove.Add(kvp.Key);
                }
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                _stunnedEnemies.Remove(toRemove[i]);
            }
        }

        public TearGasState CaptureState()
        {
            return new TearGasState
            {
                itemId = _state.itemId,
                stunDurationTurns = _state.stunDurationTurns,
                uselessVsGasMask = _state.uselessVsGasMask,
                uselessVsMutants = _state.uselessVsMutants
            };
        }

        public void RestoreState(TearGasState saved)
        {
            if (saved == null) return;
            _state.itemId = saved.itemId;
            _state.stunDurationTurns = saved.stunDurationTurns;
            _state.uselessVsGasMask = saved.uselessVsGasMask;
            _state.uselessVsMutants = saved.uselessVsMutants;
        }
    }
}
