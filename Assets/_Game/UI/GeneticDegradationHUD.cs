using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GeneticMutationSnapshot
    {
        public string survivorId;
        public string survivorName;
        public string mutationName;
        public int mutationStage; // 1..3
        public int radAwayInjectionsCount;
        public bool isCritical;
    }

    public class GeneticDegradationSnapshot
    {
        public int totalMutationsRecorded;
        public int totalRadAwayAdministered;
        public List<GeneticMutationSnapshot> mutations = new List<GeneticMutationSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Genetic Degradation & Mutation Tracking HUD view-model.
    /// Monitors chronic radiation genetic mutations, mutation stage progression (1..3),
    /// Anti-Rad/Rad-Away injection therapies, and cellular DNA stability telemetry.
    /// </summary>
    public class GeneticDegradationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedMutationIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGeneticDegradationChanged;
        public event Action<string> OnAdministerRadAwayRequested; // (survivorId)

        private Func<GeneticDegradationSnapshot> _getSnapshot;
        private GeneticDegradationSnapshot _snapshot;

        public void Bind(Func<GeneticDegradationSnapshot> getSnapshot)
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

        public bool SelectNextMutation()
        {
            if (!IsOpen || _snapshot == null || _snapshot.mutations == null || _snapshot.mutations.Count == 0)
                return false;
            SelectedMutationIndex = (SelectedMutationIndex + 1) % _snapshot.mutations.Count;
            ReportOutcome("Selected genetic mutation record: " + GetSelectedMutationName());
            return true;
        }

        public bool SelectPreviousMutation()
        {
            if (!IsOpen || _snapshot == null || _snapshot.mutations == null || _snapshot.mutations.Count == 0)
                return false;
            SelectedMutationIndex = (SelectedMutationIndex - 1 + _snapshot.mutations.Count) % _snapshot.mutations.Count;
            ReportOutcome("Selected genetic mutation record: " + GetSelectedMutationName());
            return true;
        }

        public bool RequestAdministerRadAway()
        {
            if (!IsOpen || _snapshot == null || _snapshot.mutations == null || _snapshot.mutations.Count == 0)
            {
                ReportOutcome("No mutation patient selected for Rad-Away injection.");
                return false;
            }

            var mutation = GetSelectedMutation();
            if (mutation == null) return false;

            if (OnAdministerRadAwayRequested == null)
            {
                ReportOutcome("Rad-Away injector link offline.");
                return false;
            }

            OnAdministerRadAwayRequested.Invoke(mutation.survivorId);
            ReportOutcome("Administering Rad-Away injection to " + mutation.survivorName + " (Arresting genetic mutation)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No genetic action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGeneticDegradationChanged?.Invoke();
        }

        private GeneticMutationSnapshot GetSelectedMutation()
        {
            if (_snapshot != null && _snapshot.mutations != null && SelectedMutationIndex >= 0 && SelectedMutationIndex < _snapshot.mutations.Count)
            {
                return _snapshot.mutations[SelectedMutationIndex];
            }
            return null;
        }

        private string GetSelectedMutationName()
        {
            var m = GetSelectedMutation();
            return m != null ? (m.survivorName + " — " + m.mutationName) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GENETIC DEGRADATION & DNA MUTATION MONITOR  [N] close  ·  [Tab] cycle  ·  [A] administer rad-away");

            if (_snapshot == null)
            {
                sb.Append("\nGenetic DNA telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nGENETIC STATS: Mutations Recorded: ").Append(_snapshot.totalMutationsRecorded)
              .Append("  ·  Rad-Away Doses Administered: ").Append(_snapshot.totalRadAwayAdministered);

            sb.Append("\n\nGENETIC MUTATION RECORDS:");
            if (_snapshot.mutations == null || _snapshot.mutations.Count == 0)
            {
                sb.Append("\n  No genetic mutations detected in survivor genome.");
            }
            else
            {
                for (int i = 0; i < _snapshot.mutations.Count; i++)
                {
                    var mut = _snapshot.mutations[i];
                    if (mut == null) continue;

                    bool selected = (i == SelectedMutationIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(mut.survivorName ?? mut.survivorId)
                      .Append(" — Mutation: ").Append(mut.mutationName ?? "Genetic Shift")
                      .Append(" (Stage ").Append(mut.mutationStage).Append(" / 3)")
                      .Append(" | Injections: ").Append(mut.radAwayInjectionsCount);

                    if (mut.isCritical) sb.Append("  ★ [CRITICAL STAGE 3 MUTATION]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nGENETIC LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
