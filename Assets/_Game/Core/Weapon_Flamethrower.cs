using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Combat lanes for positional combat. Defined here until Combat_Flanking.cs is created.
    /// </summary>
    public enum CombatLane
    {
        far_left = 0,
        left = 1,
        center = 2,
        right = 3,
        far_right = 4
    }

    [Serializable]
    public class FlamethrowerState
    {
        public string weaponId = "weapon_flamethrower";
        public float fuelPerUse = 0.2f;
        public int fearRadius = 2;
        public float tankExplodeOnCritChance = 0.25f;
    }

    public class Weapon_Flamethrower
    {
        public event Action<string, CombatLane> OnLaneIgnited;
        public event Action<string> OnEnemiesFled;
        public event Action<string> OnTankExploded;

        private FlamethrowerState _state;

        public Weapon_Flamethrower()
        {
            _state = new FlamethrowerState();
        }

        public Weapon_Flamethrower(FlamethrowerState state)
        {
            _state = state ?? new FlamethrowerState();
        }

        public FlamethrowerState CaptureState() => _state;

        public void RestoreState(FlamethrowerState state)
        {
            _state = state ?? new FlamethrowerState();
        }

        /// <summary>
        /// Fires the flamethrower at the target lane.
        /// Consumes fuel, ignites the lane, causes enemies to flee from fear.
        /// If the carrier is critically hit, the tank explodes (instant death).
        /// </summary>
        /// <param name="survivorId">ID of the survivor firing the weapon.</param>
        /// <param name="targetLane">The combat lane to ignite.</param>
        /// <param name="currentFuel">Current fuel level (0-1 range).</param>
        /// <param name="rng">Random number generator for deterministic rolls.</param>
        /// <returns>Tuple: (fired successfully, tank exploded).</returns>
        public (bool fired, bool tankExploded) Fire(
            string survivorId,
            CombatLane targetLane,
            float currentFuel,
            Random rng)
        {
            if (string.IsNullOrEmpty(survivorId))
                return (false, false);

            // Check if enough fuel
            if (currentFuel < _state.fuelPerUse)
                return (false, false);

            // Check for tank explosion on critical hit
            float critRoll = (float)rng.NextDouble();
            if (critRoll < _state.tankExplodeOnCritChance)
            {
                OnTankExploded?.Invoke(survivorId);
                return (false, true);
            }

            // Fire successfully — ignite the lane
            OnLaneIgnited?.Invoke(survivorId, targetLane);

            // Fear effect causes enemies to flee
            OnEnemiesFled?.Invoke(survivorId);

            return (true, false);
        }

        public float GetFuelCost() => _state.fuelPerUse;

        public int GetFearRadius() => _state.fearRadius;

        public float GetTankExplodeChance() => _state.tankExplodeOnCritChance;
    }
}
