// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;

namespace Ashfall.Core.Vehicles
{
    /// <summary>
    /// Strict snake_case projection of one authored vehicle module row
    /// (<c>vehicle_modules.json</c>). The data authority decides which modules
    /// exist and their costs/effects; <see cref="VehicleCustomizationSystem"/>
    /// decides how an installed module changes a live vehicle.
    /// </summary>
    public sealed class VehicleModuleData
    {
        public string module_id { get; set; } = string.Empty;
        public string module_type { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public float defense_bonus { get; set; }
        public float speed_modifier { get; set; }
        public float cargo_bonus { get; set; }
        public int bunk_capacity { get; set; }
        public int installation_days { get; set; } = 1;
        public int scrap_cost { get; set; } = 20;
        public int components_cost { get; set; } = 10;
    }

    /// <summary>Root document of <c>vehicle_modules.json</c>.</summary>
    public sealed class VehicleCustomizationCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<VehicleModuleData>? modules { get; set; }
    }

    /// <summary>
    /// Result of a strict load: either validated modules or the collected
    /// errors. A row with an empty or duplicate id, an unknown module type, an
    /// empty name, a non-finite effect, a speed modifier that would drive the
    /// vehicle below its floor, a cargo penalty larger than the base hold, a
    /// negative bunk capacity, or a sub-day installation is rejected rather
    /// than silently coerced — the lenient path would otherwise hide an
    /// authored typo inside a live vehicle build.
    /// </summary>
    public sealed class VehicleModuleCatalogLoadResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<VehicleModule> Modules { get; } = new List<VehicleModule>();
        public string? SourcePath { get; private set; }

        public bool Success => Errors.Count == 0;
        public bool HasErrors => Errors.Count > 0;

        internal void Set(string path) => SourcePath = path;
    }

    /// <summary>
    /// Plan 152 — strict loader for the authored vehicle module table. Mirrors
    /// the established catalog-loader contract: snake_case projection,
    /// collected errors, validation before any row is accepted, and a hard
    /// error on an empty table (an empty table would leave every vehicle
    /// uncustomizable).
    /// </summary>
    public static class VehicleModuleCatalogLoader
    {
        public const string FileName = "vehicle_modules.json";

        /// <summary>Module-type vocabulary the Core system understands.</summary>
        public static readonly IReadOnlyList<string> KnownModuleTypes = new[]
        {
            "armor", "cargo", "living", "weapon", "utility"
        };

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public static VehicleModuleCatalogLoadResult Load(string dataDirectory, IFileIO files)
        {
            var result = new VehicleModuleCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDirectory) || files == null)
            {
                result.Errors.Add("VehicleModuleCatalogLoader: invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"VehicleModuleCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            VehicleCustomizationCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<VehicleCustomizationCatalogData>(
                    files.ReadAllText(path), Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"VehicleModuleCatalogLoader: failed to parse {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"VehicleModuleCatalogLoader: {FileName} produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set(path);
            return result;
        }

        /// <summary>Strict load of an already-read document (used by tests and probes).</summary>
        public static VehicleModuleCatalogLoadResult LoadFromJson(string json)
        {
            var result = new VehicleModuleCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("VehicleModuleCatalogLoader: empty module document.");
                return result;
            }

            VehicleCustomizationCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<VehicleCustomizationCatalogData>(json, Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"VehicleModuleCatalogLoader: failed to parse: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add("VehicleModuleCatalogLoader: produced no data.");
                return result;
            }

            Validate(data, result);
            return result;
        }

        private static void Validate(VehicleCustomizationCatalogData data, VehicleModuleCatalogLoadResult result)
        {
            if (data.schema_version != 1)
                result.Errors.Add(
                    $"VehicleModuleCatalogLoader: unsupported schema_version '{data.schema_version}' (expected 1).");

            if (data.modules == null || data.modules.Count == 0)
            {
                result.Errors.Add("VehicleModuleCatalogLoader: modules must not be empty.");
                return;
            }

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in data.modules)
            {
                if (row == null)
                {
                    result.Errors.Add("VehicleModuleCatalogLoader: null vehicle module row.");
                    continue;
                }

                string id = (row.module_id ?? string.Empty).Trim();
                if (id.Length == 0)
                {
                    result.Errors.Add("VehicleModuleCatalogLoader: vehicle module with an empty module_id.");
                }
                else if (!ids.Add(id))
                {
                    result.Errors.Add($"VehicleModuleCatalogLoader: duplicate vehicle module id '{id}'.");
                }

                string type = (row.module_type ?? string.Empty).Trim();
                if (!ContainsIgnoreCase(KnownModuleTypes, type))
                {
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' names unknown module_type '{type}'. "
                        + "Known types: " + string.Join(", ", KnownModuleTypes) + ".");
                }

                string name = (row.name ?? string.Empty).Trim();
                if (name.Length == 0)
                    result.Errors.Add($"VehicleModuleCatalogLoader: vehicle module '{id}' has an empty name.");

                if (!IsFinite(row.defense_bonus))
                    result.Errors.Add($"VehicleModuleCatalogLoader: vehicle module '{id}' has a non-finite defense_bonus.");
                else if (row.defense_bonus < 0f)
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' has a negative defense_bonus '{row.defense_bonus}'.");

                if (!IsFinite(row.speed_modifier))
                    result.Errors.Add($"VehicleModuleCatalogLoader: vehicle module '{id}' has a non-finite speed_modifier.");
                else if (row.speed_modifier < -0.9f)
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' has speed_modifier '{row.speed_modifier}' "
                        + "(must be >= -0.9; CalculateEffectiveStats floors speed at 0.20).");

                if (!IsFinite(row.cargo_bonus))
                    result.Errors.Add($"VehicleModuleCatalogLoader: vehicle module '{id}' has a non-finite cargo_bonus.");
                else if (row.cargo_bonus < -100f)
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' has cargo_bonus '{row.cargo_bonus}' "
                        + "(must be >= -100; the default base hold is 100).");

                if (row.bunk_capacity < 0)
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' has a negative bunk_capacity '{row.bunk_capacity}'.");

                if (row.installation_days < 1)
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' has installation_days '{row.installation_days}' (must be >= 1).");

                if (row.scrap_cost < 0)
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' has a negative scrap_cost '{row.scrap_cost}'.");

                if (row.components_cost < 0)
                    result.Errors.Add(
                        $"VehicleModuleCatalogLoader: vehicle module '{id}' has a negative components_cost '{row.components_cost}'.");

                result.Modules.Add(new VehicleModule
                {
                    ModuleId = id,
                    ModuleType = type,
                    Name = name,
                    Description = row.description ?? string.Empty,
                    DefenseBonus = row.defense_bonus,
                    SpeedModifier = row.speed_modifier,
                    CargoBonus = row.cargo_bonus,
                    BunkCapacity = row.bunk_capacity,
                    InstallationDays = row.installation_days,
                    ScrapCost = row.scrap_cost,
                    ComponentsCost = row.components_cost
                });
            }
        }

        private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

        private static bool ContainsIgnoreCase(IReadOnlyList<string> known, string value)
        {
            for (int i = 0; i < known.Count; i++)
                if (string.Equals(known[i], value, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}
