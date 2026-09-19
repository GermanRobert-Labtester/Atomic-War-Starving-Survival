// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.DutyRoster;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private const string SleepScheduleModifierSource = "shelter_schedule.sleep";
        private const string OverworkFatigueModifierSource = "overwork.fatigue";
        private const string OverworkMoraleModifierSource = "overwork.morale";
        private const string GriefBondModifierSource = "grief.bond_loss";
        private FitnessRoleCatalog? _fitnessRoleCatalog;
        private FitnessForDutyModel? _fitnessForDuty;
        private string _fitnessCatalogError = string.Empty;
        private DutyHourLedger? _dutyHourLedger;
        private WorkerProductivityContract? _workerProductivity;

        /// <summary>
        /// Plan 24A bootstrap. Duty fitness is derived from existing authorities
        /// and is intentionally not a save section or a second survivor ledger.
        /// </summary>
        private void SetupFitnessForDuty()
        {
            if (_fitnessForDuty != null) return;

            var result = FitnessRoleCatalogLoader.LoadDetailed(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            if (!result.IsSuccess || result.Catalog == null)
            {
                _fitnessCatalogError = string.Join("; ", result.Errors);
                GD.PrintErr("[Ashfall Godot] Duty fitness catalog unavailable: " + _fitnessCatalogError);
                return;
            }

            _fitnessRoleCatalog = result.Catalog;
            _fitnessForDuty = new FitnessForDutyModel(result.Catalog.Thresholds);
            GD.Print("[Ashfall Godot] Duty fitness ready. roles=" + result.Catalog.Roles.Count);
        }

        private RoleFitnessVerdict EvaluateDutyRoleFitness(string survivorId, string roleId)
        {
            SetupFitnessForDuty();
            if (_fitnessForDuty == null || _fitnessRoleCatalog == null
                || !_fitnessRoleCatalog.TryGetRole(roleId, out var requirements))
            {
                var missing = new FitnessVerdict(
                    survivorId,
                    FitnessLevel.Incapacitated,
                    new[] { "missing_duty_fitness_catalog" },
                    Array.Empty<string>(),
                    Array.Empty<NeedKind>());
                return new RoleFitnessVerdict(
                    survivorId, roleId, missing, false, false,
                    missing.BlockingReasons, Array.Empty<string>(), 0f);
            }

            return _fitnessForDuty.EvaluateForRole(BuildFitnessFacts(survivorId), requirements);
        }

        private FitnessVerdict EvaluateSurvivorFitness(string survivorId)
        {
            SetupFitnessForDuty();
            if (_fitnessForDuty == null)
            {
                return new FitnessVerdict(
                    survivorId,
                    FitnessLevel.Incapacitated,
                    new[] { "missing_duty_fitness_catalog" },
                    Array.Empty<string>(),
                    Array.Empty<NeedKind>());
            }
            return _fitnessForDuty.Evaluate(BuildFitnessFacts(survivorId));
        }

        /// <summary>
        /// Takes a read-only snapshot from the established survivor, radiation,
        /// disease, roster, and shared skill owners. No values are written here.
        /// </summary>
        private FitnessEvaluationFacts BuildFitnessFacts(string survivorId)
        {
            SetupDoseLedger();
            var needs = _survivors?.Needs.Get(survivorId);
            var radiation = _survivors?.RadStateFor(survivorId);
            var row = _dutyRoster?.Roster.GetRow(survivorId);
            // Plan 216: exercise is owned by SurvivorSocialCoordinator and
            // persisted in its existing section. Do not create profiles while
            // the duty roster is bootstrapping; an absent profile means the
            // neutral capability baseline until social state is ready.
            float conditioning = 100f;
            if (_survivorSocial != null)
            {
                var profile = _survivorSocial.Exercise.TryGetProfile(survivorId);
                conditioning = profile?.OverallConditioning ?? 100f;
            }
            int activeIllnessBand = -1;
            var illnessBand = _doseLedger?.SickList.GetBand(survivorId);
            if (illnessBand != null
                && illnessBand.releaseDay < 0
                && string.Equals(illnessBand.severitySource, SickListSystem.SourceIllness, StringComparison.Ordinal))
                activeIllnessBand = illnessBand.band;
            int daysSinceDischarge = -1;
            if (_medicalWard?.State?.Admissions != null)
            {
                int latestDischargeDay = -1;
                var admissions = _medicalWard.State.Admissions;
                for (int i = 0; i < admissions.Count; i++)
                {
                    var admission = admissions[i];
                    if (admission == null
                        || admission.Status != Ashfall.Core.Medical.MedicalAdmissionStatus.Discharged
                        || !string.Equals(admission.PatientId, survivorId, StringComparison.Ordinal)) continue;
                    if (admission.DischargedDay > latestDischargeDay)
                        latestDischargeDay = admission.DischargedDay;
                }
                if (latestDischargeDay >= 0 && latestDischargeDay <= _simDay)
                    daysSinceDischarge = _simDay - latestDischargeDay;
            }

            bool quarantined = false;
            bool infectious = false;
            if (_disease?.Engine != null && _disease.Catalog?.All != null)
            {
                var diseases = _disease.Catalog.All;
                for (int i = 0; i < diseases.Count; i++)
                {
                    var disease = diseases[i];
                    if (disease == null || string.IsNullOrEmpty(disease.id)) continue;
                    quarantined |= _disease.Engine.IsQuarantined(survivorId, disease.id);
                    infectious |= _disease.Engine.IsContagious(survivorId, disease.id);
                }
            }

            var facts = new FitnessEvaluationFacts
            {
                SurvivorId = survivorId ?? string.Empty,
                IsAlive = needs?.IsAliveState ?? false,
                IsDead = needs == null || needs.IsDead,
                IsQuarantined = quarantined,
                IsMedicallyIncapacitated = _medicalWard?.GetActiveAdmission(survivorId) != null,
                IsInfectious = infectious,
                HasSevereArs = radiation?.HasAcuteRadiationSyndrome ?? false,
                HasActiveWithdrawal = _chemicalDependency?.System.HasActiveWithdrawal(survivorId) ?? false,
                HasCombatTrauma = _phase0?.CombatTrauma != null
                    && _phase0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.6f,
                HasRespiratoryImpairment = (_phase0?.Respiratory?.RespiratoryDegradation(survivorId) ?? 0f) > 0f,
                Health = needs?.Health ?? 0f,
                Hunger = needs?.Hunger ?? 100f,
                Thirst = needs?.Thirst ?? 100f,
                Fatigue = needs?.Fatigue ?? 100f,
                Warmth = needs?.Warmth ?? 0f,
                Conditioning = conditioning,
                CumulativeDoseMsv = Math.Max(
                    radiation?.LifetimeRadiationExposure ?? 0f,
                    _doseLedger?.Ledger.GetCumulative(survivorId) ?? 0f),
                DaysSinceSleep = row != null && row.lastSleptDay >= 0
                    ? Math.Max(0, _simDay - row.lastSleptDay)
                    : -1,
                DaysSinceDischarge = daysSinceDischarge,
                ActiveIllnessBand = activeIllnessBand
            };

            var skillLevels = new Dictionary<string, float>(StringComparer.Ordinal);
            var skills = EnsureSharedSkillProgression();
            if (_fitnessRoleCatalog != null)
            {
                foreach (var pair in _fitnessRoleCatalog.Roles)
                {
                    var requirements = pair.Value;
                    if (string.IsNullOrEmpty(requirements.SkillId)) continue;
                    float level = skills.HasActiveSkill(survivorId, requirements.SkillId)
                        ? 1f
                        : 0f;
                    var def = skills.GetSkill(requirements.SkillId);
                    if (def != null)
                    {
                        level = Math.Min(1f, Math.Max(level,
                            skills.GetDisciplineProgress01(survivorId, def.disciplineId)
                            + skills.GetCachedBonus(survivorId, def.disciplineId)));
                    }
                    skillLevels[requirements.SkillId] = level;
                }
            }
            facts.SkillLevels = skillLevels;
            return facts;
        }

        /// <summary>Read-only host access for duty UI and diagnostics.</summary>
        public RoleFitnessVerdict? PreviewDutyFitness(string survivorId, string roleId)
        {
            if (_dutyRoster == null) SetupDutyRoster();
            return _dutyRoster?.Roster.PreviewRoleFitness(survivorId, roleId);
        }

        /// <summary>
        /// Plan 24B A2 — the duty-hour accumulator over the canonical roster
        /// assignment state, the data-authored role shift loads, and the live
        /// fitness verdicts. Derived only; never persisted.
        /// </summary>
        public DutyHourLedger EnsureDutyHourLedger()
        {
            if (_dutyHourLedger != null) return _dutyHourLedger;
            SetupDutyRoster();
            SetupFitnessForDuty();
            var roster = _dutyRoster!.Roster;
            _dutyHourLedger = new DutyHourLedger(
                getRoleOf: survivorId => roster.GetRoleOf(survivorId) ?? string.Empty,
                getRoleCommittedHours: roleId =>
                    _fitnessRoleCatalog != null
                    && _fitnessRoleCatalog.TryGetRole(roleId, out var requirements)
                    && requirements.MaximumHours > 0f
                        ? requirements.MaximumHours
                        : DutyHourLedger.DefaultRoleHours,
                previewRoleFitness: (survivorId, roleId) => PreviewDutyFitness(survivorId, roleId),
                getAssignedSurvivorIds: () =>
                {
                    var assignments = roster.State.assignments;
                    var ids = new List<string>();
                    for (int i = 0; i < assignments.Count; i++)
                    {
                        var a = assignments[i];
                        if (a == null || string.IsNullOrEmpty(a.survivorId)) continue;
                        ids.Add(a.survivorId);
                    }
                    return ids;
                });
            // Plan 24B A2 — surface the hours projection to the duty panel
            // through the roster's optional read seam (null-safe, legacy-neutral).
            roster.DutyHourResolver = survivorId => _dutyHourLedger!.For(survivorId);
            return _dutyHourLedger;
        }

        /// <summary>
        /// Plan 24B A2 — the ONE shared worker-productivity seam. Binds the
        /// campaign skill authority, the fitness projection, and the duty-hour
        /// ledger once; every producer (kitchen cook, workshop crafter, …)
        /// resolves through this single contract. No per-producer setters.
        /// </summary>
        public WorkerProductivityContract EnsureWorkerProductivityContract()
        {
            if (_workerProductivity != null) return _workerProductivity;
            SetupFitnessForDuty();
            var ledger = EnsureDutyHourLedger();
            var contract = new WorkerProductivityContract
            {
                SkillLevelResolver = (survivorId, skillId) =>
                {
                    var skills = EnsureSharedSkillProgression();
                    if (!skills.HasActiveSkill(survivorId, skillId))
                    {
                        var def = skills.GetSkill(skillId);
                        if (def == null) return null; // unknown skill — legacy path
                        float progress = skills.GetDisciplineProgress01(survivorId, def.disciplineId)
                            + skills.GetCachedBonus(survivorId, def.disciplineId);
                        if (progress <= 0f) return null; // no level at all — legacy path
                        return Math.Clamp(progress, 0f, 1f) * 100f;
                    }
                    var granted = skills.GetSkill(skillId);
                    float level = 100f;
                    if (granted != null)
                        level = Math.Max(100f,
                            Math.Clamp(skills.GetDisciplineProgress01(survivorId, granted.disciplineId)
                                + skills.GetCachedBonus(survivorId, granted.disciplineId), 0f, 1f) * 100f);
                    return level;
                },
                FitnessResolver = survivorId =>
                {
                    var verdict = EvaluateSurvivorFitness(survivorId);
                    return verdict.Level;
                },
                OverworkResolver = survivorId => ledger.IsOverworked(survivorId),
            };
            if (_fitnessRoleCatalog != null)
            {
                contract.OverworkRules = _fitnessRoleCatalog.Overwork;
                contract.YieldBand = _fitnessRoleCatalog.WorkerYield;
            }
            _workerProductivity = contract;
            return contract;
        }

        /// <summary>
        /// Plan 24C (A3) — the grief-to-needs projection: every mourning
        /// relationship's persisted facts (grief amount, onset day) derive a
        /// time-decaying fatigue/morale rate through the shared needs modifier
        /// seam (`grief.bond_loss`), expiring after the authored window.
        /// Fully derived — a reload recomputes identical rates from the
        /// relationship ledger; nothing about the effect is saved elsewhere.
        /// </summary>
        public void ApplyGriefNeedsModifiers()
        {
            if (_survivors?.Needs == null || _survivorRelationsCore == null) return;
            var needs = _survivors.Needs;
            var relations = _survivorRelationsCore.State?.relationships;
            if (relations == null) return;

            for (int i = 0; i < relations.Count; i++)
            {
                var rel = relations[i];
                if (rel == null || rel.grief <= 0f || rel.grief_since_day < 0) continue;

                int daysSince = _simDay - rel.grief_since_day;
                if (daysSince < 0 || daysSince >= Ashfall.Core.Memorial.RelationsGriefSink.BondGriefDurationDays)
                {
                    // Window expired — clear any rate the previous window set.
                    needs.RemoveExternalModifier(rel.dwellerA, GriefBondModifierSource, NeedKind.Morale);
                    needs.RemoveExternalModifier(rel.dwellerA, GriefBondModifierSource, NeedKind.Fatigue);
                    needs.RemoveExternalModifier(rel.dwellerB, GriefBondModifierSource, NeedKind.Morale);
                    needs.RemoveExternalModifier(rel.dwellerB, GriefBondModifierSource, NeedKind.Fatigue);
                    continue;
                }

                // Linear decay to zero across the window via the sink's pure
                // rate function; intensity from the persisted relationship
                // grief (absorbs the death-quality scale).
                float moralePerHour = Ashfall.Core.Memorial.RelationsGriefSink.BondMoralePerHour(
                    rel.grief, daysSince);
                float fatiguePerHour = Ashfall.Core.Memorial.RelationsGriefSink.BondFatiguePerHour(
                    rel.grief, daysSince);
                if (moralePerHour == 0f && fatiguePerHour == 0f) continue;
                needs.SetExternalModifier(rel.dwellerA, GriefBondModifierSource, NeedKind.Morale, moralePerHour, priority: 25);
                needs.SetExternalModifier(rel.dwellerA, GriefBondModifierSource, NeedKind.Fatigue, fatiguePerHour, priority: 25);
                needs.SetExternalModifier(rel.dwellerB, GriefBondModifierSource, NeedKind.Morale, moralePerHour, priority: 25);
                needs.SetExternalModifier(rel.dwellerB, GriefBondModifierSource, NeedKind.Fatigue, fatiguePerHour, priority: 25);
            }
        }

        /// <summary>
        /// Plan 24B A2 — measured overwork consequences. Runs at the daily
        /// boundary beside the schedule projection: each overworked survivor's
        /// excess hours route data-authored fatigue/morale rates through the
        /// shared needs modifier seam (the A1 overwork source slot). Rates are
        /// refreshed per day (replace-on-Set, no accumulation) and removed
        /// when the assignment no longer overworks the survivor.
        /// </summary>
        public void ApplyOverworkNeedsModifiers()
        {
            if (_survivors?.Needs == null) return;
            SetupFitnessForDuty();
            if (_fitnessRoleCatalog == null) return; // catalog unavailable — legacy no-op
            var ledger = EnsureDutyHourLedger();
            var needs = _survivors.Needs;
            var rules = _fitnessRoleCatalog.Overwork;

            var assigned = ledger.Overworked();
            var overworkedIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < assigned.Count; i++)
            {
                var snapshot = assigned[i];
                if (string.IsNullOrEmpty(snapshot.SurvivorId)) continue;
                var survivor = needs.Get(snapshot.SurvivorId);
                if (survivor == null || !survivor.IsAliveState) continue;
                overworkedIds.Add(snapshot.SurvivorId);

                float excess = snapshot.ExcessHours;
                // Per-day totals converted to per-hour rates over the nominal
                // 24h day (the shelter-schedule precedent), so the campaign's
                // one 24-hour needs tick applies exactly the authored total.
                needs.SetExternalModifier(
                    snapshot.SurvivorId, OverworkFatigueModifierSource, NeedKind.Fatigue,
                    excess * rules.FatiguePerExcessHour / 24f, priority: 25);
                needs.SetExternalModifier(
                    snapshot.SurvivorId, OverworkMoraleModifierSource, NeedKind.Morale,
                    excess * rules.MoralePerExcessHour / 24f, priority: 25);
            }

            // Resolved: assignments that no longer overwork lose their rates.
            var assignments = _dutyRoster?.Roster.State.assignments;
            if (assignments != null)
            {
                for (int i = 0; i < assignments.Count; i++)
                {
                    var a = assignments[i];
                    if (a == null || string.IsNullOrEmpty(a.survivorId)) continue;
                    if (overworkedIds.Contains(a.survivorId)) continue;
                    needs.RemoveExternalModifier(a.survivorId, OverworkFatigueModifierSource, NeedKind.Fatigue);
                    needs.RemoveExternalModifier(a.survivorId, OverworkMoraleModifierSource, NeedKind.Morale);
                }
            }
        }

        /// <summary>
        /// Revalidates committed shifts after authoritative condition owners
        /// tick. This is event-boundary enforcement, not per-frame polling: a
        /// role that has become unavailable is released through the roster's
        /// canonical vacancy callback and briefing queue.
        /// </summary>
        private void VacateInvalidFitnessAssignments()
        {
            SetupDutyRoster();
            var roster = _dutyRoster.Roster;
            var assignments = new List<(string role, string survivor)>();
            var current = roster.State.assignments;
            for (int i = 0; i < current.Count; i++)
            {
                var assignment = current[i];
                if (assignment == null || string.IsNullOrEmpty(assignment.role)
                    || string.IsNullOrEmpty(assignment.survivorId)) continue;
                assignments.Add((assignment.role, assignment.survivorId));
            }

            for (int i = 0; i < assignments.Count; i++)
            {
                var assignment = assignments[i];
                var verdict = EvaluateDutyRoleFitness(assignment.survivor, assignment.role);
                if (verdict.Allowed) continue;
                roster.RemoveAssignmentsFor(assignment.survivor);
            }
        }

        /// <summary>
        /// Projects the existing shelter sleep assignment into the one needs
        /// modifier seam. The schedule owns who has a bed; NeedsSystem remains
        /// the sole writer of fatigue. The derived rate is refreshed once per
        /// day so changing a bed cannot accumulate duplicate modifiers.
        /// </summary>
        private void ApplyScheduleNeedsModifiers()
        {
            if (_survivors?.Needs == null) return;
            SetupShelterSchedule();

            for (int i = 0; i < _survivors.Needs.Registered.Count; i++)
            {
                var survivor = _survivors.Needs.Registered[i];
                if (survivor == null || string.IsNullOrEmpty(survivor.Id)) continue;

                _survivors.Needs.RemoveExternalModifier(
                    survivor.Id, SleepScheduleModifierSource, NeedKind.Fatigue);
                if (!survivor.IsAliveState || _shelterSchedule == null
                    || !_shelterSchedule.System.IsSleepEligible(survivor.Id)) continue;

                // Six points of fatigue recovery over a nominal eight-hour rest
                // period, scaled by the authored schedule modifier. This is a
                // per-day projection because the campaign owner advances needs
                // in one 24-hour step.
                float recoveryPerHour = -6f
                    * Math.Max(0f, _shelterSchedule.System.FatigueRecoveryModifier) / 24f;
                _survivors.Needs.SetExternalModifier(
                    survivor.Id, SleepScheduleModifierSource, NeedKind.Fatigue,
                    recoveryPerHour, priority: 20);
            }
        }
    }
}
