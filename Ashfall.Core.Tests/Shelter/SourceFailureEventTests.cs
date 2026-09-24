// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// B5–B8 expansion (flagship §27): source maintenance/failure events.
    /// Degradation edges fire once per wear cycle from the standardized
    /// sources; restores re-seed the latches so a reload never replays a
    /// warning; servicing resets the cycle.
    /// </summary>
    public class SourceFailureEventTests
    {
        private static PowerGridSystem MakeGrid(float fuelUnits = 20f)
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical, "fx_filtration_off")
            };
            var state = new PowerGridState
            {
                GenerationWatts = 800f,
                FuelUnits = fuelUnits,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 0f
            };
            return new PowerGridSystem(state, rooms, new SeededRng(91));
        }

        [Fact]
        public void GeneratorWorn_FiresOnce_PerWearCycle()
        {
            var grid = MakeGrid(fuelUnits: 100f);
            int wornCount = 0;
            grid.OnPowerChanged += e => { if (e.Kind == PowerGridEventKind.GeneratorWorn) wornCount++; };

            // Drive to just above the degradation threshold, then burn across it
            // (wear is 0.25/burning day — realistic crossings take ~200 days).
            grid.State.GeneratorCondition = PowerGridSystem.GeneratorDegradationThreshold + 0.1f;
            grid.TickDay(1, new SeededRng(91)); // wear crosses below 50
            Assert.Equal(1, wornCount);

            grid.TickDay(2, new SeededRng(92));
            grid.TickDay(3, new SeededRng(93));
            Assert.Equal(1, wornCount); // never re-fires while worn
        }

        [Fact]
        public void GeneratorWorn_RestoredWornGrid_DoesNotReplay()
        {
            var grid = MakeGrid(fuelUnits: 100f);
            grid.State.GeneratorCondition = PowerGridSystem.GeneratorDegradationThreshold + 0.1f;
            grid.TickDay(1, new SeededRng(91)); // crossing edge fires pre-restore
            Assert.True(grid.GeneratorCondition < PowerGridSystem.GeneratorDegradationThreshold);
            var saved = grid.CaptureState();

            var restored = MakeGrid();
            restored.RestoreState(saved);
            int wornCount = 0;
            restored.OnPowerChanged += e => { if (e.Kind == PowerGridEventKind.GeneratorWorn) wornCount++; };
            restored.TickDay(7, new SeededRng(92));

            Assert.Equal(0, wornCount); // already-worn: no replayed warning
        }

        [Fact]
        public void GeneratorWorn_ServiceResetsLatch()
        {
            var grid = MakeGrid(fuelUnits: 100f);
            int wornCount = 0;
            grid.OnPowerChanged += e => { if (e.Kind == PowerGridEventKind.GeneratorWorn) wornCount++; };

            grid.State.GeneratorCondition = PowerGridSystem.GeneratorDegradationThreshold + 0.1f;
            grid.TickDay(1, new SeededRng(93));
            Assert.Equal(1, wornCount);

            Assert.True(grid.PerformGeneratorMaintenance(out _)); // clears the latch
            grid.State.GeneratorCondition = PowerGridSystem.GeneratorDegradationThreshold + 0.1f;
            grid.TickDay(2, new SeededRng(94));
            Assert.Equal(2, wornCount); // latch cleared by service: a new cycle warns again
        }

        [Fact]
        public void FuelStarved_FiresOnce_WhileDry_RecoversOnRefuel()
        {
            var grid = MakeGrid(fuelUnits: 5f);
            int starvedCount = 0;
            grid.OnPowerChanged += e => { if (e.Kind == PowerGridEventKind.FuelStarved) starvedCount++; };

            for (int day = 1; day <= 5; day++)
                grid.TickDay(day, new SeededRng(95));
            Assert.True(grid.State.FuelUnits <= 0f);
            Assert.Equal(1, starvedCount); // dry days 2..5 warn once

            grid.AddFuel(100f);
            grid.TickDay(6, new SeededRng(96));
            Assert.Equal(1, starvedCount);

            grid.State.FuelUnits = 0f; // dry again → second failure edge
            grid.TickDay(7, new SeededRng(97));
            Assert.Equal(2, starvedCount);
        }

        [Fact]
        public void FuelStarved_RestoreReseedsLatch()
        {
            var grid = MakeGrid(fuelUnits: 0f);
            grid.TickDay(1, new SeededRng(98)); // starved edge fires
            var restored = MakeGrid(fuelUnits: 0f);
            restored.RestoreState(grid.CaptureState());

            int starvedCount = 0;
            restored.OnPowerChanged += e => { if (e.Kind == PowerGridEventKind.FuelStarved) starvedCount++; };
            restored.TickDay(2, new SeededRng(99));
            Assert.Equal(0, starvedCount); // re-seeded from restored dry state: no replay
        }

        [Fact]
        public void DeepWell_PumpWorn_FiresOnce_RestoreDoesNotReplay_ServiceResets()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            var well = new DeepWellSystem(grid, water, NullLog.Instance);
            well.TryBuild(true, out _);
            int wornCount = 0;
            well.OnPumpWorn += _ => wornCount++;

            // Wear is 0.2/pumping day (400-day lifetime); drive to just above
            // the threshold, then cross it with one pumping day.
            var nearWornState = well.CaptureState();
            nearWornState.condition = DeepWellSystem.PumpWornThreshold + 0.1f;
            well.RestoreState(nearWornState);
            well.TickDay(1); // crosses to 39.9 → warn edge fires
            Assert.Equal(1, wornCount);

            var restored = new DeepWellSystem(grid, water, NullLog.Instance);
            restored.RestoreState(well.CaptureState());
            int restoredWorn = 0;
            restored.OnPumpWorn += _ => restoredWorn++;
            restored.TickDay(400);
            Assert.Equal(0, restoredWorn); // no replay from an already-worn restore

            restored.PerformMaintenance(out _);
            restored.TickDay(401); // condition 100 → far from threshold: no fire
            Assert.Equal(0, restoredWorn);
        }

        [Fact]
        public void Condenser_MembraneSpent_FiresOnce_RestoreDoesNotReplay()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            var c = new AtmosphericCondenserSystem(grid, water,
                MakeRainWeather(), NullLog.Instance);
            c.TryBuild(true, out _);
            int spentCount = 0;
            c.OnMembraneSpent += _ => spentCount++;

            // 15 L/day at index 1.0 wears the membrane 0.5/day (200-day
            // lifetime); drive to just above zero, then cross.
            var nearSpentState = c.CaptureState();
            nearSpentState.membraneIntegrity = 0.4f;
            c.RestoreState(nearSpentState);
            c.TickDay(1); // integrity → 0 → spent edge fires
            Assert.Equal(1, spentCount);
            Assert.Equal(0f, c.MembraneIntegrity, 3);

            var restored = new AtmosphericCondenserSystem(grid, water,
                MakeRainWeather(), NullLog.Instance);
            restored.RestoreState(c.CaptureState());
            int restoredSpent = 0;
            restored.OnMembraneSpent += _ => restoredSpent++;
            restored.TickDay(200);
            Assert.Equal(0, restoredSpent); // spent latch re-seeded: no replay

            restored.ReplaceMembrane(out _);
            restored.TickDay(201);
            Assert.Equal(0, restoredSpent); // fresh membrane: clean cycle
        }

        private static WeatherSystem MakeRainWeather()
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            weather.ForceWeather(WeatherKind.Rain);
            return weather;
        }
    }
}
