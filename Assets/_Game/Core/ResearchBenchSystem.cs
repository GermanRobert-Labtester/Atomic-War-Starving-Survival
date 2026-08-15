using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion V — The Research Bench. You do not unlock blueprints by leveling up.
    /// You find schematics in the dark, and you must pay in blood and time to understand them.
    /// Requires ShelterModule_ResearchTable. Assign a survivor with high Intellect skill.
    /// Research consumes clean_water (focus), fuel (desk lamp), and Days.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ResearchBenchSystem
    {
        // ── Module id ─────────────────────────────────────────────────
        public const string ResearchTableModuleId = "research_table";

        // ── Schematic ids ─────────────────────────────────────────────
        public const string Sch_AquiferTap = "sch_aquifer_tap";
        public const string Sch_AmmoReloader = "sch_ammo_reloader";
        public const string Sch_UVCycling = "sch_uv_cycling";
        public const string ChemSynthesis = "sch_chem_synthesis";
        public const string Sch_VehicleArmor = "sch_vehicle_armor";

        // ── Base research costs ───────────────────────────────────────
        public const float WaterPerResearchDay = 2f;   // clean_water units per day
        public const float FuelPerResearchDay = 1f;    // fuel_1l units per day

        // ── Skill requirements ────────────────────────────────────────
        public const float MinIntellectSkill = 0.5f;   // Minimum skill to begin research

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnSchematicDiscovered;
        public event Action<string> OnResearchStarted;
        public event Action<string> OnResearchCompleted;
        public event Action<string, float> OnResearchProgress;  // schematicId, progress

        private readonly Dictionary<string, SchematicState> _schematics = new Dictionary<string, SchematicState>();
        private string _activeResearchId;
        private string _assignedResearcherId;

        public string ActiveResearchId => _activeResearchId;
        public string AssignedResearcherId => _assignedResearcherId;
        public bool IsResearching => !string.IsNullOrEmpty(_activeResearchId);
        public IReadOnlyDictionary<string, SchematicState> AllSchematics => _schematics;

        // ── Schematic discovery ───────────────────────────────────────

        /// <summary>
        /// Discover a schematic (found in the world as Item_Schematic_Rolled).
        /// </summary>
        public bool DiscoverSchematic(string schematicId, string displayName,
            int researchDays, string unlockEffect)
        {
            if (string.IsNullOrEmpty(schematicId)) return false;
            if (_schematics.ContainsKey(schematicId)) return false;

            _schematics[schematicId] = new SchematicState
            {
                Id = schematicId,
                DisplayName = displayName,
                ResearchDaysRequired = researchDays,
                UnlockEffect = unlockEffect,
                State = SchematicProgressState.Discovered
            };

            OnSchematicDiscovered?.Invoke(schematicId);
            return true;
        }

        /// <summary>Check if a schematic has been discovered.</summary>
        public bool HasSchematic(string schematicId)
        {
            return _schematics.ContainsKey(schematicId);
        }

        /// <summary>Check if a schematic has been fully researched.</summary>
        public bool IsResearched(string schematicId)
        {
            return _schematics.TryGetValue(schematicId, out var s)
                && s.State == SchematicProgressState.Researched;
        }

        // ── Research assignment ───────────────────────────────────────

        /// <summary>
        /// Assign a researcher and begin research on a schematic.
        /// Requires operational research table module.
        /// </summary>
        public bool BeginResearch(string schematicId, string researcherId,
            float researcherIntellectSkill, bool hasResearchTable)
        {
            if (!hasResearchTable) return false;
            if (researcherIntellectSkill < MinIntellectSkill) return false;
            if (!_schematics.TryGetValue(schematicId, out var schematic)) return false;
            if (schematic.State != SchematicProgressState.Discovered) return false;
            if (IsResearching) return false;

            _activeResearchId = schematicId;
            _assignedResearcherId = researcherId;
            schematic.State = SchematicProgressState.InProgress;
            schematic.ResearchDaysElapsed = 0;

            OnResearchStarted?.Invoke(schematicId);
            return true;
        }

        /// <summary>
        /// Cancel current research. Progress is preserved.
        /// </summary>
        public void CancelResearch()
        {
            if (!IsResearching) return;
            if (_schematics.TryGetValue(_activeResearchId, out var s))
                s.State = SchematicProgressState.Discovered;
            _activeResearchId = null;
            _assignedResearcherId = null;
        }

        // ── Research tick ─────────────────────────────────────────────

        /// <summary>
        /// Advance research by one day. Consumes water and fuel.
        /// Returns true if research completes this tick.
        /// </summary>
        public bool TickResearchDay(Func<float, bool> consumeWater,
            Func<float, bool> consumeFuel)
        {
            if (!IsResearching) return false;
            if (!_schematics.TryGetValue(_activeResearchId, out var schematic)) return false;

            // Consume resources
            if (consumeWater != null && !consumeWater(WaterPerResearchDay))
                return false; // Not enough water — research stalls
            if (consumeFuel != null && !consumeFuel(FuelPerResearchDay))
                return false; // Not enough fuel — research stalls

            schematic.ResearchDaysElapsed++;
            float progress = (float)schematic.ResearchDaysElapsed / schematic.ResearchDaysRequired;
            OnResearchProgress?.Invoke(_activeResearchId, progress);

            if (schematic.ResearchDaysElapsed >= schematic.ResearchDaysRequired)
            {
                schematic.State = SchematicProgressState.Researched;
                string completedId = _activeResearchId;
                _activeResearchId = null;
                _assignedResearcherId = null;
                OnResearchCompleted?.Invoke(completedId);
                return true;
            }
            return false;
        }

        /// <summary>Get research progress (0..1) for a schematic.</summary>
        public float GetProgress(string schematicId)
        {
            if (!_schematics.TryGetValue(schematicId, out var s)) return 0f;
            if (s.State == SchematicProgressState.Researched) return 1f;
            if (s.ResearchDaysRequired <= 0) return 0f;
            return Mathf.Clamp01((float)s.ResearchDaysElapsed / s.ResearchDaysRequired);
        }

        // ── Factory: create default schematics ────────────────────────

        /// <summary>Create the discoverable schematics from Expansion V.</summary>
        public static List<SchematicDefinition> CreateDefaultSchematics()
        {
            return new List<SchematicDefinition>
            {
                new SchematicDefinition
                {
                    Id = Sch_AquiferTap,
                    DisplayName = "Deep Aquifer Tap",
                    FoundIn = "location_submerged_mall",
                    ResearchDays = 4,
                    UnlockEffect = "Unlocks Project_DeepWell. Infinite water, risks SinkholeCollapse."
                },
                new SchematicDefinition
                {
                    Id = Sch_AmmoReloader,
                    DisplayName = "Precision Reloading Press",
                    FoundIn = "location_blacksite_echo",
                    ResearchDays = 3,
                    UnlockEffect = "Craft ammo_*_jhp_ap from casings. +40% hatch defense damage."
                },
                new SchematicDefinition
                {
                    Id = Sch_UVCycling,
                    DisplayName = "Hydroponic UV Cycling",
                    FoundIn = "location_railyard_graveyard",
                    ResearchDays = 5,
                    UnlockEffect = "+50% crop yield, +200% shelter power draw."
                },
                new SchematicDefinition
                {
                    Id = ChemSynthesis,
                    DisplayName = "Prussian Blue Synthesis",
                    FoundIn = "location_cathedral_st_jude",
                    ResearchDays = 6,
                    UnlockEffect = "Craft Item_PrussianBlue (rad purge) from fertilizer + scrap_metal."
                },
                new SchematicDefinition
                {
                    Id = Sch_VehicleArmor,
                    DisplayName = "Slag Armor Plating",
                    FoundIn = "location_highway_pileup",
                    ResearchDays = 4,
                    UnlockEffect = "Weld scrap_metal to ArmoredTruck. Less damage, more fuel."
                }
            };
        }

        // ── Save / Load ───────────────────────────────────────────────

        public ResearchSave CaptureState()
        {
            var entries = new SchematicSave[_schematics.Count];
            int i = 0;
            foreach (var kv in _schematics)
            {
                var s = kv.Value;
                entries[i++] = new SchematicSave
                {
                    Id = s.Id,
                    DisplayName = s.DisplayName,
                    ResearchDaysRequired = s.ResearchDaysRequired,
                    ResearchDaysElapsed = s.ResearchDaysElapsed,
                    UnlockEffect = s.UnlockEffect,
                    State = s.State
                };
            }
            return new ResearchSave
            {
                ActiveResearchId = _activeResearchId,
                AssignedResearcherId = _assignedResearcherId,
                Schematics = entries
            };
        }

        public void RestoreState(ResearchSave save)
        {
            _schematics.Clear();
            _activeResearchId = null;
            _assignedResearcherId = null;
            if (save == null) return;
            _activeResearchId = save.ActiveResearchId;
            _assignedResearcherId = save.AssignedResearcherId;
            if (save.Schematics != null)
                for (int i = 0; i < save.Schematics.Length; i++)
                {
                    var e = save.Schematics[i];
                    if (e == null || string.IsNullOrEmpty(e.Id)) continue;
                    _schematics[e.Id] = new SchematicState
                    {
                        Id = e.Id,
                        DisplayName = e.DisplayName,
                        ResearchDaysRequired = e.ResearchDaysRequired,
                        ResearchDaysElapsed = e.ResearchDaysElapsed,
                        UnlockEffect = e.UnlockEffect,
                        State = e.State
                    };
                }
        }
    }

    public enum SchematicProgressState
    {
        Discovered,
        InProgress,
        Researched
    }

    public class SchematicState
    {
        public string Id;
        public string DisplayName;
        public int ResearchDaysRequired;
        public int ResearchDaysElapsed;
        public string UnlockEffect;
        public SchematicProgressState State;
    }

    public class SchematicDefinition
    {
        public string Id;
        public string DisplayName;
        public string FoundIn;
        public int ResearchDays;
        public string UnlockEffect;
    }

    [Serializable]
    public class ResearchSave
    {
        public string ActiveResearchId;
        public string AssignedResearcherId;
        public SchematicSave[] Schematics;
    }

    [Serializable]
    public class SchematicSave
    {
        public string Id;
        public string DisplayName;
        public int ResearchDaysRequired;
        public int ResearchDaysElapsed;
        public string UnlockEffect;
        public SchematicProgressState State;
    }
}
