// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Records
{
    /// <summary>
    /// Strict snake_case projection of one authored retention policy row.
    /// The data authority decides what a campaign is allowed to remember;
    /// <see cref="RetentionPolicyCatalog"/> decides how that bound is applied.
    /// </summary>
    public sealed class RetentionPolicyData
    {
        /// <summary>Canonical collection key (snake_case) owned by a Core authority.</summary>
        public string collection_key { get; set; } = string.Empty;

        /// <summary>Maximum retained entries. Obligation rows must be effectively unbounded.</summary>
        public int max_capacity { get; set; } = 200;

        /// <summary>
        /// <c>keep_newest_k</c> | <c>rollup_summary</c> | <c>drop_prose_keep_ids</c>.
        /// </summary>
        public string action { get; set; } = "keep_newest_k";

        /// <summary>Obligation state is iron-rule protected and can never be pruned.</summary>
        public bool protected_obligation { get; set; }
    }

    /// <summary>
    /// Root document of <c>retention_policies.json</c>.
    /// </summary>
    public sealed class RetentionPolicyCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<RetentionPolicyData>? policies { get; set; }
    }

    /// <summary>
    /// Result of a strict load: either the validated rows or the collected errors.
    /// A malformed row is never silently defaulted, so a bad policy cannot
    /// quietly widen or tighten a campaign's memory.
    /// </summary>
    public sealed class RetentionPolicyCatalogLoadResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<RetentionPolicyDefinition> Policies { get; } = new List<RetentionPolicyDefinition>();
        public string? SourcePath { get; private set; }

        public bool HasErrors => Errors.Count > 0;

        internal void Set(string path) => SourcePath = path;
    }

    /// <summary>
    /// Plan 55 / Task 55A — strict loader for the authored retention policy table.
    /// Mirrors the established catalog-loader contract: snake_case projection,
    /// collected errors, validation before any row is accepted.
    /// </summary>
    public static class RetentionPolicyCatalogLoader
    {
        public const string FileName = "retention_policies.json";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public static RetentionPolicyCatalogLoadResult Load(string dataDirectory, IFileIO files)
        {
            var result = new RetentionPolicyCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDirectory) || files == null)
            {
                result.Errors.Add("RetentionPolicyCatalogLoader: invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"RetentionPolicyCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            RetentionPolicyCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<RetentionPolicyCatalogData>(
                    files.ReadAllText(path), Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"RetentionPolicyCatalogLoader: failed to parse {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"RetentionPolicyCatalogLoader: {FileName} produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set(path);
            return result;
        }

        private static void Validate(RetentionPolicyCatalogData data, RetentionPolicyCatalogLoadResult result)
        {
            if (data.schema_version != 1)
                result.Errors.Add($"RetentionPolicyCatalogLoader: unsupported schema_version '{data.schema_version}' (expected 1).");

            if (data.policies == null || data.policies.Count == 0)
            {
                result.Errors.Add("RetentionPolicyCatalogLoader: policies must not be empty.");
                return;
            }

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in data.policies)
            {
                if (row == null)
                {
                    result.Errors.Add("RetentionPolicyCatalogLoader: null policy row.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(row.collection_key))
                {
                    result.Errors.Add("RetentionPolicyCatalogLoader: collection_key is required.");
                    continue;
                }

                // Plan 55 data discipline: keys are canonical snake_case identifiers,
                // never free prose, so a typo cannot silently create a second policy.
                if (!IsSnakeCase(row.collection_key))
                {
                    result.Errors.Add(
                        $"RetentionPolicyCatalogLoader: collection_key '{row.collection_key}' must be snake_case.");
                    continue;
                }

                if (!seen.Add(row.collection_key))
                {
                    result.Errors.Add(
                        $"RetentionPolicyCatalogLoader: duplicate collection_key '{row.collection_key}'.");
                    continue;
                }

                if (!TryParseAction(row.action, out RetentionAction action))
                {
                    result.Errors.Add(
                        $"RetentionPolicyCatalogLoader: unknown action '{row.action}' for '{row.collection_key}'.");
                    continue;
                }

                if (row.max_capacity < 1)
                {
                    result.Errors.Add(
                        $"RetentionPolicyCatalogLoader: max_capacity must be >= 1 for '{row.collection_key}'.");
                    continue;
                }

                // Iron rule enforced at the data seam: an obligation cannot be authored
                // with a finite cap, because pruning obligation state is the exact bug
                // this policy exists to prevent.
                if (row.protected_obligation && row.max_capacity != int.MaxValue)
                {
                    result.Errors.Add(
                        $"RetentionPolicyCatalogLoader: '{row.collection_key}' is a protected obligation and must use an unbounded capacity.");
                    continue;
                }

                result.Policies.Add(new RetentionPolicyDefinition(
                    row.collection_key, row.max_capacity, action, row.protected_obligation));
            }
        }

        internal static bool IsSnakeCase(string value)
        {
            foreach (char c in value)
            {
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return value.Length > 0;
        }

        internal static bool TryParseAction(string? raw, out RetentionAction action)
        {
            switch ((raw ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "keep_newest_k": action = RetentionAction.KeepNewestK; return true;
                case "rollup_summary": action = RetentionAction.RollupSummary; return true;
                case "drop_prose_keep_ids": action = RetentionAction.DropProseKeepIds; return true;
                default: action = RetentionAction.KeepNewestK; return false;
            }
        }
    }
}
