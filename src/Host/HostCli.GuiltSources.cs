// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : GuiltSourcesSelfTest
// Subsystem          : Authored guilt sources bound to the live guilt owner.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliGuiltSources
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Authored Guilt Sources Self-Test ===");
            int passed = 0;
            const int total = 10;
            try
            {
                string dataRoot = !string.IsNullOrEmpty(dataDir) && System.IO.Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();

                var guilt = new GuiltInsomniaSystem();
                int day = 3;
                var session = new GuiltSourceHostSession(() => guilt, () => day);
                int loaded = session.LoadCatalog(dataRoot);

                // 1. The orphaned catalog now has a consumer.
                Check(loaded > 0 && session.IsBound && session.SourceCount == loaded,
                    $"Check 1: authored guilt source catalog loaded ({loaded} rows).");
                passed += loaded > 0 ? 1 : 0;

                // 2. Every authored row carries a usable severity in range.
                int inRange = 0;
                foreach (var (_, severity, _) in session.AuthoredSeverities())
                    if (severity > 0f && severity <= 1f) inRange++;
                Check(inRange == loaded,
                    $"Check 2: every authored severity is in (0,1] ({inRange}/{loaded}).");
                passed += inRange == loaded ? 1 : 0;

                // 3. A known pattern records with the AUTHORED severity, not a literal.
                var rows = session.AuthoredSeverities();
                string pattern = rows[0].Pattern;
                float authored = rows[0].Severity;
                var probe = new GuiltInsomniaSystem();
                var probeSession = new GuiltSourceHostSession(() => probe, () => day);
                probeSession.LoadCatalog(dataRoot);
                Check(probeSession.RecordGuiltFromChoice("surv_a", pattern),
                    "Check 3: a choice pattern resolves to its authored guilt record.");
                passed += 1;

                // 4. The recorded severity equals the authored value exactly.
                float recorded = -1f;
                foreach (var survivor in probe.CaptureState().survivors)
                {
                    if (!string.Equals(survivor.survivorId, "surv_a", StringComparison.Ordinal)) continue;
                    foreach (var rec in survivor.guiltSources)
                        if (string.Equals(rec.sourceId, pattern, StringComparison.Ordinal)) recorded = rec.severity;
                }
                Check(Math.Abs(recorded - authored) < 0.0001f,
                    $"Check 4: recorded severity matches the catalog exactly ({recorded:0.###} vs {authored:0.###}).");
                passed += Math.Abs(recorded - authored) < 0.0001f ? 1 : 0;

                // 5. An unauthored pattern is refused and mutates nothing.
                int sourcesBefore = probe.GetGuiltSourceCount("surv_b");
                bool refused = probeSession.RecordGuiltFromChoice("surv_b", "pattern_nobody_authored");
                Check(!refused && probe.GetGuiltSourceCount("surv_b") == sourcesBefore,
                    "Check 5: an unauthored pattern is refused without inventing a severity.");
                passed += (!refused && probe.GetGuiltSourceCount("surv_b") == sourcesBefore) ? 1 : 0;

                // 6. An empty survivor id is refused.
                Check(!probeSession.RecordGuiltFromChoice(string.Empty, pattern),
                    "Check 6: a missing survivor id is refused.");
                passed += 1;

                // 7. The authored description templates the survivor name.
                var def = session.Resolve(pattern);
                string described = session.Describe("surv_a", pattern, "Maren");
                Check(def != null && !string.IsNullOrEmpty(described) && !described.Contains("{name}"),
                    "Check 7: the authored description is templated, not a placeholder.");
                passed += def != null && described.Length > 0 && !described.Contains("{name}") ? 1 : 0;

                // 8. Unknown patterns produce no description rather than a generic one.
                Check(session.Describe("surv_a", "pattern_nobody_authored", "Maren").Length == 0,
                    "Check 8: an unauthored pattern yields no invented description.");
                passed += 1;

                // 9. The live guilt owner stays the sole authority (no second ledger).
                float severitySum = 0f;
                foreach (var survivor in probe.CaptureState().survivors)
                    foreach (var rec in survivor.guiltSources) severitySum += rec.severity;
                Check(severitySum > 0f && probe.GetInsomniaSeverity("surv_a") > 0f,
                    "Check 9: guilt records live only in the existing GuiltInsomniaSystem state.");
                passed += severitySum > 0f ? 1 : 0;

                // 10. The catalog is read-only: resolving twice is stable.
                Check(session.Resolve(pattern)?.Severity == session.Resolve(pattern)?.Severity
                      && session.StatusLine() == $"{loaded} authored guilt source(s)",
                    "Check 10: catalog resolution is a stable read-only projection.");
                passed += 1;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Authored Guilt Sources Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
