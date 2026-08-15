using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GarrisonPatrolSnapshot
    {
        public string patrolId;
        public string sectorId;
        public int threatLevel; // 1..5
        public bool isHostile;
        public int patrolStrength;
    }

    public class GarrisonEscalationSnapshot
    {
        public int escalationLevel; // 1..5
        public float alertMeterPercent; // 0..100
        public List<GarrisonPatrolSnapshot> patrols = new List<GarrisonPatrolSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Garrison Escalation & Armed Patrol Threat HUD view-model.
    /// Monitors military garrison escalation levels, alert meters, armed patrol frequency,
    /// threat escalation triggers, and bribe/tribute negotiations.
    /// </summary>
    public class GarrisonEscalationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedPatrolIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGarrisonEscalationChanged;
        public event Action<string> OnPayTributeRequested; // (patrolId)

        private Func<GarrisonEscalationSnapshot> _getSnapshot;
        private GarrisonEscalationSnapshot _snapshot;

        public void Bind(Func<GarrisonEscalationSnapshot> getSnapshot)
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

        public bool SelectNextPatrol()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patrols == null || _snapshot.patrols.Count == 0)
                return false;
            SelectedPatrolIndex = (SelectedPatrolIndex + 1) % _snapshot.patrols.Count;
            ReportOutcome("Selected garrison patrol: " + GetSelectedPatrolName());
            return true;
        }

        public bool SelectPreviousPatrol()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patrols == null || _snapshot.patrols.Count == 0)
                return false;
            SelectedPatrolIndex = (SelectedPatrolIndex - 1 + _snapshot.patrols.Count) % _snapshot.patrols.Count;
            ReportOutcome("Selected garrison patrol: " + GetSelectedPatrolName());
            return true;
        }

        public bool RequestPayTribute()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patrols == null || _snapshot.patrols.Count == 0)
            {
                ReportOutcome("No garrison patrol selected for tribute payment.");
                return false;
            }

            var patrol = GetSelectedPatrol();
            if (patrol == null) return false;

            if (OnPayTributeRequested == null)
            {
                ReportOutcome("Garrison negotiator link offline.");
                return false;
            }

            OnPayTributeRequested.Invoke(patrol.patrolId);
            ReportOutcome("Paying ammo tribute to Garrison Patrol " + patrol.patrolId + " (De-escalating Alert Meter)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No garrison action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGarrisonEscalationChanged?.Invoke();
        }

        private GarrisonPatrolSnapshot GetSelectedPatrol()
        {
            if (_snapshot != null && _snapshot.patrols != null && SelectedPatrolIndex >= 0 && SelectedPatrolIndex < _snapshot.patrols.Count)
            {
                return _snapshot.patrols[SelectedPatrolIndex];
            }
            return null;
        }

        private string GetSelectedPatrolName()
        {
            var p = GetSelectedPatrol();
            return p != null ? (p.patrolId + " @ " + p.sectorId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GARRISON ESCALATION & PATROL MONITOR  [G] close  ·  [Tab] cycle  ·  [P] pay ammo tribute");

            if (_snapshot == null)
            {
                sb.Append("\nGarrison radar telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nALERT THREAT: Escalation Level ").Append(_snapshot.escalationLevel).Append(" / 5")
              .Append("  ·  Alert Meter: ").Append(_snapshot.alertMeterPercent.ToString("0")).Append("%");

            if (_snapshot.alertMeterPercent > 80f)
                sb.Append("  [CRITICAL ALERT: GARRISON SIEGE INBOUND!]");

            sb.Append("\n\nARMED GARRISON PATROLS IN REGION:");
            if (_snapshot.patrols == null || _snapshot.patrols.Count == 0)
            {
                sb.Append("\n  No active garrison patrols detected in wasteland.");
            }
            else
            {
                for (int i = 0; i < _snapshot.patrols.Count; i++)
                {
                    var patrol = _snapshot.patrols[i];
                    if (patrol == null) continue;

                    bool selected = (i == SelectedPatrolIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Patrol ").Append(patrol.patrolId)
                      .Append(" @ Sector ").Append(patrol.sectorId)
                      .Append(" — Strength: ").Append(patrol.patrolStrength).Append(" soldiers")
                      .Append(" | Threat: Level ").Append(patrol.threatLevel);

                    if (patrol.isHostile) sb.Append("  ★ [HOSTILE - SHOOT ON SIGHT]");
                    else sb.Append("  [NEUTRAL PATROL]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nALERT LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
