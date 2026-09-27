// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : TradeRouteRiskSelfTest (ORPHAN-SEAL A.04)
// Subsystem          : Trade route transit risk binding
// Core               : Ashfall.Core.Economy.TradeRouteRiskBindingEngine over
//                      the wired TradeRouteHostSession contracts
// Contract           : deterministic risk evaluation over committed contracts,
//                      escort mitigation, alert thresholds, crosswalk mapping,
//                      and null-contract refusal.
// ============================================================================

using System;
using System.Linq;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliTradeRouteRisk
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Trade-Route Risk Binding Self-Test (A.04) ===");
            int passed = 0;
            const int total = 10;

            try
            {
                // Check 1: null contract/routes refuse cleanly (engine contract).
                var noContract = TradeRouteRiskBindingEngine.EvaluateTransitRisk(null!, null!, 500, 500);
                if (!noContract.IsTransitViable && noContract.RaidProbabilityPermille == 1000)
                {
                    Console.WriteLine("[PASS] Check 1: invalid bindings read non-viable at maximum risk.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: null binding did not fail closed."); }

                // Check 2: sessions load the authored caravan catalog.
                var session = TradeRouteHostSession.Create(dataDir ?? ".");
                int routeCount = session.AvailableRoutes.Count;
                if (routeCount >= 3)
                {
                    Console.WriteLine($"[PASS] Check 2: authored caravan routes load ({routeCount}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: only {routeCount} routes loaded."); }

                // Check 3: risk evaluation for a committed contract without
                // registration reads null (no shadow routes).
                var unregistered = session.EvaluateContractRisk("route_not_registered", 500, 500);
                if (unregistered == null)
                {
                    Console.WriteLine("[PASS] Check 3: unregistered routes refuse evaluation (no shadow table).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 3: unregistered route evaluated."); }

                // Check 4: register a contract from an authored route, then
                // evaluate with strong escorts → viable transit.
                var routeId = session.AvailableRoutes[0].route_id;
                var contract = TradeRouteContractFactory(session, routeId);
                bool registered = session.System.RegisterContract(contract);
                if (!registered)
                {
                    Console.WriteLine("[FAIL] Check 4: contract registration refused.");
                }
                else
                {
                    var strong = session.EvaluateContractRisk(routeId, escortStrengthPermille: 900, regionalHostilityPermille: 200);
                    if (strong is { } strongVal && strongVal.IsTransitViable && strongVal.RaidProbabilityPermille < 300)
                    {
                        Console.WriteLine($"[PASS] Check 4: strong escorts keep transit viable (raid {strongVal.RaidProbabilityPermille}/1000).");
                        passed++;
                    }
                    else { Console.WriteLine($"[FAIL] Check 4: strong-escort transit not viable ({strong?.RaidProbabilityPermille})."); }
                }

                // Check 5: weak escorts + high hostility flip the disruption alert.
                var risky = session.EvaluateContractRisk(routeId, escortStrengthPermille: 100, regionalHostilityPermille: 900);
                if (risky is { } riskyVal && riskyVal.TriggersDisruptionAlert && riskyVal.RaidProbabilityPermille > 300)
                {
                    Console.WriteLine($"[PASS] Check 5: weak escorts in hostile territory raise the alert (raid {riskyVal.RaidProbabilityPermille}/1000).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: hostile corridor not flagged ({risky?.RaidProbabilityPermille})."); }

                // Check 6: escort mitigation strictly reduces raid probability.
                var mid = session.EvaluateContractRisk(routeId, 500, 500);
                var none = session.EvaluateContractRisk(routeId, 0, 500);
                var max = session.EvaluateContractRisk(routeId, 1000, 500);
                if (max is { } maxVal && mid is { } midVal && none is { } noneVal
                    && maxVal.RaidProbabilityPermille < midVal.RaidProbabilityPermille
                    && midVal.RaidProbabilityPermille < noneVal.RaidProbabilityPermille)
                {
                    Console.WriteLine($"[PASS] Check 6: escort scaling is monotone ({noneVal.RaidProbabilityPermille} → {midVal.RaidProbabilityPermille} → {maxVal.RaidProbabilityPermille}).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: escort mitigation not monotone."); }

                // Check 7: attrition is capped at 50%.
                var worst = session.EvaluateContractRisk(routeId, 0, 1000);
                if (worst is { } worstVal && worstVal.ExpectedCargoAttritionPermille <= 500)
                {
                    Console.WriteLine($"[PASS] Check 7: attrition caps at {worstVal.ExpectedCargoAttritionPermille}/1000 (≤50%).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: attrition uncapped ({worst?.ExpectedCargoAttritionPermille})."); }

                // Check 8: summary strings are truthful by band.
                var secure = session.EvaluateContractRisk(routeId, 1000, 0);
                var danger = session.EvaluateContractRisk(routeId, 0, 1000);
                if (secure is { } secureVal && danger is { } dangerVal
                    && secureVal.RiskSummary.Contains("Secure")
                    && dangerVal.RiskSummary.Contains("Critical"))
                {
                    Console.WriteLine("[PASS] Check 8: risk summaries match the band thresholds.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: summaries wrong ('{secure?.RiskSummary}' / '{danger?.RiskSummary}')."); }

                // Check 9: escort mitigation ceiling is 75%.
                var rawHighR = session.EvaluateContractRisk(routeId, 0, 1000);
                var mitigatedR = session.EvaluateContractRisk(routeId, 1000, 1000);
                if (rawHighR is not { } rawHighV || mitigatedR is not { } mitigatedV)
                { Console.WriteLine("[FAIL] Check 9: evaluation returned null."); }
                else
                {
                int rawHigh = rawHighV.RaidProbabilityPermille;
                int mitigated = mitigatedV.RaidProbabilityPermille;
                if (mitigated >= rawHigh / 4) // authored ceiling: mitigation ≤75%; floor 10 permille may bind at low raw values
                {
                    Console.WriteLine($"[PASS] Check 9: mitigation respects the 75% ceiling ({rawHigh} → {mitigated}).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 9: mitigation exceeded the authored ceiling."); }
                }

                // Check 10: the projection is derived — evaluating repeatedly
                // does not mutate the contract or census.
                int runsBefore = session.System.GetContract(routeId)!.RunsCompleted;
                for (int i = 0; i < 5; i++)
                    session.EvaluateContractRisk(routeId, 500, 500);
                int runsAfter = session.System.GetContract(routeId)!.RunsCompleted;
                if (runsBefore == runsAfter)
                {
                    Console.WriteLine("[PASS] Check 10: risk projection is side-effect free.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 10: projection mutated the contract ledger."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Trade-route risk: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }

        /// <summary>Builds a Tier-2 contract from an authored route (deterministic).</summary>
        private static TradeRouteContract TradeRouteContractFactory(TradeRouteHostSession session, string routeId)
        {
            var definition = session.AvailableRoutes[0];
            var save = new TradeRouteContractSaveState
            {
                RouteId = routeId,
                CounterpartyId = definition.faction_id,
                CadenceDays = definition.arrival_interval_days,
                BaseTariffChits = 20,
                CaravanSlotsRequired = 1,
                EstablishedDay = 100,
                ReliabilityScore = TradeRouteContract.Tier2Threshold + 1,
                RunsCompleted = 6,
                RunsFailed = 0,
                NextRunDay = 120
            };
            return TradeRouteContract.RestoreState(save);
        }
    }
}
