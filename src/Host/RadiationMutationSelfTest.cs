// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RadiationMutationSelfTest
// Subsystem          : Plan 172 — Radiation Mutation & Genetic Instability
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public static class RadiationMutationSelfTest
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Radiation Mutation System Self-Test (Plan 172) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from mutations.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "mutations.json");

                var inv = new Ashfall.Core.Inventory.Inventory();
                var session = RadiationMutationHostSession.Create(dataRoot, new SeededRng(17201), inv);

                if (session.Census.AuthoredMutationsCount == 9)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.Census.AuthoredMutationsCount} authored mutations.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog count mismatch (expected 9, got {session.Census.AuthoredMutationsCount}).");
                }

                // Check 2: Initial profile baseline
                var prof = session.System.EnsureProfile("surv_alpha");
                if (prof.cumulativeRadDose == 0f && prof.geneticInstability == 0f && prof.activeMutationIds.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 2: Initial survivor mutation profile baseline verified (zero dose, zero instability, 0 mutations).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Initial profile was not cleanly zeroed.");
                }

                // Check 3: Positive exposure accrual without premature instability spike (<= 20 mSv)
                bool exposed15 = session.AddExposure("surv_alpha", 15.0f, 1);
                prof = session.GetProfile("surv_alpha")!;
                if (exposed15 && Math.Abs(prof.cumulativeRadDose - 15.0f) < 0.001f && prof.geneticInstability == 0f)
                {
                    Console.WriteLine("[PASS] Check 3: Positive exposure accrual verified (15.0 mSv added, 0 instability spike below 20 mSv).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Exposure accrual failed (dose={prof?.cumulativeRadDose}, instability={prof?.geneticInstability}).");
                }

                // Check 4: Zero and negative exposure refusal
                bool refZero = session.AddExposure("surv_alpha", 0f, 1);
                bool refNeg = session.AddExposure("surv_alpha", -10.0f, 1);
                if (!refZero && !refNeg && prof != null && Math.Abs(prof.cumulativeRadDose - 15.0f) < 0.001f)
                {
                    Console.WriteLine("[PASS] Check 4: Zero and negative exposure delta safely refused without side effects.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Zero/negative exposure was not refused.");
                }

                // Check 5: Instability spike threshold (> 20.0 mSv)
                // Adding 30 mSv: spike = (30 - 20) * 0.25 = 2.5
                bool exposed30 = session.AddExposure("surv_alpha", 30.0f, 1);
                if (exposed30 && prof != null && Math.Abs(prof.geneticInstability - 2.5f) < 0.01f && Math.Abs(prof.lifetimePeakInstability - 2.5f) < 0.01f)
                {
                    Console.WriteLine("[PASS] Check 5: Instability spike threshold verified (> 20 mSv induced 2.5% instability).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Instability spike calculation mismatch (got {prof?.geneticInstability}).");
                }

                // Check 6: Deterministic mutation evaluation under threshold
                // Add more exposure so cumulative dose >= 80 mSv
                session.AddExposure("surv_alpha", 60.0f, 1); // Total dose = 15 + 30 + 60 = 105 mSv
                // With cumulative dose 105 mSv, mutation_low_light_adaptation (req 80) and mutation_photophobia (req 100) are eligible.
                // We use deterministic rng seeded session
                var detRng = new SeededRng(42);
                var detSession = RadiationMutationHostSession.Create(dataRoot, detRng, inv);
                bool mutated = false;
                for (int d = 1; d <= 5 && !mutated; d++)
                {
                    detSession.AddExposure("surv_det", 60.0f, d);
                    mutated = detSession.TryMutateSurvivor("surv_det", d);
                }
                var detProf = detSession.GetProfile("surv_det")!;
                if (mutated && detProf != null && detProf.activeMutationIds.Count >= 1)
                {
                    Console.WriteLine($"[PASS] Check 6: Deterministic mutation acquisition succeeded (developed {detProf.activeMutationIds[0]}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Deterministic mutation evaluation failed (mutated={mutated}, count={detProf?.activeMutationIds.Count}).");
                }

                // Check 7: Mutually exclusive nodes respected
                // Manually grant mutation_low_light_adaptation, then verify mutation_photophobia cannot be acquired
                var testSession = RadiationMutationHostSession.Create(dataRoot, new SeededRng(999), inv);
                var testProf = testSession.System.EnsureProfile("surv_excl");
                testProf.cumulativeRadDose = 200.0f;
                testProf.activeMutationIds.Add("mutation_low_light_adaptation");
                // TryMutate on day 2 should not pick mutation_photophobia
                testSession.TryMutateSurvivor("surv_excl", 2);
                bool hasExclusiveConflict = testProf.activeMutationIds.Contains("mutation_low_light_adaptation")
                    && testProf.activeMutationIds.Contains("mutation_photophobia");
                if (!hasExclusiveConflict)
                {
                    Console.WriteLine("[PASS] Check 7: Mutually exclusive mutation nodes properly respected.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Exclusive mutation nodes conflicted.");
                }

                // Check 8: Parent prerequisites enforced
                // mutation_heightened_hearing requires parent mutation_low_light_adaptation
                var orphanSession = RadiationMutationHostSession.Create(dataRoot, new SeededRng(1), inv);
                var orphanProf = orphanSession.System.EnsureProfile("surv_orphan");
                orphanProf.cumulativeRadDose = 300.0f; // More than 180 required for heightened hearing
                // TryMutate on day 1 should never pick heightened hearing because parent is missing
                orphanSession.TryMutateSurvivor("surv_orphan", 1);
                bool acquiredHearingWithoutParent = orphanProf.activeMutationIds.Contains("mutation_heightened_hearing");
                if (!acquiredHearingWithoutParent)
                {
                    Console.WriteLine("[PASS] Check 8: Parent mutation prerequisites strictly enforced.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Tier 2 mutation acquired without parent requirement.");
                }

                // Check 9: Capability tags and stat modifiers projection
                var projSession = RadiationMutationHostSession.Create(dataRoot, new SeededRng(1), inv);
                var projProf = projSession.System.EnsureProfile("surv_proj");
                projProf.activeMutationIds.Add("mutation_low_light_adaptation");
                projProf.activeMutationIds.Add("mutation_dense_bone");
                var caps = projSession.GetCapabilityTags("surv_proj");
                var stats = projSession.GetStatModifiers("surv_proj");
                if (caps.Contains("capability_low_light_vision") && caps.Contains("capability_dense_skeleton")
                    && stats.TryGetValue("blunt_resistance", out float blunt) && Math.Abs(blunt - 0.20f) < 0.001f)
                {
                    Console.WriteLine("[PASS] Check 9: Capability tags and stat modifiers projected correctly from active mutations.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Capability/stat projection failed.");
                }

                // Check 10: Social stigma penalty from visible tags
                var vis = projSession.GetVisibleTags("surv_proj");
                float stigma = projSession.CalculateSocialStigmaPenalty("surv_proj");
                // 2 visible tags: eye_amber_glow, heavy_brow -> 2 * 0.05 = 0.10
                if (vis.Count == 2 && Math.Abs(stigma - 0.10f) < 0.001f)
                {
                    Console.WriteLine($"[PASS] Check 10: Social stigma penalty accurately computed ({stigma:F2} from {vis.Count} visible tags).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Social stigma calculation mismatch (got {stigma}, count={vis.Count}).");
                }

                // Check 11: Clinical gene therapy with retroviral vial consumption
                var therapyInv = new Ashfall.Core.Inventory.Inventory();
                therapyInv.Add(new Ashfall.Core.Inventory.ItemDefinition { id = "gene_therapy_retroviral_vial", displayName = "Retroviral Vial" }, 1);
                var therapySession = RadiationMutationHostSession.Create(dataRoot, new SeededRng(1), therapyInv);
                var therapyProf = therapySession.System.EnsureProfile("surv_cure");
                therapyProf.activeMutationIds.Add("mutation_dense_bone");
                therapyProf.geneticInstability = 30.0f;

                var therapyRes = therapySession.PerformGeneTherapy("surv_cure", "mutation_dense_bone", 3);
                if (therapyRes.Success && !therapyProf.activeMutationIds.Contains("mutation_dense_bone")
                    && therapyInv.CountById("gene_therapy_retroviral_vial") == 0
                    && Math.Abs(therapyProf.geneticInstability - 15.0f) < 0.001f)
                {
                    Console.WriteLine("[PASS] Check 11: Clinical gene therapy succeeded: vial consumed, mutation excised, instability reduced by 15.0%.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: Gene therapy failed (success={therapyRes.Success}, code={therapyRes.FailureCode}).");
                }

                // Check 12: SaveStore round-trip capture/restore parity
                var savedState = therapySession.CaptureSave();
                var freshSession = RadiationMutationHostSession.Create(dataRoot, new SeededRng(1), new Ashfall.Core.Inventory.Inventory());
                freshSession.RestoreSave(savedState);
                var restoredProf = freshSession.GetProfile("surv_cure");
                if (restoredProf != null && restoredProf.geneTherapiesReceived == 1
                    && freshSession.Census.TotalGeneTherapiesReceived == 1
                    && Math.Abs(restoredProf.geneticInstability - 15.0f) < 0.001f)
                {
                    Console.WriteLine("[PASS] Check 12: Save store round-trip capture and restore preserved all genetic profiles and totals.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Save store restore round-trip mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Radiation Mutation SelfTest encountered unexpected exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine($"=== Radiation Mutation Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
