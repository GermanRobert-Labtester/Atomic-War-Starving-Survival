// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// C2 / Plan 17A-S — no-silent-drop contract for the daily briefing.
    ///
    /// The re-audit disproved the original Plan 17A premise ("owners are mute"):
    /// all owners emit, but the builder's closed switch silently discarded
    /// unknown kinds. These tests pin the repaired contract:
    ///
    /// 1. every unhandled, non-heartbeat kind renders visibly (generic section),
    /// 2. heartbeat kinds are intentionally classified non-player-facing,
    /// 3. the current emitted vocabulary is fully classified (parity pin),
    /// 4. ordering/dedup is deterministic.
    /// </summary>
    public sealed class DayEventVocabularyTests
    {
        private static DayStateChangeEvent Evt(string kind, string owner = "test_owner",
            string? primary = null, string? secondary = null, float numeric = 0f)
            => new(kind, owner, primary, secondary, numeric);

        // Current emitted vocabulary from src/ producers (measured 2026-09-15;
        // re-measured by the source gate in DayEventParitySourceGateTests).
        private static readonly string[] EmittedUnhandledKinds =
        {
            "aeroponics_ticked", "aquaponics_ticked", "cryo_vault_ticked", "debt_ledger_ticked",
            "duty_roster_ticked", "espionage_ticked", "events_evaluated", "expedition_ticked",
            "expeditions_ticked", "flagship_institutions_ticked", "fluid_logistics_ticked",
            "geothermal_orc_ticked", "greenhouse_foundry_ticked", "holdfast_ticked",
            "journal_ticked", "maritime_ticked", "market_shocks_active", "market_ticked",
            "medical_disease_ticked", "memorial_checked", "morale_contagion_ticked",
            "narrative_arc_selected", "narrative_ticked", "needs_ticked",
            "nuclear_generation_published", "obligation_met", "personal_quest_progressed",
            "pneumatic_dispatch_ticked", "power_ticked", "precision_metrology_ticked",
            "procedural_narrative_ticked", "psychology_arcs_ticked", "psyops_ticked",
            "radio_program_production_active_delta", "radio_program_production_ticked",
            "research_ticked", "sanitation_disease_sweep", "sanitation_spill",
            "sanitation_ticked", "seismic_geology_ticked", "shelter_decor_morale",
            "shelter_facilities_ticked", "shelter_fire_ticked", "subterranean_ticked",
            "survivor_social_ticked", "survivors_ticked", "trapping_harvest",
            "trapping_ticked", "underworld_ticked", "world_evolution_ticked", "world_ticked"
        };

        [Fact]
        public void UnknownKind_RendersGenericEntry_NeverSilentlyDropped()
        {
            // Mapped kind routes to its semantic section
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(7, 7,
                new[] { Evt("sanitation_spill", "sanitation", "room_cistern", "spill depth 300", 300f) });

            var section = report.Sections.FirstOrDefault(s => s.Title == "Warnings");
            Assert.NotNull(section);
            var entry = Assert.Single(section!.Entries);
            Assert.Contains("sanitation spill", entry.Text);
            Assert.Contains("sanitation", entry.Text); // source owner
            Assert.Contains("room_cistern", entry.Text);

            // Truly unknown synthetic kind falls back to GenericSectionTitle
            var unmappedReport = DailyBriefingReportBuilder.BuildFromDayEvents(7, 7,
                new[] { Evt("synthetic_unmapped_event", "custom_mod", "entity_1") });
            var genericSection = unmappedReport.Sections.FirstOrDefault(s => s.Title == DayEventVocabulary.GenericSectionTitle);
            Assert.NotNull(genericSection);
            var genericEntry = Assert.Single(genericSection!.Entries);
            Assert.Contains("synthetic unmapped event", genericEntry.Text);
        }

        [Fact]
        public void HeartbeatKinds_AreIntentionallyNonPlayerFacing()
        {
            var events = EmittedUnhandledKinds.Where(k => DayEventVocabulary.IsInternalHeartbeat(k))
                .Select(k => Evt(k)).ToArray();
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(3, 3, events);

            // Heartbeats never surface as briefing sections.
            Assert.True(report.IsEmpty,
                "heartbeat kinds must be classified non-player-facing, not rendered");
        }

        [Fact]
        public void NonHeartbeatEmittedKinds_RenderVisibleEntries()
        {
            var playerFacing = EmittedUnhandledKinds.Where(k => !DayEventVocabulary.IsInternalHeartbeat(k))
                .ToList();
            Assert.NotEmpty(playerFacing);

            foreach (string kind in playerFacing)
            {
                var report = DailyBriefingReportBuilder.BuildFromDayEvents(5, 5, new[] { Evt(kind) });
                var semantic = DayEventVocabulary.GetSemanticKind(kind);
                var expectedSection = DayEventVocabulary.SectionTitleFor(semantic);
                Assert.True(report.Sections.Any(s => s.Title == expectedSection),
                    $"emitted non-heartbeat kind '{kind}' must render visibly in section '{expectedSection}', never silently drop");
            }
        }

        [Fact]
        public void EveryCurrentEmittedKind_IsHandledClassifiedOrGeneric()
        {
            foreach (string kind in EmittedUnhandledKinds)
            {
                bool classified = DayEventVocabulary.IsInternalHeartbeat(kind);
                if (classified) continue;
                var report = DailyBriefingReportBuilder.BuildFromDayEvents(1, 1, new[] { Evt(kind) });
                var semantic = DayEventVocabulary.GetSemanticKind(kind);
                var expectedSection = DayEventVocabulary.SectionTitleFor(semantic);
                Assert.True(report.Sections.Any(s => s.Title == expectedSection),
                    $"kind '{kind}' must be visible in '{expectedSection}' or classified — silent drop forbidden (C2 §6.3 / D11)");
            }
        }

        [Fact]
        public void GenericRendering_IsDeterministic()
        {
            var a = DayEventVocabulary.RenderGeneric(Evt("market_shocks_active", "economy", "food", "shock 1500", 1500f));
            var b = DayEventVocabulary.RenderGeneric(Evt("market_shocks_active", "economy", "food", "shock 1500", 1500f));
            Assert.Equal(a, b);
        }

        [Fact]
        public void GenericSection_IsCappedByOverflowRule()
        {
            var events = new List<DayStateChangeEvent>();
            for (int i = 0; i < 25; i++)
                events.Add(Evt($"unique_kind_{i}", "owner", $"entity_{i}"));

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(9, 9, events);
            var section = Assert.Single(report.Sections, s => s.Title == DayEventVocabulary.GenericSectionTitle);
            Assert.True(section!.Entries.Length <= DailyBriefingReportBuilder.DefaultMaxEntriesPerSection + 1,
                "generic section must respect the briefing noise budget (overflow indicator)");
            Assert.Contains("more items", section.Entries.Last().Text);
        }

        [Fact]
        public void SameEvents_ProduceIdenticalBriefingOrdering()
        {
            var events = new[]
            {
                Evt("sanitation_spill", "sanitation", "room_a"),
                Evt("market_shocks_active", "economy", "fuel"),
                Evt("trapping_harvest", "trapping", "pelts")
            };

            var r1 = DailyBriefingReportBuilder.BuildFromDayEvents(4, 4, events);
            var r2 = DailyBriefingReportBuilder.BuildFromDayEvents(4, 4, events);

            Assert.Equal(r1.Sections.Count, r2.Sections.Count);
            for (int i = 0; i < r1.Sections.Count; i++)
            {
                Assert.Equal(r1.Sections[i].Title, r2.Sections[i].Title);
                Assert.Equal(r1.Sections[i].Entries.Length, r2.Sections[i].Entries.Length);
                for (int j = 0; j < r1.Sections[i].Entries.Length; j++)
                    Assert.Equal(r1.Sections[i].Entries[j].Text, r2.Sections[i].Entries[j].Text);
            }
        }

        [Fact]
        public void HandledKinds_KeepTailoredRendering()
        {
            // The default case must not change existing handled behavior.
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(6, 6,
                new[] { Evt("survivor_perished", "survivor_fate", "survivor_ivan") });

            var deaths = Assert.Single(report.Sections, s => s.Title == "Deaths");
            Assert.Contains("survivor_ivan", deaths!.Entries[0].Text);
            Assert.DoesNotContain(report.Sections, s => s.Title == DayEventVocabulary.GenericSectionTitle);
        }
    }
}
