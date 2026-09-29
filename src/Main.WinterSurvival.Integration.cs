// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Recreation;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        // ── Plan 197: deep-freeze console commands ───────────────────────

        private void HandleWinterFreezeAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseWinterFreezePanel(); return; }
            if (_winterFreezePanel == null || _yearOfAsh == null) return;

            var deepFreeze = _yearOfAsh.DeepFreeze;
            if (deepFreeze == null) return;

            switch (action)
            {
                case "clear_ice":
                {
                    deepFreeze.ClearIntakeIce();
                    _winterFreezePanel.ShowFeedback("Cold-work party reports the intake cowling clear.", false);
                    break;
                }
                case "insulate":
                {
                    // Materials transact atomically through the canonical
                    // inventory authority; insulation boost goes through Core.
                    var bill = new InventoryBill();
                    bill.AddCost("scrap_wood", 4);
                    bill.AddCost("cloth", 1);
                    if (TryPayBill(bill))
                    {
                        deepFreeze.UpgradeThermalInsulation(0.10f);
                        _winterFreezePanel.ShowFeedback(
                            $"Insulation reinforced — quality now {deepFreeze.State.thermalInsulationQuality * 100f:0}%.", false);
                    }
                    else
                    {
                        _winterFreezePanel.ShowFeedback("Insulation work needs 4 scrap wood and 1 cloth.", true);
                    }
                    break;
                }
            }
            _winterFreezePanel.RefreshView();
        }

    }
}
