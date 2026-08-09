using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Keeps dormant hazard classes available without constructing or serializing
    /// them from the production bootstrap.
    /// </summary>
    [TestFixture]
    public class HazardCookOffDemotionTests
    {
        private GameObject _go;
        private GameBootstrap _bootstrap;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("HazardCookOffDemotionTests");
            _go.SetActive(false);
            _bootstrap = _go.AddComponent<GameBootstrap>();
            RegistryDispatchWiringTests.InjectBootstrapFields(_bootstrap);

            MethodInfo initialize = typeof(GameBootstrap).GetMethod(
                "InitializeSystems",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(initialize, Is.Not.Null);
            initialize.Invoke(_bootstrap, null);
            _go.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null)
                Object.DestroyImmediate(_go);
            _bootstrap = null;
        }

        [Test]
        public void Bootstrap_LeavesDormantHazardsUnconstructed_WhileMethaneRemainsLive()
        {
            Assert.That(_bootstrap.HazardCookOff, Is.Null);
            Assert.That(_bootstrap.Registry.IsSystemTicked("hazard_cook_off"), Is.False);
            CollectionAssert.DoesNotContain(_bootstrap.GetUntickedSystemNames(), "HazardCookOff");
            CollectionAssert.DoesNotContain(UntickedSystemsBaseline.Load(), "HazardCookOff");

            Assert.That(_bootstrap.HazardExplosiveCrafting, Is.Null);
            Assert.That(_bootstrap.Registry.IsSystemTicked("hazard_explosive_crafting"), Is.False);
            CollectionAssert.DoesNotContain(
                _bootstrap.GetUntickedSystemNames(),
                "HazardExplosiveCrafting");
            CollectionAssert.DoesNotContain(
                UntickedSystemsBaseline.Load(),
                "HazardExplosiveCrafting");

            Assert.That(_bootstrap.HazardFriendlyFire, Is.Null);
            Assert.That(_bootstrap.Registry.IsSystemTicked("hazard_friendly_fire"), Is.False);
            CollectionAssert.DoesNotContain(
                _bootstrap.GetUntickedSystemNames(),
                "HazardFriendlyFire");
            CollectionAssert.DoesNotContain(
                UntickedSystemsBaseline.Load(),
                "HazardFriendlyFire");

            Assert.That(_bootstrap.HazardMethane, Is.Not.Null);
            Assert.That(_bootstrap.Registry.IsSystemTicked("hazard_methane"), Is.True);
            CollectionAssert.DoesNotContain(_bootstrap.GetUntickedSystemNames(), "HazardMethane");
        }

        [Test]
        public void BootstrapSnapshot_OmitsDormantHazards_ButIncludesMethane()
        {
            MethodInfo capture = typeof(SaveSystem).GetMethod(
                "CaptureSnapshot",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(capture, Is.Not.Null);

            var snapshot = capture.Invoke(_bootstrap.SaveSystem, null) as SaveData;

            Assert.That(snapshot, Is.Not.Null);
            Assert.That(snapshot.SubsystemSaveIds, Is.Not.Null);
            CollectionAssert.DoesNotContain(snapshot.SubsystemSaveIds, "hazard_cook_off");
            CollectionAssert.DoesNotContain(
                snapshot.SubsystemSaveIds,
                "hazard_explosive_crafting");
            CollectionAssert.DoesNotContain(snapshot.SubsystemSaveIds, "hazard_friendly_fire");
            CollectionAssert.Contains(snapshot.SubsystemSaveIds, "hazard_methane");
        }

        [Test]
        public void DormantClass_RemainsConstructibleAndSaveSafe()
        {
            var cookOff = new Hazard_CookOff();
            CookOffState state = cookOff.CaptureState();

            Assert.That(state, Is.Not.Null);
            Assert.That(state.hazardId, Is.EqualTo("hazard_cook_off"));
            Assert.DoesNotThrow(() => cookOff.RestoreState(state));
        }

        [Test]
        public void ExplosiveCraftingClass_RemainsConstructibleAndSaveSafe()
        {
            var explosiveCrafting = new Hazard_ExplosiveCrafting();
            ExplosiveCraftingState state = explosiveCrafting.CaptureState();

            Assert.That(state, Is.Not.Null);
            Assert.That(state.craftingId, Is.EqualTo("hazard_explosive_crafting"));
            Assert.DoesNotThrow(() => explosiveCrafting.RestoreState(state));
        }

        [Test]
        public void FriendlyFireClass_RemainsConstructibleAndSaveSafe()
        {
            var friendlyFire = new Hazard_FriendlyFire();
            FriendlyFireState state = friendlyFire.CaptureState();

            Assert.That(state, Is.Not.Null);
            Assert.That(state.hazardId, Is.EqualTo("hazard_friendly_fire"));
            Assert.DoesNotThrow(() => friendlyFire.RestoreState(state));
        }
    }
}
