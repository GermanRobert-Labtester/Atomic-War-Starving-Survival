using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class NeedsSystemTests
    {
        [Fact]
        public void Tick_AdvancesHungerThirstFatigue()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Register(s);
            sys.Tick(24f);
            Assert.True(s.Hunger > 0f);
            Assert.True(s.Thirst > 0f);
            Assert.True(s.Fatigue > 0f);
            // Warmth decays in the cold (no heat hook).
            Assert.True(s.Warmth < 100f);
        }

        [Fact]
        public void Tick_NearHeatSource_RestoresWarmth()
        {
            var sys = new NeedsSystem(null, _ => true);
            var s = new SurvivorNeedsState { Id = "sv1", Warmth = 40f };
            sys.Register(s);
            sys.Tick(10f);
            Assert.True(s.Warmth > 40f);
        }

        [Fact]
        public void CriticalHunger_LosesHealthAndFiresEvent()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Hunger = 95f, Health = 100f };
            bool critical = false;
            sys.OnNeedCritical += (_, kind) => { if (kind == NeedKind.Hunger) critical = true; };
            sys.Register(s);
            sys.Tick(2f);
            Assert.True(critical);
            Assert.True(s.Health < 100f); // starving hurts
        }

        [Fact]
        public void Modify_ClampsToCapAnd100()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1" };
            sys.Modify(s, NeedKind.Morale, -999f);
            Assert.Equal(0f, s.Morale);
            sys.Modify(s, NeedKind.Hunger, 999f);
            Assert.Equal(100f, s.Hunger);
            sys.Modify(s, NeedKind.Health, -5f);
            Assert.Equal(95f, s.Health);
        }

        [Fact]
        public void HealthZero_FiresDied()
        {
            var sys = new NeedsSystem();
            var s = new SurvivorNeedsState { Id = "sv1", Health = 5f };
            bool died = false;
            sys.OnDied += _ => died = true;
            sys.SetHealth(s, -1f);
            Assert.True(died);
            Assert.True(s.IsDead);
        }

        [Fact]
        public void TryDeferDeath_GatesDeathAtZero()
        {
            var sys = new NeedsSystem();
            sys.TryDeferDeath = _ => true;
            var s = new SurvivorNeedsState { Id = "sv1", Health = 1f };
            bool died = false;
            sys.OnDied += _ => died = true;
            sys.Modify(s, NeedKind.Health, -10f);
            Assert.False(died);
            Assert.False(s.IsDead);
            Assert.Equal(0f, s.Health);
        }
    }

    public class RadiationSystemTests
    {
        private static SurvivorRadState Sv(string id) => new SurvivorRadState { Id = id };

        [Fact]
        public void Expose_AccumulatesDoseAndLifetime()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.Register(s);
            sys.Expose(s, 10f, 2f);
            Assert.Equal(20f, s.LifetimeRadiationExposure, 3);
            Assert.Equal(20f, s.RadiationDose, 3);
        }

        [Fact]
        public void Expose_AcuteThreshold_GrantsStatusAndDamagesHealth()
        {
            float healthDelta = 0f;
            var sys = new RadiationSystem(applyNeed: (s, need, d) => { if (need == "health") healthDelta = d; });
            var s = Sv("sv1");
            sys.Expose(s, 90f, 1f); // 90 ≥ 80 → acute
            Assert.True(s.HasAcuteRadiationSickness);
            Assert.True(healthDelta < 0f);
        }

        [Fact]
        public void ChronicThreshold_OnLifetime_GrantsChronic()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.SeedLifetimeExposure(s, 450f);
            Assert.True(s.HasChronicIllness);
        }

        [Fact]
        public void AdministerIodine_GrantsTimedResistance_ThatExpires()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.Register(s);
            sys.AdministerIodine(s);
            Assert.True(s.HasRadResistance);
            Assert.Equal(RadiationSystem.IodineResistanceHours, s.RadResistanceHoursRemaining, 3);
            sys.Tick(2f);   // 2h < 6h window: still active
            Assert.True(s.HasRadResistance);
            Assert.Equal(4f, s.RadResistanceHoursRemaining, 3);
            sys.Tick(20f);  // 22h total > 6h: expired
            Assert.False(s.HasRadResistance);
            Assert.Equal(0f, s.RadResistanceHoursRemaining, 3);
        }

        [Fact]
        public void AdministerAntiRad_LowersDose_KeepsLifetime()
        {
            var sys = new RadiationSystem();
            var s = Sv("sv1");
            sys.Expose(s, 50f, 1f);
            sys.AdministerAntiRad(s, 20f);
            Assert.Equal(30f, s.RadiationDose, 3);
            Assert.Equal(50f, s.LifetimeRadiationExposure, 3); // lifetime untouched
        }

        [Fact]
        public void Tick_WithContext_AppliesGearProtection()
        {
            var gear = new WornGear { RadProtection = 30f, MaxDurability = 100f, CurrentDurability = 100f };
            var sys = new RadiationSystem(exposureContext: s => new ExposureContext
            {
                ZoneRadLevel = 50f,
                WornGear = new System.Collections.Generic.List<WornGear> { gear }
            });
            var s = Sv("sv1");
            sys.Register(s);
            sys.Tick(1f);
            // exposure = 50 - 30 = 20 mSv/hr * 1h
            Assert.Equal(20f, s.LifetimeRadiationExposure, 2);
        }

        [Fact]
        public void Tick_Paused_AccumulatesNothing()
        {
            var sys = new RadiationSystem(exposureContext: _ => new ExposureContext { ZoneRadLevel = 50f });
            sys.IsPaused = true;
            var s = Sv("sv1");
            sys.Register(s);
            sys.Tick(5f);
            Assert.Equal(0f, s.LifetimeRadiationExposure);
        }

        [Fact]
        public void GearProtection_ScalesWithDurability()
        {
            var gear = new WornGear { RadProtection = 40f, MaxDurability = 100f, CurrentDurability = 50f };
            Assert.Equal(20f, gear.EffectiveProtection(), 3);
            // ComputeGearProtection sums effective (already durability-scaled) protection.
            Assert.Equal(20f, RadiationSystem.ComputeGearProtection(
                new System.Collections.Generic.List<WornGear> { gear }), 3);
        }

        [Fact]
        public void MathfCompat_MirrorsUnitySemantics()
        {
            Assert.Equal(0f, MathfCompat.Clamp01(-1f));
            Assert.Equal(1f, MathfCompat.Clamp01(2f));
            Assert.Equal(0.5f, MathfCompat.Clamp01(0.5f));
            Assert.Equal(5f, MathfCompat.Clamp(99f, 0f, 5f));
            Assert.Equal(1.5f, MathfCompat.Lerp(1f, 2f, 0.5f), 3);
            Assert.True(MathfCompat.Approximately(0.1f + 0.2f, 0.3f));
        }
    }
}
