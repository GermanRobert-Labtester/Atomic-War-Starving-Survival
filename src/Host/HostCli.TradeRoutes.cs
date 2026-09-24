// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 192 (Trade Route Contracts & Reliability Tiers).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliTradeRoutes
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Trade Route Contracts Self-Test (Plan 192) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var host = TradeRouteHostSession.Create(dataDir);
                if (host.AvailableRoutes.Count >= 8)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({host.AvailableRoutes.Count} routes).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Route catalog count mismatch: {host.AvailableRoutes.Count}.");
                }

                // Check 2: Establishing contract
                bool established = host.EstablishContract(
                    routeId: "route_test_alpha",
                    counterpartyId: "faction_the_compact",
                    cadenceDays: 4,
                    baseTariffChits: 100,
                    caravanSlotsRequired: 2,
                    exclusiveGoodId: "item_high_grade_fuel",
                    establishedDay: 1,
                    goodsOut: new[] { new TradeRouteGoodLeg { ItemId = "clean_water", UnitsPerRun = 10 } },
                    goodsIn: new[] { new TradeRouteGoodLeg { ItemId = "ammo_556", UnitsPerRun = 20 } });

                if (established && host.Contracts.Count == 1)
                {
                    GD.Print("[PASS] Check 2: Successfully established contract 'route_test_alpha'.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: Failed to establish contract.");
                }

                // Check 3: Cadence and initial state
                var contract = host.GetContract("route_test_alpha");
                if (contract != null && contract.CadenceDays == 4 && contract.NextRunDay == 5 &&
                    contract.Tier == TradeRouteReliabilityTier.Tier1 && !contract.IsExclusiveGoodUnlocked)
                {
                    GD.Print("[PASS] Check 3: Initial contract state and cadence verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Contract initial state mismatch.");
                }

                // Check 4: Run outcome recording & score increase
                contract!.RecordRunOutcome(TradeRouteRunOutcome.OnTime, currentDay: 5);
                if (contract.ReliabilityScore == 1 && contract.RunsCompleted == 1 && contract.NextRunDay == 9)
                {
                    GD.Print("[PASS] Check 4: Run outcome OnTime recorded score 1 and next run day 9.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Run outcome recording failed.");
                }

                // Check 5: Progression to Tier 2 (threshold: 5)
                for (int i = 2; i <= 5; i++)
                {
                    contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, currentDay: 5 + i * 4);
                }
                if (contract.ReliabilityScore == 5 && contract.Tier == TradeRouteReliabilityTier.Tier2)
                {
                    GD.Print("[PASS] Check 5: Progression to Tier 2 verified (score 5).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Expected Tier 2, got {contract.Tier} (score {contract.ReliabilityScore}).");
                }

                // Check 6: Progression to Tier 3 (threshold: 12) & 25% tariff discount
                for (int i = 6; i <= 12; i++)
                {
                    contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, currentDay: 5 + i * 4);
                }
                if (contract.ReliabilityScore == 12 && contract.Tier == TradeRouteReliabilityTier.Tier3 &&
                    contract.EffectiveTariffChits == 75) // 100 * 0.75 = 75
                {
                    GD.Print("[PASS] Check 6: Progression to Tier 3 and 25% tariff discount (75 chits) verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Tier 3 tariff discount mismatch (effective: {contract.EffectiveTariffChits}).");
                }

                // Check 7: Progression to Tier 4 (threshold: 20) & exclusive good unlock
                for (int i = 13; i <= 20; i++)
                {
                    contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, currentDay: 5 + i * 4);
                }
                if (contract.ReliabilityScore == 20 && contract.Tier == TradeRouteReliabilityTier.Tier4 &&
                    contract.IsExclusiveGoodUnlocked)
                {
                    GD.Print("[PASS] Check 7: Progression to Tier 4 and exclusive good unlock verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Tier 4 exclusive good unlock failed.");
                }

                // Check 8: Suspension and resumption
                host.SuspendContract("route_test_alpha");
                bool canRunSuspended = contract.CanScheduleRun(contract.NextRunDay);
                host.ResumeContract("route_test_alpha");
                bool canRunResumed = contract.CanScheduleRun(contract.NextRunDay);
                if (!canRunSuspended && canRunResumed)
                {
                    GD.Print("[PASS] Check 8: Contract suspension and resumption verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Suspension or resumption check failed.");
                }

                // Check 9: Cancellation and 30-day cooldown
                host.CancelContract("route_test_alpha", currentDay: 100);
                bool cooldownActiveDay110 = contract.IsOnCooldown(110);
                bool cooldownEndedDay131 = contract.IsOnCooldown(131);
                if (contract.IsSuspended && contract.ReliabilityScore == 0 && cooldownActiveDay110 && !cooldownEndedDay131)
                {
                    GD.Print("[PASS] Check 9: Cancellation reset reliability to 0 and enforced 30-day cooldown.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: Cancellation cooldown or reliability reset failed.");
                }

                // Check 10: FundsLedger integration on daily tick
                var funds = new FundsLedger(initialBalance: 200);
                var activeContract = new TradeRouteContract("route_tick_test", "faction_the_scale", 1, 50, establishedDay: 1);
                host.RegisterContract(activeContract);
                // Next run day is 2. On day 2, tick should pay 50 chits and complete run OnTime
                host.TickDay(currentDay: 2, funds);
                if (funds.Balance == 150 && activeContract.RunsCompleted == 1 && activeContract.ReliabilityScore == 1)
                {
                    GD.Print("[PASS] Check 10: FundsLedger tariff payment during daily tick verified (balance: 150 chits).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: FundsLedger tariff payment failed (balance: {funds.Balance}).");
                }

                // Check 11: State capture and restore round-trip
                var snapshot = host.CaptureState();
                var freshHost = TradeRouteHostSession.Create(dataDir);
                freshHost.RestoreState(snapshot);
                var restoredTickRoute = freshHost.GetContract("route_tick_test");
                if (freshHost.Contracts.Count == 2 && restoredTickRoute != null && restoredTickRoute.RunsCompleted == 1)
                {
                    GD.Print("[PASS] Check 11: State capture and restore round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: State restore mismatch.");
                }

                // Check 12: Checksummed TradeRouteSaveStore bare round-trip
                string persistedJson = TradeRouteSaveStore.TryCapturePersisted(snapshot);
                var restoredFromBare = TradeRouteSaveStore.TryRestorePersisted(persistedJson);
                if (restoredFromBare != null && restoredFromBare.Contracts.Count == 2)
                {
                    GD.Print("[PASS] Check 12: Checksummed TradeRouteSaveStore round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: TradeRouteSaveStore bare round-trip failed.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in TradeRoute self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Trade Route Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
