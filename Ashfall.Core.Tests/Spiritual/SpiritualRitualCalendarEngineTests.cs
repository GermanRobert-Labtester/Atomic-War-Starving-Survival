// SPDX-License-Identifier: MIT
using Ashfall.Core.Spiritual;
using Xunit;

namespace Ashfall.Core.Tests.Spiritual
{
    public sealed class SpiritualRitualCalendarEngineTests
    {
        [Fact]
        public void GetScheduledObservance_MatchingDayAndMovement_ReturnsObservance()
        {
            // Day 45 is Day of the Unforgotten for ash_witnesses
            var observance = SpiritualRitualCalendarEngine.GetScheduledObservance(campaignDay: 45, movementId: "ash_witnesses");

            Assert.NotNull(observance);
            Assert.Equal("holy_day_unforgotten", observance.Value.HolyDayId);
            Assert.Equal("Day of the Unforgotten", observance.Value.Title);
            Assert.Equal(120, observance.Value.MoraleBonusPermille);
        }

        [Fact]
        public void GetScheduledObservance_NonMatchingDay_ReturnsNull()
        {
            var observance = SpiritualRitualCalendarEngine.GetScheduledObservance(campaignDay: 150, movementId: "ash_witnesses");

            Assert.Null(observance);
        }

        [Fact]
        public void EvaluateRitualObservance_OnCooldown_FailsWithRemainingDays()
        {
            var ritual = new SpiritualRitualDefinition
            {
                Id = "ritual_roll_call",
                CooldownDays = 4,
                MoraleDelta = 2.0f
            };

            var result = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritual, daysSinceLastPerformed: 2);

            Assert.False(result.IsAllowed);
            Assert.Equal(2, result.CooldownRemainingDays);
            Assert.Contains("cooldown", result.Reason);
        }

        [Fact]
        public void EvaluateRitualObservance_LowMorale_BoostsImpact()
        {
            var ritual = new SpiritualRitualDefinition
            {
                Id = "ritual_first_sip",
                CooldownDays = 2,
                MoraleDelta = 2.0f // 200 permille base
            };

            // Shelter morale is critically low (250 < 300) -> 1.5x boost = 300 permille
            var result = SpiritualRitualCalendarEngine.EvaluateRitualObservance(
                ritual,
                daysSinceLastPerformed: 3,
                shelterMoralePermille: 250);

            Assert.True(result.IsAllowed);
            Assert.Equal(300, result.MoraleDeltaPermille);
            Assert.Equal(0, result.CooldownRemainingDays);
        }

        [Fact]
        public void CalculateIdeologicalFrictionMitigation_SharedRitual_ReducesFriction()
        {
            int mitigationWithout = SpiritualRitualCalendarEngine.CalculateIdeologicalFrictionMitigation(
                "listeners",
                "ash_witnesses",
                sharedRitualObserved: false);

            int mitigationWith = SpiritualRitualCalendarEngine.CalculateIdeologicalFrictionMitigation(
                "listeners",
                "ash_witnesses",
                sharedRitualObserved: true);

            Assert.Equal(250, mitigationWithout); // 100 base + 150 complementary
            Assert.Equal(500, mitigationWith);    // 250 + 250 shared ritual
            Assert.True(mitigationWith > mitigationWithout);
        }
    }
}
