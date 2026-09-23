// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Strict snake_case projection of one authored courtship row
    /// (<c>romance_courtship.json</c>). The data authority decides which
    /// courtship activities exist and what they are worth;
    /// <see cref="RomanceFamilySystem"/> decides how they advance a live
    /// relationship.
    /// </summary>
    public sealed class RomanceCourtshipEventData
    {
        public string event_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public int min_affinity { get; set; } = 20;
        public float base_success_rate { get; set; } = 0.70f;
        public int score_gain { get; set; } = 8;
        public float stress_reduction { get; set; } = 5.0f;
        public string description { get; set; } = string.Empty;
    }

    /// <summary>Root document of <c>romance_courtship.json</c>.</summary>
    public sealed class RomanceCourtshipCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<RomanceCourtshipEventData>? courtship_events { get; set; }
    }

    /// <summary>
    /// Result of a strict load: either validated rows or the collected errors.
    /// A row with an empty or duplicate id, a name that is empty, an affinity
    /// outside 0..100, a non-finite or out-of-range success rate, a
    /// non-positive score gain, or a negative stress reduction is rejected
    /// rather than silently coerced — the engine's defaulting path would
    /// otherwise turn an authored typo into a live courtship event.
    /// </summary>
    public sealed class RomanceCourtshipCatalogLoadResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<CourtshipEventDef> Events { get; } = new List<CourtshipEventDef>();
        public string? SourcePath { get; private set; }

        public bool Success => Errors.Count == 0;
        public bool HasErrors => Errors.Count > 0;

        internal void Set(string path) => SourcePath = path;
    }

    /// <summary>
    /// Plan 150 — strict loader for the authored courtship table. Mirrors the
    /// established catalog-loader contract: snake_case projection, collected
    /// errors, validation before any row is accepted, and a hard error on an
    /// empty table (an empty table would leave the system with no authored
    /// courtship path at all).
    /// </summary>
    public static class RomanceCourtshipCatalogLoader
    {
        public const string FileName = "romance_courtship.json";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public static RomanceCourtshipCatalogLoadResult Load(string dataDirectory, IFileIO files)
        {
            var result = new RomanceCourtshipCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDirectory) || files == null)
            {
                result.Errors.Add("RomanceCourtshipCatalogLoader: invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"RomanceCourtshipCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            RomanceCourtshipCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<RomanceCourtshipCatalogData>(
                    files.ReadAllText(path), Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"RomanceCourtshipCatalogLoader: failed to parse {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"RomanceCourtshipCatalogLoader: {FileName} produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set(path);
            return result;
        }

        /// <summary>Strict load of an already-read document (used by tests and probes).</summary>
        public static RomanceCourtshipCatalogLoadResult LoadFromJson(string json)
        {
            var result = new RomanceCourtshipCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("RomanceCourtshipCatalogLoader: empty courtship document.");
                return result;
            }

            RomanceCourtshipCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<RomanceCourtshipCatalogData>(json, Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"RomanceCourtshipCatalogLoader: failed to parse: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add("RomanceCourtshipCatalogLoader: produced no data.");
                return result;
            }

            Validate(data, result);
            return result;
        }

        private static void Validate(RomanceCourtshipCatalogData data, RomanceCourtshipCatalogLoadResult result)
        {
            if (data.schema_version != 1)
                result.Errors.Add(
                    $"RomanceCourtshipCatalogLoader: unsupported schema_version '{data.schema_version}' (expected 1).");

            if (data.courtship_events == null || data.courtship_events.Count == 0)
            {
                result.Errors.Add("RomanceCourtshipCatalogLoader: courtship_events must not be empty.");
                return;
            }

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in data.courtship_events)
            {
                if (row == null)
                {
                    result.Errors.Add("RomanceCourtshipCatalogLoader: null courtship event row.");
                    continue;
                }

                string id = (row.event_id ?? string.Empty).Trim();
                if (id.Length == 0)
                {
                    result.Errors.Add("RomanceCourtshipCatalogLoader: courtship event with an empty event_id.");
                }
                else if (!ids.Add(id))
                {
                    result.Errors.Add($"RomanceCourtshipCatalogLoader: duplicate courtship event id '{id}'.");
                }

                string name = (row.name ?? string.Empty).Trim();
                if (name.Length == 0)
                    result.Errors.Add($"RomanceCourtshipCatalogLoader: courtship event '{id}' has an empty name.");

                if (row.min_affinity < 0 || row.min_affinity > 100)
                    result.Errors.Add(
                        $"RomanceCourtshipCatalogLoader: courtship event '{id}' has min_affinity '{row.min_affinity}' (must be 0..100).");

                if (float.IsNaN(row.base_success_rate) || float.IsInfinity(row.base_success_rate))
                    result.Errors.Add(
                        $"RomanceCourtshipCatalogLoader: courtship event '{id}' has a non-finite base_success_rate.");
                else if (row.base_success_rate <= 0f || row.base_success_rate > 1f)
                    result.Errors.Add(
                        $"RomanceCourtshipCatalogLoader: courtship event '{id}' has base_success_rate '{row.base_success_rate}' (must be > 0 and <= 1).");

                if (row.score_gain < 1)
                    result.Errors.Add(
                        $"RomanceCourtshipCatalogLoader: courtship event '{id}' has score_gain '{row.score_gain}' (must be >= 1).");

                if (float.IsNaN(row.stress_reduction) || float.IsInfinity(row.stress_reduction))
                    result.Errors.Add(
                        $"RomanceCourtshipCatalogLoader: courtship event '{id}' has a non-finite stress_reduction.");
                else if (row.stress_reduction < 0f)
                    result.Errors.Add(
                        $"RomanceCourtshipCatalogLoader: courtship event '{id}' has a negative stress_reduction '{row.stress_reduction}'.");

                result.Events.Add(new CourtshipEventDef
                {
                    EventId = id,
                    Name = name,
                    MinAffinity = row.min_affinity,
                    BaseSuccessRate = row.base_success_rate,
                    ScoreGain = row.score_gain,
                    StressReduction = row.stress_reduction,
                    Description = row.description ?? string.Empty
                });
            }
        }
    }
}
