using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Combat
{
    /// <summary>Deterministic ballistic hit-path result — full audit of one shot.</summary>
    [Serializable]
    public class BallisticOutcome
    {
        // Shooter / weapon / ammo
        public string ShooterId = string.Empty;
        public string ShooterName = string.Empty;
        public string WeaponId = string.Empty;
        public string WeaponName = string.Empty;
        public string AmmoId = string.Empty;
        public string AmmoName = string.Empty;
        // Targets
        public string IntendedTargetId = string.Empty;
        public string ResolvedTargetId = string.Empty;
        public string ResolvedSurfaceId = string.Empty; // cover/barrier material
        public bool IsPlayerShooter;
        // Result / reason
        public BallisticResult Result;
        public BallisticReason Reason;
        public string ReasonCode => Reason.ToString();
        // Energy accounting
        public float InitialEnergy;
        public float DamageDealt;
        public float ArmorAbsorbed;
        public float CoverAbsorbed;
        public float BarrierAbsorbed;
        public float EnergyRemaining;      // redirected/outgoing energy after the path
        public bool IsCritical;
        // The ordered hit path for the combat log (DirectHit/Blocked/Penetrate/Ricochet/...)
        public List<string> Path = new List<string>();

        public void PushStep(string step) => Path.Add(step);
    }

    /// <summary>Immutable inputs to one ballistic resolution.</summary>
    public class BallisticContext
    {
        public string ShooterId = string.Empty;
        public string ShooterName = string.Empty;
        public bool IsPlayerShooter;
        public string WeaponId = string.Empty;
        public string WeaponName = string.Empty;
        public float WeaponAccuracy = 0.6f;
        public float WeaponDamage = 12f;
        public float WeaponRangeMod = 1f;
        public string AmmoId = string.Empty;
        public string AmmoName = string.Empty;
        public float AmmoDamageMod = 1f;
        public float AmmoRangeMod = 1f;
        public float StanceAccuracyMod = 1f;
        public float StanceDamageMod = 1f;
        public float ExternalAccuracyMod = 1f;   // perks / condition
        public float ExternalDamageMod = 1f;     // perks / close quarters / flanking
        public bool IsFirstShotCritBonus;        // Cold Bore
        public float ExtraCritChance;

        public CombatantState IntendedTarget;
        public CombatMaterialDefinition CoverMaterial;   // cover in target's lane (optional)
        public CombatMaterialDefinition ArmorMaterial;   // worn armor (optional)
        public CombatMaterialDefinition BarrierMaterial; // lane barrier (optional)
        public float BarrierIntegrityPct = 100f;
        public List<CombatantState> RicochetTargets = new List<CombatantState>(); // adjacent-lane living enemies
    }

    /// <summary>
    /// Pure, engine-agnostic, deterministic ballistic resolver using an abstract
    /// ordered hit-path model — no 3D physics engine. Each roll consumes from the
    /// injected ISeededRng in a fixed order.
    ///
    /// Hit path: accuracy → (cover intercept/graze) → (barrier) → (armor) →
    /// (bounded ricochet to a deterministic secondary target) → resolve.
    /// </summary>
    public static class BallisticsSystem
    {
        public const int MaxRicochetCount = 2;
        public const float MinResidualEnergy = 2f;

        public static BallisticOutcome Resolve(BallisticContext ctx, ISeededRng rng)
        {
            var o = new BallisticOutcome
            {
                ShooterId = ctx.ShooterId,
                ShooterName = ctx.ShooterName,
                IsPlayerShooter = ctx.IsPlayerShooter,
                WeaponId = ctx.WeaponId,
                WeaponName = ctx.WeaponName,
                AmmoId = ctx.AmmoId,
                AmmoName = ctx.AmmoName,
                IntendedTargetId = ctx.IntendedTarget != null ? ctx.IntendedTarget.Id : string.Empty,
                ResolvedTargetId = ctx.IntendedTarget != null ? ctx.IntendedTarget.Id : string.Empty
            };

            if (ctx.IntendedTarget == null)
            {
                o.Result = BallisticResult.Stopped;
                o.Reason = BallisticReason.None;
                o.PushStep("no_target");
                return o;
            }

            // 1. Accuracy.
            float rangePenalty = RangePenalty(ctx);
            float accuracy = ctx.WeaponAccuracy * ctx.StanceAccuracyMod * ctx.ExternalAccuracyMod * ctx.AmmoRangeMod * (1f - rangePenalty);
            accuracy = MathfCompat.Clamp01(accuracy);

            if (rng.NextDouble() >= accuracy)
            {
                o.Result = BallisticResult.Missed;
                o.Reason = BallisticReason.AccuracyFail;
                o.PushStep("miss");
                return o;
            }
            o.PushStep("aim_hit");

            // 2. Initial energy (the momentum of the round).
            float energy = ctx.WeaponDamage * ctx.AmmoDamageMod * ctx.StanceDamageMod * ctx.ExternalDamageMod;
            o.InitialEnergy = energy;

            // Cold Bore first-shot crit.
            bool crit = false;
            if (ctx.IsFirstShotCritBonus)
            {
                if (rng.NextDouble() < MathfCompat.Clamp01(ctx.ExtraCritChance))
                {
                    crit = true;
                    energy *= 2f;
                }
            }

            CombatantState currentTarget = ctx.IntendedTarget;
            int ricochets = 0;
            int guard = 0;

            while (energy > 0f && guard++ < (MaxRicochetCount + 4))
            {
                // ----- Resolve one target exactly once -----
                // Cover intercept / graze (happens once per target).
                if (ctx.CoverMaterial != null && currentTarget.CoverRating > 0f)
                {
                    if (rng.NextDouble() < (currentTarget.CoverRating * 0.5f))
                    {
                        // Shot smacks into cover and is stopped.
                        o.CoverAbsorbed += energy;
                        o.EnergyRemaining = 0f;
                        o.ResolvedSurfaceId = ctx.CoverMaterial.id;
                        o.Result = BallisticResult.Blocked;
                        o.Reason = BallisticReason.CoverBlocked;
                        o.PushStep("cover_blocked");
                        o.IsCritical = crit;
                        return o;
                    }
                    if (rng.NextDouble() < (currentTarget.CoverRating * 0.5f))
                    {
                        // Graze: cover absorbs a fraction, the round penetrates.
                        float absorbed = energy * ctx.CoverMaterial.armorReduction;
                        o.CoverAbsorbed += absorbed;
                        energy -= absorbed;
                        o.ResolvedSurfaceId = ctx.CoverMaterial.id;
                        o.Result = BallisticResult.Penetrated;
                        o.Reason = BallisticReason.CoverPenetrated;
                        o.PushStep("cover_penetrate");
                    }
                }

                // Barrier.
                if (energy > 0f && ctx.BarrierMaterial != null && ctx.BarrierIntegrityPct > 0f)
                {
                    float barrierAbsorb = energy * ctx.BarrierMaterial.armorReduction;
                    if (energy - barrierAbsorb <= MinResidualEnergy)
                    {
                        o.BarrierAbsorbed += energy;
                        o.EnergyRemaining = 0f;
                        o.ResolvedSurfaceId = ctx.BarrierMaterial.id;
                        o.Result = BallisticResult.Blocked;
                        o.Reason = BallisticReason.BarrierBlocked;
                        o.PushStep("barrier_blocked");
                        o.IsCritical = crit;
                        return o;
                    }
                    o.BarrierAbsorbed += barrierAbsorb;
                    energy -= barrierAbsorb;
                    o.ResolvedSurfaceId = ctx.BarrierMaterial.id;
                    o.Result = BallisticResult.Penetrated;
                    o.Reason = BallisticReason.BarrierPenetrated;
                    o.PushStep("barrier_penetrate");
                }

                // Worn armor.
                if (energy > 0f && ctx.ArmorMaterial != null && ctx.ArmorMaterial.armorReduction > 0f)
                {
                    float armorAbsorb = energy * ctx.ArmorMaterial.armorReduction;
                    if (energy - armorAbsorb <= MinResidualEnergy)
                    {
                        o.ArmorAbsorbed += energy;
                        o.EnergyRemaining = 0f;
                        o.Result = BallisticResult.Stopped;
                        o.Reason = BallisticReason.ArmorAbsorbed;
                        o.PushStep("armor_absorbed");
                        o.IsCritical = crit;
                        return o;
                    }
                    o.ArmorAbsorbed += armorAbsorb;
                    energy -= armorAbsorb;
                    o.PushStep("armor_partial");
                }

                if (energy <= 0f)
                {
                    o.EnergyRemaining = 0f;
                    o.Result = BallisticResult.Stopped;
                    o.Reason = BallisticReason.EnergyExhausted;
                    o.PushStep("energy_exhausted");
                    o.IsCritical = crit;
                    return o;
                }

                // Ricochet — bounded chain to a deterministic secondary target.
                float ricochetChance = RicochetChance(ctx);
                if (ricochetChance > 0f && rng.NextDouble() < ricochetChance
                    && ricochets < MaxRicochetCount
                    && ctx.RicochetTargets != null && ctx.RicochetTargets.Count > 0)
                {
                    CombatantState secondary = PickSecondary(ctx, currentTarget, rng);
                    if (secondary != null)
                    {
                        float retained = RicochetRetention(ctx);
                        energy *= retained;
                        o.EnergyRemaining = energy;
                        o.ResolvedTargetId = secondary.Id;
                        o.ResolvedSurfaceId = RicochetSurface(ctx);
                        o.Result = BallisticResult.Ricocheted;
                        o.Reason = BallisticReason.RicochetedToSecondary;
                        o.PushStep("ricochet->" + secondary.Id);
                        currentTarget = secondary;
                        ricochets++;
                        // New target: re-evaluate its cover/armor next loop.
                        continue;
                    }
                }

                // No ricochet: resolve directly against the current target.
                if (energy <= MinResidualEnergy)
                {
                    o.EnergyRemaining = 0f;
                    o.Result = BallisticResult.Stopped;
                    o.Reason = BallisticReason.EnergyExhausted;
                    o.PushStep("energy_fizzle");
                    o.IsCritical = crit;
                    return o;
                }

                o.DamageDealt = energy;
                o.EnergyRemaining = 0f;
                o.ResolvedTargetId = currentTarget.Id;
                o.Result = BallisticResult.DirectHit;
                o.Reason = (ricochets > 0)
                    ? BallisticReason.RicochetedToSecondary
                    : BallisticReason.None;
                o.PushStep("direct_hit");
                o.IsCritical = crit;
                return o;
            }

            // Guard exhausted — treat as stopped (deterministic, no infinite loop).
            o.EnergyRemaining = 0f;
            o.Result = BallisticResult.Stopped;
            o.Reason = BallisticReason.EnergyExhausted;
            o.PushStep("guard_stop");
            o.IsCritical = crit;
            return o;
        }

        /// <summary>Off-range penalty: shots get less accurate the further off their ideal.</summary>
        private static float RangePenalty(BallisticContext ctx)
        {
            // Models an engagement near the weapon's ideal range; mild penalty when
            // the ammo range mod does not match the weapon's ideal.
            float ideal = ctx.WeaponRangeMod > 0f ? ctx.WeaponRangeMod : 1f;
            float off = Math.Abs(ideal - ctx.AmmoRangeMod);
            return MathfCompat.Clamp01(off * 0.15f);
        }

        private static float RicochetChance(BallisticContext ctx)
        {
            // Cover that was penetrated provides the ricochet surface; armor also
            // contributes a minor deflect chance.
            float chance = 0f;
            if (ctx.CoverMaterial != null) chance = Math.Max(chance, ctx.CoverMaterial.ricochetChance);
            if (ctx.ArmorMaterial != null) chance = Math.Max(chance, ctx.ArmorMaterial.ricochetChance * 0.5f);
            return chance;
        }

        private static float RicochetRetention(BallisticContext ctx)
        {
            float ret = 0.6f; // default
            if (ctx.CoverMaterial != null) ret = Math.Max(ret, ctx.CoverMaterial.ricochetEnergyRetained);
            if (ctx.ArmorMaterial != null) ret = Math.Max(ret, ctx.ArmorMaterial.ricochetEnergyRetained);
            return MathfCompat.Clamp01(ret);
        }

        private static string RicochetSurface(BallisticContext ctx)
        {
            if (ctx.CoverMaterial != null) return ctx.CoverMaterial.id;
            if (ctx.ArmorMaterial != null) return ctx.ArmorMaterial.id;
            return "surface_ground";
        }

        /// <summary>Deterministic secondary target: prefer an adjacent lane, then first living enemy there.</summary>
        private static CombatantState? PickSecondary(BallisticContext ctx, CombatantState current, ISeededRng rng)
        {
            if (ctx.RicochetTargets == null || ctx.RicochetTargets.Count == 0) return null;

            // Sort deterministically so iteration order can't vary across hosts.
            var candidates = new List<CombatantState>(ctx.RicochetTargets);
            candidates.Sort((a, b) => string.CompareOrdinal(a.Id, b.Id));
            // Exclude the current target and anything already downed.
            candidates.RemoveAll(c => c == null || (current != null && c.Id == current.Id) || c.IsDowned);
            if (candidates.Count == 0) return null;
            int idx = rng.Next(0, candidates.Count);
            return candidates[idx];
        }
    }
}
