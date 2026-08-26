using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL — Expansion Quest System save envelope.
    /// Cross-host save envelope for expansion quests with checksum validation.
    /// </summary>
    [Serializable]
    public class ExpansionQuestSaveEnvelope
    {
        public const int CurrentVersion = 1;
        public const int MigrationFromVersion = 1;

        public int version = CurrentVersion;
        public ExpansionQuestSystemState state = new ExpansionQuestSystemState();
        public string checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v1 envelope shape for migration validation.
    /// </summary>
    [Serializable]
    public class ExpansionQuestSaveEnvelopeV1
    {
        public int version = 1;
        public ExpansionQuestSystemState state = new ExpansionQuestSystemState();
        public string checksum = string.Empty;
    }

    public static class ExpansionQuestSaveCodec
    {
        public static string Encode(ExpansionQuestSaveEnvelope envelope, IJsonSerializer json)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));
            envelope.checksum = SaveChecksum.Compute(envelope);
            return json.Serialize(envelope);
        }

        public static ExpansionQuestSaveEnvelope Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("ExpansionQuestSave: empty save payload.");

            ExpansionQuestSaveEnvelope envelope;
            try { envelope = json.Deserialize<ExpansionQuestSaveEnvelope>(jsonText!); }
            catch (Exception e)
            {
                throw new InvalidOperationException("ExpansionQuestSave: malformed save payload: " + e.Message, e);
            }
            if (envelope == null)
                throw new InvalidOperationException("ExpansionQuestSave: empty save payload.");

            if (envelope.version > ExpansionQuestSaveEnvelope.CurrentVersion)
                throw new InvalidOperationException(
                    "ExpansionQuestSave: version " + envelope.version + " is newer than supported.");
            if (envelope.version < ExpansionQuestSaveEnvelope.MigrationFromVersion)
                throw new InvalidOperationException("ExpansionQuestSave: invalid version.");

            if (string.IsNullOrEmpty(envelope.checksum))
                throw new InvalidOperationException("ExpansionQuestSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(envelope);
            if (!string.Equals(envelope.checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException("ExpansionQuestSave: checksum mismatch (corrupt or foreign save).");
            return envelope;
        }
    }
}
