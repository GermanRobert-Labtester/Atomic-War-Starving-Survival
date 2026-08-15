using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class PregnantMotherSnapshot
    {
        public string motherId;
        public string motherName;
        public int gestationWeeks; // 1..40
        public float healthPercent; // 0..100
        public float nutritionPercent; // 0..100
        public bool medicalBedAssigned;
        public int dueInDays;
    }

    public class PregnancyCareSnapshot
    {
        public int totalPregnantMothers;
        public List<PregnantMotherSnapshot> mothers = new List<PregnantMotherSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Wasteland Pregnancy & Maternal Medical Care HUD view-model.
    /// Monitors pregnant survivor health, gestation progress (weeks 1..40), maternal nutrition,
    /// sterile medical bed assignments, delivery room prep, and infant birth telemetry.
    /// </summary>
    public class PregnancyCareHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedMotherIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPregnancyCareChanged;
        public event Action<string> OnAssignMedicalBedRequested; // (motherId)

        private Func<PregnancyCareSnapshot> _getSnapshot;
        private PregnancyCareSnapshot _snapshot;

        public void Bind(Func<PregnancyCareSnapshot> getSnapshot)
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

        public bool SelectNextMother()
        {
            if (!IsOpen || _snapshot == null || _snapshot.mothers == null || _snapshot.mothers.Count == 0)
                return false;
            SelectedMotherIndex = (SelectedMotherIndex + 1) % _snapshot.mothers.Count;
            ReportOutcome("Selected pregnant mother: " + GetSelectedMotherName());
            return true;
        }

        public bool SelectPreviousMother()
        {
            if (!IsOpen || _snapshot == null || _snapshot.mothers == null || _snapshot.mothers.Count == 0)
                return false;
            SelectedMotherIndex = (SelectedMotherIndex - 1 + _snapshot.mothers.Count) % _snapshot.mothers.Count;
            ReportOutcome("Selected pregnant mother: " + GetSelectedMotherName());
            return true;
        }

        public bool RequestAssignMedicalBed()
        {
            if (!IsOpen || _snapshot == null || _snapshot.mothers == null || _snapshot.mothers.Count == 0)
            {
                ReportOutcome("No pregnant mother selected for medical bed assignment.");
                return false;
            }

            var mother = GetSelectedMother();
            if (mother == null) return false;

            if (OnAssignMedicalBedRequested == null)
            {
                ReportOutcome("Medical ward bed dispatcher link offline.");
                return false;
            }

            OnAssignMedicalBedRequested.Invoke(mother.motherId);
            ReportOutcome("Assigning sterile Medical Bed to " + mother.motherName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No maternal care action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPregnancyCareChanged?.Invoke();
        }

        private PregnantMotherSnapshot GetSelectedMother()
        {
            if (_snapshot != null && _snapshot.mothers != null && SelectedMotherIndex >= 0 && SelectedMotherIndex < _snapshot.mothers.Count)
            {
                return _snapshot.mothers[SelectedMotherIndex];
            }
            return null;
        }

        private string GetSelectedMotherName()
        {
            var m = GetSelectedMother();
            return m != null ? (m.motherName ?? m.motherId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("MATERNAL MEDICAL CARE & PREGNANCY MONITOR  [P] close  ·  [Tab] cycle  ·  [B] assign medical bed");

            if (_snapshot == null)
            {
                sb.Append("\nMaternal medical telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nMATERNAL STATS: Total Pregnant Mothers: ").Append(_snapshot.totalPregnantMothers);

            sb.Append("\n\nPREGNANT SURVIVORS IN SHELTER:");
            if (_snapshot.mothers == null || _snapshot.mothers.Count == 0)
            {
                sb.Append("\n  No pregnant survivors in shelter.");
            }
            else
            {
                for (int i = 0; i < _snapshot.mothers.Count; i++)
                {
                    var mother = _snapshot.mothers[i];
                    if (mother == null) continue;

                    bool selected = (i == SelectedMotherIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(mother.motherName ?? mother.motherId)
                      .Append(" — Gestation: Week ").Append(mother.gestationWeeks).Append(" / 40")
                      .Append(" (Due in ").Append(mother.dueInDays).Append(" days)")
                      .Append(" | Health: ").Append(mother.healthPercent.ToString("0")).Append("%")
                      .Append(" | Nutrition: ").Append(mother.nutritionPercent.ToString("0")).Append("%");

                    if (mother.medicalBedAssigned) sb.Append("  ✔ [STERILE MEDICAL BED ASSIGNED]");
                    else sb.Append("  ★ [NO MEDICAL BED — HIGH COMPLICATION RISK]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nMATERNAL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
