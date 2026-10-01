# ASHFALL Data Authority & Master Catalog Registry

**Authoritative Location:** `Assets/StreamingAssets/Data/` | **Last Verified:** 2026-10-01
**Total Catalogs:** 712 | **Total Definitions:** 14490 | **Domain Families:** 32

> [!IMPORTANT]
> **DATA AUTHORITY INVARIANT (Invariant 6):**
> `Assets/StreamingAssets/Data/` is the single authoritative source of truth for all game definitions.
> Never invent an ID outside the master prefixes. All cross-references must resolve through Tier-1 or Tier-2 integrity rules.

---

## Master ID Prefix Routing Directory

| ID Prefix | Domain / Purpose | Primary Authoritative Files |
|---|---|---|
| `affliction_` | Medical & Psychological Afflictions | `disease_catalog.json, medical_texts.json` |
| `dose_` | Radiation Dosage & Treatment | `dose_items.json, dose_registers.json` |
| `echo_` | Memory & Historical Echoes | `echoes.json, memorial_echoes.json` |
| `encounter_` | Exploration Encounters | `narrative_encounters.json, door_encounters.json` |
| `event_` | World & Shelter Events | `events.json, year_of_ash_events.json, faction_war_events.json` |
| `expansion_` | Expansion Packs 01–10 | `expansion_item_tags.json, quests_expansion_05.json` |
| `faction_` | Factions & Alignments | `faction_lore.json, holdfast_factions.json, crossing_factions.json` |
| `flag_` | Narrative & World State Flags | `moral_choice_flags.json, dynamic_questlines.json` |
| `item_` | Items & Equipment | `items.json, dose_items.json, holdfast_items.json, etc.` |
| `loc_` | Locations & Points of Interest | `locations.json, dose_locations.json, duty_roster_locations.json` |
| `npc_` | Characters & Special Survivors | `characters.json, verdict_npcs.json` |
| `quest_` | Quests & Missions | `questline_master.json, moral_choice_quests.json, holdfast_quests.json` |
| `radio_` | Radio Transmissions & Scripts | `radio.json, faction_war_radio.json, year_of_ash_radio.json` |
| `recipe_` | Crafting & Reverse Engineering | `recipes.json, relic_recipes.json, pharma_recipes.json` |
| `trait_` | Survivor Traits & Backgrounds | `survivors.json, starting_survivors.json` |
| `zone_` | Map Zones & Hazards | `wasteland_map_v1.json, damaged_map_zones.json` |

---

## Tier-2 Foreign-Key Dependency Contracts

The following JSON property keys are validated as strict foreign keys by `CatalogIntegrityValidator`:

| Property Key | Target Domain | Resolving Registry / Catalogs |
|---|---|---|
| `resultItemId` / `requiredItemId` | Items | `items.json` & expansion item catalogs |
| `target_location_id` | Locations | `locations.json` & regional maps |
| `prereq_quest_id` / `nextQuestId` | Quests | `questline_master.json` & quest system |
| `giver_npc_id` | Characters | `characters.json`, `verdict_npcs.json` |
| `requiredTrait` | Survivor Traits | `survivors.json` trait definitions |
| `required_flag` / `set_flag` | World State Flags | Dynamic runtime state ledger |
| `recipe_id` | Crafting Recipes | `recipes.json`, `pharma_recipes.json` |
| `disease_id` / `affliction_id` | Medical Diseases | `disease_catalog.json` |

---

## Functional Catalog Encyclopedia by Domain Family

### Audio & Music (4 Catalogs, 248 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `audio_accessibility_cues.json` | 10 | `1.0.0` | `UNRESOLVED` | `SourceReference:audio_accessibility_cues.json` |
| `audio_cues.json` | 196 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterAudioCueCatalogLoader` |
| `audio_logs_expansion_05.json` | 30 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AudioConditionSystem` |
| `cassette_sets.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VinylMoraleSystem` |

### Combat & Warlords (4 Catalogs, 137 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `chemical_weapons.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ChemWarfareSystem` |
| `combat_arenas.json` | 1 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CombatArenaCatalog` |
| `combat_catalog.json` | 53 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CombatCatalog` |
| `warlord_doctrines.json` | 78 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WarlordDoctrineCatalog` |

### Core / Miscellaneous (231 Catalogs, 3595 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `accessibility_profiles.json` | 5 | `1.0.0` | `UNRESOLVED` | `SourceReference:accessibility_profiles.json` |
| `achievements.json` | 16 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AchievementCatalog` |
| `acoustic_triangulation_catalog.json` | 11 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `aeroponics_nutrient_catalog.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AeroponicsCatalogLoader, AeroponicsSystem` |
| `affliction_bridge_rules.json` | 12 | `1.0.0` | `UNRESOLVED` | `SourceReference:affliction_bridge_rules.json` |
| `aircraft_parts.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AircraftPartsCatalog` |
| `alloys_and_ores.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:alloys_and_ores.json` |
| `amphibious_draisine_catalog.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AmphibiousDraisineCatalogLoader` |
| `anomalies.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AbyssalAnomaliesCatalog` |
| `apprenticeship_catalog.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ApprenticeshipSystem` |
| `aquaponics_system_catalog.json` | 13 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AquaponicsCatalogLoader` |
| `archive_categories.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:archive_categories.json` |
| `archive_inks.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ArchiveInkCatalogLoader` |
| `armored_crawler_modules.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ArmoredCrawlerModuleCatalogLoader` |
| `art_forms.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ArtFormCatalog` |
| `asset_registry.json` | 355 | `1.0.0` | `UNRESOLVED` | `AssetRegistry` |
| `atmosphere_profiles.json` | 7 | `1.0.0` | `UNRESOLVED` | `SourceReference:atmosphere_profiles.json` |
| `atmospheric_sounding_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AtmosphericSoundingCatalogLoader` |
| `autonomy_actions.json` | 20 | `1.0.0` | `UNRESOLVED` | `SourceReference:autonomy_actions.json` |
| `backstory_templates.json` | 20 | `1.0.0` | `UNRESOLVED` | `SourceReference:backstory_templates.json` |
| `ballistic_shield_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BallisticShieldCatalogLoader` |
| `ballistics_workbench_catalog.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BallisticsWorkbenchCatalogLoader, BallisticsWorkbenchSystem` |
| `barter_rules.json` | 3 | `1.0.0` | `UNRESOLVED` | `SourceReference:barter_rules.json` |
| `belief_movements.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SpiritualCatalogLoader, SpiritualMeaningCoordinator` |
| `bio_fermentation_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BioFermentationCatalog` |
| `bionics.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BionicsCatalogLoader` |
| `bounty_board.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:bounty_board.json` |
| `breaching_equipment_catalog.json` | 17 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BreachingCatalogLoader` |
| `bunker_graffiti_postings.json` | 30 | `1.0.0` | `UNRESOLVED` | `SourceReference:bunker_graffiti_postings.json` |
| `camouflage_gear.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CamouflageGearCatalog` |
| `captive_interrogations.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CaptiveInterrogationCatalogLoader` |
| `caravans.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CaravanTradeRouteCatalogLoader` |
| `carbon_composite_catalog.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CarbonCompositeCatalogLoader, CarbonCompositeEngine` |
| `cargo_airdrop_catalog.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CargoAirdropCatalog` |
| `cascade_rules.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CascadeRuleCatalogLoader` |
| `cellulosic_ethanol_catalog.json` | 4 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `ceremonies.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CeremonySystem` |
| `chapter_profiles.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ChapterProfileCatalog` |
| `chemical_syntheses.json` | 10 | `1.0.0` | `UNRESOLVED` | `SourceReference:chemical_syntheses.json` |
| `chlor_alkali_synthesis_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ChlorAlkaliSynthesisCatalogLoader` |
| `chronic_conditions.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ChronicConditionsCatalog` |
| `climbing_winch_catalog.json` | 8 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `cohort_tuning.json` | 0 | `1.0.0` | `UNRESOLVED` | `SourceReference:cohort_tuning.json` |
| `collectibles.json` | 40 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CollectibleEffectDispatcher, CollectibleCatalogLoader` |
| `colony_blueprints.json` | 11 | `1.0.0` | `UNRESOLVED` | `SourceReference:colony_blueprints.json` |
| `commitments.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CommitmentCatalogLoader` |
| `commodity_baselines.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CommodityBaselineCatalogLoader` |
| `comms_targets.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CommsArraySystem` |
| `communication_templates.json` | 7 | `1.0.0` | `UNRESOLVED` | `SourceReference:communication_templates.json` |
| `communications_networks.json` | 12 | `1.0.0` | `UNRESOLVED` | `SourceReference:communications_networks.json` |
| `companion_animals.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CompanionAnimalCatalogLoader` |
| `conflict_templates.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ConflictTemplatesCatalog` |
| `cryo_cultivars.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CryoCultivarCatalogLoader` |
| `cryogenic_air_separation.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CryogenicAirSeparationCatalogLoader` |
| `cultural_archive_tomes.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CulturalArchiveTomeCatalogLoader` |
| `cvd_diamond_catalog.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CvdDiamondCatalogLoader` |
| `death_legacy_templates.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:death_legacy_templates.json` |
| `decontamination_protocol_catalog.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DeconProtocolCatalogLoader` |
| `defenses.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TrapCatalogLoader` |
| `development_traits.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DevelopmentTraitsCatalog` |
| `difficulty_presets.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DifficultyPresetCatalogLoader` |
| `diplomatic_treaties.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DiplomaticTreatyCatalogLoader` |
| `direction_finding_catalog.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DirectionFindingCatalogLoader` |
| `disaster_templates.json` | 11 | `1.0.0` | `UNRESOLVED` | `SourceReference:disaster_templates.json` |
| `discovery_consequences.json` | 5 | `1.0.0` | `UNRESOLVED` | `DiscoveryConsequenceCatalog` |
| `dream_templates.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:dream_templates.json` |
| `duty_roles.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:duty_roles.json` |
| `ebpvd_coating_catalog.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EbPvdCoatingCatalogLoader, EbPvdCoatingEngine` |
| `echoes.json` | 23 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EchoCatalogLoader` |
| `ecological_infestations.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EcologicalInfestationCatalogLoader` |
| `education_curriculum.json` | 15 | `1.0.0` | `UNRESOLVED` | `SourceReference:education_curriculum.json` |
| `electrostatic_filtration_catalog.json` | 1 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ElectrostaticFiltrationCatalogLoader` |
| `emergency_alerts.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EmergencyAlertsCatalog` |
| `endings.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:endings.json` |
| `environmental_atmosphere_expansion.json` | 189 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WeatherSystem` |
| `environmental_texts_expansion_05.json` | 42 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterSystem` |
| `espionage_missions.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EspionageMissionCatalogLoader` |
| `espionage_operations.json` | 5 | `1.0.0` | `UNRESOLVED` | `SourceReference:espionage_operations.json` |
| `excavation_hazard_mitigation.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:excavation_hazard_mitigation.json` |
| `excavation_sites.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:excavation_sites.json` |
| `exercise_routines.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:exercise_routines.json` |
| `fallout_patterns.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:fallout_patterns.json` |
| `family_name_templates.json` | 3 | `1.0.0` | `UNRESOLVED` | `SourceReference:family_name_templates.json` |
| `feedback_messages.json` | 200 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FeedbackMessageCatalogLoader` |
| `field_guide.json` | 38 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FieldGuideCatalog` |
| `fischer_tropsch_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FischerTropschCatalogLoader, FischerTropschSynthesisEngine` |
| `fluid_infrastructure.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FluidInfrastructureCatalogLoader` |
| `fog_harvesting_catalog.json` | 2 | `1.0.0` | `UNRESOLVED` | `FogHarvestingCatalog` |
| `food_preservation.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FoodPreservationCatalogLoader` |
| `food_types.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FoodTypesCatalog` |
| `geodetic_survey_catalog.json` | 16 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GeodeticSurveyCatalogLoader` |
| `geothermal_drilling_depths.json` | 5 | `1.0.0` | `UNRESOLVED` | `SourceReference:geothermal_drilling_depths.json` |
| `geothermal_strata_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GeothermalStrataCatalogLoader, GeothermalOrcSystem` |
| `gpr_exploration_catalog.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GroundPenetratingRadarCatalogLoader, GroundPenetratingRadarEngine` |
| `grain_processing.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GrainProcessingCatalogLoader` |
| `heliograph.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HeliographCatalogLoader` |
| `hidden_agendas.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HiddenAgendaSystem` |
| `hobby_definitions.json` | 10 | `1.0.0` | `UNRESOLVED` | `SourceReference:hobby_definitions.json` |
| `hydraulic_extrusion_catalog.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HydraulicExtrusionCatalogLoader` |
| `infiltrator_profiles.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `InfiltratorCatalogLoader` |
| `insar_geodesy_catalog.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `InSarGeodesyCatalogLoader` |
| `interrogation_tactics.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `InterrogationTacticsCatalog` |
| `kinetic_flywheel_catalog.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `KineticFlywheelCatalogLoader` |
| `labor_camps.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LaborCampsCatalog` |
| `leadership_policies.json` | 5 | `1.0.0` | `UNRESOLVED` | `SourceReference:leadership_policies.json` |
| `ledger_debt_templates.json` | 25 | `1.0.0` | `UNRESOLVED` | `SourceReference:ledger_debt_templates.json` |
| `legacy_traits.json` | 20 | `1.0.0` | `UNRESOLVED` | `SourceReference:legacy_traits.json` |
| `life_stages.json` | 11 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LifeStagesCatalogLoader` |
| `lore_archives.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:lore_archives.json` |
| `low_background_lead_catalog.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LowBackgroundLeadCatalogLoader` |
| `lyophilization_catalog.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LyophilizationCatalogLoader` |
| `memorial_rites.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SpiritualCatalogLoader, SpiritualMeaningCoordinator` |
| `memorials_expansion_05.json` | 27 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MemorialSystem` |
| `memory_decay_rates.json` | 10 | `1.0.0` | `UNRESOLVED` | `SourceReference:memory_decay_rates.json` |
| `mental_arcs.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MentalArcCatalogLoader` |
| `merchant_caravans.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterBarterSystem` |
| `meta_unlockables.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MetaUnlockablesCatalog` |
| `metrology_standards_catalog.json` | 13 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PrecisionMetrologyCatalogLoader` |
| `microfluidic_diagnostic_catalog.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MicrofluidicDiagnosticCatalogLoader, MicrofluidicDiagnosticEngine` |
| `mine_flail_catalog.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MineFlailCatalogLoader, MineClearingFlailEngine` |
| `mineral_acid_synthesis_catalog.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:mineral_acid_synthesis_catalog.json` |
| `mod_manifest_schema.json` | 2 | `1.0.0` | `UNRESOLVED` | `SourceReference:mod_manifest_schema.json` |
| `museum_collection_templates.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:museum_collection_templates.json` |
| `mutations.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MutationCatalog` |
| `narcotics.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarcoticsCatalog` |
| `narrative_discovery_manifest.json` | 243 | `1.0.0` | `UNRESOLVED` | `SourceReference:narrative_discovery_manifest.json` |
| `narrative_encounters.json` | 16 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterCatalogLoader` |
| `narrative_encounters_expansion.json` | 29 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterSystem` |
| `narrative_encounters_npc_arcs.json` | 31 | `1.0.0` | `UNRESOLVED` | `SourceReference:narrative_encounters_npc_arcs.json` |
| `narrative_progression.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterSystem` |
| `naval_vessels.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NavalVesselCatalog` |
| `needs_performance.json` | 0 | `1.0.0` | `UNRESOLVED` | `SourceReference:needs_performance.json` |
| `night_watch_operations.json` | 67 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NightWatchOperationsCatalogLoader` |
| `noise_sources.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NoiseSourcesCatalog` |
| `npc_arcs.json` | 24 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NpcArcCatalog` |
| `npc_memory_dialogue.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NpcMemoryDialogueCatalog` |
| `nuclear_core_profiles.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:nuclear_core_profiles.json` |
| `nuclear_winter_phases.json` | 9 | `1.0.0` | `UNRESOLVED` | `SourceReference:nuclear_winter_phases.json` |
| `nutrition_profiles.json` | 17 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NutritionProfileCatalogLoader` |
| `nvis_communications_catalog.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NvisCommunicationsCatalogLoader` |
| `outposts.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `OutpostSettlementSystem` |
| `pathogens.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PathogenStrainCatalogLoader` |
| `perimeter_defenses.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PerimeterDefenseCatalogLoader` |
| `personal_belongings.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PersonalBelongingsSystem` |
| `phantom_heirlooms.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HeirloomCatalog, HeirloomSystem` |
| `phantom_triggers.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PhantomMemoryHostSession, PhantomMemoryEngine` |
| `piezometer_network_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:piezometer_network_catalog.json` |
| `plastic_pyrolysis_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PlasticPyrolysisCatalog` |
| `pneumatic_network_catalog.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PneumaticNetworkCatalogLoader, PneumaticDispatchSystem` |
| `policies.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PoliticalPoliciesCatalog` |
| `political_policies.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PoliticalPoliciesCatalog` |
| `powder_metallurgy_catalog.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PowderMetallurgyCatalogLoader` |
| `precision_broaching_catalog.json` | 5 | `1.0.0` | `UNRESOLVED` | `PrecisionBroachingCatalog` |
| `precision_optics_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PrecisionOpticsCatalogLoader` |
| `prewar_archives.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PrewarArchiveCatalogLoader` |
| `prologue_sequence.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PrologueSequenceCatalog` |
| `propaganda_campaigns.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:propaganda_campaigns.json` |
| `propaganda_templates.json` | 4 | `1.0.0` | `UNRESOLVED` | `SourceReference:propaganda_templates.json` |
| `psychological_therapies.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PsychologicalTherapyCatalogLoader` |
| `psychological_trauma.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PsychologicalTraumaCatalogLoader, SurvivorMentalHealthSystem` |
| `psychology_profiles.json` | 12 | `1.0.0` | `UNRESOLVED` | `SourceReference:psychology_profiles.json` |
| `radar_ecm_catalog.json` | 2 | `1.0.0` | `UNRESOLVED` | `RadarEcmCatalog` |
| `radiation_hotspot_policy.json` | 4 | `1.0.0` | `UNRESOLVED` | `SourceReference:radiation_hotspot_policy.json` |
| `rail_grinding_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RailGrindingCatalogLoader, RailGrindingEngine` |
| `rail_logistics_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RailLogisticsCatalogLoader, RailwaySystem` |
| `rail_network.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:rail_network.json` |
| `railway_interlock_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RailwayInterlockCatalog` |
| `rationing_protocols.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RationingProtocolCatalogLoader` |
| `recon_telemetry_probes.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:recon_telemetry_probes.json` |
| `recreation.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CultureCreationSystem` |
| `recruitment_templates.json` | 11 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RecruitmentTemplatesCatalog` |
| `regional_prices.json` | 24 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RegionalPriceCatalogLoader` |
| `regional_treaties.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RegionalTreatyCatalogLoader, RegionalTreatySystem` |
| `relationship_bands.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RelationshipBandsCatalog` |
| `relationship_decay_profiles.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:relationship_decay_profiles.json` |
| `reputation_dimensions.json` | 13 | `1.0.0` | `UNRESOLVED` | `SourceReference:reputation_dimensions.json` |
| `rerailing_equipment_catalog.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RerailingEquipmentCatalogLoader` |
| `research_knowledge.json` | 62 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ResearchKnowledgeCatalogLoader` |
| `research_unlocks.json` | 30 | `1.0.0` | `UNRESOLVED` | `SourceReference:research_unlocks.json` |
| `retention_policies.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:retention_policies.json` |
| `robotics.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RoboticsSystem` |
| `romance_courtship.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RomanceCourtshipCatalogLoader` |
| `routine_templates.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RoutineTemplateCatalogLoader` |
| `rumor_hubs.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:rumor_hubs.json` |
| `runflat_tire_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RunFlatTireCatalogLoader` |
| `salvage_teardown.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SalvageTeardownCatalog` |
| `sanitation_facilities.json` | 9 | `1.0.0` | `UNRESOLVED` | `SourceReference:sanitation_facilities.json` |
| `scavenging_tables.json` | 59 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ScavengingTableCatalog` |
| `seasonal_human_migration.json` | 4 | `1.0.0` | `UNRESOLVED` | `SourceReference:seasonal_human_migration.json` |
| `seismic_fault_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SeismicDynamicsSystem` |
| `settlements.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `OutpostSettlementSystem` |
| `skill_certifications.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SkillCertificationCatalog` |
| `skills.json` | 161 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SkillCatalogLoader` |
| `sky_defense_ordnance.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SkyDefenseOrdnanceCatalogLoader` |
| `sky_layer_armor_catalog.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SkyLayerArmorCatalogLoader` |
| `slice_seven_days.json` | 7 | `1.0.0` | `UNRESOLVED` | `SourceReference:slice_seven_days.json` |
| `solar_concentrator_catalog.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SolarConcentratorCatalogLoader` |
| `sound_ranging_catalog.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SoundRangingCatalogLoader` |
| `spiritual_rituals.json` | 19 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SpiritualCatalogLoader, SpiritualMeaningCoordinator` |
| `standing_gates.json` | 22 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StandingGateCatalogLoader` |
| `starting_supplies.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StartingLevelSystem` |
| `store_capability_claims.json` | 11 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `subterranean_zones.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SubterraneanZoneCatalogLoader` |
| `sump_drainage_catalog.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SumpDrainageCatalogLoader` |
| `supply_lines.json` | 5 | `1.0.0` | `UNRESOLVED` | `SourceReference:supply_lines.json` |
| `surgical_procedures.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurgicalProcedureCatalogLoader` |
| `tablet_manufacturing_catalog.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TabletManufacturingCatalog` |
| `tech_salvage.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TechSalvageCatalogLoader` |
| `thermal_gear.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ThermalGearCatalog` |
| `time_capsules.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TimeCapsuleSystem` |
| `toxic_chemical_catalog.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ToxicChemicalCatalogLoader` |
| `travel_encounters.json` | 57 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TravelEncounterCatalog` |
| `treaty_templates.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TreatyTemplatesCatalog` |
| `trophies.json` | 11 | `1.0.0` | `UNRESOLVED` | `SourceReference:trophies.json` |
| `underground_flora.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `UndergroundFloraCatalog` |
| `underground_tunnels.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:underground_tunnels.json` |
| `utility_actions.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `UtilityAiSystem` |
| `uv_corona_detector_catalog.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `UvCoronaDetectionCatalogLoader, UvCoronaDetectionEngine` |
| `visitor_templates.json` | 5 | `1.0.0` | `UNRESOLVED` | `SourceReference:visitor_templates.json` |
| `wall_carving_templates.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MemorialSystem` |
| `wasteland_grave_epitaphs.json` | 30 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MemorialSystem` |
| `wasteland_laws.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WastelandLawsCatalog` |
| `wasteland_religions.json` | 3 | `1.0.0` | `UNRESOLVED` | `SourceReference:wasteland_religions.json` |
| `wasteland_settlement_npcs.json` | 18 | `1.0.0` | `UNRESOLVED` | `SourceReference:wasteland_settlement_npcs.json` |
| `water_sources.json` | 11 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WaterSourcesCatalog` |
| `waystations.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WaystationCatalogLoader` |
| `wildlife_ecosystem.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WildlifeEcosystemCatalogLoader` |
| `wildlife_trapping_catalog.json` | 31 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WildlifeTrappingCatalogLoader` |
| `world_evolution_seeds.json` | 118 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EvolvingWorldCatalog` |
| `world_history.json` | 79 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EvolvingWorldCatalog` |
| `year_two_chapter.json` | 2 | `1.0.0` | `UNRESOLVED` | `SourceReference:year_two_chapter.json` |

### Crafting & Relics (7 Catalogs, 228 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `glassworks_recipes.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:glassworks_recipes.json` |
| `library_manuals.json` | 24 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LibraryManualCatalogLoader` |
| `metallurgy_recipes.json` | 12 | `1.0.0` | `UNRESOLVED` | `SourceReference:metallurgy_recipes.json` |
| `recipes.json` | 123 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RecipeCatalogLoader` |
| `recipes_cooking.json` | 15 | `1.0.0` | `UNRESOLVED` | `SourceReference:recipes_cooking.json` |
| `relic_recipes.json` | 39 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RelicCatalogLoader` |
| `workshop_recipes.json` | 13 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:workshop_recipes.json` |

### Crossing (Exp 04) (1 Catalogs, 37 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_encounters.json` | 37 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |

### Documents & History (1 Catalogs, 0 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `documents/vel_triage_log_names.json` | 0 | `1.0.0` | `OPTIONAL` | `NarrativeBatchCatalog` |

### Duty Roster (Exp 02) (2 Catalogs, 51 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `duty_roster_marks.json` | 43 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterCatalog` |
| `duty_roster_seasons.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterCatalog` |

### Economy & Trade (10 Catalogs, 168 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `black_market_inventory.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BlackMarketInventoryCatalogLoader` |
| `caravan_trade_routes.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CaravanTradeRouteCatalogLoader` |
| `economy_goods.json` | 51 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GoodsCatalog` |
| `hardcore_economy_tuning.json` | 22 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HardcoreEconomyTuningLoader` |
| `radiation_economy_social.json` | 17 | `1.0.0` | `UNRESOLVED` | `SourceReference:radiation_economy_social.json` |
| `trade_embargoes.json` | 14 | `1.0.0` | `UNRESOLVED` | `SourceReference:trade_embargoes.json` |
| `trade_screen_scenarios.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TradeScreenScenarios` |
| `trade_specialties.json` | 16 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TradeSpecialtySystem` |
| `trade_tell_lines.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TradeTellEngine` |
| `trade_texts.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TradeScreenPresenter` |

### Events (11 Catalogs, 400 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `contagion_events.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ContagionEventCatalogLoader` |
| `desperation_events.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:desperation_events.json` |
| `events.json` | 240 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EventsHostSession` |
| `ideological_events.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:ideological_events.json` |
| `incidents.json` | 25 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterEncounterSystem` |
| `narrative_arc_events.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeArcEventSystem` |
| `orbital_harrow_events.json` | 12 | `1.0.0` | `UNRESOLVED` | `SourceReference:orbital_harrow_events.json` |
| `seasonal_events.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SeasonalEventCatalogLoader` |
| `shelter_social_events.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:shelter_social_events.json` |
| `world_evolution_events.json` | 13 | `1.0.0` | `UNRESOLVED` | `SourceReference:world_evolution_events.json` |
| `year_of_ash_events.json` | 52 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Expeditions & Vehicles (5 Catalogs, 117 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `expeditions.json` | 75 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpeditionCatalogLoader` |
| `vehicle_armor_grades.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VehicleArmorGradeCatalogLoader, VehicleGarageSystem` |
| `vehicle_modifications.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VehicleGarageCatalogLoader, VehicleGarageSystem` |
| `vehicle_modules.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VehicleModuleCatalogLoader` |
| `vehicles.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpeditionVehicleSystem` |

### Factions (20 Catalogs, 416 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_factions.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |
| `faction_combat_thresholds.json` | 6 | `1.0.0` | `UNRESOLVED` | `FactionCombatThresholdCatalog` |
| `faction_intelligence.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionIntelligenceCatalogLoader, ShelterEspionageSystem` |
| `faction_lore.json` | 47 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionIconCatalog, FactionIconLoader` |
| `faction_radio_corpus.json` | 35 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_territory.json` | 24 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionTerritoryCatalog` |
| `faction_war_communiques.json` | 40 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_dialogue.json` | 40 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_events.json` | 38 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_journal.json` | 26 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_radio.json` | 33 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `foundry_faction.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalogLoader` |
| `holdfast_factions.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastFactionsCatalog` |
| `independent_faction_branch.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `IndependentBranchCatalog` |
| `military_faction_branch.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MilitaryBranchCatalog` |
| `moral_choice_faction_reactions.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceFactionReactionsCatalogLoader` |
| `muster_faction_actions.json` | 12 | `1.0.0` | `UNRESOLVED` | `SourceReference:muster_faction_actions.json` |
| `muster_faction_culture.json` | 25 | `1.0.0` | `UNRESOLVED` | `SourceReference:muster_faction_culture.json` |
| `rebel_faction_branch.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RebelBranchCatalog` |
| `standing_record_factions.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StandingRecordCatalog` |

### Foundry & Industry (4 Catalogs, 75 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `cupola_foundry_catalog.json` | 7 | `1.0.0` | `UNRESOLVED` | `CupolaFoundryCatalog` |
| `foundry_accords.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalog, SilentFoundryCatalogLoader` |
| `foundry_production.json` | 35 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalogLoader` |
| `foundry_treaty_consequences.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryConsequencePolicy` |

### Greenhouse & Biology (2 Catalogs, 27 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crop_strains.json` | 17 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CropStrainCatalogLoader` |
| `hydroponic_crops.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HydroponicCropCatalogLoader, HydroponicBiomeSystem` |

### Holdfast (Exp 01) (2 Catalogs, 10 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `holdfast_flavor.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastFlavorCatalog` |
| `holdfast_npcs.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastNpcCatalogLoader` |

### Items (14 Catalogs, 1379 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `agriculture_items.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:agriculture_items.json` |
| `black_flotilla_items.json` | 36 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ProceduralScavengeSystem` |
| `chemical_dependency_items.json` | 13 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ChemicalDependencySystem` |
| `crossing_items.json` | 25 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |
| `dose_items.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseContentCatalog` |
| `expansion_item_tags.json` | 115 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ItemCatalogLoader` |
| `foundry_items.json` | 30 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalog` |
| `greenhouse_items.json` | 34 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GreenhouseExpansionCatalog` |
| `holdfast_items.json` | 55 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastItemsCatalog` |
| `item_degradation.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ItemDegradationCatalog` |
| `item_description_texts.json` | 183 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ItemDescriptionCatalogLoader, ItemCatalogLoader` |
| `items.json` | 789 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ItemCatalogLoader, LoadItems` |
| `verdict_items.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:verdict_items.json` |
| `year_of_ash_items.json` | 62 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Journal & Logs (3 Catalogs, 91 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `codex_entries.json` | 63 | `1.0.0` | `UNRESOLVED` | `SourceReference:codex_entries.json` |
| `journal_entries_expansion_05.json` | 28 | `1.0.0` | `GAMEPLAY_CONSUMED` | `JournalCorpusCatalogLoader` |
| `journal_voice_prose.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `JournalVoiceProseCatalog` |

### Locations & Map (15 Catalogs, 555 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_locations.json` | 13 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |
| `damaged_map_zones.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WastelandMapSystem` |
| `deep_lore_locations.json` | 25 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DeepLoreLocationCatalogLoader` |
| `dose_locations.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseContentCatalog` |
| `duty_roster_locations.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterCatalog` |
| `faction_war_location_overrides.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `holdfast_locations.json` | 38 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastCatalog` |
| `locations.json` | 179 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationLayoutSystem, WastelandMapCatalogLoader` |
| `locations_expansion3.json` | 21 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationLayoutSystem` |
| `map_regions.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:map_regions.json` |
| `micro_locations.json` | 28 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MicroLocationEncounterLoader` |
| `verdict_locations.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:verdict_locations.json` |
| `wasteland_map_v1.json` | 91 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WastelandMapCatalogLoader` |
| `weather_route_gate_locations.json` | 11 | `1.0.0` | `UNRESOLVED` | `SourceReference:weather_route_gate_locations.json` |
| `year_of_ash_locations.json` | 66 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Maritime & Deep Lore (1 Catalogs, 14 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `dive_sites.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DiveSiteCatalog` |

### Medical & Health (6 Catalogs, 189 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `autopsy_procedures.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AutopsyProcedureCatalogLoader` |
| `disease_catalog.json` | 29 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DiseaseCatalog, DiseaseSystem` |
| `dose_registers.json` | 31 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseRegistersCatalog` |
| `medical_record_templates.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MedicalRecordTemplatesCatalog` |
| `medical_texts.json` | 83 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MedicalWardSystem` |
| `pharma_recipes.json` | 27 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PharmaRecipeCatalogLoader` |

### Moral Choice (3 Catalogs, 117 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `moral_choice_chains.json` | 92 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceChainCatalogLoader` |
| `moral_choice_flags.json` | 25 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceFlagCatalogLoader` |
| `moral_choice_gossip.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceGossipCatalogLoader` |

### Muster & Epilogue (7 Catalogs, 102 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `campaign_epilogues.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CampaignEpilogueCatalogLoader` |
| `currents.json` | 17 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CurrentsCatalog` |
| `epilogue_chronicle.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EpilogueMatrix` |
| `epilogue_personalization.json` | 0 | `1.0.0` | `UNRESOLVED` | `SourceReference:epilogue_personalization.json` |
| `muster_camp_scenes.json` | 4 | `1.0.0` | `UNRESOLVED` | `SourceReference:muster_camp_scenes.json` |
| `muster_epilogues.json` | 25 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EpilogueMatrix` |
| `muster_witnesses.json` | 27 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WitnessCatalog` |

### Narrative (Codex) (280 Catalogs, 3620 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `narrative/activated_carbon_adsorption_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:activated_carbon_adsorption_records.json` |
| `narrative/ammo_hoist_jam_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:ammo_hoist_jam_reports.json` |
| `narrative/ammonia_chiller_leak_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:ammonia_chiller_leak_logs.json` |
| `narrative/annealing_lehr_birefringence_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:annealing_lehr_birefringence_records.json` |
| `narrative/antler_horn_sawing_records.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BoneHornSourceAdapter` |
| `narrative/apiculture_red_light_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:apiculture_red_light_audits.json` |
| `narrative/aramid_fiber_rot_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:aramid_fiber_rot_reports.json` |
| `narrative/architect_vault_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:architect_vault_audits.json` |
| `narrative/armored_cockroach_hive_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:armored_cockroach_hive_logs.json` |
| `narrative/armored_locomotive_manifests.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:armored_locomotive_manifests.json` |
| `narrative/artesian_well_contamination_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:artesian_well_contamination_logs.json` |
| `narrative/awl_saddle_stitch_journals.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:awl_saddle_stitch_journals.json` |
| `narrative/bark_tanning_vat_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bark_tanning_vat_logs.json` |
| `narrative/beeswax_clarification_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:beeswax_clarification_records.json` |
| `narrative/beeswax_rendering_dipping_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:beeswax_rendering_dipping_assays.json` |
| `narrative/biochar_cation_exchange_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:biochar_cation_exchange_reports.json` |
| `narrative/bisque_firing_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bisque_firing_records.json` |
| `narrative/blast_gate_mechanical_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:blast_gate_mechanical_audits.json` |
| `narrative/blind_cave_molerat_studies.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:blind_cave_molerat_studies.json` |
| `narrative/boiler_feedwater_deaerator_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:boiler_feedwater_deaerator_audits.json` |
| `narrative/bolting_silk_mesh_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bolting_silk_mesh_reports.json` |
| `narrative/bone_degreasing_prep_logs.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BoneHornSourceAdapter` |
| `narrative/borosilicate_sight_glass_thermal_shock.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:borosilicate_sight_glass_thermal_shock.json` |
| `narrative/brain_tanning_hide_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:brain_tanning_hide_reports.json` |
| `narrative/brewers_yeast_krausen_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:brewers_yeast_krausen_audits.json` |
| `narrative/brine_pickling_barrel_spoilage.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:brine_pickling_barrel_spoilage.json` |
| `narrative/bullet_alloy_assay_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bullet_alloy_assay_reports.json` |
| `narrative/bunker_blueprints_codex.json` | 24 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bunker_blueprints_codex.json` |
| `narrative/bunker_bureaucratic_anomalies.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_children_folklore.json` | 19 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bunker_children_folklore.json` |
| `narrative/bunker_children_folklore_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bunker_children_folklore_batch_2.json` |
| `narrative/bunker_contraband_barter.json` | 20 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bunker_contraband_barter.json` |
| `narrative/bunker_court_verdicts_batch_2.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_court_verdicts_codex.json` | 24 | `1.0.0` | `CODEX_ONLY` | `BunkerCourtCatalog` |
| `narrative/bunker_graffiti_postings.json` | 36 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BunkerGraffitiCatalog` |
| `narrative/bunker_herbalism_pharmacology.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bunker_herbalism_pharmacology.json` |
| `narrative/bunker_maintenance_glitches.json` | 20 | `1.0.0` | `CODEX_ONLY` | `BunkerMaintenanceCatalog` |
| `narrative/bunker_maintenance_logs_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_maintenance_logs_batch_3.json` | 25 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_rituals_and_cults.json` | 5 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_shift_schedules_and_notices.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_trade_ledger_batch_2.json` | 20 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bunker_trade_ledger_batch_2.json` |
| `narrative/bunker_wiretap_transcripts.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:bunker_wiretap_transcripts.json` |
| `narrative/bunker_wiretap_transcripts_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bureaucratic_document_runtime_map.json` | 27 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BureaucraticDocumentCatalogLoader` |
| `narrative/bureaucratic_documents_expansion.json` | 27 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BureaucraticDocumentCatalogLoader` |
| `narrative/burr_millstone_dressing_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:burr_millstone_dressing_logs.json` |
| `narrative/calcium_hypochlorite_titration_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:calcium_hypochlorite_titration_reports.json` |
| `narrative/candle_dip_mould_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:candle_dip_mould_assays.json` |
| `narrative/canyon_mudflow_hazard_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:canyon_mudflow_hazard_reports.json` |
| `narrative/carbide_tool_wear_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:carbide_tool_wear_audits.json` |
| `narrative/carrion_vulture_sighting_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:carrion_vulture_sighting_logs.json` |
| `narrative/cave_aquatic_biota_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:cave_aquatic_biota_logs.json` |
| `narrative/celluloid_film_decomposition_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:celluloid_film_decomposition_records.json` |
| `narrative/charcoal_mound_pyrolysis_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:charcoal_mound_pyrolysis_logs.json` |
| `narrative/chef_recipe_development.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/chemist_lab_notes_batch_1.json` | 1 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/childrens_artwork_batch_2.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/childrens_folklore_expansion.json` | 31 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/chrome_alum_tanning_assays.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:chrome_alum_tanning_assays.json` |
| `narrative/clay_wedging_forming_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:clay_wedging_forming_logs.json` |
| `narrative/cobalt_arming_directives.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:cobalt_arming_directives.json` |
| `narrative/cobalt_liturgies.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FringeCultSourceAdapter` |
| `narrative/cobalt_liturgies_batch_2.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cold_process_soap_curing_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:cold_process_soap_curing_reports.json` |
| `narrative/conflict_mediation_records.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/council_meeting_minutes.json` | 32 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/courier_dispatches_master.json` | 30 | `1.0.0` | `CODEX_ONLY` | `SourceReference:courier_dispatches_master.json` |
| `narrative/courier_mission_logs.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/courier_mission_logs_batch_2.json` | 1 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/crater_lake_limnology_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:crater_lake_limnology_records.json` |
| `narrative/crop_experiment_logs.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/crop_genome_degradation_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:crop_genome_degradation_reports.json` |
| `narrative/crucible_clay_pot_slag_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:crucible_clay_pot_slag_logs.json` |
| `narrative/cryo_germplasm_viability_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:cryo_germplasm_viability_audits.json` |
| `narrative/cryo_seed_ampoule_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:cryo_seed_ampoule_logs.json` |
| `narrative/cryopod_failure_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `AbyssalAnomaliesCatalog` |
| `narrative/culinary_ration_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/culinary_ration_codex.json` | 30 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cupola_melting_ratio_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:cupola_melting_ratio_audits.json` |
| `narrative/cupola_slag_leaching_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:cupola_slag_leaching_records.json` |
| `narrative/currents_pamphlets.json` | 16 | `1.0.0` | `CODEX_ONLY` | `CurrentsPamphletCatalog` |
| `narrative/currying_burnishing_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:currying_burnishing_assays.json` |
| `narrative/dead_hand_directives.json` | 20 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DeadHandDirectiveCatalog` |
| `narrative/deadbeat_escapement_wear_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:deadbeat_escapement_wear_logs.json` |
| `narrative/deckle_mould_watermark_audits.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/deep_lore_texts.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/diplomatic_contact_records_batch_1.json` | 5 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/documents_batch_1.json` | 5 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/documents_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/documents_batch_3.json` | 33 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/drone_carrier_blackboxes.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:drone_carrier_blackboxes.json` |
| `narrative/drop_spindle_fibre_drafting_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:drop_spindle_fibre_drafting_logs.json` |
| `narrative/dweller_dependency_backstories.json` | 6 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/dweller_heirlooms_master.json` | 30 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/dweller_medical_casebook.json` | 40 | `1.0.0` | `CODEX_ONLY` | `SourceReference:dweller_medical_casebook.json` |
| `narrative/dweller_psychological_journals.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:dweller_psychological_journals.json` |
| `narrative/education_session_records.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/emp_atmospheric_sniffer_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:emp_atmospheric_sniffer_logs.json` |
| `narrative/engineering_logs_expansion.json` | 32 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/engineering_mod_notes.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/equipment_failure_logs.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/eulogy_corpus_batch_1.json` | 4 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_briefs_expansion.json` | 30 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_field_reports.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_field_reports_batch_2.json` | 1 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_planning_briefs_batch_1.json` | 1 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/faction_directives_and_notices.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/faction_field_documents.json` | 12 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/faction_texts_expansion.json` | 29 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/fallout_sensory_loss_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:fallout_sensory_loss_records.json` |
| `narrative/fermentation_crock_airlock_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:fermentation_crock_airlock_assays.json` |
| `narrative/fibre_heckling_prep_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:fibre_heckling_prep_logs.json` |
| `narrative/field_reports_expansion.json` | 39 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/forge_charcoal_ash_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:forge_charcoal_ash_assays.json` |
| `narrative/found_objects_expansion.json` | 40 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/fulling_trough_nap_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:fulling_trough_nap_assays.json` |
| `narrative/gear_quenching_fault_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:gear_quenching_fault_logs.json` |
| `narrative/geological_strata_logs.json` | 24 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/geophone_hymnals.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FringeCultSourceAdapter` |
| `narrative/geothermal_borehole_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `AbyssalAnomaliesCatalog` |
| `narrative/geothermal_steam_vent_diagnostics.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:geothermal_steam_vent_diagnostics.json` |
| `narrative/geothermal_steam_well_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:geothermal_steam_well_logs.json` |
| `narrative/ghost_transmissions.json` | 12 | `1.0.0` | `CODEX_ONLY` | `GhostTransmissionCatalog` |
| `narrative/graffiti_expansion.json` | 41 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BunkerGraffitiCatalog` |
| `narrative/grain_silo_weevil_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:grain_silo_weevil_audits.json` |
| `narrative/green_sand_bentonite_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:green_sand_bentonite_assays.json` |
| `narrative/greenhouse_cultivation_logs.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ground_glass_joint_greasing_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:ground_glass_joint_greasing_audits.json` |
| `narrative/heirloom_seed_viability_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:heirloom_seed_viability_reports.json` |
| `narrative/hemp_fiber_hackling_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:hemp_fiber_hackling_logs.json` |
| `narrative/hollander_beater_pulping_logs.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/honey_extractor_balance_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:honey_extractor_balance_reports.json` |
| `narrative/hydrophone_acoustic_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `AbyssalAnomaliesCatalog` |
| `narrative/improvised_repair_guides_batch_2.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/inkle_loom_warp_tally_sheets.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:inkle_loom_warp_tally_sheets.json` |
| `narrative/intake_filter_clogging_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:intake_filter_clogging_logs.json` |
| `narrative/invar_pendulum_thermal_expansion.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:invar_pendulum_thermal_expansion.json` |
| `narrative/iron_gall_ink_acidity_reports.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/iron_synod_canons.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FringeCultSourceAdapter` |
| `narrative/journal_entries_batch_1.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `JournalCorpusCatalogLoader` |
| `narrative/journal_entries_batch_2.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `JournalCorpusCatalogLoader` |
| `narrative/journal_entries_batch_3.json` | 88 | `1.0.0` | `GAMEPLAY_CONSUMED` | `JournalCorpusCatalogLoader` |
| `narrative/journals_expansion.json` | 40 | `1.0.0` | `GAMEPLAY_CONSUMED` | `JournalCorpusCatalogLoader` |
| `narrative/jrnl_templates_cycle_c.json` | 6 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/jrnl_templates_cycle_d.json` | 4 | `1.0.0` | `CODEX_ONLY` | `SourceReference:jrnl_templates_cycle_d.json` |
| `narrative/kiln_draw_trial_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:kiln_draw_trial_assays.json` |
| `narrative/langstroth_hive_foundation_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:langstroth_hive_foundation_logs.json` |
| `narrative/lead_crystal_scintillator_aging_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:lead_crystal_scintillator_aging_logs.json` |
| `narrative/lead_wall_degradation_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:lead_wall_degradation_logs.json` |
| `narrative/leather_harness_conditioning_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:leather_harness_conditioning_audits.json` |
| `narrative/letters_expansion.json` | 25 | `1.0.0` | `CODEX_ONLY` | `PersonalLetterCatalog` |
| `narrative/liebig_condenser_fracture_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:liebig_condenser_fracture_logs.json` |
| `narrative/lime_kiln_calcination_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:lime_kiln_calcination_logs.json` |
| `narrative/liquid_nitrogen_compressor_failures.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:liquid_nitrogen_compressor_failures.json` |
| `narrative/load_shed_schedule_001.json` | 6 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/lost_tech_manuals.json` | 24 | `1.0.0` | `CODEX_ONLY` | `LostTechManualCatalog` |
| `narrative/mainspring_fatigue_rupture_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:mainspring_fatigue_rupture_audits.json` |
| `narrative/manila_hawser_breakage_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:manila_hawser_breakage_reports.json` |
| `narrative/medical_documents_expansion.json` | 36 | `1.0.0` | `CODEX_ONLY` | `SourceReference:medical_documents_expansion.json` |
| `narrative/memorials_expansion.json` | 40 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/mill_dampener_tempering_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:mill_dampener_tempering_assays.json` |
| `narrative/mortise_tenon_failure_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:mortise_tenon_failure_reports.json` |
| `narrative/mudbrick_weathering_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:mudbrick_weathering_assays.json` |
| `narrative/munitions_leaching_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:munitions_leaching_records.json` |
| `narrative/mutated_botanical_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:mutated_botanical_logs.json` |
| `narrative/needle_awl_hook_assays.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BoneHornSourceAdapter` |
| `narrative/neoprene_gasket_degradation_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:neoprene_gasket_degradation_logs.json` |
| `narrative/new_arrival_intake_interviews.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/night_watch_expansion.json` | 35 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/night_watch_logbook.json` | 15 | `1.0.0` | `CODEX_ONLY` | `SourceReference:night_watch_logbook.json` |
| `narrative/numbers_station_ciphers.json` | 11 | `1.0.0` | `CODEX_ONLY` | `SourceReference:numbers_station_ciphers.json` |
| `narrative/oak_bark_tanning_pit_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:oak_bark_tanning_pit_logs.json` |
| `narrative/operating_theater_surgical_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:operating_theater_surgical_logs.json` |
| `narrative/optical_coating_rad_browning_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:optical_coating_rad_browning_reports.json` |
| `narrative/oral_lore_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `SourceReference:oral_lore_batch_2.json` |
| `narrative/oral_lore_codex.json` | 16 | `1.0.0` | `CODEX_ONLY` | `SourceReference:oral_lore_codex.json` |
| `narrative/orbital_kinetic_telemetry.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:orbital_kinetic_telemetry.json` |
| `narrative/ozone_contact_tower_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:ozone_contact_tower_audits.json` |
| `narrative/patrol_debriefs.json` | 36 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pattern_maker_shrinkage_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:pattern_maker_shrinkage_records.json` |
| `narrative/periscope_prism_delamination_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:periscope_prism_delamination_logs.json` |
| `narrative/permafrost_methane_eruption_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:permafrost_methane_eruption_logs.json` |
| `narrative/personal_effects_inventory_batch_2.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pipeline_sabotage_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:pipeline_sabotage_records.json` |
| `narrative/plan17_discoverable_documents.json` | 18 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pneumatic_carrier_capsule_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:pneumatic_carrier_capsule_logs.json` |
| `narrative/pneumatic_cylinder_leather_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:pneumatic_cylinder_leather_assays.json` |
| `narrative/pneumatic_tube_diverter_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:pneumatic_tube_diverter_audits.json` |
| `narrative/pot_furnace_glass_melts.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:pot_furnace_glass_melts.json` |
| `narrative/power_grid_management_logs.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pozzolan_mortar_formulations.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:pozzolan_mortar_formulations.json` |
| `narrative/quest_narrative_documents.json` | 12 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rad_pathology_autopsy_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:rad_pathology_autopsy_records.json` |
| `narrative/radiation_survey_readings_batch_2.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_broadcast_rundowns.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_mysteries_expansion.json` | 3 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_scriptbook.json` | 15 | `1.0.0` | `CODEX_ONLY` | `RadioScriptbookCatalog` |
| `narrative/radio_scripts_expansion.json` | 36 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_transcripts_batch_2.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_transcripts_batch_3.json` | 27 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rag_pulp_beater_records.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/ragdoll_germination_assays.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:ragdoll_germination_assays.json` |
| `narrative/ration_fraud_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:ration_fraud_records.json` |
| `narrative/ration_records_expansion.json` | 36 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rawhide_bating_failure_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:rawhide_bating_failure_reports.json` |
| `narrative/refractory_firebrick_spalling_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:refractory_firebrick_spalling_logs.json` |
| `narrative/regional_treaty_protocols.json` | 16 | `1.0.0` | `CODEX_ONLY` | `SourceReference:regional_treaty_protocols.json` |
| `narrative/relic_provenance_dossiers.json` | 32 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/retort_wood_vinegar_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:retort_wood_vinegar_audits.json` |
| `narrative/root_cellar_humidity_rot_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:root_cellar_humidity_rot_reports.json` |
| `narrative/rootes_blower_vacuum_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:rootes_blower_vacuum_reports.json` |
| `narrative/rope_break_load_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:rope_break_load_assays.json` |
| `narrative/rope_transmission_splicing_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:rope_transmission_splicing_audits.json` |
| `narrative/salt_mine_inscriptions.json` | 7 | `1.0.0` | `CODEX_ONLY` | `AbyssalAnomaliesCatalog` |
| `narrative/scavenger_expedition_route_notes.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:scavenger_expedition_route_notes.json` |
| `narrative/scraping_polishing_reports.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `BoneHornSourceAdapter` |
| `narrative/screw_press_felt_reports.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/security_incident_reports_batch_2.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/seismic_array_fault_alarms.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:seismic_array_fault_alarms.json` |
| `narrative/shelter_notices_expansion.json` | 26 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/shelter_songs_expansion.json` | 31 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/silage_lactic_pit_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:silage_lactic_pit_reports.json` |
| `narrative/silica_gel_seed_desiccation_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:silica_gel_seed_desiccation_audits.json` |
| `narrative/silo_mosquito_vector_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:silo_mosquito_vector_records.json` |
| `narrative/slip_glaze_formulation_notes.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:slip_glaze_formulation_notes.json` |
| `narrative/slow_sand_schmutzdecke_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:slow_sand_schmutzdecke_logs.json` |
| `narrative/smoked_meat_creosote_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:smoked_meat_creosote_assays.json` |
| `narrative/sonar_array_fault_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:sonar_array_fault_logs.json` |
| `narrative/sourdough_mother_acidity_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:sourdough_mother_acidity_logs.json` |
| `narrative/square_set_shoring_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:square_set_shoring_audits.json` |
| `narrative/stalactite_mineral_assay_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:stalactite_mineral_assay_reports.json` |
| `narrative/steam_trap_water_hammer_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:steam_trap_water_hammer_logs.json` |
| `narrative/stencil_propaganda_smear_logs.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/strand_twisting_lay_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:strand_twisting_lay_reports.json` |
| `narrative/substation_transformer_fires.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:substation_transformer_fires.json` |
| `narrative/sump_drainage_silt_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:sump_drainage_silt_reports.json` |
| `narrative/supply_audit_records.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/supply_audit_records_batch_2.json` | 34 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/surface_dragline_ruins.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:surface_dragline_ruins.json` |
| `narrative/surface_radiation_topo_sheets.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:surface_radiation_topo_sheets.json` |
| `narrative/surgeons_casebook_batch_2.json` | 20 | `1.0.0` | `CODEX_ONLY` | `SourceReference:surgeons_casebook_batch_2.json` |
| `narrative/survivor_letters_lost_kin.json` | 25 | `1.0.0` | `CODEX_ONLY` | `SourceReference:survivor_letters_lost_kin.json` |
| `narrative/survivor_profiles_expansion.json` | 40 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/sweet_water_glycerin_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:sweet_water_glycerin_assays.json` |
| `narrative/tallow_rendering_vat_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:tallow_rendering_vat_logs.json` |
| `narrative/tallow_saponification_kettle_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:tallow_saponification_kettle_audits.json` |
| `narrative/therapist_session_notes.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/therapist_session_notes_batch_2.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/therapist_session_notes_batch_3.json` | 1 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/three_strand_rope_closing_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:three_strand_rope_closing_logs.json` |
| `narrative/timber_creosote_treatment_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:timber_creosote_treatment_logs.json` |
| `narrative/timber_dry_rot_fruiting_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:timber_dry_rot_fruiting_records.json` |
| `narrative/tire_retreading_compound_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:tire_retreading_compound_logs.json` |
| `narrative/trade_ledgers_expansion.json` | 23 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/treadle_loom_heddle_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:treadle_loom_heddle_reports.json` |
| `narrative/tub_sizing_gelatin_assays.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/turbine_blade_erosion_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:turbine_blade_erosion_reports.json` |
| `narrative/typographic_lead_wear_logs.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PaperPrintSourceAdapter` |
| `narrative/underground_fungi_flora.json` | 24 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/undertaker_burial_records.json` | 25 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/unsent_letters_batch_2.json` | 1 | `1.0.0` | `CODEX_ONLY` | `PersonalLetterCatalog` |
| `narrative/vault_seal_breach_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:vault_seal_breach_logs.json` |
| `narrative/vinyl_record_archive.json` | 30 | `1.0.0` | `CODEX_ONLY` | `SourceReference:vinyl_record_archive.json` |
| `narrative/wasteland_expeditions_master.json` | 30 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_grave_epitaphs.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FringeCultSourceAdapter` |
| `narrative/wasteland_grave_epitaphs_batch_2.json` | 12 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_settlement_gazetteer.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_trade_caravan_routes.json` | 18 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_wildlife_bestiary.json` | 24 | `1.0.0` | `CODEX_ONLY` | `SourceReference:wasteland_wildlife_bestiary.json` |
| `narrative/water_clock_orifice_silt_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:water_clock_orifice_silt_records.json` |
| `narrative/water_quality_test_reports_batch_2.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/weather_almanac_expansion.json` | 30 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wick_braiding_priming_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `SourceReference:wick_braiding_priming_reports.json` |
| `narrative/wildlife_field_encounter_logs.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wire_confessions.json` | 30 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WireConfessionCatalog` |
| `narrative/wire_rope_stranding_assays.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:wire_rope_stranding_assays.json` |
| `narrative/wood_ash_lye_hydrometer_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `SourceReference:wood_ash_lye_hydrometer_logs.json` |
| `narrative/world_history_expansion.json` | 39 | `1.0.0` | `CODEX_ONLY` | `Core default` |

### Quests (27 Catalogs, 1573 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_quests.json` | 23 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingQuestSystem` |
| `dose_quests.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseContentCatalog` |
| `duty_roster_quests.json` | 28 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterQuestRuntime` |
| `dynamic_quest_templates.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DynamicQuestTemplateCatalogLoader` |
| `dynamic_questlines.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `QuestlineSystem` |
| `holdfast_quests.json` | 24 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastQuestSystem` |
| `moral_choice_quests.json` | 68 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceCatalogLoader` |
| `moral_choice_quests_branching.json` | 100 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceBranchQuestCatalogLoader` |
| `moral_choice_quests_distress.json` | 12 | `1.0.0` | `UNRESOLVED` | `SourceReference:moral_choice_quests_distress.json` |
| `moral_choice_quests_expansion.json` | 50 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceExpansionQuestCatalogLoader` |
| `narrative_questlines.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterSystem` |
| `personal_quests.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PersonalQuestSystem` |
| `quest_templates.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `QuestTemplateCatalogLoader` |
| `questline_master.json` | 511 | `1.0.0` | `GAMEPLAY_CONSUMED` | `QuestlineMasterCatalog` |
| `quests_bureaucratic_morality.json` | 2 | `1.0.0` | `UNRESOLVED` | `SourceReference:quests_bureaucratic_morality.json` |
| `quests_expansion_05.json` | 37 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpansionQuestSystem` |
| `quests_expansion_06.json` | 19 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpansionQuestSystem` |
| `quests_faction_branching.json` | 200 | `1.0.0` | `UNRESOLVED` | `SourceReference:quests_faction_branching.json` |
| `quests_massive_expansion_200.json` | 200 | `1.0.0` | `UNRESOLVED` | `SourceReference:quests_massive_expansion_200.json` |
| `quests_moral_branching_expansion.json` | 30 | `1.0.0` | `UNRESOLVED` | `SourceReference:quests_moral_branching_expansion.json` |
| `quests_npc_arcs.json` | 40 | `1.0.0` | `UNRESOLVED` | `SourceReference:quests_npc_arcs.json` |
| `repeatable_quests.json` | 6 | `1.0.0` | `UNRESOLVED` | `SourceReference:repeatable_quests.json` |
| `standing_record_quests.json` | 32 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StandingRecordCatalog` |
| `thirdonary_quests.json` | 75 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ThirdonaryCatalogLoader` |
| `verdict_questlines.json` | 23 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VerdictQuestCatalogLoader` |
| `year_of_ash_questlines.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |
| `year_of_ash_quests.json` | 32 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Radio & Signals (8 Catalogs, 239 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `radio.json` | 87 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RadioHostSession, RadioScriptbookCatalog` |
| `radio_distress_signals.json` | 25 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SignalTriangulationSystem` |
| `radio_distress_signals_expansion.json` | 23 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SignalTriangulationSystem` |
| `radio_intercepts.json` | 16 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:radio_intercepts.json` |
| `radio_programs.json` | 2 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RadioProgramProductionSystem` |
| `radio_stations.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RadioStationCatalogLoader` |
| `verdict_radio.json` | 30 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VerdictRadioSystem` |
| `year_of_ash_radio.json` | 50 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Shelter & Power (16 Catalogs, 282 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `power_grid.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PowerGridSystem` |
| `power_subgrid_nodes.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SourceReference:power_subgrid_nodes.json` |
| `shelter_audio_cues.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterAudioCueCatalogLoader, ShelterAcousticDirector` |
| `shelter_celebrations.json` | 11 | `1.0.0` | `UNRESOLVED` | `SourceReference:shelter_celebrations.json` |
| `shelter_components.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterComponentCatalogLoader` |
| `shelter_construction.json` | 10 | `1.0.0` | `UNRESOLVED` | `SourceReference:shelter_construction.json` |
| `shelter_governance_blocs.json` | 4 | `1.0.0` | `UNRESOLVED` | `SourceReference:shelter_governance_blocs.json` |
| `shelter_insulation_catalog.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterThermalSystem` |
| `shelter_machine_identities.json` | 38 | `1.0.0` | `UNRESOLVED` | `SourceReference:shelter_machine_identities.json` |
| `shelter_origins.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterOriginCatalogLoader` |
| `shelter_room_identities.json` | 87 | `1.0.0` | `UNRESOLVED` | `SourceReference:shelter_room_identities.json` |
| `shelter_rooms.json` | 35 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterRoomIdentityCatalog` |
| `shelter_schedules.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterScheduleCatalogLoader` |
| `shelter_security_zones.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:shelter_security_zones.json` |
| `shelter_shielding.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterShieldingCatalog` |
| `sofc_power_catalog.json` | 11 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SofcPowerCatalogLoader` |

### Social & Psychology (3 Catalogs, 130 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `confession_secrets.json` | 38 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ConfessionSecrets` |
| `final_wishes.json` | 52 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FinalWishSystem` |
| `guilt_sources.json` | 40 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GuiltInsomniaSystem` |

### Standing Record (Exp 03) (2 Catalogs, 66 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `standing_record_layouts.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationLayoutSystem` |
| `standing_record_memory.json` | 52 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationMemorySystem` |

### Survivors (10 Catalogs, 362 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `antigravity_survivor_fields.json` | 11 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalog` |
| `characters.json` | 84 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalog` |
| `deep_lore_survivor_fields.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalog` |
| `expansion_survivor_fields.json` | 73 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalog` |
| `starting_survivor_cohorts.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StartingCohortCatalogLoader` |
| `starting_survivors.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorStartingStateLoader` |
| `survivor_roles.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorRolesCatalog` |
| `survivor_voice_lines.json` | 8 | `1.0.0` | `UNRESOLVED` | `SourceReference:survivor_voice_lines.json` |
| `survivors.json` | 129 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalogLoader, SurvivorCatalog` |
| `year_of_ash_survivors.json` | 36 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Verdict (Exp 03) (2 Catalogs, 41 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `verdict_data.json` | 23 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VerdictCatalogLoader` |
| `verdict_npcs.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VerdictNpcSystem` |

### Weather & Environment (6 Catalogs, 77 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `weather_effects.json` | 22 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WeatherEffectsCatalog` |
| `weather_gameplay_effects.json` | 15 | `1.0.0` | `UNRESOLVED` | `SourceReference:weather_gameplay_effects.json` |
| `weather_hardening_upgrades.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WeatherHardeningCatalogLoader` |
| `weather_route_gates.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WeatherRouteGateCatalog` |
| `weather_seasons.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WeatherSystem` |
| `year_two_climate.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearTwoClimateCatalog` |

### Whitelists & Infrastructure (3 Catalogs, 50 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `whitelists/companion_trust_flags.json` | 4 | `1.0.0` | `OPTIONAL` | `SourceReference:companion_trust_flags.json` |
| `whitelists/orphan_knocks.json` | 1 | `1.0.0` | `OPTIONAL` | `SourceReference:orphan_knocks.json` |
| `whitelists/plan25_flags.json` | 45 | `1.0.0` | `OPTIONAL` | `SourceReference:plan25_flags.json` |

### Year of Ash (Exp 05) (2 Catalogs, 94 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `door_encounters.json` | 80 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoorEncounterCatalogLoader` |
| `year_of_ash_storm_windows.json` | 14 | `1.0.0` | `UNRESOLVED` | `SourceReference:year_of_ash_storm_windows.json` |

---

## Verification & Integrity Gates

- **Data Integrity Selftest:** `godot --headless --path . -- --data-integrity-selftest` (verifies 137+ primary catalogs, 5,122+ authored IDs, 0 errors).
- **Content Utilization Gate:** `godot --headless --path . -- --content-utilization-selftest` (verifies utilization stages and classification).
- **Schema Policy Gate:** `python3 scripts/ci/json-schema-policy-gate.py` (validates snake_case and schema_version).
