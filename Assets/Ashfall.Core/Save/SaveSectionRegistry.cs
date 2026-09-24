// SPDX-License-Identifier: MIT
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
            new("sanitation", "SaveSanitation", "SetupSanitation", "shelter", "Plan 210 — room waste, hygiene, compost queue, and spills"),
            new("deep_well", "SaveDeepWell", "SetupDeepWell", "infrastructure", "B5–B8 Phase 6 — built deep-well pump: build state, condition, yield ledger (raw water into treatment via the Plan 189 intake seam)"),
            new("water_condenser", "SaveWaterCondenser", "SetupWaterCondenser", "infrastructure", "B5–B8 expansion — Peltier condensation array: build state, membrane integrity, weather-indexed yield ledger"),
            new("black_market", "SaveBlackMarket", "SetupBlackMarket", "economy", "Plan 211 — underworld contacts, stock snapshots, debts, heat, and trust"),
            new("verdict", "SaveVerdict", "SetupVerdict", "verdict", "The Verdict investigation and tribunal state"),
            new("maritime", "SaveMaritime", "SetupMaritime", "maritime", "The Black Flotilla dives and naval wrecks"),
            new("expedition", "SaveExpeditions", "SetupExpeditions", "expeditions", "Wasteland expedition runs & status"),
            new("combat", "SaveCombat", "SetupCombat", "combat", "Combat encounters and tactical trauma"),
            new("narrative", "SaveNarrative", "SetupNarrative", "narrative", "Branching story arcs and narrative flags"),
            new("echoes", "SaveEchoes", "SetupEchoes", "narrative", "Field echoes: surfaced and resolved one-time narrative state"),
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
            new("weather_hardening", "SaveWeatherHardening", "SetupWeatherHardening", "infrastructure", "Cryo-ash weather hardening & thermal insulation", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("geothermal_aquifer", "SaveGeothermalAquifer", "SetupGeothermalAquifer", "infrastructure", "Deep geothermal boreholes & aquifer pumping", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("counter_intelligence", "SaveCounterIntelligence", "SetupCounterIntelligence", "factions", "Counter-intelligence, vetting, and defector management", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("recon_telemetry", "SaveReconTelemetry", "SetupReconTelemetry", "expeditions", "Long-range recon drones & high-altitude mapping", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("sump_flooding", "SaveSumpFlooding", "SetupSumpFlooding", "maintenance", "Bunker sump pump drainage & flood risk", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("decontamination", "SaveDecontamination", "SetupDecontamination", "radiation", "Rad-scrubbing showers and chambers", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("kitchen_nutrition", "SaveKitchenNutrition", "SetupKitchenNutrition", "nutrition", "Rationing recipes and caloric balance", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("grain_processing", "SaveGrainProcessing", "SetupGrainProcessing", "nutrition", "Grain milling, silo safety, and pest pressure", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("cryogenic_air_separation", "SaveCryogenicAirSeparation", "SetupCryogenicAirSeparation", "infrastructure", "Abstract gas production and plant condition", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("library_study", "SaveLibraryStudy", "SetupLibraryStudy", "knowledge", "Research library books and blueprints", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("research", "SaveResearch", null, "knowledge", "Research knowledge progress: unlocked, active, and completed nodes (Plan 34)", RequiresSetup: false, LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("espionage", "SaveEspionage", "SetupPlans166To169", "factions", "Campaign intelligence networks, missions, and captured agents"),
            new("fluid_logistics", "SaveFluidLogistics", "SetupPlans166To169", "infrastructure", "Shelter fluid topology, pressure, leaks, and distributed quality"),
            new("procedural_narrative", "SaveProceduralNarrative", "SetupPlans166To169", "quests", "Procedural narrative metadata and the shared quest runtime"),
            new("archive_desk", "SaveArchiveDesk", "SetupArchiveDesk", "knowledge", "Document archiving, ink, and scribing", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("contractor_roster", "SaveContractorRoster", "SetupContractorRoster", "personnel", "Hired mercenaries and specialists", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("mental_health_crisis", "SaveMentalHealthCrisis", "SetupMentalHealthCrisis", "psychology", "Psychological trauma and psych ward", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_assignment", "SaveShelterAssignment", "SetupShelterAssignment", "shelter", "Room assignments and living quarters", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_decor", "SaveShelterDecor", "SetupShelterDecor", "shelter", "Room decor placements, memorial plaques, and localized morale items", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_atmosphere", "SaveShelterAtmosphere", "SetupShelterAtmosphere", "shelter", "Plan 220 — shelter composite atmosphere, ambiance profile, and environmental facets", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_noise", "SaveShelterAtmosphere", "SetupShelterAtmosphere", "shelter", "Plan 205 — shelter acoustic noise, room soundproofing, and quiet hours", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("hidden_agenda", "SaveHiddenAgenda", "SetupHiddenAgenda", "survivors", "Plan 132 — survivor hidden agendas, secret motivations, clue discovery, and confrontation arcs", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_reputation", "SaveShelterReputation", "SetupShelterReputation", "shelter", "Plan 207 — shelter reputation, notoriety, public tags, and external perception", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("propaganda_campaigns", "SavePropaganda", "SetupPropaganda", "shelter", "Plan 168 — propaganda messages, multi-day campaigns, detection, and morale warfare", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("wasteland_rumors", "SaveRumorNetwork", "SetupRumorNetwork", "world", "Plan 203 / 131 — wasteland rumors, information hubs, propagation, and intercepts", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_security", "SaveShelterSecurity", "SetupShelterSecurity", "shelter", "Plan 138 — shelter security zones, clearances, locks, lockdowns, and breaches", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("visitor_integration", "SaveVisitorIntegration", "SetupVisitorIntegration", "visitors", "Plan 214 — admitted visitor stays, temporary housing, processing requirements, and departures", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("time_capsules", "SaveTimeCapsules", "SetupTimeCapsules", "communication", "Plan 212 — time capsules, legacy messages, delayed discovery, and cross-generational communication", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("death_legacy", "SaveDeathLegacy", "SetupDeathLegacy", "survivors", "Plan 206 — survivor death records, last wills, estate inheritance, and disputes", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("relationship_decay", "SaveRelationshipDecay", "SetupRelationshipDecay", "social", "Plan 182 — survivor pair bond decay, interaction tracking, and social drift", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("survivor_social", "SaveSurvivorSocial", "SetupSurvivorSocial", "social", "Leadership, friction, ration conflict, trauma bonds, skill atrophy"),
            new("morale_contagion", "SaveMoraleContagion", "SetupMoraleContagion", "social", "Flagship XI Plan 154 — morale contagion channels, breakdowns, social isolation, schism ledger, HopeBeacon installation"),
            new("pathogen_strains", "SavePathogenStrains", "SetupPathogenStrains", "medical", "Flagship XI Plan 155 — fictional strain layer: cure projects and unlocked cures"),
            new("subterranean", "SaveSubterranean", "SetupSubterranean", "world", "Flagship XI Plan 156 — generated underground topology, oxygen/collapse/flood/shoring state, discovery"),
            new("psyops", "SavePsyOps", "SetupPsyOps", "radio", "Flagship XI Plan 157 — broadcast campaigns, jamming, counter-propaganda, ideological pressure"),
            new("radio_program_production", "SaveRadioProgramProduction", "SetupRadioProgramProduction", "radio", "Plan 173 — player radio program prep/delivery jobs and follow-ups"),
            new("low_background_metrology", "SaveLowBackgroundMetrology", "SetupLowBackgroundMetrology", "radiation", "Plan 138 — low-background shield install, detector calibration, smelting batches, bounded assay history"),
            new("insar_deformation", "SaveInSarMapping", "SetupInSarMapping", "world", "Plan 139 — repeat-pass InSAR survey passes, coherence, deformation summaries, excavation/travel intelligence"),
            new("hydraulic_extrusion", "SaveHydraulicExtrusion", "SetupHydraulicExtrusion", "foundry", "Plan 140 — advanced hydraulic extrusion batches, tooling condition, quality grades"),
            new("runflat_tire", "SaveRunFlatTire", "SetupRunFlatTire", "expeditions", "Plan 141 — run-flat wheel profiles, integrity, heat, rim/bead, rolling-resistance cost"),
            new("sofc_power", "SaveSofcPower", "SetupSofcPower", "shelter", "Plan 122 — SOFC plant operating mode, thermal level, stack health, seal integrity, degradation, faults"),
            new("cvd_diamond", "SaveCvdDiamond", "SetupCvdDiamond", "shelter", "Plan 124 — CVD diamond reactor condition, plasma stability, growth batches, faults"),
            new("sound_ranging", "SaveSoundRanging", "SetupSoundRanging", "combat", "Plan 123 — defensive sound-ranging calibration, node status, observation history, threat estimate"),
            new("amphibious_draisine", "SaveAmphibiousDraisine", "SetupAmphibiousDraisine", "expeditions", "Plan 125 — per-vehicle amphibious kit condition, pontoons, ingress, crossing state"),
            new("piezometer_network", "SavePiezometer", "SetupPiezometer", "infrastructure", "Plan 189 — aquifer monitoring network state driving the water-treatment intake advisory gate"),
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
            new("anomaly_hazard", "SaveAnomalyHazard", "SetupAnomalyHazard", "world", "Plan 176 — authored anomaly and storm-front hazard zones, movement, warnings, loot-site resolution"),
            new("desperation", "SaveDesperation", "SetupDesperation", "survival", "Starvation crisis desperation acts and cannibalism history"),
            new("mercenary_bounties", "SaveMercenary", "SetupMercenary", "economy", "Mercenary bounty contracts, target intel, and rival tracking"),
            new("archaeology", "SaveArchaeology", "SetupArchaeology", "knowledge", "Archaeology excavation ruins, archive decryption, and lore unlocks", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("amputation", "SaveAmputation", "SetupAmputation", "medical", "Infection progression, amputations, prosthetics and bionics"),
            new("bionics", "SaveBionics", "SetupBionics", "medical", "Plan 177 — bionic implant instances: condition, integration, power, maintenance, complications"),
            new("zealotry", "SaveZealotry", "SetupZealotry", "social", "Plan 175 — fictional ideological pressure: belief state, fervor, dissent, shrines, escalation"),
            new("spiritual_meaning", "SaveSpiritual", "SetupSpiritual", "spiritual", "Plan 30 — spiritual-meaning coordinator: mourning arcs, ritual cooldowns, memorial rites"),
            new("railway", "SaveRailway", "SetupRailway", "expedition", "Rail network, track repair, and armored train operations"),
            new("fungi_cultivation", "SaveFungi", "SetupFungi", "farming", "Subterranean fungi beds, substrate, spores, and blooms", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("bio_fermentation", "SaveBioFermentation", "SetupBioFermentation", "farming", "Plan 126 — fermentation reactor, process health, contamination, outputs", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("contraband_stash", "SaveContrabandStash", "SetupContrabandStash", "narrative", "Plan 147 — bunker contraband stash claim ledger (once-only discovery)"),
            new("shelter_barter", "SaveShelterBarter", "SetupShelterBarter", "economy", "Plan 54/147 — shelter barter caravans, pinned stock, and the contraband broker counter"),
            new("black_projects_archive", "SaveBlackProjectsArchive", "SetupBlackProjectsArchive", "narrative", "Plan 152 — Black Projects intelligence archive: discovered-record ledger (IDs only)"),
            new("oral_lore", "SaveOralLore", "SetupOralLore", "narrative", "Plan 155 — oral lore first-heard ledger (lore IDs only)"),
            new("hydrogeology_archive", "SaveHydroGeologyDiscovery", "SetupHydroGeologyDiscovery", "narrative", "Plan 154 — Hydrogeology science archive: discovered-record ledger (IDs only)"),
            new("technical_material_archive", "SaveTechnicalMaterialArchive", "SetupTechnicalMaterialArchive", "narrative", "Plan 158 — cordage/cable/polymer/textile technical material archive: discovered-record ledger (IDs only)"),
            new("grain_milling_archive", "SaveGrainMillingArchive", "SetupGrainMillingArchive", "narrative", "Plan 157 — Grain milling, storage & food-processing knowledge archive: discovered-record ledger (IDs only)"),
            new("leatherwork_archive", "SaveLeatherworkArchive", "SetupLeatherworkArchive", "narrative", "Plan 159 — Tanning/leather material provenance & workshop knowledge archive: discovered-record ledger (IDs only)"),
            new("plastic_pyrolysis", "SavePlasticPyrolysis", "SetupPlasticPyrolysis", "industry", "Retort bay — waste plastic to synthetic fuel fractions (Plan 202)", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("cargo_airdrop", "SaveCargoAirdrop", "SetupCargoAirdrop", "expedition", "Airdrop events, crate contents, beacons, and interception races (Plan 205)", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("wasteland_justice", "SaveJustice", "SetupJustice", "narrative", "Crime incidents, trials, punishments, banishments, and grudges"),
            new("child_development", "SaveGenerational", "SetupGenerational", "social", "Child development phases, education, trauma, and adulthood", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("prisoner_management", "SavePrisoners", "SetupPrisoners", "factions", "Captive detention, upkeep, interrogation, escape, and recruitment"),
            new("food_preservation", "SaveFoodPreservation", "SetupPlans62To65", "shelter", "Food spoilage, curing, and cryogenic preservation"),
            new("prewar_archives", "SavePrewarArchives", "SetupPlans62To65", "knowledge", "Pre-war archive discovery and decryption"),
            new("mutation_tree", "SaveMutations", "SetupMutations", "medical", "Radiation exposure, genetic instability, and mutation trees"),
            new("expedition_stealth", "SaveStealth", "SetupStealth", "combat", "Expedition stealth, detection risk, camouflage, and night ops"),
            new("aviation", "SaveAviation", "SetupAviation", "expedition", "Aviation airframes, flight plans, aerial mapping, and crash rescue", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("forced_labor", "SaveForcedLabor", "SetupForcedLabor", "factions", "Captive forced labor assignments, cruelty index, and rebellion risks"),
            new("narcotics", "SaveNarcotics", "SetupNarcotics", "medical", "Chemical medicines, toxicity, tolerance, addiction, and rehab beds"),
            new("settlement_politics", "SavePolitics", "SetupPolitics", "narrative", "Settlement elections, political policies, approval rating, and coups"),
            // Flagship institutions (Tasks 5-8): culture / diplomacy / sky defense / sanatorium
            new("cultural_archives", "SaveCulturalArchive", "SetupCulturalArchive", "knowledge", "Deep-vault cultural archives: restoration, transcription, microfiche preservation, discs, salons, chronicles"),
            new("diplomatic_summits", "SaveDiplomaticSummit", "SetupDiplomaticSummit", "factions", "Wasteland summits, treaty lifecycle, guarantees, DMZ rules, violations"),
            new("sky_defense_battery", "SaveSkyDefense", "SetupSkyDefense", "combat", "Kinetic sky-layer counter-battery: turret state, magazine, tracks, maintenance"),
            new("psychological_sanatorium", "SaveSanatorium", "SetupSanatorium", "medical", "Trauma sanatorium: admissions, therapies, sedatives, relapse, discharge"),
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
            new("narrative_questlines", "SaveNarrativeQuestlines", "SetupNarrativeQuestlines", "quests", "Survivor narrative questline arcs and crisis branch outcomes"),
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
            new("commitment", "SaveCommitments", "SetupCommitments", "campaign", "Plan 38 — authored obligations/deadlines: warnings, met/missed terminal state, and consequence routing", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("session_durability", "SaveSessionDurability", "SetupSessionDurability", "save", "Plan 39 — observed slot summaries, day-advance soak samples, and corruption/recovery audit", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("playable_metrics", "SavePlayMetrics", "SetupPlayMetrics", "save", "Plan 46 — local session telemetry: aggregate readiness report and first-hour funnel completion", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("survivor_voice", "SaveSurvivorVoice", "SetupSurvivorVoice", "survivors", "Plan 42 — survivor voice line catalog selection, cooldowns, utterance history, and playback dispatch", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("seven_day_slice", "SaveSevenDaySlice", "SetupSevenDaySlice", "save", "Plan 54 — seven-day slice playtest scorecard: frozen scenario hash, beat verification evidence, retention counts", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("survivor_autonomy", "SaveSurvivorAutonomy", "SetupSurvivorAutonomy", "survivors", "ORPHAN-SEAL-W1 — daily survivor autonomy evaluations, cooldowns, and personal goals", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("nuclear_winter_progression", "SaveNuclearWinter", "SetupNuclearWinter", "world", "ORPHAN-SEAL-W1 — nuclear-winter phase/season progression and recorded climate events", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("seasonal_celebration", "SaveSeasonalCelebration", "SetupSeasonalCelebration", "events", "ORPHAN-SEAL-W1 — holidays, anniversaries, scales, and celebration history", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("disaster_response", "SaveDisasterResponse", "SetupDisasterResponse", "shelter", "ORPHAN-SEAL-W1 — active disasters, protocols, mitigation, and resilience", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("communications", "SaveCommunications", "SetupCommunications", "radio", "ORPHAN-SEAL-W1 — antenna layer, intercepted messages, and outgoing broadcasts", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("colony", "SaveColony", "SetupColony", "expedition", "ORPHAN-SEAL-W1 — player-founded colonies, buildings, and supply lines", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("hobby", "SaveHobby", "SetupHobby", "survivors", "ORPHAN-SEAL-W1 — survivor hobby progress, mastery, and co-participation", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("survivor_education", "SaveSurvivorEducation", "SetupSurvivorEducation", "survivors", "ORPHAN-SEAL-W1 — learner records, subjects, teachers, and graduation", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_expansion", "SaveShelterExpansion", "SetupShelterExpansion", "shelter", "ORPHAN-SEAL-W1 — expansion rooms, construction projects, and upgrade state", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("confession_secret", "SaveConfessionSecrets", "SetupConfessionSecrets", "survivors", "ORPHAN-SEAL-W1 — discovered secrets, resolution choices, and leverage records", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_festival", "SaveShelterFestival", "SetupShelterFestival", "events", "ORPHAN-SEAL-W1 — player-scheduled festivals, commodity costs, and completion state", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("faction_covert_ops", "SaveFactionCovertOps", "SetupFactionCovertOps", "factions", "ORPHAN-SEAL-W1 — rival-faction covert operations, suspicion ladder, and intelligence reports", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("lyophilization", "SaveLyophilization", "SetupLyophilization", "medical", "Plans 130-133 — preserved-biologic batches and viability ledger", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("draisine_recovery", "SaveDraisineRerailing", "SetupDraisineRerailing", "expedition", "Plans 130-133 — armored draisine derailment recovery"),
            new("route_infrastructure", "SaveRouteInfrastructure", "SetupRouteInfrastructure", "world", "Plans 146-149 — mutable route infrastructure, corridor maintenance, and minefield clearance"),
            new("ebpvd_coating", "SaveEbPvdCoating", "SetupEbPvdCoating", "shelter", "Plans 146-149 — EB-PVD thermal barrier coating machinery, job state, and records"),
            new("microfluidic_diagnostic", "SaveMicrofluidicDiagnostic", "SetupMicrofluidicDiagnostic", "medical", "Plans 146-149 — microfluidic diagnostic cartridge manufacturing and run records"),
            new("mine_clearing_flail", "SaveMineClearingFlail", "SetupMineClearingFlail", "expeditions", "Plans 146-149 — mine-clearing flail vehicle modules and active breaches"),
            new("rail_grinding", "SaveRailGrinding", "SetupRailGrinding", "expeditions", "Plans 146-149 — rail grinding vehicle modules and active corridor jobs"),
            new("agriculture", "SaveAgriculture", "SetupAgriculture", "farming", "Plans 162-165 — advanced crop strains, plot medium, pests, compost, and dietary diversity"),
            new("retention", "SaveRetention", "SetupRetention", "records", "Plan 55 — retention policy audit report: bounded collection bounds, pruned totals, and preserved obligations"),
            new("outpost_settlement", "SaveOutpostSettlement", "SetupOutpostSettlement", "expeditions", "Plan 58 — authored outposts: establishment, condition, garrison assignments, and ration reserve"),
            new("weather_cascade", "SaveWeatherCascade", "SetupWeatherCascade", "world", "Plan 135 — weather→gameplay cascade: active weather events, their effects, and the event history"),
            new("settlement_defenses", "SaveDefense", "SetupDefense", "combat", "Plans 162-165 — trap installations, pre-combat raid resolution, captures, and the raid log"),
            new("psychological_arcs", "SavePsychologyArcs", "SetupPsychologyArcs", "psychology", "Plans 162-165 — breakdown arcs, exposure, treatment progress, private stashes, catharsis"),
            new("wildlife_ecosystem", "SaveWildlifeEcosystem", "SetupWildlifeEcosystem", "hunting", "Plans 162-165 — ecology pressures, extinction flags, apex activity, taming, bestiary knowledge"),
            new("companion_animals", "SaveCompanionAnimals", "SetupCompanionAnimals", "hunting", "Plan 174 — persistent companion animals: care, bond, training, roles, sickness, assignments"),
            new("vehicle_garage", "SaveVehicleGarage", "SetupVehicleGarage", "expeditions", "Plans 50-53 — expedition overland vehicle modifications and garage maintenance state"),
            new("faction_espionage", "SaveShelterEspionage", "SetupShelterEspionage", "factions", "Plans 50-53 — shelter faction espionage, sleeper assets, counter-intel, and sabotage state"),
            new("survivor_mental_health", "SaveSurvivorMentalHealth", "SetupSurvivorMentalHealth", "psychology", "Plans 50-53 — survivor psychological trauma, stress levels, catharsis, and mental health crises"),
            new("seismic_dynamics", "SaveSeismicDynamics", "SetupSeismicDynamics", "shelter", "Plan B68 — fault tension, slips, geophone coverage, dampener integrity, quake history"),
            new("cryo_vault", "SaveCryoVault", "SetupCryoVault", "shelter", "Plan B69 — cryo canisters, viability, coolant reserve, insulation, breach state"),
            new("geothermal_orc", "SaveGeothermalOrc", "SetupGeothermalOrc", "power_grid", "Plan B74 — geothermal organic Rankine loop, heat reserve, fouling, and leakage"),
            new("ballistics_workbench", "SaveBallisticsWorkbench", "SetupBallisticsWorkbench", "combat", "Plan B75 — weapon calibration, headspace wear, custom ammunition, and failure state"),
            new("aeroponics", "SaveAeroponics", "SetupAeroponics", "farming", "Plan B76 — aeroponic chambers, nutrient chemistry, disease, lighting, and harvest"),
            new("pneumatic_dispatch", "SavePneumaticDispatch", "SetupPneumaticDispatch", "infrastructure", "Plan B77 — pneumatic stations, capsule routing, seals, jams, and blackout-safe dispatch"),
            new("precision_metrology", "SavePrecisionMetrology", "SetupPrecisionMetrology", "shelter", "Plan B89 — precision metrology grades, certificates, and registered-consumer calibration"),
            new("aquaponics", "SaveAquaponics", "SetupAquaponics", "farming", "Plan B87 — closed-loop aquaponics ecology, biofilter health, and harvest yields"),
            new("territory_control", "SaveTerritoryControl", "SetupTerritoryControl", "factions", "Plan 134 — dynamic faction territory & supply line control: contested locations, fortification, garrison, supply line status", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("cooking", "SaveCooking", "SetupCooking", "cooking", "Plan 136 — wildlife trapping food pipeline & cooking system: recipes, active operations, meals prepared, and food decontamination", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("campaign_legacy", "SaveCampaignLegacy", "SetupCampaignLegacy", "campaign", "Plan 140 — Generational legacy, heirlooms, campaign inheritance, and New Game+ multi-generational continuity", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("research_unlock", "SaveResearchUnlock", "SetupResearchUnlockBridge", "knowledge", "Plan 141 — Research downstream unlocks bridge: items, recipes, shelter, expedition, combat, and medical capabilities", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("unified_ending", "SaveUnifiedEnding", "SetupUnifiedEnding", "endgame", "Plan 145 — Unified ending resolution & epilogue personalization: political, social, moral, personal, and expedition resolution", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("npc_memory", "SaveNpcMemory", "SetupNpcMemory", "narrative", "Plan 147 — Per-NPC memory and relationship depth: trust, grudge, favors owed, forgiveness, and trade multipliers", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("ideological_friction", "SaveIdeologicalFriction", "SetupIdeologicalFriction", "survivors", "Plan 148 — Ideological friction events and quests: confrontations, conversions, bunker factions, and mediation", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("romance_family", "SaveRomanceFamily", "SetupRomanceFamily", "survivors", "Plan 150 — Romance & family dynamics: attraction, courtship, partnership, bonded pairs, family units, and adoption", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("vehicle_customization", "SaveVehicleCustomization", "SetupVehicleCustomization", "expedition", "Plan 152 — Vehicle customization & mobile base: module installation, effective vehicle stats, and deployed base camps", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("backstory", "SaveBackstory", "SetupBackstory", "survivors", "Plan 174 — Procedural survivor backstories & origin mechanics: occupations, experiences, and secrets", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("meta_progression", "SaveMetaProgression", "SetupMetaProgression", "endgame", "Plan 175 — Meta progression & cross-run profile store: prestige scoring, crests, and NG+ boons", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_identity", "SaveShelterIdentity", "SetupShelterIdentity", "holdfast", "Plan 166 — Shelter identity, naming, origin projection, emblems, mottos, infamy, and community legacy tags", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("trade_routes", "SaveTradeRoutes", "SetupTradeRoutes", "economy", "Plan 192 — Scheduled trade route contracts, tariffs, reliability tiers, and exclusive goods", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("human_migration", "SaveHumanMigration", "SetupHumanMigration", "world", "Plan 199 — Seasonal human migration engine, regional population weights, and dwell hysteresis", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_governance", "SaveShelterGovernance", "SetupShelterGovernance", "governance", "Plan 159 — Shelter governance & political system: ideological blocs, policy consent, civil disputes, and shelter stability", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("aging", "SaveAging", "SetupAging", "survivors", "Plan 176 — Aging & elderly survivor system: chronological age progression, life stages, retirement, elder mentorship, and milestones", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("shelter_maintenance", "SaveShelterMaintenance", "SetupShelterMaintenance", "shelter", "Plan 186 — Shelter maintenance & degradation: component condition, environmental stress, maintenance actions, and alert states", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("survivor_routines", "SaveSurvivorRoutines", "SetupSurvivorRoutines", "survivors", "Plan 188 — Individual survivor daily routines: activity time blocks, chronotypes, satisfaction evaluation, and interpersonal conflicts", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("difficulty_settings", "SaveDifficultySettings", "SetupDifficultySettings", "campaign", "Plan 181 — runtime difficulty settings: active preset, custom slider values, and the ironman lock. The campaign identity preset stays in the checksummed header.", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("rail_track_maintenance", "SaveRailTrackMaintenance", "SetupRailTrackMaintenance", "expeditions", "Expansion 25 — rail track gauge/wear/bridge maintenance ledger over the canonical RailwaySystem topology", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("glassworks", "SaveGlassworks", "SetupGlassworks", "shelter", "Expansion 29 — glass vitrification batches, vision prescriptions, and abrasive grit stock", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("broadsheet_press", "SaveBroadsheetPress", "SetupBroadsheetPress", "narrative", "Expansion 30 — movable type tray, ink and paper consumables, and the bound archive of printed publications. Rumor facts stay with RumorSystem; morale stays with the survivors' needs authority.", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("kilnworks", "SaveKilnworks", "SetupKilnworks", "shelter", "Expansion 31 — queued kiln batches, kiln fuel reserve, refractory lining wear, and drawn-output tallies. Metallurgy stays with CupolaFoundryEngine.", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("wildlife_harvest", "SaveWildlifeHarvest", "SetupWildlifeHarvest", "hunting", "Expansion 32 — per-species seasonal harvest ledger and sustainable quota", LifecycleGroup: ExpandedShelterLifecycleGroup),
            new("storm_forecast", "SaveStormForecast", "SetupStormForecast", "world", "Expansion 33 — observation-post forecast skill, storm-response drill recency, and issued warnings", LifecycleGroup: ExpandedShelterLifecycleGroup),
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
                { "commitment", "commitment_save.json" },
                { "session_durability", "session_durability_save.json" },
                { "playable_metrics", "playable_metrics_save.json" },
                { "survivor_voice", "survivor_voice_save.json" },
                { "seven_day_slice", "seven_day_slice_save.json" },
                { "survivor_autonomy", "survivor_autonomy_save.json" },
                { "nuclear_winter_progression", "nuclear_winter_progression_save.json" },
                { "seasonal_celebration", "seasonal_celebration_save.json" },
                { "disaster_response", "disaster_response_save.json" },
                { "communications", "communications_save.json" },
                { "colony", "colony_save.json" },
                { "hobby", "hobby_save.json" },
                { "survivor_education", "survivor_education_save.json" },
                { "shelter_expansion", "shelter_expansion_save.json" },
                { "confession_secret", "confession_secret_save.json" },
                { "shelter_festival", "shelter_festival_save.json" },
                { "faction_covert_ops", "faction_covert_ops_save.json" },
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
                { "sanitation", "sanitation_save.json" },
                { "deep_well", "deep_well_save.json" },
                { "water_condenser", "water_condenser_save.json" },
                { "anomaly_hazard", "anomaly_hazard_save.json" },
                { "black_market", "black_market_save.json" },
                { "verdict", "verdict_save.json" },
                { "maritime", "maritime_save.json" },
                { "expedition", "expedition_save.json" },
                { "combat", "combat_save.json" },
                { "narrative", "narrative_save.json" },
                { "echoes", "echoes_save.json" },
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
                { "weather_hardening", "weather_hardening_save.json" },
                { "geothermal_aquifer", "geothermal_aquifer_save.json" },
                { "counter_intelligence", "counter_intelligence_save.json" },
                { "recon_telemetry", "recon_telemetry_save.json" },
                { "sump_flooding", "sump_flooding_save.json" },
                { "decontamination", "decontamination_save.json" },
                { "kitchen_nutrition", "kitchen_nutrition_save.json" },
                { "grain_processing", "grain_processing_save.json" },
                { "cryogenic_air_separation", "cryogenic_air_separation_save.json" },
                { "library_study", "library_study_save.json" },
                { "research", "research_save.json" },
                { "espionage", "espionage_save.json" },
                { "fluid_logistics", "fluid_logistics_save.json" },
                { "procedural_narrative", "procedural_narrative_save.json" },
                { "archive_desk", "archive_desk_save.json" },
                { "contractor_roster", "contractor_roster_save.json" },
                { "mental_health_crisis", "mental_health_crisis_save.json" },
                { "shelter_assignment", "shelter_assignment_save.json" },
                { "shelter_decor", "shelter_decor_save.json" },
                { "shelter_atmosphere", "shelter_atmosphere_save.json" },
                { "shelter_noise", "shelter_noise_save.json" },
                { "hidden_agenda", "hidden_agenda_save.json" },
                { "shelter_reputation", "shelter_reputation_save.json" },
                { "propaganda_campaigns", "propaganda_save.json" },
                { "wasteland_rumors", "rumor_network_save.json" },
                { "shelter_security", "shelter_security_save.json" },
                { "visitor_integration", "visitor_integration_save.json" },
                { "time_capsules", "time_capsules_save.json" },
                { "death_legacy", "death_legacy_save.json" },
                { "relationship_decay", "relationship_decay_save.json" },
                { "survivor_social", "survivor_social_save.json" },
                { "morale_contagion", "morale_contagion_save.json" },
                { "pathogen_strains", "pathogen_strains_save.json" },
                { "subterranean", "subterranean_save.json" },
                { "psyops", "psyops_save.json" },
                { "radio_program_production", "radio_program_production_save.json" },
                { "low_background_metrology", "low_background_metrology_save.json" },
                { "insar_deformation", "insar_deformation_save.json" },
                { "hydraulic_extrusion", "hydraulic_extrusion_save.json" },
                { "runflat_tire", "runflat_tire_save.json" },
                { "sofc_power", "sofc_power_save.json" },
                { "cvd_diamond", "cvd_diamond_save.json" },
                { "sound_ranging", "sound_ranging_save.json" },
                { "amphibious_draisine", "amphibious_draisine_save.json" },
                { "piezometer_network", "piezometer_network_save.json" },
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
                { "bio_fermentation", "bio_fermentation_save.json" },
                { "contraband_stash", "contraband_stash_save.json" },
                { "shelter_barter", "shelter_barter_save.json" },
                { "black_projects_archive", "black_projects_archive_save.json" },
                { "oral_lore", "oral_lore_save.json" },
                { "hydrogeology_archive", "hydrogeology_archive_save.json" },
                { "technical_material_archive", "technical_material_archive_save.json" },
                { "grain_milling_archive", "grain_milling_archive_save.json" },
                { "leatherwork_archive", "leatherwork_archive_save.json" },
                { "plastic_pyrolysis", "plastic_pyrolysis_save.json" },
                { "cargo_airdrop", "cargo_airdrop_save.json" },
                { "wasteland_justice", "wasteland_justice_save.json" },
                { "child_development", "child_development_save.json" },
                { "prisoner_management", "prisoner_save.json" },
                { "mutation_tree", "mutation_save.json" },
                { "expedition_stealth", "stealth_save.json" },
                { "aviation", "aviation_save.json" },
                { "forced_labor", "forced_labor_save.json" },
                { "narcotics", "narcotics_save.json" },
                { "settlement_politics", "settlement_politics_save.json" },
                { "cultural_archives", "cultural_archives_save.json" },
                { "diplomatic_summits", "diplomatic_summits_save.json" },
                { "sky_defense_battery", "sky_defense_battery_save.json" },
                { "psychological_sanatorium", "psychological_sanatorium_save.json" },
                { "endgame", "endgame_save.json" },
                { "caravan_trade_network", "caravan_trade_network_save.json" },
                { "surgical_ward", "surgical_ward_save.json" },
                { "power_subgrids", "power_subgrids_save.json" },
                { "perimeter_defense", "perimeter_defense_save.json" },
                { "hydroponic_biomes", "hydroponic_biomes_save.json" },
                { "agriculture", "agriculture_save.json" },
                { "retention", "retention_save.json" },
                { "outpost_settlement", "outpost_settlement_save.json" },
                { "weather_cascade", "weather_cascade_save.json" },
                { "settlement_defenses", "settlement_defenses_save.json" },
                { "psychological_arcs", "psychological_arcs_save.json" },
                { "wildlife_ecosystem", "wildlife_ecosystem_save.json" },
                { "companion_animals", "companion_animals_save.json" },
                { "bionics", "bionics_save.json" },
                { "zealotry", "zealotry_save.json" },
                { "spiritual_meaning", "spiritual_meaning_save.json" },
                { "nuclear_core_lifecycle", "nuclear_core_lifecycle_save.json" },
                { "armored_crawlers", "armored_crawlers_save.json" },
                { "personal_quests", "personal_quests_save.json" },
                { "narrative_questlines", "narrative_questlines_save.json" },
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
                { "route_infrastructure", "route_infrastructure_save.json" },
                { "ebpvd_coating", "ebpvd_coating_save.json" },
                { "microfluidic_diagnostic", "microfluidic_diagnostic_save.json" },
                { "mine_clearing_flail", "mine_clearing_flail_save.json" },
                { "rail_grinding", "rail_grinding_save.json" },
                { "food_preservation", "food_preservation_save.json" },
                { "prewar_archives", "prewar_archives_save.json" },
                { "vehicle_garage", "vehicle_garage_save.json" },
                { "faction_espionage", "faction_espionage_save.json" },
                { "survivor_mental_health", "survivor_mental_health_save.json" },
                { "seismic_dynamics", "seismic_dynamics_save.json" },
                { "cryo_vault", "cryo_vault_save.json" },
                { "geothermal_orc", "geothermal_orc_save.json" },
                { "ballistics_workbench", "ballistics_workbench_save.json" },
                { "aeroponics", "aeroponics_save.json" },
                { "pneumatic_dispatch", "pneumatic_dispatch_save.json" },
                { "precision_metrology", "precision_metrology_save.json" },
                { "aquaponics", "aquaponics_save.json" },
                { "territory_control", "territory_control_save.json" },
                { "cooking", "cooking_save.json" },
                { "campaign_legacy", "campaign_legacy_save.json" },
                { "research_unlock", "research_unlock_save.json" },
                { "unified_ending", "unified_ending_save.json" },
                { "npc_memory", "npc_memory_save.json" },
                { "ideological_friction", "ideological_friction_save.json" },
                { "romance_family", "romance_family_save.json" },
                { "vehicle_customization", "vehicle_customization_save.json" },
                { "backstory", "backstory_save.json" },
                { "meta_progression", "meta_progression_save.json" },
                { "shelter_identity", "shelter_identity_save.json" },
                { "trade_routes", "trade_routes_save.json" },
                { "human_migration", "human_migration_save.json" },
                { "shelter_governance", "shelter_governance_save.json" },
                { "aging", "aging_save.json" },
                { "shelter_maintenance", "shelter_maintenance_save.json" },
                { "survivor_routines", "survivor_routines_save.json" },
                { "difficulty_settings", "difficulty_settings_save.json" },
                { "rail_track_maintenance", "rail_track_maintenance_save.json" },
                { "glassworks", "glassworks_save.json" },
                { "broadsheet_press", "broadsheet_press_save.json" },
                { "kilnworks", "kilnworks_save.json" },
                { "wildlife_harvest", "wildlife_harvest_save.json" },
                { "storm_forecast", "storm_forecast_save.json" },
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
