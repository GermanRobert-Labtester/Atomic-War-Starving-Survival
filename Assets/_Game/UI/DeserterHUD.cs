using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class DeserterEntrySnapshot
    {
        public string survivorId;
        public string originFactionId;
        public bool isSpy;
        public bool spyRevealed;
        public float spyDaysUntilSabotage;
        public bool sabotageExecuted;
    }

    public class DeserterSnapshot
    {
        public int totalDesertersAccepted;
        public int totalSpiesRevealed;
        public int totalSabotagesExecuted;
        public List<DeserterEntrySnapshot> deserters = new List<DeserterEntrySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Faction Deserter & Spy Sabotage HUD view-model.
    /// Manages recruit deserters seeking asylum, hidden 30% spy probability detection,
    /// spy interrogation, tribunal executions, and air filter / hatch sabotage prevention.
    /// </summary>
    public class DeserterHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedDeserterIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnDeserterChanged;
        public event Action<string> OnInterrogateDeserterRequested; // (survivorId)
        public event Action<string> OnExecuteDeserterRequested;     // (survivorId)

        private Func<DeserterSnapshot> _getSnapshot;
        private DeserterSnapshot _snapshot;

        public void Bind(Func<DeserterSnapshot> getSnapshot)
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

        public bool SelectNextDeserter()
        {
            if (!IsOpen || _snapshot == null || _snapshot.deserters == null || _snapshot.deserters.Count == 0)
                return false;
            SelectedDeserterIndex = (SelectedDeserterIndex + 1) % _snapshot.deserters.Count;
            ReportOutcome("Selected deserter record: " + GetSelectedDeserterName());
            return true;
        }

        public bool SelectPreviousDeserter()
        {
            if (!IsOpen || _snapshot == null || _snapshot.deserters == null || _snapshot.deserters.Count == 0)
                return false;
            SelectedDeserterIndex = (SelectedDeserterIndex - 1 + _snapshot.deserters.Count) % _snapshot.deserters.Count;
            ReportOutcome("Selected deserter record: " + GetSelectedDeserterName());
            return true;
        }

        public bool RequestInterrogateDeserter()
        {
            if (!IsOpen || _snapshot == null || _snapshot.deserters == null || _snapshot.deserters.Count == 0)
            {
                ReportOutcome("No deserter selected for interrogation.");
                return false;
            }

            var entry = GetSelectedDeserter();
            if (entry == null) return false;

            if (OnInterrogateDeserterRequested == null)
            {
                ReportOutcome("Counter-intelligence link offline.");
                return false;
            }

            OnInterrogateDeserterRequested.Invoke(entry.survivorId);
            ReportOutcome("Interrogating deserter " + entry.survivorId + " from " + entry.originFactionId + "...");
            return true;
        }

        public bool RequestExecuteDeserter()
        {
            if (!IsOpen || _snapshot == null || _snapshot.deserters == null || _snapshot.deserters.Count == 0)
            {
                ReportOutcome("No deserter selected for tribunal execution.");
                return false;
            }

            var entry = GetSelectedDeserter();
            if (entry == null) return false;

            if (OnExecuteDeserterRequested == null)
            {
                ReportOutcome("Tribunal execution link offline.");
                return false;
            }

            OnExecuteDeserterRequested.Invoke(entry.survivorId);
            ReportOutcome("Executing tribunal verdict against spy " + entry.survivorId + "!");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No deserter action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnDeserterChanged?.Invoke();
        }

        private DeserterEntrySnapshot GetSelectedDeserter()
        {
            if (_snapshot != null && _snapshot.deserters != null && SelectedDeserterIndex >= 0 && SelectedDeserterIndex < _snapshot.deserters.Count)
            {
                return _snapshot.deserters[SelectedDeserterIndex];
            }
            return null;
        }

        private string GetSelectedDeserterName()
        {
            var d = GetSelectedDeserter();
            return d != null ? (d.survivorId + " [" + d.originFactionId + "]") : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("DESERTERS & COUNTER-INTELLIGENCE MONITOR  [D] close  ·  [Tab] cycle  ·  [I] interrogate  ·  [E] execute tribunal");

            if (_snapshot == null)
            {
                sb.Append("\nDeserter telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nRECRUITMENT STATS: Deserters Accepted: ").Append(_snapshot.totalDesertersAccepted)
              .Append("  ·  Spies Unmasked: ").Append(_snapshot.totalSpiesRevealed)
              .Append("  ·  Sabotages Executed: ").Append(_snapshot.totalSabotagesExecuted);

            sb.Append("\n\nACCEPTED DESERTERS IN SHELTER:");
            if (_snapshot.deserters == null || _snapshot.deserters.Count == 0)
            {
                sb.Append("\n  No faction deserters currently housed in shelter.");
            }
            else
            {
                for (int i = 0; i < _snapshot.deserters.Count; i++)
                {
                    var entry = _snapshot.deserters[i];
                    if (entry == null) continue;

                    bool selected = (i == SelectedDeserterIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(entry.survivorId)
                      .Append(" (Origin: ").Append(entry.originFactionId).Append(")");

                    if (entry.sabotageExecuted) sb.Append("  ✔ [SABOTAGE EXECUTED — HATCH / AIR FILTER BREACHED]");
                    else if (entry.spyRevealed) sb.Append("  ★ [UNMASKED SPY — ").Append(entry.spyDaysUntilSabotage.ToString("0.#")).Append(" days until sabotage]");
                    else sb.Append("  [AUTHENTICATED DESERTER - 30% SPY RISK]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nINTEL LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
