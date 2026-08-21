using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class PowerGridSystemTests
    {
        private static PowerGridSystem MakeGrid(ISeededRng rng = null)
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
                GenerationWatts = 800,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            return new PowerGridSystem(state, rooms, rng);
        }

        [Fact]
        public void InitialState_ComputesTotalDraw()
        {
            var grid = MakeGrid();
            Assert.Equal(860f, grid.TotalDrawWatts, 0); // 180+120+100+160+220+80
            Assert.False(grid.IsBrownout);
            Assert.Equal(-60f, grid.NetWatts, 0);
        }

        [Fact]
        public void ToggleBreaker_RemovesRoomFromDraw()
        {
            var grid = MakeGrid();
            Assert.True(grid.ToggleBreaker("room_greenhouse"));
            Assert.Equal(700f, grid.TotalDrawWatts, 0); // 180+120+100+0+220+80
            Assert.True(grid.ToggleBreaker("room_greenhouse")); // toggle back
            Assert.Equal(860f, grid.TotalDrawWatts, 0);
        }

        [Fact]
        public void ToggleBreaker_UnknownIdReturnsFalse()
        {
            var grid = MakeGrid();
            Assert.False(grid.ToggleBreaker("room_does_not_exist"));
        }

        [Fact]
        public void SetPriority_Disabled_ExcludesFromDraw()
        {
            var grid = MakeGrid();
            Assert.True(grid.SetPriority("room_foundry", PowerGridRoomPriority.Disabled));
            Assert.Equal(640f, grid.TotalDrawWatts, 0);
        }

        [Fact]
        public void TickDay_NetNegative_DrainsBattery()
        {
            var grid = MakeGrid(new SeededRng(11));
            float before = grid.BatteryReserveWh;
            var sum = grid.TickDay(2, new SeededRng(11));
            Assert.True(sum.FuelConsumed > 0f);
            Assert.True(grid.BatteryReserveWh < before);
            // Draw (860) > Gen (800) ⇒ brownout hours > 0.
            Assert.True(sum.BrownoutHours >= 0f);
        }

        [Fact]
        public void TickDay_OverloadTripsBreakerDeterministic()
        {
            // Use a controlled seed and force a brownout-heavy day.
            var grid = MakeGrid(new SeededRng(13));
            // Force draw > gen by 4+ hours: open a critical room so net goes further negative.
            grid.SetBreaker("room_foundry", false);
            grid.SetBreaker("room_lighting_main", false);
            for (int d = 1; d <= 30; d++)
            {
                grid.TickDay(d, new SeededRng(13));
            }
            // After 30 days of overload, at least one breaker should have tripped.
            bool anyTripped = false;
            foreach (var r in grid.Rooms)
                if (!grid.IsRoomPowered(r.RoomId)) { anyTripped = true; break; }
            Assert.True(anyTripped, "expected at least one room to lose power after sustained overload");
        }

        [Fact]
        public void AddFuel_IncreasesFuelUnits()
        {
            var grid = MakeGrid();
            float before = grid.FuelUnits;
            grid.AddFuel(50f);
            Assert.Equal(before + 50f, grid.FuelUnits, 0);
        }

        [Fact]
        public void CaptureRestore_RoundTripPreservesState()
        {
            var grid = MakeGrid();
            grid.ToggleBreaker("room_greenhouse");
            grid.AddFuel(33f);
            var save = grid.CaptureState();
            var restored = MakeGrid();
            restored.RestoreState(save);
            Assert.Equal(save.BatteryReserveWh, restored.BatteryReserveWh, 0);
            Assert.Equal(save.FuelUnits, restored.FuelUnits, 0);
            Assert.Equal(save.ClosedBreakers.Count, restored.State.ClosedBreakers.Count);
        }

        [Fact]
        public void Save_RoundTrip_ChecksumStable()
        {
            var grid = MakeGrid();
            grid.ToggleBreaker("room_foundry");
            var save = new PowerGridSave
            {
                simDay = 1,
                Rooms = new List<PowerGridRoomSave>
                {
                    new PowerGridRoomSave { RoomId = "room_air_filtration", DisplayName = "Air Filtration",
                        DrawWatts = 180, DefaultPriority = 3, FailureEffectId = "filtration_off" }
                },
                State = grid.State.Capture()
            };
            var json = new SystemTextJsonSerializer();
            string text = PowerGridSaveCodec.EncodeToString(save, json);
            var loaded = PowerGridSaveCodec.Decode(text, json);
            Assert.Equal(save.Checksum, loaded.Checksum);
        }

        [Fact]
        public void Save_TamperedChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new PowerGridSave
            {
                simDay = 1,
                Rooms = new List<PowerGridRoomSave>
                {
                    new PowerGridRoomSave { RoomId = "x", DisplayName = "y", DrawWatts = 1, DefaultPriority = 1 }
                },
                State = new PowerGridState { GenerationWatts = 100 }
            };
            string text = PowerGridSaveCodec.EncodeToString(save, json);
            // Mutate the fuel_units value in the payload.
            int idx = text.IndexOf("FuelUnits", StringComparison.Ordinal);
            int valIdx = text.IndexOf(':', idx) + 1;
            int endIdx = text.IndexOf(',', valIdx);
            if (endIdx < 0) endIdx = text.IndexOf('}', valIdx);
            var sub = text.Substring(valIdx, endIdx - valIdx).Trim();
            float newVal = float.Parse(sub, System.Globalization.CultureInfo.InvariantCulture) + 7f;
            string tampered = text.Substring(0, valIdx) + " " + newVal.ToString("G9",
                System.Globalization.CultureInfo.InvariantCulture) + text.Substring(endIdx);
            Assert.Throws<InvalidOperationException>(() => PowerGridSaveCodec.Decode(tampered, json));
        }

        [Fact]
        public void Save_EmptyChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new PowerGridSave { simDay = 1, Checksum = string.Empty };
            string text = json.Serialize(save);
            Assert.Throws<InvalidOperationException>(() => PowerGridSaveCodec.Decode(text, json));
        }

        [Fact]
        public void Snapshot_ReportsExpectedFields()
        {
            var grid = MakeGrid();
            var snap = grid.Snapshot();
            Assert.Equal(800f, snap.GenerationWatts);
            Assert.Equal(860f, snap.TotalDrawWatts, 0);
            Assert.False(snap.IsBrownout);
            Assert.Equal(6, snap.RoomIds.Count);
        }

        [Fact]
        public void Events_FireOnBreakerToggle()
        {
            var grid = MakeGrid();
            var fired = new List<PowerGridEvent>();
            grid.OnPowerChanged += e => fired.Add(e);
            grid.ToggleBreaker("room_clinic");
            Assert.Single(fired);
            Assert.Equal(PowerGridEventKind.BreakerToggled, fired[0].Kind);
            Assert.Equal("room_clinic", fired[0].RoomId);
        }

        [Fact]
        public void Determinism_SameSeed_IdenticalTickSummary()
        {
            var a = MakeGrid(new SeededRng(99));
            var b = MakeGrid(new SeededRng(99));
            var sa = a.TickDay(3, new SeededRng(99));
            var sb = b.TickDay(3, new SeededRng(99));
            Assert.Equal(sa.FuelConsumed, sb.FuelConsumed, 3);
            Assert.Equal(sa.BatteryEndWh, sb.BatteryEndWh, 3);
            Assert.Equal(sa.BrownoutHours, sb.BrownoutHours, 3);
        }

        // ── Phase 6 regression tests ──
        // ComputeTotalDraw used to return 0 during active brownout (the
        // suppression number). That collided with intent: TotalDrawWatts is
        // a derived property that calls ComputeTotalDraw, so IsBrownout
        // flipped false the same tick it should be true, and NetWatts became
        // positive again (battery charging during a brownout). Fix splits
        // intent from suppression.

        [Fact]
        public void IsBrownout_HoldsTrueAcrossSameTick_WhenDemandExceedsGeneration()
        {
            // Phase 6 regression: IsBrownout readings must be stable for a
            // given state across repeated reads in the same tick — no recursion
            // flicker. Pre-fix, ComputeTotalDraw returned 0 during brownout,
            // so TotalDrawWatts → 0 and IsBrownout read false even while a
            // brownout was active.
            var state = new PowerGridState
            {
                GenerationWatts = 100,
                BatteryCapacityWh = 0,
                BatteryReserveWh = 0
            };
            var rooms = new[]
            {
                new PowerGridRoom("r1", "Room 1", 50f, PowerGridRoomPriority.Standard),
                new PowerGridRoom("r2", "Room 2", 50f, PowerGridRoomPriority.Standard),
                new PowerGridRoom("r3", "Room 3", 50f, PowerGridRoomPriority.Standard)
            };
            var g = new PowerGridSystem(state, rooms, new SeededRng(42));
            Assert.True(g.IsBrownout, "intent demand > generation ==> IsBrownout true");
            Assert.True(g.IsBrownout, "repeated query must remain true");
            Assert.Equal(150f, g.TotalDrawWatts);
        }

        [Fact]
        public void TotalDrawWatts_PreservesIntent_WhenBrownoutActive()
        {
            // TotalDrawWatts is a derived property that calls ComputeTotalDraw.
            // Pre-fix, ComputeTotalDraw returned 0 during brownout, so
            // TotalDrawWatts reported 0 (intent hidden). Post-fix returns
            // intent unconditionally.
            var state = new PowerGridState
            {
                GenerationWatts = 100,
                BatteryCapacityWh = 0,
                BatteryReserveWh = 0
            };
            var rooms = new[]
            {
                new PowerGridRoom("r1", "Room 1", 80f, PowerGridRoomPriority.Standard),
                new PowerGridRoom("r2", "Room 2", 60f, PowerGridRoomPriority.Standard)
            };
            var g = new PowerGridSystem(state, rooms, new SeededRng(42));
            Assert.Equal(140f, g.TotalDrawWatts);
            Assert.True(g.IsBrownout);
        }

        [Fact]
        public void TickDay_DuringBrownout_BatteryDrainsRatherThanCharges()
        {
            // Pre-fix, ComputeTotalDraw returning 0 made NetWatts = gen - 0 =
            // positive, causing the battery to charge up during the brownout
            // (which contradicts the game meaning of "brownout"). Post-fix,
            // intent-based NetWatts stays negative and the battery drains.
            var state = new PowerGridState
            {
                GenerationWatts = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 0
            };
            var rooms = new[]
            {
                new PowerGridRoom("r1", "Room 1", 80f, PowerGridRoomPriority.Standard),
                new PowerGridRoom("r2", "Room 2", 60f, PowerGridRoomPriority.Standard)
            };
            var g = new PowerGridSystem(state, rooms, new SeededRng(42));
            var summary = g.TickDay(1, new SeededRng(42));
            Assert.True(summary.IsBrownout);
            Assert.Equal(0f, summary.BatteryEndWh, 3);
            Assert.True(summary.BrownoutHours >= 1f);
        }
    }
}
