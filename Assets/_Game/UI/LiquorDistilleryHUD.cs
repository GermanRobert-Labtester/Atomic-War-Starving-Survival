using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class DistilleryBatchSnapshot
    {
        public string batchId;
        public int moonshineProof; // e.g. 120 proof
        public float volumeLiters;
        public float disinfectantGradePercent;
        public float fermentationProgressPercent; // 0..100
        public bool isReady;
    }

    public class LiquorDistillerySnapshot
    {
        public float totalMoonshineProducedLiters;
        public float totalDisinfectantProducedLiters;
        public List<DistilleryBatchSnapshot> batches = new List<DistilleryBatchSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Moonshine Distillery & Medical Alcohol HUD view-model.
    /// Monitors copper still fermentation batches, moonshine proof levels (100-160 proof),
    /// high-grade medical disinfectant alcohol refining, trade barter spirit bottling, and still explosions.
    /// </summary>
    public class LiquorDistilleryHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedBatchIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnLiquorDistilleryChanged;
        public event Action<string> OnBottleMoonshineRequested; // (batchId)
        public event Action<string> OnRefineDisinfectantRequested; // (batchId)

        private Func<LiquorDistillerySnapshot> _getSnapshot;
        private LiquorDistillerySnapshot _snapshot;

        public void Bind(Func<LiquorDistillerySnapshot> getSnapshot)
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
            ReportOutcome("Selected distillery batch: " + GetSelectedBatchName());
            return true;
        }

        public bool SelectPreviousBatch()
        {
            if (!IsOpen || _snapshot == null || _snapshot.batches == null || _snapshot.batches.Count == 0)
                return false;
            SelectedBatchIndex = (SelectedBatchIndex - 1 + _snapshot.batches.Count) % _snapshot.batches.Count;
            ReportOutcome("Selected distillery batch: " + GetSelectedBatchName());
            return true;
        }

        public bool RequestBottleMoonshine()
        {
            if (!IsOpen || _snapshot == null || _snapshot.batches == null || _snapshot.batches.Count == 0)
            {
                ReportOutcome("No distillery batch selected for bottling.");
                return false;
            }

            var batch = GetSelectedBatch();
            if (batch == null) return false;

            if (!batch.isReady)
            {
                ReportOutcome("Batch " + batch.batchId + " is still fermenting (" + batch.fermentationProgressPercent.ToString("0") + "%).");
                return false;
            }

            if (OnBottleMoonshineRequested == null)
            {
                ReportOutcome("Distillery bottling link offline.");
                return false;
            }

            OnBottleMoonshineRequested.Invoke(batch.batchId);
            ReportOutcome("Bottling fermented Moonshine (" + batch.moonshineProof + " Proof)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No distillery action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnLiquorDistilleryChanged?.Invoke();
        }

        private DistilleryBatchSnapshot GetSelectedBatch()
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
            var sb = new StringBuilder("MOONSHINE DISTILLERY & MEDICAL ALCOHOL STILL  [D] close  ·  [Tab] cycle  ·  [B] bottle moonshine");

            if (_snapshot == null)
            {
                sb.Append("\nDistillery telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nDISTILLERY STATS: Moonshine Produced: ").Append(_snapshot.totalMoonshineProducedLiters.ToString("0.#")).Append(" L")
              .Append("  ·  Medical Disinfectant: ").Append(_snapshot.totalDisinfectantProducedLiters.ToString("0.#")).Append(" L");

            sb.Append("\n\nDISTILLERY FERMENTATION BATCHES:");
            if (_snapshot.batches == null || _snapshot.batches.Count == 0)
            {
                sb.Append("\n  No active alcohol batches in copper still.");
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
                      .Append(" — Proof: ").Append(batch.moonshineProof).Append(" Proof")
                      .Append(" | Volume: ").Append(batch.volumeLiters.ToString("0.#")).Append(" L")
                      .Append(" | Fermentation: ").Append(batch.fermentationProgressPercent.ToString("0")).Append("%");

                    if (batch.isReady) sb.Append("  ✔ [READY TO BOTTLE]");
                    else sb.Append("  [FERMENTING IN COPPER STILL]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nDISTILLERY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
