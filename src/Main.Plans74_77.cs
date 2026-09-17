// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Shelter;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private GeothermalOrcHostSession _geothermalOrc = null!;
        private BallisticsWorkbenchHostSession _ballisticsWorkbench = null!;
        private AeroponicsHostSession _aeroponics = null!;
        private PneumaticDispatchHostSession _pneumaticDispatch = null!;
        private bool _geothermalOrcDirty;
        private bool _ballisticsWorkbenchDirty;
        private bool _aeroponicsDirty;
        private bool _pneumaticDispatchDirty;

        private void ComposePlans74To77()
        {
            SetupPowerGrid();
            SetupInventory();
            SetupCrafting();
            SetupEquipmentCondition();

            SetupGeothermalOrc();
            SetupBallisticsWorkbench();
            SetupAeroponics();
            SetupPneumaticDispatch();
        }

        private void SetupGeothermalOrc()
        {
            if (_geothermalOrc != null) return;
            SetupCampaignDay();
            SetupPowerGrid();
            SetupInventory();
            var rng = _campaignDay.Rng.GetStream(
                Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng;
            _geothermalOrc = GeothermalOrcHostSession.Create(
                _dataDir,
                rng,
                _inventory?.Inventory,
                _powerGrid?.System);
            _geothermalOrc.StateChanged += () => _geothermalOrcDirty = true;
        }

        private void SetupBallisticsWorkbench()
        {
            if (_ballisticsWorkbench != null) return;
            SetupCampaignDay();
            SetupInventory();
            SetupCrafting();
            SetupEquipmentCondition();
            // Plan B89: ensure metrology is available before ballistics calibrate/refurbish.
            SetupPrecisionMetrology();
            var rng = _campaignDay.Rng.GetStream(
                Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng;
            _ballisticsWorkbench = BallisticsWorkbenchHostSession.Create(
                _dataDir,
                rng,
                _inventory?.Inventory,
                _equipmentCondition?.System);
            if (_combat != null)
                _combat.Ballistics = _ballisticsWorkbench.System;
            _ballisticsWorkbench.StateChanged += () => _ballisticsWorkbenchDirty = true;
        }

        private void SetupAeroponics()
        {
            if (_aeroponics != null) return;
            SetupCampaignDay();
            SetupInventory();
            SetupPowerGrid();
            var rng = _campaignDay.Rng.GetStream(
                Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng;
            _aeroponics = AeroponicsHostSession.Create(
                _dataDir,
                rng,
                _inventory.Inventory,
                _powerGrid?.System);
            _aeroponics.StateChanged += () => _aeroponicsDirty = true;
        }

        private void SetupPneumaticDispatch()
        {
            if (_pneumaticDispatch != null) return;
            SetupCampaignDay();
            SetupInventory();
            var rng = _campaignDay.Rng.GetStream(
                Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng;
            _pneumaticDispatch = PneumaticDispatchHostSession.Create(_dataDir, rng);

            // Inventory remains the canonical warehouse authority. The network
            // owns only routing/capsule state; endpoint registration projects
            // room terminals onto that same inventory without duplicating cargo.
            _pneumaticDispatch.RegisterEndpoint("room_station_clinic", _inventory.Inventory);
            _pneumaticDispatch.RegisterEndpoint("room_station_armory", _inventory.Inventory);
            _pneumaticDispatch.RegisterEndpoint("room_station_greenhouse", _inventory.Inventory);
            _pneumaticDispatch.StateChanged += () => _pneumaticDispatchDirty = true;
        }

        private void SaveGeothermalOrc()
        {
            if (_geothermalOrc == null) return;
            if (CaptureSection("geothermal_orc", _geothermalOrc.CapturePersisted()))
                _geothermalOrcDirty = false;
        }

        private void SaveBallisticsWorkbench()
        {
            if (_ballisticsWorkbench == null) return;
            if (CaptureSection("ballistics_workbench", _ballisticsWorkbench.CapturePersisted()))
                _ballisticsWorkbenchDirty = false;
        }

        private void SaveAeroponics()
        {
            if (_aeroponics == null) return;
            if (CaptureSection("aeroponics", _aeroponics.CapturePersisted()))
                _aeroponicsDirty = false;
        }

        private void SavePneumaticDispatch()
        {
            if (_pneumaticDispatch == null) return;
            if (CaptureSection("pneumatic_dispatch", _pneumaticDispatch.CapturePersisted()))
                _pneumaticDispatchDirty = false;
        }

        private void TickGeothermalOrc(int day)
        {
            SetupGeothermalOrc();
            _geothermalOrc.TickDay(day);
            if (_geothermalOrcDirty) SaveGeothermalOrc();
        }

        private void TickAeroponics(int day)
        {
            SetupAeroponics();
            _aeroponics.TickDay(day, operatorSkill: 0f);
            if (_aeroponicsDirty) SaveAeroponics();
        }

        private void TickPneumaticDispatch(int day)
        {
            SetupPneumaticDispatch();
            // C2[6] 23A: the grid owns the tube network's blackout state. The
            // blower shares the foundry/workshop electrical bus, so a brownout that
            // still serves that bus keeps the network pressurised. This overwrites
            // the legacy manual toggle every day, removing the private authority.
            bool serviced = _powerGrid?.System == null
                || _powerGrid.System.IsRoomServed("room_foundry")
                || _powerGrid.System.IsRoomServed("room_workshop");
            _pneumaticDispatch.SetBlackout(!serviced);
            _pneumaticDispatch.TickDay(day, serviced ? 1f : 0f);
            if (_pneumaticDispatchDirty) SavePneumaticDispatch();
        }

        private void HandleGeothermalOrcAction(string action, string param)
        {
            SetupGeothermalOrc();
            int day = _core?.Clock.Day ?? _simDay;
            var parts = (param ?? string.Empty).Split('|');
            ActionResult result;
            switch (action)
            {
                case "add_loop":
                    result = parts.Length >= 2
                        ? (_geothermalOrc.AddLoop(parts[0], parts[1])
                            ? ActionResult.Success("geothermal_orc.loop_added")
                            : ActionResult.Failed("loop_rejected", "geothermal_orc.loop_rejected"))
                        : ActionResult.Failed("invalid_loop", "geothermal_orc.invalid_loop");
                    break;
                case "set_flow":
                    result = parts.Length >= 2 &&
                             float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var flow)
                        ? _geothermalOrc.SetFlow(parts[0], flow)
                        : ActionResult.Failed("invalid_flow", "geothermal_orc.invalid_flow");
                    break;
                case "commission":
                    result = _geothermalOrc.CommissionLoop(param);
                    break;
                case "descale":
                    result = _geothermalOrc.Descale(param);
                    break;
                case "repair":
                    result = _geothermalOrc.Repair(param);
                    break;
                default:
                    result = ActionResult.Failed("unknown_action", "geothermal_orc.unknown_action");
                    break;
            }
            _statusLabel.Text = result.MessageKey;
            if (_geothermalOrcDirty) SaveGeothermalOrc();
            _geothermalOrcPanel?.RefreshView();
        }

        private void HandleBallisticsWorkbenchAction(string action, string param)
        {
            SetupBallisticsWorkbench();
            int day = _core?.Clock.Day ?? _simDay;
            ActionResult result;
            switch (action)
            {
                case "ensure":
                {
                    var parts = (param ?? string.Empty).Split('|');
                    if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]))
                        result = ActionResult.Failed("invalid_weapon", "ballistics.invalid_weapon");
                    else
                    {
                        _ballisticsWorkbench.EnsureProfile(parts[0], parts[1]);
                        result = ActionResult.Success("ballistics.profile_ready");
                    }
                    break;
                }
                case "inspect":
                    result = _ballisticsWorkbench.Inspect(param, day);
                    break;
                case "calibrate":
                {
                    // Plan B89: live tooling calibration from precision metrology when
                    // registered; otherwise workshop precision-room Calibration; never a
                    // hardcoded bunker-wide constant.
                    float tooling = ResolveBallisticsToolingCalibration();
                    result = _ballisticsWorkbench.Calibrate(param, 0.65f, tooling, day);
                    break;
                }
                case "refurbish":
                {
                    float tooling = ResolveBallisticsToolingCalibration();
                    result = _ballisticsWorkbench.Refurbish(
                        param, new[] { "item_ballistics_cleaning_kit" }, tooling, day);
                    break;
                }
                case "attach_optic":
                {
                    var parts = (param ?? string.Empty).Split('|');
                    if (parts.Length < 3 ||
                        string.IsNullOrWhiteSpace(parts[0]) ||
                        string.IsNullOrWhiteSpace(parts[1]) ||
                        string.IsNullOrWhiteSpace(parts[2]))
                    {
                        result = ActionResult.Failed("invalid_optic", "ballistics.invalid_optic");
                        break;
                    }

                    SetupPrecisionOptics();
                    var existing = _ballisticsWorkbench.System.FindProfile(parts[0]);
                    var workpiece = _precisionOptics?.System.State.activeWorkpiece;
                    if (existing?.OpticQuality > 0f)
                    {
                        result = ActionResult.Blocked(
                            "optic_already_attached",
                            "ballistics.optic_already_attached");
                    }
                    else if (workpiece == null || !workpiece.isCompleted)
                    {
                        result = ActionResult.Blocked(
                            "optic_not_ready",
                            "ballistics.optic_not_ready");
                    }
                    else
                    {
                        _ballisticsWorkbench.EnsureProfile(parts[0], parts[1]);
                        float quality = workpiece.accumulatedQuality;
                        var completed = _precisionOptics!.CompleteOptic(parts[2]);
                        if (!completed.IsSuccess)
                        {
                            result = completed;
                        }
                        else if (!_inventory.Inventory.TryConsumeById(parts[2], 1))
                        {
                            result = ActionResult.Failed(
                                "optic_consume_failed",
                                "ballistics.optic_consume_failed");
                        }
                        else
                        {
                            result = _ballisticsWorkbench.AttachOptic(parts[0], quality);
                        }
                    }
                    break;
                }
                default:
                    result = ActionResult.Failed("unknown_action", "ballistics.unknown_action");
                    break;
            }
            _statusLabel.Text = result.MessageKey;
            if (_ballisticsWorkbenchDirty) SaveBallisticsWorkbench();
            _ballisticsWorkbenchPanel?.RefreshView();
        }

        private void HandleAeroponicsAction(string action, string param)
        {
            SetupAeroponics();
            int day = _core?.Clock.Day ?? _simDay;
            var parts = (param ?? string.Empty).Split('|');
            ActionResult result;
            switch (action)
            {
                case "add_chamber":
                    result = _aeroponics.AddChamber(param, "room_greenhouse");
                    break;
                case "plant":
                    result = parts.Length >= 3
                        ? _aeroponics.Plant(parts[0], parts[1], parts[2], day)
                        : ActionResult.Failed("invalid_crop", "aeroponics.invalid_crop");
                    break;
                case "water":
                    if (!_inventory.Inventory.HasSufficient("clean_water", 20))
                    {
                        result = ActionResult.Blocked("missing_water", "aeroponics.missing_water");
                    }
                    else
                    {
                        result = _aeroponics.AddWater(param, 20f, 1f);
                        if (result.IsSuccess)
                            _inventory.Inventory.Remove("clean_water", 20);
                    }
                    break;
                case "harvest":
                {
                    var harvest = _aeroponics.Harvest(param, day);
                    result = harvest.Success
                        ? ActionResult.Success($"aeroponics.harvested.{harvest.ItemId}")
                        : ActionResult.Blocked("not_ready", "aeroponics.not_ready");
                    break;
                }
                default:
                    result = ActionResult.Failed("unknown_action", "aeroponics.unknown_action");
                    break;
            }
            _statusLabel.Text = result.MessageKey;
            if (_aeroponicsDirty) SaveAeroponics();
            _aeroponicsPanel?.RefreshView();
        }

        private void HandlePneumaticDispatchAction(string action, string param)
        {
            SetupPneumaticDispatch();
            int day = _core?.Clock.Day ?? _simDay;
            ActionResult actionResult;
            switch (action)
            {
                case "dispatch":
                {
                    var parts = (param ?? string.Empty).Split('|');
                    if (parts.Length < 4 ||
                        !int.TryParse(parts[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount))
                        actionResult = ActionResult.Failed("invalid_cargo", "pneumatic.invalid_cargo");
                    else
                    {
                        var dispatch = _pneumaticDispatch.Dispatch(
                            parts[0], parts[1], parts[2], amount, 0.5f, 1f,
                            PneumaticDispatchPriority.Normal, day);
                        actionResult = dispatch.Success
                            ? ActionResult.Success($"pneumatic.dispatched.{dispatch.CapsuleId}")
                            : ActionResult.Failed(dispatch.FailureCode, "pneumatic.dispatch_failed");
                    }
                    break;
                }
                case "clear_jam":
                    actionResult = _pneumaticDispatch.ClearJam(param);
                    break;
                case "maintain":
                    actionResult = _pneumaticDispatch.Maintain(param, 10f, day);
                    break;
                case "blackout":
                    _pneumaticDispatch.SetBlackout(!_pneumaticDispatch.System.Snapshot().Blackout);
                    actionResult = ActionResult.Success("pneumatic.blackout_changed");
                    break;
                default:
                    actionResult = ActionResult.Failed("unknown_action", "pneumatic.unknown_action");
                    break;
            }
            _statusLabel.Text = actionResult.MessageKey;
            if (_pneumaticDispatchDirty) SavePneumaticDispatch();
            _pneumaticDispatchPanel?.RefreshView();
        }

        private sealed class GeothermalOrcDayOwner : IDayAdvanceOwner
        {
            private readonly Main _main;
            public GeothermalOrcDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickGeothermalOrc(day);
                events.Add(new DayStateChangeEvent(
                    "geothermal_orc_ticked", "geothermal_orc", null, null, day));
            }
        }

        private sealed class AeroponicsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _main;
            public AeroponicsDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickAeroponics(day);
                events.Add(new DayStateChangeEvent(
                    "aeroponics_ticked", "aeroponics", null, null, day));
            }
        }

        private sealed class PneumaticDispatchDayOwner : IDayAdvanceOwner
        {
            private readonly Main _main;
            public PneumaticDispatchDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPneumaticDispatch(day);
                events.Add(new DayStateChangeEvent(
                    "pneumatic_dispatch_ticked", "pneumatic_dispatch", null, null, day));
            }
        }
    }
}
