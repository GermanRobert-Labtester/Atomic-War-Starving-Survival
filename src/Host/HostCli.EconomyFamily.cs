// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : EconomyFamilySelfTest
// Subsystem          : PLAN-ECONOMY-DATA-FAMILY-TRUTH-270 — Economy family
// ============================================================================
using System;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliEconomyFamily
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Economy Family Self-Test (PLAN-ECONOMY-DATA-FAMILY-TRUTH-270) ===");
            int passed = 0; const int total = 9;
            try
            {
                var session = new EconomyFamilyHostSession();

                // trade-route monopoly
                session.RecordRouteDelivery("route_iron", "good_steel", 12, 1);
                if (session.GetMonopolyPremiumPermille("route_iron") >= 0) { Console.WriteLine("[PASS] Check 1: Route monopoly premium resolves."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: monopoly premium failed.");

                var monopolySave = session.Monopoly.CaptureState();
                var monopolyRestored = new TradeRouteMonopolyEngine();
                monopolyRestored.RestoreState(monopolySave);
                if (monopolyRestored.GetCurrentMonopolyPremiumPermille("route_iron") == session.GetMonopolyPremiumPermille("route_iron"))
                { Console.WriteLine("[PASS] Check 2: Route monopoly capture/restore round-trips."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: monopoly restore mismatch.");

                // contraband quoting (pure)
                var buy = session.QuoteContrabandBuy("item_narcotics", 3, 100, ContrabandClassification.Narcotics, 50);
                if (buy != null && buy.UnitPriceChits > 0) { Console.WriteLine($"[PASS] Check 3: Contraband buy quote ({buy.UnitPriceChits}/unit)."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: contraband buy quote failed.");

                var locked = session.QuoteContrabandBuy("item_narcotics", 3, 100, ContrabandClassification.Narcotics, 900);
                if (locked != null && !locked.IsViable) { Console.WriteLine("[PASS] Check 4: High-heat contraband purchase refused (raid lockout)."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: lockout not enforced.");

                var sell = session.QuoteContrabandSell("item_narcotics", 3, 100, ContrabandClassification.Narcotics, 50);
                if (sell != null && sell.UnitPriceChits > 0 && sell.UnitPriceChits <= buy.UnitPriceChits) { Console.WriteLine("[PASS] Check 5: Contraband sell quote ≤ buy quote."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: sell quote wrong.");

                // chit purity assay (pure, deterministic)
                var assay1 = session.EvaluateChitAssay(100, PurityTier.StandardAlloy, 800, 12345u);
                var assay2 = session.EvaluateChitAssay(100, PurityTier.StandardAlloy, 800, 12345u);
                if (assay1.StatusNotice == assay2.StatusNotice && assay1.AcceptedAmount == assay2.AcceptedAmount)
                { Console.WriteLine("[PASS] Check 6: Chit assay is deterministic for a fixed seed."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: chit assay nondeterministic.");

                var scrap = session.EvaluateChitAssay(100, PurityTier.CounterfeitLead, 900, 7u);
                if (scrap != null) { Console.WriteLine($"[PASS] Check 7: Counterfeit assay evaluated (accepted={scrap.AcceptedAmount}, confiscated={scrap.ConfiscatedAmount})."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: counterfeit assay failed.");

                // syndicate heat attention
                session.AddSyndicateHeat("syn_a", 60, "probe", 1);
                var band = session.EvaluateSyndicateBand("syn_a");
                if (session.Heat.CaptureState() != null && !string.IsNullOrEmpty(band.ToString()))
                { Console.WriteLine($"[PASS] Check 8: Syndicate heat band evaluated ({band})."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: heat band failed.");

                // heat save round-trip
                var save = session.CaptureState();
                var restored = new EconomyFamilyHostSession();
                restored.RestoreState(save);
                if (restored.GetMonopolyPremiumPermille("route_iron") == session.GetMonopolyPremiumPermille("route_iron"))
                { Console.WriteLine("[PASS] Check 9: Composite economy-family save round-trips."); passed++; }
                else Console.WriteLine("[FAIL] Check 9: composite restore mismatch.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Economy Family Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
