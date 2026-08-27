using System;
using System.Collections.Generic;
using Ashfall.Core.Clock;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// ASHFALL — Authoritative Campaign Calendar.
    ///
    /// Single authority for the in-game campaign day (>= 1).
    /// Distinguishes whole campaign days from sub-day simulation ticks (ISimClock)
    /// and wall-clock time. Every other system clock (Holdfast, Duty Roster,
    /// Verdict, Year of Ash, Economy) is an adapted projection of this authority.
    /// </summary>
    public interface ICampaignCalendar
    {
        /// <summary>The authoritative campaign day (>= 1).</summary>
        int CurrentDay { get; }

        /// <summary>Raised whenever the authoritative day changes.</summary>
        event Action<int>? OnDayChanged;

        /// <summary>
        /// Update the authoritative campaign day.
        /// Only the CampaignDayCoordinator and save restoration pipeline should call this.
        /// </summary>
        void SetDay(int day);

        /// <summary>Exposes this calendar as an <see cref="IClock"/> projection.</summary>
        IClock AsClock();

        /// <summary>Exposes this calendar as an <see cref="ISimClock"/> projection.</summary>
        ISimClock AsSimClock();
    }

    /// <summary>Default concrete implementation of <see cref="ICampaignCalendar"/>.</summary>
    public sealed class CampaignCalendar : ICampaignCalendar
    {
        private int _currentDay;

        public int CurrentDay => _currentDay;

        public event Action<int>? OnDayChanged;

        public CampaignCalendar(int initialDay = 1)
        {
            _currentDay = Math.Max(1, initialDay);
        }

        public void SetDay(int day)
        {
            if (day < 1)
                throw new ArgumentOutOfRangeException(nameof(day), "Campaign day must be >= 1");
            if (day == _currentDay) return;
            _currentDay = day;
            OnDayChanged?.Invoke(_currentDay);
        }

        public IClock AsClock() => new CalendarClockAdapter(this);

        public ISimClock AsSimClock() => new CalendarSimClockAdapter(this);
    }

    /// <summary>Projects <see cref="ICampaignCalendar"/> to the historical <see cref="IClock"/> port.</summary>
    public sealed class CalendarClockAdapter : IClock
    {
        private readonly ICampaignCalendar _calendar;

        public CalendarClockAdapter(ICampaignCalendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public int Day => _calendar.CurrentDay;

        public void AdvanceDays(int days)
        {
            if (days <= 0) return;
            _calendar.SetDay(_calendar.CurrentDay + days);
        }

        public void SetDay(int day)
        {
            _calendar.SetDay(day);
        }
    }

    /// <summary>Projects <see cref="ICampaignCalendar"/> to the <see cref="ISimClock"/> intraday clock.</summary>
    public sealed class CalendarSimClockAdapter : ISimClock, IClock
    {
        public const long TicksPerHour = 60;
        public const long TicksPerDay = TicksPerHour * 24;

        private readonly ICampaignCalendar _calendar;
        private long _intradayTicks;

        public CalendarSimClockAdapter(ICampaignCalendar calendar)
        {
            _calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }

        public long CurrentTick => ((long)_calendar.CurrentDay * TicksPerDay) + _intradayTicks;

        public int DayIndex => _calendar.CurrentDay;
        public int Day => _calendar.CurrentDay;
        public int HourOfDay => (int)((_intradayTicks % TicksPerDay) / TicksPerHour);

        public void AdvanceTicks(long ticks)
        {
            if (ticks <= 0) return;
            _intradayTicks += ticks;
            if (_intradayTicks >= TicksPerDay)
            {
                int daysToAdd = (int)(_intradayTicks / TicksPerDay);
                _intradayTicks %= TicksPerDay;
                _calendar.SetDay(_calendar.CurrentDay + daysToAdd);
            }
        }

        public void AdvanceHours(int hours)
        {
            if (hours <= 0) return;
            AdvanceTicks(hours * TicksPerHour);
        }

        public void AdvanceDays(int days)
        {
            if (days <= 0) return;
            _calendar.SetDay(_calendar.CurrentDay + days);
        }

        public void SetDay(int day)
        {
            _calendar.SetDay(day);
            _intradayTicks = 0;
        }
    }

    /// <summary>
    /// Reconciles conflicting day values stored across legacy or independent save sections.
    /// Identifies the authoritative campaign day and surfaces structured diagnostics for mismatches.
    /// </summary>
    public static class CampaignCalendarReconciler
    {
        public sealed class MismatchRecord
        {
            public string SectionName { get; }
            public int SectionDay { get; }
            public int AuthoritativeDay { get; }

            public MismatchRecord(string sectionName, int sectionDay, int authoritativeDay)
            {
                SectionName = sectionName;
                SectionDay = sectionDay;
                AuthoritativeDay = authoritativeDay;
            }

            public string FormatLogMessage() =>
                $"[CALENDAR_MISMATCH] section='{SectionName}' section_day={SectionDay} authoritative_day={AuthoritativeDay}";
        }

        public sealed class ReconciliationResult
        {
            public int AuthoritativeDay { get; }
            public string PrimarySource { get; }
            public IReadOnlyList<MismatchRecord> Mismatches { get; }
            public bool HasMismatches => Mismatches.Count > 0;

            public ReconciliationResult(int authoritativeDay, string primarySource, IReadOnlyList<MismatchRecord> mismatches)
            {
                AuthoritativeDay = Math.Max(1, authoritativeDay);
                PrimarySource = primarySource;
                Mismatches = mismatches ?? Array.Empty<MismatchRecord>();
            }
        }

        /// <summary>
        /// Reconciles day values collected from save sections.
        /// Priority:
        /// 1. campaign_day section if > 0.
        /// 2. Max of (holdfast, year_of_ash, duty_roster, memorial, 1).
        /// </summary>
        public static ReconciliationResult Reconcile(IReadOnlyDictionary<string, int>? sectionDays, ILog? log = null)
        {
            if (sectionDays == null || sectionDays.Count == 0)
            {
                return new ReconciliationResult(1, "default", Array.Empty<MismatchRecord>());
            }

            int authDay = 1;
            string source = "fallback";

            if (sectionDays.TryGetValue("campaign_day", out int campDay) && campDay > 0)
            {
                authDay = campDay;
                source = "campaign_day";
            }
            else if (sectionDays.TryGetValue("holdfast", out int holdfastDay) && holdfastDay > 0)
            {
                authDay = holdfastDay;
                source = "holdfast";
            }
            else
            {
                int maxDay = 1;
                foreach (var kv in sectionDays)
                {
                    if (kv.Value > maxDay)
                    {
                        maxDay = kv.Value;
                        source = kv.Key;
                    }
                }
                authDay = maxDay;
            }

            var mismatches = new List<MismatchRecord>();
            foreach (var kv in sectionDays)
            {
                if (kv.Value > 0 && kv.Value != authDay)
                {
                    var rec = new MismatchRecord(kv.Key, kv.Value, authDay);
                    mismatches.Add(rec);
                    log?.Warn(rec.FormatLogMessage());
                }
            }

            return new ReconciliationResult(authDay, source, mismatches);
        }
    }
}
