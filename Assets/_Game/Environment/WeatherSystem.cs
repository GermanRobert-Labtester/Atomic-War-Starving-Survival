namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Kinds of weather, including fallout storms that spike radiation exposure.
    /// </summary>
    public enum WeatherKind
    {
        Clear,
        Overcast,
        Rain,
        FalloutStorm,
        Blizzard
    }

    /// <summary>
    /// Drives weather state transitions over time and publishes changes on the
    /// EventBus. Fallout storms raise radiation and reduce temperature and
    /// visibility; blizzards drive nuclear-winter cold.
    /// </summary>
    public class WeatherSystem
    {
        public WeatherKind Current { get; private set; } = WeatherKind.Clear;

        /// <summary>Advance weather state over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();

        /// <summary>Force a specific weather state (debug / scripted events).</summary>
        public void ForceWeather(WeatherKind kind) => throw new System.NotImplementedException();
    }
}
