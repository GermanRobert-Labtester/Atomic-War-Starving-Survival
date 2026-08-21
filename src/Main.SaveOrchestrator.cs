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
            _dutyRoster = null!;
            _expansions = null!;
            _phantomMemory = null!;
            _phase0 = null!;
            _doseLedger = null!;
            _inventory = null!;
            _survivors = null!;
            _economy = null!;
            _utilityAi = null!;
            _journal = null!;
            _muster = null!;
            _verdict = null!;
            _maritime = null!;
            if (_expeditions != null)
                _expeditions.OnEncounterSurfaced -= OnExpeditionEncounterSurfaced;
            _expeditions = null!;
            _combat = null!;
            _combatDirty = false;
            _narrative = null!;
            _medical = null!;
            _world = null!;
            _crafting = null!;
            _caravans = null!;
            _yearOfAsh = null!;
            _startingLevel = null!;
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
            _medicalDirty = false;
            _worldDirty = false;
            _craftingDirty = false;
            _caravansDirty = false;
            _phase0Dirty = false;
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
                "caravan_save.json", "journal_save.json", "year_of_ash_save.json",
                "starting_level_save.json", "greenhouse_save.json", "radio_save.json"
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
            SaveHoldfast();
            SaveHoldfastRuntime();
            SaveDutyRoster();
            SaveExpansionHub();
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
            _audio?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.SaveSuccess);
        }

    }
}
