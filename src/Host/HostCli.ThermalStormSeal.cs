// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ThermalStormSealSelfTest
// Subsystem          : Authored insulation catalog bound to the live thermal owner.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp
{
    public static class HostCliThermalStormSeal
    {
        private const string StormSealingId = "insul_storm_sealing";

        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        private static bool Ok(ActionResult r) => r.Status == ActionResult.StatusKind.Success;

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Authored Insulation Catalog Self-Test ===");
            int passed = 0;
            const int total = 10;
            try
            {
                string dir = !string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();
                string json = File.ReadAllText(Path.Combine(dir, "shelter_insulation_catalog.json"));

                var system = new ShelterThermalSystem(
                    new SeededRng(1986), new NeedsSystem(), new StartingLevelSystem(),
                    new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState()), NullLog.Instance);
                system.State.rooms.Add(new ThermalRoomNode { roomId = "room_bunk", displayName = "Bunk" });
                string room = "room_bunk";

                // 1-2. Reproduce the shipped defect exactly.
                Check(!system.InsulationCatalog.ContainsKey(StormSealingId),
                    "Check 1: built-in defaults do not include the id the storm-sealing routine asks for.");
                passed += !system.InsulationCatalog.ContainsKey(StormSealingId) ? 1 : 0;

                var beforeBind = system.RetrofitInsulation(room, StormSealingId, null);
                Check(!Ok(beforeBind) && beforeBind.FailureCode == "unknown_insulation",
                    "Check 2: storm sealing therefore fails 'unknown_insulation' (the silent bug).");
                passed += (!Ok(beforeBind) && beforeBind.FailureCode == "unknown_insulation") ? 1 : 0;

                // 3. Bind authored rows through the owner's own seam.
                int before = system.InsulationCatalog.Count;
                system.LoadInsulationCatalog(json);
                Check(system.InsulationCatalog.Count > before
                      && system.InsulationCatalog.ContainsKey(StormSealingId),
                    $"Check 3: authored rows loaded ({before} -> {system.InsulationCatalog.Count}).");
                passed += system.InsulationCatalog.ContainsKey(StormSealingId) ? 1 : 0;

                // 4. All authored tiers are present.
                Check(system.InsulationCatalog.Count == 5,
                    $"Check 4: all 5 authored tiers reachable ({system.InsulationCatalog.Count}).");
                passed += system.InsulationCatalog.Count == 5 ? 1 : 0;

                // 5. Authored numbers win over built-in literals.
                var scrap = system.InsulationCatalog["insul_scrap_panels"];
                Check(Math.Abs(scrap.thermal_conductivity - 0.085f) < 0.0001f
                      && Math.Abs(scrap.air_leak_factor - 0.45f) < 0.0001f
                      && scrap.level == 1 && scrap.max_upgrade_level == 3,
                    "Check 5: a pre-existing tier answers to authored values (JSON authoritative).");
                passed += Math.Abs(scrap.thermal_conductivity - 0.085f) < 0.0001f ? 1 : 0;

                // 6. The live action now works.
                var afterBind = system.RetrofitInsulation(room, StormSealingId, null);
                Check(Ok(afterBind), "Check 6: storm sealing now actually seals a room.");
                passed += Ok(afterBind) ? 1 : 0;

                // 7. Observable outcome recorded on the owner's persisted state.
                Check(system.State.roomInstalledInsulation.TryGetValue(room, out var installed)
                      && installed == StormSealingId,
                    "Check 7: the retrofit is recorded in thermal state (save path already owns it).");
                passed += system.State.roomInstalledInsulation.TryGetValue(room, out var i2) && i2 == StormSealingId ? 1 : 0;

                // 8. Room insulation factor moved (the gameplay effect of sealing).
                var node = system.State.rooms.Find(r => r.roomId == room);
                Check(node != null && node.insulationFactor > 1f,
                    $"Check 8: sealing raised the room's insulation factor ({node?.insulationFactor:0.###}).");
                passed += node != null && node.insulationFactor > 1f ? 1 : 0;

                // 9. Unauthorised ids are still rejected.
                var ghost = system.RetrofitInsulation(room, "insul_not_authored", null);
                Check(!Ok(ghost) && ghost.FailureCode == "unknown_insulation",
                    "Check 9: ids outside the authored table are still refused.");
                passed += !Ok(ghost) ? 1 : 0;

                // 10. Reloading is idempotent and a bad payload cannot empty the table.
                int stable = system.InsulationCatalog.Count;
                system.LoadInsulationCatalog(json);
                system.LoadInsulationCatalog("{ \"insulations\": [] }");
                system.LoadInsulationCatalog("   ");
                Check(system.InsulationCatalog.Count == stable,
                    "Check 10: re-binding is idempotent and empty/invalid payloads change nothing.");
                passed += system.InsulationCatalog.Count == stable ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Authored Insulation Catalog Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
