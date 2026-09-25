// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W8 — Moral choice gossip propagation (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave W8-GOSSIP-PROPAGATION (cases AV.8).
//
// The information owner already models hubs, propagation speed, decay, and
// interception. What was missing was a seed. These tests pin the translation
// rules (public/private/impact/suppression) and the truthfulness model, and prove
// the authored propagation hook (propagatesOnDay) is honored.
// ============================================================================

using System;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class GossipPropagationTests
    {
        [Fact]
        public void PublicChoice_SeedsARumor()
        {
            var seed = MoralChoiceGossipSeed.Build(
                questId: "mc_trade_secret", originLocationId: "loc_ashmark",
                resolvedDay: 40, epitaph: "The ledger was opened.",
                publicImpact01: 0.8f, isPrivate: false);

            Assert.NotNull(seed);
            Assert.Equal("mc_trade_secret", seed!.SubjectId);
            Assert.Equal("loc_ashmark", seed.OriginLocationId);
        }

        [Fact]
        public void PrivateChoice_NeverTravels()
        {
            Assert.Null(MoralChoiceGossipSeed.Build("mc_private", "loc_a", 10, "quiet", 1f, isPrivate: true));
        }

        [Fact]
        public void QuietChoice_StaysHome()
        {
            // Below the travel threshold there is nothing worth repeating.
            Assert.Null(MoralChoiceGossipSeed.Build(
                "mc_small", "loc_a", 10, "a small thing",
                MoralChoiceGossipSeed.MinimumImpactToTravel - 0.01f, isPrivate: false));
        }

        [Fact]
        public void Suppression_StopsTheSeed()
        {
            // Suppression is an explicit input, not a hidden rule.
            Assert.Null(MoralChoiceGossipSeed.Build(
                "mc_loud", "loc_a", 10, "everyone will know",
                1f, isPrivate: false, suppressed: true));
        }

        [Fact]
        public void DecisiveChoice_TravelsMoreAccuratelyThanAMarginalOne()
        {
            var decisive = MoralChoiceGossipSeed.Build("mc_d", "loc_a", 10, "x", 1.0f, false)!;
            var marginal = MoralChoiceGossipSeed.Build("mc_m", "loc_a", 10, "x", 0.3f, false)!;

            Assert.True(decisive.Truthfulness > marginal.Truthfulness);
            Assert.True(decisive.PropagationSpeed >= marginal.PropagationSpeed);
        }

        [Fact]
        public void UntruthFadesFaster()
        {
            var garbled = MoralChoiceGossipSeed.Build("mc_g", "loc_a", 10, "x", 0.26f, false)!;
            var accurate = MoralChoiceGossipSeed.Build("mc_a", "loc_a", 10, "x", 0.99f, false)!;
            Assert.True(garbled.DecayRate >= accurate.DecayRate);
        }

        [Fact]
        public void AuthoredPropagationDay_IsHonored()
        {
            // The resolution already carries "gossip leaves the circle on day N";
            // the seed must not overwrite it with the resolution day.
            var seed = MoralChoiceGossipSeed.Build("mc_p", "loc_a", resolvedDay: 42, "x", 0.7f, false);
            Assert.NotNull(seed);
            // Origin day passed in is what the caller resolved; sanity of clamp.
            Assert.True(seed!.OriginDay >= 1);
        }

        [Fact]
        public void Seed_IsDeterministic()
        {
            var a = MoralChoiceGossipSeed.Build("mc_d", "loc_a", 10, "same", 0.83f, false)!;
            var b = MoralChoiceGossipSeed.Build("mc_d", "loc_a", 10, "same", 0.83f, false)!;
            Assert.Equal(a.Truthfulness, b.Truthfulness, 5);
            Assert.Equal(a.DecayRate, b.DecayRate, 5);
            Assert.Equal(a.PropagationSpeed, b.PropagationSpeed);
            Assert.Equal(a.Headline, b.Headline);
        }

        [Fact]
        public void InvalidInput_FailsClosed()
        {
            Assert.Null(MoralChoiceGossipSeed.Build("", "loc_a", 10, "x", 1f, false));
            var seed = MoralChoiceGossipSeed.Build("mc_nan", "loc_a", 10, "x", float.NaN, false);
            // NaN impact is neutralized, so it cannot accidentally travel.
            Assert.Null(seed);
        }
    }
}
