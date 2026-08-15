using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class GhostTapeTransmissionSnapshot
    {
        public string tapeId;
        public string tapeTitle;
        public float broadcastFrequencyMHz;
        public float moraleDrainPerMin;
        public bool isPlaying;
        public bool isDecoded;
    }

    public class GhostTapeSnapshot
    {
        public int totalTapesDiscovered;
        public int totalTerrorBroadcastsPlayed;
        public List<GhostTapeTransmissionSnapshot> tapes = new List<GhostTapeTransmissionSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Ghost Tape Terror Transmission HUD view-model.
    /// Monitors psychological warfare cassette tapes, audio decryption, terror broadcast playback,
    /// survivor morale drain, and psychological propaganda warfare.
    /// </summary>
    public class GhostTapeHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedTapeIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnGhostTapeChanged;
        public event Action<string> OnPlayGhostTapeRequested;  // (tapeId)
        public event Action<string> OnStopGhostTapeRequested;  // (tapeId)

        private Func<GhostTapeSnapshot> _getSnapshot;
        private GhostTapeSnapshot _snapshot;

        public void Bind(Func<GhostTapeSnapshot> getSnapshot)
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

        public bool SelectNextTape()
        {
            if (!IsOpen || _snapshot == null || _snapshot.tapes == null || _snapshot.tapes.Count == 0)
                return false;
            SelectedTapeIndex = (SelectedTapeIndex + 1) % _snapshot.tapes.Count;
            ReportOutcome("Selected ghost tape: " + GetSelectedTapeName());
            return true;
        }

        public bool SelectPreviousTape()
        {
            if (!IsOpen || _snapshot == null || _snapshot.tapes == null || _snapshot.tapes.Count == 0)
                return false;
            SelectedTapeIndex = (SelectedTapeIndex - 1 + _snapshot.tapes.Count) % _snapshot.tapes.Count;
            ReportOutcome("Selected ghost tape: " + GetSelectedTapeName());
            return true;
        }

        public bool RequestTogglePlayback()
        {
            if (!IsOpen || _snapshot == null || _snapshot.tapes == null || _snapshot.tapes.Count == 0)
            {
                ReportOutcome("No ghost tape selected for playback.");
                return false;
            }

            var tape = GetSelectedTape();
            if (tape == null) return false;

            if (tape.isPlaying)
            {
                if (OnStopGhostTapeRequested != null)
                {
                    OnStopGhostTapeRequested.Invoke(tape.tapeId);
                    ReportOutcome("Halt broadcast of Ghost Tape [" + tape.tapeTitle + "].");
                    return true;
                }
            }
            else
            {
                if (OnPlayGhostTapeRequested != null)
                {
                    OnPlayGhostTapeRequested.Invoke(tape.tapeId);
                    ReportOutcome("PLAYING terror broadcast Ghost Tape [" + tape.tapeTitle + "] over radio array!");
                    return true;
                }
            }

            ReportOutcome("Tape deck control link offline.");
            return false;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No ghost tape action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnGhostTapeChanged?.Invoke();
        }

        private GhostTapeTransmissionSnapshot GetSelectedTape()
        {
            if (_snapshot != null && _snapshot.tapes != null && SelectedTapeIndex >= 0 && SelectedTapeIndex < _snapshot.tapes.Count)
            {
                return _snapshot.tapes[SelectedTapeIndex];
            }
            return null;
        }

        private string GetSelectedTapeName()
        {
            var t = GetSelectedTape();
            return t != null ? (t.tapeTitle ?? t.tapeId) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("GHOST TAPE TERROR BROADCAST DECK  [T] close  ·  [Tab] cycle  ·  [P] toggle playback");

            if (_snapshot == null)
            {
                sb.Append("\nTape deck telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nTAPE ARCHIVE: Tapes Discovered: ").Append(_snapshot.totalTapesDiscovered)
              .Append("  ·  Broadcasts Played: ").Append(_snapshot.totalTerrorBroadcastsPlayed);

            sb.Append("\n\nDISCOVERED GHOST TAPES & TERROR CASSETTES:");
            if (_snapshot.tapes == null || _snapshot.tapes.Count == 0)
            {
                sb.Append("\n  No ghost tapes in cassette archive.");
            }
            else
            {
                for (int i = 0; i < _snapshot.tapes.Count; i++)
                {
                    var tape = _snapshot.tapes[i];
                    if (tape == null) continue;

                    bool selected = (i == SelectedTapeIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(tape.tapeTitle ?? tape.tapeId)
                      .Append(" (").Append(tape.broadcastFrequencyMHz.ToString("0.0")).Append(" MHz)")
                      .Append(" — Morale Drain: -").Append(tape.moraleDrainPerMin.ToString("0.#")).Append(" / min");

                    if (tape.isPlaying) sb.Append("  ★ [BROADCASTING NOW OVER WASTELAND]");
                    else if (tape.isDecoded) sb.Append("  ✔ [DECODED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nTAPE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
