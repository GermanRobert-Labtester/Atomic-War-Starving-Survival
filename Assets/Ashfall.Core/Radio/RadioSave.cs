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

    /// <summary>
    /// ASHFALL — radio host save state. Owns every authoritative mutable value of
    /// the receiver: intercept history (ordered, capped), played-broadcast dedup
    /// keys, the tuned frequency, and the sim day. Versioned + checksummed via
    /// <see cref="RadioSaveCodec"/>. Engine-agnostic; the Godot host persists it
    /// through <c>RadioSaveStore</c>.
    ///
    /// Note: the receiver's <c>ISeededRng</c> is a presentation read-model and is
    /// intentionally not persisted — the radio engine is deterministic and
    /// re-seeded per session, so broadcast selection order may differ across
    /// restarts. This does not affect any simulation state.
    /// </summary>
    [Serializable]
    public class RadioSaveState
    {
        public int saveVersion = RadioSaveCodec.CurrentSaveVersion;
        public int day;
        public float currentFrequency;
        public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();
        public List<string> playedBroadcastKeys = new List<string>();
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Radio save codec: checksum recomputed on encode, hard-reject on decode for
    /// tamper / checksumless / newer-version payloads (mirrors VerdictSaveCodec).
    /// There is no pre-checksum legacy radio format; a missing or unreadable save
    /// degrades to a fresh receiver (the host's no-radio-save fallback).
    /// </summary>
    public static class RadioSaveCodec
    {
        public const int CurrentSaveVersion = 1;
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
                if (string.IsNullOrEmpty(decoded.Checksum)) return false;     // malformed new format — reject
                if (!string.Equals(SaveChecksum.Compute(decoded), decoded.Checksum, StringComparison.Ordinal))
                    return false;                                             // tampered
                if (decoded.history == null) decoded.history = new List<RadioInterceptEntry>();
                if (decoded.playedBroadcastKeys == null) decoded.playedBroadcastKeys = new List<string>();
                state = decoded;
                return true;
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return false;
                                }
        }
    }
}
