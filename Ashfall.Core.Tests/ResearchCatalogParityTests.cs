// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 34 parity gate: proves the externalized research_knowledge.json catalog is
    /// value- and behavior-identical to the legacy hardcoded definitions for every node
    /// that existed in <c>ResearchSystem.RegisterDefaults()</c>. This is the evidence that
    /// authorized deleting the hardcoded production authority, and it remains as the
    /// regression tripwire against catalog drift for the original save-contract nodes.
    /// </summary>
    public sealed class ResearchCatalogParityTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        private static List<ResearchKnowledgeDef> LoadAuthoritativeCatalog()
        {
            var nodes = ResearchKnowledgeCatalogLoader.Load(
                ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotEmpty(nodes);
            return nodes;
        }

        private static ResearchSystem BuildFrom(IEnumerable<ResearchKnowledgeDef> defs)
        {
            var system = new ResearchSystem();
            foreach (var def in defs) system.Register(def);
            return system;
        }

        [Fact]
        public void LegacyFixture_Has31Defs_Original15First()
        {
            var legacy = ResearchLegacyCatalogFixture.CreateLegacyDefinitions();
            Assert.Equal(31, legacy.Count);
            for (int i = 0; i < ResearchLegacyCatalogFixture.Original15Ids.Length; i++)
                Assert.Equal(ResearchLegacyCatalogFixture.Original15Ids[i], legacy[i].id);
        }

        [Fact]
        public void Catalog_ContainsEveryLegacyDef_WithValueParity()
        {
            var catalog = LoadAuthoritativeCatalog().ToDictionary(d => d.id, d => d);
            foreach (var legacy in ResearchLegacyCatalogFixture.CreateLegacyDefinitions())
            {
                Assert.True(catalog.TryGetValue(legacy.id, out var json),
                    $"authoritative catalog is missing legacy node '{legacy.id}'");
                Assert.Equal(legacy.displayName, json.displayName);
                Assert.Equal(legacy.category, json.category);
                Assert.Equal(legacy.description, json.description);
                Assert.Equal(legacy.daysToComplete, json.daysToComplete);
                Assert.Equal(legacy.breakthroughItem, json.breakthroughItem);
                Assert.Equal(
                    (legacy.prerequisites ?? Array.Empty<string>()).OrderBy(x => x, StringComparer.Ordinal),
                    (json.prerequisites ?? Array.Empty<string>()).OrderBy(x => x, StringComparer.Ordinal));
            }
        }

        [Fact]
        public void Catalog_PreservesOriginal15RegistrationOrder()
        {
            var catalog = LoadAuthoritativeCatalog();
            for (int i = 0; i < ResearchLegacyCatalogFixture.Original15Ids.Length; i++)
                Assert.Equal(ResearchLegacyCatalogFixture.Original15Ids[i], catalog[i].id);
        }

        [Fact]
        public void Behavior_EligibilityAndCompletionMatchLegacyBaseline()
        {
            var legacySystem = BuildFrom(ResearchLegacyCatalogFixture.CreateLegacyDefinitions());
            var jsonSystem = BuildFrom(LoadAuthoritativeCatalog());

            // From-scratch eligibility: identical startable sets, identical rejections.
            var legacyStartable = legacySystem.Catalog.Keys
                .Where(id => LegacyShim(legacySystem, id)).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var jsonStartable = jsonSystem.Catalog.Keys
                .Where(id => LegacyShim(jsonSystem, id)).OrderBy(x => x, StringComparer.Ordinal).ToList();
            Assert.Equal(legacyStartable, jsonStartable);

            // Complete a representative root in both and compare the newly eligible sets,
            // completion day budget, and state projection.
            foreach (var root in new[] { "knowledge_water_basics", "knowledge_combat_training" })
            {
                var a = BuildFrom(ResearchLegacyCatalogFixture.CreateLegacyDefinitions());
                var b = BuildFrom(LoadAuthoritativeCatalog());
                Assert.True(a.StartResearch(root, 1));
                Assert.True(b.StartResearch(root, 1));
                a.Tick(60);
                b.Tick(60);
                Assert.Equal(a.Catalog[root].daysToComplete, b.Catalog[root].daysToComplete);
                Assert.Equal(
                    a.Catalog.Values.Where(d => d.isCompleted).Select(d => d.id).OrderBy(x => x, StringComparer.Ordinal),
                    b.Catalog.Values.Where(d => d.isCompleted).Select(d => d.id).OrderBy(x => x, StringComparer.Ordinal));
                Assert.Equal(a.State.activeResearchId, b.State.activeResearchId);
            }
        }

        /// <summary>Dry-run eligibility probe that mirrors StartResearch's prerequisite gate without mutating state.</summary>
        private static bool LegacyShim(ResearchSystem system, string id)
        {
            var def = system.GetKnowledge(id);
            if (def == null || def.isCompleted) return false;
            if (def.prerequisites != null)
            {
                foreach (var prereq in def.prerequisites)
                {
                    var p = system.GetKnowledge(prereq);
                    if (p == null || !p.isCompleted) return false;
                }
            }
            return true;
        }
    }
}
