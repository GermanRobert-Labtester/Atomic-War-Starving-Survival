// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Medical
{
    public enum SurgicalMilestone
    {
        Incision,
        Midpoint,
        Closure
    }

    [Serializable]
    public sealed class SurgicalOperationState
    {
        public string operation_id { get; set; } = string.Empty;
        public string patient_id { get; set; } = string.Empty;
        public string surgeon_id { get; set; } = string.Empty;
        public string procedure_id { get; set; } = string.Empty;
        public int progress_hours { get; set; }
        public int total_duration_hours { get; set; } = 4;
        public float shock_percent { get; set; } // 0 - 100
        public float anesthesia_level { get; set; } = 100f; // 0 - 100
        public bool milestone_incision_resolved { get; set; }
        public bool milestone_midpoint_resolved { get; set; }
        public bool milestone_closure_resolved { get; set; }
        public bool is_completed { get; set; }
        public bool patient_survived { get; set; } = true;
        public int recovery_days_remaining { get; set; }
        public List<string> complications { get; set; } = new List<string>();
        public float rad_mSv_purged { get; set; }
        public string installed_prosthetic_item_id { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class AdvancedSurgicalWardSave
    {
        public string systemId { get; set; } = "surgical_ward";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; } = 1;
        public float sterile_field_percent { get; set; } = 100f;
        public int operating_table_count { get; set; } = 1;
        public List<SurgicalOperationState> active_operations { get; set; } = new List<SurgicalOperationState>();
        public List<SurgicalOperationState> recovery_patients { get; set; } = new List<SurgicalOperationState>();
        public List<SurgicalOperationState> historical_operations { get; set; } = new List<SurgicalOperationState>();
    }

    public sealed class AdvancedSurgicalWardSystem
    {
        public const string SystemId = "surgical_ward";
        public const float RadPerMsvRatio = 0.1f; // 150 mSv = 15 rads

        private readonly List<SurgicalProcedureDefinition> _procedures = new List<SurgicalProcedureDefinition>();
        private readonly Dictionary<string, SurgicalProcedureDefinition> _proceduresById = new Dictionary<string, SurgicalProcedureDefinition>(StringComparer.Ordinal);
        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private AdvancedSurgicalWardSave _state = new AdvancedSurgicalWardSave();

        public event Action<SurgicalOperationState>? OnOperationStarted;
        public event Action<SurgicalOperationState, SurgicalMilestone, string>? OnComplicationEncountered;
        public event Action<SurgicalOperationState, bool>? OnOperationCompleted; // op, survived
        public event Action<SurgicalOperationState>? OnPatientDischarged;
        public event Action<float>? OnSterileFieldChanged;

        public IReadOnlyList<SurgicalProcedureDefinition> Procedures => _procedures;
        public IReadOnlyList<SurgicalOperationState> ActiveOperations => _state.active_operations;
        public IReadOnlyList<SurgicalOperationState> RecoveryPatients => _state.recovery_patients;
        public float SterileFieldPercent => _state.sterile_field_percent;

        public AdvancedSurgicalWardSystem(
            IEnumerable<SurgicalProcedureDefinition> procedures,
            Inventory.Inventory inventory,
            ISeededRng rng,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;

            if (procedures != null)
            {
                foreach (var p in procedures)
                {
                    if (p == null || string.IsNullOrEmpty(p.procedure_id)) continue;
                    _procedures.Add(p);
                    _proceduresById[p.procedure_id] = p;
                }
            }
        }

        public SurgicalProcedureDefinition? FindProcedure(string procedureId) =>
            _proceduresById.TryGetValue(procedureId, out var def) ? def : null;

        public ActionResult ValidatePreOp(
            string patientId,
            string surgeonId,
            string procedureId,
            bool surgeonHasTriageSkill = true)
        {
            if (string.IsNullOrEmpty(patientId))
                return ActionResult.Failed("invalid_patient", "surg.invalid_patient");
            if (string.IsNullOrEmpty(surgeonId))
                return ActionResult.Failed("invalid_surgeon", "surg.invalid_surgeon");
            if (!_proceduresById.TryGetValue(procedureId, out var def))
                return ActionResult.Failed("unknown_procedure", "surg.unknown_procedure");

            if (!surgeonHasTriageSkill)
                return ActionResult.Blocked("surgeon_lacks_skill", "surg.surgeon_lacks_skill");

            if (_state.active_operations.Count >= _state.operating_table_count)
                return ActionResult.Blocked("operating_table_occupied", "surg.table_occupied");

            // Check tools
            foreach (var tool in def.required_tools)
            {
                if (!_inventory.HasSufficient(tool, 1))
                    return ActionResult.Blocked($"missing_tool_{tool}", "surg.missing_tool");
            }

            // Check consumables
            foreach (var cost in def.consumable_costs)
            {
                if (!_inventory.HasSufficient(cost.Key, cost.Value))
                    return ActionResult.Blocked($"missing_consumable_{cost.Key}", "surg.missing_consumable");
            }

            return ActionResult.Success("surg.validation_ok");
        }

        public ActionResult StartOperation(
            string patientId,
            string surgeonId,
            string procedureId,
            bool surgeonHasTriageSkill = true)
        {
            var val = ValidatePreOp(patientId, surgeonId, procedureId, surgeonHasTriageSkill);
            if (!val.IsSuccess) return val;

            var def = _proceduresById[procedureId];

            // Consume required surgical consumables atomically
            foreach (var cost in def.consumable_costs)
            {
                if (!_inventory.TryConsume(cost.Key, cost.Value))
                    throw new InvalidOperationException($"Atomic consumption failed for {cost.Key}");
            }

            string opId = $"op_{procedureId}_{patientId}_{_state.historical_operations.Count + _state.active_operations.Count + 1}";
            var op = new SurgicalOperationState
            {
                operation_id = opId,
                patient_id = patientId,
                surgeon_id = surgeonId,
                procedure_id = procedureId,
                progress_hours = 0,
                total_duration_hours = Math.Max(1, def.base_duration_hours),
                shock_percent = 0f,
                anesthesia_level = 100f,
                recovery_days_remaining = def.recovery_days
            };

            // Detect prosthetic attachment
            if (def.consumable_costs.ContainsKey("prosthetic_wooden_arm"))
                op.installed_prosthetic_item_id = "prosthetic_wooden_arm";
            else if (def.consumable_costs.ContainsKey("prosthetic_wooden_leg"))
                op.installed_prosthetic_item_id = "prosthetic_wooden_leg";

            _state.active_operations.Add(op);
            OnOperationStarted?.Invoke(op);
            _log.Info($"[SurgicalWard] Started {def.display_name} for patient {patientId}.");
            return ActionResult.Success("surg.operation_started");
        }

        public void TickOperationHour(SurgicalOperationState op)
        {
            if (op.is_completed) return;
            if (!_proceduresById.TryGetValue(op.procedure_id, out var def)) return;

            op.progress_hours++;

            // Deplete anesthesia (-18% to -25% per hour)
            float anesthesiaDepletion = 100f / op.total_duration_hours;
            op.anesthesia_level = Math.Max(0f, op.anesthesia_level - anesthesiaDepletion);

            // Accumulate shock: base + acute shock if anesthesia ran dry
            float hourlyShock = (def.base_shock_risk * 25f);
            if (op.anesthesia_level <= 0f)
            {
                hourlyShock += 25f; // extreme pain shock
            }

            // Unsterile environment adds shock/infection stress
            if (_state.sterile_field_percent < 50f)
            {
                hourlyShock += (50f - _state.sterile_field_percent) * 0.2f;
            }

            op.shock_percent = Math.Clamp(op.shock_percent + hourlyShock, 0f, 100f);

            // Milestones
            float progressFraction = (float)op.progress_hours / op.total_duration_hours;

            if (progressFraction >= 0.25f && !op.milestone_incision_resolved)
            {
                op.milestone_incision_resolved = true;
                CheckMilestoneComplication(op, def, SurgicalMilestone.Incision);
            }

            if (progressFraction >= 0.50f && !op.milestone_midpoint_resolved)
            {
                op.milestone_midpoint_resolved = true;
                CheckMilestoneComplication(op, def, SurgicalMilestone.Midpoint);
            }

            if (op.shock_percent >= 100f)
            {
                // Fatal surgical shock
                op.is_completed = true;
                op.patient_survived = false;
                _state.active_operations.Remove(op);
                _state.historical_operations.Add(op);
                OnOperationCompleted?.Invoke(op, false);
                _log.Warn($"[SurgicalWard] Patient {op.patient_id} died from acute surgical shock during {def.procedure_id}!");
                return;
            }

            if (op.progress_hours >= op.total_duration_hours)
            {
                op.milestone_closure_resolved = true;
                CheckMilestoneComplication(op, def, SurgicalMilestone.Closure);

                op.is_completed = true;
                op.patient_survived = true;

                // Cellular radiation purge
                if (op.procedure_id == "surg_cellular_rad_scrub")
                {
                    op.rad_mSv_purged = 150f; // 150 mSv = 15 rads
                    _log.Info($"[SurgicalWard] Cellular radiation purge completed: 150 mSv (15 rads) scrubbed.");
                }

                // Degrade sterile field
                _state.sterile_field_percent = Math.Max(10f, _state.sterile_field_percent - 20f);
                OnSterileFieldChanged?.Invoke(_state.sterile_field_percent);

                _state.active_operations.Remove(op);
                _state.recovery_patients.Add(op);
                _state.historical_operations.Add(op);
                OnOperationCompleted?.Invoke(op, true);
                _log.Info($"[SurgicalWard] {def.display_name} successfully completed. Patient {op.patient_id} transferred to recovery.");
            }
        }

        private void CheckMilestoneComplication(
            SurgicalOperationState op,
            SurgicalProcedureDefinition def,
            SurgicalMilestone milestone)
        {
            double roll = _rng.NextDouble();
            float risk = def.base_complication_risk;
            if (_state.sterile_field_percent < 80f)
                risk += 0.15f;

            if (roll < risk)
            {
                string complication = milestone switch
                {
                    SurgicalMilestone.Incision => "arterial_nicked_hemorrhage",
                    SurgicalMilestone.Midpoint => "hypotensive_bradycardia",
                    SurgicalMilestone.Closure => "post_op_wound_dehiscence",
                    _ => "anesthesia_reversal_lag"
                };
                op.complications.Add(complication);
                op.shock_percent = Math.Clamp(op.shock_percent + 20f, 0f, 100f);
                OnComplicationEncountered?.Invoke(op, milestone, complication);
                _log.Warn($"[SurgicalWard] Complication during {milestone}: {complication} (Shock: {op.shock_percent:F1}%)");
            }
        }

        public ActionResult RunAutoclaveCycle(bool hasPower = true)
        {
            if (!hasPower)
                return ActionResult.Blocked("no_power", "surg.autoclave_unpowered");

            if (!_inventory.HasSufficient("clean_water", 1))
                return ActionResult.Blocked("no_water", "surg.autoclave_no_water");

            if (!_inventory.TryConsume("clean_water", 1))
                return ActionResult.Failed("water_consume_failed", "surg.water_consume_failed");

            _state.sterile_field_percent = 100f;
            OnSterileFieldChanged?.Invoke(100f);
            _log.Info("[SurgicalWard] Autoclave cycle complete. Operating theater sterile field restored to 100%.");
            return ActionResult.Success("surg.autoclave_sterilized");
        }

        public void TickDay(int day)
        {
            _state.last_tick_day = day;

            // Advance recovery patients
            for (int i = _state.recovery_patients.Count - 1; i >= 0; i--)
            {
                var patient = _state.recovery_patients[i];
                patient.recovery_days_remaining--;
                if (patient.recovery_days_remaining <= 0)
                {
                    _state.recovery_patients.RemoveAt(i);
                    OnPatientDischarged?.Invoke(patient);
                    _log.Info($"[SurgicalWard] Patient {patient.patient_id} fully recovered and discharged from surgical ward.");
                }
            }
        }

        public AdvancedSurgicalWardSave CaptureState()
        {
            var save = new AdvancedSurgicalWardSave
            {
                systemId = SystemId,
                schema_version = 1,
                last_tick_day = _state.last_tick_day,
                sterile_field_percent = _state.sterile_field_percent,
                operating_table_count = _state.operating_table_count
            };

            foreach (var op in _state.active_operations) save.active_operations.Add(CloneOp(op));
            foreach (var op in _state.recovery_patients) save.recovery_patients.Add(CloneOp(op));
            foreach (var op in _state.historical_operations) save.historical_operations.Add(CloneOp(op));

            return save;
        }

        public void RestoreState(AdvancedSurgicalWardSave? save)
        {
            if (save == null) return;
            _state.last_tick_day = save.last_tick_day;
            _state.sterile_field_percent = save.sterile_field_percent;
            _state.operating_table_count = save.operating_table_count;

            _state.active_operations.Clear();
            _state.recovery_patients.Clear();
            _state.historical_operations.Clear();

            if (save.active_operations != null)
                foreach (var op in save.active_operations) _state.active_operations.Add(CloneOp(op));
            if (save.recovery_patients != null)
                foreach (var op in save.recovery_patients) _state.recovery_patients.Add(CloneOp(op));
            if (save.historical_operations != null)
                foreach (var op in save.historical_operations) _state.historical_operations.Add(CloneOp(op));
        }

        private static SurgicalOperationState CloneOp(SurgicalOperationState src)
        {
            return new SurgicalOperationState
            {
                operation_id = src.operation_id,
                patient_id = src.patient_id,
                surgeon_id = src.surgeon_id,
                procedure_id = src.procedure_id,
                progress_hours = src.progress_hours,
                total_duration_hours = src.total_duration_hours,
                shock_percent = src.shock_percent,
                anesthesia_level = src.anesthesia_level,
                milestone_incision_resolved = src.milestone_incision_resolved,
                milestone_midpoint_resolved = src.milestone_midpoint_resolved,
                milestone_closure_resolved = src.milestone_closure_resolved,
                is_completed = src.is_completed,
                patient_survived = src.patient_survived,
                recovery_days_remaining = src.recovery_days_remaining,
                rad_mSv_purged = src.rad_mSv_purged,
                installed_prosthetic_item_id = src.installed_prosthetic_item_id,
                complications = new List<string>(src.complications ?? new())
            };
        }
    }
}
