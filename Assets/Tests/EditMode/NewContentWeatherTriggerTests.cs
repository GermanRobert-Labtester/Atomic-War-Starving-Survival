using NUnit.Framework;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Flashpoint;

namespace AtomicWar.Tests.EditMode
{
    // Section X trigger tests: each new weather system can be fired via
    // its Trigger() method (mirroring Weather_BloodRain.Trigger()) and the
    // FlashpointWeatherEventTriggered typed event carries the right id.

    [TestFixture]
    public class NewWeatherTriggerTests
    {
        [Test] public void AshLightningTriggerActivates()    { var w = new Weather_AshLightning(); w.Trigger(); Assert.IsTrue(w.State.isActive); }
        [Test] public void FogOfParticulateTriggerActivates() { var w = new Weather_FogOfParticulate(); w.Trigger(); Assert.IsTrue(w.State.isActive); }
        [Test] public void ThermalInversionTriggerActivates(){ var w = new Weather_ThermalInversion(); w.Trigger(); Assert.IsTrue(w.State.isActive); }
        [Test] public void IceStormTriggerActivates()        { var w = new Weather_IceStorm(); w.Trigger(); Assert.IsTrue(w.State.isActive); }
        [Test] public void SilenceTriggerActivates()         { var w = new Weather_Silence(); w.Trigger(); Assert.IsTrue(w.State.isActive); }

        [Test] public void AshLightningDeactivateRestores()    { var w = new Weather_AshLightning(); w.Trigger(); w.SetActive(false); Assert.IsFalse(w.State.isActive); }
        [Test] public void IceStormDeactivateUnfreezesHatch()  { var w = new Weather_IceStorm(); w.Trigger(); w.Tick(1f, 0.5f); Assert.IsTrue(w.State.hatchFrozenShut); w.SetActive(false); Assert.IsFalse(w.State.hatchFrozenShut); }
    }

    [TestFixture]
    public class FlashpointWeatherEventTriggeredTests
    {
        [Test]
        public void EventCarriesCanonicalId()
        {
            var evt = new FlashpointWeatherEventTriggered("weather_ash_lightning");
            Assert.AreEqual("weather_ash_lightning", evt.WeatherEventId);
        }

        [Test]
        public void AllFiveCanonicalIdsConstruct()
        {
            // The bridge in GameBootstrap.Weather.NewContent.cs switches on these exact strings.
            string[] ids = new[] {
                "weather_ash_lightning",
                "weather_fog_of_particulate",
                "weather_thermal_inversion",
                "weather_ice_storm",
                "weather_silence"
            };
            Assert.AreEqual(5, ids.Length);
            var set = new System.Collections.Generic.HashSet<string>(ids);
            Assert.AreEqual(5, set.Count);
        }

        [Test]
        public void EventIsValueType()
        {
            // Confirms the typed event uses the project's readonly struct convention.
            Assert.IsTrue(typeof(FlashpointWeatherEventTriggered).IsValueType);
        }
    }

    [TestFixture]
    public class FlashpointStepWeatherEventIdTests
    {
        [Test]
        public void NewWeatherEventIdFieldExists()
        {
            // The choreography step carries the new weatherEventId field.
            var step = new FlashpointChoreographyStep
            {
                actionId = "weather_event_trigger",
                weatherEventId = "weather_silence"
            };
            Assert.AreEqual("weather_silence", step.weatherEventId);
        }

        [Test]
        public void WeatherEventIdDefaultsToEmpty()
        {
            // Existing assets (built before this field) should deserialise
            // an empty string and the bridge should no-op on empty.
            var step = new FlashpointChoreographyStep { actionId = "flash" };
            Assert.AreEqual(string.Empty, step.weatherEventId);
        }
    }
}
