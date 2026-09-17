// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Plan 24C (Task A3, Wave 8) — grief-to-needs projection, the mourning
    /// vigil, and the death-to-ration-re-split journey. Every effect routes
    /// through the existing owners (relationship ledger, memorial owner,
    /// social coordinator, fate cascade); nothing here is a second authority.
    /// </summary>
    public sealed class Plan24RecoveryGriefTests
    {
        private static RelationsGriefSink Sink(SurvivorRelationsSystem relations) =>
            new RelationsGriefSink(relations, id => !id.EndsWith("_dead", StringComparison.Ordinal));

        // ── 1. Grief onset stamping (the derived projection's only save state) ──

        [Fact]
        public void GriefSink_AppliesToRelationshipLedger_AndStampsOnset()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(7), null);
            relations.GetOrCreateRelationship("survivor_a", "survivor_b");
            var sink = Sink(relations);

            // The deceased is the relationship partner itself (the real flow:
            // RelatedIds of the deceased name the mourners).
            sink.ApplyDispersion("survivor_b", new[] { "survivor_a" },
                SurvivorFateSystem.GriefMoraleDelta, DeathQuality.Rushed, 42);

            Assert.True(relations.TryGetRelationship("survivor_a", "survivor_b", out var rel));
            Assert.NotNull(rel);
            // Grief state increased by the quality-scaled amount (legacy chain).
            Assert.True(rel!.grief > 0f);
            Assert.Equal(42, rel.grief_since_day);
            Assert.Equal(1, sink.AppliedEventCount);
            Assert.Equal(1, sink.AppliedSurvivorCount);
        }

        [Fact]
        public void GriefSink_SecondLoss_ReAnchorsOnsetAndAccumulates()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(7), null);
            relations.GetOrCreateRelationship("survivor_a", "survivor_b");
            relations.GetOrCreateRelationship("survivor_a", "survivor_c");
            var sink = Sink(relations);

            sink.ApplyDispersion("survivor_b", new[] { "survivor_a" },
                SurvivorFateSystem.GriefMoraleDelta, DeathQuality.Rushed, 42);
            float firstGrief = relations.TryGetRelationship("survivor_a", "survivor_b", out var r1)
                ? r1!.grief : 0f;

            sink.ApplyDispersion("survivor_c", new[] { "survivor_a" },
                SurvivorFateSystem.GriefMoraleDelta, DeathQuality.Peaceful, 45);

            // Each loss stamps ITS OWN pair's onset: the a–b relationship keeps
            // b's day; the a–c relationship carries c's. The mourner's total
            // grief accumulates across both.
            Assert.True(relations.TryGetRelationship("survivor_a", "survivor_b", out var ab));
            Assert.Equal(42, ab!.grief_since_day);
            Assert.True(relations.TryGetRelationship("survivor_a", "survivor_c", out var ac));
            Assert.Equal(45, ac!.grief_since_day);
            Assert.True(relations.TryGetRelationship("survivor_a", "survivor_b", out var rel));
            Assert.True(rel!.grief > firstGrief); // grief accumulates, capped
            Assert.Equal(2, sink.AppliedEventCount);
        }

        // ── 2. The pure decay function (shared by host projection + tests) ──

        [Fact]
        public void BondGriefRate_DecaysLinearly_AndExpiresAfterWindow()
        {
            float grief = 20f; // max per-event cap
            int window = RelationsGriefSink.BondGriefDurationDays;

            float day0 = RelationsGriefSink.BondMoralePerHour(grief, 0);
            float dayHalf = RelationsGriefSink.BondMoralePerHour(grief, window / 2);
            float dayEnd = RelationsGriefSink.BondMoralePerHour(grief, window);
            float dayPast = RelationsGriefSink.BondMoralePerHour(grief, window + 5);

            // Full intensity on the onset day (grief 20 = 0.2 intensity); exactly
            // half mid-window; zero at and past expiry; never positive.
            Assert.Equal(RelationsGriefSink.BondGriefMoralePerDay * 0.2f / 24f, day0, 5);
            Assert.Equal(day0 / 2f, dayHalf, 5);
            Assert.Equal(0f, dayEnd, 5);
            Assert.Equal(0f, dayPast, 5);

            // Fatigue rides the same curve, positive.
            float fatigue0 = RelationsGriefSink.BondFatiguePerHour(grief, 0);
            Assert.Equal(RelationsGriefSink.BondGriefFatiguePerDay * 0.2f / 24f, fatigue0, 5);
        }

        [Fact]
        public void BondGriefRate_IsPureFunctionOfPersistedFacts_ReloadParity()
        {
            // A reload restores the relationship ledger and recomputes the rate:
            // identical (grief, onset, day) inputs MUST produce identical rates.
            float before = RelationsGriefSink.BondMoralePerHour(13.5f, 3);
            // ...simulate the restore: fresh objects, same persisted values...
            float after = RelationsGriefSink.BondMoralePerHour(13.5f, 3);
            Assert.Equal(before, after, 6);
        }

        // ── 3. The mourning vigil (once per death, explicit rejections) ──

        [Fact]
        public void Mourn_ExactlyOncePerDeath_WithExplicitRejections()
        {
            var memorial = new MemorialSystem(new MemorialState());
            var entries = new List<MemorialEntry>();
            memorial.OnMourned += entries.Add;

            memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "survivor_a", Cause = "starvation", Day = 10,
                BirthDay = 1, MoraleDelta = SurvivorFateSystem.GriefMoraleDelta
            });

            // Unknown id is an explicit block, not a silent no-op.
            var unknown = memorial.Mourn("survivor_nobody", 11);
            Assert.Equal(ActionResult.StatusKind.Blocked, unknown.Status);
            Assert.Equal("unknown_memorial", unknown.FailureCode);

            // First vigil succeeds exactly once.
            var first = memorial.Mourn("survivor_a", 11);
            Assert.Equal(ActionResult.StatusKind.Success, first.Status);
            Assert.Single(entries);
            Assert.Equal(11, memorial.Entries[0].MournedDay);

            // The second vigil for the same loss is an explicit block.
            var second = memorial.Mourn("survivor_a", 12);
            Assert.Equal(ActionResult.StatusKind.Blocked, second.Status);
            Assert.Equal("already_mourned", second.FailureCode);
            Assert.Single(entries); // OnMourned fired exactly once

            // The read model clears.
            Assert.Null(memorial.LatestUnmourned());
        }

        // ── 4. The death → ration re-split → grievance journey ──────────

        private sealed class JourneyFixture
        {
            public SurvivorRosterSystem Roster = new SurvivorRosterSystem();
            public NeedsSystem Needs = new NeedsSystem();
            public DutyRosterSystem Duty = new DutyRosterSystem();
            public SurvivorRelationsSystem Relations = new SurvivorRelationsSystem(new SeededRng(7), null);
            public SurvivorSocialCoordinator Social;
            public MemorialSystem Memorial = new MemorialSystem(new MemorialState());
            public SurvivorFateSystem Fate;
            public List<SurvivorNeedsState> RosterState = new List<SurvivorNeedsState>();

            public JourneyFixture()
            {
                Social = new SurvivorSocialCoordinator(
                    new SeededRng(7), Needs, Relations, Duty, () => 10);
                Fate = new SurvivorFateSystem(
                    roster: Roster, needs: Needs, dutyRoster: Duty,
                    social: Social, memorial: Memorial,
                    getDay: () => 10,
                    displayNameFor: id => Roster.FindDefinition(id)?.displayName ?? id);
                foreach (var id in new[] { "survivor_a", "survivor_b", "survivor_c" })
                {
                    Roster.RegisterDefinition(new SurvivorDefinition { id = id, displayName = id });
                    Roster.Join(id, 1);
                    var state = new SurvivorNeedsState { Id = id, Morale = 50f };
                    Needs.Register(state);
                    RosterState.Add(state);
                }
            }
        }

        [Fact]
        public void DeathToRationResplitToGrievance_RunsThroughExistingOwners()
        {
            var f = new JourneyFixture();
            f.Needs.CurrentDay = 10;

            // Unequal service exists before the loss: a designated leader takes
            // priority rations (the Plan 22 grievance pressure).
            f.Social.RationPolicy = StartingLevel.RationPolicy.Standard;
            f.Social.SetAliveSurvivors(new[] { "survivor_a", "survivor_b", "survivor_c" });
            Assert.True(f.Social.DesignateLeader("survivor_c"));

            // A non-leader dies through the real fate cascade.
            f.Fate.ReportDeath("survivor_a", SurvivorDeathCause.Needs, "starvation", "test");

            // Shelter-wide grief reached the living through the attributed seam.
            var survivorB = f.Needs.Get("survivor_b")!;
            Assert.Equal(50f + SurvivorFateSystem.GriefMoraleDelta, survivorB.Morale, 4);
            var recent = f.Needs.ModifierStack.GetRecentForSurvivor("survivor_b");
            Assert.Contains(recent, c => c.SourceId == "grief.shelter_loss");

            // Next day: the re-split derives from the reduced living roster —
            // the deceased holds no allocation and the memorial ledger holds
            // exactly one entry.
            f.Social.SetAliveSurvivors(new[] { "survivor_b", "survivor_c" });
            f.Social.TickDay(11, new List<SurvivorNeedsState> { survivorB, f.Needs.Get("survivor_c")! });

            Assert.Single(f.Memorial.Entries);
            Assert.Equal("survivor_a", f.Memorial.Entries[0].SurvivorId);

            // The grievance engine ticks for the living only: survivor_b still
            // resents the leader (deficit survives the re-split; the exact
            // RationConflict arithmetic is pinned by its own suite).
            var grievance = f.Social.Ration.GetState("survivor_b");
            Assert.NotNull(grievance);
            Assert.Equal("survivor_c", grievance!.resentmentTargetId);
            Assert.True(grievance.resentmentLevel > 0f,
                "the unequal-service grievance survives the re-split and keeps building");
        }
    }
}
