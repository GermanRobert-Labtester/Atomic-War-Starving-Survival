using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class HypoxiaAlertSnapshot
    {
        public float oxygenLevelPercent; // 0..21%
        public float co2PpmLevel; // ppm
        public float coPpmLevel; // ppm
        public float scrubberCartridgeHealthPercent; // 0..100
        public bool isHypoxiaCritical;
        public float hypoxiaMinutesRemaining;
    }

    /// <summary>
    /// Protocol Zero — Hypoxia Alert & Air Quality Monitoring HUD view-model.
    /// Monitors indoor oxygen levels (% O2), CO2/CO poisoning thresholds,
    /// air scrubber cartridge degradation, emergency vent purging, and suffocation alerts.
    /// </summary>
    public class HypoxiaAlertHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHypoxiaAlertChanged;
        public event Action OnReplaceScrubberCartridgeRequested;
        public event Action OnPurgeEmergencyVentRequested;

        private Func<HypoxiaAlertSnapshot> _getSnapshot;
        private HypoxiaAlertSnapshot _snapshot;

        public void Bind(Func<HypoxiaAlertSnapshot> getSnapshot)
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

        public bool RequestReplaceCartridge()
        {
            if (!IsOpen) return false;
            if (OnReplaceScrubberCartridgeRequested == null)
            {
                ReportOutcome("Air scrubber link offline.");
                return false;
            }

            OnReplaceScrubberCartridgeRequested.Invoke();
            ReportOutcome("Replacing Air Scrubber Chemical Cartridge...");
            return true;
        }

        public bool RequestEmergencyPurge()
        {
            if (!IsOpen) return false;
            if (OnPurgeEmergencyVentRequested == null)
            {
                ReportOutcome("Emergency vent purge valve link offline.");
                return false;
            }

            OnPurgeEmergencyVentRequested.Invoke();
            ReportOutcome("PURGING EMERGENCY AIR VENTS (O2 Refreshed, Toxic Gases Cleared!)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No hypoxia action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHypoxiaAlertChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("HYPOXIA ALERT & OXYGEN QUALITY MONITOR  [O] close  ·  [R] replace scrubber  ·  [P] emergency purge");

            if (_snapshot == null)
            {
                sb.Append("\nOxygen gas sensor offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nATMOSPHERE TELEMETRY:");
            sb.Append("\n  · Oxygen (O2): ").Append(_snapshot.oxygenLevelPercent.ToString("0.0")).Append("%  [Normal: 20.9%]");
            sb.Append("\n  · Carbon Dioxide (CO2): ").Append(_snapshot.co2PpmLevel.ToString("0")).Append(" ppm");
            sb.Append("\n  · Carbon Monoxide (CO): ").Append(_snapshot.coPpmLevel.ToString("0")).Append(" ppm");
            sb.Append("\n  · Air Scrubber Cartridge: ").Append(_snapshot.scrubberCartridgeHealthPercent.ToString("0")).Append("%");

            sb.Append("\n\nHYPOXIA STATUS: ");
            if (_snapshot.isHypoxiaCritical)
            {
                sb.Append("★ [CRITICAL HYPOXIA ALERT: SUFFOCATION IN ").Append(_snapshot.hypoxiaMinutesRemaining.ToString("0.#")).Append(" MINUTES!]");
            }
            else
            {
                sb.Append("✔ [ATMOSPHERE BREATHABLE]");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nATMOSPHERE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
