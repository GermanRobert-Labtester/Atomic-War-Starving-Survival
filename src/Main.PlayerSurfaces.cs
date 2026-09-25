// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private void RegisterPlayerSurfaces()
        {
            // Core Dashboard Panels
            PanelRegistry.ConfigureActions("status",
                bindAction: () => { SetupSurvivors(); SetupWorld(); SetupInventory(); _statusPanel.Bind(_survivors, _world?.Weather, _powerGrid, _inventory, _simDay); },
                openAction: () => _statusPanel.Open(),
                closeAction: () => CloseStatusPanel());

            PanelRegistry.ConfigureActions("help",
                bindAction: () => { _tutorialPanel.Bind(_simDay); FlushContextualTutorialQueue(); },
                openAction: () => _tutorialPanel.Open(),
                closeAction: () => CloseTutorialPanel());

            PanelRegistry.ConfigureActions("guidance",
                bindAction: () => { SetupOnboarding(); EnsureOnboardingPanel(); },
                openAction: () =>
                {
                    if (AtomicWar.GodotApp.Settings.UserSettingsStore.Current.TutorialMode == 2)
                    {
                        if (_statusLabel != null)
                            _statusLabel.Text = AtomicWar.GodotApp.Localization.AshfallLocalization.Tr(
                                "onboarding.status.disabled",
                                "ONBOARDING DISABLED — veteran mode is active.");
                        return;
                    }
                    EnsureOnboardingPanel();
                    _onboardingHintPanel?.Show();
                },
                closeAction: () => ClosePanelAnimated(_onboardingHintPanel));

            PanelRegistry.ConfigureActions("emergency_response",
                bindAction: () =>
                {
                    _crisisCoordinator.Bind(_powerGrid?.System, _disease?.Engine, _world?.Weather, _startingLevel?.System,
                        _survivors?.Radiation, _survivorFate, _shelterFireSession?.System, _sumpFlooding?.System);
                    _crisisCoordinator.EvaluateCrisisState();
                    _crisisHud.Bind(_crisisPresentationSnapshot);
                },
                openAction: () =>
                {
                    _crisisCoordinator.Bind(_powerGrid?.System, _disease?.Engine, _world?.Weather, _startingLevel?.System,
                        _survivors?.Radiation, _survivorFate, _shelterFireSession?.System, _sumpFlooding?.System);
                    _crisisCoordinator.EvaluateCrisisState();
                    _crisisHud.Bind(_crisisPresentationSnapshot);
                    _crisisHud.Open();
                },
                closeAction: () => _crisisHud.Close());

            PanelRegistry.ConfigureActions("expansion_fallout_plume",
                bindAction: () => _falloutPlumePanel.Bind(EnsureFallout()),
                openAction: () => ShowPanelLifecycle(_falloutPlumePanel),
                closeAction: () => ClosePanelAnimated(_falloutPlumePanel));

            PanelRegistry.ConfigureActions("desperation_crisis",
                bindAction: () => _desperationCrisisPanel.Bind(EnsureDesperation()),
                openAction: () => ShowPanelLifecycle(_desperationCrisisPanel),
                closeAction: () => ClosePanelAnimated(_desperationCrisisPanel));

            PanelRegistry.ConfigureActions("mercenary_bounty_board",
                bindAction: () => _mercenaryBountyBoardPanel.Bind(EnsureMercenary()),
                openAction: () => { _mercenaryBountyBoardPanel.SetDisplayClock(_simDay); ShowPanelLifecycle(_mercenaryBountyBoardPanel); },
                closeAction: () => ClosePanelAnimated(_mercenaryBountyBoardPanel));

            PanelRegistry.ConfigureActions("archaeology_excavation",
                bindAction: () => _archaeologyExcavationPanel.Bind(EnsureArchaeology()),
                openAction: () => ShowPanelLifecycle(_archaeologyExcavationPanel),
                closeAction: () => ClosePanelAnimated(_archaeologyExcavationPanel));

            PanelRegistry.ConfigureActions("amputation_surgery",
                bindAction: () => _amputationTriagePanel.Bind(EnsureAmputation()),
                openAction: () => ShowPanelLifecycle(_amputationTriagePanel),
                closeAction: () => ClosePanelAnimated(_amputationTriagePanel));

            PanelRegistry.ConfigureActions("railway_logistics",
                bindAction: () => _railwayTerminalPanel.Bind(EnsureRailway()),
                openAction: () => ShowPanelLifecycle(_railwayTerminalPanel),
                closeAction: () => ClosePanelAnimated(_railwayTerminalPanel));

            PanelRegistry.ConfigureActions("fungi_cultivation",
                bindAction: () => _fungiCultivationBedPanel.Bind(EnsureFungi()),
                openAction: () => ShowPanelLifecycle(_fungiCultivationBedPanel),
                closeAction: () => ClosePanelAnimated(_fungiCultivationBedPanel));

            PanelRegistry.ConfigureActions("plastic_pyrolysis",
                bindAction: () => _plasticPyrolysisPanel.Bind(EnsurePlasticPyrolysis()),
                openAction: () => ShowPanelLifecycle(_plasticPyrolysisPanel),
                closeAction: () => ClosePanelAnimated(_plasticPyrolysisPanel));

            PanelRegistry.ConfigureActions("cargo_airdrop",
                bindAction: () => _cargoAirdropPanel.Bind(EnsureCargoAirdrop()),
                openAction: () => ShowPanelLifecycle(_cargoAirdropPanel),
                closeAction: () => ClosePanelAnimated(_cargoAirdropPanel));

            PanelRegistry.ConfigureActions("justice_tribunal",
                bindAction: () => _justiceTribunalPanel.Bind(EnsureJustice()),
                openAction: () => ShowPanelLifecycle(_justiceTribunalPanel),
                closeAction: () => ClosePanelAnimated(_justiceTribunalPanel));

            PanelRegistry.ConfigureActions("chem_warfare_defense",
                bindAction: () => _chemWarfareDefensePanel.Bind(EnsureChemWarfare()),
                openAction: () => ShowPanelLifecycle(_chemWarfareDefensePanel),
                closeAction: () => ClosePanelAnimated(_chemWarfareDefensePanel));

            // Plans 146–149 industrial flagship consoles.
            PanelRegistry.ConfigureActions("ebpvd_coating",
                bindAction: () => { SetupEbPvdCoating(); if (_ebPvdCoating != null) _ebPvdCoatingPanel?.Bind(_ebPvdCoating); },
                openAction: () => HandleEbPvdCoatingAction("OPEN"),
                closeAction: () => HandleEbPvdCoatingAction("CLOSE"));

            PanelRegistry.ConfigureActions("microfluidic_diagnostic",
                bindAction: () => { SetupMicrofluidicDiagnostic(); if (_microfluidicDiagnostic != null) _microfluidicDiagnosticPanel?.Bind(_microfluidicDiagnostic); },
                openAction: () => HandleMicrofluidicDiagnosticAction("OPEN"),
                closeAction: () => HandleMicrofluidicDiagnosticAction("CLOSE"));

            PanelRegistry.ConfigureActions("mine_clearing_flail",
                bindAction: () => { SetupMineClearingFlail(); if (_mineClearingFlail != null) _mineFlailPanel?.Bind(_mineClearingFlail); },
                openAction: () => HandleMineFlailAction("OPEN"),
                closeAction: () => HandleMineFlailAction("CLOSE"));

            PanelRegistry.ConfigureActions("rail_grinding",
                bindAction: () => { SetupRailGrinding(); if (_railGrinding != null) _railGrindingPanel?.Bind(_railGrinding); },
                openAction: () => HandleRailGrindingAction("OPEN"),
                closeAction: () => HandleRailGrindingAction("CLOSE"));

            PanelRegistry.ConfigureActions("comms_array_transceiver",
                bindAction: () => _commsArrayTransceiverPanel.Bind(EnsureCommsArray()),
                openAction: () => { _commsArrayTransceiverPanel.SetDisplayClock(_simDay, 12); ShowPanelLifecycle(_commsArrayTransceiverPanel); },
                closeAction: () => ClosePanelAnimated(_commsArrayTransceiverPanel));

            PanelRegistry.ConfigureActions("ceremony_ritual",
                bindAction: () => _ceremonyFestivalPanel.Bind(EnsureCeremonySystem()),
                openAction: () => ShowPanelLifecycle(_ceremonyFestivalPanel),
                closeAction: () => ClosePanelAnimated(_ceremonyFestivalPanel));

            PanelRegistry.ConfigureActions("robotics_assembly",
                bindAction: () => _roboticsWorkshopPanel.Bind(EnsureRobotics()),
                openAction: () => ShowPanelLifecycle(_roboticsWorkshopPanel),
                closeAction: () => ClosePanelAnimated(_roboticsWorkshopPanel));

            PanelRegistry.ConfigureActions("bio_fermentation",
                bindAction: () => _bioFermentationPanel.Bind(EnsureBioFermentation()),
                openAction: () => ShowPanelLifecycle(_bioFermentationPanel),
                closeAction: () => ClosePanelAnimated(_bioFermentationPanel));

            PanelRegistry.ConfigureActions("survivor_downtime",
                bindAction: () => _survivorDowntimePanel.Bind(EnsureRecreation()),
                openAction: () => ShowPanelLifecycle(_survivorDowntimePanel),
                closeAction: () => ClosePanelAnimated(_survivorDowntimePanel));

            PanelRegistry.ConfigureActions("winter_freeze",
                bindAction: () => _winterFreezePanel.Bind(_yearOfAsh != null ? _yearOfAsh.DeepFreeze : null!),
                openAction: () => ShowPanelLifecycle(_winterFreezePanel),
                closeAction: () => ClosePanelAnimated(_winterFreezePanel));

            PanelRegistry.ConfigureActions("afflictions",
                bindAction: () => { SetupSurvivors(); SetupInventory(); SetupMedical(); SetupPhase0(); _afflictionsPanel.Bind(_medical, _survivors, _inventory, _phase0?.Respiratory); },
                openAction: () => _afflictionsPanel.Open(),
                closeAction: () => CloseAfflictionsPanel());

            PanelRegistry.ConfigureActions("radiation_detail",
                bindAction: () => { SetupSurvivors(); SetupPhase0(); _radiationDetailPanel.Bind(_doseLedger, _survivors); },
                openAction: () => { ObserveSigil("dose.read"); _radiationDetailPanel.Open(); },
                closeAction: () => CloseRadiationDetailPanel());

            PanelRegistry.ConfigureActions("research",
                bindAction: () =>
                {
                    _sharedResearch = EnsureSharedResearch();
                    _researchHostSession ??= ResearchHostSession.Create(_dataDir, _sharedResearch);
                    // Plan 71: room_laboratory_research — new research requires a
                    // powered laboratory (start-boundary gate; in-flight progress
                    // is owned by the research authority and never touched here).
                    _researchHostSession.StartResearchGate = () =>
                        _powerGrid?.System?.IsRoomPowered("room_laboratory_research") ?? true;
                    _researchPanel.Bind(_sharedResearch, _researchHostSession);
                },
                openAction: () => _researchPanel.Open(),
                closeAction: () => CloseResearchPanel());

            PanelRegistry.ConfigureActions("weather_detail",
                bindAction: () => { SetupWorld(); _weatherDetailPanel.Bind(_world?.Weather); },
                openAction: () => { MaybeRequestSevereWeatherLesson(); _weatherDetailPanel.Open(); },
                closeAction: () => CloseWeatherDetailPanel());

            PanelRegistry.ConfigureActions("weather_forecast",
                bindAction: () => { SetupWorld(); _weatherForecastPanel.Bind(_world?.Weather, _world?.WeatherIntelligence); },
                openAction: () => { MaybeRequestSevereWeatherLesson(); _weatherForecastPanel.Open(); },
                closeAction: () => CloseWeatherForecastPanel());

            PanelRegistry.ConfigureActions("weather_history",
                bindAction: () => { SetupWorld(); _weatherHistoryPanel.Bind(_world?.Weather); },
                openAction: () => OpenWeatherHistoryPanel(),
                closeAction: () => ClosePanelAnimated(_weatherHistoryPanel));

            PanelRegistry.ConfigureActions("event_detail",
                bindAction: () => { SetupEventsHost(); _eventDetailPanel.Bind(_eventsHost); },
                openAction: () => _eventDetailPanel.Open(),
                closeAction: () => CloseEventDetailPanel());

            PanelRegistry.ConfigureActions("narrative_arc",
                bindAction: () => SetupNarrative(),
                openAction: () => OpenNarrativeArcModal(),
                closeAction: () => CloseNarrativeArcModal());

            PanelRegistry.ConfigureActions("events_log",
                bindAction: () => { SetupEventsHost(); _eventsLogPanel.Bind(_eventsHost); },
                openAction: () => _eventsLogPanel.Open(),
                closeAction: () => CloseEventsLogPanel());

            PanelRegistry.ConfigureActions("economy_detail",
                bindAction: () =>
                {
                    SetupEconomy();
                    // Plan 14A (B1): the embargo banner needs the live weather;
                    // the provider is read-only (the authority stays in Core).
                    _economyDetailPanel.Bind(_economy, () => _world?.Weather?.Current ?? Ashfall.Core.WeatherKind.Clear);
                },
                openAction: () => _economyDetailPanel.Open(),
                closeAction: () => CloseEconomyDetailPanel());

            PanelRegistry.ConfigureActions("radiation_history",
                bindAction: () => { SetupPhase0(); _radiationHistoryPanel.Bind(_doseLedger); },
                openAction: () => _radiationHistoryPanel.Open(),
                closeAction: () => CloseRadiationHistoryPanel());

            PanelRegistry.ConfigureActions("journal_detail",
                bindAction: () => { SetupJournal(); _journalDetailPanel.Bind(_journal); },
                openAction: () => _journalDetailPanel.Open(),
                closeAction: () => CloseJournalDetailPanel());

            PanelRegistry.ConfigureActions("survival_detail",
                bindAction: () => { SetupSurvivors(); _survivalDetailPanel.Bind(_survivors); },
                openAction: () => _survivalDetailPanel.Open(),
                closeAction: () => CloseSurvivalDetailPanel());

            PanelRegistry.ConfigureActions("survivor_detail",
                bindAction: () => { SetupSurvivors(); SetupEnrichment(); SetupSurvivorSocial(); SetupCulturalArchive(); SetupBackstory(); SetupIdeologicalFriction(); SetupRomanceFamily(); _survivorDetailPanel.BelongingsProvider = id => _survivorSocial?.Belongings.GetBelongingsForSurvivor(id) ?? Array.Empty<Ashfall.Core.Survivors.PersonalBelonging>(); _survivorDetailPanel.DocumentationProvider = id => GetSurvivorDocumentation(id); var first = _survivors?.RosterState?.FirstOrDefault(s => s != null)?.Id ?? ""; _survivorDetailPanel.Bind(_survivors, first, _enrichmentService); },
                openAction: () => _survivorDetailPanel.Open(),
                closeAction: () => CloseSurvivorDetailPanel());

            PanelRegistry.ConfigureActions("inventory_detail",
                bindAction: () => { SetupInventory(); SetupEnrichment(); var first = _inventory?.Inventory?.FindSlot("bandage")?.Item?.id ?? "bandage"; _inventoryDetailPanel.Bind(_inventory, first, null, _enrichment); },
                openAction: () => _inventoryDetailPanel.Open(),
                closeAction: () => CloseInventoryDetailPanel());

            PanelRegistry.ConfigureActions("achievements",
                bindAction: () => { SetupSurvivors(); _achievementsPanel.Bind(_survivors, _simDay); },
                openAction: () => _achievementsPanel.Open(),
                closeAction: () => CloseAchievementsPanel());

            PanelRegistry.ConfigureActions("survivors",
                bindAction: () => { SetupSurvivors(); _survivorsOverlay.Bind(_survivors); },
                openAction: () => _survivorsOverlay.Open(),
                closeAction: () => CloseSurvivorsOverlay());

            PanelRegistry.ConfigureActions("inventory",
                bindAction: () => { SetupInventory(); _inventoryOverlay.Bind(_inventory); _inventoryOverlay.RefreshView(); },
                openAction: () => _inventoryOverlay.Open(),
                closeAction: () => CloseInventoryOverlay());

            PanelRegistry.ConfigureActions("crafting",
                bindAction: () => { SetupCrafting(); SetupInventory(); SetupSurvivors(); SetupPhase0(); SyncCraftingStationsFromShelter(); _craftingPanel.Bind(_crafting, _inventory, _survivors, _phase0?.TradeSpecialty, sid => ResolveSurvivorProfessionId(sid)); },
                openAction: () => { SyncCraftingStationsFromShelter(); _craftingPanel.Open(); },
                closeAction: () => CloseCraftingPanel());

            PanelRegistry.ConfigureActions("workshop",
                bindAction: () => { EnsureShelterWorkshop(); SetupInventory(); SetupEquipmentCondition(); SetupExpeditions(); SetupSurvivors(); SetupCrafting(); _workshopPanel.Bind(_shelterWorkshop!, _inventory.Inventory, _equipmentCondition?.System, _expeditions?.Vehicles, _survivors); _workshopPanel.BindRelicWorkshop(_crafting!.Workshop, _inventory.Inventory, _crafting.LoadedItemCatalog, _survivors); },
                openAction: () => _workshopPanel.Open(),
                closeAction: () => CloseWorkshopPanel());

            PanelRegistry.ConfigureActions("radio_intelligence",
                bindAction: () => { EnsureRadioStation(); _radioIntelligencePanel.Bind(_radioStationSystem!, EnsureOrbitalHarrowTelemetry(), _simDay); },
                openAction: () => _radioIntelligencePanel.Open(),
                closeAction: () => CloseRadioIntelligencePanel());

            PanelRegistry.ConfigureActions("shelter_social",
                bindAction: () => { EnsureShelterSocialDynamics(); SetupSurvivors(); _shelterSocialPanel.Bind(_shelterSocialDynamics!, _survivorRelations?.System, _survivors?.Needs, _memorial, _survivors, _inventory?.Inventory, _simDay); },
                openAction: () => _shelterSocialPanel.Open(),
                closeAction: () => CloseShelterSocialPanel());

            PanelRegistry.ConfigureActions("subterranean_operations",
                bindAction: () => { EnsureExcavationHazards(); SetupInventory(); SetupSurvivors(); _subterraneanOperationsPanel.Bind(_excavationHazards!, _inventory.Inventory, _survivors); },
                openAction: () => _subterraneanOperationsPanel.Open(),
                closeAction: () => CloseSubterraneanOperationsPanel());

            PanelRegistry.ConfigureActions("pharma_lab",
                bindAction: () => { SetupCrafting(); SetupInventory(); SetupSurvivors(); SetupMentalHealthCrisis(); _pharmaLabPanel.Bind(_crafting.PharmaLab, _inventory.Inventory, _chemicalDependency?.System, _survivors); },
                openAction: () => _pharmaLabPanel.Open(),
                closeAction: () => ClosePharmaLabPanel());

            PanelRegistry.ConfigureActions("pharma",
                bindAction: () => { SetupCrafting(); SetupInventory(); SetupSurvivors(); SetupMentalHealthCrisis(); _pharmaLabPanel.Bind(_crafting.PharmaLab, _inventory.Inventory, _chemicalDependency?.System, _survivors); },
                openAction: () => _pharmaLabPanel.Open(),
                closeAction: () => ClosePharmaLabPanel());

            PanelRegistry.ConfigureActions("medical",
                bindAction: () => { SetupJournal(); DiscoverBureaucraticDocuments("medical_office"); SetupSurvivors(); SetupInventory(); SetupMedical(); SetupPhase0(); EnsureMedicalPipeline(); _medicalPanel.Bind(_medical, _survivors, _inventory, _phase0?.Respiratory); },
                openAction: () => _medicalPanel.Open(),
                closeAction: () => CloseMedicalPanel());

            PanelRegistry.ConfigureActions("phase0",
                openAction: () => OpenPhase0Panel(),
                closeAction: () => ClosePhase0Panel());

            PanelRegistry.ConfigureActions("expeditions",
                bindAction: () => { SetupExpeditions(); SetupExpansions(); _expeditions.CrossingGate = _expansions.Vouch; SetupSurvivors(); SetupInventory(); SetupWorld(); SetupEvolvingWorldInfluence(); _expeditionPanel.Bind(_expeditions, _survivors, _inventory, _equipmentCondition?.System, _world); },
                openAction: () => { MaybeRequestProtectionLesson(); _expeditionPanel.Open(); },
                closeAction: () => CloseExpeditionPanel());

            PanelRegistry.ConfigureActions("weather",
                bindAction: () => { SetupWorld(); _weatherPanel.Bind(_world); },
                openAction: () => { MaybeRequestSevereWeatherLesson(); _weatherPanel.Open(); },
                closeAction: () => CloseWeatherPanel());

            PanelRegistry.ConfigureActions("radio",
                bindAction: () =>
                {
                    SetupRadio();
                    _radioPanel.Bind(_radio);
                    _radioPanel.BindProduction(EnsureRadioProgramProductionSession());
                },
                openAction: () => _radioPanel.Open(),
                closeAction: () => CloseRadioPanel());

            PanelRegistry.ConfigureActions("map",
                bindAction: () => { SetupHoldfastRuntime(); SetupExpeditions(); SetupExpansions(); SetupWorld(); SetupJournal(); SetupDeepCoast(); SetupYearOfAsh(); SetupWildlifeTrapping(); _mapPanel.Bind(_core, _expeditions, _expansions, _world, _journalCodex?.Catalogs, _deepCoast, _yearOfAsh); },
                openAction: () => _mapPanel.Open(),
                closeAction: () => CloseMapPanel());

            PanelRegistry.ConfigureActions("map_detail",
                openAction: () => _mapDetailPanel.Open(),
                closeAction: () => CloseMapDetailPanel());

            PanelRegistry.ConfigureActions("shelter",
                bindAction: () => {
                    SetupJournal();
                    DiscoverBureaucraticDocuments("shelter_records");
                    SetupSurvivors();
                    SetupWorld();
                    SetupInventory();
                    int shelterDay = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
                    _shelterPanel.Bind(_survivors, _world, _inventory, GetShelterRoomIdentityCatalog(), GetBunkerGraffitiCatalog(), shelterDay, _dutyRoster, _shelterAssignment);
                    _shelterPanel.SetMachineTellCatalog(GetMachineTellCatalog());
                },
                openAction: () => _shelterPanel.Open(),
                closeAction: () => CloseShelterPanel());

            PanelRegistry.ConfigureActions("factions",
                bindAction: () => { SetupHoldfastRuntime(); SetupMuster(); SetupExpansions(); SetupYearOfAsh(); SetupFactionBranch(); SetupMoralChoice(); _factionsPanel.Bind(_core.Catalog.Factions, _holdfastRuntime?.Trade, _muster, _expansions, _yearOfAsh, _factionBranch?.Coordinator, _moralChoice); },
                openAction: () => _factionsPanel.Open(),
                closeAction: () => CloseFactionsPanel());

            PanelRegistry.ConfigureActions("faction_detail",
                openAction: () => _factionDetailPanel.Open(),
                closeAction: () => CloseFactionDetailPanel());

            PanelRegistry.ConfigureActions("faction_culture_codex",
                bindAction: () => { SetupMuster(); _factionCultureCodexPanel.Bind(_muster); },
                openAction: () => { SetupMuster(); _factionCultureCodexPanel.Bind(_muster); _factionCultureCodexPanel.Open(); },
                closeAction: () => CloseFactionCultureCodexPanel());

            PanelRegistry.ConfigureActions("quests",
                bindAction: () => { SetupHoldfastRuntime(); SetupExpansions(); SetupDutyRoster(); SetupFactionBranch(); SetupMoralChoice(); BindQuestsPanel(); },
                openAction: () => _questsPanel.Open(),
                closeAction: () => CloseQuestsPanel());

            PanelRegistry.ConfigureActions("quest_detail",
                openAction: () => _questDetailPanel.Open(),
                closeAction: () => CloseQuestDetailPanel());

            PanelRegistry.ConfigureActions("moral_choice",
                bindAction: () => SetupMoralChoice(),
                openAction: () => OpenMoralChoiceModal(null),
                closeAction: () => CloseMoralChoiceModal());

            PanelRegistry.ConfigureActions("journal",
                bindAction: () => SetupJournal(),
                openAction: () => _journalBook.Open(),
                closeAction: () => _journalBook.Close());

            PanelRegistry.ConfigureActions("codex",
                bindAction: () => SetupJournal(),
                openAction: () => _journalBook.Open(),
                closeAction: () => _journalBook.Close());

            PanelRegistry.ConfigureActions("protocol",
                bindAction: () => { SetupStartingLevel(); _openingProtocolModal.Bind(_startingLevel); },
                openAction: () => _openingProtocolModal.Open(),
                closeAction: () => CloseOpeningProtocolModal());

            PanelRegistry.ConfigureActions("greenhouse",
                bindAction: () => { SetupGreenhouse(); _greenhousePanel.Bind(_greenhouse); },
                openAction: () => _greenhousePanel.Open(),
                closeAction: () => CloseGreenhousePanel());

            PanelRegistry.ConfigureActions("silent_foundry",
                bindAction: () => { SetupExpansions(); SetupSilentFoundry(); _silentFoundryPanel.Bind(_silentFoundry, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay); _silentFoundryPanel.SetMachineTellCatalog(GetMachineTellCatalog()); },
                openAction: () => _silentFoundryPanel.Open(),
                closeAction: () => CloseSilentFoundryPanel());

            PanelRegistry.ConfigureActions("trade",
                bindAction: () => { SetupEconomy(); SetupSilentFoundry(); },
                openAction: () => OpenTradeScreen(),
                closeAction: () => CloseTradePanel());

            PanelRegistry.ConfigureActions("muster",
                bindAction: () => { SetupMuster(); _musterPanel.Bind(_muster, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay); },
                openAction: () => _musterPanel.Open(),
                closeAction: () => CloseMusterPanel());

            PanelRegistry.ConfigureActions("expansions",
                bindAction: () => { SetupExpansions(); SetupGreenhouse(); SetupDutyRoster(); SetupMuster(); SetupMaritime(); SetupDeepCoast(); SetupWorld(); SetupMedical(); SetupVerdict(); _expansionsHubPanel.Bind(_expansions, _greenhouse, _dutyRoster, _muster, _maritime, _deepCoast, _world, _medical, _verdict, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay); },
                openAction: () => _expansionsHubPanel.Open(),
                closeAction: () => CloseExpansionsHubPanel());

            PanelRegistry.ConfigureActions("standing_record",
                bindAction: () => { SetupExpansions(); _standingRecordPanel.Bind(_expansions?.Layouts); },
                openAction: () => _standingRecordPanel.Open(),
                closeAction: () => CloseStandingRecordPanel());

            PanelRegistry.ConfigureActions("crossing_quests",
                bindAction: () => { SetupExpansions(); _crossingQuestPanel.Bind(_expansions, _expansions?.Vouch, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay); },
                openAction: () => _crossingQuestPanel.Open(),
                closeAction: () => CloseCrossingQuestPanel());

            PanelRegistry.ConfigureActions("maritime",
                bindAction: () => { SetupMaritime(); SetupSurvivors(); _maritimePanel.Bind(_maritime, _survivors); },
                openAction: () => _maritimePanel.Open(),
                closeAction: () => CloseMaritimePanel());

            PanelRegistry.ConfigureActions("deep_coast",
                bindAction: () => { SetupDeepCoast(); _deepCoastPanel.Bind(_deepCoast, _core); _deepCoastPanel.SetSimDay(_simDay); },
                openAction: () => _deepCoastPanel.Open(),
                closeAction: () => CloseDeepCoastPanel());

            PanelRegistry.ConfigureActions("century_seed",
                bindAction: () => { SetupExpansions(); SetupSurvivors(); _centurySeedPanel.Bind(_expansions?.Generational, _survivors); },
                openAction: () => _centurySeedPanel.Open(),
                closeAction: () => CloseCenturySeedPanel());

            PanelRegistry.ConfigureActions("epilogue",
                bindAction: () => { _epiloguePanel.Bind(BuildCurrentEpilogueContext()); },
                openAction: () => _epiloguePanel.Open(),
                closeAction: () => CloseEpiloguePanel());

            PanelRegistry.ConfigureActions("chronicle",
                bindAction: () => { SetupEndgame(); _chroniclePanel.Bind(_endgame); },
                openAction: () => OpenChroniclePanel(),
                closeAction: () => CloseChroniclePanel());

            PanelRegistry.ConfigureActions("verdict",
                bindAction: () => { SetupVerdict(); _verdictPanel.Bind(_verdict); },
                openAction: () => _verdictPanel.Open(),
                closeAction: () => CloseVerdictPanel());

            PanelRegistry.ConfigureActions("holdfast",
                bindAction: () => { SetupHoldfastRuntime(); if (_holdfastTerminal != null) { _holdfastTerminal.BindSession(_holdfastRuntime); } },
                openAction: () => { if (_holdfastTerminal != null) _holdfastTerminal.OpenTerminal(); },
                closeAction: () => ClosePanelAnimated(_holdfastTerminal));

            PanelRegistry.ConfigureActions("duty_roster",
                bindAction: () => { SetupJournal(); DiscoverBureaucraticDocuments("duty_roster"); SetupDutyRoster(); SetupSurvivors(); _dutyRosterPanel.Bind(_dutyRoster, _survivors); },
                openAction: () => _dutyRosterPanel.Open(),
                closeAction: () => CloseDutyRosterPanel());

            PanelRegistry.ConfigureActions("duty_roster_detail",
                bindAction: () => { SetupDutyRoster(); _dutyRosterDetailPanel.Bind(_dutyRoster); },
                openAction: () => _dutyRosterDetailPanel.Open(),
                closeAction: () => CloseDutyRosterDetailPanel());

            PanelRegistry.ConfigureActions("save",
                bindAction: () => SaveAll(),
                openAction: () => _saveLoadPanel.Open(),
                closeAction: () => CloseSaveLoadPanel());

            PanelRegistry.ConfigureActions("settings",
                openAction: () => _settingsPanel.Open(),
                closeAction: () => CloseSettingsPanel());

            PanelRegistry.ConfigureActions("combat",
                openAction: () => _combatPanel.Open(),
                closeAction: () => CloseCombatPanel());

            PanelRegistry.ConfigureActions("combat_detail",
                openAction: () => _combatDetailPanel.Open(),
                closeAction: () => CloseCombatDetailPanel());

            PanelRegistry.ConfigureActions("combat_history",
                openAction: () => _combatHistoryPanel.Open(),
                closeAction: () => CloseCombatHistoryPanel());

            // ── Standalone & Subsystem Consoles ──────────────────────────────
            PanelRegistry.ConfigureActions("brine_extraction",
                bindAction: () => { SetupSilentFoundry(); if (_silentFoundry != null) _brineExtractionPanel.Bind(_silentFoundry); },
                openAction: () => _brineExtractionPanel.Open(),
                closeAction: () => ClosePanelAnimated(_brineExtractionPanel));

            PanelRegistry.ConfigureActions("expedition_camp",
                bindAction: () => { SetupExpeditions(); SetupSurvivors(); string survId = _survivors?.RosterState?.FirstOrDefault()?.Id ?? string.Empty; _expeditionCampPanel.Bind(_expeditions, survId); },
                openAction: () => _expeditionCampPanel.Open(),
                closeAction: () => ClosePanelAnimated(_expeditionCampPanel));

            PanelRegistry.ConfigureActions("fire_incident",
                bindAction: () =>
                {
                    SetupShelterFireHazard();
                    SetupSurvivors();
                    _fireIncidentPanel.RosterWorkerProvider = () =>
                        _survivors?.RosterState?.Where(s => s.IsAlive && !s.IsDead)
                            .Select(s => s.Id).Take(3).ToList() ?? new System.Collections.Generic.List<string>();
                    _fireIncidentPanel.Rng = _campaignDay?.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 15);
                    _fireIncidentPanel.Bind(_shelterFireSession!);
                },
                openAction: () => _fireIncidentPanel.Open(),
                closeAction: () => ClosePanelAnimated(_fireIncidentPanel));

            PanelRegistry.ConfigureActions("geiger_calibration",
                bindAction: () => { SetupPhase0(); _geigerCalibrationPanel.Bind(_doseLedger, ResolveLiveDosimeterTag()); },
                openAction: () => _geigerCalibrationPanel.Open(),
                closeAction: () => ClosePanelAnimated(_geigerCalibrationPanel));

            PanelRegistry.ConfigureActions("triangulation",
                bindAction: () => { SetupRadio(); _triangulationPanel.Bind(_radio, ResolveLiveTriangulationSignalId()); },
                openAction: () => _triangulationPanel.Open(),
                closeAction: () => ClosePanelAnimated(_triangulationPanel));

            PanelRegistry.ConfigureActions("weather_sonde",
                bindAction: () =>
                {
                    SetupWeatherSonde();
                    _weatherSondePanel.Bind(_weatherSondeHost);
                },
                openAction: () => _weatherSondePanel.Open(),
                closeAction: () => ClosePanelAnimated(_weatherSondePanel));

            PanelRegistry.ConfigureActions("power_grid",
                bindAction: () => OpenPowerGrid(),
                openAction: () => _powerGridPanel?.Open(),
                closeAction: () => ClosePanelAnimated(_powerGridPanel));

            PanelRegistry.ConfigureActions("geothermal_orc",
                bindAction: () => { ComposePlans74To77(); _geothermalOrcPanel.Bind(_geothermalOrc); },
                openAction: () => _geothermalOrcPanel.Open(),
                closeAction: () => _geothermalOrcPanel.Close());

            PanelRegistry.ConfigureActions("ballistics_workbench",
                bindAction: () => { ComposePlans74To77(); _ballisticsWorkbenchPanel.Bind(_ballisticsWorkbench); },
                openAction: () => _ballisticsWorkbenchPanel.Open(),
                closeAction: () => _ballisticsWorkbenchPanel.Close());

            PanelRegistry.ConfigureActions("aeroponics",
                bindAction: () => { ComposePlans74To77(); _aeroponicsPanel.Bind(_aeroponics); },
                openAction: () => _aeroponicsPanel.Open(),
                closeAction: () => _aeroponicsPanel.Close());

            PanelRegistry.ConfigureActions("pneumatic_dispatch",
                bindAction: () => { ComposePlans74To77(); _pneumaticDispatchPanel.Bind(_pneumaticDispatch); },
                openAction: () => _pneumaticDispatchPanel.Open(),
                closeAction: () => _pneumaticDispatchPanel.Close());

            PanelRegistry.ConfigureActions("expedition_radar",
                bindAction: () => { SetupExpeditions(); SetupSurvivors(); _expeditionRadarPanel.Bind(_expeditions, _survivors); },
                openAction: () => _expeditionRadarPanel.Open(),
                closeAction: () => ClosePanelAnimated(_expeditionRadarPanel));

            PanelRegistry.ConfigureActions("dose_ledger",
                bindAction: () => { SetupPhase0(); SetupSurvivors(); _doseLedgerPanel.Bind(_doseLedger, _survivors); },
                openAction: () => { ObserveSigil("dose.read"); _doseLedgerPanel.Open(); },
                closeAction: () => ClosePanelAnimated(_doseLedgerPanel));

            PanelRegistry.ConfigureActions("dose_geography",
                bindAction: () => { SetupPhase0(); _doseGeographyPanel.Bind(_doseLedger); },
                openAction: () => _doseGeographyPanel.Open(),
                closeAction: () => _doseGeographyPanel.Close());

            PanelRegistry.ConfigureActions("caravan_barter",
                openAction: () => _caravanBarterLedgerPanel.Open(),
                closeAction: () => ClosePanelAnimated(_caravanBarterLedgerPanel));

            PanelRegistry.ConfigureActions("shelter_barter",
                bindAction: () =>
                {
                    SetupInventory();
                    EnsureShelterBarterPanel().Bind(
                        EnsureShelterBarter(),
                        _inventory!.Inventory,
                        _journal,
                        id => _inventory!.Catalog?.Get(id));
                },
                openAction: () => OpenShelterBarterPanel(),
                closeAction: () => { if (_shelterBarterPanel != null) _shelterBarterPanel.Close(); });

            PanelRegistry.ConfigureActions("black_projects_archive",
                bindAction: () =>
                {
                    EnsureBlackProjectsArchivePanel().Bind(
                        EnsureBlackProjectsArchive(),
                        _journal);
                },
                openAction: () => OpenBlackProjectsArchivePanel(),
                closeAction: () => CloseBlackProjectsArchivePanel());

            PanelRegistry.ConfigureActions("faction_matrix",
                bindAction: () => _factionMatrixPanel.Bind(EnsureSharedFactionStance()),
                openAction: () => _factionMatrixPanel.Open(),
                closeAction: () => ClosePanelAnimated(_factionMatrixPanel));

            PanelRegistry.ConfigureActions("factions_narrative",
                bindAction: () => _factionsNarrativePanel.Bind(EnsureSharedFactionStance()),
                openAction: () => _factionsNarrativePanel.Open(),
                closeAction: () => ClosePanelAnimated(_factionsNarrativePanel));

            PanelRegistry.ConfigureActions("faction_communique_board",
                bindAction: () => { SetupYearOfAsh(); _communiqueBoardPanel.Bind(_yearOfAsh, _simDay); },
                openAction: () => _communiqueBoardPanel.Open(),
                closeAction: () => ClosePanelAnimated(_communiqueBoardPanel));

            PanelRegistry.ConfigureActions("skill_matrix",
                bindAction: () => { SetupSurvivors(); _skillMatrixPanel.Bind(EnsureSharedSkillProgression(), _survivors); },
                openAction: () => _skillMatrixPanel.Open(),
                closeAction: () => ClosePanelAnimated(_skillMatrixPanel));

            PanelRegistry.ConfigureActions("survival_workstation",
                bindAction: () => { SetupCrafting(); SetupInventory(); SetupSurvivors(); _survivalWorkstationPanel.Bind(_crafting, _inventory, _survivors); },
                openAction: () => _survivalWorkstationPanel.Open(),
                closeAction: () => ClosePanelAnimated(_survivalWorkstationPanel));

            PanelRegistry.ConfigureActions("verdict_dashboard",
                bindAction: () => { SetupVerdict(); _verdictDashboardPanel.Bind(_verdictPanel, _verdict); },
                openAction: () => _verdictDashboardPanel.Open(),
                closeAction: () => ClosePanelAnimated(_verdictDashboardPanel));

            PanelRegistry.ConfigureActions("map_atlas",
                bindAction: () => { SetupExpeditions(); SetupWorld(); _mapAtlasPanel.Bind(_expeditions, _world); },
                openAction: () => _mapAtlasPanel.Open(),
                closeAction: () => ClosePanelAnimated(_mapAtlasPanel));

            PanelRegistry.ConfigureActions("maritime_atlas",
                bindAction: () => { SetupMaritime(); if (_maritime != null) _maritimeAtlasPanel.Bind(_maritime); },
                openAction: () => _maritimeAtlasPanel.Open(),
                closeAction: () => ClosePanelAnimated(_maritimeAtlasPanel));

            PanelRegistry.ConfigureActions("muster_atlas",
                bindAction: () => { SetupMuster(); if (_muster != null) _musterAtlasPanel.Bind(_muster); },
                openAction: () => _musterAtlasPanel.Open(),
                closeAction: () => ClosePanelAnimated(_musterAtlasPanel));

            PanelRegistry.ConfigureActions("quests_atlas",
                bindAction: () => { SetupHoldfastRuntime(); SetupExpansions(); _questsAtlasPanel.Bind(_core.Quests, _expansions?.CrossingQuests); },
                openAction: () => _questsAtlasPanel.Open(),
                closeAction: () => ClosePanelAnimated(_questsAtlasPanel));

            PanelRegistry.ConfigureActions("research_atlas",
                bindAction: () =>
                {
                    _researchHostSession ??= ResearchHostSession.Create(_dataDir, EnsureSharedResearch());
                    _researchHostSession.StartResearchGate = () =>
                        _powerGrid?.System?.IsRoomPowered("room_laboratory_research") ?? true;
                    _researchAtlasPanel.Bind(_researchHostSession);
                },
                openAction: () => _researchAtlasPanel.Open(),
                closeAction: () => ClosePanelAnimated(_researchAtlasPanel));

            PanelRegistry.ConfigureActions("standing_record_atlas",
                bindAction: () => { _standingRecordHostSession ??= StandingRecordHostSession.Create(_dataDir); _standingRecordAtlasPanel.Bind(_standingRecordHostSession); },
                openAction: () => _standingRecordAtlasPanel.Open(),
                closeAction: () => ClosePanelAnimated(_standingRecordAtlasPanel));

            PanelRegistry.ConfigureActions("combat_hud",
                bindAction: () => { if (_combat != null) _combatHudOverlay.Bind(_combat); },
                openAction: () => _combatHudOverlay.Open(),
                closeAction: () => ClosePanelAnimated(_combatHudOverlay));

            // ── Plans 178-201 Expansion Surfaces ─────────────────────────
            PanelRegistry.ConfigureActions("aviation",
                bindAction: () => { SetupAviation(); _aviationPanel.Bind(EnsureAviation()); },
                openAction: () => _aviationPanel.Open(),
                closeAction: () => _aviationPanel.Close());

            PanelRegistry.ConfigureActions("narcotics",
                bindAction: () => { SetupNarcotics(); _chemPanel.Bind(EnsureNarcotics()); },
                openAction: () => _chemPanel.Open(),
                closeAction: () => _chemPanel.Close());

            PanelRegistry.ConfigureActions("forced_labor",
                bindAction: () => { SetupForcedLabor(); _laborPanel.Bind(EnsureForcedLabor()); },
                openAction: () => _laborPanel.Open(),
                closeAction: () => _laborPanel.Close());

            PanelRegistry.ConfigureActions("politics",
                bindAction: () => { SetupPolitics(); _politicsPanel.Bind(EnsurePolitics()); },
                openAction: () => _politicsPanel.Open(),
                closeAction: () => _politicsPanel.Close());

            PanelRegistry.ConfigureActions("prisoners",
                bindAction: () => { SetupPrisoners(); _prisonerPanel.Bind(EnsurePrisoners()); },
                openAction: () => _prisonerPanel.Open(),
                closeAction: () => _prisonerPanel.Close());

            PanelRegistry.ConfigureActions("stealth",
                bindAction: () => { SetupStealth(); _stealthReadoutPanel.Bind(EnsureStealth()); },
                openAction: () => _stealthReadoutPanel.Open(),
                closeAction: () => _stealthReadoutPanel.Close());

            PanelRegistry.ConfigureActions("mutation_tree",
                bindAction: () => { SetupMutations(); _mutationTreePanel.Bind(EnsureMutations()); },
                openAction: () => _mutationTreePanel.Open(),
                closeAction: () => _mutationTreePanel.Close());

            PanelRegistry.ConfigureActions("nursery",
                bindAction: () => { SetupGenerational(); _nurseryPanel.Bind(EnsureGenerational()); },
                openAction: () => _nurseryPanel.Open(),
                closeAction: () => _nurseryPanel.Close());

            PanelRegistry.ConfigureActions("fallout_detail",
                bindAction: () => { SetupFallout(); _falloutPlumePanel.Bind(EnsureFallout()); },
                openAction: () => _falloutPlumePanel.Open(),
                closeAction: () => _falloutPlumePanel.Close());

            // ── Flagship Consoles (Shelter Systems) ───────────────────────────
            PanelRegistry.ConfigureActions("slurry_dewatering_sump",
                bindAction: () => { SetupSumpFlooding(); _slurryDewateringSumpPanel.Bind(_sumpFlooding); },
                openAction: () => _slurryDewateringSumpPanel.Open(),
                closeAction: () => ClosePanelAnimated(_slurryDewateringSumpPanel));

            PanelRegistry.ConfigureActions("electrostatic_scrubber",
                bindAction: () =>
                {
                    SetupExpandedShelterSystems();
                    if (_ventilationHost != null)
                        _electrostaticScrubberPanel.Bind(_ventilationHost);
                },
                openAction: () => _electrostaticScrubberPanel.Open(),
                closeAction: () => ClosePanelAnimated(_electrostaticScrubberPanel));

            PanelRegistry.ConfigureActions("plans_94_97",
                bindAction: () =>
                {
                    SetupGrainProcessing();
                    SetupCryogenicAirSeparation();
                    SetupHeliograph();
                    SetupPlans94To97Panel();
                },
                openAction: () => OpenExpandedPanel("plans_94_97"),
                closeAction: () => { if (_plans94To97Panel != null) _plans94To97Panel.Close(); });

            PanelRegistry.ConfigureActions("plans_130_133",
                bindAction: () =>
                {
                    SetupPlans130To133();
                },
                openAction: () => OpenExpandedPanel("plans_130_133"),
                closeAction: () => { if (_plans130To133Panel != null) _plans130To133Panel.Close(); });

            // Plans 162-165 — advanced agriculture (Plan 162). Bind-on-open keeps
            // the session lazy; the panel routes commands through HandleAgricultureAction.
            PanelRegistry.ConfigureActions("farming",
                bindAction: () => HandleAgricultureAction("OPEN", ""),
                openAction: () => HandleAgricultureAction("OPEN", ""),
                closeAction: () => HandleAgricultureAction("CLOSE", ""));

            // Plans 162-165 — settlement defense grid (Plan 163).
            PanelRegistry.ConfigureActions("defense_grid",
                bindAction: () => HandleDefenseAction("OPEN", ""),
                openAction: () => HandleDefenseAction("OPEN", ""),
                closeAction: () => HandleDefenseAction("CLOSE", ""));

            // Plans 162-165 — psychological breakdown arcs (Plan 164).
            PanelRegistry.ConfigureActions("psychology_arcs",
                bindAction: () => HandlePsychologyAction("OPEN", ""),
                openAction: () => HandlePsychologyAction("OPEN", ""),
                closeAction: () => HandlePsychologyAction("CLOSE", ""));

            // Plans 162-165 — wasteland bestiary (Plan 165).
            PanelRegistry.ConfigureActions("bestiary",
                bindAction: () => HandleWildlifeAction("OPEN", ""),
                openAction: () => HandleWildlifeAction("OPEN", ""),
                closeAction: () => HandleWildlifeAction("CLOSE", ""));

            // Note: 29 flagship prototype consoles (Issues 01–28, 30) are registered as
            // PanelMaturity.Prototype and excluded from player navigation. Their classes
            // remain available for snapshots, previews, and future host session development.

            // Expanded Panels
            string[] expandedIds =
            {
                "water_treatment", "airlock_security", "survivor_relations", "regional_treaty",
                "vinyl_morale", "wildlife_trapping", "excavation", "apprenticeship",
                "caregiving", "shelter_thermal", "shelter_schedule", "shelter_decor", "shelter_atmosphere", "hidden_agenda", "shelter_reputation", "autopsy_report",
                "waystation_network", "chemical_dependency", "sump_flooding", "decontamination",
                "kitchen_nutrition", "equipment_condition", "library_study", "archive_desk",
                "contractor_roster", "mental_health_crisis", "phantom_memory",
                "traveling_caravan", "medical_ward", "plans_94_97", "plans_130_133",
                "farming", "defense_grid", "psychology_arcs", "bestiary",
                "low_background_metrology", "insar_mapping", "hydraulic_extrusion", "runflat_tire",
                "sofc_power", "sound_ranging", "night_watch", "cvd_diamond", "amphibious_draisine",
                "shelter_operations",
                "sanitation", "black_market",
                "companion_kennel", "beliefs_panel", "anomaly_watch", "cybernetics",
                "sky_defense_battery",
                "dynamic_quests",
                "vehicle_garage",
                "propaganda",
                "rumors",
                "shelter_security",
                "visitor_integration",
                "personal_belongings",
                "personal_quests",
                "time_capsule",
                "death_legacy",
                "relationship_decay"
            };

            foreach (var expId in expandedIds)
            {
                string id = expId;
                PanelRegistry.ConfigureActions(id,
                    openAction: () => OpenExpandedPanel(id));
            }

            // ── UI/UX audit 2026-09-25 — prototype console re-route ──────────────
            // Twenty previously-shelved console ids are now Live and resolve to
            // the live surface that owns their domain (one authority per concern:
            // the shelved shells display static fixture text and were never
            // session-bound — showing them directly would be a fake operational
            // route). iron_cenotaph_memorial opens its own panel, which is already
            // bound to the live mourning projection (Plan 24 A3). Nine ids remain
            // prototype pending a foreman-approved target surface
            // (docs/ui/UI_UX_AUDIT_2026-09-25.md).
            PanelRegistry.ConfigureActions("iron_cenotaph_memorial",
                openAction: () => _ironCenotaphMemorialPanel.Open());
            RedirectPrototypeRoute("biogas_digester", "bio_fermentation");
            RedirectPrototypeRoute("fungal_protein_fermenter", "bio_fermentation");
            RedirectPrototypeRoute("geothermal_turbine", "geothermal_orc");
            RedirectPrototypeRoute("heavy_marine_diesel_gen", "power_grid");
            RedirectPrototypeRoute("induction_cupola_furnace", "silent_foundry");
            RedirectPrototypeRoute("plasma_smelting", "silent_foundry");
            RedirectPrototypeRoute("logistics_airlock", "airlock_security");
            RedirectPrototypeRoute("long_walk_expedition", "expeditions");
            RedirectPrototypeRoute("mechanical_prosthetics_lathe", "cybernetics");
            RedirectPrototypeRoute("printing_press", "propaganda");
            RedirectPrototypeRoute("subterranean_debt_ledger", "black_market");
            RedirectPrototypeRoute("war_dog_kennel", "companion_kennel");
            RedirectPrototypeRoute("ultrasonic_decontam_airlock", "decontamination");
            RedirectPrototypeRoute("trauma_bonding_cohort", "survivor_relations");
            RedirectPrototypeRoute("crossing_safe_conduct_vouch", "crossing_quests");
            RedirectPrototypeRoute("cartography_gis", "map_atlas");
            RedirectPrototypeRoute("magnetic_drum_archive", "archive_desk");
            RedirectPrototypeRoute("surface_shrapnel_aegis", "sky_defense_battery");
            RedirectPrototypeRoute("clandestine_insurgency", "factions");

            // ── UI/UX audit 2026-09-25 — third batch, aquifer seal + conversions ──
            // geothermal_aquifer: bound panel + handler existed with no route.
            // The two redirects resolve to the live surface owning the domain.
            // The three conversions bind their previously-static shells to the
            // live host authorities (YearOfAsh radon, SeismicDynamics, CryoVault).
            PanelRegistry.ConfigureActions("geothermal_aquifer",
                bindAction: () => SetupGeothermalAquifer(),
                openAction: () => HandleGeothermalAction("OPEN"),
                closeAction: () => HandleGeothermalAction("CLOSE"));

            RedirectPrototypeRoute("aquifer_treaty_concession", "geothermal_aquifer");
            RedirectPrototypeRoute("vault_door_breaching", "combat");

            PanelRegistry.ConfigureActions("basal_radon_migration",
                bindAction: () => { SetupYearOfAsh(); _basalRadonMigrationPanel.Bind(_yearOfAsh); },
                openAction: () => _basalRadonMigrationPanel.Open(),
                closeAction: () => ClosePanelAnimated(_basalRadonMigrationPanel));
            PanelRegistry.ConfigureActions("borehole_seismograph",
                bindAction: () => { SetupSeismicDynamics(); _boreholeSeismographPanel.Bind(_seismicDynamics); },
                openAction: () => _boreholeSeismographPanel.Open(),
                closeAction: () => ClosePanelAnimated(_boreholeSeismographPanel));
            PanelRegistry.ConfigureActions("cryo_permafrost_core",
                bindAction: () => { SetupCryoVault(); _cryoPermafrostCorePanel.Bind(_cryoVault); },
                openAction: () => _cryoPermafrostCorePanel.Open(),
                closeAction: () => ClosePanelAnimated(_cryoPermafrostCorePanel));

            PanelRegistry.ConfigureActions("romance_family_board",
                bindAction: BindRomanceFamilyBoard,
                openAction: () => _romanceFamilyBoard?.Open(),
                closeAction: () => ClosePanelAnimated(_romanceFamilyBoard));
            PanelRegistry.ConfigureActions("colony_operations",
                bindAction: BindColonyOperationsBoard,
                openAction: () => _colonyOperationsBoard?.Open(),
                closeAction: () => ClosePanelAnimated(_colonyOperationsBoard));
            PanelRegistry.ConfigureActions("ideological_mediation_desk",
                bindAction: BindIdeologicalMediationDesk,
                openAction: () => _ideologicalMediationDesk?.Open(),
                closeAction: () => ClosePanelAnimated(_ideologicalMediationDesk));

            // Construct before the central lifecycle/accessibility walk so the
            // three board roots join overlay close, focus, and motion handling.
            EnsurePfglOctetBoardPanels();

            // ── Central accessibility-gated open motion (UI/UX audit 2026-09-25) ──
            // Every overlay root the player can open animates its entrance through
            // one shared seam: the 167 IBindablePanel surfaces, the modal family,
            // and every other direct child of the UI host (dashboards/overlays).
            // The motion gate is the existing ReducedMotion authority
            // (AccessibilityPresentation), auto-disabled under --headless and while
            // SnapshotOrchestrator captures, so goldens never see a fade. Closing
            // stays synchronous — Esc dismissal is never delayed by motion.
            AshfallUiTheme.Install(this);
            RegisterOpenMotionRecursive(this, isHostRoot: true);
        }

        /// <summary>
        /// Registers the shared full-scale entrance animation on every visible
        /// overlay root. Direct children of the UI host are overlay roots by
        /// construction (player panels, dashboards, full-screen modals); nested
        /// IBindablePanel/IModalPanel controls are registered too so embedded
        /// surfaces animate when they open.
        /// </summary>
        private void RegisterOpenMotionRecursive(Control node, bool isHostRoot)
        {
            foreach (var child in node.GetChildren())
            {
                if (child is Button fxButton)
                    UiMotion.AttachButtonFx(fxButton);

                if (child is Control control && control is not Button &&
                    (isHostRoot || control is IBindablePanel || control is AtomicWar.GodotApp.UI.IModalPanel))
                {
                    Control captured = control;
                    captured.VisibilityChanged += () =>
                    {
                        if (captured.Visible)
                        {
                            _lastVisibilityGranted = captured;
                            UiMotion.AnimateOpen(captured);
                            EnsureInitialFocus(captured);
                        }
                    };
                }

                if (child is Control childControl)
                    RegisterOpenMotionRecursive(childControl, isHostRoot: false);
            }
        }

        /// <summary>
        /// Contextual first-hour lessons (2026-09-25): event-driven nudges through
        /// the persisted onboarding authority. Each fires at most once per campaign
        /// and only when the real runtime state warrants it.
        /// </summary>
        private Ashfall.Core.World.WeatherSystem? _weatherLessonSubscription;
        private Ashfall.Core.Expeditions.ExpeditionSystem? _dispatchLessonSubscription;

        /// <summary>Subscribes once to the live weather system (re-subscribes if the session swaps).</summary>
        private void EnsureWeatherLessonHook()
        {
            if (_world == null)
                return;
            var weather = _world.Weather;
            if (ReferenceEquals(weather, _weatherLessonSubscription))
                return;
            if (_weatherLessonSubscription != null)
                _weatherLessonSubscription.OnWeatherChanged -= OnLessonWeatherChanged;
            _weatherLessonSubscription = weather;
            weather.OnWeatherChanged += OnLessonWeatherChanged;
        }

        private void OnLessonWeatherChanged(Ashfall.Core.WeatherKind kind)
        {
            if (_world != null && _world.IsSevereWeather(kind))
                MaybeRequestSevereWeatherLesson();
        }

        /// <summary>Subscribes once to the live expedition engine (re-subscribes if the session swaps).</summary>
        private void EnsureDispatchLessonHook()
        {
            if (_expeditions?.Engine == null)
                return;
            var engine = _expeditions.Engine;
            if (ReferenceEquals(engine, _dispatchLessonSubscription))
                return;
            if (_dispatchLessonSubscription != null)
                _dispatchLessonSubscription.OnExpeditionStarted -= OnLessonExpeditionStarted;
            _dispatchLessonSubscription = engine;
            engine.OnExpeditionStarted += OnLessonExpeditionStarted;
        }

        private void OnLessonExpeditionStarted(Ashfall.Core.Expeditions.ExpeditionState _) =>
            MaybeRequestProtectionLesson();

        private void MaybeRequestProtectionLesson()
        {
            SetupOnboarding();
            EnsureDispatchLessonHook();
            if (_onboardingJourney == null || _expeditions == null || _expeditions.Definitions.Count == 0)
                return;
            var definition = _expeditions.Definitions[0];
            var estimate = _expeditions.EstimateExpedition(
                definition.id, Ashfall.Core.Expeditions.ExpeditionStance.Stealth);
            if (estimate.HasValue && estimate.Value.estimate.unprotectedCount > 0)
            {
                _onboardingJourney.RequestContextualTutorial(
                    Ashfall.Core.Localization.OnboardingLessonLocalization.ProtectionBeforeDispatchId);
            }
        }

        private void MaybeRequestSevereWeatherLesson()
        {
            SetupOnboarding();
            EnsureWeatherLessonHook();
            if (_onboardingJourney == null || _world == null)
                return;
            if (_world.IsSevereWeather(_world.Weather.Current))
            {
                _onboardingJourney.RequestContextualTutorial(
                    Ashfall.Core.Localization.OnboardingLessonLocalization.SevereWeatherPrepId);
            }
        }

        /// <summary>Last overlay root that became visible during an open call.</summary>
        private Control? _lastVisibilityGranted;

        /// <summary>
        /// Keyboard/controller alpha usability (UI/UX audit follow-up): when a
        /// panel opens, focus its first interactive control once layout settles,
        /// so D-pad/arrows navigate from inside the panel immediately. Deferred
        /// and guarded — a panel (or modal) that already chose a focus owner
        /// keeps it, and the focus ring only appears for keyboard/controller use.
        /// </summary>
        private static void EnsureInitialFocus(Control panel)
        {
            Callable.From(() =>
            {
                if (!GodotObject.IsInstanceValid(panel) || !panel.Visible)
                    return;
                var owner = panel.GetViewport()?.GuiGetFocusOwner();
                if (owner != null && GodotObject.IsInstanceValid(owner) && panel.IsAncestorOf(owner))
                    return; // the panel already focused something deliberately
                var first = AtomicWar.GodotApp.UI.AshfallFocusPolicy.FindFirstFocusable(panel);
                if (first != null && GodotObject.IsInstanceValid(first))
                    first.GrabFocus();
            }).CallDeferred();
        }

        /// <summary>
        /// Route → overlay-root map learned lazily from visibility grants. Used
        /// only to cancel an in-flight exit transition when a route is re-opened
        /// before its close fade completes.
        /// </summary>
        private readonly System.Collections.Generic.Dictionary<string, Control> _openControlById = new();

        /// <summary>
        /// Host close seam (UI/UX audit 2026-09-25 follow-up): plays the shared
        /// exit transition when motion is available, else hides immediately.
        /// </summary>
        private static void ClosePanelAnimated(Control? panel)
        {
            if (panel == null || !GodotObject.IsInstanceValid(panel))
                return;
            if (!UiMotion.AnimateClose(panel))
                panel.Visible = false;
        }

        /// <summary>Closes a route's control with the shared exit transition when known.</summary>
        private void CloseRouteAnimated(string targetId)
        {
            if (_openControlById.TryGetValue(targetId, out var control) &&
                control != null && GodotObject.IsInstanceValid(control) &&
                UiMotion.AnimateClose(control))
            {
                return;
            }
            PanelRegistry.TryClose(targetId);
        }

        /// <summary>
        /// Routes a promoted prototype console id to the live surface that owns
        /// its domain (UI/UX audit 2026-09-25). Opening re-enters the canonical
        /// OpenPlayerPanel path so the target opens exactly as it would from its
        /// own navigation entry (availability, menu gating, and fallback wiring
        /// all apply); the AudioManager cue cooldown absorbs the second confirm.
        /// </summary>
        private void RedirectPrototypeRoute(string sourceId, string targetId)
        {
            PanelRegistry.ConfigureActions(sourceId,
                openAction: () => OpenPlayerPanel(targetId),
                closeAction: () => CloseRouteAnimated(targetId));
        }

        /// <summary>
        /// Live dosimeter selection for the geiger calibration surface: the
        /// ordinal-first device tag registered in the campaign dose ledger, or an
        /// explicit empty selection when no device is registered. Never a demo
        /// tag (INV-16.4); the panel renders its explicit no-device state when
        /// the selection is empty (N16.6).
        /// </summary>
        private string ResolveLiveDosimeterTag()
        {
            var devices = _doseLedger?.Calibration?.Devices;
            if (devices == null || devices.Count == 0) return string.Empty;
            string selected = string.Empty;
            foreach (var tag in devices.Keys)
            {
                if (string.IsNullOrEmpty(selected) || string.CompareOrdinal(tag, selected) < 0)
                    selected = tag;
            }
            return selected;
        }

        /// <summary>
        /// Live signal selection for the triangulation surface: the
        /// ordinal-first active distress signal in the campaign radio, or an
        /// explicit empty selection when no signal is active. Never a canned
        /// signal id (INV-16.4); the panel renders its explicit no-signal state
        /// when the selection is empty (N16.6).
        /// </summary>
        private string ResolveLiveTriangulationSignalId()
        {
            var signals = _radio?.DistressSystem?.ActiveSignals;
            if (signals == null) return string.Empty;
            string selected = string.Empty;
            foreach (var signal in signals)
            {
                if (signal == null || string.IsNullOrEmpty(signal.SignalId)) continue;
                if (string.IsNullOrEmpty(selected) || string.CompareOrdinal(signal.SignalId, selected) < 0)
                    selected = signal.SignalId;
            }
            return selected;
        }

        private ResearchHostSession? _researchHostSession;
        private StandingRecordHostSession? _standingRecordHostSession;
    }
}
