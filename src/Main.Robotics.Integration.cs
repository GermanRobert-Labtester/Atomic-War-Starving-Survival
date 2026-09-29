// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RoboticsSystem? _robotics;
        private bool _roboticsDirty;

        // ── Plan 201: Advanced Robotics & Pre-War AI ─────────────────────

        public RoboticsSystem EnsureRobotics()
        {
            if (_robotics != null) return _robotics;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("robotics") : new SeededRng(201);
            _robotics = new RoboticsSystem(rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("robotics.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    _robotics.LoadCatalog(json);
                }
            }

            var saved = RoboticsSaveStore.TryLoad();
            if (saved != null)
            {
                _robotics.RestoreState(saved);
            }

            _robotics.OnRogueEventTriggered += (unit) =>
            {
                _journal?.TryAddRawEntry("robot_rogue_event", $"WARNING: Automaton unit {unit.UnitId} logic core corrupted! Unit is unresponsive and rogue.", null!, _simDay);
            };

            _robotics.OnStateChanged += () => _roboticsDirty = true;
            return _robotics;
        }

        private void SetupRobotics()
        {
            EnsureRobotics();
        }

        private void SaveRobotics()
        {
            if (_robotics != null)
            {
                CaptureSection("robotics", RoboticsSaveStore.TryCapturePersisted(_robotics.CaptureState()));
                _roboticsDirty = false;
            }
        }

        // ── Plan 201: robotics workshop commands ─────────────────────────

        private void HandleRoboticsAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseRoboticsWorkshopPanel(); return; }
            if (_roboticsWorkshopPanel == null || _robotics == null) return;

            switch (action)
            {
                case "reactivate":
                {
                    // Atomic material payment before the core raises the unit.
                    if (!_robotics.RobotCatalog.TryGetValue(param, out var def))
                        break;
                    var bill = new InventoryBill();
                    foreach (var m in def.ReactivationMaterials)
                        bill.AddCost(m.ItemId, m.Quantity);
                    if (TryPayBill(bill))
                    {
                        var unit = _robotics.ReactivateRobot(param, 0.5f, out string error);
                        if (unit != null)
                        {
                            _roboticsWorkshopPanel.ShowFeedback($"{def.DisplayName} powers on. Servos remember their work.", false);
                        }
                        else
                        {
                            RefundBill(bill);
                            _roboticsWorkshopPanel.ShowFeedback(error, true);
                        }
                    }
                    else
                    {
                        _roboticsWorkshopPanel.ShowFeedback("Reactivation parts missing from storage.", true);
                    }
                    break;
                }
                case "program":
                {
                    // param: unitId:directiveId
                    var parts = param.Split(':');
                    if (parts.Length != 2) break;
                    if (_robotics.ProgramDirective(parts[0], parts[1], 0.5f, out string error))
                    {
                        _roboticsWorkshopPanel.ShowFeedback("Directive accepted.", false);
                    }
                    else
                    {
                        _roboticsWorkshopPanel.ShowFeedback(error, true);
                    }
                    break;
                }
                case "repair":
                {
                    var bill = new InventoryBill();
                    bill.AddCost("scrap_metal", 2);
                    if (TryPayBill(bill))
                    {
                        if (!_robotics.RepairRobot(param, 250))
                        {
                            RefundBill(bill);
                            _roboticsWorkshopPanel.ShowFeedback("That unit cannot take repair now.", true);
                        }
                        else
                        {
                            _roboticsWorkshopPanel.ShowFeedback("Chassis patched — 250 integrity restored.", false);
                        }
                    }
                    else
                    {
                        _roboticsWorkshopPanel.ShowFeedback("Repair needs 2 scrap metal.", true);
                    }
                    break;
                }
            }
            _roboticsWorkshopPanel.RefreshView();
        }

    }
}
