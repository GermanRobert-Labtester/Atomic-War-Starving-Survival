using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// RAD-001 — RadiationSystem was built with no exposure hook, so every tick saw
    /// zone 0 / no gear / no shelter and nothing but scripted Expose() could dose a
    /// survivor. These tests pin the wired hook: fallout outside reaches survivors,
    /// bunker contamination reaches them, and a clean day does not.
    /// </summary>
    [TestFixture]
    public class RadiationExposureWiringTests
    {
        private GameObject _go;
        private GameBootstrap _bootstrap;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("RadiationExposureWiringTests");
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

        private Survivor RegisterSurvivor(string id = "sv_rad")
        {
            // Bootstrap parks the system paused until a run actually starts.
            _bootstrap.RadiationSystem.IsPaused = false;
            var survivor = new Survivor { Id = id, DisplayName = id };
            _bootstrap.RadiationSystem.Register(survivor);
            return survivor;
        }

        [Test]
        public void CleanDay_InsideTheBunker_DosesNobody()
        {
            var survivor = RegisterSurvivor();

            _bootstrap.RadiationSystem.Tick(24f);

            Assert.That(survivor.RadiationDose, Is.EqualTo(0f).Within(1e-3f),
                "A sealed bunker on a clean day must not irradiate anyone");
        }

        [Test]
        public void BunkerContamination_DosesSurvivorsInside()
        {
            var survivor = RegisterSurvivor();
            _bootstrap.Shelter.AddBunkerContamination(3f);

            _bootstrap.RadiationSystem.Tick(2f);

            Assert.That(survivor.RadiationDose, Is.EqualTo(6f).Within(1e-3f),
                "Contamination on the bunker floor must dose the people standing on it");
        }

        [Test]
        public void FalloutStorm_ReachesSurvivorsThroughAnUnshieldedBunker()
        {
            var survivor = RegisterSurvivor();
            _bootstrap.WeatherSystem.ForceWeather(WeatherKind.FalloutStorm);

            _bootstrap.RadiationSystem.Tick(1f);

            // Fallout-storm dose (interior ≈ 105 rad/hr) crosses ProdromalTriggerDose
            // (100) on a single tick, so PrognosisPipeline.TriggerProdromal intentionally
            // clears the acute RadiationDose reading — only LatentDamage and lifetime
            // exposure carry the injury forward (see PrognosisPipeline.cs TriggerProdromal
            // comment). Assert on the durable signal, not the acute one that the pipeline
            // is by design zeroing.
            Assert.That(survivor.LifetimeRadiationExposure, Is.GreaterThan(0f),
                $"A fallout storm over an unshielded bunker must accumulate exposure. "
                + $"weather={_bootstrap.WeatherSystem.Current} "
                + $"outdoorMod={_bootstrap.WeatherSystem.OutdoorRadModifier} "
                + $"overworldShield={_bootstrap.Shelter.OverworldShieldingBonus} "
                + $"interior={_bootstrap.Shelter.GetInteriorRadsPerHour(150f)} "
                + $"gear={_bootstrap.Inventory.GetEquippedProtection()} "
                + $"ctx={DescribeContext(survivor)} "
                + $"paused={_bootstrap.RadiationSystem.IsPaused} "
                + $"dose={survivor.RadiationDose} latent={survivor.LatentDamage}");
        }

        private string DescribeContext(Survivor survivor)
        {
            var m = typeof(GameBootstrap).GetMethod(
                "BuildExposureContext", BindingFlags.Instance | BindingFlags.NonPublic);
            if (m == null) return "no-method";
            var ctx = m.Invoke(_bootstrap, new object[] { survivor })
                as AtomicWar._Game.Radiation.ExposureContext;
            if (ctx == null) return "null-ctx";
            return $"zone={ctx.ZoneRadLevel} query={(ctx.ShelterRadQuery != null ? ctx.ShelterRadQuery(ctx.ZoneRadLevel).ToString() : "null")} wornCount={ctx.WornGear.Count}";
        }

        [Test]
        public void ExposureHook_IsWiredAtAll()
        {
            // Guard against a regression to `new RadiationSystem(NeedsSystem)`: without
            // the hook every one of the tests above passes vacuously at zero dose.
            var field = typeof(AtomicWar._Game.Radiation.RadiationSystem).GetField(
                "_exposureContext", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, "RadiationSystem must keep an exposure-context hook.");
            Assert.IsNotNull(field.GetValue(_bootstrap.RadiationSystem),
                "GameBootstrap must inject an exposure context, or ambient radiation is off.");
        }
    }
}
