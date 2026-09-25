// SPDX-License-Identifier: MIT
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
using Ashfall.Core.Save;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using Ashfall.Core.Feedback;
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
        private static readonly string[] AllSaveSections = SaveSectionRegistry.SectionKeys.ToArray();

        /// <summary>
        /// In-memory campaign section payloads for the envelope-primary save.
        /// Each SaveXxx captures its section's persisted bytes here (instead
        /// of writing a section file); SaveAll packs them into ONE atomic
        /// campaign.json write. Keys are SaveSectionRegistry section keys.
        /// </summary>
        private readonly Dictionary<string, string> _sectionPayloads = new();

        /// <summary>
        /// Set when any section capture failed during the current SaveAll;
        /// the envelope write is then aborted so a partially captured
        /// generation can never be presented as a coherent snapshot.
        /// </summary>
        private bool _sectionCaptureFailed;

        /// <summary>
        /// Record one section's captured payload. Returns true when the
        /// payload is usable (callers clear their dirty flag); an empty
        /// payload marks the capture as failed and aborts the save.
        /// </summary>
        internal bool CaptureSection(string sectionKey, string payload)
        {
            if (string.IsNullOrWhiteSpace(sectionKey))
            {
                _sectionCaptureFailed = true;
                GD.PrintErr("[Ashfall Godot] Capture rejected: section key is empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(payload))
            {
                // Never allow a failed capture to leave an older generation's
                // bytes eligible for a later aggregate write.
                _sectionPayloads.Remove(sectionKey);
                _sectionCaptureFailed = true;
                GD.PrintErr($"[Ashfall Godot] Section '{sectionKey}' captured empty — save will be aborted.");
                return false;
            }

            _sectionPayloads[sectionKey] = payload;
            return true;
        }

        /// <summary>
        /// Flushes dirty save stores that were marked during the in-flight day
        /// advance. Invoked by the campaign-day coordinator before the briefing
        /// modal opens so a crash mid-modal does not lose the day's mutations.
        /// With the envelope-primary save this performs a full (silent) save:
        /// every capture is in-memory and the single envelope write is cheaper
        /// than the old per-file flushes.
        /// </summary>
        internal void FlushDirtyStoresForDayAdvance()
        {
            SaveAll(playCue: false);
        }

        /// <summary>
        /// Legacy helper retained for composition-root tests. Production New
        /// Game allocates a new slot and does not delete prior campaigns.
        /// </summary>
        private void ResetAllSessions()
        {
            ResetAllSessionsInMemory();
            DeleteGlobalSavesOnDisk();
            GD.Print("[Ashfall Godot] Sessions reset; persisted campaign slots preserved.");
        }

        /// <summary>
        /// Attempts to load and restore game state from a slot.
        /// If the save is missing, corrupt, or checksum-invalid, reports the error
        /// to the caller/UI and leaves live session state completely intact.
        /// </summary>
        public bool TryLoadAndRestoreGame(SaveSlotId slotId, out string message)
        {
            if (_saveLoadHost == null)
            {
                message = "Save/load host service is not initialized.";
                return false;
            }

            bool loaded = _saveLoadHost.TryLoadSlot(
                slotId,
                out var result,
                ValidateDifficultyEnvelope);
            message = result.UserMessage;
            if (!loaded || !result.IsSuccess)
            {
                GD.PrintErr($"[Ashfall Godot] Restore aborted for slot '{slotId}': {message}");
                RecordSessionLoadFailure(slotId.Value, message);
                FeedbackMessages.Emit(new FeedbackEvent(
                    key: "load_failed",
                    category: "system",
                    dedupeKey: "load_failed"
                ));
                return false;
            }

            ApplyDifficultyFromLoadedManifest();

            // The slot root now points at the requested campaign. Dispose all
            // live session instances before setup so guarded SetupXxx methods
            // cannot retain state, event subscriptions, or panels from the
            // previously active slot.
            _campaignInitializationMode = CampaignInitializationMode.Restore;
            ResetAllSessionsInMemory();

            _state = GameState.Playing;
            if (_mainMenu != null) _mainMenu.Visible = false;
            if (_gameOver != null) _gameOver.Visible = false;
            if (_gameUiContainer != null) _gameUiContainer.Visible = false;
            if (_dashboard != null) _dashboard.Visible = true;
            CloseAllOverlayPanels();

            RestoreAllSubsystemsFromDisk();

            if (_statusLabel != null)
                _statusLabel.Text = message;

            FeedbackMessages.Emit(new FeedbackEvent(
                key: "load_success",
                category: "system",
                dedupeKey: "load_success"
            ));

            return true;
        }

        private void RestoreAllSubsystemsFromDisk()
        {
            // campaign_day is the campaign header authority. Restore it
            // before dependent sessions so immutable campaign selections
            // (including difficulty) are available throughout composition.
            SetupCampaignDay();
            SetupHoldfastRuntime();
            _holdfastTerminal?.OpenTerminal();

            ExecuteSubsystemManifestBootstrap();

            SetupStartingLevel();
            SetupSurvivors();
            SetupInventory();
            SetupMedical();
            SetupMedicalWard();
            SetupDifficulty();
            SetupWorld();
            SetupRadio();
            SetupMoraleContagion();
            SetupPathogenStrains();
            SetupSubterranean();
            SetupPsyOps();
            SetupRadioProgramProduction();
            SetupLowBackgroundMetrology();
            SetupInSarMapping();
            SetupHydraulicExtrusion();
            SetupRunFlatTire();
            SetupSofcPower();
            SetupCvdDiamond();
            SetupSoundRanging();
            SetupAmphibiousDraisine();
            SetupPiezometer();
            SetupCrafting();
            SetupCaravans();
            SetupExpeditions();
            SetupCombat();
            SetupNarrative(reloadEventAdapter: true);
            SetupEchoes();
            SetupEconomy();
            SetupSanitation();
            SetupDeepWell();
            SetupWaterCondenser();
            SetupBlackMarket();
            SetupUtilityAi();
            SetupDutyRoster();
            SetupVerdict();
            SetupMaritime();
            SetupPhantom();
            SetupPhase0();
            EnsureMedicalPipeline();
            SetupDoseLedger();
            SetupMuster();
            SetupYearOfAsh();
            SetupExpansions();
            SetupExpansionQuests();
            SetupThirdonary();
            SetupGreenhouse();
            SetupPowerGrid();
            ComposePlans74To77();
            SetupSilentFoundry();
            SetupDisease();
            SetupEncounterChoiceResolver();
            SetupTravelEncounters();
            SetupSurvivorSocial();
            SetupMemorial();
            SetupSurvivorFate();
            SetupSpiritual();
            SetupExpandedShelterSystems();
            SetupPlans166To169();
            SetupFactionBranch();
            SetupOnboarding();
            SetupEcologicalInfestation();
            SetupFieldGuide();
            SetupWorkshop();
            SetupRadioStation();
            SetupShelterSocial();
            SetupExcavationHazards();
            SetupDynamicQuests();
            SetupPlans50To53();
            SetupConsequenceLedger();
            // ── Plans 178-201: expansion systems (Ensure* restores persisted state) ──
            SetupGenerational();
            SetupPrisoners();
            SetupMutations();
            SetupStealth();
            SetupAviation();
            SetupForcedLabor();
            SetupNarcotics();
            SetupPolitics();
            // ── Flagship institutions (Tasks 5-8) ──
            SetupFlagshipInstitutions();
            SetupAnomalyHazard();
            SetupCompanionAnimals();
            SetupBionics();
            SetupZealotry();
            SetupFallout();
            SetupDesperation();
            SetupMercenary();
            SetupArchaeology();
            SetupAmputation();
            SetupRailway();
            SetupFungi();
            SetupContrabandStash();
            SetupShelterBarter();
            SetupBlackProjectsArchive();
            SetupTechnicalMaterialArchive();
            SetupOralLore();
            SetupHydroGeologyDiscovery();
            SetupGrainMillingArchive();
            SetupLeatherworkArchive();
            SetupJustice();
            SetupRecreation();
            SetupChemWarfare();
            SetupCommsArray();
            SetupCeremony();
            SetupRobotics();
            SetupBioFermentation();
            // ── Previously incomplete SaveStores — Setup restores persisted state ──
            SetupEndgame();
            SetupCaravanTrade();
            SetupSurgicalWard();
            SetupPowerSubgrids();
            SetupPerimeterDefense();
            SetupHydroponicBiomes();
            SetupAgriculture();
            SetupDefense();
            SetupPsychologyArcs();
            SetupWildlifeEcosystem();
            SetupNuclearCore();
            SetupArmoredCrawlers();
            SetupPersonalQuests();
            SetupNarrativeQuestlines();
            SetupChemicalSynthesis();
            SetupCollectibles();
            SetupShelterFireHazard();
            // ── Plans 55 + 58 + 135 — retention audit, authored outpost
            //    network, weather→gameplay cascade ──
            SetupRetention();
            SetupOutpostSettlement();
            SetupWeatherCascade();
            SetupTerritoryControl();
            SetupCooking();
            SetupCampaignLegacy();
            SetupResearchUnlockBridge();
            SetupUnifiedEnding();
            SetupNpcMemory();
            SetupIdeologicalFriction();
            SetupRomanceFamily();
            SetupVehicleCustomization();
            SetupBackstory();
            SetupMetaProgression();
            SetupTradeRoutes();
            SetupHumanMigration();
            SetupShelterIdentity();
            SetupDynamicQuestGeneration();
            SetupShelterGovernance();
            SetupAging();
            SetupShelterMaintenance();
            SetupSurvivorRoutines();
            SetupDifficultySettings();
            SetupRailTrackMaintenance();
            SetupGlassworks();
            SetupBroadsheetPress();
            SetupKilnworks();
            SetupWildlifeHarvest();
            SetupStormForecast();
            SetupDependencyTaperWithdrawal();
            SetupAntenatalMaternalHealth();
            SetupClinicalWardTriage();
            SetupChemicalReagentSynthesis();
            SetupMechanicalDriveline();
            SetupSleepAcousticRest();
            SetupShelterArchive();
            SetupSurvivorDreams();
            SetupAccessibilitySettings();
            SetupMemoryDecay();
            SetupInterpersonalConflict();
            SetupExercise();
            SetupCultureCreation();
            SetupPsychologicalProfiles();
            SetupSkillCertifications();
            SetupBestiary();
            SetupHealthHistory();
            SetupChildDevelopment();
            // Moral ledger is reset by ResetEnrolledFlagshipSessions; re-Setup

            // before any early SaveAll so Continue cannot drop resolved choices.
            SetupMoralChoice();

            BindDifficultyConsumers();

            UpdateHud();
        }

        /// <summary>
        /// Restore every persisted subsystem and rebuild player-facing UI so a continued
        /// campaign presents the same state that was saved — no silent resets, no fresh-state seeding.
        /// </summary>
        private void ContinueGame()
        {
            if (_saveLoadHost?.ActiveSlotId != null)
            {
                if (TryLoadAndRestoreGame(_saveLoadHost.ActiveSlotId.Value, out string msg))
                    return;
                GD.PrintErr($"[Ashfall Godot] ContinueGame failed: {msg}");
            }

            _state = GameState.Playing;
            _mainMenu.Visible = false;
            _gameOver.Visible = false;
            _gameUiContainer.Visible = false;
            _dashboard.Visible = true;
            CloseAllOverlayPanels();

            // No active slot: migrate pre-slot global section files (if any)
            // into a fresh envelope-backed slot and load that. Payloads are
            // the legacy file bytes verbatim; originals stay untouched.
            var migrated = _saveLoadHost?.MigrateLegacyGlobalSaves(ProjectSettings.GlobalizePath("user://"));
            if (migrated != null)
            {
                if (TryLoadAndRestoreGame(migrated.Value, out string migrateMsg))
                    return;
                GD.PrintErr($"[Ashfall Godot] Legacy migration load failed: {migrateMsg}");
            }

            RestoreAllSubsystemsFromDisk();

            _statusLabel.Text = "Save loaded. The ledger continues.";
        }

        private void SaveAll() => SaveAll(playCue: true);

        private bool SaveAll(bool playCue)
        {
            // Every invocation is a new capture generation. Never let a
            // previous campaign, slot, or failed attempt leak into this one.
            _sectionPayloads.Clear();
            _sectionCaptureFailed = false;

            try
            {
                SaveJournal();
                SaveMoralChoice();
                SaveCounterIntelligence();
                SaveHoldfast();
                SaveHoldfastRuntime();
                SaveDutyRoster();
                SaveExpansionHub();
                SaveExpansionQuests();
                SaveThirdonary();
                SavePhantomMemory();
                SaveDoseLedger();
                SaveMuster();
                SaveInventory();
                SaveSurvivors();
                SaveEconomy();
                SaveSanitation();
                SaveDeepWell();
                SaveWaterCondenser();
                SaveBlackMarket();
                SaveVerdict();
                SaveMaritime();
                SaveExpeditions();
                SaveReconTelemetry();
                SaveCombat();
                SaveNarrative();
                SaveEchoes();
                SaveEventAdapter();
                SaveMedical();
                SaveMedicalPipeline();
                SaveWorld();
                SaveCrafting();
                SaveCaravans();
                SaveYearOfAsh();
                SavePhase0();
                SaveStartingLevel();
                SaveGreenhouse();
                SaveGeothermalOrc();
                SaveBallisticsWorkbench();
                SaveAeroponics();
                SavePneumaticDispatch();
                SaveRadio();
                SaveMoraleContagion();
                SavePathogenStrains();
                SaveSubterranean();
                SavePsyOps();
                SaveRadioProgramProduction();
                SaveLowBackgroundMetrology();
                SaveInSarMapping();
                SaveHydraulicExtrusion();
                SaveRunFlatTire();
                SaveSofcPower();
                SaveCvdDiamond();
                SaveSoundRanging();
                SaveAmphibiousDraisine();
                SavePiezometer();
                SaveDailyBriefing();
                SavePowerGrid();
                SaveGeothermalAquifer();
                SaveMedicalWard();
                SaveMemorial();
                SaveOnboarding();
                SaveEcologicalInfestation();
                SaveFieldGuide();
                SaveWorkshop();
                SaveRadioStation();
                SaveShelterSocial();
                SaveExcavationHazards();
                SaveDynamicQuests();
                SaveVehicleGarage();
                SaveShelterEspionage();
                SaveSurvivorMentalHealth();
                // ── Plans B68/B69 — seismic monitoring & cryo vault ──
                SaveSeismicDynamics();
                SaveCryoVault();
                // ── Plan B89 — precision metrology ──
                SavePrecisionMetrology();
                // ── Plan B87 — closed-loop aquaponics ──
                SaveAquaponics();
                // ── Audit-PR triad repairs ───────────────────────────────
                SaveSilentFoundry();
                SaveDisease();
                SaveWastelandMap();
                SaveEncounterChoice();
                SaveTravelEncounters();
                // ─────────────────────────────────────────────────────────
                SaveAllExpandedShelterSystems();
                SaveEspionage();
                SaveFluidLogistics();
                SaveProceduralNarrative();
                SaveFoodPreservation();
                SavePrewarArchives();
                SaveSurvivorSocial();
                SaveSurvivorFate();
                SaveCampaignDay();
                // ── Plans 178-201: expansion systems (null-guarded; uncreated systems omit their section) ──
                SaveGenerational();
                SavePrisoners();
                SaveMutations();
                SaveStealth();
                SaveAviation();
                SaveForcedLabor();
                SaveNarcotics();
                SavePolitics();
                SaveCulturalArchive();
                SaveDiplomaticSummit();
                SaveSkyDefense();
                SaveSanatorium();
                SaveAnomalyHazard();
                SaveCompanionAnimals();
                SaveBionics();
                SaveZealotry();
                SaveSpiritual();
                SaveFallout();
                SaveDesperation();
                SaveMercenary();
                SaveArchaeology();
                SaveAmputation();
                SaveRailway();
                SaveFungi();
                SaveContrabandStash();
                SaveShelterBarter();
                SaveBlackProjectsArchive();
                SaveTechnicalMaterialArchive();
                SaveOralLore();
                SaveHydroGeologyDiscovery();
                SaveGrainMillingArchive();
                SaveLeatherworkArchive();
                SavePlasticPyrolysis();
                SaveCargoAirdrop();
                SaveJustice();
                SaveRecreation();
                SaveChemWarfare();
                SaveCommsArray();
                SaveCeremony();
                SaveRobotics();
                SaveBioFermentation();
                // ── Previously incomplete SaveStores now enrolled in registry ──
                SaveEndgame();
                SaveCaravanTrade();
                SaveSurgicalWard();
                SavePowerSubgrids();
                SavePerimeterDefense();
                SaveHydroponicBiomes();
                SaveAgriculture();
                SaveDefense();
                SavePsychologyArcs();
                SaveWildlifeEcosystem();
                SaveNuclearCore();
                SaveArmoredCrawlers();
                SavePersonalQuests();
                SaveNarrativeQuestlines();
                SaveChemicalSynthesis();
                SaveCollectibles();
                SaveShelterFire();
                // ── Audit 2026-09-22 triad repair: registered sections that were
                //    only flushed from _Process and never captured by SaveAll ──
                SaveDeathLegacy();
                SaveRelationshipDecay();
                SaveTimeCapsules();
                // ── Plans 55 + 58 + 135 — retention audit, authored outpost
                //    network, weather→gameplay cascade ──
                SaveRetention();
                SaveOutpostSettlement();
                SaveWeatherCascade();
                SaveTerritoryControl();
                SaveCooking();
                SaveCampaignLegacy();
                SaveResearchUnlock();
                SaveUnifiedEnding();
                SaveNpcMemory();
                SaveIdeologicalFriction();
                SaveRomanceFamily();
                SaveVehicleCustomization();
                SaveBackstory();
                SaveMetaProgression();
                SaveTradeRoutes();
                SaveHumanMigration();
                SaveShelterIdentity();
                SaveShelterGovernance();
                SaveAging();
                SaveShelterMaintenance();
                SaveSurvivorRoutines();
                SaveDifficultySettings();
                SaveRailTrackMaintenance();
                SaveGlassworks();
                SaveBroadsheetPress();
                SaveKilnworks();
                SaveWildlifeHarvest();
                SaveStormForecast();
                SaveDependencyTaperWithdrawal();
                SaveAntenatalMaternalHealth();
                SaveClinicalWardTriage();
                SaveChemicalReagentSynthesis();
                SaveMechanicalDriveline();
                SaveSleepAcousticRest();
                SaveShelterArchive();
                SaveSurvivorDreams();
                SaveAccessibilitySettings();
                SaveMemoryDecay();
                SaveInterpersonalConflict();
                SaveExercise();
                SaveCultureCreation();
                SavePsychologicalProfiles();
                SaveSkillCertifications();
                SaveBestiary();
                SaveHealthHistory();
                SaveConsequenceLedger();

                if (_sectionCaptureFailed)

                {
                    GD.PrintErr("[Ashfall Godot] SaveAll aborted: one or more sections failed to capture; previous campaign envelope preserved.");
                    FeedbackMessages.Emit(new FeedbackEvent(
                        key: "save_failed",
                        category: "system",
                        dedupeKey: "save_failed"
                    ));
                    return false;
                }

                bool committed = _saveLoadHost != null && _saveLoadHost.SaveAllDirect(_sectionPayloads);
                if (!committed)
                {
                    string reason = _saveLoadHost == null
                        ? "no save/load host is wired in this context"
                        : "campaign envelope was not committed";
                    GD.PrintErr($"[Ashfall Godot] SaveAll failed: {reason}; previous envelope preserved.");
                    FeedbackMessages.Emit(new FeedbackEvent(
                        key: "save_failed",
                        category: "system",
                        dedupeKey: "save_failed"
                    ));
                    return false;
                }

                if (playCue)
                {
                    _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.SaveSuccess);
                    FeedbackMessages.Emit(new FeedbackEvent(
                        key: "save_success",
                        category: "system",
                        dedupeKey: "save_success"
                    ));
                }

                // Plan 39 — mirror the committed save into the durability audit
                // read model (slot summary + digest). The canonical slot service
                // remains the only slot authority.
                var committedSlot = _saveLoadHost?.ActiveSlotId;
                if (committedSlot != null)
                {
                    string campaignName = _saveLoadHost!.BuildSlotCard(committedSlot.Value).CampaignName;
                    RecordSessionSaveResult(
                        committedSlot.Value.Value,
                        campaignName,
                        ComputeSessionChecksum(_sectionPayloads));
                }
                return true;
            }
            finally
            {
                // The map is a transaction buffer, not a second persistence
                // authority. Drop it after both success and failure.
                _sectionPayloads.Clear();
                _sectionCaptureFailed = false;
            }
        }

        /// <summary>
        /// Captures the campaign consequence ledger into the save envelope.
        /// The ledger holds cross-quest/moral-choice flags, counters, and history
        /// that gate progression. Persisted so a reload preserves consequence state.
        /// </summary>
        private void SaveConsequenceLedger()
        {
            if (_consequenceLedger == null) return;
            var state = _consequenceLedger.CaptureState();
            if (CaptureSection("consequence_ledger", ConsequenceLedgerSaveStore.TryCapture(state)))
            {
                GD.Print($"[Ashfall Godot] Consequence ledger saved ({state.flags.Count} flags, {state.counters.Count} counters).");
            }
        }

        /// <summary>
        /// Creates the consequence ledger and restores from disk if a save exists.
        /// Called from RestoreAllSubsystemsFromDisk and the fresh-campaign path.
        /// </summary>
        private void SetupConsequenceLedger()
        {
            // _consequenceLedger is readonly and initialized inline; just restore from disk.
            var saved = ConsequenceLedgerSaveStore.TryLoad();
            if (saved != null)
            {
                _consequenceLedger.RestoreState(saved);
                GD.Print($"[Ashfall Godot] Consequence ledger restored ({saved.flags.Count} flags, {saved.counters.Count} counters).");
            }
        }

    }
}
