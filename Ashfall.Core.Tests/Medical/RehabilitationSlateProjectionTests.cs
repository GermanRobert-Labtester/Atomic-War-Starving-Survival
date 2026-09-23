// SPDX-License-Identifier: MIT
using System;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class RehabilitationSlateProjectionTests
    {
        [Fact]
        public void Project_NullOrIntactBody_ReturnsNonePhase()
        {
            var slateNull = RehabilitationSlateProjection.Project("survivor_1", null);
            Assert.False(slateNull.HasProsthetics);
            Assert.Equal("none", slateNull.CurrentPhase);
            Assert.Equal(100f, slateNull.QualityPercent);
            Assert.Equal("No prosthetics fitted", slateNull.NextMilestone);

            var slateIntact = RehabilitationSlateProjection.Project("survivor_2", SurvivorBodyState.CreateDefaultIntact());
            Assert.False(slateIntact.HasProsthetics);
            Assert.Equal("none", slateIntact.CurrentPhase);
            Assert.Equal(100f, slateIntact.QualityPercent);
        }

        [Fact]
        public void Project_FittingPhase_RendersFittingStatus()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.LeftArmKey, "prosthetized", "item_hook_prosthetic");
            body.Rehab = new RehabRecord("hand", "fitting", 2, 500);

            var slate = RehabilitationSlateProjection.Project("survivor_3", body, hasPhantomPain: true);

            Assert.True(slate.HasProsthetics);
            Assert.Equal(1, slate.FittedProstheticsCount);
            Assert.Equal("fitting", slate.CurrentPhase);
            Assert.Equal(2, slate.DaysInPhase);
            Assert.Equal(50f, slate.QualityPercent);
            Assert.Contains("Adaptation phase in 2 days", slate.NextMilestone);
            Assert.True(slate.HasPhantomPain);
        }

        [Fact]
        public void Project_AdaptationPhase_RendersProgress()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.RightLegKey, "prosthetized", "item_peg_leg");
            body.Rehab = new RehabRecord("leg", "adaptation", 7, 750);

            var slate = RehabilitationSlateProjection.Project("survivor_4", body, adaptationDurationDays: 14);

            Assert.True(slate.HasProsthetics);
            Assert.Equal("adaptation", slate.CurrentPhase);
            Assert.Equal(7, slate.DaysInPhase);
            Assert.Equal(75f, slate.QualityPercent);
            Assert.Contains("Full mastery in 7 days", slate.NextMilestone);
        }

        [Fact]
        public void Project_MasteryPhase_RendersMastery()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.LeftArmKey, "prosthetized", "item_articulated_hand");
            body.Rehab = new RehabRecord("hand", "mastery", 10, 1000);

            var slate = RehabilitationSlateProjection.Project("survivor_5", body);

            Assert.True(slate.HasProsthetics);
            Assert.Equal("mastery", slate.CurrentPhase);
            Assert.Equal(100f, slate.QualityPercent);
            Assert.Equal("Prosthetic mastery achieved", slate.NextMilestone);
        }
    }
}
