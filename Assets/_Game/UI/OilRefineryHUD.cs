using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class RefineryBatchSnapshot
    {
        public string batchId;
        public float crudeOilInputLiters;
        public float keroseneYieldLiters;
        public float dieselYieldLiters;
        public float toxicSludgeRunoffLiters;
        public bool isRefining;
    }

    public class OilRefinerySnapshot
    {
        public float totalCrudeOilInStock;
        public float totalKeroseneInStock;
        public float totalDieselInStock;
        public List<RefineryBatchSnapshot> batches = new List<RefineryBatchSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Crude Oil Refinery & Fuel Fractional Distillation HUD view-model.
    /// Monitors crude oil refining into kerosene (stove fuel) and diesel (generator fuel),
    /// toxic sludge runoff disposal, thermal catalytic cracker temperature, and refining efficiency.
    /// </summary>
    public class OilRefineryHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedBatchIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnOilRefineryChanged;
        public event Action<string> OnStartRefiningBatchRequested; // (batchId)

        private Func<OilRefinerySnapshot> _getSnapshot;
        private OilRefinerySnapshot _snapshot;

        public void Bind(Func<OilRefinerySnapshot> getSnapshot)
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

        public bool SelectNextBatch()
        {
            if (!IsOpen || _snapshot == null || _snapshot.batches == null || _snapshot.batches.Count == 0)
                return false;
            SelectedBatchIndex = (SelectedBatchIndex + 1) % _snapshot.batches.Count;
            ReportOutcome("Selected refinery batch: " + GetSelectedBatchName());
            return true;
        }

        public bool SelectPreviousBatch()
        {
            if (!IsOpen || _snapshot == null || _snapshot.batches == null || _snapshot.batches.Count == 0)
                return false;
            SelectedBatchIndex = (SelectedBatchIndex - 1 + _snapshot.batches.Count) % _snapshot.batches.Count;
            ReportOutcome("Selected refinery batch: " + GetSelectedBatchName());
            return true;
        }

        public bool RequestStartRefining()
        {
            if (!IsOpen || _snapshot == null || _snapshot.batches == null || _snapshot.batches.Count == 0)
            {
                ReportOutcome("No refinery batch selected to start distillation.");
                return false;
            }

            var batch = GetSelectedBatch();
            if (batch == null) return false;

            if (_snapshot != null && _snapshot.totalCrudeOilInStock < batch.crudeOilInputLiters)
            {
                ReportOutcome("CANNOT REFINE: Insufficient Crude Oil in storage!");
                return false;
            }

            if (OnStartRefiningBatchRequested == null)
            {
                ReportOutcome("Oil refinery catalytic cracker link offline.");
                return false;
            }

            OnStartRefiningBatchRequested.Invoke(batch.batchId);
            ReportOutcome("Starting Crude Oil Fractional Distillation for Batch " + batch.batchId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No oil refinery action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnOilRefineryChanged?.Invoke();
        }

        private RefineryBatchSnapshot GetSelectedBatch()
        {
            if (_snapshot != null && _snapshot.batches != null && SelectedBatchIndex >= 0 && SelectedBatchIndex < _snapshot.batches.Count)
            {
                return _snapshot.batches[SelectedBatchIndex];
            }
            return null;
        }

        private string GetSelectedBatchName()
        {
            var b = GetSelectedBatch();
            return b != null ? b.batchId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CRUDE OIL REFINERY & FUEL DISTILLATION  [O] close  ·  [Tab] cycle  ·  [R] start refining batch");

            if (_snapshot == null)
            {
                sb.Append("\nOil refinery telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nREFINERY FUEL STOCKS: Crude Oil: ").Append(_snapshot.totalCrudeOilInStock.ToString("0.#")).Append(" L")
              .Append("  ·  Kerosene: ").Append(_snapshot.totalKeroseneInStock.ToString("0.#")).Append(" L")
              .Append("  ·  Diesel: ").Append(_snapshot.totalDieselInStock.ToString("0.#")).Append(" L");

            sb.Append("\n\nDISTILLERY CATALYTIC BATCHES:");
            if (_snapshot.batches == null || _snapshot.batches.Count == 0)
            {
                sb.Append("\n  No refinery batches configured.");
            }
            else
            {
                for (int i = 0; i < _snapshot.batches.Count; i++)
                {
                    var batch = _snapshot.batches[i];
                    if (batch == null) continue;

                    bool selected = (i == SelectedBatchIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Batch ").Append(batch.batchId)
                      .Append(" — Input: ").Append(batch.crudeOilInputLiters.ToString("0.#")).Append(" L Crude")
                      .Append(" | Yield: ").Append(batch.keroseneYieldLiters.ToString("0.#")).Append(" L Kerosene / ").Append(batch.dieselYieldLiters.ToString("0.#")).Append(" L Diesel")
                      .Append(" | Sludge: ").Append(batch.toxicSludgeRunoffLiters.ToString("0.#")).Append(" L");

                    if (batch.isRefining) sb.Append("  ★ [DISTILLATION CRACKING IN PROGRESS]");
                    else sb.Append("  [READY TO REFINE]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nREFINERY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
