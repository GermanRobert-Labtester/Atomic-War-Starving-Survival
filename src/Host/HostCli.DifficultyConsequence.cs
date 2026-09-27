// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : DifficultyConsequenceSelfTest
// Core Authority     : Ashfall.Core.Difficulty.DifficultyConsequenceWeave (EN-01)
// Purpose            : difficulty scalars -> war/crisis/shock consequence read model
// ============================================================================

using System;
using Ashfall.Core.Difficulty;

namespace AtomicWar.GodotApp
{
    public static class HostCliDifficultyConsequence
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Difficulty-Consequence Weave Self-Test (EN-01) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                bool nullRefused = false;
                try { _ = new DifficultyConsequenceWeave(null!); }
                catch (ArgumentNullException) { nullRefused = true; }
                if (nullRefused)
                {
                    Console.WriteLine("[PASS] Check 1: a null scalar provider is refused.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: null provider accepted."); }

                var legacy = new DifficultyConsequenceWeave(DifficultyScalarsProvider.Legacy);
                if (Math.Abs(legacy.WarStageSeverityMultiplier - 1f) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 2: legacy severity multiplier is neutral (1.0).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: severity = {legacy.WarStageSeverityMultiplier}."); }

                if (legacy.ComputeCrisisDeadlineDays(10) == 10 && legacy.ComputeCrisisDeadlineDays(0) == 1 && legacy.ComputeCrisisDeadlineDays(-5) == 1)
                {
                    Console.WriteLine("[PASS] Check 3: legacy deadline passes through, and non-positive base days clamp to 1.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: deadline {legacy.ComputeCrisisDeadlineDays(10)}/{legacy.ComputeCrisisDeadlineDays(0)}/{legacy.ComputeCrisisDeadlineDays(-5)}."); }

                if (Math.Abs(legacy.ShockWeightMultiplier - 1f) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 4: legacy shock weight multiplier is neutral (1.0).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: shock weight = {legacy.ShockWeightMultiplier}."); }

                if (DifficultyConsequenceWeave.IsMonotonicallyHarsherOrEqual(DifficultyScalarsProvider.Legacy, DifficultyScalarsProvider.Legacy))
                {
                    Console.WriteLine("[PASS] Check 5: legacy is monotonically equal to itself.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 5: legacy self-monotonicity failed."); }

                var harsh = DifficultyScalarsProvider.FromPreset(new DifficultyPreset
                {
                    id = "difficulty_harsh_selftest",
                    display_name = "Harsh",
                    description = "Harsh survival parameters.",
                    scalars = new DifficultyScalars
                    {
                        hunger_rate_mult = 1.5f,
                        thirst_rate_mult = 1.5f,
                        radiation_gain_mult = 1.5f,
                        disease_onset_mult = 1.5f,
                        hostile_encounter_mult = 1.5f,
                        market_price_mult = 1.2f,
                        equipment_decay_mult = 1.3f,
                        crisis_deadline_mult = 0.8f
                    }
                });
                var weave = new DifficultyConsequenceWeave(harsh);

                if (Math.Abs(weave.WarStageSeverityMultiplier - 1.875f) < 0.001f)
                {
                    Console.WriteLine($"[PASS] Check 6: harsh severity scalar scales to 1.875 ({weave.WarStageSeverityMultiplier:F3}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: harsh severity = {weave.WarStageSeverityMultiplier}."); }

                if (weave.ComputeCrisisDeadlineDays(10) == 8)
                {
                    Console.WriteLine("[PASS] Check 7: harsh deadline shortens 10 days to 8 (0.8x).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: harsh deadline = {weave.ComputeCrisisDeadlineDays(10)}."); }

                bool harsherForward = DifficultyConsequenceWeave.IsMonotonicallyHarsherOrEqual(harsh, DifficultyScalarsProvider.Legacy);
                bool harsherReverse = DifficultyConsequenceWeave.IsMonotonicallyHarsherOrEqual(DifficultyScalarsProvider.Legacy, harsh);
                if (harsherForward && !harsherReverse)
                {
                    Console.WriteLine("[PASS] Check 8: monotonicity ordering holds in one direction only (harsh > legacy).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: monotonicity forward={harsherForward} reverse={harsherReverse}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Difficulty-consequence weave: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
