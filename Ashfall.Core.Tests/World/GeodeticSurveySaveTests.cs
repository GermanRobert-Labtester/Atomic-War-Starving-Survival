using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 79 — geodetic survey save/restore parity tests.
    /// Pins: network state round-trips; unlock-once survives restore;
    /// restore is silent; recapture is normalized.
    /// </summary>
    public class GeodeticSurveySaveTests
    {
        private static GeodeticSurveyEngine CreateEngine(int seed = 88)
        {
            var catalog = new GeodeticSurveyCatalog
            {
                survey_points = new List<SurveyPointDef>
                {
                    new SurveyPointDef { survey_point_id = "pt_a", world_node_id = "loc_a", display_name = "A", point_type = "peak", elevation_m = 1000f, baseline_quality = 0.9f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string> { "route_shortcut_y" } },
                    new SurveyPointDef { survey_point_id = "pt_b", world_node_id = "loc_b", display_name = "B", point_type = "ridge", elevation_m = 800f, baseline_quality = 0.8f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string> { "route_shortcut_y" } },
                    new SurveyPointDef { survey_point_id = "pt_c", world_node_id = "loc_c", display_name = "C", point_type = "tower_ruin", elevation_m = 600f, baseline_quality = 0.7f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string>() }
                },
                triangulation = new TriangulationParamsDef { min_baseline_length_m = 150f, network_accuracy_floor = 0.1f, network_accuracy_max = 1.0f }
            };
            return new GeodeticSurveyEngine(catalog, new SeededRng(seed));
        }

        private static bool ConsumeAny(string itemId, int amount) => true;

        private static void BuildNetwork(GeodeticSurveyEngine engine)
        {
            foreach (var id in new[] { "pt_a", "pt_b", "pt_c" })
                engine.EstablishMonument(id, 1, ConsumeAny);
            engine.Observe("pt_a", "pt_b", "clear");
            engine.Observe("pt_b", "pt_c", "clear");
            engine.Observe("pt_c", "pt_a", "clear");
            engine.TryResolveTriangle("pt_a", "pt_b", "pt_c");
            engine.MarkCorridorSurveyed("corridor_1");
        }

        [Fact]
        public void SaveRestore_NetworkParity()
        {
            var engine = CreateEngine();
            BuildNetwork(engine);
            var captured = engine.CaptureState();

            var restored = new GeodeticSurveyEngine(engine.Catalog, new SeededRng(77));
            restored.RestoreState(captured);

            Assert.Equal(engine.NetworkAccuracy, restored.NetworkAccuracy, 6);
            Assert.Equal(engine.State.monuments.Count, restored.State.monuments.Count);
            Assert.Equal(engine.State.observations.Count, restored.State.observations.Count);
            Assert.Equal(engine.State.resolvedTriangles.Count, restored.State.resolvedTriangles.Count);
            Assert.Equal(engine.UnlockedShortcuts, restored.UnlockedShortcuts);
            Assert.Single(restored.State.surveyedCorridorIds);
        }

        [Fact]
        public void ShortcutUnlocked_ExactlyOnce_AcrossRestore()
        {
            var engine = CreateEngine();
            BuildNetwork(engine);
            var captured = engine.CaptureState();

            var restored = new GeodeticSurveyEngine(engine.Catalog, new SeededRng(77));
            restored.RestoreState(captured);

            // Re-resolving the same triangle after restore must not double-unlock.
            restored.TryResolveTriangle("pt_a", "pt_b", "pt_c");
            Assert.Single(restored.UnlockedShortcuts);
            Assert.Single(restored.State.resolvedTriangles);
        }

        [Fact]
        public void Restore_IsSilent_NoEvents()
        {
            var engine = CreateEngine();
            BuildNetwork(engine);
            var captured = engine.CaptureState();

            int monumentEvents = 0, triangleEvents = 0, shortcutEvents = 0, changedEvents = 0;
            var restored = new GeodeticSurveyEngine(engine.Catalog, new SeededRng(77));
            restored.OnMonumentEstablished += _ => monumentEvents++;
            restored.OnTriangleResolved += _ => triangleEvents++;
            restored.OnShortcutUnlocked += _ => shortcutEvents++;
            restored.OnSurveyChanged += () => changedEvents++;
            restored.RestoreState(captured);

            Assert.Equal(0, monumentEvents);
            Assert.Equal(0, triangleEvents);
            Assert.Equal(0, shortcutEvents);
            Assert.Equal(0, changedEvents);
        }

        [Fact]
        public void Restore_NonZeroState_ContinuesIdenticalTrajectory()
        {
            var control = CreateEngine(seed: 44);
            BuildNetwork(control);
            control.Observe("pt_a", "pt_c", "heavy_ash");

            var rng = new SeededRng(44);
            var saved = new GeodeticSurveyEngine(control.Catalog, rng);
            BuildNetwork(saved);
            var capturedAtMark = saved.CaptureState();

            var restored = new GeodeticSurveyEngine(saved.Catalog, rng);
            restored.RestoreState(capturedAtMark);
            restored.Observe("pt_a", "pt_c", "heavy_ash");

            // Same seed continuation => identical new observation.
            Assert.Equal(control.State.observations[3].horizontalAngleDegrees,
                         restored.State.observations[3].horizontalAngleDegrees, 6);
            Assert.Equal(control.State.observations[3].uncertaintyDegrees,
                         restored.State.observations[3].uncertaintyDegrees, 6);
        }

        [Fact]
        public void Recapture_AfterRestore_IsNormalized()
        {
            var engine = CreateEngine();
            BuildNetwork(engine);
            var first = engine.CaptureState();

            var restored = new GeodeticSurveyEngine(engine.Catalog, new SeededRng(77));
            restored.RestoreState(first);
            var second = restored.CaptureState();

            Assert.Equal(
                new SystemTextJsonSerializer().Serialize(first),
                new SystemTextJsonSerializer().Serialize(second));
        }
    }
}
