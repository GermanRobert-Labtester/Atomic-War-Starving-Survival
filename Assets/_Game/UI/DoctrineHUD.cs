using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class DoctrineStateSnapshot
    {
        public string doctrineId;
        public string displayName;
        public bool isAdopted;
        public string[] conflictingDoctrines;
        public string unlockRequirementSummary;
        public string passiveBuffSummary;
        public string hiddenCostSummary;
    }

    public class DoctrineSnapshot
    {
        public int adoptedDoctrinesCount;
        public List<DoctrineStateSnapshot> doctrines = new List<DoctrineStateSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Faction Doctrines & Progression HUD view-model.
    /// Manages adoption of permanent faction doctrines (Doctrine of Iron, Roots, Glow, Toll, Ghost),
    /// mutually exclusive faction conflicts, unlock sacrifices, passive buffs, and hidden costs.
    /// </summary>
    public class DoctrineHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedDoctrineIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnDoctrineChanged;
        public event Action<string> OnAdoptDoctrineRequested; // (doctrineId)

        private Func<DoctrineSnapshot> _getSnapshot;
        private DoctrineSnapshot _snapshot;

        public void Bind(Func<DoctrineSnapshot> getSnapshot)
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

        public bool SelectNextDoctrine()
        {
            if (!IsOpen || _snapshot == null || _snapshot.doctrines == null || _snapshot.doctrines.Count == 0)
                return false;
            SelectedDoctrineIndex = (SelectedDoctrineIndex + 1) % _snapshot.doctrines.Count;
            ReportOutcome("Selected faction doctrine: " + GetSelectedDoctrineName());
            return true;
        }

        public bool SelectPreviousDoctrine()
        {
            if (!IsOpen || _snapshot == null || _snapshot.doctrines == null || _snapshot.doctrines.Count == 0)
                return false;
            SelectedDoctrineIndex = (SelectedDoctrineIndex - 1 + _snapshot.doctrines.Count) % _snapshot.doctrines.Count;
            ReportOutcome("Selected faction doctrine: " + GetSelectedDoctrineName());
            return true;
        }

        public bool RequestAdoptDoctrine()
        {
            if (!IsOpen || _snapshot == null || _snapshot.doctrines == null || _snapshot.doctrines.Count == 0)
            {
                ReportOutcome("No doctrine selected for adoption.");
                return false;
            }

            var doctrine = GetSelectedDoctrine();
            if (doctrine == null) return false;

            if (doctrine.isAdopted)
            {
                ReportOutcome("Doctrine " + doctrine.displayName + " is already adopted!");
                return false;
            }

            if (OnAdoptDoctrineRequested == null)
            {
                ReportOutcome("Doctrine council link offline.");
                return false;
            }

            OnAdoptDoctrineRequested.Invoke(doctrine.doctrineId);
            ReportOutcome("Adopting Doctrine: " + doctrine.displayName + " (Permanent Alignment!)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No doctrine action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnDoctrineChanged?.Invoke();
        }

        private DoctrineStateSnapshot GetSelectedDoctrine()
        {
            if (_snapshot != null && _snapshot.doctrines != null && SelectedDoctrineIndex >= 0 && SelectedDoctrineIndex < _snapshot.doctrines.Count)
            {
                return _snapshot.doctrines[SelectedDoctrineIndex];
            }
            return null;
        }

        private string GetSelectedDoctrineName()
        {
            var d = GetSelectedDoctrine();
            return d != null ? (d.displayName ?? d.doctrineId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("FACTION DOCTRINES & SACRIFICES  [O] close  ·  [Tab] cycle  ·  [A] adopt doctrine");

            if (_snapshot == null)
            {
                sb.Append("\nDoctrine council telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nALIGNMENT STATS: Active Doctrines Adopted: ").Append(_snapshot.adoptedDoctrinesCount);

            sb.Append("\n\nFACTION DOCTRINES:");
            if (_snapshot.doctrines == null || _snapshot.doctrines.Count == 0)
            {
                sb.Append("\n  No doctrines available for adoption.");
            }
            else
            {
                for (int i = 0; i < _snapshot.doctrines.Count; i++)
                {
                    var doc = _snapshot.doctrines[i];
                    if (doc == null) continue;

                    bool selected = (i == SelectedDoctrineIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(doc.displayName ?? doc.doctrineId)
                      .Append(" — ").Append(doc.passiveBuffSummary ?? "Passive alignment");

                    if (doc.isAdopted) sb.Append("  ★ [ADOPTED DOCTRINE]");
                    if (doc.conflictingDoctrines != null && doc.conflictingDoctrines.Length > 0)
                    {
                        sb.Append("\n    [CONFLICTS WITH: ").Append(string.Join(", ", doc.conflictingDoctrines)).Append("]");
                    }
                    if (!string.IsNullOrEmpty(doc.unlockRequirementSummary))
                    {
                        sb.Append("\n    [SACRIFICE REQ: ").Append(doc.unlockRequirementSummary).Append("]");
                    }
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nCOUNCIL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
