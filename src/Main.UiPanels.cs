using Godot;
using System;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.YearOfAsh;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Legacy UI shell fields (moved from Main.cs for cohesion) ──
        private Label _titleLabel = null!;
        private Label _statusLabel = null!;
        private Label _diagnosticsLabel = null!;
        private Label _iceRoadLabel = null!;
        private Label _catalogLabel = null!;
        private Label _briefingPreviewLabel = null!;
        private VBoxContainer _menuContainer = null!;
        private TextEdit _codexViewer = null!;

        // ── UI Panel fields (GAP-ARCH-01 Phase 1) ──
        private MainMenuPanel _mainMenu = null!;
        private GameOverPanel _gameOver = null!;
        private GameHudOverlay _hudOverlay = null!;
        private GameDashboardPanel _dashboard = null!;
        private VBoxContainer _gameUiContainer = null!;
        private AudioManager _audio = null!;
        private SettingsPanel _settingsPanel = null!;
        private InventoryPanel _inventoryOverlay = null!;
        private SurvivorsPanel _survivorsOverlay = null!;
        private CraftingPanel _craftingPanel = null!;
        private WorkshopPanel _workshopPanel = null!;
        private PharmaLabPanel _pharmaLabPanel = null!;
        private RadioPanel _radioPanel = null!;
        private MedicalPanel _medicalPanel = null!;
        private Phase0Panel _phase0Panel = null!;
        private DutyRosterPanel _dutyRosterPanel = null!;
        private ExpeditionPanel _expeditionPanel = null!;
        private WeatherPanel _weatherPanel = null!;
        private QuestsPanel _questsPanel = null!;
        private JournalPanel _journalPanel = null!;
        private FactionsPanel _factionsPanel = null!;
        private MusterPanel _musterPanel = null!;
        private ExpansionsHubPanel _expansionsHubPanel = null!;
        private StandingRecordPanel _standingRecordPanel = null!;
        private MaritimePanel _maritimePanel = null!;
        private DeepCoastPanel _deepCoastPanel = null!;
        private CenturySeedPanel _centurySeedPanel = null!;
        private EpiloguePanel _epiloguePanel = null!;
        private CrossingQuestPanel _crossingQuestPanel = null!;
        private ResearchPanel _researchPanel = null!;
        private ShelterPanel _shelterPanel = null!;
        private CombatPanel _combatPanel = null!;
        private MapPanel _mapPanel = null!;
        private InventoryPanel _inventoryPanel = null!;
        private EconomyMarketPanel _economyPanel = null!;
        private UtilityAiPanel _utilityAiPanel = null!;
        private JournalCodex _journalCodex = null!;
        private JournalBookUI _journalBook = null!;

        // ── Plans 178-201 expansion panels (Plans178_201 UI surfaces) ──
        private AviationUI _aviationPanel = null!;
        private ChemUI _chemPanel = null!;
        private LaborUI _laborPanel = null!;
        private PoliticsUI _politicsPanel = null!;
        private PrisonerPanel _prisonerPanel = null!;
        private StealthReadoutPanel _stealthReadoutPanel = null!;
        private MutationTreePanel _mutationTreePanel = null!;
        private NurseryPanel _nurseryPanel = null!;
        private SurvivorDetailPanel _survivorDetailPanel = null!;
        private InventoryDetailPanel _inventoryDetailPanel = null!;
        private QuestDetailPanel _questDetailPanel = null!;
        private AchievementsPanel _achievementsPanel = null!;
        private WeatherDetailPanel _weatherDetailPanel = null!;
        private RadiationDetailPanel _radiationDetailPanel = null!;
        private EventsLogPanel _eventsLogPanel = null!;
        private DutyRosterDetailPanel _dutyRosterDetailPanel = null!;
        private EconomyDetailPanel _economyDetailPanel = null!;
        private CombatDetailPanel _combatDetailPanel = null!;
        private FactionDetailPanel _factionDetailPanel = null!;
        private SaveLoadPanel _saveLoadPanel = null!;
        private TutorialPanel _tutorialPanel = null!;
        private AfflictionsPanel _afflictionsPanel = null!;
        private WeatherForecastPanel _weatherForecastPanel = null!;
        private RadiationHistoryPanel _radiationHistoryPanel = null!;
        private JournalDetailPanel _journalDetailPanel = null!;
        private CombatHistoryPanel _combatHistoryPanel = null!;
        private MapDetailPanel _mapDetailPanel = null!;
        private EventDetailPanel _eventDetailPanel = null!;
        private StatusPanel _statusPanel = null!;
        private SurvivalDetailPanel _survivalDetailPanel = null!;
        private WeatherHistoryPanel _weatherHistoryPanel = null!;

        // ── Additional & Flagship Console Panels ──
        private BrineExtractionPanel _brineExtractionPanel = null!;
        private ExpeditionCampPanel _expeditionCampPanel = null!;
        private FireIncidentPanel _fireIncidentPanel = null!;
        private GeigerCalibrationPanel _geigerCalibrationPanel = null!;
        private TriangulationPanel _triangulationPanel = null!;
        private WeatherSondePanel _weatherSondePanel = null!;
        private PowerGridPanel _powerGridPanel = null!;
        private ExpeditionRadarPanel _expeditionRadarPanel = null!;
        private DoseLedgerPanel _doseLedgerPanel = null!;
        private CaravanBarterLedgerPanel _caravanBarterLedgerPanel = null!;
        private FactionMatrixPanel _factionMatrixPanel = null!;
        private FactionsNarrativePanel _factionsNarrativePanel = null!;
        private SkillMatrixPanel _skillMatrixPanel = null!;
        private SurvivalWorkstationPanel _survivalWorkstationPanel = null!;
        private VerdictDashboardPanel _verdictDashboardPanel = null!;
        private MapAtlasPanel _mapAtlasPanel = null!;
        private MaritimeAtlasPanel _maritimeAtlasPanel = null!;
        private MusterAtlasPanel _musterAtlasPanel = null!;
        private QuestsAtlasPanel _questsAtlasPanel = null!;
        private ResearchAtlasPanel _researchAtlasPanel = null!;
        private StandingRecordAtlasPanel _standingRecordAtlasPanel = null!;
        private CombatHudOverlay _combatHudOverlay = null!;
        private AnaerobicBiogasDigesterPanel _biogasDigesterPanel = null!;
        private SubterraneanCartographyPanel _cartographyGisPanel = null!;
        private UndergroundPrintingPressPanel _printingPressPanel = null!;
        private SiliconIngotSlicingPanel _siliconSlicingPanel = null!;
        private GeothermalSteamTurbinePanel _geothermalTurbinePanel = null!;
        private WarDogKennelPanel _warDogKennelPanel = null!;
        private IsotopeSeparatorPanel _isotopeSeparatorPanel = null!;
        private PlasmaArcSmeltingPanel _plasmaSmeltingPanel = null!;
        private BoreholeSeismographPanel _boreholeSeismographPanel = null!;
        private HeavyLogisticsAirlockPanel _logisticsAirlockPanel = null!;
        private CryogenicPermafrostCorePanel _cryoPermafrostCorePanel = null!;
        private BasalRadonMigrationPanel _basalRadonMigrationPanel = null!;
        private TraumaBondingCohortPanel _traumaBondingCohortPanel = null!;
        private ClandestineInsurgencyPanel _clandestineInsurgencyPanel = null!;
        private SubterraneanDebtLedgerPanel _subterraneanDebtLedgerPanel = null!;
        private SurfaceShrapnelAegisPanel _surfaceShrapnelAegisPanel = null!;
        private LongWalkExpeditionPanel _longWalkExpeditionPanel = null!;
        private SonicRuptureDrillPanel _sonicRuptureDrillPanel = null!;
        private VaultDoorBreachingPanel _vaultDoorBreachingPanel = null!;
        private IronCenotaphMemorialPanel _ironCenotaphMemorialPanel = null!;
        private AquiferTreatyConcessionPanel _aquiferTreatyConcessionPanel = null!;
        private CrossingSafeConductVouchPanel _crossingSafeConductVouchPanel = null!;
        private MechanicalProstheticsLathePanel _mechanicalProstheticsLathePanel = null!;
        private FungalProteinFermenterPanel _fungalProteinFermenterPanel = null!;
        private UltrasonicDecontaminationAirlockPanel _ultrasonicDecontamAirlockPanel = null!;
        private TroposphericRadioRelayPanel _troposphericRadioRelayPanel = null!;
        private InductionCupolaFurnacePanel _inductionCupolaFurnacePanel = null!;
        private HeavyMarineDieselGeneratorPanel _heavyMarineDieselGenPanel = null!;
        private SlurryDewateringSumpPanel _slurryDewateringSumpPanel = null!;
        private MagneticDrumArchivePanel _magneticDrumArchivePanel = null!;

        private void BuildUserInterface()
        {
            // Root full-rect styling
            SetAnchorsPreset(LayoutPreset.FullRect);

            var bg = new ColorRect
            {
                Color = AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Ink)
            };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            // ── Audio manager (buses + playback) ──
            _audio = new AudioManager();
            AddChild(_audio);
            _audio.PlayMainMenuMusic();

            // ── Player-facing game shell ──
            // The old developer workbench remains available behind the DEV CONSOLE
            // action, but it is not the first thing a player sees after starting a run.
            _dashboard = new GameDashboardPanel();
            _dashboard.OnMenuRequested += ReturnToMenu;
            _dashboard.OnAdvanceDayRequested += OnTickIceRoadClicked;
            _dashboard.OnSaveRequested += SaveAll;
            _dashboard.OnDeveloperRequested += ToggleDeveloperConsole;
            _dashboard.OnOpenPanelRequested += OpenPlayerPanel;
            _dashboard.OnServiceFilterRequested += () =>
            {
                SetupStartingLevel();
                if (_startingLevel?.ServiceAirFilter() == true)
                    HandleShelterRoomRepairPerformed("room_filtration");
                UpdateHud();
            };
            _dashboard.OnReplaceFilterRequested += () =>
            {
                SetupStartingLevel();
                if (_startingLevel?.ReplaceAirFilter() == true)
                    HandleShelterRoomRepairPerformed("room_filtration");
                UpdateHud();
            };
            AddChild(_dashboard);

            // ── Game UI container (hidden initially) ──
            var gameUiContainer = new VBoxContainer();
            gameUiContainer.SetAnchorsPreset(LayoutPreset.FullRect);
            gameUiContainer.Visible = false;
            AddChild(gameUiContainer);
            _gameUiContainer = gameUiContainer;

            // ── HUD overlay ──
            _hudOverlay = new GameHudOverlay();
            _hudOverlay.OnMenuRequested += ReturnToMenu;
            gameUiContainer.AddChild(_hudOverlay);

            // ── Settings panel (overlay) ──
            _settingsPanel = new SettingsPanel();
            _settingsPanel.OnClose += CloseSettingsPanel;
            AddChild(_settingsPanel);

            // ── Inventory overlay panel ──
            _inventoryOverlay = new InventoryPanel();
            _inventoryOverlay.OnClose += CloseInventoryOverlay;
            AddChild(_inventoryOverlay);

            // ── Survivors overlay panel ──
            _survivorsOverlay = new SurvivorsPanel();
            _survivorsOverlay.OnClose += CloseSurvivorsOverlay;
            AddChild(_survivorsOverlay);

            // ── Crafting panel (overlay) ──
            _craftingPanel = PanelSceneLoader.Load<CraftingPanel>("res://assets/ui/panels/CraftingPanel.tscn");
            _craftingPanel.OnClose += CloseCraftingPanel;
            _craftingPanel.OnCraftStarted += () => { UpdateHud(); _craftingDirty = true; };
            _craftingPanel.OnOpenWorkshopRequested += () => OpenPlayerPanel("workshop");
            _craftingPanel.OnOpenPharmaLabRequested += () => OpenPlayerPanel("pharma_lab");
            AddChild(_craftingPanel);

            // ── Workshop panel (relic reverse engineering) ──
            _workshopPanel = PanelSceneLoader.Load<WorkshopPanel>("res://assets/ui/panels/WorkshopPanel.tscn");
            _workshopPanel.OnClose += CloseWorkshopPanel;
            AddChild(_workshopPanel);

            // ── Pharma Lab panel (compounding & distillation) ──
            _pharmaLabPanel = new PharmaLabPanel();
            _pharmaLabPanel.OnClose += ClosePharmaLabPanel;
            AddChild(_pharmaLabPanel);

            // ── Radio panel (overlay) ──
            _radioPanel = new RadioPanel();
            _radioPanel.OnClose += CloseRadioPanel;
            _radioPanel.OnRadioBroadcastSent += UpdateHud;
            AddChild(_radioPanel);

            // ── Medical panel (overlay) ──
            _medicalPanel = new MedicalPanel();
            _medicalPanel.OnClose += CloseMedicalPanel;
            _medicalPanel.OnTreatmentAdministered += UpdateHud;
            AddChild(_medicalPanel);

            // ── Phase 0 panel (overlay) ──
            _phase0Panel = new Phase0Panel();
            _phase0Panel.OnClose += ClosePhase0Panel;
            AddChild(_phase0Panel);

            // ── Duty Roster panel (overlay) ──
            _dutyRosterPanel = new DutyRosterPanel();
            _dutyRosterPanel.OnClose += CloseDutyRosterPanel;
            _dutyRosterPanel.OnAssignmentChanged += UpdateHud;
            _dutyRosterPanel.OnDetailsRequested += () => OpenPlayerPanel("duty_roster_detail");
            AddChild(_dutyRosterPanel);

            // ── Expedition panel (overlay) ──
            _expeditionPanel = new ExpeditionPanel();
            _expeditionPanel.OnClose += CloseExpeditionPanel;
            _expeditionPanel.OnExpeditionUpdated += UpdateHud;
            _expeditionPanel.OnLootDeposited += loot =>
            {
                UpdateHud();
                GD.Print($"[Expeditions] Recovered {loot.Count} loot items.");
            };
            AddChild(_expeditionPanel);

            // ── Weather panel (overlay) ──
            _weatherPanel = new WeatherPanel();
            _weatherPanel.OnClose += CloseWeatherPanel;
            AddChild(_weatherPanel);

            // ── Quests panel (overlay) ──
            _questsPanel = new QuestsPanel();
            _questsPanel.OnClose += CloseQuestsPanel;
            _questsPanel.OnQuestDetailRequested += OpenQuestDetailPanel;
            _questsPanel.OnCrossingPanelRequested += OpenCrossingQuestPanel;
            AddChild(_questsPanel);

            // ── Journal panel (overlay) ──
            _journalPanel = new JournalPanel();
            _journalPanel.OnClose += CloseJournalPanel;
            AddChild(_journalPanel);

            // ── Factions panel (overlay) ──
            _factionsPanel = new FactionsPanel();
            _factionsPanel.OnClose += CloseFactionsPanel;
            _factionsPanel.OnFactionDetailRequested += OpenFactionDetailPanel;
            _factionsPanel.OnMusterPanelRequested += () => OpenPlayerPanel("muster");
            _factionsPanel.OnFoundryPanelRequested += () => OpenPlayerPanel("silent_foundry");
            AddChild(_factionsPanel);

            // ── Muster panel (overlay) ──
            _musterPanel = new MusterPanel();
            _musterPanel.OnClose += CloseMusterPanel;
            _musterPanel.OnApproachModalRequested += OpenMusterApproachModal;
            AddChild(_musterPanel);

            // ── Expansions Hub panel (overlay) ──
            _expansionsHubPanel = new ExpansionsHubPanel();
            _expansionsHubPanel.OnClose += CloseExpansionsHubPanel;
            _expansionsHubPanel.OnOpenExpansionRequested += (expId) => OpenPlayerPanel(expId);
            AddChild(_expansionsHubPanel);

            // ── Standing Record panel (overlay) ──
            _standingRecordPanel = new StandingRecordPanel();
            _standingRecordPanel.OnClose += CloseStandingRecordPanel;
            AddChild(_standingRecordPanel);

            // ── Maritime panel (overlay) ──
            _maritimePanel = new MaritimePanel();
            _maritimePanel.OnClose += CloseMaritimePanel;
            AddChild(_maritimePanel);

            // ── Deep Coast panel (overlay, Exp 01 sibling layer) ──
            _deepCoastPanel = new DeepCoastPanel();
            _deepCoastPanel.OnClose += CloseDeepCoastPanel;
            AddChild(_deepCoastPanel);

            // ── Century Seed panel (overlay) ──
            _centurySeedPanel = new CenturySeedPanel();
            _centurySeedPanel.OnClose += CloseCenturySeedPanel;
            AddChild(_centurySeedPanel);

            // ── Epilogue panel (overlay) ──
            _epiloguePanel = new EpiloguePanel();
            _epiloguePanel.OnClose += CloseEpiloguePanel;
            AddChild(_epiloguePanel);

            // ── Verdict panel (overlay) ──
            _verdictPanel = new VerdictPanel();
            _verdictPanel.OnClose += CloseVerdictPanel;
            AddChild(_verdictPanel);

            // ── Holdfast Terminal (overlay) ──
            _holdfastTerminal = new HoldfastTerminalPanel();
            AddChild(_holdfastTerminal);

            // ── Research panel (overlay) ──
            _researchPanel = new ResearchPanel();
            _researchPanel.OnClose += CloseResearchPanel;
            AddChild(_researchPanel);

            // ── Shelter panel (overlay) ──
            _shelterPanel = new ShelterPanel();
            _shelterPanel.OnClose += CloseShelterPanel;
            _shelterPanel.RoomSelected += HandleShelterRoomSelected; // Plan 29 29A: click = inspect
            AddChild(_shelterPanel);

            // ── Greenhouse panel (overlay) ──
            _greenhousePanel = new GreenhousePanel();
            _greenhousePanel.OnClose += CloseGreenhousePanel;
            _greenhousePanel.OnActionRequested += HandleGreenhouseAction;
            AddChild(_greenhousePanel);

            // ── Silent Foundry panel (overlay) ──
            _silentFoundryPanel = new SilentFoundryPanel();
            _silentFoundryPanel.OnClose += CloseSilentFoundryPanel;
            AddChild(_silentFoundryPanel);

            // ── Trade screen (overlay) — guild stance gates the stall ──
            _tradePanel = new AtomicWar.GodotApp.Economy.TradeScreenGodotPanel();
            _tradePanel.OnClose += CloseTradePanel;
            AddChild(_tradePanel);

            // ── Combat panel (overlay) ──
            _combatPanel = new CombatPanel();
            _combatPanel.OnClose += CloseCombatPanel;
            AddChild(_combatPanel);

            // ── Map panel (overlay) ──
            _mapPanel = new MapPanel();
            _mapPanel.OnClose += CloseMapPanel;
            _mapPanel.OnLocationDetailRequested += OpenMapDetailPanel;
            AddChild(_mapPanel);

            // ── Survivor Detail panel (overlay) ──
            _survivorDetailPanel = PanelSceneLoader.Load<SurvivorDetailPanel>("res://assets/ui/panels/SurvivorDetailPanel.tscn");
            _survivorDetailPanel.OnClose += CloseSurvivorDetailPanel;
            AddChild(_survivorDetailPanel);

            // ── Inventory Detail panel (overlay) ──
            _inventoryDetailPanel = PanelSceneLoader.Load<InventoryDetailPanel>("res://assets/ui/panels/InventoryDetailPanel.tscn");
            _inventoryDetailPanel.OnClose += CloseInventoryDetailPanel;
            AddChild(_inventoryDetailPanel);

            // ── Quest Detail panel (overlay) ──
            _questDetailPanel = PanelSceneLoader.Load<QuestDetailPanel>("res://assets/ui/panels/QuestDetailPanel.tscn");
            _questDetailPanel.OnClose += CloseQuestDetailPanel;
            AddChild(_questDetailPanel);

            // ── Achievements panel (overlay) ──
            _achievementsPanel = new AchievementsPanel();
            _achievementsPanel.OnClose += CloseAchievementsPanel;
            AddChild(_achievementsPanel);

            // ── Weather Detail panel (overlay) ──
            _weatherDetailPanel = PanelSceneLoader.Load<WeatherDetailPanel>("res://assets/ui/panels/WeatherDetailPanel.tscn");
            _weatherDetailPanel.OnClose += CloseWeatherDetailPanel;
            AddChild(_weatherDetailPanel);

            // ── Radiation Detail panel (overlay) ──
            _radiationDetailPanel = PanelSceneLoader.Load<RadiationDetailPanel>("res://assets/ui/panels/RadiationDetailPanel.tscn");
            _radiationDetailPanel.OnClose += CloseRadiationDetailPanel;
            AddChild(_radiationDetailPanel);

            // ── Events Log panel (overlay) ──
            _eventsLogPanel = new EventsLogPanel();
            _eventsLogPanel.OnClose += CloseEventsLogPanel;
            AddChild(_eventsLogPanel);

            // ── Duty Roster Detail panel (overlay) ──
            _dutyRosterDetailPanel = PanelSceneLoader.Load<DutyRosterDetailPanel>("res://assets/ui/panels/DutyRosterDetailPanel.tscn");
            _dutyRosterDetailPanel.OnClose += CloseDutyRosterDetailPanel;
            AddChild(_dutyRosterDetailPanel);

            // ── Economy Detail panel (overlay) ──
            _economyDetailPanel = PanelSceneLoader.Load<EconomyDetailPanel>("res://assets/ui/panels/EconomyDetailPanel.tscn");
            _economyDetailPanel.OnClose += CloseEconomyDetailPanel;
            AddChild(_economyDetailPanel);

            // ── Combat Detail panel (overlay) ──
            _combatDetailPanel = PanelSceneLoader.Load<CombatDetailPanel>("res://assets/ui/panels/CombatDetailPanel.tscn");
            _combatDetailPanel.OnClose += CloseCombatDetailPanel;
            AddChild(_combatDetailPanel);

            // ── Faction Detail panel (overlay) ──
            _factionDetailPanel = PanelSceneLoader.Load<FactionDetailPanel>("res://assets/ui/panels/FactionDetailPanel.tscn");
            _factionDetailPanel.OnClose += CloseFactionDetailPanel;
            AddChild(_factionDetailPanel);

            // ── Crossing Quest panel (overlay) ──
            _crossingQuestPanel = new CrossingQuestPanel();
            _crossingQuestPanel.OnClose += CloseCrossingQuestPanel;
            AddChild(_crossingQuestPanel);

            // ── Plans 178-201 expansion panels (overlay) ──
            _aviationPanel = new AviationUI();
            _aviationPanel.OnClose += () => _aviationPanel.Visible = false;
            AddChild(_aviationPanel);

            _chemPanel = new ChemUI();
            _chemPanel.OnClose += () => _chemPanel.Visible = false;
            AddChild(_chemPanel);

            _laborPanel = new LaborUI();
            _laborPanel.OnClose += () => _laborPanel.Visible = false;
            AddChild(_laborPanel);

            _politicsPanel = new PoliticsUI();
            _politicsPanel.OnClose += () => _politicsPanel.Visible = false;
            AddChild(_politicsPanel);

            _prisonerPanel = new PrisonerPanel();
            _prisonerPanel.OnClose += () => _prisonerPanel.Visible = false;
            AddChild(_prisonerPanel);

            _stealthReadoutPanel = new StealthReadoutPanel();
            _stealthReadoutPanel.OnClose += () => _stealthReadoutPanel.Visible = false;
            AddChild(_stealthReadoutPanel);

            _mutationTreePanel = new MutationTreePanel();
            _mutationTreePanel.OnClose += () => _mutationTreePanel.Visible = false;
            AddChild(_mutationTreePanel);

            _nurseryPanel = new NurseryPanel();
            _nurseryPanel.OnClose += () => _nurseryPanel.Visible = false;
            AddChild(_nurseryPanel);

            // ── Save/Load panel (overlay) ──
            _saveLoadPanel = new SaveLoadPanel();
            _saveLoadPanel.OnClose += CloseSaveLoadPanel;
            _saveLoadPanel.OnSlotSelected += slotId =>
            {
                _saveLoadHost?.SelectSlot(slotId);
                UpdateContinueButton();
            };
            _saveLoadPanel.OnLoadRequested += slotId =>
            {
                bool success = TryLoadAndRestoreGame(slotId, out string message);
                if (success)
                {
                    _saveLoadPanel.ShowSuccess(message);
                    if (_statusLabel != null) _statusLabel.Text = message;
                }
                else
                {
                    _saveLoadPanel.ShowError(message);
                    if (_statusLabel != null) _statusLabel.Text = message;
                }
                _saveLoadPanel.RefreshView();
                UpdateContinueButton();
            };
            _saveLoadPanel.OnSaveRequested += () =>
            {
                SaveAll();
                _saveLoadPanel.RefreshView();
                UpdateContinueButton();
            };
            _saveLoadPanel.OnDeleteRequested += slotId =>
            {
                _saveLoadHost?.DeleteSlot(slotId);
                UpdateContinueButton();
            };
            _saveLoadPanel.OnImportRequested += profileId =>
            {
                string basePath = ProjectSettings.GlobalizePath("user://");
                string[] candidateFiles = {
                    System.IO.Path.Combine(basePath, "holdfast_s1_save.json"),
                    System.IO.Path.Combine(basePath, "inventory_save.json"),
                    HoldfastSaveStore.SavePath,
                    InventorySaveStore.SavePath
                };
                foreach (var candidate in candidateFiles)
                {
                    if (System.IO.File.Exists(candidate))
                    {
                        _saveLoadHost?.ImportLegacySave(candidate);
                        break;
                    }
                }
                _saveLoadPanel.RefreshView();
                UpdateContinueButton();
            };
            AddChild(_saveLoadPanel);

            // ── Tutorial panel (overlay) ──
            _tutorialPanel = new TutorialPanel();
            _tutorialPanel.OnClose += CloseTutorialPanel;
            AddChild(_tutorialPanel);

            // ── Afflictions panel (overlay) ──
            _afflictionsPanel = PanelSceneLoader.Load<AfflictionsPanel>("res://assets/ui/panels/AfflictionsPanel.tscn");
            _afflictionsPanel.OnClose += CloseAfflictionsPanel;
            AddChild(_afflictionsPanel);

            // ── Status panel (overlay) ──
            _statusPanel = new StatusPanel();
            _statusPanel.OnClose += CloseStatusPanel;
            AddChild(_statusPanel);

            // ── Survival Detail panel (overlay) ──
            _survivalDetailPanel = PanelSceneLoader.Load<SurvivalDetailPanel>("res://assets/ui/panels/SurvivalDetailPanel.tscn");
            _survivalDetailPanel.OnClose += CloseSurvivalDetailPanel;
            AddChild(_survivalDetailPanel);

            // ── Weather Forecast panel (overlay) ──
            _weatherForecastPanel = new WeatherForecastPanel();
            _weatherForecastPanel.OnClose += CloseWeatherForecastPanel;
            AddChild(_weatherForecastPanel);

            // ── Weather History panel (overlay, F5 surface) ──
            _weatherHistoryPanel = new WeatherHistoryPanel();
            _weatherHistoryPanel.OnClose += () => _weatherHistoryPanel.Visible = false;
            AddChild(_weatherHistoryPanel);

            // ── Radiation History panel (overlay) ──
            _radiationHistoryPanel = new RadiationHistoryPanel();
            _radiationHistoryPanel.OnClose += CloseRadiationHistoryPanel;
            AddChild(_radiationHistoryPanel);

            // ── Journal Detail panel (overlay) ──
            _journalDetailPanel = PanelSceneLoader.Load<JournalDetailPanel>("res://assets/ui/panels/JournalDetailPanel.tscn");
            _journalDetailPanel.OnClose += CloseJournalDetailPanel;
            AddChild(_journalDetailPanel);

            // ── Combat History panel (overlay) ──
            _combatHistoryPanel = new CombatHistoryPanel();
            _combatHistoryPanel.OnClose += CloseCombatHistoryPanel;
            AddChild(_combatHistoryPanel);

            // ── Map Detail panel (overlay) ──
            _mapDetailPanel = PanelSceneLoader.Load<MapDetailPanel>("res://assets/ui/panels/MapDetailPanel.tscn");
            _mapDetailPanel.OnClose += CloseMapDetailPanel;
            AddChild(_mapDetailPanel);

            // ── Event Detail panel (overlay) ──
            _eventDetailPanel = PanelSceneLoader.Load<EventDetailPanel>("res://assets/ui/panels/EventDetailPanel.tscn");
            _eventDetailPanel.OnClose += CloseEventDetailPanel;
            AddChild(_eventDetailPanel);

            // ── Opening Protocol Directives Modal (Day 1 vertical slice) ──
            _openingProtocolModal = new OpeningProtocolModal();
            _openingProtocolModal.OnClose += CloseOpeningProtocolModal;
            _openingProtocolModal.OnRationPolicySelected += policy =>
            {
                SetupStartingLevel();
                _startingLevel.ResolveMorningRationTriage(policy);
                int day = _holdfastRuntime?.Day ?? 1;
                SetupJournal();
                _journal.TryAddRawEntry(
                    $"directive_ration_{day}_{policy}",
                    $"Day {day}: Set ration policy to {policy}. Stores adjusted for the cohort.",
                    author: null!,
                    day: day);
                UpdateHud();
                ObserveSigil("protocol.ration");
            };
            _openingProtocolModal.OnMaintenanceDirectiveSelected += directive =>
            {
                SetupStartingLevel();
                _startingLevel.ResolveMiddayMaintenance(directive);
                SetupInventory();
                if (directive == Ashfall.Core.StartingLevel.MaintenanceDirective.ServiceFilterStack)
                {
                    _inventory.Remove("scrap_mechanical", 1);
                }
                else if (directive == Ashfall.Core.StartingLevel.MaintenanceDirective.FortifyBunksLead)
                {
                    _inventory.Remove("scrap_mechanical", 2);
                    SetupSurvivors();
                    _survivors.Shelter.UpgradeCeiling("room_bunks_living", Ashfall.Core.Shelter.MaterialShieldingSystem.WallMaterial.Lead);
                }
                int day = _holdfastRuntime?.Day ?? 1;
                SetupJournal();
                _journal.TryAddRawEntry(
                    $"directive_maint_{day}_{directive}",
                    $"Day {day}: Maintenance priority {directive} completed. Structural integrity confirmed.",
                    author: null!,
                    day: day);
                UpdateHud();
                ObserveSigil("protocol.maintenance");
            };
            _openingProtocolModal.OnRadioProtocolSelected += protocol =>
            {
                SetupStartingLevel();
                _startingLevel.ResolveEveningRadio(protocol);
                int day = _holdfastRuntime?.Day ?? 1;
                SetupRadio();
                _radio.Listen(142.85f);
                SetupJournal();
                _journal.TryAddRawEntry(
                    $"directive_radio_{day}_{protocol}",
                    $"Day {day}: Radio protocol {protocol} executed on 142.850 MHz carrier frequency.",
                    author: null!,
                    day: day);
                UpdateHud();
                ObserveSigil("protocol.radio");
            };
            AddChild(_openingProtocolModal);

            // ── Additional & Flagship Console Panels ──
            _brineExtractionPanel = new BrineExtractionPanel { Visible = false };
            _brineExtractionPanel.OnClose += () => _brineExtractionPanel.Visible = false;
            AddChild(_brineExtractionPanel);

            _expeditionCampPanel = new ExpeditionCampPanel { Visible = false };
            _expeditionCampPanel.OnClose += () => _expeditionCampPanel.Visible = false;
            AddChild(_expeditionCampPanel);

            _fireIncidentPanel = new FireIncidentPanel { Visible = false };
            _fireIncidentPanel.OnClose += () => _fireIncidentPanel.Visible = false;
            AddChild(_fireIncidentPanel);

            _geigerCalibrationPanel = new GeigerCalibrationPanel { Visible = false };
            _geigerCalibrationPanel.OnClose += () => _geigerCalibrationPanel.Visible = false;
            AddChild(_geigerCalibrationPanel);

            _triangulationPanel = new TriangulationPanel { Visible = false };
            _triangulationPanel.OnClose += () => _triangulationPanel.Visible = false;
            AddChild(_triangulationPanel);

            _weatherSondePanel = new WeatherSondePanel { Visible = false };
            _weatherSondePanel.OnClose += () => _weatherSondePanel.Visible = false;
            AddChild(_weatherSondePanel);

            _powerGridPanel = new PowerGridPanel { Visible = false };
            _powerGridPanel.OnClose += () => _powerGridPanel.Visible = false;
            AddChild(_powerGridPanel);

            _expeditionRadarPanel = new ExpeditionRadarPanel { Visible = false };
            _expeditionRadarPanel.OnClose += () => _expeditionRadarPanel.Visible = false;
            AddChild(_expeditionRadarPanel);

            _doseLedgerPanel = new DoseLedgerPanel { Visible = false };
            _doseLedgerPanel.OnClose += () => _doseLedgerPanel.Visible = false;
            AddChild(_doseLedgerPanel);

            _caravanBarterLedgerPanel = new CaravanBarterLedgerPanel { Visible = false };
            _caravanBarterLedgerPanel.OnClose += () => _caravanBarterLedgerPanel.Visible = false;
            AddChild(_caravanBarterLedgerPanel);

            _factionMatrixPanel = new FactionMatrixPanel { Visible = false };
            _factionMatrixPanel.OnClose += () => _factionMatrixPanel.Visible = false;
            AddChild(_factionMatrixPanel);

            _factionsNarrativePanel = new FactionsNarrativePanel { Visible = false };
            _factionsNarrativePanel.OnClose += () => _factionsNarrativePanel.Visible = false;
            AddChild(_factionsNarrativePanel);

            _skillMatrixPanel = new SkillMatrixPanel { Visible = false };
            _skillMatrixPanel.OnClose += () => _skillMatrixPanel.Visible = false;
            AddChild(_skillMatrixPanel);

            _survivalWorkstationPanel = new SurvivalWorkstationPanel { Visible = false };
            _survivalWorkstationPanel.OnClose += () => _survivalWorkstationPanel.Visible = false;
            AddChild(_survivalWorkstationPanel);

            _verdictDashboardPanel = new VerdictDashboardPanel { Visible = false };
            _verdictDashboardPanel.OnClose += () => _verdictDashboardPanel.Visible = false;
            AddChild(_verdictDashboardPanel);

            _mapAtlasPanel = new MapAtlasPanel { Visible = false };
            _mapAtlasPanel.OnClose += () => _mapAtlasPanel.Visible = false;
            AddChild(_mapAtlasPanel);

            _maritimeAtlasPanel = new MaritimeAtlasPanel { Visible = false };
            _maritimeAtlasPanel.OnClose += () => _maritimeAtlasPanel.Visible = false;
            AddChild(_maritimeAtlasPanel);

            _musterAtlasPanel = new MusterAtlasPanel { Visible = false };
            _musterAtlasPanel.OnClose += () => _musterAtlasPanel.Visible = false;
            AddChild(_musterAtlasPanel);

            _questsAtlasPanel = new QuestsAtlasPanel { Visible = false };
            _questsAtlasPanel.OnClose += () => _questsAtlasPanel.Visible = false;
            AddChild(_questsAtlasPanel);

            _researchAtlasPanel = new ResearchAtlasPanel { Visible = false };
            _researchAtlasPanel.OnClose += () => _researchAtlasPanel.Visible = false;
            AddChild(_researchAtlasPanel);

            _standingRecordAtlasPanel = new StandingRecordAtlasPanel { Visible = false };
            _standingRecordAtlasPanel.OnClose += () => _standingRecordAtlasPanel.Visible = false;
            AddChild(_standingRecordAtlasPanel);

            _combatHudOverlay = new CombatHudOverlay { Visible = false };
            _combatHudOverlay.OnClose += () => _combatHudOverlay.Visible = false;
            AddChild(_combatHudOverlay);

            _biogasDigesterPanel = new AnaerobicBiogasDigesterPanel { Visible = false };
            _biogasDigesterPanel.OnClose += () => _biogasDigesterPanel.Visible = false;
            AddChild(_biogasDigesterPanel);

            _cartographyGisPanel = new SubterraneanCartographyPanel { Visible = false };
            _cartographyGisPanel.OnClose += () => _cartographyGisPanel.Visible = false;
            AddChild(_cartographyGisPanel);

            _printingPressPanel = new UndergroundPrintingPressPanel { Visible = false };
            _printingPressPanel.OnClose += () => _printingPressPanel.Visible = false;
            AddChild(_printingPressPanel);

            _siliconSlicingPanel = new SiliconIngotSlicingPanel { Visible = false };
            _siliconSlicingPanel.OnClose += () => _siliconSlicingPanel.Visible = false;
            AddChild(_siliconSlicingPanel);

            _geothermalTurbinePanel = new GeothermalSteamTurbinePanel { Visible = false };
            _geothermalTurbinePanel.OnClose += () => _geothermalTurbinePanel.Visible = false;
            AddChild(_geothermalTurbinePanel);

            _warDogKennelPanel = new WarDogKennelPanel { Visible = false };
            _warDogKennelPanel.OnClose += () => _warDogKennelPanel.Visible = false;
            AddChild(_warDogKennelPanel);

            _isotopeSeparatorPanel = new IsotopeSeparatorPanel { Visible = false };
            _isotopeSeparatorPanel.OnClose += () => _isotopeSeparatorPanel.Visible = false;
            AddChild(_isotopeSeparatorPanel);

            _plasmaSmeltingPanel = new PlasmaArcSmeltingPanel { Visible = false };
            _plasmaSmeltingPanel.OnClose += () => _plasmaSmeltingPanel.Visible = false;
            AddChild(_plasmaSmeltingPanel);

            _boreholeSeismographPanel = new BoreholeSeismographPanel { Visible = false };
            _boreholeSeismographPanel.OnClose += () => _boreholeSeismographPanel.Visible = false;
            AddChild(_boreholeSeismographPanel);

            _logisticsAirlockPanel = new HeavyLogisticsAirlockPanel { Visible = false };
            _logisticsAirlockPanel.OnClose += () => _logisticsAirlockPanel.Visible = false;
            AddChild(_logisticsAirlockPanel);

            _cryoPermafrostCorePanel = new CryogenicPermafrostCorePanel { Visible = false };
            _cryoPermafrostCorePanel.OnClose += () => _cryoPermafrostCorePanel.Visible = false;
            AddChild(_cryoPermafrostCorePanel);

            _basalRadonMigrationPanel = new BasalRadonMigrationPanel { Visible = false };
            _basalRadonMigrationPanel.OnClose += () => _basalRadonMigrationPanel.Visible = false;
            AddChild(_basalRadonMigrationPanel);

            _traumaBondingCohortPanel = new TraumaBondingCohortPanel { Visible = false };
            _traumaBondingCohortPanel.OnClose += () => _traumaBondingCohortPanel.Visible = false;
            AddChild(_traumaBondingCohortPanel);

            _clandestineInsurgencyPanel = new ClandestineInsurgencyPanel { Visible = false };
            _clandestineInsurgencyPanel.OnClose += () => _clandestineInsurgencyPanel.Visible = false;
            AddChild(_clandestineInsurgencyPanel);

            _subterraneanDebtLedgerPanel = new SubterraneanDebtLedgerPanel { Visible = false };
            _subterraneanDebtLedgerPanel.OnClose += () => _subterraneanDebtLedgerPanel.Visible = false;
            AddChild(_subterraneanDebtLedgerPanel);

            _surfaceShrapnelAegisPanel = new SurfaceShrapnelAegisPanel { Visible = false };
            _surfaceShrapnelAegisPanel.OnClose += () => _surfaceShrapnelAegisPanel.Visible = false;
            AddChild(_surfaceShrapnelAegisPanel);

            _longWalkExpeditionPanel = new LongWalkExpeditionPanel { Visible = false };
            _longWalkExpeditionPanel.OnClose += () => _longWalkExpeditionPanel.Visible = false;
            AddChild(_longWalkExpeditionPanel);

            _sonicRuptureDrillPanel = new SonicRuptureDrillPanel { Visible = false };
            _sonicRuptureDrillPanel.OnClose += () => _sonicRuptureDrillPanel.Visible = false;
            AddChild(_sonicRuptureDrillPanel);

            _vaultDoorBreachingPanel = new VaultDoorBreachingPanel { Visible = false };
            _vaultDoorBreachingPanel.OnClose += () => _vaultDoorBreachingPanel.Visible = false;
            AddChild(_vaultDoorBreachingPanel);

            _ironCenotaphMemorialPanel = new IronCenotaphMemorialPanel { Visible = false };
            _ironCenotaphMemorialPanel.OnClose += () => _ironCenotaphMemorialPanel.Visible = false;
            AddChild(_ironCenotaphMemorialPanel);

            _aquiferTreatyConcessionPanel = new AquiferTreatyConcessionPanel { Visible = false };
            _aquiferTreatyConcessionPanel.OnClose += () => _aquiferTreatyConcessionPanel.Visible = false;
            AddChild(_aquiferTreatyConcessionPanel);

            _crossingSafeConductVouchPanel = new CrossingSafeConductVouchPanel { Visible = false };
            _crossingSafeConductVouchPanel.OnClose += () => _crossingSafeConductVouchPanel.Visible = false;
            AddChild(_crossingSafeConductVouchPanel);

            _mechanicalProstheticsLathePanel = new MechanicalProstheticsLathePanel { Visible = false };
            _mechanicalProstheticsLathePanel.OnClose += () => _mechanicalProstheticsLathePanel.Visible = false;
            AddChild(_mechanicalProstheticsLathePanel);

            _fungalProteinFermenterPanel = new FungalProteinFermenterPanel { Visible = false };
            _fungalProteinFermenterPanel.OnClose += () => _fungalProteinFermenterPanel.Visible = false;
            AddChild(_fungalProteinFermenterPanel);

            _ultrasonicDecontamAirlockPanel = new UltrasonicDecontaminationAirlockPanel { Visible = false };
            _ultrasonicDecontamAirlockPanel.OnClose += () => _ultrasonicDecontamAirlockPanel.Visible = false;
            AddChild(_ultrasonicDecontamAirlockPanel);

            _troposphericRadioRelayPanel = new TroposphericRadioRelayPanel { Visible = false };
            _troposphericRadioRelayPanel.OnClose += () => _troposphericRadioRelayPanel.Visible = false;
            AddChild(_troposphericRadioRelayPanel);

            _inductionCupolaFurnacePanel = new InductionCupolaFurnacePanel { Visible = false };
            _inductionCupolaFurnacePanel.OnClose += () => _inductionCupolaFurnacePanel.Visible = false;
            AddChild(_inductionCupolaFurnacePanel);

            _heavyMarineDieselGenPanel = new HeavyMarineDieselGeneratorPanel { Visible = false };
            _heavyMarineDieselGenPanel.OnClose += () => _heavyMarineDieselGenPanel.Visible = false;
            AddChild(_heavyMarineDieselGenPanel);

            _slurryDewateringSumpPanel = new SlurryDewateringSumpPanel { Visible = false };
            _slurryDewateringSumpPanel.OnClose += () => _slurryDewateringSumpPanel.Visible = false;
            AddChild(_slurryDewateringSumpPanel);

            _magneticDrumArchivePanel = new MagneticDrumArchivePanel { Visible = false };
            _magneticDrumArchivePanel.OnClose += () => _magneticDrumArchivePanel.Visible = false;
            AddChild(_magneticDrumArchivePanel);

            // ── Game content area ──
            var margin = new MarginContainer();
            margin.SizeFlagsVertical = SizeFlags.ExpandFill;
            margin.AddThemeConstantOverride("margin_left", 60);
            margin.AddThemeConstantOverride("margin_top", 10);
            margin.AddThemeConstantOverride("margin_right", 60);
            margin.AddThemeConstantOverride("margin_bottom", 50);
            gameUiContainer.AddChild(margin);

            // MarginContainer gives EVERY child the same full rect, so it must hold
            // exactly one child. Adding the diagnostics bar as a second child made it
            // render on top of the whole UI instead of docking at the bottom. Root the
            // content in a VBox and let that own the split + the diagnostics strip.
            var rootColumn = new VBoxContainer();
            rootColumn.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            margin.AddChild(rootColumn);

            var hSplit = new HSplitContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            rootColumn.AddChild(hSplit);

            // Left Column: Branding and Menu
            var leftBox = new VBoxContainer
            {
                CustomMinimumSize = new Vector2(480, 0)
            };
            leftBox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingLg);
            hSplit.AddChild(leftBox);

            _titleLabel = new Label
            {
                Text = "ASHFALL\nATOMIC WAR: STARVING SURVIVAL",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _titleLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH1);
            _titleLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            leftBox.AddChild(_titleLabel);

            var subtitle = new Label
            {
                Text = "Post-Nuclear Survival Strategy & Narrative RPG\nPowered by Godot Engine (.NET Edition)",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            subtitle.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            subtitle.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            leftBox.AddChild(subtitle);

            leftBox.AddChild(new HSeparator());

            _menuContainer = new VBoxContainer();
            _menuContainer.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            leftBox.AddChild(_menuContainer);

            MainMenuBuilder.BuildMenu(
                _menuContainer,
                AddMenuButton,
                AddSectionHeader,
                OnColdCountClicked,
                OnHydroBaronsClicked,
                OnIronRaidersClicked,
                OnLongWalkClicked,
                OnProvisionedClicked,
                OnScavengerGuildClicked,
                OnStartGameClicked,
                OnTickIceRoadClicked,
                OnCycleWeatherClicked,
                OnShowBriefingClicked,
                OnCensusLevyClicked,
                OnOrder12CClicked,
                OnUnlockPlantClicked,
                OnRepairMembraneClicked,
                OnToggleOutfallClicked,
                OnSaveHoldfastClicked,
                OnHoldfastOpenClicked,
                OnHoldfastNewLedgerClicked,
                OnCycleEndingClicked,
                OnRosterInspectWallClicked,
                OnRosterPencilClicked,
                OnRosterInkClicked,
                OnRosterBurnClicked,
                OnRosterTickNightClicked,
                OnRosterVisitorClicked,
                OnRosterSecondWinterClicked,
                OnWaystationTickClicked,
                OnWaystationWatchClicked,
                OnStandingRecordClicked,
                OnRecordWalkKm19Clicked,
                OnCrossingVouchClicked,
                OnCrossingBurnClicked,
                OnArbitrationLoadBackersClicked,
                OnArbitrationCallStandingClicked,
                OnArbitrationBribeClicked,
                OnArbitrationOverturnClicked,
                OnLedgerSignClicked,
                OnLedgerTickClicked,
                OnLedgerPayClicked,
                OnGreenhousePlantClicked,
                OnGreenhouseTickClicked,
                OnDoorEncounterClicked,
                OnTickYearOfAshClicked,
                OnQuestlinesClicked,
                OnPhantomScavengeClicked,
                OnPhantomTickClicked,
                OnPhase0ScavengeClicked,
                OnPhase0NoiseClicked,
                OnPhase0CraftClicked,
                OnPhase0TickClicked,
                OnDoseSealClicked,
                OnDoseScribeClicked,
                OnDoseDiagnoseClicked,
                OnDoseCohortClicked,
                OnDoseVolunteerClicked,
                OnDoseRegisterClicked,
                OnMusterEscalateClicked,
                OnMusterRallyClicked,
                OnMusterWitnessesClicked,
                OnVerdictOpenClicked,
                OnVerdictTickClicked,
                OnVerdictCensusClicked,
                OnMaritimeStartDiveClicked,
                OnMaritimeTickDiveClicked,
                OnMaritimeScavengeClicked,
                OnExpeditionTickClicked,
                OnExpeditionDiveClicked,
                OnViewCodexClicked,
                OnDiagnosticsClicked,
                OnEconomyOpenClicked,
                OnInventoryOpenClicked,
                OnSurvivorsOpenClicked,
                OpenSettingsPanel,
                OpenCraftingPanel,
                OpenRadioPanel,
                OpenMedicalPanel,
                OpenPhase0Panel,
                OpenDutyRosterPanel,
                OpenExpeditionPanel,
                OpenWeatherPanel,
                OpenQuestsPanel,
                OpenJournalPanel,
                OpenFactionsPanel,
                OpenShelterPanel,
                OpenCombatPanel,
                OpenMapPanel,
                OpenSaveLoadPanel,
                OnExitGameClicked);

            _statusLabel = new Label
            {
                Text = "System Status: Initializing game state...",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _statusLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _statusLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot));
            leftBox.AddChild(_statusLabel);

            _iceRoadLabel = new Label
            {
                Text = "Ice road: not wired",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _iceRoadLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _iceRoadLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            leftBox.AddChild(_iceRoadLabel);

            _catalogLabel = new Label
            {
                Text = "Holdfast catalog: —",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _catalogLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _catalogLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            leftBox.AddChild(_catalogLabel);

            _briefingPreviewLabel = new Label
            {
                Text = "Quest briefing: —",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _briefingPreviewLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            _briefingPreviewLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            leftBox.AddChild(_briefingPreviewLabel);

            // Right Column: Terminal / Codex Viewer
            var rightBox = new VBoxContainer();
            rightBox.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            hSplit.AddChild(rightBox);
            _rightColumn = rightBox;

            var codexHeader = new Label
            {
                Text = "DATA TERMINAL & SURVIVAL LOGS"
            };
            codexHeader.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH3);
            codexHeader.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            rightBox.AddChild(codexHeader);

            _codexViewer = new TextEdit
            {
                Editable = false,
                WrapMode = TextEdit.LineWrappingMode.Boundary,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _codexViewer.AddThemeColorOverride("background_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.InkPanel));
            _codexViewer.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            rightBox.AddChild(_codexViewer);

            // Bottom Diagnostics bar
            _diagnosticsLabel = new Label
            {
                Text = "FPS: 60 | Static Mem: 0 MB"
            };
            _diagnosticsLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            _diagnosticsLabel.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            rootColumn.AddChild(_diagnosticsLabel);

            // Year of Ash Door Encounter Modal
            _questlineModal = new QuestlineModal();
            AddChild(_questlineModal);
            _questlineModal.OnQuestlineChosen += OnQuestlineChosen;
            _questlineModal.OnChoiceTaken += OnQuestlineChoiceTaken;

            _doorModal = new DoorEncounterModal();
            AddChild(_doorModal);
            _doorModal.OnChoiceClicked += OnDoorEncounterChoiceClicked;

            // ── Main Menu (overlay, shown initially) ──
            _mainMenu = new MainMenuPanel();
            _mainMenu.OnNewGame += StartNewGame;
            _mainMenu.OnContinue += ContinueGame;
            _mainMenu.OnSettings += () => { _settingsPanel.Open(); };
            _mainMenu.OnCodex += () => { OpenPlayerPanel("codex"); };
            _mainMenu.OnQuit += () => { SaveAll(); GetTree().Quit(); };
            AddChild(_mainMenu);

            // ── Game Over (overlay, hidden) ──
            _gameOver = new GameOverPanel();
            _gameOver.OnNewGame += StartNewGame;
            _gameOver.OnReturnToMenu += ReturnToMenu;
            AddChild(_gameOver);

            // ── Check for existing save ──
            UpdateContinueButton();

            // ── Setup Expanded Shelter Systems (Water, Airlock, Relations, Treaties, etc.) ──
            SetupExpandedShelterSystems();

            // ── Register Typed Player Surface Actions ──
            RegisterPlayerSurfaces();

            // ── Start in menu state ──
            _state = GameState.Menu;
        }

        private void UpdateContinueButton()
        {
            bool hasSave = false;
            if (_saveLoadHost != null)
            {
                // Continue requires at least one slot that is not terminal. A
                // run-finalized slot is a sealed memorial/archive, not a
                // continuable save, so it must not enable Continue.
                var slots = _saveLoadHost.GetSlots();
                for (int i = 0; i < slots.Count; i++)
                {
                    var card = _saveLoadHost.BuildSlotCard(slots[i]);
                    if (card.Exists && !card.IsTerminalIronMan)
                    {
                        hasSave = true;
                        break;
                    }
                }
            }
            if (!hasSave)
            {
                // Fall back to legacy global save files.
                hasSave = System.IO.File.Exists(HoldfastSaveStore.SavePath) ||
                          System.IO.File.Exists(InventorySaveStore.SavePath) ||
                          System.IO.File.Exists(SurvivorsSaveStore.SavePath);
            }
            _mainMenu?.EnableContinue(hasSave);
        }

        private void AddMenuButton(string text, Action callback)
        {
            var btn = new Button
            {
                Text = text,
                CustomMinimumSize = new Vector2(0, Ashfall.Core.UI.Theme.FontSizeBody + Ashfall.Core.UI.Theme.SpacingLg)
            };
            btn.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            btn.Pressed += callback;
            _menuContainer.AddChild(btn);
        }

        private void AddSectionHeader(string title)
        {
            var lbl = AtomicWar.GodotApp.UI.AshfallUiHelpers.MakeSectionHeader(title);
            lbl.AddThemeColorOverride("font_color", AtomicWar.GodotApp.UI.AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            _menuContainer.AddChild(lbl);
        }

        // -----------------------------------------------------------------
        // Journal wiring
        // -----------------------------------------------------------------
    }
}
