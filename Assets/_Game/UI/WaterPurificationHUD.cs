using System;
using System.Text;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Readout and input surface for the bunker cistern and purifier. Core keeps
    /// the water simulation and queue state; this widget only presents snapshots
    /// and emits a queue-cycle intent.
    /// </summary>
    public class WaterPurificationHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnWaterPurificationChanged;
        public event Action<int> OnQueueCycleRequested;

        private Func<WaterPurificationSnapshot> _getWaterSnapshot;
        private Func<BunkerRationingSnapshot> _getRationSnapshot;
        private WaterPurificationSnapshot _waterSnapshot;
        private BunkerRationingSnapshot _rationSnapshot;

        public void Bind(
            Func<WaterPurificationSnapshot> getWaterSnapshot,
            Func<BunkerRationingSnapshot> getRationSnapshot)
        {
            _getWaterSnapshot = getWaterSnapshot;
            _getRationSnapshot = getRationSnapshot;
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

        public bool QueuePrevious() => RequestQueueCycle(-1);
        public bool QueueNext() => RequestQueueCycle(1);

        public void ReportQueueResult(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "Queue unchanged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _waterSnapshot = _getWaterSnapshot != null ? _getWaterSnapshot() : null;
            _rationSnapshot = _getRationSnapshot != null ? _getRationSnapshot() : null;
            RebuildPanel();
            OnWaterPurificationChanged?.Invoke();
        }

        private bool RequestQueueCycle(int direction)
        {
            if (!IsOpen || direction == 0) return false;
            if (OnQueueCycleRequested == null)
            {
                ReportQueueResult("Purifier control link offline.");
                return false;
            }

            OnQueueCycleRequested.Invoke(direction);
            return true;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("BUNKER WATER TERMINAL  [Y] close  ·  [,/.] queue");
            if (_waterSnapshot == null)
            {
                sb.Append("\nCistern telemetry is unavailable.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nCISTERN: clean ").Append(_waterSnapshot.CleanWater.ToString("0.#"))
                .Append("  ·  dirty ").Append(_waterSnapshot.DirtyWater.ToString("0.#"))
                .Append("  ·  irradiated ").Append(_waterSnapshot.IrradiatedWater.ToString("0.#"));
            sb.Append("\nPURIFIER: ").Append(_waterSnapshot.PurifierOperational ? "RUNNING" : "OFFLINE")
                .Append("  ·  filter ").Append(_waterSnapshot.FilterHealth.ToString("0.#")).Append("%");
            sb.Append("\nQUEUE: ").Append(QueueLabel(_waterSnapshot.QueueMode))
                .Append("  ·  next ").Append(_waterSnapshot.NextSourceLabel)
                .Append(" → ").Append(_waterSnapshot.NextOutputLabel)
                .Append("  ·  ").Append(_waterSnapshot.UnitsQueued).Append(" whole units waiting");
            if (_waterSnapshot.PurifierOperational)
                sb.Append("\nNext conversion: ").Append(_waterSnapshot.HoursUntilNextUnit.ToString("0.#"))
                    .Append("h  ·  cycle ").Append(_waterSnapshot.ConversionProgressHours.ToString("0.#"))
                    .Append("/").Append(_waterSnapshot.HoursPerUnit.ToString("0.#")).Append("h");
            else
                sb.Append("\nPurification halted: restore power, installation, or filter health.");

            AppendRationProjection(sb, _rationSnapshot);
            if (!string.IsNullOrEmpty(LastOutcome)) sb.Append("\nREPORT: ").Append(LastOutcome);
            PanelSummary = sb.ToString();
        }

        private static void AppendRationProjection(StringBuilder sb, BunkerRationingSnapshot ration)
        {
            if (ration == null) return;
            sb.Append("\n--- RATION LINK ---");
            sb.Append("\nClean cistern available to rations: ").Append(ration.CleanCisternWaterOnHand)
                .Append("  ·  water pool ").Append(ration.WaterOnHand)
                .Append("/").Append(ration.WaterRequired).Append(" required")
                .Append("  ·  ").Append((ration.ProjectedWaterCoverage * 100f).ToString("0")).Append("% covered");
            sb.Append("\nProjected thirst relief: -").Append(ration.ProjectedThirstReduction.ToString("0.#"))
                .Append(" per survivor.");
        }

        private static string QueueLabel(PurifierQueueMode queueMode)
        {
            switch (queueMode)
            {
                case PurifierQueueMode.IrradiatedFirst: return "IRRADIATED FIRST";
                case PurifierQueueMode.DirtyFirst: return "DIRTY FIRST";
                default: return "AUTO (IRRADIATED FIRST)";
            }
        }
    }
}
