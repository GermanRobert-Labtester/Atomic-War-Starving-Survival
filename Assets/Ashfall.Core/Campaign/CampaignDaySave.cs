// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Save;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Save envelope for <see cref="CampaignDayCoordinator"/>.
    /// Only the advancement history needs persistence; owners are
    /// re-registered on setup.
    /// </summary>
    [Serializable]
    public class CampaignDaySave
    {
        public const int CurrentSaveVersion = 2;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int lastAdvancedDay = -1;
        public int masterSeed = 1986;
        public int derivationVersion = 1;
        public System.Collections.Generic.Dictionary<string, int> streamPositions =
            new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
        // XP-01: immutable, catalog-backed difficulty selected only while a
        // fresh campaign is created. Empty is the explicit v1 legacy state
        // and resolves to the authored default during host restoration.
        public string difficulty_preset_id = string.Empty;
        public string Checksum = string.Empty;
    }

    public static class CampaignDaySaveCodec
    {
        public static CampaignDaySave Encode(CampaignDaySave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (save.saveVersion > CampaignDaySave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "CampaignDaySave: refusing to encode a saveVersion newer than supported.");
            if (save.saveVersion < CampaignDaySave.MigrationFromVersion)
                throw new InvalidOperationException("CampaignDaySave: invalid saveVersion.");
            save.saveVersion = CampaignDaySave.CurrentSaveVersion;
            save.difficulty_preset_id ??= string.Empty;
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string EncodeToString(CampaignDaySave save, IJsonSerializer json)
        {
            Encode(save, json);
            return json.Serialize(save);
        }

        public static CampaignDaySave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("CampaignDaySave: empty save payload.");
            CampaignDaySave save;
            try { save = json.Deserialize<CampaignDaySave>(jsonText!); }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "CampaignDaySave: malformed save payload: " + e.Message, e);
            }
            if (save == null)
                throw new InvalidOperationException("CampaignDaySave: empty save payload.");
            if (save.saveVersion > CampaignDaySave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "CampaignDaySave: saveVersion " + save.saveVersion + " is newer than supported.");
            if (save.saveVersion < CampaignDaySave.MigrationFromVersion)
                throw new InvalidOperationException("CampaignDaySave: invalid saveVersion.");
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "CampaignDaySave: save carries no checksum (truncated or tampered file).");
            string actual = save.saveVersion == 1
                ? ComputeV1Checksum(save)
                : SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "CampaignDaySave: checksum mismatch (corrupt or foreign save).");

            // Verify the original v1 payload before assigning the new field.
            // A subsequent save rewrites this state as a v2 checksummed header.
            if (save.saveVersion == 1)
            {
                save.saveVersion = CampaignDaySave.CurrentSaveVersion;
                save.difficulty_preset_id = string.Empty;
            }
            else
            {
                save.difficulty_preset_id ??= string.Empty;
            }
            return save;
        }

        private static string ComputeV1Checksum(CampaignDaySave save)
        {
            return SaveChecksum.Compute(new CampaignDaySaveV1Checksum
            {
                saveVersion = save.saveVersion,
                lastAdvancedDay = save.lastAdvancedDay,
                masterSeed = save.masterSeed,
                derivationVersion = save.derivationVersion,
                streamPositions = save.streamPositions,
                Checksum = save.Checksum
            });
        }

        /// <summary>
        /// Exact public-field shape of the v1 header. SaveChecksum hashes
        /// field names and values, so validating a v1 envelope through the v2
        /// DTO would falsely reject its correct historical checksum.
        /// </summary>
        [Serializable]
        private sealed class CampaignDaySaveV1Checksum
        {
            public int saveVersion;
            public int lastAdvancedDay;
            public int masterSeed;
            public int derivationVersion;
            public System.Collections.Generic.Dictionary<string, int> streamPositions =
                new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
            public string Checksum = string.Empty;
        }
    }
}
