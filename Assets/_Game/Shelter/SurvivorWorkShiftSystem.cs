using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Continuous bunker duties that reserve one survivor at a time.</summary>
    public enum WorkShiftDuty
    {
        AirFiltration,
        HeaterFuel,
        WaterPurification,
        RationPreparation
    }

    /// <summary>Player-facing urgency for a suggested, never automatic, duty assignment.</summary>
    public enum WorkShiftRecommendationPriority
    {
        Standard,
        Urgent,
        Critical
    }

    /// <summary>Service reserve state for a staffed bunker duty at its current burn rate.</summary>
    public enum WorkShiftAvailabilityStatus
    {
        TelemetryUnavailable,
        Offline,
        Idle,
        Stable,
        Low,
        Critical,
        Depleted
    }

    /// <summary>
    /// Owns sustained crew assignments around existing shelter systems. It does
    /// not recreate filter, fuel, water, or ration simulation; it makes the
    /// crew commitment explicit, saveable, interruptible, and conflict-safe.
    /// </summary>
    public sealed class SurvivorWorkShiftSystem
    {
        public const string SystemId = "survivor_work_shifts";
        public const float SupervisedFilterWearMultiplier = 0.75f;
        public const float TendedHeaterFuelBurnMultiplier = 0.80f;
        public const float SupervisedPurifierHoursPerUnitMultiplier = 0.75f;
        public const float PreparedRationRestoreMultiplier = 1.10f;
        public const float RotationHours = 4f;
        public const float FatiguePerStaffedHour = 4f;
        public const float EmergencyFatigueThreshold = 85f;
        public const float RecommendationMaxFatigue = 65f;
        public const float RecommendationNeedFloor = 45f;
        public const float CriticalAirQuality = 45f;
        public const float LowFilterHealth = 50f;
        public const float CriticalIndoorTemperatureCelsius = 5f;
        public const float LowHeaterFuel = 6f;
        public const float CriticalIrradiatedWater = 3f;
        public const int UrgentPurifierQueue = 2;
        public const float CriticalCrewNeed = 35f;
        public const float LowCrewNeed = 55f;
        public const float CriticalAvailabilityHours = 4f;
        public const float LowAvailabilityHours = 12f;
        public const float CriticalRationCoverageHours = 24f;
        public const float LowRationCoverageHours = 72f;

        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;
        private readonly Func<WorkShiftDuty, bool> _isDutySupported;
        private readonly RepairWorkOrderSystem _repairWorkOrders;
        private readonly Action<Survivor, float> _applyFatigue;
        private readonly Func<WorkShiftRecommendationContext> _getRecommendationContext;
        private readonly Dictionary<WorkShiftDuty, SurvivorWorkShiftState> _shifts
            = new Dictionary<WorkShiftDuty, SurvivorWorkShiftState>();
        private readonly List<SurvivorWorkShiftRecommendationSnapshot> _recommendations
            = new List<SurvivorWorkShiftRecommendationSnapshot>();
        private string _lastReport = "No duty shifts assigned.";

        /// <summary>Raised for assignment, progress, cancellation, and restored-state changes.</summary>
        public event Action OnChanged;
        public event Action<SurvivorWorkShiftResult> OnShiftAssigned;
        public event Action<SurvivorWorkShiftResult> OnShiftCancelled;
        public event Action<SurvivorWorkShiftResult> OnShiftReliefAssigned;
        public event Action<SurvivorWorkShiftResult> OnShiftHandedOver;
        public event Action<SurvivorWorkShiftResult> OnShiftRecommendationApproved;

        public SurvivorWorkShiftSystem(
            Func<IReadOnlyList<Survivor>> getSurvivors,
            Func<WorkShiftDuty, bool> isDutySupported,
            RepairWorkOrderSystem repairWorkOrders = null,
            Action<Survivor, float> applyFatigue = null,
            Func<WorkShiftRecommendationContext> getRecommendationContext = null)
        {
            _getSurvivors = getSurvivors;
            _isDutySupported = isDutySupported;
            _repairWorkOrders = repairWorkOrders;
            _applyFatigue = applyFatigue;
            _getRecommendationContext = getRecommendationContext;
        }

        public bool TryAssign(WorkShiftDuty duty, string survivorId, out SurvivorWorkShiftResult result)
        {
            result = new SurvivorWorkShiftResult { Duty = duty, AssignedSurvivorId = survivorId };
            if (!IsKnownDuty(duty))
            {
                result.Reason = "HELD: unknown duty shift.";
                return false;
            }
            if (_shifts.TryGetValue(duty, out var staffedShift))
                return TryAssignRelief(duty, staffedShift, survivorId, out result);
            if (!IsDutySupported(duty))
            {
                result.Reason = "HELD: " + DutyDisplayName(duty) + " station is unavailable.";
                return false;
            }

            var survivor = FindSurvivor(survivorId);
            if (!CanAssignSurvivor(survivor, out var reason))
            {
                result.Reason = reason;
                return false;
            }

            var state = new SurvivorWorkShiftState
            {
                Duty = duty,
                AssignedSurvivorId = survivor.Id
            };
            _shifts.Add(duty, state);
            survivor.State = SurvivorState.Working;
            RebuildRecommendations();
            _lastReport = "ASSIGNED: " + SurvivorDisplayName(survivor) + " to " + DutyDisplayName(duty) + ".";
            result.Succeeded = true;
            result.Reason = _lastReport;
            OnShiftAssigned?.Invoke(result);
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Re-evaluate non-binding suggestions from the latest shelter and crew telemetry.</summary>
        public bool RefreshRecommendations()
        {
            bool changed = RebuildRecommendations();
            if (changed) OnChanged?.Invoke();
            return changed;
        }

        /// <summary>Accept one displayed recommendation after revalidating its duty and suggested worker.</summary>
        public bool TryApproveRecommendation(WorkShiftDuty duty, out SurvivorWorkShiftResult result)
        {
            result = new SurvivorWorkShiftResult { Duty = duty };
            RefreshRecommendations();
            var recommendation = FindRecommendation(duty);
            if (recommendation == null)
            {
                result.Reason = "HELD: no pending " + DutyDisplayName(duty) + " recommendation.";
                return false;
            }
            if (IsDutyStaffed(duty))
            {
                RefreshRecommendations();
                result.Reason = "HELD: " + DutyDisplayName(duty) + " is already staffed.";
                return false;
            }

            if (!TryAssign(duty, recommendation.SuggestedSurvivorId, out result))
            {
                RefreshRecommendations();
                return false;
            }

            _lastReport = "APPROVED: " + recommendation.SuggestedSurvivorName + " assigned to "
                + DutyDisplayName(duty) + ".";
            result.Reason = _lastReport;
            result.WasRecommendationApproved = true;
            RebuildRecommendations();
            OnShiftRecommendationApproved?.Invoke(result);
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Reserve an idle survivor as relief for an already staffed duty.</summary>
        public bool TryAssignRelief(WorkShiftDuty duty, string survivorId, out SurvivorWorkShiftResult result)
        {
            result = new SurvivorWorkShiftResult { Duty = duty, AssignedSurvivorId = survivorId };
            if (!_shifts.TryGetValue(duty, out var state))
            {
                result.Reason = "HELD: assign a primary worker before selecting relief.";
                return false;
            }
            return TryAssignRelief(duty, state, survivorId, out result);
        }

        public bool CancelShift(WorkShiftDuty duty, out SurvivorWorkShiftResult result)
        {
            result = new SurvivorWorkShiftResult { Duty = duty };
            if (!_shifts.TryGetValue(duty, out var state))
            {
                result.Reason = "HELD: no " + DutyDisplayName(duty) + " shift is assigned.";
                return false;
            }

            _shifts.Remove(duty);
            ReleaseWorker(state.AssignedSurvivorId);
            ReleaseWorker(state.ReliefSurvivorId);
            RebuildRecommendations();
            _lastReport = "CANCELLED: " + DutyDisplayName(duty) + " shift released.";
            result.Succeeded = true;
            result.Cancelled = true;
            result.AssignedSurvivorId = state.AssignedSurvivorId;
            result.HoursWorked = state.HoursWorked;
            result.Reason = _lastReport;
            OnShiftCancelled?.Invoke(result);
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Advance active staffing, fatigue, scheduled rotations, and emergency relief handovers.</summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;
            if (_shifts.Count == 0)
            {
                RefreshRecommendations();
                return;
            }

            var cancelled = new List<WorkShiftDuty>();
            bool changed = false;
            foreach (var pair in _shifts)
            {
                var state = pair.Value;
                var survivor = FindSurvivor(state.AssignedSurvivorId);
                if (!IsDutySupported(pair.Key))
                {
                    cancelled.Add(pair.Key);
                    continue;
                }
                if (!CanContinueShift(survivor))
                {
                    if (!TryHandover(pair.Key, state, emergency: true, "primary interrupted"))
                        cancelled.Add(pair.Key);
                    else
                        changed = true;
                    continue;
                }

                state.HoursWorked += gameHours;
                state.HoursSinceHandover += gameHours;
                ApplyFatigue(survivor, FatiguePerStaffedHour * gameHours);
                changed = true;

                if (IsEmergencyFatigued(survivor))
                {
                    if (!TryHandover(pair.Key, state, emergency: true, "critical fatigue"))
                        cancelled.Add(pair.Key);
                }
                else if (state.HoursSinceHandover >= RotationHours
                    && !string.IsNullOrEmpty(state.ReliefSurvivorId))
                {
                    if (!TryHandover(pair.Key, state, emergency: false, "rotation due"))
                    {
                        ReleaseWorker(state.ReliefSurvivorId);
                        state.ReliefSurvivorId = null;
                        _lastReport = "RELIEF LOST: " + DutyDisplayName(pair.Key)
                            + " remains with " + SurvivorDisplayName(survivor) + ".";
                    }
                }
            }

            for (int i = 0; i < cancelled.Count; i++)
                CancelInvalidShift(cancelled[i]);
            bool recommendationsChanged = RebuildRecommendations();
            if ((changed && cancelled.Count == 0) || recommendationsChanged)
                OnChanged?.Invoke();
        }

        public bool TryGetShiftForSurvivor(string survivorId, out SurvivorWorkShiftSlotSnapshot slot)
        {
            slot = null;
            if (string.IsNullOrEmpty(survivorId)) return false;
            foreach (var pair in _shifts)
            {
                bool primary = string.Equals(pair.Value.AssignedSurvivorId, survivorId, StringComparison.Ordinal);
                bool relief = string.Equals(pair.Value.ReliefSurvivorId, survivorId, StringComparison.Ordinal);
                if (!primary && !relief) continue;
                slot = BuildSlot(pair.Key, pair.Value,
                    _getRecommendationContext != null ? _getRecommendationContext() : null);
                slot.IsReliefAssignment = relief;
                return true;
            }
            return false;
        }

        public SurvivorWorkShiftSnapshot GetSnapshot()
        {
            var context = _getRecommendationContext != null ? _getRecommendationContext() : null;
            var snapshot = new SurvivorWorkShiftSnapshot
            {
                Slots = new List<SurvivorWorkShiftSlotSnapshot>(),
                Recommendations = CloneRecommendations(),
                LastReport = _lastReport
            };
            foreach (WorkShiftDuty duty in Enum.GetValues(typeof(WorkShiftDuty)))
            {
                _shifts.TryGetValue(duty, out var state);
                snapshot.Slots.Add(BuildSlot(duty, state, context));
            }
            return snapshot;
        }

        /// <summary>
        /// Returns the live, bounded shift effects. They are derived entirely
        /// from active staffing, so restored assignments immediately recreate
        /// the same modifiers without a second saved source of truth.
        /// </summary>
        public SurvivorWorkShiftEffectsSnapshot GetEffectsSnapshot()
        {
            return new SurvivorWorkShiftEffectsSnapshot
            {
                FilterWearMultiplier = IsDutyStaffed(WorkShiftDuty.AirFiltration)
                    ? SupervisedFilterWearMultiplier
                    : 1f,
                HeaterFuelBurnMultiplier = IsDutyStaffed(WorkShiftDuty.HeaterFuel)
                    ? TendedHeaterFuelBurnMultiplier
                    : 1f,
                PurifierHoursPerUnitMultiplier = IsDutyStaffed(WorkShiftDuty.WaterPurification)
                    ? SupervisedPurifierHoursPerUnitMultiplier
                    : 1f,
                RationRestoreMultiplier = IsDutyStaffed(WorkShiftDuty.RationPreparation)
                    ? PreparedRationRestoreMultiplier
                    : 1f
            };
        }

        /// <summary>Module-tick adapter used by Shelter; only climate loads are affected here.</summary>
        public float GetModuleResourceConsumptionMultiplier(ShelterModuleInstance module)
        {
            if (module == null) return 1f;
            if (string.Equals(module.ModuleId, AirHeatManagementSystem.AirFiltrationModuleId, StringComparison.Ordinal))
                return GetEffectsSnapshot().FilterWearMultiplier;
            if (string.Equals(module.ModuleId, AirHeatManagementSystem.HeaterModuleId, StringComparison.Ordinal))
                return GetEffectsSnapshot().HeaterFuelBurnMultiplier;
            return 1f;
        }

        public SurvivorWorkShiftSave CaptureState()
        {
            var save = new SurvivorWorkShiftSave
            {
                systemId = SystemId,
                lastReport = _lastReport,
                shifts = new List<SurvivorWorkShiftSaveEntry>(),
                recommendations = new List<SurvivorWorkShiftRecommendationSaveEntry>()
            };
            foreach (var pair in _shifts)
            {
                save.shifts.Add(new SurvivorWorkShiftSaveEntry
                {
                    duty = (int)pair.Key,
                    assignedSurvivorId = pair.Value.AssignedSurvivorId,
                    reliefSurvivorId = pair.Value.ReliefSurvivorId,
                    hoursWorked = pair.Value.HoursWorked,
                    hoursSinceHandover = pair.Value.HoursSinceHandover,
                    handoverCount = pair.Value.HandoverCount
                });
            }
            for (int i = 0; i < _recommendations.Count; i++)
            {
                var recommendation = _recommendations[i];
                save.recommendations.Add(new SurvivorWorkShiftRecommendationSaveEntry
                {
                    duty = (int)recommendation.Duty,
                    suggestedSurvivorId = recommendation.SuggestedSurvivorId,
                    priority = (int)recommendation.Priority,
                    reason = recommendation.Reason
                });
            }
            return save;
        }

        public void RestoreState(SurvivorWorkShiftSave save)
        {
            ReleaseAllWorkers();
            _shifts.Clear();
            _recommendations.Clear();
            _lastReport = save != null && !string.IsNullOrEmpty(save.lastReport)
                ? save.lastReport
                : "No duty shifts assigned.";
            if (save != null && save.shifts != null)
            {
                for (int i = 0; i < save.shifts.Count; i++)
                {
                    var entry = save.shifts[i];
                    var duty = (WorkShiftDuty)entry.duty;
                    if (!IsKnownDuty(duty) || _shifts.ContainsKey(duty) || !IsDutySupported(duty)) continue;
                    var survivor = FindSurvivor(entry.assignedSurvivorId);
                    if (survivor == null || !survivor.IsAlive || survivor.IsOnExpedition || survivor.CannotCraft) continue;
                    if (HasRepairReservation(survivor.Id) || IsAssignedElsewhere(survivor.Id)) continue;

                    survivor.State = SurvivorState.Working;
                    var state = new SurvivorWorkShiftState
                    {
                        Duty = duty,
                        AssignedSurvivorId = survivor.Id,
                        HoursWorked = Mathf.Max(0f, entry.hoursWorked),
                        HoursSinceHandover = Mathf.Max(0f, entry.hoursSinceHandover),
                        HandoverCount = Mathf.Max(0, entry.handoverCount)
                    };
                    _shifts.Add(duty, state);

                    var relief = FindSurvivor(entry.reliefSurvivorId);
                    if (relief == null || !relief.IsAlive || relief.IsOnExpedition || relief.CannotCraft
                        || string.Equals(relief.Id, survivor.Id, StringComparison.Ordinal)
                        || HasRepairReservation(relief.Id) || IsAssignedElsewhere(relief.Id)) continue;
                    relief.State = SurvivorState.Working;
                    state.ReliefSurvivorId = relief.Id;
                }
            }
            RestoreRecommendations(save);
            OnChanged?.Invoke();
        }

        public static string DutyDisplayName(WorkShiftDuty duty)
        {
            switch (duty)
            {
                case WorkShiftDuty.AirFiltration: return "air filtration";
                case WorkShiftDuty.HeaterFuel: return "heater fuel";
                case WorkShiftDuty.WaterPurification: return "water purification";
                case WorkShiftDuty.RationPreparation: return "ration preparation";
                default: return "duty shift";
            }
        }

        public static string DutyEffectSummary(WorkShiftDuty duty)
        {
            switch (duty)
            {
                case WorkShiftDuty.AirFiltration: return "25% less filter wear";
                case WorkShiftDuty.HeaterFuel: return "20% less fuel burn";
                case WorkShiftDuty.WaterPurification: return "25% faster purifier";
                case WorkShiftDuty.RationPreparation: return "10% stronger rations";
                default: return "no effect";
            }
        }

        private bool RebuildRecommendations()
        {
            var context = _getRecommendationContext != null ? _getRecommendationContext() : null;
            if (context == null) return false;

            var next = BuildRecommendations(context);
            if (RecommendationsEqual(_recommendations, next)) return false;
            _recommendations.Clear();
            _recommendations.AddRange(next);
            return true;
        }

        private List<SurvivorWorkShiftRecommendationSnapshot> BuildRecommendations(
            WorkShiftRecommendationContext context)
        {
            var recommendations = new List<SurvivorWorkShiftRecommendationSnapshot>();
            TryAddRecommendation(
                recommendations,
                context.FilterOperational && context.AirQuality <= CriticalAirQuality,
                WorkShiftDuty.AirFiltration,
                WorkShiftRecommendationPriority.Critical,
                "Air quality is becoming unsafe.",
                context);
            TryAddRecommendation(
                recommendations,
                context.FilterOperational && context.FilterHealth <= LowFilterHealth,
                WorkShiftDuty.AirFiltration,
                WorkShiftRecommendationPriority.Urgent,
                "Filter health is low.",
                context);
            TryAddRecommendation(
                recommendations,
                context.HeaterOperational && context.IndoorTemperatureCelsius <= CriticalIndoorTemperatureCelsius,
                WorkShiftDuty.HeaterFuel,
                WorkShiftRecommendationPriority.Critical,
                "The bunker is dangerously cold.",
                context);
            TryAddRecommendation(
                recommendations,
                context.HeaterOperational && context.HeaterFuel <= LowHeaterFuel,
                WorkShiftDuty.HeaterFuel,
                WorkShiftRecommendationPriority.Urgent,
                "Heater fuel is running low.",
                context);
            TryAddRecommendation(
                recommendations,
                context.PurifierOperational && context.IrradiatedWater >= CriticalIrradiatedWater,
                WorkShiftDuty.WaterPurification,
                WorkShiftRecommendationPriority.Critical,
                "Irradiated water is waiting for treatment.",
                context);
            TryAddRecommendation(
                recommendations,
                context.PurifierOperational && context.PurifierUnitsQueued >= UrgentPurifierQueue,
                WorkShiftDuty.WaterPurification,
                WorkShiftRecommendationPriority.Urgent,
                "The purifier queue is growing.",
                context);

            float lowestCrewNeed = GetLowestCrewNeed();
            bool rationSupplyAvailable = context.ProjectedFoodCoverage > 0f || context.ProjectedWaterCoverage > 0f;
            TryAddRecommendation(
                recommendations,
                rationSupplyAvailable && lowestCrewNeed <= CriticalCrewNeed,
                WorkShiftDuty.RationPreparation,
                WorkShiftRecommendationPriority.Critical,
                "Crew hunger or thirst is critical.",
                context);
            TryAddRecommendation(
                recommendations,
                rationSupplyAvailable && lowestCrewNeed <= LowCrewNeed,
                WorkShiftDuty.RationPreparation,
                WorkShiftRecommendationPriority.Urgent,
                "Crew hunger or thirst is slipping.",
                context);

            recommendations.Sort((left, right) =>
            {
                int priority = right.Priority.CompareTo(left.Priority);
                return priority != 0 ? priority : left.Duty.CompareTo(right.Duty);
            });
            return recommendations;
        }

        private void TryAddRecommendation(
            List<SurvivorWorkShiftRecommendationSnapshot> recommendations,
            bool condition,
            WorkShiftDuty duty,
            WorkShiftRecommendationPriority priority,
            string reason,
            WorkShiftRecommendationContext context)
        {
            if (!condition || IsDutyStaffed(duty) || !IsDutySupported(duty)
                || FindRecommendation(recommendations, duty) != null) return;

            var survivor = FindBestRecommendationWorker();
            if (survivor == null) return;
            recommendations.Add(new SurvivorWorkShiftRecommendationSnapshot
            {
                Duty = duty,
                SuggestedSurvivorId = survivor.Id,
                SuggestedSurvivorName = SurvivorDisplayName(survivor),
                Priority = priority,
                Reason = AppendForecastToRecommendation(reason, BuildAvailabilityForecast(duty, context, false))
            });
        }

        private Survivor FindBestRecommendationWorker()
        {
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors == null) return null;

            Survivor best = null;
            float bestStrain = float.MaxValue;
            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (!CanAssignSurvivor(survivor, out _) || !IsRecommendationReady(survivor)) continue;
                float strain = GetRecommendationStrain(survivor);
                if (best == null || strain < bestStrain
                    || (Mathf.Approximately(strain, bestStrain)
                        && string.CompareOrdinal(survivor.Id, best.Id) < 0))
                {
                    best = survivor;
                    bestStrain = strain;
                }
            }
            return best;
        }

        private static bool IsRecommendationReady(Survivor survivor)
        {
            if (survivor == null || survivor.Needs == null) return false;
            return survivor.Needs.Fatigue < RecommendationMaxFatigue
                && survivor.Needs.Hunger >= RecommendationNeedFloor
                && survivor.Needs.Thirst >= RecommendationNeedFloor
                && survivor.Needs.Warmth >= RecommendationNeedFloor
                && survivor.Needs.Health >= RecommendationNeedFloor
                && survivor.Needs.Morale >= RecommendationNeedFloor;
        }

        private static float GetRecommendationStrain(Survivor survivor)
        {
            var needs = survivor.Needs;
            return needs.Fatigue * 1.5f
                + (100f - needs.Hunger) * 0.25f
                + (100f - needs.Thirst) * 0.25f
                + (100f - needs.Warmth) * 0.25f
                + (100f - needs.Health) * 0.25f
                + (100f - needs.Morale) * 0.10f;
        }

        private float GetLowestCrewNeed()
        {
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            float lowest = 100f;
            if (survivors == null) return lowest;
            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (survivor == null || !survivor.IsAlive || survivor.Needs == null) continue;
                lowest = Mathf.Min(lowest, survivor.Needs.Hunger, survivor.Needs.Thirst);
            }
            return lowest;
        }

        private SurvivorWorkShiftRecommendationSnapshot FindRecommendation(WorkShiftDuty duty)
        {
            return FindRecommendation(_recommendations, duty);
        }

        private static SurvivorWorkShiftRecommendationSnapshot FindRecommendation(
            List<SurvivorWorkShiftRecommendationSnapshot> recommendations,
            WorkShiftDuty duty)
        {
            if (recommendations == null) return null;
            for (int i = 0; i < recommendations.Count; i++)
                if (recommendations[i] != null && recommendations[i].Duty == duty) return recommendations[i];
            return null;
        }

        private static bool RecommendationsEqual(
            List<SurvivorWorkShiftRecommendationSnapshot> left,
            List<SurvivorWorkShiftRecommendationSnapshot> right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left == null || right == null || left.Count != right.Count) return false;
            for (int i = 0; i < left.Count; i++)
            {
                var a = left[i];
                var b = right[i];
                if (a == null || b == null)
                {
                    if (!ReferenceEquals(a, b)) return false;
                    continue;
                }
                if (a.Duty != b.Duty || a.Priority != b.Priority
                    || !string.Equals(a.SuggestedSurvivorId, b.SuggestedSurvivorId, StringComparison.Ordinal)
                    || !string.Equals(a.Reason, b.Reason, StringComparison.Ordinal)) return false;
            }
            return true;
        }

        private List<SurvivorWorkShiftRecommendationSnapshot> CloneRecommendations()
        {
            var clone = new List<SurvivorWorkShiftRecommendationSnapshot>();
            for (int i = 0; i < _recommendations.Count; i++)
            {
                var recommendation = _recommendations[i];
                if (recommendation == null) continue;
                clone.Add(new SurvivorWorkShiftRecommendationSnapshot
                {
                    Duty = recommendation.Duty,
                    SuggestedSurvivorId = recommendation.SuggestedSurvivorId,
                    SuggestedSurvivorName = recommendation.SuggestedSurvivorName,
                    Priority = recommendation.Priority,
                    Reason = recommendation.Reason
                });
            }
            return clone;
        }

        private void RestoreRecommendations(SurvivorWorkShiftSave save)
        {
            if (save == null || save.recommendations == null) return;
            for (int i = 0; i < save.recommendations.Count; i++)
            {
                var entry = save.recommendations[i];
                var duty = (WorkShiftDuty)entry.duty;
                if (!IsKnownDuty(duty) || IsDutyStaffed(duty) || !IsDutySupported(duty)
                    || FindRecommendation(duty) != null) continue;
                var survivor = FindSurvivor(entry.suggestedSurvivorId);
                if (!CanAssignSurvivor(survivor, out _) || !IsRecommendationReady(survivor)) continue;
                _recommendations.Add(new SurvivorWorkShiftRecommendationSnapshot
                {
                    Duty = duty,
                    SuggestedSurvivorId = survivor.Id,
                    SuggestedSurvivorName = SurvivorDisplayName(survivor),
                    Priority = (WorkShiftRecommendationPriority)Mathf.Clamp(
                        entry.priority,
                        (int)WorkShiftRecommendationPriority.Standard,
                        (int)WorkShiftRecommendationPriority.Critical),
                    Reason = string.IsNullOrEmpty(entry.reason)
                        ? "Duty staffing is recommended."
                        : entry.reason
                });
            }
            _recommendations.Sort((left, right) =>
            {
                int priority = right.Priority.CompareTo(left.Priority);
                return priority != 0 ? priority : left.Duty.CompareTo(right.Duty);
            });
        }

        private static string AppendForecastToRecommendation(
            string reason,
            WorkShiftAvailabilityForecast forecast)
        {
            if (forecast == null || forecast.Status == WorkShiftAvailabilityStatus.TelemetryUnavailable)
                return reason;
            return string.IsNullOrEmpty(reason) ? forecast.Summary : reason + " " + forecast.Summary;
        }

        private static WorkShiftAvailabilityForecast BuildAvailabilityForecast(
            WorkShiftDuty duty,
            WorkShiftRecommendationContext context,
            bool isStaffed)
        {
            if (context == null)
            {
                return new WorkShiftAvailabilityForecast
                {
                    Duty = duty,
                    IsStaffed = isStaffed,
                    Status = WorkShiftAvailabilityStatus.TelemetryUnavailable,
                    Summary = "Service forecast unavailable."
                };
            }

            switch (duty)
            {
                case WorkShiftDuty.AirFiltration:
                    return BuildHourlyAvailability(
                        duty,
                        isStaffed,
                        context.FilterOperational,
                        context.FilterRuntimeHours,
                        context.FilterBurnPerHour);
                case WorkShiftDuty.HeaterFuel:
                    return BuildHourlyAvailability(
                        duty,
                        isStaffed,
                        context.HeaterOperational,
                        context.HeaterRuntimeHours,
                        context.HeaterBurnPerHour);
                case WorkShiftDuty.WaterPurification:
                    return BuildHourlyAvailability(
                        duty,
                        isStaffed,
                        context.PurifierOperational,
                        context.PurifierRuntimeHours,
                        context.PurifierFilterBurnPerHour);
                case WorkShiftDuty.RationPreparation:
                    return BuildRationAvailability(context, isStaffed);
                default:
                    return new WorkShiftAvailabilityForecast
                    {
                        Duty = duty,
                        IsStaffed = isStaffed,
                        Status = WorkShiftAvailabilityStatus.TelemetryUnavailable,
                        Summary = "Service forecast unavailable."
                    };
            }
        }

        private static WorkShiftAvailabilityForecast BuildHourlyAvailability(
            WorkShiftDuty duty,
            bool isStaffed,
            bool isStationOperational,
            float remainingHours,
            float burnPerHour)
        {
            var forecast = new WorkShiftAvailabilityForecast
            {
                Duty = duty,
                IsStaffed = isStaffed,
                IsStationOperational = isStationOperational,
                RemainingHours = remainingHours,
                CurrentBurnPerHour = Mathf.Max(0f, burnPerHour)
            };
            if (!isStationOperational)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Offline;
                forecast.Summary = "Station is offline.";
                return forecast;
            }
            if (remainingHours < 0f)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Idle;
                forecast.Summary = "No current resource burn.";
                return forecast;
            }
            if (remainingHours <= 0f)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Depleted;
                forecast.Summary = "No safe service time remains.";
                return forecast;
            }

            string duration = remainingHours.ToString("0.0") + "h";
            string burn = forecast.CurrentBurnPerHour.ToString("0.0") + "/h";
            if (remainingHours <= CriticalAvailabilityHours)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Critical;
                forecast.Summary = "Only " + duration + " of safe service remains at " + burn + ".";
            }
            else if (remainingHours <= LowAvailabilityHours)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Low;
                forecast.Summary = "Safe for " + duration + " at " + burn + "; reserve is low.";
            }
            else
            {
                forecast.Status = WorkShiftAvailabilityStatus.Stable;
                forecast.Summary = "Safe for " + duration + " at " + burn + ".";
            }
            return forecast;
        }

        private static WorkShiftAvailabilityForecast BuildRationAvailability(
            WorkShiftRecommendationContext context,
            bool isStaffed)
        {
            var forecast = new WorkShiftAvailabilityForecast
            {
                Duty = WorkShiftDuty.RationPreparation,
                IsStaffed = isStaffed,
                IsStationOperational = context.RationOperational,
                FoodDaysRemaining = context.FoodDaysRemaining,
                WaterDaysRemaining = context.WaterDaysRemaining,
                CurrentFoodUnitsPerDay = Mathf.Max(0, context.FoodUnitsPerDay),
                CurrentWaterUnitsPerDay = Mathf.Max(0, context.WaterUnitsPerDay)
            };
            if (!forecast.IsStationOperational)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Offline;
                forecast.Summary = "Ration service is offline.";
                return forecast;
            }
            if (forecast.FoodDaysRemaining < 0f || forecast.WaterDaysRemaining < 0f)
            {
                forecast.Status = WorkShiftAvailabilityStatus.TelemetryUnavailable;
                forecast.Summary = "Ration forecast unavailable.";
                return forecast;
            }
            if (forecast.CurrentFoodUnitsPerDay <= 0 && forecast.CurrentWaterUnitsPerDay <= 0)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Idle;
                forecast.Summary = "No current ration demand.";
                return forecast;
            }

            forecast.RemainingHours = Mathf.Min(forecast.FoodDaysRemaining, forecast.WaterDaysRemaining) * 24f;
            string food = forecast.FoodDaysRemaining.ToString("0.0") + "d food";
            string water = forecast.WaterDaysRemaining.ToString("0.0") + "d water";
            if (forecast.RemainingHours <= 0f)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Depleted;
                forecast.Summary = "No full ration coverage remains (" + food + " · " + water + ").";
            }
            else if (forecast.RemainingHours <= CriticalRationCoverageHours)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Critical;
                forecast.Summary = "Only " + (forecast.RemainingHours / 24f).ToString("0.0")
                    + "d of ration coverage remains (" + food + " · " + water + ").";
            }
            else if (forecast.RemainingHours <= LowRationCoverageHours)
            {
                forecast.Status = WorkShiftAvailabilityStatus.Low;
                forecast.Summary = "Rations cover " + (forecast.RemainingHours / 24f).ToString("0.0")
                    + "d at current issue (" + food + " · " + water + ").";
            }
            else
            {
                forecast.Status = WorkShiftAvailabilityStatus.Stable;
                forecast.Summary = "Rations cover " + (forecast.RemainingHours / 24f).ToString("0.0")
                    + "d at current issue (" + food + " · " + water + ").";
            }
            return forecast;
        }

        private SurvivorWorkShiftSlotSnapshot BuildSlot(
            WorkShiftDuty duty,
            SurvivorWorkShiftState state,
            WorkShiftRecommendationContext context = null)
        {
            var survivor = state != null ? FindSurvivor(state.AssignedSurvivorId) : null;
            var relief = state != null ? FindSurvivor(state.ReliefSurvivorId) : null;
            return new SurvivorWorkShiftSlotSnapshot
            {
                Duty = duty,
                DisplayName = DutyDisplayName(duty),
                IsSupported = IsDutySupported(duty),
                AssignedSurvivorId = state != null ? state.AssignedSurvivorId : null,
                AssignedSurvivorName = survivor != null ? SurvivorDisplayName(survivor) : null,
                ReliefSurvivorId = state != null ? state.ReliefSurvivorId : null,
                ReliefSurvivorName = relief != null ? SurvivorDisplayName(relief) : null,
                HoursWorked = state != null ? state.HoursWorked : 0f,
                HoursSinceHandover = state != null ? state.HoursSinceHandover : 0f,
                RotationHours = RotationHours,
                HandoverCount = state != null ? state.HandoverCount : 0,
                EffectSummary = DutyEffectSummary(duty),
                EffectMultiplier = GetDutyEffectMultiplier(duty),
                Availability = BuildAvailabilityForecast(
                    duty,
                    context ?? (_getRecommendationContext != null ? _getRecommendationContext() : null),
                    state != null && !string.IsNullOrEmpty(state.AssignedSurvivorId))
            };
        }

        private bool IsDutyStaffed(WorkShiftDuty duty)
        {
            return _shifts.TryGetValue(duty, out var state)
                && state != null
                && !string.IsNullOrEmpty(state.AssignedSurvivorId);
        }

        private static float GetDutyEffectMultiplier(WorkShiftDuty duty)
        {
            switch (duty)
            {
                case WorkShiftDuty.AirFiltration: return SupervisedFilterWearMultiplier;
                case WorkShiftDuty.HeaterFuel: return TendedHeaterFuelBurnMultiplier;
                case WorkShiftDuty.WaterPurification: return SupervisedPurifierHoursPerUnitMultiplier;
                case WorkShiftDuty.RationPreparation: return PreparedRationRestoreMultiplier;
                default: return 1f;
            }
        }

        private bool CanAssignSurvivor(Survivor survivor, out string reason)
        {
            if (survivor == null || !survivor.IsAlive)
            {
                reason = "HELD: no living survivor selected.";
                return false;
            }
            if (survivor.IsOnExpedition)
            {
                reason = "HELD: " + SurvivorDisplayName(survivor) + " is already on expedition.";
                return false;
            }
            if (survivor.CannotCraft || survivor.State == SurvivorState.Incapacitated)
            {
                reason = "HELD: " + SurvivorDisplayName(survivor) + " cannot take a duty shift.";
                return false;
            }
            if (IsEmergencyFatigued(survivor))
            {
                reason = "HELD: " + SurvivorDisplayName(survivor) + " is too exhausted for a shift.";
                return false;
            }
            if (HasRepairReservation(survivor.Id))
            {
                reason = "HELD: " + SurvivorDisplayName(survivor) + " is committed to bunker repair.";
                return false;
            }
            if (IsAssignedElsewhere(survivor.Id) || survivor.State != SurvivorState.Idle)
            {
                reason = "HELD: " + SurvivorDisplayName(survivor) + " is assigned elsewhere.";
                return false;
            }

            reason = null;
            return true;
        }

        private bool IsDutySupported(WorkShiftDuty duty) => _isDutySupported != null && _isDutySupported(duty);

        private bool HasRepairReservation(string survivorId)
        {
            var repair = _repairWorkOrders != null ? _repairWorkOrders.GetSnapshot() : null;
            return repair != null && repair.HasActiveOrder
                && string.Equals(repair.AssignedSurvivorId, survivorId, StringComparison.Ordinal);
        }

        private bool IsAssignedElsewhere(string survivorId)
        {
            return TryGetShiftForSurvivor(survivorId, out _);
        }

        private void CancelInvalidShift(WorkShiftDuty duty)
        {
            if (!_shifts.TryGetValue(duty, out var state)) return;
            _shifts.Remove(duty);
            ReleaseWorker(state.AssignedSurvivorId);
            ReleaseWorker(state.ReliefSurvivorId);
            _lastReport = "CANCELLED: " + DutyDisplayName(duty) + " shift was interrupted.";
            var result = new SurvivorWorkShiftResult
            {
                Succeeded = true,
                Cancelled = true,
                Duty = duty,
                AssignedSurvivorId = state.AssignedSurvivorId,
                HoursWorked = state.HoursWorked,
                Reason = _lastReport
            };
            OnShiftCancelled?.Invoke(result);
            OnChanged?.Invoke();
        }

        private void ReleaseAllWorkers()
        {
            foreach (var pair in _shifts)
            {
                ReleaseWorker(pair.Value.AssignedSurvivorId);
                ReleaseWorker(pair.Value.ReliefSurvivorId);
            }
        }

        private bool TryAssignRelief(
            WorkShiftDuty duty,
            SurvivorWorkShiftState state,
            string survivorId,
            out SurvivorWorkShiftResult result)
        {
            result = new SurvivorWorkShiftResult { Duty = duty, AssignedSurvivorId = survivorId };
            if (!IsDutySupported(duty))
            {
                result.Reason = "HELD: " + DutyDisplayName(duty) + " station is unavailable.";
                return false;
            }
            if (state == null || string.IsNullOrEmpty(state.AssignedSurvivorId))
            {
                result.Reason = "HELD: assign a primary worker before selecting relief.";
                return false;
            }
            if (!string.IsNullOrEmpty(state.ReliefSurvivorId))
            {
                result.Reason = "HELD: " + DutyDisplayName(duty) + " already has relief assigned.";
                return false;
            }
            if (string.Equals(state.AssignedSurvivorId, survivorId, StringComparison.Ordinal))
            {
                result.Reason = "HELD: the primary worker cannot relieve their own shift.";
                return false;
            }

            var survivor = FindSurvivor(survivorId);
            if (!CanAssignSurvivor(survivor, out var reason))
            {
                result.Reason = reason;
                return false;
            }

            state.ReliefSurvivorId = survivor.Id;
            survivor.State = SurvivorState.Working;
            _lastReport = "RELIEF ASSIGNED: " + SurvivorDisplayName(survivor)
                + " stands by for " + DutyDisplayName(duty) + ".";
            result.Succeeded = true;
            result.IsReliefAssignment = true;
            result.ReliefSurvivorId = survivor.Id;
            result.Reason = _lastReport;
            OnShiftReliefAssigned?.Invoke(result);
            OnChanged?.Invoke();
            return true;
        }

        private bool TryHandover(WorkShiftDuty duty, SurvivorWorkShiftState state, bool emergency, string cause)
        {
            var relief = FindSurvivor(state != null ? state.ReliefSurvivorId : null);
            if (!CanContinueShift(relief) || IsEmergencyFatigued(relief)) return false;

            string outgoingId = state.AssignedSurvivorId;
            var outgoing = FindSurvivor(outgoingId);
            ReleaseWorker(outgoingId);
            state.AssignedSurvivorId = relief.Id;
            state.ReliefSurvivorId = null;
            state.HoursSinceHandover = 0f;
            state.HandoverCount++;
            relief.State = SurvivorState.Working;
            _lastReport = (emergency ? "EMERGENCY HANDOVER: " : "ROTATION: ")
                + SurvivorDisplayName(outgoing) + " relieved by " + SurvivorDisplayName(relief)
                + " on " + DutyDisplayName(duty) + " (" + cause + ").";
            var result = new SurvivorWorkShiftResult
            {
                Succeeded = true,
                Duty = duty,
                AssignedSurvivorId = relief.Id,
                PreviousSurvivorId = outgoingId,
                WasEmergencyHandover = emergency,
                HoursWorked = state.HoursWorked,
                Reason = _lastReport
            };
            OnShiftHandedOver?.Invoke(result);
            return true;
        }

        private bool CanContinueShift(Survivor survivor)
        {
            return survivor != null && survivor.IsAlive && !survivor.IsOnExpedition
                && !survivor.CannotCraft && survivor.State == SurvivorState.Working;
        }

        private static bool IsEmergencyFatigued(Survivor survivor)
        {
            return survivor != null && survivor.Needs != null
                && survivor.Needs.Fatigue >= EmergencyFatigueThreshold;
        }

        private void ApplyFatigue(Survivor survivor, float fatigue)
        {
            if (survivor == null || fatigue <= 0f) return;
            if (_applyFatigue != null)
            {
                _applyFatigue(survivor, fatigue);
                return;
            }
            if (survivor.Needs != null)
                survivor.Needs.Fatigue = Mathf.Clamp(survivor.Needs.Fatigue + fatigue, 0f, 100f);
        }

        private void ReleaseWorker(string survivorId)
        {
            var survivor = FindSurvivor(survivorId);
            if (survivor != null && survivor.State == SurvivorState.Working)
                survivor.State = SurvivorState.Idle;
        }

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

        private static string SurvivorDisplayName(Survivor survivor)
        {
            return survivor != null && !string.IsNullOrEmpty(survivor.DisplayName)
                ? survivor.DisplayName
                : (survivor != null ? survivor.Id : "operative");
        }

        private static bool IsKnownDuty(WorkShiftDuty duty)
        {
            return duty >= WorkShiftDuty.AirFiltration && duty <= WorkShiftDuty.RationPreparation;
        }
    }

    [Serializable]
    public sealed class SurvivorWorkShiftSave
    {
        public string systemId = SurvivorWorkShiftSystem.SystemId;
        public string lastReport;
        public List<SurvivorWorkShiftSaveEntry> shifts = new List<SurvivorWorkShiftSaveEntry>();
        public List<SurvivorWorkShiftRecommendationSaveEntry> recommendations
            = new List<SurvivorWorkShiftRecommendationSaveEntry>();
    }

    [Serializable]
    public sealed class SurvivorWorkShiftSaveEntry
    {
        public int duty;
        public string assignedSurvivorId;
        public string reliefSurvivorId;
        public float hoursWorked;
        public float hoursSinceHandover;
        public int handoverCount;
    }

    [Serializable]
    public sealed class SurvivorWorkShiftRecommendationSaveEntry
    {
        public int duty;
        public string suggestedSurvivorId;
        public int priority;
        public string reason;
    }

    [Serializable]
    public sealed class SurvivorWorkShiftSnapshot
    {
        public List<SurvivorWorkShiftSlotSnapshot> Slots;
        public List<SurvivorWorkShiftRecommendationSnapshot> Recommendations;
        public string LastReport;
    }

    /// <summary>Detached live telemetry supplied by Core; recommendations never own these values.</summary>
    [Serializable]
    public sealed class WorkShiftRecommendationContext
    {
        public bool FilterOperational;
        public float AirQuality;
        public float FilterHealth;
        public float FilterBurnPerHour;
        public float FilterRuntimeHours = -1f;
        public bool HeaterOperational;
        public float IndoorTemperatureCelsius;
        public float HeaterFuel;
        public float HeaterBurnPerHour;
        public float HeaterRuntimeHours = -1f;
        public bool PurifierOperational;
        public float IrradiatedWater;
        public int PurifierUnitsQueued;
        public float PurifierFilterBurnPerHour;
        public float PurifierRuntimeHours = -1f;
        public bool RationOperational;
        public float ProjectedFoodCoverage;
        public float ProjectedWaterCoverage;
        public float FoodDaysRemaining = -1f;
        public float WaterDaysRemaining = -1f;
        public int FoodUnitsPerDay;
        public int WaterUnitsPerDay;
    }

    [Serializable]
    public sealed class SurvivorWorkShiftRecommendationSnapshot
    {
        public WorkShiftDuty Duty;
        public string SuggestedSurvivorId;
        public string SuggestedSurvivorName;
        public WorkShiftRecommendationPriority Priority;
        public string Reason;
    }

    /// <summary>Derived display data; resource ownership remains with the station systems.</summary>
    [Serializable]
    public sealed class WorkShiftAvailabilityForecast
    {
        public WorkShiftDuty Duty;
        public bool IsStaffed;
        public bool IsStationOperational;
        public WorkShiftAvailabilityStatus Status;
        public float RemainingHours = -1f;
        public float CurrentBurnPerHour;
        public float FoodDaysRemaining = -1f;
        public float WaterDaysRemaining = -1f;
        public int CurrentFoodUnitsPerDay;
        public int CurrentWaterUnitsPerDay;
        public string Summary;
    }

    [Serializable]
    public sealed class SurvivorWorkShiftSlotSnapshot
    {
        public WorkShiftDuty Duty;
        public string DisplayName;
        public bool IsSupported;
        public string AssignedSurvivorId;
        public string AssignedSurvivorName;
        public string ReliefSurvivorId;
        public string ReliefSurvivorName;
        public float HoursWorked;
        public float HoursSinceHandover;
        public float RotationHours;
        public int HandoverCount;
        public bool IsReliefAssignment;
        public string EffectSummary;
        public float EffectMultiplier;
        public WorkShiftAvailabilityForecast Availability;
    }

    /// <summary>Derived, presentation-safe modifiers granted by currently staffed duties.</summary>
    [Serializable]
    public sealed class SurvivorWorkShiftEffectsSnapshot
    {
        public float FilterWearMultiplier = 1f;
        public float HeaterFuelBurnMultiplier = 1f;
        public float PurifierHoursPerUnitMultiplier = 1f;
        public float RationRestoreMultiplier = 1f;
    }

    [Serializable]
    public sealed class SurvivorWorkShiftResult
    {
        public bool Succeeded;
        public bool Cancelled;
        public WorkShiftDuty Duty;
        public string AssignedSurvivorId;
        public string ReliefSurvivorId;
        public string PreviousSurvivorId;
        public bool IsReliefAssignment;
        public bool WasEmergencyHandover;
        public bool WasRecommendationApproved;
        public float HoursWorked;
        public string Reason;
    }

    [Serializable]
    internal sealed class SurvivorWorkShiftState
    {
        public WorkShiftDuty Duty;
        public string AssignedSurvivorId;
        public string ReliefSurvivorId;
        public float HoursWorked;
        public float HoursSinceHandover;
        public int HandoverCount;
    }
}
