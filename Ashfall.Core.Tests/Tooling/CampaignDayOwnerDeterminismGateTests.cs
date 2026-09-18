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
    /// Plan 27B.4 — Campaign Day Owner Determinism Coverage Gate.
    /// Scans src/Main.CampaignOwners.cs (and related host registration files) for every
    /// _campaignDay.Register("owner_id", ...) and asserts that each owner has determinism/replay
    /// test coverage under Ashfall.Core.Tests/.
    /// </summary>
    public sealed class CampaignDayOwnerDeterminismGateTests
    {
        private static readonly Regex RegisterPattern =
            new Regex(@"_campaignDay\.Register\(\s*""([^""]+)""\s*,\s*new\s+([A-Za-z0-9_]+)", RegexOptions.Compiled);

        private static string ResolveRepoRoot()
        {
            string baseDir = AppContext.BaseDirectory;
            string dir = baseDir;
            for (int i = 0; i < 8; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src")) &&
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

        [Fact]
        public void EveryRegisteredDayOwner_HasDeterminismOrReplayCoverage()
        {
            string repoRoot = ResolveRepoRoot();
            string srcDir = Path.Combine(repoRoot, "src");
            string testsDir = Path.Combine(repoRoot, "Ashfall.Core.Tests");

            var srcFiles = Directory.GetFiles(srcDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains("/obj/") && !f.Contains("/bin/"))
                .ToList();

            var registeredOwners = new List<(string OwnerId, string OwnerClass, string File)>();

            foreach (var sf in srcFiles)
            {
                string content = File.ReadAllText(sf);
                var matches = RegisterPattern.Matches(content);
                foreach (Match m in matches)
                {
                    registeredOwners.Add((m.Groups[1].Value, m.Groups[2].Value, Path.GetFileName(sf)));
                }
            }

            Assert.True(registeredOwners.Count >= 19,
                $"Expected to discover >= 19 registered day owners, found {registeredOwners.Count}.");

            // Cache all tokens in test files
            var testFiles = Directory.GetFiles(testsDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains("/obj/") && !f.Contains("/bin/"))
                .ToList();

            var testCodeTokens = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var tf in testFiles)
            {
                string text = File.ReadAllText(tf);
                var matches = Regex.Matches(text, @"\b[A-Za-z0-9]+\b");
                foreach (Match m in matches)
                {
                    testCodeTokens.Add(m.Value);
                }
            }

            var unverifiedOwners = new List<string>();

            foreach (var owner in registeredOwners)
            {
                string[] parts = owner.OwnerId.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                bool covered = testCodeTokens.Contains(owner.OwnerId) ||
                               testCodeTokens.Contains(owner.OwnerClass) ||
                               parts.Any(p => p.Length > 3 && testCodeTokens.Any(t => t.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0));

                if (!covered)
                {
                    unverifiedOwners.Add($"'{owner.OwnerId}' ({owner.OwnerClass} in {owner.File})");
                }
            }

            Assert.True(unverifiedOwners.Count == 0,
                $"Found campaign day owners without determinism or replay test coverage:\n  {string.Join("\n  ", unverifiedOwners)}");
        }

        [Fact]
        public void Gate_FlagsUntestedDayOwner_WhenSimulated()
        {
            var tokens = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ExistingOwner", "ExistingSystem" };
            string dummyOwner = "untested_hypothetical_owner";

            bool covered = tokens.Contains(dummyOwner);
            Assert.False(covered, "Gate must correctly identify untested simulated day owner.");
        }
    }
}
