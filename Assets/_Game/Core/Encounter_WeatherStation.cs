using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WeatherStationState
    {
        public string id = "encounter_weather_station";
        public string displayName = "Automated Weather Station";
        public bool isSolarPanelTaken = false;
        public bool isDataExtracted = false;
        public bool isElectronicsScavenged = false;
        public int forecastAccuracyBonusDays = 5;
        public int electronicScrapYield = 4;
        public int solarCellYield = 2;
    }

    /// <summary>
    /// Prompt #903: Encounter — Automated Weather Station.
    /// On a low hill three kilometers east of Tessarat, a pre-war weather
    /// station is still running on a cracked solar panel. Its LED blinks
    /// green every twelve seconds. The data logger contains five days of
    /// barometric pressure, wind speed, and plume-drift readings.
    ///
    /// The survivor can:
    ///   - Extract the weather data (improves forecast accuracy for 5 days,
    ///     reducing the chance of being caught in a fallout storm)
    ///   - Take the solar panel (2 solar_cell items, but station goes dark)
    ///   - Scavenge the electronics (4 electronic_scrap, destroys station)
    ///   - Leave it running (a small, quiet act of preservation)
    ///
    /// No enemies. No timer. Just a choice about what to preserve and what
    /// to use up.
    /// </summary>
    public class Encounter_WeatherStation
    {
        private static System.Random _fallbackRng;
        private static System.Random FallbackRng =>
            _fallbackRng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("encounter_weatherstation");

        private WeatherStationState _state = new WeatherStationState();

        /// <summary>Item id for solar cells harvested from the panel.</summary>
        public const string SolarCellItemId = "solar_cell";

        /// <summary>Item id for electronic scrap.</summary>
        public const string ElectronicScrapItemId = "electronic_scrap";

        /// <summary>World flag set when data is extracted (WeatherSystem reads this).</summary>
        public const string ForecastBoostFlag = "weather_data_extracted";

        /// <summary>Small morale lift for leaving it running.</summary>
        public const float LeaveRunningMoraleBoost = 3f;

        // ── Events ──────────────────────────────────────────────────────
        public event Action<WeatherStationState> OnStationDiscovered;
        public event Action<WeatherStationState> OnDataExtracted;
        public event Action<WeatherStationState> OnSolarPanelTaken;
        public event Action<WeatherStationState> OnElectronicsScavenged;
        public event Action<WeatherStationState> OnLeftRunning;

        public WeatherStationState State => _state;

        // ── Public API ──────────────────────────────────────────────────

        public void Discover()
        {
            OnStationDiscovered?.Invoke(_state);
        }

        /// <summary>
        /// Extract five days of weather data. Improves forecast accuracy.
        /// Sets a world flag that WeatherSystem reads.
        /// </summary>
        public bool ExtractData()
        {
            if (_state.isDataExtracted) return false;
            _state.isDataExtracted = true;
            OnDataExtracted?.Invoke(_state);
            return true;
        }

        /// <summary>
        /// Take the solar panel. Yields solar cells but the station
        /// stops transmitting.
        /// </summary>
        public int TakeSolarPanel()
        {
            if (_state.isSolarPanelTaken) return 0;
            _state.isSolarPanelTaken = true;
            OnSolarPanelTaken?.Invoke(_state);
            return _state.solarCellYield;
        }

        /// <summary>
        /// Strip the station for electronic scrap. Destroys it completely.
        /// </summary>
        public int ScavengeElectronics()
        {
            if (_state.isElectronicsScavenged) return 0;
            _state.isElectronicsScavenged = true;
            _state.isSolarPanelTaken = true; // panel gets taken too
            OnElectronicsScavenged?.Invoke(_state);
            return _state.electronicScrapYield;
        }

        /// <summary>
        /// Leave the station running. Small morale boost — a quiet act
        /// of preservation in a world that's running out of things
        /// that still work.
        /// </summary>
        public float LeaveRunning()
        {
            OnLeftRunning?.Invoke(_state);
            return LeaveRunningMoraleBoost;
        }

        // ── Forecast accuracy bonus ─────────────────────────────────────

        /// <summary>How many days of improved forecasts are available.</summary>
        public int GetForecastBonusDays() =>
            _state.isDataExtracted ? _state.forecastAccuracyBonusDays : 0;

        // ── Save / Load ─────────────────────────────────────────────────

        public WeatherStationState CaptureState() => _state;

        public void RestoreState(WeatherStationState save)
        {
            if (save != null) _state = save;
        }
    }
}
