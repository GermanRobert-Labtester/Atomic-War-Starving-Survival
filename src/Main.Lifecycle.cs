// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core.Lifecycle;
using Ashfall.Core.Orchestration;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private SessionLifecycleRegistry _lifecycleRegistry = new SessionLifecycleRegistry();
        private bool _lifecycleRegistered;

        /// <summary>
        /// Registers all domain sessions, host adapters, and UI panels with the
        /// centralized typed lifecycle registry.
        /// </summary>
        private void RegisterLifecycleParticipants()
        {
            if (_lifecycleRegistered) return;
            _lifecycleRegistered = true;

            // Campaign header and day coordinator must be rebuilt for every
            // selected save slot. Retaining this owner across a restore would
            // skip the new slot's checksummed campaign_day state.
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "campaign_day",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "campaign_day",
                onReset: () =>
                {
                    _campaignDay = null!;
                    _dailyBriefing = null!;
                    _briefingPending = false;
                    _dailyBriefingDirty = false;
                    _campaignDayDirty = false;
                }));

            // Core & Holdfast
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "core_holdfast",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "holdfast",
                onReset: () =>
                {
                    _core = null!;
                    _holdfastRuntime = null!;
                    if (_holdfastTerminal != null && _holdfastTerminal.IsInsideTree())
                        RemoveChild(_holdfastTerminal);
                    _holdfastTerminal = null!;
                    _holdfastDirty = false;
                }));

            // Survivors & Needs
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "survivors",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "survivors",
                onReset: () =>
                {
                    _survivors = null!;
                }));

            // Inventory
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "inventory",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "inventory",
                onReset: () =>
                {
                    _inventory = null!;
                    // Collectible OnItemAdded subscription dies with the inventory
                    // instance; allow WireCollectibleInventoryFeeder to rebind.
                    _collectibleInventoryWired = false;
                }));

            // Duty Roster
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "duty_roster",
                dependsOn: new[] { "core_holdfast", "survivors" },
                saveSectionKey: "duty_roster",
                onReset: () =>
                {
                    _dutyRoster?.Dispose();
                    _dutyRoster = null!;
                    _dutyRosterDirty = false;
                }));

            // Expansions & Quests
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "expansions",
                dependsOn: new[] { "core_holdfast" },
                saveSectionKey: "expansion_hub",
                onReset: () =>
                {
                    // Debt owns event subscriptions outside the generic
                    // StatefulSessionBase reset. Detach the host bridge first,
                    // then shut down the session-owned dispatcher, so a new
                    // campaign or reload cannot retain old callbacks.
                    _debtBridge?.Detach();
                    _debtBridge = null;
                    _tradeCredit = null;
                    _debtBridgeDirty = false;
                    if (_expansions != null)
                    _expansions.DutyRoster.IsSurvivorReservedExternally = null;
                    _expansions?.ShutdownDebtIntegration();
                    _expansions?.Dispose();
                    _expansions = null!;
                    _expansionHubDirty = false;
                }));

            // Expeditions & Combat
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "expeditions",
                dependsOn: new[] { "survivors", "inventory" },
                saveSectionKey: "expedition",
                onReset: () =>
                {
                    if (_expeditions != null)
                        _expeditions.OnEncounterSurfaced -= OnExpeditionEncounterSurfaced;
                    _expeditions?.Dispose();
                    _expeditions = null!;
                    _expeditionDirty = false;
                    _travelEncounters = null;
                    _travelEncountersDirty = false;
                }));

            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "combat",
                dependsOn: new[] { "expeditions" },
                saveSectionKey: "combat",
                onReset: () =>
                {
                    HostWiringValidator.UnregisterReporter(_combat);
                    _combat?.Dispose();
                    _combat = null!;
                    _combatDirty = false;
                }));

            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "recon_telemetry",
                dependsOn: new[] { "expeditions", "wasteland_map", "radio" },
                saveSectionKey: "recon_telemetry",
                onReset: () =>
                {
                    _reconTelemetry?.Dispose();
                    _reconTelemetry = null!;
                    _reconTelemetryDirty = false;
                }));

            // Economy & Foundry
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "economy",
                dependsOn: new[] { "inventory" },
                saveSectionKey: "economy",
                onReset: () =>
                {
                    _economy?.Dispose();
                    _economy = null!;
                    _economyDirty = false;
                }));

            // World & Weather
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "world_weather",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "world",
                onReset: () =>
                {
                    _world?.Dispose();
                    _world = null!;
                    _worldDirty = false;
                    // The sonde wrapper caches the world's WeatherSystem; drop it
                    // with the world so a new campaign re-resolves a live weather
                    // session instead of displaying the previous campaign's state
                    // (INV-16.6 — session swap invalidates bindings).
                    _weatherSondeHost = null;
                }));

            // Medical & Disease
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "medical",
                dependsOn: new[] { "survivors" },
                saveSectionKey: "medical",
                onReset: () =>
                {
                    _medical?.Dispose();
                    _medical = null!;
                    _medicalDirty = false;
                }));

            // Narrative & Radio
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "narrative_radio",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "narrative",
                onReset: () =>
                {
                    _narrative?.Dispose();
                    _narrative = null!;
                    _narrativeDirty = false;
                    _hostEventAdapter?.Dispose();
                    _hostEventAdapter = null!;
                    _hostEventAdapterDirty = false;
                    _radio?.Dispose();
                    _radio = null!;
                    _radioTerminal = null!;
                }));

            // Field echoes
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "echoes",
                dependsOn: new[] { "narrative_radio", "journal" },
                saveSectionKey: "echoes",
                onReset: () =>
                {
                    _echoes?.Dispose();
                    _echoes = null;
                    _echoesDirty = false;
                }));

            // Journal
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "journal",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "journal",
                onReset: () =>
                {
                    _journal = null!;
                    if (_journalBook != null && _journalBook.IsInsideTree())
                        RemoveChild(_journalBook);
                    _journalBook = null!;
                    _journalCodex = null!;
                }));

            // Verdict
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "verdict",
                dependsOn: new[] { "core_holdfast" },
                saveSectionKey: "verdict",
                onReset: () =>
                {
                    _verdict?.Dispose();
                    _verdict = null!;
                    _verdictDirty = false;
                }));

            // Counter-Intelligence
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "counter_intelligence",
                dependsOn: new[] { "faction_branch" },
                saveSectionKey: "counter_intelligence",
                onReset: () =>
                {
                    _counterIntelligence?.Dispose();
                    _counterIntelligence = null!;
                    _counterIntelligenceDirty = false;
                }));

            // Maritime & Deep Coast
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "maritime",
                dependsOn: new[] { "core_holdfast" },
                saveSectionKey: "maritime",
                onReset: () =>
                {
                    _maritime?.Dispose();
                    _maritime = null!;
                    _maritimeDirty = false;
                }));

            // Crafting
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "crafting",
                dependsOn: new[] { "inventory" },
                saveSectionKey: "crafting",
                onReset: () =>
                {
                    _crafting?.Dispose();
                    _crafting = null!;
                    _craftingDirty = false;
                }));

            // Caravans
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "caravans",
                dependsOn: new[] { "economy" },
                saveSectionKey: "caravan",
                onReset: () =>
                {
                    _caravans?.Dispose();
                    _caravans = null!;
                    _caravansDirty = false;
                }));

            // Starting Level & Greenhouse
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "starting_level_greenhouse",
                dependsOn: new[] { "inventory" },
                saveSectionKey: "starting_level",
                onReset: () =>
                {
                    _startingLevel?.Dispose();
                    _startingLevel = null!;
                    _startingLevelDirty = false;
                    _greenhouse?.Dispose();
                    _greenhouse = null!;
                    _greenhouseDirty = false;
                    _sharedResearch = null!;
                    _researchHostSession = null;
                }));

            // Year of Ash & Muster
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "year_of_ash_muster",
                dependsOn: new[] { "core_holdfast" },
                saveSectionKey: "year_of_ash",
                onReset: () =>
                {
                    _yearOfAsh = null!;
                    _muster?.Dispose();
                    _muster = null!;
                    if (_yearOfAshPanel != null && _rightColumn != null && _yearOfAshPanel.IsInsideTree())
                        _rightColumn.RemoveChild(_yearOfAshPanel);
                    _yearOfAshPanel = null!;
                    _factionWarMap = null!;
                    _geothermalWidget = null!;
                    _radonWidget = null!;
                }));

            // Phase 0 & Dose Ledger & Phantom Memory
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "phase0_auxiliary",
                dependsOn: new[] { "core_holdfast", "survivors" },
                saveSectionKey: "phase0",
                onReset: () =>
                {
                    _phantomMemory?.Dispose();
                    _phantomMemory = null!;
                    _phase0?.Dispose();
                    _phase0 = null!;
                    _phase0Dirty = false;
                    _doseLedger?.Dispose();
                    _doseLedger = null!;
                    _utilityAi = null!;
                    _hostEventAdapterDirty = false;
                    _campaignDayDirty = false;
                }));

            // Expanded Shelter Batch
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "expanded_shelter_batch",
                dependsOn: new[] { "survivors", "inventory", "world_weather" },
                lifecycleGroup: SaveSectionRegistry.ExpandedShelterLifecycleGroup,
                onReset: ResetExpandedShelterSessions));

            // Plans 178-201 expansion systems (generational, prisoners, mutations,
            // stealth, aviation, forced labor, narcotics, politics, fallout,
            // desperation, mercenary, archaeology, amputation, railway, fungi,
            // justice, recreation, chem warfare, comms array, ceremony, robotics)
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "plans_178_201",
                dependsOn: new[] { "survivors", "inventory", "world_weather" },
                onReset: ResetPlansExpansionSessions));

            // Newly enrolled save sections (endgame, advanced shelter, personal
            // quests, chemical synthesis, collectibles, shelter fire) + moral
            // choice ledger. Must null so Setup* re-runs and restores from disk
            // after Continue / slot switch / new-game reset.
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "enrolled_flagship_sections",
                dependsOn: new[] { "survivors", "inventory", "journal" },
                onReset: ResetEnrolledFlagshipSessions));

            // Late-wave host integrations that are created lazily on their first
            // day tick or save (Plan 135 weather cascade, Plan 147 NPC memory,
            // Plan 148 ideological friction, Plan 150 romance/family,
            // Plan 152 vehicle customization, Plan 174 backstory, Plan 175 meta
            // progression). Their Setup* methods are null-guarded, so without
            // this reset a new campaign or slot switch would keep the previous
            // run's live instance and never rebuild from the new slot.
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "late_wave_integrations",
                dependsOn: new[] { "survivors", "inventory", "world_weather" },
                onReset: ResetLateWaveIntegrationSessions));

            // First-Hour Onboarding Journey (Task 120)
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "onboarding",
                dependsOn: Array.Empty<string>(),
                saveSectionKey: "onboarding",
                onReset: () =>
                {
                    _onboardingJourney = null!;
                    _onboardingDirty = false;
                    if (_onboardingHintPanel != null && _onboardingHintPanel.IsInsideTree())
                        RemoveChild(_onboardingHintPanel);
                    _onboardingHintPanel = null!;
                }));
        }

        /// <summary>
        /// Drops every Plans 178-201 expansion session instance so the next
        /// Ensure* reconstructs from disk (restored state), never from a stale
        /// pre-load instance. These systems hold no Godot resources, so no
        /// Dispose is required.
        /// </summary>
        private void ResetPlansExpansionSessions()
        {
            // Plans 178-181
            _generational = null;
            _prisoners = null;
            _mutations = null;
            _stealth = null;
            // Plans 182-185
            _aviation = null;
            _forcedLabor = null;
            _narcotics = null;
            _politics = null;
            // Plans 186-189
            _fallout = null;
            _desperation = null;
            _mercenary = null;
            _archaeology = null;
            // Plans 190-193
            _amputation = null;
            _railway = null;
            _fungi = null;
            _justice = null;
            // Plans 194-197
            _recreation = null;
            _recreationDirty = false;
            // Plans 198-201
            _chemWarfare = null;
            _commsArray = null;
            _ceremonySystem = null;
            _robotics = null;
            _chemWarfareDirty = false;
            _commsArrayDirty = false;
            _ceremonyDirty = false;
            _roboticsDirty = false;
        }

        /// <summary>
        /// Drops newly enrolled flagship sessions so the next Setup* reconstructs
        /// from disk (restored campaign envelope), never from a stale pre-load instance.
        /// </summary>
        private void ResetEnrolledFlagshipSessions()
        {
            void RemovePanel(Control? panel)
            {
                if (panel != null && panel.IsInsideTree())
                    RemoveChild(panel);
            }

            _moralChoice = null!;
            _moralChoiceDirty = false;

            _endgame = null;
            // The completion-history store must be reset WITH its authority.
            // Nulling only _endgame left the store live, so the next
            // SetupEndgame() passed its `_endgame != null` guard and executed
            // `_completionHistory = CompletionHistoryStore.Load()` — replacing
            // an already-live store and discarding whatever chronicle state it
            // held. Both are one lifecycle unit; reset both or neither.
            _completionHistory = null;
            _endgameDirty = false;

            _caravanTradeNetwork = null;
            _surgicalWard = null;
            _powerSubgrids = null;
            _perimeterDefense = null;
            _hydroponicBiomes = null;
            _nuclearCore = null;
            _armoredCrawlers = null;
            _caravanTradeNetworkDirty = false;
            _surgicalWardDirty = false;
            _powerSubgridsDirty = false;
            _perimeterDefenseDirty = false;
            _hydroponicBiomesDirty = false;
            _nuclearCoreDirty = false;
            _armoredCrawlersDirty = false;

            _personalQuests = null;
            _personalQuestsDirty = false;

            _chemicalSynthesis = null;
            _chemicalSynthesisDirty = false;

            _collectibleCatalog = null;
            _collectibleDiscovery = null;
            _uniqueClaims = null;
            _collectibleDispatcher = null;
            _collectibleInventoryWired = false;
            _collectiblesDirty = false;

            _fireIncidentPanel?.Unbind();
            _shelterFireHazard = null;
            _stageFireHazard = null;
            _shelterFireSession = null;
            _shelterFireDirty = false;

            _espionage166?.Dispose();
            _fluidLogistics168?.Dispose();
            _proceduralNarrative169?.Dispose();
            _espionage166 = null;
            _fluidLogistics168 = null;
            _proceduralNarrative169 = null;
            _espionageAgentsAway.Clear();
            _espionage166Dirty = false;
            _fluidLogistics168Dirty = false;
            _proceduralNarrative169Dirty = false;

            // CF-P28: these manifest sessions restore state at construction;
            // clear them with their panels so a slot switch cannot retain the
            // previous campaign's instances or bindings.
            _memorial = null!;
            _memorialDirty = false;

            _blackMarketPanel?.Unbind();
            RemovePanel(_blackMarketPanel);
            _blackMarketPanel = null;
            _blackMarket = null;
            _blackMarketDirty = false;

            _vehicleGaragePanel?.Unbind();
            RemovePanel(_vehicleGaragePanel);
            _vehicleGaragePanel = null;
            _vehicleGaragePanelBoundSystem = null;
            _vehicleGarage = null;
            _vehicleGarageDirty = false;

            _silentFoundry = null!;
            _sharedSkillProgression = null;
            _sharedFactionStance = null;
        }

        /// <summary>
        /// Drops the late-wave integration sessions (Plans 135/147/148/150/152/174/175)
        /// so the next Setup* reconstructs from the selected slot's disk state instead
        /// of reusing the previous campaign's live instance. These sessions hold no
        /// Godot resources, so no removal from the scene tree is required.
        /// </summary>
        private void ResetLateWaveIntegrationSessions()
        {
            ResetWeatherCascade();
            ResetNpcMemory();
            ResetIdeologicalFriction();
            ResetRomanceFamily();
            ResetVehicleCustomization();
            ResetBackstory();
            ResetMetaProgression();
            ResetAudioAccessibility();
            ResetShelterIdentity();
            ResetOriginMechanics();
            ResetDynamicQuestGeneration();
            ResetTradeRoutes();
            ResetHumanMigration();
            ResetShelterGovernance();
            ResetAging();
            ResetShelterMaintenance();
            ResetSurvivorRoutines();
            ResetDifficultySettings();
            ResetRailTrackMaintenance();
            ResetGlassworks();
        }

        /// <summary>
        /// Executes an in-memory reset of all registered sessions in safe reverse-dependency order.
        /// Does not touch persisted campaign files on disk.
        /// </summary>
        public void ResetAllSessionsInMemory()
        {
            RegisterLifecycleParticipants();
            _isRestoringSurvivorState = false;
            _survivorInitializationApplied = false;
            _sectionPayloads.Clear();
            _sectionCaptureFailed = false;
            ResetDifficultyForCampaign();
            _lifecycleRegistry.ResetAll();
            GD.Print("[Ashfall Godot] Lifecycle: all in-memory sessions reset in reverse dependency order.");
        }

        /// <summary>
        /// Deletes all legacy global save files from disk, derived from the single SaveSectionRegistry authority.
        /// </summary>
        public void DeleteGlobalSavesOnDisk()
        {
            foreach (var fileName in SaveSectionRegistry.SectionFileNames.Values)
            {
                string p = System.IO.Path.Combine(ProjectSettings.GlobalizePath("user://"), fileName);
                if (System.IO.File.Exists(p))
                    System.IO.File.Delete(p);
                string bak = p + ".bak";
                if (System.IO.File.Exists(bak))
                    System.IO.File.Delete(bak);
            }
            GD.Print("[Ashfall Godot] Lifecycle: global save files cleared from user:// storage.");
        }

        private bool _manifestSetupRegistered;

        /// <summary>
        /// Registers host setup delegates with the declarative SubsystemManifest (Plan 28C / QUEUE-PLAN28-MAIN-CONSTRUCTOR-MIGRATION).
        /// Allows universal constructor iteration and lifecycle formalization via the Core manifest.
        /// </summary>
        public void RegisterManifestSetupActions()
        {
            if (_manifestSetupRegistered) return;
            _manifestSetupRegistered = true;

            SubsystemManifest.RegisterSetupAction("journal", () => SetupJournal());
            SubsystemManifest.RegisterSetupAction("needs", () => SetupSurvivors());
            SubsystemManifest.RegisterSetupAction("inventory", () => SetupInventory());
            SubsystemManifest.RegisterSetupAction("weather", () => SetupWorld());
            SubsystemManifest.RegisterSetupAction("radiation", () => SetupDoseLedger());
            SubsystemManifest.RegisterSetupAction("radio", () => SetupRadio());
            SubsystemManifest.RegisterSetupAction("expeditions", () => SetupExpeditions());
            SubsystemManifest.RegisterSetupAction("duty_roster", () => SetupDutyRoster());
            SubsystemManifest.RegisterSetupAction("crafting", () => SetupCrafting());
            SubsystemManifest.RegisterSetupAction("research", () => EnsureSharedResearch());
            SubsystemManifest.RegisterSetupAction("medical", () => { SetupMedical(); SetupMedicalWard(); });
            SubsystemManifest.RegisterSetupAction("factions", () => SetupFactionBranch());
            SubsystemManifest.RegisterSetupAction("economy", () => SetupEconomy());
            SubsystemManifest.RegisterSetupAction("greenhouse", () => SetupGreenhouse());
            SubsystemManifest.RegisterSetupAction("shelter_defense", () => SetupSkyDefense());
            SubsystemManifest.RegisterSetupAction("vehicle_garage", () => SetupVehicleGarage());
            SubsystemManifest.RegisterSetupAction("black_market", () => SetupBlackMarket());
            SubsystemManifest.RegisterSetupAction("memorial", () => SetupMemorial());
        }

        /// <summary>
        /// Executes universal constructor iteration for subsystems declared in SubsystemManifest (Plan 28C).
        /// </summary>
        public int ExecuteSubsystemManifestBootstrap(LifecyclePhase? phase = null)
        {
            RegisterManifestSetupActions();
            return SubsystemManifest.ExecuteSetup(phase);
        }
    }
}
