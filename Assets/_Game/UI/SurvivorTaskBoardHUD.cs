using System;
using System.Text;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>Presentation-only command panel for the survivor task-board snapshot.</summary>
    public sealed class SurvivorTaskBoardHUD : MonoBehaviour
    {
        private Func<SurvivorTaskBoardSnapshot> _getSnapshot;
        private SurvivorTaskBoardSnapshot _snapshot;

        public bool IsOpen { get; private set; }
        public string PanelSummary { get; private set; }
        public string LastOutcome { get; private set; }
        public int SelectedShiftIndex { get; private set; }
        public int SelectedSurvivorIndex { get; private set; }

        public event Action<int> OnPriorityAdjustmentRequested;
        public event Action OnCancellationRequested;
        public event Action<WorkShiftDuty, string> OnShiftAssignmentRequested;
        public event Action<WorkShiftDuty> OnShiftCancellationRequested;
        public event Action OnShiftRecommendationApprovalRequested;
        public event Action OnSurvivorTaskBoardChanged;

        public void Bind(Func<SurvivorTaskBoardSnapshot> getSnapshot)
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

        public bool IncreasePriority() => RequestPriorityAdjustment(1);
        public bool DecreasePriority() => RequestPriorityAdjustment(-1);

        public bool SelectNextShift() => SelectShift(1);
        public bool SelectPreviousShift() => SelectShift(-1);
        public bool SelectNextSurvivor() => SelectSurvivor(1);
        public bool SelectPreviousSurvivor() => SelectSurvivor(-1);

        public bool AssignSelectedShift()
        {
            if (!IsOpen) return false;
            var shift = GetSelectedShift();
            var survivor = GetSelectedSurvivor();
            if (shift == null || survivor == null)
            {
                ReportOutcome("Select a duty shift and a survivor first.");
                return false;
            }
            if (OnShiftAssignmentRequested == null)
            {
                ReportOutcome("Task board duty link is offline.");
                return false;
            }

            OnShiftAssignmentRequested.Invoke(shift.Duty, survivor.SurvivorId);
            return true;
        }

        /// <summary>Ask Core to approve the highest-priority pending duty recommendation.</summary>
        public bool ApproveTopShiftRecommendation()
        {
            if (!IsOpen) return false;
            if (_snapshot == null || _snapshot.ShiftRecommendations == null
                || _snapshot.ShiftRecommendations.Count == 0)
            {
                ReportOutcome("No pending duty recommendation.");
                return false;
            }
            if (OnShiftRecommendationApprovalRequested == null)
            {
                ReportOutcome("Task board recommendation link is offline.");
                return false;
            }

            OnShiftRecommendationApprovalRequested.Invoke();
            return true;
        }

        public bool CancelActiveTask()
        {
            if (!IsOpen) return false;
            var shift = GetSelectedShift();
            if (shift != null && !string.IsNullOrEmpty(shift.AssignedSurvivorId))
            {
                if (OnShiftCancellationRequested == null)
                {
                    ReportOutcome("Task board duty link is offline.");
                    return false;
                }
                OnShiftCancellationRequested.Invoke(shift.Duty);
                return true;
            }
            if (OnCancellationRequested == null)
            {
                ReportOutcome("Task board link is offline.");
                return false;
            }
            OnCancellationRequested.Invoke();
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No task-board report." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            ClampSelections();
            RebuildPanel();
            OnSurvivorTaskBoardChanged?.Invoke();
        }

        private bool RequestPriorityAdjustment(int direction)
        {
            if (!IsOpen || direction == 0) return false;
            if (OnPriorityAdjustmentRequested == null)
            {
                ReportOutcome("Task board link is offline.");
                return false;
            }
            OnPriorityAdjustmentRequested.Invoke(direction);
            return true;
        }

        private bool SelectShift(int direction)
        {
            int count = _snapshot != null && _snapshot.ShiftSlots != null ? _snapshot.ShiftSlots.Count : 0;
            if (!IsOpen || count == 0 || direction == 0) return false;
            SelectedShiftIndex = CycleIndex(SelectedShiftIndex, count, direction);
            Refresh();
            return true;
        }

        private bool SelectSurvivor(int direction)
        {
            int count = _snapshot != null && _snapshot.SurvivorAssignments != null
                ? _snapshot.SurvivorAssignments.Count
                : 0;
            if (!IsOpen || count == 0 || direction == 0) return false;
            SelectedSurvivorIndex = CycleIndex(SelectedSurvivorIndex, count, direction);
            Refresh();
            return true;
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("SURVIVOR TASK BOARD  [V] close  ·  [TAB] duty  ·  [←/→] crew  ·  [ENTER] assign / relief");
            sb.Append("\n[,/.] repair priority  ·  [BACKSPACE] cancel selected shift / repair");
            sb.Append("\n[R] approve top duty recommendation");
            if (_snapshot == null)
            {
                sb.Append("\nTask allocation data is unavailable.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\n--- ACTIVE WORK ORDERS ---");
            if (_snapshot.ActiveTasks == null || _snapshot.ActiveTasks.Count == 0)
            {
                sb.Append("\nnone.");
            }
            else
            {
                for (int i = 0; i < _snapshot.ActiveTasks.Count; i++)
                    AppendTask(sb, _snapshot.ActiveTasks[i]);
            }

            sb.Append("\n--- SHIFT RECOMMENDATIONS ---");
            if (_snapshot.ShiftRecommendations == null || _snapshot.ShiftRecommendations.Count == 0)
            {
                sb.Append("\nnone.");
            }
            else
            {
                for (int i = 0; i < _snapshot.ShiftRecommendations.Count; i++)
                    AppendRecommendation(sb, _snapshot.ShiftRecommendations[i], i == 0);
            }

            sb.Append("\n--- DUTY SHIFTS ---");
            if (_snapshot.ShiftSlots == null || _snapshot.ShiftSlots.Count == 0)
            {
                sb.Append("\nDuty-shift data is unavailable.");
            }
            else
            {
                for (int i = 0; i < _snapshot.ShiftSlots.Count; i++)
                    AppendShift(sb, _snapshot.ShiftSlots[i], i == SelectedShiftIndex);
            }

            var selectedCrew = GetSelectedSurvivor();
            if (selectedCrew != null)
                sb.Append("\nCREW PICK: ").Append(selectedCrew.SurvivorName)
                    .Append(" — ").Append(selectedCrew.AssignmentLabel ?? selectedCrew.State ?? "unavailable");

            sb.Append("\n--- CREW ALLOCATION ---");
            if (_snapshot.SurvivorAssignments == null || _snapshot.SurvivorAssignments.Count == 0)
            {
                sb.Append("\nNo survivor roster available.");
            }
            else
            {
                for (int i = 0; i < _snapshot.SurvivorAssignments.Count; i++)
                    AppendAssignment(sb, _snapshot.SurvivorAssignments[i], i == SelectedSurvivorIndex);
            }

            if (!string.IsNullOrEmpty(_snapshot.LastReport))
                sb.Append("\nBOARD: ").Append(_snapshot.LastReport);
            if (!string.IsNullOrEmpty(LastOutcome))
                sb.Append("\nREPORT: ").Append(LastOutcome);
            PanelSummary = sb.ToString();
        }

        private static void AppendTask(StringBuilder sb, SurvivorTaskBoardTask task)
        {
            if (task == null) return;
            sb.Append("\n> ").Append((task.DisplayName ?? "work order").ToUpperInvariant())
                .Append(": ").Append((task.Status ?? "queued").ToUpperInvariant())
                .Append("  ·  ").Append(task.AssignedSurvivorName ?? "unassigned")
                .Append("  ·  ").Append(task.ProgressHours.ToString("0.0"));
            if (task.IsWorkShift)
            {
                sb.Append("h staffed");
                if (!string.IsNullOrEmpty(task.EffectSummary))
                    sb.Append("  ·  ").Append(task.EffectSummary);
                AppendRotation(sb, task.HoursSinceHandover, task.RotationHours,
                    task.ReliefSurvivorName, task.HandoverCount);
                return;
            }
            sb.Append("/").Append(task.RequiredWorkHours.ToString("0.0")).Append("h")
                .Append("  ·  ").Append(PriorityLabel(task.Priority));
        }

        private static void AppendShift(StringBuilder sb, SurvivorWorkShiftSlotSnapshot shift, bool selected)
        {
            if (shift == null) return;
            sb.Append("\n").Append(selected ? "> " : "  ")
                .Append((shift.DisplayName ?? "duty shift").ToUpperInvariant()).Append(": ");
            if (!shift.IsSupported)
            {
                sb.Append("OFFLINE");
            }
            else if (string.IsNullOrEmpty(shift.AssignedSurvivorId))
            {
                sb.Append("UNASSIGNED");
            }
            else
            {
                sb.Append(shift.AssignedSurvivorName ?? shift.AssignedSurvivorId)
                    .Append("  ·  ").Append(shift.HoursWorked.ToString("0.0")).Append("h staffed");
                AppendRotation(sb, shift.HoursSinceHandover, shift.RotationHours,
                    shift.ReliefSurvivorName, shift.HandoverCount);
            }
            if (!string.IsNullOrEmpty(shift.EffectSummary))
                sb.Append("  ·  ").Append(shift.EffectSummary);
            if (!string.IsNullOrEmpty(shift.AssignedSurvivorId)
                && shift.Availability != null
                && !string.IsNullOrEmpty(shift.Availability.Summary))
                sb.Append("  ·  ").Append(shift.Availability.Summary);
        }

        private static void AppendRotation(
            StringBuilder sb,
            float hoursSinceHandover,
            float rotationHours,
            string reliefSurvivorName,
            int handoverCount)
        {
            if (rotationHours > 0f)
                sb.Append("  ·  rotate ").Append(hoursSinceHandover.ToString("0.0"))
                    .Append("/").Append(rotationHours.ToString("0.0")).Append("h");
            if (!string.IsNullOrEmpty(reliefSurvivorName))
                sb.Append("  ·  RELIEF: ").Append(reliefSurvivorName);
            if (handoverCount > 0)
                sb.Append("  ·  handovers ").Append(handoverCount);
        }

        private static void AppendAssignment(StringBuilder sb, SurvivorTaskBoardAssignment assignment, bool selected)
        {
            if (assignment == null) return;
            sb.Append("\n").Append(selected ? "> " : "  ")
                .Append(assignment.SurvivorName ?? "unknown")
                .Append(": ").Append(assignment.AssignmentLabel ?? assignment.State ?? "unavailable");
        }

        private static void AppendRecommendation(
            StringBuilder sb,
            SurvivorWorkShiftRecommendationSnapshot recommendation,
            bool isTopRecommendation)
        {
            if (recommendation == null) return;
            sb.Append("\n").Append(isTopRecommendation ? "> " : "  ")
                .Append(recommendation.Priority.ToString().ToUpperInvariant())
                .Append(" · ").Append(SurvivorWorkShiftSystem.DutyDisplayName(recommendation.Duty).ToUpperInvariant())
                .Append(" — ").Append(recommendation.SuggestedSurvivorName ?? "no rested crew")
                .Append("  ·  ").Append(recommendation.Reason ?? "Duty staffing is recommended.");
            if (isTopRecommendation) sb.Append("  ·  [R] APPROVE");
        }

        private SurvivorWorkShiftSlotSnapshot GetSelectedShift()
        {
            if (_snapshot == null || _snapshot.ShiftSlots == null
                || SelectedShiftIndex < 0 || SelectedShiftIndex >= _snapshot.ShiftSlots.Count)
                return null;
            return _snapshot.ShiftSlots[SelectedShiftIndex];
        }

        private SurvivorTaskBoardAssignment GetSelectedSurvivor()
        {
            if (_snapshot == null || _snapshot.SurvivorAssignments == null
                || SelectedSurvivorIndex < 0 || SelectedSurvivorIndex >= _snapshot.SurvivorAssignments.Count)
                return null;
            return _snapshot.SurvivorAssignments[SelectedSurvivorIndex];
        }

        private void ClampSelections()
        {
            int shiftCount = _snapshot != null && _snapshot.ShiftSlots != null ? _snapshot.ShiftSlots.Count : 0;
            int survivorCount = _snapshot != null && _snapshot.SurvivorAssignments != null
                ? _snapshot.SurvivorAssignments.Count
                : 0;
            SelectedShiftIndex = shiftCount > 0 ? Mathf.Clamp(SelectedShiftIndex, 0, shiftCount - 1) : -1;
            SelectedSurvivorIndex = survivorCount > 0 ? Mathf.Clamp(SelectedSurvivorIndex, 0, survivorCount - 1) : -1;
        }

        private static int CycleIndex(int current, int count, int direction)
        {
            if (count <= 0) return -1;
            return (current + (direction > 0 ? 1 : -1) + count) % count;
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
