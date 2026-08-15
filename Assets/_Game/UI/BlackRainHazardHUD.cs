using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class BlackRainHazardSnapshot
    {
        public bool isBlackRainActive;
        public float hazmatDegradeMultiplier;
        public float dreadMoraleDrainPerHour;
        public int outdoorExposedSurvivorsCount;
        public float roofRunoffAcidity; // 0..10
    }

    /// <summary>
    /// Protocol Zero — Black Rain Hazard HUD view-model.
    /// Monitors hyper-radioactive oily black rain weather, hazmat suit degradation multipliers,
    /// surface water catchment contamination risk, and outdoor scavenger Dread morale drain.
    /// </summary>
    public class BlackRainHazardHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnBlackRainHazardChanged;
        public event Action OnSealCatchmentRequested;

        private Func<BlackRainHazardSnapshot> _getSnapshot;
        private BlackRainHazardSnapshot _snapshot;

        public void Bind(Func<BlackRainHazardSnapshot> getSnapshot)
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

        public bool RequestSealCatchment()
        {
            if (!IsOpen) return false;
            if (OnSealCatchmentRequested == null)
            {
                ReportOutcome("Roof catchment control valve link offline.");
                return false;
            }

            OnSealCatchmentRequested.Invoke();
            ReportOutcome("SEALING surface water catchment valves to prevent Black Rain contamination!");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No Black Rain action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnBlackRainHazardChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BLACK RAIN HAZARD MONITOR  [X] close  ·  [C] seal catchment valves");

            if (_snapshot == null)
            {
                sb.Append("\nBlack Rain atmospheric sensor offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nATMOSPHERIC STATUS: ");
            if (_snapshot.isBlackRainActive)
            {
                sb.Append("[CRITICAL: OILY BLACK RAIN FALLOUT DOWNPOUR IN PROGRESS!]");
                sb.Append("\n  · Hazmat Degrade Rate: ").Append(_snapshot.hazmatDegradeMultiplier.ToString("0.0")).Append("x faster");
                sb.Append("\n  · Dread Morale Drain: -").Append(_snapshot.dreadMoraleDrainPerHour.ToString("0.#")).Append(" morale/hr");
                sb.Append("\n  · Outdoor Exposed Survivors: ").Append(_snapshot.outdoorExposedSurvivorsCount);
                sb.Append("\n  · Roof Runoff Acidity: pH ").Append((7.0f - _snapshot.roofRunoffAcidity).ToString("0.0"));
            }
            else
            {
                sb.Append("[CLEAR - Normal atmospheric precipitation]");
                sb.Append("\n  · Hazmat Degrade Rate: 1.0x (Standard)");
                sb.Append("\n  · Dread Morale Drain: None");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nHAZARD LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
