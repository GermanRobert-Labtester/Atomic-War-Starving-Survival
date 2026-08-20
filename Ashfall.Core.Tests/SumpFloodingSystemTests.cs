using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SumpFloodingSystemTests
    {
        [Fact] public void AddNode_CreatesNode()
        {
            var s = Create(out _, out _, out _);
            var r = s.AddNode("sump_a", "Lower Level");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(s.State.nodes);
        }

        [Fact] public void InstallPump_AddsPump()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            var r = s.InstallPump("sump_a");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(s.State.nodes[0].hasSumpPump);
        }

        [Fact] public void AddMitigation_FloatValve()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            var r = s.AddMitigation("sump_a", "float_valve");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(s.State.nodes[0].hasFloatValve);
        }

        [Fact] public void TickDay_NoPower_PumpDegrades()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            s.SetNodePower("sump_a", true); // pump switched on, but grid has no power for room
            for (int i = 0; i < 200; i++) s.TickDay(i + 1);
            Assert.True(s.State.nodes[0].pumpCondition < 100f);
        }

        [Fact] public void DrainNode_ReducesWater()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].waterLevelCm = 100f;
            var r = s.DrainNode("sump_a");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(50f, s.State.nodes[0].waterLevelCm);
        }

        [Fact] public void IsNodeAvailable_FalseWhenFlooded()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].isFlooded = true;
            s.State.nodes[0].equipmentDisabled = true;
            Assert.False(s.IsNodeAvailable("sump_a"));
        }

        [Fact] public void CaptureRestoreState_PreservesNodes()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            var state = s.CaptureState();
            Assert.Single(state.nodes);

            var s2 = Create(out _, out _, out _);
            s2.RestoreState(state);
            Assert.Single(s2.State.nodes);
            Assert.True(s2.State.nodes[0].hasSumpPump);
        }

        private static SumpFloodingSystem Create(out WeatherSystem weather, out PowerGridSystem power, out YearOfAshDeepFreezeSystem df)
        {
            weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var rooms = new System.Collections.Generic.List<PowerGridRoom>
            {
                new PowerGridRoom("sump_a", "Lower Level", 100f)
            };
            power = new PowerGridSystem(state, rooms, new SeededRng(42));
            df = new YearOfAshDeepFreezeSystem();
            return new SumpFloodingSystem(new SeededRng(42), weather, power, df);
        }
    }
}
