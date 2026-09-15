// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Authored InSAR sensor/platform profile (Plan 139 Phase 1). Abstract
    /// remote-sensing characteristics — never a real radar engineering spec.
    /// </summary>
    public sealed class InSarSensorDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        /// <summary>Swath coverage, kilometres.</summary>
        public int coverage_width_km { get; set; }
        /// <summary>Nominal ground resolution, metres.</summary>
        public int nominal_resolution_m { get; set; }
        /// <summary>Nominal days between compatible repeat passes.</summary>
        public int repeat_pass_interval_days { get; set; } = 6;
        /// <summary>Coherence lost per day of temporal baseline, basis points.</summary>
        public int coherence_decay_rate_bp { get; set; } = 10;
        /// <summary>How strongly poor weather decorrelates a pass, basis points.</summary>
        public int weather_decorrelation_modifier_bp { get; set; } = 30;
        /// <summary>Smallest theoretical deformation the profile can resolve, millimetres.</summary>
        public int minimum_detectable_deformation_mm { get; set; } = 8;
        /// <summary>How much operator skill can improve processing, basis points (0..100).</summary>
        public int processing_skill_modifier_bp { get; set; } = 15;
        /// <summary>Terrain tags the profile decorrelates over (e.g. forest, canopy).</summary>
        public List<string> terrain_decorrelation_tags { get; set; } = new List<string>();
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class InSarGeodesyCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<InSarSensorDef> sensor_profiles { get; set; } = new List<InSarSensorDef>();

        private readonly Dictionary<string, InSarSensorDef> _byId =
            new Dictionary<string, InSarSensorDef>(StringComparer.Ordinal);

        public void Index()
        {
            _byId.Clear();
            foreach (var def in sensor_profiles)
            {
                if (def != null && !string.IsNullOrEmpty(def.id))
                    _byId[def.id] = def;
            }
        }

        public InSarSensorDef? Get(string sensorId)
        {
            if (string.IsNullOrEmpty(sensorId)) return null;
            _byId.TryGetValue(sensorId, out var def);
            return def;
        }

        public IReadOnlyCollection<InSarSensorDef> All => _byId.Values;
    }

    public static class InSarGeodesyCatalogLoader
    {
        public const string DefaultFileName = "insar_geodesy_catalog.json";

        public static InSarGeodesyCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir))
                return Empty();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path))
                return Empty();

            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                return Empty();

            var catalog = JsonSerializer.Deserialize<InSarGeodesyCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();

            catalog.Index();
            return catalog;
        }

        private static InSarGeodesyCatalog Empty()
        {
            var catalog = new InSarGeodesyCatalog();
            catalog.Index();
            return catalog;
        }
    }

    /// <summary>Stable deformation-intelligence classes. Never terrain truth.</summary>
    public static class InSarClassification
    {
        public const string Unsurveyed = "unsurveyed";
        public const string Stable = "stable";
        public const string SlowSubsidence = "slow_subsidence";
        public const string AcceleratingSubsidence = "accelerating_subsidence";
        public const string AbruptDeformation = "abrupt_deformation";
        public const string LowConfidence = "low_confidence";
    }

    /// <summary>A single survey observation. Two compatible passes make a deformation map.</summary>
    [Serializable]
    public sealed class SurveyPass
    {
        public string PassId { get; set; } = string.Empty;
        public string SensorProfileId { get; set; } = string.Empty;
        public string SectorId { get; set; } = string.Empty;
        public int ObservationDay { get; set; }
        /// <summary>Observation quality 0..100 (phase/backscatter read, supplied by the world owner).</summary>
        public int QualityBp { get; set; }
        /// <summary>Reference geometry id; only same-reference passes are compatible.</summary>
        public string ReferenceGeometryId { get; set; } = string.Empty;
        /// <summary>Weather quality during the pass 0..100 (100 = clear).</summary>
        public int WeatherQualityBp { get; set; } = 100;
        public bool Processed { get; set; }
    }

    /// <summary>Derived sector intelligence: bounded, uncertain, presentation-facing.</summary>
    [Serializable]
    public sealed class SectorDeformationSummary
    {
        public string SectorId { get; set; } = string.Empty;
        public string SensorProfileId { get; set; } = string.Empty;
        /// <summary>Interferometric coherence 0..1.</summary>
        public double Coherence { get; set; }
        /// <summary>Relative line-of-sight displacement, millimetres (negative = subsidence).</summary>
        public double RelativeDisplacementMm { get; set; }
        /// <summary>Bounded confidence 0.05..0.97.</summary>
        public double Confidence { get; set; }
        /// <summary>Displacement rate, millimetres per day.</summary>
        public double TrendVelocityMmPerDay { get; set; }
        public string Classification { get; set; } = InSarClassification.Unsurveyed;
        public int CompatiblePassCount { get; set; }
        public int LastProcessedDay { get; set; }
        public int WeatherQualityBp { get; set; } = 100;
        public string Note { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class InSarDeformationState
    {
        public string SystemId { get; set; } = InSarDeformationEngine.SystemId;
        public int SchemaVersion { get; set; } = 1;
        /// <summary>Bounded pass history (oldest evicted first).</summary>
        public List<SurveyPass> Passes { get; set; } = new List<SurveyPass>();
        public List<SectorDeformationSummary> Summaries { get; set; } = new List<SectorDeformationSummary>();
        public int NextPassSeq { get; set; }
        public int TotalPasses { get; set; }
        public int TotalProcessed { get; set; }
    }

    /// <summary>
    /// Plan 139 Phase 1 — repeat-pass InSAR deformation intelligence. Reads
    /// survey observations and projects trend/uncertainty; it never mutates
    /// world terrain or predicts a deterministic earthquake. Two compatible
    /// passes are required before any map exists; below minimum coherence the
    /// result is explicitly low-confidence, never a clean read.
    /// </summary>
    public sealed class InSarDeformationEngine
    {
        public const string SystemId = "insar_deformation";

        public const int MaxPasses = 256;
        public const int MinCompatiblePasses = 2;

        /// <summary>Below this coherence no usable deformation map exists.</summary>
        public const double MinCoherence = 0.35;
        /// <summary>Velocities at or below this magnitude are classified stable (mm/day).</summary>
        public const double StableVelocityMmPerDay = 0.05;
        /// <summary>Absolute displacement at or above this is abrupt deformation (mm).</summary>
        public const double AbruptDisplacementMm = 25.0;
        /// <summary>Second-half rate must exceed the first-half rate by this factor to accelerate.</summary>
        public const double AcceleratingRatio = 1.5;

        public const double MaxConfidence = 0.97;
        public const double MinConfidence = 0.05;

        private InSarDeformationState _state = new InSarDeformationState();
        private readonly InSarGeodesyCatalog _catalog;
        private readonly ILog _log;

        /// <summary>Host wires the seeded RNG for deterministic processing jitter.</summary>
        public ISeededRng? Rng { get; set; }

        public InSarDeformationState State => _state;
        public InSarGeodesyCatalog Catalog => _catalog;

        public InSarDeformationEngine(InSarGeodesyCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        // ---- survey passes --------------------------------------------------

        /// <summary>
        /// Records one survey observation. Quality is the world owner's reading of
        /// the returned phase/backscatter; the engine only stores and interprets it.
        /// </summary>
        public ActionResult RecordSurveyPass(
            string sensorProfileId,
            string sectorId,
            int day,
            int qualityBp,
            int weatherQualityBp,
            string referenceGeometryId)
        {
            var sensor = _catalog.Get(sensorProfileId);
            if (sensor == null)
                return ActionResult.Blocked("processing_data_missing", "insar.processing_data_missing");
            if (string.IsNullOrEmpty(sectorId))
                return ActionResult.Blocked("sector_not_surveyed", "insar.sector_not_surveyed");

            _state.NextPassSeq++;
            _state.Passes.Add(new SurveyPass
            {
                PassId = $"insar_pass_{day}_{_state.NextPassSeq}",
                SensorProfileId = sensorProfileId,
                SectorId = sectorId,
                ObservationDay = day,
                QualityBp = ClampBp(qualityBp),
                ReferenceGeometryId = referenceGeometryId ?? string.Empty,
                WeatherQualityBp = ClampBp(weatherQualityBp)
            });
            _state.TotalPasses++;

            if (_state.Passes.Count > MaxPasses)
                _state.Passes.RemoveAt(0);

            return ActionResult.Success("insar.pass_recorded");
        }

        /// <summary>
        /// Processes the compatible repeat-pass stack for a sector into a
        /// deformation summary. Requires at least two same-reference passes;
        /// a single pass is refused rather than guessed.
        /// </summary>
        public ActionResult ProcessSector(
            string sectorId,
            double processingSkill,
            IReadOnlyCollection<string>? terrainTags = null)
        {
            if (string.IsNullOrEmpty(sectorId))
                return ActionResult.Blocked("sector_not_surveyed", "insar.sector_not_surveyed");

            var all = PassesForSector(sectorId);
            if (all.Count == 0)
                return ActionResult.Blocked("sector_not_surveyed", "insar.sector_not_surveyed");

            // Stable reference selection: the most recent pass's reference geometry.
            string reference = all[all.Count - 1].ReferenceGeometryId;
            var compatible = all.FindAll(p => string.Equals(p.ReferenceGeometryId, reference, StringComparison.Ordinal));

            if (compatible.Count < MinCompatiblePasses)
            {
                // Other passes exist under a different geometry: that is a
                // compatibility failure, not merely "not enough data".
                if (all.Count >= MinCompatiblePasses)
                    return ActionResult.Blocked("pass_geometry_incompatible", "insar.pass_geometry_incompatible");
                return ActionResult.Blocked("insufficient_survey_passes", "insar.insufficient_survey_passes");
            }

            compatible.Sort((a, b) => a.ObservationDay != b.ObservationDay
                ? a.ObservationDay.CompareTo(b.ObservationDay)
                : string.CompareOrdinal(a.PassId, b.PassId));

            int span = compatible[compatible.Count - 1].ObservationDay - compatible[0].ObservationDay;
            if (span <= 0)
                return ActionResult.Blocked("insufficient_survey_passes", "insar.insufficient_survey_passes");

            var sensor = _catalog.Get(compatible[0].SensorProfileId);
            if (sensor == null)
                return ActionResult.Blocked("processing_data_missing", "insar.processing_data_missing");

            double coherence = ComputeCoherence(sensor, compatible, span, terrainTags);
            int avgWeather = AverageWeather(compatible);
            double avgQuality = AverageQuality(compatible);

            if (coherence < MinCoherence)
            {
                StoreSummary(new SectorDeformationSummary
                {
                    SectorId = sectorId,
                    SensorProfileId = sensor.id,
                    Coherence = coherence,
                    Confidence = MinConfidence,
                    Classification = InSarClassification.LowConfidence,
                    CompatiblePassCount = compatible.Count,
                    LastProcessedDay = compatible[compatible.Count - 1].ObservationDay,
                    WeatherQualityBp = avgWeather,
                    Note = "coherence_below_threshold"
                }, compatible);
                _log.Info($"[InSAR] Sector {sectorId} rejected: coherence {coherence:F2} < {MinCoherence:F2}");
                return ActionResult.Blocked("low_coherence", "insar.low_coherence");
            }

            // Displacement derived from the observation quality swing: the world
            // owner reads phase, the engine converts it. No synthetic geology.
            double scaleMm = sensor.minimum_detectable_deformation_mm * 4.0;
            double firstDisp = QualityToDisplacement(compatible[0].QualityBp, scaleMm);
            double lastDisp = QualityToDisplacement(compatible[compatible.Count - 1].QualityBp, scaleMm);
            double displacement = lastDisp - firstDisp;
            double velocity = displacement / span;

            string classification = Classify(compatible, displacement, velocity, scaleMm);

            double skillModifier = Clamp01(processingSkill) * (sensor.processing_skill_modifier_bp / 100.0);
            double confidence = 0.30
                + 0.25 * coherence
                + 0.20 * (avgQuality / 100.0)
                + 0.20 * skillModifier;
            confidence = Math.Max(MinConfidence, Math.Min(MaxConfidence, confidence));
            if (Rng != null)
                confidence = Math.Max(MinConfidence, Math.Min(MaxConfidence,
                    confidence + (Rng.NextDouble() - 0.5) * 0.03));

            StoreSummary(new SectorDeformationSummary
            {
                SectorId = sectorId,
                SensorProfileId = sensor.id,
                Coherence = coherence,
                RelativeDisplacementMm = displacement,
                Confidence = confidence,
                TrendVelocityMmPerDay = velocity,
                Classification = classification,
                CompatiblePassCount = compatible.Count,
                LastProcessedDay = compatible[compatible.Count - 1].ObservationDay,
                WeatherQualityBp = avgWeather,
                Note = string.Empty
            }, compatible);

            _state.TotalProcessed++;
            _log.Info($"[InSAR] Sector {sectorId} processed: {classification}, {displacement:F1}mm over {span}d, coherence {coherence:F2}");
            return ActionResult.Success("insar.sector_processed");
        }

        /// <summary>Deterministic quality→displacement mapping, centred on quality 50.</summary>
        private static double QualityToDisplacement(int qualityBp, double scaleMm)
            => (qualityBp - 50) / 50.0 * scaleMm;

        private string Classify(List<SurveyPass> passes, double displacement, double velocity, double scaleMm)
        {
            if (Math.Abs(displacement) >= AbruptDisplacementMm)
                return InSarClassification.AbruptDeformation;

            // With three or more passes, compare early and late rates. Only a
            // same-direction acceleration counts; a bounce is not a trend.
            if (passes.Count >= 3)
            {
                double firstRate = RateAcross(passes, 0, passes.Count / 2);
                double lastRate = RateAcross(passes, passes.Count / 2, passes.Count - 1);
                if (firstRate < 0 && lastRate < 0 && Math.Abs(lastRate) > Math.Abs(firstRate) * AcceleratingRatio)
                    return InSarClassification.AcceleratingSubsidence;
            }

            if (velocity <= -StableVelocityMmPerDay)
                return InSarClassification.SlowSubsidence;

            return InSarClassification.Stable;
        }

        private static double RateAcross(List<SurveyPass> passes, int start, int end)
        {
            if (end <= start) return 0.0;
            var sensor = passes[start];
            int span = passes[end].ObservationDay - passes[start].ObservationDay;
            if (span <= 0) return 0.0;
            double scale = 40.0;
            double d = QualityToDisplacement(passes[end].QualityBp, scale)
                     - QualityToDisplacement(sensor.QualityBp, scale);
            return d / span;
        }

        private static double ComputeCoherence(
            InSarSensorDef sensor,
            List<SurveyPass> passes,
            int span,
            IReadOnlyCollection<string>? terrainTags)
        {
            double coherence = 0.95;
            coherence -= (sensor.coherence_decay_rate_bp / 10000.0) * span;

            int avgWeather = AverageWeather(passes);
            double weatherDeficit = (1.0 - avgWeather / 100.0) * (sensor.weather_decorrelation_modifier_bp / 100.0);
            coherence -= weatherDeficit;

            if (terrainTags != null && sensor.terrain_decorrelation_tags.Count > 0)
            {
                int matches = 0;
                foreach (var tag in terrainTags)
                {
                    if (string.IsNullOrEmpty(tag)) continue;
                    for (int i = 0; i < sensor.terrain_decorrelation_tags.Count; i++)
                    {
                        if (string.Equals(sensor.terrain_decorrelation_tags[i], tag, StringComparison.Ordinal))
                        {
                            matches++;
                            break;
                        }
                    }
                }
                coherence -= Math.Min(0.30, matches * 0.10);
            }

            return Math.Max(0.0, Math.Min(0.99, coherence));
        }

        private static int AverageWeather(List<SurveyPass> passes)
        {
            if (passes.Count == 0) return 100;
            int sum = 0;
            foreach (var p in passes) sum += p.WeatherQualityBp;
            return sum / passes.Count;
        }

        private static double AverageQuality(List<SurveyPass> passes)
        {
            if (passes.Count == 0) return 0;
            int sum = 0;
            foreach (var p in passes) sum += p.QualityBp;
            return (double)sum / passes.Count;
        }

        private void StoreSummary(SectorDeformationSummary summary, List<SurveyPass> compatible)
        {
            int existing = _state.Summaries.FindIndex(s => string.Equals(s.SectorId, summary.SectorId, StringComparison.Ordinal));
            if (existing >= 0)
                _state.Summaries[existing] = summary;
            else
                _state.Summaries.Add(summary);

            foreach (var p in compatible)
                p.Processed = true;
        }

        // ---- read models ----------------------------------------------------

        public List<SurveyPass> PassesForSector(string sectorId)
        {
            var list = new List<SurveyPass>();
            if (string.IsNullOrEmpty(sectorId)) return list;
            foreach (var p in _state.Passes)
            {
                if (string.Equals(p.SectorId, sectorId, StringComparison.Ordinal))
                    list.Add(p);
            }
            return list;
        }

        public SectorDeformationSummary? GetSummary(string sectorId)
        {
            if (string.IsNullOrEmpty(sectorId)) return null;
            return _state.Summaries.Find(s => string.Equals(s.SectorId, sectorId, StringComparison.Ordinal));
        }

        /// <summary>
        /// Engineering warning for excavation planning. InSAR never collapses a
        /// tunnel; it only raises awareness for the excavation owner to consume.
        /// </summary>
        public string GetExcavationWarning(string sectorId)
        {
            var s = GetSummary(sectorId);
            if (s == null) return "No deformation intelligence for this sector.";
            switch (s.Classification)
            {
                case InSarClassification.AcceleratingSubsidence:
                    return "Accelerating subsidence detected — reinforce underground works before proceeding.";
                case InSarClassification.AbruptDeformation:
                    return "Abrupt deformation detected — treat ground as unstable and re-survey.";
                case InSarClassification.SlowSubsidence:
                    return "Slow subsidence detected — monitor before heavy excavation.";
                case InSarClassification.LowConfidence:
                    return "Deformation read is low-confidence — inconclusive, not safe.";
                default:
                    return "No significant deformation trend detected.";
            }
        }

        /// <summary>Known travel-corridor risk band. Only known intelligence affects routing.</summary>
        public string GetTravelRisk(string sectorId)
        {
            var s = GetSummary(sectorId);
            if (s == null) return InSarClassification.Unsurveyed;
            switch (s.Classification)
            {
                case InSarClassification.AcceleratingSubsidence:
                case InSarClassification.AbruptDeformation:
                    return "fractured";
                case InSarClassification.SlowSubsidence:
                    return "subsiding";
                case InSarClassification.LowConfidence:
                    return "unknown";
                default:
                    return InSarClassification.Stable;
            }
        }

        private static int ClampBp(int bp) => Math.Max(0, Math.Min(100, bp));
        private static double Clamp01(double v) => Math.Max(0.0, Math.Min(1.0, v));

        // ---- persistence ----------------------------------------------------

        public InSarDeformationState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<InSarDeformationState>(json) ?? new InSarDeformationState();
        }

        public void RestoreState(InSarDeformationState? saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<InSarDeformationState>(json) ?? new InSarDeformationState();
            if (string.IsNullOrEmpty(_state.SystemId))
                _state.SystemId = SystemId;
        }
    }
}
