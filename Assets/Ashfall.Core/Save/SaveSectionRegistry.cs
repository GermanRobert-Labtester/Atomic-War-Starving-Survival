using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Save
{
    /// <summary>
    /// Metadata describing one aggregate save section, its persistence methods,
    /// ownership, and whether it requires a dedicated setup phase.
    /// </summary>
    public record SaveSectionMetadata(
        string SectionKey,
        string SaveMethod,
        string? SetupMethod,
        string Owner,
        string Description,
        bool RequiresSetup = true
    );

    /// <summary>
    /// Declarative authority for all save sections across ASHFALL.
    /// Consumed by the Godot host save orchestrator, aggregate save envelopes,
    /// unit tests, and CI triad drift validation.
    /// </summary>
    public static class SaveSectionRegistry
    {
        public static readonly IReadOnlyList<SaveSectionMetadata> All = new List<SaveSectionMetadata>
        {
            new("journal", "SaveJournal", "SetupJournal", "journal", "Player journal, logs, and codex entries"),
            new("holdfast", "SaveHoldfast", "SetupHoldfastRuntime", "holdfast", "Holdfast S1 bunker state"),
            new("holdfast_trade", "SaveHoldfastRuntime", "SetupHoldfastRuntime", "holdfast", "Holdfast trade session state"),
            new("duty_roster", "SaveDutyRoster", "SetupDutyRoster", "duty_roster", "Duty roster shifts and assignments"),
            new("expansion_hub", "SaveExpansionHub", "SetupExpansions", "expansion_hub", "Expansion hub discovery state"),
            new("expansion_quest", "SaveExpansionQuests", "SetupExpansionQuests", "expansion_quest", "Expansion questline progression"),
            new("thirdonary", "SaveThirdonary", "SetupThirdonary", "thirdonary", "Thirdonary covenant & dispute states"),
            new("phantom_memory", "SavePhantomMemory", "SetupPhantom", "phase0", "Phantom memory lineages and echoes"),
            new("dose_ledger", "SaveDoseLedger", "SetupDoseLedger", "dose_ledger", "Survivor radiation dose ledger & cohorts"),
            new("muster", "SaveMuster", "SetupMuster", "muster", "The Muster military rally & conflict state"),
            new("inventory", "SaveInventory", "SetupInventory", "inventory", "Shelter warehouse & items storage"),
            new("survivors", "SaveSurvivors", "SetupSurvivors", "survivors", "Living survivors, needs, and traits"),
            new("economy", "SaveEconomy", "SetupEconomy", "economy", "Dynamic economy rates and market orders"),
            new("verdict", "SaveVerdict", "SetupVerdict", "verdict", "The Verdict investigation and tribunal state"),
            new("maritime", "SaveMaritime", "SetupMaritime", "maritime", "The Black Flotilla dives and naval wrecks"),
            new("expedition", "SaveExpeditions", "SetupExpeditions", "expeditions", "Wasteland expedition runs & status"),
            new("combat", "SaveCombat", "SetupCombat", "combat", "Combat encounters and tactical trauma"),
            new("narrative", "SaveNarrative", "SetupNarrative", "narrative", "Branching story arcs and narrative flags"),
            new("medical", "SaveMedical", "SetupMedical", "medical", "Triage, illnesses, and treatments"),
            new("world", "SaveWorld", "SetupWorld", "world", "World map nodes, sectors, and discovery"),
            new("crafting", "SaveCrafting", "SetupCrafting", "crafting", "Known recipes and workbench queues"),
            new("caravan", "SaveCaravans", "SetupCaravans", "caravans", "Trade caravans, routes, and arrivals"),
            new("campaign_day", "SaveCampaignDay", "SetupCampaignDay", "campaign", "Master campaign day counter & ticks"),
            new("year_of_ash", "SaveYearOfAsh", "SetupYearOfAsh", "year_of_ash", "The Year of Ash harsh winter state"),
            new("phase0", "SavePhase0", "SetupPhase0", "phase0", "Pre-war timeline and bunker startup"),
            new("starting_level", "SaveStartingLevel", "SetupStartingLevel", "starting_level", "Bunker initial configuration & tier"),
            new("greenhouse", "SaveGreenhouse", "SetupGreenhouse", "greenhouse", "Hydroponic crops and food production"),
            new("host_event", "SaveEventAdapter", "SetupEventAdapter", "events", "Host event ledger & moral decisions"),
            new("moral_choice", "SaveMoralChoice", "SetupMoralChoice", "events", "Moral choice ledger and community trust", RequiresSetup: false),
            new("radio", "SaveRadio", "SetupRadio", "radio", "Radio frequencies, logs, and distress signals"),
            new("daily_briefing", "SaveDailyBriefing", "SetupDailyBriefingModal", "campaign", "Daily dawn briefing notes & status"),
            new("power_grid", "SavePowerGrid", "SetupPowerGrid", "power_grid", "Shelter generator & power allocations"),
            new("medical_ward", "SaveMedicalWard", "SetupMedicalWard", "medical", "Hospital ward beds and inpatients"),
            new("memorial", "SaveMemorial", "SetupMemorial", "memorial", "Fallen survivors memorial wall"),
            new("silent_foundry", "SaveSilentFoundry", "SetupSilentFoundry", "foundry", "Automated foundry machinery & smelters"),
            new("disease", "SaveDisease", "SetupDisease", "medical", "Epidemics, contagions, and pathogen spread"),
            new("wasteland_map", "SaveWastelandMap", null, "world", "Wasteland map markers and fog-of-war", RequiresSetup: false),
            new("encounter_choice", "SaveEncounterChoice", "SetupEncounterChoice", "encounters", "Encounter choice history & outcomes"),
            new("water_treatment", "SaveWaterTreatment", "SetupWaterTreatment", "infrastructure", "Water filtration and purification"),
            new("airlock_security", "SaveAirlockSecurity", "SetupAirlockSecurity", "infrastructure", "Airlock decontamination and security"),
            new("apprenticeship", "SaveApprenticeship", "SetupApprenticeship", "social", "Mentorship pairings and skill growth"),
            new("caregiving", "SaveCaregiving", "SetupCaregiving", "social", "Childcare, elderly care, and comfort"),
            new("autopsy", "SaveAutopsy", "SetupAutopsy", "medical", "Post-mortem forensic analysis"),
            new("chemical_dependency", "SaveChemicalDependency", "SetupMentalHealthCrisis", "medical", "Substance dependencies and withdrawal", RequiresSetup: false),
            new("equipment_condition", "SaveEquipmentCondition", "SetupEquipmentCondition", "equipment", "Tool and weapon wear/repair"),
            new("survivor_relations", "SaveSurvivorRelations", "SetupSurvivorRelations", "social", "Survivor affinities, feuds, and bonds"),
            new("regional_treaty", "SaveRegionalTreaty", "SetupRegionalTreaty", "factions", "Faction treaties and non-aggression pacts"),
            new("vinyl_morale", "SaveVinylMorale", "SetupVinylMorale", "morale", "Gramophone records and music morale"),
            new("wildlife_trapping", "SaveWildlifeTrapping", "SetupWildlifeTrapping", "hunting", "Snares, game catches, and foraging"),
            new("excavation", "SaveExcavation", "SetupExcavation", "shelter", "Shelter expansion rubble clearing"),
            new("waystation", "SaveWaystation", "SetupWaystation", "infrastructure", "Wasteland outpost network & relay hubs"),
            new("shelter_thermal", "SaveShelterThermal", "SetupShelterThermal", "thermal", "Heating, insulation, and frost protection"),
            new("shelter_schedule", "SaveShelterSchedule", "SetupShelterSchedule", "schedule", "Shift rotations and curfews"),
            new("sump_flooding", "SaveSumpFlooding", "SetupSumpFlooding", "maintenance", "Bunker sump pump drainage & flood risk"),
            new("decontamination", "SaveDecontamination", "SetupDecontamination", "radiation", "Rad-scrubbing showers and chambers"),
            new("kitchen_nutrition", "SaveKitchenNutrition", "SetupKitchenNutrition", "nutrition", "Rationing recipes and caloric balance"),
            new("library_study", "SaveLibraryStudy", "SetupLibraryStudy", "knowledge", "Research library books and blueprints"),
            new("archive_desk", "SaveArchiveDesk", "SetupArchiveDesk", "knowledge", "Document archiving, ink, and scribing"),
            new("contractor_roster", "SaveContractorRoster", "SetupContractorRoster", "personnel", "Hired mercenaries and specialists"),
            new("mental_health_crisis", "SaveMentalHealthCrisis", "SetupMentalHealthCrisis", "psychology", "Psychological trauma and psych ward"),
            new("shelter_assignment", "SaveShelterAssignment", "SetupShelterAssignment", "shelter", "Room assignments and living quarters"),
            new("survivor_social", "SaveSurvivorSocial", "SetupSurvivorSocial", "social", "Leadership, friction, ration conflict, trauma bonds, skill atrophy"),
            new("survivor_fate", "SaveSurvivorFate", "SetupSurvivorFate", "memorial", "Unified survivor-death ledger: one immutable fate record per deceased survivor"),
            new("weight_of_choices", "SaveFactionBranch", "SetupFactionBranch", "factions", "Weight of choices faction branch progression and PoNR commitments"),
            new("onboarding", "SaveOnboarding", "SetupOnboarding", "onboarding", "First-hour onboarding journey progress, dismissed hints, assistance level, completion")
        };

        private static readonly Dictionary<string, SaveSectionMetadata> ByKeyMap =
            All.ToDictionary(s => s.SectionKey, s => s, StringComparer.Ordinal);

        /// <summary>
        /// The on-disk section file name for every registry key — the single
        /// authority for the envelope whitelist, V1 filename→key migration,
        /// and registry-derived cleanup lists. Weather is deliberately absent:
        /// the world section is the canonical weather persistence.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> SectionFileNames =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "journal", "journal_save.json" },
                { "holdfast", "holdfast_s1_save.json" },
                { "holdfast_trade", "holdfast_trade_save.json" },
                { "duty_roster", "duty_roster_save.json" },
                { "expansion_hub", "expansion_hub_save.json" },
                { "expansion_quest", "expansion_quest_save.json" },
                { "thirdonary", "thirdonary_quest_save.json" },
                { "phantom_memory", "phantom_memory_save.json" },
                { "dose_ledger", "dose_ledger_save.json" },
                { "muster", "muster_save.json" },
                { "inventory", "inventory_save.json" },
                { "survivors", "survivors_save.json" },
                { "economy", "economy_save.json" },
                { "verdict", "verdict_save.json" },
                { "maritime", "maritime_save.json" },
                { "expedition", "expedition_save.json" },
                { "combat", "combat_save.json" },
                { "narrative", "narrative_save.json" },
                { "medical", "medical_save.json" },
                { "world", "world_save.json" },
                { "crafting", "crafting_save.json" },
                { "caravan", "caravan_save.json" },
                { "campaign_day", "campaign_day_save.json" },
                { "year_of_ash", "year_of_ash_save.json" },
                { "phase0", "phase0_save.json" },
                { "starting_level", "starting_level_save.json" },
                { "greenhouse", "greenhouse_save.json" },
                { "host_event", "host_event_save.json" },
                { "moral_choice", "moral_choice_save.json" },
                { "radio", "radio_save.json" },
                { "daily_briefing", "daily_briefing_save.json" },
                { "power_grid", "power_grid_save.json" },
                { "medical_ward", "medical_ward_save.json" },
                { "memorial", "memorial_save.json" },
                { "silent_foundry", "silent_foundry_save.json" },
                { "disease", "disease_save.json" },
                { "wasteland_map", "wasteland_map_save.json" },
                { "encounter_choice", "encounter_choice_save.json" },
                { "water_treatment", "water_treatment_save.json" },
                { "airlock_security", "airlock_security_save.json" },
                { "apprenticeship", "apprenticeship_save.json" },
                { "caregiving", "caregiving_save.json" },
                { "autopsy", "autopsy_save.json" },
                { "chemical_dependency", "chemical_dependency_save.json" },
                { "equipment_condition", "equipment_condition_save.json" },
                { "survivor_relations", "survivor_relations_save.json" },
                { "regional_treaty", "regional_treaty_save.json" },
                { "vinyl_morale", "vinyl_morale_save.json" },
                { "wildlife_trapping", "wildlife_trapping_save.json" },
                { "excavation", "excavation_save.json" },
                { "waystation", "waystation_save.json" },
                { "shelter_thermal", "shelter_thermal_save.json" },
                { "shelter_schedule", "shelter_schedule_save.json" },
                { "sump_flooding", "sump_flooding_save.json" },
                { "decontamination", "decontamination_save.json" },
                { "kitchen_nutrition", "kitchen_nutrition_save.json" },
                { "library_study", "library_study_save.json" },
                { "archive_desk", "archive_desk_save.json" },
                { "contractor_roster", "contractor_roster_save.json" },
                { "mental_health_crisis", "mental_health_crisis_save.json" },
                { "shelter_assignment", "shelter_assignment_save.json" },
                { "survivor_social", "survivor_social_save.json" },
                { "survivor_fate", "survivor_fate_save.json" },
                { "weight_of_choices", "weight_of_choices_save.json" },
                { "onboarding", "onboarding_save.json" },
            };

        /// <summary>
        /// Envelope section schema versions for sections whose payload embeds
        /// a Core save codec with its own saveVersion ladder. Sections without
        /// an entry carry unversioned <c>{ State, Checksum }</c> payloads (1).
        /// </summary>
        public static readonly IReadOnlyDictionary<string, int> SchemaVersions =
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                { "holdfast", 5 },
                { "year_of_ash", 4 },
                { "dose_ledger", 2 },
                { "expansion_hub", 4 },
                { "weight_of_choices", 2 },
            };

        /// <summary>File name for a section key, or null for unknown keys.</summary>
        public static string? FileNameFor(string sectionKey) =>
            SectionFileNames.TryGetValue(sectionKey, out var fileName) ? fileName : null;

        /// <summary>Envelope schema version for a section key (1 when unversioned).</summary>
        public static int SchemaVersionFor(string sectionKey) =>
            SchemaVersions.TryGetValue(sectionKey, out var version) ? version : 1;

        /// <summary>
        /// Resolve a registry key from a legacy V1 section name. V1 named
        /// sections after their file (with or without the .json extension);
        /// both forms resolve. Returns false for unknown/stray names.
        /// </summary>
        public static bool TryGetKeyForSectionName(string sectionName, out string? sectionKey)
        {
            sectionKey = null;
            if (string.IsNullOrEmpty(sectionName)) return false;

            if (ByKeyMap.ContainsKey(sectionName))
            {
                sectionKey = sectionName;
                return true;
            }

            string fileName = sectionName.EndsWith(".json", StringComparison.Ordinal)
                ? sectionName
                : sectionName + ".json";
            foreach (var pair in SectionFileNames)
            {
                if (string.Equals(pair.Value, fileName, StringComparison.Ordinal))
                {
                    sectionKey = pair.Key;
                    return true;
                }
            }
            return false;
        }

        public static bool TryGetSection(string sectionKey, out SaveSectionMetadata? metadata)
        {
            if (ByKeyMap.TryGetValue(sectionKey, out var result))
            {
                metadata = result;
                return true;
            }
            metadata = null;
            return false;
        }

        public static IReadOnlyList<string> SectionKeys => All.Select(s => s.SectionKey).ToList();
    }
}
