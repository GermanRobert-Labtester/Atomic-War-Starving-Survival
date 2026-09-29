// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.IO;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BallisticsWorkbenchHostSession _ballisticsWorkbench = null!;
        private bool _ballisticsWorkbenchDirty;

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

        private void SaveBallisticsWorkbench()
        {
            if (_ballisticsWorkbench == null) return;
            if (CaptureSection("ballistics_workbench", _ballisticsWorkbench.CapturePersisted()))
                _ballisticsWorkbenchDirty = false;
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

        /// <summary>
        /// Live tooling calibration for the ballistics workbench consumer only.
        /// Uncalibrated / unavailable returns 0 — never borrows another consumer's grade.
        /// </summary>
        private float ResolveBallisticsToolingCalibration()
        {
            SetupPrecisionMetrology();
            if (_precisionMetrology == null) return 0f;
            return _precisionMetrology.QueryToolingCalibration("ballistics_workbench");
        }

    }
}
