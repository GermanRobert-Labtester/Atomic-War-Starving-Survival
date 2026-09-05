using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Collectible effect-target resolution gate (flagship data-integrity
    /// expansion): every authored collectible effect target must resolve
    /// against its canonical authority — knowledge targets against the
    /// research catalog, location_clue targets against the wasteland map,
    /// journal/faction keys against the free-form codex (non-empty by the
    /// catalog whitelist), and morale/none requiring none.
    /// </summary>
    public class CollectibleEffectTargetResolutionTests
    {
        private static readonly string DataDir = FindDataDir();
        private static readonly IFileIO FileIO = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static CollectibleCatalog LoadCollectibles() =>
            CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer)
                ?? throw new InvalidOperationException("collectibles.json must load");

        private static ResearchSystem LoadResearch()
        {
            var engine = new ResearchSystem();
            int count = ResearchKnowledgeCatalogLoader.LoadAndRegister(engine, DataDir, FileIO, Serializer);
            Assert.True(count > 0, "research_knowledge.json must register nodes");
            return engine;
        }

        private static HashSet<string> LoadMapNodeIds()
        {
            string raw = FileIO.ReadAllText(Path.Combine(DataDir, "wasteland_map_v1.json"));
            var json = JsonDocument.Parse(raw);
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var node in json.RootElement.GetProperty("nodes").EnumerateArray())
            {
                if (node.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String)
                    ids.Add(id.GetString()!);
            }
            return ids;
        }

        [Fact]
        public void KnowledgeTargets_ResolveAgainstResearchCatalog()
        {
            var research = LoadResearch();
            var broken = LoadCollectibles().ByItemId.Values
                .Where(d => d.effect_type == "knowledge")
                .Where(d => !research.IsManualUnlocked(d.effect_target) &&
                            !research.Catalog.ContainsKey(d.effect_target))
                .Select(d => $"{d.item_id} -> {d.effect_target}")
                .ToList();
            Assert.True(broken.Count == 0,
                "authored knowledge targets must exist in research_knowledge.json:\n" + string.Join("\n", broken));
        }

        [Fact]
        public void LocationClueTargets_ResolveAgainstWastelandMap()
        {
            var nodes = LoadMapNodeIds();
            var broken = LoadCollectibles().ByItemId.Values
                .Where(d => d.effect_type == "location_clue")
                .Where(d => !nodes.Contains(d.effect_target))
                .Select(d => $"{d.item_id} -> {d.effect_target}")
                .ToList();
            Assert.True(broken.Count == 0,
                "authored location_clue targets must exist in wasteland_map_v1.json:\n" + string.Join("\n", broken));
        }

        [Fact]
        public void JournalAndFactionTargets_AreNonEmptyCodexKeys()
        {
            var broken = LoadCollectibles().ByItemId.Values
                .Where(d => d.effect_type == "journal_unlock" || d.effect_type == "faction_info")
                .Where(d => string.IsNullOrWhiteSpace(d.effect_target))
                .Select(d => d.item_id)
                .ToList();
            Assert.Empty(broken); // codex authority is free-form: key resolves by construction
        }

        [Fact]
        public void MoraleAndNoneTargets_CarryNoTarget()
        {
            var offenders = LoadCollectibles().ByItemId.Values
                .Where(d => (d.effect_type == "none" || d.effect_type == "morale") &&
                            !string.IsNullOrEmpty(d.effect_target))
                .Select(d => $"{d.item_id} ({d.effect_type})")
                .ToList();
            Assert.Empty(offenders);
        }

        [Fact]
        public void ClueRevealNodes_AreTravelConnected()
        {
            // The three clue-revealed map markers must sit on the route graph
            // (a revealed location the shelter cannot reach would be dead UI).
            string raw = FileIO.ReadAllText(Path.Combine(DataDir, "wasteland_map_v1.json"));
            var json = JsonDocument.Parse(raw);
            var endpoints = new HashSet<string>(StringComparer.Ordinal);
            foreach (var route in json.RootElement.GetProperty("routes").EnumerateArray())
            {
                endpoints.Add(route.GetProperty("from").GetString()!);
                endpoints.Add(route.GetProperty("to").GetString()!);
            }

            var clueNodeIds = LoadCollectibles().ByItemId.Values
                .Where(d => d.effect_type == "location_clue")
                .Select(d => d.effect_target)
                .Distinct()
                .ToList();
            var isolated = clueNodeIds.Where(id => !endpoints.Contains(id)).ToList();
            Assert.Empty(isolated);
        }
    }
}
