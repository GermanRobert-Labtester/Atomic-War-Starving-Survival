// TODO(phase12): Main.cs is a 6,640-line partial-class monolith with 74 Setup/Save/Flush triads.
// Risk: triad drift (Setup without Save) is mitigated by I1/I2 fixes, but the file remains
// hard to navigate. Consider splitting into per-domain partials (EconomyHostSession, JournalHostSession,
// SurvivorsHostSession, etc.) and move the 74 triad methods into those files. Keep this file
// as the single entry point that wires systems and owns the Godot scene tree.

using Godot;
using System;
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
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;



namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private Label _titleLabel = null!;
        private Label _statusLabel = null!;
        private Label _diagnosticsLabel = null!;
        private Label _iceRoadLabel = null!;
        private Label _catalogLabel = null!;
        private Label _briefingPreviewLabel = null!;
        private VBoxContainer _menuContainer = null!;
        private TextEdit _codexViewer = null!;

        // Year of Ash (Days 180-360)
        private YearOfAshHostSession _yearOfAsh = null!;
        private bool _yearOfAshDirty;
        private DoorEncounterModal _doorModal = null!;
        private QuestlineModal _questlineModal = null!;
        private int _doorEncounterIndex = 0;
        private FactionWarMapWidget _factionWarMap = null!;
        private RadioBroadcastTerminal _radioTerminal = null!;
        private GeothermalHeatingWidget _geothermalWidget = null!;
        private RadonVentilationWidget _radonWidget = null!;
        private VBoxContainer _yearOfAshPanel = null!;
        private VBoxContainer _rightColumn = null!;
        private PhantomMemoryHostSession _phantomMemory = null!;
        private Phase0HostSession _phase0 = null!;
        private bool _phase0Dirty;

        // Phase 0 — Campaign Day Coordinator (single authority for day-advance)
        private CampaignDayCoordinator _campaignDay = null!;
        private DailyBriefingState _dailyBriefing = null!;
        private DailyBriefingModal _dailyBriefingModal = null!;
        private bool _briefingPending;
        private bool _dailyBriefingDirty;

        // Phase 1.7 — Medical Ward (item 11)
        private Ashfall.Core.Medical.MedicalWardSystem _medicalWard = null!;
        private bool _medicalWardDirty;
        // Phase 1.9 — Memorial (item 15)
        private Ashfall.Core.Memorial.MemorialSystem _memorial = null!;
        private bool _memorialDirty;
        // Phase 2.10 — Travel Map (item 4)
        private Ashfall.Core.World.WastelandMapSystem _wastelandMap = null!;
        // Phase 2.11 — Encounter Choice Resolver (item 5)
        private Ashfall.Core.Expeditions.EncounterChoiceResolver _encounterChoice = null!;
        private bool _encounterChoiceDirty;
        private DoseLedgerHostSession _doseLedger = null!;
        private bool _doseLedgerDirty;
        private DoseRegisterSurface _doseSurface = null!;

        // Muster (Expansion 06, Days 180-360 escalation)
        private MusterHostSession _muster = null!;
        private CurrentsRosterWidget _currentsRoster = null!;
        private ApproachSelectionModal _approachModal = null!;
        private DeserterCoalitionCampWidget _campWidget = null!;
        private JournalWitnessPanel _witnessPanel = null!;

        // ASHFALL: THE VERDICT (Expansion 08 — the machine that keeps the count)
        private AtomicWar.GodotApp.VerdictHostSession _verdict = null!;
        private Godot.Label _verdictReadoutLabel = null!;
        private VerdictPanel _verdictPanel = null!;
        private bool _verdictDirty;

        // ASHFALL: THE BLACK FLOTILLA (Expansion 09 — maritime salvage & stealth dive)
        private MaritimeHostSession _maritime = null!;
        private bool _maritimeDirty;
        private DeepCoastHostSession _deepCoast = null!;

        // Expedition (Encounters port + dive instance)
        private ExpeditionHostSession _expeditions = null!;
        private bool _expeditionDirty;

        // ASHFALL: COMBAT EXPANSION (Expansion 06 — tactical combat authority)
        private CombatHostSession _combat = null!;
        private bool _combatDirty;

        // Narrative (encounters port), Medical (chemical dependency), World (weather), Crafting
        private NarrativeHostSession _narrative = null!;
        private bool _narrativeDirty;
        private MedicalHostSession _medical = null!;
        private bool _medicalDirty;
        private WorldHostSession _world = null!;
        private bool _worldDirty;
        private RadioHostSession _radio = null!;
        private CraftingHostSession _crafting = null!;
        private bool _craftingDirty;

        // ASHFALL: traveling caravans (Expansion V spec §3.3 — wandering merchants)
        private TravelingCaravanHostSession _caravans = null!;
        private bool _caravansDirty;

        // Inventory (ported from Unity _Game/Inventory)
        private InventoryHostSession _inventory = null!;
        private AtomicWar.GodotApp.UI.InventoryPanel _inventoryPanel = null!;

        // Survivors (needs + radiation, ported from Unity Survivors/Radiation)
        private SurvivorsHostSession _survivors = null!;
        private EconomyHostSession _economy = null!;
        private bool _economyDirty;
        private EconomyMarketPanel _economyPanel = null!;
        private UtilityAiHostSession _utilityAi = null!;
        private UtilityAiPanel _utilityAiPanel = null!;

        // Journal (docs/ui/JOURNAL_UI_PLAN.md)
        private JournalSystem _journal = null!;
        private JournalCodex _journalCodex = null!;
        private JournalBookUI _journalBook = null!;
        private Ashfall.Core.Events.SimpleEventBus _eventBus = new Ashfall.Core.Events.SimpleEventBus();
        private AtomicWar.GodotApp.Host.HostEventAdapter _hostEventAdapter = null!;
        private string _dataDir = string.Empty;
        private int _simDay = 4;

        // Diagnostics strip throttling. Engine.GetVersionInfo() allocates a Godot
        // Dictionary, so the version string is resolved once and cached for the process.
        private const double DiagnosticsRefreshSeconds = 0.25;
        private static readonly string s_engineVersion =
            Engine.GetVersionInfo()["string"].AsString();
        private double _diagnosticsAccum;
        private double _diagnosticsLogAccum;

        // Journal save coalescing. Saving on every entry rewrote the whole file once
        // per seeded entry; entries are marked dirty and flushed on the diagnostics tick,
        // on close, and on quit instead.
        private bool _journalDirty;

        // Phase 0 core slice: ice-road seasonal gate + Holdfast catalogs
        private CoreDemoSession _core = null!;
        // Playable Holdfast vertical slice: catalog-backed terminal + mutable trade state.
        private HoldfastRuntimeSession _holdfastRuntime = null!;
        private HoldfastTerminalPanel _holdfastTerminal = null!;
        // ASHFALL: THE DUTY ROSTER (Exp 02) — chart, marks, encounters
        private DutyRosterHostSession _dutyRoster = null!;
        private ExpansionHostSession _expansions = null!;
        // ASHFALL: THE SILENT FOUNDRY (Exp 10) — thin presentation wrapper over
        // the Core system owned by the expansion hub.
        private AtomicWar.GodotApp.SilentFoundryHostSession _silentFoundry = null!;
        private SilentFoundryPanel _silentFoundryPanel = null!;
        // ASHFALL: DISEASE EXPANSION — thin presentation wrapper over the Core
        // contagion engine owned by the expansion hub (rides the hub save).
        private AtomicWar.GodotApp.DiseaseHostSession _disease = null!;
        private AtomicWar.GodotApp.Economy.TradeScreenGodotPanel _tradePanel = null!;
        private Ashfall.Core.Radio.FactionRadioEngine _tradeRadio = null!;
        // Holdfast S1 save coalescing (same pattern as the journal): any state
        // change in IceRoad or Census marks the save dirty; the diagnostics tick
        // flushes it. Quit and the explicit menu button flush immediately.
        private bool _holdfastDirty;
        // Duty Roster (Exp 02) and Expansion Hub save coalescing — same pattern.
        private bool _dutyRosterDirty;
        private bool _expansionHubDirty;
        private bool _foundryDirty;

        // Sleep / Advance confirmation fields
        private const double AdvanceCountdownDefaultSeconds = 3.0;
        private double _advanceTimerRemaining;
        private bool _advanceConfirmed;
        private bool _advanceCancelled;

        // ── Game flow state ───────────────────────────────────────────
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
        private RadioPanel _radioPanel = null!;
        private MedicalPanel _medicalPanel = null!;
        private Phase0Panel _phase0Panel = null!;
        private DutyRosterPanel _dutyRosterPanel = null!;
        private EconomyOverlayPanel _economyOverlayPanel = null!;
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
        private StartingLevelHostSession _startingLevel = null!;
        private bool _startingLevelDirty;
        private OpeningProtocolModal _openingProtocolModal = null!;
        private PowerGridHostSession _powerGrid = null!;
        private PowerGridPanel _powerGridPanel = null!;
        private bool _powerGridDirty;
        private GreenhouseHostSession _greenhouse = null!;
        private GreenhousePanel _greenhousePanel = null!;
        private bool _greenhouseDirty;
        private CombatPanel _combatPanel = null!;
        private MapPanel _mapPanel = null!;
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
        private MedicalDetailPanel _medicalDetailPanel = null!;
        private ExpeditionDetailPanel _expeditionDetailPanel = null!;
        private RadioDetailPanel _radioDetailPanel = null!;
        private ShelterDetailPanel _shelterDetailPanel = null!;
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
        private CraftingDetailPanel _craftingDetailPanel = null!;
        private TradeDetailPanel _tradeDetailPanel = null!;
        private ResearchDetailPanel _researchDetailPanel = null!;
        private WeatherHistoryPanel _weatherHistoryPanel = null!;
        private FactionHistoryPanel _factionHistoryPanel = null!;
        private MedicalHistoryPanel _medicalHistoryPanel = null!;
        private ExpeditionHistoryPanel _expeditionHistoryPanel = null!;
        private ShelterHistoryPanel _shelterHistoryPanel = null!;
        private CraftingHistoryPanel _craftingHistoryPanel = null!;
        private enum GameState { Menu, Playing, GameOver }
        private GameState _state = GameState.Menu;

        public override void _Ready()
        {
            GD.Print("[Ashfall Godot] Initializing ASHFALL: Atomic War - Starving Survival...");

            ResolveDataDir();
            switch (HostCli.Parse(OS.GetCmdlineUserArgs()))
            {
                case HostCliAction.Help:
                    HostCli.PrintHelp();
                    GetTree().Quit(0);
                    return;
                case HostCliAction.ExpansionsSelfTest:
                    GetTree().Quit(HostCli.RunExpansionsSelfTest(_dataDir));
                    return;
                case HostCliAction.HoldfastSelfTest:
                    GetTree().Quit(HostCli.RunHoldfastSelfTest(_dataDir));
                    return;
                case HostCliAction.DutyRosterSelfTest:
                    GetTree().Quit(HostCli.RunDutyRosterSelfTest(_dataDir));
                    return;
                case HostCliAction.StandingRecordSelfTest:
                    GetTree().Quit(HostCli.RunStandingRecordSelfTest(_dataDir));
                    return;
                case HostCliAction.CrossingSelfTest:
                    GetTree().Quit(HostCli.RunCrossingSelfTest(_dataDir));
                    return;
                case HostCliAction.ArbitrationSelfTest:
                    GetTree().Quit(HostCli.RunArbitrationSelfTest());
                    return;
                case HostCliAction.LedgerDebtSelfTest:
                    GetTree().Quit(HostCli.RunLedgerDebtSelfTest());
                    return;
                case HostCliAction.GreenhouseSelfTest:
                    GetTree().Quit(HostCli.RunGreenhouseSelfTest());
                    return;
                case HostCliAction.SilentFoundrySelfTest:
                    GetTree().Quit(HostCli.RunSilentFoundrySelfTest(_dataDir));
                    return;
                case HostCliAction.DiseaseSelfTest:
                    GetTree().Quit(HostCli.RunDiseaseSelfTest(_dataDir));
                    return;
                case HostCliAction.CombatSelfTest:
                    GetTree().Quit(HostCli.RunCombatSelfTest(_dataDir));
                    return;
                case HostCliAction.SilentFoundryUiTest:
                    RunSilentFoundryUiTestAndQuit();
                    return;
                case HostCliAction.DutyRosterUiTest:
                    RunDutyRosterUiTestAndQuit();
                    return;
                case HostCliAction.IceRoadSelfTest:
                    GetTree().Quit(HostCli.RunIceRoadSelfTest(_dataDir));
                    return;
                case HostCliAction.CensusSelfTest:
                    GetTree().Quit(HostCli.RunCensusSelfTest());
                    return;
                case HostCliAction.CoreSelfTest:
                    GetTree().Quit(HostCli.RunCoreSelfTest(_dataDir));
                    return;
                case HostCliAction.HoldfastBriefing:
                    GetTree().Quit(HostCli.RunHoldfastBriefing(_dataDir));
                    return;
                case HostCliAction.IceRoadTickDemo:
                    GetTree().Quit(HostCli.RunIceRoadTickDemo(_dataDir));
                    return;
                case HostCliAction.HoldfastSaveSelfTest:
                    GetTree().Quit(HostCli.RunHoldfastSaveSelfTest(_dataDir));
                    return;
                case HostCliAction.HoldfastRuntimeUiTest:
                    RunHoldfastRuntimeUiTestAndQuit();
                    return;
                case HostCliAction.BrineSelfTest:
                    GetTree().Quit(HostCli.RunBrineSelfTest());
                    return;
                case HostCliAction.MusterSelfTest:
                    GetTree().Quit(HostCli.RunMusterSelfTest());
                    return;
                case HostCliAction.VerdictSelfTest:
                    GetTree().Quit(HostCli.RunVerdictSelfTest(_dataDir));
                    return;
                case HostCliAction.ClusterSelfTest:
                    GetTree().Quit(HostCli.RunClusterSelfTest(_dataDir));
                    return;
                case HostCliAction.EndingsSelfTest:
                    GetTree().Quit(HostCli.RunEndingsSelfTest());
                    return;
                case HostCliAction.JournalSelfTest:
                    RunSelfTestAndQuit();
                    return;
                case HostCliAction.JournalUiTest:
                    RunJournalUiTestAndQuit();
                    return;
                case HostCliAction.DashboardUiTest:
                    RunDashboardUiTestAndQuit();
                    return;
                case HostCliAction.PlayerPanelsUiTest:
                    RunPlayerPanelsUiTestAndQuit();
                    return;
                case HostCliAction.MusterUiTest:
                    RunMusterUiTestAndQuit();
                    return;
                case HostCliAction.DoseUiTest:
                    RunDoseUiTestAndQuit();
                    return;
                case HostCliAction.VerdictUiTest:
                    RunVerdictUiTestAndQuit();
                    return;
                case HostCliAction.EconomyUiTest:
                    RunEconomyUiTestAndQuit();
                    return;
                case HostCliAction.UtilityAiSelfTest:
                    GetTree().Quit(HostCli.RunUtilityAiSelfTest(_dataDir));
                    return;
                case HostCliAction.UtilityAiUiTest:
                    RunUtilityAiUiTestAndQuit();
                    return;
                case HostCliAction.InventoryUiTest:
                    RunInventoryUiTestAndQuit();
                    return;
                case HostCliAction.ExpeditionPanelUiTest:
                    RunExpeditionPanelUiTestAndQuit();
                    return;
                case HostCliAction.SurvivorsUiTest:
                    RunSurvivorsUiTestAndQuit();
                    return;
                case HostCliAction.Phase0UiTest:
                    RunPhase0UiTestAndQuit();
                    return;
                case HostCliAction.YearOfAshSaveSelfTest:
                    GetTree().Quit(HostCli.RunYearOfAshSaveSelfTest(_dataDir));
                    return;
                case HostCliAction.DutyRosterSaveSelfTest:
                    GetTree().Quit(HostCli.RunDutyRosterSaveSelfTest(_dataDir));
                    return;
                case HostCliAction.ExpansionHubSaveSelfTest:
                    GetTree().Quit(HostCli.RunExpansionHubSaveSelfTest(_dataDir));
                    return;
                case HostCliAction.DoseLedgerSelfTest:
                    GetTree().Quit(HostCli.RunDoseLedgerSelfTest(_dataDir));
                    return;
                case HostCliAction.ExpeditionSelfTest:
                    GetTree().Quit(HostCli.RunExpeditionSelfTest());
                    return;
                case HostCliAction.BridgeSelfTest:
                    GetTree().Quit(HostCli.RunBridgeSelfTest());
                    return;
                case HostCliAction.ExpeditionEncounterBridgeSelfTest:
                    GetTree().Quit(HostCli.RunExpeditionEncounterBridgeSelfTest());
                    return;
                case HostCliAction.MedicalSelfTest:
                    GetTree().Quit(HostCli.RunMedicalSelfTest());
                    return;
                case HostCliAction.NarrativeSelfTest:
                    GetTree().Quit(HostCli.RunNarrativeSelfTest());
                    return;
                case HostCliAction.SurvivorsSelfTest:
                    GetTree().Quit(HostCli.RunSurvivorsSelfTest());
                    return;
                case HostCliAction.WorldSelfTest:
                    GetTree().Quit(HostCli.RunWorldSelfTest());
                    return;
                case HostCliAction.EconomySelfTest:
                    GetTree().Quit(HostCli.RunEconomySelfTest(_dataDir));
                    return;
                case HostCliAction.DataIntegritySelfTest:
                    GetTree().Quit(HostCli.RunDataIntegritySelfTest(_dataDir));
                    return;
                case HostCliAction.CaravanSelfTest:
                    GetTree().Quit(HostCli.RunCaravanSelfTest());
                    return;
                case HostCliAction.AssetRegistrySelfTest:
                    GetTree().Quit(HostCli.RunAssetRegistrySelfTest(_dataDir));
                    return;
                case HostCliAction.AssetCoverageReport:
                    GetTree().Quit(HostCli.RunAssetCoverageReport(_dataDir));
                    return;
                case HostCliAction.StandaloneSystemsSelfTest:
                    GetTree().Quit(HostCli.RunStandaloneSystemsSelfTest());
                    return;
                case HostCliAction.DeepCoastSelfTest:
                    GetTree().Quit(HostCli.RunDeepCoastSelfTest(_dataDir));
                    return;
                case HostCliAction.DeepCoastHostSelfTest:
                    GetTree().Quit(HostCli.RunDeepCoastHostSelfTest());
                    return;
                case HostCliAction.WarlordSelfTest:
                    GetTree().Quit(HostCli.RunWarlordSelfTest(_dataDir));
                    return;
                case HostCliAction.WarlordHostSelfTest:
                    GetTree().Quit(HostCli.RunWarlordHostSelfTest(_dataDir));
                    return;
                case HostCliAction.WarlordUiSelfTest:
                    GetTree().Quit(HostCli.RunWarlordUiSelfTest(_dataDir));
                    return;
                case HostCliAction.Phase0SelfTest:
                    GetTree().Quit(HostCli.RunPhase0SelfTest());
                    return;
                case HostCliAction.Day1PlayableSelfTest:
                    GetTree().Quit(HostCli.RunDay1PlayableSelfTest(_dataDir));
                    return;
                case HostCliAction.Day1ToDay2MilestoneSelfTest:
                    GetTree().Quit(HostCli.RunDay1ToDay2MilestoneSelfTest(_dataDir));
                    return;
                case HostCliAction.UiLayoutSelfTest:
                    GetTree().Quit(HostCli.RunUiLayoutSelfTest(_dataDir));
                    return;
                case HostCliAction.SettingsSelfTest:
                    GetTree().Quit(HostCli.RunSettingsSelfTest(_dataDir));
                    return;
                case HostCliAction.PlayableShellSelfTest:
                    GetTree().Quit(HostCli.RunPlayableShellSelfTest(_dataDir));
                    return;
                case HostCliAction.ShelterHazardLoopSelfTest:
                    GetTree().Quit(HostCli.RunShelterHazardLoopSelfTest(_dataDir));
                    return;
                case HostCliAction.ShelterOperationsSelfTest:
                    GetTree().Quit(HostCli.RunShelterOperationsSelfTest(_dataDir));
                    return;
                case HostCliAction.AudioSelfTest:
                    GetTree().Quit(AtomicWar.GodotApp.Audio.AudioSelfTest.Run());
                    return;
                case HostCliAction.BlackFlotillaSelfTest:
                    GetTree().Quit(HostCli.RunBlackFlotillaSelfTest(_dataDir));
                    return;
                case HostCliAction.RadioSelfTest:
                    GetTree().Quit(HostCli.RunRadioSelfTest());
                    return;
                case HostCliAction.UiSnapshotSelfTest:
                    GetTree().Quit(HostCli.RunUiSnapshotSelfTest());
                    return;
            }

            AtomicWar.GodotApp.Settings.UserSettingsStore.Apply(AtomicWar.GodotApp.Settings.UserSettingsStore.Current);
            BuildUserInterface();
            SetupJournal();
            SetupIceRoad();
            SetupDutyRoster();
            SetupExpansions();
            // Year of Ash used to initialise lazily on first button press, so its save
            // was not restored at boot and it was the only subsystem with no banner line.
            SetupYearOfAsh();
        }

        public override void _Process(double delta)
        {
            // The diagnostics strip used to rebuild its string every frame AND call
            // Engine.GetVersionInfo(), which allocates a Godot Dictionary — 60 allocations
            // a second for a version that never changes. Cache the version, refresh ~4x/sec.
            _diagnosticsAccum += delta;
            if (_diagnosticsAccum < DiagnosticsRefreshSeconds) return;
            double elapsed = _diagnosticsAccum;
            _diagnosticsAccum = 0.0;

            if (_diagnosticsLabel == null) return;
            double fps = Engine.GetFramesPerSecond();
            double memMb = (long)OS.GetStaticMemoryUsage() / (1024.0 * 1024.0);
            string verdictSave = _verdict != null
                ? $" | VerdictSave v{_verdict.LoadedSaveVersion}{( _verdict.WasSaveMigrated ? " (migrated)" : "")}"
                : string.Empty;
            _diagnosticsLabel.Text = $"FPS: {fps:F0} | Static Mem: {memMb:F1} MB | Godot {s_engineVersion}{verdictSave}";

            _diagnosticsLogAccum += elapsed;
            if (_diagnosticsLogAccum >= 1.0)
            {
                _diagnosticsLogAccum = 0.0;
                GD.Print($"[DevUI Diagnostics] FPS: {fps:F0} | Static Mem: {memMb:F1} MB | Godot {s_engineVersion}");
            }

            // Flush any journal writes that were coalesced since the last tick.
            FlushJournalIfDirty();
            // Flush the Holdfast S1 save the same way — one write per burst, not per event.
            FlushHoldfastIfDirty();
            FlushDutyRosterIfDirty();
            FlushExpansionHubIfDirty();
            FlushVerdictIfDirty();
            FlushMaritimeIfDirty();
            FlushExpeditionIfDirty();
            FlushNarrativeIfDirty();
            FlushMedicalIfDirty();
            FlushWorldIfDirty();
            FlushCraftingIfDirty();
            FlushCaravanIfDirty();
            FlushYearOfAshIfDirty();
            FlushPhase0IfDirty();

            // ── Sleep / End Day countdown timer (Phase 2 continuation)
            if (_advanceTimerRemaining > 0 && !_advanceCancelled)
            {
                _advanceTimerRemaining -= delta;
                if (_advanceTimerRemaining <= 0)
                {
                    _advanceTimerRemaining = 0;
                    _statusLabel.Text = "Sleep accepted — advancing day …";
                    CommitAdvance();
                }
                else if (_statusLabel != null)
                {
                    _statusLabel.Text = $"Sleep in progress … {_advanceTimerRemaining:F0}s remaining";
                }
            }
        }

        public override void _UnhandledKeyInput(InputEvent @event)
        {
            var key = @event as InputEventKey;
            if (key == null || !key.Pressed || key.Echo) return;

            if (key.Keycode == Key.J)
            {
                if (_state == GameState.Playing && _dashboard.Visible)
                    OpenPlayerPanel("journal");
                else
                    ToggleJournal();
                GetViewport().SetInputAsHandled();
            }
            else if (key.Keycode == Key.F1 && _state == GameState.Playing)
            {
                ToggleDeveloperConsole();
                GetViewport().SetInputAsHandled();
            }
            else if (_journalBook != null && _journalBook.IsOpen)
            {
                if (key.Keycode >= Key.Key1 && key.Keycode <= Key.Key5)
                {
                    _journal.SwitchTab((int)(key.Keycode - Key.Key1));
                    GetViewport().SetInputAsHandled();
                }
                else if (key.Keycode == Key.Escape)
                {
                    // Cancel a pending sleep advance before closing the journal.
                    CancelAdvanceConfirmation();
                    _journalBook.Close();
                    GetViewport().SetInputAsHandled();
                }
            }
        }

        public override void _Notification(int what)
        {
            if (what == NotificationWMCloseRequest)
            {
                // Always cancel any in-progress sleep advance on teardown so stale
                // countdowns don't tick after the window closes.
                CancelAdvanceConfirmation();

                SaveJournal();
                SaveHoldfast();
                SaveHoldfastRuntime();
                SaveDutyRoster();
                SaveExpansionHub();
                SavePhantomMemory();
                SaveDoseLedger();
                SaveMuster();
                SaveInventory();
                SaveSurvivors();
                SaveEconomy();

                GetTree().Quit();
            }
        }

        private void ResolveDataDir()
        {
            _dataDir = CatalogPath.ResolveDataDir();
        }

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
                _startingLevel?.ServiceAirFilter();
                UpdateHud();
            };
            _dashboard.OnReplaceFilterRequested += () =>
            {
                SetupStartingLevel();
                _startingLevel?.ReplaceAirFilter();
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
            _craftingPanel = new CraftingPanel();
            _craftingPanel.OnClose += CloseCraftingPanel;
            _craftingPanel.OnCraftStarted += () => { UpdateHud(); _craftingDirty = true; };
            AddChild(_craftingPanel);

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

            // ── Economy panel (overlay) ──
            _economyOverlayPanel = new EconomyOverlayPanel();
            _economyOverlayPanel.OnClose += CloseEconomyPanel;
            AddChild(_economyOverlayPanel);

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
            _survivorDetailPanel = new SurvivorDetailPanel();
            _survivorDetailPanel.OnClose += CloseSurvivorDetailPanel;
            AddChild(_survivorDetailPanel);

            // ── Inventory Detail panel (overlay) ──
            _inventoryDetailPanel = new InventoryDetailPanel();
            _inventoryDetailPanel.OnClose += CloseInventoryDetailPanel;
            AddChild(_inventoryDetailPanel);

            // ── Quest Detail panel (overlay) ──
            _questDetailPanel = new QuestDetailPanel();
            _questDetailPanel.OnClose += CloseQuestDetailPanel;
            AddChild(_questDetailPanel);

            // ── Achievements panel (overlay) ──
            _achievementsPanel = new AchievementsPanel();
            _achievementsPanel.OnClose += CloseAchievementsPanel;
            AddChild(_achievementsPanel);

            // ── Weather Detail panel (overlay) ──
            _weatherDetailPanel = new WeatherDetailPanel();
            _weatherDetailPanel.OnClose += CloseWeatherDetailPanel;
            AddChild(_weatherDetailPanel);

            // ── Radiation Detail panel (overlay) ──
            _radiationDetailPanel = new RadiationDetailPanel();
            _radiationDetailPanel.OnClose += CloseRadiationDetailPanel;
            AddChild(_radiationDetailPanel);

            // ── Events Log panel (overlay) ──
            _eventsLogPanel = new EventsLogPanel();
            _eventsLogPanel.OnClose += CloseEventsLogPanel;
            AddChild(_eventsLogPanel);

            // ── Duty Roster Detail panel (overlay) ──
            _dutyRosterDetailPanel = new DutyRosterDetailPanel();
            _dutyRosterDetailPanel.OnClose += CloseDutyRosterDetailPanel;
            AddChild(_dutyRosterDetailPanel);

            // ── Economy Detail panel (overlay) ──
            _economyDetailPanel = new EconomyDetailPanel();
            _economyDetailPanel.OnClose += CloseEconomyDetailPanel;
            AddChild(_economyDetailPanel);

            // ── Combat Detail panel (overlay) ──
            _combatDetailPanel = new CombatDetailPanel();
            _combatDetailPanel.OnClose += CloseCombatDetailPanel;
            AddChild(_combatDetailPanel);

            // ── Faction Detail panel (overlay) ──
            _factionDetailPanel = new FactionDetailPanel();
            _factionDetailPanel.OnClose += CloseFactionDetailPanel;
            AddChild(_factionDetailPanel);

            // ── Crossing Quest panel (overlay) ──
            _crossingQuestPanel = new CrossingQuestPanel();
            _crossingQuestPanel.OnClose += CloseCrossingQuestPanel;
            AddChild(_crossingQuestPanel);

            // ── Save/Load panel (overlay) ──
            _saveLoadPanel = new SaveLoadPanel();
            _saveLoadPanel.OnClose += CloseSaveLoadPanel;
            AddChild(_saveLoadPanel);

            // ── Tutorial panel (overlay) ──
            _tutorialPanel = new TutorialPanel();
            _tutorialPanel.OnClose += CloseTutorialPanel;
            AddChild(_tutorialPanel);

            // ── Afflictions panel (overlay) ──
            _afflictionsPanel = new AfflictionsPanel();
            _afflictionsPanel.OnClose += CloseAfflictionsPanel;
            AddChild(_afflictionsPanel);

            // ── Status panel (overlay) ──
            _statusPanel = new StatusPanel();
            _statusPanel.OnClose += CloseStatusPanel;
            AddChild(_statusPanel);

            // ── Survival Detail panel (overlay) ──
            _survivalDetailPanel = new SurvivalDetailPanel();
            _survivalDetailPanel.OnClose += CloseSurvivalDetailPanel;
            AddChild(_survivalDetailPanel);

            // ── Weather Forecast panel (overlay) ──
            _weatherForecastPanel = new WeatherForecastPanel();
            _weatherForecastPanel.OnClose += CloseWeatherForecastPanel;
            AddChild(_weatherForecastPanel);

            // ── Radiation History panel (overlay) ──
            _radiationHistoryPanel = new RadiationHistoryPanel();
            _radiationHistoryPanel.OnClose += CloseRadiationHistoryPanel;
            AddChild(_radiationHistoryPanel);

            // ── Journal Detail panel (overlay) ──
            _journalDetailPanel = new JournalDetailPanel();
            _journalDetailPanel.OnClose += CloseJournalDetailPanel;
            AddChild(_journalDetailPanel);

            // ── Combat History panel (overlay) ──
            _combatHistoryPanel = new CombatHistoryPanel();
            _combatHistoryPanel.OnClose += CloseCombatHistoryPanel;
            AddChild(_combatHistoryPanel);

            // ── Map Detail panel (overlay) ──
            _mapDetailPanel = new MapDetailPanel();
            _mapDetailPanel.OnClose += CloseMapDetailPanel;
            AddChild(_mapDetailPanel);

            // ── Event Detail panel (overlay) ──
            _eventDetailPanel = new EventDetailPanel();
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
            };
            AddChild(_openingProtocolModal);

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
            bool hasSave = System.IO.File.Exists(HoldfastSaveStore.SavePath);
            _mainMenu.EnableContinue(hasSave);

            // ── Setup Expanded Shelter Systems (Water, Airlock, Relations, Treaties, etc.) ──
            SetupExpandedShelterSystems();

            // ── Start in menu state ──
            _state = GameState.Menu;
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







        private void UpdateStatus()
        {
            if (_statusLabel == null || _journal == null) return;
            _statusLabel.Text =
                $"Ready: {_dataDir}\n" +
                $"Journal: {_journal.EntryCount} pages · " +
                $"{(_journal.HasUnread ? "unread" : "nothing new")} · " +
                $"Day {_simDay} · [J] toggles the ledger.";
        }






        /// <summary>
        /// Real home-occupant snapshot for the Duty Roster morning tick: every
        /// alive survivor currently at home is a row candidate (sleptHere=true).
        /// The chart is a document other systems read — no rules are computed here.
        /// </summary>
        private List<Ashfall.Core.DutyRosterOccupant> BuildHomeOccupantSnapshot()
        {
            var occupants = new List<Ashfall.Core.DutyRosterOccupant>();
            if (_survivors == null) return occupants;
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var s = _survivors.RosterState[i];
                if (s == null || string.IsNullOrEmpty(s.Id) || !s.IsAliveState) continue;
                occupants.Add(new Ashfall.Core.DutyRosterOccupant
                {
                    survivorId = s.Id,
                    displayName = FormatSurvivorName(s.Id),
                    occupationObserved = string.Empty,
                    sleptHere = true
                });
            }
            occupants.Sort((a, b) => string.CompareOrdinal(a.survivorId, b.survivorId));
            return occupants;
        }














        private void OnWaystationTickClicked()
        {
            SetupExpansions();
            _expansions.UnlockWaystation();
            // The wintering filter burn depends on the real ice-road state, not a
            // host literal: an open window is the only way the bunks trade.
            bool roadOpen = _core != null && _core.IceRoad.IsOpen;
            _expansions.TickWaystation(roadOpen);
            _statusLabel.Text = "Waystation: " + _expansions.WaystationLine();
            RefreshExpansionsStatus();
        }

        private void OnWaystationWatchClicked()
        {
            SetupExpansions();
            _expansions.UnlockWaystation();
            _expansions.AssignWaystationWatch(new[] { "elena_vasquez", "marcus_olejnik", "suki_tanaka" });
            _expansions.SetWaystationWintering(true);
            _statusLabel.Text = "Watch assigned (Vasquez, Olejnik, Tanaka). Wintering mode on — stove lit, filter degrades faster.";
            RefreshExpansionsStatus();
        }








        // ── Nobody's Charter: Crossing Arbitration & Ledger ─────────────────























        // -----------------------------------------------------------------
        // Phantom Memory (Antigravity #41)
        // -----------------------------------------------------------------





        // -----------------------------------------------------------------
        // Phase-0 effects (phantom work-efficiency/refusal, flashbacks,
        // trade specialty, final-wish shelter buff, respiratory stamina)
        // -----------------------------------------------------------------








        // ── THE DOSE (Exp 07) host wiring ───────────────────────────────










        // ── INVENTORY (ported from Unity _Game/Inventory) host wiring ───








        // ── SURVIVORS (needs + radiation) host wiring ──────────────────


        // ── UTILITY AI (NPC decisions) host wiring ───────────────────



        // ── ECONOMY (market core) host wiring ─────────────────────────















        // ── THE MUSTER (Exp 06) host wiring ─────────────────────────────








        public void OpenSettingsPanel() => _settingsPanel?.Open();
        public void OpenCraftingPanel()
        {
            SetupCrafting();
            SetupInventory();
            _craftingPanel.Bind(_crafting, _inventory);
            _craftingPanel.Open();
        }
        public void OpenRadioPanel() => _radioPanel?.Open();
        public void OpenMedicalPanel() => _medicalPanel?.Open();

        public void OpenPhase0Panel()
        {
            SetupSurvivors();
            SetupPhase0();
            _phase0Panel.Bind(_phase0, _survivors);
            _phase0Panel.Open();
        }
        public void OpenDutyRosterPanel() => _dutyRosterPanel?.Open();
        public void OpenExpeditionPanel() => _expeditionPanel?.Open();
        public void OpenWeatherPanel() => _weatherPanel?.Open();
        public void OpenQuestsPanel()
        {
            SetupHoldfastRuntime();
            SetupExpansions();
            SetupDutyRoster();
            _questsPanel.Bind(_core.Quests, _expansions?.CrossingQuests, _dutyRoster, _holdfastRuntime?.Day ?? _simDay);
            _questsPanel.Open();
        }
        public void OpenJournalPanel() => _journalPanel?.Open();
        public void OpenFactionsPanel()
        {
            SetupHoldfastRuntime();
            SetupMuster();
            SetupExpansions();
            SetupYearOfAsh();
            _factionsPanel.Bind(_core.Catalog.Factions, _holdfastRuntime?.Trade, _muster, _expansions, _yearOfAsh);
            _factionsPanel.OnWarlordTributePay -= PayWarlordTribute;
            _factionsPanel.OnWarlordTributePay += PayWarlordTribute;
            _factionsPanel.OnWarlordTributeRefuse -= RefuseWarlordTribute;
            _factionsPanel.OnWarlordTributeRefuse += RefuseWarlordTribute;
            _factionsPanel.Open();
        }


        public void OpenShelterPanel() => _shelterPanel?.Open();
        public void OpenCombatPanel()
        {
            SetupCombat();
            _combatPanel.Bind(_combat);
            _combatPanel.Open();
        }
        public void OpenMapPanel()
        {
            SetupHoldfastRuntime();
            SetupExpeditions();
            SetupExpansions();
            SetupWorld();
            SetupJournal();
            SetupDeepCoast();
            SetupYearOfAsh();
            _mapPanel.Bind(_core, _expeditions, _expansions, _world, _journalCodex?.Catalogs, _deepCoast, _yearOfAsh);
            _mapPanel.Open();
        }
        public void OpenMapDetailPanel(string locationId)
        {
            SetupHoldfastRuntime();
            SetupExpeditions();
            SetupJournal();
            var holdfastLoc = _core?.Catalog?.GetLocation(locationId);
            LocationDefinitionData? journalLoc = null;
            if (_journalCodex?.Catalogs?.Locations != null)
            {
                foreach (var l in _journalCodex.Catalogs.Locations)
                {
                    if (l != null && l.id == locationId)
                    {
                        journalLoc = l;
                        break;
                    }
                }
            }
            _mapDetailPanel.Bind(holdfastLoc, journalLoc);
            _mapDetailPanel.Open();
        }
        public void OpenFactionDetailPanel(string factionId)
        {
            SetupHoldfastRuntime();
            SetupMuster();
            SetupExpansions();
            var faction = _core?.Catalog?.Factions?.GetById(factionId);
            if (faction != null)
            {
                _factionDetailPanel.Bind(faction, _holdfastRuntime?.Trade, _muster, _expansions);
            }
            _factionDetailPanel.Open();
        }
        public void OpenQuestDetailPanel(string questId)
        {
            SetupHoldfastRuntime();
            SetupExpansions();
            var holdfastDef = _core?.Quests?.GetDef(questId);
            var holdfastProgress = _core?.Quests?.GetProgress(questId);
            if (holdfastDef != null)
            {
                _questDetailPanel.Bind(holdfastDef, holdfastProgress);
            }
            else if (_expansions?.CrossingQuests != null)
            {
                var crossingDef = _expansions.CrossingQuests.GetDef(questId);
                var crossingProgress = _expansions.CrossingQuests.GetProgress(questId);
                if (crossingDef != null)
                    _questDetailPanel.Bind(crossingDef, crossingProgress);
            }
            _questDetailPanel.Open();
        }
        public void OpenSaveLoadPanel() => _saveLoadPanel?.Open();
        public void OpenCrossingQuestPanel()
        {
            SetupExpansions();
            _crossingQuestPanel.Bind(_expansions, _expansions.Vouch, _simDay);
            _crossingQuestPanel.Open();
        }
        public void OnExitGameClicked() { SaveAll(); GetTree().Quit(); }









        private string _selectedApproachQuestlineId = "quest_the_rate_card_war";







        // ── ASHFALL: THE VERDICT (Expansion 08) ────────────────────────────────



        // Chain 1 tracking: previous-tick living-count snapshot held in host
        // state. Day boundary resets so we do not attribute today's losses
        // to last week. Threshold is observed but the doctrine check lives
        // in ReckoningSystem.
        private int _previousLivingCount = -1;
        private int _previousLivingDay = -1;





        // ── District 8 deep-coast route (Exp 01 sibling layer) ─────────




        // ── ASHFALL: THE BLACK FLOTILLA (Expansion 09 — maritime salvage) ──────







        // ── EXPEDITIONS (Encounters port) ─────────────────────────────────────



        // ── COMBAT (Expansion 06) ───────────────────────────────────────────









        // ── NARRATIVE · MEDICAL · WORLD · CRAFTING ────────────────────────────



















        // ── TRAVELING CARAVANS (Exp V spec §3.3) ─────────────────────────────





        // ── STARTING LEVEL & HOLDFAST DIRECTIVES ───────────────────────



        // ── POWER GRID (item 13) ────────────────────────────────────────────





        // ── MEDICAL WARD (item 11) ─────────────────────────────────────




        // ── MEMORIAL (item 15) ──────────────────────────────────────────




        // ── STATE-LOSS TRIAD REPAIR (audit fix) ─────────────────────────────
        // The four SaveXxx methods below close the 12 Setup-without-Save gaps
        // called out in the forensic audit. They each persist a single Core
        // envelope to user:// via a dedicated save store. The matching load
        // step runs at the corresponding SetupXxx entry-point (see the audit
        // reference at the top of this file for the full mapping).





        // ── TRAVEL MAP (item 4) ─────────────────────────────────────────


        // ── ENCOUNTER CHOICE (item 5) ──────────────────────────────────




        // ── PHASE 0 / CAMPAIGN DAY COORDINATOR ───────────────────────────

        private const string DailyBriefingSaveKey = "daily_briefing_v1";








        // ── GREENHOUSE / THE GLASS ORCHARD (Exp 05 / XI) ───────────────




        // ── THE SILENT FOUNDRY (Exp 10) ─────────────────────────────────




































        /// <summary>
        /// UI smoke tests create and queue-free a large widget tree. Give Godot one
        /// process frame to flush queued frees before shutting down, otherwise the
        /// test can pass while reporting false-positive node/RID/resource leaks.
        /// </summary>
        private async void QuitUiTestAfterFrame(int exitCode)
        {
            var tree = GetTree();
            await ToSignal(tree, SceneTree.SignalName.ProcessFrame);

            // The UI smoke tests construct the shell directly under Main rather
            // than loading a disposable child scene. Free those test-owned roots
            // explicitly so Godot does not leave their controls in ObjectDB at
            // process exit (normal gameplay never calls this path).
            foreach (Node child in GetChildren())
                child.QueueFree();

            await ToSignal(tree, SceneTree.SignalName.ProcessFrame);
            await ToSignal(tree, SceneTree.SignalName.ProcessFrame);
            tree.Quit(exitCode);
        }

        /// <summary>Headless smoke test for the player-facing Godot shell.</summary>
        private void RunDashboardUiTestAndQuit()
        {
            BuildUserInterface();
            SetupHoldfastRuntime();
            UpdateHud();
            _dashboard.Visible = true;

            bool shellBuilt = _dashboard.GetChildCount() > 0 && _dashboard.Visible;
            bool overlayParentedToRoot = _inventoryOverlay.GetParent() == this;
            OpenPlayerPanel("inventory");
            bool inventoryOpened = _inventoryOverlay.Visible;
            CloseAllOverlayPanels();

            bool liveSources = _world != null && _inventory != null && _survivors != null;
            bool pass = shellBuilt && overlayParentedToRoot && inventoryOpened && liveSources;
            GD.Print($"[DashboardUiTest] shell={shellBuilt} rootOverlay={overlayParentedToRoot} inventory={inventoryOpened} liveSources={liveSources}");
            GD.Print(pass ? "DASHBOARD_UITEST PASS" : "DASHBOARD_UITEST FAIL");
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }


        // -----------------------------------------------------------------
        // Menu callbacks
        // -----------------------------------------------------------------

        private void LoadGameCatalogs()
        {
            int jsonCount = 0;
            var summary = new System.Text.StringBuilder();
            summary.AppendLine("=== ASHFALL SURVIVAL ARCHIVE LOADED ===");
            summary.AppendLine($"Archive Location: {_dataDir}");
            // Host diagnostics only — NOT simulation time. Ashfall.Core.IClock owns the
            // sim calendar and bans DateTime.Now; use UTC + invariant culture here so the
            // banner is timezone- and locale-stable and can never be mistaken for sim day.
            summary.AppendLine(
                "Timestamp: " +
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss'Z'", CultureInfo.InvariantCulture) +
                "\n");

            if (Directory.Exists(_dataDir))
            {
                var files = Directory.GetFiles(_dataDir, "*.json");
                jsonCount = files.Length;
                summary.AppendLine($"Discovered {files.Length} Data Catalogs:\n");

                foreach (var f in files)
                {
                    string fileName = Path.GetFileName(f);
                    long sz = new FileInfo(f).Length;
                    summary.AppendLine($" [✓] {fileName,-35} ({sz / 1024.0:F1} KB)");
                }
            }
            else
            {
                summary.AppendLine("[!] Note: StreamingAssets/Data folder not found at relative path.");
            }

            if (_statusLabel != null)
                _statusLabel.Text = $"Ready: {jsonCount} JSON Game Catalogs connected.";
            if (_codexViewer != null)
                _codexViewer.Text = summary.ToString();
        }

        // -----------------------------------------------------------------
        // Game flow: Menu → Playing → GameOver
        // -----------------------------------------------------------------

        private void StartNewGame()
        {
            _state = GameState.Playing;
            _mainMenu.Visible = false;
            _gameOver.Visible = false;
            _gameUiContainer.Visible = false;
            _dashboard.Visible = true;
            CloseAllOverlayPanels();

            _audio?.StopMusic();
            _audio?.PlayGameplayMusic();
            _audio?.StartBunkerAmbience();

            // A new game must not inherit the previous run's in-memory sessions or
            // on-disk saves. Null every session so the next SetupXxx re-creates clean,
            // and delete the store files so Continue stays disabled for a fresh run.
            ResetAllSessions();

            // Initialize Holdfast & Starting Level
            SetupHoldfastRuntime();
            _holdfastTerminal.PressNewLedger();
            _holdfastTerminal.OpenTerminal();

            SetupStartingLevel();
            _openingProtocolModal.Bind(_startingLevel);
            _openingProtocolModal.Open();

            // Update HUD
            UpdateHud();

            _statusLabel.Text = "New game started. Day 1. The ash is settling.";
        }




        private void ReturnToMenu()
        {
            // Cancel any in-progress sleep advance so stale timers don't tick
            // after returning to the menu.
            CancelAdvanceConfirmation();

            _state = GameState.Menu;
            _gameUiContainer.Visible = false;
            _dashboard.Visible = false;
            _gameOver.Visible = false;
            _mainMenu.Visible = true;

            _audio?.StopAmbience();
            _audio?.StopMusic();

            CloseAllOverlayPanels();

            // Save before returning
            SaveAll();

            // Check for existing save
            bool hasSave = System.IO.File.Exists(HoldfastSaveStore.SavePath);
            _mainMenu.EnableContinue(hasSave);
        }

        private void ToggleDeveloperConsole()
        {
            bool showConsole = !_gameUiContainer.Visible;
            _gameUiContainer.Visible = showConsole;
            _dashboard.Visible = !showConsole;
            if (showConsole)
            {
                CloseAllOverlayPanels();
                _statusLabel.Text = "Developer console active. Use the player shell when you are ready to resume.";
            }
            else
            {
                _dashboard.SetDeveloperMode(false);
                UpdateHud();
            }
        }

        private void OpenPlayerPanel(string panelId)
        {
            CloseAllOverlayPanels();

            switch (panelId)
            {
                case "survivors":
                    SetupSurvivors();
                    _survivorsOverlay.Bind(_survivors);
                    _survivorsOverlay.Open();
                    break;
                case "inventory":
                    SetupInventory();
                    _inventoryOverlay.Bind(_inventory);
                    _inventoryOverlay.RefreshView();
                    _inventoryOverlay.Open();
                    break;
                case "crafting":
                    SetupCrafting();
                    SetupInventory();
                    _craftingPanel.Bind(_crafting, _inventory);
                    _craftingPanel.Open();
                    break;
                case "medical":
                    SetupSurvivors();
                    SetupInventory();
                    SetupMedical();
                    SetupPhase0();
                    _medicalPanel.Bind(_medical, _survivors, _inventory,
                        _phase0?.Respiratory);
                    _medicalPanel.Open();
                    break;
                case "phase0":
                    OpenPhase0Panel();
                    break;
                case "expeditions":
                    SetupExpeditions();
                    SetupExpansions();
                    _expeditions.CrossingGate = _expansions.Vouch;
                    SetupSurvivors();
                    SetupInventory();
                    _expeditionPanel.Bind(_expeditions, _survivors, _inventory);
                    _expeditionPanel.Open();
                    break;
                case "weather":
                    SetupWorld();
                    _weatherPanel.Bind(_world);
                    _weatherPanel.Open();
                    break;
                case "radio":
                    SetupRadio();
                    _radioPanel.Bind(_radio);
                    _radioPanel.Open();
                    break;
                case "map":
                    SetupHoldfastRuntime();
                    SetupExpeditions();
                    SetupExpansions();
                    SetupWorld();
                    SetupJournal();
                    SetupDeepCoast();
                    SetupYearOfAsh();
                    _mapPanel.Bind(_core, _expeditions, _expansions, _world, _journalCodex?.Catalogs, _deepCoast, _yearOfAsh);
                    _mapPanel.Open();
                    break;
                case "shelter":
                    SetupSurvivors();
                    SetupWorld();
                    SetupInventory();
                    _shelterPanel.Bind(_survivors, _world, _inventory);
                    _shelterPanel.Open();
                    break;
                case "factions":
                    SetupHoldfastRuntime();
                    SetupMuster();
                    SetupExpansions();
                    _factionsPanel.Bind(_core.Catalog.Factions, _holdfastRuntime?.Trade, _muster, _expansions);
                    _factionsPanel.Open();
                    break;
                case "quests":
                    SetupHoldfastRuntime();
                    SetupExpansions();
                    SetupDutyRoster();
                    _questsPanel.Bind(_core.Quests, _expansions?.CrossingQuests, _dutyRoster, _holdfastRuntime?.Day ?? _simDay);
                    _questsPanel.Open();
                    break;
                case "journal":
                    SetupJournal();
                    _journalBook.Open();
                    break;
                case "protocol":
                    SetupStartingLevel();
                    _openingProtocolModal.Bind(_startingLevel);
                    _openingProtocolModal.Open();
                    break;
                case "greenhouse":
                    SetupGreenhouse();
                    _greenhousePanel.Bind(_greenhouse);
                    _greenhousePanel.Open();
                    break;
                case "silent_foundry":
                    SetupExpansions();
                    SetupSilentFoundry();
                    _silentFoundryPanel.Bind(_silentFoundry, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
                    _silentFoundryPanel.Open();
                    break;
                case "trade":
                    SetupEconomy();
                    SetupSilentFoundry();
                    OpenTradeScreen();
                    break;
                case "muster":
                    SetupMuster();
                    _musterPanel.Bind(_muster, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
                    _musterPanel.Open();
                    break;
                case "expansions":
                    SetupExpansions();
                    SetupGreenhouse();
                    SetupDutyRoster();
                    SetupMuster();
                    SetupMaritime();
                    SetupDeepCoast();
                    SetupWorld();
                    SetupMedical();
                    SetupVerdict();
                    _expansionsHubPanel.Bind(_expansions, _greenhouse, _dutyRoster, _muster, _maritime, _deepCoast, _world, _medical, _verdict, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
                    _expansionsHubPanel.Open();
                    break;
                case "standing_record":
                    SetupExpansions();
                    _standingRecordPanel.Bind(_expansions?.Layouts);
                    _standingRecordPanel.Open();
                    break;
                case "crossing_quests":
                    SetupExpansions();
                    _crossingQuestPanel.Bind(_expansions, _expansions?.Vouch, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay);
                    _crossingQuestPanel.Open();
                    break;
                case "maritime":
                    SetupMaritime();
                    SetupSurvivors();
                    _maritimePanel.Bind(_maritime, _survivors);
                    _maritimePanel.Open();
                    break;
                case "deep_coast":
                    SetupDeepCoast();
                    _deepCoastPanel.Bind(_deepCoast, _core);
                    _deepCoastPanel.SetSimDay(_simDay);
                    _deepCoastPanel.Open();
                    break;
                case "century_seed":
                    SetupExpansions();
                    SetupSurvivors();
                    _centurySeedPanel.Bind(_expansions?.Generational, _survivors);
                    _centurySeedPanel.Open();
                    break;
                case "epilogue":
                    SetupExpansions();
                    SetupSurvivors();
                    _epiloguePanel.Bind(_simDay, _survivors?.RosterState?.Count ?? 4, 0, true, true, true, true, true);
                    _epiloguePanel.Open();
                    break;
                case "verdict":
                    SetupVerdict();
                    _verdictPanel.Bind(_verdict);
                    _verdictPanel.Open();
                    break;
                case "holdfast":
                    SetupHoldfastRuntime();
                    if (_holdfastTerminal != null)
                    {
                        _holdfastTerminal.BindSession(_holdfastRuntime);
                        _holdfastTerminal.OpenTerminal();
                    }
                    break;
                case "duty_roster":
                    SetupDutyRoster();
                    SetupSurvivors();
                    _dutyRosterPanel.Bind(_dutyRoster, _survivors);
                    _dutyRosterPanel.Open();
                    break;
                case "duty_roster_detail":
                    SetupDutyRoster();
                    _dutyRosterDetailPanel.Bind(_dutyRoster);
                    _dutyRosterDetailPanel.Open();
                    break;
                case "save":
                    SaveAll();
                    _saveLoadPanel.Open();
                    break;
                case "water_treatment":
                case "airlock_security":
                case "survivor_relations":
                case "regional_treaty":
                case "vinyl_morale":
                case "wildlife_trapping":
                case "excavation":
                case "apprenticeship":
                case "shelter_thermal":
                case "shelter_schedule":
                case "autopsy_report":
                case "waystation_network":
                    OpenExpandedPanel(panelId);
                    break;
            }
        }

        private void ShowGameOver(string cause, string stats)
        {
            _state = GameState.GameOver;
            _gameUiContainer.Visible = false;
            _dashboard.Visible = false;
            _mainMenu.Visible = false;
            _gameOver.ShowGameOver(cause, stats);

            _audio?.StopAmbience();
            _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.GameOver);

            // Save final state
            SaveAll();

            // A finished run must not be continuable: the saved state is a dead
            // (or won) ledger. Clear the holdfast saves so ReturnToMenu keeps the
            // Continue button disabled instead of resurrecting an ended run.
            ClearContinuableSaves();
        }


        private void UpdateHud()
        {
            if (_holdfastRuntime == null) return;
            SetupWorld();
            SetupInventory();
            SetupSurvivors();

            long value = _holdfastRuntime.Trade.PlayerValue;
            string faction = _holdfastTerminal?.SelectedFactionId ?? "";
            string weather = _world.Weather.Current.ToString();
            _hudOverlay.UpdateState(_holdfastRuntime.Day, value, faction, weather);
            _hudOverlay.UpdateHealth(_holdfastRuntime.Health, HoldfastRuntimeSession.MaxHealth);
            _hudOverlay.UpdateRadiation(_holdfastRuntime.Radiation);

            int totalSurvivors = 0;
            int livingSurvivors = 0;
            float livingHealth = 0f;
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var survivor = _survivors.RosterState[i];
                if (survivor == null) continue;
                totalSurvivors++;
                if (!survivor.IsAliveState) continue;
                livingSurvivors++;
                livingHealth += survivor.Health;
            }

            var stores = _inventory.Inventory;
            int filterSpares = stores.CountById("air_filter")
                + stores.CountById("filter_item")
                + stores.CountById("water_filter")
                + stores.CountById("respirator_filter")
                + stores.CountById("respirator_filter_box_5");
            string lastEvent = !string.IsNullOrWhiteSpace(_holdfastRuntime.World.LastEvent)
                ? _holdfastRuntime.World.LastEvent
                : !string.IsNullOrWhiteSpace(_world.LastEvent)
                    ? _world.LastEvent
                    : _survivors.LastEvent;

            SetupStartingLevel();
            string intakeAssignee = _dutyRoster?.Roster.GetAssignment(Ashfall.Core.DutyRosterSystem.RoleIntakeSleeper) ?? "Dr. Sarah Chen";

            _dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
            {
                Day = _holdfastRuntime.Day,
                Health = _holdfastRuntime.Health,
                MaxHealth = HoldfastRuntimeSession.MaxHealth,
                Radiation = _holdfastRuntime.Radiation,
                Hunger = _holdfastRuntime.Hunger,
                Thirst = _holdfastRuntime.Thirst,
                Value = value,
                Weather = weather,
                WeatherVisibility = _world.Weather.VisibilityFactor,
                OutdoorRadiation = _world.Weather.OutdoorRadModifier,
                LivingSurvivors = livingSurvivors,
                TotalSurvivors = totalSurvivors,
                AverageSurvivorHealth = livingSurvivors > 0 ? livingHealth / livingSurvivors : 0f,
                CleanWater = stores.CountById("clean_water"),
                Food = stores.CountById("canned_food"),
                MedicalStock = stores.CountByType(ItemType.Medical),
                FilterSpares = _startingLevel?.System.State.filterSparesCount ?? filterSpares,
                MechanicalScrap = _startingLevel?.System.State.mechanicalScrapCount ?? 6,
                AirFilterHealth = _startingLevel?.System.State.airFilterHealthPercent ?? 100.0f,
                AirQuality = _startingLevel?.System.State.airQualityPercent ?? 100.0f,
                RadonLevel = _startingLevel?.System.State.radonLevelBqm3 ?? 12.0f,
                AirWarning = _startingLevel?.System.State.airHazardWarning ?? false,
                FilterDutyAssignee = intakeAssignee,
                Forecast = _world.Weather.PeekForecast(3),
                LastEvent = lastEvent
            });
        }

        private void OnPlayerDied(string cause)
        {
            string stats = $"Survived {_holdfastRuntime.Day} days. " +
                           $"Final value: {_holdfastRuntime.Trade.PlayerValue}. " +
                           $"Radiation: {_holdfastRuntime.Radiation:F0} mSv.";
            ShowGameOver(cause, stats);
        }

        private void OnGameWon(string message)
        {
            string stats = $"The Holdfast endures. Day {_holdfastRuntime.Day}. " +
                           $"Final value: {_holdfastRuntime.Trade.PlayerValue}. " +
                           $"All {HoldfastQuestSystem.MainQuestIds.Length} quests complete.";
            ShowGameOver(message, stats);
        }



        private bool AnyOverlayPanelOpen()
        {
            if (_journalBook != null && _journalBook.IsOpen) return true;
            Control[] panels =
            {
                _settingsPanel, _inventoryOverlay, _survivorsOverlay, _craftingPanel,
                _radioPanel, _medicalPanel, _dutyRosterPanel, _economyOverlayPanel,
                _expeditionPanel, _weatherPanel, _questsPanel, _journalPanel,
                _factionsPanel, _researchPanel, _shelterPanel, _greenhousePanel, _combatPanel, _mapPanel,
                _silentFoundryPanel,
                _tradePanel,
                _survivorDetailPanel, _inventoryDetailPanel, _questDetailPanel,
                _achievementsPanel, _weatherDetailPanel, _radiationDetailPanel,
                _eventsLogPanel, _dutyRosterDetailPanel, _economyDetailPanel,
                _combatDetailPanel, _crossingQuestPanel, _saveLoadPanel, _tutorialPanel, _afflictionsPanel,
                _statusPanel, _survivalDetailPanel, _weatherForecastPanel,
                _radiationHistoryPanel, _journalDetailPanel, _combatHistoryPanel,
                _mapDetailPanel, _eventDetailPanel, _openingProtocolModal,
                _dailyBriefingModal
            };

            foreach (Control panel in panels)
            {
                if (panel != null && panel.Visible)
                    return true;
            }
            if (_briefingPending && _dailyBriefingModal != null && _dailyBriefingModal.IsOpen)
                return true;
            return false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                if (AnyOverlayPanelOpen())
                {
                    CloseAllOverlayPanels();
                    GetViewport().SetInputAsHandled();
                    return;
                }
                if (_state == GameState.Playing)
                {
                    ReturnToMenu();
                    GetViewport().SetInputAsHandled();
                }
            }
        }

        private void OnStartGameClicked()
        {
            SetupIceRoad();
            _core.UnlockAndClerk();
            _simDay = _core.Clock.Day;
            _statusLabel.Text = $"Holdfast unlocked. Clerk at the hatch. Day {_core.Clock.Day}. Tick the ice road.";
            _codexViewer.Text =
                "=== ICE ROAD (Ashfall.Core) ===\n" +
                $"Catalog: {_dataDir}\n" +
                $"{_core.CatalogLine()}\n" +
                "Sheet → clerk → freeze window. Not a loading screen.\n\n" +
                HoldfastBriefingView.FormatQuest(_core.CurrentQuest, _core.Catalog);
            RefreshIceRoadLabel();
        }


















        // -----------------------------------------------------------------
        // Year of Ash (Days 180-360) Wiring
        // -----------------------------------------------------------------














}
}
