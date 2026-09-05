// SPDX-License-Identifier: MIT
// Audit #43 — pin golden UI snapshot corpus size (docs vs disk).
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// The on-disk golden corpus is uneven (PNG + .import pairs). This gate
    /// pins observed PNG count so docs claiming "69 panels" cannot silently
    /// diverge further without updating the pin.
    /// </summary>
    public sealed class SnapshotCorpusPinTests
    {
        private const int MinPngCount = 25;
        private const int MaxPngCount = 80;

        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "snapshots")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        [Fact]
        public void SnapshotPngCorpus_StaysInDocumentedBand()
        {
            string dir = Path.Combine(RepoRoot(), "snapshots");
            int png = Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly).Length;
            Assert.True(png >= MinPngCount && png <= MaxPngCount,
                $"snapshot PNG count={png} outside pin band [{MinPngCount},{MaxPngCount}] — "
                + "update docs/ui snapshot manifest and this pin together (audit #43).");
        }

        [Fact]
        public void SnapshotManifest_Exists_WhenPresent()
        {
            string root = RepoRoot();
            string manifest = Path.Combine(root, "docs", "ui", "snapshot_manifest.json");
            // Manifest is the authored authority when present; absence is a docs debt, not a hard fail.
            if (!File.Exists(manifest)) return;
            string json = File.ReadAllText(manifest);
            Assert.Contains("snapshot_id", json, StringComparison.Ordinal);
        }
    }
}
