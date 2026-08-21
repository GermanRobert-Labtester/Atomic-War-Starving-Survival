using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// PowerGrid cross-host save envelope. Same checksummed rules as the
    /// other expansion envelopes.
    /// </summary>
    [Serializable]
    public class PowerGridSave
    {
        public const int CurrentSaveVersion = 1;
        public const int MigrationFromVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public List<PowerGridRoomSave> Rooms = new List<PowerGridRoomSave>();
        public PowerGridState State = new PowerGridState();
        public string Checksum = string.Empty;
    }

    [Serializable]
    public sealed class PowerGridRoomSave
    {
        public string RoomId;
        public string DisplayName;
        public float DrawWatts;
        public int DefaultPriority; // serialized as int for cross-host stability
        public string FailureEffectId;
    }

    public static class PowerGridSaveCodec
    {
        public static PowerGridSave Encode(PowerGridSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (save.saveVersion > PowerGridSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "PowerGridSave: refusing to encode a saveVersion newer than supported.");
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string EncodeToString(PowerGridSave save, IJsonSerializer json)
        {
            Encode(save, json);
            return json.Serialize(save);
        }

        public static PowerGridSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                throw new InvalidOperationException("PowerGridSave: empty save payload.");
            PowerGridSave save;
            try { save = json.Deserialize<PowerGridSave>(jsonText!); }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    "PowerGridSave: malformed save payload: " + e.Message, e);
            }
            if (save == null)
                throw new InvalidOperationException("PowerGridSave: empty save payload.");
            if (save.saveVersion > PowerGridSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    "PowerGridSave: saveVersion " + save.saveVersion + " is newer than supported.");
            if (save.saveVersion < PowerGridSave.MigrationFromVersion)
                throw new InvalidOperationException("PowerGridSave: invalid saveVersion.");
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException(
                    "PowerGridSave: save carries no checksum (truncated or tampered file).");
            string actual = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "PowerGridSave: checksum mismatch (corrupt or foreign save).");
            return save;
        }

        public static PowerGridRoom ToRoom(PowerGridRoomSave s)
        {
            return new PowerGridRoom(s.RoomId, s.DisplayName, s.DrawWatts,
                (PowerGridRoomPriority)s.DefaultPriority, s.FailureEffectId);
        }

        public static PowerGridRoomSave FromRoom(PowerGridRoom r)
        {
            return new PowerGridRoomSave
            {
                RoomId = r.RoomId,
                DisplayName = r.DisplayName,
                DrawWatts = r.DrawWatts,
                DefaultPriority = (int)r.DefaultPriority,
                FailureEffectId = r.FailureEffectId
            };
        }
    }
}
