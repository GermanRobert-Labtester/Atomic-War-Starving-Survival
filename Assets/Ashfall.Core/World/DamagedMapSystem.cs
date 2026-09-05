using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Runtime authority for the damaged treasure-map layer (Plan 85).
    ///
    /// Fragments are discovery tokens, not inventory items: scavenging
    /// producers carry a <c>map_fragment_id</c>, the roll result is forwarded
    /// here, and registration appends the fragment id to
    /// <see cref="WastelandMapState.RegisteredMapFragments"/> — the single
    /// persisted fragment-progress authority (persisted by the existing
    /// wasteland_map save section).
    ///
    /// Completion is derived (every zone fragment registered) and
    /// edge-triggered: the transition from incomplete to complete fires the
    /// reveal at most once. Reveal goes through the authoritative world map —
    /// <see cref="WastelandMapSystem.Discover"/> plus
    /// <see cref="WastelandMapSystem.Unlock"/> of the installation node —
    /// never through a parallel location registry.
    ///
    /// Reveal is not loot: completed maps surface a destination the player
    /// must travel to through normal expedition rules.
    /// </summary>
    public sealed class DamagedMapSystem
    {
        private readonly List<DamagedMapZone> _zones;
        private readonly Dictionary<string, DamagedMapZone> _zonesById;
        private readonly Dictionary<string, DamagedMapZone> _zonesByFragment;
        private readonly Dictionary<string, DamagedMapZone> _zonesByDestination;
        private readonly WastelandMapSystem? _map;

        /// <summary>Fired once when a zone transitions incomplete → complete (edge-triggered).</summary>
        public event Action<DamagedMapZone>? OnZoneCompleted;

        /// <summary>Fired when the zone's installation node is revealed on the world map.</summary>
        public event Action<DamagedMapZone, string>? OnInstallationRevealed;

        /// <summary>Fired when a fragment is newly registered (UI refresh hook).</summary>
        public event Action<string>? OnFragmentRegistered;

        /// <summary>Creates the system. <paramref name="map"/> may be null in headless
        /// tools; reveal is then limited to raising events without map mutation.</summary>
        public DamagedMapSystem(IReadOnlyList<DamagedMapZone> zones, WastelandMapSystem? map)
        {
            _map = map;
            _zones = new List<DamagedMapZone>();
            _zonesById = new Dictionary<string, DamagedMapZone>(StringComparer.Ordinal);
            _zonesByFragment = new Dictionary<string, DamagedMapZone>(StringComparer.Ordinal);
            _zonesByDestination = new Dictionary<string, DamagedMapZone>(StringComparer.Ordinal);

            foreach (var zone in zones)
            {
                if (zone == null || string.IsNullOrEmpty(zone.ZoneId)) continue;
                if (_zonesById.ContainsKey(zone.ZoneId)) continue;
                _zones.Add(zone);
                _zonesById[zone.ZoneId] = zone;

                foreach (var fragment in zone.Fragments)
                {
                    if (fragment == null || string.IsNullOrEmpty(fragment.fragment_id)) continue;
                    // First zone wins on collision; the catalog validator
                    // already flags duplicates for data authority.
                    if (!_zonesByFragment.ContainsKey(fragment.fragment_id))
                        _zonesByFragment[fragment.fragment_id] = zone;
                }

                string? nodeId = ResolveRevealNodeId(zone.InstallationId);
                if (!string.IsNullOrEmpty(nodeId) && !_zonesByDestination.ContainsKey(nodeId))
                    _zonesByDestination[nodeId] = zone;
            }
        }

        /// <summary>All loaded zones in catalog order (ordering never affects logic).</summary>
        public IReadOnlyList<DamagedMapZone> Zones => _zones;

        /// <summary>Zone lookup by id, or null.</summary>
        public DamagedMapZone? FindZone(string zoneId)
            => !string.IsNullOrEmpty(zoneId) && _zonesById.TryGetValue(zoneId, out var z) ? z : null;

        /// <summary>Zone owning a fragment id, or null when unknown.</summary>
        public DamagedMapZone? FindZoneByFragment(string fragmentId)
            => !string.IsNullOrEmpty(fragmentId) && _zonesByFragment.TryGetValue(fragmentId, out var z) ? z : null;

        /// <summary>Zone whose installation maps to a destination/location id, or null.</summary>
        public DamagedMapZone? FindZoneByDestination(string locationId)
            => !string.IsNullOrEmpty(locationId) && _zonesByDestination.TryGetValue(locationId, out var z) ? z : null;

        /// <summary>
        /// Maps a hidden-installation identity onto the wasteland-map node
        /// namespace. Exact ids resolve directly; otherwise the map catalog's
        /// <c>loc_</c> prefix convention is applied. Existing ids are never
        /// renamed to force string equality.
        /// </summary>
        public static string? ResolveRevealNodeId(string installationId)
        {
            if (string.IsNullOrWhiteSpace(installationId)) return null;
            return installationId.StartsWith("loc_", StringComparison.Ordinal)
                ? installationId
                : "loc_" + installationId;
        }

        /// <summary>True when the fragment id has been permanently registered.</summary>
        public bool IsFragmentRegistered(string fragmentId)
        {
            var state = MapState;
            if (state == null || string.IsNullOrEmpty(fragmentId)) return false;
            return state.RegisteredMapFragments.Contains(fragmentId);
        }

        /// <summary>Distinct fragments registered toward a zone so far.</summary>
        public int RegisteredCount(string zoneId)
        {
            var zone = FindZone(zoneId);
            var state = MapState;
            if (zone == null || state == null) return 0;
            int count = 0;
            foreach (var fragment in zone.Fragments)
            {
                if (fragment != null && state.RegisteredMapFragments.Contains(fragment.fragment_id)) count++;
            }
            return count;
        }

        /// <summary>True when every fragment of the zone is registered.</summary>
        public bool IsZoneComplete(string zoneId)
        {
            var zone = FindZone(zoneId);
            if (zone == null || zone.Fragments.Count == 0) return false;
            return RegisteredCount(zoneId) >= zone.Fragments.Count;
        }

        /// <summary>
        /// Registers a discovered fragment. Idempotent: duplicate registration
        /// is a no-op and can never double-count or re-fire completion.
        /// Unknown fragment ids are ignored.
        /// </summary>
        public bool RegisterFragment(string fragmentId)
        {
            var state = MapState;
            if (state == null || string.IsNullOrEmpty(fragmentId)) return false;
            var zone = FindZoneByFragment(fragmentId);
            if (zone == null) return false;
            if (state.RegisteredMapFragments.Contains(fragmentId)) return false;

            state.RegisteredMapFragments.Add(fragmentId);
            OnFragmentRegistered?.Invoke(fragmentId);

            if (IsZoneComplete(zone.ZoneId))
            {
                OnZoneCompleted?.Invoke(zone);
                RevealInstallation(zone);
            }
            return true;
        }

        /// <summary>
        /// True when the installation's world-map node is already discovered —
        /// the persisted reveal state (survives save/load; dropping or selling
        /// physical fragments never un-reveals).
        /// </summary>
        public bool IsInstallationRevealed(string zoneId)
        {
            var zone = FindZone(zoneId);
            if (zone == null) return false;
            string? nodeId = ResolveRevealNodeId(zone.InstallationId);
            if (string.IsNullOrEmpty(nodeId) || _map == null) return false;
            return _map.IsDiscovered(nodeId);
        }

        /// <summary>
        /// Dispatch gate: true while the destination belongs to an
        /// installation whose map has not been completed and revealed.
        /// Non-installation locations are never gated.
        /// </summary>
        public bool IsDestinationLocked(string locationId)
        {
            var zone = FindZoneByDestination(locationId);
            if (zone == null) return false;
            return !IsInstallationRevealed(zone.ZoneId);
        }

        /// <summary>
        /// Reveal operation — idempotent by construction: Discover/Unlock are
        /// no-ops on already-discovered/unlocked nodes, and the caller gates
        /// on the discovered state.
        /// </summary>
        private void RevealInstallation(DamagedMapZone zone)
        {
            if (_map == null) return;
            string? nodeId = ResolveRevealNodeId(zone.InstallationId);
            if (string.IsNullOrEmpty(nodeId)) return;
            if (_map.GetNode(nodeId) == null) return; // catalog misconfiguration; completion still fires

            _map.Discover(nodeId);
            _map.Unlock(nodeId);
            OnInstallationRevealed?.Invoke(zone, nodeId);
        }

        private WastelandMapState? MapState => _map?.State;
    }
}
