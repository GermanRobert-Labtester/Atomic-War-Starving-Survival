using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CampaignDayCoordinatorPhaseTests
    {
        [Fact]
        public void Owners_TickInPhaseOrder()
        {
            var coord = new CampaignDayCoordinator();
            var order = new List<string>();

            var phase1 = new DelegateOwner(id: "phase1_a", onTick: (day, events) => order.Add("phase1_a"));
            var phase2 = new DelegateOwner(id: "phase2_a", onTick: (day, events) => order.Add("phase2_a"));
            var phase1b = new DelegateOwner(id: "phase1_b", onTick: (day, events) => order.Add("phase1_b"));
            var phase3 = new DelegateOwner(id: "phase3_a", onTick: (day, events) => order.Add("phase3_a"));

            coord.Register("phase1_a", phase1, phase: 1);
            coord.Register("phase2_a", phase2, phase: 2);
            coord.Register("phase1_b", phase1b, phase: 1);
            coord.Register("phase3_a", phase3, phase: 3);

            var result = coord.Advance(1);
            Assert.NotNull(result);
            Assert.Equal(4, result.OwnerCount);

            // Expected order: phase1_a, phase1_b (phase 1, alphabetical), phase2_a (phase 2), phase3_a (phase 3)
            Assert.Equal("phase1_a", order[0]);
            Assert.Equal("phase1_b", order[1]);
            Assert.Equal("phase2_a", order[2]);
            Assert.Equal("phase3_a", order[3]);
        }

        [Fact]
        public void DefaultPhase_Is3()
        {
            var coord = new CampaignDayCoordinator();
            var phaseDefault = new DelegateOwner(id: "default", onTick: (day, events) => { });
            var phase1 = new DelegateOwner(id: "phase1", onTick: (day, events) => { });

            coord.Register("default", phaseDefault); // defaults to phase 3
            coord.Register("phase1", phase1, phase: 1);

            // Verify phase1 (phase 1) ticks before default (phase 3) even though registered after
            var ticks = new List<string>();
            var pd = new DelegateOwner(id: "default", onTick: (day, events) => ticks.Add("default"));
            var p1 = new DelegateOwner(id: "phase1", onTick: (day, events) => ticks.Add("phase1"));

            var coord2 = new CampaignDayCoordinator();
            coord2.Register("default", pd);
            coord2.Register("phase1", p1, phase: 1);
            coord2.Advance(1);

            Assert.Equal("phase1", ticks[0]);
            Assert.Equal("default", ticks[1]);
        }

        [Fact]
        public void AllFivePhases_TickInCorrectOrder()
        {
            var coord = new CampaignDayCoordinator();
            var order = new List<int>();

            coord.Register("phase1", new DelegateOwner(id: "p1", onTick: (d, e) => order.Add(1)), phase: 1);
            coord.Register("phase2", new DelegateOwner(id: "p2", onTick: (d, e) => order.Add(2)), phase: 2);
            coord.Register("phase3", new DelegateOwner(id: "p3", onTick: (d, e) => order.Add(3)), phase: 3);
            coord.Register("phase4", new DelegateOwner(id: "p4", onTick: (d, e) => order.Add(4)), phase: 4);
            coord.Register("phase5", new DelegateOwner(id: "p5", onTick: (d, e) => order.Add(5)), phase: 5);

            coord.Advance(1);
            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, order);
        }

        [Fact]
        public void SamePhase_AlphabeticalById()
        {
            var coord = new CampaignDayCoordinator();
            var order = new List<string>();

            coord.Register("zulu", new DelegateOwner(id: "z", onTick: (d, e) => order.Add("zulu")), phase: 2);
            coord.Register("alpha", new DelegateOwner(id: "a", onTick: (d, e) => order.Add("alpha")), phase: 2);
            coord.Register("bravo", new DelegateOwner(id: "b", onTick: (d, e) => order.Add("bravo")), phase: 2);

            coord.Advance(1);
            Assert.Equal("alpha", order[0]);
            Assert.Equal("bravo", order[1]);
            Assert.Equal("zulu", order[2]);
        }

        private sealed class DelegateOwner : IDayAdvanceOwner
        {
            private readonly string _id;
            private readonly System.Action<int, List<DayStateChangeEvent>> _onTick;

            public DelegateOwner(string id, System.Action<int, List<DayStateChangeEvent>> onTick)
            {
                _id = id;
                _onTick = onTick;
            }

            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events) => _onTick(day, events);
        }
    }
}
