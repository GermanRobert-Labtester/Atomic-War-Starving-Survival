using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Creates quest-enabled map nodes for Prompts #85-#94. Each node wraps a
    /// standard MapNode with quest-specific encounter data. Called by GameBootstrap
    /// after map generation to sprinkle quest locations onto the wasteland graph.
    /// </summary>
    public static class LocationQuestNodeFactory
    {
        public struct QuestNodeDef
        {
            public string NodeId;
            public string DisplayName;
            public Environment.DangerRing Ring;
            public float DistanceFromShelter;
            public float TrueRad;
            public float DangerLevel;
            public string QuestType;
            public bool HasUxo;
            public string LootTableId;
        }

        /// <summary>All 10 quest node definitions.</summary>
        public static List<QuestNodeDef> AllDefinitions()
        {
            return new List<QuestNodeDef>
            {
                // Prompt #85
                new QuestNodeDef { NodeId = "node_hospital", DisplayName = "Ruined Hospital",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 8f,
                    TrueRad = 25f, DangerLevel = 3f, QuestType = "hospital_centrifuge",
                    LootTableId = "medical_supply" },

                // Prompt #86
                new QuestNodeDef { NodeId = "node_water_plant", DisplayName = "Water Treatment Plant",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 6f,
                    TrueRad = 40f, DangerLevel = 3f, QuestType = "water_plant_flush",
                    LootTableId = "industrial_scrap" },

                // Prompt #87
                new QuestNodeDef { NodeId = "node_checkpoint", DisplayName = "Military Checkpoint",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 5f,
                    TrueRad = 15f, DangerLevel = 4f, QuestType = "checkpoint_radio",
                    HasUxo = true, LootTableId = "military_supply" },

                // Prompt #88
                new QuestNodeDef { NodeId = "node_school", DisplayName = "Elementary School",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 3f,
                    TrueRad = 18f, DangerLevel = 1f, QuestType = "school_manifest",
                    LootTableId = "civilian_supply" },

                // Prompt #89
                new QuestNodeDef { NodeId = "node_hardware", DisplayName = "Hardware Store",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 4f,
                    TrueRad = 10f, DangerLevel = 2f, QuestType = "hardware_safe",
                    LootTableId = "industrial_scrap" },

                // Prompt #90
                new QuestNodeDef { NodeId = "node_highway", DisplayName = "Highway Pileup",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 7f,
                    TrueRad = 30f, DangerLevel = 3f, QuestType = "highway_siphon",
                    LootTableId = "vehicle_scrap" },

                // Prompt #91
                new QuestNodeDef { NodeId = "node_substation", DisplayName = "Electrical Substation",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 5f,
                    TrueRad = 20f, DangerLevel = 2f, QuestType = "substation_grid",
                    LootTableId = "electronic_scrap" },

                // Prompt #92
                new QuestNodeDef { NodeId = "node_gardens", DisplayName = "Botanical Gardens",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 9f,
                    TrueRad = 12f, DangerLevel = 2f, QuestType = "gardens_seeds",
                    LootTableId = "botanical_supply" },

                // Prompt #93
                new QuestNodeDef { NodeId = "node_cult_church", DisplayName = "Church of the Glow",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 10f,
                    TrueRad = 35f, DangerLevel = 5f, QuestType = "cult_church_martyr",
                    LootTableId = "cult_supply" },

                // Prompt #94
                new QuestNodeDef { NodeId = "node_train", DisplayName = "Abandoned Train",
                    Ring = Environment.DangerRing.GroundZero, DistanceFromShelter = 12f,
                    TrueRad = 45f, DangerLevel = 4f, QuestType = "train_derailment",
                    LootTableId = "military_supply" },
                // #134
                new QuestNodeDef { NodeId = "node_airport", DisplayName = "International Airport",
                    Ring = Environment.DangerRing.GroundZero, DistanceFromShelter = 15f,
                    TrueRad = 55f, DangerLevel = 4f, QuestType = "airport_fuel", LootTableId = "industrial_supply" },
                // #135
                new QuestNodeDef { NodeId = "node_radio_tower", DisplayName = "Broadcast Tower",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 10f,
                    TrueRad = 20f, DangerLevel = 3f, QuestType = "radio_tower_climb", LootTableId = "electronic_scrap" },
                // #136
                new QuestNodeDef { NodeId = "node_pharmacy", DisplayName = "Looted Pharmacy",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 4f,
                    TrueRad = 8f, DangerLevel = 4f, QuestType = "pharmacy_traps", LootTableId = "medical_supply" },
                // #137
                new QuestNodeDef { NodeId = "node_mass_grave", DisplayName = "Mass Grave",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 3f,
                    TrueRad = 12f, DangerLevel = 1f, QuestType = "mass_grave_dig", LootTableId = "civilian_supply" },
                // #138
                new QuestNodeDef { NodeId = "node_prison", DisplayName = "Penitentiary",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 11f,
                    TrueRad = 18f, DangerLevel = 5f, QuestType = "prison_armory", LootTableId = "military_supply" },
                // #139
                new QuestNodeDef { NodeId = "node_railyard", DisplayName = "Railyard Depot",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 8f,
                    TrueRad = 22f, DangerLevel = 3f, QuestType = "railyard_tunnels", LootTableId = "industrial_scrap" },
                // #140
                new QuestNodeDef { NodeId = "node_library", DisplayName = "City Library",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 7f,
                    TrueRad = 10f, DangerLevel = 2f, QuestType = "library_books", LootTableId = "civilian_supply" },
                // #141
                new QuestNodeDef { NodeId = "node_decoy_bunker", DisplayName = "Decoy Bunker",
                    Ring = Environment.DangerRing.CityOutskirts, DistanceFromShelter = 9f,
                    TrueRad = 60f, DangerLevel = 5f, QuestType = "decoy_bunker_trap", LootTableId = "military_supply" },
                // #142
                new QuestNodeDef { NodeId = "node_water_tower", DisplayName = "Water Tower",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 5f,
                    TrueRad = 5f, DangerLevel = 3f, QuestType = "water_tower_extract", LootTableId = "civilian_supply" },
                // #143
                new QuestNodeDef { NodeId = "node_gun_store", DisplayName = "Gun Store (Burned)",
                    Ring = Environment.DangerRing.Suburbs, DistanceFromShelter = 4f,
                    TrueRad = 15f, DangerLevel = 2f, QuestType = "gun_store_lead", LootTableId = "industrial_scrap" }
            };
        }

        /// <summary>Convert a QuestNodeDef into a MapNode for the map graph.</summary>
        public static Environment.MapNode ToMapNode(QuestNodeDef def)
        {
            return new Environment.MapNode
            {
                NodeId = def.NodeId,
                DisplayName = def.DisplayName,
                Ring = def.Ring,
                DistanceFromShelter = def.DistanceFromShelter,
                TrueRad = def.TrueRad,
                DangerLevel = def.DangerLevel,
                HasUxo = def.HasUxo,
                LootTableId = def.LootTableId,
                IsRevealed = false,
                IsVisited = false,
                EncounterDeckIds = new List<string> { $"enc_{def.QuestType}" }
            };
        }
    }
}
