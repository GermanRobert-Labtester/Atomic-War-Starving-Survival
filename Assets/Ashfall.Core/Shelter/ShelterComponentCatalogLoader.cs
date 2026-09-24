// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 186: Shelter Component Catalog Loader
// Strict catalog loader for shelter_components.json.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Shelter
{
    public static class ShelterComponentCatalogLoader
    {
        private static readonly HashSet<string> ValidComponentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Air",
            "Water",
            "Power",
            "Structural"
        };

        public static ShelterComponentsCatalog LoadFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException($"Shelter components catalog not found at: {path}", path);

            string json = File.ReadAllText(path);
            return LoadFromJson(json);
        }

        public static ShelterComponentsCatalog LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("Shelter components JSON cannot be null or empty.");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            ShelterComponentsCatalog? catalog;
            try
            {
                catalog = JsonSerializer.Deserialize<ShelterComponentsCatalog>(json, options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse shelter components JSON: {ex.Message}", ex);
            }

            if (catalog == null)
                throw new InvalidOperationException("Shelter components catalog deserialized to null.");

            if (catalog.schema_version != 1)
                throw new InvalidOperationException($"Unsupported schema version {catalog.schema_version}. Expected 1.");

            if (catalog.components == null || catalog.components.Count == 0)
                throw new InvalidOperationException("Shelter components catalog must define at least one component.");

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var comp in catalog.components)
            {
                if (string.IsNullOrWhiteSpace(comp.component_id))
                    throw new InvalidOperationException("Component ID cannot be null or empty.");

                if (!seenIds.Add(comp.component_id))
                    throw new InvalidOperationException($"Duplicate component ID detected: '{comp.component_id}'.");

                if (string.IsNullOrWhiteSpace(comp.display_name))
                    throw new InvalidOperationException($"Component '{comp.component_id}' has empty display_name.");

                if (string.IsNullOrWhiteSpace(comp.component_type) || !ValidComponentTypes.Contains(comp.component_type))
                    throw new InvalidOperationException($"Component '{comp.component_id}' has invalid component_type '{comp.component_type}'. Expected Air, Water, Power, or Structural.");

                if (comp.max_condition <= 0.0f)
                    throw new InvalidOperationException($"Component '{comp.component_id}' has non-positive max_condition ({comp.max_condition}).");

                if (comp.base_degradation_rate <= 0.0f)
                    throw new InvalidOperationException($"Component '{comp.component_id}' has non-positive base_degradation_rate ({comp.base_degradation_rate}).");

                if (comp.failure_threshold < 0.0f)
                    throw new InvalidOperationException($"Component '{comp.component_id}' has negative failure_threshold ({comp.failure_threshold}).");

                if (comp.warning_threshold <= comp.failure_threshold)
                    throw new InvalidOperationException($"Component '{comp.component_id}' has warning_threshold ({comp.warning_threshold}) <= failure_threshold ({comp.failure_threshold}).");

                if (comp.warning_threshold > comp.max_condition)
                    throw new InvalidOperationException($"Component '{comp.component_id}' has warning_threshold ({comp.warning_threshold}) > max_condition ({comp.max_condition}).");

                if (comp.repair_time_hours < 0)
                    throw new InvalidOperationException($"Component '{comp.component_id}' has negative repair_time_hours ({comp.repair_time_hours}).");
            }

            return catalog;
        }

        /// <summary>
        /// Loads shelter_components.json from dataDir, returning a result with Success flag and Errors list.
        /// Does not throw; caller inspects result.Success and result.Errors.
        /// </summary>
        public static ShelterComponentCatalogLoadResult Load(string dataDir)
        {
            var result = new ShelterComponentCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDir))
            {
                result.Errors.Add("Data directory is empty.");
                return result;
            }

            string path = Path.Combine(dataDir, "shelter_components.json");
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

    public sealed class ShelterComponentCatalogLoadResult
    {
        public bool Success => Errors.Count == 0;
        public ShelterComponentsCatalog? Catalog { get; set; }
        public List<string> Errors { get; } = new List<string>();
    }
}
