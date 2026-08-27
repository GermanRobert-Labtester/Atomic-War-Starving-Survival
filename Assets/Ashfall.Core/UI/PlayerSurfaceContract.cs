// SPDX-License-Identifier: MIT
// ASHFALL Core Player-Surface Contract & Coverage Manifest (Task 109).
using System;

namespace Ashfall.Core.UI
{
    public enum SurfaceRouteKind
    {
        Dashboard,
        ExpandedShelter,
        MainMenu,
        SecondaryDetail,
        ModalOverlay
    }

    public enum SurfaceBindingKind
    {
        HostSession,
        CoreSystem,
        Composite,
        ReadOnlyPresentation
    }

    public enum SurfaceActionCoverage
    {
        InteractiveCommands,
        ReadOnlyObservational,
        PendingWiring
    }

    public enum SurfaceRenderCoverage
    {
        ProductionRendered,
        PreviewRendered,
        Stub
    }

    public enum SurfaceCloseBehavior
    {
        DashboardShellCloseButton,
        OverlayCloseButton,
        ModalDismiss,
        EscKeyOnly,
        CustomCloseAction
    }

    /// <summary>
    /// Formal contract metadata and verification tracking for a single player-facing surface.
    /// </summary>
    public sealed class PlayerSurfaceContract
    {
        public string PanelId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public PanelGroup Group { get; set; }
        public SurfaceRouteKind RouteKind { get; set; }
        public string[] SetupDependencies { get; set; } = Array.Empty<string>();
        public bool AvailableInMenu { get; set; }
        public SurfaceBindingKind BindingKind { get; set; }
        public string BindingTarget { get; set; } = string.Empty;
        public SurfaceCloseBehavior CloseBehavior { get; set; }
        public bool HasStatusRail { get; set; }
        public bool HasDashboardShell { get; set; }
        public bool HasSnapshotCoverage { get; set; }
        public SurfaceActionCoverage ActionCoverage { get; set; }
        public SurfaceRenderCoverage RenderCoverage { get; set; }
        public string[] ReachableRoutes { get; set; } = Array.Empty<string>();
        public string Description { get; set; } = string.Empty;

        public override string ToString() => $"[Surface:{PanelId} ({Group}) Route={RouteKind} Actions={ActionCoverage}]";
    }
}
