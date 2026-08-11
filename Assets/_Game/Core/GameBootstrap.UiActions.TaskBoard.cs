using AtomicWar._Game.UI;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Open or close the survivor allocation board ([V]).</summary>
        public void ToggleSurvivorTaskBoard()
        {
            var board = _hud != null ? _hud.SurvivorTaskBoardHUD : null;
            if (board == null) return;
            if (!board.IsOpen)
            {
                _hud.ScavengeDispatchHUD?.Close();
                _hud.OverflowCrateHUD?.Close();
                _hud.FieldGearLoadoutHUD?.Close();
                _hud.BunkerRationingHUD?.Close();
                _hud.WaterPurificationHUD?.Close();
                _hud.AirHeatManagementHUD?.Close();
                _hud.BunkerMaintenanceHUD?.Close();
            }
            board.Toggle();
        }

        public bool IncreaseTaskBoardPriority() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.IncreasePriority();

        public bool DecreaseTaskBoardPriority() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.DecreasePriority();

        public bool CancelTaskBoardActiveTask() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.CancelActiveTask();

        public bool SelectNextTaskBoardShift() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.SelectNextShift();

        public bool SelectPreviousTaskBoardShift() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.SelectPreviousShift();

        public bool SelectNextTaskBoardSurvivor() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.SelectNextSurvivor();

        public bool SelectPreviousTaskBoardSurvivor() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.SelectPreviousSurvivor();

        public bool AssignSelectedTaskBoardShift() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.AssignSelectedShift();

        public bool ApproveTopTaskBoardShiftRecommendation() =>
            _hud?.SurvivorTaskBoardHUD != null && _hud.SurvivorTaskBoardHUD.ApproveTopShiftRecommendation();

        private void HandleTaskBoardPriorityAdjustmentRequested(int direction)
        {
            var board = _hud != null ? _hud.SurvivorTaskBoardHUD : null;
            if (SurvivorTaskBoardSystem == null)
            {
                board?.ReportOutcome("Task board system is offline.");
                return;
            }

            SurvivorTaskBoardSystem.TryAdjustActivePriority(direction, out var result);
            board?.ReportOutcome(result != null ? result.Reason : "Task board report unavailable.");
        }

        private void HandleTaskBoardCancellationRequested()
        {
            var board = _hud != null ? _hud.SurvivorTaskBoardHUD : null;
            if (SurvivorTaskBoardSystem == null)
            {
                board?.ReportOutcome("Task board system is offline.");
                return;
            }

            SurvivorTaskBoardSystem.CancelActiveTask(out var result);
            board?.ReportOutcome(result != null ? result.Reason : "Task board report unavailable.");
        }

        private void HandleTaskBoardShiftAssignmentRequested(WorkShiftDuty duty, string survivorId)
        {
            var board = _hud != null ? _hud.SurvivorTaskBoardHUD : null;
            if (SurvivorTaskBoardSystem == null)
            {
                board?.ReportOutcome("Task board system is offline.");
                return;
            }

            SurvivorTaskBoardSystem.TryAssignShift(duty, survivorId, out var result);
            board?.ReportOutcome(result != null ? result.Reason : "Duty-shift report unavailable.");
        }

        private void HandleTaskBoardShiftCancellationRequested(WorkShiftDuty duty)
        {
            var board = _hud != null ? _hud.SurvivorTaskBoardHUD : null;
            if (SurvivorTaskBoardSystem == null)
            {
                board?.ReportOutcome("Task board system is offline.");
                return;
            }

            SurvivorTaskBoardSystem.CancelShift(duty, out var result);
            board?.ReportOutcome(result != null ? result.Reason : "Duty-shift report unavailable.");
        }

        private void HandleTaskBoardRecommendationApprovalRequested()
        {
            var board = _hud != null ? _hud.SurvivorTaskBoardHUD : null;
            if (SurvivorTaskBoardSystem == null)
            {
                board?.ReportOutcome("Task board system is offline.");
                return;
            }

            SurvivorTaskBoardSystem.TryApproveTopShiftRecommendation(out var result);
            board?.ReportOutcome(result != null ? result.Reason : "Duty recommendation report unavailable.");
        }
    }
}
