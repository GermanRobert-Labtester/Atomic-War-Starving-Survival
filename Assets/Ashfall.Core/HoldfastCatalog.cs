using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    public class HoldfastLocationEntry
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

    public class HoldfastQuestStageEntry
    {
        public string id;
        public string text;
    }

    public class HoldfastQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    public class HoldfastQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public HoldfastQuestStageEntry[] stages;
        public HoldfastQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    public sealed class HoldfastCatalog
    {
        public List<HoldfastLocationEntry> Locations { get; } = new List<HoldfastLocationEntry>();
        public List<HoldfastQuestEntry> Quests { get; } = new List<HoldfastQuestEntry>();

        public HoldfastLocationEntry GetLocation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Locations.Count; i++)
                if (Locations[i] != null && Locations[i].id == id)
                    return Locations[i];
            return null;
        }

        public HoldfastQuestEntry GetQuest(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Quests.Count; i++)
                if (Quests[i] != null && Quests[i].id == id)
                    return Quests[i];
            return null;
        }
    }

    /// <summary>
    /// Loads holdfast_locations.json / holdfast_quests.json from the shared
    /// StreamingAssets/Data directory. No ScriptableObject materialisation.
    /// </summary>
    public sealed class HoldfastCatalogLoader
    {
        public const string LocationsFile = "holdfast_locations.json";
        public const string QuestsFile = "holdfast_quests.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public HoldfastCatalogLoader(IFileIO files, IJsonSerializer json, ILog log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        /// <param name="expansionUnlocked">
        /// When false, the 26 District 8 cards stay dark. recast_always (3 Sector 4 ids)
        /// still load so copy overlays can apply. overlay_on_unlock rows wait on unlock.
        /// </param>
        public HoldfastCatalog Load(string dataDirectory, bool expansionUnlocked = true)
        {
            var catalog = new HoldfastCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Holdfast catalog directory missing: " + dataDirectory);
                return catalog;
            }

            LoadLocations(_files.Combine(dataDirectory, LocationsFile), catalog.Locations, expansionUnlocked);
            LoadList(_files.Combine(dataDirectory, QuestsFile), catalog.Quests, "quests");
            return catalog;
        }

        public static bool IncludeLocation(HoldfastLocationEntry e, bool expansionUnlocked)
        {
            if (e == null || string.IsNullOrEmpty(e.id)) return false;
            if (e.recast_always) return true;
            return expansionUnlocked;
        }

        /// <summary>Strips authoring notes such as "(existing)" / "(recast; existing id)" from display names.</summary>
        public static string StripAuthorNotes(string displayName)
        {
            if (string.IsNullOrEmpty(displayName)) return displayName;
            int idx = displayName.IndexOf(" (existing", StringComparison.Ordinal);
            if (idx > 0) return displayName.Substring(0, idx);
            idx = displayName.IndexOf(" (recast", StringComparison.Ordinal);
            if (idx > 0) return displayName.Substring(0, idx);
            return displayName;
        }

        private void LoadLocations(string path, List<HoldfastLocationEntry> dest, bool expansionUnlocked)
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Holdfast locations file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var items = _json.Deserialize<List<HoldfastLocationEntry>>(json);
                if (items == null) return;
                for (int i = 0; i < items.Count; i++)
                {
                    var e = items[i];
                    if (e == null) continue;
                    e.displayName = StripAuthorNotes(e.displayName);
                    if (!IncludeLocation(e, expansionUnlocked)) continue;
                    dest.Add(e);
                }
            }
            catch (Exception e)
            {
                _log.Error("Holdfast locations parse failed: " + e.Message);
            }
        }

        private void LoadList<T>(string path, List<T> dest, string label) where T : class
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Holdfast " + label + " file missing: " + path);
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
                _log.Error("Holdfast " + label + " parse failed: " + e.Message);
            }
        }
    }
}
