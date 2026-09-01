// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 34 integration contract: research progress is JSON-authoritative,
    /// save-safe (ID-based round-trip incl. unknown-node policy), and grants
    /// are transition-only (the completion event never fires on restore).
    /// </summary>
    public sealed class ResearchSaveIntegrationTests
    {
        private static ResearchSystem BuildCatalogEngine(ResearchState? state = null)
        {
            var engine = new ResearchSystem(log: new NullLog(), state: state);
            ResearchLegacyCatalogFixture.LoadAuthoritativeCatalogInto(engine);
            return engine;
        }

        [Fact]
        public void CompletedEvent_FiresOncePerNode_OnTransitionOnly()
        {
            var engine = BuildCatalogEngine();
            int fired = 0;
            string? lastId = null;
            engine.OnResearchCompleted += def => { fired++; lastId = def.id; };

            engine.StartResearch("knowledge_water_basics", 1);
            engine.Tick(10);
            Assert.Equal(1, fired);
            Assert.Equal("knowledge_water_basics", lastId);

            // Idempotent completion: no duplicate event, no duplicate grant.
            Assert.False(engine.CompleteResearch("knowledge_water_basics"));
            Assert.Equal(1, fired);

            // Force-completing a different node fires once for that node.
            Assert.True(engine.CompleteResearch("knowledge_solar_basics"));
            Assert.Equal(2, fired);
            Assert.Equal("knowledge_solar_basics", lastId);
        }

        [Fact]
        public void RestoreState_DoesNotFireCompletedEvent_ButRestoresFlags()
        {
            var engine = BuildCatalogEngine();
            engine.StartResearch("knowledge_water_basics", 1);
            engine.Tick(10);
            var saved = engine.CaptureState();

            var restored = BuildCatalogEngine(saved);
            int fired = 0;
            restored.OnResearchCompleted += _ => fired++;
            restored.RestoreState(saved);

            Assert.Equal(0, fired);
            Assert.True(restored.GetKnowledge("knowledge_water_basics")!.isCompleted);
        }

        [Fact]
        public void CaptureState_PreservesUnknownNodeIds()
        {
            // §34D.5 unknown-node policy: a save referencing nodes absent from
            // the loaded catalog must never silently lose them.
            var saved = new ResearchState
            {
                unlockedIds = { "knowledge_water_basics", "knowledge_removed_in_future", "res_legacy_phantom" },
                completedIds = { "knowledge_solar_basics", "knowledge_removed_in_future" },
                activeResearchId = "knowledge_radio_basics",
                activeResearchDays = 2,
                currentDay = 9,
            };
            var engine = BuildCatalogEngine(saved);

            var captured = engine.CaptureState();
            Assert.Contains("knowledge_water_basics", captured.unlockedIds);
            Assert.Contains("knowledge_removed_in_future", captured.unlockedIds);
            Assert.Contains("res_legacy_phantom", captured.unlockedIds);
            Assert.Contains("knowledge_solar_basics", captured.completedIds);
            Assert.Contains("knowledge_removed_in_future", captured.completedIds);
            Assert.DoesNotContain("knowledge_water_basics", captured.completedIds);
            Assert.Equal("knowledge_radio_basics", captured.activeResearchId);
        }

        [Fact]
        public void SaveRoundTrip_PreservesProgressAcrossFreshEngine()
        {
            var engine = BuildCatalogEngine();
            engine.StartResearch("knowledge_water_basics", 1);
            engine.Tick(10); // completed
            engine.StartResearch("knowledge_radio_basics", 10);
            engine.Tick(13); // 3 days in

            var saved = engine.CaptureState();
            string encoded = SchemaVersionedEnvelope<ResearchState>.Encode(
                saved, new SystemTextJsonSerializer());

            var decoded = SchemaVersionedEnvelope<ResearchState>.Decode(
                encoded, new SystemTextJsonSerializer());
            Assert.NotNull(decoded);
            var fresh = BuildCatalogEngine(decoded);

            Assert.True(fresh.GetKnowledge("knowledge_water_basics")!.isCompleted);
            Assert.Equal("knowledge_radio_basics", fresh.State.activeResearchId);
            Assert.Equal(3, fresh.State.activeResearchDays);

            // Resumed node finishes on schedule after the round-trip.
            fresh.Tick(13 + fresh.GetKnowledge("knowledge_radio_basics")!.daysToComplete);
            Assert.True(fresh.GetKnowledge("knowledge_radio_basics")!.isCompleted);
            Assert.Null(fresh.GetActiveResearch());
        }

        [Fact]
        public void LoadAndRegister_MissingCatalog_RegistersNothing_NoFallback()
        {
            // §1.10: a missing catalog must never resurrect hardcoded defaults.
            string emptyDir = Path.Combine(Path.GetTempPath(), "ashfall-research-selftest-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(emptyDir);
            try
            {
                var engine = new ResearchSystem();
                int count = ResearchKnowledgeCatalogLoader.LoadAndRegister(
                    engine, emptyDir, new FileSystemIO(), new SystemTextJsonSerializer());
                Assert.Equal(0, count);
                Assert.Equal(0, engine.CatalogCount);
            }
            finally
            {
                Directory.Delete(emptyDir, recursive: true);
            }
        }

        [Fact]
        public void Catalog_BreakthroughItemsResolve_AcrossAuthoritativeItemIds()
        {
            // §34C.3 Tier-2: every authored breakthrough_item must exist in the
            // item authority — an orphan unlock target is an integrity failure.
            var itemIds = CatalogIntegrityTestHarness.AuthoredItemIds();
            var engine = BuildCatalogEngine();
            foreach (var def in engine.Catalog.Values)
            {
                if (string.IsNullOrEmpty(def.breakthroughItem)) continue;
                Assert.True(itemIds.Contains(def.breakthroughItem),
                    $"node '{def.id}' references unknown breakthrough item '{def.breakthroughItem}'");
            }
        }
    }

    /// <summary>Minimal item-id universe pulled from the JSON data authority for reference checks.</summary>
    internal static class CatalogIntegrityTestHarness
    {
        public static System.Collections.Generic.HashSet<string> AuthoredItemIds()
        {
            var ids = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            foreach (var file in Directory.GetFiles(ResolveDataDir(), "*.json", SearchOption.AllDirectories))
            {
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(file));
                    Walk(doc.RootElement, ids);
                }
                catch (System.Text.Json.JsonException)
                {
                    // Non-JSON-shaped data files are other catalogs' concern.
                }
            }
            return ids;
        }

        private static void Walk(System.Text.Json.JsonElement element, System.Collections.Generic.HashSet<string> ids)
        {
            switch (element.ValueKind)
            {
                case System.Text.Json.JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == System.Text.Json.JsonValueKind.String
                            && prop.Value.GetString() is string s
                            && s.StartsWith("item_", StringComparison.Ordinal))
                        {
                            ids.Add(s);
                        }
                        else
                        {
                            Walk(prop.Value, ids);
                        }
                    }
                    break;
                case System.Text.Json.JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                        Walk(item, ids);
                    break;
            }
        }

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
    }
}
