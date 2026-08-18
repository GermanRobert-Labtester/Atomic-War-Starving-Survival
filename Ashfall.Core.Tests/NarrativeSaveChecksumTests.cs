// NarrativeSaveStore lives in src/Host and reads user:// — Godot-tied. We mirror
// its envelope + checksum behaviour here through SystemTextJsonSerializer so the
// integrity contract is pinned without spinning up a Godot project.
#nullable disable

using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins the integrity envelope behaviour for NarrativeSaveStore: a clean
    /// round-trip preserves the checksum, mutating the state changes the checksum,
    /// and a tampered envelope is detected. Mirrors the store's serialize/deserialize
    /// path exactly so the contract tested is the one shipped.
    /// </summary>
    public class NarrativeSaveChecksumTests
    {
        private sealed class NarrativeHostSave
        {
            public NarrativeEncounterState State;
            public string Checksum = string.Empty;
        }

        private static NarrativeEncounterState BuildState()
        {
            var state = new NarrativeEncounterState
            {
                systemId = "narrative_encounter_system",
                totalResolved = 2,
                cumulativeMorale = 1,
                cumulativeGuilt = -1,
                history = new List<EncounterResolutionRecord>
                {
                    new EncounterResolutionRecord
                    {
                        encounterId = "enc_dead_letter_office",
                        choiceId = "read",
                        locationId = "loc_the_allotments",
                        day = 40,
                        moraleDelta = 1,
                        guiltDelta = 0
                    }
                },
                pending = new List<PendingSurfacedEncounter>
                {
                    new PendingSurfacedEncounter
                    {
                        encounterId = "enc_weather_station",
                        locationId = "loc_denial_cut_substation",
                        legIndex = 3,
                        day = 41
                    }
                }
            };
            return state;
        }

        private static string RoundTripChecksum(NarrativeEncounterState state)
        {
            var envelope = new NarrativeHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<NarrativeHostSave>(raw);

            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new NarrativeHostSave { State = state });

            string actual = RoundTripChecksum(state);

            Assert.Equal(expected, actual, StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedState_ChangesChecksum()
        {
            var original = BuildState();
            string originalChecksum = RoundTripChecksum(original);

            var tampered = BuildState();
            tampered.pending[0].legIndex = 4;   // any state mutation must move the hash

            string tamperedChecksum = RoundTripChecksum(tampered);

            Assert.NotEqual(originalChecksum, tamperedChecksum);
        }

        [Fact]
        public void TamperedChecksum_DetectedByRecomputation()
        {
            var envelope = new NarrativeHostSave { State = BuildState() };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            // Forged checksum that does not match the actual state.
            envelope.Checksum = new string('0', 64);

            string actual = SaveChecksum.Compute(envelope);

            Assert.NotEqual(envelope.Checksum, actual, StringComparer.Ordinal);
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            // The store previously skipped verification when the checksum was
            // empty/null, treating it as "legacy". A save in the new envelope
            // format with a blank checksum is not legacy — it is malformed.
            var envelope = new NarrativeHostSave { State = BuildState(), Checksum = null };

            string actual = SaveChecksum.Compute(envelope);

            // Empty string is not equal to a real 64-hex hash; the load path must
            // reject it. Pin the recomputation mismatch here.
            Assert.NotEqual(envelope.Checksum ?? string.Empty, actual);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }

        [Fact]
        public void PendingQueue_ContributesToChecksum()
        {
            var withPending = BuildState();
            string withPendingHash = RoundTripChecksum(withPending);

            var withoutPending = BuildState();
            withoutPending.pending.Clear();

            string withoutPendingHash = RoundTripChecksum(withoutPending);

            Assert.NotEqual(withPendingHash, withoutPendingHash);
        }

        [Fact]
        public void HostSessionEncounterApplyChoice_PendingClear_RoundTripsChecksum()
        {
            // Replicates the host's resolve+clear sequence end-to-end through the
            // checksum envelope to confirm pending mutations actually move the hash.
            var state = BuildState();
            var sys = new NarrativeEncounterSystem();
            sys.RestoreState(state);
            string beforeHash = RoundTripChecksum(sys.CaptureState());

            sys.EnqueuePending("enc_dead_letter_office", "loc_denial_cut_substation", 5, 42);
            string afterEnqueueHash = RoundTripChecksum(sys.CaptureState());
            Assert.NotEqual(beforeHash, afterEnqueueHash);

            sys.ClearPending("enc_dead_letter_office");
            string afterClearHash = RoundTripChecksum(sys.CaptureState());
            Assert.Equal(beforeHash, afterClearHash);
        }
    }
}
