// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Commitments
{
    public sealed class CommitmentLoadResult
    {
        public List<CommitmentDefinition> Commitments { get; } = new List<CommitmentDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>
    /// ASHFALL — Strict loader and structural validator for commitments.json (Plan 38 §38C.3).
    /// </summary>
    public static class CommitmentCatalogLoader
    {
        public const string FileName = "commitments.json";

        public static CommitmentLoadResult Load(string dataDirectory, IFileIO files, IJsonSerializer serializer)
        {
            var result = new CommitmentLoadResult();
            if (string.IsNullOrEmpty(dataDirectory) || files == null || serializer == null)
            {
                result.Errors.Add("CommitmentCatalogLoader: Invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"CommitmentCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            string json;
            try
            {
                json = files.ReadAllText(path);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"CommitmentCatalogLoader: Failed to read {FileName}: {ex.Message}");
                return result;
            }

            CommitmentCatalogData? data;
            try
            {
                data = serializer.Deserialize<CommitmentCatalogData>(json);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"CommitmentCatalogLoader: JSON deserialize failure in {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"CommitmentCatalogLoader: Root object is null in {FileName}.");
                return result;
            }

            if (data.schema_version < 1)
            {
                result.Errors.Add($"CommitmentCatalogLoader: {FileName} schema_version must be >= 1, got {data.schema_version}.");
            }

            if (data.commitments == null || data.commitments.Count == 0)
            {
                result.Errors.Add($"CommitmentCatalogLoader: {FileName} has no commitment definitions.");
                return result;
            }

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < data.commitments.Count; i++)
            {
                var def = data.commitments[i];
                if (def == null)
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment[{i}] is null.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(def.id))
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment[{i}] has empty id.");
                    continue;
                }

                if (!def.id.StartsWith("commitment_", StringComparison.Ordinal))
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment[{i}] id '{def.id}' must start with 'commitment_'.");
                }

                if (!seenIds.Add(def.id))
                {
                    result.Errors.Add($"CommitmentCatalogLoader: Duplicate commitment id '{def.id}'.");
                }

                if (def.start_day < 1)
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment '{def.id}' start_day {def.start_day} must be >= 1.");
                }

                if (def.due_day < def.start_day)
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment '{def.id}' due_day {def.due_day} is earlier than start_day {def.start_day}.");
                }

                if (def.warning_lead_days < 0)
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment '{def.id}' warning_lead_days {def.warning_lead_days} must be >= 0.");
                }

                if (def.due_day - def.warning_lead_days < 1)
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment '{def.id}' warning lead underflows before campaign start (due {def.due_day} - lead {def.warning_lead_days} < 1).");
                }

                if (def.target_quantity < 1)
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment '{def.id}' target_quantity {def.target_quantity} must be >= 1.");
                }

                if (string.IsNullOrWhiteSpace(def.consequence_class))
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment '{def.id}' consequence_class is empty.");
                }

                if (string.IsNullOrWhiteSpace(def.consequence_target))
                {
                    result.Errors.Add($"CommitmentCatalogLoader: commitment '{def.id}' consequence_target is empty.");
                }

                result.Commitments.Add(def);
            }

            return result;
        }
    }
}
