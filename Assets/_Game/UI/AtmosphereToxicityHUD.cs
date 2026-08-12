using System;
using System.Text;
using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Protocol Zero — Atmosphere Toxicity HUD view-model.
    /// Monitors sealed shelter atmospheric health: O2 percentage, CO2 ppm levels
    /// (hypoxia/asphyxiation risk thresholds), Carbon Monoxide CO ppm levels,
    /// scrubber cartridge runtime, and hatch ventilation controls.
    /// </summary>
    public class AtmosphereToxicityHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnAtmosphereChanged;
        public event Action OnToggleVentilationRequested;
        public event Action OnToggleHatchVentRequested;
        public event Action OnInstallScrubberRequested;

        private Func<AtmosphereToxicitySave> _getSave;
        private AtmosphereToxicitySave _save;

        public void Bind(Func<AtmosphereToxicitySave> getSave)
        {
            _getSave = getSave;
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

        public bool RequestToggleVentilation()
        {
            if (!IsOpen) return false;
            if (OnToggleVentilationRequested == null)
            {
                ReportOutcome("Ventilation control system offline.");
                return false;
            }
            OnToggleVentilationRequested.Invoke();
            ReportOutcome("Toggled shelter ventilation intake.");
            return true;
        }

        public bool RequestToggleHatchVent()
        {
            if (!IsOpen) return false;
            if (OnToggleHatchVentRequested == null)
            {
                ReportOutcome("Bunker hatch vent release mechanism offline.");
                return false;
            }
            OnToggleHatchVentRequested.Invoke();
            ReportOutcome("Emergency hatch vent toggled (heat loss warning!).");
            return true;
        }

        public bool RequestInstallScrubber()
        {
            if (!IsOpen) return false;
            if (OnInstallScrubberRequested == null)
            {
                ReportOutcome("Scrubber installation mechanism offline.");
                return false;
            }
            OnInstallScrubberRequested.Invoke();
            ReportOutcome("Installing CO2 scrubber cartridge...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No atmospheric adjustment recorded." : message;
            Refresh();
        }

        public void Refresh()
        {
            _save = _getSave != null ? _getSave() : null;
            RebuildPanel();
            OnAtmosphereChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("ATMOSPHERE & TOXICITY MONITOR  [V] close  ·  [1] toggle vent  ·  [2] hatch vent  ·  [3] scrubber");

            if (_save == null)
            {
                sb.Append("\nAtmosphere gas analyzer telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            // Gas telemetry lines
            sb.Append("\nGAS TELEMETRY:");

            // O2 status
            sb.Append("\n  O2 LEVEL: ").Append(_save.o2Percent.ToString("0.#")).Append("%");
            if (_save.o2Percent < AtmosphereToxicitySystem.O2LowThresholdPercent)
                sb.Append("  [CRITICAL: LOW OXYGEN]");

            // CO2 status
            sb.Append("\n  CO2 LEVEL: ").Append(_save.co2Ppm.ToString("0")).Append(" ppm");
            if (_save.co2Ppm >= AtmosphereToxicitySystem.AsphyxiationThresholdPpm)
                sb.Append("  [DANGER: ASPHYXIATION — FATAL IN SLEEP]");
            else if (_save.co2Ppm >= AtmosphereToxicitySystem.HypoxiaThresholdPpm)
                sb.Append("  [WARNING: HYPOXIA — CRAFTING PENALTY & HALLUCINATIONS]");

            // CO status
            sb.Append("\n  CO LEVEL: ").Append(_save.coPpm.ToString("0")).Append(" ppm");
            if (_save.coPpm >= AtmosphereToxicitySystem.COThresholdPpm)
                sb.Append("  [WARNING: CARBON MONOXIDE POISONING]");

            // Filtration / Systems status
            sb.Append("\n\nVENTILATION & SCRUBBERS:");
            sb.Append("\n  Ventilation Fan: ").Append(_save.ventilationActive ? "[ACTIVE]" : "[OFF - CO2 ACCUMULATING]");
            sb.Append("\n  Hatch Seal: ").Append(_save.hatchSealed ? "[SEALED]" : "[VENTING - HEAT LOSS -20%]");
            sb.Append("\n  CO2 Scrubbers Installed: ").Append(_save.scrubberCartridgesInstalled);
            if (_save.scrubberCartridgesInstalled > 0)
            {
                sb.Append(" (").Append(_save.scrubberHoursRemaining.ToString("0.#")).Append(" hrs remaining)");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nLOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
