using System;
using System.Collections.Generic;
using System.Text;
using AtomicWar._Game.Inventory;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Terminal presentation and selection for field face/body protection. It
    /// publishes intent only; Core sends the request to FieldGearLoadoutSystem.
    /// </summary>
    public class FieldGearLoadoutHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string SelectedItemId { get; private set; }
        public EquipSlot SelectedSlot { get; private set; } = EquipSlot.Body;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;
        public IReadOnlyList<FieldGearCandidate> Candidates => _candidates;

        public event Action OnFieldGearLoadoutChanged;
        public event Action<string> OnEquipRequested;
        public event Action<EquipSlot> OnUnequipRequested;

        private readonly List<FieldGearCandidate> _candidates = new List<FieldGearCandidate>();
        private Func<FieldGearLoadoutSnapshot> _getSnapshot;
        private FieldGearLoadoutSnapshot _snapshot;

        public void Bind(Func<FieldGearLoadoutSnapshot> getSnapshot)
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

        public bool ToggleSelectedSlot()
        {
            SelectedSlot = SelectedSlot == EquipSlot.Body ? EquipSlot.Face : EquipSlot.Body;
            Refresh();
            return true;
        }

        public bool EquipSelected()
        {
            SyncSnapshot();
            var selected = GetSelected();
            if (!IsOpen || selected == null) return false;
            if (OnEquipRequested == null)
            {
                LastOutcome = "HELD: loadout link offline.";
                Refresh();
                return false;
            }
            OnEquipRequested.Invoke(selected.ItemId);
            return true;
        }

        public bool UnequipSelectedSlot()
        {
            if (!IsOpen) return false;
            if (OnUnequipRequested == null)
            {
                LastOutcome = "HELD: loadout link offline.";
                Refresh();
                return false;
            }
            OnUnequipRequested.Invoke(SelectedSlot);
            return true;
        }

        public void ReportActionResult(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "HELD: loadout report unavailable." : message;
            Refresh();
        }

        public void Refresh()
        {
            SyncSnapshot();
            RebuildPanel();
            OnFieldGearLoadoutChanged?.Invoke();
        }

        private bool SelectByOffset(int offset)
        {
            SyncSnapshot();
            if (_candidates.Count == 0) return false;
            int current = IndexOfSelected();
            if (current < 0) current = offset < 0 ? 0 : -1;
            int next = (current + offset) % _candidates.Count;
            if (next < 0) next += _candidates.Count;
            SelectedItemId = _candidates[next].ItemId;
            Refresh();
            return true;
        }

        private void SyncSnapshot()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            _candidates.Clear();
            if (_snapshot?.Candidates != null)
            {
                for (int i = 0; i < _snapshot.Candidates.Count; i++)
                {
                    var candidate = _snapshot.Candidates[i];
                    if (candidate != null && candidate.Amount > 0 && !string.IsNullOrEmpty(candidate.ItemId))
                        _candidates.Add(candidate);
                }
            }
            if (_candidates.Count == 0)
                SelectedItemId = null;
            else if (IndexOfSelected() < 0)
                SelectedItemId = _candidates[0].ItemId;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("FIELD GEAR LOADOUT  [L] close  ·  [,/.] gear  ·  [TAB] slot  ·  [ENTER] equip  ·  [U] stow");
            sb.Append("\nSCAVENGE SHIELDING: ").Append((_snapshot?.TotalProtection ?? 0f).ToString("0.#")).Append(" rad/h");
            AppendSlot(sb, _snapshot?.Face, "FACE");
            AppendSlot(sb, _snapshot?.Body, "BODY");

            if (_snapshot != null)
            {
                sb.Append("\nFIELD BAG: ").Append(_snapshot.FieldBagStackCount);
                if (_snapshot.FieldBagCapacity > 0) sb.Append("/").Append(_snapshot.FieldBagCapacity);
                sb.Append(" stacks · ").Append(_snapshot.FieldBagWeight.ToString("0.#"));
                if (_snapshot.FieldBagMaxWeight > 0f) sb.Append("/").Append(_snapshot.FieldBagMaxWeight.ToString("0.#"));
                sb.Append(" kg.");
            }

            sb.Append("\n--- CARRIED PROTECTION ---");
            if (_candidates.Count == 0)
                sb.Append("\nNo face or body gear in the field bag.");
            else
            {
                for (int i = 0; i < _candidates.Count; i++)
                {
                    var candidate = _candidates[i];
                    bool selected = candidate.ItemId == SelectedItemId;
                    sb.Append("\n").Append(selected ? "> " : "  ").Append(i + 1).Append(". ")
                        .Append(DisplayName(candidate)).Append("  ×").Append(candidate.Amount)
                        .Append("  [").Append(SlotLabel(candidate.Slot)).Append("]  ")
                        .Append(candidate.Protection.ToString("0.#")).Append(" rad/h");
                }
            }

            sb.Append("\nSELECTED STOW SLOT: ").Append(SlotLabel(SelectedSlot).ToUpperInvariant()).Append(".");
            if (!string.IsNullOrEmpty(LastOutcome)) sb.Append("\nREPORT: ").Append(LastOutcome);
            PanelSummary = sb.ToString();
        }

        private static void AppendSlot(StringBuilder sb, FieldGearSlotState slot, string label)
        {
            sb.Append("\n").Append(label).Append(": ");
            if (slot == null || !slot.IsEquipped)
            {
                sb.Append("clear.");
                return;
            }

            sb.Append(string.IsNullOrEmpty(slot.DisplayName) ? slot.ItemId : slot.DisplayName);
            if (slot.DurabilityPercent >= 0f)
                sb.Append("  ").Append(slot.DurabilityPercent.ToString("0")).Append("%");
            sb.Append("  ·  ").Append(slot.Protection.ToString("0.#")).Append(" rad/h");
        }

        private FieldGearCandidate GetSelected()
        {
            int index = IndexOfSelected();
            return index >= 0 && index < _candidates.Count ? _candidates[index] : null;
        }

        private int IndexOfSelected()
        {
            if (string.IsNullOrEmpty(SelectedItemId)) return -1;
            for (int i = 0; i < _candidates.Count; i++)
            {
                if (_candidates[i].ItemId == SelectedItemId) return i;
            }
            return -1;
        }

        private static string DisplayName(FieldGearCandidate candidate)
        {
            return candidate == null || string.IsNullOrEmpty(candidate.DisplayName)
                ? candidate?.ItemId
                : candidate.DisplayName;
        }

        private static string SlotLabel(EquipSlot slot)
        {
            return slot == EquipSlot.Face ? "face" : "body";
        }
    }
}
