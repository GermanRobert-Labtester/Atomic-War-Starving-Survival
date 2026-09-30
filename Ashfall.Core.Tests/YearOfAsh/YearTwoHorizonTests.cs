// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class YearTwoHorizonTests
    {
        private static string GetRealCatalogJson()
        {
            string[] possiblePaths = new[]
            {
                Path.Combine("Assets", "StreamingAssets", "Data", "year_two_climate.json"),
                Path.Combine("..", "Assets", "StreamingAssets", "Data", "year_two_climate.json"),
                Path.Combine("..", "..", "Assets", "StreamingAssets", "Data", "year_two_climate.json"),
                Path.Combine("..", "..", "..", "Assets", "StreamingAssets", "Data", "year_two_climate.json"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data", "year_two_climate.json"),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "year_two_climate.json")
            };

            foreach (var p in possiblePaths)
            {
                if (File.Exists(p)) return File.ReadAllText(p);
            }

            return @"{
  ""schema_version"": 1,
  ""title"": ""Year Two: The Long Thaw — Environmental Climate Catalog"",
  ""start_day"": 361,
  ""end_day"": 720,
  ""phases"": [
    {
      ""phase_id"": ""year_two_q1_thaw_mud"",
      ""quarter"": 1,
      ""title"": ""The Concessions (Thaw Mud)"",
      ""start_day"": 361,
      ""end_day"": 450,
      ""curve_type"": ""linear"",
      ""temp_start_c"": -2.0,
      ""temp_end_c"": 4.0,
      ""temp_min_c"": -4.0,
      ""temp_max_c"": 6.0,
      ""ash_opacity_start"": 0.30,
      ""ash_opacity_end"": 0.15,
      ""radon_start"": 0.70,
      ""radon_end"": 0.35,
      ""thermal_stress_start"": 0.15,
      ""thermal_stress_end"": 0.10
    },
    {
      ""phase_id"": ""year_two_q2_green_fringe"",
      ""quarter"": 2,
      ""title"": ""The Second Door (Green Fringe)"",
      ""start_day"": 451,
      ""end_day"": 540,
      ""curve_type"": ""linear"",
      ""temp_start_c"": 4.0,
      ""temp_end_c"": 16.0,
      ""temp_min_c"": 2.0,
      ""temp_max_c"": 18.0,
      ""ash_opacity_start"": 0.15,
      ""ash_opacity_end"": 0.05,
      ""radon_start"": 0.35,
      ""radon_end"": 0.15,
      ""thermal_stress_start"": 0.05,
      ""thermal_stress_end"": 0.05
    },
    {
      ""phase_id"": ""year_two_q3_short_summer"",
      ""quarter"": 3,
      ""title"": ""The Short Summer (Late Snap)"",
      ""start_day"": 541,
      ""end_day"": 630,
      ""curve_type"": ""late_snap"",
      ""temp_start_c"": 16.0,
      ""temp_end_c"": -10.0,
      ""temp_min_c"": -12.0,
      ""temp_max_c"": 20.0,
      ""ash_opacity_start"": 0.05,
      ""ash_opacity_end"": 0.20,
      ""radon_start"": 0.15,
      ""radon_end"": 0.10,
      ""thermal_stress_start"": 0.05,
      ""thermal_stress_end"": 0.45
    },
    {
      ""phase_id"": ""year_two_q4_second_winter"",
      ""quarter"": 4,
      ""title"": ""The Handing Over (Second Winter)"",
      ""start_day"": 631,
      ""end_day"": 720,
      ""curve_type"": ""winter_trough"",
      ""temp_start_c"": -5.0,
      ""temp_end_c"": -18.0,
      ""temp_min_c"": -25.0,
      ""temp_max_c"": -2.0,
      ""ash_opacity_start"": 0.20,
      ""ash_opacity_end"": 0.60,
      ""radon_start"": 0.10,
      ""radon_end"": 0.05,
      ""thermal_stress_start"": 0.50,
      ""thermal_stress_end"": 0.75
    }
  ]
}";
        }

        [Fact]
        public void YearOne_ReplayIsBitIdentical_WithAndWithoutYearTwoCatalog()
        {
            var uncataloged = new YearOfAshTimelineSystem();
            var cataloged = new YearOfAshTimelineSystem();
            var catalog = YearTwoClimateCatalog.LoadFromJson(GetRealCatalogJson());
            cataloged.BindYearTwoClimateCatalog(catalog);

            for (int day = 180; day <= 360; day++)
            {
                uncataloged.AdvanceDay(day);
                cataloged.AdvanceDay(day);

                Assert.Equal(uncataloged.CurrentDay, cataloged.CurrentDay);
                Assert.Equal(uncataloged.CurrentPhase, cataloged.CurrentPhase);
                Assert.Equal(uncataloged.AmbientTemperatureCelsius, cataloged.AmbientTemperatureCelsius);
                Assert.Equal(uncataloged.AshCloudOpacity, cataloged.AshCloudOpacity);
                Assert.Equal(uncataloged.RadonInfiltrationRate, cataloged.RadonInfiltrationRate);
                Assert.Equal(uncataloged.ThermalStressLevel, cataloged.ThermalStressLevel);
                Assert.Equal(uncataloged.ContinuityDecreeActive, cataloged.ContinuityDecreeActive);
                Assert.Equal(uncataloged.FinalBroadcastsActive, cataloged.FinalBroadcastsActive);
                Assert.Equal(string.Empty, cataloged.ActiveClimatePhaseId);
                Assert.Equal(0, cataloged.YearTwoQuarter);
            }
        }

        [Fact]
        public void YearTwo_Days361To720_AdvancesThroughFourDistinguishableQuarters()
        {
            var timeline = new YearOfAshTimelineSystem();
            var catalog = YearTwoClimateCatalog.LoadFromJson(GetRealCatalogJson());
            var report = catalog.Validate();
            Assert.True(report.IsValid, string.Join("; ", report.Errors));

            timeline.BindYearTwoClimateCatalog(catalog);
            timeline.AdvanceDay(360);
            Assert.Equal(360, timeline.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase6_TheGreatThaw, timeline.CurrentPhase);

            // Advance into Quarter 1 (Day 361)
            timeline.AdvanceDay(361);
            Assert.Equal(361, timeline.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase7_TheLongThaw, timeline.CurrentPhase);
            Assert.Equal("year_two_q1_thaw_mud", timeline.ActiveClimatePhaseId);
            Assert.Equal(1, timeline.YearTwoQuarter);
            Assert.InRange(timeline.AmbientTemperatureCelsius, -4.0f, 6.0f);

            // Advance to middle of Quarter 1 (Day 400)
            timeline.AdvanceDay(400);
            Assert.Equal(1, timeline.YearTwoQuarter);

            // Advance into Quarter 2 (Day 460)
            timeline.AdvanceDay(460);
            Assert.Equal("year_two_q2_green_fringe", timeline.ActiveClimatePhaseId);
            Assert.Equal(2, timeline.YearTwoQuarter);
            Assert.True(timeline.AmbientTemperatureCelsius > 4.0f, "Green fringe warms above 4C");

            // Advance into Quarter 3 (Day 580)
            timeline.AdvanceDay(580);
            Assert.Equal("year_two_q3_short_summer", timeline.ActiveClimatePhaseId);
            Assert.Equal(3, timeline.YearTwoQuarter);

            // Advance into Quarter 4 (Day 700)
            timeline.AdvanceDay(700);
            Assert.Equal("year_two_q4_second_winter", timeline.ActiveClimatePhaseId);
            Assert.Equal(4, timeline.YearTwoQuarter);
            Assert.True(timeline.AmbientTemperatureCelsius < 0.0f, "Second winter plunges below 0C");

            // Advance to Year Two ceiling (Day 720)
            timeline.AdvanceDay(720);
            Assert.Equal(720, timeline.CurrentDay);
            Assert.Equal(-18.0f, timeline.AmbientTemperatureCelsius, 1);

            // Advance beyond 720 should clamp to 720
            timeline.AdvanceDay(750);
            Assert.Equal(720, timeline.CurrentDay);
        }

        [Fact]
        public void YearTwo_SubsystemsReceiveChangingValuesAcross361To720()
        {
            var timeline = new YearOfAshTimelineSystem();
            var catalog = YearTwoClimateCatalog.LoadFromJson(GetRealCatalogJson());
            timeline.BindYearTwoClimateCatalog(catalog);

            var deepFreeze = new YearOfAshDeepFreezeSystem();
            var radon = new YearOfAshRadonSystem();
            var iceRoad = new YearOfAshIceRoadSystem();

            float initialTemp = 0f;
            float minTemp = float.MaxValue;
            float maxTemp = float.MinValue;

            for (int day = 361; day <= 720; day++)
            {
                timeline.AdvanceDay(day);
                float temp = timeline.AmbientTemperatureCelsius;
                if (day == 361) initialTemp = temp;
                if (temp < minTemp) minTemp = temp;
                if (temp > maxTemp) maxTemp = temp;

                deepFreeze.TickDailyThermal(day, temp);
                radon.TickDailyRadon(day, temp);
                iceRoad.TickDay(day, temp, Array.Empty<StormWindowEntry>());
            }

            // Verify temperature had significant swing rather than staying flat at legacy 4C
            Assert.True(maxTemp - minTemp > 20.0f, $"Temperature swing must be > 20C, was {maxTemp - minTemp}C");
            Assert.NotEqual(initialTemp, maxTemp);
        }

        [Fact]
        public void YearTwoClimateCatalog_ValidationCatchesGapsOverlapsAndInvalidCurves()
        {
            // Missing coverage / gap
            string gapJson = @"{
  ""schema_version"": 1,
  ""start_day"": 361,
  ""end_day"": 720,
  ""phases"": [
    {
      ""quarter"": 1,
      ""phase_id"": ""q1"",
      ""title"": ""Q1"",
      ""start_day"": 361,
      ""end_day"": 450,
      ""temp_start_c"": 0,
      ""temp_end_c"": -10,
      ""temp_min_c"": -12,
      ""temp_max_c"": 2,
      ""curve_type"": ""linear"",
      ""ash_opacity_start"": 0.3,
      ""ash_opacity_end"": 0.5,
      ""radon_start"": 0.5,
      ""radon_end"": 0.5,
      ""thermal_stress_start"": 0.2,
      ""thermal_stress_end"": 0.4
    },
    {
      ""quarter"": 2,
      ""phase_id"": ""q2"",
      ""title"": ""Q2"",
      ""start_day"": 500,
      ""end_day"": 720,
      ""temp_start_c"": -10,
      ""temp_end_c"": 10,
      ""temp_min_c"": -12,
      ""temp_max_c"": 12,
      ""curve_type"": ""linear"",
      ""ash_opacity_start"": 0.5,
      ""ash_opacity_end"": 0.2,
      ""radon_start"": 0.5,
      ""radon_end"": 0.5,
      ""thermal_stress_start"": 0.4,
      ""thermal_stress_end"": 0.1
    }
  ]
}";
            var gapCatalog = YearTwoClimateCatalog.LoadFromJson(gapJson);
            var gapReport = gapCatalog.Validate();
            Assert.False(gapReport.IsValid);
            Assert.Contains(gapReport.Errors, e => e.Contains("Gap detected"));

            // Overlap
            string overlapJson = @"{
  ""schema_version"": 1,
  ""start_day"": 361,
  ""end_day"": 720,
  ""phases"": [
    {
      ""quarter"": 1,
      ""phase_id"": ""q1"",
      ""title"": ""Q1"",
      ""start_day"": 361,
      ""end_day"": 500,
      ""temp_start_c"": 0,
      ""temp_end_c"": -10,
      ""temp_min_c"": -12,
      ""temp_max_c"": 2,
      ""curve_type"": ""linear"",
      ""ash_opacity_start"": 0.3,
      ""ash_opacity_end"": 0.5,
      ""radon_start"": 0.5,
      ""radon_end"": 0.5,
      ""thermal_stress_start"": 0.2,
      ""thermal_stress_end"": 0.4
    },
    {
      ""quarter"": 2,
      ""phase_id"": ""q2"",
      ""title"": ""Q2"",
      ""start_day"": 450,
      ""end_day"": 720,
      ""temp_start_c"": -10,
      ""temp_end_c"": 10,
      ""temp_min_c"": -12,
      ""temp_max_c"": 12,
      ""curve_type"": ""linear"",
      ""ash_opacity_start"": 0.5,
      ""ash_opacity_end"": 0.2,
      ""radon_start"": 0.5,
      ""radon_end"": 0.5,
      ""thermal_stress_start"": 0.4,
      ""thermal_stress_end"": 0.1
    }
  ]
}";
            var overlapCatalog = YearTwoClimateCatalog.LoadFromJson(overlapJson);
            var overlapReport = overlapCatalog.Validate();
            Assert.False(overlapReport.IsValid);
            Assert.Contains(overlapReport.Errors, e => e.Contains("overlaps"));

            // Unknown curve
            string badCurveJson = @"{
  ""schema_version"": 1,
  ""start_day"": 361,
  ""end_day"": 720,
  ""phases"": [
    {
      ""quarter"": 1,
      ""phase_id"": ""q1"",
      ""title"": ""Q1"",
      ""start_day"": 361,
      ""end_day"": 720,
      ""temp_start_c"": 0,
      ""temp_end_c"": 10,
      ""temp_min_c"": -5,
      ""temp_max_c"": 15,
      ""curve_type"": ""hyperbolic_sine_unknown"",
      ""ash_opacity_start"": 0.3,
      ""ash_opacity_end"": 0.5,
      ""radon_start"": 0.5,
      ""radon_end"": 0.5,
      ""thermal_stress_start"": 0.2,
      ""thermal_stress_end"": 0.4
    }
  ]
}";
            var badCurveCatalog = YearTwoClimateCatalog.LoadFromJson(badCurveJson);
            var badCurveReport = badCurveCatalog.Validate();
            Assert.False(badCurveReport.IsValid);
            Assert.Contains(badCurveReport.Errors, e => e.Contains("unsupported curve"));
        }

        [Fact]
        public void YearTwo_SaveRestore_MidChapterRoundTripsAndPreservesLegacy()
        {
            var catalog = YearTwoClimateCatalog.LoadFromJson(GetRealCatalogJson());

            // 1. Legacy save (day <= 360)
            var legacyTimeline = new YearOfAshTimelineSystem();
            legacyTimeline.AdvanceDay(250);
            var legacyCaptured = legacyTimeline.CaptureState();

            var restoredLegacy = new YearOfAshTimelineSystem();
            restoredLegacy.BindYearTwoClimateCatalog(catalog);
            restoredLegacy.RestoreState(legacyCaptured);

            Assert.Equal(250, restoredLegacy.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase5_FactionSiege, restoredLegacy.CurrentPhase);
            Assert.Equal(string.Empty, restoredLegacy.ActiveClimatePhaseId);
            Assert.Equal(0, restoredLegacy.YearTwoQuarter);

            // 2. Mid-chapter Year Two save (e.g. Day 540)
            var y2Timeline = new YearOfAshTimelineSystem();
            y2Timeline.BindYearTwoClimateCatalog(catalog);
            y2Timeline.AdvanceDay(540);

            var y2Captured = y2Timeline.CaptureState();
            Assert.Equal(540, y2Captured.currentDay);
            Assert.Equal(YearOfAshPhase.Phase7_TheLongThaw, y2Captured.phase);
            Assert.Equal("year_two_q2_green_fringe", y2Captured.activeClimatePhaseId);
            Assert.Equal(2, y2Captured.yearTwoQuarter);

            var restoredY2 = new YearOfAshTimelineSystem();
            restoredY2.BindYearTwoClimateCatalog(catalog);
            restoredY2.RestoreState(y2Captured);

            Assert.Equal(540, restoredY2.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase7_TheLongThaw, restoredY2.CurrentPhase);
            Assert.Equal("year_two_q2_green_fringe", restoredY2.ActiveClimatePhaseId);
            Assert.Equal(2, restoredY2.YearTwoQuarter);
            Assert.Equal(y2Timeline.AmbientTemperatureCelsius, restoredY2.AmbientTemperatureCelsius);
            Assert.Equal(y2Timeline.AshCloudOpacity, restoredY2.AshCloudOpacity);
            Assert.Equal(y2Timeline.RadonInfiltrationRate, restoredY2.RadonInfiltrationRate);
            Assert.Equal(y2Timeline.ThermalStressLevel, restoredY2.ThermalStressLevel);
        }

        [Fact]
        public void YearTwo_UncatalogedTimeline_ClampsAtDay360()
        {
            var timeline = new YearOfAshTimelineSystem();
            timeline.AdvanceDay(400);

            Assert.Equal(360, timeline.CurrentDay);
            Assert.Equal(YearOfAshPhase.Phase6_TheGreatThaw, timeline.CurrentPhase);
        }
    }
}
