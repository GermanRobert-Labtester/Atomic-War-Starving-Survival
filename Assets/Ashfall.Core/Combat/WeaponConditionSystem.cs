using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Immutable host-port bundle the combat sim uses to reach real
    /// survivor/inventory state. Every hook is set once at construction; there
    /// are no mutable fields to silently leave unwired.
    ///
    /// Effect classification:
    ///  - Essential (health/morale): default to named no-op adapters when null
    ///    so the sim never NullRefs, but are tracked as unbound.
    ///  - Production-required (inventory/progression): stay null when unbound so
    ///    the sim's existing null-check fallbacks (in-encounter bookkeeping)
    ///    keep working; tracked as unbound for startup validation.
    ///  - Truly optional (RaiseTrauma): null when unbound, not tracked.
    ///
    /// Use <see cref="NoOp"/> for isolated tests. Production wiring builds a
    /// fully-bound instance via the constructor and checks
    /// <see cref="UnboundRequiredEffects"/> at startup.
    /// </summary>
    public sealed class CombatHostPorts
    {
        // ── Essential effects (health / morale) — no-op when unbound ──

        /// <summary>Apply damage to a real survivor. Returns the new health (or the input when unset).</summary>
        public Func<string, float, float> DamageSurvivor { get; }
        /// <summary>Apply healing to a real survivor. Returns the new health.</summary>
        public Func<string, float, float> HealSurvivor { get; }
        /// <summary>Apply a morale delta to a real survivor.</summary>
        public Action<string, float> ApplyMoraleDelta { get; }

        // ── Production-required (inventory / progression) — null when unbound ──

        /// <summary>Consume rounds of the given ammo item. Returns -1 if it cannot be afforded, else remaining. Null → in-encounter bookkeeping.</summary>
        public Func<string, int, int> ConsumeAmmo { get; }
        /// <summary>Consume a generic item (scrap, bandage, etc.). Returns true when consumed. Null → action refused.</summary>
        public Func<string, int, bool> ConsumeItem { get; }
        /// <summary>Grant captured loot to the real inventory. Null → loot discarded.</summary>
        public Action<CombatLootEntry> GrantLoot { get; }
        /// <summary>Record that a survivor survived a combat encounter (CombatTraumaSystem.OnCombatSurvived). Null → survival unrecorded.</summary>
        public Action<string> MarkCombatSurvived { get; }

        // ── Truly optional — null when unbound, not validated ──

        /// <summary>Raise a trauma/affliction on a real survivor (kind, severity). Null → trauma skipped.</summary>
        public Action<string, string, float> RaiseTrauma { get; }

        private readonly List<string> _unboundRequired;

        /// <summary>
        /// Names of production-required effects that were not explicitly bound
        /// (null passed → no-op/null). Essential effects appear here when they
        /// fell back to a no-op adapter. Empty when fully wired. Checked at
        /// startup so a production system cannot silently run with a missing
        /// health, morale, inventory, or progression effect.
        /// </summary>
        public IReadOnlyList<string> UnboundRequiredEffects => _unboundRequired;

        public CombatHostPorts(
            Func<string, float, float>? damageSurvivor = null,
            Func<string, float, float>? healSurvivor = null,
            Action<string, float>? applyMoraleDelta = null,
            Func<string, int, int>? consumeAmmo = null,
            Func<string, int, bool>? consumeItem = null,
            Action<string, string, float>? raiseTrauma = null,
            Action<CombatLootEntry>? grantLoot = null,
            Action<string>? markCombatSurvived = null)
        {
            DamageSurvivor = damageSurvivor ?? NoOpDamageSurvivor;
            HealSurvivor = healSurvivor ?? NoOpHealSurvivor;
            ApplyMoraleDelta = applyMoraleDelta ?? NoOpApplyMoraleDelta;
            ConsumeAmmo = consumeAmmo;
            ConsumeItem = consumeItem;
            RaiseTrauma = raiseTrauma;
            GrantLoot = grantLoot;
            MarkCombatSurvived = markCombatSurvived;

            _unboundRequired = new List<string>();
            if (damageSurvivor == null) _unboundRequired.Add(nameof(DamageSurvivor));
            if (healSurvivor == null) _unboundRequired.Add(nameof(HealSurvivor));
            if (applyMoraleDelta == null) _unboundRequired.Add(nameof(ApplyMoraleDelta));
            if (consumeAmmo == null) _unboundRequired.Add(nameof(ConsumeAmmo));
            if (consumeItem == null) _unboundRequired.Add(nameof(ConsumeItem));
            if (grantLoot == null) _unboundRequired.Add(nameof(GrantLoot));
            if (markCombatSurvived == null) _unboundRequired.Add(nameof(MarkCombatSurvived));
        }

        /// <summary>All-effects-unbound instance for isolated tests. Essential effects use no-op adapters; optional effects are null (fallbacks active).</summary>
        public static CombatHostPorts NoOp() =>
            new CombatHostPorts(null, null, null, null, null, null, null, null);

        // ── Named no-op adapters (referenced by NoOp and as fallbacks) ──

        /// <summary>Returns the input damage; the return is unused by the sim.</summary>
        public static readonly Func<string, float, float> NoOpDamageSurvivor = (_, d) => d;
        /// <summary>Returns the input heal amount.</summary>
        public static readonly Func<string, float, float> NoOpHealSurvivor = (_, h) => h;
        /// <summary>Does nothing.</summary>
        public static readonly Action<string, float> NoOpApplyMoraleDelta = (_, __) => { };
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
        public bool TryFieldRepair(WeaponInstanceState weapon, CombatHostPorts ports, Func<string, int, bool>? consume = null)
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
