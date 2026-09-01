// SPDX-License-Identifier: MIT
// Plan 60 / D7 — the grief chain is bound. MemorialSystem.Memorialize already
// routed to IGriefSink, and SurvivorRelationsSystem.ApplyGrief already existed,
// but the sink was never assigned in the host and ApplyGrief was never called
// from gameplay: two live authorities with nothing between them. These tests pin
// the bridge (RelationsGriefSink) and the relationship query it depends on.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests.Memorial
{
    public class RelationsGriefBindingTests
    {
        private static SurvivorRelationsSystem Relations(
            params (string A, string B)[] pairs)
        {
            var sr = new SurvivorRelationsSystem(new SeededRng(7));
            foreach (var p in pairs) sr.GetOrCreateRelationship(p.A, p.B);
            return sr;
        }

        private static float GriefOf(SurvivorRelationsSystem sr, string a, string b) =>
            sr.GetOrCreateRelationship(a, b).grief;

        // ── RelatedIds: who mourns whom ────────────────────────────────

        [Fact]
        public void RelatedIds_ReturnsCounterpartiesSortedAndDeduped()
        {
            var sr = Relations(
                ("sv_z", "sv_b"),
                ("sv_a", "sv_z"),
                ("sv_b", "sv_z"));   // duplicate counterpart on purpose

            Assert.Equal(new[] { "sv_a", "sv_b" }, sr.RelatedIds("sv_z").ToArray());
        }

        [Fact]
        public void RelatedIds_EmptyOrUnknown_IsEmptyNotNull()
        {
            var sr = Relations(("sv_a", "sv_b"));

            Assert.Empty(sr.RelatedIds(null));
            Assert.Empty(sr.RelatedIds(""));
            Assert.Empty(sr.RelatedIds("sv_nobody"));
        }

        [Fact]
        public void RelatedIds_SelfLoopDoesNotReportTheSurvivorAsTheirOwnMourner()
        {
            var sr = new SurvivorRelationsSystem(new SeededRng(7));
            sr.GetOrCreateRelationship("sv_a", "sv_a");

            Assert.DoesNotContain("sv_a", sr.RelatedIds("sv_a"));
        }

        // ── the sink itself ────────────────────────────────────────────

        [Fact]
        public void ApplyDispersion_GrievesEachSurvivingRelationOnce()
        {
            var sr = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
            var sink = new RelationsGriefSink(sr);

            sink.ApplyDispersion(
                "sv_dead", new List<string> { "sv_b", "sv_a", "sv_a", "sv_dead" },
                baseGriefAmount: -8f, DeathQuality.Rushed, day: 30);

            Assert.Equal(8f, GriefOf(sr, "sv_dead", "sv_a"), 3);
            Assert.Equal(8f, GriefOf(sr, "sv_dead", "sv_b"), 3);
            Assert.Equal(2, sink.AppliedSurvivorCount);
            Assert.Equal(1, sink.AppliedEventCount);
        }

        [Theory]
        [InlineData(DeathQuality.Peaceful,   4f)]   // 8 × 0.5
        [InlineData(DeathQuality.Rushed,     8f)]   // 8 × 1.0
        [InlineData(DeathQuality.Unattended, 10f)]  // 8 × 1.25
        public void ApplyDispersion_ScalesByAuthoredDeathQuality(
            DeathQuality quality, float expected)
        {
            var sr = Relations(("sv_dead", "sv_a"));
            var sink = new RelationsGriefSink(sr);

            sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 8f, quality, day: 1);

            Assert.Equal(expected, GriefOf(sr, "sv_dead", "sv_a"), 3);
        }

        [Fact]
        public void ApplyDispersion_UsesMagnitudeNotSignOfTheMoraleChannel()
        {
            var sr = Relations(("sv_dead", "sv_a"));
            var sink = new RelationsGriefSink(sr);

            // The fate path passes GriefMoraleDelta (-8), i.e. a morale loss whose
            // magnitude is the grief base. Grief must not be applied as a negative.
            sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, -8f, DeathQuality.Rushed, 1);
            sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 8f, DeathQuality.Rushed, 2);

            Assert.Equal(16f, GriefOf(sr, "sv_dead", "sv_a"), 3);
        }

        [Fact]
        public void ApplyDispersion_CapsPerSurvivorPerEvent()
        {
            var sr = Relations(("sv_dead", "sv_a"));
            var sink = new RelationsGriefSink(sr);

            sink.ApplyDispersion(
                "sv_dead", new[] { "sv_a" },
                baseGriefAmount: 900f, DeathQuality.Unattended, day: 1);

            Assert.Equal(RelationsGriefSink.MaxGriefPerSurvivorPerEvent,
                GriefOf(sr, "sv_dead", "sv_a"), 3);
        }

        [Fact]
        public void ApplyDispersion_SkipsTheDeceasedAndTheNotAlive()
        {
            var sr = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
            var sink = new RelationsGriefSink(sr, id => id != "sv_b");

            sink.ApplyDispersion(
                "sv_dead", new[] { "sv_a", "sv_b", "sv_dead", "" },
                8f, DeathQuality.Rushed, 1);

            Assert.Equal(8f, GriefOf(sr, "sv_dead", "sv_a"), 3);
            Assert.Equal(0f, GriefOf(sr, "sv_dead", "sv_b"), 3);
            Assert.Equal(1, sink.AppliedSurvivorCount);
        }

        [Fact]
        public void ApplyDispersion_ZeroOrNegativeBase_AppliesNothing()
        {
            var sr = Relations(("sv_dead", "sv_a"));
            var sink = new RelationsGriefSink(sr);

            sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 0f, DeathQuality.Rushed, 1);
            sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, -0f, DeathQuality.Rushed, 2);

            Assert.Equal(0f, GriefOf(sr, "sv_dead", "sv_a"), 3);
            Assert.Equal(2, sink.AppliedEventCount);
            Assert.Equal(0, sink.AppliedSurvivorCount);
        }

        [Fact]
        public void ApplyDispersion_WithoutRelationsIsADocumentedNoOp()
        {
            var sink = new RelationsGriefSink(null);

            var ex = Record.Exception(() =>
                sink.ApplyDispersion("sv_dead", new[] { "sv_a" }, 8f, DeathQuality.Rushed, 1));

            Assert.Null(ex);
        }

        // ── end of chain: memorialize → grief in the ledger ────────────

        [Fact]
        public void Memorialize_RoutesGriefIntoTheRelationshipLedgerOnceOnly()
        {
            var sr = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
            var memorial = new MemorialSystem(new MemorialState())
            {
                GriefSink = new RelationsGriefSink(sr),
            };

            var input = new MemorialInput
            {
                SurvivorId = "sv_dead",
                Cause = "radiation",
                Day = 44,
                BirthDay = 0,
                MoraleDelta = -8f,
                DeathQuality = DeathQuality.Unattended,
                SurvivingRelationshipIds = sr.RelatedIds("sv_dead"),
            };

            var first = memorial.Memorialize(input);
            var repeat = memorial.Memorialize(input);   // idempotent by survivor id

            Assert.Same(first, repeat);
            Assert.Equal(10f, GriefOf(sr, "sv_dead", "sv_a"), 3);  // 8 × 1.25
            Assert.Equal(10f, GriefOf(sr, "sv_dead", "sv_b"), 3);
            // Grief must also pull affinity down — the ledger already models that.
            Assert.True(sr.GetOrCreateRelationship("sv_dead", "sv_a").affinity < 0f);
        }

        [Fact]
        public void GriefApplication_IsOrderIndependent()
        {
            var forward = Relations(("sv_dead", "sv_a"), ("sv_dead", "sv_b"));
            var reverse = Relations(("sv_dead", "sv_b"), ("sv_dead", "sv_a"));

            new RelationsGriefSink(forward).ApplyDispersion(
                "sv_dead", new[] { "sv_a", "sv_b" }, 8f, DeathQuality.Rushed, 9);
            new RelationsGriefSink(reverse).ApplyDispersion(
                "sv_dead", new[] { "sv_b", "sv_a" }, 8f, DeathQuality.Rushed, 9);

            Assert.Equal(
                GriefOf(forward, "sv_dead", "sv_a"),
                GriefOf(reverse, "sv_dead", "sv_a"), 3);
            Assert.Equal(
                GriefOf(forward, "sv_dead", "sv_b"),
                GriefOf(reverse, "sv_dead", "sv_b"), 3);
        }
    }
}
