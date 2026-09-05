// SPDX-License-Identifier: MIT
// Audit #36 — agent rulebook body must stay synced with AGENTS.md.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins multi-client rulebook sync. Headers may differ (client title /
    /// generated date); body after the non-negotiable marker must match
    /// <c>AGENTS.md</c>. Equivalent to <c>python3 scripts/ci/sync-agent-rulebooks.py --check</c>.
    /// </summary>
    public sealed class AgentRulebookSyncGateTests
    {
        private static readonly string[] ClientFiles =
        {
            "CLAUDE.md", "CODEX.md", "CRUSH.md", "GOOSE.md", "QWEN.md", "VIBE.md",
            "MIMOCODE.md", "OPENSETUP.md", "ANTIGRAVITY.md",
            ".clinerules", ".cursorrules", ".windsurfrules",
        };

        private const string Marker = "## READ THIS FIRST — NON-NEGOTIABLE RULES";

        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "AGENTS.md")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        private static string BodyAfterMarker(string text)
        {
            int idx = text.IndexOf(Marker, StringComparison.Ordinal);
            Assert.True(idx >= 0, "missing non-negotiable marker");
            return text.Substring(idx);
        }

        [Fact]
        public void ClientRulebooks_MatchAgentsBody()
        {
            string root = RepoRoot();
            string canonical = BodyAfterMarker(File.ReadAllText(Path.Combine(root, "AGENTS.md")));
            var drifted = new List<string>();

            foreach (string name in ClientFiles)
            {
                string path = Path.Combine(root, name);
                if (!File.Exists(path))
                {
                    drifted.Add($"{name} (missing)");
                    continue;
                }

                string body = BodyAfterMarker(File.ReadAllText(path));
                if (!string.Equals(body, canonical, StringComparison.Ordinal))
                    drifted.Add(name);
            }

            Assert.True(drifted.Count == 0,
                "Client rulebooks drifted from AGENTS.md body — run scripts/ci/sync-agent-rulebooks.py:\n  "
                + string.Join("\n  ", drifted));
        }

        [Fact]
        public void SyncScript_CheckMode_ExitsZero()
        {
            string root = RepoRoot();
            string script = Path.Combine(root, "scripts", "ci", "sync-agent-rulebooks.py");
            Assert.True(File.Exists(script), "sync-agent-rulebooks.py missing");

            var psi = new ProcessStartInfo
            {
                FileName = "python3",
                Arguments = $"\"{script}\" --check",
                WorkingDirectory = root,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            using var proc = Process.Start(psi);
            Assert.NotNull(proc);
            string stdout = proc!.StandardOutput.ReadToEnd();
            string stderr = proc.StandardError.ReadToEnd();
            proc.WaitForExit(60_000);
            Assert.True(proc.ExitCode == 0,
                $"sync --check failed (exit {proc.ExitCode}):\n{stdout}\n{stderr}");
        }
    }
}
