// SPDX-License-Identifier: MIT
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Plan 212 follow-up — trade rumors from real market state. The rumor
    /// text is a deterministic projection of the shock the canonical market
    /// itself applied; no RNG, no invented prices, additive kind value.
    /// </summary>
    public sealed class Plan212MarketRumorRulesTests
    {
        private static MarketShockState Shock(bool shortage) => new MarketShockState
        {
            shockId = "shock_test_1",
            categoryId = "medical",
            isShortage = shortage,
            severityBp = 1500,
            startDay = 12,
            expiryDay = 15,
            sourceId = "sanitation_crisis"
        };

        [Fact]
        public void ShortageStart_ProducesRestrainedRumorLine()
        {
            string line = EconomyMarketRumorRules.ShockStartedLine(Shock(shortage: true));
            Assert.Contains("medical", line);
            Assert.Contains("running thin", line);
            Assert.DoesNotContain("CRISIS", line);   // restrained tone
            Assert.DoesNotContain("! ", line);       // no alarmist punctuation
        }

        [Fact]
        public void SurplusStart_ProducesFloodingLine()
        {
            string line = EconomyMarketRumorRules.ShockStartedLine(Shock(shortage: false));
            Assert.Contains("flooding the counters", line);
            Assert.Contains("medical", line);
        }

        [Fact]
        public void Expiry_ProducesEasingLine()
        {
            string line = EconomyMarketRumorRules.ShockExpiredLine(Shock(shortage: true));
            Assert.Contains("squeeze has eased", line);
            Assert.Contains("medical", line);
        }

        [Fact]
        public void Deterministic_SameState_SameLine()
        {
            Assert.Equal(
                EconomyMarketRumorRules.ShockStartedLine(Shock(shortage: true)),
                EconomyMarketRumorRules.ShockStartedLine(Shock(shortage: true)));
            Assert.Equal(
                EconomyMarketRumorRules.ShockExpiredLine(Shock(shortage: false)),
                EconomyMarketRumorRules.ShockExpiredLine(Shock(shortage: false)));
        }

        [Fact]
        public void NullShock_ReturnsEmpty_Gracefully()
        {
            Assert.Empty(EconomyMarketRumorRules.ShockStartedLine(null!));
            Assert.Empty(EconomyMarketRumorRules.ShockExpiredLine(null!));
        }

        [Fact]
        public void MarketRumorKind_IsAppendOnly_AtValueSix()
        {
            // Appended after CulturalBroadcast; existing save kind ints remain valid.
            Assert.Equal(6, (int)Ashfall.Core.Radio.RadioEventKind.MarketRumor);
        }
    }
}
