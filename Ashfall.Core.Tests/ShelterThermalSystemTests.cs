using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterThermalSystemTests
    {
        [Fact] public void AddRoom_CreatesNode()
        {
            var t = Create(out _, out _, out _);
            var r = t.AddRoom("room_a", "Bunker Hall", 80f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(t.State.rooms);
        }

        [Fact] public void AddRoom_Duplicate_Blocks()
        {
            var t = Create(out _, out _, out _);
            t.AddRoom("room_a", "Bunker Hall", 80f);
            var r = t.AddRoom("room_a", "Bunker Hall", 80f);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void SetBoilerActive_HeatsRoom()
        {
            var t = Create(out _, out _, out _);
            t.AddRoom("room_a", "Bunker Hall", 80f);
            t.SetBoilerActive(true, 70f);
            t.TickDay(1);
            Assert.True(t.State.rooms[0].currentTempC > 10f);
        }

        [Fact] public void SetRadiatorValve_UpdatesValve()
        {
            var t = Create(out _, out _, out _);
            t.AddRoom("room_a", "Bunker Hall", 80f, hasRadiator: true);
            var r = t.SetRadiatorValve("room_a", 0.5f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(0.5f, t.State.rooms[0].radiatorValveOpen);
        }

        [Fact] public void Freeze_FrozenRoom()
        {
            var df = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState { indoorTemperatureCelsius = -15f });
            var t = new ShelterThermalSystem(new SeededRng(42), new NeedsSystem(), new StartingLevelSystem(), df);
            t.AddRoom("room_a", "Cold Room", 80f);
            t.State.boilerActive = false;
            for (int i = 0; i < 50; i++) t.TickDay(i + 1);
            // Room should drop toward cold outdoor temp
            Assert.True(t.State.rooms[0].currentTempC < 5f);
        }

        [Fact] public void RepairPipe_RestoresCondition()
        {
            var t = Create(out _, out _, out _);
            t.AddPipe("pipe_1", "room_a", "room_b");
            var pipe = t.State.pipes[0];
            pipe.hasBurst = true;
            pipe.condition = 20f;
            var r = t.RepairPipe("pipe_1", 40f);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(60f, pipe.condition);
        }

        [Fact] public void GetRoomWarmthModifier_ReturnsModifier()
        {
            var t = Create(out _, out _, out _);
            t.AddRoom("room_a", "Warm Room", 80f);
            t.State.rooms[0].currentTempC = 20f;
            Assert.True(t.GetRoomWarmthModifier("room_a") > 0);
        }

        [Fact] public void IsRoomAvailable_FrozenRoom_ReturnsFalse()
        {
            var t = Create(out _, out _, out _);
            t.AddRoom("room_a", "Cold Room", 80f);
            t.State.rooms[0].isFrozen = true;
            Assert.False(t.IsRoomAvailable("room_a"));
        }

        [Fact] public void AddRoom_FloorAtIndoorTemp()
        {
            // Bug-12 regression: a fresh room added to a cold bunker must not
            // inherit a stale or default boilerCurrentTempC from the field
            // default (20°C) — that value has no physical meaning if the boiler
            // was never actually run. Rooms should start at the indoor
            // baseline (the deep-freeze target), then equilibrate via heat
            // exchange with existing rooms and the boiler.
            var df = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState
            {
                indoorTemperatureCelsius = 5f
            });
            var t = new ShelterThermalSystem(new SeededRng(42),
                new NeedsSystem(), new StartingLevelSystem(), df);
            // Sanity: field default boilerCurrentTempC is 20°C even though the
            // shelter has never been warmed by anything.
            Assert.Equal(20f, t.State.boilerCurrentTempC);
            Assert.Equal(5f, df.IndoorTempCelsius);
            t.AddRoom("room_a", "Cold Bunker", 80f);
            float roomTemp = t.State.rooms[0].currentTempC;
            // The room must start close to the indoor baseline (5C), not the
            // 20C boiler field default.
            Assert.True(roomTemp <= df.IndoorTempCelsius + 1f,
                $"room {roomTemp}C starts too warm (indoor baseline {df.IndoorTempCelsius}C)");
        }

        [Fact] public void CaptureRestoreState_PreservesRooms()
        {
            var t = Create(out _, out _, out _);
            t.AddRoom("room_a", "Bunker Hall", 80f);
            t.SetBoilerActive(true);
            t.TickDay(1);
            var state = t.CaptureState();
            Assert.Single(state.rooms);

            var t2 = Create(out _, out _, out _);
            t2.RestoreState(state);
            Assert.Single(t2.State.rooms);
            Assert.True(t2.State.boilerActive);
        }

        private static ShelterThermalSystem Create(out NeedsSystem needs, out StartingLevelSystem sl, out YearOfAshDeepFreezeSystem df)
        {
            needs = new NeedsSystem();
            sl = new StartingLevelSystem();
            df = new YearOfAshDeepFreezeSystem();
            return new ShelterThermalSystem(new SeededRng(42), needs, sl, df);
        }

        // Bug-04 regression: per-room temperature relaxation now follows the
        // analytic solve of dT/dt = (G - k·(T - T_out)) / C (stable at a
        // 86400 s timestep, no explicit-Euler overshoot). Verify the room
        // relaxes toward steady state T_out + G/k and never jumps past the
        // clamp, regardless of boiler power.
        [Fact] public void Bug04_HeatGain_Physics_Matches_Audit_Formula()
        {
            var df = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState
            {
                indoorTemperatureCelsius = 0f
            });
            var t = new ShelterThermalSystem(new SeededRng(42), new NeedsSystem(),
                new StartingLevelSystem(), df);
            t.AddRoom("room_a", "Isothermal Test Chamber", 100f, insulationFactor: 1f, hasRadiator: true);
            t.SetRadiatorValve("room_a", 0.5f);
            // Pin fuel so the boiler power is known: fuel 9.5 × 0.05 kW/fuel.
            t.State.boilerFuelLevel = 10f;
            t.SetBoilerActive(true, 70f);

            // Analytic expectations:
            float volume = 100f;
            float powerW = 9.5f * ShelterThermalSystem.KwPerFuelUnit * 0.5f * 1000f;
            float conductionW = ShelterThermalSystem.NewtonCoolingCoefficient * volume
                                / 1f * 1000f; // 100 W per K
            float steadyC = 0f + powerW / conductionW;
            Assert.True(steadyC > 0, "positive gain should produce a positive steady");

            t.TickDay(1);

            float actualTemp = t.State.rooms[0].currentTempC;
            // One day at τ=C/k = (100·1.225·1005)/100 s = 1231 s ≈ 0.014 d
            // means essentially full relaxation; expect the room to approach
            // steadyC (± 1 °C at the clamp-reviewed path).
            Assert.True(Math.Abs(actualTemp - steadyC) <= 1f,
                $"analytic solve diverged: steady ≈ {steadyC:F2}°C, actual {actualTemp:F2}°C");
            // Anticipation of clamp: steady < boilerTarget(70)+10, so no clamp
            // should trigger.
            Assert.True(actualTemp < ShelterThermalSystem.KwPerFuelUnit * 100f + 10f,
                "thermal clamp should not trigger in this fixture");
        }

        // Bug-04 hookup test: adding a second room must NOT halve the
        // first room's heat gain. (Old /roomCount reduction made per-room
        // heat go DOWN as more rooms attached — the source of the audit's
        // "user-visible kW label is a lie" complaint.)
        [Fact] public void Bug04_Adding_Room_Does_Not_Reduce_PerRoomHeat()
        {
            var df = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState
            {
                indoorTemperatureCelsius = 0f
            });
            var tt = new ShelterThermalSystem(new SeededRng(42), new NeedsSystem(),
                new StartingLevelSystem(), df);
            tt.AddRoom("room_a", "A", 100f, hasRadiator: true);
            tt.SetRadiatorValve("room_a", 0.5f);
            tt.State.boilerFuelLevel = 10f;
            tt.SetBoilerActive(true, 70f);
            tt.TickDay(1);
            float oneRoomDelta = tt.State.rooms[0].currentTempC;

            var tt2 = new ShelterThermalSystem(new SeededRng(42), new NeedsSystem(),
                new StartingLevelSystem(), df);
            tt2.AddRoom("room_a", "A", 100f, hasRadiator: true);
            tt2.AddRoom("room_b", "B", 100f, hasRadiator: true);
            tt2.SetRadiatorValve("room_a", 0.5f);
            tt2.SetRadiatorValve("room_b", 0.5f);
            tt2.State.boilerFuelLevel = 10f;
            tt2.SetBoilerActive(true, 70f);
            tt2.TickDay(1);
            float twoRoomDeltaA = tt2.State.rooms[0].currentTempC;

            Assert.True(Math.Abs(twoRoomDeltaA - oneRoomDelta) <= 1f,
                $"Bug-04 not closed: 1-room ΔT={oneRoomDelta:F2}, 2-room A ΔT={twoRoomDeltaA:F2}");
        }

        // Bug-03 regression: a warm room restores Warmth for every survivor
        // assigned to it (via the optional ShelterAssignmentSystem increment.
        // Warmth is 0..100 where LOW = worse, so a positive room-warmth
        // modifier translating to `Modify(Warmth, +x)` must raise the
        // survivor's Warmth value.
        [Fact] public void Bug03_Warmth_Propagates_ToInRoomSurvivors()
        {
            var df = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState
            {
                indoorTemperatureCelsius = 25f
            });
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "survivor_warm", Warmth = 30f };
            needs.Register(survivor);

            var assignState = new ShelterAssignmentState();
            var assign = new ShelterAssignmentSystem(assignState,
                new[] { new ShelterRoom("warm_room", "Warm Room", capacity: 2) },
                new SeededRng(42));
            var assignResult = assign.Assign("survivor_warm", "warm_room", day: 1);
            Assert.True(assignResult.Succeeded, "assign should succeed: " + assignResult.ReasonCode);

            var t = new ShelterThermalSystem(new SeededRng(42), needs,
                new StartingLevelSystem(), df, null!, assign);
            t.AddRoom("warm_room", "Warm Room", 100f, insulationFactor: 1f, hasRadiator: false);
            // With ambient 25 °C, the room starts at 25 °C and stays warm
            // (no boiler needed), so GetRoomWarmthModifier > 0 and the
            // propagation block runs.
            t.State.rooms[0].currentTempC = 25f;

            t.TickDay(1);

            var after = needs.Get("survivor_warm");
            Assert.NotNull(after);
            Assert.True(after!.Warmth > 30f,
                $"in-room survivor in 25°C room should warm; Warmth={after.Warmth}");
        }
    }
}
