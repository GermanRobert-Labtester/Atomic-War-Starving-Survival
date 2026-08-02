using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Designer-tunable campaign-wide environment data: the nuclear-winter ambient
    /// temperature curve TemperatureSystem samples, and the per-season weather
    /// transition weights WeatherSystem rolls against. One asset drives the whole
    /// campaign.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSeasonProfile", menuName = "ASHFALL/Environment/Season Profile")]
    public class SeasonProfile : ScriptableObject
    {
        [Header("Campaign Length")]
        [Tooltip("Total campaign length in in-game days; the temperature curve spans 0..1 over this range.")]
        public int campaignLengthDays = 90;

        [Header("Nuclear Winter Curve")]
        [Tooltip("X: 0..1 fraction of the campaign elapsed. Y: ambient Celsius. Author non-increasing for a believable nuclear winter.")]
        public AnimationCurve ambientTemperatureCurve = AnimationCurve.Linear(0f, 5f, 1f, -35f);

        [Header("Daylight Curve")]
        [Tooltip("X: 0..1 campaign fraction. Y: base hours of daylight per day (0..16). " +
                 "Deep nuclear winter should approach 2-4 h; ash storms push it to near-zero " +
                 "via PhotoperiodSystem.skyClarity, not this curve directly.")]
        public AnimationCurve daylightCurve = AnimationCurve.Linear(0f, 12f, 1f, 2f);

        [Header("Weather")]
        [Tooltip("WeatherSystem rolls for a transition this often.")]
        public float weatherCheckIntervalHours = 6f;

        [Tooltip("Ordered by startDay ascending; the last entry with startDay <= the current day is active.")]
        public SeasonWindow[] seasons = Array.Empty<SeasonWindow>();

        /// <summary>Sample the nuclear-winter curve at the given elapsed campaign hours (clamped past the campaign end).</summary>
        public float EvaluateAmbientCelsius(float elapsedHours)
        {
            float days = elapsedHours / 24f;
            float dayFraction = campaignLengthDays > 0 ? Mathf.Clamp01(days / campaignLengthDays) : 0f;
            return ambientTemperatureCurve.Evaluate(dayFraction);
        }

        /// <summary>
        /// Sample the daylight-hours curve at the given elapsed campaign hours.
        /// Returns base hours of useful daylight in [0..16]; multiply by
        /// <see cref="AtomicWar._Game.Environment.PhotoperiodSystem.SkyClarity"/> for effective daylight.
        /// </summary>
        public float EvaluateDaylightHours(float elapsedHours)
        {
            float days = elapsedHours / 24f;
            float dayFraction = campaignLengthDays > 0 ? Mathf.Clamp01(days / campaignLengthDays) : 0f;
            return Mathf.Clamp(daylightCurve.Evaluate(dayFraction), 0f, 16f);
        }


        /// <summary>The active season window for a given campaign day (last entry with startDay &lt;= day; a designer-neutral default if none configured).</summary>
        public SeasonWindow GetSeasonForDay(int day)
        {
            if (seasons == null || seasons.Length == 0)
            {
                return SeasonWindow.Default;
            }

            SeasonWindow current = null;
            for (int i = 0; i < seasons.Length; i++)
            {
                if (seasons[i] != null && seasons[i].startDay <= day)
                {
                    current = seasons[i];
                }
            }
            return current ?? SeasonWindow.Default;
        }
    }

    /// <summary>
    /// One named stretch of the campaign with its own weighted-random weather mix.
    /// </summary>
    [Serializable]
    public class SeasonWindow
    {
        /// <summary>Fallback used when a SeasonProfile has no configured seasons: equal weights, always active.</summary>
        public static readonly SeasonWindow Default = new SeasonWindow
        {
            id = "default",
            displayName = "Default",
            startDay = 0,
            clearWeight = 1f,
            ashfallWeight = 1f,
            falloutStormWeight = 1f,
            blizzardWeight = 1f
        };

        public string id;
        public string displayName;

        /// <summary>First campaign day (inclusive) this window applies from.</summary>
        public int startDay;

        public float clearWeight = 1f;
        public float ashfallWeight = 1f;
        public float falloutStormWeight = 1f;
        public float blizzardWeight = 1f;

        /// <summary>Designer-tunable weight for one WeatherKind within this season.</summary>
        public float GetWeight(WeatherKind kind)
        {
            switch (kind)
            {
                case WeatherKind.Clear: return clearWeight;
                case WeatherKind.Ashfall: return ashfallWeight;
                case WeatherKind.FalloutStorm: return falloutStormWeight;
                case WeatherKind.Blizzard: return blizzardWeight;
                default: return 0f;
            }
        }
    }
}
