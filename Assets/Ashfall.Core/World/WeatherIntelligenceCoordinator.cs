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
    /// weather-station calibration/forecast state, orbital-harrow telemetry
    /// state, and seasonal events so the world section persists them together.
    /// </summary>
    [Serializable]
    public sealed class WeatherIntelligenceSaveState
    {
        public WeatherStationState station = new WeatherStationState();
        public OrbitalTelemetryState orbital = new OrbitalTelemetryState();
        public SeasonalEventSaveState seasonal = new SeasonalEventSaveState();
    }

    // ── Read model (consumed by Weather and Map panels) ─────────────────────

    /// <summary>
    /// Read-only projection of weather intelligence for the UI: forecast
    /// confidence, orbital warning lead time, seasonal phase, and expedition route-safety
    /// information. Improving weather infrastructure (installing/calibrating
    /// the station, activating orbital telemetry) demonstrably enriches this
    /// model — that is the player-facing signal that infrastructure matters.
    /// </summary>
    [Serializable]
    public sealed class WeatherIntelligenceReadModel
    {
        // Season
        public string seasonId = string.Empty;
        public string seasonDisplayName = string.Empty;

        // Station
        public bool stationInstalled;
        public bool stationCalibrated;
        public bool stationOperational;
        public WeatherStationTier stationTier;
        public string stationTierName = string.Empty;
        public float stationAccuracy;
        public float stationDurability;
        public int forecastHorizonDays;
        public int lastForecastDay;
        public List<ForecastEntry> forecast = new List<ForecastEntry>();

        // Orbital
        public bool telemetryActive;
        public bool hasPendingImpact;
        public int impactDay;
        public int warningLeadDays;
        public int daysUntilImpact;
        public int activeSalvageCount;
        public List<OrbitalSalvageOpportunity> activeSalvage = new List<OrbitalSalvageOpportunity>();

        // Seasonal events
        public List<ActiveSeasonalEvent> activeSeasonalEvents = new List<ActiveSeasonalEvent>();

        // Derived expedition information
        public int routeSafeDays;
        public int bestTravelDay;
        public float bestTravelConfidence;
        public string advisory = string.Empty;
    }

    /// <summary>
    /// Weather-intelligence coordinator — the single wiring point for the
    /// weather-infrastructure systems (WeatherStation, OrbitalHarrowTelemetry,
    /// and SeasonalEventSystem). Owns all three systems, feeds the station from the
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
        public SeasonalEventSystem Seasonal { get; }

        private readonly WeatherSystem _weather;
        private readonly SkyLayerArmorSystem _armor;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        /// <summary>
        /// Raised whenever the station forecast, orbital telemetry, or seasonal event changes,
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
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;

            int seed = _rng.Seed;
            Station = new WeatherStationSystem(_weather, new SeededRng(seed), _log);
            Orbital = new OrbitalHarrowTelemetrySystem(_armor, new SeededRng(unchecked(seed ^ 0x5A5A5A5A)), _log);
            Seasonal = new SeasonalEventSystem(_log);

            Station.OnForecastUpdated += RaiseChanged;
            Station.OnStationStateChanged += RaiseChanged;
            Orbital.OnTelemetryChanged += RaiseChanged;
            Orbital.OnImpactWarning += _ => RaiseChanged();
            Orbital.OnImpactResolved += (_, _) => RaiseChanged();
            Seasonal.OnStateChanged += RaiseChanged;
        }

        // ── Daily tick ──────────────────────────────────────────────────────

        /// <summary>
        /// Advance the weather-intelligence cluster by one day. Regenerates
        /// the station forecast (if operational), resolves pending orbital impacts,
        /// and evaluates seasonal events.
        /// </summary>
        public void TickDay(int day)
        {
            _currentDay = day;
            if (Station.IsOperational)
                Station.GenerateForecast(day);

            Orbital.TickDay(day);

            var season = _weather.GetSeasonForDay(day);
            Seasonal.TickDay(day, season?.id ?? "window_ashfall", new SeededRng(unchecked(_rng.Seed * 31 + day)));
        }

        // ── Read model ─────────────────────────────────────────────────────

        public WeatherIntelligenceReadModel BuildReadModel()
        {
            var s = Station.State;
            var o = Orbital.State;
            var season = _weather.GetSeasonForDay(_currentDay);

            var rm = new WeatherIntelligenceReadModel
            {
                seasonId = season?.id ?? "window_ashfall",
                seasonDisplayName = season?.displayName ?? "Ash Fall",
                stationInstalled = s.isInstalled,
                stationCalibrated = s.isCalibrated,
                stationOperational = Station.IsOperational,
                stationTier = Station.CurrentTier,
                stationTierName = Station.CurrentTier.ToString(),
                stationAccuracy = s.accuracy,
                stationDurability = s.durability,
                forecastHorizonDays = Station.EffectiveHorizonDays,
                lastForecastDay = s.lastForecastDay,
                telemetryActive = o.telemetryActive,
                hasPendingImpact = Orbital.HasPendingImpact,
                impactDay = o.nextImpactDay,
                warningLeadDays = o.warningLeadDays,
                daysUntilImpact = o.nextImpactDay > _currentDay ? o.nextImpactDay - _currentDay : 0,
                activeSalvageCount = o.activeSalvage.FindAll(x => !x.isClaimed).Count
            };

            foreach (var f in s.cachedForecast)
                rm.forecast.Add(f);

            foreach (var sal in o.activeSalvage)
                rm.activeSalvage.Add(sal);

            foreach (var evt in Seasonal.ActiveEvents)
                rm.activeSeasonalEvents.Add(evt);

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
            sb.Append($"Season: {rm.seasonDisplayName}. ");

            if (!rm.stationOperational)
            {
                sb.Append("Weather station offline/damaged — forecast unavailable. ");
            }
            else
            {
                sb.Append($"Station tier: {rm.stationTierName} (accuracy {rm.stationAccuracy:P0}, {rm.forecastHorizonDays}d horizon). ");
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

            if (rm.activeSeasonalEvents.Count > 0)
            {
                sb.Append($"Active seasonal hazards: {rm.activeSeasonalEvents.Count}. ");
            }

            return sb.ToString().Trim();
        }

        // ── Save / Load ─────────────────────────────────────────────────────

        public WeatherIntelligenceSaveState CaptureState()
        {
            return new WeatherIntelligenceSaveState
            {
                station = Station.CaptureState(),
                orbital = Orbital.CaptureState(),
                seasonal = Seasonal.CaptureState()
            };
        }

        public void RestoreState(WeatherIntelligenceSaveState? saved)
        {
            if (saved == null) return;
            if (saved.station != null) Station.RestoreState(saved.station);
            if (saved.orbital != null) Orbital.RestoreState(saved.orbital);
            if (saved.seasonal != null) Seasonal.RestoreState(saved.seasonal);
            RaiseChanged();
        }

        private void RaiseChanged() => OnIntelligenceChanged?.Invoke();
    }
}
