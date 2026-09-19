// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Underground;
using Xunit;

namespace Ashfall.Core.Tests.Underground
{
    public sealed class TunnelNetworkSystemTests
    {
        [Fact]
        public void RegisterJunction_And_RegisterSegment_ConnectsNodes()
        {
            var system = new TunnelNetworkSystem();
            var j1 = system.RegisterJunction("junc_shelter", "Shelter Access");
            var j2 = system.RegisterJunction("junc_vault", "Old Vault Entry", hasResource: true, resourceType: "seeds");

            var seg = system.RegisterSegment("seg_1", "Maintenance Tunnel A", "junc_shelter", "junc_vault", lengthHours: 3f, difficulty: 2);

            Assert.NotNull(seg);
            Assert.Equal("seg_1", seg.SegmentId);
            Assert.Equal(1, system.TotalSegmentCount);
            Assert.Equal(2, system.JunctionCount);
            Assert.Contains("seg_1", j1.ConnectedSegmentIds);
            Assert.Contains("seg_1", j2.ConnectedSegmentIds);
        }

        [Fact]
        public void CanTraverse_RejectsUndiscoveredOrCollapsedSegments()
        {
            var system = new TunnelNetworkSystem();
            system.RegisterSegment("seg_2", "Dark Passage", "node_a", "node_b", integrity: 100f);

            // Undiscovered
            var resUndiscovered = system.CanTraverse("node_a", "node_b");
            Assert.False(resUndiscovered.CanTraverse);

            // Discovered but collapsed
            system.DiscoverSegment("seg_2");
            var seg = system.GetDiscoveredSegments().First();
            seg.Status = TunnelStatus.Collapsed;

            var resCollapsed = system.CanTraverse("node_a", "node_b");
            Assert.False(resCollapsed.CanTraverse);
        }

        [Fact]
        public void DiscoverSegment_EnablesTraversal()
        {
            var system = new TunnelNetworkSystem();
            system.RegisterSegment("seg_3", "Service Tunnel", "hub_1", "hub_2", lengthHours: 1.5f);

            system.DiscoverSegment("seg_3");

            var res = system.CanTraverse("hub_1", "hub_2");
            Assert.True(res.CanTraverse);
            Assert.Equal(1.5f, res.TravelTimeHours);
        }

        [Fact]
        public void TickDay_DegradesIntegrityAndCollapsesWhenDepleted()
        {
            var system = new TunnelNetworkSystem();
            var seg = system.RegisterSegment("seg_fragile", "Weak Shaft", "a", "b", integrity: 0.5f);

            TunnelSegment? collapsed = null;
            system.OnSegmentCollapsed += s => collapsed = s;

            system.TickDay(currentDay: 2);

            Assert.Equal(0f, seg.StructuralIntegrity);
            Assert.Equal(TunnelStatus.Collapsed, seg.Status);
            Assert.NotNull(collapsed);
            Assert.Equal("seg_fragile", collapsed.SegmentId);
        }

        [Fact]
        public void RepairSegment_RestoresIntegrityAndClearsCollapse()
        {
            var system = new TunnelNetworkSystem();
            var seg = system.RegisterSegment("seg_ruin", "Ruin Path", "x", "y", integrity: 0f);
            Assert.Equal(TunnelStatus.Collapsed, seg.Status);

            bool repaired = system.RepairSegment("seg_ruin", repairAmount: 60f);

            Assert.True(repaired);
            Assert.Equal(60f, seg.StructuralIntegrity);
            Assert.Equal(TunnelStatus.Clear, seg.Status);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new TunnelNetworkSystem();
            system1.RegisterJunction("junc_metro", "Subway Station", hasResource: true, resourceType: "scrap");
            system1.RegisterSegment("seg_metro", "Metro Line 4", "shelter", "junc_metro", 2.5f, 3, 85f);
            system1.DiscoverSegment("seg_metro");

            var state = system1.CaptureState();
            Assert.Single(state.Junctions);
            Assert.Single(state.Segments);

            var system2 = new TunnelNetworkSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TotalSegmentCount);
            Assert.Equal(1, system2.DiscoveredSegmentCount);
            var res = system2.CanTraverse("shelter", "junc_metro");
            Assert.True(res.CanTraverse);
            Assert.Equal(2.5f, res.TravelTimeHours);
        }
    }
}
