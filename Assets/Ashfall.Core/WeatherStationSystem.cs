using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.World;

namespace Ashfall.Core
{
    public enum WeatherStationTier
    {
        Offline = 0,
        Damaged = 1,
        Functional = 2,
        Calibrated = 3
    }

    [Serializable]
    public sealed class WeatherStationState
    {
        public string systemId = WeatherStationSystem.SystemId;
        public bool isInstalled;
        public bool isCalibrated;
        public int installDay = -1;
        public int calibrationDay = -1;
        public int forecastHorizonDays = 3;
        public float accuracy = 0.7f;
        public float durability = 100f; // 0 to 100
        public bool hasSensorFault;
        public string faultReason = string.Empty;
        public int lastForecastDay = -1;
        public List<ForecastEntry> cachedForecast = new List<ForecastEntry>();
    }

    [Serializable]
    public sealed class ForecastEntry
    {
        public int day;
        public WeatherKind weather;
        public float confidence;
        public bool isRouteSafe;
        public float temperature;
        public string warning = string.Empty;
        public string preparationPayoff = string.Empty;
        public string atmosphericFlavor = string.Empty;
    }

    public sealed class WeatherStationSystem
    {
        public const string SystemId = "weather_station";

        private WeatherStationState _state = new WeatherStationState();
        private readonly WeatherSystem _weatherSystem;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public WeatherStationState State => _state;
        public bool IsOperational => _state.isInstalled && _state.isCalibrated && !_state.hasSensorFault && _state.durability >= 20f;
        public WeatherStationTier CurrentTier
        {
            get
            {
                if (!_state.isInstalled || _state.durability <= 0f)
                    return WeatherStationTier.Offline;
                if (_state.durability < 40f || _state.hasSensorFault)
                    return WeatherStationTier.Damaged;
                if (!_state.isCalibrated)
                    return WeatherStationTier.Functional;
                return WeatherStationTier.Calibrated;
            }
        }

        public int EffectiveHorizonDays => CurrentTier switch
        {
            WeatherStationTier.Offline => 0,
            WeatherStationTier.Damaged => 1,
            WeatherStationTier.Functional => 3,
            WeatherStationTier.Calibrated => 7,
            _ => 0
        };

        public event Action OnForecastUpdated;
        public event Action OnStationStateChanged;

        public WeatherStationSystem(WeatherSystem weatherSystem, ISeededRng rng, ILog? log = null)
        {
            _weatherSystem = weatherSystem ?? throw new ArgumentNullException(nameof(weatherSystem));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult Install(int day)
        {
            if (_state.isInstalled) return ActionResult.Blocked("already_installed", "weather.already_installed");
            _state.isInstalled = true;
            _state.installDay = day;
            _state.durability = 100f;
            _state.hasSensorFault = false;
            _state.faultReason = string.Empty;
            _log.Info($"[WeatherStation] installed on day {day}");
            OnStationStateChanged?.Invoke();
            return ActionResult.Success("weather.installed");
        }

        public ActionResult Calibrate(int day)
        {
            if (!_state.isInstalled) return ActionResult.Blocked("not_installed", "weather.not_installed");
            if (_state.isCalibrated) return ActionResult.Blocked("already_calibrated", "weather.already_calibrated");
            _state.isCalibrated = true;
            _state.calibrationDay = day;
            _state.accuracy = 0.75f + (float)_rng.NextDouble() * 0.20f;
            _state.forecastHorizonDays = 7;
            _log.Info($"[WeatherStation] calibrated (accuracy={_state.accuracy:F2})");
            OnStationStateChanged?.Invoke();
            return ActionResult.Success("weather.calibrated", new Dictionary<string, double> { { "accuracy", _state.accuracy } });
        }

        public ActionResult Repair(int day, float durabilityAmount)
        {
            if (!_state.isInstalled) return ActionResult.Blocked("not_installed", "weather.not_installed");
            _state.durability = Math.Min(100f, _state.durability + Math.Max(0f, durabilityAmount));
            if (_state.durability >= 40f && _state.hasSensorFault)
            {
                _state.hasSensorFault = false;
                _state.faultReason = string.Empty;
            }
            _log.Info($"[WeatherStation] repaired to durability {_state.durability:F1}");
            OnStationStateChanged?.Invoke();
            return ActionResult.Success("weather.repaired", new Dictionary<string, double> { { "durability", _state.durability } });
        }

        public void Degrade(float durabilityAmount)
        {
            if (!_state.isInstalled) return;
            _state.durability = Math.Max(0f, _state.durability - Math.Max(0f, durabilityAmount));
            if (_state.durability < 25f && !_state.hasSensorFault)
            {
                _state.hasSensorFault = true;
                _state.faultReason = "Anemometer bearing siezed and barometer drift";
            }
            OnStationStateChanged?.Invoke();
        }

        public void TriggerSensorFault(string reason)
        {
            _state.hasSensorFault = true;
            _state.faultReason = reason ?? "Sensor telemetry desynchronized";
            OnStationStateChanged?.Invoke();
        }

        public void ClearSensorFault()
        {
            _state.hasSensorFault = false;
            _state.faultReason = string.Empty;
            OnStationStateChanged?.Invoke();
        }

        public ActionResult GenerateForecast(int currentDay)
        {
            if (!IsOperational)
            {
                _state.cachedForecast.Clear();
                OnForecastUpdated?.Invoke();
                return ActionResult.Blocked("not_operational", "weather.not_operational");
            }

            _state.cachedForecast.Clear();
            _state.lastForecastDay = currentDay;
            int horizon = EffectiveHorizonDays;

            var rawForecast = _weatherSystem.PeekForecast(horizon);
            for (int i = 0; i < rawForecast.Count && i < horizon; i++)
            {
                var f = rawForecast[i];
                float tierMultiplier = CurrentTier switch
                {
                    WeatherStationTier.Damaged => 0.40f,
                    WeatherStationTier.Functional => 0.75f,
                    WeatherStationTier.Calibrated => 0.95f,
                    _ => 0.20f
                };

                float confidence = Math.Max(0.1f, Math.Min(1f, _state.accuracy * tierMultiplier * (1f - i * 0.12f)));
                bool isRouteSafe = f.Kind switch
                {
                    WeatherKind.Clear or WeatherKind.Overcast or WeatherKind.Rain => true,
                    WeatherKind.FalloutStorm or WeatherKind.BlackRain or WeatherKind.Blizzard => false,
                    _ => confidence > 0.5f
                };

                _state.cachedForecast.Add(new ForecastEntry
                {
                    day = f.Day,
                    weather = f.Kind,
                    confidence = confidence,
                    isRouteSafe = isRouteSafe,
                    temperature = 5f + WeatherSystem.TemperaturePenaltyForWeather(f.Kind),
                    warning = (!isRouteSafe && confidence > 0.35f)
                        ? $"WARNING: {f.Kind} expected — overland travel dangerous"
                        : string.Empty,
                    preparationPayoff = GetPreparationPayoff(f.Kind),
                    atmosphericFlavor = GetAtmosphericFlavor(f.Kind, currentDay + i)
                });
            }

            OnForecastUpdated?.Invoke();
            return ActionResult.Success("weather.forecast_generated",
                new Dictionary<string, double> { { "days", horizon }, { "tier", (int)CurrentTier } });
        }

        public static string GetPreparationPayoff(WeatherKind kind)
        {
            return kind switch
            {
                WeatherKind.FalloutStorm => "Action: Stock carbon air filters, seal exterior ventilation grilles, and recall surface expeditions.",
                WeatherKind.BlackRain => "Action: Seal rainwater cisterns, prepare chemical neutralizing wash, equip full hazmat suits.",
                WeatherKind.Blizzard => "Action: Stoke central heating furnace, allocate emergency diesel fuel, brace greenhouse exterior frames.",
                WeatherKind.Ashfall => "Action: Pre-clean intake louvres, service oxygen scrubbers, minimize outdoor worker shifts.",
                WeatherKind.BlackSnow => "Action: Prepare de-icing salt, shovel shelter roof cells, monitor perimeter radiation buildup.",
                WeatherKind.Rain => "Action: Open rainwater catchment lines, inspect drainage sump pumps.",
                WeatherKind.Overcast => "Action: Standard operations. Stable overland travel window.",
                WeatherKind.Clear => "Action: Prime solar exposure. Excellent overland scouting and long-range caravan window.",
                _ => "Standard survival protocol."
            };
        }

        public static string GetAtmosphericFlavor(WeatherKind kind, int day)
        {
            return kind switch
            {
                WeatherKind.FalloutStorm => "Ionized sky glows dull crimson; static hiss crackles across the station mast.",
                WeatherKind.BlackRain => "Oily dark precipitation condenses on sensor glass, smelling of sulfur and wet slag.",
                WeatherKind.Blizzard => "Sub-zero gale screams down the river gorge; ice needles glaze the external anemometer.",
                WeatherKind.Ashfall => "Pale powdery fallout drifts silently across the permafrost, muffling all surface noise.",
                WeatherKind.BlackSnow => "Charcoal-grey snow flakes coat the roof slabs in a heavy, radioactive blanket.",
                WeatherKind.Rain => "Cold persistent drizzle beats against the surface concrete, washing away grey ash.",
                WeatherKind.Overcast => "A low leaden ceiling of dense stratus clouds presses down upon the horizon.",
                WeatherKind.Clear => "Thin arctic sunlight cuts through the haze, illuminating jagged ruins against blue sky.",
                _ => "Atmospheric readings nominal."
            };
        }

        public IReadOnlyList<ForecastEntry> GetForecast() => _state.cachedForecast.AsReadOnly();

        public bool IsRouteSafe(int day)
        {
            foreach (var e in _state.cachedForecast)
                if (e.day == day) return e.isRouteSafe;
            return false;
        }

        public float GetConfidence(int day)
        {
            foreach (var e in _state.cachedForecast)
                if (e.day == day) return e.confidence;
            return 0f;
        }

        public WeatherStationState CaptureState() => CloneState(_state);

        public void RestoreState(WeatherStationState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static WeatherStationState CloneState(WeatherStationState src)
        {
            if (src == null) return new WeatherStationState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<WeatherStationState>(json) ?? new WeatherStationState();
        }
    }
}
