// SPDX-License-Identifier: MIT
// Audit #27 — production collectible effect feeder must stay wired on Main.
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class CollectibleProductionFeederGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        [Fact]
        public void MainCollectibles_ConstructsDispatcher_AndWiresInventoryOnItemAdded()
        {
            string text = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Collectibles.cs"));
            Assert.Contains("new CollectibleEffectDispatcher(", text);
            Assert.Contains("OnItemAdded", text);
            Assert.Contains("DispatchOnAcquire", text);
            Assert.Contains("MarkCollectiblesDirty()", text);
            Assert.True(
                Regex.IsMatch(text, @"WireCollectibleInventoryFeeder"),
                "inventory feeder helper must remain for SetupInventory rebind");
        }

        [Fact]
        public void SetupInventory_RebindsCollectibleFeeder()
        {
            string text = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Inventory.cs"));
            Assert.Contains("WireCollectibleInventoryFeeder()", text);
        }
    }
}
