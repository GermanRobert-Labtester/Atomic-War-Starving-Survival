// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Plan139World
{
    /// <summary>
    /// Plan 139 Phase 1 — repeat-pass InSAR deformation intelligence contract:
    /// compatible pass pairing, geometry rejection, stable/subsidence/
    /// accelerating/abrupt classification, weather + skill effects, excavation
    /// and travel projections, save/load, deterministic replay. Intelligence
    /// only — never terrain truth, never deterministic quake prediction.
    /// </summary>
    public sealed class Plan139InSarDeformationTests
    {
        private static InSarGeodesyCatalog CreateCatalog()
        {
            var catalog = new InSarGeodesyCatalog
            {
                sensor_profiles =
                {
                    new InSarSensorDef
                    {
                        id = "ground_repeat_radar",
                        display_name = "Ground Repeat-Pass Radar",
                        coverage_width_km = 8,
                        nominal_resolution_m = 6,
                        repeat_pass_interval_days = 2,
                        coherence_decay_rate_bp = 10,
                        weather_decorrelation_modifier_bp = 60,
                        minimum_detectable_deformation_mm = 8,
                        processing_skill_modifier_bp = 40,
                        terrain_decorrelation_tags = { "forest" }
                    },
                    new InSarSensorDef
                    {
                        id = "uav_radar_pod",
                        display_name = "UAV Radar Pod",
                        coverage_width_km = 14,
                        nominal_resolution_m = 9,
                        repeat_pass_interval_days = 3,
                        coherence_decay_rate_bp = 16,
                        weather_decorrelation_modifier_bp = 40,
                        minimum_detectable_deformation_mm = 5,
                        processing_skill_modifier_bp = 10,
                        terrain_decorrelation_tags = { "forest", "canopy" }
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static InSarDeformationEngine CreateEngine(ISeededRng? rng = null)
        {
            var engine = new InSarDeformationEngine(CreateCatalog());
            engine.Rng = rng;
            return engine;
        }

        private static void Record(
            InSarDeformationEngine engine,
            string sector,
            int day,
            int quality,
            int weather = 100,
            string reference = "ref_a",
            string sensor = "ground_repeat_radar")
        {
            engine.RecordSurveyPass(sensor, sector, day, quality, weather, reference);
        }

        // ---- pass pairing & geometry ---------------------------------------

        [Fact]
        public void Unknown_sensor_is_a_typed_failure()
        {
            var engine = CreateEngine();
            var result = engine.RecordSurveyPass("no_such_sensor", "s1", 1, 50, 100, "ref_a");
            Assert.True(result.IsFailure);
            Assert.Equal("processing_data_missing", result.FailureCode);
        }

        [Fact]
        public void Single_pass_is_insufficient_for_a_map()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 60);

            var result = engine.ProcessSector("s1", 0.5);

            Assert.True(result.IsFailure);
            Assert.Equal("insufficient_survey_passes", result.FailureCode);
            Assert.Null(engine.GetSummary("s1"));
        }

        [Fact]
        public void Unsaved_sector_has_no_intelligence()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 60);
            Record(engine, "s1", 11, 55);

            var result = engine.ProcessSector("s2", 0.5);

            Assert.True(result.IsFailure);
            Assert.Equal("sector_not_surveyed", result.FailureCode);
            Assert.Equal(InSarClassification.Unsurveyed, engine.GetTravelRisk("s2"));
        }

        [Fact]
        public void Incompatible_reference_geometry_is_rejected()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 60, 100, "ref_a");
            Record(engine, "s1", 11, 55, 100, "ref_b");

            var result = engine.ProcessSector("s1", 0.5);

            Assert.True(result.IsFailure);
            Assert.Equal("pass_geometry_incompatible", result.FailureCode);
        }

        // ---- classification -------------------------------------------------

        [Fact]
        public void Matching_passes_classify_stable()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 60);
            Record(engine, "s1", 11, 60);

            var result = engine.ProcessSector("s1", 0.5);

            Assert.True(result.IsSuccess);
            var summary = engine.GetSummary("s1")!;
            Assert.Equal(InSarClassification.Stable, summary.Classification);
            Assert.Equal(0.0, summary.RelativeDisplacementMm, 3);
            Assert.Equal(InSarClassification.Stable, engine.GetTravelRisk("s1"));
        }

        [Fact]
        public void Falling_quality_detects_slow_subsidence()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 70);
            Record(engine, "s1", 21, 45);

            Assert.True(engine.ProcessSector("s1", 0.5).IsSuccess);

            var summary = engine.GetSummary("s1")!;
            Assert.Equal(InSarClassification.SlowSubsidence, summary.Classification);
            Assert.True(summary.RelativeDisplacementMm < 0);
            Assert.True(summary.TrendVelocityMmPerDay <= -InSarDeformationEngine.StableVelocityMmPerDay);
            Assert.Equal("subsiding", engine.GetTravelRisk("s1"));
        }

        [Fact]
        public void Sharp_quality_jump_classifies_abrupt_deformation()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 90);
            Record(engine, "s1", 4, 10);

            Assert.True(engine.ProcessSector("s1", 0.5).IsSuccess);

            Assert.Equal(InSarClassification.AbruptDeformation, engine.GetSummary("s1")!.Classification);
        }

        [Fact]
        public void Accelerating_subsidence_is_distinguished_from_slow()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 0, 60);
            Record(engine, "s1", 10, 52);
            Record(engine, "s1", 20, 32);

            Assert.True(engine.ProcessSector("s1", 0.5).IsSuccess);

            var summary = engine.GetSummary("s1")!;
            Assert.Equal(InSarClassification.AcceleratingSubsidence, summary.Classification);
            Assert.True(summary.RelativeDisplacementMm < InSarDeformationEngine.AbruptDisplacementMm);
        }

        // ---- coherence, weather, skill -------------------------------------

        [Fact]
        public void Poor_weather_and_long_baseline_reject_low_coherence()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 60, weather: 0);
            Record(engine, "s1", 31, 55, weather: 0);

            var result = engine.ProcessSector("s1", 0.5);

            Assert.True(result.IsFailure);
            Assert.Equal("low_coherence", result.FailureCode);
            var summary = engine.GetSummary("s1")!;
            Assert.Equal(InSarClassification.LowConfidence, summary.Classification);
            Assert.Equal("unknown", engine.GetTravelRisk("s1"));
        }

        [Fact]
        public void Weather_quality_raises_coherence()
        {
            var clear = CreateEngine();
            Record(clear, "s1", 1, 60, weather: 100);
            Record(clear, "s1", 11, 55, weather: 100);
            Assert.True(clear.ProcessSector("s1", 0.5).IsSuccess);

            var hazy = CreateEngine();
            Record(hazy, "s1", 1, 60, weather: 40);
            Record(hazy, "s1", 11, 55, weather: 40);
            Assert.True(hazy.ProcessSector("s1", 0.5).IsSuccess);

            Assert.True(clear.GetSummary("s1")!.Coherence > hazy.GetSummary("s1")!.Coherence);
        }

        [Fact]
        public void Processing_skill_improves_confidence_but_stays_bounded()
        {
            var low = CreateEngine();
            Record(low, "s1", 1, 60);
            Record(low, "s1", 11, 55);
            Assert.True(low.ProcessSector("s1", 0.0).IsSuccess);

            var high = CreateEngine();
            Record(high, "s1", 1, 60);
            Record(high, "s1", 11, 55);
            Assert.True(high.ProcessSector("s1", 1.0).IsSuccess);

            Assert.True(high.GetSummary("s1")!.Confidence > low.GetSummary("s1")!.Confidence);
            Assert.True(high.GetSummary("s1")!.Confidence <= InSarDeformationEngine.MaxConfidence);
            Assert.True(low.GetSummary("s1")!.Confidence >= InSarDeformationEngine.MinConfidence);
        }

        [Fact]
        public void Terrain_decorrelation_lowers_coherence()
        {
            var plain = CreateEngine();
            Record(plain, "s1", 1, 60);
            Record(plain, "s1", 11, 55);
            Assert.True(plain.ProcessSector("s1", 0.5).IsSuccess);

            var forest = CreateEngine();
            Record(forest, "s1", 1, 60);
            Record(forest, "s1", 11, 55);
            Assert.True(forest.ProcessSector("s1", 0.5, new[] { "forest" }).IsSuccess);

            Assert.True(forest.GetSummary("s1")!.Coherence < plain.GetSummary("s1")!.Coherence);
        }

        // ---- projections ----------------------------------------------------

        [Fact]
        public void Excavation_warning_escalates_with_trend()
        {
            var engine = CreateEngine();
            Record(engine, "stable", 0, 60);
            Record(engine, "stable", 10, 60);
            Assert.True(engine.ProcessSector("stable", 0.5).IsSuccess);

            Record(engine, "fast", 0, 60);
            Record(engine, "fast", 10, 52);
            Record(engine, "fast", 20, 32);
            Assert.True(engine.ProcessSector("fast", 0.5).IsSuccess);

            Assert.Contains("No significant", engine.GetExcavationWarning("stable"));
            Assert.Contains("Accelerating", engine.GetExcavationWarning("fast"));
        }

        [Fact]
        public void Unknown_sector_warning_is_inconclusive_not_safe()
        {
            var engine = CreateEngine();
            Assert.Contains("No deformation intelligence", engine.GetExcavationWarning("never_surveyed"));
        }

        // ---- persistence & determinism -------------------------------------

        [Fact]
        public void Save_load_round_trip_preserves_intelligence()
        {
            var engine = CreateEngine();
            Record(engine, "s1", 1, 70);
            Record(engine, "s1", 21, 45);
            Assert.True(engine.ProcessSector("s1", 0.5).IsSuccess);
            var before = engine.GetSummary("s1")!;

            var saved = engine.CaptureState();
            var restored = CreateEngine();
            restored.RestoreState(saved);

            var after = restored.GetSummary("s1")!;
            Assert.Equal(before.Classification, after.Classification);
            Assert.Equal(before.RelativeDisplacementMm, after.RelativeDisplacementMm, 6);
            Assert.Equal(before.Coherence, after.Coherence, 6);
            Assert.Equal(before.Confidence, after.Confidence, 6);
            Assert.Equal(engine.PassesForSector("s1").Count, restored.PassesForSector("s1").Count);
        }

        [Fact]
        public void Split_run_yields_same_map()
        {
            var continuous = CreateEngine();
            Record(continuous, "s1", 0, 60);
            Record(continuous, "s1", 10, 52);
            Record(continuous, "s1", 20, 32);
            Assert.True(continuous.ProcessSector("s1", 0.7).IsSuccess);
            var expected = continuous.GetSummary("s1")!;

            var split = CreateEngine();
            Record(split, "s1", 0, 60);
            Record(split, "s1", 10, 52);
            Assert.True(split.ProcessSector("s1", 0.7).IsSuccess);
            var midSave = split.CaptureState();

            var reloaded = CreateEngine();
            reloaded.RestoreState(midSave);
            Record(reloaded, "s1", 20, 32);
            Assert.True(reloaded.ProcessSector("s1", 0.7).IsSuccess);

            var actual = reloaded.GetSummary("s1")!;
            Assert.Equal(expected.Classification, actual.Classification);
            Assert.Equal(expected.RelativeDisplacementMm, actual.RelativeDisplacementMm, 6);
        }

        [Fact]
        public void Pristine_deformation_does_not_require_rng()
        {
            // Processing is deterministic even with no RNG injected --- the map
            // depends only on observation inputs, not wall-clock or hash order.
            var a = CreateEngine();
            var b = CreateEngine();
            foreach (var engine in new[] { a, b })
            {
                Record(engine, "s1", 1, 66);
                Record(engine, "s1", 9, 61);
                Record(engine, "s1", 17, 44);
                Assert.True(engine.ProcessSector("s1", 0.4).IsSuccess);
            }

            var sa = a.GetSummary("s1")!;
            var sb = b.GetSummary("s1")!;
            Assert.Equal(sa.Classification, sb.Classification);
            Assert.Equal(sa.RelativeDisplacementMm, sb.RelativeDisplacementMm, 9);
            Assert.Equal(sa.Confidence, sb.Confidence, 9);
        }
    }
}
