using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core
{
    /// <summary>
    /// Logical map marker representing a collectible discovery origin point.
    /// Exposes presentation and accessibility metadata without leaking hidden
    /// effect targets, location clues, or internal dispatch IDs (Plan 47 Task 7).
    /// </summary>
    public sealed class CollectibleMapMarker
    {
        public const string SemanticRoleName = "Collectible discovery";

        public string MarkerId { get; }
        public string CollectibleId { get; }
        public string DisplayName { get; }
        public string Category { get; }
        public string LocationId { get; }
        public string FoundHereLabel { get; }
        public string AccessibleText { get; }
        public string SemanticRole => SemanticRoleName;

        public CollectibleMapMarker(
            string collectibleId,
            string locationId,
            string? displayName = null,
            string? category = null)
        {
            if (string.IsNullOrEmpty(collectibleId)) throw new ArgumentException("Collectible ID cannot be null or empty.", nameof(collectibleId));
            if (string.IsNullOrEmpty(locationId)) throw new ArgumentException("Location ID cannot be null or empty.", nameof(locationId));

            CollectibleId = collectibleId;
            LocationId = locationId;
            MarkerId = GenerateMarkerId(collectibleId, locationId);
            DisplayName = !string.IsNullOrEmpty(displayName) ? displayName : collectibleId;
            Category = !string.IsNullOrEmpty(category) ? category : "artifact";
            FoundHereLabel = $"Found at {locationId}";
            AccessibleText = $"{SemanticRoleName}: {DisplayName} ({Category}) found at {LocationId}";
        }

        public static string GenerateMarkerId(string collectibleId, string locationId) =>
            $"collectible-discovery:{collectibleId}:{locationId}";
    }

    /// <summary>
    /// Presentation cluster aggregating multiple collectible discoveries at the same map location.
    /// Individual logical markers remain preserved and addressable.
    /// </summary>
    public sealed class CollectibleMapCluster
    {
        public string LocationId { get; }
        public IReadOnlyList<CollectibleMapMarker> Markers { get; }
        public int Count => Markers.Count;
        public string AccessibleText { get; }

        public CollectibleMapCluster(string locationId, IReadOnlyList<CollectibleMapMarker> markers)
        {
            LocationId = locationId ?? string.Empty;
            Markers = markers ?? Array.Empty<CollectibleMapMarker>();
            AccessibleText = $"{Markers.Count} artifact(s) found here: {string.Join(", ", Markers.Select(m => m.DisplayName))}";
        }
    }

    /// <summary>
    /// Pure engine-agnostic cartography projection converting persistent collectible discovery
    /// state into map markers. Idempotent, deterministic, and save-safe.
    /// </summary>
    public static class CollectibleMapProjector
    {
        /// <summary>
        /// Projects logical map markers from authoritative discovery state.
        /// Rebuilt deterministically on map initialization/restore.
        /// Ignores discoveries with no recorded origin location.
        /// Hidden effect targets and location clues are strictly excluded from the payload.
        /// </summary>
        public static IReadOnlyList<CollectibleMapMarker> ProjectMarkers(
            CollectibleDiscoveryState discoveryState,
            CollectibleCatalog catalog,
            IReadOnlyDictionary<string, string>? itemDisplayNames = null)
        {
            if (discoveryState == null) throw new ArgumentNullException(nameof(discoveryState));
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));

            var list = new List<CollectibleMapMarker>();

            foreach (var kv in discoveryState.DiscoveryLocations)
            {
                string itemId = kv.Key;
                string locationId = kv.Value;

                if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(locationId))
                    continue;

                // Ensure item was actually discovered in this campaign
                if (!discoveryState.IsDiscovered(itemId))
                    continue;

                var def = catalog.GetByItemId(itemId);
                string category = def?.category ?? "artifact";
                string displayName = itemId;

                if (itemDisplayNames != null && itemDisplayNames.TryGetValue(itemId, out var resolvedName) && !string.IsNullOrEmpty(resolvedName))
                {
                    displayName = resolvedName;
                }

                list.Add(new CollectibleMapMarker(itemId, locationId, displayName, category));
            }

            // Ordinal sort by MarkerId guarantees determinism across all platforms and runs
            list.Sort((a, b) => string.CompareOrdinal(a.MarkerId, b.MarkerId));
            return list;
        }

        /// <summary>
        /// Clusters logical markers by location for high-level cartography presentation.
        /// Deterministically sorted by LocationId.
        /// </summary>
        public static IReadOnlyList<CollectibleMapCluster> ClusterByLocation(IEnumerable<CollectibleMapMarker> markers)
        {
            if (markers == null) return Array.Empty<CollectibleMapCluster>();

            var groups = new Dictionary<string, List<CollectibleMapMarker>>(StringComparer.Ordinal);
            foreach (var m in markers)
            {
                if (m == null || string.IsNullOrEmpty(m.LocationId)) continue;
                if (!groups.TryGetValue(m.LocationId, out var groupList))
                {
                    groupList = new List<CollectibleMapMarker>();
                    groups[m.LocationId] = groupList;
                }
                groupList.Add(m);
            }

            var locKeys = groups.Keys.ToList();
            locKeys.Sort(StringComparer.Ordinal);

            var result = new List<CollectibleMapCluster>(locKeys.Count);
            foreach (var loc in locKeys)
            {
                var groupList = groups[loc];
                groupList.Sort((a, b) => string.CompareOrdinal(a.MarkerId, b.MarkerId));
                result.Add(new CollectibleMapCluster(loc, groupList));
            }

            return result;
        }
    }
}
