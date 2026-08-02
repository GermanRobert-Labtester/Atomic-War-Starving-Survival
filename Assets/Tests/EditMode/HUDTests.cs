using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.UI;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Events;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class HUDTests
    {
        private const float Eps = 1e-4f;

        private GameObject _hudObject;
        private HUD _hud;

        [SetUp]
        public void SetUp()
        {
            _hudObject = new GameObject("TestHUD");
            _hud = _hudObject.AddComponent<HUD>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_hudObject != null)
            {
                Object.DestroyImmediate(_hudObject);
            }
        }

        [Test]
        public void HUD_SimulatedRisingExposure_UpdatesDosimeterNeedleAndGeigerCadence()
        {
            float rateLow = 10f;
            float rateMid = 50f;
            float rateHigh = 90f;

            // Step 1: Low exposure rate
            _hud.OnRadiationUpdated(cumulativeDose: 15f, currentRate: rateLow);
            Assert.That(_hud.DosimeterHUD.CurrentRate, Is.EqualTo(rateLow).Within(Eps));
            float needleAngleLow = _hud.DosimeterHUD.GetNeedleRotationDegrees();
            float clickFreqLow = _hud.GeigerAudioHook.CurrentClickFrequency;

            // Step 2: Mid exposure rate
            _hud.OnRadiationUpdated(cumulativeDose: 30f, currentRate: rateMid);
            Assert.That(_hud.DosimeterHUD.CurrentRate, Is.EqualTo(rateMid).Within(Eps));
            float needleAngleMid = _hud.DosimeterHUD.GetNeedleRotationDegrees();
            float clickFreqMid = _hud.GeigerAudioHook.CurrentClickFrequency;

            // Step 3: High exposure rate
            _hud.OnRadiationUpdated(cumulativeDose: 80f, currentRate: rateHigh);
            Assert.That(_hud.DosimeterHUD.CurrentRate, Is.EqualTo(rateHigh).Within(Eps));
            float needleAngleHigh = _hud.DosimeterHUD.GetNeedleRotationDegrees();
            float clickFreqHigh = _hud.GeigerAudioHook.CurrentClickFrequency;

            // Assert monotonic needle rotation and geiger cadence increases
            Assert.That(needleAngleMid, Is.GreaterThan(needleAngleLow));
            Assert.That(needleAngleHigh, Is.GreaterThan(needleAngleMid));

            Assert.That(clickFreqMid, Is.GreaterThan(clickFreqLow));
            Assert.That(clickFreqHigh, Is.GreaterThan(clickFreqMid));
        }

        [Test]
        public void HUD_SurvivorNeeds_UpdatesNeedsBar_AndDetectsCriticalRanges()
        {
            var survivor = new Survivor { Id = "s1" };
            survivor.Needs.Hunger = 85f; // Critical hunger
            survivor.Needs.Thirst = 10f;  // Normal thirst

            _hud.Bind(survivor);

            var hungerData = _hud.NeedsBar.NeedBars["hunger"];
            var thirstData = _hud.NeedsBar.NeedBars["thirst"];

            Assert.That(hungerData.IsCritical, Is.True);
            Assert.That(thirstData.IsCritical, Is.False);

            Color activeHungerColor = _hud.NeedsBar.GetActiveColor("hunger");
            Assert.That(activeHungerColor, Is.EqualTo(hungerData.CriticalColor));
        }

        [Test]
        public void HUD_DebugToggleF2_SwitchesRawValuesMode()
        {
            Assert.That(_hud.DebugModeEnabled, Is.False);
            Assert.That(_hud.NeedsBar.ShowRawValues, Is.False);

            _hud.SetDebugMode(true);

            Assert.That(_hud.DebugModeEnabled, Is.True);
            Assert.That(_hud.NeedsBar.ShowRawValues, Is.True);
            Assert.That(_hud.DosimeterHUD.ShowRawValues, Is.True);
            Assert.That(_hud.EnvironmentStatusHud.ShowRawValues, Is.True);
        }

        [Test]
        public void HUD_EventRunnerBinding_TriggersEventModal()
        {
            var runner = new EventRunner();
            _hud.BindEventRunner(runner);

            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "test_ev";
            ev.title = "Test Event";

            Assert.That(_hud.EventModalUI.IsOpen, Is.False);

            runner.Run(ev, new EventContext());

            Assert.That(_hud.EventModalUI.IsOpen, Is.True);
            Assert.That(_hud.EventModalUI.ActiveEvent, Is.EqualTo(ev));
        }
    }
}
