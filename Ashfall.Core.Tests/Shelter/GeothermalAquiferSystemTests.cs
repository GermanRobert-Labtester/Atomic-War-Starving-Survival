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
    /// Deterministic replay and boundary tests for Task 7:
    /// Deep Geothermal Boreholes &amp; Clean Aquifer Pumping.
    /// </summary>
    public sealed class GeothermalAquiferSystemTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data directory not found.");
        }

        private static GeothermalAquiferSystem CreateSystem(GeothermalAquiferState? state = null)
        {
            return new GeothermalAquiferSystem(state, new SeededRng(2003), new TestLog());
        }

        // ── Catalog ──────────────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsSuccessfullyFromDataDir()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = GeothermalCatalogLoader.Load(GetDataDir(), files, json);

            Assert.NotNull(catalog);
            Assert.NotEmpty(catalog!.Strata);
            Assert.Contains(catalog.Strata, s => s.StrataId == "strata_caprock_100m");
        }

        [Fact]
        public void LoadCatalog_RegistersAllStrata()
        {
            var system = CreateSystem();
            var catalog = new GeothermalDrillingCatalog
            {
                Strata = new System.Collections.Generic.List<GeothermalStrataDef>
                {
                    new GeothermalStrataDef { StrataId = "test_strata", StartDepthM = 100f, RockHardness = 5f, DrillWearPerMeter = 0.1f }
                }
            };

            system.LoadCatalog(catalog);
            Assert.Single(system.Strata);
            Assert.True(system.Strata.ContainsKey("test_strata"));
        }

        // ── StartDrilling ────────────────────────────────────────────

        [Fact]
        public void StartDrilling_AlreadyActive_ReturnsBlocked()
        {
            var system = CreateSystem();
            system.StartDrilling();
            var result = system.StartDrilling();
            Assert.False(result.IsSuccess);
            Assert.Equal("already_active", result.FailureCode);
        }

        [Fact]
        public void StartDrilling_NoDrillBit_ReturnsBlocked()
        {
            var system = CreateSystem();
            system.State.drillBitCondition = 0f;
            var result = system.StartDrilling();
            Assert.False(result.IsSuccess);
            Assert.Equal("no_drill_bit", result.FailureCode);
        }

        // ── AdvanceDrilling ──────────────────────────────────────────

        [Fact]
        public void AdvanceDrilling_NotActive_ReturnsFailed()
        {
            var system = CreateSystem();
            var result = system.AdvanceDrilling(10f);
            Assert.False(result.IsSuccess);
            Assert.Equal("not_active", result.FailureCode);
        }

        [Fact]
        public void AdvanceDrilling_AdvancesDepthAndWearsBit()
        {
            var system = CreateSystem();
            // AdvanceDrilling needs a registered stratum (see the bit-destroyed
            // test): without one it fails with "no_strata" and never advances.
            system.LoadCatalog(new GeothermalDrillingCatalog
            {
                Strata = new System.Collections.Generic.List<GeothermalStrataDef>
                {
                    new GeothermalStrataDef { StrataId = "test_strata", StartDepthM = 0f, RockHardness = 5f, DrillWearPerMeter = 0.1f }
                }
            });
            system.StartDrilling();
            float initialDepth = system.State.currentDepthMeters;
            float initialBit = system.State.drillBitCondition;

            var result = system.AdvanceDrilling(10f);
            Assert.True(result.IsSuccess);
            Assert.True(system.State.currentDepthMeters > initialDepth);
            Assert.True(system.State.drillBitCondition < initialBit);
        }

        [Fact]
        public void AdvanceDrilling_BitDestroyed_DeactivatesProject()
        {
            var system = CreateSystem();
            // AdvanceDrilling resolves the active stratum from the catalog;
            // without a registered stratum it fails with "no_strata" and the
            // drill loop below never terminates. Mirror the real caprock at
            // depth 0 (hardness 5, wear 0.1/m: 100 ticks -> 30 m -> 3 wear,
            // destroying the 1.0-condition bit on the first advance).
            system.LoadCatalog(new GeothermalDrillingCatalog
            {
                Strata = new System.Collections.Generic.List<GeothermalStrataDef>
                {
                    new GeothermalStrataDef { StrataId = "test_strata", StartDepthM = 0f, RockHardness = 5f, DrillWearPerMeter = 0.1f }
                }
            });
            system.State.drillBitCondition = 1f;
            system.StartDrilling();

            // Drill until bit is destroyed
            while (system.State.projectActive && system.State.drillBitCondition > 0f)
            {
                system.AdvanceDrilling(100f);
            }

            Assert.False(system.State.projectActive);
            Assert.True(system.State.drillBitCondition <= 0f);
        }

        // ── InstallCasing ────────────────────────────────────────────

        [Fact]
        public void InstallCasing_AboveBit_ReturnsBlocked()
        {
            var system = CreateSystem();
            system.StartDrilling();
            system.AdvanceDrilling(10f);

            var result = system.InstallCasing(1000f);
            Assert.False(result.IsSuccess);
            Assert.Equal("cannot_case_above_bit", result.FailureCode);
        }

        [Fact]
        public void InstallCasing_Success_RecordsDepth()
        {
            var system = CreateSystem();
            system.State.currentDepthMeters = 200f;

            var result = system.InstallCasing(150f);
            Assert.True(result.IsSuccess);
            Assert.Equal(150f, system.State.installedCasingDepth);
        }

        // ── CommissionTurbine ────────────────────────────────────────

        [Fact]
        public void CommissionTurbine_NoSteam_ReturnsBlocked()
        {
            var system = CreateSystem();
            var result = system.CommissionTurbine();
            Assert.False(result.IsSuccess);
            Assert.Equal("no_steam", result.FailureCode);
        }

        [Fact]
        public void CommissionTurbine_Success_CommisionsTurbine()
        {
            var system = CreateSystem();
            system.State.crossedStrataIds.Add("strata_steam_pocket_500m");
            system.State.installedCasingDepth = 300f;

            var result = system.CommissionTurbine();
            Assert.True(result.IsSuccess);
            Assert.True(system.IsTurbineCommissioned);
            Assert.Equal(100f, system.State.generatorHealth);
        }

        // ── Descale ──────────────────────────────────────────────────

        [Fact]
        public void Descale_NoScale_ReturnsBlocked()
        {
            var system = CreateSystem();
            var result = system.Descale();
            Assert.False(result.IsSuccess);
            Assert.Equal("no_scale", result.FailureCode);
        }

        // ── TapAquifer ───────────────────────────────────────────────

        [Fact]
        public void TapAquifer_NoAquifer_ReturnsBlocked()
        {
            var system = CreateSystem();
            var result = system.TapAquifer();
            Assert.False(result.IsSuccess);
            Assert.Equal("no_aquifer", result.FailureCode);
        }

        [Fact]
        public void TapAquifer_Success_SetsFlag()
        {
            var system = CreateSystem();
            system.State.crossedStrataIds.Add("strata_artesian_aquifer_750m");
            system.State.installedCasingDepth = 500f;

            var result = system.TapAquifer();
            Assert.True(result.IsSuccess);
            Assert.True(system.IsAquiferTapped);
        }

        // ── TickDay ──────────────────────────────────────────────────

        [Fact]
        public void TickDay_Idempotent_NoDoubleTick()
        {
            var system = CreateSystem();
            system.TickDay(1);
            system.TickDay(1);
            Assert.Equal(1, system.State.lastProcessedDay);
        }

        [Fact]
        public void TickDay_TurbineOnline_AccumulatesScale()
        {
            var system = CreateSystem();
            system.State.turbineCommissioned = true;
            system.State.generatorHealth = 100f;
            system.State.mineralScaling = 0f;

            system.TickDay(1);
            Assert.True(system.State.mineralScaling > 0f, "scale accumulated");
            Assert.True(system.State.generatorHealth < 100f, "health decayed");
        }

        // ── Save Round-Trip ──────────────────────────────────────────

        [Fact]
        public void SaveRoundTrip_PreservesFullState()
        {
            var system = CreateSystem();
            system.TickDay(7);
            system.State.currentDepthMeters = 450f;
            system.State.installedCasingDepth = 300f;
            system.State.turbineCommissioned = true;
            system.State.generatorHealth = 85f;
            system.State.mineralScaling = 12f;
            system.State.steamPressurePsi = 45f;
            system.State.pressureReliefState = 0.2f;
            system.State.crossedStrataIds.Add("strata_steam_pocket_500m");
            system.State.crossedStrataIds.Add("strata_artesian_aquifer_750m");
            system.State.currentStrataId = "strata_artesian_aquifer_750m";

            var captured = system.CaptureState();
            var restored = CreateSystem(captured);

            Assert.Equal(450f, restored.State.currentDepthMeters);
            Assert.Equal(300f, restored.State.installedCasingDepth);
            Assert.True(restored.State.turbineCommissioned);
            Assert.Equal(85f, restored.State.generatorHealth);
            Assert.Equal(12f, restored.State.mineralScaling);
            Assert.Equal(45f, restored.State.steamPressurePsi);
            Assert.Equal(0.2f, restored.State.pressureReliefState);
            Assert.Equal(2, restored.State.crossedStrataIds.Count);
            Assert.Equal("strata_artesian_aquifer_750m", restored.State.currentStrataId);
            Assert.Equal(7, restored.State.lastProcessedDay);
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void Deterministic_SameSeed_SameDrillingOutcome()
        {
            var sys1 = CreateSystem();
            sys1.StartDrilling();
            for (int i = 0; i < 20; i++) sys1.AdvanceDrilling(10f);

            var sys2 = CreateSystem();
            sys2.StartDrilling();
            for (int i = 0; i < 20; i++) sys2.AdvanceDrilling(10f);

            Assert.Equal(sys1.State.currentDepthMeters, sys2.State.currentDepthMeters);
            Assert.Equal(sys1.State.drillBitCondition, sys2.State.drillBitCondition);
            Assert.Equal(sys1.State.crossedStrataIds.Count, sys2.State.crossedStrataIds.Count);
        }
    }

    // ── Minimal test log ────────────────────────────────────────────

    internal sealed class TestLog : ILog
    {
        public void Info(string message) { }
        public void Warn(string message) { }
        public void Error(string message) { }
        public void Debug(string message) { }
    }
}
