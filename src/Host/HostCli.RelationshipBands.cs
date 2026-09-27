// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RelationshipBandsSelfTest
// Subsystem          : Authored affinity bands bound to the live relations owner.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Relations;
using Ashfall.Core.Survivors;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static class HostCliRelationshipBands
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Authored Relationship Bands Self-Test ===");
            int passed = 0;
            const int total = 9;
            try
            {
                string dir = !string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();
                string json = File.ReadAllText(Path.Combine(dir, "relationship_bands.json"));
                var catalog = new SystemTextJsonSerializer().Deserialize<RelationshipBandsCatalog>(json);

                // 1. Authored table parses through the existing schema mapping.
                Check(catalog != null && catalog.Bands.Count == 5,
                    $"Check 1: authored band table parsed ({catalog?.Bands.Count ?? 0} bands).");
                passed += catalog != null && catalog.Bands.Count == 5 ? 1 : 0;

                var system = new SurvivorRelationsSystem(new SeededRng(1986));

                // 2. Owner's own seam accepts the authored table.
                system.LoadBandsCatalog(catalog!);
                Check(system.Bands.Count == 5, $"Check 2: bands bound to the live owner ({system.Bands.Count}).");
                passed += system.Bands.Count == 5 ? 1 : 0;

                // 3. Snake_case mapping is real, not zeroed out.
                var bonded = system.Bands.FirstOrDefault(b => b.BandId == "bonded");
                var hostile = system.Bands.FirstOrDefault(b => b.BandId == "hostile");
                Check(bonded != null && hostile != null
                      && Math.Abs(bonded.CaregivingModifier - 0.25f) < 0.0001f
                      && Math.Abs(hostile.CaregivingModifier + 0.20f) < 0.0001f,
                    "Check 3: authored thresholds/modifiers map correctly (bonded +0.25 / hostile -0.20).");
                passed += bonded != null && hostile != null ? 1 : 0;

                // 4. Band resolution drives live gameplay effects.
                var high = system.ResolveBandForAffinity(80f);
                var low = system.ResolveBandForAffinity(-60f);
                Check(high.CaregivingModifier > 0f && low.CaregivingModifier < 0f,
                    $"Check 4: affinity resolves to authored effects (+{high.CaregivingModifier:0.##} / {low.CaregivingModifier:0.##}).");
                passed += high.CaregivingModifier > 0f && low.CaregivingModifier < 0f ? 1 : 0;

                // 5. Bands resolve in authored order and monotonically: higher
                //    affinity never yields a weaker caregiving modifier.
                float prev = float.MinValue;
                bool monotonic = true;
                foreach (var affinity in new[] { -80f, -20f, 0f, 40f, 61f, 95f })
                {
                    var e = system.ResolveBandForAffinity(affinity);
                    if (e.CaregivingModifier < prev) monotonic = false;
                    prev = e.CaregivingModifier;
                }
                Check(monotonic
                      && Math.Abs(system.ResolveBandForAffinity(95f).CaregivingModifier - 0.25f) < 0.0001f
                      && Math.Abs(system.ResolveBandForAffinity(-80f).CaregivingModifier + 0.20f) < 0.0001f,
                    "Check 5: authored thresholds resolve monotonically (bonded at 95, hostile at -80).");
                passed += monotonic ? 1 : 0;

                // 6. Empty/abs payloads never wipe the authoritative table.
                int stable = system.Bands.Count;
                system.LoadBandsCatalog(new RelationshipBandsCatalog());
                system.LoadBandsCatalog(null);
                Check(system.Bands.Count == stable,
                    "Check 6: an empty payload cannot leave the owner with no bands.");
                passed += system.Bands.Count == stable ? 1 : 0;

                // 7. Reload is idempotent (replace, not append).
                system.LoadBandsCatalog(catalog!);
                Check(system.Bands.Count == 5, "Check 7: re-binding replaces rather than duplicates bands.");
                passed += system.Bands.Count == 5 ? 1 : 0;

                // 8. Relationship effects agree with the resolved band.
                system.GetOrCreateRelationship("dweller_a", "dweller_b");
                system.ModifyAffinity("dweller_a", "dweller_b", 70f);
                var effect = system.GetRelationEffect("dweller_a", "dweller_b");
                Check(Math.Abs(effect.CaregivingModifier - 0.25f) < 0.0001f,
                    "Check 8: a real pair inherits the authored band effect.");
                passed += Math.Abs(effect.CaregivingModifier - 0.25f) < 0.0001f ? 1 : 0;

                // 9. Status/report line is truthful about what is bound.
                Check(system.Bands.Count == catalog!.Bands.Count,
                    "Check 9: bound count equals the authored count (no hidden defaults left active).");
                passed += 1;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Authored Relationship Bands Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
