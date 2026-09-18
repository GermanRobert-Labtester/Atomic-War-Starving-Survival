// SPDX-License-Identifier: MIT
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

        /// <summary>The current day's resolved calendar read model.</summary>
        CampaignCalendarReadModel CurrentReadModel { get; }

        /// <summary>Purely resolves time, seasonal context, and ambient baseline for any day without mutating state.</summary>
        CampaignCalendarReadModel ResolveDay(int day);

        /// <summary>Binds authored season profile definition (e.g. from weather_seasons.json).</summary>
        void BindProfile(Ashfall.Core.World.SeasonProfileDef? profile);

        /// <summary>Raised when advancing days crosses into a new season window (oldSeasonId, newSeasonId).</summary>
        event Action<string, string>? OnSeasonChanged;

        /// <summary>Exposes this calendar as an <see cref="IClock"/> projection.</summary>
        IClock AsClock();

        /// <summary>Exposes this calendar as an <see cref="ISimClock"/> projection.</summary>
        ISimClock AsSimClock();
    }

    /// <summary>Default concrete implementation of <see cref="ICampaignCalendar"/>.</summary>
    public sealed class CampaignCalendar : ICampaignCalendar
    {
        public const int DaysPerYear = 365;

        private int _currentDay;
        private Ashfall.Core.World.SeasonProfileDef? _profile;

        public int CurrentDay => _currentDay;

        public CampaignCalendarReadModel CurrentReadModel => ResolveDay(_currentDay);

        public event Action<int>? OnDayChanged;
        public event Action<string, string>? OnSeasonChanged;

        public CampaignCalendar(int initialDay = 1, Ashfall.Core.World.SeasonProfileDef? profile = null)
        {
            _currentDay = Math.Max(1, initialDay);
            _profile = profile;
        }

        public void BindProfile(Ashfall.Core.World.SeasonProfileDef? profile)
        {
            _profile = profile;
        }

        public void SetDay(int day)
        {
            if (day < 1)
                throw new ArgumentOutOfRangeException(nameof(day), "Campaign day must be >= 1");
            if (day == _currentDay) return;

            string oldSeasonId = ResolveDay(_currentDay).SeasonId;
            _currentDay = day;
            string newSeasonId = ResolveDay(_currentDay).SeasonId;

            OnDayChanged?.Invoke(_currentDay);
            if (!string.Equals(oldSeasonId, newSeasonId, StringComparison.Ordinal))
            {
                OnSeasonChanged?.Invoke(oldSeasonId, newSeasonId);
            }
        }

        public CampaignCalendarReadModel ResolveDay(int day)
        {
            if (day < 1) day = 1;

            int dayInYear = ((day - 1) % DaysPerYear) + 1;
            int year = ((day - 1) / DaysPerYear) + 1;
            int chapter = year;

            var seasons = _profile?.seasons;
            if (seasons == null || seasons.Count == 0)
            {
                seasons = DefaultProfileSeasons;
            }

            // Find matching window by largest startDay <= dayInYear (seasons[0] startDay 0 covers day 1)
            int matchIndex = 0;
            for (int i = 0; i < seasons.Count; i++)
            {
                if (seasons[i] != null && seasons[i].startDay <= dayInYear)
                {
                    matchIndex = i;
                }
            }

            var currentWindow = seasons[matchIndex] ?? DefaultProfileSeasons[0];
            int nextStartDay = (matchIndex + 1 < seasons.Count && seasons[matchIndex + 1] != null)
                ? seasons[matchIndex + 1].startDay
                : DaysPerYear + 1;

            int startDay = Math.Max(1, currentWindow.startDay);
            int daysIntoSeason = dayInYear - startDay + 1;
            int duration = Math.Max(1, nextStartDay - startDay);
            int daysToSeasonEnd = Math.Max(0, nextStartDay - 1 - dayInYear);
            float progress = Math.Clamp((float)daysIntoSeason / duration, 0f, 1f);

            float ambientTemp = CalculateBaselineAmbientTemperature(dayInYear);
            float severity = Math.Clamp((10.0f - ambientTemp) / 55.0f, 0.10f, 1.0f);
            float dayLength = Math.Clamp(10.0f + (ambientTemp / 10.0f) * 2.0f, 6.0f, 16.0f);
            float preservationBias = Math.Clamp(1.0f - (ambientTemp * 0.015f), 0.70f, 1.60f);
            float migrationBias = Math.Clamp(1.0f + (ambientTemp * 0.02f), 0.30f, 1.40f);

            return new CampaignCalendarReadModel(
                day: day,
                seasonId: currentWindow.id,
                seasonDisplayName: currentWindow.displayName,
                seasonIndex: matchIndex,
                seasonProgress: progress,
                daysIntoSeason: daysIntoSeason,
                daysToSeasonEnd: daysToSeasonEnd,
                year: year,
                chapter: chapter,
                ambientTemperatureC: ambientTemp,
                seasonalSeverity: severity,
                dayLengthHours: dayLength,
                migrationBias: migrationBias,
                preservationBias: preservationBias
            );
        }

        private static float CalculateBaselineAmbientTemperature(int dayInYear)
        {
            if (dayInYear <= 29)
            {
                // First Thaw: -2C rising to +3C
                float t = (dayInYear - 1) / 29.0f;
                return -2.0f + (5.0f * t);
            }
            if (dayInYear <= 59)
            {
                // Ash Settling: +3C dropping to -10C
                float t = (dayInYear - 30) / 30.0f;
                return 3.0f - (13.0f * t);
            }
            if (dayInYear <= 89)
            {
                // The Deep Freeze: -10C dropping to -25C
                float t = (dayInYear - 60) / 30.0f;
                return -10.0f - (15.0f * t);
            }
            if (dayInYear <= 119)
            {
                // Spring Storms: -25C rising to +2C
                float t = (dayInYear - 90) / 30.0f;
                return -25.0f + (27.0f * t);
            }
            if (dayInYear <= 149)
            {
                // Dry Ash: +2C to +8C back to +3C
                float t = (dayInYear - 120) / 30.0f;
                return 2.0f + (6.0f * (float)Math.Sin(t * Math.PI));
            }
            if (dayInYear <= 179)
            {
                // First Fallout: +3C dropping to -25C
                float t = (dayInYear - 150) / 30.0f;
                return 3.0f - (28.0f * t);
            }
            if (dayInYear <= 240)
            {
                // Year of Ash Phase 4 (Deep Freeze): -25C to -45C at day 210, then -30C
                float t = (dayInYear - 180) / 60.0f;
                return -25.0f - (20.0f * (float)Math.Sin(t * Math.PI));
            }
            if (dayInYear <= 300)
            {
                // Year of Ash Phase 5 (Faction Siege): -30C to -10C
                float t = (dayInYear - 240) / 60.0f;
                return -30.0f + (20.0f * t);
            }
            if (dayInYear <= 360)
            {
                // Year of Ash Phase 6 (Great Thaw): -10C to +4C
                float t = (dayInYear - 300) / 60.0f;
                return -10.0f + (14.0f * t);
            }

            // Days 361-365: glide from +4C to -2C into next year's First Thaw
            float endT = (dayInYear - 360) / 5.0f;
            return 4.0f - (6.0f * endT);
        }

        private static readonly List<Ashfall.Core.World.SeasonWindowDef> DefaultProfileSeasons = new List<Ashfall.Core.World.SeasonWindowDef>
        {
            new() { id = "window_first_thaw", displayName = "First Thaw", startDay = 0 },
            new() { id = "window_ash_settling", displayName = "Ash Settling", startDay = 30 },
            new() { id = "window_deep_freeze", displayName = "The Deep Freeze", startDay = 60 },
            new() { id = "window_spring_storms", displayName = "Spring Storms", startDay = 90 },
            new() { id = "window_dry_ash", displayName = "Dry Ash", startDay = 120 },
            new() { id = "window_first_fallout", displayName = "First Fallout", startDay = 150 },
            new() { id = "window_false_spring", displayName = "False Spring", startDay = 180 },
            new() { id = "window_deep_ash", displayName = "Deep Ash", startDay = 200 },
            new() { id = "window_long_winter", displayName = "The Long Winter", startDay = 240 },
            new() { id = "window_black_rain_season", displayName = "Black Rain Season", startDay = 280 }
        };

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
