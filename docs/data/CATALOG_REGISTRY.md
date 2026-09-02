# ASHFALL Data Authority & Master Catalog Registry

**Authoritative Location:** `Assets/StreamingAssets/Data/` | **Last Verified:** 2026-09-02
**Total Catalogs:** 487 | **Total Definitions:** 8266 | **Domain Families:** 31

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

### Audio & Music (3 Catalogs, 31 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `audio_cues.json` | 1 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `audio_logs_expansion_05.json` | 30 | `1.0.0` | `OPTIONAL` | `AudioConditionSystem` |
| `cassette_sets.json` | 0 | `1.0.0` | `OPTIONAL` | `VinylMoraleSystem` |

### Combat & Warlords (3 Catalogs, 75 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `chemical_weapons.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `combat_catalog.json` | 46 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CombatCatalog` |
| `warlord_doctrines.json` | 24 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WarlordDoctrineCatalog` |

### Core / Miscellaneous (58 Catalogs, 781 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `aircraft_parts.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `archive_inks.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ArchiveInkCatalogLoader` |
| `belief_movements.json` | 3 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `bounty_board.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `bunker_graffiti_postings.json` | 18 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `camouflage_gear.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `caravans.json` | 0 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `ceremonies.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `collectibles.json` | 0 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `comms_targets.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `development_traits.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `echoes.json` | 23 | `1.0.0` | `OPTIONAL` | `Core default` |
| `ecological_infestations.json` | 10 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `environmental_atmosphere_expansion.json` | 189 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WeatherSystem` |
| `environmental_texts_expansion_05.json` | 42 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterSystem` |
| `excavation_hazard_mitigation.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `excavation_sites.json` | 0 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `fallout_patterns.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `feedback_messages.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FeedbackMessageCatalogLoader` |
| `field_guide.json` | 38 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `interrogation_tactics.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `labor_camps.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `ledger_debt_templates.json` | 25 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `lore_archives.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `memorial_rites.json` | 6 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `memorials_expansion_05.json` | 27 | `1.0.0` | `OPTIONAL` | `MemorialSystem` |
| `mutations.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `narcotics.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `narrative_encounters.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterCatalogLoader` |
| `narrative_encounters_expansion.json` | 29 | `1.0.0` | `OPTIONAL` | `NarrativeEncounterSystem` |
| `narrative_progression.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `NarrativeEncounterSystem` |
| `naval_vessels.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `phantom_heirlooms.json` | 0 | `1.0.0` | `OPTIONAL` | `HeirloomCatalog, HeirloomSystem` |
| `phantom_triggers.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PhantomMemoryHostSession, PhantomMemoryEngine` |
| `political_policies.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `rail_network.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `recreation.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `research_knowledge.json` | 56 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ResearchKnowledgeCatalogLoader` |
| `robotics.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `scavenging_tables.json` | 20 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `settlements.json` | 12 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `skills.json` | 148 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SkillCatalogLoader` |
| `sky_layer_armor_catalog.json` | 6 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `spiritual_rituals.json` | 19 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `starting_supplies.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StartingLevelSystem` |
| `surgical_procedures.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `thermal_gear.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `travel_encounters.json` | 51 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `underground_flora.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `utility_actions.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `UtilityAiSystem` |
| `wall_carving_templates.json` | 0 | `1.0.0` | `OPTIONAL` | `MemorialSystem` |
| `wasteland_grave_epitaphs.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MemorialSystem` |
| `wasteland_laws.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `wasteland_settlement_npcs.json` | 18 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `waystations.json` | 6 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `wildlife_trapping_catalog.json` | 0 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `world_evolution_seeds.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EvolvingWorldCatalog` |
| `world_history.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EvolvingWorldCatalog` |

### Crafting & Relics (4 Catalogs, 85 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `library_manuals.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LibraryManualCatalogLoader` |
| `recipes.json` | 73 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RecipeCatalogLoader` |
| `relic_recipes.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RelicCatalogLoader` |
| `workshop_recipes.json` | 12 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |

### Crossing (Exp 04) (1 Catalogs, 19 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_encounters.json` | 19 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |

### Documents & History (1 Catalogs, 0 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `documents/vel_triage_log_names.json` | 0 | `1.0.0` | `OPTIONAL` | `NarrativeBatchCatalog` |

### Duty Roster (Exp 02) (2 Catalogs, 44 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `duty_roster_marks.json` | 43 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterCatalog` |
| `duty_roster_seasons.json` | 1 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterCatalog` |

### Economy & Trade (6 Catalogs, 44 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `economy_goods.json` | 33 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GoodsCatalog` |
| `hardcore_economy_tuning.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HardcoreEconomyTuningLoader` |
| `trade_screen_scenarios.json` | 3 | `1.0.0` | `OPTIONAL` | `TradeScreenScenarios` |
| `trade_specialties.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TradeSpecialtySystem` |
| `trade_tell_lines.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `TradeTellEngine` |
| `trade_texts.json` | 4 | `1.0.0` | `OPTIONAL` | `TradeScreenPresenter` |

### Events (9 Catalogs, 353 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `desperation_events.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `events.json` | 220 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EventsHostSession` |
| `incidents.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterEncounterSystem` |
| `narrative_arc_events.json` | 15 | `1.0.0` | `OPTIONAL` | `NarrativeEncounterSystem` |
| `orbital_harrow_events.json` | 12 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `seasonal_events.json` | 18 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `shelter_social_events.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `world_evolution_events.json` | 13 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `year_of_ash_events.json` | 52 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Expeditions & Vehicles (3 Catalogs, 53 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `anomalous_expedition_encounters.json` | 0 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `expeditions.json` | 53 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpeditionCatalogLoader` |
| `vehicles.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpeditionVehicleSystem` |

### Factions (18 Catalogs, 174 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_factions.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |
| `faction_lore.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionIconCatalog, FactionIconLoader` |
| `faction_radio_corpus.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_territory.json` | 24 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `faction_war_communiques.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_dialogue.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_events.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_journal.json` | 26 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `faction_war_radio.json` | 33 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `foundry_faction.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalogLoader` |
| `holdfast_factions.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastFactionsCatalog` |
| `independent_faction_branch.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `IndependentBranchCatalog` |
| `military_faction_branch.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MilitaryBranchCatalog` |
| `moral_choice_faction_reactions.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceFactionReactionsCatalogLoader` |
| `muster_faction_actions.json` | 12 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `muster_faction_culture.json` | 6 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `rebel_faction_branch.json` | 8 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RebelBranchCatalog` |
| `standing_record_factions.json` | 1 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StandingRecordCatalog` |

### Foundry & Industry (3 Catalogs, 0 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `foundry_accords.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalog, SilentFoundryCatalogLoader` |
| `foundry_production.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalogLoader` |
| `foundry_treaty_consequences.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryConsequencePolicy` |

### Holdfast (Exp 01) (1 Catalogs, 0 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `holdfast_flavor.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastFlavorCatalog` |

### Items (13 Catalogs, 727 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `black_flotilla_items.json` | 36 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ProceduralScavengeSystem` |
| `chemical_dependency_items.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ChemicalDependencySystem` |
| `crossing_items.json` | 11 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |
| `dose_items.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseContentCatalog` |
| `expansion_item_tags.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ItemCatalogLoader` |
| `foundry_items.json` | 30 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SilentFoundryCatalog` |
| `greenhouse_items.json` | 30 | `1.0.0` | `GAMEPLAY_CONSUMED` | `GreenhouseExpansionCatalog` |
| `holdfast_items.json` | 55 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastItemsCatalog` |
| `item_degradation.json` | 0 | `1.0.0` | `OPTIONAL` | `Core default` |
| `item_description_texts.json` | 0 | `1.0.0` | `OPTIONAL` | `ItemCatalogLoader` |
| `items.json` | 479 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ItemCatalogLoader, LoadItems` |
| `verdict_items.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `year_of_ash_items.json` | 62 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Journal & Logs (3 Catalogs, 91 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `codex_entries.json` | 63 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `journal_entries_expansion_05.json` | 28 | `1.0.0` | `OPTIONAL` | `JournalSystem` |
| `journal_voice_prose.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `JournalVoiceProseCatalog` |

### Locations & Map (13 Catalogs, 365 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_locations.json` | 13 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingCatalog` |
| `damaged_map_zones.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WastelandMapSystem` |
| `deep_lore_locations.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DeepLoreLocationCatalogLoader` |
| `dose_locations.json` | 5 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseContentCatalog` |
| `duty_roster_locations.json` | 14 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterCatalog` |
| `faction_war_location_overrides.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalog` |
| `holdfast_locations.json` | 38 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastCatalog` |
| `locations.json` | 151 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationLayoutSystem, WastelandMapCatalogLoader` |
| `locations_expansion3.json` | 21 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationLayoutSystem` |
| `micro_locations.json` | 25 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `verdict_locations.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `wasteland_map_v1.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WastelandMapCatalogLoader` |
| `year_of_ash_locations.json` | 66 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Maritime & Deep Lore (1 Catalogs, 4 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `dive_sites.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DiveSiteCatalog` |

### Medical & Health (5 Catalogs, 117 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `autopsy_procedures.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `AutopsyProcedureCatalogLoader` |
| `disease_catalog.json` | 16 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DiseaseCatalog, DiseaseSystem` |
| `dose_registers.json` | 18 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseRegistersCatalog` |
| `medical_texts.json` | 83 | `1.0.0` | `OPTIONAL` | `MedicalWardSystem` |
| `pharma_recipes.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PharmaRecipeCatalogLoader` |

### Moral Choice (3 Catalogs, 14 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `moral_choice_chains.json` | 4 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceChainCatalogLoader` |
| `moral_choice_flags.json` | 10 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceFlagCatalogLoader` |
| `moral_choice_gossip.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceGossipCatalogLoader` |

### Muster & Epilogue (5 Catalogs, 36 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `currents.json` | 17 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CurrentsCatalog` |
| `epilogue_chronicle.json` | 0 | `1.0.0` | `OPTIONAL` | `EpilogueMatrix` |
| `muster_camp_scenes.json` | 4 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `muster_epilogues.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `EpilogueMatrix` |
| `muster_witnesses.json` | 15 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WitnessCatalog` |

### Narrative (Codex) (279 Catalogs, 1665 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `narrative/activated_carbon_adsorption_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ammo_hoist_jam_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ammonia_chiller_leak_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/annealing_lehr_birefringence_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/antler_horn_sawing_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/apiculture_red_light_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/aramid_fiber_rot_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/architect_vault_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/armored_cockroach_hive_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/armored_locomotive_manifests.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/artesian_well_contamination_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/awl_saddle_stitch_journals.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bark_tanning_vat_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/beeswax_clarification_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/beeswax_rendering_dipping_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/biochar_cation_exchange_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bisque_firing_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/blast_gate_mechanical_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/blind_cave_molerat_studies.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/boiler_feedwater_deaerator_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bolting_silk_mesh_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bone_degreasing_prep_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/borosilicate_sight_glass_thermal_shock.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/brain_tanning_hide_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/brewers_yeast_krausen_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/brine_pickling_barrel_spoilage.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bullet_alloy_assay_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_blueprints_codex.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_bureaucratic_anomalies.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_children_folklore.json` | 19 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_children_folklore_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_contraband_barter.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_court_verdicts_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_court_verdicts_codex.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_graffiti_postings.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_herbalism_pharmacology.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_maintenance_glitches.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_maintenance_logs_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_maintenance_logs_batch_3.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_rituals_and_cults.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_shift_schedules_and_notices.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_trade_ledger_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_wiretap_transcripts.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bunker_wiretap_transcripts_batch_2.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/bureaucratic_documents_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/burr_millstone_dressing_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/calcium_hypochlorite_titration_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/candle_dip_mould_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/canyon_mudflow_hazard_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/carbide_tool_wear_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/carrion_vulture_sighting_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cave_aquatic_biota_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/celluloid_film_decomposition_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/charcoal_mound_pyrolysis_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/chef_recipe_development.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/chemist_lab_notes_batch_1.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/childrens_artwork_batch_2.json` | 20 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/childrens_folklore_expansion.json` | 31 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/chrome_alum_tanning_assays.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/clay_wedging_forming_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cobalt_arming_directives.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cobalt_liturgies.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cobalt_liturgies_batch_2.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cold_process_soap_curing_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/conflict_mediation_records.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/council_meeting_minutes.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/courier_dispatches_master.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/courier_mission_logs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/courier_mission_logs_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/crater_lake_limnology_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/crop_experiment_logs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/crop_genome_degradation_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/crucible_clay_pot_slag_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cryo_germplasm_viability_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cryo_seed_ampoule_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cryopod_failure_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/culinary_ration_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/culinary_ration_codex.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cupola_melting_ratio_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/cupola_slag_leaching_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/currents_pamphlets.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/currying_burnishing_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/dead_hand_directives.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/deadbeat_escapement_wear_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/deckle_mould_watermark_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/deep_lore_texts.json` | 10 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/diplomatic_contact_records_batch_1.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/documents_batch_1.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/documents_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/documents_batch_3.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/drone_carrier_blackboxes.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/drop_spindle_fibre_drafting_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/dweller_dependency_backstories.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/dweller_heirlooms_master.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/dweller_medical_casebook.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/dweller_psychological_journals.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/education_session_records.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/emp_atmospheric_sniffer_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/engineering_logs_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/engineering_mod_notes.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/equipment_failure_logs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/eulogy_corpus_batch_1.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_briefs_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_field_reports.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_field_reports_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_planning_briefs_batch_1.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/faction_directives_and_notices.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/faction_field_documents.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/faction_texts_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/fallout_sensory_loss_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/fermentation_crock_airlock_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/fibre_heckling_prep_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/field_reports_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/forge_charcoal_ash_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/found_objects_expansion.json` | 40 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/fulling_trough_nap_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/gear_quenching_fault_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/geological_strata_logs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/geophone_hymnals.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/geothermal_borehole_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/geothermal_steam_vent_diagnostics.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/geothermal_steam_well_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ghost_transmissions.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/graffiti_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/grain_silo_weevil_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/green_sand_bentonite_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/greenhouse_cultivation_logs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ground_glass_joint_greasing_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/heirloom_seed_viability_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/hemp_fiber_hackling_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/hollander_beater_pulping_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/honey_extractor_balance_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/hydrophone_acoustic_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/improvised_repair_guides_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/inkle_loom_warp_tally_sheets.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/intake_filter_clogging_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/invar_pendulum_thermal_expansion.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/iron_gall_ink_acidity_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/iron_synod_canons.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/journal_entries_batch_1.json` | 18 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/journal_entries_batch_2.json` | 15 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/journal_entries_batch_3.json` | 88 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/journals_expansion.json` | 40 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/jrnl_templates_cycle_c.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/jrnl_templates_cycle_d.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/kiln_draw_trial_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/langstroth_hive_foundation_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/lead_crystal_scintillator_aging_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/lead_wall_degradation_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/leather_harness_conditioning_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/letters_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/liebig_condenser_fracture_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/lime_kiln_calcination_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/liquid_nitrogen_compressor_failures.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/load_shed_schedule_001.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/lost_tech_manuals.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/mainspring_fatigue_rupture_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/manila_hawser_breakage_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/medical_documents_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/memorials_expansion.json` | 40 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/mill_dampener_tempering_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/mortise_tenon_failure_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/mudbrick_weathering_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/munitions_leaching_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/mutated_botanical_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/needle_awl_hook_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/neoprene_gasket_degradation_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/new_arrival_intake_interviews.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/night_watch_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/night_watch_logbook.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/numbers_station_ciphers.json` | 11 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/oak_bark_tanning_pit_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/operating_theater_surgical_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/optical_coating_rad_browning_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/oral_lore_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/oral_lore_codex.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/orbital_kinetic_telemetry.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ozone_contact_tower_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/patrol_debriefs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pattern_maker_shrinkage_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/periscope_prism_delamination_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/permafrost_methane_eruption_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/personal_effects_inventory_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pipeline_sabotage_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/plan17_discoverable_documents.json` | 18 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pneumatic_carrier_capsule_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pneumatic_cylinder_leather_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pneumatic_tube_diverter_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pot_furnace_glass_melts.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/power_grid_management_logs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/pozzolan_mortar_formulations.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/quest_narrative_documents.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rad_pathology_autopsy_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radiation_survey_readings_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_broadcast_rundowns.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_mysteries_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_scriptbook.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_scripts_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_transcripts_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/radio_transcripts_batch_3.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rag_pulp_beater_records.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ragdoll_germination_assays.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ration_fraud_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/ration_records_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rawhide_bating_failure_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/refractory_firebrick_spalling_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/regional_treaty_protocols.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/relic_provenance_dossiers.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/retort_wood_vinegar_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/root_cellar_humidity_rot_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rootes_blower_vacuum_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rope_break_load_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/rope_transmission_splicing_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/salt_mine_inscriptions.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/scavenger_expedition_route_notes.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/scraping_polishing_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/screw_press_felt_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/security_incident_reports_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/seismic_array_fault_alarms.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/shelter_notices_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/shelter_songs_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/silage_lactic_pit_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/silica_gel_seed_desiccation_audits.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/silo_mosquito_vector_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/slip_glaze_formulation_notes.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/slow_sand_schmutzdecke_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/smoked_meat_creosote_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/sonar_array_fault_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/sourdough_mother_acidity_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/square_set_shoring_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/stalactite_mineral_assay_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/steam_trap_water_hammer_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/stencil_propaganda_smear_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/strand_twisting_lay_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/substation_transformer_fires.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/sump_drainage_silt_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/supply_audit_records.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/supply_audit_records_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/surface_dragline_ruins.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/surface_radiation_topo_sheets.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/surgeons_casebook_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/survivor_letters_lost_kin.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/survivor_profiles_expansion.json` | 40 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/sweet_water_glycerin_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/tallow_rendering_vat_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/tallow_saponification_kettle_audits.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/therapist_session_notes.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/therapist_session_notes_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/therapist_session_notes_batch_3.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/three_strand_rope_closing_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/timber_creosote_treatment_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/timber_dry_rot_fruiting_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/tire_retreading_compound_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/trade_ledgers_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/treadle_loom_heddle_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/tub_sizing_gelatin_assays.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/turbine_blade_erosion_reports.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/typographic_lead_wear_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/underground_fungi_flora.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/undertaker_burial_records.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/unsent_letters_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/vault_seal_breach_logs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/vinyl_record_archive.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_expeditions_master.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_grave_epitaphs.json` | 7 | `1.0.0` | `CODEX_ONLY` | `MemorialSystem` |
| `narrative/wasteland_grave_epitaphs_batch_2.json` | 12 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_settlement_gazetteer.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_trade_caravan_routes.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wasteland_wildlife_bestiary.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/water_clock_orifice_silt_records.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/water_quality_test_reports_batch_2.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/weather_almanac_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wick_braiding_priming_reports.json` | 7 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wildlife_field_encounter_logs.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wire_confessions.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wire_rope_stranding_assays.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/wood_ash_lye_hydrometer_logs.json` | 8 | `1.0.0` | `CODEX_ONLY` | `Core default` |
| `narrative/world_history_expansion.json` | 0 | `1.0.0` | `CODEX_ONLY` | `Core default` |

### Quests (23 Catalogs, 2943 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `crossing_quests.json` | 109 | `1.0.0` | `GAMEPLAY_CONSUMED` | `CrossingQuestSystem` |
| `dose_quests.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoseContentCatalog` |
| `duty_roster_quests.json` | 160 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DutyRosterQuestRuntime` |
| `dynamic_questlines.json` | 4 | `1.0.0` | `OPTIONAL` | `QuestlineSystem` |
| `holdfast_quests.json` | 91 | `1.0.0` | `GAMEPLAY_CONSUMED` | `HoldfastQuestSystem` |
| `moral_choice_quest_stubs.json` | 10 | `1.0.0` | `OPTIONAL` | `Core default` |
| `moral_choice_quests.json` | 65 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceCatalogLoader` |
| `moral_choice_quests_branching.json` | 100 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceBranchQuestCatalogLoader` |
| `moral_choice_quests_expansion.json` | 50 | `1.0.0` | `GAMEPLAY_CONSUMED` | `MoralChoiceExpansionQuestCatalogLoader` |
| `narrative_questlines.json` | 8 | `1.0.0` | `OPTIONAL` | `NarrativeEncounterSystem` |
| `questline_master.json` | 451 | `1.0.0` | `GAMEPLAY_CONSUMED` | `QuestlineMasterCatalog` |
| `quests_bureaucratic_morality.json` | 6 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `quests_expansion_05.json` | 116 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpansionQuestSystem` |
| `quests_expansion_06.json` | 52 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ExpansionQuestSystem` |
| `quests_faction_branching.json` | 601 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `quests_massive_expansion_200.json` | 601 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `quests_moral_branching_expansion.json` | 91 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `repeatable_quests.json` | 10 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `standing_record_quests.json` | 137 | `1.0.0` | `GAMEPLAY_CONSUMED` | `StandingRecordCatalog` |
| `thirdonary_quests.json` | 249 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ThirdonaryCatalogLoader` |
| `verdict_questlines.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VerdictQuestCatalogLoader` |
| `year_of_ash_questlines.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |
| `year_of_ash_quests.json` | 32 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Radio & Signals (6 Catalogs, 144 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `radio.json` | 65 | `1.0.0` | `GAMEPLAY_CONSUMED` | `RadioHostSession, RadioScriptbookCatalog` |
| `radio_distress_signals.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SignalTriangulationSystem` |
| `radio_distress_signals_expansion.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SignalTriangulationSystem` |
| `radio_intercepts.json` | 16 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `verdict_radio.json` | 13 | `1.0.0` | `GAMEPLAY_CONSUMED` | `Core default` |
| `year_of_ash_radio.json` | 50 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Shelter & Power (5 Catalogs, 165 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `power_grid.json` | 6 | `1.0.0` | `GAMEPLAY_CONSUMED` | `PowerGridSystem` |
| `shelter_machine_identities.json` | 38 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `shelter_room_identities.json` | 87 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `shelter_rooms.json` | 34 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `shelter_schedules.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `ShelterScheduleCatalogLoader` |

### Social & Psychology (3 Catalogs, 0 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `confession_secrets.json` | 0 | `1.0.0` | `OPTIONAL` | `ConfessionSecrets` |
| `final_wishes.json` | 0 | `1.0.0` | `OPTIONAL` | `FinalWishSystem` |
| `guilt_sources.json` | 0 | `1.0.0` | `OPTIONAL` | `GuiltInsomniaSystem` |

### Standing Record (Exp 03) (2 Catalogs, 66 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `standing_record_layouts.json` | 66 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationLayoutSystem` |
| `standing_record_memory.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `LocationMemorySystem` |

### Survivors (7 Catalogs, 228 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `antigravity_survivor_fields.json` | 0 | `1.0.0` | `OPTIONAL` | `SurvivorCatalog` |
| `characters.json` | 60 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalog` |
| `deep_lore_survivor_fields.json` | 0 | `1.0.0` | `OPTIONAL` | `SurvivorCatalog` |
| `expansion_survivor_fields.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalog` |
| `starting_survivors.json` | 3 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorStartingStateLoader` |
| `survivors.json` | 129 | `1.0.0` | `GAMEPLAY_CONSUMED` | `SurvivorCatalogLoader, SurvivorCatalog` |
| `year_of_ash_survivors.json` | 36 | `1.0.0` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader` |

### Verdict (Exp 03) (2 Catalogs, 20 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `verdict_data.json` | 11 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VerdictCatalogLoader` |
| `verdict_npcs.json` | 9 | `1.0.0` | `GAMEPLAY_CONSUMED` | `VerdictNpcSystem` |

### Weather & Environment (2 Catalogs, 22 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `weather_route_gates.json` | 15 | `1.0.0` | `UNRESOLVED` | `Core default` |
| `weather_seasons.json` | 7 | `1.0.0` | `GAMEPLAY_CONSUMED` | `WeatherSystem` |

### Whitelists & Infrastructure (2 Catalogs, 0 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `whitelists/orphan_knocks.json` | 0 | `1.0.0` | `OPTIONAL` | `Core default` |
| `whitelists/plan25_flags.json` | 0 | `1.0.0` | `OPTIONAL` | `Core default` |

### Year of Ash (Exp 05) (1 Catalogs, 0 Definitions)

| Catalog Path | Definitions | Schema | Classification | Primary C# Loader |
|---|---|---|---|---|
| `door_encounters.json` | 0 | `1.0.0` | `GAMEPLAY_CONSUMED` | `DoorEncounterCatalogLoader` |

---

## Verification & Integrity Gates

- **Data Integrity Selftest:** `godot --headless --path . -- --data-integrity-selftest` (verifies 137+ primary catalogs, 5,122+ authored IDs, 0 errors).
- **Content Utilization Gate:** `godot --headless --path . -- --content-utilization-selftest` (verifies utilization stages and classification).
- **Schema Policy Gate:** `python3 scripts/ci/json-schema-policy-gate.py` (validates snake_case and schema_version).
