// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.IO;

namespace Ashfall.Core.Economy
{
    public sealed class SeasonalMigrationCatalogLoadResult
    {
        public bool Success => Errors.Count == 0;
        public SeasonalMigrationCatalog Catalog { get; set; } = new();
        public List<string> Errors { get; } = new();
    }

    /// <summary>
    /// Strict catalog loader for seasonal_human_migration.json (Plan 199 / C3-199).
    /// </summary>
    public static class SeasonalMigrationCatalogLoader
    {
        public const string DefaultFileName = "seasonal_human_migration.json";

        public static SeasonalMigrationCatalogLoadResult Load(string dataDir, IFileIO? fileIO = null)
        {
            fileIO ??= new FileSystemIO();
            var result = new SeasonalMigrationCatalogLoadResult();

            if (string.IsNullOrWhiteSpace(dataDir))
            {
                result.Errors.Add("Data directory is empty.");
                return result;
            }

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add($"File not found: {path}");
                return result;
            }

            try
            {
                string raw = fileIO.ReadAllText(path);
                return LoadFromJson(raw);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to read {path}: {ex.Message}");
                return result;
            }
        }

        public static SeasonalMigrationCatalogLoadResult LoadFromJson(string json)
        {
            var result = new SeasonalMigrationCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("JSON content is empty.");
                return result;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var catalog = JsonSerializer.Deserialize<SeasonalMigrationCatalog>(json, options);
                if (catalog == null)
                {
                    result.Errors.Add("Deserialized catalog is null.");
                    return result;
                }

                if (catalog.Factions == null || catalog.Factions.Count == 0)
                {
                    result.Errors.Add("Catalog contains no factions.");
                    return result;
                }

                result.Catalog = catalog;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"JSON deserialization error: {ex.Message}");
            }

            return result;
        }
    }
}
