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
        /// Drop every session reference and clear the on-disk saves so a new game
        /// starts from a clean slate. The Godot user:// store is the only place the
        /// run history lives; deleting it is what makes Continue unavailable.
        /// </summary>
        private void ResetAllSessions()
        {
            ResetAllSessionsInMemory();
            _saveLoadHost?.ResetSlotForNewGame(new SaveSlotId("slot_1"));
            DeleteGlobalSavesOnDisk();
            GD.Print("[Ashfall Godot] New game: all sessions reset, saves cleared.");
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

            bool loaded = _saveLoadHost.TryLoadSlot(slotId, out var result);
            message = result.UserMessage;
            if (!loaded || !result.IsSuccess)
            {
                GD.PrintErr($"[Ashfall Godot] Restore aborted for slot '{slotId}': {message}");
                return false;
            }

            // The slot root now points at the requested campaign. Dispose all
            // live session instances before setup so guarded SetupXxx methods
            // cannot retain state, event subscriptions, or panels from the
            // previously active slot.
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

            return true;
        }

        private void RestoreAllSubsystemsFromDisk()
        {
            SetupHoldfastRuntime();
            _holdfastTerminal?.OpenTerminal();

            SetupStartingLevel();
            SetupSurvivors();
            SetupInventory();
            SetupMedical();
            SetupMedicalWard();
            SetupWorld();
            SetupRadio();
            SetupMoraleContagion();
            SetupCrafting();
            SetupCaravans();
            SetupExpeditions();
            SetupCombat();
            SetupNarrative(reloadEventAdapter: true);
            SetupEconomy();
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
            SetupSilentFoundry();
            SetupDisease();
            SetupEncounterChoiceResolver();
            SetupSurvivorSocial();
            SetupMemorial();
            SetupSurvivorFate();
            SetupExpandedShelterSystems();
            SetupFactionBranch();
            SetupOnboarding();
            SetupEcologicalInfestation();
            SetupFieldGuide();
            SetupWorkshop();
            SetupRadioStation();
            SetupShelterSocial();
            SetupExcavationHazards();
            // ── Plans 178-201: expansion systems (Ensure* restores persisted state) ──
            SetupGenerational();
            SetupPrisoners();
            SetupMutations();
            SetupStealth();
            SetupAviation();
            SetupForcedLabor();
            SetupNarcotics();
            SetupPolitics();
            SetupFallout();
            SetupDesperation();
            SetupMercenary();
            SetupArchaeology();
            SetupAmputation();
            SetupRailway();
            SetupFungi();
            SetupJustice();
            SetupRecreation();
            SetupChemWarfare();
            SetupCommsArray();
            SetupCeremony();
            SetupRobotics();

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
                SaveVerdict();
                SaveMaritime();
                SaveExpeditions();
                SaveCombat();
                SaveNarrative();
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
                SaveRadio();
                SaveMoraleContagion();
                SaveDailyBriefing();
                SavePowerGrid();
                SaveMedicalWard();
                SaveMemorial();
                SaveOnboarding();
                SaveEcologicalInfestation();
                SaveFieldGuide();
                SaveWorkshop();
                SaveRadioStation();
                SaveShelterSocial();
                SaveExcavationHazards();
                // ── Audit-PR triad repairs ───────────────────────────────
                SaveSilentFoundry();
                SaveDisease();
                SaveWastelandMap();
                SaveEncounterChoice();
                // ─────────────────────────────────────────────────────────
                SaveAllExpandedShelterSystems();
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
                SaveFallout();
                SaveDesperation();
                SaveMercenary();
                SaveArchaeology();
                SaveAmputation();
                SaveRailway();
                SaveFungi();
                SaveJustice();
                SaveRecreation();
                SaveChemWarfare();
                SaveCommsArray();
                SaveCeremony();
                SaveRobotics();

                if (_sectionCaptureFailed)
                {
                    GD.PrintErr("[Ashfall Godot] SaveAll aborted: one or more sections failed to capture; previous campaign envelope preserved.");
                    return false;
                }

                bool committed = _saveLoadHost != null && _saveLoadHost.SaveAllDirect(_sectionPayloads);
                if (!committed)
                {
                    string reason = _saveLoadHost == null
                        ? "no save/load host is wired in this context"
                        : "campaign envelope was not committed";
                    GD.PrintErr($"[Ashfall Godot] SaveAll failed: {reason}; previous envelope preserved.");
                    return false;
                }

                if (playCue)
                    _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.SaveSuccess);
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

    }
}
