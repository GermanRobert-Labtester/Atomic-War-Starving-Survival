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
        // ── Narrative fields (GAP-ARCH-01 Phase 1) ──
        private NarrativeHostSession _narrative = null!;
        private bool _narrativeDirty;
        private RadioHostSession _radio = null!;
        private CraftingHostSession _crafting = null!;
        private bool _craftingDirty;
        private JournalSystem _journal = null!;

        private void SetupJournal()
        {
            if (_journal != null) return;

            var catalogs = CatalogJsonLoader.Load(_dataDir);
            _journal = new JournalSystem();
            // Mark dirty rather than writing the whole save file per entry; the
            // _Process tick flushes it. Seeding adds many entries in one frame and
            // used to rewrite journal_save.json once for each of them.
            _journal.OnEntryAdded += _ => _journalDirty = true;
            _journal.OnTabChanged += _ => _journalDirty = true;

            _journalCodex = new JournalCodex(_journal, catalogs);

            if (_journalBook == null || !_journalBook.IsInsideTree())
            {
                _journalBook = new JournalBookUI();
                _journalBook.SetAnchorsPreset(LayoutPreset.FullRect);
                AddChild(_journalBook);
            }
            _journalBook.Bind(
                _journal,
                tab => _journalCodex.BuildRows(tab),
                tab => _journal.HasUnreadForTab(tab),
                () => _simDay);
            _journalBook.OnClosed += SaveJournal;

            if (JournalSaveStore.Exists)
            {
                var save = JournalSaveStore.Load();
                if (save != null) _journal.RestoreState(save);
                _simDay = MaxEntryDay();
                _journalBook.SetEntries(_journal.Entries);
                _journalBook.ApplyUiState(
                    _journal.HudIsOpen,
                    _journal.HasUnread,
                    _journal.NotificationPing,
                    _journal.ActiveTab);
                GD.Print("[Ashfall Godot] Journal restored from save.");
            }
            else
            {
                int seededDay = JournalDemoHarness.Seed(_journal, catalogs);
                _simDay = Math.Max(4, seededDay);
                _journalBook.SetEntries(_journal.Entries);
                SaveJournal();
                GD.Print("[Ashfall Godot] Journal seeded with opening-day entries.");
            }

            UpdateStatus();
        }

        private int MaxEntryDay()
        {
            int day = 4;
            for (int i = 0; i < _journal.EntryCount; i++)
                if (_journal.Entries[i].Day > day) day = _journal.Entries[i].Day;
            return day;
        }

        private void ToggleJournal()
        {
            if (_journalBook != null) _journalBook.Toggle();
            UpdateStatus();
        }

        private void SaveJournal()
        {
            if (_journal == null) return;
            JournalSaveStore.Save(_journal.CaptureState());
            _journalDirty = false;
        }

        private void SetupEventAdapter()
        {
            if (_hostEventAdapter != null) return;
            SetupJournal();
            if (_eventBus == null) _eventBus = new Ashfall.Core.Events.SimpleEventBus();
            _hostEventAdapter = new AtomicWar.GodotApp.Host.HostEventAdapter(_eventBus, _journal);
            _hostEventAdapter.OnEventDispatched += (id, desc) =>
            {
                if (_statusLabel != null)
                    _statusLabel.Text = $"[EVENT DISPATCHED] {id}: {desc}";
                _journalDirty = true;
            };
        }

        /// <summary>
        /// Writes the journal only when something actually changed. Called from the
        /// throttled _Process tick so a burst of entries costs one file write.
        /// </summary>
        private void FlushJournalIfDirty()
        {
            if (_journalDirty) SaveJournal();
        }

        private void FlushNarrativeIfDirty()
        {
            if (_narrativeDirty) SaveNarrative();
        }

        private void SetupNarrative()
        {
            if (_narrative != null) return;
            _narrative = NarrativeHostSession.Create(_dataDir);
            _narrative.StateChanged += () => _narrativeDirty = true;
            GD.Print("[Ashfall Godot] Narrative host ready.");
        }

        private void SaveNarrative()
        {
            if (_narrative == null) return;
            if (NarrativeSaveStore.TrySave(_narrative.CaptureSave()))
            {
                _narrativeDirty = false;
                GD.Print("[Ashfall Godot] Narrative save written.");
            }
        }

        private void OnNarrativeOpenClicked()
        {
            SetupNarrative();
            _statusLabel.Text = _narrative.SelectDemo("cautious", 0.5f, "loc_denial_cut_substation")
                + "\n" + _narrative.StatusLine();
        }

        private void SetupRadio()
        {
            if (_radio != null)
            {
                _radio.SetDay(_core != null ? _core.Clock.Day : _simDay);
                return;
            }

            _radio = RadioHostSession.Create(_dataDir, _core != null ? _core.Clock.Day : _simDay);
            _radio.StateChanged += () => _radioPanel?.RefreshView();
            GD.Print("[Ashfall Godot] Radio host ready.");
        }

        private void SaveRadio()
        {
            if (_radio == null) return;
            if (RadioSaveStore.TrySave(_radio.CaptureSave()))
            {
                GD.Print("[Ashfall Godot] Radio save written.");
            }
        }

        private void CloseRadioPanel()
        {
            _radioPanel.Visible = false;
        }

        private void CloseJournalPanel()
        {
            _journalPanel.Visible = false;
        }

        private void CloseJournalDetailPanel()
        {
            _journalDetailPanel.Visible = false;
        }

    }
}
