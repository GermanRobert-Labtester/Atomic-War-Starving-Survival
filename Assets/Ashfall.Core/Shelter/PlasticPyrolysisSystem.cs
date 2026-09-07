// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    // ─────────────────────────────────────────────────────────────────
    // Plan 202 — Subterranean hydrocarbon pyrolysis (waste-plastic to
    // synthetic fuel fractions). Catalog-driven batch process. All
    // randomness flows through the injected ISeededRng; the host projects
    // power availability; inventory flows through injected delegates so the
    // engine never touches a host object. Outputs are canonical Fuel/Material
    // items — no isolated industrial inventory.
    // ─────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class PyrolysisMachineDef
    {
        public string machine_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
        public int construction_labor_days { get; set; } = 2;
        public float max_condition { get; set; } = 100f;
        public int maintenance_interval_days { get; set; } = 6;
        public Dictionary<string, int> maintenance_required_items { get; set; } = new Dictionary<string, int>();
        public string room_id { get; set; } = string.Empty;
        public float ventilation_load_per_active_day { get; set; } = 0.04f;
    }

    [Serializable]
    public sealed class PyrolysisFeedstockProfile
    {
        public string feedstock_profile_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public List<string> accepted_item_ids { get; set; } = new List<string>();
        public string contamination_level { get; set; } = "mixed"; // clean | mixed | contaminated
        public int batch_input_units { get; set; } = 10;
        public float energy_cost_kwh_per_day { get; set; } = 6f;
        public int process_duration_days { get; set; } = 2;
        public int liquid_fuel_yield_units { get; set; }
        public int light_fraction_yield_units { get; set; }
        public int solid_carbon_yield_units { get; set; }
        // Bounded mass accounting (kg per batch) — documented losses, never created mass.
        public float input_mass_kg { get; set; }
        public float liquid_mass_kg { get; set; }
        public float light_mass_kg { get; set; }
        public float carbon_mass_kg { get; set; }
        public float offgas_energy_credit_kwh { get; set; }
        public int hazard_risk_bp { get; set; } = 100;
        public float equipment_wear_per_batch { get; set; } = 2f;
        public float operator_skill_risk_reduction_pct { get; set; } = 30f;
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class PyrolysisHazardOutcomes
    {
        public int quality_loss_weight_pct { get; set; } = 55;
        public int machine_damage_weight_pct { get; set; } = 25;
        public float machine_damage_condition_loss { get; set; } = 15f;
        public int fire_event_weight_pct { get; set; } = 12;
        public int gas_release_weight_pct { get; set; } = 8;
        public float gas_release_severity { get; set; } = 0.35f;
    }

    [Serializable]
    public sealed class PlasticPyrolysisCatalog
    {
        public int schema_version { get; set; } = 1;
        public PyrolysisMachineDef machine { get; set; } = new PyrolysisMachineDef();
        public List<PyrolysisFeedstockProfile> feedstock_profiles { get; set; } = new List<PyrolysisFeedstockProfile>();
        public PyrolysisOutputMap outputs { get; set; } = new PyrolysisOutputMap();
        public PyrolysisHazardOutcomes hazard_outcomes { get; set; } = new PyrolysisHazardOutcomes();
        /// <summary>Offgas energy credit may never exceed this share of the batch's energy cost.</summary>
        public float offgas_credit_cap_pct { get; set; } = 40f;
        public int storage_buffer_max_batches { get; set; } = 3;
    }

    [Serializable]
    public sealed class PyrolysisOutputMap
    {
        public string liquid_fuel_item_id { get; set; } = "synthetic_fuel_canister";
        public string light_fraction_item_id { get; set; } = "fuel_1l";
        public string solid_carbon_item_id { get; set; } = "carbon_black_powder";
    }

    [Serializable]
    public sealed class PyrolysisBatchState
    {
        public string batch_id { get; set; } = string.Empty;
        public string feedstock_profile_id { get; set; } = string.Empty;
        public string feedstock_item_id { get; set; } = string.Empty;
        public int input_units { get; set; }
        public int progress_days { get; set; }
        public string phase { get; set; } = "reacting"; // reacting | stalling
        public bool quality_degraded { get; set; }
        public int stall_days_total { get; set; }
        public int started_day { get; set; }
        public float operator_skill { get; set; } = 0.5f;
    }

    [Serializable]
    public sealed class PyrolysisOutputBatch
    {
        public string batch_id { get; set; } = string.Empty;
        public string feedstock_profile_id { get; set; } = string.Empty;
        public int liquid_fuel_units { get; set; }
        public int light_fraction_units { get; set; }
        public int solid_carbon_units { get; set; }
        public bool quality_degraded { get; set; }
        public int completed_day { get; set; }
    }

    [Serializable]
    public sealed class PlasticPyrolysisState
    {
        public int schema_version { get; set; } = 1;
        public bool machine_constructed { get; set; }
        public float machine_condition { get; set; } = 100f;
        public int days_since_maintenance { get; set; }
        public PyrolysisBatchState? active_batch { get; set; }
        public List<PyrolysisOutputBatch> output_buffer { get; set; } = new List<PyrolysisOutputBatch>();
        public int total_batches_completed { get; set; }
        public int total_incidents { get; set; }
        public int next_batch_number { get; set; } = 1;
    }

    /// <summary>Typed failure codes for the Godot host to format (never raw strings in UI).</summary>
    public static class PyrolysisFailures
    {
        public const string FeedstockInvalid = "pyro.feedstock_invalid";
        public const string MachineUnavailable = "pyro.machine_unavailable";
        public const string BatchAlreadyActive = "pyro.batch_already_active";
        public const string StorageUnavailable = "pyro.storage_unavailable";
        public const string MaintenanceRequired = "pyro.maintenance_required";
        public const string NotConstructed = "pyro.not_constructed";
        public const string MaterialsMissing = "pyro.materials_missing";
        public const string NothingToClaim = "pyro.nothing_to_claim";
    }

    public sealed class PlasticPyrolysisSystem
    {
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private PlasticPyrolysisCatalog _catalog = new PlasticPyrolysisCatalog();
        private PlasticPyrolysisState _state = new PlasticPyrolysisState();

        // Inventory ports (host-bound; deterministic).
        private Func<string, int> _getCount = _ => 0;
        private Func<string, int, bool> _canAdd = (_, _) => false;
        private Action<string, int> _addItem = (_, _) => { };
        private Action<string, int> _consume = (_, _) => { };

        /// <summary>Host-injected campaign day provider (deterministic — never wall-clock).</summary>
        public Func<int>? DayProvider { get; set; }
        /// <summary>Host-injected operator skill in [0,1]; duty-roster projection is host work.</summary>
        public Func<float>? OperatorSkillProvider { get; set; }

        private int CurrentDay() => DayProvider?.Invoke() ?? 0;
        private float OperatorSkill() => Math.Clamp(OperatorSkillProvider?.Invoke() ?? 0.5f, 0f, 1f);

        /// <summary>Read-only inventory probe for presentation projections.</summary>
        public int CountItem(string itemId) => _getCount(itemId);

        public PlasticPyrolysisState State => _state;
        public PlasticPyrolysisCatalog Catalog => _catalog;

        public event Action<PyrolysisBatchState>? OnBatchCompleted;
        public event Action<PyrolysisBatchState, string>? OnIncident;        // batch, outcome kind
        public event Action<float>? OnGasRelease;                            // severity 0..1 → ventilation authority
        public event Action<string>? OnFireIncident;                         // machineId → hazard authority
        public event Action<PyrolysisOutputBatch>? OnOutputsClaimed;
        public event Action<string>? OnEventRaised;

        public PlasticPyrolysisSystem(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(2020);
            _log = log ?? NullLog.Instance;
        }

        public void BindCatalog(PlasticPyrolysisCatalog catalog)
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

        public PyrolysisFeedstockProfile? FindProfile(string profileId)
        {
            foreach (var p in _catalog.feedstock_profiles)
                if (p.feedstock_profile_id == profileId) return p;
            return null;
        }

        // ── Construction & maintenance ──────────────────────────────────

        public ActionResult ConstructMachine()
        {
            if (_state.machine_constructed)
                return ActionResult.Blocked(PyrolysisFailures.MachineUnavailable, "pyro.already_constructed");

            var def = _catalog.machine;
            foreach (var cost in def.construction_required_items)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(PyrolysisFailures.MaterialsMissing, "pyro.missing_construction_material");
            }
            foreach (var cost in def.construction_required_items)
                _consume(cost.Key, cost.Value);

            _state.machine_constructed = true;
            _state.machine_condition = def.max_condition;
            _state.days_since_maintenance = 0;
            Raise("pyro.machine_constructed");
            _log.Info("[Pyrolysis] Retort bay constructed.");
            return ActionResult.Success("pyro.machine_constructed");
        }

        public ActionResult PerformMaintenance()
        {
            if (!_state.machine_constructed)
                return ActionResult.Failed(PyrolysisFailures.NotConstructed, "pyro.not_constructed");

            foreach (var cost in _catalog.machine.maintenance_required_items)
            {
                if (_getCount(cost.Key) < cost.Value)
                    return ActionResult.Blocked(PyrolysisFailures.MaterialsMissing, "pyro.missing_maintenance_material");
            }
            foreach (var cost in _catalog.machine.maintenance_required_items)
                _consume(cost.Key, cost.Value);

            _state.machine_condition = Math.Min(_catalog.machine.max_condition, _state.machine_condition + 35f);
            _state.days_since_maintenance = 0;
            Raise("pyro.maintained");
            return ActionResult.Success("pyro.maintained");
        }

        // ── Batching ────────────────────────────────────────────────────

        public ActionResult StartBatch(string profileId)
        {
            if (!_state.machine_constructed)
                return ActionResult.Failed(PyrolysisFailures.NotConstructed, "pyro.not_constructed");
            if (_state.active_batch != null)
                return ActionResult.Blocked(PyrolysisFailures.BatchAlreadyActive, "pyro.batch_active");
            if (_state.machine_condition < 25f)
                return ActionResult.Blocked(PyrolysisFailures.MaintenanceRequired, "pyro.condition_critical");
            if (_state.output_buffer.Count >= _catalog.storage_buffer_max_batches)
                return ActionResult.Blocked(PyrolysisFailures.StorageUnavailable, "pyro.buffer_full");

            var profile = FindProfile(profileId);
            if (profile == null)
                return ActionResult.Failed(PyrolysisFailures.FeedstockInvalid, "pyro.unknown_profile");

            // Feedstock must be present in batch quantity from a single accepted item id.
            string? sourceItem = null;
            foreach (var id in profile.accepted_item_ids)
            {
                if (_getCount(id) >= profile.batch_input_units) { sourceItem = id; break; }
            }
            if (sourceItem == null)
                return ActionResult.Blocked(PyrolysisFailures.FeedstockInvalid, "pyro.insufficient_feedstock");

            _consume(sourceItem, profile.batch_input_units);

            _state.active_batch = new PyrolysisBatchState
            {
                batch_id = $"batch_{_state.next_batch_number}",
                feedstock_profile_id = profileId,
                feedstock_item_id = sourceItem,
                input_units = profile.batch_input_units,
                progress_days = 0,
                phase = "reacting",
                started_day = CurrentDay(),
                operator_skill = OperatorSkill()
            };
            _state.next_batch_number++;
            Raise("pyro.batch_started");
            _log.Info($"[Pyrolysis] Batch started: {profileId} ({sourceItem}).");
            return ActionResult.Success("pyro.batch_started");
        }

        /// <summary>
        /// Advances one day. <paramref name="gridPowerAvailableKwh"/> is the energy the
        /// shelter can actually supply today; a deficit stalls the batch (no progress,
        /// no hazard roll — the retort is simply cold). Returns the offgas energy credit
        /// harvested on a completion day (never exceeds the capped share of total cost).
        /// </summary>
        public float TickDay(float gridPowerAvailableKwh)
        {
            if (_state.days_since_maintenance >= 0)
                _state.days_since_maintenance++;

            var batch = _state.active_batch;
            if (batch == null) return 0f;

            var profile = FindProfile(batch.feedstock_profile_id);
            if (profile == null)
            {
                // Unknown profile (catalog changed under a save) — fail the batch safely.
                _state.active_batch = null;
                Raise("pyro.batch_lost_unknown_profile");
                return 0f;
            }

            float needed = profile.energy_cost_kwh_per_day;
            if (gridPowerAvailableKwh + 0.001f < needed)
            {
                batch.phase = "stalling";
                batch.stall_days_total++;
                Raise("pyro.batch_stalled");
                return 0f;
            }

            batch.phase = "reacting";
            batch.progress_days++;

            if (batch.progress_days < profile.process_duration_days)
                return 0f;

            // ── Completion: hazard roll, then yields ──
            // Plan 202 Wave F: the roll derives from the campaign day (fresh-seed
            // house pattern) — identical outcomes for continuous and save/load
            // split runs, since no engine-internal RNG sequence is carried.
            float risk = RollBatchRisk(profile, batch.operator_skill);
            var hazardRng = new SeededRng(unchecked(CurrentDay() * 7919 + 101));
            if (hazardRng.NextDouble() < risk)
                ApplyIncident(profile, batch);

            bool machineDestroyed = _state.machine_condition <= 0f;
            int qualityNumerator = batch.quality_degraded ? 2 : 3; // degraded batches lose 1/3 of yields
            CompleteBatch(profile, batch, qualityNumerator);
            _state.active_batch = null;

            if (machineDestroyed)
            {
                _state.machine_constructed = false;
                OnFireIncident?.Invoke(_catalog.machine.machine_id);
                Raise("pyro.machine_destroyed");
            }

            float credit = ComputeOffgasCredit(profile);
            return credit;
        }

        private float RollBatchRisk(PyrolysisFeedstockProfile profile, float operatorSkill)
        {
            float risk = profile.hazard_risk_bp / 10000f;
            // Poor condition raises risk up to +100%.
            float conditionFactor = 1f + Math.Max(0f, (50f - _state.machine_condition) / 50f);
            risk *= conditionFactor;
            // Maintenance overdue raises risk up to +50%.
            int overdue = Math.Max(0, _state.days_since_maintenance - _catalog.machine.maintenance_interval_days);
            risk *= 1f + Math.Min(0.5f, overdue * 0.05f);
            // Contaminated feedstock raises risk up to +50%.
            if (profile.contamination_level == "contaminated") risk *= 1.5f;
            else if (profile.contamination_level == "mixed") risk *= 1.2f;
            // Operator skill reduces risk by the profile's bounded reduction share.
            float reduction = Math.Clamp(operatorSkill, 0f, 1f) * profile.operator_skill_risk_reduction_pct / 100f;
            risk *= 1f - reduction;
            return Math.Min(0.9f, risk);
        }

        private void ApplyIncident(PyrolysisFeedstockProfile profile, PyrolysisBatchState batch)
        {
            _state.total_incidents++;
            var hazard = _catalog.hazard_outcomes;
            var incidentRng = new SeededRng(unchecked(CurrentDay() * 613 + batch.started_day * 17 + 7));
            int roll = (int)(incidentRng.NextDouble() * 100);
            int cumulative = 0;

            cumulative += hazard.quality_loss_weight_pct;
            if (roll < cumulative)
            {
                batch.quality_degraded = true;
                OnIncident?.Invoke(batch, "quality_loss");
                Raise("pyro.incident_quality_loss");
                return;
            }
            cumulative += hazard.machine_damage_weight_pct;
            if (roll < cumulative)
            {
                _state.machine_condition = Math.Max(0f, _state.machine_condition - hazard.machine_damage_condition_loss);
                OnIncident?.Invoke(batch, "machine_damage");
                Raise("pyro.incident_machine_damage");
                return;
            }
            cumulative += hazard.fire_event_weight_pct;
            if (roll < cumulative)
            {
                _state.machine_condition = Math.Max(0f, _state.machine_condition - hazard.machine_damage_condition_loss * 2f);
                OnFireIncident?.Invoke(_catalog.machine.machine_id);
                OnIncident?.Invoke(batch, "fire");
                Raise("pyro.incident_fire");
                return;
            }
            // Remaining weight: gas release → ventilation authority owns the consequence.
            OnGasRelease?.Invoke(Math.Clamp(hazard.gas_release_severity, 0f, 1f));
            OnIncident?.Invoke(batch, "gas_release");
            Raise("pyro.incident_gas_release");
        }

        private void CompleteBatch(PyrolysisFeedstockProfile profile, PyrolysisBatchState batch, int qualityNumerator)
        {
            int liquid = profile.liquid_fuel_yield_units * qualityNumerator / 3;
            int light = profile.light_fraction_yield_units * qualityNumerator / 3;
            int carbon = profile.solid_carbon_yield_units * qualityNumerator / 3;

            _state.output_buffer.Add(new PyrolysisOutputBatch
            {
                batch_id = batch.batch_id,
                feedstock_profile_id = batch.feedstock_profile_id,
                liquid_fuel_units = liquid,
                light_fraction_units = light,
                solid_carbon_units = carbon,
                quality_degraded = batch.quality_degraded,
                completed_day = batch.started_day + batch.progress_days
            });
            _state.total_batches_completed++;
            _state.machine_condition = Math.Max(0f, _state.machine_condition - profile.equipment_wear_per_batch);
            OnBatchCompleted?.Invoke(batch);
        }

        /// <summary>
        /// Offgas energy credit for a completed batch: the credit itself, capped so it can
        /// never exceed <see cref="PlasticPyrolysisCatalog.offgas_credit_cap_pct"/> of the
        /// batch's total energy cost — the process is never net energy positive.
        /// </summary>
        public float ComputeOffgasCredit(PyrolysisFeedstockProfile profile)
        {
            float totalCost = profile.energy_cost_kwh_per_day * profile.process_duration_days;
            float cap = totalCost * _catalog.offgas_credit_cap_pct / 100f;
            return Math.Min(profile.offgas_energy_credit_kwh, cap);
        }

        // ── Output claim ────────────────────────────────────────────────

        /// <summary>
        /// Claims all buffered outputs into canonical inventory. Idempotent per batch:
        /// claimed batches are removed from the buffer, so a repeated call cannot
        /// duplicate items. Returns null when the buffer is empty.
        /// </summary>
        public ClaimedPyrolysisOutputs? ClaimOutputs()
        {
            if (_state.output_buffer.Count == 0)
                return null;

            var totals = new ClaimedPyrolysisOutputs();
            var outIds = _catalog.outputs;
            var remaining = new List<PyrolysisOutputBatch>();
            foreach (var ob in _state.output_buffer)
            {
                // Respect inventory capacity: if the sink cannot take an output, keep the
                // batch buffered (claim is partial, retryable — never lost, never duplicated).
                bool allFit =
                    (ob.liquid_fuel_units == 0 || _canAdd(outIds.liquid_fuel_item_id, ob.liquid_fuel_units)) &&
                    (ob.light_fraction_units == 0 || _canAdd(outIds.light_fraction_item_id, ob.light_fraction_units)) &&
                    (ob.solid_carbon_units == 0 || _canAdd(outIds.solid_carbon_item_id, ob.solid_carbon_units));
                if (!allFit)
                {
                    remaining.Add(ob);
                    continue;
                }

                if (ob.liquid_fuel_units > 0) _addItem(outIds.liquid_fuel_item_id, ob.liquid_fuel_units);
                if (ob.light_fraction_units > 0) _addItem(outIds.light_fraction_item_id, ob.light_fraction_units);
                if (ob.solid_carbon_units > 0) _addItem(outIds.solid_carbon_item_id, ob.solid_carbon_units);

                totals.liquid_fuel_units += ob.liquid_fuel_units;
                totals.light_fraction_units += ob.light_fraction_units;
                totals.solid_carbon_units += ob.solid_carbon_units;
                totals.batches_claimed++;
                OnOutputsClaimed?.Invoke(ob);
            }

            _state.output_buffer = remaining;
            if (totals.batches_claimed == 0) return null;
            Raise("pyro.outputs_claimed");
            return totals;
        }

        // ── Persistence ─────────────────────────────────────────────────

        public PlasticPyrolysisState CaptureState()
        {
            // State is already a plain serializable DTO — clone defensively.
            var clone = new PlasticPyrolysisState
            {
                schema_version = _state.schema_version,
                machine_constructed = _state.machine_constructed,
                machine_condition = _state.machine_condition,
                days_since_maintenance = _state.days_since_maintenance,
                output_buffer = new List<PyrolysisOutputBatch>(_state.output_buffer),
                total_batches_completed = _state.total_batches_completed,
                total_incidents = _state.total_incidents,
                next_batch_number = _state.next_batch_number
            };
            if (_state.active_batch != null)
                clone.active_batch = new PyrolysisBatchState
                {
                    batch_id = _state.active_batch.batch_id,
                    feedstock_profile_id = _state.active_batch.feedstock_profile_id,
                    feedstock_item_id = _state.active_batch.feedstock_item_id,
                    input_units = _state.active_batch.input_units,
                    progress_days = _state.active_batch.progress_days,
                    phase = _state.active_batch.phase,
                    quality_degraded = _state.active_batch.quality_degraded,
                    stall_days_total = _state.active_batch.stall_days_total,
                    started_day = _state.active_batch.started_day
                };
            return clone;
        }

        public void RestoreState(PlasticPyrolysisState? state)
        {
            if (state == null) return;
            _state = state;
            if (_state.output_buffer == null) _state.output_buffer = new List<PyrolysisOutputBatch>();
            if (_state.schema_version < 1 || _state.schema_version > 1)
                _state.schema_version = 1;
        }

        private void Raise(string eventId) => OnEventRaised?.Invoke(eventId);
    }

    public sealed class ClaimedPyrolysisOutputs
    {
        public int liquid_fuel_units { get; set; }
        public int light_fraction_units { get; set; }
        public int solid_carbon_units { get; set; }
        public int batches_claimed { get; set; }
    }
}
