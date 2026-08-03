using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Definition of a shelter electricity source (diesel gen, bicycle, wind, solar).
    /// Data-driven; runtime state lives on <see cref="PowerSourceInstance"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "PowerSource", menuName = "ASHFALL/Shelter/Power Source")]
    public class PowerSourceSO : ScriptableObject
    {
        [Header("Identity")]
        public string SourceId;
        public string DisplayName;
        [TextArea(2, 4)] public string Description;
        public PowerSourceKind Kind = PowerSourceKind.Diesel;

        [Header("Output")]
        [Tooltip("Watts contributed while this source is actively generating.")]
        public float OutputWatts = 50f;

        [Header("Diesel")]
        [Tooltip("Fuel units burned per game-hour while generating.")]
        public float FuelPerHour = 2f;
        public string FuelItemId = "fuel";
        [Tooltip("Carbon monoxide ppm added per game-hour of diesel generation (Prompt #20).")]
        public float CoPpmPerHour = 8f;

        [Header("Bicycle / human power")]
        [Tooltip("Fatigue points added to the pedaling survivor per game-hour.")]
        public float FatiguePerHour = 12f;
        [Tooltip("Hunger points added (calories burned) per game-hour of pedaling.")]
        public float HungerPerHour = 6f;
        [Tooltip("If true, generation requires an assigned pedaling survivor.")]
        public bool RequiresPedaling = false;

        [Header("Weather gates (Wind / Solar)")]
        [Tooltip("Comma-separated WeatherKind names that allow generation (empty = always).")]
        public string RequiredWeatherKinds = "";

        public bool IsWeatherGated =>
            Kind == PowerSourceKind.Wind || Kind == PowerSourceKind.Solar
            || !string.IsNullOrEmpty(RequiredWeatherKinds);

        /// <summary>True when current weather name is allowed for this source.</summary>
        public bool AllowsWeather(string weatherName)
        {
            if (!IsWeatherGated && string.IsNullOrEmpty(RequiredWeatherKinds))
                return true;
            if (string.IsNullOrEmpty(RequiredWeatherKinds))
            {
                // Defaults when Kind implies weather but no explicit list set.
                if (Kind == PowerSourceKind.Solar)
                    return string.Equals(weatherName, "Clear", System.StringComparison.OrdinalIgnoreCase);
                if (Kind == PowerSourceKind.Wind)
                    return string.Equals(weatherName, "Ashfall", System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(weatherName, "Blizzard", System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(weatherName, "FalloutStorm", System.StringComparison.OrdinalIgnoreCase)
                        || string.Equals(weatherName, "Rain", System.StringComparison.OrdinalIgnoreCase);
                return true;
            }

            if (string.IsNullOrEmpty(weatherName)) return false;
            var parts = RequiredWeatherKinds.Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                if (string.Equals(parts[i].Trim(), weatherName, System.StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>Factory: 50 W diesel generator (test / bootstrap default).</summary>
        public static PowerSourceSO CreateDieselGenerator(float outputWatts = 50f)
        {
            var so = CreateInstance<PowerSourceSO>();
            so.SourceId = "diesel_generator";
            so.DisplayName = "Diesel Generator";
            so.Description = "Loud, thirsty, and it fouls the air. Keeps the critical loads alive.";
            so.Kind = PowerSourceKind.Diesel;
            so.OutputWatts = outputWatts;
            so.FuelPerHour = 2f;
            so.FuelItemId = "fuel";
            so.CoPpmPerHour = 8f;
            so.RequiresPedaling = false;
            return so;
        }

        /// <summary>Factory: low-watt bicycle generator for manual generation.</summary>
        public static PowerSourceSO CreateBicycleGenerator(float outputWatts = 20f)
        {
            var so = CreateInstance<PowerSourceSO>();
            so.SourceId = "bicycle_generator";
            so.DisplayName = "Bicycle Generator";
            so.Description = "Legs for watts. Burns fatigue and calories; no fuel, no CO.";
            so.Kind = PowerSourceKind.Bicycle;
            so.OutputWatts = outputWatts;
            so.FuelPerHour = 0f;
            so.FatiguePerHour = 12f;
            so.HungerPerHour = 6f;
            so.RequiresPedaling = true;
            so.CoPpmPerHour = 0f;
            return so;
        }
    }
}
