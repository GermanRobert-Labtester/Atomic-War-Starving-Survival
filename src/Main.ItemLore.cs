// SPDX-License-Identifier: MIT
// ============================================================================
// Main partial : Main.ItemLore
// Core State   : Ashfall.Core.Inventory.ItemLoreSystem (Plan 190)
// Purpose      : Plan 190 — per-item provenance & lore host wiring. Producers
//                are ONLY committed facts: a crafting completion and an
//                inventory intake. The owner stays immutable-by-day (no decay
//                tick), so persistence is save-on-mutation and lifecycle rides
//                the inventory setup.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ItemLoreHostSession? _itemLore;
        private bool _itemLoreDirty;

        public ItemLoreHostSession? ItemLore => _itemLore;

        private void SetupItemLore()
        {
            if (_itemLore != null) return;

            var saved = ItemLoreSaveStore.TryLoad();
            _itemLore = ItemLoreHostSession.Create(saved);
            _itemLore.StateChanged += () => _itemLoreDirty = true;

            // Producer seam 1: committed craft completions craft provenance.
            // Re-attached per setup; the host unsubscribes on reset.
            if (_inventory != null)
            {
                _inventory.Inventory.OnItemAdded += OnItemLoreIntake;
            }

            // Producer seam 2: craft completions through the crafting engine.
            if (_crafting != null)
            {
                _crafting.Engine.OnCraftCompleted += OnItemLoreCrafted;
            }
        }

        /// <summary>
        /// Crafting provenance: the recipe's result id is the provenance key
        /// (plan contract: instance chains reference canonical item ids — the
        /// inventory stays non-instanced, so provenance tracks the item id).
        /// </summary>
        private void OnItemLoreCrafted(Recipe recipe, string crafterId)
        {
            if (_itemLore == null || recipe?.result?.id == null) return;
            _itemLore.RecordCraftProvenance(recipe.result.id,
                string.IsNullOrWhiteSpace(crafterId) ? null : crafterId, _simDay);
            _itemLoreDirty = true;
        }

        /// <summary>
        /// Intake provenance: only expedition-location intake is attributed —
        /// a plain stock-up is not a discovery, so this records the shelter
        /// intake context without inventing an adventure.
        /// </summary>
        private void OnItemLoreIntake(ItemDefinition def, int amount)
        {
            if (_itemLore == null || def == null || string.IsNullOrEmpty(def.id)) return;
            // Keep the ledger small: record each distinct intake id once
            // (RegisterItem is idempotent anyway — this avoids event spam).
            if (_itemLore.System.GetProvenance(def.id) != null) return;
            _itemLore.RecordDiscoveryProvenance(def.id, "shelter_intake", _simDay, "intake");
            _itemLoreDirty = true;
        }

        public string GetItemLoreReadout(string itemId)
        {
            if (_itemLore == null) return "Item lore session not ready.";
            var prov = _itemLore.GetProvenance(itemId);
            if (prov == null) return $"{itemId}: no provenance recorded yet.";
            var sb = new System.Text.StringBuilder();
            sb.Append($"{itemId} provenance: ");
            if (!string.IsNullOrEmpty(prov.CrafterSurvivorId))
                sb.Append($"crafted by {prov.CrafterSurvivorId} (day {prov.CraftingDay}); ");
            if (!string.IsNullOrEmpty(prov.DiscoveryLocationId))
                sb.Append($"found at {prov.DiscoveryLocationId} (day {prov.DiscoveryDay}); ");
            sb.Append($"chain [{string.Join(" → ", prov.OwnershipChain)}] — significance {prov.Significance}");
            return sb.ToString();
        }

        public void SaveItemLore()
        {
            if (_itemLore == null) return;
            var state = _itemLore.CaptureState();
            if (CaptureSection(ItemLoreSaveStore.SectionName, ItemLoreSaveStore.TryCapturePersisted(state)))
                _itemLoreDirty = false;
        }

        public void ResetItemLore()
        {
            if (_inventory != null)
            {
                _inventory.Inventory.OnItemAdded -= OnItemLoreIntake;
            }
            if (_crafting != null)
            {
                _crafting.Engine.OnCraftCompleted -= OnItemLoreCrafted;
            }
            _itemLore = null;
            _itemLoreDirty = false;
        }
    }
}
