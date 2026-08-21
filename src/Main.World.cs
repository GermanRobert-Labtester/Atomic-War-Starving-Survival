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
        private void OnGreenhousePlantClicked()
        {
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.PlantGreenhouse(0, "item_seed_tuber", day);
            _expansions.WaterGreenhouse(0, 60f);
            _statusLabel.Text = "Plot 0 planted (seed_tuber) and watered on day " + day + ". The glass holds its heat.";
            RefreshExpansionsStatus();
        }

        /// <summary>Item 10: routes greenhouse panel action buttons through GreenhouseHostSession.</summary>
        private void HandleGreenhouseAction(string action, int plotIndex)
        {
            if (_greenhouse == null || plotIndex < 0) return;
            SetupInventory();
            int day = _core != null ? _core.Clock.Day : _simDay;
            switch (action)
            {
                case "plant":
                    _greenhouse.Plant(plotIndex, "item_seed_tuber", day);
                    break;
                case "water":
                    _greenhouse.Water(plotIndex, 50f, tainted: false);
                    break;
                case "treat":
                    _greenhouse.TreatBlight(plotIndex);
                    break;
                case "clear":
                    _greenhouse.Clear(plotIndex);
                    break;
                case "harvest":
                    _greenhouse.Harvest(plotIndex);
                    break;
            }
            if (_greenhouseDirty) SaveGreenhouse();
            _statusLabel.Text = _greenhouse.LastEvent;
            _greenhousePanel?.RefreshView();
        }

        private void OnGreenhouseTickClicked()
        {
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            _expansions.TickGreenhouse(day);
            _statusLabel.Text = "Greenhouse day ticked (day " + day + "). " + _expansions.GreenhouseLine();
            RefreshExpansionsStatus();
        }

        private void FlushWorldIfDirty()
        {
            if (_worldDirty) SaveWorld();
        }

        private void FlushCraftingIfDirty()
        {
            if (_craftingDirty) SaveCrafting();
        }

        private void SetupWorld()
        {
            if (_world != null) return;
            _world = WorldHostSession.Create(_dataDir);
            _world.StateChanged += () =>
            {
                _worldDirty = true;
                _weatherPanel?.RefreshView();
                _shelterPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };
            GD.Print("[Ashfall Godot] World host ready.");
        }

        private void SaveWorld()
        {
            if (_world == null) return;
            if (WorldSaveStore.TrySave(
                _world!.CaptureSave()!,
                _world!.CaptureSkyArmorSave()!,
                _world!.LocationEvolution?.CaptureState()!,
                _world!.Wildlife?.CaptureState()!,
                _world!.Landmarks?.CaptureState()!))
            {
                _worldDirty = false;
                GD.Print("[Ashfall Godot] World save written.");
            }
        }

        private void OnWorldTickClicked()
        {
            SetupWorld();
            _statusLabel.Text = _world.TickDemo(6f) + "\n" + _world.StatusLine();
        }

        private void OnWorldStormClicked()
        {
            SetupWorld();
            _statusLabel.Text = _world.ForceDemo(WeatherKind.FalloutStorm) + "\n" + _world.StatusLine();
        }

        private void OnWorldSkyArmorClicked(string material)
        {
            SetupWorld();
            _statusLabel.Text = _world.SetSkyArmorDemo(0, material, 1f) + "\n" + _world.SkyArmorStatusLine();
        }

        private void SetupCrafting()
        {
            if (_crafting != null) return;
            SetupInventory();
            _crafting = CraftingHostSession.Create(_dataDir, _inventory.Inventory);
            _crafting.StateChanged += () => _craftingDirty = true;
            GD.Print("[Ashfall Godot] Crafting host ready.");
        }

        private void SaveCrafting()
        {
            if (_crafting == null) return;
            if (CraftingSaveStore.TrySave(_crafting.CaptureSave()))
            {
                _craftingDirty = false;
                GD.Print("[Ashfall Godot] Crafting save written.");
            }
        }

        private void OnCraftingStartClicked()
        {
            SetupCrafting();
            _statusLabel.Text = _crafting.Start("recipe_bandage") + "\n" + _crafting.CraftingLine();
        }

        private void OnCraftingFinishClicked()
        {
            SetupCrafting();
            _statusLabel.Text = _crafting.CompleteAll(1f) + "\n" + _crafting.CraftingLine();
        }

        private void SetupStartingLevel()
        {
            if (_startingLevel != null) return;
            _startingLevel = StartingLevelHostSession.Create();
            _startingLevel.StateChanged += () =>
            {
                _startingLevelDirty = true;
                _openingProtocolModal?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };
            if (_openingProtocolModal != null)
                _openingProtocolModal.Bind(_startingLevel);
            GD.Print("[Ashfall Godot] Starting level host ready.");
        }

        private void SaveStartingLevel()
        {
            if (_startingLevel == null) return;
            if (StartingLevelSaveStore.TrySave(_startingLevel.CaptureState()))
            {
                _startingLevelDirty = false;
                GD.Print("[Ashfall Godot] Starting level save written.");
            }
        }

        private void SetupPowerGrid()
        {
            if (_powerGrid != null) return;
            var rng = new Ashfall.Core.SeededRng(unchecked(_simDay * 31 + 7));
            _powerGrid = PowerGridHostSession.CreateDefault(rng);
            _powerGrid.TryLoad();
            _powerGrid.OnStateChanged += () => _powerGridDirty = true;
        }

        private void SavePowerGrid()
        {
            if (_powerGrid == null) return;
            if (_powerGrid.TrySave()) _powerGridDirty = false;
        }

        private void TickPowerGrid(int day)
        {
            SetupPowerGrid();
            _powerGrid.TickDay(day);
            if (_powerGridDirty) SavePowerGrid();
        }

        private void OpenPowerGrid()
        {
            SetupPowerGrid();
            if (_powerGridPanel == null)
            {
                _powerGridPanel = new PowerGridPanel();
                _powerGridPanel.OnRoomToggled += id => _powerGrid.ToggleBreaker(id);
                _powerGridPanel.OnPriorityChanged += (id, p) => _powerGrid.SetPriority(id, p);
                _powerGridPanel.OnFuelAdded += u => _powerGrid.AddFuel(u);
                AddChild(_powerGridPanel);
            }
            _powerGridPanel.Bind(_powerGrid);
            _powerGridPanel.Open();
        }

        private void SetupMemorial()
        {
            if (_memorial != null) return;
            _memorial = new Ashfall.Core.Memorial.MemorialSystem(
                new Ashfall.Core.Memorial.MemorialState());
            _memorial.OnMemorialized += _ => _memorialDirty = true;
            LoadMemorial();
        }

        private void LoadMemorial()
        {
            try
            {
                var loaded = MemorialSaveStore.TryLoad();
                if (loaded != null) _memorial.RestoreState(loaded.State);
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] Memorial load failed: " + e.Message);
            }
        }

        private void SaveMemorial()
        {
            if (_memorial == null) return;
            try
            {
                var save = new Ashfall.Core.Memorial.MemorialSave
                {
                    simDay = _simDay,
                    State = _memorial.CaptureState()
                };
                if (MemorialSaveStore.TrySave(save))
                    _memorialDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] Memorial save failed: " + e.Message);
            }
        }

        private void SetupCampaignDay()
        {
            if (_campaignDay != null) return;
            _campaignDay = new CampaignDayCoordinator();
            _dailyBriefing = new DailyBriefingState();
            LoadDailyBriefing();
        }

        private void SetupDailyBriefingModal()
        {
            if (_dailyBriefingModal != null) return;
            _dailyBriefingModal = new DailyBriefingModal();
            _dailyBriefingModal.OnAcknowledged += OnBriefingAcknowledged;
            AddChild(_dailyBriefingModal);
            _dailyBriefingModal.Hide();
        }

        private void LoadDailyBriefing()
        {
            try
            {
                var loaded = DailyBriefingSaveStore.TryLoad();
                if (loaded != null) _dailyBriefing.RestoreState(loaded);
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] DailyBriefing load failed: " + e.Message);
                _dailyBriefing = new DailyBriefingState();
            }
        }

        private void SaveDailyBriefing()
        {
            if (_dailyBriefing == null) return;
            try
            {
                var save = _dailyBriefing.CaptureState();
                if (DailyBriefingSaveStore.TrySave(save)) _dailyBriefingDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] DailyBriefing save failed: " + e.Message);
            }
        }

        private void OnBriefingAcknowledged(int day)
        {
            _briefingPending = false;
            if (_dailyBriefing == null) return;
            _dailyBriefing.Consume(day);
            _dailyBriefingDirty = true;
            if (_dailyBriefingDirty) SaveDailyBriefing();
            UpdateHud();
        }

        private void CloseOpeningProtocolModal()
        {
            _openingProtocolModal.Visible = false;
        }

        private void SetupGreenhouse()
        {
            if (_greenhouse != null) return;
            SetupInventory();
            _greenhouse = GreenhouseHostSession.Create(_inventory);
            _greenhouse.StateChanged += () =>
            {
                _greenhouseDirty = true;
                _greenhousePanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };
            if (_greenhousePanel != null)
                _greenhousePanel.Bind(_greenhouse);
            GD.Print("[Ashfall Godot] Greenhouse host ready.");
        }

        private void SaveGreenhouse()
        {
            if (_greenhouse == null) return;
            if (GreenhouseSaveStore.TrySave(_greenhouse.CaptureSave()))
            {
                _greenhouseDirty = false;
                GD.Print("[Ashfall Godot] Greenhouse save written.");
            }
        }

        private void CloseGreenhousePanel()
        {
            _greenhousePanel.Visible = false;
        }

        private void CloseCraftingPanel()
        {
            _craftingPanel.Visible = false;
        }

        private void CloseWeatherPanel()
        {
            _weatherPanel.Visible = false;
        }

        private void CloseWeatherDetailPanel()
        {
            _weatherDetailPanel.Visible = false;
        }

        private void CloseWeatherForecastPanel()
        {
            _weatherForecastPanel.Visible = false;
        }

        /// <summary>
        /// Builds the typed <see cref="DailyBriefingInputs"/> snapshot and shows
        /// the briefing modal. Blocks further simulation until acknowledged.
        /// </summary>
        private void ShowBriefingForDay(int day)
        {
            SetupDailyBriefingModal();
            var inputs = new DailyBriefingInputs
            {
                Day = day,
                GeneratedUtc = DateTime.UtcNow.ToString("o"),
                BuildSeed = day
            };
            var report = DailyBriefingReportBuilder.Build(inputs);
            if (report.IsEmpty) return;
            _dailyBriefing.Enqueue(report);
            _dailyBriefingDirty = true;
            SaveDailyBriefing();
            _briefingPending = true;
            _dailyBriefingModal.Show(report);
        }

    }
}
