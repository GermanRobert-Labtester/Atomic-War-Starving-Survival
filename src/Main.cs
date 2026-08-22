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
    }
}
