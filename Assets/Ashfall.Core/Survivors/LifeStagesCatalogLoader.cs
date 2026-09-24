// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    public sealed class LifeStagesCatalogLoadResult
    {
        public bool Success => Errors.Count == 0;
        public LifeStagesCatalog? Catalog { get; set; }
        public List<string> Errors { get; } = new();
    }

    /// <summary>
    /// Strict catalog loader for life_stages.json (Plan 176).
    /// Enforces schema validity, stage uniqueness, valid SurvivorLifeStage enum mappings,
    /// non-negative age spans, and positive throughput multipliers.
    /// </summary>
    public static class LifeStagesCatalogLoader
    {
        public const string DefaultFileName = "life_stages.json";

        public static LifeStagesCatalogLoadResult Load(string dataDir, IFileIO? fileIO = null)
        {
            fileIO ??= new FileSystemIO();
            var result = new LifeStagesCatalogLoadResult();

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

        public static LifeStagesCatalogLoadResult LoadFromJson(string json)
        {
            var result = new LifeStagesCatalogLoadResult();
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
                var catalog = JsonSerializer.Deserialize<LifeStagesCatalog>(json, options);
                if (catalog == null)
                {
                    result.Errors.Add("Deserialized catalog is null.");
                    return result;
                }

                if (catalog.schema_version < 1)
                {
                    result.Errors.Add($"Invalid schema_version: {catalog.schema_version}. Must be >= 1.");
                }

                if (catalog.days_per_year <= 0)
                {
                    result.Errors.Add($"Invalid days_per_year: {catalog.days_per_year}. Must be > 0.");
                }

                if (catalog.min_retirement_age_years <= 0)
                {
                    result.Errors.Add($"Invalid min_retirement_age_years: {catalog.min_retirement_age_years}. Must be > 0.");
                }

                if (catalog.life_stages == null || catalog.life_stages.Count == 0)
                {
                    result.Errors.Add("Catalog contains no life stages.");
                    return result;
                }

                var seenStageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var seenEnums = new HashSet<SurvivorLifeStage>();

                for (int i = 0; i < catalog.life_stages.Count; i++)
                {
                    var s = catalog.life_stages[i];
                    if (s == null)
                    {
                        result.Errors.Add($"Life stage at index {i} is null.");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(s.stage_id))
                    {
                        result.Errors.Add($"Life stage at index {i} has empty stage_id.");
                    }
                    else if (!seenStageIds.Add(s.stage_id))
                    {
                        result.Errors.Add($"Duplicate stage_id '{s.stage_id}' at index {i}.");
                    }

                    if (string.IsNullOrWhiteSpace(s.stage_enum))
                    {
                        result.Errors.Add($"Life stage '{s.stage_id}' has empty stage_enum.");
                    }
                    else if (!Enum.TryParse<SurvivorLifeStage>(s.stage_enum, ignoreCase: true, out var parsedEnum))
                    {
                        result.Errors.Add($"Life stage '{s.stage_id}' has unrecognized stage_enum '{s.stage_enum}'.");
                    }
                    else if (!seenEnums.Add(parsedEnum))
                    {
                        result.Errors.Add($"Duplicate stage_enum mapping '{parsedEnum}' at stage '{s.stage_id}'.");
                    }

                    if (string.IsNullOrWhiteSpace(s.display_name))
                    {
                        result.Errors.Add($"Life stage '{s.stage_id}' has empty display_name.");
                    }

                    if (s.min_age < 0)
                    {
                        result.Errors.Add($"Life stage '{s.stage_id}' has negative min_age {s.min_age}.");
                    }

                    if (s.max_age < s.min_age)
                    {
                        result.Errors.Add($"Life stage '{s.stage_id}' has max_age ({s.max_age}) less than min_age ({s.min_age}).");
                    }

                    if (s.physical_labor_multiplier <= 0f)
                    {
                        result.Errors.Add($"Life stage '{s.stage_id}' has invalid physical_labor_multiplier {s.physical_labor_multiplier}; must be > 0.");
                    }

                    if (s.fatigue_accumulation_multiplier <= 0f)
                    {
                        result.Errors.Add($"Life stage '{s.stage_id}' has invalid fatigue_accumulation_multiplier {s.fatigue_accumulation_multiplier}; must be > 0.");
                    }
                }

                if (catalog.milestones != null)
                {
                    var seenMilestoneAges = new HashSet<int>();
                    for (int i = 0; i < catalog.milestones.Count; i++)
                    {
                        var m = catalog.milestones[i];
                        if (m == null)
                        {
                            result.Errors.Add($"Milestone at index {i} is null.");
                            continue;
                        }

                        if (m.age <= 0)
                        {
                            result.Errors.Add($"Milestone at index {i} has invalid age {m.age}; must be > 0.");
                        }
                        else if (!seenMilestoneAges.Add(m.age))
                        {
                            result.Errors.Add($"Duplicate milestone age {m.age} at index {i}.");
                        }

                        if (string.IsNullOrWhiteSpace(m.label))
                        {
                            result.Errors.Add($"Milestone for age {m.age} has empty label.");
                        }

                        if (m.morale_bonus < 0)
                        {
                            result.Errors.Add($"Milestone for age {m.age} has negative morale_bonus {m.morale_bonus}.");
                        }
                    }
                }

                if (result.Success)
                {
                    result.Catalog = catalog;
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
