using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class HydroponicsCropBedSnapshot
    {
        public string bedId;
        public string cropName;
        public float growthPercent; // 0..100
        public float waterIrrigationLiters;
        public bool isSunlampPowered;
        public bool isReadyToHarvest;
    }

    public class HydroponicsCropSnapshot
    {
        public int totalHarvestedYield;
        public float totalWaterConsumption;
        public List<HydroponicsCropBedSnapshot> beds = new List<HydroponicsCropBedSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Hydroponics Crop Growth & Irrigation HUD view-model.
    /// Monitors indoor greenhouse crop growth (0..100%), water irrigation telemetry,
    /// LED sunlamp power draw, crop harvest yields, and blight prevention.
    /// </summary>
    public class HydroponicsCropHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedBedIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHydroponicsChanged;
        public event Action<string> OnWaterBedRequested; // (bedId)
        public event Action<string> OnHarvestBedRequested; // (bedId)

        private Func<HydroponicsCropSnapshot> _getSnapshot;
        private HydroponicsCropSnapshot _snapshot;

        public void Bind(Func<HydroponicsCropSnapshot> getSnapshot)
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

        public bool SelectNextBed()
        {
            if (!IsOpen || _snapshot == null || _snapshot.beds == null || _snapshot.beds.Count == 0)
                return false;
            SelectedBedIndex = (SelectedBedIndex + 1) % _snapshot.beds.Count;
            ReportOutcome("Selected crop bed: " + GetSelectedBedName());
            return true;
        }

        public bool SelectPreviousBed()
        {
            if (!IsOpen || _snapshot == null || _snapshot.beds == null || _snapshot.beds.Count == 0)
                return false;
            SelectedBedIndex = (SelectedBedIndex - 1 + _snapshot.beds.Count) % _snapshot.beds.Count;
            ReportOutcome("Selected crop bed: " + GetSelectedBedName());
            return true;
        }

        public bool RequestWaterBed()
        {
            if (!IsOpen || _snapshot == null || _snapshot.beds == null || _snapshot.beds.Count == 0)
            {
                ReportOutcome("No crop bed selected for irrigation.");
                return false;
            }

            var bed = GetSelectedBed();
            if (bed == null) return false;

            if (OnWaterBedRequested == null)
            {
                ReportOutcome("Irrigation valve link offline.");
                return false;
            }

            OnWaterBedRequested.Invoke(bed.bedId);
            ReportOutcome("Irrigating Hydroponic Bed " + bed.bedId + " (" + bed.cropName + ")...");
            return true;
        }

        public bool RequestHarvestBed()
        {
            if (!IsOpen || _snapshot == null || _snapshot.beds == null || _snapshot.beds.Count == 0)
            {
                ReportOutcome("No crop bed selected for harvest.");
                return false;
            }

            var bed = GetSelectedBed();
            if (bed == null) return false;

            if (!bed.isReadyToHarvest)
            {
                ReportOutcome("Bed " + bed.bedId + " crop (" + bed.cropName + ") is not yet fully grown.");
                return false;
            }

            if (OnHarvestBedRequested == null)
            {
                ReportOutcome("Greenhouse harvest link offline.");
                return false;
            }

            OnHarvestBedRequested.Invoke(bed.bedId);
            ReportOutcome("HARVESTING mature crops from Bed " + bed.bedId + " (" + bed.cropName + ")!");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No hydroponics action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHydroponicsChanged?.Invoke();
        }

        private HydroponicsCropBedSnapshot GetSelectedBed()
        {
            if (_snapshot != null && _snapshot.beds != null && SelectedBedIndex >= 0 && SelectedBedIndex < _snapshot.beds.Count)
            {
                return _snapshot.beds[SelectedBedIndex];
            }
            return null;
        }

        private string GetSelectedBedName()
        {
            var b = GetSelectedBed();
            return b != null ? (b.bedId + " — " + b.cropName) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("HYDROPONICS GREENHOUSE & IRRIGATION  [H] close  ·  [Tab] cycle  ·  [W] water  ·  [H] harvest");

            if (_snapshot == null)
            {
                sb.Append("\nGreenhouse telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nGREENHOUSE STATS: Harvest Yield: ").Append(_snapshot.totalHarvestedYield).Append(" kg")
              .Append("  ·  Water Consumed: ").Append(_snapshot.totalWaterConsumption.ToString("0.#")).Append(" L");

            sb.Append("\n\nHYDROPONIC CROP BEDS:");
            if (_snapshot.beds == null || _snapshot.beds.Count == 0)
            {
                sb.Append("\n  No hydroponic beds active.");
            }
            else
            {
                for (int i = 0; i < _snapshot.beds.Count; i++)
                {
                    var bed = _snapshot.beds[i];
                    if (bed == null) continue;

                    bool selected = (i == SelectedBedIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Bed ").Append(bed.bedId)
                      .Append(" — Crop: ").Append(bed.cropName ?? "Vegetables")
                      .Append(" — Growth: ").Append(bed.growthPercent.ToString("0")).Append("%")
                      .Append(" | Water: ").Append(bed.waterIrrigationLiters.ToString("0.#")).Append(" L");

                    if (bed.isReadyToHarvest) sb.Append("  ✔ [READY TO HARVEST!]");
                    else if (bed.isSunlampPowered) sb.Append("  [GROWING — SUNLAMP ON]");
                    else sb.Append("  ✖ [NO SUNLAMP POWER]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nGREENHOUSE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
