using System;

namespace Ashfall.Core.Save
{
    /// <summary>
    /// Legacy-compat adapter for the shelter-batch family of save stores whose
    /// on-disk envelope is <c>{ SchemaVersion, State, Checksum }</c> declared
    /// with C# <b>properties</b>. Two consequences are preserved verbatim and
    /// must not be "fixed" here without a save-evolution plan:
    /// <list type="bullet">
    /// <item><see cref="SaveChecksum"/> walks public instance <b>fields</b>, so
    /// the stamped checksum is a constant over the empty field set — the payload
    /// was never actually integrity-protected by it.</item>
    /// <item>Matching historical load behavior, the checksum is checked for
    /// presence only, never verified against the payload.</item>
    /// </list>
    /// This adapter exists so those sections can move onto the shared
    /// <see cref="SaveStore{T}"/> service without changing a single byte of
    /// their existing files. New sections must use the canonical
    /// <see cref="SaveEnvelope{T}"/> instead.
    /// </summary>
    [Serializable]
    public sealed class SchemaVersionedEnvelope<T> where T : class
    {
        public string SchemaVersion { get; set; } = "1.0";
        public T State { get; set; } = null!;
        public string Checksum { get; set; } = string.Empty;

        /// <summary>
        /// Encodes state exactly the way the hand-rolled stores did: stamp the
        /// (constant) envelope checksum, serialize the property envelope.
        /// </summary>
        public static string Encode(T state, IJsonSerializer json)
        {
            var envelope = new SchemaVersionedEnvelope<T> { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            return json.Serialize(envelope);
        }

        /// <summary>
        /// Decodes the legacy shape with the historical semantics: a parsed
        /// envelope with state requires a non-empty checksum (presence only —
        /// no verification, matching the stores this replaces); anything else
        /// falls back to bare-state parsing. Returns null when nothing parses.
        /// </summary>
        public static T? Decode(string raw, IJsonSerializer json)
        {
            var envelope = json.Deserialize<SchemaVersionedEnvelope<T>>(raw);
            if (envelope != null && envelope.State != null)
            {
                if (string.IsNullOrEmpty(envelope.Checksum))
                    throw new InvalidOperationException("checksum field missing (corrupt save).");
                return envelope.State;
            }

            return json.Deserialize<T>(raw);
        }
    }
}
