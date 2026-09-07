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
        public void V3Save_EncodesAndDecodes_WithTriangulationNest()
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
                },
                triangulation = new TriangulationState
                {
                    observations =
                    {
                        new RadioObservation
                        {
                            signalId = "sig_civil_defense",
                            stationId = "station_shelter_primary",
                            bearingDegrees = 40f,
                            errorDegrees = 2f,
                            signalStrength = 0.9f,
                            weatherCondition = "Clear",
                            operatorSkill = 0.8f
                        }
                    },
                    discoveredLocationIds = { "loc_broadcast_bunker_echo" },
                    stationBaselines =
                    {
                        new StationBaselineEntry
                        {
                            stationId = "station_shelter_primary",
                            xKm = 0f,
                            yKm = 0f,
                            arrayId = "df_array_shelter_loop"
                        }
                    }
                }
            };

            string encoded = RadioSaveCodec.Encode(state, json);
            Assert.NotNull(encoded);
            Assert.Contains("\"saveVersion\":3", encoded);

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
            Assert.NotNull(restored.triangulation);
            Assert.Single(restored.triangulation.observations);
            Assert.Contains("loc_broadcast_bunker_echo", restored.triangulation.discoveredLocationIds);
            Assert.Single(restored.triangulation.stationBaselines);
        }

        [Fact]
        public void V1LegacySave_MigratesSeamlesslyToV3()
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
            Assert.NotNull(migrated.triangulation);
            Assert.Empty(migrated.triangulation.observations);
        }

        [Fact]
        public void V2LegacySave_MigratesToV3_WithEmptyTriangulationNest()
        {
            var json = new SystemTextJsonSerializer();
            var v2 = new RadioSaveStateFrozenV2
            {
                saveVersion = 2,
                day = 33,
                currentFrequency = 104.2f,
                history = new List<RadioInterceptEntry>
                {
                    new RadioInterceptEntry
                    {
                        factionId = "faction_civil_defense",
                        callsign = "CIVIL",
                        frequencyMhz = 104.2f,
                        kind = 0,
                        message = "relay",
                        signalStrength = 5,
                        day = 33
                    }
                },
                playedBroadcastKeys = new List<string> { "33:104.20:relay" },
                discoveredStationIds = new List<string> { RadioStationCatalog.StationCivilDefense },
                customPresets = new List<float> { 104.2f },
                distressSignals = new List<DistressSignalSaveEntry>(),
                signalLog = new List<SignalLogEntry>(),
                recordedCassettes = new List<RecordedCassetteEntry>(),
                stationOverrides = new List<StationStateOverrideEntry>()
            };
            v2.Checksum = SaveChecksum.Compute(v2);

            string v2Json = json.Serialize(v2);
            Assert.Contains("\"saveVersion\":2", v2Json);

            bool ok = RadioSaveCodec.TryDecode(v2Json, json, out var migrated);
            Assert.True(ok);
            Assert.NotNull(migrated);
            Assert.Equal(3, migrated!.saveVersion);
            Assert.Equal(33, migrated.day);
            Assert.Equal(104.2f, migrated.currentFrequency);
            Assert.Single(migrated.history);
            Assert.Single(migrated.discoveredStationIds);
            Assert.NotNull(migrated.triangulation);
            Assert.Empty(migrated.triangulation.observations);
            Assert.Empty(migrated.triangulation.candidates);
            Assert.Empty(migrated.triangulation.discoveredLocationIds);
        }

        [Fact]
        public void TamperedV3Payload_IsRejected()
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
