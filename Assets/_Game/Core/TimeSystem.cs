using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Advances game time (hours/days) from real-time delta and broadcasts
    /// periodic tick events (hourly need decay, daily summary). Deterministic
    /// so a save/load round-trip reproduces the same clock.
    /// </summary>
    public class TimeSystem
    {
        private int _day = 1;
        private float _hourAccumulator;
        private float _totalElapsedSeconds;

        /// <summary>Real-time seconds per in-game hour (e.g. 10 = 1 game hour every 10 real seconds).</summary>
        public float SecondsPerGameHour = 10f;

        /// <summary>Current in-game day (1-based).</summary>
        public int CurrentDay => _day;

        /// <summary>Current hour within the day (0..23).</summary>
        public int CurrentHour => (int)_hourAccumulator;

        /// <summary>Fractional hour (0..24).</summary>
        public float CurrentHourFloat => _hourAccumulator;

        /// <summary>Total in-game hours elapsed since start.</summary>
        public float TotalElapsedHours => (_day - 1) * 24f + _hourAccumulator;

        /// <summary>Fired every in-game hour tick with (day, hour).</summary>
        public event Action<int, int> OnHourTick;

        /// <summary>Fired every in-game day tick with (day).</summary>
        public event Action<int> OnDayTick;

        /// <summary>Advance the clock by a real-time delta (seconds).</summary>
        public void Tick(float deltaTimeSeconds)
        {
            if (deltaTimeSeconds <= 0f || SecondsPerGameHour <= 0f) return;

            _totalElapsedSeconds += deltaTimeSeconds;
            float gameHours = deltaTimeSeconds / SecondsPerGameHour;
            _hourAccumulator += gameHours;

            while (_hourAccumulator >= 24f)
            {
                _hourAccumulator -= 24f;
                _day++;
                OnDayTick?.Invoke(_day);
            }

            OnHourTick?.Invoke(_day, CurrentHour);
        }

        /// <summary>Advance the clock by a number of game hours directly.</summary>
        public void TickHours(float gameHours)
        {
            if (gameHours <= 0f) return;
            _hourAccumulator += gameHours;

            while (_hourAccumulator >= 24f)
            {
                _hourAccumulator -= 24f;
                _day++;
                OnDayTick?.Invoke(_day);
            }

            OnHourTick?.Invoke(_day, CurrentHour);
        }

        /// <summary>Force the clock to an explicit day/hour (used by save/load).</summary>
        public void SetTime(int day, int hour)
        {
            _day = Math.Max(1, day);
            _hourAccumulator = Math.Clamp(hour, 0, 23);
        }

        /// <summary>Force the clock to an explicit total elapsed hours (used by save/load).</summary>
        public void SetElapsedHours(float totalHours)
        {
            if (totalHours < 0f) totalHours = 0f;
            _day = (int)(totalHours / 24f) + 1;
            _hourAccumulator = totalHours - (_day - 1) * 24f;
        }
    }
}
