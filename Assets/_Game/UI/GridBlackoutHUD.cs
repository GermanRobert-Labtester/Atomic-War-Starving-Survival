using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class ElectricalSubcircuitSnapshot
    {
        public string circuitId;
        public string circuitName;
        public float powerLoadWatts;
        public bool isTripped;
        public bool isCritical;
    }

    public class GridBlackoutSnapshot
    {
        public bool isGridBlackedOut;
        public float totalGridWattageLoad;
        public float maxGridCapacityWatts;
        public List<ElectricalSubcircuitSnapshot> subcircuits = new List<ElectricalSubcircuitSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Grid Blackout & Circuit Breaker HUD view-model.
    /// Monitors shelter electrical grid overload, emergency fuse tripping, generator capacity,
    /// blackout alerts, and subcircuit breaker reset operations.
    /// </summary>
    public class GridBlackoutHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedCircuitIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGridBlackoutChanged;
        public event Action<string> OnResetBreakerRequested; // (circuitId)

        private Func<GridBlackoutSnapshot> _getSnapshot;
        private GridBlackoutSnapshot _snapshot;

        public void Bind(Func<GridBlackoutSnapshot> getSnapshot)
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

        public bool SelectNextCircuit()
        {
            if (!IsOpen || _snapshot == null || _snapshot.subcircuits == null || _snapshot.subcircuits.Count == 0)
                return false;
            SelectedCircuitIndex = (SelectedCircuitIndex + 1) % _snapshot.subcircuits.Count;
            ReportOutcome("Selected subcircuit: " + GetSelectedCircuitName());
            return true;
        }

        public bool SelectPreviousCircuit()
        {
            if (!IsOpen || _snapshot == null || _snapshot.subcircuits == null || _snapshot.subcircuits.Count == 0)
                return false;
            SelectedCircuitIndex = (SelectedCircuitIndex - 1 + _snapshot.subcircuits.Count) % _snapshot.subcircuits.Count;
            ReportOutcome("Selected subcircuit: " + GetSelectedCircuitName());
            return true;
        }

        public bool RequestResetBreaker()
        {
            if (!IsOpen || _snapshot == null || _snapshot.subcircuits == null || _snapshot.subcircuits.Count == 0)
            {
                ReportOutcome("No subcircuit selected for breaker reset.");
                return false;
            }

            var sub = GetSelectedCircuit();
            if (sub == null) return false;

            if (OnResetBreakerRequested == null)
            {
                ReportOutcome("Electrical breaker box link offline.");
                return false;
            }

            OnResetBreakerRequested.Invoke(sub.circuitId);
            ReportOutcome("Resetting electrical breaker for subcircuit [" + sub.circuitName + "]...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No blackout action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGridBlackoutChanged?.Invoke();
        }

        private ElectricalSubcircuitSnapshot GetSelectedCircuit()
        {
            if (_snapshot != null && _snapshot.subcircuits != null && SelectedCircuitIndex >= 0 && SelectedCircuitIndex < _snapshot.subcircuits.Count)
            {
                return _snapshot.subcircuits[SelectedCircuitIndex];
            }
            return null;
        }

        private string GetSelectedCircuitName()
        {
            var c = GetSelectedCircuit();
            return c != null ? c.circuitName : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("MAIN ELECTRICAL GRID & CIRCUIT BREAKER  [B] close  ·  [Tab] cycle  ·  [R] reset circuit breaker");

            if (_snapshot == null)
            {
                sb.Append("\nElectrical grid telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nGRID STATUS: ");
            if (_snapshot.isGridBlackedOut)
            {
                sb.Append("[CRITICAL BLACKOUT: TOTAL SHELTER POWER LOSS!]");
            }
            else
            {
                sb.Append("[GRID ONLINE — Load: ").Append(_snapshot.totalGridWattageLoad.ToString("0.#")).Append(" / ").Append(_snapshot.maxGridCapacityWatts.ToString("0.#")).Append(" W]");
            }

            sb.Append("\n\nELECTRICAL SUBCIRCUITS:");
            if (_snapshot.subcircuits == null || _snapshot.subcircuits.Count == 0)
            {
                sb.Append("\n  No subcircuits registered on breaker panel.");
            }
            else
            {
                for (int i = 0; i < _snapshot.subcircuits.Count; i++)
                {
                    var circuit = _snapshot.subcircuits[i];
                    if (circuit == null) continue;

                    bool selected = (i == SelectedCircuitIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(circuit.circuitName ?? circuit.circuitId)
                      .Append(" — Load: ").Append(circuit.powerLoadWatts.ToString("0.#")).Append(" W");

                    if (circuit.isTripped) sb.Append("  ✖ [FUSE TRIPPED — NEEDS RESET]");
                    else sb.Append("  ✔ [ENERGIZED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nGRID LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
