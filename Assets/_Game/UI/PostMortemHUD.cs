using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class AutopsyReportSnapshot
    {
        public string corpseId;
        public string deceasedSurvivorName;
        public string causeOfDeathText;
        public float radiationRadsAtDeath;
        public bool isAutopsyCompleted;
        public float harvestedFatKg;
    }

    public class PostMortemSnapshot
    {
        public int totalAutopsiesCompleted;
        public List<AutopsyReportSnapshot> reports = new List<AutopsyReportSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Post-Mortem Autopsy & Cause of Death HUD view-model.
    /// Monitors corpse pathology investigations, exact cause of death forensics,
    /// tissue radiation accumulation, and medical organ/tallow harvesting logs.
    /// </summary>
    public class PostMortemHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedReportIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPostMortemChanged;
        public event Action<string> OnPerformAutopsyRequested; // (corpseId)

        private Func<PostMortemSnapshot> _getSnapshot;
        private PostMortemSnapshot _snapshot;

        public void Bind(Func<PostMortemSnapshot> getSnapshot)
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

        public bool SelectNextReport()
        {
            if (!IsOpen || _snapshot == null || _snapshot.reports == null || _snapshot.reports.Count == 0)
                return false;
            SelectedReportIndex = (SelectedReportIndex + 1) % _snapshot.reports.Count;
            ReportOutcome("Selected autopsy report: " + GetSelectedReportName());
            return true;
        }

        public bool SelectPreviousReport()
        {
            if (!IsOpen || _snapshot == null || _snapshot.reports == null || _snapshot.reports.Count == 0)
                return false;
            SelectedReportIndex = (SelectedReportIndex - 1 + _snapshot.reports.Count) % _snapshot.reports.Count;
            ReportOutcome("Selected autopsy report: " + GetSelectedReportName());
            return true;
        }

        public bool RequestPerformAutopsy()
        {
            if (!IsOpen || _snapshot == null || _snapshot.reports == null || _snapshot.reports.Count == 0)
            {
                ReportOutcome("No corpse selected for forensic autopsy.");
                return false;
            }

            var report = GetSelectedReport();
            if (report == null) return false;

            if (report.isAutopsyCompleted)
            {
                ReportOutcome("Autopsy on " + report.deceasedSurvivorName + " is already completed.");
                return false;
            }

            if (OnPerformAutopsyRequested == null)
            {
                ReportOutcome("Morgue autopsy table link offline.");
                return false;
            }

            OnPerformAutopsyRequested.Invoke(report.corpseId);
            ReportOutcome("Performing forensic autopsy on " + report.deceasedSurvivorName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No autopsy action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPostMortemChanged?.Invoke();
        }

        private AutopsyReportSnapshot GetSelectedReport()
        {
            if (_snapshot != null && _snapshot.reports != null && SelectedReportIndex >= 0 && SelectedReportIndex < _snapshot.reports.Count)
            {
                return _snapshot.reports[SelectedReportIndex];
            }
            return null;
        }

        private string GetSelectedReportName()
        {
            var r = GetSelectedReport();
            return r != null ? (r.deceasedSurvivorName ?? r.corpseId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("MORGUE FORENSIC AUTOPSY & POST-MORTEM  [P] close  ·  [Tab] cycle  ·  [A] perform autopsy");

            if (_snapshot == null)
            {
                sb.Append("\nMorgue pathology telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nMORGUE STATS: Total Autopsies Completed: ").Append(_snapshot.totalAutopsiesCompleted);

            sb.Append("\n\nSHELTER DECEASED AUTOPSY RECORDS:");
            if (_snapshot.reports == null || _snapshot.reports.Count == 0)
            {
                sb.Append("\n  No deceased corpses awaiting autopsy in morgue.");
            }
            else
            {
                for (int i = 0; i < _snapshot.reports.Count; i++)
                {
                    var report = _snapshot.reports[i];
                    if (report == null) continue;

                    bool selected = (i == SelectedReportIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(report.deceasedSurvivorName ?? report.corpseId)
                      .Append(" — Rads: ").Append(report.radiationRadsAtDeath.ToString("0")).Append(" rads");

                    if (report.isAutopsyCompleted)
                    {
                        sb.Append("  ✔ [AUTOPSY COMPLETE — CAUSE: ").Append(report.causeOfDeathText ?? "Unknown").Append("]");
                    }
                    else
                    {
                        sb.Append("  ★ [AWAITING FORENSIC AUTOPSY]");
                    }
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nMORGUE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
