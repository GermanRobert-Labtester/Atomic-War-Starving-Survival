using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GutsScavengingSnapshot
    {
        public int gutStringsCrafted;
        public int boneHooksCrafted;
        public float organFatHarvestedKg;
        public float butcherInfectionRiskPercent;
    }

    /// <summary>
    /// Protocol Zero — Guts Scavenging & Animal Organ Harvesting HUD view-model.
    /// Controls animal carcass viscera butchering: gut strings (bowstrings / sutures),
    /// bone fishing hooks, organ tallow harvesting, and butcher bacterial infection risk.
    /// </summary>
    public class GutsScavengingHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGutsScavengingChanged;
        public event Action OnCraftGutStringRequested;
        public event Action OnCraftBoneHookRequested;

        private Func<GutsScavengingSnapshot> _getSnapshot;
        private GutsScavengingSnapshot _snapshot;

        public void Bind(Func<GutsScavengingSnapshot> getSnapshot)
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

        public bool RequestCraftGutString()
        {
            if (!IsOpen) return false;
            if (OnCraftGutStringRequested == null)
            {
                ReportOutcome("Butcher bench link offline.");
                return false;
            }

            OnCraftGutStringRequested.Invoke();
            ReportOutcome("Processing animal entrails into Gut String (Bowstring / Suture)...");
            return true;
        }

        public bool RequestCraftBoneHook()
        {
            if (!IsOpen) return false;
            if (OnCraftBoneHookRequested == null)
            {
                ReportOutcome("Butcher bench link offline.");
                return false;
            }

            OnCraftBoneHookRequested.Invoke();
            ReportOutcome("Carving animal bone into Fishing Hook...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No guts action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGutsScavengingChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GUTS & ANIMAL ORGAN SCAVENGING  [U] close  ·  [S] gut string  ·  [H] bone hook");

            if (_snapshot == null)
            {
                sb.Append("\nButcher morgue telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nBUTCHER HARVEST STATS: Gut Strings: ").Append(_snapshot.gutStringsCrafted)
              .Append("  ·  Bone Hooks: ").Append(_snapshot.boneHooksCrafted)
              .Append("  ·  Organ Fat Harvested: ").Append(_snapshot.organFatHarvestedKg.ToString("0.#")).Append(" kg");

            sb.Append("\n  · Butcher Bacterial Infection Risk: ").Append(_snapshot.butcherInfectionRiskPercent.ToString("0")).Append("%");

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nBUTCHER LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
