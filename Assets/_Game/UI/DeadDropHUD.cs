using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class DeadDropSnapshotEntry
    {
        public string dropId;
        public string nodeId;
        public string factionId;
        public float hoursUntilResolve;
        public bool isResolved;
        public bool wasStolen;
        public string[] depositedItems;
        public string[] expectedReturnItems;
    }

    public class DeadDropSnapshot
    {
        public int totalSuccessfulDrops;
        public int totalStolenDrops;
        public List<DeadDropSnapshotEntry> drops = new List<DeadDropSnapshotEntry>();
    }

    /// <summary>
    /// Protocol Zero — Dead Drop Contactless Trade HUD view-model.
    /// Monitors secret locker trade stashes across wasteland nodes, 48-hour resolution timers,
    /// 15% scavenger theft risk, deposited vs expected return items, and faction trust bonuses.
    /// </summary>
    public class DeadDropHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedDropIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnDeadDropChanged;
        public event Action<string, string> OnPlaceDeadDropRequested; // (nodeId, factionId)

        private Func<DeadDropSnapshot> _getSnapshot;
        private DeadDropSnapshot _snapshot;

        public void Bind(Func<DeadDropSnapshot> getSnapshot)
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

        public bool SelectNextDrop()
        {
            if (!IsOpen || _snapshot == null || _snapshot.drops == null || _snapshot.drops.Count == 0)
                return false;
            SelectedDropIndex = (SelectedDropIndex + 1) % _snapshot.drops.Count;
            ReportOutcome("Selected dead drop location: " + GetSelectedDropName());
            return true;
        }

        public bool SelectPreviousDrop()
        {
            if (!IsOpen || _snapshot == null || _snapshot.drops == null || _snapshot.drops.Count == 0)
                return false;
            SelectedDropIndex = (SelectedDropIndex - 1 + _snapshot.drops.Count) % _snapshot.drops.Count;
            ReportOutcome("Selected dead drop location: " + GetSelectedDropName());
            return true;
        }

        public bool RequestPlaceDeadDrop(string nodeId, string factionId)
        {
            if (!IsOpen) return false;
            if (OnPlaceDeadDropRequested == null)
            {
                ReportOutcome("Dead drop dispatch link offline.");
                return false;
            }

            OnPlaceDeadDropRequested.Invoke(nodeId ?? "node_ruined_locker", factionId ?? "garrison");
            ReportOutcome("Placing contactless Dead Drop at " + (nodeId ?? "ruined locker") + " for " + (factionId ?? "garrison") + " (48h resolve)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No dead drop action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnDeadDropChanged?.Invoke();
        }

        private DeadDropSnapshotEntry GetSelectedDrop()
        {
            if (_snapshot != null && _snapshot.drops != null && SelectedDropIndex >= 0 && SelectedDropIndex < _snapshot.drops.Count)
            {
                return _snapshot.drops[SelectedDropIndex];
            }
            return null;
        }

        private string GetSelectedDropName()
        {
            var d = GetSelectedDrop();
            return d != null ? (d.nodeId + " [" + d.factionId + "]") : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("DEAD DROP CONTACTLESS TRADE  [L] close  ·  [Tab] cycle  ·  [P] place new drop");

            if (_snapshot == null)
            {
                sb.Append("\nDead drop telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nTRADE STATS: Successful Drops: ").Append(_snapshot.totalSuccessfulDrops)
              .Append("  ·  Stolen by Scavengers: ").Append(_snapshot.totalStolenDrops);

            sb.Append("\n\nACTIVE & COMPLETED DEAD DROPS:");
            if (_snapshot.drops == null || _snapshot.drops.Count == 0)
            {
                sb.Append("\n  No active dead drop stashes placed in wasteland.");
            }
            else
            {
                for (int i = 0; i < _snapshot.drops.Count; i++)
                {
                    var drop = _snapshot.drops[i];
                    if (drop == null) continue;

                    bool selected = (i == SelectedDropIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(drop.dropId ?? "Drop")
                      .Append(" @ ").Append(drop.nodeId)
                      .Append(" [").Append(drop.factionId).Append("]");

                    if (drop.wasStolen) sb.Append("  ✖ [STOLEN BY SCAVENGERS - MORALE PENALTY]");
                    else if (drop.isResolved) sb.Append("  ✔ [TRADE COMPLETED +5 TRUST]");
                    else sb.Append("  (").Append(drop.hoursUntilResolve.ToString("0.#")).Append(" hrs remaining)");

                    if (drop.depositedItems != null && drop.depositedItems.Length > 0)
                        sb.Append("\n    [DEPOSITED: ").Append(string.Join(", ", drop.depositedItems)).Append("]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nTRADE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
