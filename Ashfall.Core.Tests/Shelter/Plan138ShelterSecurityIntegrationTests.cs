// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan138ShelterSecurityIntegrationTests
    {
        [Fact]
        public void ConfigureZone_InitializesZoneProperties()
        {
            var system = new ShelterSecuritySystem();
            var zone = system.ConfigureZone("zone_armory", "room_armory", "Main Armory", SecurityLevel.HighSecurity, DoorLockState.Locked);

            Assert.NotNull(zone);
            Assert.Equal("zone_armory", zone.ZoneId);
            Assert.Equal("Main Armory", zone.ZoneName);
            Assert.Equal(SecurityLevel.HighSecurity, zone.Level);
            Assert.Equal(DoorLockState.Locked, zone.LockState);
            Assert.Equal(1, system.ZoneCount);
        }

        [Fact]
        public void GrantClearance_AndGetClearance_TracksSurvivorLevels()
        {
            var system = new ShelterSecuritySystem();
            system.GrantClearance("surv_lead", ClearanceLevel.AllAccess, 1, "council", "Shelter Administrator");
            system.GrantClearance("surv_guard", ClearanceLevel.Restricted, 1, "surv_lead", "Guard rotation");

            Assert.Equal(ClearanceLevel.AllAccess, system.GetClearance("surv_lead"));
            Assert.Equal(ClearanceLevel.Restricted, system.GetClearance("surv_guard"));
            Assert.Equal(ClearanceLevel.None, system.GetClearance("surv_unknown"));
        }

        [Fact]
        public void RequestAccess_AllowsAuthorized_AndDeniesUnauthorizedWithAlarm()
        {
            var system = new ShelterSecuritySystem();
            system.ConfigureZone("zone_vault", "room_vault", "Vault", SecurityLevel.Locked);
            system.GrantClearance("surv_tech", ClearanceLevel.Restricted);

            var okAttempt = system.RequestAccess("surv_tech", "zone_vault", 1);
            Assert.True(okAttempt.IsGranted);
            Assert.False(okAttempt.TriggeredAlarm);

            var failAttempt = system.RequestAccess("surv_rookie", "zone_vault", 1);
            Assert.False(failAttempt.IsGranted);
            Assert.True(failAttempt.TriggeredAlarm);
            Assert.Equal(1, system.ActiveBreachCount);
        }

        [Fact]
        public void ShelterLockdown_SealsAllDoors_AndDeniesAllExceptAllAccess()
        {
            var system = new ShelterSecuritySystem();
            var z1 = system.ConfigureZone("zone_open", "room_quarters", "Quarters", SecurityLevel.Open);
            system.GrantClearance("surv_guard", ClearanceLevel.HighSecurity);
            system.GrantClearance("surv_admin", ClearanceLevel.AllAccess);

            system.SetShelterLockdown(true, 1);
            Assert.True(system.IsInLockdown);
            Assert.Equal(DoorLockState.Sealed, z1.LockState);

            var denied = system.RequestAccess("surv_guard", "zone_open", 1);
            Assert.False(denied.IsGranted);

            var allowed = system.RequestAccess("surv_admin", "zone_open", 1);
            Assert.True(allowed.IsGranted);
        }

        [Fact]
        public void StateSerialization_RoundTrips_ZonesClearancesAndBreaches()
        {
            var original = new ShelterSecuritySystem();
            original.ConfigureZone("zone_a", "room_a", "Room A", SecurityLevel.HighSecurity, DoorLockState.Locked);
            original.GrantClearance("surv_1", ClearanceLevel.HighSecurity, 2, "overseer", "Special operative");
            original.RequestAccess("surv_intruder", "zone_a", 2);

            var captured = original.CaptureState();
            string json = JsonSerializer.Serialize(captured);
            var restoredState = JsonSerializer.Deserialize<ShelterSecurityState>(json);

            Assert.NotNull(restoredState);
            var restored = new ShelterSecuritySystem(restoredState);

            Assert.Equal(original.ZoneCount, restored.ZoneCount);
            Assert.Equal(original.ActiveBreachCount, restored.ActiveBreachCount);
            Assert.Equal(original.GetClearance("surv_1"), restored.GetClearance("surv_1"));
        }

        private static string GetDataPath(string filename)
        {
            var current = new System.IO.DirectoryInfo(AppContext.BaseDirectory);
            while (current != null)
            {
                string candidate = System.IO.Path.Combine(current.FullName, "Assets", "StreamingAssets", "Data", filename);
                if (System.IO.File.Exists(candidate)) return candidate;
                current = current.Parent;
            }
            return System.IO.Path.Combine("Assets", "StreamingAssets", "Data", filename);
        }

        [Fact]
        public void LoadCatalog_FromCanonicalJson_ConfiguresAllShelterZones()
        {
            var system = new ShelterSecuritySystem();
            string jsonPath = GetDataPath("shelter_security_zones.json");

            Assert.True(System.IO.File.Exists(jsonPath), $"Canonical file must exist at {jsonPath}");
            string json = System.IO.File.ReadAllText(jsonPath);

            system.LoadCatalog(json);
            Assert.True(system.ZoneDefinitions.Count >= 8);
            Assert.True(system.ZoneCount >= 8);

            var armory = system.GetZone("zone_armory");
            Assert.NotNull(armory);
            Assert.Equal(SecurityLevel.Locked, armory.Level);
            Assert.Equal(DoorLockState.Locked, armory.LockState);

            var reactor = system.GetZone("zone_reactor_core");
            Assert.NotNull(reactor);
            Assert.Equal(SecurityLevel.Critical, reactor.Level);
            Assert.Equal(DoorLockState.Sealed, reactor.LockState);

            var quarters = system.GetZone("zone_common_bunks");
            Assert.NotNull(quarters);
            Assert.Equal(SecurityLevel.Open, quarters.Level);
            Assert.Equal(DoorLockState.Unlocked, quarters.LockState);
        }

        [Fact]
        public void AlarmRelayBridge_FiresOnBreachAttempt()
        {
            var system = new ShelterSecuritySystem();
            SecurityZone? breachedZone = null;
            SecurityBreach? breachDetails = null;
            system.AlarmRelayBridge = (zone, breach) =>
            {
                breachedZone = zone;
                breachDetails = breach;
            };

            system.ConfigureZone("zone_armory", "room_armory", "Main Armory", SecurityLevel.Locked, DoorLockState.Locked);
            system.GrantClearance("surv_recruit", ClearanceLevel.Basic);

            // Attempt unauthorized access
            var result = system.RequestAccess("surv_recruit", "zone_armory", 1);
            Assert.False(result.IsGranted);
            Assert.True(result.TriggeredAlarm);

            Assert.NotNull(breachedZone);
            Assert.Equal("zone_armory", breachedZone.ZoneId);
            Assert.NotNull(breachDetails);
            Assert.Equal("surv_recruit", breachDetails.IntruderId);
            Assert.False(breachDetails.IsResolved);
        }

        [Fact]
        public void LockdownStateBridge_FiresOnLockdownToggle()
        {
            var system = new ShelterSecuritySystem();
            bool? lastLockdownReported = null;
            system.LockdownStateBridge = (active) => lastLockdownReported = active;

            system.SetShelterLockdown(true, 1);
            Assert.True(lastLockdownReported);

            system.SetShelterLockdown(false, 1);
            Assert.False(lastLockdownReported);
        }

        [Fact]
        public void SecurityDenialLogger_LogsAllUnauthorizedAttempts()
        {
            var system = new ShelterSecuritySystem();
            var denialLogs = new List<(string survivor, string zone, string reason)>();
            system.SecurityDenialLogger = (survivor, zone, reason) =>
            {
                denialLogs.Add((survivor, zone, reason));
            };

            system.ConfigureZone("zone_vault", "room_vault", "Vault", SecurityLevel.HighSecurity);
            system.RequestAccess("surv_guest", "zone_vault", 1);

            Assert.Single(denialLogs);
            Assert.Equal("surv_guest", denialLogs[0].survivor);
            Assert.Equal("zone_vault", denialLogs[0].zone);
            Assert.Contains("Insufficient clearance", denialLogs[0].reason);
        }
    }
}
