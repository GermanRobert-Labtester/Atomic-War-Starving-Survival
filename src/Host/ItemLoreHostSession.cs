// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store + Host Session : ItemLore
// Core State : Ashfall.Core.Inventory.ItemLoreState
// Host Caller: Main.ItemLore
// Purpose    : Plan 190 — Item Lore & Provenance host session. Records each
//              item's truthful provenance from the committed produce events
//              (craft completion / inventory intake) and each survivor-to-
//              survivor ownership transfer; the read model feeds the item
//              detail surface. No second inventory: instance "provenance
//              chains" reference the canonical inventory item ids and never
//              mint stock.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ItemLoreSaveStore
    {
        public const string FileName = "item_lore_save.json";
        public const string SectionName = "item_lore";

        private static readonly SaveStore<ItemLoreState> s_store =
            SaveStoreHub.Checksummed<ItemLoreState>(FileName, nameof(ItemLoreSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(ItemLoreState state) => s_store.TrySave(state);
        public static ItemLoreState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ItemLoreState state) => s_store.CaptureBare(state);
    }

    /// <summary>
    /// Plan 190 host session over <see cref="ItemLoreSystem"/>. Producers are
    /// only real committed facts; the panel reads and never mutates.
    /// </summary>
    public sealed class ItemLoreHostSession : HostSessionBase
    {
        private readonly ItemLoreSystem _system;

        public ItemLoreSystem System => _system;
        public string LastEvent { get; private set; } = string.Empty;

        public ItemLoreHostSession()
        {
            _system = new ItemLoreSystem();
        }

        public static ItemLoreHostSession Create(ItemLoreState? state = null)
        {
            var session = new ItemLoreHostSession();
            if (state != null) session._system.RestoreState(state);
            return session;
        }

        /// <summary>
        /// One committed craft-completion fact per call. Idempotent on the
        /// instance id, so the same recipe repeating keeps its chain.
        /// </summary>
        public void RecordCraftProvenance(string itemInstanceId, string? crafterId, int craftingDay)
        {
            var prov = _system.RegisterItem(itemInstanceId, crafterId, craftingDay);
            LastEvent = prov.OwnershipChain.Count > 0
                ? $"Provenance recorded for {itemInstanceId} (crafted by {prov.CrafterSurvivorId}, day {prov.CraftingDay})."
                : $"Provenance recorded for {itemInstanceId} (origin unspecified).";
            RaiseStateChanged();
        }

        /// <summary>
        /// One committed inventory-intake fact per call (discovery through a
        /// real expedition/claim event, never a forecast).
        /// </summary>
        public void RecordDiscoveryProvenance(string itemInstanceId, string locationId, int discoveryDay, string context)
        {
            var prov = _system.RegisterItem(itemInstanceId, discoveryLocationId: locationId, discoveryDay: discoveryDay, context: context);
            LastEvent = $"Provenance recorded for {itemInstanceId} (found in {locationId}, day {discoveryDay}).";
            RaiseStateChanged();
        }

        /// <summary>
        /// Explicit ownership transfer between two named survivors.
        /// </summary>
        public bool TransferOwnership(string itemInstanceId, string newOwnerId, int day)
        {
            bool ok = _system.TransferOwnership(itemInstanceId, newOwnerId, day);
            if (ok)
            {
                LastEvent = $"Ownership of {itemInstanceId} transferred to {newOwnerId} (day {day}).";
                RaiseStateChanged();
            }
            return ok;
        }

        /// <summary>
        /// Adds one lore fragment to a tracked item (a real observation by a
        /// survivor, not filler).
        /// </summary>
        public ItemLoreEntry? AddLore(string itemInstanceId, LoreTriggerType trigger, string text, int day, string survivorId = "")
        {
            var entry = _system.AddLore(itemInstanceId, trigger, text, day, survivorId);
            if (entry != null)
            {
                LastEvent = $"Lore recorded for {itemInstanceId} (day {day}).";
                RaiseStateChanged();
            }
            return entry;
        }

        public ItemProvenanceChain? GetProvenance(string itemInstanceId) => _system.GetProvenance(itemInstanceId);
        public IReadOnlyList<ItemLoreEntry> GetItemLore(string itemInstanceId) => _system.GetLoreEntries(itemInstanceId);
        public ItemLoreState CaptureState() => _system.CaptureState();

        public void RestoreState(ItemLoreState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored item-lore state.";
            RaiseStateChanged();
        }

        public bool TrySave() => ItemLoreSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = ItemLoreSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
