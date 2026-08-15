using System;
using System.Collections.Generic;

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class DoorEncounterCatalogContainer
    {
        public List<DoorEncounterEntry> entries = new List<DoorEncounterEntry>();
    }

    /// <summary>
    /// Loads shelter door encounter definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class DoorEncounterCatalogLoader
    {
        public const string DefaultFileName = "door_encounters.json";

        public static List<DoorEncounterEntry> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<DoorEncounterEntry>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<DoorEncounterEntry>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<DoorEncounterEntry>();

            var container = json.Deserialize<DoorEncounterCatalogContainer>(rawText);
            return container?.entries ?? new List<DoorEncounterEntry>();
        }

        public static int LoadAndRegister(
            DoorEncounterSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var entries = Load(dataDir, fileIO, json);
            int count = 0;
            foreach (var entry in entries)
            {
                system.RegisterEncounter(entry);
                count++;
            }
            return count;
        }
    }
}
