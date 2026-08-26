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
        public float confidence = 0f;          // 0..1
        public int observationCount = 0;
    }

    /// <summary>Triangulation state (save DTO).</summary>
    [Serializable]
    public class TriangulationState
    {
        public string systemId = SignalTriangulationSystem.SystemId;
        public List<RadioObservation> observations = new List<RadioObservation>();
        public List<TriangulationCandidate> candidates = new List<TriangulationCandidate>();
        public List<string> discoveredLocationIds = new List<string>();
        public string activeSignalId = string.Empty;
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
    /// </summary>
    public class SignalTriangulationSystem
    {
        public const string SystemId = "signal_triangulation_system";
        public const int MinObservationsForHypothesis = 2;
        public const int MinObservationsForDiscovery = 3;
        public const float ConfidenceThreshold = 0.7f;
        public const float BaseUncertaintyKm = 50f;
        public const float ObservationUncertaintyReduction = 0.4f; // each good obs reduces uncertainty
        public const float WeatherNoisePenalty = 0.15f;
        public const float MaxBearingErrorDegrees = 15f;

        private readonly TriangulationState _state = new TriangulationState();
        private readonly Dictionary<string, RadioObservation> _observationsBySignal = new Dictionary<string, RadioObservation>();

        // Events
        public event Action<string> OnFrequencyLocked;           // signalId
        public event Action<RadioObservation> OnObservationRecorded;
        public event Action<string> OnAntennaCalibrationChanged; // stationId
        public event Action<TriangulationCandidate> OnCandidateChanged;
        public event Action<string> OnTriangulationCompleted;    // locationId
        public event Action<string> OnTriangulationFailed;       // signalId
        public event Action<string> OnLocationRevealed;          // locationId
        public event Action<TriangulationState> OnStateChanged;

        public TriangulationState State => _state;
        public IReadOnlyList<RadioObservation> Observations => _state.observations;
        public IReadOnlyList<TriangulationCandidate> Candidates => _state.candidates;
        public IReadOnlyList<string> DiscoveredLocations => _state.discoveredLocationIds;

        public SignalTriangulationSystem()
        {
        }

        // ── Observation ──────────────────────────────────────────────

        /// <summary>Record a directional observation of a radio signal.</summary>
        public bool RecordObservation(RadioObservation obs)
        {
            if (obs == null || string.IsNullOrEmpty(obs.signalId)) return false;
            if (obs.bearingDegrees < 0f || obs.bearingDegrees >= 360f) return false;
            if (obs.errorDegrees <= 0f || obs.errorDegrees > MaxBearingErrorDegrees) return false;

            _state.observations.Add(obs);

            // Track latest observation per signal
            _observationsBySignal[obs.signalId] = obs;

            OnObservationRecorded?.Invoke(obs);
            RaiseChanged();
            return true;
        }

        // ── Triangulation ────────────────────────────────────────────

        /// <summary>
        /// Attempt to triangulate a signal from its observations.
        /// Returns a candidate if enough observations exist, null otherwise.
        /// </summary>
        public TriangulationCandidate? Triangulate(string signalId, ISeededRng rng)
        {
            if (string.IsNullOrEmpty(signalId)) return null;

            // Collect observations for this signal
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

            // Calculate intersection confidence
            float confidence = CalculateConfidence(signalObs);
            float uncertainty = CalculateUncertainty(signalObs);

            // Estimate position from bearing intersection
            var (estX, estY) = EstimatePosition(signalObs);

            // Create or update candidate
            var candidate = new TriangulationCandidate
            {
                locationId = "triangulated_" + signalId,
                displayName = "Triangulated Signal " + signalId,
                estimatedX = estX,
                estimatedY = estY,
                uncertaintyRadiusKm = uncertainty,
                confidence = confidence,
                observationCount = signalObs.Count
            };

            // Update or add candidate
            bool found = false;
            for (int i = 0; i < _state.candidates.Count; i++)
            {
                if (_state.candidates[i].locationId == candidate.locationId)
                {
                    _state.candidates[i] = candidate;
                    found = true;
                    break;
                }
            }
            if (!_state.candidates.Contains(candidate))
                _state.candidates.Add(candidate);

            OnCandidateChanged?.Invoke(candidate);

            // Check if discovery threshold is met
            if (confidence >= ConfidenceThreshold && signalObs.Count >= MinObservationsForDiscovery)
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

        // ── Confidence calculation ───────────────────────────────────

        private float CalculateConfidence(List<RadioObservation> obs)
        {
            if (obs.Count == 0) return 0f;

            float totalConfidence = 0f;
            foreach (var o in obs)
            {
                // Base confidence from signal strength
                float obsConfidence = o.signalStrength;

                // Reduce by noise
                obsConfidence *= (1f - o.noiseLevel * 0.5f);

                // Reduce by bearing error (wider error = less confidence)
                float errorFactor = 1f - (o.errorDegrees / MaxBearingErrorDegrees);
                obsConfidence *= errorFactor;

                // Weather penalty
                if (o.weatherCondition == "FalloutStorm" || o.weatherCondition == "Blizzard")
                    obsConfidence *= (1f - WeatherNoisePenalty);

                // Operator skill bonus
                obsConfidence *= (0.5f + o.operatorSkill * 0.5f);

                totalConfidence += Math.Max(0f, obsConfidence);
            }

            // Average confidence, with bonus for more observations
            float avgConfidence = totalConfidence / obs.Count;
            float observationBonus = Math.Min(0.2f, (obs.Count - MinObservationsForHypothesis) * 0.05f);

            return Math.Clamp(avgConfidence + observationBonus, 0f, 1f);
        }

        // ── Uncertainty calculation ──────────────────────────────────

        private float CalculateUncertainty(List<RadioObservation> obs)
        {
            if (obs.Count == 0) return BaseUncertaintyKm;

            // Base uncertainty decreases with more observations
            float uncertainty = BaseUncertaintyKm * (1f - Math.Min(1f, obs.Count * ObservationUncertaintyReduction));

            // Increase by average bearing error
            float avgError = 0f;
            foreach (var o in obs) avgError += o.errorDegrees;
            avgError /= obs.Count;
            uncertainty *= (1f + avgError / MaxBearingErrorDegrees);

            // Increase by noise
            float avgNoise = 0f;
            foreach (var o in obs) avgNoise += o.noiseLevel;
            avgNoise /= obs.Count;
            uncertainty *= (1f + avgNoise * 0.5f);

            return Math.Max(1f, uncertainty);
        }

        // ── Position estimation ──────────────────────────────────────

        /// <summary>
        /// Estimate position from bearing intersection using weighted centroid.
        /// Each observation defines a ray from its station; the intersection
        /// region is approximated by the weighted centroid of closest approaches.
        /// </summary>
        private (float x, float y) EstimatePosition(List<RadioObservation> obs)
        {
            if (obs.Count == 0) return (0f, 0f);
            if (obs.Count == 1)
            {
                // Single observation: estimate along the bearing
                var o = obs[0];
                float rad = DegreesToRadians(o.bearingDegrees);
                return ((float)Math.Cos(rad) * 25f, (float)Math.Sin(rad) * 25f);
            }

            // Multiple observations: weighted centroid of ray intersections
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
                        // Weight by signal strength and inverse noise
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

            // Fallback: centroid of observation directions
            float cx = 0f, cy = 0f;
            foreach (var o in obs)
            {
                float rad = DegreesToRadians(o.bearingDegrees);
                cx += (float)Math.Cos(rad);
                cy += (float)Math.Sin(rad);
            }
            return (cx / obs.Count * 25f, cy / obs.Count * 25f);
        }

        /// <summary>Intersect two bearing rays. Returns (NaN, NaN) if parallel.</summary>
        private (float x, float y) IntersectRays(RadioObservation a, RadioObservation b)
        {
            // Each ray: origin + direction * t
            // For simplicity, assume stations at origin (0,0) and (10,0)
            float ax = 0f, ay = 0f;
            float bx = 10f, by = 0f;

            float radA = DegreesToRadians(a.bearingDegrees);
            float radB = DegreesToRadians(b.bearingDegrees);

            float dax = (float)Math.Cos(radA);
            float day = (float)Math.Sin(radA);
            float dbx = (float)Math.Cos(radB);
            float dby = (float)Math.Sin(radB);

            // Solve: ax + dax*t = bx + dbx*s, ay + day*t = by + dby*s
            float denom = dax * dby - day * dbx;
            if (Math.Abs(denom) < 1e-6f) return (float.NaN, float.NaN); // parallel

            float s = (dax * (ay - by) - day * (ax - bx)) / denom;
            float ix = bx + dbx * s;
            float iy = by + dby * s;

            return (ix, iy);
        }

        private static float DegreesToRadians(float degrees) => degrees * (float)(Math.PI / 180.0);

        // ── Queries ──────────────────────────────────────────────────

        /// <summary>Check if a location has been discovered through triangulation.</summary>
        public bool IsLocationDiscovered(string locationId)
        {
            return _state.discoveredLocationIds.Contains(locationId);
        }

        /// <summary>Get the candidate for a signal, if any.</summary>
        public TriangulationCandidate? GetCandidate(string signalId)
        {
            string candidateId = "triangulated_" + signalId;
            foreach (var c in _state.candidates)
            {
                if (c.locationId == candidateId) return c;
            }
            return null;
        }

        /// <summary>Get observation count for a signal.</summary>
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
            var copy = new TriangulationState
            {
                systemId = _state.systemId,
                activeSignalId = _state.activeSignalId
            };
            // Ordinal-ordered copies
            foreach (var obs in _state.observations)
            {
                copy.observations.Add(new RadioObservation
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
                    weatherCondition = obs.weatherCondition,
                    operatorSkill = obs.operatorSkill
                });
            }
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
                    observationCount = c.observationCount
                });
            }
            foreach (var id in _state.discoveredLocationIds)
                copy.discoveredLocationIds.Add(id);
            return copy;
        }

        public void RestoreState(TriangulationState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.activeSignalId = saved.activeSignalId ?? string.Empty;
            _state.observations.Clear();
            _state.candidates.Clear();
            _state.discoveredLocationIds.Clear();
            _observationsBySignal.Clear();

            if (saved.observations != null)
            {
                foreach (var obs in saved.observations)
                {
                    if (obs == null || string.IsNullOrEmpty(obs.signalId)) continue;
                    _state.observations.Add(obs);
                    _observationsBySignal[obs.signalId] = obs;
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
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
