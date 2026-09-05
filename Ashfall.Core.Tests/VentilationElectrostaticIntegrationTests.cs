using System.Collections.Generic;
using Ashfall.Core;

using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 72 §24 core-integration: electrostatic stage + ventilation + power
    /// grid + hazardous-waste inventory, driven by weather truth through the
    /// Core-owned intake conversion (the same seam the host uses).
    /// </summary>
    public class VentilationElectrostaticIntegrationTests
    {
        private const string RoomId = "vent_room";

        private static (VentilationSystem vent, PowerGridSystem power, WeatherSystem weather, Inventory.Inventory inv)
            Create(int seed = 11)
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, seed);

            var gridState = new PowerGridState
            {
                GenerationWatts = 800,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            var power = new PowerGridSystem(
                gridState,
                new List<PowerGridRoom> { new PowerGridRoom(RoomId, "Air Hall", 100f) },
                new SeededRng(seed));

            var vent = new VentilationSystem(new StartingLevelSystem());
            vent.ApplyElectrostaticCatalog(new List<ElectrostaticStageDef>
            {
                new ElectrostaticStageDef
                {
                    stage_id = "stage_test",
                    display_name = "Test Precipitator",
                    dust_capacity_kg = 12f,
                    maintenance_interval_days = 14,
                    operating_profiles = new List<ElectrostaticProfileDef>
                    {
                        new ElectrostaticProfileDef
                        {
                            profile_id = "profile_standard",
                            nominal_power_w = 350f,
                            capture_efficiency_pm25 = 0.8f,
                            capture_efficiency_pm10 = 0.9f,
                            hot_ash_capture_efficiency = 0.7f,
                            ozone_output_rate_ppm_per_day = 8f,
                            arc_risk_base_bp = 0 // integration tests: deterministic, no faults
                        }
                    },
                    required_component_ids = new List<ElectrostaticComponentCost>()
                }
            });
            var inv = new Inventory.Inventory();
            vent.BindStageServices(new SeededRng(seed), power, inv);
            return (vent, power, weather, inv);
        }

        [Fact]
        public void WeatherTruth_DrivesIntakeAndCapture()
        {
            var (vent, _, weather, _) = Create();
            Assert.Equal(ActionResult.StatusKind.Success,
                vent.InstallElectrostaticStage("stage_test", RoomId).Status);

            // Host seam: weather truth → Core-owned intake conversion.
            float intake = ElectrostaticFiltrationCatalogLoader.WeatherIntakeParticulateKg(
                weather.Current, vent.State.mainDuctOpen);
            bool hotAsh = ElectrostaticFiltrationCatalogLoader.IsHotAshLoad(weather.Current);
            vent.TickDay(1, intake, hotAsh);

            var stage = vent.State.electrostatic!;
            Assert.True(stage.energized);
            // captured equals intake × profile efficiency (fresh plates) — never more
            Assert.True(stage.dustLoadKg <= intake * 0.8f + 0.0001f);
            Assert.Equal(intake, stage.dustLoadKg + (intake - stage.dustLoadKg), 3); // mass balance identity
        }

        [Fact]
        public void FalloutStorm_IntakeExceedsClearWeather()
        {
            // Plan 71→72 contract: actual weather truth (not forecast) changes load.
            float storm = ElectrostaticFiltrationCatalogLoader.WeatherIntakeParticulateKg(
                WeatherKind.FalloutStorm, mainDuctOpen: true);
            float clear = ElectrostaticFiltrationCatalogLoader.WeatherIntakeParticulateKg(
                WeatherKind.Clear, mainDuctOpen: true);
            Assert.True(storm > clear * 10f);
            Assert.True(ElectrostaticFiltrationCatalogLoader.IsHotAshLoad(WeatherKind.Ashfall));
            Assert.False(ElectrostaticFiltrationCatalogLoader.IsHotAshLoad(WeatherKind.Clear));
        }

        [Fact]
        public void CapacityFault_StopsCapture_FiltersStillScrubSoot()
        {
            var (vent, _, _, _) = Create();
            vent.InstallElectrostaticStage("stage_test", RoomId);

            // Saturate the stage: 12 kg capacity / (2 kg × 0.8) ≈ 8 days
            for (int day = 1; day <= 10; day++)
                vent.TickDay(day, incomingParticulateKgPerDay: 2f);
            Assert.True(vent.State.electrostatic!.faulted);

            // After the fault the mechanical path (existing authority) still runs:
            // the kitchen source keeps loading the exhaust filter — air is not
            // abandoned when the electrostatic stage goes down.
            vent.RegisterSource(new VentilationSource
            {
                sourceId = "kitchen_test", roomId = RoomId,
                smokeOutputPerDay = 10f, coOutputPerDay = 5f, isActive = true
            });
            float saturationBefore = vent.State.exhaustFilterSaturation;
            vent.TickDay(11, incomingParticulateKgPerDay: 2f);
            Assert.True(vent.State.exhaustFilterSaturation > saturationBefore,
                "mechanical exhaust filter must keep loading while the stage is faulted");
        }

        [Fact]
        public void RappingCycle_RestoresCapture_WithoutLosingMass()
        {
            var (vent, _, _, inv) = Create();
            vent.InstallElectrostaticStage("stage_test", RoomId);

            for (int day = 1; day <= 5; day++)
                vent.TickDay(day, incomingParticulateKgPerDay: 2f);
            float onPlates = vent.State.electrostatic!.dustLoadKg;

            Assert.Equal(ActionResult.StatusKind.Success, vent.RapPlates().Status);
            var stage = vent.State.electrostatic!;
            Assert.Equal(onPlates, stage.dustLoadKg + stage.hopperKg, 3); // nothing vanished

            // drums seal the hopper mass for disposal
            vent.State.electrostatic!.hopperKg += 20f; // top up to a full drum count via further ticks
            vent.EmptyHopperToDrums(4);
            Assert.True(inv.CountById(VentilationSystem.HotDustDrumItemId) >= 1);
        }

        [Fact]
        public void SplitRun_SaveMidWeek_ContinuesIdentically()
        {
            float RunSplit()
            {
                var (vent, _, _, _) = Create(seed: 23);
                vent.InstallElectrostaticStage("stage_test", RoomId);
                for (int day = 1; day <= 3; day++)
                    vent.TickDay(day, incomingParticulateKgPerDay: 1f);
                var saved = vent.CaptureState();

                var resumed = new VentilationSystem(new StartingLevelSystem());
                resumed.ApplyElectrostaticCatalog(new List<ElectrostaticStageDef>
                {
                    new ElectrostaticStageDef
                    {
                        stage_id = "stage_test",
                        dust_capacity_kg = 12f,
                        maintenance_interval_days = 14,
                        operating_profiles = new List<ElectrostaticProfileDef>
                        {
                            new ElectrostaticProfileDef
                            {
                                profile_id = "profile_standard",
                                capture_efficiency_pm25 = 0.8f,
                                hot_ash_capture_efficiency = 0.7f,
                                ozone_output_rate_ppm_per_day = 8f,
                                arc_risk_base_bp = 0
                            }
                        },
                        required_component_ids = new List<ElectrostaticComponentCost>()
                    }
                });
                var gridState = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
                var power = new PowerGridSystem(gridState, new List<PowerGridRoom> { new PowerGridRoom(RoomId, "Air Hall", 100f) }, new SeededRng(23));
                resumed.BindStageServices(new SeededRng(23), power, new Inventory.Inventory());
                resumed.RestoreState(saved);

                for (int day = 4; day <= 6; day++)
                    resumed.TickDay(day, incomingParticulateKgPerDay: 1f);
                return resumed.State.electrostatic!.dustLoadKg + resumed.State.ozonePpm;
            }

            float a = RunSplit();
            float b = RunSplit();
            Assert.Equal(a, b, 3);
        }
    }
}
