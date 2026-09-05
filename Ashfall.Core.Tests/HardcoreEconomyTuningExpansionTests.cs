using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class HardcoreEconomyTuningExpansionTests
    {
        private static string FindDataDir()
        {
            string current = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Path.GetDirectoryName(current);
                if (parent == current) break;
                current = parent;
            }
            throw new DirectoryNotFoundException("Could not find Assets/StreamingAssets/Data directory.");
        }

        private static HardcoreEconomyTuningBundle LoadAuthoritativeBundle()
        {
            string path = Path.Combine(FindDataDir(), "hardcore_economy_tuning.json");
            string json = File.ReadAllText(path);
            var result = HardcoreEconomyTuningLoader.Load(json);
            Assert.True(result.IsValid, string.Join("; ", result.Errors));
            Assert.NotNull(result.Bundle);
            return result.Bundle!;
        }

        [Fact]
        public void AuthoritativeCatalog_LoadsSuccessfully()
        {
            var bundle = LoadAuthoritativeBundle();
            Assert.NotNull(bundle);
            Assert.Equal(8, bundle.ScarcityTiers.Count);
            Assert.Equal(8, bundle.FactionPreferences.Count);
            Assert.Equal(6, bundle.PriceShockRules.Count);
        }

        [Fact]
        public void ScarcityTiers_ExactEightTiers_AndBaselinesPreserved()
        {
            var bundle = LoadAuthoritativeBundle();
            Assert.Equal(8, bundle.ScarcityTiers.Count);

            // Tier 0: Critical (Days 1-15)
            var critical = bundle.ScarcityTiers[0];
            Assert.Equal(ScarcityTier.Critical, critical.Tier);
            Assert.Equal(2.5f, critical.Multiplier);
            Assert.Equal("Days 1-15", critical.DayRangeLabel);
            Assert.Contains("clean_water", critical.AffectedItemIds);
            Assert.Contains("iodine_pills", critical.AffectedItemIds);

            // Tier 1: High (Days 15-40)
            var high = bundle.ScarcityTiers[1];
            Assert.Equal(ScarcityTier.High, high.Tier);
            Assert.Equal(2.0f, high.Multiplier);
            Assert.Equal("Days 15-40", high.DayRangeLabel);
            Assert.Contains("antibiotics", high.AffectedItemIds);
            Assert.Contains("fuel", high.AffectedItemIds);

            // Tier 2: Moderate (Days 41-100)
            var moderate = bundle.ScarcityTiers[2];
            Assert.Equal(ScarcityTier.Moderate, moderate.Tier);
            Assert.Equal(1.6f, moderate.Multiplier);
            Assert.Equal("Days 41-100", moderate.DayRangeLabel);

            // Tier 3: Stable (Days 101-160)
            var stable = bundle.ScarcityTiers[3];
            Assert.Equal(ScarcityTier.Stable, stable.Tier);
            Assert.Equal(1.3f, stable.Multiplier);
            Assert.Equal("Days 101-160", stable.DayRangeLabel);

            // Tier 4: Reconstruction (Days 161-220) - derived 6th new tier
            var reconstruction = bundle.ScarcityTiers[4];
            Assert.Equal(ScarcityTier.Reconstruction, reconstruction.Tier);
            Assert.Equal(1.5f, reconstruction.Multiplier);
            Assert.Equal("Days 161-220", reconstruction.DayRangeLabel);

            // Tier 5: LateScarcity (Days 221-280)
            var late = bundle.ScarcityTiers[5];
            Assert.Equal(ScarcityTier.LateScarcity, late.Tier);
            Assert.Equal(1.8f, late.Multiplier);
            Assert.Equal("Days 221-280", late.DayRangeLabel);

            // Tier 6: DeepWinter (Days 281-340)
            var winter = bundle.ScarcityTiers[6];
            Assert.Equal(ScarcityTier.DeepWinter, winter.Tier);
            Assert.Equal(2.2f, winter.Multiplier);
            Assert.Equal("Days 281-340", winter.DayRangeLabel);

            // Tier 7: Endgame (Days 341+)
            var endgame = bundle.ScarcityTiers[7];
            Assert.Equal(ScarcityTier.Endgame, endgame.Tier);
            Assert.Equal(2.4f, endgame.Multiplier);
            Assert.Equal("Days 341+", endgame.DayRangeLabel);
        }

        [Fact]
        public void ScarcityTiers_FullCampaignDayCoverage_AcrossAllTiers()
        {
            var bundle = LoadAuthoritativeBundle();
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(bundle);

            // Day 5: Critical
            Assert.Equal(2.5f, tuning.GetScarcityMultiplier(5, "clean_water"));

            // Day 25: High
            Assert.Equal(2.0f, tuning.GetScarcityMultiplier(25, "antibiotics"));

            // Day 50: Moderate
            Assert.Equal(1.6f, tuning.GetScarcityMultiplier(50, "scrap_mechanical"));

            // Day 120: Stable
            Assert.Equal(1.3f, tuning.GetScarcityMultiplier(120, "seed_packets"));

            // Day 180: Reconstruction
            Assert.Equal(1.5f, tuning.GetScarcityMultiplier(180, "engine"));

            // Day 250: Late Scarcity
            Assert.Equal(1.8f, tuning.GetScarcityMultiplier(250, "fuel"));

            // Day 300: Deep Winter
            Assert.Equal(2.2f, tuning.GetScarcityMultiplier(300, "clean_water"));

            // Day 350 & 500: Endgame (open-ended '+')
            Assert.Equal(2.4f, tuning.GetScarcityMultiplier(350, "engine"));
            Assert.Equal(2.4f, tuning.GetScarcityMultiplier(500, "engine"));
        }

        [Fact]
        public void MatchesItem_WildcardPrefix_MatchesCorrectly()
        {
            var bundle = LoadAuthoritativeBundle();
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(bundle);

            // In LateScarcity (Day 250), "ammo_*" is affected
            Assert.Equal(1.8f, tuning.GetScarcityMultiplier(250, "ammo_9x19"));
            Assert.Equal(1.8f, tuning.GetScarcityMultiplier(250, "ammo_762"));
            Assert.Equal(1.8f, tuning.GetScarcityMultiplier(250, "ammo_308"));
            // Unrelated item not affected
            Assert.Equal(1.0f, tuning.GetScarcityMultiplier(250, "paper_scrap"));
        }

        [Fact]
        public void FactionPreferences_ExactEightUniqueFactions()
        {
            var bundle = LoadAuthoritativeBundle();
            Assert.Equal(8, bundle.FactionPreferences.Count);

            var expectedFactions = new[]
            {
                "central_garrison_remnants",
                "faction_black_flotilla",
                "faction_the_scale",
                "faction_the_compact",
                "faction_the_underwrite",
                "faction_the_cutters",
                "faction_the_rebuilders",
                "faction_the_overlay"
            };

            var actualIds = bundle.FactionPreferences.Select(f => f.FactionId).ToList();
            Assert.Equal(expectedFactions.Length, actualIds.Distinct().Count());

            foreach (var expected in expectedFactions)
            {
                Assert.Contains(expected, actualIds);
            }
        }

        [Fact]
        public void FactionPreferences_NoCollisionBetweenPremiumAndRefuses()
        {
            var bundle = LoadAuthoritativeBundle();
            foreach (var f in bundle.FactionPreferences)
            {
                Assert.NotEmpty(f.BuysAtPremium);
                Assert.NotEmpty(f.Refuses);
                Assert.False(string.IsNullOrWhiteSpace(f.TradeCurrency));

                var premium = new HashSet<string>(f.BuysAtPremium, StringComparer.OrdinalIgnoreCase);
                foreach (var refused in f.Refuses)
                {
                    Assert.DoesNotContain(refused, premium);
                }
            }
        }

        [Fact]
        public void PriceShocks_ExactSixShocks_AndBaselinesPreserved()
        {
            var bundle = LoadAuthoritativeBundle();
            Assert.Equal(6, bundle.PriceShockRules.Count);

            // PlumePassing (baseline)
            var plume = bundle.PriceShockRules.First(s => s.Kind == PriceShockKind.PlumePassing);
            Assert.Equal(1.8f, plume.Multiplier);
            Assert.Equal(3, plume.DurationDays);
            Assert.Contains("*", plume.AffectedItemIds);

            // ConvoyAmbush
            var convoy = bundle.PriceShockRules.First(s => s.Kind == PriceShockKind.ConvoyAmbush);
            Assert.Equal(1.6f, convoy.Multiplier);
            Assert.Equal(3, convoy.DurationDays);
            Assert.Contains("fuel", convoy.AffectedItemIds);

            // FactionConflict
            var conflict = bundle.PriceShockRules.First(s => s.Kind == PriceShockKind.FactionConflict);
            Assert.Equal(1.7f, conflict.Multiplier);
            Assert.Equal(5, conflict.DurationDays);

            // SeasonalScarcity
            var season = bundle.PriceShockRules.First(s => s.Kind == PriceShockKind.SeasonalScarcity);
            Assert.Equal(1.5f, season.Multiplier);
            Assert.Equal(7, season.DurationDays);

            // DiseaseOutbreak
            var disease = bundle.PriceShockRules.First(s => s.Kind == PriceShockKind.DiseaseOutbreak);
            Assert.Equal(2.0f, disease.Multiplier);
            Assert.Equal(4, disease.DurationDays);

            // FuelShortage
            var fuel = bundle.PriceShockRules.First(s => s.Kind == PriceShockKind.FuelShortage);
            Assert.Equal(1.9f, fuel.Multiplier);
            Assert.Equal(3, fuel.DurationDays);
        }

        [Fact]
        public void PriceShocks_QueryWithinAndBeyondDuration()
        {
            var bundle = LoadAuthoritativeBundle();
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(bundle);

            // DiseaseOutbreak has duration 4
            Assert.True(tuning.TryGetPriceShock(PriceShockKind.DiseaseOutbreak, 0, out var rule0));
            Assert.Equal(2.0f, rule0.Multiplier);

            Assert.True(tuning.TryGetPriceShock(PriceShockKind.DiseaseOutbreak, 3, out var rule3));
            Assert.Equal(2.0f, rule3.Multiplier);

            // Day offset 4 is at duration boundary -> expired
            Assert.False(tuning.TryGetPriceShock(PriceShockKind.DiseaseOutbreak, 4, out _));
        }

        [Fact]
        public void Stacking_CombinedMultiplierRemainsBounded()
        {
            var bundle = LoadAuthoritativeBundle();
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(bundle);

            // Simulate worst-case stacking on clean_water:
            // Critical Tier (2.5) * Price Shock PlumePassing (1.8) = 4.5
            float scarcity = tuning.GetScarcityMultiplier(5, "clean_water");
            Assert.Equal(2.5f, scarcity);

            bool hasShock = tuning.TryGetPriceShock(PriceShockKind.PlumePassing, 1, out var shock);
            Assert.True(hasShock);

            float combined = scarcity * shock.Multiplier;
            Assert.Equal(4.5f, combined);
            Assert.True(combined <= 10.0f, "Effective stacked price multiplier must remain bounded below 10.0x.");
        }

        [Fact]
        public void NegativeFixture_InvalidTier_ReturnsFailure()
        {
            string invalid = @"{
                ""version"": 1,
                ""scarcity_tiers"": [
                    { ""tier"": ""GalacticHyperinflation"", ""multiplier"": 5.0, ""day_range_label"": ""Days 1-10"", ""affected_item_ids"": [""fuel""], ""rationale"": ""r"" }
                ],
                ""faction_preferences"": [],
                ""price_shock_rules"": []
            }";

            var result = HardcoreEconomyTuningLoader.Load(invalid);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.Contains("invalid tier"));
        }

        [Fact]
        public void NegativeFixture_DuplicateFaction_ReturnsFailure()
        {
            string invalid = @"{
                ""version"": 1,
                ""scarcity_tiers"": [],
                ""faction_preferences"": [
                    { ""faction_id"": ""fac_a"", ""buys_at_premium"": [""fuel""], ""refuses"": [""book""], ""trade_currency"": ""c"" },
                    { ""faction_id"": ""fac_a"", ""buys_at_premium"": [""ammo_9x19""], ""refuses"": [""jewelry""], ""trade_currency"": ""c"" }
                ],
                ""price_shock_rules"": []
            }";

            var result = HardcoreEconomyTuningLoader.Load(invalid);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.Contains("duplicate FactionId"));
        }

        [Fact]
        public void Persistence_OldSaveSimulation_OperatesSafely()
        {
            // Simulate restoring a save game from before Plan 99 that had no shock or custom preference
            var bundle = LoadAuthoritativeBundle();
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(bundle);

            // New factions queryable immediately without save migration
            Assert.True(tuning.TryGetFactionPreference("faction_the_scale", out var scalePref));
            Assert.Equal("faction_the_scale", scalePref.FactionId);
            Assert.Contains("water_filter", scalePref.BuysAtPremium);

            // Default fallback when querying non-existent faction
            Assert.False(tuning.TryGetFactionPreference("unregistered_raider_band", out _));
        }
    }
}
