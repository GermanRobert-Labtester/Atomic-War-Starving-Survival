using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class ContaminationEntrySnapshot
    {
        public string survivorId;
        public string survivorName;
        public string contaminationType; // e.g. "Thousand Yard Stare", "Disgust Cascade", "Phantom Smell", "Child Cot Trauma"
        public int remainingDays;
        public string[] blockedActions;
    }

    public class PsychologicalContaminationSnapshot
    {
        public int totalActiveContaminations;
        public int totalMentalBreaksTriggered;
        public List<ContaminationEntrySnapshot> entries = new List<ContaminationEntrySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Psychological Contamination HUD view-model.
    /// Monitors survivor trauma contamination carried home from mass graves, abattoirs,
    /// daycare centers, and radiation quarantine zones. Tracks blocked work actions,
    /// mental break risks, and psychological counseling needs.
    /// </summary>
    public class PsychologicalContaminationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedEntryIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPsychologicalContaminationChanged;
        public event Action<string, string> OnCounselSurvivorRequested; // (survivorId, contamType)

        private Func<PsychologicalContaminationSnapshot> _getSnapshot;
        private PsychologicalContaminationSnapshot _snapshot;

        public void Bind(Func<PsychologicalContaminationSnapshot> getSnapshot)
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

        public bool SelectNextEntry()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
                return false;
            SelectedEntryIndex = (SelectedEntryIndex + 1) % _snapshot.entries.Count;
            ReportOutcome("Selected trauma record: " + GetSelectedEntryName());
            return true;
        }

        public bool SelectPreviousEntry()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
                return false;
            SelectedEntryIndex = (SelectedEntryIndex - 1 + _snapshot.entries.Count) % _snapshot.entries.Count;
            ReportOutcome("Selected trauma record: " + GetSelectedEntryName());
            return true;
        }

        public bool RequestCounselSurvivor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                ReportOutcome("No traumatized survivor selected for psychological counseling.");
                return false;
            }

            var entry = GetSelectedEntry();
            if (entry == null) return false;

            if (OnCounselSurvivorRequested == null)
            {
                ReportOutcome("Psychological therapy link offline.");
                return false;
            }

            OnCounselSurvivorRequested.Invoke(entry.survivorId, entry.contaminationType);
            ReportOutcome("Administering trauma counseling to " + entry.survivorName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No trauma log recorded." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPsychologicalContaminationChanged?.Invoke();
        }

        private ContaminationEntrySnapshot GetSelectedEntry()
        {
            if (_snapshot != null && _snapshot.entries != null && SelectedEntryIndex >= 0 && SelectedEntryIndex < _snapshot.entries.Count)
            {
                return _snapshot.entries[SelectedEntryIndex];
            }
            return null;
        }

        private string GetSelectedEntryName()
        {
            var e = GetSelectedEntry();
            return e != null ? (e.survivorName + " (" + e.contaminationType + ")") : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("PSYCHOLOGICAL CONTAMINATION & TRAUMA  [P] close  ·  [Tab] cycle  ·  [C] counsel survivor");

            if (_snapshot == null)
            {
                sb.Append("\nPsychological contamination telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nTRAUMA STATS: Active Contaminations: ").Append(_snapshot.totalActiveContaminations)
              .Append("  ·  Mental Breaks Triggered: ").Append(_snapshot.totalMentalBreaksTriggered);

            sb.Append("\n\nACTIVE SURVIVOR TRAUMA CONTAMINATIONS:");
            if (_snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                sb.Append("\n  No active psychological contaminations recorded in shelter.");
            }
            else
            {
                for (int i = 0; i < _snapshot.entries.Count; i++)
                {
                    var entry = _snapshot.entries[i];
                    if (entry == null) continue;

                    bool selected = (i == SelectedEntryIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(entry.survivorName ?? entry.survivorId)
                      .Append(" — ").Append(entry.contaminationType)
                      .Append(" (").Append(entry.remainingDays).Append(" days remaining)");

                    if (entry.blockedActions != null && entry.blockedActions.Length > 0)
                    {
                        sb.Append("\n    [BLOCKED WORK ACTIONS: ").Append(string.Join(", ", entry.blockedActions)).Append("]");
                    }
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nTRAUMA LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
