using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.World;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// P2 alias isolation for Caravan, WastelandMap, Waystation, TravelingCaravan.
    /// Follows Direction A (mutate live → snapshot unchanged) and Direction B (mutate snapshot → live unchanged).
    /// </summary>
    public class SaveSnapshotAliasCaravanWastelandTests
    {
        private static T CloneViaJson<T>(T src) where T : class, new()
        {
            var s = new SystemTextJsonSerializer();
            return s.Deserialize<T>(s.Serialize(src)) ?? new T();
        }

        [Fact]
        public void WastelandMapState_Clone_IsDeep_ListReference()
        {
            var src = new WastelandMapState();
            src.Discovered.Add("loc_a");
            var clone = src.Capture();
            Assert.False(ReferenceEquals(src.Discovered, clone.Discovered));
            clone.Discovered.Add("tamper");
            Assert.Single(src.Discovered);
            Assert.Equal(2, clone.Discovered.Count);
            src.Discovered.Add("live_mutate");
            Assert.Equal(2, clone.Discovered.Count);
            Assert.DoesNotContain("live_mutate", clone.Discovered);
            Assert.Contains("tamper", clone.Discovered);
        }

        [Fact]
        public void WastelandMapSystem_Capture_IsSnapshot_List()
        {
            var nodes = new List<MapNode> { new MapNode { Id = "loc_a", StartingUnlocked = true }, new MapNode { Id = "loc_b" } };
            var routes = new List<MapRoute>();
            var state = new WastelandMapState();
            state.Discovered.Add("loc_a");
            var sys = new WastelandMapSystem(state, nodes, routes);
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap.Discovered, sys.CaptureState().Discovered));
            snap.Discovered.Add("tamper");
            Assert.DoesNotContain("tamper", sys.CaptureState().Discovered);
            // Direction B
            var live = new WastelandMapState();
            live.Discovered.Add("live_a");
            var snap2 = CloneViaJson(live);
            snap2.Discovered.Add("snap_b");
            Assert.Single(live.Discovered);
        }

        [Fact]
        public void CaravanTradeState_Clone_IsDeep_ListReference()
        {
            var src = new CaravanTradeState();
            src.Committed.Add(new CaravanCommittedTrade { QuoteId = "q1" });
            var clone = src.Capture();
            Assert.False(ReferenceEquals(src.Committed, clone.Committed));
            clone.Committed.Add(new CaravanCommittedTrade { QuoteId = "tamper" });
            Assert.Single(src.Committed);
            Assert.Equal(2, clone.Committed.Count);
            src.Committed.Add(new CaravanCommittedTrade { QuoteId = "live2" });
            Assert.Equal(2, clone.Committed.Count);
            Assert.Equal(2, src.Committed.Count);
            Assert.DoesNotContain(clone.Committed, c => c.QuoteId == "live2");
            Assert.DoesNotContain(src.Committed, c => c.QuoteId == "tamper");
        }

        [Fact]
        public void CaravanSystem_Capture_ListReference_IsSnapshot()
        {
            var state = new CaravanTradeState();
            state.Committed.Add(new CaravanCommittedTrade { QuoteId = "q1" });
            var sys = new CaravanAtomicTrader(state);
            var snap = sys.CaptureState();
            var snap2 = sys.CaptureState();
            Assert.False(ReferenceEquals(snap.Committed, snap2.Committed));
            snap.Committed.Add(new CaravanCommittedTrade { QuoteId = "tamper" });
            Assert.Single(sys.CaptureState().Committed);
        }

        [Fact]
        public void WaystationSystem_Capture_IsSnapshot_ArrayClone()
        {
            var sys = new WaystationSystem();
            sys.Unlock();
            sys.AssignWatch(new[] { "a", "b" });
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap.watchSurvivorIds, sys.State.watchSurvivorIds));
            // Mutate snapshot array
            snap.watchSurvivorIds[0] = "tamper";
            Assert.Equal("a", sys.State.watchSurvivorIds[0]);
            // Mutate live
            sys.State.watchSurvivorIds[0] = "live_mutate";
            Assert.Equal("tamper", snap.watchSurvivorIds[0]);
        }

        [Fact]
        public void TravelingCaravanSystem_Capture_IsDeep()
        {
            var sys = new TravelingCaravanSystem();
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap.activeCaravans, sys.CaptureState().activeCaravans));
            snap.activeCaravans.Add(new CaravanEntry { caravanId = "tamper" });
            Assert.Empty(sys.CaptureState().activeCaravans);
            // Also verify inner list deep copy via system API: add a caravan then snapshot, mutate snapshot's routeNodeIds
            // We can't easily add via public API without catalog, so we test the state's clone via json for inner.
            var state = new TravelingCaravanState();
            state.activeCaravans.Add(new CaravanEntry { caravanId = "c1", routeNodeIds = new List<string> { "n1" } });
            var clone = CloneViaJson(state);
            clone.activeCaravans[0].routeNodeIds.Add("tamper");
            Assert.Single(state.activeCaravans[0].routeNodeIds);
        }
    }
}
