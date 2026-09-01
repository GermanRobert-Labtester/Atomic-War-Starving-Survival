// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    public sealed class ResearchKnowledgeCatalogLoaderTests
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

        [Fact]
        public void Load_Loads56NodesFromCatalog()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var nodes = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotEmpty(nodes);
            Assert.True(nodes.Count >= 56, $"Expected >= 56 nodes, found {nodes.Count}");
        }

        [Fact]
        public void Load_AllNodesHaveValidIdAndDisplayName()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var nodes = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, json);
            foreach (var node in nodes)
            {
                Assert.False(string.IsNullOrWhiteSpace(node.id));
                Assert.StartsWith("knowledge_", node.id);
                Assert.False(string.IsNullOrWhiteSpace(node.displayName));
                Assert.False(string.IsNullOrWhiteSpace(node.category));
                Assert.True(node.daysToComplete > 0);
            }
        }

        [Fact]
        public void ValidateDag_SucceedsOnAuthoritativeCatalog()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var nodes = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, json);
            bool ok = ResearchKnowledgeCatalogLoader.ValidateDag(nodes, out string error);
            Assert.True(ok, $"DAG validation failed: {error}");
            Assert.Empty(error);
        }

        [Fact]
        public void ValidateDag_DetectsDirectCycle()
        {
            var nodes = new List<ResearchKnowledgeDef>
            {
                new ResearchKnowledgeDef("knowledge_a", "A", "survival", "A", 5, new[] { "knowledge_b" }),
                new ResearchKnowledgeDef("knowledge_b", "B", "survival", "B", 5, new[] { "knowledge_a" })
            };

            bool ok = ResearchKnowledgeCatalogLoader.ValidateDag(nodes, out string error);
            Assert.False(ok);
            Assert.Contains("Cycle detected", error);
        }

        [Fact]
        public void ValidateDag_DetectsMissingPrerequisite()
        {
            var nodes = new List<ResearchKnowledgeDef>
            {
                new ResearchKnowledgeDef("knowledge_a", "A", "survival", "A", 5, new[] { "knowledge_missing" })
            };

            bool ok = ResearchKnowledgeCatalogLoader.ValidateDag(nodes, out string error);
            Assert.False(ok);
            Assert.Contains("unresolved prerequisite", error);
        }

        [Fact]
        public void LoadAndRegister_PopulatesResearchSystemCatalog()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new ResearchSystem();

            int count = ResearchKnowledgeCatalogLoader.LoadAndRegister(system, dataDir, fileIO, json);
            Assert.True(count >= 56);
            Assert.Equal(count, system.CatalogCount);
            Assert.NotNull(system.GetKnowledge("knowledge_water_basics"));
            Assert.NotNull(system.GetKnowledge("knowledge_water_advanced"));
        }
    }
}
