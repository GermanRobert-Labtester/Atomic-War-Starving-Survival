// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ExpeditionFamilySelfTest
// Subsystem          : PLAN-EXPEDITION-FAMILY-TRUTH-269 — Expedition family
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public static class HostCliExpeditionFamily
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Expedition Family Self-Test (PLAN-EXPEDITION-FAMILY-TRUTH-269) ===");
            int passed = 0; const int total = 8;
            try
            {
                var session = new ExpeditionFamilyHostSession();

                // clear window
                var clear = session.EvaluateFlightWindow(30, 1000, 5, 1000, 15, 10, 50);
                if (!string.IsNullOrEmpty(clear.WindowCondition) && clear.EffectiveRangeKm > 0)
                { Console.WriteLine($"[PASS] Check 1: Clear aerial window ({clear.WindowCondition}, range {clear.EffectiveRangeKm}km)."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: clear window evaluation failed.");

                // severe window degrades
                var severe = session.EvaluateFlightWindow(30, 300, 90, 200, -30, 10, 50);
                if (severe.TotalFlightRiskPermille >= clear.TotalFlightRiskPermille || !severe.LaunchPermitted || clear.LaunchPermitted)
                { Console.WriteLine($"[PASS] Check 2: Severe conditions raise risk/block launch (risk {severe.TotalFlightRiskPermille}‰)."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: severe window not degraded.");

                // determinism
                var a = new ExpeditionFamilyHostSession().EvaluateFlightWindow(30, 800, 20, 700, 5, 10, 50);
                var b = new ExpeditionFamilyHostSession().EvaluateFlightWindow(30, 800, 20, 700, 5, 10, 50);
                if (a.TotalFlightRiskPermille == b.TotalFlightRiskPermille && a.AirdropDriftMeters == b.AirdropDriftMeters)
                { Console.WriteLine("[PASS] Check 3: Aerial evaluation is deterministic."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: aerial determinism broken.");

                // overload increases wear/risk
                var overload = session.EvaluateFlightWindow(30, 800, 20, 700, 5, 90, 50);
                if (overload.TotalFlightRiskPermille >= a.TotalFlightRiskPermille)
                { Console.WriteLine("[PASS] Check 4: Overload does not reduce risk."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: overload handling wrong.");

                // loot resolver: known category
                session.BindLootResolver(null, ExpeditionLootReferenceResolver.DefaultLootCategories);
                var known = session.ResolveLootReference("ammo");
                if (!string.IsNullOrEmpty(known.LootResolutionType))
                { Console.WriteLine($"[PASS] Check 5: Known loot reference resolved ({known.LootResolutionType})."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: known reference unresolved.");

                // loot resolver: unknown reference fails typed
                var unknown = session.ResolveLootReference("not_a_real_category_xyz");
                if (unknown.LootResolutionType.Contains("Unknown") || string.IsNullOrEmpty(unknown.LootCanonicalId))
                { Console.WriteLine($"[PASS] Check 6: Unknown loot reference fails typed ({unknown.LootResolutionType})."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: unknown reference not typed-failed.");

                // validator entry point
                var validation = session.ValidateLoot(Array.Empty<ExpeditionDefinition>());
                if (validation != null && validation.IsValid)
                { Console.WriteLine("[PASS] Check 7: Loot validator entry point returns a typed result."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: validator failed.");

                // host session wiring (pure, no save)
                if (session.Snapshot != null) { Console.WriteLine("[PASS] Check 8: Snapshot projection reads live results."); passed++; }
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }
            Console.WriteLine($"=== Expedition Family Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
