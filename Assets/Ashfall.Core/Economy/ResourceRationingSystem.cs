// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Economy
{
    public enum RationingTier
    {
        Full = 0,
        ThreeQuarter = 1,
        Half = 2,
        Third = 3,
        Quarter = 4,
        Minimal = 5,
        None = 6
    }

    public enum ResourceCrisisType
    {
        FoodShortage = 0,
        WaterShortage = 1,
        MedicalShortage = 2,
        FuelShortage = 3,
        GeneralScarcity = 4,
        MultipleShortage = 5
    }

    public enum PriorityGroupTier
    {
        Critical = 0, // Medics, essential engineers, children
        High = 1,     // Frontline workers, guards
        Standard = 2, // General population
        Low = 3       // Prisoners, idle visitors
    }

    [Serializable]
    public sealed class RationTarget
    {
        public string ResourceId { get; set; } = string.Empty;
        public RationingTier Tier { get; set; } = RationingTier.Full;
        public float BaseMultiplier { get; set; } = 1.0f;
    }

    [Serializable]
    public sealed class SurvivorRationAssignment
    {
        public string SurvivorId { get; set; } = string.Empty;
        public PriorityGroupTier Priority { get; set; } = PriorityGroupTier.Standard;
        public float PriorityBonus { get; set; } = 1.0f;
    }

    [Serializable]
    public sealed class ResourceCrisis
    {
        public string CrisisId { get; set; } = string.Empty;
        public ResourceCrisisType Type { get; set; } = ResourceCrisisType.FoodShortage;
        public int DeclaredDay { get; set; } = 1;
        public bool IsResolved { get; set; } = false;
        public string Severity { get; set; } = "moderate";
        public string Notes { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RationingEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string ResourceId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float MoraleImpact { get; set; } = 0f;
    }

    /// <summary>
    /// A request from a real resource consumer. Rationing never owns or
    /// mutates the stock count; the caller supplies the canonical demand and
    /// available quantity and commits the returned amount through its owner.
    /// </summary>
    [Serializable]
    public sealed class ResourceAllocationRequest
    {
        public string ResourceId { get; set; } = string.Empty;
        public string ConsumerId { get; set; } = string.Empty;
        public int DemandUnits { get; set; }
        public int AvailableUnits { get; set; }
        public int CurrentDay { get; set; }
    }

    /// <summary>Bounded policy decision returned to a canonical consumer.</summary>
    [Serializable]
    public sealed class ResourceAllocationDecision
    {
        public bool Authorized { get; set; }
        public string ResourceId { get; set; } = string.Empty;
        public string ConsumerId { get; set; } = string.Empty;
        public int RequestedUnits { get; set; }
        public int AllocatedUnits { get; set; }
        public int AvailableUnits { get; set; }
        public float AppliedMultiplier { get; set; } = 1f;
        public string Reason { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ResourceRationingState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<RationTarget> Targets { get; set; } = new List<RationTarget>();
        public List<SurvivorRationAssignment> Assignments { get; set; } = new List<SurvivorRationAssignment>();
        public List<ResourceCrisis> Crises { get; set; } = new List<ResourceCrisis>();
        public List<RationingEvent> Events { get; set; } = new List<RationingEvent>();
    }

    /// <summary>
    /// Plan 215 — Shelter Resource Rationing & Crisis Management System.
    /// Manages resource rationing tiers (Full, ThreeQuarter, Half, Minimal, etc.),
    /// priority group allocation modifiers (children, medics, workers),
    /// crisis declarations, and rationing morale impact.
    /// </summary>
    public sealed class ResourceRationingSystem
    {
        private readonly ResourceRationingState _state;
        private Func<string, bool>? _resourceValidator;

        public event Action<RationTarget>? OnRationingTierChanged;
        public event Action<ResourceCrisis>? OnCrisisDeclared;
        public event Action<ResourceCrisis>? OnCrisisResolved;
        public event Action<ResourceAllocationDecision>? OnAllocationAuthorized;

        public int ActiveCrisesCount => _state.Crises.Count(c => !c.IsResolved);
        public int TargetCount => _state.Targets.Count;

        public ResourceRationingSystem(
            ResourceRationingState? state = null,
            Func<string, bool>? resourceValidator = null)
        {
            _state = state ?? new ResourceRationingState();
            _resourceValidator = resourceValidator;
        }

        /// <summary>
        /// Binds the canonical catalog/consumer validator supplied by the
        /// host. Compatibility/unit paths may leave this unset; production
        /// callers must bind it before accepting player policy commands.
        /// Invalid legacy rows are ignored when the validator is attached.
        /// </summary>
        public void BindResourceValidator(Func<string, bool>? validator)
        {
            _resourceValidator = validator;
            if (_resourceValidator == null) return;

            _state.Targets.RemoveAll(t =>
                t == null || string.IsNullOrWhiteSpace(t.ResourceId) ||
                !_resourceValidator(t.ResourceId));
        }

        public static float GetTierBaseMultiplier(RationingTier tier) => tier switch
        {
            RationingTier.Full => 1.0f,
            RationingTier.ThreeQuarter => 0.75f,
            RationingTier.Half => 0.50f,
            RationingTier.Third => 0.33f,
            RationingTier.Quarter => 0.25f,
            RationingTier.Minimal => 0.12f,
            RationingTier.None => 0.0f,
            _ => 1.0f
        };

        public static float GetPriorityBonus(PriorityGroupTier priority) => priority switch
        {
            PriorityGroupTier.Critical => 1.30f,
            PriorityGroupTier.High => 1.15f,
            PriorityGroupTier.Standard => 1.0f,
            PriorityGroupTier.Low => 0.75f,
            _ => 1.0f
        };

        public RationTarget SetRationTier(string resourceId, RationingTier tier, int currentDay)
        {
            if (string.IsNullOrWhiteSpace(resourceId)) throw new ArgumentNullException(nameof(resourceId));
            resourceId = resourceId.Trim();
            if (_resourceValidator != null && !_resourceValidator(resourceId))
                throw new ArgumentException($"Resource '{resourceId}' is not present in the canonical resource catalog.", nameof(resourceId));

            var target = _state.Targets.FirstOrDefault(t => string.Equals(t.ResourceId, resourceId, StringComparison.OrdinalIgnoreCase));
            if (target == null)
            {
                target = new RationTarget
                {
                    ResourceId = resourceId.Trim(),
                    Tier = tier,
                    BaseMultiplier = GetTierBaseMultiplier(tier)
                };
                _state.Targets.Add(target);
            }
            else
            {
                target.Tier = tier;
                target.BaseMultiplier = GetTierBaseMultiplier(tier);
            }

            var ev = new RationingEvent
            {
                EventId = $"rev_{_state.NextSequence++}",
                EventType = "rationing_tier_changed",
                Day = currentDay,
                ResourceId = resourceId,
                Description = $"Rationing for {resourceId} adjusted to {tier} ({target.BaseMultiplier:P0}).",
                MoraleImpact = (target.BaseMultiplier - 1.0f) * 10f
            };
            _state.Events.Add(ev);

            OnRationingTierChanged?.Invoke(target);
            return target;
        }

        public RationingTier GetRationTier(string resourceId)
        {
            var target = _state.Targets.FirstOrDefault(t => string.Equals(t.ResourceId, resourceId, StringComparison.OrdinalIgnoreCase));
            return target?.Tier ?? RationingTier.Full;
        }

        public void AssignSurvivorPriority(string survivorId, PriorityGroupTier priority)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));

            var assignment = _state.Assignments.FirstOrDefault(a => string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            if (assignment == null)
            {
                assignment = new SurvivorRationAssignment
                {
                    SurvivorId = survivorId.Trim(),
                    Priority = priority,
                    PriorityBonus = GetPriorityBonus(priority)
                };
                _state.Assignments.Add(assignment);
            }
            else
            {
                assignment.Priority = priority;
                assignment.PriorityBonus = GetPriorityBonus(priority);
            }
        }

        public PriorityGroupTier GetSurvivorPriority(string survivorId)
        {
            var assignment = _state.Assignments.FirstOrDefault(a => string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            return assignment?.Priority ?? PriorityGroupTier.Standard;
        }

        public float GetAllocationMultiplier(string resourceId, string? survivorId = null)
        {
            var target = _state.Targets.FirstOrDefault(t => string.Equals(t.ResourceId, resourceId, StringComparison.OrdinalIgnoreCase));
            float baseMult = target?.BaseMultiplier ?? 1.0f;

            if (string.IsNullOrEmpty(survivorId)) return baseMult;

            var assignment = _state.Assignments.FirstOrDefault(a => string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            float priorityMult = assignment?.PriorityBonus ?? 1.0f;

            return MathF.Round(Math.Clamp(baseMult * priorityMult, 0f, 2.0f), 2);
        }

        /// <summary>
        /// Applies only the persisted ration policy to a real consumer request.
        /// It does not decrement stock, alter needs, or apply morale: the
        /// canonical resource owner commits <see cref="AllocatedUnits"/>.
        /// </summary>
        public ResourceAllocationDecision AuthorizeAllocation(ResourceAllocationRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            string resourceId = request.ResourceId?.Trim() ?? string.Empty;
            string consumerId = request.ConsumerId?.Trim() ?? string.Empty;
            int demand = Math.Max(0, request.DemandUnits);
            int available = Math.Max(0, request.AvailableUnits);
            bool validResource = !string.IsNullOrEmpty(resourceId) &&
                (_resourceValidator == null || _resourceValidator(resourceId));

            var decision = new ResourceAllocationDecision
            {
                Authorized = validResource && demand > 0,
                ResourceId = resourceId,
                ConsumerId = consumerId,
                RequestedUnits = demand,
                AvailableUnits = available,
                AppliedMultiplier = validResource ? GetAllocationMultiplier(resourceId, consumerId) : 0f,
                Reason = !validResource
                    ? "unknown_resource"
                    : demand <= 0
                        ? "empty_demand"
                        : available <= 0
                            ? "no_stock"
                            : "authorized"
            };

            if (decision.Authorized)
            {
                int policyUnits = (int)Math.Floor(demand * decision.AppliedMultiplier);
                decision.AllocatedUnits = Math.Min(demand, Math.Min(available, Math.Max(0, policyUnits)));
                if (decision.AllocatedUnits == 0 && decision.AppliedMultiplier > 0f)
                    decision.Reason = "below_one_unit_after_policy";
                else if (decision.AllocatedUnits < demand)
                    decision.Reason = available < demand ? "stock_bounded" : "rationed";
            }

            OnAllocationAuthorized?.Invoke(decision);
            return decision;
        }

        public ResourceCrisis DeclareCrisis(ResourceCrisisType type, string severity, int currentDay, string notes = "")
        {
            var crisis = new ResourceCrisis
            {
                CrisisId = $"rc_{_state.NextSequence++}",
                Type = type,
                DeclaredDay = Math.Max(1, currentDay),
                IsResolved = false,
                Severity = severity ?? "severe",
                Notes = notes ?? string.Empty
            };

            _state.Crises.Add(crisis);

            var ev = new RationingEvent
            {
                EventId = $"rev_{_state.NextSequence++}",
                EventType = "crisis_declared",
                Day = currentDay,
                ResourceId = type.ToString(),
                Description = $"Resource crisis declared: {type} ({severity}).",
                MoraleImpact = -8.0f
            };
            _state.Events.Add(ev);

            OnCrisisDeclared?.Invoke(crisis);
            return crisis;
        }

        public bool ResolveCrisis(string crisisId, int currentDay)
        {
            var crisis = _state.Crises.FirstOrDefault(c => string.Equals(c.CrisisId, crisisId, StringComparison.OrdinalIgnoreCase));
            if (crisis == null || crisis.IsResolved) return false;

            crisis.IsResolved = true;

            var ev = new RationingEvent
            {
                EventId = $"rev_{_state.NextSequence++}",
                EventType = "crisis_resolved",
                Day = currentDay,
                ResourceId = crisis.Type.ToString(),
                Description = $"Resource crisis resolved: {crisis.Type}.",
                MoraleImpact = 5.0f
            };
            _state.Events.Add(ev);

            OnCrisisResolved?.Invoke(crisis);
            return true;
        }

        public float CalculateMoraleImpact(string survivorId)
        {
            float impact = 0f;
            var priority = GetSurvivorPriority(survivorId);

            foreach (var target in _state.Targets)
            {
                float mult = target.BaseMultiplier * GetPriorityBonus(priority);
                if (mult < 1.0f)
                {
                    impact += (mult - 1.0f) * 6.0f;
                }
            }

            // Penalty for active unresolved crises
            impact -= (ActiveCrisesCount * 3.0f);

            return MathF.Round(Math.Clamp(impact, -30f, 5f), 1);
        }

        public ResourceRationingState CaptureState()
        {
            var state = new ResourceRationingState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Targets = new List<RationTarget>(_state.Targets.Count),
                Assignments = new List<SurvivorRationAssignment>(_state.Assignments.Count),
                Crises = new List<ResourceCrisis>(_state.Crises.Count),
                Events = new List<RationingEvent>(_state.Events.Count)
            };

            foreach (var t in _state.Targets)
            {
                state.Targets.Add(new RationTarget
                {
                    ResourceId = t.ResourceId,
                    Tier = t.Tier,
                    BaseMultiplier = t.BaseMultiplier
                });
            }

            foreach (var a in _state.Assignments)
            {
                state.Assignments.Add(new SurvivorRationAssignment
                {
                    SurvivorId = a.SurvivorId,
                    Priority = a.Priority,
                    PriorityBonus = a.PriorityBonus
                });
            }

            foreach (var c in _state.Crises)
            {
                state.Crises.Add(new ResourceCrisis
                {
                    CrisisId = c.CrisisId,
                    Type = c.Type,
                    DeclaredDay = c.DeclaredDay,
                    IsResolved = c.IsResolved,
                    Severity = c.Severity,
                    Notes = c.Notes
                });
            }

            foreach (var e in _state.Events)
            {
                state.Events.Add(new RationingEvent
                {
                    EventId = e.EventId,
                    EventType = e.EventType,
                    Day = e.Day,
                    ResourceId = e.ResourceId,
                    Description = e.Description,
                    MoraleImpact = e.MoraleImpact
                });
            }

            return state;
        }

        public void RestoreState(ResourceRationingState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Targets.Clear();
            _state.Assignments.Clear();
            _state.Crises.Clear();
            _state.Events.Clear();

            if (state.Targets != null)
            {
                foreach (var t in state.Targets)
                {
                    if (t == null || string.IsNullOrWhiteSpace(t.ResourceId) ||
                        (_resourceValidator != null && !_resourceValidator(t.ResourceId.Trim())))
                        continue;

                    _state.Targets.Add(new RationTarget
                    {
                        ResourceId = t.ResourceId.Trim(),
                        Tier = t.Tier,
                        BaseMultiplier = t.BaseMultiplier
                    });
                }
            }

            if (state.Assignments != null)
            {
                foreach (var a in state.Assignments)
                {
                    _state.Assignments.Add(new SurvivorRationAssignment
                    {
                        SurvivorId = a.SurvivorId,
                        Priority = a.Priority,
                        PriorityBonus = a.PriorityBonus
                    });
                }
            }

            if (state.Crises != null)
            {
                foreach (var c in state.Crises)
                {
                    _state.Crises.Add(new ResourceCrisis
                    {
                        CrisisId = c.CrisisId,
                        Type = c.Type,
                        DeclaredDay = c.DeclaredDay,
                        IsResolved = c.IsResolved,
                        Severity = c.Severity,
                        Notes = c.Notes
                    });
                }
            }

            if (state.Events != null)
            {
                foreach (var e in state.Events)
                {
                    _state.Events.Add(new RationingEvent
                    {
                        EventId = e.EventId,
                        EventType = e.EventType,
                        Day = e.Day,
                        ResourceId = e.ResourceId,
                        Description = e.Description,
                        MoraleImpact = e.MoraleImpact
                    });
                }
            }
        }
    }
}
