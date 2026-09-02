// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 201 — Advanced Robotics & Pre-War AI System
// Subsystem    : Autonomous Mechanical Units, Labor Automation & Rogue AI Risks
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Crafting
{
    [Serializable]
    public sealed class RobotMaterialRequirement
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;
    }

    [Serializable]
    public sealed class RobotDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public string Role { get; set; } = "utility";

        [JsonPropertyName("armor_rating")]
        public float ArmorRating { get; set; } = 0.2f;

        [JsonPropertyName("max_chassis_integrity")]
        public int MaxChassisIntegrity { get; set; } = 1000;

        [JsonPropertyName("max_core_charge_wh")]
        public int MaxCoreChargeWh { get; set; } = 5000;

        [JsonPropertyName("labor_drain_w")]
        public int LaborDrainW { get; set; } = 250;

        [JsonPropertyName("charging_rate_w")]
        public int ChargingRateW { get; set; } = 500;

        [JsonPropertyName("emp_disable_hours")]
        public int EmpDisableHours { get; set; } = 24;

        [JsonPropertyName("reactivation_materials")]
        public List<RobotMaterialRequirement> ReactivationMaterials { get; set; } = new List<RobotMaterialRequirement>();

        [JsonPropertyName("compatible_tasks")]
        public List<string> CompatibleTasks { get; set; } = new List<string>();

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RobotUnitState
    {
        public string UnitId { get; set; } = string.Empty;
        public string DefinitionId { get; set; } = string.Empty;
        public int ChassisIntegrity { get; set; } = 1000;
        public int LogicIntegrity { get; set; } = 1000;
        public int CoreChargeWh { get; set; } = 2500;
        public string AssignedDirective { get; set; } = "directive_idle";
        public string AssignedTask { get; set; } = string.Empty;
        public bool IsEmpDisabled { get; set; }
        public int EmpDisableHoursRemaining { get; set; }
        public bool IsRogue { get; set; }

        public RobotUnitState Clone() => new RobotUnitState
        {
            UnitId = UnitId,
            DefinitionId = DefinitionId,
            ChassisIntegrity = ChassisIntegrity,
            LogicIntegrity = LogicIntegrity,
            CoreChargeWh = CoreChargeWh,
            AssignedDirective = AssignedDirective,
            AssignedTask = AssignedTask,
            IsEmpDisabled = IsEmpDisabled,
            EmpDisableHoursRemaining = EmpDisableHoursRemaining,
            IsRogue = IsRogue
        };
    }

    [Serializable]
    public sealed class RoboticsSaveState
    {
        public string SystemId { get; set; } = RoboticsSystem.SystemId;
        public List<RobotUnitState> Units { get; set; } = new List<RobotUnitState>();
        public int TotalUnitsReactivated { get; set; }
        public int TotalRogueEventsTriggered { get; set; }
    }

    /// <summary>
    /// Engine-agnostic advanced robotics and pre-war automaton system.
    /// Manages reactivatable mechanical units, directive hacking, power core charge drain,
    /// automated shelter labor shifts, EMP vulnerability, and rogue AI instability.
    /// </summary>
    public sealed class RoboticsSystem
    {
        public const string SystemId = "robotics_system";

        private readonly Dictionary<string, RobotDefinition> _robotCatalog =
            new Dictionary<string, RobotDefinition>(StringComparer.Ordinal);

        private RoboticsSaveState _state = new RoboticsSaveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<RobotUnitState>? OnRobotReactivated;
        public event Action<string, string>? OnDirectiveChanged;        // unitId, directiveId
        public event Action<string, int>? OnRobotEmpDisabled;           // unitId, hours
        public event Action<RobotUnitState>? OnRogueEventTriggered;
        public event Action? OnStateChanged;

        public RoboticsSaveState State => _state;
        public IReadOnlyDictionary<string, RobotDefinition> RobotCatalog => _robotCatalog;
        public IReadOnlyList<RobotUnitState> Units => _state.Units;

        public RoboticsSystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("robots", out var robotsEl) && robotsEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in robotsEl.EnumerateArray())
                {
                    var r = JsonSerializer.Deserialize<RobotDefinition>(el.GetRawText());
                    if (r != null && !string.IsNullOrEmpty(r.Id))
                    {
                        _robotCatalog[r.Id] = r;
                    }
                }
            }
        }

        public RobotUnitState? ReactivateRobot(string definitionId, float programmerSkill01, out string error)
        {
            error = string.Empty;
            if (!_robotCatalog.TryGetValue(definitionId, out var def))
            {
                error = $"Unknown robot archetype: {definitionId}";
                return null;
            }

            programmerSkill01 = Math.Clamp(programmerSkill01, 0f, 1f);
            _state.TotalUnitsReactivated++;
            string unitId = $"robot_{def.Role}_{_state.TotalUnitsReactivated:D3}";

            // Starting logic integrity scales with programmer skill (600..1000 permille)
            int logic = (int)(600 + (programmerSkill01 * 400));

            var unit = new RobotUnitState
            {
                UnitId = unitId,
                DefinitionId = definitionId,
                ChassisIntegrity = def.MaxChassisIntegrity,
                LogicIntegrity = Math.Clamp(logic, 100, 1000),
                CoreChargeWh = def.MaxCoreChargeWh / 2, // 50% initial battery
                AssignedDirective = "directive_idle",
                AssignedTask = string.Empty,
                IsEmpDisabled = false,
                EmpDisableHoursRemaining = 0,
                IsRogue = false
            };

            _state.Units.Add(unit);
            OnRobotReactivated?.Invoke(unit);
            OnStateChanged?.Invoke();
            return unit;
        }

        public bool ProgramDirective(string unitId, string directiveId, float programmerSkill01, out string error)
        {
            error = string.Empty;
            var unit = _state.Units.Find(u => u.UnitId == unitId);
            if (unit == null)
            {
                error = $"Robot unit {unitId} not found.";
                return false;
            }

            if (unit.IsEmpDisabled)
            {
                error = "Cannot program directives while unit is EMP-disabled.";
                return false;
            }

            if (unit.IsRogue)
            {
                error = "Unit logic core is rogue; manual directives rejected.";
                return false;
            }

            programmerSkill01 = Math.Clamp(programmerSkill01, 0f, 1f);

            // If logic integrity is fragile (< 300) and programmer skill is low, risk logic corruption
            if (unit.LogicIntegrity < 300 && programmerSkill01 < 0.25f && _rng.NextDouble() < 0.35)
            {
                unit.IsRogue = true;
                _state.TotalRogueEventsTriggered++;
                OnRogueEventTriggered?.Invoke(unit);
                error = "Logic core corruption occurred during programming; unit turned rogue!";
                OnStateChanged?.Invoke();
                return false;
            }

            unit.AssignedDirective = directiveId ?? "directive_idle";
            OnDirectiveChanged?.Invoke(unitId, unit.AssignedDirective);
            OnStateChanged?.Invoke();
            return true;
        }

        public void ApplyEmpShock(int disableHours)
        {
            if (disableHours <= 0) return;

            foreach (var unit in _state.Units)
            {
                int duration = disableHours;
                unit.IsEmpDisabled = true;
                unit.EmpDisableHoursRemaining = duration;
                OnRobotEmpDisabled?.Invoke(unit.UnitId, duration);
            }

            OnStateChanged?.Invoke();
        }

        public void TickLabor(int hours, bool isDockedToGrid, float gridPowerAvailableWatts)
        {
            if (hours <= 0 || _state.Units.Count == 0) return;

            foreach (var unit in _state.Units)
            {
                // Process EMP recovery
                if (unit.IsEmpDisabled)
                {
                    unit.EmpDisableHoursRemaining = Math.Max(0, unit.EmpDisableHoursRemaining - hours);
                    if (unit.EmpDisableHoursRemaining == 0)
                    {
                        unit.IsEmpDisabled = false;
                    }
                    continue;
                }

                if (!_robotCatalog.TryGetValue(unit.DefinitionId, out var def)) continue;

                // Charging when docked
                if (isDockedToGrid && gridPowerAvailableWatts >= def.ChargingRateW && unit.CoreChargeWh < def.MaxCoreChargeWh)
                {
                    int chargeAmount = def.ChargingRateW * hours;
                    unit.CoreChargeWh = Math.Min(def.MaxCoreChargeWh, unit.CoreChargeWh + chargeAmount);
                }

                // Labor power drain when assigned to non-idle directive
                if (!string.Equals(unit.AssignedDirective, "directive_idle", StringComparison.OrdinalIgnoreCase) && !unit.IsRogue)
                {
                    int drain = def.LaborDrainW * hours;
                    unit.CoreChargeWh = Math.Max(0, unit.CoreChargeWh - drain);

                    // Mechanical chassis wear: 2 permille per 8h labor
                    int wear = (int)(2f * (hours / 8f) * 2);
                    unit.ChassisIntegrity = Math.Max(0, unit.ChassisIntegrity - Math.Max(1, wear));

                    // If battery exhausted, falls back to idle
                    if (unit.CoreChargeWh <= 0)
                    {
                        unit.AssignedDirective = "directive_idle";
                        OnDirectiveChanged?.Invoke(unit.UnitId, "directive_idle");
                    }
                }

                // Rogue AI instability evaluation if logic integrity degraded
                if (!unit.IsRogue && unit.LogicIntegrity < 250 && _rng.NextDouble() < 0.05)
                {
                    unit.IsRogue = true;
                    _state.TotalRogueEventsTriggered++;
                    OnRogueEventTriggered?.Invoke(unit);
                }
            }

            OnStateChanged?.Invoke();
        }

        public bool RepairRobot(string unitId, int integrityRestored)
        {
            var unit = _state.Units.Find(u => u.UnitId == unitId);
            if (unit == null || integrityRestored <= 0) return false;

            int maxChassis = 1000;
            if (_robotCatalog.TryGetValue(unit.DefinitionId, out var def))
            {
                maxChassis = def.MaxChassisIntegrity;
            }

            unit.ChassisIntegrity = Math.Min(maxChassis, unit.ChassisIntegrity + integrityRestored);
            OnStateChanged?.Invoke();
            return true;
        }

        // ── Save / Restore ──────────────────────────────────────────────────

        public RoboticsSaveState CaptureState()
        {
            var copy = new RoboticsSaveState
            {
                SystemId = _state.SystemId,
                TotalUnitsReactivated = _state.TotalUnitsReactivated,
                TotalRogueEventsTriggered = _state.TotalRogueEventsTriggered,
                Units = new List<RobotUnitState>(_state.Units.Count)
            };

            foreach (var u in _state.Units)
            {
                copy.Units.Add(u.Clone());
            }

            return copy;
        }

        public void RestoreState(RoboticsSaveState? state)
        {
            if (state == null)
            {
                _state = new RoboticsSaveState();
                return;
            }

            _state = new RoboticsSaveState
            {
                SystemId = state.SystemId ?? SystemId,
                TotalUnitsReactivated = state.TotalUnitsReactivated,
                TotalRogueEventsTriggered = state.TotalRogueEventsTriggered,
                Units = new List<RobotUnitState>()
            };

            if (state.Units != null)
            {
                foreach (var u in state.Units)
                {
                    if (u != null)
                    {
                        _state.Units.Add(u.Clone());
                    }
                }
            }

            OnStateChanged?.Invoke();
        }
    }
}
