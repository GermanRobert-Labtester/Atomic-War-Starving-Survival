// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class SurvivorBodyPresentationSlateTests
    {
        [Fact]
        public void Project_DefaultIntact_FullGrip_1000Mobility_NoAlerts()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            var slate = SurvivorBodyPresentationSlate.Project("survivor_01", body);

            Assert.Equal("survivor_01", slate.SurvivorId);
            Assert.Equal(2, slate.EffectiveHands);
            Assert.Equal("Full Two-Hand Grip", slate.GripCapability);
            Assert.Equal(1000, slate.OverallMobilityPermille);
            Assert.False(slate.HasPhantomPain);
            Assert.Empty(slate.PhantomPainAlert);
            Assert.Equal(0, slate.MaintenanceIssuesCount);
            Assert.False(slate.RequiresImmediateService);
            Assert.Equal("Combat Ready", slate.SummaryStatus);

            Assert.Equal(4, slate.Limbs.Count);
            foreach (var limb in slate.Limbs)
            {
                Assert.Equal("Intact", limb.StatusText);
                Assert.False(limb.RequiresMaintenance);
            }
        }

        [Fact]
        public void Project_AmputatedArm_SimpleProsthetic_ReportsSimpleGrip()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.LeftArmKey, "amputated");
            body.SetLimbCondition(SurvivorBodyState.RightArmKey, "prosthetized", "item_hook_hand");

            var slate = SurvivorBodyPresentationSlate.Project(
                "survivor_02",
                body,
                prostheticConditions: new Dictionary<string, int> { { "item_hook_hand", 900 } }
            );

            Assert.Equal(1, slate.EffectiveHands);
            Assert.Equal("Simple Only", slate.GripCapability);
            Assert.Equal(1000, slate.OverallMobilityPermille);
            Assert.Contains(slate.Limbs, l => l.LimbKey == SurvivorBodyState.LeftArmKey && l.StatusText == "Amputated (No Prosthetic)");
            Assert.Contains(slate.Limbs, l => l.LimbKey == SurvivorBodyState.RightArmKey && l.GripContribution == "Simple Grip");
        }

        [Fact]
        public void Project_DegradedProstheticLeg_FlagsMaintenanceNotice_AndReducedMobility()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.RightLegKey, "prosthetized", "item_peg_leg");

            // Peg leg with condition 200 (critical, < 350)
            var slate = SurvivorBodyPresentationSlate.Project(
                "survivor_03",
                body,
                prostheticConditions: new Dictionary<string, int> { { "item_peg_leg", 200 } }
            );

            Assert.True(slate.OverallMobilityPermille < 1000);
            Assert.True(slate.RequiresImmediateService);
            Assert.True(slate.MaintenanceIssuesCount > 0);
            Assert.Equal("Operational with Restrictions", slate.SummaryStatus);

            var rightLeg = Assert.Single(slate.Limbs, l => l.LimbKey == SurvivorBodyState.RightLegKey);
            Assert.True(rightLeg.RequiresMaintenance);
            Assert.NotEmpty(rightLeg.MaintenanceNotice);
        }

        [Fact]
        public void Project_PhantomPain_RendersTruthfulAlert()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.LeftLegKey, "amputated");

            var slate = SurvivorBodyPresentationSlate.Project(
                "survivor_04",
                body,
                hasPhantomPain: true
            );

            Assert.True(slate.HasPhantomPain);
            Assert.Contains("phantom limb pain", slate.PhantomPainAlert);
            Assert.Equal("Operational with Restrictions", slate.SummaryStatus);
        }

        [Fact]
        public void Project_BilateralLegAmputation_CriticallyImpairedStatus()
        {
            var body = SurvivorBodyState.CreateDefaultIntact();
            body.SetLimbCondition(SurvivorBodyState.LeftLegKey, "amputated");
            body.SetLimbCondition(SurvivorBodyState.RightLegKey, "amputated");

            var slate = SurvivorBodyPresentationSlate.Project("survivor_05", body);

            Assert.Equal(0, slate.OverallMobilityPermille);
            Assert.Equal("Critically Impaired", slate.SummaryStatus);
        }
    }
}
