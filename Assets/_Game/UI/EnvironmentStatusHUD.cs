using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Renders weather, season, time-of-day status strip alongside shelter indoor air quality
    /// and filter health gauges. Refreshed via system events. Supports raw values debug mode.
    /// </summary>
    public class EnvironmentStatusHUD : MonoBehaviour
    {
        [SerializeField] private bool _showRawValues = false;

        public int Day { get; private set; } = 1;
        public float Hour { get; private set; } = 12f;
        public string WeatherName { get; private set; } = "Clear";
        public string SeasonName { get; private set; } = "Nuclear Winter";
        public float AirQuality { get; private set; } = 100f;
        public float FilterHealth { get; private set; } = 100f;
        /// <summary>Simulation speed (1 normal, 3 fast-forward). Display-only.</summary>
        public float TimeScale { get; private set; } = 1f;

        public bool ShowRawValues => _showRawValues;

        public void SetEnvironment(int day, float hour, string weather, string season = "Nuclear Winter")
        {
            Day = day;
            Hour = Mathf.Clamp(hour, 0f, 24f);
            WeatherName = !string.IsNullOrEmpty(weather) ? weather : "Clear";
            SeasonName = !string.IsNullOrEmpty(season) ? season : "Nuclear Winter";
        }

        /// <summary>Fast-forward indicator state (pushed from TimeSystem via HUD.Tick).</summary>
        public void SetTimeScale(float timeScale)
        {
            TimeScale = Mathf.Max(0f, timeScale);
        }

        public void SetShelterStats(float airQuality, float filterHealth)
        {
            AirQuality = Mathf.Clamp(airQuality, 0f, 100f);
            FilterHealth = Mathf.Clamp(filterHealth, 0f, 100f);
        }

        public void SetShowRawValues(bool show)
        {
            _showRawValues = show;
        }

        public string GetFormattedTimeStrip()
        {
            int h = Mathf.FloorToInt(Hour);
            int m = Mathf.FloorToInt((Hour - h) * 60f);
            string strip = $"Day {Day} - {h:D2}:{m:D2} | {WeatherName} | {SeasonName}";
            if (TimeScale > 1.001f)
                strip += $" | >> {TimeScale:0.#}x";
            return strip;
        }

        public string GetFormattedShelterStrip()
        {
            if (_showRawValues)
            {
                return $"Air Quality: {AirQuality:F1}% | Filter Health: {FilterHealth:F1}%";
            }
            return $"Air: {(AirQuality > 60f ? "Good" : AirQuality > 30f ? "Fair" : "POOR")} | Filter: {FilterHealth:F0}%";
        }
    }
}
