using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public class WastelandMapPersistenceTests
    {
        private static List<MapNode> DummyNodes => new List<MapNode>
        {
            new MapNode { Id = "loc_a", DisplayName = "Holdfast Base", StartingUnlocked = true, Danger = MapNodeDanger.None },
            new MapNode { Id = "loc_b", DisplayName = "Abandoned Outpost", StartingUnlocked = false, Danger = MapNodeDanger.Low },
            new MapNode { Id = "loc_c", DisplayName = "Arsenal Ruin", StartingUnlocked = false, Danger = MapNodeDanger.Medium },
            new MapNode { Id = "loc_d", DisplayName = "Hazard Valley", StartingUnlocked = false, Danger = MapNodeDanger.High },
            new MapNode { Id = "loc_locked_static", DisplayName = "Contested Gate", StartingUnlocked = false, Danger = MapNodeDanger.Locked }
        };

        private static List<MapRoute> DummyRoutes => new List<MapRoute>
        {
            new MapRoute { From = "loc_a", To = "loc_b", DistanceKm = 10 },
            new MapRoute { From = "loc_b", To = "loc_c", DistanceKm = 15 },
            new MapRoute { From = "loc_c", To = "loc_d", DistanceKm = 20 },
            new MapRoute { From = "loc_a", To = "loc_locked_static", DistanceKm = 5 }
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
            Assert.True(sys.IsDiscovered("loc_a"));
        }

        [Fact]
        public void Discovered_SaveLoadRoundTrip_PreservesDiscoveredNodes()
        {
            var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys1.Discover("loc_b");
            sys1.Discover("loc_c");

            var state = sys1.CaptureState();

            var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys2.RestoreState(state);

            Assert.True(sys2.IsDiscovered("loc_a")); // starting unlocked
            Assert.True(sys2.IsDiscovered("loc_b")); // discovered
            Assert.True(sys2.IsDiscovered("loc_c")); // discovered
            Assert.False(sys2.IsDiscovered("loc_d")); // undiscovered
        }

        [Fact]
        public void Locked_SaveLoadRoundTrip_PreservesStaticAndDynamicLocks()
        {
            var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            // loc_locked_static is locked by definition
            Assert.True(sys1.IsLocked("loc_locked_static"));

            // Dynamically lock loc_b and unlock loc_locked_static
            sys1.Lock("loc_b");
            sys1.Unlock("loc_locked_static");

            Assert.True(sys1.IsLocked("loc_b"));
            Assert.False(sys1.IsLocked("loc_locked_static"));

            var state = sys1.CaptureState();

            var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys2.RestoreState(state);

            Assert.True(sys2.IsLocked("loc_b"));
            Assert.False(sys2.IsLocked("loc_locked_static"));
            Assert.False(sys2.IsLocked("loc_a"));
        }

        [Fact]
        public void Completed_SaveLoadRoundTrip_PreservesCompletedNodes()
        {
            var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys1.Complete("loc_b");
            sys1.Complete("loc_c");

            Assert.True(sys1.IsCompleted("loc_b"));
            Assert.True(sys1.IsCompleted("loc_c"));
            Assert.False(sys1.IsCompleted("loc_d"));
            // Completing automatically implies discovery
            Assert.True(sys1.IsDiscovered("loc_b"));
            Assert.True(sys1.IsDiscovered("loc_c"));

            var state = sys1.CaptureState();

            var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys2.RestoreState(state);

            Assert.True(sys2.IsCompleted("loc_b"));
            Assert.True(sys2.IsCompleted("loc_c"));
            Assert.False(sys2.IsCompleted("loc_a"));
            Assert.False(sys2.IsCompleted("loc_d"));
            Assert.True(sys2.IsDiscovered("loc_b"));
            Assert.True(sys2.IsDiscovered("loc_c"));
        }

        [Fact]
        public void MixedState_SaveLoadRoundTrip_ResolvesCorrectNodeStatuses()
        {
            var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            // loc_a: starting unlocked (Discovered)
            // loc_b: Completed
            sys1.Complete("loc_b");
            // loc_c: Discovered
            sys1.Discover("loc_c");
            // loc_d: Reachable from loc_c (Available)
            // loc_locked_static: Locked
            sys1.Lock("loc_locked_static");

            Assert.Equal(MapNodeStatusKind.Discovered, sys1.ResolveNodeStatus("loc_a"));
            Assert.Equal(MapNodeStatusKind.Completed, sys1.ResolveNodeStatus("loc_b"));
            Assert.Equal(MapNodeStatusKind.Discovered, sys1.ResolveNodeStatus("loc_c"));
            Assert.Equal(MapNodeStatusKind.Available, sys1.ResolveNodeStatus("loc_d"));
            Assert.Equal(MapNodeStatusKind.Locked, sys1.ResolveNodeStatus("loc_locked_static"));

            var state = sys1.CaptureState();

            var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys2.RestoreState(state);

            Assert.Equal(MapNodeStatusKind.Discovered, sys2.ResolveNodeStatus("loc_a"));
            Assert.Equal(MapNodeStatusKind.Completed, sys2.ResolveNodeStatus("loc_b"));
            Assert.Equal(MapNodeStatusKind.Discovered, sys2.ResolveNodeStatus("loc_c"));
            Assert.Equal(MapNodeStatusKind.Available, sys2.ResolveNodeStatus("loc_d"));
            Assert.Equal(MapNodeStatusKind.Locked, sys2.ResolveNodeStatus("loc_locked_static"));
        }

        [Fact]
        public void JsonEnvelope_SerializationAndChecksum_RoundTripsSuccessfully()
        {
            var sys1 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys1.Discover("loc_b");
            sys1.Complete("loc_c");
            sys1.Lock("loc_d");

            var state1 = sys1.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state1);

            Assert.False(string.IsNullOrWhiteSpace(json));
            Assert.Contains("loc_b", json);
            Assert.Contains("loc_c", json);
            Assert.Contains("loc_d", json);

            var state2 = serializer.Deserialize<WastelandMapState>(json);
            Assert.NotNull(state2);

            var sys2 = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys2.RestoreState(state2);

            Assert.True(sys2.IsDiscovered("loc_b"));
            Assert.True(sys2.IsCompleted("loc_c"));
            Assert.True(sys2.IsLocked("loc_d"));
        }

        [Fact]
        public void SnapshotIsolation_MutatingCapturedState_DoesNotAffectActiveSystem()
        {
            var sys = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            sys.Discover("loc_b");
            sys.Complete("loc_c");
            sys.Lock("loc_d");

            var snap = sys.CaptureState();

            // Mutate snapshot
            snap.Discovered.Remove("loc_b");
            snap.Completed.Remove("loc_c");
            snap.Locked.Remove("loc_d");
            snap.Discovered.Add("tampered_id");

            // Live system remains intact
            Assert.True(sys.IsDiscovered("loc_b"));
            Assert.True(sys.IsCompleted("loc_c"));
            Assert.True(sys.IsLocked("loc_d"));
            Assert.False(sys.IsDiscovered("tampered_id"));
        }

        [Fact]
        public void Events_FireOnCompleteAndLockChanged()
        {
            var sys = new WastelandMapSystem(new WastelandMapState(), DummyNodes, DummyRoutes);
            string? completedNode = null;
            string? lockChangedNode = null;
            bool? lockChangedState = null;

            sys.OnNodeCompleted += id => completedNode = id;
            sys.OnNodeLockChanged += (id, locked) =>
            {
                lockChangedNode = id;
                lockChangedState = locked;
            };

            sys.Complete("loc_b");
            Assert.Equal("loc_b", completedNode);

            sys.Lock("loc_c");
            Assert.Equal("loc_c", lockChangedNode);
            Assert.True(lockChangedState);

            sys.Unlock("loc_c");
            Assert.Equal("loc_c", lockChangedNode);
            Assert.False(lockChangedState);
        }
    }
}
