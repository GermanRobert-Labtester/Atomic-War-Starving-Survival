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
using AtomicWar.GodotApp.Host;
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
        private EventsHostSession _eventsHost = null!;
        private ExpansionQuestHostSession _expansionQuests = null!;
        private SaveLoadHostSession _saveLoadHost = null!;

        // Questline master registry (loaded early for expansion quest ID validation)
        private QuestlineMasterCatalog _questlineMaster = null!;

        // Journal (docs/ui/JOURNAL_UI_PLAN.md)
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


        // Sleep / Advance confirmation fields
        private const double AdvanceCountdownDefaultSeconds = 3.0;
        private double _advanceTimerRemaining;
        private bool _advanceConfirmed;
        private bool _advanceCancelled;

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
                case HostCliAction.JournalSaveSelfTest:
                    GetTree().Quit(HostCli.RunJournalSaveSelfTest());
                    return;
                case HostCliAction.MoralChoiceSelfTest:
                    GetTree().Quit(HostCli.RunMoralChoiceSelfTest(_dataDir));
                    return;
                case HostCliAction.ChemicalDependencySaveSelfTest:
                    GetTree().Quit(HostCli.RunChemicalDependencySaveSelfTest());
                    return;
                case HostCliAction.MedicalWardSaveSelfTest:
                    GetTree().Quit(HostCli.RunMedicalWardSaveSelfTest());
                    return;
                case HostCliAction.WeatherSaveSelfTest:
                    GetTree().Quit(HostCli.RunWeatherSaveSelfTest());
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
                case HostCliAction.JournalWeatherPanelSelfTest:
                    GetTree().Quit(HostCli.RunJournalWeatherPanelSelfTest());
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
                case HostCliAction.InventorySaveSelfTest:
                    GetTree().Quit(HostCli.RunInventorySaveSelfTest());
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
            // Questline master registry: loaded early so expansion quest catalogs
            // can validate their quest IDs against the canonical list.
            _questlineMaster = new QuestlineMasterCatalogLoader(
                new FileSystemIO(), new SystemTextJsonSerializer()).Load(_dataDir);
            GD.Print($"[Ashfall Godot] Questline master: {_questlineMaster.Count} quest IDs registered");
            SetupExpansions();
            // Year of Ash used to initialise lazily on first button press, so its save
            // was not restored at boot and it was the only subsystem with no banner line.
            SetupYearOfAsh();
            // Moral choice ledger ("The Weight of Survival"): constructed at boot so
            // its save restores before any encounter can resolve against a blank ledger.
            SetupMoralChoice();

            // ── Save/Load host session ───────────────────────────────────────
            _saveLoadHost = new SaveLoadHostSession();
            _saveLoadHost.Initialize(ProjectSettings.GlobalizePath("user://"));
            AddChild(_saveLoadHost);
            _saveLoadPanel.Bind(_saveLoadHost);
            // ─────────────────────────────────────────────────────────────────
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
                // GD.Print($"[DevUI Diagnostics] FPS: {fps:F0} | Static Mem: {memMb:F1} MB | Godot {s_engineVersion}");
            }

            // Flush any journal writes that were coalesced since the last tick.
            FlushJournalIfDirty();
            // Flush the Holdfast S1 save the same way — one write per burst, not per event.
            FlushHoldfastIfDirty();
            FlushDutyRosterIfDirty();
            FlushExpansionQuestsIfDirty();
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
            FlushMoralChoiceIfDirty();

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

            if (key.Keycode == Key.F && _state == GameState.Playing)
            {
                OpenWeatherForecastPanel();
                GetViewport().SetInputAsHandled();
            }
            else if (key.Keycode == Key.H && _state == GameState.Playing)
            {
                OpenWeatherHistoryPanel();
                GetViewport().SetInputAsHandled();
            }
            else if (key.Keycode == Key.J)
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
            else if (key.Keycode == Key.E && _state == GameState.Playing)
            {
                OpenEventsLogPanel();
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

                // GAP-ARCH-01 Phase 0: save ALL 34 stores on window close, not just
                // the original 11. The partial list silently dropped Verdict, Maritime,
                // Expeditions, Combat, Narrative, Medical, World, Crafting, Caravans,
                // YearOfAsh, Phase0, StartingLevel, Greenhouse, Radio, DailyBriefing,
                // PowerGrid, MedicalWard, Memorial, SilentFoundry, Disease, WastelandMap,
                // EncounterChoice, and all 21 ExpandedShelter stores.
                SaveAll();

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
                $"Day {_simDay} · [J] toggles the ledger · [E] opens events log.";
        }

        private void OpenEventsLogPanel()
        {
            if (_eventsLogPanel == null)
            {
                _eventsLogPanel = new EventsLogPanel();
                _eventsLogPanel.OnClose += () => _eventsLogPanel.Visible = false;
                AddChild(_eventsLogPanel);
            }
            _eventsLogPanel.Bind(_eventsHost);
            _eventsLogPanel.Open();
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















        private void SetupEventsHost()
        {
            _eventsHost = new EventsHostSession(new Ashfall.Core.SystemTextJsonSerializer(), new Ashfall.Core.FileSystemIO());
            AddChild(_eventsHost);
        }

        private bool _expansionQuestsDirty;

        private void SetupExpansionQuests()
        {
            if (_expansionQuests != null) return;
            _expansionQuests = ExpansionQuestHostSession.Create(_dataDir);
            _expansionQuests.StateChanged += () => _expansionQuestsDirty = true;

            var save = ExpansionQuestSaveStore.TryLoad();
            if (save != null)
            {
                _expansionQuests.RestoreState(save.state);
            }
        }

        private void SaveExpansionQuests()
        {
            if (_expansionQuests == null) return;
            var state = _expansionQuests.CaptureState();
            var envelope = new ExpansionQuestSaveEnvelope
            {
                version = ExpansionQuestSaveEnvelope.CurrentVersion,
                state = state,
                checksum = SaveChecksum.Compute(state)
            };
            ExpansionQuestSaveStore.Save(envelope);
            _expansionQuestsDirty = false;
        }

        private void FlushExpansionQuestsIfDirty()
        {
            if (_expansionQuestsDirty) SaveExpansionQuests();
        }

        // ── THE MUSTER (Exp 06) host wiring ─────────────────────────────
    }
}
