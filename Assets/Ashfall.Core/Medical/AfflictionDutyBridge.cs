// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Plan 143 / C1[23] — typed medical overlay for the canonical duty-fitness
    /// verdict. This class owns no survivor state and never infers duties from
    /// role-name substrings; it consumes the established fitness facts and the
    /// data-authored role hazard class.
    /// </summary>
    public static class AfflictionDutyBridge
    {
        public const string ReasonInfectiousFoodHazard = "affliction_infectious_food_hazard";
        public const string ReasonRespiratoryOutdoorHazard = "affliction_respiratory_hazard";
        public const string ReasonTraumaPerimeterHazard = "affliction_trauma_perimeter_hazard";
        public const string ReasonDependencyImpairment = "affliction_dependency_impairment";
        public const string ReasonMedicalContagionRisk = "affliction_medical_contagion_risk";

        /// <summary>
        /// Tighten an already evaluated role verdict with medical-to-duty rules.
        /// The base model remains the sole fitness authority; this overlay may
        /// only add blockers/warnings or shorten the recommended shift.
        /// </summary>
        public static RoleFitnessVerdict ApplyToRoleVerdict(
            FitnessEvaluationFacts facts,
            RoleRequirements requirements,
            RoleFitnessVerdict baseVerdict)
        {
            if (facts == null) throw new ArgumentNullException(nameof(facts));
            if (requirements == null) throw new ArgumentNullException(nameof(requirements));
            if (baseVerdict == null) throw new ArgumentNullException(nameof(baseVerdict));

            var blockers = new List<string>(baseVerdict.BlockingReasons);
            var warnings = new List<string>(baseVerdict.WarningReasons);
            bool addedBlocker = false;
            bool addedWarning = false;

            if (facts.IsInfectious)
            {
                if (string.Equals(requirements.HazardClass, DutyHazardClassIds.Food, StringComparison.Ordinal))
                    addedBlocker |= Add(blockers, ReasonInfectiousFoodHazard);
                else if (string.Equals(requirements.HazardClass, DutyHazardClassIds.Medical, StringComparison.Ordinal))
                    addedBlocker |= Add(blockers, ReasonMedicalContagionRisk);
            }

            if (facts.HasCombatTrauma
                && string.Equals(requirements.HazardClass, DutyHazardClassIds.Perimeter, StringComparison.Ordinal))
                addedWarning |= Add(warnings, ReasonTraumaPerimeterHazard);

            if (facts.HasRespiratoryImpairment && IsOutdoorHazard(requirements.HazardClass))
                addedWarning |= Add(warnings, ReasonRespiratoryOutdoorHazard);

            if (facts.HasActiveWithdrawal && requirements.PrecisionWork)
                addedWarning |= Add(warnings, ReasonDependencyImpairment);

            bool allowed = baseVerdict.Allowed && !addedBlocker;
            bool warning = allowed && (baseVerdict.Warning || addedWarning);
            float hours = allowed ? baseVerdict.RecommendedMaxHours : 0f;
            if (allowed && addedWarning)
                hours = Math.Min(hours, 6f);

            return new RoleFitnessVerdict(
                baseVerdict.SurvivorId,
                baseVerdict.RoleId,
                baseVerdict.BaseVerdict,
                allowed,
                warning,
                blockers,
                warnings,
                hours);
        }

        private static bool IsOutdoorHazard(string hazardClass)
        {
            return string.Equals(hazardClass, DutyHazardClassIds.Perimeter, StringComparison.Ordinal)
                || string.Equals(hazardClass, DutyHazardClassIds.Surface, StringComparison.Ordinal)
                || string.Equals(hazardClass, DutyHazardClassIds.Airlock, StringComparison.Ordinal);
        }

        private static bool Add(List<string> values, string value)
        {
            if (values.Contains(value)) return false;
            values.Add(value);
            return true;
        }
    }
}
