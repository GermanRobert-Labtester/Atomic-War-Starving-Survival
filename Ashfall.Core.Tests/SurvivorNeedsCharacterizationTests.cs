using System;
using System.Collections.Generic;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Characterization tests for survivor needs state and NeedsSystem behavior.
    /// These tests lock current behavior before P0-2 host-session authority refactor.
    /// </summary>
    public sealed class SurvivorNeedsCharacterizationTests
    {
        [Fact]
        public void SurvivorNeedsState_DefaultsAreCorrect()
        {
            var s = new SurvivorNeedsState { Id = "sv1" };
            Assert.Equal("sv1", s.Id);
            Assert.Equal(0f, s.Hunger);
            Assert.Equal(0f, s.Thirst);
            Assert.Equal(0f, s.Fatigue);
            Assert.Equal(100f, s.Warmth);
            Assert.Equal(50f, s.Morale);
            Assert.Equal(100f, s.Health);
            Assert.Equal(100f, s.Hygiene);
            Assert.True(s.IsAlive);
            Assert.False(s.IsDead);
            Assert.True(s.IsAliveState);
            Assert.False(s.WasHungerCritical);
            Assert.False(s.WasThirstCritical);
            Assert.False(s.WasWarmthCritical);
        }

        [Fact]
        public void NeedsSystem_RegisterAndGet()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Register(s);
            var found = sys.Get("sv1");
            Assert.Same(s, found);
            Assert.Null(sys.Get("missing"));
        }

        [Fact]
        public void NeedsSystem_UnregisterRemovesSurvivor()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Register(s);
            sys.Unregister(s);
            Assert.Null(sys.Get("sv1"));
        }

        [Fact]
        public void NeedsSystem_TickAdvancesAllNeeds()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Register(s);
            sys.Tick(24f);
            Assert.True(s.Hunger > 0f, "hunger should advance");
            Assert.True(s.Thirst > 0f, "thirst should advance");
            Assert.True(s.Fatigue > 0f, "fatigue should advance");
            Assert.True(s.Warmth < 100f, "warmth should decay without heat");
        }

        [Fact]
        public void NeedsSystem_TickNearHeatRestoresWarmth()
        {
            var sys = new NeedsSystem(null, _ => true);
            var s = new SurvivorNeedsState { Id = "sv1", Warmth = 40f };
            sys.Register(s);
            sys.Tick(10f);
            Assert.True(s.Warmth > 40f, "warmth should restore near heat");
        }

        [Fact]
        public void NeedsSystem_CriticalHungerFiresEventAndHurtsHealth()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Hunger = 95f, Health = 100f };
            bool criticalFired = false;
            bool died = false;
            sys.OnNeedCritical += (_, kind) => { if (kind == NeedKind.Hunger) criticalFired = true; };
            sys.OnDied += _ => died = true;
            sys.Register(s);
            sys.Tick(2f);
            Assert.True(criticalFired, "hunger critical event should fire");
            Assert.True(s.Health < 100f, "starving should hurt health");
            Assert.False(died, "should not die from hunger alone in 2h");
        }

        [Fact]
        public void NeedsSystem_DeathAtZeroHealth()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Health = 1f };
            bool died = false;
            sys.OnDied += _ => died = true;
            sys.Register(s);
            sys.Modify(s, NeedKind.Health, -5f);
            Assert.True(died, "death event should fire");
            Assert.True(s.IsDead);
            Assert.False(s.IsAlive);
            Assert.False(s.IsAliveState);
        }

        [Fact]
        public void NeedsSystem_ForceDeath()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Health = 50f };
            bool died = false;
            sys.OnDied += _ => died = true;
            sys.Register(s);
            sys.ForceDeath(s);
            Assert.True(died, "force death should fire event");
            Assert.True(s.IsDead);
            Assert.Equal(0f, s.Health);
        }

        [Fact]
        public void NeedsSystem_SetHealthClampsAndFiresEvent()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Health = 50f };
            float lastHealth = -1f;
            sys.OnNeedChanged += (_, __, h) => lastHealth = h;
            sys.Register(s);
            sys.SetHealth(s, 75f);
            Assert.Equal(75f, s.Health);
            Assert.Equal(75f, lastHealth);
            sys.SetHealth(s, 150f);
            Assert.Equal(100f, s.Health, 1); // clamped to cap
        }

        [Fact]
        public void NeedsSystem_AdjustHealthAddsDelta()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Health = 50f };
            sys.Register(s);
            sys.AdjustHealth(s, 25f);
            Assert.Equal(75f, s.Health);
            sys.AdjustHealth(s, -10f);
            Assert.Equal(65f, s.Health);
        }

        [Fact]
        public void NeedsSystem_NotifyNeedsRestoredFiresAllEvents()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Health = 50f };
            int eventCount = 0;
            sys.OnNeedChanged += (_, __, ___) => eventCount++;
            sys.Register(s);
            sys.NotifyNeedsRestored(s);
            Assert.Equal(7, eventCount); // 7 needs should fire
        }

        [Fact]
        public void NeedsSystem_TickSkipsDeadSurvivors()
        {
            var sys = new NeedsSystem();
            var alive = new SurvivorNeedsState { Id = "alive", Health = 100f };
            var dead = new SurvivorNeedsState { Id = "dead", Health = 0f, IsDead = true };
            sys.Register(alive);
            sys.Register(dead);
            sys.Tick(24f);
            Assert.True(alive.Hunger > 0f, "alive should tick");
            Assert.Equal(0f, dead.Hunger);
        }

        [Fact]
        public void NeedsSystem_ModifyById()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Register(s);
            sys.Modify("sv1", NeedKind.Morale, -20f);
            Assert.Equal(30f, s.Morale);
            sys.Modify("sv1", NeedKind.Hunger, 50f);
            Assert.Equal(50f, s.Hunger);
            sys.Modify("missing", NeedKind.Hunger, 10f); // should not throw
        }

        [Fact]
        public void NeedsSystem_TryDeferDeath()
        {
            bool deferCalled = false;
            var sys = new NeedsSystem();
            sys.TryDeferDeath = s => { deferCalled = true; return true; };
            var s = new SurvivorNeedsState { Id = "sv1", Health = 1f };
            sys.Register(s);
            sys.Modify(s, NeedKind.Health, -5f);
            Assert.True(deferCalled, "defer callback should be consulted");
            Assert.False(s.IsDead, "death should be deferred");
        }

        [Fact]
        public void SurvivorNeedsState_MaxHealthCapConstrainsSetHealth()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", MaxHealthCap = 50f, Health = 50f };
            sys.Register(s);
            sys.SetHealth(s, 75f);
            Assert.Equal(50f, s.Health, 1); // clamped to MaxHealthCap
        }
    }
}
