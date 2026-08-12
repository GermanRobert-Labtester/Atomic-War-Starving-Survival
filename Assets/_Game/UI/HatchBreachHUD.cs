using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class HatchBreachSnapshot
    {
        public float hatchIntegrityPercent; // 0..100
        public bool isBreachImminent;
        public float breachTimerSeconds;
        public int reinforcementLevel;
        public float lockPlateHealth; // 0..100
    }

    /// <summary>
    /// Protocol Zero — Surface Hatch Breach & Emergency Lock HUD view-model.
    /// Monitors surface blast hatch integrity, raider thermite breach timers,
    /// heavy steel lockplate health, lock welding reinforcement, and seal emergency overrides.
    /// </summary>
    public class HatchBreachHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHatchBreachChanged;
        public event Action OnWeldReinforceHatchRequested;

        private Func<HatchBreachSnapshot> _getSnapshot;
        private HatchBreachSnapshot _snapshot;

        public void Bind(Func<HatchBreachSnapshot> getSnapshot)
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

        public bool RequestWeldReinforce()
        {
            if (!IsOpen) return false;
            if (OnWeldReinforceHatchRequested == null)
            {
                ReportOutcome("Hatch welding torch link offline.");
                return false;
            }

            OnWeldReinforceHatchRequested.Invoke();
            ReportOutcome("WELDING steel plates onto Blast Hatch lockplate (+25 Integrity)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No hatch action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHatchBreachChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SURFACE BLAST HATCH & BREACH MONITOR  [H] close  ·  [W] weld reinforce hatch");

            if (_snapshot == null)
            {
                sb.Append("\nBlast hatch sensors offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nHATCH INTEGRITY: ").Append(_snapshot.hatchIntegrityPercent.ToString("0")).Append("%")
              .Append("  ·  Lockplate Health: ").Append(_snapshot.lockPlateHealth.ToString("0")).Append("%")
              .Append("  ·  Reinforcement Level: ").Append(_snapshot.reinforcementLevel);

            sb.Append("\n\nBREACH STATUS: ");
            if (_snapshot.isBreachImminent)
            {
                sb.Append("[CRITICAL ALERT: THERMITE HATCH BREACH IN PROGRESS — ").Append(_snapshot.breachTimerSeconds.ToString("0.#")).Append("s UNTIL FAILURE!]");
            }
            else
            {
                sb.Append("[HATCH SECURE & SEALED]");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nHATCH LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
