// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ResourceMassBalanceSelfTest
// Core Authority     : Ashfall.Core.Balance.ResourceMassBalanceSimulator
// Purpose            : deterministic 30-day survival-loop mass-balance gate
// ============================================================================

using System;
using Ashfall.Core.Balance;

namespace AtomicWar.GodotApp
{
    public static class HostCliMassBalance
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Resource Mass Balance Self-Test (release-craft balance gate) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var config = new ResourceMassBalanceConfig
                {
                    Seed = 42,
                    Days = 30,
                    CrewSize = 4,
                    ScenarioName = "Baseline"
                };
                var first = ResourceMassBalanceSimulator.Run(config);
                var second = ResourceMassBalanceSimulator.Run(config);

                if (first != null && second != null)
                {
                    Console.WriteLine("[PASS] Check 1: baseline 30-day run completes.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: simulator returned null."); }

                if (first is { Success: true })
                {
                    Console.WriteLine("[PASS] Check 2: baseline invariants hold (Success = true).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: baseline failed ({string.Join("; ", first?.InvariantViolations ?? new())})."); }

                if (first is { } alive && alive.SurvivorsAlive == config.CrewSize)
                {
                    Console.WriteLine($"[PASS] Check 3: all {config.CrewSize} crew survive the baseline scenario.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: survivors alive = {first?.SurvivorsAlive}."); }

                if (first is { } tele && tele.Telemetry.Count == config.Days)
                {
                    Console.WriteLine($"[PASS] Check 4: telemetry covers every simulated day ({config.Days}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: telemetry days = {first?.Telemetry.Count}."); }

                if (first is { } water && water.MaxWaterDiscrepancy < 0.05)
                {
                    Console.WriteLine($"[PASS] Check 5: water mass balance holds (max discrepancy {water.MaxWaterDiscrepancy:F4} L < 0.05).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: water discrepancy = {first?.MaxWaterDiscrepancy}."); }

                if (first is { } a && second is { } b
                    && Math.Abs(a.FinalWaterStored - b.FinalWaterStored) < 0.0001
                    && Math.Abs(a.AvgSurvivorHealth - b.AvgSurvivorHealth) < 0.0001
                    && a.TotalMealsServed == b.TotalMealsServed)
                {
                    Console.WriteLine("[PASS] Check 6: same-seed runs are deterministic (water/health/meals identical).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: same-seed runs diverged."); }

                var alternate = ResourceMassBalanceSimulator.Run(new ResourceMassBalanceConfig
                {
                    Seed = 43,
                    Days = 30,
                    CrewSize = 4,
                    ScenarioName = "Baseline"
                });
                if (alternate is { } alt && alt.Success && alt.Telemetry.Count == 30)
                {
                    Console.WriteLine($"[PASS] Check 7: alternate seed 43 is a valid, separate trajectory (meals {alt.TotalMealsServed}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: alternate seed run invalid ({string.Join("; ", alternate?.InvariantViolations ?? new())})."); }

                if (first is { } clean && clean.InvariantViolations.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 8: no invariant violations in the baseline run.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: violations = {string.Join("; ", first?.InvariantViolations ?? new())}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Resource mass balance: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
