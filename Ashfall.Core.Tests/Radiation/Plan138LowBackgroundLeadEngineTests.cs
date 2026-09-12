// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Plan138Radiation
{
    /// <summary>
    /// Plan 138 Phase 1 — low-background metrology engine contract: provenance
    /// classes, bounded background composition, transactional cross-contamination
    /// downgrade, assay detection limit/confidence, determinism, save/load.
    /// Measurement only — the engine never purifies and never claims zero activity.
    /// </summary>
    public sealed class Plan138LowBackgroundLeadEngineTests
    {
        private const int NativeDetectorBp = 40;
        private const int EnvironmentalBp = 100;

        private static LowBackgroundLeadCatalog CreateCatalog()
        {
            var catalog = new LowBackgroundLeadCatalog
            {
                material_profiles =
                {
                    new LowBackgroundMaterialDef
                    {
                        id = "lb_lead_pre_industrial_salvage",
                        source_tag = "pre_industrial_salvage",
                        background_activity_class = "very_low",
                        residual_activity_bp = 8,
                        shielding_factor_pct = 62,
                        cross_contamination_sensitivity = 0.85,
                        processing_loss_pct = 12
                    },
                    new LowBackgroundMaterialDef
                    {
                        id = "lb_lead_certified_shield_stock",
                        source_tag = "certified_shield_stock",
                        background_activity_class = "low",
                        residual_activity_bp = 11,
                        shielding_factor_pct = 58,
                        cross_contamination_sensitivity = 0.5,
                        processing_loss_pct = 5
                    },
                    new LowBackgroundMaterialDef
                    {
                        id = "lead_ordinary_modern_scrap",
                        source_tag = "ordinary_modern_scrap",
                        background_activity_class = "ordinary",
                        residual_activity_bp = 90,
                        shielding_factor_pct = 40,
                        cross_contamination_sensitivity = 0.2,
                        processing_loss_pct = 8
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static LowBackgroundLeadEngine CreateEngine(LowBackgroundLeadCatalog? catalog = null, ISeededRng? rng = null)
        {
            var engine = new LowBackgroundLeadEngine(catalog ?? CreateCatalog());
            engine.Rng = rng;
            return engine;
        }

        private static void InstallCalibratedBench(LowBackgroundLeadEngine engine, int day = 1)
        {
            engine.SetDetectorInstalled(true);
            engine.InstallShieldModule("lb_lead_pre_industrial_salvage", 4, day);
            engine.Calibrate(day);
        }

        private static SeededRngStub Rng(double first) => new SeededRngStub(first);

        // ---- provenance & catalog -------------------------------------------

        [Fact]
        public void Catalog_classifies_provenance_classes()
        {
            var catalog = CreateCatalog();
            var pre = catalog.Get("lb_lead_pre_industrial_salvage")!;
            var ord = catalog.Get("lead_ordinary_modern_scrap")!;

            Assert.Equal("very_low", pre.background_activity_class);
            Assert.Equal("ordinary", ord.background_activity_class);
            Assert.True(pre.residual_activity_bp < ord.residual_activity_bp);
            // No profile claims zero residual activity.
            Assert.True(pre.residual_activity_bp > 0, "low-background must not equal zero activity");
        }

        // ---- background composition -----------------------------------------

        [Fact]
        public void Shielding_reduces_effective_background()
        {
            var engine = CreateEngine();
            InstallCalibratedBench(engine);

            int withShield = engine.EffectiveBackgroundBp(NativeDetectorBp, EnvironmentalBp);
            int baseline = NativeDetectorBp + 0 + EnvironmentalBp; // no shield modules

            Assert.True(withShield < baseline, $"with={withShield} baseline={baseline}");
        }

        [Fact]
        public void Effective_background_clamps_to_non_negative()
        {
            var engine = CreateEngine();
            engine.SetDetectorInstalled(true);
            engine.InstallShieldModule("lb_lead_pre_industrial_salvage", 8, 1);

            // Zero sources: only the shield's own residual remains — never negative.
            Assert.Equal(8, engine.EffectiveBackgroundBp(0, 0));
            Assert.True(engine.EffectiveBackgroundBp(NativeDetectorBp, EnvironmentalBp) >= 0);
        }

        [Fact]
        public void Ordinary_shield_modules_raise_residual_vs_low_background()
        {
            var engineA = CreateEngine();
            engineA.SetDetectorInstalled(true);
            engineA.InstallShieldModule("lb_lead_pre_industrial_salvage", 2, 1);

            var engineB = CreateEngine();
            engineB.SetDetectorInstalled(true);
            engineB.InstallShieldModule("lead_ordinary_modern_scrap", 2, 1);

            Assert.True(engineA.InstalledResidualBp() < engineB.InstalledResidualBp());
            Assert.Equal(8, engineA.InstalledResidualBp());
            Assert.Equal(90, engineB.InstalledResidualBp());
        }

        // ---- cross-contamination (transactional) ------------------------------

        [Fact]
        public void Cross_contamination_downgrades_batch_but_never_deletes_material()
        {
            var engine = CreateEngine();
            engine.StartBatch("lb_lead_pre_industrial_salvage", 100, 1);
            var batch = engine.State.Batches[0];

            // Massive ordinary-scrap addition into a very-low batch: pressure > 0.5.
            engine.AddFeedstock(batch.BatchId, "lead_ordinary_modern_scrap", 200);

            Assert.Equal("ordinary", batch.BackgroundClass);
            Assert.True(batch.Contaminated);
            Assert.Contains("ordinary_modern_scrap", batch.ContaminationNote);
            // Material conserved, not deleted.
            Assert.Equal(300, batch.UnitsCommitted);

            engine.CommitBatch(batch.BatchId);
            Assert.True(batch.Committed);
            Assert.True(batch.UnitsCommitted > 0, "contaminated batch must not vanish");
            Assert.True(batch.UnitsLost > 0);
        }

        [Fact]
        public void Same_provenance_feedstock_does_not_downgrade()
        {
            var engine = CreateEngine();
            engine.StartBatch("lb_lead_pre_industrial_salvage", 100, 1);
            var batch = engine.State.Batches[0];

            var result = engine.AddFeedstock(batch.BatchId, "lb_lead_pre_industrial_salvage", 50);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(string.Empty, result.FailureCode);
            Assert.Equal("very_low", batch.BackgroundClass);
            Assert.False(batch.Contaminated);
        }

        [Fact]
        public void Committed_batch_rejects_feedstock()
        {
            var engine = CreateEngine();
            engine.StartBatch("lb_lead_pre_industrial_salvage", 10, 1);
            var batch = engine.State.Batches[0];
            engine.CommitBatch(batch.BatchId);

            var result = engine.AddFeedstock(batch.BatchId, "lead_ordinary_modern_scrap", 5);

            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("batch_committed", result.FailureCode);
        }

        // ---- assay -------------------------------------------------------------

        [Fact]
        public void Assay_detects_above_limit_and_reports_band()
        {
            var engine = CreateEngine();
            InstallCalibratedBench(engine);

            var outcome = engine.RunAssay(new AssaySample("s1", 500), NativeDetectorBp, EnvironmentalBp, 6, 0.8, 0.8, 2);

            Assert.True(outcome.Detected);
            Assert.False(outcome.BelowDetectionLimit);
            Assert.Equal("heavy", outcome.EstimatedBand);
            Assert.InRange(outcome.Confidence, LowBackgroundLeadEngine.MinConfidence, LowBackgroundLeadEngine.MaxConfidence);
        }

        [Fact]
        public void Assay_below_detection_limit_is_indeterminate_never_clean()
        {
            var engine = CreateEngine();
            InstallCalibratedBench(engine);

            var outcome = engine.RunAssay(new AssaySample("s2", 3), NativeDetectorBp, EnvironmentalBp, 6, 0.8, 0.8, 2);

            Assert.False(outcome.Detected);
            Assert.True(outcome.BelowDetectionLimit, "below-limit sample must classify as indeterminate, not clean");
            Assert.Equal("below_detection_limit", outcome.EstimatedBand);
        }

        [Fact]
        public void Skill_improves_confidence_monotonically()
        {
            var engine = CreateEngine();
            InstallCalibratedBench(engine);

            var low = engine.RunAssay(new AssaySample("s3", 400), NativeDetectorBp, EnvironmentalBp, 6, 0.5, 0.1, 3);
            var high = engine.RunAssay(new AssaySample("s3", 400), NativeDetectorBp, EnvironmentalBp, 6, 0.5, 0.9, 3);

            Assert.True(high.Confidence > low.Confidence, $"high={high.Confidence} low={low.Confidence}");
        }

        [Fact]
        public void Uncalibrated_detector_degrades_confidence()
        {
            var engine = CreateEngine();
            engine.SetDetectorInstalled(true);
            engine.InstallShieldModule("lb_lead_pre_industrial_salvage", 4, 1);
            // No Calibrate() call.

            var outcome = engine.RunAssay(new AssaySample("s4", 400), NativeDetectorBp, EnvironmentalBp, 6, 0.8, 0.8, 2);

            // Still bounded above zero (measurement, not silence), but degraded.
            Assert.InRange(outcome.Confidence, LowBackgroundLeadEngine.MinConfidence, LowBackgroundLeadEngine.MaxConfidence);
            Assert.True(outcome.Confidence <= 0.6, $"uncalibrated should be weak: {outcome.Confidence}");
        }

        [Fact]
        public void Assay_without_detector_fails_typed()
        {
            var engine = CreateEngine();

            var outcome = engine.RunAssay(new AssaySample("s5", 400), NativeDetectorBp, EnvironmentalBp, 6, 0.8, 0.8, 2);

            Assert.False(outcome.Detected);
            Assert.Equal("low_background.detector_unavailable", outcome.EstimatedBand);
            Assert.Equal(0.0, outcome.Confidence);
        }

        [Fact]
        public void Assay_history_is_bounded()
        {
            var engine = CreateEngine();
            InstallCalibratedBench(engine);

            for (int i = 0; i < 50; i++)
                engine.RunAssay(new AssaySample($"s{i}", 400), NativeDetectorBp, EnvironmentalBp, 6, 0.8, 0.8, 2);

            Assert.Equal(LowBackgroundMetrologyState.MaxAssayHistory, engine.State.AssayHistory.Count);
        }

        // ---- determinism -------------------------------------------------------

        [Fact]
        public void Same_seed_replays_identical_assay()
        {
            Func<AssayOutcome> run = () =>
            {
                var engine = CreateEngine(rng: new SeededRngStub(0.42));
                InstallCalibratedBench(engine);
                return engine.RunAssay(new AssaySample("det", 350), NativeDetectorBp, EnvironmentalBp, 6, 0.8, 0.8, 5);
            };

            var a = run();
            var b = run();

            Assert.Equal(a.Confidence, b.Confidence, 10);
            Assert.Equal(a.EstimatedBand, b.EstimatedBand);
            Assert.Equal(a.DetectionLimitBp, b.DetectionLimitBp);
            Assert.Equal(a.Detected, b.Detected);
        }

        // ---- save / load -------------------------------------------------------

        [Fact]
        public void Capture_restore_round_trips_exactly()
        {
            var engine = CreateEngine();
            InstallCalibratedBench(engine);
            engine.StartBatch("lb_lead_pre_industrial_salvage", 100, 1);
            var batch = engine.State.Batches[0];
            engine.AddFeedstock(batch.BatchId, "lead_ordinary_modern_scrap", 200);
            engine.CommitBatch(batch.BatchId);
            engine.RunAssay(new AssaySample("s9", 400), NativeDetectorBp, EnvironmentalBp, 6, 0.8, 0.8, 7);

            var captured = engine.CaptureState();

            // Restore into a fresh engine (host restart path).
            var fresh = CreateEngine();
            fresh.RestoreState(captured);

            Assert.True(fresh.State.DetectorCalibrated);
            Assert.Equal(engine.State.CalibratedOnDay, fresh.State.CalibratedOnDay);
            Assert.Equal(engine.State.InstalledResidualActivityBp, fresh.State.InstalledResidualActivityBp);
            Assert.Equal(engine.State.InstalledShieldingFactorPct, fresh.State.InstalledShieldingFactorPct);
            Assert.Equal(engine.State.Batches.Count, fresh.State.Batches.Count);
            Assert.Equal("ordinary", fresh.State.Batches[0].BackgroundClass);
            Assert.Equal(engine.State.AssayHistory.Count, fresh.State.AssayHistory.Count);
            Assert.Equal(engine.State.AssayHistory[0].Confidence, fresh.State.AssayHistory[0].Confidence, 10);

            // Old-save baseline: empty state restores to factory defaults, no free tech.
            var fresh2 = CreateEngine();
            fresh2.RestoreState(new LowBackgroundMetrologyState());
            Assert.False(fresh2.State.DetectorInstalled);
            Assert.False(fresh2.State.DetectorCalibrated);
            Assert.Equal(0, fresh2.State.InstalledModuleProfileIds.Count);
        }

        [Fact]
        public void Catalog_loader_reads_streaming_assets_json()
        {
            var dataDir = Path.Combine(Directory.GetCurrentDirectory(),
                "Assets", "StreamingAssets", "Data");
            if (!File.Exists(Path.Combine(dataDir, "low_background_lead_catalog.json")))
            {
                // Repo-root relative fallback (test runner cwd differs).
                var root = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..");
                dataDir = Path.GetFullPath(Path.Combine(root, "Assets", "StreamingAssets", "Data"));
            }

            var catalog = LowBackgroundLeadCatalogLoader.Load(dataDir, new FileIoStub());
            Assert.Equal(1, catalog.schema_version);
            Assert.True(catalog.All.Count >= 4, $"expected >=4 profiles, got {catalog.All.Count}");
            Assert.NotNull(catalog.Get("lb_lead_pre_industrial_salvage"));
            Assert.NotNull(catalog.Get("lead_ordinary_modern_scrap"));
        }
    }

    /// <summary>Deterministic RNG stub: returns the scripted value, then the script value forever.</summary>
    internal sealed class SeededRngStub : ISeededRng
    {
        private readonly double _nextDouble;
        public int Seed => 138;

        public SeededRngStub(double nextDouble) { _nextDouble = nextDouble; }

        public int Next(int minInclusive, int maxExclusive) => minInclusive;
        public float NextFloat() => (float)_nextDouble;
        public double NextDouble() => _nextDouble;
    }

    /// <summary>Minimal IFileIO for catalog loader tests (abstract members only).</summary>
    internal sealed class FileIoStub : IFileIO
    {
        public bool DirectoryExists(string path) => Directory.Exists(path);
        public bool FileExists(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
        public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
        public string Combine(params string[] parts) => Path.Combine(parts);
    }
}
