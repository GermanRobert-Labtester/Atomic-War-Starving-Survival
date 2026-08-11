using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public sealed class CookOffState
    {
        public string hazardId = "hazard_cook_off";
        public float durabilityThreshold = 0.1f;  // 10%
        public float cookOffChance = 0.5f;         // 50%
    }

    public readonly struct CookOffResult
    {
        public readonly bool cookOff;
        public readonly List<string> hitTargets;

        public CookOffResult(bool cookOff, List<string> hitTargets)
        {
            this.cookOff = cookOff;
            this.hitTargets = hitTargets;
        }
    }

    public sealed class Hazard_CookOff
    {
        public event Action<string, string> OnCookOffTriggered;   // (survivorId, weaponId)
        public event Action<string, string> OnRandomTargetHit;    // (weaponId, hitTargetId)
        public event Action<string> OnWeaponMelted;               // (weaponId)

        private CookOffState _state = new CookOffState();

        // Attempt to fire a weapon. If durability is below threshold and the
        // random roll triggers, a cook-off occurs: the entire magazine fires
        // uncontrollably at random combatants (including allies), then the
        // weapon melts.
        //
        // Parameters:
        //   survivorId       — the survivor pulling the trigger
        //   weaponId         — the weapon being fired
        //   durability       — current weapon durability (0-1 normalised)
        //   allCombatantIds  — all entities in the fight (friend + foe)
        //   rng              — seeded random for deterministic replay
        //
        // Returns CookOffResult with cookOff flag and list of targets hit.
        public CookOffResult TryFire(
            string survivorId,
            string weaponId,
            float durability,
            List<string> allCombatantIds,
            Random rng)
        {
            if (string.IsNullOrEmpty(survivorId))
                throw new ArgumentNullException(nameof(survivorId));
            if (string.IsNullOrEmpty(weaponId))
                throw new ArgumentNullException(nameof(weaponId));
            if (allCombatantIds == null || allCombatantIds.Count == 0)
                return new CookOffResult(false, new List<string>());
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            // Check if durability is below the cook-off threshold.
            if (durability >= _state.durabilityThreshold)
            {
                // Normal fire — no cook-off.
                return new CookOffResult(false, new List<string>());
            }

            // Roll for cook-off.
            double roll = rng.NextDouble();
            if (roll >= _state.cookOffChance)
            {
                // Lucky — survived this shot without cook-off.
                return new CookOffResult(false, new List<string>());
            }

            // --- COOK-OFF ---
            OnCookOffTriggered?.Invoke(survivorId, weaponId);

            // Fire entire magazine at random targets.
            // Magazine size is abstracted as hitting every combatant once.
            List<string> hitTargets = new List<string>(allCombatantIds.Count);
            // Shuffle a copy and pick targets.
            List<string> pool = new List<string>(allCombatantIds);
            while (pool.Count > 0)
            {
                int idx = rng.Next(pool.Count);
                string target = pool[idx];
                pool.RemoveAt(idx);
                hitTargets.Add(target);
                OnRandomTargetHit?.Invoke(weaponId, target);
            }

            // Weapon melts after the cook-off.
            OnWeaponMelted?.Invoke(weaponId);

            return new CookOffResult(true, hitTargets);
        }

        public float GetDurabilityThreshold() => _state.durabilityThreshold;
        public float GetCookOffChance() => _state.cookOffChance;

        // --- Save / Load -----------------------------------------------------
        public CookOffState CaptureState() => new CookOffState
        {
            hazardId = _state.hazardId,
            durabilityThreshold = _state.durabilityThreshold,
            cookOffChance = _state.cookOffChance
        };

        public void RestoreState(CookOffState saved)
        {
            _state.hazardId = saved.hazardId;
            _state.durabilityThreshold = saved.durabilityThreshold;
            _state.cookOffChance = saved.cookOffChance;
        }
    }
}
