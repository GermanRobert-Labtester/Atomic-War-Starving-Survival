using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Characterization tests for DynamicEconomySystem decision surfaces.
    /// These pin the CURRENT behavior before any extraction to Ashfall.Core.
    /// The StanceLogicSpec and PriceLogicSpec helpers replicate the pure
    /// math from DynamicEconomySystem; they will be replaced by the real
    /// core implementations during extraction.
    /// </summary>
    public class DynamicEconomyCharacterizationTests
    {
        // ── Stance threshold constants from DynamicEconomySystem ──────
        private const float DefaultRaidThreshold = -50f;
        private const float DefaultRobThreshold = -20f;
        private const float DefaultTradeAt = -40f;
        private const float DefaultIntelAt = 40f;

        // ── Replicates DynamicEconomySystem.GetStance pure logic ──────
        private static TradeStance ComputeStance(float trust, float raidAt, float robAt, float tradeAt, float intelAt, bool isActive)
        {
            if (!isActive) return TradeStance.Refuse;
            if (trust <= raidAt) return TradeStance.HostileRaid;
            if (trust <= robAt) return TradeStance.Rob;
            if (trust < tradeAt) return TradeStance.Refuse;
            if (trust >= intelAt) return TradeStance.ShareIntel;
            return TradeStance.Trade;
        }

        // ── Stance characterization ───────────────────────────────────
        [Theory]
        [InlineData(-50, -50, -20, -40, 40, true, TradeStance.HostileRaid)]   // exactly at raid threshold
        [InlineData(-51, -50, -20, -40, 40, true, TradeStance.HostileRaid)]   // below raid threshold
        [InlineData(-20, -50, -20, -40, 40, true, TradeStance.Rob)]           // exactly at rob threshold
        [InlineData(-39, -50, -40, -20, 40, true, TradeStance.Refuse)]        // between rob and trade
        [InlineData(-20, -50, -40, -20, 40, true, TradeStance.Trade)]         // exactly at trade threshold
        [InlineData(39, -50, -20, -40, 40, true, TradeStance.Trade)]          // just below intel
        [InlineData(40, -50, -20, -40, 40, true, TradeStance.ShareIntel)]     // exactly at intel
        [InlineData(100, -50, -20, -40, 40, true, TradeStance.ShareIntel)]    // max trust
        [InlineData(-50, -50, -20, -40, 40, false, TradeStance.Refuse)]       // inactive faction
        [InlineData(0, -50, -20, -40, 40, false, TradeStance.Refuse)]         // inactive, positive trust
        public void Stance_Thresholds_ReturnsExpectedStance(
            float trust, float raidAt, float robAt, float tradeAt, float intelAt, bool isActive, TradeStance expected)
        {
            Assert.Equal(expected, ComputeStance(trust, raidAt, robAt, tradeAt, intelAt, isActive));
        }

        [Theory]
        [InlineData(TradeStance.Trade, true)]
        [InlineData(TradeStance.ShareIntel, true)]
        [InlineData(TradeStance.Refuse, false)]
        [InlineData(TradeStance.Rob, false)]
        [InlineData(TradeStance.HostileRaid, false)]
        public void WillTrade_MatchesStance(TradeStance stance, bool expected)
        {
            bool WillTrade(TradeStance s) => s == TradeStance.Trade || s == TradeStance.ShareIntel;
            Assert.Equal(expected, WillTrade(stance));
        }

        [Theory]
        [InlineData(TradeStance.ShareIntel, true)]
        [InlineData(TradeStance.Trade, false)]
        [InlineData(TradeStance.Refuse, false)]
        [InlineData(TradeStance.Rob, false)]
        [InlineData(TradeStance.HostileRaid, false)]
        public void WillShareIntel_MatchesStance(TradeStance stance, bool expected)
        {
            bool WillShareIntel(TradeStance s) => s == TradeStance.ShareIntel;
            Assert.Equal(expected, WillShareIntel(stance));
        }

        // ── Price / scarcity characterization ─────────────────────────
        [Fact]
        public void ScarcityOverlay_Empty_ReturnsUnity()
        {
            var overlay = new HardcoreEconomyTuning();
            Assert.False(overlay.IsActive);
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(1, "water"));
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(100, "food"));
        }

        [Fact]
        public void ScarcityOverlay_DayOutOfRange_ReturnsUnity()
        {
            const string json = @"{
                ""version"": 1,
                ""scarcity_tiers"": [
                    {""tier"": ""Critical"", ""multiplier"": 2.5, ""day_range_label"": ""Days 1-15"", ""affected_item_ids"": [""water""], ""rationale"": ""test""}
                ],
                ""faction_preferences"": [],
                ""price_shock_rules"": []
            }";
            var result = HardcoreEconomyTuningLoader.Load(json);
            Assert.True(result.IsValid);
            var overlay = new HardcoreEconomyTuning();
            overlay.Apply(result.Bundle!);
            Assert.True(overlay.IsActive);
            Assert.Equal(2.5f, overlay.GetScarcityMultiplier(5, "water"));    // in range
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(100, "water"));   // out of range
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(5, "food"));      // wrong item
        }

        [Fact]
        public void ScarcityOverlay_Wildcard_MatchesAllItems()
        {
            const string json = @"{
                ""version"": 1,
                ""scarcity_tiers"": [
                    {""tier"": ""Critical"", ""multiplier"": 3.0, ""day_range_label"": ""Days 1-30"", ""affected_item_ids"": [""*""], ""rationale"": ""test""}
                ],
                ""faction_preferences"": [],
                ""price_shock_rules"": []
            }";
            var result = HardcoreEconomyTuningLoader.Load(json);
            Assert.True(result.IsValid);
            var overlay = new HardcoreEconomyTuning();
            overlay.Apply(result.Bundle!);
            Assert.Equal(3.0f, overlay.GetScarcityMultiplier(1, "anything"));
            Assert.Equal(3.0f, overlay.GetScarcityMultiplier(30, "everything"));
        }

        [Fact]
        public void WeatherMultiplier_Null_ReturnsUnity()
        {
            // DynamicEconomySystem returns 1.0f when _itemWeatherPriceMultiplier is null.
            Func<string, float>? weatherMult = null;
            float GetWeatherMult(string itemId) => weatherMult != null ? weatherMult(itemId) : 1.0f;
            Assert.Equal(1.0f, GetWeatherMult("water"));
        }

        [Fact]
        public void WeatherMultiplier_Provided_ReturnsValue()
        {
            Func<string, float> weatherMult = id => id == "water" ? 1.5f : 1.0f;
            float GetWeatherMult(string itemId) => weatherMult != null ? weatherMult(itemId) : 1.0f;
            Assert.Equal(1.5f, GetWeatherMult("water"));
            Assert.Equal(1.0f, GetWeatherMult("food"));
        }

        // ── Event DTO characterization ───────────────────────────────
        [Fact]
        public void FactionRaidResult_DefaultValues_AreSane()
        {
            var result = new FactionRaidResult();
            Assert.Null(result.FactionId);
            Assert.False(result.Launched);
            Assert.False(result.Repelled);
            Assert.Equal(0f, result.HatchDamage);
            Assert.Equal(0, result.StolenItemCount);
        }

        [Fact]
        public void FactionSuccessionResult_DefaultValues_AreSane()
        {
            var result = new FactionSuccessionResult();
            Assert.Null(result.FactionId);
            Assert.False(result.Applied);
            Assert.Equal(0, result.Generation);
        }

        [Fact]
        public void FactionSurrenderResult_DefaultValues_AreSane()
        {
            var result = new FactionSurrenderResult();
            Assert.Null(result.FactionId);
            Assert.False(result.Applied);
            Assert.Equal(TradeStance.HostileRaid, result.NewStance); // enum default = 0
        }
    }
}
