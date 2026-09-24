// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Difficulty;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.World;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private void RunDashboardUiTestAndQuit()
        {
            BuildUserInterface();
            SetupHoldfastRuntime();
            UpdateHud();
            // Dashboard + inventory are gameplay surfaces; open them from the
            // Playing state so the PanelRegistry menu-availability rule does not
            // correctly block the inventory route.
            _state = GameState.Playing;
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

        /// <summary>
        /// Physics-base self-test: proves the survivor actor is a grounded
        /// CharacterBody2D that collides with the interior static bounds, seeks
        /// its room anchor under acceleration, and animates from the blockout
        /// character sheet. Presentation-only — no Core state is touched.
        /// </summary>
        private async void RunShelterPhysicsSelfTestAndQuit()
        {
            int pass = 0, fail = 0;

            var view = new HoldfastInteriorView();
            AddChild(view);

            var actor = new SurvivorActorView { SurvivorId = "selftest_actor" };
            view.AddChild(actor);

            bool sheetLoaded = actor.Body.Texture != null;
            if (sheetLoaded) pass++; else fail++;

            // Initialise at a start anchor, then seek a distant anchor.
            actor.Position = new Vector2(300f, HoldfastInteriorView.FloorStandY);
            actor.SetMoveTarget(new Vector2(300f, HoldfastInteriorView.FloorStandY));
            actor.SetMoveTarget(new Vector2(430f, HoldfastInteriorView.FloorStandY));

            // Let the engine run real physics steps so gravity, floor collision,
            // and MoveAndSlide actually execute (manual pumping is a no-op).
            for (int i = 0; i < 180; i++)
                await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

            float dx = Mathf.Abs(actor.Position.X - 430f);
            bool arrived = dx < 12f;
            if (arrived) pass++; else fail++;

            bool grounded = actor.IsOnFloor() && Mathf.Abs(actor.Position.Y - HoldfastInteriorView.FloorStandY) < 10f;
            if (grounded) pass++; else fail++;

            bool animated = actor.Body.Texture != null && actor.Body.Frame >= 0;
            if (animated) pass++; else fail++;

            GD.Print($"[ShelterPhysics] sheet={sheetLoaded} arrived={arrived} grounded={grounded} " +
                     $"x={actor.Position.X:F1} y={actor.Position.Y:F1} dx={dx:F1} frame={actor.Body.Frame} onFloor={actor.IsOnFloor()}");
            HostCli.EmitSummary("shelter_physics", fail == 0, fail == 0 ? 0 : 1);
            QuitUiTestAfterFrame(fail == 0 ? 0 : 1);
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
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss'Z'", CultureInfo.InvariantCulture) + // DETERMINISM_ALLOWLIST: Host diagnostics banner timestamp
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

            // Ticket #127: Replace misleading "connected" metric with accurate
            // utilization-aware status. File enumeration only proves discovery.
            // The utilization graph is generated separately via --content-utilization-selftest.
            if (_statusLabel != null)
                _statusLabel.Text = $"Archive: {jsonCount} JSON catalogs discovered.\n" +
                    "Run --content-utilization-selftest for utilization analysis.";
            if (_codexViewer != null)
                _codexViewer.Text = summary.ToString();
        }

        // -----------------------------------------------------------------
        // Game flow: Menu → Playing → GameOver
        // -----------------------------------------------------------------

        private void StartNewGame()
        {
            StartNewGame(
                Ashfall.Core.Survivors.StartingCohortCatalog.StandardProfileId,
                _cliStartingSuppliesProfileId ??
                StartingSuppliesCatalog.StandardProfileId,
                DefaultDifficultyPresetId());
        }

        private void StartNewGame(string profileId)
        {
            StartNewGame(
                profileId,
                StartingSuppliesCatalog.StandardProfileId,
                DefaultDifficultyPresetId());
        }

        private void StartNewGame(string cohortProfileId, string startingSuppliesProfileId)
        {
            StartNewGame(
                cohortProfileId,
                startingSuppliesProfileId,
                DefaultDifficultyPresetId());
        }

        private void StartNewGame(
            string cohortProfileId,
            string startingSuppliesProfileId,
            string difficultyPresetId)
        {
            // Validate the submitted stable ID before allocating a new slot so
            // a malformed selection cannot leave an empty campaign behind.
            string resolvedDifficultyPresetId;
            try
            {
                resolvedDifficultyPresetId = ResolveDifficultyPresetId(difficultyPresetId);
            }
            catch (Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] New Game aborted: " + ex.Message);
                if (_statusLabel != null) _statusLabel.Text = "Unable to select the requested difficulty.";
                return;
            }

            // Fresh campaigns are transactions, not resets of the currently
            // selected campaign. Allocate the next deterministic slot before
            // tearing down live sessions so an existing campaign remains
            // loadable if allocation fails.
            if (_saveLoadHost != null &&
                !_saveLoadHost.TryCreateFreshCampaignSlot(out _))
            {
                GD.PrintErr("[Ashfall Godot] New Game aborted: no fresh campaign slot could be allocated.");
                if (_statusLabel != null)
                    _statusLabel.Text = "Unable to allocate a fresh campaign slot.";
                return;
            }

            _saveLoadHost?.UpdateManifest(m =>
            {
                m.manifestVersion = SaveManifest.CurrentManifestVersion;
                m.difficultyPresetId = _difficultyPresetId;
            });

            _state = GameState.Playing;
            _mainMenu.Visible = false;
            _gameOver.Visible = false;
            _gameUiContainer.Visible = false;
            _dashboard.Visible = true;
            CloseAllOverlayPanels();

            _audio?.StopMusic();
            _audio?.PlayGameplayMusic();
            _audio?.StartBunkerAmbience();

            // A new game must not inherit the previous run's in-memory sessions.
            // Reset only memory: the newly allocated slot is empty and all
            // existing campaign roots remain untouched on disk.
            ResetAllSessionsInMemory();
            PrepareDifficultyForNewCampaign();
            _campaignInitializationMode = CampaignInitializationMode.FreshInitialize;
            _startingCohortProfileId = string.IsNullOrEmpty(cohortProfileId)
                ? Ashfall.Core.Survivors.StartingCohortCatalog.StandardProfileId
                : cohortProfileId;
            _startingSuppliesProfileId = string.IsNullOrEmpty(startingSuppliesProfileId)
                ? StartingSuppliesCatalog.StandardProfileId
                : startingSuppliesProfileId;
            SelectDifficultyForNewCampaign(resolvedDifficultyPresetId);

            // Compose all campaign-owned services before any panel opens.
            ComposeCampaign();
            GrantDifficultyStartingBonusesOnce();

            // Plan 174 — assign a deterministic procedural origin to every
            // starting survivor so the survivor-inspection panel shows a real
            // backstory from day 1 (previously the catalog was loaded but no
            // survivor was ever assigned an origin).
            AssignMissingBackstories();

            // Plan 169 — bind audio accessibility (catalog, ducking, presets) and
            // the day-fact -> critical-cue bridge.
            SetupAudioAccessibility();

            // Plan 140 — Apply generational legacy starting context to New Game
            var legacyContext = PrepareStartingCampaignContext();
            if (legacyContext != null)
            {
                if (legacyContext.factionModifiers != null && _yearOfAsh?.FactionWar != null)
                {
                    foreach (var kvp in legacyContext.factionModifiers)
                    {
                        _yearOfAsh.FactionWar.ModifyStanding(kvp.Key, (int)kvp.Value);
                    }
                }
                if (legacyContext.inheritedTraits != null && legacyContext.inheritedTraits.Count > 0 && _journal != null)
                {
                    var traitNames = legacyContext.inheritedTraits.Select(t => t.name);
                    _journal.TryAddRawEntry(
                        $"legacy_traits_{_simDay}",
                        $"Inherited {legacyContext.inheritedTraits.Count} generational legacy trait(s): {string.Join(", ", traitNames)}.",
                        null!,
                        _simDay);
                }
            }

            _openingProtocolModal.Bind(_startingLevel);
            // Veteran mode (TutorialMode 2): land on the clean game view instead
            // of forcing the protocol modal. It stays openable via its registry
            // route and bind action (see RegisterPlayerSurfaces).
            if (AtomicWar.GodotApp.Settings.UserSettingsStore.Current.TutorialMode != 2)
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
            _audio?.PlayMainMenuMusic();

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

            if (!descriptor.IsPlayerNavigable)
            {
                string msg = $"[PanelRegistry] PROTOTYPE ROUTE: '{panelId}' is a shelved prototype and not player-navigable.";
                GD.PrintErr(msg);
                if (_statusLabel != null)
                    _statusLabel.Text = msg;
                return;
            }

            if (_state == GameState.Menu && !descriptor.AvailableInMenu)
            {
                string msg = "[PanelRegistry] BLOCKED ROUTE: '" + panelId + "' is not accessible from the main menu.";
                GD.PrintErr(msg);
                if (_statusLabel != null)
                    _statusLabel.Text = msg;
                return;
            }

            if (!descriptor.IsPlayerNavigable)
            {
                string msg = "[PanelRegistry] PROTOTYPE ROUTE: '" + panelId + "' is a shelved prototype and not player-navigable.";
                GD.PrintErr(msg);
                if (_statusLabel != null)
                    _statusLabel.Text = msg;
                return;
            }

            CloseAllOverlayPanels();
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.UiConfirm);

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
                case "guidance":
                    SetupOnboarding();
                    EnsureOnboardingPanel();
                    _onboardingHintPanel?.Show();
                    break;
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
                    _sharedResearch = EnsureSharedResearch();
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
                    SetupEconomy();
                    _economyDetailPanel.Bind(_economy, () => _world?.Weather?.Current ?? Ashfall.Core.WeatherKind.Clear);
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
                    ObserveSigil("store.opened");
                    break;
                case "crafting":
                    SetupCrafting();
                    SetupInventory();
                    SetupSurvivors();
                    SetupPhase0();
                    SyncCraftingStationsFromShelter();
                    _craftingPanel.Bind(_crafting, _inventory, _survivors,
                        _phase0?.TradeSpecialty, sid => ResolveSurvivorProfessionId(sid));
                    _craftingPanel.Open();
                    break;
                case "medical":
                    SetupJournal();
                    DiscoverBureaucraticDocuments("medical_office");
                    SetupSurvivors();
                    SetupInventory();
                    SetupMedical();
                    SetupPhase0();
                    EnsureMedicalPipeline();
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
                    _expeditionPanel.Bind(_expeditions, _survivors, _inventory, _equipmentCondition?.System);
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
                    _radioPanel.BindProduction(EnsureRadioProgramProductionSession());
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
                    SetupJournal();
                    DiscoverBureaucraticDocuments("shelter_records");
                    SetupSurvivors();
                    SetupWorld();
                    SetupInventory();
                    int shelterDay = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
                    _shelterPanel.Bind(_survivors, _world, _inventory, GetShelterRoomIdentityCatalog(), GetBunkerGraffitiCatalog(), shelterDay);
                    _shelterPanel.SetMachineTellCatalog(GetMachineTellCatalog());
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
                case "faction_culture_codex":
                    SetupMuster();
                    _factionCultureCodexPanel.Bind(_muster);
                    _factionCultureCodexPanel.Open();
                    break;
                case "quests":
                    SetupHoldfastRuntime();
                    SetupExpansions();
                    SetupDutyRoster();
                    SetupFactionBranch();
                    SetupMoralChoice();
                    BindQuestsPanel();
                    _questsPanel.Open();
                    break;
                case "moral_choice":
                    SetupMoralChoice();
                    OpenMoralChoiceModal(null);
                    break;
                case "narrative_arc":
                    OpenNarrativeArcModal();
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
                    _silentFoundryPanel.SetMachineTellCatalog(GetMachineTellCatalog());
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
                    _epiloguePanel.Bind(BuildCurrentEpilogueContext());
                    _epiloguePanel.Open();
                    break;
                case "chronicle":
                    OpenChroniclePanel();
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
                    SetupJournal();
                    DiscoverBureaucraticDocuments("duty_roster");
                    SetupDutyRoster();
                    SetupSurvivors();
                    _dutyRosterPanel.Bind(_dutyRoster, _survivors);
                    _dutyRosterPanel.OnAssignmentChanged += () => ObserveSigil("duty.assigned");
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
                case "shelter_decor":
                case "autopsy_report":
                case "waystation_network":
                case "chemical_dependency":
                case "sump_flooding":
                case "decontamination":
                case "low_background_metrology":
                case "insar_mapping":
                case "hydraulic_extrusion":
                case "runflat_tire":
                case "sofc_power":
                case "sound_ranging":
                case "cvd_diamond":
                case "amphibious_draisine":
                case "sanitation":
                case "black_market":
                case "companion_kennel":
                case "beliefs_panel":
                case "anomaly_watch":
                case "cybernetics":
                case "kitchen_nutrition":
                case "equipment_condition":
                case "library_study":
                case "archive_desk":
                case "contractor_roster":
                case "mental_health_crisis":
                case "phantom_memory":
                case "traveling_caravan":
                case "shelter_barter":
                case "medical_ward":
                case "sky_defense_battery":
                case "dynamic_quests":
                case "vehicle_garage":
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

            // Run finalization (Task 121): seal the active slot as terminal.
            // The full campaign envelope stays on disk as an inspectable
            // memorial/archive — the final state is preserved — but the
            // manifest is marked TerminalLoss, which blocks SelectSlot,
            // DeleteSlot, and TryLoadAggregate, so a completed campaign cannot
            // resurrect through a stale aggregate save. This replaces the old
            // selective deletion of three Holdfast files (which abandoned the
            // other 58 sections and still left the slot continuable).
            FinalizeTerminalRun();
        }

        /// <summary>
        /// Seal the active save slot as terminal after a game-over / victory.
        /// Keeps the envelope as the memorial archive; blocks continuation.
        /// </summary>
        private void FinalizeTerminalRun()
        {
            if (_saveLoadHost == null) return;
            int finalDay = _holdfastRuntime != null ? _holdfastRuntime.Day : _simDay;
            _saveLoadHost.MarkActiveSlotTerminal(finalDay);
            UpdateContinueButton();
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

            // Plan 140 — evaluate critical survival transitions
            if (_feedbackService != null)
            {
                if (_feedbackService.Deduplicator.EvaluateTransition("health_injury_critical", _holdfastRuntime.Health <= 25))
                {
                    _feedbackService.Emit(new Ashfall.Core.Feedback.FeedbackEvent("injury_critical", new object[] { "Dr. Sarah Chen" }, category: "health_warning", dedupeKey: "health_injury_critical"));
                }
                if (_feedbackService.Deduplicator.EvaluateTransition("survival_food_critical", _holdfastRuntime.Hunger >= 90))
                {
                    _feedbackService.Emit(new Ashfall.Core.Feedback.FeedbackEvent("food_critical", new object[] { _holdfastRuntime.Hunger }, category: "resource_warning", dedupeKey: "survival_food_critical"));
                }
                else if (_feedbackService.Deduplicator.EvaluateTransition("survival_food_low", _holdfastRuntime.Hunger >= 70 && _holdfastRuntime.Hunger < 90))
                {
                    _feedbackService.Emit(new Ashfall.Core.Feedback.FeedbackEvent("food_low", new object[] { 100 - _holdfastRuntime.Hunger }, category: "resource_warning", dedupeKey: "survival_food_low"));
                }
                if (_feedbackService.Deduplicator.EvaluateTransition("survival_dehydration_imminent", _holdfastRuntime.Thirst >= 90))
                {
                    _feedbackService.Emit(new Ashfall.Core.Feedback.FeedbackEvent("dehydration_imminent", new object[] { "Dr. Sarah Chen" }, category: "health_warning", dedupeKey: "survival_dehydration_imminent"));
                }
                else if (_feedbackService.Deduplicator.EvaluateTransition("survival_water_low", _holdfastRuntime.Thirst >= 70 && _holdfastRuntime.Thirst < 90))
                {
                    _feedbackService.Emit(new Ashfall.Core.Feedback.FeedbackEvent("water_low", new object[] { 100 - _holdfastRuntime.Thirst }, category: "resource_warning", dedupeKey: "survival_water_low"));
                }
                if (_feedbackService.Deduplicator.EvaluateTransition("radiation_high_rate", _holdfastRuntime.Radiation >= 50f))
                {
                    _feedbackService.Emit(new Ashfall.Core.Feedback.FeedbackEvent("radiation_high", new object[] { (int)_holdfastRuntime.Radiation }, category: "health_warning", dedupeKey: "radiation_high_rate"));
                }
            }

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
                + stores.CountById("item_air_filter_hepa")
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
                FilterSpares = _startingLevel?.HasMaintenanceDependencies == true
                    ? filterSpares
                    : _startingLevel?.System.State.filterSparesCount ?? filterSpares,
                MechanicalScrap = _startingLevel?.HasMaintenanceDependencies == true
                    ? stores.CountById("scrap_mechanical")
                    : _startingLevel?.System.State.mechanicalScrapCount ?? 6,
                AirFilterHealth = _startingLevel?.System.State.airFilterHealthPercent ?? 100.0f,
                AirQuality = _startingLevel?.System.State.airQualityPercent ?? 100.0f,
                RadonLevel = _startingLevel?.System.State.radonLevelBqm3 ?? 12.0f,
                AirWarning = _startingLevel?.System.State.airHazardWarning ?? false,
                FilterDutyAssignee = intakeAssignee,
                Forecast = _world.Weather.PeekForecast(3),
                LastEvent = lastEvent,
                MachineTellText = BuildMachineTellText(),
                MemorialCount = _memorial?.Entries?.Count ?? 0,
                CohortLivingCount = _doseLedger?.Cohort?.SurvivingChildrenCount ?? 0
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
                _geothermalOrcPanel, _ballisticsWorkbenchPanel, _aeroponicsPanel,
                _pneumaticDispatchPanel,
                _doseGeographyPanel,
                _dailyBriefingModal, _narrativeArcModal,
                _chemWarfareDefensePanel, _commsArrayTransceiverPanel,
                _ceremonyFestivalPanel, _roboticsWorkshopPanel,
                _survivorDowntimePanel, _winterFreezePanel,
                _amputationTriagePanel, _justiceTribunalPanel,
                _railwayTerminalPanel, _archaeologyExcavationPanel,
                _desperationCrisisPanel, _mercenaryBountyBoardPanel,
                _falloutPlumePanel
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


        private void OnStartGameClicked()
        {
            SetupIceRoad();
            _core.UnlockAndClerk();
            SetupCampaignDay();
            // Calendar-led: a new campaign starts at the calendar's day; the
            // holdfast clock follows.
            _core.Clock.SetDay(_campaignDay.Calendar.CurrentDay);
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
