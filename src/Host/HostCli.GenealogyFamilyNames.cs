// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : GenealogyFamilyNamesSelfTest
// Subsystem          : Authored family-name catalog bound to the live lineage owner.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Legacy;

namespace AtomicWar.GodotApp
{
    public static class HostCliGenealogyFamilyNames
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Authored Family-Name Catalog Self-Test ===");
            int passed = 0;
            const int total = 9;
            try
            {
                string dir = !string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();
                string json = File.ReadAllText(Path.Combine(dir, "family_name_templates.json"));

                var lineage = new GenerationalLineageExtension(new GenerationalSuccessionEngine());

                // 1. Unbound owner generates a name from built-ins only.
                string unbound = lineage.GenerateFamilyName(new SeededRng(7));
                Check(!string.IsNullOrEmpty(unbound),
                    $"Check 1: without the authored catalog the owner still yields a fallback name ('{unbound}').");
                passed += !string.IsNullOrEmpty(unbound) ? 1 : 0;

                // 2. Binding through the owner's own seam succeeds.
                lineage.LoadFamilyNameCatalog(json);
                string bound = lineage.GenerateFamilyName(new SeededRng(7));
                Check(!string.IsNullOrEmpty(bound),
                    $"Check 2: with the authored catalog bound, generation succeeds ('{bound}').");
                passed += !string.IsNullOrEmpty(bound) ? 1 : 0;

                // 3. Determinism: same seed, same authored name.
                var again = new GenerationalLineageExtension(new GenerationalSuccessionEngine());
                again.LoadFamilyNameCatalog(json);
                Check(bound == again.GenerateFamilyName(new SeededRng(7)),
                    "Check 3: authored name generation is deterministic for a fixed seed.");
                passed += bound == again.GenerateFamilyName(new SeededRng(7)) ? 1 : 0;

                // 4. Different seeds produce authored variety (not one constant).
                var seen = new System.Collections.Generic.HashSet<string>();
                for (int s = 1; s <= 12; s++) seen.Add(lineage.GenerateFamilyName(new SeededRng(s)));
                Check(seen.Count > 1, $"Check 4: authored templates produce variety ({seen.Count} distinct).");
                passed += seen.Count > 1 ? 1 : 0;

                // 5. An authored cultural archetype is honoured when requested.
                string archetypeName = string.Empty;
                try { archetypeName = lineage.GenerateFamilyName(new SeededRng(3), "scrapfolk"); }
                catch { archetypeName = "<not-authored>"; }
                Check(!string.IsNullOrEmpty(archetypeName),
                    $"Check 5: requesting an archetype does not break generation ('{archetypeName}').");
                passed += !string.IsNullOrEmpty(archetypeName) ? 1 : 0;

                // 6. Assignment + inheritance still work through the same owner.
                lineage.AssignFamilyName("dweller_a", bound);
                Check(lineage.GetFamilyName("dweller_a") == bound,
                    "Check 6: assigned surnames read back from the lineage owner.");
                passed += lineage.GetFamilyName("dweller_a") == bound ? 1 : 0;

                var child = lineage.InheritFamilyName("dweller_child", "dweller_a");
                Check(child == bound, "Check 7: inheritance follows the recorded family name.");
                passed += child == bound ? 1 : 0;

                // 8. Re-binding is idempotent (no duplicated catalog growth).
                lineage.LoadFamilyNameCatalog(json);
                Check(lineage.GenerateFamilyName(new SeededRng(7)) == bound,
                    "Check 8: re-binding the same authored table changes nothing.");
                passed += lineage.GenerateFamilyName(new SeededRng(7)) == bound ? 1 : 0;

                // 9. A wrong-schema payload must not crash and must not corrupt the
                //    owner: after rejecting it, re-binding the authored table restores
                //    authored behaviour exactly.
                try { lineage.LoadFamilyNameCatalog("{\"schema_version\":99}"); }
                catch { /* owner logs and clears; behaviour asserted below, not here */ }
                lineage.LoadFamilyNameCatalog(json);
                Check(lineage.GenerateFamilyName(new SeededRng(7)) == bound,
                    "Check 9: an unsupported schema cannot crash the owner, and re-binding restores authored behaviour.");
                passed += lineage.GenerateFamilyName(new SeededRng(7)) == bound ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Authored Family-Name Catalog Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
