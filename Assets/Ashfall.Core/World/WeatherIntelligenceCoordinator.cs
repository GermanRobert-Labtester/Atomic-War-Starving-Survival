using System;
using System.Collections.Generic;
using System.Text;
#pragma warning disable CS8618

using Ashfall.Core.Shelter;

namespace Ashfall.Core.World
{
    // ── Aggregate save DTO (one nested state, not two loose files) ──────────

    /// <summary>
    /// Single persisted state for the weather-intelligence cluster. Nests the
    /// weather-station calibration/forecast state and the orbital-harrow
    /// telemetry state so the world section persists them together.
    /// </summary>
    [Serializable]
    public sealed class WeatherIntelligenceSaveState
    {
        public WeatherStationState station = new WeatherStationState();
        public OrbitalTelemetryState orbital = new OrbitalTelemetryState();
    }

    // ── Read model (consumed by Weather and Map panels) ─────────────────────

    /// <summary>
    /// Read-only projection of weather intelligence for the UI: forecast
    /// confidence, orbital warning lead time, and expedition route-safety
    /// information. Improving weather infrastructure (installing/calibrating
    /// the station, activating orbital telemetry) demonstrably enriches this
    /// model — that is the player-facing signal that infrastructure matters.
    /// </summary>
    [Serializable]
    public sealed class WeatherIntelligenceReadModel
    {
        // Station
        public bool stationInstalled;
        public bool stationCalibrated;
        public bool stationOperational;
        public float stationAccuracy;
        public int forecastHorizonDays;
        public int lastForecastDay;
        public List<ForecastEntry> forecast = new List<ForecastEntry>();

        // Orbital
        public bool telemetryActive;
        public bool hasPendingImpact;
        public int impactDay;
        public int warningLeadDays;
        public int daysUntilImpact;

        // Derived expedition information
        public int routeSafeDays;
        public int bestTravelDay;
        public float bestTravelConfidence;
        public string advisory = string.Empty;
    }

    /// <summary>
    /// Weather-intelligence coordinator — the single wiring point for the two
    /// dormant weather-infrastructure systems (WeatherStation and
    /// OrbitalHarrowTelemetry). Owns both systems, feeds the station from the
    /// authoritative <see cref="WeatherSystem"/> forecast rolls, ticks the
    /// orbital impact clock against <see cref="SkyLayerArmorSystem"/>, and
    /// persists as ONE nested state inside the world section of the campaign
    /// envelope.
    ///
    /// Engine-agnostic (Core). The host constructs it with real system
    /// references and calls <see cref="TickDay"/> from the daily tick.
    /// </summary>
    public sealed class WeatherIntelligenceCoordinator
    {
        public WeatherStationSystem Station { get; }
        public OrbitalHarrowTelemetrySystem Orbital { get; }

        private readonly WeatherSystem _weather;
        private readonly SkyLayerArmorSystem _armor;
        private readonly ILog _log;
        private int _currentDay;

        /// <summary>
        /// Raised whenever the station forecast or orbital telemetry changes,
        /// so the host can mark the world section dirty and refresh panels.
        /// </summary>
        public event Action? OnIntelligenceChanged;

        public WeatherIntelligenceCoordinator(
            WeatherSystem weather,
            SkyLayerArmorSystem armor,
            ISeededRng rng,
            ILog? log = null)
        {
            _weather = weather ?? throw new ArgumentNullException(nameof(weather));
            _armor = armor ?? throw new ArgumentNullException(nameof(armor));
            _log = log ?? NullLog.Instance;

            // Independent deterministic RNG streams so station calibration and
            // orbital rolls never interfere with each other's sequence.
            int seed = rng?.Seed ?? 0;
            Station = new WeatherStationSystem(_weather, new SeededRng(seed), _log);
            Orbital = new OrbitalHarrowTelemetrySystem(_armor, new SeededRng(unchecked(seed ^ 0x5A5A5A5A)), _log);

            Station.OnForecastUpdated += RaiseChanged;
            Station.OnStationStateChanged += RaiseChanged;
            Orbital.OnTelemetryChanged += RaiseChanged;
            Orbital.OnImpactWarning += _ => RaiseChanged();
            Orbital.OnImpactResolved += (_, _) => RaiseChanged();
        }

        // ── Daily tick ──────────────────────────────────────────────────────

        /// <summary>
        /// Advance the weather-intelligence cluster by one day. Regenerates
        /// the station forecast (if operational) and resolves any pending
        /// orbital impact that lands on this day.
        /// </summary>
        public void TickDay(int day)
        {
            _currentDay = day;
            if (Station.IsOperational)
                Station.GenerateForecast(day);
            Orbital.TickDay(day);
        }

        // ── Read model ─────────────────────────────────────────────────────

        /// <summary>
        /// Build a UI-facing read model. Without a calibrated station the
        /// forecast list is empty and confidence is zero; with one, the
        /// forecast carries confidence-weighted route-safety entries. Without
        /// telemetry the orbital block is inert; with it, impact warnings
        /// carry a lead time. This progression is the player-facing signal
        /// that weather infrastructure investment pays off.
        /// </summary>
        public WeatherIntelligenceReadModel BuildReadModel()
        {
            var s = Station.State;
            var o = Orbital.State;

            var rm = new WeatherIntelligenceReadModel
            {
                stationInstalled = s.isInstalled,
                stationCalibrated = s.isCalibrated,
                stationOperational = Station.IsOperational,
                stationAccuracy = s.accuracy,
                forecastHorizonDays = s.forecastHorizonDays,
                lastForecastDay = s.lastForecastDay,
                telemetryActive = o.telemetryActive,
                hasPendingImpact = Orbital.HasPendingImpact,
                impactDay = o.nextImpactDay,
                warningLeadDays = o.warningLeadDays,
                daysUntilImpact = o.nextImpactDay > _currentDay ? o.nextImpactDay - _currentDay : 0
            };

            // Copy the cached forecast (if any).
            foreach (var f in s.cachedForecast)
                rm.forecast.Add(f);

            // Derive expedition route-safety information from the forecast.
            int safeDays = 0;
            int bestDay = 0;
            float bestConf = 0f;
            foreach (var f in s.cachedForecast)
            {
                if (f.isRouteSafe)
                {
                    safeDays++;
                    if (bestDay == 0 || f.confidence > bestConf)
                    {
                        bestDay = f.day;
                        bestConf = f.confidence;
                    }
                }
            }
            rm.routeSafeDays = safeDays;
            rm.bestTravelDay = bestDay;
            rm.bestTravelConfidence = bestConf;

            rm.advisory = BuildAdvisory(rm);
            return rm;
        }

        private static string BuildAdvisory(WeatherIntelligenceReadModel rm)
        {
            var sb = new StringBuilder();
            if (!rm.stationOperational)
            {
                sb.Append("No calibrated weather station — forecast unavailable. ");
            }
            else
            {
                sb.Append($"Station online (accuracy {rm.stationAccuracy:P0}). ");
                if (rm.routeSafeDays > 0)
                    sb.Append($"Best travel window: day {rm.bestTravelDay} ({rm.bestTravelConfidence:P0} confidence). ");
                else
                    sb.Append("No safe travel windows in forecast. ");
            }
            if (rm.telemetryActive)
            {
                if (rm.hasPendingImpact)
                    sb.Append($"Orbital impact in {rm.daysUntilImpact}d (grid warning active). ");
                else
                    sb.Append("Orbital telemetry clear. ");
            }
            return sb.ToString().Trim();
        }

        // ── Save / Load ─────────────────────────────────────────────────────

        public WeatherIntelligenceSaveState CaptureState()
        {
            return new WeatherIntelligenceSaveState
            {
                station = Station.CaptureState(),
                orbital = Orbital.CaptureState()
            };
        }

        public void RestoreState(WeatherIntelligenceSaveState? saved)
        {
            if (saved == null) return;
            Station.RestoreState(saved.station);
            Orbital.RestoreState(saved.orbital);
            RaiseChanged();
        }

        private void RaiseChanged() => OnIntelligenceChanged?.Invoke();
    }
}
