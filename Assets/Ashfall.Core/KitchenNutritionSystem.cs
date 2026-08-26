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
        private KitchenNutritionState _state = new KitchenNutritionState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory _inventory;
        private readonly NeedsSystem _needs;
        private int _currentDay;

        public KitchenNutritionState State => _state;
        public event Action<PrepJob> OnJobCompleted;
        public event Action<MealServingLog> OnMealServed;
        public event Action OnKitchenChanged;

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
            if (_state.activeJobs.Exists(j => j.recipeId == recipeId && !j.isComplete && !j.isCancelled))
                return ActionResult.Blocked("job_active", "kitchen.job_active");

            // CR3-02: was a single-pass loop that called _inventory.RemoveById
            // before checking the next iteration's CountById. If a later req
            // failed, prior decrement(s) were not rolled back. Make this
            // atomic: pre-check ALL required counts first; only consume
            // when every required count is satisfiable.
            if (inputRequirements != null)
            {
                foreach (var req in inputRequirements)
                {
                    if (_inventory.CountById(req.Key) < req.Value)
                        return ActionResult.Blocked("insufficient_ingredients", "kitchen.insufficient_ingredients");
                }
            }

            var reservedIds = new List<string>();
            var reservedCounts = new List<int>();
            if (inputRequirements != null)
            {
                foreach (var req in inputRequirements)
                {
                    _inventory.RemoveById(req.Key, req.Value);
                    reservedIds.Add(req.Key);
                    reservedCounts.Add(req.Value);
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
                totalHoursRequired = 2f
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

            foreach (var job in _state.activeJobs)
            {
                if (job.isComplete || job.isCancelled) continue;

                job.progressHours += 8f; // standard work day
                if (job.progressHours >= job.totalHoursRequired)
                {
                    job.isComplete = true;
                    job.portionsProduced = 3; // catalog-defined portions
                    _state.totalMealsPrepared += job.portionsProduced;
                    _log.Info($"[Kitchen] {job.recipeId} complete: {job.portionsProduced} portions");

                    // Add to pantry
                    var pantryItem = new PantryItem
                    {
                        itemId = job.recipeId,
                        displayName = job.recipeId,
                        portionCount = job.portionsProduced,
                        spoilageTimer = GetSpoilageDays(job.recipeId),
                        preservation = GetPreservationMethod(job.recipeId)
                    };
                    _state.pantry.Add(pantryItem);

                    OnJobCompleted?.Invoke(job);
                }
            }

            // Update spoilage
            UpdateSpoilage();

            // CR3-05: evict terminally-finished jobs from the underlying list.
            // GetActiveJobs already filters on read, but without this RemoveAll
            // the list serialises every completed job into every save and grows
            // without bound across long campaigns. Mirrors ArchiveDeskSystem
            // and MentalHealthCrisisSystem patterns.
            _state.activeJobs.RemoveAll(j => j.isComplete || j.isCancelled);

            OnKitchenChanged?.Invoke();
        }

        public ActionResult ServeMeal(string survivorId, string recipeId)
        {
            var pantryItem = _state.pantry.Find(p => p.itemId == recipeId && p.portionCount > 0 && !p.isSpoiled);
            if (pantryItem == null)
                return ActionResult.Blocked("no_meal", "kitchen.no_meal");

            pantryItem.portionCount--;

            // Calculate nutrition/morale effect
            float safetyRoll = (float)_rng.NextDouble();
            bool wasSafe = safetyRoll < 0.9f; // 90% safe

            float moraleBonus = wasSafe ? 5f : -5f;
            float nutritionScore = wasSafe ? 8f : 2f;

            if (!wasSafe)
            {
                _needs.Modify(survivorId, NeedKind.Health, -5f);
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
                    { "nutrition", nutritionScore }
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
