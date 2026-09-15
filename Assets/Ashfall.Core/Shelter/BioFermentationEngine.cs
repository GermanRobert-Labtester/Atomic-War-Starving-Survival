// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 126 — Subterranean Biological Fermentation & Organic-Acid Production
// Core engine: reactor machine/process state, culture & process health,
// aeration/temperature/acidity bands, contamination events, batch progress,
// yield quality, filter wear and maintenance. Outputs are items granted
// through the canonical Inventory; the engine never mutates food, soil,
// or any foreign authority. All stochastic outcomes use the injected
// ISeededRng with a fixed draw order (see TickDay): one contamination roll
// per active tick, then one completion-quality roll on the tick the batch
// completes. HarvestBatch performs no draws and is idempotent-safe.
//
// Safety abstraction rule: game-level process bands only — no culturing,
// sterilization, or organism-handling procedures are encoded here.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum BioFermentationPhase
    {
        Unbuilt = 0,
        Idle = 1,
        SterilizedReady = 2,
        Inoculated = 3,
        Fermenting = 4,
        Conditioning = 5,
        Separation = 6,
        Complete = 7,
        Contaminated = 8,
        Failed = 9,
        MaintenanceRequired = 10,
    }

    [Serializable]
    public sealed class BioFermentationFeedstockCharge
    {
        public string item_id = string.Empty;
        public int units;
    }

    /// <summary>
    /// Persisted fermentation machine state. Gameplay bands only; no
    /// high-resolution biological telemetry. batch_progress, yield_quality
    /// and all outcomes are captured so a reload never re-rolls or
    /// duplicates a completed batch.
    /// </summary>
    [Serializable]
    public sealed class BioFermentationState
    {
        public string SystemId = BioFermentationEngine.SystemId;
        public string reactor_id = "bio_ferm_reactor_01";
        public int phase = (int)BioFermentationPhase.Unbuilt;
        public string process_id = string.Empty;
        public float culture_health;       // 0..100
        public float process_health;       // 0..100
        public bool aeration_state;
        public string temperature_state = "cold";   // cold|cool|moderate|warm|hot
        public string acidity_state = "neutral";    // neutral|weak|mild|moderate|high
        public string contamination_state = "clear"; // clear|suspect|contaminated|spoiled
        public float batch_progress;       // 0..1
        public float yield_quality = -1f;  // -1 = unresolved (only meaningful when Complete)
        public string current_feedstock_batch_id = string.Empty;
        public List<BioFermentationFeedstockCharge> staged_feedstock = new List<BioFermentationFeedstockCharge>();
        public List<BioFermentationFeedstockCharge> committed_feedstock = new List<BioFermentationFeedstockCharge>();
        public string committed_starter_item_id = string.Empty;
        public float filter_condition = 100f; // 0..100
        public string fault_state = string.Empty; // ""|filter_clogged|power_loss|contaminated_batch
        public int cycle_count;
        public int last_service_day = -1;
        public string operator_id = string.Empty;
        public List<string> operator_trait_ids = new List<string>();
        public float operator_skill = 0.5f;
        public int day_started = -1;
        public int phase_days_elapsed;
    }

    // ── Catalog DTOs (snake_case to match the JSON authority exactly) ──────

    [Serializable]
    public sealed class BioFermentationReactorConfig
    {
        public string reactor_id = "bio_ferm_reactor_01";
        public string display_name = string.Empty;
        public string description = string.Empty;
        public List<string> build_cost_item_ids = new List<string>();
        public List<int> build_cost_amounts = new List<int>();
        public List<string> sanitize_cost_item_ids = new List<string>();
        public List<int> sanitize_cost_amounts = new List<int>();
        public List<string> service_cost_item_ids = new List<string>();
        public List<int> service_cost_amounts = new List<int>();
        public string filter_item_id = string.Empty;
        public int filter_units_per_replace = 1;
        public float service_filter_restore = 100f;
        public List<string> facility_tags = new List<string>();
    }

    [Serializable]
    public sealed class BioFermentationProcessDef
    {
        public string process_id = string.Empty;
        public string display_name = string.Empty;
        public string process_family = string.Empty;        // preservation|industrial|acid
        public bool aeration_required;
        public string temperature_band = "moderate";        // cool|moderate|warm
        public string acidity_target_band = "moderate";     // mild|moderate|high
        public int base_duration_days = 4;
        public int base_yield_units = 3;
        public float contamination_sensitivity = 0.25f;     // 0..1
        public List<string> feedstock_item_ids = new List<string>();
        public int feedstock_units_required = 2;
        public string starter_item_id = string.Empty;
        public int starter_quantity = 1;
        public string output_item_id = string.Empty;
        public List<string> byproduct_item_ids = new List<string>();
        public float filter_wear_per_batch = 20f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class BioFermentationCatalog
    {
        public int schema_version = 1;
        public BioFermentationReactorConfig reactor = new BioFermentationReactorConfig();
        public List<BioFermentationProcessDef> processes = new List<BioFermentationProcessDef>();
    }

    /// <summary>
    /// Plan 126 — fermentation process authority. Owns ONLY this machine:
    /// reactor build/service, staging, batch start/progress/completion,
    /// health bands, contamination events, and output grants. Food authority
    /// (KitchenNutritionSystem / FoodPreservationSystem), workshop authority,
    /// water/inventory and power remain external; this engine consumes
    /// item bills through the canonical Inventory transactions.
    /// </summary>
    public sealed class BioFermentationEngine
    {
        public const string SystemId = "bio_fermentation";
        public const string TraitBioprocessEngineer = "trait_bioprocess_engineer";
        public const string TraitFermentationMicrobiologist = "trait_fermentation_microbiologist";
        public const string RoomId = "room_workshop"; // fermenter gallery — an existing shelter room in the power grid

        // Quality thresholds (gameplay bands, not real process values).
        public const float PremiumQualityThreshold = 60f;
        public const float ContaminatedQualityCap = 30f;

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private BioFermentationCatalog _catalog = new BioFermentationCatalog();
        private readonly Dictionary<string, BioFermentationProcessDef> _processesById =
            new Dictionary<string, BioFermentationProcessDef>(StringComparer.Ordinal);
        private readonly List<string> _bindErrors = new List<string>();
        private BioFermentationState _state = new BioFermentationState();

        /// <summary>Traits of a survivor id; captured ONCE per batch at StartBatch.</summary>
        public Func<string, IReadOnlyList<string>>? TraitsOf { get; set; }
        /// <summary>Operator skill 0..1; captured ONCE per batch at StartBatch.</summary>
        public Func<string, float>? OperatorSkillProvider { get; set; }
        /// <summary>Room temperature °C; null = headless default (moderate).</summary>
        public Func<float>? RoomTempC { get; set; }
        /// <summary>Power predicate for the fermenter room; null = power assumed.</summary>
        public Func<bool>? IsRoomPowered { get; set; }

        public BioFermentationState State => _state;
        public IReadOnlyList<string> CatalogErrors => _bindErrors;
        public IReadOnlyDictionary<string, BioFermentationProcessDef> Processes => _processesById;

        public event Action<string>? OnBatchStarted;              // processId
        public event Action<string>? OnBatchCompleted;            // processId
        public event Action<string>? OnBatchAborted;              // reason
        public event Action<string, string>? OnContaminationEvent; // oldState, newState
        public event Action<string>? OnFaultChange;               // fault_state
        public event Action? OnStateChanged;

        private BioFermentationPhase Phase
        {
            get
            {
                var p = (BioFermentationPhase)_state.phase;
                if (!Enum.IsDefined(typeof(BioFermentationPhase), p)) return BioFermentationPhase.Unbuilt;
                return p;
            }
            set => _state.phase = (int)value;
        }

        public bool HasActiveBatch =>
            Phase == BioFermentationPhase.Inoculated
            || Phase == BioFermentationPhase.Fermenting
            || Phase == BioFermentationPhase.Conditioning
            || Phase == BioFermentationPhase.Separation
            || Phase == BioFermentationPhase.Complete;

        public bool HasRunningBatch =>
            Phase == BioFermentationPhase.Inoculated
            || Phase == BioFermentationPhase.Fermenting
            || Phase == BioFermentationPhase.Conditioning
            || Phase == BioFermentationPhase.Separation;

        public BioFermentationEngine(Inventory.Inventory inventory, ISeededRng rng, ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        // ── Catalog binding ───────────────────────────────────────────────

        /// <summary>
        /// Bind/rebind the catalog, validating every process shape. Invalid
        /// processes are logged and excluded from lookup — the engine never
        /// runs a batch against a broken definition. Returns the number of
        /// usable processes.
        /// </summary>
        public int BindCatalog(BioFermentationCatalog catalog)
        {
            _processesById.Clear();
            _bindErrors.Clear();
            if (catalog == null) return 0;
            _catalog = catalog;

            foreach (var process in catalog.processes ?? new List<BioFermentationProcessDef>())
            {
                string? error = ValidateProcess(process);
                if (error != null)
                {
                    _bindErrors.Add(process.process_id + ": " + error);
                    _log.Warn("[BioFermentation] catalog process rejected — " + error);
                    continue;
                }
                _processesById[process.process_id] = process;
            }
            return _processesById.Count;
        }

        private static string? ValidateProcess(BioFermentationProcessDef? p)
        {
            if (p == null) return "null process entry";
            if (string.IsNullOrWhiteSpace(p.process_id)) return "empty process_id";
            if (string.IsNullOrWhiteSpace(p.output_item_id)) return "empty output_item_id";
            if (p.feedstock_item_ids == null || p.feedstock_item_ids.Count == 0) return "no feedstock item ids";
            if (string.IsNullOrWhiteSpace(p.starter_item_id)) return "empty starter_item_id";
            if (p.base_duration_days < 1) return "base_duration_days < 1";
            if (p.base_yield_units < 1) return "base_yield_units < 1";
            if (p.feedstock_units_required < 1) return "feedstock_units_required < 1";
            if (p.contamination_sensitivity < 0f || p.contamination_sensitivity > 1f)
                return "contamination_sensitivity outside 0..1";
            if (p.filter_wear_per_batch < 0f) return "negative filter_wear_per_batch";
            if (!IsKnownBand(p.temperature_band)) return "unknown temperature_band";
            if (!IsKnownBand(p.acidity_target_band, acidity: true)) return "unknown acidity_target_band";
            return null;
        }

        private static bool IsKnownBand(string band, bool acidity = false)
        {
            if (acidity) return band == "mild" || band == "moderate" || band == "high";
            return band == "cool" || band == "moderate" || band == "warm";
        }

        public BioFermentationProcessDef? GetProcess(string processId)
        {
            if (string.IsNullOrEmpty(processId)) return null;
            _processesById.TryGetValue(processId, out var p);
            return p;
        }

        // ── Trait helpers (snapshot-aware: read from operator_trait_ids) ──

        private bool OperatorHasTrait(string traitId)
            => _state.operator_trait_ids != null && _state.operator_trait_ids.Contains(traitId);

        private float TraitContaminationMult
        {
            get
            {
                float mult = 1f;
                if (OperatorHasTrait(TraitBioprocessEngineer)) mult *= 0.7f;
                if (OperatorHasTrait(TraitFermentationMicrobiologist)) mult *= 0.85f;
                return mult;
            }
        }

        private float TraitYieldBonus
        {
            get
            {
                float bonus = 0f;
                if (OperatorHasTrait(TraitBioprocessEngineer)) bonus += 10f;
                if (OperatorHasTrait(TraitFermentationMicrobiologist)) bonus += 5f;
                return bonus;
            }
        }

        private float TraitWearMult
            => OperatorHasTrait(TraitFermentationMicrobiologist) ? 0.8f : 1f;

        // ── Build / maintenance surface ────────────────────────────────────

        public ActionResult BuildReactor()
        {
            if (Phase != BioFermentationPhase.Unbuilt)
                return ActionResult.Blocked("already_built", "bioferm.already_built");

            var reactor = _catalog.reactor ?? new BioFermentationReactorConfig();
            var bill = CostBill(reactor.build_cost_item_ids, reactor.build_cost_amounts);
            return Commit(bill, "missing_parts", "bioferm.missing_parts", () =>
            {
                Phase = BioFermentationPhase.Idle;
                _state.filter_condition = 100f;
                _state.contamination_state = "clear";
                _state.fault_state = string.Empty;
                _log.Info("[BioFermentation] reactor built.");
            });
        }

        public ActionResult SanitizeReactor()
        {
            if (Phase != BioFermentationPhase.Idle)
            {
                if (Phase == BioFermentationPhase.SterilizedReady)
                    return ActionResult.Success("bioferm.already_sanitized");
                return ActionResult.Blocked("not_idle", "bioferm.not_idle");
            }

            var reactor = _catalog.reactor ?? new BioFermentationReactorConfig();
            var bill = CostBill(reactor.sanitize_cost_item_ids, reactor.sanitize_cost_amounts);
            return Commit(bill, "missing_sanitize_supplies", "bioferm.missing_sanitize_supplies", () =>
            {
                Phase = BioFermentationPhase.SterilizedReady;
                _state.culture_health = 0f;
                _state.process_health = 0f;
                _state.contamination_state = "clear";
                _log.Info("[BioFermentation] reactor sanitized and ready.");
            });
        }

        public ActionResult ServiceReactor(int day)
        {
            bool needsFilter = _state.filter_condition < 50f;
            bool needsClear = _state.fault_state.Length > 0
                || _state.contamination_state != "clear"
                || Phase == BioFermentationPhase.MaintenanceRequired
                || Phase == BioFermentationPhase.Failed
                || Phase == BioFermentationPhase.Contaminated;

            if (!needsFilter && !needsClear)
                return ActionResult.Blocked("nothing_to_service", "bioferm.nothing_to_service");

            var reactor = _catalog.reactor ?? new BioFermentationReactorConfig();
            var bill = new InventoryBill();
            for (int i = 0; i < reactor.service_cost_item_ids.Count; i++)
            {
                int amount = i < reactor.service_cost_amounts.Count ? reactor.service_cost_amounts[i] : 1;
                if (amount > 0) bill.AddCost(reactor.service_cost_item_ids[i], amount);
            }
            if (needsFilter && !string.IsNullOrEmpty(reactor.filter_item_id))
                bill.AddCost(reactor.filter_item_id, Math.Max(1, reactor.filter_units_per_replace));

            return Commit(bill, "missing_kit", "bioferm.missing_kit", () =>
            {
                float restore = reactor.service_filter_restore > 0f ? reactor.service_filter_restore : 100f;
                _state.filter_condition = Math.Clamp(restore, 0f, 100f);
                _state.fault_state = string.Empty;
                _state.contamination_state = "clear";
                _state.last_service_day = day;
                if (!HasRunningBatch)
                {
                    if (Phase == BioFermentationPhase.Contaminated || Phase == BioFermentationPhase.Failed)
                    {
                        // Contaminated/failed mass is written off on service.
                        _state.committed_feedstock.Clear();
                        _state.committed_starter_item_id = string.Empty;
                        _state.process_id = string.Empty;
                        _state.batch_progress = 0f;
                        _state.yield_quality = -1f;
                    }
                    Phase = BioFermentationPhase.Idle;
                }
                _log.Info("[BioFermentation] reactor serviced.");
            });
        }

        private static InventoryBill CostBill(List<string>? itemIds, List<int>? amounts)
        {
            var bill = new InventoryBill();
            if (itemIds == null) return bill;
            for (int i = 0; i < itemIds.Count; i++)
            {
                int amount = amounts != null && i < amounts.Count ? amounts[i] : 1;
                if (amount > 0) bill.AddCost(itemIds[i], amount);
            }
            return bill;
        }

        private ActionResult Commit(InventoryBill bill, string failCode, string msgKey, Action onCommitted)
        {
            if (bill.IsEmpty)
                return ActionResult.Blocked(failCode, msgKey);
            bool ok = _inventory.TryExecuteTransaction(bill, () =>
            {
                onCommitted();
                OnStateChanged?.Invoke();
            });
            return ok ? ActionResult.Success("bioferm.committed") : ActionResult.Blocked(failCode, msgKey);
        }

        // ── Staging / batch control ────────────────────────────────────────

        public bool SetAeration(bool on)
        {
            _state.aeration_state = on;
            OnStateChanged?.Invoke();
            return on;
        }

        /// <summary>Stage feedstock for a future batch. Validates only process
        /// eligibility; inventory availability is checked at StartBatch (atomic).</summary>
        public ActionResult StageFeedstock(string itemId, int units)
        {
            if (Phase == BioFermentationPhase.Unbuilt)
                return ActionResult.Blocked("reactor_unbuilt", "bioferm.reactor_unbuilt");
            if (units <= 0) return ActionResult.Blocked("invalid_units", "bioferm.invalid_units");
            if (!IsEligibleFeedstock(itemId))
                return ActionResult.Blocked("invalid_item", "bioferm.invalid_feedstock");

            var staged = _state.staged_feedstock.Find(c => c.item_id == itemId);
            if (staged == null)
            {
                staged = new BioFermentationFeedstockCharge { item_id = itemId, units = 0 };
                _state.staged_feedstock.Add(staged);
            }
            staged.units += units;
            OnStateChanged?.Invoke();
            return ActionResult.Success("bioferm.staged");
        }

        public ActionResult ClearStagedFeedstock()
        {
            _state.staged_feedstock.Clear();
            OnStateChanged?.Invoke();
            return ActionResult.Success("bioferm.staged_cleared");
        }

        public bool IsEligibleFeedstock(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            foreach (var process in _processesById.Values)
                if (process.feedstock_item_ids.Contains(itemId, StringComparer.Ordinal))
                    return true;
            return false;
        }

        public ActionResult StartBatch(string processId, string operatorId, int day)
        {
            if (Phase != BioFermentationPhase.SterilizedReady)
                return ActionResult.Blocked("not_sanitized", "bioferm.not_sanitized");

            BioFermentationProcessDef? process = GetProcess(processId);
            if (process == null)
                return ActionResult.Blocked("missing_process", "bioferm.missing_process");

            int stagedUnits = _state.staged_feedstock.Sum(c => c.units);
            if (stagedUnits < process.feedstock_units_required)
                return ActionResult.Blocked("needs_feedstock", "bioferm.needs_feedstock");

            // Snapshot operator modifiers ONCE — later delegate changes must
            // not alter a running batch (once-application pinned by test).
            var traitIds = new List<string>();
            var traits = TraitsOf?.Invoke(operatorId ?? string.Empty);
            if (traits != null)
                foreach (var t in traits)
                    if (t == TraitBioprocessEngineer || t == TraitFermentationMicrobiologist)
                        traitIds.Add(t);
            float skill = Math.Clamp(OperatorSkillProvider?.Invoke(operatorId ?? string.Empty) ?? 0.5f, 0f, 1f);

            var bill = new InventoryBill();
            if (process.starter_quantity > 0)
                bill.AddCost(process.starter_item_id, process.starter_quantity);
            foreach (var charge in _state.staged_feedstock)
                bill.AddCost(charge.item_id, charge.units);

            return Commit(bill, "insufficient_inputs", "bioferm.insufficient_inputs", () =>
            {
                _state.process_id = process.process_id;
                _state.committed_feedstock = _state.staged_feedstock
                    .Select(c => new BioFermentationFeedstockCharge { item_id = c.item_id, units = c.units })
                    .ToList();
                _state.staged_feedstock.Clear();
                _state.committed_starter_item_id = process.starter_item_id;
                _state.current_feedstock_batch_id = "ferm_batch_" + _state.cycle_count + "_d" + day;
                _state.operator_id = operatorId ?? string.Empty;
                _state.operator_trait_ids = traitIds;
                _state.operator_skill = skill;
                _state.day_started = day;
                _state.phase_days_elapsed = 0;
                _state.batch_progress = 0f;
                _state.yield_quality = -1f;
                _state.culture_health = 55f;
                _state.process_health = 60f;
                _state.contamination_state = "clear";
                _state.fault_state = string.Empty;
                Phase = BioFermentationPhase.Inoculated;
                OnBatchStarted?.Invoke(process.process_id);
                _log.Info("[BioFermentation] batch started: " + process.process_id);
            });
        }

        public ActionResult AbortBatch()
        {
            if (!HasActiveBatch && !HasRunningBatch && Phase != BioFermentationPhase.MaintenanceRequired
                && Phase != BioFermentationPhase.Contaminated && Phase != BioFermentationPhase.Failed)
                return ActionResult.Blocked("nothing_active", "bioferm.nothing_active");

            string reason = _state.fault_state.Length > 0 ? _state.fault_state : "aborted";
            ResetBatch();
            Phase = BioFermentationPhase.Idle;
            OnBatchAborted?.Invoke(reason);
            OnStateChanged?.Invoke();
            return ActionResult.Success("bioferm.aborted");
        }

        // ── Daily tick (fixed RNG draw order, documented) ───────────────────

        /// <summary>
        /// Advance the reactor one day. Draw order (when the batch is running):
        ///   1. one contamination roll (only when sensitivity &gt; 0),
        ///   2. exactly one completion-quality roll on the tick the batch
        ///      transitions to Complete.
        /// Stall conditions (no progress, no draws): filter clogged, power
        /// loss, or batch phase not running. No wall-clock input.
        /// </summary>
        public void TickDay(int day)
        {
            if (Phase == BioFermentationPhase.Unbuilt) return;

            UpdateTemperatureBand();

            if (!HasRunningBatch)
            {
                OnStateChanged?.Invoke();
                return;
            }

            BioFermentationProcessDef? process = GetProcess(_state.process_id);
            if (process == null)
            {
                ResetBatch();
                Phase = BioFermentationPhase.Failed;
                _state.fault_state = "unknown_process";
                OnFaultChange?.Invoke(_state.fault_state);
                OnStateChanged?.Invoke();
                return;
            }

            // Power stalls the batch (no progress, no health drift, no rolls).
            if (IsRoomPowered != null && !IsRoomPowered())
            {
                _state.fault_state = "power_loss";
                _state.temperature_state = "cold";
                OnFaultChange?.Invoke(_state.fault_state);
                OnStateChanged?.Invoke();
                return;
            }
            _state.fault_state = string.Empty;

            // Filter clog stalls the batch; service is required to resume.
            if (_state.filter_condition <= 0f)
            {
                _state.fault_state = "filter_clogged";
                Phase = BioFermentationPhase.MaintenanceRequired;
                OnFaultChange?.Invoke(_state.fault_state);
                OnStateChanged?.Invoke();
                return;
            }

            bool tempOk = _state.temperature_state == process.temperature_band;
            bool aerOk = !process.aeration_required || _state.aeration_state;
            bool acidOk = _state.acidity_state == process.acidity_target_band;

            float tempDelta = tempOk ? 2f : -6f;
            float aerDelta = aerOk ? 2f : -8f;
            float acidDelta = acidOk ? 2f : -3f;
            float filterDelta = (_state.filter_condition - 60f) * 0.05f;
            float skillDelta = _state.operator_skill * 8f - 4f;
            float drift = tempDelta + aerDelta + acidDelta + filterDelta + skillDelta;

            _state.culture_health = Math.Clamp(_state.culture_health + drift * 0.7f, 0f, 100f);
            _state.process_health = Math.Clamp(_state.process_health + drift, 0f, 100f);

            // Draw 1 — contamination (wild contamination as abstract categories).
            if (process.contamination_sensitivity > 0f)
            {
                float healthAvg = (_state.culture_health + _state.process_health) / 200f;
                float risk = process.contamination_sensitivity * (1f - healthAvg) * TraitContaminationMult;
                if (_rng.NextDouble() < risk)
                {
                    double severityRoll = _rng.NextDouble(); // draw 1b (fixed order)
                    if (severityRoll < 0.35)
                    {
                        string before = _state.contamination_state;
                        _state.contamination_state = "suspect";
                        _state.culture_health = Math.Max(0f, _state.culture_health - 10f);
                        _state.process_health = Math.Max(0f, _state.process_health - 10f);
                        OnContaminationEvent?.Invoke(before, _state.contamination_state);
                    }
                    else if (severityRoll < 0.75)
                    {
                        string before = _state.contamination_state;
                        _state.contamination_state = "contaminated";
                        _state.process_health = Math.Max(0f, _state.process_health - 15f);
                        OnContaminationEvent?.Invoke(before, _state.contamination_state);
                    }
                    else
                    {
                        string before = _state.contamination_state;
                        _state.contamination_state = "spoiled";
                        Phase = BioFermentationPhase.Contaminated;
                        _state.fault_state = "contaminated_batch";
                        OnContaminationEvent?.Invoke(before, _state.contamination_state);
                        OnFaultChange?.Invoke(_state.fault_state);
                        OnStateChanged?.Invoke();
                        return;
                    }
                }
            }

            // Advance one phase day.
            _state.phase_days_elapsed++;
            _state.batch_progress = Math.Clamp(
                (float)_state.phase_days_elapsed / Math.Max(1, process.base_duration_days), 0f, 1f);

            bool nowComplete = _state.phase_days_elapsed >= process.base_duration_days;
            if (nowComplete)
            {
                // Draw 2 — completion quality variance, drawn exactly once
                // when the batch completes; persisted from here on.
                float healthAvg = (_state.culture_health + _state.process_health) / 200f;
                float variance = (float)(_rng.NextDouble() - 0.5) * 10f;
                float quality = Math.Clamp(healthAvg * 100f + TraitYieldBonus + variance, 0f, 100f);
                if (_state.contamination_state == "contaminated")
                    quality = Math.Min(quality, ContaminatedQualityCap);
                if (_state.contamination_state == "suspect")
                    quality = Math.Min(quality, 75f);
                _state.yield_quality = quality;
                Phase = BioFermentationPhase.Complete;
                _log.Info("[BioFermentation] batch complete: " + process.process_id);
            }
            else
            {
                Phase = _state.phase_days_elapsed switch
                {
                    1 => BioFermentationPhase.Fermenting,
                    2 => BioFermentationPhase.Conditioning,
                    _ => BioFermentationPhase.Separation,
                };
            }

            OnStateChanged?.Invoke();
        }

        private void UpdateTemperatureBand()
        {
            float t = RoomTempC?.Invoke() ?? 20f;
            _state.temperature_state = t < 10f ? "cold" : t < 18f ? "cool" : t < 24f ? "moderate" : t < 32f ? "warm" : "hot";
            // Acidity follows process health (pure arithmetic, no RNG).
            float h = (_state.culture_health + _state.process_health) / 2f;
            _state.acidity_state = h >= 70f ? "high" : h >= 50f ? "moderate" : h >= 30f ? "mild" : "weak";
        }

        // ── Harvest (zero draws; idempotent-safe) ──────────────────────────

        /// <summary>
        /// Collect a completed batch. Output quantity derives from the
        /// persisted quality and contamination state — no RNG, no re-roll,
        /// and the grant is one atomic inventory transaction. A failed
        /// grant leaves the batch Complete for a safe retry.
        /// </summary>
        public ActionResult HarvestBatch()
        {
            if (Phase != BioFermentationPhase.Complete)
                return ActionResult.Blocked("not_complete", "bioferm.not_complete");

            BioFermentationProcessDef? process = GetProcess(_state.process_id);
            if (process == null)
            {
                ResetBatch();
                Phase = BioFermentationPhase.Idle;
                return ActionResult.Failed("harvest_unknown_process", "bioferm.missing_process");
            }

            float quality = _state.yield_quality < 0f
                ? Math.Clamp((_state.culture_health + _state.process_health) / 2f + TraitYieldBonus, 0f, 100f)
                : _state.yield_quality;

            bool contaminated = _state.contamination_state == "contaminated" || _state.contamination_state == "spoiled";
            if (contaminated)
                quality = Math.Min(quality, ContaminatedQualityCap);

            int units;
            if (contaminated) units = Math.Max(1, process.base_yield_units / 2);
            else if (quality >= PremiumQualityThreshold + 20f) units = process.base_yield_units + 2;
            else if (quality >= PremiumQualityThreshold) units = process.base_yield_units + 1;
            else units = process.base_yield_units;

            var bill = new InventoryBill();
            bill.AddGrant(process.output_item_id, units);
            int premiumCut = process.base_yield_units + 1;
            if (units < premiumCut && process.byproduct_item_ids != null)
                foreach (var byproduct in process.byproduct_item_ids)
                    bill.AddGrant(byproduct, 1);

            bool granted = _inventory.TryExecuteTransaction(bill, () => { });
            if (!granted)
            {
                _log.Warn("[BioFermentation] harvest grant failed; batch held complete for retry.");
                return ActionResult.Failed("grant_failed", "bioferm.grant_failed");
            }

            _state.yield_quality = quality;
            _state.filter_condition = Math.Max(0f,
                _state.filter_condition - process.filter_wear_per_batch * TraitWearMult);
            _state.cycle_count++;

            ResetBatch();
            if (_state.filter_condition <= 0f)
            {
                Phase = BioFermentationPhase.MaintenanceRequired;
                _state.fault_state = "filter_clogged";
                OnFaultChange?.Invoke(_state.fault_state);
            }
            else
            {
                Phase = BioFermentationPhase.Idle;
            }

            OnBatchCompleted?.Invoke(process.process_id);
            OnStateChanged?.Invoke();
            return ActionResult.Success("bioferm.harvested",
                new Dictionary<string, double> { { "units", units }, { "quality", quality } });
        }

        private void ResetBatch()
        {
            _state.process_id = string.Empty;
            _state.committed_feedstock.Clear();
            _state.committed_starter_item_id = string.Empty;
            _state.batch_progress = 0f;
            _state.phase_days_elapsed = 0;
            _state.day_started = -1;
            _state.operator_id = string.Empty;
            _state.operator_trait_ids.Clear();
            _state.operator_skill = 0.5f;
        }

        // ── Persistence — restore is strictly non-operative ────────────────

        public BioFermentationState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<BioFermentationState>(json) ?? new BioFermentationState();
        }

        public void RestoreState(BioFermentationState? saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            var restored = s.Deserialize<BioFermentationState>(json);
            if (restored == null) return;

            // Guard against malformed/corrupt phase values — never invent work.
            if (!Enum.IsDefined(typeof(BioFermentationPhase), restored.phase))
                restored.phase = (int)BioFermentationPhase.Unbuilt;
            restored.culture_health = Math.Clamp(restored.culture_health, 0f, 100f);
            restored.process_health = Math.Clamp(restored.process_health, 0f, 100f);
            restored.filter_condition = Math.Clamp(restored.filter_condition, 0f, 100f);
            restored.batch_progress = Math.Clamp(restored.batch_progress, 0f, 1f);
            _state = restored;
            // Deliberately no production, wear, rolls, or inventory movement here.
        }
    }
}