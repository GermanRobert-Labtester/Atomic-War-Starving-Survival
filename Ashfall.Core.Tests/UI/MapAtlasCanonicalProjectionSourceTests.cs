// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    /// <summary>
    /// Pins the Map Atlas presentation contract to the canonical wasteland-map
    /// authority and prevents regression to fabricated UI telemetry.
    /// </summary>
    public sealed class MapAtlasCanonicalProjectionSourceTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "Ashfall.sln")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "Assets", "Ashfall.Core")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return Directory.GetCurrentDirectory();
        }

        [Fact]
        public void MapAtlas_UsesCanonicalWorldMapAndDoesNotInventTelemetryOrCommands()
        {
            string root = RepoRoot();
            string panel = File.ReadAllText(Path.Combine(root, "src", "UI", "MapAtlasPanel.cs"));
            string surfaces = File.ReadAllText(Path.Combine(root, "src", "Main.PlayerSurfaces.cs"));

            Assert.Contains("WorldHostSession", panel, StringComparison.Ordinal);
            Assert.Contains("WastelandMap", panel, StringComparison.Ordinal);
            Assert.Contains("GetNodeIntel(", panel, StringComparison.Ordinal);
            Assert.Contains("ResolveNodeStatus(", panel, StringComparison.Ordinal);
            Assert.Contains("GetRoutesFrom(", panel, StringComparison.Ordinal);
            Assert.Contains("_mapAtlasPanel.Bind(_expeditions, _world)", surfaces, StringComparison.Ordinal);

            Assert.DoesNotContain("InferSector", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("dangerLevel * 2.5", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("\\\"Rads/h\\\"", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("BuildActionFixtureRows", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("\\\"Dispatch Sortie\\\"", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("\\\"Plot Waypoint\\\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void MapAtlas_SelectionUsesOneCanonicalRowIndex()
        {
            string root = RepoRoot();
            string panel = File.ReadAllText(Path.Combine(root, "src", "UI", "MapAtlasPanel.cs"));

            Assert.Contains("_selectedIndex = index;", panel, StringComparison.Ordinal);
            Assert.Contains("OnLocationSelected?.Invoke(_locations[index].Id);", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("ResolveVisibleRow", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("QuadrantForSector", panel, StringComparison.Ordinal);
        }
    }
}
