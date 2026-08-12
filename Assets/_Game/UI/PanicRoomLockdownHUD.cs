using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class PanicRoomLockdownSnapshot
    {
        public bool isLockdownActive;
        public float lockdownMinutesRemaining;
        public float panicRoomOxygenReservePercent; // 0..100
        public int occupantsCount;
        public bool isReinforced;
    }

    /// <summary>
    /// Protocol Zero — Emergency Panic Room & Shelter Lockdown HUD view-model.
    /// Monitors panic room blast door seal status, emergency lockdown timers,
    /// dedicated oxygen reserve cylinder levels, occupant capacity, and manual override seals.
    /// </summary>
    public class PanicRoomLockdownHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPanicRoomLockdownChanged;
        public event Action OnTriggerLockdownRequested;
        public event Action OnLiftLockdownRequested;

        private Func<PanicRoomLockdownSnapshot> _getSnapshot;
        private PanicRoomLockdownSnapshot _snapshot;

        public void Bind(Func<PanicRoomLockdownSnapshot> getSnapshot)
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

        public bool RequestToggleLockdown()
        {
            if (!IsOpen) return false;
            if (_snapshot != null && _snapshot.isLockdownActive)
            {
                if (OnLiftLockdownRequested != null)
                {
                    OnLiftLockdownRequested.Invoke();
                    ReportOutcome("Lifting Panic Room Emergency Lockdown...");
                    return true;
                }
            }
            else
            {
                if (OnTriggerLockdownRequested != null)
                {
                    OnTriggerLockdownRequested.Invoke();
                    ReportOutcome("SEALING PANIC ROOM BLAST DOORS — EMERGENCY LOCKDOWN ENGAGED!");
                    return true;
                }
            }

            ReportOutcome("Panic room blast door controller offline.");
            return false;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No panic room action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPanicRoomLockdownChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("EMERGENCY PANIC ROOM & LOCKDOWN CONTROL  [P] close  ·  [L] toggle lockdown");

            if (_snapshot == null)
            {
                sb.Append("\nPanic room telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nLOCKDOWN STATUS: ");
            if (_snapshot.isLockdownActive)
            {
                sb.Append("★ [SEALED — LOCKDOWN ACTIVE FOR ").Append(_snapshot.lockdownMinutesRemaining.ToString("0.#")).Append(" MIN]");
                sb.Append("\n  · Occupants Sealed Inside: ").Append(_snapshot.occupantsCount);
                sb.Append("\n  · Emergency O2 Reserve: ").Append(_snapshot.panicRoomOxygenReservePercent.ToString("0")).Append("%");
            }
            else
            {
                sb.Append("✔ [UNLOCKED & STANDBY]");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nLOCKDOWN LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
