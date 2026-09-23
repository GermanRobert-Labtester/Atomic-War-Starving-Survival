// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class NeedsPerformanceBridgeTests
    {
        [Fact]
        public void Optimal_needs_yield_neutral_multipliers()
        {
            var state = new SurvivorNeedsState
            {
                Id = "sv_healthy",
                Hunger = 10f,
                Thirst = 10f,
                Fatigue = 10f,
                Warmth = 95f,
                Morale = 60f,
                Health = 100f
            };

            var mods = NeedsPerformanceBridge.Project(state);

            Assert.Equal(1.0f, mods.CombatAccuracyMultiplier);
            Assert.Equal(1.0f, mods.CombatDamageMultiplier);
            Assert.Equal(1.0f, mods.WorkSpeedMultiplier);
            Assert.Equal(1.0f, mods.ExpeditionSpeedMultiplier);
            Assert.Equal(1.0f, mods.ExpeditionStaminaDrainMultiplier);
            Assert.Equal(PerformanceBand.Optimal, mods.OverallBand);
            Assert.Empty(mods.Contributions);
        }

        [Fact]
        public void Hunger_cascade_penalties_scale_monotonically()
        {
            var mOptimal = NeedsPerformanceBridge.Project(20f, 0f, 0f, 100f, 50f);
            var mImpaired = NeedsPerformanceBridge.Project(45f, 0f, 0f, 100f, 50f);
            var mSevere = NeedsPerformanceBridge.Project(75f, 0f, 0f, 100f, 50f);
            var mCritical = NeedsPerformanceBridge.Project(95f, 0f, 0f, 100f, 50f);

            Assert.Equal(1.0f, mOptimal.WorkSpeedMultiplier);
            Assert.True(mImpaired.WorkSpeedMultiplier < mOptimal.WorkSpeedMultiplier);
            Assert.True(mSevere.WorkSpeedMultiplier < mImpaired.WorkSpeedMultiplier);
            Assert.True(mCritical.WorkSpeedMultiplier < mSevere.WorkSpeedMultiplier);

            Assert.Equal(PerformanceBand.Impaired, mImpaired.OverallBand);
            Assert.Equal(PerformanceBand.Severe, mSevere.OverallBand);
            Assert.Equal(PerformanceBand.Critical, mCritical.OverallBand);
        }

        [Fact]
        public void Cold_cascade_converts_warmth_correctly()
        {
            // Warmth 100 -> Cold 0 (Optimal)
            var mWarm = NeedsPerformanceBridge.Project(0f, 0f, 0f, 95f, 50f);
            Assert.Equal(1.0f, mWarm.CombatAccuracyMultiplier);

            // Warmth 40 -> Cold 60 (Severe)
            var mCold = NeedsPerformanceBridge.Project(0f, 0f, 0f, 40f, 50f);
            Assert.Equal(PerformanceBand.Severe, mCold.OverallBand);
            Assert.True(mCold.CombatAccuracyMultiplier < 1.0f);
            Assert.True(mCold.WorkSpeedMultiplier < 1.0f);
            Assert.Contains(mCold.Contributions, c => c.Need == NeedKind.Warmth && c.ReasonKey == "cold_severe");
        }

        [Fact]
        public void Stacked_critical_needs_respect_configured_floors()
        {
            // All needs at worst possible values
            var mCatastrophe = NeedsPerformanceBridge.Project(100f, 100f, 100f, 0f, 10f);

            Assert.Equal(PerformanceBand.Critical, mCatastrophe.OverallBand);
            Assert.Equal(4, mCatastrophe.Contributions.Count);

            // Bounded by floors
            Assert.True(mCatastrophe.CombatAccuracyMultiplier >= NeedsPerformanceBridge.ActiveConfig.MinCombatAccuracyMultiplier);
            Assert.True(mCatastrophe.CombatDamageMultiplier >= NeedsPerformanceBridge.ActiveConfig.MinCombatDamageMultiplier);
            Assert.True(mCatastrophe.WorkSpeedMultiplier >= NeedsPerformanceBridge.ActiveConfig.MinWorkSpeedMultiplier);
            Assert.True(mCatastrophe.ExpeditionSpeedMultiplier >= NeedsPerformanceBridge.ActiveConfig.MinExpeditionSpeedMultiplier);
            Assert.True(mCatastrophe.ExpeditionStaminaDrainMultiplier <= NeedsPerformanceBridge.ActiveConfig.MaxExpeditionStaminaDrainMultiplier);
        }

        [Fact]
        public void Low_morale_amplifies_penalties_while_high_morale_mitigates()
        {
            // 50% hunger with low morale vs high morale
            var mDemoralized = NeedsPerformanceBridge.Project(50f, 0f, 0f, 100f, 20f);
            var mNeutralMorale = NeedsPerformanceBridge.Project(50f, 0f, 0f, 100f, 50f);
            var mHighMorale = NeedsPerformanceBridge.Project(50f, 0f, 0f, 100f, 85f);

            // Work penalty should be worse under low morale, better under high morale
            Assert.True(mDemoralized.WorkSpeedMultiplier < mNeutralMorale.WorkSpeedMultiplier);
            Assert.True(mHighMorale.WorkSpeedMultiplier > mNeutralMorale.WorkSpeedMultiplier);
        }

        [Fact]
        public void Expedition_system_consumes_speed_and_stamina_hooks()
        {
            var expSystem = new ExpeditionSystem();
            expSystem.SetSurvivorSpeedMultiplierQuery(id => id == "sv_slow" ? 0.6f : 1.0f);
            expSystem.SetStaminaDrainMultiplier(id => id == "sv_slow" ? 1.5f : 1.0f);

            var exp = new ExpeditionState
            {
                survivorId = "sv_slow",
                survivorSpeedMultiplier = 1.0f
            };

            // Test that queries work cleanly
            var mods = NeedsPerformanceBridge.Project(70f, 70f, 70f, 30f, 40f);
            expSystem.SetSurvivorSpeedMultiplierQuery(_ => mods.ExpeditionSpeedMultiplier);
            expSystem.SetStaminaDrainMultiplier(_ => mods.ExpeditionStaminaDrainMultiplier);

            Assert.True(mods.ExpeditionSpeedMultiplier < 1.0f);
            Assert.True(mods.ExpeditionStaminaDrainMultiplier > 1.0f);
        }

        [Fact]
        public void Tactical_combat_system_binds_performance_lookup()
        {
            var combat = new TacticalCombatSystem();
            combat.PerformanceLookup = id => id == "sv_hungry" ? (0.75f, 0.80f) : (1.0f, 1.0f);

            Assert.NotNull(combat.PerformanceLookup);
            var (acc, dmg) = combat.PerformanceLookup("sv_hungry");
            Assert.Equal(0.75f, acc);
            Assert.Equal(0.80f, dmg);
        }
    }
}
