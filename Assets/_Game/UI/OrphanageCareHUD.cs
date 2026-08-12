using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class OrphanChildSnapshot
    {
        public string childId;
        public string childName;
        public int age;
        public string assignedGuardianName;
        public float moralePercent; // 0..100
        public float educationLevelPercent; // 0..100
        public bool isProtected;
    }

    public class OrphanageCareSnapshot
    {
        public int totalOrphansInShelter;
        public List<OrphanChildSnapshot> orphans = new List<OrphanChildSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Wasteland Orphanage Care & Guardian Assignment HUD view-model.
    /// Monitors shelter wasteland orphans, guardian survivor pairings, child morale stabilization,
    /// basic education / skill training, and trauma shielding.
    /// </summary>
    public class OrphanageCareHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedChildIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnOrphanageCareChanged;
        public event Action<string, string> OnAssignGuardianRequested; // (childId, guardianSurvivorId)

        private Func<OrphanageCareSnapshot> _getSnapshot;
        private OrphanageCareSnapshot _snapshot;

        public void Bind(Func<OrphanageCareSnapshot> getSnapshot)
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

        public bool SelectNextChild()
        {
            if (!IsOpen || _snapshot == null || _snapshot.orphans == null || _snapshot.orphans.Count == 0)
                return false;
            SelectedChildIndex = (SelectedChildIndex + 1) % _snapshot.orphans.Count;
            ReportOutcome("Selected orphan child: " + GetSelectedChildName());
            return true;
        }

        public bool SelectPreviousChild()
        {
            if (!IsOpen || _snapshot == null || _snapshot.orphans == null || _snapshot.orphans.Count == 0)
                return false;
            SelectedChildIndex = (SelectedChildIndex - 1 + _snapshot.orphans.Count) % _snapshot.orphans.Count;
            ReportOutcome("Selected orphan child: " + GetSelectedChildName());
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No orphanage action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnOrphanageCareChanged?.Invoke();
        }

        private OrphanChildSnapshot GetSelectedChild()
        {
            if (_snapshot != null && _snapshot.orphans != null && SelectedChildIndex >= 0 && SelectedChildIndex < _snapshot.orphans.Count)
            {
                return _snapshot.orphans[SelectedChildIndex];
            }
            return null;
        }

        private string GetSelectedChildName()
        {
            var c = GetSelectedChild();
            return c != null ? (c.childName ?? c.childId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SHELTER ORPHANAGE & CHILD CARE  [O] close  ·  [Tab] cycle");

            if (_snapshot == null)
            {
                sb.Append("\nOrphanage telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nORPHANAGE STATS: Total Children in Care: ").Append(_snapshot.totalOrphansInShelter);

            sb.Append("\n\nSHELTER ORPHANS & GUARDIAN ASSIGNMENTS:");
            if (_snapshot.orphans == null || _snapshot.orphans.Count == 0)
            {
                sb.Append("\n  No orphan children currently in shelter.");
            }
            else
            {
                for (int i = 0; i < _snapshot.orphans.Count; i++)
                {
                    var child = _snapshot.orphans[i];
                    if (child == null) continue;

                    bool selected = (i == SelectedChildIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(child.childName ?? child.childId)
                      .Append(" (Age ").Append(child.age).Append(")")
                      .Append(" — Guardian: ").Append(child.assignedGuardianName ?? "Unassigned")
                      .Append(" | Morale: ").Append(child.moralePercent.ToString("0")).Append("%")
                      .Append(" | Education: ").Append(child.educationLevelPercent.ToString("0")).Append("%");

                    if (child.isProtected) sb.Append("  ✔ [GUARDIAN ASSIGNED — MORALE STABLE]");
                    else sb.Append("  ★ [NO GUARDIAN — HIGH TRAUMA RISK]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nORPHANAGE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
