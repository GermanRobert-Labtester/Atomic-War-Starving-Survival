using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class HeadcountEntrySnapshot
    {
        public string survivorId;
        public string survivorName;
        public bool isAccountedFor;
        public string lastSeenLocation;
        public bool isMissing;
    }

    public class HeadcountSnapshot
    {
        public int totalBunkerPopulation;
        public int totalMissingSurvivors;
        public List<HeadcountEntrySnapshot> entries = new List<HeadcountEntrySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Daily Headcount & Roll Call HUD view-model.
    /// Monitors shelter survivor headcount verification, missing persons alerts,
    /// last-seen corridor locations, and search party dispatch.
    /// </summary>
    public class HeadcountHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedSurvivorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHeadcountChanged;
        public event Action<string> OnDispatchSearchPartyRequested; // (missingSurvivorId)

        private Func<HeadcountSnapshot> _getSnapshot;
        private HeadcountSnapshot _snapshot;

        public void Bind(Func<HeadcountSnapshot> getSnapshot)
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
            ReportOutcome("Selected survivor headcount entry: " + GetSelectedSurvivorName());
            return true;
        }

        public bool SelectPreviousSurvivor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
                return false;
            SelectedSurvivorIndex = (SelectedSurvivorIndex - 1 + _snapshot.entries.Count) % _snapshot.entries.Count;
            ReportOutcome("Selected survivor headcount entry: " + GetSelectedSurvivorName());
            return true;
        }

        public bool RequestDispatchSearchParty()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                ReportOutcome("No missing survivor selected for search party dispatch.");
                return false;
            }

            var entry = GetSelectedSurvivor();
            if (entry == null) return false;

            if (!entry.isMissing)
            {
                ReportOutcome(entry.survivorName + " is present and accounted for.");
                return false;
            }

            if (OnDispatchSearchPartyRequested == null)
            {
                ReportOutcome("Search party dispatcher link offline.");
                return false;
            }

            OnDispatchSearchPartyRequested.Invoke(entry.survivorId);
            ReportOutcome("Dispatching Search Party to find missing survivor " + entry.survivorName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No headcount action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHeadcountChanged?.Invoke();
        }

        private HeadcountEntrySnapshot GetSelectedSurvivor()
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
            var sb = new StringBuilder("SHELTER ROLL CALL & HEADCOUNT  [C] close  ·  [Tab] cycle  ·  [S] search party dispatch");

            if (_snapshot == null)
            {
                sb.Append("\nHeadcount registry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nPOPULATION STATS: Total Shelter Population: ").Append(_snapshot.totalBunkerPopulation)
              .Append("  ·  Missing Survivors: ").Append(_snapshot.totalMissingSurvivors);

            sb.Append("\n\nROLL CALL REGISTRY:");
            if (_snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                sb.Append("\n  No survivors registered in shelter database.");
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
                      .Append(" — Last Seen: ").Append(entry.lastSeenLocation ?? "Bunker Living Quarters");

                    if (entry.isMissing) sb.Append("  ★ [MISSING IN ACTION — DISPATCH SEARCH PARTY]");
                    else if (entry.isAccountedFor) sb.Append("  ✔ [ACCOUNTED FOR]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nROLL CALL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
