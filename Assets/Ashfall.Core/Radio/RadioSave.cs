// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Serialized radio intercept. Mirrors <see cref="RadioIntercept"/> with the
    /// enum stored numerically (matching how both serializers persist enums and
    /// how <see cref="SaveChecksum"/> hashes them).
    /// </summary>
    [Serializable]
    public class RadioInterceptEntry
    {
        public string factionId = string.Empty;
        public string callsign = string.Empty;
        public float frequencyMhz;
        public int kind; // RadioEventKind numeric
        public string message = string.Empty;
        public int signalStrength;
        public int day;
    }

    [Serializable]
    public class DistressSignalSaveEntry
    {
        public string signalId = string.Empty;
        public int status; // DistressSignalStatus enum numeric
        public int interceptedDay;
        public int daysRemaining;
        public float highestClarity;
        public bool isDispatched;
        public bool isResolved;
        public string resolutionType = string.Empty;
    }

    [Serializable]
    public class SignalLogEntry
    {
        public string id = string.Empty;
        public string title = string.Empty;
        public string stationId = string.Empty;
        public float frequencyMhz;
        public int dayLogged;
        public string summary = string.Empty;
        public bool isDecoded;
        public bool isTriangulated;
    }

    [Serializable]
    public class RecordedCassetteEntry
    {
        public string cassetteId = string.Empty;
        public string broadcastId = string.Empty;
        public string title = string.Empty;
        public string transcript = string.Empty;
        public int recordedDay;
        public float frequencyMhz;
        public string sourceName = string.Empty;
        public string audioCue = string.Empty;
    }

    [Serializable]
    public class StationStateOverrideEntry
    {
        public string stationId = string.Empty;
        public int state; // RadioStationState enum numeric
        public int overrideUntilDay;
    }

    /// <summary>
    /// ASHFALL — radio host save state (Version 2). Owns every authoritative mutable value of
    /// the receiver: intercept history, played-broadcast dedup keys, tuned frequency,
    /// sim day, discovered stations, custom presets, active/resolved distress signals,
    /// signal intelligence log, recorded cassettes, and station overrides.
    /// Versioned + checksummed via <see cref="RadioSaveCodec"/>.
    /// </summary>
    [Serializable]
    public class RadioSaveState
    {
        public int saveVersion = RadioSaveCodec.CurrentSaveVersion;
        public int day;
        public float currentFrequency;
        public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
        public List<string> playedBroadcastKeys = new List<string>();

        // Plan 24 additions (V2)
        public List<string> discoveredStationIds = new List<string>();
        public List<float> customPresets = new List<float>();
        public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
        public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
        public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
        public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();

        public string Checksum = string.Empty;
    }

    [Serializable]
    public class RadioSaveStateFrozenV1
    {
        public int saveVersion = 1;
        public int day;
        public float currentFrequency;
        public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
        public List<string> playedBroadcastKeys = new List<string>();
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Radio save codec: checksum recomputed on encode, hard-reject on decode for
    /// tamper / checksumless / newer-version payloads (mirrors VerdictSaveCodec).
    /// Supports V1 -> V2 migration with frozen shape validation.
    /// </summary>
    public static class RadioSaveCodec
    {
        public const int CurrentSaveVersion = 2;
        public const int MigrationFromVersion = 1;

        public static string Encode(RadioSaveState state, IJsonSerializer json)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (json == null) throw new ArgumentNullException(nameof(json));
            state.saveVersion = CurrentSaveVersion;
            state.Checksum = SaveChecksum.Compute(state);
            return json.Serialize(state);
        }

        public static bool TryDecode(string json, IJsonSerializer serializer, out RadioSaveState state)
        {
            state = null!;
            if (string.IsNullOrEmpty(json) || serializer == null) return false;
            try
            {
                var decoded = serializer.Deserialize<RadioSaveState>(json);
                if (decoded == null) return false;
                if (decoded.saveVersion > CurrentSaveVersion) return false;   // future — reject
                if (decoded.saveVersion < MigrationFromVersion) return false; // too old — reject

                if (decoded.saveVersion == 1)
                {
                    return MigrateV1(json, serializer, out state);
                }

                if (string.IsNullOrEmpty(decoded.Checksum)) return false;     // malformed new format — reject
                if (!string.Equals(SaveChecksum.Compute(decoded), decoded.Checksum, StringComparison.Ordinal))
                    return false;                                             // tampered

                EnsureCollections(decoded);
                state = decoded;
                return true;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<decode>", "RadioSaveState", ex_CATDIAG);
                return false;
            }
        }

        private static bool MigrateV1(string json, IJsonSerializer serializer, out RadioSaveState state)
        {
            state = null!;
            var v1 = serializer.Deserialize<RadioSaveStateFrozenV1>(json);
            if (v1 == null) return false;
            if (string.IsNullOrEmpty(v1.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v1), v1.Checksum, StringComparison.Ordinal))
                return false; // tampered V1 save

            state = new RadioSaveState
            {
                saveVersion = CurrentSaveVersion,
                day = v1.day,
                currentFrequency = v1.currentFrequency,
                history = v1.history ?? new List<RadioInterceptEntry>(),
                playedBroadcastKeys = v1.playedBroadcastKeys ?? new List<string>(),
                discoveredStationIds = new List<string>(),
                customPresets = new List<float>(),
                distressSignals = new List<DistressSignalSaveEntry>(),
                signalLog = new List<SignalLogEntry>(),
                recordedCassettes = new List<RecordedCassetteEntry>(),
                stationOverrides = new List<StationStateOverrideEntry>(),
                Checksum = string.Empty
            };
            return true;
        }

        private static void EnsureCollections(RadioSaveState state)
        {
            if (state.history == null) state.history = new List<RadioInterceptEntry>();
            if (state.playedBroadcastKeys == null) state.playedBroadcastKeys = new List<string>();
            if (state.discoveredStationIds == null) state.discoveredStationIds = new List<string>();
            if (state.customPresets == null) state.customPresets = new List<float>();
            if (state.distressSignals == null) state.distressSignals = new List<DistressSignalSaveEntry>();
            if (state.signalLog == null) state.signalLog = new List<SignalLogEntry>();
            if (state.recordedCassettes == null) state.recordedCassettes = new List<RecordedCassetteEntry>();
            if (state.stationOverrides == null) state.stationOverrides = new List<StationStateOverrideEntry>();
        }
    }
}
