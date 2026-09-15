// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// C2 / Plan 20B (§29) — shelter shielding degradation must surface through
    /// the canonical day-event vocabulary with player-legible causality:
    /// filter loss → shielding weaker; hatch unsealed → ingress risk; decon
    /// cycle start/complete. No silent drops (the parity gate in
    /// DayEventParitySourceGateTests holds emitted kinds to builder cases).
    /// </summary>
    public sealed class Plan20BShelterDayEventTests
    {
        private static DayStateChangeEvent Evt(string kind, string owner,
            string? primary = null, string? secondary = null, float numeric = 0f)
            => new(kind, owner, primary, secondary, numeric);

        [Fact]
        public void FilterDegraded_RendersWarning_WithBandAndPercent()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(12, 12, new[]
            {
                Evt("shelter_filter_degraded", "starting_level_air_filter",
                    "air_filter", "degraded", 62f)
            });

            var section = report.Sections.FirstOrDefault(s => s.Title == "Warnings");
            Assert.NotNull(section);
            var entry = Assert.Single(section!.Entries);
            Assert.Contains("Air filter", entry.Text);
            Assert.Contains("degraded", entry.Text);
            Assert.Contains("62", entry.Text);
            Assert.Contains("shielding", entry.Text);
        }

        [Fact]
        public void HatchUnsealed_RendersWarning_WithState()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(12, 12, new[]
            {
                Evt("shelter_hatch_unsealed", "airlock_security", "airlock", "Open")
            });

            var section = report.Sections.FirstOrDefault(s => s.Title == "Warnings");
            Assert.NotNull(section);
            var entry = Assert.Single(section!.Entries);
            Assert.Contains("Airlock", entry.Text);
            Assert.Contains("Open", entry.Text);
            Assert.Contains("ingress", entry.Text);
        }

        [Fact]
        public void DeconCycle_StartAndComplete_RenderShelterEntries()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(12, 12, new[]
            {
                Evt("shelter_decon_started", "decontamination"),
                Evt("shelter_decon_completed", "decontamination")
            });

            var shelter = report.Sections.FirstOrDefault(s => s.Title == "Shelter");
            Assert.NotNull(shelter);
            Assert.Equal(2, shelter!.Entries.Length);
            Assert.Contains(shelter.Entries, e => e.Text.Contains("started"));
            Assert.Contains(shelter.Entries, e => e.Text.Contains("complete"));
        }

        [Fact]
        public void NewKinds_NeverFallThroughToGenericRendering()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(12, 12, new[]
            {
                Evt("shelter_filter_degraded", "starting_level_air_filter", "air_filter", "critical", 10f),
                Evt("shelter_hatch_unsealed", "airlock_security", "airlock", "Breached"),
                Evt("shelter_decon_started", "decontamination"),
                Evt("shelter_decon_completed", "decontamination")
            });

            Assert.DoesNotContain(report.Sections,
                s => s.Title == DayEventVocabulary.GenericSectionTitle);
        }

        [Fact]
        public void Rendering_IsDeterministic()
        {
            var events = new[]
            {
                Evt("shelter_filter_degraded", "starting_level_air_filter", "air_filter", "degraded", 62f),
                Evt("shelter_decon_started", "decontamination")
            };

            string Render()
            {
                var report = DailyBriefingReportBuilder.BuildFromDayEvents(5, 5, events);
                return string.Join("|", report.Sections
                    .SelectMany(s => s.Entries.Select(e => $"{s.Title}::{e.Text}")));
            }

            Assert.Equal(Render(), Render());
        }
    }
}