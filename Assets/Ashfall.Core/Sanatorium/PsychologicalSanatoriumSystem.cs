using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Sanatorium;

namespace Ashfall.Core.Sanatorium
{
    // ---------------------------------------------------------------------
    // Ports
    // ---------------------------------------------------------------------

    /// <summary>
    /// Canonical survivor-condition authority port. The sanatorium keeps only
    /// treatment-progress state; active condition effects, acute stress and
    /// trait suppression live in the canonical survivor/needs/trauma systems
    /// (plan §9.9 / Risk 13).
    /// </summary>
    public interface ISurvivorConditionPort
    {
        /// <summary>True when the canonical survivor state carries the condition.</summary>
        bool HasCondition(string survivorId, string conditionId);

        /// <summary>Canonical acute stress intensity, 0..1000 permille.</summary>
        int GetAcuteStressPermille(string survivorId);

        /// <summary>Applies an authored acute-stress reduction through the canonical API.</summary>
        void ApplyAcuteStressReduction(string survivorId, int permille);

        /// <summary>Applies authored recovery progress through the canonical API.</summary>
        void ApplyRecoveryProgress(string survivorId, int progress);

        /// <summary>
        /// Suppresses a reversible condition through the canonical API. Called
        /// ONLY for authored reversible conditions at full recovery.
        /// </summary>
        void SuppressReversibleCondition(string survivorId, string conditionId);

        /// <summary>Canonical therapist→patient relationship trust, 0..100.</summary>
        int GetRelationshipTrust(string therapistId, string patientId);
    }

    // ---------------------------------------------------------------------
    // Persisted state
    // ---------------------------------------------------------------------

    [Serializable]
    public sealed class SanatoriumPatientState
    {
        public string survivor_id = string.Empty;
        public List<string> condition_ids = new();
        public int admission_acute_permille;
        public int treatment_progress;                     // 0..1000
        public string active_therapy_id = string.Empty;
        public int therapy_days_elapsed;
        public string assigned_therapist_id = string.Empty;
        public int session_count;
        public int completed_therapy_count;
        public int admission_day = -1;
        public int last_treatment_day = -1;
        public int relapse_risk_permille = 200;            // authored starting risk
        public int sedated_until_day = -1;
        public string status = "admitted";                 // admitted | discharged
        public int discharge_day = -1;
    }

    [Serializable]
    public sealed class PsychologicalSanatoriumSave
    {
        public int schema_version = 1;
        public List<SanatoriumPatientState> patients = new();
        public int next_relapse_ordinal;
    }

    // ---------------------------------------------------------------------
    // System
    // ---------------------------------------------------------------------

    /// <summary>
    /// Survivor psychological conditioning & trauma sanatorium (flagship
    /// Task 8). A gameplay rehabilitation institution: acute reduction,
    /// recovery progress, remission and relapse — never a one-click cure.
    /// Canonical condition/stress state is mutated ONLY through
    /// <see cref="ISurvivorConditionPort"/>; sedatives stay global-inventory
    /// authority; relapse uses a keyed RNG stream (no persisted continuation).
    /// </summary>
    public sealed class PsychologicalSanatoriumSystem
    {
        public const string SystemId = "psychological_sanatorium";
        public const string InstitutionId = "institution_sanatorium";

        public const int DefaultBedCapacity = 2;
        public const int RelapseAcuteIncreasePermille = 150;
        public const int SedativeAcuteReductionPermille = 150;
        public const int SedativeDurationDays = 1;
        public const string SedativeItemId = "sedative_draught";
        public const int RelapseCheckBaseRiskPermille = 200;

        private readonly Inventory.Inventory? _inventory;
        private readonly ILog _log;
        private readonly int _masterSeed;
        private readonly IInstitutionAvailability? _availability;
        private readonly ISurvivorSkillsPort? _skills;
        private readonly ISurvivorConditionPort? _conditions;

        private readonly Dictionary<string, PsychologicalTherapyDefinition> _therapies = new(StringComparer.Ordinal);
        private readonly Dictionary<string, TherapyConditionDefinition> _conditions_catalog = new(StringComparer.Ordinal);
        private PsychologicalSanatoriumSave _state = new();

        public PsychologicalSanatoriumSystem(
            int masterSeed,
            Inventory.Inventory? inventory = null,
            ILog? log = null,
            IInstitutionAvailability? availability = null,
            ISurvivorSkillsPort? skills = null,
            ISurvivorConditionPort? conditions = null)
        {
            _masterSeed = masterSeed;
            _inventory = inventory;
            _log = log ?? new ConsoleLog();
            _availability = availability;
            _skills = skills;
            _conditions = conditions;
        }

        // -----------------------------------------------------------------
        // Events
        // -----------------------------------------------------------------

        public event Action<SanatoriumPatientState>? OnPatientAdmitted;
        public event Action<SanatoriumPatientState, string>? OnTherapyStarted;     // patient, therapy
        public event Action<SanatoriumPatientState, string>? OnTherapyCompleted;   // patient, therapy
        public event Action<string, string, int>? OnPatientRelapsed;               // survivor, condition, day
        public event Action<SanatoriumPatientState>? OnPatientDischarged;
        public event Action<string, string>? OnTherapeuticJournalCompleted;        // survivor, therapy

        // -----------------------------------------------------------------
        // Catalog + queries
        // -----------------------------------------------------------------

        public void LoadTherapyCatalog(PsychologicalTherapyCatalogContainer container)
        {
            if (container == null) return;
            _therapies.Clear();
            _conditions_catalog.Clear();
            foreach (var t in container.therapies)
                if (!string.IsNullOrEmpty(t.therapy_id))
                    _therapies[t.therapy_id] = t;
            foreach (var c in container.conditions)
                if (!string.IsNullOrEmpty(c.condition_id))
                    _conditions_catalog[c.condition_id] = c;
        }

        public IReadOnlyList<SanatoriumPatientState> Patients => _state.patients.AsReadOnly();
        public SanatoriumPatientState? GetPatient(string survivorId) =>
            _state.patients.FirstOrDefault(p => p.survivor_id == survivorId);
        public int OccupiedBeds => _state.patients.Count(p => p.status == "admitted");
        public bool HasBed => OccupiedBeds < DefaultBedCapacity;
        public PsychologicalTherapyDefinition? GetTherapy(string therapyId) => _therapies.GetValueOrDefault(therapyId);

        // -----------------------------------------------------------------
        // Admission
        // -----------------------------------------------------------------

        public ActionResult TryAdmitPatient(string survivorId, string conditionId, int day)
        {
            if (string.IsNullOrEmpty(survivorId))
                return ActionResult.Blocked("invalid_survivor", "sanatorium.invalid_survivor");
            if (!_conditions_catalog.ContainsKey(conditionId))
                return ActionResult.Blocked("unknown_condition", "sanatorium.unknown_condition");
            if (_conditions == null || !_conditions.HasCondition(survivorId, conditionId))
                return ActionResult.Blocked("not_eligible", "sanatorium.not_eligible");
            if (GetPatient(survivorId) is { } existing && existing.status == "admitted")
                return ActionResult.Blocked("already_admitted", "sanatorium.already_admitted");
            if (!HasBed)
                return ActionResult.Blocked("no_beds", "sanatorium.no_beds");
            if (_availability != null && !_availability.TryClaim(survivorId, InstitutionId, "patient"))
                return ActionResult.Blocked("survivor_unavailable", "sanatorium.survivor_unavailable");

            // Re-admission after a past discharge starts a fresh episode but
            // keeps the identity (the patient list keeps one row per survivor).
            var patient = GetPatient(survivorId);
            if (patient == null)
            {
                patient = new SanatoriumPatientState { survivor_id = survivorId };
                _state.patients.Add(patient);
            }
            patient.status = "admitted";
            patient.discharge_day = -1;
            if (!patient.condition_ids.Contains(conditionId))
                patient.condition_ids.Add(conditionId);
            patient.admission_acute_permille = _conditions?.GetAcuteStressPermille(survivorId) ?? 0;
            patient.admission_day = day;
            patient.last_treatment_day = day;
            patient.treatment_progress = 0;
            patient.active_therapy_id = string.Empty;
            patient.assigned_therapist_id = string.Empty;

            _log.Info($"[Sanatorium] admitted '{survivorId}' ({conditionId}) on day {day}");
            OnPatientAdmitted?.Invoke(patient);
            return ActionResult.Success("sanatorium.admitted");
        }

        // -----------------------------------------------------------------
        // Therapy
        // -----------------------------------------------------------------

        public ActionResult TryStartTherapy(string survivorId, string therapyId, string therapistId, int day)
        {
            var patient = GetPatient(survivorId);
            if (patient == null || patient.status != "admitted")
                return ActionResult.Blocked("not_admitted", "sanatorium.not_admitted");
            if (!string.IsNullOrEmpty(patient.active_therapy_id))
                return ActionResult.Blocked("therapy_in_progress", "sanatorium.therapy_in_progress");
            if (!_therapies.TryGetValue(therapyId, out var therapy))
                return ActionResult.Blocked("unknown_therapy", "sanatorium.unknown_therapy");
            if (!patient.condition_ids.Any(therapy.EligibleForCondition))
                return ActionResult.Blocked("therapy_not_eligible", "sanatorium.therapy_not_eligible");

            int skill = 0;
            if (_skills != null)
            {
                if (!_skills.HasSkill(therapistId, therapy.staff_skill_id))
                    return ActionResult.Blocked("therapist_unqualified", "sanatorium.therapist_unqualified");
                if (_skills.HasSkill(therapistId, "skill_cold_analysis")) skill += 10;
                if (_skills.HasSkill(therapistId, "skill_watchful")) skill += 5;
            }

            // Relationship trust can assist where authored (plan §8.18).
            int trust = _conditions?.GetRelationshipTrust(therapistId, survivorId) ?? 50;
            if (trust < 20)
                return ActionResult.Blocked("no_trust", "sanatorium.no_trust");

            // Atomic resource consumption at therapy start.
            if (_inventory != null && therapy.resource_costs is { Count: > 0 })
            {
                var bill = new InventoryBill();
                foreach (var c in therapy.resource_costs)
                    bill.AddCost(c.item_id, c.amount);
                if (!_inventory.TryExecuteTransaction(bill))
                    return ActionResult.Blocked("missing_inputs", "sanatorium.missing_therapy_inputs");
            }

            if (_availability != null && !_availability.TryClaim(therapistId, InstitutionId, "therapist"))
                return ActionResult.Blocked("therapist_unavailable", "sanatorium.therapist_unavailable");

            patient.active_therapy_id = therapyId;
            patient.assigned_therapist_id = therapistId;
            patient.therapy_days_elapsed = 0;
            patient.last_treatment_day = day;
            patient.session_count++;
            TherapistAssistBonus = skill;
            _log.Info($"[Sanatorium] therapy '{therapyId}' started for '{survivorId}' by '{therapistId}'");
            OnTherapyStarted?.Invoke(patient, therapyId);
            return ActionResult.Success("sanatorium.therapy_started");
        }

        /// <summary>One-shot assist bonus from therapist skill/trust for the active therapy (authored).</summary>
        private int TherapistAssistBonus;

        /// <summary>
        /// THE canonical treatment-outcome applier (plan §8.13). Every therapy
        /// effect funnels through here — no per-therapy survivor mutation.
        /// </summary>
        private void ApplyTherapyOutcome(SanatoriumPatientState patient, PsychologicalTherapyDefinition therapy)
        {
            int reduction = therapy.acute_stress_reduction_permille + TherapistAssistBonus;
            _conditions?.ApplyAcuteStressReduction(patient.survivor_id, reduction);
            _conditions?.ApplyRecoveryProgress(patient.survivor_id, therapy.recovery_progress);

            patient.treatment_progress = Math.Min(1000, patient.treatment_progress + therapy.recovery_progress * 5);
            patient.completed_therapy_count++;
            patient.relapse_risk_permille = Math.Max(0,
                patient.relapse_risk_permille - (int)(therapy.relapse_modifier * 1000));

            // Reversible-condition suppression is explicit and gated: only
            // authored reversible conditions, only at full recovery.
            if (patient.treatment_progress >= 1000)
            {
                foreach (var condId in patient.condition_ids)
                {
                    if (_conditions_catalog.TryGetValue(condId, out var cond) && cond.reversible)
                        _conditions?.SuppressReversibleCondition(patient.survivor_id, condId);
                }
            }

            if (therapy.grants_journal_entry)
                OnTherapeuticJournalCompleted?.Invoke(patient.survivor_id, therapy.therapy_id);

            TherapistAssistBonus = 0;
        }

        // -----------------------------------------------------------------
        // Sedative stabilization
        // -----------------------------------------------------------------

        public ActionResult TryAdministerSedative(string survivorId, int day)
        {
            var patient = GetPatient(survivorId);
            if (patient == null || patient.status != "admitted")
                return ActionResult.Blocked("not_admitted", "sanatorium.not_admitted");
            if (day <= patient.sedated_until_day)
                return ActionResult.Blocked("already_sedated", "sanatorium.already_sedated");
            if (_inventory == null)
                return ActionResult.Blocked("no_inventory", "sanatorium.no_inventory");

            var bill = new InventoryBill();
            bill.AddCost(SedativeItemId, 1);
            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("missing_sedative", "sanatorium.missing_sedative");

            patient.sedated_until_day = day + SedativeDurationDays;
            patient.last_treatment_day = day;
            _conditions?.ApplyAcuteStressReduction(survivorId, SedativeAcuteReductionPermille);
            _log.Info($"[Sanatorium] sedative administered to '{survivorId}' (until day {patient.sedated_until_day})");
            return ActionResult.Success("sanatorium.sedative_administered",
                new Dictionary<string, double> { { "sedated_until_day", patient.sedated_until_day } });
        }

        // -----------------------------------------------------------------
        // Discharge
        // -----------------------------------------------------------------

        public ActionResult TryDischargePatient(string survivorId, int day)
        {
            var patient = GetPatient(survivorId);
            if (patient == null || patient.status != "admitted")
                return ActionResult.Blocked("not_admitted", "sanatorium.not_admitted");

            return Discharge(patient, day);
        }

        private ActionResult Discharge(SanatoriumPatientState patient, int day)
        {
            patient.status = "discharged";
            patient.discharge_day = day;
            patient.active_therapy_id = string.Empty;
            if (!string.IsNullOrEmpty(patient.assigned_therapist_id))
            {
                _availability?.Release(patient.assigned_therapist_id, InstitutionId, "therapist");
                patient.assigned_therapist_id = string.Empty;
            }
            _availability?.Release(patient.survivor_id, InstitutionId, "patient");
            _log.Info($"[Sanatorium] discharged '{patient.survivor_id}' on day {day}");
            OnPatientDischarged?.Invoke(patient);
            return ActionResult.Success("sanatorium.discharged");
        }

        // -----------------------------------------------------------------
        // Daily tick
        // -----------------------------------------------------------------

        private int _currentDay;

        public void TickDay(int day)
        {
            _currentDay = day;

            // Stable ordering by survivor id (plan §8.15).
            foreach (var patient in _state.patients.OrderBy(p => p.survivor_id, StringComparer.Ordinal).ToList())
            {
                if (patient.status != "admitted") continue;

                // Active therapy progression.
                if (!string.IsNullOrEmpty(patient.active_therapy_id)
                    && _therapies.TryGetValue(patient.active_therapy_id, out var therapy))
                {
                    patient.therapy_days_elapsed++;
                    patient.last_treatment_day = day;

                    if (patient.therapy_days_elapsed >= therapy.duration_days)
                    {
                        ApplyTherapyOutcome(patient, therapy);
                        if (!string.IsNullOrEmpty(patient.assigned_therapist_id))
                        {
                            _availability?.Release(patient.assigned_therapist_id, InstitutionId, "therapist");
                            patient.assigned_therapist_id = string.Empty;
                        }
                        patient.active_therapy_id = string.Empty;
                        patient.therapy_days_elapsed = 0;
                        _log.Info($"[Sanatorium] therapy '{therapy.therapy_id}' completed for '{patient.survivor_id}'");
                        OnTherapyCompleted?.Invoke(patient, therapy.therapy_id);
                    }
                }
                else if (day > patient.sedated_until_day)
                {
                    // Relapse check — only for untreated patients with risk > 0.
                    if (patient.relapse_risk_permille > 0 && patient.condition_ids.Count > 0)
                    {
                        var rng = StreamFor(patient.survivor_id, day);
                        int roll = rng.Next(0, 1000);
                        if (roll < patient.relapse_risk_permille)
                        {
                            string condition = patient.condition_ids[0];
                            _conditions?.ApplyAcuteStressReduction(patient.survivor_id, -RelapseAcuteIncreasePermille);
                            patient.relapse_risk_permille = Math.Min(1000,
                                patient.relapse_risk_permille + RelapseCheckBaseRiskPermille / 2);
                            _state.next_relapse_ordinal++;
                            _log.Info($"[Sanatorium] relapse: '{patient.survivor_id}' ({condition}) on day {day}");
                            OnPatientRelapsed?.Invoke(patient.survivor_id, condition, day);
                        }
                    }
                }

                // Automatic discharge at full recovery.
                if (patient.status == "admitted" && patient.treatment_progress >= 1000)
                    Discharge(patient, day);
            }
        }

        // -----------------------------------------------------------------
        // Keyed RNG streams
        // -----------------------------------------------------------------

        private SeededRng StreamFor(string survivorId, int day)
        {
            ulong h = 1469598103934665603UL;
            foreach (char c in survivorId)
            {
                h ^= c;
                h *= 1099511628211UL;
            }
            h ^= (uint)day;
            h *= 1099511628211UL;
            h ^= (uint)_masterSeed;
            h *= 1099511628211UL;
            return new SeededRng(unchecked((int)(h ^ (h >> 32))));
        }

        // -----------------------------------------------------------------
        // Save / restore
        // -----------------------------------------------------------------

        public PsychologicalSanatoriumSave CaptureState() => Clone(_state);

        public void RestoreState(PsychologicalSanatoriumSave? saved)
        {
            if (saved == null) return;
            _state = Clone(saved);
        }

        private static PsychologicalSanatoriumSave Clone(PsychologicalSanatoriumSave src)
        {
            var json = new SystemTextJsonSerializer();
            return json.Deserialize<PsychologicalSanatoriumSave>(json.Serialize(src)) ?? new PsychologicalSanatoriumSave();
        }
    }
}
