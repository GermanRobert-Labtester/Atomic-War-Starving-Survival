using System;
using Ashfall.Core;

namespace Ashfall.Core.Clock
{
    public interface ISimClock
    {
        long CurrentTick { get; }
        int DayIndex { get; }
        int HourOfDay { get; }
        void AdvanceTicks(long ticks);
        void AdvanceHours(int hours);
        void AdvanceDays(int days);
    }

    public sealed class SimClock : ISimClock, IClock
    {
        public const long TicksPerHour = 60;
        public const long TicksPerDay = TicksPerHour * 24;

        public long CurrentTick { get; private set; }

        public int DayIndex => (int)(CurrentTick / TicksPerDay);
        public int HourOfDay => (int)((CurrentTick % TicksPerDay) / TicksPerHour);

        // IClock implementation
        public int Day => DayIndex;

        public SimClock(long initialTick = 0)
        {
            CurrentTick = Math.Max(0, initialTick);
        }

        public void AdvanceTicks(long ticks)
        {
            CurrentTick += Math.Max(0, ticks);
        }

        public void AdvanceHours(int hours)
        {
            AdvanceTicks(hours * TicksPerHour);
        }

        public void AdvanceDays(int days)
        {
            AdvanceTicks(days * TicksPerDay);
        }

        public void SetTick(long tick)
        {
            CurrentTick = Math.Max(0, tick);
        }

        public void SetDay(int day)
        {
            if (day < 0)
                throw new ArgumentOutOfRangeException(nameof(day));
            CurrentTick = (long)day * TicksPerDay;
        }
    }
}
