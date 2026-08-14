using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.YearOfAsh;

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
        private int _doorEncounterIndex = 0;

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

        // Journal save coalescing. Saving on every entry rewrote the whole file once
        // per seeded entry; entries are marked dirty and flushed on the diagnostics tick,
        // on close, and on quit instead.
        private bool _journalDirty;

        // Phase 0 core slice: ice-road seasonal gate + Holdfast catalogs
        private CoreDemoSession _core = null!;
        // Holdfast S1 save coalescing (same pattern as the journal): any state
        // change in IceRoad or Census marks the save dirty; the diagnostics tick
        // flushes it. Quit and the explicit menu button flush immediately.
        private bool _holdfastDirty;

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
                case HostCliAction.JournalSelfTest:
                    RunSelfTestAndQuit();
                    return;
                case HostCliAction.JournalUiTest:
                    RunJournalUiTestAndQuit();
                    return;
                case HostCliAction.BridgeSelfTest:
                    GetTree().Quit(Ashfall.Bridge.BridgeSelfTest.Run());
                    return;
            }

            BuildUserInterface();
            SetupJournal();
            SetupIceRoad();
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
            _diagnosticsAccum = 0.0;

            if (_diagnosticsLabel == null) return;
            double fps = Engine.GetFramesPerSecond();
            double memMb = (long)OS.GetStaticMemoryUsage() / (1024.0 * 1024.0);
            _diagnosticsLabel.Text = $"FPS: {fps:F0} | Static Mem: {memMb:F1} MB | Godot {s_engineVersion}";

            // Flush any journal writes that were coalesced since the last tick.
            FlushJournalIfDirty();
            // Flush the Holdfast S1 save the same way — one write per burst, not per event.
            FlushHoldfastIfDirty();
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
            AddMenuButton("Unlock plant (salt trade)", OnUnlockPlantClicked);
            AddMenuButton("Repair membrane (resin)", OnRepairMembraneClicked);
            AddMenuButton("Toggle outfall shift", OnToggleOutfallClicked);
            AddMenuButton("Save holdfast state", OnSaveHoldfastClicked);
            AddMenuButton("Hatch Encounter (Year of Ash)", OnDoorEncounterClicked);
            AddMenuButton("Tick Year of Ash (+10 Days)", OnTickYearOfAshClicked);
            AddMenuButton("Open Bunker Ledger  [J]", OnViewCodexClicked);
            AddMenuButton("Inspect System Diagnostics", OnDiagnosticsClicked);
            AddMenuButton("Exit Game", () => { SaveJournal(); SaveHoldfast(); GetTree().Quit(); });

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

        private void RefreshIceRoadLabel()
        {
            if (_core == null) return;
            if (_iceRoadLabel != null)
                _iceRoadLabel.Text = _core.StatusLine() + "\n" + _core.BrineLine();
            if (_catalogLabel != null)
                _catalogLabel.Text = _core.CatalogLine() + "\n" + _core.CensusLine();
            if (_briefingPreviewLabel != null)
                _briefingPreviewLabel.Text = HoldfastBriefingView.PreviewLine(_core.CurrentQuest);
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
            _codexViewer.Text = diag.ToString();
        }

        // -----------------------------------------------------------------
        // Year of Ash (Days 180-360) Wiring
        // -----------------------------------------------------------------

        private void SetupYearOfAsh()
        {
            if (_yearOfAsh != null) return;
            _yearOfAsh = YearOfAshHostSession.Create(_dataDir);
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

        private void OnTickYearOfAshClicked()
        {
            SetupYearOfAsh();
            int targetDay = Math.Min(360, _yearOfAsh.Timeline.CurrentDay + 10);
            _yearOfAsh.TickDay(targetDay);
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
