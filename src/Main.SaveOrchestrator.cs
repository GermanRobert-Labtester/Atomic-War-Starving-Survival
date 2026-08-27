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
            if (string.IsNullOrWhiteSpace(payload))
            {
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
            _core = null!;
            _holdfastRuntime = null!;
            if (_holdfastTerminal != null && _holdfastTerminal.IsInsideTree())
                RemoveChild(_holdfastTerminal);
            _holdfastTerminal = null!;
            _dutyRoster?.Dispose();
            _dutyRoster = null!;
            _expansions?.Dispose();
            _expansions = null!;
            _phantomMemory?.Dispose();
            _phantomMemory = null!;
            _phase0?.Dispose();
            _phase0 = null!;
            _doseLedger?.Dispose();
            _doseLedger = null!;
            _inventory = null!;
            _survivors = null!;
            _economy?.Dispose();
            _economy = null!;
            _utilityAi = null!;
            _journal = null!;
            _muster?.Dispose();
            _muster = null!;
            _verdict?.Dispose();
            _verdict = null!;
            _maritime?.Dispose();
            _maritime = null!;
            if (_expeditions != null)
                _expeditions.OnEncounterSurfaced -= OnExpeditionEncounterSurfaced;
            _expeditions?.Dispose();
            _expeditions = null!;
            _combat?.Dispose();
            _combat = null!;
            _combatDirty = false;
            _narrative?.Dispose();
            _narrative = null!;
            _medical?.Dispose();
            _medical = null!;
            _world?.Dispose();
            _world = null!;
            _crafting?.Dispose();
            _crafting = null!;
            _caravans?.Dispose();
            _caravans = null!;
            _yearOfAsh = null!;
            _startingLevel?.Dispose();
            _startingLevel = null!;
            _greenhouse?.Dispose();
            _greenhouse = null!;
            _sharedResearch = null!;
            // The Year of Ash panel holds widgets bound to the old session; drop it
            // so BuildYearOfAshPanel re-creates and rebinds to the fresh session.
            if (_yearOfAshPanel != null && _rightColumn != null && _yearOfAshPanel.IsInsideTree())
                _rightColumn.RemoveChild(_yearOfAshPanel);
            _yearOfAshPanel = null!;
            _factionWarMap = null!;
            _geothermalWidget = null!;
            _radonWidget = null!;
            _radioTerminal = null!;
            _radio?.Dispose();
            _radio = null!;

            // Journal: drop the codex + book so they re-create and re-bind once;
            // keeping the book and re-binding would stack OnClosed handlers.
            if (_journalBook != null && _journalBook.IsInsideTree())
                RemoveChild(_journalBook);
            _journalBook = null!;
            _journalCodex = null!;

            _verdictDirty = false;
            _maritimeDirty = false;
            _expeditionDirty = false;
            _narrativeDirty = false;
            _hostEventAdapterDirty = false;
            _medicalDirty = false;
            _worldDirty = false;
            _craftingDirty = false;
            _caravansDirty = false;
            _phase0Dirty = false;
            _campaignDayDirty = false;
            _startingLevelDirty = false;
            _greenhouseDirty = false;

            ResetExpandedShelterSessions();

            // Registry-derived cleanup: every registered section file (and its
            // .bak) is removed from the global user:// directory so a new
            // game starts from a clean slate. Slots are untouched — they are
            // independent campaigns.
            foreach (var fileName in SaveSectionRegistry.SectionFileNames.Values)
            {
                string p = System.IO.Path.Combine(ProjectSettings.GlobalizePath("user://"), fileName);
                if (System.IO.File.Exists(p))
                    System.IO.File.Delete(p);
                string bak = p + ".bak";
                if (System.IO.File.Exists(bak))
                    System.IO.File.Delete(bak);
            }
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
            SetupWorld();
            SetupRadio();
            SetupCrafting();
            SetupCaravans();
            SetupExpeditions();
            SetupNarrative();
            SetupEconomy();
            SetupUtilityAi();
            SetupDutyRoster();
            SetupVerdict();
            SetupMaritime();
            SetupPhantom();
            SetupPhase0();
            SetupDoseLedger();
            SetupMuster();
            SetupYearOfAsh();
            SetupExpansions();
            SetupGreenhouse();
            SetupExpandedShelterSystems();

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

            // Load-from-envelope: restore directly from aggregate envelope when available.
            bool loadedFromEnvelope = _saveLoadHost?.LoadAllDirect() ?? false;
            if (!loadedFromEnvelope)
            {
                // Fallback: unpack aggregate back to individual files so
                // dependency-safe SetupXxx() calls can restore from disk as before.
                _saveLoadHost?.UnpackAggregateEnvelope();
            }

            RestoreAllSubsystemsFromDisk();

            _statusLabel.Text = "Save loaded. The ledger continues.";
        }

        /// <summary>Remove the holdfast base + trade saves (and backup) so a
        /// completed run cannot be continued into an immediate game-over loop.
        /// With the envelope-primary save the authoritative copy is the active
        /// slot's campaign envelope, so that (and its backup) goes too.</summary>
        private void ClearContinuableSaves()
        {
            if (System.IO.File.Exists(HoldfastSaveStore.SavePath))
                System.IO.File.Delete(HoldfastSaveStore.SavePath);
            if (System.IO.File.Exists(HoldfastTradeSaveStore.SavePath))
                System.IO.File.Delete(HoldfastTradeSaveStore.SavePath);
            if (System.IO.File.Exists(HoldfastTradeSaveStore.BackupPath))
                System.IO.File.Delete(HoldfastTradeSaveStore.BackupPath);

            if (_saveLoadHost?.ActiveSlotId != null)
            {
                _saveLoadHost.ClearActiveSlotEnvelope();
            }
        }

        private void SaveAll() => SaveAll(playCue: true);

        private void SaveAll(bool playCue)
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
            SaveWorld();
            SaveCrafting();
            SaveCaravans();
            SaveYearOfAsh();
            SavePhase0();
            SaveStartingLevel();
            SaveGreenhouse();
            SaveRadio();
            SaveDailyBriefing();
            SavePowerGrid();
            SaveMedicalWard();
            SaveMemorial();
            // ── Audit-PR triad repairs ───────────────────────────────────
            SaveSilentFoundry();
            SaveDisease();
            SaveWastelandMap();
            SaveEncounterChoice();
            // ─────────────────────────────────────────────────────────────
            SaveAllExpandedShelterSystems();
            SaveCampaignDay();
            if (playCue)
                _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.SaveSuccess);

            // Envelope-primary save: ONE atomic campaign.json write from the
            // in-memory payload map. A failed capture aborts here so a mixed-
            // generation snapshot can never be written; the previous envelope
            // stays intact.
            if (_sectionCaptureFailed)
            {
                _sectionCaptureFailed = false;
                GD.PrintErr("[Ashfall Godot] SaveAll aborted: one or more sections failed to capture; previous campaign envelope preserved.");
                return;
            }
            _saveLoadHost?.SaveAllDirect(_sectionPayloads);
        }

    }
}
