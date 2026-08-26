using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class IslandBridgesTests
    {
        // ── Task 4: Maritime & Diving System ──
        [Fact]
        public void MaritimeDive_ConductDive_ResolvesAndPreservesState()
        {
            var sys = new MaritimeDiveSystem(new SeededRng(42));
            var reg = sys.RegisterSite("dive_sub_wreck", "Sunken Submarine Wreck", depthMeters: 45f, hazardLevel: 0.3f);
            Assert.Equal(ActionResult.StatusKind.Success, reg.Status);

            var dive = sys.ConductDive("dive_sub_wreck", "diver_1", equipmentQuality: 1.2f);
            Assert.Equal(ActionResult.StatusKind.Success, dive.Status);
            Assert.Single(sys.State.outcomes);
            Assert.True(sys.State.sites[0].isExplored);

            var state = sys.CaptureState();
            var sys2 = new MaritimeDiveSystem(new SeededRng(42));
            sys2.RestoreState(state);
            Assert.Single(sys2.State.sites);
            Assert.Single(sys2.State.outcomes);
            Assert.Equal("dive_sub_wreck", sys2.State.outcomes[0].siteId);
        }

        // ── Task 5: Workshop Reverse Engineering ──
        [Fact]
        public void WorkshopReverseEngineering_ExamineAndRepair_UnlocksRelic()
        {
            var inv = new Inventory.Inventory();
            var research = new ResearchSystem();
            research.RegisterDefaults();
            var crafting = new CraftingSystem(inv);
            var workshop = new WorkshopReverseEngineeringSystem(inv, research, crafting);

            workshop.LoadCatalog(new RelicCatalog
            {
                relics = new List<RelicDefinition>
                {
                    new RelicDefinition
                    {
                        relic_id = "relic_gyroscope",
                        display_name = "Navigational Gyroscope",
                        repair_time_hours = 4f,
                        required_components = new List<string> { "item_scrap_metal" },
                        research_unlock_id = "knowledge_solar_basics"
                    }
                }
            });

            inv.AddById("item_scrap_metal", 2);
            var start = workshop.StartResearch("relic_gyroscope", "engineer_1");
            Assert.Equal(ActionResult.StatusKind.Success, start.Status);

            var tick = workshop.TickProgress(10f);
            Assert.Equal(ActionResult.StatusKind.Success, tick.Status);
            Assert.True(workshop.State.isComplete);
            Assert.Contains("relic_gyroscope", workshop.State.completedRelicIds);
            Assert.True(research.IsManualUnlocked("knowledge_solar_basics"));

            var state = workshop.CaptureState();
            var workshop2 = new WorkshopReverseEngineeringSystem(inv, research, crafting);
            workshop2.RestoreState(state);
            Assert.Contains("relic_gyroscope", workshop2.State.completedRelicIds);
        }

        // ── Task 6: Pharma Lab & Chemical Synthesis ──
        [Fact]
        public void PharmaLab_SynthesizeMedicine_ProducesOutput()
        {
            var inv = new Inventory.Inventory();
            var lab = new PharmaLabSystem(inv, new SeededRng(42));

            lab.LoadCatalog(new PharmaRecipeCatalog
            {
                recipes = new List<PharmaRecipe>
                {
                    new PharmaRecipe
                    {
                        recipe_id = "pharma_rad_blocker",
                        display_name = "Potassium Iodide Solution",
                        input_ids = new List<string> { "item_chemical_reagents" },
                        input_amounts = new List<int> { 2 },
                        output_item_id = "item_rad_blocker_pure",
                        output_amount = 3,
                        base_hours = 2f,
                        purity_target = 0.5f
                    }
                }
            });

            inv.AddById("item_chemical_reagents", 5);
            var start = lab.StartBatch("pharma_rad_blocker", "chemist_1");
            Assert.Equal(ActionResult.StatusKind.Success, start.Status);

            var tick = lab.TickProgress(3f);
            Assert.Equal(ActionResult.StatusKind.Success, tick.Status);
            Assert.False(lab.State.isProcessing);
            Assert.True(inv.CountById("item_rad_blocker_pure") >= 1);
            Assert.Equal(1, lab.State.totalBatchesProduced);

            var state = lab.CaptureState();
            var lab2 = new PharmaLabSystem(inv, new SeededRng(42));
            lab2.RestoreState(state);
            Assert.Equal(1, lab2.State.totalBatchesProduced);
        }

        // ── Task 7: Weather Station ──
        [Fact]
        public void WeatherStation_InstallAndCalibrate_GeneratesForecast()
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "season_nuclear_winter" }, 42);
            var station = new WeatherStationSystem(weather, new SeededRng(42));

            var inst = station.Install(1);
            Assert.Equal(ActionResult.StatusKind.Success, inst.Status);

            var cal = station.Calibrate(2);
            Assert.Equal(ActionResult.StatusKind.Success, cal.Status);
            Assert.True(station.IsOperational);

            var fc = station.GenerateForecast(3);
            Assert.Equal(ActionResult.StatusKind.Success, fc.Status);
            Assert.True(station.State.cachedForecast.Count > 0);

            var state = station.CaptureState();
            var station2 = new WeatherStationSystem(weather, new SeededRng(42));
            station2.RestoreState(state);
            Assert.True(station2.IsOperational);
            Assert.Equal(state.lastForecastDay, station2.State.lastForecastDay);
        }

        // ── Task 8: Orbital Harrow Telemetry ──
        [Fact]
        public void OrbitalHarrow_WarningAndImpact_ResolvesDamage()
        {
            var armor = new SkyLayerArmorSystem();
            armor.SetCellArmor(gridX: 2, CeilingMaterialTier.ReinforcedConcrete, thicknessMeters: 1.5f, durability: 100f);
            var orbital = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));

            orbital.ActivateTelemetry(1);
            orbital.ScheduleImpact(day: 5, gridX: 2, energyMj: 50f);
            var brace = orbital.Brace("item_scrap_metal", 2);
            Assert.Equal(ActionResult.StatusKind.Success, brace.Status);

            orbital.TickDay(5);
            Assert.Contains(5, orbital.State.impactHistory);
            Assert.Equal(5, orbital.State.lastImpactDay);

            var state = orbital.CaptureState();
            var orbital2 = new OrbitalHarrowTelemetrySystem(armor, new SeededRng(42));
            orbital2.RestoreState(state);
            Assert.True(orbital2.State.telemetryActive);
            Assert.Contains(5, orbital2.State.impactHistory);
        }

        // ── Task 9: Expedition Vehicle System ──
        [Fact]
        public void ExpeditionVehicle_RefuelAndTravel_TracksCondition()
        {
            var vehicles = new ExpeditionVehicleSystem(new SeededRng(42));
            vehicles.LoadCatalog(new VehicleCatalog
            {
                vehicles = new List<VehicleDefinition>
                {
                    new VehicleDefinition
                    {
                        vehicle_id = "vehicle_armored_truck",
                        display_name = "Armored 6x6 Hauler",
                        max_fuel = 80f,
                        cargo_capacity = 250f,
                        fuel_consumption_per_km = 0.2f
                    }
                }
            });

            var reg = vehicles.AcquireVehicle("vehicle_armored_truck");
            Assert.Equal(ActionResult.StatusKind.Success, reg.Status);

            var refuel = vehicles.Refuel("vehicle_armored_truck", 50f);
            Assert.Equal(ActionResult.StatusKind.Success, refuel.Status);
            Assert.True(vehicles.State.ownedVehicles["vehicle_armored_truck"].fuel > 0f);

            var prep = vehicles.PrepareForExpedition("vehicle_armored_truck", distanceKm: 50f);
            Assert.True(prep.fuelCost > 0f);

            var state = vehicles.CaptureState();
            var vehicles2 = new ExpeditionVehicleSystem(new SeededRng(42));
            vehicles2.RestoreState(state);
            Assert.True(vehicles2.State.ownedVehicles.ContainsKey("vehicle_armored_truck"));
            Assert.Equal(vehicles.State.ownedVehicles["vehicle_armored_truck"].fuel, vehicles2.State.ownedVehicles["vehicle_armored_truck"].fuel);
        }
    }
}
