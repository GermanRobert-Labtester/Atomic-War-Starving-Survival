using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>Container shape for shelter_schedules.json (the authority).</summary>
    [Serializable]
    public sealed class ShelterScheduleCatalogContainer
    {
        public List<ScheduleDefinition> schedules = new List<ScheduleDefinition>();
    }

    /// <summary>
    /// Loads shelter schedule definitions from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class ShelterScheduleCatalogLoader
    {
        public const string DefaultFileName = "shelter_schedules.json";

        public static List<ScheduleDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<ScheduleDefinition>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<ScheduleDefinition>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<ScheduleDefinition>();

            var container = json.Deserialize<ShelterScheduleCatalogContainer>(rawText);
            return container?.schedules ?? new List<ScheduleDefinition>();
        }

        public static int LoadAndRegister(
            ShelterScheduleSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var defs = Load(dataDir, fileIO, json);
            system.LoadCatalog(defs);
            return defs.Count;
        }
    }
}
