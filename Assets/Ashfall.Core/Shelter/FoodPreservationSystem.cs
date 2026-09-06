// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class FoodBatchCohort
    {
        public string CohortId { get; set; } = string.Empty;
        public string FoodItemId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string TierId { get; set; } = "preservation_ambient";
        public int DayStored { get; set; }
        public float FreshnessPercent { get; set; } = 100f;
        public bool IsSpoiled { get; set; }
    }

    [Serializable]
    public sealed class ActiveCuringJob
    {
        public string JobId { get; set; } = string.Empty;
        public string RecipeId { get; set; } = string.Empty;
        public string AssignedCookId { get; set; } = string.Empty;
        public int DayStarted { get; set; }
        public int ProgressTicks { get; set; }
        public int TargetTicks { get; set; } = 4;
        public bool IsComplete { get; set; }
    }

    [Serializable]
    public sealed class FoodPreservationState
    {
        public string SystemId { get; set; } = FoodPreservationSystem.SystemId;
        public List<FoodBatchCohort> Cohorts { get; set; } = new List<FoodBatchCohort>();
        public List<ActiveCuringJob> ActiveJobs { get; set; } = new List<ActiveCuringJob>();
        public bool IsPowerOnline { get; set; } = true;
        public int UnpoweredDays { get; set; }
        public int TotalSpoiledDiscarded { get; set; }
        public int TotalCured { get; set; }
        public int TotalConsumed { get; set; }
        public int NextCohortSeq { get; set; }
    }

    public sealed class FoodPreservationSystem
    {
        public const string SystemId = "food_preservation";

        private FoodPreservationState _state = new FoodPreservationState();
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly FoodPreservationCatalog _catalog;
        private readonly ILog _log;
        private int _currentDay;

        public FoodPreservationState State => _state;
        public bool IsPowerOnline => _state.IsPowerOnline;
        public int UnpoweredDays => _state.UnpoweredDays;

        public event Action<FoodBatchCohort>? OnFoodSpoiled;
        public event Action<ActiveCuringJob>? OnCuringCompleted;
        public event Action<string, int, bool>? OnFoodConsumed;

        public FoodPreservationSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            FoodPreservationCatalog catalog,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public void SetPowerStatus(bool isOnline)
        {
            _state.IsPowerOnline = isOnline;
            if (isOnline)
            {
                _state.UnpoweredDays = 0;
            }
        }

        public ActionResult AddCohort(string foodItemId, int quantity, string tierId, int currentDay)
        {
            if (string.IsNullOrEmpty(foodItemId) || quantity <= 0)
                return ActionResult.Blocked("invalid_args", "food.invalid_item_or_qty");

            var tier = _catalog.GetTier(tierId) ?? _catalog.GetTier("preservation_ambient");
            string actualTierId = tier?.id ?? "preservation_ambient";

            _state.NextCohortSeq++;
            var cohort = new FoodBatchCohort
            {
                CohortId = $"cohort_{currentDay}_{foodItemId}_{_state.NextCohortSeq}",
                FoodItemId = foodItemId,
                Quantity = quantity,
                TierId = actualTierId,
                DayStored = currentDay,
                FreshnessPercent = 100f,
                IsSpoiled = false
            };

            _state.Cohorts.Add(cohort);
            _log.Info($"[FoodPreservation] Added {quantity}x {foodItemId} under {actualTierId}");
            return ActionResult.Success("food.cohort_added");
        }

        public ActionResult StartCuringJob(string recipeId, string cookId, int currentDay)
        {
            var recipe = _catalog.GetRecipe(recipeId);
            if (recipe == null)
                return ActionResult.Blocked("unknown_recipe", "curing.unknown_recipe");

            // Atomic inventory consumption check
            var bill = new Dictionary<string, int>
            {
                { recipe.input_item_id, recipe.input_quantity },
                { recipe.preservative_item_id, recipe.preservative_quantity }
            };

            if (!_inventory.TryConsumeBill(bill))
                return ActionResult.Blocked("insufficient_ingredients", "curing.insufficient_ingredients");

            var job = new ActiveCuringJob
            {
                JobId = $"curing_{currentDay}_{recipeId}_{_state.ActiveJobs.Count + 1}",
                RecipeId = recipeId,
                AssignedCookId = cookId,
                DayStarted = currentDay,
                ProgressTicks = 0,
                TargetTicks = recipe.work_ticks_required,
                IsComplete = false
            };

            _state.ActiveJobs.Add(job);
            _log.Info($"[FoodPreservation] Started curing job {job.JobId} for {recipe.display_name}");
            return ActionResult.Success("curing.job_started");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            if (!_state.IsPowerOnline)
            {
                _state.UnpoweredDays++;
            }

            // Update preservation freshness for each cohort
            foreach (var cohort in _state.Cohorts)
            {
                if (cohort.IsSpoiled || cohort.Quantity <= 0) continue;

                var tier = _catalog.GetTier(cohort.TierId);
                float shelfDays = tier != null && tier.shelf_life_days > 0 ? tier.shelf_life_days : 5f;

                // Cryogenic outage check: if unpowered > 1 day, decay accelerates to ambient
                if (cohort.TierId == "preservation_cryogenic" && !_state.IsPowerOnline)
                {
                    if (_state.UnpoweredDays > 1)
                    {
                        shelfDays = 5f; // ambient rate
                    }
                }

                float decayPercentPerDay = 100f / Math.Max(1f, shelfDays);
                cohort.FreshnessPercent -= decayPercentPerDay;

                if (cohort.FreshnessPercent <= 0f)
                {
                    cohort.FreshnessPercent = 0f;
                    cohort.IsSpoiled = true;
                    OnFoodSpoiled?.Invoke(cohort);
                    _log.Warn($"[FoodPreservation] Cohort {cohort.CohortId} ({cohort.FoodItemId}) spoiled!");
                }
            }

            // Progress active curing jobs
            for (int i = _state.ActiveJobs.Count - 1; i >= 0; i--)
            {
                var job = _state.ActiveJobs[i];
                if (job.IsComplete) continue;

                job.ProgressTicks++;
                if (job.ProgressTicks >= job.TargetTicks)
                {
                    job.IsComplete = true;
                    var recipe = _catalog.GetRecipe(job.RecipeId);
                    if (recipe != null)
                    {
                        _inventory.AddById(recipe.output_item_id, recipe.output_quantity);
                        AddCohort(recipe.output_item_id, recipe.output_quantity, recipe.target_tier, _currentDay);
                        _state.TotalCured += recipe.output_quantity;
                        _log.Info($"[FoodPreservation] Curing job {job.JobId} completed -> {recipe.output_quantity}x {recipe.output_item_id}");
                    }
                    OnCuringCompleted?.Invoke(job);
                }
            }

            // Clean up completed jobs
            _state.ActiveJobs.RemoveAll(j => j.IsComplete);
        }

        public int ConsumeFood(string foodItemId, int neededCount, out int spoiledConsumed)
        {
            spoiledConsumed = 0;
            int remaining = neededCount;

            // Sort cohorts: consume lowest freshness first (FIFO / perishability priority)
            _state.Cohorts.Sort((a, b) => a.FreshnessPercent.CompareTo(b.FreshnessPercent));

            for (int i = 0; i < _state.Cohorts.Count && remaining > 0; i++)
            {
                var cohort = _state.Cohorts[i];
                if (cohort.FoodItemId != foodItemId || cohort.Quantity <= 0) continue;

                int take = Math.Min(remaining, cohort.Quantity);
                cohort.Quantity -= take;
                remaining -= take;
                _state.TotalConsumed += take;

                if (cohort.IsSpoiled)
                {
                    spoiledConsumed += take;
                }

                OnFoodConsumed?.Invoke(foodItemId, take, cohort.IsSpoiled);
            }

            // Clean up empty cohorts
            _state.Cohorts.RemoveAll(c => c.Quantity <= 0);

            return neededCount - remaining;
        }

        public int DiscardSpoiled(string? foodItemId = null)
        {
            int discarded = 0;
            for (int i = _state.Cohorts.Count - 1; i >= 0; i--)
            {
                var cohort = _state.Cohorts[i];
                if (cohort.IsSpoiled && (foodItemId == null || cohort.FoodItemId == foodItemId))
                {
                    discarded += cohort.Quantity;
                    _state.Cohorts.RemoveAt(i);
                }
            }

            _state.TotalSpoiledDiscarded += discarded;
            _log.Info($"[FoodPreservation] Discarded {discarded} spoiled food units.");
            return discarded;
        }

        public int GetTotalFood(string? foodItemId = null)
        {
            int total = 0;
            foreach (var c in _state.Cohorts)
            {
                if (!c.IsSpoiled && (foodItemId == null || c.FoodItemId == foodItemId))
                    total += c.Quantity;
            }
            return total;
        }

        public int GetSpoiledFood(string? foodItemId = null)
        {
            int total = 0;
            foreach (var c in _state.Cohorts)
            {
                if (c.IsSpoiled && (foodItemId == null || c.FoodItemId == foodItemId))
                    total += c.Quantity;
            }
            return total;
        }

        public FoodPreservationState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<FoodPreservationState>(json) ?? new FoodPreservationState();
        }

        public void RestoreState(FoodPreservationState saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<FoodPreservationState>(json) ?? new FoodPreservationState();
        }
    }
}
