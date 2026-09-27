// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CommonTableRationingSaveStore
// Core Engine: Ashfall.Core.Nutrition.CommonTableRationingEngine
// Host Caller: Main.CommonTableRationing
// Purpose    : Expansion 26 — common-table nutrition. The Core engine is the
//              sole authority over dietary diversity tiers, deficiency risk,
//              rationing policy effects, and cook-duty waste reduction; the
//              host owns only the meal-session ledger and its persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Nutrition;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persisted common-table state: the meal-session ledger plus policy facts.
    /// </summary>
    [Serializable]
    public sealed class CommonTableRationingSaveState
    {
        public int schema_version = 1;
        public int currentDay;
        public RationLevel activePolicy = RationLevel.Standard;
        public int consecutiveLeanDays;
        public int cookSkillPermille = 500;
        public int populationCount;
        public bool hasRationInequality;
        public List<MealSessionEntry> sessions = new List<MealSessionEntry>();
    }

    /// <summary>One recorded group meal session and its Core-evaluated outcome.</summary>
    [Serializable]
    public sealed class MealSessionEntry
    {
        public int day;
        public RationLevel policy;
        public List<int> consumedCategories = new List<int>();
        public int uniqueCategoryCount;
        public int netCalorieIntakePercent;
        public int deficiencyRiskPermille;
        public int moraleDeltaPermille;
        public int grievanceProbabilityPermille;
        public int requiredFoodUnits;
    }

    public static class CommonTableRationingSaveStore
    {
        public const string FileName = "common_table_rationing_save.json";
        public const string SectionName = "common_table_rationing";

        private static readonly SaveStore<CommonTableRationingSaveState> s_store =
            SaveStoreHub.Checksummed<CommonTableRationingSaveState>(FileName, nameof(CommonTableRationingSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(CommonTableRationingSaveState state) => s_store.CaptureBare(state);
        public static CommonTableRationingSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(CommonTableRationingSaveState state) => s_store.TrySave(state);
        public static CommonTableRationingSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the pure Core common-table rationing engine.</summary>
    public sealed class CommonTableRationingHostSession : HostSessionBase
    {
        private readonly CommonTableRationingSaveState _state;

        public CommonTableRationingHostSession(CommonTableRationingSaveState? state = null)
        {
            _state = state ?? new CommonTableRationingSaveState();
            _state.sessions ??= new List<MealSessionEntry>();
        }

        public static CommonTableRationingHostSession Create(CommonTableRationingSaveState? state = null) =>
            new CommonTableRationingHostSession(state);

        public string LastEvent { get; private set; } = string.Empty;

        public int CurrentDay => _state.currentDay;
        public int SessionCount => _state.sessions.Count;
        public IReadOnlyList<MealSessionEntry> Sessions => _state.sessions;
        public RationLevel ActivePolicy => _state.activePolicy;
        public int ConsecutiveLeanDays => _state.consecutiveLeanDays;
        public int CookSkillPermille => _state.cookSkillPermille;
        public int PopulationCount => _state.populationCount;
        public bool HasRationInequality => _state.hasRationInequality;

        public bool SetRationingPolicy(RationLevel policy)
        {
            if (policy == _state.activePolicy) return false;
            _state.activePolicy = policy;
            _state.consecutiveLeanDays = policy == RationLevel.StarvationEmergency || policy == RationLevel.LeanRation
                ? _state.consecutiveLeanDays + 1
                : 0;
            LastEvent = $"Common-table rationing policy set to {policy}.";
            RaiseStateChanged();
            return true;
        }

        public bool SetCookSkillPermille(int permille)
        {
            int clamped = Math.Max(0, Math.Min(1000, permille));
            if (clamped == _state.cookSkillPermille) return false;
            _state.cookSkillPermille = clamped;
            LastEvent = $"Cook duty skill set to {clamped} permille.";
            RaiseStateChanged();
            return true;
        }

        public bool SetPopulationCount(int count)
        {
            int clamped = Math.Max(0, count);
            if (clamped == _state.populationCount) return false;
            _state.populationCount = clamped;
            LastEvent = $"Common-table diners set to {clamped}.";
            RaiseStateChanged();
            return true;
        }

        public bool SetRationInequality(bool present)
        {
            if (present == _state.hasRationInequality) return false;
            _state.hasRationInequality = present;
            LastEvent = present ? "Rationing inequality reported at the common table." : "Rationing equalized.";
            RaiseStateChanged();
            return true;
        }

        /// <summary>
        /// Records one group meal. Every diversity, deficiency, grievance, and
        /// consumption outcome is decided by the Core engine; the host only
        /// supplies the categories served and the roster/skill facts.
        /// </summary>
        public NutritionEvaluationResult ServeMeal(int day, IEnumerable<FoodCategory> consumedCategories)
        {
            _state.currentDay = day;
            var result = CommonTableRationingEngine.Evaluate(
                consumedCategories ?? Enumerable.Empty<FoodCategory>(),
                _state.activePolicy,
                _state.cookSkillPermille,
                _state.populationCount,
                _state.consecutiveLeanDays,
                _state.hasRationInequality);

            _state.sessions.Add(new MealSessionEntry
            {
                day = day,
                policy = result.RationLevel,
                consumedCategories = (consumedCategories ?? Enumerable.Empty<FoodCategory>())
                    .Distinct().OrderBy(c => (int)c).Select(c => (int)c).ToList(),
                uniqueCategoryCount = result.UniqueCategoryCount,
                netCalorieIntakePercent = result.NetCalorieIntakePercent,
                deficiencyRiskPermille = result.DeficiencyRiskPermille,
                moraleDeltaPermille = result.MoraleDeltaPermille,
                grievanceProbabilityPermille = result.GrievanceProbabilityPermille,
                requiredFoodUnits = result.RequiredFoodUnits
            });

            LastEvent = $"Day {day} common-table meal: {result.UniqueCategoryCount} categories, net {result.NetCalorieIntakePercent}% calories, {result.RequiredFoodUnits} units.";
            RaiseStateChanged();
            return result;
        }

        /// <summary>
        /// Advances the lean-day streak for one elapsed day without a recorded
        /// meal. The Core engine decides the compounding deficiency risk; this
        /// only moves the host-owned streak counter.
        /// </summary>
        public int AdvanceDay(int day)
        {
            _state.currentDay = day;
            bool lean = _state.activePolicy == RationLevel.StarvationEmergency
                        || _state.activePolicy == RationLevel.LeanRation;
            _state.consecutiveLeanDays = lean ? _state.consecutiveLeanDays + 1 : 0;
            LastEvent = $"Common-table day {day} advanced; lean streak {_state.consecutiveLeanDays}.";
            RaiseStateChanged();
            return _state.consecutiveLeanDays;
        }

        public int StarvationGrievanceSessionCount => _state.sessions.Count(s =>
            s.policy == RationLevel.StarvationEmergency && s.grievanceProbabilityPermille > 0);

        public CommonTableRationingSaveState CaptureState()
        {
            var copy = new CommonTableRationingSaveState
            {
                schema_version = _state.schema_version,
                currentDay = _state.currentDay,
                activePolicy = _state.activePolicy,
                consecutiveLeanDays = _state.consecutiveLeanDays,
                cookSkillPermille = _state.cookSkillPermille,
                populationCount = _state.populationCount,
                hasRationInequality = _state.hasRationInequality
            };
            foreach (var s in _state.sessions)
            {
                copy.sessions.Add(new MealSessionEntry
                {
                    day = s.day,
                    policy = s.policy,
                    consumedCategories = new List<int>(s.consumedCategories ?? new List<int>()),
                    uniqueCategoryCount = s.uniqueCategoryCount,
                    netCalorieIntakePercent = s.netCalorieIntakePercent,
                    deficiencyRiskPermille = s.deficiencyRiskPermille,
                    moraleDeltaPermille = s.moraleDeltaPermille,
                    grievanceProbabilityPermille = s.grievanceProbabilityPermille,
                    requiredFoodUnits = s.requiredFoodUnits
                });
            }
            return copy;
        }

        public void RestoreState(CommonTableRationingSaveState? state)
        {
            _state.currentDay = state?.currentDay ?? 0;
            _state.activePolicy = state?.activePolicy ?? RationLevel.Standard;
            _state.consecutiveLeanDays = state?.consecutiveLeanDays ?? 0;
            _state.cookSkillPermille = state?.cookSkillPermille ?? 500;
            _state.populationCount = state?.populationCount ?? 0;
            _state.hasRationInequality = state?.hasRationInequality ?? false;
            _state.sessions.Clear();
            if (state == null) return;
            _state.schema_version = state.schema_version;
            foreach (var s in state.sessions ?? new List<MealSessionEntry>())
                if (s != null) _state.sessions.Add(s);
            LastEvent = "Restored common-table rationing state.";
            RaiseStateChanged();
        }

        public bool TrySave() => CommonTableRationingSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = CommonTableRationingSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
