// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class MineClearingFlailEngineTests
    {
        private static string FindDataDir()
        {
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            return dir ?? string.Empty;
        }

        [Fact]
        public void StartBreach_ValidatesMinefieldExistence()
        {
            var routeSystem = new RouteInfrastructureSystem();
            var engine = new MineClearingFlailEngine();

            // Segment without minefield
            routeSystem.GetOrCreateSegment("route_clean", "seg_01");
            bool ok = engine.StartBreach("route_clean", "seg_01", "op_1", 1, 1000f, routeSystem, out string err);
            Assert.False(ok);
            Assert.Equal("no_minefield_detected_on_segment", err);

            // Register minefield
            routeSystem.RegisterMinefield("route_danger", "seg_02", 0.7f, day: 1);
            bool ok2 = engine.StartBreach("route_danger", "seg_02", "op_1", 1, 1000f, routeSystem, out string err2);
            Assert.True(ok2, err2);
            Assert.NotNull(engine.StateDto.ActiveBreach);
            Assert.Equal(ProcessState.Running, engine.State);
        }

        [Fact]
        public void TickBreach_AdvancesProgressAndMutatesRouteInfrastructure()
        {
            var routeSystem = new RouteInfrastructureSystem();
            routeSystem.RegisterMinefield("route_danger", "seg_02", 0.8f, day: 1);

            var engine = new MineClearingFlailEngine();
            engine.StartBreach("route_danger", "seg_02", "op_1", 1, 1000f, routeSystem, out _);

            var rng = new SeededRng(999);
            // Tick for 0.1 hours (nominal 8 km/h => 0.8 km = 800m)
            engine.TickBreach(0.1f, 1, routeSystem, null, rng);

            var seg = routeSystem.FindSegment("route_danger", "seg_02");
            Assert.NotNull(seg);
            Assert.True(seg.MinefieldState.ClearedFraction01 >= 0.7f);
            Assert.True(seg.MinefieldState.ResidualRisk01 < 0.8f);
            Assert.Equal("partial", seg.ClearanceState);

            // Complete the remaining distance
            engine.TickBreach(0.05f, 1, routeSystem, null, rng);
            Assert.Equal(1.0f, seg.MinefieldState.ClearedFraction01);
            Assert.Equal("cleared", seg.ClearanceState);
            Assert.Equal(ProcessState.Completed, engine.State);
        }

        [Fact]
        public void TickBreach_SparseMinefield_DoesNotThrowAndClampsToOwnDensity()
        {
            var routeSystem = new RouteInfrastructureSystem();
            routeSystem.RegisterMinefield("route_thin", "seg_thin", 0.01f, day: 1);

            var engine = new MineClearingFlailEngine();
            engine.StartBreach("route_thin", "seg_thin", "op_1", 1, 500f, routeSystem, out _);

            var rng = new SeededRng(31337);
            // A field below the generic residual floor must not crash the clamp
            engine.TickBreach(0.2f, 1, routeSystem, null, rng);

            var seg = routeSystem.FindSegment("route_thin", "seg_thin");
            Assert.NotNull(seg);
            Assert.Equal(1.0f, seg.MinefieldState.ClearedFraction01);
            Assert.True(seg.MinefieldState.ResidualRisk01 <= 0.01f + 1e-6f,
                $"residual {seg.MinefieldState.ResidualRisk01} exceeded the field's own density");
        }

        [Fact]
        public void Maintenance_RestoresChainLinksAndShield()
        {
            var engine = new MineClearingFlailEngine();
            engine.StateDto.ChainLinksRemaining = 10;
            engine.StateDto.BlastShieldIntegrity01 = 0.4f;

            engine.PerformMaintenance("replace_chains");
            Assert.Equal(40, engine.StateDto.ChainLinksRemaining);

            engine.PerformMaintenance("repair_blast_shield");
            Assert.Equal(1.0f, engine.StateDto.BlastShieldIntegrity01);
        }

        [Fact]
        public void Maintenance_UsesActiveModuleDefinition_NotHardcodedConstants()
        {
            var engine = new MineClearingFlailEngine();
            engine.StateDto.ModuleDefinitionId = "module_light_scout_flail";
            engine.StateDto.ChainLinksRemaining = 10;
            engine.StateDto.HydraulicPressureBar = 90.0f;

            engine.PerformMaintenance("replace_chains");
            Assert.Equal(24, engine.StateDto.ChainLinksRemaining); // light scout capacity

            engine.PerformMaintenance("service_hydraulics");
            Assert.Equal(140.0f, engine.StateDto.HydraulicPressureBar); // light scout nominal
        }

        [Fact]
        public void StateCaptureAndRestore_RoundtripsAccurately()
        {
            var engine = new MineClearingFlailEngine();
            engine.StateDto.ChainLinksRemaining = 28;
            engine.StateDto.TotalClearedDistanceKm = 14.5f;

            var captured = engine.CaptureState();
            var restored = new MineClearingFlailEngine(captured);

            Assert.Equal(28, restored.StateDto.ChainLinksRemaining);
            Assert.Equal(14.5f, restored.StateDto.TotalClearedDistanceKm);
        }

        [Fact]
        public void Catalog_LoadsBothModules_AndMatchesEngineAuthority()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");

            var catalog = MineFlailCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(2, catalog.modules.Count);

            var engine = new MineClearingFlailEngine();
            var catalogIds = catalog.ToModuleDefs().Select(d => d.Id).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var engineIds = new[] { "module_heavy_flail_m1", "module_light_scout_flail" }.OrderBy(x => x, StringComparer.Ordinal).ToList();
            Assert.Equal(engineIds, catalogIds);

            var lightScout = catalog.ToModuleDefs().First(d => d.Id == "module_light_scout_flail");
            Assert.Equal(24, lightScout.ChainLinkCapacity);
            Assert.Equal(140.0f, lightScout.HydraulicPressureNominal);
        }
    }
}
