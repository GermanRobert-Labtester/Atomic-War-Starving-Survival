// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Expeditions
{
    public enum ColonyType
    {
        Outpost = 0,
        Settlement = 1,
        Fortress = 2,
        TradingPost = 3,
        FarmingCommune = 4
    }

    public enum SupplyLineStatus
    {
        Active = 0,
        Disrupted = 1,
        Blocked = 2
    }

    [Serializable]
    public sealed class ColonyBuilding
    {
        public string BuildingId { get; set; } = string.Empty;
        public string DefinitionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = "infrastructure";
        public float Condition { get; set; } = 100f;
        public int Capacity { get; set; } = 2;
        public float DefenseBonus { get; set; } = 0f;
        public float MoraleBonus { get; set; } = 0f;
        public int ConstructionDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class ColonyTypeBlueprintDef
    {
        public string TypeId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float BaseDefense { get; set; } = 50f;
        public int BaseCapacity { get; set; } = 4;
        public float RequiredSupplies { get; set; } = 20f;
    }

    [Serializable]
    public sealed class ColonyBuildingDef
    {
        public string BuildingId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = "infrastructure";
        public int Capacity { get; set; } = 2;
        public float DefenseBonus { get; set; } = 0f;
        public float MoraleBonus { get; set; } = 0f;
        public float MaterialCost { get; set; } = 25f;
    }

    [Serializable]
    public sealed class ColonyOutpost
    {
        public string ColonyId { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ColonyType Type { get; set; } = ColonyType.Outpost;
        public int EstablishedDay { get; set; } = 1;
        public float DefenseRating { get; set; } = 50f;
        public float MoraleRating { get; set; } = 70f;
        public List<string> PopulationIds { get; set; } = new List<string>();
        public float StoredSupplies { get; set; } = 50f;
        public bool IsActive { get; set; } = true;
        public List<ColonyBuilding> Buildings { get; set; } = new List<ColonyBuilding>();

        public float TotalDefense => DefenseRating + Buildings.Sum(b => b.DefenseBonus * (b.Condition / 100f));
        public int TotalCapacity => 4 + Buildings.Sum(b => b.Capacity);
    }

    [Serializable]
    public sealed class SupplyLine
    {
        public string LineId { get; set; } = string.Empty;
        public string OriginId { get; set; } = string.Empty;
        public string DestinationId { get; set; } = string.Empty;
        public SupplyLineStatus Status { get; set; } = SupplyLineStatus.Active;
        public float CargoCapacity { get; set; } = 100f;
        public float DailyFlow { get; set; } = 10f;
        public int LastSupplyDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class ColonyState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<ColonyOutpost> Colonies { get; set; } = new List<ColonyOutpost>();
        public List<SupplyLine> SupplyLines { get; set; } = new List<SupplyLine>();
        public List<ColonyBuildingDef> AuthoredBuildings { get; set; } = new List<ColonyBuildingDef>();
        public List<ColonyTypeBlueprintDef> AuthoredTypes { get; set; } = new List<ColonyTypeBlueprintDef>();
    }

    /// <summary>
    /// Plan 160 — Expedition Colony & Outpost System.
    /// Manages forward operating bases and permanent settlements at expedition destinations,
    /// garrison assignments, resource supply lines, defense ratings, building construction, and local colony morale.
    /// </summary>
    public sealed class ColonySystem
    {
        private readonly ColonyState _state;

        public event Action<ColonyOutpost>? OnColonyEstablished;
        public event Action<SupplyLine, SupplyLineStatus>? OnSupplyLineStatusChanged;
        public event Action<ColonyOutpost, float>? OnColonySuppliesUpdated;
        public event Action<ColonyOutpost, ColonyBuilding>? OnBuildingConstructed;

        public Action<ColonyOutpost>? OnColonyEstablishedSeam { get; set; }
        public Action<SupplyLine, SupplyLineStatus>? OnSupplyLineStatusChangedSeam { get; set; }
        public Action<ColonyOutpost, float>? OnColonySuppliesUpdatedSeam { get; set; }
        public Action<ColonyOutpost, ColonyBuilding>? OnBuildingConstructedSeam { get; set; }

        public int TotalColonyCount => _state.Colonies.Count;
        public int ActiveSupplyLineCount => _state.SupplyLines.Count(s => s.Status == SupplyLineStatus.Active);
        public IReadOnlyList<ColonyBuildingDef> AuthoredBuildings => _state.AuthoredBuildings;
        public IReadOnlyList<ColonyTypeBlueprintDef> AuthoredTypes => _state.AuthoredTypes;

        public ColonySystem(ColonyState? state = null)
        {
            _state = state ?? new ColonyState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("colony_types", out var typesEl) && typesEl.ValueKind == JsonValueKind.Array)
                {
                    _state.AuthoredTypes.Clear();
                    foreach (var item in typesEl.EnumerateArray())
                    {
                        var def = new ColonyTypeBlueprintDef
                        {
                            TypeId = item.TryGetProperty("type_id", out var idProp) ? idProp.GetString() ?? "" : "",
                            DisplayName = item.TryGetProperty("display_name", out var nameProp) ? nameProp.GetString() ?? "" : "",
                            Description = item.TryGetProperty("description", out var descProp) ? descProp.GetString() ?? "" : "",
                            BaseDefense = item.TryGetProperty("base_defense", out var defProp) ? (float)defProp.GetDouble() : 50f,
                            BaseCapacity = item.TryGetProperty("base_capacity", out var capProp) ? capProp.GetInt32() : 4,
                            RequiredSupplies = item.TryGetProperty("required_supplies", out var reqProp) ? (float)reqProp.GetDouble() : 20f
                        };
                        if (!string.IsNullOrEmpty(def.TypeId))
                        {
                            _state.AuthoredTypes.Add(def);
                        }
                    }
                }

                if (root.TryGetProperty("buildings", out var bldEl) && bldEl.ValueKind == JsonValueKind.Array)
                {
                    _state.AuthoredBuildings.Clear();
                    foreach (var item in bldEl.EnumerateArray())
                    {
                        var def = new ColonyBuildingDef
                        {
                            BuildingId = item.TryGetProperty("building_id", out var idProp) ? idProp.GetString() ?? "" : "",
                            Name = item.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? "" : "",
                            Category = item.TryGetProperty("category", out var catProp) ? catProp.GetString() ?? "infrastructure" : "infrastructure",
                            Capacity = item.TryGetProperty("capacity", out var capProp) ? capProp.GetInt32() : 2,
                            DefenseBonus = item.TryGetProperty("defense_bonus", out var defProp) ? (float)defProp.GetDouble() : 0f,
                            MoraleBonus = item.TryGetProperty("morale_bonus", out var morProp) ? (float)morProp.GetDouble() : 0f,
                            MaterialCost = item.TryGetProperty("material_cost", out var costProp) ? (float)costProp.GetDouble() : 25f
                        };
                        if (!string.IsNullOrEmpty(def.BuildingId))
                        {
                            _state.AuthoredBuildings.Add(def);
                        }
                    }
                }
            }
            catch
            {
                // Catalog parsing fallback
            }
        }

        public ColonyOutpost EstablishColony(
            string locationId,
            string name,
            ColonyType type,
            IEnumerable<string>? initialGarrison,
            float initialSupplies = 50f,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(locationId)) throw new ArgumentNullException(nameof(locationId));

            string typeKey = type switch
            {
                ColonyType.Fortress => "fortress",
                ColonyType.Settlement => "settlement",
                ColonyType.TradingPost => "trading_post",
                ColonyType.FarmingCommune => "farming_commune",
                _ => "outpost"
            };

            var bp = _state.AuthoredTypes.FirstOrDefault(t => string.Equals(t.TypeId, typeKey, StringComparison.OrdinalIgnoreCase));
            float baseDefense = bp != null ? bp.BaseDefense : (type switch
            {
                ColonyType.Fortress => 80f,
                ColonyType.Outpost => 50f,
                ColonyType.TradingPost => 40f,
                ColonyType.FarmingCommune => 30f,
                _ => 45f
            });

            var colony = new ColonyOutpost
            {
                ColonyId = $"col_{_state.NextSequence++}",
                LocationId = locationId.Trim(),
                Name = string.IsNullOrWhiteSpace(name) ? $"Outpost {locationId}" : name.Trim(),
                Type = type,
                EstablishedDay = Math.Max(1, currentDay),
                DefenseRating = baseDefense,
                MoraleRating = 70f,
                PopulationIds = initialGarrison != null ? new List<string>(initialGarrison) : new List<string>(),
                StoredSupplies = Math.Max(0f, initialSupplies),
                IsActive = true
            };

            _state.Colonies.Add(colony);
            OnColonyEstablished?.Invoke(colony);
            OnColonyEstablishedSeam?.Invoke(colony);
            return colony;
        }

        public ColonyBuilding? ConstructBuilding(string colonyId, string definitionId, int currentDay = 1)
        {
            var colony = GetColony(colonyId);
            if (colony == null || string.IsNullOrWhiteSpace(definitionId)) return null;

            var def = _state.AuthoredBuildings.FirstOrDefault(b => string.Equals(b.BuildingId, definitionId, StringComparison.OrdinalIgnoreCase))
                ?? new ColonyBuildingDef { BuildingId = definitionId, Name = definitionId, Category = "infrastructure", Capacity = 2, DefenseBonus = 10f };

            var building = new ColonyBuilding
            {
                BuildingId = $"bld_{_state.NextSequence++}",
                DefinitionId = def.BuildingId,
                Name = def.Name,
                Category = def.Category,
                Condition = 100f,
                Capacity = def.Capacity,
                DefenseBonus = def.DefenseBonus,
                MoraleBonus = def.MoraleBonus,
                ConstructionDay = currentDay
            };

            colony.Buildings.Add(building);
            colony.MoraleRating = Math.Clamp(colony.MoraleRating + def.MoraleBonus, 0f, 100f);

            OnBuildingConstructed?.Invoke(colony, building);
            OnBuildingConstructedSeam?.Invoke(colony, building);
            return building;
        }

        public SupplyLine EstablishSupplyLine(
            string originId,
            string destinationId,
            float cargoCapacity = 100f,
            float dailyFlow = 10f,
            int currentDay = 1)
        {
            var line = new SupplyLine
            {
                LineId = $"sl_{_state.NextSequence++}",
                OriginId = originId ?? "shelter",
                DestinationId = destinationId ?? string.Empty,
                Status = SupplyLineStatus.Active,
                CargoCapacity = Math.Max(10f, cargoCapacity),
                DailyFlow = Math.Max(1f, dailyFlow),
                LastSupplyDay = currentDay
            };

            _state.SupplyLines.Add(line);
            return line;
        }

        public bool SetSupplyLineStatus(string lineId, SupplyLineStatus status)
        {
            var line = _state.SupplyLines.FirstOrDefault(s => string.Equals(s.LineId, lineId, StringComparison.OrdinalIgnoreCase));
            if (line == null) return false;

            line.Status = status;
            OnSupplyLineStatusChanged?.Invoke(line, status);
            OnSupplyLineStatusChangedSeam?.Invoke(line, status);
            return true;
        }

        public bool AssignSurvivorToColony(string colonyId, string survivorId)
        {
            var colony = GetColony(colonyId);
            if (colony == null || string.IsNullOrWhiteSpace(survivorId)) return false;

            if (!colony.PopulationIds.Contains(survivorId))
            {
                colony.PopulationIds.Add(survivorId);
            }
            return true;
        }

        public float TransferSupplies(string colonyId, float amount)
        {
            var colony = GetColony(colonyId);
            if (colony == null) return 0f;

            colony.StoredSupplies = Math.Max(0f, colony.StoredSupplies + amount);
            OnColonySuppliesUpdated?.Invoke(colony, colony.StoredSupplies);
            OnColonySuppliesUpdatedSeam?.Invoke(colony, colony.StoredSupplies);
            return colony.StoredSupplies;
        }

        public void TickDay(int currentDay)
        {
            // 1. Process Supply Lines
            foreach (var line in _state.SupplyLines)
            {
                if (line.Status == SupplyLineStatus.Active)
                {
                    var destColony = GetColony(line.DestinationId);
                    if (destColony != null && destColony.IsActive)
                    {
                        destColony.StoredSupplies += line.DailyFlow;
                        line.LastSupplyDay = currentDay;
                        OnColonySuppliesUpdated?.Invoke(destColony, destColony.StoredSupplies);
                        OnColonySuppliesUpdatedSeam?.Invoke(destColony, destColony.StoredSupplies);
                    }
                }
            }

            // 2. Consume supplies and update colony morale
            foreach (var colony in _state.Colonies)
            {
                if (!colony.IsActive) continue;

                float consumption = Math.Max(1, colony.PopulationIds.Count) * 1.5f;
                colony.StoredSupplies = Math.Max(0f, colony.StoredSupplies - consumption);

                if (colony.StoredSupplies <= 0f)
                {
                    // Starvation / supply shortage
                    colony.MoraleRating = Math.Clamp(colony.MoraleRating - 10f, 0f, 100f);
                }
                else if (colony.MoraleRating < 80f)
                {
                    colony.MoraleRating = Math.Clamp(colony.MoraleRating + 1f, 0f, 100f);
                }
            }
        }

        public IReadOnlyList<ColonyOutpost> GetColonies() => _state.Colonies;

        public ColonyOutpost? GetColony(string colonyId)
        {
            return _state.Colonies.FirstOrDefault(c => string.Equals(c.ColonyId, colonyId, StringComparison.OrdinalIgnoreCase));
        }

        public ColonyState CaptureState()
        {
            var state = new ColonyState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Colonies = new List<ColonyOutpost>(_state.Colonies.Count),
                SupplyLines = new List<SupplyLine>(_state.SupplyLines.Count),
                AuthoredBuildings = new List<ColonyBuildingDef>(_state.AuthoredBuildings),
                AuthoredTypes = new List<ColonyTypeBlueprintDef>(_state.AuthoredTypes)
            };

            foreach (var c in _state.Colonies)
            {
                var copyCol = new ColonyOutpost
                {
                    ColonyId = c.ColonyId,
                    LocationId = c.LocationId,
                    Name = c.Name,
                    Type = c.Type,
                    EstablishedDay = c.EstablishedDay,
                    DefenseRating = c.DefenseRating,
                    MoraleRating = c.MoraleRating,
                    PopulationIds = new List<string>(c.PopulationIds),
                    StoredSupplies = c.StoredSupplies,
                    IsActive = c.IsActive,
                    Buildings = new List<ColonyBuilding>(c.Buildings.Count)
                };

                foreach (var b in c.Buildings)
                {
                    copyCol.Buildings.Add(new ColonyBuilding
                    {
                        BuildingId = b.BuildingId,
                        DefinitionId = b.DefinitionId,
                        Name = b.Name,
                        Category = b.Category,
                        Condition = b.Condition,
                        Capacity = b.Capacity,
                        DefenseBonus = b.DefenseBonus,
                        MoraleBonus = b.MoraleBonus,
                        ConstructionDay = b.ConstructionDay
                    });
                }

                state.Colonies.Add(copyCol);
            }

            foreach (var s in _state.SupplyLines)
            {
                state.SupplyLines.Add(new SupplyLine
                {
                    LineId = s.LineId,
                    OriginId = s.OriginId,
                    DestinationId = s.DestinationId,
                    Status = s.Status,
                    CargoCapacity = s.CargoCapacity,
                    DailyFlow = s.DailyFlow,
                    LastSupplyDay = s.LastSupplyDay
                });
            }

            return state;
        }

        public void RestoreState(ColonyState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Colonies.Clear();
            _state.SupplyLines.Clear();
            _state.AuthoredBuildings.Clear();
            _state.AuthoredTypes.Clear();

            if (state.AuthoredBuildings != null)
            {
                _state.AuthoredBuildings.AddRange(state.AuthoredBuildings);
            }

            if (state.AuthoredTypes != null)
            {
                _state.AuthoredTypes.AddRange(state.AuthoredTypes);
            }

            if (state.Colonies != null)
            {
                foreach (var c in state.Colonies)
                {
                    var restCol = new ColonyOutpost
                    {
                        ColonyId = c.ColonyId,
                        LocationId = c.LocationId,
                        Name = c.Name,
                        Type = c.Type,
                        EstablishedDay = c.EstablishedDay,
                        DefenseRating = c.DefenseRating,
                        MoraleRating = c.MoraleRating,
                        PopulationIds = new List<string>(c.PopulationIds ?? Enumerable.Empty<string>()),
                        StoredSupplies = c.StoredSupplies,
                        IsActive = c.IsActive,
                        Buildings = new List<ColonyBuilding>()
                    };

                    if (c.Buildings != null)
                    {
                        foreach (var b in c.Buildings)
                        {
                            restCol.Buildings.Add(new ColonyBuilding
                            {
                                BuildingId = b.BuildingId,
                                DefinitionId = b.DefinitionId,
                                Name = b.Name,
                                Category = b.Category,
                                Condition = b.Condition,
                                Capacity = b.Capacity,
                                DefenseBonus = b.DefenseBonus,
                                MoraleBonus = b.MoraleBonus,
                                ConstructionDay = b.ConstructionDay
                            });
                        }
                    }

                    _state.Colonies.Add(restCol);
                }
            }

            if (state.SupplyLines != null)
            {
                foreach (var s in state.SupplyLines)
                {
                    _state.SupplyLines.Add(new SupplyLine
                    {
                        LineId = s.LineId,
                        OriginId = s.OriginId,
                        DestinationId = s.DestinationId,
                        Status = s.Status,
                        CargoCapacity = s.CargoCapacity,
                        DailyFlow = s.DailyFlow,
                        LastSupplyDay = s.LastSupplyDay
                    });
                }
            }
        }
    }
}
