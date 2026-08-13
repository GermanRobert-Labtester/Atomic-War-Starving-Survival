using System;
using System.Collections.Generic;

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

    public sealed class DutyRosterCatalog
    {
        public List<DutyRosterLocationEntry> Locations { get; } = new List<DutyRosterLocationEntry>();
        public List<DutyRosterQuestEntry> Quests { get; } = new List<DutyRosterQuestEntry>();
        public List<DutyRosterMarkEntry> Marks { get; } = new List<DutyRosterMarkEntry>();

        public DutyRosterLocationEntry GetLocation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Locations.Count; i++)
                if (Locations[i] != null && Locations[i].id == id)
                    return Locations[i];
            return null;
        }

        public DutyRosterQuestEntry GetQuest(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Quests.Count; i++)
                if (Quests[i] != null && Quests[i].id == id)
                    return Quests[i];
            return null;
        }

        public DutyRosterMarkEntry GetMark(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Marks.Count; i++)
                if (Marks[i] != null && Marks[i].id == id)
                    return Marks[i];
            return null;
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

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public DutyRosterCatalogLoader(IFileIO files, IJsonSerializer json, ILog log = null)
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
            return catalog;
        }

        private void LoadList<T>(string path, List<T> dest, string label) where T : class
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Duty Roster " + label + " file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var items = _json.Deserialize<List<T>>(json);
                if (items == null) return;
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
