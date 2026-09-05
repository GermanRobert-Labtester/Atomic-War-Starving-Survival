using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plans130To133CoreTests
    {
        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var path))
                return path;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data was not found.");
        }

        [Fact]
        public void Catalogs_LoadFromAuthoritativeDataDirectory()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var powder = PowderMetallurgyCatalogLoader.Load(DataDir(), files, json);
            var nvis = NvisCommunicationsCatalogLoader.Load(DataDir(), files, json);
            var lyophilization = LyophilizationCatalogLoader.Load(DataDir(), files, json);
            var rerailing = RerailingEquipmentCatalogLoader.Load(DataDir(), files, json);

            Assert.Equal(2, powder.processes.Count);
            Assert.Equal(2, nvis.channels.Count);
            Assert.Equal(2, lyophilization.recipes.Count);
            Assert.Equal(2, rerailing.equipment.Count);
        }

        [Fact]
        public void PowderMetallurgy_ConsumesFeedstockAtomically_AndProducesQualityRecord()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("scrap_metal", 4);
            inventory.AddById("mechanical_parts", 1);
            var system = new PowderMetallurgySystem(inventory, new SeededRng(130));
            system.LoadCatalog(new PowderMetallurgyCatalog
            {
                processes = new List<PowderMetallurgyProcessDefinition>
                {
                    new PowderMetallurgyProcessDefinition
                    {
                        process_id = "process_test",
                        feedstock_costs = new List<PowderMetallurgyFeedstockCost>
                        {
                            new PowderMetallurgyFeedstockCost { item_id = "scrap_metal", amount = 4 },
                            new PowderMetallurgyFeedstockCost { item_id = "mechanical_parts", amount = 1 }
                        },
                        output_item_id = "item_foundry_structural_coupling",
                        output_units = 1,
                        duration_days = 2,
                        quality_floor = 0.5f,
                        quality_ceiling = 0.9f
                    }
                }
            });

            Assert.True(system.StartBatch("process_test", 4).IsSuccess);
            Assert.Equal(0, inventory.CountById("scrap_metal"));
            Assert.Equal(0, inventory.CountById("mechanical_parts"));
            Assert.True(system.TickDay(5).IsSuccess);
            Assert.True(system.TickDay(6).IsSuccess);

            Assert.Equal(1, inventory.CountById("item_foundry_structural_coupling"));
            Assert.Equal(PowderMetallurgyStatus.Ready, system.State.status);
            Assert.True(system.TryGetLatestModifier("item_foundry_structural_coupling", out var modifier));
            Assert.InRange(modifier.quality01, 0.5f, 0.9f);
            Assert.InRange(modifier.ReadinessMultiplier, 0.5f, 1.1f);
        }

        [Fact]
        public void PowderMetallurgy_MissingFeedstockDoesNotCreateJob()
        {
            var inventory = new Inventory.Inventory();
            var system = new PowderMetallurgySystem(inventory);
            system.LoadCatalog(new PowderMetallurgyCatalog
            {
                processes = new List<PowderMetallurgyProcessDefinition>
                {
                    new PowderMetallurgyProcessDefinition
                    {
                        process_id = "process_test",
                        feedstock_costs = new List<PowderMetallurgyFeedstockCost>
                        {
                            new PowderMetallurgyFeedstockCost { item_id = "scrap_metal", amount = 2 }
                        }
                    }
                }
            });

            Assert.False(system.StartBatch("process_test", 1).IsSuccess);
            Assert.Equal(PowderMetallurgyStatus.Ready, system.State.status);
            Assert.Empty(system.State.batches);
        }

        [Fact]
        public void PowderMetallurgy_SaveRoundTripPreservesActiveJob()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("scrap_metal", 2);
            var system = new PowderMetallurgySystem(inventory);
            system.LoadCatalog(new PowderMetallurgyCatalog
            {
                processes = new List<PowderMetallurgyProcessDefinition>
                {
                    new PowderMetallurgyProcessDefinition
                    {
                        process_id = "process_test",
                        feedstock_costs = new List<PowderMetallurgyFeedstockCost>
                        {
                            new PowderMetallurgyFeedstockCost { item_id = "scrap_metal", amount = 2 }
                        },
                        output_item_id = "item_foundry_casing_blanks",
                        duration_days = 3
                    }
                }
            });
            Assert.True(system.StartBatch("process_test", 8).IsSuccess);
            Assert.True(system.TickDay(9).IsSuccess);

            var restored = new PowderMetallurgySystem(inventory);
            restored.LoadCatalog(new PowderMetallurgyCatalog
            {
                processes = new List<PowderMetallurgyProcessDefinition>
                {
                    new PowderMetallurgyProcessDefinition
                    {
                        process_id = "process_test",
                        output_item_id = "item_foundry_casing_blanks",
                        duration_days = 3
                    }
                }
            });
            restored.RestoreState(system.CaptureState());

            Assert.Equal(PowderMetallurgyStatus.Processing, restored.State.status);
            Assert.Equal(1, restored.State.days_elapsed);
            Assert.Equal("process_test", restored.State.active_process_id);
        }

        [Fact]
        public void Nvis_TransmissionUsesSeededOutcome_AndQueuesRecallOnce()
        {
            var catalog = new NvisCommunicationsCatalog
            {
                channels = new List<NvisChannelDefinition>
                {
                    new NvisChannelDefinition
                    {
                        channel_id = "channel_test",
                        base_signal_quality = 1f,
                        required_power_watts = 1f,
                        recall_capable = true
                    }
                }
            };
            var system = new NvisCommunicationsSystem(new SeededRng(131), () => 10f);
            system.LoadCatalog(catalog);
            Assert.True(system.SelectChannel("channel_test").IsSuccess);
            Assert.True(system.BeginStatusTransmission("status", 2, 0).IsSuccess);
            system.TickDay(3);

            Assert.Equal(1, system.State.total_transmissions);
            Assert.Equal(1, system.State.delivered_transmissions);
            Assert.True(system.RequestRecall("survivor_1", 3).IsSuccess);
            Assert.False(system.RequestRecall("survivor_1", 3).IsSuccess);
            Assert.True(system.AcknowledgeRecall("survivor_1"));
            Assert.Equal(NvisCommunicationsMode.Listening, system.State.mode);
        }

        [Fact]
        public void Nvis_SaveRoundTripPreservesTransmissionAndRecallLedger()
        {
            var system = new NvisCommunicationsSystem(new SeededRng(9));
            system.LoadCatalog(new NvisCommunicationsCatalog
            {
                channels = new List<NvisChannelDefinition>
                {
                    new NvisChannelDefinition { channel_id = "channel_test", recall_capable = true }
                }
            });
            Assert.True(system.BeginStatusTransmission("status", 1, 0).IsSuccess);
            system.TickDay(2);
            Assert.True(system.RequestRecall("survivor_1", 2).IsSuccess);

            var restored = new NvisCommunicationsSystem(new SeededRng(9));
            restored.LoadCatalog(new NvisCommunicationsCatalog
            {
                channels = new List<NvisChannelDefinition>
                {
                    new NvisChannelDefinition { channel_id = "channel_test", recall_capable = true }
                }
            });
            restored.RestoreState(system.CaptureState());

            Assert.Single(restored.State.transmissions);
            Assert.Single(restored.State.recall_requests);
            Assert.Equal(system.State.mode, restored.State.mode);
            Assert.Equal(system.State.last_contact_day, restored.State.last_contact_day);
        }

        [Fact]
        public void Lyophilization_ConsumesInputsAtomically_AndExpiresBatches()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("blood_sample", 1);
            inventory.AddById("item_hermetic_sample_ampoule", 1);
            var system = new LyophilizationSystem(inventory, new SeededRng(132));
            system.LoadCatalog(new LyophilizationCatalog
            {
                recipes = new List<LyophilizationRecipeDefinition>
                {
                    new LyophilizationRecipeDefinition
                    {
                        recipe_id = "recipe_test",
                        input_item_id = "blood_sample",
                        container_item_id = "item_hermetic_sample_ampoule",
                        output_item_id = "item_medical_saline_salt",
                        duration_days = 1,
                        shelf_life_days = 2,
                        base_viability01 = 0.8f,
                        viability_variance01 = 0f
                    }
                }
            });

            Assert.True(system.StartBatch("recipe_test", 5).IsSuccess);
            Assert.Equal(0, inventory.CountById("blood_sample"));
            Assert.True(system.TickDay(6).IsSuccess);
            Assert.Single(system.State.batches);
            string batchId = system.State.batches[0].batch_id;
            Assert.True(system.CanUseBatch(batchId, 7));
            Assert.True(system.CanUseBatch(batchId, 8));
            Assert.False(system.TryUseBatch(batchId, 9, 1, out _, out var reason));
            Assert.Equal("expired", reason);
            Assert.True(system.State.batches[0].spoiled);
        }

        [Fact]
        public void Lyophilization_SaveRoundTripPreservesViabilityLedger()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("organic_residue", 2);
            inventory.AddById("item_hermetic_sample_ampoule", 1);
            var system = new LyophilizationSystem(inventory, new SeededRng(132));
            system.LoadCatalog(new LyophilizationCatalog
            {
                recipes = new List<LyophilizationRecipeDefinition>
                {
                    new LyophilizationRecipeDefinition
                    {
                        recipe_id = "recipe_test",
                        input_item_id = "organic_residue",
                        container_item_id = "item_hermetic_sample_ampoule",
                        output_item_id = "item_medical_saline_salt",
                        duration_days = 2
                    }
                }
            });
            Assert.True(system.StartBatch("recipe_test", 1).IsSuccess);
            Assert.True(system.TickDay(2).IsSuccess);
            Assert.True(system.TickDay(3).IsSuccess);
            var saved = system.CaptureState();

            var restored = new LyophilizationSystem(inventory);
            restored.RestoreState(saved);

            Assert.Single(restored.State.batches);
            Assert.Equal(saved.batches[0].viability01, restored.State.batches[0].viability01);
            Assert.Equal(saved.batches[0].expiry_day, restored.State.batches[0].expiry_day);
        }

        [Fact]
        public void DraisineRecovery_ConsumesTool_AndRestoresCanonicalRailwayState()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("item_hydraulic_actuator", 1);
            var railway = NewRailway();
            var train = railway.CreateStarterTrain("train_test", "Test Draisine", "node_a");
            train.activeSegmentId = "segment_test";
            train.status = TrainDispatchStatus.Derailment;
            train.cars[0].condition = 40f;
            var recovery = new DraisineRerailingSystem(inventory, railway, new SeededRng(133), () => 500f);
            recovery.LoadCatalog(new RerailingEquipmentCatalog
            {
                equipment = new List<RerailingEquipmentDefinition>
                {
                    new RerailingEquipmentDefinition
                    {
                        equipment_id = "rerail_test",
                        required_item_id = "item_hydraulic_actuator",
                        required_power_watts = 100f,
                        duration_days = 1,
                        success_chance01 = 1f,
                        train_condition_restored = 20f,
                        track_integrity_restored = 0.1f
                    }
                }
            });

            Assert.True(recovery.StartRecovery("train_test", "rerail_test", 10).IsSuccess);
            Assert.Equal(0, inventory.CountById("item_hydraulic_actuator"));
            Assert.True(recovery.TickDay(11).IsSuccess);
            Assert.Equal(DraisineRecoveryStatus.Recovered, recovery.State.status);
            Assert.Equal(TrainDispatchStatus.Idle, train.status);
            Assert.Equal(60f, train.cars[0].condition);
        }

        [Fact]
        public void DraisineRecovery_FailedAttemptDoesNotPretendTrainWasRecovered()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("item_hydraulic_actuator", 1);
            var railway = NewRailway();
            var train = railway.CreateStarterTrain("train_test", "Test Draisine", "node_a");
            train.activeSegmentId = "segment_test";
            train.status = TrainDispatchStatus.Derailment;
            var recovery = new DraisineRerailingSystem(inventory, railway, new SeededRng(133));
            recovery.LoadCatalog(new RerailingEquipmentCatalog
            {
                equipment = new List<RerailingEquipmentDefinition>
                {
                    new RerailingEquipmentDefinition
                    {
                        equipment_id = "rerail_test",
                        required_item_id = "item_hydraulic_actuator",
                        duration_days = 1,
                        success_chance01 = 0f
                    }
                }
            });

            Assert.True(recovery.StartRecovery("train_test", "rerail_test", 1).IsSuccess);
            Assert.False(recovery.TickDay(2).IsSuccess);
            Assert.Equal(DraisineRecoveryStatus.Failed, recovery.State.status);
            Assert.Equal(TrainDispatchStatus.Derailment, train.status);
        }

        [Fact]
        public void DraisineRecovery_SaveRoundTripPreservesActiveJob()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById("item_hydraulic_actuator", 1);
            var railway = NewRailway();
            var train = railway.CreateStarterTrain("train_test", "Test Draisine", "node_a");
            train.activeSegmentId = "segment_test";
            train.status = TrainDispatchStatus.Derailment;
            var recovery = new DraisineRerailingSystem(inventory, railway);
            recovery.LoadCatalog(new RerailingEquipmentCatalog
            {
                equipment = new List<RerailingEquipmentDefinition>
                {
                    new RerailingEquipmentDefinition
                    {
                        equipment_id = "rerail_test",
                        required_item_id = "item_hydraulic_actuator",
                        duration_days = 3
                    }
                }
            });
            Assert.True(recovery.StartRecovery("train_test", "rerail_test", 1).IsSuccess);
            Assert.True(recovery.TickDay(2).IsSuccess);

            var restored = new DraisineRerailingSystem(inventory, railway);
            restored.LoadCatalog(new RerailingEquipmentCatalog
            {
                equipment = new List<RerailingEquipmentDefinition>
                {
                    new RerailingEquipmentDefinition { equipment_id = "rerail_test", duration_days = 3 }
                }
            });
            restored.RestoreState(recovery.CaptureState());

            Assert.Equal(DraisineRecoveryStatus.Rerailing, restored.State.status);
            Assert.Equal(1, restored.State.days_elapsed);
            Assert.Equal("train_test", restored.State.train_id);
        }

        private static RailwaySystem NewRailway()
        {
            var railway = new RailwaySystem();
            railway.RegisterCatalog(new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "node_a" },
                    new RailNodeDef { node_id = "node_b" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "segment_test",
                        start_node_id = "node_a",
                        end_node_id = "node_b",
                        base_integrity = 0.5f
                    }
                },
                cars = new List<TrainCarDef>
                {
                    new TrainCarDef
                    {
                        car_type_id = "car_locomotive_diesel",
                        empty_mass = 70f
                    },
                    new TrainCarDef
                    {
                        car_type_id = "car_freight_hopper",
                        empty_mass = 25f
                    }
                }
            });
            return railway;
        }
    }
}
