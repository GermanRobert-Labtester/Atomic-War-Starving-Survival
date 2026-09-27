// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Economy;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests.Maritime
{
    /// <summary>
    /// Core-contract tests for the authored Black Flotilla standing policy and its
    /// registration helper on the live stance engine. FactionStanceEngine stays the
    /// only trust store; this table supplies only thresholds and tier verdicts.
    /// </summary>
    public sealed class PlanBlackFlotillaStandingTests
    {
        private const string Id = BlackFlotillaStanding.FactionId;

        [Fact]
        public void AuthoredPolicyTableIsExact()
        {
            Assert.Equal("faction_black_flotilla", Id);
            Assert.Equal(-50f, BlackFlotillaStanding.RaidThreshold);
            Assert.Equal(-20f, BlackFlotillaStanding.RobThreshold);
            Assert.Equal(0f, BlackFlotillaStanding.MinTrustToTrade);
            Assert.Equal(40f, BlackFlotillaStanding.IntelShareThreshold);
            Assert.Equal(0.35f, BlackFlotillaStanding.RaidAggression);
            Assert.Equal(30f, BlackFlotillaStanding.SalvageTrustedTrust);
            Assert.Equal(55f, BlackFlotillaStanding.DeepCooperationTrust);
        }

        [Fact]
        public void ThresholdsCarryTheAuthoredFactionIdAndNoTrustInversion()
        {
            var thresholds = BlackFlotillaStanding.Thresholds;
            Assert.Equal(Id, thresholds.FactionId);
            Assert.Equal(-50f, thresholds.RaidThreshold);
            Assert.Equal(40f, thresholds.IntelShareThreshold);
            Assert.False(thresholds.TrustInversion);
        }

        [Fact]
        public void RegisterPutsTheAuthoredRowOnTheLiveEngine()
        {
            var engine = new FactionStanceEngine();
            BlackFlotillaStanding.Register(engine);

            Assert.True(engine.IsFactionActive(Id));
            Assert.Equal(BlackFlotillaStanding.RaidAggression, engine.GetRaidAggression(Id));
        }

        [Fact]
        public void RegisterIsIdempotentAndNeverOverwritesLiveTrust()
        {
            var engine = new FactionStanceEngine();
            BlackFlotillaStanding.Register(engine);
            engine.SetTrust(Id, 12f);
            BlackFlotillaStanding.Register(engine);

            Assert.Equal(12f, engine.GetEffectiveTrust(Id), 3);
        }

        [Fact]
        public void TradeGateSitsOnTheAuthoredZeroBoundary()
        {
            Assert.False(BlackFlotillaStanding.CanTrade(-5f));
            Assert.True(BlackFlotillaStanding.CanTrade(0f));
            Assert.True(BlackFlotillaStanding.CanTrade(40f));
        }

        [Fact]
        public void IntelGateSitsOnTheAuthoredFortyBoundary()
        {
            Assert.False(BlackFlotillaStanding.CanShareIntel(39.9f));
            Assert.True(BlackFlotillaStanding.CanShareIntel(40f));
        }

        [Fact]
        public void Plan23TierBoundariesAreDistinctAndMonotone()
        {
            var hostile = BlackFlotillaStanding.TierFor(-80f);
            var wary = BlackFlotillaStanding.TierFor(5f);
            var salvage = BlackFlotillaStanding.TierFor(31f);
            var deep = BlackFlotillaStanding.TierFor(60f);

            Assert.NotEqual(hostile, wary);
            Assert.NotEqual(wary, salvage);
            Assert.NotEqual(salvage, deep);
            Assert.True(BlackFlotillaStanding.IsSalvageTrusted(30f));
            Assert.False(BlackFlotillaStanding.IsSalvageTrusted(29.9f));
            Assert.True(BlackFlotillaStanding.CanCooperateOnDeepDives(55f));
            Assert.False(BlackFlotillaStanding.CanCooperateOnDeepDives(54.9f));
        }

        [Fact]
        public void VerdictsTrackLiveEngineTrustInsteadOfASecondLedger()
        {
            var engine = new FactionStanceEngine();
            BlackFlotillaStanding.Register(engine);

            engine.SetTrust(Id, -5f);
            Assert.False(BlackFlotillaStanding.CanTrade(engine.GetEffectiveTrust(Id)));
            engine.SetTrust(Id, 45f);
            Assert.True(BlackFlotillaStanding.CanShareIntel(engine.GetEffectiveTrust(Id)));
        }

        [Fact]
        public void TrustChangesReResolveTheTier()
        {
            var engine = new FactionStanceEngine();
            BlackFlotillaStanding.Register(engine);
            engine.SetTrust(Id, 10f);

            var before = BlackFlotillaStanding.TierFor(engine.GetEffectiveTrust(Id));
            engine.ModifyTrust(Id, 50f);
            var after = BlackFlotillaStanding.TierFor(engine.GetEffectiveTrust(Id));

            Assert.NotEqual(before, after);
        }
    }
}
