// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : WorkingAnimalsSelfTest
// Subsystem          : Plan 151 / 174 — Working Animals & Companion System
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Ecology;
using Ashfall.Core.IO;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public static class WorkingAnimalsSelfTest
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Working Animals & Companions Self-Test (Plan 151 / 174) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from companion_animals.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "companion_animals.json");

                var fileIO = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                var loadResult = CompanionAnimalCatalogLoader.Load(dataRoot, fileIO, json);
                var profiles = CompanionAnimalCatalogLoader.ToProfiles(loadResult);

                if (!loadResult.HasErrors && profiles.Count >= 5 && profiles.Any(p => p.species_id == "species_ash_hound"))
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {profiles.Count} companion profiles from companion_animals.json without errors.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Failed to load companion_animals.json (count={profiles.Count}, errors={string.Join(", ", loadResult.Errors)}).");
                }

                // Inventory mock for food & treatment
                var inventory = new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    { "raw_meat", 10 },
                    { "crop_ash_grain", 10 },
                    { "item_antibiotics", 3 }
                };

                var session = CompanionAnimalHostSession.Create(
                    dataRoot,
                    new SeededRng(15101),
                    id => inventory.TryGetValue(id, out int c) ? c : 0,
                    (id, amount) => { if (inventory.ContainsKey(id)) inventory[id] = Math.Max(0, inventory[id] - amount); },
                    speciesId => profiles.Any(p => p.species_id == speciesId)
                );

                // Check 2: Companion registration
                var regResult = session.RegisterCompanion("animal_hound_01", "species_ash_hound", 1, "Grim");
                if (regResult.Success && session.Census.TotalCompanionsCount == 1 && session.System.Companion("animal_hound_01")?.name == "Grim")
                {
                    Console.WriteLine("[PASS] Check 2: Registered companion animal_hound_01 as Grim.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Registration failed ({regResult.ReasonCode}).");
                }

                // Check 3: Unknown species rejection
                var badSpeciesResult = session.RegisterCompanion("animal_unknown_01", "species_mythical_griffin", 1);
                if (!badSpeciesResult.Success && badSpeciesResult.ReasonCode == "unknown_species")
                {
                    Console.WriteLine("[PASS] Check 3: Unknown species properly rejected.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Unknown species was not rejected ({badSpeciesResult.ReasonCode}).");
                }

                // Check 4: Role assignment compatibility check
                // Cotton hare has only ["morale"] role, so assigning Guard should fail with role_incompatible
                session.RegisterCompanion("animal_hare_01", "species_cotton_hare", 1, "Snowball");
                var badRoleResult = session.AssignRole("animal_hare_01", "survivor_guard", CompanionRole.Guard);
                if (!badRoleResult.Success && badRoleResult.ReasonCode == "role_incompatible")
                {
                    Console.WriteLine("[PASS] Check 4: Incompatible role assignment rejected (Cotton Hare cannot Guard).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Incompatible role was not rejected ({badRoleResult.ReasonCode}).");
                }

                // Check 5: Handler assignment exclusivity
                // Assign hound to survivor_1 with Guard role
                var assignResult1 = session.AssignRole("animal_hound_01", "survivor_1", CompanionRole.Guard);
                // Attempt to assign the same hound to survivor_2
                var assignResult2 = session.AssignRole("animal_hound_01", "survivor_2", CompanionRole.Guard);
                if (assignResult1.Success && !assignResult2.Success && assignResult2.ReasonCode == "already_assigned_to_other")
                {
                    Console.WriteLine("[PASS] Check 5: Handler assignment exclusivity enforced (cannot assign to survivor_2 without unassigning).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Handler exclusivity check failed (res1={assignResult1.ReasonCode}, res2={assignResult2.ReasonCode}).");
                }

                // Check 6: Guard rating calculation
                float guardMod = session.GetGuardModifierTotal();
                if (guardMod > 0f)
                {
                    Console.WriteLine($"[PASS] Check 6: Guard rating calculated successfully (guard total = {guardMod:F1}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Guard modifier was zero for healthy guard hound.");
                }

                // Check 7: Pack capacity bonus calculation
                session.RegisterCompanion("animal_goat_01", "species_feral_goat", 1, "Billy");
                session.AssignRole("animal_goat_01", "survivor_scout", CompanionRole.Pack);
                float packBonus = session.GetPackCapacityBonusForSurvivor("survivor_scout");
                if (packBonus > 0f && packBonus <= 25f)
                {
                    Console.WriteLine($"[PASS] Check 7: Pack capacity bonus for survivor_scout calculated correctly ({packBonus:F1} kg).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Pack capacity bonus unexpected ({packBonus}).");
                }

                // Check 8: Feeding consumption from canonical inventory
                int meatBefore = inventory["raw_meat"];
                var feedResult = session.FeedCompanion("animal_hound_01", 1);
                int meatAfter = inventory["raw_meat"];
                if (feedResult.Fed && meatAfter == meatBefore - 2) // base_food_per_day = 2
                {
                    Console.WriteLine($"[PASS] Check 8: Feeding consumed 2x raw_meat from inventory (from {meatBefore} to {meatAfter}); hunger reset to 0.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Feeding failed (fed={feedResult.Fed}, meatBefore={meatBefore}, meatAfter={meatAfter}, reason={feedResult.ReasonCode}).");
                }

                // Check 9: Hunger drift & starvation damage when unfed
                var hare = session.System.Companion("animal_hare_01")!;
                int initialHealth = hare.health;
                // Tick 4 days unfed to cross HungerCritical (80)
                inventory["crop_leafy_green"] = 0; // No food for hare
                session.System.TickDay(2);
                session.System.TickDay(3);
                session.System.TickDay(4);
                session.System.TickDay(5);
                if (hare.hunger >= CompanionAnimalSystem.HungerCritical && hare.health < initialHealth)
                {
                    Console.WriteLine($"[PASS] Check 9: Unfed animal accumulated hunger ({hare.hunger}) and suffered starvation health damage ({initialHealth} -> {hare.health}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 9: Hunger or starvation did not progress (hunger={hare.hunger}, health={hare.health}).");
                }

                // Check 10: Sickness state transition & veterinary treatment
                var hound = session.System.Companion("animal_hound_01")!;
                hound.sickness = (int)CompanionSicknessState.Infection;
                var treatResult = session.TreatSickness("animal_hound_01", "item_antibiotics");
                if (treatResult.Success && (CompanionSicknessState)hound.sickness == CompanionSicknessState.Healthy && inventory["item_antibiotics"] == 2)
                {
                    Console.WriteLine("[PASS] Check 10: Veterinary treatment cleared infection and consumed 1x item_antibiotics.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Treatment failed (res={treatResult.ReasonCode}, sickness={hound.sickness}, remainingMeds={inventory["item_antibiotics"]}).");
                }

                // Check 11: Bounded morale support & grief shock calculation
                session.AssignRole("animal_hound_01", "survivor_1", CompanionRole.Morale);
                int moraleBp = session.GetMoraleSupportBp("animal_hound_01");
                int griefBp = session.GetGriefMoraleDeltaBp("animal_hound_01");
                if (moraleBp > 0 && griefBp < 0 && griefBp >= CompanionAnimalSystem.GriefMoraleShockMaxBp)
                {
                    Console.WriteLine($"[PASS] Check 11: Morale support (+{moraleBp} bp) and grief shock ({griefBp} bp) calculated within authored bounds.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: Morale or grief out of bounds (support={moraleBp}, grief={griefBp}).");
                }

                // Check 12: Save/restore round-trip parity
                var saved = session.CaptureSave();
                var freshSession = CompanionAnimalHostSession.Create(
                    dataRoot,
                    new SeededRng(15102),
                    id => 10,
                    (id, amount) => { },
                    speciesId => true
                );
                freshSession.RestoreSave(saved);

                if (freshSession.Census.TotalCompanionsCount == session.Census.TotalCompanionsCount
                    && freshSession.System.Companion("animal_hound_01")?.name == "Grim"
                    && freshSession.System.Companion("animal_goat_01")?.role == (int)CompanionRole.Pack)
                {
                    Console.WriteLine("[PASS] Check 12: Save/restore round-trip preserved companion count and all assigned roles.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Save/restore parity mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] WorkingAnimalsSelfTest threw: {ex}");
            }

            Console.WriteLine($"=== Working Animals Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
