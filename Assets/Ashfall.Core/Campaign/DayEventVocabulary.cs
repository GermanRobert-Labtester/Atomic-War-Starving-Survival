// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// C2 / Plan 17A-S — semantic-vocabulary classification for day events.
    ///
    /// The re-audit of Plan 17 found that <see cref="DailyBriefingReportBuilder"/>
    /// handled only a subset of the kinds producers actually emit, and its closed
    /// switch silently discarded the rest. This vocabulary is the C2 consumer-side
    /// repair: every emitted kind is either
    ///
    ///   1. explicitly handled by a builder case (player-facing, tailored text),
    ///   2. classified as an internal heartbeat (intentionally non-player-facing —
    ///      steady-state owner ticks that would flood the briefing noise budget),
    ///   or 3. rendered through the documented generic representation
    ///      (<see cref="GenericSectionTitle"/> / <see cref="RenderGeneric"/>) so a
    ///      valid producer event can never silently disappear.
    ///
    /// This is the Plan 31 semantic-kind authority and C2 consumer contract.
    /// Every registered event kind maps to exactly one non-unknown <see cref="SemanticKind"/>.
    /// Classification is deterministic: no RNG, no dictionary-order dependence.
    /// </summary>
    public static class DayEventVocabulary
    {
        /// <summary>Section title for generically rendered non-heartbeat kinds.</summary>
        public const string GenericSectionTitle = "System Activity";

        /// <summary>
        /// Total Plan 31 semantic kind mapping covering all registered day-event kinds.
        /// Statically defined, deterministic, and enforced by totality unit tests.
        /// </summary>
        private static readonly Dictionary<string, SemanticKind> SemanticKindMap = new(StringComparer.Ordinal)
        {
            // ── Heartbeats (Internal steady-state simulation ticks) ──
            { "aeroponics_ticked", SemanticKind.Heartbeat },
            { "aquaponics_ticked", SemanticKind.Heartbeat },
            { "backstory_ticked", SemanticKind.Heartbeat },
            { "campaign_legacy_ticked", SemanticKind.Heartbeat },
            { "cooking_ticked", SemanticKind.Heartbeat },
            { "cryo_vault_ticked", SemanticKind.Heartbeat },
            { "debt_ledger_ticked", SemanticKind.Heartbeat },
            { "duty_roster_ticked", SemanticKind.Heartbeat },
            { "espionage_ticked", SemanticKind.Heartbeat },
            { "events_evaluated", SemanticKind.Heartbeat },
            { "expedition_ticked", SemanticKind.Heartbeat },
            { "expeditions_ticked", SemanticKind.Heartbeat },
            { "flagship_institutions_ticked", SemanticKind.Heartbeat },
            { "fluid_logistics_ticked", SemanticKind.Heartbeat },
            { "geothermal_orc_ticked", SemanticKind.Heartbeat },
            { "greenhouse_foundry_ticked", SemanticKind.Heartbeat },
            { "holdfast_ticked", SemanticKind.Heartbeat },
            { "ideological_friction_ticked", SemanticKind.Heartbeat },
            { "romance_family_ticked", SemanticKind.Heartbeat },
            { "vehicle_customization_ticked", SemanticKind.Heartbeat },
            { "shelter_governance_ticked", SemanticKind.Heartbeat },
            { "journal_ticked", SemanticKind.Heartbeat },
            { "maritime_ticked", SemanticKind.Heartbeat },
            { "market_ticked", SemanticKind.Heartbeat },
            { "medical_disease_ticked", SemanticKind.Heartbeat },
            { "meta_progression_ticked", SemanticKind.Heartbeat },
            { "tunnel_network_ticked", SemanticKind.Heartbeat },
            { "shelter_identity_ticked", SemanticKind.Heartbeat },
            { "morale_contagion_ticked", SemanticKind.Heartbeat },
            { "narrative_ticked", SemanticKind.Heartbeat },
            { "needs_ticked", SemanticKind.Heartbeat },
            { "needs_performance_ticked", SemanticKind.Heartbeat },
            { "npc_memory_ticked", SemanticKind.Heartbeat },
            { "pneumatic_dispatch_ticked", SemanticKind.Heartbeat },
            { "power_ticked", SemanticKind.Heartbeat },
            { "precision_metrology_ticked", SemanticKind.Heartbeat },
            { "procedural_narrative_ticked", SemanticKind.Heartbeat },
            { "psychology_arcs_ticked", SemanticKind.Heartbeat },
            { "psyops_ticked", SemanticKind.Heartbeat },
            { "radio_program_production_ticked", SemanticKind.Heartbeat },
            { "research_ticked", SemanticKind.Heartbeat },
            { "research_unlock_ticked", SemanticKind.Heartbeat },
            { "sanitation_ticked", SemanticKind.Heartbeat },
            { "seismic_geology_ticked", SemanticKind.Heartbeat },
            { "shelter_facilities_ticked", SemanticKind.Heartbeat },
            { "shelter_fire_ticked", SemanticKind.Heartbeat },
            { "subterranean_ticked", SemanticKind.Heartbeat },
            { "survivor_social_ticked", SemanticKind.Heartbeat },
            { "survivors_ticked", SemanticKind.Heartbeat },
            { "territory_control_ticked", SemanticKind.Heartbeat },
            { "trapping_ticked", SemanticKind.Heartbeat },
            { "underworld_ticked", SemanticKind.Heartbeat },
            { "unified_ending_ticked", SemanticKind.Heartbeat },
            { "world_evolution_ticked", SemanticKind.Heartbeat },
            { "world_ticked", SemanticKind.Heartbeat },

            // ── Casualties ──
            { "survivor_perished", SemanticKind.Casualty },
            { "child_lost", SemanticKind.Casualty },

            // ── Hazards & Warnings ──
            { "hazard_warning", SemanticKind.Hazard },
            { "cascade_warning", SemanticKind.Hazard },
            { "power_critical_deficit", SemanticKind.Hazard },
            { "power_brownout_began", SemanticKind.Hazard },
            { "shelter_filter_degraded", SemanticKind.Hazard },
            { "shelter_hatch_unsealed", SemanticKind.Hazard },
            { "subterranean_cave_in", SemanticKind.Hazard },
            { "subterranean_flood_warning", SemanticKind.Hazard },
            { "subterranean_methane_warning", SemanticKind.Hazard },
            { "subterranean_shoring_warning", SemanticKind.Hazard },
            { "social_dispute_unresolved", SemanticKind.Hazard },
            { "social_privacy_warning", SemanticKind.Hazard },
            { "sanitation_spill", SemanticKind.Hazard },
            { "weather_unexpected_storm", SemanticKind.Hazard },

            // ── Survivors & Crew ──
            { "ate", SemanticKind.Survivor },
            { "drank", SemanticKind.Survivor },
            { "med_taken", SemanticKind.Survivor },
            { "meal_served", SemanticKind.Survivor },
            { "consumed_rations", SemanticKind.Survivor },
            { "consumed_child_rations", SemanticKind.Survivor },
            { "contaminated_meal", SemanticKind.Survivor },
            { "portions_spoiled", SemanticKind.Survivor },
            { "child_born", SemanticKind.Survivor },
            { "child_aged", SemanticKind.Survivor },
            { "generation_advanced", SemanticKind.Survivor },
            { "survivor_condition", SemanticKind.Survivor },
            { "duty_vacated", SemanticKind.Survivor },
            { "medical_admitted", SemanticKind.Survivor },
            { "medical_discharged", SemanticKind.Survivor },
            { "memorial_checked", SemanticKind.Survivor },

            // ── Shelter & Infrastructure ──
            { "power_shed_automatic", SemanticKind.Shelter },
            { "power_shed_player", SemanticKind.Shelter },
            { "power_brownout_restored", SemanticKind.Shelter },
            { "cascade_recovered", SemanticKind.Shelter },
            { "shelter_consequence", SemanticKind.Shelter },
            { "shelter_decon_started", SemanticKind.Shelter },
            { "shelter_decon_completed", SemanticKind.Shelter },
            { "shelter_decor_morale", SemanticKind.Shelter },
            { "sanitation_disease_sweep", SemanticKind.Shelter },
            { "nuclear_generation_published", SemanticKind.Shelter },
            { "workshop_job_completed", SemanticKind.Shelter },
            { "workshop_machine_degraded", SemanticKind.Shelter },
            { "workshop_machine_overhauled", SemanticKind.Shelter },

            // ── Production & Economy ──
            { "crafting_completed", SemanticKind.Production },
            { "crafting_production", SemanticKind.Production },
            { "resource_delta", SemanticKind.Production },
            { "trapping_harvest", SemanticKind.Production },
            { "market_shocks_active", SemanticKind.Production },

            // ── Expeditions & Rescues ──
            { "expedition_milestone", SemanticKind.Expedition },
            { "expeditions_caravans_ticked", SemanticKind.Expedition },
            { "subterranean_rescue_active", SemanticKind.Expedition },
            { "subterranean_rescue_completed", SemanticKind.Expedition },
            { "subterranean_rescue_failed", SemanticKind.Expedition },

            // ── Communications & Signals ──
            { "radio_intercept", SemanticKind.Communication },
            { "radio_intercept_decrypted", SemanticKind.Communication },
            { "radio_location_triangulated", SemanticKind.Communication },
            { "radio_transmission", SemanticKind.Communication },
            { "radio_distress_active", SemanticKind.Communication },
            { "radio_distress_expiring", SemanticKind.Communication },
            { "radio_program_production_active_delta", SemanticKind.Communication },

            // ── Weather & Atmosphere ──
            { "weather_condition", SemanticKind.Weather },
            { "weather_forecast_miss", SemanticKind.Weather },
            { "weather_ticked", SemanticKind.Weather },

            // ── Narrative & Story ──
            { "echo_surfaced", SemanticKind.Narrative },
            { "echo_consequence_due", SemanticKind.Narrative },
            { "narrative_arc_selected", SemanticKind.Narrative },
            { "personal_quest_progressed", SemanticKind.Narrative },
            { "obligation_warning", SemanticKind.Narrative },
            { "obligation_missed", SemanticKind.Narrative },
            { "obligation_met", SemanticKind.Narrative },
            { "social_dispute_mediated", SemanticKind.Narrative },

            // ── Settlement Expansion & Records (Plans 55 / 58) ──
            { "outpost_network_ticked", SemanticKind.Heartbeat },
            { "retention_ticked", SemanticKind.Heartbeat },
            { "weather_cascade_ticked", SemanticKind.Heartbeat }
        };

        /// <summary>Read-only view of the static semantic kind mappings.</summary>
        public static IReadOnlyDictionary<string, SemanticKind> AllSemanticMappings => SemanticKindMap;

        /// <summary>
        /// Resolves the semantic kind for an event kind identifier.
        /// Returns <see cref="SemanticKind.Heartbeat"/> for steady-state ticks,
        /// the registered semantic kind for recognized events, or <see cref="SemanticKind.Unknown"/> if unclassified.
        /// </summary>
        public static SemanticKind GetSemanticKind(string? kind)
        {
            if (string.IsNullOrEmpty(kind)) return SemanticKind.Unknown;
            if (SemanticKindMap.TryGetValue(kind, out var sem)) return sem;
            if (IsInternalHeartbeat(kind)) return SemanticKind.Heartbeat;
            return SemanticKind.Unknown;
        }

        /// <summary>
        /// Extension helper resolving the semantic kind directly on a <see cref="DayStateChangeEvent"/>.
        /// </summary>
        public static SemanticKind GetSemanticKind(this DayStateChangeEvent? evt)
        {
            return evt == null ? SemanticKind.Unknown : GetSemanticKind(evt.Kind);
        }

        /// <summary>
        /// Attempts to resolve the semantic kind for an event kind.
        /// Returns true if classified into a valid domain category (non-unknown).
        /// </summary>
        public static bool TryGetSemanticKind(string? kind, out SemanticKind result)
        {
            result = GetSemanticKind(kind);
            return result != SemanticKind.Unknown;
        }

        /// <summary>
        /// Kinds that do not end in the heartbeat suffix but are still
        /// internal aggregation markers, intentionally not shown to players.
        /// Documented and tested as intentionally retained.
        /// </summary>
        private static readonly HashSet<string> InternalKinds = new(StringComparer.Ordinal)
        {
            "events_evaluated", // protagonist/narrative aggregate heartbeat
            "world_ticked",     // world-simulation aggregate heartbeat
        };

        /// <summary>
        /// True when the kind is a steady-state owner heartbeat. Heartbeats are
        /// intentionally classified as non-player-facing: rendering every daily
        /// tick would breach the briefing noise budget (C2 §42.1).
        /// Follows the Plan 31 SemanticKind authority.
        /// </summary>
        public static bool IsInternalHeartbeat(string? kind)
        {
            if (string.IsNullOrEmpty(kind)) return true;
            if (InternalKinds.Contains(kind)) return true;
            if (kind.EndsWith("_ticked", StringComparison.Ordinal))
            {
                // Handled player-facing exceptions to the _ticked suffix convention:
                if (string.Equals(kind, "weather_ticked", StringComparison.Ordinal) ||
                    string.Equals(kind, "expeditions_caravans_ticked", StringComparison.Ordinal))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deterministic generic rendering for an unhandled, non-heartbeat event.
        /// Shows kind, source owner, entities, and numeric payload so the player
        /// sees that something happened and where it came from — never silently
        /// dropped.
        /// </summary>
        public static string RenderGeneric(DayStateChangeEvent evt)
        {
            if (evt == null) return string.Empty;
            string kind = string.IsNullOrEmpty(evt.Kind) ? "unknown" : evt.Kind;
            string owner = string.IsNullOrEmpty(evt.SourceOwnerId) ? "system" : evt.SourceOwnerId;
            string text = $"{kind.Replace('_', ' ')} ({owner}";
            if (!string.IsNullOrEmpty(evt.PrimaryId)) text += $": {evt.PrimaryId}";
            text += ")";
            if (!string.IsNullOrEmpty(evt.SecondaryId)) text += $" — {evt.SecondaryId}";
            if (evt.Numeric != 0f) text += $" [{evt.Numeric:F0}]";
            return text;
        }

        /// <summary>
        /// Closed section-title routing for semantic kinds (D11-B vocabulary).
        /// A kind with no entry renders under GenericSectionTitle.
        /// </summary>
        private static readonly Dictionary<SemanticKind, string> SectionTitleMap =
            new()
            {
                { SemanticKind.Hazard, "Warnings" },
                { SemanticKind.Casualty, "Deaths" },
                { SemanticKind.Survivor, "Survivor Changes" },
                { SemanticKind.Shelter, "Shelter" },
                { SemanticKind.Communication, "Radio Intercepts" },
                { SemanticKind.Weather, "Weather Forecast" },
                { SemanticKind.Narrative, "Chronicle" },
                { SemanticKind.Production, "Production & Maintenance" },
                { SemanticKind.Expedition, "Expedition Milestones" },
            };

        public static string SectionTitleFor(SemanticKind kind) =>
            SectionTitleMap.TryGetValue(kind, out var title) ? title : GenericSectionTitle;
    }
}
