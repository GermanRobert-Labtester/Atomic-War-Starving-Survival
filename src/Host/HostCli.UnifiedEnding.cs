// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 145 (Unified Ending Resolution & Epilogue Personalization).

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public static class HostCliUnifiedEnding
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Unified Ending Resolution Self-Test (Plan 145) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var host = UnifiedEndingHostSession.Create(dataDir);
                var census = host.Census;
                if (census.DurationTemplatesCount >= 3 && census.PoliticalTemplatesCount >= 5 && census.SocialTemplatesCount >= 5)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully (dur={census.DurationTemplatesCount}, pol={census.PoliticalTemplatesCount}, soc={census.SocialTemplatesCount}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Catalog templates missing or incomplete. Dur={census.DurationTemplatesCount}, Pol={census.PoliticalTemplatesCount}.");
                }

                // Check 2: Short duration resolution (< 180 days)
                var shortCtx = new UnifiedEndingContext { totalDaysSurvived = 90 };
                var shortRes = host.Resolve(shortCtx);
                if (shortRes.campaignDurationLabel == "Short" && shortRes.durationProse.Contains("90"))
                {
                    GD.Print("[PASS] Check 2: Short duration (< 180 days) resolved correctly.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Short duration failed. Label={shortRes.campaignDurationLabel}.");
                }

                // Check 3: Medium duration resolution (180 - 365 days)
                var medCtx = new UnifiedEndingContext { totalDaysSurvived = 250 };
                var medRes = host.Resolve(medCtx);
                if (medRes.campaignDurationLabel == "Medium" && medRes.durationProse.Contains("250"))
                {
                    GD.Print("[PASS] Check 3: Medium duration (180 - 365 days) resolved correctly.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Medium duration failed. Label={medRes.campaignDurationLabel}.");
                }

                // Check 4: Long duration resolution (> 365 days)
                var longCtx = new UnifiedEndingContext { totalDaysSurvived = 500 };
                var longRes = host.Resolve(longCtx);
                if (longRes.campaignDurationLabel == "Long" && longRes.durationProse.Contains("500"))
                {
                    GD.Print("[PASS] Check 4: Long duration (> 365 days) resolved correctly.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Long duration failed. Label={longRes.campaignDurationLabel}.");
                }

                // Check 5: Holdfast ending priority & political resolution
                var polCtx = new UnifiedEndingContext
                {
                    totalDaysSurvived = 200,
                    factionBranchId = "Military",
                    holdfastEndingId = HoldfastEndings.Schedule
                };
                var polRes = host.Resolve(polCtx);
                if (polRes.politicalOutcome == "The Schedule Holds" && polRes.politicalProse.Contains("iron discipline"))
                {
                    GD.Print("[PASS] Check 5: Military + Schedule ending resolved to 'The Schedule Holds' with iron discipline prose.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Political resolution failed. Outcome={polRes.politicalOutcome}.");
                }

                // Check 6: Faction branch fallback without Holdfast ending
                var rebelCtx = new UnifiedEndingContext
                {
                    totalDaysSurvived = 200,
                    factionBranchId = "Rebel"
                };
                var rebelRes = host.Resolve(rebelCtx);
                if (rebelRes.politicalOutcome == "Free Commonwealth")
                {
                    GD.Print("[PASS] Check 6: Rebel branch fallback resolved to 'Free Commonwealth'.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Faction branch fallback failed. Outcome={rebelRes.politicalOutcome}.");
                }

                // Check 7: Muster approach social outcome
                var socCtx = new UnifiedEndingContext
                {
                    totalDaysSurvived = 200,
                    musterApproachId = "the_open_muster"
                };
                var socRes = host.Resolve(socCtx);
                if (socRes.socialOutcome == "The Open Muster" && socRes.socialProse.Contains("commonwealth"))
                {
                    GD.Print("[PASS] Check 7: Muster approach 'the_open_muster' resolved to 'The Open Muster'.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Social resolution failed. Outcome={socRes.socialOutcome}.");
                }

                // Check 8: Moral choice band resolution
                var moralCtx = new UnifiedEndingContext
                {
                    totalDaysSurvived = 200,
                    moralChoiceBand = "VeryPositive"
                };
                var moralRes = host.Resolve(moralCtx);
                if (moralRes.moralOutcome == "VeryPositive" && moralRes.moralProse.Contains("compassion"))
                {
                    GD.Print("[PASS] Check 8: Moral band 'VeryPositive' resolved compassion prose.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Moral resolution failed. Prose={moralRes.moralProse}.");
                }

                // Check 9: Key survivor personal epilogues
                var survCtx = new UnifiedEndingContext
                {
                    totalDaysSurvived = 200,
                    keySurvivorFates = new List<SurvivorEpilogueFate>
                    {
                        new() { survivorId = "s1", survivorName = "Elena", status = SurvivorFateStatus.Alive, notableTrait = "social" },
                        new() { survivorId = "s2", survivorName = "Marcus", status = SurvivorFateStatus.Deceased, notableTrait = "brave" },
                        new() { survivorId = "s3", survivorName = "Old Joe", status = SurvivorFateStatus.Retired, notableTrait = "stoic" }
                    }
                };
                var survRes = host.Resolve(survCtx);
                if (survRes.survivorEpilogues.Count == 3 &&
                    survRes.survivorEpilogues[0].epilogueText.Contains("Elena") &&
                    survRes.survivorEpilogues[1].epilogueText.Contains("memorial") &&
                    survRes.survivorEpilogues[2].epilogueText.Contains("elder"))
                {
                    GD.Print("[PASS] Check 9: Personal survivor epilogues generated for living, deceased, and retired survivors.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Survivor epilogues generation failed. Count={survRes.survivorEpilogues.Count}.");
                }

                // Check 10: Legacy traits awarded from campaign achievements
                var legCtx = new UnifiedEndingContext
                {
                    totalDaysSurvived = 300,
                    grandTreatySigned = true,
                    livingDwellerCount = 15,
                    tempestDecommissioned = true,
                    moralChoiceBand = "Positive"
                };
                var legRes = host.Resolve(legCtx);
                if (legRes.legacyTraitsAwarded.Contains("legacy_trait_diplomat") &&
                    legRes.legacyTraitsAwarded.Contains("legacy_trait_prosperous_haven") &&
                    legRes.legacyTraitsAwarded.Contains("legacy_trait_storm_breaker") &&
                    legRes.legacyTraitsAwarded.Contains("legacy_trait_humanitarian"))
                {
                    GD.Print("[PASS] Check 10: Legacy traits awarded from campaign achievements (diplomat, prosperous_haven, storm_breaker, humanitarian).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Legacy traits awarding failed. Awarded: {string.Join(", ", legRes.legacyTraitsAwarded)}");
                }

                // Check 11: State capture / restore round-trip
                var captured = host.CaptureState();
                var freshHost = UnifiedEndingHostSession.Create(dataDir);
                freshHost.RestoreState(captured);
                if (freshHost.IsResolved && freshHost.LastResult != null &&
                    freshHost.LastResult.overallTitle == host.LastResult?.overallTitle)
                {
                    GD.Print("[PASS] Check 11: Unified ending state captures and restores faithfully.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: State capture/restore failed.");
                }

                // Check 12: Deterministic resolution fingerprint
                string fp1 = UnifiedEndingResolver.ComputeContextFingerprint(legCtx);
                string fp2 = UnifiedEndingResolver.ComputeContextFingerprint(legCtx);
                legCtx.totalDaysSurvived = 301;
                string fp3 = UnifiedEndingResolver.ComputeContextFingerprint(legCtx);

                if (fp1 == fp2 && fp1 != fp3)
                {
                    GD.Print("[PASS] Check 12: Deterministic fingerprint matches for identical contexts and differs when state changes.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Fingerprint determinism failed. fp1={fp1}, fp2={fp2}, fp3={fp3}.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ERROR] Unified Ending Self-Test threw exception: {ex}");
            }

            GD.Print($"=== Unified Ending Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
