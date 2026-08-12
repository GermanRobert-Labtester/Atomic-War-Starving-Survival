using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class CipherTransmissionSnapshot
    {
        public string transmissionId;
        public float frequencyMHz;
        public string cipherCodeSequence; // e.g. "7492-0184-9921"
        public float decryptionProgressPercent; // 0..100
        public string unlockedIntelText;
        public bool isFullyDecoded;
    }

    public class NumbersStationDecoderSnapshot
    {
        public int totalCiphersDecoded;
        public List<CipherTransmissionSnapshot> transmissions = new List<CipherTransmissionSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Shortwave Numbers Station Cipher Decoder HUD view-model.
    /// Monitors shortwave radio cipher broadcasts, codebook decryption progress,
    /// hidden military supply drop coordinates, and mysterious ghost broadcasts.
    /// </summary>
    public class NumbersStationDecoderHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedTransmissionIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnNumbersStationDecoderChanged;
        public event Action<string> OnDecodeCipherRequested; // (transmissionId)

        private Func<NumbersStationDecoderSnapshot> _getSnapshot;
        private NumbersStationDecoderSnapshot _snapshot;

        public void Bind(Func<NumbersStationDecoderSnapshot> getSnapshot)
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

        public bool SelectNextTransmission()
        {
            if (!IsOpen || _snapshot == null || _snapshot.transmissions == null || _snapshot.transmissions.Count == 0)
                return false;
            SelectedTransmissionIndex = (SelectedTransmissionIndex + 1) % _snapshot.transmissions.Count;
            ReportOutcome("Selected cipher transmission: " + GetSelectedTransmissionName());
            return true;
        }

        public bool SelectPreviousTransmission()
        {
            if (!IsOpen || _snapshot == null || _snapshot.transmissions == null || _snapshot.transmissions.Count == 0)
                return false;
            SelectedTransmissionIndex = (SelectedTransmissionIndex - 1 + _snapshot.transmissions.Count) % _snapshot.transmissions.Count;
            ReportOutcome("Selected cipher transmission: " + GetSelectedTransmissionName());
            return true;
        }

        public bool RequestDecodeCipher()
        {
            if (!IsOpen || _snapshot == null || _snapshot.transmissions == null || _snapshot.transmissions.Count == 0)
            {
                ReportOutcome("No transmission selected for codebook decoding.");
                return false;
            }

            var trans = GetSelectedTransmission();
            if (trans == null) return false;

            if (trans.isFullyDecoded)
            {
                ReportOutcome("Transmission " + trans.transmissionId + " is already fully decoded.");
                return false;
            }

            if (OnDecodeCipherRequested == null)
            {
                ReportOutcome("Codebook deciphering desk link offline.");
                return false;
            }

            OnDecodeCipherRequested.Invoke(trans.transmissionId);
            ReportOutcome("Deciphering code sequence [" + trans.cipherCodeSequence + "] with Military Codebook...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No numbers station action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnNumbersStationDecoderChanged?.Invoke();
        }

        private CipherTransmissionSnapshot GetSelectedTransmission()
        {
            if (_snapshot != null && _snapshot.transmissions != null && SelectedTransmissionIndex >= 0 && SelectedTransmissionIndex < _snapshot.transmissions.Count)
            {
                return _snapshot.transmissions[SelectedTransmissionIndex];
            }
            return null;
        }

        private string GetSelectedTransmissionName()
        {
            var t = GetSelectedTransmission();
            return t != null ? (t.transmissionId + " (" + t.frequencyMHz.ToString("0.0") + " MHz)") : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SHORTWAVE NUMBERS STATION CIPHER DECODER  [N] close  ·  [Tab] cycle  ·  [D] decode cipher");

            if (_snapshot == null)
            {
                sb.Append("\nNumbers station radio telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nDECODER STATS: Ciphers Fully Decoded: ").Append(_snapshot.totalCiphersDecoded);

            sb.Append("\n\nINTERCEPTED SHORTWAVE CIPHER BROADCASTS:");
            if (_snapshot.transmissions == null || _snapshot.transmissions.Count == 0)
            {
                sb.Append("\n  No shortwave cipher broadcasts intercepted.");
            }
            else
            {
                for (int i = 0; i < _snapshot.transmissions.Count; i++)
                {
                    var trans = _snapshot.transmissions[i];
                    if (trans == null) continue;

                    bool selected = (i == SelectedTransmissionIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Station ").Append(trans.transmissionId)
                      .Append(" (").Append(trans.frequencyMHz.ToString("0.0")).Append(" MHz)")
                      .Append(" — Code: ").Append(trans.cipherCodeSequence ?? "???")
                      .Append(" | Progress: ").Append(trans.decryptionProgressPercent.ToString("0")).Append("%");

                    if (trans.isFullyDecoded)
                    {
                        sb.Append("  ✔ [DECODED]");
                        if (!string.IsNullOrEmpty(trans.unlockedIntelText))
                            sb.Append("\n    [INTEL UNLOCKED: ").Append(trans.unlockedIntelText).Append("]");
                    }
                    else
                    {
                        sb.Append("  ★ [ENCRYPTED]");
                    }
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nCIPHER LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
