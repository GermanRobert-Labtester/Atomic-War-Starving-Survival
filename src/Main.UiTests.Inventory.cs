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
using Ashfall.Core.Inventory;
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
        /// <summary>Headless smoke: inventory panel builds, add/equip/check flow, save roundtrip.</summary>
        private void RunInventoryUiTestAndQuit()
        {
            BuildUserInterface();
            SetupInventory();

            // This test verifies the add/equip/save path on a clean container.
            // SetupInventory() seeds starting supplies (19/20 capacity slots), so
            // clear first — otherwise capacity/stack limits make the adds fail and
            // the canned_food count assertion can't hold.
            _inventory.Inventory.Clear();

            bool panel = _inventoryPanel != null;
            bool catalog = _inventory.Catalog.Count >= 15
                && _inventory.Catalog.Contains("canned_food")
                && _inventory.Catalog.Contains("geiger_counter")
                && _inventory.Catalog.Contains("gas_mask")
                && _inventory.Catalog.Contains("clean_water");

            string added = _inventory.Add("canned_food", 6);
            bool addOk = added.Contains("Added");
            string geiger = _inventory.Add("geiger_counter", 1);
            bool geigerOk = geiger.Contains("Added");
            string mask = _inventory.Add("gas_mask", 1);
            bool maskOk = mask.Contains("Added");
            string equip = _inventory.Equip("gas_mask");
            bool equipOk = equip.Contains("Equipped");
            bool working = _inventory.Inventory.HasWorkingGeiger();
            string water = _inventory.Add("clean_water", 4);
            bool waterOk = water.Contains("Added");

            // Plan #10: EquipResult/ConsumeResult must be branchable on a
            // typed status rather than the message string. Prove success,
            // unknown-item failure, blocked-but-known-item, and zero
            // mutation on either failure path.
            var faceSlotBeforeUnknown = _inventory.Inventory.GetEquipped(EquipSlot.Face);
            var equipResultBlocked = _inventory.EquipResult("battery"); // held, not equipable -> Blocked
            bool equipTypedBlockedIsBlockedNotSuccess = equipResultBlocked.Status == ActionResult.StatusKind.Blocked;
            var unknownItemResult = _inventory.EquipResult("item_does_not_exist_xyz");
            bool unknownItemIsFailed = unknownItemResult.Status == ActionResult.StatusKind.Failed
                && unknownItemResult.FailureCode == "unknown_item";
            var faceSlotAfterUnknown = _inventory.Inventory.GetEquipped(EquipSlot.Face);
            bool noMutationOnUnknownItemFailure = ReferenceEquals(faceSlotBeforeUnknown, faceSlotAfterUnknown);

            _inventory.Inventory.Unequip(EquipSlot.Face);
            var equipTypedSuccess = _inventory.EquipResult("gas_mask");
            bool equipTypedSuccessIsSuccess = equipTypedSuccess.Status == ActionResult.StatusKind.Success;

            var consumeUnknownResult = _inventory.ConsumeResult("item_does_not_exist_xyz");
            bool consumeUnknownIsFailed = consumeUnknownResult.Status == ActionResult.StatusKind.Failed
                && consumeUnknownResult.FailureCode == "unknown_item";

            int canned = _inventory.Inventory.CountById("canned_food");
            bool itemCheckCount = canned == 6;
            bool protection = _inventory.Inventory.GetEquippedProtection() > 0f;

            // Save → restore roundtrip.
            var save = _inventory.CaptureSave();
            var fresh = new InventoryHostSession();
            fresh.RestoreSave(save);
            bool roundtrip = fresh.Inventory.CountById("canned_food") == 6
                && fresh.Inventory.GetEquipped(EquipSlot.Face) != null;

            bool pass = panel && catalog && addOk && geigerOk && maskOk && equipOk
                && working && waterOk && itemCheckCount && protection && roundtrip
                && equipTypedBlockedIsBlockedNotSuccess && unknownItemIsFailed && noMutationOnUnknownItemFailure
                && equipTypedSuccessIsSuccess && consumeUnknownIsFailed;
            GD.Print($"[InventoryUiTest] panel={panel} catalog={catalog} add={addOk} geiger={geigerOk} " +
                     $"mask={maskOk} equip={equipOk} working={working} water={waterOk} " +
                     $"canned={itemCheckCount} protection={protection} roundtrip={roundtrip} " +
                     $"typedBlocked={equipTypedBlockedIsBlockedNotSuccess} typedUnknownFailed={unknownItemIsFailed} " +
                     $"noMutationOnFailure={noMutationOnUnknownItemFailure} typedSuccess={equipTypedSuccessIsSuccess} " +
                     $"consumeUnknownFailed={consumeUnknownIsFailed}");
            HostCli.EmitSummary("inventory_uitest", pass, pass ? 0 : 1);
            if (System.IO.File.Exists(InventorySaveStore.SavePath))
                System.IO.File.Delete(InventorySaveStore.SavePath);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
