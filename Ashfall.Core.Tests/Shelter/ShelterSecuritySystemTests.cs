// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterSecuritySystemTests
    {
        [Fact]
        public void ConfigureZone_InitializesZoneCorrectly()
        {
            var system = new ShelterSecuritySystem();
            var zone = system.ConfigureZone("zone_armory", "room_armory", "Armory", SecurityLevel.Locked);

            Assert.NotNull(zone);
            Assert.Equal("zone_armory", zone.ZoneId);
            Assert.Equal(SecurityLevel.Locked, zone.Level);
            Assert.Equal(DoorLockState.Unlocked, zone.LockState);
            Assert.Equal(1, system.ZoneCount);
        }

        [Fact]
        public void GrantClearance_AssignsClearanceLevel()
        {
            var system = new ShelterSecuritySystem();
            system.GrantClearance("guard_1", ClearanceLevel.Restricted, day: 2, reason: "Security guard");

            Assert.Equal(ClearanceLevel.Restricted, system.GetClearance("guard_1"));
            Assert.Equal(ClearanceLevel.None, system.GetClearance("civilian_1"));
        }

        [Fact]
        public void RequestAccess_AllowsWhenClearanceMatches()
        {
            var system = new ShelterSecuritySystem();
            system.ConfigureZone("zone_workshop", "room_ws", "Workshop", SecurityLevel.Restricted);
            system.GrantClearance("mechanic_1", ClearanceLevel.Basic);

            var res = system.RequestAccess("mechanic_1", "zone_workshop", 1);

            Assert.True(res.IsGranted);
            Assert.False(res.TriggeredAlarm);
        }

        [Fact]
        public void RequestAccess_DeniesAndTriggersBreachOnHighSecurityViolation()
        {
            var system = new ShelterSecuritySystem();
            system.ConfigureZone("zone_vault", "room_vault", "Main Vault", SecurityLevel.HighSecurity);
            system.GrantClearance("dweller_1", ClearanceLevel.Basic);

            SecurityBreach? breachEvent = null;
            system.OnSecurityBreachDetected += b => breachEvent = b;

            var res = system.RequestAccess("dweller_1", "zone_vault", 5);

            Assert.False(res.IsGranted);
            Assert.True(res.TriggeredAlarm);
            Assert.NotNull(breachEvent);
            Assert.Equal("zone_vault", breachEvent.ZoneId);
            Assert.Equal("dweller_1", breachEvent.IntruderId);
            Assert.Equal(1, system.ActiveBreachCount);
        }

        [Fact]
        public void SetShelterLockdown_SealsZonesAndRestrictsAccess()
        {
            var system = new ShelterSecuritySystem();
            var zone = system.ConfigureZone("zone_canteen", "room_canteen", "Mess Hall", SecurityLevel.Open);
            system.GrantClearance("dweller_2", ClearanceLevel.Basic);

            system.SetShelterLockdown(true, currentDay: 7);

            Assert.True(system.IsInLockdown);
            Assert.Equal(DoorLockState.Sealed, zone.LockState);

            var res = system.RequestAccess("dweller_2", "zone_canteen", 7);
            Assert.False(res.IsGranted);

            // Leader with AllAccess can still enter
            system.GrantClearance("leader_1", ClearanceLevel.AllAccess);
            var resLeader = system.RequestAccess("leader_1", "zone_canteen", 7);
            Assert.True(resLeader.IsGranted);
        }

        [Fact]
        public void ResolveBreach_MarksResolvedAndResetsAlarm()
        {
            var system = new ShelterSecuritySystem();
            system.ConfigureZone("zone_med", "room_med", "Medical Bay", SecurityLevel.Locked);
            var res = system.RequestAccess("intruder_1", "zone_med", 2);
            var breach = system.GetActiveBreaches().First();

            bool resolved = system.ResolveBreach(breach.BreachId, "Guards escorted intruder away");

            Assert.True(resolved);
            Assert.Equal(0, system.ActiveBreachCount);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ShelterSecuritySystem();
            system1.ConfigureZone("zone_lab", "room_lab", "Laboratory", SecurityLevel.Restricted);
            system1.GrantClearance("scientist_1", ClearanceLevel.Restricted, day: 3);

            var state = system1.CaptureState();
            Assert.Single(state.Zones);
            Assert.Single(state.Clearances);

            var system2 = new ShelterSecuritySystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ZoneCount);
            Assert.Equal(ClearanceLevel.Restricted, system2.GetClearance("scientist_1"));
            var res = system2.RequestAccess("scientist_1", "zone_lab", 3);
            Assert.True(res.IsGranted);
        }
    }
}
