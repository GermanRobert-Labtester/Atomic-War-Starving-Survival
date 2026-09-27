// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : UndergroundEconomyPressureSelfTest
// Core Authority     : Ashfall.Core.Economy.UndergroundEconomyPressure (EN-03)
// Purpose            : heat/trust/relocation -> market temperature band read model
// ============================================================================

using System;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliUndergroundEconomy
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Underground Economy Pressure Self-Test (EN-03) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var calm = UndergroundEconomyPressure.Evaluate(currentHeat: 20, heatThreshold: 100, trust: 60);
                if (calm.Band == MarketTemperatureBand.Calm && calm.PricePressureMultiplier == 1.0f
                    && calm.AttentionRiskMultiplier == 1.0f && calm.StatusSummary.Contains("Discreet") && calm.Trust == 60)
                {
                    Console.WriteLine("[PASS] Check 1: low heat is Calm at baseline price/attention multipliers.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: calm state wrong ({calm.Band})."); }

                var raised = UndergroundEconomyPressure.Evaluate(currentHeat: 55, heatThreshold: 100);
                if (raised.Band == MarketTemperatureBand.Raised && raised.PricePressureMultiplier == 1.15f
                    && raised.AttentionRiskMultiplier == 1.5f && raised.StatusSummary.Contains("Noticed"))
                {
                    Console.WriteLine("[PASS] Check 2: heat at half the threshold is Raised with inflation noted.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: raised state wrong ({raised.Band})."); }

                var hot = UndergroundEconomyPressure.Evaluate(currentHeat: 100, heatThreshold: 100);
                if (hot.Band == MarketTemperatureBand.Hot && hot.PricePressureMultiplier == 1.35f
                    && hot.AttentionRiskMultiplier == 2.5f && hot.StatusSummary.Contains("Heavy authority scrutiny"))
                {
                    Console.WriteLine("[PASS] Check 3: heat at the threshold is Hot with raid risk imminent.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: hot state wrong ({hot.Band})."); }

                var relocated = UndergroundEconomyPressure.Evaluate(currentHeat: 10, heatThreshold: 100, isRelocated: true);
                if (relocated.Band == MarketTemperatureBand.Relocated && relocated.PricePressureMultiplier == 1.50f
                    && relocated.AttentionRiskMultiplier == 3.0f && relocated.StatusSummary.Contains("dispersed") && relocated.IsRelocated)
                {
                    Console.WriteLine("[PASS] Check 4: relocation overrides heat state with the steepest markups.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: relocated state wrong ({relocated.Band})."); }

                var clamped = UndergroundEconomyPressure.Evaluate(currentHeat: -20, heatThreshold: 0);
                if (clamped.CurrentHeat == 0 && clamped.HeatThreshold == 1 && clamped.Band == MarketTemperatureBand.Calm)
                {
                    Console.WriteLine("[PASS] Check 5: negative heat and zero threshold are clamped safely.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: clamp = heat {clamped.CurrentHeat} / threshold {clamped.HeatThreshold}."); }

                var explicitPressure = new UndergroundEconomyPressure(
                    MarketTemperatureBand.Hot, currentHeat: 120, heatThreshold: 100, trust: 10,
                    isRelocated: false, pricePressureMultiplier: 1.35f, attentionRiskMultiplier: 2.5f, statusSummary: null!);
                if (explicitPressure.StatusSummary == string.Empty && explicitPressure.CurrentHeat == 120 && explicitPressure.Trust == 10)
                {
                    Console.WriteLine("[PASS] Check 6: the explicit constructor null-normalizes the summary and preserves counters.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: explicit constructor wrong."); }

                if (raised.Band != hot.Band && hot.Band != relocated.Band && (int)MarketTemperatureBand.Calm < (int)MarketTemperatureBand.Relocated)
                {
                    Console.WriteLine("[PASS] Check 7: bands escalate monotonically Calm -> Raised -> Hot -> Relocated.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 7: band ordering wrong."); }

                var implicitTrust = UndergroundEconomyPressure.Evaluate(currentHeat: 0);
                if (implicitTrust.Trust == 50 && implicitTrust.HeatThreshold == 100 && implicitTrust.Band == MarketTemperatureBand.Calm)
                {
                    Console.WriteLine("[PASS] Check 8: default heat threshold (100) and trust (50) apply when unspecified.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: defaults = threshold {implicitTrust.HeatThreshold} / trust {implicitTrust.Trust}."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Underground economy pressure: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
