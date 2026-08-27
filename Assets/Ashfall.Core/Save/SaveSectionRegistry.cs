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
            new("shelter_assignment", "SaveShelterAssignment", "SetupShelterAssignment", "shelter", "Room assignments and living quarters")
        };

        private static readonly Dictionary<string, SaveSectionMetadata> ByKeyMap =
            All.ToDictionary(s => s.SectionKey, s => s, StringComparer.Ordinal);

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
