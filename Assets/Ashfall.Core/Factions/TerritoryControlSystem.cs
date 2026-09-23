#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Factions
{
    public enum SupplyLineStatus
    {
        Active,
        Disrupted,
        Severed
    }

    /// <summary>
    /// Authored territory definition from StreamingAssets/Data/faction_territory.json.
    /// </summary>
    public sealed class FactionTerritoryDef
    {
        public string Id { get; set; } = string.Empty;
        public string Faction { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public string TerritoryScale { get; set; } = string.Empty;
        public string PrimaryResourceInterest { get; set; } = string.Empty;
        public List<string> ControlledNodes { get; set; } = new List<string>();
        public List<string> ControlPoints { get; set; } = new List<string>();
        public List<string> ContestedWith { get; set; } = new List<string>();
        public int BaseControlStrength { get; set; } = 50;
        public double TradeTax { get; set; } = 0.05;
        public double TravelSafety { get; set; } = 0.75;
        public string ShiftTrigger { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Authored supply line definition from StreamingAssets/Data/supply_lines.json.
    /// </summary>
    public sealed class SupplyLineDef
    {
        public string Id { get; set; } = string.Empty;
        public string OwningFactionId { get; set; } = string.Empty;
        public string OriginLocationId { get; set; } = string.Empty;
        public string DestinationLocationId { get; set; } = string.Empty;
        public List<string> RouteWaypoints { get; set; } = new List<string>();
        public string CargoType { get; set; } = string.Empty;
        public int ThroughputCapacity { get; set; } = 30;
        public int TravelDays { get; set; } = 2;
    }

    /// <summary>
    /// Runtime dynamic state for a location under territorial control.
    /// </summary>
    public sealed class LocationTerritoryState
    {
        public string LocationId { get; set; } = string.Empty;
        public string ControllingFactionId { get; set; } = string.Empty;
        public int ControlStrength { get; set; } = 50;
        public bool IsContested { get; set; }
        public int FortificationLevel { get; set; }
        public int GarrisonStrength { get; set; }
        public int LastContestDay { get; set; }
    }

    /// <summary>
    /// Runtime state for a faction supply line.
    /// </summary>
    public sealed class SupplyLineState
    {
        public string SupplyLineId { get; set; } = string.Empty;
        public string OwningFactionId { get; set; } = string.Empty;
        public SupplyLineStatus Status { get; set; } = SupplyLineStatus.Active;
        public int LastDeliveredDay { get; set; }
        public int TotalDelivered { get; set; }
    }

    /// <summary>
    /// Pure domain engine for Plan 134: Dynamic Faction Territory & Supply Line Control.
    /// Manages territorial expansion, contested locations, fortifications, and supply line corridors.
    /// Zero engine references (Godot/UnityEngine free).
    /// </summary>
    public sealed class TerritoryControlSystem
    {
        public const string SystemId = "territory_control_system";

        private readonly Dictionary<string, FactionTerritoryDef> _territories = new Dictionary<string, FactionTerritoryDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, SupplyLineDef> _supplyLineDefs = new Dictionary<string, SupplyLineDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, LocationTerritoryState> _locationStates = new Dictionary<string, LocationTerritoryState>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, SupplyLineState> _supplyLineStates = new Dictionary<string, SupplyLineState>(StringComparer.OrdinalIgnoreCase);

        // Seam delegates
        public Action<string, string, string>? OnTerritoryControlChangedSeam { get; set; }
        public Action<string, string, string>? OnTerritoryContestedSeam { get; set; }
        public Action<string, SupplyLineStatus>? OnSupplyLineStatusChangedSeam { get; set; }
        public Action<string, int>? OnSupplyLineDeliveredSeam { get; set; }
        public Action<string, int>? OnLocationFortifiedSeam { get; set; }

        public TerritoryControlSystem(
            IEnumerable<FactionTerritoryDef>? territories = null,
            IEnumerable<SupplyLineDef>? supplyLines = null)
        {
            if (territories != null)
            {
                foreach (var t in territories)
                {
                    if (t != null && !string.IsNullOrEmpty(t.Id))
                    {
                        _territories[t.Id] = t;

                        // Initialize locations from controlled nodes and points
                        var allNodes = t.ControlledNodes.Concat(t.ControlPoints).Distinct(StringComparer.OrdinalIgnoreCase);
                        foreach (var node in allNodes)
                        {
                            if (!string.IsNullOrEmpty(node) && !_locationStates.ContainsKey(node))
                            {
                                _locationStates[node] = new LocationTerritoryState
                                {
                                    LocationId = node,
                                    ControllingFactionId = t.Faction,
                                    ControlStrength = t.BaseControlStrength,
                                    IsContested = false,
                                    FortificationLevel = 0,
                                    GarrisonStrength = 10,
                                    LastContestDay = 0
                                };
                            }
                        }
                    }
                }
            }

            if (supplyLines != null)
            {
                foreach (var line in supplyLines)
                {
                    if (line != null && !string.IsNullOrEmpty(line.Id))
                    {
                        _supplyLineDefs[line.Id] = line;
                        _supplyLineStates[line.Id] = new SupplyLineState
                        {
                            SupplyLineId = line.Id,
                            OwningFactionId = line.OwningFactionId,
                            Status = SupplyLineStatus.Active,
                            LastDeliveredDay = 0,
                            TotalDelivered = 0
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Factory parser for faction_territory.json and supply_lines.json catalogs.
        /// </summary>
        public static TerritoryControlSystem FromJson(string territoryJson, string supplyLineJson)
        {
            var territories = ParseTerritories(territoryJson);
            var supplyLines = ParseSupplyLines(supplyLineJson);
            return new TerritoryControlSystem(territories, supplyLines);
        }

        private static List<FactionTerritoryDef> ParseTerritories(string json)
        {
            var list = new List<FactionTerritoryDef>();
            if (string.IsNullOrWhiteSpace(json)) return list;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.TryGetProperty("territories", out var arr) && arr.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in arr.EnumerateArray())
                {
                    var def = new FactionTerritoryDef
                    {
                        Id = el.TryGetProperty("id", out var idElem) ? idElem.GetString() ?? string.Empty : string.Empty,
                        Faction = el.TryGetProperty("faction", out var facElem) ? facElem.GetString() ?? string.Empty : string.Empty,
                        DisplayName = el.TryGetProperty("display_name", out var nameElem) ? nameElem.GetString() ?? string.Empty : string.Empty,
                        Classification = el.TryGetProperty("classification", out var clsElem) ? clsElem.GetString() ?? string.Empty : string.Empty,
                        TerritoryScale = el.TryGetProperty("territory_scale", out var sclElem) ? sclElem.GetString() ?? string.Empty : string.Empty,
                        PrimaryResourceInterest = el.TryGetProperty("primary_resource_interest", out var priElem) ? priElem.GetString() ?? string.Empty : string.Empty,
                        BaseControlStrength = el.TryGetProperty("control_strength", out var strElem) ? strElem.GetInt32() : 50,
                        TradeTax = el.TryGetProperty("trade_tax", out var taxElem) ? taxElem.GetDouble() : 0.05,
                        TravelSafety = el.TryGetProperty("travel_safety", out var safeElem) ? safeElem.GetDouble() : 0.75,
                        ShiftTrigger = el.TryGetProperty("shift_trigger", out var shfElem) ? shfElem.GetString() ?? string.Empty : string.Empty,
                        Description = el.TryGetProperty("description", out var descElem) ? descElem.GetString() ?? string.Empty : string.Empty
                    };

                    if (el.TryGetProperty("controlled_nodes", out var cnElem) && cnElem.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var node in cnElem.EnumerateArray())
                        {
                            var s = node.GetString();
                            if (!string.IsNullOrEmpty(s)) def.ControlledNodes.Add(s);
                        }
                    }

                    if (el.TryGetProperty("control_points", out var cpElem) && cpElem.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var cp in cpElem.EnumerateArray())
                        {
                            var s = cp.GetString();
                            if (!string.IsNullOrEmpty(s)) def.ControlPoints.Add(s);
                        }
                    }

                    if (el.TryGetProperty("contested_with", out var cwElem) && cwElem.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var cw in cwElem.EnumerateArray())
                        {
                            var s = cw.GetString();
                            if (!string.IsNullOrEmpty(s)) def.ContestedWith.Add(s);
                        }
                    }

                    list.Add(def);
                }
            }

            return list;
        }

        private static List<SupplyLineDef> ParseSupplyLines(string json)
        {
            var list = new List<SupplyLineDef>();
            if (string.IsNullOrWhiteSpace(json)) return list;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.TryGetProperty("supply_lines", out var arr) && arr.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in arr.EnumerateArray())
                {
                    var def = new SupplyLineDef
                    {
                        Id = el.TryGetProperty("id", out var idElem) ? idElem.GetString() ?? string.Empty : string.Empty,
                        OwningFactionId = el.TryGetProperty("owning_faction_id", out var ownElem) ? ownElem.GetString() ?? string.Empty : string.Empty,
                        OriginLocationId = el.TryGetProperty("origin_location_id", out var orgElem) ? orgElem.GetString() ?? string.Empty : string.Empty,
                        DestinationLocationId = el.TryGetProperty("destination_location_id", out var dstElem) ? dstElem.GetString() ?? string.Empty : string.Empty,
                        CargoType = el.TryGetProperty("cargo_type", out var crgElem) ? crgElem.GetString() ?? string.Empty : string.Empty,
                        ThroughputCapacity = el.TryGetProperty("throughput_capacity", out var capElem) ? capElem.GetInt32() : 30,
                        TravelDays = el.TryGetProperty("travel_days", out var dayElem) ? dayElem.GetInt32() : 2
                    };

                    if (el.TryGetProperty("route_waypoints", out var rwElem) && rwElem.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var wp in rwElem.EnumerateArray())
                        {
                            var s = wp.GetString();
                            if (!string.IsNullOrEmpty(s)) def.RouteWaypoints.Add(s);
                        }
                    }

                    list.Add(def);
                }
            }

            return list;
        }

        public FactionTerritoryDef? GetTerritory(string territoryId)
        {
            if (string.IsNullOrEmpty(territoryId)) return null;
            _territories.TryGetValue(territoryId, out var def);
            return def;
        }

        public LocationTerritoryState? GetLocationState(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            _locationStates.TryGetValue(locationId, out var state);
            return state;
        }

        public SupplyLineState? GetSupplyLineState(string lineId)
        {
            if (string.IsNullOrEmpty(lineId)) return null;
            _supplyLineStates.TryGetValue(lineId, out var state);
            return state;
        }

        public IReadOnlyList<FactionTerritoryDef> GetAllTerritories() => _territories.Values.ToList();
        public IReadOnlyList<LocationTerritoryState> GetAllLocationStates() => _locationStates.Values.ToList();
        public IReadOnlyList<SupplyLineState> GetAllSupplyLines() => _supplyLineStates.Values.ToList();

        /// <summary>
        /// Upgrades or modifies the fortification level of a controlled location (0..3).
        /// </summary>
        public bool FortifyLocation(string locationId, int levelDelta = 1)
        {
            if (!_locationStates.TryGetValue(locationId, out var state)) return false;

            int newLevel = Math.Clamp(state.FortificationLevel + levelDelta, 0, 3);
            if (newLevel == state.FortificationLevel) return false;

            state.FortificationLevel = newLevel;
            state.ControlStrength = Math.Clamp(state.ControlStrength + (levelDelta * 10), 0, 100);
            OnLocationFortifiedSeam?.Invoke(locationId, newLevel);
            return true;
        }

        /// <summary>
        /// Assigns or modifies garrison strength at a location.
        /// </summary>
        public bool AssignGarrison(string locationId, int garrisonDelta)
        {
            if (!_locationStates.TryGetValue(locationId, out var state)) return false;

            state.GarrisonStrength = Math.Max(0, state.GarrisonStrength + garrisonDelta);
            state.ControlStrength = Math.Clamp(state.ControlStrength + (garrisonDelta > 0 ? 5 : -5), 10, 100);
            return true;
        }

        /// <summary>
        /// Resolves a territorial contest or attack against a location.
        /// Returns true if the location changes controlling faction.
        /// </summary>
        public bool ContestLocation(string locationId, string attackingFactionId, int attackPower, ISeededRng rng, int currentDay = 0)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (string.IsNullOrEmpty(attackingFactionId)) return false;
            if (!_locationStates.TryGetValue(locationId, out var state)) return false;

            if (string.Equals(state.ControllingFactionId, attackingFactionId, StringComparison.OrdinalIgnoreCase))
                return false;

            state.LastContestDay = currentDay;
            OnTerritoryContestedSeam?.Invoke(locationId, state.ControllingFactionId, attackingFactionId);

            int effectiveDefense = state.ControlStrength + (state.FortificationLevel * 15) + (state.GarrisonStrength * 3);

            if (attackPower >= effectiveDefense * 2)
            {
                // Overwhelming victory guarantees capture
                string oldFaction = state.ControllingFactionId;
                state.ControllingFactionId = attackingFactionId;
                state.ControlStrength = Math.Clamp(attackPower - effectiveDefense + 20, 25, 80);
                state.IsContested = false;
                state.FortificationLevel = Math.Max(0, state.FortificationLevel - 1);
                state.GarrisonStrength = Math.Max(5, attackPower / 10);

                OnTerritoryControlChangedSeam?.Invoke(locationId, oldFaction, attackingFactionId);
                return true;
            }

            if (attackPower > effectiveDefense)
            {
                double winChance = Math.Clamp((attackPower - effectiveDefense) / 100.0 + 0.45, 0.20, 0.95);
                if (rng.NextDouble() < winChance)
                {
                    string oldFaction = state.ControllingFactionId;
                    state.ControllingFactionId = attackingFactionId;
                    state.ControlStrength = Math.Clamp(attackPower - effectiveDefense + 20, 25, 80);
                    state.IsContested = false;
                    state.FortificationLevel = Math.Max(0, state.FortificationLevel - 1);
                    state.GarrisonStrength = Math.Max(5, attackPower / 10);

                    OnTerritoryControlChangedSeam?.Invoke(locationId, oldFaction, attackingFactionId);
                    return true;
                }
                else
                {
                    // Defense held, but garrison and control suffered
                    state.ControlStrength = Math.Max(10, state.ControlStrength - 20);
                    state.GarrisonStrength = Math.Max(1, state.GarrisonStrength - 5);
                    state.IsContested = true;
                    return false;
                }
            }
            else
            {
                // Defense clearly held
                state.ControlStrength = Math.Max(15, state.ControlStrength - 5);
                state.IsContested = false;
                return false;
            }
        }

        /// <summary>
        /// Raids an active supply line. Returns true if the line was disrupted or severed.
        /// </summary>
        public bool RaidSupplyLine(string supplyLineId, int raidIntensity, ISeededRng rng)
        {
            if (rng == null) throw new ArgumentNullException(nameof(rng));
            if (!_supplyLineStates.TryGetValue(supplyLineId, out var state)) return false;
            if (state.Status == SupplyLineStatus.Severed) return false;

            double disruptionChance = Math.Clamp(raidIntensity / 100.0, 0.1, 0.95);
            if (rng.NextDouble() < disruptionChance)
            {
                var newStatus = raidIntensity >= 80 ? SupplyLineStatus.Severed : SupplyLineStatus.Disrupted;
                state.Status = newStatus;

                // Disrupting a line degrades destination location control
                if (_supplyLineDefs.TryGetValue(supplyLineId, out var def))
                {
                    if (_locationStates.TryGetValue(def.DestinationLocationId, out var locState))
                    {
                        locState.ControlStrength = Math.Max(10, locState.ControlStrength - 15);
                    }
                }

                OnSupplyLineStatusChangedSeam?.Invoke(supplyLineId, newStatus);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Restores a disrupted or severed supply line back to active service.
        /// </summary>
        public bool RestoreSupplyLine(string supplyLineId)
        {
            if (!_supplyLineStates.TryGetValue(supplyLineId, out var state)) return false;
            if (state.Status == SupplyLineStatus.Active) return false;

            state.Status = SupplyLineStatus.Active;
            OnSupplyLineStatusChangedSeam?.Invoke(supplyLineId, SupplyLineStatus.Active);
            return true;
        }

        /// <summary>
        /// Daily advance: delivers supply shipments along active lines and stabilizes control.
        /// </summary>
        public void TickDay(int currentDay, ISeededRng? rng = null)
        {
            foreach (var kvp in _supplyLineStates)
            {
                var state = kvp.Value;
                if (!_supplyLineDefs.TryGetValue(state.SupplyLineId, out var def)) continue;

                if (state.Status == SupplyLineStatus.Active)
                {
                    state.TotalDelivered += def.ThroughputCapacity;
                    state.LastDeliveredDay = currentDay;

                    // Reinforce destination control strength
                    if (_locationStates.TryGetValue(def.DestinationLocationId, out var locState))
                    {
                        locState.ControlStrength = Math.Min(100, locState.ControlStrength + 2);
                    }

                    OnSupplyLineDeliveredSeam?.Invoke(state.SupplyLineId, def.ThroughputCapacity);
                }
            }
        }

        /// <summary>
        /// Captures the persistent runtime state of all locations and supply lines.
        /// Serialized in stable order for deterministic round-trips.
        /// </summary>
        public TerritoryControlSaveState CaptureState()
        {
            var state = new TerritoryControlSaveState { schema_version = 1 };

            foreach (var loc in _locationStates.Values.OrderBy(l => l.LocationId, StringComparer.Ordinal))
            {
                state.locations.Add(new LocationTerritorySaveState
                {
                    location_id = loc.LocationId,
                    controlling_faction_id = loc.ControllingFactionId,
                    control_strength = loc.ControlStrength,
                    is_contested = loc.IsContested,
                    fortification_level = loc.FortificationLevel,
                    garrison_strength = loc.GarrisonStrength,
                    last_contest_day = loc.LastContestDay
                });
            }

            foreach (var line in _supplyLineStates.Values.OrderBy(s => s.SupplyLineId, StringComparer.Ordinal))
            {
                state.supply_lines.Add(new SupplyLineSaveState
                {
                    supply_line_id = line.SupplyLineId,
                    owning_faction_id = line.OwningFactionId,
                    status = line.Status.ToString(),
                    last_delivered_day = line.LastDeliveredDay,
                    total_delivered = line.TotalDelivered
                });
            }

            return state;
        }

        /// <summary>
        /// Restores persistent state over the current catalog instances.
        /// Phantom locations or supply lines not present in the catalog are safely ignored.
        /// </summary>
        public bool RestoreState(TerritoryControlSaveState? state)
        {
            if (state == null || state.schema_version != 1) return false;

            if (state.locations != null)
            {
                foreach (var locSave in state.locations)
                {
                    if (locSave == null || string.IsNullOrWhiteSpace(locSave.location_id)) continue;
                    if (!_locationStates.TryGetValue(locSave.location_id, out var locState)) continue;

                    if (!string.IsNullOrEmpty(locSave.controlling_faction_id))
                    {
                        locState.ControllingFactionId = locSave.controlling_faction_id;
                    }
                    locState.ControlStrength = Math.Clamp(locSave.control_strength, 0, 100);
                    locState.IsContested = locSave.is_contested;
                    locState.FortificationLevel = Math.Clamp(locSave.fortification_level, 0, 3);
                    locState.GarrisonStrength = Math.Max(0, locSave.garrison_strength);
                    locState.LastContestDay = Math.Max(0, locSave.last_contest_day);
                }
            }

            if (state.supply_lines != null)
            {
                foreach (var lineSave in state.supply_lines)
                {
                    if (lineSave == null || string.IsNullOrWhiteSpace(lineSave.supply_line_id)) continue;
                    if (!_supplyLineStates.TryGetValue(lineSave.supply_line_id, out var lineState)) continue;

                    if (Enum.TryParse<SupplyLineStatus>(lineSave.status, true, out var parsedStatus))
                    {
                        lineState.Status = parsedStatus;
                    }
                    lineState.LastDeliveredDay = Math.Max(0, lineSave.last_delivered_day);
                    lineState.TotalDelivered = Math.Max(0, lineSave.total_delivered);
                }
            }

            return true;
        }

        /// <summary>
        /// Produces a read-only census summary of territories, locations, and supply line statuses.
        /// </summary>
        public TerritoryCensus ReadCensus()
        {
            int contestedCount = _locationStates.Values.Count(l => l.IsContested);
            int activeLines = _supplyLineStates.Values.Count(s => s.Status == SupplyLineStatus.Active);
            int disruptedLines = _supplyLineStates.Values.Count(s => s.Status == SupplyLineStatus.Disrupted);
            int severedLines = _supplyLineStates.Values.Count(s => s.Status == SupplyLineStatus.Severed);

            return new TerritoryCensus(
                _territories.Count,
                _locationStates.Count,
                contestedCount,
                _supplyLineStates.Count,
                activeLines,
                disruptedLines,
                severedLines);
        }
    }

    /// <summary>
    /// Save DTO for a location under territorial control.
    /// </summary>
    public sealed class LocationTerritorySaveState
    {
        public string location_id { get; set; } = string.Empty;
        public string controlling_faction_id { get; set; } = string.Empty;
        public int control_strength { get; set; } = 50;
        public bool is_contested { get; set; }
        public int fortification_level { get; set; }
        public int garrison_strength { get; set; }
        public int last_contest_day { get; set; }
    }

    /// <summary>
    /// Save DTO for a faction supply line.
    /// </summary>
    public sealed class SupplyLineSaveState
    {
        public string supply_line_id { get; set; } = string.Empty;
        public string owning_faction_id { get; set; } = string.Empty;
        public string status { get; set; } = "Active";
        public int last_delivered_day { get; set; }
        public int total_delivered { get; set; }
    }

    /// <summary>
    /// Plan 134 persistence envelope for TerritoryControlSystem.
    /// </summary>
    public sealed class TerritoryControlSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<LocationTerritorySaveState> locations { get; set; } = new List<LocationTerritorySaveState>();
        public List<SupplyLineSaveState> supply_lines { get; set; } = new List<SupplyLineSaveState>();
    }

    /// <summary>
    /// Read-only snapshot of territorial control metrics.
    /// </summary>
    public readonly struct TerritoryCensus
    {
        public readonly int TerritoriesCount;
        public readonly int LocationsCount;
        public readonly int ContestedLocationsCount;
        public readonly int TotalSupplyLines;
        public readonly int ActiveSupplyLines;
        public readonly int DisruptedSupplyLines;
        public readonly int SeveredSupplyLines;

        public int TotalTerritories => TerritoriesCount;
        public int TotalNodes => LocationsCount;
        public int ContestedLocations => ContestedLocationsCount;

        public TerritoryCensus(int territories, int locations, int contested, int totalLines, int activeLines, int disruptedLines, int severedLines)
        {
            TerritoriesCount = territories;
            LocationsCount = locations;
            ContestedLocationsCount = contested;
            TotalSupplyLines = totalLines;
            ActiveSupplyLines = activeLines;
            DisruptedSupplyLines = disruptedLines;
            SeveredSupplyLines = severedLines;
        }

        public string Describe() =>
            $"territories: {TerritoriesCount} defined, {LocationsCount} node(s) ({ContestedLocationsCount} contested); "
            + $"supply lines: {ActiveSupplyLines}/{TotalSupplyLines} active, {DisruptedSupplyLines} disrupted, {SeveredSupplyLines} severed";
    }
}
