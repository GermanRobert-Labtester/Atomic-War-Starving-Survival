using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Ashfall.Core.Clock;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignCalendarTests
    {
        [Fact]
        public void Calendar_InitializesAndAdvancesMonotonically()
        {
            var cal = new CampaignCalendar(initialDay: 1);
            Assert.Equal(1, cal.CurrentDay);

            int eventCount = 0;
            int lastEventDay = 0;
            cal.OnDayChanged += day =>
            {
                eventCount++;
                lastEventDay = day;
            };

            cal.SetDay(2);
            Assert.Equal(2, cal.CurrentDay);
            Assert.Equal(1, eventCount);
            Assert.Equal(2, lastEventDay);

            // Setting identical day does not re-raise event
            cal.SetDay(2);
            Assert.Equal(1, eventCount);

            // Invalid day throws
            Assert.Throws<ArgumentOutOfRangeException>(() => cal.SetDay(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => cal.SetDay(-5));
        }

        [Fact]
        public void CalendarClockAdapter_SynchronizesWithAuthoritativeCalendar()
        {
            var cal = new CampaignCalendar(3);
            var clock = cal.AsClock();

            Assert.Equal(3, clock.Day);

            clock.AdvanceDays(2);
            Assert.Equal(5, cal.CurrentDay);
            Assert.Equal(5, clock.Day);

            clock.SetDay(10);
            Assert.Equal(10, cal.CurrentDay);
            Assert.Equal(10, clock.Day);
        }

        [Fact]
        public void CalendarSimClockAdapter_TracksIntradayTicksAndAdvancesDays()
        {
            var cal = new CampaignCalendar(1);
            var simClock = cal.AsSimClock();

            Assert.Equal(1, simClock.DayIndex);
            Assert.Equal(0, simClock.HourOfDay);
            Assert.Equal(CalendarSimClockAdapter.TicksPerDay, simClock.CurrentTick);

            // Advance 6 hours
            simClock.AdvanceHours(6);
            Assert.Equal(1, cal.CurrentDay);
            Assert.Equal(6, simClock.HourOfDay);
            Assert.Equal(CalendarSimClockAdapter.TicksPerDay + (6 * CalendarSimClockAdapter.TicksPerHour), simClock.CurrentTick);

            // Advance another 20 hours (crossing midnight into Day 2)
            simClock.AdvanceHours(20);
            Assert.Equal(2, cal.CurrentDay);
            Assert.Equal(2, simClock.HourOfDay);
        }

        [Fact]
        public void Reconciler_IdentifiesAuthoritativeDay_AndSurfacesMismatches()
        {
            // Consistent state
            var consistent = new Dictionary<string, int>
            {
                ["campaign_day"] = 12,
                ["holdfast"] = 12,
                ["duty_roster"] = 12,
                ["year_of_ash"] = 12
            };
            var result1 = CampaignCalendarReconciler.Reconcile(consistent);
            Assert.Equal(12, result1.AuthoritativeDay);
            Assert.Equal("campaign_day", result1.PrimarySource);
            Assert.False(result1.HasMismatches);

            // Mismatched state (campaign_day is authoritative, holdfast & memorial drifted)
            var drifted = new Dictionary<string, int>
            {
                ["campaign_day"] = 15,
                ["holdfast"] = 14,
                ["memorial"] = 12,
                ["duty_roster"] = 15
            };
            var result2 = CampaignCalendarReconciler.Reconcile(drifted);
            Assert.Equal(15, result2.AuthoritativeDay);
            Assert.True(result2.HasMismatches);
            Assert.Equal(2, result2.Mismatches.Count);
            Assert.Contains(result2.Mismatches, m => m.SectionName == "holdfast" && m.SectionDay == 14);
            Assert.Contains(result2.Mismatches, m => m.SectionName == "memorial" && m.SectionDay == 12);
            Assert.Contains("[CALENDAR_MISMATCH] section='holdfast' section_day=14 authoritative_day=15",
                result2.Mismatches[0].FormatLogMessage());

            // Legacy fallback without campaign_day section
            var legacy = new Dictionary<string, int>
            {
                ["holdfast"] = 8,
                ["year_of_ash"] = 10,
                ["memorial"] = 7
            };
            var result3 = CampaignCalendarReconciler.Reconcile(legacy);
            Assert.Equal(8, result3.AuthoritativeDay); // holdfast preferred over legacy
            Assert.Equal("holdfast", result3.PrimarySource);
        }

        [Fact]
        public void CoordinatorAndCalendar_StaySynchronizedAcrossAdvanceAndRestore()
        {
            var calendar = new CampaignCalendar(1);
            var coord = new CampaignDayCoordinator(calendar);

            Assert.Equal(1, coord.Calendar.CurrentDay);
            Assert.Equal(-1, coord.LastAdvancedDay);

            coord.Register("stub", new StubOwner());
            var res = coord.Advance(2);
            Assert.NotNull(res);
            Assert.True(res.Succeeded);
            Assert.Equal(2, coord.Calendar.CurrentDay);
            Assert.Equal(2, coord.LastAdvancedDay);

            // Capture and restore
            var save = coord.CaptureState();
            Assert.Equal(2, save.lastAdvancedDay);

            var coord2 = new CampaignDayCoordinator();
            coord2.RestoreState(save);
            Assert.Equal(2, coord2.Calendar.CurrentDay);
            Assert.Equal(2, coord2.LastAdvancedDay);
        }

        [Fact]
        public void AllCampaignProjections_AgreeAfterAdvance()
        {
            var cal = new CampaignCalendar(1);
            var coord = new CampaignDayCoordinator(cal);
            var clock = cal.AsClock();
            var simClock = cal.AsSimClock();

            coord.Register("stub", new StubOwner());
            coord.Advance(42);

            Assert.Equal(42, cal.CurrentDay);
            Assert.Equal(42, coord.Calendar.CurrentDay);
            Assert.Equal(42, clock.Day);
            Assert.Equal(42, simClock.DayIndex);
        }

        private sealed class StubOwner : IDayAdvanceOwner
        {
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events) { }
        }
    }
}
