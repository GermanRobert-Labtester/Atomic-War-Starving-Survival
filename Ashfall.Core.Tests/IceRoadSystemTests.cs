using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class IceRoadSystemTests
    {
        private static IceRoadSystem Sys(int seed = 808) => new IceRoadSystem(seed);

        [Fact]
        public void DarkUntilUnlock()
        {
            var ice = Sys();
            ice.NotifyClerkStarted();
            ice.TickDaily(1, WeatherKind.Blizzard, -20f);
            ice.TickDaily(2, WeatherKind.Blizzard, -20f);
            ice.TickDaily(3, WeatherKind.IceStorm, -25f);
            Assert.False(ice.IsUnlocked);
            Assert.False(ice.IsOpen);
            Assert.True(ice.IsTravelBlocked(IceRoadSystem.LocIceRoadGate));
        }

        [Fact]
        public void FirstWindowNeedsClerk()
        {
            var ice = Sys();
            ice.Unlock(90);
            for (int d = 90; d < 120; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -22f);
            Assert.False(ice.IsOpen);
            ice.NotifyClerkStarted();
            ice.TickDaily(120, WeatherKind.Blizzard, -22f);
            Assert.True(ice.IceThicknessM >= IceRoadSystem.OpenThicknessM);
            Assert.True(ice.IsOpen);
            Assert.False(ice.IsTravelBlocked(IceRoadSystem.LocIceRoadGate));
        }

        [Fact]
        public void WindowClosesAfterLength()
        {
            var ice = Sys();
            ice.Unlock(1);
            ice.NotifyClerkStarted();
            int openedOn = -1;
            for (int d = 1; d <= 80; d++)
            {
                ice.TickDaily(d, WeatherKind.IceStorm, -24f);
                if (ice.IsOpen) { openedOn = d; break; }
            }
            Assert.True(openedOn > 0);
            int remaining = ice.WindowDaysRemaining;
            Assert.True(remaining >= IceRoadSystem.MinWindowDays);
            for (int i = 0; i < remaining; i++)
                ice.TickDaily(openedOn + 1 + i, WeatherKind.Clear, -12f);
            Assert.False(ice.IsOpen);
            Assert.True(ice.IsTravelBlocked(IceRoadSystem.LocKilometre19));
        }

        [Fact]
        public void DarkBeaconClosesRoad()
        {
            var ice = Sys();
            ice.Unlock(1);
            ice.NotifyClerkStarted();
            for (int d = 1; d <= 80 && !ice.IsOpen; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -22f);
            Assert.True(ice.IsOpen);
            ice.SetBeaconLit(IceRoadSystem.LocSouthBeacon, false);
            Assert.False(ice.IsOpen);
            Assert.True(ice.IsTravelBlocked(IceRoadSystem.LocSouthBeacon));
        }

        [Fact]
        public void FalloutStormDoesNotOpen()
        {
            var closed = Sys(909);
            closed.Unlock(1);
            closed.NotifyClerkStarted();
            for (int d = 1; d <= 60; d++)
                closed.TickDaily(d, WeatherKind.FalloutStorm, -30f);
            Assert.False(closed.IsOpen);
            Assert.True(closed.IsTravelBlocked(IceRoadSystem.LocIceRoadGate));
        }

        [Fact]
        public void SaveRoundtrip()
        {
            var json = new SystemTextJsonSerializer();
            var ice = Sys();
            ice.Unlock(12);
            ice.NotifyClerkStarted();
            ice.LogAccident();
            string blob = json.Serialize(ice.CaptureState());
            var restored = new IceRoadSystem(1);
            restored.RestoreState(json.Deserialize<IceRoadSystemState>(blob));
            Assert.True(restored.IsUnlocked);
            Assert.True(restored.State.clerkStarted);
            Assert.Equal(1, restored.State.accidentCount);
            Assert.Equal(ice.IceThicknessM, restored.IceThicknessM, 3);
        }

        [Fact]
        public void ShallowsBoatNotACutNode()
        {
            var ice = Sys();
            Assert.False(ice.IsCutNode(IceRoadSystem.LocShallowsMarket));
            ice.Unlock(1);
            float mul = ice.TravelHoursMultiplier(IceRoadSystem.LocShallowsMarket);
            Assert.Equal(IceRoadSystem.ClosedBoatTravelMultiplier, mul, 3);
        }

        [Fact]
        public void SameSeedSameWindowLength()
        {
            var a = Sys(808);
            var b = Sys(808);
            a.Unlock(1);
            b.Unlock(1);
            a.NotifyClerkStarted();
            b.NotifyClerkStarted();
            int openA = -1, openB = -1;
            for (int d = 1; d <= 80; d++)
            {
                a.TickDaily(d, WeatherKind.IceStorm, -24f);
                b.TickDaily(d, WeatherKind.IceStorm, -24f);
                if (a.IsOpen && openA < 0) openA = d;
                if (b.IsOpen && openB < 0) openB = d;
            }
            Assert.Equal(openA, openB);
            Assert.Equal(a.WindowDaysRemaining, b.WindowDaysRemaining);
            Assert.Equal(a.IceThicknessM, b.IceThicknessM, 5);
        }
    }
}
