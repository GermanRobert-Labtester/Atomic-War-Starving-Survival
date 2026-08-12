using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class RitualOfferingItemSnapshot
    {
        public string offeringId;
        public string itemName;
        public float moraleBoost;
        public float cultFavorBoost;
        public int offeringCostRations;
        public bool isOffered;
    }

    public class RitualOfferingSnapshot
    {
        public int totalOfferingsMade;
        public float currentCultFavorLevel; // 0..100
        public List<RitualOfferingItemSnapshot> offerings = new List<RitualOfferingItemSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Cult Shrine Ritual Offering HUD view-model.
    /// Monitors food/candle ritual offerings at bunker cult shrines, cult favor progression,
    /// survivor morale stabilization rituals, and taboo sacrifice offerings.
    /// </summary>
    public class RitualOfferingHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedOfferingIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnRitualOfferingChanged;
        public event Action<string> OnMakeOfferingRequested; // (offeringId)

        private Func<RitualOfferingSnapshot> _getSnapshot;
        private RitualOfferingSnapshot _snapshot;

        public void Bind(Func<RitualOfferingSnapshot> getSnapshot)
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

        public bool SelectNextOffering()
        {
            if (!IsOpen || _snapshot == null || _snapshot.offerings == null || _snapshot.offerings.Count == 0)
                return false;
            SelectedOfferingIndex = (SelectedOfferingIndex + 1) % _snapshot.offerings.Count;
            ReportOutcome("Selected ritual offering: " + GetSelectedOfferingName());
            return true;
        }

        public bool SelectPreviousOffering()
        {
            if (!IsOpen || _snapshot == null || _snapshot.offerings == null || _snapshot.offerings.Count == 0)
                return false;
            SelectedOfferingIndex = (SelectedOfferingIndex - 1 + _snapshot.offerings.Count) % _snapshot.offerings.Count;
            ReportOutcome("Selected ritual offering: " + GetSelectedOfferingName());
            return true;
        }

        public bool RequestMakeOffering()
        {
            if (!IsOpen || _snapshot == null || _snapshot.offerings == null || _snapshot.offerings.Count == 0)
            {
                ReportOutcome("No offering item selected for shrine sacrifice.");
                return false;
            }

            var item = GetSelectedOffering();
            if (item == null) return false;

            if (OnMakeOfferingRequested == null)
            {
                ReportOutcome("Shrine altar link offline.");
                return false;
            }

            OnMakeOfferingRequested.Invoke(item.offeringId);
            ReportOutcome("Placing ritual offering [" + item.itemName + "] on Cult Shrine Altar...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No shrine action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnRitualOfferingChanged?.Invoke();
        }

        private RitualOfferingItemSnapshot GetSelectedOffering()
        {
            if (_snapshot != null && _snapshot.offerings != null && SelectedOfferingIndex >= 0 && SelectedOfferingIndex < _snapshot.offerings.Count)
            {
                return _snapshot.offerings[SelectedOfferingIndex];
            }
            return null;
        }

        private string GetSelectedOfferingName()
        {
            var o = GetSelectedOffering();
            return o != null ? o.itemName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CULT SHRINE & RITUAL OFFERINGS  [R] close  ·  [Tab] cycle  ·  [O] make offering");

            if (_snapshot == null)
            {
                sb.Append("\nCult shrine telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSHRINE STATS: Offerings Made: ").Append(_snapshot.totalOfferingsMade)
              .Append("  ·  Cult Favor Level: ").Append(_snapshot.currentCultFavorLevel.ToString("0")).Append("%");

            sb.Append("\n\nCULT SHRINE OFFERING ITEMS:");
            if (_snapshot.offerings == null || _snapshot.offerings.Count == 0)
            {
                sb.Append("\n  No offering items available.");
            }
            else
            {
                for (int i = 0; i < _snapshot.offerings.Count; i++)
                {
                    var item = _snapshot.offerings[i];
                    if (item == null) continue;

                    bool selected = (i == SelectedOfferingIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(item.itemName ?? item.offeringId)
                      .Append(" — Cost: ").Append(item.offeringCostRations).Append(" rations")
                      .Append(" | Morale Boost: +").Append(item.moraleBoost.ToString("0.#"))
                      .Append(" | Cult Favor: +").Append(item.cultFavorBoost.ToString("0.#"));

                    if (item.isOffered) sb.Append("  ✔ [OFFERED TO SHRINE]");
                    else sb.Append("  [AVAILABLE IN INVENTORY]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nSHRINE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
