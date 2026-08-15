#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class ReinforcedHatchSnapshot
    {
        public float hatchIntegrityPercent; // 0..100
        public float hydraulicSealPressureBar;
        public float armorPlatingThicknessMm;
        public bool isSealed;
        public bool isHydraulicPowerOk;
    }

    /// <summary>
    /// Protocol Zero — Reinforced Blast Hatch & Hydraulic Seal HUD view-model.
    /// Monitors heavy steel blast hatch structural integrity, hydraulic seal pressure (bar),
    /// armor plate thickness (mm), emergency seal locking, and hydraulic fluid leak alerts.
    /// </summary>
    public class ReinforcedHatchHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnReinforcedHatchChanged;
        public event Action OnEngageHydraulicSealRequested;
        public event Action OnReleaseHatchLockRequested;

        private Func<ReinforcedHatchSnapshot> _getSnapshot;
        private ReinforcedHatchSnapshot _snapshot;

        public void Bind(Func<ReinforcedHatchSnapshot> getSnapshot)
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

        public bool RequestEngageHydraulicSeal()
        {
            if (!IsOpen) return false;
            if (OnEngageHydraulicSealRequested == null)
            {
                ReportOutcome("Hydraulic hatch seal controller offline.");
                return false;
            }

            OnEngageHydraulicSealRequested.Invoke();
            ReportOutcome("ENGAGING HYDRAULIC BLAST HATCH SEAL (Pressure Locking)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No reinforced hatch action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnReinforcedHatchChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("REINFORCED BLAST HATCH & HYDRAULIC LOCK  [H] close  ·  [S] engage hydraulic seal");

            if (_snapshot == null)
            {
                sb.Append("\nHatch telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nHATCH TELEMETRY:");
            sb.Append("\n  · Hatch Integrity: ").Append(_snapshot.hatchIntegrityPercent.ToString("0")).Append("%");
            sb.Append("\n  · Hydraulic Pressure: ").Append(_snapshot.hydraulicSealPressureBar.ToString("0.0")).Append(" bar");
            sb.Append("\n  · Armor Plating: ").Append(_snapshot.armorPlatingThicknessMm.ToString("0")).Append(" mm Steel");

            sb.Append("\n\nSEAL STATUS: ");
            if (_snapshot.isSealed)
            {
                sb.Append("✔ [HYDRAULICALLY SEALED & PRESSURE LOCKED]");
            }
            else
            {
                sb.Append("★ [UNSEALED / MANUAL UNLOCK]");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nHATCH LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
