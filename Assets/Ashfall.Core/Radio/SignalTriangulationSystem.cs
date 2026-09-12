// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Radio
{
    // ── Observation ─────────────────────────────────────────────────

    /// <summary>One directional radio observation.</summary>
    [Serializable]
    public class RadioObservation
    {
        public string signalId = string.Empty;
        public string stationId = string.Empty;
        public int day = 0;
        public float hour = 0f;
        public float bearingDegrees = 0f;      // 0-360, clockwise from north
        public float errorDegrees = 5f;        // ± uncertainty in bearing
        public float signalStrength = 0f;      // 0..1
        public float noiseLevel = 0f;          // 0..1
        public float frequencyMhz = 0f;
        public string weatherCondition = "Clear";
        public float operatorSkill = 0.5f;     // 0..1
        /// <summary>Optional polarization fade factor 0..1 (higher = worse).</summary>
        public float polarizationFade = 0f;
    }

    /// <summary>A candidate location from triangulation.</summary>
    [Serializable]
    public class TriangulationCandidate
    {
        public string locationId = string.Empty;
        public string displayName = string.Empty;
        public float estimatedX = 0f;
        public float estimatedY = 0f;
        public float uncertaintyRadiusKm = 0f;
        public float confidence = 0f;          // 0..1 location confidence
        /// <summary>Identity confidence from fingerprint match (separate from location).</summary>
        public float identityConfidence = 0f;
        public int observationCount = 0;
        public bool isFalseSignature = false;
    }

    /// <summary>Registered DF station baseline coordinates (km).</summary>
    [Serializable]
    public class StationBaselineEntry
    {
        public string stationId = string.Empty;
        public float xKm = 0f;
        public float yKm = 0f;
        public string arrayId = string.Empty;
    }

    /// <summary>Triangulation state (save DTO).</summary>
    [Serializable]
    public class TriangulationState
    {
        public string systemId = SignalTriangulationSystem.SystemId;
        public List<RadioObservation> observations = new List<RadioObservation>();
        public List<TriangulationCandidate> candidates = new List<TriangulationCandidate>();
        public List<string> discoveredLocationIds = new List<string>();
        public List<StationBaselineEntry> stationBaselines = new List<StationBaselineEntry>();
        public string activeSignalId = string.Empty;
        public int lastCalibrationDay = 0;
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL — Radio Direction Finding and Triangulation system.
    /// Players collect multiple directional observations of a radio signal,
    /// account for antenna/weather/noise quality, and derive a bounded
    /// location hypothesis. A canonical location becomes discoverable only
    /// when enough evidence meets the configured confidence threshold.
    ///
    /// Determinism: bearing intersection uses stable angle math.
    /// Same observations in same order = same candidate set.
    /// Plan B88: station baselines, skywave/polarization modifiers,
    /// fingerprint identity confidence, and catalog-driven array profiles.
    /// </summary>
    public class SignalTriangulationSystem
    {
        public const string SystemId = "signal_triangulation_system";
        public const int MinObservationsForHypothesis = 2;
        public const int MinObservationsForDiscovery = 3;
        public const float ConfidenceThreshold = 0.7f;
        public const float BaseUncertaintyKm = 50f;
        public const float ObservationUncertaintyReduction = 0.4f;
        public const float WeatherNoisePenalty = 0.15f;
        public const float MaxBearingErrorDegrees = 15f;
        public const float SkywaveDefaultUncertaintyMult = 1.55f;
        public const float PolarizationUncertaintyMult = 1.25f;

        private readonly TriangulationState _state = new TriangulationState();
        private readonly Dictionary<string, RadioObservation> _observationsBySignal =
            new Dictionary<string, RadioObservation>(StringComparer.Ordinal);
        private readonly Dictionary<string, StationBaselineEntry> _baselines =
            new Dictionary<string, StationBaselineEntry>(StringComparer.Ordinal);
        private DirectionFindingCatalog? _catalog;

        // Events
        public event Action<string> OnFrequencyLocked;
        public event Action<RadioObservation> OnObservationRecorded;
        public event Action<string> OnAntennaCalibrationChanged;
        public event Action<TriangulationCandidate> OnCandidateChanged;
        public event Action<string> OnTriangulationCompleted;
        public event Action<string> OnTriangulationFailed;
        public event Action<string> OnLocationRevealed;
        public event Action<TriangulationState> OnStateChanged;

        public TriangulationState State => _state;
        public IReadOnlyList<RadioObservation> Observations => _state.observations;
        public IReadOnlyList<TriangulationCandidate> Candidates => _state.candidates;
        public IReadOnlyList<string> DiscoveredLocations => _state.discoveredLocationIds;
        public IReadOnlyDictionary<string, StationBaselineEntry> StationBaselines => _baselines;
        public DirectionFindingCatalog? Catalog => _catalog;

        public SignalTriangulationSystem()
        {
        }

        // ── Catalog / baselines ──────────────────────────────────────

        /// <summary>Load HF/DF array catalog and register authored station baselines.</summary>
        public void LoadCatalog(DirectionFindingCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            foreach (var array in catalog.Arrays.Values)
            {
                RegisterStationBaseline(array.station_id, array.baseline_x_km, array.baseline_y_km, array.array_id);
            }
            RaiseChanged();
        }

        public void LoadCatalog(DirectionFindingCatalogDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            DirectionFindingCatalogLoader.Validate(dto);
            LoadCatalog(DirectionFindingCatalogLoader.Build(dto));
        }

        public void RegisterStationBaseline(string stationId, float xKm, float yKm, string arrayId = "")
        {
            if (string.IsNullOrEmpty(stationId)) return;
            var entry = new StationBaselineEntry
            {
                stationId = stationId,
                xKm = xKm,
                yKm = yKm,
                arrayId = arrayId ?? string.Empty
            };
            _baselines[stationId] = entry;
            SyncBaselineList();
            RaiseChanged();
        }

        public bool TryGetStationBaseline(string stationId, out StationBaselineEntry entry)
        {
            entry = null!;
            if (string.IsNullOrEmpty(stationId)) return false;
            return _baselines.TryGetValue(stationId, out entry!);
        }

        public void NotifyAntennaCalibration(string stationId, int day = 0)
        {
            if (string.IsNullOrEmpty(stationId)) return;
            if (day > 0) _state.lastCalibrationDay = day;
            OnAntennaCalibrationChanged?.Invoke(stationId);
            RaiseChanged();
        }

        // ── Observation ──────────────────────────────────────────────

        /// <summary>Record a directional observation of a radio signal.</summary>
        public bool RecordObservation(RadioObservation obs) => RecordObservation(obs, rng: null);

        /// <summary>
        /// Record an observation, optionally applying deterministic skywave jitter
        /// and false-signature rolls from the DF catalog / RNG streams.
        /// </summary>
        public bool RecordObservation(RadioObservation obs, ISeededRng? rng)
        {
            if (obs == null || string.IsNullOrEmpty(obs.signalId)) return false;
            if (obs.bearingDegrees < 0f || obs.bearingDegrees >= 360f) return false;
            if (obs.errorDegrees <= 0f || obs.errorDegrees > MaxBearingErrorDegrees) return false;

            var stored = CloneObservation(obs);
            ApplyAtmosphereModifiers(stored, rng);

            // Re-clamp after skywave jitter (may push error up to MaxBearingErrorDegrees).
            if (stored.errorDegrees <= 0f) stored.errorDegrees = 0.5f;
            if (stored.errorDegrees > MaxBearingErrorDegrees) stored.errorDegrees = MaxBearingErrorDegrees;

            _state.observations.Add(stored);
            _observationsBySignal[stored.signalId] = stored;

            OnObservationRecorded?.Invoke(stored);
            OnFrequencyLocked?.Invoke(stored.signalId);
            RaiseChanged();
            return true;
        }

        // ── Triangulation ────────────────────────────────────────────

        public TriangulationCandidate? Triangulate(string signalId, ISeededRng rng)
        {
            if (string.IsNullOrEmpty(signalId)) return null;

            var signalObs = new List<RadioObservation>();
            foreach (var obs in _state.observations)
            {
                if (obs.signalId == signalId)
                    signalObs.Add(obs);
            }

            if (signalObs.Count < MinObservationsForHypothesis)
            {
                OnTriangulationFailed?.Invoke(signalId);
                return null;
            }

            float confidence = CalculateConfidence(signalObs);
            float uncertainty = CalculateUncertainty(signalObs);
            var (estX, estY) = EstimatePosition(signalObs);

            float identityConfidence = 0f;
            string locationId = "triangulated_" + signalId;
            string displayName = "Triangulated Signal " + signalId;
            bool falseSignature = false;

            var fingerprint = _catalog?.GetFingerprintForSignal(signalId);
            if (fingerprint != null)
            {
                identityConfidence = Math.Clamp(fingerprint.identity_confidence, 0f, 1f);
                if (!string.IsNullOrEmpty(fingerprint.display_name))
                    displayName = fingerprint.display_name;
                // Mapped location is only used once location confidence clears threshold.
                if (!string.IsNullOrEmpty(fingerprint.mapped_location_id)
                    && confidence >= ConfidenceThreshold
                    && signalObs.Count >= MinObservationsForDiscovery)
                {
                    locationId = fingerprint.mapped_location_id;
                }
            }

            // Deterministic false-signature check under severe skywave (identity spoof).
            if (rng != null && signalObs.Count > 0)
            {
                float maxFalseChance = 0f;
                foreach (var o in signalObs)
                {
                    var season = _catalog?.ResolveSkywave(o.weatherCondition);
                    if (season != null)
                        maxFalseChance = Math.Max(maxFalseChance, season.false_signature_chance);
                }
                if (maxFalseChance > 0f)
                {
                    float roll = rng.NextFloat();
                    if (roll < maxFalseChance)
                    {
                        falseSignature = true;
                        identityConfidence *= 0.35f;
                        confidence *= 0.5f;
                        uncertainty *= 1.4f;
                    }
                }
            }

            var candidate = new TriangulationCandidate
            {
                locationId = locationId,
                displayName = displayName,
                estimatedX = estX,
                estimatedY = estY,
                uncertaintyRadiusKm = uncertainty,
                confidence = confidence,
                identityConfidence = identityConfidence,
                observationCount = signalObs.Count,
                isFalseSignature = falseSignature
            };

            bool found = false;
            string hypothesisKey = "triangulated_" + signalId;
            for (int i = 0; i < _state.candidates.Count; i++)
            {
                var existing = _state.candidates[i];
                if (existing.locationId == candidate.locationId
                    || existing.locationId == hypothesisKey
                    || (fingerprint != null
                        && !string.IsNullOrEmpty(fingerprint.mapped_location_id)
                        && existing.locationId == fingerprint.mapped_location_id))
                {
                    _state.candidates[i] = candidate;
                    found = true;
                    break;
                }
            }
            if (!found)
                _state.candidates.Add(candidate);

            OnCandidateChanged?.Invoke(candidate);

            if (!falseSignature
                && confidence >= ConfidenceThreshold
                && signalObs.Count >= MinObservationsForDiscovery)
            {
                if (!_state.discoveredLocationIds.Contains(candidate.locationId))
                {
                    _state.discoveredLocationIds.Add(candidate.locationId);
                    OnLocationRevealed?.Invoke(candidate.locationId);
                }
                OnTriangulationCompleted?.Invoke(candidate.locationId);
            }

            RaiseChanged();
            return candidate;
        }

        // ── Confidence / uncertainty ─────────────────────────────────

        private float CalculateConfidence(List<RadioObservation> obs)
        {
            if (obs.Count == 0) return 0f;

            float totalConfidence = 0f;
            foreach (var o in obs)
            {
                float obsConfidence = o.signalStrength;
                obsConfidence *= (1f - o.noiseLevel * 0.5f);

                float errorFactor = 1f - (o.errorDegrees / MaxBearingErrorDegrees);
                obsConfidence *= errorFactor;

                if (o.weatherCondition == "FalloutStorm" || o.weatherCondition == "Blizzard")
                    obsConfidence *= (1f - WeatherNoisePenalty);

                var season = _catalog?.ResolveSkywave(o.weatherCondition);
                if (season != null)
                    obsConfidence *= Math.Clamp(season.confidence_mult, 0.1f, 1.5f);
                else if (IsSkywaveWeather(o.weatherCondition))
                    obsConfidence *= 0.75f;

                if (o.polarizationFade > 0f)
                    obsConfidence *= (1f - Math.Clamp(o.polarizationFade, 0f, 1f) * 0.35f);

                obsConfidence *= (0.5f + o.operatorSkill * 0.5f);
                totalConfidence += Math.Max(0f, obsConfidence);
            }

            float avgConfidence = totalConfidence / obs.Count;
            float observationBonus = Math.Min(0.2f, (obs.Count - MinObservationsForHypothesis) * 0.05f);

            // Multi-baseline bonus: distinct registered stations improve fix quality.
            int distinctBaselines = CountDistinctRegisteredStations(obs);
            if (distinctBaselines >= 2)
                observationBonus += Math.Min(0.1f, (distinctBaselines - 1) * 0.05f);

            return Math.Clamp(avgConfidence + observationBonus, 0f, 1f);
        }

        private float CalculateUncertainty(List<RadioObservation> obs)
        {
            if (obs.Count == 0) return BaseUncertaintyKm;

            float uncertainty = BaseUncertaintyKm * (1f - Math.Min(1f, obs.Count * ObservationUncertaintyReduction));

            float avgError = 0f;
            float avgNoise = 0f;
            float skywaveMult = 1f;
            foreach (var o in obs)
            {
                avgError += o.errorDegrees;
                avgNoise += o.noiseLevel;
                var season = _catalog?.ResolveSkywave(o.weatherCondition);
                if (season != null)
                    skywaveMult = Math.Max(skywaveMult, season.uncertainty_mult);
                else if (IsSkywaveWeather(o.weatherCondition))
                    skywaveMult = Math.Max(skywaveMult, SkywaveDefaultUncertaintyMult);
                if (o.polarizationFade > 0.2f)
                    skywaveMult = Math.Max(skywaveMult, PolarizationUncertaintyMult);
            }
            avgError /= obs.Count;
            avgNoise /= obs.Count;
            uncertainty *= (1f + avgError / MaxBearingErrorDegrees);
            uncertainty *= (1f + avgNoise * 0.5f);
            uncertainty *= skywaveMult;

            // Longer registered baselines shrink the confidence region.
            int distinctBaselines = CountDistinctRegisteredStations(obs);
            if (distinctBaselines >= 2)
                uncertainty *= Math.Max(0.55f, 1f - (distinctBaselines - 1) * 0.12f);

            return Math.Max(1f, uncertainty);
        }

        // ── Position estimation ──────────────────────────────────────

        private (float x, float y) EstimatePosition(List<RadioObservation> obs)
        {
            if (obs.Count == 0) return (0f, 0f);
            if (obs.Count == 1)
            {
                var o = obs[0];
                var (ox, oy) = ResolveStationPosition(o.stationId);
                float rad = DegreesToRadians(o.bearingDegrees);
                return (ox + (float)Math.Cos(rad) * 25f, oy + (float)Math.Sin(rad) * 25f);
            }

            float totalWeight = 0f;
            float weightedX = 0f;
            float weightedY = 0f;

            for (int i = 0; i < obs.Count; i++)
            {
                for (int j = i + 1; j < obs.Count; j++)
                {
                    var (ix, iy) = IntersectRays(obs[i], obs[j]);
                    if (!float.IsNaN(ix) && !float.IsNaN(iy))
                    {
                        float weight = (obs[i].signalStrength + obs[j].signalStrength) * 0.5f;
                        weight *= (1f - (obs[i].noiseLevel + obs[j].noiseLevel) * 0.25f);
                        weightedX += ix * weight;
                        weightedY += iy * weight;
                        totalWeight += weight;
                    }
                }
            }

            if (totalWeight > 0f)
                return (weightedX / totalWeight, weightedY / totalWeight);

            float cx = 0f, cy = 0f;
            foreach (var o in obs)
            {
                float rad = DegreesToRadians(o.bearingDegrees);
                cx += (float)Math.Cos(rad);
                cy += (float)Math.Sin(rad);
            }
            return (cx / obs.Count * 25f, cy / obs.Count * 25f);
        }

        private (float x, float y) IntersectRays(RadioObservation a, RadioObservation b)
        {
            var (ax, ay) = ResolveStationPosition(a.stationId);
            var (bx, by) = ResolveStationPosition(b.stationId);

            float radA = DegreesToRadians(a.bearingDegrees);
            float radB = DegreesToRadians(b.bearingDegrees);

            float dax = (float)Math.Cos(radA);
            float day = (float)Math.Sin(radA);
            float dbx = (float)Math.Cos(radB);
            float dby = (float)Math.Sin(radB);

            float denom = dax * dby - day * dbx;
            if (Math.Abs(denom) < 1e-6f) return (float.NaN, float.NaN);

            float s = (dax * (ay - by) - day * (ax - bx)) / denom;
            float ix = bx + dbx * s;
            float iy = by + dby * s;
            return (ix, iy);
        }

        /// <summary>
        /// Resolve station coordinates: registered baseline first, else a
        /// deterministic fallback ring so unregistered stations still differ.
        /// Legacy pair without baselines still yields a usable intersection.
        /// </summary>
        private (float x, float y) ResolveStationPosition(string stationId)
        {
            if (!string.IsNullOrEmpty(stationId) && _baselines.TryGetValue(stationId, out var entry))
                return (entry.xKm, entry.yKm);

            // Deterministic fallback ring (does not invent gameplay authority).
            int h = StableHash.Of(stationId ?? string.Empty);
            float angle = (Math.Abs(h) % 360) * (float)(Math.PI / 180.0);
            float radius = 5f + (Math.Abs(h / 360) % 20);
            return ((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius);
        }

        private static float DegreesToRadians(float degrees) => degrees * (float)(Math.PI / 180.0);

        private static bool IsSkywaveWeather(string weather) =>
            weather == "Night" || weather == "Skywave" || weather == "PolarizationFade";

        private int CountDistinctRegisteredStations(List<RadioObservation> obs)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var o in obs)
            {
                if (!string.IsNullOrEmpty(o.stationId) && _baselines.ContainsKey(o.stationId))
                    seen.Add(o.stationId);
            }
            return seen.Count;
        }

        private void ApplyAtmosphereModifiers(RadioObservation stored, ISeededRng? rng)
        {
            var season = _catalog?.ResolveSkywave(stored.weatherCondition);
            float uncMult = 1f;
            if (season != null)
                uncMult = Math.Max(1f, season.uncertainty_mult);
            else if (IsSkywaveWeather(stored.weatherCondition))
                uncMult = SkywaveDefaultUncertaintyMult;

            if (stored.polarizationFade > 0f)
                uncMult = Math.Max(uncMult, 1f + Math.Clamp(stored.polarizationFade, 0f, 1f) * 0.4f);

            if (uncMult > 1f)
                stored.errorDegrees = Math.Min(MaxBearingErrorDegrees, stored.errorDegrees * uncMult);

            // Catalog array base error floor.
            var array = _catalog?.GetArrayByStation(stored.stationId);
            if (array != null && stored.errorDegrees < array.base_bearing_error_deg)
                stored.errorDegrees = array.base_bearing_error_deg;

            if (rng != null && uncMult > 1.05f)
            {
                // Bounded deterministic jitter — never invents a new bearing class.
                float jitter = (rng.NextFloat() - 0.5f) * 2f * Math.Min(2.5f, stored.errorDegrees * 0.15f);
                stored.bearingDegrees = (stored.bearingDegrees + jitter + 360f) % 360f;
            }
        }

        private static RadioObservation CloneObservation(RadioObservation obs) => new RadioObservation
        {
            signalId = obs.signalId,
            stationId = obs.stationId,
            day = obs.day,
            hour = obs.hour,
            bearingDegrees = obs.bearingDegrees,
            errorDegrees = obs.errorDegrees,
            signalStrength = obs.signalStrength,
            noiseLevel = obs.noiseLevel,
            frequencyMhz = obs.frequencyMhz,
            weatherCondition = obs.weatherCondition ?? "Clear",
            operatorSkill = obs.operatorSkill,
            polarizationFade = obs.polarizationFade
        };

        // ── Queries ──────────────────────────────────────────────────

        public bool IsLocationDiscovered(string locationId) =>
            _state.discoveredLocationIds.Contains(locationId);

        public TriangulationCandidate? GetCandidate(string signalId)
        {
            string hypothesisKey = "triangulated_" + signalId;
            var fingerprint = _catalog?.GetFingerprintForSignal(signalId);
            foreach (var c in _state.candidates)
            {
                if (c.locationId == hypothesisKey) return c;
                if (fingerprint != null
                    && !string.IsNullOrEmpty(fingerprint.mapped_location_id)
                    && c.locationId == fingerprint.mapped_location_id)
                    return c;
            }
            return null;
        }

        public int GetObservationCount(string signalId)
        {
            int count = 0;
            foreach (var obs in _state.observations)
            {
                if (obs.signalId == signalId) count++;
            }
            return count;
        }

        // ── Save / Load ──────────────────────────────────────────────

        public TriangulationState CaptureState()
        {
            SyncBaselineList();
            var copy = new TriangulationState
            {
                systemId = _state.systemId,
                activeSignalId = _state.activeSignalId,
                lastCalibrationDay = _state.lastCalibrationDay
            };
            foreach (var obs in _state.observations)
                copy.observations.Add(CloneObservation(obs));
            foreach (var c in _state.candidates)
            {
                copy.candidates.Add(new TriangulationCandidate
                {
                    locationId = c.locationId,
                    displayName = c.displayName,
                    estimatedX = c.estimatedX,
                    estimatedY = c.estimatedY,
                    uncertaintyRadiusKm = c.uncertaintyRadiusKm,
                    confidence = c.confidence,
                    identityConfidence = c.identityConfidence,
                    observationCount = c.observationCount,
                    isFalseSignature = c.isFalseSignature
                });
            }
            foreach (var id in _state.discoveredLocationIds)
                copy.discoveredLocationIds.Add(id);
            foreach (var b in _state.stationBaselines)
            {
                copy.stationBaselines.Add(new StationBaselineEntry
                {
                    stationId = b.stationId,
                    xKm = b.xKm,
                    yKm = b.yKm,
                    arrayId = b.arrayId
                });
            }
            return copy;
        }

        public void RestoreState(TriangulationState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.activeSignalId = saved.activeSignalId ?? string.Empty;
            _state.lastCalibrationDay = saved.lastCalibrationDay;
            _state.observations.Clear();
            _state.candidates.Clear();
            _state.discoveredLocationIds.Clear();
            _state.stationBaselines.Clear();
            _observationsBySignal.Clear();
            _baselines.Clear();

            if (saved.observations != null)
            {
                foreach (var obs in saved.observations)
                {
                    if (obs == null || string.IsNullOrEmpty(obs.signalId)) continue;
                    var clone = CloneObservation(obs);
                    _state.observations.Add(clone);
                    _observationsBySignal[clone.signalId] = clone;
                }
            }
            if (saved.candidates != null)
            {
                foreach (var c in saved.candidates)
                {
                    if (c == null || string.IsNullOrEmpty(c.locationId)) continue;
                    _state.candidates.Add(c);
                }
            }
            if (saved.discoveredLocationIds != null)
            {
                foreach (var id in saved.discoveredLocationIds)
                {
                    if (!string.IsNullOrEmpty(id))
                        _state.discoveredLocationIds.Add(id);
                }
            }
            if (saved.stationBaselines != null)
            {
                foreach (var b in saved.stationBaselines)
                {
                    if (b == null || string.IsNullOrEmpty(b.stationId)) continue;
                    RegisterStationBaseline(b.stationId, b.xKm, b.yKm, b.arrayId);
                }
            }

            // Re-apply catalog baselines after restore (catalog wins for geometry).
            if (_catalog != null)
            {
                foreach (var array in _catalog.Arrays.Values)
                    RegisterStationBaseline(array.station_id, array.baseline_x_km, array.baseline_y_km, array.array_id);
            }

            RaiseChanged();
        }

        private void SyncBaselineList()
        {
            _state.stationBaselines.Clear();
            var keys = new List<string>(_baselines.Keys);
            keys.Sort(StringComparer.Ordinal);
            foreach (var key in keys)
            {
                var b = _baselines[key];
                _state.stationBaselines.Add(new StationBaselineEntry
                {
                    stationId = b.stationId,
                    xKm = b.xKm,
                    yKm = b.yKm,
                    arrayId = b.arrayId
                });
            }
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
