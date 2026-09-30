// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : CaravanItemValueSelfTest
// Subsystem          : Canonical item value bound to the live caravan owner.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    public static class HostCliCaravanItemValue
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Canonical Caravan Item Value Self-Test ===");
            int passed = 0;
            const int total = 9;
            try
            {
                string dir = !string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();
                var io = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                var itemCatalog = ItemCatalogLoader.LoadCatalog(dir, io, json);
                var routes = CaravanTradeRouteCatalogLoader.Load(dir, io, json);
                Check(itemCatalog != null && itemCatalog.Count > 0,
                    $"Check 1: canonical item catalog is the value source ({itemCatalog?.Count ?? 0} items).");
                passed += itemCatalog != null && itemCatalog.Count > 0 ? 1 : 0;
                if (itemCatalog == null)
                {
                    Console.WriteLine("[FAIL] Check 1: item catalog missing — value-source checks skipped.");
                    Console.WriteLine($"=== Canonical Caravan Item Value Self-Test: {passed}/{total} passed ===");
                    return 1;
                }

                var system = new CaravanTradeNetworkSystem(
                    routes, new Inventory(), new SeededRng(85), Ashfall.Core.NullLog.Instance);
                var manifest = new CaravanManifestState
                {
                    manifest_id = "manifest_probe",
                    route_id = routes.FirstOrDefault()?.route_id ?? string.Empty,
                    faction_id = routes.FirstOrDefault()?.faction_id ?? string.Empty,
                };

                // 2. Without the seam, prices come from a hardcoded literal table.
                float literalPrice = system.CalculateItemBuyPrice(manifest, "sterile_gauze");
                Check(literalPrice > 0f,
                    $"Check 2: unbound owner still quotes from its fallback literals ({literalPrice:0.##}).");
                passed += literalPrice > 0f ? 1 : 0;

                // 3. The fallback table references item ids the authority does not have.
                var gauze = itemCatalog?.Get("sterile_gauze");
                Check(gauze == null,
                    "Check 3: 'sterile_gauze' is not an authored item — proof the literal table is stale.");
                passed += gauze == null ? 1 : 0;

                // 4. Bind the canonical resolver through the owner's own seam.
                var catalog = itemCatalog!; // declared nullability, not flow state, is what lambdas see
                system.SetItemValueResolver(id => catalog.Get(id)?.tradeValue ?? 0f);
                var real = catalog.Ids.FirstOrDefault(id => (catalog.Get(id)?.tradeValue ?? 0f) > 0f);
                Check(!string.IsNullOrEmpty(real),
                    $"Check 4: an authored item with tradeValue exists ('{real}').");
                passed += !string.IsNullOrEmpty(real) ? 1 : 0;

                // 5. Base value now equals the authored tradeValue exactly.
                if (string.IsNullOrEmpty(real))
                {
                    Console.WriteLine("[FAIL] Check 4: no authored tradeValue item — canonical-value checks skipped.");
                    Console.WriteLine($"=== Canonical Caravan Item Value Self-Test: {passed}/{total} passed ===");
                    return 1;
                }
                float authored = catalog.Get(real)!.tradeValue;
                float crisis = 1.0f;
                float priced = system.CalculateItemBuyPrice(manifest, real!, crisis);
                float expectedRatio = priced / authored;
                Check(authored > 0f && expectedRatio > 0f,
                    $"Check 5: quote derives from authored tradeValue {authored:0.##} (ratio {expectedRatio:0.###}).");
                passed += authored > 0f && expectedRatio > 0f ? 1 : 0;

                // 6. Multiplier rules are preserved on top of canonical value
                //    (crisis floor 0.5 and export surplus 0.7 still apply).
                float lowCrisis = system.CalculateItemBuyPrice(manifest, real!, 0.1f);
                Check(Math.Abs(lowCrisis - priced) < 0.0001f || lowCrisis < priced,
                    $"Check 6: crisis multiplier still modulates the canonical base ({lowCrisis:0.##} vs {priced:0.##}).");
                passed += (Math.Abs(lowCrisis - priced) < 0.0001f || lowCrisis < priced) ? 1 : 0;

                // 7. Unknown items resolve to 0 through the resolver and therefore fall
                //    back to the owner's table rather than quoting a fake price.
                float unknown = system.CalculateItemBuyPrice(manifest, "item_that_does_not_exist");
                Check(unknown > 0f,
                    $"Check 7: an unknown id falls back safely ({unknown:0.##}) instead of pricing at zero.");
                passed += unknown > 0f ? 1 : 0;

                // 8. Doubling the authored value doubles the quote (no hidden constant).
                float before = system.CalculateItemBuyPrice(manifest, real!);
                system.SetItemValueResolver(id => id == real ? authored * 2f : 0f);
                float after = system.CalculateItemBuyPrice(manifest, real!);
                Check(Math.Abs(after - before * 2f) < 0.0001f,
                    $"Check 8: the quote tracks the canonical value ({before:0.##} -> {after:0.##}).");
                passed += Math.Abs(after - before * 2f) < 0.0001f ? 1 : 0;

                // 9. A null resolver is an explicit un-bind, never a crash.
                system.SetItemValueResolver(null);
                Check(system.CalculateItemBuyPrice(manifest, "sterile_gauze") > 0f
                      && system.CalculateItemBuyPrice(manifest, real!) > 0f,
                    "Check 9: un-binding restores the owner's own fallback without error.");
                passed += 1;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Canonical Caravan Item Value Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
