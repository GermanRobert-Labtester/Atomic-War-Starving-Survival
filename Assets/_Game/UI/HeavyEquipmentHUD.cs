using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class HeavyEquipmentUnitSnapshot
    {
        public string equipmentId;
        public string equipmentName;
        public float fuelConsumptionRate;
        public float durabilityPercent; // 0..100
        public bool isOperational;
        public bool isBrokenDown;
    }

    public class HeavyEquipmentSnapshot
    {
        public int totalEquipmentUnits;
        public float fuelAvailable;
        public List<HeavyEquipmentUnitSnapshot> units = new List<HeavyEquipmentUnitSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Heavy Industrial Machinery HUD view-model.
    /// Monitors industrial excavators, diesel generators, crane loaders, fuel consumption rates,
    /// mechanical breakdown hazards, and spare parts repair dispatch.
    /// </summary>
    public class HeavyEquipmentHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedUnitIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHeavyEquipmentChanged;
        public event Action<string> OnRepairEquipmentRequested; // (equipmentId)

        private Func<HeavyEquipmentSnapshot> _getSnapshot;
        private HeavyEquipmentSnapshot _snapshot;

        public void Bind(Func<HeavyEquipmentSnapshot> getSnapshot)
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

        public bool SelectNextUnit()
        {
            if (!IsOpen || _snapshot == null || _snapshot.units == null || _snapshot.units.Count == 0)
                return false;
            SelectedUnitIndex = (SelectedUnitIndex + 1) % _snapshot.units.Count;
            ReportOutcome("Selected machinery unit: " + GetSelectedUnitName());
            return true;
        }

        public bool SelectPreviousUnit()
        {
            if (!IsOpen || _snapshot == null || _snapshot.units == null || _snapshot.units.Count == 0)
                return false;
            SelectedUnitIndex = (SelectedUnitIndex - 1 + _snapshot.units.Count) % _snapshot.units.Count;
            ReportOutcome("Selected machinery unit: " + GetSelectedUnitName());
            return true;
        }

        public bool RequestRepairUnit()
        {
            if (!IsOpen || _snapshot == null || _snapshot.units == null || _snapshot.units.Count == 0)
            {
                ReportOutcome("No heavy machinery selected for repair.");
                return false;
            }

            var unit = GetSelectedUnit();
            if (unit == null) return false;

            if (OnRepairEquipmentRequested == null)
            {
                ReportOutcome("Heavy machinery repair bench link offline.");
                return false;
            }

            OnRepairEquipmentRequested.Invoke(unit.equipmentId);
            ReportOutcome("Repairing heavy machinery [" + unit.equipmentName + "] with spare parts...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No heavy equipment action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHeavyEquipmentChanged?.Invoke();
        }

        private HeavyEquipmentUnitSnapshot GetSelectedUnit()
        {
            if (_snapshot != null && _snapshot.units != null && SelectedUnitIndex >= 0 && SelectedUnitIndex < _snapshot.units.Count)
            {
                return _snapshot.units[SelectedUnitIndex];
            }
            return null;
        }

        private string GetSelectedUnitName()
        {
            var u = GetSelectedUnit();
            return u != null ? u.equipmentName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("HEAVY INDUSTRIAL MACHINERY MONITOR  [M] close  ·  [Tab] cycle  ·  [R] repair machinery");

            if (_snapshot == null)
            {
                sb.Append("\nHeavy machinery telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nMACHINERY STATS: Total Heavy Units: ").Append(_snapshot.totalEquipmentUnits)
              .Append("  ·  Diesel Fuel: ").Append(_snapshot.fuelAvailable.ToString("0.#")).Append(" L");

            sb.Append("\n\nINDUSTRIAL HEAVY MACHINERY FLEET:");
            if (_snapshot.units == null || _snapshot.units.Count == 0)
            {
                sb.Append("\n  No heavy machinery units registered.");
            }
            else
            {
                for (int i = 0; i < _snapshot.units.Count; i++)
                {
                    var unit = _snapshot.units[i];
                    if (unit == null) continue;

                    bool selected = (i == SelectedUnitIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(unit.equipmentName ?? unit.equipmentId)
                      .Append(" — Health: ").Append(unit.durabilityPercent.ToString("0")).Append("%")
                      .Append(" | Fuel Rate: ").Append(unit.fuelConsumptionRate.ToString("0.#")).Append(" L/hr");

                    if (unit.isBrokenDown) sb.Append("  ✖ [MECHANICAL BREAKDOWN — SPARE PARTS REQUIRED]");
                    else if (unit.isOperational) sb.Append("  ✔ [OPERATIONAL]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nMACHINERY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
