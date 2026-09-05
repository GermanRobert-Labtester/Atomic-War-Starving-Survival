using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class LeadershipAffinityMaturationTests
    {
        [Fact]
        public void Leadership_Designation_CrisisStress_BreakRisk_AndStepDown()
        {
            var leadership = new LeadershipSystem();
            leadership.GetAliveSurvivorIds = () => new[] { "leader_candidate", "survivor_b" };

            string designated = null;
            leadership.OnLeaderDesignated += id => designated = id;

            bool breakRiskFired = false;
            leadership.OnLeaderBreakRisk += id => breakRiskFired = true;

            // 1. Designate leader
            Assert.True(leadership.DesignateLeader("leader_candidate"));
            Assert.Equal("leader_candidate", leadership.CurrentLeaderId);
            Assert.Equal("leader_candidate", designated);
            Assert.True(leadership.IsDesignatedLeader("leader_candidate"));

            // 2. Crisis events increase leader stress
            leadership.OnCrisisEvent(20f);
            Assert.True(leadership.GetLeaderStress("leader_candidate") >= 20f);

            // Trigger multiple crises to push stress to max (100)
            leadership.OnCrisisEvent(30f);
            leadership.OnCrisisEvent(30f);
            leadership.OnCrisisEvent(30f);

            Assert.True(breakRiskFired, "Expected break risk event to fire when leader stress reaches max");
            Assert.Equal(100f, leadership.GetLeaderStress("leader_candidate"));

            // 3. Step down and cooldown
            Assert.True(leadership.StepDown("leader_candidate"));
            Assert.Null(leadership.CurrentLeaderId);
            Assert.True(leadership.StepDownCooldown > 0f);

            // Cannot immediately designate while on cooldown
            Assert.False(leadership.DesignateLeader("survivor_b"));

            // After ticking down cooldown (Tick expects gameHours)
            leadership.Tick((leadership.StepDownCooldown + 1f) * 24f);
            Assert.True(leadership.DesignateLeader("survivor_b"));
            Assert.Equal("survivor_b", leadership.CurrentLeaderId);
        }

        [Fact]
        public void SurvivorRelations_EffectOf_ReturnsBoundedNetScore_AndModulatesCaregiving()
        {
            var rng = new SeededRng(42);
            var relations = new SurvivorRelationsSystem(rng);

            // Unknown or self returns 0
            Assert.Equal(0f, relations.EffectOf("unknown_a", "unknown_b"));
            Assert.Equal(0f, relations.EffectOf("survivor_a", "survivor_a"));

            // Positive relationship (high affinity + trust)
            relations.ModifyAffinity("survivor_a", "survivor_b", 60f);
            relations.ModifyTrust("survivor_a", "survivor_b", 40f);
            float positiveEffect = relations.EffectOf("survivor_a", "survivor_b");
            Assert.True(positiveEffect > 0.4f && positiveEffect <= 1.0f);

            // Negative relationship (resentment + grief)
            relations.ModifyAffinity("survivor_c", "survivor_d", -80f);
            relations.ApplyGrief("survivor_d", 50f);
            float negativeEffect = relations.EffectOf("survivor_c", "survivor_d");
            Assert.True(negativeEffect < -0.4f && negativeEffect >= -1.0f);

            // Verify CaregivingSystem consumes GetRelationshipEffect
            float recoveryBonusApplied = 0f;
            var caregiving = new CaregivingSystem();
            caregiving.ApplyHealthRecoveryBonus = (patientId, amount) => recoveryBonusApplied += amount;
            caregiving.GetRelationshipEffect = (c, p) => relations.EffectOf(c, p);

            // Case 1: Caring for positive partner
            caregiving.AssignCaregiver("survivor_a", "survivor_b");
            caregiving.Tick(24f); // 1 full day
            float positiveBonus = recoveryBonusApplied;

            // Case 2: Caring for negative partner
            caregiving.UnassignCaregiver("survivor_b");
            recoveryBonusApplied = 0f;
            caregiving.AssignCaregiver("survivor_c", "survivor_d");
            caregiving.Tick(24f);
            float negativeBonus = recoveryBonusApplied;

            Assert.True(positiveBonus > negativeBonus,
                $"Expected positive relationship recovery ({positiveBonus}) to exceed negative relationship recovery ({negativeBonus})");
        }

        [Fact]
        public void Cohort_Maturation_AdvancesWithCampaignCalendar_OnceAndIdempotent()
        {
            var cohort = new CohortSystem();

            // Book two children on day 10
            Assert.True(cohort.BookChild("child_alice", new[] { "parent_a" }, "medium", birthDay: 10));
            Assert.True(cohort.BookChild("child_bob", new[] { "parent_b" }, "low", birthDay: 100));

            string lastMaturedChild = null;
            int lastMaturationDay = 0;
            cohort.OnMaturation += (id, day) =>
            {
                lastMaturedChild = id;
                lastMaturationDay = day;
            };

            // Test manual TryMaturation
            Assert.False(cohort.TryMaturation("child_alice", day: 0), "Invalid day should fail");
            Assert.False(cohort.TryMaturation("nonexistent_child", day: 50), "Unknown child should fail");

            // Maturation threshold: 365 days.
            // Day 200: Alice is 190 days old (< 365), Bob is 100 days old (< 365)
            int maturedAt200 = 0;
            foreach (var c in cohort.Children)
            {
                if (!c.isMatured && 200 >= c.birthDay + 365)
                {
                    if (cohort.TryMaturation(c.survivorId, 200)) maturedAt200++;
                }
            }
            Assert.Equal(0, maturedAt200);
            Assert.Null(lastMaturedChild);

            // Day 380: Alice is 370 days old (>= 365), Bob is 280 days old (< 365)
            int maturedAt380 = 0;
            foreach (var c in cohort.Children)
            {
                if (!c.isMatured && 380 >= c.birthDay + 365)
                {
                    if (cohort.TryMaturation(c.survivorId, 380)) maturedAt380++;
                }
            }
            Assert.Equal(1, maturedAt380);
            Assert.Equal("child_alice", lastMaturedChild);
            Assert.Equal(380, lastMaturationDay);
            Assert.True(cohort.GetChild("child_alice")?.isMatured);
            Assert.False(cohort.GetChild("child_bob")?.isMatured);

            // Re-attempting maturation on Alice returns false (idempotent, one-way)
            Assert.False(cohort.TryMaturation("child_alice", 381));
        }
    }
}
