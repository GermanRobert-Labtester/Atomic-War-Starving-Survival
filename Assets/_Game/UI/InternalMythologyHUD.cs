using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class MythStateSnapshot
    {
        public string mythId;
        public string displayName;
        public bool isActive;
        public bool isInstitutionalized;
        public float foodCostPerDay;
        public float moraleBoost;
        public string description;
    }

    public class InternalMythologySnapshot
    {
        public float totalFoodSacrificed;
        public int totalMythsDebunked;
        public List<MythStateSnapshot> activeMyths = new List<MythStateSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Internal Mythology & Bunker Folklore HUD view-model.
    /// Manages urban legends born in dark shelter corridors (The Vent Walker,
    /// The Third Hatch, The Iron Worm, The Ash Devil). Manages ritual food offerings,
    /// institutionalizing myths into shelter religion, or debunking them to restore rationalism.
    /// </summary>
    public class InternalMythologyHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedMythIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnInternalMythologyChanged;
        public event Action<string> OnInstitutionalizeMythRequested; // (mythId)
        public event Action<string> OnDebunkMythRequested;           // (mythId)

        private Func<InternalMythologySnapshot> _getSnapshot;
        private InternalMythologySnapshot _snapshot;

        public void Bind(Func<InternalMythologySnapshot> getSnapshot)
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

        public bool SelectNextMyth()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeMyths == null || _snapshot.activeMyths.Count == 0)
                return false;
            SelectedMythIndex = (SelectedMythIndex + 1) % _snapshot.activeMyths.Count;
            ReportOutcome("Selected bunker myth: " + GetSelectedMythName());
            return true;
        }

        public bool SelectPreviousMyth()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeMyths == null || _snapshot.activeMyths.Count == 0)
                return false;
            SelectedMythIndex = (SelectedMythIndex - 1 + _snapshot.activeMyths.Count) % _snapshot.activeMyths.Count;
            ReportOutcome("Selected bunker myth: " + GetSelectedMythName());
            return true;
        }

        public bool RequestInstitutionalizeMyth()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeMyths == null || _snapshot.activeMyths.Count == 0)
            {
                ReportOutcome("No myth selected for institutionalization.");
                return false;
            }

            var myth = GetSelectedMyth();
            if (myth == null) return false;

            if (myth.isInstitutionalized)
            {
                ReportOutcome("Myth " + myth.displayName + " is already institutionalized as shelter doctrine.");
                return false;
            }

            if (OnInstitutionalizeMythRequested == null)
            {
                ReportOutcome("Bunker social director link offline.");
                return false;
            }

            OnInstitutionalizeMythRequested.Invoke(myth.mythId);
            ReportOutcome("Institutionalizing myth: " + myth.displayName + " into shelter lore...");
            return true;
        }

        public bool RequestDebunkMyth()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeMyths == null || _snapshot.activeMyths.Count == 0)
            {
                ReportOutcome("No myth selected for debunking.");
                return false;
            }

            var myth = GetSelectedMyth();
            if (myth == null) return false;

            if (OnDebunkMythRequested == null)
            {
                ReportOutcome("Bunker social director link offline.");
                return false;
            }

            OnDebunkMythRequested.Invoke(myth.mythId);
            ReportOutcome("Debunking myth: " + myth.displayName + " (Morale hit imminent!)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No myth action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnInternalMythologyChanged?.Invoke();
        }

        private MythStateSnapshot GetSelectedMyth()
        {
            if (_snapshot != null && _snapshot.activeMyths != null && SelectedMythIndex >= 0 && SelectedMythIndex < _snapshot.activeMyths.Count)
            {
                return _snapshot.activeMyths[SelectedMythIndex];
            }
            return null;
        }

        private string GetSelectedMythName()
        {
            var m = GetSelectedMyth();
            return m != null ? m.displayName ?? m.mythId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("INTERNAL MYTHOLOGY & BUNKER FOLKLORE  [M] close  ·  [Tab] cycle  ·  [I] institutionalize  ·  [D] debunk");

            if (_snapshot == null)
            {
                sb.Append("\nBunker folklore telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nFOLKLORE STATS: Food Offered to Vents: ").Append(_snapshot.totalFoodSacrificed.ToString("0.#")).Append(" rations")
              .Append("  ·  Myths Debunked: ").Append(_snapshot.totalMythsDebunked);

            sb.Append("\n\nACTIVE BUNKER MYTHS & URBAN LEGENDS:");
            if (_snapshot.activeMyths == null || _snapshot.activeMyths.Count == 0)
            {
                sb.Append("\n  No active urban legends spreading through shelter corridors.");
            }
            else
            {
                for (int i = 0; i < _snapshot.activeMyths.Count; i++)
                {
                    var myth = _snapshot.activeMyths[i];
                    if (myth == null) continue;

                    bool selected = (i == SelectedMythIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(myth.displayName ?? myth.mythId)
                      .Append(" — ").Append(myth.description ?? "Corridor rumor")
                      .Append(" | Morale Boost: +").Append(myth.moraleBoost.ToString("0.#"))
                      .Append(" | Food Cost: ").Append(myth.foodCostPerDay.ToString("0.#")).Append(" r/day");

                    if (myth.isInstitutionalized) sb.Append("  ★ [INSTITUTIONALIZED DOCTRINE]");
                    else if (myth.isActive) sb.Append("  [ACTIVE RUMOR]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nFOLKLORE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
