// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Economy
{
    /// <summary>
    /// Strict snake_case projection of one authored rationing protocol row
    /// from <c>rationing_protocols.json</c> (Plan 215, DEC-200). The data
    /// authority decides which shelter-wide rationing policies exist; the
    /// <see cref="ResourceRationingSystem"/> owns how a policy is applied.
    /// </summary>
    public sealed class RationingProtocolRow
    {
        /// <summary>Canonical protocol id (snake_case), e.g. protocol_standard_distribution.</summary>
        public string id { get; set; } = string.Empty;

        /// <summary>Player-facing protocol name.</summary>
        public string name { get; set; } = string.Empty;

        /// <summary>Free-form policy family tag (standard / tight / emergency / triage).</summary>
        public string protocol_type { get; set; } = "standard";

        /// <summary>
        /// Default ration tier applied to targeted resources:
        /// <c>full</c> | <c>three_quarter</c> | <c>third</c> | <c>half</c> |
        /// <c>quarter</c> | <c>minimal</c> | <c>none</c>.
        /// </summary>
        public string default_tier { get; set; } = "full";

        /// <summary>Bounded non-negative daily morale weight for the policy.</summary>
        public float morale_penalty_scale { get; set; } = 0f;

        /// <summary>Restrained diegetic description of the policy.</summary>
        public string description { get; set; } = string.Empty;
    }

    /// <summary>Root document of <c>rationing_protocols.json</c>.</summary>
    public sealed class RationingProtocolDocument
    {
        public int schema_version { get; set; } = 1;
        public List<RationingProtocolRow>? protocols { get; set; }
    }

    /// <summary>
    /// Result of a strict load: either the validated rows or the collected
    /// errors. A malformed row is never silently defaulted, so a bad policy
    /// cannot quietly tighten or loosen the shelter's allocations.
    /// </summary>
    public sealed class RationingProtocolLoadResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<RationingProtocolDefinition> Protocols { get; } = new List<RationingProtocolDefinition>();
        public string? SourcePath { get; private set; }

        public bool HasErrors => Errors.Count > 0;

        internal void Set(string path) => SourcePath = path;
    }

    /// <summary>
    /// Plan 215 overlay completion — strict loader for the authored rationing
    /// protocol table. Mirrors the established catalog-loader contract
    /// (snake_case projection, collected errors, validation before any row is
    /// accepted). The host feeds the validated rows into the canonical
    /// <see cref="ResourceRationingSystem.LoadCatalog(RationingProtocolCatalogData)"/>
    /// seam; the loader never touches rationing state itself.
    /// </summary>
    public static class RationingProtocolCatalogLoader
    {
        public const string FileName = "rationing_protocols.json";

        private static readonly HashSet<string> KnownTiers = new(StringComparer.Ordinal)
        {
            "full", "three_quarter", "third", "half", "quarter", "minimal", "none"
        };

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public static RationingProtocolLoadResult Load(string dataDirectory, IFileIO files)
        {
            var result = new RationingProtocolLoadResult();
            if (string.IsNullOrWhiteSpace(dataDirectory) || files == null)
            {
                result.Errors.Add("RationingProtocolCatalogLoader: invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"RationingProtocolCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            RationingProtocolDocument? data;
            try
            {
                data = JsonSerializer.Deserialize<RationingProtocolDocument>(
                    files.ReadAllText(path), Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"RationingProtocolCatalogLoader: failed to parse {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"RationingProtocolCatalogLoader: {FileName} produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set(path);
            return result;
        }

        /// <summary>Validates an already-parsed document (unit-testable path).</summary>
        public static RationingProtocolLoadResult Validate(RationingProtocolDocument data)
        {
            var result = new RationingProtocolLoadResult();
            Validate(data, result);
            return result;
        }

        private static void Validate(RationingProtocolDocument data, RationingProtocolLoadResult result)
        {
            if (data.schema_version != 1)
                result.Errors.Add($"RationingProtocolCatalogLoader: unsupported schema_version '{data.schema_version}' (expected 1).");

            if (data.protocols == null || data.protocols.Count == 0)
            {
                result.Errors.Add("RationingProtocolCatalogLoader: protocols must not be empty.");
                return;
            }

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in data.protocols)
            {
                if (row == null)
                {
                    result.Errors.Add("RationingProtocolCatalogLoader: null protocol row.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(row.id))
                {
                    result.Errors.Add("RationingProtocolCatalogLoader: protocol row with empty id.");
                    continue;
                }

                if (!seen.Add(row.id.Trim()))
                {
                    result.Errors.Add($"RationingProtocolCatalogLoader: duplicate protocol id '{row.id}'.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(row.name))
                    result.Errors.Add($"RationingProtocolCatalogLoader: protocol '{row.id}' has an empty name.");

                if (!KnownTiers.Contains((row.default_tier ?? string.Empty).Trim().ToLowerInvariant()))
                {
                    result.Errors.Add(
                        $"RationingProtocolCatalogLoader: protocol '{row.id}' has unknown default_tier '{row.default_tier}'.");
                }

                if (float.IsNaN(row.morale_penalty_scale) || float.IsInfinity(row.morale_penalty_scale)
                    || row.morale_penalty_scale < 0f)
                {
                    result.Errors.Add(
                        $"RationingProtocolCatalogLoader: protocol '{row.id}' has non-finite or negative morale_penalty_scale.");
                }

                // Rows are accumulated independently so one bad row does not
                // hide the diagnostics of the remaining rows; a single error
                // clears the accepted set at the end.
                result.Protocols.Add(new RationingProtocolDefinition
                {
                    Id = row.id.Trim(),
                    Name = row.name ?? string.Empty,
                    ProtocolType = string.IsNullOrWhiteSpace(row.protocol_type) ? "standard" : row.protocol_type.Trim(),
                    DefaultTier = string.IsNullOrWhiteSpace(row.default_tier) ? "full" : row.default_tier.Trim(),
                    MoralePenaltyScale = row.morale_penalty_scale,
                    Description = row.description ?? string.Empty
                });
            }

            if (result.Errors.Count > 0) result.Protocols.Clear();
        }
    }
}
