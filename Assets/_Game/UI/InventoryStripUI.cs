using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Inventory icon strip: one pooled <see cref="InventoryIcon"/> per stocked
    /// slot. Icons are acquired/released from a GenericObjectPool as stock
    /// churns — never instantiated or destroyed at runtime, so long sessions
    /// and 3x fast-forward produce zero icon garbage.
    /// Data-only view-model like JournalBookUI: no GameObjects, string summary.
    /// </summary>
    public class InventoryStripUI : MonoBehaviour
    {
        private readonly List<InventoryIcon> _icons = new List<InventoryIcon>();
        private readonly GenericObjectPool<InventoryIcon> _iconPool = new GenericObjectPool<InventoryIcon>(
            () => new InventoryIcon(),
            icon =>
            {
                icon.ItemId = null;
                icon.DisplayName = null;
                icon.Amount = 0;
                icon.TotalWeight = 0f;
                icon.Type = ItemType.Material;
            });

        /// <summary>Active icons, one per stocked slot (newest-stock order = slot order).</summary>
        public IReadOnlyList<InventoryIcon> Icons => _icons;

        /// <summary>Pool backing <see cref="Icons"/> (profiling/test hook).</summary>
        public GenericObjectPool<InventoryIcon> IconPool => _iconPool;

        public int IconCount => _icons.Count;
        public string StripSummary { get; private set; } = "STORES: empty";

        /// <summary>
        /// Resync icons from live inventory state. Reuses pooled instances for
        /// slots that remain stocked; releases icons for emptied slots; acquires
        /// from the pool for newly stocked ones. Never destroys anything.
        /// </summary>
        public void Sync(AtomicWar._Game.Inventory.Inventory inventory)
        {
            int slotIndex = 0;
            if (inventory != null && inventory.Slots != null)
            {
                var slots = inventory.Slots;
                for (int i = 0; i < slots.Count; i++)
                {
                    var slot = slots[i];
                    if (slot == null || slot.Item == null || slot.Amount <= 0) continue;

                    InventoryIcon icon;
                    if (slotIndex < _icons.Count)
                    {
                        icon = _icons[slotIndex]; // reuse in place: no pool round-trip
                    }
                    else
                    {
                        icon = _iconPool.Acquire();
                        if (icon == null) break; // pool capped: degrade gracefully
                        _icons.Add(icon);
                    }
                    icon.ItemId = slot.Item.id;
                    icon.DisplayName = slot.Item.displayName;
                    icon.Amount = slot.Amount;
                    icon.TotalWeight = slot.Item.weight * slot.Amount;
                    icon.Type = slot.Item.type;
                    slotIndex++;
                }
            }

            // Release surplus icons (slots that emptied) back to the pool.
            while (_icons.Count > slotIndex)
            {
                var surplus = _icons[_icons.Count - 1];
                _icons.RemoveAt(_icons.Count - 1);
                _iconPool.Release(surplus);
            }

            RebuildSummary();
        }

        /// <summary>Release every icon (scene teardown / save-load reset).</summary>
        public void Clear()
        {
            for (int i = 0; i < _icons.Count; i++)
                _iconPool.Release(_icons[i]);
            _icons.Clear();
            RebuildSummary();
        }

        private void RebuildSummary()
        {
            if (_icons.Count == 0)
            {
                StripSummary = "STORES: empty";
                return;
            }
            var sb = new StringBuilder();
            sb.Append("STORES: ");
            for (int i = 0; i < _icons.Count; i++)
            {
                if (i > 0) sb.Append("  ");
                var icon = _icons[i];
                sb.Append(icon.DisplayName ?? icon.ItemId).Append(' ').Append('x').Append(icon.Amount);
            }
            StripSummary = sb.ToString();
        }
    }

    /// <summary>
    /// One pooled inventory icon view-model. Instances live in
    /// InventoryStripUI's GenericObjectPool — recycled, never destroyed.
    /// </summary>
    public class InventoryIcon
    {
        public string ItemId;
        public string DisplayName;
        public int Amount;
        public float TotalWeight;
        public ItemType Type;
    }
}
