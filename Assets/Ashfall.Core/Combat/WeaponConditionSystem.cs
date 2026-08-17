using System;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Host ports the combat sim uses to reach real survivor/inventory state.
    /// Every hook is optional; unset hooks degrade to in-encounter bookkeeping so
    /// the sim stays deterministic and testable without a full host.
    /// </summary>
    public class CombatHostPorts
    {
        /// <summary>Apply damage to a real survivor. Returns the new health (or the input when unset).</summary>
        public Func<string, float, float> DamageSurvivor;
        /// <summary>Apply healing to a real survivor. Returns the new health.</summary>
        public Func<string, float, float> HealSurvivor;
        /// <summary>Apply a morale delta to a real survivor.</summary>
        public Action<string, float> ApplyMoraleDelta;
        /// <summary>Consume rounds of the given ammo item. Returns -1 if it cannot be afforded, else remaining.</summary>
        public Func<string, int, int> ConsumeAmmo;
        /// <summary>Consume a generic item (scrap, bandage, etc.). Returns true when consumed.</summary>
        public Func<string, int, bool> ConsumeItem;
        /// <summary>Raise a trauma/affliction on a real survivor (kind, severity).</summary>
        public Action<string, string, float> RaiseTrauma;
        /// <summary>Grant captured loot to the real inventory.</summary>
        public Action<CombatLootEntry> GrantLoot;
        /// <summary>Record that a survivor survived a combat encounter (CombatTraumaSystem.OnCombatSurvived).</summary>
        public Action<string> MarkCombatSurvived;
    }

    /// <summary>
    /// Weapon durability / jamming authority. Degradation, jam probability and
    /// the roll that fails a shot all derive from ONE method so the UI-stated
    /// chance is exactly the value used by combat resolution.
    ///
    /// Deterministic: every random roll takes an injected ISeededRng.
    /// </summary>
    public class WeaponConditionSystem
    {
        public const float Pristine = 1f;
        public const float Ruined = 0f;
        public const int DefaultJamClearTicks = 5;
        public const int BurstJamBaseTicks = 1; // pipe + military ammo burst failure needs one clear

        // Scrap material used for a field repair (authoritative item id).
        public const string ScrapMaterialId = "scrap_metal";

        // ── Public CAM ──────────────────────────────────────────────────

        /// <summary>
        /// The exact jam chance the sim uses when this weapon fires. Condition,
        /// jury-rigging, poor condition, incompatible ammo and the environment
        /// all feed in. Exposed to the UI as the same number.
        /// </summary>
        public static float ComputeJamChance(WeaponInstanceState weapon)
        {
            if (weapon == null) return 0f;
            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null) return 0f;

            float chance = def.jamBase;

            // Poor condition raises jam risk steeply below the threshold.
            float cond = MathfCompat.Clamp01(weapon.ConditionPct);
            if (cond < def.conditionThreshold)
            {
                float shortfall = def.conditionThreshold - cond;
                chance += shortfall * 1.2f; // up to +0.36 for a fully shot-out gun
            }

            // Jury-rigged firearms are naturally finicky.
            if (def.isJuryRigged) chance += 0.03f;

            // Incompatible / heaviest military ammunition in an improvised action.
            if (WAmmoIsMilitary(weapon.AmmoId) && def.isJuryRigged)
                chance += 0.08f;

            // Environmental fouling (ash / contamination) raises jam risk.
            if (weapon.AshFoul > 0f)
                chance += MathfCompat.Clamp01(weapon.AshFoul) * 0.6f;

            return MathfCompat.Clamp01(chance);
        }

        /// <summary>Derive the condition degradation from firing one burst of a weapon.</summary>
        public static float ComputeDegradePerBurst(WeaponInstanceState weapon)
        {
            if (weapon == null) return 0f;
            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null) return 0f;
            int burst = Math.Max(1, def.burst);
            return def.degradePerShot * burst;
        }

        /// <summary>Roll whether this shot jams, and if so mark the weapon jammed. Mutates weapon.</summary>
        public static bool TryJammed(WeaponInstanceState weapon, ISeededRng rng)
        {
            if (weapon == null || rng == null) return false;
            if (weapon.IsJammed) return true; // already jammed cannot fire
            float chance = ComputeJamChance(weapon);
            weapon.CachedJamChance = chance;
            if (rng.NextDouble() < chance)
            {
                weapon.IsJammed = true;
                weapon.JamClearTicksRemaining = DefaultJamClearTicks;
                return true;
            }
            return false;
        }

        /// <summary>Reduce a weapon's condition, never below zero. Returns the new condition.</summary>
        public static float Degrade(WeaponInstanceState weapon, float amount)
        {
            if (weapon == null) return 0f;
            weapon.ConditionPct = MathfCompat.Max(Ruined, weapon.ConditionPct - amount);
            return weapon.ConditionPct;
        }

        /// <summary>Environmental exposure (Ash Dunes / contamination) clogs and degrades a firearm.</summary>
        public static float ExposeToAsh(WeaponInstanceState weapon, float severity = 1f)
        {
            if (weapon == null) return 0f;
            // Ash clogs the action: severe condition loss + high jam risk.
            float loss = 0.35f * MathfCompat.Clamp01(severity);
            weapon.ConditionPct = MathfCompat.Max(Ruined, weapon.ConditionPct - loss);
            weapon.AshFoul = MathfCompat.Clamp01(weapon.AshFoul + severity);
            weapon.CachedJamChance = ComputeJamChance(weapon);
            // A clogged action is functionally jammed until cleaned/cleared.
            weapon.IsJammed = true;
            weapon.JamClearTicksRemaining = Math.Max(weapon.JamClearTicksRemaining, 2);
            return weapon.ConditionPct;
        }

        /// <summary>
        /// Pipe weapon fired with military ammo may burst — cripple the hand and
        /// wreck the weapon. Deterministic via rng.
        /// </summary>
        public static bool TryWeaponBurst(WeaponInstanceState weapon, ISeededRng rng)
        {
            if (weapon == null || rng == null) return false;
            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null || !def.isJuryRigged) return false;
            if (!WAmmoIsMilitary(weapon.AmmoId)) return false;

            const float burstChance = 0.50f; // Hazard_WeaponBurst default
            if (rng.NextDouble() < burstChance)
            {
                // Burst: weapon blown out of the hand, obliterates the firearm.
                weapon.ConditionPct = 0.05f;
                weapon.IsJammed = true;
                weapon.JamClearTicksRemaining = BurstJamBaseTicks;
                return true;
            }
            return false;
        }

        /// <summary>Cost in scrap to field-repair this weapon to restored integrity.</summary>
        public static int GetScrapRepairCost(WeaponInstanceState weapon)
        {
            if (weapon == null) return 0;
            var def = CombatCatalog.GetWeapon(weapon.WeaponId);
            if (def == null) return 0;
            // Cost scales with how far the weapon has degraded.
            float gap = 1f - MathfCompat.Clamp01(weapon.ConditionPct);
            int cost = Math.Max(1, (int)Math.Ceiling(def.scrapRepairCost * (0.5f + gap)));
            return cost;
        }

        /// <summary>
        /// Field repair consumes real scrap via the host port and restores the
        /// weapon to full condition, clearing any jam. Returns false when scrap
        /// cannot be afforded.
        /// </summary>
        public bool TryFieldRepair(WeaponInstanceState weapon, CombatHostPorts ports, Func<string, int, bool> consume = null)
        {
            if (weapon == null) return false;
            int cost = GetScrapRepairCost(weapon);
            var consumer = consume ?? ports?.ConsumeItem;
            if (consumer == null) return false;

            // Incompatible / meltdown-tier ammo contributes nothing to repair.
            if (!consumer(ScrapMaterialId, cost))
                return false;

            weapon.ConditionPct = Pristine;
            weapon.IsJammed = false;
            weapon.JamClearTicksRemaining = 0;
            weapon.CachedJamChance = ComputeJamChance(weapon);
            return true;
        }

        /// <summary>
        /// Clearing a jam consumes ticks; perks can cut this to 1. Returns true
        /// once the jam is fully cleared.
        /// </summary>
        public static bool TickJamClear(WeaponInstanceState weapon, int ticksToClear)
        {
            if (weapon == null || !weapon.IsJammed) return false;
            int effective = ticksToClear > 0 ? ticksToClear : DefaultJamClearTicks;
            if (weapon.JamClearTicksRemaining > effective)
            {
                weapon.JamClearTicksRemaining = effective;
                return false;
            }
            weapon.JamClearTicksRemaining = 0;
            weapon.IsJammed = false;
            return true;
        }

        /// <summary>Completely clear a jam immediately (used by workbench / action).</summary>
        public static void ClearJam(WeaponInstanceState weapon)
        {
            if (weapon == null) return;
            weapon.IsJammed = false;
            weapon.JamClearTicksRemaining = 0;
        }

        private static bool WAmmoIsMilitary(string ammoId)
        {
            var a = CombatCatalog.GetAmmo(ammoId);
            return a != null && a.isMilitaryTier;
        }
    }
}
