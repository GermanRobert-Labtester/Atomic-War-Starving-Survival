using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Definition of a starting bunker survivor from starting_survivors.json (the authority).
    /// </summary>
    [Serializable]
    public class StartingSurvivorDefinition
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public float health { get; set; } = 100f;
        public float hunger { get; set; } = 0f;
        public float thirst { get; set; } = 0f;
        public float warmth { get; set; } = 100f;
        public float morale { get; set; } = 50f;
        public float lifetimeDose { get; set; } = 0f;
        public bool acuteRad { get; set; } = false;
        public int joinedDay { get; set; } = 0;
    }

    /// <summary>
    /// Shared Core loader for the starting survivor roster and their initial conditions.
    /// Zero engine dependencies; adheres to Invariant 1 and Invariant 6.
    /// </summary>
    public static class SurvivorStartingStateLoader
    {
        public const string FileName = "starting_survivors.json";

        public static List<StartingSurvivorDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = new List<StartingSurvivorDefinition>();
            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var dtos = CatalogLocator.LoadWrappedList<StartingSurvivorDefinition>(raw, SystemTextJsonSerializer.Options);
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
                    if (string.IsNullOrEmpty(dto.displayName)) dto.displayName = dto.id;
                    result.Add(dto);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("SurvivorStartingStateLoader", FileName, ex);
            }

            return result;
        }
    }
}
