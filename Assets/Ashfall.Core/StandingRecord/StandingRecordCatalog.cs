// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

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

    /// <summary>Standing Record faction card (standing_record_factions.json).</summary>
    public class StandingRecordFactionEntry
    {
        public string id;
        public string display_name;
        public string alignment;
        public string home_region;
        public bool is_active;
        public int trust;
        public string[] wants;
        public string[] offers;
        public string signature_quote;
        public string access_rule;
        public string badge_asset_id;
    }

    public sealed class StandingRecordCatalog
    {
        public List<StandingRecordQuestEntry> Quests { get; } = new List<StandingRecordQuestEntry>();
        public List<StandingRecordFactionEntry> Factions { get; } = new List<StandingRecordFactionEntry>();

        public StandingRecordQuestEntry? GetQuest(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Quests.Count; i++)
                if (Quests[i] != null && Quests[i].id == id)
                    return Quests[i];
            return null;
        }

        public StandingRecordFactionEntry? GetFaction(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Factions.Count; i++)
                if (Factions[i] != null && Factions[i].id == id)
                    return Factions[i];
            return null;
        }
    }

    /// <summary>
    /// Loads standing_record_quests.json and standing_record_factions.json via host ports.
    /// Engine-agnostic (shared with the Godot host).
    /// </summary>
    public sealed class StandingRecordCatalogLoader
    {
        public const string QuestsFile = "standing_record_quests.json";
        public const string FactionsFile = "standing_record_factions.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public StandingRecordCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
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

            string questsPath = _files.Combine(dataDirectory, QuestsFile);
            if (_files.FileExists(questsPath))
            {
                try
                {
                    string json = _files.ReadAllText(questsPath);
                    var items = CatalogLocator.LoadWrappedList<StandingRecordQuestEntry>(json, SystemTextJsonSerializer.Options);
                    if (items != null)
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i] != null)
                                catalog.Quests.Add(items[i]);
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Standing Record quests parse failed: " + e.Message);
                }
            }
            else
            {
                _log.Warn("Standing Record quests file missing: " + questsPath);
            }

            string factionsPath = _files.Combine(dataDirectory, FactionsFile);
            if (_files.FileExists(factionsPath))
            {
                try
                {
                    string json = _files.ReadAllText(factionsPath);
                    var factions = CatalogLocator.LoadWrappedList<StandingRecordFactionEntry>(json, SystemTextJsonSerializer.Options);
                    if (factions != null)
                    {
                        for (int i = 0; i < factions.Count; i++)
                        {
                            if (factions[i] != null)
                                catalog.Factions.Add(factions[i]);
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.Error("Standing Record factions parse failed: " + e.Message);
                }
            }

            return catalog;
        }
    }
}
