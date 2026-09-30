// SPDX-License-Identifier: MIT
// ============================================================================
// Common-table nutrition host composition. The Core CommonTableRationingEngine
// is the sole authority over dietary diversity tiers, deficiency risk, ration
// policy effects, and cook-duty waste. The host owns only the meal-session
// ledger and supplies the categories served plus the roster/skill facts.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Nutrition;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CommonTableRationingHostSession? _commonTable;
        private bool _commonTableDirty;

        public CommonTableRationingHostSession? CommonTableRationingSession => _commonTable;

        public void SetupCommonTableRationing()
        {
            if (_commonTable != null) return;
            var saved = CommonTableRationingSaveStore.TryLoad();
            _commonTable = CommonTableRationingHostSession.Create(saved);
            _commonTable.StateChanged += () => _commonTableDirty = true;
        }

        /// <summary>Sets the active community rationing policy.</summary>
        public bool SetCommonTableRationingPolicy(RationLevel policy)
        {
            SetupCommonTableRationing();
            bool ok = _commonTable!.SetRationingPolicy(policy);
            if (ok) _commonTableDirty = true;
            return ok;
        }

        public bool SetCommonTableCookSkill(int skillPermille)
        {
            SetupCommonTableRationing();
            bool ok = _commonTable!.SetCookSkillPermille(skillPermille);
            if (ok) _commonTableDirty = true;
            return ok;
        }

        public bool SetCommonTablePopulation(int populationCount)
        {
            SetupCommonTableRationing();
            bool ok = _commonTable!.SetPopulationCount(populationCount);
            if (ok) _commonTableDirty = true;
            return ok;
        }

        public bool SetCommonTableRationInequality(bool present)
        {
            SetupCommonTableRationing();
            bool ok = _commonTable!.SetRationInequality(present);
            if (ok) _commonTableDirty = true;
            return ok;
        }

        /// <summary>
        /// Records one group meal. Categories served come from the existing food
        /// and inventory owners; the Core engine decides diversity, deficiency,
        /// grievance, and consumption outcomes.
        /// </summary>
        public NutritionEvaluationResult ServeCommonTableMeal(int day, IEnumerable<FoodCategory> categories)
        {
            SetupCommonTableRationing();
            var result = _commonTable!.ServeMeal(day, categories);
            _commonTableDirty = true;
            return result;
        }

        public int AdvanceCommonTableDay(int day)
        {
            SetupCommonTableRationing();
            int lean = _commonTable!.AdvanceDay(day);
            _commonTableDirty = true;
            return lean;
        }

        public (int Sessions, int LeanDays, int StarvationGrievance) GetCommonTableRationingReadout()
        {
            SetupCommonTableRationing();
            return (_commonTable!.SessionCount, _commonTable.ConsecutiveLeanDays,
                    _commonTable.StarvationGrievanceSessionCount);
        }

        public void SaveCommonTableRationing()
        {
            if (_commonTable == null) return;
            var state = _commonTable.CaptureState();
            if (CaptureSection(CommonTableRationingSaveStore.SectionName, CommonTableRationingSaveStore.TryCapturePersisted(state)))
                _commonTableDirty = false;
        }

        public void FlushCommonTableRationingIfDirty()
        {
            if (_commonTableDirty) SaveCommonTableRationing();
        }

        public void ResetCommonTableRationing()
        {
            _commonTable = null;
            _commonTableDirty = false;
        }
    }
}
