// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioSaveMigrationTests
    {
        [Fact]
        public void V2Save_EncodesAndDecodes_WithFullPlan24State()
        {
            var json = new SystemTextJsonSerializer();
            var state = new RadioSaveState
            {
                day = 45,
                currentFrequency = 88.5f,
                history = new List<RadioInterceptEntry>
                {
                    new RadioInterceptEntry
                    {
                        factionId = "faction_civil_defense",
                        callsign = "CIVIL DEFENSE 88.5",
                        frequencyMhz = 88.5f,
                        kind = 0,
                        message = "Morning weather clear.",
                        signalStrength = 7,
                        day = 45
                    }
                },
                playedBroadcastKeys = new List<string> { "45:88.50:weather" },
                discoveredStationIds = new List<string> { RadioStationCatalog.StationCivilDefense },
                customPresets = new List<float> { 88.5f, 104.2f },
                distressSignals = new List<DistressSignalSaveEntry>
                {
                    new DistressSignalSaveEntry
                    {
                        signalId = "freq_distress_77_3",
                        status = (int)DistressSignalStatus.Intercepted,
                        interceptedDay = 45,
                        daysRemaining = 5
                    }
                },
                signalLog = new List<SignalLogEntry>
                {
                    new SignalLogEntry
                    {
                        id = "log_88.5_45",
                        title = "Civil Defense Morning",
                        stationId = RadioStationCatalog.StationCivilDefense,
                        frequencyMhz = 88.5f,
                        dayLogged = 45
                    }
                },
                recordedCassettes = new List<RecordedCassetteEntry>
                {
                    new RecordedCassetteEntry
                    {
                        cassetteId = "cassette_rec_01",
                        title = "Emergency Warning",
                        frequencyMhz = 88.5f,
                        recordedDay = 45
                    }
                }
            };

            string encoded = RadioSaveCodec.Encode(state, json);
            Assert.NotNull(encoded);
            Assert.Contains("\"saveVersion\":2", encoded);

            bool ok = RadioSaveCodec.TryDecode(encoded, json, out var restored);
            Assert.True(ok);
            Assert.NotNull(restored);
            Assert.Equal(45, restored!.day);
            Assert.Equal(88.5f, restored.currentFrequency);
            Assert.Single(restored.history);
            Assert.Single(restored.discoveredStationIds);
            Assert.Equal(2, restored.customPresets.Count);
            Assert.Single(restored.distressSignals);
            Assert.Single(restored.signalLog);
            Assert.Single(restored.recordedCassettes);
        }

        [Fact]
        public void V1LegacySave_MigratesSeamlesslyToV2()
        {
            var json = new SystemTextJsonSerializer();
            var v1 = new RadioSaveStateFrozenV1
            {
                saveVersion = 1,
                day = 25,
                currentFrequency = 97.5f,
                history = new List<RadioInterceptEntry>
                {
                    new RadioInterceptEntry
                    {
                        factionId = "faction_holdfast",
                        callsign = "HOLDFAST",
                        frequencyMhz = 97.5f,
                        kind = 0,
                        message = "relay active",
                        signalStrength = 6,
                        day = 25
                    }
                },
                playedBroadcastKeys = new List<string> { "25:97.50:relay" }
            };
            v1.Checksum = SaveChecksum.Compute(v1);

            string v1Json = json.Serialize(v1);
            Assert.Contains("\"saveVersion\":1", v1Json);

            bool ok = RadioSaveCodec.TryDecode(v1Json, json, out var migrated);
            Assert.True(ok);
            Assert.NotNull(migrated);
            Assert.Equal(RadioSaveCodec.CurrentSaveVersion, migrated!.saveVersion);
            Assert.Equal(25, migrated.day);
            Assert.Equal(97.5f, migrated.currentFrequency);
            Assert.Single(migrated.history);
            Assert.NotNull(migrated.distressSignals);
            Assert.NotNull(migrated.discoveredStationIds);
            Assert.NotNull(migrated.signalLog);
            Assert.NotNull(migrated.recordedCassettes);
        }

        [Fact]
        public void TamperedV2Payload_IsRejected()
        {
            var json = new SystemTextJsonSerializer();
            var state = new RadioSaveState
            {
                day = 10,
                currentFrequency = 88.5f
            };
            string encoded = RadioSaveCodec.Encode(state, json);
            string tampered = encoded.Replace("\"day\":10", "\"day\":999");

            bool ok = RadioSaveCodec.TryDecode(tampered, json, out _);
            Assert.False(ok);
        }
    }
}
