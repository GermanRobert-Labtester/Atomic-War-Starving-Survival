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
        /// Headless smoke for the five player-facing session panels. Each view
        /// is bound, opened, and checked through its live read-model surface.
        /// </summary>
        private void RunPlayerPanelsUiTestAndQuit()
        {
            BuildUserInterface();
            SetupSurvivors();
            SetupInventory();
            SetupMedical();
            SetupWorld();
            SetupRadio();

            _survivorsOverlay.Bind(_survivors);
            _survivorsOverlay.Open();
            bool survivors = _survivorsOverlay.IsBound
                && _survivorsOverlay.RenderedSurvivorCount == _survivors.RosterState.Count
                && _survivorsOverlay.Visible;
            CloseAllOverlayPanels();

            _medicalPanel.Bind(_medical, _survivors, _inventory,
                _phase0?.Respiratory);
            _medicalPanel.Open();
            bool medical = _medicalPanel.IsBound
                && _medicalPanel.RenderedHealthCount >= _survivors.RosterState.Count
                && _medicalPanel.Visible;
            CloseAllOverlayPanels();

            _world.ForceDemo(Ashfall.Core.WeatherKind.FalloutStorm);
            _weatherPanel.Bind(_world);
            _weatherPanel.Open();
            bool weather = _weatherPanel.IsBound
                && _weatherPanel.BoundWeather == Ashfall.Core.WeatherKind.FalloutStorm
                && _weatherPanel.RenderedHazardCount > 0
                && _weatherPanel.Visible;
            CloseAllOverlayPanels();

            _radioPanel.Bind(_radio);
            _radioPanel.Open();
            bool radio = _radioPanel.IsBound
                && _radio.Engine.FactionCount > 0
                && _radioPanel.RenderedSignalCount > 0
                && _radioPanel.Visible;
            CloseAllOverlayPanels();

            _shelterPanel.Bind(_survivors, _world, _inventory);
            _shelterPanel.Open();
            bool shelter = _shelterPanel.IsBound
                && _shelterPanel.RenderedStructureCount > 0
                && _shelterPanel.Visible;
            CloseAllOverlayPanels();

            _statusPanel.Bind(_survivors, _world.Weather, _powerGrid, _inventory, _simDay);
            _statusPanel.Open();
            bool status = _statusPanel.IsBound
                && _statusPanel.RenderedDayInfoCount >= 2
                && _statusPanel.Visible;
            CloseAllOverlayPanels();

            _tutorialPanel.Bind(_simDay);
            _tutorialPanel.Open();
            bool tutorial = _tutorialPanel.IsBound
                && _tutorialPanel.RenderedControlsCount >= 7
                && _tutorialPanel.Visible;
            CloseAllOverlayPanels();

            SetupMedical();
            SetupPhase0();
            _afflictionsPanel.Bind(_medical, _survivors, _inventory, _phase0?.Respiratory);
            _afflictionsPanel.Open();
            bool afflictions = _afflictionsPanel.IsBound && _afflictionsPanel.Visible;
            CloseAllOverlayPanels();

            _radiationDetailPanel.Bind(_doseLedger, _survivors);
            _radiationDetailPanel.Open();
            bool radiation = _radiationDetailPanel.IsBound && _radiationDetailPanel.Visible;
            CloseAllOverlayPanels();

            // ── Per-Panel Bind/Unbind Lifecycle Verification (Research, Journal, Weather, Expedition) ──
            SetupExpeditions();
            SetupJournal();

            // 1. ResearchPanel: Open -> Bind -> Close -> Reopen
            _sharedResearch = new ResearchSystem(log: new GodotLog());
            _sharedResearch.RegisterDefaults();
            _sharedResearch.UnlockManual("res_rad_mapping");
            _researchPanel.Bind(_sharedResearch);
            _researchPanel.Open();
            int r1Count = _researchPanel.RenderedRowCount;
            _researchPanel.Close();
            _researchPanel.Unbind();
            _researchPanel.Bind(_sharedResearch);
            _researchPanel.Open();
            int r2Count = _researchPanel.RenderedRowCount;
            bool researchLifecycle = _researchPanel.IsBound && _researchPanel.Visible && r1Count == r2Count;
            CloseAllOverlayPanels();

            // 2. JournalPanel: Open -> Bind -> Close -> Reopen
            _journalPanel.Bind(_journal);
            _journalPanel.Open();
            _journalPanel.Close();
            _journalPanel.Unbind();
            _journalPanel.Bind(_journal);
            _journalPanel.Open();
            bool journalLifecycle = _journalPanel.Visible;
            CloseAllOverlayPanels();

            // 3. WeatherPanel: Open -> Bind -> Close -> Reopen
            _weatherPanel.Bind(_world);
            _weatherPanel.Open();
            _weatherPanel.Close();
            _weatherPanel.Unbind();
            _weatherPanel.Bind(_world);
            _weatherPanel.Open();
            bool weatherLifecycle = _weatherPanel.IsBound && _weatherPanel.Visible;
            CloseAllOverlayPanels();

            // 4. ExpeditionPanel: Open -> Bind -> Close -> Reopen
            _expeditionPanel.Bind(_expeditions, _survivors, _inventory);
            _expeditionPanel.Open();
            _expeditionPanel.Close();
            _expeditionPanel.Unbind();
            _expeditionPanel.Bind(_expeditions, _survivors, _inventory);
            _expeditionPanel.Open();
            bool expeditionLifecycle = _expeditionPanel.IsBound && _expeditionPanel.Visible;
            CloseAllOverlayPanels();

            bool lifecyclePass = researchLifecycle && journalLifecycle && weatherLifecycle && expeditionLifecycle;

            bool pass = survivors && medical && weather && radio && shelter && status && tutorial && afflictions && radiation && researchLifecycle && lifecyclePass;
            GD.Print($"[PlayerPanelsUiTest] survivors={survivors} medical={medical} weather={weather} " +
                     $"radio={radio} shelter={shelter} status={status} tutorial={tutorial} " +
                     $"afflictions={afflictions} radiation={radiation} " +
                     $"lifecycle=(res={researchLifecycle}, jrn={journalLifecycle}, wtr={weatherLifecycle}, exp={expeditionLifecycle})");
            GD.Print(pass ? "PLAYER_PANELS_UITEST PASS" : "PLAYER_PANELS_UITEST FAIL");
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
