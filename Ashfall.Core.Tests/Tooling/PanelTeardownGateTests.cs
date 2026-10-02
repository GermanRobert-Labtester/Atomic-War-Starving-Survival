// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: panel/session teardown hygiene (perf sprint 2, Task 32).
//
// A panel that subscribes to a long-lived session's StateChanged must detach
// when it leaves the tree. Otherwise the session keeps the panel alive after
// QueueFree (event-handler leak). This gate pins the panels that bind a session
// and asserts a real teardown path. It is curated rather than repo-wide because
// the broad sweep of the remaining src/UI panels is a separate package.
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class PanelTeardownGateTests
    {
        [Theory]
        [InlineData("src/Host/HoldfastTerminalPanel.cs", "_session.StateChanged += RefreshView", "_session.StateChanged -= RefreshView")]
        [InlineData("src/UI/CombatPanel.cs", "_combat.StateChanged += RefreshView", "_combat.StateChanged -= RefreshView")]
        public void SessionSubscribingPanel_HasExitTreeUnsubscribe(string relativePath, string subscribe, string unsubscribe)
        {
            string repoRoot = FindRepoRoot();
            string path = Path.Combine(repoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
            Assert.True(File.Exists(path), $"Expected panel not found: {relativePath}");
            string text = File.ReadAllText(path);

            Assert.Contains(subscribe, text);
            Assert.Contains(unsubscribe, text);
            Assert.Contains("override void _ExitTree(", text);
        }

        [Fact]
        public void HoldfastTerminalPanel_ExitTreeCallsUnbind()
        {
            string repoRoot = FindRepoRoot();
            string path = Path.Combine(repoRoot, "src", "Host", "HoldfastTerminalPanel.cs");
            string text = File.ReadAllText(path);

            Assert.Contains("public void Unbind()", text);

            int exitTree = text.IndexOf("override void _ExitTree(", StringComparison.Ordinal);
            Assert.True(exitTree >= 0, "HoldfastTerminalPanel must override _ExitTree");

            int unbindCall = text.IndexOf("Unbind();", exitTree, StringComparison.Ordinal);
            Assert.True(unbindCall > exitTree,
                "HoldfastTerminalPanel._ExitTree must call Unbind() so the bound session drops its StateChanged handler.");
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
