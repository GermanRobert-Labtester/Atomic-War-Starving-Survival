using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CrisisPresentationCoordinatorTests
    {
        private PowerGridSystem MakeFailingPowerGrid()
        {
            var state = new PowerGridState
            {
                GenerationWatts = 0f,
                FuelUnits = 0f,
                BatteryCapacityWh = 1000f,
                BatteryReserveWh = 0f
            };
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_crit", "Life Support", 200f, PowerGridRoomPriority.Critical, "ls_event")
            };
            return new PowerGridSystem(state, rooms, new SeededRng(42));
        }

        [Fact]
        public void InitialState_IsInactive()
        {
            var coord = new CrisisPresentationCoordinator();
            Assert.False(coord.CurrentSnapshot.IsActive);
            Assert.Equal(CrisisSeverity.None, coord.CurrentSnapshot.Severity);
        }

        [Fact]
        public void PowerBrownout_TriggersCriticalCrisis()
        {
            var coord = new CrisisPresentationCoordinator();
            var power = MakeFailingPowerGrid();

            coord.Bind(power, null, null);
            coord.EvaluateCrisisState();

            var snap = coord.CurrentSnapshot;
            Assert.True(snap.IsActive);
            Assert.Equal(CrisisSeverity.Critical, snap.Severity);
            Assert.Equal("crisis_power_failure", snap.CrisisId);
            Assert.Equal("Power", snap.Kind);
            Assert.NotEmpty(snap.Metrics);
            Assert.NotEmpty(snap.Actions);
            Assert.NotEmpty(snap.Affected);
            Assert.Equal("crisis_critical_stinger", snap.AudioStateId);
        }

        [Fact]
        public void FilterHazard_TriggersSevereCrisis()
        {
            var coord = new CrisisPresentationCoordinator();
            var startingLevel = new StartingLevelSystem();
            startingLevel.State.airHazardWarning = true;

            coord.Bind(null, null, null, startingLevel: startingLevel);
            coord.EvaluateCrisisState();

            var snap = coord.CurrentSnapshot;
            Assert.True(snap.IsActive);
            Assert.Equal(CrisisSeverity.Severe, snap.Severity);
            Assert.Equal("crisis_filter_hazard", snap.CrisisId);
            Assert.Equal("Ventilation", snap.Kind);
            Assert.NotEmpty(snap.Metrics);
            Assert.Equal("crisis_severe_stinger", snap.AudioStateId);
        }

        [Fact]
        public void DiseaseOutbreak_TriggersSevereCrisis()
        {
            var coord = new CrisisPresentationCoordinator();
            var disease = new DiseaseSystem(rng: new SeededRng(1234));
            disease.State.diseases.Add(new DiseaseEntryState
            {
                disease_id = "test_contagion",
                outbreak_active = true
            });

            coord.Bind(null, disease, null);
            coord.EvaluateCrisisState();

            var snap = coord.CurrentSnapshot;
            Assert.True(snap.IsActive);
            Assert.Equal(CrisisSeverity.Severe, snap.Severity);
            Assert.Equal("crisis_disease_outbreak", snap.CrisisId);
            Assert.Equal("Disease", snap.Kind);
            Assert.Equal("crisis_severe_stinger", snap.AudioStateId);
        }

        [Fact]
        public void SevereStorm_TriggersElevatedCrisis()
        {
            var coord = new CrisisPresentationCoordinator();
            var weather = new WeatherSystem();
            weather.ForceWeather(WeatherKind.FalloutStorm);

            coord.Bind(null, null, weather);
            coord.EvaluateCrisisState();

            var snap = coord.CurrentSnapshot;
            Assert.True(snap.IsActive);
            Assert.Equal(CrisisSeverity.Elevated, snap.Severity);
            Assert.Equal("crisis_severe_storm", snap.CrisisId);
            Assert.Equal("Weather", snap.Kind);
            Assert.Equal("crisis_advisory_stinger", snap.AudioStateId);
        }

        [Fact]
        public void Priority_PowerTrumpsDiseaseAndWeather()
        {
            var coord = new CrisisPresentationCoordinator();
            var power = MakeFailingPowerGrid();

            var disease = new DiseaseSystem(rng: new SeededRng(1234));
            disease.State.diseases.Add(new DiseaseEntryState
            {
                disease_id = "test_contagion",
                outbreak_active = true
            });

            var weather = new WeatherSystem();
            weather.ForceWeather(WeatherKind.FalloutStorm);

            coord.Bind(power, disease, weather);
            coord.EvaluateCrisisState();

            // Power failure has highest priority
            Assert.Equal("crisis_power_failure", coord.CurrentSnapshot.CrisisId);
            Assert.Equal(CrisisSeverity.Critical, coord.CurrentSnapshot.Severity);
        }

        [Fact]
        public void CustomCrisis_TriggersAndClears()
        {
            var coord = new CrisisPresentationCoordinator();
            var custom = new CrisisPresentationSnapshot
            {
                CrisisId = "crisis_breach",
                Kind = "Combat",
                Severity = CrisisSeverity.Critical,
                Title = "PERIMETER BREACH",
                Summary = "Hostiles breach airlock doors.",
                AudioStateId = "crisis_critical_stinger"
            };

            int eventCount = 0;
            coord.OnCrisisChanged += snap => eventCount++;

            coord.TriggerCustomCrisis(custom);
            Assert.True(coord.CurrentSnapshot.IsActive);
            Assert.Equal("crisis_breach", coord.CurrentSnapshot.CrisisId);
            Assert.Equal(1, eventCount);

            coord.ClearCrisis();
            Assert.False(coord.CurrentSnapshot.IsActive);
            Assert.Equal(2, eventCount);
        }

        [Fact]
        public void AcknowledgeCurrentCrisis_AppendsLog()
        {
            var coord = new CrisisPresentationCoordinator();
            coord.TriggerCustomCrisis(new CrisisPresentationSnapshot
            {
                CrisisId = "crisis_test",
                Severity = CrisisSeverity.Warning,
                Title = "TEST"
            });

            int initialLogCount = coord.CurrentSnapshot.Log.Count;
            coord.AcknowledgeCurrentCrisis();
            Assert.Equal(initialLogCount + 1, coord.CurrentSnapshot.Log.Count);
            Assert.Contains(coord.CurrentSnapshot.Log, l => l.Message.Contains("acknowledged"));
        }
    }
}
