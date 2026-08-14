using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Shelter;
using Ashfall.Core;

namespace AtomicWar.Tests.PlayMode
{
    [TestFixture]
    public class FalloutStormHazardPlayModeTests
    {
        private WeatherSystem _weatherSystem;
        private AudioEventBus _audioEventBus;
        private FalloutStormHazardSystem _hazardSystem;
        private ShelterRoom _entryRoom;

        [SetUp]
        public void SetUp()
        {
            _weatherSystem = new WeatherSystem();
            _audioEventBus = new AudioEventBus();
            _hazardSystem = new FalloutStormHazardSystem(_weatherSystem, _audioEventBus);
            _entryRoom = new ShelterRoom { RoomId = "entry", AmbientRadiation = 0f };
        }

        [TearDown]
        public void TearDown()
        {
            _audioEventBus.Teardown();
        }

        [UnityTest]
        public IEnumerator HighIntensityFalloutStorm_DoublesAirFilterDegradationRate()
        {
            _weatherSystem.ForceWeather(WeatherKind.FalloutStorm);
            _weatherSystem.StormIntensity = 0.8f;
            yield return null;

            Assert.That(_weatherSystem.AirFilterDegradationMultiplier, Is.EqualTo(2.0f), "FalloutStorm with intensity >= 0.7 must double air filter degradation multiplier.");

            float baseWearRate = 5f;
            float actualWearRate = _hazardSystem.CalculateFilterWearRate(baseWearRate);
            Assert.That(actualWearRate, Is.EqualTo(10f), "Calculated filter wear rate must be doubled (10.0 per hour).");
        }

        [UnityTest]
        public IEnumerator HatchBreach_DuringFalloutStorm_FloodsEntryRoomWithRadiation_And_FiresEmergencySiren()
        {
            _weatherSystem.ForceWeather(WeatherKind.FalloutStorm);
            _weatherSystem.StormIntensity = 0.9f;
            yield return null;

            Assert.That(_entryRoom.AmbientRadiation, Is.EqualTo(0f));
            Assert.That(_audioEventBus.IsEmergencySirenActive, Is.False);

            bool breachProcessed = _hazardSystem.ProcessBreachedHatch(_entryRoom);
            yield return null;

            Assert.That(breachProcessed, Is.True, "Hatch breach during FalloutStorm must process successfully.");
            Assert.That(_entryRoom.AmbientRadiation, Is.GreaterThanOrEqualTo(50f), "Entry room must be flooded with +50 rads/hr ambient radiation.");
            Assert.That(_audioEventBus.IsEmergencySirenActive, Is.True, "Emergency siren audio event must be triggered on AudioEventBus.");
        }
    }
}
