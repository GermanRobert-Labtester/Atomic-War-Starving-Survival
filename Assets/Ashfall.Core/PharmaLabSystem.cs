using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL — Pharmaceutical Laboratory System.
    ///
    /// Domain layer over CraftingSystem, RecipeCatalog, ChemicalDependencySystem,
    /// and shared inventory. Defines reagent sets, station requirements, heating/
    /// distillation phases, purity bands, output quantities, and dependency/addiction
    /// risk in versioned data.
    ///
    /// Reserves inputs at job start, releases/refunds on cancellation, and delivers
    /// outputs only once on completion. Uses ISeededRng for purity/contamination rolls.
    /// </summary>
    [Serializable]
    public sealed class PharmaLabState
    {
        public string systemId = PharmaLabSystem.SystemId;
        public bool isProcessing;
        public string currentRecipeId = string.Empty;
        public string assignedChemistId = string.Empty;
        public float progressHours;
        public float hoursRequired;
        public PharmaPhase currentPhase;
        public float temperature;
        public float purity;
        public float contaminationRisk;
        public List<string> reservedInputIds = new List<string>();
        public List<int> reservedInputAmounts = new List<int>();
        public List<string> completedRecipeIds = new List<string>();
        public int totalBatchesProduced;
        public int totalDependencyEvents;
    }

    public enum PharmaPhase { Idle, Mixing, Heating, Distillation, Cooling, Purification, Complete }

    [Serializable]
    public sealed class PharmaRecipe
    {
        public string recipe_id = string.Empty;
        public string display_name = string.Empty;
        public List<string> input_ids = new List<string>();
        public List<int> input_amounts = new List<int>();
        public string output_item_id = string.Empty;
        public int output_amount = 1;
        public float base_hours = 2f;
        public float required_temperature = 80f;
        public float purity_target = 0.9f;
        public float dependency_risk; // 0-1, chance of addiction event
        public string required_station = "pharma_bench";
        public string category = "pharmaceutical";
    }

    [Serializable]
    public sealed class PharmaRecipeCatalog
    {
        public string schema_version = "1.0";
        public List<PharmaRecipe> recipes = new List<PharmaRecipe>();
    }

    public sealed class PharmaLabSystem
    {
        public const string SystemId = "pharma_lab";

        private PharmaLabState _state = new PharmaLabState();
        private readonly Dictionary<string, PharmaRecipe> _recipes = new Dictionary<string, PharmaRecipe>(StringComparer.Ordinal);
        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private Func<string, float> _getChemistSkill;

        public PharmaLabState State => _state;
        public IReadOnlyDictionary<string, PharmaRecipe> Recipes => _recipes;
        public bool IsProcessing => _state.isProcessing;

        public event Action<ActionResult> OnBatchCompleted;
        public event Action<float> OnDependencyRisk; // parameter = risk level
        public event Action OnPharmaStateChanged;

        public PharmaLabSystem(Inventory.Inventory inventory, ISeededRng rng, ILog log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _getChemistSkill = (_) => 1.0f;
        }

        public void BindSkillEvaluator(Func<string, float> evaluator)
        {
            _getChemistSkill = evaluator ?? ((_) => 1.0f);
        }

        public void LoadCatalog(PharmaRecipeCatalog catalog)
        {
            if (catalog?.recipes == null) return;
            _recipes.Clear();
            foreach (var r in catalog.recipes)
            {
                if (!string.IsNullOrEmpty(r.recipe_id))
                    _recipes[r.recipe_id] = r;
            }
            _log.Info($"[PharmaLab] loaded {_recipes.Count} recipes");
        }

        public void RegisterRecipe(PharmaRecipe recipe)
        {
            if (recipe == null || string.IsNullOrEmpty(recipe.recipe_id)) return;
            _recipes[recipe.recipe_id] = recipe;
        }

        public PharmaRecipe? GetRecipe(string id)
        {
            _recipes.TryGetValue(id, out var r);
            return r;
        }

        // ── Actions ──────────────────────────────────────────────────────────

        public ActionResult StartBatch(string recipeId, string chemistId)
        {
            if (_state.isProcessing)
                return ActionResult.Blocked("lab_busy", "pharma.already_processing");
            if (!_recipes.TryGetValue(recipeId, out var recipe))
                return ActionResult.Failed("unknown_recipe", "pharma.unknown_recipe");

            // Check inputs
            for (int i = 0; i < recipe.input_ids.Count; i++)
            {
                if (_inventory.CountById(recipe.input_ids[i]) < recipe.input_amounts[i])
                    return ActionResult.Blocked("missing_inputs", "pharma.missing_inputs");
            }

            // Reserve inputs
            for (int i = 0; i < recipe.input_ids.Count; i++)
            {
                _inventory.RemoveById(recipe.input_ids[i], recipe.input_amounts[i]);
                _state.reservedInputIds.Add(recipe.input_ids[i]);
                _state.reservedInputAmounts.Add(recipe.input_amounts[i]);
            }

            float skill = _getChemistSkill(chemistId ?? string.Empty);
            _state.currentRecipeId = recipeId;
            _state.assignedChemistId = chemistId ?? string.Empty;
            _state.isProcessing = true;
            _state.progressHours = 0f;
            _state.hoursRequired = recipe.base_hours / skill;
            _state.currentPhase = PharmaPhase.Mixing;
            _state.temperature = 20f;
            _state.purity = 0f;
            _state.contaminationRisk = 0f;

            _log.Info($"[PharmaLab] started '{recipeId}' ({_state.hoursRequired}h, chemist={chemistId})");
            OnPharmaStateChanged?.Invoke();
            return ActionResult.Success("pharma.batch_started",
                new Dictionary<string, double> { { "hours", _state.hoursRequired } });
        }

        public ActionResult TickProgress(float hours)
        {
            if (!_state.isProcessing)
                return ActionResult.Blocked("not_processing", "pharma.not_processing");

            _state.progressHours += hours;

            // Advance through phases
            float total = _state.hoursRequired;
            float progress = _state.progressHours / total;

            if (progress < 0.2f) _state.currentPhase = PharmaPhase.Mixing;
            else if (progress < 0.4f) { _state.currentPhase = PharmaPhase.Heating; _state.temperature = 40f + progress * 100f; }
            else if (progress < 0.6f) { _state.currentPhase = PharmaPhase.Distillation; _state.temperature = 80f; }
            else if (progress < 0.8f) { _state.currentPhase = PharmaPhase.Cooling; _state.temperature = Math.Max(20f, _state.temperature - 10f); }
            else if (progress < 1.0f) _state.currentPhase = PharmaPhase.Purification;
            else return CompleteBatch();

            OnPharmaStateChanged?.Invoke();
            return ActionResult.Success("pharma.progress",
                new Dictionary<string, double>
                {
                    { "progress", _state.progressHours },
                    { "required", _state.hoursRequired },
                    { "phase", (int)_state.currentPhase },
                    { "temperature", _state.temperature }
                });
        }

        private ActionResult CompleteBatch()
        {
            if (!_recipes.TryGetValue(_state.currentRecipeId, out var recipe))
                return ActionResult.Failed("missing_recipe", "pharma.error");

            _state.currentPhase = PharmaPhase.Complete;

            // Purity roll
            float skill = _getChemistSkill(_state.assignedChemistId);
            _state.purity = (float)_rng.NextDouble() * 0.3f + 0.6f + skill * 0.1f;
            _state.purity = Math.Min(1f, Math.Max(0.1f, _state.purity));

            // Contamination risk
            _state.contaminationRisk = recipe.dependency_risk * (1f - _state.purity);

            int outputAmount = _state.purity >= recipe.purity_target
                ? recipe.output_amount
                : Math.Max(1, recipe.output_amount / 2);

            // Deliver output
            _inventory.AddById(recipe.output_item_id, outputAmount);

            _state.completedRecipeIds.Add(recipe.recipe_id);
            _state.totalBatchesProduced++;

            // Dependency risk
            if (_state.contaminationRisk > 0 && _rng.NextDouble() < _state.contaminationRisk)
            {
                _state.totalDependencyEvents++;
                OnDependencyRisk?.Invoke(_state.contaminationRisk);
            }

            var deltas = new Dictionary<string, double>
            {
                { recipe.output_item_id, outputAmount },
                { "purity", _state.purity },
                { "contamination_risk", _state.contaminationRisk }
            };

            _state.isProcessing = false;
            _state.reservedInputIds.Clear();
            _state.reservedInputAmounts.Clear();

            _log.Info($"[PharmaLab] completed '{_state.currentRecipeId}': {outputAmount}x {recipe.output_item_id} (purity={_state.purity:F2})");
            var result = ActionResult.Success("pharma.batch_complete", deltas);
            OnBatchCompleted?.Invoke(result);
            OnPharmaStateChanged?.Invoke();
            return result;
        }

        public ActionResult CancelBatch()
        {
            if (!_state.isProcessing)
                return ActionResult.Blocked("not_processing", "pharma.not_processing");

            // Refund inputs (partial — some may be consumed)
            for (int i = 0; i < _state.reservedInputIds.Count; i++)
            {
                _inventory.AddById(_state.reservedInputIds[i], _state.reservedInputAmounts[i]);
            }

            _state.isProcessing = false;
            _state.reservedInputIds.Clear();
            _state.reservedInputAmounts.Clear();
            _state.currentPhase = PharmaPhase.Idle;

            OnPharmaStateChanged?.Invoke();
            return ActionResult.Success("pharma.batch_cancelled");
        }

        // ── Persistence ──────────────────────────────────────────────────────

        public PharmaLabState CaptureState() => _state;
        public void RestoreState(PharmaLabState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnPharmaStateChanged?.Invoke();
        }
    }
}
