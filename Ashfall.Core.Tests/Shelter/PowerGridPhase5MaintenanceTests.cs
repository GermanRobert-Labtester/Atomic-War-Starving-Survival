// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// B5–B8 Phase 5 (Plan 65): base-generator condition/maintenance — the
    /// last "free forever power" closure. Wear only on fuel-burning days,
    /// bounded degradation (never zero while fueled), canonical machine_oil
    /// maintenance via the host, and a 100-default migration contract so
    /// legacy saves restore a healthy generator (no retroactive degradation).
    /// </summary>
    public class PowerGridPhase5MaintenanceTests
    {
        private static PowerGridSystem MakeGrid(float generationWatts = 800f, float fuelUnits = 100f)
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical, "fx_filtration_off")
            };
            var state = new PowerGridState
            {
                GenerationWatts = generationWatts,
                FuelUnits = fuelUnits,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 2000f
            };
            return new PowerGridSystem(state, rooms, new SeededRng(88));
        }

        [Fact]
        public void FreshGenerator_IsAtFullCondition_LegacyParity()
        {
            var grid = MakeGrid();
            Assert.Equal(100f, grid.GeneratorCondition, 1);
            Assert.Equal(1f, grid.GeneratorOutputFactor, 5);
            Assert.Equal(800f, grid.GenerationWatts, 1); // rated output
        }

        [Fact]
        public void Wear_AppliesOnly_OnFuelBurningDays()
        {
            var grid = MakeGrid(fuelUnits: 100f);
            grid.TickDay(1, new SeededRng(1));
            Assert.Equal(100f - PowerGridSystem.GeneratorWearPerDay, grid.GeneratorCondition, 3);

            // Fuel-starved day: no fuel consumed → no wear.
            var dry = MakeGrid(fuelUnits: 0f);
            dry.TickDay(1, new SeededRng(1));
            Assert.Equal(100f, dry.GeneratorCondition, 3);
        }

        [Fact]
        public void DegradedOutput_IsBounded_NeverZeroWhileFueled()
        {
            var grid = MakeGrid();
            grid.State.GeneratorCondition = 30f; // below the 50 threshold

            // factor = 0.5 + 0.5·(30/50) = 0.8 → 640 W effective
            Assert.Equal(0.8f, grid.GeneratorOutputFactor, 3);
            Assert.Equal(640f, grid.GenerationWatts, 1);

            grid.State.GeneratorCondition = 0f;
            Assert.Equal(0.5f, grid.GeneratorOutputFactor, 4); // floor, not zero
            Assert.Equal(400f, grid.GenerationWatts, 1);
        }

        [Fact]
        public void Degradation_AppliesToBaseGeneratorOnly_NotContributions()
        {
            var grid = MakeGrid();
            grid.SetGenerationContribution("nuclear_core", 500f);
            grid.State.GeneratorCondition = 0f;

            // base 800×0.5 = 400 + contribution 500 (unscaled) = 900
            Assert.Equal(900f, grid.GenerationWatts, 1);
        }

        [Fact]
        public void Maintenance_RestoresFullCondition_AndFiresTypedEvent()
        {
            var grid = MakeGrid();
            grid.TickDay(1, new SeededRng(1)); // wear
            Assert.True(grid.GeneratorCondition < 100f);

            PowerGridEvent? evt = null;
            grid.OnPowerChanged += e => evt = e;
            Assert.True(grid.PerformGeneratorMaintenance(out var reason), reason);

            Assert.Equal(100f, grid.GeneratorCondition, 1);
            Assert.Equal(1f, grid.GeneratorOutputFactor, 5);
            Assert.NotNull(evt);
            Assert.Equal(PowerGridEventKind.GeneratorMaintained, evt!.Kind);

            // Servicing a healthy generator is blocked — no wasted action.
            Assert.False(grid.PerformGeneratorMaintenance(out var blocked));
            Assert.Equal("condition_full", blocked);
        }

        [Fact]
        public void DegradedGenerator_ShedsOptionalLoads_AtSameFuelState()
        {
            // 500 W rated × 0.8 condition = 400 W effective. Rooms: critical
            // 180 + standard 160 = 340 W served; low workshop 300 W sheds.
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical, "fx_filtration_off"),
                new PowerGridRoom("room_greenhouse", "Greenhouse", 160f,
                    PowerGridRoomPriority.Standard, "fx_grow_lights_off"),
                new PowerGridRoom("room_workshop", "Workshop", 300f,
                    PowerGridRoomPriority.Low, "fx_workshop_offline")
            };
            var state = new PowerGridState
            {
                GenerationWatts = 500f, FuelUnits = 100f,
                BatteryCapacityWh = 4000f, BatteryReserveWh = 0f
            };
            var worn = new PowerGridSystem(state, rooms, new SeededRng(88));
            worn.State.GeneratorCondition = 30f;

            var sum = worn.TickDay(1, new SeededRng(2));
            Assert.Equal(400f, sum.GenerationWatts, 1);
            Assert.False(sum.HasCriticalDeficit); // critical served
            Assert.Contains("room_workshop", sum.ShedRoomIds);
        }

        [Fact]
        public void GeneratorCondition_SaveRoundTrips_Exactly()
        {
            var grid = MakeGrid();
            grid.TickDay(1, new SeededRng(1));
            grid.TickDay(2, new SeededRng(2));
            float expected = grid.GeneratorCondition;

            var restored = MakeGrid();
            restored.RestoreState(grid.CaptureState());

            Assert.Equal(expected, restored.GeneratorCondition, 3);
        }

        [Fact]
        public void LegacySave_WithoutConditionField_RestoresHealthy()
        {
            // Pre-Phase-5 payload: no GeneratorCondition key. The initializer
            // 100 is the migration contract — no retroactive degradation.
            const string legacy = "{\"SimDay\":47,\"GenerationWatts\":813.5,\"FuelUnits\":62.25," +
                "\"BatteryReserveWh\":1234.5,\"BatteryCapacityWh\":4000," +
                "\"ClosedBreakers\":[],\"TrippedRooms\":[],\"Priorities\":[],\"LastSurgeDay\":31}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<PowerGridState>(legacy);
            Assert.NotNull(state);

            var grid = MakeGrid();
            grid.RestoreState(state!);
            Assert.Equal(100f, grid.GeneratorCondition, 1);
            Assert.Equal(1f, grid.GeneratorOutputFactor, 5);
        }

        [Fact]
        public void Normalize_ClampsInvalidCondition()
        {
            var grid = MakeGrid();
            grid.State.GeneratorCondition = 250f;
            grid.State.NormalizeAndValidate(grid.Rooms);
            Assert.Equal(100f, grid.State.GeneratorCondition, 1);

            grid.State.GeneratorCondition = -5f;
            grid.State.NormalizeAndValidate(grid.Rooms);
            Assert.Equal(0f, grid.State.GeneratorCondition, 1);
        }
    }
}
