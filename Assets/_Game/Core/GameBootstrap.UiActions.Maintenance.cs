using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Open or close the bunker repair-order terminal ([N]).</summary>
        public void ToggleBunkerMaintenance()
        {
            var terminal = _hud != null ? _hud.BunkerMaintenanceHUD : null;
            if (terminal == null) return;
            if (!terminal.IsOpen)
            {
                _hud.ScavengeDispatchHUD?.Close();
                _hud.OverflowCrateHUD?.Close();
                _hud.FieldGearLoadoutHUD?.Close();
                _hud.BunkerRationingHUD?.Close();
                _hud.WaterPurificationHUD?.Close();
                _hud.AirHeatManagementHUD?.Close();
                _hud.SurvivorTaskBoardHUD?.Close();
            }
            terminal.Toggle();
        }

        public bool SelectNextBunkerMaintenanceTarget() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.SelectNextTarget();

        public bool SelectPreviousBunkerMaintenanceTarget() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.SelectPreviousTarget();

        public bool SelectNextBunkerMaintenanceSurvivor() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.SelectNextSurvivor();

        public bool SelectPreviousBunkerMaintenanceSurvivor() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.SelectPreviousSurvivor();

        public bool IncreaseBunkerMaintenancePriority() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.IncreasePriority();

        public bool DecreaseBunkerMaintenancePriority() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.DecreasePriority();

        public bool RepairSelectedBunkerMaintenanceTarget() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.RepairSelected();

        public bool CancelBunkerMaintenanceRepair() =>
            _hud?.BunkerMaintenanceHUD != null && _hud.BunkerMaintenanceHUD.CancelRepairOrder();

        private void HandleMaintenanceSurvivorAssignmentRequested(string survivorId)
        {
            var terminal = _hud != null ? _hud.BunkerMaintenanceHUD : null;
            if (BunkerMaintenanceSystem == null)
            {
                terminal?.ReportOutcome("Maintenance system is offline.");
                return;
            }
            if (!BunkerMaintenanceSystem.AssignSurvivor(survivorId))
            {
                terminal?.ReportOutcome("HELD: that survivor cannot take the repair order.");
                return;
            }

            var snapshot = BunkerMaintenanceSystem.GetSnapshot();
            terminal?.ReportOutcome("ORDERED: " + snapshot.AssignedSurvivorName + " now holds repair priority.");
        }

        private void HandleMaintenancePriorityAdjustmentRequested(int direction)
        {
            var terminal = _hud != null ? _hud.BunkerMaintenanceHUD : null;
            if (BunkerMaintenanceSystem == null)
            {
                terminal?.ReportOutcome("Maintenance system is offline.");
                return;
            }
            if (!BunkerMaintenanceSystem.AdjustPriority(direction))
            {
                terminal?.ReportOutcome("Repair priority is already at that limit.");
                return;
            }

            terminal?.ReportOutcome("REPAIR PRIORITY: " + PriorityLabel(BunkerMaintenanceSystem.RepairPriority) + ".");
        }

        private void HandleMaintenanceRepairRequested(MaintenanceTargetType targetType, string targetId)
        {
            var terminal = _hud != null ? _hud.BunkerMaintenanceHUD : null;
            if (RepairWorkOrderSystem == null)
            {
                terminal?.ReportOutcome("Maintenance system is offline.");
                return;
            }

            RepairWorkOrderSystem.TryQueue(targetType, targetId, out var result);
            terminal?.ReportOutcome(result != null ? result.Reason : "HELD: repair report unavailable.");
        }

        private void HandleMaintenanceRepairCancellationRequested()
        {
            var terminal = _hud != null ? _hud.BunkerMaintenanceHUD : null;
            if (RepairWorkOrderSystem == null)
            {
                terminal?.ReportOutcome("Maintenance system is offline.");
                return;
            }

            RepairWorkOrderSystem.CancelActiveOrder(out var result);
            terminal?.ReportOutcome(result != null ? result.Reason : "HELD: repair report unavailable.");
        }

        private static string PriorityLabel(MaintenanceRepairPriority priority)
        {
            switch (priority)
            {
                case MaintenanceRepairPriority.Critical: return "CRITICAL";
                case MaintenanceRepairPriority.Low: return "LOW";
                default: return "STANDARD";
            }
        }
    }
}
