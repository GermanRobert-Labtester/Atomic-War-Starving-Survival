// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Xunit;

namespace Ashfall.Core.Tests.Audio
{
    /// <summary>
    /// Core-contract tests for the cultural cassette collection authority and its
    /// previously-uncalled authored loader. Morale and item granting are the host's
    /// job; these tests pin the contract the host relies on: first-play
    /// deduplication, acquisition-once, authored cache data, and save round-trip.
    /// </summary>
    public sealed class PlanCassettePlaybackTests
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

        private static string DataDirectory =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static CassettePlaybackSystem Loaded()
        {
            var system = new CassettePlaybackSystem();
            system.LoadCatalog(CassetteSetCatalogLoader.Load(DataDirectory));
            return system;
        }

        private static CassetteSetDefinition FirstSet(CassettePlaybackSystem system)
        {
            foreach (var set in CassetteSetCatalogLoader.Load(DataDirectory))
                return set;
            throw new InvalidOperationException("authored cassette catalog is empty");
        }

        [Fact]
        public void AuthoredCatalogLoadsEverySetWithParts()
        {
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            Assert.NotEmpty(sets);
            foreach (var set in sets)
            {
                Assert.False(string.IsNullOrEmpty(set.set_id));
                Assert.True(set.total_parts > 0);
                Assert.NotEmpty(set.parts);
            }

            var system = Loaded();
            Assert.Equal(sets.Count, system.TotalSetsCount);
        }

        [Fact]
        public void UnknownTapeIsRefused()
        {
            var system = Loaded();
            var result = system.PlayPart("cassette_not_a_real_tape", 1);
            Assert.False(result.IsSuccess);
            Assert.Equal("unknown_tape", result.FailureCode);
        }

        [Fact]
        public void UnheldTapeCannotBePlayed()
        {
            var system = Loaded();
            string part = FirstSet(system).parts[0].item_id;
            var result = system.PlayPart(part, 1);
            Assert.False(result.IsSuccess);
            Assert.Equal("not_owned", result.FailureCode);
        }

        [Fact]
        public void AcquisitionIsOneShotPerPart()
        {
            var system = Loaded();
            string part = FirstSet(system).parts[0].item_id;

            Assert.True(system.AcquirePart(part));
            Assert.False(system.AcquirePart(part), "a held tape must not acquire twice");
            Assert.True(system.IsPartCollected(part));
        }

        [Fact]
        public void EveryPlayRaisesTheEventSoTheHostMustDedupeMorale()
        {
            var system = Loaded();
            var set = FirstSet(system);
            foreach (var part in set.parts) system.AcquirePart(part.item_id);
            string first = set.parts[0].item_id;

            int events = 0;
            system.OnTapePlayed += (_, _, _) => events++;

            Assert.True(system.PlayPart(first, 2).IsSuccess);
            Assert.True(system.PlayPart(first, 3).IsSuccess);

            // The engine dedupes its own played list but fires on every play. The
            // host therefore owns the once-only morale rule; pin that contract.
            Assert.Equal(2, events);
            Assert.Single(system.State.playedPartItemIds.FindAll(id => id == first));
        }

        [Fact]
        public void CompletingASetRevealsOnlyAuthoredCacheData()
        {
            var system = Loaded();
            var set = FirstSet(system);

            foreach (var part in set.parts) Assert.True(system.AcquirePart(part.item_id));

            Assert.True(system.IsSetComplete(set.set_id));
            Assert.Contains(set.hidden_cache_location, system.GetDiscoveredCacheLocations());

            var items = system.GetCacheItems(set.hidden_cache_location);
            Assert.NotEmpty(items);
            foreach (string authored in set.hidden_cache_items)
                Assert.Contains(authored, items);
        }

        [Fact]
        public void EachSetCompletesExactlyOnce()
        {
            var system = Loaded();
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            var completed = new List<string>();
            system.OnSetCompleted += set => completed.Add(set.set_id);

            foreach (var set in sets)
                foreach (var part in set.parts) system.AcquirePart(part.item_id);

            // Every authored set completes once — and only once — even though each
            // part is offered a second time by the loop above (AcquirePart is one
            // shot, so no completion can re-fire).
            Assert.Equal(sets.Count, completed.Count);
            Assert.Equal(sets.Count, new HashSet<string>(completed).Count);
        }

        [Fact]
        public void AcquiringAnExtraPartAfterCompletionDoesNotReFire()
        {
            var system = Loaded();
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            foreach (var set in sets)
                foreach (var part in set.parts) system.AcquirePart(part.item_id);

            int afterAll = 0;
            system.OnSetCompleted += _ => afterAll++;
            // Re-offer every part: acquisition is refused, so no completion re-fires.
            foreach (var set in sets)
                foreach (var part in set.parts) Assert.False(system.AcquirePart(part.item_id));

            Assert.Equal(0, afterAll);
        }

        [Fact]
        public void SetProgressCountsOnlyCollectedParts()
        {
            var system = Loaded();
            var set = FirstSet(system);
            system.AcquirePart(set.parts[0].item_id);

            system.GetSetProgress(set.set_id, out int collected, out int total);
            Assert.Equal(set.total_parts, total);
            Assert.Equal(1, collected);
        }

        [Fact]
        public void CaptureAndRestoreRoundTripsTheCollection()
        {
            var system = Loaded();
            var set = FirstSet(system);
            system.AcquirePart(set.parts[0].item_id);
            system.PlayPart(set.parts[0].item_id, 4);
            system.AcquirePart(set.parts[1].item_id);

            var captured = system.CaptureState();
            var reloaded = Loaded();
            reloaded.RestoreState(captured);

            Assert.True(reloaded.IsPartCollected(set.parts[0].item_id));
            Assert.True(reloaded.IsPartPlayed(set.parts[0].item_id));
            Assert.True(reloaded.IsPartCollected(set.parts[1].item_id));
            Assert.Equal(system.State.totalPlaybacks, reloaded.State.totalPlaybacks);
        }

        [Fact]
        public void EmptyTapeIdIsRefusedWithoutMutation()
        {
            var system = Loaded();
            int plays = system.State.totalPlaybacks;
            Assert.False(system.AcquirePart(string.Empty));
            Assert.False(system.PlayPart(string.Empty, 1).IsSuccess);
            Assert.Equal(plays, system.State.totalPlaybacks);
        }
    }
}
