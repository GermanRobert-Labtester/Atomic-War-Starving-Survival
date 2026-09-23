// SPDX-License-Identifier: MIT
using Ashfall.Core.Radio;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Long-range reconnaissance drones and high-altitude mapping.
    /// Owns launched recon platforms, active missions, telemetry link state,
    /// gathered recon intelligence, platform recovery/loss, and recon-derived
    /// temporary route/scouting capabilities.
    /// </summary>
    public sealed class ReconTelemetrySystem
    {
        public const string SystemId = "recon_telemetry";

        private ReconTelemetryState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Dictionary<string, ReconProbeDef> _platforms = new(StringComparer.Ordinal);
        private readonly WastelandMapSystem? _wastelandMap;
        private readonly ShelterRadioStationSystem? _radio;
        private readonly WeatherSystem? _weather;
        private readonly Ashfall.Core.Inventory.Inventory? _inventory;
        private int _currentDay;

        public ReconTelemetryState State => _state;
        public IReadOnlyDictionary<string, ReconProbeDef> Platforms => _platforms;
        public float GetRadioRangeMultiplier()
        {
            foreach (var pid in _state.launchedPlatformIds)
            {
                if (_platforms.TryGetValue(pid, out var def) && def.RadioRangeMultiplier.HasValue)
                    return def.RadioRangeMultiplier.Value;
            }
            return 1f;
        }
        public float GetRouteSpeedMultiplier(string routeId)
        {
            var scout = _state.scoutedRoutes.FirstOrDefault(r => r.routeId == routeId && r.isActive);
            return scout?.speedMultiplier ?? 1f;
        }

        public event Action<string>? OnReconLaunched;            // missionId
        public event Action<string>? OnSurveyCompleted;         // missionId
        public event Action<string, string>? OnPlatformLost;    // platformId, reason
        public event Action<string>? OnPlatformRecovered;       // platformId
        public event Action<string>? OnFalloutForecastGenerated;// forecastId
        public event Action<string>? OnRouteScouted;            // routeId

        public ReconTelemetrySystem(
            ReconTelemetryState? state,
            ISeededRng rng,
            ILog? log = null,
            WastelandMapSystem? wastelandMap = null,
            ShelterRadioStationSystem? radio = null,
            WeatherSystem? weather = null,
            Ashfall.Core.Inventory.Inventory? inventory = null)
        {
            _state = state ?? new ReconTelemetryState();
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _wastelandMap = wastelandMap;
            _radio = radio;
            _weather = weather;
            _inventory = inventory;
        }

        // ── Catalog ──────────────────────────────────────────────────

        public void RegisterPlatform(ReconProbeDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.PlatformId))
                _platforms[def.PlatformId] = def;
        }

        public void LoadCatalog(ReconTelemetryCatalog? catalog)
        {
            if (catalog?.Platforms == null) return;
            foreach (var def in catalog.Platforms)
                RegisterPlatform(def);
        }

        // ── Queries ──────────────────────────────────────────────────

        public ActiveReconMissionState? GetMission(string missionId)
        {
            return _state.activeMissions.Find(m => m.missionId == missionId);
        }

        public bool IsPlatformLaunched(string platformId)
        {
            return _state.launchedPlatformIds.Contains(platformId);
        }

        // ── Actions ──────────────────────────────────────────────────

        public LaunchResult LaunchMission(string platformId, string targetSectorId)
        {
            if (string.IsNullOrEmpty(platformId)) return LaunchResult.Failed("invalid_platform", "recon.invalid_platform");
            if (!_platforms.TryGetValue(platformId, out var def)) return LaunchResult.Failed("unknown_platform", "recon.unknown_platform");
            if (IsPlatformLaunched(platformId) && !def.Recoverable)
                return LaunchResult.Failed("already_launched", "recon.already_launched");

            // Weather check — the canonical weather authority owns surface wind
            // (Plan 205). Unbound weather keeps the legacy no-wind behavior.
            float windKph = _weather?.WindSpeedKph ?? 0f;
            if (windKph > def.WindLimitKph)
                return LaunchResult.Blocked("wind_too_high", "recon.wind_too_high");

            // Inventory check
            if (_inventory != null)
            {
                foreach (var cost in def.LaunchCosts)
                {
                    if (!_inventory.HasSufficient(cost.ItemId, cost.Amount))
                        return LaunchResult.Blocked("insufficient_items", "recon.insufficient_items");
                }
                if (def.BatteryCost > 0 && !_inventory.HasSufficient("item_battery", (int)Math.Ceiling(def.BatteryCost)))
                    return LaunchResult.Blocked("insufficient_battery", "recon.insufficient_battery");

                foreach (var cost in def.LaunchCosts)
                    _inventory.TryConsume(cost.ItemId, cost.Amount);
                if (def.BatteryCost > 0)
                    _inventory.TryConsume("item_battery", (int)Math.Ceiling(def.BatteryCost));
            }

            string missionId = $"recon_{_state.activeMissions.Count + 1}";
            var mission = new ActiveReconMissionState
            {
                missionId = missionId,
                platformId = platformId,
                targetSectorId = targetSectorId,
                batteryRemaining = def.EnduranceHours,
                status = "launched",
                launchDay = _currentDay,
                expectedRecoveryDay = _currentDay + (int)Math.Ceiling(def.EnduranceHours / 24f)
            };

            _state.activeMissions.Add(mission);
            if (!_state.launchedPlatformIds.Contains(platformId))
                _state.launchedPlatformIds.Add(platformId);

            OnReconLaunched?.Invoke(missionId);
            _log.Info($"[Recon] Launched {def.DisplayName} mission {missionId}");
            return LaunchResult.Success(missionId);
        }

        public ActionResult RecoverPlatform(string missionId)
        {
            var mission = GetMission(missionId);
            if (mission == null) return ActionResult.Failed("unknown_mission", "recon.unknown_mission");
            if (mission.status != "launched" && mission.status != "returning")
                return ActionResult.Failed("not_recoverable", "recon.not_recoverable");

            if (_platforms.TryGetValue(mission.platformId, out var def) && def.Recoverable)
            {
                _state.launchedPlatformIds.Remove(mission.platformId);
                mission.status = "recovered";
                OnPlatformRecovered?.Invoke(mission.platformId);
                _log.Info($"[Recon] Recovered {mission.platformId}");
                return ActionResult.Success("recon.recovered");
            }

            mission.status = "lost";
            OnPlatformLost?.Invoke(mission.platformId, "not_recoverable");
            return ActionResult.Success("recon.lost");
        }

        public SurveyResult SurveySectors(string missionId, List<string> sectorIds)
        {
            var mission = GetMission(missionId);
            if (mission == null) return SurveyResult.Failed("unknown_mission", "recon.unknown_mission");
            if (mission.status != "launched" && mission.status != "surveying")
                return SurveyResult.Failed("not_surveying", "recon.not_surveying");

            if (_wastelandMap != null)
            {
                foreach (var sector in sectorIds)
                {
                    _wastelandMap.Discover(sector);
                    if (!mission.surveyedSectorIds.Contains(sector))
                        mission.surveyedSectorIds.Add(sector);
                    if (!_state.surveyedSectorIds.Contains(sector))
                        _state.surveyedSectorIds.Add(sector);
                }
            }

            mission.status = "returning";
            OnSurveyCompleted?.Invoke(missionId);
            return SurveyResult.Success(sectorIds);
        }

        public ActionResult GenerateForecast(string platformId)
        {
            if (!_platforms.TryGetValue(platformId, out var def))
                return ActionResult.Failed("unknown_platform", "recon.unknown_platform");
            if (!def.SensorSuite.Contains("weather") && !def.SensorSuite.Contains("radiation"))
                return ActionResult.Blocked("no_forecast_sensors", "recon.no_forecast_sensors");

            string forecastId = $"forecast_{_state.forecasts.Count + 1}";
            _state.forecasts.Add(new ReconForecastRecord
            {
                forecastId = forecastId,
                platformId = platformId,
                generatedDay = _currentDay,
                expiryDay = _currentDay + (def.ForecastHorizonHours ?? 24) / 24,
                frontType = "fallout_front",
                confidence = 0.7f,
                affectedSectorIds = new List<string> { "loc_holdfast" }
            });

            OnFalloutForecastGenerated?.Invoke(forecastId);
            return ActionResult.Success("recon.forecast_generated");
        }

        public ActionResult ScoutRoute(string routeId, string missionId)
        {
            var mission = GetMission(missionId);
            if (mission == null) return ActionResult.Failed("unknown_mission", "recon.unknown_mission");

            _state.scoutedRoutes.Add(new RouteScoutRecord
            {
                routeId = routeId,
                missionId = missionId,
                speedMultiplier = 0.75f,
                expiryDay = _currentDay + 7,
                isActive = true
            });

            OnRouteScouted?.Invoke(routeId);
            return ActionResult.Success("recon.route_scouted");
        }

        // ── Daily Tick ───────────────────────────────────────────────

        public void TickDay(int day)
        {
            _currentDay = day;
            if (day <= _state.lastProcessedDay) return;
            _state.lastProcessedDay = day;

            // Expire old route scouts
            _state.scoutedRoutes.RemoveAll(r => r.expiryDay <= day);

            // Expire old forecasts
            _state.forecasts.RemoveAll(f => f.expiryDay <= day);

            // Platform loss check for active missions
            foreach (var mission in _state.activeMissions.ToList())
            {
                if (mission.status == "launched" || mission.status == "surveying")
                {
                    // Simplified: 2% daily loss chance
                    if (_rng.NextDouble() < 0.02)
                    {
                        mission.status = "lost";
                        mission.lossReason = "weather_event";
                        OnPlatformLost?.Invoke(mission.platformId, "weather_event");
                    }
                }
            }
        }

        // ── Persistence ──────────────────────────────────────────────

        public ReconTelemetryState CaptureState() => CloneState(_state);

        public void RestoreState(ReconTelemetryState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ReconTelemetryState CloneState(ReconTelemetryState src)
        {
            if (src == null) return new ReconTelemetryState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ReconTelemetryState>(json) ?? new ReconTelemetryState();
        }
    }

    // ── Result DTOs ────────────────────────────────────────────────

    public sealed class LaunchResult
    {
        public bool IsSuccess { get; }
        public bool IsBlocked => !IsSuccess && FailureCode != null;
        public string? FailureCode { get; }
        public string MessageKey { get; }
        public string MissionId { get; }

        private LaunchResult(bool success, string? failureCode, string messageKey, string missionId)
        {
            IsSuccess = success;
            FailureCode = failureCode;
            MessageKey = messageKey;
            MissionId = missionId;
        }

        public static LaunchResult Failed(string code, string key) => new LaunchResult(false, code, key, string.Empty);
        public static LaunchResult Blocked(string code, string key) => new LaunchResult(false, code, key, string.Empty);
        public static LaunchResult Success(string missionId) => new LaunchResult(true, null, string.Empty, missionId);
    }

    public sealed class SurveyResult
    {
        public bool IsSuccess { get; }
        public string FailureCode { get; }
        public string MessageKey { get; }
        public List<string> SurveyedSectors { get; }

        private SurveyResult(bool success, string failureCode, string messageKey, List<string> sectors)
        {
            IsSuccess = success;
            FailureCode = failureCode;
            MessageKey = messageKey;
            SurveyedSectors = sectors ?? new List<string>();
        }

        public static SurveyResult Failed(string code, string key) => new SurveyResult(false, code, key, new List<string>());
        public static SurveyResult Success(List<string> sectors) => new SurveyResult(true, null, string.Empty, sectors);
    }
}
