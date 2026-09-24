// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Espionage;
using Xunit;

namespace Ashfall.Core.Tests.Espionage
{
    public sealed class InformantNetworkTradecraftEngineTests
    {
        [Fact]
        public void ExecuteTradecraftOperation_SuccessfulDrop_DeliversIntel()
        {
            var informant = new InformantRecord
            {
                InformantId = "asset_iron_scribe",
                TargetFactionId = "faction_iron_covenant",
                Archetype = InformantArchetype.IdeologicalDefector,
                ActiveMethod = TradecraftMethod.DeadDrop,
                LoyaltyPermille = 850,
                SuspicionPermille = 50,
                IntelligenceYieldPermille = 700
            };

            var result = InformantNetworkTradecraftEngine.ExecuteTradecraftOperation(
                informant,
                simTick: 500,
                // Pinned stable-mix success case; the previous arbitrary seed
                // could cross the interception threshold between processes.
                worldSeed: 12340);

            Assert.True(result.Success);
            Assert.True(result.IntelPointsDelivered > 0);
            Assert.False(result.AssetCompromised);
            Assert.False(result.InterceptedByEnemy);
        }

        [Fact]
        public void ExecuteTradecraftOperation_CompromisedAsset_AlwaysFails()
        {
            var informant = new InformantRecord
            {
                InformantId = "compromised_asset",
                IsCompromised = true
            };

            var result = InformantNetworkTradecraftEngine.ExecuteTradecraftOperation(
                informant,
                simTick: 100,
                worldSeed: 999);

            Assert.False(result.Success);
            Assert.Equal(0, result.IntelPointsDelivered);
            Assert.True(result.AssetCompromised);
            Assert.True(result.InterceptedByEnemy);
        }

        [Fact]
        public void DetectCompromisedAsset_HighCounterIntel_DetectsCompromise()
        {
            var compromised = new InformantRecord
            {
                InformantId = "sleeper_agent_01",
                IsCompromised = true
            };

            // High counter-intel rating (950 permille)
            bool detected = InformantNetworkTradecraftEngine.DetectCompromisedAsset(
                compromised,
                shelterCounterIntelRatingPermille: 950,
                simTick: 200,
                worldSeed: 8888);

            Assert.True(detected);
        }

        [Fact]
        public void EvaluateInterrogation_HumaneVsCoercive_ReflectsEthicalDoctrine()
        {
            var captive = new InformantRecord
            {
                InformantId = "captive_lieutenant",
                LoyaltyPermille = 500
            };

            var humaneOutcome = InformantNetworkTradecraftEngine.EvaluateInterrogation(captive, humaneProtocolsEnforced: true);
            var coerciveOutcome = InformantNetworkTradecraftEngine.EvaluateInterrogation(captive, humaneProtocolsEnforced: false);

            Assert.True(humaneOutcome.ReliableIntelligenceObtained);
            Assert.True(humaneOutcome.CredibilityScorePermille >= 500);
            Assert.Equal(0, humaneOutcome.MoraleCostPermille);
            Assert.False(humaneOutcome.FabricatedIntelWarning);

            Assert.False(coerciveOutcome.ReliableIntelligenceObtained);
            Assert.True(coerciveOutcome.MoraleCostPermille > 0);
            Assert.True(coerciveOutcome.FabricatedIntelWarning);
        }
    }
}
