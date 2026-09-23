// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.IO;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan82_67VerdictCassetteIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void VerdictLocationsAndCassetteCatalog_LoadAccurately_WithoutCollisions()
        {
            // Plan 82: 15 Verdict Sites
            var locations = VerdictCatalogLoader.LoadLocations(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(locations);
            Assert.Equal(15, locations.Count);

            // Plan 67: 12 Cassette Sets with 48 parts
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            Assert.NotNull(sets);
            Assert.Equal(12, sets.Count);

            // Verify specific cross-system narrative locations
            var tapeSilo = locations.FirstOrDefault(l => l.id == "loc_archive_tape_silo");
            Assert.NotNull(tapeSilo);
            Assert.Equal("The Archive Tape-Silo", tapeSilo.displayName);
            Assert.Equal(9, tapeSilo.dangerLevel);
            Assert.Contains("CURRENT YEAR", tapeSilo.description);

            var fuseWorld = locations.FirstOrDefault(l => l.id == "loc_network_fuse_bunker");
            Assert.NotNull(fuseWorld);
            Assert.Contains("tape-silo door", fuseWorld.description);
        }

        [Fact]
        public void CassettePlaybackSystem_AcquireAndPlaySequence_GrantsMoraleAndCompletesSet()
        {
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            var vinylMorale = new VinylMoraleSystem();
            var playbackSystem = new CassettePlaybackSystem(vinylMorale);
            playbackSystem.LoadCatalog(sets);

            // Track events
            int playedEvents = 0;
            CassetteSetDefinition? completedSet = null;
            playbackSystem.OnTapePlayed += (part, set, morale) => playedEvents++;
            playbackSystem.OnSetCompleted += set => completedSet = set;

            // Acquire parts 1, 2, 3 of checkpoint_kilo
            Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_1"));
            Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_2"));
            Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_3"));

            playbackSystem.GetSetProgress("checkpoint_kilo", out int collected, out int total);
            Assert.Equal(3, collected);
            Assert.Equal(4, total);
            Assert.False(playbackSystem.IsSetComplete("checkpoint_kilo"));

            // Play part 1
            var playRes1 = playbackSystem.PlayPart("cassette_checkpoint_kilo_1");
            Assert.Equal(ActionResult.StatusKind.Success, playRes1.Status);
            Assert.True(playbackSystem.IsPartPlayed("cassette_checkpoint_kilo_1"));
            Assert.Equal(1, playedEvents);

            // Playing unowned part is blocked
            var playBlocked = playbackSystem.PlayPart("cassette_checkpoint_kilo_4");
            Assert.Equal(ActionResult.StatusKind.Blocked, playBlocked.Status);

            // Acquire final part 4 -> triggers completion
            Assert.True(playbackSystem.AcquirePart("cassette_checkpoint_kilo_4"));
            Assert.True(playbackSystem.IsSetComplete("checkpoint_kilo"));
            Assert.NotNull(completedSet);
            Assert.Equal("checkpoint_kilo", completedSet.set_id);

            // Verify cache disclosure
            var discoveredCaches = playbackSystem.GetDiscoveredCacheLocations();
            Assert.Contains("checkpoint_kilo_armory", discoveredCaches);

            var cacheItems = playbackSystem.GetCacheItems("checkpoint_kilo_armory");
            Assert.Equal(3, cacheItems.Count);
            Assert.Contains("military_mre", cacheItems);
            Assert.Contains("ammo_556", cacheItems);
            Assert.Contains("field_surgical_kit", cacheItems);
        }

        [Fact]
        public void VerdictCartographyToCassetteScavenging_CrossSystemLinkage()
        {
            var locations = VerdictCatalogLoader.LoadLocations(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);

            var playback = new CassettePlaybackSystem();
            playback.LoadCatalog(sets);

            // Find high-value tape locations in verdict cartography
            var tapeSilo = locations.First(l => l.id == "loc_archive_tape_silo");
            Assert.True(tapeSilo.dangerLevel >= 8);
            Assert.True(tapeSilo.baseRadsPerHour >= 40.0f);

            // Recover Station 14 broadcasts preserved in archive
            var station14Set = sets.First(s => s.set_id == "station_14");
            Assert.Equal(6, station14Set.total_parts);

            foreach (var part in station14Set.parts)
            {
                Assert.True(playback.AcquirePart(part.item_id));
                var res = playback.PlayPart(part.item_id);
                Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            }

            Assert.True(playback.IsSetComplete("station_14"));
            var mastCache = playback.GetDiscoveredCacheLocations();
            Assert.Contains("loc_radio_relay_mast", mastCache);

            var mastItems = playback.GetCacheItems("loc_radio_relay_mast");
            Assert.Contains("civil_defense_radio", mastItems);
            Assert.Contains("aa_batteries", mastItems);
        }

        [Fact]
        public void CassettePlaybackSystem_SaveRestoreRoundTrip_PreservesState()
        {
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            var sys1 = new CassettePlaybackSystem();
            sys1.LoadCatalog(sets);

            sys1.AcquirePart("cassette_free_radio_1");
            sys1.AcquirePart("cassette_free_radio_2");
            sys1.AcquirePart("cassette_free_radio_3");
            sys1.AcquirePart("cassette_free_radio_4");
            sys1.PlayPart("cassette_free_radio_1");
            sys1.PlayPart("cassette_free_radio_2");

            Assert.True(sys1.IsSetComplete("resistance_broadcasts"));

            // Capture state
            var state1 = sys1.CaptureState();
            Assert.Equal(4, state1.collectedPartItemIds.Count);
            Assert.Equal(2, state1.playedPartItemIds.Count);
            Assert.Single(state1.completedSetIds);
            Assert.Equal(2, state1.totalPlaybacks);
            Assert.True(state1.totalMoraleAwarded > 0);

            // Serialize & deserialize using SystemTextJsonSerializer
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state1);
            var restoredState = serializer.Deserialize<CassettePlaybackState>(json);
            Assert.NotNull(restoredState);

            // Restore into new system instance
            var sys2 = new CassettePlaybackSystem();
            sys2.LoadCatalog(sets);
            sys2.RestoreState(restoredState);

            Assert.True(sys2.IsSetComplete("resistance_broadcasts"));
            Assert.True(sys2.IsPartCollected("cassette_free_radio_3"));
            Assert.True(sys2.IsPartPlayed("cassette_free_radio_1"));
            Assert.False(sys2.IsPartPlayed("cassette_free_radio_3"));
            Assert.Equal(sys1.State.totalPlaybacks, sys2.State.totalPlaybacks);
            Assert.Equal(sys1.State.totalMoraleAwarded, sys2.State.totalMoraleAwarded);
        }
    }
}
