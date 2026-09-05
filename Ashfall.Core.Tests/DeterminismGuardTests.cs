using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 44 — Mechanical CI source-scan lint gate enforcing Invariant 4 (Determinism).
    /// Banned patterns across Assets/Ashfall.Core and src:
    ///   - new Random( / new System.Random(
    ///   - Random.Shared
    ///   - Guid.NewGuid
    ///   - DateTime.Now
    ///   - DateTime.UtcNow
    ///
    /// Any non-gameplay/host diagnostic exception must carry an inline
    /// "DETERMINISM_ALLOWLIST: <reason>" comment.
    /// </summary>
    public class DeterminismGuardTests
    {
        private static readonly Regex[] BannedPatterns = new[]
        {
            new Regex(@"\bnew\s+(?:System\.)?Random\s*\(", RegexOptions.Compiled),
            new Regex(@"\bRandom\.Shared\b", RegexOptions.Compiled),
            new Regex(@"\bGuid\.NewGuid\s*\(", RegexOptions.Compiled),
            new Regex(@"\bDateTime\.Now\b", RegexOptions.Compiled),
            new Regex(@"\bDateTime\.UtcNow\b", RegexOptions.Compiled)
        };

        private static string GetRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
            {
                dir = dir.Parent;
            }
            return dir?.FullName ?? throw new InvalidOperationException("Could not find repository root containing Ashfall.csproj");
        }

        [Fact]
        public void SourceScan_AssetsAshfallCore_And_Src_MustNotContainUnsanctionedBannedPatterns()
        {
            string repoRoot = GetRepoRoot();
            var targetDirs = new[]
            {
                Path.Combine(repoRoot, "Assets", "Ashfall.Core"),
                Path.Combine(repoRoot, "src")
            };

            var violations = new List<string>();

            foreach (var dir in targetDirs)
            {
                if (!Directory.Exists(dir)) continue;

                var csFiles = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);
                foreach (var file in csFiles)
                {
                    // Skip bin/obj or hidden directories
                    if (file.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") ||
                        file.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") ||
                        file.Contains($"{Path.DirectorySeparatorChar}."))
                    {
                        continue;
                    }

                    ScanFile(file, violations);
                }
            }

            Assert.True(violations.Count == 0,
                $"Determinism Guard found {violations.Count} unsanctioned determinism violations:\n" +
                string.Join("\n", violations));
        }

        private static void ScanFile(string filePath, List<string> violations)
        {
            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();

                // Skip full line comments
                if (trimmed.StartsWith("//") || trimmed.StartsWith("/*") || trimmed.StartsWith("*") || trimmed.StartsWith("///"))
                    continue;

                foreach (var pattern in BannedPatterns)
                {
                    var match = pattern.Match(line);
                    if (match.Success)
                    {
                        // Check if the match is inside an end-of-line comment
                        int commentIndex = line.IndexOf("//", StringComparison.Ordinal);
                        if (commentIndex >= 0 && match.Index > commentIndex)
                        {
                            // Match is inside a comment on this line
                            continue;
                        }

                        // Check if allowlisted on the same line or standalone previous comment line
                        bool isAllowlisted = line.Contains("DETERMINISM_ALLOWLIST:", StringComparison.Ordinal) ||
                                             (i > 0 && lines[i - 1].TrimStart().StartsWith("//") && lines[i - 1].Contains("DETERMINISM_ALLOWLIST:", StringComparison.Ordinal));

                        if (!isAllowlisted)
                        {
                            violations.Add($"{filePath}:{i + 1} -> '{match.Value}' in line: '{trimmed}'");
                        }
                    }
                }
            }
        }

        [Fact]
        public void GuardLogic_DetectsUnsanctionedPattern_InSampleLines()
        {
            var testLines = new[]
            {
                "var r = new Random();",
                "// DateTime.UtcNow in comment is fine",
                "var t = DateTime.UtcNow; // DETERMINISM_ALLOWLIST: Allowed test",
                "var id = Guid.NewGuid();"
            };

            var testFile = Path.GetTempFileName();
            try
            {
                File.WriteAllLines(testFile, testLines);
                var violations = new List<string>();
                ScanFile(testFile, violations);

                Assert.Equal(2, violations.Count);
                Assert.Contains(violations, v => v.Contains("new Random("));
                Assert.Contains(violations, v => v.Contains("Guid.NewGuid("));
            }
            finally
            {
                if (File.Exists(testFile)) File.Delete(testFile);
            }
        }
    }
}
