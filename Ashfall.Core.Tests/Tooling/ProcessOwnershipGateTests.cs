// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: per-frame process ownership (perf sprint 2, Task 31).
//
// Every `_Process`/`_PhysicsProcess` override in src/ must either self-gate via
// `SetProcess(...)`/`SetPhysicsProcess(...)` (so it can be disabled when hidden
// or idle) or be explicitly allowlisted here with a reason. This stops new
// always-on per-frame polling from silently re-entering the codebase, and keeps
// the allowlist shrink-only: an allowlisted file that stops processing fails so
// the reason cannot go stale.
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class ProcessOwnershipGateTests
    {
        /// <summary>
        /// Files that legitimately process every frame and cannot self-gate.
        /// Keep this list minimal and evidence-backed.
        /// </summary>
        private static readonly Dictionary<string, string> AlwaysOnAllowlist = new(StringComparer.Ordinal)
        {
            ["src/Main.Application.cs"] = "engine main loop; owns the frame pump",
            ["src/Audio/AudioManager.cs"] = "single audio-stream manager; must advance every frame",
        };

        [Fact]
        public void EveryPerFrameProcessor_SelfGatesOrIsAllowlisted()
        {
            string repoRoot = FindRepoRoot();
            string srcDir = Path.Combine(repoRoot, "src");
            Assert.True(Directory.Exists(srcDir), $"src directory not found at {srcDir}");

            var offenders = new List<string>();
            foreach (string file in Directory.EnumerateFiles(srcDir, "*.cs", SearchOption.AllDirectories))
            {
                string text = File.ReadAllText(file);
                if (!text.Contains("override void _Process(", StringComparison.Ordinal) &&
                    !text.Contains("override void _PhysicsProcess(", StringComparison.Ordinal))
                    continue;

                string relative = Path.GetRelativePath(repoRoot, file).Replace('\\', '/');
                if (AlwaysOnAllowlist.ContainsKey(relative)) continue;

                bool selfGates = text.Contains("SetProcess(", StringComparison.Ordinal) ||
                                 text.Contains("SetPhysicsProcess(", StringComparison.Ordinal);
                if (!selfGates)
                    offenders.Add(relative);
            }

            Assert.True(offenders.Count == 0,
                "These src/ files override a per-frame callback but never call SetProcess/SetPhysicsProcess, " +
                "so they cannot be disabled when hidden or idle. Gate them, or add an evidence-backed " +
                "AlwaysOnAllowlist entry in this gate.\n" + string.Join("\n", offenders));
        }

        [Fact]
        public void AllowlistEntries_StillOverrideAPerFrameCallback()
        {
            string repoRoot = FindRepoRoot();
            foreach (string relative in AlwaysOnAllowlist.Keys)
            {
                string path = Path.Combine(repoRoot, relative.Replace('/', Path.DirectorySeparatorChar));
                Assert.True(File.Exists(path), $"allowlisted file no longer exists: {relative}");
                string text = File.ReadAllText(path);
                bool processes = text.Contains("override void _Process(", StringComparison.Ordinal) ||
                                 text.Contains("override void _PhysicsProcess(", StringComparison.Ordinal);
                Assert.True(processes,
                    $"'{relative}' is allowlisted as an always-on processor but no longer overrides " +
                    "_Process/_PhysicsProcess; remove it from the allowlist (shrink-only).");
            }
        }

        [Fact]
        public void IdleSurvivorActor_GatesPhysicsProcess()
        {
            string repoRoot = FindRepoRoot();
            string path = Path.Combine(repoRoot, "src", "World", "SurvivorActorView.cs");
            Assert.True(File.Exists(path), $"SurvivorActorView.cs not found at {path}");
            string text = File.ReadAllText(path);

            Assert.Contains("override void _PhysicsProcess(", text);
            Assert.Contains("SetPhysicsProcess(", text);
            Assert.Contains("RefreshPhysicsProcess", text);
        }

        [Fact]
        public void HiddenCombatPanel_GatesProcess()
        {
            string repoRoot = FindRepoRoot();
            string path = Path.Combine(repoRoot, "src", "UI", "CombatPanel.cs");
            Assert.True(File.Exists(path), $"CombatPanel.cs not found at {path}");
            string text = File.ReadAllText(path);

            Assert.Contains("override void _Process(", text);
            Assert.Contains("SetProcess(", text);
        }

        private static string FindRepoRoot()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(dir))
            {
                if (File.Exists(Path.Combine(dir, "project.godot")))
                    return dir;
                dir = Path.GetDirectoryName(dir);
            }
            throw new InvalidOperationException("Could not locate repository root from test execution directory.");
        }
    }
}
