# ASHFALL — Evidence-Derived Architecture & Verification Graph

**Last Verified:** 2026-09-24<br>
**Total Subsystems Mapped:** 238/238 (100.0%)<br>
**Verified End-to-End Coverage:** 147/238 (61.8% across all 6 vertical layers)<br>
**Status Breakdown:** Implemented: 238/238 | Constructed: 238/238 | Ticked: 238/238 | Persisted: 238/238 | Routed: 162/238 | Tested: 178/238<br>
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
| 1 | `commitment` | Campaign | `CommitmentSystem` | `commitments.json` | `Main`, `CommitmentHostSession` | `CommitmentSaveStore` | *None (GAP)* | `--commitments-selftest`, `Plan38CommitmentHostIntegrationTests`, `CommitmentSystemTests`, `CampaignCalendarPlan38Tests` | ❌ GAP |
| 2 | `endgame` | Campaign & Lore | `EndgameSystem`, `CampaignOutcomeEvaluator` | — *(Procedural)* | `EndgameHostSession` | `EndgameSaveStore` | `EpiloguePanel` | `--endings-selftest`, `EndgameSystemTests`, `CampaignOutcomeEvaluatorTests` | ✅ 6/6 |
| 3 | `host_event` | Campaign & Lore | `MoralChoiceSystem` | `events.json` | `HostEventAdapter` | `MoralChoiceSaveStore`, `HostEventSaveStore` | `EventDetailPanel` | `--moral-choice-selftest`, `HostEventSaveSealTests` | ✅ 6/6 |
| 4 | `journal` | Campaign & Lore | `JournalSystem` | `world_history.json` | `JournalHostSession` | `JournalSaveStore` | `JournalPanel`, `JournalBookUI` | `--journal-save-selftest`, `JournalSystemTests` | ✅ 6/6 |
| 5 | `memorial` | Campaign & Lore | `MemorialSystem` | — *(Procedural)* | `MemorialSystem` | `MemorialSaveStore` | `GameDashboardPanel` | `--player-panels-uitest`, `MemorialSystemTests` | ✅ 6/6 |
| 6 | `narrative` | Campaign & Lore | `NarrativeEncounterSystem` | `narrative_encounters.json` | `NarrativeHostSession` | `NarrativeSaveStore` | `EventsLogPanel`, `FactionsNarrativePanel` | `--narrative-selftest`, `NarrativeEncounterSystemTests` | ✅ 6/6 |
| 7 | `phase0` | Campaign & Lore | `RespiratoryDegenerationSystem` | — *(Procedural)* | `Phase0HostSession` | `Phase0SaveStore` | `Phase0Panel` | `--phase0-selftest`, `--phase0-uitest`, `Phase0EffectsBridgeTests` | ✅ 6/6 |
| 8 | `survivor_fate` | Campaign & Lore | `SurvivorFateSystem` | — *(Procedural)* | `Main` | `SurvivorFateSaveStore` | `GameDashboardPanel` | `--playable-shell-selftest`, `SurvivorFateSystemTests` | ✅ 6/6 |
| 9 | `onboarding` | Campaign & Onboarding | `OnboardingJourney` | — *(Procedural)* | `Main` | `OnboardingSaveStore` | `OnboardingHintPanel` | `--onboarding-journey-selftest`, `OnboardingJourneyTests` | ✅ 6/6 |
| 10 | `archive_desk` | Campaign & Progression | `ArchiveDeskSystem` | `archive_inks.json` | `ArchiveDeskHostSession` | `ArchiveDeskSaveStore` | `ArchiveDeskPanel` | `--shelter-operations-selftest`, `ArchiveDeskSystemTests` | ✅ 6/6 |
| 11 | `campaign_day` | Campaign & Progression | `CampaignDayCoordinator` | — *(Procedural)* | `CampaignDayCoordinator` | `CampaignDaySaveStore` | `GameDashboardPanel` | `--day1-selftest`, `--day1-to-day2-selftest`, `CampaignDayCoordinatorTests` | ✅ 6/6 |
| 12 | `daily_briefing` | Campaign & Progression | `DailyBriefingReportBuilder`, `DailyBriefingState` | — *(Procedural)* | `DailyBriefingState` | `DailyBriefingSaveStore` | `DailyBriefingModal` | `--day1-selftest`, `DailyBriefingReportBuilderTests` | ✅ 6/6 |
| 13 | `library_study` | Campaign & Progression | `LibraryStudySystem` | `library_manuals.json` | `LibraryStudyHostSession` | `LibraryStudySaveStore` | `LibraryStudyPanel` | `--shelter-operations-selftest`, `LibraryStudySystemTests` | ✅ 6/6 |
| 14 | `dynamic_quests` | Campaign & Quests | `DynamicQuestlineSystem` | `dynamic_questlines.json` | `DynamicQuestSaveStore` | `DynamicQuestSaveStore` | `DynamicQuestlinePanel` | `--save-store-checksum-selftest`, `DynamicQuestlineTests` | ✅ 6/6 |
| 15 | `narrative_questlines` | Campaign & Quests | `NarrativeQuestlineSystem` | `narrative_questlines.json` | `NarrativeQuestlineHostSession` | `NarrativeQuestlineSaveStore` | `QuestsPanel` | , `NarrativeQuestlineSystemTests` | ❌ GAP |
| 16 | `chlor_alkali_synthesis` | Chemistry | `ChlorAlkaliSynthesisEngine` | — *(Procedural)* | `ChlorAlkaliHostSession` | `ChlorAlkaliSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 17 | `ballistic_shield` | Combat | `BallisticShieldEngine` | — *(Procedural)* | `BallisticShieldHostSession` | `BallisticShieldSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 18 | `ballistics_workbench` | Combat | `BallisticsWorkbenchSystem` | `ballistics_workbench_catalog.json` | `Main` | `BallisticsWorkbenchSaveStore` | `BallisticsWorkbenchPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 19 | `settlement_defenses` | Combat | `DefenseSystem` | `defenses.json` | `DefenseHostSession` | `DefenseSaveStore` | `DefenseGridPanel` | , `DefenseSystemTests` | ❌ GAP |
| 20 | `sky_defense_battery` | Combat | `SkyDefenseBatterySystem` | — *(Procedural)* | `Main` | `SkyDefenseBatterySaveStore` | `SkyDefenseBatteryPanel` | `--sky-defense-selftest`, `SkyDefenseBatteryTests` | ✅ 6/6 |
| 21 | `perimeter_defense` | Combat & Defense | `PerimeterDefenseSystem` | `perimeter_defenses.json` | `Main` | `PerimeterDefenseSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `PerimeterDefenseTests` | ✅ 6/6 |
| 22 | `sound_ranging` | Combat & Defense | `SoundRangingThreatEngine` | `sound_ranging_catalog.json` | `SoundRangingHostSession` | `SoundRangingSaveStore` | `SoundRangingPanel` | `--plans-122-125-selftest`, `Plan123SoundRangingThreatEngineTests` | ✅ 6/6 |
| 23 | `time_capsules` | Communication & Heritage (Plan 212) | `TimeCapsuleSystem` | — *(Procedural)* | `TimeCapsuleHostSession` | `TimeCapsuleSaveStore` | `TimeCapsulePanel`, `GameDashboardPanel` | `--time-capsule-selftest`, `Plan212TimeCapsuleIntegrationTests`, `TimeCapsuleSystemTests` | ✅ 6/6 |
| 24 | `chemical_synthesis` | Crafting & Chemistry | `ChemicalSynthesisSystem` | `chemical_syntheses.json` | `ChemicalSynthesisHostSession` | `ChemicalSynthesisSaveStore` | `ChemicalLabPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 25 | `trade_routes` | Economy | `PlayerTradeRouteSystem`, `TradeRouteContract`, `TradeRouteCensus` | `caravan_trade_routes.json` | `Main`, `TradeRouteHostSession` | `TradeRouteSaveStore` | *None (GAP)* | `--trade-routes-selftest`, `Plan192TradeRouteHostIntegrationTests`, `TradeRouteContractTests` | ❌ GAP |
| 26 | `black_market` | Economy & Trade | `BlackMarketSystem`, `BlackMarketInventoryCatalog` | `black_market_inventory.json` | `BlackMarketHostSession` | `BlackMarketSaveStore` | *None (GAP)* | , `Plan211BlackMarketTests`, `Plan211BlackMarketHostWiringTests` | ❌ GAP |
| 27 | `caravan` | Economy & Trade | `TravelingCaravanSystem` | `trade_texts.json` | `TravelingCaravanHostSession` | `CaravanSaveStore` | `TravelingCaravanPanel` | `--caravan-selftest`, `TradeCaravanCatalogTests` | ✅ 6/6 |
| 28 | `caravan_trade_network` | Economy & Trade | `CaravanTradeNetworkSystem` | `caravan_trade_routes.json` | `Main` | `CaravanTradeSaveStore` | `TravelingCaravanPanel` | `--caravan-selftest`, `CaravanTradeNetworkTests` | ✅ 6/6 |
| 29 | `economy` | Economy & Trade | `MarketSystem` | `economy_goods.json` | `EconomyHostSession` | `EconomySaveStore` | `EconomyMarketPanel`, `EconomyDetailPanel` | `--economy-selftest`, `--economy-uitest`, `DynamicEconomyCharacterizationTests` | ✅ 6/6 |
| 30 | `regional_treaty` | Economy & Trade | `RegionalTreatySystem` | `faction_lore.json` | `RegionalTreatyHostSession` | `RegionalTreatySaveStore` | `RegionalTreatyPanel` | `--shelter-operations-selftest`, `RegionalTreatySaveChecksumTests` | ✅ 6/6 |
| 31 | `meta_progression` | Endgame | `MetaProgressionSystem`, `MetaProgressionCensus`, `CrossRunProfileStore` | `meta_unlockables.json` | `Main`, `MetaProgressionHostSession` | `MetaProgressionSaveStore` | `EpiloguePanel`, `ChroniclePanel` | `--meta-progression-selftest`, `Plan175MetaProgressionHostIntegrationTests` | ✅ 6/6 |
| 32 | `unified_ending` | Endgame | `UnifiedEndingResolver` | `epilogue_personalization.json` | `Main`, `UnifiedEndingHostSession` | `UnifiedEndingSaveStore` | `EpiloguePanel`, `ChroniclePanel` | `--unified-ending-selftest`, `Plan145UnifiedEndingIntegrationTests`, `Plan145UnifiedEndingHostIntegrationTests` | ✅ 6/6 |
| 33 | `expansion_hub` | Expansion Framework | `ExpansionMasterSession` | — *(Procedural)* | `ExpansionHostSession` | `ExpansionHubSaveStore` | `ExpansionsHubPanel` | `--expansions-selftest`, `--expansion-hub-save-selftest`, `ExpansionHubSaveTests` | ✅ 6/6 |
| 34 | `expansion_quest` | Expansion Framework | `ExpansionQuestSystem`, `ExpansionMasterSession` | `crossing_quests.json` | `ExpansionQuestHostSession` | `ExpansionQuestSaveStore` | `CrossingQuestPanel` | `--expansions-selftest`, `VersionReportContractTests` | ✅ 6/6 |
| 35 | `holdfast` | Expansions (Exp 01) | `HoldfastQuestSystem`, `HoldfastSession` | `holdfast_quests.json`, `holdfast_items.json` | `HoldfastRuntimeSession` | `HoldfastSaveStore` | `HoldfastTerminalPanel`, `GameDashboardPanel` | `--holdfast-save-selftest`, `--holdfast-selftest`, `HoldfastSaveTests` | ✅ 6/6 |
| 36 | `holdfast_trade` | Expansions (Exp 01) | `HoldfastTradeSession` | `items.json` | `HoldfastRuntimeSession` | `HoldfastTradeSaveStore` | `TradeScreenGodotPanel`, `HoldfastTerminalPanel` | `--holdfast-trade-save-selftest`, `HoldfastTradeSessionTests` | ✅ 6/6 |
| 37 | `duty_roster` | Expansions (Exp 02) | `DutyRosterSystem` | `duty_roster_quests.json`, `survivors.json` | `DutyRosterHostSession` | `DutyRosterSaveStore` | `DutyRosterPanel`, `DutyRosterDetailPanel` | `--duty-roster-selftest`, `--duty-roster-save-selftest`, `DutyRosterSaveTests` | ✅ 6/6 |
| 38 | `phantom_memory` | Expansions (Exp 03) | `PhantomMemoryEngine` | `phantom_triggers.json` | `PhantomMemoryHostSession` | `PhantomMemorySaveStore` | `StandingRecordPanel`, `PhantomMemoryPanel` | `--standing-record-selftest`, `PhantomMemoryEngineTests` | ✅ 6/6 |
| 39 | `thirdonary` | Expansions (Exp 04) | `ThirdonaryQuestSystem` | `thirdonary_quests.json` | `ThirdonaryHostSession` | `ThirdonarySaveStore` | `CrossingQuestPanel` | `--crossing-selftest`, `--arbitration-selftest`, `ThirdonaryQuestSystemTests`, `CrossingArbitrationSystemTests` | ✅ 6/6 |
| 40 | `year_of_ash` | Expansions (Exp 05) | `YearOfAshDeepFreezeSystem`, `YearOfAshRadonSystem` | `year_of_ash_events.json` | `YearOfAshHostSession` | `YearOfAshSaveStore` | `DoorEncounterModal` | `--year-of-ash-save-selftest`, `YearOfAshQuestProbe` | ✅ 6/6 |
| 41 | `muster` | Expansions (Exp 06) | `MusterSystem` | `muster_witnesses.json` | `MusterHostSession` | `MusterSaveStore` | `MusterPanel` | `--muster-selftest`, `--muster-uitest`, `MusterSystemTests` | ✅ 6/6 |
| 42 | `dose_ledger` | Expansions (Exp 07) | `DoseLedgerSystem`, `RadiationSystem` | `dose_items.json` | `DoseLedgerHostSession` | `DoseLedgerSaveStore` | `RadiationHistoryPanel`, `RadiationDetailPanel` | `--dose-ledger-selftest`, `--dose-uitest`, `NeedsRadiationSaveRoundTripTests` | ✅ 6/6 |
| 43 | `verdict` | Expansions (Exp 08) | `ReckoningSystem`, `MachineLogSystem` | `verdict_data.json` | `VerdictHostSession` | `VerdictSaveStore` | `VerdictPanel`, `VerdictDashboardPanel` | `--verdict-selftest`, `--verdict-uitest`, `VerdictChainTests` | ✅ 6/6 |
| 44 | `maritime` | Expansions (Exp 09) | `MaritimeDiveSystem` | `dive_sites.json` | `MaritimeHostSession` | `MaritimeSaveStore` | `MaritimePanel` | `--black-flotilla-selftest`, `BlackFlotillaTests` | ✅ 6/6 |
| 45 | `silent_foundry` | Expansions (Exp 10) | `SilentFoundrySystem` | `foundry_items.json` | `SilentFoundryHostSession` | `SilentFoundrySaveStore` | `SilentFoundryPanel` | `--silent-foundry-selftest`, `--silent-foundry-uitest`, `SilentFoundryConsequenceTests` | ✅ 6/6 |
| 46 | `chemical_recon` | Expeditions | `ChemicalReconEngine` | — *(Procedural)* | `ChemicalReconSaveStore` | `ChemicalReconSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 47 | `colony` | Expeditions | `ColonySystem` | `colony_blueprints.json` | `Main` | `ColonySaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan160ColonyIntegrationTests` | ❌ GAP |
| 48 | `draisine_recovery` | Expeditions | `DraisineRerailingSystem` | — *(Procedural)* | `DraisineRerailingHostSession` | `DraisineRerailingSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 49 | `mine_clearing_flail` | Expeditions | `MineClearingFlailEngine` | `mine_flail_catalog.json` | `MineClearingFlailHostSession` | `MineClearingFlailSaveStore` | `MineFlailPanel` | `--mine-flail-uitest`, `MineClearingFlailEngineTests` | ✅ 6/6 |
| 50 | `rail_grinding` | Expeditions | `RailGrindingEngine` | `rail_grinding_catalog.json` | `RailGrindingHostSession` | `RailGrindingSaveStore` | `RailGrindingPanel` | `--rail-grinding-uitest`, `RailGrindingEngineTests` | ✅ 6/6 |
| 51 | `recon_telemetry` | Expeditions | `ReconTelemetrySystem` | — *(Procedural)* | `ReconTelemetrySaveStore` | `ReconTelemetrySaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 52 | `vehicle_garage` | Expeditions | `VehicleGarageSystem`, `VehicleArmorGradeCatalogLoader` | `vehicle_modifications.json`, `vehicle_armor_grades.json` | `Main` | `VehicleGarageSaveStore` | `VehicleGaragePanel` | `--vehicle-garage-selftest`, `VehicleGarageSystemTests`, `Plan50VehicleGarageIntegrationTests`, `Plan213VehicleArmorGradeTests` | ✅ 6/6 |
| 53 | `counter_intelligence` | Factions | `CounterIntelligenceSystem` | — *(Procedural)* | `CounterIntelligenceSaveStore` | `CounterIntelligenceSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 54 | `diplomatic_summits` | Factions | `DiplomaticSummitSystem` | — *(Procedural)* | `Main` | `DiplomaticSummitSaveStore` | *None (GAP)* | , `DiplomaticSummitTests` | ❌ GAP |
| 55 | `espionage` | Factions | `EspionageSystem` | `espionage_missions.json` | `EspionageHostSession` | `EspionageSaveStore` | *None (GAP)* | , `Plan167EspionageTests` | ❌ GAP |
| 56 | `faction_covert_ops` | Factions | `FactionCovertOpsCoordinator` | `espionage_operations.json` | `Main` | `FactionCovertOpsSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan153FactionEspionageIntegrationTests` | ❌ GAP |
| 57 | `faction_espionage` | Factions | `ShelterEspionageSystem` | `faction_intelligence.json` | `Main` | `ShelterEspionageSaveStore` | *None (GAP)* | , `ShelterEspionageSystemTests` | ❌ GAP |
| 58 | `territory_control` | Factions | `TerritoryControlSystem`, `FactionTerritoryDef`, `SupplyLineDef`, `LocationTerritoryState`, `SupplyLineState`, `TerritoryControlSaveState` | `faction_territory.json`, `supply_lines.json` | `TerritoryControlHostSession`, `Main` | `TerritoryControlSaveStore` | *None (GAP)* | `--territory-control-selftest`, `Plan134TerritoryControlHostIntegrationTests`, `Plan134TerritoryControlIntegrationTests` | ❌ GAP |
| 59 | `weight_of_choices` | Factions & Diplomacy | `FactionBranchCoordinator`, `MilitaryBranchSystem`, `RebelBranchSystem`, `IndependentBranchSystem`, `PrpfStandingSystem` | `military_faction_branch.json`, `rebel_faction_branch.json`, `independent_faction_branch.json` | `FactionBranchHostSession` | `WeightOfChoicesSaveStore` | `FactionsPanel`, `QuestsPanel` | `--expansions-selftest`, `FactionBranchCoordinatorTests`, `MilitaryBranchSystemTests`, `RebelBranchSystemTests`, `IndependentBranchSystemTests`, `PrpfStandingSystemTests`, `WeightOfChoicesSaveTests` | ✅ 6/6 |
| 60 | `aeroponics` | Farming | `AeroponicsSystem` | `aeroponics_nutrient_catalog.json` | `Main` | `AeroponicsSaveStore` | `AeroponicsPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 61 | `agriculture` | Farming | `AgricultureSystem` | `crop_strains.json` | `Main` | `AgricultureSaveStore` | *None (GAP)* | , `AgricultureSystemTests` | ❌ GAP |
| 62 | `aquaponics` | Farming | `AquaponicsSystem` | `aquaponics_system_catalog.json` | `Main` | `AquaponicsSaveStore` | *None (GAP)* | `--aquaponics-selftest`, `AquaponicsSystemTests`, `PlansB86ToB89ContinuityTests` | ❌ GAP |
| 63 | `powder_metallurgy` | Foundry | `PowderMetallurgySystem` | — *(Procedural)* | `PowderMetallurgySaveStore` | `PowderMetallurgySaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 64 | `hydraulic_extrusion` | Foundry & Industry | `HydraulicExtrusionEngine` | `hydraulic_extrusion_catalog.json` | `HydraulicExtrusionHostSession` | `HydraulicExtrusionSaveStore` | `HydraulicExtrusionPanel` | `--plans-139-141-selftest`, `Plan140HydraulicExtrusionTests` | ✅ 6/6 |
| 65 | `shelter_governance` | Governance | `ShelterGovernanceEngine`, `ShelterGovernanceCatalogLoader`, `ShelterGovernanceCensus` | `shelter_governance_blocs.json` | `Main`, `ShelterGovernanceHostSession` | `ShelterGovernanceSaveStore` | `SurvivorDetailPanel` | `--shelter-governance-selftest`, `Plan159ShelterGovernanceHostIntegrationTests`, `ShelterGovernanceEngineTests`, `Plan159_190GovernanceProvenanceIntegrationTests` | ✅ 6/6 |
| 66 | `shelter_identity` | Holdfast | `ShelterIdentitySystem`, `ShelterOriginCatalogLoader` | `shelter_origins.json` | `Main`, `ShelterIdentityHostSession` | `ShelterIdentitySaveStore` | `ShelterPanel` | `--shelter-identity-selftest`, `ShelterOriginCatalogLoaderTests`, `ShelterIdentitySystemTests` | ✅ 6/6 |
| 67 | `wildlife_ecosystem` | Hunting | `WildlifeEcosystemSystem` | `wildlife_ecosystem.json` | `WildlifeEcosystemHostSession` | `WildlifeEcosystemSaveStore` | `BestiaryPanel` | , `WildlifeEcosystemSystemTests` | ❌ GAP |
| 68 | `shelter_barter` | Illicit Economy / Barter | `ShelterBarterSystem` | `merchant_caravans.json` | `Main` | `ShelterBarterSaveStore` | *None (GAP)* | `--contraband-stash-selftest`, `ShelterBarterSystemPlan54Tests`, `ContrabandBarterRouteTests` | ❌ GAP |
| 69 | `grain_milling_archive` | Industrial Food-Processing Archive | `GrainMillingDiscoverySystem` | `burr_millstone_dressing_logs.json`, `bolting_silk_mesh_reports.json`, `grain_silo_weevil_audits.json`, `mill_dampener_tempering_assays.json` | `Main` | `GrainMillingArchiveSaveStore` | *None (GAP)* | , `GrainMillingDiscoveryTests`, `GrainMillingCatalogTests` | ❌ GAP |
| 70 | `wasteland_rumors` | Information & Rumors (Plan 203) | `RumorSystem` | — *(Procedural)* | `RumorNetworkHostSession` | `RumorNetworkSaveStore` | `RumorBoardPanel`, `GameDashboardPanel` | `--rumor-network-selftest`, `Plan203RumorNetworkIntegrationTests` | ✅ 6/6 |
| 71 | `cryogenic_air_separation` | Infrastructure | `CryogenicAirSeparationSystem` | — *(Procedural)* | `CryogenicAirSeparationSaveStore` | `CryogenicAirSeparationSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 72 | `fluid_logistics` | Infrastructure | `FluidLogisticsSystem` | `fluid_infrastructure.json` | `FluidLogisticsHostSession` | `FluidLogisticsSaveStore` | *None (GAP)* | , `Plan168FluidLogisticsTests` | ❌ GAP |
| 73 | `pneumatic_dispatch` | Infrastructure | `PneumaticDispatchSystem` | `pneumatic_network_catalog.json` | `Main` | `PneumaticDispatchSaveStore` | `PneumaticDispatchPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 74 | `black_projects_archive` | Intelligence Archive | `BlackProjectsArchiveSystem` | `orbital_kinetic_telemetry.json`, `drone_carrier_blackboxes.json`, `cobalt_arming_directives.json`, `architect_vault_audits.json` | `Main` | `BlackProjectsArchiveSaveStore` | `BlackProjectsArchivePanel` | , `BlackProjectsArchiveTests`, `BlackProjectsCatalogTests`, `BlackProjectsArchivePanelRouteTests` | ❌ GAP |
| 75 | `collectible_discovery` | Inventory & Lore | `CollectibleDiscoveryState` | `collectibles.json` | `Main` | `CollectibleDiscoverySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CollectibleDiscoveryPersistenceTests` | ✅ 6/6 |
| 76 | `unique_claims` | Inventory & Lore | `UniqueItemClaimRegistry` | `collectibles.json` | `Main` | `UniqueClaimSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CollectibleDiscoveryPersistenceTests` | ✅ 6/6 |
| 77 | `cultural_archives` | Knowledge | `CulturalArchiveVaultSystem` | — *(Procedural)* | `Main` | `CulturalArchiveSaveStore` | *None (GAP)* | , `CulturalArchiveVaultTests` | ❌ GAP |
| 78 | `field_guide` | Knowledge | `FieldGuideCatalog` | — *(Procedural)* | `Main` | `FieldGuideSaveStore` | `GameDashboardPanel` | `--world-selftest`, `FieldGuidePersistenceTests` | ✅ 6/6 |
| 79 | `prewar_archives` | Knowledge | `PrewarArchiveDecryptionSystem` | `prewar_archives.json` | `Main` | `PrewarArchiveSaveStore` | *None (GAP)* | , `PrewarArchiveDecryptionTests` | ❌ GAP |
| 80 | `research` | Knowledge | `ResearchSystem` | `research_knowledge.json` | `Main` | `ResearchSaveStore` | `ResearchPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `MedicalPipelineArchitectureGateTests` | ✅ 6/6 |
| 81 | `survivor_education` | Knowledge | `SurvivorEducationSystem` | `education_curriculum.json` | `Main` | `SurvivorEducationSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan154EducationIntegrationTests` | ❌ GAP |
| 82 | `campaign_legacy` | Legacy | `CampaignLegacySystem`, `CampaignLegacy`, `CampaignLegacyCensus`, `CampaignLegacyState`, `StartingCampaignContext`, `LegacyTrait` | `legacy_traits.json` | `CampaignLegacyHostSession`, `Main` | `CampaignLegacySaveStore` | *None (GAP)* | `--campaign-legacy-selftest`, `Plan140CampaignLegacyHostIntegrationTests`, `Plan140GenerationalLegacyIntegrationTests` | ❌ GAP |
| 83 | `leatherwork_archive` | Material Provenance Archive | `LeatherworkArchiveSystem` | `oak_bark_tanning_pit_logs.json`, `chrome_alum_tanning_assays.json`, `rawhide_bating_failure_reports.json`, `leather_harness_conditioning_audits.json` | `Main` | `LeatherworkArchiveSaveStore` | `InventoryDetailPanel` | , `LeatherworkArchiveTests`, `TanningLeatherCatalogTests` | ❌ GAP |
| 84 | `lyophilization` | Medical | `LyophilizationSystem` | — *(Procedural)* | `LyophilizationSaveStore` | `LyophilizationSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 85 | `medical_pipeline` | Medical | `MedicalPipelineCoordinator` | `disease_catalog.json` | `Main` | `MedicalPipelineSaveStore` | `MedicalPanel`, `GameDashboardPanel` | `--save-load-ui-failure-selftest`, `MedicalPipelineArchitectureGateTests` | ✅ 6/6 |
| 86 | `microfluidic_diagnostic` | Medical | `MicrofluidicDiagnosticEngine` | `microfluidic_diagnostic_catalog.json` | `MicrofluidicDiagnosticHostSession` | `MicrofluidicDiagnosticSaveStore` | `MicrofluidicDiagnosticPanel` | `--microfluidic-diagnostic-uitest`, `MicrofluidicDiagnosticEngineTests` | ✅ 6/6 |
| 87 | `pathogen_strains` | Medical | `PathogenStrainSystem` | `pathogens.json` | `Main` | `PathogenStrainSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `DiseaseSystemTests` | ✅ 6/6 |
| 88 | `psychological_sanatorium` | Medical | `PsychologicalSanatoriumSystem` | — *(Procedural)* | `Main` | `PsychologicalSanatoriumSaveStore` | *None (GAP)* | , `PsychologicalSanatoriumTests` | ❌ GAP |
| 89 | `surgical_ward` | Medical | `AdvancedSurgicalWardSystem` | — *(Procedural)* | `Main` | `SurgicalWardSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 90 | `propaganda_campaigns` | Morale & Influence (Plan 168) | `PropagandaSystem` | — *(Procedural)* | `PropagandaHostSession` | `PropagandaSaveStore` | `PropagandaPanel`, `GameDashboardPanel` | `--propaganda-selftest`, `Plan168PropagandaIntegrationTests` | ✅ 6/6 |
| 91 | `confession_secret` | Narrative | `ConfessionSecretSystem` | `confession_secrets.json` | `Main` | `ConfessionSecretSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `ConfessionSecretSystemTests` | ❌ GAP |
| 92 | `echoes` | Narrative | `EchoSystem`, `NarrativeContinuityEngine` | `echoes.json` | `EchoHostSession`, `EchoSaveStore` | `EchoSaveStore` | *None (GAP)* | , `EchoCatalogTests`, `EchoSystemTests` | ❌ GAP |
| 93 | `npc_memory` | Narrative | `NpcMemorySystem`, `NpcMemoryEntry`, `NpcRelationship`, `NpcMemoryCensus` | `npc_memory_dialogue.json` | `Main`, `NpcMemoryHostSession` | `NpcMemorySaveStore` | *None (GAP)* | `--npc-memory-selftest`, `Plan147NpcMemoryHostIntegrationTests`, `NpcMemorySystemTests` | ❌ GAP |
| 94 | `seasonal_celebration` | Narrative | `SeasonalCelebrationSystem` | `shelter_celebrations.json` | `Main` | `SeasonalCelebrationSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan170SeasonalCelebrationsIntegrationTests` | ❌ GAP |
| 95 | `shelter_festival` | Narrative | `ShelterFestivalEngine` | — *(Procedural)* | `Main` | `ShelterFestivalSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan170SeasonalCelebrationsIntegrationTests` | ❌ GAP |
| 96 | `oral_lore` | Narrative & Cultural Tradition | `OralLorePerformanceSystem` | `oral_lore_codex.json`, `oral_lore_batch_2.json` | `Main` | `OralLoreSaveStore` | *None (GAP)* | , `OralLorePlan155Tests`, `OralLoreCatalogTests` | ❌ GAP |
| 97 | `moral_choice` | Narrative & Decisions | `MoralChoiceSystem`, `MoralChoiceState` | `moral_choice_quests.json` | `MoralChoiceSystem` | `MoralChoiceSaveStore` | `GameDashboardPanel` | `--moral-choice-selftest`, `MoralChoiceSystemTests` | ✅ 6/6 |
| 98 | `contraband_stash` | Narrative & Illicit Economy | `ContrabandStashSystem` | `bunker_contraband_barter.json` | `Main` | `ContrabandSaveStore` | *None (GAP)* | `--contraband-stash-selftest`, `ContrabandPlan147Tests` | ❌ GAP |
| 99 | `cooking` | Nutrition | `CookingSystem`, `CookingRecipe`, `CookingOperation`, `CookingState`, `CookingCensus`, `CookingRecipeCatalogLoader`, `InventoryCookingSource` | `recipes_cooking.json` | `CookingHostSession`, `Main` | `CookingSaveStore` | *None (GAP)* | `--cooking-selftest`, `Plan136CookingHostIntegrationTests`, `Plan136WildlifeCookingIntegrationTests` | ❌ GAP |
| 100 | `grain_processing` | Nutrition | `GrainProcessingSystem` | — *(Procedural)* | `GrainProcessingSaveStore` | `GrainProcessingSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 101 | `companion_animals` | Plan 174 Companion Animals | `CompanionAnimalSystem` | `companion_animals.json` | `Main` | `CompanionSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `Plan174CompanionAnimalTests` | ✅ 6/6 |
| 102 | `zealotry` | Plan 175 Ideological Pressure | `ZealotrySystem` | `wasteland_religions.json` | `Main` | `ZealotrySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ZealotrySystemTests` | ✅ 6/6 |
| 103 | `anomaly_hazard` | Plan 176 Anomaly Hazard Layer | `AnomalyHazardSystem` | `anomalies.json` | `Main` | `AnomalyHazardSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `Plan176AnomalyHazardTests`, `Plan176CrossSystemConsumerTests` | ✅ 6/6 |
| 104 | `bionics` | Plan 177 Bionics & Prosthetics | `BionicsSystem` | `bionics.json` | `Main` | `BionicsSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `Plan177BionicsTests` | ✅ 6/6 |
| 105 | `spiritual_meaning` | Plan 30 Spiritual Meaning | `SpiritualMeaningCoordinator` | `spiritual_rituals.json`, `memorial_rites.json`, `belief_movements.json` | `Main` | `SpiritualSaveStore` | `IronCenotaphMemorialPanel` | `--save-store-checksum-selftest`, `Plan30SpiritualWorldTests` | ✅ 6/6 |
| 106 | `amputation` | Plans 178-201 Expansion Block | `AmputationSystem` | `surgical_procedures.json` | `Main` | `AmputationSaveStore` | `MedicalPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `AmputationSystemTests` | ✅ 6/6 |
| 107 | `archaeology` | Plans 178-201 Expansion Block | `ArchaeologySystem` | `lore_archives.json` | `Main` | `ArchaeologySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ArchaeologySystemTests` | ✅ 6/6 |
| 108 | `aviation` | Plans 178-201 Expansion Block | `AviationSystem` | `aircraft_parts.json` | `Main` | `AviationSaveStore` | `AviationUI`, `GameDashboardPanel` | `--expedition-selftest`, `AviationSystemTests` | ✅ 6/6 |
| 109 | `ceremony` | Plans 178-201 Expansion Block | `CeremonySystem` | `ceremonies.json` | `Main` | `CeremonySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CeremonySystemTests` | ✅ 6/6 |
| 110 | `chem_warfare` | Plans 178-201 Expansion Block | `ChemWarfareSystem` | `chemical_weapons.json` | `Main` | `ChemWarfareSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ChemWarfareSystemTests` | ✅ 6/6 |
| 111 | `child_development` | Plans 178-201 Expansion Block | `GenerationalSystem` | `development_traits.json` | `Main` | `GenerationalSaveStore` | `NurseryPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `GenerationalSystemTests`, `GenerationalLineageExtensionTests` | ✅ 6/6 |
| 112 | `comms_array` | Plans 178-201 Expansion Block | `CommsArraySystem` | `comms_targets.json` | `Main` | `CommsArraySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CommsArraySystemTests` | ✅ 6/6 |
| 113 | `desperation` | Plans 178-201 Expansion Block | `DesperationSystem` | `desperation_events.json` | `Main` | `DesperationSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `DesperationSystemTests` | ✅ 6/6 |
| 114 | `expedition_stealth` | Plans 178-201 Expansion Block | `StealthSystem` | `camouflage_gear.json` | `Main` | `StealthSaveStore` | `StealthReadoutPanel`, `GameDashboardPanel` | `--expedition-selftest`, `StealthSystemTests` | ✅ 6/6 |
| 115 | `fallout` | Plans 178-201 Expansion Block | `FalloutSystem` | `fallout_patterns.json` | `Main` | `FalloutSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FalloutSystemTests` | ✅ 6/6 |
| 116 | `forced_labor` | Plans 178-201 Expansion Block | `ForcedLaborSystem` | `labor_camps.json` | `Main` | `ForcedLaborSaveStore` | `LaborUI`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `ForcedLaborSystemTests` | ✅ 6/6 |
| 117 | `fungi_cultivation` | Plans 178-201 Expansion Block | `FungiCultivationSystem` | `underground_flora.json` | `Main` | `FungiSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FungiCultivationSystemTests` | ✅ 6/6 |
| 118 | `mercenary_bounties` | Plans 178-201 Expansion Block | `MercenarySystem` | `bounty_board.json` | `Main` | `MercenarySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `MercenarySystemTests` | ✅ 6/6 |
| 119 | `mutation_tree` | Plans 178-201 Expansion Block | `MutationSystem` | `mutations.json` | `Main` | `MutationSaveStore` | `MutationTreePanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `MutationSystemTests` | ✅ 6/6 |
| 120 | `narcotics` | Plans 178-201 Expansion Block | `NarcoticsSystem` | `narcotics.json` | `Main` | `NarcoticsSaveStore` | `ChemUI`, `PharmaLabPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `NarcoticsSystemTests` | ✅ 6/6 |
| 121 | `prisoner_management` | Plans 178-201 Expansion Block | `PrisonerSystem` | `interrogation_tactics.json` | `Main` | `PrisonerSaveStore` | `PrisonerPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `PrisonerSystemTests` | ✅ 6/6 |
| 122 | `railway` | Plans 178-201 Expansion Block | `RailwaySystem` | `rail_network.json` | `Main` | `RailwaySaveStore` | `GameDashboardPanel` | `--expedition-selftest`, `RailwaySystemTests` | ✅ 6/6 |
| 123 | `recreation` | Plans 178-201 Expansion Block | `SurvivorDowntimeSystem` | `recreation.json` | `Main` | `RecreationSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `SurvivorDowntimeSystemTests` | ✅ 6/6 |
| 124 | `robotics` | Plans 178-201 Expansion Block | `RoboticsSystem` | `robotics.json` | `Main` | `RoboticsSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `RoboticsSystemTests` | ✅ 6/6 |
| 125 | `settlement_politics` | Plans 178-201 Expansion Block | `PoliticsSystem` | `political_policies.json` | `Main` | `PoliticsSaveStore` | `PoliticsUI`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `PoliticsSystemTests` | ✅ 6/6 |
| 126 | `wasteland_justice` | Plans 178-201 Expansion Block | `JusticeSystem` | `wasteland_laws.json` | `Main` | `JusticeSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `JusticeSystemTests` | ✅ 6/6 |
| 127 | `plastic_pyrolysis` | Plans 202-205 Flagship (Plan 202) | `PlasticPyrolysisSystem` | `plastic_pyrolysis_catalog.json` | `Main` | `PlasticPyrolysisSaveStore` | `PlasticPyrolysisPanel` | `--save-store-checksum-selftest`, `PlasticPyrolysisEngineTests` | ✅ 6/6 |
| 128 | `cargo_airdrop` | Plans 202-205 Flagship (Plan 205) | `CargoAirdropSystem` | `cargo_airdrop_catalog.json` | `Main` | `CargoAirdropSaveStore` | `CargoAirdropPanel` | `--save-store-checksum-selftest`, `CargoAirdropEngineTests` | ✅ 6/6 |
| 129 | `geothermal_orc` | Power | `GeothermalOrcSystem` | `geothermal_strata_catalog.json` | `Main` | `GeothermalOrcSaveStore` | `GeothermalOrcPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 130 | `kinetic_storage` | Power | `KineticStorageSystem` | — *(Procedural)* | `KineticStorageSaveStore` | `KineticStorageSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 131 | `solar_concentrator` | Power | `SolarConcentratorEngine` | — *(Procedural)* | `SolarConcentratorHostSession` | `SolarConcentratorSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 132 | `hobby` | Psychology | `HobbySystem` | `hobby_definitions.json` | `Main` | `HobbySaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan161HobbyIntegrationTests` | ❌ GAP |
| 133 | `psychological_arcs` | Psychology | `PsychologicalArcSystem` | `mental_arcs.json` | `PsychologyArcHostSession` | `PsychologyArcSaveStore` | *None (GAP)* | , `PsychologicalArcSystemTests` | ❌ GAP |
| 134 | `survivor_autonomy` | Psychology | `SurvivorAutonomySystem` | `autonomy_actions.json` | `Main` | `SurvivorAutonomySaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan144SurvivorAutonomyIntegrationTests` | ❌ GAP |
| 135 | `survivor_mental_health` | Psychology | `SurvivorMentalHealthSystem` | `psychological_trauma.json` | `Main` | `SurvivorMentalHealthSaveStore` | *None (GAP)* | , `SurvivorMentalHealthTests` | ❌ GAP |
| 136 | `procedural_narrative` | Quests | `ProceduralNarrativeSystem` | `quest_templates.json` | `ProceduralNarrativeHostSession` | `ProceduralNarrativeSaveStore` | *None (GAP)* | , `Plan169ProceduralNarrativeTests` | ❌ GAP |
| 137 | `low_background_metrology` | Radiation & Metrology | `LowBackgroundLeadEngine` | `low_background_lead_catalog.json` | `LowBackgroundMetrologyHostSession` | `LowBackgroundMetrologySaveStore` | `LowBackgroundLeadPanel` | , `Plan138LowBackgroundLeadEngineTests` | ❌ GAP |
| 138 | `communications` | Radio | `CommunicationsSystem` | `communications_networks.json` | `Main` | `CommunicationsSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan157CommunicationsIntegrationTests` | ❌ GAP |
| 139 | `heliograph` | Radio | `HeliographSystem` | — *(Procedural)* | `HeliographSaveStore` | `HeliographSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 140 | `nvis_communications` | Radio | `NvisCommunicationsSystem` | — *(Procedural)* | `NvisCommunicationsSaveStore` | `NvisCommunicationsSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 141 | `psyops` | Radio | `PsyOpsSystem` | — *(Procedural)* | `PsyOpsHostSession` | `PsyOpsSaveStore` | *None (GAP)* | , `PsyOpsSystemTests` | ❌ GAP |
| 142 | `radio_program_production` | Radio | `RadioProgramProductionSystem` | `radio_programs.json` | `RadioProgramProductionHostSession` | `RadioProgramProductionSaveStore` | `RadioPanel` | , `Plan173RadioProgramProductionTests` | ❌ GAP |
| 143 | `retention` | Records | `RetentionPolicyCatalog`, `RetentionPolicyCatalogLoader`, `RollingLog` | `retention_policies.json` | `RetentionHostSession`, `Main` | `RetentionSaveStore` | *None (GAP)* | `--retention-selftest`, `Plan55RetentionHostIntegrationTests`, `RetentionPolicyCatalogLoaderTests`, `Plan55RetentionPolicyIntegrationTests` | ❌ GAP |
| 144 | `research_unlock` | Research | `ResearchUnlockBridge` | `research_unlocks.json` | `Main`, `ResearchUnlockHostSession` | `ResearchUnlockSaveStore` | `ResearchPanel` | `--research-unlock-selftest`, `Plan141ResearchUnlockBridgeIntegrationTests`, `Plan141ResearchUnlockHostIntegrationTests` | ✅ 6/6 |
| 145 | `playable_metrics` | Save | `PlaySessionRecorder`, `FirstHourFunnel`, `PlayableMetricsAggregationEngine` | — *(Procedural)* | `Main`, `PlayMetricsHostSession` | `PlayMetricsSaveStore` | *None (GAP)* | `--playable-metrics-selftest`, `Plan46PlayMetricsHostIntegrationTests`, `PlaySessionRecorderTests` | ❌ GAP |
| 146 | `session_durability` | Save | `SessionDurabilityManager` | — *(Procedural)* | `Main`, `SessionDurabilityHostSession`, `SaveLoadHostSession` | `SessionDurabilitySaveStore` | *None (GAP)* | `--session-durability-selftest`, `Plan39SessionDurabilityHostIntegrationTests`, `SessionDurabilityManagerTests` | ❌ GAP |
| 147 | `seven_day_slice` | Save | `SliceScenario`, `SliceScenarioCatalogLoader` | `slice_seven_days.json` | `SliceScenarioHostSession` | `SliceScenarioSaveStore` | *None (GAP)* | `--seven-day-slice-selftest`, `Plan54SevenDaySliceHostIntegrationTests` | ❌ GAP |
| 148 | `outpost_settlement` | Settlements | `OutpostSettlementSystem`, `OutpostDef`, `OutpostInstance`, `OutpostSettlementState` | `outposts.json` | `OutpostSettlementHostSession`, `Main` | `OutpostSettlementSaveStore` | *None (GAP)* | `--outpost-settlement-selftest`, `Plan58OutpostHostIntegrationTests`, `Plan58OutpostSettlementIntegrationTests` | ❌ GAP |
| 149 | `cryo_vault` | Shelter | `CryoVaultSystem` | `cryo_cultivars.json` | `Main` | `CryoVaultSaveStore` | *None (GAP)* | , `CryoVaultB69Tests` | ❌ GAP |
| 150 | `disaster_response` | Shelter | `DisasterResponseSystem` | `disaster_templates.json` | `Main` | `DisasterResponseSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan158DisasterResponseIntegrationTests` | ❌ GAP |
| 151 | `ebpvd_coating` | Shelter | `EbPvdCoatingEngine` | `ebpvd_coating_catalog.json` | `EbPvdCoatingHostSession` | `EbPvdCoatingSaveStore` | `EbPvdCoatingPanel` | `--ebpvd-coating-uitest`, `EbPvdCoatingEngineTests` | ✅ 6/6 |
| 152 | `excavation_hazards` | Shelter | `ExcavationHazardSystem` | — *(Procedural)* | `Main` | `ExcavationHazardSaveStore` | `GameDashboardPanel` | `--shelter-hazard-selftest`, `ExcavationSystemTests` | ✅ 6/6 |
| 153 | `food_preservation` | Shelter | `FoodPreservationSystem` | `food_preservation.json` | `Main` | `FoodPreservationSaveStore` | *None (GAP)* | , `FoodPreservationSystemTests` | ❌ GAP |
| 154 | `geothermal_aquifer` | Shelter | `GeothermalAquiferSystem` | — *(Procedural)* | `GeothermalAquiferSaveStore` | `GeothermalAquiferSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 155 | `precision_metrology` | Shelter | `PrecisionMetrologySystem` | `metrology_standards_catalog.json` | `Main` | `PrecisionMetrologySaveStore` | *None (GAP)* | `--precision-metrology-selftest`, `PrecisionMetrologySystemTests` | ❌ GAP |
| 156 | `precision_optics` | Shelter | `PrecisionOpticsEngine` | — *(Procedural)* | `PrecisionOpticsHostSession` | `PrecisionOpticsSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 157 | `radio_station` | Shelter | `ShelterRadioStationSystem` | — *(Procedural)* | `Main` | `RadioStationSaveStore` | `RadioPanel`, `GameDashboardPanel` | `--core-selftest`, `ShelterRadioStationTests` | ✅ 6/6 |
| 158 | `seismic_dynamics` | Shelter | `SeismicDynamicsSystem` | `seismic_fault_catalog.json` | `Main` | `SeismicDynamicsSaveStore` | *None (GAP)* | , `ShelterSeismicDynamicsPlan56Tests`, `SeismicMonitoringB68Tests` | ❌ GAP |
| 159 | `shelter_atmosphere` | Shelter | `ShelterAtmosphereSystem` | — *(Procedural)* | `ShelterAtmosphereHostSession` | `ShelterAtmosphereSaveStore` | `ShelterAtmospherePanel`, `GameDashboardPanel` | `--shelter-atmosphere-selftest`, `Plan220ShelterAtmosphereIntegrationTests` | ✅ 6/6 |
| 160 | `shelter_decor` | Shelter | `ShelterDecorSystem` | — *(Procedural)* | `ShelterDecorHostSession` | `ShelterDecorSaveStore` | `GameDashboardPanel` | `--shelter-decor-selftest`, `Plan12CDecorTests` | ✅ 6/6 |
| 161 | `shelter_expansion` | Shelter | `ShelterExpansionSystem` | `shelter_construction.json` | `Main` | `ShelterExpansionSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan156ShelterExpansionIntegrationTests` | ❌ GAP |
| 162 | `shelter_noise` | Shelter | `ShelterNoiseSystem` | — *(Procedural)* | `ShelterAtmosphereHostSession` | `ShelterNoiseSaveStore` | `ShelterAtmospherePanel` | `--shelter-atmosphere-selftest`, `Plan220ShelterAtmosphereIntegrationTests` | ✅ 6/6 |
| 163 | `shelter_social_dynamics` | Shelter | `ShelterSocialDynamicsSystem` | `shelter_social_events.json` | `Main` | `ShelterSocialSaveStore` | `GameDashboardPanel` | `--core-selftest`, `ShelterSocialDynamicsTests` | ✅ 6/6 |
| 164 | `shelter_workshop` | Shelter | `ShelterWorkshopSystem` | — *(Procedural)* | `Main` | `ShelterWorkshopSaveStore` | `WorkshopPanel`, `GameDashboardPanel` | `--core-selftest`, `WorkshopReverseEngineeringSystemTests` | ✅ 6/6 |
| 165 | `weather_hardening` | Shelter | `WeatherHardeningSystem` | — *(Procedural)* | `WeatherHardeningSaveStore` | `WeatherHardeningSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 166 | `cvd_diamond` | Shelter & Facilities | `CvdDiamondSynthesisEngine` | `cvd_diamond_catalog.json` | `CvdDiamondHostSession` | `CvdDiamondSaveStore` | `CvdDiamondPanel` | `--plans-122-125-selftest`, `Plan124CvdDiamondSynthesisEngineTests` | ✅ 6/6 |
| 167 | `sofc_power` | Shelter & Facilities | `SofcElectrochemistryEngine` | `sofc_power_catalog.json` | `SofcPowerHostSession` | `SofcPowerSaveStore` | `SolidOxideFuelCellPanel` | `--plans-122-125-selftest`, `Plan122SofcElectrochemistryEngineTests` | ✅ 6/6 |
| 168 | `bio_fermentation` | Shelter & Farming | `BioFermentationEngine` | `bio_fermentation_catalog.json` | `BioFermentationHostSession` | `BioFermentationSaveStore` | `BioFermentationPanel` | , `BioFermentationEngineTests` | ❌ GAP |
| 169 | `hydroponic_biomes` | Shelter & Farming | `HydroponicBiomeSystem` | `hydroponic_crops.json` | `Main` | `HydroponicBiomeSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `HydroponicBiomeTests` | ✅ 6/6 |
| 170 | `airlock_security` | Shelter & Infrastructure | `AirlockSecuritySystem` | — *(Procedural)* | `AirlockSecurityHostSession` | `AirlockSecuritySaveStore` | `AirlockSecurityPanel` | `--shelter-operations-selftest`, `AirlockSecuritySystemTests` | ✅ 6/6 |
| 171 | `decontamination` | Shelter & Infrastructure | `DecontaminationSystem` | — *(Procedural)* | `DecontaminationHostSession` | `DecontaminationSaveStore` | `DecontaminationPanel` | `--shelter-operations-selftest`, `DecontaminationSystemTests` | ✅ 6/6 |
| 172 | `excavation` | Shelter & Infrastructure | `ExcavationSystem` | — *(Procedural)* | `ExcavationHostSession` | `ExcavationSaveStore` | `ExcavationPanel` | `--shelter-operations-selftest`, `ExcavationSystemTests` | ✅ 6/6 |
| 173 | `greenhouse` | Shelter & Infrastructure | `GreenhouseSystem` | `greenhouse_items.json` | `GreenhouseHostSession` | `GreenhouseSaveStore` | `GreenhousePanel` | `--greenhouse-selftest`, `GreenhouseSystemTests` | ✅ 6/6 |
| 174 | `nuclear_core_lifecycle` | Shelter & Infrastructure | `NuclearCoreLifecycleSystem` | `nuclear_core_profiles.json` | `Main` | `NuclearCoreSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `NuclearCorePowerGridPublishTests` | ✅ 6/6 |
| 175 | `power_grid` | Shelter & Infrastructure | `PowerGridSystem` | `power_grid.json` | `PowerGridHostSession` | `PowerGridSaveStore` | `PowerGridPanel` | `--player-panels-uitest`, `PowerGridSystemTests` | ✅ 6/6 |
| 176 | `power_subgrids` | Shelter & Infrastructure | `PowerDistributionSubgridSystem` | — *(Procedural)* | `Main` | `PowerDistributionSaveStore` | `PowerGridPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 177 | `sanitation` | Shelter & Infrastructure | `SanitationSystem`, `SanitationFacilityCatalog` | `sanitation_facilities.json` | `SanitationHostSession` | `SanitationSaveStore` | *None (GAP)* | , `Plan210SanitationSystemTests`, `Plan210SanitationFacilityCatalogTests`, `Plan210SanitationHostWiringTests` | ❌ GAP |
| 178 | `shelter_assignment` | Shelter & Infrastructure | `ShelterAssignmentSystem` | — *(Procedural)* | `ShelterAssignmentHostSession` | `ShelterAssignmentSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `ShelterAssignmentSystemTests` | ✅ 6/6 |
| 179 | `shelter_fire` | Shelter & Infrastructure | `ShelterFireHazardSystem` | — *(Procedural)* | `ShelterFireHostSession` | `ShelterFireSaveStore` | `FireIncidentPanel` | `--save-store-checksum-selftest`, `ShelterFireHazardSystemTests`, `FireIncidentJourneyTests` | ✅ 6/6 |
| 180 | `shelter_schedule` | Shelter & Infrastructure | `ShelterScheduleSystem` | `shelter_schedules.json` | `ShelterScheduleHostSession` | `ShelterScheduleSaveStore` | `ShelterSchedulePanel` | `--shelter-operations-selftest`, `ShelterScheduleIntegrationTests` | ✅ 6/6 |
| 181 | `shelter_thermal` | Shelter & Infrastructure | `ShelterThermalSystem` | — *(Procedural)* | `ShelterThermalHostSession` | `ShelterThermalSaveStore` | `ShelterThermalPanel` | `--shelter-operations-selftest`, `ShelterThermalSaveChecksumTests` | ✅ 6/6 |
| 182 | `starting_level` | Shelter & Infrastructure | `StartingLevelSystem` | — *(Procedural)* | `StartingLevelHostSession` | `StartingLevelSaveStore` | `OpeningProtocolModal` | `--playable-shell-selftest`, `StartingLevelSystemTests` | ✅ 6/6 |
| 183 | `sump_flooding` | Shelter & Infrastructure | `SumpFloodingSystem` | — *(Procedural)* | `SumpFloodingHostSession` | `SumpFloodingSaveStore` | `SumpFloodingPanel` | `--shelter-operations-selftest`, `SumpFloodingSaveChecksumTests` | ✅ 6/6 |
| 184 | `survivor_social` | Shelter & Infrastructure | `SurvivorSocialCoordinator`, `LeadershipSystem`, `IdeologicalFrictionSystem`, `RationConflictSystem`, `TraumaBondSystem`, `SkillAtrophySystem` | — *(Procedural)* | `SurvivorSocialCoordinator` | `SurvivorSocialSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `SurvivorSocialCoordinatorTests` | ✅ 6/6 |
| 185 | `vinyl_morale` | Shelter & Infrastructure | `VinylMoraleSystem` | — *(Procedural)* | `VinylMoraleHostSession` | `VinylMoraleSaveStore` | `VinylMoralePanel` | `--shelter-operations-selftest`, `VinylMoraleSaveChecksumTests` | ✅ 6/6 |
| 186 | `water_treatment` | Shelter & Infrastructure | `WaterTreatmentSystem` | — *(Procedural)* | `WaterTreatmentHostSession` | `WaterTreatmentSaveStore` | `WaterTreatmentPanel` | `--shelter-operations-selftest`, `WaterTreatmentSystemTests` | ✅ 6/6 |
| 187 | `crafting` | Shelter & Logistics | `CraftingSystem` | `recipes.json` | `CraftingHostSession` | `CraftingSaveStore` | `CraftingPanel` | `--shelter-operations-selftest`, `CraftingSystemTests` | ✅ 6/6 |
| 188 | `equipment_condition` | Shelter & Logistics | `EquipmentConditionSystem` | — *(Procedural)* | `EquipmentConditionHostSession` | `EquipmentConditionSaveStore` | `EquipmentConditionPanel` | `--shelter-operations-selftest`, `EquipmentConditionSystemTests` | ✅ 6/6 |
| 189 | `inventory` | Shelter & Logistics | `Inventory` | `items.json` | `InventoryHostSession` | `InventorySaveStore` | `InventoryPanel`, `InventoryDetailPanel` | `--inventory-save-selftest`, `--inventory-uitest`, `InventorySystemTests` | ✅ 6/6 |
| 190 | `kitchen_nutrition` | Shelter & Logistics | `KitchenNutritionSystem` | — *(Procedural)* | `KitchenNutritionHostSession` | `KitchenNutritionSaveStore` | `KitchenNutritionPanel` | `--shelter-operations-selftest`, `KitchenNutritionSystemTests` | ✅ 6/6 |
| 191 | `radio` | Shelter & Logistics | `FactionRadioEngine`, `RadioStationCatalog`, `RadioStationCatalogLoader` | `radio.json`, `radio_stations.json` | `RadioHostSession` | `RadioSaveStore` | `RadioPanel`, `FactionRadioHudPanel` | `--radio-selftest`, `--radio-catalog-selftest`, `RadioSaveCodecTests`, `RadioStationCatalogTests`, `RadioStationParityTests` | ✅ 6/6 |
| 192 | `shelter_reputation` | Shelter (Plan 207) | `ShelterReputationSystem` | — *(Procedural)* | `ShelterReputationHostSession` | `ShelterReputationSaveStore` | `ShelterReputationPanel`, `GameDashboardPanel` | `--shelter-reputation-selftest`, `Plan207ShelterReputationIntegrationTests` | ✅ 6/6 |
| 193 | `shelter_security` | Shelter Defense (Plan 138) | `ShelterSecuritySystem` | — *(Procedural)* | `ShelterSecurityHostSession` | `ShelterSecuritySaveStore` | `ShelterSecurityPanel`, `GameDashboardPanel` | `--shelter-security-selftest`, `Plan138ShelterSecurityIntegrationTests` | ✅ 6/6 |
| 194 | `relationship_decay` | Social Ecology & Drift (Plan 182) | `RelationshipDecaySystem` | — *(Procedural)* | `RelationshipDecayHostSession` | `RelationshipDecaySaveStore` | `RelationshipDecayPanel`, `GameDashboardPanel` | `--relationship-decay-selftest`, `Plan182RelationshipDecayIntegrationTests`, `RelationshipDecaySystemTests` | ✅ 6/6 |
| 195 | `hydrogeology_archive` | Subterranean Science Archive | `HydroGeologyDiscoverySystem` | `artesian_well_contamination_logs.json`, `cave_aquatic_biota_logs.json`, `geothermal_steam_vent_diagnostics.json`, `stalactite_mineral_assay_reports.json` | `Main` | `HydroGeologyArchiveSaveStore` | *None (GAP)* | , `HydroGeologyDiscoveryTests`, `HydroGeologyCatalogTests` | ❌ GAP |
| 196 | `apprenticeship` | Survival & Biology | `ApprenticeshipSystem` | — *(Procedural)* | `ApprenticeshipHostSession` | `ApprenticeshipSaveStore` | `ApprenticeshipPanel` | `--shelter-operations-selftest`, `ApprenticeshipSystemTests` | ✅ 6/6 |
| 197 | `autopsy` | Survival & Biology | `AutopsySystem` | `autopsy_procedures.json` | `AutopsyHostSession` | `AutopsySaveStore` | `AutopsyReportPanel` | `--shelter-operations-selftest`, `AutopsySystemTests` | ✅ 6/6 |
| 198 | `caregiving` | Survival & Biology | `CaregivingSystem` | — *(Procedural)* | `CaregivingHostSession` | `CaregivingSaveStore` | `CaregivingPanel` | `--shelter-operations-selftest`, `CaregivingSystemTests` | ✅ 6/6 |
| 199 | `chemical_dependency` | Survival & Biology | `ChemicalDependencySystem` | `chemical_dependency_items.json` | `MentalHealthCrisisHostSession`, `ChemicalDependencyHostSession` | `ChemicalDependencySaveStore` | `ChemicalDependencyPanel` | `--chemical-dependency-save-selftest`, `ChemicalDependencySaveSealTests` | ✅ 6/6 |
| 200 | `contractor_roster` | Survival & Biology | `ContractorRosterSystem` | — *(Procedural)* | `ContractorRosterHostSession` | `ContractorRosterSaveStore` | `ContractorRosterPanel` | `--shelter-operations-selftest`, `ContractorRosterSystemTests` | ✅ 6/6 |
| 201 | `disease` | Survival & Biology | `DiseaseSystem` | `disease_catalog.json` | `DiseaseHostSession` | `DiseaseSaveStore` | `AfflictionsPanel` | `--disease-selftest`, `DiseaseSystemTests` | ✅ 6/6 |
| 202 | `medical` | Survival & Biology | `MedicalWardSystem`, `SickListSystem` | `medical_texts.json` | `MedicalHostSession` | `MedicalSaveStore` | `MedicalPanel`, `AfflictionsPanel` | `--medical-selftest`, `DwellerMedicalCatalogTests` | ✅ 6/6 |
| 203 | `medical_ward` | Survival & Biology | `MedicalWardSystem` | — *(Procedural)* | `MedicalWardHostSession` | `MedicalWardSaveStore` | `MedicalWardPanel` | `--medical-ward-save-selftest`, `MedicalWardSystemTests` | ✅ 6/6 |
| 204 | `mental_health_crisis` | Survival & Biology | `MentalHealthCrisisSystem` | — *(Procedural)* | `MentalHealthCrisisHostSession` | `MentalHealthCrisisSaveStore` | `MentalHealthCrisisPanel` | `--shelter-operations-selftest`, `MentalHealthCrisisSystemTests` | ✅ 6/6 |
| 205 | `morale_contagion` | Survival & Biology | `MoraleContagionSystem` | — *(Procedural)* | `MoraleContagionHostSession` | `MoraleContagionSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `MoraleContagionSystemTests` | ✅ 6/6 |
| 206 | `survivor_relations` | Survival & Biology | `SurvivorRelationsSystem` | — *(Procedural)* | `SurvivorRelationsHostSession` | `SurvivorRelationsSaveStore` | `SurvivorRelationsPanel` | `--shelter-operations-selftest`, `SurvivorRelationsSaveChecksumTests` | ✅ 6/6 |
| 207 | `survivors` | Survival & Biology | `NeedsSystem`, `SurvivorRosterSystem` | `survivors.json` | `SurvivorsHostSession` | `SurvivorsSaveStore` | `SurvivorsPanel`, `SurvivorDetailPanel`, `StatusPanel` | `--survivors-selftest`, `--survivors-uitest`, `--player-panels-uitest`, `NeedsSystemTests` | ✅ 6/6 |
| 208 | `death_legacy` | Survivor Memorial & Wills (Plan 206) | `SurvivorDeathLegacySystem` | — *(Procedural)* | `SurvivorDeathLegacyHostSession` | `SurvivorDeathLegacySaveStore` | `SurvivorDeathLegacyPanel`, `GameDashboardPanel` | `--death-legacy-selftest`, `Plan206SurvivorDeathLegacyIntegrationTests`, `SurvivorDeathLegacySystemTests` | ✅ 6/6 |
| 209 | `personal_quests` | Survivor Quests (Plan 200) | `PersonalQuestSystem` | `personal_quests.json` | `PersonalQuestHostSession` | `PersonalQuestSaveStore` | `PersonalQuestPanel`, `GameDashboardPanel` | `--personal-quests-selftest`, `Plan200PersonalQuestsIntegrationTests`, `PersonalQuestSystemTests` | ✅ 6/6 |
| 210 | `backstory` | Survivors | `BackstorySystem`, `BackstoryCensus` | `backstory_templates.json` | `Main`, `BackstoryHostSession` | `BackstorySaveStore` | `SurvivorDetailPanel` | `--backstory-selftest`, `Plan174BackstoryHostIntegrationTests`, `Plan174SurvivorBackstoriesIntegrationTests` | ✅ 6/6 |
| 211 | `ideological_friction` | Survivors | `IdeologicalFrictionEvents`, `IdeologicalFrictionSystem`, `IdeologicalEventInstance`, `IdeologicalFrictionCensus` | `ideological_events.json` | `Main`, `IdeologicalFrictionHostSession` | `IdeologicalFrictionSaveStore` | `SurvivorsPanel`, `SurvivorDetailPanel` | `--ideological-friction-selftest`, `Plan148IdeologicalFrictionHostIntegrationTests`, `Plan148IdeologicalFrictionIntegrationTests`, `IdeologicalFrictionSystemTests` | ✅ 6/6 |
| 212 | `romance_family` | Survivors | `RomanceFamilySystem`, `RomanticRelationship`, `FamilyUnit`, `RomanceCourtshipCatalog`, `RomanceFamilyCensus`, `RomanceCourtshipCatalogLoader` | `romance_courtship.json` | `Main`, `RomanceFamilyHostSession` | `RomanceFamilySaveStore` | `SurvivorDetailPanel` | `--romance-family-selftest`, `Plan150RomanceFamilyHostIntegrationTests`, `Plan150RomanceFamilyIntegrationTests`, `RomanceCourtshipCatalogLoaderTests` | ✅ 6/6 |
| 213 | `survivor_voice` | Survivors | `SurvivorVoiceSystem`, `VoiceLineDispatchCoordinator` | `survivor_voice_lines.json` | `Main`, `SurvivorVoiceHostSession` | `SurvivorVoiceSaveStore` | *None (GAP)* | `--survivor-voice-selftest`, `Plan42SurvivorVoiceHostIntegrationTests`, `SurvivorVoiceSystemTests` | ❌ GAP |
| 214 | `hidden_agenda` | Survivors (Plan 132) | `HiddenAgendaSystem` | — *(Procedural)* | `HiddenAgendaHostSession` | `HiddenAgendaSaveStore` | `HiddenAgendaPanel`, `GameDashboardPanel` | `--hidden-agenda-selftest`, `Plan132HiddenAgendaIntegrationTests`, `HiddenAgendaSystemTests` | ✅ 6/6 |
| 215 | `combat` | Tactical Combat | `TacticalCombatSystem`, `CombatTraumaSystem` | `combat_catalog.json` | `CombatHostSession` | `CombatSaveStore` | `CombatPanel`, `CombatDetailPanel`, `CombatHistoryPanel` | `--combat-selftest`, `CombatBallisticsTests` | ✅ 6/6 |
| 216 | `technical_material_archive` | Technical Material Archive | `TechnicalMaterialArchiveSystem` | `hemp_fiber_hackling_logs.json`, `wire_rope_stranding_assays.json`, `manila_hawser_breakage_reports.json`, `rope_transmission_splicing_audits.json`, `neoprene_gasket_degradation_logs.json`, `aramid_fiber_rot_reports.json`, `tire_retreading_compound_logs.json`, `celluloid_film_decomposition_records.json` | `Main` | `TechnicalMaterialArchiveSaveStore` | *None (GAP)* | , `TechnicalMaterialArchiveTests`, `CordageCableCatalogTests`, `PolymerTextileCatalogTests` | ❌ GAP |
| 217 | `vehicle_customization` | Vehicles | `VehicleCustomizationSystem`, `VehicleCustomizationCatalog`, `VehicleModule`, `VehicleCustomizationCensus`, `VehicleModuleCatalogLoader` | `vehicle_modules.json` | `Main`, `VehicleCustomizationHostSession` | `VehicleCustomizationSaveStore` | *None (GAP)* | `--vehicle-customization-selftest`, `Plan152VehicleCustomizationHostIntegrationTests`, `Plan152VehicleCustomizationIntegrationTests`, `VehicleModuleCatalogLoaderTests` | ❌ GAP |
| 218 | `deep_well` | Water & Infrastructure | `DeepWellSystem` | — *(Procedural)* | `DeepWellHostSession`, `DeepWellSaveStore` | `DeepWellSaveStore` | *None (GAP)* | , `DeepWellSystemTests` | ❌ GAP |
| 219 | `piezometer_network` | Water & Infrastructure | `AquiferPiezometerEngine` | `piezometer_network_catalog.json` | `PiezometerHostSession` | `PiezometerSaveStore` | *None (GAP)* | , `Plan189IntakeAdvisoryBridgeTests` | ❌ GAP |
| 220 | `water_condenser` | Water & Infrastructure | `AtmosphericCondenserSystem` | — *(Procedural)* | `WaterCondenserHostSession`, `WaterCondenserSaveStore` | `WaterCondenserSaveStore` | *None (GAP)* | , `AtmosphericCondenserSystemTests` | ❌ GAP |
| 221 | `weather_cascade` | Weather | `WeatherCascadeSystem`, `WeatherGameplayCascadeEngine`, `WeatherCascadeSeverity`, `WeatherCascadeCatalogLoader` | `weather_gameplay_effects.json`, `weather_effects.json` | `WeatherCascadeHostSession`, `Main` | `WeatherCascadeSaveStore` | *None (GAP)* | `--weather-cascade-selftest`, `Plan135WeatherCascadeHostIntegrationTests`, `Plan135WeatherCascadeIntegrationTests` | ❌ GAP |
| 222 | `ecological_infestation` | World | `EcologicalInfestationSystem` | `micro_locations.json` | `Main` | `EcologicalInfestationSaveStore` | `GameDashboardPanel` | `--faction-ecology-selftest`, `EcologicalInfestationSystemTests` | ✅ 6/6 |
| 223 | `geodetic_survey` | World | `GeodeticSurveyEngine` | — *(Procedural)* | `GeodeticSurveySaveStore` | `GeodeticSurveySaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 224 | `human_migration` | World | `SeasonalHumanMigrationEngine`, `SeasonalMigrationCatalogLoader`, `HumanMigrationCensus` | `seasonal_human_migration.json` | `Main`, `HumanMigrationHostSession` | `HumanMigrationSaveStore` | *None (GAP)* | `--human-migration-selftest`, `Plan199HumanMigrationHostIntegrationTests`, `SeasonalHumanMigrationEngineTests` | ❌ GAP |
| 225 | `nuclear_winter_progression` | World | `NuclearWinterProgressionSystem` | `nuclear_winter_phases.json` | `Main` | `NuclearWinterSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan164NuclearWinterIntegrationTests` | ❌ GAP |
| 226 | `route_infrastructure` | World | `RouteInfrastructureSystem` | — *(Procedural)* | `RouteInfrastructureSaveStore` | `RouteInfrastructureSaveStore` | *None (GAP)* | , `RouteInfrastructureSystemTests` | ❌ GAP |
| 227 | `subterranean` | World | `SubterraneanSystem` | `subterranean_zones.json` | `SubterraneanHostSession` | `SubterraneanSaveStore` | *None (GAP)* | , `SubterraneanSystemTests` | ❌ GAP |
| 228 | `amphibious_draisine` | World & Expeditions | `AmphibiousDraisineEngine` | `amphibious_draisine_catalog.json` | `AmphibiousDraisineHostSession` | `AmphibiousDraisineSaveStore` | `AmphibiousDraisinePanel` | `--plans-122-125-selftest`, `Plan125AmphibiousDraisineEngineTests` | ✅ 6/6 |
| 229 | `armored_crawlers` | World & Expeditions | `ArmoredCrawlerExpeditionSystem` | `armored_crawler_modules.json` | `Main` | `ArmoredCrawlerSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FlagshipIntegrationIxSmokeTests` | ✅ 6/6 |
| 230 | `encounter_choice` | World & Expeditions | `EncounterChoiceResolver` | `door_encounters.json` | `EncounterChoiceState` | `EncounterChoiceSaveStore` | `DoorEncounterModal` | `--moral-choice-selftest`, `EncounterChoiceResolverTests` | ✅ 6/6 |
| 231 | `expedition` | World & Expeditions | `ExpeditionSystem`, `ExpeditionEncounterBridge` | `locations.json` | `ExpeditionHostSession` | `ExpeditionSaveStore` | `ExpeditionPanel` | `--expedition-selftest`, `--expedition-panel-uitest`, `ExpeditionCampSystemTests` | ✅ 6/6 |
| 232 | `insar_deformation` | World & Expeditions | `InSarDeformationEngine` | `insar_geodesy_catalog.json` | `InSarMappingHostSession` | `InSarMappingSaveStore` | `InSarMappingPanel` | `--plans-139-141-selftest`, `Plan139InSarDeformationTests` | ✅ 6/6 |
| 233 | `runflat_tire` | World & Expeditions | `RunFlatTireEngine` | `runflat_tire_catalog.json` | `RunFlatTireHostSession` | `RunFlatTireSaveStore` | `RunFlatTirePanel` | `--plans-139-141-selftest`, `Plan141RunFlatTireTests` | ✅ 6/6 |
| 234 | `travel_encounters` | World & Expeditions | `TravelEncounterSystem`, `TravelEncounterCatalog` | `travel_encounters.json` | `TravelEncounterSystem` | `TravelEncounterSaveStore` | `ExpeditionPanel` | `--expedition-encounter-bridge-selftest`, `TravelEncounterCooldownGroupTests`, `PatrolEncounterFullRegressionTests` | ✅ 6/6 |
| 235 | `wasteland_map` | World & Expeditions | `WastelandMapSystem` | `wasteland_map_v1.json` | `WorldHostSession` | `WastelandMapSaveStore` | `MapPanel` | `--world-selftest`, `WastelandMapPersistenceTests` | ✅ 6/6 |
| 236 | `waystation` | World & Expeditions | `WaystationSystem` | `locations.json` | `WaystationHostSession` | `WaystationSaveStore` | `WaystationNetworkPanel` | `--shelter-operations-selftest`, `WaystationSystemTests` | ✅ 6/6 |
| 237 | `wildlife_trapping` | World & Expeditions | `WildlifeTrappingSystem` | — *(Procedural)* | `WildlifeTrappingHostSession` | `WildlifeTrappingSaveStore` | `WildlifeTrappingPanel` | `--shelter-operations-selftest`, `WildlifeTrappingSystemTests` | ✅ 6/6 |
| 238 | `world` | World & Expeditions | `WastelandMapSystem`, `WeatherSystem` | `locations.json` | `WorldHostSession` | `WorldSaveStore` | `MapPanel`, `WeatherPanel` | `--world-selftest`, `WorldSaveablesTests` | ✅ 6/6 |

---

## 3. Subsystem Deep Evidence Graph & Source Paths

Detailed file paths and symbols proving zero conceptual placeholders:

### 1. `commitment` — Plan 38 — authored obligations/deadlines: warnings, met/missed terminal state, and consequence routing (Campaign)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCommitments()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Commitments/CommitmentSystem.cs`](../../Assets/Ashfall.Core/Commitments/CommitmentSystem.cs)
  - Host Session: [`src/Host/CommitmentHostSession.cs`](../../src/Host/CommitmentHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CommitmentHostSession.cs`](../../src/Host/CommitmentHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`](../../Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CommitmentSystemTests.cs`](../../Ashfall.Core.Tests/Campaign/CommitmentSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan38CommitmentHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan38CommitmentHostIntegrationTests.cs)

### 2. `endgame` — Campaign endgame phase, ending selection, sealed epilogue report (Campaign & Lore)
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

### 3. `host_event` — Host event ledger & moral decisions (Campaign & Lore)
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

### 4. `journal` — Player journal, logs, and codex entries (Campaign & Lore)
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

### 5. `memorial` — Fallen survivors memorial wall (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupMemorial()` | **Cadence:** `On-Demand (Survivor Fallen Eulogy)`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Save Store: [`src/Host/MemorialSaveStore.cs`](../../src/Host/MemorialSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`](../../Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs)

### 6. `narrative` — Branching story arcs and narrative flags (Campaign & Lore)
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

### 7. `phase0` — Pre-war timeline and bunker startup (Campaign & Lore)
- **Owner Domain:** `phase0`
- **Setup Method:** `Main.SetupPhase0()` | **Cadence:** `On-Demand (Pre-War Flashback)`
- **UI Routes:** `phase0`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs`](../../Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs)
  - Host Session: [`src/Host/Phase0HostSession.cs`](../../src/Host/Phase0HostSession.cs)
  - Save Store: [`src/Host/Phase0SaveStore.cs`](../../src/Host/Phase0SaveStore.cs)
  - UI Panel: [`src/UI/Phase0Panel.cs`](../../src/UI/Phase0Panel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`](../../Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs)

### 8. `survivor_fate` — Unified survivor-death ledger: one immutable fate record per deceased survivor (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupSurvivorFate()` | **Cadence:** `Daily Survivor-Death Cascade`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorFateSaveStore.cs`](../../src/Host/SurvivorFateSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SurvivorFateSystemTests.cs`](../../Ashfall.Core.Tests/SurvivorFateSystemTests.cs)

### 9. `onboarding` — First-hour onboarding journey progress, dismissed hints, assistance level, completion (Campaign & Onboarding)
- **Owner Domain:** `onboarding`
- **Setup Method:** `Main.SetupOnboarding()` | **Cadence:** `On-Demand (Player Sigil Recording)`
- **UI Routes:** `help`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`](../../Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OnboardingSaveStore.cs`](../../src/Host/OnboardingSaveStore.cs)
  - UI Panel: [`src/UI/OnboardingHintPanel.cs`](../../src/UI/OnboardingHintPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/OnboardingJourneyTests.cs`](../../Ashfall.Core.Tests/OnboardingJourneyTests.cs)

### 10. `archive_desk` — Document archiving, ink, and scribing (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupArchiveDesk()` | **Cadence:** `Daily Scribing & Folio Archival`
- **UI Routes:** `archive_desk`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ArchiveDeskSystem.cs`](../../Assets/Ashfall.Core/ArchiveDeskSystem.cs)
  - Host Session: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - Save Store: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - UI Panel: [`src/UI/ArchiveDeskPanel.cs`](../../src/UI/ArchiveDeskPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`](../../Ashfall.Core.Tests/ArchiveDeskSystemTests.cs)

### 11. `campaign_day` — Master campaign day counter & ticks (Campaign & Progression)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCampaignDay()` | **Cadence:** `Master Sim Clock / Dawn Advance`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Host Session: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Save Store: [`src/Host/CampaignDaySaveStore.cs`](../../src/Host/CampaignDaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`](../../Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs)

### 12. `daily_briefing` — Daily dawn briefing notes & status (Campaign & Progression)
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

### 13. `library_study` — Research library books and blueprints (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupLibraryStudy()` | **Cadence:** `Daily Codex Research Ticks`
- **UI Routes:** `library_study`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/LibraryStudySystem.cs`](../../Assets/Ashfall.Core/LibraryStudySystem.cs)
  - Host Session: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - Save Store: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - UI Panel: [`src/UI/LibraryStudyPanel.cs`](../../src/UI/LibraryStudyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/LibraryStudySystemTests.cs`](../../Ashfall.Core.Tests/LibraryStudySystemTests.cs)

### 14. `dynamic_quests` — Campaign-wide emergency dynamic quests (Campaign & Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupDynamicQuests()` | **Cadence:** `On-Demand (Campaign-Wide Emergency Quests)`
- **UI Routes:** `dynamic_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/DynamicQuestlines.cs`](../../Assets/Ashfall.Core/Quests/DynamicQuestlines.cs)
  - Host Session: [`src/Host/DynamicQuestSaveStore.cs`](../../src/Host/DynamicQuestSaveStore.cs)
  - Save Store: [`src/Host/DynamicQuestSaveStore.cs`](../../src/Host/DynamicQuestSaveStore.cs)
  - UI Panel: [`src/UI/DynamicQuestlinePanel.cs`](../../src/UI/DynamicQuestlinePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs`](../../Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs)

### 15. `narrative_questlines` — Survivor narrative questline arcs and crisis branch outcomes (Campaign & Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupNarrativeQuestlines()` | **Cadence:** `On-Demand (Survivor Narrative Arc Progression)`
- **UI Routes:** `quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`](../../Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs)
  - Host Session: [`src/Host/NarrativeQuestlineHostSession.cs`](../../src/Host/NarrativeQuestlineHostSession.cs)
  - Save Store: [`src/Host/NarrativeQuestlineSaveStore.cs`](../../src/Host/NarrativeQuestlineSaveStore.cs)
  - UI Panel: [`src/UI/QuestsPanel.cs`](../../src/UI/QuestsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`](../../Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs)

### 16. `chlor_alkali_synthesis` — Plans 110-113 — chlor-alkali electrolytic plant, membrane health, hazard load, and chemical production (Chemistry)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupChlorAlkali()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs`](../../Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs)
  - Host Session: [`src/Host/ChlorAlkaliHostSession.cs`](../../src/Host/ChlorAlkaliHostSession.cs)
  - Save Store: [`src/Host/ChlorAlkaliSaveStore.cs`](../../src/Host/ChlorAlkaliSaveStore.cs)

### 17. `ballistic_shield` — Plans 110-113 — defensive ballistic shields, stances, integrity, and ground anchoring (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupBallisticShield()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/BallisticShieldEngine.cs`](../../Assets/Ashfall.Core/Combat/BallisticShieldEngine.cs)
  - Host Session: [`src/Host/BallisticShieldHostSession.cs`](../../src/Host/BallisticShieldHostSession.cs)
  - Save Store: [`src/Host/BallisticShieldSaveStore.cs`](../../src/Host/BallisticShieldSaveStore.cs)

### 18. `ballistics_workbench` — Plan B75 — weapon calibration, headspace wear, custom ammunition, and failure state (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupBallisticsWorkbench()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs`](../../Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 19. `settlement_defenses` — Plans 162-165 — trap installations, pre-combat raid resolution, captures, and the raid log (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupDefense()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `defense_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Defense/DefenseSystem.cs`](../../Assets/Ashfall.Core/Defense/DefenseSystem.cs)
  - Host Session: [`src/Host/DefenseHostSession.cs`](../../src/Host/DefenseHostSession.cs)
  - Save Store: [`src/Host/DefenseSaveStore.cs`](../../src/Host/DefenseSaveStore.cs)
  - UI Panel: [`src/UI/DefenseGridPanel.cs`](../../src/UI/DefenseGridPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DefenseSystemTests.cs`](../../Ashfall.Core.Tests/DefenseSystemTests.cs)

### 20. `sky_defense_battery` — Kinetic sky-layer counter-battery: turret state, magazine, tracks, maintenance (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupSkyDefense()` | **Cadence:** `On-Demand`
- **UI Routes:** `sky_defense_battery`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs`](../../Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SkyDefenseBatterySaveStore.cs`](../../src/Host/SkyDefenseBatterySaveStore.cs)
  - UI Panel: [`src/UI/SkyDefenseBatteryPanel.cs`](../../src/UI/SkyDefenseBatteryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SkyDefenseBatteryTests.cs`](../../Ashfall.Core.Tests/SkyDefenseBatteryTests.cs)

### 21. `perimeter_defense` — Surface perimeter defense emplacements (Combat & Defense)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupPerimeterDefense()` | **Cadence:** `Daily Emplacement Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs`](../../Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PerimeterDefenseSaveStore.cs`](../../src/Host/PerimeterDefenseSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs`](../../Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs)

### 22. `sound_ranging` — Plan 123 — defensive sound-ranging calibration, node status, observation history, threat estimate (Combat & Defense)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupSoundRanging()` | **Cadence:** `Event-Driven (Hostile-Fire Observations) + Daily Drift`
- **UI Routes:** `sound_ranging`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs`](../../Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs)
  - Host Session: [`src/Host/SoundRangingHostSession.cs`](../../src/Host/SoundRangingHostSession.cs)
  - Save Store: [`src/Host/SoundRangingSaveStore.cs`](../../src/Host/SoundRangingSaveStore.cs)
  - UI Panel: [`src/UI/SoundRangingPanel.cs`](../../src/UI/SoundRangingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/Plan123SoundRangingThreatEngineTests.cs`](../../Ashfall.Core.Tests/Combat/Plan123SoundRangingThreatEngineTests.cs)

### 23. `time_capsules` — Plan 212 — time capsules, legacy messages, delayed discovery, and cross-generational communication (Communication & Heritage (Plan 212))
- **Owner Domain:** `communication`
- **Setup Method:** `Main.SetupTimeCapsules()` | **Cadence:** `Daily (Scheduled Opening & Message Delivery)`
- **UI Routes:** `time_capsule`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs`](../../Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs)
  - Host Session: [`src/Host/TimeCapsuleHostSession.cs`](../../src/Host/TimeCapsuleHostSession.cs)
  - Save Store: [`src/Host/TimeCapsuleSaveStore.cs`](../../src/Host/TimeCapsuleSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/TimeCapsulePanel.cs`](../../src/UI/TimeCapsulePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communication/Plan212TimeCapsuleIntegrationTests.cs`](../../Ashfall.Core.Tests/Communication/Plan212TimeCapsuleIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communication/TimeCapsuleSystemTests.cs`](../../Ashfall.Core.Tests/Communication/TimeCapsuleSystemTests.cs)

### 24. `chemical_synthesis` — Chemical synthesis retorts and apparatus (Crafting & Chemistry)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupChemicalSynthesis()` | **Cadence:** `On-Demand (Retort Synthesis)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs`](../../Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs)
  - Host Session: [`src/Host/ChemicalSynthesisHostSession.cs`](../../src/Host/ChemicalSynthesisHostSession.cs)
  - Save Store: [`src/Host/ChemicalSynthesisSaveStore.cs`](../../src/Host/ChemicalSynthesisSaveStore.cs)
  - UI Panel: [`src/UI/ChemicalLabPanel.cs`](../../src/UI/ChemicalLabPanel.cs)

### 25. `trade_routes` — Plan 192 — Scheduled trade route contracts, tariffs, reliability tiers, and exclusive goods (Economy)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupTradeRoutes()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs`](../../Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/TradeRouteContract.cs`](../../Assets/Ashfall.Core/Economy/TradeRouteContract.cs)
  - Host Session: [`src/Host/TradeRouteHostSession.cs`](../../src/Host/TradeRouteHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TradeRouteHostSession.cs`](../../src/Host/TradeRouteHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan192TradeRouteHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Economy/Plan192TradeRouteHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/TradeRouteContractTests.cs`](../../Ashfall.Core.Tests/Economy/TradeRouteContractTests.cs)

### 26. `black_market` — Plan 211 — underworld contacts, stock snapshots, debts, heat, and trust (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupBlackMarket()` | **Cadence:** `Daily Underworld Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/BlackMarketInventoryCatalog.cs`](../../Assets/Ashfall.Core/Economy/BlackMarketInventoryCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`](../../Assets/Ashfall.Core/Economy/BlackMarketSystem.cs)
  - Host Session: [`src/Host/BlackMarketHostSession.cs`](../../src/Host/BlackMarketHostSession.cs)
  - Save Store: [`src/Host/BlackMarketSaveStore.cs`](../../src/Host/BlackMarketSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs`](../../Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan211BlackMarketTests.cs`](../../Ashfall.Core.Tests/Economy/Plan211BlackMarketTests.cs)

### 27. `caravan` — Trade caravans, routes, and arrivals (Economy & Trade)
- **Owner Domain:** `caravans`
- **Setup Method:** `Main.SetupCaravans()` | **Cadence:** `Daily Route Travel`
- **UI Routes:** `traveling_caravan`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/TravelingCaravanSystem.cs`](../../Assets/Ashfall.Core/TravelingCaravanSystem.cs)
  - Host Session: [`src/Host/TravelingCaravanHostSession.cs`](../../src/Host/TravelingCaravanHostSession.cs)
  - Save Store: [`src/Host/CaravanSaveStore.cs`](../../src/Host/CaravanSaveStore.cs)
  - UI Panel: [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TradeCaravanCatalogTests.cs`](../../Ashfall.Core.Tests/TradeCaravanCatalogTests.cs)

### 28. `caravan_trade_network` — Faction caravan trade network routes and arrivals (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupCaravanTrade()` | **Cadence:** `Daily Route Arrival Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs`](../../Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CaravanTradeSaveStore.cs`](../../src/Host/CaravanTradeSaveStore.cs)
  - UI Panel: [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs`](../../Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs)

### 29. `economy` — Dynamic economy rates and market orders (Economy & Trade)
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

### 30. `regional_treaty` — Faction treaties and non-aggression pacts (Economy & Trade)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupRegionalTreaty()` | **Cadence:** `Daily Non-Aggression Decay`
- **UI Routes:** `regional_treaty`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/RegionalTreatySystem.cs`](../../Assets/Ashfall.Core/RegionalTreatySystem.cs)
  - Host Session: [`src/Host/RegionalTreatyHostSession.cs`](../../src/Host/RegionalTreatyHostSession.cs)
  - Save Store: [`src/Host/RegionalTreatySaveStore.cs`](../../src/Host/RegionalTreatySaveStore.cs)
  - UI Panel: [`src/UI/RegionalTreatyPanel.cs`](../../src/UI/RegionalTreatyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 31. `meta_progression` — Plan 175 — Meta progression & cross-run profile store: prestige scoring, crests, and NG+ boons (Endgame)
- **Owner Domain:** `endgame`
- **Setup Method:** `Main.SetupMetaProgression()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `epilogue`, `chronicle`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Endgame/CrossRunProfileStore.cs`](../../Assets/Ashfall.Core/Endgame/CrossRunProfileStore.cs)
  - Core System: [`Assets/Ashfall.Core/Endgame/MetaProgressionSystem.cs`](../../Assets/Ashfall.Core/Endgame/MetaProgressionSystem.cs)
  - Host Session: [`src/Host/MetaProgressionHostSession.cs`](../../src/Host/MetaProgressionHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MetaProgressionHostSession.cs`](../../src/Host/MetaProgressionHostSession.cs)
  - UI Panel: [`src/UI/ChroniclePanel.cs`](../../src/UI/ChroniclePanel.cs)
  - UI Panel: [`src/UI/EpiloguePanel.cs`](../../src/UI/EpiloguePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Endgame/Plan175MetaProgressionHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Endgame/Plan175MetaProgressionHostIntegrationTests.cs)

### 32. `unified_ending` — Plan 145 — Unified ending resolution & epilogue personalization: political, social, moral, personal, and expedition resolution (Endgame)
- **Owner Domain:** `endgame`
- **Setup Method:** `Main.SetupUnifiedEnding()` | **Cadence:** `On-Demand (Campaign Sealed)`
- **UI Routes:** `epilogue`, `chronicle`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs`](../../Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs)
  - Host Session: [`src/Host/UnifiedEndingHostSession.cs`](../../src/Host/UnifiedEndingHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/UnifiedEndingHostSession.cs`](../../src/Host/UnifiedEndingHostSession.cs)
  - UI Panel: [`src/UI/ChroniclePanel.cs`](../../src/UI/ChroniclePanel.cs)
  - UI Panel: [`src/UI/EpiloguePanel.cs`](../../src/UI/EpiloguePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingIntegrationTests.cs`](../../Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingIntegrationTests.cs)

### 33. `expansion_hub` — Expansion hub discovery state (Expansion Framework)
- **Owner Domain:** `expansion_hub`
- **Setup Method:** `Main.SetupExpansions()` | **Cadence:** `Daily Hub Tick`
- **UI Routes:** `expansions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExpansionMasterSession.cs`](../../Assets/Ashfall.Core/ExpansionMasterSession.cs)
  - Host Session: [`src/Host/ExpansionHostSession.cs`](../../src/Host/ExpansionHostSession.cs)
  - Save Store: [`src/Host/ExpansionHubSaveStore.cs`](../../src/Host/ExpansionHubSaveStore.cs)
  - UI Panel: [`src/UI/ExpansionsHubPanel.cs`](../../src/UI/ExpansionsHubPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpansionHubSaveTests.cs`](../../Ashfall.Core.Tests/ExpansionHubSaveTests.cs)

### 34. `expansion_quest` — Expansion questline progression (Expansion Framework)
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

### 35. `holdfast` — Holdfast S1 bunker state (Expansions (Exp 01))
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

### 36. `holdfast_trade` — Holdfast trade session state (Expansions (Exp 01))
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

### 37. `duty_roster` — Duty roster shifts and assignments (Expansions (Exp 02))
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

### 38. `phantom_memory` — Phantom memory lineages and echoes (Expansions (Exp 03))
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

### 39. `thirdonary` — Thirdonary covenant & dispute states (Expansions (Exp 04))
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

### 40. `year_of_ash` — The Year of Ash harsh winter state (Expansions (Exp 05))
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

### 41. `muster` — The Muster military rally & conflict state (Expansions (Exp 06))
- **Owner Domain:** `muster`
- **Setup Method:** `Main.SetupMuster()` | **Cadence:** `On-Demand (Rally Stance)`
- **UI Routes:** `muster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Muster/MusterSystem.cs`](../../Assets/Ashfall.Core/Muster/MusterSystem.cs)
  - Host Session: [`src/Host/MusterHostSession.cs`](../../src/Host/MusterHostSession.cs)
  - Save Store: [`src/Host/MusterSaveStore.cs`](../../src/Host/MusterSaveStore.cs)
  - UI Panel: [`src/UI/MusterPanel.cs`](../../src/UI/MusterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MusterSystemTests.cs`](../../Ashfall.Core.Tests/MusterSystemTests.cs)

### 42. `dose_ledger` — Survivor radiation dose ledger & cohorts (Expansions (Exp 07))
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

### 43. `verdict` — The Verdict investigation and tribunal state (Expansions (Exp 08))
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

### 44. `maritime` — The Black Flotilla dives and naval wrecks (Expansions (Exp 09))
- **Owner Domain:** `maritime`
- **Setup Method:** `Main.SetupMaritime()` | **Cadence:** `On-Demand (Dive Sortie)`
- **UI Routes:** `maritime`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs)
  - Host Session: [`src/Host/MaritimeHostSession.cs`](../../src/Host/MaritimeHostSession.cs)
  - Save Store: [`src/Host/MaritimeSaveStore.cs`](../../src/Host/MaritimeSaveStore.cs)
  - UI Panel: [`src/UI/MaritimePanel.cs`](../../src/UI/MaritimePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BlackFlotillaTests.cs`](../../Ashfall.Core.Tests/BlackFlotillaTests.cs)

### 45. `silent_foundry` — Automated foundry machinery & smelters (Expansions (Exp 10))
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupSilentFoundry()` | **Cadence:** `Daily Smelter Cycle`
- **UI Routes:** `silent_foundry`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`](../../Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs)
  - Host Session: [`src/Foundry/SilentFoundryHostSession.cs`](../../src/Foundry/SilentFoundryHostSession.cs)
  - Save Store: [`src/Host/SilentFoundrySaveStore.cs`](../../src/Host/SilentFoundrySaveStore.cs)
  - UI Panel: [`src/UI/SilentFoundryPanel.cs`](../../src/UI/SilentFoundryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`](../../Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs)

### 46. `chemical_recon` — Plans 78-81 — chemical hazard observations, samples, and safe corridors (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupChemicalRecon()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs`](../../Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs)
  - Host Session: [`src/Host/ChemicalReconSaveStore.cs`](../../src/Host/ChemicalReconSaveStore.cs)
  - Save Store: [`src/Host/ChemicalReconSaveStore.cs`](../../src/Host/ChemicalReconSaveStore.cs)

### 47. `colony` — ORPHAN-SEAL-W1 — player-founded colonies, buildings, and supply lines (Expeditions)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupColony()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ColonySystem.cs`](../../Assets/Ashfall.Core/Expeditions/ColonySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan160ColonyIntegrationTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan160ColonyIntegrationTests.cs)

### 48. `draisine_recovery` — Plans 130-133 — armored draisine derailment recovery (Expeditions)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupDraisineRerailing()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs`](../../Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 49. `mine_clearing_flail` — Plans 146-149 — mine-clearing flail vehicle modules and active breaches (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupMineClearingFlail()` | **Cadence:** `On-Demand`
- **UI Routes:** `mine_flail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs`](../../Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs)
  - Host Session: [`src/Host/MineClearingFlailHostSession.cs`](../../src/Host/MineClearingFlailHostSession.cs)
  - Save Store: [`src/Host/MineClearingFlailSaveStore.cs`](../../src/Host/MineClearingFlailSaveStore.cs)
  - UI Panel: [`src/UI/MineFlailPanel.cs`](../../src/UI/MineFlailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/MineClearingFlailEngineTests.cs`](../../Ashfall.Core.Tests/Expeditions/MineClearingFlailEngineTests.cs)

### 50. `rail_grinding` — Plans 146-149 — rail grinding vehicle modules and active corridor jobs (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupRailGrinding()` | **Cadence:** `On-Demand`
- **UI Routes:** `rail_grinding`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs`](../../Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs)
  - Host Session: [`src/Host/RailGrindingHostSession.cs`](../../src/Host/RailGrindingHostSession.cs)
  - Save Store: [`src/Host/RailGrindingSaveStore.cs`](../../src/Host/RailGrindingSaveStore.cs)
  - UI Panel: [`src/UI/RailGrindingPanel.cs`](../../src/UI/RailGrindingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/RailGrindingEngineTests.cs`](../../Ashfall.Core.Tests/Expeditions/RailGrindingEngineTests.cs)

### 51. `recon_telemetry` — Long-range recon drones & high-altitude mapping (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupReconTelemetry()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`](../../Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs)
  - Host Session: [`src/Host/ReconTelemetrySaveStore.cs`](../../src/Host/ReconTelemetrySaveStore.cs)
  - Save Store: [`src/Host/ReconTelemetrySaveStore.cs`](../../src/Host/ReconTelemetrySaveStore.cs)

### 52. `vehicle_garage` — Plans 50-53 — expedition overland vehicle modifications and garage maintenance state (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupVehicleGarage()` | **Cadence:** `On-Demand`
- **UI Routes:** `vehicle_garage`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs`](../../Assets/Ashfall.Core/Expeditions/VehicleArmorGradeCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`](../../Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/VehicleGarageSaveStore.cs`](../../src/Host/VehicleGarageSaveStore.cs)
  - UI Panel: [`src/UI/VehicleGaragePanel.cs`](../../src/UI/VehicleGaragePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs`](../../Ashfall.Core.Tests/Expeditions/VehicleGarageSystemTests.cs)

### 53. `counter_intelligence` — Counter-intelligence, vetting, and defector management (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupCounterIntelligence()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/CounterIntelligenceSystem.cs`](../../Assets/Ashfall.Core/Factions/CounterIntelligenceSystem.cs)
  - Host Session: [`src/Host/CounterIntelligenceSaveStore.cs`](../../src/Host/CounterIntelligenceSaveStore.cs)
  - Save Store: [`src/Host/CounterIntelligenceSaveStore.cs`](../../src/Host/CounterIntelligenceSaveStore.cs)

### 54. `diplomatic_summits` — Wasteland summits, treaty lifecycle, guarantees, DMZ rules, violations (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupDiplomaticSummit()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs`](../../Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DiplomaticSummitSaveStore.cs`](../../src/Host/DiplomaticSummitSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiplomaticSummitTests.cs`](../../Ashfall.Core.Tests/DiplomaticSummitTests.cs)

### 55. `espionage` — Campaign intelligence networks, missions, and captured agents (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupPlans166To169()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/EspionageSystem.cs`](../../Assets/Ashfall.Core/Factions/EspionageSystem.cs)
  - Host Session: [`src/Host/EspionageHostSession.cs`](../../src/Host/EspionageHostSession.cs)
  - Save Store: [`src/Host/EspionageSaveStore.cs`](../../src/Host/EspionageSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan167EspionageTests.cs`](../../Ashfall.Core.Tests/Plan167EspionageTests.cs)

### 56. `faction_covert_ops` — ORPHAN-SEAL-W1 — rival-faction covert operations, suspicion ladder, and intelligence reports (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupFactionCovertOps()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/FactionCovertOpsCoordinator.cs`](../../Assets/Ashfall.Core/Factions/FactionCovertOpsCoordinator.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/Plan153FactionEspionageIntegrationTests.cs`](../../Ashfall.Core.Tests/Factions/Plan153FactionEspionageIntegrationTests.cs)

### 57. `faction_espionage` — Plans 50-53 — shelter faction espionage, sleeper assets, counter-intel, and sabotage state (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupShelterEspionage()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs`](../../Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterEspionageSaveStore.cs`](../../src/Host/ShelterEspionageSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs`](../../Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs)

### 58. `territory_control` — Plan 134 — dynamic faction territory & supply line control: contested locations, fortification, garrison, supply line status (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupTerritoryControl()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`](../../Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs)
  - Host Session: [`src/Host/TerritoryControlHostSession.cs`](../../src/Host/TerritoryControlHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TerritoryControlHostSession.cs`](../../src/Host/TerritoryControlHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/Plan134TerritoryControlHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Factions/Plan134TerritoryControlHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`](../../Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs)

### 59. `weight_of_choices` — Weight of choices faction branch progression and PoNR commitments (Factions & Diplomacy)
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

### 60. `aeroponics` — Plan B76 — aeroponic chambers, nutrient chemistry, disease, lighting, and harvest (Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupAeroponics()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 61. `agriculture` — Plans 162-165 — advanced crop strains, plot medium, pests, compost, and dietary diversity (Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupAgriculture()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Farming/AgricultureSystem.cs`](../../Assets/Ashfall.Core/Farming/AgricultureSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AgricultureSaveStore.cs`](../../src/Host/AgricultureSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AgricultureSystemTests.cs`](../../Ashfall.Core.Tests/AgricultureSystemTests.cs)

### 62. `aquaponics` — Plan B87 — closed-loop aquaponics ecology, biofilter health, and harvest yields (Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupAquaponics()` | **Cadence:** `Daily Ecology Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AquaponicsSaveStore.cs`](../../src/Host/AquaponicsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs`](../../Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/AquaponicsSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/AquaponicsSystemTests.cs)

### 63. `powder_metallurgy` — Plans 130-133 — abstract advanced-material production quality and reliability (Foundry)
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupPowderMetallurgy()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs`](../../Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 64. `hydraulic_extrusion` — Plan 140 — advanced hydraulic extrusion batches, tooling condition, quality grades (Foundry & Industry)
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupHydraulicExtrusion()` | **Cadence:** `On-Demand (Batch Phase Commands)`
- **UI Routes:** `hydraulic_extrusion`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/HydraulicExtrusionEngine.cs`](../../Assets/Ashfall.Core/Foundry/HydraulicExtrusionEngine.cs)
  - Host Session: [`src/Host/HydraulicExtrusionHostSession.cs`](../../src/Host/HydraulicExtrusionHostSession.cs)
  - Save Store: [`src/Host/HydraulicExtrusionSaveStore.cs`](../../src/Host/HydraulicExtrusionSaveStore.cs)
  - UI Panel: [`src/UI/HydraulicExtrusionPanel.cs`](../../src/UI/HydraulicExtrusionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Foundry/Plan140HydraulicExtrusionTests.cs`](../../Ashfall.Core.Tests/Foundry/Plan140HydraulicExtrusionTests.cs)

### 65. `shelter_governance` — Plan 159 — Shelter governance & political system: ideological blocs, policy consent, civil disputes, and shelter stability (Governance)
- **Owner Domain:** `governance`
- **Setup Method:** `Main.SetupShelterGovernance()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `survivor_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Governance/ShelterGovernanceCatalogLoader.cs`](../../Assets/Ashfall.Core/Governance/ShelterGovernanceCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Governance/ShelterGovernanceEngine.cs`](../../Assets/Ashfall.Core/Governance/ShelterGovernanceEngine.cs)
  - Host Session: [`src/Host/ShelterGovernanceHostSession.cs`](../../src/Host/ShelterGovernanceHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterGovernanceHostSession.cs`](../../src/Host/ShelterGovernanceHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Governance/Plan159ShelterGovernanceHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Governance/Plan159ShelterGovernanceHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Governance/Plan159_190GovernanceProvenanceIntegrationTests.cs`](../../Ashfall.Core.Tests/Governance/Plan159_190GovernanceProvenanceIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Governance/ShelterGovernanceEngineTests.cs`](../../Ashfall.Core.Tests/Governance/ShelterGovernanceEngineTests.cs)

### 66. `shelter_identity` — Plan 166 — Shelter identity, naming, origin projection, emblems, mottos, infamy, and community legacy tags (Holdfast)
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupShelterIdentity()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `shelter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterOriginCatalogLoader.cs`](../../Assets/Ashfall.Core/Shelter/ShelterOriginCatalogLoader.cs)
  - Host Session: [`src/Host/ShelterIdentityHostSession.cs`](../../src/Host/ShelterIdentityHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterIdentityHostSession.cs`](../../src/Host/ShelterIdentityHostSession.cs)
  - UI Panel: [`src/UI/ShelterPanel.cs`](../../src/UI/ShelterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterOriginCatalogLoaderTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterOriginCatalogLoaderTests.cs)

### 67. `wildlife_ecosystem` — Plans 162-165 — ecology pressures, extinction flags, apex activity, taming, bestiary knowledge (Hunting)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupWildlifeEcosystem()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `bestiary`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`](../../Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs)
  - Host Session: [`src/Host/WildlifeEcosystemHostSession.cs`](../../src/Host/WildlifeEcosystemHostSession.cs)
  - Save Store: [`src/Host/WildlifeEcosystemSaveStore.cs`](../../src/Host/WildlifeEcosystemSaveStore.cs)
  - UI Panel: [`src/UI/BestiaryPanel.cs`](../../src/UI/BestiaryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`](../../Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs)

### 68. `shelter_barter` — Plan 54/147 — shelter barter caravans, pinned stock, and the contraband broker counter (Illicit Economy / Barter)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupShelterBarter()` | **Cadence:** `On-Demand (Barter)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs`](../../Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterBarterSaveStore.cs`](../../src/Host/ShelterBarterSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/ShelterBarterSystemPlan54Tests.cs`](../../Ashfall.Core.Tests/Economy/ShelterBarterSystemPlan54Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/ContrabandBarterRouteTests.cs`](../../Ashfall.Core.Tests/Narrative/ContrabandBarterRouteTests.cs)

### 69. `grain_milling_archive` — Plan 157 — Grain milling, storage & food-processing knowledge archive: discovered-record ledger (IDs only) (Industrial Food-Processing Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupGrainMillingArchive()` | **Cadence:** `Event-Driven (Location Discovery & Shelter Room Inspection)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/GrainMillingDiscoverySystem.cs`](../../Assets/Ashfall.Core/Narrative/GrainMillingDiscoverySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/GrainMillingArchiveSaveStore.cs`](../../src/Host/GrainMillingArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GrainMillingCatalogTests.cs`](../../Ashfall.Core.Tests/GrainMillingCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs`](../../Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs)

### 70. `wasteland_rumors` — Plan 203 / 131 — wasteland rumors, information hubs, propagation, and intercepts (Information & Rumors (Plan 203))
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupRumorNetwork()` | **Cadence:** `Daily (Decay & Propagation)`
- **UI Routes:** `rumors`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/InformationFlow/RumorSystem.cs`](../../Assets/Ashfall.Core/InformationFlow/RumorSystem.cs)
  - Host Session: [`src/Host/RumorNetworkHostSession.cs`](../../src/Host/RumorNetworkHostSession.cs)
  - Save Store: [`src/Host/RumorNetworkSaveStore.cs`](../../src/Host/RumorNetworkSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/RumorBoardPanel.cs`](../../src/UI/RumorBoardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/InformationFlow/Plan203RumorNetworkIntegrationTests.cs`](../../Ashfall.Core.Tests/InformationFlow/Plan203RumorNetworkIntegrationTests.cs)

### 71. `cryogenic_air_separation` — Abstract gas production and plant condition (Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupCryogenicAirSeparation()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/CryogenicAirSeparationSystem.cs`](../../Assets/Ashfall.Core/CryogenicAirSeparationSystem.cs)
  - Host Session: [`src/Host/CryogenicAirSeparationHostSession.cs`](../../src/Host/CryogenicAirSeparationHostSession.cs)
  - Save Store: [`src/Host/CryogenicAirSeparationHostSession.cs`](../../src/Host/CryogenicAirSeparationHostSession.cs)

### 72. `fluid_logistics` — Shelter fluid topology, pressure, leaks, and distributed quality (Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupPlans166To169()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs`](../../Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs)
  - Host Session: [`src/Host/FluidLogisticsHostSession.cs`](../../src/Host/FluidLogisticsHostSession.cs)
  - Save Store: [`src/Host/FluidLogisticsSaveStore.cs`](../../src/Host/FluidLogisticsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan168FluidLogisticsTests.cs`](../../Ashfall.Core.Tests/Plan168FluidLogisticsTests.cs)

### 73. `pneumatic_dispatch` — Plan B77 — pneumatic stations, capsule routing, seals, jams, and blackout-safe dispatch (Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupPneumaticDispatch()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs`](../../Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 74. `black_projects_archive` — Plan 152 — Black Projects intelligence archive: discovered-record ledger (IDs only) (Intelligence Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupBlackProjectsArchive()` | **Cadence:** `On-Demand`
- **UI Routes:** `black_projects_archive`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/BlackProjectsArchiveSystem.cs`](../../Assets/Ashfall.Core/Narrative/BlackProjectsArchiveSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BlackProjectsArchiveSaveStore.cs`](../../src/Host/BlackProjectsArchiveSaveStore.cs)
  - UI Panel: [`src/UI/BlackProjectsArchivePanel.cs`](../../src/UI/BlackProjectsArchivePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BlackProjectsCatalogTests.cs`](../../Ashfall.Core.Tests/BlackProjectsCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/BlackProjectsArchiveTests.cs`](../../Ashfall.Core.Tests/Narrative/BlackProjectsArchiveTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/UI/BlackProjectsArchivePanelRouteTests.cs`](../../Ashfall.Core.Tests/UI/BlackProjectsArchivePanelRouteTests.cs)

### 75. `collectible_discovery` — One-time collectible discovery ledger (Inventory & Lore)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupCollectibles()` | **Cadence:** `On-Demand (One-Time Discovery Ledger)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/CollectibleDiscoveryState.cs`](../../Assets/Ashfall.Core/CollectibleDiscoveryState.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CollectibleDiscoverySaveStore.cs`](../../src/Host/CollectibleDiscoverySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`](../../Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs)

### 76. `unique_claims` — Global unique-item claim ledger (Inventory & Lore)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupCollectibles()` | **Cadence:** `On-Demand (Global Unique Claim Ledger)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/UniqueItemClaimRegistry.cs`](../../Assets/Ashfall.Core/UniqueItemClaimRegistry.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/UniqueClaimSaveStore.cs`](../../src/Host/UniqueClaimSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`](../../Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs)

### 77. `cultural_archives` — Deep-vault cultural archives: restoration, transcription, microfiche preservation, discs, salons, chronicles (Knowledge)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupCulturalArchive()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`](../../Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CulturalArchiveSaveStore.cs`](../../src/Host/CulturalArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`](../../Ashfall.Core.Tests/CulturalArchiveVaultTests.cs)

### 78. `field_guide` — Plan 20A/28 — field-guide unlocked-entry ledger (reading-the-land knowledge) (Knowledge)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupFieldGuide()` | **Cadence:** `On-Demand (Study & Discovery)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/FieldGuideCatalog.cs`](../../Assets/Ashfall.Core/World/FieldGuideCatalog.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FieldGuideSaveStore.cs`](../../src/Host/FieldGuideSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/FieldGuidePersistenceTests.cs`](../../Ashfall.Core.Tests/FieldGuidePersistenceTests.cs)

### 79. `prewar_archives` — Pre-war archive discovery and decryption (Knowledge)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupPlans62To65()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs`](../../Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PrewarArchiveSaveStore.cs`](../../src/Host/PrewarArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Research/PrewarArchiveDecryptionTests.cs`](../../Ashfall.Core.Tests/Research/PrewarArchiveDecryptionTests.cs)

### 80. `research` — Research knowledge progress: unlocked, active, and completed nodes (Plan 34) (Knowledge)
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

### 81. `survivor_education` — ORPHAN-SEAL-W1 — learner records, subjects, teachers, and graduation (Knowledge)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorEducation()` | **Cadence:** `Session-Driven`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Education/SurvivorEducationSystem.cs`](../../Assets/Ashfall.Core/Education/SurvivorEducationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Education/Plan154EducationIntegrationTests.cs`](../../Ashfall.Core.Tests/Education/Plan154EducationIntegrationTests.cs)

### 82. `campaign_legacy` — Plan 140 — Generational legacy, heirlooms, campaign inheritance, and New Game+ multi-generational continuity (Legacy)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCampaignLegacy()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs`](../../Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs)
  - Host Session: [`src/Host/CampaignLegacyHostSession.cs`](../../src/Host/CampaignLegacyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CampaignLegacyHostSession.cs`](../../src/Host/CampaignLegacyHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Legacy/Plan140CampaignLegacyHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Legacy/Plan140CampaignLegacyHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs`](../../Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs)

### 83. `leatherwork_archive` — Plan 159 — Tanning/leather material provenance & workshop knowledge archive: discovered-record ledger (IDs only) (Material Provenance Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupLeatherworkArchive()` | **Cadence:** `Event-Driven (Location Discovery & Item Inspection)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/LeatherworkArchiveSystem.cs`](../../Assets/Ashfall.Core/Narrative/LeatherworkArchiveSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/LeatherworkArchiveSaveStore.cs`](../../src/Host/LeatherworkArchiveSaveStore.cs)
  - UI Panel: [`src/UI/InventoryDetailPanel.cs`](../../src/UI/InventoryDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/LeatherworkArchiveTests.cs`](../../Ashfall.Core.Tests/Narrative/LeatherworkArchiveTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TanningLeatherCatalogTests.cs`](../../Ashfall.Core.Tests/TanningLeatherCatalogTests.cs)

### 84. `lyophilization` — Plans 130-133 — preserved-biologic batches and viability ledger (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupLyophilization()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/LyophilizationSystem.cs`](../../Assets/Ashfall.Core/Medical/LyophilizationSystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 85. `medical_pipeline` — Diagnosis knowledge, treatment reservations, scheduled procedures (Task #133) (Medical)
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

### 86. `microfluidic_diagnostic` — Plans 146-149 — microfluidic diagnostic cartridge manufacturing and run records (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMicrofluidicDiagnostic()` | **Cadence:** `On-Demand`
- **UI Routes:** `microfluidic_diagnostic`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs`](../../Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs)
  - Host Session: [`src/Host/MicrofluidicDiagnosticHostSession.cs`](../../src/Host/MicrofluidicDiagnosticHostSession.cs)
  - Save Store: [`src/Host/MicrofluidicDiagnosticSaveStore.cs`](../../src/Host/MicrofluidicDiagnosticSaveStore.cs)
  - UI Panel: [`src/UI/MicrofluidicDiagnosticPanel.cs`](../../src/UI/MicrofluidicDiagnosticPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MicrofluidicDiagnosticEngineTests.cs`](../../Ashfall.Core.Tests/Medical/MicrofluidicDiagnosticEngineTests.cs)

### 87. `pathogen_strains` — Flagship XI Plan 155 — fictional strain layer: cure projects and unlocked cures (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupPathogenStrains()` | **Cadence:** `Daily Strain Progression Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs`](../../Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PathogenStrainSaveStore.cs`](../../src/Host/PathogenStrainSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiseaseSystemTests.cs`](../../Ashfall.Core.Tests/DiseaseSystemTests.cs)

### 88. `psychological_sanatorium` — Trauma sanatorium: admissions, therapies, sedatives, relapse, discharge (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupSanatorium()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Sanatorium/PsychologicalSanatoriumSystem.cs`](../../Assets/Ashfall.Core/Sanatorium/PsychologicalSanatoriumSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PsychologicalSanatoriumSaveStore.cs`](../../src/Host/PsychologicalSanatoriumSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PsychologicalSanatoriumTests.cs`](../../Ashfall.Core.Tests/PsychologicalSanatoriumTests.cs)

### 89. `surgical_ward` — Advanced surgical ward operations and sterile field (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupSurgicalWard()` | **Cadence:** `Daily Sterile Field Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurgicalWardSaveStore.cs`](../../src/Host/SurgicalWardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)

### 90. `propaganda_campaigns` — Plan 168 — propaganda messages, multi-day campaigns, detection, and morale warfare (Morale & Influence (Plan 168))
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPropaganda()` | **Cadence:** `Daily (Campaign Decay & Distribution)`
- **UI Routes:** `propaganda`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Propaganda/PropagandaSystem.cs`](../../Assets/Ashfall.Core/Propaganda/PropagandaSystem.cs)
  - Host Session: [`src/Host/PropagandaHostSession.cs`](../../src/Host/PropagandaHostSession.cs)
  - Save Store: [`src/Host/PropagandaSaveStore.cs`](../../src/Host/PropagandaSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PropagandaPanel.cs`](../../src/UI/PropagandaPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Propaganda/Plan168PropagandaIntegrationTests.cs`](../../Ashfall.Core.Tests/Propaganda/Plan168PropagandaIntegrationTests.cs)

### 91. `confession_secret` — ORPHAN-SEAL-W1 — discovered secrets, resolution choices, and leverage records (Narrative)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupConfessionSecrets()` | **Cadence:** `Discovery-Driven`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs`](../../Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`](../../Ashfall.Core.Tests/ConfessionSecretSystemTests.cs)

### 92. `echoes` — Field echoes: surfaced and resolved one-time narrative state (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupEchoes()` | **Cadence:** `Narrative Echo Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs`](../../Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Narrative/EchoSystem.cs`](../../Assets/Ashfall.Core/Narrative/EchoSystem.cs)
  - Host Session: [`src/Host/EchoHostSession.cs`](../../src/Host/EchoHostSession.cs)
  - Host Session: [`src/Host/EchoSaveStore.cs`](../../src/Host/EchoSaveStore.cs)
  - Save Store: [`src/Host/EchoSaveStore.cs`](../../src/Host/EchoSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/EchoCatalogTests.cs`](../../Ashfall.Core.Tests/Narrative/EchoCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/EchoSystemTests.cs`](../../Ashfall.Core.Tests/Narrative/EchoSystemTests.cs)

### 93. `npc_memory` — Plan 147 — Per-NPC memory and relationship depth: trust, grudge, favors owed, forgiveness, and trade multipliers (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupNpcMemory()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs`](../../Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs)
  - Host Session: [`src/Host/NpcMemoryHostSession.cs`](../../src/Host/NpcMemoryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/NpcMemoryHostSession.cs`](../../src/Host/NpcMemoryHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/NpcMemorySystemTests.cs`](../../Ashfall.Core.Tests/Narrative/NpcMemorySystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/Plan147NpcMemoryHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Narrative/Plan147NpcMemoryHostIntegrationTests.cs)

### 94. `seasonal_celebration` — ORPHAN-SEAL-W1 — holidays, anniversaries, scales, and celebration history (Narrative)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupSeasonalCelebration()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Events/SeasonalCelebrationSystem.cs`](../../Assets/Ashfall.Core/Events/SeasonalCelebrationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs`](../../Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs)

### 95. `shelter_festival` — ORPHAN-SEAL-W1 — player-scheduled festivals, commodity costs, and completion state (Narrative)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupShelterFestival()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Culture/ShelterFestivalEngine.cs`](../../Assets/Ashfall.Core/Culture/ShelterFestivalEngine.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs`](../../Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs)

### 96. `oral_lore` — Plan 155 — oral lore first-heard ledger (lore IDs only) (Narrative & Cultural Tradition)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupOralLore()` | **Cadence:** `Event-Driven (Performance)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/OralLorePerformanceSystem.cs`](../../Assets/Ashfall.Core/Narrative/OralLorePerformanceSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OralLoreSaveStore.cs`](../../src/Host/OralLoreSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/OralLorePlan155Tests.cs`](../../Ashfall.Core.Tests/Narrative/OralLorePlan155Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/OralLoreCatalogTests.cs`](../../Ashfall.Core.Tests/OralLoreCatalogTests.cs)

### 97. `moral_choice` — Moral choice ledger and community trust (Narrative & Decisions)
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

### 98. `contraband_stash` — Plan 147 — bunker contraband stash claim ledger (once-only discovery) (Narrative & Illicit Economy)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupContrabandStash()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs`](../../Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ContrabandSaveStore.cs`](../../src/Host/ContrabandSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs`](../../Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs)

### 99. `cooking` — Plan 136 — wildlife trapping food pipeline & cooking system: recipes, active operations, meals prepared, and food decontamination (Nutrition)
- **Owner Domain:** `cooking`
- **Setup Method:** `Main.SetupCooking()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Cooking/CookingRecipeCatalogLoader.cs`](../../Assets/Ashfall.Core/Cooking/CookingRecipeCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Cooking/CookingSystem.cs`](../../Assets/Ashfall.Core/Cooking/CookingSystem.cs)
  - Host Session: [`src/Host/CookingHostSession.cs`](../../src/Host/CookingHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CookingHostSession.cs`](../../src/Host/CookingHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Cooking/Plan136CookingHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Cooking/Plan136CookingHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs`](../../Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs)

### 100. `grain_processing` — Grain milling, silo safety, and pest pressure (Nutrition)
- **Owner Domain:** `nutrition`
- **Setup Method:** `Main.SetupGrainProcessing()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/GrainProcessingSystem.cs`](../../Assets/Ashfall.Core/GrainProcessingSystem.cs)
  - Host Session: [`src/Host/GrainProcessingHostSession.cs`](../../src/Host/GrainProcessingHostSession.cs)
  - Save Store: [`src/Host/GrainProcessingHostSession.cs`](../../src/Host/GrainProcessingHostSession.cs)

### 101. `companion_animals` — Plan 174 — persistent companion animals: care, bond, training, roles, sickness, assignments (Plan 174 Companion Animals)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupCompanionAnimals()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`](../../Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CompanionSaveStore.cs`](../../src/Host/CompanionSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs`](../../Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs)

### 102. `zealotry` — Plan 175 — fictional ideological pressure: belief state, fervor, dissent, shrines, escalation (Plan 175 Ideological Pressure)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupZealotry()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/ZealotrySystem.cs`](../../Assets/Ashfall.Core/Survivors/ZealotrySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ZealotrySaveStore.cs`](../../src/Host/ZealotrySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/ZealotrySystemTests.cs`](../../Ashfall.Core.Tests/Survivors/ZealotrySystemTests.cs)

### 103. `anomaly_hazard` — Plan 176 — authored anomaly and storm-front hazard zones, movement, warnings, loot-site resolution (Plan 176 Anomaly Hazard Layer)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupAnomalyHazard()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/AnomalyHazardSystem.cs`](../../Assets/Ashfall.Core/World/AnomalyHazardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AnomalyHazardSaveStore.cs`](../../src/Host/AnomalyHazardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan176AnomalyHazardTests.cs`](../../Ashfall.Core.Tests/World/Plan176AnomalyHazardTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`](../../Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs)

### 104. `bionics` — Plan 177 — bionic implant instances: condition, integration, power, maintenance, complications (Plan 177 Bionics & Prosthetics)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupBionics()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/BionicsSystem.cs`](../../Assets/Ashfall.Core/Medical/BionicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BionicsSaveStore.cs`](../../src/Host/BionicsSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/Plan177BionicsTests.cs`](../../Ashfall.Core.Tests/Medical/Plan177BionicsTests.cs)

### 105. `spiritual_meaning` — Plan 30 — spiritual-meaning coordinator: mourning arcs, ritual cooldowns, memorial rites (Plan 30 Spiritual Meaning)
- **Owner Domain:** `spiritual`
- **Setup Method:** `Main.SetupSpiritual()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Spiritual/SpiritualMeaningCoordinator.cs`](../../Assets/Ashfall.Core/Spiritual/SpiritualMeaningCoordinator.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SpiritualSaveStore.cs`](../../src/Host/SpiritualSaveStore.cs)
  - UI Panel: [`src/UI/IronCenotaphMemorialPanel.cs`](../../src/UI/IronCenotaphMemorialPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Spiritual/Plan30SpiritualWorldTests.cs`](../../Ashfall.Core.Tests/Spiritual/Plan30SpiritualWorldTests.cs)

### 106. `amputation` — Infection progression, amputations, prosthetics and bionics (Plans 178-201 Expansion Block)
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

### 107. `archaeology` — Archaeology excavation ruins, archive decryption, and lore unlocks (Plans 178-201 Expansion Block)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupArchaeology()` | **Cadence:** `On-Demand (Excavation & Decryption)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`](../../Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ArchaeologySaveStore.cs`](../../src/Host/ArchaeologySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`](../../Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs)

### 108. `aviation` — Aviation airframes, flight plans, aerial mapping, and crash rescue (Plans 178-201 Expansion Block)
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

### 109. `ceremony` — Communal ceremonies, festivals, truces, and morale (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupCeremony()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/CeremonySystem.cs`](../../Assets/Ashfall.Core/Narrative/CeremonySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CeremonySaveStore.cs`](../../src/Host/CeremonySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/CeremonySystemTests.cs`](../../Ashfall.Core.Tests/Narrative/CeremonySystemTests.cs)

### 110. `chem_warfare` — CBRN hazard warfare and toxic contamination (Plans 178-201 Expansion Block)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupChemWarfare()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/ChemWarfareSystem.cs`](../../Assets/Ashfall.Core/Combat/ChemWarfareSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ChemWarfareSaveStore.cs`](../../src/Host/ChemWarfareSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs`](../../Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs)

### 111. `child_development` — Child development phases, education, trauma, and adulthood (Plans 178-201 Expansion Block)
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

### 112. `comms_array` — Long-range communications array and satellite telemetry (Plans 178-201 Expansion Block)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupCommsArray()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/CommsArraySystem.cs`](../../Assets/Ashfall.Core/World/CommsArraySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CommsArraySaveStore.cs`](../../src/Host/CommsArraySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/CommsArraySystemTests.cs`](../../Ashfall.Core.Tests/World/CommsArraySystemTests.cs)

### 113. `desperation` — Starvation crisis desperation acts and cannibalism history (Plans 178-201 Expansion Block)
- **Owner Domain:** `survival`
- **Setup Method:** `Main.SetupDesperation()` | **Cadence:** `On-Demand (Crisis Command)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/DesperationSystem.cs`](../../Assets/Ashfall.Core/Survivors/DesperationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DesperationSaveStore.cs`](../../src/Host/DesperationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/DesperationSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/DesperationSystemTests.cs)

### 114. `expedition_stealth` — Expedition stealth, detection risk, camouflage, and night ops (Plans 178-201 Expansion Block)
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

### 115. `fallout` — Radioactive fallout clouds, dispersal, and shelter sealing (Plans 178-201 Expansion Block)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupFallout()` | **Cadence:** `Hourly Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/FalloutSystem.cs`](../../Assets/Ashfall.Core/World/FalloutSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FalloutSaveStore.cs`](../../src/Host/FalloutSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/FalloutSystemTests.cs`](../../Ashfall.Core.Tests/World/FalloutSystemTests.cs)

### 116. `forced_labor` — Captive forced labor assignments, cruelty index, and rebellion risks (Plans 178-201 Expansion Block)
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

### 117. `fungi_cultivation` — Subterranean fungi beds, substrate, spores, and blooms (Plans 178-201 Expansion Block)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupFungi()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs`](../../Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FungiSaveStore.cs`](../../src/Host/FungiSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Farming/FungiCultivationSystemTests.cs`](../../Ashfall.Core.Tests/Farming/FungiCultivationSystemTests.cs)

### 118. `mercenary_bounties` — Mercenary bounty contracts, target intel, and rival tracking (Plans 178-201 Expansion Block)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupMercenary()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/MercenarySystem.cs`](../../Assets/Ashfall.Core/Economy/MercenarySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MercenarySaveStore.cs`](../../src/Host/MercenarySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/MercenarySystemTests.cs`](../../Ashfall.Core.Tests/Economy/MercenarySystemTests.cs)

### 119. `mutation_tree` — Radiation exposure, genetic instability, and mutation trees (Plans 178-201 Expansion Block)
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

### 120. `narcotics` — Chemical medicines, toxicity, tolerance, addiction, and rehab beds (Plans 178-201 Expansion Block)
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

### 121. `prisoner_management` — Captive detention, upkeep, interrogation, escape, and recruitment (Plans 178-201 Expansion Block)
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

### 122. `railway` — Rail network, track repair, and armored train operations (Plans 178-201 Expansion Block)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupRailway()` | **Cadence:** `On-Demand (Convoy Operations)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/RailwaySystem.cs`](../../Assets/Ashfall.Core/Expeditions/RailwaySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RailwaySaveStore.cs`](../../src/Host/RailwaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/RailwaySystemTests.cs`](../../Ashfall.Core.Tests/Expeditions/RailwaySystemTests.cs)

### 123. `recreation` — Survivor hobbies, downtime, and recreation (Plans 178-201 Expansion Block)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupRecreation()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs`](../../Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RecreationSaveStore.cs`](../../src/Host/RecreationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Recreation/SurvivorDowntimeSystemTests.cs`](../../Ashfall.Core.Tests/Recreation/SurvivorDowntimeSystemTests.cs)

### 124. `robotics` — Pre-war robotics, directives, and automation (Plans 178-201 Expansion Block)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupRobotics()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/RoboticsSystem.cs`](../../Assets/Ashfall.Core/Crafting/RoboticsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RoboticsSaveStore.cs`](../../src/Host/RoboticsSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Crafting/RoboticsSystemTests.cs`](../../Ashfall.Core.Tests/Crafting/RoboticsSystemTests.cs)

### 125. `settlement_politics` — Settlement elections, political policies, approval rating, and coups (Plans 178-201 Expansion Block)
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

### 126. `wasteland_justice` — Crime incidents, trials, punishments, banishments, and grudges (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupJustice()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/JusticeSystem.cs`](../../Assets/Ashfall.Core/Narrative/JusticeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/JusticeSaveStore.cs`](../../src/Host/JusticeSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs`](../../Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs)

### 127. `plastic_pyrolysis` — Retort bay — waste plastic to synthetic fuel fractions (Plan 202) (Plans 202-205 Flagship (Plan 202))
- **Owner Domain:** `industry`
- **Setup Method:** `Main.SetupPlasticPyrolysis()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `plastic_pyrolysis`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs`](../../Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PlasticPyrolysisSaveStore.cs`](../../src/Host/PlasticPyrolysisSaveStore.cs)
  - UI Panel: [`src/UI/PlasticPyrolysisPanel.cs`](../../src/UI/PlasticPyrolysisPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PlasticPyrolysisEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/PlasticPyrolysisEngineTests.cs)

### 128. `cargo_airdrop` — Airdrop events, crate contents, beacons, and interception races (Plan 205) (Plans 202-205 Flagship (Plan 205))
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupCargoAirdrop()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `cargo_airdrop`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/CargoAirdropSystem.cs`](../../Assets/Ashfall.Core/World/CargoAirdropSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CargoAirdropSaveStore.cs`](../../src/Host/CargoAirdropSaveStore.cs)
  - UI Panel: [`src/UI/CargoAirdropPanel.cs`](../../src/UI/CargoAirdropPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/CargoAirdropEngineTests.cs`](../../Ashfall.Core.Tests/World/CargoAirdropEngineTests.cs)

### 129. `geothermal_orc` — Plan B74 — geothermal organic Rankine loop, heat reserve, fouling, and leakage (Power)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupGeothermalOrc()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs`](../../Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 130. `kinetic_storage` — Plans 78-81 — flywheel rotor, vacuum, bearing, and containment state (Power)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupKineticStorage()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs`](../../Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs)
  - Host Session: [`src/Host/KineticStorageSaveStore.cs`](../../src/Host/KineticStorageSaveStore.cs)
  - Save Store: [`src/Host/KineticStorageSaveStore.cs`](../../src/Host/KineticStorageSaveStore.cs)

### 131. `solar_concentrator` — Plans 110-113 — parabolic solar concentrator, mirror condition, tracking mode, and thermal output (Power)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupSolarConcentrator()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs`](../../Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs)
  - Host Session: [`src/Host/SolarConcentratorHostSession.cs`](../../src/Host/SolarConcentratorHostSession.cs)
  - Save Store: [`src/Host/SolarConcentratorSaveStore.cs`](../../src/Host/SolarConcentratorSaveStore.cs)

### 132. `hobby` — ORPHAN-SEAL-W1 — survivor hobby progress, mastery, and co-participation (Psychology)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupHobby()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/HobbySystem.cs`](../../Assets/Ashfall.Core/Survivors/HobbySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan161HobbyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan161HobbyIntegrationTests.cs)

### 133. `psychological_arcs` — Plans 162-165 — breakdown arcs, exposure, treatment progress, private stashes, catharsis (Psychology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupPsychologyArcs()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs`](../../Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs)
  - Host Session: [`src/Host/PsychologyArcHostSession.cs`](../../src/Host/PsychologyArcHostSession.cs)
  - Save Store: [`src/Host/PsychologyArcSaveStore.cs`](../../src/Host/PsychologyArcSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PsychologicalArcSystemTests.cs`](../../Ashfall.Core.Tests/PsychologicalArcSystemTests.cs)

### 134. `survivor_autonomy` — ORPHAN-SEAL-W1 — daily survivor autonomy evaluations, cooldowns, and personal goals (Psychology)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorAutonomy()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorAutonomySystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorAutonomySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan144SurvivorAutonomyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan144SurvivorAutonomyIntegrationTests.cs)

### 135. `survivor_mental_health` — Plans 50-53 — survivor psychological trauma, stress levels, catharsis, and mental health crises (Psychology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupSurvivorMentalHealth()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs`](../../Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorMentalHealthSaveStore.cs`](../../src/Host/SurvivorMentalHealthSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Needs/SurvivorMentalHealthTests.cs`](../../Ashfall.Core.Tests/Needs/SurvivorMentalHealthTests.cs)

### 136. `procedural_narrative` — Procedural narrative metadata and the shared quest runtime (Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupPlans166To169()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/ProceduralNarrativeSystem.cs`](../../Assets/Ashfall.Core/Narrative/ProceduralNarrativeSystem.cs)
  - Host Session: [`src/Host/ProceduralNarrativeHostSession.cs`](../../src/Host/ProceduralNarrativeHostSession.cs)
  - Save Store: [`src/Host/ProceduralNarrativeSaveStore.cs`](../../src/Host/ProceduralNarrativeSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan169ProceduralNarrativeTests.cs`](../../Ashfall.Core.Tests/Plan169ProceduralNarrativeTests.cs)

### 137. `low_background_metrology` — Plan 138 — low-background shield install, detector calibration, smelting batches, bounded assay history (Radiation & Metrology)
- **Owner Domain:** `radiation`
- **Setup Method:** `Main.SetupLowBackgroundMetrology()` | **Cadence:** `On-Demand (Assay & Smelting Commands)`
- **UI Routes:** `low_background_metrology`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs`](../../Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs)
  - Host Session: [`src/Host/LowBackgroundMetrologyHostSession.cs`](../../src/Host/LowBackgroundMetrologyHostSession.cs)
  - Save Store: [`src/Host/LowBackgroundMetrologySaveStore.cs`](../../src/Host/LowBackgroundMetrologySaveStore.cs)
  - UI Panel: [`src/UI/LowBackgroundLeadPanel.cs`](../../src/UI/LowBackgroundLeadPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radiation/Plan138LowBackgroundLeadEngineTests.cs`](../../Ashfall.Core.Tests/Radiation/Plan138LowBackgroundLeadEngineTests.cs)

### 138. `communications` — ORPHAN-SEAL-W1 — antenna layer, intercepted messages, and outgoing broadcasts (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupCommunications()` | **Cadence:** `Daily Wear Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Communications/CommunicationsSystem.cs`](../../Assets/Ashfall.Core/Communications/CommunicationsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communications/Plan157CommunicationsIntegrationTests.cs`](../../Ashfall.Core.Tests/Communications/Plan157CommunicationsIntegrationTests.cs)

### 139. `heliograph` — Optical heliograph stations and message delivery (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupHeliograph()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HeliographSystem.cs`](../../Assets/Ashfall.Core/HeliographSystem.cs)
  - Host Session: [`src/Host/HeliographHostSession.cs`](../../src/Host/HeliographHostSession.cs)
  - Save Store: [`src/Host/HeliographHostSession.cs`](../../src/Host/HeliographHostSession.cs)

### 140. `nvis_communications` — Plans 130-133 — regional NVIS status communications and recall queue (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupNvisCommunications()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs`](../../Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 141. `psyops` — Flagship XI Plan 157 — broadcast campaigns, jamming, counter-propaganda, ideological pressure (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupPsyOps()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/PsyOpsSystem.cs`](../../Assets/Ashfall.Core/Radio/PsyOpsSystem.cs)
  - Host Session: [`src/Host/PsyOpsHostSession.cs`](../../src/Host/PsyOpsHostSession.cs)
  - Save Store: [`src/Host/PsyOpsSaveStore.cs`](../../src/Host/PsyOpsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flagship11/PsyOpsSystemTests.cs`](../../Ashfall.Core.Tests/Flagship11/PsyOpsSystemTests.cs)

### 142. `radio_program_production` — Plan 173 — player radio program prep/delivery jobs and follow-ups (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadioProgramProduction()` | **Cadence:** `Daily Program Tick`
- **UI Routes:** `radio`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs`](../../Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs)
  - Host Session: [`src/Host/RadioProgramProductionHostSession.cs`](../../src/Host/RadioProgramProductionHostSession.cs)
  - Save Store: [`src/Host/RadioProgramProductionSaveStore.cs`](../../src/Host/RadioProgramProductionSaveStore.cs)
  - UI Panel: [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/Plan173RadioProgramProductionTests.cs`](../../Ashfall.Core.Tests/Radio/Plan173RadioProgramProductionTests.cs)

### 143. `retention` — Plan 55 — retention policy audit report: bounded collection bounds, pruned totals, and preserved obligations (Records)
- **Owner Domain:** `records`
- **Setup Method:** `Main.SetupRetention()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Records/RetentionPolicy.cs`](../../Assets/Ashfall.Core/Records/RetentionPolicy.cs)
  - Core System: [`Assets/Ashfall.Core/Records/RetentionPolicyCatalogLoader.cs`](../../Assets/Ashfall.Core/Records/RetentionPolicyCatalogLoader.cs)
  - Host Session: [`src/Host/RetentionHostSession.cs`](../../src/Host/RetentionHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RetentionHostSession.cs`](../../src/Host/RetentionHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan55RetentionHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan55RetentionHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Records/Plan55RetentionPolicyIntegrationTests.cs`](../../Ashfall.Core.Tests/Records/Plan55RetentionPolicyIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Records/RetentionPolicyCatalogLoaderTests.cs`](../../Ashfall.Core.Tests/Records/RetentionPolicyCatalogLoaderTests.cs)

### 144. `research_unlock` — Plan 141 — Research downstream unlocks bridge: items, recipes, shelter, expedition, combat, and medical capabilities (Research)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupResearchUnlockBridge()` | **Cadence:** `On-Demand (Research Node Completion)`
- **UI Routes:** `research`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`](../../Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs)
  - Host Session: [`src/Host/ResearchUnlockHostSession.cs`](../../src/Host/ResearchUnlockHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ResearchUnlockHostSession.cs`](../../src/Host/ResearchUnlockHostSession.cs)
  - UI Panel: [`src/UI/ResearchPanel.cs`](../../src/UI/ResearchPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`](../../Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Research/Plan141ResearchUnlockHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Research/Plan141ResearchUnlockHostIntegrationTests.cs)

### 145. `playable_metrics` — Plan 46 — local session telemetry: aggregate readiness report and first-hour funnel completion (Save)
- **Owner Domain:** `save`
- **Setup Method:** `Main.SetupPlayMetrics()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs`](../../Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs)
  - Core System: [`Assets/Ashfall.Core/Telemetry/PlayableMetricsAggregationEngine.cs`](../../Assets/Ashfall.Core/Telemetry/PlayableMetricsAggregationEngine.cs)
  - Host Session: [`src/Host/PlayMetricsHostSession.cs`](../../src/Host/PlayMetricsHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PlayMetricsHostSession.cs`](../../src/Host/PlayMetricsHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan46PlayMetricsHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan46PlayMetricsHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Telemetry/PlaySessionRecorderTests.cs`](../../Ashfall.Core.Tests/Telemetry/PlaySessionRecorderTests.cs)

### 146. `session_durability` — Plan 39 — observed slot summaries, day-advance soak samples, and corruption/recovery audit (Save)
- **Owner Domain:** `save`
- **Setup Method:** `Main.SetupSessionDurability()` | **Cadence:** `Session-Driven`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Save/SessionDurabilityManager.cs`](../../Assets/Ashfall.Core/Save/SessionDurabilityManager.cs)
  - Host Session: [`src/Host/SaveLoadHostSession.cs`](../../src/Host/SaveLoadHostSession.cs)
  - Host Session: [`src/Host/SessionDurabilityHostSession.cs`](../../src/Host/SessionDurabilityHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SessionDurabilityHostSession.cs`](../../src/Host/SessionDurabilityHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Save/Plan39SessionDurabilityHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Save/Plan39SessionDurabilityHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Save/SessionDurabilityManagerTests.cs`](../../Ashfall.Core.Tests/Save/SessionDurabilityManagerTests.cs)

### 147. `seven_day_slice` — Plan 54 — seven-day slice playtest scorecard: frozen scenario hash, beat verification evidence, retention counts (Save)
- **Owner Domain:** `save`
- **Setup Method:** `Main.SetupSevenDaySlice()` | **Cadence:** `Playtest-Driven`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/SliceScenario.cs`](../../Assets/Ashfall.Core/Campaign/SliceScenario.cs)
  - Core System: [`Assets/Ashfall.Core/Campaign/SliceScenarioCatalogLoader.cs`](../../Assets/Ashfall.Core/Campaign/SliceScenarioCatalogLoader.cs)
  - Host Session: [`src/Host/SliceScenarioHostSession.cs`](../../src/Host/SliceScenarioHostSession.cs)
  - Save Store: [`src/Host/SliceScenarioHostSession.cs`](../../src/Host/SliceScenarioHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan54SevenDaySliceHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan54SevenDaySliceHostIntegrationTests.cs)

### 148. `outpost_settlement` — Plan 58 — authored outposts: establishment, condition, garrison assignments, and ration reserve (Settlements)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupOutpostSettlement()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`](../../Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs)
  - Host Session: [`src/Host/OutpostSettlementHostSession.cs`](../../src/Host/OutpostSettlementHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OutpostSettlementHostSession.cs`](../../src/Host/OutpostSettlementHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Settlements/Plan58OutpostHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Settlements/Plan58OutpostHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs`](../../Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs)

### 149. `cryo_vault` — Plan B69 — cryo canisters, viability, coolant reserve, insulation, breach state (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupCryoVault()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs`](../../Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CryoVaultSaveStore.cs`](../../src/Host/CryoVaultSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/CryoVaultB69Tests.cs`](../../Ashfall.Core.Tests/Shelter/CryoVaultB69Tests.cs)

### 150. `disaster_response` — ORPHAN-SEAL-W1 — active disasters, protocols, mitigation, and resilience (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupDisasterResponse()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs`](../../Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan158DisasterResponseIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan158DisasterResponseIntegrationTests.cs)

### 151. `ebpvd_coating` — Plans 146-149 — EB-PVD thermal barrier coating machinery, job state, and records (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupEbPvdCoating()` | **Cadence:** `On-Demand`
- **UI Routes:** `ebpvd_coating`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs`](../../Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs)
  - Host Session: [`src/Host/EbPvdCoatingHostSession.cs`](../../src/Host/EbPvdCoatingHostSession.cs)
  - Save Store: [`src/Host/EbPvdCoatingSaveStore.cs`](../../src/Host/EbPvdCoatingSaveStore.cs)
  - UI Panel: [`src/UI/EbPvdCoatingPanel.cs`](../../src/UI/EbPvdCoatingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/EbPvdCoatingEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/EbPvdCoatingEngineTests.cs)

### 152. `excavation_hazards` — Subterranean methane, flood, spore hazards, and cave-in rescue operations (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupExcavationHazards()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`](../../Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ExcavationHazardSaveStore.cs`](../../src/Host/ExcavationHazardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExcavationSystemTests.cs`](../../Ashfall.Core.Tests/ExcavationSystemTests.cs)

### 153. `food_preservation` — Food spoilage, curing, and cryogenic preservation (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPlans62To65()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`](../../Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FoodPreservationSaveStore.cs`](../../src/Host/FoodPreservationSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs)

### 154. `geothermal_aquifer` — Deep geothermal boreholes & aquifer pumping (Shelter)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupGeothermalAquifer()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs`](../../Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs)
  - Host Session: [`src/Host/GeothermalAquiferSaveStore.cs`](../../src/Host/GeothermalAquiferSaveStore.cs)
  - Save Store: [`src/Host/GeothermalAquiferSaveStore.cs`](../../src/Host/GeothermalAquiferSaveStore.cs)

### 155. `precision_metrology` — Plan B89 — precision metrology grades, certificates, and registered-consumer calibration (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPrecisionMetrology()` | **Cadence:** `Daily Calibration Drift`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs`](../../Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PrecisionMetrologySaveStore.cs`](../../src/Host/PrecisionMetrologySaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs`](../../Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs)

### 156. `precision_optics` — Plans 110-113 — precision optical blank grinding, figure testing, and telescope/shield viewports (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPrecisionOptics()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs`](../../Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs)
  - Host Session: [`src/Host/PrecisionOpticsHostSession.cs`](../../src/Host/PrecisionOpticsHostSession.cs)
  - Save Store: [`src/Host/PrecisionOpticsSaveStore.cs`](../../src/Host/PrecisionOpticsSaveStore.cs)

### 157. `radio_station` — Radio station frequency tuning, signal lock, and triangulation (Shelter)
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

### 158. `seismic_dynamics` — Plan B68 — fault tension, slips, geophone coverage, dampener integrity, quake history (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupSeismicDynamics()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SeismicDynamicsSaveStore.cs`](../../src/Host/SeismicDynamicsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/SeismicMonitoringB68Tests.cs`](../../Ashfall.Core.Tests/Shelter/SeismicMonitoringB68Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterSeismicDynamicsPlan56Tests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterSeismicDynamicsPlan56Tests.cs)

### 159. `shelter_atmosphere` — Plan 220 — shelter composite atmosphere, ambiance profile, and environmental facets (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAtmosphere()` | **Cadence:** `Daily (Day Coordinator)`
- **UI Routes:** `shelter_atmosphere`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs)
  - Host Session: [`src/Host/ShelterAtmosphereHostSession.cs`](../../src/Host/ShelterAtmosphereHostSession.cs)
  - Save Store: [`src/Host/ShelterAtmosphereSaveStore.cs`](../../src/Host/ShelterAtmosphereSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ShelterAtmospherePanel.cs`](../../src/UI/ShelterAtmospherePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs)

### 160. `shelter_decor` — Room decor placements, memorial plaques, and localized morale items (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterDecor()` | **Cadence:** `On-Demand (Decoration Placement)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs)
  - Host Session: [`src/Host/ShelterDecorHostSession.cs`](../../src/Host/ShelterDecorHostSession.cs)
  - Save Store: [`src/Host/ShelterDecorSaveStore.cs`](../../src/Host/ShelterDecorSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan12CDecorTests.cs`](../../Ashfall.Core.Tests/Plan12CDecorTests.cs)

### 161. `shelter_expansion` — ORPHAN-SEAL-W1 — expansion rooms, construction projects, and upgrade state (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterExpansion()` | **Cadence:** `Labor-Driven`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan156ShelterExpansionIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan156ShelterExpansionIntegrationTests.cs)

### 162. `shelter_noise` — Plan 205 — shelter acoustic noise, room soundproofing, and quiet hours (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAtmosphere()` | **Cadence:** `Daily (Midday Acoustic Audit)`
- **UI Routes:** `shelter_atmosphere`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs)
  - Host Session: [`src/Host/ShelterAtmosphereHostSession.cs`](../../src/Host/ShelterAtmosphereHostSession.cs)
  - Save Store: [`src/Host/ShelterNoiseSaveStore.cs`](../../src/Host/ShelterNoiseSaveStore.cs)
  - UI Panel: [`src/UI/ShelterAtmospherePanel.cs`](../../src/UI/ShelterAtmospherePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs)

### 163. `shelter_social_dynamics` — Living quarters privacy pressure, communal mess hall, and disputes (Shelter)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupShelterSocial()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterSocialSaveStore.cs`](../../src/Host/ShelterSocialSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterSocialDynamicsTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterSocialDynamicsTests.cs)

### 164. `shelter_workshop` — Precision workshop tooling, ammo press, and firearm refurbishment (Shelter)
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

### 165. `weather_hardening` — Cryo-ash weather hardening & thermal insulation (Shelter)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWeatherHardening()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WeatherHardeningSystem.cs`](../../Assets/Ashfall.Core/World/WeatherHardeningSystem.cs)
  - Host Session: [`src/Host/WeatherHardeningSaveStore.cs`](../../src/Host/WeatherHardeningSaveStore.cs)
  - Save Store: [`src/Host/WeatherHardeningSaveStore.cs`](../../src/Host/WeatherHardeningSaveStore.cs)

### 166. `cvd_diamond` — Plan 124 — CVD diamond reactor condition, plasma stability, growth batches, faults (Shelter & Facilities)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupCvdDiamond()` | **Cadence:** `Industrial Production Cadence (Batch Ticks)`
- **UI Routes:** `cvd_diamond`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/CvdDiamondSynthesisEngine.cs`](../../Assets/Ashfall.Core/Shelter/CvdDiamondSynthesisEngine.cs)
  - Host Session: [`src/Host/CvdDiamondHostSession.cs`](../../src/Host/CvdDiamondHostSession.cs)
  - Save Store: [`src/Host/CvdDiamondSaveStore.cs`](../../src/Host/CvdDiamondSaveStore.cs)
  - UI Panel: [`src/UI/CvdDiamondPanel.cs`](../../src/UI/CvdDiamondPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan124CvdDiamondSynthesisEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan124CvdDiamondSynthesisEngineTests.cs)

### 167. `sofc_power` — Plan 122 — SOFC plant operating mode, thermal level, stack health, seal integrity, degradation, faults (Shelter & Facilities)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupSofcPower()` | **Cadence:** `Shelter Power Cadence (TickDay)`
- **UI Routes:** `sofc_power`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SofcElectrochemistryEngine.cs`](../../Assets/Ashfall.Core/Shelter/SofcElectrochemistryEngine.cs)
  - Host Session: [`src/Host/SofcPowerHostSession.cs`](../../src/Host/SofcPowerHostSession.cs)
  - Save Store: [`src/Host/SofcPowerSaveStore.cs`](../../src/Host/SofcPowerSaveStore.cs)
  - UI Panel: [`src/UI/SolidOxideFuelCellPanel.cs`](../../src/UI/SolidOxideFuelCellPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan122SofcElectrochemistryEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan122SofcElectrochemistryEngineTests.cs)

### 168. `bio_fermentation` — Plan 126 — fermentation reactor, process health, contamination, outputs (Shelter & Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupBioFermentation()` | **Cadence:** `Daily Reactor Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/BioFermentationEngine.cs`](../../Assets/Ashfall.Core/Shelter/BioFermentationEngine.cs)
  - Host Session: [`src/Host/BioFermentationHostSession.cs`](../../src/Host/BioFermentationHostSession.cs)
  - Save Store: [`src/Host/BioFermentationSaveStore.cs`](../../src/Host/BioFermentationSaveStore.cs)
  - UI Panel: [`src/UI/BioFermentationPanel.cs`](../../src/UI/BioFermentationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/BioFermentationEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/BioFermentationEngineTests.cs)

### 169. `hydroponic_biomes` — Hydroponic biome racks and crop state (Shelter & Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupHydroponicBiomes()` | **Cadence:** `Daily Biome Rack Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs`](../../Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HydroponicBiomeSaveStore.cs`](../../src/Host/HydroponicBiomeSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/HydroponicBiomeTests.cs`](../../Ashfall.Core.Tests/Shelter/HydroponicBiomeTests.cs)

### 170. `airlock_security` — Airlock decontamination and security (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupAirlockSecurity()` | **Cadence:** `Daily Decon Interlock`
- **UI Routes:** `airlock_security`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AirlockSecuritySystem.cs`](../../Assets/Ashfall.Core/AirlockSecuritySystem.cs)
  - Host Session: [`src/Host/AirlockSecurityHostSession.cs`](../../src/Host/AirlockSecurityHostSession.cs)
  - Save Store: [`src/Host/AirlockSecuritySaveStore.cs`](../../src/Host/AirlockSecuritySaveStore.cs)
  - UI Panel: [`src/UI/AirlockSecurityPanel.cs`](../../src/UI/AirlockSecurityPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AirlockSecuritySystemTests.cs`](../../Ashfall.Core.Tests/AirlockSecuritySystemTests.cs)

### 171. `decontamination` — Rad-scrubbing showers and chambers (Shelter & Infrastructure)
- **Owner Domain:** `radiation`
- **Setup Method:** `Main.SetupDecontamination()` | **Cadence:** `Daily Rad Scrub Shower Cycle`
- **UI Routes:** `decontamination`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DecontaminationSystem.cs`](../../Assets/Ashfall.Core/DecontaminationSystem.cs)
  - Host Session: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - Save Store: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - UI Panel: [`src/UI/DecontaminationPanel.cs`](../../src/UI/DecontaminationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DecontaminationSystemTests.cs`](../../Ashfall.Core.Tests/DecontaminationSystemTests.cs)

### 172. `excavation` — Shelter expansion rubble clearing (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupExcavation()` | **Cadence:** `Daily Rubble Shoring Work`
- **UI Routes:** `excavation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExcavationSystem.cs`](../../Assets/Ashfall.Core/ExcavationSystem.cs)
  - Host Session: [`src/Host/ExcavationHostSession.cs`](../../src/Host/ExcavationHostSession.cs)
  - Save Store: [`src/Host/ExcavationSaveStore.cs`](../../src/Host/ExcavationSaveStore.cs)
  - UI Panel: [`src/UI/ExcavationPanel.cs`](../../src/UI/ExcavationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExcavationSystemTests.cs`](../../Ashfall.Core.Tests/ExcavationSystemTests.cs)

### 173. `greenhouse` — Hydroponic crops and food production (Shelter & Infrastructure)
- **Owner Domain:** `greenhouse`
- **Setup Method:** `Main.SetupGreenhouse()` | **Cadence:** `Daily Hydroponic Growth`
- **UI Routes:** `greenhouse`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`](../../Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs)
  - Host Session: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - Save Store: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - UI Panel: [`src/UI/GreenhousePanel.cs`](../../src/UI/GreenhousePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GreenhouseSystemTests.cs`](../../Ashfall.Core.Tests/GreenhouseSystemTests.cs)

### 174. `nuclear_core_lifecycle` — Nuclear core lifecycle and thermal state (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupNuclearCore()` | **Cadence:** `Daily Core Thermal Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs`](../../Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/NuclearCoreSaveStore.cs`](../../src/Host/NuclearCoreSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/NuclearCorePowerGridPublishTests.cs`](../../Ashfall.Core.Tests/Shelter/NuclearCorePowerGridPublishTests.cs)

### 175. `power_grid` — Shelter generator & power allocations (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupPowerGrid()` | **Cadence:** `Daily Fuel Consumption & Wattage`
- **UI Routes:** `power_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`](../../Assets/Ashfall.Core/Shelter/PowerGridSystem.cs)
  - Host Session: [`src/Host/PowerGridHostSession.cs`](../../src/Host/PowerGridHostSession.cs)
  - Save Store: [`src/Host/PowerGridSaveStore.cs`](../../src/Host/PowerGridSaveStore.cs)
  - UI Panel: [`src/UI/PowerGridPanel.cs`](../../src/UI/PowerGridPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs)

### 176. `power_subgrids` — Power distribution sub-grid nodes and thermal state (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupPowerSubgrids()` | **Cadence:** `Daily Thermal Distribution Tick`
- **UI Routes:** `power_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PowerDistributionSubgridSystem.cs`](../../Assets/Ashfall.Core/Shelter/PowerDistributionSubgridSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PowerDistributionSaveStore.cs`](../../src/Host/PowerDistributionSaveStore.cs)
  - UI Panel: [`src/UI/PowerGridPanel.cs`](../../src/UI/PowerGridPanel.cs)

### 177. `sanitation` — Plan 210 — room waste, hygiene, compost queue, and spills (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupSanitation()` | **Cadence:** `Daily Sanitation Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SanitationFacilityCatalog.cs`](../../Assets/Ashfall.Core/Shelter/SanitationFacilityCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/SanitationSystem.cs`](../../Assets/Ashfall.Core/Shelter/SanitationSystem.cs)
  - Host Session: [`src/Host/SanitationHostSession.cs`](../../src/Host/SanitationHostSession.cs)
  - Save Store: [`src/Host/SanitationSaveStore.cs`](../../src/Host/SanitationSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan210SanitationFacilityCatalogTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan210SanitationFacilityCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan210SanitationHostWiringTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan210SanitationHostWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan210SanitationSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan210SanitationSystemTests.cs)

### 178. `shelter_assignment` — Room assignments and living quarters (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAssignment()` | **Cadence:** `On-Demand (Bunk Reassignment)`
- **UI Routes:** `shelter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs)
  - Host Session: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - Save Store: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - UI Panel: [`src/UI/ShelterPanel.cs`](../../src/UI/ShelterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs)

### 179. `shelter_fire` — Shelter fire incidents, smoke, and brigade response (Shelter & Infrastructure)
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

### 180. `shelter_schedule` — Shift rotations and curfews (Shelter & Infrastructure)
- **Owner Domain:** `schedule`
- **Setup Method:** `Main.SetupShelterSchedule()` | **Cadence:** `Daily Curfew Rotation`
- **UI Routes:** `shelter_schedule`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterScheduleSystem.cs`](../../Assets/Ashfall.Core/ShelterScheduleSystem.cs)
  - Host Session: [`src/Host/ShelterScheduleHostSession.cs`](../../src/Host/ShelterScheduleHostSession.cs)
  - Save Store: [`src/Host/ShelterScheduleSaveStore.cs`](../../src/Host/ShelterScheduleSaveStore.cs)
  - UI Panel: [`src/UI/ShelterSchedulePanel.cs`](../../src/UI/ShelterSchedulePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs`](../../Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs)

### 181. `shelter_thermal` — Heating, insulation, and frost protection (Shelter & Infrastructure)
- **Owner Domain:** `thermal`
- **Setup Method:** `Main.SetupShelterThermal()` | **Cadence:** `Daily HVAC Frost Dissipation`
- **UI Routes:** `shelter_thermal`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterThermalSystem.cs`](../../Assets/Ashfall.Core/ShelterThermalSystem.cs)
  - Host Session: [`src/Host/ShelterThermalHostSession.cs`](../../src/Host/ShelterThermalHostSession.cs)
  - Save Store: [`src/Host/ShelterThermalSaveStore.cs`](../../src/Host/ShelterThermalSaveStore.cs)
  - UI Panel: [`src/UI/ShelterThermalPanel.cs`](../../src/UI/ShelterThermalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 182. `starting_level` — Bunker initial configuration & tier (Shelter & Infrastructure)
- **Owner Domain:** `starting_level`
- **Setup Method:** `Main.SetupStartingLevel()` | **Cadence:** `On-Demand (Opening Protocol)`
- **UI Routes:** `protocol`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`](../../Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs)
  - Host Session: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - Save Store: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - UI Panel: [`src/UI/OpeningProtocolModal.cs`](../../src/UI/OpeningProtocolModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/StartingLevelSystemTests.cs`](../../Ashfall.Core.Tests/StartingLevelSystemTests.cs)

### 183. `sump_flooding` — Bunker sump pump drainage & flood risk (Shelter & Infrastructure)
- **Owner Domain:** `maintenance`
- **Setup Method:** `Main.SetupSumpFlooding()` | **Cadence:** `Daily Drainage Pump Work`
- **UI Routes:** `sump_flooding`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SumpFloodingSystem.cs`](../../Assets/Ashfall.Core/SumpFloodingSystem.cs)
  - Host Session: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - Save Store: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - UI Panel: [`src/UI/SumpFloodingPanel.cs`](../../src/UI/SumpFloodingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs`](../../Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs)

### 184. `survivor_social` — Leadership, friction, ration conflict, trauma bonds, skill atrophy (Shelter & Infrastructure)
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

### 185. `vinyl_morale` — Gramophone records and music morale (Shelter & Infrastructure)
- **Owner Domain:** `morale`
- **Setup Method:** `Main.SetupVinylMorale()` | **Cadence:** `Daily Turntable Morale Broadcast`
- **UI Routes:** `vinyl_morale`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/VinylMoraleSystem.cs`](../../Assets/Ashfall.Core/VinylMoraleSystem.cs)
  - Host Session: [`src/Host/VinylMoraleHostSession.cs`](../../src/Host/VinylMoraleHostSession.cs)
  - Save Store: [`src/Host/VinylMoraleSaveStore.cs`](../../src/Host/VinylMoraleSaveStore.cs)
  - UI Panel: [`src/UI/VinylMoralePanel.cs`](../../src/UI/VinylMoralePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 186. `water_treatment` — Water filtration and purification (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaterTreatment()` | **Cadence:** `Daily Filtration Cycle`
- **UI Routes:** `water_treatment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaterTreatmentSystem.cs`](../../Assets/Ashfall.Core/WaterTreatmentSystem.cs)
  - Host Session: [`src/Host/WaterTreatmentHostSession.cs`](../../src/Host/WaterTreatmentHostSession.cs)
  - Save Store: [`src/Host/WaterTreatmentSaveStore.cs`](../../src/Host/WaterTreatmentSaveStore.cs)
  - UI Panel: [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaterTreatmentSystemTests.cs`](../../Ashfall.Core.Tests/WaterTreatmentSystemTests.cs)

### 187. `crafting` — Known recipes and workbench queues (Shelter & Logistics)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupCrafting()` | **Cadence:** `Daily Workbench Queue`
- **UI Routes:** `crafting`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/CraftingSystem.cs`](../../Assets/Ashfall.Core/Crafting/CraftingSystem.cs)
  - Host Session: [`src/Host/CraftingHostSession.cs`](../../src/Host/CraftingHostSession.cs)
  - Save Store: [`src/Host/CraftingSaveStore.cs`](../../src/Host/CraftingSaveStore.cs)
  - UI Panel: [`src/UI/CraftingPanel.cs`](../../src/UI/CraftingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CraftingSystemTests.cs`](../../Ashfall.Core.Tests/CraftingSystemTests.cs)

### 188. `equipment_condition` — Tool and weapon wear/repair (Shelter & Logistics)
- **Owner Domain:** `equipment`
- **Setup Method:** `Main.SetupEquipmentCondition()` | **Cadence:** `Daily Gear Wear & Maintenance`
- **UI Routes:** `equipment_condition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/EquipmentConditionSystem.cs`](../../Assets/Ashfall.Core/EquipmentConditionSystem.cs)
  - Host Session: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - Save Store: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - UI Panel: [`src/UI/EquipmentConditionPanel.cs`](../../src/UI/EquipmentConditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EquipmentConditionSystemTests.cs`](../../Ashfall.Core.Tests/EquipmentConditionSystemTests.cs)

### 189. `inventory` — Shelter warehouse & items storage (Shelter & Logistics)
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

### 190. `kitchen_nutrition` — Rationing recipes and caloric balance (Shelter & Logistics)
- **Owner Domain:** `nutrition`
- **Setup Method:** `Main.SetupKitchenNutrition()` | **Cadence:** `Daily Rationing Meal Prep`
- **UI Routes:** `kitchen_nutrition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/KitchenNutritionSystem.cs`](../../Assets/Ashfall.Core/KitchenNutritionSystem.cs)
  - Host Session: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - Save Store: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - UI Panel: [`src/UI/KitchenNutritionPanel.cs`](../../src/UI/KitchenNutritionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/KitchenNutritionSystemTests.cs`](../../Ashfall.Core.Tests/KitchenNutritionSystemTests.cs)

### 191. `radio` — Radio frequencies, logs, and distress signals (Shelter & Logistics)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadio()` | **Cadence:** `On-Demand (Frequency Scan)`
- **UI Routes:** `radio`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/FactionRadioEngine.cs`](../../Assets/Ashfall.Core/Radio/FactionRadioEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`](../../Assets/Ashfall.Core/Radio/RadioStationCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs`](../../Assets/Ashfall.Core/Radio/RadioStationCatalogLoader.cs)
  - Host Session: [`src/Host/RadioHostSession.cs`](../../src/Host/RadioHostSession.cs)
  - Save Store: [`src/Host/RadioSaveStore.cs`](../../src/Host/RadioSaveStore.cs)
  - UI Panel: [`src/Radio/FactionRadioHudPanel.cs`](../../src/Radio/FactionRadioHudPanel.cs)
  - UI Panel: [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`](../../Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`](../../Ashfall.Core.Tests/Radio/RadioStationParityTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/RadioSaveCodecTests.cs`](../../Ashfall.Core.Tests/RadioSaveCodecTests.cs)

### 192. `shelter_reputation` — Plan 207 — shelter reputation, notoriety, public tags, and external perception (Shelter (Plan 207))
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterReputation()` | **Cadence:** `Daily (Reputation Decay & Tag Evaluation)`
- **UI Routes:** `shelter_reputation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs`](../../Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs)
  - Host Session: [`src/Host/ShelterReputationHostSession.cs`](../../src/Host/ShelterReputationHostSession.cs)
  - Save Store: [`src/Host/ShelterReputationSaveStore.cs`](../../src/Host/ShelterReputationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ShelterReputationPanel.cs`](../../src/UI/ShelterReputationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Reputation/Plan207ShelterReputationIntegrationTests.cs`](../../Ashfall.Core.Tests/Reputation/Plan207ShelterReputationIntegrationTests.cs)

### 193. `shelter_security` — Plan 138 — shelter security zones, clearances, locks, lockdowns, and breaches (Shelter Defense (Plan 138))
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterSecurity()` | **Cadence:** `Daily (Breach Decay & Alert Drift)`
- **UI Routes:** `shelter_security`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterSecuritySystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterSecuritySystem.cs)
  - Host Session: [`src/Host/ShelterSecurityHostSession.cs`](../../src/Host/ShelterSecurityHostSession.cs)
  - Save Store: [`src/Host/ShelterSecuritySaveStore.cs`](../../src/Host/ShelterSecuritySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ShelterSecurityPanel.cs`](../../src/UI/ShelterSecurityPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan138ShelterSecurityIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan138ShelterSecurityIntegrationTests.cs)

### 194. `relationship_decay` — Plan 182 — survivor pair bond decay, interaction tracking, and social drift (Social Ecology & Drift (Plan 182))
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupRelationshipDecay()` | **Cadence:** `Daily (Pair Bond Decay & Social Drift)`
- **UI Routes:** `relationship_decay`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs`](../../Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs)
  - Host Session: [`src/Host/RelationshipDecayHostSession.cs`](../../src/Host/RelationshipDecayHostSession.cs)
  - Save Store: [`src/Host/RelationshipDecaySaveStore.cs`](../../src/Host/RelationshipDecaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/RelationshipDecayPanel.cs`](../../src/UI/RelationshipDecayPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/RelationshipDecaySystemTests.cs`](../../Ashfall.Core.Tests/Survivors/RelationshipDecaySystemTests.cs)

### 195. `hydrogeology_archive` — Plan 154 — Hydrogeology science archive: discovered-record ledger (IDs only) (Subterranean Science Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupHydroGeologyDiscovery()` | **Cadence:** `Event-Driven (Location Discovery)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/HydroGeologyDiscoverySystem.cs`](../../Assets/Ashfall.Core/Narrative/HydroGeologyDiscoverySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HydroGeologyArchiveSaveStore.cs`](../../src/Host/HydroGeologyArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HydroGeologyCatalogTests.cs`](../../Ashfall.Core.Tests/HydroGeologyCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/HydroGeologyDiscoveryTests.cs`](../../Ashfall.Core.Tests/Narrative/HydroGeologyDiscoveryTests.cs)

### 196. `apprenticeship` — Mentorship pairings and skill growth (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupApprenticeship()` | **Cadence:** `Daily Mentorship XP Transfer`
- **UI Routes:** `apprenticeship`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ApprenticeshipSystem.cs`](../../Assets/Ashfall.Core/ApprenticeshipSystem.cs)
  - Host Session: [`src/Host/ApprenticeshipHostSession.cs`](../../src/Host/ApprenticeshipHostSession.cs)
  - Save Store: [`src/Host/ApprenticeshipSaveStore.cs`](../../src/Host/ApprenticeshipSaveStore.cs)
  - UI Panel: [`src/UI/ApprenticeshipPanel.cs`](../../src/UI/ApprenticeshipPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`](../../Ashfall.Core.Tests/ApprenticeshipSystemTests.cs)

### 197. `autopsy` — Post-mortem forensic analysis (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupAutopsy()` | **Cadence:** `Daily Forensic Case Progress`
- **UI Routes:** `autopsy_report`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AutopsySystem.cs`](../../Assets/Ashfall.Core/AutopsySystem.cs)
  - Host Session: [`src/Host/AutopsyHostSession.cs`](../../src/Host/AutopsyHostSession.cs)
  - Save Store: [`src/Host/AutopsySaveStore.cs`](../../src/Host/AutopsySaveStore.cs)
  - UI Panel: [`src/UI/AutopsyReportPanel.cs`](../../src/UI/AutopsyReportPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AutopsySystemTests.cs`](../../Ashfall.Core.Tests/AutopsySystemTests.cs)

### 198. `caregiving` — Childcare, elderly care, and comfort (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupCaregiving()` | **Cadence:** `Daily Nursery/Eldercare Comfort`
- **UI Routes:** `caregiving`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/CaregivingSystem.cs`](../../Assets/Ashfall.Core/Survivors/CaregivingSystem.cs)
  - Host Session: [`src/Host/CaregivingHostSession.cs`](../../src/Host/CaregivingHostSession.cs)
  - Save Store: [`src/Host/CaregivingSaveStore.cs`](../../src/Host/CaregivingSaveStore.cs)
  - UI Panel: [`src/UI/CaregivingPanel.cs`](../../src/UI/CaregivingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CaregivingSystemTests.cs`](../../Ashfall.Core.Tests/CaregivingSystemTests.cs)

### 199. `chemical_dependency` — Substance dependencies and withdrawal (Survival & Biology)
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

### 200. `contractor_roster` — Hired mercenaries and specialists (Survival & Biology)
- **Owner Domain:** `personnel`
- **Setup Method:** `Main.SetupContractorRoster()` | **Cadence:** `Daily Mercenary Wage Payroll`
- **UI Routes:** `contractor_roster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ContractorRosterSystem.cs`](../../Assets/Ashfall.Core/ContractorRosterSystem.cs)
  - Host Session: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - Save Store: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - UI Panel: [`src/UI/ContractorRosterPanel.cs`](../../src/UI/ContractorRosterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ContractorRosterSystemTests.cs`](../../Ashfall.Core.Tests/ContractorRosterSystemTests.cs)

### 201. `disease` — Epidemics, contagions, and pathogen spread (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupDisease()` | **Cadence:** `Daily Pathogen Transmission`
- **UI Routes:** `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`](../../Assets/Ashfall.Core/Disease/DiseaseSystem.cs)
  - Host Session: [`src/Disease/DiseaseHostSession.cs`](../../src/Disease/DiseaseHostSession.cs)
  - Save Store: [`src/Host/DiseaseSaveStore.cs`](../../src/Host/DiseaseSaveStore.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiseaseSystemTests.cs`](../../Ashfall.Core.Tests/DiseaseSystemTests.cs)

### 202. `medical` — Triage, illnesses, and treatments (Survival & Biology)
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

### 203. `medical_ward` — Hospital ward beds and inpatients (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedicalWard()` | **Cadence:** `Daily Bed Inpatient Triage`
- **UI Routes:** `medical_ward`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/MedicalWardSystem.cs)
  - Host Session: [`src/Host/MedicalWardHostSession.cs`](../../src/Host/MedicalWardHostSession.cs)
  - Save Store: [`src/Host/MedicalWardSaveStore.cs`](../../src/Host/MedicalWardSaveStore.cs)
  - UI Panel: [`src/UI/MedicalWardPanel.cs`](../../src/UI/MedicalWardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs)

### 204. `mental_health_crisis` — Psychological trauma and psych ward (Survival & Biology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupMentalHealthCrisis()` | **Cadence:** `Daily Psych Ward Calming Ticks`
- **UI Routes:** `mental_health_crisis`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`](../../Assets/Ashfall.Core/MentalHealthCrisisSystem.cs)
  - Host Session: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - Save Store: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - UI Panel: [`src/UI/MentalHealthCrisisPanel.cs`](../../src/UI/MentalHealthCrisisPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs`](../../Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs)

### 205. `morale_contagion` — Flagship XI Plan 154 — morale contagion channels, breakdowns, social isolation, schism ledger, HopeBeacon installation (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupMoraleContagion()` | **Cadence:** `Daily Contagion / Isolation Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs`](../../Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs)
  - Host Session: [`src/Host/MoraleContagionHostSession.cs`](../../src/Host/MoraleContagionHostSession.cs)
  - Save Store: [`src/Host/MoraleContagionSaveStore.cs`](../../src/Host/MoraleContagionSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs`](../../Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs)

### 206. `survivor_relations` — Survivor affinities, feuds, and bonds (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupSurvivorRelations()` | **Cadence:** `Daily Affinity & Feud Drift`
- **UI Routes:** `survivor_relations`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SurvivorRelationsSystem.cs`](../../Assets/Ashfall.Core/SurvivorRelationsSystem.cs)
  - Host Session: [`src/Host/SurvivorRelationsHostSession.cs`](../../src/Host/SurvivorRelationsHostSession.cs)
  - Save Store: [`src/Host/SurvivorRelationsSaveStore.cs`](../../src/Host/SurvivorRelationsSaveStore.cs)
  - UI Panel: [`src/UI/SurvivorRelationsPanel.cs`](../../src/UI/SurvivorRelationsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 207. `survivors` — Living survivors, needs, and traits (Survival & Biology)
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

### 208. `death_legacy` — Plan 206 — survivor death records, last wills, estate inheritance, and disputes (Survivor Memorial & Wills (Plan 206))
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupDeathLegacy()` | **Cadence:** `Event-Driven & Daily Flush`
- **UI Routes:** `death_legacy`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorDeathLegacySystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorDeathLegacySystem.cs)
  - Host Session: [`src/Host/SurvivorDeathLegacyHostSession.cs`](../../src/Host/SurvivorDeathLegacyHostSession.cs)
  - Save Store: [`src/Host/SurvivorDeathLegacySaveStore.cs`](../../src/Host/SurvivorDeathLegacySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/SurvivorDeathLegacyPanel.cs`](../../src/UI/SurvivorDeathLegacyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan206SurvivorDeathLegacyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan206SurvivorDeathLegacyIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/SurvivorDeathLegacySystemTests.cs`](../../Ashfall.Core.Tests/Survivors/SurvivorDeathLegacySystemTests.cs)

### 209. `personal_quests` — Survivor personal quest progression (Survivor Quests (Plan 200))
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupPersonalQuests()` | **Cadence:** `Daily (Stage Progress & Life Stories)`
- **UI Routes:** `personal_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs`](../../Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs)
  - Host Session: [`src/Host/PersonalQuestHostSession.cs`](../../src/Host/PersonalQuestHostSession.cs)
  - Save Store: [`src/Host/PersonalQuestSaveStore.cs`](../../src/Host/PersonalQuestSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PersonalQuestPanel.cs`](../../src/UI/PersonalQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs`](../../Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs`](../../Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs)

### 210. `backstory` — Plan 174 — Procedural survivor backstories & origin mechanics: occupations, experiences, and secrets (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupBackstory()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `survivor_detail`, `survivors`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/BackstorySystem.cs`](../../Assets/Ashfall.Core/Survivors/BackstorySystem.cs)
  - Host Session: [`src/Host/BackstoryHostSession.cs`](../../src/Host/BackstoryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BackstoryHostSession.cs`](../../src/Host/BackstoryHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan174BackstoryHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan174BackstoryHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan174SurvivorBackstoriesIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan174SurvivorBackstoriesIntegrationTests.cs)

### 211. `ideological_friction` — Plan 148 — Ideological friction events and quests: confrontations, conversions, bunker factions, and mediation (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupIdeologicalFriction()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `survivors`, `survivor_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/IdeologicalFrictionEvents.cs`](../../Assets/Ashfall.Core/Survivors/IdeologicalFrictionEvents.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs`](../../Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs)
  - Host Session: [`src/Host/IdeologicalFrictionHostSession.cs`](../../src/Host/IdeologicalFrictionHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/IdeologicalFrictionHostSession.cs`](../../src/Host/IdeologicalFrictionHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - UI Panel: [`src/UI/SurvivorsPanel.cs`](../../src/UI/SurvivorsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/IdeologicalFrictionSystemTests.cs`](../../Ashfall.Core.Tests/IdeologicalFrictionSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan148IdeologicalFrictionHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan148IdeologicalFrictionHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan148IdeologicalFrictionIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan148IdeologicalFrictionIntegrationTests.cs)

### 212. `romance_family` — Plan 150 — Romance & family dynamics: attraction, courtship, partnership, bonded pairs, family units, and adoption (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupRomanceFamily()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:** `survivor_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/GenerationalLineageExtension.cs`](../../Assets/Ashfall.Core/GenerationalLineageExtension.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/RomanceCourtshipCatalogLoader.cs`](../../Assets/Ashfall.Core/Survivors/RomanceCourtshipCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/RomanceFamilySystem.cs`](../../Assets/Ashfall.Core/Survivors/RomanceFamilySystem.cs)
  - Host Session: [`src/Host/RomanceFamilyHostSession.cs`](../../src/Host/RomanceFamilyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RomanceFamilyHostSession.cs`](../../src/Host/RomanceFamilyHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Records/RomanceCourtshipCatalogLoaderTests.cs`](../../Ashfall.Core.Tests/Records/RomanceCourtshipCatalogLoaderTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan150RomanceFamilyHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan150RomanceFamilyHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan150RomanceFamilyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan150RomanceFamilyIntegrationTests.cs)

### 213. `survivor_voice` — Plan 42 — survivor voice line catalog selection, cooldowns, utterance history, and playback dispatch (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorVoice()` | **Cadence:** `Event-Driven`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Voice/SurvivorVoiceSystem.cs`](../../Assets/Ashfall.Core/Voice/SurvivorVoiceSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Voice/VoiceLineDispatchCoordinator.cs`](../../Assets/Ashfall.Core/Voice/VoiceLineDispatchCoordinator.cs)
  - Host Session: [`src/Host/SurvivorVoiceHostSession.cs`](../../src/Host/SurvivorVoiceHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorVoiceHostSession.cs`](../../src/Host/SurvivorVoiceHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan42SurvivorVoiceHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan42SurvivorVoiceHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Voice/SurvivorVoiceSystemTests.cs`](../../Ashfall.Core.Tests/Voice/SurvivorVoiceSystemTests.cs)

### 214. `hidden_agenda` — Plan 132 — survivor hidden agendas, secret motivations, clue discovery, and confrontation arcs (Survivors (Plan 132))
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupHiddenAgenda()` | **Cadence:** `Daily (Passive Slip-Up & Exposure Drift)`
- **UI Routes:** `hidden_agenda`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs`](../../Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs)
  - Host Session: [`src/Host/HiddenAgendaHostSession.cs`](../../src/Host/HiddenAgendaHostSession.cs)
  - Save Store: [`src/Host/HiddenAgendaSaveStore.cs`](../../src/Host/HiddenAgendaSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/HiddenAgendaPanel.cs`](../../src/UI/HiddenAgendaPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/HiddenAgendaSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/HiddenAgendaSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan132HiddenAgendaIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan132HiddenAgendaIntegrationTests.cs)

### 215. `combat` — Combat encounters and tactical trauma (Tactical Combat)
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

### 216. `technical_material_archive` — Plan 158 — cordage/cable/polymer/textile technical material archive: discovered-record ledger (IDs only) (Technical Material Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupTechnicalMaterialArchive()` | **Cadence:** `Event-Driven (Location Discovery)`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/TechnicalMaterialArchiveSystem.cs`](../../Assets/Ashfall.Core/Narrative/TechnicalMaterialArchiveSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TechnicalMaterialArchiveSaveStore.cs`](../../src/Host/TechnicalMaterialArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CordageCableCatalogTests.cs`](../../Ashfall.Core.Tests/CordageCableCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/TechnicalMaterialArchiveTests.cs`](../../Ashfall.Core.Tests/Narrative/TechnicalMaterialArchiveTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PolymerTextileCatalogTests.cs`](../../Ashfall.Core.Tests/PolymerTextileCatalogTests.cs)

### 217. `vehicle_customization` — Plan 152 — Vehicle customization & mobile base: module installation, effective vehicle stats, and deployed base camps (Vehicles)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupVehicleCustomization()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs`](../../Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Vehicles/VehicleModuleCatalogLoader.cs`](../../Assets/Ashfall.Core/Vehicles/VehicleModuleCatalogLoader.cs)
  - Host Session: [`src/Host/VehicleCustomizationHostSession.cs`](../../src/Host/VehicleCustomizationHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/VehicleCustomizationHostSession.cs`](../../src/Host/VehicleCustomizationHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Records/VehicleModuleCatalogLoaderTests.cs`](../../Ashfall.Core.Tests/Records/VehicleModuleCatalogLoaderTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Vehicles/Plan152VehicleCustomizationHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Vehicles/Plan152VehicleCustomizationHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Vehicles/Plan152VehicleCustomizationIntegrationTests.cs`](../../Ashfall.Core.Tests/Vehicles/Plan152VehicleCustomizationIntegrationTests.cs)

### 218. `deep_well` — B5–B8 Phase 6 — built deep-well pump: build state, condition, yield ledger (raw water into treatment via the Plan 189 intake seam) (Water & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupDeepWell()` | **Cadence:** `Daily Deep-Well Pump Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DeepWellSystem.cs`](../../Assets/Ashfall.Core/DeepWellSystem.cs)
  - Host Session: [`src/Host/DeepWellHostSession.cs`](../../src/Host/DeepWellHostSession.cs)
  - Host Session: [`src/Host/DeepWellSaveStore.cs`](../../src/Host/DeepWellSaveStore.cs)
  - Save Store: [`src/Host/DeepWellSaveStore.cs`](../../src/Host/DeepWellSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Water/DeepWellSystemTests.cs`](../../Ashfall.Core.Tests/Water/DeepWellSystemTests.cs)

### 219. `piezometer_network` — Plan 189 — aquifer monitoring network state driving the water-treatment intake advisory gate (Water & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupPiezometer()` | **Cadence:** `Daily Aquifer Advisory Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs`](../../Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs)
  - Host Session: [`src/Host/PiezometerHostSession.cs`](../../src/Host/PiezometerHostSession.cs)
  - Save Store: [`src/Host/PiezometerSaveStore.cs`](../../src/Host/PiezometerSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Water/Plan189IntakeAdvisoryBridgeTests.cs`](../../Ashfall.Core.Tests/Water/Plan189IntakeAdvisoryBridgeTests.cs)

### 220. `water_condenser` — B5–B8 expansion — Peltier condensation array: build state, membrane integrity, weather-indexed yield ledger (Water & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaterCondenser()` | **Cadence:** `Daily Condensate Intake Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AtmosphericCondenserSystem.cs`](../../Assets/Ashfall.Core/AtmosphericCondenserSystem.cs)
  - Host Session: [`src/Host/WaterCondenserHostSession.cs`](../../src/Host/WaterCondenserHostSession.cs)
  - Host Session: [`src/Host/WaterCondenserSaveStore.cs`](../../src/Host/WaterCondenserSaveStore.cs)
  - Save Store: [`src/Host/WaterCondenserSaveStore.cs`](../../src/Host/WaterCondenserSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs`](../../Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs)

### 221. `weather_cascade` — Plan 135 — weather→gameplay cascade: active weather events, their effects, and the event history (Weather)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWeatherCascade()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Records/WeatherCascadeCatalogLoader.cs`](../../Assets/Ashfall.Core/Records/WeatherCascadeCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Weather/WeatherCascadeSeverity.cs`](../../Assets/Ashfall.Core/Weather/WeatherCascadeSeverity.cs)
  - Core System: [`Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs`](../../Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Weather/WeatherGameplayCascadeEngine.cs`](../../Assets/Ashfall.Core/Weather/WeatherGameplayCascadeEngine.cs)
  - Host Session: [`src/Host/WeatherCascadeHostSession.cs`](../../src/Host/WeatherCascadeHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/WeatherCascadeHostSession.cs`](../../src/Host/WeatherCascadeHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Weather/Plan135WeatherCascadeHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Weather/Plan135WeatherCascadeHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Weather/Plan135WeatherCascadeIntegrationTests.cs`](../../Ashfall.Core.Tests/Weather/Plan135WeatherCascadeIntegrationTests.cs)

### 222. `ecological_infestation` — Plan 28 — location and shelter ecological infestations (trigger/clear/tolerate lifecycle) (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupEcologicalInfestation()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs`](../../Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/EcologicalInfestationSaveStore.cs`](../../src/Host/EcologicalInfestationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`](../../Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs)

### 223. `geodetic_survey` — Plans 78-81 — survey monuments, observations, resolved triangles, and network accuracy (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupGeodeticSurvey()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs`](../../Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs)
  - Host Session: [`src/Host/GeodeticSurveySaveStore.cs`](../../src/Host/GeodeticSurveySaveStore.cs)
  - Save Store: [`src/Host/GeodeticSurveySaveStore.cs`](../../src/Host/GeodeticSurveySaveStore.cs)

### 224. `human_migration` — Plan 199 — Seasonal human migration engine, regional population weights, and dwell hysteresis (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupHumanMigration()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs`](../../Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/SeasonalMigrationCatalogLoader.cs`](../../Assets/Ashfall.Core/Economy/SeasonalMigrationCatalogLoader.cs)
  - Host Session: [`src/Host/HumanMigrationHostSession.cs`](../../src/Host/HumanMigrationHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HumanMigrationHostSession.cs`](../../src/Host/HumanMigrationHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan199HumanMigrationHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Economy/Plan199HumanMigrationHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/SeasonalHumanMigrationEngineTests.cs`](../../Ashfall.Core.Tests/Economy/SeasonalHumanMigrationEngineTests.cs)

### 225. `nuclear_winter_progression` — ORPHAN-SEAL-W1 — nuclear-winter phase/season progression and recorded climate events (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupNuclearWinter()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Weather/NuclearWinterProgressionSystem.cs`](../../Assets/Ashfall.Core/Weather/NuclearWinterProgressionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Weather/Plan164NuclearWinterIntegrationTests.cs`](../../Ashfall.Core.Tests/Weather/Plan164NuclearWinterIntegrationTests.cs)

### 226. `route_infrastructure` — Plans 146-149 — mutable route infrastructure, corridor maintenance, and minefield clearance (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupRouteInfrastructure()` | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs`](../../Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs)
  - Host Session: [`src/Host/RouteInfrastructureSaveStore.cs`](../../src/Host/RouteInfrastructureSaveStore.cs)
  - Save Store: [`src/Host/RouteInfrastructureSaveStore.cs`](../../src/Host/RouteInfrastructureSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/RouteInfrastructureSystemTests.cs`](../../Ashfall.Core.Tests/World/RouteInfrastructureSystemTests.cs)

### 227. `subterranean` — Flagship XI Plan 156 — generated underground topology, oxygen/collapse/flood/shoring state, discovery (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupSubterranean()` | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Subterranean/SubterraneanSystem.cs`](../../Assets/Ashfall.Core/Subterranean/SubterraneanSystem.cs)
  - Host Session: [`src/Host/SubterraneanHostSession.cs`](../../src/Host/SubterraneanHostSession.cs)
  - Save Store: [`src/Host/SubterraneanSaveStore.cs`](../../src/Host/SubterraneanSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flagship11/SubterraneanSystemTests.cs`](../../Ashfall.Core.Tests/Flagship11/SubterraneanSystemTests.cs)

### 228. `amphibious_draisine` — Plan 125 — per-vehicle amphibious kit condition, pontoons, ingress, crossing state (World & Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupAmphibiousDraisine()` | **Cadence:** `Expedition Travel/Action Cadence (Crossing Ticks)`
- **UI Routes:** `amphibious_draisine`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/AmphibiousDraisineEngine.cs`](../../Assets/Ashfall.Core/Expeditions/AmphibiousDraisineEngine.cs)
  - Host Session: [`src/Host/AmphibiousDraisineHostSession.cs`](../../src/Host/AmphibiousDraisineHostSession.cs)
  - Save Store: [`src/Host/AmphibiousDraisineSaveStore.cs`](../../src/Host/AmphibiousDraisineSaveStore.cs)
  - UI Panel: [`src/UI/AmphibiousDraisinePanel.cs`](../../src/UI/AmphibiousDraisinePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan125AmphibiousDraisineEngineTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan125AmphibiousDraisineEngineTests.cs)

### 229. `armored_crawlers` — Armored crawler modules and forward camps (World & Expeditions)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupArmoredCrawlers()` | **Cadence:** `Daily Crawler Module Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs`](../../Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ArmoredCrawlerSaveStore.cs`](../../src/Host/ArmoredCrawlerSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs`](../../Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs)

### 230. `encounter_choice` — Encounter choice history & outcomes (World & Expeditions)
- **Owner Domain:** `encounters`
- **Setup Method:** `Main.SetupEncounterChoice()` | **Cadence:** `On-Demand (Door Event Resolution)`
- **UI Routes:** `door_encounter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Host Session: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Save Store: [`src/Host/EncounterChoiceSaveStore.cs`](../../src/Host/EncounterChoiceSaveStore.cs)
  - UI Panel: [`src/YearOfAsh/DoorEncounterModal.cs`](../../src/YearOfAsh/DoorEncounterModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs`](../../Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs)

### 231. `expedition` — Wasteland expedition runs & status (World & Expeditions)
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

### 232. `insar_deformation` — Plan 139 — repeat-pass InSAR survey passes, coherence, deformation summaries, excavation/travel intelligence (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupInSarMapping()` | **Cadence:** `On-Demand (Survey Pass & Repeat-Pass Process)`
- **UI Routes:** `insar_mapping`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/InSarDeformationEngine.cs`](../../Assets/Ashfall.Core/World/InSarDeformationEngine.cs)
  - Host Session: [`src/Host/InSarMappingHostSession.cs`](../../src/Host/InSarMappingHostSession.cs)
  - Save Store: [`src/Host/InSarMappingSaveStore.cs`](../../src/Host/InSarMappingSaveStore.cs)
  - UI Panel: [`src/UI/InSarMappingPanel.cs`](../../src/UI/InSarMappingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan139InSarDeformationTests.cs`](../../Ashfall.Core.Tests/World/Plan139InSarDeformationTests.cs)

### 233. `runflat_tire` — Plan 141 — run-flat wheel profiles, integrity, heat, rim/bead, rolling-resistance cost (World & Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupRunFlatTire()` | **Cadence:** `On-Demand (Fit, Hazard & Heat Commands)`
- **UI Routes:** `runflat_tire`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/RunFlatTireEngine.cs`](../../Assets/Ashfall.Core/Expeditions/RunFlatTireEngine.cs)
  - Host Session: [`src/Host/RunFlatTireHostSession.cs`](../../src/Host/RunFlatTireHostSession.cs)
  - Save Store: [`src/Host/RunFlatTireSaveStore.cs`](../../src/Host/RunFlatTireSaveStore.cs)
  - UI Panel: [`src/UI/RunFlatTirePanel.cs`](../../src/UI/RunFlatTirePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan141RunFlatTireTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan141RunFlatTireTests.cs)

### 234. `travel_encounters` — Travel encounters and cooldown states (World & Expeditions)
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

### 235. `wasteland_map` — Wasteland map markers and fog-of-war (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorld()` | **Cadence:** `On-Demand (Fog-of-War Discovery)`
- **UI Routes:** `map`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Host Session: [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
  - Save Store: [`src/Host/WastelandMapSaveStore.cs`](../../src/Host/WastelandMapSaveStore.cs)
  - UI Panel: [`src/UI/MapPanel.cs`](../../src/UI/MapPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`](../../Ashfall.Core.Tests/WastelandMapPersistenceTests.cs)

### 236. `waystation` — Wasteland outpost network & relay hubs (World & Expeditions)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaystation()` | **Cadence:** `Daily Outpost Relay Barter`
- **UI Routes:** `waystation_network`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaystationSystem.cs`](../../Assets/Ashfall.Core/WaystationSystem.cs)
  - Host Session: [`src/Host/WaystationHostSession.cs`](../../src/Host/WaystationHostSession.cs)
  - Save Store: [`src/Host/WaystationSaveStore.cs`](../../src/Host/WaystationSaveStore.cs)
  - UI Panel: [`src/UI/WaystationNetworkPanel.cs`](../../src/UI/WaystationNetworkPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaystationSystemTests.cs`](../../Ashfall.Core.Tests/WaystationSystemTests.cs)

### 237. `wildlife_trapping` — Snares, game catches, and foraging (World & Expeditions)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupWildlifeTrapping()` | **Cadence:** `Daily Snare Yield & Butchery`
- **UI Routes:** `wildlife_trapping`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WildlifeTrappingSystem.cs`](../../Assets/Ashfall.Core/WildlifeTrappingSystem.cs)
  - Host Session: [`src/Host/WildlifeTrappingHostSession.cs`](../../src/Host/WildlifeTrappingHostSession.cs)
  - Save Store: [`src/Host/WildlifeTrappingSaveStore.cs`](../../src/Host/WildlifeTrappingSaveStore.cs)
  - UI Panel: [`src/UI/WildlifeTrappingPanel.cs`](../../src/UI/WildlifeTrappingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs`](../../Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs)

### 238. `world` — World map nodes, sectors, and discovery (World & Expeditions)
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
| `aeroponics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `agriculture` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `airlock_security` | ✅ | ✅ | ✅ `Daily Decon Interlock` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `amphibious_draisine` | ✅ | ✅ | ⚡ `Expedition Travel/Action Cadence (Crossing Ticks)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `amputation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `anomaly_hazard` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `apprenticeship` | ✅ | ✅ | ✅ `Daily Mentorship XP Transfer` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `aquaponics` | ✅ | ✅ | ✅ `Daily Ecology Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `archaeology` | ✅ | ✅ | ⚡ `On-Demand (Excavation & Decryption)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `archive_desk` | ✅ | ✅ | ✅ `Daily Scribing & Folio Archival` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `armored_crawlers` | ✅ | ✅ | ✅ `Daily Crawler Module Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `autopsy` | ✅ | ✅ | ✅ `Daily Forensic Case Progress` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `aviation` | ✅ | ✅ | ✅ `Daily Flight Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `backstory` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ballistic_shield` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `ballistics_workbench` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `bio_fermentation` | ✅ | ✅ | ✅ `Daily Reactor Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `bionics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `black_market` | ✅ | ✅ | ✅ `Daily Underworld Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `black_projects_archive` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `campaign_day` | ✅ | ✅ | ✅ `Master Sim Clock / Dawn Advance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `campaign_legacy` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `caravan` | ✅ | ✅ | ✅ `Daily Route Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caravan_trade_network` | ✅ | ✅ | ✅ `Daily Route Arrival Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caregiving` | ✅ | ✅ | ✅ `Daily Nursery/Eldercare Comfort` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cargo_airdrop` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ceremony` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chem_warfare` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chemical_dependency` | ✅ | ✅ | ✅ `Daily Tolerance & Withdrawal` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chemical_recon` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `chemical_synthesis` | ✅ | ✅ | ⚡ `On-Demand (Retort Synthesis)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `child_development` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chlor_alkali_synthesis` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `collectible_discovery` | ✅ | ✅ | ⚡ `On-Demand (One-Time Discovery Ledger)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `colony` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `combat` | ✅ | ✅ | ⚡ `On-Demand (Turn-Based)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `commitment` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `comms_array` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `communications` | ✅ | ✅ | ✅ `Daily Wear Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `companion_animals` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `confession_secret` | ✅ | ✅ | ⚡ `Discovery-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `contraband_stash` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `contractor_roster` | ✅ | ✅ | ✅ `Daily Mercenary Wage Payroll` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cooking` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `counter_intelligence` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `crafting` | ✅ | ✅ | ✅ `Daily Workbench Queue` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cryo_vault` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `cryogenic_air_separation` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `cultural_archives` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `cvd_diamond` | ✅ | ✅ | ✅ `Industrial Production Cadence (Batch Ticks)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `daily_briefing` | ✅ | ✅ | ✅ `Daily Dawn Briefing Aggregation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `death_legacy` | ✅ | ✅ | ✅ `Event-Driven & Daily Flush` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `decontamination` | ✅ | ✅ | ✅ `Daily Rad Scrub Shower Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `deep_well` | ✅ | ✅ | ✅ `Daily Deep-Well Pump Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `desperation` | ✅ | ✅ | ⚡ `On-Demand (Crisis Command)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `diplomatic_summits` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `disaster_response` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `disease` | ✅ | ✅ | ✅ `Daily Pathogen Transmission` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dose_ledger` | ✅ | ✅ | ⚡ `On-Demand (Dose Log)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `draisine_recovery` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `duty_roster` | ✅ | ✅ | ✅ `Daily Shift Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dynamic_quests` | ✅ | ✅ | ⚡ `On-Demand (Campaign-Wide Emergency Quests)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ebpvd_coating` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `echoes` | ✅ | ✅ | ✅ `Narrative Echo Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `ecological_infestation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `economy` | ✅ | ✅ | ✅ `Daily Market Rate Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `encounter_choice` | ✅ | ✅ | ⚡ `On-Demand (Door Event Resolution)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `endgame` | ✅ | ✅ | ⚡ `On-Demand (Day Threshold / Extinction)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `equipment_condition` | ✅ | ✅ | ✅ `Daily Gear Wear & Maintenance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `espionage` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `excavation` | ✅ | ✅ | ✅ `Daily Rubble Shoring Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `excavation_hazards` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expansion_hub` | ✅ | ✅ | ✅ `Daily Hub Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expansion_quest` | ✅ | ✅ | ⚡ `On-Demand (Stage Milestone)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expedition` | ✅ | ✅ | ✅ `Daily Sortie Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expedition_stealth` | ✅ | ✅ | ⚡ `Event-Driven (Expedition Phases)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `faction_covert_ops` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `faction_espionage` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `fallout` | ✅ | ✅ | ✅ `Hourly Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `field_guide` | ✅ | ✅ | ⚡ `On-Demand (Study & Discovery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `fluid_logistics` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `food_preservation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `forced_labor` | ✅ | ✅ | ✅ `Daily Shift Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `fungi_cultivation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `geodetic_survey` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `geothermal_aquifer` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `geothermal_orc` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `grain_milling_archive` | ✅ | ✅ | ⚡ `Event-Driven (Location Discovery & Shelter Room Inspection)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `grain_processing` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `greenhouse` | ✅ | ✅ | ✅ `Daily Hydroponic Growth` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `heliograph` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `hidden_agenda` | ✅ | ✅ | ✅ `Daily (Passive Slip-Up & Exposure Drift)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `hobby` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `holdfast` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `holdfast_trade` | ✅ | ✅ | ⚡ `On-Demand (Barter)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `host_event` | ✅ | ✅ | ⚡ `On-Demand (Moral Dilemma)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `human_migration` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `hydraulic_extrusion` | ✅ | ✅ | ⚡ `On-Demand (Batch Phase Commands)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `hydrogeology_archive` | ✅ | ✅ | ⚡ `Event-Driven (Location Discovery)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `hydroponic_biomes` | ✅ | ✅ | ✅ `Daily Biome Rack Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ideological_friction` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `insar_deformation` | ✅ | ✅ | ⚡ `On-Demand (Survey Pass & Repeat-Pass Process)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `inventory` | ✅ | ✅ | ⚡ `On-Demand (Item Use)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `journal` | ✅ | ✅ | ⚡ `On-Demand (Log/Event)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `kinetic_storage` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `kitchen_nutrition` | ✅ | ✅ | ✅ `Daily Rationing Meal Prep` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `leatherwork_archive` | ✅ | ✅ | ⚡ `Event-Driven (Location Discovery & Item Inspection)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `library_study` | ✅ | ✅ | ✅ `Daily Codex Research Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `low_background_metrology` | ✅ | ✅ | ⚡ `On-Demand (Assay & Smelting Commands)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `lyophilization` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `maritime` | ✅ | ✅ | ⚡ `On-Demand (Dive Sortie)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical` | ✅ | ✅ | ✅ `Daily Recovery / Affliction` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_pipeline` | ✅ | ✅ | ⚡ `On-Demand (Triage & Procedure Commands)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_ward` | ✅ | ✅ | ✅ `Daily Bed Inpatient Triage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `memorial` | ✅ | ✅ | ⚡ `On-Demand (Survivor Fallen Eulogy)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mental_health_crisis` | ✅ | ✅ | ✅ `Daily Psych Ward Calming Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mercenary_bounties` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `meta_progression` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `microfluidic_diagnostic` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mine_clearing_flail` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `moral_choice` | ✅ | ✅ | ⚡ `On-Demand (Branch Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `morale_contagion` | ✅ | ✅ | ✅ `Daily Contagion / Isolation Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `muster` | ✅ | ✅ | ⚡ `On-Demand (Rally Stance)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mutation_tree` | ✅ | ✅ | ⚡ `Event-Driven (Dose Thresholds)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narcotics` | ✅ | ✅ | ✅ `24h Medical Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narrative` | ✅ | ✅ | ⚡ `On-Demand (Dialog Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narrative_questlines` | ✅ | ✅ | ⚡ `On-Demand (Survivor Narrative Arc Progression)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `npc_memory` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `nuclear_core_lifecycle` | ✅ | ✅ | ✅ `Daily Core Thermal Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `nuclear_winter_progression` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `nvis_communications` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `onboarding` | ✅ | ✅ | ⚡ `On-Demand (Player Sigil Recording)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `oral_lore` | ✅ | ✅ | ⚡ `Event-Driven (Performance)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `outpost_settlement` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `pathogen_strains` | ✅ | ✅ | ✅ `Daily Strain Progression Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `perimeter_defense` | ✅ | ✅ | ✅ `Daily Emplacement Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `personal_quests` | ✅ | ✅ | ✅ `Daily (Stage Progress & Life Stories)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `phantom_memory` | ✅ | ✅ | ⚡ `On-Demand (Scavenge Echo)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `phase0` | ✅ | ✅ | ⚡ `On-Demand (Pre-War Flashback)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `piezometer_network` | ✅ | ✅ | ✅ `Daily Aquifer Advisory Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `plastic_pyrolysis` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `playable_metrics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `pneumatic_dispatch` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `powder_metallurgy` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `power_grid` | ✅ | ✅ | ✅ `Daily Fuel Consumption & Wattage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `power_subgrids` | ✅ | ✅ | ✅ `Daily Thermal Distribution Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `precision_metrology` | ✅ | ✅ | ✅ `Daily Calibration Drift` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `precision_optics` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `prewar_archives` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `prisoner_management` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `procedural_narrative` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `propaganda_campaigns` | ✅ | ✅ | ✅ `Daily (Campaign Decay & Distribution)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `psychological_arcs` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `psychological_sanatorium` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `psyops` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `radio` | ✅ | ✅ | ⚡ `On-Demand (Frequency Scan)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `radio_program_production` | ✅ | ✅ | ✅ `Daily Program Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `radio_station` | ✅ | ✅ | ⚡ `On-Demand (Tuning & Broadcasts)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `rail_grinding` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `railway` | ✅ | ✅ | ⚡ `On-Demand (Convoy Operations)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `recon_telemetry` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `recreation` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `regional_treaty` | ✅ | ✅ | ✅ `Daily Non-Aggression Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `relationship_decay` | ✅ | ✅ | ✅ `Daily (Pair Bond Decay & Social Drift)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `research` | ✅ | ✅ | ⚡ `On-Demand (Study Progress)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `research_unlock` | ✅ | ✅ | ✅ `On-Demand (Research Node Completion)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `retention` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `robotics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `romance_family` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `route_infrastructure` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `runflat_tire` | ✅ | ✅ | ⚡ `On-Demand (Fit, Hazard & Heat Commands)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `sanitation` | ✅ | ✅ | ✅ `Daily Sanitation Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `seasonal_celebration` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `seismic_dynamics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `session_durability` | ✅ | ✅ | ⚡ `Session-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `settlement_defenses` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `settlement_politics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `seven_day_slice` | ✅ | ✅ | ⚡ `Playtest-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_assignment` | ✅ | ✅ | ⚡ `On-Demand (Bunk Reassignment)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_atmosphere` | ✅ | ✅ | ✅ `Daily (Day Coordinator)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_barter` | ✅ | ✅ | ⚡ `On-Demand (Barter)` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_decor` | ✅ | ✅ | ⚡ `On-Demand (Decoration Placement)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_expansion` | ✅ | ✅ | ⚡ `Labor-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_festival` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_fire` | ✅ | ✅ | ✅ `Daily Fire Propagation Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_governance` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_identity` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_noise` | ✅ | ✅ | ✅ `Daily (Midday Acoustic Audit)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_reputation` | ✅ | ✅ | ✅ `Daily (Reputation Decay & Tag Evaluation)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_schedule` | ✅ | ✅ | ✅ `Daily Curfew Rotation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_security` | ✅ | ✅ | ✅ `Daily (Breach Decay & Alert Drift)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_social_dynamics` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_thermal` | ✅ | ✅ | ✅ `Daily HVAC Frost Dissipation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_workshop` | ✅ | ✅ | ⚡ `On-Demand (Crafting & Refurbishment)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `silent_foundry` | ✅ | ✅ | ✅ `Daily Smelter Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `sky_defense_battery` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `sofc_power` | ✅ | ✅ | ✅ `Shelter Power Cadence (TickDay)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `solar_concentrator` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `sound_ranging` | ✅ | ✅ | ⚡ `Event-Driven (Hostile-Fire Observations) + Daily Drift` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `spiritual_meaning` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `starting_level` | ✅ | ✅ | ⚡ `On-Demand (Opening Protocol)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `subterranean` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `sump_flooding` | ✅ | ✅ | ✅ `Daily Drainage Pump Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `surgical_ward` | ✅ | ✅ | ✅ `Daily Sterile Field Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `survivor_autonomy` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `survivor_education` | ✅ | ✅ | ⚡ `Session-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `survivor_fate` | ✅ | ✅ | ✅ `Daily Survivor-Death Cascade` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_mental_health` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `survivor_relations` | ✅ | ✅ | ✅ `Daily Affinity & Feud Drift` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_social` | ✅ | ✅ | ✅ `Daily Shelter Social Dynamics` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_voice` | ✅ | ✅ | ⚡ `Event-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `survivors` | ✅ | ✅ | ✅ `Daily Needs Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `technical_material_archive` | ✅ | ✅ | ⚡ `Event-Driven (Location Discovery)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `territory_control` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `thirdonary` | ✅ | ✅ | ⚡ `On-Demand (Arbitration)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `time_capsules` | ✅ | ✅ | ✅ `Daily (Scheduled Opening & Message Delivery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `trade_routes` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `travel_encounters` | ✅ | ✅ | ⚡ `On-Demand (Travel Step)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `unified_ending` | ✅ | ✅ | ✅ `On-Demand (Campaign Sealed)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `unique_claims` | ✅ | ✅ | ⚡ `On-Demand (Global Unique Claim Ledger)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `vehicle_customization` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `vehicle_garage` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `verdict` | ✅ | ✅ | ✅ `Daily Machine Log Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `vinyl_morale` | ✅ | ✅ | ✅ `Daily Turntable Morale Broadcast` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_justice` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_map` | ✅ | ✅ | ⚡ `On-Demand (Fog-of-War Discovery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_rumors` | ✅ | ✅ | ✅ `Daily (Decay & Propagation)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `water_condenser` | ✅ | ✅ | ✅ `Daily Condensate Intake Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `water_treatment` | ✅ | ✅ | ✅ `Daily Filtration Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `waystation` | ✅ | ✅ | ✅ `Daily Outpost Relay Barter` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `weather_cascade` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `weather_hardening` | ✅ | ✅ | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `weight_of_choices` | ✅ | ✅ | ⚡ `On-Demand (Branch Decisions)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wildlife_ecosystem` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `wildlife_trapping` | ✅ | ✅ | ✅ `Daily Snare Yield & Butchery` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `world` | ✅ | ✅ | ✅ `Daily Weather & Hazard` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `year_of_ash` | ✅ | ✅ | ✅ `Daily Deep-Freeze Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `zealotry` | ✅ | ✅ | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |

---

## 5. Architectural Verification Invariants

1. **Invariant 1 (Core Engine Agnosticism):** Core systems contain zero references to `Godot`, `UnityEngine`, or engine globals.
2. **Invariant 3 (Save Store Integrity):** Every save store delegates to `SaveStoreHub` / `SaveEnvelopeHelper` or a Core codec and wraps state in a verified checksum envelope.
3. **Invariant 5 (Thin Host Nodes):** UI panels and host sessions handle only presentation, lifecycle, and wiring — never domain calculations.
4. **Invariant 6 (Data Authority):** `Assets/StreamingAssets/Data/` JSON files are the sole authority.
5. **Mechanical Reachability Gate:** Every system in this matrix is verified by headless test runs in `verify-fast.sh` and xUnit suites in `Ashfall.Core.Tests`.
6. **Zero Conceptual Placeholders:** If a layer is absent or procedural, it is documented with explicit status rather than filled with conceptual names.
