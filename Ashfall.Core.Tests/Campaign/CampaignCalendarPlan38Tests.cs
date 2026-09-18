// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignCalendarPlan38Tests
    {
        [Fact]
        public void ResolveDay_ReturnsAccurateReadModel_ForFirstThaw()
        {
            var calendar = new CampaignCalendar(initialDay: 1);
            var model = calendar.CurrentReadModel;

            Assert.Equal(1, model.Day);
            Assert.Equal("window_first_thaw", model.SeasonId);
            Assert.Equal("First Thaw", model.SeasonDisplayName);
            Assert.Equal(0, model.SeasonIndex);
            Assert.Equal(1, model.DaysIntoSeason);
            Assert.Equal(28, model.DaysToSeasonEnd);
            Assert.InRange(model.SeasonProgress, 0.01f, 0.05f);
            Assert.Equal(1, model.Year);
            Assert.Equal(1, model.Chapter);
            Assert.True(model.AmbientTemperatureC >= -3.0f && model.AmbientTemperatureC <= 3.0f);
            Assert.InRange(model.DayLengthHours, 6f, 16f);
            Assert.InRange(model.PreservationBias, 0.7f, 1.6f);
            Assert.InRange(model.MigrationBias, 0.3f, 1.4f);
        }

        [Fact]
        public void ResolveDay_IsPure_DoesNotMutateCalendarState()
        {
            var calendar = new CampaignCalendar(initialDay: 10);
            Assert.Equal(10, calendar.CurrentDay);

            var model100 = calendar.ResolveDay(100);
            Assert.Equal(100, model100.Day);
            Assert.Equal(10, calendar.CurrentDay); // Still 10!
        }

        [Fact]
        public void MultiYear_CalculatesYearAndChapterMonotonically()
        {
            var calendar = new CampaignCalendar(initialDay: 1);

            var modelYear1 = calendar.ResolveDay(1);
            Assert.Equal(1, modelYear1.Year);
            Assert.Equal(1, modelYear1.Chapter);

            var modelYear1End = calendar.ResolveDay(365);
            Assert.Equal(1, modelYear1End.Year);
            Assert.Equal(1, modelYear1End.Chapter);

            var modelYear2Start = calendar.ResolveDay(366);
            Assert.Equal(2, modelYear2Start.Year);
            Assert.Equal(2, modelYear2Start.Chapter);
            Assert.Equal("window_first_thaw", modelYear2Start.SeasonId);

            var modelYear3Start = calendar.ResolveDay(731);
            Assert.Equal(3, modelYear3Start.Year);
            Assert.Equal(3, modelYear3Start.Chapter);
        }

        [Fact]
        public void OnSeasonChanged_FiresOnBoundaryCrossing_AndNotOnSameSeasonDays()
        {
            var calendar = new CampaignCalendar(initialDay: 28);
            var transitions = new List<(string oldSeason, string newSeason)>();

            calendar.OnSeasonChanged += (oldS, newS) =>
            {
                transitions.Add((oldS, newS));
            };

            // Advance to day 29 (still First Thaw)
            calendar.SetDay(29);
            Assert.Empty(transitions);

            // Advance to day 30 (Ash Settling begins)
            calendar.SetDay(30);
            Assert.Single(transitions);
            Assert.Equal("window_first_thaw", transitions[0].oldSeason);
            Assert.Equal("window_ash_settling", transitions[0].newSeason);

            // Advance to day 31 (still Ash Settling)
            calendar.SetDay(31);
            Assert.Single(transitions);
        }

        [Fact]
        public void YearOfAsh_DeepFreeze_MatchesTimelineTemperatureParity()
        {
            var calendar = new CampaignCalendar(initialDay: 180);

            // At day 210, peak deep freeze reaches -45C
            var model210 = calendar.ResolveDay(210);
            Assert.InRange(model210.AmbientTemperatureC, -46.0f, -44.0f);
            Assert.True(model210.SeasonalSeverity >= 0.95f);
            Assert.True(model210.PreservationBias >= 1.5f); // High preservation in extreme cold
            Assert.True(model210.DayLengthHours <= 7.0f); // Short daylight
        }

        [Fact]
        public void CustomSeasonProfile_BindsCorrectly()
        {
            var customProfile = new SeasonProfileDef
            {
                id = "custom_test_cycle",
                displayName = "Custom Cycle",
                seasons = new List<SeasonWindowDef>
                {
                    new() { id = "custom_spring", displayName = "Custom Spring", startDay = 0 },
                    new() { id = "custom_winter", displayName = "Custom Winter", startDay = 180 }
                }
            };

            var calendar = new CampaignCalendar(initialDay: 1);
            calendar.BindProfile(customProfile);

            var modelEarly = calendar.ResolveDay(10);
            Assert.Equal("custom_spring", modelEarly.SeasonId);

            var modelLate = calendar.ResolveDay(200);
            Assert.Equal("custom_winter", modelLate.SeasonId);
        }
    }
}
