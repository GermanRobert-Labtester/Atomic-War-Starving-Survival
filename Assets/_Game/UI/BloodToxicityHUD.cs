using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class BloodToxicityEntrySnapshot
    {
        public string survivorId;
        public string survivorName;
        public int chemUsageCount;
        public float bloodToxicityLevel; // 0..100
        public int chemThreshold;
        public bool isBloodToxic;
    }

    public class BloodToxicitySnapshot
    {
        public int totalToxicBloodSurvivors;
        public List<BloodToxicityEntrySnapshot> entries = new List<BloodToxicityEntrySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Blood Toxicity & Chem Abuse Defense HUD view-model.
    /// Monitors heavy chem usage toxicity (morphine, anti-rad, amphetamines), toxic blood threshold,
    /// self-destructive bite retaliation poison defense against feral dogs & cannibals,
    /// and blood phlebotomy detox purging.
    /// </summary>
    public class BloodToxicityHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSurvivorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnBloodToxicityChanged;
        public event Action<string> OnPhlebotomyPurgeRequested; // (survivorId)

        private Func<BloodToxicitySnapshot> _getSnapshot;
        private BloodToxicitySnapshot _snapshot;

        public void Bind(Func<BloodToxicitySnapshot> getSnapshot)
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

        public bool SelectNextSurvivor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
                return false;
            SelectedSurvivorIndex = (SelectedSurvivorIndex + 1) % _snapshot.entries.Count;
            ReportOutcome("Selected survivor blood profile: " + GetSelectedSurvivorName());
            return true;
        }

        public bool SelectPreviousSurvivor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
                return false;
            SelectedSurvivorIndex = (SelectedSurvivorIndex - 1 + _snapshot.entries.Count) % _snapshot.entries.Count;
            ReportOutcome("Selected survivor blood profile: " + GetSelectedSurvivorName());
            return true;
        }

        public bool RequestPhlebotomyPurge()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                ReportOutcome("No survivor selected for blood phlebotomy detox.");
                return false;
            }

            var entry = GetSelectedSurvivor();
            if (entry == null) return false;

            if (!entry.isBloodToxic && entry.bloodToxicityLevel <= 0f)
            {
                ReportOutcome(entry.survivorName + "'s blood shows zero chem toxicity.");
                return false;
            }

            if (OnPhlebotomyPurgeRequested == null)
            {
                ReportOutcome("Medical phlebotomy link offline.");
                return false;
            }

            OnPhlebotomyPurgeRequested.Invoke(entry.survivorId);
            ReportOutcome("Executing phlebotomy blood detox on " + entry.survivorName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No blood toxicity action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnBloodToxicityChanged?.Invoke();
        }

        private BloodToxicityEntrySnapshot GetSelectedSurvivor()
        {
            if (_snapshot != null && _snapshot.entries != null && SelectedSurvivorIndex >= 0 && SelectedSurvivorIndex < _snapshot.entries.Count)
            {
                return _snapshot.entries[SelectedSurvivorIndex];
            }
            return null;
        }

        private string GetSelectedSurvivorName()
        {
            var e = GetSelectedSurvivor();
            return e != null ? (e.survivorName ?? e.survivorId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CHEM ABUSE & BLOOD TOXICITY MONITOR  [K] close  ·  [Tab] cycle  ·  [P] phlebotomy detox");

            if (_snapshot == null)
            {
                sb.Append("\nBlood toxicity telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nBLOOD STATS: Toxic Blood Survivors: ").Append(_snapshot.totalToxicBloodSurvivors);

            sb.Append("\n\nSURVIVOR BLOOD TOXICITY PROFILES:");
            if (_snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                sb.Append("\n  No survivor blood profiles registered.");
            }
            else
            {
                for (int i = 0; i < _snapshot.entries.Count; i++)
                {
                    var entry = _snapshot.entries[i];
                    if (entry == null) continue;

                    bool selected = (i == SelectedSurvivorIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(entry.survivorName ?? entry.survivorId)
                      .Append(" — Toxicity Level: ").Append(entry.bloodToxicityLevel.ToString("0.#")).Append(" / 100")
                      .Append(" | Chems Used: ").Append(entry.chemUsageCount).Append(" / ").Append(entry.chemThreshold);

                    if (entry.isBloodToxic) sb.Append("  ★ [BLOOD TOXIC — ATTACKERS SUFFER 20 POISON DAMAGE]");
                    else sb.Append("  [CLEAN BLOOD]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nBLOOD LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
