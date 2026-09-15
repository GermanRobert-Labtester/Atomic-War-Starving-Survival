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
        public bool isMoralChoiceAvailable;
        public int moralChoiceResolutionIndex = -1;
        public bool isIgnored;
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
    /// ASHFALL — radio host save state (Version 3). Owns every authoritative mutable value of
    /// the receiver: intercept history, played-broadcast dedup keys, tuned frequency,
    /// sim day, discovered stations, custom presets, active/resolved distress signals,
    /// signal intelligence log, recorded cassettes, station overrides, and continuous
    /// HF/DF triangulation observations/candidates (Plan B88).
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

        // Plan B88 (V3) — continuous DF triangulation nest (Core CaptureState shape)
        public TriangulationState triangulation = new TriangulationState();

        // Rescue-signal runtime (V4) — persistent distress-rescue mission state:
        // stage, deadline, expedition association, claimed reward receipts,
        // sender survival, ignore consequence, and authenticity assessment.
        public DistressMissionSaveState? rescueMissions;

        // Tasks 9–12 Wave 2 (V5) — radio-owned signal-trust ledger: bounded
        // credibility score + exactly-once per-signal event ledgers. Old saves
        // migrate to a neutral default (score 50, zero counters).
        public SignalTrustSaveEntry? signalTrust;

        // Tasks 9–12 Wave 3 (V6) — pending + fired distress follow-up
        // transmissions. Old saves migrate to an empty scheduler state.
        public SignalFollowUpSaveState? signalFollowUps;

        public string Checksum = string.Empty;
    }

    /// <summary>Frozen V5 wire shape for checksum-safe migration into V6.
    /// Exactly the live V5 field set — the follow-up scheduler state did not
    /// exist in V5 saves, so it must not participate in the V5 checksum.</summary>
    [Serializable]
    public class RadioSaveStateFrozenV5
    {
        public int saveVersion = 5;
        public int day;
        public float currentFrequency;
        public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
        public List<string> playedBroadcastKeys = new List<string>();
        public List<string> discoveredStationIds = new List<string>();
        public List<float> customPresets = new List<float>();
        public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
        public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
        public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
        public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
        public TriangulationState triangulation = new TriangulationState();
        public DistressMissionSaveState? rescueMissions;
        public SignalTrustSaveEntry? signalTrust;
        public string Checksum = string.Empty;
    }

    /// <summary>Frozen V4 wire shape for checksum-safe migration into V5.
    /// Exactly the live V4 field set — the signal-trust ledger did not exist
    /// in V4 saves, so it must not participate in the V4 checksum.</summary>
    [Serializable]
    public class RadioSaveStateFrozenV4
    {
        public int saveVersion = 4;
        public int day;
        public float currentFrequency;
        public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
        public List<string> playedBroadcastKeys = new List<string>();
        public List<string> discoveredStationIds = new List<string>();
        public List<float> customPresets = new List<float>();
        public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
        public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
        public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
        public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
        public TriangulationState triangulation = new TriangulationState();
        public DistressMissionSaveState? rescueMissions;
        public string Checksum = string.Empty;
    }

    /// <summary>Frozen V3 wire shape for checksum-safe migration into V4.</summary>
    [Serializable]
    public class RadioSaveStateFrozenV3
    {
        public int saveVersion = 3;
        public int day;
        public float currentFrequency;
        public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
        public List<string> playedBroadcastKeys = new List<string>();
        public List<string> discoveredStationIds = new List<string>();
        public List<float> customPresets = new List<float>();
        public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();
        public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();
        public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();
        public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
        public TriangulationState triangulation = new TriangulationState();
        public string Checksum = string.Empty;
    }

    /// <summary>Frozen V2 wire shape for checksum-safe migration into V3.</summary>
    [Serializable]
    public class RadioSaveStateFrozenV2
    {
        public int saveVersion = 2;
        public int day;
        public float currentFrequency;
        public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
        public List<string> playedBroadcastKeys = new List<string>();
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
    /// Supports V1 -> V2 -> V3 migration with frozen shape validation.
    /// </summary>
    public static class RadioSaveCodec
    {
        public const int CurrentSaveVersion = 6;
        public const int MigrationFromVersion = 1;

        public static string Encode(RadioSaveState state, IJsonSerializer json)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (json == null) throw new ArgumentNullException(nameof(json));
            state.saveVersion = CurrentSaveVersion;
            EnsureCollections(state);
            if (state.rescueMissions != null)
                state.rescueMissions.RefreshFingerprint();
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
                    return MigrateV1(json, serializer, out state);

                if (decoded.saveVersion == 2)
                    return MigrateV2(json, serializer, out state);

                if (decoded.saveVersion == 3)
                    return MigrateV3(json, serializer, out state);

                if (decoded.saveVersion == 4)
                    return MigrateV4(json, serializer, out state);

                if (decoded.saveVersion == 5)
                    return MigrateV5(json, serializer, out state);

                // V3+: normalize collections before checksum so null vs empty cannot diverge.
                EnsureCollections(decoded);
                // V4+: mission payloads are property-based (outside the field-walking
                // checksum), so the codec validates their canonical fingerprint.
                if (decoded.rescueMissions != null &&
                    !string.IsNullOrEmpty(decoded.rescueMissions.missionsFingerprint) &&
                    !string.Equals(DistressMissionSaveState.ComputeFingerprint(decoded.rescueMissions),
                        decoded.rescueMissions.missionsFingerprint, StringComparison.Ordinal))
                    return false; // tampered mission state
                if (string.IsNullOrEmpty(decoded.Checksum)) return false;     // malformed new format — reject
                if (!string.Equals(SaveChecksum.Compute(decoded), decoded.Checksum, StringComparison.Ordinal))
                    return false;                                             // tampered

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
                triangulation = new TriangulationState(),
                Checksum = string.Empty
            };
            return true;
        }

        private static bool MigrateV2(string json, IJsonSerializer serializer, out RadioSaveState state)
        {
            state = null!;
            var v2 = serializer.Deserialize<RadioSaveStateFrozenV2>(json);
            if (v2 == null) return false;
            if (string.IsNullOrEmpty(v2.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v2), v2.Checksum, StringComparison.Ordinal))
                return false; // tampered V2 save

            state = new RadioSaveState
            {
                saveVersion = CurrentSaveVersion,
                day = v2.day,
                currentFrequency = v2.currentFrequency,
                history = v2.history ?? new List<RadioInterceptEntry>(),
                playedBroadcastKeys = v2.playedBroadcastKeys ?? new List<string>(),
                discoveredStationIds = v2.discoveredStationIds ?? new List<string>(),
                customPresets = v2.customPresets ?? new List<float>(),
                distressSignals = v2.distressSignals ?? new List<DistressSignalSaveEntry>(),
                signalLog = v2.signalLog ?? new List<SignalLogEntry>(),
                recordedCassettes = v2.recordedCassettes ?? new List<RecordedCassetteEntry>(),
                stationOverrides = v2.stationOverrides ?? new List<StationStateOverrideEntry>(),
                triangulation = new TriangulationState(),
                Checksum = string.Empty
            };
            return true;
        }

        /// <summary>
        /// V3 → V4: validate the checksum against the frozen V3 shape (the live
        /// state now carries the rescue-signal runtime field, which old saves
        /// never hashed), then rebuild as V4 with an empty mission nest.
        /// </summary>
        private static bool MigrateV3(string json, IJsonSerializer serializer, out RadioSaveState state)
        {
            state = null!;
            var v3 = serializer.Deserialize<RadioSaveStateFrozenV3>(json);
            if (v3 == null) return false;
            if (string.IsNullOrEmpty(v3.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v3), v3.Checksum, StringComparison.Ordinal))
                return false; // tampered V3 save

            state = new RadioSaveState
            {
                saveVersion = CurrentSaveVersion,
                day = v3.day,
                currentFrequency = v3.currentFrequency,
                history = v3.history ?? new List<RadioInterceptEntry>(),
                playedBroadcastKeys = v3.playedBroadcastKeys ?? new List<string>(),
                discoveredStationIds = v3.discoveredStationIds ?? new List<string>(),
                customPresets = v3.customPresets ?? new List<float>(),
                distressSignals = v3.distressSignals ?? new List<DistressSignalSaveEntry>(),
                signalLog = v3.signalLog ?? new List<SignalLogEntry>(),
                recordedCassettes = v3.recordedCassettes ?? new List<RecordedCassetteEntry>(),
                stationOverrides = v3.stationOverrides ?? new List<StationStateOverrideEntry>(),
                triangulation = v3.triangulation ?? new TriangulationState(),
                rescueMissions = null,
                Checksum = string.Empty
            };
            return true;
        }

        /// <summary>
        /// V4 → V5: validate the checksum against the frozen V4 shape (the live
        /// state now carries the signal-trust ledger, which V4 saves never
        /// hashed), then rebuild as V5 with a neutral trust default.
        /// </summary>
        private static bool MigrateV4(string json, IJsonSerializer serializer, out RadioSaveState state)
        {
            state = null!;
            var v4 = serializer.Deserialize<RadioSaveStateFrozenV4>(json);
            if (v4 == null) return false;
            if (string.IsNullOrEmpty(v4.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v4), v4.Checksum, StringComparison.Ordinal))
                return false; // tampered V4 save

            state = new RadioSaveState
            {
                saveVersion = CurrentSaveVersion,
                day = v4.day,
                currentFrequency = v4.currentFrequency,
                history = v4.history ?? new List<RadioInterceptEntry>(),
                playedBroadcastKeys = v4.playedBroadcastKeys ?? new List<string>(),
                discoveredStationIds = v4.discoveredStationIds ?? new List<string>(),
                customPresets = v4.customPresets ?? new List<float>(),
                distressSignals = v4.distressSignals ?? new List<DistressSignalSaveEntry>(),
                signalLog = v4.signalLog ?? new List<SignalLogEntry>(),
                recordedCassettes = v4.recordedCassettes ?? new List<RecordedCassetteEntry>(),
                stationOverrides = v4.stationOverrides ?? new List<StationStateOverrideEntry>(),
                triangulation = v4.triangulation ?? new TriangulationState(),
                rescueMissions = v4.rescueMissions,
                signalTrust = new SignalTrustSaveEntry(), // explicit neutral default — no reconstructed history
                Checksum = string.Empty
            };
            return true;
        }

        /// <summary>
        /// V5 → V6: validate the checksum against the frozen V5 shape (the live
        /// state now carries the follow-up scheduler state, which V5 saves never
        /// hashed), then rebuild as V6 with an empty scheduler default.
        /// </summary>
        private static bool MigrateV5(string json, IJsonSerializer serializer, out RadioSaveState state)
        {
            state = null!;
            var v5 = serializer.Deserialize<RadioSaveStateFrozenV5>(json);
            if (v5 == null) return false;
            if (string.IsNullOrEmpty(v5.Checksum)) return false;
            if (!string.Equals(SaveChecksum.Compute(v5), v5.Checksum, StringComparison.Ordinal))
                return false; // tampered V5 save

            state = new RadioSaveState
            {
                saveVersion = CurrentSaveVersion,
                day = v5.day,
                currentFrequency = v5.currentFrequency,
                history = v5.history ?? new List<RadioInterceptEntry>(),
                playedBroadcastKeys = v5.playedBroadcastKeys ?? new List<string>(),
                discoveredStationIds = v5.discoveredStationIds ?? new List<string>(),
                customPresets = v5.customPresets ?? new List<float>(),
                distressSignals = v5.distressSignals ?? new List<DistressSignalSaveEntry>(),
                signalLog = v5.signalLog ?? new List<SignalLogEntry>(),
                recordedCassettes = v5.recordedCassettes ?? new List<RecordedCassetteEntry>(),
                stationOverrides = v5.stationOverrides ?? new List<StationStateOverrideEntry>(),
                triangulation = v5.triangulation ?? new TriangulationState(),
                rescueMissions = v5.rescueMissions,
                signalTrust = v5.signalTrust ?? new SignalTrustSaveEntry(),
                signalFollowUps = new SignalFollowUpSaveState(), // empty default — nothing scheduled yet
                Checksum = string.Empty
            };
            return true;
        }

        private static void EnsureCollections(RadioSaveState state)
        {
            if (state.history == null) state.history = new List<RadioInterceptEntry>();
            if (state.signalTrust == null) state.signalTrust = new SignalTrustSaveEntry();
            if (state.signalFollowUps == null) state.signalFollowUps = new SignalFollowUpSaveState();
            if (state.playedBroadcastKeys == null) state.playedBroadcastKeys = new List<string>();
            if (state.discoveredStationIds == null) state.discoveredStationIds = new List<string>();
            if (state.customPresets == null) state.customPresets = new List<float>();
            if (state.distressSignals == null) state.distressSignals = new List<DistressSignalSaveEntry>();
            if (state.signalLog == null) state.signalLog = new List<SignalLogEntry>();
            if (state.recordedCassettes == null) state.recordedCassettes = new List<RecordedCassetteEntry>();
            if (state.stationOverrides == null) state.stationOverrides = new List<StationStateOverrideEntry>();
            if (state.triangulation == null) state.triangulation = new TriangulationState();
            if (state.triangulation.observations == null) state.triangulation.observations = new List<RadioObservation>();
            if (state.triangulation.candidates == null) state.triangulation.candidates = new List<TriangulationCandidate>();
            if (state.triangulation.discoveredLocationIds == null)
                state.triangulation.discoveredLocationIds = new List<string>();
            if (state.triangulation.stationBaselines == null)
                state.triangulation.stationBaselines = new List<StationBaselineEntry>();
        }
    }
}
