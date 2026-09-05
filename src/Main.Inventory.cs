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
        // ── Inventory fields (GAP-ARCH-01 Phase 1) ──
        private InventoryHostSession _inventory = null!;

        private void SetupInventory()
        {
            if (_inventory != null) return;
            _inventory = InventoryHostSession.Create(_dataDir);
            if (_survivors != null)
            {
                _inventory.Survivors = _survivors;
                _survivors.Inventory = _inventory;
            }
            if (_holdfastRuntime != null)
            {
                _holdfastRuntime.InventorySession = _inventory;
                _holdfastRuntime.Inventory = _inventory.Inventory;
            }
            _inventory.StateChanged += () =>
            {
                SaveInventory();
                _inventoryPanel?.RefreshView();
                _inventoryOverlay?.RefreshView();
                _inventoryDetailPanel?.RefreshView();
                _medicalPanel?.RefreshView();
                _shelterPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };

            if (_inventoryPanel == null && _rightColumn != null)
            {
                _inventoryPanel = new AtomicWar.GodotApp.UI.InventoryPanel();
                _rightColumn.AddChild(_inventoryPanel);
            }
            if (_inventoryPanel != null)
            {
                _inventoryPanel.Bind(_inventory);
                _inventoryPanel.OnItemSelected -= OnInventoryItemSelected;
                _inventoryPanel.OnItemSelected += OnInventoryItemSelected;
                _inventoryPanel.RefreshView();
            }
            _inventoryOverlay?.Bind(_inventory);

            // Collectible effect feeder (audit #27): inventory may construct
            // after SetupCollectibles at boot; wire when both sides exist.
            WireCollectibleInventoryFeeder();
        }

        private void OnInventoryOpenClicked()
        {
            SetupInventory();
            _statusLabel.Text = "Inventory open. Storage and gear are listed in the right panel.";
            _codexViewer.Text = _inventory.InventoryLine() + "\n\n" + _inventory.EquipLine();
        }

        private void OnInventoryAddClicked(string itemId, int amount)
        {
            SetupInventory();
            _statusLabel.Text = _inventory.Add(itemId, amount);
            _inventoryPanel.RefreshView();
            _codexViewer.Text = _inventory.InventoryLine() + "\n\n" + _inventory.EquipLine();
        }

        private void OnInventoryRemoveClicked(string itemId, int amount)
        {
            SetupInventory();
            _statusLabel.Text = _inventory.Remove(itemId, amount);
            _inventoryPanel.RefreshView();
        }

        private void OnInventoryItemSelected(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return;
            SetupInventory();
            _inventoryDetailPanel?.Bind(_inventory, itemId);
            _inventoryDetailPanel?.Open();
        }

        private void OnInventoryConsumeClicked(string itemId)
        {
            SetupInventory();
            var result = _inventory.ConsumeResult(itemId);
            string deltas = HoldfastTerminalPanel.FormatDeltas(result.Deltas);
            _statusLabel.Text = result.IsSuccess ? $"Consumed {itemId}. {deltas}".Trim() : result.MessageKey;
            _inventoryPanel?.RefreshView();
            _inventoryDetailPanel?.RefreshView();
            if (result.IsSuccess) ObserveSigil("inventory.used");
        }

        private void OnInventoryEquipClicked(string itemId)
        {
            SetupInventory();
            var result = _inventory.EquipResult(itemId);
            _statusLabel.Text = result.IsSuccess ? $"Equipped {itemId}." : result.MessageKey;
            _inventoryPanel?.RefreshView();
            _inventoryDetailPanel?.RefreshView();
            if (result.IsSuccess) ObserveSigil("inventory.used");
        }

        private void OnInventoryCheckClicked()
        {
            SetupInventory();
            var inv = _inventory.Inventory;
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== ITEM CHECK ===");
            sb.AppendLine($"Canned food: {inv.CountById("canned_food")} on hand (trip need 3)");
            sb.AppendLine($"Clean water: {inv.CountById("clean_water")} on hand (trip need 2)");
            sb.AppendLine($"Iodine pills: {inv.CountById("iodine_pills")}");
            sb.AppendLine($"Battery: {inv.CountById("battery")}");
            sb.AppendLine($"Gas mask: {inv.CountById("gas_mask")}");
            sb.AppendLine($"Geiger: {(inv.HasWorkingGeiger() ? "WORKING" : "NONE/WORKING")}");
            sb.AppendLine($"Equipped protection: {inv.GetEquippedProtection():F2}");
            _codexViewer.Text = sb.ToString();
            _statusLabel.Text = "Item check complete. See the codex viewer.";
        }

        private void SaveInventory()
        {
            if (_inventory == null) return;
            if (CaptureSection("inventory", InventorySaveStore.TryCapturePersisted(_inventory.CaptureSave())))
                GD.Print("[Ashfall Godot] Inventory save written.");
        }

        private void CloseInventoryOverlay()
        {
            _inventoryOverlay.Visible = false;
        }

        private void CloseInventoryDetailPanel()
        {
            _inventoryDetailPanel.Visible = false;
        }

    }
}
