// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Current GreenhouseSystem input/capacity contracts. Equipment inventory
    /// policy belongs to the Godot host; Core receives plot capacity and light
    /// hours as explicit inputs.
    /// </summary>
    public sealed class GreenhouseEquipmentScalingTests
    {
        // TEST-AGGREGATION: source_rows=6 aggregate_cases=1 saved_cases=5
        [Fact]
        public void GrowLightInput_ScalesCropGrowthAndCapsAtCropRequirement()
        {
            var failures = new List<string>();
            foreach (var testCase in new[]
            {
                (GrowLightHours: 0f, ExpectedGrowth: 0f),
                (GrowLightHours: 2f, ExpectedGrowth: 12.5f),
                (GrowLightHours: 4f, ExpectedGrowth: 25f),
                (GrowLightHours: 8f, ExpectedGrowth: 25f)
            })
            {
                var system = new GreenhouseSystem(seed: 1);
                system.EnsurePlots(1);
                Assert.True(system.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _));
                system.Water(0, GreenhouseSystem.MaxWater, tainted: false);
                system.TickDay(2, testCase.GrowLightHours, ashContaminationRate: 0f);

                float actual = system.Plots[0].growth;
                if (Math.Abs(actual - testCase.ExpectedGrowth) > 0.001f)
                    failures.Add($"light={testCase.GrowLightHours}: expected growth {testCase.ExpectedGrowth}, got {actual}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void EnsurePlots_GrowsAndRemovesOnlyTrailingFallowPlots()
        {
            var system = new GreenhouseSystem(seed: 2);
            system.EnsurePlots(6);
            Assert.True(system.Plant(4, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _));
            Assert.True(system.Plant(5, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _));

            system.EnsurePlots(3);
            Assert.Equal(6, system.PlotCount);

            Assert.True(system.Clear(5));
            system.EnsurePlots(3);
            Assert.Equal(5, system.PlotCount);

            Assert.True(system.Clear(4));
            system.EnsurePlots(3);
            Assert.Equal(3, system.PlotCount);
        }

        [Fact]
        public void HarvestAndClear_KeepTheCurrentResidualContaminationContract()
        {
            var system = new GreenhouseSystem(seed: 3);
            system.EnsurePlots(1);
            Assert.True(system.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, 1, out _));
            system.Water(0, 60f, tainted: true);
            system.Plots[0].stage = (int)GreenhouseStage.Mature;

            Assert.True(system.Harvest(0).success);
            Assert.Equal(45f, system.Plots[0].soilContamination, precision: 3);

            Assert.True(system.Clear(0));
            Assert.True(GreenhouseSystem.IsFallow(system.Plots[0]));
            Assert.Equal(22.5f, system.Plots[0].soilContamination, precision: 3);
        }
    }
}
