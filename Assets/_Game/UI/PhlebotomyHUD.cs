using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class BloodPackSnapshot
    {
        public string packId;
        public string bloodType; // e.g. "O_negative", "A_positive"
        public float volumeMl;
        public bool isContaminated;
        public string donorSurvivorName;
    }

    public class PhlebotomySnapshot
    {
        public float totalStoredBloodMl;
        public int totalPlasmaUnits;
        public List<BloodPackSnapshot> packs = new List<BloodPackSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Phlebotomy & Blood Bank Transfusion HUD view-model.
    /// Monitors blood bank IV bag inventory, blood type compatibility (O-, A+, B+, AB+),
    /// plasma separation, acute anemia treatment, and transfusion reaction prevention.
    /// </summary>
    public class PhlebotomyHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedPackIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPhlebotomyChanged;
        public event Action<string> OnTransfuseBloodRequested; // (packId)
        public event Action<string> OnDrawBloodFromSurvivorRequested; // (survivorId)

        private Func<PhlebotomySnapshot> _getSnapshot;
        private PhlebotomySnapshot _snapshot;

        public void Bind(Func<PhlebotomySnapshot> getSnapshot)
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

        public bool SelectNextPack()
        {
            if (!IsOpen || _snapshot == null || _snapshot.packs == null || _snapshot.packs.Count == 0)
                return false;
            SelectedPackIndex = (SelectedPackIndex + 1) % _snapshot.packs.Count;
            ReportOutcome("Selected blood pack: " + GetSelectedPackName());
            return true;
        }

        public bool SelectPreviousPack()
        {
            if (!IsOpen || _snapshot == null || _snapshot.packs == null || _snapshot.packs.Count == 0)
                return false;
            SelectedPackIndex = (SelectedPackIndex - 1 + _snapshot.packs.Count) % _snapshot.packs.Count;
            ReportOutcome("Selected blood pack: " + GetSelectedPackName());
            return true;
        }

        public bool RequestTransfuseBlood()
        {
            if (!IsOpen || _snapshot == null || _snapshot.packs == null || _snapshot.packs.Count == 0)
            {
                ReportOutcome("No blood pack selected for IV transfusion.");
                return false;
            }

            var pack = GetSelectedPack();
            if (pack == null) return false;

            if (pack.isContaminated)
            {
                ReportOutcome("CANNOT TRANSFUSE: Blood Pack " + pack.packId + " is contaminated!");
                return false;
            }

            if (OnTransfuseBloodRequested == null)
            {
                ReportOutcome("Medical IV blood bank link offline.");
                return false;
            }

            OnTransfuseBloodRequested.Invoke(pack.packId);
            ReportOutcome("Transfusing " + pack.volumeMl.ToString("0") + " mL of " + pack.bloodType + " blood to recipient...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No phlebotomy action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPhlebotomyChanged?.Invoke();
        }

        private BloodPackSnapshot GetSelectedPack()
        {
            if (_snapshot != null && _snapshot.packs != null && SelectedPackIndex >= 0 && SelectedPackIndex < _snapshot.packs.Count)
            {
                return _snapshot.packs[SelectedPackIndex];
            }
            return null;
        }

        private string GetSelectedPackName()
        {
            var p = GetSelectedPack();
            return p != null ? (p.packId + " — " + p.bloodType) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BLOOD BANK & PHLEBOTOMY TRANSFUSION  [B] close  ·  [Tab] cycle  ·  [T] transfuse blood pack");

            if (_snapshot == null)
            {
                sb.Append("\nBlood bank telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nBLOOD BANK STATS: Total Stored Blood: ").Append(_snapshot.totalStoredBloodMl.ToString("0")).Append(" mL")
              .Append("  ·  Plasma Units: ").Append(_snapshot.totalPlasmaUnits);

            sb.Append("\n\nSTORED REFRIGERATED BLOOD PACKS:");
            if (_snapshot.packs == null || _snapshot.packs.Count == 0)
            {
                sb.Append("\n  No blood packs in medical refrigerator.");
            }
            else
            {
                for (int i = 0; i < _snapshot.packs.Count; i++)
                {
                    var pack = _snapshot.packs[i];
                    if (pack == null) continue;

                    bool selected = (i == SelectedPackIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Pack ").Append(pack.packId)
                      .Append(" — Type: ").Append(pack.bloodType ?? "O_neg")
                      .Append(" (").Append(pack.volumeMl.ToString("0")).Append(" mL)")
                      .Append(" — Donor: ").Append(pack.donorSurvivorName ?? "Unknown");

                    if (pack.isContaminated) sb.Append("  ✖ [CONTAMINATED — DO NOT TRANSFUSE]");
                    else sb.Append("  ✔ [STERILE]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nBLOOD BANK LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
