using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class BicycleUnitSnapshot
    {
        public string bicycleId;
        public string assignedSurvivorId;
        public float durability; // 0..40
        public float maxDurability; // 40
        public bool isBroken;
        public float speedMultiplier; // 0.5 = 2x speed
    }

    public class BicycleLogisticsSnapshot
    {
        public int totalBicyclesInService;
        public int totalTirePatchKitsAvailable;
        public List<BicycleUnitSnapshot> bicycles = new List<BicycleUnitSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Bicycle Logistics & Maintenance HUD view-model.
    /// Monitors expedition bicycles, 2x fast travel speed multiplier, durability breakdown tracking,
    /// tire patch kit repairs (+25 durability per kit), and walking back broken bikes.
    /// </summary>
    public class BicycleLogisticsHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedBikeIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnBicycleLogisticsChanged;
        public event Action<string> OnRepairBicycleRequested; // (bicycleId)

        private Func<BicycleLogisticsSnapshot> _getSnapshot;
        private BicycleLogisticsSnapshot _snapshot;

        public void Bind(Func<BicycleLogisticsSnapshot> getSnapshot)
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

        public bool SelectNextBike()
        {
            if (!IsOpen || _snapshot == null || _snapshot.bicycles == null || _snapshot.bicycles.Count == 0)
                return false;
            SelectedBikeIndex = (SelectedBikeIndex + 1) % _snapshot.bicycles.Count;
            ReportOutcome("Selected bicycle unit: " + GetSelectedBikeName());
            return true;
        }

        public bool SelectPreviousBike()
        {
            if (!IsOpen || _snapshot == null || _snapshot.bicycles == null || _snapshot.bicycles.Count == 0)
                return false;
            SelectedBikeIndex = (SelectedBikeIndex - 1 + _snapshot.bicycles.Count) % _snapshot.bicycles.Count;
            ReportOutcome("Selected bicycle unit: " + GetSelectedBikeName());
            return true;
        }

        public bool RequestRepairBike()
        {
            if (!IsOpen || _snapshot == null || _snapshot.bicycles == null || _snapshot.bicycles.Count == 0)
            {
                ReportOutcome("No bicycle unit selected for repair.");
                return false;
            }

            var bike = GetSelectedBike();
            if (bike == null) return false;

            if (_snapshot != null && _snapshot.totalTirePatchKitsAvailable <= 0)
            {
                ReportOutcome("CANNOT REPAIR: No Tire Patch Kits available in inventory!");
                return false;
            }

            if (OnRepairBicycleRequested == null)
            {
                ReportOutcome("Bicycle repair bench link offline.");
                return false;
            }

            OnRepairBicycleRequested.Invoke(bike.bicycleId);
            ReportOutcome("Repairing bicycle " + bike.bicycleId + " with Tire Patch Kit (+25 Durability)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No bicycle action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnBicycleLogisticsChanged?.Invoke();
        }

        private BicycleUnitSnapshot GetSelectedBike()
        {
            if (_snapshot != null && _snapshot.bicycles != null && SelectedBikeIndex >= 0 && SelectedBikeIndex < _snapshot.bicycles.Count)
            {
                return _snapshot.bicycles[SelectedBikeIndex];
            }
            return null;
        }

        private string GetSelectedBikeName()
        {
            var b = GetSelectedBike();
            return b != null ? (b.bicycleId + " [" + (b.assignedSurvivorId ?? "Unassigned") + "]") : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BICYCLE EXPEDITION LOGISTICS  [V] close  ·  [Tab] cycle  ·  [R] repair with tire patch kit");

            if (_snapshot == null)
            {
                sb.Append("\nBicycle logistics telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nLOGISTICS STATS: Bicycles in Service: ").Append(_snapshot.totalBicyclesInService)
              .Append("  ·  Tire Patch Kits: ").Append(_snapshot.totalTirePatchKitsAvailable);

            sb.Append("\n\nSHELTER BICYCLE FLEET:");
            if (_snapshot.bicycles == null || _snapshot.bicycles.Count == 0)
            {
                sb.Append("\n  No bicycles registered in shelter inventory.");
            }
            else
            {
                for (int i = 0; i < _snapshot.bicycles.Count; i++)
                {
                    var bike = _snapshot.bicycles[i];
                    if (bike == null) continue;

                    bool selected = (i == SelectedBikeIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(bike.bicycleId)
                      .Append(" (Assigned: ").Append(bike.assignedSurvivorId ?? "Unassigned").Append(")")
                      .Append(" — Durability: ").Append(bike.durability.ToString("0.#")).Append(" / ").Append(bike.maxDurability.ToString("0")).Append(" hrs");

                    if (bike.isBroken) sb.Append("  ✖ [BROKEN — 50% CARRY CAPACITY & SLOW WALK]");
                    else sb.Append("  ✔ [OPERATIONAL — 2X FAST TRAVEL]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nFLEET LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
