// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Result row representing one inspected asset ID and its resolution status.
    /// </summary>
    public struct AssetCoverageResultRow
    {
        public string Id;
        public string Category;
        public string? ResolvedPath;
        public bool Exists;
        public bool Loaded;
        public int ReferenceCount;
    }

    /// <summary>
    /// Aggregated summary report of an asset coverage scan.
    /// </summary>
    public struct AssetCoverageSummaryReport
    {
        public int TotalChecked;
        public int Missing;
        public int UniqueMissing;
        public int DuplicateFallbackRequests;
        public int FailedToLoad;
        public int Passed;
        /// <summary>Normalization/negative/dedup probe mismatches (gate-blocking).</summary>
        public int ProbeFailures;
        public List<AssetCoverageResultRow> Rows;
        public string Summary;
        public bool Clean => Missing == 0 && FailedToLoad == 0 && ProbeFailures == 0;
    }

    /// <summary>
    /// Formatting and output service for asset coverage and audit reports.
    /// </summary>
    public static class AssetCoverageReport
    {
        public static void PrintSummary(AssetCoverageSummaryReport report)
        {
            GD.Print("[AssetRegistrySelfTest] --- SUMMARY ---");
            GD.Print($"[AssetRegistrySelfTest] Total checked: {report.TotalChecked}");
            GD.Print($"[AssetRegistrySelfTest] Passed: {report.Passed}");
            GD.Print($"[AssetRegistrySelfTest] Missing (checked entries): {report.Missing}");
            GD.Print($"[AssetRegistrySelfTest] Unique missing assets: {report.UniqueMissing}");
            GD.Print($"[AssetRegistrySelfTest] Duplicate fallback requests: {report.DuplicateFallbackRequests}");
            GD.Print($"[AssetRegistrySelfTest] Failed to load: {report.FailedToLoad}");
            GD.Print($"[AssetRegistrySelfTest] {report.Summary}");

            if (report.Missing > 0 || report.FailedToLoad > 0)
            {
                GD.Print("[AssetRegistrySelfTest] --- ISSUES ---");
                foreach (var row in report.Rows)
                {
                    if (!row.Exists)
                    {
                        GD.Print($"[AssetRegistrySelfTest] MISSING: [{row.Category}] {row.Id}");
                    }
                    else if (!row.Loaded)
                    {
                        GD.Print($"[AssetRegistrySelfTest] LOAD FAILED: [{row.Category}] {row.Id} at {row.ResolvedPath}");
                    }
                }
            }

            GD.Print(report.Clean
                ? "ASSET_REGISTRY_SELFTEST PASS"
                : $"ASSET_REGISTRY_SELFTEST FAIL (missing={report.Missing}, failed={report.FailedToLoad}, probe-failures={report.ProbeFailures})");
        }

        public static void PrintFullCoverageSweep(
            int totalIds,
            int totalMissing,
            Dictionary<string, List<string>> missingByCategory,
            Dictionary<string, List<string>> idsByCategory)
        {
            foreach (var (category, ids) in idsByCategory)
            {
                var missing = missingByCategory[category];
                GD.Print($"[AssetCoverageReport] {category,-9}: {ids.Count,4} ids, {ids.Count - missing.Count,4} resolved, {missing.Count,4} missing");
                foreach (var id in missing)
                    GD.Print($"[AssetCoverageReport]   MISSING {category}: {id}");
            }

            GD.Print($"ASSET_COVERAGE_REPORT: ids={totalIds} resolved={totalIds - totalMissing} missing={totalMissing} (report-only; gate remains --asset-registry-selftest)");
        }
    }
}
