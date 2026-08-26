using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.World
{
    // ── Sonde state ─────────────────────────────────────────────────

    /// <summary>One telemetry sample from a radiosonde.</summary>
    [Serializable]
    public class SondeTelemetrySample
    {
        public int sampleIndex = 0;
        public float altitudeKm = 0f;
        public float temperatureC = 0f;
        public float radiationMsv = 0f;
        public float windSpeedKmh = 0f;
        public float windDirectionDeg = 0f;
        public float humidityPct = 0f;
        public bool isLost = false;       // signal lost at this altitude
    }

    /// <summary>Forecast confidence entry from sonde data.</summary>
    [Serializable]
    public class SondeForecastEntry
    {
        public int dayOffset = 0;         // 0 = today, 1 = tomorrow, etc.
        public string predictedKind = "Clear";
        public float confidence = 0f;     // 0..1
        public float uncertaintyRadius = 0f; // weather variability
    }

    /// <summary>Radiosonde state (save DTO).</summary>
    [Serializable]
    public class WeatherSondeState
    {
        public string systemId = WeatherSondeSystem.SystemId;
        public string sondeId = string.Empty;
        public bool isLaunched = false;
        public bool isRecovered = false;
        public bool isFailed = false;
        public int launchDay = 0;
        public float launchHour = 0f;
        public int flightDurationTicks = 0;
        public int ticksElapsed = 0;
        public float batteryLevel = 1.0f;   // 0..1
        public float hydrogenLevel = 1.0f;  // 0..1, inflation gas
        public float sensorQuality = 1.0f;  // 0..1, degrades with altitude
        public float observationQuality = 0f; // 0..1, cumulative quality
        public List<SondeTelemetrySample> samples = new List<SondeTelemetrySample>();
        public List<SondeForecastEntry> forecast = new List<SondeForecastEntry>();
        public string failureReason = string.Empty;
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL — Weather Balloon and Atmospheric Radiosonde system.
    /// A launched sonde gathers additional weather observations and provides
    /// a longer, uncertain forecast window based on WeatherSystem state.
    /// It consumes launch materials, power, labor, and maintenance, and
    /// can fail or be lost in hazardous conditions.
    ///
    /// Key invariant: the sonde is an observation layer, NOT a weather oracle.
    /// It queries the existing WeatherSystem deterministic forecast and adds
    /// confidence/uncertainty. It does NOT mutate future weather.
    /// </summary>
    public class WeatherSondeSystem
    {
        public const string SystemId = "weather_sonde_system";

        // Flight constants
        public const int DefaultFlightDurationTicks = 4;
        public const int MaxFlightDurationTicks = 8;
        public const float BatteryDrainPerTick = 0.1f;
        public const float HydrogenDrainPerTick = 0.05f;
        public const float SensorDegradationPerKm = 0.02f;
        public const float MaxAltitudeKm = 30f;
        public const float AltitudePerTickKm = 7.5f;

        // Forecast constants
        public const int BaseForecastHorizonDays = 3;
        public const int ExtendedForecastHorizonDays = 5;
        public const float HighQualityThreshold = 0.7f;
        public const float SignalLossChancePerTick = 0.05f;

        // Resource costs
        public const float HydrogenCostPerLaunch = 0.5f;
        public const float BatteryCostPerLaunch = 0.3f;

        private readonly WeatherSondeState _state = new WeatherSondeState();
        private readonly WeatherSystem _weatherSystem;

        // Events
        public event Action<string> OnLaunchStarted;           // sondeId
        public event Action<SondeTelemetrySample> OnTelemetryReceived;
        public event Action<SondeTelemetrySample> OnTelemetryLost;
        public event Action<string> OnSondeFailed;             // reason
        public event Action<string> OnSondeRecovered;          // sondeId
        public event Action<List<SondeForecastEntry>> OnForecastConfidenceChanged;
        public event Action<WeatherSondeState> OnStateChanged;

        public WeatherSondeState State => _state;
        public bool IsLaunched => _state.isLaunched;
        public bool IsComplete => _state.isRecovered || _state.isFailed;

        public WeatherSondeSystem(WeatherSystem weatherSystem)
        {
            _weatherSystem = weatherSystem ?? throw new ArgumentNullException(nameof(weatherSystem));
        }

        // ── Launch ──────────────────────────────────────────────────

        /// <summary>Launch a radiosonde. Consumes hydrogen and battery.</summary>
        public bool Launch(string sondeId, int day, float hour, float hydrogenAvailable, float batteryAvailable)
        {
            if (_state.isLaunched && !IsComplete) return false;
            if (hydrogenAvailable < HydrogenCostPerLaunch) return false;
            if (batteryAvailable < BatteryCostPerLaunch) return false;

            _state.sondeId = sondeId ?? string.Empty;
            _state.isLaunched = true;
            _state.isRecovered = false;
            _state.isFailed = false;
            _state.launchDay = day;
            _state.launchHour = hour;
            _state.flightDurationTicks = DefaultFlightDurationTicks;
            _state.ticksElapsed = 0;
            _state.batteryLevel = 1.0f - BatteryCostPerLaunch;
            _state.hydrogenLevel = 1.0f - HydrogenCostPerLaunch;
            _state.sensorQuality = 1.0f;
            _state.observationQuality = 0f;
            _state.samples.Clear();
            _state.forecast.Clear();
            _state.failureReason = string.Empty;

            OnLaunchStarted?.Invoke(sondeId);
            RaiseChanged();
            return true;
        }

        // ── Tick ────────────────────────────────────────────────────

        /// <summary>
        /// Advance one flight tick. Samples telemetry, degrades battery/sensor,
        /// and checks for signal loss or failure.
        /// </summary>
        public bool Tick(ISeededRng rng)
        {
            if (!_state.isLaunched || IsComplete) return false;

            _state.ticksElapsed++;
            float altitude = _state.ticksElapsed * AltitudePerTickKm;

            // Battery drain
            _state.batteryLevel = Math.Max(0f, _state.batteryLevel - BatteryDrainPerTick);

            // Hydrogen drain
            _state.hydrogenLevel = Math.Max(0f, _state.hydrogenLevel - HydrogenDrainPerTick);

            // Sensor degradation at altitude
            _state.sensorQuality = Math.Max(0f, 1f - (altitude * SensorDegradationPerKm));

            // Check for signal loss
            bool signalLost = false;
            if (rng != null)
            {
                float lossChance = SignalLossChancePerTick;
                if (_state.batteryLevel < 0.2f) lossChance *= 2f;
                if (altitude > 20f) lossChance *= 1.5f;
                signalLost = rng.NextDouble() < lossChance;
            }

            // Sample telemetry
            var sample = new SondeTelemetrySample
            {
                sampleIndex = _state.ticksElapsed,
                altitudeKm = altitude,
                temperatureC = SampleTemperature(altitude, rng),
                radiationMsv = SampleRadiation(altitude, rng),
                windSpeedKmh = SampleWindSpeed(altitude, rng),
                windDirectionDeg = SampleWindDirection(altitude, rng),
                humidityPct = SampleHumidity(altitude, rng),
                isLost = signalLost
            };

            _state.samples.Add(sample);

            if (signalLost)
            {
                OnTelemetryLost?.Invoke(sample);
            }
            else
            {
                // Update observation quality
                float sampleQuality = _state.sensorQuality * (1f - altitude / MaxAltitudeKm * 0.3f);
                _state.observationQuality = Math.Min(1f,
                    _state.observationQuality + sampleQuality / _state.flightDurationTicks);
                OnTelemetryReceived?.Invoke(sample);
            }

            // Check for failure conditions
            if (_state.batteryLevel <= 0f)
            {
                _state.isFailed = true;
                _state.failureReason = "Battery depleted.";
                OnSondeFailed?.Invoke(_state.failureReason);
                RaiseChanged();
                return true;
            }

            if (_state.hydrogenLevel <= 0f && altitude < 10f)
            {
                _state.isFailed = true;
                _state.failureReason = "Insufficient hydrogen for ascent.";
                OnSondeFailed?.Invoke(_state.failureReason);
                RaiseChanged();
                return true;
            }

            // Check if flight is complete
            if (_state.ticksElapsed >= _state.flightDurationTicks)
            {
                _state.isRecovered = true;
                GenerateForecast();
                OnSondeRecovered?.Invoke(_state.sondeId);
                RaiseChanged();
                return true;
            }

            RaiseChanged();
            return true;
        }

        // ── Forecast generation ─────────────────────────────────────

        /// <summary>
        /// Generate a forecast from sonde data. Queries the existing
        /// WeatherSystem forecast and adds confidence based on observation quality.
        /// Does NOT mutate the WeatherSystem.
        /// </summary>
        private void GenerateForecast()
        {
            _state.forecast.Clear();

            // Determine forecast horizon based on quality
            int horizon = _state.observationQuality >= HighQualityThreshold
                ? ExtendedForecastHorizonDays
                : BaseForecastHorizonDays;

            // Query the existing WeatherSystem forecast
            var baseForecast = _weatherSystem.PeekForecast(horizon);

            for (int i = 0; i < baseForecast.Count; i++)
            {
                var entry = baseForecast[i];
                float confidence = CalculateConfidence(i, _state.observationQuality);
                float uncertainty = CalculateUncertainty(i, _state.observationQuality);

                _state.forecast.Add(new SondeForecastEntry
                {
                    dayOffset = i,
                    predictedKind = entry.Kind.ToString(),
                    confidence = confidence,
                    uncertaintyRadius = uncertainty
                });
            }

            OnForecastConfidenceChanged?.Invoke(_state.forecast);
        }

        private float CalculateConfidence(int dayOffset, float observationQuality)
        {
            // Confidence decreases with day offset
            float baseConfidence = observationQuality * 0.9f;
            float decay = dayOffset * 0.15f;
            return Math.Max(0.1f, baseConfidence - decay);
        }

        private float CalculateUncertainty(int dayOffset, float observationQuality)
        {
            // Uncertainty increases with day offset and decreases with quality
            float baseUncertainty = 0.3f + dayOffset * 0.2f;
            float qualityReduction = observationQuality * 0.3f;
            return Math.Max(0.1f, baseUncertainty - qualityReduction);
        }

        // ── Telemetry sampling (deterministic, uses WeatherSystem state) ──

        private float SampleTemperature(float altitudeKm, ISeededRng rng)
        {
            // Temperature decreases with altitude (standard lapse rate)
            float baseTemp = 15f; // ground temperature
            float lapseRate = 6.5f; // degrees per km
            float temp = baseTemp - altitudeKm * lapseRate;
            // Add noise
            if (rng != null) temp += (float)(rng.NextDouble() - 0.5) * 2f;
            return temp;
        }

        private float SampleRadiation(float altitudeKm, ISeededRng rng)
        {
            // Radiation increases with altitude (less shielding)
            float baseRad = 0.1f;
            float altitudeFactor = altitudeKm * 0.05f;
            float rad = baseRad + altitudeFactor;
            if (rng != null) rad += (float)(rng.NextDouble() - 0.5) * 0.02f;
            return Math.Max(0f, rad);
        }

        private float SampleWindSpeed(float altitudeKm, ISeededRng rng)
        {
            // Wind increases with altitude
            float baseWind = 10f;
            float wind = baseWind + altitudeKm * 3f;
            if (rng != null) wind += (float)(rng.NextDouble() - 0.5) * 5f;
            return Math.Max(0f, wind);
        }

        private float SampleWindDirection(float altitudeKm, ISeededRng rng)
        {
            // Random direction with some altitude correlation
            float dir = altitudeKm * 10f;
            if (rng != null) dir += (float)(rng.NextDouble() - 0.5) * 30f;
            return ((dir % 360f) + 360f) % 360f;
        }

        private float SampleHumidity(float altitudeKm, ISeededRng rng)
        {
            // Humidity decreases with altitude
            float baseHumidity = 60f;
            float humidity = baseHumidity - altitudeKm * 2f;
            if (rng != null) humidity += (float)(rng.NextDouble() - 0.5) * 10f;
            return Math.Clamp(humidity, 0f, 100f);
        }

        // ── Queries ──────────────────────────────────────────────────

        public int GetSampleCount() => _state.samples.Count;
        public int GetLostSampleCount()
        {
            int count = 0;
            foreach (var s in _state.samples) if (s.isLost) count++;
            return count;
        }

        public float GetCurrentAltitude()
        {
            if (!_state.isLaunched || _state.samples.Count == 0) return 0f;
            return _state.samples[_state.samples.Count - 1].altitudeKm;
        }

        public List<SondeForecastEntry> GetForecast() => new List<SondeForecastEntry>(_state.forecast);

        // ── Save / Load ──────────────────────────────────────────────

        public WeatherSondeState CaptureState()
        {
            var copy = new WeatherSondeState
            {
                systemId = _state.systemId,
                sondeId = _state.sondeId,
                isLaunched = _state.isLaunched,
                isRecovered = _state.isRecovered,
                isFailed = _state.isFailed,
                launchDay = _state.launchDay,
                launchHour = _state.launchHour,
                flightDurationTicks = _state.flightDurationTicks,
                ticksElapsed = _state.ticksElapsed,
                batteryLevel = _state.batteryLevel,
                hydrogenLevel = _state.hydrogenLevel,
                sensorQuality = _state.sensorQuality,
                observationQuality = _state.observationQuality,
                failureReason = _state.failureReason
            };
            foreach (var s in _state.samples)
            {
                copy.samples.Add(new SondeTelemetrySample
                {
                    sampleIndex = s.sampleIndex,
                    altitudeKm = s.altitudeKm,
                    temperatureC = s.temperatureC,
                    radiationMsv = s.radiationMsv,
                    windSpeedKmh = s.windSpeedKmh,
                    windDirectionDeg = s.windDirectionDeg,
                    humidityPct = s.humidityPct,
                    isLost = s.isLost
                });
            }
            foreach (var f in _state.forecast)
            {
                copy.forecast.Add(new SondeForecastEntry
                {
                    dayOffset = f.dayOffset,
                    predictedKind = f.predictedKind,
                    confidence = f.confidence,
                    uncertaintyRadius = f.uncertaintyRadius
                });
            }
            return copy;
        }

        public void RestoreState(WeatherSondeState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.sondeId = saved.sondeId ?? string.Empty;
            _state.isLaunched = saved.isLaunched;
            _state.isRecovered = saved.isRecovered;
            _state.isFailed = saved.isFailed;
            _state.launchDay = saved.launchDay;
            _state.launchHour = saved.launchHour;
            _state.flightDurationTicks = saved.flightDurationTicks;
            _state.ticksElapsed = saved.ticksElapsed;
            _state.batteryLevel = saved.batteryLevel;
            _state.hydrogenLevel = saved.hydrogenLevel;
            _state.sensorQuality = saved.sensorQuality;
            _state.observationQuality = saved.observationQuality;
            _state.failureReason = saved.failureReason ?? string.Empty;
            _state.samples.Clear();
            _state.forecast.Clear();
            if (saved.samples != null)
            {
                foreach (var s in saved.samples)
                    _state.samples.Add(s);
            }
            if (saved.forecast != null)
            {
                foreach (var f in saved.forecast)
                    _state.forecast.Add(f);
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
