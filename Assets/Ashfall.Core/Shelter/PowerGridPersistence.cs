using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class PowerGridRoomSave
    {
        public string RoomId = string.Empty;
        public string DisplayName = string.Empty;
        public float DrawWatts;
        public int DefaultPriority;
        public string FailureEffectId = string.Empty;
    }

    /// <summary>
    /// Versioned persistence envelope for the engine-agnostic power-grid authority.
    /// Room definitions are stored with the state so a save remains self-describing
    /// even when the host reconstructs the runtime room catalog separately.
    /// </summary>
    [Serializable]
    public sealed class PowerGridSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public List<PowerGridRoomSave> Rooms = new List<PowerGridRoomSave>();
        public PowerGridState State = new PowerGridState();
        public string Checksum = string.Empty;
    }

    public static class PowerGridSaveCodec
    {
        public static string EncodeToString(PowerGridSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (json == null) throw new ArgumentNullException(nameof(json));
            if (save.saveVersion <= 0) save.saveVersion = PowerGridSave.CurrentSaveVersion;
            if (save.saveVersion > PowerGridSave.CurrentSaveVersion)
                throw new InvalidOperationException("Power grid save version is newer than this build supports.");

            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static PowerGridSave Decode(string raw, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new InvalidOperationException("Power grid save payload is empty.");
            if (json == null) throw new ArgumentNullException(nameof(json));

            var save = json.Deserialize<PowerGridSave>(raw);
            if (save == null)
                throw new InvalidOperationException("Power grid save payload could not be decoded.");
            if (save.saveVersion <= 0 || save.saveVersion > PowerGridSave.CurrentSaveVersion)
                throw new InvalidOperationException("Power grid save version is unsupported.");
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("Power grid save checksum is missing.");

            string expected = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, expected, StringComparison.Ordinal))
                throw new InvalidOperationException("Power grid save checksum mismatch.");

            save.Rooms ??= new List<PowerGridRoomSave>();
            save.State ??= new PowerGridState();
            return save;
        }
    }
}
