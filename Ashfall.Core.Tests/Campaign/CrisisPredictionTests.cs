// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests.Campaign
{
    public class CrisisPredictionTests
    {
        [Fact]
        public void Evaluate_HealthyNeutralInputs_ReturnsEmpty()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 10,
                LivingSurvivorCount = 4,
                FoodStockUnits = 100f,
                DailyFoodBurnRate = 4f, // 25 days of food
                WaterStockUnits = 100f,
                DailyWaterBurnRate = 4f,
                GenerationWatts = 1000f,
                TotalDrawWatts = 400f,
                BatteryReserveWh = 10000f,
                FuelRunwayDays = 20f,
                AverageRadiationDose = 0f,
                MaxRadiationDose = 0f,
                PathogenExposureModifier = 1.0f,
                ActiveSanitationSpills = 0,
                WorkingAdultCount = 4,
                DependentCount = 0
            };

            var records = CrisisPredictor.Evaluate(inputs);

            Assert.Empty(records);
        }

        [Fact]
        public void Evaluate_FoodDepletionTrajectory_PredictsFoodCrisisWithBoundedConfidence()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 15,
                LivingSurvivorCount = 4,
                FoodStockUnits = 12f,
                DailyFoodBurnRate = 4f // 3 days remaining
            };

            var records = CrisisPredictor.Evaluate(inputs);

            Assert.Single(records);
            var r = records[0];
            Assert.Equal(CrisisClass.FoodDepletion, r.Kind);
            Assert.Equal(18, r.ProjectedDay);
            Assert.Equal(3, r.HorizonDays);
            Assert.True(r.Confidence >= 0.6f && r.Confidence <= 1.0f);
            Assert.Equal(CrisisConfidenceBand.High, r.ConfidenceBand);
            Assert.Equal("crisis.reason.food_stock_exhaustion", r.ReasonId);
            Assert.False(string.IsNullOrWhiteSpace(r.PreparationAdvice));
        }

        [Fact]
        public void Evaluate_WaterDepletionTrajectory_PredictsWaterCrisis()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 5,
                LivingSurvivorCount = 4,
                WaterStockUnits = 8f,
                DailyWaterBurnRate = 4f // 2 days remaining
            };

            var records = CrisisPredictor.Evaluate(inputs);

            Assert.Single(records);
            var r = records[0];
            Assert.Equal(CrisisClass.WaterDepletion, r.Kind);
            Assert.Equal(7, r.ProjectedDay);
            Assert.Equal(2, r.HorizonDays);
            Assert.Equal("crisis.reason.water_stock_exhaustion", r.ReasonId);
        }

        [Fact]
        public void Evaluate_PowerDepletion_PredictsBlackout()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 20,
                GenerationWatts = 100f,
                TotalDrawWatts = 300f, // Deficit
                BatteryReserveWh = 0f,
                FuelRunwayDays = 2f
            };

            var records = CrisisPredictor.Evaluate(inputs);

            Assert.Contains(records, r => r.Kind == CrisisClass.PowerBlackout && r.ProjectedDay == 22);
        }

        [Fact]
        public void Evaluate_SevereRadiationDose_PredictsRadiationSpike()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 10,
                MaxRadiationDose = 85f // Exceeds critical 80
            };

            var records = CrisisPredictor.Evaluate(inputs);

            var radCrisis = Assert.Single(records, r => r.Kind == CrisisClass.RadiationSpike);
            Assert.Equal(11, radCrisis.ProjectedDay);
            Assert.Equal(1, radCrisis.HorizonDays);
            Assert.True(radCrisis.Confidence >= 0.8f);
        }

        [Fact]
        public void Evaluate_SanitationSpills_PredictsDiseaseOutbreak()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 12,
                ActiveSanitationSpills = 2,
                PathogenExposureModifier = 1.6f
            };

            var records = CrisisPredictor.Evaluate(inputs);

            var disease = Assert.Single(records, r => r.Kind == CrisisClass.DiseaseOutbreak);
            Assert.Equal(13, disease.ProjectedDay);
            Assert.Equal(1, disease.HorizonDays);
        }

        [Fact]
        public void Evaluate_DistressAmbush_PredictsHostileIntercept()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 30,
                PendingHighRiskDistressFollowUps = 1,
                NextDistressFollowUpDay = 33
            };

            var records = CrisisPredictor.Evaluate(inputs);

            var distress = Assert.Single(records, r => r.Kind == CrisisClass.DistressAmbush);
            Assert.Equal(33, distress.ProjectedDay);
            Assert.Equal(3, distress.HorizonDays);
        }

        [Fact]
        public void Evaluate_ZeroWorkingAdults_PredictsDemographicCollapse()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 8,
                LivingSurvivorCount = 3,
                WorkingAdultCount = 0,
                DependentCount = 3
            };

            var records = CrisisPredictor.Evaluate(inputs);

            var demo = Assert.Single(records, r => r.Kind == CrisisClass.DemographicCollapse);
            Assert.Equal(8, demo.ProjectedDay);
            Assert.Equal(0, demo.HorizonDays);
            Assert.Equal(0.99f, demo.Confidence);
        }

        [Fact]
        public void Evaluate_EmptyStock_HandlesWithoutDivideByZero()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 1,
                LivingSurvivorCount = 5,
                FoodStockUnits = 0f,
                WaterStockUnits = 0f
            };

            var records = CrisisPredictor.Evaluate(inputs);

            Assert.Contains(records, r => r.Kind == CrisisClass.FoodDepletion && r.ProjectedDay == 1 && r.Confidence >= 0.9f);
            Assert.Contains(records, r => r.Kind == CrisisClass.WaterDepletion && r.ProjectedDay == 1 && r.Confidence >= 0.9f);
        }

        [Fact]
        public void Evaluate_EmptyRoster_DoesNotStarveDeadSurvivors()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 10,
                LivingSurvivorCount = 0,
                FoodStockUnits = 0f,
                WaterStockUnits = 0f,
                WorkingAdultCount = 0
            };

            var records = CrisisPredictor.Evaluate(inputs);

            // With zero survivors, no food depletion, water depletion, or demographic collapse should be reported
            Assert.DoesNotContain(records, r => r.Kind == CrisisClass.FoodDepletion);
            Assert.DoesNotContain(records, r => r.Kind == CrisisClass.WaterDepletion);
            Assert.DoesNotContain(records, r => r.Kind == CrisisClass.DemographicCollapse);
        }

        [Fact]
        public void Evaluate_ExactThresholdBoundary_FiresAppropriately()
        {
            // Boundary at runway = 5.0f exactly
            var boundaryInputs = new CrisisPredictionInputs
            {
                CurrentDay = 1,
                LivingSurvivorCount = 2,
                FoodStockUnits = 10f,
                DailyFoodBurnRate = 2f // exactly 5 days runway
            };

            var records = CrisisPredictor.Evaluate(boundaryInputs);
            Assert.Single(records, r => r.Kind == CrisisClass.FoodDepletion);

            // Just over boundary at runway = 5.5f
            var safeInputs = new CrisisPredictionInputs
            {
                CurrentDay = 1,
                LivingSurvivorCount = 2,
                FoodStockUnits = 11f,
                DailyFoodBurnRate = 2f // 5.5 days runway
            };

            var safeRecords = CrisisPredictor.Evaluate(safeInputs);
            Assert.DoesNotContain(safeRecords, r => r.Kind == CrisisClass.FoodDepletion);
        }

        [Fact]
        public void Evaluate_MultipleCrises_SortedDeterministicallyByProjectedDayAndConfidence()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 10,
                LivingSurvivorCount = 4,
                FoodStockUnits = 16f, // 4d runway -> Day 14
                DailyFoodBurnRate = 4f,
                WaterStockUnits = 8f,  // 2d runway -> Day 12
                DailyWaterBurnRate = 4f,
                FuelRunwayDays = 1f   // 1d runway -> Day 11
            };

            var records = CrisisPredictor.Evaluate(inputs);

            Assert.True(records.Count >= 3);
            for (int i = 0; i < records.Count - 1; i++)
            {
                Assert.True(records[i].ProjectedDay <= records[i + 1].ProjectedDay,
                    $"Ordering violation at {i}: {records[i].Kind} (day {records[i].ProjectedDay}) vs {records[i+1].Kind} (day {records[i+1].ProjectedDay})");
            }
        }

        [Fact]
        public void Evaluate_PureFunction_RepeatedCallsIdentical()
        {
            var inputs = new CrisisPredictionInputs
            {
                CurrentDay = 5,
                FoodStockUnits = 12f,
                DailyFoodBurnRate = 4f
            };

            var run1 = CrisisPredictor.Evaluate(inputs);
            var run2 = CrisisPredictor.Evaluate(inputs);

            Assert.Equal(run1.Count, run2.Count);
            for (int i = 0; i < run1.Count; i++)
            {
                Assert.Equal(run1[i].Kind, run2[i].Kind);
                Assert.Equal(run1[i].ProjectedDay, run2[i].ProjectedDay);
                Assert.Equal(run1[i].HorizonDays, run2[i].HorizonDays);
                Assert.Equal(run1[i].Confidence, run2[i].Confidence);
                Assert.Equal(run1[i].ReasonId, run2[i].ReasonId);
            }
        }

        [Fact]
        public void MonotonicApproachTest_AcrossDayTransitions_HorizonDecreasesConsistently()
        {
            // Fixture: 20 units of food, consuming 4 units per day.
            // Crisis hits on Day 6 (runway drops to 0 on Day 6).
            int expectedCrisisDay = 6;
            float stock = 20f;
            float burn = 4f;

            for (int day = 1; day <= 5; day++)
            {
                var inputs = new CrisisPredictionInputs
                {
                    CurrentDay = day,
                    LivingSurvivorCount = 4,
                    FoodStockUnits = stock,
                    DailyFoodBurnRate = burn
                };

                var records = CrisisPredictor.Evaluate(inputs);
                var foodCrisis = Assert.Single(records, r => r.Kind == CrisisClass.FoodDepletion);

                Assert.Equal(expectedCrisisDay, foodCrisis.ProjectedDay);
                Assert.Equal(expectedCrisisDay - day, foodCrisis.HorizonDays);

                // Advance day and consume food
                stock -= burn;
            }
        }

        [Fact]
        public void WeatherIntelligenceCoordinator_SevereForecast_PopulatesCrisisFields()
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 12345);
            var armor = new SkyLayerArmorSystem();
            var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(12345));

            coord.Station.Install(1);
            coord.Station.Calibrate(1);
            coord.TickDay(1);

            // Force a severe forecast entry
            coord.Station.State.cachedForecast.Clear();
            coord.Station.State.cachedForecast.Add(new ForecastEntry
            {
                day = 3,
                weather = WeatherKind.GlassStorm,
                confidence = 0.9f,
                isRouteSafe = false
            });

            var rm = coord.BuildReadModel();

            Assert.True(rm.hasPredictedCrisis);
            Assert.Equal(3, rm.predictedCrisisDay);
            Assert.Equal(2, rm.daysUntilPredictedCrisis);
            Assert.Equal(WeatherKind.GlassStorm, rm.predictedWeatherKind);
            Assert.True(rm.predictedCrisisConfidence >= 0.5f);
            Assert.Contains("kinetic", rm.crisisPreparationAdvice);
            Assert.Contains("CRISIS ALERT", rm.advisory);
        }

        [Fact]
        public void WeatherIntelligenceCoordinator_NoSevereWeather_HasNoPredictedCrisis()
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 9999);
            var armor = new SkyLayerArmorSystem();
            var coord = new WeatherIntelligenceCoordinator(weather, armor, new SeededRng(9999));

            coord.Station.Install(1);
            coord.Station.Calibrate(1);
            coord.Station.State.cachedForecast.Clear();
            coord.Station.State.cachedForecast.Add(new ForecastEntry
            {
                day = 2,
                weather = WeatherKind.Clear,
                confidence = 0.95f,
                isRouteSafe = true
            });

            var rm = coord.BuildReadModel();

            Assert.False(rm.hasPredictedCrisis);
            Assert.DoesNotContain("CRISIS ALERT", rm.advisory);
        }
    }
}
