using System;
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
    /// Player "click" path = <see cref="ActivateIndex"/> / <see cref="ActivateSelected"/>
    /// (corpse icons open dispose UI via <see cref="OnIconActivated"/>).
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
                icon.IsCorpse = false;
                icon.IsContaminatedFood = false;
                icon.HasDisposeActions = false;
                icon.IsSelected = false;
                icon.IsMilitaryExclusive = false;
                icon.Tooltip = null;
            });

        /// <summary>Active icons, one per stocked slot (newest-stock order = slot order).</summary>
        public IReadOnlyList<InventoryIcon> Icons => _icons;

        /// <summary>Pool backing <see cref="Icons"/> (profiling/test hook).</summary>
        public GenericObjectPool<InventoryIcon> IconPool => _iconPool;

        public int IconCount => _icons.Count;
        public string StripSummary { get; private set; } = "STORES: empty";

        /// <summary>Keyboard / mouse focus index into <see cref="Icons"/> (-1 = none).</summary>
        public int SelectedIndex { get; private set; } = -1;

        /// <summary>Currently focused icon, or null.</summary>
        public InventoryIcon SelectedIcon =>
            SelectedIndex >= 0 && SelectedIndex < _icons.Count ? _icons[SelectedIndex] : null;

        /// <summary>Raised when the player activates (clicks) an icon.</summary>
        public event Action<InventoryIcon> OnIconActivated;

        /// <summary>Raised when selection index changes.</summary>
        public event Action OnSelectionChanged;

        /// <summary>
        /// Optional tooltip builder (itemId → multi-line tip). Core binds ammo
        /// catalog so military exclusives show [MILITARY EXCLUSIVE].
        /// </summary>
        public Func<string, string> TooltipResolver { get; set; }

        /// <summary>Optional military-exclusive flag (itemId → true).</summary>
        public Func<string, bool> MilitaryExclusiveChecker { get; set; }

        /// <summary>Tooltip for the currently selected icon, or empty.</summary>
        public string SelectedTooltip
        {
            get
            {
                var icon = SelectedIcon;
                return icon != null ? (icon.Tooltip ?? string.Empty) : string.Empty;
            }
        }

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
                    icon.IsCorpse = slot.Item.type == ItemType.Corpse;
                    icon.IsContaminatedFood = slot.Item.type == ItemType.ContaminatedFood;
                    // Corpse slots surface dispose actions (bury / fertilizer) via HUD.
                    icon.HasDisposeActions = icon.IsCorpse;
                    icon.IsMilitaryExclusive = MilitaryExclusiveChecker != null
                        && MilitaryExclusiveChecker(slot.Item.id);
                    string tip = TooltipResolver != null ? TooltipResolver(slot.Item.id) : null;
                    if (string.IsNullOrEmpty(tip))
                    {
                        tip = icon.IsMilitaryExclusive
                            ? (icon.DisplayName ?? icon.ItemId) + "\n[MILITARY EXCLUSIVE]"
                            : (icon.DisplayName ?? icon.ItemId ?? string.Empty);
                    }
                    icon.Tooltip = tip;
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

            ClampSelection();
            RebuildSummary();
        }

        /// <summary>Release every icon (scene teardown / save-load reset).</summary>
        public void Clear()
        {
            for (int i = 0; i < _icons.Count; i++)
                _iconPool.Release(_icons[i]);
            _icons.Clear();
            SelectedIndex = -1;
            RebuildSummary();
        }

        /// <summary>Focus an icon by strip index. Returns false if out of range.</summary>
        public bool SelectIndex(int index)
        {
            if (index < 0 || index >= _icons.Count) return false;
            if (SelectedIndex == index)
            {
                ApplySelectionFlags();
                return true;
            }
            SelectedIndex = index;
            ApplySelectionFlags();
            RebuildSummary();
            OnSelectionChanged?.Invoke();
            return true;
        }

        /// <summary>Cycle focus forward (wraps). Selects 0 if nothing focused.</summary>
        public bool SelectNext()
        {
            if (_icons.Count == 0) return false;
            int next = SelectedIndex < 0 ? 0 : (SelectedIndex + 1) % _icons.Count;
            return SelectIndex(next);
        }

        /// <summary>Cycle focus backward (wraps).</summary>
        public bool SelectPrev()
        {
            if (_icons.Count == 0) return false;
            int prev = SelectedIndex < 0
                ? _icons.Count - 1
                : (SelectedIndex - 1 + _icons.Count) % _icons.Count;
            return SelectIndex(prev);
        }

        /// <summary>
        /// Clear keyboard focus (hides stores tooltip unless diegetic HUD pins it).
        /// </summary>
        public bool ClearSelection()
        {
            if (SelectedIndex < 0) return false;
            SelectedIndex = -1;
            ApplySelectionFlags();
            RebuildSummary();
            OnSelectionChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Player click / confirm on focused icon. Corpses raise
        /// <see cref="OnIconActivated"/> so Core can open dispose UI.
        /// </summary>
        public bool ActivateSelected()
        {
            if (SelectedIndex < 0) return false;
            return ActivateIndex(SelectedIndex);
        }

        /// <summary>
        /// Simulate a click on strip icon <paramref name="index"/>.
        /// Focuses the icon, then fires <see cref="OnIconActivated"/>.
        /// </summary>
        public bool ActivateIndex(int index)
        {
            if (!SelectIndex(index)) return false;
            var icon = _icons[index];
            if (icon == null) return false;
            OnIconActivated?.Invoke(icon);
            return true;
        }

        /// <summary>Click the first corpse stack, if any (dispose entry point).</summary>
        public bool ActivateFirstCorpse()
        {
            for (int i = 0; i < _icons.Count; i++)
            {
                if (_icons[i] != null && _icons[i].IsCorpse)
                    return ActivateIndex(i);
            }
            return false;
        }

        private void ClampSelection()
        {
            if (_icons.Count == 0)
            {
                if (SelectedIndex != -1)
                {
                    SelectedIndex = -1;
                    OnSelectionChanged?.Invoke();
                }
                return;
            }
            if (SelectedIndex >= _icons.Count)
            {
                SelectedIndex = _icons.Count - 1;
                ApplySelectionFlags();
                OnSelectionChanged?.Invoke();
            }
            else
            {
                ApplySelectionFlags();
            }
        }

        private void ApplySelectionFlags()
        {
            for (int i = 0; i < _icons.Count; i++)
            {
                if (_icons[i] != null)
                    _icons[i].IsSelected = i == SelectedIndex;
            }
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
                string name = icon.DisplayName ?? icon.ItemId;
                if (icon.IsCorpse) name = (name ?? "Body") + " [dispose]";
                else if (icon.IsContaminatedFood) name = (name ?? "Can") + " [rust]";
                else if (icon.IsMilitaryExclusive) name = (name ?? "Ammo") + " [mil]";
                if (icon.IsSelected) name = ">" + name;
                sb.Append(name).Append(' ').Append('x').Append(icon.Amount);
            }
            StripSummary = sb.ToString();
        }

        /// <summary>First corpse icon in the strip, if any (opens dispose UI).</summary>
        public InventoryIcon FindFirstCorpseIcon()
        {
            for (int i = 0; i < _icons.Count; i++)
            {
                if (_icons[i] != null && _icons[i].IsCorpse)
                    return _icons[i];
            }
            return null;
        }

        /// <summary>Index of first corpse icon, or -1.</summary>
        public int FindFirstCorpseIndex()
        {
            for (int i = 0; i < _icons.Count; i++)
            {
                if (_icons[i] != null && _icons[i].IsCorpse)
                    return i;
            }
            return -1;
        }

        /// <summary>Total units of ContaminatedFood currently shown.</summary>
        public int ContaminatedFoodCount()
        {
            int n = 0;
            for (int i = 0; i < _icons.Count; i++)
            {
                if (_icons[i] != null && _icons[i].IsContaminatedFood)
                    n += _icons[i].Amount;
            }
            return n;
        }

        /// <summary>Total corpse units currently shown.</summary>
        public int CorpseCount()
        {
            int n = 0;
            for (int i = 0; i < _icons.Count; i++)
            {
                if (_icons[i] != null && _icons[i].IsCorpse)
                    n += _icons[i].Amount;
            }
            return n;
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
        /// <summary>True when this stack is a dead body (Internal Horror dispose UI).</summary>
        public bool IsCorpse;
        /// <summary>True when this stack is humidity-rusted food (botulism risk).</summary>
        public bool IsContaminatedFood;
        /// <summary>True when selecting this icon should open corpse dispose choices.</summary>
        public bool HasDisposeActions;
        /// <summary>True when this icon has keyboard/mouse focus in the strip.</summary>
        public bool IsSelected;
        /// <summary>True when this stack is military/rebel exclusive ammo (not workbench craftable).</summary>
        public bool IsMilitaryExclusive;
        /// <summary>Hover / focus tooltip (caliber, mod, exclusive badge).</summary>
        public string Tooltip;
    }
}
