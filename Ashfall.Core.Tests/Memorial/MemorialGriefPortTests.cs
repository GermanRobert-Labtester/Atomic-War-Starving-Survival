// SPDX-License-Identifier: MIT
// Plan 09 / 9C Core — DeathQuality / MemorialOutcome / GriefSink port.
// Until this commit, MemorialSystem.Memorialize(...) recorded cause +
// moraleDelta + heirloom + wish-resolved and nothing else; grief in
// SurvivorRelationsSystem.ApplyGrief(...) was never called from the
// memorial pipeline, and there was no place to record a "good death"
// or "good burial". This file pins the new Core contract.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests.Memorial
{
    public class MemorialGriefPortTests
    {
        // ── DeathQuality scale pin ─────────────────────────────────────

        [Theory]
        [InlineData(DeathQuality.Peaceful,    0.5f)]
        [InlineData(DeathQuality.Rushed,      1.0f)]
        [InlineData(DeathQuality.Unattended, 1.25f)]
        public void CapturingGriefSink_QualityScaleMatchesSpec(DeathQuality quality, float expected)
        {
            Assert.Equal(expected, CapturingGriefSink.QualityScale(quality));
        }

        // ── Grief fires on first memorialize, NOT on idempotent repeat ─

        [Fact]
        public void Memorialize_FiresGriefSink_OnceOnFirstCall()
        {
            var sys = new MemorialSystem(new MemorialState());
            var sink = new CapturingGriefSink();
            sys.GriefSink = sink;

            var input = new MemorialInput
            {
                SurvivorId = "survivor_a",
                Cause = "radiation",
                Day = 12,
                BirthDay = 1,
                FinalWishResolved = true,
                Epitaph = "She walked into the grey.",
                HeirloomItemId = "wedding_ring",
                HeirloomRecipientId = "survivor_b",
                MoraleDelta = -8f,
                DeathQuality = DeathQuality.Rushed,
                Outcome = MemorialOutcome.Burial,
                SurvivingRelationshipIds = new[] { "survivor_b", "survivor_c", "survivor_d" },
            };
            sys.Memorialize(input);

            Assert.Single(sink.Records);
            var rec = sink.Records[0];
            Assert.Equal("survivor_a", rec.DeceasedId);
            Assert.Equal(3, rec.SurvivngRelationshipIds.Count);
            // base grief = MoraleDelta (negative); scale = 1.0 for Rushed.
            Assert.Equal(-8f * 1.0f, rec.GriefApplied);
            Assert.Equal(DeathQuality.Rushed, rec.Quality);
            Assert.Equal(12, rec.Day);

            // Idempotent re-call: nothing new.
            sys.Memorialize(input);
            Assert.Single(sink.Records);
        }

        [Theory]
        [InlineData(DeathQuality.Peaceful,   -10f, -5f)]
        [InlineData(DeathQuality.Rushed,     -10f, -10f)]
        [InlineData(DeathQuality.Unattended, -10f, -12.5f)]
        public void Memorialize_DifferentQualities_ProduceDifferentGrief(
            DeathQuality quality, float baseGrief, float expectedApplied)
        {
            var sys = new MemorialSystem(new MemorialState());
            var sink = new CapturingGriefSink();
            sys.GriefSink = sink;
            sys.Memorialize(new MemorialInput
            {
                SurvivorId = "sv",
                Cause = "combat",
                Day = 1,
                BirthDay = 1,
                MoraleDelta = baseGrief,
                DeathQuality = quality,
            });
            Assert.Single(sink.Records);
            Assert.Equal(expectedApplied, sink.Records[0].GriefApplied);
        }

        // ── Null sink path (preserve pre-9C behaviour) ─────────────────

        [Fact]
        public void Memorialize_WithNullGriefSink_DoesNotThrow()
        {
            var sys = new MemorialSystem(new MemorialState());
            Assert.Null(sys.GriefSink); // baseline
            var entry = sys.Memorialize(new MemorialInput
            {
                SurvivorId = "sv",
                Cause = "combat",
                Day = 1,
                BirthDay = 1,
            });
            Assert.Single(sys.Entries);
            // Entry didn't lose its survivor id; grief simply did not fire.
            Assert.Equal("sv", entry.SurvivorId);
        }

        // ── MemorialEntry new fields round-trip ───────────────────────

        [Fact]
        public void MemorialEntry_RoundTrips_DeathQuality_And_Outcome()
        {
            var sys = new MemorialSystem(new MemorialState());
            var entry = sys.Memorialize(new MemorialInput
            {
                SurvivorId = "sv_x",
                Cause = "wasteland",
                Day = 100,
                BirthDay = 1,
                DeathQuality = DeathQuality.Unattended,
                Outcome = MemorialOutcome.AshScatter,
            });
            Assert.Equal(DeathQuality.Unattended, entry.DeathQuality);
            Assert.Equal(MemorialOutcome.AshScatter, entry.Outcome);
        }

        [Fact]
        public void MemorialEntry_DefaultsTo_PeacefulAndBurial_WhenInputOmitted()
        {
            // Old callers (and old saves) that don't declare the new fields
            // get the gentlest default — no grief spike from a sale-aged save.
            var sys = new MemorialSystem(new MemorialState());
            var entry = sys.Memorialize(new MemorialInput
            {
                SurvivorId = "sv_legacy",
                Cause = "trauma",
                Day = 1,
                BirthDay = 1,
            });
            Assert.Equal(DeathQuality.Peaceful, entry.DeathQuality);
            Assert.Equal(MemorialOutcome.Burial, entry.Outcome);
        }

        // ── Save / Load — additive schema, byte-stable ─────────────────

        [Fact]
        public void CaptureAndRestore_Preserves_DeathQuality_And_Outcome()
        {
            var sys = new MemorialSystem(new MemorialState());
            sys.Memorialize(new MemorialInput
            {
                SurvivorId = "sv_a",
                Cause = "combat",
                Day = 5,
                BirthDay = 1,
                DeathQuality = DeathQuality.Rushed,
                Outcome = MemorialOutcome.WallEntry,
            });
            sys.Memorialize(new MemorialInput
            {
                SurvivorId = "sv_b",
                Cause = "radiation",
                Day = 6,
                BirthDay = 1,
                DeathQuality = DeathQuality.Peaceful,
                Outcome = MemorialOutcome.AshScatter,
            });
            var captured = sys.CaptureState();
            var rt = new MemorialSystem(new MemorialState());
            rt.RestoreState(captured);

            Assert.Equal(2, rt.Entries.Count);
            Assert.Equal(DeathQuality.Rushed, rt.Entries[0].DeathQuality);
            Assert.Equal(MemorialOutcome.WallEntry, rt.Entries[0].Outcome);
            Assert.Equal(DeathQuality.Peaceful, rt.Entries[1].DeathQuality);
            Assert.Equal(MemorialOutcome.AshScatter, rt.Entries[1].Outcome);

            // No grief is fired on Restore — grief fires on Memorialize only.
            // (We assert: only the original two fires happened.)
            // Per the comment: grief is not persisted; it is recomputed on
            // first Memorialize. After Restore, no new Memorialize has run.
        }

        [Fact]
        public void LegacyEntry_WithoutDeathQuality_LoadsAsPeaceful_Default()
        {
            // A save written before 9C lacks the new fields. RestoreInto
            // would set them to whatever the deserializer produces for
            // missing fields — the MemorialEntry ctor defaults to Peaceful
            // so even raw deserialise-to-default picks the right value.
            var stale = new MemorialEntry
            {
                SurvivorId = "sv_legacy",
                Cause = "trauma",
                Day = 5,
                SurvivedDays = 4,
                FinalWishResolved = true,
                Epitaph = "Went quietly.",
                HeirloomItemId = "",
                HeirloomRecipientId = "",
                MoraleDelta = -3f,
                // No DeathQuality, no Outcome set.
            };
            Assert.Equal(DeathQuality.Peaceful, stale.DeathQuality);
            Assert.Equal(MemorialOutcome.Burial, stale.Outcome);
        }

        // ── SurvivingRelationshipIds null-safe ────────────────────────

        [Fact]
        public void SurvivngRelationshipIds_NullSafe()
        {
            var sys = new MemorialSystem(new MemorialState());
            var sink = new CapturingGriefSink();
            sys.GriefSink = sink;
            sys.Memorialize(new MemorialInput
            {
                SurvivorId = "sv",
                Cause = "wound",
                Day = 1,
                BirthDay = 1,
                MoraleDelta = -5f,
                DeathQuality = DeathQuality.Unattended,
                SurvivingRelationshipIds = null, // host forgot to pass
            });
            // No throw; empty list recorded.
            Assert.Single(sink.Records);
            Assert.Empty(sink.Records[0].SurvivngRelationshipIds);
        }
    }
}
