using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class InterpersonalRelationshipSnapshot
    {
        public string survivorIdA;
        public string survivorIdB;
        public float affinityScore; // -100..100
        public string relationshipType; // e.g. "Lovers", "Feud", "Brig Imprisoned", "Banished", "Pregnant", "Smuggling Alliance"
        public string details;
    }

    public class BunkerSocialSnapshot
    {
        public int totalActiveLovers;
        public int totalActiveFeuds;
        public int totalImprisonedInBrig;
        public int totalBanished;
        public List<InterpersonalRelationshipSnapshot> relationships = new List<InterpersonalRelationshipSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Bunker Social Director & Interpersonal Matrix HUD view-model.
    /// Monitors survivor relationships, romance & breakups, active feuds & passive sabotage,
    /// mutiny leadership challenges, brig imprisonment, banishment, pregnancy, and black market smuggling.
    /// </summary>
    public class BunkerSocialHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedRelIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnBunkerSocialChanged;
        public event Action<string, string> OnImprisonSurvivorRequested; // (survivorId, reason)
        public event Action<string, string> OnBanishSurvivorRequested;   // (survivorId, reason)

        private Func<BunkerSocialSnapshot> _getSnapshot;
        private BunkerSocialSnapshot _snapshot;

        public void Bind(Func<BunkerSocialSnapshot> getSnapshot)
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

        public bool SelectNextRelationship()
        {
            if (!IsOpen || _snapshot == null || _snapshot.relationships == null || _snapshot.relationships.Count == 0)
                return false;
            SelectedRelIndex = (SelectedRelIndex + 1) % _snapshot.relationships.Count;
            ReportOutcome("Selected relationship: " + GetSelectedRelName());
            return true;
        }

        public bool SelectPreviousRelationship()
        {
            if (!IsOpen || _snapshot == null || _snapshot.relationships == null || _snapshot.relationships.Count == 0)
                return false;
            SelectedRelIndex = (SelectedRelIndex - 1 + _snapshot.relationships.Count) % _snapshot.relationships.Count;
            ReportOutcome("Selected relationship: " + GetSelectedRelName());
            return true;
        }

        public bool RequestImprisonSelected(string reason)
        {
            if (!IsOpen || _snapshot == null || _snapshot.relationships == null || _snapshot.relationships.Count == 0)
            {
                ReportOutcome("No survivor selected for brig imprisonment.");
                return false;
            }

            var rel = GetSelectedRel();
            if (rel == null) return false;

            if (OnImprisonSurvivorRequested == null)
            {
                ReportOutcome("Brig warden link offline.");
                return false;
            }

            OnImprisonSurvivorRequested.Invoke(rel.survivorIdA, reason ?? "insubordination");
            ReportOutcome("Imprisoning survivor " + rel.survivorIdA + " in the Brig (Reason: " + (reason ?? "insubordination") + ")...");
            return true;
        }

        public bool RequestBanishSelected(string reason)
        {
            if (!IsOpen || _snapshot == null || _snapshot.relationships == null || _snapshot.relationships.Count == 0)
            {
                ReportOutcome("No survivor selected for surface banishment.");
                return false;
            }

            var rel = GetSelectedRel();
            if (rel == null) return false;

            if (OnBanishSurvivorRequested == null)
            {
                ReportOutcome("Banishment council link offline.");
                return false;
            }

            OnBanishSurvivorRequested.Invoke(rel.survivorIdA, reason ?? "exile");
            ReportOutcome("BANISHING survivor " + rel.survivorIdA + " to the surface wasteland (Exile!)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No social action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnBunkerSocialChanged?.Invoke();
        }

        private InterpersonalRelationshipSnapshot GetSelectedRel()
        {
            if (_snapshot != null && _snapshot.relationships != null && SelectedRelIndex >= 0 && SelectedRelIndex < _snapshot.relationships.Count)
            {
                return _snapshot.relationships[SelectedRelIndex];
            }
            return null;
        }

        private string GetSelectedRelName()
        {
            var r = GetSelectedRel();
            return r != null ? (r.survivorIdA + " & " + (r.survivorIdB ?? "Bunker")) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BUNKER SOCIAL DIRECTOR & RELATIONSHIP MATRIX  [Y] close  ·  [Tab] cycle  ·  [I] brig  ·  [B] banish");

            if (_snapshot == null)
            {
                sb.Append("\nSocial matrix telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSOCIAL STATS: Active Lovers: ").Append(_snapshot.totalActiveLovers)
              .Append("  ·  Active Feuds: ").Append(_snapshot.totalActiveFeuds)
              .Append("  ·  Brig Prisoners: ").Append(_snapshot.totalImprisonedInBrig)
              .Append("  ·  Banished: ").Append(_snapshot.totalBanished);

            sb.Append("\n\nINTERPERSONAL RELATIONSHIPS & TENSIONS:");
            if (_snapshot.relationships == null || _snapshot.relationships.Count == 0)
            {
                sb.Append("\n  No active interpersonal conflicts or alliances in shelter.");
            }
            else
            {
                for (int i = 0; i < _snapshot.relationships.Count; i++)
                {
                    var rel = _snapshot.relationships[i];
                    if (rel == null) continue;

                    bool selected = (i == SelectedRelIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(rel.survivorIdA)
                      .Append(string.IsNullOrEmpty(rel.survivorIdB) ? "" : (" <—> " + rel.survivorIdB))
                      .Append(" — Type: ").Append(rel.relationshipType ?? "Neutral")
                      .Append(" (Affinity: ").Append(rel.affinityScore.ToString("+0;-0;0")).Append(")");

                    if (!string.IsNullOrEmpty(rel.details))
                        sb.Append("\n    [").Append(rel.details).Append("]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nSOCIAL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
