using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GeothermalTurbineSnapshot
    {
        public string turbineId;
        public float steamPressureBar;
        public float temperatureCelsius;
        public float wattageOutput;
        public bool isOperational;
    }

    public class GeothermalSnapshot
    {
        public float totalWattageGenerated;
        public float steamPressureGlobal;
        public List<GeothermalTurbineSnapshot> turbines = new List<GeothermalTurbineSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Geothermal Generator & Steam Turbine HUD view-model.
    /// Monitors deep earth geothermal steam pressure (bar), turbine temperature (°C),
    /// wattage generation output (kW), relief valve venting, and thermal overload prevention.
    /// </summary>
    public class GeothermalHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedTurbineIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGeothermalChanged;
        public event Action<string> OnVentPressureRequested; // (turbineId)

        private Func<GeothermalSnapshot> _getSnapshot;
        private GeothermalSnapshot _snapshot;

        public void Bind(Func<GeothermalSnapshot> getSnapshot)
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

        public bool SelectNextTurbine()
        {
            if (!IsOpen || _snapshot == null || _snapshot.turbines == null || _snapshot.turbines.Count == 0)
                return false;
            SelectedTurbineIndex = (SelectedTurbineIndex + 1) % _snapshot.turbines.Count;
            ReportOutcome("Selected geothermal turbine: " + GetSelectedTurbineName());
            return true;
        }

        public bool SelectPreviousTurbine()
        {
            if (!IsOpen || _snapshot == null || _snapshot.turbines == null || _snapshot.turbines.Count == 0)
                return false;
            SelectedTurbineIndex = (SelectedTurbineIndex - 1 + _snapshot.turbines.Count) % _snapshot.turbines.Count;
            ReportOutcome("Selected geothermal turbine: " + GetSelectedTurbineName());
            return true;
        }

        public bool RequestVentPressure()
        {
            if (!IsOpen || _snapshot == null || _snapshot.turbines == null || _snapshot.turbines.Count == 0)
            {
                ReportOutcome("No turbine selected for steam pressure relief venting.");
                return false;
            }

            var turbine = GetSelectedTurbine();
            if (turbine == null) return false;

            if (OnVentPressureRequested == null)
            {
                ReportOutcome("Geothermal valve controller link offline.");
                return false;
            }

            OnVentPressureRequested.Invoke(turbine.turbineId);
            ReportOutcome("Venting steam pressure from Geothermal Turbine " + turbine.turbineId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No geothermal action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGeothermalChanged?.Invoke();
        }

        private GeothermalTurbineSnapshot GetSelectedTurbine()
        {
            if (_snapshot != null && _snapshot.turbines != null && SelectedTurbineIndex >= 0 && SelectedTurbineIndex < _snapshot.turbines.Count)
            {
                return _snapshot.turbines[SelectedTurbineIndex];
            }
            return null;
        }

        private string GetSelectedTurbineName()
        {
            var t = GetSelectedTurbine();
            return t != null ? t.turbineId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GEOTHERMAL POWER & STEAM TURBINE MONITOR  [E] close  ·  [Tab] cycle  ·  [V] vent steam pressure");

            if (_snapshot == null)
            {
                sb.Append("\nGeothermal telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nPOWER STATS: Wattage Output: ").Append(_snapshot.totalWattageGenerated.ToString("0.#")).Append(" kW")
              .Append("  ·  Global Steam Pressure: ").Append(_snapshot.steamPressureGlobal.ToString("0.0")).Append(" bar");

            sb.Append("\n\nGEOTHERMAL STEAM TURBINES:");
            if (_snapshot.turbines == null || _snapshot.turbines.Count == 0)
            {
                sb.Append("\n  No geothermal turbines connected to power grid.");
            }
            else
            {
                for (int i = 0; i < _snapshot.turbines.Count; i++)
                {
                    var turbine = _snapshot.turbines[i];
                    if (turbine == null) continue;

                    bool selected = (i == SelectedTurbineIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Turbine ").Append(turbine.turbineId)
                      .Append(" — Pressure: ").Append(turbine.steamPressureBar.ToString("0.0")).Append(" bar")
                      .Append(" | Temp: ").Append(turbine.temperatureCelsius.ToString("0")).Append("°C")
                      .Append(" | Output: ").Append(turbine.wattageOutput.ToString("0.#")).Append(" kW");

                    if (turbine.steamPressureBar > 15f) sb.Append("  ★ [CRITICAL OVERPRESSURE — VENT REQUIRED!]");
                    else if (turbine.isOperational) sb.Append("  ✔ [RUNNING]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nGEOTHERMAL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
