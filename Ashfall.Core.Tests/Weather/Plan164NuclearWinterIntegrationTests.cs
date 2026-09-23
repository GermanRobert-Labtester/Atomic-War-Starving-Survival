// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.Weather;
using Xunit;

namespace Ashfall.Core.Tests.Plan164NuclearWinter
{
    public sealed class Plan164NuclearWinterIntegrationTests
    {
        private static string GetCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/nuclear_winter_phases.json");
            if (File.Exists(path))
                return File.ReadAllText(path);

            string altPath = "Assets/StreamingAssets/Data/nuclear_winter_phases.json";
            if (File.Exists(altPath))
                return File.ReadAllText(altPath);

            return string.Empty;
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_PhasesAndSeasonsPopulated()
        {
            var system = new NuclearWinterProgressionSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            Assert.True(system.Phases.Count >= 5, "Expected at least 5 nuclear winter phases");
            Assert.True(system.Seasons.Count >= 4, "Expected 4 seasonal cycles");
            Assert.Equal("phase_initial", system.Phases[0].PhaseId);
            Assert.Equal("phase_peak", system.Phases[2].PhaseId);
        }

        [Fact]
        public void PhaseProgression_AccuratelyTracksDayThresholds()
        {
            var system = new NuclearWinterProgressionSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            var phaseDay45 = system.GetPhaseForDay(45);
            var phaseDay120 = system.GetPhaseForDay(120);
            var phaseDay200 = system.GetPhaseForDay(200);
            var phaseDay300 = system.GetPhaseForDay(300);
            var phaseDay400 = system.GetPhaseForDay(400);

            Assert.Equal("phase_initial", phaseDay45.PhaseId);
            Assert.Equal("phase_escalating", phaseDay120.PhaseId);
            Assert.Equal("phase_peak", phaseDay200.PhaseId);
            Assert.Equal("phase_late", phaseDay300.PhaseId);
            Assert.Equal("phase_stabilizing", phaseDay400.PhaseId);
        }

        [Fact]
        public void SeasonalCycle_AccuratelyCyclesAndCalculatesDaylight()
        {
            var system = new NuclearWinterProgressionSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            // Cycle is 120 days (4 * 30 days)
            var seasonWinter = system.GetSeasonForDay(15);
            var seasonSpring = system.GetSeasonForDay(45);
            var seasonSummer = system.GetSeasonForDay(75);
            var seasonAutumn = system.GetSeasonForDay(105);

            Assert.Equal("season_deep_winter", seasonWinter.SeasonId);
            Assert.Equal("season_pale_spring", seasonSpring.SeasonId);
            Assert.Equal("season_ash_summer", seasonSummer.SeasonId);
            Assert.Equal("season_black_autumn", seasonAutumn.SeasonId);

            var climateWinter = system.EvaluateClimate(15);
            var climateSummer = system.EvaluateClimate(75);

            Assert.True(climateSummer.EffectiveDaylightHours > climateWinter.EffectiveDaylightHours,
                $"Summer daylight ({climateSummer.EffectiveDaylightHours}) should exceed winter ({climateWinter.EffectiveDaylightHours})");
        }

        [Fact]
        public void ClimateEvaluation_CalculatesHeatingAndHazardMultipliers()
        {
            var system = new NuclearWinterProgressionSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            // Day 200 is peak phase deep winter
            var peakClimate = system.EvaluateClimate(200);
            Assert.True(peakClimate.GlobalTemperatureCelsius < -20.0f, $"Peak winter temperature {peakClimate.GlobalTemperatureCelsius} should be deep freeze");
            Assert.True(peakClimate.HeatingFuelCostMultiplier > 1.5f, "Peak winter heating fuel multiplier should be elevated");
            Assert.True(peakClimate.ExpeditionHazardMultiplier > 1.5f, "Peak winter expedition hazards should be elevated");

            float baseHeating = 10.0f;
            float demand = system.CalculateHeatingDemand(baseHeating, 200);
            Assert.True(demand > baseHeating * 1.5f, "Calculated heating demand should reflect temperature severity");

            float cropNormal = system.CalculateCropYieldMultiplier(200, hasGreenhouse: false);
            float cropGreenhouse = system.CalculateCropYieldMultiplier(200, hasGreenhouse: true);
            Assert.True(cropGreenhouse > cropNormal, "Greenhouse must protect and yield higher crop growth rate in peak winter");
        }

        [Fact]
        public void AdvanceDay_TriggersPhaseAndSeasonSeams()
        {
            var system = new NuclearWinterProgressionSystem();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadCatalog(json);

            int phaseTransitions = 0;
            int seasonTransitions = 0;
            system.OnPhaseTransitionedSeam = (prev, cur) => phaseTransitions++;
            system.OnSeasonTransitionedSeam = (prev, cur) => seasonTransitions++;

            // Advance through day 1 to 95 (crosses day 30, 60, 90, 91)
            system.AdvanceDay(1);
            system.AdvanceDay(31);
            system.AdvanceDay(92);

            Assert.True(seasonTransitions >= 2, $"Expected at least 2 season transitions, got {seasonTransitions}");
            Assert.True(phaseTransitions >= 1, $"Expected at least 1 phase transition, got {phaseTransitions}");
            Assert.True(system.RecordedEvents.Count >= 3, "Events should be recorded for transitions");
        }

        [Fact]
        public void SaveState_RoundTrip_PreservesDayAndRecordedEvents()
        {
            var system = new NuclearWinterProgressionSystem();
            system.AdvanceDay(15);
            system.AdvanceDay(95);

            var save = system.CaptureState();
            Assert.Equal(1, save.SchemaVersion);
            Assert.Equal(95, save.CurrentDay);
            Assert.NotEmpty(save.RecordedEvents);

            var restoredSystem = new NuclearWinterProgressionSystem();
            restoredSystem.RestoreState(save);

            Assert.Equal(95, restoredSystem.CurrentDay);
            Assert.Equal(save.RecordedEvents.Count, restoredSystem.RecordedEvents.Count);
        }
    }
}
