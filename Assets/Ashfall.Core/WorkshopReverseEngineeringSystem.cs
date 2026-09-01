using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Inventory;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL — Workshop Reverse Engineering System.
    ///
    /// Composes <see cref="ResearchSystem"/>, <see cref="Crafting.CraftingSystem"/>,
    /// shared inventory, survivor skills, and the relic catalog to provide a
    /// single authority for relic examine/dismantle/repair/research actions.
    ///
    /// Completing a relic never edits recipes directly from UI code. All
    /// relic definitions come from versioned data (relic_recipes.json).
    /// </summary>
    [Serializable]
    public sealed class WorkshopState
    {
        public string systemId = WorkshopReverseEngineeringSystem.SystemId;
        public string selectedRelicId = string.Empty;
        public string assignedResearcherId = string.Empty;
        public int workPhase; // 0=idle, 1=examining, 2=dismantling, 3=repairing, 4=researching
        public float progressHours;
        public float hoursRequired;
        public List<string> reservedComponentIds = new List<string>();
        public List<int> reservedComponentAmounts = new List<int>();
        public bool isComplete;
        public string completionUnlockId = string.Empty; // research or recipe unlocked
        public List<string> completedRelicIds = new List<string>();
    }

    /// <summary>Catalog entry for a relic from data.</summary>
    [Serializable]
    public sealed class RelicDefinition
    {
        public string relic_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public List<string> required_components = new List<string>();
        public float repair_time_hours = 8f;
        public int morale_bonus;
        public string dialogue_event_id = string.Empty;
        public string restoration_text = string.Empty;
        public string world_flag = string.Empty;
        public string research_unlock_id = string.Empty; // knowledge node unlocked on research
        public string dismantle_yield_item = string.Empty;
        public int dismantle_yield_amount = 1;
        public string category = "relic";
    }

    /// <summary>Relic catalog — loaded from StreamingAssets/Data/relic_recipes.json.</summary>
    [Serializable]
    public sealed class RelicCatalog
    {
        public string schema_version = "1.0";
        public List<RelicDefinition> relics = new List<RelicDefinition>();
        public List<RelicDefinition> recipes { get => relics; set => relics = value; }
    }

    public sealed class WorkshopReverseEngineeringSystem
    {
        public const string SystemId = "workshop_reverse_engineering";

        private WorkshopState _state = new WorkshopState();
        private readonly Dictionary<string, RelicDefinition> _relicCatalog =
            new Dictionary<string, RelicDefinition>(StringComparer.Ordinal);
        private readonly global::Ashfall.Core.Inventory.Inventory _inventory;
        private readonly ResearchSystem _researchSystem;
        private readonly Crafting.CraftingSystem _craftingSystem;
        private readonly ILog _log;
        private Func<string, float> _getSurvivorSkill; // survivorId -> relevant skill level

        public WorkshopState State => _state;
        public IReadOnlyDictionary<string, RelicDefinition> Catalog => _relicCatalog;

        public event Action<ActionResult> OnActionCompleted;
        public event Action OnWorkshopStateChanged;

        public WorkshopReverseEngineeringSystem(
            global::Ashfall.Core.Inventory.Inventory inventory,
            ResearchSystem researchSystem,
            Crafting.CraftingSystem craftingSystem,
ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _researchSystem = researchSystem ?? throw new ArgumentNullException(nameof(researchSystem));
            _craftingSystem = craftingSystem ?? throw new ArgumentNullException(nameof(craftingSystem));
            _log = log ?? NullLog.Instance;
            _getSurvivorSkill = (_) => 1.0f; // default skill multiplier
        }

        /// <summary>Bind a survivor skill evaluator: survivorId -> skill multiplier (1.0 = average).</summary>
        public void BindSkillEvaluator(Func<string, float> evaluator)
        {
            _getSurvivorSkill = evaluator ?? ((_) => 1.0f);
        }

        /// <summary>Load relic catalog from deserialized data.</summary>
        public void LoadCatalog(RelicCatalog catalog)
        {
            if (catalog?.relics == null) return;
            _relicCatalog.Clear();
            foreach (var relic in catalog.relics)
            {
                if (!string.IsNullOrEmpty(relic.relic_id) && !_relicCatalog.ContainsKey(relic.relic_id))
                    _relicCatalog[relic.relic_id] = relic;
            }
            _log.Info($"[Workshop] loaded {_relicCatalog.Count} relics from catalog");
        }

        /// <summary>Register a single relic (for testing or hard-coded fallback).</summary>
        public void RegisterRelic(RelicDefinition relic)
        {
            if (relic == null || string.IsNullOrEmpty(relic.relic_id)) return;
            if (_relicCatalog.ContainsKey(relic.relic_id))
            {
                _log.Warn($"[Workshop] duplicate relic registration: {relic.relic_id}");
                return;
            }
            _relicCatalog[relic.relic_id] = relic;
        }

        /// <summary>Get a relic definition by ID.</summary>
        public RelicDefinition? GetRelic(string relicId)
        {
            if (string.IsNullOrEmpty(relicId)) return null;
            _relicCatalog.TryGetValue(relicId, out var def);
            return def;
        }

        /// <summary>True if the relic has been completed (dismantled/repaired/researched).</summary>
        public bool IsRelicCompleted(string relicId) =>
            !string.IsNullOrEmpty(relicId) && _state.completedRelicIds.Contains(relicId);

        /// <summary>True if the workshop is currently busy with a job.</summary>
        public bool IsBusy => _state.workPhase > 0 && !_state.isComplete;

        // ── Actions ──────────────────────────────────────────────────────────

        /// <summary>Examine a relic — returns its description and metadata without consuming anything.</summary>
        public ActionResult Examine(string relicId)
        {
            if (string.IsNullOrEmpty(relicId))
                return ActionResult.Failed("invalid_relic", "workshop.invalid_relic");
            if (!_relicCatalog.TryGetValue(relicId, out var relic))
                return ActionResult.Failed("unknown_relic", "workshop.unknown_relic");

            return ActionResult.Success("workshop.examine_success",
                new Dictionary<string, double>
                {
                    { "repair_hours", relic.repair_time_hours },
                    { "morale_bonus", relic.morale_bonus },
                    { "components", relic.required_components.Count }
                });
        }

        /// <summary>Start dismantling a relic. Consumes the relic, yields scrap/components.</summary>
        public ActionResult StartDismantle(string relicId, string researcherId)
        {
            if (IsBusy)
                return ActionResult.Blocked("workshop_busy", "workshop.already_busy");
            if (string.IsNullOrEmpty(relicId))
                return ActionResult.Failed("invalid_relic", "workshop.invalid_relic");
            if (!_relicCatalog.TryGetValue(relicId, out var relic))
                return ActionResult.Failed("unknown_relic", "workshop.unknown_relic");
            if (IsRelicCompleted(relicId))
                return ActionResult.Blocked("already_dismantled", "workshop.already_dismantled");

            var skill = _getSurvivorSkill(researcherId ?? string.Empty);
            float hours = Math.Max(1f, relic.repair_time_hours * 0.5f / skill);

            _state.selectedRelicId = relicId;
            _state.assignedResearcherId = researcherId ?? string.Empty;
            _state.workPhase = 2; // dismantling
            _state.progressHours = 0f;
            _state.hoursRequired = hours;
            _state.isComplete = false;
            _state.reservedComponentIds.Clear();
            _state.reservedComponentAmounts.Clear();

            _log.Info($"[Workshop] started dismantling '{relicId}' ({hours}h, researcher={researcherId})");
            OnWorkshopStateChanged?.Invoke();
            return ActionResult.Success("workshop.dismantle_started",
                new Dictionary<string, double> { { "hours_required", hours } });
        }

        /// <summary>Start repairing a relic. Reserves required components from inventory.</summary>
        public ActionResult StartRepair(string relicId, string researcherId)
        {
            if (IsBusy)
                return ActionResult.Blocked("workshop_busy", "workshop.already_busy");
            if (string.IsNullOrEmpty(relicId))
                return ActionResult.Failed("invalid_relic", "workshop.invalid_relic");
            if (!_relicCatalog.TryGetValue(relicId, out var relic))
                return ActionResult.Failed("unknown_relic", "workshop.unknown_relic");
            if (IsRelicCompleted(relicId))
                return ActionResult.Blocked("already_repaired", "workshop.already_repaired");

            // Check and consume component availability atomically
            if (relic.required_components != null && relic.required_components.Count > 0)
            {
                if (!_inventory.TryConsumeBill(relic.required_components))
                {
                    return ActionResult.Blocked("missing_components", "workshop.missing_components");
                }

                foreach (var comp in relic.required_components)
                {
                    _state.reservedComponentIds.Add(comp);
                    _state.reservedComponentAmounts.Add(1);
                }
            }

            var skill = _getSurvivorSkill(researcherId ?? string.Empty);
            float hours = relic.repair_time_hours / skill;

            _state.selectedRelicId = relicId;
            _state.assignedResearcherId = researcherId ?? string.Empty;
            _state.workPhase = 3; // repairing
            _state.progressHours = 0f;
            _state.hoursRequired = hours;
            _state.isComplete = false;

            _log.Info($"[Workshop] started repairing '{relicId}' ({hours}h, {_state.reservedComponentIds.Count} components)");
            OnWorkshopStateChanged?.Invoke();
            return ActionResult.Success("workshop.repair_started",
                new Dictionary<string, double>
                {
                    { "hours_required", hours },
                    { "components_consumed", _state.reservedComponentIds.Count }
                });
        }

        /// <summary>Start researching a relic. Progresses the associated research node.</summary>
        public ActionResult StartResearch(string relicId, string researcherId)
        {
            if (IsBusy)
                return ActionResult.Blocked("workshop_busy", "workshop.already_busy");
            if (string.IsNullOrEmpty(relicId))
                return ActionResult.Failed("invalid_relic", "workshop.invalid_relic");
            if (!_relicCatalog.TryGetValue(relicId, out var relic))
                return ActionResult.Failed("unknown_relic", "workshop.unknown_relic");
            if (IsRelicCompleted(relicId))
                return ActionResult.Blocked("already_researched", "workshop.already_researched");
            if (string.IsNullOrEmpty(relic.research_unlock_id))
                return ActionResult.Blocked("no_research_unlock", "workshop.no_research_unlock");

            var skill = _getSurvivorSkill(researcherId ?? string.Empty);
            float hours = Math.Max(2f, 8f / skill);

            _state.selectedRelicId = relicId;
            _state.assignedResearcherId = researcherId ?? string.Empty;
            _state.workPhase = 4; // researching
            _state.progressHours = 0f;
            _state.hoursRequired = hours;
            _state.isComplete = false;

            _log.Info($"[Workshop] started researching '{relicId}' -> unlock '{relic.research_unlock_id}' ({hours}h)");
            OnWorkshopStateChanged?.Invoke();
            return ActionResult.Success("workshop.research_started",
                new Dictionary<string, double> { { "hours_required", hours } });
        }

        /// <summary>Advance workshop progress by the given number of hours.</summary>
        public ActionResult TickProgress(float hoursElapsed)
        {
            if (!IsBusy) return ActionResult.Blocked("workshop_idle", "workshop.no_active_job");
            if (_state.isComplete) return ActionResult.Blocked("already_complete", "workshop.already_complete");

            _state.progressHours += hoursElapsed;
            if (_state.progressHours >= _state.hoursRequired)
            {
                return CompleteJob();
            }

            OnWorkshopStateChanged?.Invoke();
            return ActionResult.Success("workshop.progress",
                new Dictionary<string, double>
                {
                    { "progress", _state.progressHours },
                    { "required", _state.hoursRequired },
                    { "remaining", Math.Max(0, _state.hoursRequired - _state.progressHours) }
                });
        }

        /// <summary>Cancel the current workshop job and refund reserved components.</summary>
        public ActionResult CancelJob()
        {
            if (!IsBusy) return ActionResult.Blocked("workshop_idle", "workshop.no_active_job");

            // Refund reserved components
            for (int i = 0; i < _state.reservedComponentIds.Count; i++)
            {
                _inventory.AddById(_state.reservedComponentIds[i],
                    _state.reservedComponentAmounts[i]);
            }

            var previousPhase = _state.workPhase;
            ResetState();

            _log.Info($"[Workshop] cancelled job (phase={previousPhase}) — refunded components");
            OnWorkshopStateChanged?.Invoke();
            return ActionResult.Success("workshop.cancelled", null, innerEventId: null);
        }

        // ── Completion ───────────────────────────────────────────────────────

        private ActionResult CompleteJob()
        {
            if (!_relicCatalog.TryGetValue(_state.selectedRelicId, out var relic))
                return ActionResult.Failed("missing_relic", "workshop.error_missing_relic");

            _state.isComplete = true;
            _state.completedRelicIds.Add(_state.selectedRelicId);

            var deltas = new Dictionary<string, double>();
            string messageKey;
            switch (_state.workPhase)
            {
                case 2: // dismantle
                    if (!string.IsNullOrEmpty(relic.dismantle_yield_item))
                    {
                        _inventory.AddById(relic.dismantle_yield_item,
                            relic.dismantle_yield_amount);
                        deltas[relic.dismantle_yield_item] = relic.dismantle_yield_amount;
                    }
                    messageKey = "workshop.dismantle_complete";
                    break;

                case 3: // repair
                    // Morale bonus applied through host hook
                    deltas["morale_bonus"] = relic.morale_bonus;
                    if (!string.IsNullOrEmpty(relic.world_flag))
                        deltas["flag_" + relic.world_flag] = 1;
                    messageKey = "workshop.repair_complete";
                    break;

                case 4: // research
                    if (!string.IsNullOrEmpty(relic.research_unlock_id))
                    {
                        _researchSystem.UnlockManual(relic.research_unlock_id);
                        if (_researchSystem.CompleteResearch(relic.research_unlock_id))
                        {
                            deltas["research_unlocked"] = 1;
                            _state.completionUnlockId = relic.research_unlock_id;
                            messageKey = "workshop.research_complete";
                        }
                        else if (_researchSystem.GetKnowledge(relic.research_unlock_id) != null)
                        {
                            // Already completed through another producer — still a
                            // successful research outcome for the relic.
                            _state.completionUnlockId = relic.research_unlock_id;
                            messageKey = "workshop.research_complete";
                        }
                        else
                        {
                            // Plan 34: never fabricate research definitions at
                            // runtime. A relic whose research_unlock_id is absent
                            // from research_knowledge.json is a data defect —
                            // surface it, keep the unlock flag, grant nothing.
                            _log.Warn($"[Workshop] relic '{relic.relic_id}' research unlock '{relic.research_unlock_id}' not in research catalog");
                            _state.completionUnlockId = relic.research_unlock_id;
                            messageKey = "workshop.research_complete";
                        }
                    }
                    else
                    {
                        messageKey = "workshop.research_complete_no_unlock";
                    }
                    break;

                default:
                    messageKey = "workshop.job_complete";
                    break;
            }

            _log.Info($"[Workshop] completed {_state.selectedRelicId} (phase={_state.workPhase})");
            var result = ActionResult.Success(messageKey, deltas);
            OnActionCompleted?.Invoke(result);
            OnWorkshopStateChanged?.Invoke();
            return result;
        }

        private void ResetState()
        {
            _state.selectedRelicId = string.Empty;
            _state.assignedResearcherId = string.Empty;
            _state.workPhase = 0;
            _state.progressHours = 0f;
            _state.hoursRequired = 0f;
            _state.reservedComponentIds.Clear();
            _state.reservedComponentAmounts.Clear();
            _state.isComplete = false;
            _state.completionUnlockId = string.Empty;
        }

        // ── Persistence ──────────────────────────────────────────────────────

        public WorkshopState CaptureState()
        {
            return _state;
        }

        public void RestoreState(WorkshopState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnWorkshopStateChanged?.Invoke();
        }
    }
}
