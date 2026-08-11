using System;
using System.Text;
using AtomicWar._Game.Shelter;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Bunker climate terminal view-model. It never changes module or grid state
    /// itself: Core receives the selected-load intents and applies them through
    /// AirHeatManagementSystem.
    /// </summary>
    public class AirHeatManagementHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public AirHeatLoad SelectedLoad { get; private set; } = AirHeatLoad.AirFiltration;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnAirHeatManagementChanged;
        public event Action<AirHeatLoad, int> OnPriorityAdjustmentRequested;
        public event Action<AirHeatLoad> OnRequestToggleRequested;

        private Func<AirHeatManagementSnapshot> _getSnapshot;
        private AirHeatManagementSnapshot _snapshot;

        public void Bind(Func<AirHeatManagementSnapshot> getSnapshot)
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

        public bool ToggleSelectedLoad()
        {
            if (!IsOpen) return false;
            SelectedLoad = SelectedLoad == AirHeatLoad.AirFiltration
                ? AirHeatLoad.Heater
                : AirHeatLoad.AirFiltration;
            LastOutcome = "Selected " + LoadLabel(SelectedLoad) + ".";
            Refresh();
            return true;
        }

        public bool IncreaseSelectedPriority() => RequestPriorityAdjustment(1);
        public bool DecreaseSelectedPriority() => RequestPriorityAdjustment(-1);

        public bool ToggleSelectedRequest()
        {
            if (!IsOpen) return false;
            if (OnRequestToggleRequested == null)
            {
                ReportOutcome("Climate control link offline.");
                return false;
            }
            OnRequestToggleRequested.Invoke(SelectedLoad);
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No climate change recorded." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnAirHeatManagementChanged?.Invoke();
        }

        private bool RequestPriorityAdjustment(int direction)
        {
            if (!IsOpen || direction == 0) return false;
            if (OnPriorityAdjustmentRequested == null)
            {
                ReportOutcome("Climate control link offline.");
                return false;
            }
            OnPriorityAdjustmentRequested.Invoke(SelectedLoad, direction);
            return true;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("AIR + HEAT CONTROL  [K] close  ·  [Tab] select  ·  [,/.] priority  ·  [Enter] request");
            if (_snapshot == null)
            {
                sb.Append("\nClimate telemetry is unavailable.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nINDOORS: ").Append(_snapshot.IndoorTemperatureCelsius.ToString("0.#"))
                .Append("°C  ·  outside ").Append(_snapshot.AmbientTemperatureCelsius.ToString("0.#"))
                .Append("°C  ·  air quality ").Append(_snapshot.AirQuality.ToString("0")).Append("%");
            sb.Append("\nGRID: ").Append(_snapshot.GridDrawWatts.ToString("0"))
                .Append("/").Append(_snapshot.GridGenerationWatts.ToString("0")).Append(" W");
            if (_snapshot.IsBlackout) sb.Append("  [BLACKOUT]");
            else if (_snapshot.IsLoadShedding) sb.Append("  [LOAD SHED]");
            else sb.Append("  [STABLE]");

            AppendLoad(sb, AirHeatLoad.AirFiltration, "FILTER", _snapshot.FilterInstalled,
                _snapshot.FilterOperational, _snapshot.FilterLoad, _snapshot.FilterHealth,
                _snapshot.FilterRuntimeHours, _snapshot.FilterDegradationPerHour, "% health", "wear");
            AppendLoad(sb, AirHeatLoad.Heater, "HEATER", _snapshot.HeaterInstalled,
                _snapshot.HeaterOperational, _snapshot.HeaterLoad, _snapshot.HeaterFuel,
                _snapshot.HeaterRuntimeHours, _snapshot.HeaterFuelBurnPerHour, "fuel", "burn");

            sb.Append("\nPriority 1 keeps a load longest; priority 5 is shed first.");
            if (!string.IsNullOrEmpty(LastOutcome)) sb.Append("\nREPORT: ").Append(LastOutcome);
            PanelSummary = sb.ToString();
        }

        private void AppendLoad(
            StringBuilder sb,
            AirHeatLoad load,
            string label,
            bool installed,
            bool operational,
            ClimateLoadSnapshot grid,
            float reserve,
            float runtimeHours,
            float ratePerHour,
            string reserveLabel,
            string rateLabel)
        {
            bool selected = SelectedLoad == load;
            sb.Append("\n").Append(selected ? "> " : "  ").Append(label).Append(": ");
            if (!installed)
            {
                sb.Append("NOT INSTALLED");
                return;
            }

            sb.Append(operational ? "ACTIVE" : "OFFLINE")
                .Append("  ·  ").Append(reserve.ToString("0.#")).Append(" ").Append(reserveLabel)
                .Append("  ·  ").Append(runtimeHours.ToString("0.#")).Append("h at ")
                .Append(ratePerHour.ToString("0.#")).Append("/").Append(rateLabel);
            if (grid == null || !grid.IsRegistered)
            {
                sb.Append("  ·  grid link missing");
                return;
            }

            sb.Append("\n     GRID: P").Append(grid.Priority).Append("  ·  ")
                .Append(grid.Watts.ToString("0")).Append(" W  ·  ")
                .Append(LoadStateLabel(grid));
        }

        private static string LoadLabel(AirHeatLoad load)
        {
            return load == AirHeatLoad.Heater ? "HEATER" : "AIR FILTER";
        }

        private static string LoadStateLabel(ClimateLoadSnapshot load)
        {
            if (!load.IsRequested) return "NOT REQUESTED";
            if (load.IsShed) return "SHED";
            if (load.IsPowered) return "POWERED";
            return "OFFLINE";
        }
    }
}
