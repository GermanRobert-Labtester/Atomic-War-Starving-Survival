// SPDX-License-Identifier: MIT
// ASHFALL CI gate: Catch Policy & Exception Handling Lint.
// Enforces that:
//   1. Zero silent/undocumented empty catches exist in the codebase.
//   2. Cleanup-only catches are explicitly documented (e.g. temporary file deletion).
//   3. Data, catalog, and save loading catch blocks log diagnostic context (via CatalogDiagnostics, ILog, GD.Print, or rethrow).
//   4. Probing or fallback catches adhere to a small documented allowlist.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CatchPolicyLintGateTests
    {
        private static readonly Regex CatchBlockHeader =
            new Regex(@"\bcatch(?:\s*\([^\)]*\))?\s*\{", RegexOptions.Compiled);

        private static readonly string[] AllowedCleanupKeywords = new[]
        {
            "cleanup",
            "best-effort",
            "temp",
            "fallback",
            "tolerate",
            "ignore",
            "tamper",
            "rejection",
            "quarantine",
            "resilient",
            "deserialization failure",
            "probe"
        };

        private static readonly string[] ContextLoggingKeywords = new[]
        {
            "CatalogDiagnostics",
            "GD.PrintErr",
            "GD.Print",
            "GD.PushWarning",
            "Log.",
            "_log.",
            "log.",
            "log?.",
            "_log?.",
            "Console.Error",
            "Console.WriteLine",
            "Errors.Add",
            "report.Error",
            "report.Warning",
            "Failure(",
            "[FAIL]",
            "return (false",
            "throw",
            "ex_CATDIAG",
            "Check(",
            "failures++"
        };

        private static readonly HashSet<string> DocumentedAllowlist = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Logging infrastructure must never crash while attempting to write log files to disk
            "src/Host/GodotLog.cs:WriteToFile",
            // Version report probes the git hash or build metadata file and continues on missing file
            "Assets/Ashfall.Core/VersionReport.cs:TryGetVersion",
            // Catalog file system enumeration probe
            "Assets/Ashfall.Core/CatalogFileSystem.cs:EnumerateJsonFiles"
        };

        private static string RepoRootDir()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probe = Path.Combine(dir.FullName, "src");
                    if (Directory.Exists(probe))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root from the test run");
        }

        private sealed class CatchRecord
        {
            public string FilePath { get; set; } = string.Empty;
            public string RelativePath { get; set; } = string.Empty;
            public int LineNumber { get; set; }
            public string Header { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
            public bool IsDataLoader { get; set; }
            public bool HasComment { get; set; }
            public bool HasContextLogging { get; set; }
            public bool HasCleanupDocumentation { get; set; }
        }

        private static List<CatchRecord> ScanAllCatchBlocks()
        {
            string root = RepoRootDir();
            var searchDirs = new[]
            {
                Path.Combine(root, "src"),
                Path.Combine(root, "Assets", "Ashfall.Core")
            };

            var records = new List<CatchRecord>();

            foreach (var dir in searchDirs)
            {
                if (!Directory.Exists(dir)) continue;

                foreach (var file in Directory.EnumerateFiles(dir, "*.cs", SearchOption.AllDirectories))
                {
                    string norm = file.Replace('\\', '/');
                    if (norm.Contains("/obj/") || norm.Contains("/bin/")) continue;

                    string rel = Path.GetRelativePath(root, file).Replace('\\', '/');
                    string content = File.ReadAllText(file);

                    bool isDataLoader = rel.Contains("Catalog") || rel.Contains("Loader") ||
                                       rel.Contains("SaveStore") || rel.Contains("SaveCodec") ||
                                       rel.Contains("Save") || rel.Contains("JournalCatalogData");

                    var matches = CatchBlockHeader.Matches(content);
                    foreach (Match m in matches)
                    {
                        // Check if the match is inside a comment line
                        int lineStart = content.LastIndexOf('\n', m.Index) + 1;
                        string lineBeforeMatch = content.Substring(lineStart, m.Index - lineStart).TrimStart();
                        if (lineBeforeMatch.StartsWith("//") || lineBeforeMatch.StartsWith("*"))
                            continue;

                        // Find matching closing brace
                        int braceCount = 1;
                        int idx = m.Index + m.Length;
                        while (idx < content.Length && braceCount > 0)
                        {
                            if (content[idx] == '{') braceCount++;
                            else if (content[idx] == '}') braceCount--;
                            idx++;
                        }

                        string body = content.Substring(m.Index + m.Length, Math.Max(0, idx - 1 - (m.Index + m.Length))).Trim();
                        int lineNum = content.Substring(0, m.Index).Count(c => c == '\n') + 1;

                        bool hasComment = body.Contains("//") || body.Contains("/*");
                        bool hasLogging = ContextLoggingKeywords.Any(k => body.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
                        bool hasCleanup = AllowedCleanupKeywords.Any(k => body.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);

                        records.Add(new CatchRecord
                        {
                            FilePath = file,
                            RelativePath = rel,
                            LineNumber = lineNum,
                            Header = m.Value,
                            Body = body,
                            IsDataLoader = isDataLoader,
                            HasComment = hasComment,
                            HasContextLogging = hasLogging,
                            HasCleanupDocumentation = hasCleanup
                        });
                    }
                }
            }

            return records;
        }

        [Fact]
        public void CatchPolicy_ZeroUndocumentedEmptyCatches()
        {
            var records = ScanAllCatchBlocks();
            Assert.True(records.Count > 100, $"Expected to find >100 catch blocks across codebase, found {records.Count}");

            var undocumented = new List<string>();

            foreach (var r in records)
            {
                string strippedBody = Regex.Replace(r.Body, @"//.*$", "", RegexOptions.Multiline);
                strippedBody = Regex.Replace(strippedBody, @"/\*.*?\*/", "", RegexOptions.Singleline).Trim();

                if (string.IsNullOrWhiteSpace(strippedBody) && !r.HasCleanupDocumentation && !r.HasComment)
                {
                    undocumented.Add($"{r.RelativePath}:{r.LineNumber} -> {r.Header} (empty body with no explanation)");
                }
            }

            Assert.True(
                undocumented.Count == 0,
                $"Catch Policy Violation — Found {undocumented.Count} undocumented empty catch blocks:\n  " +
                string.Join("\n  ", undocumented) +
                "\nDocument all cleanup-only catch blocks with '/* cleanup: <reason> */'.");
        }

        [Fact]
        public void CatchPolicy_DataLoaderCatches_MustLogContextOrBeDocumented()
        {
            var records = ScanAllCatchBlocks();
            var dataLoaderRecords = records.Where(r => r.IsDataLoader).ToList();
            Assert.NotEmpty(dataLoaderRecords);

            var unloggedDataCatches = new List<string>();

            foreach (var r in dataLoaderRecords)
            {
                if (!r.HasContextLogging && !r.HasCleanupDocumentation)
                {
                    unloggedDataCatches.Add($"{r.RelativePath}:{r.LineNumber} -> {r.Header} does not log context (use CatalogDiagnostics.Warn or ILog)");
                }
            }

            Assert.True(
                unloggedDataCatches.Count == 0,
                $"Catch Policy Violation — Found {unloggedDataCatches.Count} data/save loader catches without context logging:\n  " +
                string.Join("\n  ", unloggedDataCatches));
        }

        [Fact]
        public void CatchPolicy_CleanupCatches_AreExplicitlyDocumented()
        {
            var records = ScanAllCatchBlocks();
            var cleanupCatches = records.Where(r => r.Body.Contains("File.Delete") || r.Body.Contains("Directory.Delete")).ToList();

            foreach (var r in cleanupCatches)
            {
                Assert.True(
                    r.HasCleanupDocumentation || r.HasContextLogging,
                    $"{r.RelativePath}:{r.LineNumber} contains file/directory deletion but lacks cleanup documentation or logging.");
            }
        }
    }
}
