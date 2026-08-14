using System;

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class YearOfAshSave
    {
        public const int CurrentSaveVersion = 1;

        public int saveVersion = CurrentSaveVersion;
        public int simDay = 180;
        public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
        public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
        public FactionWarSystemState factionWar = new FactionWarSystemState();

        /// <summary>
        /// Integrity hash computed over all payload fields.
        /// </summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Serialization codec for the 180-360 day Year of Ash expansion state.
    /// Uses the IJsonSerializer port so saves roundtrip between Godot and Unity.
    /// </summary>
    public static class YearOfAshSaveCodec
    {
        public static YearOfAshSave Capture(
            YearOfAshTimelineSystem timeline,
            DoorEncounterSystem encounters,
            FactionWarSystem factionWar,
            IClock clock)
        {
            var save = new YearOfAshSave
            {
                simDay = clock != null ? clock.Day : timeline.CurrentDay,
                timeline = timeline.CaptureState(),
                encounters = encounters.CaptureState(),
                factionWar = factionWar.CaptureState()
            };
            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        public static string Encode(YearOfAshSave save, IJsonSerializer json)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            if (string.IsNullOrEmpty(save.Checksum))
                save.Checksum = SaveChecksum.Compute(save);
            return json.Serialize(save);
        }

        public static YearOfAshSave Decode(string jsonText, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(jsonText))
                throw new InvalidOperationException("YearOfAshSave: empty save payload.");

            var save = json.Deserialize<YearOfAshSave>(jsonText);
            if (save == null)
                throw new InvalidOperationException("YearOfAshSave: deserialization returned null.");

            if (save.saveVersion > YearOfAshSave.CurrentSaveVersion)
                throw new InvalidOperationException(
                    $"YearOfAshSave: saveVersion {save.saveVersion} is newer than supported ({YearOfAshSave.CurrentSaveVersion}).");

            if (!string.IsNullOrEmpty(save.Checksum))
            {
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("YearOfAshSave: checksum mismatch (corrupted or tampered save).");
            }

            return save;
        }
    }
}
