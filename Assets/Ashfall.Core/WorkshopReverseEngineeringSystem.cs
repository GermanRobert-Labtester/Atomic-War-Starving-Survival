// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        // Plan 166 tech-salvage job fields. The source item is committed when
        // the job starts; this prevents duplicate completion after reload.
        public string activeTechSalvageId = string.Empty;
        public string techSourceItemId = string.Empty;
        public bool techSourceConsumed;
        public int techStartedDay;
        public float techEquipmentQuality01;
        public List<string> completedTechSalvageIds = new List<string>();
        public List<ResearchNoteState> researchNotes = new List<ResearchNoteState>();
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

    [Serializable]
    public sealed class ResearchNoteState
    {
        public string noteId = string.Empty;
        public string techId = string.Empty;
        public string researcherId = string.Empty;
        public int day;
        public string progressBand = string.Empty;
    }

    public sealed class WorkshopReverseEngineeringSystem
    {
        public const string SystemId = "workshop_reverse_engineering";

        private WorkshopState _state = new WorkshopState();
        private readonly Dictionary<string, RelicDefinition> _relicCatalog =
            new Dictionary<string, RelicDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, PreWarTechDef> _techCatalog =
            new Dictionary<string, PreWarTechDef>(StringComparer.Ordinal);
        private readonly global::Ashfall.Core.Inventory.Inventory _inventory;
        private readonly ResearchSystem _researchSystem;
        private readonly Crafting.CraftingSystem _craftingSystem;
        private readonly ILog _log;
        private Func<string, float> _getSurvivorSkill; // survivorId -> relevant skill level
        private ISeededRng _techRng;

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
            _techRng = new SeededRng(166);
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

        public void LoadTechSalvageCatalog(IEnumerable<PreWarTechDef> definitions)
        {
            _techCatalog.Clear();
            if (definitions == null) return;
            foreach (var def in definitions)
            {
                if (def != null && !string.IsNullOrWhiteSpace(def.Id) && !_techCatalog.ContainsKey(def.Id))
                    _techCatalog[def.Id] = def;
            }
            _log.Info($"[Workshop] loaded {_techCatalog.Count} tech salvage definitions");
        }

        public void BindTechSalvageRng(ISeededRng rng)
        {
            _techRng = rng ?? new SeededRng(166);
        }

        public IReadOnlyDictionary<string, PreWarTechDef> TechSalvageCatalog => _techCatalog;

        public PreWarTechDef? GetTechSalvage(string techId)
        {
            if (string.IsNullOrWhiteSpace(techId)) return null;
            _techCatalog.TryGetValue(techId, out var def);
            return def;
        }

        public bool IsTechSalvageCompleted(string techId) =>
            !string.IsNullOrWhiteSpace(techId)
            && _state.completedTechSalvageIds.Contains(techId);

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

        /// <summary>Pure preview for a cataloged tech recovery job.</summary>
        public TechDismantlePreview PreviewTechDismantle(
            string sourceItemId,
            string researcherId,
            ResearchFacilityContext? facility = null)
        {
            var preview = new TechDismantlePreview { sourceItemId = sourceItemId ?? string.Empty };
            if (IsBusy) { preview.failureCode = "workshop_busy"; return preview; }
            if (!_techCatalog.TryGetValue(sourceItemId ?? string.Empty, out var def))
            {
                // Catalog IDs are the command IDs, while inventory holds the
                // source item ID. Search deterministically by source item.
                foreach (var candidate in _techCatalog.Values)
                {
                    if (candidate != null && string.Equals(candidate.SourceItemId, sourceItemId, StringComparison.Ordinal))
                    {
                        def = candidate;
                        break;
                    }
                }
            }
            if (def == null) { preview.failureCode = "unknown_tech"; return preview; }
            if (IsTechSalvageCompleted(def.Id)) { preview.failureCode = "already_salvaged"; return preview; }
            if (_inventory.CountById(def.SourceItemId) < 1) { preview.failureCode = "missing_source_item"; return preview; }

            var context = facility ?? new ResearchFacilityContext();
            if (!context.IsAvailable) { preview.failureCode = "lab_unavailable"; return preview; }
            if (!context.PowerStable) { preview.failureCode = "lab_power_unstable"; return preview; }
            if (context.EquipmentTags == null || def.RequiredResearchEquipmentTags == null)
            {
                preview.failureCode = "missing_research_equipment";
                return preview;
            }
            for (int i = 0; i < def.RequiredResearchEquipmentTags.Count; i++)
            {
                if (!ContainsOrdinal(context.EquipmentTags, def.RequiredResearchEquipmentTags[i]))
                {
                    preview.failureCode = "missing_research_equipment";
                    return preview;
                }
            }

            float skill = Math.Clamp(_getSurvivorSkill(researcherId ?? string.Empty), 0.25f, 2f);
            float equipment = Math.Clamp(context.EquipmentQuality01, 0f, 1f);
            preview.techId = def.Id;
            preview.successChance = Math.Clamp(
                def.BaseSuccessChance + (skill - 1f) * 0.15f + equipment * 0.20f - (def.Complexity - 1) * 0.04f,
                0.05f, 0.95f);
            preview.catastrophicFailureChance = Math.Clamp(
                def.CatastrophicFailureChance * (1f - equipment * 0.5f), 0f, 0.25f);
            preview.researchPoints = def.BaseResearchPoints;
            preview.blueprintRequiredPoints = def.BlueprintRequiredPoints;
            preview.isAvailable = true;
            return preview;
        }

        /// <summary>
        /// Starts a cataloged tech recovery job. The source item is consumed as
        /// the command commit, before any deterministic outcome roll occurs.
        /// </summary>
        public ActionResult StartTechDismantle(
            string sourceItemId,
            string researcherId,
            int day = 0,
            ResearchFacilityContext? facility = null)
        {
            var preview = PreviewTechDismantle(sourceItemId, researcherId, facility);
            if (!preview.isAvailable)
                return ActionResult.Blocked(preview.failureCode, "workshop.tech_dismantle_unavailable");

            var def = _techCatalog[preview.techId];
            if (!_inventory.TryConsume(def.SourceItemId, 1))
                return ActionResult.Blocked("missing_source_item", "workshop.tech_missing_source");

            float skill = Math.Clamp(_getSurvivorSkill(researcherId ?? string.Empty), 0.25f, 2f);
            float hours = Math.Max(2f, 3f + def.Complexity * 1.5f) / Math.Min(1.5f, skill);
            _state.selectedRelicId = string.Empty;
            _state.assignedResearcherId = researcherId ?? string.Empty;
            _state.workPhase = 5;
            _state.progressHours = 0f;
            _state.hoursRequired = hours;
            _state.isComplete = false;
            _state.activeTechSalvageId = def.Id;
            _state.techSourceItemId = def.SourceItemId;
            _state.techSourceConsumed = true;
            _state.techStartedDay = day;
            _state.techEquipmentQuality01 = Math.Clamp(facility?.EquipmentQuality01 ?? 0f, 0f, 1f);
            _state.completionUnlockId = string.Empty;

            _log.Info($"[Workshop] started tech recovery '{def.Id}' ({hours:F1}h)");
            OnWorkshopStateChanged?.Invoke();
            return ActionResult.Success("workshop.tech_dismantle_started",
                new Dictionary<string, double>
                {
                    { "hours_required", hours },
                    { "success_chance", preview.successChance },
                    { "catastrophic_failure_chance", preview.catastrophicFailureChance }
                });
        }

        /// <summary>
        /// Resolves a relic for a mutating workshop action through the single
        /// guard chain the inline sites previously triplicated: workshop idle,
        /// non-empty id, catalog-known, and not already completed. Returns false
        /// with the exact failure result the inline guards produced.
        /// </summary>
        private bool TryResolveActionableRelic(
            string relicId,
            string completedCode,
            string completedMessage,
            [NotNullWhen(true)] out RelicDefinition? relic,
            out ActionResult failure)
        {
            if (IsBusy)
            {
                relic = null;
                failure = ActionResult.Blocked("workshop_busy", "workshop.already_busy");
                return false;
            }
            if (string.IsNullOrEmpty(relicId))
            {
                relic = null;
                failure = ActionResult.Failed("invalid_relic", "workshop.invalid_relic");
                return false;
            }
            if (!_relicCatalog.TryGetValue(relicId, out relic))
            {
                failure = ActionResult.Failed("unknown_relic", "workshop.unknown_relic");
                return false;
            }
            if (IsRelicCompleted(relicId))
            {
                failure = ActionResult.Blocked(completedCode, completedMessage);
                return false;
            }
            failure = ActionResult.Success("workshop.relic_resolved");
            return true;
        }

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
            if (!TryResolveActionableRelic(relicId, "already_dismantled", "workshop.already_dismantled", out var relic, out var failure))
                return failure;

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
            if (!TryResolveActionableRelic(relicId, "already_repaired", "workshop.already_repaired", out var relic, out var failure))
                return failure;

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
            if (!TryResolveActionableRelic(relicId, "already_researched", "workshop.already_researched", out var relic, out var failure))
                return failure;
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
            if (hoursElapsed <= 0f) return ActionResult.Failed("invalid_hours", "workshop.invalid_hours");

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
            if (_state.workPhase == 5)
                return CompleteTechJob();

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

        private ActionResult CompleteTechJob()
        {
            if (!_techCatalog.TryGetValue(_state.activeTechSalvageId, out var def))
                return ActionResult.Failed("missing_tech", "workshop.tech_missing_definition");
            if (IsTechSalvageCompleted(def.Id))
                return ActionResult.Blocked("already_salvaged", "workshop.tech_already_salvaged");

            // The source is already consumed, so outcome math is independent
            // of the current inventory count after a save/load boundary.
            float skill = Math.Clamp(_getSurvivorSkill(_state.assignedResearcherId), 0.25f, 2f);
            float successChance = Math.Clamp(
                def.BaseSuccessChance + (skill - 1f) * 0.15f + _state.techEquipmentQuality01 * 0.20f - (def.Complexity - 1) * 0.04f,
                0.05f, 0.95f);
            bool success = _techRng.NextDouble() < successChance;
            bool catastrophic = !success && _techRng.NextDouble() <
                Math.Clamp(def.CatastrophicFailureChance * (1f - _state.techEquipmentQuality01 * 0.5f), 0f, 0.25f);

            _state.isComplete = true;
            _state.completedTechSalvageIds.Add(def.Id);
            var deltas = new Dictionary<string, double>
            {
                { "success_chance", successChance },
                { "source_consumed", _state.techSourceConsumed ? 1 : 0 }
            };

            if (catastrophic)
            {
                string[] failureTypes = { "component_arc", "pressure_cell_rupture", "stored_energy_release", "data_core_destroyed" };
                string failureType = failureTypes[_techRng.Next(0, failureTypes.Length)];
                deltas["catastrophic_failure"] = 1;
                var failure = new TechSalvageFailure
                {
                    techId = def.Id,
                    sourceItemId = def.SourceItemId,
                    failureType = failureType,
                    day = _state.techStartedDay
                };
                OnTechSalvageFailure?.Invoke(failure);
                var failed = ActionResult.Failed("catastrophic_failure", "workshop.tech_catastrophic_failure", deltas);
                OnActionCompleted?.Invoke(failed);
                OnWorkshopStateChanged?.Invoke();
                return failed;
            }

            if (!success)
            {
                deltas["salvage_recovered"] = 0;
                var failed = ActionResult.Failed("research_failed", "workshop.tech_research_failed", deltas);
                OnActionCompleted?.Invoke(failed);
                OnWorkshopStateChanged?.Invoke();
                return failed;
            }

            _researchSystem.TryAddResearchPoints(def.BaseResearchPoints, def.Id);
            deltas["research_points"] = def.BaseResearchPoints;
            deltas["blueprint_progress"] = def.BaseResearchPoints;
            for (int i = 0; i < def.BaseScrapYields.Count; i++)
            {
                var yield = def.BaseScrapYields[i];
                if (yield == null || string.IsNullOrWhiteSpace(yield.ItemId) || yield.Amount <= 0) continue;
                if (_inventory.AddById(yield.ItemId, yield.Amount))
                    deltas[yield.ItemId] = yield.Amount;
            }

            if (def.PossibleBlueprintIds.Count > 0)
            {
                int blueprintIndex = _techRng.Next(0, def.PossibleBlueprintIds.Count);
                string blueprintId = def.PossibleBlueprintIds[blueprintIndex];
                if (!string.IsNullOrWhiteSpace(blueprintId))
                {
                    _researchSystem.TryAddBlueprintProgress(
                        blueprintId,
                        def.BaseResearchPoints,
                        def.BlueprintRequiredPoints,
                        _state.techStartedDay,
                        def.Id);
                    _state.completionUnlockId = blueprintId;
                    OnBlueprintInsight?.Invoke(blueprintId);
                }
            }

            if (def.ResearchNotesPool.Count > 0)
            {
                int noteIndex = _techRng.Next(0, def.ResearchNotesPool.Count);
                _state.researchNotes.Add(new ResearchNoteState
                {
                    noteId = def.ResearchNotesPool[noteIndex],
                    techId = def.Id,
                    researcherId = _state.assignedResearcherId,
                    day = _state.techStartedDay,
                    progressBand = "decoded"
                });
            }

            var result = ActionResult.Success("workshop.tech_dismantle_complete", deltas);
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
            return new WorkshopState
            {
                systemId = _state.systemId,
                selectedRelicId = _state.selectedRelicId,
                assignedResearcherId = _state.assignedResearcherId,
                workPhase = _state.workPhase,
                progressHours = _state.progressHours,
                hoursRequired = _state.hoursRequired,
                reservedComponentIds = new List<string>(_state.reservedComponentIds ?? new List<string>()),
                reservedComponentAmounts = new List<int>(_state.reservedComponentAmounts ?? new List<int>()),
                isComplete = _state.isComplete,
                completionUnlockId = _state.completionUnlockId,
                completedRelicIds = new List<string>(_state.completedRelicIds ?? new List<string>()),
                activeTechSalvageId = _state.activeTechSalvageId,
                techSourceItemId = _state.techSourceItemId,
                techSourceConsumed = _state.techSourceConsumed,
                techStartedDay = _state.techStartedDay,
                techEquipmentQuality01 = _state.techEquipmentQuality01,
                completedTechSalvageIds = new List<string>(_state.completedTechSalvageIds ?? new List<string>()),
                researchNotes = CloneNotes(_state.researchNotes)
            };
        }

        public void RestoreState(WorkshopState saved)
        {
            if (saved == null) return;
            _state = new WorkshopState
            {
                systemId = saved.systemId ?? SystemId,
                selectedRelicId = saved.selectedRelicId ?? string.Empty,
                assignedResearcherId = saved.assignedResearcherId ?? string.Empty,
                workPhase = saved.workPhase,
                progressHours = Math.Max(0f, saved.progressHours),
                hoursRequired = Math.Max(0f, saved.hoursRequired),
                reservedComponentIds = saved.reservedComponentIds != null ? new List<string>(saved.reservedComponentIds) : new List<string>(),
                reservedComponentAmounts = saved.reservedComponentAmounts != null ? new List<int>(saved.reservedComponentAmounts) : new List<int>(),
                isComplete = saved.isComplete,
                completionUnlockId = saved.completionUnlockId ?? string.Empty,
                completedRelicIds = saved.completedRelicIds != null ? new List<string>(saved.completedRelicIds) : new List<string>(),
                activeTechSalvageId = saved.activeTechSalvageId ?? string.Empty,
                techSourceItemId = saved.techSourceItemId ?? string.Empty,
                techSourceConsumed = saved.techSourceConsumed,
                techStartedDay = saved.techStartedDay,
                techEquipmentQuality01 = Math.Clamp(saved.techEquipmentQuality01, 0f, 1f),
                completedTechSalvageIds = saved.completedTechSalvageIds != null ? new List<string>(saved.completedTechSalvageIds) : new List<string>(),
                researchNotes = CloneNotes(saved.researchNotes)
            };
            OnWorkshopStateChanged?.Invoke();
        }

        public event Action<TechSalvageFailure>? OnTechSalvageFailure;
        public event Action<string>? OnBlueprintInsight;

        private static List<ResearchNoteState> CloneNotes(List<ResearchNoteState>? notes)
        {
            var copy = new List<ResearchNoteState>();
            if (notes == null) return copy;
            foreach (var note in notes)
            {
                if (note == null) continue;
                copy.Add(new ResearchNoteState
                {
                    noteId = note.noteId,
                    techId = note.techId,
                    researcherId = note.researcherId,
                    day = note.day,
                    progressBand = note.progressBand
                });
            }
            return copy;
        }

        private static bool ContainsOrdinal(List<string> values, string value)
        {
            if (values == null) return false;
            for (int i = 0; i < values.Count; i++)
                if (string.Equals(values[i], value, StringComparison.Ordinal)) return true;
            return false;
        }
    }
}
