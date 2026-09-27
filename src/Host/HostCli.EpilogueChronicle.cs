// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : EpilogueChronicleSelfTest
// Core Authority     : Ashfall.Core.Endgame.EpilogueChronicleBuilder
// Purpose            : deterministic ordering of ending slides, fate cards, metrics
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public static class HostCliEpilogueChronicle
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Epilogue Chronicle Builder Self-Test ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var builder = new EpilogueChronicleBuilder();

                var knowing = builder.Build(new EpilogueChronicleInput
                {
                    EndingKey = "knowing",
                    Day = 365,
                    BuildSeed = 7,
                    Slides = new List<EpilogueSlide>(),
                    FateCards = new List<SurvivorFateCard>(),
                    Metrics = new List<EpilogueMetric>()
                });
                if (knowing.Title == "Knowing" && knowing.EndingKey == "knowing"
                    && knowing.GeneratedDay == 365 && knowing.BuildSeed == 7)
                {
                    Console.WriteLine("[PASS] Check 1: a known ending maps to its title and carries day/seed.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: title = {knowing.Title}."); }

                var slides = builder.Build(new EpilogueChronicleInput
                {
                    EndingKey = "knowing",
                    Day = 100,
                    BuildSeed = 1,
                    Slides = new List<EpilogueSlide>
                    {
                        new EpilogueSlide(2, "Second", "..."),
                        new EpilogueSlide(0, "First", "..."),
                        new EpilogueSlide(1, "Middle", "...")
                    },
                    FateCards = new List<SurvivorFateCard>(),
                    Metrics = new List<EpilogueMetric>()
                });
                if (slides.Slides.Count == 3 && slides.Slides[0].Title == "First"
                    && slides.Slides[1].Title == "Middle" && slides.Slides[2].Title == "Second")
                {
                    Console.WriteLine("[PASS] Check 2: slides are ordered by slide index.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 2: slide order wrong."); }

                var fates = builder.Build(new EpilogueChronicleInput
                {
                    EndingKey = "knowing",
                    Day = 100,
                    BuildSeed = 1,
                    Slides = new List<EpilogueSlide>(),
                    FateCards = new List<SurvivorFateCard>
                    {
                        new SurvivorFateCard { SurvivorId = "zulu", DisplayName = "Zulu", Fate = "Survived", Survived = true },
                        new SurvivorFateCard { SurvivorId = "alpha", DisplayName = "Alpha", Fate = "Died", Survived = false }
                    },
                    Metrics = new List<EpilogueMetric>()
                });
                if (fates.FateCards.Count == 2 && fates.FateCards[0].SurvivorId == "alpha" && fates.FateCards[1].SurvivorId == "zulu")
                {
                    Console.WriteLine("[PASS] Check 3: fate cards are ordered by survivor id.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 3: fate card order wrong."); }

                var metrics = builder.Build(new EpilogueChronicleInput
                {
                    EndingKey = "knowing",
                    Day = 100,
                    BuildSeed = 1,
                    Slides = new List<EpilogueSlide>(),
                    FateCards = new List<SurvivorFateCard>(),
                    Metrics = new List<EpilogueMetric>
                    {
                        new EpilogueMetric("total_deaths", 5, "Deaths"),
                        new EpilogueMetric("days_survived", 365, "Days"),
                        new EpilogueMetric("morale_final", 75, "Morale")
                    }
                });
                if (metrics.Metrics.Count == 3 && metrics.Metrics[0].MetricId == "days_survived"
                    && metrics.Metrics[1].MetricId == "morale_final" && metrics.Metrics[2].MetricId == "total_deaths")
                {
                    Console.WriteLine("[PASS] Check 4: metrics are ordered by metric id.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 4: metric order wrong."); }

                var input = new EpilogueChronicleInput
                {
                    EndingKey = "culpable",
                    Day = 211,
                    BuildSeed = 99,
                    Slides = new List<EpilogueSlide> { new EpilogueSlide(1, "Slide A", "Prose A"), new EpilogueSlide(0, "Slide B", "Prose B") },
                    FateCards = new List<SurvivorFateCard> { new SurvivorFateCard { SurvivorId = "s2", DisplayName = "S2" }, new SurvivorFateCard { SurvivorId = "s1", DisplayName = "S1" } },
                    Metrics = new List<EpilogueMetric> { new EpilogueMetric("m1", 1f, "M1") }
                };
                var first = builder.Build(input);
                var second = builder.Build(input);
                if (first.Title == second.Title && first.Slides[0].Title == second.Slides[0].Title
                    && first.FateCards[0].SurvivorId == second.FateCards[0].SurvivorId
                    && first.Metrics[0].MetricId == second.Metrics[0].MetricId)
                {
                    Console.WriteLine("[PASS] Check 5: identical inputs build an identical chronicle (deterministic).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 5: determinism violated."); }

                var empty = builder.Build(new EpilogueChronicleInput { EndingKey = "" });
                if (empty.Title == "UNKNOWN ENDING" && empty.Slides.Count == 0 && empty.FateCards.Count == 0 && empty.Metrics.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 6: an empty chronicle renders UNKNOWN ENDING with empty collections.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: empty title = {empty.Title}."); }

                var novel = builder.Build(new EpilogueChronicleInput { EndingKey = "novel_ending" });
                if (novel.Title == "novel_ending")
                {
                    Console.WriteLine("[PASS] Check 7: an unknown ending falls back to its key rather than inventing a title.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: novel title = {novel.Title}."); }

                bool nullRefused = false;
                try { _ = builder.Build(null!); }
                catch (ArgumentNullException) { nullRefused = true; }
                if (nullRefused)
                {
                    Console.WriteLine("[PASS] Check 8: a null input is refused.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 8: null input accepted."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Epilogue chronicle: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
