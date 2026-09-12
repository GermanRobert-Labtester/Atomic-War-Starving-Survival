// SPDX-License-Identifier: MIT
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Plan176Lifecycle
{
    /// <summary>
    /// DEBT-176-CAMPAIGN-AGE-CLOCK — one derived campaign age clock, days only.
    /// The roster/lifecycle aggregate answers tenure from <c>joinedDay</c>; no
    /// aging ledger, month, or year projection is introduced.
    /// </summary>
    public sealed class Plan176CampaignAgeClockTests
    {
        [Fact]
        public void Clock_returns_whole_days_since_join()
        {
            Assert.Equal(0, SurvivorLifecycle.CampaignAgeDays(joinedDay: 5, currentDay: 5));
            Assert.Equal(25, SurvivorLifecycle.CampaignAgeDays(joinedDay: 5, currentDay: 30));
            Assert.Equal(1, SurvivorLifecycle.CampaignAgeDays(joinedDay: 1, currentDay: 2));
        }

        [Fact]
        public void Clock_clamps_negative_and_future_join_days()
        {
            // A survivor who joins "later" than the current day must read 0, not negative.
            Assert.Equal(0, SurvivorLifecycle.CampaignAgeDays(joinedDay: 40, currentDay: 10));
        }

        [Fact]
        public void Roster_entry_and_aggregate_delegate_to_the_single_clock()
        {
            // Both surfaces must agree with SurvivorLifecycle — one formula, not two.
            var entry = new SurvivorRosterEntry { survivorId = "sv_alpha", joinedDay = 7 };
            Assert.Equal(SurvivorLifecycle.CampaignAgeDays(7, 21), entry.CampaignAgeDays(21));
            Assert.Equal(14, entry.CampaignAgeDays(21));

            var aggregate = SurvivorAggregate.Joined(new SurvivorId("sv_alpha"), "def_alpha", day: 7);
            Assert.Equal(14, aggregate.CampaignAgeDays(21));
        }

        [Fact]
        public void Clock_is_derived_not_persisted()
        {
            // The entry carries only joinedDay; tenure is computed on demand, so a
            // save/restore cannot drift and no new save section is required.
            var entry = new SurvivorRosterEntry { survivorId = "sv_bravo", joinedDay = 3 };
            int day10 = entry.CampaignAgeDays(10);
            Assert.Equal(day10, entry.CampaignAgeDays(10));

            Assert.Null(typeof(SurvivorRosterEntry).GetProperty("ageDays"));
            Assert.Null(typeof(SurvivorRosterEntry).GetField("ageDays"));
        }
    }
}
