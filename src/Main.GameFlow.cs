using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Inventory;
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
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
            HostCli.EmitSummary("dashboard_uitest", pass, pass ? 0 : 1);
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

            // Ensure an active save slot is selected before initializing sessions
            _saveLoadHost?.SelectOrCreateDefaultSlot("slot_1");

            // Initialize Holdfast & Starting Level
            SetupHoldfastRuntime();
            _holdfastTerminal.PressNewLedger();
            _holdfastTerminal.OpenTerminal();

            SetupStartingLevel();
            SetupEventsHost();
            SetupExpansionQuests();
            SetupThirdonary();
            SetupExpandedShelterSystems();
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
            UpdateContinueButton();
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
            // Validate against the typed registry — emit a visible diagnostic for any
            // unknown route so dead navigation targets surface immediately.
            var descriptor = Ashfall.Core.UI.PanelRegistry.Resolve(panelId, msg =>
            {
                GD.PrintErr(msg);
                if (_statusLabel != null)
                    _statusLabel.Text = msg;
            });
            if (descriptor == null) return; // dead route — diagnostic already emitted above

            if (_state == GameState.Menu && !descriptor.AvailableInMenu)
            {
                string msg = "[PanelRegistry] BLOCKED ROUTE: '" + panelId + "' is not accessible from the main menu.";
                GD.PrintErr(msg);
                if (_statusLabel != null)
                    _statusLabel.Text = msg;
                return;
            }

            CloseAllOverlayPanels();

            if (descriptor.OpenAction != null)
            {
                descriptor.Bind();
                descriptor.Open();
                return;
            }

            // Fallback for panels not yet migrated to registry actions.
            // All panels should have OpenAction configured via RegisterPlayerSurfaces().
            // If we reach here, a panel was registered but not wired — this is a bug.
            GD.PrintErr($"[PanelRegistry] MISSING ACTIONS: '{panelId}' is registered but has no OpenAction configured. All panels must be wired in RegisterPlayerSurfaces().");

            switch (panelId)
            {
                case "status":
                    SetupSurvivors();
                    SetupWorld();
                    SetupInventory();
                    _statusPanel.Bind(_survivors, _world?.Weather, _powerGrid, _inventory, _simDay);
                    _statusPanel.Open();
                    break;
                case "help":
                    _tutorialPanel.Bind(_simDay);
                    _tutorialPanel.Open();
                    break;
                case "afflictions":
                    SetupSurvivors();
                    SetupInventory();
                    SetupMedical();
                    SetupPhase0();
                    _afflictionsPanel.Bind(_medical, _survivors, _inventory, _phase0?.Respiratory);
                    _afflictionsPanel.Open();
                    break;
                case "radiation_detail":
                    SetupSurvivors();
                    SetupPhase0();
                    _radiationDetailPanel.Bind(_doseLedger, _survivors);
                    _radiationDetailPanel.Open();
                    break;
                case "research":
                    // Lazily create when the expanded-shelter setup hasn't run;
                    // SetupExpandedShelterSystems assigns a fresh instance on
                    // new-game/continue so the panel always rebinds to current state.
                    _sharedResearch ??= new ResearchSystem(log: new GodotLog());
                    _researchPanel.Bind(_sharedResearch);
                    _researchPanel.Open();
                    break;
                case "weather_detail":
                    SetupWorld();
                    _weatherDetailPanel.Bind(_world?.Weather);
                    _weatherDetailPanel.Open();
                    break;
                case "event_detail":
                    SetupEventsHost();
                    _eventDetailPanel.Bind(_eventsHost);
                    _eventDetailPanel.Open();
                    break;
                case "economy_detail":
                    SetupEconomy();
                    _economyDetailPanel.Bind(_economy);
                    _economyDetailPanel.Open();
                    break;
                case "radiation_history":
                    SetupPhase0();
                    _radiationHistoryPanel.Bind(_doseLedger);
                    _radiationHistoryPanel.Open();
                    break;
                case "journal_detail":
                    SetupJournal();
                    _journalDetailPanel.Bind(_journal);
                    _journalDetailPanel.Open();
                    break;
                case "survival_detail":
                    SetupSurvivors();
                    _survivalDetailPanel.Bind(_survivors);
                    _survivalDetailPanel.Open();
                    break;
                case "survivor_detail":
                    SetupSurvivors();
                    var firstSurvivor = _survivors?.RosterState?.FirstOrDefault(s => s != null)?.Id ?? "";
                    _survivorDetailPanel.Bind(_survivors, firstSurvivor);
                    _survivorDetailPanel.Open();
                    break;
                case "inventory_detail":
                    SetupInventory();
                    var firstItem = _inventory?.Inventory?.FindSlot("bandage")?.Item?.id ?? "bandage";
                    _inventoryDetailPanel.Bind(_inventory, firstItem);
                    _inventoryDetailPanel.Open();
                    break;
                case "achievements":
                    SetupSurvivors();
                    _achievementsPanel.Bind(_survivors, _simDay);
                    _achievementsPanel.Open();
                    break;
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
                    SetupYearOfAsh();
                    SetupFactionBranch();
                    SetupMoralChoice();
                    _factionsPanel.Bind(_core.Catalog.Factions, _holdfastRuntime?.Trade, _muster, _expansions, _yearOfAsh, _factionBranch?.Coordinator, _moralChoice);
                    _factionsPanel.Open();
                    break;
                case "quests":
                    SetupHoldfastRuntime();
                    SetupExpansions();
                    SetupDutyRoster();
                    SetupFactionBranch();
                    SetupMoralChoice();
                    _questsPanel.Bind(_core.Quests, _expansions?.CrossingQuests, _dutyRoster, _holdfastRuntime?.Day ?? _simDay, _factionBranch?.Coordinator, _moralChoice);
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
                case "chemical_dependency":
                case "sump_flooding":
                case "decontamination":
                case "kitchen_nutrition":
                case "equipment_condition":
                case "library_study":
                case "archive_desk":
                case "contractor_roster":
                case "mental_health_crisis":
                case "phantom_memory":
                case "traveling_caravan":
                case "medical_ward":
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
            string intakeAssignee = _dutyRoster?.Roster.GetAssignment(Ashfall.Core.DutyRosterIds.RoleIntakeSleeper) ?? "Dr. Sarah Chen";

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
                _radioPanel, _medicalPanel, _dutyRosterPanel,
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
            if (@event.IsActionPressed(AshfallInputActions.UiCancel) || @event.IsActionPressed(AshfallInputActions.Close))
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
            SetupCampaignDay();
            _campaignDay.Calendar.SetDay(_core.Clock.Day);
            _statusLabel.Text = $"Holdfast unlocked. Clerk at the hatch. Day {_core.Clock.Day}. Tick the ice road.";
            _codexViewer.Text =
                "=== ICE ROAD (Ashfall.Core) ===\n" +
                $"Catalog: {_dataDir}\n" +
                $"{_core.CatalogLine()}\n" +
                "Sheet → clerk → freeze window. Not a loading screen.\n\n" +
                HoldfastBriefingView.FormatQuest(_core.CurrentQuest, _core.Catalog);
            RefreshIceRoadLabel();
        }
    }
}
