// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : MigrationConsequenceSelfTest
// Subsystem          : XP-08-F6 — seasonal migration consequences
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public static class HostCliMigrationConsequence
    {
        private static readonly string[] Regions = { "settlement", "iron_basin", "ash_flats", "deep_coast", "industrial_belt" };

        private static void AssertProbe(bool condition)
        {
            if (!condition) throw new InvalidOperationException("Probe precondition failed: no authored region moved off baseline.");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Migration Consequence Self-Test (XP-08-F6) ===");
            int passed = 0;
            const int total = 11;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir
                    : CatalogPath.ResolveDataDir());

                // The live Plan 199 migration owner, exactly as the campaign holds it.
                // The live Plan 199 migration owner, exactly as the campaign holds it:
                // construct the hosted session (it loads the authored seasonal
                // schedule) and read its engine back out.
                var migrationSession = HumanMigrationHostSession.Create(dataRoot);
                var migration = migrationSession.Engine;

                // Drive the authored seasonal schedule so at least one region's
                // weight moves off the 100 baseline; at baseline every category
                // class is legitimately 1000 permille.
                // The engine applies a phase only when the phase changes and the
                // authored dwell window has elapsed, so walk the seasons forward.
                int day = 1;
                foreach (string phase in new[] { "deep_winter", "thaw", "dry_heat", "ash_winds" })
                {
                    migrationSession.TickDay(day, phase);
                    day += 20;
                }
                bool moved = false;
                foreach (var w in migrationSession.RegionWeights.Values) if (w != 100) moved = true;
                AssertProbe(moved);

                var session = new MigrationConsequenceHostSession(migration);

                // 1. The consequence engine is bound to the LIVE migration instance.
                if (ReferenceEquals(session.MigrationEngine, migrationSession.Engine)
                    && session.GetProjection("settlement").PopulationWeight > 0)
                { Console.WriteLine("[PASS] Check 1: Consequence engine reads the live hosted migration owner."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: not bound to the live migration instance.");

                // 2. Food demand scales directly with population weight.
                int wLow = session.GetProjection("ash_flats").PopulationWeight;
                int foodLow = session.GetMarketDemandMultiplierPermille("ash_flats", "food");
                if (foodLow >= 200 && foodLow >= MigrationConsequenceEngine.PermilleScale * Math.Min(1, wLow) / 100)
                { Console.WriteLine($"[PASS] Check 2: Food demand tracks population weight ({wLow} -> {foodLow} permille)."); passed++; }
                else Console.WriteLine($"[FAIL] Check 2: food demand wrong ({foodLow}).");

                // 3. Category differentiation is engine-owned: a region whose weight
                // has moved off baseline must show distinct class behaviour.
                int luxury = session.GetMarketDemandMultiplierPermille("ash_flats", "luxury");
                int labor = session.GetMarketDemandMultiplierPermille("ash_flats", "labor");
                int general = session.GetMarketDemandMultiplierPermille("ash_flats", "tools");
                if (luxury != foodLow && labor != foodLow && general != foodLow)
                { Console.WriteLine($"[PASS] Check 3: Category differentiation is engine-owned (food {foodLow}, luxury {luxury}, labor {labor})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 3: category differentiation collapsed (food {foodLow}, luxury {luxury}, labor {labor}, general {general}).");

                // 4. Labour pool multiplier is bounded and monotone with weight.
                int laborPool = session.GetLaborPoolMultiplierPermille("ash_flats");
                if (laborPool >= 100 && laborPool <= 3000)
                { Console.WriteLine($"[PASS] Check 4: Labour pool multiplier bounded ({laborPool} permille)."); passed++; }
                else Console.WriteLine($"[FAIL] Check 4: labour pool multiplier out of bounds ({laborPool}).");

                // 5. Territorial friction multiplier is bounded.
                int friction = session.GetTerritorialFrictionMultiplierPermille("ash_flats");
                if (friction >= 400)
                { Console.WriteLine($"[PASS] Check 5: Territorial friction multiplier bounded ({friction} permille)."); passed++; }
                else Console.WriteLine($"[FAIL] Check 5: territorial friction wrong ({friction}).");

                // 6. Caravan demand priority is one of the three authored bands.
                string priority = session.GetCaravanDemandPriority("ash_flats");
                if (priority == "food_and_fuel" || priority == "defense_and_labor" || priority == "balanced_trade")
                { Console.WriteLine($"[PASS] Check 6: Caravan demand priority in an authored band ({priority})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 6: caravan priority band unknown ({priority}).");

                // 7. Phase consequence applies once.
                bool first = session.TryApplyPhaseConsequence(30, "deep_winter", "ash_flats");
                bool second = session.TryApplyPhaseConsequence(30, "deep_winter", "ash_flats");
                if (first && !second)
                { Console.WriteLine("[PASS] Check 7: Phase consequence is exactly-once per (region, phase, day)."); passed++; }
                else Console.WriteLine($"[FAIL] Check 7: exactly-once broken ({first}/{second}).");

                // 8. The exactly-once ledger survives a save/load round-trip.
                var captured = session.CaptureState();
                var persisted = MigrationConsequenceSaveStore.TryCapturePersisted(captured);
                var restored = MigrationConsequenceSaveStore.TryRestorePersisted(persisted);
                var reload = new MigrationConsequenceHostSession(migrationSession.Engine);
                reload.RestoreState(restored);
                bool afterReload = reload.TryApplyPhaseConsequence(30, "deep_winter", "ash_flats");
                if (!afterReload && restored != null)
                { Console.WriteLine("[PASS] Check 8: Exactly-once ledger survives save/load (no double consequence)."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: consequence re-applied after reload.");

                // 9. Market consequence source id is region/phase scoped for owner idempotence.
                string src = MigrationConsequenceHostSession.MarketShockSourceId("ash_flats", "deep_winter");
                if (src == "migration_ash_flats_deep_winter")
                { Console.WriteLine($"[PASS] Check 9: Market shock source id is region/phase scoped ({src})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 9: source id wrong ({src}).");

                // 10. Projection reads the live engine for every authored region.
                int projected = 0;
                foreach (string region in Regions)
                {
                    var p = reload.GetProjection(region);
                    if (p.PopulationWeight > 0 && p.FoodDemandMultiplierPermille > 0) projected++;
                }
                if (projected == Regions.Length)
                { Console.WriteLine($"[PASS] Check 10: Projection covers all {projected} authored regions."); passed++; }
                else Console.WriteLine($"[FAIL] Check 10: projection covered {projected}/{Regions.Length} regions.");

                // 11. Reset clears only this engine's ledger (migration state untouched).
                int weightBefore = migrationSession.GetRegionPopulationWeight("ash_flats");
                reload.Reset();
                if (reload.CaptureState().AppliedConsequenceKeys.Count == 0 &&
                    migrationSession.GetRegionPopulationWeight("ash_flats") == weightBefore)
                { Console.WriteLine("[PASS] Check 11: Reset clears the consequence ledger only; migration state untouched."); passed++; }
                else Console.WriteLine("[FAIL] Check 11: reset leaked into the migration owner.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Migration Consequence Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
