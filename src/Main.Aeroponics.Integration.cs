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
        private AeroponicsHostSession _aeroponics = null!;
        private bool _aeroponicsDirty;

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

        private void SaveAeroponics()
        {
            if (_aeroponics == null) return;
            if (CaptureSection("aeroponics", _aeroponics.CapturePersisted()))
                _aeroponicsDirty = false;
        }

        private void TickAeroponics(int day)
        {
            SetupAeroponics();
            _aeroponics.TickDay(day, operatorSkill: 0f);
            if (_aeroponicsDirty) SaveAeroponics();
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

    }
}
