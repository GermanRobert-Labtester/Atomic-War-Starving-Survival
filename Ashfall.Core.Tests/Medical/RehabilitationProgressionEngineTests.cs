// SPDX-License-Identifier: MIT
using System;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class RehabilitationProgressionEngineTests
    {
        [Fact]
        public void StartRehabilitation_InitializesFittingPhaseAt500Permille()
        {
            var rehab = RehabilitationProgressionEngine.StartRehabilitation("hand");

            Assert.Equal("hand", rehab.ProstheticTypeKey);
            Assert.Equal("fitting", rehab.Phase);
            Assert.Equal(0, rehab.DaysInPhase);
            Assert.Equal(500, rehab.QualityRampPermille);
            Assert.Equal(0.5f, RehabilitationProgressionEngine.GetQualityFactor(rehab));
        }

        [Fact]
        public void AdvanceDaily_TransitionsFromFittingToAdaptation()
        {
            var rehab = RehabilitationProgressionEngine.StartRehabilitation("hand");

            // Fitting takes 4 days by default
            rehab = RehabilitationProgressionEngine.AdvanceDaily(rehab, resilienceMultiplier: 1.0f, days: 3);
            Assert.Equal("fitting", rehab.Phase);
            Assert.Equal(3, rehab.DaysInPhase);
            Assert.Equal(500, rehab.QualityRampPermille);

            // 4th day triggers transition to adaptation
            rehab = RehabilitationProgressionEngine.AdvanceDaily(rehab, resilienceMultiplier: 1.0f, days: 1);
            Assert.Equal("adaptation", rehab.Phase);
            Assert.Equal(0, rehab.DaysInPhase);
            Assert.Equal(500, rehab.QualityRampPermille);
        }

        [Fact]
        public void AdvanceDaily_RampsQualityDuringAdaptationAndTransitionsToMastery()
        {
            var rehab = new RehabRecord("hand", "adaptation", 0, 500);

            // Advance 7 days in adaptation: quality should ramp steadily
            rehab = RehabilitationProgressionEngine.AdvanceDaily(rehab, resilienceMultiplier: 1.0f, days: 7, adaptationDurationDays: 14);
            Assert.Equal("adaptation", rehab.Phase);
            Assert.Equal(7, rehab.DaysInPhase);
            Assert.True(rehab.QualityRampPermille > 500 && rehab.QualityRampPermille < 1000);

            // Advance remaining 7 days: should reach mastery
            rehab = RehabilitationProgressionEngine.AdvanceDaily(rehab, resilienceMultiplier: 1.0f, days: 7, adaptationDurationDays: 14);
            Assert.Equal("mastery", rehab.Phase);
            Assert.Equal(1000, rehab.QualityRampPermille);
            Assert.Equal(1.0f, RehabilitationProgressionEngine.GetQualityFactor(rehab));
        }

        [Fact]
        public void AdvanceDaily_MasteryIsPermanent()
        {
            var rehab = new RehabRecord("hand", "mastery", 10, 1000);

            rehab = RehabilitationProgressionEngine.AdvanceDaily(rehab, resilienceMultiplier: 1.0f, days: 5);
            Assert.Equal("mastery", rehab.Phase);
            Assert.Equal(15, rehab.DaysInPhase);
            Assert.Equal(1000, rehab.QualityRampPermille);
        }

        [Fact]
        public void GetQualityFactor_ComputesCorrectFloats()
        {
            Assert.Equal(1.0f, RehabilitationProgressionEngine.GetQualityFactor(null));
            Assert.Equal(0.75f, RehabilitationProgressionEngine.GetQualityFactor(new RehabRecord("leg", "adaptation", 5, 750)));
        }
    }
}
