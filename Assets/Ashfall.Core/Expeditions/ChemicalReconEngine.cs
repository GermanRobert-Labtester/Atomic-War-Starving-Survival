using System;
using System.Collections.Generic;
using System.IO;
#pragma warning disable CS8618

namespace Ashfall.Core.Expeditions
{
    // ─── Catalog types ───

    [Serializable]
    public sealed class ToxicChemicalCatalog
    {
        public int schema_version = 1;
        public List<ChemicalHazardProfile> hazard_profiles = new List<ChemicalHazardProfile>();
        public DetectorEquipmentDef detector_equipment = new DetectorEquipmentDef();
        public SampleCollectionDef sample_collection = new SampleCollectionDef();
        public FilterModelDef filter_model = new FilterModelDef();
        public MapOverlayDef map_overlay = new MapOverlayDef();
    }

    [Serializable]
    public sealed class ChemicalHazardProfile
    {
        public string hazard_id = string.Empty;
        public string display_name = string.Empty;
        public string hazard_class = string.Empty;
        public string detector_response_band = string.Empty;
        public float normalized_concentration;
        public float persistence;
        public float volatility;
        public float wind_response;
        public string filter_category = string.Empty;
        public float filter_load_rate;
        public float exposure_severity;
        public float sample_value;
        public float detection_threshold;
        public string safe_exposure_band = string.Empty;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class DetectorEquipmentDef
    {
        public string detector_item_id = "item_portable_pid_detector";
        public string sensor_module_item_id = "item_detector_sensor_module";
        public List<string> detector_bands = new List<string>();
        public float base_detection_confidence = 0.85f;
        public int battery_ticks_per_charge = 120;
        public int per_scan_battery_drain = 1;
    }

    [Serializable]
    public sealed class SampleCollectionDef
    {
        public string ampoule_item_id = "item_hermetic_sample_ampoule";
        public float sample_quality_base = 0.7f;
        public float sample_quality_skill_factor = 0.05f;
        public int max_samples_per_mission = 6;
    }

    [Serializable]
    public sealed class FilterModelDef
    {
        public float filter_capacity_base = 100f;
        public float incompatible_filter_penalty = 2.5f;
        public float breakthrough_warning_threshold = 0.15f;
        public List<string> filter_categories = new List<string>();
    }

    [Serializable]
    public sealed class MapOverlayDef
    {
        public List<string> discovery_state_transitions = new List<string> { "unknown", "suspected", "identified", "quantified" };
        public float safe_corridor_confidence_required = 0.7f;
        public int overlay_persistence_days = 30;
    }

    // ─── State DTOs ───

    [Serializable]
    public sealed class ChemicalReconState
    {
        public string systemId = ChemicalReconEngine.SystemId;
        public List<HazardObservation> hazardObservations = new List<HazardObservation>();
        public List<CollectedSample> collectedSamples = new List<CollectedSample>();
        public List<string> discoveredHazardIds = new List<string>();
        public int detectorBatteryRemaining = 120;
        public string activeSensorBand = "wide_band";
        public List<string> unlockedFilterCategories = new List<string> { "particulate_only" };
        public List<string> safeCorridorIds = new List<string>();
    }

    [Serializable]
    public sealed class HazardObservation
    {
        public string observationId = string.Empty;
        public string hazardId = string.Empty;
        public string locationNodeId = string.Empty;
        public float confidence;
        public float normalizedLevel;
        public string discoveryState = "unknown"; // unknown, suspected, identified, quantified
        public int observedDay;
        public int lastConfirmedDay;
        public string detectorBand = string.Empty;
        public string recommendedFilterCategory = string.Empty;
        public string safeExposureBand = string.Empty;
    }

    [Serializable]
    public sealed class CollectedSample
    {
        public string sampleId = string.Empty;
        public string hazardId = string.Empty;
        public float quality;
        public int collectedDay;
        public bool deliveredToLab;
        public string locationNodeId = string.Empty;
    }

    // ─── Result types ───

    public sealed class ChemicalDetectionResult
    {
        public bool Detected;
        public string HazardClass = string.Empty;
        public string HazardId = string.Empty;
        public float Confidence;
        public float NormalizedLevel;
        public string RecommendedFilterCategory = string.Empty;
        public string SafeExposureBand = "safe";
        public float FilterLoadRate;
    }

    // ─── Engine ───

    /// <summary>
    /// ASHFALL Chemical Reconnaissance Engine (Plan 81).
    /// Owns detector configuration, hazard survey observations, sample collection,
    /// filter suitability analysis, and safe-corridor discovery.
    /// Does not own canonical survivor affliction state, global map, expedition roster, or weather/wind.
    /// </summary>
    public sealed class ChemicalReconEngine
    {
        public const string SystemId = "chemical_recon";

        private ChemicalReconState _state = new ChemicalReconState();
        private readonly ToxicChemicalCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public ChemicalReconState State => _state;
        public IReadOnlyList<HazardObservation> Observations => _state.hazardObservations;
        public IReadOnlyList<CollectedSample> Samples => _state.collectedSamples;
        public IReadOnlyList<string> DiscoveredHazards => _state.discoveredHazardIds;

        public event Action<HazardObservation>? OnHazardIdentified;
        public event Action<CollectedSample>? OnSampleCollected;
        public event Action<string>? OnFilterBreakthrough;
        public event Action<string>? OnSafeCorridorDiscovered;
        public event Action? OnReconChanged;

        public ChemicalReconEngine(ToxicChemicalCatalog catalog, ISeededRng rng, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            // Battery capacity is catalog-owned, not a hardcoded DTO default.
            _state.detectorBatteryRemaining = _catalog.detector_equipment.battery_ticks_per_charge;
        }

        public ChemicalHazardProfile? FindHazard(string hazardId)
        {
            if (string.IsNullOrEmpty(hazardId)) return null;
            foreach (var h in _catalog.hazard_profiles)
                if (h.hazard_id == hazardId) return h;
            return null;
        }

        /// <summary>
        /// Scan a location with the detector. Returns a detection result.
        /// </summary>
        public ChemicalDetectionResult ScanLocation(string locationNodeId, string detectorBand, float surveyorSkill = 0.5f)
        {
            var result = new ChemicalDetectionResult();

            // Check battery
            if (_state.detectorBatteryRemaining <= 0)
                return result;

            _state.detectorBatteryRemaining -= _catalog.detector_equipment.per_scan_battery_drain;

            // Find hazards at this location (deterministic: use location hash as seed hint)
            foreach (var hazard in _catalog.hazard_profiles)
            {
                // Only detect hazards in the detector's band
                if (hazard.detector_response_band != detectorBand && detectorBand != "wide_band")
                    continue;

                // Deterministic detection check using location+hazard hash (stable, not GetHashCode)
                float hashVal = StableStringHash(locationNodeId + hazard.hazard_id);
                float baseConfidence = _catalog.detector_equipment.base_detection_confidence;
                float skillBonus = surveyorSkill * 0.15f;
                float confidence = baseConfidence + skillBonus;

                // Deterministic noise
                float noise = (float)(_rng.NextDouble() * 0.1 - 0.05);
                confidence += noise;

                if (confidence >= hazard.detection_threshold)
                {
                    result.Detected = true;
                    result.HazardId = hazard.hazard_id;
                    result.HazardClass = hazard.hazard_class;
                    result.Confidence = Math.Clamp(confidence, 0f, 1f);
                    result.NormalizedLevel = hazard.normalized_concentration;
                    result.RecommendedFilterCategory = hazard.filter_category;
                    result.SafeExposureBand = hazard.safe_exposure_band;
                    result.FilterLoadRate = hazard.filter_load_rate;

                    // Record or update observation
                    RecordObservation(locationNodeId, hazard, confidence, detectorBand);
                    break; // Return first detected hazard
                }
            }

            return result;
        }

        private void RecordObservation(string locationNodeId, ChemicalHazardProfile hazard, float confidence, string detectorBand)
        {
            // Check for existing observation
            HazardObservation? existing = null;
            foreach (var obs in _state.hazardObservations)
            {
                if (obs.hazardId == hazard.hazard_id && obs.locationNodeId == locationNodeId)
                {
                    existing = obs;
                    break;
                }
            }

            if (existing != null)
            {
                existing.confidence = Math.Max(existing.confidence, confidence);
                existing.lastConfirmedDay = _currentDay;
                if (existing.discoveryState == "suspected")
                    existing.discoveryState = "identified";
                if (confidence >= 0.8f && existing.discoveryState == "identified")
                    existing.discoveryState = "quantified";
            }
            else
            {
                var obs = new HazardObservation
                {
                    observationId = $"obs_{hazard.hazard_id}_{locationNodeId}",
                    hazardId = hazard.hazard_id,
                    locationNodeId = locationNodeId,
                    confidence = confidence,
                    normalizedLevel = hazard.normalized_concentration,
                    discoveryState = confidence >= 0.8f ? "identified" : "suspected",
                    observedDay = _currentDay,
                    lastConfirmedDay = _currentDay,
                    detectorBand = detectorBand,
                    recommendedFilterCategory = hazard.filter_category,
                    safeExposureBand = hazard.safe_exposure_band
                };

                _state.hazardObservations.Add(obs);

                if (!_state.discoveredHazardIds.Contains(hazard.hazard_id))
                {
                    _state.discoveredHazardIds.Add(hazard.hazard_id);
                }

                _log.Info($"[ChemRecon] hazard {hazard.hazard_id} {obs.discoveryState} at {locationNodeId} (conf={confidence:F2})");
                OnHazardIdentified?.Invoke(obs);
            }

            OnReconChanged?.Invoke();
        }

        /// <summary>
        /// Calculate filter consumption for a given exposure duration.
        /// </summary>
        public float CalculateFilterConsumption(string hazardId, string filterCategory, float exposureTicks)
        {
            var hazard = FindHazard(hazardId);
            if (hazard == null) return 0;

            float loadRate = hazard.filter_load_rate;

            // Incompatible filter penalty
            if (hazard.filter_category != filterCategory && filterCategory != "multi_gas")
            {
                loadRate *= _catalog.filter_model.incompatible_filter_penalty;
            }

            return loadRate * exposureTicks;
        }

        /// <summary>
        /// Check if a filter is at breakthrough risk.
        /// </summary>
        public bool IsFilterBreakthrough(float remainingCapacity, float filterCapacityBase)
        {
            float threshold = _catalog.filter_model.breakthrough_warning_threshold * filterCapacityBase;
            return remainingCapacity <= threshold;
        }

        /// <summary>
        /// Collect a sample from a confirmed hazard.
        /// </summary>
        public ActionResult CollectSample(string hazardId, string locationNodeId, float surveyorSkill = 0.5f, Func<string, int, bool>? consumeItem = null, Func<string, int, bool>? addItem = null)
        {
            var hazard = FindHazard(hazardId);
            if (hazard == null)
                return ActionResult.Blocked("unknown_hazard", "chemrecon.unknown_hazard");

            if (_state.collectedSamples.Count >= _catalog.sample_collection.max_samples_per_mission)
                return ActionResult.Blocked("sample_limit", "chemrecon.sample_limit");

            // Consume ampoule
            if (consumeItem != null && !consumeItem(_catalog.sample_collection.ampoule_item_id, 1))
                return ActionResult.Blocked("no_ampoule", "chemrecon.no_ampoule");

            float quality = _catalog.sample_collection.sample_quality_base +
                surveyorSkill * _catalog.sample_collection.sample_quality_skill_factor;
            quality = Math.Clamp(quality, 0.3f, 1f);

            var sample = new CollectedSample
            {
                sampleId = $"sample_{hazardId}_{_currentDay}_{_state.collectedSamples.Count}",
                hazardId = hazardId,
                quality = quality,
                collectedDay = _currentDay,
                deliveredToLab = false,
                locationNodeId = locationNodeId
            };

            _state.collectedSamples.Add(sample);
            _log.Info($"[ChemRecon] sample collected: {sample.sampleId} quality={quality:F2}");
            OnSampleCollected?.Invoke(sample);
            OnReconChanged?.Invoke();
            return ActionResult.Success("chemrecon.sample_collected");
        }

        /// <summary>
        /// Deliver a sample to the PharmaLab. Returns the sample for lab handoff.
        /// </summary>
        public CollectedSample? DeliverSampleToLab(string sampleId)
        {
            foreach (var s in _state.collectedSamples)
            {
                if (s.sampleId == sampleId && !s.deliveredToLab)
                {
                    s.deliveredToLab = true;
                    _log.Info($"[ChemRecon] sample {sampleId} delivered to lab");
                    OnReconChanged?.Invoke();
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Try to discover a safe corridor through a hazard zone.
        /// </summary>
        public bool TryDiscoverSafeCorridor(string corridorId, string locationNodeId, float surveyorSkill = 0.5f)
        {
            // Check if we have sufficient confidence in hazard observations
            float totalConfidence = 0;
            int obsCount = 0;
            foreach (var obs in _state.hazardObservations)
            {
                if (obs.locationNodeId == locationNodeId)
                {
                    totalConfidence += obs.confidence;
                    obsCount++;
                }
            }

            if (obsCount == 0) return false;

            float avgConfidence = totalConfidence / obsCount;
            float requiredConfidence = _catalog.map_overlay.safe_corridor_confidence_required;
            float skillBonus = surveyorSkill * 0.1f;

            if (avgConfidence + skillBonus >= requiredConfidence)
            {
                if (!_state.safeCorridorIds.Contains(corridorId))
                {
                    _state.safeCorridorIds.Add(corridorId);
                    _log.Info($"[ChemRecon] safe corridor discovered: {corridorId}");
                    OnSafeCorridorDiscovered?.Invoke(corridorId);
                }
                OnReconChanged?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if a corridor is known safe.
        /// </summary>
        public bool IsCorridorSafe(string corridorId) => _state.safeCorridorIds.Contains(corridorId);

        /// <summary>
        /// Get the recommended filter category for a location.
        /// </summary>
        public string GetRecommendedFilter(string locationNodeId)
        {
            foreach (var obs in _state.hazardObservations)
            {
                if (obs.locationNodeId == locationNodeId)
                    return obs.recommendedFilterCategory;
            }
            return "particulate_only";
        }

        /// <summary>
        /// Get the safe exposure band for a location.
        /// </summary>
        public string GetSafeExposureBand(string locationNodeId)
        {
            foreach (var obs in _state.hazardObservations)
            {
                if (obs.locationNodeId == locationNodeId)
                    return obs.safeExposureBand;
            }
            return "safe";
        }

        /// <summary>
        /// Change the active detector sensor band.
        /// </summary>
        public ActionResult SetSensorBand(string band)
        {
            if (!_catalog.detector_equipment.detector_bands.Contains(band))
                return ActionResult.Blocked("invalid_band", "chemrecon.invalid_band");
            _state.activeSensorBand = band;
            OnReconChanged?.Invoke();
            return ActionResult.Success("chemrecon.band_set");
        }

        /// <summary>
        /// Recharge the detector battery.
        /// </summary>
        public void RechargeBattery()
        {
            _state.detectorBatteryRemaining = _catalog.detector_equipment.battery_ticks_per_charge;
            OnReconChanged?.Invoke();
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Age out old observations
            int persistenceDays = _catalog.map_overlay.overlay_persistence_days;
            for (int i = _state.hazardObservations.Count - 1; i >= 0; i--)
            {
                var obs = _state.hazardObservations[i];
                if (day - obs.lastConfirmedDay > persistenceDays && obs.discoveryState != "quantified")
                {
                    _state.hazardObservations.RemoveAt(i);
                }
            }
        }

        public ChemicalReconState CaptureState() => CloneState(_state);

        public void RestoreState(ChemicalReconState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ChemicalReconState CloneState(ChemicalReconState src)
        {
            if (src == null) return new ChemicalReconState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ChemicalReconState>(json) ?? new ChemicalReconState();
        }

        public ToxicChemicalCatalog Catalog => _catalog;

        /// <summary>
        /// Deterministic string-to-float hash (0..1). Uses FNV-1a style for cross-platform stability.
        /// </summary>
        private static float StableStringHash(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0f;
            uint hash = 2166136261;
            foreach (char c in s)
            {
                hash ^= c;
                hash *= 16777619;
            }
            return (float)(hash % 1000000) / 1000000f;
        }
    }

    /// <summary>
    /// Loads <c>toxic_chemical_catalog.json</c>.
    /// </summary>
    public static class ToxicChemicalCatalogLoader
    {
        public static ToxicChemicalCatalog Load(string dataDir, IFileIO files, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) throw new ArgumentException("dataDir required", nameof(dataDir));
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            string path = files.Combine(dataDir, "toxic_chemical_catalog.json");
            if (!files.FileExists(path))
                return new ToxicChemicalCatalog();

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new ToxicChemicalCatalog();

            var catalog = json.Deserialize<ToxicChemicalCatalog>(raw);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize toxic_chemical_catalog.json");

            // Validate
            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var h in catalog.hazard_profiles)
            {
                if (string.IsNullOrEmpty(h.hazard_id))
                    throw new InvalidOperationException("Chemical catalog: hazard_id is required");
                if (!seenIds.Add(h.hazard_id))
                    throw new InvalidOperationException($"Chemical catalog: duplicate hazard_id '{h.hazard_id}'");
                if (h.normalized_concentration < 0 || h.normalized_concentration > 1)
                    throw new InvalidOperationException($"Chemical catalog: invalid normalized_concentration for '{h.hazard_id}'");
                if (h.filter_load_rate < 0)
                    throw new InvalidOperationException($"Chemical catalog: invalid filter_load_rate for '{h.hazard_id}'");
            }

            return catalog;
        }
    }
}