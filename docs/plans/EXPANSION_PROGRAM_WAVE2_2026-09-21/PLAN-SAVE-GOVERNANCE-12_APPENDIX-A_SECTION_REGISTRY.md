# PLAN-SAVE-GOVERNANCE-12 — Appendix A: Save Section Registry Inventory

**Generated:** 2026-09-21 from `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
(204 sections).
**Columns:** section key · save method · setup method (`—` = null) · owner ·
description.
**Use:** SG-12A/12B — every section must appear once, its methods must exist on
`Main`, and a new section needs the same row plus a matrix/count update.
**Setup method census:** 202 with dedicated setup · 2 without.

| Section key | Save method | Setup method | Owner | Description |
|---|---|---|---|---|
| `journal` | `SaveJournal` | `SetupJournal` | `journal` | Player journal, logs, and codex entries |
| `holdfast` | `SaveHoldfast` | `SetupHoldfastRuntime` | `holdfast` | Holdfast S1 bunker state |
| `holdfast_trade` | `SaveHoldfastRuntime` | `SetupHoldfastRuntime` | `holdfast` | Holdfast trade session state |
| `duty_roster` | `SaveDutyRoster` | `SetupDutyRoster` | `duty_roster` | Duty roster shifts and assignments |
| `expansion_hub` | `SaveExpansionHub` | `SetupExpansions` | `expansion_hub` | Expansion hub discovery state |
| `expansion_quest` | `SaveExpansionQuests` | `SetupExpansionQuests` | `expansion_quest` | Expansion questline progression |
| `thirdonary` | `SaveThirdonary` | `SetupThirdonary` | `thirdonary` | Thirdonary covenant & dispute states |
| `phantom_memory` | `SavePhantomMemory` | `SetupPhantom` | `phase0` | Phantom memory lineages and echoes |
| `dose_ledger` | `SaveDoseLedger` | `SetupDoseLedger` | `dose_ledger` | Survivor radiation dose ledger & cohorts |
| `muster` | `SaveMuster` | `SetupMuster` | `muster` | The Muster military rally & conflict state |
| `inventory` | `SaveInventory` | `SetupInventory` | `inventory` | Shelter warehouse & items storage |
| `survivors` | `SaveSurvivors` | `SetupSurvivors` | `survivors` | Living survivors, needs, and traits |
| `economy` | `SaveEconomy` | `SetupEconomy` | `economy` | Dynamic economy rates and market orders |
| `sanitation` | `SaveSanitation` | `SetupSanitation` | `shelter` | Plan 210 — room waste, hygiene, compost queue, and spills |
| `deep_well` | `SaveDeepWell` | `SetupDeepWell` | `infrastructure` | B5–B8 Phase 6 — built deep-well pump: build state, condition, yield ledger (raw water into treatment via the Plan 189 intake seam) |
| `water_condenser` | `SaveWaterCondenser` | `SetupWaterCondenser` | `infrastructure` | B5–B8 expansion — Peltier condensation array: build state, membrane integrity, weather-indexed yield ledger |
| `black_market` | `SaveBlackMarket` | `SetupBlackMarket` | `economy` | Plan 211 — underworld contacts, stock snapshots, debts, heat, and trust |
| `verdict` | `SaveVerdict` | `SetupVerdict` | `verdict` | The Verdict investigation and tribunal state |
| `maritime` | `SaveMaritime` | `SetupMaritime` | `maritime` | The Black Flotilla dives and naval wrecks |
| `expedition` | `SaveExpeditions` | `SetupExpeditions` | `expeditions` | Wasteland expedition runs & status |
| `combat` | `SaveCombat` | `SetupCombat` | `combat` | Combat encounters and tactical trauma |
| `narrative` | `SaveNarrative` | `SetupNarrative` | `narrative` | Branching story arcs and narrative flags |
| `echoes` | `SaveEchoes` | `SetupEchoes` | `narrative` | Field echoes: surfaced and resolved one-time narrative state |
| `medical` | `SaveMedical` | `SetupMedical` | `medical` | Triage, illnesses, and treatments |
| `medical_pipeline` | `SaveMedicalPipeline` | `SetupMedical` | `medical` | Diagnosis knowledge, treatment reservations, scheduled procedures (Task #133) |
| `world` | `SaveWorld` | `SetupWorld` | `world` | World map nodes, sectors, and discovery |
| `crafting` | `SaveCrafting` | `SetupCrafting` | `crafting` | Known recipes and workbench queues |
| `caravan` | `SaveCaravans` | `SetupCaravans` | `caravans` | Trade caravans, routes, and arrivals |
| `campaign_day` | `SaveCampaignDay` | `SetupCampaignDay` | `campaign` | Master campaign day counter & ticks |
| `year_of_ash` | `SaveYearOfAsh` | `SetupYearOfAsh` | `year_of_ash` | The Year of Ash harsh winter state |
| `phase0` | `SavePhase0` | `SetupPhase0` | `phase0` | Pre-war timeline and bunker startup |
| `starting_level` | `SaveStartingLevel` | `SetupStartingLevel` | `starting_level` | Bunker initial configuration & tier |
| `greenhouse` | `SaveGreenhouse` | `SetupGreenhouse` | `greenhouse` | Hydroponic crops and food production |
| `host_event` | `SaveEventAdapter` | `SetupEventAdapter` | `events` | Host event ledger & moral decisions |
| `moral_choice` | `SaveMoralChoice` | `SetupMoralChoice` | `events` | Moral choice ledger and community trust |
| `radio` | `SaveRadio` | `SetupRadio` | `radio` | Radio frequencies, logs, and distress signals |
| `daily_briefing` | `SaveDailyBriefing` | `SetupDailyBriefingModal` | `campaign` | Daily dawn briefing notes & status |
| `power_grid` | `SavePowerGrid` | `SetupPowerGrid` | `power_grid` | Shelter generator & power allocations |
| `medical_ward` | `SaveMedicalWard` | `SetupMedicalWard` | `medical` | Hospital ward beds and inpatients |
| `memorial` | `SaveMemorial` | `SetupMemorial` | `memorial` | Fallen survivors memorial wall |
| `silent_foundry` | `SaveSilentFoundry` | `SetupSilentFoundry` | `foundry` | Automated foundry machinery & smelters |
| `disease` | `SaveDisease` | `SetupDisease` | `medical` | Epidemics, contagions, and pathogen spread |
| `wasteland_map` | `SaveWastelandMap` | — | `world` | Wasteland map markers and fog-of-war |
| `encounter_choice` | `SaveEncounterChoice` | `SetupEncounterChoice` | `encounters` | Encounter choice history & outcomes |
| `travel_encounters` | `SaveTravelEncounters` | `SetupTravelEncounters` | `encounters` | Travel encounters and cooldown states |
| `water_treatment` | `SaveWaterTreatment` | `SetupWaterTreatment` | `infrastructure` | Water filtration and purification |
| `airlock_security` | `SaveAirlockSecurity` | `SetupAirlockSecurity` | `infrastructure` | Airlock decontamination and security |
| `apprenticeship` | `SaveApprenticeship` | `SetupApprenticeship` | `social` | Mentorship pairings and skill growth |
| `caregiving` | `SaveCaregiving` | `SetupCaregiving` | `social` | Childcare, elderly care, and comfort |
| `autopsy` | `SaveAutopsy` | `SetupAutopsy` | `medical` | Post-mortem forensic analysis |
| `chemical_dependency` | `SaveChemicalDependency` | `SetupMentalHealthCrisis` | `medical` | Substance dependencies and withdrawal |
| `equipment_condition` | `SaveEquipmentCondition` | `SetupEquipmentCondition` | `equipment` | Tool and weapon wear/repair |
| `survivor_relations` | `SaveSurvivorRelations` | `SetupSurvivorRelations` | `social` | Survivor affinities, feuds, and bonds |
| `regional_treaty` | `SaveRegionalTreaty` | `SetupRegionalTreaty` | `factions` | Faction treaties and non-aggression pacts |
| `vinyl_morale` | `SaveVinylMorale` | `SetupVinylMorale` | `morale` | Gramophone records and music morale |
| `wildlife_trapping` | `SaveWildlifeTrapping` | `SetupWildlifeTrapping` | `hunting` | Snares, game catches, and foraging |
| `excavation` | `SaveExcavation` | `SetupExcavation` | `shelter` | Shelter expansion rubble clearing |
| `waystation` | `SaveWaystation` | `SetupWaystation` | `infrastructure` | Wasteland outpost network & relay hubs |
| `shelter_thermal` | `SaveShelterThermal` | `SetupShelterThermal` | `thermal` | Heating, insulation, and frost protection |
| `shelter_schedule` | `SaveShelterSchedule` | `SetupShelterSchedule` | `schedule` | Shift rotations and curfews |
| `weather_hardening` | `SaveWeatherHardening` | `SetupWeatherHardening` | `infrastructure` | Cryo-ash weather hardening & thermal insulation |
| `geothermal_aquifer` | `SaveGeothermalAquifer` | `SetupGeothermalAquifer` | `infrastructure` | Deep geothermal boreholes & aquifer pumping |
| `counter_intelligence` | `SaveCounterIntelligence` | `SetupCounterIntelligence` | `factions` | Counter-intelligence, vetting, and defector management |
| `recon_telemetry` | `SaveReconTelemetry` | `SetupReconTelemetry` | `expeditions` | Long-range recon drones & high-altitude mapping |
| `sump_flooding` | `SaveSumpFlooding` | `SetupSumpFlooding` | `maintenance` | Bunker sump pump drainage & flood risk |
| `decontamination` | `SaveDecontamination` | `SetupDecontamination` | `radiation` | Rad-scrubbing showers and chambers |
| `kitchen_nutrition` | `SaveKitchenNutrition` | `SetupKitchenNutrition` | `nutrition` | Rationing recipes and caloric balance |
| `grain_processing` | `SaveGrainProcessing` | `SetupGrainProcessing` | `nutrition` | Grain milling, silo safety, and pest pressure |
| `cryogenic_air_separation` | `SaveCryogenicAirSeparation` | `SetupCryogenicAirSeparation` | `infrastructure` | Abstract gas production and plant condition |
| `library_study` | `SaveLibraryStudy` | `SetupLibraryStudy` | `knowledge` | Research library books and blueprints |
| `research` | `SaveResearch` | — | `knowledge` | Research knowledge progress: unlocked, active, and completed nodes (Plan 34) |
| `espionage` | `SaveEspionage` | `SetupPlans166To169` | `factions` | Campaign intelligence networks, missions, and captured agents |
| `fluid_logistics` | `SaveFluidLogistics` | `SetupPlans166To169` | `infrastructure` | Shelter fluid topology, pressure, leaks, and distributed quality |
| `procedural_narrative` | `SaveProceduralNarrative` | `SetupPlans166To169` | `quests` | Procedural narrative metadata and the shared quest runtime |
| `archive_desk` | `SaveArchiveDesk` | `SetupArchiveDesk` | `knowledge` | Document archiving, ink, and scribing |
| `contractor_roster` | `SaveContractorRoster` | `SetupContractorRoster` | `personnel` | Hired mercenaries and specialists |
| `mental_health_crisis` | `SaveMentalHealthCrisis` | `SetupMentalHealthCrisis` | `psychology` | Psychological trauma and psych ward |
| `shelter_assignment` | `SaveShelterAssignment` | `SetupShelterAssignment` | `shelter` | Room assignments and living quarters |
| `shelter_decor` | `SaveShelterDecor` | `SetupShelterDecor` | `shelter` | Room decor placements, memorial plaques, and localized morale items |
| `shelter_atmosphere` | `SaveShelterAtmosphere` | `SetupShelterAtmosphere` | `shelter` | Plan 220 — shelter composite atmosphere, ambiance profile, and environmental facets |
| `shelter_noise` | `SaveShelterAtmosphere` | `SetupShelterAtmosphere` | `shelter` | Plan 205 — shelter acoustic noise, room soundproofing, and quiet hours |
| `hidden_agenda` | `SaveHiddenAgenda` | `SetupHiddenAgenda` | `survivors` | Plan 132 — survivor hidden agendas, secret motivations, clue discovery, and confrontation arcs |
| `shelter_reputation` | `SaveShelterReputation` | `SetupShelterReputation` | `shelter` | Plan 207 — shelter reputation, notoriety, public tags, and external perception |
| `propaganda_campaigns` | `SavePropaganda` | `SetupPropaganda` | `shelter` | Plan 168 — propaganda messages, multi-day campaigns, detection, and morale warfare |
| `wasteland_rumors` | `SaveRumorNetwork` | `SetupRumorNetwork` | `world` | Plan 203 / 131 — wasteland rumors, information hubs, propagation, and intercepts |
| `shelter_security` | `SaveShelterSecurity` | `SetupShelterSecurity` | `shelter` | Plan 138 — shelter security zones, clearances, locks, lockdowns, and breaches |
| `time_capsules` | `SaveTimeCapsules` | `SetupTimeCapsules` | `communication` | Plan 212 — time capsules, legacy messages, delayed discovery, and cross-generational communication |
| `death_legacy` | `SaveDeathLegacy` | `SetupDeathLegacy` | `survivors` | Plan 206 — survivor death records, last wills, estate inheritance, and disputes |
| `relationship_decay` | `SaveRelationshipDecay` | `SetupRelationshipDecay` | `social` | Plan 182 — survivor pair bond decay, interaction tracking, and social drift |
| `survivor_social` | `SaveSurvivorSocial` | `SetupSurvivorSocial` | `social` | Leadership, friction, ration conflict, trauma bonds, skill atrophy |
| `morale_contagion` | `SaveMoraleContagion` | `SetupMoraleContagion` | `social` | Flagship XI Plan 154 — morale contagion channels, breakdowns, social isolation, schism ledger, HopeBeacon installation |
| `pathogen_strains` | `SavePathogenStrains` | `SetupPathogenStrains` | `medical` | Flagship XI Plan 155 — fictional strain layer: cure projects and unlocked cures |
| `subterranean` | `SaveSubterranean` | `SetupSubterranean` | `world` | Flagship XI Plan 156 — generated underground topology, oxygen/collapse/flood/shoring state, discovery |
| `psyops` | `SavePsyOps` | `SetupPsyOps` | `radio` | Flagship XI Plan 157 — broadcast campaigns, jamming, counter-propaganda, ideological pressure |
| `radio_program_production` | `SaveRadioProgramProduction` | `SetupRadioProgramProduction` | `radio` | Plan 173 — player radio program prep/delivery jobs and follow-ups |
| `low_background_metrology` | `SaveLowBackgroundMetrology` | `SetupLowBackgroundMetrology` | `radiation` | Plan 138 — low-background shield install, detector calibration, smelting batches, bounded assay history |
| `insar_deformation` | `SaveInSarMapping` | `SetupInSarMapping` | `world` | Plan 139 — repeat-pass InSAR survey passes, coherence, deformation summaries, excavation/travel intelligence |
| `hydraulic_extrusion` | `SaveHydraulicExtrusion` | `SetupHydraulicExtrusion` | `foundry` | Plan 140 — advanced hydraulic extrusion batches, tooling condition, quality grades |
| `runflat_tire` | `SaveRunFlatTire` | `SetupRunFlatTire` | `expeditions` | Plan 141 — run-flat wheel profiles, integrity, heat, rim/bead, rolling-resistance cost |
| `sofc_power` | `SaveSofcPower` | `SetupSofcPower` | `shelter` | Plan 122 — SOFC plant operating mode, thermal level, stack health, seal integrity, degradation, faults |
| `cvd_diamond` | `SaveCvdDiamond` | `SetupCvdDiamond` | `shelter` | Plan 124 — CVD diamond reactor condition, plasma stability, growth batches, faults |
| `sound_ranging` | `SaveSoundRanging` | `SetupSoundRanging` | `combat` | Plan 123 — defensive sound-ranging calibration, node status, observation history, threat estimate |
| `amphibious_draisine` | `SaveAmphibiousDraisine` | `SetupAmphibiousDraisine` | `expeditions` | Plan 125 — per-vehicle amphibious kit condition, pontoons, ingress, crossing state |
| `piezometer_network` | `SavePiezometer` | `SetupPiezometer` | `infrastructure` | Plan 189 — aquifer monitoring network state driving the water-treatment intake advisory gate |
| `survivor_fate` | `SaveSurvivorFate` | `SetupSurvivorFate` | `memorial` | Unified survivor-death ledger: one immutable fate record per deceased survivor |
| `weight_of_choices` | `SaveFactionBranch` | `SetupFactionBranch` | `factions` | Weight of choices faction branch progression and PoNR commitments |
| `onboarding` | `SaveOnboarding` | `SetupOnboarding` | `onboarding` | First-hour onboarding journey progress, dismissed hints, assistance level, completion |
| `ecological_infestation` | `SaveEcologicalInfestation` | `SetupEcologicalInfestation` | `world` | Plan 28 — location and shelter ecological infestations (trigger/clear/tolerate lifecycle) |
| `field_guide` | `SaveFieldGuide` | `SetupFieldGuide` | `world` | Plan 20A/28 — field-guide unlocked-entry ledger (reading-the-land knowledge) |
| `shelter_workshop` | `SaveWorkshop` | `SetupWorkshop` | `shelter` | Precision workshop tooling, ammo press, and firearm refurbishment |
| `radio_station` | `SaveRadioStation` | `SetupRadioStation` | `radio` | Radio station frequency tuning, signal lock, and triangulation |
| `heliograph` | `SaveHeliograph` | `SetupHeliograph` | `radio` | Optical heliograph stations and message delivery |
| `shelter_social_dynamics` | `SaveShelterSocial` | `SetupShelterSocial` | `social` | Living quarters privacy pressure, communal mess hall, and disputes |
| `excavation_hazards` | `SaveExcavationHazards` | `SetupExcavationHazards` | `shelter` | Subterranean methane, flood, spore hazards, and cave-in rescue operations |
| `chem_warfare` | `SaveChemWarfare` | `SetupChemWarfare` | `combat` | CBRN hazard warfare and toxic contamination |
| `comms_array` | `SaveCommsArray` | `SetupCommsArray` | `world` | Long-range communications array and satellite telemetry |
| `ceremony` | `SaveCeremony` | `SetupCeremony` | `narrative` | Communal ceremonies, festivals, truces, and morale |
| `robotics` | `SaveRobotics` | `SetupRobotics` | `crafting` | Pre-war robotics, directives, and automation |
| `recreation` | `SaveRecreation` | `SetupRecreation` | `shelter` | Survivor hobbies, downtime, and recreation |
| `fallout` | `SaveFallout` | `SetupFallout` | `world` | Radioactive fallout clouds, dispersal, and shelter sealing |
| `anomaly_hazard` | `SaveAnomalyHazard` | `SetupAnomalyHazard` | `world` | Plan 176 — authored anomaly and storm-front hazard zones, movement, warnings, loot-site resolution |
| `desperation` | `SaveDesperation` | `SetupDesperation` | `survival` | Starvation crisis desperation acts and cannibalism history |
| `mercenary_bounties` | `SaveMercenary` | `SetupMercenary` | `economy` | Mercenary bounty contracts, target intel, and rival tracking |
| `archaeology` | `SaveArchaeology` | `SetupArchaeology` | `knowledge` | Archaeology excavation ruins, archive decryption, and lore unlocks |
| `amputation` | `SaveAmputation` | `SetupAmputation` | `medical` | Infection progression, amputations, prosthetics and bionics |
| `bionics` | `SaveBionics` | `SetupBionics` | `medical` | Plan 177 — bionic implant instances: condition, integration, power, maintenance, complications |
| `zealotry` | `SaveZealotry` | `SetupZealotry` | `social` | Plan 175 — fictional ideological pressure: belief state, fervor, dissent, shrines, escalation |
| `spiritual_meaning` | `SaveSpiritual` | `SetupSpiritual` | `spiritual` | Plan 30 — spiritual-meaning coordinator: mourning arcs, ritual cooldowns, memorial rites |
| `railway` | `SaveRailway` | `SetupRailway` | `expedition` | Rail network, track repair, and armored train operations |
| `fungi_cultivation` | `SaveFungi` | `SetupFungi` | `farming` | Subterranean fungi beds, substrate, spores, and blooms |
| `bio_fermentation` | `SaveBioFermentation` | `SetupBioFermentation` | `farming` | Plan 126 — fermentation reactor, process health, contamination, outputs |
| `contraband_stash` | `SaveContrabandStash` | `SetupContrabandStash` | `narrative` | Plan 147 — bunker contraband stash claim ledger (once-only discovery) |
| `shelter_barter` | `SaveShelterBarter` | `SetupShelterBarter` | `economy` | Plan 54/147 — shelter barter caravans, pinned stock, and the contraband broker counter |
| `black_projects_archive` | `SaveBlackProjectsArchive` | `SetupBlackProjectsArchive` | `narrative` | Plan 152 — Black Projects intelligence archive: discovered-record ledger (IDs only) |
| `oral_lore` | `SaveOralLore` | `SetupOralLore` | `narrative` | Plan 155 — oral lore first-heard ledger (lore IDs only) |
| `hydrogeology_archive` | `SaveHydroGeologyDiscovery` | `SetupHydroGeologyDiscovery` | `narrative` | Plan 154 — Hydrogeology science archive: discovered-record ledger (IDs only) |
| `technical_material_archive` | `SaveTechnicalMaterialArchive` | `SetupTechnicalMaterialArchive` | `narrative` | Plan 158 — cordage/cable/polymer/textile technical material archive: discovered-record ledger (IDs only) |
| `grain_milling_archive` | `SaveGrainMillingArchive` | `SetupGrainMillingArchive` | `narrative` | Plan 157 — Grain milling, storage & food-processing knowledge archive: discovered-record ledger (IDs only) |
| `leatherwork_archive` | `SaveLeatherworkArchive` | `SetupLeatherworkArchive` | `narrative` | Plan 159 — Tanning/leather material provenance & workshop knowledge archive: discovered-record ledger (IDs only) |
| `plastic_pyrolysis` | `SavePlasticPyrolysis` | `SetupPlasticPyrolysis` | `industry` | Retort bay — waste plastic to synthetic fuel fractions (Plan 202) |
| `cargo_airdrop` | `SaveCargoAirdrop` | `SetupCargoAirdrop` | `expedition` | Airdrop events, crate contents, beacons, and interception races (Plan 205) |
| `wasteland_justice` | `SaveJustice` | `SetupJustice` | `narrative` | Crime incidents, trials, punishments, banishments, and grudges |
| `child_development` | `SaveGenerational` | `SetupGenerational` | `social` | Child development phases, education, trauma, and adulthood |
| `prisoner_management` | `SavePrisoners` | `SetupPrisoners` | `factions` | Captive detention, upkeep, interrogation, escape, and recruitment |
| `food_preservation` | `SaveFoodPreservation` | `SetupPlans62To65` | `shelter` | Food spoilage, curing, and cryogenic preservation |
| `prewar_archives` | `SavePrewarArchives` | `SetupPlans62To65` | `knowledge` | Pre-war archive discovery and decryption |
| `shelter_prisoners` | `SaveShelterPrisoners` | `SetupPlans62To65` | `factions` | Shelter prisoner custody, interrogation, parole, and recruitment |
| `mutation_tree` | `SaveMutations` | `SetupMutations` | `medical` | Radiation exposure, genetic instability, and mutation trees |
| `expedition_stealth` | `SaveStealth` | `SetupStealth` | `combat` | Expedition stealth, detection risk, camouflage, and night ops |
| `aviation` | `SaveAviation` | `SetupAviation` | `expedition` | Aviation airframes, flight plans, aerial mapping, and crash rescue |
| `forced_labor` | `SaveForcedLabor` | `SetupForcedLabor` | `factions` | Captive forced labor assignments, cruelty index, and rebellion risks |
| `narcotics` | `SaveNarcotics` | `SetupNarcotics` | `medical` | Chemical medicines, toxicity, tolerance, addiction, and rehab beds |
| `settlement_politics` | `SavePolitics` | `SetupPolitics` | `narrative` | Settlement elections, political policies, approval rating, and coups |
| `cultural_archives` | `SaveCulturalArchive` | `SetupCulturalArchive` | `knowledge` | Deep-vault cultural archives: restoration, transcription, microfiche preservation, discs, salons, chronicles |
| `diplomatic_summits` | `SaveDiplomaticSummit` | `SetupDiplomaticSummit` | `factions` | Wasteland summits, treaty lifecycle, guarantees, DMZ rules, violations |
| `sky_defense_battery` | `SaveSkyDefense` | `SetupSkyDefense` | `combat` | Kinetic sky-layer counter-battery: turret state, magazine, tracks, maintenance |
| `psychological_sanatorium` | `SaveSanatorium` | `SetupSanatorium` | `medical` | Trauma sanatorium: admissions, therapies, sedatives, relapse, discharge |
| `endgame` | `SaveEndgame` | `SetupEndgame` | `endgame` | Campaign endgame phase, ending selection, sealed epilogue report |
| `caravan_trade_network` | `SaveCaravanTrade` | `SetupCaravanTrade` | `economy` | Faction caravan trade network routes and arrivals |
| `surgical_ward` | `SaveSurgicalWard` | `SetupSurgicalWard` | `medical` | Advanced surgical ward operations and sterile field |
| `power_subgrids` | `SavePowerSubgrids` | `SetupPowerSubgrids` | `power_grid` | Power distribution sub-grid nodes and thermal state |
| `perimeter_defense` | `SavePerimeterDefense` | `SetupPerimeterDefense` | `combat` | Surface perimeter defense emplacements |
| `hydroponic_biomes` | `SaveHydroponicBiomes` | `SetupHydroponicBiomes` | `farming` | Hydroponic biome racks and crop state |
| `nuclear_core_lifecycle` | `SaveNuclearCore` | `SetupNuclearCore` | `power_grid` | Nuclear core lifecycle and thermal state |
| `armored_crawlers` | `SaveArmoredCrawlers` | `SetupArmoredCrawlers` | `expedition` | Armored crawler modules and forward camps |
| `personal_quests` | `SavePersonalQuests` | `SetupPersonalQuests` | `quests` | Survivor personal quest progression |
| `narrative_questlines` | `SaveNarrativeQuestlines` | `SetupNarrativeQuestlines` | `quests` | Survivor narrative questline arcs and crisis branch outcomes |
| `chemical_synthesis` | `SaveChemicalSynthesis` | `SetupChemicalSynthesis` | `crafting` | Chemical synthesis retorts and apparatus |
| `collectible_discovery` | `SaveCollectibles` | `SetupCollectibles` | `inventory` | One-time collectible discovery ledger |
| `unique_claims` | `SaveCollectibles` | `SetupCollectibles` | `inventory` | Global unique-item claim ledger |
| `shelter_fire` | `SaveShelterFire` | `SetupShelterFireHazard` | `shelter` | Shelter fire incidents, smoke, and brigade response |
| `dynamic_quests` | `SaveDynamicQuests` | `SetupDynamicQuests` | `quests` | Campaign-wide emergency dynamic quests |
| `geodetic_survey` | `SaveGeodeticSurvey` | `SetupGeodeticSurvey` | `world` | Plans 78-81 — survey monuments, observations, resolved triangles, and network accuracy |
| `kinetic_storage` | `SaveKineticStorage` | `SetupKineticStorage` | `power_grid` | Plans 78-81 — flywheel rotor, vacuum, bearing, and containment state |
| `chemical_recon` | `SaveChemicalRecon` | `SetupChemicalRecon` | `expeditions` | Plans 78-81 — chemical hazard observations, samples, and safe corridors |
| `chlor_alkali_synthesis` | `SaveChlorAlkali` | `SetupChlorAlkali` | `shelter` | Plans 110-113 — chlor-alkali electrolytic plant, membrane health, hazard load, and chemical production |
| `solar_concentrator` | `SaveSolarConcentrator` | `SetupSolarConcentrator` | `power_grid` | Plans 110-113 — parabolic solar concentrator, mirror condition, tracking mode, and thermal output |
| `precision_optics` | `SavePrecisionOptics` | `SetupPrecisionOptics` | `shelter` | Plans 110-113 — precision optical blank grinding, figure testing, and telescope/shield viewports |
| `ballistic_shield` | `SaveBallisticShield` | `SetupBallisticShield` | `combat` | Plans 110-113 — defensive ballistic shields, stances, integrity, and ground anchoring |
| `powder_metallurgy` | `SavePowderMetallurgy` | `SetupPowderMetallurgy` | `foundry` | Plans 130-133 — abstract advanced-material production quality and reliability |
| `nvis_communications` | `SaveNvisCommunications` | `SetupNvisCommunications` | `radio` | Plans 130-133 — regional NVIS status communications and recall queue |
| `lyophilization` | `SaveLyophilization` | `SetupLyophilization` | `medical` | Plans 130-133 — preserved-biologic batches and viability ledger |
| `draisine_recovery` | `SaveDraisineRerailing` | `SetupDraisineRerailing` | `expedition` | Plans 130-133 — armored draisine derailment recovery |
| `route_infrastructure` | `SaveRouteInfrastructure` | `SetupRouteInfrastructure` | `world` | Plans 146-149 — mutable route infrastructure, corridor maintenance, and minefield clearance |
| `ebpvd_coating` | `SaveEbPvdCoating` | `SetupEbPvdCoating` | `shelter` | Plans 146-149 — EB-PVD thermal barrier coating machinery, job state, and records |
| `microfluidic_diagnostic` | `SaveMicrofluidicDiagnostic` | `SetupMicrofluidicDiagnostic` | `medical` | Plans 146-149 — microfluidic diagnostic cartridge manufacturing and run records |
| `mine_clearing_flail` | `SaveMineClearingFlail` | `SetupMineClearingFlail` | `expeditions` | Plans 146-149 — mine-clearing flail vehicle modules and active breaches |
| `rail_grinding` | `SaveRailGrinding` | `SetupRailGrinding` | `expeditions` | Plans 146-149 — rail grinding vehicle modules and active corridor jobs |
| `agriculture` | `SaveAgriculture` | `SetupAgriculture` | `farming` | Plans 162-165 — advanced crop strains, plot medium, pests, compost, and dietary diversity |
| `settlement_defenses` | `SaveDefense` | `SetupDefense` | `combat` | Plans 162-165 — trap installations, pre-combat raid resolution, captures, and the raid log |
| `psychological_arcs` | `SavePsychologyArcs` | `SetupPsychologyArcs` | `psychology` | Plans 162-165 — breakdown arcs, exposure, treatment progress, private stashes, catharsis |
| `wildlife_ecosystem` | `SaveWildlifeEcosystem` | `SetupWildlifeEcosystem` | `hunting` | Plans 162-165 — ecology pressures, extinction flags, apex activity, taming, bestiary knowledge |
| `companion_animals` | `SaveCompanionAnimals` | `SetupCompanionAnimals` | `hunting` | Plan 174 — persistent companion animals: care, bond, training, roles, sickness, assignments |
| `vehicle_garage` | `SaveVehicleGarage` | `SetupVehicleGarage` | `expeditions` | Plans 50-53 — expedition overland vehicle modifications and garage maintenance state |
| `faction_espionage` | `SaveShelterEspionage` | `SetupShelterEspionage` | `factions` | Plans 50-53 — shelter faction espionage, sleeper assets, counter-intel, and sabotage state |
| `survivor_mental_health` | `SaveSurvivorMentalHealth` | `SetupSurvivorMentalHealth` | `psychology` | Plans 50-53 — survivor psychological trauma, stress levels, catharsis, and mental health crises |
| `seismic_dynamics` | `SaveSeismicDynamics` | `SetupSeismicDynamics` | `shelter` | Plan B68 — fault tension, slips, geophone coverage, dampener integrity, quake history |
| `cryo_vault` | `SaveCryoVault` | `SetupCryoVault` | `shelter` | Plan B69 — cryo canisters, viability, coolant reserve, insulation, breach state |
| `geothermal_orc` | `SaveGeothermalOrc` | `SetupGeothermalOrc` | `power_grid` | Plan B74 — geothermal organic Rankine loop, heat reserve, fouling, and leakage |
| `ballistics_workbench` | `SaveBallisticsWorkbench` | `SetupBallisticsWorkbench` | `combat` | Plan B75 — weapon calibration, headspace wear, custom ammunition, and failure state |
| `aeroponics` | `SaveAeroponics` | `SetupAeroponics` | `farming` | Plan B76 — aeroponic chambers, nutrient chemistry, disease, lighting, and harvest |
| `pneumatic_dispatch` | `SavePneumaticDispatch` | `SetupPneumaticDispatch` | `infrastructure` | Plan B77 — pneumatic stations, capsule routing, seals, jams, and blackout-safe dispatch |
| `precision_metrology` | `SavePrecisionMetrology` | `SetupPrecisionMetrology` | `shelter` | Plan B89 — precision metrology grades, certificates, and registered-consumer calibration |
| `aquaponics` | `SaveAquaponics` | `SetupAquaponics` | `farming` | Plan B87 — closed-loop aquaponics ecology, biofilter health, and harvest yields |
