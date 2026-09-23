// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Categorical demographic life stages for campaign survivors.
    /// </summary>
    public enum SurvivorLifeStage
    {
        Child = 0,
        YoungAdult = 1,
        Prime = 2,
        MiddleAge = 3,
        Elderly = 4
    }

    /// <summary>
    /// Pure projection snapshot of a survivor's chronological age and age-derived capabilities.
    /// Does not own or mutate survivor identity or health state.
    /// </summary>
    public sealed class SurvivorAgeProfile
    {
        public string SurvivorId { get; }
        public int EffectiveAgeYears { get; }
        public int CampaignTenureDays { get; }
        public SurvivorLifeStage Stage { get; }
        public float PhysicalLaborMultiplier { get; }
        public float FatigueAccumulationMultiplier { get; }
        public float MentorshipXpBonus { get; }
        public bool IsRetirementEligible { get; }
        public bool IsRetired { get; }
        public string Description { get; }

        public SurvivorAgeProfile(
            string survivorId,
            int effectiveAgeYears,
            int campaignTenureDays,
            SurvivorLifeStage stage,
            float physicalLaborMultiplier,
            float fatigueAccumulationMultiplier,
            float mentorshipXpBonus,
            bool isRetirementEligible,
            bool isRetired,
            string description)
        {
            SurvivorId = survivorId ?? string.Empty;
            EffectiveAgeYears = effectiveAgeYears;
            CampaignTenureDays = campaignTenureDays;
            Stage = stage;
            PhysicalLaborMultiplier = physicalLaborMultiplier;
            FatigueAccumulationMultiplier = fatigueAccumulationMultiplier;
            MentorshipXpBonus = mentorshipXpBonus;
            IsRetirementEligible = isRetirementEligible;
            IsRetired = isRetired;
            Description = description ?? string.Empty;
        }
    }

    /// <summary>
    /// Plan 176 / C1[31] — Pure domain engine for survivor aging progression,
    /// life-stage classification, physical labor capacity taper, vocational mentorship,
    /// and dignified retirement without parallel survivor entity stores.
    /// </summary>
    public static class SurvivorAgingProgressionEngine
    {
        public const int DefaultDaysPerYear = 30;
        public const int DefaultRecruitmentAgeYears = 28;
        public const int MinRetirementAgeYears = 65;

        public const float RetiredFatigueReduction = 0.30f;
        public const float RetiredLightDutyMultiplier = 0.75f;
        public const float ElderMentorshipPresenceBonus = 0.25f;

        /// <summary>
        /// Calculates effective chronological age in years from joinedDay and currentDay.
        /// Pure function; deterministic.
        /// </summary>
        public static int EvaluateAgeYears(
            int joinedDay,
            int currentDay,
            int baseAgeYears = DefaultRecruitmentAgeYears,
            int daysPerYear = DefaultDaysPerYear)
        {
            int tenure = SurvivorLifecycle.CampaignAgeDays(joinedDay, currentDay);
            int rate = daysPerYear > 0 ? daysPerYear : DefaultDaysPerYear;
            int yearsAdded = tenure / rate;
            return Math.Max(0, baseAgeYears + yearsAdded);
        }

        /// <summary>
        /// Maps age in years to canonical life stage.
        /// </summary>
        public static SurvivorLifeStage EvaluateStage(int ageYears)
        {
            if (ageYears < 18) return SurvivorLifeStage.Child;
            if (ageYears <= 30) return SurvivorLifeStage.YoungAdult;
            if (ageYears <= 50) return SurvivorLifeStage.Prime;
            if (ageYears <= 65) return SurvivorLifeStage.MiddleAge;
            return SurvivorLifeStage.Elderly;
        }

        /// <summary>
        /// Calculates complete age profile and mechanical modifiers for a survivor.
        /// </summary>
        public static SurvivorAgeProfile CalculateProfile(
            string survivorId,
            int joinedDay,
            int currentDay,
            int baseAgeYears = DefaultRecruitmentAgeYears,
            bool isRetired = false,
            int daysPerYear = DefaultDaysPerYear)
        {
            int tenureDays = SurvivorLifecycle.CampaignAgeDays(joinedDay, currentDay);
            int ageYears = EvaluateAgeYears(joinedDay, currentDay, baseAgeYears, daysPerYear);
            var stage = EvaluateStage(ageYears);

            float laborMult = 1.0f;
            float fatigueMult = 1.0f;
            float mentorBonus = 0.0f;
            bool retirementEligible = ageYears >= MinRetirementAgeYears;
            string desc;

            switch (stage)
            {
                case SurvivorLifeStage.Child:
                    laborMult = 0.50f;
                    fatigueMult = 1.10f;
                    mentorBonus = 0.0f;
                    desc = "Child dependent growing up under shelter protection.";
                    break;

                case SurvivorLifeStage.YoungAdult:
                    laborMult = 1.05f;
                    fatigueMult = 0.95f;
                    mentorBonus = 0.0f;
                    desc = "Peak physical vigor and rapid reaction, eager to learn vocational crafts.";
                    break;

                case SurvivorLifeStage.Prime:
                    laborMult = 1.00f;
                    fatigueMult = 1.00f;
                    mentorBonus = 0.10f;
                    desc = "Mature vocational competence, balanced stamina and dependable labor throughput.";
                    break;

                case SurvivorLifeStage.MiddleAge:
                    laborMult = 0.95f;
                    fatigueMult = 1.05f;
                    mentorBonus = 0.20f;
                    desc = "Deep craft experience and practical wisdom, with gradual physical endurance taper.";
                    break;

                case SurvivorLifeStage.Elderly:
                default:
                    laborMult = isRetired ? RetiredLightDutyMultiplier : 0.85f;
                    fatigueMult = isRetired ? (1.15f * (1f - RetiredFatigueReduction)) : 1.15f;
                    mentorBonus = ElderMentorshipPresenceBonus;
                    desc = isRetired
                        ? "Retired shelter elder providing wisdom, counsel, and vocational mentorship on light duty."
                        : "Venerable survivor with invaluable trade wisdom and mentoring capacity; eligible for dignified retirement.";
                    break;
            }

            return new SurvivorAgeProfile(
                survivorId,
                ageYears,
                tenureDays,
                stage,
                laborMult,
                fatigueMult,
                mentorBonus,
                retirementEligible,
                isRetired,
                desc);
        }

        /// <summary>
        /// Evaluates vocational learning acceleration multiplier for apprentices
        /// when an elder mentor is active in the shelter.
        /// </summary>
        public static float CalculateApprenticeLearningMultiplier(bool hasElderMentorPresent)
        {
            return hasElderMentorPresent ? (1.0f + ElderMentorshipPresenceBonus) : 1.0f;
        }

        /// <summary>
        /// Checks whether any living elder is present in the shelter roster to provide mentorship.
        /// </summary>
        public static bool HasLivingElderInShelter(IEnumerable<SurvivorAgeProfile>? partyProfiles)
        {
            if (partyProfiles == null) return false;
            foreach (var p in partyProfiles)
            {
                if (p != null && p.Stage == SurvivorLifeStage.Elderly)
                    return true;
            }
            return false;
        }
    }
}
