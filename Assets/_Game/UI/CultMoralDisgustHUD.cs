using System;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class CultMoralDisgustSnapshot
    {
        public int totalCultTrades;
        public float totalIrradiatedWaterSold;
        public bool massAscensionTriggered;
        public float lastTradeDisgustPenaltyApplied;
    }

    /// <summary>
    /// Protocol Zero — Doomsday Cult Moral Disgust HUD view-model.
    /// Monitors trading with "The Cult of the Glow", tracking moral disgust penalties
    /// suffered by rational survivors when selling irradiated water / contaminated food
    /// to enable mass-suicide ascension rites.
    /// </summary>
    public class CultMoralDisgustHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnCultMoralDisgustChanged;
        public event Action<string, float> OnRecordCultTradeRequested; // (itemId, amount)

        private Func<CultMoralDisgustSnapshot> _getSnapshot;
        private CultMoralDisgustSnapshot _snapshot;

        public void Bind(Func<CultMoralDisgustSnapshot> getSnapshot)
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

        public bool RequestRecordTrade(string itemId, float amount)
        {
            if (!IsOpen) return false;
            if (OnRecordCultTradeRequested == null)
            {
                ReportOutcome("Cult trade ledger link offline.");
                return false;
            }

            OnRecordCultTradeRequested.Invoke(itemId ?? "irradiated_water", amount);
            ReportOutcome("Recording trade with Cult of the Glow: " + amount.ToString("0.#") + " of " + (itemId ?? "irradiated_water") + " (Rational Morale Hit Applied)");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No cult trade action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnCultMoralDisgustChanged?.Invoke();
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CULT OF THE GLOW — MORAL DISGUST LEDGER  [U] close  ·  [T] record trade");

            if (_snapshot == null)
            {
                sb.Append("\nCult trade ledger offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nCULT TRADE STATS: Total Cult Trades: ").Append(_snapshot.totalCultTrades)
              .Append("  ·  Irradiated Water Sold: ").Append(_snapshot.totalIrradiatedWaterSold.ToString("0.#")).Append(" L");

            sb.Append("\n\nMORAL STANDING:");
            if (_snapshot.massAscensionTriggered)
            {
                sb.Append("\n  [MASS ASCENSION TRIGGERED: CULT HAS CONSUMED 50+ L OF IRRADIATED WATER — MASS SUICIDE EVENT!]");
            }
            else if (_snapshot.totalIrradiatedWaterSold > 25f)
            {
                sb.Append("\n  [WARNING: HIGH CULT DEPENDENCY — MASS ASCENSION THRESHOLD NEAR (").Append(_snapshot.totalIrradiatedWaterSold.ToString("0.#")).Append(" / 50 L)]");
            }
            else
            {
                sb.Append("\n  Rational survivors view trading with the Cult as enabling mass suicide (-8 to -12 Morale per trade).");
            }

            if (_snapshot.lastTradeDisgustPenaltyApplied > 0f)
            {
                sb.Append("\n  Last Morale Penalty Applied: -").Append(_snapshot.lastTradeDisgustPenaltyApplied.ToString("0.#")).Append(" Morale");
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nLEDGER LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
