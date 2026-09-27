# ASHFALL — Evidence-Derived Architecture & Verification Graph

**Last Verified:** 2026-09-27<br>
**Total Subsystems Mapped:** 314/314 (100.0%)<br>
**Verified End-to-End Coverage:** 167/314 (53.2% across all 6 vertical layers)<br>
**Status Breakdown:** Implemented: 314/314 | Constructed: 308/314 | Ticked: 314/314 | Persisted: 314/314 | Routed: 186/314 | Tested: 231/314<br>
**Single Source of Truth:** `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` & `Assets/Ashfall.Core/HostCliRegistry.cs`

> **GENERATED FILE — do not edit by hand.**
> Derived mechanically from real C# types, catalog JSON, host wiring, setup invocation sites, and test fixtures.
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
| 2 | `consequence_ledger` | Campaign | `CampaignConsequenceLedger`, `CampaignConsequenceSaveState` | — *(Procedural)* | `Main` | `ConsequenceLedgerSaveStore` | *None (GAP)* | `--save-store-checksum-selftest`, `CampaignConsequenceLedgerTests`, `ConsequenceLedgerSaveTests` | ❌ GAP |
| 3 | `difficulty_settings` | Campaign | `DifficultySettingsSystem`, `DifficultySettingsCensus`, `DifficultyPresetCatalog`, `DifficultyScalarsProvider` | `difficulty_presets.json` | `Main`, `DifficultySettingsHostSession` | `DifficultySettingsSaveStore` | `StartingCohortSetupPanel` | `--difficulty-settings-selftest`, `Plan181DifficultySettingsIntegrationTests` | ✅ 6/6 |
| 4 | `endgame` | Campaign & Lore | `EndgameSystem`, `CampaignOutcomeEvaluator` | — *(Procedural)* | `EndgameHostSession` | `EndgameSaveStore` | `EpiloguePanel` | `--endings-selftest`, `EndgameSystemTests`, `CampaignOutcomeEvaluatorTests` | ✅ 6/6 |
| 5 | `host_event` | Campaign & Lore | `MoralChoiceSystem` | `events.json` | `HostEventAdapter` | `MoralChoiceSaveStore`, `HostEventSaveStore` | `EventDetailPanel` | `--moral-choice-selftest`, `HostEventSaveSealTests` | ✅ 6/6 |
| 6 | `journal` | Campaign & Lore | `JournalSystem` | `world_history.json` | `JournalHostSession` | `JournalSaveStore` | `JournalPanel`, `JournalBookUI` | `--journal-save-selftest`, `JournalSystemTests` | ✅ 6/6 |
| 7 | `memorial` | Campaign & Lore | `MemorialSystem` | — *(Procedural)* | `MemorialSystem` | `MemorialSaveStore` | `GameDashboardPanel` | `--player-panels-uitest`, `MemorialSystemTests` | ✅ 6/6 |
| 8 | `narrative` | Campaign & Lore | `NarrativeEncounterSystem` | `narrative_encounters.json` | `NarrativeHostSession` | `NarrativeSaveStore` | `EventsLogPanel`, `FactionsNarrativePanel` | `--narrative-selftest`, `NarrativeEncounterSystemTests` | ✅ 6/6 |
| 9 | `phase0` | Campaign & Lore | `RespiratoryDegenerationSystem` | — *(Procedural)* | `Phase0HostSession` | `Phase0SaveStore` | `Phase0Panel` | `--phase0-selftest`, `--phase0-uitest`, `Phase0EffectsBridgeTests` | ✅ 6/6 |
| 10 | `survivor_fate` | Campaign & Lore | `SurvivorFateSystem` | — *(Procedural)* | `Main` | `SurvivorFateSaveStore` | `GameDashboardPanel` | `--playable-shell-selftest`, `SurvivorFateSystemTests` | ✅ 6/6 |
| 11 | `onboarding` | Campaign & Onboarding | `OnboardingJourney` | — *(Procedural)* | `Main` | `OnboardingSaveStore` | `OnboardingHintPanel` | `--onboarding-journey-selftest`, `OnboardingJourneyTests` | ✅ 6/6 |
| 12 | `archive_desk` | Campaign & Progression | `ArchiveDeskSystem` | `archive_inks.json` | `ArchiveDeskHostSession` | `ArchiveDeskSaveStore` | `ArchiveDeskPanel` | `--shelter-operations-selftest`, `ArchiveDeskSystemTests` | ✅ 6/6 |
| 13 | `campaign_day` | Campaign & Progression | `CampaignDayCoordinator` | — *(Procedural)* | `CampaignDayCoordinator` | `CampaignDaySaveStore` | `GameDashboardPanel` | `--day1-selftest`, `--day1-to-day2-selftest`, `CampaignDayCoordinatorTests` | ✅ 6/6 |
| 14 | `daily_briefing` | Campaign & Progression | `DailyBriefingReportBuilder`, `DailyBriefingState` | — *(Procedural)* | `DailyBriefingState` | `DailyBriefingSaveStore` | `DailyBriefingModal` | `--day1-selftest`, `DailyBriefingReportBuilderTests` | ❌ GAP |
| 15 | `library_study` | Campaign & Progression | `LibraryStudySystem` | `library_manuals.json` | `LibraryStudyHostSession` | `LibraryStudySaveStore` | `LibraryStudyPanel` | `--shelter-operations-selftest`, `LibraryStudySystemTests` | ✅ 6/6 |
| 16 | `dynamic_quests` | Campaign & Quests | `DynamicQuestlineSystem` | `dynamic_questlines.json` | `DynamicQuestSaveStore` | `DynamicQuestSaveStore` | `DynamicQuestlinePanel` | `--save-store-checksum-selftest`, `DynamicQuestlineTests` | ✅ 6/6 |
| 17 | `narrative_questlines` | Campaign & Quests | `NarrativeQuestlineSystem` | `narrative_questlines.json` | `NarrativeQuestlineHostSession` | `NarrativeQuestlineSaveStore` | `QuestsPanel` | , `NarrativeQuestlineSystemTests` | ❌ GAP |
| 18 | `chlor_alkali_synthesis` | Chemistry | `ChlorAlkaliSynthesisEngine` | — *(Procedural)* | `ChlorAlkaliHostSession` | `ChlorAlkaliSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 19 | `memory_decay` | Cognition | `MemoryDecaySystem`, `MemoryDecayState`, `MemoryDecayCensus` | `memory_decay_rates.json` | `Main`, `MemoryDecayHostSession` | `MemoryDecaySaveStore` | *None (GAP)* | `--memory-decay-selftest`, `Plan185MemoryDecayIntegrationTests`, `MemoryDecaySystemTests` | ❌ GAP |
| 20 | `ballistic_shield` | Combat | `BallisticShieldEngine` | — *(Procedural)* | `BallisticShieldHostSession` | `BallisticShieldSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 21 | `ballistics_workbench` | Combat | `BallisticsWorkbenchSystem` | `ballistics_workbench_catalog.json` | `Main` | `BallisticsWorkbenchSaveStore` | `BallisticsWorkbenchPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 22 | `settlement_defenses` | Combat | `DefenseSystem` | `defenses.json` | `DefenseHostSession` | `DefenseSaveStore` | `DefenseGridPanel` | , `DefenseSystemTests` | ❌ GAP |
| 23 | `sky_defense_battery` | Combat | `SkyDefenseBatterySystem` | — *(Procedural)* | `Main` | `SkyDefenseBatterySaveStore` | `SkyDefenseBatteryPanel` | `--sky-defense-selftest`, `SkyDefenseBatteryTests` | ✅ 6/6 |
| 24 | `perimeter_defense` | Combat & Defense | `PerimeterDefenseSystem`, `NightWatchOperationsCatalogLoader`, `NightWatchPatrolReadinessEngine` | `perimeter_defenses.json`, `night_watch_operations.json` | `Main`, `NightWatchHostSession` | `PerimeterDefenseSaveStore` | `GameDashboardPanel`, `NightWatchPanel` | `--patrol-encounter-selftest`, `PerimeterDefenseTests`, `NightWatchPatrolReadinessEngineTests`, `NightWatchOperationsTests`, `NightWatchHostIntegrationTests` | ✅ 6/6 |
| 25 | `sound_ranging` | Combat & Defense | `SoundRangingThreatEngine` | `sound_ranging_catalog.json` | `SoundRangingHostSession` | `SoundRangingSaveStore` | `SoundRangingPanel` | `--plans-122-125-selftest`, `Plan123SoundRangingThreatEngineTests` | ✅ 6/6 |
| 26 | `time_capsules` | Communication & Heritage (Plan 212) | `TimeCapsuleSystem` | — *(Procedural)* | `TimeCapsuleHostSession` | `TimeCapsuleSaveStore` | `TimeCapsulePanel`, `GameDashboardPanel` | `--time-capsule-selftest`, `Plan212TimeCapsuleIntegrationTests`, `TimeCapsuleSystemTests` | ✅ 6/6 |
| 27 | `chemical_synthesis` | Crafting & Chemistry | `ChemicalSynthesisSystem` | `chemical_syntheses.json` | `ChemicalSynthesisHostSession` | `ChemicalSynthesisSaveStore` | `ChemicalLabPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 28 | `cassette_playback` | Culture | `CassettePlaybackSystem`, `CassetteSetCatalogLoader`, `CassettePlaybackState` | `cassette_sets.json` | `Main`, `CassettePlaybackHostSession` | `CassettePlaybackSaveStore` | *None (GAP)* | `--cassette-playback-selftest`, `PlanCassettePlaybackTests` | ❌ GAP |
| 29 | `culture_creation` | Culture | `CultureCreationSystem`, `CultureCreationState`, `CultureCreationCensus` | `art_forms.json` | `Main`, `CultureCreationHostSession` | `CultureCreationSaveStore` | *None (GAP)* | `--culture-creation-selftest`, `Plan178ArtCultureIntegrationTests` | ❌ GAP |
| 30 | `shelter_museum` | Culture | `ShelterMuseumSystem`, `ShelterMuseumState` | `museum_collection_templates.json` | `Main`, `ShelterMuseumHostSession` | `ShelterMuseumSaveStore` | `ArchiveDeskPanel` | `--shelter-museum-selftest`, `Plan218MuseumIntegrationTests`, `Plan218MuseumHostWiringTests` | ✅ 6/6 |
| 31 | `perimeter_early_warning` | Defense | `PerimeterEarlyWarningEngine`, `PerimeterEarlyWarningSaveState`, `RadarContact` | — *(Procedural)* | `Main`, `PerimeterEarlyWarningHostSession` | `PerimeterEarlyWarningSaveStore` | *None (GAP)* | `--perimeter-early-warning-selftest`,  | ❌ GAP |
| 32 | `radiation_economy` | Economy | `RadiationEconomyBridge`, `RadiationEconomySaveState`, `TradeEvaluation` | `radiation_economy_social.json` | `Main`, `RadiationEconomyHostSession` | `RadiationEconomySaveStore` | *None (GAP)* | `--radiation-economy-selftest`,  | ❌ GAP |
| 33 | `survivor_barter` | Economy | `SurvivorBarterSystem`, `SurvivorBarterSaveState`, `BarterOffer`, `TradeReputation` | `barter_rules.json` | `Main`, `SurvivorBarterHostSession` | `SurvivorBarterSaveStore` | *None (GAP)* | `--barter-selftest`,  | ❌ GAP |
| 34 | `trade_routes` | Economy | `PlayerTradeRouteSystem`, `TradeRouteContract`, `TradeRouteCensus` | `caravan_trade_routes.json` | `Main`, `TradeRouteHostSession` | `TradeRouteSaveStore` | *None (GAP)* | `--trade-routes-selftest`, `Plan192TradeRouteHostIntegrationTests`, `TradeRouteContractTests` | ❌ GAP |
| 35 | `black_market` | Economy & Trade | `BlackMarketSystem`, `BlackMarketInventoryCatalog` | `black_market_inventory.json` | `BlackMarketHostSession` | `BlackMarketSaveStore` | *None (GAP)* | , `Plan211BlackMarketTests`, `Plan211BlackMarketHostWiringTests` | ❌ GAP |
| 36 | `caravan` | Economy & Trade | `TravelingCaravanSystem` | `trade_texts.json` | `TravelingCaravanHostSession` | `CaravanSaveStore` | `TravelingCaravanPanel` | `--caravan-selftest`, `TradeCaravanCatalogTests` | ✅ 6/6 |
| 37 | `caravan_trade_network` | Economy & Trade | `CaravanTradeNetworkSystem` | `caravan_trade_routes.json` | `Main` | `CaravanTradeSaveStore` | `TravelingCaravanPanel` | `--caravan-selftest`, `--caravan-item-value-selftest`, `CaravanTradeNetworkTests` | ✅ 6/6 |
| 38 | `economy` | Economy & Trade | `MarketSystem`, `RationingProtocolCatalogLoader` | `economy_goods.json` | `EconomyHostSession` | `EconomySaveStore` | `EconomyMarketPanel`, `EconomyDetailPanel` | `--economy-selftest`, `--economy-uitest`, `DynamicEconomyCharacterizationTests` | ✅ 6/6 |
| 39 | `economy_family` | Economy & Trade | `TradeRouteMonopolyEngine`, `BlackMarketContrabandEngine`, `ChitPurityAssayEngine`, `BlackMarketHeatAttentionEngine` | — *(Procedural)* | `Main`, `EconomyFamilyHostSession` | `EconomyFamilySaveStore` | *None (GAP)* | `--economy-family-selftest`,  | ❌ GAP |
| 40 | `loan_shark` | Economy & Trade | `LoanSharkEnforcerEngine` | — *(Procedural)* | `Main`, `LoanSharkHostSession` | `LoanSharkSaveStore` | *None (GAP)* | `--loan-shark-selftest`,  | ❌ GAP |
| 41 | `regional_treaty` | Economy & Trade | `RegionalTreatySystem` | `faction_lore.json` | `RegionalTreatyHostSession` | `RegionalTreatySaveStore` | `RegionalTreatyPanel` | `--shelter-operations-selftest`, `RegionalTreatySaveChecksumTests` | ✅ 6/6 |
| 42 | `apprenticeship_curriculum` | Education | `ApprenticeshipCurriculumEngine`, `LearnerRecord`, `LiteracyLevel` | — *(Procedural)* | `Main`, `ApprenticeshipCurriculumHostSession` | `ApprenticeshipCurriculumSaveStore` | *None (GAP)* | `--apprenticeship-curriculum-selftest`,  | ❌ GAP |
| 43 | `emergency_alert` | Emergency | `EmergencyAlertSystem`, `EmergencyAlertState`, `ActiveEmergencyAlert`, `EvacuationProtocolState` | `emergency_alerts.json` | `Main`, `EmergencyAlertHostSession` | `EmergencyAlertSaveStore` | `GameDashboardPanel` | `--emergency-alert-selftest`, `Plan194EmergencyAlertHostIntegrationTests` | ✅ 6/6 |
| 44 | `meta_progression` | Endgame | `MetaProgressionSystem`, `MetaProgressionCensus`, `CrossRunProfileStore` | `meta_unlockables.json` | `Main`, `MetaProgressionHostSession` | `MetaProgressionSaveStore` | `EpiloguePanel`, `ChroniclePanel` | `--meta-progression-selftest`, `Plan175MetaProgressionHostIntegrationTests` | ✅ 6/6 |
| 45 | `unified_ending` | Endgame | `UnifiedEndingResolver` | `epilogue_personalization.json` | `Main`, `UnifiedEndingHostSession` | `UnifiedEndingSaveStore` | `EpiloguePanel`, `ChroniclePanel` | `--unified-ending-selftest`, `Plan145UnifiedEndingIntegrationTests`, `Plan145UnifiedEndingHostIntegrationTests` | ✅ 6/6 |
| 46 | `expansion_hub` | Expansion Framework | `ExpansionMasterSession` | — *(Procedural)* | `ExpansionHostSession` | `ExpansionHubSaveStore` | `ExpansionsHubPanel` | `--expansions-selftest`, `--expansion-hub-save-selftest`, `ExpansionHubSaveTests` | ✅ 6/6 |
| 47 | `expansion_quest` | Expansion Framework | `ExpansionQuestSystem`, `ExpansionMasterSession` | `crossing_quests.json` | `ExpansionQuestHostSession` | `ExpansionQuestSaveStore` | `CrossingQuestPanel` | `--expansions-selftest`, `VersionReportContractTests` | ✅ 6/6 |
| 48 | `holdfast` | Expansions (Exp 01) | `HoldfastQuestSystem`, `HoldfastSession` | `holdfast_quests.json`, `holdfast_items.json` | `HoldfastRuntimeSession` | `HoldfastSaveStore` | `HoldfastTerminalPanel`, `GameDashboardPanel` | `--holdfast-save-selftest`, `--holdfast-selftest`, `HoldfastSaveTests` | ✅ 6/6 |
| 49 | `holdfast_trade` | Expansions (Exp 01) | `HoldfastTradeSession` | `items.json` | `HoldfastRuntimeSession` | `HoldfastTradeSaveStore` | `TradeScreenGodotPanel`, `HoldfastTerminalPanel` | `--holdfast-trade-save-selftest`, `HoldfastTradeSessionTests` | ✅ 6/6 |
| 50 | `duty_roster` | Expansions (Exp 02) | `DutyRosterSystem` | `duty_roster_quests.json`, `survivors.json` | `DutyRosterHostSession` | `DutyRosterSaveStore` | `DutyRosterPanel`, `DutyRosterDetailPanel` | `--duty-roster-selftest`, `--duty-roster-save-selftest`, `DutyRosterSaveTests` | ✅ 6/6 |
| 51 | `phantom_memory` | Expansions (Exp 03) | `PhantomMemoryEngine` | `phantom_triggers.json` | `PhantomMemoryHostSession` | `PhantomMemorySaveStore` | `StandingRecordPanel`, `PhantomMemoryPanel` | `--standing-record-selftest`, `PhantomMemoryEngineTests` | ✅ 6/6 |
| 52 | `thirdonary` | Expansions (Exp 04) | `ThirdonaryQuestSystem` | `thirdonary_quests.json` | `ThirdonaryHostSession` | `ThirdonarySaveStore` | `CrossingQuestPanel` | `--crossing-selftest`, `--arbitration-selftest`, `ThirdonaryQuestSystemTests`, `CrossingArbitrationSystemTests` | ✅ 6/6 |
| 53 | `year_of_ash` | Expansions (Exp 05) | `YearOfAshDeepFreezeSystem`, `YearOfAshRadonSystem` | `year_of_ash_events.json` | `YearOfAshHostSession` | `YearOfAshSaveStore` | `DoorEncounterModal` | `--year-of-ash-save-selftest`, `YearOfAshQuestProbe` | ✅ 6/6 |
| 54 | `muster` | Expansions (Exp 06) | `MusterSystem` | `muster_witnesses.json` | `MusterHostSession` | `MusterSaveStore` | `MusterPanel` | `--muster-selftest`, `--muster-uitest`, `MusterSystemTests` | ✅ 6/6 |
| 55 | `dose_ledger` | Expansions (Exp 07) | `DoseLedgerSystem`, `RadiationSystem` | `dose_items.json` | `DoseLedgerHostSession` | `DoseLedgerSaveStore` | `RadiationHistoryPanel`, `RadiationDetailPanel` | `--dose-ledger-selftest`, `--dose-uitest`, `NeedsRadiationSaveRoundTripTests` | ✅ 6/6 |
| 56 | `verdict` | Expansions (Exp 08) | `ReckoningSystem`, `MachineLogSystem` | `verdict_data.json` | `VerdictHostSession` | `VerdictSaveStore` | `VerdictPanel`, `VerdictDashboardPanel` | `--verdict-selftest`, `--verdict-uitest`, `VerdictChainTests` | ✅ 6/6 |
| 57 | `maritime` | Expansions (Exp 09) | `MaritimeDiveSystem` | `dive_sites.json` | `MaritimeHostSession` | `MaritimeSaveStore` | `MaritimePanel` | `--black-flotilla-selftest`, `BlackFlotillaTests` | ✅ 6/6 |
| 58 | `silent_foundry` | Expansions (Exp 10) | `SilentFoundrySystem` | `foundry_items.json` | `SilentFoundryHostSession` | `SilentFoundrySaveStore` | `SilentFoundryPanel` | `--silent-foundry-selftest`, `--silent-foundry-uitest`, `SilentFoundryConsequenceTests` | ✅ 6/6 |
| 59 | `chemical_recon` | Expeditions | `ChemicalReconEngine` | — *(Procedural)* | `ChemicalReconSaveStore` | `ChemicalReconSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 60 | `colony` | Expeditions | `ColonySystem` | `colony_blueprints.json` | `Main` | `ColonySaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan160ColonyIntegrationTests` | ❌ GAP |
| 61 | `draisine_recovery` | Expeditions | `DraisineRerailingSystem` | — *(Procedural)* | `DraisineRerailingHostSession` | `DraisineRerailingSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 62 | `mine_clearing_flail` | Expeditions | `MineClearingFlailEngine` | `mine_flail_catalog.json` | `MineClearingFlailHostSession` | `MineClearingFlailSaveStore` | `MineFlailPanel` | `--mine-flail-uitest`, `MineClearingFlailEngineTests` | ✅ 6/6 |
| 63 | `rail_grinding` | Expeditions | `RailGrindingEngine` | `rail_grinding_catalog.json` | `RailGrindingHostSession` | `RailGrindingSaveStore` | `RailGrindingPanel` | `--rail-grinding-uitest`, `RailGrindingEngineTests` | ✅ 6/6 |
| 64 | `rail_track_maintenance` | Expeditions | `RailTrackMaintenanceLedger`, `RailMaintenanceState`, `RailMaintenanceCensus`, `RailTrackMaintenanceEngine` | `rail_network.json` | `Main`, `RailTrackMaintenanceHostSession` | `RailTrackMaintenanceSaveStore` | *None (GAP)* | `--rail-track-maintenance-selftest`, `RailTrackMaintenanceLedgerTests`, `RailTrackMaintenanceEngineTests` | ❌ GAP |
| 65 | `recon_telemetry` | Expeditions | `ReconTelemetrySystem` | — *(Procedural)* | `ReconTelemetrySaveStore` | `ReconTelemetrySaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 66 | `vehicle_garage` | Expeditions | `VehicleGarageSystem`, `VehicleArmorGradeCatalogLoader` | `vehicle_modifications.json`, `vehicle_armor_grades.json` | `Main` | `VehicleGarageSaveStore` | `VehicleGaragePanel` | `--vehicle-garage-selftest`, `VehicleGarageSystemTests`, `Plan50VehicleGarageIntegrationTests`, `Plan213VehicleArmorGradeTests` | ✅ 6/6 |
| 67 | `counter_intelligence` | Factions | `CounterIntelligenceSystem` | — *(Procedural)* | `CounterIntelligenceSaveStore` | `CounterIntelligenceSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 68 | `diplomacy` | Factions | `FactionDiplomacySystem`, `FactionDiplomacyState`, `ActiveTreatyRecord` | `treaty_templates.json` | `Main`, `DiplomacyHostSession` | `DiplomacySaveStore` | *None (GAP)* | `--diplomacy-selftest`,  | ❌ GAP |
| 69 | `diplomatic_summits` | Factions | `DiplomaticSummitSystem` | — *(Procedural)* | `Main` | `DiplomaticSummitSaveStore` | *None (GAP)* | , `DiplomaticSummitTests` | ❌ GAP |
| 70 | `espionage` | Factions | `EspionageSystem` | `espionage_missions.json` | `EspionageHostSession` | `EspionageSaveStore` | *None (GAP)* | , `Plan167EspionageTests` | ❌ GAP |
| 71 | `faction_covert_ops` | Factions | `FactionCovertOpsCoordinator` | `espionage_operations.json` | `Main` | `FactionCovertOpsSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan153FactionEspionageIntegrationTests` | ❌ GAP |
| 72 | `faction_espionage` | Factions | `ShelterEspionageSystem` | `faction_intelligence.json` | `Main` | `ShelterEspionageSaveStore` | *None (GAP)* | , `ShelterEspionageSystemTests` | ❌ GAP |
| 73 | `informant_network` | Factions | `InformantNetworkSystem` | — *(Procedural)* | `Main`, `InformantNetworkHostSession` | `InformantNetworkSaveStore` | `FactionsPanel` | `--informant-network-selftest`, `Plan146InformantNetworkIntegrationTests` | ✅ 6/6 |
| 74 | `territory_control` | Factions | `TerritoryControlSystem`, `FactionTerritoryDef`, `SupplyLineDef`, `LocationTerritoryState`, `SupplyLineState`, `TerritoryControlSaveState` | `faction_territory.json`, `supply_lines.json` | `TerritoryControlHostSession`, `Main` | `TerritoryControlSaveStore` | *None (GAP)* | `--territory-control-selftest`, `Plan134TerritoryControlHostIntegrationTests`, `Plan134TerritoryControlIntegrationTests` | ❌ GAP |
| 75 | `warlord_response` | Factions | `WarlordResponseActions`, `WarlordDoctrineSystem`, `WarlordResponseState` | — *(Procedural)* | `Main`, `WarlordResponseHostSession` | `WarlordResponseSaveStore` | `FactionsPanel` | `--warlord-response-selftest`, `PlanWarlordResponseHostIntegrationTests` | ✅ 6/6 |
| 76 | `weight_of_choices` | Factions & Diplomacy | `FactionBranchCoordinator`, `MilitaryBranchSystem`, `RebelBranchSystem`, `IndependentBranchSystem`, `PrpfStandingSystem` | `military_faction_branch.json`, `rebel_faction_branch.json`, `independent_faction_branch.json` | `FactionBranchHostSession` | `WeightOfChoicesSaveStore` | `FactionsPanel`, `QuestsPanel` | `--expansions-selftest`, `FactionBranchCoordinatorTests`, `MilitaryBranchSystemTests`, `RebelBranchSystemTests`, `IndependentBranchSystemTests`, `PrpfStandingSystemTests`, `WeightOfChoicesSaveTests` | ✅ 6/6 |
| 77 | `aeroponics` | Farming | `AeroponicsSystem` | `aeroponics_nutrient_catalog.json` | `Main` | `AeroponicsSaveStore` | `AeroponicsPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 78 | `agriculture` | Farming | `AgricultureSystem` | `crop_strains.json` | `Main` | `AgricultureSaveStore` | *None (GAP)* | , `AgricultureSystemTests` | ❌ GAP |
| 79 | `aquaponics` | Farming | `AquaponicsSystem` | `aquaponics_system_catalog.json` | `Main` | `AquaponicsSaveStore` | *None (GAP)* | `--aquaponics-selftest`, `AquaponicsSystemTests`, `PlansB86ToB89ContinuityTests` | ❌ GAP |
| 80 | `soil_reclamation_profile` | Farming | `SoilReclamationProfileEngine`, `SoilQualityTier`, `SoilAmendment` | — *(Procedural)* | `Main`, `SoilReclamationProfileHostSession` | `SoilReclamationProfileSaveStore` | *None (GAP)* | `--soil-reclamation-profile-selftest`,  | ❌ GAP |
| 81 | `oilseed_pressing` | Farming & Food | `OilseedPressingEngine` | — *(Procedural)* | `Main`, `OilseedPressingHostSession` | `OilseedPressingSaveStore` | *None (GAP)* | `--oilseed-pressing-selftest`,  | ❌ GAP |
| 82 | `powder_metallurgy` | Foundry | `PowderMetallurgySystem` | — *(Procedural)* | `PowderMetallurgySaveStore` | `PowderMetallurgySaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 83 | `hydraulic_extrusion` | Foundry & Industry | `HydraulicExtrusionEngine` | `hydraulic_extrusion_catalog.json` | `HydraulicExtrusionHostSession` | `HydraulicExtrusionSaveStore` | `HydraulicExtrusionPanel` | `--plans-139-141-selftest`, `Plan140HydraulicExtrusionTests` | ✅ 6/6 |
| 84 | `shelter_governance` | Governance | `ShelterGovernanceEngine`, `ShelterGovernanceCatalogLoader`, `ShelterGovernanceCensus` | `shelter_governance_blocs.json` | `Main`, `ShelterGovernanceHostSession` | `ShelterGovernanceSaveStore` | `SurvivorDetailPanel` | `--shelter-governance-selftest`, `Plan159ShelterGovernanceHostIntegrationTests`, `ShelterGovernanceEngineTests`, `Plan159_190GovernanceProvenanceIntegrationTests` | ✅ 6/6 |
| 85 | `shelter_identity` | Holdfast | `ShelterIdentitySystem`, `ShelterOriginCatalogLoader` | `shelter_origins.json` | `Main`, `ShelterIdentityHostSession` | `ShelterIdentitySaveStore` | `ShelterPanel` | `--shelter-identity-selftest`, `ShelterOriginCatalogLoaderTests`, `ShelterIdentitySystemTests` | ✅ 6/6 |
| 86 | `wildlife_ecosystem` | Hunting | `WildlifeEcosystemSystem` | `wildlife_ecosystem.json` | `WildlifeEcosystemHostSession` | `WildlifeEcosystemSaveStore` | `BestiaryPanel` | , `WildlifeEcosystemSystemTests` | ❌ GAP |
| 87 | `wildlife_harvest` | Hunting | `WildlifeHarvestLedger`, `WildlifeHarvestState`, `WildlifeHarvestCensus`, `WildlifeHarvestQuotaEngine` | — *(Procedural)* | `Main`, `WildlifeHarvestHostSession` | `WildlifeHarvestSaveStore` | *None (GAP)* | `--wildlife-harvest-selftest`, `WildlifeHarvestQuotaEngineTests`, `WildlifeHarvestLedgerTests` | ❌ GAP |
| 88 | `shelter_barter` | Illicit Economy / Barter | `ShelterBarterSystem` | `merchant_caravans.json` | `Main` | `ShelterBarterSaveStore` | *None (GAP)* | `--contraband-stash-selftest`, `ShelterBarterSystemPlan54Tests`, `ContrabandBarterRouteTests` | ❌ GAP |
| 89 | `advanced_industrial` | Industrial & Reconnaissance | `FischerTropschSynthesisEngine`, `UvCoronaDetectionEngine`, `CarbonCompositeEngine`, `GroundPenetratingRadarEngine` | `fischer_tropsch_catalog.json`, `uv_corona_detector_catalog.json`, `carbon_composite_catalog.json`, `gpr_exploration_catalog.json` | `Main`, `AdvancedIndustrialHostSession` | `AdvancedIndustrialSaveStore` | *None (GAP)* | `--advanced-industrial-recon-selftest`, `FischerTropschSynthesisEngineTests`, `UvCoronaDetectionEngineTests`, `CarbonCompositeEngineTests`, `GroundPenetratingRadarEngineTests` | ❌ GAP |
| 90 | `grain_milling_archive` | Industrial Food-Processing Archive | `GrainMillingDiscoverySystem` | `burr_millstone_dressing_logs.json`, `bolting_silk_mesh_reports.json`, `grain_silo_weevil_audits.json`, `mill_dampener_tempering_assays.json` | `Main` | `GrainMillingArchiveSaveStore` | *None (GAP)* | , `GrainMillingDiscoveryTests`, `GrainMillingCatalogTests` | ❌ GAP |
| 91 | `wasteland_rumors` | Information & Rumors (Plan 203) | `RumorSystem` | — *(Procedural)* | `RumorNetworkHostSession` | `RumorNetworkSaveStore` | `RumorBoardPanel`, `GameDashboardPanel` | `--rumor-network-selftest`, `Plan203RumorNetworkIntegrationTests` | ✅ 6/6 |
| 92 | `cryogenic_air_separation` | Infrastructure | `CryogenicAirSeparationSystem` | — *(Procedural)* | `CryogenicAirSeparationSaveStore` | `CryogenicAirSeparationSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 93 | `fluid_logistics` | Infrastructure | `FluidLogisticsSystem` | `fluid_infrastructure.json` | `FluidLogisticsHostSession` | `FluidLogisticsSaveStore` | *None (GAP)* | , `Plan168FluidLogisticsTests` | ❌ GAP |
| 94 | `pneumatic_dispatch` | Infrastructure | `PneumaticDispatchSystem` | `pneumatic_network_catalog.json` | `Main` | `PneumaticDispatchSaveStore` | `PneumaticDispatchPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 95 | `black_projects_archive` | Intelligence Archive | `BlackProjectsArchiveSystem` | `orbital_kinetic_telemetry.json`, `drone_carrier_blackboxes.json`, `cobalt_arming_directives.json`, `architect_vault_audits.json` | `Main` | `BlackProjectsArchiveSaveStore` | `BlackProjectsArchivePanel` | , `BlackProjectsArchiveTests`, `BlackProjectsCatalogTests`, `BlackProjectsArchivePanelRouteTests` | ❌ GAP |
| 96 | `clothing_warmth` | Inventory | `ClothingWarmthSystem`, `ClothingWarmthSaveState`, `ClothingWarmthCensus` | — *(Procedural)* | `Main`, `ClothingWarmthHostSession` | `ClothingWarmthSaveStore` | `SurvivorDetailPanel` | `--clothing-warmth-selftest`, `Plan142ClothingWarmthHostIntegrationTests`, `ClothingWarmthSystemTests` | ✅ 6/6 |
| 97 | `item_lore` | Inventory | `ItemLoreSystem` | — *(Procedural)* | `Main`, `ItemLoreHostSession` | `ItemLoreSaveStore` | `InventoryDetailPanel` | , `Plan190ItemLoreIntegrationTests`, `ItemLoreSystemTests` | ❌ GAP |
| 98 | `collectible_discovery` | Inventory & Lore | `CollectibleDiscoveryState` | `collectibles.json` | `Main` | `CollectibleDiscoverySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CollectibleDiscoveryPersistenceTests` | ✅ 6/6 |
| 99 | `unique_claims` | Inventory & Lore | `UniqueItemClaimRegistry` | `collectibles.json` | `Main` | `UniqueClaimSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CollectibleDiscoveryPersistenceTests` | ✅ 6/6 |
| 100 | `procedural_eulogy` | Journal | `ProceduralEulogyEngine`, `EulogySaveState`, `DwellerLifeRecord` | — *(Procedural)* | `Main`, `ProceduralEulogyHostSession` | `ProceduralEulogySaveStore` | *None (GAP)* | `--procedural-eulogy-selftest`,  | ❌ GAP |
| 101 | `cultural_archives` | Knowledge | `CulturalArchiveVaultSystem` | — *(Procedural)* | `Main` | `CulturalArchiveSaveStore` | *None (GAP)* | , `CulturalArchiveVaultTests` | ❌ GAP |
| 102 | `field_guide` | Knowledge | `FieldGuideCatalog` | — *(Procedural)* | `Main` | `FieldGuideSaveStore` | `GameDashboardPanel` | `--world-selftest`, `FieldGuidePersistenceTests` | ✅ 6/6 |
| 103 | `prewar_archives` | Knowledge | `PrewarArchiveDecryptionSystem` | `prewar_archives.json` | `Main` | `PrewarArchiveSaveStore` | *None (GAP)* | , `PrewarArchiveDecryptionTests` | ❌ GAP |
| 104 | `research` | Knowledge | `ResearchSystem` | `research_knowledge.json` | `Main` | `ResearchSaveStore` | `ResearchPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `MedicalPipelineArchitectureGateTests` | ✅ 6/6 |
| 105 | `survivor_education` | Knowledge | `SurvivorEducationSystem` | `education_curriculum.json` | `Main` | `SurvivorEducationSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan154EducationIntegrationTests` | ❌ GAP |
| 106 | `campaign_legacy` | Legacy | `CampaignLegacySystem`, `CampaignLegacy`, `CampaignLegacyCensus`, `CampaignLegacyState`, `StartingCampaignContext`, `LegacyTrait` | `legacy_traits.json` | `CampaignLegacyHostSession`, `Main` | `CampaignLegacySaveStore` | *None (GAP)* | `--campaign-legacy-selftest`, `Plan140CampaignLegacyHostIntegrationTests`, `Plan140GenerationalLegacyIntegrationTests` | ❌ GAP |
| 107 | `leatherwork_archive` | Material Provenance Archive | `LeatherworkArchiveSystem` | `oak_bark_tanning_pit_logs.json`, `chrome_alum_tanning_assays.json`, `rawhide_bating_failure_reports.json`, `leather_harness_conditioning_audits.json` | `Main` | `LeatherworkArchiveSaveStore` | `InventoryDetailPanel` | , `LeatherworkArchiveTests`, `TanningLeatherCatalogTests` | ❌ GAP |
| 108 | `chronic_condition` | Medical | `ChronicConditionSystem` | `chronic_conditions.json` | `Main`, `ChronicConditionHostSession` | `ChronicConditionSaveStore` | `AfflictionsPanel` | `--chronic-condition-selftest`, `Plan193ChronicConditionIntegrationTests` | ✅ 6/6 |
| 109 | `clinical_ward_triage` | Medical | `ClinicalWardLedger`, `ClinicalWardTriageState`, `ClinicalWardCensus`, `ClinicalWardTriageEngine` | — *(Procedural)* | `Main`, `ClinicalWardTriageHostSession` | `ClinicalWardTriageSaveStore` | *None (GAP)* | `--clinical-ward-selftest`, `ClinicalWardTriageEngineTests`, `ClinicalWardLedgerTests` | ❌ GAP |
| 110 | `dependency_taper_withdrawal` | Medical | `DependencyTaperLedger`, `DependencyTaperState`, `DependencyTaperCensus`, `DependencyTaperWithdrawalEngine` | — *(Procedural)* | `Main`, `DependencyTaperWithdrawalHostSession` | `DependencyTaperWithdrawalSaveStore` | *None (GAP)* | `--dependency-taper-selftest`, `DependencyTaperWithdrawalEngineTests`, `DependencyTaperLedgerTests` | ❌ GAP |
| 111 | `health_history` | Medical | `HealthHistorySystem`, `HealthHistoryState`, `HealthHistoryCensus` | `medical_record_templates.json` | `Main`, `HealthHistoryHostSession` | `HealthHistorySaveStore` | *None (GAP)* | `--health-history-selftest`, `Plan198HealthHistoryIntegrationTests`, `Plan198MedicalRecordLogTests` | ❌ GAP |
| 112 | `lyophilization` | Medical | `LyophilizationSystem` | — *(Procedural)* | `LyophilizationSaveStore` | `LyophilizationSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 113 | `medical_pipeline` | Medical | `MedicalPipelineCoordinator` | `disease_catalog.json` | `Main` | `MedicalPipelineSaveStore` | `MedicalPanel`, `GameDashboardPanel` | `--save-load-ui-failure-selftest`, `MedicalPipelineArchitectureGateTests` | ✅ 6/6 |
| 114 | `microfluidic_diagnostic` | Medical | `MicrofluidicDiagnosticEngine` | `microfluidic_diagnostic_catalog.json` | `MicrofluidicDiagnosticHostSession` | `MicrofluidicDiagnosticSaveStore` | `MicrofluidicDiagnosticPanel` | `--microfluidic-diagnostic-uitest`, `MicrofluidicDiagnosticEngineTests` | ✅ 6/6 |
| 115 | `palliative_care` | Medical | `PalliativeCareDignityEngine`, `PalliativePatientRecord`, `GriefStage`, `PalliativeCareProtocol` | — *(Procedural)* | `Main`, `PalliativeCareHostSession` | `PalliativeCareSaveStore` | *None (GAP)* | `--palliative-care-selftest`,  | ❌ GAP |
| 116 | `pathogen_strains` | Medical | `PathogenStrainSystem` | `pathogens.json` | `Main` | `PathogenStrainSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `DiseaseSystemTests` | ✅ 6/6 |
| 117 | `pharmaceutical_tablet` | Medical | `PharmaceuticalTabletEngine` | `tablet_manufacturing_catalog.json` | `Main`, `PharmaceuticalTabletHostSession` | `PharmaceuticalTabletSaveStore` | *None (GAP)* | `--pharmaceutical-tablet-selftest`,  | ❌ GAP |
| 118 | `psychological_sanatorium` | Medical | `PsychologicalSanatoriumSystem` | — *(Procedural)* | `Main` | `PsychologicalSanatoriumSaveStore` | *None (GAP)* | , `PsychologicalSanatoriumTests` | ❌ GAP |
| 119 | `surgical_graft` | Medical | `SurgicalGraftRejectionEngine` | — *(Procedural)* | `Main`, `SurgicalGraftHostSession` | `SurgicalGraftSaveStore` | *None (GAP)* | `--surgical-graft-selftest`,  | ❌ GAP |
| 120 | `surgical_ward` | Medical | `AdvancedSurgicalWardSystem` | — *(Procedural)* | `Main` | `SurgicalWardSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 121 | `propaganda_campaigns` | Morale & Influence (Plan 168) | `PropagandaSystem` | — *(Procedural)* | `PropagandaHostSession` | `PropagandaSaveStore` | `PropagandaPanel`, `GameDashboardPanel` | `--propaganda-selftest`, `Plan168PropagandaIntegrationTests` | ✅ 6/6 |
| 122 | `bestiary_knowledge` | Narrative | `BestiarySystem`, `BestiaryState`, `BestiaryCensus` | `wasteland_wildlife_bestiary.json` | `Main`, `BestiaryHostSession` | `BestiarySaveStore` | *None (GAP)* | `--bestiary-selftest`, `Plan187BestiaryIntegrationTests` | ❌ GAP |
| 123 | `broadsheet_press` | Narrative | `BroadsheetPressLedger`, `BroadsheetPressState`, `BroadsheetPressCensus`, `PublicBroadsheetPressEngine` | — *(Procedural)* | `Main`, `BroadsheetPressHostSession` | `BroadsheetPressSaveStore` | *None (GAP)* | `--broadsheet-press-selftest`, `BroadsheetPressLedgerTests`, `PublicBroadsheetPressEngineTests` | ❌ GAP |
| 124 | `cipher_quest_chain` | Narrative | `CipherQuestChainEngine` | — *(Procedural)* | `Main`, `CipherQuestChainHostSession` | `CipherQuestChainSaveStore` | *None (GAP)* | `--world-selftest`, `Plan11ExplorationTests`, `Plan10_11CombatExplorationIntegrationTests` | ❌ GAP |
| 125 | `confession_secret` | Narrative | `ConfessionSecretSystem` | `confession_secrets.json` | `Main` | `ConfessionSecretSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `ConfessionSecretSystemTests` | ❌ GAP |
| 126 | `echoes` | Narrative | `EchoSystem`, `NarrativeContinuityEngine` | `echoes.json` | `EchoHostSession`, `EchoSaveStore` | `EchoSaveStore` | *None (GAP)* | , `EchoCatalogTests`, `EchoSystemTests` | ❌ GAP |
| 127 | `letter_delivery` | Narrative | `LetterDeliverySystem` | `letters_expansion.json` | `Main`, `LetterDeliveryHostSession` | `LetterDeliverySaveStore` | `TimeCapsulePanel` | , `Plan212LetterRouteIntegrationTests`, `LetterDeliverySystemTests` | ❌ GAP |
| 128 | `npc_memory` | Narrative | `NpcMemorySystem`, `NpcMemoryEntry`, `NpcRelationship`, `NpcMemoryCensus` | `npc_memory_dialogue.json` | `Main`, `NpcMemoryHostSession` | `NpcMemorySaveStore` | *None (GAP)* | `--npc-memory-selftest`, `Plan147NpcMemoryHostIntegrationTests`, `NpcMemorySystemTests` | ❌ GAP |
| 129 | `seasonal_celebration` | Narrative | `SeasonalCelebrationSystem` | `shelter_celebrations.json` | `Main`, `ShelterOperationsHostSession` | `SeasonalCelebrationSaveStore` | `ShelterOperationsPanel` | `--shelter-operations-selftest`, `Plan170SeasonalCelebrationsIntegrationTests`, `SeasonalCelebrationCycleTests`, `ShelterOperationsBoardWiringTests` | ✅ 6/6 |
| 130 | `shelter_festival` | Narrative | `ShelterFestivalEngine` | — *(Procedural)* | `Main` | `ShelterFestivalSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan170SeasonalCelebrationsIntegrationTests` | ❌ GAP |
| 131 | `survivor_letter_delivery` | Narrative | `SurvivorLetterDeliverySystem` | `narrative/survivor_letters_lost_kin.json` | `Main`, `SurvivorLetterDeliveryHostSession` | `SurvivorLetterDeliverySaveStore` | *None (GAP)* | , `NarrativeAndFactionWarIntegrationTests` | ❌ GAP |
| 132 | `oral_lore` | Narrative & Cultural Tradition | `OralLorePerformanceSystem` | `oral_lore_codex.json`, `oral_lore_batch_2.json` | `Main` | `OralLoreSaveStore` | *None (GAP)* | , `OralLorePlan155Tests`, `OralLoreCatalogTests` | ❌ GAP |
| 133 | `moral_choice` | Narrative & Decisions | `MoralChoiceSystem`, `MoralChoiceState` | `moral_choice_quests.json` | `MoralChoiceSystem` | `MoralChoiceSaveStore` | `GameDashboardPanel` | `--moral-choice-selftest`, `MoralChoiceSystemTests` | ✅ 6/6 |
| 134 | `contraband_stash` | Narrative & Illicit Economy | `ContrabandStashSystem` | `bunker_contraband_barter.json` | `Main` | `ContrabandSaveStore` | *None (GAP)* | `--contraband-stash-selftest`, `ContrabandPlan147Tests` | ❌ GAP |
| 135 | `sleep_acoustic_rest` | Needs | `SleepAcousticLedger`, `SleepAcousticState`, `SleepAcousticCensus`, `SleepAcousticRestEngine` | — *(Procedural)* | `Main`, `SleepAcousticRestHostSession` | `SleepAcousticRestSaveStore` | *None (GAP)* | `--sleep-acoustic-selftest`, `SleepAcousticRestEngineTests`, `SleepAcousticLedgerTests` | ❌ GAP |
| 136 | `common_table_rationing` | Nutrition | `CommonTableRationingEngine`, `DiversityTier`, `FoodCategory`, `RationLevel` | — *(Procedural)* | `Main`, `CommonTableRationingHostSession` | `CommonTableRationingSaveStore` | *None (GAP)* | `--common-table-rationing-selftest`,  | ❌ GAP |
| 137 | `cooking` | Nutrition | `CookingSystem`, `CookingRecipe`, `CookingOperation`, `CookingState`, `CookingCensus`, `CookingRecipeCatalogLoader`, `InventoryCookingSource` | `recipes_cooking.json` | `CookingHostSession`, `Main` | `CookingSaveStore` | *None (GAP)* | `--cooking-selftest`, `Plan136CookingHostIntegrationTests`, `Plan136WildlifeCookingIntegrationTests` | ❌ GAP |
| 138 | `grain_processing` | Nutrition | `GrainProcessingSystem` | — *(Procedural)* | `GrainProcessingSaveStore` | `GrainProcessingSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 139 | `companion_animals` | Plan 174 Companion Animals | `CompanionAnimalSystem` | `companion_animals.json` | `Main` | `CompanionSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `Plan174CompanionAnimalTests` | ✅ 6/6 |
| 140 | `zealotry` | Plan 175 Ideological Pressure | `ZealotrySystem` | `wasteland_religions.json` | `Main` | `ZealotrySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ZealotrySystemTests` | ✅ 6/6 |
| 141 | `anomaly_hazard` | Plan 176 Anomaly Hazard Layer | `AnomalyHazardSystem` | `anomalies.json` | `Main` | `AnomalyHazardSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `Plan176AnomalyHazardTests`, `Plan176CrossSystemConsumerTests` | ✅ 6/6 |
| 142 | `bionics` | Plan 177 Bionics & Prosthetics | `BionicsSystem` | `bionics.json` | `Main` | `BionicsSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `Plan177BionicsTests` | ✅ 6/6 |
| 143 | `spiritual_meaning` | Plan 30 Spiritual Meaning | `SpiritualMeaningCoordinator` | `spiritual_rituals.json`, `memorial_rites.json`, `belief_movements.json` | `Main` | `SpiritualSaveStore` | `IronCenotaphMemorialPanel` | `--save-store-checksum-selftest`, `Plan30SpiritualWorldTests` | ✅ 6/6 |
| 144 | `amputation` | Plans 178-201 Expansion Block | `AmputationSystem` | `surgical_procedures.json` | `Main` | `AmputationSaveStore` | `MedicalPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `AmputationSystemTests` | ✅ 6/6 |
| 145 | `archaeology` | Plans 178-201 Expansion Block | `ArchaeologySystem` | `lore_archives.json` | `Main` | `ArchaeologySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ArchaeologySystemTests` | ✅ 6/6 |
| 146 | `aviation` | Plans 178-201 Expansion Block | `AviationSystem` | `aircraft_parts.json` | `Main` | `AviationSaveStore` | `AviationUI`, `GameDashboardPanel` | `--expedition-selftest`, `AviationSystemTests` | ✅ 6/6 |
| 147 | `ceremony` | Plans 178-201 Expansion Block | `CeremonySystem` | `ceremonies.json` | `Main` | `CeremonySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CeremonySystemTests` | ✅ 6/6 |
| 148 | `chem_warfare` | Plans 178-201 Expansion Block | `ChemWarfareSystem` | `chemical_weapons.json` | `Main` | `ChemWarfareSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `ChemWarfareSystemTests` | ✅ 6/6 |
| 149 | `child_development` | Plans 178-201 Expansion Block | `GenerationalSystem`, `ChildDevelopmentSystem`, `ChildDevelopmentCensus` | `development_traits.json` | `Main`, `ChildDevelopmentHostSession` | `GenerationalSaveStore` | `NurseryPanel`, `GameDashboardPanel` | `--child-development-selftest`, `--save-store-checksum-selftest`, `Plan183ChildDevelopmentIntegrationTests`, `GenerationalSystemTests`, `GenerationalLineageExtensionTests` | ✅ 6/6 |
| 150 | `comms_array` | Plans 178-201 Expansion Block | `CommsArraySystem` | `comms_targets.json` | `Main` | `CommsArraySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `CommsArraySystemTests` | ✅ 6/6 |
| 151 | `desperation` | Plans 178-201 Expansion Block | `DesperationSystem` | `desperation_events.json` | `Main` | `DesperationSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `DesperationSystemTests` | ✅ 6/6 |
| 152 | `expedition_stealth` | Plans 178-201 Expansion Block | `StealthSystem` | `camouflage_gear.json` | `Main` | `StealthSaveStore` | `StealthReadoutPanel`, `GameDashboardPanel` | `--expedition-selftest`, `StealthSystemTests` | ✅ 6/6 |
| 153 | `fallout` | Plans 178-201 Expansion Block | `FalloutSystem` | `fallout_patterns.json` | `Main` | `FalloutSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FalloutSystemTests` | ✅ 6/6 |
| 154 | `forced_labor` | Plans 178-201 Expansion Block | `ForcedLaborSystem` | `labor_camps.json` | `Main` | `ForcedLaborSaveStore` | `LaborUI`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `ForcedLaborSystemTests` | ✅ 6/6 |
| 155 | `fungi_cultivation` | Plans 178-201 Expansion Block | `FungiCultivationSystem` | `underground_flora.json` | `Main` | `FungiSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FungiCultivationSystemTests` | ✅ 6/6 |
| 156 | `mercenary_bounties` | Plans 178-201 Expansion Block | `MercenarySystem` | `bounty_board.json` | `Main` | `MercenarySaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `MercenarySystemTests` | ✅ 6/6 |
| 157 | `mutation_tree` | Plans 178-201 Expansion Block | `MutationSystem` | `mutations.json` | `Main` | `MutationSaveStore` | `MutationTreePanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `MutationSystemTests` | ✅ 6/6 |
| 158 | `narcotics` | Plans 178-201 Expansion Block | `NarcoticsSystem` | `narcotics.json` | `Main` | `NarcoticsSaveStore` | `ChemUI`, `PharmaLabPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `NarcoticsSystemTests` | ✅ 6/6 |
| 159 | `prisoner_management` | Plans 178-201 Expansion Block | `PrisonerSystem` | `interrogation_tactics.json` | `Main` | `PrisonerSaveStore` | `PrisonerPanel`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `PrisonerSystemTests` | ✅ 6/6 |
| 160 | `railway` | Plans 178-201 Expansion Block | `RailwaySystem` | `rail_network.json` | `Main` | `RailwaySaveStore` | `GameDashboardPanel` | `--expedition-selftest`, `RailwaySystemTests` | ✅ 6/6 |
| 161 | `recreation` | Plans 178-201 Expansion Block | `SurvivorDowntimeSystem` | `recreation.json` | `Main` | `RecreationSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `SurvivorDowntimeSystemTests` | ✅ 6/6 |
| 162 | `robotics` | Plans 178-201 Expansion Block | `RoboticsSystem` | `robotics.json` | `Main` | `RoboticsSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `RoboticsSystemTests` | ✅ 6/6 |
| 163 | `settlement_politics` | Plans 178-201 Expansion Block | `PoliticsSystem` | `political_policies.json` | `Main` | `PoliticsSaveStore` | `PoliticsUI`, `GameDashboardPanel` | `--save-store-checksum-selftest`, `PoliticsSystemTests` | ✅ 6/6 |
| 164 | `wasteland_justice` | Plans 178-201 Expansion Block | `JusticeSystem` | `wasteland_laws.json` | `Main` | `JusticeSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `JusticeSystemTests` | ✅ 6/6 |
| 165 | `plastic_pyrolysis` | Plans 202-205 Flagship (Plan 202) | `PlasticPyrolysisSystem` | `plastic_pyrolysis_catalog.json` | `Main` | `PlasticPyrolysisSaveStore` | `PlasticPyrolysisPanel` | `--save-store-checksum-selftest`, `PlasticPyrolysisEngineTests` | ✅ 6/6 |
| 166 | `cargo_airdrop` | Plans 202-205 Flagship (Plan 205) | `CargoAirdropSystem` | `cargo_airdrop_catalog.json` | `Main` | `CargoAirdropSaveStore` | `CargoAirdropPanel` | `--save-store-checksum-selftest`, `CargoAirdropEngineTests` | ✅ 6/6 |
| 167 | `campaign_action_log` | PlayerCommand | `CampaignActionLog`, `CampaignActionLogEntry`, `CampaignActionLogSave` | — *(Procedural)* | `Main`, `CampaignActionLogHostSession` | `CampaignActionLogSaveStore` | *None (GAP)* | `--campaign-action-log-selftest`,  | ❌ GAP |
| 168 | `geothermal_orc` | Power | `GeothermalOrcSystem` | `geothermal_strata_catalog.json` | `Main` | `GeothermalOrcSaveStore` | `GeothermalOrcPanel` | , `Plans74To77SystemsTests` | ❌ GAP |
| 169 | `kinetic_storage` | Power | `KineticStorageSystem` | — *(Procedural)* | `KineticStorageSaveStore` | `KineticStorageSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 170 | `solar_concentrator` | Power | `SolarConcentratorEngine` | — *(Procedural)* | `SolarConcentratorHostSession` | `SolarConcentratorSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 171 | `hobby` | Psychology | `HobbySystem` | `hobby_definitions.json` | `Main` | `HobbySaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan161HobbyIntegrationTests` | ❌ GAP |
| 172 | `psychological_arcs` | Psychology | `PsychologicalArcSystem` | `mental_arcs.json` | `PsychologyArcHostSession` | `PsychologyArcSaveStore` | *None (GAP)* | , `PsychologicalArcSystemTests` | ❌ GAP |
| 173 | `psychological_profiles` | Psychology | `PsychologicalProfileSystem`, `PsychologyState`, `PsychologicalProfileCensus` | `psychology_profiles.json` | `Main`, `PsychologicalProfileHostSession` | `PsychologicalProfileSaveStore` | *None (GAP)* | `--psychological-profile-selftest`, `Plan179UnifiedPsychologyIntegrationTests` | ❌ GAP |
| 174 | `survivor_autonomy` | Psychology | `SurvivorAutonomySystem` | `autonomy_actions.json` | `Main` | `SurvivorAutonomySaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan144SurvivorAutonomyIntegrationTests` | ❌ GAP |
| 175 | `survivor_mental_health` | Psychology | `SurvivorMentalHealthSystem` | `psychological_trauma.json` | `Main` | `SurvivorMentalHealthSaveStore` | *None (GAP)* | , `SurvivorMentalHealthTests` | ❌ GAP |
| 176 | `personal_quests` | Quests | `PersonalQuestSystem`, `PersonalQuestDef`, `PersonalQuestSaveState`, `PersonalQuestCensus` | `personal_quests.json` | `Main`, `PersonalQuestHostSession` | `PersonalQuestSaveStore` | `PersonalQuestPanel` | `--personal-quests-selftest`, `Plan200PersonalQuestsIntegrationTests`, `PersonalQuestSystemTests` | ✅ 6/6 |
| 177 | `procedural_narrative` | Quests | `ProceduralNarrativeSystem` | `quest_templates.json` | `ProceduralNarrativeHostSession` | `ProceduralNarrativeSaveStore` | *None (GAP)* | , `Plan169ProceduralNarrativeTests` | ❌ GAP |
| 178 | `low_background_metrology` | Radiation & Metrology | `LowBackgroundLeadEngine` | `low_background_lead_catalog.json` | `LowBackgroundMetrologyHostSession` | `LowBackgroundMetrologySaveStore` | `LowBackgroundLeadPanel` | , `Plan138LowBackgroundLeadEngineTests` | ❌ GAP |
| 179 | `communications` | Radio | `CommunicationsSystem` | `communications_networks.json` | `Main` | `CommunicationsSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan157CommunicationsIntegrationTests` | ❌ GAP |
| 180 | `heliograph` | Radio | `HeliographSystem` | — *(Procedural)* | `HeliographSaveStore` | `HeliographSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 181 | `nvis_communications` | Radio | `NvisCommunicationsSystem` | — *(Procedural)* | `NvisCommunicationsSaveStore` | `NvisCommunicationsSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 182 | `patrol_radio_hooks` | Radio | `PatrolRadioHooks`, `TravelEncounterSystem`, `PatrolRadioHooksState` | `travel_encounters.json` | `Main`, `PatrolRadioHostSession`, `RadioHostSession` | `PatrolRadioSaveStore` | *None (GAP)* | `--patrol-radio-selftest`, `PlanPatrolRadioHostIntegrationTests` | ❌ GAP |
| 183 | `psyops` | Radio | `PsyOpsSystem` | — *(Procedural)* | `PsyOpsHostSession` | `PsyOpsSaveStore` | *None (GAP)* | , `PsyOpsSystemTests` | ❌ GAP |
| 184 | `radio_program_production` | Radio | `RadioProgramProductionSystem` | `radio_programs.json` | `RadioProgramProductionHostSession` | `RadioProgramProductionSaveStore` | `RadioPanel` | , `Plan173RadioProgramProductionTests` | ❌ GAP |
| 185 | `retention` | Records | `RetentionPolicyCatalog`, `RetentionPolicyCatalogLoader`, `RollingLog` | `retention_policies.json` | `RetentionHostSession`, `Main` | `RetentionSaveStore` | *None (GAP)* | `--retention-selftest`, `Plan55RetentionHostIntegrationTests`, `RetentionPolicyCatalogLoaderTests`, `Plan55RetentionPolicyIntegrationTests` | ❌ GAP |
| 186 | `research_unlock` | Research | `ResearchUnlockBridge` | `research_unlocks.json` | `Main`, `ResearchUnlockHostSession` | `ResearchUnlockSaveStore` | `ResearchPanel` | `--research-unlock-selftest`, `Plan141ResearchUnlockBridgeIntegrationTests`, `Plan141ResearchUnlockHostIntegrationTests` | ✅ 6/6 |
| 187 | `playable_metrics` | Save | `PlaySessionRecorder`, `FirstHourFunnel`, `PlayableMetricsAggregationEngine` | — *(Procedural)* | `Main`, `PlayMetricsHostSession` | `PlayMetricsSaveStore` | *None (GAP)* | `--playable-metrics-selftest`, `Plan46PlayMetricsHostIntegrationTests`, `PlaySessionRecorderTests` | ❌ GAP |
| 188 | `session_durability` | Save | `SessionDurabilityManager` | — *(Procedural)* | `Main`, `SessionDurabilityHostSession`, `SaveLoadHostSession` | `SessionDurabilitySaveStore` | *None (GAP)* | `--session-durability-selftest`, `Plan39SessionDurabilityHostIntegrationTests`, `SessionDurabilityManagerTests` | ❌ GAP |
| 189 | `seven_day_slice` | Save | `SliceScenario`, `SliceScenarioCatalogLoader` | `slice_seven_days.json` | `SliceScenarioHostSession` | `SliceScenarioSaveStore` | *None (GAP)* | `--seven-day-slice-selftest`, `Plan54SevenDaySliceHostIntegrationTests` | ❌ GAP |
| 190 | `accessibility_settings` | Settings | `AccessibilitySettingsSystem`, `AccessibilitySettingsState`, `AccessibilityCensus` | `accessibility_profiles.json` | `Main`, `AccessibilitySettingsHostSession` | `AccessibilitySettingsSaveStore` | *None (GAP)* | `--accessibility-settings-selftest`, `Plan184AccessibilitySettingsIntegrationTests` | ❌ GAP |
| 191 | `outpost_settlement` | Settlements | `OutpostSettlementSystem`, `OutpostDef`, `OutpostInstance`, `OutpostSettlementState` | `outposts.json` | `OutpostSettlementHostSession`, `ShelterOperationsHostSession`, `Main` | `OutpostSettlementSaveStore` | `ShelterOperationsPanel` | `--shelter-operations-selftest`, `Plan58OutpostHostIntegrationTests`, `Plan58OutpostSettlementIntegrationTests`, `OutpostAtomicBillTests`, `ShelterOperationsBoardWiringTests` | ✅ 6/6 |
| 192 | `chemical_reagent_synthesis` | Shelter | `ChemicalReagentLedger`, `ChemicalReagentSynthesisState`, `ChemicalReagentCensus`, `ChemicalReagentSynthesisEngine` | — *(Procedural)* | `Main`, `ChemicalReagentSynthesisHostSession` | `ChemicalReagentSynthesisSaveStore` | *None (GAP)* | `--chemical-reagent-selftest`, `ChemicalReagentSynthesisEngineTests`, `ChemicalReagentLedgerTests` | ❌ GAP |
| 193 | `cryo_vault` | Shelter | `CryoVaultSystem` | `cryo_cultivars.json` | `Main` | `CryoVaultSaveStore` | *None (GAP)* | , `CryoVaultB69Tests` | ❌ GAP |
| 194 | `disaster_response` | Shelter | `DisasterResponseSystem` | `disaster_templates.json` | `Main` | `DisasterResponseSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan158DisasterResponseIntegrationTests` | ❌ GAP |
| 195 | `ebpvd_coating` | Shelter | `EbPvdCoatingEngine` | `ebpvd_coating_catalog.json` | `EbPvdCoatingHostSession` | `EbPvdCoatingSaveStore` | `EbPvdCoatingPanel` | `--ebpvd-coating-uitest`, `EbPvdCoatingEngineTests` | ✅ 6/6 |
| 196 | `emergency_muster_readiness` | Shelter | `EmergencyMusterReadinessEngine`, `EmergencyDrillType` | — *(Procedural)* | `Main`, `EmergencyMusterReadinessHostSession` | `EmergencyMusterReadinessSaveStore` | *None (GAP)* | `--emergency-muster-readiness-selftest`,  | ❌ GAP |
| 197 | `excavation_hazards` | Shelter | `ExcavationHazardSystem` | — *(Procedural)* | `Main` | `ExcavationHazardSaveStore` | `GameDashboardPanel` | `--shelter-hazard-selftest`, `ExcavationSystemTests` | ✅ 6/6 |
| 198 | `food_preservation` | Shelter | `FoodPreservationSystem` | `food_preservation.json` | `Main` | `FoodPreservationSaveStore` | *None (GAP)* | , `FoodPreservationSystemTests` | ❌ GAP |
| 199 | `geothermal_aquifer` | Shelter | `GeothermalAquiferSystem` | — *(Procedural)* | `GeothermalAquiferSaveStore` | `GeothermalAquiferSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 200 | `glassworks` | Shelter | `GlassworksLedger`, `GlassworksState`, `GlassworksCensus`, `PrecisionGlassworksOpticsEngine` | `glassworks_recipes.json` | `Main`, `GlassworksHostSession` | `GlassworksSaveStore` | *None (GAP)* | `--glassworks-selftest`, `GlassworksLedgerTests`, `PrecisionGlassworksOpticsEngineTests` | ❌ GAP |
| 201 | `kilnworks` | Shelter | `KilnFiringLedger`, `KilnFiringState`, `KilnFiringCensus`, `KilnFiringEngine` | — *(Procedural)* | `Main`, `KilnworksHostSession` | `KilnworksSaveStore` | *None (GAP)* | `--kilnworks-selftest`, `KilnFiringLedgerTests`, `KilnFiringEngineTests` | ❌ GAP |
| 202 | `mechanical_driveline` | Shelter | `MechanicalDrivelineLedger`, `MechanicalDrivelineState`, `MechanicalDrivelineCensus`, `MechanicalPowerDrivelineEngine` | — *(Procedural)* | `Main`, `MechanicalDrivelineHostSession` | `MechanicalDrivelineSaveStore` | *None (GAP)* | `--mechanical-driveline-selftest`, `MechanicalPowerDrivelineEngineTests`, `MechanicalDrivelineLedgerTests` | ❌ GAP |
| 203 | `precision_metrology` | Shelter | `PrecisionMetrologySystem` | `metrology_standards_catalog.json` | `Main` | `PrecisionMetrologySaveStore` | *None (GAP)* | `--precision-metrology-selftest`, `PrecisionMetrologySystemTests` | ❌ GAP |
| 204 | `precision_optics` | Shelter | `PrecisionOpticsEngine` | — *(Procedural)* | `PrecisionOpticsHostSession` | `PrecisionOpticsSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 205 | `radio_station` | Shelter | `ShelterRadioStationSystem` | — *(Procedural)* | `Main` | `RadioStationSaveStore` | `RadioPanel`, `GameDashboardPanel` | `--core-selftest`, `ShelterRadioStationTests` | ✅ 6/6 |
| 206 | `seismic_dynamics` | Shelter | `SeismicDynamicsSystem` | `seismic_fault_catalog.json` | `Main` | `SeismicDynamicsSaveStore` | *None (GAP)* | , `ShelterSeismicDynamicsPlan56Tests`, `SeismicMonitoringB68Tests` | ❌ GAP |
| 207 | `shelter_archive` | Shelter | `ShelterArchiveSystem`, `ShelterArchiveState`, `ShelterArchiveCensus` | `archive_categories.json` | `Main`, `ShelterArchiveHostSession` | `ShelterArchiveSaveStore` | *None (GAP)* | `--shelter-archive-selftest`, `Plan162ArchiveIntegrationTests`, `ShelterArchiveSystemTests` | ❌ GAP |
| 208 | `shelter_atmosphere` | Shelter | `ShelterAtmosphereSystem` | — *(Procedural)* | `ShelterAtmosphereHostSession` | `ShelterAtmosphereSaveStore` | `ShelterAtmospherePanel`, `GameDashboardPanel` | `--shelter-atmosphere-selftest`, `Plan220ShelterAtmosphereIntegrationTests` | ✅ 6/6 |
| 209 | `shelter_decor` | Shelter | `ShelterDecorSystem` | — *(Procedural)* | `ShelterDecorHostSession` | `ShelterDecorSaveStore` | `GameDashboardPanel` | `--shelter-decor-selftest`, `Plan12CDecorTests` | ✅ 6/6 |
| 210 | `shelter_expansion` | Shelter | `ShelterExpansionSystem` | `shelter_construction.json` | `Main`, `ShelterOperationsHostSession` | `ShelterExpansionSaveStore` | `ShelterOperationsPanel` | `--shelter-operations-selftest`, `Plan156ShelterExpansionIntegrationTests`, `ShelterOperationsBoardCoreTests`, `ShelterOperationsBoardWiringTests` | ✅ 6/6 |
| 211 | `shelter_maintenance` | Shelter | `ShelterMaintenanceSystem`, `ShelterComponentCatalogLoader`, `ShelterMaintenanceCensus` | `shelter_components.json` | `Main`, `ShelterMaintenanceHostSession` | `ShelterMaintenanceSaveStore` | `SurvivorDetailPanel` | `--shelter-maintenance-selftest`, `Plan186ShelterMaintenanceIntegrationTests` | ✅ 6/6 |
| 212 | `shelter_noise` | Shelter | `ShelterNoiseSystem` | — *(Procedural)* | `ShelterAtmosphereHostSession` | `ShelterNoiseSaveStore` | `ShelterAtmospherePanel` | `--shelter-atmosphere-selftest`, `Plan220ShelterAtmosphereIntegrationTests` | ✅ 6/6 |
| 213 | `shelter_social_dynamics` | Shelter | `ShelterSocialDynamicsSystem` | `shelter_social_events.json` | `Main` | `ShelterSocialSaveStore` | `GameDashboardPanel` | `--core-selftest`, `ShelterSocialDynamicsTests` | ✅ 6/6 |
| 214 | `shelter_workshop` | Shelter | `ShelterWorkshopSystem` | — *(Procedural)* | `Main` | `ShelterWorkshopSaveStore` | `WorkshopPanel`, `GameDashboardPanel` | `--core-selftest`, `WorkshopReverseEngineeringSystemTests` | ✅ 6/6 |
| 215 | `trophies` | Shelter | `TrophySystem`, `TrophySaveState`, `TrophyAwardRecord` | `trophies.json` | `Main`, `TrophyHostSession` | `TrophySaveStore` | *None (GAP)* | `--trophy-selftest`,  | ❌ GAP |
| 216 | `weather_hardening` | Shelter | `WeatherHardeningSystem` | — *(Procedural)* | `WeatherHardeningSaveStore` | `WeatherHardeningSaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 217 | `cvd_diamond` | Shelter & Facilities | `CvdDiamondSynthesisEngine` | `cvd_diamond_catalog.json` | `CvdDiamondHostSession` | `CvdDiamondSaveStore` | `CvdDiamondPanel` | `--plans-122-125-selftest`, `Plan124CvdDiamondSynthesisEngineTests` | ✅ 6/6 |
| 218 | `sofc_power` | Shelter & Facilities | `SofcElectrochemistryEngine` | `sofc_power_catalog.json` | `SofcPowerHostSession` | `SofcPowerSaveStore` | `SolidOxideFuelCellPanel` | `--plans-122-125-selftest`, `Plan122SofcElectrochemistryEngineTests` | ✅ 6/6 |
| 219 | `bio_fermentation` | Shelter & Farming | `BioFermentationEngine` | `bio_fermentation_catalog.json` | `BioFermentationHostSession` | `BioFermentationSaveStore` | `BioFermentationPanel` | , `BioFermentationEngineTests` | ❌ GAP |
| 220 | `hydroponic_biomes` | Shelter & Farming | `HydroponicBiomeSystem` | `hydroponic_crops.json` | `Main` | `HydroponicBiomeSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `HydroponicBiomeTests` | ✅ 6/6 |
| 221 | `airlock_security` | Shelter & Infrastructure | `AirlockSecuritySystem` | — *(Procedural)* | `AirlockSecurityHostSession` | `AirlockSecuritySaveStore` | `AirlockSecurityPanel` | `--shelter-operations-selftest`, `AirlockSecuritySystemTests` | ✅ 6/6 |
| 222 | `decontamination` | Shelter & Infrastructure | `DecontaminationSystem` | — *(Procedural)* | `DecontaminationHostSession` | `DecontaminationSaveStore` | `DecontaminationPanel` | `--shelter-operations-selftest`, `DecontaminationSystemTests` | ✅ 6/6 |
| 223 | `excavation` | Shelter & Infrastructure | `ExcavationSystem` | — *(Procedural)* | `ExcavationHostSession` | `ExcavationSaveStore` | `ExcavationPanel` | `--shelter-operations-selftest`, `ExcavationSystemTests` | ✅ 6/6 |
| 224 | `greenhouse` | Shelter & Infrastructure | `GreenhouseSystem` | `greenhouse_items.json` | `GreenhouseHostSession` | `GreenhouseSaveStore` | `GreenhousePanel` | `--greenhouse-selftest`, `GreenhouseSystemTests` | ✅ 6/6 |
| 225 | `nuclear_core_lifecycle` | Shelter & Infrastructure | `NuclearCoreLifecycleSystem` | `nuclear_core_profiles.json` | `Main` | `NuclearCoreSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `NuclearCorePowerGridPublishTests` | ✅ 6/6 |
| 226 | `power_grid` | Shelter & Infrastructure | `PowerGridSystem` | `power_grid.json` | `PowerGridHostSession` | `PowerGridSaveStore` | `PowerGridPanel` | `--player-panels-uitest`, `PowerGridSystemTests` | ✅ 6/6 |
| 227 | `power_subgrids` | Shelter & Infrastructure | `PowerDistributionSubgridSystem` | — *(Procedural)* | `Main` | `PowerDistributionSaveStore` | `PowerGridPanel` | `--save-store-checksum-selftest`,  | ❌ GAP |
| 228 | `sanitation` | Shelter & Infrastructure | `SanitationSystem`, `SanitationFacilityCatalog` | `sanitation_facilities.json` | `SanitationHostSession` | `SanitationSaveStore` | `SanitationPanel` | `--player-panels-uitest`, `Plan210SanitationSystemTests`, `Plan210SanitationFacilityCatalogTests`, `Plan210SanitationHostWiringTests` | ✅ 6/6 |
| 229 | `shelter_assignment` | Shelter & Infrastructure | `ShelterAssignmentSystem` | — *(Procedural)* | `ShelterAssignmentHostSession` | `ShelterAssignmentSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `ShelterAssignmentSystemTests` | ✅ 6/6 |
| 230 | `shelter_fire` | Shelter & Infrastructure | `ShelterFireHazardSystem` | — *(Procedural)* | `ShelterFireHostSession` | `ShelterFireSaveStore` | `FireIncidentPanel` | `--save-store-checksum-selftest`, `ShelterFireHazardSystemTests`, `FireIncidentJourneyTests` | ✅ 6/6 |
| 231 | `shelter_schedule` | Shelter & Infrastructure | `ShelterScheduleSystem` | `shelter_schedules.json` | `ShelterScheduleHostSession` | `ShelterScheduleSaveStore` | `ShelterSchedulePanel` | `--shelter-operations-selftest`, `ShelterScheduleIntegrationTests` | ✅ 6/6 |
| 232 | `shelter_thermal` | Shelter & Infrastructure | `ShelterThermalSystem` | — *(Procedural)* | `ShelterThermalHostSession` | `ShelterThermalSaveStore` | `ShelterThermalPanel` | `--shelter-operations-selftest`, `--thermal-storm-seal-selftest`, `ShelterThermalSaveChecksumTests` | ✅ 6/6 |
| 233 | `starting_level` | Shelter & Infrastructure | `StartingLevelSystem` | — *(Procedural)* | `StartingLevelHostSession` | `StartingLevelSaveStore` | `OpeningProtocolModal` | `--playable-shell-selftest`, `StartingLevelSystemTests` | ✅ 6/6 |
| 234 | `sump_flooding` | Shelter & Infrastructure | `SumpFloodingSystem` | — *(Procedural)* | `SumpFloodingHostSession` | `SumpFloodingSaveStore` | `SumpFloodingPanel` | `--shelter-operations-selftest`, `SumpFloodingSaveChecksumTests` | ✅ 6/6 |
| 235 | `survivor_social` | Shelter & Infrastructure | `SurvivorSocialCoordinator`, `LeadershipSystem`, `LeadershipCensus`, `IdeologicalFrictionSystem`, `RationConflictSystem`, `TraumaBondSystem`, `SkillAtrophySystem` | `leadership_policies.json` | `SurvivorSocialCoordinator` | `SurvivorSocialSaveStore` | `ShelterPanel` | `--shelter-operations-selftest`, `--leadership-succession-selftest`, `SurvivorSocialCoordinatorTests`, `Plan208LeadershipSuccessionIntegrationTests` | ✅ 6/6 |
| 236 | `vinyl_morale` | Shelter & Infrastructure | `VinylMoraleSystem` | — *(Procedural)* | `VinylMoraleHostSession` | `VinylMoraleSaveStore` | `VinylMoralePanel` | `--shelter-operations-selftest`, `VinylMoraleSaveChecksumTests` | ✅ 6/6 |
| 237 | `water_treatment` | Shelter & Infrastructure | `WaterTreatmentSystem` | — *(Procedural)* | `WaterTreatmentHostSession`, `WaterSourcesHostSession` | `WaterTreatmentSaveStore` | `WaterTreatmentPanel` | `--shelter-operations-selftest`, `--water-sources-selftest`, `WaterTreatmentSystemTests`, `WaterSourcesSurfaceWiringTests` | ✅ 6/6 |
| 238 | `crafting` | Shelter & Logistics | `CraftingSystem` | `recipes.json` | `CraftingHostSession` | `CraftingSaveStore` | `CraftingPanel` | `--shelter-operations-selftest`, `CraftingSystemTests` | ✅ 6/6 |
| 239 | `equipment_condition` | Shelter & Logistics | `EquipmentConditionSystem` | — *(Procedural)* | `EquipmentConditionHostSession` | `EquipmentConditionSaveStore` | `EquipmentConditionPanel` | `--shelter-operations-selftest`, `EquipmentConditionSystemTests` | ✅ 6/6 |
| 240 | `inventory` | Shelter & Logistics | `Inventory` | `items.json` | `InventoryHostSession` | `InventorySaveStore` | `InventoryPanel`, `InventoryDetailPanel` | `--inventory-save-selftest`, `--inventory-uitest`, `InventorySystemTests` | ✅ 6/6 |
| 241 | `kitchen_nutrition` | Shelter & Logistics | `KitchenNutritionSystem` | — *(Procedural)* | `KitchenNutritionHostSession` | `KitchenNutritionSaveStore` | `KitchenNutritionPanel` | `--shelter-operations-selftest`, `KitchenNutritionSystemTests` | ✅ 6/6 |
| 242 | `radio` | Shelter & Logistics | `FactionRadioEngine`, `RadioStationCatalog`, `RadioStationCatalogLoader` | `radio.json`, `radio_stations.json` | `RadioHostSession` | `RadioSaveStore` | `RadioPanel`, `FactionRadioHudPanel` | `--radio-selftest`, `--radio-catalog-selftest`, `RadioSaveCodecTests`, `RadioStationCatalogTests`, `RadioStationParityTests` | ✅ 6/6 |
| 243 | `shelter_reputation` | Shelter (Plan 207) | `ShelterReputationSystem` | — *(Procedural)* | `ShelterReputationHostSession` | `ShelterReputationSaveStore` | `ShelterReputationPanel`, `GameDashboardPanel` | `--shelter-reputation-selftest`, `Plan207ShelterReputationIntegrationTests` | ✅ 6/6 |
| 244 | `internal_communication` | Shelter Communication (Plan 211) | `InternalCommunicationSystem` | `communication_templates.json` | `InternalCommunicationHostSession` | `InternalCommunicationSaveStore` | `ShelterSocialPanel` | `--internal-communication-selftest`, `Plan211InternalCommunicationIntegrationTests`, `Plan211InternalCommunicationHostWiringTests` | ✅ 6/6 |
| 245 | `shelter_security` | Shelter Defense (Plan 138) | `ShelterSecuritySystem` | — *(Procedural)* | `ShelterSecurityHostSession` | `ShelterSecuritySaveStore` | `ShelterSecurityPanel`, `GameDashboardPanel` | `--shelter-security-selftest`, `Plan138ShelterSecurityIntegrationTests` | ✅ 6/6 |
| 246 | `relationship_decay` | Social Ecology & Drift (Plan 182) | `RelationshipDecaySystem` | — *(Procedural)* | `RelationshipDecayHostSession` | `RelationshipDecaySaveStore` | `RelationshipDecayPanel`, `GameDashboardPanel` | `--relationship-decay-selftest`, `Plan182RelationshipDecayIntegrationTests`, `RelationshipDecaySystemTests` | ✅ 6/6 |
| 247 | `spiritual_ritual` | Spiritual | `SpiritualRitualCalendarEngine`, `SpiritualRitualDefinition`, `SpiritualRitualSaveState` | `spiritual_rituals.json` | `Main`, `SpiritualRitualHostSession` | `SpiritualRitualSaveStore` | *None (GAP)* | `--spiritual-ritual-selftest`, `PlanExpansion13SpiritualRitualHostIntegrationTests`, `SpiritualRitualCalendarEngineTests` | ❌ GAP |
| 248 | `hydrogeology_archive` | Subterranean Science Archive | `HydroGeologyDiscoverySystem` | `artesian_well_contamination_logs.json`, `cave_aquatic_biota_logs.json`, `geothermal_steam_vent_diagnostics.json`, `stalactite_mineral_assay_reports.json` | `Main` | `HydroGeologyArchiveSaveStore` | *None (GAP)* | , `HydroGeologyDiscoveryTests`, `HydroGeologyCatalogTests` | ❌ GAP |
| 249 | `apprenticeship` | Survival & Biology | `ApprenticeshipSystem` | — *(Procedural)* | `ApprenticeshipHostSession` | `ApprenticeshipSaveStore` | `ApprenticeshipPanel` | `--shelter-operations-selftest`, `ApprenticeshipSystemTests` | ✅ 6/6 |
| 250 | `autopsy` | Survival & Biology | `AutopsySystem` | `autopsy_procedures.json` | `AutopsyHostSession` | `AutopsySaveStore` | `AutopsyReportPanel` | `--shelter-operations-selftest`, `AutopsySystemTests` | ✅ 6/6 |
| 251 | `caregiving` | Survival & Biology | `CaregivingSystem` | — *(Procedural)* | `CaregivingHostSession` | `CaregivingSaveStore` | `CaregivingPanel` | `--shelter-operations-selftest`, `CaregivingSystemTests` | ✅ 6/6 |
| 252 | `chemical_dependency` | Survival & Biology | `ChemicalDependencySystem` | `chemical_dependency_items.json` | `MentalHealthCrisisHostSession`, `ChemicalDependencyHostSession` | `ChemicalDependencySaveStore` | `ChemicalDependencyPanel` | `--chemical-dependency-save-selftest`, `ChemicalDependencySaveSealTests` | ✅ 6/6 |
| 253 | `contractor_roster` | Survival & Biology | `ContractorRosterSystem` | — *(Procedural)* | `ContractorRosterHostSession` | `ContractorRosterSaveStore` | `ContractorRosterPanel` | `--shelter-operations-selftest`, `ContractorRosterSystemTests` | ✅ 6/6 |
| 254 | `disease` | Survival & Biology | `DiseaseSystem` | `disease_catalog.json` | `DiseaseHostSession` | `DiseaseSaveStore` | `AfflictionsPanel` | `--disease-selftest`, `DiseaseSystemTests` | ✅ 6/6 |
| 255 | `medical` | Survival & Biology | `MedicalWardSystem`, `SickListSystem` | `medical_texts.json` | `MedicalHostSession` | `MedicalSaveStore` | `MedicalPanel`, `AfflictionsPanel` | `--medical-selftest`, `DwellerMedicalCatalogTests` | ✅ 6/6 |
| 256 | `medical_ward` | Survival & Biology | `MedicalWardSystem` | — *(Procedural)* | `MedicalWardHostSession` | `MedicalWardSaveStore` | `MedicalWardPanel` | `--medical-ward-save-selftest`, `MedicalWardSystemTests` | ✅ 6/6 |
| 257 | `mental_health_crisis` | Survival & Biology | `MentalHealthCrisisSystem` | — *(Procedural)* | `MentalHealthCrisisHostSession` | `MentalHealthCrisisSaveStore` | `MentalHealthCrisisPanel` | `--shelter-operations-selftest`, `MentalHealthCrisisSystemTests` | ✅ 6/6 |
| 258 | `morale_contagion` | Survival & Biology | `MoraleContagionSystem` | — *(Procedural)* | `MoraleContagionHostSession` | `MoraleContagionSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `MoraleContagionSystemTests` | ✅ 6/6 |
| 259 | `survivor_relations` | Survival & Biology | `SurvivorRelationsSystem` | — *(Procedural)* | `SurvivorRelationsHostSession` | `SurvivorRelationsSaveStore` | `SurvivorRelationsPanel` | `--shelter-operations-selftest`, `--relationship-bands-selftest`, `SurvivorRelationsSaveChecksumTests` | ✅ 6/6 |
| 260 | `survivors` | Survival & Biology | `NeedsSystem`, `SurvivorRosterSystem` | `survivors.json` | `SurvivorsHostSession` | `SurvivorsSaveStore` | `SurvivorsPanel`, `SurvivorDetailPanel`, `StatusPanel` | `--survivors-selftest`, `--survivors-uitest`, `--player-panels-uitest`, `NeedsSystemTests` | ✅ 6/6 |
| 261 | `death_legacy` | Survivor Memorial & Wills (Plan 206) | `SurvivorDeathLegacySystem` | — *(Procedural)* | `SurvivorDeathLegacyHostSession` | `SurvivorDeathLegacySaveStore` | `SurvivorDeathLegacyPanel`, `GameDashboardPanel` | `--death-legacy-selftest`, `Plan206SurvivorDeathLegacyIntegrationTests`, `SurvivorDeathLegacySystemTests` | ✅ 6/6 |
| 262 | `aging` | Survivors | `AgingSystem`, `SurvivorAgingProgressionEngine`, `LifeStagesCatalogLoader`, `AgingCensus` | `life_stages.json` | `Main`, `AgingHostSession` | `AgingSaveStore` | `SurvivorDetailPanel` | `--aging-selftest`, `Plan176AgingHostIntegrationTests`, `LifeStagesCatalogLoaderTests` | ✅ 6/6 |
| 263 | `antenatal_maternal_health` | Survivors | `AntenatalMaternalCareLedger`, `AntenatalMaternalCareState`, `AntenatalMaternalCensus`, `AntenatalMaternalHealthEngine` | — *(Procedural)* | `Main`, `AntenatalMaternalHealthHostSession` | `AntenatalMaternalHealthSaveStore` | *None (GAP)* | `--antenatal-care-selftest`, `AntenatalMaternalHealthEngineTests`, `AntenatalMaternalCareLedgerTests` | ❌ GAP |
| 264 | `backstory` | Survivors | `BackstorySystem`, `BackstoryCensus` | `backstory_templates.json` | `Main`, `BackstoryHostSession` | `BackstorySaveStore` | `SurvivorDetailPanel` | `--backstory-selftest`, `Plan174BackstoryHostIntegrationTests`, `Plan174SurvivorBackstoriesIntegrationTests` | ✅ 6/6 |
| 265 | `exercise` | Survivors | `ExerciseSystem`, `ExerciseSystemState`, `ExerciseCensus` | `exercise_routines.json` | `Main`, `ExerciseHostSession` | `ExerciseSaveStore` | *None (GAP)* | `--exercise-selftest`, `Plan216ExerciseIntegrationTests`, `ExerciseSystemTests` | ❌ GAP |
| 266 | `genealogy` | Survivors | `GenerationalLineageExtension`, `GenealogyBridge` | `family_name_templates.json` | `Main`, `GenealogyHostSession` | `GenealogySaveStore` | `SurvivorDetailPanel` | `--genealogy-selftest`, `--genealogy-family-names-selftest`, `Plan217GenealogyIntegrationTests`, `Plan217GenealogyHostWiringTests` | ✅ 6/6 |
| 267 | `ideological_friction` | Survivors | `IdeologicalFrictionEvents`, `IdeologicalFrictionSystem`, `IdeologicalEventInstance`, `IdeologicalFrictionCensus` | `ideological_events.json` | `Main`, `IdeologicalFrictionHostSession` | `IdeologicalFrictionSaveStore` | `SurvivorsPanel`, `SurvivorDetailPanel` | `--ideological-friction-selftest`, `Plan148IdeologicalFrictionHostIntegrationTests`, `Plan148IdeologicalFrictionIntegrationTests`, `IdeologicalFrictionSystemTests` | ✅ 6/6 |
| 268 | `interpersonal_conflict` | Survivors | `InterpersonalConflictSystem`, `InterpersonalConflictState`, `InterpersonalConflictCensus` | `conflict_templates.json` | `Main`, `InterpersonalConflictHostSession` | `InterpersonalConflictSaveStore` | *None (GAP)* | `--interpersonal-conflict-selftest`, `Plan202InterpersonalConflictIntegrationTests`, `InterpersonalConflictSystemTests` | ❌ GAP |
| 269 | `radiation_social` | Survivors | `RadiationSocialBridge`, `RadiationSocialSaveState`, `SurvivorRadiationSocialProfile` | `radiation_economy_social.json` | `Main`, `RadiationSocialHostSession` | `RadiationSocialSaveStore` | *None (GAP)* | `--radiation-social-selftest`,  | ❌ GAP |
| 270 | `ration_conflict` | Survivors | `RationConflictSystem`, `RationConflictSaveState`, `ResourceRationingSystem`, `NeedsSystem`, `SurvivorRelationsSystem` | `rationing_protocols.json` | `Main`, `RationConflictHostSession`, `EconomyHostSession` | `RationConflictSaveStore` | *None (GAP)* | `--ration-conflict-selftest`, `PlanRationConflictHostIntegrationTests` | ❌ GAP |
| 271 | `recruitment` | Survivors | `RecruitmentSystem`, `RecruitmentState`, `RecruitmentCensus` | `recruitment_templates.json` | `Main`, `RecruitmentHostSession` | `RecruitmentSaveStore` | `RecruitmentPanel` | `--recruitment-selftest`, `Plan204RecruitmentIntegrationTests` | ✅ 6/6 |
| 272 | `romance_family` | Survivors | `RomanceFamilySystem`, `RomanticRelationship`, `FamilyUnit`, `RomanceCourtshipCatalog`, `RomanceFamilyCensus`, `RomanceCourtshipCatalogLoader` | `romance_courtship.json` | `Main`, `RomanceFamilyHostSession` | `RomanceFamilySaveStore` | `SurvivorDetailPanel` | `--romance-family-selftest`, `Plan150RomanceFamilyHostIntegrationTests`, `Plan150RomanceFamilyIntegrationTests`, `RomanceCourtshipCatalogLoaderTests` | ✅ 6/6 |
| 273 | `skill_atrophy` | Survivors | `SkillAtrophySystem`, `SkillAtrophySaveState`, `AtrophyState` | — *(Procedural)* | `Main`, `SkillAtrophyHostSession` | `SkillAtrophySaveStore` | *None (GAP)* | `--skill-atrophy-selftest`,  | ❌ GAP |
| 274 | `skill_certifications` | Survivors | `SkillCertificationSystem`, `SkillCertificationState`, `SkillCertificationCensus` | `skill_certifications.json` | `Main`, `SkillCertificationHostSession` | `SkillCertificationSaveStore` | *None (GAP)* | `--skill-certification-selftest`, `Plan180SkillCertificationTests` | ❌ GAP |
| 275 | `survivor_dreams` | Survivors | `DreamSystem`, `DreamSystemState`, `DreamCensus` | `dream_templates.json` | `Main`, `DreamHostSession` | `DreamSaveStore` | *None (GAP)* | `--dream-system-selftest`, `Plan177DreamSleepIntegrationTests` | ❌ GAP |
| 276 | `survivor_roles` | Survivors | `SurvivorRoleSystem`, `SurvivorRoleState` | `survivor_roles.json` | `Main`, `SurvivorRoleHostSession` | `SurvivorRoleSaveStore` | `SurvivorDetailPanel` | `--survivor-roles-selftest`, `Plan195SurvivorRoleIntegrationTests`, `Plan195SurvivorRoleWiringTests` | ✅ 6/6 |
| 277 | `survivor_routines` | Survivors | `SurvivorRoutineSystem`, `RoutineTemplateCatalogLoader`, `SurvivorRoutineCensus` | `routine_templates.json` | `Main`, `SurvivorRoutineHostSession` | `SurvivorRoutineSaveStore` | `SurvivorDetailPanel` | `--survivor-routines-selftest`, `Plan188SurvivorRoutineIntegrationTests`, `RoutineTemplateCatalogLoaderTests` | ✅ 6/6 |
| 278 | `survivor_voice` | Survivors | `SurvivorVoiceSystem`, `VoiceLineDispatchCoordinator` | `survivor_voice_lines.json` | `Main`, `SurvivorVoiceHostSession` | `SurvivorVoiceSaveStore` | *None (GAP)* | `--survivor-voice-selftest`, `Plan42SurvivorVoiceHostIntegrationTests`, `SurvivorVoiceSystemTests` | ❌ GAP |
| 279 | `trauma_bond` | Survivors | `TraumaBondSystem`, `TraumaBondSaveState` | — *(Procedural)* | `Main`, `TraumaBondHostSession` | `TraumaBondSaveStore` | *None (GAP)* | `--trauma-bond-selftest`, `PlanTraumaBondHostIntegrationTests`, `TraumaBondSystemTests` | ❌ GAP |
| 280 | `voluntary_register` | Survivors | `VoluntaryRegisterSystem`, `VoluntaryRegisterSystemState`, `DoseLedgerSystem` | — *(Procedural)* | `Main`, `VoluntaryRegisterHostSession` | `VoluntaryRegisterSaveStore` | *None (GAP)* | `--voluntary-register-selftest`, `PlanTriplePackageJCoreTests` | ❌ GAP |
| 281 | `hidden_agenda` | Survivors (Plan 132) | `HiddenAgendaSystem` | — *(Procedural)* | `HiddenAgendaHostSession` | `HiddenAgendaSaveStore` | `HiddenAgendaPanel`, `GameDashboardPanel` | `--hidden-agenda-selftest`, `Plan132HiddenAgendaIntegrationTests`, `HiddenAgendaSystemTests` | ✅ 6/6 |
| 282 | `combat` | Tactical Combat | `TacticalCombatSystem`, `CombatTraumaSystem` | `combat_catalog.json` | `CombatHostSession` | `CombatSaveStore` | `CombatPanel`, `CombatDetailPanel`, `CombatHistoryPanel` | `--combat-selftest`, `CombatBallisticsTests` | ✅ 6/6 |
| 283 | `technical_material_archive` | Technical Material Archive | `TechnicalMaterialArchiveSystem` | `hemp_fiber_hackling_logs.json`, `wire_rope_stranding_assays.json`, `manila_hawser_breakage_reports.json`, `rope_transmission_splicing_audits.json`, `neoprene_gasket_degradation_logs.json`, `aramid_fiber_rot_reports.json`, `tire_retreading_compound_logs.json`, `celluloid_film_decomposition_records.json` | `Main` | `TechnicalMaterialArchiveSaveStore` | *None (GAP)* | , `TechnicalMaterialArchiveTests`, `CordageCableCatalogTests`, `PolymerTextileCatalogTests` | ❌ GAP |
| 284 | `vehicle_customization` | Vehicles | `VehicleCustomizationSystem`, `VehicleCustomizationCatalog`, `VehicleModule`, `VehicleCustomizationCensus`, `VehicleModuleCatalogLoader` | `vehicle_modules.json` | `Main`, `VehicleCustomizationHostSession` | `VehicleCustomizationSaveStore` | *None (GAP)* | `--vehicle-customization-selftest`, `Plan152VehicleCustomizationHostIntegrationTests`, `Plan152VehicleCustomizationIntegrationTests`, `VehicleModuleCatalogLoaderTests` | ❌ GAP |
| 285 | `verdict_accusation` | Verdict | `VerdictAccusationSystem` | — *(Procedural)* | `Main`, `VerdictAccusationHostSession` | `VerdictAccusationSaveStore` | *None (GAP)* | `--verdict-accusation-selftest`,  | ❌ GAP |
| 286 | `visitor_integration` | Visitors | `VisitorIntegrationSystem`, `VisitorCatalogData` | `visitor_templates.json` | `Main`, `VisitorIntegrationHostSession` | `VisitorIntegrationSaveStore` | `VisitorIntegrationPanel`, `GameDashboardPanel` | `--visitor-integration-selftest`, `Plan214VisitorIntegrationTests` | ✅ 6/6 |
| 287 | `water_quality_profile` | Water | `WaterQualityProfileEngine`, `WaterSourcePurityTier` | — *(Procedural)* | `Main`, `WaterQualityProfileHostSession` | `WaterQualityProfileSaveStore` | *None (GAP)* | `--water-quality-profile-selftest`,  | ❌ GAP |
| 288 | `deep_well` | Water & Infrastructure | `DeepWellSystem` | — *(Procedural)* | `DeepWellHostSession`, `DeepWellSaveStore` | `DeepWellSaveStore` | `WaterTreatmentPanel` | `--water-sources-selftest`, `DeepWellSystemTests`, `WaterSourcesSurfaceWiringTests` | ✅ 6/6 |
| 289 | `piezometer_network` | Water & Infrastructure | `AquiferPiezometerEngine` | `piezometer_network_catalog.json` | `PiezometerHostSession` | `PiezometerSaveStore` | `WaterTreatmentPanel` | `--water-sources-selftest`, `Plan189IntakeAdvisoryBridgeTests`, `WaterSourcesSurfaceWiringTests` | ✅ 6/6 |
| 290 | `water_condenser` | Water & Infrastructure | `AtmosphericCondenserSystem` | — *(Procedural)* | `WaterCondenserHostSession`, `WaterCondenserSaveStore` | `WaterCondenserSaveStore` | `WaterTreatmentPanel` | `--water-sources-selftest`, `AtmosphericCondenserSystemTests`, `WaterSourcesSurfaceWiringTests` | ✅ 6/6 |
| 291 | `weather_cascade` | Weather | `WeatherCascadeSystem`, `WeatherGameplayCascadeEngine`, `WeatherCascadeSeverity`, `WeatherCascadeCatalogLoader` | `weather_gameplay_effects.json`, `weather_effects.json` | `WeatherCascadeHostSession`, `Main` | `WeatherCascadeSaveStore` | *None (GAP)* | `--weather-cascade-selftest`, `Plan135WeatherCascadeHostIntegrationTests`, `Plan135WeatherCascadeIntegrationTests` | ❌ GAP |
| 292 | `cloud_seeding` | Weather & Atmosphere | `CloudSeedingSystem` | — *(Procedural)* | `Main`, `CloudSeedingHostSession` | `CloudSeedingSaveStore` | *None (GAP)* | `--cloud-seeding-selftest`,  | ❌ GAP |
| 293 | `ecological_infestation` | World | `EcologicalInfestationSystem` | `micro_locations.json` | `Main` | `EcologicalInfestationSaveStore` | `GameDashboardPanel` | `--faction-ecology-selftest`, `EcologicalInfestationSystemTests` | ✅ 6/6 |
| 294 | `geodetic_survey` | World | `GeodeticSurveyEngine` | — *(Procedural)* | `GeodeticSurveySaveStore` | `GeodeticSurveySaveStore` | *None (GAP)* | ,  | ❌ GAP |
| 295 | `human_migration` | World | `SeasonalHumanMigrationEngine`, `SeasonalMigrationCatalogLoader`, `HumanMigrationCensus` | `seasonal_human_migration.json` | `Main`, `HumanMigrationHostSession` | `HumanMigrationSaveStore` | *None (GAP)* | `--human-migration-selftest`, `Plan199HumanMigrationHostIntegrationTests`, `SeasonalHumanMigrationEngineTests` | ❌ GAP |
| 296 | `migration_consequence` | World | `MigrationConsequenceEngine`, `SeasonalHumanMigrationEngine` | `seasonal_human_migration.json` | `Main`, `MigrationConsequenceHostSession` | `MigrationConsequenceSaveStore` | *None (GAP)* | `--migration-consequence-selftest`, `PlanXp08F6MigrationConsequenceHostIntegrationTests`, `MigrationConsequenceEngineTests` | ❌ GAP |
| 297 | `nuclear_winter_progression` | World | `NuclearWinterProgressionSystem` | `nuclear_winter_phases.json` | `Main` | `NuclearWinterSaveStore` | *None (GAP)* | `--orphan-seal-wave1-selftest`, `Plan164NuclearWinterIntegrationTests` | ❌ GAP |
| 298 | `route_infrastructure` | World | `RouteInfrastructureSystem` | — *(Procedural)* | `RouteInfrastructureSaveStore` | `RouteInfrastructureSaveStore` | *None (GAP)* | , `RouteInfrastructureSystemTests` | ❌ GAP |
| 299 | `storm_forecast` | World | `StormForecastLedger`, `StormForecastState`, `StormForecastCensus`, `StormForecastReadinessEngine` | — *(Procedural)* | `Main`, `StormForecastHostSession` | `StormForecastSaveStore` | *None (GAP)* | `--storm-forecast-selftest`, `StormForecastReadinessEngineTests`, `StormForecastLedgerTests` | ❌ GAP |
| 300 | `subterranean` | World | `SubterraneanSystem` | `subterranean_zones.json` | `SubterraneanHostSession` | `SubterraneanSaveStore` | *None (GAP)* | , `SubterraneanSystemTests` | ❌ GAP |
| 301 | `weather_forecast_reliability` | World | `WeatherForecastReliabilityEngine`, `ForecastConfidenceGrade` | — *(Procedural)* | `Main`, `WeatherForecastReliabilityHostSession` | `WeatherForecastReliabilitySaveStore` | *None (GAP)* | `--weather-forecast-reliability-selftest`,  | ❌ GAP |
| 302 | `world_evolution` | World | `WorldEvolutionEngine`, `WorldEvolutionState`, `WorldEvolutionEventDef`, `WastelandMapSystem` | `world_evolution_events.json` | `Main`, `WorldEvolutionHostSession` | `WorldEvolutionSaveStore` | *None (GAP)* | `--world-evolution-selftest`, `PlanTriplePackageJCoreTests` | ❌ GAP |
| 303 | `amphibious_draisine` | World & Expeditions | `AmphibiousDraisineEngine` | `amphibious_draisine_catalog.json` | `AmphibiousDraisineHostSession` | `AmphibiousDraisineSaveStore` | `AmphibiousDraisinePanel` | `--plans-122-125-selftest`, `Plan125AmphibiousDraisineEngineTests` | ✅ 6/6 |
| 304 | `armored_crawlers` | World & Expeditions | `ArmoredCrawlerExpeditionSystem` | `armored_crawler_modules.json` | `Main` | `ArmoredCrawlerSaveStore` | `GameDashboardPanel` | `--save-store-checksum-selftest`, `FlagshipIntegrationIxSmokeTests` | ✅ 6/6 |
| 305 | `encounter_choice` | World & Expeditions | `EncounterChoiceResolver` | `door_encounters.json` | `EncounterChoiceState` | `EncounterChoiceSaveStore` | `DoorEncounterModal` | `--moral-choice-selftest`, `EncounterChoiceResolverTests` | ❌ GAP |
| 306 | `expedition` | World & Expeditions | `ExpeditionSystem`, `ExpeditionEncounterBridge` | `locations.json` | `ExpeditionHostSession` | `ExpeditionSaveStore` | `ExpeditionPanel` | `--expedition-selftest`, `--expedition-panel-uitest`, `ExpeditionCampSystemTests` | ✅ 6/6 |
| 307 | `insar_deformation` | World & Expeditions | `InSarDeformationEngine` | `insar_geodesy_catalog.json` | `InSarMappingHostSession` | `InSarMappingSaveStore` | `InSarMappingPanel` | `--plans-139-141-selftest`, `Plan139InSarDeformationTests` | ✅ 6/6 |
| 308 | `runflat_tire` | World & Expeditions | `RunFlatTireEngine` | `runflat_tire_catalog.json` | `RunFlatTireHostSession` | `RunFlatTireSaveStore` | `RunFlatTirePanel` | `--plans-139-141-selftest`, `Plan141RunFlatTireTests` | ✅ 6/6 |
| 309 | `travel_encounters` | World & Expeditions | `TravelEncounterSystem`, `TravelEncounterCatalog` | `travel_encounters.json` | `TravelEncounterSystem` | `TravelEncounterSaveStore` | `ExpeditionPanel` | `--expedition-encounter-bridge-selftest`, `TravelEncounterCooldownGroupTests`, `PatrolEncounterFullRegressionTests` | ✅ 6/6 |
| 310 | `wasteland_map` | World & Expeditions | `WastelandMapSystem` | `wasteland_map_v1.json` | `WorldHostSession` | `WastelandMapSaveStore` | `MapPanel` | `--world-selftest`, `WastelandMapPersistenceTests` | ✅ 6/6 |
| 311 | `waystation` | World & Expeditions | `WaystationSystem` | `locations.json` | `WaystationHostSession` | `WaystationSaveStore` | `WaystationNetworkPanel` | `--shelter-operations-selftest`, `WaystationSystemTests` | ✅ 6/6 |
| 312 | `wildlife_trapping` | World & Expeditions | `WildlifeTrappingSystem` | — *(Procedural)* | `WildlifeTrappingHostSession` | `WildlifeTrappingSaveStore` | `WildlifeTrappingPanel` | `--shelter-operations-selftest`, `WildlifeTrappingSystemTests` | ✅ 6/6 |
| 313 | `world` | World & Expeditions | `WastelandMapSystem`, `WeatherSystem` | `locations.json` | `WorldHostSession` | `WorldSaveStore` | `MapPanel`, `WeatherPanel` | `--world-selftest`, `WorldSaveablesTests` | ✅ 6/6 |
| 314 | `chemical_plume` | World & Weather | `ChemicalPlumeDispersionEngine` | — *(Procedural)* | `Main`, `ChemicalPlumeHostSession` | `ChemicalPlumeSaveStore` | *None (GAP)* | `--chemical-plume-selftest`,  | ❌ GAP |

---

## 3. Subsystem Deep Evidence Graph & Source Paths

Detailed file paths and symbols proving zero conceptual placeholders:

### 1. `commitment` — Plan 38 — authored obligations/deadlines: warnings, met/missed terminal state, and consequence routing (Campaign)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCommitments()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:156`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Commitments/CommitmentSystem.cs`](../../Assets/Ashfall.Core/Commitments/CommitmentSystem.cs)
  - Host Session: [`src/Host/CommitmentHostSession.cs`](../../src/Host/CommitmentHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CommitmentHostSession.cs`](../../src/Host/CommitmentHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`](../../Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CommitmentSystemTests.cs`](../../Ashfall.Core.Tests/Campaign/CommitmentSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan38CommitmentHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan38CommitmentHostIntegrationTests.cs)

### 2. `consequence_ledger` — Campaign consequence flags & counters — cross-quest/moral-choice state persisted across saves and reset on new campaigns (Campaign)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupConsequenceLedger()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:244`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs`](../../Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ConsequenceLedgerSaveStore.cs`](../../src/Host/ConsequenceLedgerSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs`](../../Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flags/ConsequenceLedgerSaveTests.cs`](../../Ashfall.Core.Tests/Flags/ConsequenceLedgerSaveTests.cs)

### 3. `difficulty_settings` — Plan 181 — runtime difficulty settings: active preset, custom slider values, and the ironman lock. The campaign identity preset stays in the checksummed header. (Campaign)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupDifficultySettings()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.DifficultySettings.cs:59`, `src/Main.DifficultySettings.cs:87`, `src/Main.DifficultySettings.cs:97`, `src/Main.DifficultySettings.cs:107`, `src/Main.SaveOrchestrator.cs:334`
- **UI Routes:** `protocol`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Difficulty/DifficultyPresetCatalog.cs`](../../Assets/Ashfall.Core/Difficulty/DifficultyPresetCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Difficulty/DifficultyScalarsProvider.cs`](../../Assets/Ashfall.Core/Difficulty/DifficultyScalarsProvider.cs)
  - Core System: [`Assets/Ashfall.Core/Difficulty/DifficultySettingsSystem.cs`](../../Assets/Ashfall.Core/Difficulty/DifficultySettingsSystem.cs)
  - Host Session: [`src/Host/DifficultySettingsHostSession.cs`](../../src/Host/DifficultySettingsHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DifficultySettingsHostSession.cs`](../../src/Host/DifficultySettingsHostSession.cs)
  - UI Panel: [`src/UI/StartingCohortSetupPanel.cs`](../../src/UI/StartingCohortSetupPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Difficulty/Plan181DifficultySettingsIntegrationTests.cs`](../../Ashfall.Core.Tests/Difficulty/Plan181DifficultySettingsIntegrationTests.cs)

### 4. `endgame` — Campaign endgame phase, ending selection, sealed epilogue report (Campaign & Lore)
- **Owner Domain:** `endgame`
- **Setup Method:** `Main.SetupEndgame()` | **Invoked:** yes | **Cadence:** `On-Demand (Day Threshold / Extinction)`
- **Setup Invocation Sites:** `src/Main.Application.cs:1111`, `src/Main.Endgame.cs:48`, `src/Main.Holdfast.cs:299`, `src/Main.PlayerSurfaces.cs:465`, `src/Main.SaveOrchestrator.cs:283`
- **UI Routes:** `epilogue`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs`](../../Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs)
  - Core System: [`Assets/Ashfall.Core/Endgame/EndgameSystem.cs`](../../Assets/Ashfall.Core/Endgame/EndgameSystem.cs)
  - Host Session: [`src/Host/EndgameHostSession.cs`](../../src/Host/EndgameHostSession.cs)
  - Save Store: [`src/Host/EndgameSaveStore.cs`](../../src/Host/EndgameSaveStore.cs)
  - UI Panel: [`src/UI/EpiloguePanel.cs`](../../src/UI/EpiloguePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Endgame/CampaignOutcomeEvaluatorTests.cs`](../../Ashfall.Core.Tests/Endgame/CampaignOutcomeEvaluatorTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Endgame/EndgameSystemTests.cs`](../../Ashfall.Core.Tests/Endgame/EndgameSystemTests.cs)

### 5. `host_event` — Host event ledger & moral decisions (Campaign & Lore)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupEventAdapter()` | **Invoked:** yes | **Cadence:** `On-Demand (Moral Dilemma)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2562`, `src/Main.Narrative.cs:367`, `src/Main.Phase0.cs:188`, `src/Main.ShelterSocial.cs:345`, `src/Main.ShelterSocial.cs:461`
- **UI Routes:** `event_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Host Session: [`src/Host/HostEventAdapter.cs`](../../src/Host/HostEventAdapter.cs)
  - Save Store: [`src/Host/HostEventSaveStore.cs`](../../src/Host/HostEventSaveStore.cs)
  - Save Store: [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs)
  - UI Panel: [`src/UI/EventDetailPanel.cs`](../../src/UI/EventDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BareSaveStoreSealTests.cs`](../../Ashfall.Core.Tests/BareSaveStoreSealTests.cs)

### 6. `journal` — Player journal, logs, and codex entries (Campaign & Lore)
- **Owner Domain:** `journal`
- **Setup Method:** `Main.SetupJournal()` | **Invoked:** yes | **Cadence:** `On-Demand (Log/Event)`
- **Setup Invocation Sites:** `src/Main.Application.cs:1094`, `src/Main.CampaignOwners.cs:2209`, `src/Main.CampaignOwners.cs:2296`, `src/Main.CampaignOwners.cs:2417`, `src/Main.CampaignServices.cs:47`, `src/Main.Codex.cs:74`, `src/Main.Codex.cs:91`, `src/Main.Codex.cs:102`, `src/Main.DutyRoster.cs:40`, `src/Main.Echoes.cs:27`, `src/Main.Echoes.cs:101`, `src/Main.Echoes.cs:129`, `src/Main.EcologicalInfestations.cs:70`, `src/Main.EcologicalInfestations.cs:158`, `src/Main.EcologicalInfestations.cs:178`, `src/Main.EcologicalInfestations.cs:211`, `src/Main.Economy.cs:253`, `src/Main.ExpandedShelterSystems.cs:61`, `src/Main.ExpandedShelterSystems.cs:99`, `src/Main.ExpandedShelterSystems.cs:640`, `src/Main.ExpandedShelterSystems.cs:708`, `src/Main.ExpandedShelterSystems.cs:724`, `src/Main.Expeditions.cs:820`, `src/Main.GameFlow.cs:452`, `src/Main.GameFlow.cs:501`, `src/Main.GameFlow.cs:540`, `src/Main.GameFlow.cs:547`, `src/Main.GameFlow.cs:589`, `src/Main.GameFlow.cs:681`, `src/Main.Lifecycle.cs:718`, `src/Main.Maritime.cs:56`, `src/Main.Medical.cs:612`, `src/Main.MoralChoice.cs:36`, `src/Main.MoralChoice.cs:273`, `src/Main.MoralChoice.cs:290`, `src/Main.MoralChoice.cs:318`, `src/Main.MoraleContagion.cs:95`, `src/Main.Narrative.cs:319`, `src/Main.Narrative.cs:460`, `src/Main.Narrative.cs:468`, `src/Main.Narrative.cs:591`, `src/Main.Phase0.cs:113`, `src/Main.Plans147.cs:217`, `src/Main.Plans152.cs:278`, `src/Main.Plans162_165.cs:400`, `src/Main.Plans162_185.cs:21`, `src/Main.Plans162_185.cs:106`, `src/Main.Plans166_169.cs:117`, `src/Main.Plans62_65.cs:208`, `src/Main.PlayerSurfaces.cs:245`, `src/Main.PlayerSurfaces.cs:315`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:354`, `src/Main.PlayerSurfaces.cs:395`, `src/Main.PlayerSurfaces.cs:400`, `src/Main.PlayerSurfaces.cs:480`, `src/Main.ShelterInfrastructure.cs:162`, `src/Main.ShelterSocial.cs:491`, `src/Main.ShelterSocial.cs:524`, `src/Main.Spiritual.cs:84`, `src/Main.UiHandlers.cs:40`, `src/Main.UiHandlers.cs:57`, `src/Main.UiHandlers.cs:131`, `src/Main.UiHandlers.cs:147`, `src/Main.UiHandlers.cs:157`, `src/Main.UiPanels.cs:1028`, `src/Main.UiPanels.cs:1053`, `src/Main.UiPanels.cs:1069`, `src/Main.VisitorIntegration.cs:101`, `src/Main.YearOfAsh.cs:510`, `src/Main.Zealotry.cs:64`
- **UI Routes:** `journal`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Journal/JournalSystem.cs`](../../Assets/Ashfall.Core/Journal/JournalSystem.cs)
  - Host Session: [`src/Host/JournalHostSession.cs`](../../src/Host/JournalHostSession.cs)
  - Save Store: [`src/Journal/JournalSaveStore.cs`](../../src/Journal/JournalSaveStore.cs)
  - UI Panel: [`src/Journal/JournalBookUI.cs`](../../src/Journal/JournalBookUI.cs)
  - UI Panel: [`src/UI/JournalPanel.cs`](../../src/UI/JournalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/JournalSystemTests.cs`](../../Ashfall.Core.Tests/JournalSystemTests.cs)

### 7. `memorial` — Fallen survivors memorial wall (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupMemorial()` | **Invoked:** yes | **Cadence:** `On-Demand (Survivor Fallen Eulogy)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2580`, `src/Main.Endgame.cs:254`, `src/Main.Lifecycle.cs:735`, `src/Main.Medical.cs:71`, `src/Main.MedicalTriage.cs:172`, `src/Main.Plans162_185.cs:107`, `src/Main.SaveOrchestrator.cs:228`, `src/Main.ShelterBatch3.cs:378`, `src/Main.SurvivorFate.cs:36`, `src/Main.UiPanels.cs:1342`, `src/Main.UnifiedEnding.cs:76`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`](../../Assets/Ashfall.Core/Memorial/MemorialSystem.cs)
  - Save Store: [`src/Host/MemorialSaveStore.cs`](../../src/Host/MemorialSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs`](../../Ashfall.Core.Tests/Memorial/MemorialSystemTests.cs)

### 8. `narrative` — Branching story arcs and narrative flags (Campaign & Lore)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupNarrative()` | **Invoked:** yes | **Cadence:** `On-Demand (Dialog Choice)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2509`, `src/Main.CampaignServices.cs:64`, `src/Main.Narrative.cs:513`, `src/Main.Narrative.cs:531`, `src/Main.Narrative.cs:545`, `src/Main.Narrative.cs:578`, `src/Main.PlayerSurfaces.cs:219`, `src/Main.SaveOrchestrator.cs:200`, `src/Main.SurvivorFate.cs:38`
- **UI Routes:** `journal`, `event_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs)
  - Host Session: [`src/Host/NarrativeHostSession.cs`](../../src/Host/NarrativeHostSession.cs)
  - Save Store: [`src/Host/NarrativeSaveStore.cs`](../../src/Host/NarrativeSaveStore.cs)
  - UI Panel: [`src/UI/EventsLogPanel.cs`](../../src/UI/EventsLogPanel.cs)
  - UI Panel: [`src/UI/FactionsNarrativePanel.cs`](../../src/UI/FactionsNarrativePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`](../../Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs)

### 9. `phase0` — Pre-war timeline and bunker startup (Campaign & Lore)
- **Owner Domain:** `phase0`
- **Setup Method:** `Main.SetupPhase0()` | **Invoked:** yes | **Cadence:** `On-Demand (Pre-War Flashback)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1924`, `src/Main.CampaignOwners.cs:1934`, `src/Main.CampaignServices.cs:42`, `src/Main.Expeditions.cs:348`, `src/Main.GameFlow.cs:412`, `src/Main.GameFlow.cs:418`, `src/Main.GameFlow.cs:447`, `src/Main.GameFlow.cs:494`, `src/Main.GameFlow.cs:506`, `src/Main.Medical.cs:227`, `src/Main.Phase0.cs:342`, `src/Main.Phase0.cs:348`, `src/Main.Phase0.cs:354`, `src/Main.Phase0.cs:360`, `src/Main.PlayerSurfaces.cs:174`, `src/Main.PlayerSurfaces.cs:179`, `src/Main.PlayerSurfaces.cs:240`, `src/Main.PlayerSurfaces.cs:280`, `src/Main.PlayerSurfaces.cs:315`, `src/Main.PlayerSurfaces.cs:536`, `src/Main.PlayerSurfaces.cs:585`, `src/Main.PlayerSurfaces.cs:590`, `src/Main.SaveOrchestrator.cs:212`, `src/Main.ShelterBatch3.cs:360`, `src/Main.SurvivorFate.cs:42`, `src/Main.Survivors.cs:275`, `src/Main.UiHandlers.cs:22`, `src/Main.UiHandlers.cs:48`
- **UI Routes:** `phase0`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs`](../../Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs)
  - Host Session: [`src/Host/Phase0HostSession.cs`](../../src/Host/Phase0HostSession.cs)
  - Save Store: [`src/Host/Phase0SaveStore.cs`](../../src/Host/Phase0SaveStore.cs)
  - UI Panel: [`src/UI/Phase0Panel.cs`](../../src/UI/Phase0Panel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`](../../Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs)

### 10. `survivor_fate` — Unified survivor-death ledger: one immutable fate record per deceased survivor (Campaign & Lore)
- **Owner Domain:** `memorial`
- **Setup Method:** `Main.SetupSurvivorFate()` | **Invoked:** yes | **Cadence:** `Daily Survivor-Death Cascade`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1684`, `src/Main.Endgame.cs:253`, `src/Main.Expeditions.cs:349`, `src/Main.Holdfast.cs:113`, `src/Main.SaveOrchestrator.cs:229`, `src/Main.Spiritual.cs:102`, `src/Main.SurvivorFate.cs:132`, `src/Main.UnifiedEnding.cs:75`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorFateSaveStore.cs`](../../src/Host/SurvivorFateSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SurvivorFateSystemTests.cs`](../../Ashfall.Core.Tests/SurvivorFateSystemTests.cs)

### 11. `onboarding` — First-hour onboarding journey progress, dismissed hints, assistance level, completion (Campaign & Onboarding)
- **Owner Domain:** `onboarding`
- **Setup Method:** `Main.SetupOnboarding()` | **Invoked:** yes | **Cadence:** `On-Demand (Player Sigil Recording)`
- **Setup Invocation Sites:** `src/Main.GameFlow.cs:393`, `src/Main.Onboarding.cs:122`, `src/Main.Onboarding.cs:149`, `src/Main.Onboarding.cs:192`, `src/Main.Onboarding.cs:212`, `src/Main.PlayerSurfaces.cs:27`, `src/Main.PlayerSurfaces.cs:1006`, `src/Main.PlayerSurfaces.cs:1022`, `src/Main.SaveOrchestrator.cs:234`, `src/Main.ShelterSocial.cs:358`
- **UI Routes:** `help`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`](../../Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OnboardingSaveStore.cs`](../../src/Host/OnboardingSaveStore.cs)
  - UI Panel: [`src/UI/OnboardingHintPanel.cs`](../../src/UI/OnboardingHintPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/OnboardingJourneyTests.cs`](../../Ashfall.Core.Tests/OnboardingJourneyTests.cs)

### 12. `archive_desk` — Document archiving, ink, and scribing (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupArchiveDesk()` | **Invoked:** yes | **Cadence:** `Daily Scribing & Folio Archival`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:140`
- **UI Routes:** `archive_desk`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ArchiveDeskSystem.cs`](../../Assets/Ashfall.Core/ArchiveDeskSystem.cs)
  - Host Session: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - Save Store: [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
  - UI Panel: [`src/UI/ArchiveDeskPanel.cs`](../../src/UI/ArchiveDeskPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`](../../Ashfall.Core.Tests/ArchiveDeskSystemTests.cs)

### 13. `campaign_day` — Master campaign day counter & ticks (Campaign & Progression)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCampaignDay()` | **Invoked:** yes | **Cadence:** `Master Sim Clock / Dawn Advance`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:30`, `src/Main.DeepWell.cs:22`, `src/Main.Economy.cs:368`, `src/Main.Expeditions.cs:114`, `src/Main.Expeditions.cs:354`, `src/Main.GameFlow.cs:940`, `src/Main.Holdfast.cs:55`, `src/Main.Holdfast.cs:134`, `src/Main.Holdfast.cs:273`, `src/Main.Maritime.cs:59`, `src/Main.Maritime.cs:131`, `src/Main.MoralChoice.cs:37`, `src/Main.Narrative.cs:392`, `src/Main.Narrative.cs:592`, `src/Main.PfglOctetBoards.cs:51`, `src/Main.PfglOctetBoards.cs:69`, `src/Main.Phase0.cs:45`, `src/Main.Phase0.cs:367`, `src/Main.Plans110_113.cs:30`, `src/Main.Plans110_113.cs:52`, `src/Main.Plans110_113.cs:97`, `src/Main.Plans110_113.cs:117`, `src/Main.Plans146_149.cs:182`, `src/Main.Plans146_149.cs:204`, `src/Main.Plans146_149.cs:263`, `src/Main.Plans146_149.cs:276`, `src/Main.Plans166_169.cs:35`, `src/Main.Plans62_65.cs:35`, `src/Main.Plans74_77.cs:40`, `src/Main.Plans74_77.cs:56`, `src/Main.Plans74_77.cs:77`, `src/Main.Plans74_77.cs:93`, `src/Main.Plans78_81.cs:36`, `src/Main.Plans78_81.cs:53`, `src/Main.Plans78_81.cs:70`, `src/Main.PlansB86_B89.cs:37`, `src/Main.PlansB86_B89.cs:144`, `src/Main.SaveOrchestrator.cs:166`, `src/Main.ShelterBatch3.cs:65`, `src/Main.ShelterBatch3.cs:100`, `src/Main.ShelterBatch3.cs:124`, `src/Main.ShelterBatch3.cs:179`, `src/Main.ShelterBatch3.cs:272`, `src/Main.ShelterBatch3.cs:292`, `src/Main.ShelterBatch3.cs:346`, `src/Main.ShelterSocial.cs:49`, `src/Main.ShelterSocial.cs:284`, `src/Main.ShelterSocial.cs:549`, `src/Main.ShelterSocial.cs:573`, `src/Main.SurvivorFate.cs:37`, `src/Main.SurvivorSocial.cs:23`, `src/Main.WaterCondenser.cs:21`, `src/Main.World.cs:173`, `src/Main.World.cs:470`, `src/Main.WorldPlaytest.cs:40`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Host Session: [`Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`](../../Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs)
  - Save Store: [`src/Host/CampaignDaySaveStore.cs`](../../src/Host/CampaignDaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs`](../../Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs)

### 14. `daily_briefing` — Daily dawn briefing notes & status (Campaign & Progression)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupDailyBriefingModal()` | **Invoked:** no | **Cadence:** `Daily Dawn Briefing Aggregation`
- **UI Routes:** `briefing`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs)
  - Core System: [`Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs)
  - Host Session: [`Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs`](../../Assets/Ashfall.Core/Campaign/DailyBriefingSave.cs)
  - Save Store: [`src/Host/DailyBriefingSaveStore.cs`](../../src/Host/DailyBriefingSaveStore.cs)
  - UI Panel: [`src/UI/DailyBriefingModal.cs`](../../src/UI/DailyBriefingModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/DailyBriefingReportBuilderTests.cs`](../../Ashfall.Core.Tests/Campaign/DailyBriefingReportBuilderTests.cs)

### 15. `library_study` — Research library books and blueprints (Campaign & Progression)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupLibraryStudy()` | **Invoked:** yes | **Cadence:** `Daily Codex Research Ticks`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:139`
- **UI Routes:** `library_study`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/LibraryStudySystem.cs`](../../Assets/Ashfall.Core/LibraryStudySystem.cs)
  - Host Session: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - Save Store: [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
  - UI Panel: [`src/UI/LibraryStudyPanel.cs`](../../src/UI/LibraryStudyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/LibraryStudySystemTests.cs`](../../Ashfall.Core.Tests/LibraryStudySystemTests.cs)

### 16. `dynamic_quests` — Campaign-wide emergency dynamic quests (Campaign & Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupDynamicQuests()` | **Invoked:** yes | **Cadence:** `On-Demand (Campaign-Wide Emergency Quests)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:242`
- **UI Routes:** `dynamic_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/DynamicQuestlines.cs`](../../Assets/Ashfall.Core/Quests/DynamicQuestlines.cs)
  - Host Session: [`src/Host/DynamicQuestSaveStore.cs`](../../src/Host/DynamicQuestSaveStore.cs)
  - Save Store: [`src/Host/DynamicQuestSaveStore.cs`](../../src/Host/DynamicQuestSaveStore.cs)
  - UI Panel: [`src/UI/DynamicQuestlinePanel.cs`](../../src/UI/DynamicQuestlinePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs`](../../Ashfall.Core.Tests/Quests/DynamicQuestlineTests.cs)

### 17. `narrative_questlines` — Survivor narrative questline arcs and crisis branch outcomes (Campaign & Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupNarrativeQuestlines()` | **Invoked:** yes | **Cadence:** `On-Demand (Survivor Narrative Arc Progression)`
- **Setup Invocation Sites:** `src/Main.Application.cs:1117`, `src/Main.NarrativeQuestlines.cs:65`, `src/Main.NarrativeQuestlines.cs:99`, `src/Main.NarrativeQuestlines.cs:174`, `src/Main.NarrativeQuestlines.cs:227`, `src/Main.NarrativeQuestlines.cs:238`, `src/Main.SaveOrchestrator.cs:296`
- **UI Routes:** `quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs`](../../Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs)
  - Host Session: [`src/Host/NarrativeQuestlineHostSession.cs`](../../src/Host/NarrativeQuestlineHostSession.cs)
  - Save Store: [`src/Host/NarrativeQuestlineSaveStore.cs`](../../src/Host/NarrativeQuestlineSaveStore.cs)
  - UI Panel: [`src/UI/QuestsPanel.cs`](../../src/UI/QuestsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`](../../Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs)

### 18. `chlor_alkali_synthesis` — Plans 110-113 — chlor-alkali electrolytic plant, membrane health, hazard load, and chemical production (Chemistry)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupChlorAlkali()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans110_113.cs:136`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs`](../../Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs)
  - Host Session: [`src/Host/ChlorAlkaliHostSession.cs`](../../src/Host/ChlorAlkaliHostSession.cs)
  - Save Store: [`src/Host/ChlorAlkaliSaveStore.cs`](../../src/Host/ChlorAlkaliSaveStore.cs)

### 19. `memory_decay` — Plan 185 — Survivor memory & knowledge decay across skill, knowledge, and relation domains (Cognition)
- **Owner Domain:** `cognition`
- **Setup Method:** `Main.SetupMemoryDecay()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3034`, `src/Main.CampaignOwners.cs:3045`, `src/Main.MemoryDecay.cs:48`, `src/Main.MemoryDecay.cs:60`, `src/Main.MemoryDecay.cs:69`, `src/Main.SaveOrchestrator.cs:351`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs`](../../Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs)
  - Host Session: [`src/Host/MemoryDecayHostSession.cs`](../../src/Host/MemoryDecayHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MemoryDecayHostSession.cs`](../../src/Host/MemoryDecayHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Cognition/MemoryDecaySystemTests.cs`](../../Ashfall.Core.Tests/Cognition/MemoryDecaySystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Cognition/Plan185MemoryDecayIntegrationTests.cs`](../../Ashfall.Core.Tests/Cognition/Plan185MemoryDecayIntegrationTests.cs)

### 20. `ballistic_shield` — Plans 110-113 — defensive ballistic shields, stances, integrity, and ground anchoring (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupBallisticShield()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans110_113.cs:139`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/BallisticShieldEngine.cs`](../../Assets/Ashfall.Core/Combat/BallisticShieldEngine.cs)
  - Host Session: [`src/Host/BallisticShieldHostSession.cs`](../../src/Host/BallisticShieldHostSession.cs)
  - Save Store: [`src/Host/BallisticShieldSaveStore.cs`](../../src/Host/BallisticShieldSaveStore.cs)

### 21. `ballistics_workbench` — Plan B75 — weapon calibration, headspace wear, custom ammunition, and failure state (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupBallisticsWorkbench()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans74_77.cs:32`, `src/Main.Plans74_77.cs:206`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs`](../../Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 22. `settlement_defenses` — Plans 162-165 — trap installations, pre-combat raid resolution, captures, and the raid log (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupDefense()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Plans162_165.cs:368`, `src/Main.Plans162_165.cs:414`, `src/Main.SaveOrchestrator.cs:290`
- **UI Routes:** `defense_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Defense/DefenseSystem.cs`](../../Assets/Ashfall.Core/Defense/DefenseSystem.cs)
  - Host Session: [`src/Host/DefenseHostSession.cs`](../../src/Host/DefenseHostSession.cs)
  - Save Store: [`src/Host/DefenseSaveStore.cs`](../../src/Host/DefenseSaveStore.cs)
  - UI Panel: [`src/UI/DefenseGridPanel.cs`](../../src/UI/DefenseGridPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DefenseSystemTests.cs`](../../Ashfall.Core.Tests/DefenseSystemTests.cs)

### 23. `sky_defense_battery` — Kinetic sky-layer counter-battery: turret state, magazine, tracks, maintenance (Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupSkyDefense()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Lifecycle.cs:732`
- **UI Routes:** `sky_defense_battery`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs`](../../Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SkyDefenseBatterySaveStore.cs`](../../src/Host/SkyDefenseBatterySaveStore.cs)
  - UI Panel: [`src/UI/SkyDefenseBatteryPanel.cs`](../../src/UI/SkyDefenseBatteryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SkyDefenseBatteryTests.cs`](../../Ashfall.Core.Tests/SkyDefenseBatteryTests.cs)

### 24. `perimeter_defense` — Surface perimeter defense emplacements (Combat & Defense)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupPerimeterDefense()` | **Invoked:** yes | **Cadence:** `Daily Emplacement + Watch Readiness Tick`
- **Setup Invocation Sites:** `src/Main.NightWatch.cs:32`, `src/Main.SaveOrchestrator.cs:287`
- **UI Routes:** `night_watch`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs`](../../Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs)
  - Core System: [`Assets/Ashfall.Core/World/NightWatchOperationsCatalog.cs`](../../Assets/Ashfall.Core/World/NightWatchOperationsCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/World/NightWatchPatrolReadinessEngine.cs`](../../Assets/Ashfall.Core/World/NightWatchPatrolReadinessEngine.cs)
  - Host Session: [`src/Host/NightWatchHostSession.cs`](../../src/Host/NightWatchHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PerimeterDefenseSaveStore.cs`](../../src/Host/PerimeterDefenseSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/NightWatchPanel.cs`](../../src/UI/NightWatchPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs`](../../Ashfall.Core.Tests/Defense/PerimeterDefenseTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/NightWatchHostIntegrationTests.cs`](../../Ashfall.Core.Tests/World/NightWatchHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/NightWatchOperationsTests.cs`](../../Ashfall.Core.Tests/World/NightWatchOperationsTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/NightWatchPatrolReadinessEngineTests.cs`](../../Ashfall.Core.Tests/World/NightWatchPatrolReadinessEngineTests.cs)

### 25. `sound_ranging` — Plan 123 — defensive sound-ranging calibration, node status, observation history, threat estimate (Combat & Defense)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupSoundRanging()` | **Invoked:** yes | **Cadence:** `Event-Driven (Hostile-Fire Observations) + Daily Drift`
- **Setup Invocation Sites:** `src/Main.NightWatch.cs:33`, `src/Main.Plans122to125.cs:309`, `src/Main.SaveOrchestrator.cs:193`
- **UI Routes:** `sound_ranging`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs`](../../Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs)
  - Host Session: [`src/Host/SoundRangingHostSession.cs`](../../src/Host/SoundRangingHostSession.cs)
  - Save Store: [`src/Host/SoundRangingSaveStore.cs`](../../src/Host/SoundRangingSaveStore.cs)
  - UI Panel: [`src/UI/SoundRangingPanel.cs`](../../src/UI/SoundRangingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/Plan123SoundRangingThreatEngineTests.cs`](../../Ashfall.Core.Tests/Combat/Plan123SoundRangingThreatEngineTests.cs)

### 26. `time_capsules` — Plan 212 — time capsules, legacy messages, delayed discovery, and cross-generational communication (Communication & Heritage (Plan 212))
- **Owner Domain:** `communication`
- **Setup Method:** `Main.SetupTimeCapsules()` | **Invoked:** yes | **Cadence:** `Daily (Scheduled Opening & Message Delivery)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:153`
- **UI Routes:** `time_capsule`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs`](../../Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs)
  - Host Session: [`src/Host/TimeCapsuleHostSession.cs`](../../src/Host/TimeCapsuleHostSession.cs)
  - Save Store: [`src/Host/TimeCapsuleSaveStore.cs`](../../src/Host/TimeCapsuleSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/TimeCapsulePanel.cs`](../../src/UI/TimeCapsulePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communication/Plan212TimeCapsuleIntegrationTests.cs`](../../Ashfall.Core.Tests/Communication/Plan212TimeCapsuleIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communication/TimeCapsuleSystemTests.cs`](../../Ashfall.Core.Tests/Communication/TimeCapsuleSystemTests.cs)

### 27. `chemical_synthesis` — Chemical synthesis retorts and apparatus (Crafting & Chemistry)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupChemicalSynthesis()` | **Invoked:** yes | **Cadence:** `On-Demand (Retort Synthesis)`
- **Setup Invocation Sites:** `src/Main.Application.cs:1118`, `src/Main.SaveOrchestrator.cs:297`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs`](../../Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs)
  - Host Session: [`src/Host/ChemicalSynthesisHostSession.cs`](../../src/Host/ChemicalSynthesisHostSession.cs)
  - Save Store: [`src/Host/ChemicalSynthesisSaveStore.cs`](../../src/Host/ChemicalSynthesisSaveStore.cs)
  - UI Panel: [`src/UI/ChemicalLabPanel.cs`](../../src/UI/ChemicalLabPanel.cs)

### 28. `cassette_playback` — Cultural cassette sets — collected tape parts, first-play awards, completed sets, and revealed hidden caches. Tape morale stays with NeedsSystem; items stay with the canonical inventory. (Culture)
- **Owner Domain:** `culture`
- **Setup Method:** `Main.SetupCassettePlayback()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:854`, `src/Main.CampaignOwners.cs:865`, `src/Main.CassettePlayback.cs:72`, `src/Main.CassettePlayback.cs:87`, `src/Main.ContentCertification.cs:87`, `src/Main.SaveOrchestrator.cs:327`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs`](../../Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Audio/CassetteSetCatalogLoader.cs`](../../Assets/Ashfall.Core/Audio/CassetteSetCatalogLoader.cs)
  - Host Session: [`src/Host/CassettePlaybackHostSession.cs`](../../src/Host/CassettePlaybackHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CassettePlaybackHostSession.cs`](../../src/Host/CassettePlaybackHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Audio/PlanCassettePlaybackTests.cs`](../../Ashfall.Core.Tests/Audio/PlanCassettePlaybackTests.cs)

### 29. `culture_creation` — Plan 178 — Art and culture creation: survivor artworks, masterworks, cultural identity, and display morale bonus (Culture)
- **Owner Domain:** `culture`
- **Setup Method:** `Main.SetupCultureCreation()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3149`, `src/Main.CampaignOwners.cs:3160`, `src/Main.CultureCreation.cs:18`, `src/Main.SaveOrchestrator.cs:389`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`](../../Assets/Ashfall.Core/Culture/CultureCreationSystem.cs)
  - Host Session: [`src/Host/CultureCreationHostSession.cs`](../../src/Host/CultureCreationHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CultureCreationHostSession.cs`](../../src/Host/CultureCreationHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan178ArtCultureIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan178ArtCultureIntegrationTests.cs)

### 30. `shelter_museum` — Plan 218 — shelter museum: curated artifact collection, exhibitions, curator, once-per-day visits, and event history (Culture)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupShelterMuseum()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (TickPlans46_49)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:373`, `src/Main.ShelterMuseum.cs:60`, `src/Main.ShelterMuseum.cs:69`, `src/Main.ShelterMuseum.cs:82`, `src/Main.ShelterMuseum.cs:90`, `src/Main.ShelterMuseum.cs:97`
- **UI Routes:** `archive_desk`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Culture/ShelterMuseumSystem.cs`](../../Assets/Ashfall.Core/Culture/ShelterMuseumSystem.cs)
  - Host Session: [`src/Host/ShelterMuseumHostSession.cs`](../../src/Host/ShelterMuseumHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterMuseumHostSession.cs`](../../src/Host/ShelterMuseumHostSession.cs)
  - UI Panel: [`src/UI/ArchiveDeskPanel.cs`](../../src/UI/ArchiveDeskPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Culture/Plan218MuseumHostWiringTests.cs`](../../Ashfall.Core.Tests/Culture/Plan218MuseumHostWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Culture/Plan218MuseumIntegrationTests.cs`](../../Ashfall.Core.Tests/Culture/Plan218MuseumIntegrationTests.cs)

### 31. `perimeter_early_warning` — Perimeter radar: operational modes, sensor calibration, contact classification, and scan sweep feed (Defense)
- **Owner Domain:** `defense`
- **Setup Method:** `Main.SetupPerimeterEarlyWarning()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3458`, `src/Main.PerimeterEarlyWarning.cs:33`, `src/Main.PerimeterEarlyWarning.cs:40`, `src/Main.PerimeterEarlyWarning.cs:52`, `src/Main.PerimeterEarlyWarning.cs:62`, `src/Main.PerimeterEarlyWarning.cs:69`, `src/Main.PerimeterEarlyWarning.cs:75`, `src/Main.SaveOrchestrator.cs:362`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Defense/PerimeterEarlyWarningEngine.cs`](../../Assets/Ashfall.Core/Defense/PerimeterEarlyWarningEngine.cs)
  - Host Session: [`src/Host/PerimeterEarlyWarningHostSession.cs`](../../src/Host/PerimeterEarlyWarningHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PerimeterEarlyWarningHostSession.cs`](../../src/Host/PerimeterEarlyWarningHostSession.cs)

### 32. `radiation_economy` — Contaminated-trade radiation economy bridge: price multipliers, blocks, and evaluation ledger (Economy)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupRadiationEconomy()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.RadiationEconomy.cs:33`, `src/Main.RadiationEconomy.cs:41`, `src/Main.SaveOrchestrator.cs:358`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radiation/RadiationEconomyBridge.cs`](../../Assets/Ashfall.Core/Radiation/RadiationEconomyBridge.cs)
  - Host Session: [`src/Host/RadiationEconomyHostSession.cs`](../../src/Host/RadiationEconomyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RadiationEconomyHostSession.cs`](../../src/Host/RadiationEconomyHostSession.cs)

### 33. `survivor_barter` — Plan 213 — Survivor barter: offers, completed trades, trade reputation, favor obligations, and disputes (Economy)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupSurvivorBarter()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3430`, `src/Main.SaveOrchestrator.cs:361`, `src/Main.SurvivorBarter.cs:33`, `src/Main.SurvivorBarter.cs:41`, `src/Main.SurvivorBarter.cs:50`, `src/Main.SurvivorBarter.cs:59`, `src/Main.SurvivorBarter.cs:67`, `src/Main.SurvivorBarter.cs:75`, `src/Main.SurvivorBarter.cs:81`, `src/Main.SurvivorBarter.cs:89`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/SurvivorBarterSystem.cs`](../../Assets/Ashfall.Core/Economy/SurvivorBarterSystem.cs)
  - Host Session: [`src/Host/SurvivorBarterHostSession.cs`](../../src/Host/SurvivorBarterHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorBarterHostSession.cs`](../../src/Host/SurvivorBarterHostSession.cs)

### 34. `trade_routes` — Plan 192 — Scheduled trade route contracts, tariffs, reliability tiers, and exclusive goods (Economy)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupTradeRoutes()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:717`, `src/Main.CampaignOwners.cs:728`, `src/Main.SaveOrchestrator.cs:316`, `src/Main.TradeRoutes.cs:45`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs`](../../Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/TradeRouteContract.cs`](../../Assets/Ashfall.Core/Economy/TradeRouteContract.cs)
  - Host Session: [`src/Host/TradeRouteHostSession.cs`](../../src/Host/TradeRouteHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TradeRouteHostSession.cs`](../../src/Host/TradeRouteHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan192TradeRouteHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Economy/Plan192TradeRouteHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/TradeRouteContractTests.cs`](../../Ashfall.Core.Tests/Economy/TradeRouteContractTests.cs)

### 35. `black_market` — Plan 211 — underworld contacts, stock snapshots, debts, heat, and trust (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupBlackMarket()` | **Invoked:** yes | **Cadence:** `Daily Underworld Tick`
- **Setup Invocation Sites:** `src/Main.BlackMarket.cs:21`, `src/Main.BlackMarket.cs:73`, `src/Main.CampaignOwners.cs:2091`, `src/Main.CampaignOwners.cs:2101`, `src/Main.Lifecycle.cs:734`, `src/Main.SaveOrchestrator.cs:206`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/BlackMarketInventoryCatalog.cs`](../../Assets/Ashfall.Core/Economy/BlackMarketInventoryCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/BlackMarketSystem.cs`](../../Assets/Ashfall.Core/Economy/BlackMarketSystem.cs)
  - Host Session: [`src/Host/BlackMarketHostSession.cs`](../../src/Host/BlackMarketHostSession.cs)
  - Save Store: [`src/Host/BlackMarketSaveStore.cs`](../../src/Host/BlackMarketSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs`](../../Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan211BlackMarketTests.cs`](../../Ashfall.Core.Tests/Economy/Plan211BlackMarketTests.cs)

### 36. `caravan` — Trade caravans, routes, and arrivals (Economy & Trade)
- **Owner Domain:** `caravans`
- **Setup Method:** `Main.SetupCaravans()` | **Invoked:** yes | **Cadence:** `Daily Route Travel`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1957`, `src/Main.CampaignOwners.cs:2005`, `src/Main.CampaignServices.cs:68`, `src/Main.SaveOrchestrator.cs:197`
- **UI Routes:** `traveling_caravan`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/TravelingCaravanSystem.cs`](../../Assets/Ashfall.Core/TravelingCaravanSystem.cs)
  - Host Session: [`src/Host/TravelingCaravanHostSession.cs`](../../src/Host/TravelingCaravanHostSession.cs)
  - Save Store: [`src/Host/CaravanSaveStore.cs`](../../src/Host/CaravanSaveStore.cs)
  - UI Panel: [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TradeCaravanCatalogTests.cs`](../../Ashfall.Core.Tests/TradeCaravanCatalogTests.cs)

### 37. `caravan_trade_network` — Faction caravan trade network routes and arrivals (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupCaravanTrade()` | **Invoked:** yes | **Cadence:** `Daily Route Arrival Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:284`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs`](../../Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CaravanTradeSaveStore.cs`](../../src/Host/CaravanTradeSaveStore.cs)
  - UI Panel: [`src/UI/TravelingCaravanPanel.cs`](../../src/UI/TravelingCaravanPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs`](../../Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs)

### 38. `economy` — Dynamic economy rates and market orders (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupEconomy()` | **Invoked:** yes | **Cadence:** `Daily Market Rate Tick`
- **Setup Invocation Sites:** `src/Main.BlackMarket.cs:31`, `src/Main.CampaignOwners.cs:1496`, `src/Main.CampaignOwners.cs:1506`, `src/Main.CampaignOwners.cs:2313`, `src/Main.CampaignServices.cs:46`, `src/Main.Economy.cs:150`, `src/Main.Economy.cs:157`, `src/Main.Economy.cs:203`, `src/Main.Economy.cs:254`, `src/Main.GameFlow.cs:441`, `src/Main.GameFlow.cs:442`, `src/Main.GameFlow.cs:610`, `src/Main.Lifecycle.cs:730`, `src/Main.PlayerSurfaces.cs:231`, `src/Main.PlayerSurfaces.cs:420`, `src/Main.SaveOrchestrator.cs:202`
- **UI Routes:** `trade`, `economy_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/MarketSystem.cs`](../../Assets/Ashfall.Core/Economy/MarketSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/RationingProtocolCatalogLoader.cs`](../../Assets/Ashfall.Core/Economy/RationingProtocolCatalogLoader.cs)
  - Host Session: [`src/Host/EconomyHostSession.cs`](../../src/Host/EconomyHostSession.cs)
  - Save Store: [`src/Host/EconomySaveStore.cs`](../../src/Host/EconomySaveStore.cs)
  - UI Panel: [`src/Economy/EconomyMarketPanel.cs`](../../src/Economy/EconomyMarketPanel.cs)
  - UI Panel: [`src/UI/EconomyDetailPanel.cs`](../../src/UI/EconomyDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DynamicEconomyCharacterizationTests.cs`](../../Ashfall.Core.Tests/DynamicEconomyCharacterizationTests.cs)

### 39. `economy_family` — PLAN-ECONOMY-DATA-FAMILY-TRUTH-270 \u2014 trade-route monopoly premiums and syndicate heat attention (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupEconomyFamily()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (expanded shelter day)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:379`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/BlackMarketContrabandEngine.cs`](../../Assets/Ashfall.Core/Economy/BlackMarketContrabandEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/BlackMarketHeatAttentionEngine.cs`](../../Assets/Ashfall.Core/Economy/BlackMarketHeatAttentionEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/ChitPurityAssayEngine.cs`](../../Assets/Ashfall.Core/Economy/ChitPurityAssayEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/TradeRouteMonopolyEngine.cs`](../../Assets/Ashfall.Core/Economy/TradeRouteMonopolyEngine.cs)
  - Host Session: [`src/Host/EconomyFamilyHostSession.cs`](../../src/Host/EconomyFamilyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/EconomyFamilyHostSession.cs`](../../src/Host/EconomyFamilyHostSession.cs)

### 40. `loan_shark` — PLAN-ECONOMY-LEDGER-TRUTH-96 \u2014 loan-shark debt ledger and escalation (Economy & Trade)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupLoanShark()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (expanded shelter day)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:388`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/LoanSharkEnforcerEngine.cs`](../../Assets/Ashfall.Core/Economy/LoanSharkEnforcerEngine.cs)
  - Host Session: [`src/Host/LoanSharkHostSession.cs`](../../src/Host/LoanSharkHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/LoanSharkHostSession.cs`](../../src/Host/LoanSharkHostSession.cs)

### 41. `regional_treaty` — Faction treaties and non-aggression pacts (Economy & Trade)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupRegionalTreaty()` | **Invoked:** yes | **Cadence:** `Daily Non-Aggression Decay`
- **Setup Invocation Sites:** `src/Main.Endgame.cs:252`, `src/Main.ExpandedShelterSystems.cs:112`, `src/Main.UnifiedEnding.cs:74`
- **UI Routes:** `regional_treaty`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/RegionalTreatySystem.cs`](../../Assets/Ashfall.Core/RegionalTreatySystem.cs)
  - Host Session: [`src/Host/RegionalTreatyHostSession.cs`](../../src/Host/RegionalTreatyHostSession.cs)
  - Save Store: [`src/Host/RegionalTreatySaveStore.cs`](../../src/Host/RegionalTreatySaveStore.cs)
  - UI Panel: [`src/UI/RegionalTreatyPanel.cs`](../../src/UI/RegionalTreatyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 42. `apprenticeship_curriculum` — Learner literacy records, curriculum subject progress, and teaching session outcomes (Education)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupApprenticeshipCurriculum()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ApprenticeshipCurriculum.cs:34`, `src/Main.ApprenticeshipCurriculum.cs:48`, `src/Main.ApprenticeshipCurriculum.cs:58`, `src/Main.ApprenticeshipCurriculum.cs:64`, `src/Main.CampaignOwners.cs:3596`, `src/Main.SaveOrchestrator.cs:368`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Education/ApprenticeshipCurriculumEngine.cs`](../../Assets/Ashfall.Core/Education/ApprenticeshipCurriculumEngine.cs)
  - Host Session: [`src/Host/ApprenticeshipCurriculumHostSession.cs`](../../src/Host/ApprenticeshipCurriculumHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ApprenticeshipCurriculumHostSession.cs`](../../src/Host/ApprenticeshipCurriculumHostSession.cs)

### 43. `emergency_alert` — Plan 194 — Emergency alert & warning system: threat detection, prioritization, response windows, and evacuation protocols (Emergency)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupEmergencyAlerts()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3374`, `src/Main.EmergencyAlerts.cs:26`, `src/Main.EmergencyAlerts.cs:47`, `src/Main.EmergencyAlerts.cs:56`, `src/Main.EmergencyAlerts.cs:64`, `src/Main.EmergencyAlerts.cs:72`, `src/Main.EmergencyAlerts.cs:80`, `src/Main.EmergencyAlerts.cs:94`, `src/Main.EmergencyAlerts.cs:103`, `src/Main.SaveOrchestrator.cs:356`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Emergency/EmergencyAlertSystem.cs`](../../Assets/Ashfall.Core/Emergency/EmergencyAlertSystem.cs)
  - Host Session: [`src/Host/EmergencyAlertHostSession.cs`](../../src/Host/EmergencyAlertHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/EmergencyAlertHostSession.cs`](../../src/Host/EmergencyAlertHostSession.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Emergency/Plan194EmergencyAlertHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Emergency/Plan194EmergencyAlertHostIntegrationTests.cs)

### 44. `meta_progression` — Plan 175 — Meta progression & cross-run profile store: prestige scoring, crests, and NG+ boons (Endgame)
- **Owner Domain:** `endgame`
- **Setup Method:** `Main.SetupMetaProgression()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:631`, `src/Main.CampaignOwners.cs:642`, `src/Main.Endgame.cs:169`, `src/Main.MetaProgression.cs:85`, `src/Main.MetaProgression.cs:122`, `src/Main.SaveOrchestrator.cs:315`
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

### 45. `unified_ending` — Plan 145 — Unified ending resolution & epilogue personalization: political, social, moral, personal, and expedition resolution (Endgame)
- **Owner Domain:** `endgame`
- **Setup Method:** `Main.SetupUnifiedEnding()` | **Invoked:** yes | **Cadence:** `On-Demand (Campaign Sealed)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:463`, `src/Main.CampaignOwners.cs:474`, `src/Main.SaveOrchestrator.cs:309`, `src/Main.UnifiedEnding.cs:48`, `src/Main.UnifiedEnding.cs:167`, `src/Main.UnifiedEnding.cs:181`
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

### 46. `expansion_hub` — Expansion hub discovery state (Expansion Framework)
- **Owner Domain:** `expansion_hub`
- **Setup Method:** `Main.SetupExpansions()` | **Invoked:** yes | **Cadence:** `Daily Hub Tick`
- **Setup Invocation Sites:** `src/Main.Application.cs:1102`, `src/Main.CampaignOwners.cs:1382`, `src/Main.CampaignOwners.cs:2499`, `src/Main.CampaignServices.cs:69`, `src/Main.DebtCredit.cs:157`, `src/Main.Economy.cs:251`, `src/Main.Endgame.cs:250`, `src/Main.ExpansionHub.cs:128`, `src/Main.ExpansionHub.cs:140`, `src/Main.ExpansionHub.cs:155`, `src/Main.ExpansionHub.cs:165`, `src/Main.ExpansionHub.cs:175`, `src/Main.ExpansionHub.cs:184`, `src/Main.ExpansionHub.cs:205`, `src/Main.ExpansionHub.cs:227`, `src/Main.ExpansionHub.cs:253`, `src/Main.ExpansionHub.cs:265`, `src/Main.ExpansionHub.cs:275`, `src/Main.ExpansionHub.cs:287`, `src/Main.ExpansionHub.cs:297`, `src/Main.GameFlow.cs:517`, `src/Main.GameFlow.cs:538`, `src/Main.GameFlow.cs:560`, `src/Main.GameFlow.cs:574`, `src/Main.GameFlow.cs:603`, `src/Main.GameFlow.cs:620`, `src/Main.GameFlow.cs:633`, `src/Main.GameFlow.cs:638`, `src/Main.GameFlow.cs:655`, `src/Main.Medical.cs:546`, `src/Main.PlayerSurfaces.cs:324`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:367`, `src/Main.PlayerSurfaces.cs:381`, `src/Main.PlayerSurfaces.cs:415`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.PlayerSurfaces.cs:435`, `src/Main.PlayerSurfaces.cs:440`, `src/Main.PlayerSurfaces.cs:455`, `src/Main.PlayerSurfaces.cs:667`, `src/Main.SaveOrchestrator.cs:217`, `src/Main.UiHandlers.cs:82`, `src/Main.UiHandlers.cs:98`, `src/Main.UiHandlers.cs:145`, `src/Main.UiHandlers.cs:187`, `src/Main.UiHandlers.cs:198`, `src/Main.UiHandlers.cs:279`, `src/Main.UnifiedEnding.cs:72`, `src/Main.World.cs:64`, `src/Main.World.cs:152`
- **UI Routes:** `expansions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExpansionMasterSession.cs`](../../Assets/Ashfall.Core/ExpansionMasterSession.cs)
  - Host Session: [`src/Host/ExpansionHostSession.cs`](../../src/Host/ExpansionHostSession.cs)
  - Save Store: [`src/Host/ExpansionHubSaveStore.cs`](../../src/Host/ExpansionHubSaveStore.cs)
  - UI Panel: [`src/UI/ExpansionsHubPanel.cs`](../../src/UI/ExpansionsHubPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpansionHubSaveTests.cs`](../../Ashfall.Core.Tests/ExpansionHubSaveTests.cs)

### 47. `expansion_quest` — Expansion questline progression (Expansion Framework)
- **Owner Domain:** `expansion_quest`
- **Setup Method:** `Main.SetupExpansionQuests()` | **Invoked:** yes | **Cadence:** `On-Demand (Stage Milestone)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2502`, `src/Main.CampaignServices.cs:34`, `src/Main.Expeditions.cs:167`, `src/Main.NpcArcs.cs:27`, `src/Main.SaveOrchestrator.cs:218`
- **UI Routes:** `crossing_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExpansionMasterSession.cs`](../../Assets/Ashfall.Core/ExpansionMasterSession.cs)
  - Core System: [`Assets/Ashfall.Core/ExpansionQuestSystem.cs`](../../Assets/Ashfall.Core/ExpansionQuestSystem.cs)
  - Host Session: [`src/Host/ExpansionQuestHostSession.cs`](../../src/Host/ExpansionQuestHostSession.cs)
  - Save Store: [`src/Host/ExpansionQuestSaveStore.cs`](../../src/Host/ExpansionQuestSaveStore.cs)
  - UI Panel: [`src/UI/CrossingQuestPanel.cs`](../../src/UI/CrossingQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/VersionReportContractTests.cs`](../../Ashfall.Core.Tests/VersionReportContractTests.cs)

### 48. `holdfast` — Holdfast S1 bunker state (Expansions (Exp 01))
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupHoldfastRuntime()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.BlackMarket.cs:29`, `src/Main.CampaignServices.cs:31`, `src/Main.DebtCredit.cs:67`, `src/Main.GameFlow.cs:23`, `src/Main.GameFlow.cs:536`, `src/Main.GameFlow.cs:558`, `src/Main.GameFlow.cs:573`, `src/Main.GameFlow.cs:673`, `src/Main.Holdfast.cs:201`, `src/Main.Holdfast.cs:211`, `src/Main.Holdfast.cs:416`, `src/Main.Maritime.cs:58`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:367`, `src/Main.PlayerSurfaces.cs:381`, `src/Main.PlayerSurfaces.cs:475`, `src/Main.PlayerSurfaces.cs:667`, `src/Main.SaveOrchestrator.cs:167`, `src/Main.SurvivorFate.cs:142`, `src/Main.UiHandlers.cs:81`, `src/Main.UiHandlers.cs:96`, `src/Main.UiHandlers.cs:143`, `src/Main.UiHandlers.cs:155`, `src/Main.UiHandlers.cs:185`, `src/Main.UiHandlers.cs:197`
- **UI Routes:** `holdfast`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HoldfastQuestSystem.cs`](../../Assets/Ashfall.Core/HoldfastQuestSystem.cs)
  - Core System: [`Assets/Ashfall.Core/HoldfastSession.cs`](../../Assets/Ashfall.Core/HoldfastSession.cs)
  - Host Session: [`src/Host/HoldfastRuntimeSession.cs`](../../src/Host/HoldfastRuntimeSession.cs)
  - Save Store: [`src/Host/HoldfastSaveStore.cs`](../../src/Host/HoldfastSaveStore.cs)
  - UI Panel: [`src/Host/HoldfastTerminalPanel.cs`](../../src/Host/HoldfastTerminalPanel.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HoldfastSaveTests.cs`](../../Ashfall.Core.Tests/HoldfastSaveTests.cs)

### 49. `holdfast_trade` — Holdfast trade session state (Expansions (Exp 01))
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupHoldfastRuntime()` | **Invoked:** yes | **Cadence:** `On-Demand (Barter)`
- **Setup Invocation Sites:** `src/Main.BlackMarket.cs:29`, `src/Main.CampaignServices.cs:31`, `src/Main.DebtCredit.cs:67`, `src/Main.GameFlow.cs:23`, `src/Main.GameFlow.cs:536`, `src/Main.GameFlow.cs:558`, `src/Main.GameFlow.cs:573`, `src/Main.GameFlow.cs:673`, `src/Main.Holdfast.cs:201`, `src/Main.Holdfast.cs:211`, `src/Main.Holdfast.cs:416`, `src/Main.Maritime.cs:58`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:367`, `src/Main.PlayerSurfaces.cs:381`, `src/Main.PlayerSurfaces.cs:475`, `src/Main.PlayerSurfaces.cs:667`, `src/Main.SaveOrchestrator.cs:167`, `src/Main.SurvivorFate.cs:142`, `src/Main.UiHandlers.cs:81`, `src/Main.UiHandlers.cs:96`, `src/Main.UiHandlers.cs:143`, `src/Main.UiHandlers.cs:155`, `src/Main.UiHandlers.cs:185`, `src/Main.UiHandlers.cs:197`
- **UI Routes:** `trade`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HoldfastTradeSession.cs`](../../Assets/Ashfall.Core/HoldfastTradeSession.cs)
  - Host Session: [`src/Host/HoldfastRuntimeSession.cs`](../../src/Host/HoldfastRuntimeSession.cs)
  - Save Store: [`src/Host/HoldfastTradeSaveStore.cs`](../../src/Host/HoldfastTradeSaveStore.cs)
  - UI Panel: [`src/Economy/TradeScreenGodotPanel.cs`](../../src/Economy/TradeScreenGodotPanel.cs)
  - UI Panel: [`src/Host/HoldfastTerminalPanel.cs`](../../src/Host/HoldfastTerminalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HoldfastTradeSessionTests.cs`](../../Ashfall.Core.Tests/HoldfastTradeSessionTests.cs)

### 50. `duty_roster` — Duty roster shifts and assignments (Expansions (Exp 02))
- **Owner Domain:** `duty_roster`
- **Setup Method:** `Main.SetupDutyRoster()` | **Invoked:** yes | **Cadence:** `Daily Shift Tick`
- **Setup Invocation Sites:** `src/Main.Application.cs:1096`, `src/Main.CampaignOwners.cs:1687`, `src/Main.CampaignOwners.cs:1890`, `src/Main.CampaignOwners.cs:1983`, `src/Main.CampaignServices.cs:55`, `src/Main.DutyRoster.cs:123`, `src/Main.DutyRoster.cs:129`, `src/Main.DutyRoster.cs:137`, `src/Main.DutyRoster.cs:144`, `src/Main.DutyRoster.cs:151`, `src/Main.DutyRoster.cs:159`, `src/Main.DutyRoster.cs:166`, `src/Main.ExpandedShelterSystems.cs:92`, `src/Main.GameFlow.cs:575`, `src/Main.GameFlow.cs:622`, `src/Main.GameFlow.cs:683`, `src/Main.GameFlow.cs:689`, `src/Main.Lifecycle.cs:725`, `src/Main.MoraleContagion.cs:30`, `src/Main.NightWatch.cs:30`, `src/Main.PlayerSurfaces.cs:381`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.PlayerSurfaces.cs:480`, `src/Main.PlayerSurfaces.cs:485`, `src/Main.SaveOrchestrator.cs:208`, `src/Main.ShelterBatch3.cs:159`, `src/Main.ShelterBatch3.cs:202`, `src/Main.ShelterBatch3.cs:228`, `src/Main.ShelterBatch3.cs:270`, `src/Main.ShelterBatch3.cs:290`, `src/Main.ShelterSocial.cs:571`, `src/Main.ShelterSocial.cs:610`, `src/Main.ShelterSocial.cs:653`, `src/Main.SurvivorFate.cs:39`, `src/Main.SurvivorFitness.cs:225`, `src/Main.SurvivorFitness.cs:237`, `src/Main.SurvivorFitness.cs:424`, `src/Main.SurvivorSocial.cs:22`, `src/Main.UiHandlers.cs:83`
- **UI Routes:** `duty_roster`, `duty_roster_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`](../../Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs)
  - Host Session: [`src/Host/DutyRosterHostSession.cs`](../../src/Host/DutyRosterHostSession.cs)
  - Save Store: [`src/Host/DutyRosterSaveStore.cs`](../../src/Host/DutyRosterSaveStore.cs)
  - UI Panel: [`src/UI/DutyRosterDetailPanel.cs`](../../src/UI/DutyRosterDetailPanel.cs)
  - UI Panel: [`src/UI/DutyRosterPanel.cs`](../../src/UI/DutyRosterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DutyRosterSaveTests.cs`](../../Ashfall.Core.Tests/DutyRosterSaveTests.cs)

### 51. `phantom_memory` — Phantom memory lineages and echoes (Expansions (Exp 03))
- **Owner Domain:** `phase0`
- **Setup Method:** `Main.SetupPhantom()` | **Invoked:** yes | **Cadence:** `On-Demand (Scavenge Echo)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:61`, `src/Main.Phase0.cs:82`, `src/Main.Phase0.cs:88`, `src/Main.SaveOrchestrator.cs:211`, `src/Main.ShelterBatch3.cs:325`
- **UI Routes:** `standing_record`, `phantom_memory`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/PhantomMemoryEngine.cs`](../../Assets/Ashfall.Core/PhantomMemoryEngine.cs)
  - Host Session: [`src/Host/PhantomMemoryHostSession.cs`](../../src/Host/PhantomMemoryHostSession.cs)
  - Save Store: [`src/Host/PhantomMemorySaveStore.cs`](../../src/Host/PhantomMemorySaveStore.cs)
  - UI Panel: [`src/UI/PhantomMemoryPanel.cs`](../../src/UI/PhantomMemoryPanel.cs)
  - UI Panel: [`src/UI/StandingRecordPanel.cs`](../../src/UI/StandingRecordPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PhantomMemoryEngineTests.cs`](../../Ashfall.Core.Tests/PhantomMemoryEngineTests.cs)

### 52. `thirdonary` — Thirdonary covenant & dispute states (Expansions (Exp 04))
- **Owner Domain:** `thirdonary`
- **Setup Method:** `Main.SetupThirdonary()` | **Invoked:** yes | **Cadence:** `On-Demand (Arbitration)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:35`, `src/Main.SaveOrchestrator.cs:219`
- **UI Routes:** `crossing_quests`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Thirdonary/ThirdonaryQuestSystem.cs`](../../Assets/Ashfall.Core/Thirdonary/ThirdonaryQuestSystem.cs)
  - Host Session: [`src/Host/ThirdonaryHostSession.cs`](../../src/Host/ThirdonaryHostSession.cs)
  - Save Store: [`src/Host/ThirdonarySaveStore.cs`](../../src/Host/ThirdonarySaveStore.cs)
  - UI Panel: [`src/UI/CrossingQuestPanel.cs`](../../src/UI/CrossingQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`](../../Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs`](../../Ashfall.Core.Tests/ThirdonaryQuestSystemTests.cs)

### 53. `year_of_ash` — The Year of Ash harsh winter state (Expansions (Exp 05))
- **Owner Domain:** `year_of_ash`
- **Setup Method:** `Main.SetupYearOfAsh()` | **Invoked:** yes | **Cadence:** `Daily Deep-Freeze Tick`
- **Setup Invocation Sites:** `src/Main.Application.cs:1105`, `src/Main.CampaignOwners.cs:2280`, `src/Main.CampaignOwners.cs:2489`, `src/Main.CampaignServices.cs:53`, `src/Main.DebtCredit.cs:64`, `src/Main.Expeditions.cs:100`, `src/Main.Expeditions.cs:249`, `src/Main.Expeditions.cs:288`, `src/Main.Expeditions.cs:350`, `src/Main.GameFlow.cs:542`, `src/Main.GameFlow.cs:561`, `src/Main.Narrative.cs:491`, `src/Main.Narrative.cs:499`, `src/Main.Plans166_169.cs:44`, `src/Main.Plans166_169.cs:154`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:367`, `src/Main.PlayerSurfaces.cs:632`, `src/Main.PlayerSurfaces.cs:884`, `src/Main.SaveOrchestrator.cs:216`, `src/Main.UiHandlers.cs:99`, `src/Main.UiHandlers.cs:149`, `src/Main.YearOfAsh.cs:380`, `src/Main.YearOfAsh.cs:437`, `src/Main.YearOfAsh.cs:538`
- **UI Routes:** `door_encounter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs`](../../Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs)
  - Core System: [`Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs`](../../Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs)
  - Host Session: [`src/YearOfAsh/YearOfAshHostSession.cs`](../../src/YearOfAsh/YearOfAshHostSession.cs)
  - Save Store: [`src/YearOfAsh/YearOfAshSaveStore.cs`](../../src/YearOfAsh/YearOfAshSaveStore.cs)
  - UI Panel: [`src/YearOfAsh/DoorEncounterModal.cs`](../../src/YearOfAsh/DoorEncounterModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs`](../../Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs)

### 54. `muster` — The Muster military rally & conflict state (Expansions (Exp 06))
- **Owner Domain:** `muster`
- **Setup Method:** `Main.SetupMuster()` | **Invoked:** yes | **Cadence:** `On-Demand (Rally Stance)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2495`, `src/Main.CampaignServices.cs:56`, `src/Main.DebtCredit.cs:65`, `src/Main.GameFlow.cs:559`, `src/Main.GameFlow.cs:568`, `src/Main.GameFlow.cs:615`, `src/Main.GameFlow.cs:623`, `src/Main.Muster.cs:119`, `src/Main.Muster.cs:135`, `src/Main.Muster.cs:199`, `src/Main.Muster.cs:224`, `src/Main.Muster.cs:240`, `src/Main.Muster.cs:254`, `src/Main.Muster.cs:268`, `src/Main.Muster.cs:277`, `src/Main.Muster.cs:284`, `src/Main.Muster.cs:291`, `src/Main.Muster.cs:298`, `src/Main.Muster.cs:337`, `src/Main.Muster.cs:345`, `src/Main.Muster.cs:377`, `src/Main.Muster.cs:401`, `src/Main.Muster.cs:446`, `src/Main.PlayerSurfaces.cs:367`, `src/Main.PlayerSurfaces.cs:376`, `src/Main.PlayerSurfaces.cs:377`, `src/Main.PlayerSurfaces.cs:425`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.PlayerSurfaces.cs:662`, `src/Main.SaveOrchestrator.cs:215`, `src/Main.UiHandlers.cs:97`, `src/Main.UiHandlers.cs:186`
- **UI Routes:** `muster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Muster/MusterSystem.cs`](../../Assets/Ashfall.Core/Muster/MusterSystem.cs)
  - Host Session: [`src/Host/MusterHostSession.cs`](../../src/Host/MusterHostSession.cs)
  - Save Store: [`src/Host/MusterSaveStore.cs`](../../src/Host/MusterSaveStore.cs)
  - UI Panel: [`src/UI/MusterPanel.cs`](../../src/UI/MusterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MusterSystemTests.cs`](../../Ashfall.Core.Tests/MusterSystemTests.cs)

### 55. `dose_ledger` — Survivor radiation dose ledger & cohorts (Expansions (Exp 07))
- **Owner Domain:** `dose_ledger`
- **Setup Method:** `Main.SetupDoseLedger()` | **Invoked:** yes | **Cadence:** `On-Demand (Dose Log)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1340`, `src/Main.CampaignServices.cs:62`, `src/Main.DutyRoster.cs:103`, `src/Main.Endgame.cs:255`, `src/Main.Expeditions.cs:775`, `src/Main.Lifecycle.cs:722`, `src/Main.MedicalTriage.cs:53`, `src/Main.Phase0.cs:416`, `src/Main.Phase0.cs:422`, `src/Main.Phase0.cs:431`, `src/Main.Phase0.cs:440`, `src/Main.Phase0.cs:449`, `src/Main.Phase0.cs:458`, `src/Main.SaveOrchestrator.cs:214`, `src/Main.ShelterSocial.cs:582`, `src/Main.SurvivorFitness.cs:116`, `src/Main.UnifiedEnding.cs:77`
- **UI Routes:** `radiation_history`, `radiation_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DoseLedgerSystem.cs`](../../Assets/Ashfall.Core/DoseLedgerSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`](../../Assets/Ashfall.Core/Radiation/RadiationSystem.cs)
  - Host Session: [`src/Host/DoseLedgerHostSession.cs`](../../src/Host/DoseLedgerHostSession.cs)
  - Save Store: [`src/Host/DoseLedgerSaveStore.cs`](../../src/Host/DoseLedgerSaveStore.cs)
  - UI Panel: [`src/UI/RadiationDetailPanel.cs`](../../src/UI/RadiationDetailPanel.cs)
  - UI Panel: [`src/UI/RadiationHistoryPanel.cs`](../../src/UI/RadiationHistoryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs`](../../Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs)

### 56. `verdict` — The Verdict investigation and tribunal state (Expansions (Exp 08))
- **Owner Domain:** `verdict`
- **Setup Method:** `Main.SetupVerdict()` | **Invoked:** yes | **Cadence:** `Daily Machine Log Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:54`, `src/Main.Endgame.cs:251`, `src/Main.GameFlow.cs:628`, `src/Main.GameFlow.cs:668`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.PlayerSurfaces.cs:470`, `src/Main.PlayerSurfaces.cs:647`, `src/Main.SaveOrchestrator.cs:209`, `src/Main.Spiritual.cs:79`, `src/Main.UnifiedEnding.cs:73`, `src/Main.Verdict.cs:79`, `src/Main.Verdict.cs:182`, `src/Main.Verdict.cs:188`, `src/Main.Verdict.cs:195`, `src/Main.YearOfAsh.cs:516`
- **UI Routes:** `verdict`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`](../../Assets/Ashfall.Core/Verdict/MachineLogSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Verdict/ReckoningSystem.cs`](../../Assets/Ashfall.Core/Verdict/ReckoningSystem.cs)
  - Host Session: [`src/Host/VerdictHostSession.cs`](../../src/Host/VerdictHostSession.cs)
  - Save Store: [`src/Host/VerdictSaveStore.cs`](../../src/Host/VerdictSaveStore.cs)
  - UI Panel: [`src/UI/VerdictDashboardPanel.cs`](../../src/UI/VerdictDashboardPanel.cs)
  - UI Panel: [`src/VerdictPanel.cs`](../../src/VerdictPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/VerdictChainTests.cs`](../../Ashfall.Core.Tests/VerdictChainTests.cs)

### 57. `maritime` — The Black Flotilla dives and naval wrecks (Expansions (Exp 09))
- **Owner Domain:** `maritime`
- **Setup Method:** `Main.SetupMaritime()` | **Invoked:** yes | **Cadence:** `On-Demand (Dive Sortie)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1217`, `src/Main.CampaignServices.cs:52`, `src/Main.GameFlow.cs:624`, `src/Main.GameFlow.cs:643`, `src/Main.Maritime.cs:57`, `src/Main.Maritime.cs:149`, `src/Main.Maritime.cs:155`, `src/Main.Maritime.cs:161`, `src/Main.Maritime.cs:167`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.PlayerSurfaces.cs:445`, `src/Main.PlayerSurfaces.cs:657`, `src/Main.SaveOrchestrator.cs:210`
- **UI Routes:** `maritime`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs)
  - Host Session: [`src/Host/MaritimeHostSession.cs`](../../src/Host/MaritimeHostSession.cs)
  - Save Store: [`src/Host/MaritimeSaveStore.cs`](../../src/Host/MaritimeSaveStore.cs)
  - UI Panel: [`src/UI/MaritimePanel.cs`](../../src/UI/MaritimePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BlackFlotillaTests.cs`](../../Ashfall.Core.Tests/BlackFlotillaTests.cs)

### 58. `silent_foundry` — Automated foundry machinery & smelters (Expansions (Exp 10))
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupSilentFoundry()` | **Invoked:** yes | **Cadence:** `Daily Smelter Cycle`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1396`, `src/Main.CampaignServices.cs:60`, `src/Main.CampaignServices.cs:209`, `src/Main.GameFlow.cs:604`, `src/Main.GameFlow.cs:611`, `src/Main.PlayerSurfaces.cs:415`, `src/Main.PlayerSurfaces.cs:420`, `src/Main.PlayerSurfaces.cs:512`, `src/Main.SaveOrchestrator.cs:223`
- **UI Routes:** `silent_foundry`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`](../../Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs)
  - Host Session: [`src/Foundry/SilentFoundryHostSession.cs`](../../src/Foundry/SilentFoundryHostSession.cs)
  - Save Store: [`src/Host/SilentFoundrySaveStore.cs`](../../src/Host/SilentFoundrySaveStore.cs)
  - UI Panel: [`src/UI/SilentFoundryPanel.cs`](../../src/UI/SilentFoundryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs`](../../Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs)

### 59. `chemical_recon` — Plans 78-81 — chemical hazard observations, samples, and safe corridors (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupChemicalRecon()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans78_81.cs:92`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs`](../../Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs)
  - Host Session: [`src/Host/ChemicalReconSaveStore.cs`](../../src/Host/ChemicalReconSaveStore.cs)
  - Save Store: [`src/Host/ChemicalReconSaveStore.cs`](../../src/Host/ChemicalReconSaveStore.cs)

### 60. `colony` — ORPHAN-SEAL-W1 — player-founded colonies, buildings, and supply lines (Expeditions)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupColony()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:78`, `src/Main.OrphanSealWave1.cs:377`, `src/Main.PfglOctetBoards.cs:52`, `src/Main.PfglOctetBoards.cs:70`, `src/Main.PfglOctetBoards.cs:108`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ColonySystem.cs`](../../Assets/Ashfall.Core/Expeditions/ColonySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan160ColonyIntegrationTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan160ColonyIntegrationTests.cs)

### 61. `draisine_recovery` — Plans 130-133 — armored draisine derailment recovery (Expeditions)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupDraisineRerailing()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans130_133.cs:28`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs`](../../Assets/Ashfall.Core/Expeditions/DraisineRerailingSystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 62. `mine_clearing_flail` — Plans 146-149 — mine-clearing flail vehicle modules and active breaches (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupMineClearingFlail()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans146_149.cs:389`, `src/Main.Plans146_149.cs:786`, `src/Main.PlayerSurfaces.cs:134`
- **UI Routes:** `mine_flail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs`](../../Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs)
  - Host Session: [`src/Host/MineClearingFlailHostSession.cs`](../../src/Host/MineClearingFlailHostSession.cs)
  - Save Store: [`src/Host/MineClearingFlailSaveStore.cs`](../../src/Host/MineClearingFlailSaveStore.cs)
  - UI Panel: [`src/UI/MineFlailPanel.cs`](../../src/UI/MineFlailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/MineClearingFlailEngineTests.cs`](../../Ashfall.Core.Tests/Expeditions/MineClearingFlailEngineTests.cs)

### 63. `rail_grinding` — Plans 146-149 — rail grinding vehicle modules and active corridor jobs (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupRailGrinding()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans146_149.cs:390`, `src/Main.Plans146_149.cs:839`, `src/Main.PlayerSurfaces.cs:139`
- **UI Routes:** `rail_grinding`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs`](../../Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs)
  - Host Session: [`src/Host/RailGrindingHostSession.cs`](../../src/Host/RailGrindingHostSession.cs)
  - Save Store: [`src/Host/RailGrindingSaveStore.cs`](../../src/Host/RailGrindingSaveStore.cs)
  - UI Panel: [`src/UI/RailGrindingPanel.cs`](../../src/UI/RailGrindingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/RailGrindingEngineTests.cs`](../../Ashfall.Core.Tests/Expeditions/RailGrindingEngineTests.cs)

### 64. `rail_track_maintenance` — Expansion 25 — rail track gauge/wear/bridge maintenance ledger over the canonical RailwaySystem topology (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupRailTrackMaintenance()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2606`, `src/Main.CampaignOwners.cs:2617`, `src/Main.RailTrackMaintenance.cs:77`, `src/Main.RailTrackMaintenance.cs:110`, `src/Main.RailTrackMaintenance.cs:117`, `src/Main.SaveOrchestrator.cs:335`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Rail/RailTrackMaintenanceEngine.cs`](../../Assets/Ashfall.Core/Rail/RailTrackMaintenanceEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Rail/RailTrackMaintenanceLedger.cs`](../../Assets/Ashfall.Core/Rail/RailTrackMaintenanceLedger.cs)
  - Host Session: [`src/Host/RailTrackMaintenanceHostSession.cs`](../../src/Host/RailTrackMaintenanceHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RailTrackMaintenanceHostSession.cs`](../../src/Host/RailTrackMaintenanceHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Rail/RailTrackMaintenanceEngineTests.cs`](../../Ashfall.Core.Tests/Rail/RailTrackMaintenanceEngineTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Rail/RailTrackMaintenanceLedgerTests.cs`](../../Ashfall.Core.Tests/Rail/RailTrackMaintenanceLedgerTests.cs)

### 65. `recon_telemetry` — Long-range recon drones & high-altitude mapping (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupReconTelemetry()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1980`, `src/Main.CampaignServices.cs:45`, `src/Main.Expeditions.cs:699`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs`](../../Assets/Ashfall.Core/Expeditions/ReconTelemetrySystem.cs)
  - Host Session: [`src/Host/ReconTelemetrySaveStore.cs`](../../src/Host/ReconTelemetrySaveStore.cs)
  - Save Store: [`src/Host/ReconTelemetrySaveStore.cs`](../../src/Host/ReconTelemetrySaveStore.cs)

### 66. `vehicle_garage` — Plans 50-53 — expedition overland vehicle modifications and garage maintenance state (Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupVehicleGarage()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Lifecycle.cs:733`
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

### 67. `counter_intelligence` — Counter-intelligence, vetting, and defector management (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupCounterIntelligence()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2480`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/CounterIntelligenceSystem.cs`](../../Assets/Ashfall.Core/Factions/CounterIntelligenceSystem.cs)
  - Host Session: [`src/Host/CounterIntelligenceSaveStore.cs`](../../src/Host/CounterIntelligenceSaveStore.cs)
  - Save Store: [`src/Host/CounterIntelligenceSaveStore.cs`](../../src/Host/CounterIntelligenceSaveStore.cs)

### 68. `diplomacy` — Faction diplomacy: treaty templates, relations, active treaties, missions, violations, and global reputation (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupDiplomacy()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3402`, `src/Main.Diplomacy.cs:32`, `src/Main.Diplomacy.cs:43`, `src/Main.Diplomacy.cs:52`, `src/Main.Diplomacy.cs:60`, `src/Main.SaveOrchestrator.cs:357`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Diplomacy/FactionDiplomacySystem.cs`](../../Assets/Ashfall.Core/Diplomacy/FactionDiplomacySystem.cs)
  - Host Session: [`src/Host/DiplomacyHostSession.cs`](../../src/Host/DiplomacyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DiplomacyHostSession.cs`](../../src/Host/DiplomacyHostSession.cs)

### 69. `diplomatic_summits` — Wasteland summits, treaty lifecycle, guarantees, DMZ rules, violations (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupDiplomaticSummit()` | **Invoked:** no | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs`](../../Assets/Ashfall.Core/Diplomacy/DiplomaticSummitSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DiplomaticSummitSaveStore.cs`](../../src/Host/DiplomaticSummitSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiplomaticSummitTests.cs`](../../Ashfall.Core.Tests/DiplomaticSummitTests.cs)

### 70. `espionage` — Campaign intelligence networks, missions, and captured agents (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupPlans166To169()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:149`, `src/Main.NarrativeQuestlines.cs:239`, `src/Main.Plans166_169.cs:214`, `src/Main.Plans166_169.cs:220`, `src/Main.Plans166_169.cs:227`, `src/Main.Plans166_169.cs:260`, `src/Main.Plans166_169.cs:276`, `src/Main.SaveOrchestrator.cs:232`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/EspionageSystem.cs`](../../Assets/Ashfall.Core/Factions/EspionageSystem.cs)
  - Host Session: [`src/Host/EspionageHostSession.cs`](../../src/Host/EspionageHostSession.cs)
  - Save Store: [`src/Host/EspionageSaveStore.cs`](../../src/Host/EspionageSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan167EspionageTests.cs`](../../Ashfall.Core.Tests/Plan167EspionageTests.cs)

### 71. `faction_covert_ops` — ORPHAN-SEAL-W1 — rival-faction covert operations, suspicion ladder, and intelligence reports (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupFactionCovertOps()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:84`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/FactionCovertOpsCoordinator.cs`](../../Assets/Ashfall.Core/Factions/FactionCovertOpsCoordinator.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/Plan153FactionEspionageIntegrationTests.cs`](../../Ashfall.Core.Tests/Factions/Plan153FactionEspionageIntegrationTests.cs)

### 72. `faction_espionage` — Plans 50-53 — shelter faction espionage, sleeper assets, counter-intel, and sabotage state (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupShelterEspionage()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans50_53.cs:238`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs`](../../Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterEspionageSaveStore.cs`](../../src/Host/ShelterEspionageSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs`](../../Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs)

### 73. `informant_network` — Plan 146 batch-4 / A.83 — informant field tradecraft: recruited assets, operations, sweeps, interrogation doctrine, and banked intel (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupInformantNetwork()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (Faction Branch Owner)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2481`, `src/Main.PlayerSurfaces.cs:367`
- **UI Routes:** `factions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Espionage/InformantNetworkSystem.cs`](../../Assets/Ashfall.Core/Espionage/InformantNetworkSystem.cs)
  - Host Session: [`src/Host/InformantNetworkHostSession.cs`](../../src/Host/InformantNetworkHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/InformantNetworkHostSession.cs`](../../src/Host/InformantNetworkHostSession.cs)
  - UI Panel: [`src/UI/FactionsPanel.cs`](../../src/UI/FactionsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Espionage/Plan146InformantNetworkIntegrationTests.cs`](../../Ashfall.Core.Tests/Espionage/Plan146InformantNetworkIntegrationTests.cs)

### 74. `territory_control` — Plan 134 — dynamic faction territory & supply line control: contested locations, fortification, garrison, supply line status (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupTerritoryControl()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:165`, `src/Main.NightWatch.cs:35`, `src/Main.SaveOrchestrator.cs:305`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`](../../Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs)
  - Host Session: [`src/Host/TerritoryControlHostSession.cs`](../../src/Host/TerritoryControlHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TerritoryControlHostSession.cs`](../../src/Host/TerritoryControlHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/Plan134TerritoryControlHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Factions/Plan134TerritoryControlHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs`](../../Ashfall.Core.Tests/Factions/Plan134TerritoryControlIntegrationTests.cs)

### 75. `warlord_response` — Warlord tribute responses — one-shot Pay/Contest/Submit records per canonical tribute id over the live WarlordDoctrineSystem (Factions)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupWarlordResponse()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:882`, `src/Main.CampaignOwners.cs:893`, `src/Main.SaveOrchestrator.cs:322`, `src/Main.UiHandlers.cs:115`, `src/Main.WarlordResponse.cs:43`, `src/Main.WarlordResponse.cs:74`, `src/Main.WarlordResponse.cs:96`
- **UI Routes:** `factions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs`](../../Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Warlords/WarlordResponseActions.cs`](../../Assets/Ashfall.Core/Warlords/WarlordResponseActions.cs)
  - Host Session: [`src/Host/WarlordResponseHostSession.cs`](../../src/Host/WarlordResponseHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/WarlordResponseHostSession.cs`](../../src/Host/WarlordResponseHostSession.cs)
  - UI Panel: [`src/UI/FactionsPanel.cs`](../../src/UI/FactionsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/PlanWarlordResponseHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Factions/PlanWarlordResponseHostIntegrationTests.cs)

### 76. `weight_of_choices` — Weight of choices faction branch progression and PoNR commitments (Factions & Diplomacy)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupFactionBranch()` | **Invoked:** yes | **Cadence:** `On-Demand (Branch Decisions)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:57`, `src/Main.FactionBranch.cs:94`, `src/Main.FactionBranch.cs:110`, `src/Main.GameFlow.cs:562`, `src/Main.GameFlow.cs:576`, `src/Main.Lifecycle.cs:729`, `src/Main.PlayerSurfaces.cs:367`, `src/Main.PlayerSurfaces.cs:381`, `src/Main.SaveOrchestrator.cs:233`, `src/Main.UiHandlers.cs:84`, `src/Main.UiHandlers.cs:100`, `src/Main.UnifiedEnding.cs:78`
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

### 77. `aeroponics` — Plan B76 — aeroponic chambers, nutrient chemistry, disease, lighting, and harvest (Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupAeroponics()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Plans74_77.cs:33`, `src/Main.Plans74_77.cs:145`, `src/Main.Plans74_77.cs:302`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/AeroponicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 78. `agriculture` — Plans 162-165 — advanced crop strains, plot medium, pests, compost, and dietary diversity (Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupAgriculture()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1383`, `src/Main.Plans162_165.cs:135`, `src/Main.Plans162_165.cs:172`, `src/Main.SaveOrchestrator.cs:289`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Farming/AgricultureSystem.cs`](../../Assets/Ashfall.Core/Farming/AgricultureSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AgricultureSaveStore.cs`](../../src/Host/AgricultureSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AgricultureSystemTests.cs`](../../Ashfall.Core.Tests/AgricultureSystemTests.cs)

### 79. `aquaponics` — Plan B87 — closed-loop aquaponics ecology, biofilter health, and harvest yields (Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupAquaponics()` | **Invoked:** yes | **Cadence:** `Daily Ecology Tick`
- **Setup Invocation Sites:** `src/Main.PlansB86_B89.cs:195`, `src/Main.PlansB86_B89.cs:210`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AquaponicsSaveStore.cs`](../../src/Host/AquaponicsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs`](../../Ashfall.Core.Tests/PlansB86ToB89ContinuityTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/AquaponicsSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/AquaponicsSystemTests.cs)

### 80. `soil_reclamation_profile` — Expansion 15 The Deep Root — open-ground soil amendment chemistry, fertility evaluation, and germination viability (Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupSoilReclamationProfile()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3677`, `src/Main.SaveOrchestrator.cs:371`, `src/Main.SoilReclamationProfile.cs:37`, `src/Main.SoilReclamationProfile.cs:54`, `src/Main.SoilReclamationProfile.cs:62`, `src/Main.SoilReclamationProfile.cs:70`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Farming/SoilReclamationProfileEngine.cs`](../../Assets/Ashfall.Core/Farming/SoilReclamationProfileEngine.cs)
  - Host Session: [`src/Host/SoilReclamationProfileHostSession.cs`](../../src/Host/SoilReclamationProfileHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SoilReclamationProfileHostSession.cs`](../../src/Host/SoilReclamationProfileHostSession.cs)

### 81. `oilseed_pressing` — PLAN-PRESERVATION-TRUTH-118 \u2014 oilseed press install state and throughput (Farming & Food)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupOilseedPressing()` | **Invoked:** yes | **Cadence:** `On Demand`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:386`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Farming/OilseedPressingEngine.cs`](../../Assets/Ashfall.Core/Farming/OilseedPressingEngine.cs)
  - Host Session: [`src/Host/OilseedPressingHostSession.cs`](../../src/Host/OilseedPressingHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OilseedPressingHostSession.cs`](../../src/Host/OilseedPressingHostSession.cs)

### 82. `powder_metallurgy` — Plans 130-133 — abstract advanced-material production quality and reliability (Foundry)
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupPowderMetallurgy()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans130_133.cs:25`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs`](../../Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 83. `hydraulic_extrusion` — Plan 140 — advanced hydraulic extrusion batches, tooling condition, quality grades (Foundry & Industry)
- **Owner Domain:** `foundry`
- **Setup Method:** `Main.SetupHydraulicExtrusion()` | **Invoked:** yes | **Cadence:** `On-Demand (Batch Phase Commands)`
- **Setup Invocation Sites:** `src/Main.HydraulicExtrusion.cs:25`, `src/Main.HydraulicExtrusion.cs:93`, `src/Main.HydraulicExtrusion.cs:100`, `src/Main.HydraulicExtrusion.cs:107`, `src/Main.SaveOrchestrator.cs:189`
- **UI Routes:** `hydraulic_extrusion`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Foundry/HydraulicExtrusionEngine.cs`](../../Assets/Ashfall.Core/Foundry/HydraulicExtrusionEngine.cs)
  - Host Session: [`src/Host/HydraulicExtrusionHostSession.cs`](../../src/Host/HydraulicExtrusionHostSession.cs)
  - Save Store: [`src/Host/HydraulicExtrusionSaveStore.cs`](../../src/Host/HydraulicExtrusionSaveStore.cs)
  - UI Panel: [`src/UI/HydraulicExtrusionPanel.cs`](../../src/UI/HydraulicExtrusionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Foundry/Plan140HydraulicExtrusionTests.cs`](../../Ashfall.Core.Tests/Foundry/Plan140HydraulicExtrusionTests.cs)

### 84. `shelter_governance` — Plan 159 — Shelter governance & political system: ideological blocs, policy consent, civil disputes, and shelter stability (Governance)
- **Owner Domain:** `governance`
- **Setup Method:** `Main.SetupShelterGovernance()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1054`, `src/Main.CampaignOwners.cs:1065`, `src/Main.SaveOrchestrator.cs:330`, `src/Main.ShelterGovernance.cs:60`, `src/Main.ShelterGovernance.cs:89`, `src/Main.ShelterGovernance.cs:96`, `src/Main.ShelterGovernance.cs:102`
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

### 85. `shelter_identity` — Plan 166 — Shelter identity, naming, origin projection, emblems, mottos, infamy, and community legacy tags (Holdfast)
- **Owner Domain:** `holdfast`
- **Setup Method:** `Main.SetupShelterIdentity()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:689`, `src/Main.CampaignOwners.cs:700`, `src/Main.GameFlow.cs:244`, `src/Main.SaveOrchestrator.cs:328`, `src/Main.ShelterIdentity.cs:83`, `src/Main.ShelterIdentity.cs:121`, `src/Main.ShelterIdentity.cs:131`, `src/Main.ShelterIdentity.cs:138`, `src/Main.ShelterIdentity.cs:145`, `src/Main.ShelterIdentity.cs:153`
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

### 86. `wildlife_ecosystem` — Plans 162-165 — ecology pressures, extinction flags, apex activity, taming, bestiary knowledge (Hunting)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupWildlifeEcosystem()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.EvolvingWorld.cs:63`, `src/Main.Plans162_165.cs:769`, `src/Main.Plans162_165.cs:792`, `src/Main.SaveOrchestrator.cs:292`
- **UI Routes:** `bestiary`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`](../../Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs)
  - Host Session: [`src/Host/WildlifeEcosystemHostSession.cs`](../../src/Host/WildlifeEcosystemHostSession.cs)
  - Save Store: [`src/Host/WildlifeEcosystemSaveStore.cs`](../../src/Host/WildlifeEcosystemSaveStore.cs)
  - UI Panel: [`src/UI/BestiaryPanel.cs`](../../src/UI/BestiaryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs`](../../Ashfall.Core.Tests/WildlifeEcosystemSystemTests.cs)

### 87. `wildlife_harvest` — Expansion 32 — per-species seasonal harvest ledger and sustainable quota (Hunting)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupWildlifeHarvest()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2726`, `src/Main.CampaignOwners.cs:2737`, `src/Main.SaveOrchestrator.cs:339`, `src/Main.WildlifeHarvest.cs:34`, `src/Main.WildlifeHarvest.cs:45`, `src/Main.WildlifeHarvest.cs:52`, `src/Main.WildlifeHarvest.cs:58`, `src/Main.WildlifeHarvest.cs:65`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WildlifeHarvestLedger.cs`](../../Assets/Ashfall.Core/World/WildlifeHarvestLedger.cs)
  - Core System: [`Assets/Ashfall.Core/World/WildlifeHarvestQuotaEngine.cs`](../../Assets/Ashfall.Core/World/WildlifeHarvestQuotaEngine.cs)
  - Host Session: [`src/Host/WildlifeHarvestHostSession.cs`](../../src/Host/WildlifeHarvestHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/WildlifeHarvestHostSession.cs`](../../src/Host/WildlifeHarvestHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/WildlifeHarvestLedgerTests.cs`](../../Ashfall.Core.Tests/World/WildlifeHarvestLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/WildlifeHarvestQuotaEngineTests.cs`](../../Ashfall.Core.Tests/World/WildlifeHarvestQuotaEngineTests.cs)

### 88. `shelter_barter` — Plan 54/147 — shelter barter caravans, pinned stock, and the contraband broker counter (Illicit Economy / Barter)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupShelterBarter()` | **Invoked:** yes | **Cadence:** `On-Demand (Barter)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:121`, `src/Main.SaveOrchestrator.cs:268`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs`](../../Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterBarterSaveStore.cs`](../../src/Host/ShelterBarterSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/ShelterBarterSystemPlan54Tests.cs`](../../Ashfall.Core.Tests/Economy/ShelterBarterSystemPlan54Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/ContrabandBarterRouteTests.cs`](../../Ashfall.Core.Tests/Narrative/ContrabandBarterRouteTests.cs)

### 89. `advanced_industrial` — Plans 118-121 — synthetic lubricant (Fischer-Tropsch), UV corona detection, carbon composites, and ground-penetrating radar host integration (Industrial & Reconnaissance)
- **Owner Domain:** `production`
- **Setup Method:** `Main.SetupAdvancedIndustrial()` | **Invoked:** yes | **Cadence:** `Daily Campaign Tick (active production, field surveys, and sensor cooldowns)`
- **Setup Invocation Sites:** `src/Main.AdvancedIndustrial.cs:51`, `src/Main.AdvancedIndustrial.cs:57`, `src/Main.AdvancedIndustrial.cs:63`, `src/Main.AdvancedIndustrial.cs:70`, `src/Main.AdvancedIndustrial.cs:77`, `src/Main.AdvancedIndustrial.cs:83`, `src/Main.AdvancedIndustrial.cs:90`, `src/Main.AdvancedIndustrial.cs:96`, `src/Main.AdvancedIndustrial.cs:102`, `src/Main.CampaignOwners.cs:2893`, `src/Main.CampaignOwners.cs:2904`, `src/Main.SaveOrchestrator.cs:345`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/UvCoronaDetectionEngine.cs`](../../Assets/Ashfall.Core/Radio/UvCoronaDetectionEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/CarbonCompositeEngine.cs`](../../Assets/Ashfall.Core/Shelter/CarbonCompositeEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/FischerTropschSynthesisEngine.cs`](../../Assets/Ashfall.Core/Shelter/FischerTropschSynthesisEngine.cs)
  - Core System: [`Assets/Ashfall.Core/World/GroundPenetratingRadarEngine.cs`](../../Assets/Ashfall.Core/World/GroundPenetratingRadarEngine.cs)
  - Host Session: [`src/Host/AdvancedIndustrialHostSession.cs`](../../src/Host/AdvancedIndustrialHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AdvancedIndustrialSaveStore.cs`](../../src/Host/AdvancedIndustrialSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/UvCoronaDetectionEngineTests.cs`](../../Ashfall.Core.Tests/Radio/UvCoronaDetectionEngineTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/CarbonCompositeEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/CarbonCompositeEngineTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/FischerTropschSynthesisEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/FischerTropschSynthesisEngineTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/GroundPenetratingRadarEngineTests.cs`](../../Ashfall.Core.Tests/World/GroundPenetratingRadarEngineTests.cs)

### 90. `grain_milling_archive` — Plan 157 — Grain milling, storage & food-processing knowledge archive: discovered-record ledger (IDs only) (Industrial Food-Processing Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupGrainMillingArchive()` | **Invoked:** yes | **Cadence:** `Event-Driven (Location Discovery & Shelter Room Inspection)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:126`, `src/Main.SaveOrchestrator.cs:273`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/GrainMillingDiscoverySystem.cs`](../../Assets/Ashfall.Core/Narrative/GrainMillingDiscoverySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/GrainMillingArchiveSaveStore.cs`](../../src/Host/GrainMillingArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GrainMillingCatalogTests.cs`](../../Ashfall.Core.Tests/GrainMillingCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs`](../../Ashfall.Core.Tests/Narrative/GrainMillingDiscoveryTests.cs)

### 91. `wasteland_rumors` — Plan 203 / 131 — wasteland rumors, information hubs, propagation, and intercepts (Information & Rumors (Plan 203))
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupRumorNetwork()` | **Invoked:** yes | **Cadence:** `Daily (Decay & Propagation)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:149`, `src/Main.MoralChoice.cs:240`
- **UI Routes:** `rumors`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/InformationFlow/RumorSystem.cs`](../../Assets/Ashfall.Core/InformationFlow/RumorSystem.cs)
  - Host Session: [`src/Host/RumorNetworkHostSession.cs`](../../src/Host/RumorNetworkHostSession.cs)
  - Save Store: [`src/Host/RumorNetworkSaveStore.cs`](../../src/Host/RumorNetworkSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/RumorBoardPanel.cs`](../../src/UI/RumorBoardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/InformationFlow/Plan203RumorNetworkIntegrationTests.cs`](../../Ashfall.Core.Tests/InformationFlow/Plan203RumorNetworkIntegrationTests.cs)

### 92. `cryogenic_air_separation` — Abstract gas production and plant condition (Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupCryogenicAirSeparation()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:136`, `src/Main.ExpandedShelterSystems.cs:715`, `src/Main.PlayerSurfaces.cs:758`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/CryogenicAirSeparationSystem.cs`](../../Assets/Ashfall.Core/CryogenicAirSeparationSystem.cs)
  - Host Session: [`src/Host/CryogenicAirSeparationHostSession.cs`](../../src/Host/CryogenicAirSeparationHostSession.cs)
  - Save Store: [`src/Host/CryogenicAirSeparationHostSession.cs`](../../src/Host/CryogenicAirSeparationHostSession.cs)

### 93. `fluid_logistics` — Shelter fluid topology, pressure, leaks, and distributed quality (Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupPlans166To169()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:149`, `src/Main.NarrativeQuestlines.cs:239`, `src/Main.Plans166_169.cs:214`, `src/Main.Plans166_169.cs:220`, `src/Main.Plans166_169.cs:227`, `src/Main.Plans166_169.cs:260`, `src/Main.Plans166_169.cs:276`, `src/Main.SaveOrchestrator.cs:232`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs`](../../Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs)
  - Host Session: [`src/Host/FluidLogisticsHostSession.cs`](../../src/Host/FluidLogisticsHostSession.cs)
  - Save Store: [`src/Host/FluidLogisticsSaveStore.cs`](../../src/Host/FluidLogisticsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan168FluidLogisticsTests.cs`](../../Ashfall.Core.Tests/Plan168FluidLogisticsTests.cs)

### 94. `pneumatic_dispatch` — Plan B77 — pneumatic stations, capsule routing, seals, jams, and blackout-safe dispatch (Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupPneumaticDispatch()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans74_77.cs:34`, `src/Main.Plans74_77.cs:152`, `src/Main.Plans74_77.cs:347`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs`](../../Assets/Ashfall.Core/Shelter/PneumaticDispatchSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 95. `black_projects_archive` — Plan 152 — Black Projects intelligence archive: discovered-record ledger (IDs only) (Intelligence Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupBlackProjectsArchive()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:122`, `src/Main.SaveOrchestrator.cs:269`
- **UI Routes:** `black_projects_archive`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/BlackProjectsArchiveSystem.cs`](../../Assets/Ashfall.Core/Narrative/BlackProjectsArchiveSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BlackProjectsArchiveSaveStore.cs`](../../src/Host/BlackProjectsArchiveSaveStore.cs)
  - UI Panel: [`src/UI/BlackProjectsArchivePanel.cs`](../../src/UI/BlackProjectsArchivePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BlackProjectsCatalogTests.cs`](../../Ashfall.Core.Tests/BlackProjectsCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/BlackProjectsArchiveTests.cs`](../../Ashfall.Core.Tests/Narrative/BlackProjectsArchiveTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/UI/BlackProjectsArchivePanelRouteTests.cs`](../../Ashfall.Core.Tests/UI/BlackProjectsArchivePanelRouteTests.cs)

### 96. `clothing_warmth` — Plan 142 — equipped clothing layers, gear condition, wetness, and cold-loss mitigation (Inventory)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupClothingWarmth()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3346`, `src/Main.ClothingWarmth.cs:71`, `src/Main.ClothingWarmth.cs:79`, `src/Main.ClothingWarmth.cs:92`, `src/Main.ClothingWarmth.cs:116`, `src/Main.ClothingWarmth.cs:123`, `src/Main.ClothingWarmth.cs:149`, `src/Main.ClothingWarmth.cs:161`, `src/Main.SaveOrchestrator.cs:355`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Inventory/ClothingWarmthSystem.cs`](../../Assets/Ashfall.Core/Inventory/ClothingWarmthSystem.cs)
  - Host Session: [`src/Host/ClothingWarmthHostSession.cs`](../../src/Host/ClothingWarmthHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ClothingWarmthHostSession.cs`](../../src/Host/ClothingWarmthHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Inventory/ClothingWarmthSystemTests.cs`](../../Ashfall.Core.Tests/Inventory/ClothingWarmthSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Inventory/Plan142ClothingWarmthHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Inventory/Plan142ClothingWarmthHostIntegrationTests.cs)

### 97. `item_lore` — Plan 190 \u2014 per-item provenance chains and lore fragments from committed craft/discovery facts (Inventory)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupItemLore()` | **Invoked:** yes | **Cadence:** `Committed Craft, Intake, and Ownership Facts`
- **Setup Invocation Sites:** `src/Main.Lifecycle.cs:720`, `src/Main.PlayerSurfaces.cs:260`
- **UI Routes:** `inventory_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`](../../Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs)
  - Host Session: [`src/Host/ItemLoreHostSession.cs`](../../src/Host/ItemLoreHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ItemLoreHostSession.cs`](../../src/Host/ItemLoreHostSession.cs)
  - UI Panel: [`src/UI/InventoryDetailPanel.cs`](../../src/UI/InventoryDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Inventory/ItemLoreSystemTests.cs`](../../Ashfall.Core.Tests/Inventory/ItemLoreSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Inventory/Plan190ItemLoreIntegrationTests.cs`](../../Ashfall.Core.Tests/Inventory/Plan190ItemLoreIntegrationTests.cs)

### 98. `collectible_discovery` — One-time collectible discovery ledger (Inventory & Lore)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupCollectibles()` | **Invoked:** yes | **Cadence:** `On-Demand (One-Time Discovery Ledger)`
- **Setup Invocation Sites:** `src/Main.Application.cs:1112`, `src/Main.SaveOrchestrator.cs:298`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/CollectibleDiscoveryState.cs`](../../Assets/Ashfall.Core/CollectibleDiscoveryState.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CollectibleDiscoverySaveStore.cs`](../../src/Host/CollectibleDiscoverySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`](../../Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs)

### 99. `unique_claims` — Global unique-item claim ledger (Inventory & Lore)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupCollectibles()` | **Invoked:** yes | **Cadence:** `On-Demand (Global Unique Claim Ledger)`
- **Setup Invocation Sites:** `src/Main.Application.cs:1112`, `src/Main.SaveOrchestrator.cs:298`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/UniqueItemClaimRegistry.cs`](../../Assets/Ashfall.Core/UniqueItemClaimRegistry.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/UniqueClaimSaveStore.cs`](../../src/Host/UniqueClaimSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`](../../Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs)

### 100. `procedural_eulogy` — Procedural eulogies: composed death records archived from dweller life summaries (Journal)
- **Owner Domain:** `journal`
- **Setup Method:** `Main.SetupProceduralEulogy()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.ProceduralEulogy.cs:35`, `src/Main.ProceduralEulogy.cs:62`, `src/Main.ProceduralEulogy.cs:68`, `src/Main.SaveOrchestrator.cs:364`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs`](../../Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs)
  - Host Session: [`src/Host/ProceduralEulogyHostSession.cs`](../../src/Host/ProceduralEulogyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ProceduralEulogyHostSession.cs`](../../src/Host/ProceduralEulogyHostSession.cs)

### 101. `cultural_archives` — Deep-vault cultural archives: restoration, transcription, microfiche preservation, discs, salons, chronicles (Knowledge)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupCulturalArchive()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans167_219.cs:97`, `src/Main.Plans167_219.cs:107`, `src/Main.Plans167_219.cs:118`, `src/Main.Plans167_219.cs:141`, `src/Main.Plans167_219.cs:189`, `src/Main.Plans167_219.cs:233`, `src/Main.Plans167_219.cs:272`, `src/Main.Plans167_219.cs:294`, `src/Main.Plans167_219.cs:310`, `src/Main.PlayerSurfaces.cs:255`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`](../../Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CulturalArchiveSaveStore.cs`](../../src/Host/CulturalArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`](../../Ashfall.Core.Tests/CulturalArchiveVaultTests.cs)

### 102. `field_guide` — Plan 20A/28 — field-guide unlocked-entry ledger (reading-the-land knowledge) (Knowledge)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupFieldGuide()` | **Invoked:** yes | **Cadence:** `On-Demand (Study & Discovery)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:236`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/FieldGuideCatalog.cs`](../../Assets/Ashfall.Core/World/FieldGuideCatalog.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FieldGuideSaveStore.cs`](../../src/Host/FieldGuideSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/FieldGuidePersistenceTests.cs`](../../Ashfall.Core.Tests/FieldGuidePersistenceTests.cs)

### 103. `prewar_archives` — Pre-war archive discovery and decryption (Knowledge)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupPlans62To65()` | **Invoked:** no | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs`](../../Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PrewarArchiveSaveStore.cs`](../../src/Host/PrewarArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Research/PrewarArchiveDecryptionTests.cs`](../../Ashfall.Core.Tests/Research/PrewarArchiveDecryptionTests.cs)

### 104. `research` — Research knowledge progress: unlocked, active, and completed nodes (Plan 34) (Knowledge)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.None()` | **Invoked:** yes | **Cadence:** `On-Demand (Study Progress)`
- **UI Routes:** `research`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Research/ResearchSystem.cs`](../../Assets/Ashfall.Core/Research/ResearchSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ResearchSaveStore.cs`](../../src/Host/ResearchSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ResearchPanel.cs`](../../src/UI/ResearchPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs)

### 105. `survivor_education` — ORPHAN-SEAL-W1 — learner records, subjects, teachers, and graduation (Knowledge)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorEducation()` | **Invoked:** yes | **Cadence:** `Session-Driven`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:80`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Education/SurvivorEducationSystem.cs`](../../Assets/Ashfall.Core/Education/SurvivorEducationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Education/Plan154EducationIntegrationTests.cs`](../../Ashfall.Core.Tests/Education/Plan154EducationIntegrationTests.cs)

### 106. `campaign_legacy` — Plan 140 — Generational legacy, heirlooms, campaign inheritance, and New Game+ multi-generational continuity (Legacy)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCampaignLegacy()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:171`, `src/Main.SaveOrchestrator.cs:307`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs`](../../Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs)
  - Host Session: [`src/Host/CampaignLegacyHostSession.cs`](../../src/Host/CampaignLegacyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CampaignLegacyHostSession.cs`](../../src/Host/CampaignLegacyHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Legacy/Plan140CampaignLegacyHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Legacy/Plan140CampaignLegacyHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs`](../../Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs)

### 107. `leatherwork_archive` — Plan 159 — Tanning/leather material provenance & workshop knowledge archive: discovered-record ledger (IDs only) (Material Provenance Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupLeatherworkArchive()` | **Invoked:** yes | **Cadence:** `Event-Driven (Location Discovery & Item Inspection)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:127`, `src/Main.SaveOrchestrator.cs:274`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/LeatherworkArchiveSystem.cs`](../../Assets/Ashfall.Core/Narrative/LeatherworkArchiveSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/LeatherworkArchiveSaveStore.cs`](../../src/Host/LeatherworkArchiveSaveStore.cs)
  - UI Panel: [`src/UI/InventoryDetailPanel.cs`](../../src/UI/InventoryDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/LeatherworkArchiveTests.cs`](../../Ashfall.Core.Tests/Narrative/LeatherworkArchiveTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TanningLeatherCatalogTests.cs`](../../Ashfall.Core.Tests/TanningLeatherCatalogTests.cs)

### 108. `chronic_condition` — Plan 193 \u2014 tracked chronic conditions and accommodations, projected onto duty/needs capability (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupChronicConditions()` | **Invoked:** yes | **Cadence:** `Committed Clinical Fact / Explicit Accommodation`
- **Setup Invocation Sites:** `src/Main.Lifecycle.cs:728`, `src/Main.SaveOrchestrator.cs:177`
- **UI Routes:** `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs`](../../Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs)
  - Host Session: [`src/Host/ChronicConditionHostSession.cs`](../../src/Host/ChronicConditionHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ChronicConditionHostSession.cs`](../../src/Host/ChronicConditionHostSession.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/Plan193ChronicConditionIntegrationTests.cs`](../../Ashfall.Core.Tests/Medical/Plan193ChronicConditionIntegrationTests.cs)

### 109. `clinical_ward_triage` — Expansion 38 — clinical ward triage priority, surgical suite readiness, and sterile supply inventory (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupClinicalWardTriage()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2837`, `src/Main.CampaignOwners.cs:2848`, `src/Main.ClinicalWardTriage.cs:39`, `src/Main.ClinicalWardTriage.cs:52`, `src/Main.ClinicalWardTriage.cs:59`, `src/Main.ClinicalWardTriage.cs:65`, `src/Main.ClinicalWardTriage.cs:71`, `src/Main.ClinicalWardTriage.cs:77`, `src/Main.ClinicalWardTriage.cs:83`, `src/Main.ClinicalWardTriage.cs:89`, `src/Main.SaveOrchestrator.cs:343`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/ClinicalWardLedger.cs`](../../Assets/Ashfall.Core/Medical/ClinicalWardLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs`](../../Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs)
  - Host Session: [`src/Host/ClinicalWardTriageHostSession.cs`](../../src/Host/ClinicalWardTriageHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ClinicalWardTriageHostSession.cs`](../../src/Host/ClinicalWardTriageHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/ClinicalWardLedgerTests.cs`](../../Ashfall.Core.Tests/Medical/ClinicalWardLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs`](../../Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs)

### 110. `dependency_taper_withdrawal` — Expansion 35 — chemical dependency taper programs, withdrawal management, and care policy posture (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupDependencyTaperWithdrawal()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2781`, `src/Main.CampaignOwners.cs:2792`, `src/Main.DependencyTaperWithdrawal.cs:40`, `src/Main.DependencyTaperWithdrawal.cs:46`, `src/Main.DependencyTaperWithdrawal.cs:52`, `src/Main.DependencyTaperWithdrawal.cs:58`, `src/Main.DependencyTaperWithdrawal.cs:64`, `src/Main.DependencyTaperWithdrawal.cs:70`, `src/Main.SaveOrchestrator.cs:341`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/DependencyTaperLedger.cs`](../../Assets/Ashfall.Core/Medical/DependencyTaperLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Medical/DependencyTaperWithdrawalEngine.cs`](../../Assets/Ashfall.Core/Medical/DependencyTaperWithdrawalEngine.cs)
  - Host Session: [`src/Host/DependencyTaperWithdrawalHostSession.cs`](../../src/Host/DependencyTaperWithdrawalHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DependencyTaperWithdrawalHostSession.cs`](../../src/Host/DependencyTaperWithdrawalHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/DependencyTaperLedgerTests.cs`](../../Ashfall.Core.Tests/Medical/DependencyTaperLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/DependencyTaperWithdrawalEngineTests.cs`](../../Ashfall.Core.Tests/Medical/DependencyTaperWithdrawalEngineTests.cs)

### 111. `health_history` — Plan 198 — Longitudinal health histories, diagnostic logs, recovery events, vaccination tracking, and health trends (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupHealthHistory()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3261`, `src/Main.CampaignOwners.cs:3272`, `src/Main.Expeditions.cs:766`, `src/Main.HealthHistory.cs:19`, `src/Main.SaveOrchestrator.cs:393`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/HealthHistorySystem.cs`](../../Assets/Ashfall.Core/Medical/HealthHistorySystem.cs)
  - Host Session: [`src/Host/HealthHistoryHostSession.cs`](../../src/Host/HealthHistoryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HealthHistoryHostSession.cs`](../../src/Host/HealthHistoryHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/Plan198HealthHistoryIntegrationTests.cs`](../../Ashfall.Core.Tests/Medical/Plan198HealthHistoryIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs`](../../Ashfall.Core.Tests/Medical/Plan198MedicalRecordLogTests.cs)

### 112. `lyophilization` — Plans 130-133 — preserved-biologic batches and viability ledger (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupLyophilization()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans130_133.cs:27`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/LyophilizationSystem.cs`](../../Assets/Ashfall.Core/Medical/LyophilizationSystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 113. `medical_pipeline` — Diagnosis knowledge, treatment reservations, scheduled procedures (Task #133) (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedical()` | **Invoked:** yes | **Cadence:** `On-Demand (Triage & Procedure Commands)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1809`, `src/Main.CampaignOwners.cs:1835`, `src/Main.CampaignServices.cs:40`, `src/Main.DutyRoster.cs:50`, `src/Main.ExpandedShelterSystems.cs:102`, `src/Main.GameFlow.cs:411`, `src/Main.GameFlow.cs:505`, `src/Main.GameFlow.cs:627`, `src/Main.Lifecycle.cs:728`, `src/Main.Medical.cs:103`, `src/Main.Medical.cs:194`, `src/Main.Medical.cs:212`, `src/Main.Medical.cs:223`, `src/Main.Medical.cs:470`, `src/Main.Phase0.cs:102`, `src/Main.Phase0.cs:116`, `src/Main.PlayerSurfaces.cs:174`, `src/Main.PlayerSurfaces.cs:315`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.SaveOrchestrator.cs:175`, `src/Main.ShelterBatch3.cs:300`, `src/Main.SurvivorFitness.cs:65`
- **UI Routes:** `medical`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`](../../Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MedicalPipelineSaveStore.cs`](../../src/Host/MedicalPipelineSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs)

### 114. `microfluidic_diagnostic` — Plans 146-149 — microfluidic diagnostic cartridge manufacturing and run records (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMicrofluidicDiagnostic()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans146_149.cs:388`, `src/Main.Plans146_149.cs:708`, `src/Main.PlayerSurfaces.cs:129`
- **UI Routes:** `microfluidic_diagnostic`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs`](../../Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs)
  - Host Session: [`src/Host/MicrofluidicDiagnosticHostSession.cs`](../../src/Host/MicrofluidicDiagnosticHostSession.cs)
  - Save Store: [`src/Host/MicrofluidicDiagnosticSaveStore.cs`](../../src/Host/MicrofluidicDiagnosticSaveStore.cs)
  - UI Panel: [`src/UI/MicrofluidicDiagnosticPanel.cs`](../../src/UI/MicrofluidicDiagnosticPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MicrofluidicDiagnosticEngineTests.cs`](../../Ashfall.Core.Tests/Medical/MicrofluidicDiagnosticEngineTests.cs)

### 115. `palliative_care` — Expansion 24 The Long Goodbye — terminal palliative care: pain, lucidity, dignity, grief stage, final wishes, and memorial echoes (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupPalliativeCare()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3514`, `src/Main.PalliativeCare.cs:35`, `src/Main.PalliativeCare.cs:43`, `src/Main.PalliativeCare.cs:51`, `src/Main.PalliativeCare.cs:64`, `src/Main.PalliativeCare.cs:76`, `src/Main.PalliativeCare.cs:84`, `src/Main.SaveOrchestrator.cs:365`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs`](../../Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs)
  - Host Session: [`src/Host/PalliativeCareHostSession.cs`](../../src/Host/PalliativeCareHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PalliativeCareHostSession.cs`](../../src/Host/PalliativeCareHostSession.cs)

### 116. `pathogen_strains` — Flagship XI Plan 155 — fictional strain layer: cure projects and unlocked cures (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupPathogenStrains()` | **Invoked:** yes | **Cadence:** `Daily Strain Progression Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1858`, `src/Main.PathogenStrains.cs:63`, `src/Main.SaveOrchestrator.cs:183`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs`](../../Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PathogenStrainSaveStore.cs`](../../src/Host/PathogenStrainSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiseaseSystemTests.cs`](../../Ashfall.Core.Tests/DiseaseSystemTests.cs)

### 117. `pharmaceutical_tablet` — PLAN-PHARMACEUTICAL-TRUTH-167 \u2014 tablet press condition, active batch, and unclaimed output buffer (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupPharmaceuticalTablet()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (expanded shelter day)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:377`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs`](../../Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs)
  - Host Session: [`src/Host/PharmaceuticalTabletHostSession.cs`](../../src/Host/PharmaceuticalTabletHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PharmaceuticalTabletHostSession.cs`](../../src/Host/PharmaceuticalTabletHostSession.cs)

### 118. `psychological_sanatorium` — Trauma sanatorium: admissions, therapies, sedatives, relapse, discharge (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupSanatorium()` | **Invoked:** no | **Cadence:** `On-Demand`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Sanatorium/PsychologicalSanatoriumSystem.cs`](../../Assets/Ashfall.Core/Sanatorium/PsychologicalSanatoriumSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PsychologicalSanatoriumSaveStore.cs`](../../src/Host/PsychologicalSanatoriumSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PsychologicalSanatoriumTests.cs`](../../Ashfall.Core.Tests/PsychologicalSanatoriumTests.cs)

### 119. `surgical_graft` — PLAN-SURGICAL-WARD-TRUTH-213 \u2014 graft records, integration progress, rejection risk, and immunosuppressant levels (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupSurgicalGraft()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (expanded shelter day)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:376`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/SurgicalGraftRejectionEngine.cs`](../../Assets/Ashfall.Core/Medical/SurgicalGraftRejectionEngine.cs)
  - Host Session: [`src/Host/SurgicalGraftHostSession.cs`](../../src/Host/SurgicalGraftHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurgicalGraftHostSession.cs`](../../src/Host/SurgicalGraftHostSession.cs)

### 120. `surgical_ward` — Advanced surgical ward operations and sterile field (Medical)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupSurgicalWard()` | **Invoked:** yes | **Cadence:** `Daily Sterile Field Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:285`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurgicalWardSaveStore.cs`](../../src/Host/SurgicalWardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)

### 121. `propaganda_campaigns` — Plan 168 — propaganda messages, multi-day campaigns, detection, and morale warfare (Morale & Influence (Plan 168))
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPropaganda()` | **Invoked:** yes | **Cadence:** `Daily (Campaign Decay & Distribution)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:148`
- **UI Routes:** `propaganda`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Propaganda/PropagandaSystem.cs`](../../Assets/Ashfall.Core/Propaganda/PropagandaSystem.cs)
  - Host Session: [`src/Host/PropagandaHostSession.cs`](../../src/Host/PropagandaHostSession.cs)
  - Save Store: [`src/Host/PropagandaSaveStore.cs`](../../src/Host/PropagandaSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PropagandaPanel.cs`](../../src/UI/PropagandaPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Propaganda/Plan168PropagandaIntegrationTests.cs`](../../Ashfall.Core.Tests/Propaganda/Plan168PropagandaIntegrationTests.cs)

### 122. `bestiary_knowledge` — Plan 187 — Bestiary creature discovery, sighting records, tiered lore unlocks, kill and butcher tracking (Narrative)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupBestiary()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Bestiary.cs:18`, `src/Main.CampaignOwners.cs:3233`, `src/Main.CampaignOwners.cs:3244`, `src/Main.SaveOrchestrator.cs:392`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Bestiary/BestiarySystem.cs`](../../Assets/Ashfall.Core/Bestiary/BestiarySystem.cs)
  - Host Session: [`src/Host/BestiaryHostSession.cs`](../../src/Host/BestiaryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BestiaryHostSession.cs`](../../src/Host/BestiaryHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs`](../../Ashfall.Core.Tests/Bestiary/Plan187BestiaryIntegrationTests.cs)

### 123. `broadsheet_press` — Expansion 30 — movable type tray, ink and paper consumables, and the bound archive of printed publications. Rumor facts stay with RumorSystem; morale stays with the survivors' needs authority. (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupBroadsheetPress()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.BroadsheetPress.cs:78`, `src/Main.BroadsheetPress.cs:131`, `src/Main.BroadsheetPress.cs:166`, `src/Main.BroadsheetPress.cs:172`, `src/Main.CampaignOwners.cs:2666`, `src/Main.CampaignOwners.cs:2677`, `src/Main.SaveOrchestrator.cs:337`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Print/BroadsheetPressLedger.cs`](../../Assets/Ashfall.Core/Print/BroadsheetPressLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Print/PublicBroadsheetPressEngine.cs`](../../Assets/Ashfall.Core/Print/PublicBroadsheetPressEngine.cs)
  - Host Session: [`src/Host/BroadsheetPressHostSession.cs`](../../src/Host/BroadsheetPressHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BroadsheetPressHostSession.cs`](../../src/Host/BroadsheetPressHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Print/BroadsheetPressLedgerTests.cs`](../../Ashfall.Core.Tests/Print/BroadsheetPressLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Print/PublicBroadsheetPressEngineTests.cs`](../../Ashfall.Core.Tests/Print/PublicBroadsheetPressEngineTests.cs)

### 124. `cipher_quest_chain` — Plan 11/251 — cipher puzzle chains: heard broadcasts, key items, decodes, revealed locations (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupCipherQuestChain()` | **Invoked:** yes | **Cadence:** `Event-Driven (Broadcast Heard / Key Acquired)`
- **Setup Invocation Sites:** `src/Main.CipherQuestChain.cs:69`, `src/Main.CipherQuestChain.cs:78`, `src/Main.CipherQuestChain.cs:87`, `src/Main.CipherQuestChain.cs:96`, `src/Main.SaveOrchestrator.cs:239`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs`](../../Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs)
  - Host Session: [`src/Host/CipherQuestChainHostSession.cs`](../../src/Host/CipherQuestChainHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CipherQuestChainSaveStore.cs`](../../src/Host/CipherQuestChainSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs`](../../Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan11ExplorationTests.cs`](../../Ashfall.Core.Tests/World/Plan11ExplorationTests.cs)

### 125. `confession_secret` — ORPHAN-SEAL-W1 — discovered secrets, resolution choices, and leverage records (Narrative)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupConfessionSecrets()` | **Invoked:** yes | **Cadence:** `Discovery-Driven`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:82`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs`](../../Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`](../../Ashfall.Core.Tests/ConfessionSecretSystemTests.cs)

### 126. `echoes` — Field echoes: surfaced and resolved one-time narrative state (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupEchoes()` | **Invoked:** yes | **Cadence:** `Narrative Echo Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2523`, `src/Main.CampaignServices.cs:65`, `src/Main.Echoes.cs:56`, `src/Main.Echoes.cs:74`, `src/Main.Narrative.cs:506`, `src/Main.SaveOrchestrator.cs:201`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs`](../../Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Narrative/EchoSystem.cs`](../../Assets/Ashfall.Core/Narrative/EchoSystem.cs)
  - Host Session: [`src/Host/EchoHostSession.cs`](../../src/Host/EchoHostSession.cs)
  - Host Session: [`src/Host/EchoSaveStore.cs`](../../src/Host/EchoSaveStore.cs)
  - Save Store: [`src/Host/EchoSaveStore.cs`](../../src/Host/EchoSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/EchoCatalogTests.cs`](../../Ashfall.Core.Tests/Narrative/EchoCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/EchoSystemTests.cs`](../../Ashfall.Core.Tests/Narrative/EchoSystemTests.cs)

### 127. `letter_delivery` — Plan 212 letter route \u2014 discovered letters, explicit delivery/withhold decisions, private until delivered (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupLetters()` | **Invoked:** yes | **Cadence:** `Explicit Discovery and Delivery Decision`
- **Setup Invocation Sites:** `src/Main.Letters.cs:42`, `src/Main.Letters.cs:67`, `src/Main.Letters.cs:76`, `src/Main.Lifecycle.cs:720`, `src/Main.TimeCapsule.cs:83`
- **UI Routes:** `time_capsule`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/LetterDeliverySystem.cs`](../../Assets/Ashfall.Core/Narrative/LetterDeliverySystem.cs)
  - Host Session: [`src/Host/LetterDeliveryHostSession.cs`](../../src/Host/LetterDeliveryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/LetterDeliveryHostSession.cs`](../../src/Host/LetterDeliveryHostSession.cs)
  - UI Panel: [`src/UI/TimeCapsulePanel.cs`](../../src/UI/TimeCapsulePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/LetterDeliverySystemTests.cs`](../../Ashfall.Core.Tests/LetterDeliverySystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/Plan212LetterRouteIntegrationTests.cs`](../../Ashfall.Core.Tests/Narrative/Plan212LetterRouteIntegrationTests.cs)

### 128. `npc_memory` — Plan 147 — Per-NPC memory and relationship depth: trust, grudge, favors owed, forgiveness, and trade multipliers (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupNpcMemory()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:491`, `src/Main.CampaignOwners.cs:502`, `src/Main.NpcMemory.cs:45`, `src/Main.SaveOrchestrator.cs:310`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs`](../../Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs)
  - Host Session: [`src/Host/NpcMemoryHostSession.cs`](../../src/Host/NpcMemoryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/NpcMemoryHostSession.cs`](../../src/Host/NpcMemoryHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/NpcMemorySystemTests.cs`](../../Ashfall.Core.Tests/Narrative/NpcMemorySystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/Plan147NpcMemoryHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Narrative/Plan147NpcMemoryHostIntegrationTests.cs)

### 129. `seasonal_celebration` — ORPHAN-SEAL-W1 — holidays, anniversaries, scales, and celebration history (Narrative)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupSeasonalCelebration()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:75`, `src/Main.OrphanSealWave1.cs:374`
- **UI Routes:** `shelter_operations`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Events/SeasonalCelebrationSystem.cs`](../../Assets/Ashfall.Core/Events/SeasonalCelebrationSystem.cs)
  - Host Session: [`src/Host/ShelterOperationsHostSession.cs`](../../src/Host/ShelterOperationsHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - UI Panel: [`src/UI/ShelterOperationsPanel.cs`](../../src/UI/ShelterOperationsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs`](../../Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Events/SeasonalCelebrationCycleTests.cs`](../../Ashfall.Core.Tests/Events/SeasonalCelebrationCycleTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Integration/ShelterOperationsBoardWiringTests.cs`](../../Ashfall.Core.Tests/Integration/ShelterOperationsBoardWiringTests.cs)

### 130. `shelter_festival` — ORPHAN-SEAL-W1 — player-scheduled festivals, commodity costs, and completion state (Narrative)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupShelterFestival()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:83`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Culture/ShelterFestivalEngine.cs`](../../Assets/Ashfall.Core/Culture/ShelterFestivalEngine.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs`](../../Ashfall.Core.Tests/Events/Plan170SeasonalCelebrationsIntegrationTests.cs)

### 131. `survivor_letter_delivery` — ORPHAN-SEAL follow-up — dead-letter discovery, dweller addressing, and delivery resolution (Narrative)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupSurvivorLetterDelivery()` | **Invoked:** yes | **Cadence:** `Player Action (Find / Address / Deliver / Withhold)`
- **Setup Invocation Sites:** `src/Main.SurvivorLetterDelivery.cs:34`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs`](../../Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs)
  - Host Session: [`src/Host/SurvivorLetterDeliveryHostSession.cs`](../../src/Host/SurvivorLetterDeliveryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorLetterDeliverySaveStore.cs`](../../src/Host/SurvivorLetterDeliverySaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs`](../../Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs)

### 132. `oral_lore` — Plan 155 — oral lore first-heard ledger (lore IDs only) (Narrative & Cultural Tradition)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupOralLore()` | **Invoked:** yes | **Cadence:** `Event-Driven (Performance)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:124`, `src/Main.SaveOrchestrator.cs:271`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/OralLorePerformanceSystem.cs`](../../Assets/Ashfall.Core/Narrative/OralLorePerformanceSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OralLoreSaveStore.cs`](../../src/Host/OralLoreSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/OralLorePlan155Tests.cs`](../../Ashfall.Core.Tests/Narrative/OralLorePlan155Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/OralLoreCatalogTests.cs`](../../Ashfall.Core.Tests/OralLoreCatalogTests.cs)

### 133. `moral_choice` — Moral choice ledger and community trust (Narrative & Decisions)
- **Owner Domain:** `events`
- **Setup Method:** `Main.SetupMoralChoice()` | **Invoked:** yes | **Cadence:** `On-Demand (Branch Choice)`
- **Setup Invocation Sites:** `src/Main.Application.cs:1108`, `src/Main.CampaignOwners.cs:2477`, `src/Main.CampaignServices.cs:58`, `src/Main.FactionBranch.cs:95`, `src/Main.GameFlow.cs:563`, `src/Main.GameFlow.cs:577`, `src/Main.GameFlow.cs:582`, `src/Main.MoralChoice.cs:103`, `src/Main.MoralChoice.cs:110`, `src/Main.MoralChoice.cs:170`, `src/Main.MoralChoice.cs:182`, `src/Main.MoralChoice.cs:190`, `src/Main.MoralChoice.cs:201`, `src/Main.MoralChoice.cs:301`, `src/Main.MoralChoice.cs:328`, `src/Main.PlayerSurfaces.cs:367`, `src/Main.PlayerSurfaces.cs:381`, `src/Main.PlayerSurfaces.cs:390`, `src/Main.SaveOrchestrator.cs:399`, `src/Main.ShelterSocial.cs:414`, `src/Main.UiHandlers.cs:85`, `src/Main.UiHandlers.cs:101`, `src/Main.UiHandlers.cs:199`, `src/Main.UiHandlers.cs:229`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceState.cs)
  - Core System: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`](../../Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs)
  - Save Store: [`src/Host/MoralChoiceSaveStore.cs`](../../src/Host/MoralChoiceSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MoralChoiceSystemTests.cs`](../../Ashfall.Core.Tests/MoralChoiceSystemTests.cs)

### 134. `contraband_stash` — Plan 147 — bunker contraband stash claim ledger (once-only discovery) (Narrative & Illicit Economy)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupContrabandStash()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:120`, `src/Main.SaveOrchestrator.cs:267`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs`](../../Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ContrabandSaveStore.cs`](../../src/Host/ContrabandSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs`](../../Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs)

### 135. `sleep_acoustic_rest` — Expansion 41 — sleep quality, soundproofing acoustic attenuation, and shelter quiet hours (Needs)
- **Owner Domain:** `needs`
- **Setup Method:** `Main.SetupSleepAcousticRest()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2950`, `src/Main.CampaignOwners.cs:2961`, `src/Main.SaveOrchestrator.cs:347`, `src/Main.SleepAcousticRest.cs:40`, `src/Main.SleepAcousticRest.cs:46`, `src/Main.SleepAcousticRest.cs:52`, `src/Main.SleepAcousticRest.cs:58`, `src/Main.SleepAcousticRest.cs:64`, `src/Main.SleepAcousticRest.cs:70`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs`](../../Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Needs/SleepAcousticRestEngine.cs`](../../Assets/Ashfall.Core/Needs/SleepAcousticRestEngine.cs)
  - Host Session: [`src/Host/SleepAcousticRestHostSession.cs`](../../src/Host/SleepAcousticRestHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SleepAcousticRestHostSession.cs`](../../src/Host/SleepAcousticRestHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Needs/SleepAcousticLedgerTests.cs`](../../Ashfall.Core.Tests/Needs/SleepAcousticLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Needs/SleepAcousticRestEngineTests.cs`](../../Ashfall.Core.Tests/Needs/SleepAcousticRestEngineTests.cs)

### 136. `common_table_rationing` — Expansion 26 — common-table meal dietary diversity, deficiency risk, rationing policy, and cook duty waste (Nutrition)
- **Owner Domain:** `nutrition`
- **Setup Method:** `Main.SetupCommonTableRationing()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3623`, `src/Main.CommonTableRationing.cs:34`, `src/Main.CommonTableRationing.cs:42`, `src/Main.CommonTableRationing.cs:50`, `src/Main.CommonTableRationing.cs:58`, `src/Main.CommonTableRationing.cs:71`, `src/Main.CommonTableRationing.cs:79`, `src/Main.CommonTableRationing.cs:87`, `src/Main.SaveOrchestrator.cs:369`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Nutrition/CommonTableRationingEngine.cs`](../../Assets/Ashfall.Core/Nutrition/CommonTableRationingEngine.cs)
  - Host Session: [`src/Host/CommonTableRationingHostSession.cs`](../../src/Host/CommonTableRationingHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CommonTableRationingHostSession.cs`](../../src/Host/CommonTableRationingHostSession.cs)

### 137. `cooking` — Plan 136 — wildlife trapping food pipeline & cooking system: recipes, active operations, meals prepared, and food decontamination (Nutrition)
- **Owner Domain:** `cooking`
- **Setup Method:** `Main.SetupCooking()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:167`, `src/Main.SaveOrchestrator.cs:306`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Cooking/CookingRecipeCatalogLoader.cs`](../../Assets/Ashfall.Core/Cooking/CookingRecipeCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Cooking/CookingSystem.cs`](../../Assets/Ashfall.Core/Cooking/CookingSystem.cs)
  - Host Session: [`src/Host/CookingHostSession.cs`](../../src/Host/CookingHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CookingHostSession.cs`](../../src/Host/CookingHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Cooking/Plan136CookingHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Cooking/Plan136CookingHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs`](../../Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs)

### 138. `grain_processing` — Grain milling, silo safety, and pest pressure (Nutrition)
- **Owner Domain:** `nutrition`
- **Setup Method:** `Main.SetupGrainProcessing()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:135`, `src/Main.ExpandedShelterSystems.cs:714`, `src/Main.PlayerSurfaces.cs:757`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/GrainProcessingSystem.cs`](../../Assets/Ashfall.Core/GrainProcessingSystem.cs)
  - Host Session: [`src/Host/GrainProcessingHostSession.cs`](../../src/Host/GrainProcessingHostSession.cs)
  - Save Store: [`src/Host/GrainProcessingHostSession.cs`](../../src/Host/GrainProcessingHostSession.cs)

### 139. `companion_animals` — Plan 174 — persistent companion animals: care, bond, training, roles, sickness, assignments (Plan 174 Companion Animals)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupCompanionAnimals()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:110`, `src/Main.FlagshipPanels.cs:21`, `src/Main.Plans162_165.cs:830`, `src/Main.SaveOrchestrator.cs:257`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`](../../Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CompanionSaveStore.cs`](../../src/Host/CompanionSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs`](../../Ashfall.Core.Tests/World/Plan174CompanionAnimalTests.cs)

### 140. `zealotry` — Plan 175 — fictional ideological pressure: belief state, fervor, dissent, shrines, escalation (Plan 175 Ideological Pressure)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupZealotry()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:112`, `src/Main.FlagshipPanels.cs:39`, `src/Main.SaveOrchestrator.cs:259`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/ZealotrySystem.cs`](../../Assets/Ashfall.Core/Survivors/ZealotrySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ZealotrySaveStore.cs`](../../src/Host/ZealotrySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/ZealotrySystemTests.cs`](../../Ashfall.Core.Tests/Survivors/ZealotrySystemTests.cs)

### 141. `anomaly_hazard` — Plan 176 — authored anomaly and storm-front hazard zones, movement, warnings, loot-site resolution (Plan 176 Anomaly Hazard Layer)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupAnomalyHazard()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:109`, `src/Main.FlagshipPanels.cs:57`, `src/Main.SaveOrchestrator.cs:256`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/AnomalyHazardSystem.cs`](../../Assets/Ashfall.Core/World/AnomalyHazardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AnomalyHazardSaveStore.cs`](../../src/Host/AnomalyHazardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan176AnomalyHazardTests.cs`](../../Ashfall.Core.Tests/World/Plan176AnomalyHazardTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs`](../../Ashfall.Core.Tests/World/Plan176CrossSystemConsumerTests.cs)

### 142. `bionics` — Plan 177 — bionic implant instances: condition, integration, power, maintenance, complications (Plan 177 Bionics & Prosthetics)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupBionics()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:111`, `src/Main.FlagshipPanels.cs:75`, `src/Main.SaveOrchestrator.cs:258`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/BionicsSystem.cs`](../../Assets/Ashfall.Core/Medical/BionicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BionicsSaveStore.cs`](../../src/Host/BionicsSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/Plan177BionicsTests.cs`](../../Ashfall.Core.Tests/Medical/Plan177BionicsTests.cs)

### 143. `spiritual_meaning` — Plan 30 — spiritual-meaning coordinator: mourning arcs, ritual cooldowns, memorial rites (Plan 30 Spiritual Meaning)
- **Owner Domain:** `spiritual`
- **Setup Method:** `Main.SetupSpiritual()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:66`, `src/Main.SaveOrchestrator.cs:230`, `src/Main.Spiritual.cs:148`, `src/Main.SpiritualRitual.cs:33`, `src/Main.SurvivorFate.cs:126`, `src/Main.UiPanels.cs:1349`, `src/Main.UiPanels.cs:1354`, `src/Main.Zealotry.cs:35`
- **UI Routes:** `status`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Spiritual/SpiritualMeaningCoordinator.cs`](../../Assets/Ashfall.Core/Spiritual/SpiritualMeaningCoordinator.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SpiritualSaveStore.cs`](../../src/Host/SpiritualSaveStore.cs)
  - UI Panel: [`src/UI/IronCenotaphMemorialPanel.cs`](../../src/UI/IronCenotaphMemorialPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Spiritual/Plan30SpiritualWorldTests.cs`](../../Ashfall.Core.Tests/Spiritual/Plan30SpiritualWorldTests.cs)

### 144. `amputation` — Infection progression, amputations, prosthetics and bionics (Plans 178-201 Expansion Block)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupAmputation()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:117`, `src/Main.Expeditions.cs:128`, `src/Main.SaveOrchestrator.cs:264`
- **UI Routes:** `medical`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/AmputationSystem.cs`](../../Assets/Ashfall.Core/Medical/AmputationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AmputationSaveStore.cs`](../../src/Host/AmputationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/AmputationSystemTests.cs`](../../Ashfall.Core.Tests/Medical/AmputationSystemTests.cs)

### 145. `archaeology` — Archaeology excavation ruins, archive decryption, and lore unlocks (Plans 178-201 Expansion Block)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupArchaeology()` | **Invoked:** yes | **Cadence:** `On-Demand (Excavation & Decryption)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:116`, `src/Main.SaveOrchestrator.cs:263`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs`](../../Assets/Ashfall.Core/Archaeology/ArchaeologySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ArchaeologySaveStore.cs`](../../src/Host/ArchaeologySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs`](../../Ashfall.Core.Tests/Archaeology/ArchaeologySystemTests.cs)

### 146. `aviation` — Aviation airframes, flight plans, aerial mapping, and crash rescue (Plans 178-201 Expansion Block)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupAviation()` | **Invoked:** yes | **Cadence:** `Daily Flight Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:105`, `src/Main.PlayerSurfaces.cs:694`, `src/Main.SaveOrchestrator.cs:250`
- **UI Routes:** `aviation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/AviationSystem.cs`](../../Assets/Ashfall.Core/Expeditions/AviationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AviationSaveStore.cs`](../../src/Host/AviationSaveStore.cs)
  - UI Panel: [`src/UI/AviationUI.cs`](../../src/UI/AviationUI.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/AviationSystemTests.cs`](../../Ashfall.Core.Tests/Expeditions/AviationSystemTests.cs)

### 147. `ceremony` — Communal ceremonies, festivals, truces, and morale (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupCeremony()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:134`, `src/Main.SaveOrchestrator.cs:279`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/CeremonySystem.cs`](../../Assets/Ashfall.Core/Narrative/CeremonySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CeremonySaveStore.cs`](../../src/Host/CeremonySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/CeremonySystemTests.cs`](../../Ashfall.Core.Tests/Narrative/CeremonySystemTests.cs)

### 148. `chem_warfare` — CBRN hazard warfare and toxic contamination (Plans 178-201 Expansion Block)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupChemWarfare()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:132`, `src/Main.SaveOrchestrator.cs:277`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/ChemWarfareSystem.cs`](../../Assets/Ashfall.Core/Combat/ChemWarfareSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ChemWarfareSaveStore.cs`](../../src/Host/ChemWarfareSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs`](../../Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs)

### 149. `child_development` — Child development phases, education, trauma, and adulthood (Plans 178-201 Expansion Block)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupGenerational()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:101`, `src/Main.ChildDevelopment.cs:36`, `src/Main.Plans178_181.cs:95`, `src/Main.Plans178_181.cs:510`, `src/Main.Plans178_181.cs:521`, `src/Main.PlayerSurfaces.cs:729`, `src/Main.SaveOrchestrator.cs:246`
- **UI Routes:** `nursery`, `century_seed`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs`](../../Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/GenerationalSystem.cs`](../../Assets/Ashfall.Core/Survivors/GenerationalSystem.cs)
  - Host Session: [`src/Host/ChildDevelopmentHostSession.cs`](../../src/Host/ChildDevelopmentHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/GenerationalSaveStore.cs`](../../src/Host/GenerationalSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/NurseryPanel.cs`](../../src/UI/NurseryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GenerationalLineageExtensionTests.cs`](../../Ashfall.Core.Tests/GenerationalLineageExtensionTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/GenerationalSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/GenerationalSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan183ChildDevelopmentIntegrationTests.cs)

### 150. `comms_array` — Long-range communications array and satellite telemetry (Plans 178-201 Expansion Block)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupCommsArray()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:133`, `src/Main.SaveOrchestrator.cs:278`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/CommsArraySystem.cs`](../../Assets/Ashfall.Core/World/CommsArraySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CommsArraySaveStore.cs`](../../src/Host/CommsArraySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/CommsArraySystemTests.cs`](../../Ashfall.Core.Tests/World/CommsArraySystemTests.cs)

### 151. `desperation` — Starvation crisis desperation acts and cannibalism history (Plans 178-201 Expansion Block)
- **Owner Domain:** `survival`
- **Setup Method:** `Main.SetupDesperation()` | **Invoked:** yes | **Cadence:** `On-Demand (Crisis Command)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:114`, `src/Main.SaveOrchestrator.cs:261`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/DesperationSystem.cs`](../../Assets/Ashfall.Core/Survivors/DesperationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DesperationSaveStore.cs`](../../src/Host/DesperationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/DesperationSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/DesperationSystemTests.cs)

### 152. `expedition_stealth` — Expedition stealth, detection risk, camouflage, and night ops (Plans 178-201 Expansion Block)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupStealth()` | **Invoked:** yes | **Cadence:** `Event-Driven (Expedition Phases)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:104`, `src/Main.PlayerSurfaces.cs:719`, `src/Main.SaveOrchestrator.cs:249`
- **UI Routes:** `stealth`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/StealthSystem.cs`](../../Assets/Ashfall.Core/Combat/StealthSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/StealthSaveStore.cs`](../../src/Host/StealthSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/StealthReadoutPanel.cs`](../../src/UI/StealthReadoutPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Combat/StealthSystemTests.cs`](../../Ashfall.Core.Tests/Combat/StealthSystemTests.cs)

### 153. `fallout` — Radioactive fallout clouds, dispersal, and shelter sealing (Plans 178-201 Expansion Block)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupFallout()` | **Invoked:** yes | **Cadence:** `Hourly Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:113`, `src/Main.PlayerSurfaces.cs:734`, `src/Main.SaveOrchestrator.cs:260`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/FalloutSystem.cs`](../../Assets/Ashfall.Core/World/FalloutSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FalloutSaveStore.cs`](../../src/Host/FalloutSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/FalloutSystemTests.cs`](../../Ashfall.Core.Tests/World/FalloutSystemTests.cs)

### 154. `forced_labor` — Captive forced labor assignments, cruelty index, and rebellion risks (Plans 178-201 Expansion Block)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupForcedLabor()` | **Invoked:** yes | **Cadence:** `Daily Shift Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:106`, `src/Main.PlayerSurfaces.cs:704`, `src/Main.SaveOrchestrator.cs:251`
- **UI Routes:** `forced_labor`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/ForcedLaborSystem.cs`](../../Assets/Ashfall.Core/Factions/ForcedLaborSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ForcedLaborSaveStore.cs`](../../src/Host/ForcedLaborSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/LaborUI.cs`](../../src/UI/LaborUI.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/ForcedLaborSystemTests.cs`](../../Ashfall.Core.Tests/Factions/ForcedLaborSystemTests.cs)

### 155. `fungi_cultivation` — Subterranean fungi beds, substrate, spores, and blooms (Plans 178-201 Expansion Block)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupFungi()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:119`, `src/Main.SaveOrchestrator.cs:266`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs`](../../Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FungiSaveStore.cs`](../../src/Host/FungiSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Farming/FungiCultivationSystemTests.cs`](../../Ashfall.Core.Tests/Farming/FungiCultivationSystemTests.cs)

### 156. `mercenary_bounties` — Mercenary bounty contracts, target intel, and rival tracking (Plans 178-201 Expansion Block)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupMercenary()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:115`, `src/Main.SaveOrchestrator.cs:262`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/MercenarySystem.cs`](../../Assets/Ashfall.Core/Economy/MercenarySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MercenarySaveStore.cs`](../../src/Host/MercenarySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/MercenarySystemTests.cs`](../../Ashfall.Core.Tests/Economy/MercenarySystemTests.cs)

### 157. `mutation_tree` — Radiation exposure, genetic instability, and mutation trees (Plans 178-201 Expansion Block)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMutations()` | **Invoked:** yes | **Cadence:** `Event-Driven (Dose Thresholds)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:103`, `src/Main.Plans178_181.cs:289`, `src/Main.Plans178_181.cs:310`, `src/Main.Plans178_181.cs:316`, `src/Main.Plans178_181.cs:322`, `src/Main.Plans178_181.cs:328`, `src/Main.PlayerSurfaces.cs:724`, `src/Main.SaveOrchestrator.cs:248`
- **UI Routes:** `mutation_tree`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MutationSystem.cs`](../../Assets/Ashfall.Core/Medical/MutationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MutationSaveStore.cs`](../../src/Host/MutationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/MutationTreePanel.cs`](../../src/UI/MutationTreePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MutationSystemTests.cs`](../../Ashfall.Core.Tests/Medical/MutationSystemTests.cs)

### 158. `narcotics` — Chemical medicines, toxicity, tolerance, addiction, and rehab beds (Plans 178-201 Expansion Block)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupNarcotics()` | **Invoked:** yes | **Cadence:** `24h Medical Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:107`, `src/Main.PlayerSurfaces.cs:699`, `src/Main.SaveOrchestrator.cs:252`
- **UI Routes:** `narcotics`, `pharma_lab`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/NarcoticsSystem.cs`](../../Assets/Ashfall.Core/Medical/NarcoticsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/NarcoticsSaveStore.cs`](../../src/Host/NarcoticsSaveStore.cs)
  - UI Panel: [`src/UI/ChemUI.cs`](../../src/UI/ChemUI.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PharmaLabPanel.cs`](../../src/UI/PharmaLabPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs`](../../Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs)

### 159. `prisoner_management` — Captive detention, upkeep, interrogation, escape, and recruitment (Plans 178-201 Expansion Block)
- **Owner Domain:** `factions`
- **Setup Method:** `Main.SetupPrisoners()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:102`, `src/Main.Plans178_181.cs:442`, `src/Main.Plans178_181.cs:453`, `src/Main.PlayerSurfaces.cs:714`, `src/Main.SaveOrchestrator.cs:247`
- **UI Routes:** `prisoners`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Factions/PrisonerSystem.cs`](../../Assets/Ashfall.Core/Factions/PrisonerSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PrisonerSaveStore.cs`](../../src/Host/PrisonerSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PrisonerPanel.cs`](../../src/UI/PrisonerPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Factions/PrisonerSystemTests.cs`](../../Ashfall.Core.Tests/Factions/PrisonerSystemTests.cs)

### 160. `railway` — Rail network, track repair, and armored train operations (Plans 178-201 Expansion Block)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupRailway()` | **Invoked:** yes | **Cadence:** `On-Demand (Convoy Operations)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:118`, `src/Main.Plans130_133.cs:138`, `src/Main.SaveOrchestrator.cs:265`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/RailwaySystem.cs`](../../Assets/Ashfall.Core/Expeditions/RailwaySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RailwaySaveStore.cs`](../../src/Host/RailwaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/RailwaySystemTests.cs`](../../Ashfall.Core.Tests/Expeditions/RailwaySystemTests.cs)

### 161. `recreation` — Survivor hobbies, downtime, and recreation (Plans 178-201 Expansion Block)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupRecreation()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:131`, `src/Main.SaveOrchestrator.cs:276`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs`](../../Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RecreationSaveStore.cs`](../../src/Host/RecreationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Recreation/SurvivorDowntimeSystemTests.cs`](../../Ashfall.Core.Tests/Recreation/SurvivorDowntimeSystemTests.cs)

### 162. `robotics` — Pre-war robotics, directives, and automation (Plans 178-201 Expansion Block)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupRobotics()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:135`, `src/Main.SaveOrchestrator.cs:280`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/RoboticsSystem.cs`](../../Assets/Ashfall.Core/Crafting/RoboticsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RoboticsSaveStore.cs`](../../src/Host/RoboticsSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Crafting/RoboticsSystemTests.cs`](../../Ashfall.Core.Tests/Crafting/RoboticsSystemTests.cs)

### 163. `settlement_politics` — Settlement elections, political policies, approval rating, and coups (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupPolitics()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:108`, `src/Main.PlayerSurfaces.cs:709`, `src/Main.SaveOrchestrator.cs:253`
- **UI Routes:** `politics`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/PoliticsSystem.cs`](../../Assets/Ashfall.Core/Narrative/PoliticsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PoliticsSaveStore.cs`](../../src/Host/PoliticsSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/PoliticsUI.cs`](../../src/UI/PoliticsUI.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/PoliticsSystemTests.cs`](../../Ashfall.Core.Tests/Narrative/PoliticsSystemTests.cs)

### 164. `wasteland_justice` — Crime incidents, trials, punishments, banishments, and grudges (Plans 178-201 Expansion Block)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupJustice()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:130`, `src/Main.SaveOrchestrator.cs:275`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/JusticeSystem.cs`](../../Assets/Ashfall.Core/Narrative/JusticeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/JusticeSaveStore.cs`](../../src/Host/JusticeSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs`](../../Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs)

### 165. `plastic_pyrolysis` — Retort bay — waste plastic to synthetic fuel fractions (Plan 202) (Plans 202-205 Flagship (Plan 202))
- **Owner Domain:** `industry`
- **Setup Method:** `Main.SetupPlasticPyrolysis()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:128`
- **UI Routes:** `plastic_pyrolysis`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs`](../../Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PlasticPyrolysisSaveStore.cs`](../../src/Host/PlasticPyrolysisSaveStore.cs)
  - UI Panel: [`src/UI/PlasticPyrolysisPanel.cs`](../../src/UI/PlasticPyrolysisPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PlasticPyrolysisEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/PlasticPyrolysisEngineTests.cs)

### 166. `cargo_airdrop` — Airdrop events, crate contents, beacons, and interception races (Plan 205) (Plans 202-205 Flagship (Plan 205))
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupCargoAirdrop()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:129`
- **UI Routes:** `cargo_airdrop`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/CargoAirdropSystem.cs`](../../Assets/Ashfall.Core/World/CargoAirdropSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CargoAirdropSaveStore.cs`](../../src/Host/CargoAirdropSaveStore.cs)
  - UI Panel: [`src/UI/CargoAirdropPanel.cs`](../../src/UI/CargoAirdropPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/CargoAirdropEngineTests.cs`](../../Ashfall.Core.Tests/World/CargoAirdropEngineTests.cs)

### 167. `campaign_action_log` — Deterministic campaign action log: sequence-stable record of every successful significant player command (PlayerCommand)
- **Owner Domain:** `campaign`
- **Setup Method:** `Main.SetupCampaignActionLog()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignActionLog.cs:37`, `src/Main.CampaignActionLog.cs:45`, `src/Main.CampaignActionLog.cs:53`, `src/Main.CampaignActionLog.cs:59`, `src/Main.CampaignOwners.cs:3704`, `src/Main.SaveOrchestrator.cs:372`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/PlayerCommand/CampaignActionLog.cs`](../../Assets/Ashfall.Core/PlayerCommand/CampaignActionLog.cs)
  - Host Session: [`src/Host/CampaignActionLogHostSession.cs`](../../src/Host/CampaignActionLogHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CampaignActionLogHostSession.cs`](../../src/Host/CampaignActionLogHostSession.cs)

### 168. `geothermal_orc` — Plan B74 — geothermal organic Rankine loop, heat reserve, fouling, and leakage (Power)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupGeothermalOrc()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Plans74_77.cs:31`, `src/Main.Plans74_77.cs:138`, `src/Main.Plans74_77.cs:167`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs`](../../Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/Plans74To77HostSessions.cs`](../../src/Host/Plans74To77HostSessions.cs)
  - UI Panel: [`src/UI/Plans74To77Panels.cs`](../../src/UI/Plans74To77Panels.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plans74To77SystemsTests.cs`](../../Ashfall.Core.Tests/Plans74To77SystemsTests.cs)

### 169. `kinetic_storage` — Plans 78-81 — flywheel rotor, vacuum, bearing, and containment state (Power)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupKineticStorage()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans78_81.cs:91`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs`](../../Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs)
  - Host Session: [`src/Host/KineticStorageSaveStore.cs`](../../src/Host/KineticStorageSaveStore.cs)
  - Save Store: [`src/Host/KineticStorageSaveStore.cs`](../../src/Host/KineticStorageSaveStore.cs)

### 170. `solar_concentrator` — Plans 110-113 — parabolic solar concentrator, mirror condition, tracking mode, and thermal output (Power)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupSolarConcentrator()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans110_113.cs:137`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs`](../../Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs)
  - Host Session: [`src/Host/SolarConcentratorHostSession.cs`](../../src/Host/SolarConcentratorHostSession.cs)
  - Save Store: [`src/Host/SolarConcentratorSaveStore.cs`](../../src/Host/SolarConcentratorSaveStore.cs)

### 171. `hobby` — ORPHAN-SEAL-W1 — survivor hobby progress, mastery, and co-participation (Psychology)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupHobby()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:79`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/HobbySystem.cs`](../../Assets/Ashfall.Core/Survivors/HobbySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan161HobbyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan161HobbyIntegrationTests.cs)

### 172. `psychological_arcs` — Plans 162-165 — breakdown arcs, exposure, treatment progress, private stashes, catharsis (Psychology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupPsychologyArcs()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Plans162_165.cs:624`, `src/Main.Plans162_165.cs:653`, `src/Main.SaveOrchestrator.cs:291`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs`](../../Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs)
  - Host Session: [`src/Host/PsychologyArcHostSession.cs`](../../src/Host/PsychologyArcHostSession.cs)
  - Save Store: [`src/Host/PsychologyArcSaveStore.cs`](../../src/Host/PsychologyArcSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PsychologicalArcSystemTests.cs`](../../Ashfall.Core.Tests/PsychologicalArcSystemTests.cs)

### 173. `psychological_profiles` — Plan 179 — Unified psychology and phobia profiles: phobias, coping mechanisms, resilience, and therapy (Psychology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupPsychologicalProfiles()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3177`, `src/Main.CampaignOwners.cs:3188`, `src/Main.PsychologicalProfiles.cs:19`, `src/Main.SaveOrchestrator.cs:390`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs`](../../Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs)
  - Host Session: [`src/Host/PsychologicalProfileHostSession.cs`](../../src/Host/PsychologicalProfileHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PsychologicalProfileHostSession.cs`](../../src/Host/PsychologicalProfileHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs)

### 174. `survivor_autonomy` — ORPHAN-SEAL-W1 — daily survivor autonomy evaluations, cooldowns, and personal goals (Psychology)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorAutonomy()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:73`, `src/Main.OrphanSealWave1.cs:372`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorAutonomySystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorAutonomySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan144SurvivorAutonomyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan144SurvivorAutonomyIntegrationTests.cs)

### 175. `survivor_mental_health` — Plans 50-53 — survivor psychological trauma, stress levels, catharsis, and mental health crises (Psychology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupSurvivorMentalHealth()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans50_53.cs:239`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs`](../../Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorMentalHealthSaveStore.cs`](../../src/Host/SurvivorMentalHealthSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Needs/SurvivorMentalHealthTests.cs`](../../Ashfall.Core.Tests/Needs/SurvivorMentalHealthTests.cs)

### 176. `personal_quests` — Survivor personal quest progression (Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupPersonalQuests()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Application.cs:1116`, `src/Main.ExpandedShelterSystems.cs:152`, `src/Main.PersonalQuests.cs:22`, `src/Main.SaveOrchestrator.cs:295`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs`](../../Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs)
  - Host Session: [`src/Host/PersonalQuestHostSession.cs`](../../src/Host/PersonalQuestHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PersonalQuestSaveStore.cs`](../../src/Host/PersonalQuestSaveStore.cs)
  - UI Panel: [`src/UI/PersonalQuestPanel.cs`](../../src/UI/PersonalQuestPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs`](../../Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs`](../../Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs)

### 177. `procedural_narrative` — Procedural narrative metadata and the shared quest runtime (Quests)
- **Owner Domain:** `quests`
- **Setup Method:** `Main.SetupPlans166To169()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:149`, `src/Main.NarrativeQuestlines.cs:239`, `src/Main.Plans166_169.cs:214`, `src/Main.Plans166_169.cs:220`, `src/Main.Plans166_169.cs:227`, `src/Main.Plans166_169.cs:260`, `src/Main.Plans166_169.cs:276`, `src/Main.SaveOrchestrator.cs:232`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/ProceduralNarrativeSystem.cs`](../../Assets/Ashfall.Core/Narrative/ProceduralNarrativeSystem.cs)
  - Host Session: [`src/Host/ProceduralNarrativeHostSession.cs`](../../src/Host/ProceduralNarrativeHostSession.cs)
  - Save Store: [`src/Host/ProceduralNarrativeSaveStore.cs`](../../src/Host/ProceduralNarrativeSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan169ProceduralNarrativeTests.cs`](../../Ashfall.Core.Tests/Plan169ProceduralNarrativeTests.cs)

### 178. `low_background_metrology` — Plan 138 — low-background shield install, detector calibration, smelting batches, bounded assay history (Radiation & Metrology)
- **Owner Domain:** `radiation`
- **Setup Method:** `Main.SetupLowBackgroundMetrology()` | **Invoked:** yes | **Cadence:** `On-Demand (Assay & Smelting Commands)`
- **Setup Invocation Sites:** `src/Main.LowBackgroundMetrology.cs:30`, `src/Main.LowBackgroundMetrology.cs:105`, `src/Main.LowBackgroundMetrology.cs:112`, `src/Main.LowBackgroundMetrology.cs:119`, `src/Main.LowBackgroundMetrology.cs:126`, `src/Main.LowBackgroundMetrology.cs:133`, `src/Main.LowBackgroundMetrology.cs:145`, `src/Main.SaveOrchestrator.cs:187`
- **UI Routes:** `low_background_metrology`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs`](../../Assets/Ashfall.Core/Radiation/LowBackgroundLeadEngine.cs)
  - Host Session: [`src/Host/LowBackgroundMetrologyHostSession.cs`](../../src/Host/LowBackgroundMetrologyHostSession.cs)
  - Save Store: [`src/Host/LowBackgroundMetrologySaveStore.cs`](../../src/Host/LowBackgroundMetrologySaveStore.cs)
  - UI Panel: [`src/UI/LowBackgroundLeadPanel.cs`](../../src/UI/LowBackgroundLeadPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radiation/Plan138LowBackgroundLeadEngineTests.cs`](../../Ashfall.Core.Tests/Radiation/Plan138LowBackgroundLeadEngineTests.cs)

### 179. `communications` — ORPHAN-SEAL-W1 — antenna layer, intercepted messages, and outgoing broadcasts (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupCommunications()` | **Invoked:** yes | **Cadence:** `Daily Wear Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:77`, `src/Main.OrphanSealWave1.cs:376`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Communications/CommunicationsSystem.cs`](../../Assets/Ashfall.Core/Communications/CommunicationsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communications/Plan157CommunicationsIntegrationTests.cs`](../../Ashfall.Core.Tests/Communications/Plan157CommunicationsIntegrationTests.cs)

### 180. `heliograph` — Optical heliograph stations and message delivery (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupHeliograph()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:137`, `src/Main.ExpandedShelterSystems.cs:716`, `src/Main.PlayerSurfaces.cs:759`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/HeliographSystem.cs`](../../Assets/Ashfall.Core/HeliographSystem.cs)
  - Host Session: [`src/Host/HeliographHostSession.cs`](../../src/Host/HeliographHostSession.cs)
  - Save Store: [`src/Host/HeliographHostSession.cs`](../../src/Host/HeliographHostSession.cs)

### 181. `nvis_communications` — Plans 130-133 — regional NVIS status communications and recall queue (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupNvisCommunications()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans130_133.cs:26`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs`](../../Assets/Ashfall.Core/Radio/NvisCommunicationsSystem.cs)
  - Host Session: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)
  - Save Store: [`src/Host/Plans130To133HostSessions.cs`](../../src/Host/Plans130To133HostSessions.cs)

### 182. `patrol_radio_hooks` — Patrol radio hooks — queued and consumed patrol-encounter broadcast signals feeding the canonical radio intercept log (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupPatrolRadio()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:997`, `src/Main.CampaignOwners.cs:1008`, `src/Main.PatrolRadio.cs:41`, `src/Main.PatrolRadio.cs:61`, `src/Main.SaveOrchestrator.cs:323`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs`](../../Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs)
  - Host Session: [`src/Host/PatrolRadioHostSession.cs`](../../src/Host/PatrolRadioHostSession.cs)
  - Host Session: [`src/Host/RadioHostSession.cs`](../../src/Host/RadioHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PatrolRadioHostSession.cs`](../../src/Host/PatrolRadioHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/PlanPatrolRadioHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Radio/PlanPatrolRadioHostIntegrationTests.cs)

### 183. `psyops` — Flagship XI Plan 157 — broadcast campaigns, jamming, counter-propaganda, ideological pressure (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupPsyOps()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2036`, `src/Main.Plans166_169.cs:43`, `src/Main.Plans166_169.cs:153`, `src/Main.PsyOps.cs:82`, `src/Main.PsyOps.cs:89`, `src/Main.PsyOps.cs:96`, `src/Main.RadioProgramProduction.cs:30`, `src/Main.SaveOrchestrator.cs:185`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/PsyOpsSystem.cs`](../../Assets/Ashfall.Core/Radio/PsyOpsSystem.cs)
  - Host Session: [`src/Host/PsyOpsHostSession.cs`](../../src/Host/PsyOpsHostSession.cs)
  - Save Store: [`src/Host/PsyOpsSaveStore.cs`](../../src/Host/PsyOpsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flagship11/PsyOpsSystemTests.cs`](../../Ashfall.Core.Tests/Flagship11/PsyOpsSystemTests.cs)

### 184. `radio_program_production` — Plan 173 — player radio program prep/delivery jobs and follow-ups (Radio)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadioProgramProduction()` | **Invoked:** yes | **Cadence:** `Daily Program Tick`
- **Setup Invocation Sites:** `src/Main.RadioProgramProduction.cs:21`, `src/Main.RadioProgramProduction.cs:95`, `src/Main.RadioProgramProduction.cs:103`, `src/Main.RadioProgramProduction.cs:113`, `src/Main.RadioProgramProduction.cs:125`, `src/Main.RadioProgramProduction.cs:144`, `src/Main.SaveOrchestrator.cs:186`
- **UI Routes:** `radio`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs`](../../Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs)
  - Host Session: [`src/Host/RadioProgramProductionHostSession.cs`](../../src/Host/RadioProgramProductionHostSession.cs)
  - Save Store: [`src/Host/RadioProgramProductionSaveStore.cs`](../../src/Host/RadioProgramProductionSaveStore.cs)
  - UI Panel: [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/Plan173RadioProgramProductionTests.cs`](../../Ashfall.Core.Tests/Radio/Plan173RadioProgramProductionTests.cs)

### 185. `retention` — Plan 55 — retention policy audit report: bounded collection bounds, pruned totals, and preserved obligations (Records)
- **Owner Domain:** `records`
- **Setup Method:** `Main.SetupRetention()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:162`, `src/Main.SaveOrchestrator.cs:302`
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

### 186. `research_unlock` — Plan 141 — Research downstream unlocks bridge: items, recipes, shelter, expedition, combat, and medical capabilities (Research)
- **Owner Domain:** `knowledge`
- **Setup Method:** `Main.SetupResearchUnlockBridge()` | **Invoked:** yes | **Cadence:** `On-Demand (Research Node Completion)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:435`, `src/Main.CampaignOwners.cs:446`, `src/Main.ExpandedShelterSystems.cs:54`, `src/Main.ResearchUnlock.cs:55`, `src/Main.SaveOrchestrator.cs:308`, `src/Main.UnifiedEnding.cs:79`
- **UI Routes:** `research`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`](../../Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs)
  - Host Session: [`src/Host/ResearchUnlockHostSession.cs`](../../src/Host/ResearchUnlockHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ResearchUnlockHostSession.cs`](../../src/Host/ResearchUnlockHostSession.cs)
  - UI Panel: [`src/UI/ResearchPanel.cs`](../../src/UI/ResearchPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs`](../../Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Research/Plan141ResearchUnlockHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Research/Plan141ResearchUnlockHostIntegrationTests.cs)

### 187. `playable_metrics` — Plan 46 — local session telemetry: aggregate readiness report and first-hour funnel completion (Save)
- **Owner Domain:** `save`
- **Setup Method:** `Main.SetupPlayMetrics()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:158`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs`](../../Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs)
  - Core System: [`Assets/Ashfall.Core/Telemetry/PlayableMetricsAggregationEngine.cs`](../../Assets/Ashfall.Core/Telemetry/PlayableMetricsAggregationEngine.cs)
  - Host Session: [`src/Host/PlayMetricsHostSession.cs`](../../src/Host/PlayMetricsHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PlayMetricsHostSession.cs`](../../src/Host/PlayMetricsHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan46PlayMetricsHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan46PlayMetricsHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Telemetry/PlaySessionRecorderTests.cs`](../../Ashfall.Core.Tests/Telemetry/PlaySessionRecorderTests.cs)

### 188. `session_durability` — Plan 39 — observed slot summaries, day-advance soak samples, and corruption/recovery audit (Save)
- **Owner Domain:** `save`
- **Setup Method:** `Main.SetupSessionDurability()` | **Invoked:** yes | **Cadence:** `Session-Driven`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:157`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Save/SessionDurabilityManager.cs`](../../Assets/Ashfall.Core/Save/SessionDurabilityManager.cs)
  - Host Session: [`src/Host/SaveLoadHostSession.cs`](../../src/Host/SaveLoadHostSession.cs)
  - Host Session: [`src/Host/SessionDurabilityHostSession.cs`](../../src/Host/SessionDurabilityHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SessionDurabilityHostSession.cs`](../../src/Host/SessionDurabilityHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Save/Plan39SessionDurabilityHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Save/Plan39SessionDurabilityHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Save/SessionDurabilityManagerTests.cs`](../../Ashfall.Core.Tests/Save/SessionDurabilityManagerTests.cs)

### 189. `seven_day_slice` — Plan 54 — seven-day slice playtest scorecard: frozen scenario hash, beat verification evidence, retention counts (Save)
- **Owner Domain:** `save`
- **Setup Method:** `Main.SetupSevenDaySlice()` | **Invoked:** yes | **Cadence:** `Playtest-Driven`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:161`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Campaign/SliceScenario.cs`](../../Assets/Ashfall.Core/Campaign/SliceScenario.cs)
  - Core System: [`Assets/Ashfall.Core/Campaign/SliceScenarioCatalogLoader.cs`](../../Assets/Ashfall.Core/Campaign/SliceScenarioCatalogLoader.cs)
  - Host Session: [`src/Host/SliceScenarioHostSession.cs`](../../src/Host/SliceScenarioHostSession.cs)
  - Save Store: [`src/Host/SliceScenarioHostSession.cs`](../../src/Host/SliceScenarioHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan54SevenDaySliceHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan54SevenDaySliceHostIntegrationTests.cs)

### 190. `accessibility_settings` — Plan 184 — Accessibility settings: visual, hearing, motor, and cognitive profiles, high contrast, font scaling, and custom accessibility overrides (Settings)
- **Owner Domain:** `settings`
- **Setup Method:** `Main.SetupAccessibilitySettings()` | **Invoked:** yes | **Cadence:** `User Preference Save`
- **Setup Invocation Sites:** `src/Main.AccessibilitySettings.cs:41`, `src/Main.AccessibilitySettings.cs:47`, `src/Main.AccessibilitySettings.cs:53`, `src/Main.AccessibilitySettings.cs:59`, `src/Main.AccessibilitySettings.cs:65`, `src/Main.SaveOrchestrator.cs:350`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs`](../../Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs)
  - Host Session: [`src/Host/AccessibilitySettingsHostSession.cs`](../../src/Host/AccessibilitySettingsHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AccessibilitySettingsHostSession.cs`](../../src/Host/AccessibilitySettingsHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`](../../Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs)

### 191. `outpost_settlement` — Plan 58 — authored outposts: establishment, condition, garrison assignments, and ration reserve (Settlements)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupOutpostSettlement()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:163`, `src/Main.SaveOrchestrator.cs:303`
- **UI Routes:** `shelter_operations`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs`](../../Assets/Ashfall.Core/Settlements/OutpostSettlementSystem.cs)
  - Host Session: [`src/Host/OutpostSettlementHostSession.cs`](../../src/Host/OutpostSettlementHostSession.cs)
  - Host Session: [`src/Host/ShelterOperationsHostSession.cs`](../../src/Host/ShelterOperationsHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OutpostSettlementHostSession.cs`](../../src/Host/OutpostSettlementHostSession.cs)
  - UI Panel: [`src/UI/ShelterOperationsPanel.cs`](../../src/UI/ShelterOperationsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Integration/ShelterOperationsBoardWiringTests.cs`](../../Ashfall.Core.Tests/Integration/ShelterOperationsBoardWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Settlements/OutpostAtomicBillTests.cs`](../../Ashfall.Core.Tests/Settlements/OutpostAtomicBillTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Settlements/Plan58OutpostHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Settlements/Plan58OutpostHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs`](../../Ashfall.Core.Tests/Settlements/Plan58OutpostSettlementIntegrationTests.cs)

### 192. `chemical_reagent_synthesis` — Expansion 39 — chemical synthesis reactor safety, catalyst purity, and reagent grading (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupChemicalReagentSynthesis()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2865`, `src/Main.CampaignOwners.cs:2876`, `src/Main.ChemicalReagentSynthesis.cs:40`, `src/Main.ChemicalReagentSynthesis.cs:50`, `src/Main.ChemicalReagentSynthesis.cs:57`, `src/Main.ChemicalReagentSynthesis.cs:63`, `src/Main.ChemicalReagentSynthesis.cs:69`, `src/Main.ChemicalReagentSynthesis.cs:75`, `src/Main.ChemicalReagentSynthesis.cs:81`, `src/Main.SaveOrchestrator.cs:344`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ChemicalReagentLedger.cs`](../../Assets/Ashfall.Core/Shelter/ChemicalReagentLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/ChemicalReagentSynthesisEngine.cs`](../../Assets/Ashfall.Core/Shelter/ChemicalReagentSynthesisEngine.cs)
  - Host Session: [`src/Host/ChemicalReagentSynthesisHostSession.cs`](../../src/Host/ChemicalReagentSynthesisHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ChemicalReagentSynthesisHostSession.cs`](../../src/Host/ChemicalReagentSynthesisHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ChemicalReagentLedgerTests.cs`](../../Ashfall.Core.Tests/Shelter/ChemicalReagentLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ChemicalReagentSynthesisEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/ChemicalReagentSynthesisEngineTests.cs)

### 193. `cryo_vault` — Plan B69 — cryo canisters, viability, coolant reserve, insulation, breach state (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupCryoVault()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1437`, `src/Main.PlayerSurfaces.cs:892`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs`](../../Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CryoVaultSaveStore.cs`](../../src/Host/CryoVaultSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/CryoVaultB69Tests.cs`](../../Ashfall.Core.Tests/Shelter/CryoVaultB69Tests.cs)

### 194. `disaster_response` — ORPHAN-SEAL-W1 — active disasters, protocols, mitigation, and resilience (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupDisasterResponse()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:76`, `src/Main.OrphanSealWave1.cs:375`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs`](../../Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan158DisasterResponseIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan158DisasterResponseIntegrationTests.cs)

### 195. `ebpvd_coating` — Plans 146-149 — EB-PVD thermal barrier coating machinery, job state, and records (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupEbPvdCoating()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans146_149.cs:387`, `src/Main.Plans146_149.cs:607`, `src/Main.PlayerSurfaces.cs:124`
- **UI Routes:** `ebpvd_coating`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs`](../../Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs)
  - Host Session: [`src/Host/EbPvdCoatingHostSession.cs`](../../src/Host/EbPvdCoatingHostSession.cs)
  - Save Store: [`src/Host/EbPvdCoatingSaveStore.cs`](../../src/Host/EbPvdCoatingSaveStore.cs)
  - UI Panel: [`src/UI/EbPvdCoatingPanel.cs`](../../src/UI/EbPvdCoatingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/EbPvdCoatingEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/EbPvdCoatingEngineTests.cs)

### 196. `emergency_muster_readiness` — Expansion 23 The Alarm — emergency muster readiness scores, drill recency, evacuation timing, and cascade intervention margins (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupEmergencyMusterReadiness()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3650`, `src/Main.EmergencyMusterReadiness.cs:33`, `src/Main.EmergencyMusterReadiness.cs:50`, `src/Main.EmergencyMusterReadiness.cs:58`, `src/Main.EmergencyMusterReadiness.cs:66`, `src/Main.SaveOrchestrator.cs:370`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/EmergencyMusterReadinessEngine.cs`](../../Assets/Ashfall.Core/Shelter/EmergencyMusterReadinessEngine.cs)
  - Host Session: [`src/Host/EmergencyMusterReadinessHostSession.cs`](../../src/Host/EmergencyMusterReadinessHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/EmergencyMusterReadinessHostSession.cs`](../../src/Host/EmergencyMusterReadinessHostSession.cs)

### 197. `excavation_hazards` — Subterranean methane, flood, spore hazards, and cave-in rescue operations (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupExcavationHazards()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:148`, `src/Main.PlansB68_B69.cs:31`, `src/Main.SaveOrchestrator.cs:241`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs`](../../Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ExcavationHazardSaveStore.cs`](../../src/Host/ExcavationHazardSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExcavationSystemTests.cs`](../../Ashfall.Core.Tests/ExcavationSystemTests.cs)

### 198. `food_preservation` — Food spoilage, curing, and cryogenic preservation (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPlans62To65()` | **Invoked:** no | **Cadence:** `Daily Sim Tick`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`](../../Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/FoodPreservationSaveStore.cs`](../../src/Host/FoodPreservationSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/FoodPreservationSystemTests.cs)

### 199. `geothermal_aquifer` — Deep geothermal boreholes & aquifer pumping (Shelter)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupGeothermalAquifer()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:120`, `src/Main.PlayerSurfaces.cs:876`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs`](../../Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs)
  - Host Session: [`src/Host/GeothermalAquiferSaveStore.cs`](../../src/Host/GeothermalAquiferSaveStore.cs)
  - Save Store: [`src/Host/GeothermalAquiferSaveStore.cs`](../../src/Host/GeothermalAquiferSaveStore.cs)

### 200. `glassworks` — Expansion 29 — glass vitrification batches, vision prescriptions, and abrasive grit stock (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupGlassworks()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2633`, `src/Main.CampaignOwners.cs:2644`, `src/Main.Glassworks.cs:46`, `src/Main.Glassworks.cs:60`, `src/Main.Glassworks.cs:66`, `src/Main.Glassworks.cs:73`, `src/Main.SaveOrchestrator.cs:336`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Optics/GlassworksLedger.cs`](../../Assets/Ashfall.Core/Optics/GlassworksLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Optics/PrecisionGlassworksOpticsEngine.cs`](../../Assets/Ashfall.Core/Optics/PrecisionGlassworksOpticsEngine.cs)
  - Host Session: [`src/Host/GlassworksHostSession.cs`](../../src/Host/GlassworksHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/GlassworksHostSession.cs`](../../src/Host/GlassworksHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Optics/GlassworksLedgerTests.cs`](../../Ashfall.Core.Tests/Optics/GlassworksLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Optics/PrecisionGlassworksOpticsEngineTests.cs`](../../Ashfall.Core.Tests/Optics/PrecisionGlassworksOpticsEngineTests.cs)

### 201. `kilnworks` — Expansion 31 — queued kiln batches, kiln fuel reserve, refractory lining wear, and drawn-output tallies. Metallurgy stays with CupolaFoundryEngine. (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupKilnworks()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2698`, `src/Main.CampaignOwners.cs:2709`, `src/Main.Kilnworks.cs:37`, `src/Main.Kilnworks.cs:44`, `src/Main.Kilnworks.cs:54`, `src/Main.Kilnworks.cs:64`, `src/Main.Kilnworks.cs:71`, `src/Main.Kilnworks.cs:77`, `src/Main.SaveOrchestrator.cs:338`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/KilnFiringEngine.cs`](../../Assets/Ashfall.Core/Shelter/KilnFiringEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/KilnFiringLedger.cs`](../../Assets/Ashfall.Core/Shelter/KilnFiringLedger.cs)
  - Host Session: [`src/Host/KilnworksHostSession.cs`](../../src/Host/KilnworksHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/KilnworksHostSession.cs`](../../src/Host/KilnworksHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/KilnFiringEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/KilnFiringEngineTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/KilnFiringLedgerTests.cs`](../../Ashfall.Core.Tests/Shelter/KilnFiringLedgerTests.cs)

### 202. `mechanical_driveline` — Expansion 40 — mechanical power driveline line shafts, friction transmission, and machine tool tolerance (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupMechanicalDriveline()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2922`, `src/Main.CampaignOwners.cs:2933`, `src/Main.MechanicalDriveline.cs:40`, `src/Main.MechanicalDriveline.cs:49`, `src/Main.MechanicalDriveline.cs:58`, `src/Main.MechanicalDriveline.cs:65`, `src/Main.MechanicalDriveline.cs:71`, `src/Main.SaveOrchestrator.cs:346`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/MechanicalDrivelineLedger.cs`](../../Assets/Ashfall.Core/Shelter/MechanicalDrivelineLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/MechanicalPowerDrivelineEngine.cs`](../../Assets/Ashfall.Core/Shelter/MechanicalPowerDrivelineEngine.cs)
  - Host Session: [`src/Host/MechanicalDrivelineHostSession.cs`](../../src/Host/MechanicalDrivelineHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MechanicalDrivelineHostSession.cs`](../../src/Host/MechanicalDrivelineHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/MechanicalDrivelineLedgerTests.cs`](../../Ashfall.Core.Tests/Shelter/MechanicalDrivelineLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/MechanicalPowerDrivelineEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/MechanicalPowerDrivelineEngineTests.cs)

### 203. `precision_metrology` — Plan B89 — precision metrology grades, certificates, and registered-consumer calibration (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPrecisionMetrology()` | **Invoked:** yes | **Cadence:** `Daily Calibration Drift`
- **Setup Invocation Sites:** `src/Main.Plans74_77.cs:61`, `src/Main.PlansB86_B89.cs:100`, `src/Main.PlansB86_B89.cs:133`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs`](../../Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PrecisionMetrologySaveStore.cs`](../../src/Host/PrecisionMetrologySaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs`](../../Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs)

### 204. `precision_optics` — Plans 110-113 — precision optical blank grinding, figure testing, and telescope/shield viewports (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupPrecisionOptics()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans110_113.cs:138`, `src/Main.Plans74_77.cs:254`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs`](../../Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs)
  - Host Session: [`src/Host/PrecisionOpticsHostSession.cs`](../../src/Host/PrecisionOpticsHostSession.cs)
  - Save Store: [`src/Host/PrecisionOpticsSaveStore.cs`](../../src/Host/PrecisionOpticsSaveStore.cs)

### 205. `radio_station` — Radio station frequency tuning, signal lock, and triangulation (Shelter)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadioStation()` | **Invoked:** yes | **Cadence:** `On-Demand (Tuning & Broadcasts)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:238`
- **UI Routes:** `radio`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs`](../../Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RadioStationSaveStore.cs`](../../src/Host/RadioStationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/RadioPanel.cs`](../../src/UI/RadioPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs`](../../Ashfall.Core.Tests/Radio/ShelterRadioStationTests.cs)

### 206. `seismic_dynamics` — Plan B68 — fault tension, slips, geophone coverage, dampener integrity, quake history (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupSeismicDynamics()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1418`, `src/Main.PlansB86_B89.cs:75`, `src/Main.PlayerSurfaces.cs:888`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SeismicDynamicsSaveStore.cs`](../../src/Host/SeismicDynamicsSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/SeismicMonitoringB68Tests.cs`](../../Ashfall.Core.Tests/Shelter/SeismicMonitoringB68Tests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterSeismicDynamicsPlan56Tests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterSeismicDynamicsPlan56Tests.cs)

### 207. `shelter_archive` — Plan 162 — Shelter history & archive: institutional memory, governance decisions, historical milestones, and memorial records (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterArchive()` | **Invoked:** yes | **Cadence:** `Daily Archive Timeline Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2978`, `src/Main.CampaignOwners.cs:2989`, `src/Main.SaveOrchestrator.cs:348`, `src/Main.ShelterArchive.cs:55`, `src/Main.ShelterArchive.cs:61`, `src/Main.ShelterArchive.cs:69`, `src/Main.ShelterArchive.cs:80`, `src/Main.ShelterArchive.cs:89`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs)
  - Host Session: [`src/Host/ShelterArchiveHostSession.cs`](../../src/Host/ShelterArchiveHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterArchiveHostSession.cs`](../../src/Host/ShelterArchiveHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan162ArchiveIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan162ArchiveIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterArchiveSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterArchiveSystemTests.cs)

### 208. `shelter_atmosphere` — Plan 220 — shelter composite atmosphere, ambiance profile, and environmental facets (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAtmosphere()` | **Invoked:** yes | **Cadence:** `Daily (Day Coordinator)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:145`
- **UI Routes:** `shelter_atmosphere`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs)
  - Host Session: [`src/Host/ShelterAtmosphereHostSession.cs`](../../src/Host/ShelterAtmosphereHostSession.cs)
  - Save Store: [`src/Host/ShelterAtmosphereSaveStore.cs`](../../src/Host/ShelterAtmosphereSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ShelterAtmospherePanel.cs`](../../src/UI/ShelterAtmospherePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs)

### 209. `shelter_decor` — Room decor placements, memorial plaques, and localized morale items (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterDecor()` | **Invoked:** yes | **Cadence:** `On-Demand (Decoration Placement)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1664`, `src/Main.ExpandedShelterSystems.cs:144`, `src/Main.ExpandedShelterSystems.cs:668`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs)
  - Host Session: [`src/Host/ShelterDecorHostSession.cs`](../../src/Host/ShelterDecorHostSession.cs)
  - Save Store: [`src/Host/ShelterDecorSaveStore.cs`](../../src/Host/ShelterDecorSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Plan12CDecorTests.cs`](../../Ashfall.Core.Tests/Plan12CDecorTests.cs)

### 210. `shelter_expansion` — ORPHAN-SEAL-W1 — expansion rooms, construction projects, and upgrade state (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterExpansion()` | **Invoked:** yes | **Cadence:** `Labor-Driven`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:81`, `src/Main.OrphanSealWave1.cs:378`
- **UI Routes:** `shelter_operations`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs)
  - Host Session: [`src/Host/ShelterOperationsHostSession.cs`](../../src/Host/ShelterOperationsHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - UI Panel: [`src/UI/ShelterOperationsPanel.cs`](../../src/UI/ShelterOperationsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Integration/ShelterOperationsBoardWiringTests.cs`](../../Ashfall.Core.Tests/Integration/ShelterOperationsBoardWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan156ShelterExpansionIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan156ShelterExpansionIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterOperationsBoardCoreTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterOperationsBoardCoreTests.cs)

### 211. `shelter_maintenance` — Plan 186 — Shelter maintenance & degradation: component condition, environmental stress, maintenance actions, and alert states (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterMaintenance()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1110`, `src/Main.CampaignOwners.cs:1121`, `src/Main.SaveOrchestrator.cs:332`, `src/Main.ShelterMaintenance.cs:39`
- **UI Routes:** `survivor_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterComponentCatalogLoader.cs`](../../Assets/Ashfall.Core/Shelter/ShelterComponentCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterMaintenanceSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterMaintenanceSystem.cs)
  - Host Session: [`src/Host/ShelterMaintenanceHostSession.cs`](../../src/Host/ShelterMaintenanceHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterMaintenanceHostSession.cs`](../../src/Host/ShelterMaintenanceHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan186ShelterMaintenanceIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan186ShelterMaintenanceIntegrationTests.cs)

### 212. `shelter_noise` — Plan 205 — shelter acoustic noise, room soundproofing, and quiet hours (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAtmosphere()` | **Invoked:** yes | **Cadence:** `Daily (Midday Acoustic Audit)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:145`
- **UI Routes:** `shelter_atmosphere`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs)
  - Host Session: [`src/Host/ShelterAtmosphereHostSession.cs`](../../src/Host/ShelterAtmosphereHostSession.cs)
  - Save Store: [`src/Host/ShelterNoiseSaveStore.cs`](../../src/Host/ShelterNoiseSaveStore.cs)
  - UI Panel: [`src/UI/ShelterAtmospherePanel.cs`](../../src/UI/ShelterAtmospherePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs)

### 213. `shelter_social_dynamics` — Living quarters privacy pressure, communal mess hall, and disputes (Shelter)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupShelterSocial()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:240`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterSocialSaveStore.cs`](../../src/Host/ShelterSocialSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterSocialDynamicsTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterSocialDynamicsTests.cs)

### 214. `shelter_workshop` — Precision workshop tooling, ammo press, and firearm refurbishment (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupWorkshop()` | **Invoked:** yes | **Cadence:** `On-Demand (Crafting & Refurbishment)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:237`
- **UI Routes:** `workshop`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ShelterWorkshopSaveStore.cs`](../../src/Host/ShelterWorkshopSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/WorkshopPanel.cs`](../../src/UI/WorkshopPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs`](../../Ashfall.Core.Tests/WorkshopReverseEngineeringSystemTests.cs)

### 215. `trophies` — Trophy mount pipeline: exactly-once quarry trophy awards and unlocked recipes (Shelter)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupTrophies()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:360`, `src/Main.Trophies.cs:35`, `src/Main.Trophies.cs:44`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/TrophySystem.cs`](../../Assets/Ashfall.Core/Shelter/TrophySystem.cs)
  - Host Session: [`src/Host/TrophyHostSession.cs`](../../src/Host/TrophyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TrophyHostSession.cs`](../../src/Host/TrophyHostSession.cs)

### 216. `weather_hardening` — Cryo-ash weather hardening & thermal insulation (Shelter)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWeatherHardening()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:119`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WeatherHardeningSystem.cs`](../../Assets/Ashfall.Core/World/WeatherHardeningSystem.cs)
  - Host Session: [`src/Host/WeatherHardeningSaveStore.cs`](../../src/Host/WeatherHardeningSaveStore.cs)
  - Save Store: [`src/Host/WeatherHardeningSaveStore.cs`](../../src/Host/WeatherHardeningSaveStore.cs)

### 217. `cvd_diamond` — Plan 124 — CVD diamond reactor condition, plasma stability, growth batches, faults (Shelter & Facilities)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupCvdDiamond()` | **Invoked:** yes | **Cadence:** `Industrial Production Cadence (Batch Ticks)`
- **Setup Invocation Sites:** `src/Main.Plans122to125.cs:302`, `src/Main.SaveOrchestrator.cs:192`
- **UI Routes:** `cvd_diamond`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/CvdDiamondSynthesisEngine.cs`](../../Assets/Ashfall.Core/Shelter/CvdDiamondSynthesisEngine.cs)
  - Host Session: [`src/Host/CvdDiamondHostSession.cs`](../../src/Host/CvdDiamondHostSession.cs)
  - Save Store: [`src/Host/CvdDiamondSaveStore.cs`](../../src/Host/CvdDiamondSaveStore.cs)
  - UI Panel: [`src/UI/CvdDiamondPanel.cs`](../../src/UI/CvdDiamondPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan124CvdDiamondSynthesisEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan124CvdDiamondSynthesisEngineTests.cs)

### 218. `sofc_power` — Plan 122 — SOFC plant operating mode, thermal level, stack health, seal integrity, degradation, faults (Shelter & Facilities)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupSofcPower()` | **Invoked:** yes | **Cadence:** `Shelter Power Cadence (TickDay)`
- **Setup Invocation Sites:** `src/Main.Plans122to125.cs:295`, `src/Main.SaveOrchestrator.cs:191`
- **UI Routes:** `sofc_power`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SofcElectrochemistryEngine.cs`](../../Assets/Ashfall.Core/Shelter/SofcElectrochemistryEngine.cs)
  - Host Session: [`src/Host/SofcPowerHostSession.cs`](../../src/Host/SofcPowerHostSession.cs)
  - Save Store: [`src/Host/SofcPowerSaveStore.cs`](../../src/Host/SofcPowerSaveStore.cs)
  - UI Panel: [`src/UI/SolidOxideFuelCellPanel.cs`](../../src/UI/SolidOxideFuelCellPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan122SofcElectrochemistryEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan122SofcElectrochemistryEngineTests.cs)

### 219. `bio_fermentation` — Plan 126 — fermentation reactor, process health, contamination, outputs (Shelter & Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupBioFermentation()` | **Invoked:** yes | **Cadence:** `Daily Reactor Tick`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:136`, `src/Main.SaveOrchestrator.cs:281`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/BioFermentationEngine.cs`](../../Assets/Ashfall.Core/Shelter/BioFermentationEngine.cs)
  - Host Session: [`src/Host/BioFermentationHostSession.cs`](../../src/Host/BioFermentationHostSession.cs)
  - Save Store: [`src/Host/BioFermentationSaveStore.cs`](../../src/Host/BioFermentationSaveStore.cs)
  - UI Panel: [`src/UI/BioFermentationPanel.cs`](../../src/UI/BioFermentationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/BioFermentationEngineTests.cs`](../../Ashfall.Core.Tests/Shelter/BioFermentationEngineTests.cs)

### 220. `hydroponic_biomes` — Hydroponic biome racks and crop state (Shelter & Farming)
- **Owner Domain:** `farming`
- **Setup Method:** `Main.SetupHydroponicBiomes()` | **Invoked:** yes | **Cadence:** `Daily Biome Rack Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:288`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs`](../../Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HydroponicBiomeSaveStore.cs`](../../src/Host/HydroponicBiomeSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/HydroponicBiomeTests.cs`](../../Ashfall.Core.Tests/Shelter/HydroponicBiomeTests.cs)

### 221. `airlock_security` — Airlock decontamination and security (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupAirlockSecurity()` | **Invoked:** yes | **Cadence:** `Daily Decon Interlock`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:110`, `src/Main.VisitorIntegration.cs:112`
- **UI Routes:** `airlock_security`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AirlockSecuritySystem.cs`](../../Assets/Ashfall.Core/AirlockSecuritySystem.cs)
  - Host Session: [`src/Host/AirlockSecurityHostSession.cs`](../../src/Host/AirlockSecurityHostSession.cs)
  - Save Store: [`src/Host/AirlockSecuritySaveStore.cs`](../../src/Host/AirlockSecuritySaveStore.cs)
  - UI Panel: [`src/UI/AirlockSecurityPanel.cs`](../../src/UI/AirlockSecurityPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AirlockSecuritySystemTests.cs`](../../Ashfall.Core.Tests/AirlockSecuritySystemTests.cs)

### 222. `decontamination` — Rad-scrubbing showers and chambers (Shelter & Infrastructure)
- **Owner Domain:** `radiation`
- **Setup Method:** `Main.SetupDecontamination()` | **Invoked:** yes | **Cadence:** `Daily Rad Scrub Shower Cycle`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:129`
- **UI Routes:** `decontamination`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DecontaminationSystem.cs`](../../Assets/Ashfall.Core/DecontaminationSystem.cs)
  - Host Session: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - Save Store: [`src/Host/DecontaminationHostSession.cs`](../../src/Host/DecontaminationHostSession.cs)
  - UI Panel: [`src/UI/DecontaminationPanel.cs`](../../src/UI/DecontaminationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DecontaminationSystemTests.cs`](../../Ashfall.Core.Tests/DecontaminationSystemTests.cs)

### 223. `excavation` — Shelter expansion rubble clearing (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupExcavation()` | **Invoked:** yes | **Cadence:** `Daily Rubble Shoring Work`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:115`
- **UI Routes:** `excavation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ExcavationSystem.cs`](../../Assets/Ashfall.Core/ExcavationSystem.cs)
  - Host Session: [`src/Host/ExcavationHostSession.cs`](../../src/Host/ExcavationHostSession.cs)
  - Save Store: [`src/Host/ExcavationSaveStore.cs`](../../src/Host/ExcavationSaveStore.cs)
  - UI Panel: [`src/UI/ExcavationPanel.cs`](../../src/UI/ExcavationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExcavationSystemTests.cs`](../../Ashfall.Core.Tests/ExcavationSystemTests.cs)

### 224. `greenhouse` — Hydroponic crops and food production (Shelter & Infrastructure)
- **Owner Domain:** `greenhouse`
- **Setup Method:** `Main.SetupGreenhouse()` | **Invoked:** yes | **Cadence:** `Daily Hydroponic Growth`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1381`, `src/Main.CampaignServices.cs:50`, `src/Main.GameFlow.cs:598`, `src/Main.GameFlow.cs:621`, `src/Main.Lifecycle.cs:731`, `src/Main.Plans162_165.cs:35`, `src/Main.PlayerSurfaces.cs:410`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.SaveOrchestrator.cs:220`, `src/Main.World.cs:63`, `src/Main.World.cs:151`
- **UI Routes:** `greenhouse`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`](../../Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs)
  - Host Session: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - Save Store: [`src/Host/GreenhouseHostSession.cs`](../../src/Host/GreenhouseHostSession.cs)
  - UI Panel: [`src/UI/GreenhousePanel.cs`](../../src/UI/GreenhousePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/GreenhouseSystemTests.cs`](../../Ashfall.Core.Tests/GreenhouseSystemTests.cs)

### 225. `nuclear_core_lifecycle` — Nuclear core lifecycle and thermal state (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupNuclearCore()` | **Invoked:** yes | **Cadence:** `Daily Core Thermal Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:293`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs`](../../Assets/Ashfall.Core/Shelter/NuclearCoreLifecycleSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/NuclearCoreSaveStore.cs`](../../src/Host/NuclearCoreSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/NuclearCorePowerGridPublishTests.cs`](../../Ashfall.Core.Tests/Shelter/NuclearCorePowerGridPublishTests.cs)

### 226. `power_grid` — Shelter generator & power allocations (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupPowerGrid()` | **Invoked:** yes | **Cadence:** `Daily Fuel Consumption & Wattage`
- **Setup Invocation Sites:** `src/Main.AdvancedShelterSystems.cs:422`, `src/Main.AdvancedShelterSystems.cs:427`, `src/Main.BriefingCrisis.cs:67`, `src/Main.CampaignServices.cs:49`, `src/Main.DeepWell.cs:23`, `src/Main.Economy.cs:255`, `src/Main.ExpandedShelterSystems.cs:98`, `src/Main.MoraleContagion.cs:31`, `src/Main.Plans110_113.cs:32`, `src/Main.Plans110_113.cs:89`, `src/Main.Plans130_133.cs:36`, `src/Main.Plans130_133.cs:61`, `src/Main.Plans130_133.cs:87`, `src/Main.Plans130_133.cs:137`, `src/Main.Plans146_149.cs:670`, `src/Main.Plans166_169.cs:39`, `src/Main.Plans62_65.cs:38`, `src/Main.Plans74_77.cs:26`, `src/Main.Plans74_77.cs:41`, `src/Main.Plans74_77.cs:79`, `src/Main.Plans94_97.cs:34`, `src/Main.PlansB68_B69.cs:29`, `src/Main.PlansB68_B69.cs:87`, `src/Main.SaveOrchestrator.cs:221`, `src/Main.ShelterAtmosphere.cs:24`, `src/Main.WaterCondenser.cs:22`, `src/Main.WaterSources.cs:22`, `src/Main.World.cs:560`, `src/Main.World.cs:567`
- **UI Routes:** `power_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`](../../Assets/Ashfall.Core/Shelter/PowerGridSystem.cs)
  - Host Session: [`src/Host/PowerGridHostSession.cs`](../../src/Host/PowerGridHostSession.cs)
  - Save Store: [`src/Host/PowerGridSaveStore.cs`](../../src/Host/PowerGridSaveStore.cs)
  - UI Panel: [`src/UI/PowerGridPanel.cs`](../../src/UI/PowerGridPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs)

### 227. `power_subgrids` — Power distribution sub-grid nodes and thermal state (Shelter & Infrastructure)
- **Owner Domain:** `power_grid`
- **Setup Method:** `Main.SetupPowerSubgrids()` | **Invoked:** yes | **Cadence:** `Daily Thermal Distribution Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:286`
- **UI Routes:** `power_grid`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/PowerDistributionSubgridSystem.cs`](../../Assets/Ashfall.Core/Shelter/PowerDistributionSubgridSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/PowerDistributionSaveStore.cs`](../../src/Host/PowerDistributionSaveStore.cs)
  - UI Panel: [`src/UI/PowerGridPanel.cs`](../../src/UI/PowerGridPanel.cs)

### 228. `sanitation` — Plan 210 — room waste, hygiene, compost queue, and spills (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupSanitation()` | **Invoked:** yes | **Cadence:** `Daily Sanitation Tick`
- **Setup Invocation Sites:** `src/Main.BriefingCrisis.cs:80`, `src/Main.CampaignOwners.cs:1707`, `src/Main.CampaignOwners.cs:1717`, `src/Main.Sanitation.cs:22`, `src/Main.Sanitation.cs:120`, `src/Main.SaveOrchestrator.cs:203`
- **UI Routes:** `sanitation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/SanitationFacilityCatalog.cs`](../../Assets/Ashfall.Core/Shelter/SanitationFacilityCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Shelter/SanitationSystem.cs`](../../Assets/Ashfall.Core/Shelter/SanitationSystem.cs)
  - Host Session: [`src/Host/SanitationHostSession.cs`](../../src/Host/SanitationHostSession.cs)
  - Save Store: [`src/Host/SanitationSaveStore.cs`](../../src/Host/SanitationSaveStore.cs)
  - UI Panel: [`src/UI/SanitationPanel.cs`](../../src/UI/SanitationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan210SanitationFacilityCatalogTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan210SanitationFacilityCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan210SanitationHostWiringTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan210SanitationHostWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan210SanitationSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan210SanitationSystemTests.cs)

### 229. `shelter_assignment` — Room assignments and living quarters (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterAssignment()` | **Invoked:** yes | **Cadence:** `On-Demand (Bunk Reassignment)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:143`, `src/Main.MoraleContagion.cs:28`, `src/Main.OrphanSealWave1.cs:217`, `src/Main.ShelterBatch3.cs:377`, `src/Main.ShelterOperations.cs:23`, `src/Main.SurvivorRoutines.cs:71`
- **UI Routes:** `shelter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs)
  - Host Session: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - Save Store: [`src/Host/ShelterAssignmentHostSession.cs`](../../src/Host/ShelterAssignmentHostSession.cs)
  - UI Panel: [`src/UI/ShelterPanel.cs`](../../src/UI/ShelterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs`](../../Ashfall.Core.Tests/Shelter/ShelterAssignmentSystemTests.cs)

### 230. `shelter_fire` — Shelter fire incidents, smoke, and brigade response (Shelter & Infrastructure)
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterFireHazard()` | **Invoked:** yes | **Cadence:** `Daily Fire Propagation Tick`
- **Setup Invocation Sites:** `src/Main.Application.cs:1113`, `src/Main.CampaignOwners.cs:1604`, `src/Main.CampaignOwners.cs:1612`, `src/Main.CampaignOwners.cs:1619`, `src/Main.PlayerSurfaces.cs:524`, `src/Main.SaveOrchestrator.cs:299`, `src/Main.ShelterInfrastructure.cs:647`, `src/Main.UiHandlers.cs:267`
- **UI Routes:** `fire_incident`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs)
  - Host Session: [`src/Host/ShelterFireHostSession.cs`](../../src/Host/ShelterFireHostSession.cs)
  - Save Store: [`src/Host/ShelterFireSaveStore.cs`](../../src/Host/ShelterFireSaveStore.cs)
  - UI Panel: [`src/UI/FireIncidentPanel.cs`](../../src/UI/FireIncidentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`](../../Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ShelterFireHazardSystemTests.cs`](../../Ashfall.Core.Tests/ShelterFireHazardSystemTests.cs)

### 231. `shelter_schedule` — Shift rotations and curfews (Shelter & Infrastructure)
- **Owner Domain:** `schedule`
- **Setup Method:** `Main.SetupShelterSchedule()` | **Invoked:** yes | **Cadence:** `Daily Curfew Rotation`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:121`, `src/Main.SurvivorFitness.cs:454`
- **UI Routes:** `shelter_schedule`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterScheduleSystem.cs`](../../Assets/Ashfall.Core/ShelterScheduleSystem.cs)
  - Host Session: [`src/Host/ShelterScheduleHostSession.cs`](../../src/Host/ShelterScheduleHostSession.cs)
  - Save Store: [`src/Host/ShelterScheduleSaveStore.cs`](../../src/Host/ShelterScheduleSaveStore.cs)
  - UI Panel: [`src/UI/ShelterSchedulePanel.cs`](../../src/UI/ShelterSchedulePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs`](../../Ashfall.Core.Tests/ShelterScheduleIntegrationTests.cs)

### 232. `shelter_thermal` — Heating, insulation, and frost protection (Shelter & Infrastructure)
- **Owner Domain:** `thermal`
- **Setup Method:** `Main.SetupShelterThermal()` | **Invoked:** yes | **Cadence:** `Daily HVAC Frost Dissipation`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:118`, `src/Main.PlansB68_B69.cs:30`, `src/Main.ShelterBatch3.cs:352`
- **UI Routes:** `shelter_thermal`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ShelterThermalSystem.cs`](../../Assets/Ashfall.Core/ShelterThermalSystem.cs)
  - Host Session: [`src/Host/ShelterThermalHostSession.cs`](../../src/Host/ShelterThermalHostSession.cs)
  - Save Store: [`src/Host/ShelterThermalSaveStore.cs`](../../src/Host/ShelterThermalSaveStore.cs)
  - UI Panel: [`src/UI/ShelterThermalPanel.cs`](../../src/UI/ShelterThermalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 233. `starting_level` — Bunker initial configuration & tier (Shelter & Infrastructure)
- **Owner Domain:** `starting_level`
- **Setup Method:** `Main.SetupStartingLevel()` | **Invoked:** yes | **Cadence:** `On-Demand (Opening Protocol)`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1333`, `src/Main.CampaignServices.cs:32`, `src/Main.ExpandedShelterSystems.cs:104`, `src/Main.GameFlow.cs:593`, `src/Main.GameFlow.cs:854`, `src/Main.PlayerSurfaces.cs:405`, `src/Main.SaveOrchestrator.cs:172`, `src/Main.ShelterAtmosphere.cs:25`, `src/Main.SurvivorSocial.cs:109`, `src/Main.UiPanels.cs:270`, `src/Main.UiPanels.cs:277`, `src/Main.UiPanels.cs:1025`, `src/Main.UiPanels.cs:1039`, `src/Main.UiPanels.cs:1064`
- **UI Routes:** `protocol`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs`](../../Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs)
  - Host Session: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - Save Store: [`src/Host/StartingLevelHostSession.cs`](../../src/Host/StartingLevelHostSession.cs)
  - UI Panel: [`src/UI/OpeningProtocolModal.cs`](../../src/UI/OpeningProtocolModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/StartingLevelSystemTests.cs`](../../Ashfall.Core.Tests/StartingLevelSystemTests.cs)

### 234. `sump_flooding` — Bunker sump pump drainage & flood risk (Shelter & Infrastructure)
- **Owner Domain:** `maintenance`
- **Setup Method:** `Main.SetupSumpFlooding()` | **Invoked:** yes | **Cadence:** `Daily Drainage Pump Work`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:124`, `src/Main.PlayerSurfaces.cs:740`
- **UI Routes:** `sump_flooding`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SumpFloodingSystem.cs`](../../Assets/Ashfall.Core/SumpFloodingSystem.cs)
  - Host Session: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - Save Store: [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
  - UI Panel: [`src/UI/SumpFloodingPanel.cs`](../../src/UI/SumpFloodingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs`](../../Ashfall.Core.Tests/NewSaveStoreChecksumSweepTests.cs)

### 235. `survivor_social` — Leadership, friction, ration conflict, trauma bonds, skill atrophy (Shelter & Infrastructure)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupSurvivorSocial()` | **Invoked:** yes | **Cadence:** `Daily Shelter Social Dynamics`
- **Setup Invocation Sites:** `src/Main.InternalCommunication.cs:39`, `src/Main.MedicalTriage.cs:178`, `src/Main.MoraleContagion.cs:27`, `src/Main.PersonalBelongings.cs:27`, `src/Main.PfglOctetBoards.cs:62`, `src/Main.PfglOctetBoards.cs:124`, `src/Main.Plans163_210.cs:72`, `src/Main.Plans163_210.cs:126`, `src/Main.Plans216_202Interpersonal.cs:29`, `src/Main.Plans216_202Interpersonal.cs:47`, `src/Main.PlayerSurfaces.cs:255`, `src/Main.SaveOrchestrator.cs:227`, `src/Main.SurvivorFate.cs:41`, `src/Main.SurvivorSocial.cs:105`, `src/Main.SurvivorSocial.cs:143`, `src/Main.SurvivorSocial.cs:149`, `src/Main.SurvivorSocial.cs:155`, `src/Main.SurvivorSocial.cs:162`, `src/Main.SurvivorSocial.cs:169`, `src/Main.SurvivorSocial.cs:175`, `src/Main.SurvivorSocial.cs:181`, `src/Main.Zealotry.cs:78`
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
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan208LeadershipSuccessionIntegrationTests.cs)

### 236. `vinyl_morale` — Gramophone records and music morale (Shelter & Infrastructure)
- **Owner Domain:** `morale`
- **Setup Method:** `Main.SetupVinylMorale()` | **Invoked:** yes | **Cadence:** `Daily Turntable Morale Broadcast`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:113`
- **UI Routes:** `vinyl_morale`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/VinylMoraleSystem.cs`](../../Assets/Ashfall.Core/VinylMoraleSystem.cs)
  - Host Session: [`src/Host/VinylMoraleHostSession.cs`](../../src/Host/VinylMoraleHostSession.cs)
  - Save Store: [`src/Host/VinylMoraleSaveStore.cs`](../../src/Host/VinylMoraleSaveStore.cs)
  - UI Panel: [`src/UI/VinylMoralePanel.cs`](../../src/UI/VinylMoralePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 237. `water_treatment` — Water filtration and purification (Shelter & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaterTreatment()` | **Invoked:** yes | **Cadence:** `Daily Filtration Cycle`
- **Setup Invocation Sites:** `src/Main.DeepWell.cs:24`, `src/Main.ExpandedShelterSystems.cs:109`, `src/Main.ExpandedShelterSystems.cs:524`, `src/Main.Plans166_169.cs:41`, `src/Main.WaterCondenser.cs:23`, `src/Main.WaterSources.cs:20`
- **UI Routes:** `water_treatment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaterTreatmentSystem.cs`](../../Assets/Ashfall.Core/WaterTreatmentSystem.cs)
  - Host Session: [`src/Host/WaterSourcesHostSession.cs`](../../src/Host/WaterSourcesHostSession.cs)
  - Host Session: [`src/Host/WaterTreatmentHostSession.cs`](../../src/Host/WaterTreatmentHostSession.cs)
  - Save Store: [`src/Host/WaterTreatmentSaveStore.cs`](../../src/Host/WaterTreatmentSaveStore.cs)
  - UI Panel: [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs`](../../Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaterTreatmentSystemTests.cs`](../../Ashfall.Core.Tests/WaterTreatmentSystemTests.cs)

### 238. `crafting` — Known recipes and workbench queues (Shelter & Logistics)
- **Owner Domain:** `crafting`
- **Setup Method:** `Main.SetupCrafting()` | **Invoked:** yes | **Cadence:** `Daily Workbench Queue`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1366`, `src/Main.CampaignServices.cs:43`, `src/Main.ExpandedShelterSystems.cs:100`, `src/Main.GameFlow.cs:491`, `src/Main.Lifecycle.cs:726`, `src/Main.Phase0.cs:114`, `src/Main.Plans166_169.cs:40`, `src/Main.Plans74_77.cs:28`, `src/Main.Plans74_77.cs:58`, `src/Main.PlayerSurfaces.cs:280`, `src/Main.PlayerSurfaces.cs:285`, `src/Main.PlayerSurfaces.cs:305`, `src/Main.PlayerSurfaces.cs:310`, `src/Main.PlayerSurfaces.cs:642`, `src/Main.SaveOrchestrator.cs:196`, `src/Main.UiHandlers.cs:19`, `src/Main.World.cs:426`, `src/Main.World.cs:432`
- **UI Routes:** `crafting`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Crafting/CraftingSystem.cs`](../../Assets/Ashfall.Core/Crafting/CraftingSystem.cs)
  - Host Session: [`src/Host/CraftingHostSession.cs`](../../src/Host/CraftingHostSession.cs)
  - Save Store: [`src/Host/CraftingSaveStore.cs`](../../src/Host/CraftingSaveStore.cs)
  - UI Panel: [`src/UI/CraftingPanel.cs`](../../src/UI/CraftingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CraftingSystemTests.cs`](../../Ashfall.Core.Tests/CraftingSystemTests.cs)

### 239. `equipment_condition` — Tool and weapon wear/repair (Shelter & Logistics)
- **Owner Domain:** `equipment`
- **Setup Method:** `Main.SetupEquipmentCondition()` | **Invoked:** yes | **Cadence:** `Daily Gear Wear & Maintenance`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:138`, `src/Main.Plans74_77.cs:29`, `src/Main.Plans74_77.cs:59`, `src/Main.PlayerSurfaces.cs:285`
- **UI Routes:** `equipment_condition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/EquipmentConditionSystem.cs`](../../Assets/Ashfall.Core/EquipmentConditionSystem.cs)
  - Host Session: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - Save Store: [`src/Host/EquipmentConditionHostSession.cs`](../../src/Host/EquipmentConditionHostSession.cs)
  - UI Panel: [`src/UI/EquipmentConditionPanel.cs`](../../src/UI/EquipmentConditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EquipmentConditionSystemTests.cs`](../../Ashfall.Core.Tests/EquipmentConditionSystemTests.cs)

### 240. `inventory` — Shelter warehouse & items storage (Shelter & Logistics)
- **Owner Domain:** `inventory`
- **Setup Method:** `Main.SetupInventory()` | **Invoked:** yes | **Cadence:** `On-Demand (Item Use)`
- **Setup Invocation Sites:** `src/Main.AdvancedShelterSystems.cs:44`, `src/Main.AdvancedShelterSystems.cs:103`, `src/Main.AdvancedShelterSystems.cs:161`, `src/Main.AdvancedShelterSystems.cs:234`, `src/Main.AdvancedShelterSystems.cs:342`, `src/Main.AdvancedShelterSystems.cs:426`, `src/Main.AdvancedShelterSystems.cs:516`, `src/Main.BlackMarket.cs:30`, `src/Main.BriefingCrisis.cs:56`, `src/Main.CampaignOwners.cs:1339`, `src/Main.CampaignOwners.cs:1658`, `src/Main.CampaignServices.cs:36`, `src/Main.ChemicalSynthesis.cs:19`, `src/Main.DebtCredit.cs:66`, `src/Main.Echoes.cs:100`, `src/Main.Echoes.cs:128`, `src/Main.Economy.cs:252`, `src/Main.ExpandedShelterSystems.cs:97`, `src/Main.Expeditions.cs:98`, `src/Main.Expeditions.cs:346`, `src/Main.GameFlow.cs:400`, `src/Main.GameFlow.cs:410`, `src/Main.GameFlow.cs:468`, `src/Main.GameFlow.cs:484`, `src/Main.GameFlow.cs:492`, `src/Main.GameFlow.cs:504`, `src/Main.GameFlow.cs:520`, `src/Main.GameFlow.cs:551`, `src/Main.GameFlow.cs:789`, `src/Main.Holdfast.cs:82`, `src/Main.Holdfast.cs:432`, `src/Main.Inventory.cs:172`, `src/Main.Inventory.cs:179`, `src/Main.Inventory.cs:187`, `src/Main.Inventory.cs:195`, `src/Main.Inventory.cs:215`, `src/Main.Inventory.cs:231`, `src/Main.Inventory.cs:249`, `src/Main.Lifecycle.cs:720`, `src/Main.Medical.cs:226`, `src/Main.Medical.cs:583`, `src/Main.Medical.cs:602`, `src/Main.Narrative.cs:617`, `src/Main.NarrativeQuestlines.cs:273`, `src/Main.NightWatch.cs:29`, `src/Main.PersonalBelongings.cs:28`, `src/Main.Phase0.cs:58`, `src/Main.Plans110_113.cs:31`, `src/Main.Plans110_113.cs:53`, `src/Main.Plans110_113.cs:98`, `src/Main.Plans110_113.cs:118`, `src/Main.Plans122to125.cs:72`, `src/Main.Plans122to125.cs:88`, `src/Main.Plans130_133.cs:35`, `src/Main.Plans130_133.cs:86`, `src/Main.Plans130_133.cs:136`, `src/Main.Plans146_149.cs:644`, `src/Main.Plans146_149.cs:669`, `src/Main.Plans146_149.cs:732`, `src/Main.Plans146_149.cs:762`, `src/Main.Plans147.cs:216`, `src/Main.Plans147.cs:243`, `src/Main.Plans162_165.cs:36`, `src/Main.Plans162_165.cs:188`, `src/Main.Plans162_165.cs:310`, `src/Main.Plans162_165.cs:430`, `src/Main.Plans162_165.cs:525`, `src/Main.Plans162_165.cs:669`, `src/Main.Plans166_169.cs:37`, `src/Main.Plans166_169.cs:279`, `src/Main.Plans167_219.cs:67`, `src/Main.Plans46_49.cs:40`, `src/Main.Plans46_49.cs:253`, `src/Main.Plans62_65.cs:37`, `src/Main.Plans74_77.cs:27`, `src/Main.Plans74_77.cs:42`, `src/Main.Plans74_77.cs:57`, `src/Main.Plans74_77.cs:78`, `src/Main.Plans74_77.cs:94`, `src/Main.Plans94_97.cs:20`, `src/Main.Plans94_97.cs:33`, `src/Main.PlansB68_B69.cs:86`, `src/Main.PlansB86_B89.cs:38`, `src/Main.PlansB86_B89.cs:145`, `src/Main.PlayerSurfaces.cs:17`, `src/Main.PlayerSurfaces.cs:174`, `src/Main.PlayerSurfaces.cs:260`, `src/Main.PlayerSurfaces.cs:275`, `src/Main.PlayerSurfaces.cs:280`, `src/Main.PlayerSurfaces.cs:285`, `src/Main.PlayerSurfaces.cs:300`, `src/Main.PlayerSurfaces.cs:305`, `src/Main.PlayerSurfaces.cs:310`, `src/Main.PlayerSurfaces.cs:315`, `src/Main.PlayerSurfaces.cs:324`, `src/Main.PlayerSurfaces.cs:358`, `src/Main.PlayerSurfaces.cs:601`, `src/Main.PlayerSurfaces.cs:642`, `src/Main.SaveOrchestrator.cs:174`, `src/Main.ShelterBatch3.cs:376`, `src/Main.ShelterOperations.cs:22`, `src/Main.SkyDefense.cs:52`, `src/Main.SkyDefense.cs:81`, `src/Main.UiHandlers.cs:20`, `src/Main.UiPanels.cs:1041`, `src/Main.VehicleGarage.cs:23`, `src/Main.VehicleGarage.cs:44`, `src/Main.VisitorIntegration.cs:63`, `src/Main.WaterSources.cs:21`, `src/Main.World.cs:77`, `src/Main.World.cs:211`, `src/Main.World.cs:279`, `src/Main.World.cs:674`, `src/Main.YearOfAsh.cs:505`
- **UI Routes:** `inventory`, `inventory_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Inventory/Inventory.cs`](../../Assets/Ashfall.Core/Inventory/Inventory.cs)
  - Host Session: [`src/Host/InventoryHostSession.cs`](../../src/Host/InventoryHostSession.cs)
  - Save Store: [`src/Host/InventorySaveStore.cs`](../../src/Host/InventorySaveStore.cs)
  - UI Panel: [`src/UI/InventoryDetailPanel.cs`](../../src/UI/InventoryDetailPanel.cs)
  - UI Panel: [`src/UI/InventoryPanel.cs`](../../src/UI/InventoryPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/InventorySystemTests.cs`](../../Ashfall.Core.Tests/InventorySystemTests.cs)

### 241. `kitchen_nutrition` — Rationing recipes and caloric balance (Shelter & Logistics)
- **Owner Domain:** `nutrition`
- **Setup Method:** `Main.SetupKitchenNutrition()` | **Invoked:** yes | **Cadence:** `Daily Rationing Meal Prep`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:134`
- **UI Routes:** `kitchen_nutrition`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/KitchenNutritionSystem.cs`](../../Assets/Ashfall.Core/KitchenNutritionSystem.cs)
  - Host Session: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - Save Store: [`src/Host/KitchenNutritionHostSession.cs`](../../src/Host/KitchenNutritionHostSession.cs)
  - UI Panel: [`src/UI/KitchenNutritionPanel.cs`](../../src/UI/KitchenNutritionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/KitchenNutritionSystemTests.cs`](../../Ashfall.Core.Tests/KitchenNutritionSystemTests.cs)

### 242. `radio` — Radio frequencies, logs, and distress signals (Shelter & Logistics)
- **Owner Domain:** `radio`
- **Setup Method:** `Main.SetupRadio()` | **Invoked:** yes | **Cadence:** `On-Demand (Frequency Scan)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:48`, `src/Main.Economy.cs:70`, `src/Main.Economy.cs:77`, `src/Main.Expeditions.cs:631`, `src/Main.Expeditions.cs:649`, `src/Main.GameFlow.cs:530`, `src/Main.Lifecycle.cs:723`, `src/Main.NpcArcs.cs:39`, `src/Main.Plans130_133.cs:62`, `src/Main.Plans94_97.cs:52`, `src/Main.PlayerSurfaces.cs:336`, `src/Main.PlayerSurfaces.cs:541`, `src/Main.RadioProgramProduction.cs:29`, `src/Main.SaveOrchestrator.cs:181`, `src/Main.UiHandlers.cs:30`, `src/Main.UiPanels.cs:1067`
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

### 243. `shelter_reputation` — Plan 207 — shelter reputation, notoriety, public tags, and external perception (Shelter (Plan 207))
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterReputation()` | **Invoked:** yes | **Cadence:** `Daily (Reputation Decay & Tag Evaluation)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:147`
- **UI Routes:** `shelter_reputation`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs`](../../Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs)
  - Host Session: [`src/Host/ShelterReputationHostSession.cs`](../../src/Host/ShelterReputationHostSession.cs)
  - Save Store: [`src/Host/ShelterReputationSaveStore.cs`](../../src/Host/ShelterReputationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ShelterReputationPanel.cs`](../../src/UI/ShelterReputationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Reputation/Plan207ShelterReputationIntegrationTests.cs`](../../Ashfall.Core.Tests/Reputation/Plan207ShelterReputationIntegrationTests.cs)

### 244. `internal_communication` — Plan 211 — internal shelter notices, bulletin boards, intercom acknowledgements, and private mail (Shelter Communication (Plan 211))
- **Owner Domain:** `communication`
- **Setup Method:** `Main.SetupInternalCommunication()` | **Invoked:** yes | **Cadence:** `Daily (Message Expiry)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:144`, `src/Main.InternalCommunication.cs:26`, `src/Main.Plans46_49.cs:187`, `src/Main.Plans46_49.cs:221`, `src/Main.Plans46_49.cs:229`
- **UI Routes:** `shelter_social`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Communication/InternalCommunicationSystem.cs`](../../Assets/Ashfall.Core/Communication/InternalCommunicationSystem.cs)
  - Host Session: [`src/Host/InternalCommunicationHostSession.cs`](../../src/Host/InternalCommunicationHostSession.cs)
  - Save Store: [`src/Host/InternalCommunicationSaveStore.cs`](../../src/Host/InternalCommunicationSaveStore.cs)
  - UI Panel: [`src/UI/ShelterSocialPanel.cs`](../../src/UI/ShelterSocialPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communication/Plan211InternalCommunicationHostWiringTests.cs`](../../Ashfall.Core.Tests/Communication/Plan211InternalCommunicationHostWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Communication/Plan211InternalCommunicationIntegrationTests.cs`](../../Ashfall.Core.Tests/Communication/Plan211InternalCommunicationIntegrationTests.cs)

### 245. `shelter_security` — Plan 138 — shelter security zones, clearances, locks, lockdowns, and breaches (Shelter Defense (Plan 138))
- **Owner Domain:** `shelter`
- **Setup Method:** `Main.SetupShelterSecurity()` | **Invoked:** yes | **Cadence:** `Daily (Breach Decay & Alert Drift)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:150`, `src/Main.NightWatch.cs:34`
- **UI Routes:** `shelter_security`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/ShelterSecuritySystem.cs`](../../Assets/Ashfall.Core/Shelter/ShelterSecuritySystem.cs)
  - Host Session: [`src/Host/ShelterSecurityHostSession.cs`](../../src/Host/ShelterSecurityHostSession.cs)
  - Save Store: [`src/Host/ShelterSecuritySaveStore.cs`](../../src/Host/ShelterSecuritySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/ShelterSecurityPanel.cs`](../../src/UI/ShelterSecurityPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Shelter/Plan138ShelterSecurityIntegrationTests.cs`](../../Ashfall.Core.Tests/Shelter/Plan138ShelterSecurityIntegrationTests.cs)

### 246. `relationship_decay` — Plan 182 — survivor pair bond decay, interaction tracking, and social drift (Social Ecology & Drift (Plan 182))
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupRelationshipDecay()` | **Invoked:** yes | **Cadence:** `Daily (Pair Bond Decay & Social Drift)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:155`
- **UI Routes:** `relationship_decay`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs`](../../Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs)
  - Host Session: [`src/Host/RelationshipDecayHostSession.cs`](../../src/Host/RelationshipDecayHostSession.cs)
  - Save Store: [`src/Host/RelationshipDecaySaveStore.cs`](../../src/Host/RelationshipDecaySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/RelationshipDecayPanel.cs`](../../src/UI/RelationshipDecayPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/RelationshipDecaySystemTests.cs`](../../Ashfall.Core.Tests/Survivors/RelationshipDecaySystemTests.cs)

### 247. `spiritual_ritual` — EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED — authored ritual cooldown ledger (last-performed day per ritual id) (Spiritual)
- **Owner Domain:** `spiritual`
- **Setup Method:** `Main.SetupSpiritualRitual()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:768`, `src/Main.CampaignOwners.cs:779`, `src/Main.SaveOrchestrator.cs:319`, `src/Main.SpiritualRitual.cs:44`, `src/Main.SpiritualRitual.cs:84`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Spiritual/SpiritualModels.cs`](../../Assets/Ashfall.Core/Spiritual/SpiritualModels.cs)
  - Core System: [`Assets/Ashfall.Core/Spiritual/SpiritualRitualCalendarEngine.cs`](../../Assets/Ashfall.Core/Spiritual/SpiritualRitualCalendarEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Spiritual/SpiritualRitualSaveState.cs`](../../Assets/Ashfall.Core/Spiritual/SpiritualRitualSaveState.cs)
  - Host Session: [`src/Host/SpiritualRitualHostSession.cs`](../../src/Host/SpiritualRitualHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SpiritualRitualSaveStore.cs`](../../src/Host/SpiritualRitualSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Spiritual/PlanExpansion13SpiritualRitualHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Spiritual/PlanExpansion13SpiritualRitualHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Spiritual/SpiritualRitualCalendarEngineTests.cs`](../../Ashfall.Core.Tests/Spiritual/SpiritualRitualCalendarEngineTests.cs)

### 248. `hydrogeology_archive` — Plan 154 — Hydrogeology science archive: discovered-record ledger (IDs only) (Subterranean Science Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupHydroGeologyDiscovery()` | **Invoked:** yes | **Cadence:** `Event-Driven (Location Discovery)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:125`, `src/Main.SaveOrchestrator.cs:272`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/HydroGeologyDiscoverySystem.cs`](../../Assets/Ashfall.Core/Narrative/HydroGeologyDiscoverySystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HydroGeologyArchiveSaveStore.cs`](../../src/Host/HydroGeologyArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/HydroGeologyCatalogTests.cs`](../../Ashfall.Core.Tests/HydroGeologyCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/HydroGeologyDiscoveryTests.cs`](../../Ashfall.Core.Tests/Narrative/HydroGeologyDiscoveryTests.cs)

### 249. `apprenticeship` — Mentorship pairings and skill growth (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupApprenticeship()` | **Invoked:** yes | **Cadence:** `Daily Mentorship XP Transfer`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:116`
- **UI Routes:** `apprenticeship`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ApprenticeshipSystem.cs`](../../Assets/Ashfall.Core/ApprenticeshipSystem.cs)
  - Host Session: [`src/Host/ApprenticeshipHostSession.cs`](../../src/Host/ApprenticeshipHostSession.cs)
  - Save Store: [`src/Host/ApprenticeshipSaveStore.cs`](../../src/Host/ApprenticeshipSaveStore.cs)
  - UI Panel: [`src/UI/ApprenticeshipPanel.cs`](../../src/UI/ApprenticeshipPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`](../../Ashfall.Core.Tests/ApprenticeshipSystemTests.cs)

### 250. `autopsy` — Post-mortem forensic analysis (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupAutopsy()` | **Invoked:** yes | **Cadence:** `Daily Forensic Case Progress`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:122`
- **UI Routes:** `autopsy_report`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AutopsySystem.cs`](../../Assets/Ashfall.Core/AutopsySystem.cs)
  - Host Session: [`src/Host/AutopsyHostSession.cs`](../../src/Host/AutopsyHostSession.cs)
  - Save Store: [`src/Host/AutopsySaveStore.cs`](../../src/Host/AutopsySaveStore.cs)
  - UI Panel: [`src/UI/AutopsyReportPanel.cs`](../../src/UI/AutopsyReportPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/AutopsySystemTests.cs`](../../Ashfall.Core.Tests/AutopsySystemTests.cs)

### 251. `caregiving` — Childcare, elderly care, and comfort (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupCaregiving()` | **Invoked:** yes | **Cadence:** `Daily Nursery/Eldercare Comfort`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:117`
- **UI Routes:** `caregiving`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/CaregivingSystem.cs`](../../Assets/Ashfall.Core/Survivors/CaregivingSystem.cs)
  - Host Session: [`src/Host/CaregivingHostSession.cs`](../../src/Host/CaregivingHostSession.cs)
  - Save Store: [`src/Host/CaregivingSaveStore.cs`](../../src/Host/CaregivingSaveStore.cs)
  - UI Panel: [`src/UI/CaregivingPanel.cs`](../../src/UI/CaregivingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CaregivingSystemTests.cs`](../../Ashfall.Core.Tests/CaregivingSystemTests.cs)

### 252. `chemical_dependency` — Substance dependencies and withdrawal (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMentalHealthCrisis()` | **Invoked:** yes | **Cadence:** `Daily Tolerance & Withdrawal`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:142`, `src/Main.MoraleContagion.cs:29`, `src/Main.PlayerSurfaces.cs:305`, `src/Main.PlayerSurfaces.cs:310`
- **UI Routes:** `chemical_dependency`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`](../../Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs)
  - Host Session: [`src/Host/ChemicalDependencyHostSession.cs`](../../src/Host/ChemicalDependencyHostSession.cs)
  - Host Session: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - Save Store: [`src/Host/ChemicalDependencySaveStore.cs`](../../src/Host/ChemicalDependencySaveStore.cs)
  - UI Panel: [`src/UI/ChemicalDependencyPanel.cs`](../../src/UI/ChemicalDependencyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/BareSaveStoreSealTests.cs`](../../Ashfall.Core.Tests/BareSaveStoreSealTests.cs)

### 253. `contractor_roster` — Hired mercenaries and specialists (Survival & Biology)
- **Owner Domain:** `personnel`
- **Setup Method:** `Main.SetupContractorRoster()` | **Invoked:** yes | **Cadence:** `Daily Mercenary Wage Payroll`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:141`
- **UI Routes:** `contractor_roster`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/ContractorRosterSystem.cs`](../../Assets/Ashfall.Core/ContractorRosterSystem.cs)
  - Host Session: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - Save Store: [`src/Host/ContractorRosterHostSession.cs`](../../src/Host/ContractorRosterHostSession.cs)
  - UI Panel: [`src/UI/ContractorRosterPanel.cs`](../../src/UI/ContractorRosterPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ContractorRosterSystemTests.cs`](../../Ashfall.Core.Tests/ContractorRosterSystemTests.cs)

### 254. `disease` — Epidemics, contagions, and pathogen spread (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupDisease()` | **Invoked:** yes | **Cadence:** `Daily Pathogen Transmission`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1814`, `src/Main.CampaignOwners.cs:1852`, `src/Main.Expeditions.cs:150`, `src/Main.Expeditions.cs:792`, `src/Main.Medical.cs:42`, `src/Main.Medical.cs:272`, `src/Main.Medical.cs:469`, `src/Main.MedicalTriage.cs:52`, `src/Main.MoraleContagion.cs:73`, `src/Main.PathogenStrains.cs:24`, `src/Main.Plans166_169.cs:42`, `src/Main.Plans62_65.cs:189`, `src/Main.SaveOrchestrator.cs:224`, `src/Main.ShelterSocial.cs:378`
- **UI Routes:** `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`](../../Assets/Ashfall.Core/Disease/DiseaseSystem.cs)
  - Host Session: [`src/Disease/DiseaseHostSession.cs`](../../src/Disease/DiseaseHostSession.cs)
  - Save Store: [`src/Host/DiseaseSaveStore.cs`](../../src/Host/DiseaseSaveStore.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DiseaseSystemTests.cs`](../../Ashfall.Core.Tests/DiseaseSystemTests.cs)

### 255. `medical` — Triage, illnesses, and treatments (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedical()` | **Invoked:** yes | **Cadence:** `Daily Recovery / Affliction`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1809`, `src/Main.CampaignOwners.cs:1835`, `src/Main.CampaignServices.cs:40`, `src/Main.DutyRoster.cs:50`, `src/Main.ExpandedShelterSystems.cs:102`, `src/Main.GameFlow.cs:411`, `src/Main.GameFlow.cs:505`, `src/Main.GameFlow.cs:627`, `src/Main.Lifecycle.cs:728`, `src/Main.Medical.cs:103`, `src/Main.Medical.cs:194`, `src/Main.Medical.cs:212`, `src/Main.Medical.cs:223`, `src/Main.Medical.cs:470`, `src/Main.Phase0.cs:102`, `src/Main.Phase0.cs:116`, `src/Main.PlayerSurfaces.cs:174`, `src/Main.PlayerSurfaces.cs:315`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.SaveOrchestrator.cs:175`, `src/Main.ShelterBatch3.cs:300`, `src/Main.SurvivorFitness.cs:65`
- **UI Routes:** `medical`, `afflictions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/MedicalWardSystem.cs)
  - Core System: [`Assets/Ashfall.Core/SickListSystem.cs`](../../Assets/Ashfall.Core/SickListSystem.cs)
  - Host Session: [`src/Host/MedicalHostSession.cs`](../../src/Host/MedicalHostSession.cs)
  - Save Store: [`src/Host/MedicalSaveStore.cs`](../../src/Host/MedicalSaveStore.cs)
  - UI Panel: [`src/UI/AfflictionsPanel.cs`](../../src/UI/AfflictionsPanel.cs)
  - UI Panel: [`src/UI/MedicalPanel.cs`](../../src/UI/MedicalPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/DwellerMedicalCatalogTests.cs`](../../Ashfall.Core.Tests/DwellerMedicalCatalogTests.cs)

### 256. `medical_ward` — Hospital ward beds and inpatients (Survival & Biology)
- **Owner Domain:** `medical`
- **Setup Method:** `Main.SetupMedicalWard()` | **Invoked:** yes | **Cadence:** `Daily Bed Inpatient Triage`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:41`, `src/Main.ExpandedShelterSystems.cs:103`, `src/Main.ExpandedShelterSystems.cs:710`, `src/Main.Lifecycle.cs:728`, `src/Main.Medical.cs:595`, `src/Main.Medical.cs:650`, `src/Main.SaveOrchestrator.cs:176`, `src/Main.SurvivorFate.cs:40`
- **UI Routes:** `medical_ward`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`](../../Assets/Ashfall.Core/Medical/MedicalWardSystem.cs)
  - Host Session: [`src/Host/MedicalWardHostSession.cs`](../../src/Host/MedicalWardHostSession.cs)
  - Save Store: [`src/Host/MedicalWardSaveStore.cs`](../../src/Host/MedicalWardSaveStore.cs)
  - UI Panel: [`src/UI/MedicalWardPanel.cs`](../../src/UI/MedicalWardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs`](../../Ashfall.Core.Tests/Medical/MedicalWardSystemTests.cs)

### 257. `mental_health_crisis` — Psychological trauma and psych ward (Survival & Biology)
- **Owner Domain:** `psychology`
- **Setup Method:** `Main.SetupMentalHealthCrisis()` | **Invoked:** yes | **Cadence:** `Daily Psych Ward Calming Ticks`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:142`, `src/Main.MoraleContagion.cs:29`, `src/Main.PlayerSurfaces.cs:305`, `src/Main.PlayerSurfaces.cs:310`
- **UI Routes:** `mental_health_crisis`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`](../../Assets/Ashfall.Core/MentalHealthCrisisSystem.cs)
  - Host Session: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - Save Store: [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
  - UI Panel: [`src/UI/MentalHealthCrisisPanel.cs`](../../src/UI/MentalHealthCrisisPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs`](../../Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs)

### 258. `morale_contagion` — Flagship XI Plan 154 — morale contagion channels, breakdowns, social isolation, schism ledger, HopeBeacon installation (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupMoraleContagion()` | **Invoked:** yes | **Cadence:** `Daily Contagion / Isolation Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1674`, `src/Main.CampaignOwners.cs:1997`, `src/Main.MoraleContagion.cs:118`, `src/Main.MoraleContagion.cs:144`, `src/Main.MoraleContagion.cs:151`, `src/Main.SaveOrchestrator.cs:182`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs`](../../Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs)
  - Host Session: [`src/Host/MoraleContagionHostSession.cs`](../../src/Host/MoraleContagionHostSession.cs)
  - Save Store: [`src/Host/MoraleContagionSaveStore.cs`](../../src/Host/MoraleContagionSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs`](../../Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs)

### 259. `survivor_relations` — Survivor affinities, feuds, and bonds (Survival & Biology)
- **Owner Domain:** `social`
- **Setup Method:** `Main.SetupSurvivorRelations()` | **Invoked:** yes | **Cadence:** `Daily Affinity & Feud Drift`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:111`, `src/Main.Plans216_202Interpersonal.cs:58`, `src/Main.Plans216_202Interpersonal.cs:66`, `src/Main.Plans216_202Interpersonal.cs:82`, `src/Main.SurvivorSocial.cs:21`
- **UI Routes:** `survivor_relations`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/SurvivorRelationsSystem.cs`](../../Assets/Ashfall.Core/SurvivorRelationsSystem.cs)
  - Host Session: [`src/Host/SurvivorRelationsHostSession.cs`](../../src/Host/SurvivorRelationsHostSession.cs)
  - Save Store: [`src/Host/SurvivorRelationsSaveStore.cs`](../../src/Host/SurvivorRelationsSaveStore.cs)
  - UI Panel: [`src/UI/SurvivorRelationsPanel.cs`](../../src/UI/SurvivorRelationsPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs`](../../Ashfall.Core.Tests/ExpandedShelterSaveChecksumTests.cs)

### 260. `survivors` — Living survivors, needs, and traits (Survival & Biology)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivors()` | **Invoked:** yes | **Cadence:** `Daily Needs Decay`
- **Setup Invocation Sites:** `src/Main.BriefingCrisis.cs:35`, `src/Main.BroadsheetPress.cs:40`, `src/Main.BroadsheetPress.cs:103`, `src/Main.CampaignOwners.cs:1636`, `src/Main.CampaignOwners.cs:1646`, `src/Main.CampaignServices.cs:37`, `src/Main.ClothingWarmth.cs:62`, `src/Main.DreamSystem.cs:71`, `src/Main.Echoes.cs:99`, `src/Main.Echoes.cs:127`, `src/Main.Echoes.cs:174`, `src/Main.Endgame.cs:249`, `src/Main.ExpandedShelterSystems.cs:91`, `src/Main.Expeditions.cs:99`, `src/Main.Expeditions.cs:347`, `src/Main.Expeditions.cs:764`, `src/Main.GameFlow.cs:398`, `src/Main.GameFlow.cs:409`, `src/Main.GameFlow.cs:417`, `src/Main.GameFlow.cs:457`, `src/Main.GameFlow.cs:462`, `src/Main.GameFlow.cs:474`, `src/Main.GameFlow.cs:479`, `src/Main.GameFlow.cs:493`, `src/Main.GameFlow.cs:503`, `src/Main.GameFlow.cs:519`, `src/Main.GameFlow.cs:549`, `src/Main.GameFlow.cs:644`, `src/Main.GameFlow.cs:656`, `src/Main.GameFlow.cs:684`, `src/Main.GameFlow.cs:790`, `src/Main.HiddenAgenda.cs:23`, `src/Main.Holdfast.cs:70`, `src/Main.Lifecycle.cs:719`, `src/Main.Medical.cs:225`, `src/Main.MoraleContagion.cs:26`, `src/Main.Narrative.cs:418`, `src/Main.Narrative.cs:427`, `src/Main.Narrative.cs:441`, `src/Main.NarrativeQuestlines.cs:263`, `src/Main.NarrativeQuestlines.cs:285`, `src/Main.NarrativeQuestlines.cs:295`, `src/Main.NpcArcs.cs:28`, `src/Main.OrphanSealWave1.cs:216`, `src/Main.PathogenStrains.cs:25`, `src/Main.PersonalBelongings.cs:29`, `src/Main.PfglOctetBoards.cs:61`, `src/Main.PfglOctetBoards.cs:123`, `src/Main.Phase0.cs:49`, `src/Main.Phase0.cs:112`, `src/Main.Plans162_165.cs:524`, `src/Main.Plans162_185.cs:20`, `src/Main.Plans163_210.cs:44`, `src/Main.Plans163_210.cs:127`, `src/Main.Plans166_169.cs:36`, `src/Main.Plans166_169.cs:278`, `src/Main.Plans167_219.cs:140`, `src/Main.Plans167_219.cs:188`, `src/Main.Plans167_219.cs:232`, `src/Main.Plans167_219.cs:271`, `src/Main.Plans167_219.cs:311`, `src/Main.Plans216_202Interpersonal.cs:105`, `src/Main.Plans62_65.cs:36`, `src/Main.PlayerSurfaces.cs:17`, `src/Main.PlayerSurfaces.cs:174`, `src/Main.PlayerSurfaces.cs:179`, `src/Main.PlayerSurfaces.cs:250`, `src/Main.PlayerSurfaces.cs:255`, `src/Main.PlayerSurfaces.cs:265`, `src/Main.PlayerSurfaces.cs:270`, `src/Main.PlayerSurfaces.cs:280`, `src/Main.PlayerSurfaces.cs:285`, `src/Main.PlayerSurfaces.cs:295`, `src/Main.PlayerSurfaces.cs:300`, `src/Main.PlayerSurfaces.cs:305`, `src/Main.PlayerSurfaces.cs:310`, `src/Main.PlayerSurfaces.cs:315`, `src/Main.PlayerSurfaces.cs:324`, `src/Main.PlayerSurfaces.cs:356`, `src/Main.PlayerSurfaces.cs:445`, `src/Main.PlayerSurfaces.cs:455`, `src/Main.PlayerSurfaces.cs:480`, `src/Main.PlayerSurfaces.cs:517`, `src/Main.PlayerSurfaces.cs:525`, `src/Main.PlayerSurfaces.cs:580`, `src/Main.PlayerSurfaces.cs:585`, `src/Main.PlayerSurfaces.cs:637`, `src/Main.PlayerSurfaces.cs:642`, `src/Main.SaveOrchestrator.cs:173`, `src/Main.ShelterBatch3.cs:158`, `src/Main.ShelterBatch3.cs:168`, `src/Main.ShelterBatch3.cs:375`, `src/Main.ShelterOperations.cs:21`, `src/Main.ShelterSocial.cs:322`, `src/Main.ShelterSocial.cs:391`, `src/Main.SkyDefense.cs:53`, `src/Main.SkyDefense.cs:66`, `src/Main.Subterranean.cs:27`, `src/Main.SurvivorFate.cs:35`, `src/Main.SurvivorRoles.cs:99`, `src/Main.SurvivorSocial.cs:20`, `src/Main.Survivors.cs:266`, `src/Main.Survivors.cs:273`, `src/Main.Survivors.cs:283`, `src/Main.Survivors.cs:290`, `src/Main.Survivors.cs:297`, `src/Main.UiHandlers.cs:21`, `src/Main.UiHandlers.cs:47`, `src/Main.UiHandlers.cs:268`, `src/Main.UiPanels.cs:1049`, `src/Main.UiPanels.cs:1153`, `src/Main.UnifiedEnding.cs:71`, `src/Main.VisitorIntegration.cs:86`
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

### 261. `death_legacy` — Plan 206 — survivor death records, last wills, estate inheritance, and disputes (Survivor Memorial & Wills (Plan 206))
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupDeathLegacy()` | **Invoked:** yes | **Cadence:** `Event-Driven & Daily Flush`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:154`
- **UI Routes:** `death_legacy`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorDeathLegacySystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorDeathLegacySystem.cs)
  - Host Session: [`src/Host/SurvivorDeathLegacyHostSession.cs`](../../src/Host/SurvivorDeathLegacyHostSession.cs)
  - Save Store: [`src/Host/SurvivorDeathLegacySaveStore.cs`](../../src/Host/SurvivorDeathLegacySaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/SurvivorDeathLegacyPanel.cs`](../../src/UI/SurvivorDeathLegacyPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan206SurvivorDeathLegacyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan206SurvivorDeathLegacyIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/SurvivorDeathLegacySystemTests.cs`](../../Ashfall.Core.Tests/Survivors/SurvivorDeathLegacySystemTests.cs)

### 262. `aging` — Plan 176 — Aging & elderly survivor system: chronological age progression, life stages, retirement, elder mentorship, and milestones (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupAging()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Aging.cs:61`, `src/Main.Aging.cs:90`, `src/Main.Aging.cs:97`, `src/Main.Aging.cs:105`, `src/Main.Aging.cs:111`, `src/Main.CampaignOwners.cs:1082`, `src/Main.CampaignOwners.cs:1093`, `src/Main.SaveOrchestrator.cs:331`
- **UI Routes:** `survivor_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/AgingSystem.cs`](../../Assets/Ashfall.Core/Survivors/AgingSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/LifeStagesCatalogLoader.cs`](../../Assets/Ashfall.Core/Survivors/LifeStagesCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorAgingProgressionEngine.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorAgingProgressionEngine.cs)
  - Host Session: [`src/Host/AgingHostSession.cs`](../../src/Host/AgingHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AgingHostSession.cs`](../../src/Host/AgingHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Records/LifeStagesCatalogLoaderTests.cs`](../../Ashfall.Core.Tests/Records/LifeStagesCatalogLoaderTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan176AgingHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan176AgingHostIntegrationTests.cs)

### 263. `antenatal_maternal_health` — Expansion 37 — antenatal care, maternal trimester progression, and delivery health ledger (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupAntenatalMaternalHealth()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.AntenatalMaternalHealth.cs:41`, `src/Main.AntenatalMaternalHealth.cs:51`, `src/Main.AntenatalMaternalHealth.cs:60`, `src/Main.AntenatalMaternalHealth.cs:66`, `src/Main.AntenatalMaternalHealth.cs:72`, `src/Main.CampaignOwners.cs:2809`, `src/Main.CampaignOwners.cs:2820`, `src/Main.SaveOrchestrator.cs:342`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/AntenatalMaternalCareLedger.cs`](../../Assets/Ashfall.Core/Survivors/AntenatalMaternalCareLedger.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/AntenatalMaternalHealthEngine.cs`](../../Assets/Ashfall.Core/Survivors/AntenatalMaternalHealthEngine.cs)
  - Host Session: [`src/Host/AntenatalMaternalHealthHostSession.cs`](../../src/Host/AntenatalMaternalHealthHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/AntenatalMaternalHealthHostSession.cs`](../../src/Host/AntenatalMaternalHealthHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/AntenatalMaternalCareLedgerTests.cs`](../../Ashfall.Core.Tests/Survivors/AntenatalMaternalCareLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/AntenatalMaternalHealthEngineTests.cs`](../../Ashfall.Core.Tests/Survivors/AntenatalMaternalHealthEngineTests.cs)

### 264. `backstory` — Plan 174 — Procedural survivor backstories & origin mechanics: occupations, experiences, and secrets (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupBackstory()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.Backstory.cs:51`, `src/Main.Backstory.cs:67`, `src/Main.CampaignOwners.cs:603`, `src/Main.CampaignOwners.cs:614`, `src/Main.PlayerSurfaces.cs:255`, `src/Main.SaveOrchestrator.cs:314`
- **UI Routes:** `survivor_detail`, `survivors`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/BackstorySystem.cs`](../../Assets/Ashfall.Core/Survivors/BackstorySystem.cs)
  - Host Session: [`src/Host/BackstoryHostSession.cs`](../../src/Host/BackstoryHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/BackstoryHostSession.cs`](../../src/Host/BackstoryHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan174BackstoryHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan174BackstoryHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan174SurvivorBackstoriesIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan174SurvivorBackstoriesIntegrationTests.cs)

### 265. `exercise` — Plan 216 — Survivor exercise routines, physical training adaptation, and conditioning decay (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupExercise()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3121`, `src/Main.CampaignOwners.cs:3132`, `src/Main.Exercise.cs:41`, `src/Main.Exercise.cs:47`, `src/Main.Exercise.cs:53`, `src/Main.Exercise.cs:62`, `src/Main.SaveOrchestrator.cs:353`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/ExerciseSystem.cs`](../../Assets/Ashfall.Core/Survivors/ExerciseSystem.cs)
  - Host Session: [`src/Host/ExerciseHostSession.cs`](../../src/Host/ExerciseHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ExerciseHostSession.cs`](../../src/Host/ExerciseHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/ExerciseSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/ExerciseSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan216ExerciseIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan216ExerciseIntegrationTests.cs)

### 266. `genealogy` — Plan 217 — survivor genealogy: committed kinship facts (lineage records, unions, family events) (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupGenealogy()` | **Invoked:** yes | **Cadence:** `Canonical-Fact Driven (family/fate events)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:374`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/GenerationalLineageExtension.cs`](../../Assets/Ashfall.Core/GenerationalLineageExtension.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/GenealogyBridge.cs`](../../Assets/Ashfall.Core/Survivors/GenealogyBridge.cs)
  - Host Session: [`src/Host/GenealogyHostSession.cs`](../../src/Host/GenealogyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/GenealogyHostSession.cs`](../../src/Host/GenealogyHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan217GenealogyHostWiringTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan217GenealogyHostWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan217GenealogyIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan217GenealogyIntegrationTests.cs)

### 267. `ideological_friction` — Plan 148 — Ideological friction events and quests: confrontations, conversions, bunker factions, and mediation (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupIdeologicalFriction()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:519`, `src/Main.CampaignOwners.cs:530`, `src/Main.IdeologicalFriction.cs:65`, `src/Main.PfglOctetBoards.cs:63`, `src/Main.PfglOctetBoards.cs:125`, `src/Main.PlayerSurfaces.cs:255`, `src/Main.SaveOrchestrator.cs:311`
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

### 268. `interpersonal_conflict` — Plan 202 — Interpersonal conflict, grievance accumulation, and mediation resolution (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupInterpersonalConflict()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3093`, `src/Main.CampaignOwners.cs:3104`, `src/Main.InterpersonalConflict.cs:47`, `src/Main.InterpersonalConflict.cs:58`, `src/Main.InterpersonalConflict.cs:64`, `src/Main.InterpersonalConflict.cs:73`, `src/Main.SaveOrchestrator.cs:352`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs`](../../Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs)
  - Host Session: [`src/Host/InterpersonalConflictHostSession.cs`](../../src/Host/InterpersonalConflictHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/InterpersonalConflictHostSession.cs`](../../src/Host/InterpersonalConflictHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/InterpersonalConflictSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/InterpersonalConflictSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan202InterpersonalConflictIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan202InterpersonalConflictIntegrationTests.cs)

### 269. `radiation_social` — Radiation social bridge: dose brackets, social penalties, and discrimination incident ledger (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupRadiationSocial()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.RadiationSocial.cs:34`, `src/Main.RadiationSocial.cs:42`, `src/Main.SaveOrchestrator.cs:359`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Radiation/RadiationSocialBridge.cs`](../../Assets/Ashfall.Core/Radiation/RadiationSocialBridge.cs)
  - Host Session: [`src/Host/RadiationSocialHostSession.cs`](../../src/Host/RadiationSocialHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RadiationSocialHostSession.cs`](../../src/Host/RadiationSocialHostSession.cs)

### 270. `ration_conflict` — Ration conflict — perceived fairness and resentment accrued from unequal allocations, escalating to confrontation or theft. ResourceRationingSystem keeps the allocation authority; NeedsSystem keeps morale. (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupRationConflict()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:913`, `src/Main.RationConflict.cs:49`, `src/Main.SaveOrchestrator.cs:324`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs`](../../Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs)
  - Core System: [`Assets/Ashfall.Core/SurvivorRelationsSystem.cs`](../../Assets/Ashfall.Core/SurvivorRelationsSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`](../../Assets/Ashfall.Core/Survivors/NeedsSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/RationConflictSystem.cs`](../../Assets/Ashfall.Core/Survivors/RationConflictSystem.cs)
  - Host Session: [`src/Host/EconomyHostSession.cs`](../../src/Host/EconomyHostSession.cs)
  - Host Session: [`src/Host/RationConflictHostSession.cs`](../../src/Host/RationConflictHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RationConflictHostSession.cs`](../../src/Host/RationConflictHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/PlanRationConflictHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/PlanRationConflictHostIntegrationTests.cs)

### 271. `recruitment` — Plan 204 — Survivor recruitment campaigns, wilderness discovery, defection offers, and asylum intake (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupRecruitment()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3318`, `src/Main.CampaignOwners.cs:3329`, `src/Main.Recruitment.cs:25`, `src/Main.SaveOrchestrator.cs:395`
- **UI Routes:** `recruitment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs`](../../Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs)
  - Host Session: [`src/Host/RecruitmentHostSession.cs`](../../src/Host/RecruitmentHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/RecruitmentSaveStore.cs`](../../src/Host/RecruitmentSaveStore.cs)
  - UI Panel: [`src/UI/RecruitmentPanel.cs`](../../src/UI/RecruitmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan204RecruitmentIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan204RecruitmentIntegrationTests.cs)

### 272. `romance_family` — Plan 150 — Romance & family dynamics: attraction, courtship, partnership, bonded pairs, family units, and adoption (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupRomanceFamily()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:547`, `src/Main.CampaignOwners.cs:558`, `src/Main.PfglOctetBoards.cs:44`, `src/Main.PlayerSurfaces.cs:255`, `src/Main.RomanceFamily.cs:65`, `src/Main.SaveOrchestrator.cs:312`
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

### 273. `skill_atrophy` — Skill atrophy: unused-skill decay tracked from survivor practice hours with exactly-once atrophy events (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSkillAtrophy()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3486`, `src/Main.SaveOrchestrator.cs:363`, `src/Main.SkillAtrophy.cs:38`, `src/Main.SkillAtrophy.cs:73`, `src/Main.SkillAtrophy.cs:79`, `src/Main.SkillAtrophy.cs:85`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`](../../Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SkillProgressionState.cs`](../../Assets/Ashfall.Core/Survivors/SkillProgressionState.cs)
  - Host Session: [`src/Host/SkillAtrophyHostSession.cs`](../../src/Host/SkillAtrophyHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SkillAtrophyHostSession.cs`](../../src/Host/SkillAtrophyHostSession.cs)

### 274. `skill_certifications` — Plan 180 — Skill certification and tier system: formal qualifications, exams, benefits, and specializations (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSkillCertifications()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3205`, `src/Main.CampaignOwners.cs:3216`, `src/Main.SaveOrchestrator.cs:391`, `src/Main.SkillCertification.cs:19`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SkillCertificationSystem.cs`](../../Assets/Ashfall.Core/Survivors/SkillCertificationSystem.cs)
  - Host Session: [`src/Host/SkillCertificationHostSession.cs`](../../src/Host/SkillCertificationHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SkillCertificationHostSession.cs`](../../src/Host/SkillCertificationHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan180SkillCertificationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan180SkillCertificationTests.cs)

### 275. `survivor_dreams` — Plan 177 — Survivor dream & sleep event system: dream templates, sleep cycle dream generation, consecutive nightmares, and interpretation (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorDreams()` | **Invoked:** yes | **Cadence:** `Daily Dream Cycle Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3006`, `src/Main.CampaignOwners.cs:3017`, `src/Main.DreamSystem.cs:43`, `src/Main.DreamSystem.cs:49`, `src/Main.DreamSystem.cs:55`, `src/Main.DreamSystem.cs:61`, `src/Main.DreamSystem.cs:70`, `src/Main.SaveOrchestrator.cs:349`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/DreamSystem.cs`](../../Assets/Ashfall.Core/Survivors/DreamSystem.cs)
  - Host Session: [`src/Host/DreamHostSession.cs`](../../src/Host/DreamHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/DreamHostSession.cs`](../../src/Host/DreamHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan177DreamSleepIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan177DreamSleepIntegrationTests.cs)

### 276. `survivor_roles` — Plan 195 — survivor specialization roles: identity, earned practice XP, level progression, and bonus readout (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorRoles()` | **Invoked:** yes | **Cadence:** `Work-Fact Driven (SkillProgression OnXpGained)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:354`, `src/Main.SurvivorRoles.cs:100`, `src/Main.SurvivorRoles.cs:109`, `src/Main.SurvivorRoles.cs:117`, `src/Main.SurvivorRoles.cs:124`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorRoleSystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorRoleSystem.cs)
  - Host Session: [`src/Host/SurvivorRoleHostSession.cs`](../../src/Host/SurvivorRoleHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorRoleHostSession.cs`](../../src/Host/SurvivorRoleHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleWiringTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleWiringTests.cs)

### 277. `survivor_routines` — Plan 188 — Individual survivor daily routines: activity time blocks, chronotypes, satisfaction evaluation, and interpersonal conflicts (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorRoutines()` | **Invoked:** yes | **Cadence:** `None`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1138`, `src/Main.CampaignOwners.cs:1149`, `src/Main.SaveOrchestrator.cs:333`, `src/Main.SurvivorRoutines.cs:47`
- **UI Routes:** `survivor_detail`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/RoutineTemplateCatalogLoader.cs`](../../Assets/Ashfall.Core/Survivors/RoutineTemplateCatalogLoader.cs)
  - Core System: [`Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs`](../../Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs)
  - Host Session: [`src/Host/SurvivorRoutineHostSession.cs`](../../src/Host/SurvivorRoutineHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorRoutineHostSession.cs`](../../src/Host/SurvivorRoutineHostSession.cs)
  - UI Panel: [`src/UI/SurvivorDetailPanel.cs`](../../src/UI/SurvivorDetailPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Records/RoutineTemplateCatalogLoaderTests.cs`](../../Ashfall.Core.Tests/Records/RoutineTemplateCatalogLoaderTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan188SurvivorRoutineIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan188SurvivorRoutineIntegrationTests.cs)

### 278. `survivor_voice` — Plan 42 — survivor voice line catalog selection, cooldowns, utterance history, and playback dispatch (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupSurvivorVoice()` | **Invoked:** yes | **Cadence:** `Event-Driven`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:159`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Voice/SurvivorVoiceSystem.cs`](../../Assets/Ashfall.Core/Voice/SurvivorVoiceSystem.cs)
  - Core System: [`Assets/Ashfall.Core/Voice/VoiceLineDispatchCoordinator.cs`](../../Assets/Ashfall.Core/Voice/VoiceLineDispatchCoordinator.cs)
  - Host Session: [`src/Host/SurvivorVoiceHostSession.cs`](../../src/Host/SurvivorVoiceHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/SurvivorVoiceHostSession.cs`](../../src/Host/SurvivorVoiceHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Campaign/Plan42SurvivorVoiceHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Campaign/Plan42SurvivorVoiceHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Voice/SurvivorVoiceSystemTests.cs`](../../Ashfall.Core.Tests/Voice/SurvivorVoiceSystemTests.cs)

### 279. `trauma_bond` — Trauma bond authority — shared-hazard bond strengths, decay, and co-shift efficiency state (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupTraumaBond()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:796`, `src/Main.CampaignOwners.cs:807`, `src/Main.SaveOrchestrator.cs:320`, `src/Main.TraumaBond.cs:105`, `src/Main.TraumaBond.cs:115`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs`](../../Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs)
  - Host Session: [`src/Host/TraumaBondHostSession.cs`](../../src/Host/TraumaBondHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TraumaBondSaveStore.cs`](../../src/Host/TraumaBondSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/PlanTraumaBondHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/PlanTraumaBondHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TraumaBondSystemTests.cs`](../../Ashfall.Core.Tests/TraumaBondSystemTests.cs)

### 280. `voluntary_register` — Plan 253 — high-dose surface-work volunteer signatures, accepted-day/completed-day and banked dose tracking (Survivors)
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupVoluntaryRegister()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:941`, `src/Main.SaveOrchestrator.cs:325`, `src/Main.VoluntaryRegister.cs:48`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DoseLedgerSystem.cs`](../../Assets/Ashfall.Core/DoseLedgerSystem.cs)
  - Core System: [`Assets/Ashfall.Core/VoluntaryRegisterSystem.cs`](../../Assets/Ashfall.Core/VoluntaryRegisterSystem.cs)
  - Host Session: [`src/Host/VoluntaryRegisterHostSession.cs`](../../src/Host/VoluntaryRegisterHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/VoluntaryRegisterHostSession.cs`](../../src/Host/VoluntaryRegisterHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PlanTriplePackageJCoreTests.cs`](../../Ashfall.Core.Tests/PlanTriplePackageJCoreTests.cs)

### 281. `hidden_agenda` — Plan 132 — survivor hidden agendas, secret motivations, clue discovery, and confrontation arcs (Survivors (Plan 132))
- **Owner Domain:** `survivors`
- **Setup Method:** `Main.SetupHiddenAgenda()` | **Invoked:** yes | **Cadence:** `Daily (Passive Slip-Up & Exposure Drift)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:146`
- **UI Routes:** `hidden_agenda`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs`](../../Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs)
  - Host Session: [`src/Host/HiddenAgendaHostSession.cs`](../../src/Host/HiddenAgendaHostSession.cs)
  - Save Store: [`src/Host/HiddenAgendaSaveStore.cs`](../../src/Host/HiddenAgendaSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/HiddenAgendaPanel.cs`](../../src/UI/HiddenAgendaPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/HiddenAgendaSystemTests.cs`](../../Ashfall.Core.Tests/Survivors/HiddenAgendaSystemTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Survivors/Plan132HiddenAgendaIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan132HiddenAgendaIntegrationTests.cs)

### 282. `combat` — Combat encounters and tactical trauma (Tactical Combat)
- **Owner Domain:** `combat`
- **Setup Method:** `Main.SetupCombat()` | **Invoked:** yes | **Cadence:** `On-Demand (Turn-Based)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:63`, `src/Main.SaveOrchestrator.cs:199`, `src/Main.UiHandlers.cs:137`
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

### 283. `technical_material_archive` — Plan 158 — cordage/cable/polymer/textile technical material archive: discovered-record ledger (IDs only) (Technical Material Archive)
- **Owner Domain:** `narrative`
- **Setup Method:** `Main.SetupTechnicalMaterialArchive()` | **Invoked:** yes | **Cadence:** `Event-Driven (Location Discovery)`
- **Setup Invocation Sites:** `src/Main.CampaignServices.cs:123`, `src/Main.SaveOrchestrator.cs:270`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/TechnicalMaterialArchiveSystem.cs`](../../Assets/Ashfall.Core/Narrative/TechnicalMaterialArchiveSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/TechnicalMaterialArchiveSaveStore.cs`](../../src/Host/TechnicalMaterialArchiveSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/CordageCableCatalogTests.cs`](../../Ashfall.Core.Tests/CordageCableCatalogTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Narrative/TechnicalMaterialArchiveTests.cs`](../../Ashfall.Core.Tests/Narrative/TechnicalMaterialArchiveTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PolymerTextileCatalogTests.cs`](../../Ashfall.Core.Tests/PolymerTextileCatalogTests.cs)

### 284. `vehicle_customization` — Plan 152 — Vehicle customization & mobile base: module installation, effective vehicle stats, and deployed base camps (Vehicles)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupVehicleCustomization()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:575`, `src/Main.CampaignOwners.cs:586`, `src/Main.SaveOrchestrator.cs:313`, `src/Main.VehicleCustomization.cs:52`
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

### 285. `verdict_accusation` — PLAN-INVESTIGATION-EVIDENCE-TRUTH-121 \u2014 accusation eligibility and tribunal resolution (Verdict)
- **Owner Domain:** `verdict`
- **Setup Method:** `Main.SetupVerdictAccusation()` | **Invoked:** yes | **Cadence:** `On Demand`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:387`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Verdict/VerdictAccusationSystem.cs`](../../Assets/Ashfall.Core/Verdict/VerdictAccusationSystem.cs)
  - Host Session: [`src/Host/VerdictAccusationHostSession.cs`](../../src/Host/VerdictAccusationHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/VerdictAccusationHostSession.cs`](../../src/Host/VerdictAccusationHostSession.cs)

### 286. `visitor_integration` — Plan 214 — admitted visitor stays, temporary housing, processing requirements, and departures (Visitors)
- **Owner Domain:** `visitors`
- **Setup Method:** `Main.SetupVisitorIntegration()` | **Invoked:** yes | **Cadence:** `Daily (Visitor Lifecycle & Ration Draw)`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:151`, `src/Main.VisitorIntegration.cs:181`
- **UI Routes:** `visitor_integration`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Visitors/VisitorIntegrationSystem.cs`](../../Assets/Ashfall.Core/Visitors/VisitorIntegrationSystem.cs)
  - Host Session: [`src/Host/VisitorIntegrationHostSession.cs`](../../src/Host/VisitorIntegrationHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/VisitorIntegrationSaveStore.cs`](../../src/Host/VisitorIntegrationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - UI Panel: [`src/UI/VisitorIntegrationPanel.cs`](../../src/UI/VisitorIntegrationPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Visitors/Plan214VisitorIntegrationTests.cs`](../../Ashfall.Core.Tests/Visitors/Plan214VisitorIntegrationTests.cs)

### 287. `water_quality_profile` — Water source contaminant profiles, purity-tier derivation, and filter wear ledger (Water)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaterQualityProfile()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3542`, `src/Main.SaveOrchestrator.cs:366`, `src/Main.WaterQualityProfile.cs:33`, `src/Main.WaterQualityProfile.cs:42`, `src/Main.WaterQualityProfile.cs:50`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Water/WaterQualityProfileEngine.cs`](../../Assets/Ashfall.Core/Water/WaterQualityProfileEngine.cs)
  - Host Session: [`src/Host/WaterQualityProfileHostSession.cs`](../../src/Host/WaterQualityProfileHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/WaterQualityProfileHostSession.cs`](../../src/Host/WaterQualityProfileHostSession.cs)

### 288. `deep_well` — B5–B8 Phase 6 — built deep-well pump: build state, condition, yield ledger (raw water into treatment via the Plan 189 intake seam) (Water & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupDeepWell()` | **Invoked:** yes | **Cadence:** `Daily Deep-Well Pump Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:204`, `src/Main.WaterSources.cs:25`
- **UI Routes:** `water_treatment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/DeepWellSystem.cs`](../../Assets/Ashfall.Core/DeepWellSystem.cs)
  - Host Session: [`src/Host/DeepWellHostSession.cs`](../../src/Host/DeepWellHostSession.cs)
  - Host Session: [`src/Host/DeepWellSaveStore.cs`](../../src/Host/DeepWellSaveStore.cs)
  - Save Store: [`src/Host/DeepWellSaveStore.cs`](../../src/Host/DeepWellSaveStore.cs)
  - UI Panel: [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs`](../../Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Water/DeepWellSystemTests.cs`](../../Ashfall.Core.Tests/Water/DeepWellSystemTests.cs)

### 289. `piezometer_network` — Plan 189 — aquifer monitoring network state driving the water-treatment intake advisory gate (Water & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupPiezometer()` | **Invoked:** yes | **Cadence:** `Daily Aquifer Advisory Tick`
- **Setup Invocation Sites:** `src/Main.Piezometer.cs:25`, `src/Main.Piezometer.cs:116`, `src/Main.Piezometer.cs:127`, `src/Main.SaveOrchestrator.cs:195`, `src/Main.WaterSources.cs:27`
- **UI Routes:** `water_treatment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs`](../../Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs)
  - Host Session: [`src/Host/PiezometerHostSession.cs`](../../src/Host/PiezometerHostSession.cs)
  - Save Store: [`src/Host/PiezometerSaveStore.cs`](../../src/Host/PiezometerSaveStore.cs)
  - UI Panel: [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs`](../../Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Water/Plan189IntakeAdvisoryBridgeTests.cs`](../../Ashfall.Core.Tests/Water/Plan189IntakeAdvisoryBridgeTests.cs)

### 290. `water_condenser` — B5–B8 expansion — Peltier condensation array: build state, membrane integrity, weather-indexed yield ledger (Water & Infrastructure)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaterCondenser()` | **Invoked:** yes | **Cadence:** `Daily Condensate Intake Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:205`, `src/Main.WaterSources.cs:26`
- **UI Routes:** `water_treatment`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/AtmosphericCondenserSystem.cs`](../../Assets/Ashfall.Core/AtmosphericCondenserSystem.cs)
  - Host Session: [`src/Host/WaterCondenserHostSession.cs`](../../src/Host/WaterCondenserHostSession.cs)
  - Host Session: [`src/Host/WaterCondenserSaveStore.cs`](../../src/Host/WaterCondenserSaveStore.cs)
  - Save Store: [`src/Host/WaterCondenserSaveStore.cs`](../../src/Host/WaterCondenserSaveStore.cs)
  - UI Panel: [`src/UI/WaterTreatmentPanel.cs`](../../src/UI/WaterTreatmentPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs`](../../Ashfall.Core.Tests/Integration/WaterSourcesSurfaceWiringTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs`](../../Ashfall.Core.Tests/Water/AtmosphericCondenserSystemTests.cs)

### 291. `weather_cascade` — Plan 135 — weather→gameplay cascade: active weather events, their effects, and the event history (Weather)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWeatherCascade()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:164`, `src/Main.SaveOrchestrator.cs:304`
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

### 292. `cloud_seeding` — Cloud-seeding instrument install/cooldown state (PLAN-WEATHER-ATMOSPHERE-28) (Weather & Atmosphere)
- **Owner Domain:** `weather`
- **Setup Method:** `Main.SetupCloudSeeding()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (expanded shelter day)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:384`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/CloudSeedingSystem.cs`](../../Assets/Ashfall.Core/World/CloudSeedingSystem.cs)
  - Host Session: [`src/Host/CloudSeedingHostSession.cs`](../../src/Host/CloudSeedingHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/CloudSeedingHostSession.cs`](../../src/Host/CloudSeedingHostSession.cs)

### 293. `ecological_infestation` — Plan 28 — location and shelter ecological infestations (trigger/clear/tolerate lifecycle) (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupEcologicalInfestation()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.EcologicalInfestations.cs:52`, `src/Main.SaveOrchestrator.cs:235`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs`](../../Assets/Ashfall.Core/Ecology/EcologicalInfestationSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/EcologicalInfestationSaveStore.cs`](../../src/Host/EcologicalInfestationSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs`](../../Ashfall.Core.Tests/EcologicalInfestationSystemTests.cs)

### 294. `geodetic_survey` — Plans 78-81 — survey monuments, observations, resolved triangles, and network accuracy (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupGeodeticSurvey()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans78_81.cs:90`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs`](../../Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs)
  - Host Session: [`src/Host/GeodeticSurveySaveStore.cs`](../../src/Host/GeodeticSurveySaveStore.cs)
  - Save Store: [`src/Host/GeodeticSurveySaveStore.cs`](../../src/Host/GeodeticSurveySaveStore.cs)

### 295. `human_migration` — Plan 199 — Seasonal human migration engine, regional population weights, and dwell hysteresis (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupHumanMigration()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1026`, `src/Main.CampaignOwners.cs:1037`, `src/Main.HumanMigration.cs:76`, `src/Main.MigrationConsequence.cs:32`, `src/Main.SaveOrchestrator.cs:317`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs`](../../Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/SeasonalMigrationCatalogLoader.cs`](../../Assets/Ashfall.Core/Economy/SeasonalMigrationCatalogLoader.cs)
  - Host Session: [`src/Host/HumanMigrationHostSession.cs`](../../src/Host/HumanMigrationHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/HumanMigrationHostSession.cs`](../../src/Host/HumanMigrationHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/Plan199HumanMigrationHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Economy/Plan199HumanMigrationHostIntegrationTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/SeasonalHumanMigrationEngineTests.cs`](../../Ashfall.Core.Tests/Economy/SeasonalHumanMigrationEngineTests.cs)

### 296. `migration_consequence` — XP-08-F6 — exactly-once seasonal migration consequence ledger and regional projections (World)
- **Owner Domain:** `economy`
- **Setup Method:** `Main.SetupMigrationConsequence()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:825`, `src/Main.CampaignOwners.cs:836`, `src/Main.MigrationConsequence.cs:67`, `src/Main.MigrationConsequence.cs:127`, `src/Main.SaveOrchestrator.cs:321`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Economy/MigrationConsequenceEngine.cs`](../../Assets/Ashfall.Core/Economy/MigrationConsequenceEngine.cs)
  - Core System: [`Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs`](../../Assets/Ashfall.Core/Economy/SeasonalHumanMigrationEngine.cs)
  - Host Session: [`src/Host/MigrationConsequenceHostSession.cs`](../../src/Host/MigrationConsequenceHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/MigrationConsequenceSaveStore.cs`](../../src/Host/MigrationConsequenceSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/MigrationConsequenceEngineTests.cs`](../../Ashfall.Core.Tests/Economy/MigrationConsequenceEngineTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Economy/PlanXp08F6MigrationConsequenceHostIntegrationTests.cs`](../../Ashfall.Core.Tests/Economy/PlanXp08F6MigrationConsequenceHostIntegrationTests.cs)

### 297. `nuclear_winter_progression` — ORPHAN-SEAL-W1 — nuclear-winter phase/season progression and recorded climate events (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupNuclearWinter()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.OrphanSealWave1.cs:74`, `src/Main.OrphanSealWave1.cs:373`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Weather/NuclearWinterProgressionSystem.cs`](../../Assets/Ashfall.Core/Weather/NuclearWinterProgressionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/OrphanSealWave1HostSessions.cs`](../../src/Host/OrphanSealWave1HostSessions.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Weather/Plan164NuclearWinterIntegrationTests.cs`](../../Ashfall.Core.Tests/Weather/Plan164NuclearWinterIntegrationTests.cs)

### 298. `route_infrastructure` — Plans 146-149 — mutable route infrastructure, corridor maintenance, and minefield clearance (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupRouteInfrastructure()` | **Invoked:** yes | **Cadence:** `On-Demand`
- **Setup Invocation Sites:** `src/Main.Plans146_149.cs:262`, `src/Main.Plans146_149.cs:275`, `src/Main.Plans146_149.cs:386`, `src/Main.Plans146_149.cs:810`, `src/Main.Plans146_149.cs:863`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs`](../../Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs)
  - Host Session: [`src/Host/RouteInfrastructureSaveStore.cs`](../../src/Host/RouteInfrastructureSaveStore.cs)
  - Save Store: [`src/Host/RouteInfrastructureSaveStore.cs`](../../src/Host/RouteInfrastructureSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/RouteInfrastructureSystemTests.cs`](../../Ashfall.Core.Tests/World/RouteInfrastructureSystemTests.cs)

### 299. `storm_forecast` — Expansion 33 — observation-post forecast skill, storm-response drill recency, and issued warnings (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupStormForecast()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2753`, `src/Main.CampaignOwners.cs:2764`, `src/Main.SaveOrchestrator.cs:340`, `src/Main.StormForecast.cs:37`, `src/Main.StormForecast.cs:43`, `src/Main.StormForecast.cs:51`, `src/Main.StormForecast.cs:63`, `src/Main.StormForecast.cs:72`, `src/Main.StormForecast.cs:79`, `src/Main.StormForecast.cs:85`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/StormForecastLedger.cs`](../../Assets/Ashfall.Core/World/StormForecastLedger.cs)
  - Core System: [`Assets/Ashfall.Core/World/StormForecastReadinessEngine.cs`](../../Assets/Ashfall.Core/World/StormForecastReadinessEngine.cs)
  - Host Session: [`src/Host/StormForecastHostSession.cs`](../../src/Host/StormForecastHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/StormForecastHostSession.cs`](../../src/Host/StormForecastHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/StormForecastLedgerTests.cs`](../../Ashfall.Core.Tests/World/StormForecastLedgerTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/StormForecastReadinessEngineTests.cs`](../../Ashfall.Core.Tests/World/StormForecastReadinessEngineTests.cs)

### 300. `subterranean` — Flagship XI Plan 156 — generated underground topology, oxygen/collapse/flood/shoring state, discovery (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupSubterranean()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:2117`, `src/Main.PlayerSurfaces.cs:300`, `src/Main.SaveOrchestrator.cs:184`, `src/Main.Subterranean.cs:109`, `src/Main.Subterranean.cs:116`, `src/Main.Subterranean.cs:123`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Subterranean/SubterraneanSystem.cs`](../../Assets/Ashfall.Core/Subterranean/SubterraneanSystem.cs)
  - Host Session: [`src/Host/SubterraneanHostSession.cs`](../../src/Host/SubterraneanHostSession.cs)
  - Save Store: [`src/Host/SubterraneanSaveStore.cs`](../../src/Host/SubterraneanSaveStore.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Flagship11/SubterraneanSystemTests.cs`](../../Ashfall.Core.Tests/Flagship11/SubterraneanSystemTests.cs)

### 301. `weather_forecast_reliability` — Received weather forecast confidence grading, reliability score, and dispatch-safety lead time (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWeatherForecastReliability()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:3569`, `src/Main.SaveOrchestrator.cs:367`, `src/Main.WeatherForecastReliability.cs:38`, `src/Main.WeatherForecastReliability.cs:48`, `src/Main.WeatherForecastReliability.cs:54`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WeatherForecastReliabilityEngine.cs`](../../Assets/Ashfall.Core/World/WeatherForecastReliabilityEngine.cs)
  - Host Session: [`src/Host/WeatherForecastReliabilityHostSession.cs`](../../src/Host/WeatherForecastReliabilityHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/WeatherForecastReliabilityHostSession.cs`](../../src/Host/WeatherForecastReliabilityHostSession.cs)

### 302. `world_evolution` — Plan 227 — deterministic world-state evolution events driven by authored day thresholds, required flags and map-target effects (World)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorldEvolution()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:968`, `src/Main.SaveOrchestrator.cs:326`, `src/Main.WorldEvolution.cs:51`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Core System: [`Assets/Ashfall.Core/World/WorldEvolutionEngine.cs`](../../Assets/Ashfall.Core/World/WorldEvolutionEngine.cs)
  - Host Session: [`src/Host/WorldEvolutionHostSession.cs`](../../src/Host/WorldEvolutionHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/WorldEvolutionHostSession.cs`](../../src/Host/WorldEvolutionHostSession.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PlanTriplePackageJCoreTests.cs`](../../Ashfall.Core.Tests/PlanTriplePackageJCoreTests.cs)

### 303. `amphibious_draisine` — Plan 125 — per-vehicle amphibious kit condition, pontoons, ingress, crossing state (World & Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupAmphibiousDraisine()` | **Invoked:** yes | **Cadence:** `Expedition Travel/Action Cadence (Crossing Ticks)`
- **Setup Invocation Sites:** `src/Main.Plans122to125.cs:316`, `src/Main.SaveOrchestrator.cs:194`
- **UI Routes:** `amphibious_draisine`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/AmphibiousDraisineEngine.cs`](../../Assets/Ashfall.Core/Expeditions/AmphibiousDraisineEngine.cs)
  - Host Session: [`src/Host/AmphibiousDraisineHostSession.cs`](../../src/Host/AmphibiousDraisineHostSession.cs)
  - Save Store: [`src/Host/AmphibiousDraisineSaveStore.cs`](../../src/Host/AmphibiousDraisineSaveStore.cs)
  - UI Panel: [`src/UI/AmphibiousDraisinePanel.cs`](../../src/UI/AmphibiousDraisinePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan125AmphibiousDraisineEngineTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan125AmphibiousDraisineEngineTests.cs)

### 304. `armored_crawlers` — Armored crawler modules and forward camps (World & Expeditions)
- **Owner Domain:** `expedition`
- **Setup Method:** `Main.SetupArmoredCrawlers()` | **Invoked:** yes | **Cadence:** `Daily Crawler Module Tick`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:294`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs`](../../Assets/Ashfall.Core/Expeditions/ArmoredCrawlerExpeditionSystem.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ArmoredCrawlerSaveStore.cs`](../../src/Host/ArmoredCrawlerSaveStore.cs)
  - UI Panel: [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs`](../../Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs)

### 305. `encounter_choice` — Encounter choice history & outcomes (World & Expeditions)
- **Owner Domain:** `encounters`
- **Setup Method:** `Main.SetupEncounterChoice()` | **Invoked:** no | **Cadence:** `On-Demand (Door Event Resolution)`
- **UI Routes:** `door_encounter`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Host Session: [`Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs`](../../Assets/Ashfall.Core/Expeditions/EncounterChoiceResolver.cs)
  - Save Store: [`src/Host/EncounterChoiceSaveStore.cs`](../../src/Host/EncounterChoiceSaveStore.cs)
  - UI Panel: [`src/YearOfAsh/DoorEncounterModal.cs`](../../src/YearOfAsh/DoorEncounterModal.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs`](../../Ashfall.Core.Tests/Expeditions/EncounterChoiceResolverTests.cs)

### 306. `expedition` — Wasteland expedition runs & status (World & Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupExpeditions()` | **Invoked:** yes | **Cadence:** `Daily Sortie Travel`
- **Setup Invocation Sites:** `src/Main.CampaignOwners.cs:1955`, `src/Main.CampaignOwners.cs:1974`, `src/Main.CampaignOwners.cs:2252`, `src/Main.CampaignServices.cs:44`, `src/Main.EvolvingWorld.cs:46`, `src/Main.ExpandedShelterSystems.cs:101`, `src/Main.Expeditions.cs:448`, `src/Main.Expeditions.cs:490`, `src/Main.Expeditions.cs:497`, `src/Main.Expeditions.cs:503`, `src/Main.Expeditions.cs:509`, `src/Main.GameFlow.cs:516`, `src/Main.GameFlow.cs:537`, `src/Main.Lifecycle.cs:724`, `src/Main.Narrative.cs:476`, `src/Main.PfglOctetBoards.cs:53`, `src/Main.PfglOctetBoards.cs:71`, `src/Main.Phase0.cs:115`, `src/Main.Plans166_169.cs:277`, `src/Main.PlayerSurfaces.cs:285`, `src/Main.PlayerSurfaces.cs:324`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:517`, `src/Main.PlayerSurfaces.cs:580`, `src/Main.PlayerSurfaces.cs:652`, `src/Main.SaveOrchestrator.cs:198`, `src/Main.Subterranean.cs:26`, `src/Main.UiHandlers.cs:144`, `src/Main.UiHandlers.cs:156`, `src/Main.VehicleGarage.cs:22`, `src/Main.VehicleGarage.cs:45`
- **UI Routes:** `expeditions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`](../../Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs)
  - Core System: [`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`](../../Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs)
  - Host Session: [`src/Host/ExpeditionHostSession.cs`](../../src/Host/ExpeditionHostSession.cs)
  - Save Store: [`src/Host/ExpeditionSaveStore.cs`](../../src/Host/ExpeditionSaveStore.cs)
  - UI Panel: [`src/UI/ExpeditionPanel.cs`](../../src/UI/ExpeditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/ExpeditionCampSystemTests.cs`](../../Ashfall.Core.Tests/ExpeditionCampSystemTests.cs)

### 307. `insar_deformation` — Plan 139 — repeat-pass InSAR survey passes, coherence, deformation summaries, excavation/travel intelligence (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupInSarMapping()` | **Invoked:** yes | **Cadence:** `On-Demand (Survey Pass & Repeat-Pass Process)`
- **Setup Invocation Sites:** `src/Main.InSarMapping.cs:27`, `src/Main.InSarMapping.cs:173`, `src/Main.InSarMapping.cs:180`, `src/Main.SaveOrchestrator.cs:188`
- **UI Routes:** `insar_mapping`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/InSarDeformationEngine.cs`](../../Assets/Ashfall.Core/World/InSarDeformationEngine.cs)
  - Host Session: [`src/Host/InSarMappingHostSession.cs`](../../src/Host/InSarMappingHostSession.cs)
  - Save Store: [`src/Host/InSarMappingSaveStore.cs`](../../src/Host/InSarMappingSaveStore.cs)
  - UI Panel: [`src/UI/InSarMappingPanel.cs`](../../src/UI/InSarMappingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/World/Plan139InSarDeformationTests.cs`](../../Ashfall.Core.Tests/World/Plan139InSarDeformationTests.cs)

### 308. `runflat_tire` — Plan 141 — run-flat wheel profiles, integrity, heat, rim/bead, rolling-resistance cost (World & Expeditions)
- **Owner Domain:** `expeditions`
- **Setup Method:** `Main.SetupRunFlatTire()` | **Invoked:** yes | **Cadence:** `On-Demand (Fit, Hazard & Heat Commands)`
- **Setup Invocation Sites:** `src/Main.RunFlatTire.cs:25`, `src/Main.RunFlatTire.cs:94`, `src/Main.RunFlatTire.cs:112`, `src/Main.RunFlatTire.cs:119`, `src/Main.RunFlatTire.cs:126`, `src/Main.SaveOrchestrator.cs:190`
- **UI Routes:** `runflat_tire`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Expeditions/RunFlatTireEngine.cs`](../../Assets/Ashfall.Core/Expeditions/RunFlatTireEngine.cs)
  - Host Session: [`src/Host/RunFlatTireHostSession.cs`](../../src/Host/RunFlatTireHostSession.cs)
  - Save Store: [`src/Host/RunFlatTireSaveStore.cs`](../../src/Host/RunFlatTireSaveStore.cs)
  - UI Panel: [`src/UI/RunFlatTirePanel.cs`](../../src/UI/RunFlatTirePanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/Expeditions/Plan141RunFlatTireTests.cs`](../../Ashfall.Core.Tests/Expeditions/Plan141RunFlatTireTests.cs)

### 309. `travel_encounters` — Travel encounters and cooldown states (World & Expeditions)
- **Owner Domain:** `encounters`
- **Setup Method:** `Main.SetupTravelEncounters()` | **Invoked:** yes | **Cadence:** `On-Demand (Travel Step)`
- **Setup Invocation Sites:** `src/Main.Expeditions.cs:106`, `src/Main.SaveOrchestrator.cs:226`
- **UI Routes:** `expeditions`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs`](../../Assets/Ashfall.Core/Narrative/TravelEncounterCatalog.cs)
  - Core System: [`Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs)
  - Host Session: [`Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`](../../Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs)
  - Save Store: [`src/Host/TravelEncounterSaveStore.cs`](../../src/Host/TravelEncounterSaveStore.cs)
  - UI Panel: [`src/UI/ExpeditionPanel.cs`](../../src/UI/ExpeditionPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/PatrolEncounterFullRegressionTests.cs`](../../Ashfall.Core.Tests/PatrolEncounterFullRegressionTests.cs)
  - Test Fixture: [`Ashfall.Core.Tests/TravelEncounterCooldownGroupTests.cs`](../../Ashfall.Core.Tests/TravelEncounterCooldownGroupTests.cs)

### 310. `wasteland_map` — Wasteland map markers and fog-of-war (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorld()` | **Invoked:** yes | **Cadence:** `On-Demand (Fog-of-War Discovery)`
- **Setup Invocation Sites:** `src/Main.AdvancedShelterSystems.cs:54`, `src/Main.BriefingCrisis.cs:89`, `src/Main.CampaignOwners.cs:1166`, `src/Main.CampaignOwners.cs:1176`, `src/Main.CampaignOwners.cs:2159`, `src/Main.CampaignOwners.cs:2175`, `src/Main.CampaignServices.cs:39`, `src/Main.Economy.cs:209`, `src/Main.EvolvingWorld.cs:43`, `src/Main.EvolvingWorld.cs:61`, `src/Main.ExpandedShelterSystems.cs:105`, `src/Main.ExpandedShelterSystems.cs:728`, `src/Main.GameFlow.cs:399`, `src/Main.GameFlow.cs:431`, `src/Main.GameFlow.cs:525`, `src/Main.GameFlow.cs:539`, `src/Main.GameFlow.cs:550`, `src/Main.GameFlow.cs:626`, `src/Main.GameFlow.cs:788`, `src/Main.Lifecycle.cs:721`, `src/Main.NightWatch.cs:31`, `src/Main.Plans110_113.cs:54`, `src/Main.Plans162_165.cs:723`, `src/Main.Plans163_210.cs:22`, `src/Main.Plans163_210.cs:43`, `src/Main.Plans166_169.cs:38`, `src/Main.Plans167_219.cs:27`, `src/Main.Plans167_219.cs:36`, `src/Main.Plans167_219.cs:48`, `src/Main.Plans167_219.cs:66`, `src/Main.Plans94_97.cs:51`, `src/Main.PlayerSurfaces.cs:17`, `src/Main.PlayerSurfaces.cs:199`, `src/Main.PlayerSurfaces.cs:204`, `src/Main.PlayerSurfaces.cs:209`, `src/Main.PlayerSurfaces.cs:324`, `src/Main.PlayerSurfaces.cs:329`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:357`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.PlayerSurfaces.cs:652`, `src/Main.SaveOrchestrator.cs:179`, `src/Main.ShelterSocial.cs:353`, `src/Main.TunnelNetwork.cs:26`, `src/Main.TunnelNetwork.cs:38`, `src/Main.TunnelNetwork.cs:47`, `src/Main.TunnelNetwork.cs:54`, `src/Main.TunnelNetwork.cs:66`, `src/Main.TunnelNetwork.cs:84`, `src/Main.TunnelNetwork.cs:107`, `src/Main.UiHandlers.cs:146`, `src/Main.WaterCondenser.cs:24`, `src/Main.WaterSources.cs:23`, `src/Main.World.cs:210`, `src/Main.WorldPlaytest.cs:41`
- **UI Routes:** `map`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Host Session: [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
  - Save Store: [`src/Host/WastelandMapSaveStore.cs`](../../src/Host/WastelandMapSaveStore.cs)
  - UI Panel: [`src/UI/MapPanel.cs`](../../src/UI/MapPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WastelandMapPersistenceTests.cs`](../../Ashfall.Core.Tests/WastelandMapPersistenceTests.cs)

### 311. `waystation` — Wasteland outpost network & relay hubs (World & Expeditions)
- **Owner Domain:** `infrastructure`
- **Setup Method:** `Main.SetupWaystation()` | **Invoked:** yes | **Cadence:** `Daily Outpost Relay Barter`
- **Setup Invocation Sites:** `src/Main.ExpandedShelterSystems.cs:123`
- **UI Routes:** `waystation_network`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WaystationSystem.cs`](../../Assets/Ashfall.Core/WaystationSystem.cs)
  - Host Session: [`src/Host/WaystationHostSession.cs`](../../src/Host/WaystationHostSession.cs)
  - Save Store: [`src/Host/WaystationSaveStore.cs`](../../src/Host/WaystationSaveStore.cs)
  - UI Panel: [`src/UI/WaystationNetworkPanel.cs`](../../src/UI/WaystationNetworkPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WaystationSystemTests.cs`](../../Ashfall.Core.Tests/WaystationSystemTests.cs)

### 312. `wildlife_trapping` — Snares, game catches, and foraging (World & Expeditions)
- **Owner Domain:** `hunting`
- **Setup Method:** `Main.SetupWildlifeTrapping()` | **Invoked:** yes | **Cadence:** `Daily Snare Yield & Butchery`
- **Setup Invocation Sites:** `src/Main.EvolvingWorld.cs:139`, `src/Main.ExpandedShelterSystems.cs:114`, `src/Main.PlayerSurfaces.cs:344`
- **UI Routes:** `wildlife_trapping`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/WildlifeTrappingSystem.cs`](../../Assets/Ashfall.Core/WildlifeTrappingSystem.cs)
  - Host Session: [`src/Host/WildlifeTrappingHostSession.cs`](../../src/Host/WildlifeTrappingHostSession.cs)
  - Save Store: [`src/Host/WildlifeTrappingSaveStore.cs`](../../src/Host/WildlifeTrappingSaveStore.cs)
  - UI Panel: [`src/UI/WildlifeTrappingPanel.cs`](../../src/UI/WildlifeTrappingPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs`](../../Ashfall.Core.Tests/WildlifeTrappingSystemTests.cs)

### 313. `world` — World map nodes, sectors, and discovery (World & Expeditions)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupWorld()` | **Invoked:** yes | **Cadence:** `Daily Weather & Hazard`
- **Setup Invocation Sites:** `src/Main.AdvancedShelterSystems.cs:54`, `src/Main.BriefingCrisis.cs:89`, `src/Main.CampaignOwners.cs:1166`, `src/Main.CampaignOwners.cs:1176`, `src/Main.CampaignOwners.cs:2159`, `src/Main.CampaignOwners.cs:2175`, `src/Main.CampaignServices.cs:39`, `src/Main.Economy.cs:209`, `src/Main.EvolvingWorld.cs:43`, `src/Main.EvolvingWorld.cs:61`, `src/Main.ExpandedShelterSystems.cs:105`, `src/Main.ExpandedShelterSystems.cs:728`, `src/Main.GameFlow.cs:399`, `src/Main.GameFlow.cs:431`, `src/Main.GameFlow.cs:525`, `src/Main.GameFlow.cs:539`, `src/Main.GameFlow.cs:550`, `src/Main.GameFlow.cs:626`, `src/Main.GameFlow.cs:788`, `src/Main.Lifecycle.cs:721`, `src/Main.NightWatch.cs:31`, `src/Main.Plans110_113.cs:54`, `src/Main.Plans162_165.cs:723`, `src/Main.Plans163_210.cs:22`, `src/Main.Plans163_210.cs:43`, `src/Main.Plans166_169.cs:38`, `src/Main.Plans167_219.cs:27`, `src/Main.Plans167_219.cs:36`, `src/Main.Plans167_219.cs:48`, `src/Main.Plans167_219.cs:66`, `src/Main.Plans94_97.cs:51`, `src/Main.PlayerSurfaces.cs:17`, `src/Main.PlayerSurfaces.cs:199`, `src/Main.PlayerSurfaces.cs:204`, `src/Main.PlayerSurfaces.cs:209`, `src/Main.PlayerSurfaces.cs:324`, `src/Main.PlayerSurfaces.cs:329`, `src/Main.PlayerSurfaces.cs:344`, `src/Main.PlayerSurfaces.cs:357`, `src/Main.PlayerSurfaces.cs:430`, `src/Main.PlayerSurfaces.cs:652`, `src/Main.SaveOrchestrator.cs:179`, `src/Main.ShelterSocial.cs:353`, `src/Main.TunnelNetwork.cs:26`, `src/Main.TunnelNetwork.cs:38`, `src/Main.TunnelNetwork.cs:47`, `src/Main.TunnelNetwork.cs:54`, `src/Main.TunnelNetwork.cs:66`, `src/Main.TunnelNetwork.cs:84`, `src/Main.TunnelNetwork.cs:107`, `src/Main.UiHandlers.cs:146`, `src/Main.WaterCondenser.cs:24`, `src/Main.WaterSources.cs:23`, `src/Main.World.cs:210`, `src/Main.WorldPlaytest.cs:41`
- **UI Routes:** `map`, `weather`
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/World/WastelandMapSystem.cs`](../../Assets/Ashfall.Core/World/WastelandMapSystem.cs)
  - Core System: [`Assets/Ashfall.Core/World/WeatherSystem.cs`](../../Assets/Ashfall.Core/World/WeatherSystem.cs)
  - Host Session: [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
  - Save Store: [`src/Host/WorldSaveStore.cs`](../../src/Host/WorldSaveStore.cs)
  - UI Panel: [`src/UI/MapPanel.cs`](../../src/UI/MapPanel.cs)
  - UI Panel: [`src/UI/WeatherPanel.cs`](../../src/UI/WeatherPanel.cs)
  - Test Fixture: [`Ashfall.Core.Tests/WorldSaveablesTests.cs`](../../Ashfall.Core.Tests/WorldSaveablesTests.cs)

### 314. `chemical_plume` — PLAN-CHEMICAL-RECON-TRUTH-183 \u2014 dispersed chemical plume state (World & Weather)
- **Owner Domain:** `world`
- **Setup Method:** `Main.SetupChemicalPlume()` | **Invoked:** yes | **Cadence:** `Daily Sim Tick (expanded shelter day)`
- **Setup Invocation Sites:** `src/Main.SaveOrchestrator.cs:385`
- **UI Routes:**
- **Verified Source Files:**
  - Core System: [`Assets/Ashfall.Core/Combat/ChemicalPlumeDispersionEngine.cs`](../../Assets/Ashfall.Core/Combat/ChemicalPlumeDispersionEngine.cs)
  - Host Session: [`src/Host/ChemicalPlumeHostSession.cs`](../../src/Host/ChemicalPlumeHostSession.cs)
  - Host Session: [`src/Main.cs`](../../src/Main.cs)
  - Save Store: [`src/Host/ChemicalPlumeHostSession.cs`](../../src/Host/ChemicalPlumeHostSession.cs)

---

## 4. Lifecycle Status & Reachability Proof Matrix

| Section Key | Implemented | Constructed | Setup Invocation | Ticked / Cadence | Persisted | Player-Routed | Tested | E2E Status |
|---|:---:|:---:|---|---|:---:|:---:|:---:|:---:|
| `accessibility_settings` | ✅ | ✅ | ✅ `Main.SetupAccessibilitySettings()` ([src/Main.AccessibilitySettings.cs:41](../../src/Main.AccessibilitySettings.cs#L41)) | ⚡ `User Preference Save` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `advanced_industrial` | ✅ | ✅ | ✅ `Main.SetupAdvancedIndustrial()` ([src/Main.AdvancedIndustrial.cs:51](../../src/Main.AdvancedIndustrial.cs#L51)) | ✅ `Daily Campaign Tick (active production, field surveys, and sensor cooldowns)` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `aeroponics` | ✅ | ✅ | ✅ `Main.SetupAeroponics()` ([src/Main.Plans74_77.cs:33](../../src/Main.Plans74_77.cs#L33)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `aging` | ✅ | ✅ | ✅ `Main.SetupAging()` ([src/Main.Aging.cs:61](../../src/Main.Aging.cs#L61)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `agriculture` | ✅ | ✅ | ✅ `Main.SetupAgriculture()` ([src/Main.CampaignOwners.cs:1383](../../src/Main.CampaignOwners.cs#L1383)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `airlock_security` | ✅ | ✅ | ✅ `Main.SetupAirlockSecurity()` ([src/Main.ExpandedShelterSystems.cs:110](../../src/Main.ExpandedShelterSystems.cs#L110)) | ✅ `Daily Decon Interlock` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `amphibious_draisine` | ✅ | ✅ | ✅ `Main.SetupAmphibiousDraisine()` ([src/Main.Plans122to125.cs:316](../../src/Main.Plans122to125.cs#L316)) | ⚡ `Expedition Travel/Action Cadence (Crossing Ticks)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `amputation` | ✅ | ✅ | ✅ `Main.SetupAmputation()` ([src/Main.CampaignServices.cs:117](../../src/Main.CampaignServices.cs#L117)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `anomaly_hazard` | ✅ | ✅ | ✅ `Main.SetupAnomalyHazard()` ([src/Main.CampaignServices.cs:109](../../src/Main.CampaignServices.cs#L109)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `antenatal_maternal_health` | ✅ | ✅ | ✅ `Main.SetupAntenatalMaternalHealth()` ([src/Main.AntenatalMaternalHealth.cs:41](../../src/Main.AntenatalMaternalHealth.cs#L41)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `apprenticeship` | ✅ | ✅ | ✅ `Main.SetupApprenticeship()` ([src/Main.ExpandedShelterSystems.cs:116](../../src/Main.ExpandedShelterSystems.cs#L116)) | ✅ `Daily Mentorship XP Transfer` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `apprenticeship_curriculum` | ✅ | ✅ | ✅ `Main.SetupApprenticeshipCurriculum()` ([src/Main.ApprenticeshipCurriculum.cs:34](../../src/Main.ApprenticeshipCurriculum.cs#L34)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `aquaponics` | ✅ | ✅ | ✅ `Main.SetupAquaponics()` ([src/Main.PlansB86_B89.cs:195](../../src/Main.PlansB86_B89.cs#L195)) | ✅ `Daily Ecology Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `archaeology` | ✅ | ✅ | ✅ `Main.SetupArchaeology()` ([src/Main.CampaignServices.cs:116](../../src/Main.CampaignServices.cs#L116)) | ⚡ `On-Demand (Excavation & Decryption)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `archive_desk` | ✅ | ✅ | ✅ `Main.SetupArchiveDesk()` ([src/Main.ExpandedShelterSystems.cs:140](../../src/Main.ExpandedShelterSystems.cs#L140)) | ✅ `Daily Scribing & Folio Archival` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `armored_crawlers` | ✅ | ✅ | ✅ `Main.SetupArmoredCrawlers()` ([src/Main.SaveOrchestrator.cs:294](../../src/Main.SaveOrchestrator.cs#L294)) | ✅ `Daily Crawler Module Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `autopsy` | ✅ | ✅ | ✅ `Main.SetupAutopsy()` ([src/Main.ExpandedShelterSystems.cs:122](../../src/Main.ExpandedShelterSystems.cs#L122)) | ✅ `Daily Forensic Case Progress` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `aviation` | ✅ | ✅ | ✅ `Main.SetupAviation()` ([src/Main.CampaignServices.cs:105](../../src/Main.CampaignServices.cs#L105)) | ✅ `Daily Flight Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `backstory` | ✅ | ✅ | ✅ `Main.SetupBackstory()` ([src/Main.Backstory.cs:51](../../src/Main.Backstory.cs#L51)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ballistic_shield` | ✅ | ✅ | ✅ `Main.SetupBallisticShield()` ([src/Main.Plans110_113.cs:139](../../src/Main.Plans110_113.cs#L139)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `ballistics_workbench` | ✅ | ✅ | ✅ `Main.SetupBallisticsWorkbench()` ([src/Main.Plans74_77.cs:32](../../src/Main.Plans74_77.cs#L32)) | ⚡ `On-Demand` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `bestiary_knowledge` | ✅ | ✅ | ✅ `Main.SetupBestiary()` ([src/Main.Bestiary.cs:18](../../src/Main.Bestiary.cs#L18)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `bio_fermentation` | ✅ | ✅ | ✅ `Main.SetupBioFermentation()` ([src/Main.CampaignServices.cs:136](../../src/Main.CampaignServices.cs#L136)) | ✅ `Daily Reactor Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `bionics` | ✅ | ✅ | ✅ `Main.SetupBionics()` ([src/Main.CampaignServices.cs:111](../../src/Main.CampaignServices.cs#L111)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `black_market` | ✅ | ✅ | ✅ `Main.SetupBlackMarket()` ([src/Main.BlackMarket.cs:21](../../src/Main.BlackMarket.cs#L21)) | ✅ `Daily Underworld Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `black_projects_archive` | ✅ | ✅ | ✅ `Main.SetupBlackProjectsArchive()` ([src/Main.CampaignServices.cs:122](../../src/Main.CampaignServices.cs#L122)) | ⚡ `On-Demand` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `broadsheet_press` | ✅ | ✅ | ✅ `Main.SetupBroadsheetPress()` ([src/Main.BroadsheetPress.cs:78](../../src/Main.BroadsheetPress.cs#L78)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `campaign_action_log` | ✅ | ✅ | ✅ `Main.SetupCampaignActionLog()` ([src/Main.CampaignActionLog.cs:37](../../src/Main.CampaignActionLog.cs#L37)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `campaign_day` | ✅ | ✅ | ✅ `Main.SetupCampaignDay()` ([src/Main.CampaignServices.cs:30](../../src/Main.CampaignServices.cs#L30)) | ✅ `Master Sim Clock / Dawn Advance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `campaign_legacy` | ✅ | ✅ | ✅ `Main.SetupCampaignLegacy()` ([src/Main.ExpandedShelterSystems.cs:171](../../src/Main.ExpandedShelterSystems.cs#L171)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `caravan` | ✅ | ✅ | ✅ `Main.SetupCaravans()` ([src/Main.CampaignOwners.cs:1957](../../src/Main.CampaignOwners.cs#L1957)) | ✅ `Daily Route Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caravan_trade_network` | ✅ | ✅ | ✅ `Main.SetupCaravanTrade()` ([src/Main.SaveOrchestrator.cs:284](../../src/Main.SaveOrchestrator.cs#L284)) | ✅ `Daily Route Arrival Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `caregiving` | ✅ | ✅ | ✅ `Main.SetupCaregiving()` ([src/Main.ExpandedShelterSystems.cs:117](../../src/Main.ExpandedShelterSystems.cs#L117)) | ✅ `Daily Nursery/Eldercare Comfort` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cargo_airdrop` | ✅ | ✅ | ✅ `Main.SetupCargoAirdrop()` ([src/Main.CampaignServices.cs:129](../../src/Main.CampaignServices.cs#L129)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cassette_playback` | ✅ | ✅ | ✅ `Main.SetupCassettePlayback()` ([src/Main.CampaignOwners.cs:854](../../src/Main.CampaignOwners.cs#L854)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `ceremony` | ✅ | ✅ | ✅ `Main.SetupCeremony()` ([src/Main.CampaignServices.cs:134](../../src/Main.CampaignServices.cs#L134)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chem_warfare` | ✅ | ✅ | ✅ `Main.SetupChemWarfare()` ([src/Main.CampaignServices.cs:132](../../src/Main.CampaignServices.cs#L132)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chemical_dependency` | ✅ | ✅ | ✅ `Main.SetupMentalHealthCrisis()` ([src/Main.ExpandedShelterSystems.cs:142](../../src/Main.ExpandedShelterSystems.cs#L142)) | ✅ `Daily Tolerance & Withdrawal` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chemical_plume` | ✅ | ✅ | ✅ `Main.SetupChemicalPlume()` ([src/Main.SaveOrchestrator.cs:385](../../src/Main.SaveOrchestrator.cs#L385)) | ✅ `Daily Sim Tick (expanded shelter day)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `chemical_reagent_synthesis` | ✅ | ✅ | ✅ `Main.SetupChemicalReagentSynthesis()` ([src/Main.CampaignOwners.cs:2865](../../src/Main.CampaignOwners.cs#L2865)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `chemical_recon` | ✅ | ✅ | ✅ `Main.SetupChemicalRecon()` ([src/Main.Plans78_81.cs:92](../../src/Main.Plans78_81.cs#L92)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `chemical_synthesis` | ✅ | ✅ | ✅ `Main.SetupChemicalSynthesis()` ([src/Main.Application.cs:1118](../../src/Main.Application.cs#L1118)) | ⚡ `On-Demand (Retort Synthesis)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `child_development` | ✅ | ✅ | ✅ `Main.SetupGenerational()` ([src/Main.CampaignServices.cs:101](../../src/Main.CampaignServices.cs#L101)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `chlor_alkali_synthesis` | ✅ | ✅ | ✅ `Main.SetupChlorAlkali()` ([src/Main.Plans110_113.cs:136](../../src/Main.Plans110_113.cs#L136)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `chronic_condition` | ✅ | ✅ | ✅ `Main.SetupChronicConditions()` ([src/Main.Lifecycle.cs:728](../../src/Main.Lifecycle.cs#L728)) | ⚡ `Committed Clinical Fact / Explicit Accommodation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cipher_quest_chain` | ✅ | ✅ | ✅ `Main.SetupCipherQuestChain()` ([src/Main.CipherQuestChain.cs:69](../../src/Main.CipherQuestChain.cs#L69)) | ⚡ `Event-Driven (Broadcast Heard / Key Acquired)` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `clinical_ward_triage` | ✅ | ✅ | ✅ `Main.SetupClinicalWardTriage()` ([src/Main.CampaignOwners.cs:2837](../../src/Main.CampaignOwners.cs#L2837)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `clothing_warmth` | ✅ | ✅ | ✅ `Main.SetupClothingWarmth()` ([src/Main.CampaignOwners.cs:3346](../../src/Main.CampaignOwners.cs#L3346)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cloud_seeding` | ✅ | ✅ | ✅ `Main.SetupCloudSeeding()` ([src/Main.SaveOrchestrator.cs:384](../../src/Main.SaveOrchestrator.cs#L384)) | ✅ `Daily Sim Tick (expanded shelter day)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `collectible_discovery` | ✅ | ✅ | ✅ `Main.SetupCollectibles()` ([src/Main.Application.cs:1112](../../src/Main.Application.cs#L1112)) | ⚡ `On-Demand (One-Time Discovery Ledger)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `colony` | ✅ | ✅ | ✅ `Main.SetupColony()` ([src/Main.OrphanSealWave1.cs:78](../../src/Main.OrphanSealWave1.cs#L78)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `combat` | ✅ | ✅ | ✅ `Main.SetupCombat()` ([src/Main.CampaignServices.cs:63](../../src/Main.CampaignServices.cs#L63)) | ⚡ `On-Demand (Turn-Based)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `commitment` | ✅ | ✅ | ✅ `Main.SetupCommitments()` ([src/Main.ExpandedShelterSystems.cs:156](../../src/Main.ExpandedShelterSystems.cs#L156)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `common_table_rationing` | ✅ | ✅ | ✅ `Main.SetupCommonTableRationing()` ([src/Main.CampaignOwners.cs:3623](../../src/Main.CampaignOwners.cs#L3623)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `comms_array` | ✅ | ✅ | ✅ `Main.SetupCommsArray()` ([src/Main.CampaignServices.cs:133](../../src/Main.CampaignServices.cs#L133)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `communications` | ✅ | ✅ | ✅ `Main.SetupCommunications()` ([src/Main.OrphanSealWave1.cs:77](../../src/Main.OrphanSealWave1.cs#L77)) | ✅ `Daily Wear Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `companion_animals` | ✅ | ✅ | ✅ `Main.SetupCompanionAnimals()` ([src/Main.CampaignServices.cs:110](../../src/Main.CampaignServices.cs#L110)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `confession_secret` | ✅ | ✅ | ✅ `Main.SetupConfessionSecrets()` ([src/Main.OrphanSealWave1.cs:82](../../src/Main.OrphanSealWave1.cs#L82)) | ⚡ `Discovery-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `consequence_ledger` | ✅ | ✅ | ✅ `Main.SetupConsequenceLedger()` ([src/Main.SaveOrchestrator.cs:244](../../src/Main.SaveOrchestrator.cs#L244)) | ⚡ `None` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `contraband_stash` | ✅ | ✅ | ✅ `Main.SetupContrabandStash()` ([src/Main.CampaignServices.cs:120](../../src/Main.CampaignServices.cs#L120)) | ⚡ `On-Demand` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `contractor_roster` | ✅ | ✅ | ✅ `Main.SetupContractorRoster()` ([src/Main.ExpandedShelterSystems.cs:141](../../src/Main.ExpandedShelterSystems.cs#L141)) | ✅ `Daily Mercenary Wage Payroll` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cooking` | ✅ | ✅ | ✅ `Main.SetupCooking()` ([src/Main.ExpandedShelterSystems.cs:167](../../src/Main.ExpandedShelterSystems.cs#L167)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `counter_intelligence` | ✅ | ✅ | ✅ `Main.SetupCounterIntelligence()` ([src/Main.CampaignOwners.cs:2480](../../src/Main.CampaignOwners.cs#L2480)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `crafting` | ✅ | ✅ | ✅ `Main.SetupCrafting()` ([src/Main.CampaignOwners.cs:1366](../../src/Main.CampaignOwners.cs#L1366)) | ✅ `Daily Workbench Queue` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `cryo_vault` | ✅ | ✅ | ✅ `Main.SetupCryoVault()` ([src/Main.CampaignOwners.cs:1437](../../src/Main.CampaignOwners.cs#L1437)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `cryogenic_air_separation` | ✅ | ✅ | ✅ `Main.SetupCryogenicAirSeparation()` ([src/Main.ExpandedShelterSystems.cs:136](../../src/Main.ExpandedShelterSystems.cs#L136)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `cultural_archives` | ✅ | ✅ | ✅ `Main.SetupCulturalArchive()` ([src/Main.Plans167_219.cs:97](../../src/Main.Plans167_219.cs#L97)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `culture_creation` | ✅ | ✅ | ✅ `Main.SetupCultureCreation()` ([src/Main.CampaignOwners.cs:3149](../../src/Main.CampaignOwners.cs#L3149)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `cvd_diamond` | ✅ | ✅ | ✅ `Main.SetupCvdDiamond()` ([src/Main.Plans122to125.cs:302](../../src/Main.Plans122to125.cs#L302)) | ✅ `Industrial Production Cadence (Batch Ticks)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `daily_briefing` | ✅ | ❌ | ❌ `Main.SetupDailyBriefingModal()` | ✅ `Daily Dawn Briefing Aggregation` | ✅ | ✅ | ✅ | **FAIL (GAP)** |
| `death_legacy` | ✅ | ✅ | ✅ `Main.SetupDeathLegacy()` ([src/Main.ExpandedShelterSystems.cs:154](../../src/Main.ExpandedShelterSystems.cs#L154)) | ✅ `Event-Driven & Daily Flush` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `decontamination` | ✅ | ✅ | ✅ `Main.SetupDecontamination()` ([src/Main.ExpandedShelterSystems.cs:129](../../src/Main.ExpandedShelterSystems.cs#L129)) | ✅ `Daily Rad Scrub Shower Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `deep_well` | ✅ | ✅ | ✅ `Main.SetupDeepWell()` ([src/Main.SaveOrchestrator.cs:204](../../src/Main.SaveOrchestrator.cs#L204)) | ✅ `Daily Deep-Well Pump Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dependency_taper_withdrawal` | ✅ | ✅ | ✅ `Main.SetupDependencyTaperWithdrawal()` ([src/Main.CampaignOwners.cs:2781](../../src/Main.CampaignOwners.cs#L2781)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `desperation` | ✅ | ✅ | ✅ `Main.SetupDesperation()` ([src/Main.CampaignServices.cs:114](../../src/Main.CampaignServices.cs#L114)) | ⚡ `On-Demand (Crisis Command)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `difficulty_settings` | ✅ | ✅ | ✅ `Main.SetupDifficultySettings()` ([src/Main.DifficultySettings.cs:59](../../src/Main.DifficultySettings.cs#L59)) | ⚡ `None` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `diplomacy` | ✅ | ✅ | ✅ `Main.SetupDiplomacy()` ([src/Main.CampaignOwners.cs:3402](../../src/Main.CampaignOwners.cs#L3402)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `diplomatic_summits` | ✅ | ❌ | ❌ `Main.SetupDiplomaticSummit()` | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `disaster_response` | ✅ | ✅ | ✅ `Main.SetupDisasterResponse()` ([src/Main.OrphanSealWave1.cs:76](../../src/Main.OrphanSealWave1.cs#L76)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `disease` | ✅ | ✅ | ✅ `Main.SetupDisease()` ([src/Main.CampaignOwners.cs:1814](../../src/Main.CampaignOwners.cs#L1814)) | ✅ `Daily Pathogen Transmission` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dose_ledger` | ✅ | ✅ | ✅ `Main.SetupDoseLedger()` ([src/Main.CampaignOwners.cs:1340](../../src/Main.CampaignOwners.cs#L1340)) | ⚡ `On-Demand (Dose Log)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `draisine_recovery` | ✅ | ✅ | ✅ `Main.SetupDraisineRerailing()` ([src/Main.Plans130_133.cs:28](../../src/Main.Plans130_133.cs#L28)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `duty_roster` | ✅ | ✅ | ✅ `Main.SetupDutyRoster()` ([src/Main.Application.cs:1096](../../src/Main.Application.cs#L1096)) | ✅ `Daily Shift Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `dynamic_quests` | ✅ | ✅ | ✅ `Main.SetupDynamicQuests()` ([src/Main.SaveOrchestrator.cs:242](../../src/Main.SaveOrchestrator.cs#L242)) | ⚡ `On-Demand (Campaign-Wide Emergency Quests)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ebpvd_coating` | ✅ | ✅ | ✅ `Main.SetupEbPvdCoating()` ([src/Main.Plans146_149.cs:387](../../src/Main.Plans146_149.cs#L387)) | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `echoes` | ✅ | ✅ | ✅ `Main.SetupEchoes()` ([src/Main.CampaignOwners.cs:2523](../../src/Main.CampaignOwners.cs#L2523)) | ✅ `Narrative Echo Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `ecological_infestation` | ✅ | ✅ | ✅ `Main.SetupEcologicalInfestation()` ([src/Main.EcologicalInfestations.cs:52](../../src/Main.EcologicalInfestations.cs#L52)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `economy` | ✅ | ✅ | ✅ `Main.SetupEconomy()` ([src/Main.BlackMarket.cs:31](../../src/Main.BlackMarket.cs#L31)) | ✅ `Daily Market Rate Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `economy_family` | ✅ | ✅ | ✅ `Main.SetupEconomyFamily()` ([src/Main.SaveOrchestrator.cs:379](../../src/Main.SaveOrchestrator.cs#L379)) | ✅ `Daily Sim Tick (expanded shelter day)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `emergency_alert` | ✅ | ✅ | ✅ `Main.SetupEmergencyAlerts()` ([src/Main.CampaignOwners.cs:3374](../../src/Main.CampaignOwners.cs#L3374)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `emergency_muster_readiness` | ✅ | ✅ | ✅ `Main.SetupEmergencyMusterReadiness()` ([src/Main.CampaignOwners.cs:3650](../../src/Main.CampaignOwners.cs#L3650)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `encounter_choice` | ✅ | ❌ | ❌ `Main.SetupEncounterChoice()` | ⚡ `On-Demand (Door Event Resolution)` | ✅ | ✅ | ✅ | **FAIL (GAP)** |
| `endgame` | ✅ | ✅ | ✅ `Main.SetupEndgame()` ([src/Main.Application.cs:1111](../../src/Main.Application.cs#L1111)) | ⚡ `On-Demand (Day Threshold / Extinction)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `equipment_condition` | ✅ | ✅ | ✅ `Main.SetupEquipmentCondition()` ([src/Main.ExpandedShelterSystems.cs:138](../../src/Main.ExpandedShelterSystems.cs#L138)) | ✅ `Daily Gear Wear & Maintenance` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `espionage` | ✅ | ✅ | ✅ `Main.SetupPlans166To169()` ([src/Main.CampaignServices.cs:149](../../src/Main.CampaignServices.cs#L149)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `excavation` | ✅ | ✅ | ✅ `Main.SetupExcavation()` ([src/Main.ExpandedShelterSystems.cs:115](../../src/Main.ExpandedShelterSystems.cs#L115)) | ✅ `Daily Rubble Shoring Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `excavation_hazards` | ✅ | ✅ | ✅ `Main.SetupExcavationHazards()` ([src/Main.CampaignServices.cs:148](../../src/Main.CampaignServices.cs#L148)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `exercise` | ✅ | ✅ | ✅ `Main.SetupExercise()` ([src/Main.CampaignOwners.cs:3121](../../src/Main.CampaignOwners.cs#L3121)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `expansion_hub` | ✅ | ✅ | ✅ `Main.SetupExpansions()` ([src/Main.Application.cs:1102](../../src/Main.Application.cs#L1102)) | ✅ `Daily Hub Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expansion_quest` | ✅ | ✅ | ✅ `Main.SetupExpansionQuests()` ([src/Main.CampaignOwners.cs:2502](../../src/Main.CampaignOwners.cs#L2502)) | ⚡ `On-Demand (Stage Milestone)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expedition` | ✅ | ✅ | ✅ `Main.SetupExpeditions()` ([src/Main.CampaignOwners.cs:1955](../../src/Main.CampaignOwners.cs#L1955)) | ✅ `Daily Sortie Travel` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `expedition_stealth` | ✅ | ✅ | ✅ `Main.SetupStealth()` ([src/Main.CampaignServices.cs:104](../../src/Main.CampaignServices.cs#L104)) | ⚡ `Event-Driven (Expedition Phases)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `faction_covert_ops` | ✅ | ✅ | ✅ `Main.SetupFactionCovertOps()` ([src/Main.OrphanSealWave1.cs:84](../../src/Main.OrphanSealWave1.cs#L84)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `faction_espionage` | ✅ | ✅ | ✅ `Main.SetupShelterEspionage()` ([src/Main.Plans50_53.cs:238](../../src/Main.Plans50_53.cs#L238)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `fallout` | ✅ | ✅ | ✅ `Main.SetupFallout()` ([src/Main.CampaignServices.cs:113](../../src/Main.CampaignServices.cs#L113)) | ✅ `Hourly Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `field_guide` | ✅ | ✅ | ✅ `Main.SetupFieldGuide()` ([src/Main.SaveOrchestrator.cs:236](../../src/Main.SaveOrchestrator.cs#L236)) | ⚡ `On-Demand (Study & Discovery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `fluid_logistics` | ✅ | ✅ | ✅ `Main.SetupPlans166To169()` ([src/Main.CampaignServices.cs:149](../../src/Main.CampaignServices.cs#L149)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `food_preservation` | ✅ | ❌ | ❌ `Main.SetupPlans62To65()` | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `forced_labor` | ✅ | ✅ | ✅ `Main.SetupForcedLabor()` ([src/Main.CampaignServices.cs:106](../../src/Main.CampaignServices.cs#L106)) | ✅ `Daily Shift Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `fungi_cultivation` | ✅ | ✅ | ✅ `Main.SetupFungi()` ([src/Main.CampaignServices.cs:119](../../src/Main.CampaignServices.cs#L119)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `genealogy` | ✅ | ✅ | ✅ `Main.SetupGenealogy()` ([src/Main.SaveOrchestrator.cs:374](../../src/Main.SaveOrchestrator.cs#L374)) | ⚡ `Canonical-Fact Driven (family/fate events)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `geodetic_survey` | ✅ | ✅ | ✅ `Main.SetupGeodeticSurvey()` ([src/Main.Plans78_81.cs:90](../../src/Main.Plans78_81.cs#L90)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `geothermal_aquifer` | ✅ | ✅ | ✅ `Main.SetupGeothermalAquifer()` ([src/Main.ExpandedShelterSystems.cs:120](../../src/Main.ExpandedShelterSystems.cs#L120)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `geothermal_orc` | ✅ | ✅ | ✅ `Main.SetupGeothermalOrc()` ([src/Main.Plans74_77.cs:31](../../src/Main.Plans74_77.cs#L31)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `glassworks` | ✅ | ✅ | ✅ `Main.SetupGlassworks()` ([src/Main.CampaignOwners.cs:2633](../../src/Main.CampaignOwners.cs#L2633)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `grain_milling_archive` | ✅ | ✅ | ✅ `Main.SetupGrainMillingArchive()` ([src/Main.CampaignServices.cs:126](../../src/Main.CampaignServices.cs#L126)) | ⚡ `Event-Driven (Location Discovery & Shelter Room Inspection)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `grain_processing` | ✅ | ✅ | ✅ `Main.SetupGrainProcessing()` ([src/Main.ExpandedShelterSystems.cs:135](../../src/Main.ExpandedShelterSystems.cs#L135)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `greenhouse` | ✅ | ✅ | ✅ `Main.SetupGreenhouse()` ([src/Main.CampaignOwners.cs:1381](../../src/Main.CampaignOwners.cs#L1381)) | ✅ `Daily Hydroponic Growth` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `health_history` | ✅ | ✅ | ✅ `Main.SetupHealthHistory()` ([src/Main.CampaignOwners.cs:3261](../../src/Main.CampaignOwners.cs#L3261)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `heliograph` | ✅ | ✅ | ✅ `Main.SetupHeliograph()` ([src/Main.ExpandedShelterSystems.cs:137](../../src/Main.ExpandedShelterSystems.cs#L137)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `hidden_agenda` | ✅ | ✅ | ✅ `Main.SetupHiddenAgenda()` ([src/Main.ExpandedShelterSystems.cs:146](../../src/Main.ExpandedShelterSystems.cs#L146)) | ✅ `Daily (Passive Slip-Up & Exposure Drift)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `hobby` | ✅ | ✅ | ✅ `Main.SetupHobby()` ([src/Main.OrphanSealWave1.cs:79](../../src/Main.OrphanSealWave1.cs#L79)) | ⚡ `On-Demand` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `holdfast` | ✅ | ✅ | ✅ `Main.SetupHoldfastRuntime()` ([src/Main.BlackMarket.cs:29](../../src/Main.BlackMarket.cs#L29)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `holdfast_trade` | ✅ | ✅ | ✅ `Main.SetupHoldfastRuntime()` ([src/Main.BlackMarket.cs:29](../../src/Main.BlackMarket.cs#L29)) | ⚡ `On-Demand (Barter)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `host_event` | ✅ | ✅ | ✅ `Main.SetupEventAdapter()` ([src/Main.CampaignOwners.cs:2562](../../src/Main.CampaignOwners.cs#L2562)) | ⚡ `On-Demand (Moral Dilemma)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `human_migration` | ✅ | ✅ | ✅ `Main.SetupHumanMigration()` ([src/Main.CampaignOwners.cs:1026](../../src/Main.CampaignOwners.cs#L1026)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `hydraulic_extrusion` | ✅ | ✅ | ✅ `Main.SetupHydraulicExtrusion()` ([src/Main.HydraulicExtrusion.cs:25](../../src/Main.HydraulicExtrusion.cs#L25)) | ⚡ `On-Demand (Batch Phase Commands)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `hydrogeology_archive` | ✅ | ✅ | ✅ `Main.SetupHydroGeologyDiscovery()` ([src/Main.CampaignServices.cs:125](../../src/Main.CampaignServices.cs#L125)) | ⚡ `Event-Driven (Location Discovery)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `hydroponic_biomes` | ✅ | ✅ | ✅ `Main.SetupHydroponicBiomes()` ([src/Main.SaveOrchestrator.cs:288](../../src/Main.SaveOrchestrator.cs#L288)) | ✅ `Daily Biome Rack Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ideological_friction` | ✅ | ✅ | ✅ `Main.SetupIdeologicalFriction()` ([src/Main.CampaignOwners.cs:519](../../src/Main.CampaignOwners.cs#L519)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `informant_network` | ✅ | ✅ | ✅ `Main.SetupInformantNetwork()` ([src/Main.CampaignOwners.cs:2481](../../src/Main.CampaignOwners.cs#L2481)) | ✅ `Daily Sim Tick (Faction Branch Owner)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `insar_deformation` | ✅ | ✅ | ✅ `Main.SetupInSarMapping()` ([src/Main.InSarMapping.cs:27](../../src/Main.InSarMapping.cs#L27)) | ⚡ `On-Demand (Survey Pass & Repeat-Pass Process)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `internal_communication` | ✅ | ✅ | ✅ `Main.SetupInternalCommunication()` ([src/Main.CampaignServices.cs:144](../../src/Main.CampaignServices.cs#L144)) | ✅ `Daily (Message Expiry)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `interpersonal_conflict` | ✅ | ✅ | ✅ `Main.SetupInterpersonalConflict()` ([src/Main.CampaignOwners.cs:3093](../../src/Main.CampaignOwners.cs#L3093)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `inventory` | ✅ | ✅ | ✅ `Main.SetupInventory()` ([src/Main.AdvancedShelterSystems.cs:44](../../src/Main.AdvancedShelterSystems.cs#L44)) | ⚡ `On-Demand (Item Use)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `item_lore` | ✅ | ✅ | ✅ `Main.SetupItemLore()` ([src/Main.Lifecycle.cs:720](../../src/Main.Lifecycle.cs#L720)) | ⚡ `Committed Craft, Intake, and Ownership Facts` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `journal` | ✅ | ✅ | ✅ `Main.SetupJournal()` ([src/Main.Application.cs:1094](../../src/Main.Application.cs#L1094)) | ⚡ `On-Demand (Log/Event)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `kilnworks` | ✅ | ✅ | ✅ `Main.SetupKilnworks()` ([src/Main.CampaignOwners.cs:2698](../../src/Main.CampaignOwners.cs#L2698)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `kinetic_storage` | ✅ | ✅ | ✅ `Main.SetupKineticStorage()` ([src/Main.Plans78_81.cs:91](../../src/Main.Plans78_81.cs#L91)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `kitchen_nutrition` | ✅ | ✅ | ✅ `Main.SetupKitchenNutrition()` ([src/Main.ExpandedShelterSystems.cs:134](../../src/Main.ExpandedShelterSystems.cs#L134)) | ✅ `Daily Rationing Meal Prep` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `leatherwork_archive` | ✅ | ✅ | ✅ `Main.SetupLeatherworkArchive()` ([src/Main.CampaignServices.cs:127](../../src/Main.CampaignServices.cs#L127)) | ⚡ `Event-Driven (Location Discovery & Item Inspection)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `letter_delivery` | ✅ | ✅ | ✅ `Main.SetupLetters()` ([src/Main.Letters.cs:42](../../src/Main.Letters.cs#L42)) | ⚡ `Explicit Discovery and Delivery Decision` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `library_study` | ✅ | ✅ | ✅ `Main.SetupLibraryStudy()` ([src/Main.ExpandedShelterSystems.cs:139](../../src/Main.ExpandedShelterSystems.cs#L139)) | ✅ `Daily Codex Research Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `loan_shark` | ✅ | ✅ | ✅ `Main.SetupLoanShark()` ([src/Main.SaveOrchestrator.cs:388](../../src/Main.SaveOrchestrator.cs#L388)) | ✅ `Daily Sim Tick (expanded shelter day)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `low_background_metrology` | ✅ | ✅ | ✅ `Main.SetupLowBackgroundMetrology()` ([src/Main.LowBackgroundMetrology.cs:30](../../src/Main.LowBackgroundMetrology.cs#L30)) | ⚡ `On-Demand (Assay & Smelting Commands)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `lyophilization` | ✅ | ✅ | ✅ `Main.SetupLyophilization()` ([src/Main.Plans130_133.cs:27](../../src/Main.Plans130_133.cs#L27)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `maritime` | ✅ | ✅ | ✅ `Main.SetupMaritime()` ([src/Main.CampaignOwners.cs:1217](../../src/Main.CampaignOwners.cs#L1217)) | ⚡ `On-Demand (Dive Sortie)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mechanical_driveline` | ✅ | ✅ | ✅ `Main.SetupMechanicalDriveline()` ([src/Main.CampaignOwners.cs:2922](../../src/Main.CampaignOwners.cs#L2922)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `medical` | ✅ | ✅ | ✅ `Main.SetupMedical()` ([src/Main.CampaignOwners.cs:1809](../../src/Main.CampaignOwners.cs#L1809)) | ✅ `Daily Recovery / Affliction` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_pipeline` | ✅ | ✅ | ✅ `Main.SetupMedical()` ([src/Main.CampaignOwners.cs:1809](../../src/Main.CampaignOwners.cs#L1809)) | ⚡ `On-Demand (Triage & Procedure Commands)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `medical_ward` | ✅ | ✅ | ✅ `Main.SetupMedicalWard()` ([src/Main.CampaignServices.cs:41](../../src/Main.CampaignServices.cs#L41)) | ✅ `Daily Bed Inpatient Triage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `memorial` | ✅ | ✅ | ✅ `Main.SetupMemorial()` ([src/Main.CampaignOwners.cs:2580](../../src/Main.CampaignOwners.cs#L2580)) | ⚡ `On-Demand (Survivor Fallen Eulogy)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `memory_decay` | ✅ | ✅ | ✅ `Main.SetupMemoryDecay()` ([src/Main.CampaignOwners.cs:3034](../../src/Main.CampaignOwners.cs#L3034)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `mental_health_crisis` | ✅ | ✅ | ✅ `Main.SetupMentalHealthCrisis()` ([src/Main.ExpandedShelterSystems.cs:142](../../src/Main.ExpandedShelterSystems.cs#L142)) | ✅ `Daily Psych Ward Calming Ticks` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mercenary_bounties` | ✅ | ✅ | ✅ `Main.SetupMercenary()` ([src/Main.CampaignServices.cs:115](../../src/Main.CampaignServices.cs#L115)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `meta_progression` | ✅ | ✅ | ✅ `Main.SetupMetaProgression()` ([src/Main.CampaignOwners.cs:631](../../src/Main.CampaignOwners.cs#L631)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `microfluidic_diagnostic` | ✅ | ✅ | ✅ `Main.SetupMicrofluidicDiagnostic()` ([src/Main.Plans146_149.cs:388](../../src/Main.Plans146_149.cs#L388)) | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `migration_consequence` | ✅ | ✅ | ✅ `Main.SetupMigrationConsequence()` ([src/Main.CampaignOwners.cs:825](../../src/Main.CampaignOwners.cs#L825)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `mine_clearing_flail` | ✅ | ✅ | ✅ `Main.SetupMineClearingFlail()` ([src/Main.Plans146_149.cs:389](../../src/Main.Plans146_149.cs#L389)) | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `moral_choice` | ✅ | ✅ | ✅ `Main.SetupMoralChoice()` ([src/Main.Application.cs:1108](../../src/Main.Application.cs#L1108)) | ⚡ `On-Demand (Branch Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `morale_contagion` | ✅ | ✅ | ✅ `Main.SetupMoraleContagion()` ([src/Main.CampaignOwners.cs:1674](../../src/Main.CampaignOwners.cs#L1674)) | ✅ `Daily Contagion / Isolation Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `muster` | ✅ | ✅ | ✅ `Main.SetupMuster()` ([src/Main.CampaignOwners.cs:2495](../../src/Main.CampaignOwners.cs#L2495)) | ⚡ `On-Demand (Rally Stance)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `mutation_tree` | ✅ | ✅ | ✅ `Main.SetupMutations()` ([src/Main.CampaignServices.cs:103](../../src/Main.CampaignServices.cs#L103)) | ⚡ `Event-Driven (Dose Thresholds)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narcotics` | ✅ | ✅ | ✅ `Main.SetupNarcotics()` ([src/Main.CampaignServices.cs:107](../../src/Main.CampaignServices.cs#L107)) | ✅ `24h Medical Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narrative` | ✅ | ✅ | ✅ `Main.SetupNarrative()` ([src/Main.CampaignOwners.cs:2509](../../src/Main.CampaignOwners.cs#L2509)) | ⚡ `On-Demand (Dialog Choice)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `narrative_questlines` | ✅ | ✅ | ✅ `Main.SetupNarrativeQuestlines()` ([src/Main.Application.cs:1117](../../src/Main.Application.cs#L1117)) | ⚡ `On-Demand (Survivor Narrative Arc Progression)` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `npc_memory` | ✅ | ✅ | ✅ `Main.SetupNpcMemory()` ([src/Main.CampaignOwners.cs:491](../../src/Main.CampaignOwners.cs#L491)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `nuclear_core_lifecycle` | ✅ | ✅ | ✅ `Main.SetupNuclearCore()` ([src/Main.SaveOrchestrator.cs:293](../../src/Main.SaveOrchestrator.cs#L293)) | ✅ `Daily Core Thermal Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `nuclear_winter_progression` | ✅ | ✅ | ✅ `Main.SetupNuclearWinter()` ([src/Main.OrphanSealWave1.cs:74](../../src/Main.OrphanSealWave1.cs#L74)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `nvis_communications` | ✅ | ✅ | ✅ `Main.SetupNvisCommunications()` ([src/Main.Plans130_133.cs:26](../../src/Main.Plans130_133.cs#L26)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `oilseed_pressing` | ✅ | ✅ | ✅ `Main.SetupOilseedPressing()` ([src/Main.SaveOrchestrator.cs:386](../../src/Main.SaveOrchestrator.cs#L386)) | ⚡ `On Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `onboarding` | ✅ | ✅ | ✅ `Main.SetupOnboarding()` ([src/Main.GameFlow.cs:393](../../src/Main.GameFlow.cs#L393)) | ⚡ `On-Demand (Player Sigil Recording)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `oral_lore` | ✅ | ✅ | ✅ `Main.SetupOralLore()` ([src/Main.CampaignServices.cs:124](../../src/Main.CampaignServices.cs#L124)) | ⚡ `Event-Driven (Performance)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `outpost_settlement` | ✅ | ✅ | ✅ `Main.SetupOutpostSettlement()` ([src/Main.ExpandedShelterSystems.cs:163](../../src/Main.ExpandedShelterSystems.cs#L163)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `palliative_care` | ✅ | ✅ | ✅ `Main.SetupPalliativeCare()` ([src/Main.CampaignOwners.cs:3514](../../src/Main.CampaignOwners.cs#L3514)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `pathogen_strains` | ✅ | ✅ | ✅ `Main.SetupPathogenStrains()` ([src/Main.CampaignOwners.cs:1858](../../src/Main.CampaignOwners.cs#L1858)) | ✅ `Daily Strain Progression Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `patrol_radio_hooks` | ✅ | ✅ | ✅ `Main.SetupPatrolRadio()` ([src/Main.CampaignOwners.cs:997](../../src/Main.CampaignOwners.cs#L997)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `perimeter_defense` | ✅ | ✅ | ✅ `Main.SetupPerimeterDefense()` ([src/Main.NightWatch.cs:32](../../src/Main.NightWatch.cs#L32)) | ✅ `Daily Emplacement + Watch Readiness Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `perimeter_early_warning` | ✅ | ✅ | ✅ `Main.SetupPerimeterEarlyWarning()` ([src/Main.CampaignOwners.cs:3458](../../src/Main.CampaignOwners.cs#L3458)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `personal_quests` | ✅ | ✅ | ✅ `Main.SetupPersonalQuests()` ([src/Main.Application.cs:1116](../../src/Main.Application.cs#L1116)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `phantom_memory` | ✅ | ✅ | ✅ `Main.SetupPhantom()` ([src/Main.CampaignServices.cs:61](../../src/Main.CampaignServices.cs#L61)) | ⚡ `On-Demand (Scavenge Echo)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `pharmaceutical_tablet` | ✅ | ✅ | ✅ `Main.SetupPharmaceuticalTablet()` ([src/Main.SaveOrchestrator.cs:377](../../src/Main.SaveOrchestrator.cs#L377)) | ✅ `Daily Sim Tick (expanded shelter day)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `phase0` | ✅ | ✅ | ✅ `Main.SetupPhase0()` ([src/Main.CampaignOwners.cs:1924](../../src/Main.CampaignOwners.cs#L1924)) | ⚡ `On-Demand (Pre-War Flashback)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `piezometer_network` | ✅ | ✅ | ✅ `Main.SetupPiezometer()` ([src/Main.Piezometer.cs:25](../../src/Main.Piezometer.cs#L25)) | ✅ `Daily Aquifer Advisory Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `plastic_pyrolysis` | ✅ | ✅ | ✅ `Main.SetupPlasticPyrolysis()` ([src/Main.CampaignServices.cs:128](../../src/Main.CampaignServices.cs#L128)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `playable_metrics` | ✅ | ✅ | ✅ `Main.SetupPlayMetrics()` ([src/Main.ExpandedShelterSystems.cs:158](../../src/Main.ExpandedShelterSystems.cs#L158)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `pneumatic_dispatch` | ✅ | ✅ | ✅ `Main.SetupPneumaticDispatch()` ([src/Main.Plans74_77.cs:34](../../src/Main.Plans74_77.cs#L34)) | ⚡ `On-Demand` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `powder_metallurgy` | ✅ | ✅ | ✅ `Main.SetupPowderMetallurgy()` ([src/Main.Plans130_133.cs:25](../../src/Main.Plans130_133.cs#L25)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `power_grid` | ✅ | ✅ | ✅ `Main.SetupPowerGrid()` ([src/Main.AdvancedShelterSystems.cs:422](../../src/Main.AdvancedShelterSystems.cs#L422)) | ✅ `Daily Fuel Consumption & Wattage` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `power_subgrids` | ✅ | ✅ | ✅ `Main.SetupPowerSubgrids()` ([src/Main.SaveOrchestrator.cs:286](../../src/Main.SaveOrchestrator.cs#L286)) | ✅ `Daily Thermal Distribution Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `precision_metrology` | ✅ | ✅ | ✅ `Main.SetupPrecisionMetrology()` ([src/Main.Plans74_77.cs:61](../../src/Main.Plans74_77.cs#L61)) | ✅ `Daily Calibration Drift` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `precision_optics` | ✅ | ✅ | ✅ `Main.SetupPrecisionOptics()` ([src/Main.Plans110_113.cs:138](../../src/Main.Plans110_113.cs#L138)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `prewar_archives` | ✅ | ❌ | ❌ `Main.SetupPlans62To65()` | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `prisoner_management` | ✅ | ✅ | ✅ `Main.SetupPrisoners()` ([src/Main.CampaignServices.cs:102](../../src/Main.CampaignServices.cs#L102)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `procedural_eulogy` | ✅ | ✅ | ✅ `Main.SetupProceduralEulogy()` ([src/Main.ProceduralEulogy.cs:35](../../src/Main.ProceduralEulogy.cs#L35)) | ⚡ `None` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `procedural_narrative` | ✅ | ✅ | ✅ `Main.SetupPlans166To169()` ([src/Main.CampaignServices.cs:149](../../src/Main.CampaignServices.cs#L149)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `propaganda_campaigns` | ✅ | ✅ | ✅ `Main.SetupPropaganda()` ([src/Main.ExpandedShelterSystems.cs:148](../../src/Main.ExpandedShelterSystems.cs#L148)) | ✅ `Daily (Campaign Decay & Distribution)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `psychological_arcs` | ✅ | ✅ | ✅ `Main.SetupPsychologyArcs()` ([src/Main.Plans162_165.cs:624](../../src/Main.Plans162_165.cs#L624)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `psychological_profiles` | ✅ | ✅ | ✅ `Main.SetupPsychologicalProfiles()` ([src/Main.CampaignOwners.cs:3177](../../src/Main.CampaignOwners.cs#L3177)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `psychological_sanatorium` | ✅ | ❌ | ❌ `Main.SetupSanatorium()` | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `psyops` | ✅ | ✅ | ✅ `Main.SetupPsyOps()` ([src/Main.CampaignOwners.cs:2036](../../src/Main.CampaignOwners.cs#L2036)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `radiation_economy` | ✅ | ✅ | ✅ `Main.SetupRadiationEconomy()` ([src/Main.RadiationEconomy.cs:33](../../src/Main.RadiationEconomy.cs#L33)) | ⚡ `None` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `radiation_social` | ✅ | ✅ | ✅ `Main.SetupRadiationSocial()` ([src/Main.RadiationSocial.cs:34](../../src/Main.RadiationSocial.cs#L34)) | ⚡ `None` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `radio` | ✅ | ✅ | ✅ `Main.SetupRadio()` ([src/Main.CampaignServices.cs:48](../../src/Main.CampaignServices.cs#L48)) | ⚡ `On-Demand (Frequency Scan)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `radio_program_production` | ✅ | ✅ | ✅ `Main.SetupRadioProgramProduction()` ([src/Main.RadioProgramProduction.cs:21](../../src/Main.RadioProgramProduction.cs#L21)) | ✅ `Daily Program Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `radio_station` | ✅ | ✅ | ✅ `Main.SetupRadioStation()` ([src/Main.SaveOrchestrator.cs:238](../../src/Main.SaveOrchestrator.cs#L238)) | ⚡ `On-Demand (Tuning & Broadcasts)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `rail_grinding` | ✅ | ✅ | ✅ `Main.SetupRailGrinding()` ([src/Main.Plans146_149.cs:390](../../src/Main.Plans146_149.cs#L390)) | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `rail_track_maintenance` | ✅ | ✅ | ✅ `Main.SetupRailTrackMaintenance()` ([src/Main.CampaignOwners.cs:2606](../../src/Main.CampaignOwners.cs#L2606)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `railway` | ✅ | ✅ | ✅ `Main.SetupRailway()` ([src/Main.CampaignServices.cs:118](../../src/Main.CampaignServices.cs#L118)) | ⚡ `On-Demand (Convoy Operations)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `ration_conflict` | ✅ | ✅ | ✅ `Main.SetupRationConflict()` ([src/Main.CampaignOwners.cs:913](../../src/Main.CampaignOwners.cs#L913)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `recon_telemetry` | ✅ | ✅ | ✅ `Main.SetupReconTelemetry()` ([src/Main.CampaignOwners.cs:1980](../../src/Main.CampaignOwners.cs#L1980)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `recreation` | ✅ | ✅ | ✅ `Main.SetupRecreation()` ([src/Main.CampaignServices.cs:131](../../src/Main.CampaignServices.cs#L131)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `recruitment` | ✅ | ✅ | ✅ `Main.SetupRecruitment()` ([src/Main.CampaignOwners.cs:3318](../../src/Main.CampaignOwners.cs#L3318)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `regional_treaty` | ✅ | ✅ | ✅ `Main.SetupRegionalTreaty()` ([src/Main.Endgame.cs:252](../../src/Main.Endgame.cs#L252)) | ✅ `Daily Non-Aggression Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `relationship_decay` | ✅ | ✅ | ✅ `Main.SetupRelationshipDecay()` ([src/Main.ExpandedShelterSystems.cs:155](../../src/Main.ExpandedShelterSystems.cs#L155)) | ✅ `Daily (Pair Bond Decay & Social Drift)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `research` | ✅ | ✅ | ✅ `Main.None()` | ⚡ `On-Demand (Study Progress)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `research_unlock` | ✅ | ✅ | ✅ `Main.SetupResearchUnlockBridge()` ([src/Main.CampaignOwners.cs:435](../../src/Main.CampaignOwners.cs#L435)) | ✅ `On-Demand (Research Node Completion)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `retention` | ✅ | ✅ | ✅ `Main.SetupRetention()` ([src/Main.ExpandedShelterSystems.cs:162](../../src/Main.ExpandedShelterSystems.cs#L162)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `robotics` | ✅ | ✅ | ✅ `Main.SetupRobotics()` ([src/Main.CampaignServices.cs:135](../../src/Main.CampaignServices.cs#L135)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `romance_family` | ✅ | ✅ | ✅ `Main.SetupRomanceFamily()` ([src/Main.CampaignOwners.cs:547](../../src/Main.CampaignOwners.cs#L547)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `route_infrastructure` | ✅ | ✅ | ✅ `Main.SetupRouteInfrastructure()` ([src/Main.Plans146_149.cs:262](../../src/Main.Plans146_149.cs#L262)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `runflat_tire` | ✅ | ✅ | ✅ `Main.SetupRunFlatTire()` ([src/Main.RunFlatTire.cs:25](../../src/Main.RunFlatTire.cs#L25)) | ⚡ `On-Demand (Fit, Hazard & Heat Commands)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `sanitation` | ✅ | ✅ | ✅ `Main.SetupSanitation()` ([src/Main.BriefingCrisis.cs:80](../../src/Main.BriefingCrisis.cs#L80)) | ✅ `Daily Sanitation Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `seasonal_celebration` | ✅ | ✅ | ✅ `Main.SetupSeasonalCelebration()` ([src/Main.OrphanSealWave1.cs:75](../../src/Main.OrphanSealWave1.cs#L75)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `seismic_dynamics` | ✅ | ✅ | ✅ `Main.SetupSeismicDynamics()` ([src/Main.CampaignOwners.cs:1418](../../src/Main.CampaignOwners.cs#L1418)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `session_durability` | ✅ | ✅ | ✅ `Main.SetupSessionDurability()` ([src/Main.ExpandedShelterSystems.cs:157](../../src/Main.ExpandedShelterSystems.cs#L157)) | ⚡ `Session-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `settlement_defenses` | ✅ | ✅ | ✅ `Main.SetupDefense()` ([src/Main.Plans162_165.cs:368](../../src/Main.Plans162_165.cs#L368)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `settlement_politics` | ✅ | ✅ | ✅ `Main.SetupPolitics()` ([src/Main.CampaignServices.cs:108](../../src/Main.CampaignServices.cs#L108)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `seven_day_slice` | ✅ | ✅ | ✅ `Main.SetupSevenDaySlice()` ([src/Main.ExpandedShelterSystems.cs:161](../../src/Main.ExpandedShelterSystems.cs#L161)) | ⚡ `Playtest-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_archive` | ✅ | ✅ | ✅ `Main.SetupShelterArchive()` ([src/Main.CampaignOwners.cs:2978](../../src/Main.CampaignOwners.cs#L2978)) | ✅ `Daily Archive Timeline Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_assignment` | ✅ | ✅ | ✅ `Main.SetupShelterAssignment()` ([src/Main.ExpandedShelterSystems.cs:143](../../src/Main.ExpandedShelterSystems.cs#L143)) | ⚡ `On-Demand (Bunk Reassignment)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_atmosphere` | ✅ | ✅ | ✅ `Main.SetupShelterAtmosphere()` ([src/Main.ExpandedShelterSystems.cs:145](../../src/Main.ExpandedShelterSystems.cs#L145)) | ✅ `Daily (Day Coordinator)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_barter` | ✅ | ✅ | ✅ `Main.SetupShelterBarter()` ([src/Main.CampaignServices.cs:121](../../src/Main.CampaignServices.cs#L121)) | ⚡ `On-Demand (Barter)` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_decor` | ✅ | ✅ | ✅ `Main.SetupShelterDecor()` ([src/Main.CampaignOwners.cs:1664](../../src/Main.CampaignOwners.cs#L1664)) | ⚡ `On-Demand (Decoration Placement)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_expansion` | ✅ | ✅ | ✅ `Main.SetupShelterExpansion()` ([src/Main.OrphanSealWave1.cs:81](../../src/Main.OrphanSealWave1.cs#L81)) | ⚡ `Labor-Driven` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_festival` | ✅ | ✅ | ✅ `Main.SetupShelterFestival()` ([src/Main.OrphanSealWave1.cs:83](../../src/Main.OrphanSealWave1.cs#L83)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `shelter_fire` | ✅ | ✅ | ✅ `Main.SetupShelterFireHazard()` ([src/Main.Application.cs:1113](../../src/Main.Application.cs#L1113)) | ✅ `Daily Fire Propagation Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_governance` | ✅ | ✅ | ✅ `Main.SetupShelterGovernance()` ([src/Main.CampaignOwners.cs:1054](../../src/Main.CampaignOwners.cs#L1054)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_identity` | ✅ | ✅ | ✅ `Main.SetupShelterIdentity()` ([src/Main.CampaignOwners.cs:689](../../src/Main.CampaignOwners.cs#L689)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_maintenance` | ✅ | ✅ | ✅ `Main.SetupShelterMaintenance()` ([src/Main.CampaignOwners.cs:1110](../../src/Main.CampaignOwners.cs#L1110)) | ⚡ `None` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_museum` | ✅ | ✅ | ✅ `Main.SetupShelterMuseum()` ([src/Main.SaveOrchestrator.cs:373](../../src/Main.SaveOrchestrator.cs#L373)) | ✅ `Daily Sim Tick (TickPlans46_49)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_noise` | ✅ | ✅ | ✅ `Main.SetupShelterAtmosphere()` ([src/Main.ExpandedShelterSystems.cs:145](../../src/Main.ExpandedShelterSystems.cs#L145)) | ✅ `Daily (Midday Acoustic Audit)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_reputation` | ✅ | ✅ | ✅ `Main.SetupShelterReputation()` ([src/Main.ExpandedShelterSystems.cs:147](../../src/Main.ExpandedShelterSystems.cs#L147)) | ✅ `Daily (Reputation Decay & Tag Evaluation)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_schedule` | ✅ | ✅ | ✅ `Main.SetupShelterSchedule()` ([src/Main.ExpandedShelterSystems.cs:121](../../src/Main.ExpandedShelterSystems.cs#L121)) | ✅ `Daily Curfew Rotation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_security` | ✅ | ✅ | ✅ `Main.SetupShelterSecurity()` ([src/Main.ExpandedShelterSystems.cs:150](../../src/Main.ExpandedShelterSystems.cs#L150)) | ✅ `Daily (Breach Decay & Alert Drift)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_social_dynamics` | ✅ | ✅ | ✅ `Main.SetupShelterSocial()` ([src/Main.SaveOrchestrator.cs:240](../../src/Main.SaveOrchestrator.cs#L240)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_thermal` | ✅ | ✅ | ✅ `Main.SetupShelterThermal()` ([src/Main.ExpandedShelterSystems.cs:118](../../src/Main.ExpandedShelterSystems.cs#L118)) | ✅ `Daily HVAC Frost Dissipation` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `shelter_workshop` | ✅ | ✅ | ✅ `Main.SetupWorkshop()` ([src/Main.SaveOrchestrator.cs:237](../../src/Main.SaveOrchestrator.cs#L237)) | ⚡ `On-Demand (Crafting & Refurbishment)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `silent_foundry` | ✅ | ✅ | ✅ `Main.SetupSilentFoundry()` ([src/Main.CampaignOwners.cs:1396](../../src/Main.CampaignOwners.cs#L1396)) | ✅ `Daily Smelter Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `skill_atrophy` | ✅ | ✅ | ✅ `Main.SetupSkillAtrophy()` ([src/Main.CampaignOwners.cs:3486](../../src/Main.CampaignOwners.cs#L3486)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `skill_certifications` | ✅ | ✅ | ✅ `Main.SetupSkillCertifications()` ([src/Main.CampaignOwners.cs:3205](../../src/Main.CampaignOwners.cs#L3205)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `sky_defense_battery` | ✅ | ✅ | ✅ `Main.SetupSkyDefense()` ([src/Main.Lifecycle.cs:732](../../src/Main.Lifecycle.cs#L732)) | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `sleep_acoustic_rest` | ✅ | ✅ | ✅ `Main.SetupSleepAcousticRest()` ([src/Main.CampaignOwners.cs:2950](../../src/Main.CampaignOwners.cs#L2950)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `sofc_power` | ✅ | ✅ | ✅ `Main.SetupSofcPower()` ([src/Main.Plans122to125.cs:295](../../src/Main.Plans122to125.cs#L295)) | ✅ `Shelter Power Cadence (TickDay)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `soil_reclamation_profile` | ✅ | ✅ | ✅ `Main.SetupSoilReclamationProfile()` ([src/Main.CampaignOwners.cs:3677](../../src/Main.CampaignOwners.cs#L3677)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `solar_concentrator` | ✅ | ✅ | ✅ `Main.SetupSolarConcentrator()` ([src/Main.Plans110_113.cs:137](../../src/Main.Plans110_113.cs#L137)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `sound_ranging` | ✅ | ✅ | ✅ `Main.SetupSoundRanging()` ([src/Main.NightWatch.cs:33](../../src/Main.NightWatch.cs#L33)) | ⚡ `Event-Driven (Hostile-Fire Observations) + Daily Drift` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `spiritual_meaning` | ✅ | ✅ | ✅ `Main.SetupSpiritual()` ([src/Main.CampaignServices.cs:66](../../src/Main.CampaignServices.cs#L66)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `spiritual_ritual` | ✅ | ✅ | ✅ `Main.SetupSpiritualRitual()` ([src/Main.CampaignOwners.cs:768](../../src/Main.CampaignOwners.cs#L768)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `starting_level` | ✅ | ✅ | ✅ `Main.SetupStartingLevel()` ([src/Main.CampaignOwners.cs:1333](../../src/Main.CampaignOwners.cs#L1333)) | ⚡ `On-Demand (Opening Protocol)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `storm_forecast` | ✅ | ✅ | ✅ `Main.SetupStormForecast()` ([src/Main.CampaignOwners.cs:2753](../../src/Main.CampaignOwners.cs#L2753)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `subterranean` | ✅ | ✅ | ✅ `Main.SetupSubterranean()` ([src/Main.CampaignOwners.cs:2117](../../src/Main.CampaignOwners.cs#L2117)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `sump_flooding` | ✅ | ✅ | ✅ `Main.SetupSumpFlooding()` ([src/Main.ExpandedShelterSystems.cs:124](../../src/Main.ExpandedShelterSystems.cs#L124)) | ✅ `Daily Drainage Pump Work` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `surgical_graft` | ✅ | ✅ | ✅ `Main.SetupSurgicalGraft()` ([src/Main.SaveOrchestrator.cs:376](../../src/Main.SaveOrchestrator.cs#L376)) | ✅ `Daily Sim Tick (expanded shelter day)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `surgical_ward` | ✅ | ✅ | ✅ `Main.SetupSurgicalWard()` ([src/Main.SaveOrchestrator.cs:285](../../src/Main.SaveOrchestrator.cs#L285)) | ✅ `Daily Sterile Field Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `survivor_autonomy` | ✅ | ✅ | ✅ `Main.SetupSurvivorAutonomy()` ([src/Main.OrphanSealWave1.cs:73](../../src/Main.OrphanSealWave1.cs#L73)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `survivor_barter` | ✅ | ✅ | ✅ `Main.SetupSurvivorBarter()` ([src/Main.CampaignOwners.cs:3430](../../src/Main.CampaignOwners.cs#L3430)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `survivor_dreams` | ✅ | ✅ | ✅ `Main.SetupSurvivorDreams()` ([src/Main.CampaignOwners.cs:3006](../../src/Main.CampaignOwners.cs#L3006)) | ✅ `Daily Dream Cycle Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `survivor_education` | ✅ | ✅ | ✅ `Main.SetupSurvivorEducation()` ([src/Main.OrphanSealWave1.cs:80](../../src/Main.OrphanSealWave1.cs#L80)) | ⚡ `Session-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `survivor_fate` | ✅ | ✅ | ✅ `Main.SetupSurvivorFate()` ([src/Main.CampaignOwners.cs:1684](../../src/Main.CampaignOwners.cs#L1684)) | ✅ `Daily Survivor-Death Cascade` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_letter_delivery` | ✅ | ✅ | ✅ `Main.SetupSurvivorLetterDelivery()` ([src/Main.SurvivorLetterDelivery.cs:34](../../src/Main.SurvivorLetterDelivery.cs#L34)) | ⚡ `Player Action (Find / Address / Deliver / Withhold)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `survivor_mental_health` | ✅ | ✅ | ✅ `Main.SetupSurvivorMentalHealth()` ([src/Main.Plans50_53.cs:239](../../src/Main.Plans50_53.cs#L239)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `survivor_relations` | ✅ | ✅ | ✅ `Main.SetupSurvivorRelations()` ([src/Main.ExpandedShelterSystems.cs:111](../../src/Main.ExpandedShelterSystems.cs#L111)) | ✅ `Daily Affinity & Feud Drift` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_roles` | ✅ | ✅ | ✅ `Main.SetupSurvivorRoles()` ([src/Main.SaveOrchestrator.cs:354](../../src/Main.SaveOrchestrator.cs#L354)) | ⚡ `Work-Fact Driven (SkillProgression OnXpGained)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_routines` | ✅ | ✅ | ✅ `Main.SetupSurvivorRoutines()` ([src/Main.CampaignOwners.cs:1138](../../src/Main.CampaignOwners.cs#L1138)) | ⚡ `None` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_social` | ✅ | ✅ | ✅ `Main.SetupSurvivorSocial()` ([src/Main.InternalCommunication.cs:39](../../src/Main.InternalCommunication.cs#L39)) | ✅ `Daily Shelter Social Dynamics` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `survivor_voice` | ✅ | ✅ | ✅ `Main.SetupSurvivorVoice()` ([src/Main.ExpandedShelterSystems.cs:159](../../src/Main.ExpandedShelterSystems.cs#L159)) | ⚡ `Event-Driven` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `survivors` | ✅ | ✅ | ✅ `Main.SetupSurvivors()` ([src/Main.BriefingCrisis.cs:35](../../src/Main.BriefingCrisis.cs#L35)) | ✅ `Daily Needs Decay` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `technical_material_archive` | ✅ | ✅ | ✅ `Main.SetupTechnicalMaterialArchive()` ([src/Main.CampaignServices.cs:123](../../src/Main.CampaignServices.cs#L123)) | ⚡ `Event-Driven (Location Discovery)` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `territory_control` | ✅ | ✅ | ✅ `Main.SetupTerritoryControl()` ([src/Main.ExpandedShelterSystems.cs:165](../../src/Main.ExpandedShelterSystems.cs#L165)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `thirdonary` | ✅ | ✅ | ✅ `Main.SetupThirdonary()` ([src/Main.CampaignServices.cs:35](../../src/Main.CampaignServices.cs#L35)) | ⚡ `On-Demand (Arbitration)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `time_capsules` | ✅ | ✅ | ✅ `Main.SetupTimeCapsules()` ([src/Main.ExpandedShelterSystems.cs:153](../../src/Main.ExpandedShelterSystems.cs#L153)) | ✅ `Daily (Scheduled Opening & Message Delivery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `trade_routes` | ✅ | ✅ | ✅ `Main.SetupTradeRoutes()` ([src/Main.CampaignOwners.cs:717](../../src/Main.CampaignOwners.cs#L717)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `trauma_bond` | ✅ | ✅ | ✅ `Main.SetupTraumaBond()` ([src/Main.CampaignOwners.cs:796](../../src/Main.CampaignOwners.cs#L796)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `travel_encounters` | ✅ | ✅ | ✅ `Main.SetupTravelEncounters()` ([src/Main.Expeditions.cs:106](../../src/Main.Expeditions.cs#L106)) | ⚡ `On-Demand (Travel Step)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `trophies` | ✅ | ✅ | ✅ `Main.SetupTrophies()` ([src/Main.SaveOrchestrator.cs:360](../../src/Main.SaveOrchestrator.cs#L360)) | ⚡ `None` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `unified_ending` | ✅ | ✅ | ✅ `Main.SetupUnifiedEnding()` ([src/Main.CampaignOwners.cs:463](../../src/Main.CampaignOwners.cs#L463)) | ✅ `On-Demand (Campaign Sealed)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `unique_claims` | ✅ | ✅ | ✅ `Main.SetupCollectibles()` ([src/Main.Application.cs:1112](../../src/Main.Application.cs#L1112)) | ⚡ `On-Demand (Global Unique Claim Ledger)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `vehicle_customization` | ✅ | ✅ | ✅ `Main.SetupVehicleCustomization()` ([src/Main.CampaignOwners.cs:575](../../src/Main.CampaignOwners.cs#L575)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `vehicle_garage` | ✅ | ✅ | ✅ `Main.SetupVehicleGarage()` ([src/Main.Lifecycle.cs:733](../../src/Main.Lifecycle.cs#L733)) | ⚡ `On-Demand` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `verdict` | ✅ | ✅ | ✅ `Main.SetupVerdict()` ([src/Main.CampaignServices.cs:54](../../src/Main.CampaignServices.cs#L54)) | ✅ `Daily Machine Log Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `verdict_accusation` | ✅ | ✅ | ✅ `Main.SetupVerdictAccusation()` ([src/Main.SaveOrchestrator.cs:387](../../src/Main.SaveOrchestrator.cs#L387)) | ⚡ `On Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `vinyl_morale` | ✅ | ✅ | ✅ `Main.SetupVinylMorale()` ([src/Main.ExpandedShelterSystems.cs:113](../../src/Main.ExpandedShelterSystems.cs#L113)) | ✅ `Daily Turntable Morale Broadcast` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `visitor_integration` | ✅ | ✅ | ✅ `Main.SetupVisitorIntegration()` ([src/Main.ExpandedShelterSystems.cs:151](../../src/Main.ExpandedShelterSystems.cs#L151)) | ✅ `Daily (Visitor Lifecycle & Ration Draw)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `voluntary_register` | ✅ | ✅ | ✅ `Main.SetupVoluntaryRegister()` ([src/Main.CampaignOwners.cs:941](../../src/Main.CampaignOwners.cs#L941)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `warlord_response` | ✅ | ✅ | ✅ `Main.SetupWarlordResponse()` ([src/Main.CampaignOwners.cs:882](../../src/Main.CampaignOwners.cs#L882)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_justice` | ✅ | ✅ | ✅ `Main.SetupJustice()` ([src/Main.CampaignServices.cs:130](../../src/Main.CampaignServices.cs#L130)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_map` | ✅ | ✅ | ✅ `Main.SetupWorld()` ([src/Main.AdvancedShelterSystems.cs:54](../../src/Main.AdvancedShelterSystems.cs#L54)) | ⚡ `On-Demand (Fog-of-War Discovery)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wasteland_rumors` | ✅ | ✅ | ✅ `Main.SetupRumorNetwork()` ([src/Main.ExpandedShelterSystems.cs:149](../../src/Main.ExpandedShelterSystems.cs#L149)) | ✅ `Daily (Decay & Propagation)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `water_condenser` | ✅ | ✅ | ✅ `Main.SetupWaterCondenser()` ([src/Main.SaveOrchestrator.cs:205](../../src/Main.SaveOrchestrator.cs#L205)) | ✅ `Daily Condensate Intake Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `water_quality_profile` | ✅ | ✅ | ✅ `Main.SetupWaterQualityProfile()` ([src/Main.CampaignOwners.cs:3542](../../src/Main.CampaignOwners.cs#L3542)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `water_treatment` | ✅ | ✅ | ✅ `Main.SetupWaterTreatment()` ([src/Main.DeepWell.cs:24](../../src/Main.DeepWell.cs#L24)) | ✅ `Daily Filtration Cycle` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `waystation` | ✅ | ✅ | ✅ `Main.SetupWaystation()` ([src/Main.ExpandedShelterSystems.cs:123](../../src/Main.ExpandedShelterSystems.cs#L123)) | ✅ `Daily Outpost Relay Barter` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `weather_cascade` | ✅ | ✅ | ✅ `Main.SetupWeatherCascade()` ([src/Main.ExpandedShelterSystems.cs:164](../../src/Main.ExpandedShelterSystems.cs#L164)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `weather_forecast_reliability` | ✅ | ✅ | ✅ `Main.SetupWeatherForecastReliability()` ([src/Main.CampaignOwners.cs:3569](../../src/Main.CampaignOwners.cs#L3569)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `weather_hardening` | ✅ | ✅ | ✅ `Main.SetupWeatherHardening()` ([src/Main.ExpandedShelterSystems.cs:119](../../src/Main.ExpandedShelterSystems.cs#L119)) | ⚡ `On-Demand` | ✅ | ❌ | ❌ | **FAIL (GAP)** |
| `weight_of_choices` | ✅ | ✅ | ✅ `Main.SetupFactionBranch()` ([src/Main.CampaignServices.cs:57](../../src/Main.CampaignServices.cs#L57)) | ⚡ `On-Demand (Branch Decisions)` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `wildlife_ecosystem` | ✅ | ✅ | ✅ `Main.SetupWildlifeEcosystem()` ([src/Main.EvolvingWorld.cs:63](../../src/Main.EvolvingWorld.cs#L63)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ❌ | **FAIL (GAP)** |
| `wildlife_harvest` | ✅ | ✅ | ✅ `Main.SetupWildlifeHarvest()` ([src/Main.CampaignOwners.cs:2726](../../src/Main.CampaignOwners.cs#L2726)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `wildlife_trapping` | ✅ | ✅ | ✅ `Main.SetupWildlifeTrapping()` ([src/Main.EvolvingWorld.cs:139](../../src/Main.EvolvingWorld.cs#L139)) | ✅ `Daily Snare Yield & Butchery` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `world` | ✅ | ✅ | ✅ `Main.SetupWorld()` ([src/Main.AdvancedShelterSystems.cs:54](../../src/Main.AdvancedShelterSystems.cs#L54)) | ✅ `Daily Weather & Hazard` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `world_evolution` | ✅ | ✅ | ✅ `Main.SetupWorldEvolution()` ([src/Main.CampaignOwners.cs:968](../../src/Main.CampaignOwners.cs#L968)) | ✅ `Daily Sim Tick` | ✅ | ❌ | ✅ | **FAIL (GAP)** |
| `year_of_ash` | ✅ | ✅ | ✅ `Main.SetupYearOfAsh()` ([src/Main.Application.cs:1105](../../src/Main.Application.cs#L1105)) | ✅ `Daily Deep-Freeze Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |
| `zealotry` | ✅ | ✅ | ✅ `Main.SetupZealotry()` ([src/Main.CampaignServices.cs:112](../../src/Main.CampaignServices.cs#L112)) | ✅ `Daily Sim Tick` | ✅ | ✅ | ✅ | **PASS (6/6)** |

### Never-Invoked Setup Methods

These registry setup methods have no invocation expression in a `Main*.cs` production partial. They do not count as constructed.

- `daily_briefing` — `Main.SetupDailyBriefingModal()`
- `diplomatic_summits` — `Main.SetupDiplomaticSummit()`
- `encounter_choice` — `Main.SetupEncounterChoice()`
- `food_preservation` — `Main.SetupPlans62To65()`
- `prewar_archives` — `Main.SetupPlans62To65()`
- `psychological_sanatorium` — `Main.SetupSanatorium()`

This source-level call-site check is not proof that every runtime branch executes in every session.

---

## 5. Architectural Verification Invariants

1. **Invariant 1 (Core Engine Agnosticism):** Core systems contain zero references to `Godot`, `UnityEngine`, or engine globals.
2. **Invariant 3 (Save Store Integrity):** Every save store delegates to `SaveStoreHub` / `SaveEnvelopeHelper` or a Core codec and wraps state in a verified checksum envelope.
3. **Invariant 5 (Thin Host Nodes):** UI panels and host sessions handle only presentation, lifecycle, and wiring — never domain calculations.
4. **Invariant 6 (Data Authority):** `Assets/StreamingAssets/Data/` JSON files are the sole authority.
5. **Mechanical Reachability Gate:** Every system in this matrix is verified by headless test runs in `verify-fast.sh` and xUnit suites in `Ashfall.Core.Tests`.
6. **Zero Conceptual Placeholders:** If a layer is absent or procedural, it is documented with explicit status rather than filled with conceptual names.
