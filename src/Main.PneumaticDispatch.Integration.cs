// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Shelter;
using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PneumaticDispatchHostSession _pneumaticDispatch = null!;
        private bool _pneumaticDispatchDirty;

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

        private void SavePneumaticDispatch()
        {
            if (_pneumaticDispatch == null) return;
            if (CaptureSection("pneumatic_dispatch", _pneumaticDispatch.CapturePersisted()))
                _pneumaticDispatchDirty = false;
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
                    bool servicedNow = _powerGrid?.System == null
                        || _powerGrid.System.IsRoomServed("room_foundry")
                        || _powerGrid.System.IsRoomServed("room_workshop");
                    _pneumaticDispatch.SetBlackout(!servicedNow);
                    actionResult = ActionResult.Success(servicedNow
                        ? "pneumatic.grid_served"
                        : "pneumatic.grid_blackout");
                    break;
                default:
                    actionResult = ActionResult.Failed("unknown_action", "pneumatic.unknown_action");
                    break;
            }
            _statusLabel.Text = actionResult.MessageKey;
            if (_pneumaticDispatchDirty) SavePneumaticDispatch();
            _pneumaticDispatchPanel?.RefreshView();
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
