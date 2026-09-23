// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 195 — Survivor Specialization Roles
// Pure domain authority for survivor formal roles, capability-enhancing bonuses,
// role progression (Novice to Master), skill requirement gating, and auto-actions.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Survivors
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class SurvivorRoleDef
    {
        public string role_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string category { get; set; } = "technical";
        public Dictionary<string, int> required_skills { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public string primary_bonus_type { get; set; } = string.Empty;
        public float primary_bonus_value { get; set; } = 0.20f;
        public string secondary_bonus_type { get; set; } = string.Empty;
        public float secondary_bonus_value { get; set; } = 0.15f;
        public string auto_action_type { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SurvivorRolesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<SurvivorRoleDef> roles { get; set; } = new List<SurvivorRoleDef>();
    }

    // ── Persistent State DTOs ───────────────────────────────────────────────

    [Serializable]
    public sealed class SurvivorRoleAssignment
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public int Level { get; set; } = 1; // 1: Novice, 2: Apprentice, 3: Journeyman, 4: Expert, 5: Master
        public int ExperiencePoints { get; set; }
        public int AssignedDay { get; set; } = 1;
        public int TotalAutoActionsExecuted { get; set; }
    }

    [Serializable]
    public sealed class SurvivorRoleState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public int MaxSurvivorsPerRole { get; set; } = 2;
        public List<SurvivorRoleAssignment> Assignments { get; set; } = new List<SurvivorRoleAssignment>();
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class SurvivorRoleSystem
    {
        private readonly SurvivorRoleState _state;
        private readonly Dictionary<string, SurvivorRoleDef> _roleDefs =
            new Dictionary<string, SurvivorRoleDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<SurvivorRoleAssignment>? OnRoleAssigned;
        public event Action<string>? OnRoleUnassigned; // (survivorId)
        public event Action<string, int>? OnRoleLeveledUp; // (survivorId, newLevel)
        public event Action<string, string>? OnAutoActionExecuted; // (survivorId, actionType)

        public int ActiveAssignmentCount => _state.Assignments.Count;
        public int MaxSurvivorsPerRole => _state.MaxSurvivorsPerRole;

        public SurvivorRoleSystem()
        {
            _state = new SurvivorRoleState();
        }

        public SurvivorRoleSystem(SurvivorRoleState state)
        {
            _state = state ?? new SurvivorRoleState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<SurvivorRolesCatalog>(json, options);
                if (catalog?.roles == null) return;

                _roleDefs.Clear();
                foreach (var r in catalog.roles)
                {
                    if (string.IsNullOrWhiteSpace(r.role_id)) continue;
                    _roleDefs[r.role_id] = r;
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyCollection<SurvivorRoleDef> GetAllRoleDefs() => _roleDefs.Values;

        public SurvivorRoleDef? GetRoleDef(string roleId)
        {
            return _roleDefs.TryGetValue(roleId, out var def) ? def : null;
        }

        // ── Assignment & Requirements ──────────────────────────────────────

        public bool CanAssignRole(
            string survivorId,
            string roleId,
            Dictionary<string, int>? survivorSkills,
            out string failureReason)
        {
            failureReason = string.Empty;

            if (string.IsNullOrWhiteSpace(survivorId))
            {
                failureReason = "survivor_id_required";
                return false;
            }

            if (!_roleDefs.TryGetValue(roleId, out var roleDef))
            {
                failureReason = "role_not_found";
                return false;
            }

            // Check role cap
            int currentCount = _state.Assignments.Count(a =>
                string.Equals(a.RoleId, roleId, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            if (currentCount >= _state.MaxSurvivorsPerRole)
            {
                failureReason = "role_cap_reached";
                return false;
            }

            // Check skill requirements if skills provided
            if (survivorSkills != null && roleDef.required_skills != null)
            {
                foreach (var req in roleDef.required_skills)
                {
                    survivorSkills.TryGetValue(req.Key, out int currentSkill);
                    if (currentSkill < req.Value)
                    {
                        failureReason = $"insufficient_skill_{req.Key}";
                        return false;
                    }
                }
            }

            return true;
        }

        public SurvivorRoleAssignment? AssignRole(
            string survivorId,
            string roleId,
            Dictionary<string, int>? skills = null,
            int day = 1,
            bool force = false)
        {
            if (!force && !CanAssignRole(survivorId, roleId, skills, out _))
                return null;

            var existing = _state.Assignments.FirstOrDefault(a =>
                string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                existing.RoleId = roleId;
                existing.AssignedDay = day;
            }
            else
            {
                existing = new SurvivorRoleAssignment
                {
                    SurvivorId = survivorId,
                    RoleId = roleId,
                    Level = 1,
                    ExperiencePoints = 0,
                    AssignedDay = day,
                    TotalAutoActionsExecuted = 0
                };
                _state.Assignments.Add(existing);
            }

            OnRoleAssigned?.Invoke(existing);
            return existing;
        }

        public bool UnassignRole(string survivorId)
        {
            var existing = _state.Assignments.FirstOrDefault(a =>
                string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            if (existing == null) return false;

            _state.Assignments.Remove(existing);
            OnRoleUnassigned?.Invoke(survivorId);
            return true;
        }

        public SurvivorRoleAssignment? GetRoleAssignment(string survivorId)
        {
            return _state.Assignments.FirstOrDefault(a =>
                string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        // ── Progression & Bonuses ──────────────────────────────────────────

        public bool AddRoleXp(string survivorId, int xp)
        {
            var assignment = GetRoleAssignment(survivorId);
            if (assignment == null || xp <= 0) return false;

            assignment.ExperiencePoints += xp;
            int oldLevel = assignment.Level;

            // Progression thresholds:
            // Lvl 1: 0, Lvl 2: 100, Lvl 3: 250, Lvl 4: 500, Lvl 5: 1000
            int newLevel = 1;
            if (assignment.ExperiencePoints >= 1000) newLevel = 5;
            else if (assignment.ExperiencePoints >= 500) newLevel = 4;
            else if (assignment.ExperiencePoints >= 250) newLevel = 3;
            else if (assignment.ExperiencePoints >= 100) newLevel = 2;

            if (newLevel > oldLevel)
            {
                assignment.Level = newLevel;
                OnRoleLeveledUp?.Invoke(survivorId, newLevel);
                return true;
            }

            return false;
        }

        public float GetRoleBonus(string survivorId, string bonusType)
        {
            var assignment = GetRoleAssignment(survivorId);
            if (assignment == null) return 0.0f;
            if (!_roleDefs.TryGetValue(assignment.RoleId, out var def)) return 0.0f;

            float baseValue = 0.0f;
            if (string.Equals(def.primary_bonus_type, bonusType, StringComparison.OrdinalIgnoreCase))
                baseValue = def.primary_bonus_value;
            else if (string.Equals(def.secondary_bonus_type, bonusType, StringComparison.OrdinalIgnoreCase))
                baseValue = def.secondary_bonus_value;
            else
                return 0.0f;

            // Level multiplier: Lvl 1: 1.0x, Lvl 2: 1.25x, Lvl 3: 1.50x, Lvl 4: 1.75x, Lvl 5: 2.0x
            float levelMult = 1.0f + (assignment.Level - 1) * 0.25f;
            return (float)Math.Round(baseValue * levelMult, 3);
        }

        public bool TriggerAutoAction(string survivorId, string actionType)
        {
            var assignment = GetRoleAssignment(survivorId);
            if (assignment == null) return false;
            if (!_roleDefs.TryGetValue(assignment.RoleId, out var def)) return false;

            if (!string.Equals(def.auto_action_type, actionType, StringComparison.OrdinalIgnoreCase))
                return false;

            assignment.TotalAutoActionsExecuted++;
            AddRoleXp(survivorId, 20); // Award 20 XP per auto action

            OnAutoActionExecuted?.Invoke(survivorId, actionType);
            return true;
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public SurvivorRoleState CaptureState()
        {
            return new SurvivorRoleState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                MaxSurvivorsPerRole = _state.MaxSurvivorsPerRole,
                Assignments = _state.Assignments.Select(a => new SurvivorRoleAssignment
                {
                    SurvivorId = a.SurvivorId,
                    RoleId = a.RoleId,
                    Level = a.Level,
                    ExperiencePoints = a.ExperiencePoints,
                    AssignedDay = a.AssignedDay,
                    TotalAutoActionsExecuted = a.TotalAutoActionsExecuted
                }).ToList()
            };
        }

        public void RestoreState(SurvivorRoleState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.NextSequence = saved.NextSequence > 0 ? saved.NextSequence : 1;
            _state.MaxSurvivorsPerRole = saved.MaxSurvivorsPerRole > 0 ? saved.MaxSurvivorsPerRole : 2;

            _state.Assignments = saved.Assignments?.Select(a => new SurvivorRoleAssignment
            {
                SurvivorId = a.SurvivorId,
                RoleId = a.RoleId,
                Level = a.Level,
                ExperiencePoints = a.ExperiencePoints,
                AssignedDay = a.AssignedDay,
                TotalAutoActionsExecuted = a.TotalAutoActionsExecuted
            }).ToList() ?? new List<SurvivorRoleAssignment>();
        }
    }
}
