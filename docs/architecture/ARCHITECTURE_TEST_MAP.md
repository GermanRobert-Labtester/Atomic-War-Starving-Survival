# ASHFALL — Evidence-Derived Architecture & Verification Graph

**Last Verified:** 2026-09-05<br>
**Total Subsystems Mapped:** 112/112 (100.0%)<br>
**Verified End-to-End Coverage:** 109/112 (97.3% across all 6 vertical layers)<br>
**Status Breakdown:** Implemented: 112/112 | Constructed: 112/112 | Ticked: 112/112 | Persisted: 112/112 | Routed: 111/112 | Tested: 110/112<br>
**Single Source of Truth:** `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` & `Assets/Ashfall.Core/HostCliRegistry.cs`

> **GENERATED FILE — do not edit by hand.**
> Derived mechanically from real C# type definitions, catalog JSON files, host wiring, and test fixtures.
> Generated via: `bash scripts/ci/generate-architecture-map.sh`
> CI Completeness Gate: `bash scripts/ci/generate-architecture-map.sh --check`

---

## 1. Six-Tier Architectural Layering Flow & Discrete Verification Taxonomy

Every subsystem in ASHFALL is verified against six distinct, non-fungible lifecycle layers:

```
┌────────────────────────────────────────────────────────────────────────┐
│ 1. CORE DOMAIN LOGIC [Implemented]                                     │
│    Engine-agnostic C# systems under Assets/Ashfall.Core/ (0 engine refs)│
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ reads definition schemas
┌───────────────────────────────────▼────────────────────────────────────┐
│ 2. DATA CATALOG AUTHORITY [Data]                                       │
│    snake_case JSON schemas under Assets/StreamingAssets/Data/          │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ constructed & orchestrated by
┌───────────────────────────────────▼────────────────────────────────────┐
│ 3. GODOT HOST SESSION [Constructed & Ticked]                           │
│    Session lifecycle in src/Host/ with Setup* wiring & sim tick cadence │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ snapshots / restores via
┌───────────────────────────────────▼────────────────────────────────────┐
│ 4. PERSISTENCE SAVE STORE [Persisted]                                  │
│    Checksummed SaveStore<T> via SaveStoreHub, atomic writes & SaveAll  │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ presents live state to user
┌───────────────────────────────────▼────────────────────────────────────┐
│ 5. GODOT UI PANEL [Player-Routed]                                      │
│    Responsive Control under src/UI/ routed in OpenPlayerPanel/HUD      │
└───────────────────────────────────┬────────────────────────────────────┘
                                    │ protected & regression-gated by
┌───────────────────────────────────▼────────────────────────────────────┐
│ 6. CI SELF-TEST & XUNIT SUITE [Tested]                                 │
│    CLI verbs in HostCliRegistry.cs & test fixtures in Ashfall.Core.Tests│
└────────────────────────────────────────────────────────────────────────┘
```

---

## 2. Complete Architecture Subsystem & Evidence-Derived Graph Matrix

| # | Section Key | Domain | Core System | Data Catalog | Host Session | Save Store | UI Panel | CLI Self-Test / Unit Tests | Status |
|---|---|---|---|---|---|---|---|---|:---:|
| 1 | `endgame` | Campaign & Lore | `EndgameSystem`, `CampaignOutcomeEvaluator` | — *(Procedural)* | `EndgameHostSession` | `EndgameSaveStore` | `EpiloguePanel` | `--endings-selftest`, `EndgameSystemTests`, `CampaignOutcomeEvaluatorTests` | ✅ 6/6 |
| 2 | `host_event` | Campaign & Lore | `MoralChoiceSystem` | `events.json` | `HostEventAdapter` | `MoralChoiceSaveStore`, `HostEventSaveStore` | `EventDetailPanel` | `--moral-choice-selftest`, `HostEventSaveSealTests` | ✅ 6/6 |
| 3 | `journal` | Campaign & Lore | `JournalSystem` | `world_history.json` | `JournalHostSession` | `JournalSaveStore` | `JournalPanel`, `JournalBookUI` | `--journal-save-selftest`, `JournalSystemTests` | ✅ 6/6 |
| 4 | `memorial` | Campaign & Lore | `MemorialSystem` | — *(Procedural)* | `MemorialSystem` | `MemorialSaveStore` | `GameDashboardPanel` | `--player-panels-uitest`, `MemorialSystemTests` | ✅ 6/6 |
| 5 | `narrative` | Campaign & Lore | `NarrativeEncounterSystem` | `narrative_encounters.json` | `NarrativeHostSession` | `NarrativeSaveStore` | `EventsLogPanel`, `FactionsNarrativePanel` | `--narrative-selftest`, `NarrativeEncounterSystemTests` | ✅ 6/6 |
| 6 | `phase0` | Campaign & Lore | `RespiratoryDegenerationSystem` | — *(Procedural)* | `Phase0HostSession` | `Phase0SaveStore` | `Phase0Panel` | `--phase0-selftest`, `--phase0-uitest`, `Phase0EffectsBridgeTests` | ✅ 6/6 |
| 7 | `survivor_fate` | Campaign & Lore | `SurvivorFateSystem` | — *(Procedural)* | `Main` | `SurvivorFateSaveStore` | `GameDashboardPanel` | `--playable-shell-selftest`, `SurvivorFateSystemTests` | ✅ 6/6 |
| 8 | `onboarding` | Campaign & Onboarding | `OnboardingJourney` | — *(Procedural)* | `Main` | `OnboardingSaveStore` | `OnboardingHintPanel` | `--onboarding-journey-selftest`, `OnboardingJourneyTests` | ✅ 6/6 |
| 9 | `archive_desk` | Campaign & Progression | `ArchiveDeskSystem` | `archive_inks.json` | `ArchiveDeskHostSession` | `ArchiveDeskSaveStore` | `ArchiveDeskPanel` | `--shelter-operations-selftest`, `ArchiveDeskSystemTests` | ✅ 6/6 |
| 10 | `campaign_day` | Campaign & Progression | `CampaignDayCoordinator` | — *(Procedural)* | `CampaignDayCoordinator` | `CampaignDaySaveStore` | `GameDashboardPanel` | `--day1-selftest`, `--day1-to-day2-selftest`, `CampaignDayCoordinatorTests` | ✅ 6/6 |
| 11 | `daily_briefing` | Campaign & Progression | `DailyBriefingReportBuilder`, `DailyBriefingState` | — *(Procedural)* | `DailyBriefingState` | `DailyBriefingSaveStore` | `DailyBriefingModal` | `--day1-selftest`, `DailyBriefingReportBuilderTests` | ✅ 6/6 |
| 12 | `library_study` | Campaign & Progression | `LibraryStudySystem` | `library_manuals.json` | `LibraryStudyHostSession` | `LibraryStudySaveStore` | `LibraryStudyPanel` | `--shelter-operations-selftest`, `LibraryStudySystemTests` | ✅ 6/6 |
| 13 | `dynamic_quests` | Campaign & Quests | `DynamicQuestlineSystem` | `dynamic_questlines.json` | `DynamicQuestSaveStore` | `DynamicQuestSaveStore` | *None (GAP)* | `--save-store-checksum-selftest`, `DynamicQuestlineTests` | ❌ GAP |
| 14 | `personal_quests` | Campaign & Quests | `PersonalQuestSystem` | — *(Procedural)* | `PersonalQuestHostSession` | `PersonalQuestSaveStore` | `QuestsPanel`, `QuestDetailPanel` | `--save-store-checksum-selftest`, `PersonalQuestSystemTests` | ✅ 6/6 |
| 15 | `perimeter_defense` | Combat & Defense | `PerimeterDefenseSystem` | `perimeter_defenses.json` | `Main` | `PerimeterDefenseSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `PerimeterDefenseTests` | ✅ 6/6 |
| 16 | `chemical_synthesis` | Crafting & Chemistry | `ChemicalSynthesisSystem` | `chemical_syntheses.json` | `ChemicalSynthesisHostSession` | `ChemicalSynthesisSaveStore` | `ChemicalLabPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 17 | `caravan` | Economy & Trade | `TravelingCaravanSystem` | `trade_texts.json` | `TravelingCaravanHostSession` | `CaravanSaveStore` | `TravelingCaravanPanel` | `--caravan-selftest`, `TradeCaravanCatalogTests` | ✅ 6/6 |
| 18 | `caravan_trade_network` | Economy & Trade | `CaravanTradeNetworkSystem` | `caravan_trade_routes.json` | `Main` | `CaravanTradeSaveStore` | `TravelingCaravanPanel` | `--caravan-selftest`, `CaravanTradeNetworkTests` | ✅ 6/6 |
| 19 | `economy` | Economy & Trade | `MarketSystem` | `economy_goods.json` | `EconomyHostSession` | `EconomySaveStore` | `EconomyMarketPanel`, `EconomyDetailPanel` | `--economy-selftest`, `--economy-uitest`, `DynamicEconomyCharacterizationTests` | ✅ 6/6 |
| 20 | `regional_treaty` | Economy & Trade | `RegionalTreatySystem` | `faction_lore.json` | `RegionalTreatyHostSession` | `RegionalTreatySaveStore` | `RegionalTreatyPanel` | `--shelter-operations-selftest`, `RegionalTreatySaveChecksumTests` | ✅ 6/6 |
| 21 | `expansion_hub` | Expansion Framework | `ExpansionMasterSession` | — *(Procedural)* | `ExpansionHostSession` | `ExpansionHubSaveStore` | `ExpansionsHubPanel` | `--expansions-selftest`, `--expansion-hub-save-selftest`, `ExpansionHubSaveTests` | ✅ 6/6 |
| 22 | `expansion_quest` | Expansion Framework | `ExpansionQuestSystem`, `ExpansionMasterSession` | `crossing_quests.json` | `ExpansionQuestHostSession` | `ExpansionQuestSaveStore` | `CrossingQuestPanel` | `--expansions-selftest`, `VersionReportContractTests` | ✅ 6/6 |
| 23 | `holdfast` | Expansions (Exp 01) | `HoldfastQuestSystem`, `HoldfastSession` | `holdfast_quests.json`, `holdfast_items.json` | `HoldfastRuntimeSession` | `HoldfastSaveStore` | `HoldfastTerminalPanel`, `GameDashboardPanel` | `--holdfast-save-selftest`, `--holdfast-selftest`, `HoldfastSaveTests` | ✅ 6/6 |
| 24 | `holdfast_trade` | Expansions (Exp 01) | `HoldfastTradeSession` | `items.json` | `HoldfastRuntimeSession` | `HoldfastTradeSaveStore` | `TradeScreenGodotPanel`, `HoldfastTerminalPanel` | `--holdfast-trade-save-selftest`, `HoldfastTradeSessionTests` | ✅ 6/6 |
| 25 | `duty_roster` | Expansions (Exp 02) | `DutyRosterSystem` | `duty_roster_quests.json`, `survivors.json` | `DutyRosterHostSession` | `DutyRosterSaveStore` | `DutyRosterPanel`, `DutyRosterDetailPanel` | `--duty-roster-selftest`, `--duty-roster-save-selftest`, `DutyRosterSaveTests` | ✅ 6/6 |
| 26 | `phantom_memory` | Expansions (Exp 03) | `PhantomMemoryEngine` | `phantom_triggers.json` | `PhantomMemoryHostSession` | `PhantomMemorySaveStore` | `StandingRecordPanel`, `PhantomMemoryPanel` | `--standing-record-selftest`, `PhantomMemoryEngineTests` | ✅ 6/6 |
| 27 | `thirdonary` | Expansions (Exp 04) | `ThirdonaryQuestSystem` | `thirdonary_quests.json` | `ThirdonaryHostSession` | `ThirdonarySaveStore` | `CrossingQuestPanel` | `--crossing-selftest`, `--arbitration-selftest`, `ThirdonaryQuestSystemTests`, `CrossingArbitrationSystemTests` | ✅ 6/6 |
| 28 | `year_of_ash` | Expansions (Exp 05) | `YearOfAshDeepFreezeSystem`, `YearOfAshRadonSystem` | `year_of_ash_events.json` | `YearOfAshHostSession` | `YearOfAshSaveStore` | `DoorEncounterModal` | `--year-of-ash-save-selftest`, `YearOfAshQuestProbe` | ✅ 6/6 |
| 29 | `muster` | Expansions (Exp 06) | `MusterSystem` | `muster_witnesses.json` | `MusterHostSession` | `MusterSaveStore` | `MusterPanel` | `--muster-selftest`, `--muster-uitest`, `MusterSystemTests` | ✅ 6/6 |
| 30 | `dose_ledger` | Expansions (Exp 07) | `DoseLedgerSystem`, `RadiationSystem` | `dose_items.json` | `DoseLedgerHostSession` | `DoseLedgerSaveStore` | `RadiationHistoryPanel`, `RadiationDetailPanel` | `--dose-ledger-selftest`, `--dose-uitest`, `NeedsRadiationSaveRoundTripTests` | ✅ 6/6 |
| 31 | `verdict` | Expansions (Exp 08) | `ReckoningSystem`, `MachineLogSystem` | `verdict_data.json` | `VerdictHostSession` | `VerdictSaveStore` | `VerdictPanel`, `VerdictDashboardPanel` | `--verdict-selftest`, `--verdict-uitest`, `VerdictChainTests` | ✅ 6/6 |
| 32 | `maritime` | Expansions (Exp 09) | `MaritimeDiveSystem` | `dive_sites.json` | `MaritimeHostSession` | `MaritimeSaveStore` | `MaritimePanel` | `--black-flotilla-selftest`, `BlackFlotillaTests` | ✅ 6/6 |
| 33 | `silent_foundry` | Expansions (Exp 10) | `SilentFoundrySystem` | `foundry_items.json` | `SilentFoundryHostSession` | `SilentFoundrySaveStore` | `SilentFoundryPanel` | `--silent-foundry-selftest`, `--silent-foundry-uitest`, `SilentFoundryConsequenceTests` | ✅ 6/6 |
| 34 | `weight_of_choices` | Factions & Diplomacy | `FactionBranchCoordinator`, `MilitaryBranchSystem`, `RebelBranchSystem`, `IndependentBranchSystem`, `PrpfStandingSystem` | `military_faction_branch.json`, `rebel_faction_branch.json`, `independent_faction_branch.json` | `FactionBranchHostSession` | `WeightOfChoicesSaveStore` | `FactionsPanel`, `QuestsPanel` | `--expansions-selftest`, `FactionBranchCoordinatorTests`, `MilitaryBranchSystemTests`, `RebelBranchSystemTests`, `IndependentBranchSystemTests`, `PrpfStandingSystemTests`, `WeightOfChoicesSaveTests` | ✅ 6/6 |
| 35 | `collectible_discovery` | Inventory & Lore | `CollectibleDiscoveryState` | `collectibles.json` | `Main` | `CollectibleDiscoverySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CollectibleDiscoveryPersistenceTests` | ✅ 6/6 |
| 36 | `unique_claims` | Inventory & Lore | `UniqueItemClaimRegistry` | `collectibles.json` | `Main` | `UniqueClaimSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CollectibleDiscoveryPersistenceTests` | ✅ 6/6 |
| 37 | `field_guide` | Knowledge | `FieldGuideCatalog` | — *(Procedural)* | `Main` | `FieldGuideSaveStore` | `GameDashboardPanel` | `--world-selftest`, `FieldGuidePersistenceTests` | ✅ 6/6 |
| 38 | `research` | Knowledge | `ResearchSystem` | `research_knowledge.json` | `Main` | `ResearchSaveStore` | `ResearchPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `MedicalPipelineArchitectureGateTests` | ✅ 6/6 |
| 39 | `medical_pipeline` | Medical | `MedicalPipelineCoordinator` | `disease_catalog.json` | `Main` | `MedicalPipelineSaveStore` | `MedicalPanel`, `GameDashboardPanel` | `--save-load-ui-failure-selftest`, `MedicalPipelineArchitectureGateTests` | ✅ 6/6 |
| 40 | `pathogen_strains` | Medical | `PathogenStrainSystem` | `pathogens.json` | `Main` | `PathogenStrainSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `DiseaseSystemTests` | ✅ 6/6 |
| 41 | `surgical_ward` | Medical | `AdvancedSurgicalWardSystem` | — *(Procedural)* | `Main` | `SurgicalWardSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `AdvancedSurgicalWardTests` | ✅ 6/6 |
| 42 | `moral_choice` | Narrative & Decisions | `MoralChoiceSystem`, `MoralChoiceState` | `moral_choice_quests.json` | `MoralChoiceSystem` | `MoralChoiceSaveStore` | `GameDashboardPanel` | `--moral-choice-selftest`, `MoralChoiceSystemTests` | ✅ 6/6 |
| 43 | `amputation` | Plans 178-201 Expansion Block | `AmputationSystem` | `surgical_procedures.json` | `Main` | `AmputationSaveStore` | `MedicalPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `AmputationSystemTests` | ✅ 6/6 |
| 44 | `archaeology` | Plans 178-201 Expansion Block | `ArchaeologySystem` | `lore_archives.json` | `Main` | `ArchaeologySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ArchaeologySystemTests` | ✅ 6/6 |
| 45 | `aviation` | Plans 178-201 Expansion Block | `AviationSystem` | `aircraft_parts.json` | `Main` | `AviationSaveStore` | `AviationUI`, `GameDashboardPanel` | `--expedition-selftest`, `AviationSystemTests` | ✅ 6/6 |
| 46 | `ceremony` | Plans 178-201 Expansion Block | `CeremonySystem` | `ceremonies.json` | `Main` | `CeremonySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CeremonySystemTests` | ✅ 6/6 |
| 47 | `chem_warfare` | Plans 178-201 Expansion Block | `ChemWarfareSystem` | `chemical_weapons.json` | `Main` | `ChemWarfareSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ChemWarfareSystemTests` | ✅ 6/6 |
| 48 | `child_development` | Plans 178-201 Expansion Block | `GenerationalSystem` | `development_traits.json` | `Main` | `GenerationalSaveStore` | `NurseryPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `GenerationalSystemTests`, `GenerationalLineageExtensionTests` | ✅ 6/6 |
| 49 | `comms_array` | Plans 178-201 Expansion Block | `CommsArraySystem` | `comms_targets.json` | `Main` | `CommsArraySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CommsArraySystemTests` | ✅ 6/6 |
| 50 | `desperation` | Plans 178-201 Expansion Block | `DesperationSystem` | `desperation_events.json` | `Main` | `DesperationSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `DesperationSystemTests` | ✅ 6/6 |
| 51 | `expedition_stealth` | Plans 178-201 Expansion Block | `StealthSystem` | `camouflage_gear.json` | `Main` | `StealthSaveStore` | `StealthReadoutPanel`, `GameDashboardPanel` | `--expedition-selftest`, `StealthSystemTests` | ✅ 6/6 |
| 52 | `fallout` | Plans 178-201 Expansion Block | `FalloutSystem` | `fallout_patterns.json` | `Main` | `FalloutSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FalloutSystemTests` | ✅ 6/6 |
| 53 | `forced_labor` | Plans 178-201 Expansion Block | `ForcedLaborSystem` | `labor_camps.json` | `Main` | `ForcedLaborSaveStore` | `LaborUI`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `ForcedLaborSystemTests` | ✅ 6/6 |
| 54 | `fungi_cultivation` | Plans 178-201 Expansion Block | `FungiCultivationSystem` | `underground_flora.json` | `Main` | `FungiSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FungiCultivationSystemTests` | ✅ 6/6 |
| 55 | `mercenary_bounties` | Plans 178-201 Expansion Block | `MercenarySystem` | `bounty_board.json` | `Main` | `MercenarySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `MercenarySystemTests` | ✅ 6/6 |
| 56 | `mutation_tree` | Plans 178-201 Expansion Block | `MutationSystem` | `mutations.json` | `Main` | `MutationSaveStore` | `MutationTreePanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `MutationSystemTests` | ✅ 6/6 |
| 57 | `narcotics` | Plans 178-201 Expansion Block | `NarcoticsSystem` | `narcotics.json` | `Main` | `NarcoticsSaveStore` | `ChemUI`, `PharmaLabPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `NarcoticsSystemTests` | ✅ 6/6 |
| 58 | `prisoner_management` | Plans 178-201 Expansion Block | `PrisonerSystem` | `interrogation_tactics.json` | `Main` | `PrisonerSaveStore` | `PrisonerPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `PrisonerSystemTests` | ✅ 6/6 |
| 59 | `railway` | Plans 178-201 Expansion Block | `RailwaySystem` | `rail_network.json` | `Main` | `RailwaySaveStore` | `GameDashboardPanel` | `--expedition-selftest`, `RailwaySystemTests` | ✅ 6/6 |
| 60 | `recreation` | Plans 178-201 Expansion Block | `SurvivorDowntimeSystem` | `recreation.json` | `Main` | `RecreationSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `SurvivorDowntimeSystemTests` | ✅ 6/6 |
| 61 | `robotics` | Plans 178-201 Expansion Block | `RoboticsSystem` | `robotics.json` | `Main` | `RoboticsSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `RoboticsSystemTests` | ✅ 6/6 |
| 62 | `settlement_politics` | Plans 178-201 Expansion Block | `PoliticsSystem` | `political_policies.json` | `Main` | `PoliticsSaveStore` | `PoliticsUI`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `PoliticsSystemTests` | ✅ 6/6 |
| 63 | `wasteland_justice` | Plans 178-201 Expansion Block | `JusticeSystem` | `wasteland_laws.json` | `Main` | `JusticeSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `JusticeSystemTests` | ✅ 6/6 |
| 64 | `excavation_hazards` | Shelter | `ExcavationHazardSystem` | — *(Procedural)* | `Main` | `ExcavationHazardSaveStore` | `GameDashboardPanel` | `--shelter-hazard-selftest`, `ExcavationSystemTests` | ✅ 6/6 |
| 65 | `radio_station` | Shelter | `ShelterRadioStationSystem` | — *(Procedural)* | `Main` | `RadioStationSaveStore` | `RadioPanel`, `GameDashboardPanel` | `--core-selftest`, `ShelterRadioStationTests` | ✅ 6/6 |
| 66 | `shelter_decor` | Shelter | `ShelterDecorSystem` | — *(Procedural)* | `ShelterDecorHostSession` | `ShelterDecorSaveStore` | `GameDashboardPanel` | `--shelter-decor-selftest`, `Plan12CDecorTests` | ✅ 6/6 |
| 67 | `shelter_social_dynamics` | Shelter | `ShelterSocialDynamicsSystem` | `shelter_social_events.json` | `Main` | `ShelterSocialSaveStore` | `GameDashboardPanel` | `--core-selftest`, `ShelterSocialDynamicsTests` | ✅ 6/6 |
| 68 | `shelter_workshop` | Shelter | `ShelterWorkshopSystem` | — *(Procedural)* | `Main` | `ShelterWorkshopSaveStore` | `WorkshopPanel`, `GameDashboardPanel` | `--core-selftest`, `WorkshopReverseEngineeringSystemTests` | ✅ 6/6 |
| 69 | `hydroponic_biomes` | Shelter & Farming | `HydroponicBiomeSystem` | `hydroponic_crops.json` | `Main` | `HydroponicBiomeSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `HydroponicBiomeTests` | ✅ 6/6 |
| 70 | `airlock_security` | Shelter & Infrastructure | `AirlockSecuritySystem` | — *(Procedural)* | `AirlockSecurityHostSession` | `AirlockSecuritySaveStore` | `AirlockSecurityPanel` | `--shelter-operations-selftest`, `AirlockSecuritySystemTests` | ✅ 6/6 |
| 71 | `decontamination` | Shelter & Infrastructure | `DecontaminationSystem` | — *(Procedural)* | `DecontaminationHostSession` | `DecontaminationSaveStore` | `DecontaminationPanel` | `--shelter-operations-selftest`, `DecontaminationSystemTests` | ✅ 6/6 |
| 72 | `excavation` | Shelter & Infrastructure | `ExcavationSystem` | — *(Procedural)* | `ExcavationHostSession` | `ExcavationSaveStore` | `ExcavationPanel` | `--shelter-operations-selftest`, `ExcavationSystemTests` | ✅ 6/6 |
| 73 | `greenhouse` | Shelter & Infrastructure | `GreenhouseSystem` | `greenhouse_items.json` | `GreenhouseHostSession` | `GreenhouseSaveStore` | `GreenhousePanel` | `--greenhouse-selftest`, `GreenhouseSystemTests` | ✅ 6/6 |
| 74 | `nuclear_core_lifecycle` | Shelter & Infrastructure | `NuclearCoreLifecycleSystem` | `nuclear_core_profiles.json` | `Main` | `NuclearCoreSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `NuclearCoreLifecycleTests` | ✅ 6/6 |
| 75 | `power_grid` | Shelter & Infrastructure | `PowerGridSystem` | `power_grid.json` | `PowerGridHostSession` | `PowerGridSaveStore` | `PowerGridPanel` | `--player-panels-uitest`, `PowerGridSystemTests` | ✅ 6/6 |
| 76 | `power_subgrids` | Shelter & Infrastructure | `PowerDistributionSubgridSystem` | — *(Procedural)* | `Main` | `PowerDistributionSaveStore` | `PowerGridPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 77 | `shelter_assignment` | Shelter & Infrastructure | `ShelterAssignmentSystem` | — *(Procedural)* | `ShelterAssignmentHostSession` | `ShelterAssignmentSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `ShelterAssignmentSystemTests` | ✅ 6/6 |
| 78 | `shelter_fire` | Shelter & Infrastructure | `ShelterFireHazardSystem` | — *(Procedural)* | `ShelterFireHostSession` | `ShelterFireSaveStore` | `FireIncidentPanel` | `--save-store-checksum-selftest`, `ShelterFireHazardSystemTests`, `FireIncidentJourneyTests` | ✅ 6/6 |
| 79 | `shelter_schedule` | Shelter & Infrastructure | `ShelterScheduleSystem` | `shelter_schedules.json` | `ShelterScheduleHostSession` | `ShelterScheduleSaveStore` | `ShelterSchedulePanel` | `--shelter-operations-selftest`, `ShelterScheduleIntegrationTests` | ✅ 6/6 |
| 80 | `shelter_thermal` | Shelter & Infrastructure | `ShelterThermalSystem` | — *(Procedural)* | `ShelterThermalHostSession` | `ShelterThermalSaveStore` | `ShelterThermalPanel` | `--shelter-operations-selftest`, `ShelterThermalSaveChecksumTests` | ✅ 6/6 |
| 81 | `starting_level` | Shelter & Infrastructure | `StartingLevelSystem` | — *(Procedural)* | `StartingLevelHostSession` | `StartingLevelSaveStore` | `OpeningProtocolModal` | `--playable-shell-selftest`, `StartingLevelSystemTests` | ✅ 6/6 |
| 82 | `sump_flooding` | Shelter & Infrastructure | `SumpFloodingSystem` | — *(Procedural)* | `SumpFloodingHostSession` | `SumpFloodingSaveStore` | `SumpFloodingPanel` | `--shelter-operations-selftest`, `SumpFloodingSaveChecksumTests` | ✅ 6/6 |
| 83 | `survivor_social` | Shelter & Infrastructure | `SurvivorSocialCoordinator`, `LeadershipSystem`, `IdeologicalFrictionSystem`, `RationConflictSystem`, `TraumaBondSystem`, `SkillAtrophySystem` | — *(Procedural)* | `SurvivorSocialCoordinator` | `SurvivorSocialSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `SurvivorSocialCoordinatorTests` | ✅ 6/6 |
| 84 | `vinyl_morale` | Shelter & Infrastructure | `VinylMoraleSystem` | — *(Procedural)* | `VinylMoraleHostSession` | `VinylMoraleSaveStore` | `VinylMoralePanel` | `--shelter-operations-selftest`, `VinylMoraleSaveChecksumTests` | ✅ 6/6 |
| 85 | `water_treatment` | Shelter & Infrastructure | `WaterTreatmentSystem` | — *(Procedural)* | `WaterTreatmentHostSession` | `WaterTreatmentSaveStore` | `WaterTreatmentPanel` | `--shelter-operations-selftest`, `WaterTreatmentSystemTests` | ✅ 6/6 |
| 86 | `crafting` | Shelter & Logistics | `CraftingSystem` | `recipes.json` | `CraftingHostSession` | `CraftingSaveStore` | `CraftingPanel` | `--shelter-operations-selftest`, `CraftingSystemTests` | ✅ 6/6 |
| 87 | `equipment_condition` | Shelter & Logistics | `EquipmentConditionSystem` | — *(Procedural)* | `EquipmentConditionHostSession` | `EquipmentConditionSaveStore` | `EquipmentConditionPanel` | `--shelter-operations-selftest`, `EquipmentConditionSystemTests` | ✅ 6/6 |
| 88 | `inventory` | Shelter & Logistics | `Inventory` | `items.json` | `InventoryHostSession` | `InventorySaveStore` | `InventoryPanel`, `InventoryDetailPanel` | `--inventory-save-selftest`, `--inventory-uitest`, `InventorySystemTests` | ✅ 6/6 |
| 89 | `kitchen_nutrition` | Shelter & Logistics | `KitchenNutritionSystem` | — *(Procedural)* | `KitchenNutritionHostSession` | `KitchenNutritionSaveStore` | `KitchenNutritionPanel` | `--shelter-operations-selftest`, `KitchenNutritionSystemTests` | ✅ 6/6 |
| 90 | `radio` | Shelter & Logistics | `FactionRadioEngine` | `radio.json` | `RadioHostSession` | `RadioSaveStore` | `RadioPanel`, `FactionRadioHudPanel` | `--radio-selftest`, `RadioSaveCodecTests` | ✅ 6/6 |
| 91 | `apprenticeship` | Survival & Biology | `ApprenticeshipSystem` | — *(Procedural)* | `ApprenticeshipHostSession` | `ApprenticeshipSaveStore` | `ApprenticeshipPanel` | `--shelter-operations-selftest`, `ApprenticeshipSystemTests` | ✅ 6/6 |
| 92 | `autopsy` | Survival & Biology | `AutopsySystem` | `autopsy_procedures.json` | `AutopsyHostSession` | `AutopsySaveStore` | `AutopsyReportPanel` | `--shelter-operations-selftest`, `AutopsySystemTests` | ✅ 6/6 |
| 93 | `caregiving` | Survival & Biology | `CaregivingSystem` | — *(Procedural)* | `CaregivingHostSession` | `CaregivingSaveStore` | `CaregivingPanel` | `--shelter-operations-selftest`, `CaregivingSystemTests` | ✅ 6/6 |
| 94 | `chemical_dependency` | Survival & Biology | `ChemicalDependencySystem` | `chemical_dependency_items.json` | `MentalHealthCrisisHostSession`, `ChemicalDependencyHostSession` | `ChemicalDependencySaveStore` | `ChemicalDependencyPanel` | `--chemical-dependency-save-selftest`, `ChemicalDependencySaveSealTests` | ✅ 6/6 |
| 95 | `contractor_roster` | Survival & Biology | `ContractorRosterSystem` | — *(Procedural)* | `ContractorRosterHostSession` | `ContractorRosterSaveStore` | `ContractorRosterPanel` | `--shelter-operations-selftest`, `ContractorRosterSystemTests` | ✅ 6/6 |
| 96 | `disease` | Survival & Biology | `DiseaseSystem` | `disease_catalog.json` | `DiseaseHostSession` | `DiseaseSaveStore` | `AfflictionsPanel` | `--disease-selftest`, `DiseaseSystemTests` | ✅ 6/6 |
| 97 | `medical` | Survival & Biology | `MedicalWardSystem`, `SickListSystem` | `medical_texts.json` | `MedicalHostSession` | `MedicalSaveStore` | `MedicalPanel`, `AfflictionsPanel` | `--medical-selftest`, `DwellerMedicalCatalogTests` | ✅ 6/6 |
| 98 | `medical_ward` | Survival & Biology | `MedicalWardSystem` | — *(Procedural)* | `MedicalWardHostSession` | `MedicalWardSaveStore` | `MedicalWardPanel` | `--medical-ward-save-selftest`, `MedicalWardSystemTests` | ✅ 6/6 |
| 99 | `mental_health_crisis` | Survival & Biology | `MentalHealthCrisisSystem` | — *(Procedural)* | `MentalHealthCrisisHostSession` | `MentalHealthCrisisSaveStore` | `MentalHealthCrisisPanel` | `--shelter-operations-selftest`, `MentalHealthCrisisSystemTests` | ✅ 6/6 |
| 100 | `morale_contagion` | Survival & Biology | `MoraleContagionSystem` | — *(Procedural)* | `MoraleContagionHostSession` | `MoraleContagionSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `MoraleContagionSystemTests` | ✅ 6/6 |
| 101 | `survivor_relations` | Survival & Biology | `SurvivorRelationsSystem` | — *(Procedural)* | `SurvivorRelationsHostSession` | `SurvivorRelationsSaveStore` | `SurvivorRelationsPanel` | `--shelter-operations-selftest`, `SurvivorRelationsSaveChecksumTests` | ✅ 6/6 |
| 102 | `survivors` | Survival & Biology | `NeedsSystem`, `SurvivorRosterSystem` | `survivors.json` | `SurvivorsHostSession` | `SurvivorsSaveStore` | `SurvivorsPanel`, `SurvivorDetailPanel`, `StatusPanel` | `--survivors-selftest`, `--survivors-uitest`, `--player-panels-uitest`, `NeedsSystemTests` | ✅ 6/6 |
| 103 | `combat` | Tactical Combat | `TacticalCombatSystem`, `CombatTraumaSystem` | `combat_catalog.json` | `CombatHostSession` | `CombatSaveStore` | `CombatPanel`, `CombatDetailPanel`, `CombatHistoryPanel` | `--combat-selftest`, `CombatBallisticsTests` | ✅ 6/6 |
| 104 | `ecological_infestation` | World | `EcologicalInfestationSystem` | `micro_locations.json` | `Main` | `EcologicalInfestationSaveStore` | `GameDashboardPanel` | `--faction-ecology-selftest`, `EcologicalInfestationSystemTests` | ✅ 6/6 |
| 105 | `armored_crawlers` | World & Expeditions | `ArmoredCrawlerExpeditionSystem` | `armored_crawler_modules.json` | `Main` | `ArmoredCrawlerSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ArmoredCrawlerExpeditionTests` | ✅ 6/6 |
| 106 | `encounter_choice` | World & Expeditions | `EncounterChoiceResolver` | `door_encounters.json` | `EncounterChoiceState` | `EncounterChoiceSaveStore` | `DoorEncounterModal` | `--moral-choice-selftest`, `EncounterChoiceResolverTests` | ✅ 6/6 |
| 107 | `expedition` | World & Expeditions | `ExpeditionSystem`, `ExpeditionEncounterBridge` | `locations.json` | `ExpeditionHostSession` | `ExpeditionSaveStore` | `ExpeditionPanel` | `--expedition-selftest`, `--expedition-panel-uitest`, `ExpeditionCampSystemTests` | ✅ 6/6 |
| 108 | `travel_encounters` | World & Expeditions | `TravelEncounterSystem`, `TravelEncounterCatalog` | `travel_encounters.json` | `TravelEncounterSystem` | `TravelEncounterSaveStore` | `ExpeditionPanel` | `--expedition-encounter-bridge-selftest`, `TravelEncounterCooldownGroupTests`, `PatrolEncounterFullRegressionTests` | ✅ 6/6 |
| 109 | `wasteland_map` | World & Expeditions | `WastelandMapSystem` | `wasteland_map_v1.json` | `WorldHostSession` | `WastelandMapSaveStore` | `MapPanel` | `--world-selftest`, `WastelandMapPersistenceTests` | ✅ 6/6 |
| 110 | `waystation` | World & Expeditions | `WaystationSystem` | `locations.json` | `WaystationHostSession` | `WaystationSaveStore` | `WaystationNetworkPanel` | `--shelter-operations-selftest`, `WaystationSystemTests` | ✅ 6/6 |
| 111 | `wildlife_trapping` | World & Expeditions | `WildlifeTrappingSystem` | — *(Procedural)* | `WildlifeTrappingHostSession` | `WildlifeTrappingSaveStore` | `WildlifeTrappingPanel` | `--shelter-operations-selftest`, `WildlifeTrappingSystemTests` | ✅ 6/6 |
| 112 | `world` | World & Expeditions | `WastelandMapSystem`, `WeatherSystem` | `locations.json` | `WorldHostSession` | `WorldSaveStore` | `MapPanel`, `WeatherPanel` | `--world-selftest`, `WorldSaveablesTests` | ✅ 6/6 |

---

## 3. Subsystem Deep Evidence Graph & Source Paths

Detailed file paths and symbols proving zero conceptual placeholders:

### 1. `endgame` — Campaign endgame phase, ending selection, sealed epilogue report (Campaign & Lore)
- **Owner Domain:** `endgame`
- **Setup Method:** `Main.SetupEndgame()` | **Cadence:** `On-Demand (Day Threshold / Extinction)`
- **UI Routes:** `epilogue`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs`](../../Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs)
  - Core System: [`Assets/Ashfall.Core/Endgame/EndgameSystem.cs`](../../Assets/Ashfall.Core/Endgame/EndgameSystem.cs)
  - Host Session: [`src/Host/EndgameHostSession.cs`](../../src/Host/EndgameHostSession.cs)
  - Save Store: [`src/Host/EndgameSaveStore.cs`](../../src/Host/EndgameSaveStore.cs)
  - UI Panel: [`src/UI/EpiloguePanel.cs`](../../src/UI/EpiloguePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Endgame/CampaignOutcomeEvaluatorTests.cs`](../../Ashfall.Core.Tests/Endgame/CampaignOutcomeEvaluatorTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Endgame/EndgameSystemTests.cs`](../../Ashfall.Core.Tests/Endgame/EndgameSystemTests.cs)

### 2. `host_event` — Host event ledger & moral decisions (Campaign & Lore)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupEventAdapter()` | **Cadence:** `On-Demand (Moral Dilemma)`
- **UI Routes:** `event_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Host Session: [`src/Host/HostEventAdapter.cs`](../../src/Host/HostEventAdapter.cs)
  - Save Store: [`src/Host/HostEventSaveStore.cs`](../../src/Host/HostEventSaveStore.cs)
  - Save Store: [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs)
  - UI Panel: [`src/UI/EventDetailPanel.cs`](../../src/UI/EventDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BareSaveStoreSealTests.cs`](../../Ashfall.Core.Tests/BareSaveStoreSealTests.cs)

### 3. `journal` — Player journal, logs, and codex entries (Campaign & Lore)
- **Owner Domain:** `journal`
- **Setup Method:** `Main.SetupJournal()` | **Cadence:** `On-Demand (Log/Event)`
- **UI Routes:** `journal`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Journal/JournalSystem.cs`](../../Assets/Ashfall.Core/Journal/JournalSystem.cs)
  - Host Session: [`src/Host/JournalHostSession.cs`](../../src/Host/JournalHostSession.cs)
  - Save Store: [`src/Journal/JournalSaveStore.cs`](../../src/Journal/JournalSaveStore.cs)
  - UI Panel: [`src/Journal/JournalBookUI.cs`](../../src/Journal/JournalBookUI.cs)
  - UI Panel: [`src/UI/JournalPanel.cs`](../../src/UI/JournalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/JournalSystemTests.cs`](../../Ashfall.Core.Tests/JournalSystemTests.cs)

### 4. `memorial` — Fallen survivors memorial wall (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupMemorial()` | **Cadence:** `On-Demand (Survivor Fallen Eulogy)`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Save Store: [`src/Host/MemorialSaveStore.cs`](../../src/Host/MemorialSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`](../../Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs)

### 5. `narrative` — Branching story arcs and narrative flags (Campaign & Lore)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupNarrative()` | **Cadence:** `On-Demand (Dialog Choice)`
- **UI Routes:** `journal`, `event_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs)
  - Host Session: [`src/Host/NarrativeHostSession.cs`](../../src/Host/NarrativeHostSession.cs)
  - Save Store: [`src/Host/NarrativeSaveStore.cs`](../../src/Host/NarrativeSaveStore.cs)
  - UI Panel: [`src/UI/EventsLogPanel.cs`](../../src/UI/EventsLogPanel.cs)
  - UI Panel: [`src/UI/FactionsNarrativePanel.cs`](../../src/UI/FactionsNarrativePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`](../../Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs)

### 6. `phase0` — Pre-war timeline and bunker startup (Campaign & Lore)
- **Owner Domain:** `phase0`
- **Setup Method:** `Main.SetupPhase0()` | **Cadence:** `On-Demand (Pre-War Flashback)`
- **UI Routes:** `phase0`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs`](../../Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs)
  - Host Session: [`src/Host/Phase0HostSession.cs`](../../src/Host/Phase0HostSession.cs)
  - Save Store: [`src/Host/Phase0SaveStore.cs`](../../src/Host/Phase0SaveStore.cs)
  - UI Panel: [`src/UI/Phase0Panel.cs`](../../src/UI/Phase0Panel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`](../../Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs)

### 7. `survivor_fate` — Unified survivor-death ledger: one immutable fate record per deceased survivor (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupSurvivorFate()` | **Cadence:** `Daily Survivor-Death Cascade`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorFateSaveStore.cs`](../../src/Host/SurvivorFateSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SurvivorFateSystemTests.cs`](../../Ashfall.Core.Tests/SurvivorFateSystemTests.cs)

### 8. `onboarding` — First-hour onboarding journey progress, dismissed hints, assistance level, completion (Campaign & Onboarding)
- **Owner Domain:** `onboarding`
- **Setup Method:** `Main.SetupOnboarding()` | **Cadence:** `On-Demand (Player Sigil Recording)`
- **UI Routes:** `help`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`](../../Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OnboardingSaveStore.cs`](../../src/Host/OnboardingSaveStore.cs)
  - UI Panel: [`src/UI/OnboardingHintPanel.cs`](../../src/UI/OnboardingHintPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/OnboardingJourneyTests.cs`](../../Ashfall.Core.Tests/OnboardingJourneyTests.cs)

### 9. `archive_desk` — Document archiving, ink, and scribing (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupArchiveDesk()` | **Cadence:** `Daily Scribing & Folio Archival`
- **UI Routes:** `archive_desk`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ArchiveDeskSystem.cs`](../../Assets/Ashfall.Core/ArchiveDeskSystem.cs)
  - Host Session: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - Save Store: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - UI Panel: [`src/UI/ArchiveDeskPanel.cs`](../../src/UI/ArchiveDeskPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`](../../Ashfall.Core.Tests/ArchiveDeskSystemTests.cs)

### 10. `campaign_day` — Master campaign day counter & ticks (Campaign & Progression)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCampaignDay()` | **Cadence:** `Master Sim Clock / Dawn Advance`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Host Session: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Save Store: [`src/Host/CampaignDaySaveStore.cs`](../../src/Host/CampaignDaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`](../../Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs)

### 11. `daily_briefing` — Daily dawn briefing notes & status (Campaign & Progression)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupDailyBriefingModal()` | **Cadence:** `Daily Dawn Briefing Aggregation`
- **UI Routes:** `briefing`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs)
  - Core System: [`Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs)
  - Host Session: [`Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs)
  - Save Store: [`src/Host/DailyBriefingSaveStore.cs`](../../src/Host/DailyBriefingSaveStore.cs)
  - UI Panel: [`src/UI/DailyBriefingModal.cs`](../../src/UI/DailyBriefingModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/DailyBriefingReportBuilderTests.cs`](../../Ashfall.Core.Tests/Campaign/DailyBriefingReportBuilderTests.cs)

### 12. `library_study` — Research library books and blueprints (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupLibraryStudy()` | **Cadence:** `Daily Codex Research Ticks`
- **UI Routes:** `library_study`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/LibraryStudySystem.cs`](../../Assets/Ashfall.Core/LibraryStudySystem.cs)
  - Host Session: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - Save Store: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - UI Panel: [`src/UI/LibraryStudyPanel.cs`](../../src/UI/LibraryStudyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/LibraryStudySystemTests.cs`](../../Ashfall.Core.Tests/LibraryStudySystemTests.cs)

### 13. `dynamic_quests` — Campaign-wide emergency dynamic quests (Campaign & Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupDynamicQuests()` | **Cadence:** `On-Demand (Campaign-Wide Emergency Quests)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/DynamicQuestlines.cs`](../../Assets/Ashfall.Core/Quests/DynamicQuestlines.cs)
  - Host Session: [`src/Host/DynamicQuestSaveStore.cs`](../../src/Host/DynamicQuestSaveStore.cs)
  - Save Store: [`src/Host/DynamicQuestSaveStore.cs`](../../src/Host/DynamicQuestSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs`](../../Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs)

### 14. `personal_quests` — Survivor personal quest progression (Campaign & Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupPersonalQuests()` | **Cadence:** `On-Demand (Survivor Quest Progression)`
- **UI Routes:** `quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs`](../../Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs)
  - Host Session: [`src/Host/PersonalQuestHostSession.cs`](../../src/Host/PersonalQuestHostSession.cs)
  - Save Store: [`src/Host/PersonalQuestSaveStore.cs`](../../src/Host/PersonalQuestSaveStore.cs)
  - UI Panel: [`src/UI/QuestDetailPanel.cs`](../../src/UI/QuestDetailPanel.cs)
  - UI Panel: [`src/UI/QuestsPanel.cs`](../../src/UI/QuestsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs`](../../Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs)

### 15. `perimeter_defense` — Surface perimeter defense emplacements (Combat & Defense)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupPerimeterDefense()` | **Cadence:** `Daily Emplacement Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs`](../../Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PerimeterDefenseSaveStore.cs`](../../src/Host/PerimeterDefenseSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs`](../../Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs)

### 16. `chemical_synthesis` — Chemical synthesis retorts and apparatus (Crafting & Chemistry)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupChemicalSynthesis()` | **Cadence:** `On-Demand (Retort Synthesis)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs`](../../Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs)
  - Host Session: [`src/Host/ChemicalSynthesisHostSession.cs`](../../src/Host/ChemicalSynthesisHostSession.cs)
  - Save Store: [`src/Host/ChemicalSynthesisSaveStore.cs`](../../src/Host/ChemicalSynthesisSaveStore.cs)
  - UI Panel: [`src/UI/ChemicalLabPanel.cs`](../../src/UI/ChemicalLabPanel.cs)

### 17. `caravan` — Trade caravans, routes, and arrivals (Economy & Trade)
- **Owner Domain:** `caravans`
- **Setup Method:** `Main.SetupCaravans()` | **Cadence:** `Daily Route Travel`
- **UI Routes:** `traveling_caravan`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/TravelingCaravanSystem.cs`](../../Assets/Ashfall.Core/TravelingCaravanSystem.cs)
  - Host Session: [`src/Host/TravelingCaravanHostSession.cs`](../../src/Host/TravelingCaravanHostSession.cs)
  - Save Store: [`src/Host/CaravanSaveStore.cs`](../../src/Host/CaravanSaveStore.cs)
  - UI Panel: [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TradeCaravanCatalogTests.cs`](../../Ashfall.Core.Tests/TradeCaravanCatalogTests.cs)

### 18. `caravan_trade_network` — Faction caravan trade network routes and arrivals (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupCaravanTrade()` | **Cadence:** `Daily Route Arrival Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs`](../../Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CaravanTradeSaveStore.cs`](../../src/Host/CaravanTradeSaveStore.cs)
  - UI Panel: [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs`](../../Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs)

### 19. `economy` — Dynamic economy rates and market orders (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupEconomy()` | **Cadence:** `Daily Market Rate Tick`
- **UI Routes:** `trade`, `economy_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/MarketSystem.cs`](../../Assets/Ashfall.Core/Economy/MarketSystem.cs)
  - Host Session: [`src/Host/EconomyHostSession.cs`](../../src/Host/EconomyHostSession.cs)
  - Save Store: [`src/Host/EconomySaveStore.cs`](../../src/Host/EconomySaveStore.cs)
  - UI Panel: [`src/Economy/EconomyMarketPanel.cs`](../../src/Economy/EconomyMarketPanel.cs)
  - UI Panel: [`src/UI/EconomyDetailPanel.cs`](../../src/UI/EconomyDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DynamicEconomyCharacterizationTests.cs`](../../Ashfall.Core.Tests/DynamicEconomyCharacterizationTests.cs)

### 20. `regional_treaty` — Faction treaties and non-aggression pacts (Economy & Trade)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupRegionalTreaty()` | **Cadence:** `Daily Non-Aggression Decay`
- **UI Routes:** `regional_treaty`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/RegionalTreatySystem.cs`](../../Assets/Ashfall.Core/RegionalTreatySystem.cs)
  - Host Session: [`src/Host/RegionalTreatyHostSession.cs`](../../src/Host/RegionalTreatyHostSession.cs)
  - Save Store: [`src/Host/RegionalTreatySaveStore.cs`](../../src/Host/RegionalTreatySaveStore.cs)
  - UI Panel: [`src/UI/RegionalTreatyPanel.cs`](../../src/UI/RegionalTreatyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 21. `expansion_hub` — Expansion hub discovery state (Expansion Framework)
- **Owner Domain:** `expansion_hub`
- **Setup Method:** `Main.SetupExpansions()` | **Cadence:** `Daily Hub Tick`
- **UI Routes:** `expansions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExpansionMasterSession.cs`](../../Assets/Ashfall.Core/ExpansionMasterSession.cs)
  - Host Session: [`src/Host/ExpansionHostSession.cs`](../../src/Host/ExpansionHostSession.cs)
  - Save Store: [`src/Host/ExpansionHubSaveStore.cs`](../../src/Host/ExpansionHubSaveStore.cs)
  - UI Panel: [`src/UI/ExpansionsHubPanel.cs`](../../src/UI/ExpansionsHubPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpansionHubSaveTests.cs`](../../Ashfall.Core.Tests/ExpansionHubSaveTests.cs)

### 22. `expansion_quest` — Expansion questline progression (Expansion Framework)
- **Owner Domain:** `expansion_quest`
- **Setup Method:** `Main.SetupExpansionQuests()` | **Cadence:** `On-Demand (Stage Milestone)`
- **UI Routes:** `crossing_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExpansionMasterSession.cs`](../../Assets/Ashfall.Core/ExpansionMasterSession.cs)
  - Core System: [`Assets/Ashfall.Core/ExpansionQuestSystem.cs`](../../Assets/Ashfall.Core/ExpansionQuestSystem.cs)
  - Host Session: [`src/Host/ExpansionQuestHostSession.cs`](../../src/Host/ExpansionQuestHostSession.cs)
  - Save Store: [`src/Host/ExpansionQuestSaveStore.cs`](../../src/Host/ExpansionQuestSaveStore.cs)
  - UI Panel: [`src/UI/CrossingQuestPanel.cs`](../../src/UI/CrossingQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/VersionReportContractTests.cs`](../../Ashfall.Core.Tests/VersionReportContractTests.cs)

### 23. `holdfast` — Holdfast S1 bunker state (Expansions (Exp 01))
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupHoldfastRuntime()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `holdfast`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HoldfastQuestSystem.cs`](../../Assets/Ashfall.Core/HoldfastQuestSystem.cs)
  - Core System: [`Assets/Ashfall.Core/HoldfastSession.cs`](../../Assets/Ashfall.Core/HoldfastSession.cs)
  - Host Session: [`src/Host/HoldfastRuntimeSession.cs`](../../src/Host/HoldfastRuntimeSession.cs)
  - Save Store: [`src/Host/HoldfastSaveStore.cs`](../../src/Host/HoldfastSaveStore.cs)
  - UI Panel: [`src/Host/HoldfastTerminalPanel.cs`](../../src/Host/HoldfastTerminalPanel.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HoldfastSaveTests.cs`](../../Ashfall.Core.Tests/HoldfastSaveTests.cs)

### 24. `holdfast_trade` — Holdfast trade session state (Expansions (Exp 01))
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupHoldfastRuntime()` | **Cadence:** `On-Demand (Barter)`
- **UI Routes:** `trade`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HoldfastTradeSession.cs`](../../Assets/Ashfall.Core/HoldfastTradeSession.cs)
  - Host Session: [`src/Host/HoldfastRuntimeSession.cs`](../../src/Host/HoldfastRuntimeSession.cs)
  - Save Store: [`src/Host/HoldfastTradeSaveStore.cs`](../../src/Host/HoldfastTradeSaveStore.cs)
  - UI Panel: [`src/Economy/TradeScreenGodotPanel.cs`](../../src/Economy/TradeScreenGodotPanel.cs)
  - UI Panel: [`src/Host/HoldfastTerminalPanel.cs`](../../src/Host/HoldfastTerminalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HoldfastTradeSessionTests.cs`](../../Ashfall.Core.Tests/HoldfastTradeSessionTests.cs)

### 25. `duty_roster` — Duty roster shifts and assignments (Expansions (Exp 02))
- **Owner Domain:** `duty_roster`
- **Setup Method:** `Main.SetupDutyRoster()` | **Cadence:** `Daily Shift Tick`
- **UI Routes:** `duty_roster`, `duty_roster_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`](../../Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs)
  - Host Session: [`src/Host/DutyRosterHostSession.cs`](../../src/Host/DutyRosterHostSession.cs)
  - Save Store: [`src/Host/DutyRosterSaveStore.cs`](../../src/Host/DutyRosterSaveStore.cs)
  - UI Panel: [`src/UI/DutyRosterDetailPanel.cs`](../../src/UI/DutyRosterDetailPanel.cs)
  - UI Panel: [`src/UI/DutyRosterPanel.cs`](../../src/UI/DutyRosterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DutyRosterSaveTests.cs`](../../Ashfall.Core.Tests/DutyRosterSaveTests.cs)

### 26. `phantom_memory` — Phantom memory lineages and echoes (Expansions (Exp 03))
- **Owner Domain:** `phase0`
- **Setup Method:** `Main.SetupPhantom()` | **Cadence:** `On-Demand (Scavenge Echo)`
- **UI Routes:** `standing_record`, `phantom_memory`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/PhantomMemoryEngine.cs`](../../Assets/Ashfall.Core/PhantomMemoryEngine.cs)
  - Host Session: [`src/Host/PhantomMemoryHostSession.cs`](../../src/Host/PhantomMemoryHostSession.cs)
  - Save Store: [`src/Host/PhantomMemorySaveStore.cs`](../../src/Host/PhantomMemorySaveStore.cs)
  - UI Panel: [`src/UI/PhantomMemoryPanel.cs`](../../src/UI/PhantomMemoryPanel.cs)
  - UI Panel: [`src/UI/StandingRecordPanel.cs`](../../src/UI/StandingRecordPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PhantomMemoryEngineTests.cs`](../../Ashfall.Core.Tests/PhantomMemoryEngineTests.cs)

### 27. `thirdonary` — Thirdonary covenant & dispute states (Expansions (Exp 04))
- **Owner Domain:** `thirdonary`
- **Setup Method:** `Main.SetupThirdonary()` | **Cadence:** `On-Demand (Arbitration)`
- **UI Routes:** `crossing_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Thirdonary/ThirdonaryQuestSystem.cs`](../../Assets/Ashfall.Core/Thirdonary/ThirdonaryQuestSystem.cs)
  - Host Session: [`src/Host/ThirdonaryHostSession.cs`](../../src/Host/ThirdonaryHostSession.cs)
  - Save Store: [`src/Host/ThirdonarySaveStore.cs`](../../src/Host/ThirdonarySaveStore.cs)
  - UI Panel: [`src/UI/CrossingQuestPanel.cs`](../../src/UI/CrossingQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`](../../Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs`](../../Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs)

### 28. `year_of_ash` — The Year of Ash harsh winter state (Expansions (Exp 05))
- **Owner Domain:** `year_of_ash`
- **Setup Method:** `Main.SetupYearOfAsh()` | **Cadence:** `Daily Deep-Freeze Tick`
- **UI Routes:** `door_encounter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs`](../../Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs)
  - Core System: [`Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs`](../../Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs)
  - Host Session: [`src/YearOfAsh/YearOfAshHostSession.cs`](../../src/YearOfAsh/YearOfAshHostSession.cs)
  - Save Store: [`src/YearOfAsh/YearOfAshSaveStore.cs`](../../src/YearOfAsh/YearOfAshSaveStore.cs)
  - UI Panel: [`src/YearOfAsh/DoorEncounterModal.cs`](../../src/YearOfAsh/DoorEncounterModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs`](../../Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs)

### 29. `muster` — The Muster military rally & conflict state (Expansions (Exp 06))
- **Owner Domain:** `muster`
- **Setup Method:** `Main.SetupMuster()` | **Cadence:** `On-Demand (Rally Stance)`
- **UI Routes:** `muster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Muster/MusterSystem.cs`](../../Assets/Ashfall.Core/Muster/MusterSystem.cs)
  - Host Session: [`src/Host/MusterHostSession.cs`](../../src/Host/MusterHostSession.cs)
  - Save Store: [`src/Host/MusterSaveStore.cs`](../../src/Host/MusterSaveStore.cs)
  - UI Panel: [`src/UI/MusterPanel.cs`](../../src/UI/MusterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MusterSystemTests.cs`](../../Ashfall.Core.Tests/MusterSystemTests.cs)

### 30. `dose_ledger` — Survivor radiation dose ledger & cohorts (Expansions (Exp 07))
- **Owner Domain:** `dose_ledger`
- **Setup Method:** `Main.SetupDoseLedger()` | **Cadence:** `On-Demand (Dose Log)`
- **UI Routes:** `radiation_history`, `radiation_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DoseLedgerSystem.cs`](../../Assets/Ashfall.Core/DoseLedgerSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`](../../Assets/Ashfall.Core/Radiation/RadiationSystem.cs)
  - Host Session: [`src/Host/DoseLedgerHostSession.cs`](../../src/Host/DoseLedgerHostSession.cs)
  - Save Store: [`src/Host/DoseLedgerSaveStore.cs`](../../src/Host/DoseLedgerSaveStore.cs)
  - UI Panel: [`src/UI/RadiationDetailPanel.cs`](../../src/UI/RadiationDetailPanel.cs)
  - UI Panel: [`src/UI/RadiationHistoryPanel.cs`](../../src/UI/RadiationHistoryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs`](../../Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs)

### 31. `verdict` — The Verdict investigation and tribunal state (Expansions (Exp 08))
- **Owner Domain:** `verdict`
- **Setup Method:** `Main.SetupVerdict()` | **Cadence:** `Daily Machine Log Tick`
- **UI Routes:** `verdict`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`](../../Assets/Ashfall.Core/Verdict/MachineLogSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`](../../Assets/Ashfall.Core/Verdict/ReckoningSystem.cs)
  - Host Session: [`src/Host/VerdictHostSession.cs`](../../src/Host/VerdictHostSession.cs)
  - Save Store: [`src/Host/VerdictSaveStore.cs`](../../src/Host/VerdictSaveStore.cs)
  - UI Panel: [`src/UI/VerdictDashboardPanel.cs`](../../src/UI/VerdictDashboardPanel.cs)
  - UI Panel: [`src/VerdictPanel.cs`](../../src/VerdictPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/VerdictChainTests.cs`](../../Ashfall.Core.Tests/VerdictChainTests.cs)

### 32. `maritime` — The Black Flotilla dives and naval wrecks (Expansions (Exp 09))
- **Owner Domain:** `maritime`
- **Setup Method:** `Main.SetupMaritime()` | **Cadence:** `On-Demand (Dive Sortie)`
- **UI Routes:** `maritime`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs)
  - Host Session: [`src/Host/MaritimeHostSession.cs`](../../src/Host/MaritimeHostSession.cs)
  - Save Store: [`src/Host/MaritimeSaveStore.cs`](../../src/Host/MaritimeSaveStore.cs)
  - UI Panel: [`src/UI/MaritimePanel.cs`](../../src/UI/MaritimePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BlackFlotillaTests.cs`](../../Ashfall.Core.Tests/BlackFlotillaTests.cs)

### 33. `silent_foundry` — Automated foundry machinery & smelters (Expansions (Exp 10))
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupSilentFoundry()` | **Cadence:** `Daily Smelter Cycle`
- **UI Routes:** `silent_foundry`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`](../../Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs)
  - Host Session: [`src/Foundry/SilentFoundryHostSession.cs`](../../src/Foundry/SilentFoundryHostSession.cs)
  - Save Store: [`src/Host/SilentFoundrySaveStore.cs`](../../src/Host/SilentFoundrySaveStore.cs)
  - UI Panel: [`src/UI/SilentFoundryPanel.cs`](../../src/UI/SilentFoundryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`](../../Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs)

### 34. `weight_of_choices` — Weight of choices faction branch progression and PoNR commitments (Factions & Diplomacy)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupFactionBranch()` | **Cadence:** `On-Demand (Branch Decisions)`
- **UI Routes:** `factions`, `quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`](../../Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs`](../../Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs`](../../Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs`](../../Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Factions/RebelBranchSystem.cs`](../../Assets/Ashfall.Core/Factions/RebelBranchSystem.cs)
  - Host Session: [`src/Host/FactionBranchHostSession.cs`](../../src/Host/FactionBranchHostSession.cs)
  - Save Store: [`src/Host/WeightOfChoicesSaveStore.cs`](../../src/Host/WeightOfChoicesSaveStore.cs)
  - UI Panel: [`src/UI/FactionsPanel.cs`](../../src/UI/FactionsPanel.cs)
  - UI Panel: [`src/UI/QuestsPanel.cs`](../../src/UI/QuestsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs`](../../Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/IndependentBranchSystemTests.cs`](../../Ashfall.Core.Tests/IndependentBranchSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MilitaryBranchSystemTests.cs`](../../Ashfall.Core.Tests/MilitaryBranchSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PrpfStandingSystemTests.cs`](../../Ashfall.Core.Tests/PrpfStandingSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/RebelBranchSystemTests.cs`](../../Ashfall.Core.Tests/RebelBranchSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WeightOfChoicesSaveTests.cs`](../../Ashfall.Core.Tests/WeightOfChoicesSaveTests.cs)

### 35. `collectible_discovery` — One-time collectible discovery ledger (Inventory & Lore)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupCollectibles()` | **Cadence:** `On-Demand (One-Time Discovery Ledger)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/CollectibleDiscoveryState.cs`](../../Assets/Ashfall.Core/CollectibleDiscoveryState.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CollectibleDiscoverySaveStore.cs`](../../src/Host/CollectibleDiscoverySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`](../../Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs)

### 36. `unique_claims` — Global unique-item claim ledger (Inventory & Lore)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupCollectibles()` | **Cadence:** `On-Demand (Global Unique Claim Ledger)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/UniqueItemClaimRegistry.cs`](../../Assets/Ashfall.Core/UniqueItemClaimRegistry.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/UniqueClaimSaveStore.cs`](../../src/Host/UniqueClaimSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`](../../Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs)

### 37. `field_guide` — Plan 20A/28 — field-guide unlocked-entry ledger (reading-the-land knowledge) (Knowledge)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupFieldGuide()` | **Cadence:** `On-Demand (Study & Discovery)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/FieldGuideCatalog.cs`](../../Assets/Ashfall.Core/World/FieldGuideCatalog.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FieldGuideSaveStore.cs`](../../src/Host/FieldGuideSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/FieldGuidePersistenceTests.cs`](../../Ashfall.Core.Tests/FieldGuidePersistenceTests.cs)

### 38. `research` — Research knowledge progress: unlocked, active, and completed nodes (Plan 34) (Knowledge)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.None()` | **Cadence:** `On-Demand (Study Progress)`
- **UI Routes:** `research`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Research/ResearchSystem.cs`](../../Assets/Ashfall.Core/Research/ResearchSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ResearchSaveStore.cs`](../../src/Host/ResearchSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ResearchPanel.cs`](../../src/UI/ResearchPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs)

### 39. `medical_pipeline` — Diagnosis knowledge, treatment reservations, scheduled procedures (Task #133) (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedical()` | **Cadence:** `On-Demand (Triage & Procedure Commands)`
- **UI Routes:** `medical`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`](../../Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MedicalPipelineSaveStore.cs`](../../src/Host/MedicalPipelineSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs)

### 40. `pathogen_strains` — Flagship XI Plan 155 — fictional strain layer: cure projects and unlocked cures (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupPathogenStrains()` | **Cadence:** `Daily Strain Progression Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs`](../../Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PathogenStrainSaveStore.cs`](../../src/Host/PathogenStrainSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiseaseSystemTests.cs`](../../Ashfall.Core.Tests/DiseaseSystemTests.cs)

### 41. `surgical_ward` — Advanced surgical ward operations and sterile field (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupSurgicalWard()` | **Cadence:** `Daily Sterile Field Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurgicalWardSaveStore.cs`](../../src/Host/SurgicalWardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/AdvancedSurgicalWardTests.cs`](../../Ashfall.Core.Tests/Medical/AdvancedSurgicalWardTests.cs)

### 42. `moral_choice` — Moral choice ledger and community trust (Narrative & Decisions)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupMoralChoice()` | **Cadence:** `On-Demand (Branch Choice)`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs)
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Save Store: [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MoralChoiceSystemTests.cs`](../../Ashfall.Core.Tests/MoralChoiceSystemTests.cs)

### 43. `amputation` — Infection progression, amputations, prosthetics and bionics (Plans 178-201 Expansion Block)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupAmputation()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `medical`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/AmputationSystem.cs`](../../Assets/Ashfall.Core/Medical/AmputationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AmputationSaveStore.cs`](../../src/Host/AmputationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/AmputationSystemTests.cs`](../../Ashfall.Core.Tests/Medical/AmputationSystemTests.cs)

### 44. `archaeology` — Archaeology excavation ruins, archive decryption, and lore unlocks (Plans 178-201 Expansion Block)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupArchaeology()` | **Cadence:** `On-Demand (Excavation & Decryption)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`](../../Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ArchaeologySaveStore.cs`](../../src/Host/ArchaeologySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`](../../Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs)

### 45. `aviation` — Aviation airframes, flight plans, aerial mapping, and crash rescue (Plans 178-201 Expansion Block)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupAviation()` | **Cadence:** `Daily Flight Tick`
- **UI Routes:** `aviation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/AviationSystem.cs`](../../Assets/Ashfall.Core/Expeditions/AviationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AviationSaveStore.cs`](../../src/Host/AviationSaveStore.cs)
  - UI Panel: [`src/UI/AviationUI.cs`](../../src/UI/AviationUI.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/AviationSystemTests.cs`](../../Ashfall.Core.Tests/Expeditions/AviationSystemTests.cs)

### 46. `ceremony` — Communal ceremonies, festivals, truces, and morale (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupCeremony()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/CeremonySystem.cs`](../../Assets/Ashfall.Core/Narrative/CeremonySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CeremonySaveStore.cs`](../../src/Host/CeremonySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/CeremonySystemTests.cs`](../../Ashfall.Core.Tests/Narrative/CeremonySystemTests.cs)

### 47. `chem_warfare` — CBRN hazard warfare and toxic contamination (Plans 178-201 Expansion Block)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupChemWarfare()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/ChemWarfareSystem.cs`](../../Assets/Ashfall.Core/Combat/ChemWarfareSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ChemWarfareSaveStore.cs`](../../src/Host/ChemWarfareSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs`](../../Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs)

### 48. `child_development` — Child development phases, education, trauma, and adulthood (Plans 178-201 Expansion Block)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupGenerational()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `nursery`, `century_seed`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/GenerationalSystem.cs`](../../Assets/Ashfall.Core/Survivors/GenerationalSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/GenerationalSaveStore.cs`](../../src/Host/GenerationalSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/NurseryPanel.cs`](../../src/UI/NurseryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GenerationalLineageExtensionTests.cs`](../../Ashfall.Core.Tests/GenerationalLineageExtensionTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/GenerationalSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/GenerationalSystemTests.cs)

### 49. `comms_array` — Long-range communications array and satellite telemetry (Plans 178-201 Expansion Block)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupCommsArray()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/CommsArraySystem.cs`](../../Assets/Ashfall.Core/World/CommsArraySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CommsArraySaveStore.cs`](../../src/Host/CommsArraySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/CommsArraySystemTests.cs`](../../Ashfall.Core.Tests/World/CommsArraySystemTests.cs)

### 50. `desperation` — Starvation crisis desperation acts and cannibalism history (Plans 178-201 Expansion Block)
- **Owner Domain:** `survival`
- **Setup Method:** `Main.SetupDesperation()` | **Cadence:** `On-Demand (Crisis Command)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/DesperationSystem.cs`](../../Assets/Ashfall.Core/Survivors/DesperationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DesperationSaveStore.cs`](../../src/Host/DesperationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/DesperationSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/DesperationSystemTests.cs)

### 51. `expedition_stealth` — Expedition stealth, detection risk, camouflage, and night ops (Plans 178-201 Expansion Block)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupStealth()` | **Cadence:** `Event-Driven (Expedition Phases)`
- **UI Routes:** `stealth`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/StealthSystem.cs`](../../Assets/Ashfall.Core/Combat/StealthSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/StealthSaveStore.cs`](../../src/Host/StealthSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/StealthReadoutPanel.cs`](../../src/UI/StealthReadoutPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/StealthSystemTests.cs`](../../Ashfall.Core.Tests/Combat/StealthSystemTests.cs)

### 52. `fallout` — Radioactive fallout clouds, dispersal, and shelter sealing (Plans 178-201 Expansion Block)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupFallout()` | **Cadence:** `Hourly Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/FalloutSystem.cs`](../../Assets/Ashfall.Core/World/FalloutSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FalloutSaveStore.cs`](../../src/Host/FalloutSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/FalloutSystemTests.cs`](../../Ashfall.Core.Tests/World/FalloutSystemTests.cs)

### 53. `forced_labor` — Captive forced labor assignments, cruelty index, and rebellion risks (Plans 178-201 Expansion Block)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupForcedLabor()` | **Cadence:** `Daily Shift Tick`
- **UI Routes:** `forced_labor`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/ForcedLaborSystem.cs`](../../Assets/Ashfall.Core/Factions/ForcedLaborSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ForcedLaborSaveStore.cs`](../../src/Host/ForcedLaborSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/LaborUI.cs`](../../src/UI/LaborUI.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/ForcedLaborSystemTests.cs`](../../Ashfall.Core.Tests/Factions/ForcedLaborSystemTests.cs)

### 54. `fungi_cultivation` — Subterranean fungi beds, substrate, spores, and blooms (Plans 178-201 Expansion Block)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupFungi()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs`](../../Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FungiSaveStore.cs`](../../src/Host/FungiSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Farming/FungiCultivationSystemTests.cs`](../../Ashfall.Core.Tests/Farming/FungiCultivationSystemTests.cs)

### 55. `mercenary_bounties` — Mercenary bounty contracts, target intel, and rival tracking (Plans 178-201 Expansion Block)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupMercenary()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/MercenarySystem.cs`](../../Assets/Ashfall.Core/Economy/MercenarySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MercenarySaveStore.cs`](../../src/Host/MercenarySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/MercenarySystemTests.cs`](../../Ashfall.Core.Tests/Economy/MercenarySystemTests.cs)

### 56. `mutation_tree` — Radiation exposure, genetic instability, and mutation trees (Plans 178-201 Expansion Block)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMutations()` | **Cadence:** `Event-Driven (Dose Thresholds)`
- **UI Routes:** `mutation_tree`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MutationSystem.cs`](../../Assets/Ashfall.Core/Medical/MutationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MutationSaveStore.cs`](../../src/Host/MutationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/MutationTreePanel.cs`](../../src/UI/MutationTreePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MutationSystemTests.cs`](../../Ashfall.Core.Tests/Medical/MutationSystemTests.cs)

### 57. `narcotics` — Chemical medicines, toxicity, tolerance, addiction, and rehab beds (Plans 178-201 Expansion Block)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupNarcotics()` | **Cadence:** `24h Medical Tick`
- **UI Routes:** `narcotics`, `pharma_lab`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/NarcoticsSystem.cs`](../../Assets/Ashfall.Core/Medical/NarcoticsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/NarcoticsSaveStore.cs`](../../src/Host/NarcoticsSaveStore.cs)
  - UI Panel: [`src/UI/ChemUI.cs`](../../src/UI/ChemUI.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PharmaLabPanel.cs`](../../src/UI/PharmaLabPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs`](../../Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs)

### 58. `prisoner_management` — Captive detention, upkeep, interrogation, escape, and recruitment (Plans 178-201 Expansion Block)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupPrisoners()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `prisoners`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/PrisonerSystem.cs`](../../Assets/Ashfall.Core/Factions/PrisonerSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PrisonerSaveStore.cs`](../../src/Host/PrisonerSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PrisonerPanel.cs`](../../src/UI/PrisonerPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/PrisonerSystemTests.cs`](../../Ashfall.Core.Tests/Factions/PrisonerSystemTests.cs)

### 59. `railway` — Rail network, track repair, and armored train operations (Plans 178-201 Expansion Block)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupRailway()` | **Cadence:** `On-Demand (Convoy Operations)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/RailwaySystem.cs`](../../Assets/Ashfall.Core/Expeditions/RailwaySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RailwaySaveStore.cs`](../../src/Host/RailwaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/RailwaySystemTests.cs`](../../Ashfall.Core.Tests/Expeditions/RailwaySystemTests.cs)

### 60. `recreation` — Survivor hobbies, downtime, and recreation (Plans 178-201 Expansion Block)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupRecreation()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs`](../../Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RecreationSaveStore.cs`](../../src/Host/RecreationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Recreation/SurvivorDowntimeSystemTests.cs`](../../Ashfall.Core.Tests/Recreation/SurvivorDowntimeSystemTests.cs)

### 61. `robotics` — Pre-war robotics, directives, and automation (Plans 178-201 Expansion Block)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupRobotics()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/RoboticsSystem.cs`](../../Assets/Ashfall.Core/Crafting/RoboticsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RoboticsSaveStore.cs`](../../src/Host/RoboticsSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Crafting/RoboticsSystemTests.cs`](../../Ashfall.Core.Tests/Crafting/RoboticsSystemTests.cs)

### 62. `settlement_politics` — Settlement elections, political policies, approval rating, and coups (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupPolitics()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `politics`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/PoliticsSystem.cs`](../../Assets/Ashfall.Core/Narrative/PoliticsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PoliticsSaveStore.cs`](../../src/Host/PoliticsSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PoliticsUI.cs`](../../src/UI/PoliticsUI.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/PoliticsSystemTests.cs`](../../Ashfall.Core.Tests/Narrative/PoliticsSystemTests.cs)

### 63. `wasteland_justice` — Crime incidents, trials, punishments, banishments, and grudges (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupJustice()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/JusticeSystem.cs`](../../Assets/Ashfall.Core/Narrative/JusticeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/JusticeSaveStore.cs`](../../src/Host/JusticeSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs`](../../Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs)

### 64. `excavation_hazards` — Subterranean methane, flood, spore hazards, and cave-in rescue operations (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupExcavationHazards()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`](../../Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ExcavationHazardSaveStore.cs`](../../src/Host/ExcavationHazardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExcavationSystemTests.cs`](../../Ashfall.Core.Tests/ExcavationSystemTests.cs)

### 65. `radio_station` — Radio station frequency tuning, signal lock, and triangulation (Shelter)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadioStation()` | **Cadence:** `On-Demand (Tuning & Broadcasts)`
- **UI Routes:** `radio`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`](../../Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RadioStationSaveStore.cs`](../../src/Host/RadioStationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs`](../../Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs)

### 66. `shelter_decor` — Room decor placements, memorial plaques, and localized morale items (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterDecor()` | **Cadence:** `On-Demand (Decoration Placement)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs)
  - Host Session: [`src/Host/ShelterDecorHostSession.cs`](../../src/Host/ShelterDecorHostSession.cs)
  - Save Store: [`src/Host/ShelterDecorSaveStore.cs`](../../src/Host/ShelterDecorSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan12CDecorTests.cs`](../../Ashfall.Core.Tests/Plan12CDecorTests.cs)

### 67. `shelter_social_dynamics` — Living quarters privacy pressure, communal mess hall, and disputes (Shelter)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupShelterSocial()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterSocialSaveStore.cs`](../../src/Host/ShelterSocialSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterSocialDynamicsTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterSocialDynamicsTests.cs)

### 68. `shelter_workshop` — Precision workshop tooling, ammo press, and firearm refurbishment (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupWorkshop()` | **Cadence:** `On-Demand (Crafting & Refurbishment)`
- **UI Routes:** `workshop`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterWorkshopSaveStore.cs`](../../src/Host/ShelterWorkshopSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/WorkshopPanel.cs`](../../src/UI/WorkshopPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`](../../Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs)

### 69. `hydroponic_biomes` — Hydroponic biome racks and crop state (Shelter & Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupHydroponicBiomes()` | **Cadence:** `Daily Biome Rack Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs`](../../Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HydroponicBiomeSaveStore.cs`](../../src/Host/HydroponicBiomeSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/HydroponicBiomeTests.cs`](../../Ashfall.Core.Tests/Shelter/HydroponicBiomeTests.cs)

### 70. `airlock_security` — Airlock decontamination and security (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupAirlockSecurity()` | **Cadence:** `Daily Decon Interlock`
- **UI Routes:** `airlock_security`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AirlockSecuritySystem.cs`](../../Assets/Ashfall.Core/AirlockSecuritySystem.cs)
  - Host Session: [`src/Host/AirlockSecurityHostSession.cs`](../../src/Host/AirlockSecurityHostSession.cs)
  - Save Store: [`src/Host/AirlockSecuritySaveStore.cs`](../../src/Host/AirlockSecuritySaveStore.cs)
  - UI Panel: [`src/UI/AirlockSecurityPanel.cs`](../../src/UI/AirlockSecurityPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AirlockSecuritySystemTests.cs`](../../Ashfall.Core.Tests/AirlockSecuritySystemTests.cs)

### 71. `decontamination` — Rad-scrubbing showers and chambers (Shelter & Infrastructure)
- **Owner Domain:** `radiation`
- **Setup Method:** `Main.SetupDecontamination()` | **Cadence:** `Daily Rad Scrub Shower Cycle`
- **UI Routes:** `decontamination`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DecontaminationSystem.cs`](../../Assets/Ashfall.Core/DecontaminationSystem.cs)
  - Host Session: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - Save Store: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - UI Panel: [`src/UI/DecontaminationPanel.cs`](../../src/UI/DecontaminationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DecontaminationSystemTests.cs`](../../Ashfall.Core.Tests/DecontaminationSystemTests.cs)

### 72. `excavation` — Shelter expansion rubble clearing (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupExcavation()` | **Cadence:** `Daily Rubble Shoring Work`
- **UI Routes:** `excavation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExcavationSystem.cs`](../../Assets/Ashfall.Core/ExcavationSystem.cs)
  - Host Session: [`src/Host/ExcavationHostSession.cs`](../../src/Host/ExcavationHostSession.cs)
  - Save Store: [`src/Host/ExcavationSaveStore.cs`](../../src/Host/ExcavationSaveStore.cs)
  - UI Panel: [`src/UI/ExcavationPanel.cs`](../../src/UI/ExcavationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExcavationSystemTests.cs`](../../Ashfall.Core.Tests/ExcavationSystemTests.cs)

### 73. `greenhouse` — Hydroponic crops and food production (Shelter & Infrastructure)
- **Owner Domain:** `greenhouse`
- **Setup Method:** `Main.SetupGreenhouse()` | **Cadence:** `Daily Hydroponic Growth`
- **UI Routes:** `greenhouse`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`](../../Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs)
  - Host Session: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - Save Store: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - UI Panel: [`src/UI/GreenhousePanel.cs`](../../src/UI/GreenhousePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GreenhouseSystemTests.cs`](../../Ashfall.Core.Tests/GreenhouseSystemTests.cs)

### 74. `nuclear_core_lifecycle` — Nuclear core lifecycle and thermal state (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupNuclearCore()` | **Cadence:** `Daily Core Thermal Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs`](../../Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/NuclearCoreSaveStore.cs`](../../src/Host/NuclearCoreSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/NuclearCoreLifecycleTests.cs`](../../Ashfall.Core.Tests/Shelter/NuclearCoreLifecycleTests.cs)

### 75. `power_grid` — Shelter generator & power allocations (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupPowerGrid()` | **Cadence:** `Daily Fuel Consumption & Wattage`
- **UI Routes:** `power_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`](../../Assets/Ashfall.Core/Shelter/PowerGridSystem.cs)
  - Host Session: [`src/Host/PowerGridHostSession.cs`](../../src/Host/PowerGridHostSession.cs)
  - Save Store: [`src/Host/PowerGridSaveStore.cs`](../../src/Host/PowerGridSaveStore.cs)
  - UI Panel: [`src/UI/PowerGridPanel.cs`](../../src/UI/PowerGridPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs)

### 76. `power_subgrids` — Power distribution sub-grid nodes and thermal state (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupPowerSubgrids()` | **Cadence:** `Daily Thermal Distribution Tick`
- **UI Routes:** `power_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PowerDistributionSubgridSystem.cs`](../../Assets/Ashfall.Core/Shelter/PowerDistributionSubgridSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PowerDistributionSaveStore.cs`](../../src/Host/PowerDistributionSaveStore.cs)
  - UI Panel: [`src/UI/PowerGridPanel.cs`](../../src/UI/PowerGridPanel.cs)

### 77. `shelter_assignment` — Room assignments and living quarters (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAssignment()` | **Cadence:** `On-Demand (Bunk Reassignment)`
- **UI Routes:** `shelter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs)
  - Host Session: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - Save Store: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - UI Panel: [`src/UI/ShelterPanel.cs`](../../src/UI/ShelterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs)

### 78. `shelter_fire` — Shelter fire incidents, smoke, and brigade response (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterFireHazard()` | **Cadence:** `Daily Fire Propagation Tick`
- **UI Routes:** `fire_incident`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs)
  - Host Session: [`src/Host/ShelterFireHostSession.cs`](../../src/Host/ShelterFireHostSession.cs)
  - Save Store: [`src/Host/ShelterFireSaveStore.cs`](../../src/Host/ShelterFireSaveStore.cs)
  - UI Panel: [`src/UI/FireIncidentPanel.cs`](../../src/UI/FireIncidentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`](../../Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ShelterFireHazardSystemTests.cs`](../../Ashfall.Core.Tests/ShelterFireHazardSystemTests.cs)

### 79. `shelter_schedule` — Shift rotations and curfews (Shelter & Infrastructure)
- **Owner Domain:** `schedule`
- **Setup Method:** `Main.SetupShelterSchedule()` | **Cadence:** `Daily Curfew Rotation`
- **UI Routes:** `shelter_schedule`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterScheduleSystem.cs`](../../Assets/Ashfall.Core/ShelterScheduleSystem.cs)
  - Host Session: [`src/Host/ShelterScheduleHostSession.cs`](../../src/Host/ShelterScheduleHostSession.cs)
  - Save Store: [`src/Host/ShelterScheduleSaveStore.cs`](../../src/Host/ShelterScheduleSaveStore.cs)
  - UI Panel: [`src/UI/ShelterSchedulePanel.cs`](../../src/UI/ShelterSchedulePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs`](../../Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs)

### 80. `shelter_thermal` — Heating, insulation, and frost protection (Shelter & Infrastructure)
- **Owner Domain:** `thermal`
- **Setup Method:** `Main.SetupShelterThermal()` | **Cadence:** `Daily HVAC Frost Dissipation`
- **UI Routes:** `shelter_thermal`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterThermalSystem.cs`](../../Assets/Ashfall.Core/ShelterThermalSystem.cs)
  - Host Session: [`src/Host/ShelterThermalHostSession.cs`](../../src/Host/ShelterThermalHostSession.cs)
  - Save Store: [`src/Host/ShelterThermalSaveStore.cs`](../../src/Host/ShelterThermalSaveStore.cs)
  - UI Panel: [`src/UI/ShelterThermalPanel.cs`](../../src/UI/ShelterThermalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 81. `starting_level` — Bunker initial configuration & tier (Shelter & Infrastructure)
- **Owner Domain:** `starting_level`
- **Setup Method:** `Main.SetupStartingLevel()` | **Cadence:** `On-Demand (Opening Protocol)`
- **UI Routes:** `protocol`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`](../../Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs)
  - Host Session: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - Save Store: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - UI Panel: [`src/UI/OpeningProtocolModal.cs`](../../src/UI/OpeningProtocolModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/StartingLevelSystemTests.cs`](../../Ashfall.Core.Tests/StartingLevelSystemTests.cs)

### 82. `sump_flooding` — Bunker sump pump drainage & flood risk (Shelter & Infrastructure)
- **Owner Domain:** `maintenance`
- **Setup Method:** `Main.SetupSumpFlooding()` | **Cadence:** `Daily Drainage Pump Work`
- **UI Routes:** `sump_flooding`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SumpFloodingSystem.cs`](../../Assets/Ashfall.Core/SumpFloodingSystem.cs)
  - Host Session: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - Save Store: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - UI Panel: [`src/UI/SumpFloodingPanel.cs`](../../src/UI/SumpFloodingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs`](../../Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs)

### 83. `survivor_social` — Leadership, friction, ration conflict, trauma bonds, skill atrophy (Shelter & Infrastructure)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupSurvivorSocial()` | **Cadence:** `Daily Shelter Social Dynamics`
- **UI Routes:** `shelter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs`](../../Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`](../../Assets/Ashfall.Core/Survivors/LeadershipSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/RationConflictSystem.cs`](../../Assets/Ashfall.Core/Survivors/RationConflictSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`](../../Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs`](../../Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs)
  - Save Store: [`src/Host/SurvivorSocialSaveStore.cs`](../../src/Host/SurvivorSocialSaveStore.cs)
  - UI Panel: [`src/UI/ShelterPanel.cs`](../../src/UI/ShelterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs`](../../Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs)

### 84. `vinyl_morale` — Gramophone records and music morale (Shelter & Infrastructure)
- **Owner Domain:** `morale`
- **Setup Method:** `Main.SetupVinylMorale()` | **Cadence:** `Daily Turntable Morale Broadcast`
- **UI Routes:** `vinyl_morale`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/VinylMoraleSystem.cs`](../../Assets/Ashfall.Core/VinylMoraleSystem.cs)
  - Host Session: [`src/Host/VinylMoraleHostSession.cs`](../../src/Host/VinylMoraleHostSession.cs)
  - Save Store: [`src/Host/VinylMoraleSaveStore.cs`](../../src/Host/VinylMoraleSaveStore.cs)
  - UI Panel: [`src/UI/VinylMoralePanel.cs`](../../src/UI/VinylMoralePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 85. `water_treatment` — Water filtration and purification (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaterTreatment()` | **Cadence:** `Daily Filtration Cycle`
- **UI Routes:** `water_treatment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaterTreatmentSystem.cs`](../../Assets/Ashfall.Core/WaterTreatmentSystem.cs)
  - Host Session: [`src/Host/WaterTreatmentHostSession.cs`](../../src/Host/WaterTreatmentHostSession.cs)
  - Save Store: [`src/Host/WaterTreatmentSaveStore.cs`](../../src/Host/WaterTreatmentSaveStore.cs)
  - UI Panel: [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaterTreatmentSystemTests.cs`](../../Ashfall.Core.Tests/WaterTreatmentSystemTests.cs)

### 86. `crafting` — Known recipes and workbench queues (Shelter & Logistics)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupCrafting()` | **Cadence:** `Daily Workbench Queue`
- **UI Routes:** `crafting`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/CraftingSystem.cs`](../../Assets/Ashfall.Core/Crafting/CraftingSystem.cs)
  - Host Session: [`src/Host/CraftingHostSession.cs`](../../src/Host/CraftingHostSession.cs)
  - Save Store: [`src/Host/CraftingSaveStore.cs`](../../src/Host/CraftingSaveStore.cs)
  - UI Panel: [`src/UI/CraftingPanel.cs`](../../src/UI/CraftingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CraftingSystemTests.cs`](../../Ashfall.Core.Tests/CraftingSystemTests.cs)

### 87. `equipment_condition` — Tool and weapon wear/repair (Shelter & Logistics)
- **Owner Domain:** `equipment`
- **Setup Method:** `Main.SetupEquipmentCondition()` | **Cadence:** `Daily Gear Wear & Maintenance`
- **UI Routes:** `equipment_condition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/EquipmentConditionSystem.cs`](../../Assets/Ashfall.Core/EquipmentConditionSystem.cs)
  - Host Session: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - Save Store: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - UI Panel: [`src/UI/EquipmentConditionPanel.cs`](../../src/UI/EquipmentConditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EquipmentConditionSystemTests.cs`](../../Ashfall.Core.Tests/EquipmentConditionSystemTests.cs)

### 88. `inventory` — Shelter warehouse & items storage (Shelter & Logistics)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupInventory()` | **Cadence:** `On-Demand (Item Use)`
- **UI Routes:** `inventory`, `inventory_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Inventory/Inventory.cs`](../../Assets/Ashfall.Core/Inventory/Inventory.cs)
  - Host Session: [`src/Host/InventoryHostSession.cs`](../../src/Host/InventoryHostSession.cs)
  - Save Store: [`src/Host/InventorySaveStore.cs`](../../src/Host/InventorySaveStore.cs)
  - UI Panel: [`src/UI/InventoryDetailPanel.cs`](../../src/UI/InventoryDetailPanel.cs)
  - UI Panel: [`src/UI/InventoryPanel.cs`](../../src/UI/InventoryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/InventorySystemTests.cs`](../../Ashfall.Core.Tests/InventorySystemTests.cs)

### 89. `kitchen_nutrition` — Rationing recipes and caloric balance (Shelter & Logistics)
- **Owner Domain:** `nutrition`
- **Setup Method:** `Main.SetupKitchenNutrition()` | **Cadence:** `Daily Rationing Meal Prep`
- **UI Routes:** `kitchen_nutrition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/KitchenNutritionSystem.cs`](../../Assets/Ashfall.Core/KitchenNutritionSystem.cs)
  - Host Session: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - Save Store: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - UI Panel: [`src/UI/KitchenNutritionPanel.cs`](../../src/UI/KitchenNutritionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/KitchenNutritionSystemTests.cs`](../../Ashfall.Core.Tests/KitchenNutritionSystemTests.cs)

### 90. `radio` — Radio frequencies, logs, and distress signals (Shelter & Logistics)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadio()` | **Cadence:** `On-Demand (Frequency Scan)`
- **UI Routes:** `radio`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`](../../Assets/Ashfall.Core/Radio/FactionRadioEngine.cs)
  - Host Session: [`src/Host/RadioHostSession.cs`](../../src/Host/RadioHostSession.cs)
  - Save Store: [`src/Host/RadioSaveStore.cs`](../../src/Host/RadioSaveStore.cs)
  - UI Panel: [`src/Radio/FactionRadioHudPanel.cs`](../../src/Radio/FactionRadioHudPanel.cs)
  - UI Panel: [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/RadioSaveCodecTests.cs`](../../Ashfall.Core.Tests/RadioSaveCodecTests.cs)

### 91. `apprenticeship` — Mentorship pairings and skill growth (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupApprenticeship()` | **Cadence:** `Daily Mentorship XP Transfer`
- **UI Routes:** `apprenticeship`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ApprenticeshipSystem.cs`](../../Assets/Ashfall.Core/ApprenticeshipSystem.cs)
  - Host Session: [`src/Host/ApprenticeshipHostSession.cs`](../../src/Host/ApprenticeshipHostSession.cs)
  - Save Store: [`src/Host/ApprenticeshipSaveStore.cs`](../../src/Host/ApprenticeshipSaveStore.cs)
  - UI Panel: [`src/UI/ApprenticeshipPanel.cs`](../../src/UI/ApprenticeshipPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`](../../Ashfall.Core.Tests/ApprenticeshipSystemTests.cs)

### 92. `autopsy` — Post-mortem forensic analysis (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupAutopsy()` | **Cadence:** `Daily Forensic Case Progress`
- **UI Routes:** `autopsy_report`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AutopsySystem.cs`](../../Assets/Ashfall.Core/AutopsySystem.cs)
  - Host Session: [`src/Host/AutopsyHostSession.cs`](../../src/Host/AutopsyHostSession.cs)
  - Save Store: [`src/Host/AutopsySaveStore.cs`](../../src/Host/AutopsySaveStore.cs)
  - UI Panel: [`src/UI/AutopsyReportPanel.cs`](../../src/UI/AutopsyReportPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AutopsySystemTests.cs`](../../Ashfall.Core.Tests/AutopsySystemTests.cs)

### 93. `caregiving` — Childcare, elderly care, and comfort (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupCaregiving()` | **Cadence:** `Daily Nursery/Eldercare Comfort`
- **UI Routes:** `caregiving`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/CaregivingSystem.cs`](../../Assets/Ashfall.Core/Survivors/CaregivingSystem.cs)
  - Host Session: [`src/Host/CaregivingHostSession.cs`](../../src/Host/CaregivingHostSession.cs)
  - Save Store: [`src/Host/CaregivingSaveStore.cs`](../../src/Host/CaregivingSaveStore.cs)
  - UI Panel: [`src/UI/CaregivingPanel.cs`](../../src/UI/CaregivingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CaregivingSystemTests.cs`](../../Ashfall.Core.Tests/CaregivingSystemTests.cs)

### 94. `chemical_dependency` — Substance dependencies and withdrawal (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMentalHealthCrisis()` | **Cadence:** `Daily Tolerance & Withdrawal`
- **UI Routes:** `chemical_dependency`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`](../../Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs)
  - Host Session: [`src/Host/ChemicalDependencyHostSession.cs`](../../src/Host/ChemicalDependencyHostSession.cs)
  - Host Session: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - Save Store: [`src/Host/ChemicalDependencySaveStore.cs`](../../src/Host/ChemicalDependencySaveStore.cs)
  - UI Panel: [`src/UI/ChemicalDependencyPanel.cs`](../../src/UI/ChemicalDependencyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BareSaveStoreSealTests.cs`](../../Ashfall.Core.Tests/BareSaveStoreSealTests.cs)

### 95. `contractor_roster` — Hired mercenaries and specialists (Survival & Biology)
- **Owner Domain:** `personnel`
- **Setup Method:** `Main.SetupContractorRoster()` | **Cadence:** `Daily Mercenary Wage Payroll`
- **UI Routes:** `contractor_roster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ContractorRosterSystem.cs`](../../Assets/Ashfall.Core/ContractorRosterSystem.cs)
  - Host Session: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - Save Store: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - UI Panel: [`src/UI/ContractorRosterPanel.cs`](../../src/UI/ContractorRosterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ContractorRosterSystemTests.cs`](../../Ashfall.Core.Tests/ContractorRosterSystemTests.cs)

### 96. `disease` — Epidemics, contagions, and pathogen spread (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupDisease()` | **Cadence:** `Daily Pathogen Transmission`
- **UI Routes:** `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`](../../Assets/Ashfall.Core/Disease/DiseaseSystem.cs)
  - Host Session: [`src/Disease/DiseaseHostSession.cs`](../../src/Disease/DiseaseHostSession.cs)
  - Save Store: [`src/Host/DiseaseSaveStore.cs`](../../src/Host/DiseaseSaveStore.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiseaseSystemTests.cs`](../../Ashfall.Core.Tests/DiseaseSystemTests.cs)

### 97. `medical` — Triage, illnesses, and treatments (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedical()` | **Cadence:** `Daily Recovery / Affliction`
- **UI Routes:** `medical`, `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/MedicalWardSystem.cs)
  - Core System: [`Assets/Ashfall.Core/SickListSystem.cs`](../../Assets/Ashfall.Core/SickListSystem.cs)
  - Host Session: [`src/Host/MedicalHostSession.cs`](../../src/Host/MedicalHostSession.cs)
  - Save Store: [`src/Host/MedicalSaveStore.cs`](../../src/Host/MedicalSaveStore.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DwellerMedicalCatalogTests.cs`](../../Ashfall.Core.Tests/DwellerMedicalCatalogTests.cs)

### 98. `medical_ward` — Hospital ward beds and inpatients (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedicalWard()` | **Cadence:** `Daily Bed Inpatient Triage`
- **UI Routes:** `medical_ward`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/MedicalWardSystem.cs)
  - Host Session: [`src/Host/MedicalWardHostSession.cs`](../../src/Host/MedicalWardHostSession.cs)
  - Save Store: [`src/Host/MedicalWardSaveStore.cs`](../../src/Host/MedicalWardSaveStore.cs)
  - UI Panel: [`src/UI/MedicalWardPanel.cs`](../../src/UI/MedicalWardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs)

### 99. `mental_health_crisis` — Psychological trauma and psych ward (Survival & Biology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupMentalHealthCrisis()` | **Cadence:** `Daily Psych Ward Calming Ticks`
- **UI Routes:** `mental_health_crisis`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`](../../Assets/Ashfall.Core/MentalHealthCrisisSystem.cs)
  - Host Session: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - Save Store: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - UI Panel: [`src/UI/MentalHealthCrisisPanel.cs`](../../src/UI/MentalHealthCrisisPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs`](../../Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs)

### 100. `morale_contagion` — Flagship XI Plan 154 — morale contagion channels, breakdowns, social isolation, schism ledger, HopeBeacon installation (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupMoraleContagion()` | **Cadence:** `Daily Contagion / Isolation Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs`](../../Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs)
  - Host Session: [`src/Host/MoraleContagionHostSession.cs`](../../src/Host/MoraleContagionHostSession.cs)
  - Save Store: [`src/Host/MoraleContagionSaveStore.cs`](../../src/Host/MoraleContagionSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs`](../../Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs)

### 101. `survivor_relations` — Survivor affinities, feuds, and bonds (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupSurvivorRelations()` | **Cadence:** `Daily Affinity & Feud Drift`
- **UI Routes:** `survivor_relations`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SurvivorRelationsSystem.cs`](../../Assets/Ashfall.Core/SurvivorRelationsSystem.cs)
  - Host Session: [`src/Host/SurvivorRelationsHostSession.cs`](../../src/Host/SurvivorRelationsHostSession.cs)
  - Save Store: [`src/Host/SurvivorRelationsSaveStore.cs`](../../src/Host/SurvivorRelationsSaveStore.cs)
  - UI Panel: [`src/UI/SurvivorRelationsPanel.cs`](../../src/UI/SurvivorRelationsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 102. `survivors` — Living survivors, needs, and traits (Survival & Biology)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivors()` | **Cadence:** `Daily Needs Decay`
- **UI Routes:** `survivors`, `survivor_detail`, `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`](../../Assets/Ashfall.Core/Survivors/NeedsSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs)
  - Host Session: [`src/Host/SurvivorsHostSession.cs`](../../src/Host/SurvivorsHostSession.cs)
  - Save Store: [`src/Host/SurvivorsSaveStore.cs`](../../src/Host/SurvivorsSaveStore.cs)
  - UI Panel: [`src/UI/StatusPanel.cs`](../../src/UI/StatusPanel.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - UI Panel: [`src/UI/SurvivorsPanel.cs`](../../src/UI/SurvivorsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`](../../Ashfall.Core.Tests/NeedsRadiationSystemTests.cs)

### 103. `combat` — Combat encounters and tactical trauma (Tactical Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupCombat()` | **Cadence:** `On-Demand (Turn-Based)`
- **UI Routes:** `combat`, `combat_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`](../../Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs`](../../Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs)
  - Host Session: [`src/Host/CombatHostSession.cs`](../../src/Host/CombatHostSession.cs)
  - Save Store: [`src/Host/CombatSaveStore.cs`](../../src/Host/CombatSaveStore.cs)
  - UI Panel: [`src/UI/CombatDetailPanel.cs`](../../src/UI/CombatDetailPanel.cs)
  - UI Panel: [`src/UI/CombatHistoryPanel.cs`](../../src/UI/CombatHistoryPanel.cs)
  - UI Panel: [`src/UI/CombatPanel.cs`](../../src/UI/CombatPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CombatBallisticsTests.cs`](../../Ashfall.Core.Tests/CombatBallisticsTests.cs)

### 104. `ecological_infestation` — Plan 28 — location and shelter ecological infestations (trigger/clear/tolerate lifecycle) (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupEcologicalInfestation()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs`](../../Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/EcologicalInfestationSaveStore.cs`](../../src/Host/EcologicalInfestationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`](../../Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs)

### 105. `armored_crawlers` — Armored crawler modules and forward camps (World & Expeditions)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupArmoredCrawlers()` | **Cadence:** `Daily Crawler Module Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs`](../../Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ArmoredCrawlerSaveStore.cs`](../../src/Host/ArmoredCrawlerSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/ArmoredCrawlerExpeditionTests.cs`](../../Ashfall.Core.Tests/Expeditions/ArmoredCrawlerExpeditionTests.cs)

### 106. `encounter_choice` — Encounter choice history & outcomes (World & Expeditions)
- **Owner Domain:** `encounters`
- **Setup Method:** `Main.SetupEncounterChoice()` | **Cadence:** `On-Demand (Door Event Resolution)`
- **UI Routes:** `door_encounter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Host Session: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Save Store: [`src/Host/EncounterChoiceSaveStore.cs`](../../src/Host/EncounterChoiceSaveStore.cs)
  - UI Panel: [`src/YearOfAsh/DoorEncounterModal.cs`](../../src/YearOfAsh/DoorEncounterModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs`](../../Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs)

### 107. `expedition` — Wasteland expedition runs & status (World & Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupExpeditions()` | **Cadence:** `Daily Sortie Travel`
- **UI Routes:** `expeditions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`](../../Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs)
  - Core System: [`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`](../../Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs)
  - Host Session: [`src/Host/ExpeditionHostSession.cs`](../../src/Host/ExpeditionHostSession.cs)
  - Save Store: [`src/Host/ExpeditionSaveStore.cs`](../../src/Host/ExpeditionSaveStore.cs)
  - UI Panel: [`src/UI/ExpeditionPanel.cs`](../../src/UI/ExpeditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpeditionCampSystemTests.cs`](../../Ashfall.Core.Tests/ExpeditionCampSystemTests.cs)

### 108. `travel_encounters` — Travel encounters and cooldown states (World & Expeditions)
- **Owner Domain:** `encounters`
- **Setup Method:** `Main.SetupTravelEncounters()` | **Cadence:** `On-Demand (Travel Step)`
- **UI Routes:** `expeditions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs`](../../Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs)
  - Save Store: [`src/Host/TravelEncounterSaveStore.cs`](../../src/Host/TravelEncounterSaveStore.cs)
  - UI Panel: [`src/UI/ExpeditionPanel.cs`](../../src/UI/ExpeditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PatrolEncounterFullRegressionTests.cs`](../../Ashfall.Core.Tests/PatrolEncounterFullRegressionTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TravelEncounterCooldownGroupTests.cs`](../../Ashfall.Core.Tests/TravelEncounterCooldownGroupTests.cs)

### 109. `wasteland_map` — Wasteland map markers and fog-of-war (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorld()` | **Cadence:** `On-Demand (Fog-of-War Discovery)`
- **UI Routes:** `map`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Host Session: [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
  - Save Store: [`src/Host/WastelandMapSaveStore.cs`](../../src/Host/WastelandMapSaveStore.cs)
  - UI Panel: [`src/UI/MapPanel.cs`](../../src/UI/MapPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`](../../Ashfall.Core.Tests/WastelandMapPersistenceTests.cs)

### 110. `waystation` — Wasteland outpost network & relay hubs (World & Expeditions)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaystation()` | **Cadence:** `Daily Outpost Relay Barter`
- **UI Routes:** `waystation_network`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaystationSystem.cs`](../../Assets/Ashfall.Core/WaystationSystem.cs)
  - Host Session: [`src/Host/WaystationHostSession.cs`](../../src/Host/WaystationHostSession.cs)
  - Save Store: [`src/Host/WaystationSaveStore.cs`](../../src/Host/WaystationSaveStore.cs)
  - UI Panel: [`src/UI/WaystationNetworkPanel.cs`](../../src/UI/WaystationNetworkPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaystationSystemTests.cs`](../../Ashfall.Core.Tests/WaystationSystemTests.cs)

### 111. `wildlife_trapping` — Snares, game catches, and foraging (World & Expeditions)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupWildlifeTrapping()` | **Cadence:** `Daily Snare Yield & Butchery`
- **UI Routes:** `wildlife_trapping`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WildlifeTrappingSystem.cs`](../../Assets/Ashfall.Core/WildlifeTrappingSystem.cs)
  - Host Session: [`src/Host/WildlifeTrappingHostSession.cs`](../../src/Host/WildlifeTrappingHostSession.cs)
  - Save Store: [`src/Host/WildlifeTrappingSaveStore.cs`](../../src/Host/WildlifeTrappingSaveStore.cs)
  - UI Panel: [`src/UI/WildlifeTrappingPanel.cs`](../../src/UI/WildlifeTrappingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs`](../../Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs)

### 112. `world` — World map nodes, sectors, and discovery (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorld()` | **Cadence:** `Daily Weather & Hazard`
- **UI Routes:** `map`, `weather`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Core System: [`Assets/Ashfall.Core/World/WeatherSystem.cs`](../../Assets/Ashfall.Core/World/WeatherSystem.cs)
  - Host Session: [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
  - Save Store: [`src/Host/WorldSaveStore.cs`](../../src/Host/WorldSaveStore.cs)
  - UI Panel: [`src/UI/MapPanel.cs`](../../src/UI/MapPanel.cs)
  - UI Panel: [`src/UI/WeatherPanel.cs`](../../src/UI/WeatherPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WorldSaveablesTests.cs`](../../Ashfall.Core.Tests/WorldSaveablesTests.cs)

---

## 4. Lifecycle Status & Reachability Proof Matrix

| Section Key | Implemented | Constructed | Ticked / Cadence | Persisted | Player-Routed | Tested | E2E Status |
|---|:---:|:---:|---|:---:|:---:|:---:|:---:|
| `airlock_security` | ✅ | ✅ | ✅ `Daily Decon Interlock` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `amputation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `apprenticeship` | ✅ | ✅ | ✅ `Daily Mentorship XP Transfer` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `archaeology` | ✅ | ✅ | ⚡ `On-Demand (Excavation & Decryption)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `archive_desk` | ✅ | ✅ | ✅ `Daily Scribing & Folio Archival` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `armored_crawlers` | ✅ | ✅ | ✅ `Daily Crawler Module Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `autopsy` | ✅ | ✅ | ✅ `Daily Forensic Case Progress` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `aviation` | ✅ | ✅ | ✅ `Daily Flight Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `campaign_day` | ✅ | ✅ | ✅ `Master Sim Clock / Dawn Advance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caravan` | ✅ | ✅ | ✅ `Daily Route Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caravan_trade_network` | ✅ | ✅ | ✅ `Daily Route Arrival Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caregiving` | ✅ | ✅ | ✅ `Daily Nursery/Eldercare Comfort` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ceremony` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chem_warfare` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chemical_dependency` | ✅ | ✅ | ✅ `Daily Tolerance & Withdrawal` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chemical_synthesis` | ✅ | ✅ | ⚡ `On-Demand (Retort Synthesis)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `child_development` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `collectible_discovery` | ✅ | ✅ | ⚡ `On-Demand (One-Time Discovery Ledger)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `combat` | ✅ | ✅ | ⚡ `On-Demand (Turn-Based)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `comms_array` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `contractor_roster` | ✅ | ✅ | ✅ `Daily Mercenary Wage Payroll` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `crafting` | ✅ | ✅ | ✅ `Daily Workbench Queue` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `daily_briefing` | ✅ | ✅ | ✅ `Daily Dawn Briefing Aggregation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `decontamination` | ✅ | ✅ | ✅ `Daily Rad Scrub Shower Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `desperation` | ✅ | ✅ | ⚡ `On-Demand (Crisis Command)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `disease` | ✅ | ✅ | ✅ `Daily Pathogen Transmission` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dose_ledger` | ✅ | ✅ | ⚡ `On-Demand (Dose Log)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `duty_roster` | ✅ | ✅ | ✅ `Daily Shift Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dynamic_quests` | ✅ | ✅ | ⚡ `On-Demand (Campaign-Wide Emergency Quests)` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `ecological_infestation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `economy` | ✅ | ✅ | ✅ `Daily Market Rate Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `encounter_choice` | ✅ | ✅ | ⚡ `On-Demand (Door Event Resolution)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `endgame` | ✅ | ✅ | ⚡ `On-Demand (Day Threshold / Extinction)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `equipment_condition` | ✅ | ✅ | ✅ `Daily Gear Wear & Maintenance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `excavation` | ✅ | ✅ | ✅ `Daily Rubble Shoring Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `excavation_hazards` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expansion_hub` | ✅ | ✅ | ✅ `Daily Hub Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expansion_quest` | ✅ | ✅ | ⚡ `On-Demand (Stage Milestone)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expedition` | ✅ | ✅ | ✅ `Daily Sortie Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expedition_stealth` | ✅ | ✅ | ⚡ `Event-Driven (Expedition Phases)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `fallout` | ✅ | ✅ | ✅ `Hourly Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `field_guide` | ✅ | ✅ | ⚡ `On-Demand (Study & Discovery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `forced_labor` | ✅ | ✅ | ✅ `Daily Shift Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `fungi_cultivation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `greenhouse` | ✅ | ✅ | ✅ `Daily Hydroponic Growth` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `holdfast` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `holdfast_trade` | ✅ | ✅ | ⚡ `On-Demand (Barter)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `host_event` | ✅ | ✅ | ⚡ `On-Demand (Moral Dilemma)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `hydroponic_biomes` | ✅ | ✅ | ✅ `Daily Biome Rack Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `inventory` | ✅ | ✅ | ⚡ `On-Demand (Item Use)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `journal` | ✅ | ✅ | ⚡ `On-Demand (Log/Event)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `kitchen_nutrition` | ✅ | ✅ | ✅ `Daily Rationing Meal Prep` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `library_study` | ✅ | ✅ | ✅ `Daily Codex Research Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `maritime` | ✅ | ✅ | ⚡ `On-Demand (Dive Sortie)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical` | ✅ | ✅ | ✅ `Daily Recovery / Affliction` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_pipeline` | ✅ | ✅ | ⚡ `On-Demand (Triage & Procedure Commands)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_ward` | ✅ | ✅ | ✅ `Daily Bed Inpatient Triage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `memorial` | ✅ | ✅ | ⚡ `On-Demand (Survivor Fallen Eulogy)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mental_health_crisis` | ✅ | ✅ | ✅ `Daily Psych Ward Calming Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mercenary_bounties` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `moral_choice` | ✅ | ✅ | ⚡ `On-Demand (Branch Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `morale_contagion` | ✅ | ✅ | ✅ `Daily Contagion / Isolation Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `muster` | ✅ | ✅ | ⚡ `On-Demand (Rally Stance)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mutation_tree` | ✅ | ✅ | ⚡ `Event-Driven (Dose Thresholds)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narcotics` | ✅ | ✅ | ✅ `24h Medical Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narrative` | ✅ | ✅ | ⚡ `On-Demand (Dialog Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `nuclear_core_lifecycle` | ✅ | ✅ | ✅ `Daily Core Thermal Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `onboarding` | ✅ | ✅ | ⚡ `On-Demand (Player Sigil Recording)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `pathogen_strains` | ✅ | ✅ | ✅ `Daily Strain Progression Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `perimeter_defense` | ✅ | ✅ | ✅ `Daily Emplacement Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `personal_quests` | ✅ | ✅ | ⚡ `On-Demand (Survivor Quest Progression)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `phantom_memory` | ✅ | ✅ | ⚡ `On-Demand (Scavenge Echo)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `phase0` | ✅ | ✅ | ⚡ `On-Demand (Pre-War Flashback)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `power_grid` | ✅ | ✅ | ✅ `Daily Fuel Consumption & Wattage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `power_subgrids` | ✅ | ✅ | ✅ `Daily Thermal Distribution Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `prisoner_management` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `radio` | ✅ | ✅ | ⚡ `On-Demand (Frequency Scan)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `radio_station` | ✅ | ✅ | ⚡ `On-Demand (Tuning & Broadcasts)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `railway` | ✅ | ✅ | ⚡ `On-Demand (Convoy Operations)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `recreation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `regional_treaty` | ✅ | ✅ | ✅ `Daily Non-Aggression Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `research` | ✅ | ✅ | ⚡ `On-Demand (Study Progress)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `robotics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `settlement_politics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_assignment` | ✅ | ✅ | ⚡ `On-Demand (Bunk Reassignment)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_decor` | ✅ | ✅ | ⚡ `On-Demand (Decoration Placement)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_fire` | ✅ | ✅ | ✅ `Daily Fire Propagation Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_schedule` | ✅ | ✅ | ✅ `Daily Curfew Rotation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_social_dynamics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_thermal` | ✅ | ✅ | ✅ `Daily HVAC Frost Dissipation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_workshop` | ✅ | ✅ | ⚡ `On-Demand (Crafting & Refurbishment)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `silent_foundry` | ✅ | ✅ | ✅ `Daily Smelter Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `starting_level` | ✅ | ✅ | ⚡ `On-Demand (Opening Protocol)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `sump_flooding` | ✅ | ✅ | ✅ `Daily Drainage Pump Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `surgical_ward` | ✅ | ✅ | ✅ `Daily Sterile Field Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_fate` | ✅ | ✅ | ✅ `Daily Survivor-Death Cascade` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_relations` | ✅ | ✅ | ✅ `Daily Affinity & Feud Drift` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_social` | ✅ | ✅ | ✅ `Daily Shelter Social Dynamics` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivors` | ✅ | ✅ | ✅ `Daily Needs Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `thirdonary` | ✅ | ✅ | ⚡ `On-Demand (Arbitration)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `travel_encounters` | ✅ | ✅ | ⚡ `On-Demand (Travel Step)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `unique_claims` | ✅ | ✅ | ⚡ `On-Demand (Global Unique Claim Ledger)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `verdict` | ✅ | ✅ | ✅ `Daily Machine Log Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `vinyl_morale` | ✅ | ✅ | ✅ `Daily Turntable Morale Broadcast` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_justice` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_map` | ✅ | ✅ | ⚡ `On-Demand (Fog-of-War Discovery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `water_treatment` | ✅ | ✅ | ✅ `Daily Filtration Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `waystation` | ✅ | ✅ | ✅ `Daily Outpost Relay Barter` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `weight_of_choices` | ✅ | ✅ | ⚡ `On-Demand (Branch Decisions)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wildlife_trapping` | ✅ | ✅ | ✅ `Daily Snare Yield & Butchery` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `world` | ✅ | ✅ | ✅ `Daily Weather & Hazard` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `year_of_ash` | ✅ | ✅ | ✅ `Daily Deep-Freeze Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |

---

## 5. Architectural Verification Invariants

1. **Invariant 1 (Core Engine Agnosticism):** Core systems contain zero references to `Godot`, `UnityEngine`, or engine globals.
2. **Invariant 3 (Save Store Integrity):** Every save store delegates to `SaveStoreHub` / `SaveEnvelopeHelper` or a Core codec and wraps state in a verified checksum envelope.
3. **Invariant 5 (Thin Host Nodes):** UI panels and host sessions handle only presentation, lifecycle, and wiring — never domain calculations.
4. **Invariant 6 (Data Authority):** `Assets/StreamingAssets/Data/` JSON files are the sole authority.
5. **Mechanical Reachability Gate:** Every system in this matrix is verified by headless test runs in `verify-fast.sh` and xUnit suites in `Ashfall.Core.Tests`.
6. **Zero Conceptual Placeholders:** If a layer is absent or procedural, it is documented with explicit status rather than filled with conceptual names.
