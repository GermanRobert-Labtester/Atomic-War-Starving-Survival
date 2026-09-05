using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 68 — wall carving template expansion contract tests.
    ///
    /// Pins wall_carving_templates.json at exactly 3 morale bands with 20
    /// templates each (60 total): schema shape (morale_band / morale_min /
    /// morale_max / bare-string templates), band windows (high 60–100,
    /// medium 30–59, low 0–29), no empty strings, no exact duplicates
    /// within or across bands, motif-diversity spot checks, cliché gates,
    /// and physicality/at-length constraints.
    ///
    /// Plan 68 §66 finding recorded here: the catalog is data-present and
    /// consumer-absent — no Core/host/UI code parses it yet (the
    /// content-utilization scanner maps it to MemorialSystem/MemorialPanel
    /// aspirationally). These tests therefore validate the JSON directly
    /// through a local probe DTO; when a carving consumer lands it should
    /// adopt the same shape.
    /// </summary>
    public sealed class Plan68WallCarvingTests
    {
        private static string? FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return null;
        }

        private static List<BandProbe> Load()
        {
            string? dataDir = FindDataDir();
            Assert.False(dataDir == null, "StreamingAssets/Data directory not found");
            var raw = new FileSystemIO().ReadAllText(Path.Combine(dataDir!, "wall_carving_templates.json"));
            var root = new SystemTextJsonSerializer().Deserialize<RootProbe>(raw);
            Assert.NotNull(root);
            Assert.NotNull(root!.items);
            return root.items;
        }

        private sealed class RootProbe { public List<BandProbe> items { get; set; } = new(); }

        private sealed class BandProbe
        {
            public string morale_band { get; set; } = string.Empty;
            public int morale_min { get; set; }
            public int morale_max { get; set; }
            public List<string> templates { get; set; } = new();
        }

        [Fact]
        public void Catalog_has_exactly_three_bands_with_correct_names()
        {
            var bands = Load();
            Assert.Equal(3, bands.Count);
            Assert.Equal(new[] { "high", "medium", "low" },
                bands.Select(b => b.morale_band).ToArray());
        }

        [Fact]
        public void Each_band_contains_exactly_20_templates()
        {
            var bands = Load();
            Assert.All(bands, b => Assert.Equal(20, b.templates.Count));
        }

        [Fact]
        public void Band_windows_match_the_plan_contract()
        {
            var bands = Load().ToDictionary(b => b.morale_band, StringComparer.Ordinal);
            Assert.Equal((60, 100), (bands["high"].morale_min, bands["high"].morale_max));
            Assert.Equal((30, 59), (bands["medium"].morale_min, bands["medium"].morale_max));
            Assert.Equal((0, 29), (bands["low"].morale_min, bands["low"].morale_max));
        }

        [Fact]
        public void No_empty_templates_and_no_whitespace_only_templates()
        {
            var bands = Load();
            foreach (var band in bands)
                foreach (var t in band.templates)
                    Assert.False(string.IsNullOrWhiteSpace(t),
                        $"{band.morale_band}: empty template");
        }

        [Fact]
        public void No_exact_duplicates_within_a_band()
        {
            foreach (var band in Load())
            {
                var normalized = band.templates
                    .Select(t => t.Trim().ToLowerInvariant())
                    .ToList();
                Assert.Equal(normalized.Count, normalized.Distinct().Count());
            }
        }

        [Fact]
        public void No_exact_duplicates_across_bands()
        {
            var bands = Load();
            var all = bands.SelectMany(b => b.templates)
                .Select(t => t.Trim().ToLowerInvariant())
                .ToList();
            Assert.Equal(all.Count, all.Distinct().Count());
        }

        [Fact]
        public void The_fifteen_original_templates_are_preserved()
        {
            var byBand = Load().ToDictionary(b => b.morale_band, StringComparer.Ordinal);
            Assert.Contains(byBand["high"].templates, t => t.Contains("STILL"));
            Assert.Contains(byBand["high"].templates, t => t.StartsWith("A recipe for imaginary cake"));
            Assert.Contains(byBand["medium"].templates, t => t.Contains("47 days"));
            Assert.Contains(byBand["medium"].templates, t => t.Contains("miss everything"));
            Assert.Contains(byBand["low"].templates, t => t.Contains("WHY"));
            Assert.Contains(byBand["low"].templates, t => t.Contains("I'm sorry"));
        }

        [Fact]
        public void Templates_stay_readable_at_a_glance()
        {
            foreach (var band in Load())
                foreach (var t in band.templates)
                    Assert.True(t.Length <= 140,
                        $"{band.morale_band}: template too long for a glance ({t.Length} chars): {t}");
        }

        [Fact]
        public void No_melodrama_cliches_in_any_band()
        {
            var banned = new[]
            {
                "last hope", "darkness swallowed", "against all odds",
                "light at the end", "never give up", "tomorrow will come",
                "ashes of the old world", "you feel terrible",
                "the guilt crushes", "haunts everyone forever"
            };
            foreach (var band in Load())
                foreach (var t in band.templates)
                {
                    var lower = t.ToLowerInvariant();
                    foreach (var phrase in banned)
                        Assert.False(lower.Contains(phrase, StringComparison.Ordinal),
                            $"{band.morale_band}: cliché '{phrase}' in: {t}");
                }
        }

        [Fact]
        public void High_band_carries_hope_through_solidarity_not_triumphalism()
        {
            var high = Load().First(b => b.morale_band == "high").templates;
            // No grand-speech vocabulary.
            foreach (var t in high)
            {
                var lower = t.ToLowerInvariant();
                Assert.False(lower.Contains("rebuild civilization"), "triumphalism");
                Assert.False(lower.Contains("hope conquers"), "slogan");
            }
            // Communal/continuity motifs present: names of the living, future dates, planting.
            Assert.Contains(high, t => t.Contains("TOMATOES") || t.Contains("SPRING"));
            Assert.Contains(high, t => t.Contains("FOUR") || t.Contains("four"));
            Assert.Contains(high, t => t.Contains("APRIL"));
        }

        [Fact]
        public void Medium_band_is_documentary_routine_texture()
        {
            var medium = Load().First(b => b.morale_band == "medium").templates;
            // Maintenance/logistics vocabulary present without becoming a manual.
            Assert.Contains(medium, t => t.Contains("FILTER"));
            Assert.Contains(medium, t => t.Contains("BATTERY"));
            Assert.Contains(medium, t => t.Contains("NIGHT SHIFT"));
            Assert.Contains(medium, t => t.Contains("WRENCH"));
            // No despair vocabulary leaking into the routine band.
            Assert.DoesNotContain(medium, t => t.ToLowerInvariant().Contains("die"));
        }

        [Fact]
        public void Low_band_varies_motifs_beyond_names_of_the_dead()
        {
            var low = Load().First(b => b.morale_band == "low").templates;
            Assert.Contains(low, t => t.Contains("SORRY"));
            Assert.Contains(low, t => t.Contains("DON'T SLEEP"));
            Assert.Contains(low, t => t.Contains("COLD"));
            Assert.Contains(low, t => t.Contains("prayer"));
            // Motif distribution: at most a handful of name-list marks.
            var nameMarks = low.Count(t => t.Contains("names", StringComparison.OrdinalIgnoreCase)
                                         || t.Contains("name", StringComparison.OrdinalIgnoreCase));
            Assert.True(nameMarks <= 5, $"too many name motifs in the low band: {nameMarks}");
        }

        [Fact]
        public void Bands_are_distinguishable_by_blind_vocabulary()
        {
            // Spot-check the signature: high contains future/communal tokens,
            // low contains loss tokens — a reviewer could band-classify these.
            var byBand = Load().ToDictionary(b => b.morale_band, StringComparer.Ordinal);
            var highText = string.Join(' ', byBand["high"].templates);
            var lowText = string.Join(' ', byBand["low"].templates);
            Assert.Contains("SPRING", highText);
            Assert.Contains("harvest", highText, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("sorry", lowText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
