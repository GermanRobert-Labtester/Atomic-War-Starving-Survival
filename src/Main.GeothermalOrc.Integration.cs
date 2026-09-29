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
        private GeothermalOrcHostSession _geothermalOrc = null!;
        private bool _geothermalOrcDirty;

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

        private void SaveGeothermalOrc()
        {
            if (_geothermalOrc == null) return;
            if (CaptureSection("geothermal_orc", _geothermalOrc.CapturePersisted()))
                _geothermalOrcDirty = false;
        }

        private void TickGeothermalOrc(int day)
        {
            SetupGeothermalOrc();
            _geothermalOrc.TickDay(day);
            if (_geothermalOrcDirty) SaveGeothermalOrc();
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

    }
}
