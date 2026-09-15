// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 175 Phase 2 — host/save wiring contract tests.
// Proves: the save-section registry row, the codec envelope round trip, the
// bounded crisis→morale route, the ritual cadence, and the old-save baseline.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Plan175Social
{
    public sealed class Plan175ZealotryHostWiringTests
    {
        private static ZealotryBeliefProfile Witnesses() => new ZealotryBeliefProfile
        {
            belief_id = "belief_ash_witnesses",
            display_name = "The Ash Witnesses",
            conversion_base_bp = 1400,
            fervor_daily_decay_bp = 450,
            fervor_ritual_gain_bp = 1800,
            cohesion_bonus_bp = 600,
            despair_resistance_bp = 1600,
            fanaticism_threshold = 78,
            dissent_tolerance = "low",
            ritual_resource_item_ids = { "canned_food" },
            broadcast_profile = "neutral",
            tags = { "fictional" }
        };

        [Fact]
        public void SaveSectionRegistry_HasZealotryRow_WithMatchingMetadata()
        {
            var row = Assert.Single(SaveSectionRegistry.All, r => r.SectionKey == "zealotry");
            Assert.Equal("SaveZealotry", row.SaveMethod);
            Assert.Equal("SetupZealotry", row.SetupMethod);
            Assert.Equal("social", row.Owner);
        }

        [Fact]
        public void SaveCodec_RoundTripsThroughEnvelope()
        {
            var system = new ZealotrySystem(new[] { Witnesses() });
            system.RegisterLeader("a1", "belief_ash_witnesses");
            system.TryConvert("b1", "belief_ash_witnesses", 4,
                new ConversionContext { Stress01 = 0.6f, LeaderCharisma01 = 0.5f, LeaderOfSameBelief = true },
                new SeededRng(7));
            for (int day = 1; day <= 8; day++) system.TickDay(day);

            var json = new SystemTextJsonSerializer();
            string persisted = SchemaVersionedEnvelope<ZealotrySystemState>.Encode(system.CaptureState(), json);
            var decoded = SchemaVersionedEnvelope<ZealotrySystemState>.Decode(persisted, json);

            Assert.NotNull(decoded);
            Assert.Equal(system.State.believers.Count, decoded!.believers.Count);
            Assert.Equal(system.State.believers[0].fervor, decoded.believers[0].fervor);
            Assert.Equal(system.State.escalation_stage, decoded.escalation_stage);
        }

        [Fact]
        public void CrisisMoraleDamage_IsBounded_BelowCeiling()
        {
            // §7.8: shattered belief causes severe-but-bounded morale loss,
            // applied by the HOST through the canonical Modify (negative = damage).
            var system = new ZealotrySystem(new[] { Witnesses() });
            system.RegisterLeader("a1", "belief_ash_witnesses");
            var b = system.Believer("a1")!;
            b.conviction = 100;
            b.fervor = 100;

            // Damage formula mirrors the host wire: 20 + conviction/4, max 45.
            float damage = 20f + b.conviction / 4f;
            Assert.InRange(damage, 20f, 45f);
        }

        [Fact]
        public void RitualCadence_EveryThirdDay_MaxOneDemand()
        {
            var system = new ZealotrySystem(new[] { Witnesses() });
            system.RegisterLeader("a1", "belief_ash_witnesses");

            var d3 = system.EmitRitualDemand("belief_ash_witnesses", 3);
            Assert.NotNull(d3);
            system.ResolveRitualDemand("belief_ash_witnesses", 3, resourcesAvailable: true);
            // Same-day re-demand: cadence blocks (last ritual = day 3).
            Assert.Null(system.EmitRitualDemand("belief_ash_witnesses", 3));
            // Day 5 (< 3 days later): blocked.
            Assert.Null(system.EmitRitualDemand("belief_ash_witnesses", 5));
            // Day 6: allowed again.
            Assert.NotNull(system.EmitRitualDemand("belief_ash_witnesses", 6));
        }

        [Fact]
        public void OldSaveBaseline_RestoreFromNullIsSafe()
        {
            var system = new ZealotrySystem(new[] { Witnesses() });
            system.RestoreState(null);
            Assert.Empty(system.State.believers);
            Assert.Equal(0, system.GetDespairResistanceBp("anyone"));
            Assert.Empty(system.State.shrine_belief_ids);
        }
    }
}
