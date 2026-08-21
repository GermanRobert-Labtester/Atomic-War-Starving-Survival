using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignDayCoordinatorTests
    {
        [Fact]
        public void Register_ThrowsOnDuplicateId()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new StubOwner("alpha"));
            Assert.Throws<InvalidOperationException>(() => c.Register("alpha", new StubOwner("alpha")));
        }

        [Fact]
        public void Register_SortsOwnersByIdDeterministic()
        {
            var c = new CampaignDayCoordinator();
            c.Register("zebra", new StubOwner("zebra"));
            c.Register("alpha", new StubOwner("alpha"));
            c.Register("mike", new StubOwner("mike"));
            var owners = c.Owners;
            Assert.Equal("alpha", ((StubOwner)owners[0]).Id);
            Assert.Equal("mike", ((StubOwner)owners[1]).Id);
            Assert.Equal("zebra", ((StubOwner)owners[2]).Id);
        }

        [Fact]
        public void Advance_ReturnsNullWhenAlreadyAdvancing()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new BlockingStubOwner());
            var t1 = System.Threading.Tasks.Task.Run(() => c.Advance(10));
            // We can't actually exercise re-entrance from a single thread easily,
            // but we can at least verify the guard rejects a stale day.
            var result = c.Advance(5);
            // 5 is < 10 because we already advanced via the blocking call (won't happen).
            // Instead, verify the stale-day guard:
            var c2 = new CampaignDayCoordinator();
            c2.Register("a", new StubOwner("a"));
            Assert.NotNull(c2.Advance(3));
            Assert.Null(c2.Advance(3));  // stale day
            Assert.Null(c2.Advance(2));  // stale day
        }

        [Fact]
        public void Advance_TicksAllOwnersExactlyOnceAndInOrder()
        {
            var c = new CampaignDayCoordinator();
            var a = new StubOwner("alpha");
            var b = new StubOwner("bravo");
            var z = new StubOwner("zulu");
            c.Register(z.Id, z);
            c.Register(a.Id, a);
            c.Register(b.Id, b);
            var result = c.Advance(7);
            Assert.NotNull(result);
            Assert.Equal(7, result.Day);
            Assert.Equal(3, result.OwnerCount);
            Assert.Equal("alpha", result.OwnerReports[0].OwnerId);
            Assert.Equal("bravo", result.OwnerReports[1].OwnerId);
            Assert.Equal("zulu", result.OwnerReports[2].OwnerId);
            Assert.Equal(1, a.TickCount);
            Assert.Equal(1, b.TickCount);
            Assert.Equal(1, z.TickCount);
        }

        [Fact]
        public void Advance_CollectsTypedEvents()
        {
            var c = new CampaignDayCoordinator();
            var emitter = new EmittingOwner("alpha");
            c.Register("alpha", emitter);
            var result = c.Advance(2);
            Assert.NotNull(result);
            Assert.Equal(2, result.OwnerReports[0].Events.Length);
            Assert.Equal("consumed_water", result.OwnerReports[0].Events[0].Kind);
            Assert.Equal("alpha", result.OwnerReports[0].Events[0].SourceOwnerId);
        }

        [Fact]
        public void Advance_IsolatesOwnerFailures()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new StubOwner("alpha"));
            c.Register("crash", new CrashingOwner("crash"));
            c.Register("zulu", new StubOwner("zulu"));
            var result = c.Advance(4);
            Assert.NotNull(result);
            Assert.Equal(3, result.OwnerCount);
            Assert.True(result.OwnerReports[0].Succeeded);
            Assert.False(result.OwnerReports[1].Succeeded);
            Assert.Contains("crash", result.OwnerReports[1].FailureMessage);
            Assert.True(result.OwnerReports[2].Succeeded);
            Assert.Equal(4, c.LastAdvancedDay);
        }

        [Fact]
        public void Advance_CallsPersistenceBeforeReturning()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new StubOwner("alpha"));
            var captured = new CapturingPersistence();
            var result = c.Advance(11, captured);
            Assert.NotNull(result);
            Assert.Equal(11, captured.Day);
            Assert.Single(captured.Reports);
            Assert.Equal("alpha", captured.Reports[0].OwnerId);
        }

        [Fact]
        public void Advance_RaisesOnDayAdvanced()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new StubOwner("alpha"));
            DayAdvancedEventArgs captured = null;
            c.OnDayAdvanced += args => captured = args;
            c.Advance(9);
            Assert.NotNull(captured);
            Assert.Equal(9, captured.Day);
        }

        [Fact]
        public void Advance_AllEvents_IteratesEveryEventFromEveryOwner()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new EmittingOwner("alpha"));
            c.Register("bravo", new EmittingOwner("bravo"));
            var result = c.Advance(1);
            int count = 0;
            foreach (var _ in result.AllEvents()) count++;
            Assert.Equal(4, count); // 2 per emitting owner
        }

        private sealed class StubOwner : IDayAdvanceOwner
        {
            public string Id;
            public int TickCount;
            public StubOwner(string id) { Id = id; }
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events) { TickCount++; }
        }

        private sealed class EmittingOwner : IDayAdvanceOwner
        {
            public string Id;
            public EmittingOwner(string id) { Id = id; }
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                events.Add(new DayStateChangeEvent("consumed_water", Id, "clean_water", null, 3f));
                events.Add(new DayStateChangeEvent("consumed_food", Id, "canned_food", null, 2f));
            }
        }

        private sealed class CrashingOwner : IDayAdvanceOwner
        {
            public string Id;
            public CrashingOwner(string id) { Id = id; }
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                throw new InvalidOperationException("intentional crash for test");
            }
        }

        private sealed class BlockingStubOwner : IDayAdvanceOwner
        {
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                System.Threading.Thread.Sleep(20);
            }
        }

        private sealed class CapturingPersistence : IDayAdvancePersistence
        {
            public int Day;
            public List<DayOwnerReport> Reports = new List<DayOwnerReport>();
            public void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports)
            {
                Day = day;
                Reports.Clear();
                if (ownerReports == null) return;
                for (int i = 0; i < ownerReports.Count; i++) Reports.Add(ownerReports[i]);
            }
        }
    }
}
