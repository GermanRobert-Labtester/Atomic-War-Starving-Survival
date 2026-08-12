using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class RefrigerationCompressorSnapshot
    {
        public string compressorId;
        public float temperatureCelsius;
        public float coolantPressurePsi;
        public float preservationMultiplier; // e.g. 4.0x shelf life
        public bool isCompressorRunning;
        public bool isLowFreon;
    }

    public class RefrigerationSnapshot
    {
        public float coldRoomTemperatureCelsius;
        public List<RefrigerationCompressorSnapshot> compressors = new List<RefrigerationCompressorSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Cold Room Refrigeration Compressor HUD view-model.
    /// Monitors cold storage room temperature (°C), Freon coolant pressure (PSI),
    /// food preservation shelf-life multipliers (4x rot slowdown), freon recharge, and compressor overhauls.
    /// </summary>
    public class RefrigerationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedCompressorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnRefrigerationChanged;
        public event Action<string> OnRechargeFreonRequested; // (compressorId)

        private Func<RefrigerationSnapshot> _getSnapshot;
        private RefrigerationSnapshot _snapshot;

        public void Bind(Func<RefrigerationSnapshot> getSnapshot)
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

        public bool SelectNextCompressor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.compressors == null || _snapshot.compressors.Count == 0)
                return false;
            SelectedCompressorIndex = (SelectedCompressorIndex + 1) % _snapshot.compressors.Count;
            ReportOutcome("Selected refrigeration compressor: " + GetSelectedCompressorName());
            return true;
        }

        public bool SelectPreviousCompressor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.compressors == null || _snapshot.compressors.Count == 0)
                return false;
            SelectedCompressorIndex = (SelectedCompressorIndex - 1 + _snapshot.compressors.Count) % _snapshot.compressors.Count;
            ReportOutcome("Selected refrigeration compressor: " + GetSelectedCompressorName());
            return true;
        }

        public bool RequestRechargeFreon()
        {
            if (!IsOpen || _snapshot == null || _snapshot.compressors == null || _snapshot.compressors.Count == 0)
            {
                ReportOutcome("No compressor selected for Freon coolant recharge.");
                return false;
            }

            var comp = GetSelectedCompressor();
            if (comp == null) return false;

            if (OnRechargeFreonRequested == null)
            {
                ReportOutcome("Refrigeration Freon valve link offline.");
                return false;
            }

            OnRechargeFreonRequested.Invoke(comp.compressorId);
            ReportOutcome("Recharging Freon Coolant into Refrigeration Compressor " + comp.compressorId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No refrigeration action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnRefrigerationChanged?.Invoke();
        }

        private RefrigerationCompressorSnapshot GetSelectedCompressor()
        {
            if (_snapshot != null && _snapshot.compressors != null && SelectedCompressorIndex >= 0 && SelectedCompressorIndex < _snapshot.compressors.Count)
            {
                return _snapshot.compressors[SelectedCompressorIndex];
            }
            return null;
        }

        private string GetSelectedCompressorName()
        {
            var c = GetSelectedCompressor();
            return c != null ? c.compressorId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("COLD ROOM REFRIGERATION & COOLANT MONITOR  [R] close  ·  [Tab] cycle  ·  [F] recharge freon");

            if (_snapshot == null)
            {
                sb.Append("\nRefrigeration telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nCOLD STORAGE STATS: Cold Room Temp: ").Append(_snapshot.coldRoomTemperatureCelsius.ToString("0.0")).Append("°C");

            sb.Append("\n\nREFRIGERATION COMPRESSOR UNITS:");
            if (_snapshot.compressors == null || _snapshot.compressors.Count == 0)
            {
                sb.Append("\n  No refrigeration compressors installed.");
            }
            else
            {
                for (int i = 0; i < _snapshot.compressors.Count; i++)
                {
                    var comp = _snapshot.compressors[i];
                    if (comp == null) continue;

                    bool selected = (i == SelectedCompressorIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Compressor ").Append(comp.compressorId)
                      .Append(" — Temp: ").Append(comp.temperatureCelsius.ToString("0.0")).Append("°C")
                      .Append(" | Pressure: ").Append(comp.coolantPressurePsi.ToString("0")).Append(" PSI")
                      .Append(" | Preservation: ").Append(comp.preservationMultiplier.ToString("0.#")).Append("x shelf-life");

                    if (comp.isLowFreon) sb.Append("  ★ [LOW FREON COOLANT — RECHARGE REQUIRED]");
                    else if (comp.isCompressorRunning) sb.Append("  ✔ [RUNNING]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nREFRIGERATION LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
