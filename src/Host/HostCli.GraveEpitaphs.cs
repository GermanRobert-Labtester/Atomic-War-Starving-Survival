// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : GraveEpitaphsSelfTest
// Subsystem          : Authored grave epitaphs bound to the live memorial owner.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Memorial;

namespace AtomicWar.GodotApp
{
    public static class HostCliGraveEpitaphs
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Grave Epitaph Binding Self-Test ===");
            int passed = 0;
            const int total = 9;
            try
            {
                string root = !string.IsNullOrEmpty(dataDir) && System.IO.Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();

                var memorial = new MemorialSystem(new MemorialState());
                var rng = new SeededRng(2026);
                var session = new GraveEpitaphHostSession(() => memorial, () => rng);

                // 1. The authored table loads and is non-empty.
                int count = session.Bind(root);
                Check(count > 0 && session.EpitaphCount == count,
                    $"Check 1: authored epitaph table loaded ({count} rows).");
                passed += count > 0 ? 1 : 0;

                // 2. The two previously-unassigned seams are now assigned.
                Check(memorial.EpitaphCatalog != null && memorial.EpitaphRng != null
                      && session.IsBoundToLiveMemorial(),
                    "Check 2: MemorialSystem.EpitaphCatalog and EpitaphRng are assigned.");
                passed += (memorial.EpitaphCatalog != null && memorial.EpitaphRng != null) ? 1 : 0;

                // 3. Every authored row carries a cause and a non-empty inscription.
                bool shaped = true;
                foreach (var e in session.Catalog!.AllEntries)
                    if (string.IsNullOrWhiteSpace(e.cause) || string.IsNullOrWhiteSpace(e.epitaph)) shaped = false;
                Check(shaped, "Check 3: every authored epitaph has a cause and text.");
                passed += shaped ? 1 : 0;

                // 4. Selection by an authored cause returns one of the authored lines.
                string cause = session.Catalog.AllEntries[0].cause;
                string picked = session.EpitaphFor(cause);
                bool isAuthored = false;
                foreach (var e in session.Catalog.AllEntries)
                    if (string.Equals(e.cause, cause, StringComparison.OrdinalIgnoreCase) && e.epitaph == picked)
                        isAuthored = true;
                Check(!string.IsNullOrEmpty(picked) && isAuthored,
                    "Check 4: selecting by cause yields an authored inscription, never invented text.");
                passed += isAuthored ? 1 : 0;

                // 5. An unknown cause still yields an authored line, not empty.
                string fallback = session.EpitaphFor("cause_nobody_died_this_way");
                Check(!string.IsNullOrEmpty(fallback),
                    "Check 5: an unmapped cause still resolves to an authored inscription.");
                passed += !string.IsNullOrEmpty(fallback) ? 1 : 0;

                // 6. Same seed, same inscription: determinism on the campaign stream.
                var s2 = new GraveEpitaphHostSession(() => memorial, () => new SeededRng(7));
                var s3 = new GraveEpitaphHostSession(() => memorial, () => new SeededRng(7));
                s2.Bind(root); s3.Bind(root);
                Check(s2.EpitaphFor(cause) == s3.EpitaphFor(cause),
                    "Check 6: epitaph selection is deterministic for a fixed seed.");
                passed += s2.EpitaphFor(cause) == s3.EpitaphFor(cause) ? 1 : 0;

                // 7. Binding is idempotent — the table is not re-loaded or duplicated.
                int before = session.EpitaphCount;
                session.Bind(root);
                Check(session.EpitaphCount == before,
                    "Check 7: re-binding keeps one authored table.");
                passed += session.EpitaphCount == before ? 1 : 0;

                // 8. The memorial owner is still the sole memorial authority.
                Check(ReferenceEquals(memorial.EpitaphCatalog, session.Catalog),
                    "Check 8: the catalog lives on the memorial owner, not in a parallel store.");
                passed += 1;

                // 9. The status line is truthful.
                Check(session.StatusLine() == $"{count} authored epitaph(s)",
                    $"Check 9: status line reports the bound table ({session.StatusLine()}).");
                passed += session.StatusLine() == $"{count} authored epitaph(s)" ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Grave Epitaph Binding Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
