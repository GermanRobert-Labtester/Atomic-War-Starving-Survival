// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Integration tests for Task 7: Deep Geothermal Boreholes &amp; Clean Aquifer Pumping.
    /// Simulates end-to-end scenarios: drilling campaign, turbine commission,
    /// aquifer tap, multi-day operation, pressure events.
    /// </summary>
    public sealed class GeothermalAquiferIntegrationTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");
        }

        [Fact]
        public void FullDrillingCampaign_ReachesAquifer_AndTaps()
        {
            var system = new GeothermalAquiferSystem(null, new SeededRng(2003), new TestLog());
            system.LoadCatalog(GeothermalCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            // Start drilling
            Assert.True(system.StartDrilling().IsSuccess);

            // Drill until we reach aquifer depth
            int maxIterations = 1000;
            while (system.State.projectActive && maxIterations-- > 0)
            {
                system.AdvanceDrilling(50f);
                if (system.State.currentDepthMeters >= 750f)
                    break;
            }

            Assert.True(system.State.currentDepthMeters >= 750f, "reached aquifer depth");
            Assert.True(system.State.crossedStrataIds.Contains("strata_artesian_aquifer_750m"), "crossed aquifer strata");

            // Install casing
            Assert.True(system.InstallCasing(600f).IsSuccess);

            // Tap aquifer
            Assert.True(system.TapAquifer().IsSuccess);
            Assert.True(system.IsAquiferTapped);
        }

        [Fact]
        public void TurbineCommission_RequiresSteamAndCasing()
        {
            var system = new GeothermalAquiferSystem(null, new SeededRng(2003), new TestLog());

            // No steam pocket crossed
            Assert.False(system.CommissionTurbine().IsSuccess);

            // Cross steam pocket but no casing
            system.State.crossedStrataIds.Add("strata_steam_pocket_500m");
            Assert.False(system.CommissionTurbine().IsSuccess);

            // Add casing
            system.State.installedCasingDepth = 300f;
            Assert.True(system.CommissionTurbine().IsSuccess);
            Assert.True(system.IsTurbineCommissioned);
        }

        [Fact]
        public void MultiDayOperation_TurbineDegrades_AndScaleAccumulates()
        {
            var system = new GeothermalAquiferSystem(null, new SeededRng(2003), new TestLog());
            system.State.turbineCommissioned = true;
            system.State.generatorHealth = 100f;
            system.State.mineralScaling = 0f;
            system.State.steamPressurePsi = 100f;

            for (int day = 1; day <= 100; day++)
            {
                system.TickDay(day);
            }

            Assert.True(system.State.mineralScaling > 0f, "scale accumulated over 100 days");
            Assert.True(system.State.generatorHealth < 100f, "health decayed over 100 days");
            Assert.Equal(100, system.State.lastProcessedDay);
        }

        [Fact]
        public void PressureVenting_ReducesPressure_AndIncreasesRelief()
        {
            var system = new GeothermalAquiferSystem(null, new SeededRng(2003), new TestLog());
            system.State.steamPressurePsi = 200f;
            system.State.pressureReliefState = 0f;

            var result = system.VentPressure();
            Assert.True(result.IsSuccess);
            Assert.True(system.State.steamPressurePsi < 200f, "pressure reduced");
            Assert.True(system.State.pressureReliefState > 0f, "relief state increased");
        }

        [Fact]
        public void Descaling_ReducesScale_AndConsumesChemicals()
        {
            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.AddById("item_scrap_chemical", 10);
            var system = new GeothermalAquiferSystem(null, new SeededRng(2003), new TestLog(), inventory: inventory);
            system.State.mineralScaling = 50f;

            var result = system.Descale();
            Assert.True(result.IsSuccess);
            Assert.True(system.State.mineralScaling < 50f, "scale reduced");
        }

        [Fact]
        public void SaveRoundTrip_FullCampaign_PreservesAllState()
        {
            var system = new GeothermalAquiferSystem(null, new SeededRng(2003), new TestLog());
            system.LoadCatalog(GeothermalCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            // Build up state
            system.StartDrilling();
            for (int i = 0; i < 50; i++) system.AdvanceDrilling(20f);
            system.InstallCasing(300f);
            system.State.crossedStrataIds.Add("strata_steam_pocket_500m");
            system.CommissionTurbine();
            system.State.mineralScaling = 15f;
            system.TickDay(30);

            var captured = system.CaptureState();
            var restored = new GeothermalAquiferSystem(captured, new SeededRng(2003), new TestLog());
            restored.LoadCatalog(GeothermalCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            Assert.Equal(system.State.currentDepthMeters, restored.State.currentDepthMeters);
            Assert.Equal(system.State.installedCasingDepth, restored.State.installedCasingDepth);
            Assert.Equal(system.State.turbineCommissioned, restored.State.turbineCommissioned);
            Assert.Equal(system.State.generatorHealth, restored.State.generatorHealth);
            Assert.Equal(system.State.mineralScaling, restored.State.mineralScaling);
            Assert.Equal(system.State.crossedStrataIds.Count, restored.State.crossedStrataIds.Count);
            Assert.Equal(30, restored.State.lastProcessedDay);
        }
    }
}
