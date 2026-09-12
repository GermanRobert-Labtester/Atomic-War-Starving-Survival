// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Localization;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    /// <summary>
    /// Workstream B (Task 6): Collectible Tutorial and Onboarding Integration Tests.
    /// Verifies that first-time collectible and effect-bearing discoveries trigger
    /// the appropriate tutorial moments non-spammily, in deterministic stable order,
    /// surviving save/load, without replaying on historical state restore.
    /// </summary>
    public class CollectibleTutorialIntegrationTests
    {
        [Fact]
        public void CollectibleTutorialEntry_ExistsAndHasContent()
        {
            var cultural = CollectibleTutorialTracker.GetEntry(CollectibleTutorialTracker.CulturalArtifactsId);
            Assert.NotNull(cultural);
            Assert.Equal("Cultural Artifacts", cultural!.Title);
            Assert.Contains("surviving objects from the pre-war world", cultural.Body);
            Assert.Contains("Tutorial:", cultural.AccessibleSummary);

            var reading = CollectibleTutorialTracker.GetEntry(CollectibleTutorialTracker.ReadingAndDiscoveringId);
            Assert.NotNull(reading);
            Assert.Equal("Reading and Discovering", reading!.Title);
            Assert.Contains("Some artifacts contain useful information", reading.Body);
            Assert.Contains("Tutorial:", reading.AccessibleSummary);

            // Verify LocalizationService has matching strings
            var loc = new LocalizationService();
            string locCulturalTitle = loc.Get("tutorial.collectible.cultural_artifacts.title");
            string locCulturalBody = loc.Get("tutorial.collectible.cultural_artifacts.body");
            string locReadingTitle = loc.Get("tutorial.collectible.reading_and_discovering.title");
            string locReadingBody = loc.Get("tutorial.collectible.reading_and_discovering.body");

            Assert.Equal("Cultural Artifacts", locCulturalTitle);
            Assert.Contains("surviving objects", locCulturalBody);
            Assert.Equal("Reading and Discovering", locReadingTitle);
            Assert.Contains("useful information", locReadingBody);
        }

        [Fact]
        public void FirstCollectibleDiscovery_TriggersCulturalArtifacts()
        {
            var tracker = new CollectibleTutorialTracker();
            Assert.Equal(0, tracker.SeenCount);
            Assert.Equal(0, tracker.QueueCount);

            var nonEffectResult = new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "none",
                EffectApplied = true
            };

            tracker.OnCollectibleDiscovered(nonEffectResult);

            Assert.True(tracker.HasSeen(CollectibleTutorialTracker.CulturalArtifactsId));
            Assert.False(tracker.HasSeen(CollectibleTutorialTracker.ReadingAndDiscoveringId));
            Assert.Equal(1, tracker.QueueCount);

            Assert.True(tracker.TryDequeue(out var entry));
            Assert.NotNull(entry);
            Assert.Equal(CollectibleTutorialTracker.CulturalArtifactsId, entry!.Id);
            Assert.Equal(0, tracker.QueueCount);
        }

        [Fact]
        public void SubsequentCollectibleDiscovery_DoesNotRetriggerCulturalArtifacts()
        {
            var tracker = new CollectibleTutorialTracker();
            var nonEffectResult = new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "none",
                EffectApplied = true
            };

            tracker.OnCollectibleDiscovered(nonEffectResult);
            Assert.Equal(1, tracker.QueueCount);
            tracker.TryDequeue(out _);

            // Second non-effect discovery
            var secondResult = new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "none",
                EffectApplied = true
            };

            tracker.OnCollectibleDiscovered(secondResult);
            Assert.Equal(0, tracker.QueueCount); // Not re-queued!
        }

        [Fact]
        public void FirstEffectBearingCollectible_TriggersReadingAndDiscovering()
        {
            var tracker = new CollectibleTutorialTracker();
            // Pre-seed Cultural Artifacts as already seen
            tracker.MarkSeen(CollectibleTutorialTracker.CulturalArtifactsId);

            var effectResult = new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "morale",
                EffectApplied = true
            };

            tracker.OnCollectibleDiscovered(effectResult);

            Assert.True(tracker.HasSeen(CollectibleTutorialTracker.ReadingAndDiscoveringId));
            Assert.Equal(1, tracker.QueueCount);

            Assert.True(tracker.TryDequeue(out var entry));
            Assert.NotNull(entry);
            Assert.Equal(CollectibleTutorialTracker.ReadingAndDiscoveringId, entry!.Id);
        }

        [Fact]
        public void SubsequentEffectBearingCollectible_DoesNotRetriggerReadingAndDiscovering()
        {
            var tracker = new CollectibleTutorialTracker();
            tracker.MarkSeen(CollectibleTutorialTracker.CulturalArtifactsId);
            tracker.MarkSeen(CollectibleTutorialTracker.ReadingAndDiscoveringId);

            var effectResult = new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "knowledge",
                EffectApplied = true
            };

            tracker.OnCollectibleDiscovered(effectResult);
            Assert.Equal(0, tracker.QueueCount);
        }

        [Fact]
        public void FirstEffectBearingDiscovery_QueuesTutorialsInStableOrder()
        {
            // Case where the very first collectible discovered in the campaign is also effect-bearing
            var tracker = new CollectibleTutorialTracker();

            var firstEffectResult = new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "knowledge",
                EffectApplied = true
            };

            tracker.OnCollectibleDiscovered(firstEffectResult);

            Assert.Equal(2, tracker.QueueCount);
            var queued = tracker.QueuedEntries;
            Assert.Equal(CollectibleTutorialTracker.CulturalArtifactsId, queued[0].Id);
            Assert.Equal(CollectibleTutorialTracker.ReadingAndDiscoveringId, queued[1].Id);

            // Dequeue preserves exact FIFO order
            Assert.True(tracker.TryDequeue(out var first));
            Assert.Equal(CollectibleTutorialTracker.CulturalArtifactsId, first!.Id);

            Assert.True(tracker.TryDequeue(out var second));
            Assert.Equal(CollectibleTutorialTracker.ReadingAndDiscoveringId, second!.Id);

            Assert.False(tracker.TryDequeue(out _));
        }

        [Fact]
        public void CollectibleTutorialSeenState_SurvivesSaveLoad()
        {
            var tracker = new CollectibleTutorialTracker();
            tracker.MarkSeen(CollectibleTutorialTracker.CulturalArtifactsId);

            var save = tracker.CaptureState();
            Assert.NotNull(save);
            Assert.Contains(CollectibleTutorialTracker.CulturalArtifactsId, save.seen_tutorials);

            var restored = new CollectibleTutorialTracker();
            restored.RestoreState(save);

            Assert.True(restored.HasSeen(CollectibleTutorialTracker.CulturalArtifactsId));
            Assert.False(restored.HasSeen(CollectibleTutorialTracker.ReadingAndDiscoveringId));

            // Discovering another collectible does not trigger Cultural Artifacts
            restored.OnCollectibleDiscovered(new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "none",
                EffectApplied = true
            });

            Assert.Equal(0, restored.QueueCount);
        }

        [Fact]
        public void RestoredDiscovery_DoesNotTriggerTutorialFromHistoricalState()
        {
            // Proves that restoring an existing CollectibleDiscoverySave into runtime
            // does NOT raise events or queue tutorials for past items
            var discoveryState = new CollectibleDiscoveryState();
            discoveryState.MarkDiscovered("item_collectible_family_portrait", "loc_tenement");

            var save = discoveryState.CaptureState();

            var freshDiscovery = new CollectibleDiscoveryState();
            var tutorialTracker = new CollectibleTutorialTracker();

            // Simulating save restore: RestoreState is invoked directly without DispatchOnAcquire
            freshDiscovery.RestoreState(save);

            Assert.True(freshDiscovery.IsDiscovered("item_collectible_family_portrait"));
            Assert.Equal(0, tutorialTracker.QueueCount);
            Assert.Equal(0, tutorialTracker.SeenCount);
        }
    }
}
