using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class DailyBriefingEventDerivedTests
    {
        [Fact]
        public void QuietDay_ProducesMinimalBriefing_WithZeroFabricatedRadioMessages()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("weather_condition", "weather_world", "Clear", "Low radiation background", 0f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 2, buildSeed: 2, events: events);

            Assert.NotNull(report);
            Assert.Equal("DAY 2 BRIEFING", report.Title);
            Assert.Equal(string.Empty, report.GeneratedUtc);
            Assert.Single(report.Sections);
            Assert.Equal("Weather Forecast", report.Sections[0].Title);

            // Radio Intercepts must NOT be fabricated on quiet days
            Assert.DoesNotContain(report.Sections, s => s.Title == "Radio Intercepts");
        }

        [Fact]
        public void ScarcityDay_CategorizesRationDepletion_AndWarningFlags()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("consumed_rations", "starting_level_rations", "canned_food", null, 3f),
                new DayStateChangeEvent("consumed_rations", "starting_level_rations", "clean_water", null, 3f),
                new DayStateChangeEvent("shelter_consequence", "inventory", "inventory", "Food reserves critically depleted", 0f),
                new DayStateChangeEvent("survivor_condition", "survivors_needs", "survivor_1", "starving", 85f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 4, buildSeed: 4, events: events);

            Assert.NotNull(report);
            var sectionTitles = report.Sections.Select(s => s.Title).ToList();

            Assert.Contains("Warnings", sectionTitles);
            Assert.Contains("Resource Consumption", sectionTitles);

            var warnings = report.Sections.First(s => s.Title == "Warnings");
            Assert.Contains(warnings.Entries, e => e.PrimaryId == "inventory");
            Assert.Contains(warnings.Entries, e => e.PrimaryId == "survivor_1");

            var resources = report.Sections.First(s => s.Title == "Resource Consumption");
            Assert.Contains(resources.Entries, e => e.PrimaryId == "canned_food" && e.Numeric == 3f);
            Assert.Contains(resources.Entries, e => e.PrimaryId == "clean_water" && e.Numeric == 3f);
        }

        [Fact]
        public void SurvivorDeathDay_PrioritizesDeathsSection_FirstInSeverityOrder()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("consumed_rations", "starting_level_rations", "canned_food", null, 2f),
                new DayStateChangeEvent("survivor_perished", "survivor_2", "survivor_2", "severe acute radiation syndrome", 0f),
                new DayStateChangeEvent("weather_condition", "weather_world", "Fallout Storm", "High radiation", 0f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 7, buildSeed: 7, events: events);

            Assert.NotNull(report);
            Assert.True(report.Sections.Count >= 3);

            // Deaths must be the very first section in severity ordering
            Assert.Equal("Deaths", report.Sections[0].Title);
            Assert.Equal("survivor_2", report.Sections[0].Entries[0].PrimaryId);
        }

        [Fact]
        public void StormWeatherDay_CapturesWeatherForecast_AndHazardWarnings()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("weather_condition", "weather_world", "Black Rain Storm", "Extreme particulate toxicity", 0f),
                new DayStateChangeEvent("hazard_warning", "atmospheric_filter", "Filter saturation exceeded 90%", null, 92f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 12, buildSeed: 12, events: events);

            Assert.NotNull(report);
            Assert.Contains(report.Sections, s => s.Title == "Warnings");
            Assert.Contains(report.Sections, s => s.Title == "Weather Forecast");

            var weather = report.Sections.First(s => s.Title == "Weather Forecast");
            Assert.Contains("Black Rain Storm", weather.Entries[0].Text);
        }

        [Fact]
        public void ExpeditionReturnDay_ContainsMilestoneDetails_WithoutFakes()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("expedition_milestone", "expeditions_caravans", "expedition_iron_cache", "Returned with 12 scrap and 4 medical supplies.", 0f),
                new DayStateChangeEvent("radio_intercept", "radio_repeater", "relay_ch_9", "Broadcast signal intercepted: 'Survivors at Crater Ridge'.", 0f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 15, buildSeed: 15, events: events);

            Assert.NotNull(report);
            Assert.Contains(report.Sections, s => s.Title == "Expedition Milestones");
            Assert.Contains(report.Sections, s => s.Title == "Radio Intercepts");

            var exp = report.Sections.First(s => s.Title == "Expedition Milestones");
            Assert.Equal("expedition_iron_cache", exp.Entries[0].PrimaryId);
            Assert.Contains("Returned", exp.Entries[0].Text);
        }

        [Fact]
        public void MultiEventDay_DeduplicatesAndAppliesOverflowTruncation()
        {
            var events = new List<DayStateChangeEvent>();

            // Generate 15 duplicate/distinct survivor condition events to trigger deduplication + overflow
            for (int i = 0; i < 15; i++)
            {
                events.Add(new DayStateChangeEvent("survivor_condition", "survivors_needs", $"survivor_{i + 1}", "hungry", 50f));
            }
            // Add a direct duplicate
            events.Add(new DayStateChangeEvent("survivor_condition", "survivors_needs", "survivor_1", "hungry", 50f));

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 20, buildSeed: 20, events: events, maxEntriesPerSection: 5);

            Assert.NotNull(report);
            var section = report.Sections.First(s => s.Title == "Survivor Changes");

            // Max entries (5) + 1 overflow entry = 6 total entries in the section
            Assert.Equal(6, section.Entries.Length);
            Assert.Equal("overflow", section.Entries[5].PrimaryId);
            Assert.Contains("more items", section.Entries[5].Text);
        }

        [Fact]
        public void WorkshopJobsAndTooling_AppearsInProductionAndWarnings()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("workshop_job_completed", "shelter_workshop", "9x19mm FMJ Reload", "room_workshop", 40f),
                new DayStateChangeEvent("workshop_machine_degraded", "shelter_workshop", "room_workshop", null, 0.20f),
                new DayStateChangeEvent("workshop_machine_overhauled", "shelter_workshop", "room_workshop", null, 1.0f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 8, buildSeed: 8, events: events);

            Assert.NotNull(report);
            var prod = report.Sections.FirstOrDefault(s => s.Title == "Production & Maintenance");
            var warn = report.Sections.FirstOrDefault(s => s.Title == "Warnings");

            Assert.NotNull(prod);
            Assert.NotNull(warn);
            Assert.Contains(prod.Entries, e => e.Text.Contains("40 units"));
            Assert.Contains(prod.Entries, e => e.Text.Contains("overhaul completed"));
            Assert.Contains(warn.Entries, e => e.Text.Contains("tooling degraded"));
        }

        [Fact]
        public void RadioDistressAndDecryption_AppearsInBriefingSections()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("radio_intercept_decrypted", "radio_station", "MERIDIAN-ACT-7", "Convoy supply manifest decoded", 0f),
                new DayStateChangeEvent("radio_distress_active", "radio_station", "SHELTER-44-SOS", null, 3f),
                new DayStateChangeEvent("radio_distress_expiring", "radio_station", "SHELTER-44-SOS", null, 1f),
                new DayStateChangeEvent("radio_location_triangulated", "radio_station", "MERIDIAN-ACT-7", "loc_diesel_tank_farm", 0f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 16, buildSeed: 16, events: events);

            Assert.NotNull(report);
            var radioSec = report.Sections.FirstOrDefault(s => s.Title == "Radio Intercepts");
            var warnSec = report.Sections.FirstOrDefault(s => s.Title == "Warnings");

            Assert.NotNull(radioSec);
            Assert.NotNull(warnSec);
            Assert.Contains(radioSec.Entries, e => e.PrimaryId == "MERIDIAN-ACT-7");
            Assert.Contains(radioSec.Entries, e => e.Text.Contains("loc_diesel_tank_farm"));
            Assert.Contains(warnSec.Entries, e => e.Text.Contains("expires in 1 day"));
        }

        [Fact]
        public void ShelterSocialFrictionAndMediation_AppearsInShelterSocialSection()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("social_privacy_warning", "shelter_social_dynamics", "survivor_1", "room_dormitory", 780f),
                new DayStateChangeEvent("social_dispute_unresolved", "shelter_social_dynamics", "inc_bunk_noise", "room_dormitory", 0f),
                new DayStateChangeEvent("social_dispute_mediated", "shelter_social_dynamics", "inc_bunk_noise", "survivor_leader", 0f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 6, buildSeed: 6, events: events);

            Assert.NotNull(report);
            var socialSec = report.Sections.FirstOrDefault(s => s.Title == "Shelter Social");
            var warnSec = report.Sections.FirstOrDefault(s => s.Title == "Warnings");

            Assert.NotNull(socialSec);
            Assert.NotNull(warnSec);
            Assert.Contains(warnSec.Entries, e => e.Text.Contains("privacy fatigue"));
            Assert.Contains(socialSec.Entries, e => e.Text.Contains("Unresolved dispute"));
            Assert.Contains(socialSec.Entries, e => e.Text.Contains("mediated"));
        }

        [Fact]
        public void SubterraneanHazardsAndRescues_AppearsInSubterraneanOperationsAndDeaths()
        {
            var events = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("subterranean_methane_warning", "excavation_hazards", "sector_alpha", null, 3200f),
                new DayStateChangeEvent("subterranean_flood_warning", "excavation_hazards", "sector_alpha", null, 600f),
                new DayStateChangeEvent("subterranean_shoring_warning", "excavation_hazards", "sector_alpha", null, 250f),
                new DayStateChangeEvent("subterranean_cave_in", "excavation_hazards", "sector_alpha", null, 2f),
                new DayStateChangeEvent("subterranean_rescue_active", "excavation_hazards", "sector_alpha", null, 180f),
                new DayStateChangeEvent("subterranean_rescue_completed", "excavation_hazards", "sector_alpha", null, 0f),
                new DayStateChangeEvent("subterranean_rescue_failed", "excavation_hazards", "sector_beta", null, 0f)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(day: 19, buildSeed: 19, events: events);

            Assert.NotNull(report);
            var subSec = report.Sections.FirstOrDefault(s => s.Title == "Subterranean Operations");
            var warnSec = report.Sections.FirstOrDefault(s => s.Title == "Warnings");
            var deathSec = report.Sections.FirstOrDefault(s => s.Title == "Deaths");

            Assert.NotNull(subSec);
            Assert.NotNull(warnSec);
            Assert.NotNull(deathSec);
            Assert.Contains(warnSec.Entries, e => e.Text.Contains("Cave-in collapse"));
            Assert.Contains(warnSec.Entries, e => e.Text.Contains("3200 PPM"));
            Assert.Contains(subSec.Entries, e => e.Text.Contains("Rescue underway"));
            Assert.Contains(subSec.Entries, e => e.Text.Contains("completed successfully"));
            Assert.Contains(deathSec.Entries, e => e.Text.Contains("Rescue in sector_beta failed"));
        }
    }
}
