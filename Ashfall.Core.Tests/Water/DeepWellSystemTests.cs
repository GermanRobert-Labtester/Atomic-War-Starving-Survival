// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Water
{
    /// <summary>
    /// B5–B8 Phase 6 (Plan 66 §9.8): deep-well foundation. The well is a
    /// bounded source identity — build-gated on the live capability query,
    /// registered as a critical grid load (§6.3), pushing a bounded RAW-water
    /// yield into the canonical treatment authority via the Plan 189 intake
    /// seam. Never potable directly, never a second counter, wear only on
    /// pumping days, canonical maintenance, deterministic, save-safe.
    /// </summary>
    public class DeepWellSystemTests
    {
        private static PowerGridSystem MakeGrid(float generationWatts = 800f, float batteryReserveWh = 2000f)
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical, "fx_filtration_off")
            };
            var state = new PowerGridState
            {
                GenerationWatts = generationWatts,
                FuelUnits = 100f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = batteryReserveWh
            };
            return new PowerGridSystem(state, rooms, new SeededRng(66));
        }

        private static DeepWellSystem MakeWell(PowerGridSystem? grid = null, WaterTreatmentSystem? water = null)
        {
            return new DeepWellSystem(
                grid ?? MakeGrid(),
                water ?? new WaterTreatmentSystem(),
                NullLog.Instance);
        }

        [Fact]
        public void Build_RequiresCapability_ResearchNeverGrantsInfrastructure()
        {
            var well = MakeWell();
            Assert.False(well.TryBuild(hasRequiredCapability: false, out var reason));
            Assert.Equal("missing_knowledge", reason);
            Assert.False(well.IsBuilt); // research unlocked but no build → no well (§15.3)
        }

        [Fact]
        public void Build_IsIdempotent_AndRegistersCriticalGridLoad()
        {
            var grid = MakeGrid();
            var well = MakeWell(grid);
            float drawBefore = grid.TotalDrawWatts;

            Assert.True(well.TryBuild(hasRequiredCapability: true, out var reason), reason);
            Assert.False(well.TryBuild(hasRequiredCapability: true, out var again)); // already built
            Assert.Equal("already_built", again);

            Assert.True(grid.IsRoomServed(DeepWellSystem.PowerRoomId));
            Assert.Equal(drawBefore + DeepWellSystem.PumpDrawWatts, grid.TotalDrawWatts, 1);
        }

        [Fact]
        public void Pumping_PushesBoundedRawYield_IntoTreatmentIntake()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            var well = new DeepWellSystem(grid, water, NullLog.Instance);
            well.TryBuild(true, out _);

            well.TickDay(1);

            Assert.Equal((long)DeepWellSystem.RatedYieldLitersPerDay, water.State.rawWater);
            Assert.Equal((long)DeepWellSystem.RatedYieldLitersPerDay, well.TotalYieldLiters);
            Assert.Equal(100f - DeepWellSystem.WearPerPumpingDay, well.Condition, 3);
            Assert.Equal(1, well.State.lastPumpDay);
        }

        [Fact]
        public void Pumping_YieldsRawWater_NeverPotable()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            var well = new DeepWellSystem(grid, water, NullLog.Instance);
            well.TryBuild(true, out _);

            well.TickDay(1);

            Assert.Equal(0f, water.State.cleanWater); // treatment owns purification
            Assert.Equal((long)DeepWellSystem.RatedYieldLitersPerDay, water.State.rawWater);
        }

        [Fact]
        public void Pumping_Stops_WhenDisabledOrUnpowered()
        {
            var grid = MakeGrid(generationWatts: 0f, batteryReserveWh: 0f); // nothing served
            var water = new WaterTreatmentSystem();
            var well = new DeepWellSystem(grid, water, NullLog.Instance);
            well.TryBuild(true, out _);

            well.TickDay(1); // unpowered (demand 300 > gen 0, battery 0)
            Assert.Equal(0L, well.TotalYieldLiters);
            Assert.Equal(100f, well.Condition, 3); // idle pump does not wear

            // Powered grid, player-disabled pump: still no yield, no wear.
            var grid2 = MakeGrid();
            var well2 = new DeepWellSystem(grid2, water, NullLog.Instance);
            well2.TryBuild(true, out _);
            Assert.True(well2.SetEnabled(false, out _));
            well2.TickDay(2);
            Assert.Equal(0L, well2.TotalYieldLiters);
            Assert.Equal(100f, well2.Condition, 3);
        }

        [Fact]
        public void Pumping_Skips_AdvisoryBlockedIntake_ConservingWater()
        {
            // Plan 189 intake advisory isolates the deep-well source → zero
            // grant: the water stays in the aquifer (conserved, no free
            // storage, no wear — the pump never moved water).
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            water.RegisterContaminationAdvisory("adv_test", "severe",
                new List<string> { DeepWellSystem.SourceId });
            var well = new DeepWellSystem(grid, water, NullLog.Instance);
            well.TryBuild(true, out _);

            well.TickDay(1);

            Assert.Equal(0L, well.TotalYieldLiters);
            Assert.Equal(100f, well.Condition, 3); // no pumping → no wear
        }

        [Fact]
        public void Maintenance_RestoresCondition_BlockedWhenHealthy()
        {
            var well = MakeWell();
            Assert.False(well.PerformMaintenance(out var notBuilt));
            Assert.Equal("not_built", notBuilt);

            well.TryBuild(true, out _);
            well.TickDay(1);
            Assert.True(well.Condition < 100f);

            Assert.True(well.PerformMaintenance(out var reason), reason);
            Assert.Equal(100f, well.Condition, 1);

            Assert.False(well.PerformMaintenance(out var full));
            Assert.Equal("condition_full", full);
        }

        [Fact]
        public void SetEnabled_IsIdempotent_AndBlockedWhenUnbuilt()
        {
            var well = MakeWell();
            Assert.False(well.SetEnabled(false, out var unbuilt));
            Assert.Equal("not_built", unbuilt);

            well.TryBuild(true, out _);
            Assert.True(well.SetEnabled(false, out _));
            Assert.True(well.SetEnabled(false, out _)); // idempotent
            Assert.False(well.IsEnabled);
        }

        [Fact]
        public void SaveRoundTrip_PreservesState_AndReRegistersLoad()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            var well = new DeepWellSystem(grid, water, NullLog.Instance);
            well.TryBuild(true, out _);
            well.TickDay(1);
            well.TickDay(2);
            var saved = well.CaptureState();

            // Fresh session: new powered grid + treatment, restore must
            // re-expose the load (served again without replaying the pump run).
            var grid2 = MakeGrid();
            var water2 = new WaterTreatmentSystem();
            var well2 = new DeepWellSystem(grid2, water2, NullLog.Instance);
            well2.RestoreState(saved);

            Assert.True(well2.IsBuilt);
            Assert.Equal(100f - 2f * DeepWellSystem.WearPerPumpingDay, well2.Condition, 2);
            Assert.Equal(2L * (long)DeepWellSystem.RatedYieldLitersPerDay, well2.TotalYieldLiters);
            Assert.True(grid2.IsRoomServed(DeepWellSystem.PowerRoomId)); // load re-registered, served
            Assert.Equal(180f + DeepWellSystem.PumpDrawWatts, grid2.TotalDrawWatts, 1);
            // Restore does not replay the pumping: treatment pool stays empty.
            Assert.Equal(0f, water2.State.rawWater);
        }

        [Fact]
        public void LegacySave_WithoutFields_RestoresUnbuiltDefaults()
        {
            const string legacy = "{\"systemId\":\"deep_well\",\"schemaVersion\":1}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<DeepWellState>(legacy);
            Assert.NotNull(state);

            var grid = MakeGrid();
            var well = new DeepWellSystem(grid, new WaterTreatmentSystem(), NullLog.Instance);
            well.RestoreState(state!);
            Assert.False(well.IsBuilt); // never auto-granted from research or defaults
            Assert.Equal(0L, well.TotalYieldLiters);
            Assert.False(grid.IsRoomServed(DeepWellSystem.PowerRoomId)); // no load registered
        }
    }
}
