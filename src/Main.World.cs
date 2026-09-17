// SPDX-License-Identifier: MIT
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
        // C2[6] 23B: player-initiated load sheds accumulated during the day, drained
        // by the power day owner into a `power_shed_player` attribution event.
        private readonly List<string> _pendingPlayerSheds = new List<string>();
        private GreenhouseHostSession _greenhouse = null!;
        private AtomicWar.GodotApp.UI.DeconAirlockPanel _deconAirlockPanel = null!;
        private AtomicWar.GodotApp.UI.GeodeticSurveyPanel _geodeticSurveyPanel = null!;
        private AtomicWar.GodotApp.UI.KineticStoragePanel _kineticStoragePanel = null!;
        private AtomicWar.GodotApp.UI.ChemicalReconPanel _chemicalReconPanel = null!;
        private bool _deconAirlockBound;
        private bool _geodeticSurveyBound;
        private bool _kineticStorageBound;
        private bool _chemicalReconBound;
        private GreenhousePanel _greenhousePanel = null!;
        private bool _greenhouseDirty;
        private WorldHostSession _world = null!;
        private bool _worldDirty;
        private WeatherHostSession _weatherSondeHost = null!;

        private void OnGreenhousePlantClicked()
        {
            SetupGreenhouse();
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            // Single growth authority: player GreenhouseHostSession (shared into hub).
            _greenhouse.Plant(0, GreenhouseExpansionCatalog.Items.SeedTuber, day);
            _greenhouse.Water(0, 60f, tainted: false);
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
                {
                    // Panel emits "water:25:clean", "water:50:clean", "water:50:tainted"
                    // (and legacy "water:tainted" / "water:25").
                    float units = 50f;
                    bool tainted = false;
                    if (param != null)
                    {
                        string[] parts = param.Split(':');
                        if (parts.Length >= 1
                            && float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
                        {
                            units = parsed;
                        }
                        string quality = parts.Length >= 2 ? parts[parts.Length - 1] : parts[0];
                        tainted = string.Equals(quality, "tainted", StringComparison.OrdinalIgnoreCase);
                    }
                    _greenhouse.Water(plotIndex, Math.Max(1f, units), tainted);
                    break;
                }
                case "clear":
                    _greenhouse.Clear(plotIndex);
                    break;
                case "treat":
                    _greenhouse.TreatBlight(plotIndex);
                    break;
                case "harvest":
                    _greenhouse.Harvest(plotIndex);
                    break;
                case "dose_nutrients":
                    _greenhouse.ApplyNutrients(plotIndex);
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
            SetupGreenhouse();
            SetupExpansions();
            int day = _core != null ? _core.Clock.Day : _simDay;
            // Single growth authority — do not also TickGreenhouse on a hub twin.
            _greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);
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
            BindJournalWorldProducerIfReady();
            WireSurgeAdapters();
            _world.StateChanged += () =>
            {
                _worldDirty = true;
                _weatherPanel?.RefreshView();
                _shelterPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };
            GD.Print("[Ashfall Godot] World host ready.");
        }

        private bool _journalWorldProducerBound;

        private void BindJournalWorldProducerIfReady()
        {
            if (_journalWorldProducerBound
                || _journal == null
                || _world?.WastelandMap == null)
                return;

            _world.WastelandMap.OnNodeDiscovered += OnJournalWorldNodeDiscovered;
            _journalWorldProducerBound = true;
        }

        private void OnJournalWorldNodeDiscovered(string locationId)
        {
            _journal?.TryAddAuthoredEntry(locationId);
        }

        private void SetupWeatherSonde()
        {
            if (_weatherSondeHost != null) return;
            SetupWorld();
            SetupInventory();
            var catalog = Ashfall.Core.World.AtmosphericSoundingCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            _weatherSondeHost = new WeatherHostSession(_world?.Weather);
            _weatherSondeHost.SetupSoundingCatalog(catalog?.altitude_bands, catalog?.payloads);
            _weatherSondeHost.BindRecoveryInventory(_inventory?.Inventory);
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

        private bool _relicDeltaRoutingWired;

        /// <summary>
        /// Plan 87 follow-up: route relic restoration completion deltas to the
        /// real campaign authorities — shelter-wide morale through the survivors'
        /// needs system, and the restoration world flag through the campaign
        /// consequence ledger. The Core system emits each delta exactly once per
        /// relic (guarded by completedRelicIds); this routing is idempotent.
        /// </summary>
        private void WireRelicRestorationDeltas(WorkshopReverseEngineeringSystem workshop)
        {
            if (_relicDeltaRoutingWired || workshop == null) return;
            _relicDeltaRoutingWired = true;

            workshop.OnActionCompleted += result =>
            {
                if (!result.IsSuccess) return;

                if (result.Deltas.TryGetValue("morale_bonus", out var morale) && morale > 0 && _survivors != null)
                {
                    var roster = _survivors.RosterState;
                    for (int i = 0; i < roster.Count; i++)
                    {
                        var s = roster[i];
                        if (s != null && s.IsAliveState)
                            _survivors.Needs.Modify(s, NeedKind.Morale, (float)morale);
                    }
                }

                foreach (var kvp in result.Deltas)
                {
                    if (kvp.Key != null && kvp.Key.StartsWith("flag_", System.StringComparison.Ordinal) && kvp.Value > 0)
                        _consequenceLedger.Set(
                            kvp.Key.Substring("flag_".Length),
                            WorkshopReverseEngineeringSystem.SystemId,
                            result.MessageKey,
                            _core != null ? _core.Clock.Day : _simDay);
                }
            };
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

            // Plan 24B A2 — the workshop's craft-time leg resolves through the
            // ONE shared worker-productivity contract (campaign skill authority
            // + fitness + duty-hour overwork), composed with — never replacing
            // — the Phase0 penalty slot and the legacy trait evaluator. Workers
            // with no workshop-sense level resolve null ⇒ exact legacy speed.
            // Bounded [0.8, 1.3]× so degraded crafters slow and skilled ones
            // speed up without extreme swings.
            _crafting.Engine.SetCrafterProductivityTimeMultiplier(crafterId =>
            {
                if (string.IsNullOrEmpty(crafterId)) return 1f;
                var verdict = EnsureWorkerProductivityContract()
                    .Resolve(crafterId, "skill_workshop_sense");
                if (verdict == null) return 1f;
                return MathfCompat.Clamp(verdict.YieldModifierPermille / 1000f, 0.8f, 1.3f);
            });

            WireRelicRestorationDeltas(_crafting.Workshop);

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
            // Authority: shelter room "room_workshop" in _shelterAssignment or machine health in _shelterWorkshop,
            // combined with power availability in _powerGrid.
            bool roomExists = false;
            if (_shelterAssignment?.System?.Rooms != null)
            {
                for (int i = 0; i < _shelterAssignment.System.Rooms.Count; i++)
                {
                    var r = _shelterAssignment.System.Rooms[i];
                    if (r != null && (string.Equals(r.RoomId, "room_workshop", StringComparison.Ordinal) ||
                                      string.Equals(r.RoomId, "room_workshop_heavy", StringComparison.Ordinal) ||
                                      string.Equals(r.RoomId, "room_workshop_precision", StringComparison.Ordinal)))
                    {
                        roomExists = true;
                        break;
                    }
                }
            }

            bool hasMachine = false;
            float machineCondition = 100f;
            if (_shelterWorkshop?.State?.machines != null && _shelterWorkshop.State.machines.Count > 0)
            {
                if (_shelterWorkshop.State.machines.TryGetValue("room_workshop", out var machine))
                {
                    hasMachine = true;
                    machineCondition = Math.Clamp(machine.ToolingHealth * 100f, 0f, 100f);
                }
                else
                {
                    foreach (var m in _shelterWorkshop.State.machines.Values)
                    {
                        if (m != null && m.RoomId != null && m.RoomId.StartsWith("room_workshop", StringComparison.Ordinal))
                        {
                            hasMachine = true;
                            machineCondition = Math.Min(machineCondition, Math.Clamp(m.ToolingHealth * 100f, 0f, 100f));
                        }
                    }
                }
            }

            bool isPowered = _powerGrid?.System == null || _powerGrid.System.IsRoomPowered("room_workshop");

            bool workshopOperational = (roomExists || hasMachine) && isPowered && machineCondition > 0f;
            float workbenchCondition = (roomExists || hasMachine) && isPowered ? machineCondition : 0f;

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
            if (_inventory != null)
            {
                _startingLevel.BindMaintenance(
                    _inventory.Inventory,
                    knowledgeId => EnsureSharedResearch().HasCapability(knowledgeId));
            }
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
            _powerGrid = PowerGridHostSession.CreateDefault(rng, _dataDir);
            _powerGrid.TryLoad();
            _powerGrid.OnStateChanged += () =>
            {
                _powerGridDirty = true;
                SyncCraftingStationsFromShelter();
            };
            // B5–B8 expansion (§27): source degradation/failure events ride the
            // typed power events into the journal authority — presentation
            // only; the grid owns the facts.
            _powerGrid.System.OnPowerChanged += evt =>
            {
                string? entry = evt.Kind switch
                {
                    PowerGridEventKind.GeneratorWorn =>
                        "MAINTENANCE: The generator is wearing out — output derated. A machine_oil service restores full rating.",
                    PowerGridEventKind.FuelStarved =>
                        "FUEL WARNING: The generator tank ran dry mid-day — running at partial output.",
                    _ => null
                };
                if (entry != null)
                    _journal?.TryAddRawEntry(
                        evt.Kind == PowerGridEventKind.GeneratorWorn ? "generator_worn" : "generator_fuel_starved",
                        entry, null!, _simDay);
            };
            WireSurgeAdapters();
        }

        private bool _surgeAdaptersWired;

        /// <summary>
        /// SHELTER_EMP_MEDICAL_POWER (G4): EMP/orbital events feed the grid.
        /// Deterministic + bounded: <see cref="PowerGridSystem.ApplySurgeDay"/>
        /// dedups per day. Called from both setup paths (order-independent);
        /// wiring is idempotent.
        /// </summary>
        private void WireSurgeAdapters()
        {
            if (_surgeAdaptersWired || _powerGrid == null || _world == null) return;
            var weather = _world.Weather;
            if (weather == null) return;
            _surgeAdaptersWired = true;

            weather.OnWeatherChanged += kind =>
            {
                if (kind == WeatherKind.EMPStorm)
                    _powerGrid?.System.ApplySurgeDay(_simDay, _powerGrid.System.EmpStormSeverity);
            };

            var orbital = _world.WeatherIntelligence?.Orbital;
            if (orbital != null)
            {
                orbital.OnImpactDetailed += rep =>
                {
                    if (rep != null)
                        _powerGrid?.System.ApplySurgeDay(rep.Day, rep.PowerGridDisruption / 100f);
                };
            }
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
                _powerGridPanel.OnRoomToggled += id =>
                {
                    bool wasPowered = _powerGrid.System.IsRoomPowered(id);
                    if (!_powerGrid.ToggleBreaker(id))
                        return;
                    ObserveSigil("power.breaker_toggled");
                    // C2[6] 23B: if the player just took a served room offline, record
                    // it as a player shed so the briefing can say who did it.
                    if (wasPowered && !_powerGrid.System.IsRoomPowered(id))
                        _pendingPlayerSheds.Add(id);
                };
                _powerGridPanel.OnPriorityChanged += (id, p) => _powerGrid.SetPriority(id, p);
                _powerGridPanel.OnFuelAdded += u => _powerGrid.AddFuel(u);
                // C2[6] 23B: overload/surge trips need an explicit, costly reset — the
                // canonical machine_oil service is consumed once (preview/consume/commit
                // discipline, same as generator service).
                _powerGridPanel.OnBreakerResetRequested += id =>
                {
                    if (!_powerGrid.System.IsRoomTripped(id))
                        return;
                    var inv = _inventory.Inventory;
                    if (inv.CountById(PowerGridSystem.GeneratorMaintenanceItemId) < 1)
                    {
                        ObserveSigil("power.breaker_reset_missing_item");
                        return;
                    }
                    if (!_powerGrid.ClearTripped(id))
                        return;
                    inv.TryConsumeById(PowerGridSystem.GeneratorMaintenanceItemId, 1);
                    _journal?.TryAddRawEntry("power_breaker_reset",
                        $"MAINTENANCE: Breaker for {id} reset after an overload trip (machine_oil consumed).", null!, _simDay);
                    ObserveSigil("power.breaker_reset");
                };
                // B5–B8 Phase 2: battery-bank install — preview check, canonical
                // item consumed once, then authoritative commit; blocked installs
                // mutate nothing and say why.
                _powerGridPanel.OnBatteryBankInstallRequested += () =>
                {
                    var inv = _inventory.Inventory;
                    if (inv.CountById(PowerGridSystem.BatteryBankItemId) < 1)
                    {
                        ObserveSigil("power.battery_bank_missing_item");
                        return;
                    }
                    if (!_powerGrid.TryInstallBatteryBank(out var reason))
                    {
                        ObserveSigil("power.battery_bank_blocked_" + reason);
                        return;
                    }
                    inv.TryConsumeById(PowerGridSystem.BatteryBankItemId, 1);
                    ObserveSigil("power.battery_bank_installed");
                };
                // B5–B8 Phase 5: generator service — canonical machine_oil,
                // same preview/consume/commit discipline as battery banks.
                _powerGridPanel.OnGeneratorServiceRequested += () =>
                {
                    var inv = _inventory.Inventory;
                    if (inv.CountById(PowerGridSystem.GeneratorMaintenanceItemId) < 1)
                    {
                        ObserveSigil("power.generator_missing_maintenance_item");
                        return;
                    }
                    if (!_powerGrid.PerformGeneratorMaintenance(out var reason))
                    {
                        ObserveSigil("power.generator_service_blocked_" + reason);
                        return;
                    }
                    inv.TryConsumeById(PowerGridSystem.GeneratorMaintenanceItemId, 1);
                    ObserveSigil("power.generator_serviced");
                };
                // B5–B8 expansion (§27): emergency presets — Core owns the
                // policy; the host journals the outcome.
                _powerGridPanel.OnEmergencyPresetRequested += presetId =>
                {
                    if (presetId == "shed")
                    {
                        var changed = _powerGrid.System.ApplyBrownoutShedPreset();
                        _journal?.TryAddRawEntry("power_preset_shed",
                            $"EMERGENCY: Load-shed preset applied — {changed.Count} non-critical circuit(s) demoted to preserve life support.", null!, _simDay);
                        ObserveSigil("power.preset_shed");
                    }
                    else if (presetId == "defaults")
                    {
                        int restored = _powerGrid.System.ApplyCatalogDefaultPriorities();
                        _journal?.TryAddRawEntry("power_preset_defaults",
                            $"Power priorities restored to catalog defaults ({restored} override(s) cleared).", null!, _simDay);
                        ObserveSigil("power.preset_defaults");
                    }
                };
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
            // Share growth authority with the expansion hub when it already exists
            // (SetupExpansions-first path). Hub capture/restore then mirrors player plots.
            if (_expansions != null)
            {
                _expansions.BindGreenhouse(_greenhouse.System);
                _expansions.EnsureGreenhousePlots(3);
            }
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
        private void CloseDeconAirlockPanel() { _deconAirlockPanel.Visible = false; }
        private void CloseGeodeticSurveyPanel() { _geodeticSurveyPanel.Visible = false; }
        private void CloseKineticStoragePanel() { _kineticStoragePanel.Visible = false; }
        private void CloseChemicalReconPanel() { _chemicalReconPanel.Visible = false; }
        private void CloseChemWarfareDefensePanel() { _chemWarfareDefensePanel?.Visible = false; }
        private void CloseCommsArrayTransceiverPanel() { _commsArrayTransceiverPanel?.Visible = false; }
        private void CloseCeremonyFestivalPanel() { _ceremonyFestivalPanel?.Visible = false; }
        private void CloseRoboticsWorkshopPanel() { _roboticsWorkshopPanel?.Visible = false; }
        private void CloseSurvivorDowntimePanel() { _survivorDowntimePanel?.Visible = false; }
        private void CloseWinterFreezePanel() { _winterFreezePanel?.Visible = false; }
        private void CloseFungiCultivationPanel() { _fungiCultivationBedPanel.Visible = false; }
        private void CloseBioFermentationPanel() { _bioFermentationPanel.Visible = false; }
        private void ClosePlasticPyrolysisPanel() { _plasticPyrolysisPanel.Visible = false; }
        private void CloseCargoAirdropPanel() { _cargoAirdropPanel.Visible = false; }

        private void CloseCraftingPanel()
        {
            if (_craftingPanel != null) _craftingPanel.Visible = false;
        }

        private void CloseWorkshopPanel()
        {
            if (_workshopPanel != null) _workshopPanel.Visible = false;
        }

        private void CloseRadioIntelligencePanel()
        {
            if (_radioIntelligencePanel != null) _radioIntelligencePanel.Visible = false;
        }

        private void CloseShelterSocialPanel()
        {
            if (_shelterSocialPanel != null) _shelterSocialPanel.Visible = false;
        }

        private void CloseSubterraneanOperationsPanel()
        {
            if (_subterraneanOperationsPanel != null) _subterraneanOperationsPanel.Visible = false;
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


        private void HandleDeconAirlockAction(string action, string param = "")
        {
            if (action == "OPEN")
            {
                if (_deconAirlockPanel != null)
                {
                    if (!_deconAirlockBound && _decontamination != null) { _deconAirlockPanel.Bind(_decontamination); _deconAirlockBound = true; }
                    _deconAirlockPanel.Visible = true;
                }
                return;
            }
            if (action == "CLOSE") { if (_deconAirlockPanel != null) _deconAirlockPanel.Visible = false; return; }
            if (_deconAirlockPanel == null || _decontamination == null) return;
            switch(action)
            {
                case "start_decon":
                {
                    // param is the selected queue caseId — resolve the real
                    // occupant/gear/contamination from the decon queue.
                    var c = _decontamination.System.State.queue.Find(q => q.caseId == param);
                    if (c != null)
                        _decontamination.System.StartProtocolCycle("decon_standard_return", c.survivorId, c.gearId, c.surfaceContamination);
                    break;
                }
                case "tick_stage": _decontamination.System.TickActiveStage(); break;
                case "manual_override": _decontamination.System.EngageManualOverride(); break;
                case "dispose_gear": _decontamination.System.DisposeContaminatedGear(param); break;
                case "treat_effluent": _decontamination.System.TreatEffluent(); break;
                case "install_filter": _decontamination.System.InstallEffluentFilter(); break;
            }
            _deconAirlockPanel.RefreshView();
            _decontaminationDirty = true;
        }

        private void HandleGeodeticSurveyAction(string action, string param = "")
        {
            if (action == "OPEN")
            {
                if (_geodeticSurveyPanel != null)
                {
                    if (!_geodeticSurveyBound && _geodeticSurvey != null) { _geodeticSurveyPanel.Bind(_geodeticSurvey); _geodeticSurveyBound = true; }
                    _geodeticSurveyPanel.Visible = true;
                }
                return;
            }
            if (action == "CLOSE") { if (_geodeticSurveyPanel != null) _geodeticSurveyPanel.Visible = false; return; }
            if (_geodeticSurveyPanel == null || _geodeticSurvey == null) return;
            int day = _campaignDay?.Calendar?.CurrentDay ?? 1;
            switch(action)
            {
                case "establish":
                    _geodeticSurvey.EstablishMonument(param, day, (id, count) => _inventory?.Inventory?.TryConsume(id, count) ?? false);
                    break;
                case "observe":
                {
                    // From = first active monument that is not the target point.
                    string? from = _geodeticSurvey.System.Monuments
                        .Where(m => m.isActive && m.surveyPointId != param)
                        .Select(m => m.surveyPointId)
                        .FirstOrDefault();
                    if (from != null)
                        _geodeticSurvey.System.Observe(from, param, "clear", 0.5f);
                    break;
                }
                case "resolve":
                {
                    // Try every triple of active monuments (idempotent — the
                    // engine unlocks routes exactly once and returns the
                    // existing triangle on repeats).
                    var ids = _geodeticSurvey.System.Monuments
                        .Where(m => m.isActive).Select(m => m.surveyPointId).ToList();
                    for (int a = 0; a < ids.Count; a++)
                        for (int b = a + 1; b < ids.Count; b++)
                            for (int c = b + 1; c < ids.Count; c++)
                                _geodeticSurvey.System.TryResolveTriangle(ids[a], ids[b], ids[c]);
                    break;
                }
            }
            _geodeticSurveyPanel.RefreshView();
        }

        private void HandleKineticStorageAction(string action, string param = "")
        {
            if (action == "OPEN")
            {
                if (_kineticStoragePanel != null)
                {
                    if (!_kineticStorageBound && _kineticStorage != null) { _kineticStoragePanel.Bind(_kineticStorage); _kineticStorageBound = true; }
                    _kineticStoragePanel.Visible = true;
                }
                return;
            }
            if (action == "CLOSE") { if (_kineticStoragePanel != null) _kineticStoragePanel.Visible = false; return; }
            if (_kineticStoragePanel == null || _kineticStorage == null) return;
            int day = _campaignDay?.Calendar?.CurrentDay ?? 1;
            // Class-rate power limits: never exceed the catalog's charge/discharge kW.
            var flywheel = _kineticStorage.System.FindFlywheel(param);
            var fc = flywheel != null ? _kineticStorage.System.FindClass(flywheel.flywheelClassId) : null;
            switch(action)
            {
                case "CHARGE":
                    if (fc != null) _kineticStorage.System.Charge(param, fc.max_charge_kw, 600f);
                    break;
                case "DISCHARGE":
                    if (fc != null) _kineticStorage.System.Discharge(param, fc.max_discharge_kw, 60f);
                    break;
                case "EMERGENCY_BRAKE":
                    _kineticStorage.EngageEmergencyBrake(param);
                    break;
                case "MAINTENANCE":
                    _kineticStorage.PerformMaintenance(param, day, (id, count) => _inventory?.Inventory?.TryConsume(id, count) ?? false);
                    break;
            }
            _kineticStoragePanel.RefreshView();
        }

        private void HandleChemicalReconAction(string action, string param = "")
        {
            if (action == "OPEN")
            {
                if (_chemicalReconPanel != null)
                {
                    if (!_chemicalReconBound && _chemicalRecon != null) { _chemicalReconPanel.Bind(_chemicalRecon); _chemicalReconBound = true; }
                    _chemicalReconPanel.Visible = true;
                }
                return;
            }
            if (action == "CLOSE") { if (_chemicalReconPanel != null) _chemicalReconPanel.Visible = false; return; }
            if (_chemicalReconPanel == null || _chemicalRecon == null) return;
            switch(action)
            {
                case "deploy_sensor":
                    // Scan with the engine's active sensor band (Core-owned state).
                    _chemicalRecon.System.ScanLocation(param, _chemicalRecon.System.State.activeSensorBand, 0.5f);
                    break;
                case "sample":
                {
                    // param is the hazardId — resolve the location from the
                    // latest observation of that hazard.
                    var obs = _chemicalRecon.System.Observations
                        .Where(o => o.hazardId == param)
                        .OrderByDescending(o => o.lastConfirmedDay)
                        .FirstOrDefault();
                    if (obs != null)
                        _chemicalRecon.CollectSample(param, obs.locationNodeId, 0.5f,
                            (itemId, count) => _inventory?.Inventory?.TryConsume(itemId, count) ?? false);
                    break;
                }
                case "change_filter":
                {
                    // Select the recommended filter category for this location.
                    string recommended = _chemicalRecon.System.GetRecommendedFilter(param);
                    _chemicalRecon.System.SelectFilterCategory(recommended);
                    break;
                }
            }
            _chemicalReconPanel.RefreshView();
        }

        private void HandleGeothermalAction(string action, string param = "")
        {
            if (action == "OPEN") { if (_geothermalAquiferPanel != null) _geothermalAquiferPanel.Visible = true; return; }
            if (action == "CLOSE") { if (_geothermalAquiferPanel != null) _geothermalAquiferPanel.Visible = false; return; }
            if (_geothermalAquiferPanel == null || _geothermalAquifer == null) return;
            switch(action)
            {
                case "start_drilling": _geothermalAquifer.System.StartDrilling(); break;
                case "install_casing": _geothermalAquifer.System.InstallCasing(float.Parse(param ?? "100")); break;
                case "commission_turbine": _geothermalAquifer.System.CommissionTurbine(); break;
                case "descale": _geothermalAquifer.System.Descale(); break;
                case "vent_pressure": _geothermalAquifer.System.VentPressure(); break;
                case "tap_aquifer": _geothermalAquifer.System.TapAquifer(); break;
            }
            _geothermalAquiferPanel.RefreshView();
        }
    }
}
