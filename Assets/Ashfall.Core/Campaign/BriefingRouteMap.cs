// SPDX-License-Identifier: MIT
namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Plan 31B — one authority mapping day-event kinds / briefing categories to
    /// a live player-panel route id.
    ///
    /// Core carries the route STRING only (<c>panel:&lt;id&gt;</c>); it never owns
    /// Godot objects. The host validates the target against the panel registry
    /// (Plan 16 liveness contract) before navigating and falls back to the
    /// informational presentation when the target is not live. If no useful live
    /// surface exists the route is <c>null</c> and the entry is informational —
    /// no dead affordance is invented (31B.6).
    /// </summary>
    public static class BriefingRouteMap
    {
        public const string PanelPrefix = "panel:";

        /// <summary>Preferred route for a concrete day-event kind.
        /// Unrecognized kinds fall back to the semantic-kind mapping.</summary>
        public static string? RouteFor(string? kind)
        {
            if (string.IsNullOrEmpty(kind)) return null;
            switch (kind)
            {
                // Casualty / medical
                case "survivor_perished":
                case "child_lost": return Panel("survivor_detail");
                case "gangrene_warning":
                case "amputation_performed":
                case "prosthetic_fitted":
                case "phantom_pain_episode": return Panel("medical");
                case "medical_admitted":
                case "medical_discharged": return Panel("medical_ward");
                // Shelter / infrastructure
                case "power_critical_deficit":
                case "power_brownout_began":
                case "power_shed_automatic": return Panel("power_grid");
                case "shelter_filter_degraded": return Panel("water_treatment");
                case "sanitation_spill": return Panel("sanitation");
                case "shelter_consequence":
                case "hazard_warning":
                case "cascade_warning": return Panel("shelter");
                // Production / economy
                case "market_shocks_active": return Panel("economy_detail");
                // Communication
                case "radio_transmission":
                case "radio_broadcast": return Panel("radio");
                default: return RouteForSemantic(DayEventVocabulary.GetSemanticKind(kind));
            }
        }

        /// <summary>Semantic-kind fallback route (31B.5).</summary>
        public static string? RouteForSemantic(SemanticKind kind)
        {
            switch (kind)
            {
                case SemanticKind.Casualty: return Panel("survivor_detail");
                case SemanticKind.Hazard: return Panel("shelter");
                case SemanticKind.Survivor: return Panel("survivor_detail");
                case SemanticKind.Shelter: return Panel("shelter");
                case SemanticKind.Production: return Panel("inventory");
                case SemanticKind.Expedition: return Panel("expeditions");
                case SemanticKind.Communication: return Panel("radio");
                case SemanticKind.Weather: return Panel("weather");
                case SemanticKind.Narrative: return Panel("journal");
                default: return null; // Heartbeat / Unknown → informational
            }
        }

        /// <summary>Route derived from an existing briefing section category,
        /// so handled entries become actionable without per-case wiring.</summary>
        public static string? RouteForCategory(string? category)
        {
            if (string.IsNullOrEmpty(category)) return null;
            switch (category)
            {
                case "Deaths": return Panel("journal");
                case "Warnings":
                case "Hazards & Warnings":
                case "Shelter": return Panel("shelter");
                case "Critical Alerts": return Panel("emergency_response");
                case "Survivor Changes": return Panel("survivor_detail");
                case "Settlement Morale":
                case "Shelter Social": return Panel("survivor_relations");
                case "Resource Consumption": return Panel("inventory");
                case "Production & Maintenance": return Panel("crafting");
                case "Weather Forecast": return Panel("weather");
                case "Radio Intercepts": return Panel("radio");
                case "Expedition Milestones": return Panel("expeditions");
                case "Intelligence & Recon":
                case "Subterranean Operations": return Panel("map");
                default: return null; // e.g. "System Activity" → informational
            }
        }

        /// <summary>
        /// Fill <see cref="DailyBriefingEntry.DeepLinkRoute"/> and
        /// <see cref="DailyBriefingEntry.IsActionable"/> across a report,
        /// preferring the entry's concrete kind, then its category. Existing
        /// routes (e.g. from <see cref="BriefingFact"/>) are preserved.
        /// Idempotent — safe to call on an already-routed report.
        /// </summary>
        public static void ApplyRoutes(DailyBriefingReport? report)
        {
            if (report?.Sections == null) return;
            for (int s = 0; s < report.Sections.Count; s++)
            {
                var entries = report.Sections[s]?.Entries;
                if (entries == null) continue;
                for (int e = 0; e < entries.Length; e++)
                {
                    var entry = entries[e];
                    if (entry == null) continue;
                    if (string.IsNullOrEmpty(entry.DeepLinkRoute))
                    {
                        entry.DeepLinkRoute =
                            RouteFor(entry.Kind)
                            ?? RouteForCategory(entry.Category)
                            ?? string.Empty;
                    }
                    entry.IsActionable = !string.IsNullOrEmpty(entry.DeepLinkRoute);
                }
            }
        }

        private static string Panel(string id) => PanelPrefix + id;
    }
}
