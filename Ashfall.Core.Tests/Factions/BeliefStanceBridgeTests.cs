// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W9 — Belief → faction stance bridge (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W9-BELIEF-STANCE-BRIDGE (cases AV.9).
//
// Interiority is political: a community's belief adherence moves faction
// standing, through the single FactionStanceEngine write. These tests pin the
// translation, the runaway budget, the crisis inversion, counterplay, and that
// the bridge never writes standing itself.
// ============================================================================

using System.Collections.Generic;
using Ashfall.Core.Spiritual;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class BeliefStanceBridgeTests
    {
        private static SpiritualCatalog CatalogWith(params (string id, Dictionary<string, float> lean)[] beliefs)
        {
            var catalog = new SpiritualCatalog();
            foreach (var (id, leaning) in beliefs)
            {
                catalog.Movements.Add(new BeliefMovementDefinition
                {
                    Id = id,
                    DisplayName = id,
                    FactionLeaning = leaning
                });
            }
            return catalog;
        }

        private static BeliefStanceBridge Bridge(out SpiritualCatalog catalog)
        {
            catalog = CatalogWith(
                ("belief_witnesses", new Dictionary<string, float> { { "archivists", 4f }, { "black_ops", -3f } }),
                ("belief_listeners", new Dictionary<string, float> { { "black_cross", 5f } }));
            return new BeliefStanceBridge(catalog);
        }

        [Fact]
        public void ConversionMovesStanding_InAuthoredDirection()
        {
            var bridge = Bridge(out _);
            var shifts = bridge.OnBeliefAdhered("belief_witnesses", conviction: 80, day: 100);

            Assert.Equal(2, shifts.Count);
            // Ordinal faction order: archivists before black_ops.
            Assert.Equal("archivists", shifts[0].FactionId);
            Assert.True(shifts[0].Delta > 0f);
            Assert.Equal("black_ops", shifts[1].FactionId);
            Assert.True(shifts[1].Delta < 0f);
        }

        [Fact]
        public void UnknownBelief_MovesNothing()
        {
            var bridge = Bridge(out _);
            Assert.Empty(bridge.OnBeliefAdhered("belief_nonexistent", 90, 10));
        }

        [Fact]
        public void BeliefWithNoAuthoredLeaning_MovesNothing()
        {
            var bridge = new BeliefStanceBridge(CatalogWith(("belief_silent", new Dictionary<string, float>())));
            Assert.Empty(bridge.OnBeliefAdhered("belief_silent", 100, 10));
        }

        [Fact]
        public void ConvictionScalesTheEffect()
        {
            var bridge = Bridge(out _);
            var firm = bridge.OnBeliefAdhered("belief_witnesses", 100, 10);
            var bridge2 = Bridge(out _);
            var faint = bridge2.OnBeliefAdhered("belief_witnesses", 5, 10);
            Assert.True(firm[0].Delta > faint[0].Delta);
        }

        [Fact]
        public void MassConversion_CannotRunawayTrust()   // §U.10 daily bound
        {
            var bridge = Bridge(out _);
            float total = 0f;
            for (int i = 0; i < 50; i++)
            {
                foreach (var s in bridge.OnBeliefAdhered("belief_witnesses", 100, 7)) total += s.Delta;
            }
            Assert.True(total <= BeliefStanceBridge.DailyBudgetPerPair + 0.0001f,
                $"a same-day conversion wave moved {total} trust; the daily budget is the guard");
        }

        [Fact]
        public void BudgetRollsOverWithTheDay()
        {
            var bridge = Bridge(out _);
            float day1 = 0f;
            foreach (var s in bridge.OnBeliefAdhered("belief_witnesses", 100, 7)) day1 += s.Delta;
            float day2 = 0f;
            foreach (var s in bridge.OnBeliefAdhered("belief_witnesses", 100, 8)) day2 += s.Delta;
            Assert.True(day1 > 0f && day2 > 0f, "a new day restores the budget");
        }

        [Fact]
        public void FaithCrisis_InvertsTheLeaning()
        {
            var bridge = Bridge(out _);
            var shifts = bridge.OnFaithCrisis("belief_witnesses", conviction: 90, day: 20);
            // Friends of the belief lose standing; detractors gain a little.
            Assert.Equal("archivists", shifts[0].FactionId);
            Assert.True(shifts[0].Delta < 0f);
            Assert.Equal("black_ops", shifts[1].FactionId);
            Assert.True(shifts[1].Delta > 0f);
        }

        [Fact]
        public void Outreach_CanWinStandingBack()          // §AA.9 counterplay
        {
            var bridge = Bridge(out _);
            var recovery = bridge.OnOutreach(day: 21, beliefId: "belief_witnesses", factionId: "archivists");
            Assert.NotNull(recovery);
            Assert.True(recovery!.Delta < 0f, "outreach pulls leaning trust back toward neutral");
        }

        [Fact]
        public void Outreach_OnAnUnrelatedPair_IsNull()
        {
            var bridge = Bridge(out _);
            Assert.Null(bridge.OnOutreach(21, "belief_witnesses", "some_other_faction"));
        }

        [Fact]
        public void AuthoredLeaning_IsClamped()
        {
            var catalog = CatalogWith(("belief_extreme", new Dictionary<string, float> { { "f", 900f } }));
            var bridge = new BeliefStanceBridge(catalog);
            var shifts = bridge.OnBeliefAdhered("belief_extreme", 100, 5);
            Assert.True(shifts[0].Delta <= BeliefStanceBridge.DailyBudgetPerPair);
        }

        [Fact]
        public void InfluenceIsLegible()                    // §AA.9 legibility
        {
            var bridge = Bridge(out _);
            var influence = bridge.DescribeStandingInfluence("belief_witnesses");
            Assert.Equal(2, influence.Count);
            Assert.Equal("archivists", influence[0].FactionId); // stable order for UI
        }

        [Fact]
        public void BridgeNeverWritesStanding_ItOnlyProposes()
        {
            // The bridge's whole contract: proposals, not state. There is no
            // trust field on it at all — standing lives in FactionStanceEngine.
            var bridge = Bridge(out _);
            var shifts = bridge.OnBeliefAdhered("belief_witnesses", 100, 1);
            Assert.NotEmpty(shifts);
            Assert.All(shifts, s => Assert.False(string.IsNullOrEmpty(s.Reason)));
        }

        [Fact]
        public void DeterministicOrdering_AndValues()
        {
            var a = Bridge(out _).OnBeliefAdhered("belief_witnesses", 60, 3);
            var b = Bridge(out _).OnBeliefAdhered("belief_witnesses", 60, 3);
            Assert.Equal(a.Count, b.Count);
            for (int i = 0; i < a.Count; i++)
            {
                Assert.Equal(a[i].FactionId, b[i].FactionId);
                Assert.Equal(a[i].Delta, b[i].Delta, 5);
            }
        }

        [Fact]
        public void EmptyCatalog_IsSafe()
        {
            var bridge = new BeliefStanceBridge(new SpiritualCatalog());
            Assert.Equal(0, bridge.AuthoredBeliefCount);
            Assert.Empty(bridge.OnBeliefAdhered("anything", 100, 1));
        }
    }
}
