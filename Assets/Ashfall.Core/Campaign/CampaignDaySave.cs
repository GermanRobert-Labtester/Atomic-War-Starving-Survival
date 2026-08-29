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
        public const int CurrentSaveVersion = 1;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int lastAdvancedDay = -1;
        public int masterSeed = 1986;
        public int derivationVersion = 1;
        public System.Collections.Generic.Dictionary<string, int> streamPositions =
            new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
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
            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "CampaignDaySave: checksum mismatch (corrupt or foreign save).");
            return save;
        }
    }
}
