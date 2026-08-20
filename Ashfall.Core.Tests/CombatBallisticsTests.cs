using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests
{
    public class CombatBallisticsTests
    {
        private static BallisticContext Base(StyledTarget target)
        {
            CombatCatalog.SeedDefaults();
            return new BallisticContext
            {
                ShooterId = "p1",
                ShooterName = "Yuki",
                IsPlayerShooter = true,
                WeaponId = "weapon_assault_rifle",
                WeaponName = "Assault Rifle",
                WeaponAccuracy = 1f,          // deterministically "aimed" in these tests
                WeaponDamage = 20f,
                WeaponRangeMod = 1f,
                AmmoId = "ammo_556",
                AmmoName = "5.56",
                AmmoDamageMod = 1f,
                AmmoRangeMod = 1f,
                StanceAccuracyMod = 1f,
                StanceDamageMod = 1f,
                IntendedTarget = target.Target,
                CoverMaterial = target.Cover,
                ArmorMaterial = target.Armor,
                BarrierMaterial = target.Barrier,
                RicochetTargets = target.RicochetTargets
            };
        }

        public class StyledTarget
        {
            public CombatantState Target;
            public CombatMaterialDefinition Cover;
            public CombatMaterialDefinition Armor;
            public CombatMaterialDefinition Barrier;
            public List<CombatantState> RicochetTargets = new List<CombatantState>();
        }

        private static CombatantState Hostile(string id, float armor = 0f, float cover = 0f) => new CombatantState
        {
            Id = id,
            Name = "Raider",
            IsPlayer = false,
            Health = 50,
            MaxHealth = 50,
            ArmorRating = armor,
            CoverRating = cover
        };

        [Fact]
        public void DirectHit_DealsFullEnergy()
        {
            var st = new StyledTarget { Target = Hostile("en1") };
            var ctx = Base(st);
            // acc roll consumed first; any value < accuracy=1 => not a miss.
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
            Assert.Equal(BallisticResult.DirectHit, o.Result);
            Assert.Equal(ctx.WeaponDamage, o.DamageDealt, 3);
            Assert.Equal("en1", o.ResolvedTargetId);
            Assert.Contains("direct_hit", o.Path);
        }

        [Fact]
        public void Missed_OnZeroAccuracy()
        {
            var st = new StyledTarget { Target = Hostile("en1") };
            var ctx = Base(st);
            ctx.WeaponAccuracy = 0f; // always >= accuracy => miss
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.2));
            Assert.Equal(BallisticResult.Missed, o.Result);
            Assert.Equal(BallisticReason.AccuracyFail, o.Reason);
        }

        [Fact]
        public void Blocked_ByCover()
        {
            var st = new StyledTarget
            {
                Target = Hostile("en1", cover: 1f),
                Cover = CombatCatalog.GetMaterial("material_concrete")
            };
            var ctx = Base(st);
            // acc(0.5) then caught roll first double queued = 0.1 < (1*0.5) => blocked
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.1));
            Assert.Equal(BallisticResult.Blocked, o.Result);
            Assert.Equal(BallisticReason.CoverBlocked, o.Reason);
            Assert.Contains("cover_blocked", o.Path);
        }

        [Fact]
        public void CoverPenetrates_AndReducesDamage()
        {
            var st = new StyledTarget
            {
                Target = Hostile("en1", cover: 1f),
                Cover = new CombatMaterialDefinition { id = "material_test", kind = "cover", armorReduction = 0.5f, ricochetChance = 0f }
            };
            var ctx = Base(st);
            // acc(0.5), caught(0.9 => not blocked), graze(0.1 => penetrate), ricochet(0.9)
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.9, 0.1, 0.9));
            Assert.Equal(BallisticResult.DirectHit, o.Result);
            Assert.True(o.DamageDealt < ctx.WeaponDamage, "penetration leaves less than full energy");
            Assert.True(o.CoverAbsorbed > 0f, "cover absorbed energy during penetration");
            Assert.Contains("cover_penetrate", o.Path);
        }

        [Fact]
        public void Barrier_Blocks()
        {
            var st = new StyledTarget
            {
                Target = Hostile("en1"),
                Barrier = new CombatMaterialDefinition { id = "material_rebar", kind = "barrier", armorReduction = 1f, ricochetChance = 0f }
            };
            var ctx = Base(st);
            ctx.BarrierIntegrityPct = 100f;
            // acc(0.5); barrier reduces fully => blocked
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
            Assert.Equal(BallisticResult.Blocked, o.Result);
            Assert.Equal(BallisticReason.BarrierBlocked, o.Reason);
        }

        [Fact]
        public void Ricochet_RedirectsEnergyToSecondary()
        {
            var st = new StyledTarget
            {
                Target = Hostile("en1"),
                Cover = new CombatMaterialDefinition { id = "material_metal", kind = "cover", armorReduction = 0f, ricochetChance = 0.6f, ricochetEnergyRetained = 0.7f },
                RicochetTargets = new List<CombatantState> { Hostile("en2") }
            };
            var ctx = Base(st);
            // Target CoverRating 0 => cover steps skipped; cover only supplies ricochet surface.
            ctx.IntendedTarget.CoverRating = 0f;
            // acc(0.5), ricochet(0.1 < 0.6 trigger), then secondary iter ricochet(0.9 >= 0.6 no) => direct hit
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.1, 0.9));
            Assert.Equal(BallisticResult.DirectHit, o.Result);
            Assert.Equal("en2", o.ResolvedTargetId);
            Assert.Equal(BallisticReason.RicochetedToSecondary, o.Reason);
            Assert.Contains("ricochet->en2", o.Path);
            Assert.True(o.DamageDealt < ctx.WeaponDamage, "ricochet retained only a fraction of energy");
        }

        [Fact]
        public void RicochetChains_AreBounded()
        {
            var st = new StyledTarget
            {
                Target = Hostile("en1"),
                Cover = new CombatMaterialDefinition { id = "material_metal", kind = "cover", armorReduction = 0f, ricochetChance = 1.0f, ricochetEnergyRetained = 0.5f },
                RicochetTargets = new List<CombatantState> { Hostile("enA"), Hostile("enB") }
            };
            var ctx = Base(st);
            ctx.IntendedTarget.CoverRating = 0f;
            // force maximal ricochets; ensure chain stops at the cap.
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.0, 0.0, 0.0, 0.0, 0.0));
            int ricochetSteps = 0;
            foreach (var s in o.Path) if (s.StartsWith("ricochet->")) ricochetSteps++;
            Assert.True(ricochetSteps <= BallisticsSystem.MaxRicochetCount,
                $"ricochet chain exceeded bound ({ricochetSteps} > {BallisticsSystem.MaxRicochetCount})");
            Assert.True(o.DamageDealt > 0f, "energy remains to resolve a direct hit after bounded chain");
            Assert.Contains("direct_hit", o.Path);
        }

        [Fact]
        public void Armor_CanStop()
        {
            var st = new StyledTarget
            {
                Target = Hostile("en1"),
                Armor = CombatCatalog.GetMaterial("armor_plate")
            };
            var ctx = Base(st);
            // target armorRating drives the armor step; set it so reduction exceeds residual.
            ctx.IntendedTarget.ArmorRating = 0.9f;
            // acc(0.5); armor reduces => Stopped (energy * (1-0.65) <= residual for low damage)
            ctx.WeaponDamage = 5f;
            var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
            Assert.Equal(BallisticResult.Stopped, o.Result);
            Assert.Equal(BallisticReason.ArmorAbsorbed, o.Reason);
        }
    }
}
