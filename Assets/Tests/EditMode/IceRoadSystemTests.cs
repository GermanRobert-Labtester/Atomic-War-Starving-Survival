using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using Ashfall.Core;
using IceRoadSystem = AtomicWar._Game.Core.IceRoadSystem;
using IceRoadSystemState = AtomicWar._Game.Core.IceRoadSystemState;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class IceRoadSystemTests
    {
        private IceRoadSystem Sys(int seed = 808) => new IceRoadSystem(seed);

        [Test]
        public void DarkUntilUnlock()
        {
            var ice = Sys();
            ice.NotifyClerkStarted();
            ice.TickDaily(1, WeatherKind.Blizzard, -20f);
            ice.TickDaily(2, WeatherKind.Blizzard, -20f);
            ice.TickDaily(3, WeatherKind.IceStorm, -25f);
            Assert.IsFalse(ice.IsUnlocked);
            Assert.IsFalse(ice.IsOpen);
            Assert.IsTrue(ice.IsTravelBlocked(IceRoadSystem.LocIceRoadGate));
        }

        [Test]
        public void FirstWindowNeedsClerk()
        {
            var ice = Sys();
            ice.Unlock(90);
            for (int d = 90; d < 120; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -22f);
            Assert.IsFalse(ice.IsOpen, "first window waits on the clerk");
            ice.NotifyClerkStarted();
            ice.TickDaily(120, WeatherKind.Blizzard, -22f);
            Assert.GreaterOrEqual(ice.IceThicknessM, IceRoadSystem.OpenThicknessM);
            Assert.IsTrue(ice.IsOpen);
            Assert.IsFalse(ice.IsTravelBlocked(IceRoadSystem.LocIceRoadGate));
        }

        [Test]
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
            Assert.Greater(openedOn, 0);
            int remaining = ice.WindowDaysRemaining;
            Assert.GreaterOrEqual(remaining, IceRoadSystem.MinWindowDays);
            for (int i = 0; i < remaining; i++)
                ice.TickDaily(openedOn + 1 + i, WeatherKind.Clear, -12f);
            Assert.IsFalse(ice.IsOpen);
            Assert.IsTrue(ice.IsTravelBlocked(IceRoadSystem.LocKilometre19));
        }

        [Test]
        public void DarkBeaconClosesRoad()
        {
            var ice = Sys();
            ice.Unlock(1);
            ice.NotifyClerkStarted();
            for (int d = 1; d <= 80 && !ice.IsOpen; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -22f);
            Assert.IsTrue(ice.IsOpen);
            ice.SetBeaconLit(IceRoadSystem.LocSouthBeacon, false);
            Assert.IsFalse(ice.IsOpen);
            Assert.IsTrue(ice.IsTravelBlocked(IceRoadSystem.LocSouthBeacon));
        }

        [Test]
        public void FalloutStormDoesNotOpen()
        {
            var closed = Sys(909);
            closed.Unlock(1);
            closed.NotifyClerkStarted();
            for (int d = 1; d <= 60; d++)
                closed.TickDaily(d, WeatherKind.FalloutStorm, -30f);
            Assert.IsFalse(closed.IsOpen);
            Assert.IsTrue(closed.IsTravelBlocked(IceRoadSystem.LocIceRoadGate));
        }

        [Test]
        public void SaveRoundtrip()
        {
            var ice = Sys();
            ice.Unlock(12);
            ice.NotifyClerkStarted();
            ice.LogAccident();
            var blob = JsonUtility.ToJson(ice.CaptureState());
            var restored = new IceRoadSystem(1);
            restored.RestoreState(JsonUtility.FromJson<IceRoadSystemState>(blob));
            Assert.IsTrue(restored.IsUnlocked);
            Assert.IsTrue(restored.State.clerkStarted);
            Assert.AreEqual(1, restored.State.accidentCount);
            Assert.AreEqual(ice.IceThicknessM, restored.IceThicknessM, 0.001f);
        }

        [Test]
        public void ShallowsBoatNotACutNode()
        {
            var ice = Sys();
            Assert.IsFalse(ice.IsCutNode(IceRoadSystem.LocShallowsMarket));
            ice.Unlock(1);
            float mul = ice.TravelHoursMultiplier(IceRoadSystem.LocShallowsMarket);
            Assert.AreEqual(IceRoadSystem.ClosedBoatTravelMultiplier, mul, 0.001f);
        }
    }
}
