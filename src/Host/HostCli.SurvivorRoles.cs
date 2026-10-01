// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SurvivorRolesSelfTest
// Subsystem          : Plan 195 — Survivor Specialization Roles
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliSurvivorRoles
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Survivor Specialization Roles Self-Test (Plan 195) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from survivor_roles.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "survivor_roles.json");

                var session = SurvivorRoleHostSession.Create();
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.AuthoredRoleCount == 8)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.AuthoredRoleCount} survivor roles.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Survivor roles catalog failed to load (count={session.AuthoredRoleCount}).");
                }

                // Check 2: Role definition discipline gates verified
                var medic = session.GetRoleDef("role_medic");
                var engineer = session.GetRoleDef("role_engineer");
                if (medic != null && engineer != null
                    && medic.required_discipline == "medical" && medic.required_discipline_level == 40f
                    && engineer.required_discipline == "crafting" && engineer.required_discipline_level == 40f)
                {
                    Console.WriteLine("[PASS] Check 2: Discipline gates verified (medic: medical 40, engineer: crafting 40).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Role definition discipline gate verification failed.");
                }

                // Check 3: Eligibility refusal below the discipline gate
                var lowLevels = new Dictionary<string, float> { { "medical", 20f } };
                var refused = session.AssignRole("surv_low", "role_medic", lowLevels, day: 1, out string reasonLow);
                if (refused == null && reasonLow == "insufficient_discipline_medical")
                {
                    Console.WriteLine("[PASS] Check 3: Assignment refused below the discipline gate.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Expected refusal, got assignment={(refused != null)} reason='{reasonLow}'.");
                }

                // Check 4: Eligibility accepted at the discipline gate
                var okLevels = new Dictionary<string, float> { { "medical", 55f } };
                var assigned = session.AssignRole("surv_doc", "role_medic", okLevels, day: 2, out string reasonOk);
                if (assigned != null && assigned.Level == 1 && reasonOk.Length == 0)
                {
                    Console.WriteLine("[PASS] Check 4: Assignment accepted at the discipline gate (level 1).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Expected assignment, got {(assigned != null)} reason='{reasonOk}'.");
                }

                // Check 5: Role cap enforced (max 2 per role)
                var second = session.AssignRole("surv_doc2", "role_medic", okLevels, day: 2, out _);
                var third = session.AssignRole("surv_doc3", "role_medic", okLevels, day: 2, out string reasonCap);
                if (second != null && third == null && reasonCap == "role_cap_reached")
                {
                    Console.WriteLine("[PASS] Check 5: Role cap enforced (third medic refused).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Expected cap refusal, third={(third != null)} reason='{reasonCap}'.");
                }

                // Check 6: Earned practice XP advances levels
                session.AwardPracticeXp("surv_doc", 120);
                var afterXp = session.GetRoleAssignment("surv_doc");
                if (afterXp != null && afterXp.Level == 2 && afterXp.ExperiencePoints == 120)
                {
                    Console.WriteLine("[PASS] Check 6: Earned practice XP advanced the role to level 2.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Practice XP progression failed (level={afterXp?.Level}, xp={afterXp?.ExperiencePoints}).");
                }

                // Check 7: Zero/negative XP facts are rejected
                bool zeroRejected = !session.System.AddRoleXp("surv_doc", 0)
                    && !session.System.AddRoleXp("surv_doc", -5);
                if (zeroRejected)
                {
                    Console.WriteLine("[PASS] Check 7: Zero and negative XP facts rejected.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Zero/negative XP facts were not rejected.");
                }

                // Check 8: Bonus scaling follows the level multiplier (level 2 = 1.25x)
                float healing = session.GetRoleBonus("surv_doc", "healing_effectiveness");
                if (Math.Abs(healing - 0.312f) < 0.0001f)
                {
                    Console.WriteLine($"[PASS] Check 8: Level-scaled bonus verified ({healing} = 0.25 × 1.25, rounded to 3 decimals).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Bonus scaling failed ({healing}).");
                }

                // Check 9: Unassigned survivor has no bonus and unknown bonus type reads zero
                float none = session.GetRoleBonus("surv_nobody", "healing_effectiveness");
                float unknown = session.GetRoleBonus("surv_doc", "welding_speed");
                if (none == 0f && unknown == 0f)
                {
                    Console.WriteLine("[PASS] Check 9: Unassigned and unknown bonus types read zero.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 9: Bonus readout failed (none={none}, unknown={unknown}).");
                }

                // Check 10: Save round-trip through the checksummed store
                bool saved = session.TrySave();
                var reloaded = SurvivorRoleHostSession.Create();
                bool loaded = reloaded.TryLoad();
                var restored = reloaded.GetRoleAssignment("surv_doc");
                if (saved && loaded && restored != null
                    && restored.RoleId == "role_medic" && restored.Level == 2 && restored.ExperiencePoints == 120)
                {
                    Console.WriteLine("[PASS] Check 10: Save/restore round-trip preserved role identity, level, and XP.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Save/restore round-trip failed (saved={saved}, loaded={loaded}).");
                }

                // Check 11: Unassign removes the assignment and frees the cap
                bool removed = reloaded.UnassignRole("surv_doc2");
                var gone = reloaded.GetRoleAssignment("surv_doc2");
                if (removed && gone == null)
                {
                    Console.WriteLine("[PASS] Check 11: Unassignment removed the role record.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Unassignment failed.");
                }

                // Check 12: Save store contract names
                bool survivorRoleContractOk = SurvivorRoleSaveStore.SectionName == "survivor_roles"
                    && SurvivorRoleSaveStore.FileName == "survivor_roles_save.json";
                if (survivorRoleContractOk)
                {
                    Console.WriteLine("[PASS] Check 12: Save store contract names verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Save store contract names mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Survivor Roles Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
