// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class SurgicalGraftRejectionEngineTests
    {
        [Fact]
        public void PerformGraft_InitializesWithTierCompatibility()
        {
            var engine = new SurgicalGraftRejectionEngine();

            var autoGraft = engine.PerformGraft("survivor_1", "left_arm", "self_salvage", GraftBiocompatibilityTier.Autograft, currentDay: 1);
            var xenoGraft = engine.PerformGraft("survivor_2", "right_leg", "synth_matrix", GraftBiocompatibilityTier.XenograftSynthetic, currentDay: 1);

            Assert.NotNull(autoGraft);
            Assert.Equal(GraftStatus.Integrating, autoGraft.Status);
            Assert.Equal(50, autoGraft.RejectionRiskPermille); // 1000 - 950 = 50 permille (5%)

            Assert.NotNull(xenoGraft);
            Assert.Equal(GraftStatus.Integrating, xenoGraft.Status);
            Assert.Equal(650, xenoGraft.RejectionRiskPermille); // 1000 - 350 = 650 permille (65%)
        }

        [Fact]
        public void IntegrationProgress_AdvancesWithAdequateImmunosuppression()
        {
            var engine = new SurgicalGraftRejectionEngine();
            var graft = engine.PerformGraft("survivor_1", "left_arm", "donor_a", GraftBiocompatibilityTier.AllograftMatched, currentDay: 1);

            SurgicalGraftRecord? integratedRecord = null;
            engine.OnGraftIntegrated += g => integratedRecord = g;

            // Administer high dose to keep immunosuppression well above threshold
            engine.AdministerImmunosuppressant(graft.GraftId, 800, currentDay: 1);

            // Advance 20 days, maintaining immunosuppression
            for (int day = 2; day <= 21; day++)
            {
                engine.AdministerImmunosuppressant(graft.GraftId, 300, currentDay: day);
                engine.ProcessDailyTick(day);
            }

            Assert.Equal(GraftStatus.FullyIntegrated, graft.Status);
            Assert.Equal(1000, graft.IntegrationProgressPermille);
            Assert.Equal(0, graft.RejectionRiskPermille);
            Assert.NotNull(integratedRecord);
            Assert.Equal(graft.GraftId, integratedRecord.GraftId);
        }

        [Fact]
        public void LowImmunosuppression_EscalatesRejectionRiskAndTriggersRejection()
        {
            var engine = new SurgicalGraftRejectionEngine();
            // AllograftUnmatched: base compatibility 500 permille, initial risk 500 permille
            var graft = engine.PerformGraft("survivor_1", "right_arm", "generic_donor", GraftBiocompatibilityTier.AllograftUnmatched, currentDay: 1);

            SurgicalGraftRecord? rejectedRecord = null;
            engine.OnGraftRejected += g => rejectedRecord = g;

            // Day 2 (decay 200 -> level 300, above threshold 250)
            engine.ProcessDailyTick(2);
            Assert.Equal(GraftStatus.Integrating, graft.Status);

            // Day 3 (decay 200 -> level 100, below threshold 250) -> DaysWithoutImmunosuppressant becomes 1, risk increases
            // Seeded roll returns 300 (which is < risk ~570) -> triggers rejection!
            engine.ProcessDailyTick(3, seededRngRoll: (id, day) => 300);

            Assert.Equal(GraftStatus.Rejected, graft.Status);
            Assert.NotNull(rejectedRecord);
            Assert.Equal(graft.GraftId, rejectedRecord.GraftId);
        }

        [Fact]
        public void AdministerImmunosuppressant_ReplenishesLevelsAndHaltsRejectionEscalation()
        {
            var engine = new SurgicalGraftRejectionEngine();
            var graft = engine.PerformGraft("survivor_1", "left_leg", "donor_x", GraftBiocompatibilityTier.AllograftMatched, currentDay: 1);

            // Let it decay
            engine.ProcessDailyTick(3); // Level drops to 100
            Assert.True(graft.ImmunosuppressantLevelPermille < SurgicalGraftRejectionEngine.ImmunosuppressantThresholdPermille);

            // Administer dose of 500 permille
            bool administered = engine.AdministerImmunosuppressant(graft.GraftId, 500, currentDay: 3);
            Assert.True(administered);
            Assert.Equal(600, graft.ImmunosuppressantLevelPermille); // 100 + 500
            Assert.Equal(0, graft.DaysWithoutImmunosuppressant);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesAllGraftStates()
        {
            var engine1 = new SurgicalGraftRejectionEngine();
            var g1 = engine1.PerformGraft("survivor_a", "left_arm", "donor_1", GraftBiocompatibilityTier.Autograft, currentDay: 5);
            var g2 = engine1.PerformGraft("survivor_b", "right_leg", "donor_2", GraftBiocompatibilityTier.AllograftMatched, currentDay: 5);

            engine1.ProcessDailyTick(7);

            var save = engine1.CaptureState();
            Assert.NotNull(save);
            Assert.Equal(2, save.Grafts.Count);

            var engine2 = new SurgicalGraftRejectionEngine();
            engine2.RestoreState(save);

            Assert.Equal(2, engine2.Grafts.Count);
            var restored = engine2.GetGraft(g1.GraftId);
            Assert.NotNull(restored);
            Assert.Equal(g1.GraftId, restored.GraftId);
            Assert.Equal(g1.SurvivorId, restored.SurvivorId);
            Assert.Equal(g1.Tier, restored.Tier);
            Assert.Equal(g1.Status, restored.Status);
            Assert.Equal(g1.IntegrationProgressPermille, restored.IntegrationProgressPermille);
        }
    }
}
