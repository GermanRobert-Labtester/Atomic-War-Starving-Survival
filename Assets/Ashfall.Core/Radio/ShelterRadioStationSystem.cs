// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    [Serializable]
    public sealed class RadioEncryptionScheme
    {
        [JsonPropertyName("scheme")]
        public string Scheme { get; set; } = "none";

        [JsonPropertyName("difficulty")]
        public int Difficulty { get; set; }

        [JsonPropertyName("required_skill_ids")]
        public List<string> RequiredSkillIds { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class RadioTriangulationData
    {
        [JsonPropertyName("required_bearings")]
        public int RequiredBearings { get; set; } = 2;

        [JsonPropertyName("revealed_location_id")]
        public string RevealedLocationId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RadioInterceptDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("callsign")]
        public string Callsign { get; set; } = string.Empty;

        [JsonPropertyName("frequency_khz")]
        public int FrequencyKhz { get; set; } = 7000;

        [JsonPropertyName("band")]
        public string Band { get; set; } = "hf";

        [JsonPropertyName("signal_class")]
        public string SignalClass { get; set; } = "chatter";

        [JsonPropertyName("source_faction_id")]
        public string SourceFactionId { get; set; } = string.Empty;

        [JsonPropertyName("base_signal_strength")]
        public float BaseSignalStrength { get; set; } = 0.5f;

        [JsonPropertyName("encryption")]
        public RadioEncryptionScheme Encryption { get; set; } = new RadioEncryptionScheme();

        [JsonPropertyName("triangulation")]
        public RadioTriangulationData Triangulation { get; set; } = new RadioTriangulationData();

        [JsonPropertyName("expiry_days")]
        public int ExpiryDays { get; set; } = 3;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class RadioInterceptCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("intercepts")]
        public List<RadioInterceptDefinition> Intercepts { get; set; } = new List<RadioInterceptDefinition>();
    }

    [Serializable]
    public sealed class InterceptProgress
    {
        public string InterceptId { get; set; } = string.Empty;
        public bool Detected { get; set; }
        public int SignalLockPermille { get; set; }
        public int DecryptProgressPermille { get; set; }
        public int BearingsCollected { get; set; }
        public List<int> DistinctAzimuths { get; set; } = new List<int>();
        public bool Resolved { get; set; }
        public bool IsDecrypted { get; set; }
        public int DetectedDay { get; set; }
        public int? ExpiresOnDay { get; set; }
        public bool IsExpired { get; set; }
    }

    [Serializable]
    public sealed class RadioStationStateSave
    {
        public string systemId = ShelterRadioStationSystem.SystemId;
        public int schemaVersion = 1;
        public int tunedFrequencyKhz = 7115;
        public string bandId = "hf";
        public int antennaAzimuthDegrees = 0;
        public bool isOperational = true;
        public List<InterceptProgress> intercepts = new List<InterceptProgress>();
        public List<string> discoveredLocationIds = new List<string>();
        public List<string> decodedIntelligenceLogs = new List<string>();
        public int currentDay;
    }

    public sealed record RadioScanResult(
        bool FoundSignal,
        string InterceptId,
        float SignalStrength,
        float NoiseFloor,
        bool IsLocked,
        string StatusMessage);

    public sealed class ShelterRadioStationSystem
    {
        public const string SystemId = "radio_station";

        private RadioStationStateSave _state = new RadioStationStateSave();
        private readonly Dictionary<string, RadioInterceptDefinition> _catalog = new(StringComparer.Ordinal);
        private readonly OrbitalHarrowTelemetrySystem? _harrowTelemetry;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private Func<string, float>? _operatorSkillProvider; // skillId -> multiplier
        private Func<float>? _weatherNoiseProvider;

        public RadioStationStateSave State => _state;
        public IReadOnlyDictionary<string, RadioInterceptDefinition> Catalog => _catalog;

        public event Action<string>? OnInterceptDetected;
        public event Action<string>? OnInterceptDecrypted;
        public event Action<string, string>? OnLocationTriangulated; // interceptId, locationId
        public event Action<string>? OnDistressExpired;
        public event Action<OrbitalWarningEntry>? OnOrbitalWarningRelayed;
        public event Action? OnRadioStateChanged;

        public ShelterRadioStationSystem(
            ISeededRng rng,
            OrbitalHarrowTelemetrySystem? harrowTelemetry = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _harrowTelemetry = harrowTelemetry;
            _log = log ?? NullLog.Instance;
        }

        public void BindSkillProvider(Func<string, float> provider) => _operatorSkillProvider = provider;
        public void BindWeatherNoiseProvider(Func<float> provider) => _weatherNoiseProvider = provider;

        public void LoadCatalog(RadioInterceptCatalogData? data)
        {
            if (data?.Intercepts == null) return;
            _catalog.Clear();
            foreach (var intercept in data.Intercepts)
            {
                if (!string.IsNullOrEmpty(intercept.Id))
                    _catalog[intercept.Id] = intercept;
            }
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var serializer = new SystemTextJsonSerializer();
            var data = serializer.Deserialize<RadioInterceptCatalogData>(json);
            LoadCatalog(data);
        }

        public void TuneTo(int frequencyKhz, string band = "hf")
        {
            _state.tunedFrequencyKhz = Math.Max(0, frequencyKhz);
            if (!string.IsNullOrEmpty(band)) _state.bandId = band.ToLowerInvariant();
            OnRadioStateChanged?.Invoke();
        }

        public void SetAntennaAzimuth(int degrees)
        {
            _state.antennaAzimuthDegrees = ((degrees % 360) + 360) % 360;
            OnRadioStateChanged?.Invoke();
        }

        public InterceptProgress GetOrCreateInterceptProgress(string interceptId)
        {
            var existing = _state.intercepts.Find(i => i.InterceptId == interceptId);
            if (existing != null) return existing;

            var created = new InterceptProgress
            {
                InterceptId = interceptId,
                DetectedDay = _state.currentDay
            };

            _state.intercepts.Add(created);
            return created;
        }

        public RadioScanResult ScanFrequency(int day)
        {
            _state.currentDay = day;
            if (!_state.isOperational)
            {
                return new RadioScanResult(false, string.Empty, 0f, 1f, false, "radio_offline");
            }

            float weatherNoise = _weatherNoiseProvider != null ? _weatherNoiseProvider() : 0.15f;
            RadioInterceptDefinition? bestMatch = null;
            float bestClarity = 0f;

            foreach (var intercept in _catalog.Values)
            {
                var progress = _state.intercepts.Find(i => i.InterceptId == intercept.Id);
                if (progress != null && progress.IsExpired) continue;

                // Check frequency tolerance (+/- 15 kHz)
                int deltaKhz = Math.Abs(_state.tunedFrequencyKhz - intercept.FrequencyKhz);
                if (deltaKhz > 15) continue;

                float tuningMatch = 1.0f - (deltaKhz / 15.0f);
                float effectiveStrength = intercept.BaseSignalStrength * tuningMatch * (1.0f - weatherNoise);

                if (effectiveStrength > bestClarity)
                {
                    bestClarity = effectiveStrength;
                    bestMatch = intercept;
                }
            }

            if (bestMatch == null || bestClarity < 0.20f)
            {
                return new RadioScanResult(false, string.Empty, bestClarity, weatherNoise, false, "static_noise");
            }

            var activeProgress = GetOrCreateInterceptProgress(bestMatch.Id);
            if (!activeProgress.Detected)
            {
                activeProgress.Detected = true;
                activeProgress.DetectedDay = _state.currentDay;
                if (_catalog.TryGetValue(bestMatch.Id, out var def) && def.ExpiryDays > 0)
                {
                    activeProgress.ExpiresOnDay = _state.currentDay + def.ExpiryDays;
                }
                OnInterceptDetected?.Invoke(bestMatch.Id);
            }

            // Advance signal lock
            int lockGain = (int)Math.Round(bestClarity * 400f);
            activeProgress.SignalLockPermille = Math.Min(1000, activeProgress.SignalLockPermille + lockGain);

            bool isLocked = activeProgress.SignalLockPermille >= 800;
            OnRadioStateChanged?.Invoke();

            return new RadioScanResult(
                true,
                bestMatch.Id,
                bestClarity,
                weatherNoise,
                isLocked,
                isLocked ? "signal_locked" : "signal_detected");
        }

        public int ProgressDecryption(string interceptId, float skillBonus = 1.0f)
        {
            if (!_catalog.TryGetValue(interceptId, out var def)) return 0;
            var progress = GetOrCreateInterceptProgress(interceptId);
            if (progress.IsDecrypted || progress.IsExpired) return 0;

            if (!progress.Detected)
            {
                progress.Detected = true;
                progress.DetectedDay = _state.currentDay;
                if (def.ExpiryDays > 0 && !progress.ExpiresOnDay.HasValue)
                {
                    progress.ExpiresOnDay = _state.currentDay + def.ExpiryDays;
                }
            }

            if (def.Encryption.Difficulty <= 0 || def.Encryption.Scheme == "none")
            {
                progress.DecryptProgressPermille = 1000;
                progress.IsDecrypted = true;
                _state.decodedIntelligenceLogs.Add($"[{def.Callsign}] {def.Message}");
                OnInterceptDecrypted?.Invoke(interceptId);
                OnRadioStateChanged?.Invoke();
                return 1000;
            }

            float opSkill = 1.0f;
            if (_operatorSkillProvider != null)
            {
                foreach (var sk in def.Encryption.RequiredSkillIds)
                {
                    opSkill += _operatorSkillProvider(sk) * 0.5f;
                }
            }

            float baseRate = 250f / Math.Max(1, def.Encryption.Difficulty);
            int gain = (int)Math.Round(baseRate * opSkill * skillBonus * 100f);
            gain = Math.Max(25, gain);

            progress.DecryptProgressPermille = Math.Min(1000, progress.DecryptProgressPermille + gain);
            if (progress.DecryptProgressPermille >= 1000)
            {
                progress.IsDecrypted = true;
                _state.decodedIntelligenceLogs.Add($"[{def.Callsign}] {def.Message}");
                OnInterceptDecrypted?.Invoke(interceptId);
            }

            OnRadioStateChanged?.Invoke();
            return gain;
        }

        public bool RecordBearing(string interceptId, int azimuthDegrees)
        {
            if (!_catalog.TryGetValue(interceptId, out var def)) return false;
            var progress = GetOrCreateInterceptProgress(interceptId);
            if (progress.Resolved || progress.IsExpired) return false;

            if (!progress.Detected)
            {
                progress.Detected = true;
                progress.DetectedDay = _state.currentDay;
                if (def.ExpiryDays > 0 && !progress.ExpiresOnDay.HasValue)
                {
                    progress.ExpiresOnDay = _state.currentDay + def.ExpiryDays;
                }
            }

            int normAzimuth = ((azimuthDegrees % 360) + 360) % 360;

            // Check if this azimuth is sufficiently distinct (>= 20 degrees difference from existing)
            bool isDistinct = true;
            foreach (var az in progress.DistinctAzimuths)
            {
                int diff = Math.Abs(az - normAzimuth);
                if (diff > 180) diff = 360 - diff;
                if (diff < 20)
                {
                    isDistinct = false;
                    break;
                }
            }

            if (isDistinct)
            {
                progress.DistinctAzimuths.Add(normAzimuth);
                progress.BearingsCollected++;
            }

            if (progress.BearingsCollected >= def.Triangulation.RequiredBearings)
            {
                progress.Resolved = true;
                string locId = def.Triangulation.RevealedLocationId;
                if (!string.IsNullOrEmpty(locId) && !_state.discoveredLocationIds.Contains(locId))
                {
                    _state.discoveredLocationIds.Add(locId);
                    OnLocationTriangulated?.Invoke(interceptId, locId);
                }
                OnRadioStateChanged?.Invoke();
                return true;
            }

            OnRadioStateChanged?.Invoke();
            return false;
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;

            // Check SOS expiries
            foreach (var progress in _state.intercepts)
            {
                if (progress.Resolved || progress.IsExpired) continue;
                if (progress.ExpiresOnDay.HasValue && progress.ExpiresOnDay.Value <= day)
                {
                    progress.IsExpired = true;
                    OnDistressExpired?.Invoke(progress.InterceptId);
                }
            }

            // Check Orbital Early Warning
            CheckOrbitalEarlyWarning(day);

            OnRadioStateChanged?.Invoke();
        }

        public OrbitalWarningEntry? CheckOrbitalEarlyWarning(int currentDay)
        {
            if (_harrowTelemetry == null || !_state.isOperational) return null;

            if (_harrowTelemetry.HasPendingImpact && _harrowTelemetry.State.warnings.Count > 0)
            {
                var latest = _harrowTelemetry.State.warnings[^1];
                OnOrbitalWarningRelayed?.Invoke(latest);
                return latest;
            }

            return null;
        }

        public RadioStationStateSave CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<RadioStationStateSave>(json) ?? new RadioStationStateSave();
        }

        public void RestoreState(RadioStationStateSave? saved)
        {
            if (saved == null)
            {
                _state = new RadioStationStateSave();
                return;
            }

            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<RadioStationStateSave>(json) ?? new RadioStationStateSave();
            OnRadioStateChanged?.Invoke();
        }
    }
}
