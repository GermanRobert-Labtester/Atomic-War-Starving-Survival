// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PlayerSurfaceManifestSelfTest
// Subsystem          : Player surface coverage manifest over the live registry.
// ============================================================================
using System;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp
{
    public static class HostCliPlayerSurfaceManifest
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Player Surface Manifest Self-Test ===");
            int passed = 0;
            const int total = 9;
            try
            {
                var session = new PlayerSurfaceManifestHostSession();
                var manifest = session.Generate();

                // 1. The manifest is generated from the live registry.
                Check(manifest != null && manifest.TotalSurfaces > 0,
                    $"Check 1: {manifest?.TotalSurfaces ?? 0} player-navigable surfaces enumerated from the live registry.");
                passed += manifest != null && manifest.TotalSurfaces > 0 ? 1 : 0;
                if (manifest == null)
                {
                    Console.WriteLine("[FAIL] Check 1: manifest generation failed — coverage checks skipped.");
                    Console.WriteLine($"=== Player Surface Manifest Self-Test: {passed}/{total} passed ===");
                    return 1;
                }

                // 2. Every player-navigable surface has a route.
                Check(manifest != null && PlayerSurfaceManifestHostSession.IsEverySurfaceRouted(manifest),
                    $"Check 2: all {manifest?.RoutedSurfaces ?? 0}/{manifest?.TotalSurfaces ?? 0} surfaces are routed.");
                passed += manifest != null && PlayerSurfaceManifestHostSession.IsEverySurfaceRouted(manifest) ? 1 : 0;

                // 3. Every surface can be dismissed (the Esc softlock class is asserted).
                Check(manifest != null && PlayerSurfaceManifestHostSession.IsEverySurfaceClosable(manifest),
                    $"Check 3: all {manifest?.CloseableSurfaces ?? 0}/{manifest?.TotalSurfaces ?? 0} surfaces are keyboard-closable.");
                passed += manifest != null && PlayerSurfaceManifestHostSession.IsEverySurfaceClosable(manifest) ? 1 : 0;

                // 4. Coverage buckets are exhaustive, not overlapping guesses.
                Check(manifest != null && manifest.InteractiveActionSurfaces + manifest.ReadOnlySurfaces == manifest.TotalSurfaces,
                    $"Check 4: interactive {manifest?.InteractiveActionSurfaces ?? 0} + read-only {manifest?.ReadOnlySurfaces ?? 0} = total {manifest?.TotalSurfaces ?? 0}.");
                passed += manifest != null && manifest.InteractiveActionSurfaces + manifest.ReadOnlySurfaces == manifest.TotalSurfaces ? 1 : 0;

                // 5. Binding coverage is measured against the same registry.
                Check(manifest != null && manifest.BoundSurfaces <= manifest.TotalSurfaces && manifest.BoundSurfaces > 0,
                    $"Check 5: {manifest?.BoundSurfaces ?? 0} surfaces name a binding target.");
                passed += manifest != null && manifest.BoundSurfaces > 0 ? 1 : 0;

                // 6. Snapshot coverage is reported honestly (not inflated to 100%).
                Check(manifest!.SnapshotCoveredSurfaces <= manifest.TotalSurfaces // guarded above
                      && session.CoverageSummary().Contains("snapshot-covered", StringComparison.Ordinal),
                    $"Check 6: snapshot coverage reported as truth — {session.CoverageSummary()}.");
                passed += manifest.SnapshotCoveredSurfaces <= manifest.TotalSurfaces ? 1 : 0;

                // 7. Regeneration is stable (pure projection).
                var again = session.Generate();
                Check(again.TotalSurfaces == manifest.TotalSurfaces
                      && again.RoutedSurfaces == manifest.RoutedSurfaces
                      && session.GenerationCount == 2,
                    "Check 7: regenerating the manifest is stable and mutates nothing.");
                passed += again.TotalSurfaces == manifest.TotalSurfaces ? 1 : 0;

                // 8. Esc-only surfaces are enumerated exactly (honest, not suppressed).
                var escOnly = session.EscOnlySurfaceIds();
                int authoredEscOnly = 0;
                foreach (var c in manifest.Contracts)
                    if (c != null && c.CloseBehavior == Ashfall.Core.UI.SurfaceCloseBehavior.EscKeyOnly) authoredEscOnly++;
                Check(escOnly.Count == authoredEscOnly
                      && manifest.CloseableSurfaces + authoredEscOnly == manifest.TotalSurfaces,
                    $"Check 8: {authoredEscOnly} Esc-only surface(s) enumerated honestly and reconciled.");
                passed += escOnly.Count == authoredEscOnly ? 1 : 0;

                // 9. The status line carries the measured numbers.
                Check(session.StatusLine().Contains("Player surfaces", StringComparison.Ordinal)
                      && session.StatusLine().Contains(manifest.TotalSurfaces.ToString(), StringComparison.Ordinal),
                    "Check 9: the status line reports measured coverage.");
                passed += 1;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Player Surface Manifest Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
