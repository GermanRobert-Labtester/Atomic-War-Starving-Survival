// SPDX-License-Identifier: MIT
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
        public string output_item_id = "item_grain_flour";
        public int output_quantity = 1;
        public float processing_hours = 8f;
    }

    [Serializable]
    public sealed class GrainSiloDefinition
    {
        public string silo_id = string.Empty;
        public float integrity = 100f;
        public float moisture_pct = 12f;
    }

    [Serializable]
    public sealed class GrainProcessingCatalog
    {
        public int schema_version = 1;
        public List<GrainProcessingRecipe> recipes = new List<GrainProcessingRecipe>();
        public List<GrainSiloDefinition> silos = new List<GrainSiloDefinition>();
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
            new Dictionary<string, GrainProcessingRecipe>(StringComparer.OrdinalIgnoreCase);
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
            if (recipe == null || string.IsNullOrWhiteSpace(recipe.recipe_id)
                || string.IsNullOrWhiteSpace(recipe.input_item_id)
                || string.IsNullOrWhiteSpace(recipe.output_item_id)
                || recipe.input_quantity <= 0
                || recipe.output_quantity <= 0
                || !IsFinite(recipe.processing_hours)
                || recipe.processing_hours < MinimumProcessingHours)
                return;

            string recipeId = recipe.recipe_id.Trim();
            _recipes[recipeId] = new GrainProcessingRecipe
            {
                recipe_id = recipeId,
                input_item_id = recipe.input_item_id.Trim(),
                input_quantity = recipe.input_quantity,
                output_item_id = recipe.output_item_id.Trim(),
                output_quantity = recipe.output_quantity,
                processing_hours = recipe.processing_hours
            };
        }

        public void LoadCatalog(GrainProcessingCatalog catalog)
        {
            if (catalog == null) return;
            if (catalog.recipes != null)
            {
                foreach (var recipe in catalog.recipes)
                    RegisterRecipe(recipe);
            }
            if (catalog.silos != null)
            {
                foreach (var silo in catalog.silos)
                {
                    if (silo == null) continue;
                    RegisterSilo(silo.silo_id, silo.integrity, silo.moisture_pct);
                }
            }
        }

        public GrainProcessingRecipe? GetRecipe(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId)) return null;
            _recipes.TryGetValue(recipeId.Trim(), out var recipe);
            return recipe;
        }

        public bool RegisterSilo(string siloId, float integrity = 100f, float moisturePct = 12f)
        {
            if (string.IsNullOrWhiteSpace(siloId)) return false;
            string canonicalSiloId = siloId.Trim();
            if (FindSilo(canonicalSiloId) != null) return false;

            _state.silos.Add(new GrainSiloState
            {
                silo_id = canonicalSiloId,
                integrity = SanitizeRange(integrity, 0f, 100f),
                moisture_pct = SanitizeRange(moisturePct, 0f, 100f),
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
            if (string.IsNullOrWhiteSpace(treatmentItemId)
                || treatmentQuantity <= 0
                || !IsFinite(pestReduction)
                || pestReduction <= 0f)
                return ActionResult.Blocked("invalid_treatment", "grain.invalid_treatment");

            var bill = new InventoryBill();
            bill.AddCost(treatmentItemId.Trim(), treatmentQuantity);
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
            if (day < 0 || (_state.last_tick_day >= 0 && day <= _currentDay)) return;
            _currentDay = day;
            _state.last_tick_day = day;

            for (int i = 0; i < _state.silos.Count; i++)
            {
                var silo = _state.silos[i];
                if (silo == null || string.IsNullOrWhiteSpace(silo.silo_id)) continue;
                silo.integrity = SanitizeRange(silo.integrity, 0f, 100f);
                silo.moisture_pct = SanitizeRange(silo.moisture_pct, 0f, 100f);
                silo.pest_pressure = SanitizeRange(silo.pest_pressure, 0f, 100f);
                var previousBand = GetSafetyBand(silo);
                long elapsedLong = silo.last_tick_day < 0 ? 0 : (long)day - silo.last_tick_day;
                int elapsed = elapsedLong <= 0
                    ? 0
                    : elapsedLong >= int.MaxValue ? int.MaxValue : (int)elapsedLong;
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
            jobs.RemoveAll(job => job == null || string.IsNullOrWhiteSpace(job.job_id));
            jobs.Sort((a, b) => string.CompareOrdinal(a.job_id, b.job_id));
            for (int i = 0; i < jobs.Count; i++)
            {
                var job = jobs[i];
                if (job.is_complete) continue;
                var recipe = GetRecipe(job.recipe_id);
                var silo = FindSilo(job.silo_id);
                if (recipe == null || silo == null)
                {
                    job.is_blocked = true;
                    OnJobBlocked?.Invoke(job);
                    continue;
                }

                job.progress_hours = SanitizeRange(job.progress_hours, 0f, float.MaxValue);
                job.total_hours_required = Math.Max(
                    MinimumProcessingHours,
                    SanitizeRange(job.total_hours_required, MinimumProcessingHours, float.MaxValue));
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
                if (_state.total_batches_completed < int.MaxValue)
                    _state.total_batches_completed++;
                long totalOutput = (long)_state.total_output_granted + output;
                _state.total_output_granted = (int)Math.Min(totalOutput, int.MaxValue);
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
            if (string.IsNullOrWhiteSpace(siloId)) return null;
            string canonicalSiloId = siloId.Trim();
            for (int i = 0; i < _state.silos.Count; i++)
            {
                var silo = _state.silos[i];
                if (silo != null
                    && string.Equals(silo.silo_id, canonicalSiloId, StringComparison.OrdinalIgnoreCase))
                    return silo;
            }
            return null;
        }

        private static GrainSiloSafetyBand GetSafetyBand(GrainSiloState silo)
        {
            float integrity = SanitizeRange(silo.integrity, 0f, 100f);
            float pestPressure = SanitizeRange(silo.pest_pressure, 0f, 100f);
            if (integrity <= 10f || pestPressure >= 80f)
                return GrainSiloSafetyBand.Critical;
            if (integrity <= 35f || pestPressure >= 50f)
                return GrainSiloSafetyBand.Infested;
            if (integrity <= 65f || pestPressure >= 20f)
                return GrainSiloSafetyBand.Watch;
            return GrainSiloSafetyBand.Safe;
        }

        private static int CalculateOutput(GrainProcessingRecipe recipe, GrainSiloState silo)
        {
            float integrity = SanitizeRange(silo.integrity, 0f, 100f);
            float pestPressure = SanitizeRange(silo.pest_pressure, 0f, 100f);
            if (integrity > 65f && pestPressure < 20f)
                return recipe.output_quantity;

            float pestLoss = SanitizeRange(pestPressure / 100f * 0.5f, 0f, 0.5f);
            float integrityLoss = SanitizeRange((100f - integrity) / 100f * 0.2f, 0f, 0.2f);
            float output = recipe.output_quantity * (1f - pestLoss - integrityLoss);
            if (!IsFinite(output)) return 0;
            return Math.Max(0, (int)Math.Floor(output));
        }

        private static GrainProcessingState CloneState(GrainProcessingState? source)
        {
            var copy = new GrainProcessingState
            {
                system_id = string.IsNullOrWhiteSpace(source?.system_id)
                    ? GrainProcessingSystem.SystemId
                    : source!.system_id,
                total_batches_completed = Math.Max(0, source?.total_batches_completed ?? 0),
                total_output_granted = Math.Max(0, source?.total_output_granted ?? 0),
                last_tick_day = source?.last_tick_day ?? -1,
                silos = new List<GrainSiloState>(),
                active_jobs = new List<GrainProcessingJob>()
            };
            if (source == null) return copy;

            var siloIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (source.silos != null)
            {
                foreach (var silo in source.silos)
                {
                    if (silo == null || string.IsNullOrWhiteSpace(silo.silo_id)
                        || !siloIds.Add(silo.silo_id.Trim())) continue;
                    copy.silos.Add(new GrainSiloState
                    {
                        silo_id = silo.silo_id.Trim(),
                        integrity = SanitizeRange(silo.integrity, 0f, 100f),
                        pest_pressure = SanitizeRange(silo.pest_pressure, 0f, 100f),
                        moisture_pct = SanitizeRange(silo.moisture_pct, 0f, 100f),
                        last_tick_day = silo.last_tick_day
                    });
                }
            }

            var jobIds = new HashSet<string>(StringComparer.Ordinal);
            if (source.active_jobs != null)
            {
                foreach (var job in source.active_jobs)
                {
                    if (job == null || string.IsNullOrWhiteSpace(job.job_id)
                        || !jobIds.Add(job.job_id.Trim())) continue;
                    copy.active_jobs.Add(new GrainProcessingJob
                    {
                        job_id = job.job_id.Trim(),
                        recipe_id = job.recipe_id?.Trim() ?? string.Empty,
                        silo_id = job.silo_id?.Trim() ?? string.Empty,
                        worker_id = job.worker_id?.Trim() ?? string.Empty,
                        day_started = job.day_started,
                        progress_hours = SanitizeRange(job.progress_hours, 0f, float.MaxValue),
                        total_hours_required = Math.Max(
                            MinimumProcessingHours,
                            SanitizeRange(job.total_hours_required, MinimumProcessingHours, float.MaxValue)),
                        is_complete = job.is_complete,
                        is_blocked = job.is_blocked,
                        output_granted = Math.Max(0, job.output_granted)
                    });
                }
            }
            return copy;
        }

        private static bool IsFinite(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value);

        private static float SanitizeRange(float value, float min, float max)
        {
            if (!IsFinite(value)) return min;
            return Math.Clamp(value, min, max);
        }
    }

    public static class GrainProcessingCatalogLoader
    {
        public const string FileName = "grain_processing.json";

        public static GrainProcessingCatalog Load(
            string directory,
            IFileIO fileIO,
            IJsonSerializer serializer,
            ILog? log = null)
        {
            var catalog = new GrainProcessingCatalog();
            if (fileIO == null || serializer == null || string.IsNullOrEmpty(directory))
                return catalog;

            string path = fileIO.Combine(directory, FileName);
            if (!fileIO.FileExists(path)) return catalog;
            try
            {
                return serializer.Deserialize<GrainProcessingCatalog>(fileIO.ReadAllText(path))
                    ?? catalog;
            }
            catch (Exception ex)
            {
                (log ?? NullLog.Instance).Warn($"[GrainProcessingCatalog] failed to load {path}: {ex.Message}");
                return catalog;
            }
        }
    }
}
