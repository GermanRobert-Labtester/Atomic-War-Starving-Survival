namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Advances game time (hours/days) from real-time delta and broadcasts
    /// periodic tick events (hourly need decay, daily summary). Deterministic
    /// so a save/load round-trip reproduces the same clock.
    /// </summary>
    public class TimeSystem
    {
        /// <summary>Current in-game day (1-based).</summary>
        public int CurrentDay => throw new System.NotImplementedException();

        /// <summary>Current hour within the day (0-23).</summary>
        public int CurrentHour => throw new System.NotImplementedException();

        /// <summary>Advance the clock by a real-time delta (seconds).</summary>
        public void Tick(float deltaTimeSeconds) => throw new System.NotImplementedException();

        /// <summary>Force the clock to an explicit day/hour (used by save/load).</summary>
        public void SetTime(int day, int hour) => throw new System.NotImplementedException();
    }
}
