using System.Collections.Generic;
using Xunit;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public class WastelandMapPersistenceTests
    {
        private static List<MapNode> DummyNodes => new List<MapNode>
        {
            new MapNode { Id = "loc_a", StartingUnlocked = true },
            new MapNode { Id = "loc_b", StartingUnlocked = false },
            new MapNode { Id = "loc_c", StartingUnlocked = false }
        };
        private static List<MapRoute> DummyRoutes => new List<MapRoute>
        {
            new MapRoute { From = "loc_a", To = "loc_b", DistanceKm = 10 },
            new MapRoute { From = "loc_b", To = "loc_c", DistanceKm = 20 }
        };

        [Fact]
        public void Capture_IsSnapshot_DiscoveredList_A()
        {
            var state = new WastelandMapState();
            var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
            sys.Discover("loc_b");
            var snap = sys.CaptureState();
            // Mutate live
            sys.Discover("loc_c");
            Assert.Contains("loc_a", snap.Discovered);
            Assert.Contains("loc_b", snap.Discovered);
            Assert.DoesNotContain("loc_c", snap.Discovered);
            Assert.Contains("loc_c", sys.CaptureState().Discovered);
        }

        [Fact]
        public void Capture_IsSnapshot_DiscoveredList_B()
        {
            var state = new WastelandMapState();
            var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
            sys.Discover("loc_b");
            var snap = sys.CaptureState();
            snap.Discovered.Add("tamper");
            Assert.DoesNotContain("tamper", sys.CaptureState().Discovered);
        }

        [Fact]
        public void RestoreInto_IsDeep()
        {
            var state = new WastelandMapState();
            var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
            sys.Discover("loc_b");
            var snap = sys.CaptureState();
            // Mutate snapshot with a valid node id then restore
            snap.Discovered.Add("loc_c");
            var state2 = new WastelandMapState();
            var sys2 = new WastelandMapSystem(state2, DummyNodes, DummyRoutes);
            sys2.RestoreState(snap);
            Assert.Contains("loc_c", sys2.CaptureState().Discovered);
            snap.Discovered.Remove("loc_c");
            Assert.Contains("loc_c", sys2.CaptureState().Discovered);
        }

        [Fact]
        public void NormalizeAndValidate_KeepsStartingUnlocked()
        {
            var state = new WastelandMapState();
            var sys = new WastelandMapSystem(state, DummyNodes, DummyRoutes);
            // Starting node loc_a should be discovered even if not explicitly discovered
            Assert.Contains("loc_a", sys.CaptureState().Discovered);
        }
    }
}
