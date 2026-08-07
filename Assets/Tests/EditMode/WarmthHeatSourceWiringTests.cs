using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// SURV-001 — NeedsSystem's isNearHeatSource predicate was hardcoded to
    /// `sv => true`, so every survivor counted as standing next to a fire and the
    /// nuclear-winter cold hazard never fired. These tests pin the real predicate:
    /// warmth recovers only when it is actually warm where the survivor is standing.
    /// </summary>
    [TestFixture]
    public class WarmthHeatSourceWiringTests
    {
        private GameObject _go;
        private GameBootstrap _bootstrap;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("WarmthHeatSourceWiringTests");
            _go.SetActive(false);
            _bootstrap = _go.AddComponent<GameBootstrap>();

            RegistryDispatchWiringTests.InjectBootstrapFields(_bootstrap);
            var init = typeof(GameBootstrap).GetMethod(
                "InitializeSystems", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(init, "InitializeSystems must exist.");
            init.Invoke(_bootstrap, null);
            _go.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            _bootstrap = null;
        }

        [Test]
        public void ColdBunkerWithNoHeater_DrainsWarmth()
        {
            var temp = _bootstrap.TemperatureSystem;
            Assume.That(temp, Is.Not.Null);
            temp.SetAmbient(-20f); // deep nuclear winter, no heater installed

            var survivor = new Survivor { Id = "sv_cold", DisplayName = "Cold" };
            survivor.Needs.Warmth = 100f;

            _bootstrap.NeedsSystem.Tick(new List<Survivor> { survivor }, 4f);

            Assert.That(survivor.Needs.Warmth, Is.LessThan(100f),
                "A survivor in an unheated bunker at -20 C must lose warmth");
        }

        [Test]
        public void WarmBunker_RestoresWarmth()
        {
            var temp = _bootstrap.TemperatureSystem;
            Assume.That(temp, Is.Not.Null);
            temp.SetAmbient(TemperatureSystem.WarmthComfortCelsius + 5f);

            var survivor = new Survivor { Id = "sv_warm", DisplayName = "Warm" };
            survivor.Needs.Warmth = 40f;

            _bootstrap.NeedsSystem.Tick(new List<Survivor> { survivor }, 4f);

            Assert.That(survivor.Needs.Warmth, Is.GreaterThan(40f),
                "Above the comfort threshold warmth must recover");
        }

        [Test]
        public void IsWarmEnoughForRecovery_TracksTheComfortThreshold()
        {
            var weather = new WeatherSystem(null, 7);
            var temp = new TemperatureSystem(null, weather);
            var survivor = new Survivor { Id = "sv", DisplayName = "sv" };
            var shelter = new ShelterClass();

            temp.SetAmbient(TemperatureSystem.WarmthComfortCelsius - 0.5f);
            Assert.IsFalse(temp.IsWarmEnoughForRecovery(survivor, shelter),
                "Just below the threshold is still cold");

            temp.SetAmbient(TemperatureSystem.WarmthComfortCelsius);
            Assert.IsTrue(temp.IsWarmEnoughForRecovery(survivor, shelter),
                "The threshold itself counts as warm enough");
        }
    }
}
