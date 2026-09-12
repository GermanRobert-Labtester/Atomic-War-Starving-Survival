// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// User-facing and lifecycle status of a collectible item.
    /// </summary>
    public enum CollectibleDiscoveryStatus
    {
        /// <summary>Item has not been acquired or discovered in this campaign.</summary>
        Undiscovered = 0,

        /// <summary>Item was acquired for the first time; discovery is pending player acknowledgement.</summary>
        NewUnacknowledged = 1,

        /// <summary>Item discovery has been acknowledged and recorded into permanent campaign history.</summary>
        DiscoveredAcknowledged = 2
    }

    /// <summary>
    /// Persisted mapping from collectible item ID to the world location where it was discovered.
    /// </summary>
    [Serializable]
    public sealed class CollectibleDiscoveryLocationEntry
    {
        public string item_id = string.Empty;
        public string location_id = string.Empty;
    }

    /// <summary>
    /// Persisted DTO for <see cref="CollectibleDiscoveryState"/>. All id lists
    /// are written ordinal-sorted so HashSet enumeration order can never
    /// destabilize the save checksum (Invariant 4 / determinism rules).
    /// </summary>
    [Serializable]
    public sealed class CollectibleDiscoverySave
    {
        public int schema_version = 2;

        /// <summary>Union of all discovered IDs (acknowledged + unacknowledged) for backward compatibility.</summary>
        public string[] discovered_ids = Array.Empty<string>();

        /// <summary>Collectible IDs discovered but not yet acknowledged by the player (shows NEW).</summary>
        public string[] unacknowledged_ids = Array.Empty<string>();

        /// <summary>Collectible IDs whose discovery has been acknowledged (shows DISCOVERED).</summary>
        public string[] acknowledged_ids = Array.Empty<string>();

        /// <summary>All collectible IDs ever acquired into inventory in this campaign.</summary>
        public string[] ever_acquired_ids = Array.Empty<string>();

        /// <summary>Origin location IDs where collectibles were discovered.</summary>
        public CollectibleDiscoveryLocationEntry[] discovery_locations = Array.Empty<CollectibleDiscoveryLocationEntry>();
    }

    /// <summary>
    /// Campaign-scoped registry of collectible item ids whose one-time
    /// discovery effect has already been handled and whose discovery/acknowledgement
    /// state is tracked.
    ///
    /// Distinct persistent facts:
    /// - WasEverAcquired: permanent acquisition history (survives selling/dropping).
    /// - NewUnacknowledged: first-time acquisition awaiting player UI acknowledgement.
    /// - DiscoveredAcknowledged: acknowledged historical discovery.
    /// - DiscoveryLocationId: originating scavenging/world location ID.
    ///
    /// Deliberately separate from <see cref="UniqueItemClaimRegistry"/>:
    /// discovery gates the one-time EFFECT and UI presentation, uniqueness gates GENERATION.
    /// </summary>
    public sealed class CollectibleDiscoveryState
    {
        private readonly HashSet<string> _unacknowledgedIds =
            new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _acknowledgedIds =
            new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _everAcquiredIds =
            new HashSet<string>(StringComparer.Ordinal);
        private readonly Dictionary<string, string> _discoveryLocations =
            new Dictionary<string, string>(StringComparer.Ordinal);

        public int Count => _unacknowledgedIds.Count + _acknowledgedIds.Count;
        public int UnacknowledgedCount => _unacknowledgedIds.Count;
        public int AcknowledgedCount => _acknowledgedIds.Count;
        public int EverAcquiredCount => _everAcquiredIds.Count;

        /// <summary>True if the collectible was ever acquired into inventory in this campaign.</summary>
        public bool WasEverAcquired(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return _everAcquiredIds.Contains(itemId);
        }

        /// <summary>True if the collectible has been discovered (either unacknowledged or acknowledged).</summary>
        public bool IsDiscovered(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return _unacknowledgedIds.Contains(itemId) || _acknowledgedIds.Contains(itemId);
        }

        /// <summary>True if the collectible discovery is acknowledged.</summary>
        public bool IsAcknowledged(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return _acknowledgedIds.Contains(itemId);
        }

        /// <summary>True if the collectible was discovered but not yet acknowledged (NEW).</summary>
        public bool IsUnacknowledged(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return _unacknowledgedIds.Contains(itemId);
        }

        /// <summary>Resolves the 3-state discovery status for presentation and loot resolution.</summary>
        public CollectibleDiscoveryStatus GetDiscoveryStatus(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return CollectibleDiscoveryStatus.Undiscovered;
            if (_unacknowledgedIds.Contains(itemId)) return CollectibleDiscoveryStatus.NewUnacknowledged;
            if (_acknowledgedIds.Contains(itemId)) return CollectibleDiscoveryStatus.DiscoveredAcknowledged;
            return CollectibleDiscoveryStatus.Undiscovered;
        }

        public IReadOnlyDictionary<string, string> DiscoveryLocations => _discoveryLocations;

        /// <summary>Returns the world location ID where the collectible was discovered, or null if unknown or not recorded.</summary>
        public string? GetDiscoveryLocation(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            return _discoveryLocations.TryGetValue(itemId, out var loc) ? loc : null;
        }

        /// <summary>
        /// Mark a collectible acquired and discovered. Returns true only on the
        /// unknown -> discovered transition (idempotent afterwards).
        /// Newly discovered items enter the NewUnacknowledged state.
        /// Records originating discovery location when provided.
        /// </summary>
        public bool MarkDiscovered(string itemId, string? discoveryLocationId = null)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            _everAcquiredIds.Add(itemId);

            if (_acknowledgedIds.Contains(itemId) || _unacknowledgedIds.Contains(itemId))
                return false;

            if (!string.IsNullOrEmpty(discoveryLocationId))
            {
                _discoveryLocations[itemId] = discoveryLocationId;
            }

            return _unacknowledgedIds.Add(itemId);
        }

        /// <summary>
        /// Acknowledge a discovery, clearing the NEW status into DISCOVERED.
        /// Returns true if the state transitioned from NewUnacknowledged -> DiscoveredAcknowledged.
        /// </summary>
        public bool AcknowledgeDiscovery(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            if (_unacknowledgedIds.Remove(itemId))
            {
                _acknowledgedIds.Add(itemId);
                _everAcquiredIds.Add(itemId);
                return true;
            }
            return false;
        }

        /// <summary>Ordinal-sorted snapshot; checksum-stable.</summary>
        public CollectibleDiscoverySave CaptureState()
        {
            var unack = new string[_unacknowledgedIds.Count];
            _unacknowledgedIds.CopyTo(unack, 0);
            Array.Sort(unack, StringComparer.Ordinal);

            var ack = new string[_acknowledgedIds.Count];
            _acknowledgedIds.CopyTo(ack, 0);
            Array.Sort(ack, StringComparer.Ordinal);

            var ever = new string[_everAcquiredIds.Count];
            _everAcquiredIds.CopyTo(ever, 0);
            Array.Sort(ever, StringComparer.Ordinal);

            // Union for legacy v1 reader compatibility
            var allDiscovered = new List<string>(_unacknowledgedIds.Count + _acknowledgedIds.Count);
            allDiscovered.AddRange(_acknowledgedIds);
            allDiscovered.AddRange(_unacknowledgedIds);
            allDiscovered.Sort(StringComparer.Ordinal);

            var locEntries = new List<CollectibleDiscoveryLocationEntry>(_discoveryLocations.Count);
            foreach (var kv in _discoveryLocations)
            {
                locEntries.Add(new CollectibleDiscoveryLocationEntry
                {
                    item_id = kv.Key,
                    location_id = kv.Value
                });
            }
            locEntries.Sort((a, b) => string.CompareOrdinal(a.item_id, b.item_id));

            return new CollectibleDiscoverySave
            {
                schema_version = 2,
                discovered_ids = allDiscovered.ToArray(),
                unacknowledged_ids = unack,
                acknowledged_ids = ack,
                ever_acquired_ids = ever,
                discovery_locations = locEntries.ToArray()
            };
        }

        /// <summary>
        /// Restore clears current state, then loads the persisted ids.
        /// Handles legacy schema_version 1 saves by marking all discovered_ids as acknowledged.
        /// Restore never raises effects and never touches inventory.
        /// </summary>
        public void RestoreState(CollectibleDiscoverySave? save)
        {
            _unacknowledgedIds.Clear();
            _acknowledgedIds.Clear();
            _everAcquiredIds.Clear();
            _discoveryLocations.Clear();

            if (save == null) return;

            // Restore origin locations if present in save
            if (save.discovery_locations != null)
            {
                for (int i = 0; i < save.discovery_locations.Length; i++)
                {
                    var entry = save.discovery_locations[i];
                    if (entry != null && !string.IsNullOrEmpty(entry.item_id) && !string.IsNullOrEmpty(entry.location_id))
                    {
                        _discoveryLocations[entry.item_id] = entry.location_id;
                    }
                }
            }

            if (save.schema_version <= 1 ||
                ((save.acknowledged_ids == null || save.acknowledged_ids.Length == 0) &&
                 (save.unacknowledged_ids == null || save.unacknowledged_ids.Length == 0) &&
                 save.discovered_ids != null && save.discovered_ids.Length > 0))
            {
                // Pre-Plan-48 legacy save or un-split save: all historical discoveries are acknowledged
                if (save.discovered_ids != null)
                {
                    for (int i = 0; i < save.discovered_ids.Length; i++)
                    {
                        string id = save.discovered_ids[i];
                        if (!string.IsNullOrEmpty(id))
                        {
                            _acknowledgedIds.Add(id);
                            _everAcquiredIds.Add(id);
                        }
                    }
                }
                return;
            }

            // Schema version 2+
            if (save.acknowledged_ids != null)
            {
                for (int i = 0; i < save.acknowledged_ids.Length; i++)
                {
                    string id = save.acknowledged_ids[i];
                    if (!string.IsNullOrEmpty(id))
                    {
                        _acknowledgedIds.Add(id);
                        _everAcquiredIds.Add(id);
                    }
                }
            }

            if (save.unacknowledged_ids != null)
            {
                for (int i = 0; i < save.unacknowledged_ids.Length; i++)
                {
                    string id = save.unacknowledged_ids[i];
                    if (!string.IsNullOrEmpty(id))
                    {
                        _unacknowledgedIds.Add(id);
                        _everAcquiredIds.Add(id);
                    }
                }
            }

            if (save.ever_acquired_ids != null)
            {
                for (int i = 0; i < save.ever_acquired_ids.Length; i++)
                {
                    string id = save.ever_acquired_ids[i];
                    if (!string.IsNullOrEmpty(id))
                        _everAcquiredIds.Add(id);
                }
            }
        }
    }
}
