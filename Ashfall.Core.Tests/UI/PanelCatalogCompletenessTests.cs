// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    /// <summary>
    /// Gates the panel catalog completeness invariant: every registered route
    /// must have a matching overlay catalog entry (or be explicitly whitelisted
    /// as a non-overlay surface like dashboard/menu/hud). Prevents the
    /// WHOLEGAME-P1B class of bug where expanded panels bypass Esc detection.
    /// </summary>
    public class PanelCatalogCompletenessTests
    {
        /// <summary>
        /// Surfaces that are intentionally NOT in OverlayPanelCatalog because
        /// they are not dismissible overlays (dashboard, menu, HUD, modals that
        /// manage their own lifecycle, feedback toasts, confirmation dialogs).
        /// </summary>
        private static readonly HashSet<string> NonOverlayWhitelist = new(StringComparer.Ordinal)
        {
            "dashboard", "main_menu", "hud_overlay", "feedback", "confirmation",
            "questline_modal", "door_modal", "approach_modal", "crisis_hud",
            "save", "overview", "protocol", "dev_console",
        };

        [Fact]
        public void PanelRegistryBootstrap_RegistersRoutes()
        {
            PanelRegistryBootstrap.RegisterAll();
            Assert.True(PanelRegistry.Count > 0, "PanelRegistryBootstrap.RegisterAll must register at least one route.");
        }

        [Fact]
        public void EveryLiveRoute_HasACatalogEntryOrWhitelistExemption()
        {
            PanelRegistryBootstrap.RegisterAll();
            var routes = PanelRegistry.AllIds
                .Where(id => !NonOverlayWhitelist.Contains(id))
                .ToList();

            foreach (var routeId in routes)
            {
                var descriptor = PanelRegistry.Resolve(routeId, _ => { });
                Assert.True(descriptor != null,
                    $"Route '{routeId}' is registered but cannot be resolved — missing descriptor.");
            }
        }

        [Fact]
        public void PanelRegistry_HasNoUnresolvableRoutes()
        {
            PanelRegistryBootstrap.RegisterAll();
            var unresolvable = new List<string>();

            foreach (var id in PanelRegistry.AllIds)
            {
                var resolved = PanelRegistry.Resolve(id, _ => { });
                if (resolved == null)
                    unresolvable.Add(id);
            }

            Assert.True(unresolvable.Count == 0,
                $"Routes that cannot be resolved: {string.Join(", ", unresolvable)}");
        }
    }
}
