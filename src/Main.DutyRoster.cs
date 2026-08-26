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
        // ── Duty Roster fields (GAP-ARCH-01 Phase 2) ──
        private DutyRosterHostSession _dutyRoster = null!;
        private bool _dutyRosterDirty;

        private void SetupDutyRoster()
        {
            if (_dutyRoster != null) return;
            SetupJournal();
            _dutyRoster = DutyRosterHostSession.Create(_dataDir, log: null, journal: _journal);
            _dutyRoster.StateChanged += () => _dutyRosterDirty = true;

            // Cross-host roundtrip: a save written here (or by the Unity host) restores
            // the chart, marks, and encounter counters instead of starting blank.
            var save = DutyRosterSaveStore.TryLoad();
            if (save != null)
            {
                _dutyRoster.RestoreSave(save);
                _dutyRosterDirty = false; // restore just raised state-change events
                GD.Print($"[Ashfall Godot] Duty Roster state restored (day {_dutyRoster.Clock.Day}).");
            }

            _dutyRoster.Unlock(_simDay);
            RefreshRosterStatus();
            GD.Print($"[Ashfall Godot] Duty Roster ready. {_dutyRoster.CatalogLine()}");
        }

        private void RefreshRosterStatus()
        {
            if (_dutyRoster == null || _statusLabel == null) return;
            _statusLabel.Text =
                $"——— DUTY ROSTER ———\n" +
                _dutyRoster.WallLine() + "\n" +
                _dutyRoster.EncountersLine() + "\n" +
                _dutyRoster.MarksLine() + "\n" +
                $"Day {_simDay} · catalog: {_dutyRoster.CatalogLine()}";
        }

        private void OnRosterInspectWallClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.InspectWall();
        }

        private void OnRosterPencilClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ResolveChart(DutyRosterIds.ChoiceWritePencil)
                + "\n" + _dutyRoster.TickDay();
            RefreshRosterStatus();
        }

        private void OnRosterInkClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ResolveInk();
            RefreshRosterStatus();
        }

        private void OnRosterBurnClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.BurnChart();
            RefreshRosterStatus();
        }

        private void OnRosterTickNightClicked()
        {
            SetupDutyRoster();
            _simDay++;
            _dutyRoster.Clock.AdvanceDays(1);
            TickSimDay(_simDay);
            _statusLabel.Text = _dutyRoster.StartEncounter(ShelterEncounterSystem.KindNightSlate);
            RefreshRosterStatus();
        }

        private void OnRosterVisitorClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.QueueVisitor(ShelterEncounterSystem.VisitorLen);
            RefreshRosterStatus();
        }

        private void OnRosterSecondWinterClicked()
        {
            SetupDutyRoster();
            _statusLabel.Text = _dutyRoster.ActivateSecondWinter();
            RefreshRosterStatus();
        }

        /// <summary>
        /// Real home-occupant snapshot for the Duty Roster morning tick.
        /// </summary>
        private List<Ashfall.Core.DutyRosterOccupant> BuildHomeOccupantSnapshot()
        {
            var occupants = new List<Ashfall.Core.DutyRosterOccupant>();
            if (_survivors == null) return occupants;
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var s = _survivors.RosterState[i];
                if (s == null || string.IsNullOrEmpty(s.Id) || !s.IsAliveState) continue;
                occupants.Add(new Ashfall.Core.DutyRosterOccupant
                {
                    survivorId = s.Id,
                    displayName = FormatSurvivorName(s.Id),
                    occupationObserved = string.Empty,
                    sleptHere = true
                });
            }
            occupants.Sort((a, b) => string.CompareOrdinal(a.survivorId, b.survivorId));
            return occupants;
        }

        private void SaveDutyRoster()
        {
            if (_dutyRoster == null) return;
            if (DutyRosterSaveStore.TrySave(_dutyRoster.CaptureSave()))
            {
                _dutyRosterDirty = false;
                GD.Print($"[Ashfall Godot] Duty Roster save written (day {_dutyRoster.Clock.Day}).");
            }
        }

        private void FlushDutyRosterIfDirty()
        {
            if (_dutyRosterDirty) SaveDutyRoster();
        }

        private void CloseDutyRosterPanel()
        {
            _dutyRosterPanel.Visible = false;
        }

        private void CloseDutyRosterDetailPanel()
        {
            _dutyRosterDetailPanel.Visible = false;
        }
    }
}
