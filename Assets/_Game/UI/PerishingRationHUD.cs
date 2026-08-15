#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class RationCrateSnapshot
    {
        public string crateId;
        public string foodItemType;
        public float quantityKg;
        public float spoilagePercent; // 0..100
        public bool isSpoiled;
        public int daysUntilRot;
    }

    public class PerishingRationSnapshot
    {
        public float totalFoodStockKg;
        public float totalSpoiledFoodKg;
        public List<RationCrateSnapshot> crates = new List<RationCrateSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Ration Spoilage & Mould Sorting HUD view-model.
    /// Monitors food crate decomposition (0..100% rot), fungal mold contamination,
    /// sorting moldy grain from fresh rations, salt preservation, and food poisoning prevention.
    /// </summary>
    public class PerishingRationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedCrateIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPerishingRationChanged;
        public event Action<string> OnSortSpoiledRationsRequested; // (crateId)
        public event Action<string> OnSaltPreserveCrateRequested; // (crateId)

        private Func<PerishingRationSnapshot> _getSnapshot;
        private PerishingRationSnapshot _snapshot;

        public void Bind(Func<PerishingRationSnapshot> getSnapshot)
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

        public bool SelectNextCrate()
        {
            if (!IsOpen || _snapshot == null || _snapshot.crates == null || _snapshot.crates.Count == 0)
                return false;
            SelectedCrateIndex = (SelectedCrateIndex + 1) % _snapshot.crates.Count;
            ReportOutcome("Selected ration crate: " + GetSelectedCrateName());
            return true;
        }

        public bool SelectPreviousCrate()
        {
            if (!IsOpen || _snapshot == null || _snapshot.crates == null || _snapshot.crates.Count == 0)
                return false;
            SelectedCrateIndex = (SelectedCrateIndex - 1 + _snapshot.crates.Count) % _snapshot.crates.Count;
            ReportOutcome("Selected ration crate: " + GetSelectedCrateName());
            return true;
        }

        public bool RequestSortSpoiledRations()
        {
            if (!IsOpen || _snapshot == null || _snapshot.crates == null || _snapshot.crates.Count == 0)
            {
                ReportOutcome("No crate selected for moldy ration sorting.");
                return false;
            }

            var crate = GetSelectedCrate();
            if (crate == null) return false;

            if (OnSortSpoiledRationsRequested == null)
            {
                ReportOutcome("Pantry inspection desk link offline.");
                return false;
            }

            OnSortSpoiledRationsRequested.Invoke(crate.crateId);
            ReportOutcome("Sorting moldy food out of Crate " + crate.crateId + " (" + crate.foodItemType + ")...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No ration action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPerishingRationChanged?.Invoke();
        }

        private RationCrateSnapshot GetSelectedCrate()
        {
            if (_snapshot != null && _snapshot.crates != null && SelectedCrateIndex >= 0 && SelectedCrateIndex < _snapshot.crates.Count)
            {
                return _snapshot.crates[SelectedCrateIndex];
            }
            return null;
        }

        private string GetSelectedCrateName()
        {
            var c = GetSelectedCrate();
            return c != null ? (c.crateId + " — " + c.foodItemType) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("RATION SPOILAGE & PANTRY STORAGE MONITOR  [R] close  ·  [Tab] cycle  ·  [S] sort moldy rations");

            if (_snapshot == null)
            {
                sb.Append("\nPantry storage telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nPANTRY STOCKS: Total Fresh Food: ").Append(_snapshot.totalFoodStockKg.ToString("0.#")).Append(" kg")
              .Append("  ·  Total Moldy/Spoiled: ").Append(_snapshot.totalSpoiledFoodKg.ToString("0.#")).Append(" kg");

            sb.Append("\n\nSHELTER RATION CRATES:");
            if (_snapshot.crates == null || _snapshot.crates.Count == 0)
            {
                sb.Append("\n  No ration crates in pantry.");
            }
            else
            {
                for (int i = 0; i < _snapshot.crates.Count; i++)
                {
                    var crate = _snapshot.crates[i];
                    if (crate == null) continue;

                    bool selected = (i == SelectedCrateIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Crate ").Append(crate.crateId)
                      .Append(" — Content: ").Append(crate.foodItemType ?? "Rations")
                      .Append(" (").Append(crate.quantityKg.ToString("0.#")).Append(" kg)")
                      .Append(" — Spoilage: ").Append(crate.spoilagePercent.ToString("0")).Append("%")
                      .Append(" | Rot In: ").Append(crate.daysUntilRot).Append(" days");

                    if (crate.isSpoiled) sb.Append("  ✖ [SPOILED — MOLD CONTAMINATION RISK]");
                    else if (crate.spoilagePercent > 50f) sb.Append("  ★ [WARNING: HIGH SPOILAGE — SORT SOON]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nPANTRY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
