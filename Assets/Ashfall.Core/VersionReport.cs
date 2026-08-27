// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ashfall.Core
{
    /// <summary>
    /// Engine-agnostic version report for the ASHFALL host CLI `--version`
    /// flag. Surfaces three version domains in one stable, greppable format:
    ///
    ///   1. build/game version     — supplied by the host (project settings)
    ///   2. data schema versions   — scanned live from the JSON data authority
    ///   3. save schema versions   — the CurrentSaveVersion constants of every
    ///                                versioned save codec in Core
    ///
    /// Save-store versions are read from the codec constants themselves
    /// (never duplicated as literals here), so this report can never drift
    /// from the actual migration gates. The output shape is pinned by
    /// Ashfall.Core.Tests/VersionReportContractTests.cs.
    /// </summary>
    public static class VersionReport
    {
        /// <summary>A save store's current schema version, sourced from its codec constant.</summary>
        public readonly struct SaveSchemaEntry
        {
            public readonly string Store;
            public readonly int CurrentVersion;

            public SaveSchemaEntry(string store, int currentVersion)
            {
                Store = store;
                CurrentVersion = currentVersion;
            }
        }

        /// <summary>
        /// Current schema versions of every versioned save codec in Core.
        /// Values reference the codec constants directly so bumping a codec
        /// updates this report without a second edit.
        /// </summary>
        public static readonly SaveSchemaEntry[] SaveSchemaVersions =
        {
            new SaveSchemaEntry("holdfast",          HoldfastSave.CurrentSaveVersion),
            new SaveSchemaEntry("year_of_ash",       YearOfAsh.YearOfAshSave.CurrentSaveVersion),
            new SaveSchemaEntry("dose_ledger",       DoseLedgerSave.CurrentSaveVersion),
            new SaveSchemaEntry("expansion_hub",     ExpansionHubSave.CurrentSaveVersion),
            new SaveSchemaEntry("expansion_quest",   ExpansionQuestSaveEnvelope.CurrentVersion),
        };

        /// <summary>Summary of schema_version values found across the data authority.</summary>
        public sealed class DataSchemaSummary
        {
            /// <summary>Total JSON catalogs scanned (inventory: every readable *.json).</summary>
            public int Catalogs;
            /// <summary>Catalogs carrying an explicit schema_version field.</summary>
            public int WithSchemaVersion;
            /// <summary>Catalogs without a schema_version field (counted as version 0).</summary>
            public int WithoutSchemaVersion;
            /// <summary>Highest schema_version observed (0 when none present).</summary>
            public int MaxVersion;
            /// <summary>Distinct versions in ascending order with their file counts.</summary>
            public List<(int Version, int Files)> Distribution = new List<(int, int)>();
        }

        /// <summary>
        /// Scans every *.json under dataDir (recursively) and tallies the
        /// top-level schema_version field of each catalog. This is an
        /// inventory, not a validation gate (that is
        /// --data-integrity-selftest): every readable file counts, and a
        /// missing/unreadable schema_version groups under version 0.
        /// </summary>
        public static DataSchemaSummary ScanDataSchemas(string dataDir)
        {
            var summary = new DataSchemaSummary();
            if (string.IsNullOrEmpty(dataDir) || !Directory.Exists(dataDir))
                return summary;

            var counts = new SortedDictionary<int, int>();
            foreach (var path in Directory.EnumerateFiles(dataDir, "*.json", SearchOption.AllDirectories))
            {
                string text;
                try { text = File.ReadAllText(path); }
                catch (IOException) { continue; }

                // Minimal top-level schema_version extraction — consistent
                // with the tolerant raw-text extractors used by the loaders.
                summary.Catalogs++;
                int? version = ExtractTopLevelSchemaVersion(text);
                int v = version ?? 0;
                counts[v] = counts.TryGetValue(v, out var c) ? c + 1 : 1;
                if (version.HasValue)
                {
                    summary.WithSchemaVersion++;
                    if (version.Value > summary.MaxVersion) summary.MaxVersion = version.Value;
                }
                else
                {
                    summary.WithoutSchemaVersion++;
                }
            }

            summary.Distribution = counts
                .Select(kv => (kv.Key, kv.Value))
                .ToList();
            return summary;
        }

        /// <summary>
        /// Extracts a top-level "schema_version": N from JSON object text.
        /// Returns null when absent or unparseable at the top level.
        /// </summary>
        private static int? ExtractTopLevelSchemaVersion(string json)
        {
            // Find "schema_version" occurrences; take the one with the smallest
            // brace depth (top level). Depth tracking is cheap and avoids the
            // need for a full parser while ignoring nested duplicates.
            int depth = 0;
            int i = 0;
            int bestDepth = int.MaxValue;
            int bestValue = 0;
            while (i < json.Length)
            {
                char ch = json[i];
                if (ch == '{') depth++;
                else if (ch == '}') depth--;
                else if (ch == '"' && json.IndexOf("\"schema_version\"", i, StringComparison.Ordinal) == i)
                {
                    int colon = json.IndexOf(':', i + 16);
                    if (colon >= 0)
                    {
                        int j = colon + 1;
                        while (j < json.Length && (char.IsWhiteSpace(json[j]) || json[j] == '\t')) j++;
                        int start = j;
                        while (j < json.Length && char.IsDigit(json[j])) j++;
                        if (j > start && int.TryParse(json.Substring(start, j - start), out int value) && depth < bestDepth)
                        {
                            bestDepth = depth;
                            bestValue = value;
                        }
                    }
                    i += 16;
                    continue;
                }
                i++;
            }
            return bestDepth == int.MaxValue ? (int?)null : bestValue;
        }

        /// <summary>
        /// Renders the one-line data schema summary, e.g.
        /// "403 catalogs — v1: 401, v2: 2 (max v2)" or "no data directory".
        /// </summary>
        public static string FormatDataSchemas(DataSchemaSummary summary)
        {
            if (summary == null || summary.Catalogs == 0)
                return "no data directory";
            var parts = summary.Distribution
                .Select(d => $"v{d.Version}: {d.Files}");
            return $"{summary.Catalogs} catalogs — {string.Join(", ", parts)} (max v{summary.MaxVersion})";
        }

        /// <summary>
        /// Renders the one-line save schema summary, e.g.
        /// "holdfast v5 · year_of_ash v4 · dose_ledger v2 · expansion_hub v4 · expansion_quest v1".
        /// </summary>
        public static string FormatSaveSchemas()
        {
            return string.Join(" · ", SaveSchemaVersions.Select(s => $"{s.Store} v{s.CurrentVersion}"));
        }

        /// <summary>
        /// Composes the full `--version` report. Line shape is a pinned
        /// contract (see VersionReportContractTests):
        /// <code>
        /// ASHFALL version report
        /// game         : 1.0.0
        /// data schemas : 403 catalogs — v1: 401, v2: 2 (max v2)
        /// save schemas : holdfast v5 · year_of_ash v4 · ...
        /// </code>
        /// </summary>
        public static string Compose(string gameVersion, string dataDir)
        {
            var sb = new StringBuilder();
            sb.AppendLine("ASHFALL version report");
            sb.AppendLine($"game         : {gameVersion}");
            sb.AppendLine($"data schemas : {FormatDataSchemas(ScanDataSchemas(dataDir))}");
            sb.AppendLine($"save schemas : {FormatSaveSchemas()}");
            return sb.ToString();
        }
    }
}
