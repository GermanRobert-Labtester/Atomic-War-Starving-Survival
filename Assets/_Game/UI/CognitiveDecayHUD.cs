using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class CognitiveDecayEntrySnapshot
    {
        public string survivorId;
        public string survivorName;
        public bool isCognitiveDecayActive;
        public bool isUtilityAIFailing;
        public float errantActionChance; // 0..1
        public string lastErraticBehavior;
    }

    public class CognitiveDecaySnapshot
    {
        public int totalPatientsInDecay;
        public int totalErraticBehaviorsTriggered;
        public List<CognitiveDecayEntrySnapshot> entries = new List<CognitiveDecayEntrySnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Cognitive Decay & ARS Stage 4 Brain Degradation HUD view-model.
    /// Monitors extreme radiation sickness brain decay, UtilityAI control failures,
    /// erratic survivor behaviors (dropping inventory, wasting fuel/recipes, wandering into walls),
    /// and chelation therapy intervention.
    /// </summary>
    public class CognitiveDecayHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedEntryIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnCognitiveDecayChanged;
        public event Action<string> OnAdministerChelationRequested; // (survivorId)

        private Func<CognitiveDecaySnapshot> _getSnapshot;
        private CognitiveDecaySnapshot _snapshot;

        public void Bind(Func<CognitiveDecaySnapshot> getSnapshot)
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

        public bool SelectNextEntry()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
                return false;
            SelectedEntryIndex = (SelectedEntryIndex + 1) % _snapshot.entries.Count;
            ReportOutcome("Selected cognitive decay patient: " + GetSelectedEntryName());
            return true;
        }

        public bool SelectPreviousEntry()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
                return false;
            SelectedEntryIndex = (SelectedEntryIndex - 1 + _snapshot.entries.Count) % _snapshot.entries.Count;
            ReportOutcome("Selected cognitive decay patient: " + GetSelectedEntryName());
            return true;
        }

        public bool RequestAdministerChelation()
        {
            if (!IsOpen || _snapshot == null || _snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                ReportOutcome("No patient selected for radiation chelation therapy.");
                return false;
            }

            var entry = GetSelectedEntry();
            if (entry == null) return false;

            if (!entry.isCognitiveDecayActive)
            {
                ReportOutcome(entry.survivorName + " is not suffering from cognitive decay.");
                return false;
            }

            if (OnAdministerChelationRequested == null)
            {
                ReportOutcome("Medical chelation link offline.");
                return false;
            }

            OnAdministerChelationRequested.Invoke(entry.survivorId);
            ReportOutcome("Administering chelation therapy (Prussian Blue / DTPA) to " + entry.survivorName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No cognitive decay action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnCognitiveDecayChanged?.Invoke();
        }

        private CognitiveDecayEntrySnapshot GetSelectedEntry()
        {
            if (_snapshot != null && _snapshot.entries != null && SelectedEntryIndex >= 0 && SelectedEntryIndex < _snapshot.entries.Count)
            {
                return _snapshot.entries[SelectedEntryIndex];
            }
            return null;
        }

        private string GetSelectedEntryName()
        {
            var e = GetSelectedEntry();
            return e != null ? (e.survivorName ?? e.survivorId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("ARS STAGE 4 — COGNITIVE DECAY MONITOR  [G] close  ·  [Tab] cycle  ·  [C] chelation therapy");

            if (_snapshot == null)
            {
                sb.Append("\nCognitive decay telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nMONITOR STATS: Active Brain Decay: ").Append(_snapshot.totalPatientsInDecay)
              .Append("  ·  Erratic Incidents Triggered: ").Append(_snapshot.totalErraticBehaviorsTriggered);

            sb.Append("\n\nARS STAGE 4 PATIENTS (BRAIN DEGRADATION):");
            if (_snapshot.entries == null || _snapshot.entries.Count == 0)
            {
                sb.Append("\n  No survivors suffering from radiation cognitive decay.");
            }
            else
            {
                for (int i = 0; i < _snapshot.entries.Count; i++)
                {
                    var entry = _snapshot.entries[i];
                    if (entry == null) continue;

                    bool selected = (i == SelectedEntryIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(entry.survivorName ?? entry.survivorId)
                      .Append(" — Erratic Chance: ").Append((entry.errantActionChance * 100f).ToString("0")).Append("%")
                      .Append(" | UtilityAI: ").Append(entry.isUtilityAIFailing ? "[FAILING - LOSS OF AUTONOMY]" : "[STABLE]");

                    if (!string.IsNullOrEmpty(entry.lastErraticBehavior))
                        sb.Append("\n    [LAST BEHAVIOR: ").Append(entry.lastErraticBehavior).Append("]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nDECAY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
