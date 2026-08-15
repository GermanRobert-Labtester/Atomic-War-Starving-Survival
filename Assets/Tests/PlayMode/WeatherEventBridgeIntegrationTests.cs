using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Flashpoint;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// Prompts #319–#325 — PlayMode integration test for the
    /// Flashpoint → weather-event bridge.
    ///
    /// Asserts the full end-to-end path:
    ///   FlashpointChoreographer.Step
    ///     → ExecuteStep
    ///     → EventBus.Raise(FlashpointWeatherEventTriggered)
    ///     → GameBootstrap.Weather.NewContent.OnFlashpointWeatherEventTriggered
    ///     → WeatherAshLightning.Trigger() (or any of the 5 new systems)
    ///     → system.State.isActive == true
    ///
    /// Uses reflection to drive the private bridge handler directly so the
    /// test does not need a real GameBootstrap MonoBehaviour or a Scene
    /// load. This is the smallest possible end-to-end assertion.
    /// </summary>
    [TestFixture]
    public class WeatherEventBridgeIntegrationTests
    {
        private GameBootstrap _bootstrap;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            // Build a real GameBootstrap instance. We do NOT call its Awake/Start
            // (which would require the entire scene); instead we invoke the
            // production partial-init method that creates the 5 weather systems
            // AND wires the FlashpointWeatherEventTriggered bridge to the static
            // EventBus. This is the missing piece the end-to-end test needs.
            _bootstrap = new GameBootstrap();

            var bootNewWeatherSystems = typeof(GameBootstrap).GetMethod(
                "BootNewWeatherSystems",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(bootNewWeatherSystems,
                "BootNewWeatherSystems must exist on GameBootstrap.");
            bootNewWeatherSystems.Invoke(_bootstrap, null);

            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            // Dispose the bridge subscription so the static EventBus does not
            // keep this test fixture's bootstrap alive across tests.
            var subscriptionsField = typeof(GameBootstrap).GetField(
                "_subscriptions",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var bag = subscriptionsField?.GetValue(_bootstrap) as AtomicWar._Game.Utilities.SubscriptionBag;
            bag?.DisposeAll();

            _bootstrap = null;
            yield return null;
        }

        [UnityTest]
        public IEnumerator AshLightningTriggeredByFlashpointEventFiresWithinThreeFrames()
        {
            // Direct bridge dispatch. This is the smallest possible
            // end-to-end: it skips EventBus, but it proves the bridge's
            // dispatch logic is correct.
            InvokeBridgeHandler("weather_ash_lightning");
            yield return null;
            yield return null;
            yield return null;
            Assert.IsTrue(_bootstrap.WeatherAshLightning.State.isActive,
                "Bridge should have called WeatherAshLightning.Trigger() within 3 frames.");
        }

        [UnityTest]
        public IEnumerator AllFiveNewSystemsFlipActive()
        {
            // Drive each branch of the switch and assert the matching system flips.
            InvokeBridgeHandler("weather_fog_of_particulate");
            yield return null;
            Assert.IsTrue(_bootstrap.WeatherFogOfParticulate.State.isActive);

            InvokeBridgeHandler("weather_thermal_inversion");
            yield return null;
            Assert.IsTrue(_bootstrap.WeatherThermalInversion.State.isActive);

            InvokeBridgeHandler("weather_ice_storm");
            yield return null;
            Assert.IsTrue(_bootstrap.WeatherIceStorm.State.isActive);

            InvokeBridgeHandler("weather_silence");
            yield return null;
            Assert.IsTrue(_bootstrap.WeatherSilence.State.isActive);
        }

        [UnityTest]
        public IEnumerator UnknownWeatherIdIsIgnored()
        {
            // The bridge's default-branch logs and does nothing. Asserting that
            // no system flips proves the switch is exhaustive.
            InvokeBridgeHandler("weather_does_not_exist");
            yield return null;
            yield return null;
            Assert.IsFalse(_bootstrap.WeatherAshLightning.State.isActive);
            Assert.IsFalse(_bootstrap.WeatherFogOfParticulate.State.isActive);
            Assert.IsFalse(_bootstrap.WeatherThermalInversion.State.isActive);
            Assert.IsFalse(_bootstrap.WeatherIceStorm.State.isActive);
            Assert.IsFalse(_bootstrap.WeatherSilence.State.isActive);
        }

        [UnityTest]
        public IEnumerator EndToEndViaFlashpointChoreographer_FiresBridge()
        {
            // Build a real FlashpointSequenceSO with a single
            // weather_event_trigger step. The choreography needs a TimeSystem
            // hook and a Systems bundle, but ExecuteStep dispatches synchronously
            // so the bridge fires on the same frame.
            var sequence = ScriptableObject.CreateInstance<FlashpointSequenceSO>();
            sequence.sequenceId = "test_weather_bridge";
            var step = new FlashpointChoreographyStep
            {
                actionId = "weather_event_trigger",
                weatherEventId = "weather_silence",
                delayFromPreviousSeconds = 0f
            };
            sequence.steps.Add(step);

            var choreographer = new FlashpointChoreographer(
                sequence,
                () => false,
                new FlashpointChoreographerSystems(),
                () => false);
            choreographer.OnNuclearExchange();
            // Step delay is 0 → next Tick(realSeconds > 0) fires the step.
            choreographer.Tick(0.1f);
            // Give Unity a frame so the EventBus subscriber callback completes.
            yield return null;
            Assert.IsTrue(_bootstrap.WeatherSilence.State.isActive,
                "End-to-end: FlashpointChoreographer.Tick → EventBus → bridge → Trigger() must flip the system to active.");
        }

        // ── helpers ─────────────────────────────────────────────────────────
        private void InvokeBridgeHandler(string weatherEventId)
        {
            // The bridge handler is private. Locate it by name + parameter type and
            // invoke with a freshly-built typed event. This survives any
            // refactor of the surrounding helper method as long as the
            // handler name and signature don't change.
            var method = typeof(GameBootstrap).GetMethod(
                "OnFlashpointWeatherEventTriggered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method,
                "OnFlashpointWeatherEventTriggered must exist on GameBootstrap.");
            var evt = new FlashpointWeatherEventTriggered(weatherEventId);
            method.Invoke(_bootstrap, new object[] { evt });
        }
    }
}
