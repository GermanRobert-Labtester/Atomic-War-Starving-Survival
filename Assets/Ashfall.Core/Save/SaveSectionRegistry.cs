using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Save
{
    /// <summary>
    /// Metadata describing one aggregate save section, its persistence methods,
    /// ownership, and whether it requires a dedicated setup phase.
    /// <para>
    /// <see cref="LifecycleGroup"/> is lifecycle metadata only. It groups an
    /// existing section under a host lifecycle boundary and never creates an
    /// additional campaign section or projection file.
    /// </para>
    /// </summary>
    public record SaveSectionMetadata(
        string SectionKey,
        string SaveMethod,
        string? SetupMethod,
        string Owner,
        string Description,
        bool RequiresSetup = true,
        string? LifecycleGroup = null
    );

    /// <summary>
    /// Declarative authority for all save sections across ASHFALL.
    /// Consumed by the Godot host save orchestrator, aggregate save envelopes,
    /// unit tests, and CI triad drift validation.
    /// </summary>
    public static class SaveSectionRegistry
    {
        /// <summary>
        /// Lifecycle-only boundary for the expanded shelter batch. This value
        /// is deliberately not a member of <see cref="All"/> or
        /// <see cref="SectionFileNames"/>.
        /// </summary>
        public const string ExpandedShelterLifecycleGroup = "expanded_shelter";

        /// <summary>
        /// Historical lifecycle labels normalized to current-generation save
        /// section keys. Aliases are not registry entries and do not create
        /// additional envelope sections or files.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> LifecycleSectionAliases =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "crossing", "expansion_hub" },
                { "expeditions", "expedition" },
                { "caravans", "caravan" },
            };

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
            new("medical_pipeline", "SaveMedicalPipeline", "SetupMedical", "medical", "Diagnosis knowledge, treatment reservations, scheduled procedures (Task #133)"),
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
            new("travel_encounters", "SaveTravelEncounters", "SetupTravelEncounters", "encounters", "Travel encounters and cooldown states"),
            new("water_treatment", "SaveWaterTreatment", "SetupWaterTreatment", "infrastructure", "Water filtration and purification", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("airlock_security", "SaveAirlockSecurity", "SetupAirlockSecurity", "infrastructure", "Airlock decontamination and security", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("apprenticeship", "SaveApprenticeship", "SetupApprenticeship", "social", "Mentorship pairings and skill growth", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("caregiving", "SaveCaregiving", "SetupCaregiving", "social", "Childcare, elderly care, and comfort", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("autopsy", "SaveAutopsy", "SetupAutopsy", "medical", "Post-mortem forensic analysis", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("chemical_dependency", "SaveChemicalDependency", "SetupMentalHealthCrisis", "medical", "Substance dependencies and withdrawal", RequiresSetup: false, LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("equipment_condition", "SaveEquipmentCondition", "SetupEquipmentCondition", "equipment", "Tool and weapon wear/repair", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("survivor_relations", "SaveSurvivorRelations", "SetupSurvivorRelations", "social", "Survivor affinities, feuds, and bonds", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("regional_treaty", "SaveRegionalTreaty", "SetupRegionalTreaty", "factions", "Faction treaties and non-aggression pacts", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("vinyl_morale", "SaveVinylMorale", "SetupVinylMorale", "morale", "Gramophone records and music morale", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("wildlife_trapping", "SaveWildlifeTrapping", "SetupWildlifeTrapping", "hunting", "Snares, game catches, and foraging", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("excavation", "SaveExcavation", "SetupExcavation", "shelter", "Shelter expansion rubble clearing", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("waystation", "SaveWaystation", "SetupWaystation", "infrastructure", "Wasteland outpost network & relay hubs", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_thermal", "SaveShelterThermal", "SetupShelterThermal", "thermal", "Heating, insulation, and frost protection", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_schedule", "SaveShelterSchedule", "SetupShelterSchedule", "schedule", "Shift rotations and curfews", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("sump_flooding", "SaveSumpFlooding", "SetupSumpFlooding", "maintenance", "Bunker sump pump drainage & flood risk", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("decontamination", "SaveDecontamination", "SetupDecontamination", "radiation", "Rad-scrubbing showers and chambers", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("kitchen_nutrition", "SaveKitchenNutrition", "SetupKitchenNutrition", "nutrition", "Rationing recipes and caloric balance", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("grain_processing", "SaveGrainProcessing", "SetupGrainProcessing", "nutrition", "Grain milling, silo safety, and pest pressure", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("cryogenic_air_separation", "SaveCryogenicAirSeparation", "SetupCryogenicAirSeparation", "infrastructure", "Abstract gas production and plant condition", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("library_study", "SaveLibraryStudy", "SetupLibraryStudy", "knowledge", "Research library books and blueprints", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("research", "SaveResearch", null, "knowledge", "Research knowledge progress: unlocked, active, and completed nodes (Plan 34)", RequiresSetup: false, LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("archive_desk", "SaveArchiveDesk", "SetupArchiveDesk", "knowledge", "Document archiving, ink, and scribing", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("contractor_roster", "SaveContractorRoster", "SetupContractorRoster", "personnel", "Hired mercenaries and specialists", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("mental_health_crisis", "SaveMentalHealthCrisis", "SetupMentalHealthCrisis", "psychology", "Psychological trauma and psych ward", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_assignment", "SaveShelterAssignment", "SetupShelterAssignment", "shelter", "Room assignments and living quarters", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_decor", "SaveShelterDecor", "SetupShelterDecor", "shelter", "Room decor placements, memorial plaques, and localized morale items", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("survivor_social", "SaveSurvivorSocial", "SetupSurvivorSocial", "social", "Leadership, friction, ration conflict, trauma bonds, skill atrophy"),
            new("morale_contagion", "SaveMoraleContagion", "SetupMoraleContagion", "social", "Flagship XI Plan 154 — morale contagion channels, breakdowns, social isolation, schism ledger, HopeBeacon installation"),
            new("pathogen_strains", "SavePathogenStrains", "SetupPathogenStrains", "medical", "Flagship XI Plan 155 — fictional strain layer: cure projects and unlocked cures"),
            new("survivor_fate", "SaveSurvivorFate", "SetupSurvivorFate", "memorial", "Unified survivor-death ledger: one immutable fate record per deceased survivor"),
            new("weight_of_choices", "SaveFactionBranch", "SetupFactionBranch", "factions", "Weight of choices faction branch progression and PoNR commitments", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("onboarding", "SaveOnboarding", "SetupOnboarding", "onboarding", "First-hour onboarding journey progress, dismissed hints, assistance level, completion"),
            new("ecological_infestation", "SaveEcologicalInfestation", "SetupEcologicalInfestation", "world", "Plan 28 — location and shelter ecological infestations (trigger/clear/tolerate lifecycle)"),
            new("field_guide", "SaveFieldGuide", "SetupFieldGuide", "world", "Plan 20A/28 — field-guide unlocked-entry ledger (reading-the-land knowledge)"),
            new("shelter_workshop", "SaveWorkshop", "SetupWorkshop", "shelter", "Precision workshop tooling, ammo press, and firearm refurbishment", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("radio_station", "SaveRadioStation", "SetupRadioStation", "radio", "Radio station frequency tuning, signal lock, and triangulation"),
            new("heliograph", "SaveHeliograph", "SetupHeliograph", "radio", "Optical heliograph stations and message delivery"),
            new("shelter_social_dynamics", "SaveShelterSocial", "SetupShelterSocial", "social", "Living quarters privacy pressure, communal mess hall, and disputes", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("excavation_hazards", "SaveExcavationHazards", "SetupExcavationHazards", "shelter", "Subterranean methane, flood, spore hazards, and cave-in rescue operations", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("chem_warfare", "SaveChemWarfare", "SetupChemWarfare", "combat", "CBRN hazard warfare and toxic contamination"),
            new("comms_array", "SaveCommsArray", "SetupCommsArray", "world", "Long-range communications array and satellite telemetry"),
            new("ceremony", "SaveCeremony", "SetupCeremony", "narrative", "Communal ceremonies, festivals, truces, and morale"),
            new("robotics", "SaveRobotics", "SetupRobotics", "crafting", "Pre-war robotics, directives, and automation", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("recreation", "SaveRecreation", "SetupRecreation", "shelter", "Survivor hobbies, downtime, and recreation", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("fallout", "SaveFallout", "SetupFallout", "world", "Radioactive fallout clouds, dispersal, and shelter sealing"),
            new("desperation", "SaveDesperation", "SetupDesperation", "survival", "Starvation crisis desperation acts and cannibalism history"),
            new("mercenary_bounties", "SaveMercenary", "SetupMercenary", "economy", "Mercenary bounty contracts, target intel, and rival tracking"),
            new("archaeology", "SaveArchaeology", "SetupArchaeology", "knowledge", "Archaeology excavation ruins, archive decryption, and lore unlocks", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("amputation", "SaveAmputation", "SetupAmputation", "medical", "Infection progression, amputations, prosthetics and bionics"),
            new("railway", "SaveRailway", "SetupRailway", "expedition", "Rail network, track repair, and armored train operations"),
            new("fungi_cultivation", "SaveFungi", "SetupFungi", "farming", "Subterranean fungi beds, substrate, spores, and blooms", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("wasteland_justice", "SaveJustice", "SetupJustice", "narrative", "Crime incidents, trials, punishments, banishments, and grudges"),
            new("child_development", "SaveGenerational", "SetupGenerational", "social", "Child development phases, education, trauma, and adulthood", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("prisoner_management", "SavePrisoners", "SetupPrisoners", "factions", "Captive detention, upkeep, interrogation, escape, and recruitment"),
            new("mutation_tree", "SaveMutations", "SetupMutations", "medical", "Radiation exposure, genetic instability, and mutation trees"),
            new("expedition_stealth", "SaveStealth", "SetupStealth", "combat", "Expedition stealth, detection risk, camouflage, and night ops"),
            new("aviation", "SaveAviation", "SetupAviation", "expedition", "Aviation airframes, flight plans, aerial mapping, and crash rescue", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("forced_labor", "SaveForcedLabor", "SetupForcedLabor", "factions", "Captive forced labor assignments, cruelty index, and rebellion risks"),
            new("narcotics", "SaveNarcotics", "SetupNarcotics", "medical", "Chemical medicines, toxicity, tolerance, addiction, and rehab beds"),
            new("settlement_politics", "SavePolitics", "SetupPolitics", "narrative", "Settlement elections, political policies, approval rating, and coups"),
            // Advanced shelter / endgame sections previously allowlisted as incomplete.
            new("endgame", "SaveEndgame", "SetupEndgame", "endgame", "Campaign endgame phase, ending selection, sealed epilogue report"),
            new("caravan_trade_network", "SaveCaravanTrade", "SetupCaravanTrade", "economy", "Faction caravan trade network routes and arrivals"),
            new("surgical_ward", "SaveSurgicalWard", "SetupSurgicalWard", "medical", "Advanced surgical ward operations and sterile field"),
            new("power_subgrids", "SavePowerSubgrids", "SetupPowerSubgrids", "power_grid", "Power distribution sub-grid nodes and thermal state"),
            new("perimeter_defense", "SavePerimeterDefense", "SetupPerimeterDefense", "combat", "Surface perimeter defense emplacements"),
            new("hydroponic_biomes", "SaveHydroponicBiomes", "SetupHydroponicBiomes", "farming", "Hydroponic biome racks and crop state"),
            new("nuclear_core_lifecycle", "SaveNuclearCore", "SetupNuclearCore", "power_grid", "Nuclear core lifecycle and thermal state"),
            new("armored_crawlers", "SaveArmoredCrawlers", "SetupArmoredCrawlers", "expedition", "Armored crawler modules and forward camps"),
            new("personal_quests", "SavePersonalQuests", "SetupPersonalQuests", "quests", "Survivor personal quest progression"),
            new("chemical_synthesis", "SaveChemicalSynthesis", "SetupChemicalSynthesis", "crafting", "Chemical synthesis retorts and apparatus"),
            new("collectible_discovery", "SaveCollectibles", "SetupCollectibles", "inventory", "One-time collectible discovery ledger"),
            new("unique_claims", "SaveCollectibles", "SetupCollectibles", "inventory", "Global unique-item claim ledger"),
            new("shelter_fire", "SaveShelterFire", "SetupShelterFireHazard", "shelter", "Shelter fire incidents, smoke, and brigade response"),
            new("dynamic_quests", "SaveDynamicQuests", "SetupDynamicQuests", "quests", "Campaign-wide emergency dynamic quests"),
            new("geodetic_survey", "SaveGeodeticSurvey", "SetupGeodeticSurvey", "world", "Plans 78-81 — survey monuments, observations, resolved triangles, and network accuracy"),
            new("kinetic_storage", "SaveKineticStorage", "SetupKineticStorage", "power_grid", "Plans 78-81 — flywheel rotor, vacuum, bearing, and containment state"),
            new("chemical_recon", "SaveChemicalRecon", "SetupChemicalRecon", "expeditions", "Plans 78-81 — chemical hazard observations, samples, and safe corridors"),
            new("chlor_alkali_synthesis", "SaveChlorAlkali", "SetupChlorAlkali", "shelter", "Plans 110-113 — chlor-alkali electrolytic plant, membrane health, hazard load, and chemical production"),
            new("solar_concentrator", "SaveSolarConcentrator", "SetupSolarConcentrator", "power_grid", "Plans 110-113 — parabolic solar concentrator, mirror condition, tracking mode, and thermal output"),
            new("precision_optics", "SavePrecisionOptics", "SetupPrecisionOptics", "shelter", "Plans 110-113 — precision optical blank grinding, figure testing, and telescope/shield viewports"),
            new("ballistic_shield", "SaveBallisticShield", "SetupBallisticShield", "combat", "Plans 110-113 — defensive ballistic shields, stances, integrity, and ground anchoring"),
            new("powder_metallurgy", "SavePowderMetallurgy", "SetupPowderMetallurgy", "foundry", "Plans 130-133 — abstract advanced-material production quality and reliability", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("nvis_communications", "SaveNvisCommunications", "SetupNvisCommunications", "radio", "Plans 130-133 — regional NVIS status communications and recall queue", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("lyophilization", "SaveLyophilization", "SetupLyophilization", "medical", "Plans 130-133 — preserved-biologic batches and viability ledger", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("draisine_recovery", "SaveDraisineRerailing", "SetupDraisineRerailing", "expedition", "Plans 130-133 — armored draisine derailment recovery")
        };

        private static readonly Dictionary<string, SaveSectionMetadata> ByKeyMap =
            All.ToDictionary(s => s.SectionKey, s => s, StringComparer.Ordinal);

        private static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> SectionsByLifecycleGroup =
            All.Where(s => !string.IsNullOrWhiteSpace(s.LifecycleGroup))
                .GroupBy(s => s.LifecycleGroup!, StringComparer.Ordinal)
                .ToDictionary(
                    group => group.Key,
                    group => (IReadOnlyList<string>)group.Select(s => s.SectionKey).ToArray(),
                    StringComparer.Ordinal);

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
                { "medical_pipeline", "medical_pipeline_save.json" },
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
                { "travel_encounters", "travel_encounters_save.json" },
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
                { "grain_processing", "grain_processing_save.json" },
                { "cryogenic_air_separation", "cryogenic_air_separation_save.json" },
                { "library_study", "library_study_save.json" },
                { "research", "research_save.json" },
                { "archive_desk", "archive_desk_save.json" },
                { "contractor_roster", "contractor_roster_save.json" },
                { "mental_health_crisis", "mental_health_crisis_save.json" },
                { "shelter_assignment", "shelter_assignment_save.json" },
                { "shelter_decor", "shelter_decor_save.json" },
                { "survivor_social", "survivor_social_save.json" },
                { "morale_contagion", "morale_contagion_save.json" },
                { "pathogen_strains", "pathogen_strains_save.json" },
                { "survivor_fate", "survivor_fate_save.json" },
                { "weight_of_choices", "weight_of_choices_save.json" },
                { "onboarding", "onboarding_save.json" },
                { "ecological_infestation", "ecological_infestation_save.json" },
                { "field_guide", "field_guide_save.json" },
                { "shelter_workshop", "shelter_workshop_save.json" },
                { "radio_station", "radio_station_save.json" },
                { "heliograph", "heliograph_save.json" },
                { "shelter_social_dynamics", "shelter_social_dynamics_save.json" },
                { "excavation_hazards", "excavation_hazards_save.json" },
                { "chem_warfare", "chem_warfare_save.json" },
                { "comms_array", "comms_array_save.json" },
                { "ceremony", "ceremony_save.json" },
                { "robotics", "robotics_save.json" },
                { "recreation", "recreation_save.json" },
                { "fallout", "fallout_save.json" },
                { "desperation", "desperation_save.json" },
                { "mercenary_bounties", "mercenary_bounties_save.json" },
                { "archaeology", "archaeology_save.json" },
                { "amputation", "amputation_save.json" },
                { "railway", "railway_save.json" },
                { "fungi_cultivation", "fungi_cultivation_save.json" },
                { "wasteland_justice", "wasteland_justice_save.json" },
                { "child_development", "child_development_save.json" },
                { "prisoner_management", "prisoner_save.json" },
                { "mutation_tree", "mutation_save.json" },
                { "expedition_stealth", "stealth_save.json" },
                { "aviation", "aviation_save.json" },
                { "forced_labor", "forced_labor_save.json" },
                { "narcotics", "narcotics_save.json" },
                { "settlement_politics", "settlement_politics_save.json" },
                { "endgame", "endgame_save.json" },
                { "caravan_trade_network", "caravan_trade_network_save.json" },
                { "surgical_ward", "surgical_ward_save.json" },
                { "power_subgrids", "power_subgrids_save.json" },
                { "perimeter_defense", "perimeter_defense_save.json" },
                { "hydroponic_biomes", "hydroponic_biomes_save.json" },
                { "nuclear_core_lifecycle", "nuclear_core_lifecycle_save.json" },
                { "armored_crawlers", "armored_crawlers_save.json" },
                { "personal_quests", "personal_quests_save.json" },
                { "chemical_synthesis", "chemical_synthesis_save.json" },
                { "collectible_discovery", "collectible_discovery_save.json" },
                { "unique_claims", "unique_claims_save.json" },
                { "shelter_fire", "shelter_fire_save.json" },
                { "dynamic_quests", "dynamic_quests_save.json" },
                { "geodetic_survey", "geodetic_survey_save.json" },
                { "kinetic_storage", "kinetic_storage_save.json" },
                { "chemical_recon", "chemical_recon_save.json" },
                { "chlor_alkali_synthesis", "chlor_alkali_synthesis_save.json" },
                { "solar_concentrator", "solar_concentrator_save.json" },
                { "precision_optics", "precision_optics_save.json" },
                { "ballistic_shield", "ballistic_shield_save.json" },
                { "powder_metallurgy", "powder_metallurgy_save.json" },
                { "nvis_communications", "nvis_communications_save.json" },
                { "lyophilization", "lyophilization_save.json" },
                { "draisine_recovery", "draisine_recovery_save.json" },
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

        /// <summary>
        /// Returns the current-generation key for a lifecycle label. Unknown
        /// labels are preserved so callers can still report them as invalid;
        /// this method never adds an alias to the registry.
        /// </summary>
        public static string? CanonicalizeSectionKey(string? sectionKey)
        {
            if (sectionKey == null) return null;
            return LifecycleSectionAliases.TryGetValue(sectionKey, out var canonical)
                ? canonical
                : sectionKey;
        }

        /// <summary>Returns the existing save sections owned by a lifecycle group.</summary>
        public static IReadOnlyList<string> SectionKeysForLifecycleGroup(string lifecycleGroup)
        {
            if (string.IsNullOrWhiteSpace(lifecycleGroup))
                return Array.Empty<string>();

            return SectionsByLifecycleGroup.TryGetValue(lifecycleGroup, out var keys)
                ? keys
                : Array.Empty<string>();
        }

        /// <summary>Returns whether a lifecycle group owns one or more registered sections.</summary>
        public static bool IsLifecycleGroup(string lifecycleGroup) =>
            !string.IsNullOrWhiteSpace(lifecycleGroup) && SectionsByLifecycleGroup.ContainsKey(lifecycleGroup);

        /// <summary>All lifecycle groups derived from existing registry metadata.</summary>
        public static IReadOnlyCollection<string> LifecycleGroupKeys => SectionsByLifecycleGroup.Keys.ToArray();

        /// <summary>File name for a section key, or null for unknown keys.</summary>
        public static string? FileNameFor(string sectionKey)
        {
            string? canonical = CanonicalizeSectionKey(sectionKey);
            return canonical != null && SectionFileNames.TryGetValue(canonical, out var fileName)
                ? fileName
                : null;
        }

        /// <summary>Envelope schema version for a section key (1 when unversioned).</summary>
        public static int SchemaVersionFor(string sectionKey)
        {
            string? canonical = CanonicalizeSectionKey(sectionKey);
            return canonical != null && SchemaVersions.TryGetValue(canonical, out var version)
                ? version
                : 1;
        }

        /// <summary>
        /// Resolve a registry key from a legacy V1 section name. V1 named
        /// sections after their file (with or without the .json extension);
        /// both forms resolve. Returns false for unknown/stray names.
        /// </summary>
        public static bool TryGetKeyForSectionName(string sectionName, out string? sectionKey)
        {
            sectionKey = null;
            if (string.IsNullOrEmpty(sectionName)) return false;

            string? canonical = CanonicalizeSectionKey(sectionName);
            if (canonical != null && ByKeyMap.ContainsKey(canonical))
            {
                sectionKey = canonical;
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
            string? canonical = CanonicalizeSectionKey(sectionKey);
            if (canonical != null && ByKeyMap.TryGetValue(canonical, out var result))
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
