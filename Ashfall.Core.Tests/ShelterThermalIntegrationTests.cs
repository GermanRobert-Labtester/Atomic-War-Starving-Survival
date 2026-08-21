using Ashfall.Core;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterThermalIntegrationTests
    {
        private static ShelterThermalSystem CreateSystem()
        {
            var rng = new SeededRng(42);
            var needs = new NeedsSystem();
            var starting = new StartingLevelSystem();
            var deepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
            return new ShelterThermalSystem(rng, needs, starting, deepFreeze);
        }

        [Fact]
        public void SetBoilerActive_UpdatesState()
        {
            var sys = CreateSystem();
            var act = sys.SetBoilerActive(true, 75f);
            Assert.True(act.IsSuccess);
            Assert.True(sys.State.boilerActive);
            Assert.Equal(75f, sys.State.boilerTargetTempC);
        }

        [Fact]
        public void DailyTick_UpdatesBoilerTemperature()
        {
            var sys = CreateSystem();
            sys.AddRoom("room_living", "Living Quarters", 50f, 1.2f, true);
            sys.SetBoilerActive(true, 70f);
            sys.TickDay(1);

            Assert.True(sys.State.totalHeatOutputKw > 0f);
        }

        [Fact]
        public void SaveAndRestore_PreservesThermalState()
        {
            var sys1 = CreateSystem();
            sys1.AddRoom("room_vault", "Main Vault", 80f, 1.5f, true);
            sys1.SetBoilerActive(true, 80f);
            sys1.TickDay(1);

            var state = sys1.CaptureState();
            var sys2 = CreateSystem();
            sys2.RestoreState(state);

            Assert.True(sys2.State.boilerActive);
            Assert.Single(sys2.State.rooms);
            Assert.Equal("room_vault", sys2.State.rooms[0].roomId);
        }
    }
}
