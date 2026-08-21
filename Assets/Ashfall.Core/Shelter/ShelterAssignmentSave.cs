using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Shelter
{
    /// <summary>Checksummed save envelope for shelter assignments.</summary>
    [Serializable]
    public class ShelterAssignmentSave
    {
        public const int CurrentSaveVersion = 1;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public List<ShelterRoomSave> Rooms = new List<ShelterRoomSave>();
        public ShelterAssignmentState State = new ShelterAssignmentState();
        public string Checksum = string.Empty;
    }

    [Serializable]
    public sealed class ShelterRoomSave
    {
        public string RoomId;
        public string DisplayName;
        public int Capacity;
        public string RequiredSkillId;
        public string WorkstationId;
    }

    public static class ShelterAssignmentSaveCodec
    {
        public static ShelterAssignmentSave Encode(ShelterAssignmentSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (save.saveVersion > ShelterAssignmentSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "ShelterAssignmentSave: refusing to encode a saveVersion newer than supported.");
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string EncodeToString(ShelterAssignmentSave save, IJsonSerializer json)
        {
            Encode(save, json);
            return json.Serialize(save);
        }

        public static ShelterAssignmentSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("ShelterAssignmentSave: empty save payload.");
            ShelterAssignmentSave save;
            try { save = json.Deserialize<ShelterAssignmentSave>(jsonText!); }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "ShelterAssignmentSave: malformed save payload: " + e.Message, e);
            }
            if (save == null)
                throw new InvalidOperationException("ShelterAssignmentSave: empty save payload.");
            if (save.saveVersion > ShelterAssignmentSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "ShelterAssignmentSave: saveVersion " + save.saveVersion + " is newer than supported.");
            if (save.saveVersion < ShelterAssignmentSave.MigrationFromVersion)
                throw new InvalidOperationException("ShelterAssignmentSave: invalid saveVersion.");
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "ShelterAssignmentSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "ShelterAssignmentSave: checksum mismatch (corrupt or foreign save).");
            return save;
        }
    }
}
