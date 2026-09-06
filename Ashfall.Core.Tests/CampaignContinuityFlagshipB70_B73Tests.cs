// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Content;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CampaignContinuityFlagshipB70_B73Tests
    {
        private sealed class CampaignContext
        {
            public Ashfall.Core.SeededRng Rng { get; }
            public Inventory.Inventory Inventory { get; }
            public PowerGridSystem Power { get; }
            public YearOfAshDeepFreezeSystem DeepFreeze { get; }
            public SumpFloodingSystem Sump { get; }
            public VentilationSystem Ventilation { get; }
            public WeatherSystem Weather { get; }
            public WeatherSondeSystem Sonde { get; }
            public RailwaySystem Railway { get; }

            public CampaignContext(int seed)
            {
                Rng = new Ashfall.Core.SeededRng(seed);
                Inventory = new Inventory.Inventory();

                // Stock initial supplies for sump, ventilation, rail, and sonde
                Inventory.AddById("item_flocculant_dose", 20);
                Inventory.AddById("item_filter_cloth", 20);
                Inventory.AddById("scrap_metal", 50);
                Inventory.AddById("steel_rail_segment", 10);
                Inventory.AddById("railroad_ties", 10);
                Inventory.AddById("train_coal", 500);

                // Setup shared weather and power
                Weather = new WeatherSystem();
                Weather.BindProfile(new SeasonProfileDef
                {
                    id = "post_nuclear_spring",
                    displayName = "Spring of Ash",
                    weatherCheckIntervalHours = 6f,
                    seasons = new List<SeasonWindowDef>
                    {
                        new SeasonWindowDef
                        {
                            id = "ash_season",
                            displayName = "Ash Season",
                            startDay = 0,
                            clearWeight = 1f,
                            rainWeight = 1f,
                            overcastWeight = 1f,
                            ashfallWeight = 1f
                        }
                    }
                }, seed + 71);

                var powerState = new PowerGridState
                {
                    GenerationWatts = 1200,
                    FuelUnits = 100,
                    BatteryCapacityWh = 6000,
                    BatteryReserveWh = 3000
                };
                var powerRooms = new List<PowerGridRoom>
                {
                    new PowerGridRoom("sump_deep_basin", "Deep Drainage Basin", 150f),
                    new PowerGridRoom("room_core", "Shelter Core", 200f)
                };
                Power = new PowerGridSystem(powerState, powerRooms, new SeededRng(seed + 42));
                DeepFreeze = new YearOfAshDeepFreezeSystem();

                // 1. Sump Drainage System (Plan 70)
                Sump = new SumpFloodingSystem(new SeededRng(seed + 70), Weather, Power, DeepFreeze);
                Sump.AddNode("sump_deep_basin", "Deep Drainage Basin", 250f);
                Sump.ApplyStratumCatalog(new List<SumpStratumDef>
                {
                    new SumpStratumDef
                    {
                        stratum_id = "stratum_alluvial",
                        display_name = "Alluvial Silt Layer",
                        base_ingress_cm_per_day = 8f,
                        water_table_pressure = 1.0f,
                        silt_fraction = 0.08f,
                        gas_risk_profile = "reduced_sulfur"
                    }
                });
                Sump.AssignStratum("sump_deep_basin", "stratum_alluvial");
                Sump.InstallPump("sump_deep_basin");
                Sump.SetNodePower("sump_deep_basin", true);
                Sump.BindServices(Inventory, null, null);

                // 2. Ventilation & Electrostatic Filtration (Plan 72)
                var starting = new StartingLevelSystem();
                starting.State.airFilterHealthPercent = 100f;
                Ventilation = new VentilationSystem(starting);
                var stageDef = new ElectrostaticStageDef
                {
                    stage_id = "stage_precipitator_standard",
                    display_name = "Single-Stage Wire Precipitator",
                    dust_capacity_kg = 50f,
                    operating_profiles = new List<ElectrostaticProfileDef>
                    {
                        new ElectrostaticProfileDef
                        {
                            profile_id = "profile_standard",
                            display_name = "Continuous Baseline",
                            nominal_power_w = 400f,
                            capture_efficiency_pm25 = 0.95f,
                            capture_efficiency_pm10 = 0.98f,
                            hot_ash_capture_efficiency = 0.85f,
                            ozone_output_rate_ppm_per_day = 0.5f,
                            arc_risk_base_bp = 10
                        }
                    }
                };
                Ventilation.ApplyElectrostaticCatalog(new List<ElectrostaticStageDef> { stageDef });
                Ventilation.BindStageServices(new SeededRng(seed + 72), Power, Inventory, null);
                Ventilation.InstallElectrostaticStage("stage_precipitator_standard", "room_core");

                // 3. Weather Sonde (Plan 71)
                Sonde = new WeatherSondeSystem(Weather);
                Sonde.ApplySoundingCatalog(
                    new List<SoundingAltitudeBandDef>
                    {
                        new SoundingAltitudeBandDef
                        {
                            band_id = "band_troposphere",
                            altitude_min_m = 0f,
                            altitude_max_m = 30000f,
                            wind_variability = 1.0f,
                            radiation_sampling_modifier = 1.0f,
                            telemetry_quality_modifier = 1.0f
                        }
                    },
                    new List<SoundingPayloadDef>
                    {
                        new SoundingPayloadDef
                        {
                            payload_id = "payload_radiosonde_telemetry",
                            display_name = "Standard Radiosonde Telemetry Pack",
                            burst_altitude_min_m = 24000f,
                            burst_altitude_max_m = 30000f,
                            parachute_descent_rate_km_per_tick = 6f,
                            recovery_rewards = new List<SoundingRecoveryRewardDef>
                            {
                                new SoundingRecoveryRewardDef { min_condition = 0f, item_id = "scrap_metal", amount = 3 }
                            }
                        }
                    });
                Sonde.BindRecoveryInventory(Inventory);

                // 4. Railway Network & Logistics (Plan 73)
                Railway = new RailwaySystem(new SeededRng(seed + 73), Inventory);
                Railway.RegisterCatalog(new RailwayNetworkCatalog
                {
                    nodes = new List<RailNodeDef>
                    {
                        new RailNodeDef { node_id = "terminal_shelter", display_name = "Shelter Rail Depot" },
                        new RailNodeDef { node_id = "waypoint_quarry", display_name = "Quarry Siding" },
                        new RailNodeDef { node_id = "outpost_smelter", display_name = "Smelter Outpost" }
                    },
                    segments = new List<TrackSegmentDef>
                    {
                        new TrackSegmentDef
                        {
                            segment_id = "seg_shelter_to_quarry",
                            display_name = "Valley Route",
                            start_node_id = "terminal_shelter",
                            end_node_id = "waypoint_quarry",
                            distance_km = 15f,
                            base_integrity = 0.85f,
                            bridge_required = false,
                            max_train_mass = 250f
                        },
                        new TrackSegmentDef
                        {
                            segment_id = "seg_quarry_to_smelter",
                            display_name = "Gorge Crossing",
                            start_node_id = "waypoint_quarry",
                            end_node_id = "outpost_smelter",
                            distance_km = 20f,
                            base_integrity = 0.70f,
                            bridge_required = true,
                            max_train_mass = 250f
                        }
                    },
                    cars = new List<TrainCarDef>
                    {
                        new TrainCarDef { car_type_id = "car_locomotive_diesel", empty_mass = 60f, max_fuel_capacity = 400f },
                        new TrainCarDef { car_type_id = "car_freight_hopper", empty_mass = 25f }
                    }
                });

                Railway.RegisterLogisticsCatalog(new List<RailLogisticsEdgeDef>
                {
                    new RailLogisticsEdgeDef
                    {
                        rail_edge_id = "edge_shelter_quarry",
                        from_node = "terminal_shelter",
                        to_node = "waypoint_quarry",
                        grade = 0.01f,
                        derailment_risk_bp = 50
                    },
                    new RailLogisticsEdgeDef
                    {
                        rail_edge_id = "edge_quarry_smelter",
                        from_node = "waypoint_quarry",
                        to_node = "outpost_smelter",
                        grade = 0.035f,
                        derailment_risk_bp = 200
                    }
                });

                Railway.CreateStarterTrain("armored_train_01", "Grizzly Ex-01", "terminal_shelter");
            }

            public void SimulateDay(int day)
            {
                // Day 2: Launch radiosonde sounding flight
                if (day == 2)
                {
                    Sonde.Launch("sonde_flight_01", day, 6.0f, 1.0f, 1.0f);
                }

                // Day 3: Sounding flight advances with ticks
                if (day == 3 && Sonde.IsLaunched && !Sonde.IsComplete)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (!Sonde.Tick(Rng, day) || Sonde.State.isRecovered) break;
                    }
                }

                // Day 5: Flocculate sump basin solids
                if (day == 5)
                {
                    Sump.StartFlocculation("sump_deep_basin", 1);
                }

                // Day 7: Dispatch train onto first segment
                if (day == 7)
                {
                    Railway.DispatchTrain("armored_train_01", "seg_shelter_to_quarry");
                    Railway.TickTravel("armored_train_01", 0.6f);
                }

                // Day 8: Complete train travel to quarry
                if (day == 8)
                {
                    Railway.TickTravel("armored_train_01", 0.6f);
                }

                // Day 10: Rap electrostatic precipitator plates to dump dust into hopper
                if (day == 10)
                {
                    Ventilation.RapPlates();
                    Sump.RunCentrifugeBatch("sump_deep_basin");
                }

                // Day 12: Pack dewatered sludge cake
                if (day == 12)
                {
                    Sump.PackCakeForSmelting(5);
                }

                // Day 14: Recover landed sonde payload
                if (day == 14 && Sonde.State.isRecovered)
                {
                    Sonde.ClaimRecoveryPayload(day);
                }

                // Day 18: Sabotage / obstacle clears on track & bridge repair
                if (day == 18)
                {
                    var seg = Railway.EnsureSegmentState("seg_quarry_to_smelter");
                    seg.isSabotaged = true;
                    Railway.ClearTrackObstacle("seg_quarry_to_smelter");
                    Railway.RepairTrack("seg_quarry_to_smelter", 0.2f);
                }

                // Day 20: Dispatch train onto second segment (Gorge Crossing)
                if (day == 20)
                {
                    Railway.DispatchTrain("armored_train_01", "seg_quarry_to_smelter");
                    Railway.TickTravel("armored_train_01", 0.5f);
                }

                // Day 22: Empty electrostatic hopper into sealed tailings drum
                if (day == 22)
                {
                    Ventilation.EmptyHopperToDrums(1);
                }

                // Day 24: Second train leg arrives at smelter
                if (day == 24)
                {
                    Railway.TickTravel("armored_train_01", 0.6f);
                }

                // Sequential daily tick across all 4 systems
                Sump.TickDay(day);
                Ventilation.TickDay(day);
                Weather.Tick(24f);
            }
        }

        [Fact]
        public void Continuity_30DayDeterministicReplay_ProducesIdenticalOutputs()
        {
            var runA = new CampaignContext(7073);
            for (int day = 1; day <= 30; day++)
                runA.SimulateDay(day);

            var runB = new CampaignContext(7073);
            for (int day = 1; day <= 30; day++)
                runB.SimulateDay(day);

            // 1. Sump Flooding & Centrifuge Output Parity
            var nodeA = runA.Sump.State.nodes.Find(n => n.nodeId == "sump_deep_basin")!;
            var nodeB = runB.Sump.State.nodes.Find(n => n.nodeId == "sump_deep_basin")!;
            Assert.Equal(nodeA.waterLevelCm, nodeB.waterLevelCm, precision: 2);
            Assert.Equal(nodeA.settledSludgeKg, nodeB.settledSludgeKg, precision: 2);
            Assert.Equal(runA.Sump.State.dewateredCakeKg, runB.Sump.State.dewateredCakeKg, precision: 2);
            Assert.Equal(runA.Sump.State.centrifugeCondition, runB.Sump.State.centrifugeCondition, precision: 2);

            // 2. Electrostatic Ventilation Parity
            var stageA = runA.Ventilation.State.electrostatic!;
            var stageB = runB.Ventilation.State.electrostatic!;
            Assert.Equal(stageA.dustLoadKg, stageB.dustLoadKg, precision: 3);
            Assert.Equal(stageA.hopperKg, stageB.hopperKg, precision: 3);
            Assert.Equal(stageA.plateCondition, stageB.plateCondition, precision: 2);

            // 3. Atmospheric Sounding Parity
            Assert.Equal(runA.Sonde.State.currentAltitudeKm, runB.Sonde.State.currentAltitudeKm, precision: 2);
            Assert.Equal(runA.Sonde.State.driftEastKm, runB.Sonde.State.driftEastKm, precision: 2);
            Assert.Equal(runA.Sonde.State.driftNorthKm, runB.Sonde.State.driftNorthKm, precision: 2);
            Assert.Equal(runA.Sonde.State.isRecovered, runB.Sonde.State.isRecovered);

            // 4. Railway Logistics & Dispatch Parity
            var trainA = runA.Railway.GetTrain("armored_train_01")!;
            var trainB = runB.Railway.GetTrain("armored_train_01")!;
            Assert.Equal(trainA.currentNodeId, trainB.currentNodeId);
            Assert.Equal(trainA.currentFuel, trainB.currentFuel, precision: 2);
            Assert.Equal(trainA.status, trainB.status);
            Assert.Equal(runA.Railway.EnsureSegmentState("seg_quarry_to_smelter").integrity,
                         runB.Railway.EnsureSegmentState("seg_quarry_to_smelter").integrity, precision: 2);

            // 5. Consumed & Produced Inventory Parity
            Assert.Equal(runA.Inventory.CountById("scrap_metal"), runB.Inventory.CountById("scrap_metal"));
            Assert.Equal(runA.Inventory.CountById("steel_rail_segment"), runB.Inventory.CountById("steel_rail_segment"));
            Assert.Equal(runA.Inventory.CountById(SumpFloodingSystem.SludgeCakeItemId), runB.Inventory.CountById(SumpFloodingSystem.SludgeCakeItemId));
        }

        [Fact]
        public void Continuity_Day15SaveRestoreSplit_MatchesContinuousRun()
        {
            // Continuous run to Day 30
            var continuous = new CampaignContext(8084);
            for (int day = 1; day <= 30; day++)
                continuous.SimulateDay(day);

            // Split run: Day 1-15, Save, Restore, Day 16-30
            var split = new CampaignContext(8084);
            for (int day = 1; day <= 15; day++)
                split.SimulateDay(day);

            // Capture state at Day 15
            var sumpSave = split.Sump.CaptureState();
            var ventSave = split.Ventilation.CaptureState();
            var sondeSave = split.Sonde.CaptureState();
            var railSave = split.Railway.State;
            var weatherSave = split.Weather.CaptureState();
            var invSave = split.Inventory.CaptureState();
            ulong rngState = split.Rng.PeekState();

            // Create fresh context and restore
            var restored = new CampaignContext(8084);
            restored.Sump.RestoreState(sumpSave);
            restored.Ventilation.RestoreState(ventSave);
            restored.Sonde.RestoreState(sondeSave);
            restored.Railway.RestoreState(railSave);
            restored.Weather.RestoreState(weatherSave);
            restored.Inventory.RestoreState(invSave, id => new ItemDefinition { id = id });
            restored.Rng.SeekState(rngState);

            // Fast-forward restored context to Day 30
            for (int day = 16; day <= 30; day++)
                restored.SimulateDay(day);

            // Verify parity at Day 30
            var nodeCont = continuous.Sump.State.nodes.Find(n => n.nodeId == "sump_deep_basin")!;
            var nodeRest = restored.Sump.State.nodes.Find(n => n.nodeId == "sump_deep_basin")!;
            Assert.Equal(nodeCont.waterLevelCm, nodeRest.waterLevelCm, precision: 1);
            Assert.Equal(nodeCont.settledSludgeKg, nodeRest.settledSludgeKg, precision: 1);
            Assert.Equal(continuous.Sump.State.dewateredCakeKg, restored.Sump.State.dewateredCakeKg, precision: 1);

            var stageCont = continuous.Ventilation.State.electrostatic!;
            var stageRest = restored.Ventilation.State.electrostatic!;
            Assert.Equal(stageCont.dustLoadKg, stageRest.dustLoadKg, precision: 2);
            Assert.Equal(stageCont.hopperKg, stageRest.hopperKg, precision: 2);

            var trainCont = continuous.Railway.GetTrain("armored_train_01")!;
            var trainRest = restored.Railway.GetTrain("armored_train_01")!;
            Assert.Equal(trainCont.currentNodeId, trainRest.currentNodeId);
            Assert.Equal(trainCont.currentFuel, trainRest.currentFuel, precision: 1);
            Assert.Equal(trainCont.status, trainRest.status);
        }
    }
}
