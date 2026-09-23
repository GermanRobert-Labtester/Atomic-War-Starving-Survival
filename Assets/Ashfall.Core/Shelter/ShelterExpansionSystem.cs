// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 156 — Shelter Expansion & Physical Renovation
// Subsystem    : ShelterExpansionSystem / Grid Construction, Renovation & Stability
// Authority    : Next-steps-plans/Plan_156_Shelter_Expansion_Physical_Renovation.md
//                UNBLOCK-PROGRAM-WAVE32-BATCH5-PLANS (DEC-152)
// ============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    public enum ProjectStatus
    {
        Planned   = 0,
        Active    = 1,
        Completed = 2,
        Paused    = 3,
        Cancelled = 4
    }

    public enum ConstructionProjectType
    {
        NewRoom          = 0,
        Renovation       = 1,
        ExpansionTunnel  = 2,
        Upgrade          = 3,
        DepthExcavation  = 4
    }

    public sealed class BlueprintDef
    {
        [JsonPropertyName("blueprint_id")]
        public string BlueprintId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("room_type_id")]
        public string RoomTypeId { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("base_labor_days")]
        public double BaseLaborDays { get; set; } = 8.0;

        [JsonPropertyName("min_depth_level")]
        public int MinDepthLevel { get; set; } = 1;

        [JsonPropertyName("max_depth_level")]
        public int MaxDepthLevel { get; set; } = 3;

        [JsonPropertyName("stability_cost")]
        public double StabilityCost { get; set; } = 5.0;

        [JsonPropertyName("resource_costs")]
        public Dictionary<string, int> ResourceCosts { get; set; } = new();
    }

    public sealed class UpgradeDef
    {
        [JsonPropertyName("upgrade_id")]
        public string UpgradeId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("labor_days")]
        public double LaborDays { get; set; } = 3.0;

        [JsonPropertyName("stability_restored")]
        public double StabilityRestored { get; set; } = 0.0;

        [JsonPropertyName("morale_bonus")]
        public int MoraleBonus { get; set; } = 2;

        [JsonPropertyName("resource_costs")]
        public Dictionary<string, int> ResourceCosts { get; set; } = new();
    }

    public sealed class ExpansionRoomDto
    {
        public string RoomId { get; set; } = string.Empty;
        public string RoomTypeId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Condition { get; set; } = 100.0; // 0..100
        public List<string> Upgrades { get; set; } = new();
        public List<string> ConnectedRoomIds { get; set; } = new();
        public int Capacity { get; set; } = 4;
        public int GridX { get; set; }
        public int GridY { get; set; }
        public int DepthLevel { get; set; } = 1;
        public bool IsConstructed { get; set; } = true;

        public ExpansionRoomDto Clone()
        {
            return new ExpansionRoomDto
            {
                RoomId = RoomId,
                RoomTypeId = RoomTypeId,
                Name = Name,
                Condition = Condition,
                Capacity = Capacity,
                GridX = GridX,
                GridY = GridY,
                DepthLevel = DepthLevel,
                IsConstructed = IsConstructed,
                Upgrades = new List<string>(Upgrades),
                ConnectedRoomIds = new List<string>(ConnectedRoomIds)
            };
        }
    }

    public sealed class ConstructionProjectDto
    {
        public string ProjectId { get; set; } = string.Empty;
        public ConstructionProjectType ProjectType { get; set; } = ConstructionProjectType.NewRoom;
        public string TargetRoomId { get; set; } = string.Empty;
        public string BlueprintId { get; set; } = string.Empty;
        public string UpgradeId { get; set; } = string.Empty;
        public int GridX { get; set; }
        public int GridY { get; set; }
        public int DepthLevel { get; set; } = 1;
        public double LaborRequiredDays { get; set; } = 8.0;
        public double LaborInvestedDays { get; set; }
        public Dictionary<string, int> ResourceCosts { get; set; } = new();
        public int ConstructionDay { get; set; }
        public int CompletionDay { get; set; } = -1;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        public ConstructionProjectDto Clone()
        {
            var p = new ConstructionProjectDto
            {
                ProjectId = ProjectId,
                ProjectType = ProjectType,
                TargetRoomId = TargetRoomId,
                BlueprintId = BlueprintId,
                UpgradeId = UpgradeId,
                GridX = GridX,
                GridY = GridY,
                DepthLevel = DepthLevel,
                LaborRequiredDays = LaborRequiredDays,
                LaborInvestedDays = LaborInvestedDays,
                ConstructionDay = ConstructionDay,
                CompletionDay = CompletionDay,
                Status = Status
            };
            foreach (var kv in ResourceCosts)
            {
                p.ResourceCosts[kv.Key] = kv.Value;
            }
            return p;
        }
    }

    public sealed class ShelterExpansionState
    {
        public int SchemaVersion { get; set; } = 1;
        public double StabilityRating { get; set; } = 100.0;
        public int MaxDepthUnlocked { get; set; } = 1;
        public int TotalRoomsConstructed { get; set; }
        public List<ExpansionRoomDto> Rooms { get; set; } = new();
        public List<ConstructionProjectDto> Projects { get; set; } = new();
    }

    /// <summary>
    /// Pure domain engine governing physical shelter excavation, room renovation,
    /// structural grid expansion, and architectural stability ratings.
    /// Zero engine dependencies; deterministic evaluation.
    /// </summary>
    public sealed class ShelterExpansionSystem
    {
        public const double MinimumSafeStability = 40.0;

        private readonly Dictionary<string, BlueprintDef> _blueprints = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, UpgradeDef> _upgrades = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, ExpansionRoomDto> _rooms = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, ConstructionProjectDto> _projects = new(StringComparer.OrdinalIgnoreCase);

        public double StabilityRating { get; private set; } = 100.0;
        public int MaxDepthUnlocked { get; private set; } = 1;
        public int TotalRoomsConstructed { get; private set; }

        // Seams for host UI / audio / simulation alerts
        public Action<ConstructionProjectDto>? OnProjectStartedSeam { get; set; }
        public Action<ConstructionProjectDto>? OnProjectCompletedSeam { get; set; }
        public Action<ExpansionRoomDto>? OnRoomRenovatedSeam { get; set; }
        public Action<double>? OnStabilityRiskWarningSeam { get; set; }

        public ShelterExpansionSystem()
        {
            LoadEmbeddedDefaults();
            // Seed initial command center room
            _rooms["room_command_hub"] = new ExpansionRoomDto
            {
                RoomId = "room_command_hub",
                RoomTypeId = "room_command",
                Name = "Command Concourse",
                Condition = 100.0,
                Capacity = 6,
                GridX = 0,
                GridY = 0,
                DepthLevel = 1,
                IsConstructed = true
            };
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("blueprints", out var bpElem) && bpElem.ValueKind == JsonValueKind.Array)
                {
                    _blueprints.Clear();
                    foreach (var item in bpElem.EnumerateArray())
                    {
                        var bpId = item.GetProperty("blueprint_id").GetString() ?? string.Empty;
                        var name = item.GetProperty("name").GetString() ?? string.Empty;
                        var roomType = item.GetProperty("room_type_id").GetString() ?? string.Empty;
                        var desc = item.TryGetProperty("description", out var dElem) ? dElem.GetString() ?? string.Empty : string.Empty;
                        var labor = item.TryGetProperty("base_labor_days", out var lElem) ? lElem.GetDouble() : 8.0;
                        var minDepth = item.TryGetProperty("min_depth_level", out var minElem) ? minElem.GetInt32() : 1;
                        var maxDepth = item.TryGetProperty("max_depth_level", out var maxElem) ? maxElem.GetInt32() : 3;
                        var stabilityCost = item.TryGetProperty("stability_cost", out var stabElem) ? stabElem.GetDouble() : 5.0;

                        var costs = new Dictionary<string, int>();
                        if (item.TryGetProperty("resource_costs", out var rcElem))
                        {
                            foreach (var prop in rcElem.EnumerateObject())
                            {
                                costs[prop.Name] = prop.Value.GetInt32();
                            }
                        }

                        _blueprints[bpId] = new BlueprintDef
                        {
                            BlueprintId = bpId,
                            Name = name,
                            RoomTypeId = roomType,
                            Description = desc,
                            BaseLaborDays = labor,
                            MinDepthLevel = minDepth,
                            MaxDepthLevel = maxDepth,
                            StabilityCost = stabilityCost,
                            ResourceCosts = costs
                        };
                    }
                }

                if (root.TryGetProperty("upgrades", out var upgElem) && upgElem.ValueKind == JsonValueKind.Array)
                {
                    _upgrades.Clear();
                    foreach (var item in upgElem.EnumerateArray())
                    {
                        var upgId = item.GetProperty("upgrade_id").GetString() ?? string.Empty;
                        var name = item.GetProperty("name").GetString() ?? string.Empty;
                        var desc = item.TryGetProperty("description", out var dElem) ? dElem.GetString() ?? string.Empty : string.Empty;
                        var labor = item.TryGetProperty("labor_days", out var lElem) ? lElem.GetDouble() : 3.0;
                        var stabRest = item.TryGetProperty("stability_restored", out var sElem) ? sElem.GetDouble() : 0.0;
                        var morale = item.TryGetProperty("morale_bonus", out var mElem) ? mElem.GetInt32() : 2;

                        var costs = new Dictionary<string, int>();
                        if (item.TryGetProperty("resource_costs", out var rcElem))
                        {
                            foreach (var prop in rcElem.EnumerateObject())
                            {
                                costs[prop.Name] = prop.Value.GetInt32();
                            }
                        }

                        _upgrades[upgId] = new UpgradeDef
                        {
                            UpgradeId = upgId,
                            Name = name,
                            Description = desc,
                            LaborDays = labor,
                            StabilityRestored = stabRest,
                            MoraleBonus = morale,
                            ResourceCosts = costs
                        };
                    }
                }
            }
            catch
            {
                LoadEmbeddedDefaults();
            }
        }

        private void LoadEmbeddedDefaults()
        {
            _blueprints.Clear();
            _blueprints["bp_hydroponic_bay"] = new BlueprintDef
            {
                BlueprintId = "bp_hydroponic_bay",
                Name = "Hydroponic Bay Expansion",
                RoomTypeId = "room_hydroponics",
                BaseLaborDays = 8.0,
                MinDepthLevel = 1,
                MaxDepthLevel = 3,
                StabilityCost = 5.0,
                ResourceCosts = new Dictionary<string, int> { ["scrap_metal"] = 12, ["scrap_wood"] = 8 }
            };
            _blueprints["bp_deep_bunkhouse"] = new BlueprintDef
            {
                BlueprintId = "bp_deep_bunkhouse",
                Name = "Reinforced Living Quarters",
                RoomTypeId = "room_deep_dormitory",
                BaseLaborDays = 6.0,
                MinDepthLevel = 1,
                MaxDepthLevel = 4,
                StabilityCost = 4.0,
                ResourceCosts = new Dictionary<string, int> { ["scrap_metal"] = 10, ["scrap_wood"] = 10 }
            };

            _upgrades.Clear();
            _upgrades["upg_structural_pillar"] = new UpgradeDef
            {
                UpgradeId = "upg_structural_pillar",
                Name = "Reinforced Rock Strut",
                LaborDays = 4.0,
                StabilityRestored = 10.0,
                MoraleBonus = 2,
                ResourceCosts = new Dictionary<string, int> { ["scrap_metal"] = 8 }
            };
            _upgrades["upg_thermal_insulation"] = new UpgradeDef
            {
                UpgradeId = "upg_thermal_insulation",
                Name = "Rockwool Insulation Panels",
                LaborDays = 3.0,
                StabilityRestored = 0.0,
                MoraleBonus = 4,
                ResourceCosts = new Dictionary<string, int> { ["scrap_wood"] = 5 }
            };
        }

        public bool IsCellOccupied(int gridX, int gridY, int depthLevel)
        {
            foreach (var r in _rooms.Values)
            {
                if (r.GridX == gridX && r.GridY == gridY && r.DepthLevel == depthLevel)
                    return true;
            }
            foreach (var p in _projects.Values)
            {
                if (p.Status == ProjectStatus.Active && p.ProjectType == ConstructionProjectType.NewRoom &&
                    p.GridX == gridX && p.GridY == gridY && p.DepthLevel == depthLevel)
                    return true;
            }
            return false;
        }

        public ConstructionProjectDto? StartRoomConstruction(
            string blueprintId,
            int gridX,
            int gridY,
            int depthLevel,
            int currentDay)
        {
            if (!_blueprints.TryGetValue(blueprintId, out var bp))
                return null;

            if (depthLevel < bp.MinDepthLevel || depthLevel > bp.MaxDepthLevel || depthLevel > MaxDepthUnlocked)
                return null;

            if (IsCellOccupied(gridX, gridY, depthLevel))
                return null;

            string projectId = $"proj_{currentDay}_{_projects.Count}";
            var project = new ConstructionProjectDto
            {
                ProjectId = projectId,
                ProjectType = ConstructionProjectType.NewRoom,
                BlueprintId = blueprintId,
                GridX = gridX,
                GridY = gridY,
                DepthLevel = depthLevel,
                LaborRequiredDays = bp.BaseLaborDays,
                LaborInvestedDays = 0.0,
                ResourceCosts = new Dictionary<string, int>(bp.ResourceCosts),
                ConstructionDay = currentDay,
                Status = ProjectStatus.Active
            };

            _projects[projectId] = project;
            OnProjectStartedSeam?.Invoke(project);
            return project.Clone();
        }

        public ConstructionProjectDto? StartRenovation(string targetRoomId, int currentDay)
        {
            if (!_rooms.TryGetValue(targetRoomId, out var room))
                return null;

            if (room.Condition >= 99.0)
                return null;

            double conditionDeficit = 100.0 - room.Condition;
            double laborRequired = Math.Max(1.0, Math.Round(conditionDeficit / 25.0, 1));

            string projectId = $"renov_{currentDay}_{_projects.Count}";
            var project = new ConstructionProjectDto
            {
                ProjectId = projectId,
                ProjectType = ConstructionProjectType.Renovation,
                TargetRoomId = targetRoomId,
                LaborRequiredDays = laborRequired,
                LaborInvestedDays = 0.0,
                ResourceCosts = new Dictionary<string, int> { ["scrap_metal"] = 2 },
                ConstructionDay = currentDay,
                Status = ProjectStatus.Active
            };

            _projects[projectId] = project;
            OnProjectStartedSeam?.Invoke(project);
            return project.Clone();
        }

        public ConstructionProjectDto? StartUpgrade(string targetRoomId, string upgradeId, int currentDay)
        {
            if (!_rooms.TryGetValue(targetRoomId, out var room))
                return null;

            if (!_upgrades.TryGetValue(upgradeId, out var upg))
                return null;

            if (room.Upgrades.Contains(upgradeId))
                return null;

            string projectId = $"upg_{currentDay}_{_projects.Count}";
            var project = new ConstructionProjectDto
            {
                ProjectId = projectId,
                ProjectType = ConstructionProjectType.Upgrade,
                TargetRoomId = targetRoomId,
                UpgradeId = upgradeId,
                LaborRequiredDays = upg.LaborDays,
                LaborInvestedDays = 0.0,
                ResourceCosts = new Dictionary<string, int>(upg.ResourceCosts),
                ConstructionDay = currentDay,
                Status = ProjectStatus.Active
            };

            _projects[projectId] = project;
            OnProjectStartedSeam?.Invoke(project);
            return project.Clone();
        }

        public ConstructionProjectDto StartDepthExcavation(int currentDay)
        {
            int nextDepth = MaxDepthUnlocked + 1;
            string projectId = $"shaft_lv{nextDepth}";
            var project = new ConstructionProjectDto
            {
                ProjectId = projectId,
                ProjectType = ConstructionProjectType.DepthExcavation,
                DepthLevel = nextDepth,
                LaborRequiredDays = 12.0,
                LaborInvestedDays = 0.0,
                ResourceCosts = new Dictionary<string, int> { ["scrap_metal"] = 20, ["concrete_rubble"] = 10 },
                ConstructionDay = currentDay,
                Status = ProjectStatus.Active
            };

            _projects[projectId] = project;
            OnProjectStartedSeam?.Invoke(project);
            return project.Clone();
        }

        public bool ProgressProject(string projectId, double dailyLabor, int currentDay)
        {
            if (!_projects.TryGetValue(projectId, out var project))
                return false;

            if (project.Status != ProjectStatus.Active)
                return false;

            project.LaborInvestedDays += Math.Max(0.0, dailyLabor);
            if (project.LaborInvestedDays >= project.LaborRequiredDays)
            {
                CompleteProject(project, currentDay);
                return true;
            }
            return false;
        }

        private void CompleteProject(ConstructionProjectDto project, int currentDay)
        {
            project.Status = ProjectStatus.Completed;
            project.CompletionDay = currentDay;

            switch (project.ProjectType)
            {
                case ConstructionProjectType.NewRoom:
                    if (_blueprints.TryGetValue(project.BlueprintId, out var bp))
                    {
                        string roomId = $"room_{project.GridX}_{project.GridY}_d{project.DepthLevel}";
                        var newRoom = new ExpansionRoomDto
                        {
                            RoomId = roomId,
                            RoomTypeId = bp.RoomTypeId,
                            Name = bp.Name,
                            Condition = 100.0,
                            GridX = project.GridX,
                            GridY = project.GridY,
                            DepthLevel = project.DepthLevel,
                            IsConstructed = true
                        };
                        _rooms[roomId] = newRoom;
                        TotalRoomsConstructed++;

                        // Excavation stability cost
                        AdjustStability(-bp.StabilityCost);
                    }
                    break;

                case ConstructionProjectType.Renovation:
                    if (_rooms.TryGetValue(project.TargetRoomId, out var renRoom))
                    {
                        renRoom.Condition = 100.0;
                        OnRoomRenovatedSeam?.Invoke(renRoom);
                    }
                    break;

                case ConstructionProjectType.Upgrade:
                    if (_rooms.TryGetValue(project.TargetRoomId, out var upgRoom) &&
                        _upgrades.TryGetValue(project.UpgradeId, out var upgDef))
                    {
                        if (!upgRoom.Upgrades.Contains(project.UpgradeId))
                        {
                            upgRoom.Upgrades.Add(project.UpgradeId);
                        }
                        if (upgDef.StabilityRestored > 0)
                        {
                            AdjustStability(upgDef.StabilityRestored);
                        }
                    }
                    break;

                case ConstructionProjectType.DepthExcavation:
                    MaxDepthUnlocked = Math.Max(MaxDepthUnlocked, project.DepthLevel);
                    AdjustStability(-10.0);
                    break;
            }

            OnProjectCompletedSeam?.Invoke(project);
        }

        public void DegradeRoomCondition(string roomId, double wearAmount)
        {
            if (_rooms.TryGetValue(roomId, out var room))
            {
                room.Condition = Math.Clamp(room.Condition - Math.Max(0.0, wearAmount), 0.0, 100.0);
            }
        }

        public void AdjustStability(double delta)
        {
            double old = StabilityRating;
            StabilityRating = Math.Clamp(StabilityRating + delta, 0.0, 100.0);

            if (StabilityRating < MinimumSafeStability && old >= MinimumSafeStability)
            {
                OnStabilityRiskWarningSeam?.Invoke(StabilityRating);
            }
        }

        public ExpansionRoomDto? GetRoom(string roomId)
        {
            if (_rooms.TryGetValue(roomId, out var r))
                return r.Clone();
            return null;
        }

        public IReadOnlyList<ExpansionRoomDto> GetAllRooms()
        {
            var list = new List<ExpansionRoomDto>(_rooms.Count);
            foreach (var r in _rooms.Values)
            {
                list.Add(r.Clone());
            }
            return list;
        }

        public ConstructionProjectDto? GetProject(string projectId)
        {
            if (_projects.TryGetValue(projectId, out var p))
                return p.Clone();
            return null;
        }

        public IReadOnlyList<ConstructionProjectDto> GetAllProjects()
        {
            var list = new List<ConstructionProjectDto>(_projects.Count);
            foreach (var p in _projects.Values)
            {
                list.Add(p.Clone());
            }
            return list;
        }

        public ShelterExpansionState CaptureState()
        {
            var state = new ShelterExpansionState
            {
                SchemaVersion = 1,
                StabilityRating = StabilityRating,
                MaxDepthUnlocked = MaxDepthUnlocked,
                TotalRoomsConstructed = TotalRoomsConstructed
            };
            foreach (var r in _rooms.Values)
            {
                state.Rooms.Add(r.Clone());
            }
            foreach (var p in _projects.Values)
            {
                state.Projects.Add(p.Clone());
            }
            return state;
        }

        public void RestoreState(ShelterExpansionState state)
        {
            if (state == null)
                return;

            StabilityRating = Math.Clamp(state.StabilityRating, 0.0, 100.0);
            MaxDepthUnlocked = Math.Max(1, state.MaxDepthUnlocked);
            TotalRoomsConstructed = Math.Max(0, state.TotalRoomsConstructed);

            _rooms.Clear();
            if (state.Rooms != null)
            {
                foreach (var r in state.Rooms)
                {
                    _rooms[r.RoomId] = r.Clone();
                }
            }

            _projects.Clear();
            if (state.Projects != null)
            {
                foreach (var p in state.Projects)
                {
                    _projects[p.ProjectId] = p.Clone();
                }
            }
        }
    }
}
