using System;
using System.Linq;
using Xunit;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests
{
    public class HostPortContractTests
    {
        [Fact]
        public void NoOp_HasEssentialNoOpAdapters_AndOptionalNull()
        {
            var ports = CombatHostPorts.NoOp();

            // Essential effects fall back to named no-op adapters (never null).
            Assert.NotNull(ports.DamageSurvivor);
            Assert.NotNull(ports.HealSurvivor);
            Assert.NotNull(ports.ApplyMoraleDelta);

            // Optional effects stay null so the sim's null-check fallbacks fire.
            Assert.Null(ports.ConsumeAmmo);
            Assert.Null(ports.ConsumeItem);
            Assert.Null(ports.GrantLoot);
            Assert.Null(ports.MarkCombatSurvived);
            Assert.Null(ports.RaiseTrauma);
        }

        [Fact]
        public void NoOp_ListsAllRequiredEffectsAsUnbound()
        {
            var ports = CombatHostPorts.NoOp();
            var unbound = ports.UnboundRequiredEffects;

            // Every production-required effect is unbound in the NoOp factory.
            Assert.Contains("DamageSurvivor", unbound);
            Assert.Contains("HealSurvivor", unbound);
            Assert.Contains("ApplyMoraleDelta", unbound);
            Assert.Contains("ConsumeAmmo", unbound);
            Assert.Contains("ConsumeItem", unbound);
            Assert.Contains("GrantLoot", unbound);
            Assert.Contains("MarkCombatSurvived", unbound);
            // RaiseTrauma is truly optional — never tracked.
            Assert.DoesNotContain("RaiseTrauma", unbound);
        }

        [Fact]
        public void FullyBoundPort_HasNoUnboundRequiredEffects()
        {
            var ports = new CombatHostPorts(
                damageSurvivor: (id, d) => d,
                healSurvivor: (id, h) => h,
                applyMoraleDelta: (id, d) => { },
                consumeAmmo: (id, n) => n,
                consumeItem: (id, n) => true,
                raiseTrauma: (id, k, s) => { },
                grantLoot: l => { },
                markCombatSurvived: id => { });

            Assert.Empty(ports.UnboundRequiredEffects);
        }

        [Fact]
        public void PartiallyBoundPort_ListsOnlyUnboundEffects()
        {
            var ports = new CombatHostPorts(
                damageSurvivor: (id, d) => d,
                healSurvivor: null,
                applyMoraleDelta: (id, d) => { },
                consumeAmmo: (id, n) => n,
                consumeItem: null,
                grantLoot: null,
                markCombatSurvived: id => { });

            var unbound = ports.UnboundRequiredEffects;
            Assert.Equal(new[] { "HealSurvivor", "ConsumeItem", "GrantLoot" }, unbound.ToArray());
        }

        [Fact]
        public void NoOpEssentialAdapters_AreBenign()
        {
            // The no-op adapters must not throw and return benign values.
            Assert.Equal(5f, CombatHostPorts.NoOpDamageSurvivor("sv", 5f));
            Assert.Equal(15f, CombatHostPorts.NoOpHealSurvivor("sv", 15f));
            CombatHostPorts.NoOpApplyMoraleDelta("sv", -2f); // does not throw
        }

        [Fact]
        public void NoOp_PreservesCombatFallbackBehavior()
        {
            // A NoOp port must let the sim run with in-encounter bookkeeping
            // (optional ConsumeAmmo null → local ammo decrement fallback).
            var sys = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
            var players = new System.Collections.Generic.List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "sv1", IsPlayer = true, Health = 100, MaxHealth = 100 }
            };
            var weapons = new System.Collections.Generic.List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "w1", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "sv1", ConditionPct = 0.9f, AmmoId = "ammo_556", AmmoRemaining = 60 }
            };
            bool ok = sys.BeginEncounter("enc_t", "exp", "loc", "Loc", 1, 99, players, weapons, 1, 40);
            Assert.True(ok);
            Assert.Equal(CombatPhase.PlayerTurn, (CombatPhase)sys.State.Phase);
        }
    }
}
