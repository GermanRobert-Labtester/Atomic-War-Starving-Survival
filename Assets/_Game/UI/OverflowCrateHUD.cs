using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Player-facing terminal for the bunker receiving crate. It owns selection
    /// and text presentation only; the Inventory overflow system owns transfer
    /// validation and never exposes mutable inventory objects to UI code.
    /// </summary>
    public class OverflowCrateHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string SelectedItemId { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;
        public IReadOnlyList<OverflowCrateItem> Items => _items;

        public event Action OnOverflowCrateChanged;
        public event Action<string> OnTransferRequested;

        private readonly List<OverflowCrateItem> _items = new List<OverflowCrateItem>();
        private Func<OverflowCrateSnapshot> _getSnapshot;
        private OverflowCrateSnapshot _snapshot;

        public void Bind(Func<OverflowCrateSnapshot> getSnapshot)
        {
            _getSnapshot = getSnapshot;
            Refresh();
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public bool SelectNext() => SelectByOffset(1);
        public bool SelectPrevious() => SelectByOffset(-1);

        /// <summary>Request that Core move exactly one unit of the selected item.</summary>
        public bool TransferSelected()
        {
            SyncSnapshot();
            var selected = GetSelected();
            if (!IsOpen || selected == null) return false;
            if (!selected.CanTransfer)
            {
                LastOutcome = "HELD: " + (string.IsNullOrEmpty(selected.TransferBlockReason)
                    ? "Field bag cannot accept that item."
                    : selected.TransferBlockReason);
                Refresh();
                return false;
            }
            if (OnTransferRequested == null)
            {
                LastOutcome = "HELD: crate link offline.";
                Refresh();
                return false;
            }
            OnTransferRequested.Invoke(selected.ItemId);
            return true;
        }

        public void ReportTransferResult(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "HELD: crate report unavailable." : message;
            Refresh();
        }

        public void Refresh()
        {
            SyncSnapshot();
            RebuildPanel();
            OnOverflowCrateChanged?.Invoke();
        }

        private bool SelectByOffset(int offset)
        {
            SyncSnapshot();
            if (_items.Count == 0) return false;
            int current = IndexOfSelected();
            if (current < 0) current = offset < 0 ? 0 : -1;
            int next = (current + offset) % _items.Count;
            if (next < 0) next += _items.Count;
            SelectedItemId = _items[next].ItemId;
            Refresh();
            return true;
        }

        private void SyncSnapshot()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            _items.Clear();
            if (_snapshot?.Items != null)
            {
                for (int i = 0; i < _snapshot.Items.Count; i++)
                {
                    var item = _snapshot.Items[i];
                    if (item != null && item.Amount > 0 && !string.IsNullOrEmpty(item.ItemId))
                        _items.Add(item);
                }
            }

            if (_items.Count == 0)
            {
                SelectedItemId = null;
                return;
            }
            if (IndexOfSelected() < 0)
                SelectedItemId = _items[0].ItemId;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BUNKER OVERFLOW CRATE  [O] close  ·  [,/.] select  ·  [ENTER] move one");
            int count = _items.Count;
            sb.Append("\nRECEIVING CRATE: ").Append(count == 0 ? "empty." : count + " item type" + (count == 1 ? "." : "s."));

            if (_snapshot != null)
            {
                sb.Append("\nFIELD BAG: ").Append(_snapshot.FieldBagStackCount);
                if (_snapshot.FieldBagCapacity > 0) sb.Append("/").Append(_snapshot.FieldBagCapacity);
                sb.Append(" stacks · ").Append(_snapshot.FieldBagWeight.ToString("0.#"));
                if (_snapshot.FieldBagMaxWeight > 0f)
                    sb.Append("/").Append(_snapshot.FieldBagMaxWeight.ToString("0.#"));
                sb.Append(" kg.");
            }

            if (count == 0)
            {
                sb.Append("\nNo returns are waiting in the receiving crate.");
            }
            else
            {
                sb.Append("\n--- STORED RETURNS ---");
                for (int i = 0; i < _items.Count; i++)
                {
                    var item = _items[i];
                    bool selected = item.ItemId == SelectedItemId;
                    sb.Append("\n").Append(selected ? "> " : "  ").Append(i + 1).Append(". ")
                        .Append(string.IsNullOrEmpty(item.DisplayName) ? item.ItemId : item.DisplayName)
                        .Append("  ×").Append(item.Amount)
                        .Append("  ").Append(item.UnitWeight.ToString("0.#")).Append(" kg")
                        .Append(item.CanTransfer ? "  ready" : "  held — " + item.TransferBlockReason);
                }

                var chosen = GetSelected();
                if (chosen != null)
                {
                    sb.Append("\n--- SELECTED ---\n");
                    if (chosen.CanTransfer)
                        sb.Append("Move 1 ").Append(string.IsNullOrEmpty(chosen.DisplayName) ? chosen.ItemId : chosen.DisplayName)
                            .Append(" into the field bag.");
                    else
                        sb.Append("TRANSFER HELD: ").Append(chosen.TransferBlockReason);
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
                sb.Append("\nREPORT: ").Append(LastOutcome);
            PanelSummary = sb.ToString();
        }

        private OverflowCrateItem GetSelected()
        {
            int index = IndexOfSelected();
            return index >= 0 && index < _items.Count ? _items[index] : null;
        }

        private int IndexOfSelected()
        {
            if (string.IsNullOrEmpty(SelectedItemId)) return -1;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].ItemId == SelectedItemId) return i;
            }
            return -1;
        }
    }
}
