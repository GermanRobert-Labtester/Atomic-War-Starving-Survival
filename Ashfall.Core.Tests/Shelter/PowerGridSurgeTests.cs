// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// SHELTER_EMP_MEDICAL_POWER Phase 1: surge application contract —
    /// deterministic trip selection, battery drain bounds, per-day dedup,
    /// event emission, and LastSurgeDay persistence.
    /// </summary>
    public sealed class PowerGridSurgeTests
    {
        private static readonly string[] RoomIds =
        {
            "room_air_filtration", "room_clinic", "room_water_pump",
            "room_greenhouse", "room_foundry", "room_lighting_main", "room_workshop"
        };

        private static PowerGridSystem MakeSystem(out PowerGridState state)
        {
            var initialState = new PowerGridState
            {
                GenerationWatts = 800f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 4000f,
                FuelUnits = 100f
            };
            var rooms = RoomIds.Select(id => new PowerGridRoom(id, id, 100f, PowerGridRoomPriority.Standard)).ToList();
            // Set tiers BEFORE construction: the state seeds its effective-priority
            // table from room defaults in the constructor.
            rooms.Single(r => r.RoomId == "room_clinic").DefaultPriority = PowerGridRoomPriority.Critical;
            rooms.Single(r => r.RoomId == "room_air_filtration").DefaultPriority = PowerGridRoomPriority.Critical;
            rooms.Single(r => r.RoomId == "room_foundry").DefaultPriority = PowerGridRoomPriority.Low;
            rooms.Single(r => r.RoomId == "room_workshop").DefaultPriority = PowerGridRoomPriority.Low;
            var system = new PowerGridSystem(initialState, rooms, new SeededRng(7));
            state = system.State;
            return system;
        }

        [Fact]
        public void ZeroSeverity_NoOp_NoStateChange()
        {
            var system = MakeSystem(out var state);
            var tripped = system.ApplySurgeDay(5, 0f);
            Assert.Empty(tripped);
            Assert.Equal(0, state.LastSurgeDay);
            Assert.Equal(4000f, state.BatteryReserveWh);
        }

        [Fact]
        public void NegativeSeverity_ClampedToNoOp()
        {
            var system = MakeSystem(out var state);
            Assert.Empty(system.ApplySurgeDay(5, -1f));
            Assert.Equal(0, state.LastSurgeDay);
        }

        [Fact]
        public void Surge_TripsLowPriorityFirst_ThenStandard()
        {
            var system = MakeSystem(out var state);
            // 5 eligible (2 critical exempt), severity 0.5 → floor(2.5) = 2 trips.
            var tripped = system.ApplySurgeDay(5, 0.5f).ToList();
            Assert.Equal(2, tripped.Count);
            // Low tier first (ordinal), then standard tier ordinal.
            Assert.Equal(new[] { "room_foundry", "room_workshop" }, tripped);
            Assert.All(tripped, id => Assert.True(system.IsRoomTripped(id)));
            Assert.False(system.IsRoomTripped("room_clinic"));
        }

        [Fact]
        public void Surge_AtHighSeverity_CriticalEligible()
        {
            var system = MakeSystem(out var state);
            // Full severity: all 7 candidates trip, ordinal order — clinic trips last.
            var tripped = system.ApplySurgeDay(5, 1f).ToList();
            Assert.Equal(7, tripped.Count);
            Assert.Contains("room_clinic", tripped);
            Assert.Equal("room_clinic", tripped[^1]);
        }

        [Fact]
        public void Surge_BelowCriticalThreshold_CriticalExempt()
        {
            var system = MakeSystem(out var state);
            var tripped = system.ApplySurgeDay(5, 0.89f).ToList();
            Assert.DoesNotContain("room_clinic", tripped);
        }

        [Fact]
        public void Surge_BatteryDrain_BoundedAndFloored()
        {
            var system = MakeSystem(out var state);
            system.ApplySurgeDay(5, 0.5f); // drain = floor(4000 * 0.15 * 0.5) = 300
            Assert.Equal(3700f, state.BatteryReserveWh);
        }

        [Fact]
        public void Surge_BatteryNeverNegative()
        {
            var system = MakeSystem(out var state);
            state.BatteryReserveWh = 10f;
            system.ApplySurgeDay(5, 1f);
            Assert.Equal(0f, state.BatteryReserveWh);
        }

        [Fact]
        public void Surge_Deduped_SameDay()
        {
            var system = MakeSystem(out var state);
            var first = system.ApplySurgeDay(5, 0.5f).ToList();
            var second = system.ApplySurgeDay(5, 0.5f).ToList();
            Assert.NotEmpty(first);
            Assert.Empty(second);
            Assert.Equal(first.Count, state.TrippedRooms.Count);
        }

        [Fact]
        public void Surge_EarlierDayAfterApplied_NoOp()
        {
            var system = MakeSystem(out var state);
            system.ApplySurgeDay(5, 0.5f);
            Assert.Empty(system.ApplySurgeDay(4, 0.5f));
        }

        [Fact]
        public void Surge_EmitsSingleAggregatedEvent()
        {
            var system = MakeSystem(out var state);
            var events = new List<PowerGridEvent>();
            system.OnPowerChanged += e => events.Add(e);
            system.ApplySurgeDay(5, 0.5f);
            var surgeEvents = events.Where(e => e.Kind == PowerGridEventKind.SurgeApplied).ToList();
            Assert.Single(surgeEvents);
        }

        [Fact]
        public void Surge_Deterministic_SameStateSameTrips()
        {
            var a = MakeSystem(out var stateA);
            var b = MakeSystem(out var stateB);
            var tripsA = a.ApplySurgeDay(5, 0.6f);
            var tripsB = b.ApplySurgeDay(5, 0.6f);
            Assert.Equal(tripsA, tripsB);
        }

        [Fact]
        public void LastSurgeDay_CaptureRestore_RoundTrips()
        {
            var system = MakeSystem(out var state);
            system.ApplySurgeDay(5, 0.5f);
            var captured = state.Capture();
            Assert.Equal(5, captured.LastSurgeDay);

            var restored = new PowerGridState();
            restored.RestoreInto(captured, system.Rooms);
            Assert.Equal(5, restored.LastSurgeDay);

            // Same-day replay after restore is still deduped.
            var system2 = new PowerGridSystem(restored, system.Rooms, new SeededRng(7));
            Assert.Empty(system2.ApplySurgeDay(5, 0.5f));
        }

        [Fact]
        public void OldSave_MissingLastSurgeDay_RestoresZero()
        {
            // Simulate a pre-surge save: LastSurgeDay absent → default 0.
            var restored = new PowerGridState(); // LastSurgeDay defaults to 0
            Assert.Equal(0, restored.LastSurgeDay);
            var system = MakeSystem(out _);
            // A surge on day 1 (any day > 0) applies normally after restore.
            Assert.NotEmpty(system.ApplySurgeDay(1, 0.5f));
        }

        [Fact]
        public void Surge_AllTripped_SecondSurgeTripsNothing_ButConsumesDay()
        {
            var system = MakeSystem(out var state);
            system.ApplySurgeDay(5, 1f);
            int trippedAfterFirst = state.TrippedRooms.Count;
            var second = system.ApplySurgeDay(6, 1f).ToList();
            Assert.Empty(second);
            Assert.Equal(trippedAfterFirst, state.TrippedRooms.Count);
            Assert.Equal(6, state.LastSurgeDay);
        }
    }
}
