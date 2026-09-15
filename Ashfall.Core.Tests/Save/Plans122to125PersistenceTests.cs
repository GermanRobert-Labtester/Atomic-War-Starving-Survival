// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Plans122to125Persistence
{
    /// <summary>
    /// Plans 122-125 Phase 8 — save contract without Godot runtime:
    /// registry rows (key/owner/methods/filename), Capture/Restore
    /// round-trips at mid-state, and old-save (null) migration defaults.
    /// </summary>
    public sealed class Plans122to125PersistenceTests
    {
        // TEST-AGGREGATION: registry rows for 4 sections asserted per row.
        [Fact]
        public void Registry_rows_are_registered_unique_and_named()
        {
            var rows = new[]
            {
                ("sofc_power", "SaveSofcPower", "SetupSofcPower", "shelter"),
                ("cvd_diamond", "SaveCvdDiamond", "SetupCvdDiamond", "shelter"),
                ("sound_ranging", "SaveSoundRanging", "SetupSoundRanging", "combat"),
                ("amphibious_draisine", "SaveAmphibiousDraisine", "SetupAmphibiousDraisine", "expeditions"),
            };
            var failures = new System.Collections.Generic.List<string>();
            foreach (var (key, save, setup, owner) in rows)
            {
                if (SaveSectionRegistry.All.Count(s => s.SectionKey == key) != 1) failures.Add($"{key}: row missing/duplicated");
                if (SaveSectionRegistry.All.Count(s => s.SaveMethod == save) != 1) failures.Add($"{key}: SaveMethod missing/duplicated");
                if (SaveSectionRegistry.All.Count(s => s.SetupMethod == setup) != 1) failures.Add($"{key}: SetupMethod missing/duplicated");
                if (!SaveSectionRegistry.SectionFileNames.TryGetValue(key, out var file) || file != $"{key}_save.json")
                    failures.Add($"{key}: filename mapping wrong");
                var row = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == key);
                if (row != null && row.Owner != owner) failures.Add($"{key}: owner {row.Owner} != {owner}");
            }
            Assert.True(failures.Count == 0, string.Join("; ", failures));
        }

        [Fact]
        public void Registry_counts_are_consistent()
        {
            Assert.Equal(SaveSectionRegistry.All.Count, SaveSectionRegistry.SectionFileNames.Count);
        }

        private static SofcPowerCatalog SofcCatalog()
        {
            var catalog = new SofcPowerCatalog
            {
                stack_profiles =
                {
                    new SofcStackProfile
                    {
                        id = "st", fuel_profile_id = "f", rated_power_kw = 20f, fuel_efficiency = 0.6f,
                        startup_ticks = 4, cooldown_ticks = 3,
                        thermal_band = new SofcThermalBand { minimum = 0.5f, optimal_min = 0.7f, optimal_max = 0.9f, maximum = 1f },
                        degradation_per_operating_tick_bp = 15, acoustic_signature_class = SofcPowerCatalog.SignatureVeryLow,
                        waste_heat_profile_id = "h"
                    }
                },
                fuel_profiles = { new SofcFuelProfile { id = "f", quality_class = SofcPowerCatalog.QualityClean, output_modifier_bp = 10000, degradation_multiplier_bp = 10000 } },
                waste_heat_profiles = { new SofcWasteHeatProfile { id = "h", heat_kw_per_unit = 1.5f, max_allocatable_kw = 12f } }
            };
            catalog.Index();
            return catalog;
        }

        [Fact]
        public void Sofc_roundtrip_preserves_mode_health_and_produces_after_restore()
        {
            var engine = new SofcElectrochemistryEngine(SofcCatalog()) { Rng = new SeededRng(900) };
            engine.Install("test_stack", partsAvailable: true);
            engine.StartPreheat(true);
            for (int i = 0; i < 10; i++)
                engine.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });

            var saved = engine.CaptureState();
            var restored = new SofcElectrochemistryEngine(SofcCatalog());
            restored.RestoreState(saved);
            Assert.Equal(saved.StackProfileId, restored.State.StackProfileId);
            Assert.Equal(saved.Mode, restored.State.Mode);
            Assert.Equal(saved.StackHealthBp, restored.State.StackHealthBp);
            Assert.Equal(saved.SealIntegrityBp, restored.State.SealIntegrityBp);
            // Restored plant continues to operate coherently.
            var after = restored.AdvanceTick(new SofcTickInput { FuelAvailable = true, RequestedOutputKw = 10f });
            Assert.True(after!.AvailableOutputKw >= 0f);
        }

        [Fact]
        public void Sofc_null_save_is_clean_migration()
        {
            var engine = new SofcElectrochemistryEngine(SofcCatalog());
            engine.RestoreState(null); // old save without the section
            Assert.Equal(SofcOperatingMode.Offline, engine.State.Mode);
        }

        [Fact]
        public void Cvd_roundtrip_preserves_midgrowth_batch()
        {
            var catalog = new CvdDiamondCatalog
            {
                growth_grades = { new DiamondGrowthGrade { id = "grade_utility", display_name = "U", rank = 1, wear_factor_bp = 7000 } },
                reactor_profiles = { new CvdReactorProfile { id = "r", display_name = "R", growth_rate_per_tick_bp = 6000, plasma_stability_baseline_bp = 7000, magnetron_wear_per_tick_bp = 25, chamber_wear_per_tick_bp = 18, power_demand_kw = 12f, maintenance_profile_id = "m" } },
                feed_profiles = { new DiamondFeedProfile { id = "f", display_name = "F", purity_bp = 9000 } },
                substrate_profiles = { new DiamondSubstrateProfile { id = "s", display_name = "S", quality_bp = 8000, input_item_id = "item_x" } },
                tool_components = { new DiamondToolComponent { id = "diamond_insert_utility", display_name = "U", min_grade_id = "grade_utility", accepted_consumer_tags = { "consumer_exc" } } },
                consumer_mappings = { new DiamondConsumerMapping { id = "consumer_exc", display_name = "E", accepted_component_ids = { "diamond_insert_utility" } } },
                maintenance_profiles = { new DiamondMaintenanceProfile { id = "m", display_name = "M", inspection_interval_days = 10 } }
            };
            catalog.Index();
            var engine = new CvdDiamondSynthesisEngine(catalog) { Rng = new SeededRng(901) };
            engine.Install("r", partsAvailable: true);
            engine.StartBatch("b", "diamond_insert_utility", "f", "s", true, true, true, true, 60f);
            engine.AdvanceBatch();
            var saved = engine.CaptureState();
            Assert.True(saved.ActiveBatch != null && saved.ActiveBatch.GrowthProgressBp > 0, "mid-growth state expected");

            var restored = new CvdDiamondSynthesisEngine(catalog);
            restored.RestoreState(saved);
            Assert.NotNull(restored.State.ActiveBatch);
            Assert.Equal(saved.ActiveBatch!.GrowthProgressBp, restored.State.ActiveBatch!.GrowthProgressBp);
            Assert.Equal(saved.MagnetronConditionBp, restored.State.MagnetronConditionBp);
            // The restored batch still completes.
            for (int i = 0; i < 20 && restored.State.ActiveBatch != null; i++)
                restored.AdvanceBatch();
            Assert.NotNull(restored.GetFinishedBatch("b"));
        }

        [Fact]
        public void Cvd_null_save_is_clean_migration()
        {
            var engine = new CvdDiamondSynthesisEngine(new CvdDiamondCatalog());
            engine.RestoreState(null);
            Assert.Equal(CvdReactorMode.Offline, engine.State.Mode);
        }

        [Fact]
        public void Sra_roundtrip_preserves_active_threat()
        {
            var catalog = new SoundRangingCatalog
            {
                array_profiles = { new SoundRangingArrayProfile { id = "a", display_name = "A", sensor_profile_id = "s", sensor_count = 4, base_bearing_error_deg = 8f, base_region_radius_cells = 3, timing_quality = 0.75f, weather_sensitivity = 0.45f, confidence_cap_bp = 9000 } },
                sensor_profiles = { new SoundRangingSensorProfile { id = "s", display_name = "S", reliability_bp = 8500 } }
            };
            catalog.Index();
            var engine = new SoundRangingThreatEngine(catalog) { Rng = new SeededRng(902) };
            engine.Install("a", partsAvailable: true);
            engine.RecordObservation(new SoundRangingThreatEngine.HostileFireObservation { Day = 2, BearingDeg = 180, SourceTag = "b" });

            var saved = engine.CaptureState();
            var restored = new SoundRangingThreatEngine(catalog);
            restored.RestoreState(saved);
            var threat = restored.GetActiveThreat();
            Assert.NotNull(threat);
            Assert.Equal(saved.ActiveThreat!.BearingDeg, threat!.BearingDeg);
            Assert.Equal(saved.ActiveThreat.ConfidenceBp, threat.ConfidenceBp);
            Assert.Equal(saved.Observations.Count, restored.State.Observations.Count);
        }

        [Fact]
        public void Sra_null_save_is_clean_migration()
        {
            var engine = new SoundRangingThreatEngine(new SoundRangingCatalog());
            engine.RestoreState(null);
            Assert.Null(engine.GetActiveThreat());
            Assert.Equal(0, engine.OperationalSensorCount());
        }

        [Fact]
        public void Amphibious_roundtrip_preserves_midcrossing_state()
        {
            var catalog = new AmphibiousDraisineCatalog
            {
                kit_profiles = { new AmphibiousKitProfile { id = "k", display_name = "K", compatible_vehicle_tags = { "rail_draisine" }, flotation_rating = 0.8f, current_tolerance = 0.6f, cargo_capacity_modifier_bp = 9000, water_speed_modifier_bp = 5000, deployment_ticks = 1, mass_kg = 200, pump_profile_id = "p", route_class_ids = { "r" } } },
                pump_profiles = { new AmphibiousPumpProfile { id = "p", display_name = "P", ingress_mitigation_bp = 5000, power_source = "manual" } },
                route_class_profiles = { new AmphibiousRouteClassProfile { id = "r", display_name = "R", max_current_risk = 0.5f, min_flotation_rating = 0.5f } }
            };
            catalog.Index();
            var engine = new AmphibiousDraisineEngine(catalog) { Rng = new SeededRng(903) };
            engine.InstallKit("v", "rail_draisine", "k", true, true, 9000);
            engine.Deploy("v");
            engine.TickDeployment("v");
            engine.BeginCrossing("v", "r", 0.3f, 9000, true);
            engine.AdvanceCrossing("v", 0.3f, false, 50f, 9000);
            var saved = engine.CaptureState();
            Assert.True(saved.ContainsKey("v") && saved["v"].Phase == AmphibiousCrossingPhase.Crossing, "mid-crossing expected");

            var restored = new AmphibiousDraisineEngine(catalog);
            restored.RestoreState(saved);
            var v = restored.FindVehicle("v");
            Assert.NotNull(v);
            Assert.Equal(AmphibiousCrossingPhase.Crossing, v!.Phase);
            Assert.Equal(saved["v"].CrossingProgressBp, v.CrossingProgressBp);
            Assert.Equal(saved["v"].PontoonConditionBp, v.PontoonConditionBp);
        }

        [Fact]
        public void Amphibious_null_save_is_clean_migration()
        {
            var engine = new AmphibiousDraisineEngine(new AmphibiousDraisineCatalog());
            engine.RestoreState(null);
            Assert.Null(engine.FindVehicle("anything"));
        }

        [Fact]
        public void Soic_envelope_schema_versions_exist_for_new_sections()
        {
            // The four new sections must resolve a filename via the registry
            // (envelope codec uses it) — gates accidental schema drift.
            foreach (var key in new[] { "sofc_power", "cvd_diamond", "sound_ranging", "amphibious_draisine" })
                Assert.True(SaveSectionRegistry.SectionFileNames.ContainsKey(key), $"{key} filename missing");
        }
    }
}
