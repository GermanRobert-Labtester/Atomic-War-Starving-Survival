using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Behavioural proof that the Weather_* trackers are actually driven by the live
    /// tick path — not merely present in the registry.
    ///
    /// This distinction is the entire point. The existing WeatherWiringTests construct
    /// their own `new Weather_SolarFlare()` and tick it by hand, so they stayed green
    /// the whole time the bootstrap-owned instances were never ticked at all. A
    /// triggered storm would therefore hang active forever: hoursRemaining never
    /// decremented, the "ended" event never fired, and the debuff never lifted.
    ///
    /// These tests go through GameBootstrap's registry so they fail if that dispatch
    /// is ever removed again.
    /// </summary>
    [TestFixture]
    public class WeatherEventDispatchTests
    {
        private GameObject _go;
        private GameBootstrap _bootstrap;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("WeatherEventDispatchTests");
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

        /// <summary>Advance the registry by whole game-hours on a fixed day.</summary>
        private void TickHours(int hours, int day = 1)
        {
            for (int i = 0; i < hours; i++)
                _bootstrap.Registry.TickAll(1f, day);
        }

        [Test]
        public void SolarFlare_OnceTriggered_CountsDownAndEnds()
        {
            var flare = _bootstrap.WeatherSolarFlare;
            Assume.That(flare, Is.Not.Null);

            flare.Trigger();
            float atTrigger = flare.State.hoursRemaining;
            Assert.Greater(atTrigger, 0f, "Trigger must start the countdown.");

            bool ended = false;
            flare.OnSolarFlareEnded += _ => ended = true;

            // One hour of registry time must consume exactly one hour of the storm.
            TickHours(1);
            Assert.AreEqual(atTrigger - 1f, flare.State.hoursRemaining, 0.001f,
                "Registry tick must advance the solar flare clock.");

            // Run past the full duration; the storm must terminate, not hang active.
            TickHours(Mathf.CeilToInt(atTrigger) + 2);
            Assert.AreEqual(0f, flare.State.hoursRemaining, 0.001f);
            Assert.IsTrue(ended, "OnSolarFlareEnded must fire once the duration elapses.");
            Assert.IsFalse(flare.State.electronicsDisabled,
                "Electronics must come back online when the flare ends.");
        }

        [Test]
        public void OzoneHole_IsDrivenByTheDailyPass_NotTheHourlyOne()
        {
            var ozone = _bootstrap.WeatherOzoneHole;
            Assume.That(ozone, Is.Not.Null);

            // Consume day 1's daily pass first: the registry runs the daily list on the
            // first tick it sees for a given day, so triggering before this would lose a
            // day immediately and make the assertion below ambiguous.
            _bootstrap.Registry.TickAll(1f, 1);

            ozone.Trigger(2); // only summer (season == 2) activates an ozone hole
            int atTrigger = ozone.State.daysRemaining;
            Assume.That(atTrigger, Is.GreaterThan(0));

            // Many further hours inside the same day must not consume a day.
            TickHours(20, day: 1);
            Assert.AreEqual(atTrigger, ozone.State.daysRemaining,
                "Ozone hole is day-scoped; hours within one day must not advance it.");

            _bootstrap.Registry.TickAll(1f, 2);
            Assert.AreEqual(atTrigger - 1, ozone.State.daysRemaining,
                "Advancing the day must consume exactly one ozone-hole day.");
        }

        [Test]
        public void InactiveWeatherTrackers_AreUnaffectedByTicking()
        {
            var flare = _bootstrap.WeatherSolarFlare;
            var bloodRain = _bootstrap.WeatherBloodRain;
            Assume.That(flare, Is.Not.Null);
            Assume.That(bloodRain, Is.Not.Null);

            // Nothing was triggered: driving the aggregate must be a no-op, and above all
            // must not throw or drive counters negative.
            Assert.DoesNotThrow(() => TickHours(48));
            Assert.AreEqual(0f, flare.State.hoursRemaining, 0.001f);
            Assert.GreaterOrEqual(bloodRain.State.hoursRemaining, 0f);
        }
    }
}
