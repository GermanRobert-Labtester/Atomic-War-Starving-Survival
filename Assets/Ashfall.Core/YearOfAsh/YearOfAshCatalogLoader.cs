using System;
using System.Collections.Generic;

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class YearOfAshItemEntry
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string category = string.Empty;
        public string description = string.Empty;
        public float tradeValue = 0f;
        public float weightKg = 0f;
    }

    [Serializable]
    public class YearOfAshEventEntry
    {
        public string id = string.Empty;
        public string title = string.Empty;
        public string description = string.Empty;
        public int day = 180;
        public string hazardType = string.Empty;
        public string phase = string.Empty;
        public float temperatureDeltaC = 0f;
    }

    [Serializable]
    public class YearOfAshLocationEntry
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string sector = string.Empty;
        public int riskLevel = 1;
        public float radiationUsv = 0f;
        public string description = string.Empty;
    }

    [Serializable]
    public class YearOfAshRadioEntry
    {
        public string id = string.Empty;
        public string frequency = string.Empty;
        public int dayTrigger = 180;
        public bool isEmergency = false;
        public string message = string.Empty;
        public string signalStrength = string.Empty; // "S7" etc. — the radio signal scale, not a number
        public string source = string.Empty;
    }

    [Serializable]
    public class YearOfAshSurvivorEntry
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string occupation = string.Empty;
        public float rurScore = 10.0f;
        public string moralAlignment = string.Empty;
        public int age = 30;
        public float healthPercent = 100f;
        public float radiationDoseMsv = 0f;
        public int guiltScore = 0;
        public string backstory = string.Empty;
        public string confession = string.Empty;
        public string factionAffinity = string.Empty;
        public List<string> traits = new List<string>();
    }

    [Serializable]
    public class YearOfAshItemContainer { public List<YearOfAshItemEntry> items = new List<YearOfAshItemEntry>(); }

    [Serializable]
    public class YearOfAshEventContainer { public List<YearOfAshEventEntry> events = new List<YearOfAshEventEntry>(); }

    [Serializable]
    public class YearOfAshLocationContainer { public List<YearOfAshLocationEntry> locations = new List<YearOfAshLocationEntry>(); }

    [Serializable]
    public class YearOfAshRadioContainer { public List<YearOfAshRadioEntry> broadcasts = new List<YearOfAshRadioEntry>(); }

    [Serializable]
    public class YearOfAshSurvivorContainer { public List<YearOfAshSurvivorEntry> survivors = new List<YearOfAshSurvivorEntry>(); }

    [Serializable]
    public class YearOfAshQuestContainer { public List<QuestlineDefinition> quests = new List<QuestlineDefinition>(); }

    [Serializable]
    public class RawQuestEntry
    {
        public string id = string.Empty;
        public string questlineId = string.Empty;
        public string title = string.Empty;
        public string synopsis = string.Empty;
        public string faction = string.Empty;
        public string factionTag = string.Empty;
        public int minDay = 180;
        public int maxDay = 360;
        public List<RawQuestStage> stages = new List<RawQuestStage>();
    }

    [Serializable]
    public class RawQuestStage
    {
        public string stageId = string.Empty;
        public int stageIndex = 0;
        public string objective = string.Empty;
        public string title = string.Empty;
        public string narrativePrompt = string.Empty;
        public string requiredItemId = string.Empty;
        public bool isCompleted = false;
    }

    [Serializable]
    public class RawQuestContainer
    {
        public List<RawQuestEntry> quests = new List<RawQuestEntry>();
    }

    /// <summary>
    /// Engine-agnostic catalog loader for Year of Ash items, events, locations, radio broadcasts, and survivors.
    /// Uses IFileIO and IJsonSerializer ports. Zero engine namespaces.
    /// </summary>
    public static class YearOfAshCatalogLoader
    {
        public const string ItemsFile = "year_of_ash_items.json";
        public const string EventsFile = "year_of_ash_events.json";
        public const string QuestsFile = "year_of_ash_quests.json";
        public const string LocationsFile = "year_of_ash_locations.json";
        public const string RadioFile = "year_of_ash_radio.json";
        public const string SurvivorsFile = "year_of_ash_survivors.json";

        public static List<YearOfAshItemEntry> LoadItems(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<YearOfAshItemEntry>();

            string path = fileIO.Combine(dataDir, ItemsFile);
            if (!fileIO.FileExists(path))
                return new List<YearOfAshItemEntry>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<YearOfAshItemEntry>();

            try
            {
                var container = json.Deserialize<YearOfAshItemContainer>(raw);
                if (container != null && container.items != null && container.items.Count > 0)
                    return container.items;
            }
            catch { }

            try
            {
                return json.Deserialize<List<YearOfAshItemEntry>>(raw) ?? new List<YearOfAshItemEntry>();
            }
            catch
            {
                return new List<YearOfAshItemEntry>();
            }
        }

        public static List<YearOfAshEventEntry> LoadEvents(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<YearOfAshEventEntry>();

            string path = fileIO.Combine(dataDir, EventsFile);
            if (!fileIO.FileExists(path))
                return new List<YearOfAshEventEntry>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<YearOfAshEventEntry>();

            try
            {
                var container = json.Deserialize<YearOfAshEventContainer>(raw);
                if (container != null && container.events != null && container.events.Count > 0)
                    return container.events;
            }
            catch { }

            try
            {
                return json.Deserialize<List<YearOfAshEventEntry>>(raw) ?? new List<YearOfAshEventEntry>();
            }
            catch
            {
                return new List<YearOfAshEventEntry>();
            }
        }

        public static List<QuestlineDefinition> LoadQuests(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<QuestlineDefinition>();

            string path = fileIO.Combine(dataDir, QuestsFile);
            if (!fileIO.FileExists(path))
                return new List<QuestlineDefinition>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<QuestlineDefinition>();

            try
            {
                var rawContainer = json.Deserialize<RawQuestContainer>(raw);
                if (rawContainer != null && rawContainer.quests != null && rawContainer.quests.Count > 0)
                {
                    var result = new List<QuestlineDefinition>();
                    foreach (var rq in rawContainer.quests)
                    {
                        var def = new QuestlineDefinition
                        {
                            questlineId = !string.IsNullOrEmpty(rq.questlineId) ? rq.questlineId : rq.id,
                            title = rq.title,
                            synopsis = rq.synopsis,
                            factionTag = !string.IsNullOrEmpty(rq.factionTag) ? rq.factionTag : rq.faction,
                            minDay = rq.minDay,
                            maxDay = rq.maxDay
                        };
                        for (int i = 0; i < rq.stages.Count; i++)
                        {
                            var rs = rq.stages[i];
                            bool isLast = i == rq.stages.Count - 1;
                            string nextId = isLast ? null : (i + 1 < rq.stages.Count ? (!string.IsNullOrEmpty(rq.stages[i + 1].stageId) ? rq.stages[i + 1].stageId : $"stage_{rq.stages[i + 1].stageIndex}") : null);
                            var stage = new QuestStage
                            {
                                stageId = !string.IsNullOrEmpty(rs.stageId) ? rs.stageId : $"stage_{rs.stageIndex}",
                                title = !string.IsNullOrEmpty(rs.title) ? rs.title : rs.objective,
                                narrativePrompt = !string.IsNullOrEmpty(rs.narrativePrompt) ? rs.narrativePrompt : rs.objective,
                                isTerminal = isLast
                            };
                            if (!isLast)
                            {
                                stage.choices.Add(new QuestChoice
                                {
                                    choiceId = $"choice_{stage.stageId}_proceed",
                                    text = "Proceed with objective",
                                    nextStageId = nextId ?? string.Empty
                                });
                            }
                            def.stages.Add(stage);
                        }
                        if (def.stages.Count > 0)
                            def.firstStageId = def.stages[0].stageId;
                        result.Add(def);
                    }
                    return result;
                }
            }
            catch { }

            try
            {
                var container = json.Deserialize<YearOfAshQuestContainer>(raw);
                if (container != null && container.quests != null && container.quests.Count > 0)
                    return container.quests;
            }
            catch { }

            try
            {
                return json.Deserialize<List<QuestlineDefinition>>(raw) ?? new List<QuestlineDefinition>();
            }
            catch
            {
                return new List<QuestlineDefinition>();
            }
        }

        public static List<YearOfAshLocationEntry> LoadLocations(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<YearOfAshLocationEntry>();

            string path = fileIO.Combine(dataDir, LocationsFile);
            if (!fileIO.FileExists(path))
                return new List<YearOfAshLocationEntry>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<YearOfAshLocationEntry>();

            try
            {
                var container = json.Deserialize<YearOfAshLocationContainer>(raw);
                if (container != null && container.locations != null && container.locations.Count > 0)
                    return container.locations;
            }
            catch { }

            try
            {
                return json.Deserialize<List<YearOfAshLocationEntry>>(raw) ?? new List<YearOfAshLocationEntry>();
            }
            catch
            {
                return new List<YearOfAshLocationEntry>();
            }
        }

        public static List<YearOfAshRadioEntry> LoadRadioBroadcasts(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<YearOfAshRadioEntry>();

            string path = fileIO.Combine(dataDir, RadioFile);
            if (!fileIO.FileExists(path))
                return new List<YearOfAshRadioEntry>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<YearOfAshRadioEntry>();

            try
            {
                var container = json.Deserialize<YearOfAshRadioContainer>(raw);
                if (container != null && container.broadcasts != null && container.broadcasts.Count > 0)
                    return container.broadcasts;
            }
            catch { }

            try
            {
                return json.Deserialize<List<YearOfAshRadioEntry>>(raw) ?? new List<YearOfAshRadioEntry>();
            }
            catch
            {
                return new List<YearOfAshRadioEntry>();
            }
        }

        public static List<YearOfAshSurvivorEntry> LoadSurvivors(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<YearOfAshSurvivorEntry>();

            string path = fileIO.Combine(dataDir, SurvivorsFile);
            if (!fileIO.FileExists(path))
                return new List<YearOfAshSurvivorEntry>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<YearOfAshSurvivorEntry>();

            try
            {
                var container = json.Deserialize<YearOfAshSurvivorContainer>(raw);
                if (container != null && container.survivors != null && container.survivors.Count > 0)
                    return container.survivors;
            }
            catch { }

            try
            {
                return json.Deserialize<List<YearOfAshSurvivorEntry>>(raw) ?? new List<YearOfAshSurvivorEntry>();
            }
            catch
            {
                return new List<YearOfAshSurvivorEntry>();
            }
        }

        public static int LoadAndRegisterQuests(QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (system == null) return 0;
            var quests = LoadQuests(dataDir, fileIO, json);
            int count = 0;
            foreach (var q in quests)
            {
                system.RegisterQuestline(q);
                count++;
            }
            return count;
        }
    }
}
