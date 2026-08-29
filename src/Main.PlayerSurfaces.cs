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
                bindAction: () => { _sharedResearch ??= new ResearchSystem(log: new GodotLog()); _researchPanel.Bind(_sharedResearch); },
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
                bindAction: () => { SetupSurvivors(); SetupInventory(); SetupMedical(); SetupPhase0(); _medicalPanel.Bind(_medical, _survivors, _inventory, _phase0?.Respiratory); },
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
                bindAction: () => { SetupSurvivors(); SetupWorld(); SetupInventory(); _shelterPanel.Bind(_survivors, _world, _inventory); },
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
                bindAction: () => { SetupExpansions(); SetupSilentFoundry(); _silentFoundryPanel.Bind(_silentFoundry, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay); },
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

            // Expanded Panels
            string[] expandedIds =
            {
                "water_treatment", "airlock_security", "survivor_relations", "regional_treaty",
                "vinyl_morale", "wildlife_trapping", "excavation", "apprenticeship",
                "caregiving", "shelter_thermal", "shelter_schedule", "autopsy_report",
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
    }
}
