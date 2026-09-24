// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 186 (Shelter Maintenance & Degradation System).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliShelterMaintenance
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Shelter Maintenance & Degradation Self-Test (Plan 186) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var loadResult = ShelterComponentCatalogLoader.Load(dataDir);
                if (loadResult.Success && loadResult.Catalog != null &&
                    loadResult.Catalog.components.Count >= 10)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({loadResult.Catalog.components.Count} components).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Catalog load failed: {string.Join("; ", loadResult.Errors)}");
                }

                // Check 2: Host session instantiation & initial state
                var host = ShelterMaintenanceHostSession.Create(dataDir);
                if (host.TrackedComponentCount >= 10 && Math.Abs(host.AverageIntegrity - 100.0f) < 0.01f)
                {
                    GD.Print($"[PASS] Check 2: Host session initialized with {host.TrackedComponentCount} components, average integrity 100%.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Host session initialization mismatch: count={host.TrackedComponentCount}, avgIntegrity={host.AverageIntegrity}");
                }

                // Check 3: Canonical component types verification (Air, Water, Power, Structural)
                var defs = host.GetAllDefinitions();
                bool hasAir = defs.Any(d => string.Equals(d.component_type, "Air", StringComparison.OrdinalIgnoreCase));
                bool hasWater = defs.Any(d => string.Equals(d.component_type, "Water", StringComparison.OrdinalIgnoreCase));
                bool hasPower = defs.Any(d => string.Equals(d.component_type, "Power", StringComparison.OrdinalIgnoreCase));
                bool hasStructural = defs.Any(d => string.Equals(d.component_type, "Structural", StringComparison.OrdinalIgnoreCase));

                if (hasAir && hasWater && hasPower && hasStructural)
                {
                    GD.Print("[PASS] Check 3: All 4 canonical component types (Air, Water, Power, Structural) verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Missing component types: Air={hasAir}, Water={hasWater}, Power={hasPower}, Structural={hasStructural}");
                }

                // Check 4: Specific component definition & initial state query
                var airComp = host.GetComponent("air_filtration_primary");
                var airDef = host.GetDefinition("air_filtration_primary");
                if (airComp != null && airDef != null && airComp.IsOperational &&
                    !airComp.HasWarning && Math.Abs(airComp.Condition - 100.0f) < 0.01f)
                {
                    GD.Print($"[PASS] Check 4: Component 'air_filtration_primary' verified: Condition {airComp.Condition}%, Operational: {airComp.IsOperational}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Query failed for 'air_filtration_primary'.");
                }

                // Check 5: Daily degradation tick
                float initialIntegrity = host.AverageIntegrity;
                host.TickDay(currentDay: 2, weatherStressMult: 1.0f, radiationStressMult: 1.0f);
                float tickedIntegrity = host.AverageIntegrity;
                var tickedAir = host.GetComponent("air_filtration_primary");

                if (tickedIntegrity < initialIntegrity && tickedAir != null && tickedAir.Condition < 100.0f)
                {
                    GD.Print($"[PASS] Check 5: Daily wear tick applied: Integrity {initialIntegrity:F1}% -> {tickedIntegrity:F1}%, Air condition {tickedAir.Condition:F1}%.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Daily wear tick failed to degrade components: init={initialIntegrity}, ticked={tickedIntegrity}");
                }

                // Check 6: Environmental stress effect on degradation
                float preStressCond = tickedAir!.Condition;
                // Run tick under high weather and radiation stress (3.0x each)
                host.TickDay(currentDay: 3, weatherStressMult: 3.0f, radiationStressMult: 3.0f);
                var stressedAir = host.GetComponent("air_filtration_primary");
                float stressedLoss = preStressCond - stressedAir!.Condition;
                float baseLoss = 100.0f - preStressCond;

                if (stressedLoss > baseLoss * 1.5f)
                {
                    GD.Print($"[PASS] Check 6: Environmental stress verified: Stressed degradation ({stressedLoss:F2}) > Base degradation ({baseLoss:F2}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Environmental stress effect insufficient: stressed={stressedLoss}, base={baseLoss}");
                }

                // Check 7: Warning threshold trigger
                // Advance days until air component condition drops below warning threshold (<= 40)
                for (int d = 4; d <= 45; d++)
                {
                    host.TickDay(currentDay: d, weatherStressMult: 2.0f, radiationStressMult: 2.0f);
                    if (host.GetComponent("air_filtration_primary")?.HasWarning == true)
                        break;
                }

                var warningAir = host.GetComponent("air_filtration_primary");
                var warnings = host.GetWarningComponents();
                if (warningAir != null && warningAir.HasWarning && warnings.Any(w => w.ComponentId == "air_filtration_primary"))
                {
                    GD.Print($"[PASS] Check 7: Warning threshold triggered correctly at condition {warningAir.Condition:F1}%.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Warning threshold not triggered. Cond={warningAir?.Condition}, Warning={warningAir?.HasWarning}");
                }

                // Check 8: Critical failure threshold trigger
                // Advance more days until component drops <= failure threshold (<= 15)
                for (int d = 46; d <= 90; d++)
                {
                    host.TickDay(currentDay: d, weatherStressMult: 2.5f, radiationStressMult: 2.5f);
                    if (host.GetComponent("air_filtration_primary")?.IsOperational == false)
                        break;
                }

                var failedAir = host.GetComponent("air_filtration_primary");
                var failedList = host.GetFailedComponents();
                if (failedAir != null && !failedAir.IsOperational && failedList.Any(f => f.ComponentId == "air_filtration_primary"))
                {
                    GD.Print($"[PASS] Check 8: Critical failure triggered correctly: Component is non-operational at condition {failedAir.Condition:F1}%.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Critical failure not triggered. Cond={failedAir?.Condition}, Operational={failedAir?.IsOperational}");
                }

                // Check 9: Preventive maintenance (Clean)
                float beforeClean = failedAir!.Condition;
                bool cleaned = host.PerformMaintenance("air_filtration_primary", "Clean", skillLevel: 50f, day: 91);
                var cleanedAir = host.GetComponent("air_filtration_primary");
                if (cleaned && cleanedAir != null && cleanedAir.Condition > beforeClean)
                {
                    GD.Print($"[PASS] Check 9: Preventive cleaning restored condition: {beforeClean:F1}% -> {cleanedAir.Condition:F1}%.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Cleaning maintenance failed: ok={cleaned}");
                }

                // Check 10: Standard Repair restores operational status
                bool repaired = host.PerformMaintenance("air_filtration_primary", "Repair", skillLevel: 75f, day: 92);
                var repairedAir = host.GetComponent("air_filtration_primary");
                if (repaired && repairedAir != null && repairedAir.IsOperational && repairedAir.Condition > 40.0f && !repairedAir.HasWarning)
                {
                    GD.Print($"[PASS] Check 10: Repair completed: Condition restored to {repairedAir.Condition:F1}%, Operational={repairedAir.IsOperational}, Warning cleared.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Repair failed to restore operational state: cond={repairedAir?.Condition}, op={repairedAir?.IsOperational}");
                }

                // Check 11: Complete Overhaul restores to 100% max condition
                bool overhauled = host.PerformMaintenance("air_filtration_primary", "Overhaul", skillLevel: 100f, day: 93);
                var overhauledAir = host.GetComponent("air_filtration_primary");
                if (overhauled && overhauledAir != null && Math.Abs(overhauledAir.Condition - 100.0f) < 0.01f)
                {
                    GD.Print($"[PASS] Check 11: Overhaul restored component to 100.0% max condition.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Overhaul failed to restore max condition: cond={overhauledAir?.Condition}");
                }

                // Check 12: Save/restore roundtrip via ShelterMaintenanceSaveStore
                var captured = host.CaptureState();
                bool saveOk = ShelterMaintenanceSaveStore.TrySave(captured);
                var loaded = ShelterMaintenanceSaveStore.TryLoad();

                var restoredHost = ShelterMaintenanceHostSession.Create(dataDir);
                restoredHost.RestoreState(loaded);
                var restoredCensus = restoredHost.Census;

                if (saveOk && loaded != null &&
                    restoredCensus.TotalComponents == host.Census.TotalComponents &&
                    restoredCensus.TotalMaintenanceActions == 3)
                {
                    GD.Print($"[PASS] Check 12: ShelterMaintenanceSaveStore save/load/restore roundtrip verified ({restoredCensus.TotalMaintenanceActions} actions restored).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Save/restore roundtrip failed: saveOk={saveOk}, loadedNull={loaded == null}, actions={restoredCensus.TotalMaintenanceActions}");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during shelter maintenance self-test: {ex}");
            }

            GD.Print($"=== Shelter Maintenance & Degradation Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
