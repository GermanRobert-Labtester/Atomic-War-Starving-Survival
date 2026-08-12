using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class FavorEntrySnapshot
    {
        public string favorType;
        public string factionId;
        public string description;
        public int incurredDay;
        public bool isCollected;
    }

    public class DebtAndFavorSnapshot
    {
        public List<FavorEntrySnapshot> activeFavors = new List<FavorEntrySnapshot>();
        public List<FavorEntrySnapshot> collectedFavors = new List<FavorEntrySnapshot>();
        public int totalFavorsIncurred;
    }

    /// <summary>
    /// Protocol Zero — Debt and Favor HUD view-model.
    /// Manages political leverage, faction blood debts, favor collections
    /// (Tithe of Hands labor, Iron Leash coercion, Glow's Embrace rites),
    /// and debt refusal consequences.
    /// </summary>
    public class DebtAndFavorHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedFavorIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnDebtAndFavorChanged;
        public event Action<string> OnFavorCollectionRequested;
        public event Action<string> OnFavorRefusalRequested;

        private Func<DebtAndFavorSnapshot> _getSnapshot;
        private DebtAndFavorSnapshot _snapshot;

        public void Bind(Func<DebtAndFavorSnapshot> getSnapshot)
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

        public bool SelectNextFavor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeFavors == null || _snapshot.activeFavors.Count == 0)
                return false;
            SelectedFavorIndex = (SelectedFavorIndex + 1) % _snapshot.activeFavors.Count;
            ReportOutcome("Selected favor debt: " + GetSelectedFavorDescription());
            return true;
        }

        public bool SelectPreviousFavor()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeFavors == null || _snapshot.activeFavors.Count == 0)
                return false;
            SelectedFavorIndex = (SelectedFavorIndex - 1 + _snapshot.activeFavors.Count) % _snapshot.activeFavors.Count;
            ReportOutcome("Selected favor debt: " + GetSelectedFavorDescription());
            return true;
        }

        public bool RequestCollectSelected()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeFavors == null || _snapshot.activeFavors.Count == 0)
            {
                ReportOutcome("No active blood debt selected for collection.");
                return false;
            }

            if (SelectedFavorIndex < 0 || SelectedFavorIndex >= _snapshot.activeFavors.Count)
                SelectedFavorIndex = 0;

            var favor = _snapshot.activeFavors[SelectedFavorIndex];
            if (favor == null) return false;

            if (OnFavorCollectionRequested == null)
            {
                ReportOutcome("Faction diplomacy link offline.");
                return false;
            }

            OnFavorCollectionRequested.Invoke(favor.favorType);
            ReportOutcome("Honoring blood debt to " + favor.factionId + ": " + favor.favorType);
            return true;
        }

        public bool RequestRefuseSelected()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeFavors == null || _snapshot.activeFavors.Count == 0)
            {
                ReportOutcome("No active blood debt selected for refusal.");
                return false;
            }

            if (SelectedFavorIndex < 0 || SelectedFavorIndex >= _snapshot.activeFavors.Count)
                SelectedFavorIndex = 0;

            var favor = _snapshot.activeFavors[SelectedFavorIndex];
            if (favor == null) return false;

            if (OnFavorRefusalRequested == null)
            {
                ReportOutcome("Faction diplomacy link offline.");
                return false;
            }

            OnFavorRefusalRequested.Invoke(favor.favorType);
            ReportOutcome("REFUSED blood debt to " + favor.factionId + "! Consequence imminent.");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No debt action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnDebtAndFavorChanged?.Invoke();
        }

        private string GetSelectedFavorDescription()
        {
            if (_snapshot != null && _snapshot.activeFavors != null && SelectedFavorIndex >= 0 && SelectedFavorIndex < _snapshot.activeFavors.Count)
            {
                var f = _snapshot.activeFavors[SelectedFavorIndex];
                return f != null ? (f.factionId + " (" + f.favorType + ")") : "Unknown";
            }
            return "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BLOOD DEBT & FAVOR ECONOMY  [F] close  ·  [Tab] cycle  ·  [C] collect  ·  [R] refuse");

            if (_snapshot == null)
            {
                sb.Append("\nFaction ledger telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nFACTION LEDGER: ").Append(_snapshot.activeFavors?.Count ?? 0).Append(" Active Debts  ·  ")
              .Append(_snapshot.collectedFavors?.Count ?? 0).Append(" Settled Debts  ·  Total: ").Append(_snapshot.totalFavorsIncurred);

            sb.Append("\n\nACTIVE BLOOD DEBTS (Owed to Factions):");
            if (_snapshot.activeFavors == null || _snapshot.activeFavors.Count == 0)
            {
                sb.Append("\n  You owe no blood debts to outside factions.");
            }
            else
            {
                for (int i = 0; i < _snapshot.activeFavors.Count; i++)
                {
                    var favor = _snapshot.activeFavors[i];
                    if (favor == null) continue;

                    bool selected = (i == SelectedFavorIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("[").Append(favor.factionId).Append("] ")
                      .Append(favor.description ?? favor.favorType)
                      .Append(" (Day ").Append(favor.incurredDay).Append(")");
                }
            }

            sb.Append("\n\nSETTLED / EXACTION HISTORY:");
            if (_snapshot.collectedFavors == null || _snapshot.collectedFavors.Count == 0)
            {
                sb.Append("\n  No debts collected yet.");
            }
            else
            {
                foreach (var favor in _snapshot.collectedFavors)
                {
                    if (favor == null) continue;
                    sb.Append("\n  ✓ [").Append(favor.factionId).Append("] ").Append(favor.favorType).Append(" [COLLECTED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nLEDGER LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
