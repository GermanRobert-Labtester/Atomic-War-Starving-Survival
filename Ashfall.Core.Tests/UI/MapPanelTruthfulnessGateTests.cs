// SPDX-License-Identifier: MIT
// ASHFALL CI Source Gate: Map panel truthfulness (T11, 2026-10-01).
// Pins the repair of the MapDetailPanel fabricated hazard/survey rows and the
// Plan 32 fog gate on its hazard numbers. Source-scan gate in the style of
// ProductionUiNoFabricatedFallbackGateTests: the panels are presentation-only
// and carry no runtime seam, so their truthful-content contract is asserted
// against source.
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class MapPanelTruthfulnessGateTests
    {
        // Fabricated display rows removed from MapDetailPanel by T11. None of
        // this prose is backed by an authored catalog, a wired system, or a
        // survey authority.
        private static readonly string[] BannedFabricatedLiterals =
        {
            "Lead Shielding / Hazmat Suit + Gas Mask",
            "Standard Dosimeter + Particulate Filter",
            "Stealth Stance Recommended",
            "Standard March",
            "Surface Access Tunnel // Reinforced Hatch",
            "Utility Corridors & Piping Vaults",
            "Collapsed Overpass & Debris Fields",
            "Structural Scrap Metal & Mechanical Parts",
            "Electrical Wiring & Electronic Components",
            "Sealed Medical Supplies & Anti-Rad Compounds",
        };

        // Fabricated route rows removed from MapPanel by T12. None of this
        // prose is derived from the authored wasteland_map_v1.json routes.
        private static readonly string[] BannedFabricatedRouteLiterals =
        {
            "The Works Allotments [5 Ticks, Safe]",
            "Denial Cut Substation [8 Ticks, Radiation Hazard]",
            "Nobody's Crossing Gate [Vouch Access Required]",
            "S2 Logistics Depot [Cold-Weather Transit]",
        };

        // Fabricated fallback location rows and archive counters removed
        // from MapPanel by T15. The fallback prose invented four locations;
        // the archive card printed static 8/4/12/6 constants and a fabricated
        // integrity claim.
        private static readonly string[] BannedFabricatedMapPanelLiterals =
        {
            "Home shelter with active air filtration stack and reinforced airlock.",
            "Overgrown communal agricultural plots with preserved soil beds.",
            "High-voltage transmission hub with heavy fallout accumulation.",
            "Arbitration chokepoint requiring vouch authorization to traverse.",
            "100% Deterministic Seed Verification",
        };

        private static readonly Regex LineComment = new Regex(@"//.*$", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex BlockComment = new Regex(@"/\*.*?\*/", RegexOptions.Singleline | RegexOptions.Compiled);

        private static string FindRepoRoot()
        {
            foreach (string start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "project.godot")))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root from the test run");
        }

        private static string ReadSource(string relativePathFromRepoRoot)
        {
            string path = Path.Combine(FindRepoRoot(), relativePathFromRepoRoot);
            Assert.True(File.Exists(path), $"Expected source file not found: {relativePathFromRepoRoot}");
            return File.ReadAllText(path);
        }

        private static string StripComments(string code)
        {
            code = BlockComment.Replace(code, string.Empty);
            return LineComment.Replace(code, string.Empty);
        }

        [Fact]
        public void MapDetailPanel_RendersNoFabricatedHazardOrSurveyRows()
        {
            string stripped = StripComments(ReadSource("src/UI/MapDetailPanel.cs"));

            foreach (string literal in BannedFabricatedLiterals)
            {
                Assert.False(stripped.Contains(literal, StringComparison.Ordinal),
                    $"MapDetailPanel must not render fabricated content as surveyed fact: '{literal}' reappeared. " +
                    "If no authority backs a row, render a truthful 'no survey on record' state instead.");
            }
        }

        [Fact]
        public void MapDetailPanel_HazardNumbersAreGatedBehindSurveyKnowledge()
        {
            string stripped = StripComments(ReadSource("src/UI/MapDetailPanel.cs"));

            // The fog seam exists with a safe default (charted → numbers).
            Assert.Contains("bool uncharted = false", stripped, StringComparison.Ordinal);
            // The hazard card has an explicit uncharted branch…
            int gateIndex = stripped.IndexOf("if (uncharted)", StringComparison.Ordinal);
            Assert.True(gateIndex >= 0, "MapDetailPanel hazard card must branch on the uncharted fog state");
            // …that reports the absence of survey data instead of numbers…
            Assert.Contains("no survey data", stripped, StringComparison.Ordinal);
            // …and the authored hazard numbers sit in the gated (charted) branch.
            int threatIndex = stripped.IndexOf("\"Threat Tier\"", StringComparison.Ordinal);
            Assert.True(threatIndex > gateIndex,
                "The 'Threat Tier' hazard number must only render in the charted branch (after the uncharted gate)");
            Assert.Contains("\"Ambient Radiation Rate\"", stripped, StringComparison.Ordinal);
        }

        [Fact]
        public void OpenMapDetailPanel_FunnelDerivesFogStateFromCanonicalMap()
        {
            // OpenMapDetailPanel is the single funnel for both MapPanel and
            // MapAtlasPanel inspect requests; the host — not the panel — must
            // decide whether a sector is uncharted, using the same Plan 32
            // predicate as MapPanel's location list.
            string source = ReadSource("src/Main.UiHandlers.cs");

            int funnelIndex = source.IndexOf("public void OpenMapDetailPanel(", StringComparison.Ordinal);
            Assert.True(funnelIndex >= 0, "OpenMapDetailPanel funnel not found in src/Main.UiHandlers.cs");
            string funnel = source.Substring(funnelIndex, Math.Min(8000, source.Length - funnelIndex));

            Assert.Contains("GetNode(locationId)", funnel, StringComparison.Ordinal);
            Assert.Contains("!canonicalMap.IsDiscovered(locationId)", funnel, StringComparison.Ordinal);
            Assert.Contains("MapFogState.Unknown", funnel, StringComparison.Ordinal);
            Assert.Contains(", uncharted", funnel, StringComparison.Ordinal);
        }

        [Fact]
        public void MapPanel_RendersNoFabricatedRouteRows()
        {
            string stripped = StripComments(ReadSource("src/UI/MapPanel.cs"));

            foreach (string literal in BannedFabricatedRouteLiterals)
            {
                Assert.False(stripped.Contains(literal, StringComparison.Ordinal),
                    $"MapPanel must not render fabricated transit corridors: '{literal}' reappeared. " +
                    "Transit rows must project the authored WastelandMapSystem routes.");
            }
        }

        [Fact]
        public void MapPanel_TransitCorridorsProjectCanonicalFogKnownRoutes()
        {
            // Same canonical rule as MapAtlasPanel: a corridor renders only
            // when both endpoints are fog-known through the player-facing
            // intel view, with route facts from the authored map and a
            // truthful empty state when nothing is charted.
            string stripped = StripComments(ReadSource("src/UI/MapPanel.cs"));

            Assert.Contains("GetNodeIntel(route.From)", stripped, StringComparison.Ordinal);
            Assert.Contains("GetNodeIntel(route.To)", stripped, StringComparison.Ordinal);
            Assert.Contains("fromIntel.FogState == Ashfall.Core.World.MapFogState.Unknown", stripped, StringComparison.Ordinal);
            Assert.Contains("route.DistanceKm", stripped, StringComparison.Ordinal);
            Assert.Contains("None on record", stripped, StringComparison.Ordinal);
        }

        [Fact]
        public void MapPanel_LocationFallbackAndArchiveCountersAreReal()
        {
            // T15: an unbound panel must report the absence of location data
            // truthfully, the waypoint count must not be floored at 8, and
            // the archive card must read the standing-record authorities
            // instead of printing static constants.
            string stripped = StripComments(ReadSource("src/UI/MapPanel.cs"));

            foreach (string literal in BannedFabricatedMapPanelLiterals)
            {
                Assert.False(stripped.Contains(literal, StringComparison.Ordinal),
                    $"MapPanel must not render fabricated content: '{literal}' reappeared.");
            }

            Assert.DoesNotContain("Math.Max(totalLocations", stripped, StringComparison.Ordinal);
            Assert.Contains("No cataloged waypoints on record", stripped, StringComparison.Ordinal);
            Assert.Contains("unlockedRoomIds", stripped, StringComparison.Ordinal);
            Assert.Contains("visitCounts", stripped, StringComparison.Ordinal);
            Assert.Contains("StratumCount", stripped, StringComparison.Ordinal);
        }
    }
}
