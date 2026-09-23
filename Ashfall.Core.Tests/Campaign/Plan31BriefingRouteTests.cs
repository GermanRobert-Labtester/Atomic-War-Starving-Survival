// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// Plan 31B — navigable briefings. One Core kind/category → live-route
    /// authority; every produced route must resolve to a player-navigable panel
    /// (Plan 16 liveness); unmapped content is explicitly informational.
    /// </summary>
    public sealed class Plan31BriefingRouteTests
    {
        public Plan31BriefingRouteTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        private static string IdOf(string route) => route.StartsWith(BriefingRouteMap.PanelPrefix)
            ? route.Substring(BriefingRouteMap.PanelPrefix.Length)
            : route;

        private static IEnumerable<string> AllProducedRoutes()
        {
            foreach (string kind in new[]
            {
                "survivor_perished", "child_lost", "gangrene_warning", "amputation_performed",
                "prosthetic_fitted", "phantom_pain_episode", "medical_admitted", "medical_discharged",
                "power_critical_deficit", "power_brownout_began", "power_shed_automatic",
                "shelter_filter_degraded", "sanitation_spill", "shelter_consequence",
                "hazard_warning", "cascade_warning", "market_shocks_active",
                "radio_transmission", "radio_broadcast"
            })
            {
                var r = BriefingRouteMap.RouteFor(kind);
                if (r != null) yield return r;
            }
            foreach (SemanticKind k in System.Enum.GetValues<SemanticKind>())
            {
                var r = BriefingRouteMap.RouteForSemantic(k);
                if (r != null) yield return r;
            }
            foreach (string cat in new[]
            {
                "Deaths", "Warnings", "Hazards & Warnings", "Shelter", "Critical Alerts",
                "Survivor Changes", "Settlement Morale", "Shelter Social", "Resource Consumption",
                "Production & Maintenance", "Weather Forecast", "Radio Intercepts",
                "Expedition Milestones", "Intelligence & Recon", "Subterranean Operations"
            })
            {
                var r = BriefingRouteMap.RouteForCategory(cat);
                if (r != null) yield return r;
            }
        }

        [Fact]
        public void EveryProducedRoute_ResolvesToALivePlayerPanel()
        {
            foreach (string route in AllProducedRoutes())
            {
                Assert.StartsWith(BriefingRouteMap.PanelPrefix, route);
                string id = IdOf(route);
                Assert.True(PanelRegistry.IsRegistered(id), $"route target '{id}' is not registered");
                Assert.True(PanelRegistry.Get(id)!.IsPlayerNavigable,
                    $"route target '{id}' is not a live player-navigable panel");
            }
        }

        [Fact]
        public void GenericNonHeartbeatEvent_BecomesActionable()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(3, 3,
                new[] { new DayStateChangeEvent("sanitation_spill", "sanitation", "room_cistern") });

            // Plan 31 / D11-B closed-section routing: sanitation_spill is a Hazard event,
            // routed to its semantic section ("Warnings").
            var expectedSection = DayEventVocabulary.SectionTitleFor(DayEventVocabulary.GetSemanticKind("sanitation_spill"));
            var entry = Assert.Single(report.Sections, s => s.Title == expectedSection).Entries[0];
            Assert.Equal("sanitation_spill", entry.Kind);
            Assert.True(entry.IsActionable);
            Assert.Equal("panel:sanitation", entry.DeepLinkRoute);
        }

        [Fact]
        public void UnknownKind_IsInformational_NotADeadAffordance()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(3, 3,
                new[] { new DayStateChangeEvent("unique_kind_zzz", "owner", "entity") });

            var entry = Assert.Single(report.Sections, s => s.Title == DayEventVocabulary.GenericSectionTitle).Entries[0];
            Assert.False(entry.IsActionable);
            Assert.Equal(string.Empty, entry.DeepLinkRoute);
        }

        [Fact]
        public void HandledDeath_RoutesToTheLiveSurvivorLog()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(6, 6,
                new[] { new DayStateChangeEvent("survivor_perished", "survivor_fate", "survivor_ivan") });

            var entry = Assert.Single(report.Sections, s => s.Title == "Deaths").Entries[0];
            Assert.True(entry.IsActionable);
            // The memorial panel is a Prototype; 31B.4 routes to the live parent.
            Assert.Equal("panel:journal", entry.DeepLinkRoute);
        }

        [Fact]
        public void ApplyRoutes_IsIdempotent_AndPreservesExistingRoutes()
        {
            var entry = new DailyBriefingEntry("System Activity", "x", "text");
            entry.DeepLinkRoute = "panel:journal";
            var report = new DailyBriefingReport { Day = 1 };
            report.Sections.Add(new DailyBriefingSection("System Activity", new List<DailyBriefingEntry> { entry }));

            BriefingRouteMap.ApplyRoutes(report);
            BriefingRouteMap.ApplyRoutes(report);

            Assert.Equal("panel:journal", entry.DeepLinkRoute); // preserved, not overwritten
            Assert.True(entry.IsActionable);
        }

        [Fact]
        public void RouteFor_NullOrEmpty_IsNull()
        {
            Assert.Null(BriefingRouteMap.RouteFor(null));
            Assert.Null(BriefingRouteMap.RouteFor(string.Empty));
        }
    }
}