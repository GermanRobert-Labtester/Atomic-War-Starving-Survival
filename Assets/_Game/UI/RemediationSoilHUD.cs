using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class SoilPlotSnapshot
    {
        public string plotId;
        public string cropType; // e.g. "sunflowers", "hemp"
        public float soilRadiationRads;
        public float decontaminationProgressPercent; // 0..100
        public bool isClean;
    }

    public class RemediationSoilSnapshot
    {
        public int totalPlotsRemediated;
        public float averageSoilRadiationRads;
        public List<SoilPlotSnapshot> plots = new List<SoilPlotSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Soil Phytoremediation & Topsoil De-Radiation HUD view-model.
    /// Monitors outdoor topsoil radiation contamination (rads), hyper-accumulator crops
    /// (sunflowers & hemp absorbing caesium/strontium), soil tilling, and decontaminated plot readiness.
    /// </summary>
    public class RemediationSoilHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedPlotIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnRemediationSoilChanged;
        public event Action<string, string> OnPlantPhytoCropRequested; // (plotId, cropType)

        private Func<RemediationSoilSnapshot> _getSnapshot;
        private RemediationSoilSnapshot _snapshot;

        public void Bind(Func<RemediationSoilSnapshot> getSnapshot)
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

        public bool SelectNextPlot()
        {
            if (!IsOpen || _snapshot == null || _snapshot.plots == null || _snapshot.plots.Count == 0)
                return false;
            SelectedPlotIndex = (SelectedPlotIndex + 1) % _snapshot.plots.Count;
            ReportOutcome("Selected soil plot: " + GetSelectedPlotName());
            return true;
        }

        public bool SelectPreviousPlot()
        {
            if (!IsOpen || _snapshot == null || _snapshot.plots == null || _snapshot.plots.Count == 0)
                return false;
            SelectedPlotIndex = (SelectedPlotIndex - 1 + _snapshot.plots.Count) % _snapshot.plots.Count;
            ReportOutcome("Selected soil plot: " + GetSelectedPlotName());
            return true;
        }

        public bool RequestPlantSunflowers()
        {
            if (!IsOpen || _snapshot == null || _snapshot.plots == null || _snapshot.plots.Count == 0)
            {
                ReportOutcome("No soil plot selected for phytoremediation planting.");
                return false;
            }

            var plot = GetSelectedPlot();
            if (plot == null) return false;

            if (OnPlantPhytoCropRequested == null)
            {
                ReportOutcome("Soil remediation link offline.");
                return false;
            }

            OnPlantPhytoCropRequested.Invoke(plot.plotId, "sunflowers");
            ReportOutcome("Planting Radiation-Absorbing Sunflowers in Soil Plot " + plot.plotId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No soil remediation action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnRemediationSoilChanged?.Invoke();
        }

        private SoilPlotSnapshot GetSelectedPlot()
        {
            if (_snapshot != null && _snapshot.plots != null && SelectedPlotIndex >= 0 && SelectedPlotIndex < _snapshot.plots.Count)
            {
                return _snapshot.plots[SelectedPlotIndex];
            }
            return null;
        }

        private string GetSelectedPlotName()
        {
            var p = GetSelectedPlot();
            return p != null ? p.plotId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SOIL PHYTOREMEDIATION & TOPSOIL DE-RADIATION  [S] close  ·  [Tab] cycle  ·  [P] plant sunflowers");

            if (_snapshot == null)
            {
                sb.Append("\nSoil remediation telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSOIL STATS: Plots Remediated: ").Append(_snapshot.totalPlotsRemediated)
              .Append("  ·  Avg Topsoil Rads: ").Append(_snapshot.averageSoilRadiationRads.ToString("0.#")).Append(" rads");

            sb.Append("\n\nOUTDOOR TOPSOIL PLOTS:");
            if (_snapshot.plots == null || _snapshot.plots.Count == 0)
            {
                sb.Append("\n  No soil plots registered.");
            }
            else
            {
                for (int i = 0; i < _snapshot.plots.Count; i++)
                {
                    var plot = _snapshot.plots[i];
                    if (plot == null) continue;

                    bool selected = (i == SelectedPlotIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Plot ").Append(plot.plotId)
                      .Append(" — Rads: ").Append(plot.soilRadiationRads.ToString("0.#")).Append(" rads")
                      .Append(" — Phyto Crop: ").Append(plot.cropType ?? "None")
                      .Append(" | De-Rad Progress: ").Append(plot.decontaminationProgressPercent.ToString("0")).Append("%");

                    if (plot.isClean) sb.Append("  ✔ [CLEAN TOPSOIL — SAFE FOR FOOD CROPS]");
                    else sb.Append("  ★ [IRRADIATED — REMEDIATION IN PROGRESS]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nSOIL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
