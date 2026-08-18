using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Research / R&D / Breakthrough engine. Coordinates a catalog of
    /// research knowledge nodes with prerequisite gating, day-progress
    /// ticks, and breakthrough-item awards. Engine-agnostic; mirrors
    /// the Phase-18 <see cref="Survivors.SkillProgressionSystem"/> shape.
    /// </summary>
    public sealed class ResearchSystem
    {
        public const string SystemId = "research_system";

        public ResearchState State { get; private set; }

        private readonly Dictionary<string, ResearchKnowledgeDef> _catalog =
            new Dictionary<string, ResearchKnowledgeDef>();
        private readonly ILog _log;

        public int CatalogCount => _catalog.Count;
        public IReadOnlyDictionary<string, ResearchKnowledgeDef> Catalog => _catalog;

        public ResearchSystem(ILog log = null, ResearchState state = null)
        {
            _log = log ?? NullLog.Instance;
            State = state ?? new ResearchState();
        }

        /// <summary>Register a knowledge node in the catalog.</summary>
        public void Register(ResearchKnowledgeDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            if (_catalog.ContainsKey(def.id))
            {
                _log.Warn("[Research] duplicate registration: " + def.id);
                return;
            }
            // Mirror state flags from save.
            if (State.unlockedIds.Contains(def.id)) def.isUnlocked = true;
            if (State.completedIds.Contains(def.id)) def.isCompleted = true;
            _catalog[def.id] = def;
        }

        /// <summary>
        /// Build the default 15-node research catalog. Called after the
        /// engine is constructed and before the first Tick.
        /// </summary>
        public void RegisterDefaults()
        {
            Register(new ResearchKnowledgeDef(
                "knowledge_water_basics", "Water Purification Basics", "survival",
                "Boiling, charcoal filtration, and still-building from salvage.",
                5));
            Register(new ResearchKnowledgeDef(
                "knowledge_water_advanced", "Advanced Water Filtration", "survival",
                "Multi-stage ceramic filters reduce fallout particulate by 90%.",
                12, prerequisites: new[] { "knowledge_water_basics" },
                breakthroughItem: "item_water_filter_advanced"));
            Register(new ResearchKnowledgeDef(
                "knowledge_radiation_basics", "Radiation Medicine Basics", "medical",
                "Iodine prophylaxis, chelation agents, and dose-ledger tracking.",
                5));
            Register(new ResearchKnowledgeDef(
                "knowledge_radiation_shielding", "Radiation Shielding Materials", "engineering",
                "Layered lead-cloth, borated polyethylene, and sky-layer armour panels.",
                15, prerequisites: new[] { "knowledge_radiation_basics" },
                breakthroughItem: "item_radiation_shielding_panel"));
            Register(new ResearchKnowledgeDef(
                "knowledge_gas_mask_improved", "Improved Gas Masks", "engineering",
                "Charcoal-canister rebuild doubles filter lifespan under heavy fallout.",
                10, prerequisites: new[] { "knowledge_radiation_basics" },
                breakthroughItem: "item_gas_mask_improved"));
            Register(new ResearchKnowledgeDef(
                "knowledge_hydroponics", "Hydroponic Cultivation", "survival",
                "Nutrient-film technique in recycled bunker trays yields greens in 14 days.",
                8));
            Register(new ResearchKnowledgeDef(
                "knowledge_solar_basics", "Solar Power Basics", "engineering",
                "Junction-box rebuild and panel-angle tracking from scrap photovoltaic cells.",
                7));
            Register(new ResearchKnowledgeDef(
                "knowledge_solar_advanced", "Solar Power Systems", "engineering",
                "Battery-bank topology and inverter rebuild for overnight draw.",
                14, prerequisites: new[] { "knowledge_solar_basics" },
                breakthroughItem: "item_solar_inverter"));
            Register(new ResearchKnowledgeDef(
                "knowledge_food_preservation", "Food Preservation", "survival",
                "Salt-curing, cold-smoking, and vacuum-seal scavenge from ruined canneries.",
                10));
            Register(new ResearchKnowledgeDef(
                "knowledge_radio_basics", "Radio Signal Processing", "science",
                "Direction-finding, squelch calibration, and Morse decoding from static.",
                6));
            Register(new ResearchKnowledgeDef(
                "knowledge_radio_advanced", "Encrypted Radio Communication", "science",
                "One-time pad key exchange and frequency-hopping from salvaged cipher rotors.",
                12, prerequisites: new[] { "knowledge_radio_basics" },
                breakthroughItem: "item_radio_cipher_rotor"));
            Register(new ResearchKnowledgeDef(
                "knowledge_shelter_insulation", "Shelter Insulation", "engineering",
                "Spray-foam salvage and thermal-barrier panels cut bunker heat loss by 40%.",
                8));
            Register(new ResearchKnowledgeDef(
                "knowledge_air_filtration", "Air Filtration Systems", "engineering",
                "HEPA-grade filter rebuild extends bunker air-filtration lifespan by 50%.",
                10, prerequisites: new[] { "knowledge_shelter_insulation" },
                breakthroughItem: "item_air_filter_hepa"));
            Register(new ResearchKnowledgeDef(
                "knowledge_scavenge_efficiency", "Scavenge Efficiency", "scavenging",
                "Route-mapping and weight-distribution analysis cuts expedition fatigue by 15%.",
                7));
            Register(new ResearchKnowledgeDef(
                "knowledge_combat_training", "Combat Training Doctrine", "combat",
                "Close-quarters drills and cover-fire protocols improve survivor combat readiness.",
                8));
        }

        /// <summary>
        /// Begin researching a knowledge node. Fails if the node is not
        /// registered, already completed, or its prerequisites are unmet.
        /// </summary>
        public bool StartResearch(string id, int day)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (!_catalog.TryGetValue(id, out var def)) return false;
            if (def.isCompleted) return false;

            // Prerequisite gate.
            if (def.prerequisites != null)
            {
                for (int i = 0; i < def.prerequisites.Length; i++)
                {
                    string prereq = def.prerequisites[i];
                    if (!_catalog.TryGetValue(prereq, out var pdef) || !pdef.isCompleted)
                    {
                        _log.Warn($"[Research] prerequisite '{prereq}' not completed for '{id}'");
                        return false;
                    }
                }
            }

            // Mark the node as unlocked the first time it is queued.
            if (!def.isUnlocked)
            {
                def.isUnlocked = true;
                State.unlockedIds.Add(id);
            }

            State.activeResearchId = id;
            State.activeResearchDays = 0;
            State.currentDay = day;
            _log.Info($"[Research] started '{id}' on day {day}");
            return true;
        }

        /// <summary>
        /// Day-step hook. Advances the active research by
        /// (<paramref name="newDay"/> - currentDay) days and completes
        /// the node if the budget is exhausted.
        /// </summary>
        public void Tick(int newDay)
        {
            if (string.IsNullOrEmpty(State.activeResearchId)) return;
            int delta = newDay - State.currentDay;
            if (delta <= 0) return;
            State.currentDay = newDay;
            State.activeResearchDays += delta;

            if (!_catalog.TryGetValue(State.activeResearchId, out var def)) return;
            if (State.activeResearchDays >= def.daysToComplete)
            {
                CompleteResearch(def.id);
            }
        }

        /// <summary>Force-complete a research node (bypasses day budget).</summary>
        public bool CompleteResearch(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (!_catalog.TryGetValue(id, out var def)) return false;
            if (def.isCompleted) return false;

            def.isCompleted = true;
            State.completedIds.Add(id);
            if (State.activeResearchId == id)
            {
                State.activeResearchId = string.Empty;
                State.activeResearchDays = 0;
            }

            _log.Info($"[Research] completed '{id}' — breakthrough: {def.breakthroughItem ?? "(none)"}");
            return true;
        }

        /// <summary>Read-only: get the current active research def, or null if idle.</summary>
        public ResearchKnowledgeDef GetActiveResearch()
        {
            if (string.IsNullOrEmpty(State.activeResearchId)) return null;
            _catalog.TryGetValue(State.activeResearchId, out var def);
            return def;
        }

        /// <summary>Read-only: get any registered knowledge node.</summary>
        public ResearchKnowledgeDef GetKnowledge(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _catalog.TryGetValue(id, out var def);
            return def;
        }

        public ResearchState CaptureState()
        {
            // Mirror flags back into state lists for the save envelope.
            State.unlockedIds.Clear();
            State.completedIds.Clear();
            foreach (var kv in _catalog)
            {
                if (kv.Value.isUnlocked) State.unlockedIds.Add(kv.Key);
                if (kv.Value.isCompleted) State.completedIds.Add(kv.Key);
            }
            return State;
        }

        public void RestoreState(ResearchState saved)
        {
            if (saved == null) return;
            State = saved;
            // Push saved flags back into the catalog.
            foreach (var kv in _catalog)
            {
                kv.Value.isUnlocked = State.unlockedIds.Contains(kv.Key);
                kv.Value.isCompleted = State.completedIds.Contains(kv.Key);
            }
        }
    }
}
