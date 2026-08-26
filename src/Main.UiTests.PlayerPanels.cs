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

            bool pass = survivors && medical && weather && radio && shelter && status && tutorial;
            GD.Print($"[PlayerPanelsUiTest] survivors={survivors} medical={medical} weather={weather} " +
                     $"radio={radio} shelter={shelter} status={status} tutorial={tutorial}");
            GD.Print(pass ? "PLAYER_PANELS_UITEST PASS" : "PLAYER_PANELS_UITEST FAIL");
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
