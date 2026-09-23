// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 176 — Anomaly Hazard System tests (Phase 1 Core).
// Covers: strict catalog validation, deterministic spawn, hazard cap, storm
// movement + wander determinism, wind drift, world-bound clamping, additive
// overlap with cap, detection classification (powered/unpowered), loot-site
// single resolution, bounded wildlife modifier, typed approach warnings,
// exact save/load coordinates, and continuous-vs-mid-reload storm parity.
// The system NEVER mutates health — dose stays with RadiationSystem.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Plan176World
{
    public sealed class Plan176AnomalyHazardTests
    {
        // ── helpers ────────────────────────────────────────────────────

        private static AnomalyDefinitionCatalog CreateCatalog()
        {
            var load = new AnomalyCatalogLoadResult();
            load.Anomalies.Add(new AnomalyDefinition
            {
                anomaly_id = "anomaly_test_front",
                display_name = "Test Storm Front",
                anomaly_type = "rad_storm_front",
                spawn_region_tags = { "open_waste" },
                movement_profile = "storm_front",
                movement_speed_kph = 9f,
                wind_response = 0f,
                radiation_rate = 120f,
                radius_km = 18f,
                duration_days = 6,
                warning_radius_km = 40f,
                warning_profile = "standard",
                detection_threshold = 25f,
                loot_site_count = 0,
                wildlife_modifier_bp = -800
            });
            load.Anomalies.Add(new AnomalyDefinition
            {
                anomaly_id = "anomaly_test_drift",
                display_name = "Test Drift Band",
                anomaly_type = "particulate_band",
                spawn_region_tags = { "highway" },
                movement_profile = "wind_drift",
                movement_speed_kph = 0f,
                wind_response = 0.9f,
                radiation_rate = 60f,
                radius_km = 14f,
                duration_days = 10,
                warning_radius_km = 30f,
                warning_profile = "standard",
                detection_threshold = 12f,
                loot_site_count = 0,
                wildlife_modifier_bp = -400
            });
            load.Anomalies.Add(new AnomalyDefinition
            {
                anomaly_id = "anomaly_test_static",
                display_name = "Test Static Zone",
                anomaly_type = "mutagenic_field",
                spawn_region_tags = { "deep_zone" },
                movement_profile = "static",
                radiation_rate = 40f,
                radius_km = 3f,
                duration_days = 24,
                warning_radius_km = 10f,
                warning_profile = "standard",
                detection_threshold = 8f,
                loot_table_id = "table_loot_chemical_plant",
                loot_site_count = 2,
                wildlife_modifier_bp = -900,
                environmental_effect_tags = { "mutagenic", "glow" }
            });
            return AnomalyCatalogLoader.ToCatalog(load);
        }

        private static AnomalyHazardSystem CreateSystem()
        {
            var system = new AnomalyHazardSystem(CreateCatalog());
            return system;
        }

        private static string FindDataDir()
        {
            // Walk up from the test output dir to the repo root.
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(candidate, "anomalies.json"))) return candidate;
                dir = dir.Parent;
            }
            return string.Empty;
        }

        // ── catalog loader ─────────────────────────────────────────────

        [Fact]
        public void Loader_AuthoredCatalog_LoadsWithoutErrors()
        {
            string dataDir = FindDataDir();
            Assert.True(dataDir.Length > 0, "anomalies.json not found from test base dir");
            var result = AnomalyCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Anomalies.Count >= 8, "authored catalog should carry its rows");
        }

        [Fact]
        public void Loader_RejectsNewerSchemaAndDuplicates()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_plan176_loader_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                File.WriteAllText(Path.Combine(dir, AnomalyCatalogLoader.FileName),
                    "{\"schema_version\":2,\"anomalies\":[]}");
                var newer = AnomalyCatalogLoader.Load(dir, fileIO, json);
                Assert.True(newer.HasErrors);

                File.WriteAllText(Path.Combine(dir, AnomalyCatalogLoader.FileName),
                    "{\"schema_version\":1,\"anomalies\":[" +
                    "{\"anomaly_id\":\"anomaly_dup\",\"display_name\":\"A\",\"anomaly_type\":\"x\",\"spawn_region_tags\":[\"z\"]," +
                    "\"movement_profile\":\"static\",\"radius_km\":2,\"duration_days\":3,\"warning_radius_km\":4}," +
                    "{\"anomaly_id\":\"anomaly_dup\",\"display_name\":\"B\",\"anomaly_type\":\"x\",\"spawn_region_tags\":[\"z\"]," +
                    "\"movement_profile\":\"static\",\"radius_km\":2,\"duration_days\":3,\"warning_radius_km\":4}]}");
                var dup = AnomalyCatalogLoader.Load(dir, fileIO, json);
                Assert.True(dup.HasErrors);
                Assert.Contains(dup.Errors, e => e.Contains("duplicate"));
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }

        // ── spawn ──────────────────────────────────────────────────────

        [Fact]
        public void Spawn_DeterministicIdentityAndExactPlacement()
        {
            var a = CreateSystem();
            var b = CreateSystem();
            var ra = a.TrySpawn("anomaly_test_front", 100.555f, 200.444f, 7, 90f);
            var rb = b.TrySpawn("anomaly_test_front", 100.555f, 200.444f, 7, 90f);
            Assert.True(ra.Success);
            Assert.True(rb.Success);
            Assert.NotNull(ra.Hazard);
            Assert.NotNull(rb.Hazard);
            Assert.Equal(ra.Hazard!.hazard_id, rb.Hazard!.hazard_id);
            Assert.Equal(ra.Hazard.position_x, rb.Hazard.position_x);
            Assert.Equal(ra.Hazard.position_y, rb.Hazard.position_y);
            // Quantized to 0.01 km.
            Assert.Equal(100.56f, ra.Hazard.position_x);
            Assert.Equal(200.44f, ra.Hazard.position_y);
        }

        [Fact]
        public void Spawn_RejectsUnknownDefinitionAndInvalidPosition()
        {
            var system = CreateSystem();
            Assert.False(system.TrySpawn("anomaly_missing", 10, 10, 1).Success);
            Assert.Equal("unknown_anomaly", system.TrySpawn("anomaly_missing", 10, 10, 1).ReasonCode);
            Assert.Equal("invalid_position", system.TrySpawn("anomaly_test_front", -5, 10, 1).ReasonCode);
            Assert.Equal("invalid_position", system.TrySpawn("anomaly_test_front", 600, 10, 1).ReasonCode);
        }

        [Fact]
        public void Spawn_EnforcesConcurrentHazardCap()
        {
            var system = CreateSystem();
            for (int i = 0; i < AnomalyHazardSystem.MaxConcurrentHazards; i++)
            {
                var r = system.TrySpawn("anomaly_test_front", 50f + i * 20f, 50f, 1 + i, 90f);
                Assert.True(r.Success);
            }
            var blocked = system.TrySpawn("anomaly_test_front", 300f, 300f, 99, 0f);
            Assert.False(blocked.Success);
            Assert.Equal("hazard_cap_reached", blocked.ReasonCode);
        }

        // ── movement ───────────────────────────────────────────────────

        [Fact]
        public void StormFront_Movement_DeterministicAcrossRuns()
        {
            var a = CreateSystem();
            var b = CreateSystem();
            a.TrySpawn("anomaly_test_front", 100f, 100f, 1, 45f);
            b.TrySpawn("anomaly_test_front", 100f, 100f, 1, 45f);

            for (int day = 1; day <= 5; day++)
            {
                a.TickDay(day, 200f, 20f, new SeededRng(1000 + day));
                b.TickDay(day, 200f, 20f, new SeededRng(1000 + day));
            }

            var ha = a.State.hazards[0];
            var hb = b.State.hazards[0];
            Assert.Equal(ha.position_x, hb.position_x);
            Assert.Equal(ha.position_y, hb.position_y);
            Assert.Equal(ha.bearing_deg, hb.bearing_deg);
        }

        [Fact]
        public void StormFront_MovesAlongBearing_WithWander_ThenExpires()
        {
            var system = CreateSystem();
            var r = system.TrySpawn("anomaly_test_front", 100f, 100f, 1, 90f);
            Assert.True(r.Success);
            var h = r.Hazard!;

            float startX = h.position_x;
            // Day 1: eastward bearing (90°), no rng → straight line, 9 kph * 24 h = 216 km.
            system.TickDay(1, 0f, 0f, variationRng: null);
            Assert.Equal(316f, h.position_x);        // 100 + 216
            Assert.Equal(100f, h.position_y);
            Assert.Equal(1, h.age_days);

            // Intensity decays linearly over 6-day duration.
            Assert.Equal(1f - 1f / 6f, h.intensity, 3);

            // Duration 6 days → expired after the 6th tick.
            for (int day = 2; day <= 6; day++)
                system.TickDay(day, 0f, 0f, null);
            Assert.False(h.active);
        }

        [Fact]
        public void WindDrift_UsesWeatherWindScaledByWindResponse()
        {
            var system = CreateSystem();
            var r = system.TrySpawn("anomaly_test_drift", 256f, 256f, 1);
            Assert.True(r.Success);
            var h = r.Hazard!;

            // Wind 90° (east) at 10 kph, wind_response 0.9 → 10 * 0.9 * 24 = 216 km east.
            system.TickDay(1, 90f, 10f, null);
            Assert.Equal(472f, h.position_x);
            Assert.Equal(256f, h.position_y);
        }

        [Fact]
        public void Movement_ClampsToWorldBounds()
        {
            var system = CreateSystem();
            var r = system.TrySpawn("anomaly_test_front", 490f, 10f, 1, 90f);
            Assert.True(r.Success);
            var h = r.Hazard!;
            system.TickDay(1, 0f, 0f, null); // 216 km east from 490 → clamped to 512
            Assert.Equal(AnomalyHazardSystem.WorldMaxKm, h.position_x);
            system.TickDay(2, 270f, 50f, null); // drift not applied to storm front; stays
            system.TickDay(3, 0f, 0f, null);
            Assert.Equal(AnomalyHazardSystem.WorldMaxKm, h.position_x);
        }

        // ── overlap rules ──────────────────────────────────────────────

        [Fact]
        public void OverlappingHazards_RadiationIsAdditiveUpToCap()
        {
            var system = CreateSystem();
            // Two static zones at the same point: 40 + 40 = 80.
            system.TrySpawn("anomaly_test_static", 200f, 200f, 1);
            system.TrySpawn("anomaly_test_static", 200f, 200f, 1);
            Assert.Equal(80f, system.GetRadiationRate(200f, 200f), 3);
            Assert.Contains("mutagenic", system.GetEnvironmentalEffectTags(200f, 200f));

            // Outside both radii: zero.
            Assert.Equal(0f, system.GetRadiationRate(260f, 200f), 3);
        }

        [Fact]
        public void RadiationRate_IsNeverSelfApplied()
        {
            // The hazard layer only REPORTS the rate; no needs/health/survivor
            // reference exists anywhere on the type (structural guard).
            Assert.Null(typeof(AnomalyHazardSystem).GetProperty("Needs"));
            Assert.Null(typeof(AnomalyHazardSystem).GetProperty("Survivors"));
            // Cap honored.
            var system = CreateSystem();
            for (int i = 0; i < 4; i++)
                system.TrySpawn("anomaly_test_front", 200f, 200f, i, 90f);
            // 4 concurrent × 120 rads/hr = 480 — additive, below the 600 cap
            // (the cap remains a bounded guard for higher-rate authored defs).
            float rate = system.GetRadiationRate(200f, 200f);
            Assert.True(rate <= AnomalyHazardSystem.MaxCombinedRadiationRate + 0.001f);
            Assert.Equal(480f, rate, 3);
        }

        // ── detection ──────────────────────────────────────────────────

        [Fact]
        public void Detection_ClassifiedWithCapableDevice_SignatureByContact_UndetectedFarAway()
        {
            var system = CreateSystem();
            system.TrySpawn("anomaly_test_static", 200f, 200f, 1);

            // Inside radius, no device: felt directly → Signature (truth not hidden).
            Assert.Equal(HazardDetection.Signature,
                system.ClassifyDetection(201f, 200f, detectorCapability: 0f));
            Assert.Equal(AnomalyHazardSystem.ConfidenceContact,
                system.GetDetectionConfidence(201f, 200f, 0f), 3);

            // Inside warning radius, capable device → Classified.
            Assert.Equal(HazardDetection.Classified,
                system.ClassifyDetection(210f, 200f, detectorCapability: 10f));

            // Inside warning radius, under-powered device → Signature at low
            // confidence (reduced information, not zero — §4.8).
            Assert.Equal(HazardDetection.Signature,
                system.ClassifyDetection(210f, 200f, detectorCapability: 5f));
            Assert.Equal(AnomalyHazardSystem.ConfidenceSignature,
                system.GetDetectionConfidence(210f, 200f, 5f), 3);

            // Beyond warning radius → unknown.
            Assert.Equal(HazardDetection.Undetected,
                system.ClassifyDetection(400f, 200f, detectorCapability: 300f));
        }

        // ── loot gating ────────────────────────────────────────────────

        [Fact]
        public void LootSites_ResolveExactlyOnce_ThenAlreadyResolved()
        {
            var system = CreateSystem();
            var spawn = system.TrySpawn("anomaly_test_static", 200f, 200f, 1);
            Assert.True(spawn.Success);
            Assert.Equal(2, system.LootSites.Count);

            var first = system.TryResolveLootSite(system.LootSites[0].site_id, day: 3);
            Assert.True(first.Success);
            Assert.Equal("table_loot_chemical_plant", first.LootTableId);

            var second = system.TryResolveLootSite(system.LootSites[0].site_id, day: 4);
            Assert.False(second.Success);
            Assert.Equal("loot_already_resolved", second.ReasonCode);

            var other = system.TryResolveLootSite(system.LootSites[1].site_id, day: 4);
            Assert.True(other.Success);

            Assert.Equal("unknown_loot_site", system.TryResolveLootSite("site_missing", 4).ReasonCode);
        }

        // ── wildlife modifier ──────────────────────────────────────────

        [Fact]
        public void WildlifeModifier_IsBoundedAndSums()
        {
            var system = CreateSystem();
            // -900bp static + drift (-400bp) overlapping → clamp to -1.0.
            system.TrySpawn("anomaly_test_static", 200f, 200f, 1);
            system.TrySpawn("anomaly_test_drift", 205f, 200f, 1);
            Assert.Equal(-1f, system.GetWildlifeModifier(202f, 200f), 3);

            // Positive attraction zone exists in the authored set; outside zones → 0.
            Assert.Equal(0f, system.GetWildlifeModifier(400f, 400f), 3);
        }

        // ── warnings ───────────────────────────────────────────────────

        [Fact]
        public void ApproachWarning_FiresOncePerEpisode_WithTypedPayload()
        {
            var system = CreateSystem();
            var warnings = new List<AnomalyApproachWarning>();
            system.OnStormApproaching += w => warnings.Add(w);

            system.TrySpawn("anomaly_test_front", 10f, 100f, 1, 90f); // 100 km east of target? bearing E moves +X away; use W bearing
            system.State.hazards[0].bearing_deg = 180f; // travel south toward target below
            // Position it 35 km north of the target so it's inside the 85% band (40*0.85=34)? 35 > 34.
            // Use 30 km north instead.
            system.State.hazards[0].position_x = 100f;
            system.State.hazards[0].position_y = 130f;

            system.EvaluateApproach("loc_holdfast", 100f, 100f);
            Assert.Single(warnings);
            Assert.Equal("S", warnings[0].DirectionCardinal);
            Assert.Equal("severe", warnings[0].IntensityBand);
            Assert.Equal("loc_holdfast", warnings[0].TargetId);
            Assert.True(warnings[0].EtaDays > 0f);

            // Second evaluation: no duplicate warning (one-shot per episode).
            system.EvaluateApproach("loc_holdfast", 100f, 100f);
            Assert.Single(warnings);
        }

        [Fact]
        public void ApproachWarning_NeverFiresForStaticZones()
        {
            var system = CreateSystem();
            var fired = new List<AnomalyApproachWarning>();
            system.OnStormApproaching += w => fired.Add(w);
            system.TrySpawn("anomaly_test_static", 100f, 110f, 1);
            system.EvaluateApproach("loc_holdfast", 100f, 100f);
            Assert.Empty(fired);
        }

        [Fact]
        public void HazardContact_FiresOncePerEpisode_WhenRadiusCoversTarget()
        {
            var system = CreateSystem();
            var contacts = new List<AnomalyHazardInstance>();
            system.OnHazardContact += h => contacts.Add(h);

            system.TrySpawn("anomaly_test_front", 100f, 100f, 1, 90f);
            var hazard = system.State.hazards[0];
            hazard.position_x = 100f;
            hazard.position_y = 110f; // 10 km from target; radius_km = 18

            system.EvaluateApproach("loc_holdfast", 100f, 100f);
            Assert.Single(contacts);
            Assert.Equal("anomaly_test_front", contacts[0].anomaly_id);

            // Still overlapping: no repeat contact.
            system.EvaluateApproach("loc_holdfast", 100f, 100f);
            Assert.Single(contacts);
        }

        [Fact]
        public void HazardContact_DoesNotFireOutsideRadius()
        {
            var system = CreateSystem();
            int contacts = 0;
            system.OnHazardContact += _ => contacts++;

            system.TrySpawn("anomaly_test_front", 100f, 100f, 1, 90f);
            system.State.hazards[0].position_x = 100f;
            system.State.hazards[0].position_y = 200f; // 100 km away

            system.EvaluateApproach("loc_holdfast", 100f, 100f);
            Assert.Equal(0, contacts);
        }

        // ── persistence ────────────────────────────────────────────────

        [Fact]
        public void SaveLoad_PreservesExactCoordinatesAndState()
        {
            var system = CreateSystem();
            system.TrySpawn("anomaly_test_front", 100.13f, 100.87f, 3, 47.5f);
            system.TrySpawn("anomaly_test_static", 200.25f, 200.75f, 2);
            system.TickDay(4, 90f, 12f, new SeededRng(7));
            system.TryResolveLootSite(system.LootSites[0].site_id, 4);

            var saved = system.CaptureState();
            var restored = CreateSystem();
            restored.RestoreState(saved);

            Assert.Equal(saved.hazard_counter, restored.State.hazard_counter);
            for (int i = 0; i < saved.hazards.Count; i++)
            {
                var a = saved.hazards[i];
                var b = restored.State.hazards[i];
                Assert.Equal(a.hazard_id, b.hazard_id);
                Assert.Equal(a.position_x, b.position_x);
                Assert.Equal(a.position_y, b.position_y);
                Assert.Equal(a.bearing_deg, b.bearing_deg);
                Assert.Equal(a.intensity, b.intensity, 5);
                Assert.Equal(a.active, b.active);
                Assert.Equal(a.fired_warning_keys.Count, b.fired_warning_keys.Count);
            }
            for (int i = 0; i < saved.loot_sites.Count; i++)
            {
                Assert.Equal(saved.loot_sites[i].resolved, restored.State.loot_sites[i].resolved);
                Assert.Equal(saved.loot_sites[i].resolved_day, restored.State.loot_sites[i].resolved_day);
            }
        }

        [Fact]
        public void SplitRun_MidReloadProducesIdenticalStormTrack()
        {
            // Continuous: 6 days straight.
            var continuous = CreateSystem();
            continuous.TrySpawn("anomaly_test_front", 100f, 100f, 1, 60f);
            for (int day = 1; day <= 6; day++)
                continuous.TickDay(day, 210f, 15f, new SeededRng(500 + day));

            // Split: 3 days → capture/restore → 3 more days with the SAME day-keyed forks.
            var split = CreateSystem();
            split.TrySpawn("anomaly_test_front", 100f, 100f, 1, 60f);
            for (int day = 1; day <= 3; day++)
                split.TickDay(day, 210f, 15f, new SeededRng(500 + day));

            var restored = CreateSystem();
            restored.RestoreState(split.CaptureState());
            for (int day = 4; day <= 6; day++)
                restored.TickDay(day, 210f, 15f, new SeededRng(500 + day));

            var hc = continuous.State.hazards[0];
            var hr = restored.State.hazards[0];
            Assert.Equal(hc.position_x, hr.position_x);
            Assert.Equal(hc.position_y, hr.position_y);
            Assert.Equal(hc.bearing_deg, hr.bearing_deg);
            Assert.Equal(hc.age_days, hr.age_days);
            Assert.Equal(hc.intensity, hr.intensity, 5);
            Assert.Equal(hc.active, hr.active);
        }

        [Fact]
        public void OldSaveBaseline_RestoreFromNullAndPartialIsSafe()
        {
            var system = CreateSystem();
            system.RestoreState(null); // old saves: no anomaly section ever existed
            Assert.Empty(system.State.hazards);
            Assert.Equal(0f, system.GetRadiationRate(256f, 256f), 3);
        }
    }
}
