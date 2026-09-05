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
    public sealed class RailGrindingEngineTests
    {
        private static string FindDataDir()
        {
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir);
            return dir ?? string.Empty;
        }

        [Fact]
        public void StartGrindingJob_RequiresRailMode()
        {
            var routeSystem = new RouteInfrastructureSystem();
            routeSystem.GetOrCreateSegment("route_dirt", "seg_01", "road");

            var engine = new RailGrindingEngine();
            bool ok = engine.StartGrindingJob("route_dirt", "seg_01", "op_1", 10f, routeSystem, out string err);
            Assert.False(ok);
            Assert.Equal("segment_is_not_rail", err);

            // Register rail segment
            routeSystem.RegisterRailSegment("rail_trunk_01", "sector_1", 0.90f, 25f, 1);
            bool ok2 = engine.StartGrindingJob("rail_trunk_01", "sector_1", "op_1", 10f, routeSystem, out string err2);
            Assert.True(ok2, err2);
            Assert.NotNull(engine.StateDto.ActiveJob);
            Assert.Equal(ProcessState.Running, engine.State);
        }

        [Fact]
        public void StartGrindingJob_BlockedAtHeadTargetRoughness()
        {
            var routeSystem = new RouteInfrastructureSystem();
            // Corundum head targets roughness 0.20; within +0.02 tolerance there is
            // nothing left to grind.
            routeSystem.RegisterRailSegment("rail_smooth", "sector_s", 0.21f, 40f, 1);
            routeSystem.RegisterRailSegment("rail_rough", "sector_r", 0.45f, 25f, 1);

            var engine = new RailGrindingEngine();
            bool blocked = engine.StartGrindingJob("rail_smooth", "sector_s", "op_1", 5f, routeSystem, out string err);
            Assert.False(blocked);
            Assert.Equal("rail_already_at_target_smoothness", err);

            bool ok = engine.StartGrindingJob("rail_rough", "sector_r", "op_1", 5f, routeSystem, out string err2);
            Assert.True(ok, err2);
        }

        [Fact]
        public void TickGrinding_ReducesRoughnessAndWearsStone()
        {
            var routeSystem = new RouteInfrastructureSystem();
            routeSystem.RegisterRailSegment("rail_trunk_01", "sector_1", 0.85f, 25f, 1);

            var engine = new RailGrindingEngine();
            engine.StartGrindingJob("rail_trunk_01", "sector_1", "op_1", 10f, routeSystem, out _);

            var rng = new SeededRng(101);
            // 2.5 hours at 4 km/h = 10 km (complete pass)
            engine.TickGrinding(2.5f, 1, routeSystem, null, rng);

            var seg = routeSystem.FindSegment("rail_trunk_01", "sector_1");
            Assert.NotNull(seg);
            Assert.True(seg.RailCondition.RoughnessIndex < 0.85f);
            Assert.True(seg.RailCondition.SafeSpeedLimitKph > 25f);
            Assert.True(engine.StateDto.StoneDiameterMm < 250f);
            Assert.Equal(ProcessState.Completed, engine.State);
        }

        [Fact]
        public void TickGrinding_SpeedUpgrade_CappedByRouteDesignLimit()
        {
            var routeSystem = new RouteInfrastructureSystem();
            // Head allows up to 80 km/h, but this corridor was engineered for 40.
            routeSystem.RegisterRailSegment("rail_capped", "sector_c", 0.85f, 25f, 1, designSpeedKph: 40f);

            var engine = new RailGrindingEngine();
            engine.StartGrindingJob("rail_capped", "sector_c", "op_1", 10f, routeSystem, out _);

            var rng = new SeededRng(202);
            engine.TickGrinding(2.5f, 1, routeSystem, null, rng);

            var seg = routeSystem.FindSegment("rail_capped", "sector_c");
            Assert.NotNull(seg);
            Assert.True(seg.RailCondition.SafeSpeedLimitKph > 25f, "grinding should still improve speed");
            Assert.True(seg.RailCondition.SafeSpeedLimitKph <= 40f,
                $"speed {seg.RailCondition.SafeSpeedLimitKph} exceeded the 40 km/h route design limit");

            // Without a recorded design limit the head cap (80) is the ceiling
            routeSystem.RegisterRailSegment("rail_open", "sector_o", 0.85f, 25f, 1);
            engine.PerformMaintenance("replace_stones");
            engine.StartGrindingJob("rail_open", "sector_o", "op_1", 10f, routeSystem, out _);
            engine.TickGrinding(2.5f, 1, routeSystem, null, new SeededRng(203));
            var openSeg = routeSystem.FindSegment("rail_open", "sector_o");
            Assert.NotNull(openSeg);
            Assert.True(openSeg.RailCondition.SafeSpeedLimitKph <= 80f);
        }

        [Fact]
        public void Maintenance_RestoresStoneDiameter()
        {
            var engine = new RailGrindingEngine();
            engine.StateDto.StoneDiameterMm = 115.0f; // below operational threshold
            Assert.False(engine.IsOperational);

            engine.PerformMaintenance("replace_stones");
            Assert.Equal(250.0f, engine.StateDto.StoneDiameterMm);
            Assert.True(engine.IsOperational);
        }

        [Fact]
        public void StateCaptureAndRestore_RoundtripsAccurately()
        {
            var engine = new RailGrindingEngine();
            engine.StateDto.StoneDiameterMm = 210.0f;
            engine.StateDto.MotorRpm = 3800.0f;

            var captured = engine.CaptureState();
            var restored = new RailGrindingEngine(captured);

            Assert.Equal(210.0f, restored.StateDto.StoneDiameterMm);
            Assert.Equal(3800.0f, restored.StateDto.MotorRpm);
        }

        [Fact]
        public void Catalog_LoadsCorundumHead_AndMatchesEngineAuthority()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");

            var catalog = RailGrindingCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Single(catalog.grinding_heads);
            Assert.Equal(2, catalog.rail_profiles.Count);

            var head = catalog.ToHeadDefs().First();
            Assert.Equal("grinding_head_corundum_m4", head.Id);
            Assert.Equal(0.20f, head.TargetRoughness);
            Assert.Equal(80.0f, head.MaxSpeedUpgradeKph);

            var engine = new RailGrindingEngine();
            Assert.Equal(head.BaseWorkRateKmPerHour, engine.GetHead("grinding_head_corundum_m4")!.BaseWorkRateKmPerHour);
        }
    }
}
