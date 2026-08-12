using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class IntruderCaptiveSnapshot
    {
        public string captiveId;
        public string captiveName;
        public string factionAffiliation;
        public float cooperationLevelPercent; // 0..100
        public bool hasConfessed;
        public bool isInterrogatedToday;
    }

    public class IntruderInterrogationSnapshot
    {
        public int totalCaptivesInterrogated;
        public int totalIntelExtracted;
        public List<IntruderCaptiveSnapshot> captives = new List<IntruderCaptiveSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Captured Intruder Interrogation HUD view-model.
    /// Monitors prisoner interrogation in brig cells, truth serum administration,
    /// intel extraction (enemy stash locations), confession state, and prisoner execution/recruitment.
    /// </summary>
    public class IntruderInterrogationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedCaptiveIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnIntruderInterrogationChanged;
        public event Action<string> OnInterrogateCaptiveRequested; // (captiveId)
        public event Action<string> OnRecruitCaptiveRequested; // (captiveId)
        public event Action<string> OnExecuteCaptiveRequested; // (captiveId)

        private Func<IntruderInterrogationSnapshot> _getSnapshot;
        private IntruderInterrogationSnapshot _snapshot;

        public void Bind(Func<IntruderInterrogationSnapshot> getSnapshot)
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

        public bool SelectNextCaptive()
        {
            if (!IsOpen || _snapshot == null || _snapshot.captives == null || _snapshot.captives.Count == 0)
                return false;
            SelectedCaptiveIndex = (SelectedCaptiveIndex + 1) % _snapshot.captives.Count;
            ReportOutcome("Selected prisoner captive: " + GetSelectedCaptiveName());
            return true;
        }

        public bool SelectPreviousCaptive()
        {
            if (!IsOpen || _snapshot == null || _snapshot.captives == null || _snapshot.captives.Count == 0)
                return false;
            SelectedCaptiveIndex = (SelectedCaptiveIndex - 1 + _snapshot.captives.Count) % _snapshot.captives.Count;
            ReportOutcome("Selected prisoner captive: " + GetSelectedCaptiveName());
            return true;
        }

        public bool RequestInterrogateCaptive()
        {
            if (!IsOpen || _snapshot == null || _snapshot.captives == null || _snapshot.captives.Count == 0)
            {
                ReportOutcome("No captive selected for interrogation.");
                return false;
            }

            var captive = GetSelectedCaptive();
            if (captive == null) return false;

            if (OnInterrogateCaptiveRequested == null)
            {
                ReportOutcome("Brig interrogation cell link offline.");
                return false;
            }

            OnInterrogateCaptiveRequested.Invoke(captive.captiveId);
            ReportOutcome("Interrogating prisoner [" + captive.captiveName + "] in Brig Cell...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No interrogation action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnIntruderInterrogationChanged?.Invoke();
        }

        private IntruderCaptiveSnapshot GetSelectedCaptive()
        {
            if (_snapshot != null && _snapshot.captives != null && SelectedCaptiveIndex >= 0 && SelectedCaptiveIndex < _snapshot.captives.Count)
            {
                return _snapshot.captives[SelectedCaptiveIndex];
            }
            return null;
        }

        private string GetSelectedCaptiveName()
        {
            var c = GetSelectedCaptive();
            return c != null ? (c.captiveName ?? c.captiveId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BRIG PRISONER & INTRUDER INTERROGATION  [I] close  ·  [Tab] cycle  ·  [E] interrogate");

            if (_snapshot == null)
            {
                sb.Append("\nBrig cell telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nINTERROGATION STATS: Captives Interrogated: ").Append(_snapshot.totalCaptivesInterrogated)
              .Append("  ·  Intel Documents Extracted: ").Append(_snapshot.totalIntelExtracted);

            sb.Append("\n\nCAPTURED WASTELAND PRISONERS:");
            if (_snapshot.captives == null || _snapshot.captives.Count == 0)
            {
                sb.Append("\n  No captives in brig cells.");
            }
            else
            {
                for (int i = 0; i < _snapshot.captives.Count; i++)
                {
                    var captive = _snapshot.captives[i];
                    if (captive == null) continue;

                    bool selected = (i == SelectedCaptiveIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(captive.captiveName ?? captive.captiveId)
                      .Append(" (Faction: ").Append(captive.factionAffiliation ?? "Unknown Raider").Append(")")
                      .Append(" — Cooperation: ").Append(captive.cooperationLevelPercent.ToString("0")).Append("%");

                    if (captive.hasConfessed) sb.Append("  ✔ [FULL CONFESSION — ENEMY INTEL UNLOCKED]");
                    else if (captive.isInterrogatedToday) sb.Append("  [INTERROGATED TODAY]");
                    else sb.Append("  ★ [AWAITING INTERROGATION]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nBRIG LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
