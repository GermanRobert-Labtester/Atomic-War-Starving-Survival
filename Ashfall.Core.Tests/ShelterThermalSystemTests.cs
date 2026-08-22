using Ashfall.Core;
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
    }
}
