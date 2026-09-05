using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 79 — geodetic survey deterministic, boundary, and network tests.
    /// Uses a synthetic catalog for exact geometric control, plus one
    /// shipped-catalog load test proving 16 authored survey points parse.
    /// </summary>
    public class GeodeticSurveyEngineTests
    {
        private static GeodeticSurveyCatalog MakeCatalog()
        {
            var catalog = new GeodeticSurveyCatalog
            {
                survey_points = new List<SurveyPointDef>
                {
                    new SurveyPointDef { survey_point_id = "pt_a", world_node_id = "loc_test_a", display_name = "Point A", point_type = "peak", elevation_m = 1000f, baseline_quality = 0.9f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string>() },
                    new SurveyPointDef { survey_point_id = "pt_b", world_node_id = "loc_test_b", display_name = "Point B", point_type = "ridge", elevation_m = 800f, baseline_quality = 0.8f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string> { "route_shortcut_x" } },
                    new SurveyPointDef { survey_point_id = "pt_c", world_node_id = "loc_test_c", display_name = "Point C", point_type = "tower_ruin", elevation_m = 600f, baseline_quality = 0.7f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string> { "route_shortcut_x" } },
                    // Degenerate pair: identical elevation for baseline-boundary tests.
                    new SurveyPointDef { survey_point_id = "pt_flat1", world_node_id = "loc_test_d", display_name = "Flat 1", point_type = "rail_marker", elevation_m = 500f, baseline_quality = 0.7f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string>() },
                    new SurveyPointDef { survey_point_id = "pt_flat2", world_node_id = "loc_test_e", display_name = "Flat 2", point_type = "rail_marker", elevation_m = 500f, baseline_quality = 0.7f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string>() }
                },
                survey_equipment = new SurveyEquipmentDef { theodolite_base_error_degrees = 0.05f },
                weather_modifiers = new Dictionary<string, WeatherModifierDef>(StringComparer.OrdinalIgnoreCase)
                {
                    { "clear", new WeatherModifierDef { error_multiplier = 1.0f } },
                    { "heavy_ash", new WeatherModifierDef { error_multiplier = 4.0f } }
                },
                triangulation = new TriangulationParamsDef
                {
                    min_baseline_length_m = 150f,
                    network_accuracy_floor = 0.1f,
                    network_accuracy_max = 1.0f
                },
                navigation_effects = new NavigationEffectsDef
                {
                    drift_reduction_per_accuracy = 0.5f,
                    speed_bonus_per_accuracy = 0.15f,
                    max_drift_reduction = 1.0f,
                    max_speed_bonus = 0.25f
                }
            };
            return catalog;
        }

        private static GeodeticSurveyEngine Create(out List<string> consumed, GeodeticSurveyCatalog? catalog = null, int seed = 77)
        {
            consumed = new List<string>();
            var engine = new GeodeticSurveyEngine(catalog ?? MakeCatalog(), new SeededRng(seed));
            return engine;
        }

        private static Func<string, int, bool> ConsumeFrom(List<string> ledger)
        {
            return (itemId, amount) =>
            {
                for (int i = 0; i < amount; i++) ledger.Add(itemId);
                return true;
            };
        }

        private static void EstablishAll(GeodeticSurveyEngine engine, params string[] pointIds)
        {
            var ledger = new List<string>();
            foreach (var id in pointIds)
            {
                var r = engine.EstablishMonument(id, day: 1, consumeItems: ConsumeFrom(ledger));
                Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            }
        }

        // ─── Monument tests ───

        [Fact]
        public void EstablishMonument_ConsumesRequiredItems_AndPersists()
        {
            var engine = Create(out _);
            var ledger = new List<string>();
            var r = engine.EstablishMonument("pt_a", day: 3, consumeItems: ConsumeFrom(ledger));

            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.NotNull(engine.FindMonumentBySurveyPoint("pt_a"));
            Assert.Equal(3, engine.FindMonumentBySurveyPoint("pt_a")!.establishedDay);
            Assert.Single(ledger); // exactly one datum plate
        }

        [Fact]
        public void EstablishMonument_Duplicate_Blocks()
        {
            var engine = Create(out _);
            EstablishAll(engine, "pt_a");
            var ledger = new List<string>();
            var r = engine.EstablishMonument("pt_a", day: 4, consumeItems: ConsumeFrom(ledger));
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Empty(ledger); // no items consumed on failed establish
        }

        // ─── Determinism tests ───

        [Fact]
        public void SameSeed_SameInputs_SameObservation()
        {
            var e1 = Create(out _, seed: 555);
            var e2 = Create(out _, seed: 555);
            EstablishAll(e1, "pt_a");
            EstablishAll(e2, "pt_a");

            var o1 = e1.Observe("pt_a", "pt_b", "clear", 0.5f);
            var o2 = e2.Observe("pt_a", "pt_b", "clear", 0.5f);

            Assert.Equal(o1.horizontalAngleDegrees, o2.horizontalAngleDegrees, 6);
            Assert.Equal(o1.verticalAngleDegrees, o2.verticalAngleDegrees, 6);
            Assert.Equal(o1.uncertaintyDegrees, o2.uncertaintyDegrees, 6);
        }

        [Fact]
        public void AngleNormalization_AlwaysWithinZeroTo360()
        {
            var engine = Create(out _);
            EstablishAll(engine, "pt_a");
            for (int i = 0; i < 50; i++)
            {
                var o = engine.Observe("pt_a", "pt_b", "clear", 0.5f);
                Assert.InRange(o.horizontalAngleDegrees, 0f, 360f);
                Assert.InRange(o.verticalAngleDegrees, -90f, 90f);
                Assert.True(o.uncertaintyDegrees > 0f, "uncertainty must be strictly positive");
            }
        }

        [Fact]
        public void WeatherError_Deterministic_AndHeavierWeatherIncreasesUncertainty()
        {
            var e1 = Create(out _, seed: 321);
            EstablishAll(e1, "pt_a");
            var clear = e1.Observe("pt_a", "pt_b", "clear", 0.5f);
            var ash = e1.Observe("pt_a", "pt_b", "heavy_ash", 0.5f);

            Assert.True(ash.uncertaintyDegrees > clear.uncertaintyDegrees,
                $"heavy ash uncertainty {ash.uncertaintyDegrees} must exceed clear {clear.uncertaintyDegrees}");

            // Same seed replay reproduces both readings exactly.
            var e2 = Create(out _, seed: 321);
            EstablishAll(e2, "pt_a");
            var clear2 = e2.Observe("pt_a", "pt_b", "clear", 0.5f);
            var ash2 = e2.Observe("pt_a", "pt_b", "heavy_ash", 0.5f);
            Assert.Equal(clear.uncertaintyDegrees, clear2.uncertaintyDegrees, 6);
            Assert.Equal(ash.uncertaintyDegrees, ash2.uncertaintyDegrees, 6);
        }

        // ─── Triangulation tests ───

        [Fact]
        public void TriangleResolves_WithAccuracy_AndUnlocksShortcut()
        {
            var engine = Create(out _);
            EstablishAll(engine, "pt_a", "pt_b", "pt_c");
            engine.Observe("pt_a", "pt_b", "clear");
            engine.Observe("pt_b", "pt_c", "clear");
            engine.Observe("pt_c", "pt_a", "clear");

            var tri = engine.TryResolveTriangle("pt_a", "pt_b", "pt_c");

            Assert.NotNull(tri);
            Assert.True(tri!.accuracy > 0f);
            Assert.Contains("route_shortcut_x", engine.UnlockedShortcuts);
        }

        [Fact]
        public void ShortcutUnlocked_ExactlyOnce_OnDuplicateResolve()
        {
            var engine = Create(out _);
            EstablishAll(engine, "pt_a", "pt_b", "pt_c");

            var first = engine.TryResolveTriangle("pt_a", "pt_b", "pt_c");
            var second = engine.TryResolveTriangle("pt_a", "pt_b", "pt_c");

            Assert.NotNull(first);
            Assert.NotNull(second); // idempotent — returns the existing triangle
            Assert.Single(engine.UnlockedShortcuts);
            Assert.Single(engine.State.resolvedTriangles);
        }

        [Fact]
        public void MissingMonument_RejectsTriangle()
        {
            var engine = Create(out _);
            EstablishAll(engine, "pt_a", "pt_b"); // pt_c has no monument
            Assert.Null(engine.TryResolveTriangle("pt_a", "pt_b", "pt_c"));
        }

        [Fact]
        public void ZeroLengthBaseline_DegenerateTriangle_Rejected()
        {
            // pt_flat1 / pt_flat2 share elevation → synthetic baseline 100m,
            // below the catalog's min_baseline_length_m of 150m.
            var engine = Create(out _);
            EstablishAll(engine, "pt_flat1", "pt_flat2", "pt_a");
            Assert.Null(engine.TryResolveTriangle("pt_flat1", "pt_flat2", "pt_a"));
        }

        [Fact]
        public void DamagedMonument_ToZero_Deactivates_AndRejectsTriangle()
        {
            var engine = Create(out _);
            EstablishAll(engine, "pt_a", "pt_b", "pt_c");

            engine.DamageMonument("monument_pt_a", 1.0f);

            Assert.False(engine.FindMonumentBySurveyPoint("pt_a")!.isActive);
            Assert.Null(engine.TryResolveTriangle("pt_a", "pt_b", "pt_c"));
            // Network accuracy falls to the floor when no triangles resolve.
            Assert.Equal(0.1f, engine.NetworkAccuracy, 3);
        }

        // ─── Navigation effects ───

        [Fact]
        public void TravelModifiers_ZeroForUnsurveyed_BoundedForSurveyed()
        {
            var engine = Create(out _);
            Assert.Equal(0f, engine.GetDriftReduction("corridor_x"));
            Assert.Equal(0f, engine.GetSpeedBonus("corridor_x"));

            engine.MarkCorridorSurveyed("corridor_x");
            float drift = engine.GetDriftReduction("corridor_x");
            float speed = engine.GetSpeedBonus("corridor_x");
            Assert.InRange(drift, 0f, 1.0f);
            Assert.InRange(speed, 0f, 0.25f);

            // Stable across repeated reads (no drift between calls).
            Assert.Equal(drift, engine.GetDriftReduction("corridor_x"), 6);
            Assert.Equal(speed, engine.GetSpeedBonus("corridor_x"), 6);
        }

        // ─── Shipped catalog (data authority) ───

        [Fact]
        public void ShippedSurveyCatalog_Loads_With16SurveyPoints()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dataDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!Directory.Exists(dataDir))
                dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");

            var catalog = GeodeticSurveyCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(16, catalog.survey_points.Count);
            var ids = new HashSet<string>();
            foreach (var p in catalog.survey_points)
            {
                Assert.True(ids.Add(p.survey_point_id), $"duplicate survey_point_id {p.survey_point_id}");
                Assert.StartsWith("loc_", p.world_node_id);
                Assert.True(p.baseline_quality > 0f && p.baseline_quality <= 1f);
            }
        }
    }
}
