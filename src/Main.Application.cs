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
using Ashfall.Core.IO;
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
        public override void _Ready()
        {
            GD.Print("[Ashfall Godot] Initializing ASHFALL: Atomic War - Starving Survival...");

            // Register all player-navigable panel descriptors before any navigation occurs.
            Ashfall.Core.UI.PanelRegistryBootstrap.RegisterAll();

            ResolveDataDir();

            // Validate required catalogs before any systems are initialized.
            // This ensures the game cannot start with missing or malformed required data.
            ValidateRequiredCatalogs();

            switch (HostCli.Parse(OS.GetCmdlineUserArgs()))
            {
                case HostCliAction.Help:
                    HostCli.PrintHelp();
                    GetTree().Quit(0);
                    return;
                case HostCliAction.Version:
                    HostCli.PrintVersion(_dataDir);
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
                case HostCliAction.SaveLoadUiFailureSelfTest:
                    GetTree().Quit(HostCli.RunSaveLoadUiFailureSelfTest(_dataDir));
                    return;
                case HostCliAction.PanelBindLifecycleSelfTest:
                    GetTree().Quit(HostCli.RunPanelBindLifecycleSelfTest(_dataDir));
                    return;
                case HostCliAction.SaveStoreChecksumSelfTest:
                    GetTree().Quit(HostCli.RunSaveStoreChecksumSelfTest(_dataDir));
                    return;
                case HostCliAction.SevenDayDeterministicSmokeSelfTest:
                    GetTree().Quit(HostCli.RunSevenDayDeterministicSmokeSelfTest(_dataDir));
                    return;
                case HostCliAction.UiAccessibilitySelfTest:
                    GetTree().Quit(HostCli.RunUiAccessibilitySelfTest());
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
                case HostCliAction.CatalogBootPreflight:
                    GetTree().Quit(HostCli.RunCatalogBootPreflight(_dataDir));
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
                    BeginSnapshotRun(regenerate: false);
                    return;
                case HostCliAction.UiSnapshotRegenerate:
                    BeginSnapshotRun(regenerate: true);
                    return;
            }

            AtomicWar.GodotApp.Settings.UserSettingsStore.Apply(AtomicWar.GodotApp.Settings.UserSettingsStore.Current);

            // ── Save/Load host session ───────────────────────────────────────
            _saveLoadHost = new SaveLoadHostSession();
            _saveLoadHost.Initialize(ProjectSettings.GlobalizePath("user://"));
            AddChild(_saveLoadHost);

            BuildUserInterface();
            _saveLoadPanel.Bind(_saveLoadHost);
            _saveLoadHost.SlotsChanged += UpdateContinueButton;

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

            if (DisplayServer.GetName() == "headless")
            {
                string[] userArgs = OS.GetCmdlineUserArgs();
                if (userArgs != null && userArgs.Length > 0)
                {
                    GD.PrintErr($"[Ashfall Godot] Unrecognized headless argument(s): {string.Join(" ", userArgs)}. Run with --host-help to see valid flags.");
                    GetTree().Quit(1);
                    return;
                }

                GD.Print("[Ashfall Godot] Headless interactive boot completed. Exiting cleanly.");
                GetTree().Quit(0);
                return;
            }
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

            if (_diagnosticsLabel == null || !IsInstanceValid(_diagnosticsLabel)) return;
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
            }

            // Flush any journal writes that were coalesced since the last tick.
            FlushJournalIfDirty();
            // Flush the Holdfast S1 save the same way — one write per burst, not per event.
            FlushHoldfastIfDirty();
            FlushDutyRosterIfDirty();
            FlushExpansionQuestsIfDirty();
            FlushThirdonaryIfDirty();
            FlushExpansionHubIfDirty();
            FlushVerdictIfDirty();
            FlushMaritimeIfDirty();
            FlushExpeditionIfDirty();
            FlushNarrativeIfDirty();
            FlushEventAdapterIfDirty();
            FlushMedicalIfDirty();
            FlushWorldIfDirty();
            FlushCraftingIfDirty();
            FlushCaravanIfDirty();
            FlushYearOfAshIfDirty();
            FlushPhase0IfDirty();
            FlushMoralChoiceIfDirty();
            FlushCampaignDayIfDirty();

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
            if (!@event.IsPressed() || @event.IsEcho()) return;

            if (AshfallInputActions.IsForecast(@event) && _state == GameState.Playing)
            {
                OpenWeatherForecastPanel();
                GetViewport().SetInputAsHandled();
            }
            else if (AshfallInputActions.IsWeatherHistory(@event) && _state == GameState.Playing)
            {
                OpenWeatherHistoryPanel();
                GetViewport().SetInputAsHandled();
            }
            else if (AshfallInputActions.IsJournal(@event))
            {
                if (_state == GameState.Playing && _dashboard.Visible)
                    OpenPlayerPanel("journal");
                else
                    ToggleJournal();
                GetViewport().SetInputAsHandled();
            }
            else if (AshfallInputActions.IsHelp(@event) && _state == GameState.Playing)
            {
                ToggleDeveloperConsole();
                GetViewport().SetInputAsHandled();
            }
            else if (AshfallInputActions.IsEvents(@event) && _state == GameState.Playing)
            {
                OpenEventsLogPanel();
                GetViewport().SetInputAsHandled();
            }
            else if (_journalBook != null && _journalBook.IsOpen)
            {
                if (AshfallInputActions.GetJournalTabNumber(@event, out int tab))
                {
                    _journal.SwitchTab(tab - 1);
                    GetViewport().SetInputAsHandled();
                }
                else if (AshfallInputActions.IsCloseOrCancel(@event))
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

        /// <summary>
        /// Validate that all required catalogs are present and well-formed.
        /// Throws if any required catalog is missing or malformed, preventing the game from starting.
        /// </summary>
        private void ValidateRequiredCatalogs()
        {
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();

            // Use CatalogBootValidator to check all registered catalogs
            var report = CatalogBootValidator.Validate(_dataDir, fileIO, json);

            GD.Print(report.ToString());

            // Throw if any required catalogs failed to load
            CatalogBootValidator.ThrowIfRequiredFailed(report);
        }

        /// <summary>
        /// Snapshot regression driver. Mounts SnapshotOrchestrator into the
        /// tree (it needs process frames to render each panel in a SubViewport
        /// and quits the app when the run completes):
        ///   diff mode      — capture into snapshot-capture/ and compare against
        ///                    snapshots/ goldens; per-panel MATCH/NEW/DRIFT/FAIL;
        ///                    exit 1 on any drift or capture failure
        ///   regenerate mode — capture straight into snapshots/ (overwrites goldens)
        /// SubViewport texture reads need a real renderer; with --headless every
        /// target reports FAIL (renderer unavailable) instead of writing blanks.
        /// </summary>
        private void BeginSnapshotRun(bool regenerate)
        {
            string goldenRoot = HostCli.SnapshotGoldenRoot();
            var orch = new SnapshotOrchestrator();
            AddChild(orch);
            if (regenerate)
            {
                GD.Print($"[UiSnapshot] REGENERATE — overwriting goldens in {goldenRoot}");
                orch.BeginRegenerate(SnapshotHarness.Targets, goldenRoot);
            }
            else
            {
                string captureRoot = HostCli.SnapshotCaptureRoot();
                GD.Print($"[UiSnapshot] DIFF — captures in {captureRoot}, goldens in {goldenRoot}");
                orch.BeginDiff(SnapshotHarness.Targets, goldenRoot, captureRoot);
            }
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
    }
}
