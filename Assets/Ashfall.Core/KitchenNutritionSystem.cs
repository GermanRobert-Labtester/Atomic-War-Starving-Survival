// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Survivors;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class KitchenNutritionState
    {
        public string systemId = KitchenNutritionSystem.SystemId;
        public List<PrepJob> activeJobs = new List<PrepJob>();
        public List<PantryItem> pantry = new List<PantryItem>();
        public float cellarTempC = 10f;
        public bool hasCellar;
        public bool hasRefrigeration;
        public int totalMealsPrepared;
        public int totalMealsServed;
        public List<MealServingLog> servingLog = new List<MealServingLog>();
    }

    [Serializable]
    public sealed class PrepJob
    {
        public string jobId = string.Empty;
        public string recipeId = string.Empty;
        public string assignedCookId = string.Empty;
        public int dayStarted = -1;
        public List<string> reservedInputIds = new List<string>();
        public List<int> reservedInputCounts = new List<int>();
        public float progressHours;
        public float totalHoursRequired = 2f;
        public bool isComplete;
        public bool isCancelled;
        public int portionsProduced;
        /// <summary>Plan 24B A2 — the assigned cook's shared productivity
        /// modifier, stamped at job start (permille; 0 = unstamped legacy).</summary>
        public int cookQualityPermille;
    }

    [Serializable]
    public sealed class PantryItem
    {
        public string itemId = string.Empty;
        public string displayName = string.Empty;
        public float spoilageTimer;        // days until spoiled
        public float maxSpoilageDays = 7f;
        public PreservationMethod preservation;
        public bool isSpoiled;
        public int portionCount;
        /// <summary>Plan 24B A2 — the batch's cooked quality stamp (permille;
        /// 0 = unstamped, legacy behavior). Stamped from the assigned cook's
        /// shared productivity verdict at prep completion.</summary>
        public int qualityPermille;
    }

    public enum PreservationMethod { None, RootCellar, Refrigeration, Fermentation, Smoking, Canning }

    [Serializable]
    public sealed class MealServingLog
    {
        public int day;
        public string survivorId = string.Empty;
        public string recipeId = string.Empty;
        public float moraleBonus;
        public float nutritionScore;
        public bool wasSafe;
    }

    public sealed class KitchenNutritionSystem
    {
        public const string SystemId = "kitchen_nutrition";
        /// <summary>Plan 24B needs-modifier source id for meal-quality
        /// morale (safe +5 / unsafe −5, attribution only).</summary>
        public const string MealQualityModifierSource = "kitchen.meal_quality";
        /// <summary>Plan 24B needs-modifier source id for the unsafe-meal
        /// health penalty (−5, attribution only).</summary>
        public const string UnsafeMealModifierSource = "kitchen.meal_unsafe";
        private KitchenNutritionState _state = new KitchenNutritionState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory _inventory;
        private readonly NeedsSystem _needs;
        private int _currentDay;

        public KitchenNutritionState State => _state;
        public event Action<PrepJob> OnJobCompleted;
        public event Action<MealServingLog> OnMealServed;
        public event Action<string, int>? OnPortionsSpoiled;
        public event Action OnKitchenChanged;

        /// <summary>
        /// Optional projection from the campaign survivor authority. The
        /// kitchen owns prep-job mutation, so the fitness gate lives here
        /// rather than only in a panel or host callback. A null provider keeps
        /// headless legacy callers honest; production binds the shared
        /// survivor-fitness projection.
        /// </summary>
        public Func<string, FitnessVerdict?>? SurvivorFitnessProvider { get; set; }
        /// <summary>Optional shared role verdict for the authored mess/cook requirement.</summary>
        public Func<string, RoleFitnessVerdict?>? CookFitnessProvider { get; set; }
        /// <summary>Plan 24B A2 — the ONE shared worker-productivity seam
        /// (host-bound with the mess role's authored skill id). Null or a null
        /// verdict ⇒ exact legacy behavior. The kitchen never resolves skill
        /// levels itself.</summary>
        public Func<string, WorkerProductivityVerdict?>? CookProductivityResolver { get; set; }
        /// <summary>Skill-scaled meal-safety band for stamped batches: the
        /// legacy 0.9 chance scales by the cook's yield modifier, bounded
        /// [0.7, 0.98]. Unstamped batches keep exactly 0.9.</summary>
        public const float SafeMealBaseChance = 0.9f;
        public const float SafeMealMinChance = 0.7f;
        public const float SafeMealMaxChance = 0.98f;

        public KitchenNutritionSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            NeedsSystem needs,
ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult SetCellar(bool hasCellar, float tempC = 10f)
        {
            _state.hasCellar = hasCellar;
            _state.cellarTempC = Math.Clamp(tempC, -5f, 20f);
            OnKitchenChanged?.Invoke();
            return ActionResult.Success("kitchen.cellar_set");
        }

        public ActionResult SetRefrigeration(bool hasRefrigeration)
        {
            _state.hasRefrigeration = hasRefrigeration;
            OnKitchenChanged?.Invoke();
            return ActionResult.Success("kitchen.refrigeration_set");
        }

        public ActionResult StartPrepJob(string recipeId, string cookId, Dictionary<string, int> inputRequirements)
        {
            var roleFitness = CookFitnessProvider?.Invoke(cookId);
            if (roleFitness != null && !roleFitness.Allowed)
                return ActionResult.Blocked("cook_fitness_blocked", "kitchen.cook_fitness_blocked");

            var fitness = roleFitness?.BaseVerdict ?? SurvivorFitnessProvider?.Invoke(cookId);
            if (roleFitness == null && fitness != null && fitness.Level >= FitnessLevel.Unfit)
                return ActionResult.Blocked("fitness_blocked", "kitchen.fitness_blocked");

            if (_state.activeJobs.Exists(j => j.recipeId == recipeId && !j.isComplete && !j.isCancelled))
                return ActionResult.Blocked("job_active", "kitchen.job_active");

            var reservedIds = new List<string>();
            var reservedCounts = new List<int>();
            if (inputRequirements != null && inputRequirements.Count > 0)
            {
                if (!_inventory.TryConsumeBill(inputRequirements))
                    return ActionResult.Blocked("insufficient_ingredients", "kitchen.insufficient_ingredients");

                foreach (var req in inputRequirements)
                {
                    if (req.Value > 0)
                    {
                        reservedIds.Add(req.Key);
                        reservedCounts.Add(req.Value);
                    }
                }
            }

            var job = new PrepJob
            {
                jobId = $"prep_{_currentDay}_{recipeId}_{cookId}",
                recipeId = recipeId,
                assignedCookId = cookId,
                dayStarted = _currentDay,
                reservedInputIds = reservedIds,
                reservedInputCounts = reservedCounts,
                totalHoursRequired = 2f,
                cookQualityPermille = CookProductivityResolver?.Invoke(cookId)?.YieldModifierPermille ?? 0
            };
            _state.activeJobs.Add(job);
            OnKitchenChanged?.Invoke();
            return ActionResult.Success("kitchen.job_started");
        }

        public ActionResult CancelJob(string jobId)
        {
            var job = _state.activeJobs.Find(j => j.jobId == jobId);
            if (job == null || job.isComplete || job.isCancelled)
                return ActionResult.Blocked("no_job", "kitchen.no_job");

            job.isCancelled = true;
            // Refund reserved inputs
            for (int i = 0; i < job.reservedInputIds.Count; i++)
            {
                int count = i < job.reservedInputCounts.Count ? job.reservedInputCounts[i] : 1;
                _inventory.AddById(job.reservedInputIds[i], count);
            }

            OnKitchenChanged?.Invoke();
            return ActionResult.Success("kitchen.job_cancelled");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Update spoilage of existing pantry stock before fresh meals are cooked today
            UpdateSpoilage();

            foreach (var job in _state.activeJobs)
            {
                if (job.isComplete || job.isCancelled) continue;

                job.progressHours += 8f; // standard work day
                if (job.progressHours >= job.totalHoursRequired)
                {
                    job.isComplete = true;
                    job.portionsProduced = 3; // catalog-defined portions
                    // Plan 24B A2 — skill-to-yield (waste leg): a stamped,
                    // degraded cook's modifier scales completed portions down
                    // (bounded 1..recipe portions); skill never exceeds the
                    // recipe's output or bypasses ingredient costs.
                    if (job.cookQualityPermille > 0 && job.cookQualityPermille < 1000)
                    {
                        int scaled = (int)MathF.Round(3f * job.cookQualityPermille / 1000f);
                        job.portionsProduced = Math.Clamp(scaled, 1, 3);
                    }
                    _state.totalMealsPrepared += job.portionsProduced;
                    _log.Info($"[Kitchen] {job.recipeId} complete: {job.portionsProduced} portions");

                    // Add to pantry
                    var pantryItem = new PantryItem
                    {
                        itemId = job.recipeId,
                        displayName = job.recipeId,
                        portionCount = job.portionsProduced,
                        spoilageTimer = GetSpoilageDays(job.recipeId),
                        preservation = GetPreservationMethod(job.recipeId),
                        qualityPermille = job.cookQualityPermille
                    };
                    _state.pantry.Add(pantryItem);

                    OnJobCompleted?.Invoke(job);
                }
            }

            // CR3-05: evict terminally-finished jobs from the underlying list.
            // GetActiveJobs already filters on read, but without this RemoveAll
            // the list serialises every completed job into every save and grows
            // without bound across long campaigns. Mirrors ArchiveDeskSystem
            // and MentalHealthCrisisSystem patterns.
            _state.activeJobs.RemoveAll(j => j.isComplete || j.isCancelled);

            OnKitchenChanged?.Invoke();
        }

        public int GetAvailablePortions(string recipeId)
        {
            int total = 0;
            foreach (var p in _state.pantry)
            {
                if (p.itemId == recipeId && !p.isSpoiled && p.portionCount > 0)
                    total += p.portionCount;
            }
            return total;
        }

        public ActionResult ServeMeal(string survivorId, string recipeId)
        {
            var pantryItem = _state.pantry.Find(p => p.itemId == recipeId && p.portionCount > 0 && !p.isSpoiled);
            if (pantryItem == null)
                return ActionResult.Blocked("no_meal", "kitchen.no_meal");

            pantryItem.portionCount--;

            // Calculate nutrition/morale effect. Plan 24B A2 — skill-to-yield
            // (quality leg): a stamped batch scales the safe-meal chance by
            // its cook's shared productivity modifier, bounded [0.7, 0.98];
            // unstamped (legacy) batches keep exactly 0.9. The roll itself
            // stays on the kitchen's seeded stream at the same position.
            float safeChance = pantryItem.qualityPermille > 0
                ? Math.Clamp(
                    SafeMealBaseChance * pantryItem.qualityPermille / 1000f,
                    SafeMealMinChance, SafeMealMaxChance)
                : SafeMealBaseChance;
            float safetyRoll = (float)_rng.NextDouble();
            bool wasSafe = safetyRoll < safeChance;

            float moraleBonus = wasSafe ? 5f : -5f;
            float nutritionScore = wasSafe ? 8f : 2f;

            float hungerDelta = -Math.Max(30f, nutritionScore * 6f);
            // Plan 22/§2 24B.10: the hunger restoration stays DIRECT through
            // the consumption/meal authority — the modifier stack never
            // double-applies hunger. Meal-quality morale (and the unsafe-meal
            // health penalty) route through the shared attributed seam (Plan
            // 24B A1) so the survivor-detail contributor display can name
            // the kitchen as the cause.
            _needs.Modify(survivorId, NeedKind.Hunger, hungerDelta);
            _needs.ApplyAttributedDelta(
                survivorId, NeedKind.Morale, moraleBonus, MealQualityModifierSource);

            if (!wasSafe)
            {
                _needs.ApplyAttributedDelta(
                    survivorId, NeedKind.Health, -5f, UnsafeMealModifierSource);
                _log.Warn($"[Kitchen] {survivorId} served unsafe {recipeId}");
            }

            var log = new MealServingLog
            {
                day = _currentDay, survivorId = survivorId, recipeId = recipeId,
                moraleBonus = moraleBonus, nutritionScore = nutritionScore, wasSafe = wasSafe
            };
            _state.servingLog.Add(log);
            _state.totalMealsServed++;

            OnMealServed?.Invoke(log);
            return ActionResult.Success("kitchen.meal_served",
                new Dictionary<string, double>
                {
                    { "morale", moraleBonus },
                    { "nutrition", nutritionScore },
                    { "hunger_delta", hungerDelta }
                });
        }

        public ActionResult ServeAllMeals(IReadOnlyList<string> livingSurvivorIds, string recipeId)
        {
            if (livingSurvivorIds == null || livingSurvivorIds.Count == 0)
                return ActionResult.Blocked("no_survivors", "kitchen.no_survivors");

            int available = GetAvailablePortions(recipeId);
            if (available < livingSurvivorIds.Count)
            {
                return ActionResult.Blocked("insufficient_portions", "kitchen.insufficient_portions",
                    new Dictionary<string, double>
                    {
                        { "requested", livingSurvivorIds.Count },
                        { "available", available },
                        { "shortfall", livingSurvivorIds.Count - available }
                    });
            }

            foreach (var survivorId in livingSurvivorIds)
            {
                ServeMeal(survivorId, recipeId);
            }

            return ActionResult.Success("kitchen.all_served",
                new Dictionary<string, double>
                {
                    { "served_count", livingSurvivorIds.Count }
                });
        }

        private float GetSpoilageDays(string recipeId)
        {
            if (_state.hasRefrigeration) return 14f;
            if (_state.hasCellar) return 5f;
            return 2f;
        }

        private PreservationMethod GetPreservationMethod(string recipeId)
        {
            if (_state.hasRefrigeration) return PreservationMethod.Refrigeration;
            if (_state.hasCellar) return PreservationMethod.RootCellar;
            return PreservationMethod.None;
        }

        private void UpdateSpoilage()
        {
            foreach (var item in _state.pantry)
            {
                if (item.isSpoiled) continue;
                item.spoilageTimer -= 1f;
                if (item.spoilageTimer <= 0)
                {
                    item.isSpoiled = true;
                    _log.Warn($"[Kitchen] {item.displayName} spoiled");
                    OnPortionsSpoiled?.Invoke(item.itemId, item.portionCount);
                }
            }
        }

        public List<PrepJob> GetActiveJobs() => _state.activeJobs.FindAll(j => !j.isComplete && !j.isCancelled);

        public KitchenNutritionState CaptureState() => CloneState(_state);

        public void RestoreState(KitchenNutritionState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static KitchenNutritionState CloneState(KitchenNutritionState src)
        {
            if (src == null) return new KitchenNutritionState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<KitchenNutritionState>(json) ?? new KitchenNutritionState();
        }
    }
}
