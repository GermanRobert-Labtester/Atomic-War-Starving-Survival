using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    public class DutyRosterLocationEntry
    {
        public string id;
        public string displayName;
        public string inspect;
        public string description;
        public float dangerLevel;
        public float travelHours;
        public float baseRadsPerHour;
        public string region;
        public bool overlay_on_unlock;
        public bool recast_always;
    }

    public class DutyRosterQuestStageEntry
    {
        public string id;
        public string text;
    }

    public class DutyRosterQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    public class DutyRosterQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public DutyRosterQuestStageEntry[] stages;
        public DutyRosterQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;
        public string complete_mutation;
        public string fail_mutation;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    public class DutyRosterMarkEntry
    {
        public string id;
        public string later;
        public string situation;
    }

    /// <summary>
    /// Second Winter season profile (spec §5.4 — data, not a 4th simulation class).
    /// Consumed by IceRoadSystem + ShelterEncounterSystem + heater/filter ticks.
    /// </summary>
    public class DutyRosterSeasonEntry
    {
        public string id = DutyRosterIds.SeasonSecondWinter;

        // Plan 47 wave 1: canonical snake_case wire keys; legacy camelCase keys
        // are accepted through CatalogKeyNormalizer (docs/data/SNAKE_CASE_MIGRATION.md).
        [JsonPropertyName("window_min_days")]
        public int windowMinDays = DutyRosterIds.SecondWinterWindowMinDays;
        [JsonPropertyName("window_max_days")]
        public int windowMaxDays = DutyRosterIds.SecondWinterWindowMaxDays;
        [JsonPropertyName("encounter_weight")]
        public float encounterWeight = DutyRosterIds.SecondWinterEncounterWeight;
        [JsonPropertyName("steam_trip_chance_boost")]
        public float steamTripChanceBoost;
    }

    public sealed class DutyRosterCatalog
    {
        public List<DutyRosterLocationEntry> Locations { get; } = new List<DutyRosterLocationEntry>();
        public List<DutyRosterQuestEntry> Quests { get; } = new List<DutyRosterQuestEntry>();
        public List<DutyRosterMarkEntry> Marks { get; } = new List<DutyRosterMarkEntry>();
        public List<DutyRosterSeasonEntry> Seasons { get; } = new List<DutyRosterSeasonEntry>();

        public DutyRosterLocationEntry? GetLocation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Locations.Count; i++)
                if (Locations[i] != null && Locations[i].id == id)
                    return Locations[i];
            return null;
        }

        public DutyRosterQuestEntry? GetQuest(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Quests.Count; i++)
                if (Quests[i] != null && Quests[i].id == id)
                    return Quests[i];
            return null;
        }

        public DutyRosterMarkEntry? GetMark(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Marks.Count; i++)
                if (Marks[i] != null && Marks[i].id == id)
                    return Marks[i];
            return null;
        }

        public DutyRosterSeasonEntry? GetSeason(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Seasons.Count; i++)
                if (Seasons[i] != null && Seasons[i].id == id)
                    return Seasons[i];
            return null;
        }

        /// <summary>
        /// Resolves the active roster season for a campaign day (pure
        /// derivation from the authoritative campaign clock — no state).
        /// Windows are inclusive on both bounds; the matching rule is the
        /// repository's open-ended window convention (last season whose
        /// windowMinDays &lt;= day — see WeatherSystem.GetSeasonForDay), so
        /// days beyond the final window carry the last season forward and
        /// days before the first window return null. Ties keep the first
        /// listed entry, making selection deterministic regardless of file
        /// ordering.
        /// </summary>
        public DutyRosterSeasonEntry? GetSeasonForDay(int day)
        {
            DutyRosterSeasonEntry? best = null;
            for (int i = 0; i < Seasons.Count; i++)
            {
                var s = Seasons[i];
                if (s == null || day < s.windowMinDays) continue;
                if (best == null || s.windowMinDays > best.windowMinDays)
                    best = s;
            }
            return best;
        }
    }

    /// <summary>
    /// Loads duty_roster_locations.json / duty_roster_quests.json /
    /// duty_roster_marks.json from the shared StreamingAssets/Data directory.
    /// No ScriptableObject materialisation. No JsonUtility.
    /// </summary>
    public sealed class DutyRosterCatalogLoader
    {
        public const string LocationsFile = "duty_roster_locations.json";
        public const string QuestsFile = "duty_roster_quests.json";
        public const string MarksFile = "duty_roster_marks.json";
        public const string SeasonsFile = "duty_roster_seasons.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public DutyRosterCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public DutyRosterCatalog Load(string dataDirectory)
        {
            var catalog = new DutyRosterCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Duty Roster catalog directory missing: " + dataDirectory);
                return catalog;
            }

            LoadList(_files.Combine(dataDirectory, LocationsFile), catalog.Locations, "locations");
            LoadList(_files.Combine(dataDirectory, QuestsFile), catalog.Quests, "quests");
            LoadList(_files.Combine(dataDirectory, MarksFile), catalog.Marks, "marks");
            LoadList(_files.Combine(dataDirectory, SeasonsFile), catalog.Seasons, "seasons", SeasonKeyAliases);
            return catalog;
        }

        /// <summary>Plan 47 wave 1: legacy camelCase → canonical snake_case (spelling-only).</summary>
        public static readonly IReadOnlyDictionary<string, string> SeasonKeyAliases = new Dictionary<string, string>
        {
            ["windowMinDays"] = "window_min_days",
            ["windowMaxDays"] = "window_max_days",
            ["encounterWeight"] = "encounter_weight",
            ["steamTripChanceBoost"] = "steam_trip_chance_boost",
        };

        private void LoadList<T>(string path, List<T> dest, string label, IReadOnlyDictionary<string, string>? keyAliases = null) where T : class
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Duty Roster " + label + " file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                if (keyAliases != null && keyAliases.Count > 0)
                {
                    // Dual-read: legacy camelCase accepted, conflicts fail loudly.
                    json = IO.CatalogKeyNormalizer.Normalize(json, keyAliases, System.IO.Path.GetFileName(path));
                }
                var items = CatalogLocator.LoadWrappedList<T>(json, SystemTextJsonSerializer.Options);
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] != null)
                        dest.Add(items[i]);
                }
            }
            catch (Exception e)
            {
                _log.Error("Duty Roster " + label + " parse failed: " + e.Message);
            }
        }
    }
}
