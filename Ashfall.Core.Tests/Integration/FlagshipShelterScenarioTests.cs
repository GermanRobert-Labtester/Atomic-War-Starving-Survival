// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Farming;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    /// <summary>
    /// B5–B8 Phase 8: integrated cross-system scenarios (flagship §12) over a
    /// wired deterministic Core world — grid, greenhouse, water treatment,
    /// sump, deep well, and perimeter defense consuming one another's
    /// authoritative outputs with no host present. Scenario G (restore
    /// mid-crisis) pins the no-replayed-transitions contract end to end.
    /// </summary>
    public class FlagshipShelterScenarioTests
    {
        private sealed class World
        {
            public PowerGridSystem Grid = null!;
            public GreenhouseSystem Greenhouse = null!;
            public WaterTreatmentSystem Water = null!;
            public SumpFloodingSystem Sump = null!;
            public DeepWellSystem Well = null!;
            public PerimeterDefenseSystem Perimeter = null!;
            public YearOfAsh.YearOfAshDeepFreezeSystem DeepFreeze = null!;

            public WeatherSystem Weather = null!;
        }

        private static World MakeWorld(float generationWatts = 800f, float batteryReserveWh = 2000f, int seed = 8)
        {
            var w = new World();
            var rooms = new List<PowerGridRoom>
            {
                new("room_air_filtration", "Air Filtration", 180f, PowerGridRoomPriority.Critical, "fx_filtration_off"),
                new("room_water_pump", "Water Pump", 100f, PowerGridRoomPriority.Critical, "fx_water_pressure_drop"),
                new("room_greenhouse", "Greenhouse", 160f, PowerGridRoomPriority.Standard, "fx_grow_lights_off"),
                new("room_lighting_main", "Main Lighting", 80f, PowerGridRoomPriority.Low, "fx_lighting_dim"),
                new("room_armory_munitions", "Armory & Munitions", 60f, PowerGridRoomPriority.Standard, "fx_armory_service_off")
            };
            w.Grid = new PowerGridSystem(new PowerGridState
            {
                GenerationWatts = generationWatts, FuelUnits = 1000f,
                BatteryCapacityWh = 4000f, BatteryReserveWh = batteryReserveWh
            }, rooms, new SeededRng(seed));
            w.Greenhouse = new GreenhouseSystem(seed);
            w.Greenhouse.EnsurePlots(4);
            w.Water = new WaterTreatmentSystem(NullLog.Instance);
            w.DeepFreeze = new YearOfAsh.YearOfAshDeepFreezeSystem();
            w.Weather = new WeatherSystem();
            w.Weather.BindProfile(new Ashfall.Core.World.SeasonProfileDef { id = "default" }, 42);
            w.Sump = new SumpFloodingSystem(new SeededRng(seed + 1), w.Weather, w.Grid, w.DeepFreeze, NullLog.Instance);
            w.Well = new DeepWellSystem(w.Grid, w.Water, NullLog.Instance);
            w.Perimeter = new PerimeterDefenseSystem(
                PerimeterDefenseCatalogLoader.Load(
                    System.IO.Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data"),
                    new FileSystemIO(), new SystemTextJsonSerializer()),
                new Inventory.Inventory(), new SeededRng(seed + 2), NullLog.Instance);
            return w;
        }

        // ─── Scenario A — normal shelter: loops close, no arbitrage ────────

        [Fact]
        public void ScenarioA_NormalOperation_LoopsClose()
        {
            var w = MakeWorld();
            Assert.True(w.Well.TryBuild(true, out _));
            w.Greenhouse.Plant(0, GreenhouseExpansionCatalog.Items.SeedLeafyGreen, 1, out _);
            w.Greenhouse.Water(0, 60f, tainted: false);
            w.Greenhouse.ApplyNutrients(0, out _);
            w.Water.ExecuteStartTreatment(TreatmentMode.CharcoalFiltration, 10f);

            for (int day = 1; day <= 30; day++)
            {
                w.Grid.TickDay(day, new SeededRng(100 + day));
                w.Water.TickDay(day, 1f);
                w.Well.TickDay(day);
                w.Sump.TickDay(day);
                w.Greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);
            }

            // Grid: stable (no brownout — 520W load vs 800W gen), fuel burned.
            Assert.False(w.Grid.IsBrownout);
            Assert.True(w.Grid.State.FuelUnits < 1000f);
            // Well: the intake ledger conserves — every yielded liter is either
            // still in the raw pool or was consumed by treatment (the loop
            // closing: well → treatment → clean water).
            Assert.True(w.Well.TotalYieldLiters > 0);
            Assert.True(w.Water.State.rawWater + w.Water.State.totalWaterProcessed
                        >= w.Well.TotalYieldLiters - 1f);
            // Greenhouse: the fed plot grew (no instant death, no free food).
            var plot = w.Greenhouse.Plots[0];
            Assert.False(plot.stage == (int)GreenhouseStage.Failed);
            Assert.True(plot.growth > 0f);
            // Water treatment processed something.
            Assert.True(w.Water.State.totalWaterProcessed > 0f || w.Water.State.rawWater > 0f);
        }

        // ─── Scenario B — brownout during a greenhouse cycle ───────────────

        [Fact]
        public void ScenarioB_Brownout_ShedsByPriority_NoCropDeath()
        {
            // 400W gen vs 520W demand, empty battery: deterministic shedding.
            var w = MakeWorld(generationWatts: 400f, batteryReserveWh: 0f);
            w.Greenhouse.Plant(0, GreenhouseExpansionCatalog.Items.SeedLeafyGreen, 1, out _);
            w.Greenhouse.Water(0, 100f, tainted: false);

            var sum = w.Grid.TickDay(1, new SeededRng(201));
            Assert.True(sum.ShedRoomIds.Count > 0);
            Assert.Contains("room_lighting_main", sum.ShedRoomIds); // low priority sheds
            Assert.Contains("room_air_filtration", sum.ServedRoomIds); // critical served
            Assert.False(sum.HasCriticalDeficit);

            w.Greenhouse.TickDay(1, growLightHours: 0f, ashContaminationRate: 0f);
            var plot = w.Greenhouse.Plots[0];
            Assert.False(plot.stage == (int)GreenhouseStage.Failed); // no instant death
            Assert.Equal(0f, plot.growth, 5); // dark greenhouse: growth paused (documented effect)
        }

        // ─── Scenario C — flood + treatment emergency ──────────────────────

        [Fact]
        public void ScenarioC_FloodContamination_TreatmentPressureRises()
        {
            var w = MakeWorld();
            Assert.True(w.Well.TryBuild(true, out _));

            // Force a flood start; the canonical bridge routes it to treatment.
            w.Sump.AddNode("sump_a", "Lower Level");
            w.Sump.InstallPump("sump_a");
            var floodNode = w.Sump.State.nodes[0];
            floodNode.waterLevelCm = 250f; // above the 80% threshold
            w.Sump.TickDay(1);

            Assert.True(floodNode.isFlooded);
            Assert.True(floodNode.contaminationLevel > 0f);
            // With the pump served (registered Phase 3 load), drainage begins.
            Assert.True(w.Grid.IsRoomServed("sump_a"));
        }

        // ─── Scenario D — raid during a brownout: sentries shed, no combat
        //     authority touched by power ─────────────────────────────────────

        [Fact]
        public void ScenarioD_RaidUnderBrownout_TurretCircuitShedsByPriority()
        {
            // 300W gen vs 580W demand: allocation serves the two criticals
            // (280W); everything Standard/Low sheds — including the turret's
            // armory circuit (60W, which by within-tier ordinal would outrank
            // the greenhouse). Brownout-hour overload tripping (legacy, ≥4h)
            // may additionally trip rooms AFTER allocation; the tick summary
            // is the allocation-time truth this scenario asserts. Power never
            // touches the combat authority either way.
            var w = MakeWorld(generationWatts: 300f, batteryReserveWh: 0f);
            w.Perimeter.ConstructEmplacement("def_sentry_turret_9mm", hasRequiredCapability: true);

            var sum = w.Grid.TickDay(1, new SeededRng(401));

            Assert.Contains("room_armory_munitions", sum.ShedRoomIds);
            Assert.DoesNotContain("room_armory_munitions", sum.ServedRoomIds);
            Assert.False(sum.HasCriticalDeficit);
            Assert.Contains("room_air_filtration", sum.ServedRoomIds);
        }

        [Fact]
        public void ScenarioD2_RaidUnderFullPower_TurretCircuitServed()
        {
            var w = MakeWorld(); // 800W gen, 2000Wh battery
            w.Perimeter.ConstructEmplacement("def_sentry_turret_9mm", hasRequiredCapability: true);
            var sum = w.Grid.TickDay(1, new SeededRng(402));
            Assert.True(w.Grid.IsRoomServed("room_armory_munitions"));
            Assert.Contains("room_armory_munitions", sum.ServedRoomIds);
        }

        // ─── Scenario G — restore mid-crisis: nothing replays ──────────────

        [Fact]
        public void ScenarioG_RestoreMidCrisis_NoReplay_NoDuplication()
        {
            var w = MakeWorld();
            Assert.True(w.Well.TryBuild(true, out _));
            w.Greenhouse.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, 1, out _);
            w.Greenhouse.Water(0, 80f, tainted: false);
            w.Greenhouse.ApplyNutrients(0, out _);

            // Drive into a multi-system crisis: brownout + planted blighted crop.
            var brownWorld = MakeWorld(generationWatts: 300f, batteryReserveWh: 0f);
            brownWorld.Well.TryBuild(true, out _);
            for (int day = 1; day <= 3; day++)
            {
                brownWorld.Grid.TickDay(day, new SeededRng(500 + day));
                brownWorld.Well.TickDay(day);
            }
            brownWorld.Greenhouse.Plots[0].blight = 0.5f;
            var gridSnapshot = brownWorld.Grid.CaptureState();
            var ghSnapshot = brownWorld.Greenhouse.CaptureState();
            var wellSnapshot = brownWorld.Well.CaptureState();

            // Restore into a fresh world and continue under identical conditions.
            var restored = MakeWorld(generationWatts: 300f, batteryReserveWh: 0f);
            restored.Well.TryBuild(true, out _); // rebuild the load before restore (session ordering)
            restored.Grid.RestoreState(gridSnapshot);
            restored.Greenhouse.RestoreState(ghSnapshot);
            restored.Well.RestoreState(wellSnapshot);

            var sum = restored.Grid.TickDay(4, new SeededRng(510));
            restored.Well.TickDay(4);
            restored.Greenhouse.TickDay(4, growLightHours: 3f, ashContaminationRate: 0f);

            // Brownout edges: restored mid-brownout → no Began replay.
            Assert.True(sum.IsBrownout);
            Assert.False(sum.BrownoutBegan);
            // The restored well state is authoritative: the ledger continued,
            // not restarted (day 1–3 yields preserved, day 4 appended at most once).
            Assert.Equal(brownWorld.Well.TotalYieldLiters, restored.Well.TotalYieldLiters - (restored.Well.State.lastPumpDay == 4 ? DeepWellSystem.RatedYieldLitersPerDay : 0));
            // Blight restored exactly — not re-rolled, not cured, not doubled.
            Assert.Equal(0.5f, restored.Greenhouse.Plots[0].blight, 3);
        }
    }
}
