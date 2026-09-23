// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.Events;
using Xunit;

namespace Ashfall.Core.Tests.Plan170SeasonalCelebrations
{
    public sealed class Plan170SeasonalCelebrationsIntegrationTests
    {
        private static string GetCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/shelter_celebrations.json");
            if (File.Exists(path))
                return File.ReadAllText(path);

            string altPath = "Assets/StreamingAssets/Data/shelter_celebrations.json";
            if (File.Exists(altPath))
                return File.ReadAllText(altPath);

            return string.Empty;
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_HolidaysAndScalesPopulated()
        {
            var system = new SeasonalCelebrationSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            Assert.True(system.Holidays.Count >= 5, "Expected at least 5 holidays");
            Assert.True(system.Anniversaries.Count >= 3, "Expected 3 anniversary types");
            Assert.True(system.Scales.Count >= 3, "Expected 3 celebration scale tiers");
            Assert.Contains("hol_new_year", system.Holidays.Keys);
            Assert.Contains("hol_winter_solstice", system.Holidays.Keys);
        }

        [Fact]
        public void CheckHolidayForDay_DetectsFixedAndAnnualRecurringDates()
        {
            var system = new SeasonalCelebrationSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            var holDay1 = system.CheckHolidayForDay(1);
            var holDay90 = system.CheckHolidayForDay(90);
            var holDay360 = system.CheckHolidayForDay(360);
            var holDay361 = system.CheckHolidayForDay(361); // Year 2 Day 1

            Assert.NotNull(holDay1);
            Assert.Equal("hol_new_year", holDay1!.HolidayId);

            Assert.NotNull(holDay90);
            Assert.Equal("hol_spring_thaw", holDay90!.HolidayId);

            Assert.NotNull(holDay360);
            Assert.Equal("hol_winter_solstice", holDay360!.HolidayId);

            // Annual recurring check
            Assert.NotNull(holDay361);
            Assert.Equal("hol_new_year", holDay361!.HolidayId);
        }

        [Fact]
        public void HoldCelebration_CalculatesCostAndMoraleGains_WithStreak()
        {
            var system = new SeasonalCelebrationSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            CelebrationRecord? recorded = null;
            system.OnCelebrationHeldSeam = r => recorded = r;

            // Small celebration with 1 participant
            var rec1 = system.HoldCelebration("hol_new_year", "small", participantCount: 1);
            Assert.NotNull(rec1);
            Assert.Equal("hol_new_year", rec1.HolidayId);
            Assert.Equal(1, system.TraditionStreak);
            Assert.True(rec1.MoraleGained >= 8.0f);
            Assert.True(rec1.FoodSpent >= 4);

            // Large celebration with 5 participants, benefiting from streak bonus
            var rec2 = system.HoldCelebration("hol_spring_thaw", "large", participantCount: 5);
            Assert.NotNull(rec2);
            Assert.Equal(2, system.TraditionStreak);
            Assert.True(rec2.IsMemorable, "Large celebrations should be flagged memorable");
            Assert.True(rec2.MoraleGained > rec1.MoraleGained * 2.5f, "Large celebration with participants should produce high morale boost");
            Assert.NotNull(recorded);
        }

        [Fact]
        public void SkipHoliday_ResetsTraditionStreakAndAppliesPenalty()
        {
            var system = new SeasonalCelebrationSystem();
            system.HoldCelebration("hol_new_year", "small");
            system.HoldCelebration("hol_spring_thaw", "small");
            Assert.Equal(2, system.TraditionStreak);

            string skippedId = string.Empty;
            int skippedDay = -1;
            system.OnHolidaySkippedSeam = (id, day) => { skippedId = id; skippedDay = day; };

            float penalty = system.SkipHoliday("hol_midsummer_day");
            Assert.True(penalty < 0f, "Skipping holiday should yield negative morale penalty");
            Assert.Equal(0, system.TraditionStreak);
            Assert.Equal("hol_midsummer_day", skippedId);
        }

        [Fact]
        public void CommemorateAnniversary_CalculatesMoraleAndRecordsHistory()
        {
            var system = new SeasonalCelebrationSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            string recordedType = string.Empty;
            string recordedEntity = string.Empty;
            float boostGiven = 0f;
            system.OnAnniversaryCommemoratedSeam = (t, e, b) => { recordedType = t; recordedEntity = e; boostGiven = b; };

            float boost = system.CommemorateAnniversary("anniv_fallen", "Marcus Cole", 120);

            Assert.True(boost > 0f);
            Assert.Equal(boost, boostGiven);
            Assert.Equal("anniv_fallen", recordedType);
            Assert.Equal("Marcus Cole", recordedEntity);
            Assert.Contains(system.RecordedAnniversaries, a => a.Contains("Marcus Cole"));
        }

        [Fact]
        public void SaveState_RoundTrip_PreservesStreakAndCelebrationHistory()
        {
            var system = new SeasonalCelebrationSystem();
            system.HoldCelebration("hol_new_year", "small");
            system.HoldCelebration("hol_midsummer_day", "large");
            system.CommemorateAnniversary("anniv_founding", "Holdfast 4", 365);

            var save = system.CaptureState();
            Assert.Equal(1, save.SchemaVersion);
            Assert.Equal(2, save.TraditionStreak);
            Assert.Equal(2, save.CelebrationHistory.Count);
            Assert.Single(save.RecordedAnniversaries);

            var restored = new SeasonalCelebrationSystem();
            restored.RestoreState(save);

            Assert.Equal(2, restored.TraditionStreak);
            Assert.Equal(2, restored.History.Count);
            Assert.Single(restored.RecordedAnniversaries);
            Assert.Equal("hol_new_year", restored.History[0].HolidayId);
        }

        [Fact]
        public void CelebrationIds_AreDeterministic_AcrossIdenticalRuns()
        {
            string json = GetCatalogJson();
            var a = new SeasonalCelebrationSystem();
            a.LoadCatalog(json);
            var b = new SeasonalCelebrationSystem();
            b.LoadCatalog(json);

            var firstA = a.HoldCelebration("hol_new_year", "small");
            var firstB = b.HoldCelebration("hol_new_year", "small");
            Assert.Equal(firstA.CelebrationId, firstB.CelebrationId);

            var secondA = a.HoldCelebration("hol_midsummer_day", "large");
            var secondB = b.HoldCelebration("hol_midsummer_day", "large");
            Assert.Equal(secondA.CelebrationId, secondB.CelebrationId);
            Assert.NotEqual(firstA.CelebrationId, secondA.CelebrationId);
        }
    }
}
