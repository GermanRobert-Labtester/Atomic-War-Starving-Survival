using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Open or close the field face/body protection terminal ([L]).</summary>
        public void ToggleFieldGearLoadout()
        {
            var loadout = _hud != null ? _hud.FieldGearLoadoutHUD : null;
            if (loadout == null) return;
            if (!loadout.IsOpen)
            {
                _hud.ScavengeDispatchHUD?.Close();
                _hud.OverflowCrateHUD?.Close();
                _hud.BunkerRationingHUD?.Close();
                _hud.WaterPurificationHUD?.Close();
                _hud.AirHeatManagementHUD?.Close();
                _hud.BunkerMaintenanceHUD?.Close();
                _hud.SurvivorTaskBoardHUD?.Close();
            }
            loadout.Toggle();
        }

        public bool SelectNextFieldGearCandidate()
        {
            return _hud?.FieldGearLoadoutHUD != null && _hud.FieldGearLoadoutHUD.SelectNext();
        }

        public bool SelectPreviousFieldGearCandidate()
        {
            return _hud?.FieldGearLoadoutHUD != null && _hud.FieldGearLoadoutHUD.SelectPrevious();
        }

        public bool ToggleFieldGearStowSlot()
        {
            return _hud?.FieldGearLoadoutHUD != null && _hud.FieldGearLoadoutHUD.ToggleSelectedSlot();
        }

        public bool EquipSelectedFieldGear()
        {
            return _hud?.FieldGearLoadoutHUD != null && _hud.FieldGearLoadoutHUD.EquipSelected();
        }

        public bool UnequipSelectedFieldGear()
        {
            return _hud?.FieldGearLoadoutHUD != null && _hud.FieldGearLoadoutHUD.UnequipSelectedSlot();
        }

        private void HandleFieldGearEquipRequested(string itemId)
        {
            var loadout = _hud != null ? _hud.FieldGearLoadoutHUD : null;
            if (FieldGearLoadoutSystem == null)
            {
                loadout?.ReportActionResult("HELD: field gear system is offline.");
                return;
            }

            FieldGearLoadoutSystem.TryEquip(itemId, out var result);
            loadout?.ReportActionResult(FormatFieldGearResult(result, "EQUIPPED"));
        }

        private void HandleFieldGearUnequipRequested(EquipSlot slot)
        {
            var loadout = _hud != null ? _hud.FieldGearLoadoutHUD : null;
            if (FieldGearLoadoutSystem == null)
            {
                loadout?.ReportActionResult("HELD: field gear system is offline.");
                return;
            }

            FieldGearLoadoutSystem.TryUnequip(slot, out var result);
            loadout?.ReportActionResult(FormatFieldGearResult(result, "STOWED"));
        }

        private static string FormatFieldGearResult(FieldGearLoadoutResult result, string verb)
        {
            if (result == null) return "HELD: loadout report unavailable.";
            if (!result.Succeeded)
                return "HELD: " + (string.IsNullOrEmpty(result.Reason)
                    ? "the gear change could not be completed."
                    : result.Reason);

            string item = string.IsNullOrEmpty(result.DisplayName) ? result.ItemId : result.DisplayName;
            string message = verb + ": " + item + ".";
            return result.SentToOverflow
                ? message + " Outgoing gear moved to the receiving crate."
                : message;
        }
    }
}
