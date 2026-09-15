// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// B5–B8 Phase 2 (Plan 65): deterministic priority allocation, typed
    /// tick-summary projection, critical-deficit semantics, brownout edge
    /// transitions, and the allocation-aware <see cref="PowerGridSystem.IsRoomServed"/>
    /// query. Legacy aggregate battery/fuel/brownout-hours math is untouched —
    /// the allocation is a projection over the same numbers, and these tests
    /// pin that consistency (energy conservation + legacy parity).
    /// </summary>
    public class PowerGridPhase2AllocationTests
    {
        // 3 critical (180+120+100=400W), 1 standard (160W), 2 low (220+80=300W).
        private static PowerGridSystem MakeGrid(float generationWatts, float batteryReserveWh,
            ISeededRng? rng = null)
        {
            rng ??= new SeededRng(7);
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical, "filtration_off"),
                new PowerGridRoom("room_clinic", "Clinic", 120f,
                    PowerGridRoomPriority.Critical, "clinic_off"),
                new PowerGridRoom("room_water_pump", "Water Pump", 100f,
                    PowerGridRoomPriority.Critical, "water_pressure_drop"),
                new PowerGridRoom("room_greenhouse", "Greenhouse", 160f,
                    PowerGridRoomPriority.Standard, "grow_lights_off"),
                new PowerGridRoom("room_foundry", "Foundry", 220f,
                    PowerGridRoomPriority.Low, "foundry_standstill"),
                new PowerGridRoom("room_lighting_main", "Lighting", 80f,
                    PowerGridRoomPriority.Low, "lighting_dim")
            };
            var state = new PowerGridState
            {
                GenerationWatts = generationWatts,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = batteryReserveWh
            };
            return new PowerGridSystem(state, rooms, rng);
        }

        [Fact]
        public void Allocation_AllServed_WhenGenerationCoversDemand()
        {
            var grid = MakeGrid(generationWatts: 1200f, batteryReserveWh: 2000f);
            var sum = grid.TickDay(1, new SeededRng(3));
            Assert.Equal(grid.TotalDrawWatts, sum.ServedWatts, 1);
            Assert.Equal(0f, sum.UnservedWatts, 1);
            Assert.Empty(sum.ShedRoomIds);
            Assert.Equal(6, sum.ServedRoomIds.Count);
            Assert.False(sum.HasCriticalDeficit);
        }

        [Fact]
        public void Allocation_TotalOutage_ShedsEverything_AndFlagsCriticalDeficit()
        {
            var grid = MakeGrid(generationWatts: 0f, batteryReserveWh: 0f);
            var sum = grid.TickDay(1, new SeededRng(3));
            Assert.Empty(sum.ServedRoomIds);
            Assert.Equal(6, sum.ShedRoomIds.Count);
            Assert.Equal(0f, sum.ServedWatts, 1);
            Assert.Equal(grid.TotalDrawWatts, sum.UnservedWatts, 1);
            Assert.True(sum.HasCriticalDeficit, "critical loads unmet must raise the explicit deficit flag");
            Assert.True(sum.IsBrownout);
        }

        [Fact]
        public void Allocation_NoLowerPriorityServed_WhileHigherPriorityUnserved()
        {
            // 450W covers exactly the three critical rooms (400W); the standard
            // greenhouse (160W) cannot fit, so it and both low rooms must shed.
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 0f);
            var sum = grid.TickDay(1, new SeededRng(3));
            Assert.Equal(new[] { "room_air_filtration", "room_clinic", "room_water_pump" },
                sum.ServedRoomIds);
            Assert.Equal(new[] { "room_greenhouse", "room_foundry", "room_lighting_main" },
                sum.ShedRoomIds);
            Assert.Equal(400f, sum.ServedWatts, 1);
            Assert.Equal(460f, sum.UnservedWatts, 1);
            Assert.False(sum.HasCriticalDeficit, "all critical rooms are served");
        }

        [Fact]
        public void Allocation_UnderCapacity_ShedsStrictSuffix_EvenIfSmallerRoomWouldFit()
        {
            // 100W cannot serve the first critical room (180W). Strict suffix
            // semantics: everything sheds — including the 100W water pump that
            // would numerically fit — because a feeder-shedding order must
            // never skip ahead to scavenge capacity.
            var grid = MakeGrid(generationWatts: 100f, batteryReserveWh: 0f);
            var sum = grid.TickDay(1, new SeededRng(3));
            Assert.Empty(sum.ServedRoomIds);
            Assert.Equal(6, sum.ShedRoomIds.Count);
            Assert.Contains("room_water_pump", sum.ShedRoomIds);
            Assert.True(sum.HasCriticalDeficit);
        }

        [Fact]
        public void Allocation_BatteryDischarge_ExtendsServedSet_UnderLegacyMath()
        {
            // 450W gen + 2000Wh battery → 450 + 2000/24 ≈ 533.3W available.
            // Critical 400W + greenhouse 160W = 560W > 533.3W → greenhouse sheds,
            // matching the aggregate math draining the battery dry this tick.
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 2000f);
            var sum = grid.TickDay(1, new SeededRng(3));
            Assert.Contains("room_water_pump", sum.ServedRoomIds);
            Assert.DoesNotContain("room_greenhouse", sum.ServedRoomIds);
            Assert.Equal(400f, sum.ServedWatts, 1);
            Assert.Equal(0f, sum.BatteryEndWh, 1); // battery exhausted by the deficit
        }

        [Fact]
        public void Allocation_ServedPlusUnserved_EqualsRequestedDraw()
        {
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 500f);
            var sum = grid.TickDay(1, new SeededRng(3));
            Assert.Equal(sum.RequestedDrawWatts, sum.ServedWatts + sum.UnservedWatts, 1);
            Assert.Equal(grid.TotalDrawWatts, sum.RequestedDrawWatts, 1);
        }

        [Fact]
        public void TickDay_ServedPlusUnserved_MatchesDemandBookkeeping()
        {
            // Bookkeeping identity: served + unserved = requested. The legacy
            // coarse daily battery/brownout-hours math is pinned separately
            // (see TickDay_BrownoutHours_MatchLegacyAggregateMath); the strict
            // hourly energy-conservation identity belongs to the Phase 5
            // battery build chain, which introduces real efficiency semantics.
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 2000f);
            var sum = grid.TickDay(1, new SeededRng(3));
            Assert.Equal(grid.TotalDrawWatts, sum.ServedWatts + sum.UnservedWatts, 1);
            Assert.Equal(0f, sum.BatteryEndWh, 1); // legacy math drains the reserve on deficit
        }

        [Fact]
        public void TickDay_BrownoutHours_MatchLegacyAggregateMath()
        {
            // Legacy: unmet = deficit*24 - reserve; hours = unmet / draw.
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 2000f);
            var sum = grid.TickDay(1, new SeededRng(3));
            float deficit = 860f - 450f;
            float expectedHours = (deficit * 24f - 2000f) / 860f;
            Assert.Equal(expectedHours, sum.BrownoutHours, 2);
        }

        [Fact]
        public void Allocation_IsDeterministic_AcrossRuns()
        {
            var a = MakeGrid(450f, 500f, new SeededRng(99));
            var b = MakeGrid(450f, 500f, new SeededRng(99));
            var sa = a.TickDay(4, new SeededRng(5));
            var sb = b.TickDay(4, new SeededRng(5));
            Assert.Equal(sa.ServedRoomIds, sb.ServedRoomIds);
            Assert.Equal(sa.ShedRoomIds, sb.ShedRoomIds);
            Assert.Equal(sa.ServedWatts, sb.ServedWatts, 3);
            Assert.Equal(sa.HasCriticalDeficit, sb.HasCriticalDeficit);
        }

        [Fact]
        public void IsRoomServed_CriticalStaysServed_DuringBrownout_WhenGenerationCoversIt()
        {
            // Brownout (860W demand > 450W gen, battery empty): the legacy global
            // outage semantics would unpower every room; allocation keeps the
            // critical prefix served and sheds the tail.
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 0f);
            Assert.True(grid.IsBrownout);
            Assert.True(grid.IsRoomServed("room_air_filtration"));
            Assert.True(grid.IsRoomServed("room_water_pump"));
            Assert.False(grid.IsRoomServed("room_greenhouse"));
            Assert.False(grid.IsRoomServed("room_lighting_main"));
        }

        [Fact]
        public void IsRoomServed_NonEligibleRooms_AreNeverServed()
        {
            var grid = MakeGrid(generationWatts: 1200f, batteryReserveWh: 2000f);
            Assert.True(grid.ToggleBreaker("room_clinic")); // open breaker
            Assert.False(grid.IsRoomServed("room_clinic"));
            Assert.True(grid.SetPriority("room_foundry", PowerGridRoomPriority.Disabled));
            Assert.False(grid.IsRoomServed("room_foundry"));
        }

        [Fact]
        public void Brownout_Edges_FireExactlyOnce_PerTransition()
        {
            // Day 1: demand 860W, gen 450W, battery empty → brownout begins.
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 0f);
            var day1 = grid.TickDay(1, new SeededRng(11));
            Assert.True(day1.IsBrownout);
            Assert.True(day1.BrownoutBegan);
            Assert.False(day1.BrownoutEnded);

            // Day 2: identical conditions → brownout continues, no new edge.
            var day2 = grid.TickDay(2, new SeededRng(12));
            Assert.True(day2.IsBrownout);
            Assert.False(day2.BrownoutBegan);
            Assert.False(day2.BrownoutEnded);

            // Day 3: shed the low+standard loads so demand fits generation.
            grid.SetPriority("room_greenhouse", PowerGridRoomPriority.Disabled);
            grid.SetPriority("room_foundry", PowerGridRoomPriority.Disabled);
            grid.SetPriority("room_lighting_main", PowerGridRoomPriority.Disabled);
            var day3 = grid.TickDay(3, new SeededRng(13));
            Assert.False(day3.IsBrownout);
            Assert.False(day3.BrownoutBegan);
            Assert.True(day3.BrownoutEnded);
        }

        [Fact]
        public void Restore_MidBrownout_DoesNotReplayBeganEdge()
        {
            var grid = MakeGrid(generationWatts: 450f, batteryReserveWh: 0f);
            grid.TickDay(1, new SeededRng(21)); // brownout begins
            var captured = grid.CaptureState();

            var restored = MakeGrid(450f, 0f, new SeededRng(22));
            restored.RestoreState(captured);
            var sum = restored.TickDay(2, new SeededRng(23));
            Assert.True(sum.IsBrownout);
            Assert.False(sum.BrownoutBegan, "a restored mid-brownout campaign must not replay the begin transition");
            Assert.False(sum.BrownoutEnded);
        }

        [Fact]
        public void EffectivePriority_UsesCatalogDefault_WhenNoPlayerOverride()
        {
            var grid = MakeGrid(800f, 2000f);
            Assert.Equal(PowerGridRoomPriority.Critical, grid.EffectivePriority("room_air_filtration"));
            Assert.Equal(PowerGridRoomPriority.Low, grid.EffectivePriority("room_foundry"));
            // Player override wins over the catalog default.
            Assert.True(grid.SetPriority("room_air_filtration", PowerGridRoomPriority.Low));
            Assert.Equal(PowerGridRoomPriority.Low, grid.EffectivePriority("room_air_filtration"));
        }
    }
}
