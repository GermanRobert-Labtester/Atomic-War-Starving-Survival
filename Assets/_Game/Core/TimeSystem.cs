using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Advances game time (hours/days) from real-time delta and broadcasts
    /// periodic tick events (hourly need decay, daily summary). Deterministic
    /// so a save/load round-trip reproduces the same clock.
    ///
    /// Fast-forward safe: <see cref="TimeScale"/> multiplies the simulated
    /// delta (1x normal, 3x fast-forward) and <see cref="TickHours"/> splits
    /// any large span into sub-steps of at most <see cref="MaxGameHoursPerStep"/>,
    /// so hour/day ticks and the systems driven from them never get skipped —
    /// no matter how big the frame delta grows (hitches, backgrounding, 3x).
    /// </summary>
    public class TimeSystem
    {
        /// <summary>Upper clamp for <see cref="TimeScale"/> (tuning guard, not gameplay).</summary>
        public const float MaxTimeScale = 8f;

        private int _day = 1;
        private float _hourAccumulator;
        private float _totalElapsedSeconds;
        private float _timeScale = 1f;

        /// <summary>Real-time seconds per in-game hour (e.g. 10 = 1 game hour every 10 real seconds).</summary>
        public float SecondsPerGameHour = 10f;

        /// <summary>
        /// Largest chunk of game time one tick may advance. Large deltas
        /// (fast-forward, frame hitches) are split into sub-steps of this
        /// size so per-hour consumers always run.
        /// </summary>
        public float MaxGameHoursPerStep = 1f;

        /// <summary>
        /// Simulation speed multiplier: 1 = normal, 3 = fast-forward.
        /// Scales only the simulated delta — Unity's Time.timeScale, audio,
        /// and animation are untouched so the UI stays responsive.
        /// Session setting: not serialized; resets to 1 on load.
        /// </summary>
        public float TimeScale => _timeScale;

        /// <summary>Current in-game day (1-based).</summary>
        public int CurrentDay => _day;

        /// <summary>Current hour within the day (0..23).</summary>
        public int CurrentHour => (int)_hourAccumulator;

        /// <summary>Fractional hour (0..24).</summary>
        public float CurrentHourFloat => _hourAccumulator;

        /// <summary>Total in-game hours elapsed since start.</summary>
        public float TotalElapsedHours => (_day - 1) * 24f + _hourAccumulator;

        /// <summary>Fired every in-game hour tick with (day, hour). Fires once per sub-step, so large deltas produce one fire per step, never zero.</summary>
        public event Action<int, int> OnHourTick;

        /// <summary>Fired every in-game day tick with (day). Large deltas fire this once per day crossed — never skipped.</summary>
        public event Action<int> OnDayTick;

        /// <summary>Fired when the simulation speed changes (UI fast-forward indicator).</summary>
        public event Action<float> OnTimeScaleChanged;

        /// <summary>Set the simulation speed (clamped to 0..<see cref="MaxTimeScale"/>). Raises <see cref="OnTimeScaleChanged"/> on change.</summary>
        public void SetTimeScale(float scale)
        {
            if (float.IsNaN(scale)) scale = 1f;
            scale = Math.Max(0f, Math.Min(scale, MaxTimeScale));
            if (Math.Abs(scale - _timeScale) < 1e-6f) return;
            _timeScale = scale;
            OnTimeScaleChanged?.Invoke(_timeScale);
        }

        /// <summary>Advance the clock by a real-time delta (seconds), scaled by <see cref="TimeScale"/>.</summary>
        public void Tick(float deltaTimeSeconds)
        {
            if (deltaTimeSeconds <= 0f || SecondsPerGameHour <= 0f || _timeScale <= 0f) return;

            _totalElapsedSeconds += deltaTimeSeconds;
            TickHours(deltaTimeSeconds * _timeScale / SecondsPerGameHour);
        }

        /// <summary>
        /// Advance the clock by a number of game hours directly. Large spans
        /// are split into sub-steps of at most <see cref="MaxGameHoursPerStep"/>
        /// hours; each sub-step fires <see cref="OnHourTick"/> once and
        /// <see cref="OnDayTick"/> once per day boundary it crosses.
        /// </summary>
        public void TickHours(float gameHours)
        {
            if (gameHours <= 0f) return;

            float stepBudget = MaxGameHoursPerStep > 0f ? MaxGameHoursPerStep : 1f;
            float remaining = gameHours;
            while (remaining > 0f)
            {
                float step = remaining < stepBudget ? remaining : stepBudget;
                remaining -= step;
                Advance(step);
            }
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

        private void Advance(float stepHours)
        {
            _hourAccumulator += stepHours;

            while (_hourAccumulator >= 24f)
            {
                _hourAccumulator -= 24f;
                _day++;
                OnDayTick?.Invoke(_day);
            }

            OnHourTick?.Invoke(_day, CurrentHour);
        }
    }
}
