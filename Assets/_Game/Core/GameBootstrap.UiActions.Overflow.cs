using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Open or close the bunker receiving crate ([O]).</summary>
        public void ToggleOverflowCrate()
        {
            var crate = _hud != null ? _hud.OverflowCrateHUD : null;
            if (crate == null) return;
            if (!crate.IsOpen)
            {
                _hud.ScavengeDispatchHUD?.Close();
                _hud.FieldGearLoadoutHUD?.Close();
                _hud.BunkerRationingHUD?.Close();
                _hud.WaterPurificationHUD?.Close();
                _hud.AirHeatManagementHUD?.Close();
                _hud.BunkerMaintenanceHUD?.Close();
                _hud.SurvivorTaskBoardHUD?.Close();
            }
            crate.Toggle();
        }

        public bool SelectNextOverflowCrateItem()
        {
            return _hud?.OverflowCrateHUD != null && _hud.OverflowCrateHUD.SelectNext();
        }

        public bool SelectPreviousOverflowCrateItem()
        {
            return _hud?.OverflowCrateHUD != null && _hud.OverflowCrateHUD.SelectPrevious();
        }

        public bool TransferSelectedOverflowCrateItem()
        {
            return _hud?.OverflowCrateHUD != null && _hud.OverflowCrateHUD.TransferSelected();
        }

        /// <summary>
        /// Runs the transfer through the Core-owned overflow system. The system
        /// validates source/destination and calls Inventory.Transfer, which is
        /// atomic and rolls back if the field bag cannot accept the item.
        /// </summary>
        private void HandleOverflowCrateTransferRequested(string itemId)
        {
            var crate = _hud != null ? _hud.OverflowCrateHUD : null;
            if (OverflowCrateSystem == null)
            {
                crate?.ReportTransferResult("HELD: receiving crate is offline.");
                return;
            }

            OverflowCrateSystem.TryTransferOne(itemId, out var result);
            crate?.ReportTransferResult(FormatOverflowCrateTransfer(result));
        }

        private static string FormatOverflowCrateTransfer(OverflowCrateTransferResult result)
        {
            if (result == null) return "HELD: receiving crate report unavailable.";
            string item = string.IsNullOrEmpty(result.DisplayName) ? result.ItemId : result.DisplayName;
            return result.Succeeded
                ? "MOVED: 1 " + item + " to the field bag."
                : "HELD: " + (string.IsNullOrEmpty(result.Reason)
                    ? "field bag could not accept that item."
                    : result.Reason);
        }
    }
}
