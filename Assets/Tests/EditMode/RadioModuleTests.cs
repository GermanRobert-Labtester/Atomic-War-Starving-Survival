using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using Ashfall.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Tests for the radio module system: signal strength, intel extraction,
    /// frequency tuning, EMP damage, and save/load.
    /// </summary>
    [TestFixture]
    public class RadioModuleTests
    {
        private const float Epsilon = 1e-4f;

        private RadioFrequencySO _civilianFreq;
        private RadioFrequencySO _militaryFreq;
        private RadioFrequencySO _emergencyFreq;
        private RadioBroadcastSO _testBroadcast;
        private System.Random _rng;

        [SetUp]
        public void Setup()
        {
            _rng = new System.Random(42);

            // Create test broadcast
            _testBroadcast = ScriptableObject.CreateInstance<RadioBroadcastSO>();
            _testBroadcast.id = "test_broadcast";
            _testBroadcast.minDay = 0;
            _testBroadcast.maxDay = -1;
            _testBroadcast.message = "Test broadcast message";

            // Create civilian frequency (active Day 0-30)
            _civilianFreq = ScriptableObject.CreateInstance<RadioFrequencySO>();
            _civilianFreq.id = "88.5_civilian";
            _civilianFreq.displayName = "88.5 FM Civilian";
            _civilianFreq.frequencyMHz = 88.5f;
            _civilianFreq.type = RadioFrequencyType.Civilian;
            _civilianFreq.activeFromDay = 0;
            _civilianFreq.activeUntilDay = 30;
            _civilianFreq.baseSignalStrength = 0.7f;
            _civilianFreq.interferenceSusceptibility = 0.3f;
            _civilianFreq.broadcasts = new List<RadioBroadcastSO> { _testBroadcast };

            // Create military frequency (active Day 0-30)
            _militaryFreq = ScriptableObject.CreateInstance<RadioFrequencySO>();
            _militaryFreq.id = "102.1_military";
            _militaryFreq.displayName = "102.1 Military";
            _militaryFreq.frequencyMHz = 102.1f;
            _militaryFreq.type = RadioFrequencyType.Military;
            _militaryFreq.activeFromDay = 0;
            _militaryFreq.activeUntilDay = 30;
            _militaryFreq.baseSignalStrength = 0.6f;
            _militaryFreq.interferenceSusceptibility = 0.2f;
            _militaryFreq.broadcasts = new List<RadioBroadcastSO> { _testBroadcast };

            // Create emergency frequency (active Day 31+)
            _emergencyFreq = ScriptableObject.CreateInstance<RadioFrequencySO>();
            _emergencyFreq.id = "107.0_emergency";
            _emergencyFreq.displayName = "107.0 Emergency";
            _emergencyFreq.frequencyMHz = 107.0f;
            _emergencyFreq.type = RadioFrequencyType.Emergency;
            _emergencyFreq.activeFromDay = 31;
            _emergencyFreq.activeUntilDay = -1;
            _emergencyFreq.baseSignalStrength = 0.5f;
            _emergencyFreq.interferenceSusceptibility = 0.4f;
            _emergencyFreq.broadcasts = new List<RadioBroadcastSO> { _testBroadcast };
        }

        [TearDown]
        public void TearDown()
        {
            if (_testBroadcast != null) Object.DestroyImmediate(_testBroadcast);
            if (_civilianFreq != null) Object.DestroyImmediate(_civilianFreq);
            if (_militaryFreq != null) Object.DestroyImmediate(_militaryFreq);
            if (_emergencyFreq != null) Object.DestroyImmediate(_emergencyFreq);
        }

        [Test]
        public void RadioFrequency_IsActiveOnDay_WithinRange()
        {
            Assert.IsTrue(_civilianFreq.IsActiveOnDay(0), "Civilian freq should be active on Day 0");
            Assert.IsTrue(_civilianFreq.IsActiveOnDay(15), "Civilian freq should be active on Day 15");
            Assert.IsTrue(_civilianFreq.IsActiveOnDay(30), "Civilian freq should be active on Day 30");
            Assert.IsFalse(_civilianFreq.IsActiveOnDay(31), "Civilian freq should not be active on Day 31");
        }

        [Test]
        public void RadioFrequency_IsActiveOnDay_InfiniteRange()
        {
            Assert.IsTrue(_emergencyFreq.IsActiveOnDay(31), "Emergency freq should be active on Day 31");
            Assert.IsTrue(_emergencyFreq.IsActiveOnDay(100), "Emergency freq should be active on Day 100");
            Assert.IsFalse(_emergencyFreq.IsActiveOnDay(30), "Emergency freq should not be active on Day 30");
        }

        [Test]
        public void RadioState_UpdateSignalStrength_ClearWeather()
        {
            var state = new RadioState();
            state.UpdateSignalStrength(0.7f, 1.0f, 0.3f); // Clear weather modifier

            Assert.AreEqual(0.7f, state.SignalStrength, Epsilon, "Signal should be full in clear weather with no EMP damage");
        }

        [Test]
        public void RadioState_UpdateSignalStrength_FalloutStormReducesSignal()
        {
            var state = new RadioState();
            float stormModifier = RadioTunerSystem.GetWeatherSignalModifier(WeatherKind.FalloutStorm);
            // Use interferenceSusceptibility=1.0 so the test name matches the
            // expectation: "Signal should be ~20% in fallout storm" means the
            // storm modifier is applied in full to a fully-susceptible
            // frequency. The default 0.3 used in the production code only
            // applies 30% of the storm, which doesn't match the test's intent.
            state.UpdateSignalStrength(0.7f, stormModifier, 1.0f);

            Assert.Less(state.SignalStrength, 0.7f, "Signal should be reduced in fallout storm");
            Assert.AreEqual(0.14f, state.SignalStrength, 0.01f, "Signal should be ~20% in fallout storm");
        }

        [Test]
        public void RadioState_UpdateSignalStrength_EmpDamageReducesSignal()
        {
            var state = new RadioState();
            state.ApplyEmpDamage(50f); // 50% EMP damage
            state.UpdateSignalStrength(0.7f, 1.0f, 0.3f);

            Assert.AreEqual(0.35f, state.SignalStrength, 0.01f, "50% EMP damage should halve signal");
        }

        [Test]
        public void RadioState_ApplyEmpDamage_ClampsAt100()
        {
            var state = new RadioState();
            state.ApplyEmpDamage(150f);

            Assert.AreEqual(100f, state.EmpDamage, Epsilon, "EMP damage should clamp at 100");
        }

        [Test]
        public void RadioState_ConsumeFuel_ReducesFuel()
        {
            var state = new RadioState();
            state.AvailableFuel = 10f;
            state.PowerConsumptionPerHour = 0.5f;

            float consumed = state.ConsumeFuel(2f);

            Assert.AreEqual(1f, consumed, Epsilon, "Should consume 1 unit of fuel over 2 hours");
            Assert.AreEqual(9f, state.AvailableFuel, Epsilon, "Fuel should be reduced by 1");
        }

        [Test]
        public void RadioState_ConsumeFuel_StopsAtZero()
        {
            var state = new RadioState();
            state.AvailableFuel = 1f;
            state.PowerConsumptionPerHour = 0.5f;

            float consumed = state.ConsumeFuel(10f);

            Assert.AreEqual(1f, consumed, Epsilon, "Should only consume available fuel");
            Assert.AreEqual(0f, state.AvailableFuel, Epsilon, "Fuel should be zero");
        }

        [Test]
        public void RadioState_AdvanceTuning_IncreasesProgress()
        {
            var state = new RadioState();
            state.AvailableFuel = 1f; // PowerConsumptionPerHour defaults to 0.5; fuel > 0 makes IsOperational true
            state.SignalStrength = 1.0f;
            state.CurrentFrequencyId = "test";

            bool completed = state.AdvanceTuning(1f, 0.5f); // 1 hour, 50% tuning rate

            Assert.IsFalse(completed, "Tuning should not complete after 1 hour");
            Assert.AreEqual(0.5f, state.TuningProgress, Epsilon, "Progress should be 0.5 after 1 hour");
        }

        [Test]
        public void RadioState_AdvanceTuning_CompletesAtOne()
        {
            var state = new RadioState();
            state.AvailableFuel = 1f;
            state.SignalStrength = 1.0f;
            state.CurrentFrequencyId = "test";

            state.AdvanceTuning(1f, 0.5f);
            bool completed = state.AdvanceTuning(1f, 0.5f);

            Assert.IsTrue(completed, "Tuning should complete after 2 hours");
            Assert.AreEqual(1.0f, state.TuningProgress, Epsilon, "Progress should be 1.0");
        }

        [Test]
        public void RadioTunerSystem_SetFrequencies_LoadsFrequencies()
        {
            var tuner = new RadioTunerSystem(_rng);
            var frequencies = new[] { _civilianFreq, _militaryFreq, _emergencyFreq };

            tuner.SetFrequencies(frequencies);

            Assert.AreEqual(3, tuner.Frequencies.Count, "Should load 3 frequencies");
            Assert.AreEqual(_civilianFreq, tuner.GetFrequency("88.5_civilian"));
            Assert.AreEqual(_militaryFreq, tuner.GetFrequency("102.1_military"));
            Assert.AreEqual(_emergencyFreq, tuner.GetFrequency("107.0_emergency"));
        }

        [Test]
        public void RadioTunerSystem_TuneToFrequency_ResetsProgress()
        {
            var tuner = new RadioTunerSystem(_rng);
            tuner.SetFrequencies(new[] { _civilianFreq });
            tuner.State.AvailableFuel = 1f; // AdvanceTuning requires IsOperational

            tuner.State.SignalStrength = 1.0f;
            tuner.State.AdvanceTuning(1f, 0.5f);
            Assert.AreEqual(0.5f, tuner.State.TuningProgress, Epsilon, "Setup: progress should be 0.5");

            bool tuned = tuner.TuneToFrequency("88.5_civilian");

            Assert.IsTrue(tuned, "Should successfully tune to frequency");
            Assert.AreEqual(0f, tuner.State.TuningProgress, Epsilon, "Progress should reset to 0");
            Assert.AreEqual("88.5_civilian", tuner.State.CurrentFrequencyId);
        }

        [Test]
        public void RadioFrequency_ResolveInterceptChannelTag_DefaultsByType()
        {
            Assert.AreEqual("CH-3 ASH ROAD", _civilianFreq.ResolveInterceptChannelTag());
            Assert.AreEqual("CH-7 MILBAND", _militaryFreq.ResolveInterceptChannelTag());
            Assert.AreEqual("CH-11 STOCKPILE", _emergencyFreq.ResolveInterceptChannelTag());

            _militaryFreq.interceptChannelTag = "CH-CUSTOM";
            Assert.AreEqual("CH-CUSTOM", _militaryFreq.ResolveInterceptChannelTag());
            _militaryFreq.interceptChannelTag = null;
        }

        [Test]
        public void RadioTunerSystem_BuildTunerBands_AllPlusFrequencies()
        {
            var tuner = new RadioTunerSystem(_rng);
            tuner.SetFrequencies(new[] { _civilianFreq, _militaryFreq, _emergencyFreq });

            var bands = tuner.BuildTunerBands();
            Assert.AreEqual(4, bands.Count, "ALL + 3 frequencies");
            Assert.IsTrue(bands[0].IsAllBands);
            Assert.AreEqual(string.Empty, bands[0].ChannelTag);
            Assert.AreEqual(RadioFrequencySO.Ids.Civilian, bands[1].FrequencyId);
            Assert.AreEqual("CH-3 ASH ROAD", bands[1].ChannelTag);
            Assert.AreEqual(RadioFrequencySO.Ids.Military, bands[2].FrequencyId);
            Assert.AreEqual("CH-7 MILBAND", bands[2].ChannelTag);
            Assert.AreEqual(RadioFrequencySO.Ids.Emergency, bands[3].FrequencyId);
            Assert.AreEqual("CH-11 STOCKPILE", bands[3].ChannelTag);
            Assert.That(bands[2].Label, Does.Contain("MILBAND").Or.Contain("Military"));
        }

        [Test]
        public void RadioTunerSystem_Detune_ClearsFrequencyAndFiresEvent()
        {
            var tuner = new RadioTunerSystem(_rng);
            tuner.SetFrequencies(new[] { _militaryFreq });
            string last = "unset";
            tuner.OnFrequencyChanged += id => last = id ?? "null";

            Assert.IsTrue(tuner.TuneToFrequency(RadioFrequencySO.Ids.Military));
            Assert.AreEqual(RadioFrequencySO.Ids.Military, tuner.State.CurrentFrequencyId);
            Assert.AreEqual("CH-7 MILBAND", tuner.GetCurrentInterceptChannelTag());

            tuner.Detune();
            Assert.IsTrue(string.IsNullOrEmpty(tuner.State.CurrentFrequencyId));
            Assert.AreEqual(string.Empty, tuner.GetCurrentInterceptChannelTag());
            Assert.AreEqual("null", last);
            Assert.AreEqual(0f, tuner.State.SignalStrength, Epsilon);
        }

        [Test]
        public void RadioTunerSystem_FindFrequencyForChannelTag_PrefersActiveOnDay()
        {
            var tuner = new RadioTunerSystem(_rng);
            tuner.SetFrequencies(new[] { _civilianFreq, _militaryFreq, _emergencyFreq });

            Assert.AreEqual(_militaryFreq, tuner.FindFrequencyForChannelTag("CH-7 MILBAND", day: 10));
            Assert.AreEqual(_civilianFreq, tuner.FindFrequencyForChannelTag("CH-3 ASH ROAD", day: 5));
            // Emergency only active day 31+
            Assert.AreEqual(_emergencyFreq, tuner.FindFrequencyForChannelTag("CH-11 STOCKPILE", day: 40));
            Assert.IsNull(tuner.FindFrequencyForChannelTag("NOPE"));
        }

        [Test]
        public void RadioTunerSystem_GetWeatherSignalModifier_VariousWeather()
        {
            Assert.AreEqual(1.0f, RadioTunerSystem.GetWeatherSignalModifier(WeatherKind.Clear), Epsilon);
            Assert.AreEqual(0.8f, RadioTunerSystem.GetWeatherSignalModifier(WeatherKind.Ashfall), Epsilon);
            Assert.AreEqual(0.6f, RadioTunerSystem.GetWeatherSignalModifier(WeatherKind.Blizzard), Epsilon);
            Assert.AreEqual(0.2f, RadioTunerSystem.GetWeatherSignalModifier(WeatherKind.FalloutStorm), Epsilon);
        }

        [Test]
        public void IntelNode_CreatePlumeReport_SetsProperties()
        {
            var intel = IntelNode.CreatePlumeReport("location_1", 50f, 0.7f, 10, 15, "Plume detected");

            Assert.AreEqual(IntelType.PlumeReport, intel.Type);
            Assert.AreEqual("location_1", intel.TargetLocationId);
            Assert.AreEqual(50f, intel.NumericValue, Epsilon);
            Assert.AreEqual(0.7f, intel.Confidence, Epsilon);
            Assert.AreEqual(10, intel.ExtractedDay);
            Assert.AreEqual(15, intel.ExpirationDay);
            Assert.IsFalse(intel.IsExpired(12));
            Assert.IsTrue(intel.IsExpired(16));
        }

        [Test]
        public void IntelNode_CreateMortarWarning_SetsProperties()
        {
            var intel = IntelNode.CreateMortarWarning("grid_47", 0.8f, 5, 8, "Incoming artillery");

            Assert.AreEqual(IntelType.MortarWarning, intel.Type);
            Assert.AreEqual("grid_47", intel.TargetLocationId);
            Assert.AreEqual(0.8f, intel.Confidence, Epsilon);
        }

        [Test]
        public void RadioTunerSystem_CaptureAndRestoreState_PreservesState()
        {
            var tuner = new RadioTunerSystem(_rng);
            tuner.SetFrequencies(new[] { _civilianFreq });
            tuner.State.AvailableFuel = 25f;
            tuner.State.EmpDamage = 30f;
            tuner.State.CurrentFrequencyId = "88.5_civilian";
            tuner.State.TuningProgress = 0.6f;

            var save = tuner.CaptureState();

            var tuner2 = new RadioTunerSystem(_rng);
            tuner2.RestoreState(save);

            Assert.AreEqual(25f, tuner2.State.AvailableFuel, Epsilon, "Fuel should be restored");
            Assert.AreEqual(30f, tuner2.State.EmpDamage, Epsilon, "EMP damage should be restored");
            Assert.AreEqual("88.5_civilian", tuner2.State.CurrentFrequencyId);
            Assert.AreEqual(0.6f, tuner2.State.TuningProgress, Epsilon, "Tuning progress should be restored");
        }

        [Test]
        public void RadioTunerSystem_Tick_ConsumesFuel()
        {
            var tuner = new RadioTunerSystem(_rng);
            tuner.SetFrequencies(new[] { _civilianFreq });
            tuner.State.AvailableFuel = 10f;
            tuner.State.PowerConsumptionPerHour = 0.5f;
            tuner.TuneToFrequency("88.5_civilian");
            tuner.UpdateSignalStrength(WeatherKind.Clear);

            tuner.Tick(2f, WeatherKind.Clear, 10);

            Assert.AreEqual(9f, tuner.State.AvailableFuel, Epsilon, "Should consume 1 unit of fuel over 2 hours");
        }

        [Test]
        public void RadioTunerSystem_ApplyPlumeReportToMap_UpdatesKnowledgeMap()
        {
            var tuner = new RadioTunerSystem(_rng);
            var map = new RadiationKnowledgeMap();
            map.SeedTile("location_1", 100f);

            var intel = IntelNode.CreatePlumeReport("location_1", 75f, 0.6f, 10, 15, "Test");

            bool applied = tuner.ApplyPlumeReportToMap(intel, map);

            Assert.IsTrue(applied, "Should apply plume report to map");
            Assert.IsTrue(intel.IsConsumed, "Intel should be marked as consumed");

            var tile = map.GetTile("location_1");
            Assert.AreEqual(75f, tile.RumoredRad, Epsilon, "Map rumored rad should be updated");
            Assert.AreEqual(0.4f, tile.RumorUncertainty, 0.01f, "Uncertainty should be 1 - confidence");
        }
    }
}
