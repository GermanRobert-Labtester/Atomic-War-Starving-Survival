// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    /// <summary>
    /// Core-contract tests for the authored guilt-source table bound to the live
    /// guilt authority. GuiltInsomniaSystem stays the sole guilt owner; the catalog
    /// supplies severity and description so no caller passes a literal.
    /// </summary>
    public sealed class PlanGuiltSourceCatalogTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "Assets", "Ashfall.Core")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static GuiltSourceCatalog Loaded() =>
            GuiltSourceCatalog.LoadFromDirectory(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data"));

        [Fact]
        public void AuthoredCatalogLoads()
        {
            var catalog = Loaded();
            Assert.True(catalog.Count > 0);
            foreach (var def in catalog.Items)
            {
                Assert.False(string.IsNullOrEmpty(def.ChoicePattern));
                Assert.True(def.Severity > 0f && def.Severity <= 1f,
                    $"severity for '{def.ChoicePattern}' must sit in (0,1], was {def.Severity}");
            }
        }

        [Fact]
        public void ResolutionIsStableAndReadOnly()
        {
            var catalog = Loaded();
            var first = catalog.Items[0];

            Assert.Same(catalog.GetByPattern(first.ChoicePattern), catalog.GetByPattern(first.ChoicePattern));
            Assert.Equal(first.Severity, catalog.GetByPattern(first.ChoicePattern).Severity);
        }

        [Fact]
        public void UnknownPatternResolvesToNothingRatherThanAGuess()
        {
            var catalog = Loaded();
            Assert.Null(catalog.GetByPattern("pattern_nobody_authored"));
            Assert.False(catalog.TryGetSeverity("pattern_nobody_authored", out float severity));
            Assert.Equal(0f, severity);
        }

        [Fact]
        public void SeverityMatchesTheAuthoredJsonExactly()
        {
            string json = File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "guilt_sources.json"));
            var authored = new Dictionary<string, float>(StringComparer.Ordinal);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
                authored[item.GetProperty("choice_pattern").GetString()] = item.GetProperty("severity").GetSingle();

            var catalog = Loaded();
            Assert.Equal(authored.Count, catalog.Count);
            foreach (var kv in authored)
            {
                Assert.True(catalog.TryGetSeverity(kv.Key, out float got), $"missing '{kv.Key}'");
                Assert.Equal(kv.Value, got, 4);
            }
        }

        [Fact]
        public void DescriptionTemplatesTheSurvivorName()
        {
            var catalog = Loaded();
            bool anyTemplated = false;
            foreach (var def in catalog.Items)
            {
                if (!def.Description.Contains("{name}", StringComparison.Ordinal)) continue;
                anyTemplated = true;
                string text = def.FormatDescription("Maren");
                Assert.Contains("Maren", text, StringComparison.Ordinal);
                Assert.DoesNotContain("{name}", text, StringComparison.Ordinal);
            }
            Assert.True(anyTemplated, "the authored table is expected to carry {name} templates");
        }

        [Fact]
        public void TheExistingGuiltOwnerRecordsTheAuthoredSeverity()
        {
            var catalog = Loaded();
            var def = catalog.Items[0];
            var guilt = new GuiltInsomniaSystem();

            guilt.RecordGuilt("surv_a", def.ChoicePattern, def.Severity, 3);

            var state = guilt.CaptureState();
            float recorded = -1f;
            foreach (var s in state.survivors)
                foreach (var rec in s.guiltSources)
                    if (rec.sourceId == def.ChoicePattern) recorded = rec.severity;

            Assert.Equal(def.Severity, recorded, 4);
            Assert.Equal(1, guilt.GetGuiltSourceCount("surv_a"));
            Assert.True(guilt.GetInsomniaSeverity("surv_a") > 0f);
        }

        [Fact]
        public void GuiltConsequenceThresholdsRemainEngineAuthored()
        {
            // The catalog supplies severity; the escalation thresholds stay with the
            // guilt authority and must not be re-decided by the data table.
            Assert.Equal(0.7f, GuiltInsomniaSystem.HighSeverityThreshold);
            Assert.Equal(0.05f, GuiltInsomniaSystem.NaturalDecayPerDay);
            Assert.Equal(30, GuiltInsomniaSystem.GuiltExpiryDays);
        }
    }
}
