using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class QuarantineWardPatientSnapshot
    {
        public string survivorId;
        public string survivorName;
        public string contagionKind;
        public int isolationDay;
        public float symptomSeverityPercent; // 0..100
        public bool isBleedingOut;
    }

    public class QuarantineSnapshot
    {
        public int totalIsolatedPatients;
        public int totalRecovered;
        public List<QuarantineWardPatientSnapshot> patients = new List<QuarantineWardPatientSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Biohazard Quarantine Ward & Infection Containment HUD view-model.
    /// Monitors negative-pressure isolation cells, biohazard door seals, symptom tracking,
    /// antibiotic infusion treatment, and contagion recovery progress.
    /// </summary>
    public class QuarantineHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedPatientIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnQuarantineChanged;
        public event Action<string> OnTreatPatientRequested; // (survivorId)

        private Func<QuarantineSnapshot> _getSnapshot;
        private QuarantineSnapshot _snapshot;

        public void Bind(Func<QuarantineSnapshot> getSnapshot)
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
            ReportOutcome("Selected quarantine patient: " + GetSelectedPatientName());
            return true;
        }

        public bool SelectPreviousPatient()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
                return false;
            SelectedPatientIndex = (SelectedPatientIndex - 1 + _snapshot.patients.Count) % _snapshot.patients.Count;
            ReportOutcome("Selected quarantine patient: " + GetSelectedPatientName());
            return true;
        }

        public bool RequestTreatPatient()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                ReportOutcome("No patient selected for medical treatment.");
                return false;
            }

            var patient = GetSelectedPatient();
            if (patient == null) return false;

            if (OnTreatPatientRequested == null)
            {
                ReportOutcome("Quarantine medical IV link offline.");
                return false;
            }

            OnTreatPatientRequested.Invoke(patient.survivorId);
            ReportOutcome("Administering Antibiotic IV Infusion to " + patient.survivorName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No quarantine action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnQuarantineChanged?.Invoke();
        }

        private QuarantineWardPatientSnapshot GetSelectedPatient()
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
            var sb = new StringBuilder("BIOHAZARD QUARANTINE WARD  [Q] close  ·  [Tab] cycle  ·  [T] administer IV treatment");

            if (_snapshot == null)
            {
                sb.Append("\nQuarantine ward telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nQUARANTINE STATS: Patients Isolated: ").Append(_snapshot.totalIsolatedPatients)
              .Append("  ·  Total Recovered: ").Append(_snapshot.totalRecovered);

            sb.Append("\n\nNEGATIVE-PRESSURE ISOLATION CELLS:");
            if (_snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                sb.Append("\n  No patients in quarantine ward.");
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
                      .Append(" — Infection: ").Append(patient.contagionKind ?? "Severe Contagion")
                      .Append(" (Day ").Append(patient.isolationDay).Append(")")
                      .Append(" — Symptom Severity: ").Append(patient.symptomSeverityPercent.ToString("0")).Append("%");

                    if (patient.isBleedingOut) sb.Append("  ★ [CRITICAL: INTERNAL BLEEDING]");
                    else sb.Append("  ✔ [ISOLATED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nQUARANTINE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
