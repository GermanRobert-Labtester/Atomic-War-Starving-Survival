using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class FactionInfluenceSnapshot
    {
        public string factionId;
        public string factionName;
        public float reputationScore; // -100..100
        public float territoryControlledPercent; // 0..100
        public string standingDescription;
    }

    public class HegemonySnapshot
    {
        public string dominantFactionId;
        public bool isPariahStatusActive;
        public List<FactionInfluenceSnapshot> factions = new List<FactionInfluenceSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Wasteland Hegemony & Faction Influence Map HUD view-model.
    /// Monitors regional faction standings (Garrison, Militia, Cult of the Glow, Warlords),
    /// territory control percentages, reputation scores (-100 to +100), and pariah status alerts.
    /// </summary>
    public class HegemonyHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedFactionIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnHegemonyChanged;

        private Func<HegemonySnapshot> _getSnapshot;
        private HegemonySnapshot _snapshot;

        public void Bind(Func<HegemonySnapshot> getSnapshot)
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

        public bool SelectNextFaction()
        {
            if (!IsOpen || _snapshot == null || _snapshot.factions == null || _snapshot.factions.Count == 0)
                return false;
            SelectedFactionIndex = (SelectedFactionIndex + 1) % _snapshot.factions.Count;
            ReportOutcome("Selected faction: " + GetSelectedFactionName());
            return true;
        }

        public bool SelectPreviousFaction()
        {
            if (!IsOpen || _snapshot == null || _snapshot.factions == null || _snapshot.factions.Count == 0)
                return false;
            SelectedFactionIndex = (SelectedFactionIndex - 1 + _snapshot.factions.Count) % _snapshot.factions.Count;
            ReportOutcome("Selected faction: " + GetSelectedFactionName());
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No hegemony action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnHegemonyChanged?.Invoke();
        }

        private FactionInfluenceSnapshot GetSelectedFaction()
        {
            if (_snapshot != null && _snapshot.factions != null && SelectedFactionIndex >= 0 && SelectedFactionIndex < _snapshot.factions.Count)
            {
                return _snapshot.factions[SelectedFactionIndex];
            }
            return null;
        }

        private string GetSelectedFactionName()
        {
            var f = GetSelectedFaction();
            return f != null ? (f.factionName ?? f.factionId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("WASTELAND HEGEMONY & FACTION MAP  [M] close  ·  [Tab] cycle");

            if (_snapshot == null)
            {
                sb.Append("\nFaction hegemony map offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nDOMINANCE STATS: Dominant Faction: ").Append(_snapshot.dominantFactionId ?? "Contested");
            if (_snapshot.isPariahStatusActive)
                sb.Append("  [PARIAH STATUS: HOSTILE TO ALL FACTIONS!]");

            sb.Append("\n\nREGIONAL FACTION STANDINGS:");
            if (_snapshot.factions == null || _snapshot.factions.Count == 0)
            {
                sb.Append("\n  No faction data registered.");
            }
            else
            {
                for (int i = 0; i < _snapshot.factions.Count; i++)
                {
                    var faction = _snapshot.factions[i];
                    if (faction == null) continue;

                    bool selected = (i == SelectedFactionIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(faction.factionName ?? faction.factionId)
                      .Append(" — Reputation: ").Append(faction.reputationScore.ToString("+0;-0;0"))
                      .Append(" | Territory: ").Append(faction.territoryControlledPercent.ToString("0")).Append("%")
                      .Append(" | Standing: ").Append(faction.standingDescription ?? "Neutral");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nHEGEMONY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
