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
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
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
        // ── World / Shelter fields (refactored: campaign/day split to Main.Campaign) ──
        private Ashfall.Core.Medical.MedicalWardSystem _medicalWard = null!;
        private bool _medicalWardDirty;
        private StartingLevelHostSession _startingLevel = null!;
        private bool _startingLevelDirty;
        private OpeningProtocolModal _openingProtocolModal = null!;
        private PowerGridHostSession _powerGrid = null!;
        private bool _powerGridDirty;
        private GreenhouseHostSession _greenhouse = null!;
        private GreenhousePanel _greenhousePanel = null!;
        private bool _greenhouseDirty;
        private WorldHostSession _world = null!;
        private bool _worldDirty;

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

            // Plan 22 GAP action routing: parameters encoded in action string
            // so the panel event signature (string, int) stays stable.
            string baseAction = action;
            string? param = null;
            int colon = action.IndexOf(':');
            if (colon > 0)
            {
                baseAction = action.Substring(0, colon);
                param = action.Substring(colon + 1);
            }

            switch (baseAction)
            {
                case "plant":
                    _greenhouse.Plant(plotIndex, param ?? GreenhouseExpansionCatalog.Items.SeedTuber, day);
                    break;
                case "water":
                    if (param == "tainted")
                        _greenhouse.Water(plotIndex, 50f, tainted: true);
                    else
                    {
                        float units = 50f;
                        if (param != null && float.TryParse(param, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                            units = parsed;
                        _greenhouse.Water(plotIndex, Math.Max(1f, units), tainted: false);
                    }
                    break;
                case "clear":
                    _greenhouse.Clear(plotIndex);
                    break;
                case "treat":
                    _greenhouse.TreatBlight(plotIndex);
                    break;
                case "harvest":
                    _greenhouse.Harvest(plotIndex);
                    break;
                case "apiary_inspect":
                    _greenhouse.InspectHive("hive_01", day);
                    break;
                case "apiary_feed":
                    _greenhouse.FeedHive("hive_01", 0.5f);
                    break;
                case "apiary_harvest":
                    _greenhouse.HarvestHoney("hive_01");
                    break;
                case "apiary_install":
                    _greenhouse.InstallHive("hive_01", "bay_orchard", day);
                    break;
                default:
                    GD.PrintErr($"[Ashfall] unhandled greenhouse action: {action}");
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
            if (CaptureSection("world", WorldSaveStore.TryCapturePersisted(
                _world!.CaptureSave()!,
                _world!.CaptureSkyArmorSave()!,
                _world!.CaptureWeatherIntelligenceSave()!,
                _world!.LocationEvolution?.CaptureState()!,
                _world!.Wildlife?.CaptureState()!,
                _world!.Landmarks?.CaptureState()!)))
            {
                _worldDirty = false;
                GD.Print("[Ashfall Godot] World save written.");
            }
        }

        private void SetupCrafting()
        {
            if (_crafting != null) return;
            SetupInventory();
            _sharedResearch = EnsureSharedResearch();
            _crafting = CraftingHostSession.Create(_dataDir, _inventory.Inventory, _sharedResearch);

            _crafting.Workshop.BindSkillEvaluator(survivorId =>
            {
                if (string.IsNullOrEmpty(survivorId)) return 1.0f;
                var def = _survivors?.Roster?.FindDefinition(survivorId);
                if (def == null) return 1.0f;
                float skill = 1.0f;
                if (def.traitIds != null && def.traitIds.Contains("skill_crafting_expert")) skill += 0.5f;
                if (def.traitIds != null && def.traitIds.Contains("skill_scavenge_efficiency")) skill += 0.3f;
                return skill;
            });

            _crafting.PharmaLab.BindSkillEvaluator(chemistId =>
            {
                if (string.IsNullOrEmpty(chemistId)) return 1.0f;
                var def = _survivors?.Roster?.FindDefinition(chemistId);
                if (def == null) return 1.0f;
                float skill = 1.0f;
                if (def.traitIds != null && def.traitIds.Contains("skill_medical_doctor")) skill += 0.5f;
                if (def.traitIds != null && def.traitIds.Contains("skill_chemistry_specialist")) skill += 0.4f;
                return skill;
            });

            _crafting.PharmaLab.OnDependencyRisk += risk =>
            {
                if (!string.IsNullOrEmpty(_crafting.PharmaLab.State.assignedChemistId) && _chemicalDependency != null)
                {
                    _chemicalDependency.System.OnSubstanceConsumed(
                        _crafting.PharmaLab.State.assignedChemistId,
                        _crafting.PharmaLab.State.currentRecipeId,
                        Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
                }
            };

            var save = CraftingSaveStore.TryLoad();
            if (save != null)
            {
                _crafting.RestoreSave(save);
            }

            SyncCraftingStationsFromShelter();

            _crafting.StateChanged += () => _craftingDirty = true;
            GD.Print("[Ashfall Godot] Crafting host ready.");
        }

        private void SyncCraftingStationsFromShelter()
        {
            if (_crafting == null) return;

            // WT-INT-01: Bridge shelter workshop infrastructure to CraftingSystem "workbench" station.
            // Authority: shelter room "room_workshop" in _shelterAssignment or machine health in _shelterWorkshop.
            bool workshopOperational = false;
            float workbenchCondition = 100f;

            if (_shelterWorkshop != null)
            {
                if (_shelterWorkshop.State.machines.TryGetValue("room_workshop", out var machine))
                {
                    workbenchCondition = Math.Clamp(machine.ToolingHealth * 100f, 0f, 100f);
                    workshopOperational = workbenchCondition > 0f;
                }
                else
                {
                    workshopOperational = true;
                }
            }
            else if (_shelterAssignment?.System?.Rooms != null)
            {
                for (int i = 0; i < _shelterAssignment.System.Rooms.Count; i++)
                {
                    var r = _shelterAssignment.System.Rooms[i];
                    if (r != null && string.Equals(r.RoomId, "room_workshop", StringComparison.Ordinal))
                    {
                        workshopOperational = true;
                        break;
                    }
                }
            }

            if (workshopOperational)
            {
                _crafting.SyncStations(new[]
                {
                    new Ashfall.Core.Crafting.CraftingStation
                    {
                        id = "workbench",
                        displayName = "Civilian Workbench",
                        condition = workbenchCondition
                    }
                });
            }
            else
            {
                _crafting.RemoveStation("workbench");
            }
        }

        private void SaveCrafting()
        {
            if (_crafting == null) return;
            if (CaptureSection("crafting", CraftingSaveStore.TryCapturePersisted(_crafting.CaptureSave())))
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
            if (CaptureSection("starting_level", StartingLevelSaveStore.TryCapturePersisted(_startingLevel.CaptureState())))
            {
                _startingLevelDirty = false;
                GD.Print("[Ashfall Godot] Starting level save written.");
            }
        }

        private void SetupPowerGrid()
        {
            if (_powerGrid != null) return;
            SetupCampaignDay();
            var rng = _campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng;
            _powerGrid = PowerGridHostSession.CreateDefault(rng);
            _powerGrid.TryLoad();
            _powerGrid.OnStateChanged += () => _powerGridDirty = true;
        }

        private void SavePowerGrid()
        {
            if (_powerGrid == null) return;

            var save = new PowerGridSave
            {
                simDay = _powerGrid.System.State.SimDay,
                Rooms = new List<PowerGridRoomSave>(),
                State = _powerGrid.System.State.Capture()
            };
            foreach (var room in _powerGrid.System.Rooms)
                save.Rooms.Add(PowerGridSaveCodec.FromRoom(room));

            if (CaptureSection("power_grid", PowerGridSaveStore.TryCapturePersisted(save)))
            {
                _powerGridDirty = false;
                _powerGrid.ClearDirty();
            }
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
            if (CaptureSection("greenhouse", GreenhouseSaveStore.TryCapturePersisted(_greenhouse.CaptureSave())))
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
            if (_craftingPanel != null) _craftingPanel.Visible = false;
        }

        private void CloseWorkshopPanel()
        {
            if (_workshopPanel != null) _workshopPanel.Visible = false;
        }

        private void ClosePharmaLabPanel()
        {
            if (_pharmaLabPanel != null) _pharmaLabPanel.Visible = false;
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

    }
}
