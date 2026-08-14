using System;
using System.Collections.Generic;

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class YearOfAshItemEntry
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public string type = string.Empty;
        public int stackMax = 1;
        public float weight = 1.0f;
        public float tradeValue = 10f;
        public float thirstRestore = 0f;
        public float hungerRestore = 0f;
        public float moraleEffect = 0f;
    }

    [Serializable]
    public class YearOfAshEventEntry
    {
        public string id = string.Empty;
        public string title = string.Empty;
        public string bodyText = string.Empty;
        public float weight = 1.0f;
        public int minDay = 180;
        public int maxDay = 360;
        public string phase = string.Empty;
    }

    [Serializable]
    public class YearOfAshLocationEntry
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public int dangerLevel = 5;
        public int travelHours = 4;
        public float baseRadsPerHour = 20.0f;
        public float collapseRisk = 0.3f;
        public string primaryFaction = string.Empty;
        public List<string> lootCategories = new List<string>();
        public List<string> specialEvents = new List<string>();
    }

    [Serializable]
    public class YearOfAshRadioEntry
    {
        public string id = string.Empty;
        public string callSign = string.Empty;
        public string frequency = string.Empty;
        public int minDay = 180;
        public int maxDay = 360;
        public string bodyText = string.Empty;
        public bool isEmergency = false;
    }

    [Serializable]
    public class YearOfAshSurvivorEntry
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string occupation = string.Empty;
        public float rurScore = 10.0f;
        public string moralBranch = "humanist";
        public string background = string.Empty;
        public List<string> traits = new List<string>();
        public int startingMorale = 50;
        public int startingGuilt = 10;
        public string favoredFaction = string.Empty;
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

            return json.Deserialize<List<YearOfAshItemEntry>>(raw) ?? new List<YearOfAshItemEntry>();
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

            return json.Deserialize<List<YearOfAshEventEntry>>(raw) ?? new List<YearOfAshEventEntry>();
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

            return json.Deserialize<List<QuestlineDefinition>>(raw) ?? new List<QuestlineDefinition>();
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

            return json.Deserialize<List<YearOfAshLocationEntry>>(raw) ?? new List<YearOfAshLocationEntry>();
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

            return json.Deserialize<List<YearOfAshRadioEntry>>(raw) ?? new List<YearOfAshRadioEntry>();
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

            return json.Deserialize<List<YearOfAshSurvivorEntry>>(raw) ?? new List<YearOfAshSurvivorEntry>();
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
