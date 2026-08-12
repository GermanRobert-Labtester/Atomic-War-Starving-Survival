using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class InsomniaPatientSnapshot
    {
        public string survivorId;
        public string survivorName;
        public float hoursAwake;
        public int psychosisStage; // 1..3
        public float hallucinationRiskPercent; // 0..100
        public bool isPsychoticBreak;
    }

    public class InsomniaPsychosisSnapshot
    {
        public int totalPsychosisCases;
        public List<InsomniaPatientSnapshot> patients = new List<InsomniaPatientSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Insomnia Deprivation & Psychosis Stage HUD view-model.
    /// Monitors severe sleep deprivation (72h+ awake), psychosis stage escalation (1..3),
    /// auditory hallucination risks, emergency sedative administration, and forced bunk rest.
    /// </summary>
    public class InsomniaPsychosisHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedPatientIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnInsomniaPsychosisChanged;
        public event Action<string> OnAdministerSedativeRequested; // (survivorId)
        public event Action<string> OnForceBunkRestRequested; // (survivorId)

        private Func<InsomniaPsychosisSnapshot> _getSnapshot;
        private InsomniaPsychosisSnapshot _snapshot;

        public void Bind(Func<InsomniaPsychosisSnapshot> getSnapshot)
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
            ReportOutcome("Selected insomnia patient: " + GetSelectedPatientName());
            return true;
        }

        public bool SelectPreviousPatient()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
                return false;
            SelectedPatientIndex = (SelectedPatientIndex - 1 + _snapshot.patients.Count) % _snapshot.patients.Count;
            ReportOutcome("Selected insomnia patient: " + GetSelectedPatientName());
            return true;
        }

        public bool RequestAdministerSedative()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                ReportOutcome("No survivor selected for sedative injection.");
                return false;
            }

            var patient = GetSelectedPatient();
            if (patient == null) return false;

            if (OnAdministerSedativeRequested == null)
            {
                ReportOutcome("Medical sedative injector link offline.");
                return false;
            }

            OnAdministerSedativeRequested.Invoke(patient.survivorId);
            ReportOutcome("Administering Sedative Injection to " + patient.survivorName + " (Inducing emergency sleep)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No insomnia action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnInsomniaPsychosisChanged?.Invoke();
        }

        private InsomniaPatientSnapshot GetSelectedPatient()
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
            var sb = new StringBuilder("SLEEP DEPRIVATION & PSYCHOSIS WARD  [S] close  ·  [Tab] cycle  ·  [A] administer sedative");

            if (_snapshot == null)
            {
                sb.Append("\nInsomnia ward telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nPSYCHOSIS STATS: Total Psychosis Cases: ").Append(_snapshot.totalPsychosisCases);

            sb.Append("\n\nSLEEP-DEPRIVED SURVIVORS:");
            if (_snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                sb.Append("\n  No sleep-deprived survivors in psychosis ward.");
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
                      .Append(" — Hours Awake: ").Append(patient.hoursAwake.ToString("0.#")).Append(" hrs")
                      .Append(" — Psychosis Stage: ").Append(patient.psychosisStage).Append(" / 3")
                      .Append(" | Hallucination Risk: ").Append(patient.hallucinationRiskPercent.ToString("0")).Append("%");

                    if (patient.isPsychoticBreak) sb.Append("  ★ [PSYCHOTIC BREAK IN PROGRESS — DANGEROUS TO OTHERS]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nPSYCHOSIS LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
