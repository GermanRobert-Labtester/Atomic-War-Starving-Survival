using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    internal sealed class ExpeditionJsonDto
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public int distanceTicks { get; set; } = 8;
        public float travelHours { get; set; }
        public int dangerLevel { get; set; } = 1;
        public float encounterChancePerTick { get; set; } = 0.12f;
        public float baseStaminaDrainPerHour { get; set; } = 2.0f;
        public List<string>? lootCategories { get; set; }
    }

    /// <summary>
    /// Shared Core loader for expedition definitions and locations from JSON (authority).
    /// Registers definitions into ExpeditionDefinitionRegistry and returns loaded list.
    /// Zero engine dependencies; adheres to Invariant 1 and Invariant 6.
    /// </summary>
    public static class ExpeditionCatalogLoader
    {
        public const string PrimaryFileName = "expeditions.json";

        private static readonly string[] LocationFiles =
        {
            "locations_expansion3.json",
            "locations.json",
            "year_of_ash_locations.json",
            "holdfast_locations.json"
        };

        public static List<ExpeditionDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = new List<ExpeditionDefinition>();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
                return result;

            // 1. Load primary expeditions.json
            string primaryPath = fileIO.Combine(dataDir, PrimaryFileName);
            if (fileIO.FileExists(primaryPath))
            {
                LoadFile(primaryPath, result, seen, fileIO, serializer);
            }

            // 2. Load locations with loot categories or travel definitions
            for (int i = 0; i < LocationFiles.Length; i++)
            {
                string locPath = fileIO.Combine(dataDir, LocationFiles[i]);
                if (fileIO.FileExists(locPath))
                {
                    LoadFile(locPath, result, seen, fileIO, serializer);
                }
            }

            // Register all into global ExpeditionDefinitionRegistry
            for (int i = 0; i < result.Count; i++)
            {
                ExpeditionDefinitionRegistry.Register(result[i]);
            }

            return result;
        }

        private static void LoadFile(
            string path,
            List<ExpeditionDefinition> result,
            HashSet<string> seen,
            IFileIO fileIO,
            IJsonSerializer serializer)
        {
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return;

            try
            {
                var dtos = CatalogLocator.LoadWrappedList<ExpeditionJsonDto>(raw, SystemTextJsonSerializer.Options);
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
                    if (seen.Contains(dto.id)) continue;

                    int ticks = dto.distanceTicks > 0
                        ? dto.distanceTicks
                        : (dto.travelHours > 0f ? (int)Math.Round(dto.travelHours * 2f) : 8);

                    float encounterChance = dto.encounterChancePerTick > 0f
                        ? dto.encounterChancePerTick
                        : Math.Clamp(0.10f + dto.dangerLevel * 0.02f, 0.05f, 0.50f);

                    float drain = dto.baseStaminaDrainPerHour > 0f
                        ? dto.baseStaminaDrainPerHour
                        : Math.Clamp(1.5f + dto.dangerLevel * 0.25f, 1.0f, 5.0f);

                    var def = new ExpeditionDefinition
                    {
                        id = dto.id,
                        displayName = !string.IsNullOrEmpty(dto.displayName) ? dto.displayName : dto.id,
                        distanceTicks = ticks > 0 ? ticks : 8,
                        dangerLevel = dto.dangerLevel > 0 ? dto.dangerLevel : 1,
                        encounterChancePerTick = encounterChance,
                        baseStaminaDrainPerHour = drain,
                        lootCategories = dto.lootCategories != null ? new List<string>(dto.lootCategories) : new List<string>()
                    };

                    seen.Add(def.id);
                    result.Add(def);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("ExpeditionCatalogLoader", path, ex);
            }
        }
    }
}
