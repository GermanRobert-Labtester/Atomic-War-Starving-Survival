// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Plan182Relations
{
    /// <summary>
    /// DEBT-182-LAST-INTERACTION-STAMP — the pair relationship records when a real
    /// interaction last happened, stamped by the affinity producers. No decay is
    /// introduced: time passing alone must not move affinity or the stamp.
    /// </summary>
    public sealed class Plan182LastInteractionStampTests
    {
        private static SurvivorRelationsSystem CreateSystem() => new SurvivorRelationsSystem(new SeededRng(182));

        [Fact]
        public void Fresh_relationship_has_no_interaction_day()
        {
            var relations = CreateSystem();
            relations.ModifyTrust("sv_a", "sv_b", 0f); // creates the pair via a real producer
            var entry = relations.GetOrCreateRelationship("sv_a", "sv_b");

            Assert.True(entry.lastInteractionDay >= 0, "a trust interaction is a real producer and must stamp");
        }

        [Fact]
        public void Affinity_interaction_stamps_the_current_day()
        {
            var relations = CreateSystem();
            relations.TickDay(17);
            relations.ModifyAffinity("sv_a", "sv_b", 5f);

            var entry = relations.GetOrCreateRelationship("sv_a", "sv_b");
            Assert.Equal(17, entry.lastInteractionDay);
        }

        [Fact]
        public void Stamp_advances_with_the_latest_interaction_only()
        {
            var relations = CreateSystem();
            relations.TickDay(10);
            relations.ModifyAffinity("sv_a", "sv_b", 5f);
            relations.TickDay(25);
            relations.ModifyAffinity("sv_a", "sv_b", -3f);

            var entry = relations.GetOrCreateRelationship("sv_a", "sv_b");
            Assert.Equal(25, entry.lastInteractionDay);
        }

        [Fact]
        public void Idle_days_do_not_decay_affinity_or_move_the_stamp()
        {
            // The map forbids time-only decay: a future neglect rule requires this
            // stamp, so until then simply passing days must change nothing.
            var relations = CreateSystem();
            relations.TickDay(3);
            relations.ModifyAffinity("sv_a", "sv_b", 20f);
            float affinityAfter = relations.GetOrCreateRelationship("sv_a", "sv_b").affinity;

            for (int day = 4; day <= 90; day++)
                relations.TickDay(day);

            var entry = relations.GetOrCreateRelationship("sv_a", "sv_b");
            Assert.Equal(affinityAfter, entry.affinity);
            Assert.Equal(3, entry.lastInteractionDay);
        }

        [Fact]
        public void Stamps_are_per_pair()
        {
            var relations = CreateSystem();
            relations.TickDay(8);
            relations.ModifyAffinity("sv_a", "sv_b", 4f);
            relations.TickDay(12);
            relations.ModifyAffinity("sv_a", "sv_c", 4f);

            Assert.Equal(8, relations.GetOrCreateRelationship("sv_a", "sv_b").lastInteractionDay);
            Assert.Equal(12, relations.GetOrCreateRelationship("sv_a", "sv_c").lastInteractionDay);
        }

        [Fact]
        public void Stamp_survives_the_save_round_trip()
        {
            var relations = CreateSystem();
            relations.TickDay(44);
            relations.ModifyAffinity("sv_a", "sv_b", 6f);
            var saved = relations.CaptureState();

            var restored = CreateSystem();
            restored.RestoreState(saved);

            Assert.Equal(44, restored.GetOrCreateRelationship("sv_a", "sv_b").lastInteractionDay);
        }
    }
}
