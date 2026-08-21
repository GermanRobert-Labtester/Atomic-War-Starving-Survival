using System;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class YearOfAshSave
    {
        /// <summary>
        /// v3 adds the warlord doctrine/territory section (adaptive warlord AI).
        /// v1 and v2 saves migrate with a fresh toll-doctrine warlord (home
        /// controlled, no knowledge) — older saves load safely.
        /// </summary>
        public const int CurrentSaveVersion = 3;

        public int saveVersion = CurrentSaveVersion;
        public int simDay = 180;
        public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
        public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
        public FactionWarSystemState factionWar = new FactionWarSystemState();
        public WarlordDoctrineState warlord = new WarlordDoctrineState();

        // v2 sections. Before these existed the host ticked deep-freeze, radon and
        // questline state every day but never wrote them, so a reload silently reset
        // a degraded scrubber, an iced intake and every resolved questline.
        public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
        public YearOfAshRadonState radon = new YearOfAshRadonState();
        public QuestlineSystemState quests = new QuestlineSystemState();

        /// <summary>
        /// Integrity hash computed over all payload fields.
        /// </summary>
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v1 envelope shape (no deep-freeze, radon or questline sections). Kept so a
    /// v1 file on disk validates against the field set it was actually hashed with —
    /// <see cref="SaveChecksum"/> walks public fields, so validating a v1 payload against
    /// the v2 shape would always mismatch. Do not add fields here.
    /// </summary>
    [Serializable]
    public class YearOfAshSaveV1
    {
        public int saveVersion = 1;
        public int simDay = 180;
        public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
        public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
        public FactionWarSystemState factionWar = new FactionWarSystemState();
        public string Checksum = string.Empty;
    }

    /// <summary>
    /// Frozen v2 envelope shape (deep-freeze, radon and questline sections, but
    /// no warlord doctrine section). Do not add fields here — it must match what
    /// v2 wrote byte-for-byte in field set.
    /// </summary>
    [Serializable]
    public class YearOfAshSaveV2
    {
        public int saveVersion = 2;
        public int simDay = 180;
        public YearOfAshTimelineState timeline = new YearOfAshTimelineState();
        public DoorEncounterSystemState encounters = new DoorEncounterSystemState();
        public FactionWarSystemState factionWar = new FactionWarSystemState();
        public YearOfAshDeepFreezeState deepFreeze = new YearOfAshDeepFreezeState();
        public YearOfAshRadonState radon = new YearOfAshRadonState();
        public QuestlineSystemState quests = new QuestlineSystemState();
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
            IClock clock,
            YearOfAshDeepFreezeSystem deepFreeze = null!,
            YearOfAshRadonSystem radon = null!,
            QuestlineSystem quests = null!,
            WarlordDoctrineSystem warlord = null!)
        {
            var save = new YearOfAshSave
            {
                simDay = clock != null ? clock.Day : timeline.CurrentDay,
                timeline = timeline.CaptureState(),
                encounters = encounters.CaptureState(),
                factionWar = factionWar.CaptureState()
            };

            // A caller that does not own a system leaves that section at its defaults
            // rather than writing nulls, so the envelope shape stays constant.
            if (deepFreeze != null) save.deepFreeze = deepFreeze.CaptureState();
            if (radon != null) save.radon = radon.CaptureState();
            if (quests != null) save.quests = quests.CaptureState();
            if (warlord != null) save.warlord = warlord.CaptureState();

            save.Checksum = SaveChecksum.Compute(save);
            return save;
        }

        /// <summary>
        /// Restores every captured section into the live systems. Each RestoreState
        /// tolerates a null section, and the sim day rides the timeline snapshot.
        /// </summary>
        public static void Restore(
            YearOfAshSave save,
            YearOfAshTimelineSystem timeline,
            DoorEncounterSystem encounters,
            FactionWarSystem factionWar,
            YearOfAshDeepFreezeSystem deepFreeze = null!,
            YearOfAshRadonSystem radon = null!,
            QuestlineSystem quests = null!,
            WarlordDoctrineSystem warlord = null!)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            timeline.RestoreState(save.timeline);
            encounters.RestoreState(save.encounters);
            factionWar.RestoreState(save.factionWar);

            // Each RestoreState no-ops on a null section, so a migrated v1/v2 save leaves
            // these systems at their constructor defaults instead of zeroing them.
            if (deepFreeze != null) deepFreeze.RestoreState(save.deepFreeze);
            if (radon != null) radon.RestoreState(save.radon);
            if (quests != null) quests.RestoreState(save.quests);
            if (warlord != null) warlord.RestoreState(save.warlord);

            // Keep simDay authoritative if the timeline section was absent.
            if (save.timeline == null && save.simDay > 0)
                timeline.AdvanceDay(save.simDay);
        }

        public static string Encode(YearOfAshSave save, IJsonSerializer json)
        {
            if (save == null)
                throw new ArgumentNullException(nameof(save));
            // Always recompute: a caller may have mutated a captured save after
            // Capture() stamped it, and a stale checksum would poison the file.
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

            // A v1 file was hashed over the v1 field set. Validate it against the frozen
            // v1 shape and upgrade in place; the new sections start at system defaults.
            if (save.saveVersion < YearOfAshSave.CurrentSaveVersion)
                return MigrateToCurrent(jsonText, json, save.saveVersion);

            if (!string.IsNullOrEmpty(save.Checksum))
            {
                string actual = SaveChecksum.Compute(save);
                if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
                    throw new InvalidOperationException("YearOfAshSave: checksum mismatch (corrupted or tampered save).");
            }

            return save;
        }

        /// <summary>
        /// Upgrades an older envelope to the current shape. Each legacy version is parsed
        /// as its FROZEN type so the checksum is verified over exactly the fields that
        /// version wrote — anything a newer version added is dropped, never trusted.
        /// </summary>
        private static YearOfAshSave MigrateToCurrent(string jsonText, IJsonSerializer json, int version)
        {
            if (version == 1)
            {
                var v1 = json.Deserialize<YearOfAshSaveV1>(jsonText);
                if (v1 == null)
                    throw new InvalidOperationException("YearOfAshSave: v1 deserialization returned null.");

                if (!string.IsNullOrEmpty(v1.Checksum))
                {
                    string actual = SaveChecksum.Compute(v1);
                    if (!string.Equals(v1.Checksum, actual, StringComparison.Ordinal))
                        throw new InvalidOperationException("YearOfAshSave: checksum mismatch (corrupted or tampered save).");
                }

                var upgraded = new YearOfAshSave
                {
                    saveVersion = YearOfAshSave.CurrentSaveVersion,
                    simDay = v1.simDay,
                    timeline = v1.timeline,
                    encounters = v1.encounters,
                    factionWar = v1.factionWar
                    // deepFreeze / radon / quests / warlord stay at their field
                    // initialisers, which are the same defaults a fresh system
                    // would construct.
                };
                upgraded.Checksum = SaveChecksum.Compute(upgraded);
                return upgraded;
            }

            if (version == 2)
            {
                var v2 = json.Deserialize<YearOfAshSaveV2>(jsonText);
                if (v2 == null)
                    throw new InvalidOperationException("YearOfAshSave: v2 deserialization returned null.");

                if (!string.IsNullOrEmpty(v2.Checksum))
                {
                    string actual = SaveChecksum.Compute(v2);
                    if (!string.Equals(v2.Checksum, actual, StringComparison.Ordinal))
                        throw new InvalidOperationException("YearOfAshSave: checksum mismatch (corrupted or tampered save).");
                }

                var upgraded = new YearOfAshSave
                {
                    saveVersion = YearOfAshSave.CurrentSaveVersion,
                    simDay = v2.simDay,
                    timeline = v2.timeline,
                    encounters = v2.encounters,
                    factionWar = v2.factionWar,
                    deepFreeze = v2.deepFreeze,
                    radon = v2.radon,
                    quests = v2.quests
                    // warlord stays at its field initialiser (fresh toll doctrine).
                };
                upgraded.Checksum = SaveChecksum.Compute(upgraded);
                return upgraded;
            }

            throw new InvalidOperationException(
                $"YearOfAshSave: no migration path from saveVersion {version}.");
        }
    }
}
