using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Medical
{
    [Serializable]
    public sealed class LyophilizationRecipeDefinition
    {
        public string recipe_id = string.Empty;
        public string display_name = string.Empty;
        public string input_item_id = string.Empty;
        public int input_amount = 1;
        public string container_item_id = string.Empty;
        public int container_amount = 1;
        public string output_item_id = string.Empty;
        public int output_amount = 1;
        public int duration_days = 2;
        public int shelf_life_days = 30;
        public float required_power_watts = 100f;
        public float base_viability01 = 0.8f;
        public float viability_variance01 = 0.1f;
        public string medical_category = "biologic";
    }

    [Serializable]
    public sealed class LyophilizationCatalog
    {
        public int schema_version = 1;
        public List<LyophilizationRecipeDefinition> recipes = new List<LyophilizationRecipeDefinition>();
    }

    [Serializable]
    public sealed class LyophilizedBatchRecord
    {
        public string batch_id = string.Empty;
        public string recipe_id = string.Empty;
        public string output_item_id = string.Empty;
        public int amount;
        public int created_day;
        public int expiry_day;
        public float viability01;
        public bool spoiled;
    }

    [Serializable]
    public sealed class LyophilizationState
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public string system_id = LyophilizationSystem.SystemId;
        public bool installed = true;
        public LyophilizationStatus status = LyophilizationStatus.Ready;
        public string active_recipe_id = string.Empty;
        public string active_batch_id = string.Empty;
        public int active_started_day;
        public int days_elapsed;
        public int days_required;
        public int completed_batches;
        public int viable_units_produced;
        public List<LyophilizedBatchRecord> batches = new List<LyophilizedBatchRecord>();
    }

    public enum LyophilizationStatus
    {
        Offline,
        Ready,
        Drying,
        PowerStarved,
        Complete,
        MaintenanceRequired
    }

    public static class LyophilizationCatalogLoader
    {
        public const string FileName = "lyophilization_catalog.json";

        public static LyophilizationCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            if (fileIO == null || json == null || string.IsNullOrWhiteSpace(dataDir))
                return new LyophilizationCatalog();
            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[Lyophilization] catalog not found at {path}");
                return new LyophilizationCatalog();
            }
            try
            {
                return json.Deserialize<LyophilizationCatalog>(fileIO.ReadAllText(path))
                    ?? new LyophilizationCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[Lyophilization] failed loading catalog: {ex.Message}");
                return new LyophilizationCatalog();
            }
        }
    }

    /// <summary>
    /// Owns the cold-drying job and viable biological batch ledger. The
    /// medical pipeline receives these batches through explicit protocol
    /// registration; it never receives a duplicate inventory authority.
    /// </summary>
    public class LyophilizationSystem
    {
        public const string SystemId = "lyophilization";

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly Func<float> _availablePowerWatts;
        private readonly ILog _log;
        private readonly Dictionary<string, LyophilizationRecipeDefinition> _recipes =
            new Dictionary<string, LyophilizationRecipeDefinition>(StringComparer.Ordinal);
        private LyophilizationState _state = new LyophilizationState();

        public LyophilizationState State => _state;
        public IReadOnlyDictionary<string, LyophilizationRecipeDefinition> Recipes => _recipes;
        public event Action? OnStateChanged;
        public event Action<LyophilizedBatchRecord>? OnBatchCompleted;

        public LyophilizationSystem(Inventory.Inventory inventory, ISeededRng? rng = null,
            Func<float>? availablePowerWatts = null, ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? new SeededRng(132);
            _availablePowerWatts = availablePowerWatts ?? (() => float.MaxValue);
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(LyophilizationCatalog catalog)
        {
            _recipes.Clear();
            foreach (var recipe in catalog?.recipes ?? new List<LyophilizationRecipeDefinition>())
            {
                if (recipe == null || string.IsNullOrEmpty(recipe.recipe_id)) continue;
                _recipes[recipe.recipe_id] = recipe;
            }
        }

        public LyophilizationRecipeDefinition? GetRecipe(string recipeId)
            => _recipes.TryGetValue(recipeId ?? string.Empty, out var recipe) ? recipe : null;

        public ActionResult StartBatch(string recipeId, int day)
        {
            if (!_state.installed) return ActionResult.Blocked("not_installed", "lyophilization.not_installed");
            if (_state.status == LyophilizationStatus.Drying)
                return ActionResult.Blocked("already_processing", "lyophilization.already_processing");
            var recipe = GetRecipe(recipeId);
            if (recipe == null) return ActionResult.Failed("unknown_recipe", "lyophilization.unknown_recipe");
            if (_availablePowerWatts() < Math.Max(0f, recipe.required_power_watts))
                return ActionResult.Blocked("insufficient_power", "lyophilization.insufficient_power");

            var bill = new InventoryBill()
                .AddCost(recipe.input_item_id, Math.Max(1, recipe.input_amount))
                .AddCost(recipe.container_item_id, Math.Max(1, recipe.container_amount));
            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("missing_inputs", "lyophilization.missing_inputs");

            _state.active_recipe_id = recipe.recipe_id;
            _state.active_batch_id = $"lyo_{Math.Max(0, day)}_{_state.completed_batches + 1}";
            _state.active_started_day = day;
            _state.days_elapsed = 0;
            _state.days_required = Math.Max(1, recipe.duration_days);
            _state.status = LyophilizationStatus.Drying;
            OnStateChanged?.Invoke();
            return ActionResult.Success("lyophilization.batch_started");
        }

        public ActionResult TickDay(int day)
        {
            if (_state.status != LyophilizationStatus.Drying)
                return ActionResult.Success("lyophilization.idle");
            var recipe = GetRecipe(_state.active_recipe_id);
            if (recipe == null)
                return ActionResult.Failed("invalid_recipe_state", "lyophilization.invalid_recipe_state");
            if (_availablePowerWatts() < Math.Max(0f, recipe.required_power_watts))
            {
                _state.status = LyophilizationStatus.PowerStarved;
                OnStateChanged?.Invoke();
                return ActionResult.Blocked("insufficient_power", "lyophilization.power_starved");
            }

            _state.status = LyophilizationStatus.Drying;
            _state.days_elapsed++;
            if (_state.days_elapsed < _state.days_required)
            {
                OnStateChanged?.Invoke();
                return ActionResult.Success("lyophilization.progressed");
            }

            int amount = Math.Max(1, recipe.output_amount);
            if (!_inventory.TryProduce(recipe.output_item_id, amount))
            {
                _state.status = LyophilizationStatus.MaintenanceRequired;
                OnStateChanged?.Invoke();
                return ActionResult.Blocked("storage_full", "lyophilization.storage_full");
            }

            float viability = Math.Clamp(
                recipe.base_viability01 + ((float)_rng.NextDouble() * 2f - 1f) * recipe.viability_variance01,
                0f, 1f);
            var batch = new LyophilizedBatchRecord
            {
                batch_id = _state.active_batch_id,
                recipe_id = recipe.recipe_id,
                output_item_id = recipe.output_item_id,
                amount = amount,
                created_day = day,
                expiry_day = day + Math.Max(1, recipe.shelf_life_days),
                viability01 = viability,
                spoiled = false
            };
            _state.batches.Add(batch);
            _state.completed_batches++;
            _state.viable_units_produced += amount;
            _state.active_recipe_id = string.Empty;
            _state.active_batch_id = string.Empty;
            _state.days_elapsed = 0;
            _state.days_required = 0;
            _state.status = LyophilizationStatus.Complete;
            _log.Info($"[Lyophilization] completed {batch.recipe_id}; viability {viability:0.00}");
            OnBatchCompleted?.Invoke(batch);
            OnStateChanged?.Invoke();
            return ActionResult.Success("lyophilization.batch_completed",
                new Dictionary<string, double> { ["viability01"] = viability, ["output_units"] = amount });
        }

        public bool CanUseBatch(string batchId, int day, int amount = 1)
        {
            var batch = FindBatch(batchId);
            return batch != null && !batch.spoiled && batch.amount >= amount
                && day <= batch.expiry_day && batch.viability01 > 0f;
        }

        public bool TryUseBatch(string batchId, int day, int amount, out string outputItemId, out string reasonCode)
        {
            outputItemId = string.Empty;
            reasonCode = string.Empty;
            var batch = FindBatch(batchId);
            if (batch == null) { reasonCode = "unknown_batch"; return false; }
            if (day > batch.expiry_day) { batch.spoiled = true; reasonCode = "expired"; OnStateChanged?.Invoke(); return false; }
            if (batch.spoiled || batch.viability01 <= 0f) { reasonCode = "spoiled"; return false; }
            if (amount <= 0 || batch.amount < amount) { reasonCode = "insufficient_batch"; return false; }
            batch.amount -= amount;
            outputItemId = batch.output_item_id;
            if (batch.amount == 0) _state.batches.Remove(batch);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Registers a zero-cost medical protocol whose scarce input is the
        /// preserved batch ledger. The coordinator still owns the protocol
        /// transaction and event ordering.
        /// </summary>
        public bool RegisterMedicalProtocol(
            MedicalPipelineCoordinator pipeline,
            string protocolId,
            string batchId,
            int amount = 1,
            Func<int>? currentDay = null)
        {
            if (pipeline == null || string.IsNullOrEmpty(protocolId) || string.IsNullOrEmpty(batchId))
                return false;
            pipeline.RegisterProtocol(new LyophilizedMedicalProtocol(
                protocolId, batchId, amount, this, currentDay ?? (() => 0)));
            return true;
        }

        private LyophilizedBatchRecord? FindBatch(string batchId)
            => _state.batches.FirstOrDefault(b => b != null && b.batch_id == batchId);

        public LyophilizationState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<LyophilizationState>(serializer.Serialize(_state))
                ?? new LyophilizationState();
        }

        public void RestoreState(LyophilizationState? state)
        {
            if (state == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<LyophilizationState>(serializer.Serialize(state))
                ?? new LyophilizationState();
            _state.batches ??= new List<LyophilizedBatchRecord>();
            OnStateChanged?.Invoke();
        }

        private sealed class LyophilizedMedicalProtocol : IMedicalProtocolHandler
        {
            private readonly string _id;
            private readonly string _batchId;
            private readonly int _amount;
            private readonly LyophilizationSystem _system;
            private readonly Func<int> _day;

            public LyophilizedMedicalProtocol(string id, string batchId, int amount,
                LyophilizationSystem system, Func<int> day)
            {
                _id = id;
                _batchId = batchId;
                _amount = Math.Max(1, amount);
                _system = system;
                _day = day;
            }

            public string ProtocolId => _id;
            public string DisplayName => "Apply preserved biologic";
            public IReadOnlyDictionary<string, int> ItemCosts { get; } =
                new Dictionary<string, int>(StringComparer.Ordinal);
            public string? Validate() => _system.CanUseBatch(_batchId, _day(), _amount)
                ? null : "preserved_batch_unavailable";
            public bool Apply()
            {
                return _system.TryUseBatch(_batchId, _day(), _amount, out _, out _);
            }
        }
    }

    public class LyophilizationEngine : LyophilizationSystem
    {
        public LyophilizationEngine(Inventory.Inventory inventory, ISeededRng? rng = null,
            Func<float>? availablePowerWatts = null, ILog? log = null)
            : base(inventory, rng, availablePowerWatts, log) { }
    }
}
