using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Radiation;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for the Needs and Radiation systems: pure C#, no scene
    /// required.
    /// </summary>
    [TestFixture]
    public class NeedsStubTests
    {
        private NeedsProfile _profile;
        private NeedsSystem _needsSystem;

        [SetUp]
        public void SetUp()
        {
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _profile.hungerPerHour = 2f;
            _profile.thirstPerHour = 3f;
            _profile.fatiguePerHour = 1.5f;
            _profile.warmthLossPerHourInCold = 4f;
            _profile.warmthRestorePerHourNearHeat = 6f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 10f;
            _profile.healthLossFromHunger = 3f;
            _profile.healthLossFromThirst = 4f;
            _profile.healthLossFromCold = 2f;
            _profile.moraleLossPerHourWhileCritical = 1f;

            _needsSystem = new NeedsSystem(_profile);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_profile);
        }

        private static Survivor MakeSurvivor(string id = "sv_test")
        {
            return new Survivor { Id = id, DisplayName = "Test Survivor" };
        }

        [Test]
        public void NeedsSystem_Tick_IncreasesHungerThirstFatigueOverTime()
        {
            var survivor = MakeSurvivor();

            _needsSystem.Tick(survivor, 2f);

            Assert.AreEqual(4f, survivor.Needs.Hunger, 0.001f);
            Assert.AreEqual(6f, survivor.Needs.Thirst, 0.001f);
            Assert.AreEqual(3f, survivor.Needs.Fatigue, 0.001f);
        }

        [Test]
        public void NeedsSystem_Tick_DecreasesWarmthWhenNoHeatSource()
        {
            var survivor = MakeSurvivor();

            _needsSystem.Tick(survivor, 1f);

            Assert.AreEqual(96f, survivor.Needs.Warmth, 0.001f);
        }

        [Test]
        public void NeedsSystem_Tick_RestoresWarmthNearHeatSource()
        {
            var warmSystem = new NeedsSystem(_profile, _ => true);
            var survivor = MakeSurvivor();
            survivor.Needs.Warmth = 50f;

            warmSystem.Tick(survivor, 1f);

            Assert.AreEqual(56f, survivor.Needs.Warmth, 0.001f);
        }

        [Test]
        public void NeedsSystem_Modify_FiresOnNeedCriticalOnceOnTransition()
        {
            var survivor = MakeSurvivor();
            int criticalFireCount = 0;
            _needsSystem.OnNeedCritical += (s, kind) =>
            {
                if (kind == NeedKind.Hunger) criticalFireCount++;
            };

            _needsSystem.Modify(survivor, NeedKind.Hunger, 100f);
            _needsSystem.Tick(survivor, 1f); // still critical afterwards; must not re-fire

            Assert.AreEqual(1, criticalFireCount);
        }

        [Test]
        public void NeedsSystem_Modify_KillsSurvivorWhenHealthReachesZero()
        {
            var survivor = MakeSurvivor();
            Survivor died = null;
            _needsSystem.OnDied += s => died = s;

            _needsSystem.Modify(survivor, NeedKind.Health, -100f);

            Assert.AreEqual(SurvivorState.Dead, survivor.State);
            Assert.IsFalse(survivor.IsAlive);
            Assert.AreSame(survivor, died);
        }

        [Test]
        public void NeedsSystem_BulkTick_AdvancesAllRegisteredSurvivors()
        {
            var a = MakeSurvivor("sv_a");
            var b = MakeSurvivor("sv_b");
            _needsSystem.Register(a);
            _needsSystem.Register(b);

            _needsSystem.Tick(1f);

            Assert.AreEqual(2f, a.Needs.Hunger, 0.001f);
            Assert.AreEqual(2f, b.Needs.Hunger, 0.001f);
        }

        [Test]
        public void RadiationSystem_Expose_AccumulatesDoseAndLifetimeExposure()
        {
            var radiationSystem = new RadiationSystem(_needsSystem);
            var survivor = MakeSurvivor();

            radiationSystem.Expose(survivor, 5f, 3f);

            Assert.AreEqual(15f, survivor.RadiationDose, 0.001f);
            Assert.AreEqual(15f, survivor.LifetimeRadiationExposure, 0.001f);
        }

        [Test]
        public void RadiationSystem_Expose_AppliesHealthLossAtAcuteThreshold()
        {
            var radiationSystem = new RadiationSystem(_needsSystem);
            var survivor = MakeSurvivor();

            radiationSystem.Expose(survivor, 80f, 1f);

            Assert.GreaterOrEqual(survivor.RadiationDose, RadiationSystem.AcuteThreshold);
            Assert.AreEqual(100f - RadiationSystem.HealthLossPerHourAtAcute, survivor.Needs.Health, 0.001f);
            Assert.IsTrue(survivor.HasAcuteRadiationSickness);
        }

        [Test]
        public void RadiationSystem_Expose_GrantsChronicIllnessAfterLifetimeThreshold()
        {
            var radiationSystem = new RadiationSystem(_needsSystem);
            var survivor = MakeSurvivor();

            for (int i = 0; i < 7; i++)
            {
                radiationSystem.Expose(survivor, 50f, 1f);
            }
            Assert.IsFalse(survivor.HasChronicIllness);

            radiationSystem.Expose(survivor, 50f, 1f);

            Assert.IsTrue(survivor.HasChronicIllness);
            Assert.AreEqual(RadiationSystem.ChronicLifetimeThreshold, survivor.LifetimeRadiationExposure, 0.001f);
        }

        [Test]
        public void Needs_JsonUtility_RoundTripsAllFields()
        {
            var needs = new Needs
            {
                Hunger = 42f,
                Thirst = 33f,
                Fatigue = 12f,
                Warmth = 88f,
                Morale = 60f,
                Health = 77f,
                WasHungerCritical = true,
                WasThirstCritical = false,
                WasWarmthCritical = true
            };

            string json = JsonUtility.ToJson(needs);
            var restored = JsonUtility.FromJson<Needs>(json);

            Assert.AreEqual(needs.Hunger, restored.Hunger, 0.001f);
            Assert.AreEqual(needs.Thirst, restored.Thirst, 0.001f);
            Assert.AreEqual(needs.Fatigue, restored.Fatigue, 0.001f);
            Assert.AreEqual(needs.Warmth, restored.Warmth, 0.001f);
            Assert.AreEqual(needs.Morale, restored.Morale, 0.001f);
            Assert.AreEqual(needs.Health, restored.Health, 0.001f);
            Assert.AreEqual(needs.WasHungerCritical, restored.WasHungerCritical);
            Assert.AreEqual(needs.WasThirstCritical, restored.WasThirstCritical);
            Assert.AreEqual(needs.WasWarmthCritical, restored.WasWarmthCritical);
        }

        [Test]
        public void Survivor_JsonUtility_RoundTripsStateRadiationAndStatusFields()
        {
            var survivor = MakeSurvivor("sv_dose_test");
            survivor.State = SurvivorState.Sick;
            survivor.RadiationDose = 85f;
            survivor.LifetimeRadiationExposure = 420f;
            survivor.HasAcuteRadiationSickness = true;
            survivor.HasChronicIllness = true;

            string json = JsonUtility.ToJson(survivor);
            var restored = JsonUtility.FromJson<Survivor>(json);

            Assert.AreEqual(survivor.Id, restored.Id);
            Assert.AreEqual(survivor.State, restored.State);
            Assert.AreEqual(survivor.RadiationDose, restored.RadiationDose, 0.001f);
            Assert.AreEqual(survivor.LifetimeRadiationExposure, restored.LifetimeRadiationExposure, 0.001f);
            Assert.IsTrue(restored.HasAcuteRadiationSickness);
            Assert.IsTrue(restored.HasChronicIllness);
        }
    }
}
