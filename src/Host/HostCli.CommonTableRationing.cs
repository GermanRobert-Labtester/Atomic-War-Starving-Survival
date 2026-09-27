// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : CommonTableRationingSelfTest
// Subsystem          : Common Table Nutrition (Expansion 26)
// ============================================================================

using System;
using Ashfall.Core.Nutrition;

namespace AtomicWar.GodotApp
{
    public static class HostCliCommonTableRationing
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Common Table Rationing Self-Test ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var session = CommonTableRationingHostSession.Create();
                session.SetPopulationCount(12);
                session.SetCookSkillPermille(800);

                if (session.SessionCount == 0 && session.ActivePolicy == RationLevel.Standard)
                {
                    Console.WriteLine("[PASS] Check 1: Common-table ledger starts empty at standard policy.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Ledger or initial policy was wrong.");
                }

                // A single food category must be a monoculture with deficiency risk.
                var mono = session.ServeMeal(1, new[] { FoodCategory.PreservedStarch });
                if (mono.DiversityTier == DiversityTier.Monoculture
                    && mono.UniqueCategoryCount == 1
                    && mono.DeficiencyRiskPermille > 0)
                {
                    Console.WriteLine($"[PASS] Check 2: Single category is monoculture with risk {mono.DeficiencyRiskPermille} permille.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: tier {mono.DiversityTier}, risk {mono.DeficiencyRiskPermille}.");
                }

                // Four distinct categories must reach the peak diversity tier and zero deficiency.
                var spectrum = session.ServeMeal(2, new[]
                {
                    FoodCategory.PreservedStarch, FoodCategory.DriedProtein,
                    FoodCategory.ForagedGreens, FoodCategory.FreshProduce
                });
                if (spectrum.DiversityTier == DiversityTier.AbundantSpectrum
                    && spectrum.DeficiencyRiskPermille == 0
                    && spectrum.MoraleDeltaPermille > mono.MoraleDeltaPermille)
                {
                    Console.WriteLine($"[PASS] Check 3: Four categories reach AbundantSpectrum with better morale.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: tier {spectrum.DiversityTier}, risk {spectrum.DeficiencyRiskPermille}.");
                }

                // Starvation policy must cut calories and provoke grievance.
                session.SetRationingPolicy(RationLevel.StarvationEmergency);
                var starved = session.ServeMeal(3, new[] { FoodCategory.PreservedStarch, FoodCategory.DriedProtein });
                if (starved.BaseCaloriesPercent < spectrum.BaseCaloriesPercent
                    && starved.GrievanceProbabilityPermille > 0)
                {
                    Console.WriteLine($"[PASS] Check 4: Starvation ration cuts calories and drives grievance ({starved.GrievanceProbabilityPermille} permille).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: calories {starved.BaseCaloriesPercent}, grievance {starved.GrievanceProbabilityPermille}.");
                }

                // Cook skill must reduce required raw units against an unskilled cook.
                session.SetRationingPolicy(RationLevel.Standard);
                int skilledUnits = session.ServeMeal(4, new[] { FoodCategory.FreshProduce }).RequiredFoodUnits;
                session.SetCookSkillPermille(0);
                int unskilledUnits = session.ServeMeal(5, new[] { FoodCategory.FreshProduce }).RequiredFoodUnits;
                if (skilledUnits < unskilledUnits)
                {
                    Console.WriteLine($"[PASS] Check 5: Cook skill reduces food waste ({unskilledUnits} -> {skilledUnits} units).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: {unskilledUnits} -> {skilledUnits}.");
                }

                // Consecutive lean days must compound the deficiency risk.
                session.SetRationingPolicy(RationLevel.StarvationEmergency);
                var day1 = session.ServeMeal(6, new[] { FoodCategory.SyntheticPaste });
                session.AdvanceDay(7);
                session.AdvanceDay(8);
                var day3 = session.ServeMeal(9, new[] { FoodCategory.SyntheticPaste });
                if (day3.DeficiencyRiskPermille > day1.DeficiencyRiskPermille
                    && session.ConsecutiveLeanDays >= 2)
                {
                    Console.WriteLine($"[PASS] Check 6: Lean streak compounds deficiency risk ({day1.DeficiencyRiskPermille} -> {day3.DeficiencyRiskPermille}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: {day1.DeficiencyRiskPermille} -> {day3.DeficiencyRiskPermille}, streak {session.ConsecutiveLeanDays}.");
                }

                // Save while the lean streak is still active: it must survive restore.
                bool leanSaved = session.TrySave();
                var leanReloaded = CommonTableRationingHostSession.Create();
                bool leanLoaded = leanReloaded.TryLoad();
                if (leanSaved && leanLoaded && leanReloaded.ConsecutiveLeanDays >= 2
                    && leanReloaded.ActivePolicy == RationLevel.StarvationEmergency)
                {
                    Console.WriteLine($"[PASS] Check 7: Lean streak survives save/restore ({leanReloaded.ConsecutiveLeanDays} days).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: leanReloaded={leanReloaded.ConsecutiveLeanDays}, saved={leanSaved}, loaded={leanLoaded}.");
                }

                // Returning to a standard ration legitimately clears the streak.
                session.SetRationingPolicy(RationLevel.Standard);
                session.SetCookSkillPermille(600);
                session.SetRationInequality(true);
                session.ServeMeal(10, new[] { FoodCategory.PreservedStarch, FoodCategory.SyntheticPaste });
                bool saved = session.TrySave();
                var reloaded = CommonTableRationingHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded
                    && reloaded.SessionCount == session.SessionCount
                    && reloaded.ConsecutiveLeanDays == 0
                    && reloaded.HasRationInequality
                    && reloaded.PopulationCount == 12
                    && reloaded.CookSkillPermille == 600
                    && CommonTableRationingSaveStore.SectionName.Equals("common_table_rationing", StringComparison.Ordinal)
                    && CommonTableRationingSaveStore.FileName.Equals("common_table_rationing_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine($"[PASS] Check 8: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: saved={saved}, loaded={loaded}, sessions={reloaded.SessionCount}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Common Table Rationing Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
