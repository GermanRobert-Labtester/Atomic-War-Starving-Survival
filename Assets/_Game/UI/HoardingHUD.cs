using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class HoardingStashSnapshot
    {
        public string survivorId;
        public string survivorName;
        public int stashedItemsCount;
        public string stashLocation;
        public bool isDiscoveredByGroup;
    }

    public class HoardingSnapshot
    {
        public int totalSecretStashesCount;
        public int totalHoardedItemsCount;
        public List<HoardingStashSnapshot> stashes = new List<HoardingStashSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Secret Hoarding & Bunker Stash HUD view-model.
    /// Monitors survivor resource hoarding, secret stashes under mattress bunks,
    /// group discovery friction, confiscation dispatch, and inventory tension.
    /// </summary>
    public class HoardingHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedStashIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHoardingChanged;
        public event Action<string> OnConfiscateStashRequested; // (survivorId)

        private Func<HoardingSnapshot> _getSnapshot;
        private HoardingSnapshot _snapshot;

        public void Bind(Func<HoardingSnapshot> getSnapshot)
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

        public bool SelectNextStash()
        {
            if (!IsOpen || _snapshot == null || _snapshot.stashes == null || _snapshot.stashes.Count == 0)
                return false;
            SelectedStashIndex = (SelectedStashIndex + 1) % _snapshot.stashes.Count;
            ReportOutcome("Selected secret stash: " + GetSelectedStashName());
            return true;
        }

        public bool SelectPreviousStash()
        {
            if (!IsOpen || _snapshot == null || _snapshot.stashes == null || _snapshot.stashes.Count == 0)
                return false;
            SelectedStashIndex = (SelectedStashIndex - 1 + _snapshot.stashes.Count) % _snapshot.stashes.Count;
            ReportOutcome("Selected secret stash: " + GetSelectedStashName());
            return true;
        }

        public bool RequestConfiscateStash()
        {
            if (!IsOpen || _snapshot == null || _snapshot.stashes == null || _snapshot.stashes.Count == 0)
            {
                ReportOutcome("No secret stash selected for confiscation.");
                return false;
            }

            var stash = GetSelectedStash();
            if (stash == null) return false;

            if (OnConfiscateStashRequested == null)
            {
                ReportOutcome("Bunker search inspector link offline.");
                return false;
            }

            OnConfiscateStashRequested.Invoke(stash.survivorId);
            ReportOutcome("Confiscating hoarded items from " + stash.survivorName + "'s stash at " + stash.stashLocation + " (Morale hit to hoarder!)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No hoarding action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHoardingChanged?.Invoke();
        }

        private HoardingStashSnapshot GetSelectedStash()
        {
            if (_snapshot != null && _snapshot.stashes != null && SelectedStashIndex >= 0 && SelectedStashIndex < _snapshot.stashes.Count)
            {
                return _snapshot.stashes[SelectedStashIndex];
            }
            return null;
        }

        private string GetSelectedStashName()
        {
            var s = GetSelectedStash();
            return s != null ? (s.survivorName + " @ " + s.stashLocation) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SECRET HOARDING & BUNKER STASH MONITOR  [S] close  ·  [Tab] cycle  ·  [C] confiscate hoarded items");

            if (_snapshot == null)
            {
                sb.Append("\nHoarding monitor telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nHOARDING STATS: Secret Stashes Found: ").Append(_snapshot.totalSecretStashesCount)
              .Append("  ·  Total Hoarded Items: ").Append(_snapshot.totalHoardedItemsCount);

            sb.Append("\n\nDETECTED SURVIVOR HOARD STASHES:");
            if (_snapshot.stashes == null || _snapshot.stashes.Count == 0)
            {
                sb.Append("\n  No secret hoarded stashes detected in shelter.");
            }
            else
            {
                for (int i = 0; i < _snapshot.stashes.Count; i++)
                {
                    var stash = _snapshot.stashes[i];
                    if (stash == null) continue;

                    bool selected = (i == SelectedStashIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(stash.survivorName ?? stash.survivorId)
                      .Append(" @ ").Append(stash.stashLocation ?? "Under Bunk Mattress")
                      .Append(" — Hoarded Items: ").Append(stash.stashedItemsCount);

                    if (stash.isDiscoveredByGroup) sb.Append("  ★ [DISCOVERED BY GROUP — TENSION HIGH]");
                    else sb.Append("  [UNDISCOVERED STASH]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nHOARDING LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
