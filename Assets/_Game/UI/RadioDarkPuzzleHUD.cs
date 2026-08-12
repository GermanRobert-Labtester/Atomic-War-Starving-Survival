using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class RadioStationSnapshot
    {
        public string stationId;
        public string displayName;
        public float frequencyMHz;
        public string puzzleState; // "Undiscovered", "Listening", "Decrypting", "Solved", "Expired"
        public string requiredRole;
        public float decryptProgress; // 0..1
    }

    public class RadioDarkPuzzleSnapshot
    {
        public float currentTunedFrequencyMHz;
        public int totalPuzzlesSolved;
        public List<RadioStationSnapshot> activeStations = new List<RadioStationSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — The Radio Dark Puzzle HUD view-model.
    /// Manages ghost radio station frequency tuning, cipher decoding puzzles,
    /// specialist survivor assignment (meteorologist, radio host, tech bro), and
    /// encrypted audio transmission telemetry.
    /// </summary>
    public class RadioDarkPuzzleHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedStationIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnRadioDarkChanged;
        public event Action<float> OnFrequencyTuned;                     // frequencyMHz
        public event Action<string, string> OnAttemptDecryptRequested;   // (stationId, survivorId)

        private Func<RadioDarkPuzzleSnapshot> _getSnapshot;
        private RadioDarkPuzzleSnapshot _snapshot;

        public void Bind(Func<RadioDarkPuzzleSnapshot> getSnapshot)
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

        public bool SelectNextStation()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeStations == null || _snapshot.activeStations.Count == 0)
                return false;
            SelectedStationIndex = (SelectedStationIndex + 1) % _snapshot.activeStations.Count;
            ReportOutcome("Selected ghost station: " + GetSelectedStationName());
            return true;
        }

        public bool SelectPreviousStation()
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeStations == null || _snapshot.activeStations.Count == 0)
                return false;
            SelectedStationIndex = (SelectedStationIndex - 1 + _snapshot.activeStations.Count) % _snapshot.activeStations.Count;
            ReportOutcome("Selected ghost station: " + GetSelectedStationName());
            return true;
        }

        public bool RequestTuneFrequency(float deltaMHz)
        {
            if (!IsOpen) return false;
            if (OnFrequencyTuned == null)
            {
                ReportOutcome("Radio tuner offline.");
                return false;
            }

            float newFreq = Mathf.Max(80.0f, (_snapshot?.currentTunedFrequencyMHz ?? 100.0f) + deltaMHz);
            OnFrequencyTuned.Invoke(newFreq);
            ReportOutcome("Tuned receiver to " + newFreq.ToString("0.0") + " MHz");
            return true;
        }

        public bool RequestDecrypt(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.activeStations == null || _snapshot.activeStations.Count == 0)
            {
                ReportOutcome("No ghost station selected for cipher decryption.");
                return false;
            }

            var station = GetSelectedStation();
            if (station == null) return false;

            if (OnAttemptDecryptRequested == null)
            {
                ReportOutcome("Radio decrypt processor link offline.");
                return false;
            }

            OnAttemptDecryptRequested.Invoke(station.stationId, survivorId ?? "unassigned");
            ReportOutcome("Attempting cipher decryption on " + station.displayName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No radio puzzle action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnRadioDarkChanged?.Invoke();
        }

        private RadioStationSnapshot GetSelectedStation()
        {
            if (_snapshot != null && _snapshot.activeStations != null && SelectedStationIndex >= 0 && SelectedStationIndex < _snapshot.activeStations.Count)
            {
                return _snapshot.activeStations[SelectedStationIndex];
            }
            return null;
        }

        private string GetSelectedStationName()
        {
            var s = GetSelectedStation();
            return s != null ? s.displayName ?? s.stationId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("RADIO DARK CIPHER TERMINAL  [R] close  ·  [Tab] cycle  ·  [,/.] tune freq  ·  [D] decrypt");

            if (_snapshot == null)
            {
                sb.Append("\nRadio receiver telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nRECEIVER FREQUENCY: ").Append(_snapshot.currentTunedFrequencyMHz.ToString("0.0")).Append(" MHz")
              .Append("  ·  Puzzles Solved: ").Append(_snapshot.totalPuzzlesSolved);

            sb.Append("\n\nGHOST STATIONS IN RANGE:");
            if (_snapshot.activeStations == null || _snapshot.activeStations.Count == 0)
            {
                sb.Append("\n  No active ghost radio signals detected across airwaves.");
            }
            else
            {
                for (int i = 0; i < _snapshot.activeStations.Count; i++)
                {
                    var station = _snapshot.activeStations[i];
                    if (station == null) continue;

                    bool selected = (i == SelectedStationIndex);
                    bool matched = Mathf.Abs(_snapshot.currentTunedFrequencyMHz - station.frequencyMHz) < 0.2f;

                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(station.displayName ?? station.stationId)
                      .Append(" (").Append(station.frequencyMHz.ToString("0.0")).Append(" MHz)")
                      .Append(" — Status: ").Append(station.puzzleState ?? "Undiscovered");

                    if (station.decryptProgress > 0f)
                        sb.Append(" [").Append((station.decryptProgress * 100f).ToString("0")).Append("% decoded]");

                    if (matched) sb.Append("  ★ [TUNED IN]");
                    if (!string.IsNullOrEmpty(station.requiredRole))
                        sb.Append(" (Req: ").Append(station.requiredRole).Append(")");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nRECEIVER LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
