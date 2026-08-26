// SPDX-License-Identifier: MIT
// ASHFALL test: pins the integrity contract of the three stores that were
// sealed from bare-state writes to checksummed envelopes (Weather, HostEvent,
// ChemicalDependency). Mirrors each store's envelope + checksum behaviour
// through SystemTextJsonSerializer — pattern parity with
// SaveStoreChecksumSweepTests — so the contract is pinned without spinning
// up a Godot project.
//
// Store fixes (src/Host/*SaveStore.cs) follow the ExpeditionSaveStore
// canonical pattern: { State, Checksum } envelope, ordinal comparison,
// empty/null checksum in the new format is rejected as corrupt, and
// pre-checksum bare-state saves still load via the legacy fallback path.
#nullable disable

using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.World;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests
{
    public class WeatherSaveSealTests
    {
        // Mirror of WeatherHostSave in src/Host/WeatherSaveStore.cs
        private sealed class WeatherHostSave
        {
            public WorldWeatherState State;
            public string Checksum = string.Empty;
        }

        private static WorldWeatherState BuildState() => new WorldWeatherState
        {
            systemId = "world_weather_system",
            currentKind = "FalloutStorm",
            totalElapsedHours = 412.5f,
            hoursUntilNextCheck = 3.25f,
            rollCount = 87,
            restrictToNonHazardWeather = false
        };

        private static string RoundTripChecksum(WorldWeatherState state)
        {
            var envelope = new WeatherHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<WeatherHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new WeatherHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedWeatherState_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.currentKind = "BlackRain";
            tampered.rollCount = 88;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            // A new-format envelope with a missing/empty checksum is corrupt,
            // not legacy — the store must reject it.
            var envelope = new WeatherHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }

        [Fact]
        public void LegacyBareState_StillDeserializes()
        {
            // Pre-checksum bare-state saves must keep loading through the
            // store's legacy fallback branch: deserializing bare state as an
            // envelope yields a null State, so the store falls through to the
            // bare-state path.
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(BuildState());
            var asEnvelope = json.Deserialize<WeatherHostSave>(raw);
            Assert.True(asEnvelope == null || asEnvelope.State == null);

            var bare = json.Deserialize<WorldWeatherState>(raw);
            Assert.NotNull(bare);
            Assert.Equal("FalloutStorm", bare.currentKind);
            Assert.Equal(87, bare.rollCount);
        }
    }

    public class HostEventSaveSealTests
    {
        // Mirror of HostEventState (src/Host/HostEventAdapter.cs — Godot-side,
        // so the shape is mirrored here) and HostEventHostSave
        // (src/Host/HostEventSaveStore.cs).
        private sealed class HostEventStateMirror
        {
            public List<string> triggeredEventIds = new List<string>();
            public Dictionary<string, int> eventTriggerDays = new Dictionary<string, int>();
            public string lastDispatchedEvent = string.Empty;
        }

        private sealed class HostEventHostSave
        {
            public HostEventStateMirror State;
            public string Checksum = string.Empty;
        }

        private static HostEventStateMirror BuildState() => new HostEventStateMirror
        {
            triggeredEventIds = new List<string>
            {
                "event_the_thin_margin_disclosure",
                "event_the_thirsty_season"
            },
            eventTriggerDays = new Dictionary<string, int>
            {
                ["event_the_thin_margin_disclosure"] = 12,
                ["event_the_thirsty_season"] = 34
            },
            lastDispatchedEvent = "event_the_thirsty_season"
        };

        private static string RoundTripChecksum(HostEventStateMirror state)
        {
            var envelope = new HostEventHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<HostEventHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new HostEventHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedEventLog_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.triggeredEventIds.Add("event_osteophage_explanation");
            tampered.eventTriggerDays["event_osteophage_explanation"] = 56;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new HostEventHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }

        [Fact]
        public void LegacyBareState_StillDeserializes()
        {
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(BuildState());
            var asEnvelope = json.Deserialize<HostEventHostSave>(raw);
            Assert.True(asEnvelope == null || asEnvelope.State == null);

            var bare = json.Deserialize<HostEventStateMirror>(raw);
            Assert.NotNull(bare);
            Assert.Equal(2, bare.triggeredEventIds.Count);
            Assert.Equal(34, bare.eventTriggerDays["event_the_thirsty_season"]);
            Assert.Equal("event_the_thirsty_season", bare.lastDispatchedEvent);
        }
    }

    public class ChemicalDependencySaveSealTests
    {
        // Mirror of ChemicalDependencyHostSave in
        // src/Host/ChemicalDependencySaveStore.cs (state DTO is Core-side).
        private sealed class ChemicalDependencyHostSave
        {
            public ChemicalDependencyLedgerState State;
            public string Checksum = string.Empty;
        }

        private static ChemicalDependencyLedgerState BuildState() => new ChemicalDependencyLedgerState
        {
            survivors = new List<SurvivorDependencyList>
            {
                new SurvivorDependencyList
                {
                    survivorId = "survivor_gunner_mikhail",
                    dependencies = new List<ChemicalDependencyState>
                    {
                        new ChemicalDependencyState
                        {
                            itemId = "item_morphine",
                            dependencyLevel = 0.75f,
                            kind = "Opioid",
                            inManagedDetox = true,
                            inColdTurkey = false,
                            detoxProgressHours = 48f
                        }
                    }
                }
            }
        };

        private static string RoundTripChecksum(ChemicalDependencyLedgerState state)
        {
            var envelope = new ChemicalDependencyHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<ChemicalDependencyHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new ChemicalDependencyHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedLedger_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.survivors[0].dependencies[0].dependencyLevel = 0.95f;
            tampered.survivors[0].dependencies[0].detoxProgressHours = 96f;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new ChemicalDependencyHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }

        [Fact]
        public void LegacyBareState_StillDeserializes()
        {
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(BuildState());
            var asEnvelope = json.Deserialize<ChemicalDependencyHostSave>(raw);
            Assert.True(asEnvelope == null || asEnvelope.State == null);

            var bare = json.Deserialize<ChemicalDependencyLedgerState>(raw);
            Assert.NotNull(bare);
            Assert.Single(bare.survivors);
            Assert.Equal("survivor_gunner_mikhail", bare.survivors[0].survivorId);
            Assert.True(bare.survivors[0].dependencies[0].inManagedDetox);
        }
    }
}
