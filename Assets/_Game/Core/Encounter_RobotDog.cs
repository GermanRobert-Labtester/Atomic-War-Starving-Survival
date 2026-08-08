using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RobotDogState
    {
        public string id = "encounter_robot_dog";
        public string displayName = "Military Robot Dog";
        public float healthPool = 300f;
        public float speedMultiplier = 3.0f;
        public float armorRating = 0.80f;
        public int electronicScrapDrop = 5;
        public int motorDrop = 2;
        public bool isHackable = true;
        public bool isHacked = false;
        public float currentHealth = 300f;
        public bool isDefeated = false;
    }

    /// <summary>
    /// Prompt #613: Encounter: Military Robot Dog.
    /// A fast, heavily armored military pacification unit. Drops ElectronicScrap and Motors
    /// on defeat. Can be hacked by a TechBro or Mechanic character.
    /// </summary>
    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public class Encounter_RobotDog
    {
        private RobotDogState _state = new RobotDogState();

        /// <summary>
        /// Base hack success chance when a qualified character is present.
        /// </summary>
        public const float HackBaseChance = 0.60f;

        public event Action<RobotDogState, float> OnRobotDogEngaged;
        public event Action<RobotDogState> OnRobotDogDefeated;
        public event Action<RobotDogState> OnRobotDogHacked;
        public event Action<RobotDogState, int, int> OnLootDropped;

        public RobotDogState State => _state;

        /// <summary>
        /// Engages the robot dog in combat. Damage is reduced by armor rating.
        /// </summary>
        /// <param name="combatPower">The attacker's combat power.</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>True if the robot dog is defeated.</returns>
        public bool EngageCombat(float combatPower, System.Random rng)
        {
            if (_state.isDefeated || _state.isHacked)
                return false;

            // Armor reduces incoming damage
            float effectiveDamage = combatPower * (1f - _state.armorRating);
            _state.currentHealth = Mathf.Max(0f, _state.currentHealth - effectiveDamage);

            OnRobotDogEngaged?.Invoke(_state, effectiveDamage);

            if (_state.currentHealth <= 0f)
            {
                _state.isDefeated = true;
                OnRobotDogDefeated?.Invoke(_state);
                OnLootDropped?.Invoke(_state, _state.electronicScrapDrop, _state.motorDrop);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to hack the robot dog. Requires a TechBro or Mechanic character.
        /// </summary>
        /// <param name="hasTechBro">Whether a TechBro character is available.</param>
        /// <param name="hasMechanic">Whether a Mechanic character is available.</param>
        /// <param name="intelligence">The hacker's intelligence stat.</param>
        /// <returns>True if the hack succeeded.</returns>
        public bool TryHack(bool hasTechBro, bool hasMechanic, float intelligence)
        {
            if (!_state.isHackable || _state.isDefeated || _state.isHacked)
                return false;

            if (!hasTechBro && !hasMechanic)
                return false;

            // 60% base hack chance if qualified
            if (UnityEngine.Random.value < HackBaseChance)
            {
                _state.isHacked = true;
                _state.isDefeated = true;
                OnRobotDogHacked?.Invoke(_state);
                OnLootDropped?.Invoke(_state, _state.electronicScrapDrop, _state.motorDrop);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns whether the robot dog has been defeated (by combat or hack).
        /// </summary>
        public bool IsDefeated()
        {
            return _state.isDefeated;
        }

        /// <summary>
        /// Returns the loot drops if the robot dog is defeated.
        /// </summary>
        /// <returns>Tuple of (electronicScrap, motors). Zero if not defeated.</returns>
        public (int electronicScrap, int motors) GetDrops()
        {
            if (!_state.isDefeated)
                return (0, 0);

            return (_state.electronicScrapDrop, _state.motorDrop);
        }

        public RobotDogState CaptureState() => _state;

        public void RestoreState(RobotDogState saved)
        {
            _state = saved ?? new RobotDogState();
        }
    }
}
