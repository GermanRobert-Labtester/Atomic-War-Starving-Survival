// SPDX-License-Identifier: MIT
// Expansion 25 — rail track maintenance ledger (stateful owner over DEC-80).

using System;
using Ashfall.Core.Rail;
using Xunit;

namespace Ashfall.Core.Tests.Rail
{
    public sealed class RailTrackMaintenanceLedgerTests
    {
        [Fact]
        public void SeedFromTopology_IsIdempotentAndPreservesExistingState()
        {
            var ledger = new RailTrackMaintenanceLedger();
            Assert.Equal(2, ledger.SeedFromTopology(new[] { ("s1", true, 200f), ("s2", false, 120f) }));

            ledger.ApplyRun("s1", LocomotiveClass.DieselFreightRig, 60);
            int wear = ledger.Segments["s1"].TrackWearPermille;

            // Re-seeding must not reset a maintained/used segment; it may seed new ones.
            Assert.Equal(1, ledger.SeedFromTopology(new[] { ("s1", true, 200f), ("s3", false, 90f) }));
            Assert.Equal(wear, ledger.Segments["s1"].TrackWearPermille);
            Assert.Equal(3, ledger.SegmentCount);
        }

        [Fact]
        public void Census_CountsBlockedAndDegradedSegments()
        {
            var ledger = new RailTrackMaintenanceLedger();
            ledger.SeedFromTopology(new[] { ("s1", false, 150f), ("s2", false, 150f) });
            ledger.SetBlocked("s1", true);
            var spread = ledger.EnsureSegment("s2");
            spread.GaugeStability = TrackGaugeStability.MinorSpread;

            var census = ledger.GetCensus();
            Assert.Equal(2, census.SegmentCount);
            Assert.Equal(1, census.BlockedSegments);
            Assert.Equal(1, census.DegradedSegments);
        }

        [Fact]
        public void CaptureRestore_RoundTripsAndSchemaGates()
        {
            var ledger = new RailTrackMaintenanceLedger();
            ledger.SeedFromTopology(new[] { ("s1", true, 200f) });
            ledger.ApplyRun("s1", LocomotiveClass.ArmoredBattleTrain, 100);
            var state = ledger.CaptureState();

            var restored = new RailTrackMaintenanceLedger();
            restored.RestoreState(state);
            Assert.Equal(state.Segments["s1"].TrackWearPermille, restored.Segments["s1"].TrackWearPermille);

            var newer = ledger.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));

            var legacy = ledger.CaptureState();
            legacy.SchemaVersion = 0;
            restored.RestoreState(legacy);
            Assert.Equal(1, restored.CaptureState().SchemaVersion);
        }

        [Fact]
        public void Maintain_ClearsDebrisAndRestoresGauge()
        {
            var ledger = new RailTrackMaintenanceLedger();
            var seg = ledger.EnsureSegment("s1");
            seg.TrackWearPermille = 900;
            seg.GaugeStability = TrackGaugeStability.SevereDistortion;
            seg.IsBlockedByDebris = true;

            Assert.True(ledger.Maintain("s1", 1000, 100));
            Assert.False(seg.IsBlockedByDebris);
            Assert.Equal(TrackGaugeStability.MinorSpread, seg.GaugeStability);
            Assert.True(seg.TrackWearPermille < 900);
        }

        [Fact]
        public void Evaluate_IsReadOnlyAndNeverMutates()
        {
            var ledger = new RailTrackMaintenanceLedger();
            var seg = ledger.EnsureSegment("s1");
            seg.MaxBridgeLoadTons = 50;
            int before = seg.TrackWearPermille;

            var result = ledger.Evaluate("s1", LocomotiveClass.DieselFreightRig, 40);
            Assert.Equal(RailFeasibilityOutcome.BridgeLoadRefusal, result.Outcome);
            Assert.Equal(before, seg.TrackWearPermille);
        }
    }
}
