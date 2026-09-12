// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    /// <summary>
    /// Workstream C (Task 7): Map and Cartography Integration for Discovery Locations.
    /// Verifies that discovery locations are recorded at acquisition, projected into stable
    /// map markers, survive save/load, remain separate for multi-find locations, sanitize
    /// hidden effect targets, and provide accessible text representations.
    /// </summary>
    public class CollectibleMapIntegrationTests
    {
        private static readonly string RepoRoot = FindRepoRoot();
        private static readonly string DataDir = Path.Combine(RepoRoot, "Assets", "StreamingAssets", "Data");

        private static string FindRepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json")))
                    return dir;
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("repo root with data authority not found");
        }

        private static CollectibleCatalog LoadCollectibles()
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = CollectibleCatalogLoader.Load(DataDir, fileIO, serializer);
            Assert.NotNull(catalog);
            return catalog!;
        }

        [Fact]
        public void DiscoveringCollectible_RecordsOriginLocation()
        {
            var discovery = new CollectibleDiscoveryState();
            const string itemId = "item_collectible_family_portrait";
            const string locationId = "loc_suburban_ruins_01";

            Assert.True(discovery.MarkDiscovered(itemId, locationId));
            Assert.Equal(locationId, discovery.GetDiscoveryLocation(itemId));
        }

        [Fact]
        public void DiscoveringCollectible_CreatesMapProjection()
        {
            var catalog = LoadCollectibles();
            var discovery = new CollectibleDiscoveryState();
            const string itemId = "item_collectible_family_portrait";
            const string locationId = "loc_suburban_ruins_01";

            discovery.MarkDiscovered(itemId, locationId);

            var markers = CollectibleMapProjector.ProjectMarkers(discovery, catalog);
            Assert.Single(markers);

            var marker = markers[0];
            Assert.Equal($"collectible-discovery:{itemId}:{locationId}", marker.MarkerId);
            Assert.Equal(itemId, marker.CollectibleId);
            Assert.Equal(locationId, marker.LocationId);
        }

        [Fact]
        public void CollectibleMarker_UsesCorrectDiscoveryLocation()
        {
            var catalog = LoadCollectibles();
            var discovery = new CollectibleDiscoveryState();
            const string itemId = "item_collectible_diesel_service_manual";
            const string locationId = "loc_industrial_yard";

            discovery.MarkDiscovered(itemId, locationId);

            var markers = CollectibleMapProjector.ProjectMarkers(discovery, catalog);
            Assert.Single(markers);
            Assert.Equal(locationId, markers[0].LocationId);
            Assert.Equal($"Found at {locationId}", markers[0].FoundHereLabel);
        }

        [Fact]
        public void CollectibleMarker_ShowsNameOrCategory()
        {
            var catalog = LoadCollectibles();
            var discovery = new CollectibleDiscoveryState();
            const string itemId = "item_collectible_family_portrait";
            const string locationId = "loc_apartment";

            var itemNames = new Dictionary<string, string>
            {
                { itemId, "Family Portrait" }
            };

            discovery.MarkDiscovered(itemId, locationId);

            var markers = CollectibleMapProjector.ProjectMarkers(discovery, catalog, itemNames);
            Assert.Single(markers);
            Assert.Equal("Family Portrait", markers[0].DisplayName);
            Assert.Equal("photograph", markers[0].Category);
        }

        [Fact]
        public void CollectibleMarker_SurvivesSaveLoad()
        {
            var catalog = LoadCollectibles();
            var discovery = new CollectibleDiscoveryState();
            discovery.MarkDiscovered("item_collectible_family_portrait", "loc_tenement");
            discovery.MarkDiscovered("item_collectible_road_map", "loc_highway_overpass");

            var save = discovery.CaptureState();
            Assert.Equal(2, save.discovery_locations.Length);

            var restored = new CollectibleDiscoveryState();
            restored.RestoreState(save);

            var markers = CollectibleMapProjector.ProjectMarkers(restored, catalog);
            Assert.Equal(2, markers.Count);
            Assert.Equal("loc_tenement", restored.GetDiscoveryLocation("item_collectible_family_portrait"));
            Assert.Equal("loc_highway_overpass", restored.GetDiscoveryLocation("item_collectible_road_map"));
        }

        [Fact]
        public void CollectibleMarker_DoesNotExposeHiddenEffectTarget()
        {
            var catalog = LoadCollectibles();
            var discovery = new CollectibleDiscoveryState();
            // Road map has an effect_target (location_clue -> map node)
            var mapItem = catalog.ByItemId.Values.FirstOrDefault(c => c.effect_type == "location_clue");
            Assert.NotNull(mapItem);

            const string originLocation = "loc_checkpoint_alpha";
            discovery.MarkDiscovered(mapItem!.item_id, originLocation);

            var markers = CollectibleMapProjector.ProjectMarkers(discovery, catalog);
            Assert.Single(markers);

            var marker = markers[0];
            Assert.Equal(originLocation, marker.LocationId);
            Assert.DoesNotContain(mapItem.effect_target, marker.MarkerId);
            Assert.DoesNotContain(mapItem.effect_target, marker.AccessibleText);
            Assert.DoesNotContain(mapItem.effect_target, marker.FoundHereLabel);
            Assert.DoesNotContain("location_clue", marker.AccessibleText);
        }

        [Fact]
        public void MultipleCollectiblesAtSameLocation_RemainSeparateLogicalMarkers()
        {
            var catalog = LoadCollectibles();
            var discovery = new CollectibleDiscoveryState();
            const string location = "loc_central_hospital";

            discovery.MarkDiscovered("item_collectible_family_portrait", location);
            discovery.MarkDiscovered("item_collectible_pre_war_novel", location);

            var markers = CollectibleMapProjector.ProjectMarkers(discovery, catalog);
            Assert.Equal(2, markers.Count);
            Assert.NotEqual(markers[0].MarkerId, markers[1].MarkerId);
            Assert.Equal(location, markers[0].LocationId);
            Assert.Equal(location, markers[1].LocationId);

            // Clustering presentation
            var clusters = CollectibleMapProjector.ClusterByLocation(markers);
            Assert.Single(clusters);
            Assert.Equal(location, clusters[0].LocationId);
            Assert.Equal(2, clusters[0].Count);
            Assert.Contains("2 artifact(s) found here", clusters[0].AccessibleText);
        }

        [Fact]
        public void RepeatedMapProjection_DoesNotDuplicateMarkers()
        {
            var catalog = LoadCollectibles();
            var discovery = new CollectibleDiscoveryState();
            discovery.MarkDiscovered("item_collectible_family_portrait", "loc_tenement");

            var firstProjection = CollectibleMapProjector.ProjectMarkers(discovery, catalog);
            var secondProjection = CollectibleMapProjector.ProjectMarkers(discovery, catalog);

            Assert.Single(firstProjection);
            Assert.Single(secondProjection);
            Assert.Equal(firstProjection[0].MarkerId, secondProjection[0].MarkerId);
        }

        [Fact]
        public void CollectibleMarker_HasAccessibleText()
        {
            var marker = new CollectibleMapMarker(
                "item_collectible_music_box",
                "loc_nursery",
                displayName: "Worn Music Box",
                category: "toy");

            Assert.Equal("Collectible discovery", marker.SemanticRole);
            Assert.Contains("Collectible discovery:", marker.AccessibleText);
            Assert.Contains("Worn Music Box", marker.AccessibleText);
            Assert.Contains("(toy)", marker.AccessibleText);
            Assert.Contains("found at loc_nursery", marker.AccessibleText);
        }

        [Fact]
        public void LegacyDiscoveryWithoutLocation_DoesNotCrashOrFabricateMarker()
        {
            var catalog = LoadCollectibles();
            var legacySave = new CollectibleDiscoverySave
            {
                schema_version = 1,
                discovered_ids = new[] { "item_collectible_family_portrait" },
                discovery_locations = Array.Empty<CollectibleDiscoveryLocationEntry>()
            };

            var restored = new CollectibleDiscoveryState();
            restored.RestoreState(legacySave);

            Assert.True(restored.IsDiscovered("item_collectible_family_portrait"));
            Assert.Null(restored.GetDiscoveryLocation("item_collectible_family_portrait"));

            var markers = CollectibleMapProjector.ProjectMarkers(restored, catalog);
            Assert.Empty(markers); // Zero fabricated markers!
        }

        [Fact]
        public void MapRevealEffectAndDiscoveryMarker_AreIndependentConcepts()
        {
            var mapNodes = new List<MapNode>
            {
                new MapNode { Id = "loc_scavenge_site", StartingUnlocked = true },
                new MapNode { Id = "loc_hidden_bunker", StartingUnlocked = false }
            };
            var mapSystem = new WastelandMapSystem(new WastelandMapState(), mapNodes, new List<MapRoute>());

            var customCatalog = new CollectibleCatalog(new List<CollectibleDefinition>
            {
                new CollectibleDefinition
                {
                    item_id = "item_topo_map",
                    category = "map",
                    effect_type = "location_clue",
                    effect_target = "loc_hidden_bunker"
                }
            });

            var discovery = new CollectibleDiscoveryState();
            var dispatcher = new CollectibleEffectDispatcher(
                customCatalog, discovery, mapProvider: () => mapSystem);

            // Item found at loc_scavenge_site reveals loc_hidden_bunker
            var res = dispatcher.DispatchOnAcquire("item_topo_map", "loc_scavenge_site");
            Assert.True(res.EffectApplied);
            Assert.True(mapSystem.IsDiscovered("loc_hidden_bunker"));

            // But its discovery marker points to where it was found (loc_scavenge_site)
            var markers = CollectibleMapProjector.ProjectMarkers(discovery, customCatalog);
            Assert.Single(markers);
            Assert.Equal("loc_scavenge_site", markers[0].LocationId);
            Assert.NotEqual("loc_hidden_bunker", markers[0].LocationId);
        }
    }
}
