// SPDX-License-Identifier: MIT
// ============================================================================
// Tasks 5–8 Wave C — Task 5 explicit closure (§6.5–§6.10).
// Five explicit per-manual tests over the REAL authored data, each asserting:
// manual acquired → target node revealed → node NOT completed → unrelated
// nodes unchanged. Plus per-manual idempotence (§6.6), save/load round trip
// with reacquire-no-repeat (§6.7), and the research panel observable event
// path (§6.8 — the panel subscribes to shared research state, not to
// collectibles directly).
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    public sealed class CollectibleResearchIntegrationTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = AppContext.BaseDirectory;
            while (dir != null)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(candidate, "collectibles.json"))) return candidate;
                dir = Path.GetDirectoryName(dir);
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static CollectibleCatalog LoadCatalog()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog);
            return catalog!;
        }

        private static Ashfall.Core.ResearchKnowledgeDef Knowledge(string id) => new Ashfall.Core.ResearchKnowledgeDef
        {
            id = id,
            displayName = id,
            daysToComplete = 4
        };

        private static (CollectibleCatalog catalog, List<string> targets, Dictionary<string, string> itemByTarget) LoadManuals()
        {
            var catalog = LoadCatalog();
            var itemByTarget = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in catalog.ByItemId)
            {
                if (kv.Value.effect_type == "knowledge" && !string.IsNullOrEmpty(kv.Value.effect_target))
                    itemByTarget[kv.Value.effect_target!] = kv.Key;
            }
            return (catalog, itemByTarget.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList(), itemByTarget);
        }

        private static (CollectibleEffectDispatcher dispatcher, ResearchSystem research, CollectibleDiscoveryState discovery)
            BuildDispatcher(CollectibleCatalog catalog, List<string> targets)
        {
            var research = new ResearchSystem();
            foreach (var t in targets) research.Register(Knowledge(t));
            var discovery = new CollectibleDiscoveryState();
            var dispatcher = new CollectibleEffectDispatcher(
                catalog, discovery, researchProvider: () => research, dayProvider: () => 5);
            return (dispatcher, research, discovery);
        }

        // ── §6.5: five explicit per-manual tests (data-derived) ─────────

        private static void AssertManualWiresKnowledge(string knowledgeTarget)
        {
            var (catalog, targets, itemByTarget) = LoadManuals();
            Assert.Contains(knowledgeTarget, targets);   // authored data confirms the target

            var (dispatcher, research, _) = BuildDispatcher(catalog, targets);
            var result = dispatcher.DispatchOnAcquire(itemByTarget[knowledgeTarget]);

            Assert.True(result.EffectApplied, result.FailureReason);
            Assert.True(result.DiscoveryRegistered);
            Assert.True(research.IsManualUnlocked(knowledgeTarget));          // revealed
            Assert.DoesNotContain(knowledgeTarget, research.State.completedIds);  // never completed
            Assert.Null(research.GetActiveResearch());                        // no research started
            foreach (var other in targets.Where(t => t != knowledgeTarget))
                Assert.False(research.IsManualUnlocked(other));               // unrelated unchanged
        }

        [Fact]
        public void DieselServiceManual_RevealsKnowledge_DoesNotComplete()
            => AssertManualWiresKnowledge("knowledge_diesel_mechanics");

        [Fact]
        public void RadioRepairGuide_RevealsKnowledge_DoesNotComplete()
            => AssertManualWiresKnowledge("knowledge_radio_repair");

        [Fact]
        public void WaterTreatmentHandbook_RevealsKnowledge_DoesNotComplete()
            => AssertManualWiresKnowledge("knowledge_water_treatment");

        [Fact]
        public void AirFilterManual_RevealsKnowledge_DoesNotComplete()
            => AssertManualWiresKnowledge("knowledge_air_filtration");

        [Fact]
        public void DosimeterGuide_RevealsKnowledge_DoesNotComplete()
            => AssertManualWiresKnowledge("knowledge_radiation_measurement");

        // ── §6.6: reacquisition idempotence ────────────────────────────

        [Fact]
        public void ManualReacquisition_Idempotent_NodeStillNotCompleted()
        {
            var (catalog, targets, itemByTarget) = LoadManuals();
            var (dispatcher, research, discovery) = BuildDispatcher(catalog, targets);
            var itemId = itemByTarget["knowledge_diesel_mechanics"];
            int events = 0;
            dispatcher.OnCollectibleDiscovered += _ => events++;

            dispatcher.DispatchOnAcquire(itemByTarget["knowledge_diesel_mechanics"]);
            dispatcher.DispatchOnAcquire(itemByTarget["knowledge_radio_repair"]);
            Assert.Equal(2, events);

            var again = dispatcher.DispatchOnAcquire(itemId);
            Assert.True(again.AlreadyDiscovered);
            Assert.Equal(2, events);
            Assert.DoesNotContain("knowledge_diesel_mechanics", research.State.completedIds);
            Assert.Equal(targets.Count(t => t == "knowledge_diesel_mechanics"), research.State.unlockedIds.Count(id => id == "knowledge_diesel_mechanics"));
        }

        // ── §6.7: save/load round trip ─────────────────────────────────

        [Fact]
        public void ManualAcquisition_SaveRestore_NodeStillRevealed_ReacquireNoRepeat()
        {
            var (catalog, targets, itemByTarget) = LoadManuals();
            var (dispatcher, research, discovery) = BuildDispatcher(catalog, targets);
            const string manual = "item_collectible_diesel_service_manual";

            var first = dispatcher.DispatchOnAcquire(manual);
            Assert.True(first.EffectApplied);
            Assert.True(research.IsManualUnlocked("knowledge_diesel_mechanics"));

            // Save/restore all three stores the host persists.
            var researchState = research.CaptureState();
            var discoveryState = discovery.CaptureState();

            var research2 = new ResearchSystem();
            foreach (var t in targets) research2.Register(Knowledge(t));
            research2.RestoreState(researchState);
            var discovery2 = new CollectibleDiscoveryState();
            discovery2.RestoreState(discoveryState);
            var dispatcher2 = new CollectibleEffectDispatcher(
                catalog, discovery2, researchProvider: () => research2, dayProvider: () => 9);

            Assert.True(research2.IsManualUnlocked("knowledge_diesel_mechanics"));  // node still revealed
            Assert.True(discovery2.IsDiscovered(manual));                          // collectible still discovered
            Assert.DoesNotContain("knowledge_diesel_mechanics", research2.State.completedIds);

            // Reacquire after restore: no repeat effect, no duplicate discovery.
            var second = dispatcher2.DispatchOnAcquire(manual);
            Assert.True(second.AlreadyDiscovered);
            Assert.False(second.DiscoveryRegistered);
            Assert.Equal(1, research2.State.unlockedIds.Count(id => id == "knowledge_diesel_mechanics"));
        }

        // ── §6.8: research panel observable (state event, not coupling) ─

        [Fact]
        public void ManualReveal_RaisesResearchStateEvent_PanelObservable()
        {
            var (catalog, targets, itemByTarget) = LoadManuals();
            var (dispatcher, research, _) = BuildDispatcher(catalog, targets);

            int unlockEvents = 0;
            research.OnManualUnlocked += _ => unlockEvents++;

            dispatcher.DispatchOnAcquire(itemByTarget["knowledge_diesel_mechanics"]);
            Assert.Equal(1, unlockEvents);  // panels subscribing to research state see the change
        }
    }
}
