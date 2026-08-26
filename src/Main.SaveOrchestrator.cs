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
        private static readonly string[] AllSaveSections = new[]
        {
            "journal",
            "holdfast",
            "holdfast_trade",
            "duty_roster",
            "expansion_hub",
            "expansion_quest",
            "thirdonary",
            "phantom_memory",
            "dose_ledger",
            "muster",
            "inventory",
            "survivors",
            "economy",
            "verdict",
            "maritime",
            "expedition",
            "combat",
            "narrative",
            "medical",
            "world",
            "crafting",
            "caravan",
            "campaign_day",
            "year_of_ash",
            "phase0",
            "starting_level",
            "greenhouse",
            "host_event",
            "radio",
            "daily_briefing",
            "power_grid",
            "medical_ward",
            "memorial",
            "silent_foundry",
            "disease",
            "wasteland_map",
            "encounter_choice",
            "water_treatment",
            "airlock_security",
            "apprenticeship",
            "caregiving",
            "autopsy",
            "chemical_dependency",
            "equipment_condition",
            "survivor_relations",
            "regional_treaty",
            "vinyl_morale",
            "wildlife_trapping",
            "excavation",
            "waystation",
            "shelter_thermal",
            "shelter_schedule",
            "sump_flooding",
            "decontamination",
            "kitchen_nutrition",
            "library_study",
            "archive_desk",
            "contractor_roster",
            "mental_health_crisis",
            "shelter_assignment"
        };

        /// <summary>
        /// Flushes dirty save stores that were marked during the in-flight day
        /// advance. Invoked by the campaign-day coordinator before the briefing
        /// modal opens so a crash mid-modal does not lose the day's mutations.
        /// </summary>
        internal void FlushDirtyStoresForDayAdvance()
        {
            if (_dailyBriefingDirty) SaveDailyBriefing();
            // The remaining save stores flush through their standard paths
            // when SaveAll runs at the tail of CommitAdvance. Anything that
            // cannot tolerate a deferred flush should set its own dirty flag
            // and call its Save*() here.
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

            foreach (var file in new[]
            {
                "holdfast_s1_save.json", "holdfast_trade_save.json", "holdfast_trade_save.json.bak",
                "duty_roster_save.json", "expansion_hub_save.json", "phantom_memory_save.json",
                "dose_ledger_save.json", "inventory_save.json", "survivors_save.json",
                "economy_save.json", "muster_save.json", "verdict_save.json",
                "maritime_save.json", "expedition_save.json", "narrative_save.json",
                "medical_save.json", "world_save.json", "crafting_save.json",
                "caravan_save.json", "campaign_day_save.json", "journal_save.json", "year_of_ash_save.json",
                "starting_level_save.json", "greenhouse_save.json", "host_event_save.json", "radio_save.json"
            })
            {
                string p = System.IO.Path.Combine(ProjectSettings.GlobalizePath("user://"), file);
                if (System.IO.File.Exists(p))
                    System.IO.File.Delete(p);
            }
            GD.Print("[Ashfall Godot] New game: all sessions reset, saves cleared.");
        }

        /// <summary>
        /// Restore every persisted subsystem and rebuild player-facing UI so a continued
        /// campaign presents the same state that was saved — no silent resets, no fresh-state seeding.
        /// </summary>
        private void ContinueGame()
        {
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

            // Restore sessions in dependency-safe order. Each SetupXxx calls its *SaveStore.TryLoad()
            // when present; if no save exists it creates clean/default state so panels never see null.
            SetupHoldfastRuntime();
            _holdfastTerminal.OpenTerminal();

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

            // Update HUD after everything is restored/bound.
            UpdateHud();

            _statusLabel.Text = "Save loaded. The ledger continues.";
        }

        /// <summary>Remove the holdfast base + trade saves (and backup) so a
        /// completed run cannot be continued into an immediate game-over loop.</summary>
        private void ClearContinuableSaves()
        {
            if (System.IO.File.Exists(HoldfastSaveStore.SavePath))
                System.IO.File.Delete(HoldfastSaveStore.SavePath);
            if (System.IO.File.Exists(HoldfastTradeSaveStore.SavePath))
                System.IO.File.Delete(HoldfastTradeSaveStore.SavePath);
            if (System.IO.File.Exists(HoldfastTradeSaveStore.BackupPath))
                System.IO.File.Delete(HoldfastTradeSaveStore.BackupPath);
        }

        private void SaveAll()
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
            _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.SaveSuccess);

            // Aggregate-first save: pack all subsystem payloads into the canonical envelope.
            _saveLoadHost?.SaveAllDirect(AllSaveSections);
        }

    }
}
