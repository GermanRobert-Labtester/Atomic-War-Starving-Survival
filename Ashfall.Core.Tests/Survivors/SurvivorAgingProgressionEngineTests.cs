// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class SurvivorAgingProgressionEngineTests
    {
        [Fact]
        public void Age_advances_deterministically_from_tenure()
        {
            // Joined day 0, evaluated at day 0 -> 28
            Assert.Equal(28, SurvivorAgingProgressionEngine.EvaluateAgeYears(joinedDay: 0, currentDay: 0, baseAgeYears: 28, daysPerYear: 30));

            // Evaluated at day 60 (2 years elapsed) -> 30
            Assert.Equal(30, SurvivorAgingProgressionEngine.EvaluateAgeYears(joinedDay: 0, currentDay: 60, baseAgeYears: 28, daysPerYear: 30));

            // Joined day 30, evaluated at day 90 (2 years elapsed) -> 30
            Assert.Equal(30, SurvivorAgingProgressionEngine.EvaluateAgeYears(joinedDay: 30, currentDay: 90, baseAgeYears: 28, daysPerYear: 30));

            // Future join day clamps to base age
            Assert.Equal(28, SurvivorAgingProgressionEngine.EvaluateAgeYears(joinedDay: 50, currentDay: 10, baseAgeYears: 28, daysPerYear: 30));
        }

        [Fact]
        public void Life_stage_classifications_match_demographic_ranges()
        {
            Assert.Equal(SurvivorLifeStage.Child, SurvivorAgingProgressionEngine.EvaluateStage(12));
            Assert.Equal(SurvivorLifeStage.YoungAdult, SurvivorAgingProgressionEngine.EvaluateStage(25));
            Assert.Equal(SurvivorLifeStage.Prime, SurvivorAgingProgressionEngine.EvaluateStage(42));
            Assert.Equal(SurvivorLifeStage.MiddleAge, SurvivorAgingProgressionEngine.EvaluateStage(58));
            Assert.Equal(SurvivorLifeStage.Elderly, SurvivorAgingProgressionEngine.EvaluateStage(68));
        }

        [Fact]
        public void Prime_and_young_adult_modifiers_match_specification()
        {
            var young = SurvivorAgingProgressionEngine.CalculateProfile("sv_young", joinedDay: 0, currentDay: 0, baseAgeYears: 22);
            Assert.Equal(SurvivorLifeStage.YoungAdult, young.Stage);
            Assert.Equal(1.05f, young.PhysicalLaborMultiplier);
            Assert.Equal(0.95f, young.FatigueAccumulationMultiplier);
            Assert.Equal(0.0f, young.MentorshipXpBonus);
            Assert.False(young.IsRetirementEligible);

            var prime = SurvivorAgingProgressionEngine.CalculateProfile("sv_prime", joinedDay: 0, currentDay: 0, baseAgeYears: 38);
            Assert.Equal(SurvivorLifeStage.Prime, prime.Stage);
            Assert.Equal(1.00f, prime.PhysicalLaborMultiplier);
            Assert.Equal(1.00f, prime.FatigueAccumulationMultiplier);
            Assert.Equal(0.10f, prime.MentorshipXpBonus);
            Assert.False(prime.IsRetirementEligible);
        }

        [Fact]
        public void Elderly_profile_grants_mentorship_and_retirement_eligibility()
        {
            var elder = SurvivorAgingProgressionEngine.CalculateProfile("sv_elder", joinedDay: 0, currentDay: 0, baseAgeYears: 68, isRetired: false);
            Assert.Equal(SurvivorLifeStage.Elderly, elder.Stage);
            Assert.Equal(0.85f, elder.PhysicalLaborMultiplier);
            Assert.Equal(1.15f, elder.FatigueAccumulationMultiplier);
            Assert.Equal(0.25f, elder.MentorshipXpBonus);
            Assert.True(elder.IsRetirementEligible);
            Assert.False(elder.IsRetired);

            // Retired elder
            var retired = SurvivorAgingProgressionEngine.CalculateProfile("sv_elder_ret", joinedDay: 0, currentDay: 0, baseAgeYears: 68, isRetired: true);
            Assert.True(retired.IsRetired);
            Assert.Equal(0.75f, retired.PhysicalLaborMultiplier);
            // Fatigue reduced by 30%
            Assert.True(retired.FatigueAccumulationMultiplier < elder.FatigueAccumulationMultiplier);
        }

        [Fact]
        public void Mentorship_presence_accelerates_apprentice_learning()
        {
            Assert.Equal(1.00f, SurvivorAgingProgressionEngine.CalculateApprenticeLearningMultiplier(hasElderMentorPresent: false));
            Assert.Equal(1.25f, SurvivorAgingProgressionEngine.CalculateApprenticeLearningMultiplier(hasElderMentorPresent: true));

            var party = new List<SurvivorAgeProfile>
            {
                SurvivorAgingProgressionEngine.CalculateProfile("sv1", 0, 0, 25),
                SurvivorAgingProgressionEngine.CalculateProfile("sv2", 0, 0, 40),
                SurvivorAgingProgressionEngine.CalculateProfile("sv3", 0, 0, 70)
            };

            Assert.True(SurvivorAgingProgressionEngine.HasLivingElderInShelter(party));
        }
    }
}
