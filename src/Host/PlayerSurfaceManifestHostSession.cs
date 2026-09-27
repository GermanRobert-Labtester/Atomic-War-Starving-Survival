// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Player Surface Manifest host binding.
//
// PlayerSurfaceManifest / PlayerSurfaceContract shipped with no consumer, so the
// UI rules the project states in prose (keyboard close/back behaviour, focus,
// lifecycle coverage, read-only versus interactive surfaces) were unmeasured.
// This session generates the manifest from the live PanelRegistry and reports it.
// Pure projection: generating it mutates no panel and no registry state.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp
{
    public sealed class PlayerSurfaceManifestHostSession : HostSessionBase
    {
        public string LastEvent { get; private set; } = string.Empty;
        public PlayerSurfaceManifest? Manifest { get; private set; }
        public int GenerationCount { get; private set; }

        public int TotalSurfaces => Manifest?.TotalSurfaces ?? 0;
        public int RoutedSurfaces => Manifest?.RoutedSurfaces ?? 0;
        public int BoundSurfaces => Manifest?.BoundSurfaces ?? 0;
        public int CloseableSurfaces => Manifest?.CloseableSurfaces ?? 0;
        public int SnapshotCoveredSurfaces => Manifest?.SnapshotCoveredSurfaces ?? 0;
        public int InteractiveActionSurfaces => Manifest?.InteractiveActionSurfaces ?? 0;
        public int ReadOnlySurfaces => Manifest?.ReadOnlySurfaces ?? 0;

        /// <summary>
        /// Surfaces whose ONLY documented dismissal is the Escape key — the weakest
        /// tier, and the class that historically produced controller/softlock
        /// complaints. An empty list means every surface has a real close affordance.
        /// </summary>
        public IReadOnlyList<string> EscOnlySurfaceIds()
        {
            var ids = new List<string>();
            if (Manifest == null) return ids;
            foreach (var c in Manifest.Contracts)
                if (c != null && c.CloseBehavior == SurfaceCloseBehavior.EscKeyOnly) ids.Add(c.PanelId);
            ids.Sort(StringComparer.Ordinal);
            return ids;
        }

        public static bool IsEverySurfaceRouted(PlayerSurfaceManifest? manifest)
            => manifest != null && manifest.TotalSurfaces > 0
               && manifest.RoutedSurfaces == manifest.TotalSurfaces;

        public static bool IsEverySurfaceClosable(PlayerSurfaceManifest? manifest)
            => manifest != null && manifest.TotalSurfaces > 0
               && manifest.CloseableSurfaces == manifest.TotalSurfaces;

        /// <summary>Regenerates the manifest from the live registry. Read-only.</summary>
        public PlayerSurfaceManifest Generate()
        {
            var manifest = PlayerSurfaceManifest.Generate();
            Manifest = manifest;
            GenerationCount++;
            LastEvent = $"Player surfaces: {manifest.TotalSurfaces} navigable · routed {manifest.RoutedSurfaces}"
                      + $" · closable {manifest.CloseableSurfaces} · snapshot {manifest.SnapshotCoveredSurfaces}"
                      + $" · interactive {manifest.InteractiveActionSurfaces} · read-only {manifest.ReadOnlySurfaces}";
            RaiseStateChanged();
            return manifest;
        }

        public string CoverageSummary()
        {
            if (Manifest == null) return "surface manifest not generated";
            int total = Manifest.TotalSurfaces;
            if (total == 0) return "no player-navigable surfaces";
            return $"{Manifest.SnapshotCoveredSurfaces}/{total} snapshot-covered"
                 + $" ({(int)Math.Round(100.0 * Manifest.SnapshotCoveredSurfaces / total)}%)"
                 + $", {Manifest.InteractiveActionSurfaces} interactive / {Manifest.ReadOnlySurfaces} read-only";
        }

        public string StatusLine() => LastEvent;
    }
}
