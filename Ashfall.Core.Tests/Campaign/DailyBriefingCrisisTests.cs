// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// C1.4 deferred consumer — the daily briefing renders authoritative crisis
    /// predictions (produced by <see cref="CrisisPredictor"/>) as a bounded
    /// "Crisis Warnings" section, and leaves the report untouched when none
    /// clear their thresholds.
    /// </summary>
    public sealed class DailyBriefingCrisisTests
    {
        [Fact]
        public void AppendCrisisWarnings_NullOrEmpty_LeavesReportUnchanged()
        {
            var report = new DailyBriefingReport { Day = 3 };
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, null);
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, new List<CrisisPredictionRecord>());
            Assert.Empty(report.Sections);
            Assert.True(report.IsEmpty);
        }

        [Fact]
        public void AppendCrisisWarnings_RendersSectionWithAdviceAndConfidence()
        {
            var record = new CrisisPredictionRecord(
                CrisisClass.FoodDepletion,
                projectedDay: 12,
                horizonDays: 2,
                confidence: 0.82f,
                confidenceBand: CrisisConfidenceBand.High,
                reasonId: "crisis.reason.food_stock_exhaustion",
                preparationAdvice: "Stock non-perishable rations.");

            var report = new DailyBriefingReport { Day = 10 };
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, new[] { record });

            var section = Assert.Single(report.Sections);
            Assert.Equal("Crisis Warnings", section.Title);
            var entry = Assert.Single(section.Entries);
            Assert.Equal("crisis.reason.food_stock_exhaustion", entry.PrimaryId);
            Assert.Contains("FoodDepletion", entry.Text);
            Assert.Contains("in 2 days", entry.Text);
            Assert.Contains("day 12", entry.Text);
            Assert.Contains("high confidence", entry.Text);
            Assert.Contains("Stock non-perishable rations.", entry.Text);
        }

        [Theory]
        [InlineData(0, "today")]
        [InlineData(1, "tomorrow")]
        [InlineData(3, "in 3 days")]
        public void AppendCrisisWarnings_HorizonWording(int horizon, string expected)
        {
            var record = new CrisisPredictionRecord(
                CrisisClass.WaterDepletion, 10 + horizon, horizon, 0.6f,
                CrisisConfidenceBand.Moderate, "crisis.reason.water_stock_exhaustion", "Ration water.");

            var report = new DailyBriefingReport { Day = 10 };
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, new[] { record });

            Assert.Contains(expected, Assert.Single(Assert.Single(report.Sections).Entries).Text);
        }

        [Fact]
        public void AppendCrisisWarnings_BoundsAndDeduplicates()
        {
            var records = new List<CrisisPredictionRecord>();
            for (int i = 0; i < 15; i++)
            {
                records.Add(new CrisisPredictionRecord(
                    CrisisClass.FoodDepletion, 20 + i, i, 0.6f,
                    CrisisConfidenceBand.Moderate, $"crisis.reason.food_{i}", "Advice."));
            }

            var report = new DailyBriefingReport { Day = 10 };
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, records, maxEntriesPerSection: 5);

            var section = Assert.Single(report.Sections);
            Assert.Equal(6, section.Entries.Length); // 5 kept + 1 overflow marker
            Assert.Contains(section.Entries, e => e.PrimaryId == "overflow");
        }

        [Fact]
        public void AppendCrisisWarnings_ConsumesRealPredictorOutput()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 10,
                LivingSurvivorCount = 4,
                FoodStockUnits = 4f,
                DailyFoodBurnRate = 4f
            };
            var predictions = CrisisPredictor.Evaluate(inputs);
            Assert.Contains(predictions, p => p.Kind == CrisisClass.FoodDepletion);

            var report = new DailyBriefingReport { Day = 10 };
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, predictions);

            Assert.False(report.IsEmpty);
            Assert.Contains(report.Sections, s => s.Title == "Crisis Warnings");
        }

        [Fact]
        public void AppendCrisisWarnings_HealthyInputs_AddsNothing()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 1,
                LivingSurvivorCount = 4,
                FoodStockUnits = 500f,
                WaterStockUnits = 500f,
                FuelRunwayDays = 90f,
                GenerationWatts = 500f,
                TotalDrawWatts = 300f,
                MaxRadiationDose = 5f,
                AverageRadiationDose = 5f
            };
            var predictions = CrisisPredictor.Evaluate(inputs);
            Assert.Empty(predictions);

            var report = new DailyBriefingReport { Day = 1 };
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, predictions);
            Assert.True(report.IsEmpty);
        }
    }
}