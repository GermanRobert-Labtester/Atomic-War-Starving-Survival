// SPDX-License-Identifier: MIT
// Tests for Plan 146: Radiation → Economy & Social Bridge

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Plan146Radiation
{
    public class Plan146RadiationBridgeIntegrationTests
    {
        private readonly string _catalogPath;

        public Plan146RadiationBridgeIntegrationTests()
        {
            _catalogPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/radiation_economy_social.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/radiation_economy_social.json");
            }
        }

        private sealed class TestEconomySink : IRadiationEconomySink
        {
            public TradeEvaluation? LastEvaluation { get; private set; }
            public void OnTradeEvaluated(TradeEvaluation evaluation)
            {
                LastEvaluation = evaluation;
            }
        }

        private sealed class TestSocialSink : IRadiationSocialSink
        {
            public SurvivorRadiationSocialProfile? LastProfile { get; private set; }
            public void OnSocialEvaluation(SurvivorRadiationSocialProfile profile)
            {
                LastProfile = profile;
            }
        }

        [Fact]
        public void Catalog_LoadsEconomyRulesAndPoliciesSuccessfully()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file missing at {_catalogPath}");
            string json = File.ReadAllText(_catalogPath);

            var econBridge = new RadiationEconomyBridge();
            econBridge.LoadCatalog(json);

            Assert.NotEmpty(econBridge.Rules);
            Assert.Contains(econBridge.Rules, r => r.category == "food_water");
            Assert.Contains(econBridge.Rules, r => r.category == "equipment");

            var socialBridge = new RadiationSocialBridge();
            socialBridge.LoadCatalog(json);

            Assert.NotEmpty(socialBridge.Brackets);
            Assert.Contains(socialBridge.Brackets, b => b.bracket_id == "severe");
        }

        [Fact]
        public void Economy_ContaminatedProvisions_AppliesDiscount()
        {
            string json = File.ReadAllText(_catalogPath);
            var econBridge = new RadiationEconomyBridge();
            econBridge.LoadCatalog(json);

            var sink = new TestEconomySink();
            econBridge.Sink = sink;

            float modifiedMult = 0f;
            econBridge.OnRadiationPriceModifiedSeam = (item, fac, mult) => modifiedMult = mult;

            // 45% contamination food
            var eval = econBridge.EvaluateTrade("canned_peaches", "food_water", contaminationLevel: 45, basePrice: 50.0f);

            Assert.False(eval.isBlocked);
            Assert.Equal(0.40f, eval.priceMultiplier);
            Assert.Equal(20.0f, eval.finalPrice);
            Assert.Equal(0.40f, modifiedMult);
            Assert.Same(eval, sink.LastEvaluation);
        }

        [Fact]
        public void Economy_SevereContamination_BlocksTradeAndTriggersSeam()
        {
            string json = File.ReadAllText(_catalogPath);
            var econBridge = new RadiationEconomyBridge();
            econBridge.LoadCatalog(json);

            string blockedReason = "";
            econBridge.OnContaminatedTradeBlockedSeam = (item, fac, reason) => blockedReason = reason;

            // 75% contamination food -> trade blocked
            var eval = econBridge.EvaluateTrade("tainted_water", "food_water", contaminationLevel: 75, basePrice: 30.0f);

            Assert.True(eval.isBlocked);
            Assert.NotEmpty(eval.blockReason);
            Assert.NotEmpty(blockedReason);
            Assert.Equal(1, econBridge.TotalBlockedTrades);
        }

        [Fact]
        public void Social_DoseBrackets_AppliesSocialPenalty()
        {
            string json = File.ReadAllText(_catalogPath);
            var socialBridge = new RadiationSocialBridge();
            socialBridge.LoadCatalog(json);

            var sink = new TestSocialSink();
            socialBridge.Sink = sink;

            int recordedPenalty = 0;
            socialBridge.OnRadiationSocialPenaltyAppliedSeam = (id, bracket, pen) => recordedPenalty = pen;

            // Safe survivor (<20 mSv)
            var cleanProfile = socialBridge.EvaluateSocialStance("survivor_clean", doseMsv: 5.0f);
            Assert.Equal("safe", cleanProfile.bracketId);
            Assert.Equal(0, cleanProfile.socialPenalty);

            // Severe survivor (>100 mSv)
            var severeProfile = socialBridge.EvaluateSocialStance("survivor_hot", doseMsv: 150.0f);
            Assert.Equal("severe", severeProfile.bracketId);
            Assert.Equal(50, severeProfile.socialPenalty);
            Assert.Equal(50, recordedPenalty);
            Assert.Same(severeProfile, sink.LastProfile);
        }

        [Fact]
        public void Social_EncounteringFaction_RejectsIrradiatedPersonnel()
        {
            string json = File.ReadAllText(_catalogPath);
            var socialBridge = new RadiationSocialBridge();
            socialBridge.LoadCatalog(json);

            int standingPenalty = 0;
            socialBridge.OnRadiationFactionStandingAdjustedSeam = (survivor, faction, delta) => standingPenalty = delta;

            // Irradiated survivor approaching Hydro Barons
            var profile = socialBridge.EvaluateSocialStance(
                survivorId: "survivor_vask",
                doseMsv: 65.0f,
                encounteringFactionId: "faction_hydro_barons");

            Assert.Contains("faction_hydro_barons", profile.quarantinedByFactions);
            Assert.Equal(-10, standingPenalty);
            Assert.Equal(1, socialBridge.TotalFactionProtests);
        }

        [Fact]
        public void Bridges_SaveState_RoundTripsAccurately()
        {
            string json = File.ReadAllText(_catalogPath);
            var econBridge = new RadiationEconomyBridge();
            econBridge.LoadCatalog(json);

            econBridge.EvaluateTrade("item_1", "food_water", 15, 20f);
            econBridge.EvaluateTrade("item_2", "medical_supplies", 50, 100f);

            var econState = econBridge.CaptureState();
            Assert.Equal(1, econState.schema_version);
            Assert.Equal(2, econState.totalEvaluations);
            Assert.Equal(1, econState.totalBlockedTrades);

            var restoredEcon = new RadiationEconomyBridge();
            restoredEcon.RestoreState(econState);
            Assert.Equal(2, restoredEcon.TotalEvaluations);
            Assert.Equal(1, restoredEcon.TotalBlockedTrades);

            var socialBridge = new RadiationSocialBridge();
            socialBridge.LoadCatalog(json);
            socialBridge.EvaluateSocialStance("survivor_1", 120.0f, encounteringFactionId: "faction_garrison");

            var socialState = socialBridge.CaptureState();
            Assert.Equal(1, socialState.schema_version);
            Assert.Equal(1, socialState.totalFactionProtests);

            var restoredSocial = new RadiationSocialBridge();
            restoredSocial.RestoreState(socialState);
            Assert.Equal(1, restoredSocial.TotalFactionProtests);
        }
    }
}
