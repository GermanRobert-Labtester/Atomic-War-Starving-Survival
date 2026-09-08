// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 116 — Pharmaceutical Tablet Manufacturing & Controlled-Release
// Production.
//
// AUTHORITY MAP (Flagship Plans 114-117, Wave 4):
//   * MedicalSystem / MedicalTreatmentCatalog (Ashfall.Core.Medical) remain
//     the ONLY treatment authority. This engine NEVER heals, treats or
//     applies medical effects — it manufactures canonical medication ITEMS
//     (e.g. "antibiotics") that the existing pipeline already consumes via
//     its ItemCosts.
//   * Precursors/resources come from canonical inventory via bound
//     delegates (ChemicalPlantSystem / scavenging / trade own production).
//     No controlled-drug synthesis happens here — formulations are
//     high-level game abstractions with no operational detail.
//   * Trade: batch results carry trade_value_modifier metadata for trade
//     surfaces to consume. This engine has no currency port at all.
//   * State machine: idle → staging → blending → forming → coating →
//     packaging → quality_check → complete | rejected (plus faulted,
//     maintenance_required). Transitions are explicit, deterministic and
//     save-safe; completed batches resolve exactly once — reloads never
//     re-run quality checks or duplicate outputs.
//   * No wall clock, no System.Random — ISeededRng only.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Medical
{
    // ─────────────────────────────────────────────────────────────────
    // Catalog DTOs (data authority: Assets/StreamingAssets/Data/
    // tablet_manufacturing_catalog.json)
    // ─────────────────────────────────────────────────────────────────

    /// <summary>Gameplay-level release abstraction — no operational detail.</summary>
    [Serializable]
    public sealed class TabletReleaseClassDef
    {
        public string release_class_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        /// <summary>standard | extended | protected</summary>
        public string shelf_life_class { get; set; } = "standard";
        public float coating_quality_floor { get; set; } = 1f;
        public float trade_value_bonus { get; set; } = 0f;
        /// <summary>Extra process day for coated batches.</summary>
        public int extra_process_days { get; set; } = 0;
    }

    [Serializable]
    public sealed class TabletFormulationDef
    {
        public string formulation_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        /// <summary>Canonical medication item the batch produces (consumed by
        /// the existing medical treatment pipeline via its ItemCosts).</summary>
        public string medical_effect_item_id { get; set; } = string.Empty;
        /// <summary>antibiotic | analgesic | supplement</summary>
        public string formulation_class { get; set; } = "supplement";
        /// <summary>Explicit canonical precursor items and amounts.</summary>
        public Dictionary<string, int> precursor_costs { get; set; } = new Dictionary<string, int>();
        /// <summary>Documentation tags for the precursor classes (non-authoritative).</summary>
        public List<string> required_precursor_tags { get; set; } = new List<string>();
        public string binder_resource_id { get; set; } = string.Empty;
        public int binder_amount { get; set; } = 1;
        public string packaging_resource_id { get; set; } = string.Empty;
        /// <summary>immediate | delayed | protected</summary>
        public string release_class_id { get; set; } = "immediate";
        public int base_batch_size { get; set; } = 10;
        public float quality_threshold { get; set; } = 0.5f;
        public float trade_value_modifier { get; set; } = 1f;
        /// <summary>Content-policy tag; informational for surfaces and review.</summary>
        public string controlled_substance_tag { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class TabletPressDef
    {
        public string press_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
        public int construction_labor_days { get; set; } = 2;
        public float max_condition { get; set; } = 100f;
        public int maintenance_interval_days { get; set; } = 8;
        public Dictionary<string, int> maintenance_required_items { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> tooling_replacement_items { get; set; } = new Dictionary<string, int>();
        public string room_id { get; set; } = string.Empty;
        /// <summary>Tooling wear per completed batch at 0..100 scale.</summary>
        public float tooling_wear_per_batch { get; set; } = 2.5f;
    }

    [Serializable]
    public sealed class TabletManufacturingCatalog
    {
        public int schema_version { get; set; } = 1;
        public string network_id { get; set; } = "tablet_manufacturing_primary";
        public string display_name { get; set; } = "Shelter Tablet Works";
        public TabletPressDef press { get; set; } = new TabletPressDef();
        public List<TabletFormulationDef> formulations { get; set; } = new List<TabletFormulationDef>();
        public List<TabletReleaseClassDef> release_classes { get; set; } = new List<TabletReleaseClassDef>();
        /// <summary>Batches buffered before claiming must be claimed (bounded).</summary>
        public int output_buffer_max_batches { get; set; } = 3;
        public List<string> tags { get; set; } = new List<string>();
    }

    // ─────────────────────────────────────────────────────────────────
    // State DTOs
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class PharmaceuticalTabletPressState
    {
        public int schema_version { get; set; } = 1;
        public string press_id { get; set; } = string.Empty;
        public bool constructed { get; set; }
        public float machine_condition { get; set; } = 100f;
        /// <summary>0..100 tooling condition (dies/punches abstraction).</summary>
        public float tooling_condition { get; set; } = 100f;
        /// <summary>0..1 calibration quality; decays per batch, restored by recalibration.</summary>
        public float calibration_state { get; set; } = 1f;
        public string current_formulation_id { get; set; } = string.Empty;
        /// <summary>idle | staging | blending | forming | coating | packaging | quality_check | complete | rejected | faulted | maintenance_required</summary>
        public string machine_state { get; set; } = "idle";
        public float batch_progress { get; set; } = 0f;
        public float uniformity_quality { get; set; } = 0f;
        public float tablet_integrity_quality { get; set; } = 0f;
        public float coating_quality { get; set; } = 0f;
        public float packaging_quality { get; set; } = 0f;
        public float reject_fraction { get; set; } = 0f;
        public string fault_state { get; set; } = string.Empty;
        public int cycle_count { get; set; } = 0;
        public int days_since_maintenance { get; set; } = 0;
        public int next_batch_number { get; set; } = 1;
    }

    [Serializable]
    public sealed class TabletBatchState
    {
        public string batch_id { get; set; } = string.Empty;
        public string formulation_id { get; set; } = string.Empty;
        public string release_class_id { get; set; } = string.Empty;
        public int input_batch_size { get; set; }
        public int progress_days { get; set; } = 0;
        public int total_process_days { get; set; } = 3;
        public string machine_state_at_save { get; set; } = "staging";
        public bool quality_resolved { get; set; }
        public bool packaging_applied { get; set; }
    }

    /// <summary>Resolved batch outcome — the trade/medical-facing metadata.</summary>
    [Serializable]
    public sealed class PharmaceuticalBatchResult
    {
        public string batch_id { get; set; } = string.Empty;
        /// <summary>Canonical medication item id (medical authority consumes this).</summary>
        public string result_item_id { get; set; } = string.Empty;
        public int quantity { get; set; }
        /// <summary>excellent | good | fair | poor</summary>
        public string quality_grade { get; set; } = string.Empty;
        /// <summary>short | standard | extended | protected</summary>
        public string shelf_life_class { get; set; } = string.Empty;
        public int reject_quantity { get; set; }
        public float trade_value_modifier { get; set; } = 1f;
        public List<string> warning_tags { get; set; } = new List<string>();
        public int completed_day { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────
    // Failure codes
    // ─────────────────────────────────────────────────────────────────

    public static class PharmaceuticalTabletFailures
    {
        public const string NotConstructed = "tab.not_constructed";
        public const string AlreadyConstructed = "tab.already_constructed";
        public const string MaterialsMissing = "tab.materials_missing";
        public const string UnknownFormulation = "tab.unknown_formulation";
        public const string MachineBusy = "tab.machine_busy";
        public const string MachineFaulted = "tab.machine_faulted";
        public const string MaintenanceRequired = "tab.maintenance_required";
        public const string NoBatch = "tab.no_batch";
        public const string NothingToClaim = "tab.nothing_to_claim";
        public const string BufferFull = "tab.buffer_full";
    }

    // ─────────────────────────────────────────────────────────────────
    // Engine
    // ─────────────────────────────────────────────────────────────────

    public sealed class PharmaceuticalTabletEngine
    {
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private TabletManufacturingCatalog _catalog = new TabletManufacturingCatalog();
        private PharmaceuticalTabletPressState _state = new PharmaceuticalTabletPressState();
        private TabletBatchState? _activeBatch;
        private readonly List<PharmaceuticalBatchResult> _outputBuffer = new List<PharmaceuticalBatchResult>();

        // Inventory ports (host-bound; deterministic).
        private Func<string, int> _getCount = _ => 0;
        private Func<string, int, bool> _canAdd = (_, _) => false;
        private Action<string, int> _addItem = (_, _) => { };
        private Action<string, int> _consume = (_, _) => { };

        /// <summary>Host-injected campaign day provider (deterministic).</summary>
        public Func<int>? DayProvider { get; set; }
        /// <summary>Pharmaceutical chemist skill 0..1 — batch quality, rejects.</summary>
        public Func<float>? PharmaceuticalChemistSkillProvider { get; set; }
        /// <summary>Formulation technician skill 0..1 — tooling wear, calibration.</summary>
        public Func<float>? FormulationTechnicianSkillProvider { get; set; }

        private int CurrentDay() => DayProvider?.Invoke() ?? 0;
        private float ChemistSkill() => Math.Clamp(PharmaceuticalChemistSkillProvider?.Invoke() ?? 0f, 0f, 1f);
        private float TechnicianSkill() => Math.Clamp(FormulationTechnicianSkillProvider?.Invoke() ?? 0f, 0f, 1f);

        public PharmaceuticalTabletPressState State => _state;
        public TabletManufacturingCatalog Catalog => _catalog;
        public bool IsConstructed => _state.constructed;
        public TabletBatchState? ActiveBatch => _activeBatch;
        public IReadOnlyList<PharmaceuticalBatchResult> OutputBuffer => _outputBuffer;

        /// <summary>Raised when a batch resolves (complete or rejected).</summary>
        public event Action<PharmaceuticalBatchResult, bool>? OnBatchResolved; // result, accepted
        public event Action<string>? OnEventRaised;

        public PharmaceuticalTabletEngine(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(2116);
            _log = log ?? NullLog.Instance;
        }

        public void BindCatalog(TabletManufacturingCatalog catalog)
        {
            if (catalog != null) _catalog = catalog;
        }

        public void BindInventory(
            Func<string, int> getCount,
            Func<string, int, bool> canAdd,
            Action<string, int> addItem,
            Action<string, int> consume)
        {
            _getCount = getCount ?? _getCount;
            _canAdd = canAdd ?? _canAdd;
            _addItem = addItem ?? _addItem;
            _consume = consume ?? _consume;
        }

        private TabletFormulationDef? FindFormulation(string formulationId)
        {
            foreach (var f in _catalog.formulations)
                if (f.formulation_id == formulationId) return f;
            return null;
        }

        private TabletReleaseClassDef? FindReleaseClass(string releaseClassId)
        {
            foreach (var r in _catalog.release_classes)
                if (r.release_class_id == releaseClassId) return r;
            return null;
        }

        // ── Construction & upkeep ───────────────────────────────────────

        public ActionResult ConstructPress()
        {
            if (_state.constructed)
                return ActionResult.Blocked(PharmaceuticalTabletFailures.AlreadyConstructed, "tab.already_constructed");

            var def = _catalog.press;
            foreach (var cost in def.construction_required_items)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(PharmaceuticalTabletFailures.MaterialsMissing, "tab.missing_construction_material");
            }
            foreach (var cost in def.construction_required_items)
                _consume(cost.Key, cost.Value);

            _state = new PharmaceuticalTabletPressState
            {
                schema_version = 1,
                press_id = def.press_id,
                constructed = true,
                machine_condition = def.max_condition,
                tooling_condition = 100f,
                calibration_state = 1f,
                machine_state = "idle"
            };
            Raise("tab.press_constructed");
            return ActionResult.Success("tab.press_constructed");
        }

        public ActionResult MaintainPress()
        {
            if (!_state.constructed)
                return ActionResult.Failed(PharmaceuticalTabletFailures.NotConstructed, "tab.not_constructed");

            foreach (var cost in _catalog.press.maintenance_required_items)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(PharmaceuticalTabletFailures.MaterialsMissing, "tab.missing_maintenance_material");
            }
            foreach (var cost in _catalog.press.maintenance_required_items)
                _consume(cost.Key, cost.Value);

            _state.machine_condition = Math.Min(_catalog.press.max_condition, _state.machine_condition + 30f);
            _state.days_since_maintenance = 0;
            if (_state.machine_state == "maintenance_required" || _state.machine_state == "faulted")
            {
                _state.machine_state = "idle";
                _state.fault_state = string.Empty;
            }
            Raise("tab.press_maintained");
            return ActionResult.Success("tab.press_maintained");
        }

        /// <summary>Replaces press tooling (fresh dies) — resets wear-driven rejects.</summary>
        public ActionResult ReplaceTooling()
        {
            if (!_state.constructed)
                return ActionResult.Failed(PharmaceuticalTabletFailures.NotConstructed, "tab.not_constructed");

            foreach (var cost in _catalog.press.tooling_replacement_items)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(PharmaceuticalTabletFailures.MaterialsMissing, "tab.missing_tooling_material");
            }
            foreach (var cost in _catalog.press.tooling_replacement_items)
                _consume(cost.Key, cost.Value);

            float tech = TechnicianSkill();
            _state.tooling_condition = Math.Min(100f, 100f * (0.9f + 0.1f * tech));
            Raise("tab.tooling_replaced");
            return ActionResult.Success("tab.tooling_replaced");
        }

        /// <summary>Recalibrates the press — restores calibration quality.</summary>
        public ActionResult Recalibrate()
        {
            if (!_state.constructed)
                return ActionResult.Failed(PharmaceuticalTabletFailures.NotConstructed, "tab.not_constructed");
            if (_activeBatch != null)
                return ActionResult.Blocked(PharmaceuticalTabletFailures.MachineBusy, "tab.machine_busy");

            float tech = TechnicianSkill();
            _state.calibration_state = Math.Min(1f, 0.85f + 0.15f * tech);
            Raise("tab.press_recalibrated");
            return ActionResult.Success("tab.press_recalibrated");
        }

        // ── Batch lifecycle ─────────────────────────────────────────────

        /// <summary>
        /// Stages a batch: validates the formulation, consumes precursors,
        /// binder and packaging resources, and moves the press to staging.
        /// Deterministic; repeated staging with a live batch is blocked.
        /// </summary>
        public ActionResult StageBatch(string formulationId)
        {
            if (!_state.constructed)
                return ActionResult.Failed(PharmaceuticalTabletFailures.NotConstructed, "tab.not_constructed");
            if (_activeBatch != null || _state.machine_state != "idle" && _state.machine_state != "maintenance_required")
                return ActionResult.Blocked(PharmaceuticalTabletFailures.MachineBusy, "tab.machine_busy");
            if (_state.machine_state == "faulted")
                return ActionResult.Blocked(PharmaceuticalTabletFailures.MachineFaulted, "tab.machine_faulted");
            var formulation = FindFormulation(formulationId);
            if (formulation == null)
                return ActionResult.Failed(PharmaceuticalTabletFailures.UnknownFormulation, "tab.unknown_formulation");
            if (_outputBuffer.Count >= _catalog.output_buffer_max_batches)
                return ActionResult.Blocked(PharmaceuticalTabletFailures.BufferFull, "tab.buffer_full");
            if (_state.machine_condition < 20f)
                return ActionResult.Blocked(PharmaceuticalTabletFailures.MaintenanceRequired, "tab.condition_critical");

            // Canonical inputs: precursors + binder + packaging.
            var allCosts = new Dictionary<string, int>(formulation.precursor_costs, StringComparer.Ordinal);
            if (!string.IsNullOrEmpty(formulation.binder_resource_id))
                allCosts[formulation.binder_resource_id] = formulation.binder_amount;
            if (!string.IsNullOrEmpty(formulation.packaging_resource_id))
                allCosts[formulation.packaging_resource_id] = 1;

            foreach (var cost in allCosts)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(PharmaceuticalTabletFailures.MaterialsMissing, "tab.missing_batch_material");
            }
            foreach (var cost in allCosts)
                _consume(cost.Key, cost.Value);

            var release = FindReleaseClass(formulation.release_class_id);
            int processDays = 3 + (release?.extra_process_days ?? 0);

            _activeBatch = new TabletBatchState
            {
                batch_id = $"batch_{formulation.formulation_id}_{_state.next_batch_number}",
                formulation_id = formulation.formulation_id,
                release_class_id = formulation.release_class_id,
                input_batch_size = formulation.base_batch_size,
                total_process_days = processDays,
                machine_state_at_save = "staging"
            };
            _state.next_batch_number++;
            _state.current_formulation_id = formulationId;
            _state.machine_state = "staging";
            _state.batch_progress = 0f;
            _state.fault_state = string.Empty;

            // Packaging quality is fixed by the resource grade at staging time.
            _state.packaging_quality = string.Equals(formulation.packaging_resource_id, "item_sealed_packaging_foil", StringComparison.Ordinal)
                ? 0.95f : 0.7f;

            Raise("tab.batch_staged");
            return ActionResult.Success("tab.batch_staged");
        }

        /// <summary>
        /// Daily press tick — active batches only, no per-frame simulation.
        /// Deterministic given state, catalog and seed.
        /// </summary>
        public void TickDay(int day)
        {
            if (!_state.constructed) return;

            _state.days_since_maintenance++;
            if (_activeBatch == null)
            {
                if (_state.machine_state != "faulted" && _state.machine_state != "maintenance_required" && _state.machine_state != "idle")
                    _state.machine_state = "idle";
                return;
            }

            var formulation = FindFormulation(_activeBatch.formulation_id);
            if (formulation == null)
            {
                // Unknown formulation (catalog changed) — fail safe, no output.
                _state.machine_state = "faulted";
                _state.fault_state = "formulation_missing";
                return;
            }

            var release = FindReleaseClass(_activeBatch.release_class_id);
            _activeBatch.progress_days++;
            _state.batch_progress = Math.Clamp((float)_activeBatch.progress_days / _activeBatch.total_process_days, 0f, 1f);

            // Fault roll: weighted by machine wear; chemist vigilance reduces.
            int faultBp = (int)(1500 * (1f - _state.machine_condition / 100f) * (1f - 0.4f * ChemistSkill()));
            if (_rng.Next(0, 10000) < faultBp)
            {
                _state.machine_state = "faulted";
                _state.fault_state = "press_jam";
                _activeBatch = null;
                _state.current_formulation_id = string.Empty;
                Raise("tab.batch_faulted");
                _log.Warn("[TabletPress] Batch lost to press fault — machine requires service.");
                return;
            }

            switch (_activeBatch.progress_days)
            {
                case 1: // blending → uniformity
                    _state.machine_state = "blending";
                    _state.uniformity_quality = Math.Clamp(
                        0.4f * _state.calibration_state + 0.3f * (_state.machine_condition / 100f) + 0.3f * ChemistSkill()
                        + (float)(_rng.NextDouble() - 0.5) * 0.04f, 0f, 1f);
                    break;

                case 2: // forming → integrity + tooling wear
                    _state.machine_state = "forming";
                    _state.tablet_integrity_quality = Math.Clamp(
                        0.5f * (_state.tooling_condition / 100f) + 0.25f * ChemistSkill() + 0.25f * TechnicianSkill()
                        + (float)(_rng.NextDouble() - 0.5) * 0.04f, 0f, 1f);
                    _state.tooling_condition = Math.Max(0f, _state.tooling_condition - _catalog.press.tooling_wear_per_batch);
                    break;

                default:
                    if (_activeBatch.progress_days == 2 + (release?.extra_process_days ?? 0) && release != null && release.release_class_id != "immediate")
                    {
                        // Coating phase (delayed/protected release abstraction).
                        _state.machine_state = "coating";
                        _state.coating_quality = Math.Clamp(
                            MathF.Max(release.coating_quality_floor, 0.5f * _state.calibration_state + 0.5f * ChemistSkill()), 0f, 1f);
                    }
                    else if (_activeBatch.progress_days >= _activeBatch.total_process_days)
                    {
                        ResolveBatch(day, formulation, release);
                    }
                    else
                    {
                        _state.machine_state = _activeBatch.progress_days == 3 ? "packaging" : _state.machine_state;
                    }
                    break;
            }
        }

        private void ResolveBatch(int day, TabletFormulationDef formulation, TabletReleaseClassDef? release)
        {
            if (_activeBatch == null) return;

            _state.machine_state = "quality_check";
            _activeBatch.machine_state_at_save = "quality_check";

            // Reject fraction driven by tooling wear; technician skill reduces.
            float wear = 1f - _state.tooling_condition / 100f;
            float rejectFraction = Math.Clamp(wear * 0.6f - 0.25f * TechnicianSkill(), 0f, 0.6f);
            _state.reject_fraction = rejectFraction;

            // Composite quality — packaging improves shelf life, not dose count.
            float coating = release == null || release.release_class_id == "immediate" ? 1f : _state.coating_quality;
            float composite = 0.35f * _state.uniformity_quality
                              + 0.35f * _state.tablet_integrity_quality
                              + 0.15f * coating
                              + 0.15f * _state.packaging_quality;

            bool accepted = composite >= formulation.quality_threshold && rejectFraction < 0.35f;

            if (!accepted)
            {
                // Rejected batch: no valid medicine output is created, ever.
                _state.machine_state = "rejected";
                _activeBatch.quality_resolved = true;
                var rejected = new PharmaceuticalBatchResult
                {
                    batch_id = _activeBatch.batch_id,
                    result_item_id = formulation.medical_effect_item_id,
                    quantity = 0,
                    quality_grade = "rejected",
                    shelf_life_class = release?.shelf_life_class ?? "standard",
                    reject_quantity = _activeBatch.input_batch_size,
                    trade_value_modifier = 0f,
                    warning_tags = { "batch_rejected" },
                    completed_day = day
                };
                OnBatchResolved?.Invoke(rejected, false);
                _activeBatch = null;
                _state.current_formulation_id = string.Empty;
                Raise("tab.batch_rejected");
                return;
            }

            int quantity = (int)MathF.Round(formulation.base_batch_size * (1f - rejectFraction));
            var result = new PharmaceuticalBatchResult
            {
                batch_id = _activeBatch.batch_id,
                result_item_id = formulation.medical_effect_item_id,
                quantity = quantity,
                quality_grade = composite >= 0.85f ? "excellent" : composite >= 0.7f ? "good" : "fair",
                shelf_life_class = ComputeShelfLife(formulation, release, _state.packaging_quality),
                reject_quantity = formulation.base_batch_size - quantity,
                trade_value_modifier = MathF.Min(2f, formulation.trade_value_modifier * (0.6f + 0.4f * composite)
                    + (release?.trade_value_bonus ?? 0f)),
                warning_tags = { },
                completed_day = day
            };
            if (rejectFraction > 0.15f) result.warning_tags.Add("high_rejects");
            if (_state.tooling_condition < 30f) result.warning_tags.Add("tooling_worn");

            _state.machine_state = "complete";
            _activeBatch.quality_resolved = true;
            _outputBuffer.Add(result);
            _state.cycle_count++;
            OnBatchResolved?.Invoke(result, true);
            _activeBatch = null;
            _state.current_formulation_id = string.Empty;
            Raise("tab.batch_completed");
        }

        private static string ComputeShelfLife(TabletFormulationDef formulation, TabletReleaseClassDef? release, float packagingQuality)
        {
            string baseClass = release?.shelf_life_class ?? "standard";
            // Good sealed packaging raises the class by one step, never two.
            if (packagingQuality >= 0.9f)
            {
                return baseClass switch
                {
                    "short" => "standard",
                    "standard" => "extended",
                    _ => baseClass
                };
            }
            return baseClass;
        }

        /// <summary>
        /// Claims buffered batch outputs into canonical inventory. Partial
        /// claims retry safely — a batch is only consumed when everything
        /// fits (never lost, never duplicated).
        /// </summary>
        public ClaimedTabletOutputs? ClaimOutputs()
        {
            if (_outputBuffer.Count == 0)
                return null;

            var totals = new ClaimedTabletOutputs();
            var remaining = new List<PharmaceuticalBatchResult>();
            foreach (var result in _outputBuffer)
            {
                if (result.quantity <= 0)
                    continue; // rejected residue — drop without creating items
                if (!_canAdd(result.result_item_id, result.quantity))
                {
                    remaining.Add(result);
                    continue;
                }
                _addItem(result.result_item_id, result.quantity);
                totals.batches_claimed++;
                totals.total_units += result.quantity;
                if (!totals.item_units.TryGetValue(result.result_item_id, out var u))
                    totals.item_units[result.result_item_id] = 0;
                totals.item_units[result.result_item_id] += result.quantity;
            }
            _outputBuffer.Clear();
            _outputBuffer.AddRange(remaining);
            _state.machine_state = _outputBuffer.Count > 0 ? _state.machine_state : "idle";
            if (totals.batches_claimed == 0) return null;
            Raise("tab.outputs_claimed");
            return totals;
        }

        // ── Persistence ─────────────────────────────────────────────────

        public PharmaceuticalTabletPressState CaptureState()
        {
            var clone = new PharmaceuticalTabletPressState
            {
                schema_version = _state.schema_version,
                press_id = _state.press_id,
                constructed = _state.constructed,
                machine_condition = _state.machine_condition,
                tooling_condition = _state.tooling_condition,
                calibration_state = _state.calibration_state,
                current_formulation_id = _state.current_formulation_id,
                machine_state = _state.machine_state,
                batch_progress = _state.batch_progress,
                uniformity_quality = _state.uniformity_quality,
                tablet_integrity_quality = _state.tablet_integrity_quality,
                coating_quality = _state.coating_quality,
                packaging_quality = _state.packaging_quality,
                reject_fraction = _state.reject_fraction,
                fault_state = _state.fault_state,
                cycle_count = _state.cycle_count,
                days_since_maintenance = _state.days_since_maintenance,
                next_batch_number = _state.next_batch_number
            };
            if (_activeBatch != null)
            {
                clone.batch_progress = _state.batch_progress;
            }
            return clone;
        }

        /// <summary>Capture the full engine state including the active batch and buffer.</summary>
        public TabletWorksFullState CaptureFullState()
        {
            return new TabletWorksFullState
            {
                press = CaptureState(),
                active_batch = _activeBatch == null ? null : new TabletBatchState
                {
                    batch_id = _activeBatch.batch_id,
                    formulation_id = _activeBatch.formulation_id,
                    release_class_id = _activeBatch.release_class_id,
                    input_batch_size = _activeBatch.input_batch_size,
                    progress_days = _activeBatch.progress_days,
                    total_process_days = _activeBatch.total_process_days,
                    machine_state_at_save = _activeBatch.machine_state_at_save,
                    quality_resolved = _activeBatch.quality_resolved,
                    packaging_applied = _activeBatch.packaging_applied
                },
                output_buffer = _outputBuffer.Select(r => new PharmaceuticalBatchResult
                {
                    batch_id = r.batch_id,
                    result_item_id = r.result_item_id,
                    quantity = r.quantity,
                    quality_grade = r.quality_grade,
                    shelf_life_class = r.shelf_life_class,
                    reject_quantity = r.reject_quantity,
                    trade_value_modifier = r.trade_value_modifier,
                    warning_tags = new List<string>(r.warning_tags),
                    completed_day = r.completed_day
                }).ToList()
            };
        }

        public void RestoreFullState(TabletWorksFullState? state)
        {
            if (state == null) return; // Old saves: unbuilt press default.
            if (state.press != null)
            {
                _state = state.press;
                if (_state.schema_version < 1 || _state.schema_version > 1) _state.schema_version = 1;
            }
            _activeBatch = state.active_batch;
            _outputBuffer.Clear();
            if (state.output_buffer != null) _outputBuffer.AddRange(state.output_buffer);
            // A batch saved pre-resolution re-runs its remaining days
            // deterministically; a resolved batch is never re-resolved
            // (quality_resolved is honored by the tick state machine).
        }

        private void Raise(string eventId) => OnEventRaised?.Invoke(eventId);
    }

    /// <summary>Full persistence payload (press + active batch + buffer).</summary>
    [Serializable]
    public sealed class TabletWorksFullState
    {
        public int schema_version { get; set; } = 1;
        public PharmaceuticalTabletPressState? press { get; set; }
        public TabletBatchState? active_batch { get; set; }
        public List<PharmaceuticalBatchResult> output_buffer { get; set; } = new List<PharmaceuticalBatchResult>();
    }

    public sealed class ClaimedTabletOutputs
    {
        public int batches_claimed { get; set; }
        public int total_units { get; set; }
        public Dictionary<string, int> item_units { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
    }
}
