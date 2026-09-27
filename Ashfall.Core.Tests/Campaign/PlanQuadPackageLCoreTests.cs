// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// Quad Package L (2026-09-26) — focused Core tests pinning the fail-closed
    /// persistence contract of <see cref="CampaignDayCoordinator"/>.
    ///
    /// The behaviour under test was proven broken by
    /// <c>FollowUpRemediationGateTests.F16_CampaignDayCoordinator_PersistenceFailure_ArmsPendingRestore</c>:
    /// the coordinator committed <see cref="CampaignDayCoordinator.LastAdvancedDay"/>
    /// (and moved the calendar) *before* persisting, so a persistence throw left
    /// the campaign header claiming a day that had never been saved.
    ///
    /// These tests are engine-free (Core contract only) and do not duplicate the
    /// gate; they pin the specific invariants the gate asserts plus the
    /// report-preservation detail it does not.
    /// </summary>
    public sealed class PlanQuadPackageLCoreTests
    {
        private sealed class CountingOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            public int TickCount;
            public int RestoreCount;
            public int SnapshotCount;

            public void CapturePreDaySnapshot(int day) => SnapshotCount++;
            public void TickDay(int day, List<DayStateChangeEvent> events) => TickCount++;
            public void RestorePreDaySnapshot(int day) => RestoreCount++;
        }

        private sealed class ThrowingPersistence : IDayAdvancePersistence
        {
            public bool ShouldThrow = true;
            public int CallCount;

            public void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports)
            {
                CallCount++;
                if (ShouldThrow) throw new InvalidOperationException("persistence failure");
            }
        }

        [Fact]
        public void PersistenceFailure_LeavesDayUncommitted_AndDoesNotAdvanceCalendar()
        {
            var coord = new CampaignDayCoordinator();
            coord.Register("owner", new CountingOwner());
            var persistence = new ThrowingPersistence();

            var result = coord.Advance(2, persistence);

            Assert.NotNull(result);
            Assert.True(result!.HasFailures);
            // The day is not committed...
            Assert.Equal(-1, coord.LastAdvancedDay);
            // ...and the calendar header is rolled back to its pre-commit day,
            // not left claiming the day whose save never landed.
            Assert.Equal(1, coord.Calendar.CurrentDay);
        }

        [Fact]
        public void PersistenceFailure_StillTickedEveryOwnerOnce()
        {
            var coord = new CampaignDayCoordinator();
            var owner = new CountingOwner();
            coord.Register("owner", owner);
            var persistence = new ThrowingPersistence();

            coord.Advance(2, persistence);

            Assert.Equal(1, owner.TickCount);
        }

        [Fact]
        public void PersistenceFailure_RetryRollsOwnersBackBeforeReTicking()
        {
            var coord = new CampaignDayCoordinator();
            var owner = new CountingOwner();
            coord.Register("owner", owner);
            var persistence = new ThrowingPersistence();

            coord.Advance(2, persistence);

            persistence.ShouldThrow = false;
            var retry = coord.Advance(2, persistence);

            Assert.NotNull(retry);
            Assert.True(retry!.Succeeded);
            // Rolled back the failed attempt before re-ticking, then ticked again.
            Assert.Equal(1, owner.RestoreCount);
            Assert.Equal(2, owner.TickCount);
            Assert.Equal(2, coord.LastAdvancedDay);
            Assert.Equal(2, coord.Calendar.CurrentDay);
        }

        [Fact]
        public void PersistenceFailure_PreservesOwnerReportsAlongsideThePersistenceReport()
        {
            var coord = new CampaignDayCoordinator();
            coord.Register("owner", new CountingOwner());
            var persistence = new ThrowingPersistence();

            var result = coord.Advance(2, persistence);

            Assert.NotNull(result);
            var reports = result!.OwnerReports;
            Assert.Contains(reports, r => r.OwnerId == "owner");
            Assert.Contains(reports, r => r.OwnerId == "persistence");
        }

        [Fact]
        public void CalendarIsNotLeftClaimingADayThatWasNeverSaved()
        {
            var coord = new CampaignDayCoordinator();
            coord.Register("owner", new CountingOwner());
            var persistence = new ThrowingPersistence();
            int dayBefore = coord.Calendar.CurrentDay;

            coord.Advance(2, persistence);

            // The day header must stay where it was, because nothing was persisted.
            Assert.Equal(dayBefore, coord.Calendar.CurrentDay);
            Assert.Equal(-1, coord.LastAdvancedDay);
        }
    }
}
