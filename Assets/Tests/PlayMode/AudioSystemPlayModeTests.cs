using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.UI;
using Ashfall.Core;

namespace AtomicWar.Tests.PlayMode
{
    [TestFixture]
    public class AudioSystemPlayModeTests
    {
        private AudioEventBus _audioBus;

        [SetUp]
        public void SetUp()
        {
            _audioBus = new AudioEventBus();
        }

        [TearDown]
        public void TearDown()
        {
            _audioBus?.Teardown();
        }

        [UnityTest]
        public IEnumerator FalloutStorm_Underground_IsMuffled_And_BreachRemovesMuffle()
        {
            _audioBus.SetUnderground(true);
            _audioBus.SetHatchBreached(false);
            _audioBus.SetWeather(WeatherKind.FalloutStorm);

            yield return null;

            Assert.That(_audioBus.IsWindPlaying, Is.True, "FalloutStorm should start wind audio.");
            Assert.That(_audioBus.IsWindMuffled, Is.True, "Wind audio must be muffled when underground.");
            Assert.That(_audioBus.LowPassCutoffHz, Is.EqualTo(500f), "Muffled wind lowpass cutoff should be 500 Hz.");

            // Trigger hatch breach (#33)
            _audioBus.SetHatchBreached(true);

            yield return null;

            Assert.That(_audioBus.IsWindMuffled, Is.False, "Hatch breach must remove muffling filter.");
            Assert.That(_audioBus.LowPassCutoffHz, Is.EqualTo(22000f), "Unmuffled wind cutoff should be 22000 Hz.");
        }

        [UnityTest]
        public IEnumerator PowerBlackout_Or_RadiationAnxiety_TriggersHeartbeatAudio()
        {
            Assert.That(_audioBus.IsHeartbeatPlaying, Is.False, "Heartbeat should be silent normally.");

            // Trigger blackout (#27)
            _audioBus.SetBlackout(true);
            yield return null;
            Assert.That(_audioBus.IsHeartbeatPlaying, Is.True, "Blackout must activate heartbeat audio.");

            // Clear blackout
            _audioBus.SetBlackout(false);
            yield return null;
            Assert.That(_audioBus.IsHeartbeatPlaying, Is.False, "Clearing blackout should silence heartbeat if no anxiety.");

            // Spike survivor radiation anxiety (#19)
            _audioBus.SetSurvivorAnxiety("survivor_1", true);
            yield return null;
            Assert.That(_audioBus.IsHeartbeatPlaying, Is.True, "Radiation anxiety spike must trigger heartbeat audio.");
        }

        [UnityTest]
        public IEnumerator GeigerCadence_IsLogarithmic_And_ScreamsAtHighRads()
        {
            float lowRate = GeigerAudioHook.ComputeLogarithmicCadence(5f);
            float medRate = GeigerAudioHook.ComputeLogarithmicCadence(25f);
            float highRate = GeigerAudioHook.ComputeLogarithmicCadence(100f);

            Assert.That(lowRate, Is.GreaterThan(0f));
            Assert.That(medRate, Is.GreaterThan(lowRate));
            Assert.That(highRate, Is.GreaterThan(medRate));

            // Logarithmic check: ratio (100 vs 25) < linear ratio (4x)
            float ratioMedToLow = medRate / lowRate;
            float ratioHighToMed = highRate / medRate;
            Assert.That(ratioHighToMed, Is.LessThan(ratioMedToLow), "Click cadence growth rate must be logarithmic (diminishing returns).");

            var go = new GameObject("GeigerHook");
            var hook = go.AddComponent<GeigerAudioHook>();
            hook.UpdateExposureRate(100f);

            yield return null;

            Assert.That(hook.IsStaticScreamActive, Is.True, "High rads must activate continuous static scream.");
            Object.Destroy(go);
        }
    }
}
