using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GlowRiteSnapshot
    {
        public int totalRitesPerformed;
        public float cultDevotionLevel; // 0..100
        public int irradiatedCommunionDoses;
        public float moraleBoostPerRite;
        public bool isCommunionActive;
    }

    /// <summary>
    /// Protocol Zero — Glow Rite & Cult Ritual HUD view-model.
    /// Monitors Cult of the Glow ritual rites, radiation communion doses,
    /// cult devotion progression, morale stabilization rites, and sacred glow telemetry.
    /// </summary>
    public class GlowRiteHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGlowRiteChanged;
        public event Action OnPerformGlowRiteRequested;

        private Func<GlowRiteSnapshot> _getSnapshot;
        private GlowRiteSnapshot _snapshot;

        public void Bind(Func<GlowRiteSnapshot> getSnapshot)
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

        public bool RequestPerformRite()
        {
            if (!IsOpen) return false;
            if (OnPerformGlowRiteRequested == null)
            {
                ReportOutcome("Cult altar link offline.");
                return false;
            }

            OnPerformGlowRiteRequested.Invoke();
            ReportOutcome("Performing Sacred Glow Rite (Communion Doses Consumed)...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No glow rite action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGlowRiteChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GLOW RITE & CULT SACRED ALTAR  [G] close  ·  [R] perform glow rite");

            if (_snapshot == null)
            {
                sb.Append("\nAltar telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nCULT COMMUNION STATS: Total Rites: ").Append(_snapshot.totalRitesPerformed)
              .Append("  ·  Devotion Level: ").Append(_snapshot.cultDevotionLevel.ToString("0")).Append("%")
              .Append("  ·  Communion Doses Available: ").Append(_snapshot.irradiatedCommunionDoses);

            sb.Append("\n\nRITUAL STATUS: ");
            if (_snapshot.isCommunionActive)
            {
                sb.Append("[SACRED GLOW COMMUNION IN PROGRESS — MORALE +").Append(_snapshot.moraleBoostPerRite.ToString("0.#")).Append("]");
            }
            else
            {
                sb.Append("[ALTAR INACTIVE — AWAITING RITE]");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nALTAR LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
