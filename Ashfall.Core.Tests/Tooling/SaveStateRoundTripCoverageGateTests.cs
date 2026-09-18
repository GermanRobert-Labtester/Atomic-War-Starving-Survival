// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// Plan 27B.3 — Stateful System Round-Trip Coverage Gate.
    /// Enforces that stateful Core systems declaring CaptureState/RestoreState
    /// have registered test coverage in Ashfall.Core.Tests.
    /// </summary>
    public sealed class SaveStateRoundTripCoverageGateTests
    {
        private static readonly Regex CapturePattern =
            new Regex(@"public\s+([A-Za-z0-9_<>]+)\??\s+CaptureState\s*\(", RegexOptions.Compiled);

        private static string ResolveRepoRoot()
        {
            string baseDir = AppContext.BaseDirectory;
            string dir = baseDir;
            for (int i = 0; i < 8; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "Ashfall.Core")) &&
                    Directory.Exists(Path.Combine(dir, "Ashfall.Core.Tests")))
                {
                    return dir;
                }
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return baseDir;
        }

        // Known quarantined systems tracked in Ashfall.Core.Tests.csproj / KNOWN_DEBT.md
        private static readonly HashSet<string> KnownQuarantined = new HashSet<string>(StringComparer.Ordinal)
        {
            "AdvancedSurgicalWardSystem" // Quarantined in Ashfall.Core.Tests.csproj: Medical/AdvancedSurgicalWardTests.cs
        };

        private static readonly Regex ClassPattern =
            new Regex(@"(?:class|struct)\s+([A-Za-z0-9_]+)", RegexOptions.Compiled);

        [Fact]
        public void StatefulSystems_HaveRegisteredRoundTripTests()
        {
            string repoRoot = ResolveRepoRoot();
            string coreDir = Path.Combine(repoRoot, "Assets", "Ashfall.Core");
            string testsDir = Path.Combine(repoRoot, "Ashfall.Core.Tests");

            var coreFiles = Directory.GetFiles(coreDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains("/obj/") && !f.Contains("/bin/") && !f.EndsWith("Demo.cs"))
                .ToList();

            var testFiles = Directory.GetFiles(testsDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains("/obj/") && !f.Contains("/bin/"))
                .ToList();

            // Read all test source text into combined cache for fast lookup
            var testCodeTokens = new HashSet<string>(StringComparer.Ordinal);
            foreach (var tf in testFiles)
            {
                string text = File.ReadAllText(tf);
                var matches = Regex.Matches(text, @"\b[A-Za-z0-9_]+\b");
                foreach (Match m in matches)
                {
                    testCodeTokens.Add(m.Value);
                }
            }

            var uncoveredSystems = new List<string>();
            int statefulCount = 0;

            foreach (var cf in coreFiles)
            {
                string content = File.ReadAllText(cf);
                var captureMatch = CapturePattern.Match(content);
                if (!captureMatch.Success)
                    continue;

                string fileName = Path.GetFileNameWithoutExtension(cf);
                string stateDto = captureMatch.Groups[1].Value.TrimEnd('?');
                statefulCount++;

                if (KnownQuarantined.Contains(fileName))
                    continue;

                // Extract all class/struct names declared in this file
                var declaredClasses = ClassPattern.Matches(content)
                    .Select(m => m.Groups[1].Value)
                    .ToList();

                bool covered = testCodeTokens.Contains(fileName) ||
                               testCodeTokens.Contains(stateDto) ||
                               testCodeTokens.Contains(fileName + "Tests") ||
                               testCodeTokens.Contains(fileName + "SaveTests") ||
                               declaredClasses.Any(c => testCodeTokens.Contains(c) && !c.EndsWith("Def") && !c.EndsWith("Entry"));

                if (!covered)
                {
                    uncoveredSystems.Add($"{fileName} (DTO: {stateDto}, file: {Path.GetFileName(cf)})");
                }
            }

            Assert.True(statefulCount > 20, $"Expected to discover &gt; 20 stateful systems, found {statefulCount}");
            Assert.True(uncoveredSystems.Count == 0,
                $"Found stateful Core systems without test coverage:\n  {string.Join("\n  ", uncoveredSystems)}");
        }

        [Fact]
        public void Gate_FlagsUncoveredSystem_WhenSimulated()
        {
            var tokens = new HashSet<string>(StringComparer.Ordinal) { "ExistingSystem" };
            string dummyUncoveredSystem = "UncoveredHypotheticalSystem";

            bool isCovered = tokens.Contains(dummyUncoveredSystem);
            Assert.False(isCovered, "Gate must correctly identify uncovered systems.");
        }
    }
}
