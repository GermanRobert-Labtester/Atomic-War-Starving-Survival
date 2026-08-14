using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>Standing Record quest card (standing_record_quests.json).</summary>
    public class StandingRecordQuestStageEntry
    {
        public string id;
        public string text;
    }

    public class StandingRecordQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    public class StandingRecordQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public StandingRecordQuestStageEntry[] stages;
        public StandingRecordQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;
        public string complete_mutation;
        public string fail_mutation;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    public sealed class StandingRecordCatalog
    {
        public List<StandingRecordQuestEntry> Quests { get; } = new List<StandingRecordQuestEntry>();

        public StandingRecordQuestEntry GetQuest(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Quests.Count; i++)
                if (Quests[i] != null && Quests[i].id == id)
                    return Quests[i];
            return null;
        }
    }

    /// <summary>
    /// Loads standing_record_quests.json via host ports. No ScriptableObject.
    /// No JsonUtility. Engine-agnostic (shared with the Godot host).
    /// </summary>
    public sealed class StandingRecordCatalogLoader
    {
        public const string QuestsFile = "standing_record_quests.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public StandingRecordCatalogLoader(IFileIO files, IJsonSerializer json, ILog log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public StandingRecordCatalog Load(string dataDirectory)
        {
            var catalog = new StandingRecordCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Standing Record catalog directory missing: " + dataDirectory);
                return catalog;
            }

            string path = _files.Combine(dataDirectory, QuestsFile);
            if (!_files.FileExists(path))
            {
                _log.Warn("Standing Record quests file missing: " + path);
                return catalog;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var items = _json.Deserialize<List<StandingRecordQuestEntry>>(json);
                if (items == null) return catalog;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] != null)
                        catalog.Quests.Add(items[i]);
                }
            }
            catch (Exception e)
            {
                _log.Error("Standing Record quests parse failed: " + e.Message);
            }
            return catalog;
        }
    }
}