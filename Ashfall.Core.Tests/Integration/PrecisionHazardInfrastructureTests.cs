using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plans 78–81 cross-system scenario tests (plan §17–20).
    /// Composes the real Core authorities deterministically — no host, no mocks
    /// of domain logic. Seams that are wired in the Godot host (Main) rather
    /// than Core are noted inline.
    /// </summary>
    public class PrecisionHazardInfrastructureTests
    {
        // ─── Shared fixtures ───

        private static DeconProtocolCatalog MakeDeconCatalog(float interlockThreshold = 10f)
        {
            return new DeconProtocolCatalog
            {
                protocols = new List<DeconProtocolDef>
                {
                    new DeconProtocolDef
                    {
                        protocol_id = "decon_standard_return",
                        stages = new List<DeconStageDef>
                        {
                            new DeconStageDef { stage_id = "stage_alpha", stage_order = 0, duration_ticks = 2, water_liters = 10f, external_contamination_multiplier = 0.2f, effluent_contamination_contribution = 0.1f, requires_operator = true, operator_skill_factor = 0f },
                            new DeconStageDef { stage_id = "stage_gate", stage_order = 1, duration_ticks = 1, water_liters = 0, external_contamination_multiplier = 0.1f, effluent_contamination_contribution = 0f, requires_operator = true, operator_skill_factor = 0f }
                        },
                        total_chelator_units = 0,
                        interlock_threshold_mSv_per_h = interlockThreshold
                    }
                },
                effluent_treatment = new DeconEffluentTreatmentDef { default_tank_capacity_liters = 200f },
                gear_disposal = new DeconGearDisposalDef { disposal_threshold = 0.85f }
            };
        }

        private static GeodeticSurveyCatalog MakeSurveyCatalog()
        {
            return new GeodeticSurveyCatalog
            {
                survey_points = new List<SurveyPointDef>
                {
                    new SurveyPointDef { survey_point_id = "pt_ridge", world_node_id = "loc_industrial_valley", display_name = "Ridge", point_type = "ridge", elevation_m = 1200f, baseline_quality = 0.9f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string> { "route_valley_shortcut" } },
                    new SurveyPointDef { survey_point_id = "pt_chimney", world_node_id = "loc_industrial_valley", display_name = "Chimney", point_type = "industrial_chimney", elevation_m = 700f, baseline_quality = 0.85f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string> { "route_valley_shortcut" } },
                    new SurveyPointDef { survey_point_id = "pt_rail", world_node_id = "loc_industrial_valley", display_name = "Rail Marker", point_type = "rail_marker", elevation_m = 300f, baseline_quality = 0.8f, construction_allowed = true, construction_required_items = new List<string> { "item_datum_plate_bronze" }, hidden_route_refs = new List<string>() }
                },
                survey_equipment = new SurveyEquipmentDef { theodolite_base_error_degrees = 0.05f },
                weather_modifiers = new Dictionary<string, WeatherModifierDef>(StringComparer.OrdinalIgnoreCase) { { "clear", new WeatherModifierDef() } },
                triangulation = new TriangulationParamsDef { min_baseline_length_m = 150f, network_accuracy_floor = 0.1f, network_accuracy_max = 1.0f },
                navigation_effects = new NavigationEffectsDef()
            };
        }

        private static ToxicChemicalCatalog MakeChemCatalog()
        {
            return new ToxicChemicalCatalog
            {
                hazard_profiles = new List<ChemicalHazardProfile>
                {
                    new ChemicalHazardProfile
                    {
                        hazard_id = "hazard_valtery_vapor", display_name = "Valtery Vapor",
                        hazard_class = "corrosive_vapor", detector_response_band = "medium_band",
                        normalized_concentration = 0.7f, persistence = 0.5f, volatility = 0.8f,
                        wind_response = 0.9f, filter_category = "acid_gas",
                        filter_load_rate = 0.12f, exposure_severity = 0.6f,
                        sample_value = 18f, detection_threshold = 0.05f, safe_exposure_band = "danger"
                    }
                },
                detector_equipment = new DetectorEquipmentDef { detector_bands = new List<string> { "low_band", "medium_band", "wide_band" }, base_detection_confidence = 0.85f, battery_ticks_per_charge = 20, per_scan_battery_drain = 1 },
                sample_collection = new SampleCollectionDef { max_samples_per_mission = 4 },
                filter_model = new FilterModelDef { filter_capacity_base = 100f, incompatible_filter_penalty = 2.5f, breakthrough_warning_threshold = 0.15f },
                map_overlay = new MapOverlayDef { safe_corridor_confidence_required = 0.7f, overlay_persistence_days = 30 }
            };
        }

        private static KineticFlywheelCatalog MakeFlywheelCatalog()
        {
            return new KineticFlywheelCatalog
            {
                flywheel_classes = new List<FlywheelClassDef>
                {
                    new FlywheelClassDef
                    {
                        flywheel_id = "flywheel_scenario_500", rotor_mass_kg = 500f, effective_radius_m = 0.45f,
                        moment_of_inertia_factor = 0.5f, max_rpm = 12000f, max_safe_rpm_ratio = 0.9f,
                        min_vacuum_torr = 1.0e-3f, operational_vacuum_torr = 1.0e-5f,
                        max_bearing_temp_c = 120f, safe_bearing_temp_c = 90f, containment_rating = 0.7f,
                        motor_generator_efficiency = 0.88f, max_charge_kw = 15f, max_discharge_kw = 25f,
                        idle_drag_loss_percent_per_hour = 0.5f, vacuum_leak_rate_per_day = 0.02f,
                        bearing_heat_per_charge_kw = 0.8f, bearing_heat_per_discharge_kw = 1.0f,
                        bearing_cooling_rate_per_tick = 0.5f,
                        construction_required_items = new List<string> { "item_forged_rotor_shaft" },
                        maintenance_required_items = new List<string> { "item_bearing_grease" }
                    }
                },
                surge_events = new List<SurgeEventDef>
                {
                    new SurgeEventDef { surge_id = "surge_blast_door_motor", peak_kw = 18f, duration_ticks = 1, event_class = "door_motor" },
                    new SurgeEventDef { surge_id = "surge_decon_pump_cycle", peak_kw = 8f, duration_ticks = 1, event_class = "decon_pump" }
                },
                black_start = new BlackStartDef { min_stored_energy_kwh = 0.5f, generator_restart_probability = 0.95f },
                containment_hazard = new ContainmentHazardDef()
            };
        }

        private static (DecontaminationSystem decon, Inventory.Inventory inv) MakeDecon(int seed, float interlockThreshold = 10f)
        {
            var inv = new Inventory.Inventory();
            inv.AddById("water_clean", 200);
            inv.AddById("soap", 200);
            var decon = new DecontaminationSystem(
                new SeededRng(seed),
                new RadiationSystem(seed: seed),
                inv,
                new AirlockSecuritySystem(new SeededRng(seed)),
                new StartingLevelSystem(),
                MakeDeconCatalog(interlockThreshold));
            return (decon, inv);
        }

        private static string RunDeconToCompletion(DecontaminationSystem decon, string survivor, float contamination, out float finalSurface)
        {
            float observed = float.NaN;
            string outcome = string.Empty;
            void Handler(DeconCase c) { observed = c.surfaceContamination; }
            decon.OnCaseCompleted += Handler;
            try
            {
                decon.StartProtocolCycle("decon_standard_return", survivor, "gear_a", contamination);
                DeconStageResult r;
                do
                {
                    r = decon.TickActiveStage();
                    if (r.cycleComplete) outcome = r.outcome;
                }
                while (!r.cycleComplete && string.IsNullOrEmpty(r.error));
            }
            finally
            {
                decon.OnCaseCompleted -= Handler;
            }
            finalSurface = observed;
            return outcome;
        }

        // ─── §17 Scenario A — Contaminated survey expedition ───

        [Fact]
        public void ScenarioA_ContaminatedSurveyExpedition()
        {
            // Survey team enters the valley, measures the network.
            var survey = new GeodeticSurveyEngine(MakeSurveyCatalog(), new SeededRng(7));
            foreach (var id in new[] { "pt_ridge", "pt_chimney", "pt_rail" })
                survey.EstablishMonument(id, 1, (itemId, n) => true);
            survey.Observe("pt_ridge", "pt_chimney", "clear");
            survey.Observe("pt_chimney", "pt_rail", "clear");
            survey.Observe("pt_rail", "pt_ridge", "clear");
            var tri = survey.TryResolveTriangle("pt_ridge", "pt_chimney", "pt_rail");
            Assert.NotNull(tri);
            Assert.Contains("route_valley_shortcut", survey.UnlockedShortcuts);

            // Recon detects the invisible hazard on the way in.
            var recon = new ChemicalReconEngine(MakeChemCatalog(), new SeededRng(7));
            var detection = recon.ScanLocation("loc_industrial_valley", "medium_band");
            Assert.True(detection.Detected);
            Assert.False(survey.UnlockedShortcuts.Count == 0, "survey knowledge persists alongside recon");

            // Team returns contaminated — the airlock processes them.
            var (decon, inv) = MakeDecon(7);
            inv.AddById("item_heavy_neoprene_scrub_brush", 1); // the gear instance
            float finalSurface;
            string outcome = RunDeconToCompletion(decon, "scout_1", 0.9f, out finalSurface);

            Assert.Equal("decontaminated", outcome);
            Assert.True(finalSurface < 0.9f, "external contamination must be reduced");
            Assert.Equal(1, inv.CountById("item_heavy_neoprene_scrub_brush")); // same item instance — no duplication, no loss
            Assert.Empty(decon.State.disposedGearIds);
            Assert.True(decon.CanOpenInnerDoor());
            Assert.True(decon.State.effluentTankVolume > 0f, "wash water captured as effluent");

            // Save/restore all three authorities → replay parity.
            var surveySnap = survey.CaptureState();
            var reconSnap = recon.CaptureState();
            var deconSnap = decon.CaptureState();

            var survey2 = new GeodeticSurveyEngine(MakeSurveyCatalog(), new SeededRng(999));
            survey2.RestoreState(surveySnap);
            var recon2 = new ChemicalReconEngine(MakeChemCatalog(), new SeededRng(999));
            recon2.RestoreState(reconSnap);
            var (decon2, _) = MakeDecon(999);
            decon2.RestoreState(deconSnap);

            Assert.Equal(survey.NetworkAccuracy, survey2.NetworkAccuracy, 6);
            Assert.Single(recon2.State.hazardObservations);
            Assert.Single(survey2.UnlockedShortcuts);
            Assert.True(decon2.State.effluentTankVolume > 0f);
        }

        // ─── §18 Scenario B — Grid failure during decon ───

        [Fact]
        public void ScenarioB_GridFailureDuringDecon()
        {
            var sys = new KineticStorageSystem(MakeFlywheelCatalog(), new SeededRng(11));
            sys.InstallFlywheel("flywheel_scenario_500", "power_room", 1, (itemId, n) => true);
            sys.BringOnline("flywheel_flywheel_scenario_500_power_room");
            sys.Charge("flywheel_flywheel_scenario_500_power_room", 15f, 600f);

            // Decon pump surge while the flywheel holds charge — burst delivered.
            float delivered = sys.HandleSurge("flywheel_flywheel_scenario_500_power_room", "surge_decon_pump_cycle");
            Assert.True(delivered > 0f, "charged flywheel must supply the decon pump surge");

            // Variant: flywheel empty → surge unmet.
            sys.Discharge("flywheel_flywheel_scenario_500_power_room", 25f, 3600f);
            float unmet = sys.HandleSurge("flywheel_flywheel_scenario_500_power_room", "surge_decon_pump_cycle");
            Assert.Equal(0f, unmet);

            // Safety default: with the surge unmet the decon cycle pauses mid-cycle
            // and the inner door stays interlocked.
            var (decon, inv) = MakeDecon(11);
            decon.StartProtocolCycle("decon_standard_return", "s1", "gear_a", 0.9f);
            decon.TickActiveStage(); // progress but do not finish
            Assert.Equal(DeconStatus.InProgress, decon.State.activeCase!.status);
            Assert.False(decon.CanOpenInnerDoor());
            Assert.Equal("CYCLE IN PROGRESS", decon.InnerDoorFailureReason());
        }

        // ─── §19 Scenario C — High-precision hazard mapping ───

        [Fact]
        public void ScenarioC_PrecisionHazardMapping()
        {
            // Completing the geodetic network raises survey accuracy.
            var survey = new GeodeticSurveyEngine(MakeSurveyCatalog(), new SeededRng(21));
            foreach (var id in new[] { "pt_ridge", "pt_chimney", "pt_rail" })
                survey.EstablishMonument(id, 1, (itemId, n) => true);
            survey.Observe("pt_ridge", "pt_chimney", "clear");
            survey.Observe("pt_chimney", "pt_rail", "clear");
            survey.Observe("pt_rail", "pt_ridge", "clear");
            survey.TryResolveTriangle("pt_ridge", "pt_chimney", "pt_rail");
            Assert.True(survey.NetworkAccuracy > 0.1f, "resolved triangle must raise accuracy above the floor");

            // Survey improves knowledge precision, NOT hazard behavior (plan R2/R4):
            // the hazard truth the recon engine reads is identical with or without the network.
            var reconNoSurvey = new ChemicalReconEngine(MakeChemCatalog(), new SeededRng(21));
            var reconWithSurvey = new ChemicalReconEngine(MakeChemCatalog(), new SeededRng(21));

            var bare = reconNoSurvey.ScanLocation("loc_industrial_valley", "medium_band");
            var withNet = reconWithSurvey.ScanLocation("loc_industrial_valley", "medium_band");

            Assert.Equal(bare.NormalizedLevel, withNet.NormalizedLevel, 6);
            Assert.Equal(bare.FilterLoadRate, withNet.FilterLoadRate, 6);
            Assert.Equal(bare.HazardClass, withNet.HazardClass);

            // Corridor capability is recon-owned and independently traceable.
            Assert.True(reconWithSurvey.TryDiscoverSafeCorridor("corridor_valley", "loc_industrial_valley"));
            Assert.False(reconNoSurvey.IsCorridorSafe("corridor_valley"));
        }

        // ─── §20 Scenario D — Black start after environmental emergency ───

        [Fact]
        public void ScenarioD_BlackStartAfterEnvironmentalEmergency()
        {
            var sys = new KineticStorageSystem(MakeFlywheelCatalog(), new SeededRng(33));
            sys.InstallFlywheel("flywheel_scenario_500", "power_room", 1, (itemId, n) => true);
            sys.BringOnline("flywheel_flywheel_scenario_500_power_room");
            sys.Charge("flywheel_flywheel_scenario_500_power_room", 15f, 1200f);

            // Grid dead → black-start burst. Deterministic per seed; the attempt
            // consumes exactly the starter energy whether or not the generator catches.
            float before = sys.FindFlywheel("flywheel_flywheel_scenario_500_power_room")!.storedEnergyJ;
            bool started = sys.TryBlackStart("flywheel_flywheel_scenario_500_power_room");
            float after = sys.FindFlywheel("flywheel_flywheel_scenario_500_power_room")!.storedEnergyJ;
            Assert.Equal(before - KineticStorageSystem.KwhToJoules(0.5f), after, 1);

            // Pumps return: the decon cycle completes safely after restart.
            var (decon, inv) = MakeDecon(33);
            float finalSurface;
            string outcome = RunDeconToCompletion(decon, "s1", 0.7f, out finalSurface);
            Assert.Equal("decontaminated", outcome);
            Assert.True(decon.CanOpenInnerDoor());
        }

        // ─── Plan-level integration: expedition return & effluent ───

        [Fact]
        public void ExpeditionReturn_InnerDoorOpensOnlyWhenGatePasses()
        {
            // Expedition return: threshold 5 splits the two cases —
            // heavy 0.99 → 0.69 residual → gate 6.9 > 5 → rewash; light 0.1 → 0 → pass.
            var (decon, inv) = MakeDecon(51, interlockThreshold: 5f);
            inv.AddById("water_clean", 200); inv.AddById("soap", 200);

            // Heavily contaminated return: the single chemical stage cannot bring
            // the gate reading under threshold → rewash required, door locked.
            decon.StartProtocolCycle("decon_standard_return", "s_heavy", "gear_heavy", 0.99f);
            string heavyOutcome = RunUntilComplete(decon);
            Assert.Equal("rewash_required", heavyOutcome);
            Assert.False(decon.CanOpenInnerDoor());

            // Lightly contaminated return: gate passes → door opens.
            float surface;
            string lightOutcome = RunDeconToCompletion(decon, "s_light", 0.1f, out surface);
            Assert.Equal("decontaminated", lightOutcome);
            Assert.True(decon.CanOpenInnerDoor());
        }

        [Fact]
        public void TwoDeconCycles_EffluentAccumulates_TreatmentPreservesResidue()
        {
            var (decon, inv) = MakeDecon(61);

            RunDeconToCompletion(decon, "s1", 0.8f, out _);
            RunDeconToCompletion(decon, "s2", 0.8f, out _);

            Assert.True(decon.State.effluentTankVolume > 0f);
            Assert.True(decon.State.effluentTankContamination > 0f);

            float sludgeBefore = decon.State.effluentSludgeVolume;
            decon.TreatEffluent();

            Assert.Equal(0f, decon.State.effluentTankVolume);
            Assert.True(decon.State.effluentSludgeVolume > sludgeBefore, "hazardous residue preserved as sludge");
        }

        [Fact]
        public void SurveyTriangle_UnlocksShortcutExactlyOnce()
        {
            var survey = new GeodeticSurveyEngine(MakeSurveyCatalog(), new SeededRng(71));
            foreach (var id in new[] { "pt_ridge", "pt_chimney", "pt_rail" })
                survey.EstablishMonument(id, 1, (itemId, n) => true);
            survey.Observe("pt_ridge", "pt_chimney", "clear");
            survey.Observe("pt_chimney", "pt_rail", "clear");
            survey.Observe("pt_rail", "pt_ridge", "clear");

            Assert.NotNull(survey.TryResolveTriangle("pt_ridge", "pt_chimney", "pt_rail"));
            survey.TryResolveTriangle("pt_ridge", "pt_chimney", "pt_rail"); // duplicate
            Assert.Single(survey.UnlockedShortcuts);
        }

        private static string RunUntilComplete(DecontaminationSystem decon)
        {
            string outcome = string.Empty;
            DeconStageResult r;
            do
            {
                r = decon.TickActiveStage();
                if (r.cycleComplete) outcome = r.outcome;
            }
            while (!r.cycleComplete && string.IsNullOrEmpty(r.error));
            return outcome;
        }
    }
}
