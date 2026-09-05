using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core
{
    public enum GrainSiloSafetyBand
    {
        Safe,
        Watch,
        Infested,
        Critical
    }

    [Serializable]
    public sealed class GrainProcessingRecipe
    {
        public string recipe_id = string.Empty;
        public string input_item_id = "crop_ash_grain";
        public int input_quantity = 1;
        public string output_item_id = "grain_flour";
        public int output_quantity = 1;
        public float processing_hours = 8f;
    }

    [Serializable]
    public sealed class GrainSiloState
    {
        public string silo_id = string.Empty;
        public float integrity = 100f;
        public float pest_pressure;
        public float moisture_pct = 12f;
        public int last_tick_day = -1;
    }

    [Serializable]
    public sealed class GrainProcessingJob
    {
        public string job_id = string.Empty;
        public string recipe_id = string.Empty;
        public string silo_id = string.Empty;
        public string worker_id = string.Empty;
        public int day_started = -1;
        public float progress_hours;
        public float total_hours_required = 8f;
        public bool is_complete;
        public bool is_blocked;
        public int output_granted;
    }

    [Serializable]
    public sealed class GrainProcessingState
    {
        public string system_id = GrainProcessingSystem.SystemId;
        public List<GrainSiloState> silos = new List<GrainSiloState>();
        public List<GrainProcessingJob> active_jobs = new List<GrainProcessingJob>();
        public int total_batches_completed;
        public int total_output_granted;
        public int last_tick_day = -1;
    }

    /// <summary>
    /// Abstract grain milling and silo-safety authority. Inventory remains the
    /// only quantity owner: jobs consume grain and grant flour through atomic
    /// inventory transactions. Silo moisture, pests, and integrity are
    /// bounded gameplay bands rather than real storage instrumentation.
    /// </summary>
    public sealed class GrainProcessingSystem
    {
        public const string SystemId = "grain_processing";
        public const float DailyPestGrowth = 1.5f;
        public const float MoisturePestFactor = 0.08f;
        public const float MinimumProcessingHours = 1f;

        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;
        private readonly Dictionary<string, GrainProcessingRecipe> _recipes =
            new Dictionary<string, GrainProcessingRecipe>(StringComparer.Ordinal);
        private GrainProcessingState _state = new GrainProcessingState();
        private int _currentDay;

        public GrainProcessingState State => _state;
        public IReadOnlyDictionary<string, GrainProcessingRecipe> Recipes => _recipes;

        public event Action<GrainProcessingJob>? OnJobCompleted;
        public event Action<GrainProcessingJob>? OnJobBlocked;
        public event Action<string, GrainSiloSafetyBand>? OnSiloBandChanged;
        public event Action? OnStateChanged;

        public GrainProcessingSystem(Inventory.Inventory inventory, ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _log = log ?? NullLog.Instance;
        }

        public void RegisterRecipe(GrainProcessingRecipe recipe)
        {
            if (recipe == null || string.IsNullOrEmpty(recipe.recipe_id)
                || string.IsNullOrEmpty(recipe.input_item_id)
                || string.IsNullOrEmpty(recipe.output_item_id)
                || recipe.input_quantity <= 0
                || recipe.output_quantity <= 0
                || recipe.processing_hours < MinimumProcessingHours)
                return;

            _recipes[recipe.recipe_id] = recipe;
        }

        public GrainProcessingRecipe? GetRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return null;
            _recipes.TryGetValue(recipeId, out var recipe);
            return recipe;
        }

        public bool RegisterSilo(string siloId, float integrity = 100f, float moisturePct = 12f)
        {
            if (string.IsNullOrEmpty(siloId)) return false;
            if (FindSilo(siloId) != null) return false;

            _state.silos.Add(new GrainSiloState
            {
                silo_id = siloId,
                integrity = Math.Clamp(integrity, 0f, 100f),
                moisture_pct = Math.Clamp(moisturePct, 0f, 100f),
                last_tick_day = _currentDay
            });
            OnStateChanged?.Invoke();
            return true;
        }

        public GrainSiloState? GetSilo(string siloId) => FindSilo(siloId);

        public GrainSiloSafetyBand GetSafetyBand(string siloId)
        {
            var silo = FindSilo(siloId);
            return silo == null ? GrainSiloSafetyBand.Critical : GetSafetyBand(silo);
        }

        public ActionResult StartMilling(string recipeId, string siloId, string workerId = "")
        {
            var recipe = GetRecipe(recipeId);
            var silo = FindSilo(siloId);
            if (recipe == null)
                return ActionResult.Failed("unknown_recipe", "grain.unknown_recipe");
            if (silo == null)
                return ActionResult.Failed("unknown_silo", "grain.unknown_silo");
            if (GetSafetyBand(silo) == GrainSiloSafetyBand.Critical)
                return ActionResult.Blocked("silo_unsafe", "grain.silo_unsafe");
            if (_state.active_jobs.Exists(j => j.silo_id == siloId && !j.is_complete))
                return ActionResult.Blocked("silo_busy", "grain.silo_busy");

            var bill = new InventoryBill();
            bill.AddCost(recipe.input_item_id, recipe.input_quantity);
            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                _state.active_jobs.Add(new GrainProcessingJob
                {
                    job_id = $"grain_{_currentDay}_{recipeId}_{siloId}",
                    recipe_id = recipeId,
                    silo_id = siloId,
                    worker_id = workerId ?? string.Empty,
                    day_started = _currentDay,
                    total_hours_required = recipe.processing_hours
                });
            });
            if (!committed)
                return ActionResult.Blocked("insufficient_grain", "grain.insufficient_grain");

            OnStateChanged?.Invoke();
            return ActionResult.Success("grain.milling_started");
        }

        /// <summary>
        /// Consume a treatment item and reduce pest pressure atomically.
        /// </summary>
        public ActionResult TreatSilo(
            string siloId,
            string treatmentItemId,
            int treatmentQuantity,
            float pestReduction)
        {
            var silo = FindSilo(siloId);
            if (silo == null)
                return ActionResult.Failed("unknown_silo", "grain.unknown_silo");
            if (string.IsNullOrEmpty(treatmentItemId) || treatmentQuantity <= 0 || pestReduction <= 0f)
                return ActionResult.Blocked("invalid_treatment", "grain.invalid_treatment");

            var bill = new InventoryBill();
            bill.AddCost(treatmentItemId, treatmentQuantity);
            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                silo.pest_pressure = Math.Max(0f, silo.pest_pressure - pestReduction);
            });
            if (!committed)
                return ActionResult.Blocked("insufficient_treatment", "grain.insufficient_treatment");

            OnStateChanged?.Invoke();
            return ActionResult.Success("grain.silo_treated",
                new Dictionary<string, double> { { "pest_pressure", silo.pest_pressure } });
        }

        public void TickDay(int day)
        {
            if (day < _currentDay) return;
            _currentDay = day;
            _state.last_tick_day = day;

            for (int i = 0; i < _state.silos.Count; i++)
            {
                var silo = _state.silos[i];
                var previousBand = GetSafetyBand(silo);
                int elapsed = silo.last_tick_day < 0 ? 0 : Math.Max(0, day - silo.last_tick_day);
                if (elapsed > 0)
                {
                    float moisturePenalty = Math.Max(0f, silo.moisture_pct - 12f) * MoisturePestFactor;
                    silo.pest_pressure = Math.Clamp(
                        silo.pest_pressure + elapsed * (DailyPestGrowth + moisturePenalty),
                        0f,
                        100f);
                }
                silo.last_tick_day = day;
                var nextBand = GetSafetyBand(silo);
                if (nextBand != previousBand)
                    OnSiloBandChanged?.Invoke(silo.silo_id, nextBand);
            }

            var jobs = new List<GrainProcessingJob>(_state.active_jobs);
            jobs.Sort((a, b) => string.CompareOrdinal(a.job_id, b.job_id));
            for (int i = 0; i < jobs.Count; i++)
            {
                var job = jobs[i];
                if (job.is_complete) continue;
                var recipe = GetRecipe(job.recipe_id);
                var silo = FindSilo(job.silo_id);
                if (recipe == null || silo == null) continue;

                if (job.progress_hours < job.total_hours_required)
                    job.progress_hours += 8f;
                if (job.progress_hours < job.total_hours_required) continue;

                int output = CalculateOutput(recipe, silo);
                var bill = new InventoryBill();
                if (output > 0) bill.AddGrant(recipe.output_item_id, output);
                bool granted = _inventory.TryExecuteTransaction(bill, () => { });
                if (!granted)
                {
                    job.is_blocked = true;
                    OnJobBlocked?.Invoke(job);
                    continue;
                }

                job.is_complete = true;
                job.is_blocked = false;
                job.output_granted = output;
                _state.total_batches_completed++;
                _state.total_output_granted += output;
                _log.Info($"[Grain] {job.recipe_id} completed: {output} output units");
                OnJobCompleted?.Invoke(job);
            }

            _state.active_jobs.RemoveAll(j => j.is_complete);
            OnStateChanged?.Invoke();
        }

        public GrainProcessingState CaptureState() => CloneState(_state);

        public void RestoreState(GrainProcessingState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            _currentDay = _state.last_tick_day;
        }

        private GrainSiloState? FindSilo(string siloId)
        {
            if (string.IsNullOrEmpty(siloId)) return null;
            for (int i = 0; i < _state.silos.Count; i++)
                if (_state.silos[i].silo_id == siloId) return _state.silos[i];
            return null;
        }

        private static GrainSiloSafetyBand GetSafetyBand(GrainSiloState silo)
        {
            if (silo.integrity <= 10f || silo.pest_pressure >= 80f)
                return GrainSiloSafetyBand.Critical;
            if (silo.integrity <= 35f || silo.pest_pressure >= 50f)
                return GrainSiloSafetyBand.Infested;
            if (silo.integrity <= 65f || silo.pest_pressure >= 20f)
                return GrainSiloSafetyBand.Watch;
            return GrainSiloSafetyBand.Safe;
        }

        private static int CalculateOutput(GrainProcessingRecipe recipe, GrainSiloState silo)
        {
            if (silo.integrity > 65f && silo.pest_pressure < 20f)
                return recipe.output_quantity;

            float pestLoss = Math.Clamp(silo.pest_pressure / 100f * 0.5f, 0f, 0.5f);
            float integrityLoss = Math.Clamp((100f - silo.integrity) / 100f * 0.2f, 0f, 0.2f);
            return Math.Max(0, (int)Math.Floor(recipe.output_quantity * (1f - pestLoss - integrityLoss)));
        }

        private static GrainProcessingState CloneState(GrainProcessingState src)
        {
            if (src == null) return new GrainProcessingState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(src);
            return serializer.Deserialize<GrainProcessingState>(json) ?? new GrainProcessingState();
        }
    }
}
