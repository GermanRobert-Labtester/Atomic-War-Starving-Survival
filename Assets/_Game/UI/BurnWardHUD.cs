using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class BurnPatientSnapshot
    {
        public string survivorId;
        public string survivorName;
        public bool hasSevereBurns;
        public bool isSkinGraftCompleted;
        public float infectionRateMultiplier;
    }

    public class BurnWardSnapshot
    {
        public bool isMedicalBedSterile;
        public float roomHygienePercent; // 0..1
        public int totalGraftsCompleted;
        public int totalBurnShockDeaths;
        public List<BurnPatientSnapshot> patients = new List<BurnPatientSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Burn Ward & Severe Burn Trauma HUD view-model.
    /// Manages fire victim burn shock treatment, sterile medical bed requirements,
    /// skin graft surgery procedures, and burn infection risk telemetry.
    /// </summary>
    public class BurnWardHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedPatientIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnBurnWardChanged;
        public event Action<string> OnPerformSkinGraftRequested; // (survivorId)

        private Func<BurnWardSnapshot> _getSnapshot;
        private BurnWardSnapshot _snapshot;

        public void Bind(Func<BurnWardSnapshot> getSnapshot)
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
            ReportOutcome("Selected burn patient: " + GetSelectedPatientName());
            return true;
        }

        public bool SelectPreviousPatient()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
                return false;
            SelectedPatientIndex = (SelectedPatientIndex - 1 + _snapshot.patients.Count) % _snapshot.patients.Count;
            ReportOutcome("Selected burn patient: " + GetSelectedPatientName());
            return true;
        }

        public bool RequestPerformSkinGraft()
        {
            if (!IsOpen || _snapshot == null || _snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                ReportOutcome("No burn patient selected for skin graft surgery.");
                return false;
            }

            var patient = GetSelectedPatient();
            if (patient == null) return false;

            if (patient.isSkinGraftCompleted)
            {
                ReportOutcome("Skin graft surgery already completed for " + patient.survivorName + ".");
                return false;
            }

            if (_snapshot != null && (!_snapshot.isMedicalBedSterile || _snapshot.roomHygienePercent < 1.0f))
            {
                ReportOutcome("CANNOT OPERATE: Medical bed must be STERILE and room hygiene at 100%!");
                return false;
            }

            if (OnPerformSkinGraftRequested == null)
            {
                ReportOutcome("Surgery room control link offline.");
                return false;
            }

            OnPerformSkinGraftRequested.Invoke(patient.survivorId);
            ReportOutcome("Performing skin graft surgery on " + patient.survivorName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No burn ward action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnBurnWardChanged?.Invoke();
        }

        private BurnPatientSnapshot GetSelectedPatient()
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
            return p != null ? p.survivorName ?? p.survivorId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BURN WARD & SKIN GRAFT SURGERY  [B] close  ·  [Tab] cycle  ·  [S] perform skin graft");

            if (_snapshot == null)
            {
                sb.Append("\nBurn ward telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nENVIRONMENT: Sterile Bed: ").Append(_snapshot.isMedicalBedSterile ? "[STERILE ✓]" : "[UNSTERILE - CRITICAL RISK!]")
              .Append("  ·  Room Hygiene: ").Append((_snapshot.roomHygienePercent * 100f).ToString("0")).Append("%")
              .Append("  ·  Grafts Done: ").Append(_snapshot.totalGraftsCompleted)
              .Append("  ·  Burn Deaths: ").Append(_snapshot.totalBurnShockDeaths);

            sb.Append("\n\nSEVERE BURN PATIENTS IN WARD:");
            if (_snapshot.patients == null || _snapshot.patients.Count == 0)
            {
                sb.Append("\n  No survivors suffering from severe burns.");
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
                      .Append(" — Infection Rate: ").Append(patient.infectionRateMultiplier.ToString("0.0")).Append("x");

                    if (patient.isSkinGraftCompleted) sb.Append("  ✔ [GRAFT COMPLETED - RECOVERING]");
                    else if (patient.hasSevereBurns) sb.Append("  ★ [CRITICAL SEVERE BURNS - SURGERY REQUIRED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nWARD LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
