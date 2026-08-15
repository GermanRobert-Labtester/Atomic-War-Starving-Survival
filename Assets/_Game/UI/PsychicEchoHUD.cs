using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class PsychicWhisperSnapshot
    {
        public string whisperId;
        public float frequencyHz;
        public float intensityPercent;
        public float moraleImpact;
        public bool isSilenced;
    }

    public class PsychicEchoSnapshot
    {
        public int totalWhispersDetected;
        public float sanityIndexPercent; // 0..100
        public List<PsychicWhisperSnapshot> whispers = new List<PsychicWhisperSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Post-Blast Psychic Echo & Auditory Hallucination HUD view-model.
    /// Monitors electromagnetic trauma whispers (Hz), sanity index (% baseline),
    /// acoustic dampening ear defenders, psychological therapy, and fear containment.
    /// </summary>
    public class PsychicEchoHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedWhisperIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnPsychicEchoChanged;
        public event Action<string> OnSilenceWhisperRequested; // (whisperId)

        private Func<PsychicEchoSnapshot> _getSnapshot;
        private PsychicEchoSnapshot _snapshot;

        public void Bind(Func<PsychicEchoSnapshot> getSnapshot)
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

        public bool SelectNextWhisper()
        {
            if (!IsOpen || _snapshot == null || _snapshot.whispers == null || _snapshot.whispers.Count == 0)
                return false;
            SelectedWhisperIndex = (SelectedWhisperIndex + 1) % _snapshot.whispers.Count;
            ReportOutcome("Selected psychic whisper: " + GetSelectedWhisperName());
            return true;
        }

        public bool SelectPreviousWhisper()
        {
            if (!IsOpen || _snapshot == null || _snapshot.whispers == null || _snapshot.whispers.Count == 0)
                return false;
            SelectedWhisperIndex = (SelectedWhisperIndex - 1 + _snapshot.whispers.Count) % _snapshot.whispers.Count;
            ReportOutcome("Selected psychic whisper: " + GetSelectedWhisperName());
            return true;
        }

        public bool RequestSilenceWhisper()
        {
            if (!IsOpen || _snapshot == null || _snapshot.whispers == null || _snapshot.whispers.Count == 0)
            {
                ReportOutcome("No psychic whisper selected for acoustic dampening.");
                return false;
            }

            var whisper = GetSelectedWhisper();
            if (whisper == null) return false;

            if (OnSilenceWhisperRequested == null)
            {
                ReportOutcome("Acoustic dampener link offline.");
                return false;
            }

            OnSilenceWhisperRequested.Invoke(whisper.whisperId);
            ReportOutcome("Applying acoustic dampening ear defenders to silence whisper " + whisper.whisperId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No psychic echo action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnPsychicEchoChanged?.Invoke();
        }

        private PsychicWhisperSnapshot GetSelectedWhisper()
        {
            if (_snapshot != null && _snapshot.whispers != null && SelectedWhisperIndex >= 0 && SelectedWhisperIndex < _snapshot.whispers.Count)
            {
                return _snapshot.whispers[SelectedWhisperIndex];
            }
            return null;
        }

        private string GetSelectedWhisperName()
        {
            var w = GetSelectedWhisper();
            return w != null ? w.whisperId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("PSYCHIC ECHO & TRAUMA AUDITORY WHISPER MONITOR  [E] close  ·  [Tab] cycle  ·  [S] silence whisper");

            if (_snapshot == null)
            {
                sb.Append("\nPsychic echo telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSANITY TELEMETRY: Shelter Sanity Index: ").Append(_snapshot.sanityIndexPercent.ToString("0")).Append("%")
              .Append("  ·  Active Whispers Detected: ").Append(_snapshot.totalWhispersDetected);

            sb.Append("\n\nINTERCEPTED PSYCHIC WHISPERS:");
            if (_snapshot.whispers == null || _snapshot.whispers.Count == 0)
            {
                sb.Append("\n  No psychic trauma whispers active.");
            }
            else
            {
                for (int i = 0; i < _snapshot.whispers.Count; i++)
                {
                    var whisper = _snapshot.whispers[i];
                    if (whisper == null) continue;

                    bool selected = (i == SelectedWhisperIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Whisper ").Append(whisper.whisperId)
                      .Append(" (").Append(whisper.frequencyHz.ToString("0")).Append(" Hz)")
                      .Append(" — Intensity: ").Append(whisper.intensityPercent.ToString("0")).Append("%")
                      .Append(" | Morale Impact: -").Append(whisper.moraleImpact.ToString("0.#"));

                    if (whisper.isSilenced) sb.Append("  ✔ [SILENCED BY ACOUSTIC FOAM]");
                    else sb.Append("  ★ [ACTIVE WHISPERING — MORALE DRAIN]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nPSYCHIC LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
