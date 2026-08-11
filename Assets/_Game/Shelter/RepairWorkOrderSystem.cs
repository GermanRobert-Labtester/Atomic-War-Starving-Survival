using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Lifecycle of a single, assigned bunker repair task.</summary>
    public enum RepairWorkOrderStatus
    {
        Queued,
        Working
    }

    /// <summary>
    /// Bridges the maintenance terminal to Utility AI. The terminal queues one
    /// material-safe repair order; only its assigned survivor can claim it through
    /// RepairWorkOrderActionSO. Materials are consumed by BunkerMaintenanceSystem
    /// only after enough uninterrupted game-hours have elapsed.
    /// </summary>
    public sealed class RepairWorkOrderSystem : IDisposable
    {
        public const string SystemId = "repair_work_order";
        public const float ModuleWorkHours = 2f;
        public const float PowerSourceWorkHours = 3f;

        private readonly BunkerMaintenanceSystem _maintenance;
        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;
        private RepairWorkOrderState _order;
        private string _lastReport = "No active repair work order.";
        private bool _isCompleting;

        /// <summary>Raised for every queued, started, progressed, cancelled, or completed change.</summary>
        public event Action OnChanged;
        public event Action<RepairWorkOrderResult> OnOrderQueued;
        public event Action<RepairWorkOrderResult> OnOrderStarted;
        public event Action<RepairWorkOrderResult> OnOrderCancelled;
        public event Action<RepairWorkOrderResult> OnOrderCompleted;

        public bool HasActiveOrder => _order != null;

        public RepairWorkOrderSystem(
            BunkerMaintenanceSystem maintenance,
            Func<IReadOnlyList<Survivor>> getSurvivors)
        {
            _maintenance = maintenance;
            _getSurvivors = getSurvivors;
            if (_maintenance != null)
                _maintenance.OnChanged += HandleMaintenanceChanged;
        }

        /// <summary>Queue one target using the terminal's current worker and urgency.</summary>
        public bool TryQueue(
            MaintenanceTargetType targetType,
            string targetId,
            out RepairWorkOrderResult result)
        {
            result = new RepairWorkOrderResult
            {
                TargetType = targetType,
                TargetId = targetId
            };

            if (_order != null)
            {
                result.Reason = "HELD: cancel the active repair work order before issuing another.";
                return false;
            }
            if (!ValidateCandidate(targetType, targetId, out var reason))
            {
                result.Reason = reason;
                return false;
            }

            _order = new RepairWorkOrderState
            {
                TargetType = targetType,
                TargetId = targetId,
                AssignedSurvivorId = _maintenance.AssignedSurvivorId,
                Priority = _maintenance.RepairPriority,
                RequiredWorkHours = RequiredHoursFor(targetType),
                Status = RepairWorkOrderStatus.Queued
            };
            _lastReport = "QUEUED: " + TargetDisplayName(targetType, targetId) + " awaits "
                + WorkerDisplayName(_order.AssignedSurvivorId) + ".";
            result.Succeeded = true;
            result.AssignedSurvivorId = _order.AssignedSurvivorId;
            result.Priority = _order.Priority;
            result.Reason = _lastReport;
            OnOrderQueued?.Invoke(result);
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Utility-AI gate: only the terminal-assigned, able survivor may claim the order.</summary>
        public bool CanSurvivorWork(Survivor survivor)
        {
            if (_order == null || survivor == null || !survivor.IsAlive) return false;
            if (!string.Equals(survivor.Id, _order.AssignedSurvivorId, StringComparison.Ordinal)) return false;
            return ValidateOrder(out _);
        }

        /// <summary>Utility-AI execute hook that starts the queued order without consuming materials.</summary>
        public bool TryStartWork(Survivor survivor, out RepairWorkOrderResult result)
        {
            result = ResultForCurrentOrder();
            if (_order == null)
            {
                result.Reason = "HELD: no repair work order is queued.";
                return false;
            }
            if (!CanSurvivorWork(survivor))
            {
                result.Reason = "HELD: this survivor cannot claim the active repair work order.";
                return false;
            }
            if (_order.Status == RepairWorkOrderStatus.Working)
            {
                result.Succeeded = true;
                result.Reason = "WORKING: repair task already claimed.";
                return true;
            }

            _order.Status = RepairWorkOrderStatus.Working;
            survivor.State = SurvivorState.Working;
            _lastReport = "WORKING: " + WorkerDisplayName(survivor.Id) + " started "
                + TargetDisplayName(_order.TargetType, _order.TargetId) + ".";
            result = ResultForCurrentOrder();
            result.Succeeded = true;
            result.Reason = _lastReport;
            OnOrderStarted?.Invoke(result);
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Advance work that Utility AI has claimed. Invalidated orders are cancelled
        /// before the completion primitive runs, so no material is lost on a blackout,
        /// reassignment, death, expedition departure, or removed target.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (_order == null || _order.Status != RepairWorkOrderStatus.Working || gameHours <= 0f)
                return;
            if (!ValidateOrder(out var reason))
            {
                CancelInternal(reason);
                return;
            }

            _order.ProgressHours = Mathf.Min(
                _order.RequiredWorkHours,
                _order.ProgressHours + gameHours);
            if (_order.ProgressHours < _order.RequiredWorkHours)
            {
                OnChanged?.Invoke();
                return;
            }

            var completing = _order;
            _isCompleting = true;
            BunkerMaintenanceRepairResult repairResult = null;
            bool repaired = _maintenance != null && _maintenance.TryRepair(
                completing.TargetType,
                completing.TargetId,
                out repairResult);
            _isCompleting = false;
            if (!repaired)
            {
                CancelInternal(repairResult != null ? repairResult.Reason : "maintenance completion failed.");
                return;
            }

            ReleaseWorker(completing.AssignedSurvivorId);
            _order = null;
            _lastReport = repairResult.Reason;
            var result = new RepairWorkOrderResult
            {
                Succeeded = true,
                TargetType = completing.TargetType,
                TargetId = completing.TargetId,
                AssignedSurvivorId = completing.AssignedSurvivorId,
                Priority = completing.Priority,
                ProgressHours = completing.RequiredWorkHours,
                RequiredWorkHours = completing.RequiredWorkHours,
                Reason = _lastReport
            };
            OnOrderCompleted?.Invoke(result);
            OnChanged?.Invoke();
        }

        /// <summary>Explicit player cancellation. Work progress and material reservations are discarded safely.</summary>
        public bool CancelActiveOrder(out RepairWorkOrderResult result)
        {
            if (_order == null)
            {
                result = new RepairWorkOrderResult { Reason = "HELD: no repair work order is active." };
                return false;
            }
            CancelInternal("cancelled at the maintenance terminal.");
            result = new RepairWorkOrderResult
            {
                Succeeded = true,
                Cancelled = true,
                Reason = _lastReport
            };
            return true;
        }

        /// <summary>
        /// Changes the urgency of the live order through the maintenance system so
        /// the terminal, Utility AI, saved state, and task board retain one source
        /// of truth for priority.
        /// </summary>
        public bool TryAdjustPriority(int direction, out RepairWorkOrderResult result)
        {
            result = ResultForCurrentOrder();
            if (_order == null)
            {
                result.Reason = "HELD: no repair work order is active.";
                return false;
            }
            if (direction == 0)
            {
                result.Reason = "HELD: repair priority was unchanged.";
                return false;
            }
            if (_maintenance == null || !_maintenance.AdjustPriority(direction))
            {
                result.Reason = "HELD: repair priority is already at that limit.";
                return false;
            }

            result = ResultForCurrentOrder();
            result.Succeeded = true;
            result.Reason = "PRIORITY UPDATED: " + PriorityLabel(result.Priority) + ".";
            return true;
        }

        /// <summary>Detached UI state; it contains no live shelter or survivor references.</summary>
        public RepairWorkOrderSnapshot GetSnapshot()
        {
            var snapshot = new RepairWorkOrderSnapshot
            {
                HasActiveOrder = _order != null,
                LastReport = _lastReport
            };
            if (_order == null) return snapshot;

            snapshot.Status = _order.Status;
            snapshot.TargetType = _order.TargetType;
            snapshot.TargetId = _order.TargetId;
            snapshot.TargetDisplayName = TargetDisplayName(_order.TargetType, _order.TargetId);
            snapshot.AssignedSurvivorId = _order.AssignedSurvivorId;
            snapshot.AssignedSurvivorName = WorkerDisplayName(_order.AssignedSurvivorId);
            snapshot.Priority = _order.Priority;
            snapshot.ProgressHours = _order.ProgressHours;
            snapshot.RequiredWorkHours = _order.RequiredWorkHours;
            return snapshot;
        }

        public RepairWorkOrderSave CaptureState()
        {
            return new RepairWorkOrderSave
            {
                systemId = SystemId,
                hasActiveOrder = _order != null,
                status = _order != null ? (int)_order.Status : (int)RepairWorkOrderStatus.Queued,
                targetType = _order != null ? (int)_order.TargetType : (int)MaintenanceTargetType.Module,
                targetId = _order != null ? _order.TargetId : null,
                assignedSurvivorId = _order != null ? _order.AssignedSurvivorId : null,
                priority = _order != null ? (int)_order.Priority : (int)MaintenanceRepairPriority.Standard,
                progressHours = _order != null ? _order.ProgressHours : 0f,
                requiredWorkHours = _order != null ? _order.RequiredWorkHours : 0f,
                lastReport = _lastReport
            };
        }

        /// <summary>
        /// Loaded working orders intentionally resume as queued. The next Utility-AI
        /// evaluation must consciously reclaim them, preventing hidden work from
        /// continuing while a save is paused or restored into changed bunker state.
        /// </summary>
        public void RestoreState(RepairWorkOrderSave state)
        {
            _order = null;
            _lastReport = state != null && !string.IsNullOrEmpty(state.lastReport)
                ? state.lastReport
                : "No active repair work order.";
            if (state == null || !state.hasActiveOrder)
            {
                OnChanged?.Invoke();
                return;
            }

            var targetType = (MaintenanceTargetType)Mathf.Clamp(
                state.targetType,
                (int)MaintenanceTargetType.Module,
                (int)MaintenanceTargetType.PowerSource);
            _order = new RepairWorkOrderState
            {
                Status = RepairWorkOrderStatus.Queued,
                TargetType = targetType,
                TargetId = state.targetId,
                AssignedSurvivorId = state.assignedSurvivorId,
                Priority = (MaintenanceRepairPriority)Mathf.Clamp(
                    state.priority,
                    (int)MaintenanceRepairPriority.Low,
                    (int)MaintenanceRepairPriority.Critical),
                ProgressHours = Mathf.Max(0f, state.progressHours),
                RequiredWorkHours = state.requiredWorkHours > 0f
                    ? state.requiredWorkHours
                    : RequiredHoursFor(targetType)
            };
            _order.ProgressHours = Mathf.Min(_order.ProgressHours, _order.RequiredWorkHours);
            if (!ValidateOrder(out var reason))
            {
                _order = null;
                _lastReport = "CANCELLED: " + reason;
            }
            else
            {
                _lastReport = "QUEUED AFTER LOAD: " + TargetDisplayName(targetType, state.targetId)
                    + " awaits " + WorkerDisplayName(state.assignedSurvivorId) + ".";
            }
            OnChanged?.Invoke();
        }

        public void Dispose()
        {
            if (_maintenance != null)
                _maintenance.OnChanged -= HandleMaintenanceChanged;
        }

        private bool ValidateCandidate(MaintenanceTargetType targetType, string targetId, out string reason)
        {
            if (_maintenance == null)
            {
                reason = "HELD: maintenance controls are offline.";
                return false;
            }
            if (!_maintenance.CanStartRepairWork(targetType, targetId, out var maintenanceResult))
            {
                reason = maintenanceResult != null ? maintenanceResult.Reason : "HELD: maintenance validation failed.";
                return false;
            }

            var survivor = FindSurvivor(_maintenance.AssignedSurvivorId);
            if (!CanPerformRepair(survivor))
            {
                reason = "HELD: the assigned survivor is unavailable for repair work.";
                return false;
            }

            reason = null;
            return true;
        }

        private bool ValidateOrder(out string reason)
        {
            if (_order == null)
            {
                reason = "no repair work order is active.";
                return false;
            }
            if (_maintenance == null)
            {
                reason = "maintenance controls are offline.";
                return false;
            }
            if (!string.Equals(_maintenance.AssignedSurvivorId, _order.AssignedSurvivorId, StringComparison.Ordinal))
            {
                reason = "the assigned repair worker changed.";
                return false;
            }
            var survivor = FindSurvivor(_order.AssignedSurvivorId);
            if (!CanPerformRepair(survivor))
            {
                reason = "the assigned survivor is unavailable for repair work.";
                return false;
            }
            if (_order.Status == RepairWorkOrderStatus.Working && survivor.State != SurvivorState.Working)
            {
                reason = "the repair task was interrupted by another survivor state.";
                return false;
            }
            if (!_maintenance.CanStartRepairWork(_order.TargetType, _order.TargetId, out var maintenanceResult))
            {
                reason = maintenanceResult != null ? maintenanceResult.Reason : "maintenance validation failed.";
                return false;
            }

            reason = null;
            return true;
        }

        private void HandleMaintenanceChanged()
        {
            if (_order == null || _isCompleting) return;

            if (_maintenance != null && _order.Priority != _maintenance.RepairPriority
                && string.Equals(_order.AssignedSurvivorId, _maintenance.AssignedSurvivorId, StringComparison.Ordinal))
            {
                _order.Priority = _maintenance.RepairPriority;
                _lastReport = "PRIORITY UPDATED: " + PriorityLabel(_order.Priority) + ".";
                OnChanged?.Invoke();
            }
            if (!ValidateOrder(out var reason))
                CancelInternal(reason);
        }

        private void CancelInternal(string reason)
        {
            if (_order == null) return;
            var cancelled = _order;
            ReleaseWorker(cancelled.AssignedSurvivorId);
            _order = null;
            _lastReport = "CANCELLED: " + reason;
            var result = new RepairWorkOrderResult
            {
                Succeeded = true,
                Cancelled = true,
                TargetType = cancelled.TargetType,
                TargetId = cancelled.TargetId,
                AssignedSurvivorId = cancelled.AssignedSurvivorId,
                Priority = cancelled.Priority,
                ProgressHours = cancelled.ProgressHours,
                RequiredWorkHours = cancelled.RequiredWorkHours,
                Reason = _lastReport
            };
            OnOrderCancelled?.Invoke(result);
            OnChanged?.Invoke();
        }

        private RepairWorkOrderResult ResultForCurrentOrder()
        {
            return _order == null
                ? new RepairWorkOrderResult()
                : new RepairWorkOrderResult
                {
                    TargetType = _order.TargetType,
                    TargetId = _order.TargetId,
                    AssignedSurvivorId = _order.AssignedSurvivorId,
                    Priority = _order.Priority,
                    ProgressHours = _order.ProgressHours,
                    RequiredWorkHours = _order.RequiredWorkHours
                };
        }

        private static float RequiredHoursFor(MaintenanceTargetType targetType) =>
            targetType == MaintenanceTargetType.PowerSource ? PowerSourceWorkHours : ModuleWorkHours;

        private Survivor FindSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (survivor != null && string.Equals(survivor.Id, survivorId, StringComparison.Ordinal))
                    return survivor;
            }
            return null;
        }

        private static bool CanPerformRepair(Survivor survivor) => survivor != null
            && survivor.IsAlive
            && !survivor.CannotCraft
            && !survivor.IsOnExpedition
            && survivor.State != SurvivorState.Incapacitated;

        private void ReleaseWorker(string survivorId)
        {
            var survivor = FindSurvivor(survivorId);
            if (survivor != null && survivor.State == SurvivorState.Working)
                survivor.State = SurvivorState.Idle;
        }

        private string TargetDisplayName(MaintenanceTargetType targetType, string targetId)
        {
            var snapshot = _maintenance != null ? _maintenance.GetSnapshot() : null;
            if (snapshot != null && snapshot.Targets != null)
            {
                for (int i = 0; i < snapshot.Targets.Count; i++)
                {
                    var target = snapshot.Targets[i];
                    if (target != null && target.TargetType == targetType && target.TargetId == targetId)
                        return target.DisplayName;
                }
            }
            return string.IsNullOrEmpty(targetId) ? "maintenance target" : targetId;
        }

        private string WorkerDisplayName(string survivorId)
        {
            var survivor = FindSurvivor(survivorId);
            if (survivor != null && !string.IsNullOrEmpty(survivor.DisplayName)) return survivor.DisplayName;
            return string.IsNullOrEmpty(survivorId) ? "an unassigned worker" : survivorId;
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

    [Serializable]
    public sealed class RepairWorkOrderSave
    {
        public string systemId = RepairWorkOrderSystem.SystemId;
        public bool hasActiveOrder;
        public int status;
        public int targetType;
        public string targetId;
        public string assignedSurvivorId;
        public int priority;
        public float progressHours;
        public float requiredWorkHours;
        public string lastReport;
    }

    [Serializable]
    public sealed class RepairWorkOrderSnapshot
    {
        public bool HasActiveOrder;
        public RepairWorkOrderStatus Status;
        public MaintenanceTargetType TargetType;
        public string TargetId;
        public string TargetDisplayName;
        public string AssignedSurvivorId;
        public string AssignedSurvivorName;
        public MaintenanceRepairPriority Priority;
        public float ProgressHours;
        public float RequiredWorkHours;
        public string LastReport;
    }

    [Serializable]
    public sealed class RepairWorkOrderResult
    {
        public bool Succeeded;
        public bool Cancelled;
        public MaintenanceTargetType TargetType;
        public string TargetId;
        public string AssignedSurvivorId;
        public MaintenanceRepairPriority Priority;
        public float ProgressHours;
        public float RequiredWorkHours;
        public string Reason;
    }

    [Serializable]
    internal sealed class RepairWorkOrderState
    {
        public RepairWorkOrderStatus Status;
        public MaintenanceTargetType TargetType;
        public string TargetId;
        public string AssignedSurvivorId;
        public MaintenanceRepairPriority Priority;
        public float ProgressHours;
        public float RequiredWorkHours;
    }
}
