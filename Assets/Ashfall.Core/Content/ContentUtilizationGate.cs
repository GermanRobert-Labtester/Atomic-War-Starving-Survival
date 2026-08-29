// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Utilization CI Gate
//
// CI enforcement for content utilization regressions.
// Prevents new required content from becoming silently orphaned.
// Compares identities, not merely aggregate counts.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ashfall.Core.Content
{
    [Serializable]
    public sealed class UtilizationBaseline
    {
        public string SchemaVersion { get; set; } = "1.0.0";
        public string GeneratedFromCommit { get; set; } = string.Empty;
        public Dictionary<string, string> CatalogClassifications { get; set; } = new Dictionary<string, string>();
        public List<string> KnownOrphans { get; set; } = new List<string>();
        public List<string> ExemptedCatalogs { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class UtilizationGateResult
    {
        public bool Passed { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Regressions { get; set; } = new List<string>();
        public List<string> NewOrphans { get; set; } = new List<string>();
        public List<string> Improvements { get; set; } = new List<string>();
    }

    /// <summary>
    /// CI gate that compares current utilization state against a baseline.
    /// Detects regressions, new orphans, and improvements.
    /// </summary>
    public static class ContentUtilizationGate
    {
        public const string BaselinePath = "artifacts/content-utilization-baseline.json";

        /// <summary>
        /// Run the CI gate against a current graph and optional baseline.
        /// </summary>
        public static UtilizationGateResult Run(ContentUtilizationGraph current, UtilizationBaseline? baseline = null)
        {
            var result = new UtilizationGateResult();

            // If no baseline exists, create one and pass (first run)
            if (baseline == null)
            {
                result.Warnings.Add("No baseline found — this is the first run. Creating baseline.");
                result.Passed = true;
                return result;
            }

            // Check for regressions: previously GAMEPLAY_CONSUMED catalogs that are now worse
            foreach (var (path, prevClass) in baseline.CatalogClassifications)
            {
                var currentCat = current.Catalogs.FirstOrDefault(c => c.Path == path);
                if (currentCat == null)
                {
                    // Catalog was removed — check if it was consumed
                    if (prevClass == ContentClassification.GAMEPLAY_CONSUMED.ToString())
                    {
                        result.Warnings.Add($"Previously consumed catalog removed: {path}");
                    }
                    continue;
                }

                string currentClass = currentCat.Classification.ToString();

                // Regression: GAMEPLAY_CONSUMED → anything worse
                if (prevClass == ContentClassification.GAMEPLAY_CONSUMED.ToString()
                    && currentClass != ContentClassification.GAMEPLAY_CONSUMED.ToString()
                    && currentClass != ContentClassification.UI_ONLY.ToString()
                    && currentClass != ContentClassification.CODEX_ONLY.ToString())
                {
                    result.Regressions.Add($"REGRESSION: {path} was {prevClass}, now {currentClass}");
                    result.Errors.Add($"Regression: {path} {prevClass} → {currentClass}");
                }

                // Regression: GAMEPLAY_CONSUMED → ORPHANED
                if (prevClass == ContentClassification.GAMEPLAY_CONSUMED.ToString()
                    && currentClass == ContentClassification.ORPHANED.ToString())
                {
                    result.Errors.Add($"CRITICAL REGRESSION: {path} was GAMEPLAY_CONSUMED, now ORPHANED");
                }

                // Improvement: previously orphaned → now consumed
                if (prevClass == ContentClassification.ORPHANED.ToString()
                    && currentClass == ContentClassification.GAMEPLAY_CONSUMED.ToString())
                {
                    result.Improvements.Add($"Fixed: {path} was ORPHANED, now GAMEPLAY_CONSUMED");
                }
            }

            // Check for new orphans (not in baseline)
            foreach (var cat in current.Catalogs)
            {
                if (cat.Classification == ContentClassification.ORPHANED
                    && string.IsNullOrEmpty(cat.ExemptionId))
                {
                    bool wasKnown = baseline.CatalogClassifications.ContainsKey(cat.Path)
                        && baseline.CatalogClassifications[cat.Path] == ContentClassification.ORPHANED.ToString();

                    if (!wasKnown && !baseline.KnownOrphans.Contains(cat.Path))
                    {
                        result.NewOrphans.Add($"NEW ORPHAN: {cat.Path} — {string.Join("; ", cat.Findings)}");
                        result.Errors.Add($"New orphan: {cat.Path}");
                    }
                }
            }

            // Check for catalog with no consumer but has loader
            foreach (var cat in current.Catalogs)
            {
                if (!string.IsNullOrEmpty(cat.Loader)
                    && cat.ConsumerSystems.Count == 0
                    && cat.Classification != ContentClassification.CODEX_ONLY
                    && cat.Classification != ContentClassification.OPTIONAL
                    && string.IsNullOrEmpty(cat.ExemptionId))
                {
                    bool wasKnown = baseline.CatalogClassifications.ContainsKey(cat.Path);
                    if (!wasKnown)
                    {
                        result.Warnings.Add($"LOADER WITHOUT CONSUMER: {cat.Path} — loader '{cat.Loader}' but no consumer");
                    }
                }
            }

            result.Passed = result.Errors.Count == 0;
            return result;
        }

        /// <summary>
        /// Create a baseline from the current graph state.
        /// </summary>
        public static UtilizationBaseline CreateBaseline(ContentUtilizationGraph graph, string commitHash = "")
        {
            var baseline = new UtilizationBaseline
            {
                GeneratedFromCommit = commitHash,
                CatalogClassifications = graph.Catalogs.ToDictionary(
                    c => c.Path,
                    c => c.Classification.ToString()),
                KnownOrphans = graph.Catalogs
                    .Where(c => c.Classification == ContentClassification.ORPHANED)
                    .Select(c => c.Path)
                    .ToList(),
                ExemptedCatalogs = graph.Catalogs
                    .Where(c => !string.IsNullOrEmpty(c.ExemptionId))
                    .Select(c => c.Path)
                    .ToList()
            };
            return baseline;
        }

        /// <summary>
        /// Save a baseline to disk.
        /// </summary>
        public static void SaveBaseline(UtilizationBaseline baseline, string path = BaselinePath)
        {
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };
            string json = System.Text.Json.JsonSerializer.Serialize(baseline, options);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Load a baseline from disk.
        /// </summary>
        public static UtilizationBaseline? LoadBaseline(string path = BaselinePath)
        {
            if (!File.Exists(path)) return null;
            string json = File.ReadAllText(path);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };
            return System.Text.Json.JsonSerializer.Deserialize<UtilizationBaseline>(json, options);
        }

        /// <summary>
        /// Print a human-readable gate report.
        /// </summary>
        public static string FormatReport(UtilizationGateResult result)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"CI Content Utilization Gate: {(result.Passed ? "PASS" : "FAIL")}");
            sb.AppendLine();

            if (result.Errors.Count > 0)
            {
                sb.AppendLine($"## Errors ({result.Errors.Count})");
                foreach (var err in result.Errors)
                    sb.AppendLine($"  ❌ {err}");
                sb.AppendLine();
            }

            if (result.Regressions.Count > 0)
            {
                sb.AppendLine($"## Regressions ({result.Regressions.Count})");
                foreach (var reg in result.Regressions)
                    sb.AppendLine($"  ⚠ {reg}");
                sb.AppendLine();
            }

            if (result.NewOrphans.Count > 0)
            {
                sb.AppendLine($"## New Orphans ({result.NewOrphans.Count})");
                foreach (var orp in result.NewOrphans)
                    sb.AppendLine($"  🆕 {orp}");
                sb.AppendLine();
            }

            if (result.Warnings.Count > 0)
            {
                sb.AppendLine($"## Warnings ({result.Warnings.Count})");
                foreach (var warn in result.Warnings)
                    sb.AppendLine($"  ⚠ {warn}");
                sb.AppendLine();
            }

            if (result.Improvements.Count > 0)
            {
                sb.AppendLine($"## Improvements ({result.Improvements.Count})");
                foreach (var imp in result.Improvements)
                    sb.AppendLine($"  ✅ {imp}");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}