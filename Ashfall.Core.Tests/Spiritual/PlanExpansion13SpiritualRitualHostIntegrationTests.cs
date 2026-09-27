// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Expansion 13 (The Faithful & The Fractured) — spiritual
// ritual calendar. Focused contract tests over the sealed engine and the bounded
// cooldown ledger state DTO.

using Ashfall.Core.Spiritual;
using Xunit;

namespace Ashfall.Core.Tests.Spiritual
{
    public sealed class PlanExpansion13SpiritualRitualHostIntegrationTests
    {
        private static SpiritualRitualDefinition Ritual(string id, float moraleDelta, int cooldownDays = 3) =>
            new()
            {
                Id = id,
                Title = id,
                MoraleDelta = moraleDelta,
                CooldownDays = cooldownDays,
                FrictionFlag = string.Empty
            };

        [Fact]
        public void RitualObservance_IsAllowed_AndReportsMoraleAndFriction()
        {
            var result = SpiritualRitualCalendarEngine.EvaluateRitualObservance(
                Ritual("r", 2.0f), daysSinceLastPerformed: int.MaxValue, shelterMoralePermille: 700);

            Assert.True(result.IsAllowed);
            Assert.Equal(200, result.MoraleDeltaPermille);
            Assert.Equal(50, result.FrictionReductionPermille);
            Assert.Equal(0, result.CooldownRemainingDays);
        }

        [Fact]
        public void Cooldown_IsEnforcedByTheEngine_NotByTheHost()
        {
            var ritual = Ritual("r", 1.0f, cooldownDays: 4);

            var first = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritual, int.MaxValue, 500);
            var second = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritual, 2, 500);
            var third = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritual, 4, 500);

            Assert.True(first.IsAllowed);
            Assert.False(second.IsAllowed);
            Assert.Equal(2, second.CooldownRemainingDays);
            Assert.True(third.IsAllowed);
        }

        [Fact]
        public void LowMorale_ScalesSpiritualComfort()
        {
            var ritual = Ritual("r", 2.0f);

            int normal = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritual, int.MaxValue, 700).MoraleDeltaPermille;
            int low = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritual, int.MaxValue, 250).MoraleDeltaPermille;

            Assert.Equal(300, low);
            Assert.True(low > normal);
        }

        [Fact]
        public void HolyDayWindows_AreDeterministic_AndScopedToTheMovement()
        {
            var onDay = SpiritualRitualCalendarEngine.GetScheduledObservance(45, "ash_witnesses");
            var repeat = SpiritualRitualCalendarEngine.GetScheduledObservance(45, "ash_witnesses");
            var offDay = SpiritualRitualCalendarEngine.GetScheduledObservance(200, "ash_witnesses");
            var wrongMovement = SpiritualRitualCalendarEngine.GetScheduledObservance(45, "rebuilders");

            Assert.True(onDay.HasValue);
            Assert.NotNull(repeat);
            Assert.Equal(onDay!.Value.HolyDayId, repeat!.Value.HolyDayId);
            Assert.False(offDay.HasValue);
            Assert.False(wrongMovement.HasValue);
            Assert.Equal("holy_day_unforgotten", onDay!.Value.HolyDayId);
        }

        [Fact]
        public void FrictionMitigation_IsSymmetrical_AndRewardsSharedRites()
        {
            int same = SpiritualRitualCalendarEngine.CalculateIdeologicalFrictionMitigation("ash_witnesses", "ash_witnesses", false);
            int crossPlain = SpiritualRitualCalendarEngine.CalculateIdeologicalFrictionMitigation("ash_witnesses", "rebuilders", false);
            int crossShared = SpiritualRitualCalendarEngine.CalculateIdeologicalFrictionMitigation("rebuilders", "ash_witnesses", true);

            Assert.Equal(1000, same);
            Assert.True(crossPlain < same);
            Assert.Equal(crossShared, crossPlain + 250);
        }

        [Fact]
        public void CooldownLedgerState_RoundTripsThroughSystemTextJson()
        {
            var state = new SpiritualRitualSaveState
            {
                schema_version = 1,
                LastPerformedDay = { { "r", 12 }, { "s", 30 } }
            };

            string json = new SystemTextJsonSerializer().Serialize(state);
            var restored = new SystemTextJsonSerializer().Deserialize<SpiritualRitualSaveState>(json);

            Assert.NotNull(restored);
            Assert.Equal(2, restored!.LastPerformedDay.Count);
            Assert.Equal(12, restored.LastPerformedDay["r"]);
            Assert.Equal(30, restored.LastPerformedDay["s"]);
        }

        [Fact]
        public void MissingCooldownLedger_RestoresAsNeverPerformed()
        {
            var restored = new SystemTextJsonSerializer().Deserialize<SpiritualRitualSaveState>("{}");

            Assert.NotNull(restored);
            Assert.Empty(restored!.LastPerformedDay);

            var ritual = Ritual("r", 1.0f);
            var result = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritual, int.MaxValue, 500);
            Assert.True(result.IsAllowed);
        }
    }
}
