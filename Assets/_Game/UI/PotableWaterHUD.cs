using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class PotableWaterSnapshot
    {
        public float cleanWaterLiters;
        public float irradiatedWaterLiters;
        public float charcoalFilterHealthPercent; // 0..100
        public bool boilingStoveStatus;
        public float distillationRateLitersPerHr;
    }

    /// <summary>
    /// Protocol Zero — Potable Water Distillation & Charcoal Filter HUD view-model.
    /// Monitors clean water storage vs raw irradiated runoff, charcoal filter cartridge health,
    /// solar still distillation rates (L/hr), boiling stove status, and dehydration prevention.
    /// </summary>
    public class PotableWaterHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPotableWaterChanged;
        public event Action OnReplaceCharcoalFilterRequested;
        public event Action OnStartBoilingWaterRequested;

        private Func<PotableWaterSnapshot> _getSnapshot;
        private PotableWaterSnapshot _snapshot;

        public void Bind(Func<PotableWaterSnapshot> getSnapshot)
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

        public bool RequestReplaceFilter()
        {
            if (!IsOpen) return false;
            if (OnReplaceCharcoalFilterRequested == null)
            {
                ReportOutcome("Water filter bench link offline.");
                return false;
            }

            OnReplaceCharcoalFilterRequested.Invoke();
            ReportOutcome("Replacing Charcoal Water Filter Cartridge...");
            return true;
        }

        public bool RequestStartBoiling()
        {
            if (!IsOpen) return false;
            if (OnStartBoilingWaterRequested == null)
            {
                ReportOutcome("Water boiling stove link offline.");
                return false;
            }

            OnStartBoilingWaterRequested.Invoke();
            ReportOutcome("Boiling irradiated water to produce clean potable water...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No water action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPotableWaterChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("POTABLE WATER PURIFICATION & DISTILLATION  [W] close  ·  [R] replace charcoal filter  ·  [B] boil water");

            if (_snapshot == null)
            {
                sb.Append("\nWater purification telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nWATER RESERVOIR STOCKS:");
            sb.Append("\n  · Potable Clean Water: ").Append(_snapshot.cleanWaterLiters.ToString("0.#")).Append(" L");
            sb.Append("\n  · Irradiated Raw Water: ").Append(_snapshot.irradiatedWaterLiters.ToString("0.#")).Append(" L");
            sb.Append("\n  · Charcoal Filter Health: ").Append(_snapshot.charcoalFilterHealthPercent.ToString("0")).Append("%");
            sb.Append("\n  · Solar Distillation Rate: ").Append(_snapshot.distillationRateLitersPerHr.ToString("0.#")).Append(" L/hr");

            sb.Append("\n\nBOILING STOVE: ");
            if (_snapshot.boilingStoveStatus)
            {
                sb.Append("[BOILING WATER — PURIFYING RUNOFF]");
            }
            else
            {
                sb.Append("[STOVE IDLE]");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nWATER LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
