// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Muster;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class FactionCultureCodexTests
    {
        private static string RepositoryRoot
        {
            get
            {
                var directory = new DirectoryInfo(AppContext.BaseDirectory);
                while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md")))
                {
                    directory = directory.Parent;
                }

                return directory?.FullName ?? throw new DirectoryNotFoundException("ASHFALL repository root was not found");
            }
        }

        [Fact]
        public void MusterFactionCultureCatalog_LoadsAllAuthoredEntries()
        {
            string dataDir = Path.Combine(RepositoryRoot, "Assets", "StreamingAssets", "Data");
            var entries = FactionCultureCatalogLoader.LoadEntries(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.NotNull(entries);
            Assert.True(entries.Count >= 20, $"Expected >= 20 authored culture entries, got {entries.Count}");

            foreach (var entry in entries)
            {
                Assert.False(string.IsNullOrWhiteSpace(entry.id), "Culture entry must have non-empty id");
                Assert.False(string.IsNullOrWhiteSpace(entry.factionId), $"Culture entry '{entry.id}' must have non-empty factionId");
                Assert.False(string.IsNullOrWhiteSpace(entry.title), $"Culture entry '{entry.id}' must have non-empty title");
                Assert.False(string.IsNullOrWhiteSpace(entry.body), $"Culture entry '{entry.id}' must have non-empty body text");
            }
        }

        [Fact]
        public void FactionCultureCatalog_ContainsCanonicalFactions()
        {
            string dataDir = Path.Combine(RepositoryRoot, "Assets", "StreamingAssets", "Data");
            var entries = FactionCultureCatalogLoader.LoadEntries(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var factions = entries.Select(e => e.factionId).ToHashSet(StringComparer.OrdinalIgnoreCase);

            Assert.Contains("faction_scavenger_guild", factions);
            Assert.Contains("faction_hydro_barons", factions);
            Assert.Contains("faction_iron_raiders", factions);
            Assert.Contains("faction_deserter_coalition", factions);
        }

        [Fact]
        public void FactionCultureEntries_HaveUniqueIds()
        {
            string dataDir = Path.Combine(RepositoryRoot, "Assets", "StreamingAssets", "Data");
            var entries = FactionCultureCatalogLoader.LoadEntries(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var duplicateIds = entries
                .GroupBy(e => e.id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            Assert.Empty(duplicateIds);
        }

        [Fact]
        public void PanelRegistry_FactionCultureCodexRouteIsRegistered()
        {
            PanelRegistryBootstrap.RegisterAll();

            Assert.True(PanelRegistry.IsRegistered("faction_culture_codex"),
                "faction_culture_codex route must be registered in PanelRegistry.");

            var desc = PanelRegistry.Get("faction_culture_codex");
            Assert.NotNull(desc);
            Assert.Equal("Faction Culture Codex", desc!.DisplayName);
            Assert.Equal(PanelGroup.Secondary, desc.Group);
            Assert.Contains("factions", desc.SetupDependencies);
            Assert.Contains("muster", desc.SetupDependencies);
        }
    }
}
