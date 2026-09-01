using Godot;
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.UI;

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
                bindAction: () => _tutorialPanel.Bind(_simDay),
                openAction: () => _tutorialPanel.Open(),
                closeAction: () => CloseTutorialPanel());

            PanelRegistry.ConfigureActions("afflictions",
                bindAction: () => { SetupSurvivors(); SetupInventory(); SetupMedical(); SetupPhase0(); _afflictionsPanel.Bind(_medical, _survivors, _inventory, _phase0?.Respiratory); },
                openAction: () => _afflictionsPanel.Open(),
                closeAction: () => CloseAfflictionsPanel());

            PanelRegistry.ConfigureActions("radiation_detail",
                bindAction: () => { SetupSurvivors(); SetupPhase0(); _radiationDetailPanel.Bind(_doseLedger, _survivors); },
                openAction: () => _radiationDetailPanel.Open(),
                closeAction: () => CloseRadiationDetailPanel());

            PanelRegistry.ConfigureActions("research",
                bindAction: () => { _sharedResearch = EnsureSharedResearch(); _researchPanel.Bind(_sharedResearch); },
                openAction: () => _researchPanel.Open(),
                closeAction: () => CloseResearchPanel());

            PanelRegistry.ConfigureActions("weather_detail",
                bindAction: () => { SetupWorld(); _weatherDetailPanel.Bind(_world?.Weather); },
                openAction: () => _weatherDetailPanel.Open(),
                closeAction: () => CloseWeatherDetailPanel());

            PanelRegistry.ConfigureActions("weather_forecast",
                bindAction: () => { SetupWorld(); _weatherForecastPanel.Bind(_world?.Weather); },
                openAction: () => _weatherForecastPanel.Open(),
                closeAction: () => CloseWeatherForecastPanel());

            PanelRegistry.ConfigureActions("event_detail",
                bindAction: () => { SetupEventsHost(); _eventDetailPanel.Bind(_eventsHost); },
                openAction: () => _eventDetailPanel.Open(),
                closeAction: () => CloseEventDetailPanel());

            PanelRegistry.ConfigureActions("events_log",
                bindAction: () => { SetupEventsHost(); _eventsLogPanel.Bind(_eventsHost); },
                openAction: () => _eventsLogPanel.Open(),
                closeAction: () => CloseEventsLogPanel());

            PanelRegistry.ConfigureActions("economy_detail",
                bindAction: () => { SetupEconomy(); _economyDetailPanel.Bind(_economy); },
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
                bindAction: () => { SetupSurvivors(); var first = _survivors?.RosterState?.FirstOrDefault(s => s != null)?.Id ?? ""; _survivorDetailPanel.Bind(_survivors, first); },
                openAction: () => _survivorDetailPanel.Open(),
                closeAction: () => CloseSurvivorDetailPanel());

            PanelRegistry.ConfigureActions("inventory_detail",
                bindAction: () => { SetupInventory(); var first = _inventory?.Inventory?.FindSlot("bandage")?.Item?.id ?? "bandage"; _inventoryDetailPanel.Bind(_inventory, first); },
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
                bindAction: () => { SetupCrafting(); SetupInventory(); _craftingPanel.Bind(_crafting, _inventory); },
                openAction: () => _craftingPanel.Open(),
                closeAction: () => CloseCraftingPanel());

            PanelRegistry.ConfigureActions("workshop",
                bindAction: () => { SetupCrafting(); SetupInventory(); SetupSurvivors(); _workshopPanel.Bind(_crafting.Workshop, _inventory.Inventory, _survivors); },
                openAction: () => _workshopPanel.Open(),
                closeAction: () => CloseWorkshopPanel());

            PanelRegistry.ConfigureActions("pharma_lab",
                bindAction: () => { SetupCrafting(); SetupInventory(); SetupSurvivors(); SetupMentalHealthCrisis(); _pharmaLabPanel.Bind(_crafting.PharmaLab, _inventory.Inventory, _chemicalDependency?.System, _survivors); },
                openAction: () => _pharmaLabPanel.Open(),
                closeAction: () => ClosePharmaLabPanel());

            PanelRegistry.ConfigureActions("pharma",
                bindAction: () => { SetupCrafting(); SetupInventory(); SetupSurvivors(); SetupMentalHealthCrisis(); _pharmaLabPanel.Bind(_crafting.PharmaLab, _inventory.Inventory, _chemicalDependency?.System, _survivors); },
                openAction: () => _pharmaLabPanel.Open(),
                closeAction: () => ClosePharmaLabPanel());

            PanelRegistry.ConfigureActions("medical",
                bindAction: () => { SetupSurvivors(); SetupInventory(); SetupMedical(); SetupPhase0(); EnsureMedicalPipeline(); _medicalPanel.Bind(_medical, _survivors, _inventory, _phase0?.Respiratory); },
                openAction: () => _medicalPanel.Open(),
                closeAction: () => CloseMedicalPanel());

            PanelRegistry.ConfigureActions("phase0",
                openAction: () => OpenPhase0Panel(),
                closeAction: () => ClosePhase0Panel());

            PanelRegistry.ConfigureActions("expeditions",
                bindAction: () => { SetupExpeditions(); SetupExpansions(); _expeditions.CrossingGate = _expansions.Vouch; SetupSurvivors(); SetupInventory(); SetupWorld(); SetupEvolvingWorldInfluence(); _expeditionPanel.Bind(_expeditions, _survivors, _inventory, _equipmentCondition?.System, _world); },
                openAction: () => _expeditionPanel.Open(),
                closeAction: () => CloseExpeditionPanel());

            PanelRegistry.ConfigureActions("weather",
                bindAction: () => { SetupWorld(); _weatherPanel.Bind(_world); },
                openAction: () => _weatherPanel.Open(),
                closeAction: () => CloseWeatherPanel());

            PanelRegistry.ConfigureActions("radio",
                bindAction: () => { SetupRadio(); _radioPanel.Bind(_radio); },
                openAction: () => _radioPanel.Open(),
                closeAction: () => CloseRadioPanel());

            PanelRegistry.ConfigureActions("map",
                bindAction: () => { SetupHoldfastRuntime(); SetupExpeditions(); SetupExpansions(); SetupWorld(); SetupJournal(); SetupDeepCoast(); SetupYearOfAsh(); _mapPanel.Bind(_core, _expeditions, _expansions, _world, _journalCodex?.Catalogs, _deepCoast, _yearOfAsh); },
                openAction: () => _mapPanel.Open(),
                closeAction: () => CloseMapPanel());

            PanelRegistry.ConfigureActions("map_detail",
                openAction: () => _mapDetailPanel.Open(),
                closeAction: () => CloseMapDetailPanel());

            PanelRegistry.ConfigureActions("shelter",
                bindAction: () => { SetupSurvivors(); SetupWorld(); SetupInventory(); _shelterPanel.Bind(_survivors, _world, _inventory, GetShelterRoomIdentityCatalog()); _shelterPanel.SetMachineTellCatalog(GetMachineTellCatalog()); },
                openAction: () => _shelterPanel.Open(),
                closeAction: () => CloseShelterPanel());

            PanelRegistry.ConfigureActions("factions",
                bindAction: () => { SetupHoldfastRuntime(); SetupMuster(); SetupExpansions(); SetupYearOfAsh(); SetupFactionBranch(); SetupMoralChoice(); _factionsPanel.Bind(_core.Catalog.Factions, _holdfastRuntime?.Trade, _muster, _expansions, _yearOfAsh, _factionBranch?.Coordinator, _moralChoice); },
                openAction: () => _factionsPanel.Open(),
                closeAction: () => CloseFactionsPanel());

            PanelRegistry.ConfigureActions("faction_detail",
                openAction: () => _factionDetailPanel.Open(),
                closeAction: () => CloseFactionDetailPanel());

            PanelRegistry.ConfigureActions("quests",
                bindAction: () => { SetupHoldfastRuntime(); SetupExpansions(); SetupDutyRoster(); SetupFactionBranch(); SetupMoralChoice(); _questsPanel.Bind(_core.Quests, _expansions?.CrossingQuests, _dutyRoster, _holdfastRuntime?.Day ?? _simDay, _factionBranch?.Coordinator, _moralChoice); },
                openAction: () => _questsPanel.Open(),
                closeAction: () => CloseQuestsPanel());

            PanelRegistry.ConfigureActions("quest_detail",
                openAction: () => _questDetailPanel.Open(),
                closeAction: () => CloseQuestDetailPanel());

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
                bindAction: () => { SetupExpansions(); SetupSurvivors(); _epiloguePanel.Bind(_simDay, _survivors?.RosterState?.Count ?? 4, 0, true, true, true, true, true); },
                openAction: () => _epiloguePanel.Open(),
                closeAction: () => CloseEpiloguePanel());

            PanelRegistry.ConfigureActions("verdict",
                bindAction: () => { SetupVerdict(); _verdictPanel.Bind(_verdict); },
                openAction: () => _verdictPanel.Open(),
                closeAction: () => CloseVerdictPanel());

            PanelRegistry.ConfigureActions("holdfast",
                bindAction: () => { SetupHoldfastRuntime(); if (_holdfastTerminal != null) { _holdfastTerminal.BindSession(_holdfastRuntime); } },
                openAction: () => { if (_holdfastTerminal != null) _holdfastTerminal.OpenTerminal(); },
                closeAction: () => { if (_holdfastTerminal != null) _holdfastTerminal.Visible = false; });

            PanelRegistry.ConfigureActions("duty_roster",
                bindAction: () => { SetupDutyRoster(); SetupSurvivors(); _dutyRosterPanel.Bind(_dutyRoster, _survivors); },
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
                closeAction: () => _brineExtractionPanel.Visible = false);

            PanelRegistry.ConfigureActions("expedition_camp",
                bindAction: () => { SetupExpeditions(); SetupSurvivors(); string survId = _survivors?.RosterState?.FirstOrDefault()?.Id ?? "surv_01"; _expeditionCampPanel.Bind(_expeditions, survId); },
                openAction: () => _expeditionCampPanel.Open(),
                closeAction: () => _expeditionCampPanel.Visible = false);

            PanelRegistry.ConfigureActions("fire_incident",
                bindAction: () => _fireIncidentPanel.Bind(new Ashfall.Core.Shelter.ShelterFireHazardSystem(), "inc_default"),
                openAction: () => _fireIncidentPanel.Open(),
                closeAction: () => _fireIncidentPanel.Visible = false);

            PanelRegistry.ConfigureActions("geiger_calibration",
                bindAction: () => { SetupPhase0(); _geigerCalibrationPanel.Bind(_doseLedger, "tag_1"); },
                openAction: () => _geigerCalibrationPanel.Open(),
                closeAction: () => _geigerCalibrationPanel.Visible = false);

            PanelRegistry.ConfigureActions("triangulation",
                bindAction: () => { SetupRadio(); _triangulationPanel.Bind(_radio, "sig_distress"); },
                openAction: () => _triangulationPanel.Open(),
                closeAction: () => _triangulationPanel.Visible = false);

            PanelRegistry.ConfigureActions("weather_sonde",
                bindAction: () => { SetupWorld(); _weatherSondePanel.Bind(new WeatherHostSession(_world?.Weather)); },
                openAction: () => _weatherSondePanel.Open(),
                closeAction: () => _weatherSondePanel.Visible = false);

            PanelRegistry.ConfigureActions("power_grid",
                bindAction: () => { if (_powerGrid != null) _powerGridPanel.Bind(_powerGrid); },
                openAction: () => _powerGridPanel.Open(),
                closeAction: () => _powerGridPanel.Visible = false);

            PanelRegistry.ConfigureActions("expedition_radar",
                bindAction: () => { SetupExpeditions(); SetupSurvivors(); _expeditionRadarPanel.Bind(_expeditions, _survivors); },
                openAction: () => _expeditionRadarPanel.Open(),
                closeAction: () => _expeditionRadarPanel.Visible = false);

            PanelRegistry.ConfigureActions("dose_ledger",
                bindAction: () => { SetupPhase0(); SetupSurvivors(); _doseLedgerPanel.Bind(_doseLedger, _survivors); },
                openAction: () => _doseLedgerPanel.Open(),
                closeAction: () => _doseLedgerPanel.Visible = false);

            PanelRegistry.ConfigureActions("caravan_barter",
                openAction: () => _caravanBarterLedgerPanel.Open(),
                closeAction: () => _caravanBarterLedgerPanel.Visible = false);

            PanelRegistry.ConfigureActions("faction_matrix",
                bindAction: () => _factionMatrixPanel.Bind(new Ashfall.Core.Economy.FactionStanceEngine()),
                openAction: () => _factionMatrixPanel.Open(),
                closeAction: () => _factionMatrixPanel.Visible = false);

            PanelRegistry.ConfigureActions("factions_narrative",
                bindAction: () => _factionsNarrativePanel.Bind(new Ashfall.Core.Economy.FactionStanceEngine()),
                openAction: () => _factionsNarrativePanel.Open(),
                closeAction: () => _factionsNarrativePanel.Visible = false);

            PanelRegistry.ConfigureActions("skill_matrix",
                bindAction: () => { SetupSurvivors(); _skillMatrixPanel.Bind(new Ashfall.Core.Survivors.SkillProgressionSystem(), _survivors); },
                openAction: () => _skillMatrixPanel.Open(),
                closeAction: () => _skillMatrixPanel.Visible = false);

            PanelRegistry.ConfigureActions("survival_workstation",
                bindAction: () => { SetupCrafting(); SetupInventory(); _survivalWorkstationPanel.Bind(_crafting, _inventory); },
                openAction: () => _survivalWorkstationPanel.Open(),
                closeAction: () => _survivalWorkstationPanel.Visible = false);

            PanelRegistry.ConfigureActions("verdict_dashboard",
                bindAction: () => { SetupVerdict(); _verdictDashboardPanel.Bind(_verdictPanel, _verdict); },
                openAction: () => _verdictDashboardPanel.Open(),
                closeAction: () => _verdictDashboardPanel.Visible = false);

            PanelRegistry.ConfigureActions("map_atlas",
                bindAction: () => { SetupExpeditions(); _mapAtlasPanel.Bind(_expeditions); },
                openAction: () => _mapAtlasPanel.Open(),
                closeAction: () => _mapAtlasPanel.Visible = false);

            PanelRegistry.ConfigureActions("maritime_atlas",
                bindAction: () => { SetupMaritime(); if (_maritime != null) _maritimeAtlasPanel.Bind(_maritime); },
                openAction: () => _maritimeAtlasPanel.Open(),
                closeAction: () => _maritimeAtlasPanel.Visible = false);

            PanelRegistry.ConfigureActions("muster_atlas",
                bindAction: () => { SetupMuster(); if (_muster != null) _musterAtlasPanel.Bind(_muster); },
                openAction: () => _musterAtlasPanel.Open(),
                closeAction: () => _musterAtlasPanel.Visible = false);

            PanelRegistry.ConfigureActions("quests_atlas",
                bindAction: () => { SetupHoldfastRuntime(); SetupExpansions(); _questsAtlasPanel.Bind(_core.Quests, _expansions?.CrossingQuests); },
                openAction: () => _questsAtlasPanel.Open(),
                closeAction: () => _questsAtlasPanel.Visible = false);

            PanelRegistry.ConfigureActions("research_atlas",
                bindAction: () => { _researchHostSession ??= ResearchHostSession.Create(_dataDir, EnsureSharedResearch()); _researchAtlasPanel.Bind(_researchHostSession); },
                openAction: () => _researchAtlasPanel.Open(),
                closeAction: () => _researchAtlasPanel.Visible = false);

            PanelRegistry.ConfigureActions("standing_record_atlas",
                bindAction: () => { _standingRecordHostSession ??= StandingRecordHostSession.Create(_dataDir); _standingRecordAtlasPanel.Bind(_standingRecordHostSession); },
                openAction: () => _standingRecordAtlasPanel.Open(),
                closeAction: () => _standingRecordAtlasPanel.Visible = false);

            PanelRegistry.ConfigureActions("combat_hud",
                bindAction: () => { if (_combat != null) _combatHudOverlay.Bind(_combat); },
                openAction: () => _combatHudOverlay.Open(),
                closeAction: () => _combatHudOverlay.Visible = false);

            // ── Flagship Consoles (Stitch Suite) ─────────────────────────────
            PanelRegistry.ConfigureActions("biogas_digester",
                openAction: () => _biogasDigesterPanel.Open(),
                closeAction: () => _biogasDigesterPanel.Visible = false);

            PanelRegistry.ConfigureActions("cartography_gis",
                openAction: () => _cartographyGisPanel.Open(),
                closeAction: () => _cartographyGisPanel.Visible = false);

            PanelRegistry.ConfigureActions("printing_press",
                openAction: () => _printingPressPanel.Open(),
                closeAction: () => _printingPressPanel.Visible = false);

            PanelRegistry.ConfigureActions("silicon_slicing",
                openAction: () => _siliconSlicingPanel.Open(),
                closeAction: () => _siliconSlicingPanel.Visible = false);

            PanelRegistry.ConfigureActions("geothermal_turbine",
                openAction: () => _geothermalTurbinePanel.Open(),
                closeAction: () => _geothermalTurbinePanel.Visible = false);

            PanelRegistry.ConfigureActions("war_dog_kennel",
                openAction: () => _warDogKennelPanel.Open(),
                closeAction: () => _warDogKennelPanel.Visible = false);

            PanelRegistry.ConfigureActions("isotope_separator",
                openAction: () => _isotopeSeparatorPanel.Open(),
                closeAction: () => _isotopeSeparatorPanel.Visible = false);

            PanelRegistry.ConfigureActions("plasma_smelting",
                openAction: () => _plasmaSmeltingPanel.Open(),
                closeAction: () => _plasmaSmeltingPanel.Visible = false);

            PanelRegistry.ConfigureActions("borehole_seismograph",
                openAction: () => _boreholeSeismographPanel.Open(),
                closeAction: () => _boreholeSeismographPanel.Visible = false);

            PanelRegistry.ConfigureActions("logistics_airlock",
                openAction: () => _logisticsAirlockPanel.Open(),
                closeAction: () => _logisticsAirlockPanel.Visible = false);

            PanelRegistry.ConfigureActions("cryo_permafrost_core",
                openAction: () => _cryoPermafrostCorePanel.Open(),
                closeAction: () => _cryoPermafrostCorePanel.Visible = false);

            PanelRegistry.ConfigureActions("basal_radon_migration",
                openAction: () => _basalRadonMigrationPanel.Open(),
                closeAction: () => _basalRadonMigrationPanel.Visible = false);

            PanelRegistry.ConfigureActions("trauma_bonding_cohort",
                openAction: () => _traumaBondingCohortPanel.Open(),
                closeAction: () => _traumaBondingCohortPanel.Visible = false);

            PanelRegistry.ConfigureActions("clandestine_insurgency",
                openAction: () => _clandestineInsurgencyPanel.Open(),
                closeAction: () => _clandestineInsurgencyPanel.Visible = false);

            PanelRegistry.ConfigureActions("subterranean_debt_ledger",
                openAction: () => _subterraneanDebtLedgerPanel.Open(),
                closeAction: () => _subterraneanDebtLedgerPanel.Visible = false);

            PanelRegistry.ConfigureActions("surface_shrapnel_aegis",
                openAction: () => _surfaceShrapnelAegisPanel.Open(),
                closeAction: () => _surfaceShrapnelAegisPanel.Visible = false);

            PanelRegistry.ConfigureActions("long_walk_expedition",
                openAction: () => _longWalkExpeditionPanel.Open(),
                closeAction: () => _longWalkExpeditionPanel.Visible = false);

            PanelRegistry.ConfigureActions("sonic_rupture_drill",
                openAction: () => _sonicRuptureDrillPanel.Open(),
                closeAction: () => _sonicRuptureDrillPanel.Visible = false);

            PanelRegistry.ConfigureActions("vault_door_breaching",
                openAction: () => _vaultDoorBreachingPanel.Open(),
                closeAction: () => _vaultDoorBreachingPanel.Visible = false);

            PanelRegistry.ConfigureActions("iron_cenotaph_memorial",
                openAction: () => _ironCenotaphMemorialPanel.Open(),
                closeAction: () => _ironCenotaphMemorialPanel.Visible = false);

            PanelRegistry.ConfigureActions("aquifer_treaty_concession",
                openAction: () => _aquiferTreatyConcessionPanel.Open(),
                closeAction: () => _aquiferTreatyConcessionPanel.Visible = false);

            PanelRegistry.ConfigureActions("crossing_safe_conduct_vouch",
                openAction: () => _crossingSafeConductVouchPanel.Open(),
                closeAction: () => _crossingSafeConductVouchPanel.Visible = false);

            PanelRegistry.ConfigureActions("mechanical_prosthetics_lathe",
                openAction: () => _mechanicalProstheticsLathePanel.Open(),
                closeAction: () => _mechanicalProstheticsLathePanel.Visible = false);

            PanelRegistry.ConfigureActions("fungal_protein_fermenter",
                openAction: () => _fungalProteinFermenterPanel.Open(),
                closeAction: () => _fungalProteinFermenterPanel.Visible = false);

            PanelRegistry.ConfigureActions("ultrasonic_decontam_airlock",
                openAction: () => _ultrasonicDecontamAirlockPanel.Open(),
                closeAction: () => _ultrasonicDecontamAirlockPanel.Visible = false);

            PanelRegistry.ConfigureActions("tropospheric_radio_relay",
                openAction: () => _troposphericRadioRelayPanel.Open(),
                closeAction: () => _troposphericRadioRelayPanel.Visible = false);

            PanelRegistry.ConfigureActions("induction_cupola_furnace",
                openAction: () => _inductionCupolaFurnacePanel.Open(),
                closeAction: () => _inductionCupolaFurnacePanel.Visible = false);

            PanelRegistry.ConfigureActions("heavy_marine_diesel_gen",
                openAction: () => _heavyMarineDieselGenPanel.Open(),
                closeAction: () => _heavyMarineDieselGenPanel.Visible = false);

            PanelRegistry.ConfigureActions("slurry_dewatering_sump",
                openAction: () => _slurryDewateringSumpPanel.Open(),
                closeAction: () => _slurryDewateringSumpPanel.Visible = false);

            PanelRegistry.ConfigureActions("magnetic_drum_archive",
                openAction: () => _magneticDrumArchivePanel.Open(),
                closeAction: () => _magneticDrumArchivePanel.Visible = false);

            // Expanded Panels
            string[] expandedIds =
            {
                "water_treatment", "airlock_security", "survivor_relations", "regional_treaty",
                "vinyl_morale", "wildlife_trapping", "excavation", "apprenticeship",
                "caregiving", "shelter_thermal", "shelter_schedule", "shelter_decor", "autopsy_report",
                "waystation_network", "chemical_dependency", "sump_flooding", "decontamination",
                "kitchen_nutrition", "equipment_condition", "library_study", "archive_desk",
                "contractor_roster", "mental_health_crisis", "phantom_memory",
                "traveling_caravan", "medical_ward"
            };

            foreach (var expId in expandedIds)
            {
                string id = expId;
                PanelRegistry.ConfigureActions(id,
                    openAction: () => OpenExpandedPanel(id));
            }
        }

        private ResearchHostSession? _researchHostSession;
        private StandingRecordHostSession? _standingRecordHostSession;
    }
}
