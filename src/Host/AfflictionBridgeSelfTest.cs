// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : AfflictionBridgeSelfTest
// Subsystem          : Plan 143 — Medical Afflictions → Quest/Work Bridge
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class AfflictionBridgeSelfTest
    {
        public static int Run(string? dataDirectory)
        {
            Console.WriteLine("=== [HostCli] Medical Afflictions -> Quest/Work Bridge Self-Test (Plan 143) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDirectory) && Directory.Exists(dataDirectory) ? dataDirectory : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "affliction_bridge_rules.json");

                var bridge = new AfflictionQuestWorkBridge();
                if (File.Exists(catPath))
                {
                    bridge.LoadCatalog(File.ReadAllText(catPath));
                }

                var census = bridge.GetCensus();

                // Check 1: Catalog loading from affliction_bridge_rules.json
                if (census.SchemaVersion == 1 && census.WorkModifierCount == 6 && census.QuestGateCount == 6)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded cleanly (schema={census.SchemaVersion}, modifiers={census.WorkModifierCount}, gates={census.QuestGateCount}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog load failed (schema={census.SchemaVersion}, modifiers={census.WorkModifierCount}, gates={census.QuestGateCount}).");
                }

                // Check 2: Single work speed and quality modifier
                var legMod = bridge.CalculateWorkModifiers(new[] { "affliction_broken_leg" });
                if (Math.Abs(legMod.SpeedMultiplier - 0.5f) < 0.001f &&
                    Math.Abs(legMod.QualityMultiplier - 0.9f) < 0.001f &&
                    legMod.ExcludedDutyTypes.Contains("heavy_labour") &&
                    legMod.ExcludedDutyTypes.Contains("expedition"))
                {
                    Console.WriteLine($"[PASS] Check 2: Single affliction modifier verified (speed={legMod.SpeedMultiplier}, quality={legMod.QualityMultiplier}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Single affliction modifier mismatch (speed={legMod.SpeedMultiplier}, quality={legMod.QualityMultiplier}).");
                }

                // Check 3: Multi-affliction multiplicative compounding
                var multiMod = bridge.CalculateWorkModifiers(new[] { "affliction_broken_leg", "affliction_combat_trauma" });
                float expectedSpeed = 0.5f * 0.8f; // 0.40
                float expectedQuality = 0.9f * 0.75f; // 0.675
                if (Math.Abs(multiMod.SpeedMultiplier - expectedSpeed) < 0.001f &&
                    Math.Abs(multiMod.QualityMultiplier - expectedQuality) < 0.001f)
                {
                    Console.WriteLine($"[PASS] Check 3: Multi-affliction compounding verified (speed={multiMod.SpeedMultiplier:F3}, quality={multiMod.QualityMultiplier:F3}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Multi-affliction compounding mismatch (speed={multiMod.SpeedMultiplier}, expected={expectedSpeed}).");
                }

                // Check 4: Duty exclusion union
                var unionMod = bridge.CalculateWorkModifiers(new[] { "affliction_broken_leg", "affliction_radiation_sickness" });
                bool hasLegDuties = unionMod.ExcludedDutyTypes.Contains("heavy_labour") && unionMod.ExcludedDutyTypes.Contains("expedition");
                bool hasRadDuties = unionMod.ExcludedDutyTypes.Contains("food_handling") && unionMod.ExcludedDutyTypes.Contains("childcare");
                bool distinctDuties = unionMod.ExcludedDutyTypes.Count == unionMod.ExcludedDutyTypes.Distinct().Count();
                if (hasLegDuties && hasRadDuties && distinctDuties)
                {
                    Console.WriteLine($"[PASS] Check 4: Duty exclusion union verified ({unionMod.ExcludedDutyTypes.Count} unique excluded duties).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Duty exclusion union missing duties or contains duplicates.");
                }

                // Check 5: Role exclusion mapping
                bool expExcluded = bridge.IsRoleExcluded(new[] { "affliction_broken_leg" }, "RoleExpedition", out var expDuties);
                bool guardExcluded = bridge.IsRoleExcluded(new[] { "affliction_combat_trauma" }, "RoleGuard", out var guardDuties);
                if (expExcluded && expDuties.Contains("expedition") &&
                    guardExcluded && guardDuties.Contains("guard_duty"))
                {
                    Console.WriteLine("[PASS] Check 5: Role exclusion mapping correctly flags RoleExpedition and RoleGuard.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Role exclusion mapping failed (expExcluded={expExcluded}, guardExcluded={guardExcluded}).");
                }

                // Check 6: Duty roster assignment validation
                bool researchExcluded = bridge.IsRoleExcluded(new[] { "affliction_broken_leg" }, "RoleResearch", out _);
                bool cookExcluded = bridge.IsRoleExcluded(new[] { "affliction_radiation_sickness" }, "RoleCook", out var cookDuties);
                if (!researchExcluded && cookExcluded && cookDuties.Contains("food_handling"))
                {
                    Console.WriteLine("[PASS] Check 6: Duty roster assignment validation allows research and rejects cooking.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Assignment validation failed (researchExcluded={researchExcluded}, cookExcluded={cookExcluded}).");
                }

                // Check 7: Duty roster throughput floor calculation
                var extremeMod = bridge.CalculateWorkModifiers(new[]
                {
                    "affliction_broken_leg",
                    "affliction_broken_arm",
                    "affliction_respiratory_degeneration",
                    "affliction_radiation_sickness",
                    "affliction_chemical_dependency"
                });
                if (Math.Abs(extremeMod.SpeedMultiplier - 0.1f) < 0.001f)
                {
                    Console.WriteLine($"[PASS] Check 7: Work speed throughput floor capped at 0.10 (actual={extremeMod.SpeedMultiplier}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Work speed floor mismatch (actual={extremeMod.SpeedMultiplier}, expected=0.10).");
                }

                // Check 8: Quest gate blocking
                var legGate = bridge.QueryQuestGate("expedition_quest", new[] { "affliction_broken_leg" });
                var armGate = bridge.QueryQuestGate("combat_quest", new[] { "affliction_broken_arm" });
                var safeGate = bridge.QueryQuestGate("water_well_quest", new[] { "affliction_broken_leg" });
                if (legGate.IsBlocked && legGate.BlockingAfflictionIds.Contains("affliction_broken_leg") &&
                    armGate.IsBlocked && armGate.BlockingAfflictionIds.Contains("affliction_broken_arm") &&
                    !safeGate.IsBlocked)
                {
                    Console.WriteLine("[PASS] Check 8: Quest gate blocking verified for expedition_quest and combat_quest.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Quest gate blocking failed.");
                }

                // Check 9: Quest gate unlocking
                var radUnlock = bridge.QueryQuestGate("find_anti_rad", new[] { "affliction_radiation_sickness" });
                var ptsdUnlock = bridge.QueryQuestGate("ptsd_support", new[] { "affliction_combat_trauma" });
                var detoxUnlock = bridge.QueryQuestGate("detox_program", new[] { "affliction_chemical_dependency" });
                var respUnlock = bridge.QueryQuestGate("air_filtration_upgrade", new[] { "affliction_respiratory_degeneration" });
                if (radUnlock.IsUnlocked && ptsdUnlock.IsUnlocked && detoxUnlock.IsUnlocked && respUnlock.IsUnlocked)
                {
                    Console.WriteLine("[PASS] Check 9: Quest gate unlocking verified for all 4 medical opportunity quests.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Quest gate unlocking failed for one or more opportunity quests.");
                }

                // Check 10: Side-effect-free idempotency
                var firstQuery = bridge.QueryQuestGate("find_anti_rad", new[] { "affliction_radiation_sickness" });
                var secondQuery = bridge.QueryQuestGate("find_anti_rad", new[] { "affliction_radiation_sickness" });
                var censusAfter = bridge.GetCensus();
                if (firstQuery.IsUnlocked == secondQuery.IsUnlocked &&
                    firstQuery.IsBlocked == secondQuery.IsBlocked &&
                    censusAfter.WorkModifierCount == census.WorkModifierCount &&
                    censusAfter.QuestGateCount == census.QuestGateCount)
                {
                    Console.WriteLine("[PASS] Check 10: Side-effect-free idempotency confirmed across repeated queries.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Query mutation or state drift detected during repeated queries.");
                }

                // Check 11: Recovery / treatment restores capacity
                var healedMod = bridge.CalculateWorkModifiers(Array.Empty<string>());
                bool healedExpExcluded = bridge.IsRoleExcluded(Array.Empty<string>(), "RoleExpedition", out _);
                var healedGate = bridge.QueryQuestGate("expedition_quest", Array.Empty<string>());
                if (Math.Abs(healedMod.SpeedMultiplier - 1.0f) < 0.001f &&
                    Math.Abs(healedMod.QualityMultiplier - 1.0f) < 0.001f &&
                    healedMod.ExcludedDutyTypes.Count == 0 &&
                    !healedExpExcluded &&
                    !healedGate.IsBlocked &&
                    !healedGate.IsUnlocked)
                {
                    Console.WriteLine("[PASS] Check 11: Survivor recovery restores full 1.0x capacity, 0 duty exclusions, and unlocks/unblocks cleanly.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Survivor recovery did not fully restore capacity.");
                }

                // Check 12: Host session bridge binding
                var session = MedicalHostSession.Create(dataRoot);
                var sessionCensus = session.Bridge.GetCensus();
                if (session.Bridge != null &&
                    sessionCensus.WorkModifierCount == 6 &&
                    sessionCensus.QuestGateCount == 6)
                {
                    Console.WriteLine($"[PASS] Check 12: MedicalHostSession initialized and bound bridge cleanly ({sessionCensus.WorkModifierCount} modifiers, {sessionCensus.QuestGateCount} gates).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 12: MedicalHostSession bridge binding failed (modifiers={sessionCensus.WorkModifierCount}, gates={sessionCensus.QuestGateCount}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception during AfflictionBridgeSelfTest: {ex.Message}\n{ex.StackTrace}");
            }

            Console.WriteLine($"=== AfflictionBridgeSelfTest: {passed}/{total} checks passed. ===");
            return passed == total ? 0 : 1;
        }
    }
}
