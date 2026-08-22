using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    public enum ShelterAssignmentStatus
    {
        Active = 0,
        Suspended = 1,
        Ended = 2
    }

    [Serializable]
    public sealed class ShelterRoomSave
    {
        public string RoomId = string.Empty;
        public string DisplayName = string.Empty;
        public int Capacity;
    }

    [Serializable]
    public sealed class ShelterAssignment
    {
        public string SurvivorId = string.Empty;
        public string RoomId = string.Empty;
        public string WorkstationId = string.Empty;
        public int AssignedDay;
        public ShelterAssignmentStatus Status;
    }

    [Serializable]
    public sealed class ShelterAssignmentState
    {
        public List<ShelterAssignment> Assignments = new List<ShelterAssignment>();
    }

    /// <summary>
    /// Versioned, engine-agnostic persistence envelope for shelter assignment data.
    /// The runtime assignment coordinator is intentionally not defined here: this
    /// contract exists independently so save integrity is stable before/after host wiring.
    /// </summary>
    [Serializable]
    public sealed class ShelterAssignmentSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public List<ShelterRoomSave> Rooms = new List<ShelterRoomSave>();
        public ShelterAssignmentState State = new ShelterAssignmentState();
        public string Checksum = string.Empty;
    }

    public static class ShelterAssignmentSaveCodec
    {
        public static string EncodeToString(ShelterAssignmentSave save, IJsonSerializer json)
        {
            if (save == null) throw new ArgumentNullException(nameof(save));
            if (json == null) throw new ArgumentNullException(nameof(json));
            if (save.saveVersion <= 0) save.saveVersion = ShelterAssignmentSave.CurrentSaveVersion;
            if (save.saveVersion > ShelterAssignmentSave.CurrentSaveVersion)
                throw new InvalidOperationException("Shelter assignment save version is newer than this build supports.");

            save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static ShelterAssignmentSave Decode(string raw, IJsonSerializer json)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new InvalidOperationException("Shelter assignment save payload is empty.");
            if (json == null) throw new ArgumentNullException(nameof(json));

            var save = json.Deserialize<ShelterAssignmentSave>(raw);
            if (save == null)
                throw new InvalidOperationException("Shelter assignment save payload could not be decoded.");
            if (save.saveVersion <= 0 || save.saveVersion > ShelterAssignmentSave.CurrentSaveVersion)
                throw new InvalidOperationException("Shelter assignment save version is unsupported.");
            if (string.IsNullOrEmpty(save.Checksum))
                throw new InvalidOperationException("Shelter assignment save checksum is missing.");

            string expected = SaveChecksum.Compute(save);
            if (!string.Equals(save.Checksum, expected, StringComparison.Ordinal))
                throw new InvalidOperationException("Shelter assignment save checksum mismatch.");

            save.Rooms ??= new List<ShelterRoomSave>();
            save.State ??= new ShelterAssignmentState();
            save.State.Assignments ??= new List<ShelterAssignment>();
            return save;
        }
    }
}
