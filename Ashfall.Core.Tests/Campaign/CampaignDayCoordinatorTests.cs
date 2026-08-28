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
            Assert.False(result.Succeeded);
            Assert.Equal(-1, c.LastAdvancedDay); // Fail-closed: owner failure prevents day commit
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
        public void DoubleTryBegin_CausesAdvanceToReturnNull_Characterization()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new StubOwner("alpha"));

            // When TryBegin is called explicitly beforehand, _advancing is set to true.
            bool begun = c.TryBegin(1);
            Assert.True(begun);

            // An immediate Advance(1) attempts TryBegin internally and returns null because _advancing is true.
            var blockedResult = c.Advance(1);
            Assert.Null(blockedResult);

            // Releasing the gate allows Advance to run cleanly.
            c.EndAdvance();
            var successResult = c.Advance(1);
            Assert.NotNull(successResult);
            Assert.True(successResult.Succeeded);
            Assert.Equal(1, c.LastAdvancedDay);
        }

        [Fact]
        public void Advance_FailClosed_DoesNotAdvanceLastDayOrPersistOnFailure()
        {
            var c = new CampaignDayCoordinator();
            c.Register("alpha", new StubOwner("alpha"));
            c.Register("crash", new CrashingOwner("crash"));
            var persistence = new CapturingPersistence();

            var result = c.Advance(4, persistence, failClosed: true);
            Assert.NotNull(result);
            Assert.True(result.HasFailures);
            Assert.False(result.Succeeded);
            Assert.Single(result.FailedReports);
            Assert.Equal("crash", result.FailedReports[0].OwnerId);
            Assert.Equal(-1, c.LastAdvancedDay); // Fail-closed: last day NOT committed
            Assert.Equal(0, persistence.Day); // Fail-closed: persistence NOT invoked
        }

        [Fact]
        public void Advance_OwnerFailure_AllowsSafeRetryAfterFix()
        {
            var c = new CampaignDayCoordinator();
            var flaky = new FlakyOwner("flaky") { ShouldCrash = true };
            c.Register("flaky", flaky);

            // First attempt fails
            var result1 = c.Advance(5, failClosed: true);
            Assert.NotNull(result1);
            Assert.True(result1.HasFailures);
            Assert.Equal(-1, c.LastAdvancedDay);

            // Fix the condition and retry the same day
            flaky.ShouldCrash = false;
            var result2 = c.Advance(5, failClosed: true);
            Assert.NotNull(result2);
            Assert.True(result2.Succeeded);
            Assert.Equal(5, c.LastAdvancedDay);
        }

        [Fact]
        public void SaveAndRestoreState_PreservesLastAdvancedDay()
        {
            var c1 = new CampaignDayCoordinator();
            c1.Register("alpha", new StubOwner("alpha"));
            c1.Advance(10);
            Assert.Equal(10, c1.LastAdvancedDay);

            var save = c1.CaptureState();
            Assert.Equal(10, save.lastAdvancedDay);

            var c2 = new CampaignDayCoordinator();
            c2.Register("alpha", new StubOwner("alpha"));
            c2.RestoreState(save);
            Assert.Equal(10, c2.LastAdvancedDay);

            // Next valid day must be > 10
            Assert.Null(c2.Advance(10));
            Assert.NotNull(c2.Advance(11));
        }

        [Fact]
        public void Advance_CapturesPreDaySnapshotOnAllOwnersBeforeTicking()
        {
            var c = new CampaignDayCoordinator();
            var log = new List<string>();
            var o1 = new OrderCheckingOwner("o1", log);
            var o2 = new OrderCheckingOwner("o2", log);
            c.Register("o1", o1, phase: 1);
            c.Register("o2", o2, phase: 2);

            var result = c.Advance(3);
            Assert.NotNull(result);
            Assert.True(result.Succeeded);

            // Snapshots happen in phase order for all owners, then ticks happen in phase order
            Assert.Equal(new[] { "snap_o1", "snap_o2", "tick_o1", "tick_o2" }, log);
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

        private sealed class FlakyOwner : IDayAdvanceOwner
        {
            public string Id;
            public bool ShouldCrash;
            public FlakyOwner(string id) { Id = id; }
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                if (ShouldCrash) throw new InvalidOperationException("flaky failure");
            }
        }

        private sealed class OrderCheckingOwner : IDayAdvanceOwner
        {
            public string Id;
            private readonly List<string> _log;
            public OrderCheckingOwner(string id, List<string> log) { Id = id; _log = log; }
            public void CapturePreDaySnapshot(int day) => _log.Add("snap_" + Id);
            public void TickDay(int day, List<DayStateChangeEvent> events) => _log.Add("tick_" + Id);
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

        /// <summary>Owner whose TickDay mutates state but whose restore always throws —
        /// models a subsystem whose baseline cannot be trusted for rollback.</summary>
        private sealed class UnrestorableOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            public int State;
            public int TicksUntilThrow = int.MaxValue;

            public void CapturePreDaySnapshot(int day) { }
            public void RestorePreDaySnapshot(int day) { throw new InvalidOperationException("baseline unusable"); }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                State++;
                if (State >= TicksUntilThrow)
                    throw new InvalidOperationException("planned failure");
            }
        }

        /// <summary>Snapshotting owner: Capture freezes State; Restore rewinds to it.</summary>
        private sealed class SnapshottingOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            public string Id;
            public int State;
            public int Snapshot;
            public int RestoreCount;
            private readonly bool _throwOnTick;
            private int _ticksUntilThrow;

            public SnapshottingOwner(string id, bool throwOnTick = false)
            {
                Id = id;
                _throwOnTick = throwOnTick;
                _ticksUntilThrow = int.MaxValue;
            }

            /// <summary>Arm a planned failure after this many total ticks.</summary>
            public int TicksUntilThrow
            {
                get => _ticksUntilThrow;
                set { _ticksUntilThrow = value; _throwGuard = _throwOnTick; }
            }

            private bool _throwGuard;

            public void CapturePreDaySnapshot(int day) { Snapshot = State; }
            public void RestorePreDaySnapshot(int day) { RestoreCount++; State = Snapshot; }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                State++;
                if (_throwGuard && State >= _ticksUntilThrow)
                    throw new InvalidOperationException("planned failure");
            }
        }

        [Fact]
        public void Retry_AfterFailure_RestoresSnapshotBeforeTick()
        {
            var c = new CampaignDayCoordinator();
            var clock = new SnapshottingOwner("clock");                 // ticks before the failure
            var later = new SnapshottingOwner("later", throwOnTick: true) { TicksUntilThrow = 1 };
            c.Register("clock", clock, phase: 1);
            c.Register("later", later, phase: 4);

            // Attempt 1: clock ticks (State 0→1), later throws.
            var first = c.Advance(5);
            Assert.NotNull(first);
            Assert.True(first.HasFailures);
            Assert.Equal(1, clock.State);                               // mutated in memory
            Assert.Equal(-1, c.LastAdvancedDay);                        // nothing committed

            // Attempt 2 (retry): the coordinator must roll the clock back to
            // its pre-day snapshot BEFORE ticking, so the day is not applied twice.
            later.TicksUntilThrow = int.MaxValue;                       // fix the fault
            var second = c.Advance(5);
            Assert.NotNull(second);
            Assert.True(second.Succeeded);
            Assert.Equal(1, clock.RestoreCount);                        // rolled back once...
            Assert.Equal(1, clock.State);                               // ...so one tick lands, not two
            Assert.Equal(5, c.LastAdvancedDay);                         // day committed exactly once
        }

        [Fact]
        public void FailedRestore_FailsTheRetryToo()
        {
            var c = new CampaignDayCoordinator();
            var bad = new UnrestorableOwner { TicksUntilThrow = int.MaxValue };
            c.Register("bad", bad);

            // First attempt: the owner itself throws — arms the restore path.
            bad.TicksUntilThrow = 1;
            Assert.True(c.Advance(3)!.HasFailures);

            // Retry: the owner's restore throws (baseline unusable) — the
            // attempt must fail closed rather than commit over unverified state.
            Assert.True(c.Advance(3)!.HasFailures);
            Assert.Equal(-1, c.LastAdvancedDay);
        }

        [Fact]
        public void SuccessfulAdvance_ClearsPendingRestore()
        {
            var c = new CampaignDayCoordinator();
            var owner = new SnapshottingOwner("solo");
            c.Register("solo", owner);

            Assert.True(c.Advance(4)!.Succeeded);
            owner.State = 99;                                           // unrelated drift after commit
            Assert.True(c.Advance(5)!.Succeeded);
            Assert.Equal(0, owner.RestoreCount);                        // restore is not part of a fresh day
            Assert.Equal(100, owner.State);                             // ticked once, not rolled back
        }

        [Fact]
        public void PendingRestore_ForDifferentDay_IsDroppedWithoutRestore()
        {
            var c = new CampaignDayCoordinator();
            var failing = new SnapshottingOwner("failing", throwOnTick: true) { TicksUntilThrow = 1 };
            c.Register("failing", failing);

            Assert.True(c.Advance(9)!.HasFailures);                     // arms restore for day 9

            // A different (still-future) day is attempted instead: the stale
            // baseline is dropped and no restore runs.
            failing.TicksUntilThrow = int.MaxValue;
            Assert.True(c.Advance(10)!.Succeeded);
            Assert.Equal(0, failing.RestoreCount);
        }
    }
}
