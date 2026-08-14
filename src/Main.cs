using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;

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
        private DoseLedgerHostSession _doseLedger = null!;
        private bool _doseLedgerDirty;
        private DoseRegisterSurface _doseSurface = null!;

        // Muster (Expansion 06, Days 180-360 escalation)
        private MusterHostSession _muster = null!;
        private CurrentsRosterWidget _currentsRoster = null!;
        private ApproachSelectionModal _approachModal = null!;
        private DeserterCoalitionCampWidget _campWidget = null!;
        private JournalWitnessPanel _witnessPanel = null!;

        // Inventory (ported from Unity _Game/Inventory)
        private InventoryHostSession _inventory = null!;
        private AtomicWar.GodotApp.Inventory.InventoryPanel _inventoryPanel = null!;

        // Survivors (needs + radiation, ported from Unity Survivors/Radiation)
        private SurvivorsHostSession _survivors = null!;

        // Journal (docs/ui/JOURNAL_UI_PLAN.md)
        private JournalSystem _journal = null!;
        private JournalCodex _journalCodex = null!;
        private JournalBookUI _journalBook = null!;
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
        // ASHFALL: THE DUTY ROSTER (Exp 02) — chart, marks, encounters
        private DutyRosterHostSession _dutyRoster = null!;
        private ExpansionHostSession _expansions = null!;
        // Holdfast S1 save coalescing (same pattern as the journal): any state
        // change in IceRoad or Census marks the save dirty; the diagnostics tick
        // flushes it. Quit and the explicit menu button flush immediately.
        private bool _holdfastDirty;
        // Duty Roster (Exp 02) and Expansion Hub save coalescing — same pattern.
        private bool _dutyRosterDirty;
        private bool _expansionHubDirty;

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
                case HostCliAction.BrineSelfTest:
                    GetTree().Quit(HostCli.RunBrineSelfTest());
                    return;
                case HostCliAction.MusterSelfTest:
                    GetTree().Quit(HostCli.RunMusterSelfTest());
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
                case HostCliAction.MusterUiTest:
                    RunMusterUiTestAndQuit();
                    return;
                case HostCliAction.DoseUiTest:
                    RunDoseUiTestAndQuit();
                    return;
                case HostCliAction.InventoryUiTest:
                    RunInventoryUiTestAndQuit();
                    return;
                case HostCliAction.SurvivorsUiTest:
                    RunSurvivorsUiTestAndQuit();
                    return;
                case HostCliAction.BridgeSelfTest:
                    GetTree().Quit(Ashfall.Bridge.BridgeSelfTest.Run());
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
                case HostCliAction.MedicalSelfTest:
                    GetTree().Quit(HostCli.RunMedicalSelfTest());
                    return;
                case HostCliAction.NarrativeSelfTest:
                    GetTree().Quit(HostCli.RunNarrativeSelfTest());
                    return;
                case HostCliAction.DataIntegritySelfTest:
                    GetTree().Quit(HostCli.RunDataIntegritySelfTest(_dataDir));
                    return;
                case HostCliAction.CaravanSelfTest:
                    GetTree().Quit(HostCli.RunCaravanSelfTest());
                    return;
            }

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
            // Drive the UnityEngine shim's lifecycle: magic-method dispatch, coroutines, and the
            // clock behind Time.deltaTime. Without this pump, any Unity behaviour that does get
            // instantiated would register and then never receive Awake/Start/Update.
            Ashfall.Bridge.BridgeRuntime.Tick((float)delta);

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
            _diagnosticsLabel.Text = $"FPS: {fps:F0} | Static Mem: {memMb:F1} MB | Godot {s_engineVersion}";

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
        }

        public override void _UnhandledKeyInput(InputEvent @event)
        {
            var key = @event as InputEventKey;
            if (key == null || !key.Pressed || key.Echo) return;

            if (key.Keycode == Key.J)
            {
                ToggleJournal();
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
                    _journalBook.Close();
                    GetViewport().SetInputAsHandled();
                }
            }
        }

        public override void _Notification(int what)
        {
            if (what == NotificationWMCloseRequest)
            {
                SaveJournal();
                SaveHoldfast();
                SaveDutyRoster();
                SaveExpansionHub();
                SavePhantomMemory();
                SaveDoseLedger();
                SaveMuster();
                SaveInventory();
                SaveSurvivors();
                // Give any live Unity behaviours their OnDisable/OnDestroy before the tree goes.
                Ashfall.Bridge.BridgeRuntime.Shutdown();
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
                Color = new Color(0.06f, 0.07f, 0.08f, 1.0f)
            };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", 60);
            margin.AddThemeConstantOverride("margin_top", 50);
            margin.AddThemeConstantOverride("margin_right", 60);
            margin.AddThemeConstantOverride("margin_bottom", 50);
            AddChild(margin);

            // MarginContainer gives EVERY child the same full rect, so it must hold
            // exactly one child. Adding the diagnostics bar as a second child made it
            // render on top of the whole UI instead of docking at the bottom. Root the
            // content in a VBox and let that own the split + the diagnostics strip.
            var rootColumn = new VBoxContainer();
            rootColumn.AddThemeConstantOverride("separation", 8);
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
            leftBox.AddThemeConstantOverride("separation", 18);
            hSplit.AddChild(leftBox);

            _titleLabel = new Label
            {
                Text = "ASHFALL\nATOMIC WAR: STARVING SURVIVAL",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            _titleLabel.AddThemeFontSizeOverride("font_size", 32);
            _titleLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.65f, 0.25f));
            leftBox.AddChild(_titleLabel);

            var subtitle = new Label
            {
                Text = "Post-Nuclear Survival Strategy & Narrative RPG\nPowered by Godot Engine (.NET Edition)",
                HorizontalAlignment = HorizontalAlignment.Left
            };
            subtitle.AddThemeFontSizeOverride("font_size", 14);
            subtitle.AddThemeColorOverride("font_color", new Color(0.65f, 0.75f, 0.7f));
            leftBox.AddChild(subtitle);

            leftBox.AddChild(new HSeparator());

            _menuContainer = new VBoxContainer();
            _menuContainer.AddThemeConstantOverride("separation", 12);
            leftBox.AddChild(_menuContainer);

            AddMenuButton("Start Survival Simulation", OnStartGameClicked);
            AddMenuButton("Tick ice-road day", OnTickIceRoadClicked);
            AddMenuButton("Cycle weather", OnCycleWeatherClicked);
            AddMenuButton("Show quest briefing", OnShowBriefingClicked);
            AddMenuButton("Census honour levy", OnCensusLevyClicked);
            AddMenuButton("Order 12-C (office acts)", OnOrder12CClicked);
            AddMenuButton("Unlock plant (salt trade)", OnUnlockPlantClicked);
            AddMenuButton("Repair membrane (resin)", OnRepairMembraneClicked);
            AddMenuButton("Toggle outfall shift", OnToggleOutfallClicked);
            AddMenuButton("Save holdfast state", OnSaveHoldfastClicked);
            AddMenuButton("Cycle ending (S4)", OnCycleEndingClicked);
            // ── ASHFALL: THE DUTY ROSTER (Exp 02) ──────────────────────
            AddMenuButton("Roster: inspect the Chart", OnRosterInspectWallClicked);
            AddMenuButton("Roster: morning row (pencil)", OnRosterPencilClicked);
            AddMenuButton("Roster: ink the wall (ending)", OnRosterInkClicked);
            AddMenuButton("Roster: burn the chart", OnRosterBurnClicked);
            AddMenuButton("Roster: tick a night (encounters)", OnRosterTickNightClicked);
            AddMenuButton("Roster: queue a visitor (hatch)", OnRosterVisitorClicked);
            AddMenuButton("Duty Roster: Second Winter", OnRosterSecondWinterClicked);
            // ── Waystation · Standing Record · Crossing · Greenhouse ──────
            AddMenuButton("Waystation: unlock + tick", OnWaystationTickClicked);
            AddMenuButton("Waystation: assign watch", OnWaystationWatchClicked);
            AddMenuButton("Standing Record: inspect", OnStandingRecordClicked);
            AddMenuButton("Standing Record: walk Km 19", OnRecordWalkKm19Clicked);
            AddMenuButton("Crossing: grant vouch (Osran)", OnCrossingVouchClicked);
            AddMenuButton("Crossing: burn vouch", OnCrossingBurnClicked);
            AddMenuButton("Arbitration: load backers", OnArbitrationLoadBackersClicked);
            AddMenuButton("Arbitration: call Standing", OnArbitrationCallStandingClicked);
            AddMenuButton("Arbitration: bribe a backer", OnArbitrationBribeClicked);
            AddMenuButton("Arbitration: overturn ruling", OnArbitrationOverturnClicked);
            AddMenuButton("Ledger: present + sign contract", OnLedgerSignClicked);
            AddMenuButton("Ledger: tick day", OnLedgerTickClicked);
            AddMenuButton("Ledger: pay contract", OnLedgerPayClicked);
            AddMenuButton("Greenhouse: plant + water", OnGreenhousePlantClicked);
            AddMenuButton("Greenhouse: tick + harvest", OnGreenhouseTickClicked);
            AddMenuButton("Hatch Encounter (Year of Ash)", OnDoorEncounterClicked);
            AddMenuButton("Tick Year of Ash (+10 Days)", OnTickYearOfAshClicked);
            AddMenuButton("Year of Ash: questlines", OnQuestlinesClicked);
            // ── Phantom Memory (Antigravity #41) ─────────────────────────
            AddMenuButton("Phantom Memory: scavenge item", OnPhantomScavengeClicked);
            AddMenuButton("Phantom Memory: tick hour", OnPhantomTickClicked);
            // ── THE DOSE (Exp 07) ───────────────────────────────────────
            AddMenuButton("Dose: seal dosimeters", OnDoseSealClicked);
            AddMenuButton("Dose: book a reading", OnDoseScribeClicked);
            AddMenuButton("Dose: name to Sick List", OnDoseDiagnoseClicked);
            AddMenuButton("Dose: book a Cohort child", OnDoseCohortClicked);
            AddMenuButton("Dose: sign a volunteer", OnDoseVolunteerClicked);
            AddMenuButton("Dose: open the Register", OnDoseRegisterClicked);
            // ── THE MUSTER (Exp 06) ────────────────────────────────────
            AddMenuButton("Muster: escalate to Day 260", OnMusterEscalateClicked);
            AddMenuButton("Muster: show currents (15)", OnMusterRosterClicked);
            AddMenuButton("Muster: Rate Card War approaches", OnMusterRateCardClicked);
            AddMenuButton("Muster: rally a deserter", OnMusterRallyClicked);
            AddMenuButton("Muster: strategy B (Standing Ground)", OnMusterStrategyBClicked);
            AddMenuButton("Muster: strategy D (Blood Price)", OnMusterStrategyDClicked);
            AddMenuButton("Muster: three witnesses (Harven)", OnMusterWitnessesClicked);
            AddMenuButton("Muster: witness author bias", OnMusterAuthorBiasClicked);
            AddMenuButton("Muster: epilogue matrix", OnMusterEpiloguesClicked);
            // ── INVENTORY (ported from Unity _Game/Inventory) ───────────
            AddMenuButton("Inventory: open the panel", OnInventoryOpenClicked);
            AddMenuButton("Inventory: add canned food ×6", () => OnInventoryAddClicked("canned_food", 6));
            AddMenuButton("Inventory: add clean water ×4", () => OnInventoryAddClicked("clean_water", 4));
            AddMenuButton("Inventory: add geiger counter", () => OnInventoryAddClicked("geiger_counter", 1));
            AddMenuButton("Inventory: add gas mask", () => OnInventoryAddClicked("gas_mask", 1));
            AddMenuButton("Inventory: item check", OnInventoryCheckClicked);
            // ── SURVIVORS (needs + radiation, from Unity) ──────────────
            AddMenuButton("Survivors: open panel", OnSurvivorsOpenClicked);
            AddMenuButton("Survivors: tick 6 hours", OnSurvivorsTickClicked);
            AddMenuButton("Survivors: expose Mikhail to 60 mSv/hr", () => OnSurvivorsExposeClicked("survivor_gunner_mikhail", 60f));
            AddMenuButton("Survivors: iodine for Mikhail", () => OnSurvivorsIodineClicked("survivor_gunner_mikhail"));
            AddMenuButton("Survivors: anti-rad for Mikhail", () => OnSurvivorsAntiRadClicked("survivor_gunner_mikhail", 30f));
            AddMenuButton("Open Bunker Ledger  [J]", OnViewCodexClicked);
            AddMenuButton("Inspect System Diagnostics", OnDiagnosticsClicked);
            AddMenuButton("Exit Game", () => { SaveJournal(); SaveHoldfast(); SaveDutyRoster(); SaveExpansionHub(); SavePhantomMemory(); SaveDoseLedger(); SaveMuster(); SaveInventory(); SaveSurvivors(); GetTree().Quit(); });

            _statusLabel = new Label
            {
                Text = "System Status: Initializing game state...",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _statusLabel.AddThemeFontSizeOverride("font_size", 13);
            _statusLabel.AddThemeColorOverride("font_color", new Color(0.5f, 0.85f, 0.5f));
            leftBox.AddChild(_statusLabel);

            _iceRoadLabel = new Label
            {
                Text = "Ice road: not wired",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _iceRoadLabel.AddThemeFontSizeOverride("font_size", 13);
            _iceRoadLabel.AddThemeColorOverride("font_color", new Color(0.75f, 0.85f, 0.95f));
            leftBox.AddChild(_iceRoadLabel);

            _catalogLabel = new Label
            {
                Text = "Holdfast catalog: —",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _catalogLabel.AddThemeFontSizeOverride("font_size", 13);
            _catalogLabel.AddThemeColorOverride("font_color", new Color(0.85f, 0.78f, 0.55f));
            leftBox.AddChild(_catalogLabel);

            _briefingPreviewLabel = new Label
            {
                Text = "Quest briefing: —",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _briefingPreviewLabel.AddThemeFontSizeOverride("font_size", 12);
            _briefingPreviewLabel.AddThemeColorOverride("font_color", new Color(0.7f, 0.72f, 0.68f));
            leftBox.AddChild(_briefingPreviewLabel);

            // Right Column: Terminal / Codex Viewer
            var rightBox = new VBoxContainer();
            rightBox.AddThemeConstantOverride("separation", 10);
            hSplit.AddChild(rightBox);
            _rightColumn = rightBox;

            var codexHeader = new Label
            {
                Text = "DATA TERMINAL & SURVIVAL LOGS"
            };
            codexHeader.AddThemeFontSizeOverride("font_size", 16);
            codexHeader.AddThemeColorOverride("font_color", new Color(0.4f, 0.8f, 0.9f));
            rightBox.AddChild(codexHeader);

            _codexViewer = new TextEdit
            {
                Editable = false,
                WrapMode = TextEdit.LineWrappingMode.Boundary,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _codexViewer.AddThemeColorOverride("background_color", new Color(0.03f, 0.04f, 0.05f, 0.9f));
            _codexViewer.AddThemeColorOverride("font_color", new Color(0.85f, 0.9f, 0.85f));
            rightBox.AddChild(_codexViewer);

            // Bottom Diagnostics bar
            _diagnosticsLabel = new Label
            {
                Text = "FPS: 60 | Static Mem: 0 MB"
            };
            _diagnosticsLabel.AddThemeFontSizeOverride("font_size", 12);
            _diagnosticsLabel.AddThemeColorOverride("font_color", new Color(0.45f, 0.55f, 0.5f));
            rootColumn.AddChild(_diagnosticsLabel);

            // Year of Ash Door Encounter Modal
            _questlineModal = new QuestlineModal();
            AddChild(_questlineModal);
            _questlineModal.OnQuestlineChosen += OnQuestlineChosen;
            _questlineModal.OnChoiceTaken += OnQuestlineChoiceTaken;

            _doorModal = new DoorEncounterModal();
            AddChild(_doorModal);
            _doorModal.OnChoiceClicked += OnDoorEncounterChoiceClicked;
        }

        private void AddMenuButton(string text, Action callback)
        {
            var btn = new Button
            {
                Text = text,
                CustomMinimumSize = new Vector2(0, 42)
            };
            btn.AddThemeFontSizeOverride("font_size", 15);
            btn.Pressed += callback;
            _menuContainer.AddChild(btn);
        }

        // -----------------------------------------------------------------
        // Journal wiring
        // -----------------------------------------------------------------

        private void SetupJournal()
        {
            var catalogs = CatalogJsonLoader.Load(_dataDir);
            _journal = new JournalSystem();
            // Mark dirty rather than writing the whole save file per entry; the
            // _Process tick flushes it. Seeding adds many entries in one frame and
            // used to rewrite journal_save.json once for each of them.
            _journal.OnEntryAdded += _ => _journalDirty = true;
            _journal.OnTabChanged += _ => _journalDirty = true;

            _journalCodex = new JournalCodex(_journal, catalogs);

            _journalBook = new JournalBookUI();
            _journalBook.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(_journalBook);
            _journalBook.Bind(
                _journal,
                tab => _journalCodex.BuildRows(tab),
                tab => _journal.HasUnreadForTab(tab),
                () => _simDay);
            _journalBook.OnClosed += SaveJournal;

            if (JournalSaveStore.Exists)
            {
                var save = JournalSaveStore.Load();
                if (save != null) _journal.RestoreState(save);
                _simDay = MaxEntryDay();
                _journalBook.SetEntries(_journal.Entries);
                _journalBook.ApplyUiState(
                    _journal.HudIsOpen,
                    _journal.HasUnread,
                    _journal.NotificationPing,
                    _journal.ActiveTab);
                GD.Print("[Ashfall Godot] Journal restored from save.");
            }
            else
            {
                int seededDay = JournalDemoHarness.Seed(_journal, catalogs);
                _simDay = Math.Max(4, seededDay);
                _journalBook.SetEntries(_journal.Entries);
                SaveJournal();
                GD.Print("[Ashfall Godot] Journal seeded with opening-day entries.");
            }

            UpdateStatus();
        }

        private int MaxEntryDay()
        {
            int day = 4;
            for (int i = 0; i < _journal.EntryCount; i++)
                if (_journal.Entries[i].Day > day) day = _journal.Entries[i].Day;
            return day;
        }

        private void ToggleJournal()
        {
            if (_journalBook != null) _journalBook.Toggle();
            UpdateStatus();
        }

        private void SaveJournal()
        {
            if (_journal == null) return;
            JournalSaveStore.Save(_journal.CaptureState());
            _journalDirty = false;
        }

        /// <summary>
        /// Writes the journal only when something actually changed. Called from the
        /// throttled _Process tick so a burst of entries costs one file write.
        /// </summary>
        private void FlushJournalIfDirty()
        {
            if (_journalDirty) SaveJournal();
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

        private void RunSelfTestAndQuit()
        {
            var catalogs = CatalogJsonLoader.Load(_dataDir);
            int code = JournalSelfTest.Run(catalogs);
            GetTree().Quit(code);
        }

        private void SetupIceRoad()
        {
            if (_core != null) return;
            _core = CoreDemoSession.Create(_dataDir);
            _core.IceRoad.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };
            _core.Census.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };
            _core.Brine.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };
            _core.Quests.OnStateChanged += _ => { _holdfastDirty = true; RefreshIceRoadLabel(); };

            // Cross-host roundtrip: a save written here (or by the Unity host) restores
            // the S1 gate instead of starting dark again. Codec validates the checksum.
            var save = HoldfastSaveStore.TryLoad();
            if (save != null)
            {
                _core.RestoreSave(save);
                _simDay = _core.Clock.Day;
                _holdfastDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Holdfast S1 state restored (day {_core.Clock.Day}).");
            }

            RefreshIceRoadLabel();
            GD.Print($"[Ashfall Godot] Ice road ready. {_core.CatalogLine()}");
        }

        private void SetupDutyRoster()
        {
            if (_dutyRoster != null) return;
            _dutyRoster = DutyRosterHostSession.Create(_dataDir);
            _dutyRoster.StateChanged += () => _dutyRosterDirty = true;

            // Cross-host roundtrip: a save written here (or by the Unity host) restores
            // the chart, marks, and encounter counters instead of starting blank.
            var save = DutyRosterSaveStore.TryLoad();
            if (save != null)
            {
                _dutyRoster.RestoreSave(save);
                _dutyRosterDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Duty Roster state restored (day {_dutyRoster.Clock.Day}).");
            }

            _dutyRoster.Unlock(_simDay);
            RefreshRosterStatus();
            GD.Print($"[Ashfall Godot] Duty Roster ready. {_dutyRoster.CatalogLine()}");
        }

        private void RefreshRosterStatus()
        {
            if (_dutyRoster == null || _statusLabel == null) return;
            _statusLabel.Text =
                $"——— DUTY ROSTER ———\n" +
                _dutyRoster.WallLine() + "\n" +
                _dutyRoster.EncountersLine() + "\n" +
                _dutyRoster.MarksLine() + "\n" +
                $"Day {_simDay} · catalog: {_dutyRoster.CatalogLine()}";
        }

        private void SetupExpansions()
        {
            if (_expansions != null) return;
            _expansions = ExpansionHostSession.Create(_dataDir);
            _expansions.StateChanged += () => _expansionHubDirty = true;

            // Cross-host roundtrip for waystation, standing record, crossing vouch,
            // and greenhouse plots.
            var save = ExpansionHubSaveStore.TryLoad();
            if (save != null)
            {
                _expansions.RestoreSave(save);
                _expansionHubDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Expansion hub state restored (day {save.simDay}).");
            }

            _expansions.EnsureGreenhousePlots(3);
            RefreshExpansionsStatus();
            GD.Print("[Ashfall Godot] Expansion hub ready: waystation · standing record · crossing · greenhouse");
        }

        private void RefreshExpansionsStatus()
        {
            if (_expansions == null || _statusLabel == null) return;
            _statusLabel.Text =
                $"——— EXPANSION HUB (Standing Record · Crossing · Greenhouse) ———\n" +
                _expansions.StandingRecordLine() + "\n" +
                _expansions.CrossingLine() + "\n" +
                _expansions.GreenhouseLine() + "\n" +
                _expansions.WaystationLine() + "\n" +
                _expansions.ArbitrationLine() + "\n" +
                _expansions.LedgerLine();
        }

        private void OnRosterInspectWallClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.InspectWall();
        }

        private void OnRosterPencilClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ResolveChart(DutyRosterSystem.ChoiceWritePencil)
                + "\n" + _dutyRoster.TickDay();
            RefreshRosterStatus();
        }

        private void OnRosterInkClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ResolveInk();
            RefreshRosterStatus();
        }

        private void OnRosterBurnClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.BurnChart();
            RefreshRosterStatus();
        }

        private void OnRosterTickNightClicked()
        {
            SetupDutyRoster();
            _simDay++;
            _dutyRoster.Clock.AdvanceDays(1);
            _statusLabel.Text = _dutyRoster.StartEncounter(ShelterEncounterSystem.KindNightSlate);
            RefreshRosterStatus();
        }

        private void OnRosterVisitorClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.QueueVisitor(ShelterEncounterSystem.VisitorLen);
            RefreshRosterStatus();
        }

        private void OnRosterSecondWinterClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ActivateSecondWinter();
            RefreshRosterStatus();
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

        private void OnStandingRecordClicked()
        {
            SetupExpansions();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== STANDING RECORD (Exp 03) ===");
            sb.AppendLine(_expansions.StandingRecordLine());
            sb.AppendLine(_expansions.RecordQuestLine());
            sb.AppendLine("Walk the route: Km 19 → Transit → Archive → Ministry → Weighbridge → Grange → Bridge → Lock → 12-B → Vault.");
            _codexViewer.Text = sb.ToString().TrimEnd();
            RefreshExpansionsStatus();
        }

        private void OnRecordWalkKm19Clicked()
        {
            SetupExpansions();
            var sb = new System.Text.StringBuilder();
            _expansions.UnlockRecord();
            _expansions.ArriveAtSite("loc_cut_kilometre_19");
            _expansions.EnterSiteRoom("room_km19_post");
            _expansions.InspectSiteRoom("room_km19_post");
            _expansions.EnterSiteRoom("room_km19_seam");
            sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_post"));
            sb.AppendLine();
            sb.AppendLine(_expansions.RoomLine("loc_cut_kilometre_19", "room_km19_seam"));
            _statusLabel.Text = sb.ToString().TrimEnd();
        }

        private void OnCrossingVouchClicked()
        {
            SetupExpansions();
            bool granted = _expansions.GrantVouch("npc_osran_kell");
            _statusLabel.Text = granted
                ? "Vouch granted by Osran Kell. The Crossing gate is open."
                : "Vouch refused (already granted, burned, or last resort spent).";
            RefreshExpansionsStatus();
        }

        private void OnCrossingBurnClicked()
        {
            SetupExpansions();
            bool burned = _expansions.BurnVouch();
            _statusLabel.Text = burned
                ? "Vouch burned. The gate is closed again — last resort remains available."
                : "Nothing to burn: no active vouch.";
            RefreshExpansionsStatus();
        }

        private void OnGreenhousePlantClicked()
        {
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.PlantGreenhouse(0, "item_seed_tuber", day);
            _expansions.WaterGreenhouse(0, 60f);
            _statusLabel.Text = "Plot 0 planted (seed_tuber) and watered on day " + day + ". The glass holds its heat.";
            RefreshExpansionsStatus();
        }

        private void OnGreenhouseTickClicked()
        {
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.TickGreenhouse(day);
            _statusLabel.Text = "Greenhouse day ticked (day " + day + "). " + _expansions.GreenhouseLine();
            RefreshExpansionsStatus();
        }

        // ── Nobody's Charter: Crossing Arbitration & Ledger ─────────────────

        private void OnArbitrationLoadBackersClicked()
        {
            SetupExpansions();
            _expansions.LoadDefaultBackerPool();
            _statusLabel.Text = "Backer pool loaded: Osran Kell (principled), Mattis Cray (principled), Halden Mire, Bram Ostrowski, Leva Quist, Dessa Penn.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationCallStandingClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
                _statusLabel.Text = "No backer pool — loaded defaults first.";
            }
            int day = _core != null ? _core.Clock.Day : _simDay;
            string topic = "quest_crossing_the_terms";
            bool called = _expansions.Arbitration.CallStanding(topic, day);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
            _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
            _statusLabel.Text = called
                ? $"Standing called on '{topic}' with 3 backers (Osran, Mattis, Halden). Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}"
                : "Standing already held — overturn first or call a different topic.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationBribeClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
                _statusLabel.Text = "No backer pool — loaded defaults first.";
            }
            // Set up a fresh ruling on a new topic
            string topic = CrossingIds.ScaleIntegrity;
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.Arbitration.CallStanding(topic, day);
            _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
            // Try bribing a principled backer (refused) and an unprincipled one (accepted)
            var resultPrincipled = _expansions.Arbitration.TryBribeBacker(topic, CrossingIds.NpcMattis);
            var resultBought = _expansions.Arbitration.TryBribeBacker(topic, "npc_bram_ostrowski");
            _expansions.Arbitration.DeclareBacker(topic, "npc_leva_quist");
            _statusLabel.Text = $"Bribe results: Mattis={resultPrincipled}, Bram={resultBought}. Ruling: {_expansions.Arbitration.GetRuling(topic)?.shape}";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnArbitrationOverturnClicked()
        {
            SetupExpansions();
            if (_expansions.Arbitration.BackerPool.Count == 0)
            {
                _expansions.LoadDefaultBackerPool();
            }
            string topic = "quest_crossing_the_terms";
            int day = _core != null ? _core.Clock.Day : _simDay;
            // Ensure a ruling exists to overturn
            if (!_expansions.Arbitration.IsRulingActive(topic))
            {
                _expansions.Arbitration.CallStanding(topic, day);
                _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcOsran);
                _expansions.Arbitration.DeclareBacker(topic, CrossingIds.NpcMattis);
                _expansions.Arbitration.DeclareBacker(topic, "npc_halden_mire");
            }
            bool overturned = _expansions.Arbitration.OverturnRuling(topic,
                new List<string> { "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire" });
            _statusLabel.Text = overturned
                ? "Ruling overturned! Counter-backers (Bram, Leva, Halden) hold the Crossing now."
                : "Overturn failed — need 3+ different, living backers.";
            _codexViewer.Text = _expansions.ArbitrationLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerSignClicked()
        {
            SetupExpansions();
            string debtor = CrossingIds.NpcWyn;
            bool firstRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
            bool secondRead = _expansions.Ledger.PresentContract(debtor, 12f, 30, 0.2f, "the pledged grain");
            bool signed = _expansions.Ledger.SignContract(debtor, _core != null ? _core.Clock.Day : _simDay);
            _statusLabel.Text = $"Contract for {debtor}: first reading={firstRead}, second reading={secondRead}, signed={signed}.";
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerTickClicked()
        {
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.Ledger.TickDaily(day);
            _statusLabel.Text = "Ledger day ticked. " + _expansions.LedgerLine();
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void OnLedgerPayClicked()
        {
            SetupExpansions();
            string debtor = CrossingIds.NpcWyn;
            bool paid = _expansions.Ledger.PayContract(debtor, _core != null ? _core.Clock.Day : _simDay);
            _statusLabel.Text = paid
                ? $"Contract for {debtor} paid in full. The ink is history."
                : "Payment failed — no signed contract or already paid.";
            _codexViewer.Text = _expansions.LedgerLine();
            RefreshExpansionsStatus();
        }

        private void SaveHoldfast()
        {
            if (_core == null) return;
            if (HoldfastSaveStore.TrySave(_core.CaptureSave()))
            {
                _holdfastDirty = false;
                GD.Print($"[Ashfall Godot] Holdfast S1 save written (day {_core.Clock.Day}).");
            }
        }

        /// <summary>Writes the S1 save only when a system changed since the last flush.</summary>
        private void FlushHoldfastIfDirty()
        {
            if (_holdfastDirty) SaveHoldfast();
        }

        private void SaveDutyRoster()
        {
            if (_dutyRoster == null) return;
            if (DutyRosterSaveStore.TrySave(_dutyRoster.CaptureSave()))
            {
                _dutyRosterDirty = false;
                GD.Print($"[Ashfall Godot] Duty Roster save written (day {_dutyRoster.Clock.Day}).");
            }
        }

        private void FlushDutyRosterIfDirty()
        {
            if (_dutyRosterDirty) SaveDutyRoster();
        }

        private void SaveExpansionHub()
        {
            if (_expansions == null) return;
            int day = _core != null ? _core.Clock.Day : _simDay;
            if (ExpansionHubSaveStore.TrySave(_expansions.CaptureSave(day)))
            {
                _expansionHubDirty = false;
                GD.Print($"[Ashfall Godot] Expansion hub save written (day {day}).");
            }
        }

        private void FlushExpansionHubIfDirty()
        {
            if (_expansionHubDirty) SaveExpansionHub();
        }

        // -----------------------------------------------------------------
        // Phantom Memory (Antigravity #41)
        // -----------------------------------------------------------------

        private void SetupPhantom()
        {
            if (_phantomMemory != null) return;
            _phantomMemory = PhantomMemoryHostSession.Create(_dataDir);
            _phantomMemory.StateChanged += () => SavePhantomMemory();

            var save = PhantomMemorySaveStore.TryLoad();
            if (save != null)
            {
                _phantomMemory.RestoreSave(save);
                GD.Print("[Ashfall Godot] Phantom Memory state restored.");
            }
        }

        private void OnPhantomScavengeClicked()
        {
            SetupPhantom();
            _statusLabel.Text = _phantomMemory.ScavengeItem("survivor_gunner_mikhail", "armour_heavy_military");
        }

        private void OnPhantomTickClicked()
        {
            SetupPhantom();
            _statusLabel.Text = _phantomMemory.TickDemo();
        }

        private void SavePhantomMemory()
        {
            if (_phantomMemory == null) return;
            if (PhantomMemorySaveStore.TrySave(_phantomMemory.CaptureSave()))
                GD.Print("[Ashfall Godot] Phantom Memory save written.");
        }

        // ── THE DOSE (Exp 07) host wiring ───────────────────────────────

        private void SetupDoseLedger()
        {
            if (_doseLedger != null) return;
            _doseLedger = DoseLedgerHostSession.Create(_dataDir);
            _doseLedger.StateChanged += () => _doseLedgerDirty = true;

            var save = DoseLedgerSaveStore.TryLoad();
            if (save != null)
            {
                _doseLedger.RestoreSave(save);
                _doseLedgerDirty = false; // restore just raised state-change events
                GD.Print("[Ashfall Godot] Dose Ledger state restored.");
            }

            if (_doseSurface == null && _rightColumn != null)
            {
                _doseSurface = new DoseRegisterSurface();
                _rightColumn.AddChild(_doseSurface);
            }
            if (_doseSurface != null)
            {
                _doseSurface.BindSession(_doseLedger);
                _doseSurface.RefreshView();
            }
        }

        private void OnDoseRegisterClicked()
        {
            SetupDoseLedger();
            _statusLabel.Text = "The Dose Register is open. Four tabs, four people who keep books.";
        }

        private void OnDoseSealClicked()
        {
            SetupDoseLedger();
            _doseLedger.SealDemoSurvivors();
            _statusLabel.Text = "Dosimeters sealed: Gunner Mikhail (tag_1), Elena Vasquez (tag_2).";
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseScribeClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.ScribeReading(180f, highEnergy: false);
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseDiagnoseClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed);
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseCohortClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.BookDemoChild();
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseVolunteerClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.SignDemoVolunteer();
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void SaveDoseLedger()
        {
            if (_doseLedger == null) return;
            int day = _core != null ? _core.Clock.Day : _simDay;
            if (DoseLedgerSaveStore.TrySave(_doseLedger.CaptureSave(day)))
            {
                _doseLedgerDirty = false;
                GD.Print($"[Ashfall Godot] Dose Ledger save written (day {day}).");
            }
        }

        private void FlushDoseLedgerIfDirty()
        {
            if (_doseLedgerDirty) SaveDoseLedger();
        }

        // ── INVENTORY (ported from Unity _Game/Inventory) host wiring ───

        private void SetupInventory()
        {
            if (_inventory != null) return;
            _inventory = InventoryHostSession.Create(_dataDir);
            _inventory.StateChanged += () => SaveInventory();

            if (_inventoryPanel == null && _rightColumn != null)
            {
                _inventoryPanel = new AtomicWar.GodotApp.Inventory.InventoryPanel();
                _rightColumn.AddChild(_inventoryPanel);
            }
            if (_inventoryPanel != null)
            {
                _inventoryPanel.Bind(_inventory);
                _inventoryPanel.RefreshView();
            }
        }

        private void OnInventoryOpenClicked()
        {
            SetupInventory();
            _statusLabel.Text = "Inventory open. Storage and gear are listed in the right panel.";
            _codexViewer.Text = _inventory.InventoryLine() + "\n\n" + _inventory.EquipLine();
        }

        private void OnInventoryAddClicked(string itemId, int amount)
        {
            SetupInventory();
            _statusLabel.Text = _inventory.Add(itemId, amount);
            _inventoryPanel.RefreshView();
            _codexViewer.Text = _inventory.InventoryLine() + "\n\n" + _inventory.EquipLine();
        }

        private void OnInventoryRemoveClicked(string itemId, int amount)
        {
            SetupInventory();
            _statusLabel.Text = _inventory.Remove(itemId, amount);
            _inventoryPanel.RefreshView();
        }

        private void OnInventoryConsumeClicked(string itemId)
        {
            SetupInventory();
            _statusLabel.Text = _inventory.Consume(itemId);
            _inventoryPanel.RefreshView();
        }

        private void OnInventoryCheckClicked()
        {
            SetupInventory();
            var inv = _inventory.Inventory;
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== ITEM CHECK ===");
            sb.AppendLine($"Canned food: {inv.CountById("canned_food")} on hand (trip need 3)");
            sb.AppendLine($"Clean water: {inv.CountById("clean_water")} on hand (trip need 2)");
            sb.AppendLine($"Iodine pills: {inv.CountById("iodine_pills")}");
            sb.AppendLine($"Battery: {inv.CountById("battery")}");
            sb.AppendLine($"Gas mask: {inv.CountById("gas_mask")}");
            sb.AppendLine($"Geiger: {(inv.HasWorkingGeiger() ? "WORKING" : "NONE/WORKING")}");
            sb.AppendLine($"Equipped protection: {inv.GetEquippedProtection():F2}");
            _codexViewer.Text = sb.ToString();
            _statusLabel.Text = "Item check complete. See the codex viewer.";
        }

        private void SaveInventory()
        {
            if (_inventory == null) return;
            if (InventorySaveStore.TrySave(_inventory.CaptureSave()))
                GD.Print("[Ashfall Godot] Inventory save written.");
        }

        // ── SURVIVORS (needs + radiation) host wiring ──────────────────

        private void SetupSurvivors()
        {
            if (_survivors != null) return;
            _survivors = new SurvivorsHostSession();
            _survivors.SeedDemoRoster();
            _survivors.StateChanged += () => SaveSurvivors();

            var save = SurvivorsSaveStore.TryLoad();
            if (save != null && save.survivors.Count > 0)
                _survivors.RestoreSave(save);
        }

        private void OnSurvivorsOpenClicked()
        {
            SetupSurvivors();
            _statusLabel.Text = "Survivors panel open. Needs and radiation are simulated.";
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsTickClicked()
        {
            SetupSurvivors();
            _survivors.TickHour(6f);
            _statusLabel.Text = _survivors.LastEvent;
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsExposeClicked(string id, float rads)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.ExposeToZone(id, rads);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsIodineClicked(string id)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.AdministerIodine(id);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsAntiRadClicked(string id, float rads)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.AdministerAntiRad(id, rads);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void SaveSurvivors()
        {
            if (_survivors == null) return;
            if (SurvivorsSaveStore.TrySave(_survivors.CaptureSave()))
                GD.Print("[Ashfall Godot] Survivors save written.");
        }

        // ── THE MUSTER (Exp 06) host wiring ─────────────────────────────

        private void SetupMuster()
        {
            if (_muster != null) return;
            _muster = MusterHostSession.Create(_dataDir);
            _muster.StateChanged += () => SaveMuster();

            if (_currentsRoster == null)
            {
                _currentsRoster = new CurrentsRosterWidget();
                _rightColumn.AddChild(_currentsRoster);
            }
            _currentsRoster.Bind(_muster.Roster, _muster.Engine);
            _currentsRoster.RefreshView();

            if (_campWidget == null)
            {
                _campWidget = new DeserterCoalitionCampWidget();
                _rightColumn.AddChild(_campWidget);
            }
            _campWidget.Bind(_muster.Camp);
            _campWidget.RefreshView();

            if (_witnessPanel == null)
            {
                _witnessPanel = new JournalWitnessPanel();
                _rightColumn.AddChild(_witnessPanel);
            }
            _witnessPanel.Bind(_muster.Witnesses);
            _witnessPanel.RefreshView(_yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay, _muster.AuthorBias);

            if (_approachModal == null)
            {
                _approachModal = new ApproachSelectionModal();
                _approachModal.OnApproachChosen += OnMusterApproachChosen;
                _approachModal.OnModalClosed += () =>
                {
                    _approachModal.QueueFree();
                    _approachModal = null;
                };
                AddChild(_approachModal);
            }

            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            _muster.Escalate(day);
            GD.Print("[Ashfall Godot] Muster ready. Day " + day +
                     (_muster.Engine.MusterTriggered ? " — THE MUSTER IS OPEN." : "."));
        }

        private void OnMusterEscalateClicked()
        {
            SetupMuster();
            int target = Math.Min(360, _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay + 10 : _simDay + 10);
            _statusLabel.Text = _muster.Escalate(target);
            _currentsRoster.RefreshView();
            _campWidget.RefreshView();
        }

        private void OnMusterRallyClicked()
        {
            SetupMuster();
            _statusLabel.Text = _muster.RallyDeserter();
            _campWidget.RefreshView();
        }

        private void OnMusterStrategyBClicked()
        {
            SetupMuster();
            _statusLabel.Text = _muster.SetStrategy(QuestApproach.B);
            _campWidget.RefreshView();
        }

        private void OnMusterStrategyDClicked()
        {
            SetupMuster();
            _statusLabel.Text = _muster.SetStrategy(QuestApproach.D);
            _campWidget.RefreshView();
        }

        private void OnMusterWitnessesClicked()
        {
            SetupMuster();
            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            _witnessPanel.RefreshView(day, _muster.AuthorBias);
            _statusLabel.Text = _muster.Witnesses.Count == 0
                ? "No witness accounts loaded."
                : $"Three accounts: {_muster.Witnesses.Count} loaded. Day {day} · {_muster.AuthorBias} author.";
        }

        private void OnMusterAuthorBiasClicked()
        {
            SetupMuster();
            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            _statusLabel.Text = _muster.CycleAuthorBias();
            _witnessPanel.RefreshView(day, _muster.AuthorBias);
        }

        private void OnMusterEpiloguesClicked()
        {
            SetupMuster();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== THE EPILOGUE MATRIX (DAY 360) ===");
            for (int i = 0; i < _muster.Epilogues.Count; i++)
            {
                var e = _muster.Epilogues[i];
                bool resolved = _muster.Engine.EndingKeyForAny(e.endingKey);
                sb.AppendLine(resolved
                    ? $"[RESOLVED] {e.title}"
                    : $"[open]     {e.title}");
            }
            sb.AppendLine();
            sb.AppendLine("=== RESOLVED OUTCOMES ===");
            bool any = false;
            for (int i = 0; i < _muster.Epilogues.Count; i++)
            {
                var e = _muster.Epilogues[i];
                string prose = _muster.EndingProseFor(e.endingKey);
                if (_muster.Engine.EndingKeyForAny(e.endingKey) && prose.Length > 0)
                {
                    any = true;
                    sb.AppendLine(prose);
                    sb.AppendLine();
                }
            }
            if (!any) sb.AppendLine("None. The Muster has not resolved an outcome yet.");
            _codexViewer.Text = sb.ToString();
            _statusLabel.Text = $"Epilogue matrix: {_muster.Epilogues.Count} outcomes.";
        }

        private void OnMusterRosterClicked()
        {
            SetupMuster();
            _statusLabel.Text = $"Currents shown: {_muster.Roster.Count} (fifteenth: faction_hydro_barons).";
        }

        private void OnMusterRateCardClicked()
        {
            SetupMuster();
            var def = _muster.Engine.FindDefinition("quest_the_rate_card_war");
            if (def == null)
            {
                _statusLabel.Text = "Rate Card War questline not registered.";
                return;
            }
            _approachModal.ShowQuestline(def.questlineId, def.approaches);
            _statusLabel.Text = "Rate Card War: choose an approach.";
        }

        private void OnMusterApproachChosen(QuestApproach approach)
        {
            if (_muster == null) return;
            _statusLabel.Text = _muster.SelectApproach("quest_the_rate_card_war", approach);
            _currentsRoster.RefreshView();
        }

        /// <summary>Auto-escalate the Muster from the Year-of-Ash clock.</summary>
        private void AutoEscalateMuster()
        {
            if (_yearOfAsh == null) return;
            SetupMuster();
            _muster.Escalate(_yearOfAsh.Timeline.CurrentDay);
            _currentsRoster.RefreshView();
            _campWidget.RefreshView();
            _witnessPanel.RefreshView(_yearOfAsh.Timeline.CurrentDay, _muster.AuthorBias);
        }

        private void SaveMuster()
        {
            if (_muster == null) return;
            if (MusterSaveStore.TrySave(_muster.CaptureSave()))
                GD.Print("[Ashfall Godot] Muster save written.");
        }

        private void RefreshIceRoadLabel()
        {
            if (_core == null) return;
            if (_iceRoadLabel != null)
                _iceRoadLabel.Text = _core.StatusLine() + "\n" + _core.BrineLine() + "\n" +
                    _core.QuestLine() + "\n" + _core.EndingLine();
            if (_catalogLabel != null)
                _catalogLabel.Text = _core.CatalogLine() + "\n" + _core.CensusLine();
            if (_briefingPreviewLabel != null)
                _briefingPreviewLabel.Text = HoldfastBriefingView.PreviewLine(_core.CurrentQuest);
        }

        /// <summary>Headless smoke: dose register surface builds, actions run, tabs render.</summary>
        private void RunDoseUiTestAndQuit()
        {
            BuildUserInterface();
            SetupDoseLedger();

            bool surface = _doseSurface != null;
            bool npcs = _doseLedger.Registers.npcs.Count == 4;

            _doseLedger.SealDemoSurvivors();
            string booked = _doseLedger.ScribeReading(120f, highEnergy: true);
            bool book = booked.Contains("band");
            bool diagnose = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed).Contains("Diagnosed");
            bool palliative = _doseLedger.SickList.AssignPalliative("survivor_gunner_mikhail", "plan_morphine_tray");
            string child = _doseLedger.BookDemoChild();
            bool cohort = child.Contains("corrected");
            bool volunteer = _doseLedger.SignDemoVolunteer().Contains("banked");

            string ledgerText = _doseLedger.LedgerLine();
            bool rendered = ledgerText.Contains("survivor_gunner_mikhail")
                && _doseLedger.SickList.Bands.Count == 1
                && _doseLedger.Cohort.Children.Count == 1
                && _doseLedger.Voluntary.Entries.Count == 1;

            bool pass = surface && npcs && book && diagnose && palliative && cohort && volunteer && rendered;
            GD.Print($"[DoseUiTest] surface={surface} npcs={npcs} book={book} diagnose={diagnose} " +
                     $"palliative={palliative} cohort={cohort} volunteer={volunteer} rendered={rendered}");
            GD.Print(pass ? "DOSE_UITEST PASS" : "DOSE_UITEST FAIL");
            GetTree().Quit(pass ? 0 : 1);
        }

        /// <summary>Headless smoke: inventory panel builds, add/equip/check flow, save roundtrip.</summary>
        private void RunInventoryUiTestAndQuit()
        {
            BuildUserInterface();
            SetupInventory();

            bool panel = _inventoryPanel != null;
            bool catalog = _inventory.Catalog.Count == 15;

            string added = _inventory.Add("canned_food", 6);
            bool addOk = added.Contains("Added");
            string geiger = _inventory.Add("geiger_counter", 1);
            bool geigerOk = geiger.Contains("Added");
            string mask = _inventory.Add("gas_mask", 1);
            bool maskOk = mask.Contains("Added");
            string equip = _inventory.Equip("gas_mask");
            bool equipOk = equip.Contains("Equipped");
            bool working = _inventory.Inventory.HasWorkingGeiger();
            string water = _inventory.Add("clean_water", 4);
            bool waterOk = water.Contains("Added");

            int canned = _inventory.Inventory.CountById("canned_food");
            bool itemCheckCount = canned == 6;
            bool protection = _inventory.Inventory.GetEquippedProtection() > 0f;

            // Save → restore roundtrip.
            var save = _inventory.CaptureSave();
            var fresh = new InventoryHostSession();
            fresh.RestoreSave(save);
            bool roundtrip = fresh.Inventory.CountById("canned_food") == 6
                && fresh.Inventory.GetEquipped(EquipSlot.Face) != null;

            bool pass = panel && catalog && addOk && geigerOk && maskOk && equipOk
                && working && waterOk && itemCheckCount && protection && roundtrip;
            GD.Print($"[InventoryUiTest] panel={panel} catalog={catalog} add={addOk} geiger={geigerOk} " +
                     $"mask={maskOk} equip={equipOk} working={working} water={waterOk} " +
                     $"canned={itemCheckCount} protection={protection} roundtrip={roundtrip}");
            GD.Print(pass ? "INVENTORY_UITEST PASS" : "INVENTORY_UITEST FAIL");
            if (System.IO.File.Exists(InventorySaveStore.SavePath))
                System.IO.File.Delete(InventorySaveStore.SavePath);
            GetTree().Quit(pass ? 0 : 1);
        }

        /// <summary>Headless smoke: survivors rosters build, needs tick, rad exposure, iodine/anti-rad, save roundtrip.</summary>
        private void RunSurvivorsUiTestAndQuit()
        {
            BuildUserInterface();
            SetupSurvivors();

            bool roster = _survivors.Roster.Count == 3;
            _survivors.TickHour(6f);
            bool needsMoved = _survivors.Roster[0].Hunger > 0f;

            string exposed = _survivors.ExposeToZone("survivor_gunner_mikhail", 60f);
            bool doseClimbed = _survivors.Radiation.GetDosimeter("survivor_gunner_mikhail").LifetimeDose > 0f;

            string iodine = _survivors.AdministerIodine("survivor_gunner_mikhail");
            bool resistance = _survivors.Radiation.GetDosimeter("survivor_gunner_mikhail") != null
                && System.Linq.Enumerable.Any(_survivors.Roster, s => s.Id == "survivor_gunner_mikhail");

            string antiRad = _survivors.AdministerAntiRad("survivor_gunner_mikhail", 30f);
            bool antiRadApplied = antiRad.Contains("cleared");

            // Save → restore roundtrip.
            var save = _survivors.CaptureSave();
            var fresh = new SurvivorsHostSession();
            fresh.RestoreSave(save);
            bool roundtrip = fresh.Roster.Count == 3;
            var restoredRad = fresh.Radiation.GetDosimeter("survivor_gunner_mikhail");
            bool radRestored = restoredRad != null;

            bool pass = roster && needsMoved && doseClimbed && resistance && antiRadApplied && roundtrip && radRestored;
            GD.Print($"[SurvivorsUiTest] roster={roster} needs={needsMoved} dose={doseClimbed} " +
                     $"iodine={resistance} antiRad={antiRadApplied} roundtrip={roundtrip} rad={radRestored}");
            GD.Print(pass ? "SURVIVORS_UITEST PASS" : "SURVIVORS_UITEST FAIL");
            if (System.IO.File.Exists(SurvivorsSaveStore.SavePath))
                System.IO.File.Delete(SurvivorsSaveStore.SavePath);
            GetTree().Quit(pass ? 0 : 1);
        }

        /// <summary>Headless smoke: muster roster widget + approach modal render, escalate, select.</summary>
        private void RunMusterUiTestAndQuit()
        {
            BuildUserInterface();
            SetupMuster();

            bool roster = _currentsRoster != null && _muster.Roster.Count >= 15;
            bool camp = _campWidget != null;
            bool witnesses = _witnessPanel != null && _muster.Witnesses.Count == 3;
            bool epilogues = _muster.Epilogues.Count >= 8;
            bool modal = _approachModal != null;
            bool escalate = _muster.Escalate(300).Contains("Muster is open");
            bool campFormed = _muster.Camp.Formed && _muster.Camp.MembersRallied == CoalitionCampSystem.BaseMembers;
            bool strategy = _muster.SetStrategy(QuestApproach.B).Contains("Strategy B");
            bool resolved = _muster.SelectApproach("quest_the_rate_card_war", QuestApproach.A)
                .Contains("selected");
            bool ending = _muster.Engine.EndingKeyFor("quest_the_rate_card_war") == "the_rate_card_revised";
            bool matrix = _muster.Engine.EndingKeyForAny("the_rate_card_revised")
                && _muster.EndingProseFor("the_rate_card_revised").Contains("rate card is finally a published price");
            _muster.CycleAuthorBias();
            bool biasCycle = _muster.AuthorBias != RiskBiasTrait.Realist;

            bool pass = roster && camp && witnesses && epilogues && modal && escalate &&
                        campFormed && strategy && resolved && ending && matrix && biasCycle;
            GD.Print($"[MusterUiTest] roster={roster} camp={camp} witnesses={witnesses} " +
                     $"epilogues={epilogues} modal={modal} escalate={escalate} campFormed={campFormed} " +
                     $"strategy={strategy} select={resolved} ending={ending} matrix={matrix}");
            GD.Print(pass ? "MUSTER_UITEST PASS" : "MUSTER_UITEST FAIL");
            if (System.IO.File.Exists(MusterSaveStore.SavePath))
                System.IO.File.Delete(MusterSaveStore.SavePath);
            GetTree().Quit(pass ? 0 : 1);
        }

        /// <summary>Headless smoke test: build the book, open it, cycle every tab.</summary>
        private void RunJournalUiTestAndQuit()
        {
            BuildUserInterface();
            SetupJournal();

            _journalBook.Open();
            bool opened = _journalBook.IsOpen && _journalBook.Visible;
            int logLen = _journalBook.ActiveTabContent.Length;
            int summaryLen = _journalBook.DetailSummary.Length;

            int tabsWithContent = 0;
            for (int t = 0; t < JournalSystem.TabCount; t++)
            {
                _journal.SwitchTab(t);
                if (_journalBook.ActiveTabContent.Length > 0) tabsWithContent++;
                GD.Print($"[JournalUiTest] tab {t} ({_journalBook.ActiveTab}) content={_journalBook.ActiveTabContent.Length} chars · status=\"{_journalBook.StatusLine}\"");
            }
            _journalBook.Close();
            bool closed = !_journalBook.IsOpen && !_journalBook.Visible;

            bool pass = opened && closed && logLen > 0 && summaryLen > 0 && tabsWithContent == JournalSystem.TabCount;
            GD.Print(pass ? "JOURNAL_UITEST PASS" : "JOURNAL_UITEST FAIL");
            GetTree().Quit(pass ? 0 : 1);
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

        private void OnTickIceRoadClicked()
        {
            SetupIceRoad();
            string delta = _core.TickDay();
            _simDay = _core.Clock.Day;
            _statusLabel.Text =
                $"Day {_core.Clock.Day} tick ({_core.Weather}, {_core.OutdoorCelsius:0}°C): {delta}. " +
                $"{_core.LocationCount} locations.";
            RefreshIceRoadLabel();
            UpdateStatus();
        }

        private void OnCycleWeatherClicked()
        {
            SetupIceRoad();
            _core.CycleWeather();
            _statusLabel.Text =
                $"Weather set to {_core.Weather} ({_core.OutdoorCelsius:0}°C). Next tick uses this.";
            RefreshIceRoadLabel();
        }

        private void OnShowBriefingClicked()
        {
            SetupIceRoad();
            if (_core.QuestCount == 0)
            {
                _statusLabel.Text = "No Holdfast quests in catalog.";
                _codexViewer.Text = _core.CatalogLine();
                RefreshIceRoadLabel();
                return;
            }

            _codexViewer.Text =
                "=== HOLDFAST QUEST BRIEFING ===\n" +
                $"{_core.CatalogLine()}\n" +
                $"Showing {(_core.QuestIndex + 1)}/{_core.QuestCount}\n\n" +
                HoldfastBriefingView.FormatQuest(_core.CurrentQuest, _core.Catalog);
            _statusLabel.Text = HoldfastBriefingView.PreviewLine(_core.CurrentQuest);
            RefreshIceRoadLabel();
            _core.AdvanceQuest();
        }

        private void OnCensusLevyClicked()
        {
            SetupIceRoad();
            string result = _core.HonourDemoLevy();
            _statusLabel.Text = result;
            _codexViewer.Text =
                "=== CENSUS (Ashfall.Core) ===\n" +
                _core.CensusLine() + "\n" +
                "Named cap is three. Honour assigns them away until the levy days run out.\n";
            RefreshIceRoadLabel();
        }

        private void OnOrder12CClicked()
        {
            SetupIceRoad();
            bool wasActive = _core.Census.Order12CActive;
            _core.Activate12C();
            _statusLabel.Text = wasActive
                ? "Order 12-C already published. The unlisted are a reserve. The office will come south when the ice allows."
                : "Order 12-C published. Unlisted occupants of Allocation 12 are a labour reserve.";
            _codexViewer.Text =
                "=== ORDER 12-C (Ashfall.Core) ===\n" +
                _core.QuestLine() + "\n" +
                "\"You are living in a facility that authenticated for fourteen. " +
                "The fourteen did not arrive. I am not collecting you. I am scheduling you.\"\n" +
                "The Second List quest gates on the refuse branch or the membrane resolution.\n";
            RefreshIceRoadLabel();
        }

        private void OnCycleEndingClicked()
        {
            SetupIceRoad();
            string current = _core.Quests.State.endingId;
            // Cycle: none → schedule → reserve → dark road → tender → white → none.
            int index = -1;
            if (!string.IsNullOrEmpty(current))
                for (int i = 0; i < HoldfastEndings.All.Length; i++)
                    if (HoldfastEndings.All[i] == current) { index = i; break; }
            string next = index >= 0 && index + 1 < HoldfastEndings.All.Length
                ? HoldfastEndings.All[index + 1]
                : HoldfastEndings.None;

            if (string.IsNullOrEmpty(next))
            {
                _core.Quests.SetEnding(HoldfastEndings.None);
                _statusLabel.Text = "Ending disarmed. No ending armed — the road stays open.";
            }
            else
            {
                bool armed = _core.SetEnding(next);
                _statusLabel.Text = armed
                    ? $"Ending armed: {HoldfastEndings.DisplayName(next)} [{next}]. " +
                      "Arming a second ending overwrites the first — endings are exclusive."
                    : "Ending rejected: id not in the master list.";
            }
            _codexViewer.Text =
                "=== ENDINGS (Sprint 4) ===\n" +
                _core.EndingLine() + "\n" +
                "Five endings, mutually exclusive. The ice takes a column south and a column north.\n" +
                "Receipts in triplicate. Nobody is shot.\n";
            RefreshIceRoadLabel();
        }

        private void OnSaveHoldfastClicked()
        {
            SetupIceRoad();
            SaveHoldfast();
            _statusLabel.Text =
                $"Holdfast S1 state saved (day {_core.Clock.Day}) → {HoldfastSaveStore.FileName}\n" +
                _core.StatusLine();
        }

        private void OnUnlockPlantClicked()
        {
            SetupIceRoad();
            bool wasUnlocked = _core.Brine.Unlocked;
            _core.UnlockPlant();
            _statusLabel.Text = wasUnlocked
                ? "Plant already unlocked. Salt trade is open."
                : "Plant unlocked. Steam rises from Membrane Hall. The Office has noticed the water.";
            _codexViewer.Text =
                "=== BRINE WATER (Ashfall.Core) ===\n" +
                _core.BrineLine() + "\n" +
                "Sector 4 dies of thirst; District 8 drowns in brine. Potability needs resin, iodine, heat.\n" +
                "Tick days to watch the membrane degrade.\n";
            RefreshIceRoadLabel();
        }

        private void OnRepairMembraneClicked()
        {
            SetupIceRoad();
            bool repaired = _core.RepairMembrane(4);
            _statusLabel.Text = repaired
                ? "Four resin drums rolled into the hall. " + _core.BrineLine()
                : "Repair rejected (resin drums must be positive).";
            _codexViewer.Text =
                "=== MEMBRANE CRISIS ===\n" +
                _core.BrineLine() + "\n" +
                "Resin above 40% restores steam; the Cluster rewarms to 14°C.\n";
            RefreshIceRoadLabel();
        }

        private void OnToggleOutfallClicked()
        {
            SetupIceRoad();
            _core.ToggleOutfallShift();
            _statusLabel.Text = _core.OutfallShifted
                ? "Outfall shift on — brine load cut to 55%."
                : "Outfall shift off — full brine load resumes.";
            _codexViewer.Text =
                "=== OUTFALL SHIFT ===\n" +
                _core.BrineLine() + "\n" +
                "Shifting the outfall costs bodies on the yard. It halves what the membrane eats.\n";
            RefreshIceRoadLabel();
        }

        private void OnViewCodexClicked()
        {
            // One surface: the bunker ledger is the lore archive.
            if (_journalBook != null) _journalBook.Open();
            LoadGameCatalogs();
            UpdateStatus();
        }

        private void OnDiagnosticsClicked()
        {
            var diag = new System.Text.StringBuilder();
            diag.AppendLine("=== ASHFALL SYSTEM DIAGNOSTICS (GODOT .NET) ===");
            diag.AppendLine($"Engine: Godot {Engine.GetVersionInfo()["string"]}");
            diag.AppendLine($"Target FPS: {Engine.MaxFps}");
            diag.AppendLine($"Current FPS: {Engine.GetFramesPerSecond():F1}");
            diag.AppendLine($"Static Memory: {OS.GetStaticMemoryUsage() / (1024 * 1024.0):F2} MB");
            diag.AppendLine($"GC Heap Memory: {GC.GetTotalMemory(false) / (1024 * 1024.0):F2} MB");
            diag.AppendLine($"Operating System: {OS.GetName()} ({OS.GetDistributionName()})");
            diag.AppendLine($"Architecture: {Engine.GetArchitectureName()}");
            diag.AppendLine($"Processors: {OS.GetProcessorCount()} cores");
            diag.AppendLine($"Video Adapter: {RenderingServer.GetVideoAdapterName()}");
            if (_journal != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== JOURNAL STATE ===");
                diag.AppendLine($"Entries: {_journal.EntryCount}/64 · Unlocks: {_journal.CodexUnlockCount}");
                diag.AppendLine($"Unread: {_journal.HasUnread} · Ping: {_journal.NotificationPing} · Tab: {_journal.ActiveTab}");
                diag.AppendLine($"Open: {_journal.HudIsOpen} · Save: {JournalSaveStore.Exists}");
            }
            if (_core != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== ICE ROAD (Ashfall.Core) ===");
                diag.AppendLine($"Unlocked: {_core.IceRoad.IsUnlocked}  Open: {_core.IceRoad.IsOpen}");
                diag.AppendLine($"Thickness: {_core.IceRoad.IceThicknessM:0.000} m  Window: {_core.IceRoad.WindowDaysRemaining}/{_core.IceRoad.State.windowLengthDays}");
                diag.AppendLine($"Weather: {_core.Weather}  Outdoor: {_core.OutdoorCelsius:0}°C");
                diag.AppendLine($"Gate blocked: {_core.GateBlocked}  Clerk: {_core.IceRoad.State.clerkStarted}");
                diag.AppendLine(_core.CatalogLine());
                diag.AppendLine(_core.CensusLine());
                diag.AppendLine(_core.BrineLine());
                diag.AppendLine(_core.QuestLine());
                diag.AppendLine(_core.EndingLine());
                diag.AppendLine($"Data: {_dataDir}");
                diag.AppendLine($"S1 save: {(HoldfastSaveStore.Exists ? HoldfastSaveStore.SavePath : "none")} · dirty: {_holdfastDirty}");
                diag.AppendLine();
                diag.AppendLine("=== HOLDFAST BRIEFING ===");
                diag.AppendLine(HoldfastBriefingView.FormatQuest(_core.CurrentQuest, _core.Catalog));
            }
            if (_yearOfAsh != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== YEAR OF ASH (Ashfall.Core) ===");
                diag.AppendLine(_yearOfAsh.GetStatusSummary());
            }
            if (_dutyRoster != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== DUTY ROSTER (Ashfall.Core) ===");
                diag.AppendLine(_dutyRoster.WallLine());
                diag.AppendLine(_dutyRoster.EncountersLine());
                diag.AppendLine("Save: " + (DutyRosterSaveStore.Exists ? DutyRosterSaveStore.SavePath : "none")
                    + " · dirty: " + _dutyRosterDirty);
            }
            if (_expansions != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== EXPANSION HUB (Ashfall.Core) ===");
                diag.AppendLine("Save: " + (ExpansionHubSaveStore.Exists ? ExpansionHubSaveStore.SavePath : "none")
                    + " · dirty: " + _expansionHubDirty);
            }
            if (_doseLedger != null)
            {
                diag.AppendLine();
                diag.AppendLine("=== THE DOSE (Ashfall.Core) ===");
                diag.AppendLine(_doseLedger.DoseStatusLine());
                diag.AppendLine("Save: " + (DoseLedgerSaveStore.Exists ? DoseLedgerSaveStore.SavePath : "none")
                    + " · dirty: " + _doseLedgerDirty);
            }
            _codexViewer.Text = diag.ToString();
        }

        // -----------------------------------------------------------------
        // Year of Ash (Days 180-360) Wiring
        // -----------------------------------------------------------------

        private void SetupYearOfAsh()
        {
            if (_yearOfAsh != null) return;
            _yearOfAsh = YearOfAshHostSession.Create(_dataDir);
            BuildYearOfAshPanel();

            // Questline progress rides the same save as the rest of Year of Ash, so any
            // resolution marks it dirty exactly like an encounter does.
            _yearOfAsh.Quests.OnQuestlineStarted += def =>
                GD.Print($"[Ashfall Godot] Questline started: {def.questlineId}");
            _yearOfAsh.Quests.OnQuestlineResolved += (id, status) =>
                GD.Print($"[Ashfall Godot] Questline {id} → {status}");

            int playable = _yearOfAsh.Quests.GetPlayableQuestlines(_yearOfAsh.Timeline.CurrentDay).Count;
            int withheld = _yearOfAsh.Quests.WithheldQuestlineCount(_yearOfAsh.Timeline.CurrentDay);
            GD.Print($"[Ashfall Godot] Year of Ash ready. Day {_yearOfAsh.Timeline.CurrentDay} · " +
                     $"questlines: {playable} playable, {withheld} withheld (no authored choices)");
        }

        /// <summary>
        /// Wires the four Year-of-Ash presentation widgets (faction war map, radio
        /// terminal, geothermal heating, radon ventilation) into the right column.
        /// They were authored but never instantiated — dead presentation code.
        /// Widgets are added to the tree before BindSession so their _Ready has run
        /// and the labels exist when the first RefreshView fires.
        /// </summary>
        private void BuildYearOfAshPanel()
        {
            if (_yearOfAshPanel != null || _rightColumn == null || _yearOfAsh == null) return;

            _yearOfAshPanel = new VBoxContainer();
            _yearOfAshPanel.AddThemeConstantOverride("separation", 8);

            var header = new Label
            {
                Text = "YEAR OF ASH — SYSTEMS (DAYS 180–360)"
            };
            header.AddThemeFontSizeOverride("font_size", 15);
            header.AddThemeColorOverride("font_color", new Color(0.4f, 0.8f, 0.9f));
            _yearOfAshPanel.AddChild(header);

            _factionWarMap = new FactionWarMapWidget();
            _geothermalWidget = new GeothermalHeatingWidget();
            _radonWidget = new RadonVentilationWidget();
            _radioTerminal = new RadioBroadcastTerminal();

            _yearOfAshPanel.AddChild(_factionWarMap);
            _yearOfAshPanel.AddChild(_geothermalWidget);
            _yearOfAshPanel.AddChild(_radonWidget);
            _yearOfAshPanel.AddChild(_radioTerminal);

            // Enter the tree first so each widget's _Ready has built its labels.
            _rightColumn.AddChild(_yearOfAshPanel);

            _factionWarMap.BindSession(_yearOfAsh);
            _geothermalWidget.BindSession(_yearOfAsh);
            _radonWidget.BindSession(_yearOfAsh);
            _radioTerminal.LoadBroadcasts(_dataDir);
            _radioTerminal.RefreshView(_yearOfAsh.Timeline.CurrentDay);
        }

        private void OnDoorEncounterClicked()
        {
            SetupYearOfAsh();
            if (_yearOfAsh.Encounters.Catalog.Count == 0)
            {
                _statusLabel.Text = "No door encounters found in catalog.";
                return;
            }

            var enc = _yearOfAsh.Encounters.Catalog[_doorEncounterIndex % _yearOfAsh.Encounters.Catalog.Count];
            _doorEncounterIndex++;
            _doorModal.DisplayEncounter(enc, _yearOfAsh.DemoRoster);
            _statusLabel.Text = $"Shelter door visitor arrived: {enc.visitorName}.";
        }

        private void OnDoorEncounterChoiceClicked(DoorEncounterEntry encounter, EncounterChoice choice)
        {
            if (_yearOfAsh == null) return;
            var result = _yearOfAsh.Encounters.ResolveChoice(encounter, choice, _yearOfAsh.DemoRoster);
            _doorModal.DisplayResolution(result);
            _statusLabel.Text = $"Encounter resolved: {encounter.visitorName}. Morale: {result.netMoraleDelta:+#;-#;0}, Guilt: {result.netGuiltDelta:+#;-#;0}";
            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());
        }

        /// <summary>
        /// Opens the questline ledger. Resumes the first active questline if one is in
        /// flight, otherwise offers what can be started today.
        /// </summary>
        private void OnQuestlinesClicked()
        {
            SetupYearOfAsh();
            int day = _yearOfAsh.Timeline.CurrentDay;

            var active = _yearOfAsh.Quests.State.active
                .Find(a => a.status == QuestlineStatus.Active);
            if (active != null && ShowQuestlineStage(active.questlineId, day))
            {
                _statusLabel.Text = $"Questline in progress: {active.questlineId} (day {day}).";
                return;
            }

            var offers = _yearOfAsh.Quests.GetPlayableQuestlines(day);
            int withheld = _yearOfAsh.Quests.WithheldQuestlineCount(day);
            _questlineModal.DisplayOffers(offers, day, withheld);
            _statusLabel.Text = withheld > 0
                ? $"{offers.Count} questlines open on day {day}. {withheld} withheld — no authored choices."
                : $"{offers.Count} questlines open on day {day}.";
        }

        /// <summary>Renders the current stage of an active questline. False if it cannot.</summary>
        private bool ShowQuestlineStage(string questlineId, int day)
        {
            var record = _yearOfAsh.Quests.GetActiveRecord(questlineId);
            var def = _yearOfAsh.Quests.FindDefinition(questlineId);
            if (record == null || def == null) return false;

            var stage = def.FindStage(record.currentStageId);
            if (stage == null || stage.choices.Count == 0) return false;

            _questlineModal.DisplayStage(def, stage, day);
            return true;
        }

        private void OnQuestlineChosen(QuestlineDefinition def)
        {
            if (_yearOfAsh == null || def == null) return;
            int day = _yearOfAsh.Timeline.CurrentDay;

            if (!_yearOfAsh.Quests.StartQuestline(def.questlineId, day))
            {
                _statusLabel.Text = $"Could not start {def.questlineId} — already active or unknown.";
                return;
            }

            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());
            ShowQuestlineStage(def.questlineId, day);
            _statusLabel.Text = $"Questline begun: {def.title} (day {day}).";
        }

        private void OnQuestlineChoiceTaken(string questlineId, string choiceId)
        {
            if (_yearOfAsh == null) return;
            int day = _yearOfAsh.Timeline.CurrentDay;

            var result = _yearOfAsh.Quests.TakeChoice(questlineId, choiceId, day);
            if (result == null)
            {
                _statusLabel.Text = $"Choice {choiceId} was refused by {questlineId}.";
                return;
            }

            // A choice that moves a faction moves the actual war model, not just text.
            if (!string.IsNullOrEmpty(result.factionId) && result.factionDelta != 0)
                _yearOfAsh.FactionWar.ModifyStanding(result.factionId, result.factionDelta);

            bool ended = result.newQuestStatus != QuestlineStatus.Active;
            _questlineModal.DisplayResolution(result, ended);

            // Persist immediately: questline progress is the one Year of Ash surface a
            // player would most obviously expect to survive a quit.
            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());

            _statusLabel.Text = ended
                ? $"{questlineId} → {result.newQuestStatus}. Morale {result.moraleDelta:+#;-#;0}, guilt {result.guiltDelta:+#;-#;0}."
                : $"{questlineId} advanced to {result.nextStageId}.";

            if (!ended) ShowQuestlineStage(questlineId, day);
        }

        private void OnTickYearOfAshClicked()
        {
            SetupYearOfAsh();
            int targetDay = Math.Min(360, _yearOfAsh.Timeline.CurrentDay + 10);
            _yearOfAsh.TickDay(targetDay);
            // Persist after the day advance too, so a quit between ticks doesn't
            // lose the timeline (encounter resolutions already save on their own).
            YearOfAshSaveStore.TrySave(_yearOfAsh.CaptureSave());
            AutoEscalateMuster();
            if (_radioTerminal != null)
                _radioTerminal.RefreshView(_yearOfAsh.Timeline.CurrentDay);
            _statusLabel.Text = _yearOfAsh.GetStatusSummary();
            if (_codexViewer != null)
            {
                _codexViewer.Text = $"=== YEAR OF ASH (DAYS 180-360) ===\n{_yearOfAsh.GetStatusSummary()}\n\n" +
                                   $"Phase: {_yearOfAsh.Timeline.CurrentPhase}\n" +
                                   $"Ambient Temp: {_yearOfAsh.Timeline.AmbientTemperatureCelsius:F1}°C\n" +
                                   $"Caloric Multiplier: {_yearOfAsh.Timeline.CalculateCaloricMultiplier():F2}x\n" +
                                   $"Radon Infiltration: {_yearOfAsh.Timeline.RadonInfiltrationRate * 100:F1}%\n" +
                                   $"War Tension: {_yearOfAsh.FactionWar.WarTension}/100\n" +
                                   $"Dominant Faction: {_yearOfAsh.FactionWar.DominantFactionId}\n" +
                                   $"Encounters Available: {_yearOfAsh.Encounters.Catalog.Count}\n";
        }
    }
}
}
