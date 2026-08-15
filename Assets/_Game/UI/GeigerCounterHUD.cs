using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GeigerCounterSnapshot
    {
        public float currentRadLevel; // rads/hr
        public float clickFrequencyHz; // audio click rate
        public bool isGeigerEquipped;
        public float batteryDurability; // 0..100
        public string radZoneDangerName;
    }

    /// <summary>
    /// Protocol Zero — Geiger Counter Audio-Visual Telemetry HUD view-model.
    /// Monitors ambient fallout radiation levels (rads/hr), audio click frequency (Hz),
    /// geiger battery lifespan, audio crackle intensity, and rad zone hazard classification.
    /// </summary>
    public class GeigerCounterHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGeigerCounterChanged;
        public event Action OnToggleGeigerPowerRequested;

        private Func<GeigerCounterSnapshot> _getSnapshot;
        private GeigerCounterSnapshot _snapshot;

        public void Bind(Func<GeigerCounterSnapshot> getSnapshot)
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

        public bool RequestTogglePower()
        {
            if (!IsOpen) return false;
            if (OnToggleGeigerPowerRequested == null)
            {
                ReportOutcome("Geiger counter power switch link offline.");
                return false;
            }

            OnToggleGeigerPowerRequested.Invoke();
            ReportOutcome("Toggling Geiger Counter power state...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No geiger action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGeigerCounterChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GEIGER COUNTER RAD MONITOR  [G] close  ·  [P] toggle power");

            if (_snapshot == null)
            {
                sb.Append("\nGeiger counter sensor offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nDOSIMETER STATUS: ");
            if (_snapshot.isGeigerEquipped)
            {
                sb.Append("[POWERED ON — CLICK FREQ: ").Append(_snapshot.clickFrequencyHz.ToString("0.#")).Append(" Hz]");
                sb.Append("\n  · Ambient Rad Level: ").Append(_snapshot.currentRadLevel.ToString("0.0")).Append(" rads/hr");
                sb.Append("\n  · Zone Hazard: ").Append(_snapshot.radZoneDangerName ?? "Low Radiation");
                sb.Append("\n  · Battery Durability: ").Append(_snapshot.batteryDurability.ToString("0")).Append("%");

                if (_snapshot.currentRadLevel > 50f)
                    sb.Append("\n  ★ [WARNING: LETHAL RAD ZONE — TAKE IODINE / RETREAT!]");
            }
            else
            {
                sb.Append("[POWERED OFF / NOT EQUIPPED]");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nDOSIMETER LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
