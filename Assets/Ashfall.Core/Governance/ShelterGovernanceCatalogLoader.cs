// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.IO;

namespace Ashfall.Core.Governance
{
    public sealed class ShelterGovernanceCatalogLoadResult
    {
        public bool Success => Errors.Count == 0;
        public List<IdeologicalBlocDef> Blocs { get; set; } = new();
        public List<string> Errors { get; } = new();
    }

    /// <summary>
    /// Strict catalog loader for shelter_governance_blocs.json (Plan 159).
    /// </summary>
    public static class ShelterGovernanceCatalogLoader
    {
        public const string DefaultFileName = "shelter_governance_blocs.json";

        public static ShelterGovernanceCatalogLoadResult Load(string dataDir, IFileIO? fileIO = null)
        {
            fileIO ??= new FileSystemIO();
            var result = new ShelterGovernanceCatalogLoadResult();

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

        public static ShelterGovernanceCatalogLoadResult LoadFromJson(string json)
        {
            var result = new ShelterGovernanceCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("JSON content is empty.");
                return result;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };
                var catalog = JsonSerializer.Deserialize<ShelterGovernanceBlocsCatalogJson>(json, options);
                if (catalog == null)
                {
                    result.Errors.Add("Deserialized catalog is null.");
                    return result;
                }

                if (catalog.schema_version < 1)
                {
                    result.Errors.Add($"Invalid schema_version: {catalog.schema_version}. Must be >= 1.");
                }

                if (catalog.blocs == null || catalog.blocs.Count == 0)
                {
                    result.Errors.Add("Catalog contains no ideological blocs.");
                    return result;
                }

                var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < catalog.blocs.Count; i++)
                {
                    var b = catalog.blocs[i];
                    if (b == null)
                    {
                        result.Errors.Add($"Bloc at index {i} is null.");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(b.bloc_id))
                    {
                        result.Errors.Add($"Bloc at index {i} has empty bloc_id.");
                    }
                    else if (!seenIds.Add(b.bloc_id))
                    {
                        result.Errors.Add($"Duplicate bloc_id '{b.bloc_id}' at index {i}.");
                    }

                    if (string.IsNullOrWhiteSpace(b.display_name))
                    {
                        result.Errors.Add($"Bloc '{b.bloc_id}' has empty display_name.");
                    }

                    if (string.IsNullOrWhiteSpace(b.core_ideology))
                    {
                        result.Errors.Add($"Bloc '{b.bloc_id}' has empty core_ideology.");
                    }

                    if (b.baseline_weight <= 0)
                    {
                        result.Errors.Add($"Bloc '{b.bloc_id}' has invalid baseline_weight {b.baseline_weight}; must be > 0.");
                    }

                    if (b.grievance_decay_rate <= 0)
                    {
                        result.Errors.Add($"Bloc '{b.bloc_id}' has invalid grievance_decay_rate {b.grievance_decay_rate}; must be > 0.");
                    }
                }

                if (result.Success)
                {
                    result.Blocs = catalog.blocs;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"JSON deserialization error: {ex.Message}");
            }

            return result;
        }
    }
}
