// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Multi-Agent Rulebook Integrity Tests

using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public class AgentRuleIntegrityTests
    {
        private static string RepoRoot()
        {
            string dir = AppContext.BaseDirectory;
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir, "AGENTS.md")) && Directory.Exists(Path.Combine(dir, "Assets", "StreamingAssets", "Data")))
                    return dir;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            throw new DirectoryNotFoundException("Repository root not found");
        }

        private static readonly string[] ClientRuleFiles =
        {
            "CLAUDE.md",
            "CODEX.md",
            "CRUSH.md",
            "GOOSE.md",
            "QWEN.md",
            "VIBE.md",
            "MIMOCODE.md",
            "OPENSETUP.md",
            "ANTIGRAVITY.md",
            ".clinerules",
            ".cursorrules",
            ".windsurfrules"
        };

        [Fact]
        public void CanonicalAgentsMd_ContainsFiveNonNegotiableRules()
        {
            string root = RepoRoot();
            string agentsMd = File.ReadAllText(Path.Combine(root, "AGENTS.md"));

            Assert.Contains("READ THIS FIRST — NON-NEGOTIABLE RULES", agentsMd);
            Assert.Contains("Unity is NOT a target editor", agentsMd);
            Assert.Contains("All verification uses `dotnet` + `godot --headless`", agentsMd);
            Assert.Contains("Migration direction is Unity → Godot, always", agentsMd);
            Assert.Contains("Unity assets must be migrated to Godot assets", agentsMd);
            Assert.Contains("Core stays engine-agnostic", agentsMd);
        }

        [Fact]
        public void AllClientRulebooks_ContainFiveNonNegotiableRulesAndCanonicalMCPAliases()
        {
            string root = RepoRoot();

            foreach (var clientFile in ClientRuleFiles)
            {
                string filePath = Path.Combine(root, clientFile);
                Assert.True(File.Exists(filePath), $"Client rule file missing: {clientFile}");

                string content = File.ReadAllText(filePath);
                Assert.Contains("READ THIS FIRST — NON-NEGOTIABLE RULES", content);
                Assert.Contains("Unity is NOT a target editor", content);
                Assert.Contains("All verification uses `dotnet` + `godot --headless`", content);
                Assert.Contains("Core stays engine-agnostic", content);
                Assert.Contains("composio", content);
                Assert.Contains("google-stitch", content);
            }
        }
    }
}
