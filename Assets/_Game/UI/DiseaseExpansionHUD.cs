#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class ContagiousPatientSnapshot
    {
        public string survivorId;
        public string survivorName;
        public string diseaseName;
        public float contagionRiskPercent; // 0..100
        public bool isQuarantined;
        public int daysSick;
    }

    public class DiseaseExpansionSnapshot
    {
        public int totalQuarantinedCount;
        public int totalOutbreaksPrevented;
        public List<ContagiousPatientSnapshot> patients = new List<ContagiousPatientSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Advanced Disease & Quarantine Outbreak HUD view-model.
    /// Monitors contagious pandemic outbreaks (bacterial infection, trench foot, radiation sickness),
    /// survivor quarantine room isolation, antibiotic therapy, and shelter contagion containment.
    /// </summary>
    public class DiseaseExpansionHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedPatientIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnDiseaseExpansionChanged;
        public event Action<string> OnToggleQuarantineRequested; // (survivorId)
        public event Action<string> OnAdministerAntibioticsRequested; // (survivorId)

        private Func<DiseaseExpansionSnapshot> _getSnapshot;
        private DiseaseExpansionSnapshot _snapshot;

        public void Bind(Func<DiseaseExpansionSnapshot> getSnapshot)
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

        public bool SelectNextPatient()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
                return false;
            SelectedPatientIndex = (SelectedPatientIndex + 1) % _snapshot.patients.Count;
            ReportOutcome("Selected contagious patient: " + GetSelectedPatientName());
            return true;
        }

        public bool SelectPreviousPatient()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
                return false;
            SelectedPatientIndex = (SelectedPatientIndex - 1 + _snapshot.patients.Count) % _snapshot.patients.Count;
            ReportOutcome("Selected contagious patient: " + GetSelectedPatientName());
            return true;
        }

        public bool RequestToggleQuarantine()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                ReportOutcome("No patient selected for quarantine toggle.");
                return false;
            }

            var patient = GetSelectedPatient();
            if (patient == null) return false;

            if (OnToggleQuarantineRequested == null)
            {
                ReportOutcome("Quarantine isolation door link offline.");
                return false;
            }

            OnToggleQuarantineRequested.Invoke(patient.survivorId);
            ReportOutcome((patient.isQuarantined ? "Releasing " : "Isolating ") + patient.survivorName + (patient.isQuarantined ? " from" : " in") + " Quarantine Ward...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No disease action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnDiseaseExpansionChanged?.Invoke();
        }

        private ContagiousPatientSnapshot GetSelectedPatient()
        {
            if (_snapshot != null && _snapshot.patients != null && SelectedPatientIndex >= 0 && SelectedPatientIndex < _snapshot.patients.Count)
            {
                return _snapshot.patients[SelectedPatientIndex];
            }
            return null;
        }

        private string GetSelectedPatientName()
        {
            var p = GetSelectedPatient();
            return p != null ? (p.survivorName ?? p.survivorId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("DISEASE OUTBREAK & QUARANTINE WARD  [Q] close  ·  [Tab] cycle  ·  [I] isolate in quarantine");

            if (_snapshot == null)
            {
                sb.Append("\nDisease outbreak telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nOUTBREAK STATS: Patients in Quarantine: ").Append(_snapshot.totalQuarantinedCount)
              .Append("  ·  Outbreaks Prevented: ").Append(_snapshot.totalOutbreaksPrevented);

            sb.Append("\n\nCONTAGIOUS SURVIVORS:");
            if (_snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                sb.Append("\n  No active contagious infections detected in shelter.");
            }
            else
            {
                for (int i = 0; i < _snapshot.patients.Count; i++)
                {
                    var patient = _snapshot.patients[i];
                    if (patient == null) continue;

                    bool selected = (i == SelectedPatientIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(patient.survivorName ?? patient.survivorId)
                      .Append(" — Affliction: ").Append(patient.diseaseName ?? "Infection")
                      .Append(" (Day ").Append(patient.daysSick).Append(")")
                      .Append(" | Contagion Risk: ").Append(patient.contagionRiskPercent.ToString("0")).Append("%");

                    if (patient.isQuarantined) sb.Append("  ✔ [QUARANTINED - CONTAINED]");
                    else sb.Append("  ★ [UNQUARANTINED - HIGH RISK TO LIVING QUARTERS]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nEPIDEMIC LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
