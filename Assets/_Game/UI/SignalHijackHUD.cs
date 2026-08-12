using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public enum RadioHijackMode
    {
        DecoyBroadcast,
        GhostTape,
        NumbersInjection
    }

    public class SignalHijackSnapshot
    {
        public bool isRadioArrayDestroyed;
        public int totalDecoysSent;
        public int totalRaidsDiverted;
        public int estimatedCasualties;
    }

    /// <summary>
    /// Protocol Zero — Signal Hijack HUD view-model.
    /// Controls electronic warfare & radio array signal manipulation:
    /// luring enemy raiding parties with decoy broadcasts, playing ghost tapes of
    /// fallen survivors to inflict psychological falter, or injecting false cipher
    /// codes into the Numbers Station.
    /// </summary>
    public class SignalHijackHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public RadioHijackMode SelectedMode { get; private set; } = RadioHijackMode.DecoyBroadcast;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnSignalHijackChanged;
        public event Action<RadioHijackMode, string> OnBroadcastRequested;

        private Func<SignalHijackSnapshot> _getSnapshot;
        private SignalHijackSnapshot _snapshot;

        public void Bind(Func<SignalHijackSnapshot> getSnapshot)
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

        public bool CycleSelectedMode()
        {
            if (!IsOpen) return false;
            SelectedMode = SelectedMode switch
            {
                RadioHijackMode.DecoyBroadcast => RadioHijackMode.GhostTape,
                RadioHijackMode.GhostTape => RadioHijackMode.NumbersInjection,
                _ => RadioHijackMode.DecoyBroadcast
            };
            ReportOutcome("Selected transmission mode: " + SelectedMode.ToString());
            return true;
        }

        public bool RequestBroadcast(string broadcasterId)
        {
            if (!IsOpen) return false;
            if (_snapshot != null && _snapshot.isRadioArrayDestroyed)
            {
                ReportOutcome("CANNOT BROADCAST: Radio Array module is destroyed.");
                return false;
            }

            if (OnBroadcastRequested == null)
            {
                ReportOutcome("Radio transmission array connection offline.");
                return false;
            }

            OnBroadcastRequested.Invoke(SelectedMode, broadcasterId ?? "unassigned");
            ReportOutcome("Broadcasting signal: " + SelectedMode.ToString() + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No transmission logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnSignalHijackChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SIGNAL HIJACK & ELECTRONIC WARFARE  [H] close  ·  [Tab] mode  ·  [Enter] transmit");

            if (_snapshot == null)
            {
                sb.Append("\nRadio Array telemetry link unavailable.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nARRAY STATUS: ");
            if (_snapshot.isRadioArrayDestroyed)
            {
                sb.Append("[DESTROYED — REPAIR REQUIRED]");
            }
            else
            {
                sb.Append("[ONLINE & POWERED]")
                  .Append("  ·  Decoys Sent: ").Append(_snapshot.totalDecoysSent)
                  .Append("  ·  Raids Diverted: ").Append(_snapshot.totalRaidsDiverted)
                  .Append("  ·  Enemy Casualties: ").Append(_snapshot.estimatedCasualties);
            }

            sb.Append("\n\nTRANSMISSION MODES:");

            AppendMode(sb, RadioHijackMode.DecoyBroadcast,
                "DECOY BROADCAST", "Lures approaching raider warbands to fake coordinates. Cost: 15L fuel. Risk: 25% artillery triangulation.");

            AppendMode(sb, RadioHijackMode.GhostTape,
                "GHOST TAPE", "Transmits voice recordings of fallen survivors to terrorize raiders. Cost: 5L fuel. Risk: Grief cascade for family.");

            AppendMode(sb, RadioHijackMode.NumbersInjection,
                "NUMBERS STATION INJECTION", "Injects false cipher codes into hostile radio relays. Cost: 10L fuel. Disables enemy artillery strikes.");

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nTELEMETRY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }

        private void AppendMode(StringBuilder sb, RadioHijackMode mode, string title, string description)
        {
            bool selected = (SelectedMode == mode);
            sb.Append("\n").Append(selected ? "> " : "  ")
              .Append("[").Append(title).Append("]")
              .Append("\n    ").Append(description);
        }
    }
}
