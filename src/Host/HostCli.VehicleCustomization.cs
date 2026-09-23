// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 152 (Vehicle Customization & Mobile Base).

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Vehicles;

namespace AtomicWar.GodotApp
{
    public static class HostCliVehicleCustomization
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Vehicle Customization & Mobile Base Self-Test (Plan 152) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Strict catalog loading
                var host = VehicleCustomizationHostSession.Create(dataDir);
                if (host.UsingAuthoredCatalog && host.Catalog.AllModules.Count >= 20)
                {
                    GD.Print($"[PASS] Check 1: Authored vehicle module catalog loaded strictly ({host.Catalog.AllModules.Count} modules).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Expected >= 20 authored modules, got {host.Catalog.AllModules.Count} (error: {host.LoadError}).");
                }

                // Check 2: All five module categories present
                var types = host.Catalog.AllModules.Select(m => m.ModuleType.ToLowerInvariant()).Distinct().ToList();
                if (types.Contains("armor") && types.Contains("cargo") && types.Contains("living") &&
                    types.Contains("weapon") && types.Contains("utility"))
                {
                    GD.Print($"[PASS] Check 2: All 5 module categories present ({string.Join(", ", types.OrderBy(t => t))}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Missing categories. Found: {string.Join(", ", types)}");
                }

                // Check 3: Install a module
                bool installed = host.InstallModule("v_hauler", "reinforced_hull");
                if (installed && host.GetInstalledModules("v_hauler").Count == 1)
                {
                    GD.Print("[PASS] Check 3: Module installed onto the vehicle.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Module install failed.");
                }

                // Check 4: Slot cap enforced
                host.InstallModule("v_hauler", "roof_rack");
                host.InstallModule("v_hauler", "bunk_beds_module");
                host.InstallModule("v_hauler", "extended_cargo_bed");
                bool fifth = host.InstallModule("v_hauler", "heavy_winch", maxSlots: 4);
                if (!fifth && host.GetInstalledModules("v_hauler").Count == 4)
                {
                    GD.Print("[PASS] Check 4: Slot cap enforced at 4 modules; the 5th install was refused.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Slot cap not enforced (installed={host.GetInstalledModules("v_hauler").Count}, fifth={fifth}).");
                }

                // Check 5: Duplicate install rejected
                bool duplicate = host.InstallModule("v_hauler", "reinforced_hull");
                if (!duplicate)
                {
                    GD.Print("[PASS] Check 5: Duplicate module install refused.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Duplicate module install accepted.");
                }

                // Check 6: Remove a module
                bool removed = host.RemoveModule("v_hauler", "roof_rack");
                if (removed && host.GetInstalledModules("v_hauler").Count == 3)
                {
                    GD.Print("[PASS] Check 6: Module removed cleanly.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Module removal failed.");
                }

                // Check 7: Effective stats reflect installed modules
                // hull(-0.06 def20) + bunk_beds(-0.05) + extended_bed(-0.04) => speed 0.85, cargo 150, defense 20
                var stats = host.CalculateEffectiveStats("v_hauler");
                if (Math.Abs(stats.speedMult - 0.85f) < 0.001f &&
                    Math.Abs(stats.cargoCapacity - 150f) < 0.001f &&
                    Math.Abs(stats.defense - 20f) < 0.001f)
                {
                    GD.Print($"[PASS] Check 7: Effective stats correct (speed={stats.speedMult:F2}, cargo={stats.cargoCapacity:F0}, defense={stats.defense:F0}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Stats unexpected (speed={stats.speedMult:F2}, cargo={stats.cargoCapacity:F0}, defense={stats.defense:F0}).");
                }

                // Check 8: Mobile base capability requires a living module
                bool stockCapable = host.IsMobileBaseCapable("v_stock");
                bool fittedCapable = host.IsMobileBaseCapable("v_hauler");
                if (!stockCapable && fittedCapable)
                {
                    GD.Print("[PASS] Check 8: Mobile base capability tracks the living module (stock=false, fitted=true).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Capability unexpected (stock={stockCapable}, fitted={fittedCapable}).");
                }

                // Check 9: Bunk capacity from living modules
                int bunks = host.GetBunkCapacity("v_hauler");
                if (bunks == 4)
                {
                    GD.Print($"[PASS] Check 9: Bunk capacity reported as {bunks}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Expected 4 bunks, got {bunks}.");
                }

                // Check 10: Base camp deploy / pack lifecycle
                bool deployed = host.DeployBaseCamp("v_hauler", "loc_relay_station");
                bool locationOk = host.GetBaseCampLocation("v_hauler") == "loc_relay_station";
                bool packed = host.PackBaseCamp("v_hauler");
                if (deployed && locationOk && packed && !host.IsBaseCampDeployed("v_hauler"))
                {
                    GD.Print("[PASS] Check 10: Base camp deployed, located, and packed cleanly.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Base camp lifecycle failed (deployed={deployed}, locationOk={locationOk}, packed={packed}).");
                }

                // Check 11: Shelter capacity is bounded by bunks
                var sleepers = new List<string> { "s1", "s2", "s3", "s4", "s5", "s6" };
                int rested = host.RestSurvivorsInVehicle("v_hauler", sleepers);
                if (rested == 4)
                {
                    GD.Print($"[PASS] Check 11: {rested} of {sleepers.Count} survivors rested, capped by bunk capacity.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Expected 4 rested, got {rested}.");
                }

                // Check 12: Save capture / restore round-trip + SaveStore metadata
                host.DeployBaseCamp("v_hauler", "loc_relay_station");
                var persisted = host.CapturePersistedState();
                var restored = new VehicleCustomizationHostSession(dataDir);
                restored.RestoreCoreState(persisted.core_state);
                if (restored.GetInstalledModules("v_hauler").Count == 3 &&
                    restored.GetBaseCampLocation("v_hauler") == "loc_relay_station" &&
                    string.Equals(VehicleCustomizationSaveStore.FileName, "vehicle_customization_save.json", StringComparison.Ordinal) &&
                    string.Equals(VehicleCustomizationSaveStore.SectionName, "vehicle_customization", StringComparison.Ordinal))
                {
                    GD.Print("[PASS] Check 12: Save capture / restore verified with correct SaveStore metadata.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Round-trip mismatch (modules={restored.GetInstalledModules("v_hauler").Count}, camp={restored.GetBaseCampLocation("v_hauler")}).");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FATAL] HostCliVehicleCustomization exception: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== [HostCli] VehicleCustomization Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
