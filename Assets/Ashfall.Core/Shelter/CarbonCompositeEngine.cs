// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum CompositeQualityGrade
    {
        Reject,
        Field,
        Structural,
        Certified
    }

    [Serializable]
    public sealed class CompositeJobState
    {
        public string job_id = string.Empty;
        public string component_id = string.Empty;
        public string material_profile_id = string.Empty;
        public int material_age_days;
        public int progress_ticks;
        public float operator_skill = 0.5f;
        public float defect_roll = 0.5f;
    }

    [Serializable]
    public sealed class CompositeOutputState
    {
        public string job_id = string.Empty;
        public string component_id = string.Empty;
        public string output_item_id = string.Empty;
        public CompositeQualityGrade quality;
        public float mass_factor = 1f;
        public float durability_factor = 1f;
        public bool certified;
    }

    [Serializable]
    public sealed class CarbonCompositeState
    {
        public int schema_version = 1;
        public ulong rng_state;
        public float autoclave_condition = 1f;
        public int next_job_number = 1;
        public CompositeJobState? active_job;
        public List<CompositeOutputState> output_buffer = new List<CompositeOutputState>();
    }

    public sealed class CompositeComponentProjection
    {
        public string ComponentId { get; set; } = string.Empty;
        public CompositeQualityGrade Quality { get; set; }
        public float MassFactor { get; set; } = 1f;
        public float DurabilityFactor { get; set; } = 1f;
        public bool Certified { get; set; }
    }

    public sealed class CompositeTickResult
    {
        public bool Progressed { get; set; }
        public bool Paused { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public CompositeOutputState? CompletedOutput { get; set; }
    }

    public static class CarbonCompositeFailureCodes
    {
        public const string ComponentUnknown = "composite.component_unknown";
        public const string MaterialMissing = "composite.material_missing";
        public const string MaterialExpired = "composite.material_expired";
        public const string AutoclaveUnavailable = "composite.autoclave_unavailable";
        public const string QualityBelowRequirement = "composite.quality_below_requirement";
        public const string OutputUnavailable = "composite.output_unavailable";
        public const string JobActive = "composite.job_active";
    }

    /// <summary>Abstract, deterministic quality pipeline for explicit composite components.</summary>
    public sealed class CarbonCompositeEngine
    {
        private readonly ISeededRng _rng;
        private CarbonCompositeCatalog _catalog;
        private CarbonCompositeState _state = new CarbonCompositeState();
        private IPlayerInventoryPort? _inventory;
        private int _tick;

        public CarbonCompositeEngine(ISeededRng? rng = null, CarbonCompositeCatalog? catalog = null)
        {
            _rng = rng ?? new SeededRng(120);
            _catalog = catalog ?? new CarbonCompositeCatalog(new CarbonCompositeCatalogDto
            {
                materials = new List<CompositeMaterialProfile> { new CompositeMaterialProfile { material_profile_id = "prepreg_standard" } },
                cures = new List<CompositeCureProfile> { new CompositeCureProfile { cure_profile_id = "precision_cure" } },
                components = new List<CompositeComponentProfile>
                {
                    new CompositeComponentProfile { component_id = "composite_sensor_housing", input_material_id = "prepreg_standard", output_item_id = "item_faraday_mesh", cure_profile_id = "precision_cure", required_quality = "structural", mass_factor = 0.8f, durability_factor = 1.1f }
                }
            });
        }

        public CarbonCompositeCatalog Catalog => _catalog;
        public CarbonCompositeState State => _state;
        public void BindCatalog(CarbonCompositeCatalog catalog) => _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        public void BindInventory(IPlayerInventoryPort inventory) => _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));

        public ActionResult StartJob(string componentId, string materialProfileId, int materialAgeDays = 0, float operatorSkill = 0.5f)
        {
            var component = _catalog.FindComponent(componentId);
            var material = _catalog.FindMaterial(materialProfileId);
            if (component == null || material == null)
                return ActionResult.Blocked(CarbonCompositeFailureCodes.ComponentUnknown, "composite.profile_unknown");
            if (_state.active_job != null)
                return ActionResult.Blocked(CarbonCompositeFailureCodes.JobActive, "composite.job_active");
            if (materialAgeDays > material.freshness_days)
                return ActionResult.Blocked(CarbonCompositeFailureCodes.MaterialExpired, "composite.material_expired");
            if (_inventory == null || !_inventory.HasSufficient(materialProfileId, component.input_quantity))
                return ActionResult.Blocked(CarbonCompositeFailureCodes.MaterialMissing, "composite.material_missing");
            if (!_inventory.TryConsume(materialProfileId, component.input_quantity))
                return ActionResult.Blocked(CarbonCompositeFailureCodes.MaterialMissing, "composite.material_missing");

            _state.active_job = new CompositeJobState
            {
                job_id = $"composite_job_{_state.next_job_number++}",
                component_id = componentId,
                material_profile_id = materialProfileId,
                material_age_days = Math.Max(0, materialAgeDays),
                operator_skill = Math.Clamp(operatorSkill, 0f, 1f),
                defect_roll = (float)_rng.NextDouble()
            };
            return ActionResult.Success("composite.job_started");
        }

        public CompositeTickResult Tick(float thermalConformity, float pressureConformity, float sealIntegrity, float coldStorage, float operatorSkill = 0.5f)
        {
            var job = _state.active_job;
            if (job == null) return new CompositeTickResult { StatusCode = "composite.idle" };
            var component = _catalog.FindComponent(job.component_id);
            if (component == null) return new CompositeTickResult { Paused = true, StatusCode = CarbonCompositeFailureCodes.ComponentUnknown };
            var cure = _catalog.FindCure(component.cure_profile_id);
            var material = _catalog.FindMaterial(job.material_profile_id);
            if (cure == null || material == null) return new CompositeTickResult { Paused = true, StatusCode = CarbonCompositeFailureCodes.ComponentUnknown };
            if (_state.autoclave_condition <= 0f) return new CompositeTickResult { Paused = true, StatusCode = CarbonCompositeFailureCodes.AutoclaveUnavailable };

            _tick++;
            job.progress_ticks++;
            _state.autoclave_condition = Math.Clamp(_state.autoclave_condition - 0.005f, 0f, 1f);
            if (job.progress_ticks < cure.process_ticks)
                return new CompositeTickResult { Progressed = true, StatusCode = "composite.progress" };

            float conformity = Math.Clamp(1f - (Math.Abs(thermalConformity - cure.target_thermal)
                + Math.Abs(pressureConformity - cure.target_pressure)) * 0.7f, 0f, 1f);
            conformity *= Math.Clamp(sealIntegrity, 0f, 1f);
            conformity *= Math.Clamp(coldStorage, 0f, 1f);
            float agePenalty = Math.Max(0f, job.material_age_days - material.freshness_days * 0.5f) * material.aging_penalty_per_day;
            float qualityScore = Math.Clamp(conformity * (0.85f + Math.Clamp(job.operator_skill + operatorSkill, 0f, 1f) * 0.15f) - agePenalty, 0f, 1f);
            if (job.defect_roll < cure.defect_rate * (1.2f - qualityScore)) qualityScore *= 0.45f;
            CompositeQualityGrade quality = qualityScore < 0.35f ? CompositeQualityGrade.Reject
                : qualityScore < 0.58f ? CompositeQualityGrade.Field
                : qualityScore < 0.82f ? CompositeQualityGrade.Structural
                : CompositeQualityGrade.Certified;
            var output = new CompositeOutputState
            {
                job_id = job.job_id,
                component_id = component.component_id,
                output_item_id = component.output_item_id,
                quality = quality,
                mass_factor = component.mass_factor,
                durability_factor = component.durability_factor * Math.Max(0.5f, qualityScore),
                certified = quality >= CompositeQualityGrade.Structural
            };
            _state.output_buffer.Add(output);
            _state.active_job = null;
            return new CompositeTickResult { Progressed = true, StatusCode = "composite.complete", CompletedOutput = output };
        }

        public ActionResult ClaimOutput()
        {
            if (_state.output_buffer.Count == 0) return ActionResult.Blocked(CarbonCompositeFailureCodes.OutputUnavailable, "composite.no_output");
            if (_inventory == null) return ActionResult.Blocked(CarbonCompositeFailureCodes.OutputUnavailable, "composite.inventory_unbound");
            var bill = new InventoryBill();
            foreach (var output in _state.output_buffer)
                if (output.quality != CompositeQualityGrade.Reject) bill.AddGrant(output.output_item_id, 1);
            if (!bill.IsEmpty && !_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked(CarbonCompositeFailureCodes.OutputUnavailable, "composite.output_unavailable");
            int count = _state.output_buffer.Count;
            _state.output_buffer.Clear();
            return ActionResult.Success("composite.output_claimed", new Dictionary<string, double> { ["outputs"] = count });
        }

        public CompositeComponentProjection? ProjectComponent(string componentId)
        {
            for (int i = 0; i < _state.output_buffer.Count; i++)
            {
                var output = _state.output_buffer[i];
                if (output.component_id == componentId && output.quality != CompositeQualityGrade.Reject)
                    return new CompositeComponentProjection { ComponentId = componentId, Quality = output.quality, MassFactor = output.mass_factor, DurabilityFactor = output.durability_factor, Certified = output.certified };
            }
            return null;
        }

        public CarbonCompositeState CaptureState()
        {
            var copy = new CarbonCompositeState
            {
                schema_version = _state.schema_version,
                rng_state = _rng is SeededRng seeded ? seeded.PeekState() : 0UL,
                autoclave_condition = _state.autoclave_condition,
                next_job_number = _state.next_job_number,
                output_buffer = new List<CompositeOutputState>()
            };
            if (_state.active_job != null) copy.active_job = new CompositeJobState
            {
                job_id = _state.active_job.job_id, component_id = _state.active_job.component_id,
                material_profile_id = _state.active_job.material_profile_id, material_age_days = _state.active_job.material_age_days,
                progress_ticks = _state.active_job.progress_ticks, operator_skill = _state.active_job.operator_skill, defect_roll = _state.active_job.defect_roll
            };
            foreach (var output in _state.output_buffer) copy.output_buffer.Add(new CompositeOutputState
            {
                job_id = output.job_id, component_id = output.component_id, output_item_id = output.output_item_id,
                quality = output.quality, mass_factor = output.mass_factor, durability_factor = output.durability_factor, certified = output.certified
            });
            return copy;
        }

        public void RestoreState(CarbonCompositeState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state = state;
            _state.output_buffer ??= new List<CompositeOutputState>();
            _state.autoclave_condition = Math.Clamp(_state.autoclave_condition, 0f, 1f);
            if (_rng is SeededRng seeded && _state.rng_state != 0UL)
                seeded.SeekState(_state.rng_state);
        }
    }
}
