// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SkillAtrophySelfTest
// Subsystem          : Skill Atrophy (unused-skill decay)
// ============================================================================

using System;
using System.Linq;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliSkillAtrophy
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Skill Atrophy Self-Test ===");
            int passed = 0;
            const int total = 7;

            try
            {
                var session = SkillAtrophyHostSession.Create();

                if (session.TrackedActorCount == 0 && session.GetAtrophiedSkillIds("surv_a").Count == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Atrophy ledger starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Ledger was not empty.");
                }

                // High-morale actor across many days must not atrophy.
                session.Tick(24f, new[] { new HostedSkillActor("surv_ok", 60f, 100f, 0f) });
                if (session.GetAtrophiedSkillIds("surv_ok").Count == 0)
                {
                    Console.WriteLine("[PASS] Check 2: High-morale survivor stays practiced.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: High-morale survivor atrophied.");
                }

                // Low morale below the window: no atrophy yet.
                session.Tick(24f * 7f, new[] { new HostedSkillActor("surv_a", 5f, 100f, 0f) });
                if (session.GetAtrophiedSkillIds("surv_a").Count == 0
                    && session.TrackedActorCount == 2)
                {
                    Console.WriteLine("[PASS] Check 3: Below the atrophy window no skill decays.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Premature atrophy ({session.GetAtrophiedSkillIds("surv_a").Count}).");
                }

                // Cross the window: medical + crafting atrophy together.
                session.Tick(24f * 8f, new[] { new HostedSkillActor("surv_a", 5f, 100f, 0f) });
                var atrophied = session.GetAtrophiedSkillIds("surv_a");
                if (atrophied.Count == 2
                    && atrophied.Contains("medical") && atrophied.Contains("crafting"))
                {
                    Console.WriteLine("[PASS] Check 4: Sustained low morale atrophied medical and crafting.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Atrophy set mismatch ({string.Join(",", atrophied)}).");
                }

                // Exactly-once: more low-morale time must not re-add skills.
                session.Tick(24f * 20f, new[] { new HostedSkillActor("surv_a", 5f, 100f, 0f) });
                if (session.GetAtrophiedSkillIds("surv_a").Count == 2 && session.IsAtrophied("surv_a", "medical"))
                {
                    Console.WriteLine("[PASS] Check 5: Atrophy events are exactly-once.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Atrophy repeated ({session.GetAtrophiedSkillIds("surv_a").Count}).");
                }

                // Recovering morale records tracked state without reversing history.
                session.Tick(24f, new[] { new HostedSkillActor("surv_a", 60f, 100f, 0f) });
                if (session.GetAtrophiedSkillIds("surv_a").Count == 2)
                {
                    Console.WriteLine("[PASS] Check 6: Recovered morale preserves the atrophy ledger.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Atrophy ledger was reset by recovery.");
                }

                bool saved = session.TrySave();
                var reloaded = SkillAtrophyHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.GetAtrophiedSkillIds("surv_a").Count == 2
                    && SkillAtrophySaveStore.SectionName.Equals("skill_atrophy", StringComparison.Ordinal)
                    && SkillAtrophySaveStore.FileName.Equals("skill_atrophy_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 7: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Save/restore failed (saved={saved}, loaded={loaded}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Skill Atrophy Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
