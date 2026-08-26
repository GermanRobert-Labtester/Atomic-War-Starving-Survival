using System;
using System.IO;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TradeThemeAndEconomyTests
    {
        [Fact]
        public void FactionStanceEngine_NormalFaction_EvaluatesStancesCorrectly()
        {
            var engine = new FactionStanceEngine();
            engine.RegisterFaction(new FactionThresholds(
                "scavenger_camp",
                raidThreshold: -60f,
                robThreshold: -30f,
                minTrustToTrade: -15f,
                intelShareThreshold: 50f,
                raidAggression: 0.4f,
                trustInversion: false,
                healthyRadiationCeiling: 20f,
                highRadiationFloor: 60f));

            Assert.True(engine.IsFactionActive("scavenger_camp"));
            Assert.Equal(0.4f, engine.GetRaidAggression("scavenger_camp"));

            engine.SetTrust("scavenger_camp", -70f);
            Assert.Equal(TradeStance.HostileRaid, engine.GetStance("scavenger_camp"));
            Assert.False(engine.WillTrade("scavenger_camp"));

            engine.SetTrust("scavenger_camp", -30f);
            Assert.Equal(TradeStance.Rob, engine.GetStance("scavenger_camp"));
            Assert.False(engine.WillTrade("scavenger_camp"));

            engine.SetTrust("scavenger_camp", -20f);
            Assert.Equal(TradeStance.Refuse, engine.GetStance("scavenger_camp"));
            Assert.False(engine.WillTrade("scavenger_camp"));

            engine.SetTrust("scavenger_camp", 0f);
            Assert.Equal(TradeStance.Trade, engine.GetStance("scavenger_camp"));
            Assert.True(engine.WillTrade("scavenger_camp"));
            Assert.False(engine.WillShareIntel("scavenger_camp"));

            engine.SetTrust("scavenger_camp", 55f);
            Assert.Equal(TradeStance.ShareIntel, engine.GetStance("scavenger_camp"));
            Assert.True(engine.WillTrade("scavenger_camp"));
            Assert.True(engine.WillShareIntel("scavenger_camp"));
        }

        [Fact]
        public void FactionStanceEngine_TrustInversion_CultOfTheGlow_DayGatedAndRadiationAware()
        {
            var engine = new FactionStanceEngine();
            int day = 10;
            float partyRad = 0f;
            bool hasArs = false;
            bool hasHazmat = false;

            engine.DayProvider = () => day;
            engine.PartyRadiationProvider = () => partyRad;
            engine.PartyHasArsProvider = () => hasArs;
            engine.PartyIntactHazmatProvider = () => hasHazmat;

            engine.RegisterFaction(new FactionThresholds(
                "cult_of_the_glow",
                raidThreshold: -50f,
                robThreshold: -20f,
                minTrustToTrade: -40f,
                intelShareThreshold: 40f,
                raidAggression: 0.5f,
                trustInversion: true,
                healthyRadiationCeiling: 20f,
                highRadiationFloor: 60f));

            // Before Day 30: inactive -> Refuse
            Assert.False(engine.IsFactionActive("cult_of_the_glow"));
            Assert.Equal(TradeStance.Refuse, engine.GetStance("cult_of_the_glow"));

            // Day 30+: active
            day = 30;
            Assert.True(engine.IsFactionActive("cult_of_the_glow"));

            // Low radiation (0 rads <= 20 ceiling) -> MinTrust (-100) -> HostileRaid
            partyRad = 10f;
            Assert.Equal(FactionStanceConstants.MinTrust, engine.GetEffectiveTrust("cult_of_the_glow"));
            Assert.Equal(TradeStance.HostileRaid, engine.GetStance("cult_of_the_glow"));

            // High radiation (>= 60 floor) -> MaxTrust (100) -> ShareIntel
            partyRad = 80f;
            Assert.Equal(FactionStanceConstants.MaxTrust, engine.GetEffectiveTrust("cult_of_the_glow"));
            Assert.Equal(TradeStance.ShareIntel, engine.GetStance("cult_of_the_glow"));

            // Hazmat worn with low radiation -> MinTrust
            partyRad = 10f;
            hasHazmat = true;
            Assert.Equal(FactionStanceConstants.MinTrust, engine.GetEffectiveTrust("cult_of_the_glow"));

            // ARS reverence outranks hazmat -> MaxTrust
            hasArs = true;
            Assert.Equal(FactionStanceConstants.MaxTrust, engine.GetEffectiveTrust("cult_of_the_glow"));
        }

        [Fact]
        public void HardcoreEconomyTuning_PriceShocks_LoadsRulesAndCalculatesMultipliers()
        {
            var scarcity = new[]
            {
                new ScarcityEntry(ScarcityTier.Critical, 2.0f, "1-10", new[] { "clean_water" }, "drought"),
                new ScarcityEntry(ScarcityTier.High, 1.5f, "20-30", new[] { "*" }, "general shortage")
            };
            var shocks = new[]
            {
                new PriceShockRule(PriceShockKind.PlumePassing, 2.5f, 3, new[] { "rad_pills", "filter" }, "rad plume")
            };
            var bundle = new HardcoreEconomyTuningBundle(scarcity, Array.Empty<FactionTradePreference>(), shocks);
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(bundle);

            Assert.True(tuning.IsActive);

            // Day 5 water multiplier should be 2.0
            float multDay5 = tuning.GetScarcityMultiplier(5, "clean_water");
            Assert.Equal(2.0f, multDay5);

            // Wildcard test on day 25
            float multMeds = tuning.GetScarcityMultiplier(25, "antibiotics");
            Assert.Equal(1.5f, multMeds);

            // Price shock query
            bool hasShock = tuning.TryGetPriceShock(PriceShockKind.PlumePassing, 1, out var rule);
            Assert.True(hasShock);
            Assert.Equal(2.5f, rule.Multiplier);
        }

        [Fact]
        public void GeneratedUiAssets_ExistOnDiskInExpectedDirectories()
        {
            string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
            string[] expectedAssets = new[]
            {
                "assets/ui/Textures/panel_bg_9slice.png",
                "assets/ui/Textures/header_bar_9slice.png",
                "assets/ui/Icons/icon_shock_plume.png",
                "assets/ui/Icons/icon_shock_convoy.png",
                "assets/ui/Icons/icon_shock_war.png",
                "assets/ui/Icons/icon_shock_winter.png",
                "assets/ui/Icons/faction_icon_military_remnants.png",
                "assets/ui/Icons/faction_icon_scavenger_camp.png",
                "assets/ui/Icons/faction_icon_cult_of_the_glow.png",
                "assets/sprites/Characters/placeholder_survivor.png",
                "assets/ui/Icons/icon_placeholder.png"
            };

            foreach (var relPath in expectedAssets)
            {
                string fullPath = Path.Combine(root, relPath);
                Assert.True(File.Exists(fullPath), $"Expected asset missing: {fullPath}");
            }
        }
    }
}
