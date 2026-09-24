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
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.Inventory;

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

    public enum ConstructionStartCode
    {
        Started,
        UnknownBlueprint,
        UnknownRoom,
        UnknownUpgrade,
        InvalidDepth,
        DepthLocked,
        OccupiedCell,
        UnsafeStability,
        AlreadyUpgraded,
        DuplicateProject,
        InvalidTarget,
        CostConsumerUnavailable,
        InsufficientResources
    }

    public sealed class ConstructionStartResult
    {
        public bool Succeeded { get; }
        public ConstructionStartCode Code { get; }
        public ConstructionProjectDto? Project { get; }

        public ConstructionStartResult(
            bool succeeded,
            ConstructionStartCode code,
            ConstructionProjectDto? project = null)
        {
            Succeeded = succeeded;
            Code = code;
            Project = project?.Clone();
        }
    }

    public sealed class BlueprintDef
    {
        [JsonPropertyName("blueprint_id")]
        public string BlueprintId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("room_type_id")]
        public string RoomTypeId { get; set; } = string.Empty;

        [JsonPropertyName("canonical_room_id")]
        public string CanonicalRoomId { get; set; } = string.Empty;

        [JsonPropertyName("capacity_bonus")]
        public int CapacityBonus { get; set; }

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

    public sealed class ShelterCrewRules
    {
        [JsonPropertyName("max_crew_per_project")]
        public int MaxCrewPerProject { get; set; } = 3;

        [JsonPropertyName("skill_id")]
        public string SkillId { get; set; } = "skill_crafting";

        [JsonPropertyName("fatigue_per_day")]
        public float FatiguePerDay { get; set; } = 2.0f;
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
        public string CanonicalRoomId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Condition { get; set; } = 100.0; // 0..100
        public List<string> Upgrades { get; set; } = new();
        public List<string> ConnectedRoomIds { get; set; } = new();
        public int Capacity { get; set; } = 4;
        public int CapacityBonus { get; set; }
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
                CanonicalRoomId = CanonicalRoomId,
                Name = Name,
                Condition = Condition,
                Capacity = Capacity,
                CapacityBonus = CapacityBonus,
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
        public List<string> CrewSurvivorIds { get; set; } = new();

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
                Status = Status,
                CrewSurvivorIds = CrewSurvivorIds == null
                    ? new List<string>()
                    : new List<string>(CrewSurvivorIds)
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
        public int LastCrewProgressDay { get; set; }
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
        private ShelterCrewRules _crewRules = new();

        public double StabilityRating { get; private set; } = 100.0;
        public int MaxDepthUnlocked { get; private set; } = 1;
        public int TotalRoomsConstructed { get; private set; }
        public int LastCrewProgressDay { get; private set; }
        public ShelterCrewRules CrewRules => new ShelterCrewRules
        {
            MaxCrewPerProject = _crewRules.MaxCrewPerProject,
            SkillId = _crewRules.SkillId,
            FatiguePerDay = _crewRules.FatiguePerDay
        };

        public IReadOnlyDictionary<string, BlueprintDef> Blueprints => _blueprints;
        public IReadOnlyDictionary<string, UpgradeDef> Upgrades => _upgrades;

        // Seams for host UI / audio / simulation alerts
        public Action<ConstructionProjectDto>? OnProjectStartedSeam { get; set; }
        public Action<ConstructionProjectDto>? OnProjectCompletedSeam { get; set; }
        public Action<ExpansionRoomDto>? OnRoomRenovatedSeam { get; set; }
        public Action<double>? OnStabilityRiskWarningSeam { get; set; }
        public Func<string, bool>? IsCrewSurvivorEligible { get; set; }
        public Func<string, string, float>? ResolveCrewSkillBonus { get; set; }
        public Action<string, string, float, int>? RecordCrewSkillPractice { get; set; }
        public Action<string, float>? ApplyCrewFatigue { get; set; }
        public Action<string, float>? ApplySurvivorMoraleDelta { get; set; }

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
                        var canonicalRoomId = item.TryGetProperty("canonical_room_id", out var roomIdElem)
                            ? roomIdElem.GetString() ?? string.Empty : string.Empty;
                        var capacityBonus = item.TryGetProperty("capacity_bonus", out var capacityElem)
                            ? capacityElem.GetInt32() : 0;
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
                            CanonicalRoomId = canonicalRoomId,
                            CapacityBonus = Math.Max(0, capacityBonus),
                            Description = desc,
                            BaseLaborDays = labor,
                            MinDepthLevel = minDepth,
                            MaxDepthLevel = maxDepth,
                            StabilityCost = stabilityCost,
                            ResourceCosts = costs
                        };
                    }
                }

                if (root.TryGetProperty("crew_rules", out var crewElem)
                    && crewElem.ValueKind == JsonValueKind.Object)
                {
                    _crewRules = new ShelterCrewRules
                    {
                        MaxCrewPerProject = crewElem.TryGetProperty("max_crew_per_project", out var maxCrewElem)
                            ? Math.Max(1, maxCrewElem.GetInt32()) : 3,
                        SkillId = crewElem.TryGetProperty("skill_id", out var skillElem)
                            ? skillElem.GetString() ?? "skill_crafting" : "skill_crafting",
                        FatiguePerDay = crewElem.TryGetProperty("fatigue_per_day", out var fatigueElem)
                            ? Math.Max(0f, fatigueElem.GetSingle()) : 2.0f
                    };
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
                CanonicalRoomId = "room_greenhouse_shelter",
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
                CanonicalRoomId = "room_bunks",
                CapacityBonus = 4,
                BaseLaborDays = 6.0,
                MinDepthLevel = 1,
                MaxDepthLevel = 4,
                StabilityCost = 4.0,
                ResourceCosts = new Dictionary<string, int> { ["scrap_metal"] = 10, ["scrap_wood"] = 10 }
            };
            _blueprints["bp_deep_shaft_expansion"] = new BlueprintDef
            {
                BlueprintId = "bp_deep_shaft_expansion",
                Name = "Sub-Level Shaft Excavation",
                RoomTypeId = "room_shaft_stairwell",
                CanonicalRoomId = "room_bunker_corridor",
                BaseLaborDays = 14.0,
                MinDepthLevel = 0,
                MaxDepthLevel = 5,
                StabilityCost = 12.0,
                ResourceCosts = new Dictionary<string, int>
                {
                    ["scrap_metal"] = 25,
                    ["concrete_rubble"] = 15,
                    ["wooden_plank"] = 6
                }
            };

            _crewRules = new ShelterCrewRules();

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

        public ConstructionStartResult TryStartRoomConstruction(
            string blueprintId,
            int gridX,
            int gridY,
            int depthLevel,
            int currentDay,
            IPlayerInventoryPort? inventory)
        {
            if (!_blueprints.TryGetValue(blueprintId ?? string.Empty, out var bp))
                return Failed(ConstructionStartCode.UnknownBlueprint);
            if (depthLevel < bp.MinDepthLevel || depthLevel > bp.MaxDepthLevel)
                return Failed(ConstructionStartCode.InvalidDepth);
            if (depthLevel > MaxDepthUnlocked)
                return Failed(ConstructionStartCode.DepthLocked);
            if (IsCellOccupied(gridX, gridY, depthLevel))
                return Failed(ConstructionStartCode.OccupiedCell);
            if (StabilityRating - bp.StabilityCost < MinimumSafeStability)
                return Failed(ConstructionStartCode.UnsafeStability);
            if (HasActiveTargetProject(ConstructionProjectType.NewRoom, gridX, gridY, depthLevel))
                return Failed(ConstructionStartCode.DuplicateProject);
            if (inventory == null)
                return Failed(ConstructionStartCode.CostConsumerUnavailable);

            var project = NewProject("build", currentDay, ConstructionProjectType.NewRoom);
            project.BlueprintId = bp.BlueprintId;
            project.GridX = gridX;
            project.GridY = gridY;
            project.DepthLevel = depthLevel;
            project.LaborRequiredDays = bp.BaseLaborDays;
            project.ResourceCosts = new Dictionary<string, int>(bp.ResourceCosts);
            project.ConstructionDay = currentDay;
            return CommitProject(project, inventory);
        }

        public ConstructionStartResult TryStartRenovation(
            string targetRoomId,
            int currentDay,
            IPlayerInventoryPort? inventory)
        {
            if (!_rooms.TryGetValue(targetRoomId ?? string.Empty, out var room))
                return Failed(ConstructionStartCode.UnknownRoom);
            if (room.Condition >= 99.0)
                return Failed(ConstructionStartCode.InvalidTarget);
            if (HasActiveRoomProject(targetRoomId))
                return Failed(ConstructionStartCode.DuplicateProject);
            if (inventory == null)
                return Failed(ConstructionStartCode.CostConsumerUnavailable);

            double conditionDeficit = 100.0 - room.Condition;
            var project = NewProject("renov", currentDay, ConstructionProjectType.Renovation);
            project.TargetRoomId = targetRoomId;
            project.LaborRequiredDays = Math.Max(1.0, Math.Round(conditionDeficit / 25.0, 1));
            project.ResourceCosts = new Dictionary<string, int> { ["scrap_metal"] = 2 };
            project.ConstructionDay = currentDay;
            return CommitProject(project, inventory);
        }

        public ConstructionStartResult TryStartUpgrade(
            string targetRoomId,
            string upgradeId,
            int currentDay,
            IPlayerInventoryPort? inventory)
        {
            if (!_rooms.TryGetValue(targetRoomId ?? string.Empty, out var room))
                return Failed(ConstructionStartCode.UnknownRoom);
            if (!_upgrades.TryGetValue(upgradeId ?? string.Empty, out var upgrade))
                return Failed(ConstructionStartCode.UnknownUpgrade);
            if (room.Upgrades.Contains(upgradeId))
                return Failed(ConstructionStartCode.AlreadyUpgraded);
            if (HasActiveRoomProject(targetRoomId))
                return Failed(ConstructionStartCode.DuplicateProject);
            if (inventory == null)
                return Failed(ConstructionStartCode.CostConsumerUnavailable);

            var project = NewProject("upg", currentDay, ConstructionProjectType.Upgrade);
            project.TargetRoomId = targetRoomId;
            project.UpgradeId = upgradeId;
            project.LaborRequiredDays = upgrade.LaborDays;
            project.ResourceCosts = new Dictionary<string, int>(upgrade.ResourceCosts);
            project.ConstructionDay = currentDay;
            return CommitProject(project, inventory);
        }

        public ConstructionStartResult TryStartDepthExcavation(
            int currentDay,
            IPlayerInventoryPort? inventory)
        {
            if (!_blueprints.TryGetValue("bp_deep_shaft_expansion", out var blueprint))
                return Failed(ConstructionStartCode.UnknownBlueprint);
            int nextDepth = MaxDepthUnlocked + 1;
            if (nextDepth > blueprint.MaxDepthLevel)
                return Failed(ConstructionStartCode.DepthLocked);
            if (StabilityRating - blueprint.StabilityCost < MinimumSafeStability)
                return Failed(ConstructionStartCode.UnsafeStability);
            if (HasActiveDepthExcavation(nextDepth))
                return Failed(ConstructionStartCode.DuplicateProject);
            if (inventory == null)
                return Failed(ConstructionStartCode.CostConsumerUnavailable);

            var project = NewProject($"shaft_lv{nextDepth}", currentDay, ConstructionProjectType.DepthExcavation);
            project.BlueprintId = blueprint.BlueprintId;
            project.DepthLevel = nextDepth;
            project.LaborRequiredDays = blueprint.BaseLaborDays;
            project.ResourceCosts = new Dictionary<string, int>(blueprint.ResourceCosts);
            project.ConstructionDay = currentDay;
            return CommitProject(project, inventory);
        }

        public bool TryAssignCrew(string projectId, string survivorId)
        {
            if (string.IsNullOrWhiteSpace(projectId) || string.IsNullOrWhiteSpace(survivorId)
                || !_projects.TryGetValue(projectId, out var project)
                || project.Status != ProjectStatus.Active
                || (IsCrewSurvivorEligible != null && !IsCrewSurvivorEligible(survivorId)))
                return false;

            foreach (var existing in _projects.Values)
                if (existing.Status == ProjectStatus.Active
                    && existing.CrewSurvivorIds.Contains(survivorId, StringComparer.Ordinal))
                    return false;
            if (project.CrewSurvivorIds.Count >= _crewRules.MaxCrewPerProject
                || project.CrewSurvivorIds.Contains(survivorId, StringComparer.Ordinal))
                return false;

            project.CrewSurvivorIds.Add(survivorId);
            project.CrewSurvivorIds.Sort(StringComparer.Ordinal);
            return true;
        }

        public bool RelieveCrew(string projectId, string survivorId)
        {
            if (string.IsNullOrWhiteSpace(projectId) || string.IsNullOrWhiteSpace(survivorId)
                || !_projects.TryGetValue(projectId, out var project))
                return false;
            return project.CrewSurvivorIds.Remove(survivorId);
        }

        public int ProgressAssignedProjects(int currentDay)
        {
            if (currentDay <= LastCrewProgressDay || currentDay <= 0)
                return 0;
            LastCrewProgressDay = currentDay;
            var projects = new List<ConstructionProjectDto>(_projects.Values);
            projects.Sort((left, right) => StringComparer.Ordinal.Compare(left.ProjectId, right.ProjectId));
            int completed = 0;
            foreach (var project in projects)
            {
                if (project.Status != ProjectStatus.Active || project.CrewSurvivorIds.Count == 0)
                    continue;

                double labor = 0.0;
                var crew = new List<string>(project.CrewSurvivorIds);
                crew.Sort(StringComparer.Ordinal);
                foreach (string survivorId in crew)
                {
                    float bonus = ResolveCrewSkillBonus?.Invoke(survivorId, _crewRules.SkillId) ?? 0f;
                    bonus = float.IsNaN(bonus) || float.IsInfinity(bonus) ? 0f : Math.Clamp(bonus, 0f, 1f);
                    labor += 1.0 + bonus;
                    RecordCrewSkillPractice?.Invoke(survivorId, _crewRules.SkillId, 1f, currentDay);
                    ApplyCrewFatigue?.Invoke(survivorId, _crewRules.FatiguePerDay);
                }

                if (ProgressProject(project.ProjectId, labor, currentDay))
                    completed++;
            }
            return completed;
        }

        private ConstructionStartResult CommitProject(
            ConstructionProjectDto project,
            IPlayerInventoryPort inventory)
        {
            if (!inventory.TryConsumeBill(project.ResourceCosts))
                return Failed(ConstructionStartCode.InsufficientResources);

            _projects.Add(project.ProjectId, project);
            OnProjectStartedSeam?.Invoke(project);
            return new ConstructionStartResult(true, ConstructionStartCode.Started, project);
        }

        private ConstructionProjectDto NewProject(
            string prefix,
            int currentDay,
            ConstructionProjectType type)
        {
            string projectId = $"{prefix}_{currentDay}_{_projects.Count}";
            int suffix = _projects.Count;
            while (_projects.ContainsKey(projectId))
                projectId = $"{prefix}_{currentDay}_{++suffix}";
            return new ConstructionProjectDto
            {
                ProjectId = projectId,
                ProjectType = type,
                Status = ProjectStatus.Active
            };
        }

        private bool HasActiveTargetProject(ConstructionProjectType type, int gridX, int gridY, int depthLevel)
        {
            foreach (var project in _projects.Values)
                if (project.Status == ProjectStatus.Active && project.ProjectType == type
                    && project.GridX == gridX && project.GridY == gridY && project.DepthLevel == depthLevel)
                    return true;
            return false;
        }

        private bool HasActiveRoomProject(string roomId)
        {
            foreach (var project in _projects.Values)
                if (project.Status == ProjectStatus.Active
                    && string.Equals(project.TargetRoomId, roomId, StringComparison.Ordinal))
                    return true;
            return false;
        }

        private bool HasActiveDepthExcavation(int depthLevel)
        {
            foreach (var project in _projects.Values)
                if (project.Status == ProjectStatus.Active
                    && project.ProjectType == ConstructionProjectType.DepthExcavation
                    && project.DepthLevel == depthLevel)
                    return true;
            return false;
        }

        private static ConstructionStartResult Failed(ConstructionStartCode code)
            => new(false, code);

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
            foreach (var activeProject in _projects.Values)
                if (activeProject.Status == ProjectStatus.Active
                    && activeProject.ProjectType == ConstructionProjectType.DepthExcavation
                    && activeProject.DepthLevel == nextDepth)
                    return activeProject.Clone();

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
                            CanonicalRoomId = bp.CanonicalRoomId,
                            Name = bp.Name,
                            Condition = 100.0,
                            CapacityBonus = bp.CapacityBonus,
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
                    if (_blueprints.TryGetValue(project.BlueprintId, out var shaftBlueprint))
                        AdjustStability(-shaftBlueprint.StabilityCost);
                    else
                        AdjustStability(-10.0);
                    break;
            }

            if (project.ProjectType == ConstructionProjectType.Upgrade
                && _upgrades.TryGetValue(project.UpgradeId, out var completedUpgrade)
                && completedUpgrade.MoraleBonus != 0)
            {
                foreach (string survivorId in project.CrewSurvivorIds)
                    ApplySurvivorMoraleDelta?.Invoke(survivorId, completedUpgrade.MoraleBonus);
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

        public IReadOnlyDictionary<string, int> GetCompletedCapacityBonuses()
        {
            var bonuses = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var room in _rooms.Values)
            {
                if (!room.IsConstructed || room.CapacityBonus <= 0
                    || string.IsNullOrWhiteSpace(room.CanonicalRoomId))
                    continue;
                bonuses.TryGetValue(room.CanonicalRoomId, out int total);
                bonuses[room.CanonicalRoomId] = checked(total + room.CapacityBonus);
            }
            return bonuses;
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
                TotalRoomsConstructed = TotalRoomsConstructed,
                LastCrewProgressDay = LastCrewProgressDay
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
            LastCrewProgressDay = Math.Max(0, state.LastCrewProgressDay);

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
