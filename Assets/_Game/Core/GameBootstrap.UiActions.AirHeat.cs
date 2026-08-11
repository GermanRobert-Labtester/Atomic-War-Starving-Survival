using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Open or close the bunker air and heat control terminal ([K]).</summary>
        public void ToggleAirHeatManagement()
        {
            var terminal = _hud != null ? _hud.AirHeatManagementHUD : null;
            if (terminal == null) return;
            if (!terminal.IsOpen)
            {
                _hud.ScavengeDispatchHUD?.Close();
                _hud.OverflowCrateHUD?.Close();
                _hud.FieldGearLoadoutHUD?.Close();
                _hud.BunkerRationingHUD?.Close();
                _hud.WaterPurificationHUD?.Close();
                _hud.BunkerMaintenanceHUD?.Close();
                _hud.SurvivorTaskBoardHUD?.Close();
            }
            terminal.Toggle();
        }

        public bool ToggleSelectedAirHeatLoad()
        {
            return _hud?.AirHeatManagementHUD != null && _hud.AirHeatManagementHUD.ToggleSelectedLoad();
        }

        public bool IncreaseSelectedAirHeatPriority()
        {
            return _hud?.AirHeatManagementHUD != null && _hud.AirHeatManagementHUD.IncreaseSelectedPriority();
        }

        public bool DecreaseSelectedAirHeatPriority()
        {
            return _hud?.AirHeatManagementHUD != null && _hud.AirHeatManagementHUD.DecreaseSelectedPriority();
        }

        public bool ToggleSelectedAirHeatRequest()
        {
            return _hud?.AirHeatManagementHUD != null && _hud.AirHeatManagementHUD.ToggleSelectedRequest();
        }

        private void HandleAirHeatPriorityAdjustmentRequested(AirHeatLoad load, int direction)
        {
            var terminal = _hud != null ? _hud.AirHeatManagementHUD : null;
            if (AirHeatManagementSystem == null)
            {
                terminal?.ReportOutcome("Climate system is offline.");
                return;
            }

            bool changed = AirHeatManagementSystem.AdjustPriority(load, direction);
            if (!changed)
            {
                terminal?.ReportOutcome(LoadLabel(load) + " is already at that priority limit.");
                return;
            }

            var snapshot = AirHeatManagementSystem.GetSnapshot();
            var current = load == AirHeatLoad.Heater ? snapshot.HeaterLoad : snapshot.FilterLoad;
            terminal?.ReportOutcome(LoadLabel(load) + " priority set to P" + (current != null ? current.Priority : 0) + ".");
        }

        private void HandleAirHeatRequestToggleRequested(AirHeatLoad load)
        {
            var terminal = _hud != null ? _hud.AirHeatManagementHUD : null;
            if (AirHeatManagementSystem == null)
            {
                terminal?.ReportOutcome("Climate system is offline.");
                return;
            }

            if (!AirHeatManagementSystem.ToggleRequested(load))
            {
                terminal?.ReportOutcome("No grid load is installed for " + LoadLabel(load) + ".");
                return;
            }

            var snapshot = AirHeatManagementSystem.GetSnapshot();
            var current = load == AirHeatLoad.Heater ? snapshot.HeaterLoad : snapshot.FilterLoad;
            terminal?.ReportOutcome(LoadLabel(load) + (current != null && current.IsRequested
                ? " requested from the grid."
                : " removed from the grid request."));
        }

        private static string LoadLabel(AirHeatLoad load)
        {
            return load == AirHeatLoad.Heater ? "HEATER" : "AIR FILTER";
        }
    }
}
