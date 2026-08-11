using System.Collections.Generic;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.UI;
using NUnit.Framework;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Climate-terminal contract: runtime estimates use installed module rates,
    /// player priorities remain owned by the persisted power network, and the
    /// UI emits intent instead of changing a consumer directly.
    /// </summary>
    [TestFixture]
    public class AirHeatManagementTests
    {
        private readonly List<Object> _toDestroy = new List<Object>();
        private AirFiltrationModuleSO _filterDefinition;
        private HeaterModuleSO _heaterDefinition;

        [SetUp]
        public void SetUp()
        {
            _filterDefinition = ScriptableObject.CreateInstance<AirFiltrationModuleSO>();
            _filterDefinition.ModuleId = "air_filtration";
            _filterDefinition.DisplayName = "Air Filter";
            _filterDefinition.DegradationRatePerHour = 2f;
            _toDestroy.Add(_filterDefinition);

            _heaterDefinition = ScriptableObject.CreateInstance<HeaterModuleSO>();
            _heaterDefinition.ModuleId = "heater";
            _heaterDefinition.DisplayName = "Heater";
            _heaterDefinition.FuelConsumptionRatePerHour = 1.5f;
            _heaterDefinition.HeatOutputPerLevel = 5f;
            _toDestroy.Add(_heaterDefinition);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy.Clear();
        }

        [Test]
        public void Snapshot_ReportsModuleRateRuntimeAndGridState()
        {
            var shelter = CreateShelter();
            var network = PowerNetwork.CreateDefault();
            network.ApplyToShelter(shelter);
            using (var climate = new AirHeatManagementSystem(shelter, network, () => 4f, () => -12f))
            {
                var snapshot = climate.GetSnapshot();

                Assert.That(snapshot.IndoorTemperatureCelsius, Is.EqualTo(4f));
                Assert.That(snapshot.AmbientTemperatureCelsius, Is.EqualTo(-12f));
                Assert.That(snapshot.FilterHealth, Is.EqualTo(50f));
                Assert.That(snapshot.FilterRuntimeHours, Is.EqualTo(25f).Within(0.001f));
                Assert.That(snapshot.FilterLoad.IsPowered, Is.True);
                Assert.That(snapshot.HeaterFuel, Is.EqualTo(9f));
                Assert.That(snapshot.HeaterFuelBurnPerHour, Is.EqualTo(1.2f).Within(0.001f));
                Assert.That(snapshot.HeaterRuntimeHours, Is.EqualTo(7.5f).Within(0.001f));
                Assert.That(snapshot.HeaterLoad.IsShed, Is.True,
                    "Filter P1 should be retained before heater P2 on the default 50W generator.");
            }
        }

        [Test]
        public void PriorityAndRequest_PersistThroughPowerNetworkSave()
        {
            var shelter = CreateShelter();
            var network = PowerNetwork.CreateDefault();
            network.ApplyToShelter(shelter);
            using (var climate = new AirHeatManagementSystem(shelter, network))
            {
                int changes = 0;
                climate.OnChanged += () => changes++;

                Assert.That(climate.AdjustPriority(AirHeatLoad.Heater, -1), Is.True);
                Assert.That(climate.ToggleRequested(AirHeatLoad.AirFiltration), Is.True);
                Assert.That(changes, Is.GreaterThanOrEqualTo(2));

                var saved = network.CaptureState();
                var restoredNetwork = PowerNetwork.CreateDefault();
                restoredNetwork.RestoreState(saved);
                using (var restored = new AirHeatManagementSystem(shelter, restoredNetwork))
                {
                    var snapshot = restored.GetSnapshot();
                    Assert.That(snapshot.HeaterLoad.Priority, Is.EqualTo(1));
                    Assert.That(snapshot.FilterLoad.IsRequested, Is.False);
                    Assert.That(snapshot.FilterLoad.IsPowered, Is.False);
                }
            }
        }

        [Test]
        public void Terminal_EmitsClimateIntentsAndUsesKBinding()
        {
            var shelter = CreateShelter();
            var network = PowerNetwork.CreateDefault();
            network.ApplyToShelter(shelter);
            using (var climate = new AirHeatManagementSystem(shelter, network, () => 1f, () => -10f))
            {
                var go = new GameObject("AirHeatManagementHudTests");
                _toDestroy.Add(go);
                var terminal = go.AddComponent<AirHeatManagementHUD>();
                terminal.Bind(climate.GetSnapshot);
                terminal.OnPriorityAdjustmentRequested += (load, direction) =>
                {
                    climate.AdjustPriority(load, direction);
                    terminal.ReportOutcome("Priority adjusted.");
                };
                terminal.OnRequestToggleRequested += load =>
                {
                    climate.ToggleRequested(load);
                    terminal.ReportOutcome("Request updated.");
                };

                terminal.Open();
                StringAssert.Contains("AIR + HEAT CONTROL", terminal.PanelSummary);
                StringAssert.Contains("FILTER: ACTIVE", terminal.PanelSummary);
                StringAssert.Contains("HEATER: OFFLINE", terminal.PanelSummary);
                Assert.That(terminal.IncreaseSelectedPriority(), Is.True);
                Assert.That(network.GetConsumer("air_filtration").Priority, Is.EqualTo(2));
                Assert.That(terminal.ToggleSelectedLoad(), Is.True);
                Assert.That(terminal.SelectedLoad, Is.EqualTo(AirHeatLoad.Heater));
                Assert.That(terminal.ToggleSelectedRequest(), Is.True);
                Assert.That(network.GetConsumer("heater").IsRequested, Is.False);
                StringAssert.Contains("REPORT: Request updated.", terminal.PanelSummary);

                var inputGo = new GameObject("AirHeatManagementInputTests");
                _toDestroy.Add(inputGo);
                var input = inputGo.AddComponent<PlayerInputHandler>();
                Assert.That(input.AirHeatManagementKey, Is.EqualTo(KeyCode.K));
            }
        }

        private Shelter CreateShelter()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_filterDefinition, 1)
            {
                FilterHealth = 50f,
                IsEnabled = true
            });
            shelter.AddModule(new ShelterModuleInstance(_heaterDefinition, 1)
            {
                Fuel = 9f,
                FuelBurnMultiplier = 0.8f,
                IsEnabled = true
            });
            return shelter;
        }
    }
}
