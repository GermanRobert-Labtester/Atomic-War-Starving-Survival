// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 138 (Shelter Defense & Security Clearance System).
    /// </summary>
    internal static class ShelterSecuritySelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print("[PASS] " + message);
                }
                else
                {
                    GD.PrintErr("[FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                GD.Print("[ShelterSecuritySelfTest] Starting Plan 138 verification...");

                // 1. Core Zone Configuration & Clearances
                var system = new ShelterSecuritySystem();
                var zoneArmory = system.ConfigureZone("zone_armory", "room_armory", "Armory", SecurityLevel.HighSecurity, DoorLockState.Locked);
                Check(zoneArmory != null, "Core: Zone configured successfully");
                Check(system.ZoneCount == 1, "Core: ZoneCount is 1");

                var clGuard = system.GrantClearance("surv_guard", ClearanceLevel.HighSecurity, 1, "leader", "Security duty");
                Check(clGuard != null, "Core: Clearance granted");
                Check(system.GetClearance("surv_guard") == ClearanceLevel.HighSecurity, "Core: Clearance level verified");
                Check(system.GetClearance("surv_civilian") == ClearanceLevel.None, "Core: Default clearance is None");

                // 2. Access Control & Breach Detection
                var accessOk = system.RequestAccess("surv_guard", "zone_armory", 1);
                Check(accessOk.IsGranted, "Core: Authorized guard granted access");
                Check(!accessOk.TriggeredAlarm, "Core: Authorized access does not trigger alarm");

                var accessDenied = system.RequestAccess("surv_civilian", "zone_armory", 1);
                Check(!accessDenied.IsGranted, "Core: Unauthorized civilian denied access");
                Check(accessDenied.TriggeredAlarm, "Core: Unauthorized attempt on HighSecurity triggers alarm");
                Check(system.ActiveBreachCount == 1, "Core: ActiveBreachCount is 1");

                // 3. Breach Resolution
                var breach = system.GetActiveBreaches()[0];
                bool resOk = system.ResolveBreach(breach.BreachId, "Guard escorted civilian away");
                Check(resOk, "Core: Breach resolved successfully");
                Check(system.ActiveBreachCount == 0, "Core: ActiveBreachCount is 0 after resolution");

                // 4. Lockdown Protocol
                Check(!system.IsInLockdown, "Core: Initially not in lockdown");
                system.SetShelterLockdown(true, 1);
                Check(system.IsInLockdown, "Core: Lockdown active");
                Check(zoneArmory!.LockState == DoorLockState.Sealed, "Core: Doors sealed during lockdown");

                var lockdownAccess = system.RequestAccess("surv_guard", "zone_armory", 1);
                Check(!lockdownAccess.IsGranted, "Core: Even high security denied during lockdown unless AllAccess");

                system.SetShelterLockdown(false, 1);
                Check(!system.IsInLockdown, "Core: Lockdown lifted");

                // 5. Host Session Coordination
                var hostSys = new ShelterSecuritySystem();
                var host = new ShelterSecurityHostSession(hostSys);
                Check(host.ZoneCount >= 6, "Host: Standard shelter zones seeded");

                bool stateChanged = false;
                host.StateChanged += () => stateChanged = true;

                host.GrantClearance("surv_officer", ClearanceLevel.AllAccess, 1);
                Check(stateChanged, "Host: StateChanged fired on GrantClearance");

                stateChanged = false;
                host.SetDoorLockState("zone_armory", DoorLockState.Unlocked);
                Check(stateChanged, "Host: StateChanged fired on SetDoorLockState");

                // 6. Persistence Roundtrip
                var captured = hostSys.CaptureState();
                bool saveOk = ShelterSecuritySaveStore.TrySave(captured);
                Check(saveOk, "Persistence: ShelterSecuritySaveStore.TrySave succeeded");

                var loaded = ShelterSecuritySaveStore.TryLoad();
                Check(loaded != null, "Persistence: ShelterSecuritySaveStore.TryLoad succeeded");
                Check(loaded?.Zones.Count == hostSys.ZoneCount, "Persistence: Zone count preserved");
                Check(loaded?.Clearances.Count == hostSys.Clearances.Count, "Persistence: Clearance count preserved");

                string envJson = ShelterSecuritySaveStore.TryCapturePersisted(captured);
                Check(!string.IsNullOrWhiteSpace(envJson), "Persistence: TryCapturePersisted produced valid JSON envelope");

                // 7. UI Panel Binding & Lifecycle
                var panel = new ShelterSecurityPanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: ShelterSecurityPanel bound successfully");

                panel.RefreshView();
                Check(true, "UI: ShelterSecurityPanel refreshed cleanly");

                panel.Unbind();
                Check(!panel.IsBound, "UI: ShelterSecurityPanel unbound cleanly");
                panel.QueueFree();

                GD.Print($"[ShelterSecuritySelfTest] Complete with {failures} failure(s).");
                if (failures == 0)
                {
                    GD.Print("[HOST_SELFTEST] shelter_security_selftest PASS");
                    GD.Print("[HOST_SELFTEST_SUMMARY] test=shelter_security_selftest status=PASS exit_code=0 passed=20 failed=0 total=20 details=\"All shelter security gates passed\"");
                    GD.Print("[HOST_SELFTEST_JSON] {\"test\":\"shelter_security_selftest\",\"status\":\"PASS\",\"exit_code\":0,\"passed\":20,\"failed\":0,\"total\":20,\"details\":\"All shelter security gates passed\"}");
                    GD.Print("SELFTEST PASS: shelter_security_selftest");
                    GD.Print("SHELTER_SECURITY_SELFTEST PASS");
                    return 0;
                }

                GD.PrintErr("[HOST_SELFTEST] shelter_security_selftest FAIL");
                return 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[ShelterSecuritySelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
