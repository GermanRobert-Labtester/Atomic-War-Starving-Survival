// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 188: Routine Template Catalog Loader
// Strict catalog loader for routine_templates.json.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Survivors
{
    public static class RoutineTemplateCatalogLoader
    {
        private static readonly HashSet<string> ValidChronotypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "early_riser",
            "night_owl",
            "intermediate"
        };

        private static readonly HashSet<string> ValidActivityTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Sleep",
            "Work",
            "Meal",
            "Social",
            "Personal",
            "Leisure"
        };

        private static readonly HashSet<string> ValidFlexibility = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "rigid",
            "flexible"
        };

        public static RoutineTemplatesCatalog LoadFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException($"Routine templates catalog not found at: {path}", path);

            string json = File.ReadAllText(path);
            return LoadFromJson(json);
        }

        public static RoutineTemplatesCatalog LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("Routine templates JSON cannot be null or empty.");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            RoutineTemplatesCatalog? catalog;
            try
            {
                catalog = JsonSerializer.Deserialize<RoutineTemplatesCatalog>(json, options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse routine templates JSON: {ex.Message}", ex);
            }

            if (catalog == null)
                throw new InvalidOperationException("Routine templates catalog deserialized to null.");

            if (catalog.schema_version != 1)
                throw new InvalidOperationException($"Unsupported schema version {catalog.schema_version}. Expected 1.");

            if (catalog.templates == null || catalog.templates.Count == 0)
                throw new InvalidOperationException("Routine templates catalog must define at least one template.");

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var t in catalog.templates)
            {
                if (string.IsNullOrWhiteSpace(t.template_id))
                    throw new InvalidOperationException("Template ID cannot be null or empty.");

                if (!seenIds.Add(t.template_id))
                    throw new InvalidOperationException($"Duplicate template ID detected: '{t.template_id}'.");

                if (string.IsNullOrWhiteSpace(t.display_name))
                    throw new InvalidOperationException($"Template '{t.template_id}' has empty display_name.");

                if (t.wake_hour < 0 || t.wake_hour > 23)
                    throw new InvalidOperationException($"Template '{t.template_id}' has invalid wake_hour ({t.wake_hour}). Expected 0..23.");

                if (t.sleep_hour < 0 || t.sleep_hour > 23)
                    throw new InvalidOperationException($"Template '{t.template_id}' has invalid sleep_hour ({t.sleep_hour}). Expected 0..23.");

                if (t.meal_hours == null || t.meal_hours.Count == 0)
                    throw new InvalidOperationException($"Template '{t.template_id}' must specify at least one meal hour.");

                foreach (int mealHour in t.meal_hours)
                {
                    if (mealHour < 0 || mealHour > 23)
                        throw new InvalidOperationException($"Template '{t.template_id}' has invalid meal hour ({mealHour}). Expected 0..23.");
                }

                if (t.work_start_hour < 0 || t.work_start_hour > 23)
                    throw new InvalidOperationException($"Template '{t.template_id}' has invalid work_start_hour ({t.work_start_hour}). Expected 0..23.");

                if (t.work_end_hour < 0 || t.work_end_hour > 23)
                    throw new InvalidOperationException($"Template '{t.template_id}' has invalid work_end_hour ({t.work_end_hour}). Expected 0..23.");

                if (string.IsNullOrWhiteSpace(t.preferred_chronotype) || !ValidChronotypes.Contains(t.preferred_chronotype))
                    throw new InvalidOperationException($"Template '{t.template_id}' has invalid preferred_chronotype '{t.preferred_chronotype}'. Expected early_riser, night_owl, or intermediate.");

                if (t.default_blocks == null || t.default_blocks.Count == 0)
                    throw new InvalidOperationException($"Template '{t.template_id}' must define at least one time block.");

                foreach (var b in t.default_blocks)
                {
                    if (string.IsNullOrWhiteSpace(b.block_id))
                        throw new InvalidOperationException($"Template '{t.template_id}' has a time block with empty block_id.");

                    if (b.start_hour < 0 || b.start_hour > 23)
                        throw new InvalidOperationException($"Template '{t.template_id}' block '{b.block_id}' has invalid start_hour ({b.start_hour}). Expected 0..23.");

                    if (b.end_hour < 0 || b.end_hour > 23)
                        throw new InvalidOperationException($"Template '{t.template_id}' block '{b.block_id}' has invalid end_hour ({b.end_hour}). Expected 0..23.");

                    if (string.IsNullOrWhiteSpace(b.activity_type) || !ValidActivityTypes.Contains(b.activity_type))
                        throw new InvalidOperationException($"Template '{t.template_id}' block '{b.block_id}' has invalid activity_type '{b.activity_type}'.");

                    if (string.IsNullOrWhiteSpace(b.flexibility) || !ValidFlexibility.Contains(b.flexibility))
                        throw new InvalidOperationException($"Template '{t.template_id}' block '{b.block_id}' has invalid flexibility '{b.flexibility}'. Expected rigid or flexible.");
                }
            }

            return catalog;
        }

        /// <summary>
        /// Loads routine_templates.json from dataDir, returning a result with Success flag and Errors list.
        /// Does not throw; caller inspects result.Success and result.Errors.
        /// </summary>
        public static RoutineTemplateCatalogLoadResult Load(string dataDir)
        {
            var result = new RoutineTemplateCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDir))
            {
                result.Errors.Add("Data directory is empty.");
                return result;
            }

            string path = Path.Combine(dataDir, "routine_templates.json");
            try
            {
                result.Catalog = LoadFromPath(path);
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }

            return result;
        }
    }

    public sealed class RoutineTemplateCatalogLoadResult
    {
        public bool Success => Errors.Count == 0;
        public RoutineTemplatesCatalog? Catalog { get; set; }
        public List<string> Errors { get; } = new List<string>();
    }
}
