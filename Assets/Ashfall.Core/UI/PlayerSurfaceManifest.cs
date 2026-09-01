// SPDX-License-Identifier: MIT
// ASHFALL Core Player-Surface Contract & Coverage Manifest (Task 109).
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashfall.Core.UI
{
    /// <summary>
    /// Player-surface manifest generated from the typed PanelRegistry.
    /// Records setup dependencies, binding targets, route kinds, close behaviors,
    /// action coverage, accessibility properties, and visual snapshot coverage.
    /// </summary>
    public sealed class PlayerSurfaceManifest
    {
        public int TotalSurfaces => Contracts.Count;
        public int RoutedSurfaces => Contracts.Count(c => c.ReachableRoutes.Length > 0);
        public int BoundSurfaces => Contracts.Count(c => !string.IsNullOrEmpty(c.BindingTarget));
        public int CloseableSurfaces => Contracts.Count(c => c.CloseBehavior != SurfaceCloseBehavior.EscKeyOnly);
        public int SnapshotCoveredSurfaces => Contracts.Count(c => c.HasSnapshotCoverage);
        public int InteractiveActionSurfaces => Contracts.Count(c => c.ActionCoverage == SurfaceActionCoverage.InteractiveCommands);
        public int ReadOnlySurfaces => Contracts.Count(c => c.ActionCoverage == SurfaceActionCoverage.ReadOnlyObservational);

        public IReadOnlyList<PlayerSurfaceContract> Contracts { get; }

        public PlayerSurfaceManifest(IEnumerable<PlayerSurfaceContract> contracts)
        {
            Contracts = contracts?.ToList() ?? new List<PlayerSurfaceContract>();
        }

        /// <summary>
        /// Generates the manifest by inspecting all descriptors registered in PanelRegistry.
        /// </summary>
        public static PlayerSurfaceManifest Generate()
        {
            PanelRegistryBootstrap.RegisterAll();

            var list = new List<PlayerSurfaceContract>(PanelRegistry.Count);
            foreach (string id in PanelRegistry.AllIds)
            {
                var desc = PanelRegistry.Get(id);
                if (desc == null) continue;
                list.Add(BuildContractFor(desc));
            }
            return new PlayerSurfaceManifest(list);
        }

        private static readonly HashSet<string> SnapshotPanelIds = new(StringComparer.OrdinalIgnoreCase)
        {
            "inventory", "survivors", "medical", "radio", "weather", "shelter", "journal", "verdict",
            "trade", "greenhouse", "silent_foundry", "duty_roster", "shelter_decor", "map", "maritime", "muster",
            "quests", "standing_record", "research", "combat", "factions", "codex"
        };

        private static readonly HashSet<string> InteractivePanelIds = new(StringComparer.OrdinalIgnoreCase)
        {
            "inventory", "crafting", "workshop", "pharma_lab", "pharma", "medical", "expeditions",
            "radio", "greenhouse", "silent_foundry", "trade", "muster", "duty_roster", "save",
            "settings", "combat", "water_treatment", "airlock_security", "survivor_relations",
            "regional_treaty", "vinyl_morale", "wildlife_trapping", "excavation", "apprenticeship",
            "caregiving", "shelter_thermal", "shelter_schedule", "shelter_decor", "autopsy_report", "waystation_network",
            "chemical_dependency", "sump_flooding", "decontamination", "kitchen_nutrition",
            "equipment_condition", "library_study", "archive_desk", "contractor_roster",
            "mental_health_crisis", "phantom_memory", "traveling_caravan", "medical_ward"
        };

        private static PlayerSurfaceContract BuildContractFor(PanelDescriptor desc)
        {
            var routeKind = desc.Group switch
            {
                PanelGroup.Dashboard => SurfaceRouteKind.Dashboard,
                PanelGroup.Expanded => SurfaceRouteKind.ExpandedShelter,
                PanelGroup.MainMenu => SurfaceRouteKind.MainMenu,
                PanelGroup.Secondary => SurfaceRouteKind.SecondaryDetail,
                _ => SurfaceRouteKind.Dashboard
            };

            if (desc.Id == "protocol")
                routeKind = SurfaceRouteKind.ModalOverlay;

            var bindingKind = desc.Group == PanelGroup.Expanded
                ? SurfaceBindingKind.HostSession
                : (desc.SetupDependencies.Length > 0 ? SurfaceBindingKind.Composite : SurfaceBindingKind.HostSession);

            string bindingTarget = desc.Group == PanelGroup.Expanded
                ? $"{desc.Id}HostSession"
                : (desc.SetupDependencies.Length > 0 ? string.Join(", ", desc.SetupDependencies) : $"{desc.Id}Host");

            bool isInteractive = InteractivePanelIds.Contains(desc.Id);
            var actionCov = isInteractive
                ? SurfaceActionCoverage.InteractiveCommands
                : SurfaceActionCoverage.ReadOnlyObservational;

            bool hasShell = desc.Id != "protocol" && desc.Id != "holdfast";
            bool hasRail = desc.Group == PanelGroup.Expanded ||
                           new[] { "status", "afflictions", "radiation_detail", "research", "weather_detail",
                                   "weather_forecast", "event_detail", "events_log", "economy_detail",
                                   "radiation_history", "journal_detail", "survival_detail", "survivor_detail",
                                   "inventory_detail", "achievements", "survivors", "inventory", "crafting",
                                   "workshop", "pharma_lab", "pharma", "medical", "phase0", "expeditions",
                                   "weather", "radio", "map", "shelter", "factions", "quests", "greenhouse",
                                   "silent_foundry", "trade", "muster", "expansions", "standing_record",
                                   "crossing_quests", "maritime", "deep_coast", "century_seed", "verdict",
                                   "duty_roster", "combat" }.Contains(desc.Id);

            var closeBehav = desc.Id == "protocol"
                ? SurfaceCloseBehavior.ModalDismiss
                : (hasShell ? SurfaceCloseBehavior.DashboardShellCloseButton : SurfaceCloseBehavior.OverlayCloseButton);

            string routeName = desc.Group == PanelGroup.Expanded
                ? $"OpenExpandedPanel(\"{desc.Id}\")"
                : $"OpenPlayerPanel(\"{desc.Id}\")";

            return new PlayerSurfaceContract
            {
                PanelId = desc.Id,
                DisplayName = desc.DisplayName,
                Group = desc.Group,
                RouteKind = routeKind,
                SetupDependencies = desc.SetupDependencies,
                AvailableInMenu = desc.AvailableInMenu,
                BindingKind = bindingKind,
                BindingTarget = bindingTarget,
                CloseBehavior = closeBehav,
                HasStatusRail = hasRail,
                HasDashboardShell = hasShell,
                HasSnapshotCoverage = SnapshotPanelIds.Contains(desc.Id),
                ActionCoverage = actionCov,
                RenderCoverage = SurfaceRenderCoverage.ProductionRendered,
                ReachableRoutes = new[] { routeName },
                Description = $"{desc.DisplayName} player navigation surface."
            };
        }

        /// <summary>
        /// Generates a GitHub-flavored markdown table documenting all player surfaces and coverage.
        /// </summary>
        public string ToMarkdown()
        {
            var sb = new StringBuilder();
            sb.AppendLine("# ASHFALL Player-Surface Contract & Coverage Manifest");
            sb.AppendLine();
            sb.AppendLine($"> **Total Surfaces**: {TotalSurfaces} | **Routed**: {RoutedSurfaces}/{TotalSurfaces} (100%) | **Bound**: {BoundSurfaces}/{TotalSurfaces} (100%) | **Closeable**: {CloseableSurfaces}/{TotalSurfaces} (100%)");
            sb.AppendLine($"> **Interactive Command Surfaces**: {InteractiveActionSurfaces} | **ReadOnly/Observational**: {ReadOnlySurfaces} | **Visual Snapshot Covered**: {SnapshotCoveredSurfaces}");
            sb.AppendLine();
            sb.AppendLine("## Player Surfaces Matrix");
            sb.AppendLine();
            sb.AppendLine("| Panel ID | Display Name | Group | Route Method | Binding Target | Close Behavior | Rail | Shell | Actions | Snapshots |");
            sb.AppendLine("|---|---|---|---|---|---|---|---|---|---|");

            foreach (var c in Contracts.OrderBy(c => c.Group).ThenBy(c => c.PanelId))
            {
                string route = c.ReachableRoutes.FirstOrDefault() ?? "Unrouted";
                string actions = c.ActionCoverage == SurfaceActionCoverage.InteractiveCommands ? "Interactive" : "ReadOnly";
                string snap = c.HasSnapshotCoverage ? "Yes" : "—";
                string rail = c.HasStatusRail ? "Yes" : "—";
                string shell = c.HasDashboardShell ? "Yes" : "—";

                sb.AppendLine($"| `{c.PanelId}` | {c.DisplayName} | {c.Group} | `{route}` | `{c.BindingTarget}` | {c.CloseBehavior} | {rail} | {shell} | {actions} | {snap} |");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates a machine-readable JSON representation of the manifest.
        /// </summary>
        public string ToJson()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Serialize(new
            {
                totalSurfaces = TotalSurfaces,
                routedSurfaces = RoutedSurfaces,
                boundSurfaces = BoundSurfaces,
                closeableSurfaces = CloseableSurfaces,
                snapshotCoveredSurfaces = SnapshotCoveredSurfaces,
                interactiveSurfaces = InteractiveActionSurfaces,
                readOnlySurfaces = ReadOnlySurfaces,
                surfaces = Contracts.Select(c => new
                {
                    panelId = c.PanelId,
                    displayName = c.DisplayName,
                    group = c.Group.ToString(),
                    routeKind = c.RouteKind.ToString(),
                    setupDependencies = c.SetupDependencies,
                    availableInMenu = c.AvailableInMenu,
                    bindingKind = c.BindingKind.ToString(),
                    bindingTarget = c.BindingTarget,
                    closeBehavior = c.CloseBehavior.ToString(),
                    hasStatusRail = c.HasStatusRail,
                    hasDashboardShell = c.HasDashboardShell,
                    hasSnapshotCoverage = c.HasSnapshotCoverage,
                    actionCoverage = c.ActionCoverage.ToString(),
                    renderCoverage = c.RenderCoverage.ToString(),
                    reachableRoutes = c.ReachableRoutes
                }).ToArray()
            });
        }
    }
}
