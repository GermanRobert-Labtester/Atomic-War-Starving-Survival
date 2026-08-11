using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Read-only roster and command surface for survivor work already owned by
    /// other systems. The board deliberately does not duplicate work-order state:
    /// it observes the repair order and reserves its assigned survivor while the
    /// order is queued as well as while it is being performed.
    /// </summary>
    public sealed class SurvivorTaskBoardSystem : IDisposable
    {
        public const string SystemId = "survivor_task_board";

        private readonly RepairWorkOrderSystem _repairWorkOrders;
        private readonly SurvivorWorkShiftSystem _workShifts;
        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;
        private string _lastReport = "No active work orders.";

        /// <summary>Raised whenever board-visible allocation or command feedback changes.</summary>
        public event Action OnChanged;

        public SurvivorTaskBoardSystem(
            RepairWorkOrderSystem repairWorkOrders,
            Func<IReadOnlyList<Survivor>> getSurvivors,
            SurvivorWorkShiftSystem workShifts = null)
        {
            _repairWorkOrders = repairWorkOrders;
            _getSurvivors = getSurvivors;
            _workShifts = workShifts;
            if (_repairWorkOrders != null)
                _repairWorkOrders.OnChanged += HandleRepairWorkOrderChanged;
            if (_workShifts != null)
                _workShifts.OnChanged += HandleWorkShiftsChanged;
        }

        /// <summary>
        /// Returns a human-facing conflict reason whenever the survivor has been
        /// reserved by a board-tracked order. A queued repair is intentionally a
        /// reservation: dispatching that worker elsewhere would create two owners.
        /// </summary>
        public string GetAssignmentConflictReason(Survivor survivor)
        {
            if (survivor == null) return null;
            if (_workShifts != null && _workShifts.TryGetShiftForSurvivor(survivor.Id, out var shift))
            {
                string workerName = string.IsNullOrEmpty(survivor.DisplayName) ? "Operative" : survivor.DisplayName;
                return workerName + (shift.IsReliefAssignment ? " is on relief for " : " is committed to ")
                    + shift.DisplayName + ".";
            }

            var repair = _repairWorkOrders != null ? _repairWorkOrders.GetSnapshot() : null;
            if (repair == null || !repair.HasActiveOrder
                || !string.Equals(repair.AssignedSurvivorId, survivor.Id, StringComparison.Ordinal)) return null;

            string worker = string.IsNullOrEmpty(repair.AssignedSurvivorName)
                ? survivor.DisplayName
                : repair.AssignedSurvivorName;
            return (string.IsNullOrEmpty(worker) ? "Operative" : worker)
                + " is committed to bunker repair.";
        }

        /// <summary>Raise or lower the only current board task's urgency.</summary>
        public bool TryAdjustActivePriority(int direction, out SurvivorTaskBoardActionResult result)
        {
            result = new SurvivorTaskBoardActionResult();
            if (_repairWorkOrders == null)
            {
                result.Reason = "Task board link is offline.";
                return false;
            }

            bool adjusted = _repairWorkOrders.TryAdjustPriority(direction, out var repairResult);
            result.Succeeded = adjusted;
            result.Reason = repairResult != null ? repairResult.Reason : "Task board report unavailable.";
            if (!adjusted) return false;

            _lastReport = result.Reason;
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Cancel the active board task without spending reserved materials.</summary>
        public bool CancelActiveTask(out SurvivorTaskBoardActionResult result)
        {
            result = new SurvivorTaskBoardActionResult();
            if (_repairWorkOrders == null)
            {
                result.Reason = "Task board link is offline.";
                return false;
            }

            bool cancelled = _repairWorkOrders.CancelActiveOrder(out var repairResult);
            result.Succeeded = cancelled;
            result.Cancelled = cancelled && repairResult != null && repairResult.Cancelled;
            result.Reason = repairResult != null ? repairResult.Reason : "Task board report unavailable.";
            if (!cancelled) return false;

            _lastReport = result.Reason;
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Assign an idle survivor to one supported continuous shelter duty.</summary>
        public bool TryAssignShift(
            WorkShiftDuty duty,
            string survivorId,
            out SurvivorTaskBoardActionResult result)
        {
            result = new SurvivorTaskBoardActionResult();
            if (_workShifts == null)
            {
                result.Reason = "Task-board duty link is offline.";
                return false;
            }

            bool assigned = _workShifts.TryAssign(duty, survivorId, out var shiftResult);
            result.Succeeded = assigned;
            result.Reason = shiftResult != null ? shiftResult.Reason : "Duty-shift report unavailable.";
            if (!assigned) return false;

            _lastReport = result.Reason;
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Accept the highest-priority pending duty suggestion; this never auto-assigns on its own.</summary>
        public bool TryApproveTopShiftRecommendation(out SurvivorTaskBoardActionResult result)
        {
            result = new SurvivorTaskBoardActionResult();
            if (_workShifts == null)
            {
                result.Reason = "Task-board recommendation link is offline.";
                return false;
            }

            var recommendations = _workShifts.GetSnapshot().Recommendations;
            if (recommendations == null || recommendations.Count == 0)
            {
                result.Reason = "HELD: no pending duty recommendation.";
                return false;
            }

            bool approved = _workShifts.TryApproveRecommendation(recommendations[0].Duty, out var shiftResult);
            result.Succeeded = approved;
            result.Reason = shiftResult != null ? shiftResult.Reason : "Duty recommendation report unavailable.";
            if (!approved) return false;

            _lastReport = result.Reason;
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Release one selected shift while retaining unrelated repair work.</summary>
        public bool CancelShift(WorkShiftDuty duty, out SurvivorTaskBoardActionResult result)
        {
            result = new SurvivorTaskBoardActionResult();
            if (_workShifts == null)
            {
                result.Reason = "Task-board duty link is offline.";
                return false;
            }

            bool cancelled = _workShifts.CancelShift(duty, out var shiftResult);
            result.Succeeded = cancelled;
            result.Cancelled = cancelled && shiftResult != null && shiftResult.Cancelled;
            result.Reason = shiftResult != null ? shiftResult.Reason : "Duty-shift report unavailable.";
            if (!cancelled) return false;

            _lastReport = result.Reason;
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Detached, serializable state only; work ownership persists in its owner system.</summary>
        public SurvivorTaskBoardSnapshot GetSnapshot()
        {
            var snapshot = new SurvivorTaskBoardSnapshot
            {
                LastReport = _lastReport,
                ActiveTasks = new List<SurvivorTaskBoardTask>(),
                SurvivorAssignments = new List<SurvivorTaskBoardAssignment>(),
                ShiftSlots = new List<SurvivorWorkShiftSlotSnapshot>(),
                ShiftRecommendations = new List<SurvivorWorkShiftRecommendationSnapshot>()
            };
            RepairWorkOrderSnapshot repair = _repairWorkOrders != null
                ? _repairWorkOrders.GetSnapshot()
                : null;

            if (repair != null && repair.HasActiveOrder)
            {
                snapshot.ActiveTasks.Add(new SurvivorTaskBoardTask
                {
                    DisplayName = "Bunker repair",
                    Status = repair.Status == RepairWorkOrderStatus.Working ? "working" : "queued",
                    AssignedSurvivorId = repair.AssignedSurvivorId,
                    AssignedSurvivorName = repair.AssignedSurvivorName,
                    Priority = repair.Priority,
                    ProgressHours = repair.ProgressHours,
                    RequiredWorkHours = repair.RequiredWorkHours
                });
            }

            var shifts = _workShifts != null ? _workShifts.GetSnapshot() : null;
            if (shifts != null && shifts.Recommendations != null)
            {
                for (int i = 0; i < shifts.Recommendations.Count; i++)
                {
                    var recommendation = shifts.Recommendations[i];
                    if (recommendation != null) snapshot.ShiftRecommendations.Add(recommendation);
                }
            }
            if (shifts != null && shifts.Slots != null)
            {
                for (int i = 0; i < shifts.Slots.Count; i++)
                {
                    var shift = shifts.Slots[i];
                    if (shift == null) continue;
                    snapshot.ShiftSlots.Add(shift);
                    if (string.IsNullOrEmpty(shift.AssignedSurvivorId)) continue;
                    snapshot.ActiveTasks.Add(new SurvivorTaskBoardTask
                    {
                        IsWorkShift = true,
                        WorkShiftDuty = shift.Duty,
                        DisplayName = shift.DisplayName + " shift",
                        Status = string.IsNullOrEmpty(shift.ReliefSurvivorId) ? "staffed" : "relief ready",
                        AssignedSurvivorId = shift.AssignedSurvivorId,
                        AssignedSurvivorName = shift.AssignedSurvivorName,
                        ReliefSurvivorId = shift.ReliefSurvivorId,
                        ReliefSurvivorName = shift.ReliefSurvivorName,
                        ProgressHours = shift.HoursWorked,
                        HoursSinceHandover = shift.HoursSinceHandover,
                        RotationHours = shift.RotationHours,
                        HandoverCount = shift.HandoverCount,
                        EffectSummary = shift.EffectSummary
                    });
                }
            }

            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors == null) return snapshot;
            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (survivor == null) continue;
                string conflict = GetAssignmentConflictReason(survivor);
                snapshot.SurvivorAssignments.Add(new SurvivorTaskBoardAssignment
                {
                    SurvivorId = survivor.Id,
                    SurvivorName = string.IsNullOrEmpty(survivor.DisplayName) ? survivor.Id : survivor.DisplayName,
                    State = survivor.State.ToString(),
                    AssignmentLabel = string.IsNullOrEmpty(conflict)
                        ? AvailabilityLabel(survivor)
                        : ReservationLabel(survivor),
                    IsReserved = !string.IsNullOrEmpty(conflict)
                });
            }
            return snapshot;
        }

        public SurvivorTaskBoardSave CaptureState()
        {
            return new SurvivorTaskBoardSave
            {
                systemId = SystemId,
                lastReport = _lastReport
            };
        }

        public void RestoreState(SurvivorTaskBoardSave state)
        {
            _lastReport = state != null && !string.IsNullOrEmpty(state.lastReport)
                ? state.lastReport
                : "No active work orders.";
            OnChanged?.Invoke();
        }

        public void Dispose()
        {
            if (_repairWorkOrders != null)
                _repairWorkOrders.OnChanged -= HandleRepairWorkOrderChanged;
            if (_workShifts != null)
                _workShifts.OnChanged -= HandleWorkShiftsChanged;
        }

        private void HandleRepairWorkOrderChanged()
        {
            var repair = _repairWorkOrders != null ? _repairWorkOrders.GetSnapshot() : null;
            if (repair != null && !string.IsNullOrEmpty(repair.LastReport))
                _lastReport = repair.LastReport;
            OnChanged?.Invoke();
        }

        private void HandleWorkShiftsChanged()
        {
            var shifts = _workShifts != null ? _workShifts.GetSnapshot() : null;
            if (shifts != null && !string.IsNullOrEmpty(shifts.LastReport))
                _lastReport = shifts.LastReport;
            OnChanged?.Invoke();
        }

        private static string AvailabilityLabel(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive) return "unavailable";
            if (survivor.IsOnExpedition) return "on expedition";
            if (survivor.State == SurvivorState.Incapacitated) return "incapacitated";
            if (survivor.State == SurvivorState.Working) return "working elsewhere";
            return "available";
        }

        private string ReservationLabel(Survivor survivor)
        {
            if (_workShifts != null && _workShifts.TryGetShiftForSurvivor(survivor.Id, out var shift))
                return shift.DisplayName + (shift.IsReliefAssignment ? " relief [reserved]" : " [reserved]");
            return "Bunker repair [reserved]";
        }
    }

    [Serializable]
    public sealed class SurvivorTaskBoardSave
    {
        public string systemId = SurvivorTaskBoardSystem.SystemId;
        public string lastReport;
    }

    [Serializable]
    public sealed class SurvivorTaskBoardSnapshot
    {
        public List<SurvivorTaskBoardTask> ActiveTasks;
        public List<SurvivorTaskBoardAssignment> SurvivorAssignments;
        public List<SurvivorWorkShiftSlotSnapshot> ShiftSlots;
        public List<SurvivorWorkShiftRecommendationSnapshot> ShiftRecommendations;
        public string LastReport;
    }

    [Serializable]
    public sealed class SurvivorTaskBoardTask
    {
        public bool IsWorkShift;
        public WorkShiftDuty WorkShiftDuty;
        public string DisplayName;
        public string Status;
        public string AssignedSurvivorId;
        public string AssignedSurvivorName;
        public string ReliefSurvivorId;
        public string ReliefSurvivorName;
        public MaintenanceRepairPriority Priority;
        public float ProgressHours;
        public float RequiredWorkHours;
        public float HoursSinceHandover;
        public float RotationHours;
        public int HandoverCount;
        public string EffectSummary;
    }

    [Serializable]
    public sealed class SurvivorTaskBoardAssignment
    {
        public string SurvivorId;
        public string SurvivorName;
        public string State;
        public string AssignmentLabel;
        public bool IsReserved;
    }

    [Serializable]
    public sealed class SurvivorTaskBoardActionResult
    {
        public bool Succeeded;
        public bool Cancelled;
        public string Reason;
    }
}
