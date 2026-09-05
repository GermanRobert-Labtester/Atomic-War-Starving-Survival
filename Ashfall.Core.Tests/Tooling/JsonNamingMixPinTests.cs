// SPDX-License-Identifier: MIT
// Audit #33 — pin heavily mixed camelCase/snake_case catalogs (no mass rename).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Documents the known heavily mixed JSON catalogs. Mass camel→snake
    /// migration remains deferred; this gate fails if a newly mixed catalog
    /// appears without being dispositioned, or if a pinned file disappears.
    /// </summary>
    public sealed class JsonNamingMixPinTests
    {
        /// <summary>Root catalogs that still meet <see cref="IsHeavilyMixed"/>.</summary>
        private static readonly string[] PinnedHeavilyMixedRelativePaths =
        {
            "items.json",
            "wildlife_trapping_catalog.json",
            "year_of_ash_radio.json",
            "wasteland_map_v1.json",
            "expeditions.json",
            "mineral_acid_synthesis_catalog.json",
            "chemical_syntheses.json",
            "holdfast_locations.json",
            "duty_roster_locations.json",
            "crossing_locations.json",
            "dive_sites.json",
            "verdict_items.json",
        };

        /// <summary>
        /// Known mixed catalogs below the heavy-mix bar (still tracked so they
        /// are not reclassified as "new" if they tip over the threshold later).
        /// </summary>
        private static readonly string[] PinnedBelowThresholdMixedRelativePaths =
        {
            "verdict_radio.json",
        };

        private static string DataRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("StreamingAssets/Data not found");
        }

        [Fact]
        public void PinnedMixedCatalogs_StillExist()
        {
            string root = DataRoot();
            var allPins = PinnedHeavilyMixedRelativePaths
                .Concat(PinnedBelowThresholdMixedRelativePaths);
            var missing = allPins
                .Where(rel => !File.Exists(Path.Combine(root, rel)))
                .ToList();
            Assert.True(missing.Count == 0,
                "Pinned mixed catalog missing (update pin list if intentionally removed):\n  "
                + string.Join("\n  ", missing));
        }

        [Fact]
        public void PinnedHeavilyMixedCatalogs_StillMeetMixFloor()
        {
            string root = DataRoot();
            var dropped = new List<string>();
            foreach (string rel in PinnedHeavilyMixedRelativePaths)
            {
                string text = File.ReadAllText(Path.Combine(root, rel));
                if (!IsHeavilyMixed(text)) dropped.Add(rel);
            }

            Assert.True(dropped.Count == 0,
                "Pinned heavily-mixed catalog no longer meets mix floor (move to below-threshold list or migrate):\n  "
                + string.Join("\n  ", dropped));
        }

        [Fact]
        public void BelowThresholdPins_RemainBelowHeavyMixBar()
        {
            string root = DataRoot();
            var tipped = new List<string>();
            foreach (string rel in PinnedBelowThresholdMixedRelativePaths)
            {
                string text = File.ReadAllText(Path.Combine(root, rel));
                if (IsHeavilyMixed(text)) tipped.Add(rel);
            }

            Assert.True(tipped.Count == 0,
                "Below-threshold pin now meets heavy mix bar (promote to heavily-mixed pin list):\n  "
                + string.Join("\n  ", tipped));
        }

        [Fact]
        public void NoNewHeavilyMixedRootCatalogs_OutsidePinList()
        {
            string root = DataRoot();
            var pinned = new HashSet<string>(
                PinnedHeavilyMixedRelativePaths.Concat(PinnedBelowThresholdMixedRelativePaths),
                StringComparer.Ordinal);
            var unexpected = new List<string>();

            foreach (string path in Directory.GetFiles(root, "*.json", SearchOption.TopDirectoryOnly))
            {
                string rel = Path.GetFileName(path);
                if (pinned.Contains(rel)) continue;
                if (!IsHeavilyMixed(File.ReadAllText(path))) continue;
                unexpected.Add(rel);
            }

            Assert.True(unexpected.Count == 0,
                "New heavily mixed root catalogs (add to pin list with disposition, or migrate to snake_case):\n  "
                + string.Join("\n  ", unexpected.OrderBy(s => s, StringComparer.Ordinal)));
        }

        private static bool IsHeavilyMixed(string text)
        {
            var keys = Regex.Matches(text, "\"([A-Za-z_][A-Za-z0-9_]*)\"\\s*:")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .ToList();
            int camel = keys.Count(k => Regex.IsMatch(k, "[a-z][A-Z]"));
            int snake = keys.Count(k => k.Contains('_') && k == k.ToLowerInvariant());
            return camel >= 20 && snake >= 5 && (camel + snake) > 40;
        }
    }
}
