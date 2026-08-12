using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GhostSentrySnapshot
    {
        public string sentryId;
        public string locationNodeId;
        public float ammoBeltDurability; // 0..100
        public float barrelHeat;
        public bool isActive;
        public bool burnedOut;
        public float roundsFired;
    }

    public class AutomatedThreatSnapshot
    {
        public int totalSentriesBurnedOut;
        public int totalMunitionsAttracted;
        public int totalDecoysDeployed;
        public List<GhostSentrySnapshot> sentries = new List<GhostSentrySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Automated Threat & Ghost Sentry HUD view-model.
    /// Monitors rusted pneumatic machine-gun sentry emplacements, ammo belt durability,
    /// barrel overheat cook-off mechanics, acoustic decoy deployment, and loitering
    /// suicide drone threat telemetry.
    /// </summary>
    public class AutomatedThreatHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSentryIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnAutomatedThreatChanged;
        public event Action<string> OnDeployDecoyRequested;     // sentryId
        public event Action<string> OnScavengeSentryRequested;  // sentryId

        private Func<AutomatedThreatSnapshot> _getSnapshot;
        private AutomatedThreatSnapshot _snapshot;

        public void Bind(Func<AutomatedThreatSnapshot> getSnapshot)
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

        public bool SelectNextSentry()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sentries == null || _snapshot.sentries.Count == 0)
                return false;
            SelectedSentryIndex = (SelectedSentryIndex + 1) % _snapshot.sentries.Count;
            ReportOutcome("Selected sentry target: " + GetSelectedSentryName());
            return true;
        }

        public bool SelectPreviousSentry()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sentries == null || _snapshot.sentries.Count == 0)
                return false;
            SelectedSentryIndex = (SelectedSentryIndex - 1 + _snapshot.sentries.Count) % _snapshot.sentries.Count;
            ReportOutcome("Selected sentry target: " + GetSelectedSentryName());
            return true;
        }

        public bool RequestDeployDecoy()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sentries == null || _snapshot.sentries.Count == 0)
            {
                ReportOutcome("No target sentry selected for decoy deployment.");
                return false;
            }

            var sentry = GetSelectedSentry();
            if (sentry == null) return false;

            if (!sentry.isActive || sentry.burnedOut)
            {
                ReportOutcome("Selected sentry is already disabled/burned out.");
                return false;
            }

            if (OnDeployDecoyRequested == null)
            {
                ReportOutcome("Acoustic decoy launcher link offline.");
                return false;
            }

            OnDeployDecoyRequested.Invoke(sentry.sentryId);
            ReportOutcome("Deploying acoustic decoy against Sentry " + sentry.sentryId + " (forcing ammo belt burn)...");
            return true;
        }

        public bool RequestScavengeSentry()
        {
            if (!IsOpen || _snapshot == null || _snapshot.sentries == null || _snapshot.sentries.Count == 0)
            {
                ReportOutcome("No sentry selected for scavenging.");
                return false;
            }

            var sentry = GetSelectedSentry();
            if (sentry == null) return false;

            if (sentry.isActive && !sentry.burnedOut)
            {
                ReportOutcome("CANNOT SCAVENGE: Sentry is active and hostile! Burn out ammo belt first.");
                return false;
            }

            if (OnScavengeSentryRequested == null)
            {
                ReportOutcome("Scavenging kit link offline.");
                return false;
            }

            OnScavengeSentryRequested.Invoke(sentry.sentryId);
            ReportOutcome("Scavenging tactical scrap from disabled Sentry " + sentry.sentryId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No sentry action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnAutomatedThreatChanged?.Invoke();
        }

        private GhostSentrySnapshot GetSelectedSentry()
        {
            if (_snapshot != null && _snapshot.sentries != null && SelectedSentryIndex >= 0 && SelectedSentryIndex < _snapshot.sentries.Count)
            {
                return _snapshot.sentries[SelectedSentryIndex];
            }
            return null;
        }

        private string GetSelectedSentryName()
        {
            var s = GetSelectedSentry();
            return s != null ? s.sentryId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("AUTOMATED GHOST SENTRIES  [T] close  ·  [Tab] cycle  ·  [D] deploy decoy  ·  [S] scavenge scrap");

            if (_snapshot == null)
            {
                sb.Append("\nSentry threat radar telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nRADAR STATS: Sentries Neutralized: ").Append(_snapshot.totalSentriesBurnedOut)
              .Append("  ·  Acoustic Decoys Deployed: ").Append(_snapshot.totalDecoysDeployed)
              .Append("  ·  Loitering Munitions Tracked: ").Append(_snapshot.totalMunitionsAttracted);

            sb.Append("\n\nGHOST SENTRY EMPLACEMENTS IN SECTOR:");
            if (_snapshot.sentries == null || _snapshot.sentries.Count == 0)
            {
                sb.Append("\n  No active automated sentry emplacements detected in current sector.");
            }
            else
            {
                for (int i = 0; i < _snapshot.sentries.Count; i++)
                {
                    var sentry = _snapshot.sentries[i];
                    if (sentry == null) continue;

                    bool selected = (i == SelectedSentryIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Sentry ").Append(sentry.sentryId)
                      .Append(" @ ").Append(sentry.locationNodeId)
                      .Append(" — Belt Health: ").Append(sentry.ammoBeltDurability.ToString("0")).Append("%")
                      .Append(" | Barrel Heat: ").Append(sentry.barrelHeat.ToString("0")).Append("°C");

                    if (sentry.burnedOut) sb.Append("  ✔ [BURNED OUT / SAFE TO SCAVENGE]");
                    else if (sentry.isActive) sb.Append("  ★ [HOSTILE - ACTIVE TARGETING]");
                    else sb.Append("  [JAMMED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nSENTRY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
