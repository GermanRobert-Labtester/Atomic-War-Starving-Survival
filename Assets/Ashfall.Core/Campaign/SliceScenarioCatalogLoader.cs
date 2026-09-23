// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Plan 54 — strict loader and structural validator for the authored
    /// seven-day slice scenario (slice_seven_days.json).
    ///
    /// The data authority is snake_case, so this loader owns the snake_case
    /// projection the way other catalog loaders own their own schema. A missing,
    /// unreadable, or incomplete scenario is an error rather than a silent
    /// default — the instrument must never invent beats to pass itself.
    /// </summary>
    public sealed class SliceScenarioLoadResult
    {
        public SliceScenarioData? Scenario { get; private set; }
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
        public string FilePath { get; private set; } = string.Empty;

        internal void Set(SliceScenarioData data, string path)
        {
            Scenario = data;
            FilePath = path;
        }
    }

    public static class SliceScenarioCatalogLoader
    {
        public const string FileName = "slice_seven_days.json";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public static SliceScenarioLoadResult Load(string dataDirectory, IFileIO files)
        {
            var result = new SliceScenarioLoadResult();
            if (string.IsNullOrWhiteSpace(dataDirectory) || files == null)
            {
                result.Errors.Add("SliceScenarioCatalogLoader: invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"SliceScenarioCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            SliceScenarioData? data;
            try
            {
                data = JsonSerializer.Deserialize<SliceScenarioData>(files.ReadAllText(path), Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"SliceScenarioCatalogLoader: failed to parse {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"SliceScenarioCatalogLoader: {FileName} produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set(data, path);
            return result;
        }

        private static void Validate(SliceScenarioData data, SliceScenarioLoadResult result)
        {
            if (string.IsNullOrWhiteSpace(data.ScenarioId))
                result.Errors.Add("SliceScenarioCatalogLoader: scenario_id is required.");
            if (data.TargetDays <= 0)
                result.Errors.Add("SliceScenarioCatalogLoader: target_days must be positive.");
            if (data.Beats == null || data.Beats.Count == 0)
            {
                result.Errors.Add("SliceScenarioCatalogLoader: beats must not be empty.");
                return;
            }

            var seenDays = new HashSet<int>();
            foreach (var beat in data.Beats)
            {
                if (beat == null)
                {
                    result.Errors.Add("SliceScenarioCatalogLoader: null beat entry.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(beat.BeatId))
                    result.Errors.Add("SliceScenarioCatalogLoader: beat_id is required.");
                if (beat.Day <= 0)
                {
                    result.Errors.Add($"SliceScenarioCatalogLoader: beat '{beat.BeatId}' has a non-positive day.");
                    continue;
                }
                if (!seenDays.Add(beat.Day))
                    result.Errors.Add($"SliceScenarioCatalogLoader: duplicate beat for day {beat.Day}.");
                if (string.IsNullOrWhiteSpace(beat.ActionKey))
                    result.Errors.Add($"SliceScenarioCatalogLoader: beat '{beat.BeatId}' has no action_key.");
                if (string.IsNullOrWhiteSpace(beat.ExpectedOutcome))
                    result.Errors.Add($"SliceScenarioCatalogLoader: beat '{beat.BeatId}' has no expected_outcome.");
            }
        }
    }
}
