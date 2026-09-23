// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 195: Survivor Specialization Roles — Integration Tests
// Verifies survivor roles catalog loading, skill prerequisite gating, role caps,
// role assignment, level progression and bonus scaling, auto-action execution,
// and save/restore state persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan195SurvivorRoleIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsAllEightRoles()
        {
            var system = new SurvivorRoleSystem();
            string path = Path.Combine(DataDirectory, "survivor_roles.json");
            Assert.True(File.Exists(path), $"survivor_roles.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var roles = system.GetAllRoleDefs();
            Assert.Equal(8, roles.Count);

            var medic = system.GetRoleDef("role_medic");
            Assert.NotNull(medic);
            Assert.Equal("medical", medic.category);
            Assert.Equal(40, medic.required_skills["medicine"]);
            Assert.Equal(30, medic.required_skills["first_aid"]);
            Assert.Equal(0.25f, medic.primary_bonus_value);
            Assert.Equal("auto_heal", medic.auto_action_type);
        }

        [Fact]
        public void CanAssignRole_EnforcesSkillPrerequisitesAndCaps()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));

            var lowSkills = new Dictionary<string, int> { { "medicine", 20 }, { "first_aid", 10 } };
            bool canAssignLow = system.CanAssignRole("surv_low", "role_medic", lowSkills, out string reasonLow);
            Assert.False(canAssignLow);
            Assert.Equal("insufficient_skill_medicine", reasonLow);

            var highSkills = new Dictionary<string, int> { { "medicine", 50 }, { "first_aid", 40 } };
            bool canAssignHigh = system.CanAssignRole("surv_01", "role_medic", highSkills, out _);
            Assert.True(canAssignHigh);

            // Assign 2 medics (cap is 2)
            system.AssignRole("surv_01", "role_medic", highSkills);
            system.AssignRole("surv_02", "role_medic", highSkills);

            // 3rd medic fails due to role cap
            bool canAssignThird = system.CanAssignRole("surv_03", "role_medic", highSkills, out string reasonCap);
            Assert.False(canAssignThird);
            Assert.Equal("role_cap_reached", reasonCap);
        }

        [Fact]
        public void AssignRole_GrantsRoleAndCalculatesBonuses()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));

            SurvivorRoleAssignment? assigned = null;
            system.OnRoleAssigned += a => assigned = a;

            var skills = new Dictionary<string, int> { { "crafting", 50 }, { "mechanics", 40 } };
            var record = system.AssignRole("surv_eng", "role_engineer", skills, day: 3);

            Assert.NotNull(record);
            Assert.NotNull(assigned);
            Assert.Equal(1, system.ActiveAssignmentCount);
            Assert.Equal(1, record.Level);

            // Primary bonus for engineer is repair_speed (0.25)
            float repairBonus = system.GetRoleBonus("surv_eng", "repair_speed");
            Assert.Equal(0.25f, repairBonus);

            // Unrelated bonus returns 0
            float combatBonus = system.GetRoleBonus("surv_eng", "combat_damage");
            Assert.Equal(0.0f, combatBonus);
        }

        [Fact]
        public void AddRoleXp_LevelsUpRoleAndEnhancesBonuses()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));

            system.AssignRole("surv_scout", "role_scout", force: true);

            int leveledUpLevel = 0;
            system.OnRoleLeveledUp += (s, lvl) => leveledUpLevel = lvl;

            // 300 XP qualifies for Level 3 (Journeyman, threshold 250)
            bool leveled = system.AddRoleXp("surv_scout", 300);

            Assert.True(leveled);
            Assert.Equal(3, leveledUpLevel);

            var assignment = system.GetRoleAssignment("surv_scout");
            Assert.NotNull(assignment);
            Assert.Equal(3, assignment.Level);

            // Level 3 multiplier is 1.0 + (3-1)*0.25 = 1.50x
            // Base expedition_speed is 0.25 -> 0.25 * 1.5 = 0.375
            float bonus = system.GetRoleBonus("surv_scout", "expedition_speed");
            Assert.Equal(0.375f, bonus);
        }

        [Fact]
        public void TriggerAutoAction_ExecutesAndTracksPerformance()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));

            system.AssignRole("surv_doc", "role_medic", force: true);

            string? autoActionDone = null;
            system.OnAutoActionExecuted += (s, act) => autoActionDone = act;

            bool triggered = system.TriggerAutoAction("surv_doc", "auto_heal");

            Assert.True(triggered);
            Assert.Equal("auto_heal", autoActionDone);

            var a = system.GetRoleAssignment("surv_doc");
            Assert.NotNull(a);
            Assert.Equal(1, a.TotalAutoActionsExecuted);
            Assert.Equal(20, a.ExperiencePoints); // Awarded 20 XP
        }

        [Fact]
        public void SaveRestoreState_PreservesAssignmentsAndProgression()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));

            system.AssignRole("surv_leader", "role_leader", force: true, day: 5);
            system.AddRoleXp("surv_leader", 600); // Reaches Level 4

            var state = system.CaptureState();

            var restoredSystem = new SurvivorRoleSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));
            restoredSystem.RestoreState(state);

            Assert.Equal(1, restoredSystem.ActiveAssignmentCount);
            var a = restoredSystem.GetRoleAssignment("surv_leader");
            Assert.NotNull(a);
            Assert.Equal("role_leader", a.RoleId);
            Assert.Equal(4, a.Level);
            Assert.Equal(600, a.ExperiencePoints);
        }
    }
}
