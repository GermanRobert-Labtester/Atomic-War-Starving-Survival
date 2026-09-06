// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Factions;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.World
{
    public interface ITerritoryAuthority
    {
        bool IsClaimedBy(string locationId, string factionId);
        WarlordTerritoryState GetTerritoryState(string locationId);
        string GetController(string locationId);
        bool IsClaimant(string locationId, string factionId);
    }

    public static class PatrolTerritoryResolver
    {
        private static readonly Dictionary<string, string> RegionToLocationMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "the_toll", "loc_toll_house" },
            { "high_scarp", "pass_mount_karkov" },
            { "industrial_belt", "loc_denial_cut_substation" },
            { "dead_suburbs", "settlement_13" }
        };

        public static string ResolveLocationForRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region)) return string.Empty;
            return RegionToLocationMap.TryGetValue(region.Trim(), out string? loc) ? loc : region.Trim();
        }

        public static string ResolveLocation(string locationId, string region)
        {
            if (!string.IsNullOrWhiteSpace(locationId)) return locationId.Trim();
            return ResolveLocationForRegion(region);
        }
    }

    public sealed class DynamicTerritoryAuthority : ITerritoryAuthority
    {
        private sealed class LocationTerritoryEntry
        {
            public string LocationId { get; set; } = string.Empty;
            public WarlordTerritoryState State { get; set; } = WarlordTerritoryState.None;
            public string ControllerFactionId { get; set; } = string.Empty;
            public HashSet<string> Claimants { get; } = new(StringComparer.OrdinalIgnoreCase);
        }

        private readonly Dictionary<string, LocationTerritoryEntry> _entries = new(StringComparer.OrdinalIgnoreCase);
        private readonly WarlordDoctrineSystem? _warlord;

        public DynamicTerritoryAuthority(WarlordDoctrineSystem? warlord = null)
        {
            _warlord = warlord;
        }

        public void SetTerritory(
            string locationId,
            WarlordTerritoryState state,
            string? controllerFactionId = null,
            IEnumerable<string>? claimantFactions = null)
        {
            if (string.IsNullOrWhiteSpace(locationId)) return;
            string loc = locationId.Trim();

            if (!_entries.TryGetValue(loc, out var entry))
            {
                entry = new LocationTerritoryEntry { LocationId = loc };
                _entries[loc] = entry;
            }

            entry.State = state;
            entry.ControllerFactionId = !string.IsNullOrWhiteSpace(controllerFactionId)
                ? FactionStandingIdResolver.ToSystemsId(controllerFactionId)
                : string.Empty;

            entry.Claimants.Clear();
            if (claimantFactions != null)
            {
                foreach (var c in claimantFactions)
                {
                    if (!string.IsNullOrWhiteSpace(c))
                    {
                        entry.Claimants.Add(FactionStandingIdResolver.ToSystemsId(c));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(entry.ControllerFactionId))
            {
                entry.Claimants.Add(entry.ControllerFactionId);
            }

            if (_warlord != null)
            {
                bool isWarlordController = string.Equals(entry.ControllerFactionId, WarlordDoctrineSystem.CanonicalFactionId, StringComparison.OrdinalIgnoreCase);
                if (isWarlordController || string.IsNullOrWhiteSpace(entry.ControllerFactionId))
                {
                    _warlord.SetTerritoryState(loc, state);
                }
                else if (state == WarlordTerritoryState.Controlled)
                {
                    // Warlord lost control of this node to another faction
                    _warlord.SetTerritoryState(loc, WarlordTerritoryState.None);
                }
            }
        }

        public WarlordTerritoryState GetTerritoryState(string locationId)
        {
            if (string.IsNullOrWhiteSpace(locationId)) return WarlordTerritoryState.None;
            string loc = locationId.Trim();

            if (_entries.TryGetValue(loc, out var entry))
            {
                return entry.State;
            }

            if (_warlord != null)
            {
                return _warlord.TerritoryState(loc);
            }

            return WarlordTerritoryState.None;
        }

        public string GetController(string locationId)
        {
            if (string.IsNullOrWhiteSpace(locationId)) return string.Empty;
            string loc = locationId.Trim();

            if (_entries.TryGetValue(loc, out var entry))
            {
                return entry.ControllerFactionId;
            }

            if (_warlord != null)
            {
                var st = _warlord.TerritoryState(loc);
                if (st == WarlordTerritoryState.Controlled)
                {
                    return WarlordDoctrineSystem.CanonicalFactionId;
                }
            }

            return string.Empty;
        }

        public bool IsClaimant(string locationId, string factionId)
        {
            if (string.IsNullOrWhiteSpace(locationId) || string.IsNullOrWhiteSpace(factionId)) return false;
            string loc = locationId.Trim();
            string canonical = FactionStandingIdResolver.ToSystemsId(factionId);

            if (_entries.TryGetValue(loc, out var entry))
            {
                return entry.Claimants.Contains(canonical);
            }

            if (_warlord != null)
            {
                var st = _warlord.TerritoryState(loc);
                if (st is WarlordTerritoryState.Claimed or WarlordTerritoryState.Contested or WarlordTerritoryState.Controlled)
                {
                    if (string.Equals(canonical, WarlordDoctrineSystem.CanonicalFactionId, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsClaimedBy(string locationId, string factionId)
        {
            if (string.IsNullOrWhiteSpace(locationId) || string.IsNullOrWhiteSpace(factionId)) return false;
            string loc = locationId.Trim();
            string canonical = FactionStandingIdResolver.ToSystemsId(factionId);

            if (_entries.TryGetValue(loc, out var entry))
            {
                return entry.State switch
                {
                    WarlordTerritoryState.None => false,
                    WarlordTerritoryState.Claimed => entry.Claimants.Contains(canonical) || string.Equals(entry.ControllerFactionId, canonical, StringComparison.OrdinalIgnoreCase),
                    WarlordTerritoryState.Contested => entry.Claimants.Contains(canonical) || string.Equals(entry.ControllerFactionId, canonical, StringComparison.OrdinalIgnoreCase),
                    WarlordTerritoryState.Controlled => string.Equals(entry.ControllerFactionId, canonical, StringComparison.OrdinalIgnoreCase),
                    _ => false
                };
            }

            if (_warlord != null)
            {
                var st = _warlord.TerritoryState(loc);
                bool isWarlord = string.Equals(canonical, WarlordDoctrineSystem.CanonicalFactionId, StringComparison.OrdinalIgnoreCase);

                return st switch
                {
                    WarlordTerritoryState.None => false,
                    WarlordTerritoryState.Claimed => isWarlord,
                    WarlordTerritoryState.Contested => isWarlord,
                    WarlordTerritoryState.Controlled => isWarlord,
                    _ => false
                };
            }

            return false;
        }

        public void SetNode(string locationId, WarlordTerritoryState state, string? controllerFactionId = null)
        {
            SetTerritory(locationId, state, controllerFactionId);
        }

        public DynamicTerritoryState CaptureState()
        {
            var state = new DynamicTerritoryState();
            foreach (var kvp in _entries)
            {
                state.Nodes.Add(new TerritoryNodeRecord
                {
                    LocationId = kvp.Value.LocationId,
                    State = kvp.Value.State,
                    ControllerFactionId = kvp.Value.ControllerFactionId,
                    Claimants = new List<string>(kvp.Value.Claimants)
                });
            }
            return state;
        }

        public void RestoreState(DynamicTerritoryState? state)
        {
            _entries.Clear();
            if (state?.Nodes == null) return;
            foreach (var node in state.Nodes)
            {
                SetTerritory(node.LocationId, node.State, node.ControllerFactionId, node.Claimants);
            }
        }
    }

    [Serializable]
    public sealed class TerritoryNodeRecord
    {
        public string LocationId { get; set; } = string.Empty;
        public WarlordTerritoryState State { get; set; } = WarlordTerritoryState.None;
        public string ControllerFactionId { get; set; } = string.Empty;
        public List<string> Claimants { get; set; } = new();
    }

    [Serializable]
    public sealed class DynamicTerritoryState
    {
        public List<TerritoryNodeRecord> Nodes { get; set; } = new();
    }
}
