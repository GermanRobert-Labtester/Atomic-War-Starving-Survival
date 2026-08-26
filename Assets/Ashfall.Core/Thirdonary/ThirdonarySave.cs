using System;

namespace Ashfall.Core.Thirdonary
{
    /// <summary>
    /// Cross-host save envelope for Thirdonary quests with checksum validation.
    /// Follows the ExpansionQuestSaveEnvelope pattern exactly.
    /// </summary>
    [Serializable]
    public class ThirdonarySaveEnvelope
    {
        public const int CurrentVersion = 1;

        public int version = CurrentVersion;
        public ThirdonaryState state = new ThirdonaryState();
        public string checksum = string.Empty;
    }

    public static class ThirdonarySaveCodec
    {
        public static string Encode(ThirdonarySaveEnvelope envelope, IJsonSerializer json)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));
            envelope.checksum = SaveChecksum.Compute(envelope);
            return json.Serialize(envelope);
        }

        public static ThirdonarySaveEnvelope Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("ThirdonarySave: empty save payload.");

            ThirdonarySaveEnvelope envelope;
            try { envelope = json.Deserialize<ThirdonarySaveEnvelope>(jsonText!); }
            catch (Exception e)
            {
                throw new InvalidOperationException("ThirdonarySave: malformed save payload: " + e.Message, e);
            }
            if (envelope == null)
                throw new InvalidOperationException("ThirdonarySave: empty save payload.");

            if (envelope.version > ThirdonarySaveEnvelope.CurrentVersion)
                throw new InvalidOperationException(
                    "ThirdonarySave: version " + envelope.version + " is newer than supported.");

            if (string.IsNullOrEmpty(envelope.checksum))
                throw new InvalidOperationException("ThirdonarySave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(envelope);
            if (!string.Equals(envelope.checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException("ThirdonarySave: checksum mismatch (corrupt or foreign save).");
            return envelope;
        }
    }
}
