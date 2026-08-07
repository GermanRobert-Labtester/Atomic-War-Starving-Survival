using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Multi-stage location quests (Prompts #85-#94). Expedition nodes aren't
    /// loot pinatas; they're puzzles requiring multiple visits, specific gear,
    /// skill checks, and push-your-luck mechanics.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class LocationQuestSystem
    {
        /// <summary>Quest progress per location node.</summary>
        private readonly Dictionary<string, LocationQuestState> _quests = new Dictionary<string, LocationQuestState>();

        /// <summary>L-12: Lazy-init guard. Definitions are seeded on first access,
        /// not in the constructor, so test fixtures that only need one quest
        /// don't pay the cost of seeding all 14.</summary>
        private bool _seeded;

        // -- Events --
        public event Action<string, int> OnQuestStageAdvanced;  // nodeId, newStage
        public event Action<string, string> OnQuestCompleted;    // nodeId, rewardId
        public event Action<string, string> OnQuestFailed;       // nodeId, reason

        /// <summary>
        /// L-12: Constructor is cheap — definitions are seeded lazily on
        /// first access to GetQuest / TickDaily / CaptureState.
        /// </summary>
        public LocationQuestSystem() { }

        /// <summary>
        /// L-12: Lazy-init guard. Seeds quest definitions on first access
        /// rather than in the constructor.
        /// </summary>
        private void EnsureSeeded()
        {
            if (!_seeded) SeedQuestDefinitions();
        }

        /// <summary>Get the quest state for a node (never null).</summary>
        public LocationQuestState GetQuest(string nodeId)
        {
            EnsureSeeded();
            if (string.IsNullOrEmpty(nodeId)) return null;
            if (!_quests.TryGetValue(nodeId, out var state))
            {
                state = new LocationQuestState { NodeId = nodeId, QuestType = "unknown", CurrentStage = 0, MaxStages = 1 };
                _quests[nodeId] = state;
            }
            return state;
        }

        /// <summary>Whether a node has an active quest that isn't completed.</summary>
        public bool HasActiveQuest(string nodeId)
        {
            var q = GetQuest(nodeId);
            return q != null && !q.IsCompleted && q.QuestType != "unknown";
        }

        /// <summary>Advance a quest stage. Returns true if quest completed this call.</summary>
        public bool AdvanceStage(string nodeId)
        {
            var q = GetQuest(nodeId);
            if (q == null || q.IsCompleted || q.CurrentStage >= q.MaxStages) return false;

            q.CurrentStage++;
            OnQuestStageAdvanced?.Invoke(nodeId, q.CurrentStage);

            if (q.CurrentStage >= q.MaxStages)
            {
                q.IsCompleted = true;
                OnQuestCompleted?.Invoke(nodeId, q.RewardId);
                return true;
            }
            return false;
        }

        /// <summary>Mark a quest as failed.</summary>
        public void FailQuest(string nodeId, string reason)
        {
            var q = GetQuest(nodeId);
            if (q == null) return;
            q.IsFailed = true;
            OnQuestFailed?.Invoke(nodeId, reason);
        }

        /// <summary>
        /// Per-day housekeeping (SystemWiring). Decrements any active
        /// temporary-reward timers; expires quests whose temporary reward
        /// window has elapsed.
        /// </summary>
        public void TickDaily(int currentDay)
        {
            EnsureSeeded();
            if (_quests == null) return;
            foreach (var kv in _quests)
            {
                var q = kv.Value;
                if (q == null) continue;
                if (q.TemporaryRewardHoursRemaining > 0f)
                {
                    q.TemporaryRewardHoursRemaining = Mathf.Max(0f, q.TemporaryRewardHoursRemaining - 24f);
                    if (q.TemporaryRewardHoursRemaining <= 0f && !q.IsCompleted)
                    {
                        q.IsFailed = true;
                        OnQuestFailed?.Invoke(q.NodeId, "temporary_reward_expired");
                    }
                }
            }
        }

        // -----------------------------------------------------------------
        // Quest definitions (Prompts #85-#94)
        // -----------------------------------------------------------------

        private void SeedQuestDefinitions()
        {
            if (_seeded) return;
            _seeded = true;
            // Prompt #85 — Ruined Hospital
            _quests["node_hospital"] = new LocationQuestState
            {
                NodeId = "node_hospital", QuestType = "hospital_centrifuge",
                DisplayName = "The Ruined Hospital",
                CurrentStage = 0, MaxStages = 3,
                StageNames = new[] { "Floor 1: Medical Scrap", "Floor 2: Infected Ward", "Basement: The Centrifuge" },
                RewardId = "centrifuge",
                RewardDescription = "Unlocks AdvancedMedicalBed crafting — cures Sepsis.",
                RequiredSkill = "medical",
                RequiredSkillLevel = 0.5f,
                RequiredGear = new[] { "hazmat_suit" }
            };

            // Prompt #86 — Water Treatment Plant
            _quests["node_water_plant"] = new LocationQuestState
            {
                NodeId = "node_water_plant", QuestType = "water_plant_flush",
                DisplayName = "Water Treatment Plant",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Survey the Flooding", "Flush the Tanks (6h)" },
                RewardId = "renewable_dirty_water",
                RewardDescription = "Node permanently becomes a renewable DirtyWater source.",
                RequiredSkill = "mechanical",
                RequiredSkillLevel = 0.5f,
                RequiredGear = new[] { "hazmat_suit" },
                ActionHours = 6f
            };

            // Prompt #87 — Military Checkpoint
            _quests["node_checkpoint"] = new LocationQuestState
            {
                NodeId = "node_checkpoint", QuestType = "checkpoint_radio",
                DisplayName = "Military Checkpoint",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Navigate Minefield", "Commander's Tent" },
                RewardId = "encryption_codes",
                RewardDescription = "Unlocks 3 hidden Military Frequencies on the radio.",
                RequiredSkill = "perception",
                RequiredSkillLevel = 0.7f,
                DangerLevel = 4f,
                HasMinefield = true
            };

            // Prompt #88 — Elementary School
            _quests["node_school"] = new LocationQuestState
            {
                NodeId = "node_school", QuestType = "school_manifest",
                DisplayName = "Elementary School",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Search the Rationing Office", "Parse the Manifest (24h at shelter)" },
                RewardId = "ration_manifest",
                RewardDescription = "Reveals 5 hidden Supply Cache nodes on the world map.",
                ContaminationLevel = 0.6f,
                MentalBreakRisk = true,
                ParseHours = 24f
            };

            // Prompt #89 — Hardware Store
            _quests["node_hardware"] = new LocationQuestState
            {
                NodeId = "node_hardware", QuestType = "hardware_safe",
                DisplayName = "Hardware Store",
                CurrentStage = 0, MaxStages = 3,
                StageNames = new[] { "Find the Safe", "Carry Safe Back (50kg)", "Crack Safe at Bunker (12h + Saw)" },
                RewardId = "electronic_scrap_cache",
                RewardDescription = "High-tier ElectronicScrap yield.",
                HeavyLootWeight = 50f,
                ParseHours = 12f,
                RequiredItem = "saw"
            };

            // Prompt #90 — Highway Pileup
            _quests["node_highway"] = new LocationQuestState
            {
                NodeId = "node_highway", QuestType = "highway_siphon",
                DisplayName = "Highway Pileup",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Search the Wreckage", "Siphon Fuel (hours variable)" },
                RewardId = "siphoned_fuel",
                RewardDescription = "Significant fuel yield; risk of FalloutStorm exposure.",
                TetanusRisk = true,
                RequiredGear = new[] { "gloves" },
                PushYourLuck = true,
                ActionHours = 4f
            };

            // Prompt #91 — Substation
            _quests["node_substation"] = new LocationQuestState
            {
                NodeId = "node_substation", QuestType = "substation_grid",
                DisplayName = "Electrical Substation",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Approach Substation", "Reroute the Grid" },
                RewardId = "infinite_power_48h",
                RewardDescription = "Grants the shelter infinite power for 48 hours.",
                RequiredGear = new[] { "rubber_gloves" },
                TemporaryReward = true,
                TemporaryRewardHours = 48f
            };

            // Prompt #92 — Botanical Gardens
            _quests["node_gardens"] = new LocationQuestState
            {
                NodeId = "node_gardens", QuestType = "gardens_seeds",
                DisplayName = "Botanical Gardens",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Cross the Shattered Dome", "Deep Vault: Heirloom Seeds" },
                RewardId = "heirloom_seeds",
                RewardDescription = "Upgrades PlanterBox to 2x food yield (but attracts faction raids).",
                ExtremeCold = true,
                AttractsRaids = true
            };

            // Prompt #93 — Church of the Glow
            _quests["node_cult_church"] = new LocationQuestState
            {
                NodeId = "node_cult_church", QuestType = "cult_church_martyr",
                DisplayName = "Church of the Glow",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Approach the Church", "Rescue the Doctor" },
                RewardId = "rescued_doctor",
                RewardDescription = "Permanently raises shelter Medical stat ceiling.",
                DilemmaType = "assault_or_trade",
                DilemmaTradeItems = new[] { "irradiated_water:5", "geiger_counter:1" },
                CombatRisk = true
            };

            // Prompt #94 — Abandoned Train
            _quests["node_train"] = new LocationQuestState
            {
                NodeId = "node_train", QuestType = "train_derailment",
                DisplayName = "Abandoned Train",
                CurrentStage = 0, MaxStages = 3,
                StageNames = new[] { "Car 1: Safe Loot", "Car 2: Unstable (20% collapse)", "Car 3: Fatal Risk (50% collapse)" },
                RewardId = "military_armor",
                RewardDescription = "Military-grade armor from Car 3.",
                PushYourLuck = true,
                FatalRisk = true,
                PushLuckChances = new[] { 0f, 0.2f, 0.5f }
            };

            // ── Prompts #134–#143 ──────────────────────────────────────
            _quests["node_airport"] = new LocationQuestState
            {
                NodeId = "node_airport", QuestType = "airport_fuel", DisplayName = "International Airport",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Cross the Tarmac (tripled travel time)", "Siphon Aviation Fuel" },
                RewardId = "aviation_fuel", RewardDescription = "3x generator burn time, zero CO output.",
                ExtremeCold = true, ActionHours = 6f
            };
            _quests["node_radio_tower"] = new LocationQuestState
            {
                NodeId = "node_radio_tower", QuestType = "radio_tower_climb", DisplayName = "Broadcast Tower",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Climb the Tower (Agility check)", "Attach Signal Repeater" },
                RewardId = "global_radio", RewardDescription = "Unlocks international endgame broadcasts.",
                RequiredSkill = "agility", RequiredSkillLevel = 0.5f, FatalRisk = true
            };
            _quests["node_pharmacy"] = new LocationQuestState
            {
                NodeId = "node_pharmacy", QuestType = "pharmacy_traps", DisplayName = "Looted Pharmacy",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Descend to Basement", "Disarm Tripwires (Mechanical + Flashlight)" },
                RewardId = "antibiotics_cache", RewardDescription = "High-tier Antibiotics + Morphine.",
                RequiredSkill = "mechanical", RequiredSkillLevel = 0.5f, RequiredGear = new[] { "flashlight" }, DangerLevel = 4f
            };
            _quests["node_mass_grave"] = new LocationQuestState
            {
                NodeId = "node_mass_grave", QuestType = "mass_grave_dig", DisplayName = "Mass Grave",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Dig with Shovel", "Take the Jewelry and Gold" },
                RewardId = "grave_loot", RewardDescription = "Jewelry (high trade) + GoldTeeth. -20 bunker Morale.",
                RequiredItem = "shovel", MentalBreakRisk = true
            };
            _quests["node_prison"] = new LocationQuestState
            {
                NodeId = "node_prison", QuestType = "prison_armory", DisplayName = "Penitentiary",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Navigate Sealed Cellblocks", "Breach the Armory" },
                RewardId = "riot_gear", RewardDescription = "RiotGear (high armor) + TearGas (non-lethal raid end).",
                DangerLevel = 5f, CombatRisk = true
            };
            _quests["node_railyard"] = new LocationQuestState
            {
                NodeId = "node_railyard", QuestType = "railyard_tunnels", DisplayName = "Railyard Depot",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Clear the Depot", "Open Subway Access" },
                RewardId = "underground_travel", RewardDescription = "Halves travel time, no storm exposure, doubles mutant/gang encounters."
            };
            _quests["node_library"] = new LocationQuestState
            {
                NodeId = "node_library", QuestType = "library_books", DisplayName = "City Library",
                CurrentStage = 0, MaxStages = 1,
                StageNames = new[] { "Haul Books Back" },
                RewardId = "book_collection", RewardDescription = "Passive Morale + Skill regen aura OR burn as low-tier fuel.",
                HeavyLootWeight = 80f, ActionHours = 4f
            };
            _quests["node_decoy_bunker"] = new LocationQuestState
            {
                NodeId = "node_decoy_bunker", QuestType = "decoy_bunker_trap", DisplayName = "Decoy Bunker",
                CurrentStage = 0, MaxStages = 1,
                StageNames = new[] { "The vault seals behind you." },
                RewardId = "survival", RewardDescription = "Escape with your life. +500mSv dose triggers LatentPrognosis.",
                DangerLevel = 5f, FatalRisk = true
            };
            _quests["node_water_tower"] = new LocationQuestState
            {
                NodeId = "node_water_tower", QuestType = "water_tower_extract", DisplayName = "Water Tower",
                CurrentStage = 0, MaxStages = 2,
                StageNames = new[] { "Climb the Tower", "Extract Water (multiple trips)" },
                RewardId = "clean_water_50", RewardDescription = "50 gallons of CleanWater. Requires Buckets or HoseSystem.",
                ActionHours = 8f
            };
            _quests["node_gun_store"] = new LocationQuestState
            {
                NodeId = "node_gun_store", QuestType = "gun_store_lead", DisplayName = "Gun Store (Burned)",
                CurrentStage = 0, MaxStages = 1,
                StageNames = new[] { "Harvest Lead from Melted Weapons" },
                RewardId = "lead_scrap", RewardDescription = "200 LeadScrap for RadiationShielding upgrades (#127).",
                HeavyLootWeight = 60f
            };
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public LocationQuestSave CaptureState()
        {
            EnsureSeeded();
            var entries = new QuestEntrySave[_quests.Count];
            int i = 0;
            foreach (var kv in _quests)
            {
                entries[i++] = new QuestEntrySave
                {
                    NodeId = kv.Value.NodeId,
                    QuestType = kv.Value.QuestType,
                    CurrentStage = kv.Value.CurrentStage,
                    IsCompleted = kv.Value.IsCompleted,
                    IsFailed = kv.Value.IsFailed,
                    TemporaryRewardHoursRemaining = kv.Value.TemporaryRewardHoursRemaining
                };
            }
            return new LocationQuestSave { Quests = entries };
        }

        public void RestoreState(LocationQuestSave save)
        {
            // Reset lazy-seed flag so definitions repopulate after Clear.
            // Subsystem restore probes CaptureState() as a type template first,
            // which sets _seeded=true; without this reset, SeedQuestDefinitions
            // early-returns and leaves _quests empty for the rest of the load.
            _quests.Clear();
            _seeded = false;
            SeedQuestDefinitions();
            if (save?.Quests == null) return;
            for (int i = 0; i < save.Quests.Length; i++)
            {
                var e = save.Quests[i];
                if (e == null || string.IsNullOrEmpty(e.NodeId)) continue;
                if (_quests.TryGetValue(e.NodeId, out var q))
                {
                    q.CurrentStage = e.CurrentStage;
                    q.IsCompleted = e.IsCompleted;
                    q.IsFailed = e.IsFailed;
                    q.TemporaryRewardHoursRemaining = e.TemporaryRewardHoursRemaining;
                }
            }
        }
    }

    /// <summary>Runtime quest state for a location node.</summary>
    [Serializable]
    public class LocationQuestState
    {
        public string NodeId;
        public string QuestType;
        public string DisplayName;
        public int CurrentStage;
        public int MaxStages = 1;
        public string[] StageNames;
        public string RewardId;
        public string RewardDescription;
        public bool IsCompleted;
        public bool IsFailed;

        // Mechanics.
        public string RequiredSkill;           // "medical", "mechanical", "perception"
        public float RequiredSkillLevel = 0.5f;
        public string[] RequiredGear;          // item ids that must be equipped/in inventory
        public string RequiredItem;            // single required item
        public float DangerLevel = 2f;
        public float ContaminationLevel;
        public bool HasMinefield;
        public bool TetanusRisk;
        public bool ExtremeCold;
        public bool MentalBreakRisk;
        public bool PushYourLuck;
        public bool FatalRisk;
        public float[] PushLuckChances;        // per-stage collapse chance
        public float HeavyLootWeight;          // kg to carry
        public float ActionHours;              // hours required for the action
        public float ParseHours;               // shelter-based parsing hours
        public string DilemmaType;             // "assault_or_trade"
        public string[] DilemmaTradeItems;     // "itemId:count"
        public bool CombatRisk;
        public bool AttractsRaids;
        public bool TemporaryReward;
        public float TemporaryRewardHours;
        public float TemporaryRewardHoursRemaining;
    }

    [Serializable]
    public class LocationQuestSave
    {
        public QuestEntrySave[] Quests;
    }

    [Serializable]
    public class QuestEntrySave
    {
        public string NodeId;
        public string QuestType;
        public int CurrentStage;
        public bool IsCompleted;
        public bool IsFailed;
        public float TemporaryRewardHoursRemaining;
    }
}
