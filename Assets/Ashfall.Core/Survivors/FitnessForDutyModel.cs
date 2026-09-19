// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Survivors
{
    /// <summary>Ordered severity used by health-aware work validation.</summary>
    public enum FitnessLevel
    {
        Fit = 0,
        Impaired = 1,
        Unfit = 2,
        Incapacitated = 3
    }

    /// <summary>
    /// Stable reason ids. These are machine-facing ids, not prose. Hosts may
    /// resolve them to localized explanation text at the presentation seam.
    /// </summary>
    public static class FitnessReasonIds
    {
        public const string Dead = "dead";
        public const string Quarantined = "quarantined";
        public const string Unconscious = "unconscious";
        public const string MedicallyIncapacitated = "medically_incapacitated";
        public const string SevereArs = "severe_ars";
        public const string CriticalHealth = "critical_health";
        public const string SevereFatigue = "severe_fatigue";
        public const string SleepDeprived = "sleep_deprived";
        public const string Starving = "starving";
        public const string Dehydrated = "dehydrated";
        public const string Hypothermic = "hypothermic";
        public const string DoseHigh = "dose_high";
        public const string Infectious = "infectious";
        public const string Withdrawal = "withdrawal";
        public const string CombatTrauma = "combat_trauma";
        public const string RecentDischarge = "recent_discharge";
        public const string ActiveIllness = "active_illness";
        public const string TerminalIllness = "terminal_illness";
        public const string OutcomePending = "illness_outcome_pending";
        public const string RoleSkillBelowMinimum = "role_skill_below_minimum";
        public const string RoleFatigueLimit = "role_fatigue_limit";
        public const string RoleHealthLimit = "role_health_limit";
        public const string RoleDoseLimit = "role_dose_limit";
        public const string RoleQuarantine = "role_quarantine";
        public const string LowConditioning = "low_conditioning";
    }

    /// <summary>Sanctioned hazard classes used by the live duty-role catalog.</summary>
    public static class DutyHazardClassIds
    {
        public const string Perimeter = "perimeter";
        public const string Food = "food";
        public const string Airlock = "airlock";
        public const string Intake = "intake";
        public const string Surface = "surface";
        public const string Medical = "medical";

        public static bool IsKnown(string value)
        {
            switch (value)
            {
                case Perimeter:
                case Food:
                case Airlock:
                case Intake:
                case Surface:
                case Medical:
                    return true;
                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Engine-neutral snapshot supplied by a host. It is deliberately a facts
    /// object rather than a second survivor state owner; fitness is derived
    /// from the existing survivor, needs, disease, dose, and skill authorities.
    /// </summary>
    public sealed class FitnessEvaluationFacts
    {
        public string SurvivorId { get; set; } = string.Empty;
        public bool IsAlive { get; set; } = true;
        public bool IsDead { get; set; }
        public bool IsQuarantined { get; set; }
        public bool IsUnconscious { get; set; }
        public bool IsMedicallyIncapacitated { get; set; }
        public bool IsInfectious { get; set; }
        public bool HasSevereArs { get; set; }
        public bool HasActiveWithdrawal { get; set; }
        public bool HasCombatTrauma { get; set; }
        public bool HasRespiratoryImpairment { get; set; }
        public float Health { get; set; } = 100f;
        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Fatigue { get; set; }
        public float Warmth { get; set; } = 100f;
        /// <summary>
        /// Persisted conditioning projection from ExerciseSystem. A default of
        /// 100 keeps callers that do not participate in the exercise package
        /// byte-compatible while the host wires the live profile when present.
        /// </summary>
        public float Conditioning { get; set; } = 100f;
        public float CumulativeDoseMsv { get; set; }
        public int DaysSinceSleep { get; set; } = -1;
        public int DaysSinceDischarge { get; set; } = -1;
        /// <summary>Active illness-sourced sick-list band, or -1 when absent/released.</summary>
        public int ActiveIllnessBand { get; set; } = -1;
        public IReadOnlyDictionary<string, float> SkillLevels { get; set; } =
            new Dictionary<string, float>(StringComparer.Ordinal);
    }

    /// <summary>Data-authored global thresholds for base fitness.</summary>
    public sealed class FitnessThresholds
    {
        public float FatigueImpaired { get; }
        public float FatigueUnfit { get; }
        public float HealthImpaired { get; }
        public float HealthUnfit { get; }
        public float HungerImpaired { get; }
        public float HungerUnfit { get; }
        public float ThirstImpaired { get; }
        public float ThirstUnfit { get; }
        public float WarmthImpaired { get; }
        public float WarmthUnfit { get; }
        public int DaysWithoutSleepImpaired { get; }
        public int DaysWithoutSleepUnfit { get; }
        public int DischargeRecoveryDays { get; }
        public int SickBandImpaired { get; }
        public int SickBandUnfit { get; }
        public int SickBandIncapacitated { get; }
        public float DoseImpairedMsv { get; }
        public float DoseUnfitMsv { get; }

        public FitnessThresholds(
            float fatigueImpaired,
            float fatigueUnfit,
            float healthImpaired,
            float healthUnfit,
            float hungerImpaired,
            float hungerUnfit,
            float thirstImpaired,
            float thirstUnfit,
            float warmthImpaired,
            float warmthUnfit,
            int daysWithoutSleepImpaired,
            int daysWithoutSleepUnfit,
            float doseImpairedMsv,
            float doseUnfitMsv,
            int dischargeRecoveryDays = 2,
            int sickBandImpaired = 1,
            int sickBandUnfit = 2,
            int sickBandIncapacitated = 3)
        {
            FatigueImpaired = fatigueImpaired;
            FatigueUnfit = fatigueUnfit;
            HealthImpaired = healthImpaired;
            HealthUnfit = healthUnfit;
            HungerImpaired = hungerImpaired;
            HungerUnfit = hungerUnfit;
            ThirstImpaired = thirstImpaired;
            ThirstUnfit = thirstUnfit;
            WarmthImpaired = warmthImpaired;
            WarmthUnfit = warmthUnfit;
            DaysWithoutSleepImpaired = daysWithoutSleepImpaired;
            DaysWithoutSleepUnfit = daysWithoutSleepUnfit;
            DischargeRecoveryDays = dischargeRecoveryDays;
            SickBandImpaired = sickBandImpaired;
            SickBandUnfit = sickBandUnfit;
            SickBandIncapacitated = sickBandIncapacitated;
            DoseImpairedMsv = doseImpairedMsv;
            DoseUnfitMsv = doseUnfitMsv;
        }
    }

    /// <summary>Data-authored role suitability constraints.</summary>
    public sealed class RoleRequirements
    {
        public string RoleId { get; }
        public string SkillId { get; }
        public float MinimumSkill { get; }
        public float MaximumFatigue { get; }
        public float MinimumHealth { get; }
        public float MaximumDoseMsv { get; }
        public float MaximumHours { get; }
        public float MaximumHoursIfImpaired { get; }
        public bool AllowUnfit { get; }
        public bool LightDuty { get; }
        public bool RequiresNotQuarantined { get; }
        public bool PrecisionWork { get; }
        public string HazardClass { get; }

        public RoleRequirements(
            string roleId,
            string skillId,
            float minimumSkill,
            float maximumFatigue,
            float minimumHealth,
            float maximumDoseMsv,
            float maximumHours,
            float maximumHoursIfImpaired,
            bool allowUnfit,
            bool lightDuty,
            bool requiresNotQuarantined,
            bool precisionWork,
            string hazardClass)
        {
            RoleId = roleId ?? string.Empty;
            SkillId = skillId ?? string.Empty;
            MinimumSkill = minimumSkill;
            MaximumFatigue = maximumFatigue;
            MinimumHealth = minimumHealth;
            MaximumDoseMsv = maximumDoseMsv;
            MaximumHours = maximumHours;
            MaximumHoursIfImpaired = maximumHoursIfImpaired;
            AllowUnfit = allowUnfit;
            LightDuty = lightDuty;
            RequiresNotQuarantined = requiresNotQuarantined;
            PrecisionWork = precisionWork;
            HazardClass = hazardClass ?? string.Empty;
        }
    }

    /// <summary>Deterministic result of evaluating one survivor's base health.</summary>
    public sealed class FitnessVerdict
    {
        public string SurvivorId { get; }
        public FitnessLevel Level { get; }
        public IReadOnlyList<string> BlockingReasons { get; }
        public IReadOnlyList<string> DegradedFactors { get; }
        public IReadOnlyList<NeedKind> AffectedNeeds { get; }

        public bool IsHardBlocked => Level == FitnessLevel.Incapacitated;
        public bool HasBlockingReasons => BlockingReasons.Count > 0;

        /// <summary>
        /// Base duty-hours recommendation before role-specific limits. Role
        /// requirements may tighten this value further for hazardous work.
        /// </summary>
        public float RecommendedMaxHours => Level switch
        {
            FitnessLevel.Fit => 12f,
            FitnessLevel.Impaired => 8f,
            _ => 0f
        };

        public FitnessVerdict(
            string survivorId,
            FitnessLevel level,
            IReadOnlyList<string> blockingReasons,
            IReadOnlyList<string> degradedFactors,
            IReadOnlyList<NeedKind> affectedNeeds)
        {
            SurvivorId = survivorId ?? string.Empty;
            Level = level;
            BlockingReasons = Copy(blockingReasons);
            DegradedFactors = Copy(degradedFactors);
            AffectedNeeds = Copy(affectedNeeds);
        }

        private static IReadOnlyList<T> Copy<T>(IReadOnlyList<T> source)
        {
            return new List<T>(source ?? Array.Empty<T>()).AsReadOnly();
        }
    }

    /// <summary>Role-specific extension of a base fitness verdict.</summary>
    public sealed class RoleFitnessVerdict
    {
        public string SurvivorId { get; }
        public string RoleId { get; }
        public FitnessVerdict BaseVerdict { get; }
        public bool Allowed { get; }
        public bool Warning { get; }
        public bool RequiresConfirmation => Allowed && Warning;
        public IReadOnlyList<string> BlockingReasons { get; }
        public IReadOnlyList<string> WarningReasons { get; }
        public float RecommendedMaxHours { get; }

        public RoleFitnessVerdict(
            string survivorId,
            string roleId,
            FitnessVerdict baseVerdict,
            bool allowed,
            bool warning,
            IReadOnlyList<string> blockingReasons,
            IReadOnlyList<string> warningReasons,
            float recommendedMaxHours)
        {
            SurvivorId = survivorId ?? string.Empty;
            RoleId = roleId ?? string.Empty;
            BaseVerdict = baseVerdict;
            Allowed = allowed;
            Warning = warning;
            BlockingReasons = new List<string>(blockingReasons ?? Array.Empty<string>()).AsReadOnly();
            WarningReasons = new List<string>(warningReasons ?? Array.Empty<string>()).AsReadOnly();
            RecommendedMaxHours = recommendedMaxHours;
        }
    }

    /// <summary>Immutable-at-use catalog of the five live duty roles.</summary>
    public sealed class FitnessRoleCatalog
    {
        private readonly Dictionary<string, RoleRequirements> _roles;

        public FitnessThresholds Thresholds { get; }
        public IReadOnlyDictionary<string, RoleRequirements> Roles => _roles;
        /// <summary>Plan 24B A2 data-authored overwork magnitudes (defaults
        /// when the block is absent — legacy catalogs stay loadable).</summary>
        public DutyOverworkRules Overwork { get; }
        /// <summary>Plan 24B A2 data-authored skill-to-yield band.</summary>
        public WorkerYieldBand WorkerYield { get; }

        public FitnessRoleCatalog(FitnessThresholds thresholds, IEnumerable<RoleRequirements> roles,
            DutyOverworkRules? overwork = null, WorkerYieldBand? workerYield = null)
        {
            Thresholds = thresholds ?? throw new ArgumentNullException(nameof(thresholds));
            Overwork = overwork ?? DutyOverworkRules.Default();
            WorkerYield = workerYield ?? WorkerYieldBand.Default();
            _roles = new Dictionary<string, RoleRequirements>(StringComparer.Ordinal);
            if (roles == null) return;
            foreach (var role in roles)
            {
                if (role == null || string.IsNullOrEmpty(role.RoleId)) continue;
                _roles[role.RoleId] = role;
            }
        }

        public bool TryGetRole(string roleId, out RoleRequirements requirements)
        {
            return _roles.TryGetValue(roleId ?? string.Empty, out requirements!);
        }
    }

    /// <summary>
    /// Pure fitness evaluator. It owns no survivor state and therefore needs no
    /// save section; re-evaluating from the restored authorities is sufficient.
    /// </summary>
    public sealed class FitnessForDutyModel
    {
        private readonly FitnessThresholds _thresholds;

        public FitnessForDutyModel(FitnessThresholds thresholds)
        {
            _thresholds = thresholds ?? throw new ArgumentNullException(nameof(thresholds));
        }

        public FitnessVerdict Evaluate(FitnessEvaluationFacts facts)
        {
            if (facts == null) throw new ArgumentNullException(nameof(facts));

            var blockers = new List<string>();
            var degraded = new List<string>();
            var needs = new List<NeedKind>();
            FitnessLevel level = FitnessLevel.Fit;

            if (facts.IsDead || !facts.IsAlive)
            {
                Add(blockers, FitnessReasonIds.Dead);
                level = Max(level, FitnessLevel.Incapacitated);
            }
            if (facts.IsQuarantined)
            {
                Add(blockers, FitnessReasonIds.Quarantined);
                level = Max(level, FitnessLevel.Incapacitated);
            }
            if (facts.IsUnconscious)
            {
                Add(blockers, FitnessReasonIds.Unconscious);
                level = Max(level, FitnessLevel.Incapacitated);
            }
            if (facts.IsMedicallyIncapacitated)
            {
                Add(blockers, FitnessReasonIds.MedicallyIncapacitated);
                level = Max(level, FitnessLevel.Incapacitated);
            }
            if (facts.HasSevereArs)
            {
                Add(blockers, FitnessReasonIds.SevereArs);
                level = Max(level, FitnessLevel.Incapacitated);
            }

            if (facts.Health <= _thresholds.HealthUnfit)
            {
                Add(degraded, FitnessReasonIds.CriticalHealth);
                Add(needs, NeedKind.Health);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.Health <= _thresholds.HealthImpaired)
            {
                Add(degraded, FitnessReasonIds.CriticalHealth);
                Add(needs, NeedKind.Health);
                level = Max(level, FitnessLevel.Impaired);
            }

            if (facts.Fatigue >= _thresholds.FatigueUnfit)
            {
                Add(degraded, FitnessReasonIds.SevereFatigue);
                Add(needs, NeedKind.Fatigue);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.Fatigue >= _thresholds.FatigueImpaired)
            {
                Add(degraded, FitnessReasonIds.SevereFatigue);
                Add(needs, NeedKind.Fatigue);
                level = Max(level, FitnessLevel.Impaired);
            }

            // Plan 216: conditioning is a capability projection, not a second
            // health/needs authority. The host supplies the persisted exercise
            // profile; these conservative bands only affect duty fitness.
            if (facts.Conditioning <= 20f)
            {
                Add(degraded, FitnessReasonIds.LowConditioning);
                Add(needs, NeedKind.Fatigue);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.Conditioning <= 25f)
            {
                Add(degraded, FitnessReasonIds.LowConditioning);
                Add(needs, NeedKind.Fatigue);
                level = Max(level, FitnessLevel.Impaired);
            }

            if (facts.Hunger >= _thresholds.HungerUnfit)
            {
                Add(degraded, FitnessReasonIds.Starving);
                Add(needs, NeedKind.Hunger);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.Hunger >= _thresholds.HungerImpaired)
            {
                Add(degraded, FitnessReasonIds.Starving);
                Add(needs, NeedKind.Hunger);
                level = Max(level, FitnessLevel.Impaired);
            }

            if (facts.Thirst >= _thresholds.ThirstUnfit)
            {
                Add(degraded, FitnessReasonIds.Dehydrated);
                Add(needs, NeedKind.Thirst);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.Thirst >= _thresholds.ThirstImpaired)
            {
                Add(degraded, FitnessReasonIds.Dehydrated);
                Add(needs, NeedKind.Thirst);
                level = Max(level, FitnessLevel.Impaired);
            }

            if (facts.Warmth <= _thresholds.WarmthUnfit)
            {
                Add(degraded, FitnessReasonIds.Hypothermic);
                Add(needs, NeedKind.Warmth);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.Warmth <= _thresholds.WarmthImpaired)
            {
                Add(degraded, FitnessReasonIds.Hypothermic);
                Add(needs, NeedKind.Warmth);
                level = Max(level, FitnessLevel.Impaired);
            }

            if (facts.DaysSinceSleep >= _thresholds.DaysWithoutSleepUnfit)
            {
                Add(degraded, FitnessReasonIds.SleepDeprived);
                Add(needs, NeedKind.Fatigue);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.DaysSinceSleep >= _thresholds.DaysWithoutSleepImpaired)
            {
                Add(degraded, FitnessReasonIds.SleepDeprived);
                Add(needs, NeedKind.Fatigue);
                level = Max(level, FitnessLevel.Impaired);
            }

            // Discharge is not an automatic return to full fitness. The
            // discharge day is already persisted by MedicalWardState, so this
            // recovery projection needs no second save authority.
            if (_thresholds.DischargeRecoveryDays > 0
                && facts.DaysSinceDischarge >= 0
                && facts.DaysSinceDischarge < _thresholds.DischargeRecoveryDays)
            {
                Add(degraded, FitnessReasonIds.RecentDischarge);
                level = Max(level, FitnessLevel.Impaired);
            }

            if (facts.CumulativeDoseMsv >= _thresholds.DoseUnfitMsv)
            {
                Add(degraded, FitnessReasonIds.DoseHigh);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.CumulativeDoseMsv >= _thresholds.DoseImpairedMsv)
            {
                Add(degraded, FitnessReasonIds.DoseHigh);
                level = Max(level, FitnessLevel.Impaired);
            }

            if (facts.IsInfectious)
            {
                Add(degraded, FitnessReasonIds.Infectious);
                level = Max(level, FitnessLevel.Impaired);
            }
            if (facts.ActiveIllnessBand >= _thresholds.SickBandIncapacitated)
            {
                Add(blockers, FitnessReasonIds.OutcomePending);
                level = Max(level, FitnessLevel.Incapacitated);
            }
            else if (facts.ActiveIllnessBand >= _thresholds.SickBandUnfit)
            {
                Add(degraded, FitnessReasonIds.TerminalIllness);
                level = Max(level, FitnessLevel.Unfit);
            }
            else if (facts.ActiveIllnessBand >= _thresholds.SickBandImpaired)
            {
                Add(degraded, FitnessReasonIds.ActiveIllness);
                level = Max(level, FitnessLevel.Impaired);
            }
            if (facts.HasActiveWithdrawal)
            {
                Add(degraded, FitnessReasonIds.Withdrawal);
                level = Max(level, FitnessLevel.Impaired);
            }
            if (facts.HasCombatTrauma)
            {
                Add(degraded, FitnessReasonIds.CombatTrauma);
                level = Max(level, FitnessLevel.Impaired);
            }

            return new FitnessVerdict(facts.SurvivorId, level, blockers, degraded, needs);
        }

        public RoleFitnessVerdict EvaluateForRole(
            FitnessEvaluationFacts facts,
            RoleRequirements requirements)
        {
            if (facts == null) throw new ArgumentNullException(nameof(facts));
            if (requirements == null) throw new ArgumentNullException(nameof(requirements));

            var baseVerdict = Evaluate(facts);
            var blockers = new List<string>(baseVerdict.BlockingReasons);
            var warnings = new List<string>(baseVerdict.DegradedFactors);

            if (requirements.RequiresNotQuarantined && facts.IsQuarantined)
                Add(blockers, FitnessReasonIds.RoleQuarantine);

            if (facts.Fatigue > requirements.MaximumFatigue)
                Add(blockers, FitnessReasonIds.RoleFatigueLimit);
            if (facts.Health < requirements.MinimumHealth)
                Add(blockers, FitnessReasonIds.RoleHealthLimit);
            if (requirements.MaximumDoseMsv > 0f && facts.CumulativeDoseMsv > requirements.MaximumDoseMsv)
                Add(blockers, FitnessReasonIds.RoleDoseLimit);

            if (!string.IsNullOrEmpty(requirements.SkillId))
            {
                float skill = 0f;
                if (facts.SkillLevels != null)
                    facts.SkillLevels.TryGetValue(requirements.SkillId, out skill);
                if (skill < requirements.MinimumSkill)
                    Add(blockers, FitnessReasonIds.RoleSkillBelowMinimum);
            }

            // A quarantined/dead/medically incapacitated person is never made
            // assignable by a permissive role. AllowUnfit only means that a
            // non-incapacitated unfit person may cover an explicitly marked
            // light-duty role when its data opts in.
            bool incapacitated = baseVerdict.Level == FitnessLevel.Incapacitated;
            bool roleThresholdFailure = blockers.Count > baseVerdict.BlockingReasons.Count;
            bool allowed = !incapacitated
                && (baseVerdict.Level == FitnessLevel.Fit
                    || baseVerdict.Level == FitnessLevel.Impaired
                    || (baseVerdict.Level == FitnessLevel.Unfit
                        && requirements.AllowUnfit && requirements.LightDuty))
                && !roleThresholdFailure;

            bool warning = allowed && (baseVerdict.Level != FitnessLevel.Fit || warnings.Count > 0);
            float hours = requirements.MaximumHours > 0f ? requirements.MaximumHours : 12f;
            if (baseVerdict.Level == FitnessLevel.Impaired)
                hours = requirements.MaximumHoursIfImpaired > 0f
                    ? Math.Min(hours, requirements.MaximumHoursIfImpaired)
                    : Math.Min(hours, 6f);
            else if (baseVerdict.Level == FitnessLevel.Unfit && requirements.LightDuty)
                hours = requirements.MaximumHoursIfImpaired > 0f
                    ? Math.Min(hours, requirements.MaximumHoursIfImpaired)
                    : Math.Min(hours, 4f);
            if (!allowed) hours = 0f;

            var roleVerdict = new RoleFitnessVerdict(
                facts.SurvivorId,
                requirements.RoleId,
                baseVerdict,
                allowed,
                warning,
                blockers,
                warnings,
                hours);
            return AfflictionDutyBridge.ApplyToRoleVerdict(facts, requirements, roleVerdict);
        }

        private static FitnessLevel Max(FitnessLevel a, FitnessLevel b) => a > b ? a : b;

        private static void Add(List<string> list, string value)
        {
            if (!list.Contains(value)) list.Add(value);
        }

        private static void Add(List<NeedKind> list, NeedKind value)
        {
            if (!list.Contains(value)) list.Add(value);
        }
    }

    /// <summary>Result of loading the data-authored duty fitness catalog.</summary>
    public sealed class FitnessRoleCatalogLoadResult
    {
        public FitnessRoleCatalog? Catalog { get; internal set; }
        public List<string> Errors { get; } = new List<string>();
        public bool IsSuccess => Catalog != null && Errors.Count == 0;
    }

    [Serializable]
    internal sealed class FitnessRoleCatalogDto
    {
        [JsonPropertyName("schema_version")] public int SchemaVersion { get; set; }
        [JsonPropertyName("collection_id")] public string CollectionId { get; set; } = string.Empty;
        [JsonPropertyName("thresholds")] public FitnessThresholdsDto Thresholds { get; set; } = new FitnessThresholdsDto();
        [JsonPropertyName("roles")] public List<RoleRequirementsDto> Roles { get; set; } = new List<RoleRequirementsDto>();
        [JsonPropertyName("overwork")] public DutyOverworkRulesDto? Overwork { get; set; }
        [JsonPropertyName("worker_yield")] public WorkerYieldBandDto? WorkerYield { get; set; }
    }

    [Serializable]
    internal sealed class DutyOverworkRulesDto
    {
        [JsonPropertyName("fatigue_per_excess_hour")] public float FatiguePerExcessHour { get; set; }
        [JsonPropertyName("morale_per_excess_hour")] public float MoralePerExcessHour { get; set; }
        [JsonPropertyName("yield_penalty_permille")] public int YieldPenaltyPermille { get; set; }

        public DutyOverworkRules ToDomain() => new DutyOverworkRules(
            FatiguePerExcessHour, MoralePerExcessHour, YieldPenaltyPermille);
    }

    [Serializable]
    internal sealed class WorkerYieldBandDto
    {
        [JsonPropertyName("floor_permille")] public int FloorPermille { get; set; }
        [JsonPropertyName("cap_permille")] public int CapPermille { get; set; }
        [JsonPropertyName("impaired_penalty_permille")] public int ImpairedPenaltyPermille { get; set; }
        [JsonPropertyName("absolute_floor_permille")] public int AbsoluteFloorPermille { get; set; }

        public WorkerYieldBand ToDomain() => new WorkerYieldBand(
            FloorPermille, CapPermille, ImpairedPenaltyPermille, AbsoluteFloorPermille);
    }

    [Serializable]
    internal sealed class FitnessThresholdsDto
    {
        [JsonPropertyName("fatigue_impaired")] public float FatigueImpaired { get; set; }
        [JsonPropertyName("fatigue_unfit")] public float FatigueUnfit { get; set; }
        [JsonPropertyName("health_impaired")] public float HealthImpaired { get; set; }
        [JsonPropertyName("health_unfit")] public float HealthUnfit { get; set; }
        [JsonPropertyName("hunger_impaired")] public float HungerImpaired { get; set; }
        [JsonPropertyName("hunger_unfit")] public float HungerUnfit { get; set; }
        [JsonPropertyName("thirst_impaired")] public float ThirstImpaired { get; set; }
        [JsonPropertyName("thirst_unfit")] public float ThirstUnfit { get; set; }
        [JsonPropertyName("warmth_impaired")] public float WarmthImpaired { get; set; }
        [JsonPropertyName("warmth_unfit")] public float WarmthUnfit { get; set; }
        [JsonPropertyName("days_without_sleep_impaired")] public int DaysWithoutSleepImpaired { get; set; }
        [JsonPropertyName("days_without_sleep_unfit")] public int DaysWithoutSleepUnfit { get; set; }
        [JsonPropertyName("discharge_recovery_days")] public int DischargeRecoveryDays { get; set; } = 2;
        [JsonPropertyName("sick_band_impaired")] public int SickBandImpaired { get; set; } = 1;
        [JsonPropertyName("sick_band_unfit")] public int SickBandUnfit { get; set; } = 2;
        [JsonPropertyName("sick_band_incapacitated")] public int SickBandIncapacitated { get; set; } = 3;
        [JsonPropertyName("dose_impaired_msv")] public float DoseImpairedMsv { get; set; }
        [JsonPropertyName("dose_unfit_msv")] public float DoseUnfitMsv { get; set; }

        public FitnessThresholds ToDomain()
        {
            return new FitnessThresholds(
                FatigueImpaired, FatigueUnfit, HealthImpaired, HealthUnfit,
                HungerImpaired, HungerUnfit, ThirstImpaired, ThirstUnfit,
                WarmthImpaired, WarmthUnfit, DaysWithoutSleepImpaired,
                DaysWithoutSleepUnfit, DoseImpairedMsv, DoseUnfitMsv,
                DischargeRecoveryDays, SickBandImpaired, SickBandUnfit,
                SickBandIncapacitated);
        }
    }

    [Serializable]
    internal sealed class RoleRequirementsDto
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("skill_id")] public string SkillId { get; set; } = string.Empty;
        [JsonPropertyName("minimum_skill")] public float MinimumSkill { get; set; }
        [JsonPropertyName("maximum_fatigue")] public float MaximumFatigue { get; set; }
        [JsonPropertyName("minimum_health")] public float MinimumHealth { get; set; }
        [JsonPropertyName("maximum_dose_msv")] public float MaximumDoseMsv { get; set; }
        [JsonPropertyName("maximum_hours")] public float MaximumHours { get; set; }
        [JsonPropertyName("maximum_hours_if_impaired")] public float MaximumHoursIfImpaired { get; set; }
        [JsonPropertyName("allow_unfit")] public bool AllowUnfit { get; set; }
        [JsonPropertyName("light_duty")] public bool LightDuty { get; set; }
        [JsonPropertyName("requires_not_quarantined")] public bool RequiresNotQuarantined { get; set; }
        [JsonPropertyName("precision_work")] public bool PrecisionWork { get; set; }
        [JsonPropertyName("hazard_class")] public string HazardClass { get; set; } = string.Empty;

        public RoleRequirements ToDomain()
        {
            return new RoleRequirements(
                Id, SkillId, MinimumSkill, MaximumFatigue, MinimumHealth,
                MaximumDoseMsv, MaximumHours, MaximumHoursIfImpaired,
                AllowUnfit, LightDuty, RequiresNotQuarantined, PrecisionWork,
                HazardClass);
        }
    }

    /// <summary>Loader for duty_roles.json; no engine or host dependencies.</summary>
    public static class FitnessRoleCatalogLoader
    {
        public const string DefaultFileName = "duty_roles.json";

        public static FitnessRoleCatalogLoadResult LoadDetailed(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            var result = new FitnessRoleCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("duty_roles.json: missing loader arguments");
                return result;
            }

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add(path + ": file is missing");
                return result;
            }

            FitnessRoleCatalogDto? dto;
            try
            {
                dto = json.Deserialize<FitnessRoleCatalogDto>(fileIO.ReadAllText(path));
            }
            catch (Exception ex)
            {
                result.Errors.Add(path + ": invalid JSON: " + ex.Message);
                return result;
            }

            if (dto == null)
            {
                result.Errors.Add(path + ": empty document");
                return result;
            }
            if (dto.SchemaVersion != 1)
                result.Errors.Add(path + ": schema_version must be 1");
            if (!string.Equals(dto.CollectionId, "duty_roles", StringComparison.Ordinal))
                result.Errors.Add(path + ": collection_id must be 'duty_roles'");
            if (dto.Thresholds == null)
                result.Errors.Add(path + ": thresholds are required");
            else
                ValidateThresholds(dto.Thresholds, path, result.Errors);

            var domains = new List<RoleRequirements>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            if (dto.Roles == null)
            {
                result.Errors.Add(path + ": roles are required");
            }
            else
            {
                for (int i = 0; i < dto.Roles.Count; i++)
                {
                    var roleDto = dto.Roles[i];
                    if (roleDto == null || string.IsNullOrEmpty(roleDto.Id))
                    {
                        result.Errors.Add(path + ": role " + i + " has no id");
                        continue;
                    }
                    if (!seen.Add(roleDto.Id))
                    {
                        result.Errors.Add(path + ": duplicate role '" + roleDto.Id + "'");
                        continue;
                    }
                    if (roleDto.MinimumSkill < 0f || roleDto.MinimumSkill > 1f)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' minimum_skill must be 0..1");
                    if (roleDto.MaximumFatigue < 0f || roleDto.MaximumFatigue > 100f)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' maximum_fatigue must be 0..100");
                    if (roleDto.MinimumHealth < 0f || roleDto.MinimumHealth > 100f)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' minimum_health must be 0..100");
                    if (!IsFinite(roleDto.MaximumDoseMsv) || roleDto.MaximumDoseMsv < 0f)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' maximum_dose_msv must be finite and non-negative");
                    if (!IsFinite(roleDto.MaximumHours) || roleDto.MaximumHours < 0f || roleDto.MaximumHours > 24f)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' maximum_hours must be 0..24");
                    if (!IsFinite(roleDto.MaximumHoursIfImpaired) || roleDto.MaximumHoursIfImpaired < 0f
                        || roleDto.MaximumHoursIfImpaired > 24f
                        || (roleDto.MaximumHours > 0f && roleDto.MaximumHoursIfImpaired > roleDto.MaximumHours))
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' maximum_hours_if_impaired is invalid");
                    if (roleDto.AllowUnfit && !roleDto.LightDuty)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' allow_unfit requires light_duty");
                    if (roleDto.LightDuty && roleDto.PrecisionWork)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' cannot be both light_duty and precision_work");
                    if (roleDto.LightDuty && roleDto.MaximumHours > 8f)
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' light-duty maximum_hours must not exceed 8");
                    if (string.IsNullOrWhiteSpace(roleDto.HazardClass)
                        || !DutyHazardClassIds.IsKnown(roleDto.HazardClass))
                        result.Errors.Add(path + ": role '" + roleDto.Id + "' has an unknown hazard_class");
                    domains.Add(roleDto.ToDomain());
                }
            }

            for (int i = 0; i < DutyRosterIds.AssignmentRoles.Length; i++)
            {
                if (!seen.Contains(DutyRosterIds.AssignmentRoles[i]))
                    result.Errors.Add(path + ": missing live role '" + DutyRosterIds.AssignmentRoles[i] + "'");
            }

            if (result.Errors.Count == 0 && dto.Thresholds != null)
            {
                // Plan 24B A2: additive labor blocks. Absent blocks keep the
                // data-authored defaults so legacy catalogs load unchanged.
                var overworkRules = dto.Overwork?.ToDomain() ?? DutyOverworkRules.Default();
                if (dto.Overwork != null)
                {
                    if (!IsFinite(dto.Overwork.FatiguePerExcessHour) || dto.Overwork.FatiguePerExcessHour < 0f
                        || dto.Overwork.FatiguePerExcessHour > 10f)
                        result.Errors.Add(path + ": overwork.fatigue_per_excess_hour must be 0..10");
                    if (!IsFinite(dto.Overwork.MoralePerExcessHour) || dto.Overwork.MoralePerExcessHour > 0f
                        || dto.Overwork.MoralePerExcessHour < -10f)
                        result.Errors.Add(path + ": overwork.morale_per_excess_hour must be -10..0");
                    if (dto.Overwork.YieldPenaltyPermille < 0 || dto.Overwork.YieldPenaltyPermille > 500)
                        result.Errors.Add(path + ": overwork.yield_penalty_permille must be 0..500");
                }
                if (dto.WorkerYield != null)
                {
                    if (dto.WorkerYield.FloorPermille < 100 || dto.WorkerYield.FloorPermille > 1000)
                        result.Errors.Add(path + ": worker_yield.floor_permille must be 100..1000");
                    if (dto.WorkerYield.CapPermille < dto.WorkerYield.FloorPermille
                        || dto.WorkerYield.CapPermille > 2000)
                        result.Errors.Add(path + ": worker_yield.cap_permille must be ordered above floor within 2000");
                    if (dto.WorkerYield.ImpairedPenaltyPermille < 0 || dto.WorkerYield.ImpairedPenaltyPermille > 500)
                        result.Errors.Add(path + ": worker_yield.impaired_penalty_permille must be 0..500");
                    if (dto.WorkerYield.AbsoluteFloorPermille < 100
                        || dto.WorkerYield.AbsoluteFloorPermille > dto.WorkerYield.FloorPermille)
                        result.Errors.Add(path + ": worker_yield.absolute_floor_permille must be 100..floor");
                }
                if (result.Errors.Count == 0)
                    result.Catalog = new FitnessRoleCatalog(
                        dto.Thresholds.ToDomain(), domains,
                        overworkRules, dto.WorkerYield?.ToDomain() ?? WorkerYieldBand.Default());
            }
            return result;
        }

        private static void ValidateThresholds(
            FitnessThresholdsDto thresholds,
            string path,
            List<string> errors)
        {
            if (!IsFinite(thresholds.FatigueImpaired) || !IsFinite(thresholds.FatigueUnfit)
                || thresholds.FatigueImpaired < 0f || thresholds.FatigueUnfit > 100f
                || thresholds.FatigueImpaired >= thresholds.FatigueUnfit)
                errors.Add(path + ": fatigue thresholds must be ordered within 0..100");
            if (!IsFinite(thresholds.HealthImpaired) || !IsFinite(thresholds.HealthUnfit)
                || thresholds.HealthUnfit < 0f || thresholds.HealthImpaired > 100f
                || thresholds.HealthImpaired <= thresholds.HealthUnfit)
                errors.Add(path + ": health thresholds must be ordered within 0..100");
            if (!IsFinite(thresholds.HungerImpaired) || !IsFinite(thresholds.HungerUnfit)
                || thresholds.HungerImpaired < 0f || thresholds.HungerUnfit > 100f
                || thresholds.HungerImpaired >= thresholds.HungerUnfit)
                errors.Add(path + ": hunger thresholds must be ordered within 0..100");
            if (!IsFinite(thresholds.ThirstImpaired) || !IsFinite(thresholds.ThirstUnfit)
                || thresholds.ThirstImpaired < 0f || thresholds.ThirstUnfit > 100f
                || thresholds.ThirstImpaired >= thresholds.ThirstUnfit)
                errors.Add(path + ": thirst thresholds must be ordered within 0..100");
            if (!IsFinite(thresholds.WarmthImpaired) || !IsFinite(thresholds.WarmthUnfit)
                || thresholds.WarmthUnfit < 0f || thresholds.WarmthImpaired > 100f
                || thresholds.WarmthImpaired <= thresholds.WarmthUnfit)
                errors.Add(path + ": warmth thresholds must be ordered within 0..100");
            if (thresholds.DaysWithoutSleepImpaired < 0
                || thresholds.DaysWithoutSleepImpaired >= thresholds.DaysWithoutSleepUnfit)
                errors.Add(path + ": sleep thresholds must be ordered and non-negative");
            if (thresholds.DischargeRecoveryDays < 0 || thresholds.DischargeRecoveryDays > 30)
                errors.Add(path + ": discharge_recovery_days must be 0..30");
            if (thresholds.SickBandImpaired < 0
                || thresholds.SickBandImpaired >= thresholds.SickBandUnfit
                || thresholds.SickBandUnfit >= thresholds.SickBandIncapacitated
                || thresholds.SickBandIncapacitated > 3)
                errors.Add(path + ": sick-list fitness bands must be strictly ordered within 0..3");
            if (!IsFinite(thresholds.DoseImpairedMsv) || !IsFinite(thresholds.DoseUnfitMsv)
                || thresholds.DoseImpairedMsv < 0f
                || thresholds.DoseImpairedMsv >= thresholds.DoseUnfitMsv)
                errors.Add(path + ": dose thresholds must be ordered and non-negative");
        }

        private static bool IsFinite(float value)
            => !float.IsNaN(value) && !float.IsInfinity(value);

        public static FitnessRoleCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            return LoadDetailed(dataDir, fileIO, json).Catalog;
        }
    }
}
