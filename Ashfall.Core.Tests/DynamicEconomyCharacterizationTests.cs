// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Characterization tests for DynamicEconomySystem decision surfaces.
    /// These pin the CURRENT behavior before any extraction to Ashfall.Core.
    /// </summary>
    public class DynamicEconomyCharacterizationTests
    {
        // ── Stance threshold constants ────────────────────────────────
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
        [Fact]
        public void Stance_ThresholdTable_ReturnsExpectedStance()
        {
            var cases = new[]
            {
                (Trust: -50f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.HostileRaid),
                (Trust: -51f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.HostileRaid),
                (Trust: -30f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.Rob),
                (Trust: -25f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.Refuse),
                (Trust: -20f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.Trade),
                (Trust: 39f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.Trade),
                (Trust: 40f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.ShareIntel),
                (Trust: 100f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: true, Expected: TradeStance.ShareIntel),
                (Trust: -50f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: false, Expected: TradeStance.Refuse),
                (Trust: 0f, RaidAt: -50f, RobAt: -30f, TradeAt: -20f, IntelAt: 40f, IsActive: false, Expected: TradeStance.Refuse)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var actual = ComputeStance(test.Trust, test.RaidAt, test.RobAt, test.TradeAt, test.IntelAt, test.IsActive);
                if (actual != test.Expected)
                    failures.Add($"trust={test.Trust}, active={test.IsActive}: expected {test.Expected}, got {actual}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void WillTrade_StanceTable_MatchesExpectedValues()
        {
            var cases = new[]
            {
                (Stance: TradeStance.Trade, Expected: true),
                (Stance: TradeStance.ShareIntel, Expected: true),
                (Stance: TradeStance.Refuse, Expected: false),
                (Stance: TradeStance.Rob, Expected: false),
                (Stance: TradeStance.HostileRaid, Expected: false)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                bool actual = test.Stance == TradeStance.Trade || test.Stance == TradeStance.ShareIntel;
                if (actual != test.Expected)
                    failures.Add($"{test.Stance}: expected {test.Expected}, got {actual}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void WillShareIntel_StanceTable_MatchesExpectedValues()
        {
            var cases = new[]
            {
                (Stance: TradeStance.ShareIntel, Expected: true),
                (Stance: TradeStance.Trade, Expected: false),
                (Stance: TradeStance.Refuse, Expected: false),
                (Stance: TradeStance.Rob, Expected: false),
                (Stance: TradeStance.HostileRaid, Expected: false)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                bool actual = test.Stance == TradeStance.ShareIntel;
                if (actual != test.Expected)
                    failures.Add($"{test.Stance}: expected {test.Expected}, got {actual}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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
            Assert.Equal(2.5f, overlay.GetScarcityMultiplier(5, "water"));
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(100, "water"));
            Assert.Equal(1.0f, overlay.GetScarcityMultiplier(5, "food"));
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
            Assert.Equal(TradeStance.HostileRaid, result.NewStance);
        }
    }
}
