// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class RouteInfrastructureSystemTests
    {
        [Fact]
        public void MinefieldClearance_ProgressivelyReducesResidualRisk()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterMinefield("route_alpha", "seg_01", 0.8f, new List<string> { "blast_mine" }, day: 5);

            var seg = system.FindSegment("route_alpha", "seg_01");
            Assert.NotNull(seg);
            Assert.Equal(0.8f, seg.MinefieldState.Density01);
            Assert.Equal(0.8f, seg.MinefieldState.ResidualRisk01);
            Assert.Equal("unbreached", seg.ClearanceState);

            // Breach 50%
            system.ApplyMineClearance("route_alpha", "seg_01", 500f, 1000f, clearanceEfficiency: 0.95f, day: 6);
            Assert.Equal(0.5f, seg.MinefieldState.ClearedFraction01);
            Assert.Equal("partial", seg.ClearanceState);
            Assert.True(seg.MinefieldState.ResidualRisk01 < 0.8f);

            // Complete breach (remaining 500m)
            system.ApplyMineClearance("route_alpha", "seg_01", 500f, 1000f, clearanceEfficiency: 0.95f, day: 7);
            Assert.Equal(1.0f, seg.MinefieldState.ClearedFraction01);
            Assert.Equal("cleared", seg.ClearanceState);
            Assert.True(seg.MinefieldState.ResidualRisk01 <= 0.05f);
        }

        [Fact]
        public void MinefieldClearance_SparseField_ClampsToOwnDensityWithoutThrowing()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterMinefield("route_thin", "seg_t", 0.005f, day: 1);

            system.ApplyMineClearance("route_thin", "seg_t", 1000f, 1000f, clearanceEfficiency: 0.9f, day: 2);

            var seg = system.FindSegment("route_thin", "seg_t");
            Assert.NotNull(seg);
            Assert.Equal("cleared", seg.ClearanceState);
            Assert.True(seg.MinefieldState.ResidualRisk01 <= 0.005f + 1e-6f);
        }

        [Fact]
        public void RailGrinding_ReducesRoughnessAndIncreasesSpeedLimit()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterRailSegment("rail_line_1", "sector_a", initialRoughness: 0.9f, safeSpeedKph: 20f, day: 10);

            var seg = system.FindSegment("rail_line_1", "sector_a");
            Assert.NotNull(seg);
            Assert.Equal(0.9f, seg.RailCondition.RoughnessIndex);
            Assert.Equal(20f, seg.RailCondition.SafeSpeedLimitKph);

            // Grind 10 km on a 10 km segment
            system.ApplyRailGrinding("rail_line_1", "sector_a", progressKm: 10f, segmentLengthKm: 10f, roughnessReduction: 0.4f, speedBonusKph: 35f, maxSpeedCapKph: 70f, day: 11);

            Assert.Equal(1.0f, seg.RailCondition.GroundFraction01);
            Assert.InRange(seg.RailCondition.RoughnessIndex, 0.49f, 0.51f);
            Assert.InRange(seg.RailCondition.SafeSpeedLimitKph, 54f, 56f);
        }

        [Fact]
        public void RailGrinding_DesignLimitBelowHeadCap_CapsSpeed()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterRailSegment("rail_design", "sector_d", 0.9f, 20f, day: 1, designSpeedKph: 35f);

            system.ApplyRailGrinding("rail_design", "sector_d", progressKm: 10f, segmentLengthKm: 10f, roughnessReduction: 0.4f, speedBonusKph: 60f, maxSpeedCapKph: 80f, day: 2);

            var seg = system.FindSegment("rail_design", "sector_d");
            Assert.NotNull(seg);
            Assert.True(seg.RailCondition.SafeSpeedLimitKph <= 35f,
                $"speed {seg.RailCondition.SafeSpeedLimitKph} must respect the 35 km/h design limit");
            Assert.True(seg.RailCondition.SafeSpeedLimitKph > 20f);
        }

        [Fact]
        public void RegisterRailSegment_UpgradesPreExistingRoadSegmentToRailMode()
        {
            var system = new RouteInfrastructureSystem();
            // Same key first created as a plain road segment
            system.GetOrCreateSegment("rail_col", "seg_k", "road");

            system.RegisterRailSegment("rail_col", "seg_k", 0.8f, 30f, day: 1);

            var seg = system.FindSegment("rail_col", "seg_k");
            Assert.NotNull(seg);
            Assert.Equal("rail", seg.Mode);
            Assert.Equal(0.8f, seg.RailCondition.RoughnessIndex);
        }

        [Fact]
        public void CanTraverse_EvaluatesImpassableAndCapabilities()
        {
            var system = new RouteInfrastructureSystem();
            var seg = system.GetOrCreateSegment("route_blocked", "seg_x", "road");
            seg.HazardFlags.Add("impassable");

            Assert.False(system.CanTraverse("route_blocked"));

            var railSeg = system.GetOrCreateSegment("rail_only", "seg_y", "rail");
            Assert.False(system.CanTraverse("rail_only", new[] { "wheeled" }));
            Assert.True(system.CanTraverse("rail_only", new[] { "rail_capable" }));
        }

        [Fact]
        public void GetTravelModifier_IsBoundedAndReflectsRouteSpeed()
        {
            var system = new RouteInfrastructureSystem();
            // Unknown route: neutral modifier
            Assert.Equal(1.0f, system.GetTravelModifier("route_unknown"));

            // Damaged rail (25 km/h) is slower than the 50 km/h baseline
            system.RegisterRailSegment("rail_slow", "s1", 0.9f, 25f, 1);
            Assert.True(system.GetTravelModifier("rail_slow") > 1.0f);

            // Reprofiled express corridor (80 km/h) beats the baseline
            system.RegisterRailSegment("rail_fast", "s1", 0.15f, 80f, 1);
            float express = system.GetTravelModifier("rail_fast");
            Assert.True(express < 1.0f);
            Assert.InRange(express, 0.25f, 3.0f);
        }

        [Fact]
        public void GetHazardModifier_SpikesForActiveMinefields_AndDropsAfterClearance()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterMinefield("route_mined", "s1", 0.8f, day: 1);
            float before = system.GetHazardModifier("route_mined");
            Assert.True(before > 1.0f);

            system.ApplyMineClearance("route_mined", "s1", 1000f, 1000f, 0.95f, 2);
            float after = system.GetHazardModifier("route_mined");
            Assert.True(after < before);

            // Untouched route is unaffected
            Assert.Equal(1.0f, system.GetHazardModifier("route_other"));
        }

        [Fact]
        public void CaptureState_IsDeterministicAcrossRepeatedCaptures()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterMinefield("route_b", "seg_1", 0.7f, day: 1);
            system.RegisterMinefield("route_a", "seg_2", 0.5f, day: 2);
            system.RegisterRailSegment("route_a", "seg_1", 0.9f, 30f, 3);

            var first = system.CaptureState();
            var second = system.CaptureState();

            Assert.Equal(first.Segments.Count, second.Segments.Count);
            for (int i = 0; i < first.Segments.Count; i++)
            {
                Assert.Equal(first.Segments[i].RouteId, second.Segments[i].RouteId);
                Assert.Equal(first.Segments[i].SegmentId, second.Segments[i].SegmentId);
                Assert.Equal(first.Segments[i].MinefieldState.ResidualRisk01, second.Segments[i].MinefieldState.ResidualRisk01);
                Assert.Equal(first.Segments[i].RailCondition.RoughnessIndex, second.Segments[i].RailCondition.RoughnessIndex);
            }
            // Registration order must not leak: keys are sorted ordinally
            Assert.True(string.CompareOrdinal(
                first.Segments[0].RouteId + ":" + first.Segments[0].SegmentId,
                first.Segments[1].RouteId + ":" + first.Segments[1].SegmentId) < 0);
        }

        [Fact]
        public void RestoreState_DoesNotAliasCapturedLists()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterMinefield("route_1", "seg_1", 0.75f, day: 3);

            var captured = system.CaptureState();
            captured.Segments[0].MinefieldState.ClearedFraction01 = 1f;
            captured.Segments.Add(new RouteSegmentInfrastructureRecord { RouteId = "ghost", SegmentId = "seg" });

            var seg = system.FindSegment("route_1", "seg_1");
            Assert.NotNull(seg);
            Assert.Equal(0f, seg.MinefieldState.ClearedFraction01);
            Assert.Null(system.FindSegment("ghost", "seg"));
        }

        [Fact]
        public void StateCaptureAndRestore_RoundtripsIdentically()
        {
            var system = new RouteInfrastructureSystem();
            system.RegisterMinefield("route_1", "seg_1", 0.75f, new List<string> { "tripwire" }, day: 3);
            system.RegisterRailSegment("route_2", "seg_2", 0.85f, 30f, day: 4);

            var captured = system.CaptureState();
            Assert.Equal(2, captured.Segments.Count);

            var restored = new RouteInfrastructureSystem(captured);
            var seg1 = restored.FindSegment("route_1", "seg_1");
            var seg2 = restored.FindSegment("route_2", "seg_2");

            Assert.NotNull(seg1);
            Assert.NotNull(seg2);
            Assert.Equal(0.75f, seg1.MinefieldState.Density01);
            Assert.Equal(0.85f, seg2.RailCondition.RoughnessIndex);
            Assert.Equal(30f, seg2.RailCondition.SafeSpeedLimitKph);
        }
    }
}
