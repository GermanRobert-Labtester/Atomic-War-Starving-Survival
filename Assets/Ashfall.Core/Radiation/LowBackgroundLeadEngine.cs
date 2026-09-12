// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Radiation
{
    /// <summary>
    /// Authored low-background shielding material profile (Plan 138 Phase 1).
    /// Provenance-authored residual activity — never "zero radioactivity".
    /// </summary>
    public sealed class LowBackgroundMaterialDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        /// <summary>Provenance class: pre_industrial_salvage / submerged_ballast / certified_shield_stock / ordinary_modern_scrap.</summary>
        public string source_tag { get; set; } = string.Empty;
        /// <summary>very_low / low / ordinary.</summary>
        public string background_activity_class { get; set; } = "ordinary";
        /// <summary>Residual background contribution in basis points of the ordinary baseline (bounded 0..200).</summary>
        public int residual_activity_bp { get; set; }
        /// <summary>Shielding effectiveness, percent (bounded 0..95).</summary>
        public int shielding_factor_pct { get; set; }
        /// <summary>0..1 — how strongly a foreign source tag contaminates a shared batch.</summary>
        public double cross_contamination_sensitivity { get; set; }
        /// <summary>Material lost when smelting into shield stock, percent (bounded 0..50).</summary>
        public int processing_loss_pct { get; set; }
        public List<string> detector_compatibility_tags { get; set; } = new List<string>();
        public string required_quality_control { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class LowBackgroundLeadCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<LowBackgroundMaterialDef> material_profiles { get; set; } = new List<LowBackgroundMaterialDef>();

        private readonly Dictionary<string, LowBackgroundMaterialDef> _byId =
            new Dictionary<string, LowBackgroundMaterialDef>(StringComparer.Ordinal);

        public void Index()
        {
            _byId.Clear();
            foreach (var def in material_profiles)
            {
                if (def != null && !string.IsNullOrEmpty(def.id))
                    _byId[def.id] = def;
            }
        }

        public LowBackgroundMaterialDef? Get(string materialId)
        {
            if (string.IsNullOrEmpty(materialId)) return null;
            _byId.TryGetValue(materialId, out var def);
            return def;
        }

        public IReadOnlyCollection<LowBackgroundMaterialDef> All => _byId.Values;
    }

    public static class LowBackgroundLeadCatalogLoader
    {
        public const string DefaultFileName = "low_background_lead_catalog.json";

        public static LowBackgroundLeadCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir))
                return Empty();

            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path))
                return Empty();

            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                return Empty();

            var catalog = JsonSerializer.Deserialize<LowBackgroundLeadCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();

            catalog.Index();
            return catalog;
        }

        private static LowBackgroundLeadCatalog Empty()
        {
            var catalog = new LowBackgroundLeadCatalog();
            catalog.Index();
            return catalog;
        }
    }

    /// <summary>Transactional smelting batch. Contamination downgrades; never deletes material.</summary>
    [Serializable]
    public sealed class LowBackgroundBatch
    {
        public string BatchId { get; set; } = string.Empty;
        public string ProfileId { get; set; } = string.Empty;
        public string SourceTag { get; set; } = string.Empty;
        /// <summary>very_low / low / ordinary — current class after any downgrade.</summary>
        public string BackgroundClass { get; set; } = "ordinary";
        public int UnitsCommitted { get; set; }
        public int UnitsLost { get; set; }
        public bool Contaminated { get; set; }
        public string ContaminationNote { get; set; } = string.Empty;
        public int Day { get; set; }
        public bool Committed { get; set; }
    }

    /// <summary>Installed shielding + detector calibration, all bounded.</summary>
    [Serializable]
    public sealed class LowBackgroundMetrologyState
    {
        public string SystemId { get; set; } = LowBackgroundLeadEngine.SystemId;
        public int SchemaVersion { get; set; } = 1;
        public bool DetectorInstalled { get; set; }
        public bool DetectorCalibrated { get; set; }
        public int CalibratedOnDay { get; set; }
        /// <summary>Installed shield module profile ids (installed = transactional).</summary>
        public List<string> InstalledModuleProfileIds { get; set; } = new List<string>();
        /// <summary>Composite residual background of installed shielding, bp of ordinary baseline.</summary>
        public int InstalledResidualActivityBp { get; set; }
        /// <summary>Composite shielding factor of installed modules, percent.</summary>
        public int InstalledShieldingFactorPct { get; set; }
        public List<LowBackgroundBatch> Batches { get; set; } = new List<LowBackgroundBatch>();
        /// <summary>Bounded assay history (last <see cref="MaxAssayHistory"/> entries).</summary>
        public List<AssayRecord> AssayHistory { get; set; } = new List<AssayRecord>();
        public int NextBatchSeq { get; set; }
        public int TotalAssays { get; set; }

        public const int MaxAssayHistory = 32;
    }

    [Serializable]
    public sealed class AssayRecord
    {
        public string SampleId { get; set; } = string.Empty;
        public int Day { get; set; }
        public bool Detected { get; set; }
        public bool BelowDetectionLimit { get; set; }
        public double Confidence { get; set; }
        public string EstimatedBand { get; set; } = string.Empty;
        public int DetectionLimitBp { get; set; }
    }

    /// <summary>Sample truth is external (contamination owner); the assay only reads a band.</summary>
    public readonly struct AssaySample
    {
        public readonly string SampleId;
        public readonly int ContaminationBp;
        public readonly string MaterialProfileId;

        public AssaySample(string sampleId, int contaminationBp, string materialProfileId = "")
        {
            SampleId = sampleId ?? string.Empty;
            ContaminationBp = Math.Max(0, contaminationBp);
            MaterialProfileId = materialProfileId ?? string.Empty;
        }
    }

    public struct AssayOutcome
    {
        public bool Detected;
        public bool BelowDetectionLimit;
        public double Confidence;
        public string EstimatedBand;
        public int DetectionLimitBp;
    }

    /// <summary>
    /// Plan 138 Phase 1 — low-background metrology engine. Extends the radiation
    /// assay surface with shield-quality, calibration, and provenance inputs.
    /// Measurement only: it never purifies food/water and never mutates
    /// environmental contamination truth. Detection limits and confidence are
    /// bounded; a below-limit sample yields an indeterminate result, never a
    /// "guaranteed clean" verdict.
    /// </summary>
    public sealed class LowBackgroundLeadEngine
    {
        public const string SystemId = "low_background_metrology";

        /// <summary>Ordinary-scrap baseline background, bp. Scale anchor for all residual values.</summary>
        public const int OrdinaryBaselineBp = 100;
        /// <summary>Assay confidence is bounded to this ceiling regardless of skill/gear.</summary>
        public const double MaxConfidence = 0.97;
        /// <summary>Floors — an uncalibrated or unshielded bench still measures, just poorly.</summary>
        public const double MinConfidence = 0.05;

        private LowBackgroundMetrologyState _state = new LowBackgroundMetrologyState();
        private readonly LowBackgroundLeadCatalog _catalog;
        private readonly ILog _log;

        /// <summary>Host wires the seeded RNG for deterministic assay noise.</summary>
        public ISeededRng? Rng { get; set; }

        public LowBackgroundMetrologyState State => _state;
        public LowBackgroundLeadCatalog Catalog => _catalog;

        public LowBackgroundLeadEngine(LowBackgroundLeadCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        // ---- installation & calibration ------------------------------------

        public ActionResult InstallShieldModule(string profileId, int modules, int day)
        {
            if (!State.DetectorInstalled)
                return ActionResult.Blocked("detector_unavailable", "low_background.detector_unavailable");

            var def = _catalog.Get(profileId);
            if (def == null)
                return ActionResult.Blocked("material_not_suitable", "low_background.unknown_profile");
            if (modules <= 0)
                return ActionResult.Blocked("invalid_args", "low_background.invalid_args");
            if (def.background_activity_class == "ordinary" && modules > 0)
            {
                // Ordinary scrap may serve as mass, but it worsens the install.
            }

            for (int i = 0; i < modules; i++)
                _state.InstalledModuleProfileIds.Add(profileId);
            RecomputeCompositeShield();
            _log.Info($"[LowBackground] Installed {modules}x {profileId}; composite {InstalledResidualBp()}bp/{InstalledShieldingPct()}%");
            return ActionResult.Success("low_background.module_installed");
        }

        public ActionResult SetDetectorInstalled(bool installed)
        {
            _state.DetectorInstalled = installed;
            if (!installed)
            {
                _state.DetectorCalibrated = false;
                _state.InstalledModuleProfileIds.Clear();
                RecomputeCompositeShield();
            }
            return ActionResult.Success("low_background.detector_state_set");
        }

        public ActionResult Calibrate(int day)
        {
            if (!State.DetectorInstalled)
                return ActionResult.Blocked("detector_unavailable", "low_background.detector_unavailable");
            _state.DetectorCalibrated = true;
            _state.CalibratedOnDay = day;
            return ActionResult.Success("low_background.calibrated");
        }

        // ---- smelting batches ----------------------------------------------

        public ActionResult StartBatch(string profileId, int units, int day)
        {
            var def = _catalog.Get(profileId);
            if (def == null || units <= 0)
                return ActionResult.Blocked("material_not_suitable", "low_background.material_not_suitable");

            _state.NextBatchSeq++;
            var batch = new LowBackgroundBatch
            {
                BatchId = $"lb_batch_{day}_{_state.NextBatchSeq}",
                ProfileId = profileId,
                SourceTag = def.source_tag,
                BackgroundClass = def.background_activity_class,
                UnitsCommitted = units,
                Day = day,
                Committed = false
            };
            _state.Batches.Add(batch);
            return ActionResult.Success("low_background.batch_started");
        }

        /// <summary>
        /// Adds feedstock from a different provenance to an open batch.
        /// Classification is mass-weighted: composite residual activity decides
        /// the batch class. Contamination downgrades class but never deletes
        /// material. Transactional: either the whole addition lands or nothing
        /// changes.
        /// </summary>
        public ActionResult AddFeedstock(string batchId, string addedProfileId, int units)
        {
            var batch = _state.Batches.Find(b => string.Equals(b.BatchId, batchId, StringComparison.Ordinal));
            var added = _catalog.Get(addedProfileId);
            if (batch == null || added == null || units <= 0)
                return ActionResult.Blocked("material_not_suitable", "low_background.material_not_suitable");
            if (batch.Committed)
                return ActionResult.Blocked("batch_committed", "low_background.batch_committed");

            bool foreign = !string.Equals(batch.SourceTag, added.source_tag, StringComparison.Ordinal);
            bool downgraded = false;
            if (foreign)
            {
                // Mass-weighted blend: batch class anchor + added material residual.
                int oldBp = TypicalBpForClass(batch.BackgroundClass);
                int newBp = ClampBp(added.residual_activity_bp);
                int composite = (oldBp * batch.UnitsCommitted + newBp * units) / (batch.UnitsCommitted + units);
                string newClass = ClassifyComposite(composite);
                if (newClass != batch.BackgroundClass)
                {
                    batch.BackgroundClass = newClass;
                    downgraded = true;
                }

                if (downgraded)
                {
                    batch.Contaminated = true;
                    batch.ContaminationNote = $"cross_contamination:{added.source_tag}";
                }
            }

            batch.UnitsCommitted += units;
            _log.Info($"[LowBackground] Batch {batch.BatchId} +{units} {addedProfileId}; class={batch.BackgroundClass} downgraded={downgraded}");
            return ActionResult.Success("low_background.feedstock_added");
        }

        /// <summary>Class anchor bp used for mass-weighted blending.</summary>
        private static int TypicalBpForClass(string backgroundClass) => backgroundClass switch
        {
            "very_low" => 10,
            "low" => 25,
            _ => OrdinaryBaselineBp
        };

        /// <summary>very_low &lt; 25bp; low &lt; 50bp; otherwise ordinary.</summary>
        private static string ClassifyComposite(int compositeBp)
        {
            if (compositeBp < 25) return "very_low";
            if (compositeBp < 50) return "low";
            return "ordinary";
        }

        /// <summary>Commits a batch, applying processing loss. Material is conserved.</summary>
        public ActionResult CommitBatch(string batchId)
        {
            var batch = _state.Batches.Find(b => string.Equals(b.BatchId, batchId, StringComparison.Ordinal));
            if (batch == null)
                return ActionResult.Blocked("no_batch", "low_background.no_batch");
            if (batch.Committed)
                return ActionResult.Blocked("batch_committed", "low_background.batch_committed");

            var def = _catalog.Get(batch.ProfileId);
            int lossPct = def?.processing_loss_pct ?? 10;
            if (batch.Contaminated) lossPct = Math.Max(lossPct, 10);
            batch.UnitsLost = batch.UnitsCommitted * lossPct / 100;
            batch.UnitsCommitted -= batch.UnitsLost;
            batch.Committed = true;
            return ActionResult.Success("low_background.batch_committed");
        }

        // ---- assay -----------------------------------------------------------

        /// <summary>
        /// Runs a contamination assay. effective background = native + shield residual
        /// + environmental − shielding reduction, clamped ≥ 0. Detection limit scales
        /// with effective background; confidence scales with duration, prep, skill,
        /// and calibration. Same inputs + same seed → same outcome.
        /// </summary>
        public AssayOutcome RunAssay(
            AssaySample sample,
            int nativeDetectorBackgroundBp,
            int environmentalBackgroundBp,
            int assayTicks,
            double prepQuality,
            double operatorSkill,
            int day)
        {
            if (sample.SampleId == null || sample.ContaminationBp < 0)
                return Failure(sample, day, "low_background.sample_invalid");

            if (!State.DetectorInstalled)
                return Failure(sample, day, "low_background.detector_unavailable");

            if (!State.DetectorCalibrated)
            {
                // Uncalibrated bench: strongly degraded confidence, still bounded.
                var degraded = RunAssayInternal(sample, nativeDetectorBackgroundBp, environmentalBackgroundBp,
                    assayTicks, prepQuality, operatorSkill * 0.5, day);
                degraded.Confidence = Math.Max(MinConfidence, degraded.Confidence * 0.5);
                Record(sample, degraded, day);
                return degraded;
            }

            var outcome = RunAssayInternal(sample, nativeDetectorBackgroundBp, environmentalBackgroundBp,
                assayTicks, prepQuality, operatorSkill, day);
            Record(sample, outcome, day);
            return outcome;
        }

        private AssayOutcome RunAssayInternal(
            AssaySample sample, int nativeBp, int environmentalBp,
            int assayTicks, double prepQuality, double skill, int day)
        {
            int shieldResidual = InstalledResidualBp();
            int shielding = InstalledShieldingPct();
            int reduction = environmentalBp * shielding / 100;
            int effective = Math.Max(0, nativeBp + shieldResidual + environmentalBp - reduction);

            // Detection limit: ~ effective background + instrument noise floor.
            int noiseFloor = 10 + effective / 4;
            int detectionLimit = Math.Max(5, noiseFloor - (int)(skill * 5));

            double durationFactor = Math.Min(1.0, assayTicks / 6.0);
            double confidence = 0.35
                + 0.25 * durationFactor
                + 0.15 * Clamp01(prepQuality)
                + 0.15 * Clamp01(skill);
            confidence = Math.Max(MinConfidence, Math.Min(MaxConfidence, confidence));

            bool detected = sample.ContaminationBp >= detectionLimit;
            bool belowLimit = !detected && sample.ContaminationBp < detectionLimit;

            // Deterministic noise: same seed → same jitter. Bounded, never decisive
            // for detected samples.
            double jitter = 0.0;
            if (Rng != null)
                jitter = (Rng.NextDouble() - 0.5) * 0.04;
            confidence = Math.Max(MinConfidence, Math.Min(MaxConfidence, confidence + jitter));

            string band;
            if (belowLimit)
                band = "below_detection_limit";
            else if (sample.ContaminationBp < detectionLimit * 3)
                band = "trace";
            else if (sample.ContaminationBp < detectionLimit * 10)
                band = "moderate";
            else
                band = "heavy";

            return new AssayOutcome
            {
                Detected = detected,
                BelowDetectionLimit = belowLimit,
                Confidence = confidence,
                EstimatedBand = band,
                DetectionLimitBp = detectionLimit
            };
        }

        private static AssayOutcome Failure(AssaySample sample, int day, string code)
        {
            // Typed failure: no result recorded, caller surfaces the code.
            return new AssayOutcome
            {
                Detected = false,
                BelowDetectionLimit = false,
                Confidence = 0.0,
                EstimatedBand = code,
                DetectionLimitBp = 0
            };
        }

        private void Record(AssaySample sample, AssayOutcome outcome, int day)
        {
            _state.TotalAssays++;
            _state.AssayHistory.Add(new AssayRecord
            {
                SampleId = sample.SampleId,
                Day = day,
                Detected = outcome.Detected,
                BelowDetectionLimit = outcome.BelowDetectionLimit,
                Confidence = outcome.Confidence,
                EstimatedBand = outcome.EstimatedBand,
                DetectionLimitBp = outcome.DetectionLimitBp
            });
            if (_state.AssayHistory.Count > LowBackgroundMetrologyState.MaxAssayHistory)
                _state.AssayHistory.RemoveAt(0);
        }

        // ---- composite math --------------------------------------------------

        private void RecomputeCompositeShield()
        {
            if (_state.InstalledModuleProfileIds.Count == 0)
            {
                _state.InstalledResidualActivityBp = 0;
                _state.InstalledShieldingFactorPct = 0;
                return;
            }

            double residualSum = 0;
            double shieldingSum = 0;
            foreach (var id in _state.InstalledModuleProfileIds)
            {
                var def = _catalog.Get(id);
                if (def == null) continue;
                residualSum += ClampBp(def.residual_activity_bp);
                shieldingSum += ClampShielding(def.shielding_factor_pct);
            }
            int n = _state.InstalledModuleProfileIds.Count;
            _state.InstalledResidualActivityBp = (int)Math.Round(residualSum / n);
            _state.InstalledShieldingFactorPct = (int)Math.Round(shieldingSum / n);
        }

        public int InstalledResidualBp() => ClampBp(_state.InstalledResidualActivityBp);
        public int InstalledShieldingPct() => ClampShielding(_state.InstalledShieldingFactorPct);

        public int EffectiveBackgroundBp(int nativeDetectorBackgroundBp, int environmentalBackgroundBp)
        {
            int reduction = environmentalBackgroundBp * InstalledShieldingPct() / 100;
            return Math.Max(0, nativeDetectorBackgroundBp + InstalledResidualBp() + environmentalBackgroundBp - reduction);
        }

        private static int ClampBp(int bp) => Math.Max(0, Math.Min(200, bp));
        private static int ClampShielding(int pct) => Math.Max(0, Math.Min(95, pct));
        private static double Clamp01(double v) => Math.Max(0.0, Math.Min(1.0, v));

        // ---- persistence -------------------------------------------------------

        public LowBackgroundMetrologyState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<LowBackgroundMetrologyState>(json) ?? new LowBackgroundMetrologyState();
        }

        public void RestoreState(LowBackgroundMetrologyState? saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<LowBackgroundMetrologyState>(json) ?? new LowBackgroundMetrologyState();
            if (string.IsNullOrEmpty(_state.SystemId))
                _state.SystemId = SystemId;
        }
    }
}
