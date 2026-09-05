using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core.Lifecycle;
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
                    _combat?.Dispose();
                    _combat = null!;
                    _combatDirty = false;
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
            _moralChoice = null!;
            _moralChoiceDirty = false;

            _endgame = null;
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
        }

        /// <summary>
        /// Executes an in-memory reset of all registered sessions in safe reverse-dependency order.
        /// Does not touch persisted campaign files on disk.
        /// </summary>
        public void ResetAllSessionsInMemory()
        {
            RegisterLifecycleParticipants();
            _sectionPayloads.Clear();
            _sectionCaptureFailed = false;
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
    }
}
