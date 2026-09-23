// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Generations;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Generations
{
    public sealed class SecondGenerationMilestoneEngineTests
    {
        [Fact]
        public void EvaluateNextMilestone_Infant_RequiresToddlerForFirstWords()
        {
            var profile = new ChildProfile
            {
                ChildId = "child_01",
                Stage = DevelopmentStage.Infant,
                Milestones = new List<string>()
            };

            var result = SecondGenerationMilestoneEngine.EvaluateNextMilestone(profile, currentDay: 5);

            Assert.False(result.Eligible);
            Assert.Equal(MilestoneKind.FirstWords, result.Milestone);
            Assert.Equal(DevelopmentStage.Toddler, result.RequiredStage);
        }

        [Fact]
        public void EvaluateNextMilestone_ChildWithLowEducation_NotEligibleForFoundationalLetters()
        {
            var profile = new ChildProfile
            {
                ChildId = "child_02",
                Stage = DevelopmentStage.Child,
                EducationScore = 5f,
                Milestones = new List<string> { "first_words" }
            };

            var result = SecondGenerationMilestoneEngine.EvaluateNextMilestone(profile, currentDay: 100);

            Assert.False(result.Eligible);
            Assert.Equal(MilestoneKind.FoundationalLetters, result.Milestone);
            Assert.Equal(10.0f, result.RequiredEducation);
        }

        [Fact]
        public void EvaluateNextMilestone_ChildWithAdequateEducation_EligibleForFoundationalLetters()
        {
            var profile = new ChildProfile
            {
                ChildId = "child_03",
                Stage = DevelopmentStage.Child,
                EducationScore = 15f,
                Milestones = new List<string> { "first_words" }
            };

            var result = SecondGenerationMilestoneEngine.EvaluateNextMilestone(profile, currentDay: 120);

            Assert.True(result.Eligible);
            Assert.Equal(MilestoneKind.FoundationalLetters, result.Milestone);
            Assert.Equal(100, result.AptitudeBonusPermille);
        }

        [Fact]
        public void CalculateSuccessionReadiness_AdolescentWithDeceasedParent_ScoresAppropriately()
        {
            var profile = new ChildProfile
            {
                ChildId = "child_04",
                Stage = DevelopmentStage.Adolescent,
                EducationScore = 50f,
                Milestones = new List<string> { "first_words", "foundational_letters", "tool_handling" }
            };

            int readiness = SecondGenerationMilestoneEngine.CalculateSuccessionReadinessPermille(
                profile,
                parentDeceased: true,
                parentKinshipPermille: 1000);

            // Base 500 + ed(75) + milestones(60) + deceased(50) = 685 permille
            Assert.InRange(readiness, 650, 750);
        }

        [Fact]
        public void CalculateAptitudeModifiers_WithCraftMilestones_IncreasesCraftAptitude()
        {
            var profile = new ChildProfile
            {
                ChildId = "child_05",
                Milestones = new List<string> { "tool_handling", "vocational_apprenticeship" }
            };

            var aptitudes = SecondGenerationMilestoneEngine.CalculateAptitudeModifiers(profile);

            Assert.Equal(750, aptitudes["craft"]); // 500 base + 150 + 100
            Assert.Equal(600, aptitudes["medicine"]); // 500 base + 100
            Assert.Equal(500, aptitudes["scavenge"]); // 500 base
        }
    }
}
