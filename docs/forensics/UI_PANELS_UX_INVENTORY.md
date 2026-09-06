# UI panels and route inventory — 2026-09-05

Companion to [the forensic report](UI_PANELS_UX_FORENSIC_REPORT.md). This is an as-observed working-tree inventory, not a generated completion certificate.

## Scope and interpretation

- 178 source files defining a class ending in `Panel`, plus the four expansion classes `AviationUI`, `ChemUI`, `LaborUI`, `PoliticsUI`, across `src/` (test classes excluded). The total **includes**, rather than adds, those four UI-named classes.
- The initial sweep found 174; the concurrently added Plans 146–149 panels bring the final inventory to 178. Changes already present or made concurrently belong to the user and were not edited by this audit.
- 141 registry descriptors: 112 Live and 29 Prototype. 134 unique configured action IDs: 111 registered Live and 23 unregistered. These categories overlap panel classes; do not add them as a defect total.
- Modal/HUD/component classes with other names are outside this naming-based panel count, but their routing/lifecycle, shared components and 22 scene contracts were inspected separately. This count is not the total number of all UI C# files, scenes, or screens.
- Bind signature is source evidence only. A typed field, Main reference, registry entry or passing constructor does not prove working actions. “No targeted defect assigned” below means no confirmed family finding in this sweep, **not** fully verified/complete.
- “Main references” include field/construction/helper code and are not proof of a reachable player entry. The new Plans 146–149 helper also contains tests; read UI-14 for the actual path.

## Every inventoried panel source

| Class / source | Lines | Public Bind signature(s) | Observed shape | Audit disposition |
|---|---:|---|---|---|
| [AchievementsPanel](../../src/UI/AchievementsPanel.cs) | 182 | `SurvivorsHostSession? survivors, int simDay = 1` | custom presentation | No targeted defect assigned; not certified complete |
| [AfflictionsPanel](../../src/UI/AfflictionsPanel.cs) | 309 | `MedicalHostSession? medical = null, SurvivorsHostSession? survivors = null, InventoryHostSession? inventory = null, RespiratoryDegenerationSystem? respiratory = null` | custom presentation | No targeted defect assigned; not certified complete |
| [AirlockSecurityPanel](../../src/UI/AirlockSecurityPanel.cs) | 141 | `AirlockSecurityHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [AmputationTriagePanel](../../src/UI/AmputationTriagePanel.cs) | 33 | `AmputationSystem system` | shell, EMPTY refresh | UI-07 |
| [AnaerobicBiogasDigesterPanel](../../src/UI/AnaerobicBiogasDigesterPanel.cs) | 130 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [ApprenticeshipPanel](../../src/UI/ApprenticeshipPanel.cs) | 112 | `ApprenticeshipHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [AquiferTreatyConcessionPanel](../../src/UI/AquiferTreatyConcessionPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [ArchaeologyExcavationPanel](../../src/UI/ArchaeologyExcavationPanel.cs) | 33 | `ArchaeologySystem system` | shell, EMPTY refresh | UI-07 |
| [ArchiveDeskPanel](../../src/UI/ArchiveDeskPanel.cs) | 302 | `ArchiveDeskHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [AutopsyReportPanel](../../src/UI/AutopsyReportPanel.cs) | 116 | `AutopsyHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [AviationUI](../../src/UI/AviationUI.cs) | 127 | `AviationSystem system` | shell, status rail | UI-08 |
| [BasalRadonMigrationPanel](../../src/UI/BasalRadonMigrationPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [BoreholeSeismographPanel](../../src/UI/BoreholeSeismographPanel.cs) | 126 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [BrineExtractionPanel](../../src/UI/BrineExtractionPanel.cs) | 235 | `SilentFoundryHostSession foundryHost` | custom presentation | No targeted defect assigned; not certified complete |
| [CaravanBarterLedgerPanel](../../src/UI/CaravanBarterLedgerPanel.cs) | 261 | `EconomyHostSession session, IFactionStanceProvider? stanceProvider = null, IPriceShockProvider? priceShockProvider = null, IFactionRadioProvider? radioProvider = null, ISeededRng? rng = null` | shell, status rail | No targeted defect assigned; not certified complete |
| [CaregivingPanel](../../src/UI/CaregivingPanel.cs) | 127 | `CaregivingHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [CenturySeedPanel](../../src/UI/CenturySeedPanel.cs) | 266 | `GenerationalSuccessionEngine? succession, SurvivorsHostSession? survivors` | scroll | No targeted defect assigned; not certified complete |
| [CeremonyFestivalPanel](../../src/UI/CeremonyFestivalPanel.cs) | 33 | `CeremonySystem system` | shell, EMPTY refresh | UI-07 |
| [ChemicalDependencyPanel](../../src/UI/ChemicalDependencyPanel.cs) | 412 | `ChemicalDependencyHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [ChemicalLabPanel](../../src/UI/ChemicalLabPanel.cs) | 477 | `ChemicalSynthesisHostSession? host`<br>`object? session` | shell, status rail, scroll | UI-13 |
| [ChemicalReconPanel](../../src/UI/ChemicalReconPanel.cs) | 396 | `ChemicalReconHostSession session` | shell, status rail, data grid | UI-11, UI-12 |
| [ChemUI](../../src/UI/ChemUI.cs) | 126 | `NarcoticsSystem system` | shell, status rail | UI-08 |
| [ChemWarfareDefensePanel](../../src/UI/ChemWarfareDefensePanel.cs) | 33 | `ChemWarfareSystem system` | shell, EMPTY refresh | UI-07 |
| [ChroniclePanel](../../src/UI/ChroniclePanel.cs) | 192 | `EndgameHostSession? host`<br>`EndgameSystem? system` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [ClandestineInsurgencyPanel](../../src/UI/ClandestineInsurgencyPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [CombatDetailPanel](../../src/UI/CombatDetailPanel.cs) | 159 | `CombatHostSession combat` | custom presentation | No targeted defect assigned; not certified complete |
| [CombatHistoryPanel](../../src/UI/CombatHistoryPanel.cs) | 197 | `CombatHostSession combat` | scroll | No targeted defect assigned; not certified complete |
| [CombatPanel](../../src/UI/CombatPanel.cs) | 463 | `CombatHostSession combat` | data grid, scroll | No targeted defect assigned; not certified complete |
| [CommsArrayTransceiverPanel](../../src/UI/CommsArrayTransceiverPanel.cs) | 33 | `CommsArraySystem system` | shell, EMPTY refresh | UI-07 |
| [ContractorRosterPanel](../../src/UI/ContractorRosterPanel.cs) | 330 | `ContractorRosterHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [CraftingPanel](../../src/UI/CraftingPanel.cs) | 325 | `CraftingHostSession crafting, InventoryHostSession? inventory = null` | custom presentation | No targeted defect assigned; not certified complete |
| [CrossingQuestPanel](../../src/UI/CrossingQuestPanel.cs) | 493 | `ExpansionHostSession expansions, VouchAccessSystem? vouch, int currentDay` | scroll | No targeted defect assigned; not certified complete |
| [CrossingSafeConductVouchPanel](../../src/UI/CrossingSafeConductVouchPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [CryogenicPermafrostCorePanel](../../src/UI/CryogenicPermafrostCorePanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [DeconAirlockPanel](../../src/UI/DeconAirlockPanel.cs) | 399 | `DecontaminationHostSession session` | shell, status rail, data grid | UI-11, UI-12 |
| [DecontaminationPanel](../../src/UI/DecontaminationPanel.cs) | 347 | `DecontaminationHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [DeepCoastPanel](../../src/UI/DeepCoastPanel.cs) | 338 | `DeepCoastHostSession? deepCoast, CoreDemoSession? core` | scroll | UI-16 |
| [DesperationCrisisPanel](../../src/UI/DesperationCrisisPanel.cs) | 146 | `DesperationSystem system` | shell, status rail | UI-08 |
| [DoseLedgerPanel](../../src/UI/DoseLedgerPanel.cs) | 452 | `DoseLedgerHostSession session, SurvivorsHostSession? survivors = null` | shell, status rail, data grid | UI-19 |
| [DutyRosterDetailPanel](../../src/UI/DutyRosterDetailPanel.cs) | 164 | `DutyRosterHostSession roster` | custom presentation | No targeted defect assigned; not certified complete |
| [DutyRosterPanel](../../src/UI/DutyRosterPanel.cs) | 536 | `DutyRosterHostSession host, SurvivorsHostSession? survivors = null` | shell, status rail, data grid | UI-19 |
| [EbPvdCoatingPanel](../../src/UI/EbPvdCoatingPanel.cs) | 261 | `EbPvdCoatingHostSession session` | shell, status rail, data grid | UI-14 |
| [EconomyDetailPanel](../../src/UI/EconomyDetailPanel.cs) | 156 | `EconomyHostSession? economy` | custom presentation | No targeted defect assigned; not certified complete |
| [EconomyMarketPanel](../../src/Economy/EconomyMarketPanel.cs) | 181 | No public Bind found | scroll | No targeted defect assigned; not certified complete |
| [ElectrostaticScrubberPanel](../../src/UI/ElectrostaticScrubberPanel.cs) | 320 | `object? session` | custom presentation | UI-13 |
| [EpiloguePanel](../../src/UI/EpiloguePanel.cs) | 257 | `CampaignOutcomeSnapshot snapshot`<br>`EpilogueEvaluationContext context`<br>`int daysSurvived, int livingCount, int deathsCount, bool grandTreaty, bool tempestDecom, bool ledgersBurned, bool childrenAlive, bool velExposed` | scroll | No targeted defect assigned; not certified complete |
| [EquipmentConditionPanel](../../src/UI/EquipmentConditionPanel.cs) | 314 | `EquipmentConditionHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [EventDetailPanel](../../src/UI/EventDetailPanel.cs) | 134 | `EventsHostSession? events` | custom presentation | No targeted defect assigned; not certified complete |
| [EventsLogPanel](../../src/UI/EventsLogPanel.cs) | 199 | `object events` | custom presentation | No targeted defect assigned; not certified complete |
| [ExcavationPanel](../../src/UI/ExcavationPanel.cs) | 106 | `ExcavationHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [ExpansionsHubPanel](../../src/UI/ExpansionsHubPanel.cs) | 468 | `ExpansionHostSession? expansions, GreenhouseHostSession? greenhouse, DutyRosterHostSession? dutyRoster, MusterHostSession? muster, MaritimeHostSession? maritime, DeepCoastHostSession? deepCoast, WorldHostSession? world, MedicalHostSession? medical, VerdictHostSession? verdict, int currentDay` | scroll | No targeted defect assigned; not certified complete |
| [ExpeditionCampPanel](../../src/UI/ExpeditionCampPanel.cs) | 251 | `ExpeditionHostSession expeditionHost, string survivorId` | custom presentation | No targeted defect assigned; not certified complete |
| [ExpeditionPanel](../../src/UI/ExpeditionPanel.cs) | 1094 | `ExpeditionHostSession expeditionHost, SurvivorsHostSession? survivorsHost = null, InventoryHostSession? inventoryHost = null, Ashfall.Core.EquipmentConditionSystem? equipment = null, WorldHostSession? world = null` | scroll | No targeted defect assigned; not certified complete |
| [ExpeditionRadarPanel](../../src/UI/ExpeditionRadarPanel.cs) | 602 | `ExpeditionHostSession expeditions, SurvivorsHostSession? survivors = null` | shell, status rail, data grid | UI-18, UI-19 |
| [FactionActionPanel](../../src/Muster/FactionActionPanel.cs) | 180 | `FactionActionBoard board` | scroll | No targeted defect assigned; not certified complete |
| [FactionDetailPanel](../../src/UI/FactionDetailPanel.cs) | 155 | `HoldfastFactionEntry faction, HoldfastTradeSession? trade = null, MusterHostSession? muster = null, ExpansionHostSession? expansions = null`<br>`object faction` | custom presentation | No targeted defect assigned; not certified complete |
| [FactionMatrixPanel](../../src/UI/FactionMatrixPanel.cs) | 481 | `IFactionStanceProvider stanceProvider` | shell, status rail, data grid | UI-19 |
| [FactionRadioHudPanel](../../src/Radio/FactionRadioHudPanel.cs) | 389 | No public Bind found | scroll | No targeted defect assigned; not certified complete |
| [FactionsNarrativePanel](../../src/UI/FactionsNarrativePanel.cs) | 500 | `IFactionStanceProvider stance` | shell, status rail, data grid | UI-18, UI-19 |
| [FactionsPanel](../../src/UI/FactionsPanel.cs) | 513 | `HoldfastFactionsCatalog? factions, HoldfastTradeSession? trade = null, MusterHostSession? muster = null, ExpansionHostSession? expansions = null, YearOfAshHostSession? yearOfAsh = null, Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null, Ashfall.Core.MoralChoice.MoralChoiceSystem? moralChoice = null` | scroll | No targeted defect assigned; not certified complete |
| [FalloutPlumePanel](../../src/UI/FalloutPlumePanel.cs) | 121 | `FalloutSystem system` | shell, status rail | UI-08 |
| [FireIncidentPanel](../../src/UI/FireIncidentPanel.cs) | 321 | `ShelterFireHostSession session, string? incidentId = null`<br>`ShelterFireHazardSystem fireSystem, string? incidentId = null` | custom presentation | No targeted defect assigned; not certified complete |
| [FungalProteinFermenterPanel](../../src/UI/FungalProteinFermenterPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [FungiCultivationBedPanel](../../src/UI/FungiCultivationBedPanel.cs) | 33 | `FungiCultivationSystem system` | shell, EMPTY refresh | UI-07 |
| [GameDashboardPanel](../../src/UI/GameDashboardPanel.cs) | 675 | No public Bind found | custom presentation | UI-23 |
| [GameOverPanel](../../src/UI/GameOverPanel.cs) | 118 | No public Bind found | custom presentation | No targeted defect assigned; not certified complete |
| [GeigerCalibrationPanel](../../src/UI/GeigerCalibrationPanel.cs) | 261 | `DoseLedgerHostSession doseHost, string deviceTag = "tag_1"` | custom presentation | No targeted defect assigned; not certified complete |
| [GeodeticSurveyPanel](../../src/UI/GeodeticSurveyPanel.cs) | 397 | `GeodeticSurveyHostSession session` | shell, status rail, data grid | UI-11, UI-12 |
| [GeothermalAquiferPanel](../../src/UI/GeothermalAquiferPanel.cs) | 172 | `GeothermalAquiferHostSession session` | custom presentation | UI-13 |
| [GeothermalSteamTurbinePanel](../../src/UI/GeothermalSteamTurbinePanel.cs) | 130 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [GreenhousePanel](../../src/UI/GreenhousePanel.cs) | 743 | `GreenhouseHostSession session` | shell, status rail, data grid | UI-19, UI-20 |
| [HeavyLogisticsAirlockPanel](../../src/UI/HeavyLogisticsAirlockPanel.cs) | 126 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [HeavyMarineDieselGeneratorPanel](../../src/UI/HeavyMarineDieselGeneratorPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [HoldfastTerminalPanel](../../src/Host/HoldfastTerminalPanel.cs) | 937 | No public Bind found | custom presentation | No targeted defect assigned; not certified complete |
| [InductionCupolaFurnacePanel](../../src/UI/InductionCupolaFurnacePanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [InventoryDetailPanel](../../src/UI/InventoryDetailPanel.cs) | 189 | `InventoryHostSession? inventory, string itemId` | custom presentation | No targeted defect assigned; not certified complete |
| [InventoryPanel](../../src/UI/InventoryPanel.cs) | 282 | `InventoryHostSession inventory` | shell, status rail | No targeted defect assigned; not certified complete |
| [IronCenotaphMemorialPanel](../../src/UI/IronCenotaphMemorialPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [IsotopeSeparatorPanel](../../src/UI/IsotopeSeparatorPanel.cs) | 130 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [JournalDetailPanel](../../src/UI/JournalDetailPanel.cs) | 134 | `JournalSystem? journal` | custom presentation | No targeted defect assigned; not certified complete |
| [JournalPanel](../../src/UI/JournalPanel.cs) | 486 | `JournalHostSession session`<br>`JournalSystem journal` | shell, scroll | No targeted defect assigned; not certified complete |
| [JournalWitnessPanel](../../src/Muster/JournalWitnessPanel.cs) | 91 | `List<WitnessDefinition> witnesses` | scroll | No targeted defect assigned; not certified complete |
| [JusticeTribunalPanel](../../src/UI/JusticeTribunalPanel.cs) | 33 | `JusticeSystem system` | shell, EMPTY refresh | UI-07 |
| [KineticStoragePanel](../../src/UI/KineticStoragePanel.cs) | 388 | `KineticStorageHostSession session` | shell, status rail, data grid | UI-11, UI-12 |
| [KitchenNutritionPanel](../../src/UI/KitchenNutritionPanel.cs) | 312 | `KitchenNutritionHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [LaborUI](../../src/UI/LaborUI.cs) | 132 | `ForcedLaborSystem system` | shell, status rail | UI-08 |
| [LibraryStudyPanel](../../src/UI/LibraryStudyPanel.cs) | 347 | `LibraryStudyHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [LongWalkExpeditionPanel](../../src/UI/LongWalkExpeditionPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [MagneticDrumArchivePanel](../../src/UI/MagneticDrumArchivePanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [MainMenuPanel](../../src/UI/MainMenuPanel.cs) | 186 | No public Bind found | custom presentation | No targeted defect assigned; not certified complete |
| [MapAtlasPanel](../../src/UI/MapAtlasPanel.cs) | 519 | `ExpeditionHostSession host` | shell, status rail, data grid | UI-02, UI-04, UI-19 |
| [MapDetailPanel](../../src/UI/MapDetailPanel.cs) | 189 | `string locationId, string displayName, string region, float dangerLevel, float baseRadsPerHour, float travelHours, string description, string inspectNotes = "", List<string>? subLayouts = null, List<string>? lootCategories = null`<br>`HoldfastLocationEntry? holdfastLoc, LocationDefinitionData? journalLoc = null` | custom presentation | No targeted defect assigned; not certified complete |
| [MapPanel](../../src/UI/MapPanel.cs) | 505 | `CoreDemoSession? core, ExpeditionHostSession? expeditions = null, ExpansionHostSession? expansions = null, WorldHostSession? world = null, JournalCatalogs? catalogs = null, DeepCoastHostSession? deepCoast = null, YearOfAshHostSession? yearOfAsh = null` | scroll | No targeted defect assigned; not certified complete |
| [MaritimeAtlasPanel](../../src/UI/MaritimeAtlasPanel.cs) | 467 | `MaritimeHostSession host` | shell, status rail, data grid | UI-02, UI-03, UI-19 |
| [MaritimePanel](../../src/UI/MaritimePanel.cs) | 315 | `MaritimeHostSession? maritime, SurvivorsHostSession? survivors` | scroll | No targeted defect assigned; not certified complete |
| [MechanicalProstheticsLathePanel](../../src/UI/MechanicalProstheticsLathePanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [MedicalPanel](../../src/UI/MedicalPanel.cs) | 808 | `MedicalHostSession medical, SurvivorsHostSession? survivors = null, InventoryHostSession? inventory = null, RespiratoryDegenerationSystem? respiratory = null` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [MedicalWardPanel](../../src/UI/MedicalWardPanel.cs) | 465 | `MedicalWardHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [MentalHealthCrisisPanel](../../src/UI/MentalHealthCrisisPanel.cs) | 301 | `MentalHealthCrisisHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [MercenaryBountyBoardPanel](../../src/UI/MercenaryBountyBoardPanel.cs) | 59 | `MercenarySystem system` | shell, status rail | UI-08 |
| [MicrofluidicDiagnosticPanel](../../src/UI/MicrofluidicDiagnosticPanel.cs) | 257 | `MicrofluidicDiagnosticHostSession session` | shell, status rail, data grid | UI-14 |
| [MineFlailPanel](../../src/UI/MineFlailPanel.cs) | 234 | `MineClearingFlailHostSession session` | shell, status rail, data grid | UI-14 |
| [MusterAtlasPanel](../../src/UI/MusterAtlasPanel.cs) | 517 | `MusterHostSession host` | shell, status rail, data grid | UI-01, UI-02 |
| [MusterPanel](../../src/UI/MusterPanel.cs) | 388 | `MusterHostSession muster, int currentDay` | scroll | No targeted defect assigned; not certified complete |
| [MutationTreePanel](../../src/UI/MutationTreePanel.cs) | 112 | `MutationSystem system` | shell, status rail | UI-08 |
| [NurseryPanel](../../src/UI/NurseryPanel.cs) | 116 | `GenerationalSystem system` | shell, status rail | UI-08 |
| [OnboardingHintPanel](../../src/UI/OnboardingHintPanel.cs) | 357 | `OnboardingJourney journey` | scroll | No targeted defect assigned; not certified complete |
| [PhantomMemoryPanel](../../src/UI/PhantomMemoryPanel.cs) | 334 | `PhantomMemoryHostSession session, InventoryHostSession? inventory = null` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [PharmaLabPanel](../../src/UI/PharmaLabPanel.cs) | 376 | `PharmaLabSystem pharma, Ashfall.Core.Inventory.Inventory inventory, ChemicalDependencySystem? chemicalDependency = null, SurvivorsHostSession? survivors = null` | scroll | UI-16 |
| [Phase0Panel](../../src/UI/Phase0Panel.cs) | 303 | `Phase0HostSession phase0, SurvivorsHostSession? survivors = null, MedicalPipelineCoordinator? pipeline = null` | scroll | UI-15, UI-16 |
| [Plans130To133Panel](../../src/UI/Plans130To133Panel.cs) | 343 | `PowderMetallurgyHostSession powder, NvisCommunicationsHostSession nvis, LyophilizationHostSession lyophilization, DraisineRerailingHostSession draisine, RailwaySystem railway, ExpeditionHostSession? expeditions, Func<int>? dayProvider, Action<string>? acknowledgeRecall` | shell | UI-15 |
| [Plans94To97Panel](../../src/UI/Plans94To97Panel.cs) | 256 | `GrainProcessingHostSession grain, CryogenicAirSeparationHostSession cryogenic, HeliographHostSession heliograph, Func<int>? dayProvider = null` | shell | UI-15 |
| [PlasmaArcSmeltingPanel](../../src/UI/PlasmaArcSmeltingPanel.cs) | 126 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [PoliticsUI](../../src/UI/PoliticsUI.cs) | 127 | `PoliticsSystem system` | shell, status rail | UI-08 |
| [PowerGridPanel](../../src/UI/PowerGridPanel.cs) | 209 | `PowerGridHostSession session` | scroll | No targeted defect assigned; not certified complete |
| [PrisonerPanel](../../src/UI/PrisonerPanel.cs) | 131 | `PrisonerSystem system` | shell, status rail | UI-08 |
| [QuestDetailPanel](../../src/UI/QuestDetailPanel.cs) | 333 | `string questId, string displayName, string type, string briefing, int currentStage, List<string> stages, List<string>? choices = null, string? targetLocation = null, string? rewards = null, bool isCompleted = false`<br>`HoldfastQuestEntry? holdfastDef, HoldfastQuestProgress? progress = null`<br>`CrossingQuestDef? crossingDef, CrossingQuestProgress? progress = null`<br>`MoralChoiceQuestDefinition? moralDef, MoralChoiceSystem? moralChoice = null, Action<string, int>? onChoiceSelected = null` | custom presentation | No targeted defect assigned; not certified complete |
| [QuestsAtlasPanel](../../src/UI/QuestsAtlasPanel.cs) | 431 | `HoldfastQuestSystem holdfast, CrossingQuestSystem? crossing = null` | shell, status rail, data grid | UI-02, UI-03, UI-19 |
| [QuestsPanel](../../src/UI/QuestsPanel.cs) | 529 | `HoldfastQuestSystem? holdfastQuests, CrossingQuestSystem? crossingQuests = null, DutyRosterHostSession? dutyRoster = null, int currentDay = 1, Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null, Ashfall.Core.MoralChoice.MoralChoiceSystem? moralChoice = null, IReadOnlyList<Ashfall.Core.MoralChoice.MoralChoiceQuestDefinition>? moralDefs = null` | scroll | No targeted defect assigned; not certified complete |
| [RadiationDetailPanel](../../src/UI/RadiationDetailPanel.cs) | 263 | `DoseLedgerHostSession? dose = null, SurvivorsHostSession? survivors = null` | custom presentation | No targeted defect assigned; not certified complete |
| [RadiationHistoryPanel](../../src/UI/RadiationHistoryPanel.cs) | 183 | `DoseLedgerHostSession? dose` | custom presentation | No targeted defect assigned; not certified complete |
| [RadioPanel](../../src/UI/RadioPanel.cs) | 420 | `RadioHostSession radio` | shell, status rail, data grid | No targeted defect assigned; not certified complete |
| [RailGrindingPanel](../../src/UI/RailGrindingPanel.cs) | 242 | `RailGrindingHostSession session` | shell, status rail, data grid | UI-14 |
| [RailwayTerminalPanel](../../src/UI/RailwayTerminalPanel.cs) | 33 | `RailwaySystem system` | shell, EMPTY refresh | UI-07 |
| [RegionalTreatyPanel](../../src/UI/RegionalTreatyPanel.cs) | 160 | `RegionalTreatyHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [ResearchAtlasPanel](../../src/UI/ResearchAtlasPanel.cs) | 560 | `ResearchHostSession host` | shell, status rail, data grid | UI-01, UI-02 |
| [ResearchPanel](../../src/UI/ResearchPanel.cs) | 203 | `ResearchSystem? research` | custom presentation | No targeted defect assigned; not certified complete |
| [RoboticsWorkshopPanel](../../src/UI/RoboticsWorkshopPanel.cs) | 33 | `RoboticsSystem system` | shell, EMPTY refresh | UI-07 |
| [SaveLoadPanel](../../src/UI/SaveLoadPanel.cs) | 402 | `SaveLoadHostSession session` | custom presentation | No targeted defect assigned; not certified complete |
| [SettingsPanel](../../src/UI/SettingsPanel.cs) | 475 | No public Bind found | scroll | No targeted defect assigned; not certified complete |
| [ShelterDecorPanel](../../src/UI/ShelterDecorPanel.cs) | 401 | `ShelterDecorHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [ShelterHudPanel](../../src/UI/ShelterHudPanel.cs) | 610 | No public Bind found | shell, status rail, data grid | No targeted defect assigned; not certified complete |
| [ShelterPanel](../../src/UI/ShelterPanel.cs) | 429 | `SurvivorsHostSession survivors, WorldHostSession world, InventoryHostSession? inventory = null, ShelterRoomIdentityCatalog? roomIdentities = null` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [ShelterSchedulePanel](../../src/UI/ShelterSchedulePanel.cs) | 126 | `ShelterScheduleHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [ShelterThermalPanel](../../src/UI/ShelterThermalPanel.cs) | 125 | `ShelterThermalHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [SilentFoundryPanel](../../src/UI/SilentFoundryPanel.cs) | 507 | `SilentFoundryHostSession session, int currentDay` | shell, status rail, data grid | UI-19 |
| [SiliconIngotSlicingPanel](../../src/UI/SiliconIngotSlicingPanel.cs) | 126 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [SkillMatrixPanel](../../src/UI/SkillMatrixPanel.cs) | 499 | `SkillProgressionSystem skills, SurvivorsHostSession? survivors = null` | shell, status rail, data grid | UI-18, UI-19 |
| [SlurryDewateringSumpPanel](../../src/UI/SlurryDewateringSumpPanel.cs) | 362 | `object? session` | custom presentation | No targeted defect assigned; not certified complete |
| [SonicRuptureDrillPanel](../../src/UI/SonicRuptureDrillPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [StandingRecordAtlasPanel](../../src/UI/StandingRecordAtlasPanel.cs) | 516 | `StandingRecordHostSession host` | shell, status rail, data grid | UI-01, UI-02 |
| [StandingRecordPanel](../../src/UI/StandingRecordPanel.cs) | 344 | `LocationLayoutSystem? layoutSystem` | scroll | No targeted defect assigned; not certified complete |
| [StatusPanel](../../src/UI/StatusPanel.cs) | 361 | `SurvivorsHostSession? survivors = null, WeatherSystem? weather = null, PowerGridHostSession? power = null, InventoryHostSession? inventory = null, int simDay = 1` | scroll | No targeted defect assigned; not certified complete |
| [StealthReadoutPanel](../../src/UI/StealthReadoutPanel.cs) | 114 | `StealthSystem system` | shell, status rail | UI-08 |
| [SubterraneanCartographyPanel](../../src/UI/SubterraneanCartographyPanel.cs) | 126 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [SubterraneanDebtLedgerPanel](../../src/UI/SubterraneanDebtLedgerPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [SumpFloodingPanel](../../src/UI/SumpFloodingPanel.cs) | 344 | `SumpFloodingHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [SurfaceShrapnelAegisPanel](../../src/UI/SurfaceShrapnelAegisPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [SurvivalDetailPanel](../../src/UI/SurvivalDetailPanel.cs) | 140 | `SurvivorsHostSession? survivors` | custom presentation | No targeted defect assigned; not certified complete |
| [SurvivalWorkstationPanel](../../src/UI/SurvivalWorkstationPanel.cs) | 597 | `CraftingHostSession crafting, InventoryHostSession? inventory = null` | shell, status rail, data grid | UI-19 |
| [SurvivorDetailPanel](../../src/UI/SurvivorDetailPanel.cs) | 167 | `SurvivorsHostSession? survivors, string survivorId` | custom presentation | No targeted defect assigned; not certified complete |
| [SurvivorDowntimePanel](../../src/UI/SurvivorDowntimePanel.cs) | 33 | `SurvivorDowntimeSystem system` | shell, EMPTY refresh | UI-07 |
| [SurvivorRelationsPanel](../../src/UI/SurvivorRelationsPanel.cs) | 183 | `SurvivorRelationsHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [SurvivorsPanel](../../src/UI/SurvivorsPanel.cs) | 332 | `SurvivorsHostSession survivors` | shell, status rail | No targeted defect assigned; not certified complete |
| [TradeScreenGodotPanel](../../src/Economy/TradeScreenGodotPanel.cs) | 933 | No public Bind found | scroll | No targeted defect assigned; not certified complete |
| [TraumaBondingCohortPanel](../../src/UI/TraumaBondingCohortPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [TravelingCaravanPanel](../../src/UI/TravelingCaravanPanel.cs) | 303 | `TravelingCaravanHostSession session` | shell, status rail, scroll | No targeted defect assigned; not certified complete |
| [TriangulationPanel](../../src/UI/TriangulationPanel.cs) | 202 | `RadioHostSession radioHost, string signalId = "sig_distress"` | custom presentation | No targeted defect assigned; not certified complete |
| [TroposphericRadioRelayPanel](../../src/UI/TroposphericRadioRelayPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [TutorialPanel](../../src/UI/TutorialPanel.cs) | 208 | `int simDay = 1` | custom presentation | No targeted defect assigned; not certified complete |
| [UltrasonicDecontaminationAirlockPanel](../../src/UI/UltrasonicDecontaminationAirlockPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [UndergroundPrintingPressPanel](../../src/UI/UndergroundPrintingPressPanel.cs) | 126 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [UtilityAiPanel](../../src/UtilityAI/UtilityAiPanel.cs) | 104 | No public Bind found | scroll | No targeted defect assigned; not certified complete |
| [VaultDoorBreachingPanel](../../src/UI/VaultDoorBreachingPanel.cs) | 197 | `object? session` | ALWAYS bound | UI-05 |
| [VerdictDashboardPanel](../../src/UI/VerdictDashboardPanel.cs) | 161 | `VerdictPanel verdict, VerdictHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [VerdictPanel](../../src/VerdictPanel.cs) | 432 | `VerdictHostSession verdict` | scroll | No targeted defect assigned; not certified complete |
| [VinylMoralePanel](../../src/UI/VinylMoralePanel.cs) | 206 | `VinylMoraleHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [WarDogKennelPanel](../../src/UI/WarDogKennelPanel.cs) | 130 | No public Bind found | EMPTY refresh, ALWAYS bound | UI-06 |
| [WaterTreatmentPanel](../../src/UI/WaterTreatmentPanel.cs) | 142 | `WaterTreatmentHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [WaystationNetworkPanel](../../src/UI/WaystationNetworkPanel.cs) | 185 | `WaystationHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [WeatherDetailPanel](../../src/UI/WeatherDetailPanel.cs) | 141 | `WeatherSystem? weather` | custom presentation | No targeted defect assigned; not certified complete |
| [WeatherForecastPanel](../../src/UI/WeatherForecastPanel.cs) | 247 | `WeatherSystem weather` | custom presentation | No targeted defect assigned; not certified complete |
| [WeatherHistoryPanel](../../src/UI/WeatherHistoryPanel.cs) | 193 | `WeatherSystem weather` | custom presentation | UI-16 |
| [WeatherPanel](../../src/UI/WeatherPanel.cs) | 478 | `WeatherHostSession weather`<br>`WorldHostSession weather` | shell, status rail, data grid | UI-19 |
| [WeatherSondePanel](../../src/UI/WeatherSondePanel.cs) | 201 | `WeatherHostSession weatherHost` | custom presentation | No targeted defect assigned; not certified complete |
| [WildlifeTrappingPanel](../../src/UI/WildlifeTrappingPanel.cs) | 192 | `WildlifeTrappingHostSession session` | shell, status rail | No targeted defect assigned; not certified complete |
| [WinterFreezePanel](../../src/UI/WinterFreezePanel.cs) | 33 | `YearOfAshDeepFreezeSystem system` | shell, EMPTY refresh | UI-07 |
| [WorkshopPanel](../../src/UI/WorkshopPanel.cs) | 343 | `WorkshopReverseEngineeringSystem workshop, Ashfall.Core.Inventory.Inventory inventory, SurvivorsHostSession? survivors = null` | custom presentation | UI-16 |

## Every registered descriptor

Evidence: `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`, `src/Main.PlayerSurfaces.cs`. Configured means a host action assignment exists in source, not that its UI is functional or player-discoverable.

| Route ID | Registry label | Maturity | Host action assignment |
|---|---|---|---|
| `status` | Survival Status | Live — declared | Found; execution/completeness not certified |
| `help` | Tutorial / Help | Live — declared | Found; execution/completeness not certified |
| `guidance` | Onboarding Guidance | Live — declared | Found; execution/completeness not certified |
| `afflictions` | Afflictions | Live — declared | Found; execution/completeness not certified |
| `radiation_detail` | Radiation Detail | Live — declared | Found; execution/completeness not certified |
| `research` | Research | Live — declared | Found; execution/completeness not certified |
| `weather_detail` | Weather Detail | Live — declared | Found; execution/completeness not certified |
| `weather_forecast` | Weather Forecast | Live — declared | Found; execution/completeness not certified |
| `event_detail` | Event Detail | Live — declared | Found; execution/completeness not certified |
| `events_log` | Events Log | Live — declared | Found; execution/completeness not certified |
| `economy_detail` | Economy Detail | Live — declared | Found; execution/completeness not certified |
| `radiation_history` | Radiation History | Live — declared | Found; execution/completeness not certified |
| `journal_detail` | Journal Detail | Live — declared | Found; execution/completeness not certified |
| `survival_detail` | Survival Detail | Live — declared | Found; execution/completeness not certified |
| `survivor_detail` | Survivor Detail | Live — declared | Found; execution/completeness not certified |
| `inventory_detail` | Inventory Detail | Live — declared | Found; execution/completeness not certified |
| `achievements` | Achievements | Live — declared | Found; execution/completeness not certified |
| `survivors` | Survivors Panel | Live — declared | Found; execution/completeness not certified |
| `inventory` | Inventory Panel | Live — declared | Found; execution/completeness not certified |
| `crafting` | Crafting Panel | Live — declared | Found; execution/completeness not certified |
| `medical` | Medical Panel | Live — declared | Found; execution/completeness not certified |
| `phase0` | Phase 0 Panel | Live — declared | Found; execution/completeness not certified |
| `expeditions` | Expeditions Panel | Live — declared | Found; execution/completeness not certified |
| `weather` | Weather Panel | Live — declared | Found; execution/completeness not certified |
| `radio` | Radio Panel | Live — declared | Found; execution/completeness not certified |
| `map` | Map Panel | Live — declared | Found; execution/completeness not certified |
| `map_detail` | Map Location Detail | Live — declared | Found; execution/completeness not certified |
| `shelter` | Shelter Panel | Live — declared | Found; execution/completeness not certified |
| `factions` | Factions Panel | Live — declared | Found; execution/completeness not certified |
| `faction_detail` | Faction Detail | Live — declared | Found; execution/completeness not certified |
| `quests` | Quests Panel | Live — declared | Found; execution/completeness not certified |
| `quest_detail` | Quest Detail | Live — declared | Found; execution/completeness not certified |
| `moral_choice` | Moral Choice / Ethical Dilemma | Live — declared | Found; execution/completeness not certified |
| `journal` | Journal Panel | Live — declared | Found; execution/completeness not certified |
| `protocol` | Opening Protocol | Live — declared | Found; execution/completeness not certified |
| `greenhouse` | Greenhouse Panel | Live — declared | Found; execution/completeness not certified |
| `silent_foundry` | Silent Foundry Panel | Live — declared | Found; execution/completeness not certified |
| `trade` | Trade / Economy Panel | Live — declared | Found; execution/completeness not certified |
| `muster` | The Muster Panel | Live — declared | Found; execution/completeness not certified |
| `expansions` | Expansions Hub | Live — declared | Found; execution/completeness not certified |
| `standing_record` | Standing Record Panel | Live — declared | Found; execution/completeness not certified |
| `crossing_quests` | Crossing Quest Panel | Live — declared | Found; execution/completeness not certified |
| `maritime` | Maritime / Black Flotilla | Live — declared | Found; execution/completeness not certified |
| `deep_coast` | Deep Coast Panel | Live — declared | Found; execution/completeness not certified |
| `century_seed` | Century Seed Panel | Live — declared | Found; execution/completeness not certified |
| `epilogue` | Epilogue Panel | Live — declared | Found; execution/completeness not certified |
| `verdict` | Verdict Panel | Live — declared | Found; execution/completeness not certified |
| `holdfast` | Holdfast Terminal | Live — declared | Found; execution/completeness not certified |
| `duty_roster` | Duty Roster Panel | Live — declared | Found; execution/completeness not certified |
| `duty_roster_detail` | Duty Roster Detail | Live — declared | Found; execution/completeness not certified |
| `save` | Save / Load Panel | Live — declared | Found; execution/completeness not certified |
| `combat` | Combat Panel | Live — declared | Found; execution/completeness not certified |
| `combat_detail` | Combat Detail | Live — declared | Found; execution/completeness not certified |
| `combat_history` | Combat History | Live — declared | Found; execution/completeness not certified |
| `workshop` | Relic Workshop | Live — declared | Found; execution/completeness not certified |
| `pharma_lab` | Pharma Lab | Live — declared | Found; execution/completeness not certified |
| `pharma` | Pharma Lab (alias) | Live — declared | Found; execution/completeness not certified |
| `codex` | Codex (Journal from menu) | Live — declared | Found; execution/completeness not certified |
| `settings` | Settings Panel | Live — declared | Found; execution/completeness not certified |
| `water_treatment` | Water Treatment | Live — declared | Found; execution/completeness not certified |
| `airlock_security` | Airlock Security | Live — declared | Found; execution/completeness not certified |
| `survivor_relations` | Survivor Relations | Live — declared | Found; execution/completeness not certified |
| `regional_treaty` | Regional Treaty | Live — declared | Found; execution/completeness not certified |
| `vinyl_morale` | Vinyl Morale | Live — declared | Found; execution/completeness not certified |
| `wildlife_trapping` | Wildlife Trapping | Live — declared | Found; execution/completeness not certified |
| `excavation` | Excavation | Live — declared | Found; execution/completeness not certified |
| `apprenticeship` | Apprenticeship | Live — declared | Found; execution/completeness not certified |
| `caregiving` | Caregiving | Live — declared | Found; execution/completeness not certified |
| `shelter_thermal` | Shelter Thermal | Live — declared | Found; execution/completeness not certified |
| `shelter_schedule` | Shelter Schedule | Live — declared | Found; execution/completeness not certified |
| `shelter_decor` | Shelter Interior & Memorial Wall | Live — declared | Found; execution/completeness not certified |
| `autopsy_report` | Autopsy Report | Live — declared | Found; execution/completeness not certified |
| `waystation_network` | Waystation Network | Live — declared | Found; execution/completeness not certified |
| `chemical_dependency` | Chemical Dependency | Live — declared | Found; execution/completeness not certified |
| `sump_flooding` | Sump Flooding | Live — declared | Found; execution/completeness not certified |
| `decontamination` | Decontamination | Live — declared | Found; execution/completeness not certified |
| `kitchen_nutrition` | Kitchen Nutrition | Live — declared | Found; execution/completeness not certified |
| `equipment_condition` | Equipment Condition | Live — declared | Found; execution/completeness not certified |
| `library_study` | Library Study | Live — declared | Found; execution/completeness not certified |
| `archive_desk` | Archive Desk | Live — declared | Found; execution/completeness not certified |
| `contractor_roster` | Contractor Roster | Live — declared | Found; execution/completeness not certified |
| `mental_health_crisis` | Mental Health Crisis | Live — declared | Found; execution/completeness not certified |
| `phantom_memory` | Phantom Memory | Live — declared | Found; execution/completeness not certified |
| `traveling_caravan` | Traveling Caravan | Live — declared | Found; execution/completeness not certified |
| `medical_ward` | Medical Ward | Live — declared | Found; execution/completeness not certified |
| `brine_extraction` | Brine Extraction | Live — declared | Found; execution/completeness not certified |
| `expedition_camp` | Expedition Camp | Live — declared | Found; execution/completeness not certified |
| `fire_incident` | Fire Incident | Live — declared | Found; execution/completeness not certified |
| `geiger_calibration` | Geiger Calibration | Live — declared | Found; execution/completeness not certified |
| `triangulation` | Radio Triangulation | Live — declared | Found; execution/completeness not certified |
| `weather_sonde` | Weather Sonde | Live — declared | Found; execution/completeness not certified |
| `power_grid` | Power Grid | Live — declared | Found; execution/completeness not certified |
| `expedition_radar` | Expedition Radar | Live — declared | Found; execution/completeness not certified |
| `dose_ledger` | Dose Ledger | Live — declared | Found; execution/completeness not certified |
| `caravan_barter` | Caravan Barter Ledger | Live — declared | Found; execution/completeness not certified |
| `faction_matrix` | Faction Stance Matrix | Live — declared | Found; execution/completeness not certified |
| `factions_narrative` | Factions Narrative | Live — declared | Found; execution/completeness not certified |
| `skill_matrix` | Skill Progression Matrix | Live — declared | Found; execution/completeness not certified |
| `survival_workstation` | Survival Workstation | Live — declared | Found; execution/completeness not certified |
| `verdict_dashboard` | Verdict Dashboard | Live — declared | Found; execution/completeness not certified |
| `map_atlas` | Subterranean Map Atlas | Live — declared | Found; execution/completeness not certified |
| `maritime_atlas` | Maritime Expedition Atlas | Live — declared | Found; execution/completeness not certified |
| `muster_atlas` | The Muster Atlas | Live — declared | Found; execution/completeness not certified |
| `quests_atlas` | Quests & Operations Atlas | Live — declared | Found; execution/completeness not certified |
| `research_atlas` | Research Technology Atlas | Live — declared | Found; execution/completeness not certified |
| `standing_record_atlas` | Standing Record Atlas | Live — declared | Found; execution/completeness not certified |
| `combat_hud` | Tactical Combat HUD | Live — declared | Found; execution/completeness not certified |
| `emergency_response` | Emergency Response | Live — declared | Found; execution/completeness not certified |
| `biogas_digester` | Anaerobic Biogas Digester | Prototype — shelved | Shelved; player route rejected |
| `cartography_gis` | 3D Cavity GIS Cartography | Prototype — shelved | Shelved; player route rejected |
| `printing_press` | Clandestine Printing Press | Prototype — shelved | Shelved; player route rejected |
| `silicon_slicing` | Silicon Ingot Slicing | Prototype — shelved | Shelved; player route rejected |
| `geothermal_turbine` | Geothermal Steam Turbine | Prototype — shelved | Shelved; player route rejected |
| `war_dog_kennel` | War Dog Kennel & Bio-Monitor | Prototype — shelved | Shelved; player route rejected |
| `isotope_separator` | Isotope Separator & Calutron | Prototype — shelved | Shelved; player route rejected |
| `plasma_smelting` | Plasma Arc Smelting | Prototype — shelved | Shelved; player route rejected |
| `borehole_seismograph` | Deep Borehole Seismograph | Prototype — shelved | Shelved; player route rejected |
| `logistics_airlock` | Heavy Logistics Airlock | Prototype — shelved | Shelved; player route rejected |
| `cryo_permafrost_core` | Cryogenic Permafrost Core | Prototype — shelved | Shelved; player route rejected |
| `basal_radon_migration` | Basal Radon Migration | Prototype — shelved | Shelved; player route rejected |
| `trauma_bonding_cohort` | Trauma Bonding & Cohort | Prototype — shelved | Shelved; player route rejected |
| `clandestine_insurgency` | Clandestine Insurgency | Prototype — shelved | Shelved; player route rejected |
| `subterranean_debt_ledger` | Subterranean Debt Ledger | Prototype — shelved | Shelved; player route rejected |
| `surface_shrapnel_aegis` | Surface Shrapnel Aegis | Prototype — shelved | Shelved; player route rejected |
| `long_walk_expedition` | Long Walk Expedition | Prototype — shelved | Shelved; player route rejected |
| `sonic_rupture_drill` | Sonic Rupture Drill | Prototype — shelved | Shelved; player route rejected |
| `vault_door_breaching` | Vault Door Breaching | Prototype — shelved | Shelved; player route rejected |
| `iron_cenotaph_memorial` | Iron Cenotaph Memorial | Prototype — shelved | Shelved; player route rejected |
| `aquifer_treaty_concession` | Aquifer Treaty Concession | Prototype — shelved | Shelved; player route rejected |
| `crossing_safe_conduct_vouch` | Crossing Safe Conduct Vouch | Prototype — shelved | Shelved; player route rejected |
| `mechanical_prosthetics_lathe` | Mechanical Prosthetics Lathe | Prototype — shelved | Shelved; player route rejected |
| `fungal_protein_fermenter` | Fungal Protein Fermenter | Prototype — shelved | Shelved; player route rejected |
| `ultrasonic_decontam_airlock` | Ultrasonic Decontam Airlock | Prototype — shelved | Shelved; player route rejected |
| `tropospheric_radio_relay` | Tropospheric Radio Relay | Prototype — shelved | Shelved; player route rejected |
| `induction_cupola_furnace` | Induction Cupola Furnace | Prototype — shelved | Shelved; player route rejected |
| `heavy_marine_diesel_gen` | Heavy Marine Turbodiesel Gen | Prototype — shelved | Shelved; player route rejected |
| `slurry_dewatering_sump` | Slurry Dewatering Sump | Live — declared | Found; execution/completeness not certified |
| `plans_94_97` | Plans 94–97 Operations Console | Live — declared | Found; execution/completeness not certified |
| `plans_110_113` | Plans 110–113 Industrial Operations Console | Live — declared | MISSING — UI-10 |
| `plans_130_133` | Plans 130–133 Operations Console | Live — declared | Found; execution/completeness not certified |
| `magnetic_drum_archive` | Magnetic Drum & Microfiche | Prototype — shelved | Shelved; player route rejected |

## Every configured but unregistered ID

UI-09: `ConfigureActions` returns false and callers ignore it. These 23 IDs map to 22 panel classes; two Fallout aliases address the same class.

- `expansion_fallout_plume`
- `desperation_crisis`
- `mercenary_bounty_board`
- `archaeology_excavation`
- `amputation_surgery`
- `railway_logistics`
- `fungi_cultivation`
- `justice_tribunal`
- `chem_warfare_defense`
- `comms_array_transceiver`
- `ceremony_ritual`
- `robotics_assembly`
- `survivor_downtime`
- `winter_freeze`
- `aviation`
- `narcotics`
- `forced_labor`
- `politics`
- `prisoners`
- `stealth`
- `mutation_tree`
- `nursery`
- `fallout_detail`

## Structural clone evidence

Normalization removes comments, strings, class identifiers, numbers, whitespace and a limited set of color-token names. It detects repeated program structure, not byte-identical files or proof that all similarly styled panels are defective.

### Group 1 — 8 identical normalized structures

- `AquiferTreatyConcessionPanel`
- `CrossingSafeConductVouchPanel`
- `FungalProteinFermenterPanel`
- `HeavyMarineDieselGeneratorPanel`
- `InductionCupolaFurnacePanel`
- `MechanicalProstheticsLathePanel`
- `TroposphericRadioRelayPanel`
- `UltrasonicDecontaminationAirlockPanel`

### Group 2 — 10 identical normalized structures

- `BasalRadonMigrationPanel`
- `ClandestineInsurgencyPanel`
- `CryogenicPermafrostCorePanel`
- `IronCenotaphMemorialPanel`
- `LongWalkExpeditionPanel`
- `SonicRuptureDrillPanel`
- `SubterraneanDebtLedgerPanel`
- `SurfaceShrapnelAegisPanel`
- `TraumaBondingCohortPanel`
- `VaultDoorBreachingPanel`

`MagneticDrumArchivePanel` is a nineteenth near-copy with a changed margin accessor; `SlurryDewateringSumpPanel` shares a three-column comment/style but has real SumpFlooding binding and must **not** be classified as a stub from appearance or comment alone.
