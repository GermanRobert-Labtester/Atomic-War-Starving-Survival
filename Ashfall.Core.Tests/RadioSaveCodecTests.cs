using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins the radio save contract: one canonical persisted owner, checksummed
    /// envelope, versioned rejection, and a clean round-trip of every
    /// authoritative mutable receiver value (history, played-broadcast dedup keys,
    /// frequency, day).
    /// </summary>
    public class RadioSaveCodecTests
    {
        private static RadioSaveState BuildState()
        {
            var state = new RadioSaveState
            {
                day = 42,
                currentFrequency = 97.5f,
                history = new List<RadioInterceptEntry>
                {
                    new RadioInterceptEntry
                    {
                        factionId = "faction_holdfast", callsign = "HOLDFAST BASE",
                        frequencyMhz = 97.5f, kind = (int)RadioEventKind.InterceptChatter,
                        message = "relay three", signalStrength = 6, day = 42
                    },
                    new RadioInterceptEntry
                    {
                        factionId = "warlords_sector_4", callsign = "TOLL HOUSE RELAY",
                        frequencyMhz = 94.2f, kind = (int)RadioEventKind.RaidWarning,
                        message = "column moving", signalStrength = 3, day = 41
                    }
                },
                playedBroadcastKeys = new List<string>
                {
                    "42:97.50:relay three", "41:94.20:column moving"
                }
            };
            return state;
        }

        [Fact]
        public void RoundTrip_PreservesAllAuthoritativeValues()
        {
            var state = BuildState();
            var json = new SystemTextJsonSerializer();
            string encoded = RadioSaveCodec.Encode(state, json);
            Assert.True(RadioSaveCodec.TryDecode(encoded, json, out var loaded));

            Assert.Equal(42, loaded.day);
            Assert.Equal(97.5f, loaded.currentFrequency);
            Assert.Equal(2, loaded.history.Count);
            Assert.Equal("faction_holdfast", loaded.history[0].factionId);
            Assert.Equal("relay three", loaded.history[0].message);
            Assert.Equal((int)RadioEventKind.RaidWarning, loaded.history[1].kind);
            Assert.Equal(2, loaded.playedBroadcastKeys.Count);
            Assert.Contains("42:97.50:relay three", loaded.playedBroadcastKeys);
        }

        [Fact]
        public void MutatedState_ChangesChecksum_AndIsRejected()
        {
            var state = BuildState();
            var json = new SystemTextJsonSerializer();
            string encoded = RadioSaveCodec.Encode(state, json);

            string tampered = encoded.Replace("relay three", "relay four");
            Assert.NotEqual(encoded, tampered);
            Assert.False(RadioSaveCodec.TryDecode(tampered, json, out _));
        }

        [Fact]
        public void EmptyChecksum_NewFormat_IsRejected()
        {
            var state = BuildState();
            state.Checksum = string.Empty;
            var json = new SystemTextJsonSerializer();
            // A checksummed serialization recomputes the hash, so serialize the
            // raw DTO (no Encode) to simulate a malformed new-format save.
            string raw = json.Serialize(state);
            Assert.False(RadioSaveCodec.TryDecode(raw, json, out _));
        }

        [Fact]
        public void FutureVersion_IsRejected()
        {
            var state = BuildState();
            state.saveVersion = RadioSaveCodec.CurrentSaveVersion + 1;
            state.Checksum = SaveChecksum.Compute(state);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(state);
            Assert.False(RadioSaveCodec.TryDecode(raw, json, out _));
        }

        [Fact]
        public void NoRadioSave_OrGarbage_FallsBackToNull()
        {
            var json = new SystemTextJsonSerializer();
            Assert.False(RadioSaveCodec.TryDecode(null, json, out _));
            Assert.False(RadioSaveCodec.TryDecode(string.Empty, json, out _));
            Assert.False(RadioSaveCodec.TryDecode("not json", json, out _));
            // Bare "{}" is neither a valid old format nor a checksummed new
            // format — rejected, never silently accepted.
            Assert.False(RadioSaveCodec.TryDecode("{}", json, out _));
        }

        [Fact]
        public void DeterministicEncode_SameState_SamePayload()
        {
            var json = new SystemTextJsonSerializer();
            string a = RadioSaveCodec.Encode(BuildState(), json);
            string b = RadioSaveCodec.Encode(BuildState(), json);
            Assert.Equal(a, b);
        }
    }
}
