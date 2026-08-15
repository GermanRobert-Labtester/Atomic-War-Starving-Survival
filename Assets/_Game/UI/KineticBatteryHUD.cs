using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class KineticFlywheelSnapshot
    {
        public string flywheelId;
        public float rpm;
        public float maxRpm;
        public float energyStoredJoules;
        public float dischargeWattage;
        public bool isSpinning;
    }

    public class KineticBatterySnapshot
    {
        public float totalEnergyStoredJoules;
        public List<KineticFlywheelSnapshot> flywheels = new List<KineticFlywheelSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Kinetic Flywheel Battery Array HUD view-model.
    /// Monitors mechanical flywheel rotational speed (RPM), kinetic energy storage (kJ),
    /// discharge wattage output (W), hand-crank spin acceleration, and mechanical friction loss.
    /// </summary>
    public class KineticBatteryHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedFlywheelIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnKineticBatteryChanged;
        public event Action<string> OnSpinUpFlywheelRequested; // (flywheelId)

        private Func<KineticBatterySnapshot> _getSnapshot;
        private KineticBatterySnapshot _snapshot;

        public void Bind(Func<KineticBatterySnapshot> getSnapshot)
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

        public bool SelectNextFlywheel()
        {
            if (!IsOpen || _snapshot == null || _snapshot.flywheels == null || _snapshot.flywheels.Count == 0)
                return false;
            SelectedFlywheelIndex = (SelectedFlywheelIndex + 1) % _snapshot.flywheels.Count;
            ReportOutcome("Selected flywheel: " + GetSelectedFlywheelName());
            return true;
        }

        public bool SelectPreviousFlywheel()
        {
            if (!IsOpen || _snapshot == null || _snapshot.flywheels == null || _snapshot.flywheels.Count == 0)
                return false;
            SelectedFlywheelIndex = (SelectedFlywheelIndex - 1 + _snapshot.flywheels.Count) % _snapshot.flywheels.Count;
            ReportOutcome("Selected flywheel: " + GetSelectedFlywheelName());
            return true;
        }

        public bool RequestSpinUpFlywheel()
        {
            if (!IsOpen || _snapshot == null || _snapshot.flywheels == null || _snapshot.flywheels.Count == 0)
            {
                ReportOutcome("No flywheel selected for manual spin-up.");
                return false;
            }

            var fw = GetSelectedFlywheel();
            if (fw == null) return false;

            if (OnSpinUpFlywheelRequested == null)
            {
                ReportOutcome("Kinetic flywheel hand-crank link offline.");
                return false;
            }

            OnSpinUpFlywheelRequested.Invoke(fw.flywheelId);
            ReportOutcome("Cranking manual spin-up on Flywheel " + fw.flywheelId + " (+RPM)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No kinetic action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnKineticBatteryChanged?.Invoke();
        }

        private KineticFlywheelSnapshot GetSelectedFlywheel()
        {
            if (_snapshot != null && _snapshot.flywheels != null && SelectedFlywheelIndex >= 0 && SelectedFlywheelIndex < _snapshot.flywheels.Count)
            {
                return _snapshot.flywheels[SelectedFlywheelIndex];
            }
            return null;
        }

        private string GetSelectedFlywheelName()
        {
            var f = GetSelectedFlywheel();
            return f != null ? f.flywheelId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("KINETIC FLYWHEEL BATTERY ARRAY  [K] close  ·  [Tab] cycle  ·  [C] crank spin-up");

            if (_snapshot == null)
            {
                sb.Append("\nKinetic battery telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nBATTERY ARRAY STATS: Stored Kinetic Energy: ").Append((_snapshot.totalEnergyStoredJoules / 1000f).ToString("0.#")).Append(" kJ");

            sb.Append("\n\nMECHANICAL FLYWHEEL UNITS:");
            if (_snapshot.flywheels == null || _snapshot.flywheels.Count == 0)
            {
                sb.Append("\n  No kinetic flywheels installed.");
            }
            else
            {
                for (int i = 0; i < _snapshot.flywheels.Count; i++)
                {
                    var fw = _snapshot.flywheels[i];
                    if (fw == null) continue;

                    bool selected = (i == SelectedFlywheelIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Flywheel ").Append(fw.flywheelId)
                      .Append(" — Rotational Speed: ").Append(fw.rpm.ToString("0")).Append(" / ").Append(fw.maxRpm.ToString("0")).Append(" RPM")
                      .Append(" | Output: ").Append(fw.dischargeWattage.ToString("0.#")).Append(" W");

                    if (fw.isSpinning) sb.Append("  ✔ [SPINNING]");
                    else sb.Append("  [STATIONARY — SPUN DOWN]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nKINETIC LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
