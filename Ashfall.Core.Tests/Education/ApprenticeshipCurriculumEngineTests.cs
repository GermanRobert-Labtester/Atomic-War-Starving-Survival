// SPDX-License-Identifier: MIT
// Expansion 28 — The Lesson : ApprenticeshipCurriculumEngine focused tests
using System;
using Xunit;
using Ashfall.Core.Education;

namespace Ashfall.Core.Tests.Education
{
    public sealed class ApprenticeshipCurriculumEngineTests
    {
        // ── 1. A literacy session yields comprehension and advances tier when threshold met ──
        [Fact]
        public void AdvanceLiteracySession_AdvancesTier_WhenComprehensionReachesThreshold()
        {
            var learner = new LearnerRecord
            {
                SurvivorId            = "learner-01",
                LiteracyLevel         = LiteracyLevel.Illiterate,
                ComprehensionPermille = 950  // near threshold
            };

            // High teacher skill + full library; should push over 1000 and advance
            var result = ApprenticeshipCurriculumEngine.AdvanceLiteracySession(
                learner, teacherSkillPermille: 1000, manualAvailabilityPermille: 1000, sessionSeed: 42);

            Assert.True(result.AdvancedLiteracyTier,
                "Should have advanced literacy tier");
            Assert.Equal(LiteracyLevel.Basic, result.NewLiteracyLevel);
            Assert.True(learner.ComprehensionPermille < ApprenticeshipCurriculumEngine.TierAdvanceThreshold,
                "Comprehension should carry-over, not exceed threshold");
        }

        // ── 2. Fatigue penalty degrades comprehension yield after FatigueOnsetSessions ──
        [Fact]
        public void AdvanceLiteracySession_FatiguePenalty_AppliesAfterOnsetSessions()
        {
            var learner = new LearnerRecord
            {
                SurvivorId            = "learner-02",
                LiteracyLevel         = LiteracyLevel.Basic,
                ComprehensionPermille = 0,
                FatigueSessionCount   = ApprenticeshipCurriculumEngine.FatigueOnsetSessions + 2
            };

            var result = ApprenticeshipCurriculumEngine.AdvanceLiteracySession(
                learner, 800, 800, sessionSeed: 99);

            Assert.True(result.FatiguePenaltyPermille > 0,
                "Fatigue penalty should be positive after onset threshold");
        }

        // ── 3. Vocational certification requires Functional literacy + high trade skill ──
        [Fact]
        public void EvaluateVocationalCertification_ReturnsFalse_WhenLiteracyBelowFunctional()
        {
            var learner = new LearnerRecord
            {
                SurvivorId    = "learner-03",
                LiteracyLevel = LiteracyLevel.Basic
            };
            learner.TradeSkillPermille["carpentry"] = 950;

            bool certified = ApprenticeshipCurriculumEngine.EvaluateVocationalCertification(
                learner, "carpentry");

            Assert.False(certified, "Basic literacy should not qualify for certification");
        }

        // ── 4. Vocational certification succeeds when literacy Functional and skill ≥ threshold ──
        [Fact]
        public void EvaluateVocationalCertification_ReturnsTrue_WhenQualified()
        {
            var learner = new LearnerRecord
            {
                SurvivorId    = "learner-04",
                LiteracyLevel = LiteracyLevel.Functional
            };
            learner.TradeSkillPermille["blacksmithing"] = 950; // above 900 threshold

            bool certified = ApprenticeshipCurriculumEngine.EvaluateVocationalCertification(
                learner, "blacksmithing");

            Assert.True(certified, "Functional literacy + high skill should certify");
        }

        // ── 5. Transcription yield is 0 for non-Scholarly, positive for Scholarly ──
        [Fact]
        public void CalculateManualTranscriptionYield_ZeroForNonScholarly_PositiveForScholarly()
        {
            int illiterateYield = ApprenticeshipCurriculumEngine.CalculateManualTranscriptionYield(
                LiteracyLevel.Illiterate, 8);
            int scholarlyYield  = ApprenticeshipCurriculumEngine.CalculateManualTranscriptionYield(
                LiteracyLevel.Scholarly, 8);

            Assert.Equal(0, illiterateYield);
            Assert.True(scholarlyYield > 0,
                $"Scholarly survivor should transcribe pages; got {scholarlyYield}");
        }
    }
}
