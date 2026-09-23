// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.Weather;

namespace Ashfall.Core.Records
{
    /// <summary>
    /// Strict snake_case projection of one authored weather-cascade template row
    /// (<c>weather_gameplay_effects.json</c>). The data authority decides
    /// which canonical systems a given weather kind pressures and by how much;
    /// <see cref="WeatherGameplayCascadeEngine"/> decides how that pressure is
    /// applied to live owners.
    /// </summary>
    public sealed class WeatherCascadeTemplateData
    {
        public string id { get; set; } = string.Empty;
        public string weather_kind { get; set; } = string.Empty;
        public string target_system { get; set; } = string.Empty;
        public string effect_type { get; set; } = string.Empty;
        public float magnitude { get; set; }
        public int duration_days { get; set; } = 1;
        public string description { get; set; } = string.Empty;
    }

    /// <summary>Root document of <c>weather_gameplay_effects.json</c>.</summary>
    public sealed class WeatherCascadeCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<WeatherCascadeTemplateData>? cascade_templates { get; set; }
    }

    /// <summary>
    /// Result of a strict load: either validated rows or the collected errors.
    /// A row that names a weather kind that is not a <see cref="WeatherKind"/>,
    /// a target system or effect type outside the Core vocabulary, a non-finite
    /// magnitude, a sub-day duration, or a duplicate id is rejected rather than
    /// silently coerced — the engine's lenient <c>ParseTarget</c>/<c>ParseEffectType</c>
    /// fallbacks would otherwise hide an authored typo inside a live cascade.
    /// </summary>
    public sealed class WeatherCascadeCatalogLoadResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<WeatherEffectDef> Templates { get; } = new List<WeatherEffectDef>();
        public string? SourcePath { get; private set; }

        public bool HasErrors => Errors.Count > 0;

        internal void Set(string path) => SourcePath = path;
    }

    /// <summary>
    /// Plan 135 / C2[27] — strict loader for the authored weather-cascade
    /// template table. Mirrors the established catalog-loader contract:
    /// snake_case projection, collected errors, validation before any row is
    /// accepted, and a hard error on an empty table (an empty table would make
    /// the engine fall back to generated effects, i.e. invent the cascade).
    /// </summary>
    public static class WeatherCascadeCatalogLoader
    {
        public const string FileName = WeatherGameplayCascadeEngine.DefaultCatalogFileName;

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        /// <summary>Target-system vocabulary the Core engine understands.</summary>
        public static readonly IReadOnlyList<string> KnownTargetSystems = new[]
        {
            "shelter", "expedition", "faction", "economy", "mental_health", "location"
        };

        /// <summary>Effect-type vocabulary the Core engine understands.</summary>
        public static readonly IReadOnlyList<string> KnownEffectTypes = new[]
        {
            "damage", "delay", "price_change", "behavior_change", "accessibility",
            "thermal_load", "filtration_stress", "power_output", "hazard_surge"
        };

        public static WeatherCascadeCatalogLoadResult Load(string dataDirectory, IFileIO files)
        {
            var result = new WeatherCascadeCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDirectory) || files == null)
            {
                result.Errors.Add("WeatherCascadeCatalogLoader: invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"WeatherCascadeCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            WeatherCascadeCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<WeatherCascadeCatalogData>(
                    files.ReadAllText(path), Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"WeatherCascadeCatalogLoader: failed to parse {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"WeatherCascadeCatalogLoader: {FileName} produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set(path);
            return result;
        }

        /// <summary>Strict load of an already-read document (used by tests and probes).</summary>
        public static WeatherCascadeCatalogLoadResult LoadFromJson(string json)
        {
            var result = new WeatherCascadeCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("WeatherCascadeCatalogLoader: empty cascade document.");
                return result;
            }

            WeatherCascadeCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<WeatherCascadeCatalogData>(json, Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"WeatherCascadeCatalogLoader: failed to parse: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add("WeatherCascadeCatalogLoader: produced no data.");
                return result;
            }

            Validate(data, result);
            return result;
        }

        private static void Validate(WeatherCascadeCatalogData data, WeatherCascadeCatalogLoadResult result)
        {
            if (data.schema_version != 1)
                result.Errors.Add(
                    $"WeatherCascadeCatalogLoader: unsupported schema_version '{data.schema_version}' (expected 1).");

            if (data.cascade_templates == null || data.cascade_templates.Count == 0)
            {
                // An empty table is a hard error, not a silent "use generated
                // effects": the plan's contract is an authored cascade.
                result.Errors.Add("WeatherCascadeCatalogLoader: cascade_templates must not be empty.");
                return;
            }

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in data.cascade_templates)
            {
                if (row == null)
                {
                    result.Errors.Add("WeatherCascadeCatalogLoader: null cascade template row.");
                    continue;
                }

                string id = (row.id ?? string.Empty).Trim();
                if (id.Length == 0)
                {
                    result.Errors.Add("WeatherCascadeCatalogLoader: cascade template with an empty id.");
                }
                else if (!ids.Add(id))
                {
                    result.Errors.Add($"WeatherCascadeCatalogLoader: duplicate cascade template id '{id}'.");
                }

                string kind = (row.weather_kind ?? string.Empty).Trim();
                if (kind.Length == 0)
                {
                    result.Errors.Add(
                        $"WeatherCascadeCatalogLoader: cascade template '{id}' has an empty weather_kind.");
                }
                else if (!TryResolveKind(kind, out WeatherKind resolved))
                {
                    result.Errors.Add(
                        $"WeatherCascadeCatalogLoader: cascade template '{id}' names unknown weather_kind '{kind}'.");
                }

                string target = (row.target_system ?? string.Empty).Trim();
                if (!ContainsIgnoreCase(KnownTargetSystems, target))
                {
                    result.Errors.Add(
                        $"WeatherCascadeCatalogLoader: cascade template '{id}' names unknown target_system '{target}'. "
                        + "Known targets: " + string.Join(", ", KnownTargetSystems) + ".");
                }

                string effect = (row.effect_type ?? string.Empty).Trim();
                if (!ContainsIgnoreCase(KnownEffectTypes, effect))
                {
                    result.Errors.Add(
                        $"WeatherCascadeCatalogLoader: cascade template '{id}' names unknown effect_type '{effect}'. "
                        + "Known effect types: " + string.Join(", ", KnownEffectTypes) + ".");
                }

                if (float.IsNaN(row.magnitude) || float.IsInfinity(row.magnitude))
                {
                    result.Errors.Add(
                        $"WeatherCascadeCatalogLoader: cascade template '{id}' has a non-finite magnitude '{row.magnitude}'.");
                }

                if (row.duration_days < 1)
                {
                    result.Errors.Add(
                        $"WeatherCascadeCatalogLoader: cascade template '{id}' has duration_days '{row.duration_days}' (must be >= 1).");
                }

                result.Templates.Add(new WeatherEffectDef
                {
                    id = id,
                    weather_kind = kind,
                    target_system = target,
                    effect_type = effect,
                    magnitude = row.magnitude,
                    duration_days = Math.Max(1, row.duration_days),
                    description = row.description ?? string.Empty
                });
            }
        }

        internal static bool TryResolveKind(string kind, out WeatherKind resolved)
        {
            foreach (WeatherKind candidate in Enum.GetValues(typeof(WeatherKind)))
            {
                if (string.Equals(candidate.ToString(), kind, StringComparison.OrdinalIgnoreCase))
                {
                    resolved = candidate;
                    return true;
                }
            }
            resolved = default;
            return false;
        }

        private static bool ContainsIgnoreCase(IReadOnlyList<string> known, string value)
        {
            for (int i = 0; i < known.Count; i++)
                if (string.Equals(known[i], value, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}
