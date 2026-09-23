// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Culture
{
    public enum FestivalType
    {
        HarvestCommunion = 0,
        RemembranceVigil = 1,
        MidwinterSolstice = 2,
        FoundingJubilee = 3
    }

    [Serializable]
    public sealed class FestivalCommodityRequirement
    {
        public string ItemId { get; set; } = string.Empty;
        public int RequiredUnits { get; set; }

        public FestivalCommodityRequirement() { }

        public FestivalCommodityRequirement(string itemId, int requiredUnits)
        {
            ItemId = itemId ?? string.Empty;
            RequiredUnits = Math.Max(1, requiredUnits);
        }
    }

    [Serializable]
    public sealed class FestivalPlanSaveState
    {
        public string FestivalId { get; set; } = string.Empty;
        public FestivalType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PlannedDay { get; set; }
        public int DurationDays { get; set; }
        public int DaysActive { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public int MoraleBoostPermille { get; set; }
        public int DespairReductionPermille { get; set; }
        public List<FestivalCommodityRequirement> RequiredCommodities { get; set; } = new();
    }

    [Serializable]
    public sealed class ShelterFestivalSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<FestivalPlanSaveState> Festivals { get; set; } = new();
    }

    public sealed class FestivalPlan
    {
        public string FestivalId { get; }
        public FestivalType Type { get; }
        public string Title { get; }
        public int PlannedDay { get; }
        public int DurationDays { get; }
        public int DaysActive { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public int MoraleBoostPermille { get; }
        public int DespairReductionPermille { get; }
        public IReadOnlyList<FestivalCommodityRequirement> RequiredCommodities => _requiredCommodities;

        private readonly List<FestivalCommodityRequirement> _requiredCommodities = new();

        public FestivalPlan(
            string festivalId,
            FestivalType type,
            string title,
            int plannedDay,
            int durationDays,
            int moraleBoostPermille,
            int despairReductionPermille,
            IEnumerable<FestivalCommodityRequirement> commodities)
        {
            FestivalId = festivalId ?? throw new ArgumentNullException(nameof(festivalId));
            Type = type;
            Title = string.IsNullOrWhiteSpace(title) ? type.ToString() : title;
            PlannedDay = plannedDay;
            DurationDays = Math.Max(1, durationDays);
            DaysActive = 0;
            IsActive = false;
            IsCompleted = false;
            MoraleBoostPermille = Math.Max(0, moraleBoostPermille);
            DespairReductionPermille = Math.Max(0, despairReductionPermille);

            if (commodities != null)
            {
                _requiredCommodities.AddRange(commodities);
            }
        }

        internal FestivalPlan(FestivalPlanSaveState state)
        {
            FestivalId = state.FestivalId;
            Type = state.Type;
            Title = state.Title;
            PlannedDay = state.PlannedDay;
            DurationDays = state.DurationDays;
            DaysActive = state.DaysActive;
            IsActive = state.IsActive;
            IsCompleted = state.IsCompleted;
            MoraleBoostPermille = state.MoraleBoostPermille;
            DespairReductionPermille = state.DespairReductionPermille;

            if (state.RequiredCommodities != null)
            {
                _requiredCommodities.AddRange(state.RequiredCommodities);
            }
        }

        public FestivalPlanSaveState CaptureState()
        {
            return new FestivalPlanSaveState
            {
                FestivalId = FestivalId,
                Type = Type,
                Title = Title,
                PlannedDay = PlannedDay,
                DurationDays = DurationDays,
                DaysActive = DaysActive,
                IsActive = IsActive,
                IsCompleted = IsCompleted,
                MoraleBoostPermille = MoraleBoostPermille,
                DespairReductionPermille = DespairReductionPermille,
                RequiredCommodities = new List<FestivalCommodityRequirement>(_requiredCommodities)
            };
        }
    }

    /// <summary>
    /// Expansion 17 / UNBLOCK-02 §2.5 / UNBLOCK-05 §3.1 / §18.7:
    /// The Long Evening — Cultural Leisure & Shelter Festival Engine.
    /// Coordinates communal festivals, memorial vigils, and rest periods to counter survivor despair.
    /// Strictly adheres to the intake rule: festivals consume physical commodities, NEVER chits/funds.
    /// </summary>
    public sealed class ShelterFestivalEngine
    {
        private readonly List<FestivalPlan> _festivals = new();

        public IReadOnlyList<FestivalPlan> Festivals => _festivals;
        public IReadOnlyList<FestivalPlan> ActiveFestivals => _festivals.Where(f => f.IsActive).ToList();

        public event Action<FestivalPlan>? OnFestivalScheduled;
        public event Action<FestivalPlan>? OnFestivalCommenced;
        public event Action<FestivalPlan>? OnFestivalCompleted;
        public event Action<FestivalPlan>? OnFestivalCancelled;

        public FestivalPlan ScheduleFestival(
            FestivalType type,
            string title,
            int plannedDay,
            int durationDays = 1,
            IEnumerable<FestivalCommodityRequirement>? customCommodities = null)
        {
            string id = $"festival_{type}_{plannedDay}_{_festivals.Count + 1}";

            int moraleBoost;
            int despairReduction;
            List<FestivalCommodityRequirement> commodities = new();

            if (customCommodities != null)
            {
                commodities.AddRange(customCommodities);
            }

            switch (type)
            {
                case FestivalType.HarvestCommunion:
                    moraleBoost = 150; // +15% morale
                    despairReduction = 200; // -20% despair
                    if (commodities.Count == 0)
                    {
                        commodities.Add(new FestivalCommodityRequirement("item_rations", 10));
                        commodities.Add(new FestivalCommodityRequirement("item_fuel", 5));
                    }
                    break;

                case FestivalType.RemembranceVigil:
                    moraleBoost = 100; // +10% morale
                    despairReduction = 300; // -30% despair (vigil eases grief)
                    if (commodities.Count == 0)
                    {
                        commodities.Add(new FestivalCommodityRequirement("item_fuel", 8));
                    }
                    break;

                case FestivalType.MidwinterSolstice:
                    moraleBoost = 250; // +25% morale
                    despairReduction = 250; // -25% despair
                    if (commodities.Count == 0)
                    {
                        commodities.Add(new FestivalCommodityRequirement("item_fuel", 15));
                        commodities.Add(new FestivalCommodityRequirement("item_rations", 15));
                    }
                    break;

                case FestivalType.FoundingJubilee:
                default:
                    moraleBoost = 300; // +30% morale
                    despairReduction = 150; // -15% despair
                    if (commodities.Count == 0)
                    {
                        commodities.Add(new FestivalCommodityRequirement("item_rations", 20));
                        commodities.Add(new FestivalCommodityRequirement("item_fuel", 10));
                    }
                    break;
            }

            var plan = new FestivalPlan(id, type, title, plannedDay, durationDays, moraleBoost, despairReduction, commodities);
            _festivals.Add(plan);
            OnFestivalScheduled?.Invoke(plan);
            return plan;
        }

        public bool TryCommenceFestival(
            string festivalId,
            int currentDay,
            Func<string, int, bool> tryConsumeCommodity)
        {
            if (string.IsNullOrWhiteSpace(festivalId) || tryConsumeCommodity == null)
                return false;

            var plan = _festivals.FirstOrDefault(f => string.Equals(f.FestivalId, festivalId, StringComparison.OrdinalIgnoreCase));
            if (plan == null || plan.IsActive || plan.IsCompleted)
                return false;

            // Preflight check: verify all commodities can be consumed
            // (Note: UNBLOCK-02 §2.5 strictly prohibits chit/currency consumption here)
            foreach (var req in plan.RequiredCommodities)
            {
                if (string.IsNullOrWhiteSpace(req.ItemId) || req.RequiredUnits <= 0)
                    continue;

                // Ensure item is not currency or chits
                if (req.ItemId.Contains("chit") || req.ItemId.Contains("currency"))
                    return false;
            }

            // Consume all required commodities
            foreach (var req in plan.RequiredCommodities)
            {
                if (req.RequiredUnits > 0)
                {
                    bool success = tryConsumeCommodity(req.ItemId, req.RequiredUnits);
                    if (!success)
                    {
                        return false;
                    }
                }
            }

            plan.IsActive = true;
            plan.DaysActive = 1;
            OnFestivalCommenced?.Invoke(plan);
            return true;
        }

        public void ProcessDailyTick(int currentDay)
        {
            for (int i = 0; i < _festivals.Count; i++)
            {
                var plan = _festivals[i];
                if (!plan.IsActive) continue;

                plan.DaysActive++;
                if (plan.DaysActive > plan.DurationDays)
                {
                    plan.IsActive = false;
                    plan.IsCompleted = true;
                    OnFestivalCompleted?.Invoke(plan);
                }
            }
        }

        public bool CancelFestival(string festivalId)
        {
            var plan = _festivals.FirstOrDefault(f => string.Equals(f.FestivalId, festivalId, StringComparison.OrdinalIgnoreCase));
            if (plan == null || plan.IsCompleted) return false;

            plan.IsActive = false;
            _festivals.Remove(plan);
            OnFestivalCancelled?.Invoke(plan);
            return true;
        }

        public ShelterFestivalSaveState CaptureState()
        {
            var save = new ShelterFestivalSaveState
            {
                schema_version = 1,
                Festivals = new List<FestivalPlanSaveState>(_festivals.Count)
            };

            for (int i = 0; i < _festivals.Count; i++)
            {
                save.Festivals.Add(_festivals[i].CaptureState());
            }

            return save;
        }

        public void RestoreState(ShelterFestivalSaveState? state)
        {
            _festivals.Clear();
            if (state?.Festivals == null) return;

            for (int i = 0; i < state.Festivals.Count; i++)
            {
                var fState = state.Festivals[i];
                if (fState != null)
                {
                    _festivals.Add(new FestivalPlan(fState));
                }
            }
        }
    }
}
